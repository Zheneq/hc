using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FogOfWar : MonoBehaviour
{
	private struct VisibleSquareEntry
	{
		public int m_visibleFlags;
	}

	private bool m_updateVisibility;
	private bool m_updatedVisibilityThisFrame;
	private bool m_visibilityPersonalOnly;
	private PlayerData m_ownerPlayer;
	private ActorData m_owner;
	private Dictionary<BoardSquare, VisibleSquareEntry> m_visibleSquares;
	private float m_lastRecalcTime;

	public static FogOfWar GetClientFog()
	{
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				return GameFlowData.Get().activeOwnedActorData.GetFogOfWar();
			}
			if (GameFlowData.Get().LocalPlayerData != null)
			{
				return GameFlowData.Get().LocalPlayerData.GetFogOfWar();
			}
		}
		return null;
	}

	private void Awake()
	{
		m_visibleSquares = new Dictionary<BoardSquare, VisibleSquareEntry>();
		m_ownerPlayer = GetComponent<PlayerData>();
		m_owner = GetComponent<ActorData>();
	}

	private bool ShouldUpdateMyVisibility()
	{
		if (m_updateVisibility)
		{
			if (NetworkServer.active)
			{
				return true;
			}
			if (GameFlowData.Get().LocalPlayerData != null)
			{
				Team localTeamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
				Team ownerTeamViewing = m_ownerPlayer.GetTeamViewing();
				if ((localTeamViewing == Team.Invalid || localTeamViewing == ownerTeamViewing)
					&& m_ownerPlayer.PlayerIndex != PlayerData.s_invalidPlayerIndex)
				{
					return true;
				}
			}
		}
		return m_visibilityPersonalOnly != InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowPersonalVisibility);
	}

	private bool ShouldUpdateClientActorVisibility()
	{
		if (GameFlowData.Get().LocalPlayerData == null ||
			GameFlowData.Get().LocalPlayerData.GetFogOfWar() == null)
		{
			return false;
		}
		
		Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
		bool flag = m_owner == null || teamViewing == Team.Invalid || teamViewing == m_owner.GetTeam();
		bool ownerPlayerIsNotLocal = m_ownerPlayer != GameFlowData.Get().LocalPlayerData;
		bool notYetUpdatedThisFrame = !GetClientFog().m_updatedVisibilityThisFrame;
		return m_updateVisibility && flag && ownerPlayerIsNotLocal && notYetUpdatedThisFrame;
	}

	private void Update()
	{
		m_updatedVisibilityThisFrame = false;
		if (ShouldUpdateMyVisibility())
		{
			UpdateVisibilityOfSquares();
			m_updatedVisibilityThisFrame = true;
		}
		if (ShouldUpdateClientActorVisibility())
		{
			GetClientFog().MarkForRecalculateVisibility();
		}
		m_updateVisibility = false;
	}

	private void CalcVisibleSquares(
		BoardSquare center,
		float radius,
		bool radiusAsStraightLineDist,
		BoardSquare.VisibilityFlags flag,
		bool ignoreLOS,
		VisionProviderInfo.BrushRevealType brushRevealType = VisionProviderInfo.BrushRevealType.BaseOnCenterPosition)
	{
		if (center == null || radius <= 0f)
		{
			return;
		}

		radius = Mathf.Max(radius, 0f);
		int centerX = center.GetGridPos().x;
		int centerY = center.GetGridPos().y;
		int minX = Mathf.Max(Mathf.FloorToInt(centerX - radius), 0);
		int maxX = Mathf.Min(Mathf.CeilToInt(centerX + radius), Board.Get().GetMaxX() - 1);
		int minY = Mathf.Max(Mathf.FloorToInt(centerY - radius), 0);
		int maxY = Mathf.Min(Mathf.CeilToInt(centerY + radius), Board.Get().GetMaxY() - 1);
		bool isSeeingThroughBrush = m_owner != null && m_owner.GetActorStatus().HasStatus(StatusType.SeeThroughBrush);
		for (int x = minX; x <= maxX; x++)
		{
			for (int y = minY; y <= maxY; y++)
			{
				BoardSquare square = Board.Get().GetSquareFromIndex(x, y);
				if (square != null
					&& (!m_visibleSquares.TryGetValue(square, out VisibleSquareEntry value)
						|| GetVisTypePriority(flag) > GetHighestVisTypePriority(value.m_visibleFlags)))
				{
					float distance = radiusAsStraightLineDist
						? square.HorizontalDistanceInSquaresTo(center)
						: CalcHorizontalDistanceOnBoardTo(centerX, centerY, x, y);
					if (distance <= radius && (ignoreLOS || center.GetLOS(x, y)))
					{
						bool isSquareHidden;
						if ((!m_owner || !isSeeingThroughBrush) && BrushCoordinator.Get() != null)
						{
							if (brushRevealType == VisionProviderInfo.BrushRevealType.Never && square.BrushRegion >= 0)
							{
								isSquareHidden = BrushCoordinator.Get().IsRegionFunctioning(square.BrushRegion);
							}
							else if (brushRevealType == VisionProviderInfo.BrushRevealType.Always && square.BrushRegion >= 0)
							{
								isSquareHidden = false;
							}
							else
							{
								isSquareHidden = BrushCoordinator.Get().IsSquareHiddenFrom(square, center);
							}
						}
						else
						{
							isSquareHidden = false;
						}
						bool isVisionBlocked = m_owner
						                       && BarrierManager.Get() != null
						                       && BarrierManager.Get().IsVisionBlocked(m_owner, center, square);
						if (!isSquareHidden && !isVisionBlocked)
						{
							value.m_visibleFlags |= (int)flag;
							m_visibleSquares[square] = value;
						}
					}
				}
			}
		}
	}

	private void AppendToVisibleSquares(List<BoardSquare> squares, BoardSquare.VisibilityFlags flag, bool seeThroughBrush)
	{
		foreach (BoardSquare square in squares)
		{
			if (square != null)
			{
				bool isSquareHidden = false;
				if (!seeThroughBrush && BrushCoordinator.Get() != null && square.BrushRegion > 0)
				{
					isSquareHidden = BrushCoordinator.Get().IsRegionFunctioning(square.BrushRegion);
				}
				if (!isSquareHidden)
				{
					m_visibleSquares.TryGetValue(square, out VisibleSquareEntry value);
					value.m_visibleFlags |= (int)flag;
					m_visibleSquares[square] = value;
				}
			}
		}
	}

	private int GetVisTypePriority(BoardSquare.VisibilityFlags visType)
	{
		switch (visType)
		{
			case BoardSquare.VisibilityFlags.Revealed:
				return 4;
			case BoardSquare.VisibilityFlags.Objective:
				return 3;
			case BoardSquare.VisibilityFlags.Self:
				return 2;
			case BoardSquare.VisibilityFlags.Team:
				return 1;
		}
		return 0;
	}

	private int GetHighestVisTypePriority(int flags)
	{
		if ((flags & (int)BoardSquare.VisibilityFlags.Revealed) != 0)
		{
			return 4;
		}
		if ((flags & (int)BoardSquare.VisibilityFlags.Objective) != 0)
		{
			return 3;
		}
		if ((flags & (int)BoardSquare.VisibilityFlags.Self) != 0)
		{
			return 2;
		}
		if ((flags & (int)BoardSquare.VisibilityFlags.Team) != 0)
		{
			return 1;
		}
		return 0;
	}

	private float CalcHorizontalDistanceOnBoardTo(int firstX, int firstY, int secondX, int secondY)
	{
		int xDist = Mathf.Abs(firstX - secondX);
		int yDist = Mathf.Abs(firstY - secondY);
		if (xDist > yDist)
		{
			return (xDist - yDist) + yDist * 1.5f;
		}
		else
		{
			return (yDist - xDist) + xDist * 1.5f;
		}
	}

	public bool IsVisible(BoardSquare square)
	{
		if (!NetworkServer.active && m_owner != GameFlowData.Get().activeOwnedActorData)
		{
			Log.Warning("Calling FogOfWar::IsVisible(BoardSquare square) on a client for not-the-client actor.");
		}
		if (square != null)
		{
			return m_visibleSquares.ContainsKey(square);
		}
		return false;
	}

	public bool IsVisibleBySelf(BoardSquare square)
	{
		return square != null &&
			m_visibleSquares.ContainsKey(square) &&
			(m_visibleSquares[square].m_visibleFlags & 1) != 0;
	}

	public bool IsVisibleBySelf(ActorData otherActor)
	{
		if (otherActor != null)
		{
			BoardSquare travelBoardSquare = otherActor.GetTravelBoardSquare();
			return travelBoardSquare != null && IsVisibleBySelf(travelBoardSquare);
		}
		return false;
	}

	internal Bounds CalcVisibleBySelfBounds()
	{
		Bounds result = default(Bounds);
		bool hasBounds = false;
		foreach (KeyValuePair<BoardSquare, VisibleSquareEntry> square in m_visibleSquares)
		{
			if ((square.Value.m_visibleFlags & 1) != 0)
			{
				if (hasBounds)
				{
					result.Encapsulate(square.Key.CameraBounds);
				}
				else
				{
					result = square.Key.CameraBounds;
					hasBounds = true;
				}
			}
		}
		return result;
	}

	public static void CalculateFogOfWarForTeam(Team team)
	{
		if (GameFlowData.Get() == null)
		{
			Debug.LogError("Trying to calculate fog of war, but GameFlowData does not exist.");
			return;
		}
		foreach (ActorData actor in GameFlowData.Get().GetActors())
		{
			if (actor.GetTeam() == team)
			{
				FogOfWar fogOfWar = actor.GetFogOfWar();
				if (fogOfWar != null)
				{
					fogOfWar.MarkForRecalculateVisibility();
				}
			}
		}
	}

	public void MarkForRecalculateVisibility()
	{
		m_updateVisibility = true;
	}

	public void UpdateVisibilityOfSquares(bool updateShade = true)
	{
		m_visibleSquares.Clear();
		m_visibilityPersonalOnly = InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowPersonalVisibility);
		if (m_owner != null)
		{
			bool isDisabled = m_owner.IsDead() || NetworkClient.active && m_owner.IsInRagdoll();
			float radius = isDisabled ? 0f : m_owner.GetSightRange();
			CalcVisibleSquares(m_owner.GetTravelBoardSquare(), radius, false, BoardSquare.VisibilityFlags.Self, false);
		}
		bool isInGlobalBlind = GameplayMutators.IsStatusActive(StatusType.Blind, GameFlowData.Get().CurrentTurn);
		if (!m_visibilityPersonalOnly)
		{
			if (m_owner != null)
			{
				ActorAdditionalVisionProviders actorAdditionalVisionProviders = m_owner.GetAdditionalActorVisionProviders();
				if (actorAdditionalVisionProviders != null)
				{
					foreach (VisionProviderInfo visionProvider in actorAdditionalVisionProviders.GetVisionProviders())
					{
						if ((!isInGlobalBlind || visionProvider.m_canFunctionInGlobalBlind)
							&& visionProvider.GetBoardSquare() != null)
						{
							CalcVisibleSquares(
								visionProvider.GetBoardSquare(),
								visionProvider.m_radius,
								visionProvider.m_radiusAsStraightLineDist,
								visionProvider.m_flag,
								visionProvider.m_ignoreLos,
								visionProvider.m_brushRevealType);
						}
					}
				}
				List<ControlPoint> allControlPoints = ControlPoint.GetAllControlPoints();
				if (allControlPoints != null)
				{
					foreach (ControlPoint controlPoint in allControlPoints)
					{
						if (controlPoint.IsGrantingVisionForTeam(m_owner.GetTeam()))
						{
							AppendToVisibleSquares(controlPoint.GetSquaresForVision(), BoardSquare.VisibilityFlags.Team, controlPoint.m_visionSeeThroughBrush);
						}
					}
				}
			}
			Team team = m_owner ? m_owner.GetTeam() : m_ownerPlayer.GetTeamViewing();
			ActorStatus actorStatus = m_owner?.GetActorStatus() ?? null;
			List<ActorData> teammates;
			List<ActorData> opponents;
			if (team != Team.Invalid)
			{
				teammates = GameFlowData.Get().GetAllTeamMembers(team);
				opponents = GameFlowData.Get().GetAllTeamMembers(team.OtherTeam());
			}
			else
			{
				teammates = new List<ActorData>(GameFlowData.Get().GetAllTeamMembers(Team.TeamA));
				teammates.AddRange(GameFlowData.Get().GetAllTeamMembers(Team.TeamB));
				opponents = new List<ActorData>();
			}
			bool hasAllyVision = !actorStatus || !actorStatus.HasStatus(StatusType.LoseAllyVision);
			if (hasAllyVision)
			{
				foreach (ActorData teammate in teammates)
				{
					if (teammate != null
						&& (m_owner == null || m_owner != teammate)
						&& teammate.GetTravelBoardSquare() != null
						&& !teammate.GetActorStatus().HasStatus(StatusType.IsolateVisionFromAllies))
					{
						bool isDisabled = teammate.IsDead() || NetworkClient.active && teammate.IsInRagdoll();
						float radius = isDisabled ? 0f : teammate.GetSightRange();
						CalcVisibleSquares(teammate.GetTravelBoardSquare(), radius, false, BoardSquare.VisibilityFlags.Team, false);
						ActorAdditionalVisionProviders component = teammate.GetComponent<ActorAdditionalVisionProviders>();
						if (component != null)
						{
							foreach (VisionProviderInfo visionProvider in component.GetVisionProviders())
							{
								if ((!isInGlobalBlind || visionProvider.m_canFunctionInGlobalBlind)
									&& visionProvider.GetBoardSquare() != null)
								{
									CalcVisibleSquares(
										visionProvider.GetBoardSquare(),
										visionProvider.m_radius,
										visionProvider.m_radiusAsStraightLineDist,
										visionProvider.m_flag,
										visionProvider.m_ignoreLos,
										visionProvider.m_brushRevealType);
								}
							}
						}
					}
				}
			}
			foreach (ActorData opponent in opponents)
			{
				if (opponent != null)
				{
					BoardSquare travelBoardSquare = opponent.GetTravelBoardSquare();
					bool isRevealed = opponent.GetActorStatus()?.HasStatus(StatusType.Revealed) ?? false;
					bool isRevealedByFlagOnClient = !NetworkServer.active && CaptureTheFlag.IsActorRevealedByFlag_Client(opponent);
					if (travelBoardSquare && (isRevealed || isRevealedByFlagOnClient))
					{
						CalcVisibleSquares(opponent.GetTravelBoardSquare(), 0.1f, false, BoardSquare.VisibilityFlags.Revealed, true);
					}
					if (travelBoardSquare && m_owner && m_owner.IsLineOfSightVisibleException(opponent))
					{
						m_visibleSquares.TryGetValue(travelBoardSquare, out VisibleSquareEntry value);
						value.m_visibleFlags |= (int)BoardSquare.VisibilityFlags.Revealed;
						m_visibleSquares[travelBoardSquare] = value;
					}
				}
			}
		}
		if (updateShade)
		{
			SetVisibleShadeOfAllSquares();
		}
		m_lastRecalcTime = Time.time;
	}

	private bool IsClientFog()
	{
		if (GameFlowData.Get().activeOwnedActorData != null)
		{
			return GameFlowData.Get().activeOwnedActorData == m_owner;
		}
		return m_ownerPlayer == GameFlowData.Get().LocalPlayerData;
	}

	public void SetVisibleShadeOfAllSquares()
	{
		if (!IsClientFog())
		{
			return;
		}
		bool anySquareShadeChanged = false;
		for (int x = 0; x < Board.Get().GetMaxX(); x++)
		{
			for (int y = 0; y < Board.Get().GetMaxY(); y++)
			{
				BoardSquare square = Board.Get().GetSquareFromIndex(x, y);
				if (square != null)
				{
					if (m_visibleSquares.ContainsKey(square))
					{
						square.SetVisibleShade(m_visibleSquares[square].m_visibleFlags, ref anySquareShadeChanged);
					}
					else
					{
						square.SetVisibleShade(0, ref anySquareShadeChanged);
					}
				}
				else
				{
					Log.Info($"Trying to set visible shade of square ({x}, {y}), but it's out of bounds");
				}
			}
		}
		if (anySquareShadeChanged || GameEventManager.Get() != null)
		{
			GameEventManager.Get().FireEvent(GameEventManager.EventType.BoardSquareVisibleShadeChanged, null);
		}
	}

	private void OnDrawGizmos()
	{
		if (CameraManager.ShouldDrawGizmosForCurrentCamera()
			&& GameFlowData.Get() != null
			&& m_owner != null
			&& m_owner == GameFlowData.Get().activeOwnedActorData)
		{
			foreach (KeyValuePair<BoardSquare, VisibleSquareEntry> current in m_visibleSquares)
			{
				Gizmos.color = (current.Value.m_visibleFlags & 1) != 0 ? Color.gray : Color.blue;
				Gizmos.DrawWireCube(current.Key.CameraBounds.center, current.Key.CameraBounds.size * 0.7f);
			}
			Gizmos.color = Color.white;
			float num = 1f - Mathf.Clamp((Time.time - m_lastRecalcTime) * 2f, 0f, 1f);
			Gizmos.color = new Color(0f, num, num, num);
			Gizmos.DrawSphere(m_owner.GetOverheadPosition(10f), 0.5f);
		}
	}
}
