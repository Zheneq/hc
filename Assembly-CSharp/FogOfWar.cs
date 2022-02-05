// ROGUES
// SERVER
//using System;
using System.Collections.Generic;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

public class FogOfWar : MonoBehaviour
{
	// removed in rogues
	private struct VisibleSquareEntry
	{
		public int m_visibleFlags;
	}

	private bool m_updateVisibility;
	private bool m_updatedVisibilityThisFrame;
	private bool m_visibilityPersonalOnly;
	private PlayerData m_ownerPlayer;
	private ActorData m_owner;
	private Dictionary<BoardSquare, VisibleSquareEntry> m_visibleSquares; // replaced with private HashSet<BoardSquare> m_hiddenSquares; in rogues
	private float m_lastRecalcTime;

	public static FogOfWar GetClientFog()
	{
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				return GameFlowData.Get().activeOwnedActorData.GetFogOfWar();
			}
			else if (GameFlowData.Get().LocalPlayerData != null)
			{
				return GameFlowData.Get().LocalPlayerData.GetFogOfWar();
			}
		}
		return null;
	}

	private void Awake()
	{
		// reactor
		m_visibleSquares = new Dictionary<BoardSquare, VisibleSquareEntry>();
		// rogues
		//m_hiddenSquares = new HashSet<BoardSquare>();

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
			else if (GameFlowData.Get().LocalPlayerData != null)
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
		bool radiusAsStraightLineDist,  // removed in rogues
		BoardSquare.VisibilityFlags flag,
		bool ignoreLOS,  // removed in rogues
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
		bool isSeeingThrougBrush = m_owner?.GetActorStatus().HasStatus(StatusType.SeeThroughBrush) ?? false;
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
						if ((!m_owner || !isSeeingThrougBrush) && BrushCoordinator.Get() != null)
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
						bool isVisionBlocked = m_owner && (BarrierManager.Get()?.IsVisionBlocked(m_owner, center, square) ?? false);
						if (!isSquareHidden && !isVisionBlocked)
						{
							value.m_visibleFlags |= (int)flag;
							m_visibleSquares[square] = value;
						}
					}
				}
			}
		}

		// rogues
		//ActorData owner = m_owner;
		//Board.Get();
		//if (!(owner != null) || !owner.GetActorStatus().HasStatus(StatusType.SeeThroughBrush, true))
		//{
		//	BrushCoordinator brushCoordinator = BrushCoordinator.Get();
		//	if (brushCoordinator)
		//	{
		//		for (int i = 0; i < brushCoordinator.m_regions.Length; i++)
		//		{
		//			if (brushCoordinator.IsRegionFunctioning(i))
		//			{
		//				foreach (BoardSquare boardSquare in brushCoordinator.m_regions[i].GetSquaresInRegion())
		//				{
		//					if (brushRevealType == VisionProviderInfo.BrushRevealType.Never)
		//					{
		//						m_hiddenSquares.Add(boardSquare);
		//					}
		//					else if (brushRevealType == VisionProviderInfo.BrushRevealType.BaseOnCenterPosition)
		//					{
		//						if (BrushCoordinator.Get().IsSquareHiddenFrom(boardSquare, center))
		//						{
		//							m_hiddenSquares.Add(boardSquare);
		//						}
		//					}
		//					else if (brushRevealType == VisionProviderInfo.BrushRevealType.Always && boardSquare.HorizontalDistanceInSquaresTo(center) > radius)
		//					{
		//						m_hiddenSquares.Add(boardSquare);
		//					}
		//				}
		//			}
		//		}
		//	}
		//}
	}

	private void AppendToVisibleSquares(List<BoardSquare> squares, BoardSquare.VisibilityFlags flag, bool seeThroughBrush)  // flag & seeThroughBrush removed in rogues
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

		// rogues
		//for (int i = 0; i < squares.Count; i++)
		//{
		//	BoardSquare boardSquare = squares[i];
		//	if (boardSquare != null)
		//	{
		//		m_hiddenSquares.Remove(boardSquare);
		//	}
		//}
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
		// reactor
		if (square != null)
		{
			return m_visibleSquares.ContainsKey(square);
		}
		return false;
		// rogues
		// return !(square == null) && !m_hiddenSquares.Contains(square);
	}

	public bool IsVisibleBySelf(BoardSquare square)
	{
#if SERVER
		if (!NetworkServer.active && m_owner != GameFlowData.Get().activeOwnedActorData) // warning added in rogues
		{
			Log.Warning("Calling FogOfWar::IsVisibleBySelf(BoardSquare square) on a client for not-the-client actor.");
		}
#endif
		
		return square != null &&
			m_visibleSquares.ContainsKey(square) &&
			(m_visibleSquares[square].m_visibleFlags & 1) != 0;
		//return square != null && !m_hiddenSquares.Contains(square); // rogues
    }

    public bool IsVisibleBySelf(ActorData otherActor)
	{
#if SERVER
		if (!NetworkServer.active && m_owner != GameFlowData.Get().activeOwnedActorData) // warning added in rogues
		{
			Log.Warning("Calling FogOfWar::IsVisibleBySelf(ActorData otherActor) on a client for not-the-client actor.");
		}
#endif
		if (otherActor != null)
		{
			BoardSquare travelBoardSquare = otherActor.GetTravelBoardSquare();
			return travelBoardSquare != null && IsVisibleBySelf(travelBoardSquare);
		}
		return false;
	}

	// removed in rogues
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
		// reactor
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
		// rogues
		//foreach (ActorData actor in GameFlowData.Get().GetPlayerAndBotTeamMembers(team))
		//{
		//	FogOfWar fogOfWar = actor.GetFogOfWar();
		//	if (fogOfWar != null)
		//	{
		//		fogOfWar.MarkForRecalculateVisibility();
		//	}
		//}
	}

	// rogues or server-only?
