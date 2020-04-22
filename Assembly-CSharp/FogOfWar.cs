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
		FogOfWar result = null;
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				result = GameFlowData.Get().activeOwnedActorData.GetFogOfWar();
			}
			else if (GameFlowData.Get().LocalPlayerData != null)
			{
				result = GameFlowData.Get().LocalPlayerData.GetFogOfWar();
			}
		}
		return result;
	}

	private void Awake()
	{
		m_visibleSquares = new Dictionary<BoardSquare, VisibleSquareEntry>();
		m_ownerPlayer = GetComponent<PlayerData>();
		m_owner = GetComponent<ActorData>();
	}

	private bool ShouldUpdateMyVisibility()
	{
		bool result = false;
		if (m_updateVisibility)
		{
			if (NetworkServer.active)
			{
				result = true;
			}
			else if (GameFlowData.Get().LocalPlayerData != null)
			{
				Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
				Team teamViewing2 = m_ownerPlayer.GetTeamViewing();
				if (teamViewing != Team.Invalid)
				{
					if (teamViewing != teamViewing2)
					{
						goto IL_0097;
					}
				}
				if (m_ownerPlayer.PlayerIndex != PlayerData.s_invalidPlayerIndex)
				{
					result = true;
				}
			}
		}
		goto IL_0097;
		IL_0097:
		if (m_visibilityPersonalOnly != InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowPersonalVisibility))
		{
			result = true;
		}
		return result;
	}

	private bool ShouldUpdateClientActorVisibility()
	{
		bool result = false;
		if (GameFlowData.Get().LocalPlayerData != null)
		{
			if (GameFlowData.Get().LocalPlayerData.GetFogOfWar() != null)
			{
				Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
				bool updateVisibility = m_updateVisibility;
				int num;
				if (!(m_owner == null) && teamViewing != Team.Invalid)
				{
					num = ((teamViewing == m_owner.GetTeam()) ? 1 : 0);
				}
				else
				{
					num = 1;
				}
				bool flag = (byte)num != 0;
				bool flag2 = m_ownerPlayer != GameFlowData.Get().LocalPlayerData;
				bool flag3 = !GetClientFog().m_updatedVisibilityThisFrame;
				if (updateVisibility)
				{
					if (flag)
					{
						if (flag2 && flag3)
						{
							result = true;
						}
					}
				}
			}
		}
		return result;
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

	private void CalcVisibleSquares(BoardSquare center, float radius, bool radiusAsStraightLineDist, BoardSquare.VisibilityFlags flag, bool ignoreLOS, VisionProviderInfo.BrushRevealType brushRevealType = VisionProviderInfo.BrushRevealType.BaseOnCenterPosition)
	{
		if (!(center != null))
		{
			return;
		}
		while (true)
		{
			if (!(radius > 0f))
			{
				return;
			}
			ActorData owner = m_owner;
			Board board = Board.Get();
			float num = Mathf.Max(radius, 0f);
			int x = center.GetGridPos().x;
			int y = center.GetGridPos().y;
			int num2 = Mathf.Max(Mathf.FloorToInt((float)x - num), 0);
			int num3 = Mathf.Min(Mathf.CeilToInt((float)x + num), board.GetMaxX() - 1);
			int num4 = Mathf.Max(Mathf.FloorToInt((float)y - num), 0);
			int num5 = Mathf.Min(Mathf.CeilToInt((float)y + num), board.GetMaxY() - 1);
			int num6;
			if (owner != null)
			{
				num6 = (owner.GetActorStatus().HasStatus(StatusType.SeeThroughBrush) ? 1 : 0);
			}
			else
			{
				num6 = 0;
			}
			bool flag2 = (byte)num6 != 0;
			for (int i = num2; i <= num3; i++)
			{
				for (int j = num4; j <= num5; j++)
				{
					BoardSquare boardSquare = board.GetBoardSquare(i, j);
					if (!(boardSquare != null))
					{
						continue;
					}
					if (m_visibleSquares.TryGetValue(boardSquare, out VisibleSquareEntry value))
					{
						if (GetVisTypePriority(flag) <= GetHighestVisTypePriority(value.m_visibleFlags))
						{
							continue;
						}
					}
					float num7;
					if (radiusAsStraightLineDist)
					{
						num7 = boardSquare.HorizontalDistanceInSquaresTo(center);
					}
					else
					{
						num7 = CalcHorizontalDistanceOnBoardTo(x, y, i, j);
					}
					float num8 = num7;
					if (!(num8 <= num))
					{
						continue;
					}
					if (!ignoreLOS)
					{
						if (!center._0013(i, j))
						{
							continue;
						}
					}
					if ((bool)owner)
					{
						if (flag2)
						{
							goto IL_0204;
						}
					}
					if (BrushCoordinator.Get() == null)
					{
						goto IL_0204;
					}
					bool flag3;
					if (brushRevealType == VisionProviderInfo.BrushRevealType.Never)
					{
						if (boardSquare.BrushRegion >= 0)
						{
							flag3 = BrushCoordinator.Get().IsRegionFunctioning(boardSquare.BrushRegion);
							goto IL_0286;
						}
					}
					if (brushRevealType == VisionProviderInfo.BrushRevealType.Always)
					{
						if (boardSquare.BrushRegion >= 0)
						{
							flag3 = false;
							goto IL_0286;
						}
					}
					flag3 = BrushCoordinator.Get().IsSquareHiddenFrom(boardSquare, center);
					goto IL_0286;
					IL_0204:
					flag3 = false;
					goto IL_0286;
					IL_0286:
					bool flag4;
					if (BarrierManager.Get() != null)
					{
						int num9;
						if ((bool)owner)
						{
							num9 = (BarrierManager.Get().IsVisionBlocked(owner, center, boardSquare) ? 1 : 0);
						}
						else
						{
							num9 = 0;
						}
						flag4 = ((byte)num9 != 0);
					}
					else
					{
						flag4 = false;
					}
					if (flag3)
					{
						continue;
					}
					if (flag4)
					{
					}
					else
					{
						value.m_visibleFlags |= (int)flag;
						m_visibleSquares[boardSquare] = value;
					}
				}
			}
			return;
		}
	}

	private void AppendToVisibleSquares(List<BoardSquare> squares, BoardSquare.VisibilityFlags flag, bool seeThroughBrush)
	{
		for (int i = 0; i < squares.Count; i++)
		{
			BoardSquare boardSquare = squares[i];
			if (!(boardSquare != null))
			{
				continue;
			}
			bool flag2 = false;
			if (!seeThroughBrush)
			{
				if (BrushCoordinator.Get() != null && boardSquare.BrushRegion > 0)
				{
					flag2 = BrushCoordinator.Get().IsRegionFunctioning(boardSquare.BrushRegion);
				}
			}
			if (!flag2)
			{
				m_visibleSquares.TryGetValue(boardSquare, out VisibleSquareEntry value);
				value.m_visibleFlags |= (int)flag;
				m_visibleSquares[boardSquare] = value;
			}
		}
	}

	private int GetVisTypePriority(BoardSquare.VisibilityFlags visType)
	{
		if (visType == BoardSquare.VisibilityFlags.Revealed)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return 4;
				}
			}
		}
		if (visType == BoardSquare.VisibilityFlags.Objective)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return 3;
				}
			}
		}
		if (visType == BoardSquare.VisibilityFlags.Self)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return 2;
				}
			}
		}
		if (visType == BoardSquare.VisibilityFlags.Team)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return 1;
				}
			}
		}
		return 0;
	}

	private int GetHighestVisTypePriority(int flags)
	{
		if ((flags & 8) != 0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return 4;
				}
			}
		}
		if ((flags & 4) != 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return 3;
				}
			}
		}
		if ((flags & 1) != 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return 2;
				}
			}
		}
		if ((flags & 2) != 0)
		{
			return 1;
		}
		return 0;
	}

	private float CalcHorizontalDistanceOnBoardTo(int firstX, int firstY, int secondX, int secondY)
	{
		int num = Mathf.Abs(firstX - secondX);
		int num2 = Mathf.Abs(firstY - secondY);
		float num3 = 0f;
		if (num > num2)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return (float)(num - num2) + (float)num2 * 1.5f;
				}
			}
		}
		return (float)(num2 - num) + (float)num * 1.5f;
	}

	public bool IsVisible(BoardSquare square)
	{
		if (!NetworkServer.active)
		{
			if (m_owner != GameFlowData.Get().activeOwnedActorData)
			{
				Log.Warning("Calling FogOfWar::IsVisible(BoardSquare square) on a client for not-the-client actor.");
			}
		}
		if (square == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		return m_visibleSquares.ContainsKey(square);
	}

	public bool IsVisibleBySelf(BoardSquare square)
	{
		bool result = false;
		if (square != null)
		{
			if (m_visibleSquares.ContainsKey(square))
			{
				VisibleSquareEntry visibleSquareEntry = m_visibleSquares[square];
				if ((visibleSquareEntry.m_visibleFlags & 1) != 0)
				{
					result = true;
				}
			}
		}
		return result;
	}

	public bool IsVisibleBySelf(ActorData otherActor)
	{
		bool result = false;
		if (otherActor != null)
		{
			BoardSquare travelBoardSquare = otherActor.GetTravelBoardSquare();
			if (travelBoardSquare != null)
			{
				if (IsVisibleBySelf(travelBoardSquare))
				{
					result = true;
				}
			}
		}
		return result;
	}

	internal Bounds CalcVisibleBySelfBounds()
	{
		Bounds result = default(Bounds);
		bool flag = false;
		using (Dictionary<BoardSquare, VisibleSquareEntry>.Enumerator enumerator = m_visibleSquares.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<BoardSquare, VisibleSquareEntry> current = enumerator.Current;
				BoardSquare key = current.Key;
				VisibleSquareEntry value = current.Value;
				if ((value.m_visibleFlags & 1) != 0)
				{
					if (flag)
					{
						result.Encapsulate(key.CameraBounds);
					}
					else
					{
						result = key.CameraBounds;
						flag = true;
					}
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return result;
				}
			}
		}
	}

	public static void CalculateFogOfWarForTeam(Team team)
	{
		if (GameFlowData.Get() == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Debug.LogError("Trying to calculate fog of war, but GameFlowData does not exist.");
					return;
				}
			}
		}
		List<ActorData> actors = GameFlowData.Get().GetActors();
		using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData current = enumerator.Current;
				if (current.GetTeam() == team)
				{
					FogOfWar fogOfWar = current.GetFogOfWar();
					if (fogOfWar != null)
					{
						fogOfWar.MarkForRecalculateVisibility();
					}
				}
			}
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
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
		if ((bool)m_owner)
		{
			bool flag = m_owner.IsDead();
			if (!flag)
			{
				if (NetworkClient.active)
				{
					flag = m_owner.IsModelAnimatorDisabled();
				}
			}
			float num;
			if (flag)
			{
				num = 0f;
			}
			else
			{
				num = m_owner.GetActualSightRange();
			}
			float radius = num;
			CalcVisibleSquares(m_owner.GetTravelBoardSquare(), radius, false, BoardSquare.VisibilityFlags.Self, false);
		}
		bool flag2 = GameplayMutators.IsStatusActive(StatusType.Blind, GameFlowData.Get().CurrentTurn);
		if (!m_visibilityPersonalOnly)
		{
			if ((bool)m_owner)
			{
				ActorAdditionalVisionProviders actorAdditionalVisionProviders = m_owner.GetActorAdditionalVisionProviders();
				if (actorAdditionalVisionProviders != null)
				{
					SyncListVisionProviderInfo visionProviders = actorAdditionalVisionProviders.GetVisionProviders();
					for (int i = 0; i < visionProviders.Count; i++)
					{
						if (flag2)
						{
							VisionProviderInfo visionProviderInfo = visionProviders[i];
							if (!visionProviderInfo.m_canFunctionInGlobalBlind)
							{
								continue;
							}
						}
						if (visionProviders[i].GetBoardSquare() != null)
						{
							BoardSquare boardSquare = visionProviders[i].GetBoardSquare();
							VisionProviderInfo visionProviderInfo2 = visionProviders[i];
							float radius2 = visionProviderInfo2.m_radius;
							VisionProviderInfo visionProviderInfo3 = visionProviders[i];
							bool radiusAsStraightLineDist = visionProviderInfo3.m_radiusAsStraightLineDist;
							VisionProviderInfo visionProviderInfo4 = visionProviders[i];
							BoardSquare.VisibilityFlags flag3 = visionProviderInfo4.m_flag;
							VisionProviderInfo visionProviderInfo5 = visionProviders[i];
							bool ignoreLos = visionProviderInfo5.m_ignoreLos;
							VisionProviderInfo visionProviderInfo6 = visionProviders[i];
							CalcVisibleSquares(boardSquare, radius2, radiusAsStraightLineDist, flag3, ignoreLos, visionProviderInfo6.m_brushRevealType);
						}
					}
				}
				List<ControlPoint> allControlPoints = ControlPoint.GetAllControlPoints();
				if (allControlPoints != null)
				{
					for (int j = 0; j < allControlPoints.Count; j++)
					{
						ControlPoint controlPoint = allControlPoints[j];
						if (controlPoint.IsGrantingVisionForTeam(m_owner.GetTeam()))
						{
							AppendToVisibleSquares(controlPoint.GetSquaresForVision(), BoardSquare.VisibilityFlags.Team, controlPoint.m_visionSeeThroughBrush);
						}
					}
				}
			}
			Team team = (!m_owner) ? m_ownerPlayer.GetTeamViewing() : m_owner.GetTeam();
			object obj;
			if ((bool)m_owner)
			{
				obj = m_owner.GetActorStatus();
			}
			else
			{
				obj = null;
			}
			ActorStatus actorStatus = (ActorStatus)obj;
			List<ActorData> list;
			List<ActorData> list2;
			if (team != Team.Invalid)
			{
				list = GameFlowData.Get().GetAllTeamMembers(team);
				list2 = GameFlowData.Get().GetAllTeamMembers(team.OtherTeam());
			}
			else
			{
				list = new List<ActorData>(GameFlowData.Get().GetAllTeamMembers(Team.TeamA));
				list.AddRange(GameFlowData.Get().GetAllTeamMembers(Team.TeamB));
				list2 = new List<ActorData>();
			}
			bool flag4 = true;
			if ((bool)actorStatus && actorStatus.HasStatus(StatusType.LoseAllyVision))
			{
				flag4 = false;
			}
			if (flag4)
			{
				foreach (ActorData item in list)
				{
					if ((bool)item)
					{
						if (!(m_owner == null))
						{
							if (!(m_owner != item))
							{
								continue;
							}
						}
						if ((bool)item.GetTravelBoardSquare())
						{
							ActorStatus actorStatus2 = item.GetActorStatus();
							if (!actorStatus2.HasStatus(StatusType.IsolateVisionFromAllies))
							{
								bool flag5 = item.IsDead();
								if (!flag5 && NetworkClient.active)
								{
									flag5 = item.IsModelAnimatorDisabled();
								}
								float radius3 = (!flag5) ? item.GetActualSightRange() : 0f;
								CalcVisibleSquares(item.GetTravelBoardSquare(), radius3, false, BoardSquare.VisibilityFlags.Team, false);
								ActorAdditionalVisionProviders component = item.GetComponent<ActorAdditionalVisionProviders>();
								if (component != null)
								{
									SyncListVisionProviderInfo visionProviders2 = component.GetVisionProviders();
									for (int k = 0; k < visionProviders2.Count; k++)
									{
										if (flag2)
										{
											VisionProviderInfo visionProviderInfo7 = visionProviders2[k];
											if (!visionProviderInfo7.m_canFunctionInGlobalBlind)
											{
												continue;
											}
										}
										if (visionProviders2[k].GetBoardSquare() != null)
										{
											BoardSquare boardSquare2 = visionProviders2[k].GetBoardSquare();
											VisionProviderInfo visionProviderInfo8 = visionProviders2[k];
											float radius4 = visionProviderInfo8.m_radius;
											VisionProviderInfo visionProviderInfo9 = visionProviders2[k];
											bool radiusAsStraightLineDist2 = visionProviderInfo9.m_radiusAsStraightLineDist;
											VisionProviderInfo visionProviderInfo10 = visionProviders2[k];
											BoardSquare.VisibilityFlags flag6 = visionProviderInfo10.m_flag;
											VisionProviderInfo visionProviderInfo11 = visionProviders2[k];
											bool ignoreLos2 = visionProviderInfo11.m_ignoreLos;
											VisionProviderInfo visionProviderInfo12 = visionProviders2[k];
											CalcVisibleSquares(boardSquare2, radius4, radiusAsStraightLineDist2, flag6, ignoreLos2, visionProviderInfo12.m_brushRevealType);
										}
									}
								}
							}
						}
					}
				}
			}
			using (List<ActorData>.Enumerator enumerator2 = list2.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActorData current2 = enumerator2.Current;
					if (!(current2 != null))
					{
						continue;
					}
					BoardSquare travelBoardSquare = current2.GetTravelBoardSquare();
					ActorStatus actorStatus3 = current2.GetActorStatus();
					int num2;
					if ((bool)actorStatus3)
					{
						num2 = (actorStatus3.HasStatus(StatusType.Revealed) ? 1 : 0);
					}
					else
					{
						num2 = 0;
					}
					bool flag7 = (byte)num2 != 0;
					bool flag8 = false;
					if (!NetworkServer.active)
					{
						if (CaptureTheFlag.IsActorRevealedByFlag_Client(current2))
						{
							flag8 = true;
						}
					}
					if ((bool)travelBoardSquare)
					{
						if (!flag7)
						{
							if (!flag8)
							{
								goto IL_06a6;
							}
						}
						CalcVisibleSquares(current2.GetTravelBoardSquare(), 0.1f, false, BoardSquare.VisibilityFlags.Revealed, true);
					}
					goto IL_06a6;
					IL_06a6:
					if ((bool)travelBoardSquare)
					{
						if ((bool)m_owner && m_owner.IsLineOfSightVisibleException(current2))
						{
							m_visibleSquares.TryGetValue(travelBoardSquare, out VisibleSquareEntry value);
							value.m_visibleFlags |= 8;
							m_visibleSquares[travelBoardSquare] = value;
						}
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
			if (GameFlowData.Get().activeOwnedActorData != m_owner)
			{
				return false;
			}
		}
		else if (m_ownerPlayer != GameFlowData.Get().LocalPlayerData)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		return true;
	}

	public void SetVisibleShadeOfAllSquares()
	{
		if (!IsClientFog())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		bool anySquareShadeChanged = false;
		Board board = Board.Get();
		for (int i = 0; i < board.GetMaxX(); i++)
		{
			for (int j = 0; j < board.GetMaxY(); j++)
			{
				BoardSquare boardSquare = board.GetBoardSquare(i, j);
				if (boardSquare != null)
				{
					if (m_visibleSquares.ContainsKey(boardSquare))
					{
						VisibleSquareEntry visibleSquareEntry = m_visibleSquares[boardSquare];
						boardSquare.SetVisibleShade(visibleSquareEntry.m_visibleFlags, ref anySquareShadeChanged);
					}
					else
					{
						boardSquare.SetVisibleShade(0, ref anySquareShadeChanged);
					}
				}
				else
				{
					Log.Info($"Trying to set visible shade of square ({i}, {j}), but it's out of bounds");
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					goto end_IL_00d1;
				}
				continue;
				end_IL_00d1:
				break;
			}
		}
		while (true)
		{
			if (!anySquareShadeChanged)
			{
				return;
			}
			while (true)
			{
				if (GameEventManager.Get() != null)
				{
					while (true)
					{
						GameEventManager.Get().FireEvent(GameEventManager.EventType.BoardSquareVisibleShadeChanged, null);
						return;
					}
				}
				return;
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (!(GameFlowData.Get() != null) || !(m_owner != null))
		{
			return;
		}
		while (true)
		{
			if (m_owner == GameFlowData.Get().activeOwnedActorData)
			{
				while (true)
				{
					using (Dictionary<BoardSquare, VisibleSquareEntry>.Enumerator enumerator = m_visibleSquares.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<BoardSquare, VisibleSquareEntry> current = enumerator.Current;
							BoardSquare key = current.Key;
							VisibleSquareEntry value = current.Value;
							if ((value.m_visibleFlags & 1) != 0)
							{
								Gizmos.color = Color.gray;
							}
							else
							{
								Gizmos.color = Color.blue;
							}
							Gizmos.DrawWireCube(key.CameraBounds.center, key.CameraBounds.size * 0.7f);
						}
					}
					Gizmos.color = Color.white;
					float num = 1f - Mathf.Clamp((Time.time - m_lastRecalcTime) * 2f, 0f, 1f);
					Color color2 = Gizmos.color = new Color(0f, num, num, num);
					Gizmos.DrawSphere(m_owner.GetNameplatePosition(10f), 0.5f);
					return;
				}
			}
			return;
		}
	}
}