#if SERVER
	public static void ImmediateUpdateVisibilityForTeam(Team team)
    {
        if (GameFlowData.Get() == null)
        {
            Debug.LogError("Trying to calculate fog of war, but GameFlowData does not exist.");
            return;
        }
        foreach (ActorData actorData in GameFlowData.Get().GetPlayerAndBotTeamMembers(team))
        {
            FogOfWar fogOfWar = actorData.GetFogOfWar();
            if (fogOfWar != null)
            {
                fogOfWar.ImmediateUpdateVisibilityOfSquares();
            }
        }
    }
#endif

    public void MarkForRecalculateVisibility()
	{
		m_updateVisibility = true;
	}

	public void UpdateVisibilityOfSquares(bool updateShade = true)  // reworked in rogues
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

	// rogues
	//public void UpdateVisibilityOfSquares(bool updateShade = true)
	//{
	//	m_hiddenSquares.Clear();
	//	m_visibilityPersonalOnly = InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowPersonalVisibility);
	//	if (m_owner)
	//	{
	//		bool flag = m_owner.IsDead();
	//		if (!flag && NetworkClient.active)
	//		{
	//			flag = m_owner.IsInRagdoll();
	//		}
	//		float radius = flag ? 0f : m_owner.GetSightRange();
	//		CalcVisibleSquares(m_owner.GetTravelBoardSquare(), radius, BoardSquare.VisibilityFlags.Self, VisionProviderInfo.BrushRevealType.BaseOnCenterPosition);
	//	}
	//	if (!m_visibilityPersonalOnly)
	//	{
	//		if (m_owner)
	//		{
	//			ActorAdditionalVisionProviders additionalActorVisionProviders = m_owner.GetAdditionalActorVisionProviders();
	//			if (additionalActorVisionProviders != null)
	//			{
	//				SyncListVisionProviderInfo visionProviders = additionalActorVisionProviders.GetVisionProviders();
	//				for (int i = 0; i < visionProviders.Count; i++)
	//				{
	//					if (visionProviders[i].GetBoardSquare() != null)
	//					{
	//						CalcVisibleSquares(
	//                               visionProviders[i].GetBoardSquare(),
	//                               visionProviders[i].m_radius,
	//                               visionProviders[i].m_flag,
	//                               visionProviders[i].m_brushRevealType);
	//					}
	//				}
	//			}
	//			List<ControlPoint> allControlPoints = ControlPoint.GetAllControlPoints();
	//			if (allControlPoints != null)
	//			{
	//				for (int j = 0; j < allControlPoints.Count; j++)
	//				{
	//					ControlPoint controlPoint = allControlPoints[j];
	//					if (controlPoint.IsGrantingVisionForTeam(m_owner.GetTeam()))
	//					{
	//						AppendToVisibleSquares(controlPoint.GetRegion().GetSquaresInRegion());
	//					}
	//				}
	//			}
	//		}
	//		Team teamViewing = m_ownerPlayer.GetTeamViewing();
	//		ActorStatus actorStatus = m_owner ? m_owner.GetActorStatus() : null;
	//		List<ActorData> list;
	//		List<ActorData> list2;
	//		if (teamViewing != Team.Invalid)
	//		{
	//			list = GameFlowData.Get().GetAllTeamMembers(teamViewing);
	//			list2 = GameFlowData.Get().GetAllTeamMembers(teamViewing.OtherTeam());
	//		}
	//		else
	//		{
	//			list = new List<ActorData>(GameFlowData.Get().GetAllTeamMembers(Team.TeamA));
	//			list.AddRange(GameFlowData.Get().GetAllTeamMembers(Team.TeamB));
	//			list2 = new List<ActorData>();
	//		}
	//		bool flag2 = true;
	//		if (actorStatus && actorStatus.HasStatus(StatusType.LoseAllyVision, true))
	//		{
	//			flag2 = false;
	//		}
	//		if (flag2)
	//		{
	//			foreach (ActorData actorData in list)
	//			{
	//				if (actorData && (m_owner == null || m_owner != actorData) && actorData.GetTravelBoardSquare() && !actorData.GetActorStatus().HasStatus(StatusType.IsolateVisionFromAllies, true))
	//				{
	//					bool flag3 = actorData.IsDead();
	//					if (!flag3 && NetworkClient.active)
	//					{
	//						flag3 = actorData.IsInRagdoll();
	//					}
	//					float radius2 = flag3 ? 0f : actorData.GetSightRange();
	//					CalcVisibleSquares(actorData.GetTravelBoardSquare(), radius2, BoardSquare.VisibilityFlags.Team, VisionProviderInfo.BrushRevealType.BaseOnCenterPosition);
	//					ActorAdditionalVisionProviders component = actorData.GetComponent<ActorAdditionalVisionProviders>();
	//					if (component != null)
	//					{
	//						SyncListVisionProviderInfo visionProviders2 = component.GetVisionProviders();
	//						for (int k = 0; k < visionProviders2.Count; k++)
	//						{
	//							if (visionProviders2[k].GetBoardSquare() != null)
	//							{
	//								CalcVisibleSquares(visionProviders2[k].GetBoardSquare(), visionProviders2[k].m_radius, visionProviders2[k].m_flag, visionProviders2[k].m_brushRevealType);
	//							}
	//						}
	//					}
	//				}
	//			}
	//		}
	//		foreach (ActorData actorData2 in list2)
	//		{
	//			if (actorData2 != null)
	//			{
	//				BoardSquare travelBoardSquare = actorData2.GetTravelBoardSquare();
	//				ActorStatus actorStatus2 = actorData2.GetActorStatus();
	//				bool flag4 = actorStatus2 && actorStatus2.HasStatus(StatusType.Revealed, true);
	//				bool flag5 = false;
	//				if (!NetworkServer.active && CaptureTheFlag.IsActorRevealedByFlag_Client(actorData2))
	//				{
	//					flag5 = true;
	//				}
	//				if (NetworkServer.active && CaptureTheFlag.IsActorRevealedByFlag_Server(actorData2))
	//				{
	//					flag5 = true;
	//				}
	//				if (travelBoardSquare && (flag4 || flag5))
	//				{
	//					CalcVisibleSquares(actorData2.GetTravelBoardSquare(), 0.1f, BoardSquare.VisibilityFlags.Revealed, VisionProviderInfo.BrushRevealType.BaseOnCenterPosition);
	//				}
	//				if (travelBoardSquare && m_owner && m_owner.IsLineOfSightVisibleException(actorData2))
	//				{
	//					m_hiddenSquares.Remove(travelBoardSquare);
	//				}
	//			}
	//		}
	//	}
	//	m_lastRecalcTime = Time.time;
	//}

	private bool IsClientFog()
	{
		if (GameFlowData.Get().activeOwnedActorData != null)
		{
			return GameFlowData.Get().activeOwnedActorData == m_owner;
		}
		return m_ownerPlayer == GameFlowData.Get().LocalPlayerData;
	}

	// rogues or server-only?
#if SERVER
	public void ImmediateUpdateVisibilityOfSquares()
	{
		UpdateVisibilityOfSquares(false);
		m_updateVisibility = false;
	}
#endif


	// removed in rogues
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
