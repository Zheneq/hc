using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FogOfWar : MonoBehaviour
{
	private bool m_updateVisibility;

	private bool m_updatedVisibilityThisFrame;

	private bool m_visibilityPersonalOnly;

	private PlayerData m_ownerPlayer;

	private ActorData m_owner;

	private Dictionary<BoardSquare, FogOfWar.VisibleSquareEntry> m_visibleSquares;

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
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.GetClientFog()).MethodHandle;
				}
				result = GameFlowData.Get().LocalPlayerData.GetFogOfWar();
			}
		}
		return result;
	}

	private void Awake()
	{
		this.m_visibleSquares = new Dictionary<BoardSquare, FogOfWar.VisibleSquareEntry>();
		this.m_ownerPlayer = base.GetComponent<PlayerData>();
		this.m_owner = base.GetComponent<ActorData>();
	}

	private bool ShouldUpdateMyVisibility()
	{
		bool result = false;
		if (this.m_updateVisibility)
		{
			if (NetworkServer.active)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.ShouldUpdateMyVisibility()).MethodHandle;
				}
				result = true;
			}
			else if (GameFlowData.Get().LocalPlayerData != null)
			{
				Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
				Team teamViewing2 = this.m_ownerPlayer.GetTeamViewing();
				if (teamViewing != Team.Invalid)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (teamViewing != teamViewing2)
					{
						goto IL_97;
					}
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (this.m_ownerPlayer.PlayerIndex != PlayerData.s_invalidPlayerIndex)
				{
					result = true;
				}
			}
		}
		IL_97:
		if (this.m_visibilityPersonalOnly != InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowPersonalVisibility))
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.ShouldUpdateClientActorVisibility()).MethodHandle;
			}
			if (GameFlowData.Get().LocalPlayerData.GetFogOfWar() != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
				bool updateVisibility = this.m_updateVisibility;
				bool flag;
				if (!(this.m_owner == null) && teamViewing != Team.Invalid)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = (teamViewing == this.m_owner.GetTeam());
				}
				else
				{
					flag = true;
				}
				bool flag2 = flag;
				bool flag3 = this.m_ownerPlayer != GameFlowData.Get().LocalPlayerData;
				bool flag4 = !FogOfWar.GetClientFog().m_updatedVisibilityThisFrame;
				if (updateVisibility)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (flag2)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (flag3 && flag4)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
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
		this.m_updatedVisibilityThisFrame = false;
		if (this.ShouldUpdateMyVisibility())
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.Update()).MethodHandle;
			}
			this.UpdateVisibilityOfSquares(true);
			this.m_updatedVisibilityThisFrame = true;
		}
		if (this.ShouldUpdateClientActorVisibility())
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			FogOfWar.GetClientFog().MarkForRecalculateVisibility();
		}
		this.m_updateVisibility = false;
	}

	private void CalcVisibleSquares(BoardSquare center, float radius, bool radiusAsStraightLineDist, BoardSquare.VisibilityFlags flag, bool ignoreLOS, VisionProviderInfo.BrushRevealType brushRevealType = VisionProviderInfo.BrushRevealType.BaseOnCenterPosition)
	{
		if (center != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.CalcVisibleSquares(BoardSquare, float, bool, BoardSquare.VisibilityFlags, bool, VisionProviderInfo.BrushRevealType)).MethodHandle;
			}
			if (radius > 0f)
			{
				ActorData owner = this.m_owner;
				Board board = Board.Get();
				float num = Mathf.Max(radius, 0f);
				int x = center.GetGridPos().x;
				int y = center.GetGridPos().y;
				int num2 = Mathf.Max(Mathf.FloorToInt((float)x - num), 0);
				int num3 = Mathf.Min(Mathf.CeilToInt((float)x + num), board.GetMaxX() - 1);
				int num4 = Mathf.Max(Mathf.FloorToInt((float)y - num), 0);
				int num5 = Mathf.Min(Mathf.CeilToInt((float)y + num), board.GetMaxY() - 1);
				bool flag2;
				if (owner != null)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					flag2 = owner.GetActorStatus().HasStatus(StatusType.SeeThroughBrush, true);
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				for (int i = num2; i <= num3; i++)
				{
					for (int j = num4; j <= num5; j++)
					{
						BoardSquare boardSquare = board.GetBoardSquare(i, j);
						if (boardSquare != null)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							FogOfWar.VisibleSquareEntry value;
							if (this.m_visibleSquares.TryGetValue(boardSquare, out value))
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								if (this.GetVisTypePriority(flag) <= this.GetHighestVisTypePriority(value.m_visibleFlags))
								{
									goto IL_2F6;
								}
							}
							float num6;
							if (radiusAsStraightLineDist)
							{
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								num6 = boardSquare.HorizontalDistanceInSquaresTo(center);
							}
							else
							{
								num6 = this.CalcHorizontalDistanceOnBoardTo(x, y, i, j);
							}
							float num7 = num6;
							if (num7 <= num)
							{
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!ignoreLOS)
								{
									for (;;)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									if (!center.\u0013(i, j))
									{
										goto IL_2F6;
									}
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
								}
								if (!owner)
								{
									goto IL_1F3;
								}
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								if (flag3)
								{
									goto IL_204;
								}
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									goto IL_1F3;
								}
								IL_286:
								bool flag5;
								if (BarrierManager.Get() != null)
								{
									bool flag4;
									if (owner)
									{
										for (;;)
										{
											switch (1)
											{
											case 0:
												continue;
											}
											break;
										}
										flag4 = BarrierManager.Get().IsVisionBlocked(owner, center, boardSquare);
									}
									else
									{
										flag4 = false;
									}
									flag5 = flag4;
								}
								else
								{
									flag5 = false;
								}
								bool flag6;
								if (!flag6)
								{
									if (flag5)
									{
										for (;;)
										{
											switch (7)
											{
											case 0:
												continue;
											}
											break;
										}
									}
									else
									{
										value.m_visibleFlags |= (int)flag;
										this.m_visibleSquares[boardSquare] = value;
									}
								}
								goto IL_2F6;
								IL_1F3:
								if (!(BrushCoordinator.Get() == null))
								{
									if (brushRevealType == VisionProviderInfo.BrushRevealType.Never)
									{
										for (;;)
										{
											switch (3)
											{
											case 0:
												continue;
											}
											break;
										}
										if (boardSquare.BrushRegion >= 0)
										{
											for (;;)
											{
												switch (3)
												{
												case 0:
													continue;
												}
												break;
											}
											flag6 = BrushCoordinator.Get().IsRegionFunctioning(boardSquare.BrushRegion);
											goto IL_286;
										}
									}
									if (brushRevealType == VisionProviderInfo.BrushRevealType.Always)
									{
										for (;;)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										if (boardSquare.BrushRegion >= 0)
										{
											for (;;)
											{
												switch (5)
												{
												case 0:
													continue;
												}
												break;
											}
											flag6 = false;
											goto IL_286;
										}
									}
									flag6 = BrushCoordinator.Get().IsSquareHiddenFrom(boardSquare, center);
									goto IL_286;
								}
								IL_204:
								flag6 = false;
								goto IL_286;
							}
						}
						IL_2F6:;
					}
				}
			}
		}
	}

	private void AppendToVisibleSquares(List<BoardSquare> squares, BoardSquare.VisibilityFlags flag, bool seeThroughBrush)
	{
		for (int i = 0; i < squares.Count; i++)
		{
			BoardSquare boardSquare = squares[i];
			if (boardSquare != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.AppendToVisibleSquares(List<BoardSquare>, BoardSquare.VisibilityFlags, bool)).MethodHandle;
				}
				bool flag2 = false;
				if (!seeThroughBrush)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (BrushCoordinator.Get() != null && boardSquare.BrushRegion > 0)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						flag2 = BrushCoordinator.Get().IsRegionFunctioning(boardSquare.BrushRegion);
					}
				}
				if (!flag2)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					FogOfWar.VisibleSquareEntry value;
					this.m_visibleSquares.TryGetValue(boardSquare, out value);
					value.m_visibleFlags |= (int)flag;
					this.m_visibleSquares[boardSquare] = value;
				}
			}
		}
	}

	private int GetVisTypePriority(BoardSquare.VisibilityFlags visType)
	{
		if (visType == BoardSquare.VisibilityFlags.Revealed)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.GetVisTypePriority(BoardSquare.VisibilityFlags)).MethodHandle;
			}
			return 4;
		}
		if (visType == BoardSquare.VisibilityFlags.Objective)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			return 3;
		}
		if (visType == BoardSquare.VisibilityFlags.Self)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			return 2;
		}
		if (visType == BoardSquare.VisibilityFlags.Team)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			return 1;
		}
		return 0;
	}

	private int GetHighestVisTypePriority(int flags)
	{
		if ((flags & 8) != 0)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.GetHighestVisTypePriority(int)).MethodHandle;
			}
			return 4;
		}
		if ((flags & 4) != 0)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			return 3;
		}
		if ((flags & 1) != 0)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			return 2;
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
		float result;
		if (num > num2)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.CalcHorizontalDistanceOnBoardTo(int, int, int, int)).MethodHandle;
			}
			result = (float)(num - num2) + (float)num2 * 1.5f;
		}
		else
		{
			result = (float)(num2 - num) + (float)num * 1.5f;
		}
		return result;
	}

	public bool IsVisible(BoardSquare square)
	{
		if (!NetworkServer.active)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.IsVisible(BoardSquare)).MethodHandle;
			}
			if (this.m_owner != GameFlowData.Get().activeOwnedActorData)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				Log.Warning("Calling FogOfWar::IsVisible(BoardSquare square) on a client for not-the-client actor.", new object[0]);
			}
		}
		if (square == null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			return false;
		}
		return this.m_visibleSquares.ContainsKey(square);
	}

	public bool IsVisibleBySelf(BoardSquare square)
	{
		bool result = false;
		if (square != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.IsVisibleBySelf(BoardSquare)).MethodHandle;
			}
			if (this.m_visibleSquares.ContainsKey(square) && (this.m_visibleSquares[square].m_visibleFlags & 1) != 0)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				result = true;
			}
		}
		return result;
	}

	public bool IsVisibleBySelf(ActorData otherActor)
	{
		bool result = false;
		if (otherActor != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.IsVisibleBySelf(ActorData)).MethodHandle;
			}
			BoardSquare travelBoardSquare = otherActor.GetTravelBoardSquare();
			if (travelBoardSquare != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.IsVisibleBySelf(travelBoardSquare))
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
		using (Dictionary<BoardSquare, FogOfWar.VisibleSquareEntry>.Enumerator enumerator = this.m_visibleSquares.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<BoardSquare, FogOfWar.VisibleSquareEntry> keyValuePair = enumerator.Current;
				BoardSquare key = keyValuePair.Key;
				if ((keyValuePair.Value.m_visibleFlags & 1) != 0)
				{
					if (flag)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.CalcVisibleBySelfBounds()).MethodHandle;
						}
						result.Encapsulate(key.CameraBounds);
					}
					else
					{
						result = key.CameraBounds;
						flag = true;
					}
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return result;
	}

	public static void CalculateFogOfWarForTeam(Team team)
	{
		if (GameFlowData.Get() == null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.CalculateFogOfWarForTeam(Team)).MethodHandle;
			}
			Debug.LogError("Trying to calculate fog of war, but GameFlowData does not exist.");
		}
		else
		{
			List<ActorData> actors = GameFlowData.Get().GetActors();
			using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ActorData actorData = enumerator.Current;
					if (actorData.GetTeam() == team)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						FogOfWar fogOfWar = actorData.GetFogOfWar();
						if (fogOfWar != null)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							fogOfWar.MarkForRecalculateVisibility();
						}
					}
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
	}

	public void MarkForRecalculateVisibility()
	{
		this.m_updateVisibility = true;
	}

	public void UpdateVisibilityOfSquares(bool updateShade = true)
	{
		this.m_visibleSquares.Clear();
		this.m_visibilityPersonalOnly = InputManager.Get().IsKeyBindingHeld(KeyPreference.ShowPersonalVisibility);
		if (this.m_owner)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.UpdateVisibilityOfSquares(bool)).MethodHandle;
			}
			bool flag = this.m_owner.IsDead();
			if (!flag)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (NetworkClient.active)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = this.m_owner.IsModelAnimatorDisabled();
				}
			}
			float num;
			if (flag)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				num = 0f;
			}
			else
			{
				num = this.m_owner.GetActualSightRange();
			}
			float radius = num;
			this.CalcVisibleSquares(this.m_owner.GetTravelBoardSquare(), radius, false, BoardSquare.VisibilityFlags.Self, false, VisionProviderInfo.BrushRevealType.BaseOnCenterPosition);
		}
		bool flag2 = GameplayMutators.IsStatusActive(StatusType.Blind, GameFlowData.Get().CurrentTurn, GameplayMutators.ActionPhaseCheckMode.Default);
		if (!this.m_visibilityPersonalOnly)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_owner)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				ActorAdditionalVisionProviders actorAdditionalVisionProviders = this.m_owner.GetActorAdditionalVisionProviders();
				if (actorAdditionalVisionProviders != null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					SyncListVisionProviderInfo visionProviders = actorAdditionalVisionProviders.GetVisionProviders();
					int i = 0;
					while (i < (int)visionProviders.Count)
					{
						if (!flag2)
						{
							goto IL_163;
						}
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (visionProviders[i].m_canFunctionInGlobalBlind)
						{
							goto IL_163;
						}
						IL_20C:
						i++;
						continue;
						IL_163:
						if (visionProviders[i].GetBoardSquare() != null)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							this.CalcVisibleSquares(visionProviders[i].GetBoardSquare(), visionProviders[i].m_radius, visionProviders[i].m_radiusAsStraightLineDist, visionProviders[i].m_flag, visionProviders[i].m_ignoreLos, visionProviders[i].m_brushRevealType);
							goto IL_20C;
						}
						goto IL_20C;
					}
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				List<ControlPoint> allControlPoints = ControlPoint.GetAllControlPoints();
				if (allControlPoints != null)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					for (int j = 0; j < allControlPoints.Count; j++)
					{
						ControlPoint controlPoint = allControlPoints[j];
						if (controlPoint.IsGrantingVisionForTeam(this.m_owner.GetTeam()))
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							this.AppendToVisibleSquares(controlPoint.GetSquaresForVision(), BoardSquare.VisibilityFlags.Team, controlPoint.m_visionSeeThroughBrush);
						}
					}
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			Team team = (!this.m_owner) ? this.m_ownerPlayer.GetTeamViewing() : this.m_owner.GetTeam();
			ActorStatus actorStatus;
			if (this.m_owner)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				actorStatus = this.m_owner.GetActorStatus();
			}
			else
			{
				actorStatus = null;
			}
			ActorStatus actorStatus2 = actorStatus;
			List<ActorData> list;
			List<ActorData> list2;
			if (team != Team.Invalid)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				list = GameFlowData.Get().GetAllTeamMembers(team);
				list2 = GameFlowData.Get().GetAllTeamMembers(team.OtherTeam());
			}
			else
			{
				list = new List<ActorData>(GameFlowData.Get().GetAllTeamMembers(Team.TeamA));
				list.AddRange(GameFlowData.Get().GetAllTeamMembers(Team.TeamB));
				list2 = new List<ActorData>();
			}
			bool flag3 = true;
			if (actorStatus2 && actorStatus2.HasStatus(StatusType.LoseAllyVision, true))
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				flag3 = false;
			}
			if (flag3)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				foreach (ActorData actorData in list)
				{
					if (actorData)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!(this.m_owner == null))
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!(this.m_owner != actorData))
							{
								continue;
							}
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						if (actorData.GetTravelBoardSquare())
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							ActorStatus actorStatus3 = actorData.GetActorStatus();
							if (!actorStatus3.HasStatus(StatusType.IsolateVisionFromAllies, true))
							{
								bool flag4 = actorData.IsDead();
								if (!flag4 && NetworkClient.active)
								{
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									flag4 = actorData.IsModelAnimatorDisabled();
								}
								float radius2 = (!flag4) ? actorData.GetActualSightRange() : 0f;
								this.CalcVisibleSquares(actorData.GetTravelBoardSquare(), radius2, false, BoardSquare.VisibilityFlags.Team, false, VisionProviderInfo.BrushRevealType.BaseOnCenterPosition);
								ActorAdditionalVisionProviders component = actorData.GetComponent<ActorAdditionalVisionProviders>();
								if (component != null)
								{
									for (;;)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									SyncListVisionProviderInfo visionProviders2 = component.GetVisionProviders();
									for (int k = 0; k < (int)visionProviders2.Count; k++)
									{
										if (!flag2 || visionProviders2[k].m_canFunctionInGlobalBlind)
										{
											if (visionProviders2[k].GetBoardSquare() != null)
											{
												for (;;)
												{
													switch (2)
													{
													case 0:
														continue;
													}
													break;
												}
												this.CalcVisibleSquares(visionProviders2[k].GetBoardSquare(), visionProviders2[k].m_radius, visionProviders2[k].m_radiusAsStraightLineDist, visionProviders2[k].m_flag, visionProviders2[k].m_ignoreLos, visionProviders2[k].m_brushRevealType);
											}
										}
									}
									for (;;)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
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
					ActorData actorData2 = enumerator2.Current;
					if (actorData2 != null)
					{
						BoardSquare travelBoardSquare = actorData2.GetTravelBoardSquare();
						ActorStatus actorStatus4 = actorData2.GetActorStatus();
						bool flag5;
						if (actorStatus4)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							flag5 = actorStatus4.HasStatus(StatusType.Revealed, true);
						}
						else
						{
							flag5 = false;
						}
						bool flag6 = flag5;
						bool flag7 = false;
						if (!NetworkServer.active)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (CaptureTheFlag.IsActorRevealedByFlag_Client(actorData2))
							{
								for (;;)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								flag7 = true;
							}
						}
						if (travelBoardSquare)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!flag6)
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!flag7)
								{
									goto IL_6A6;
								}
							}
							this.CalcVisibleSquares(actorData2.GetTravelBoardSquare(), 0.1f, false, BoardSquare.VisibilityFlags.Revealed, true, VisionProviderInfo.BrushRevealType.BaseOnCenterPosition);
						}
						IL_6A6:
						if (travelBoardSquare)
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (this.m_owner && this.m_owner.IsLineOfSightVisibleException(actorData2))
							{
								for (;;)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								FogOfWar.VisibleSquareEntry value;
								this.m_visibleSquares.TryGetValue(travelBoardSquare, out value);
								value.m_visibleFlags |= 8;
								this.m_visibleSquares[travelBoardSquare] = value;
							}
						}
					}
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		if (updateShade)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			this.SetVisibleShadeOfAllSquares();
		}
		this.m_lastRecalcTime = Time.time;
	}

	private bool IsClientFog()
	{
		if (GameFlowData.Get().activeOwnedActorData != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.IsClientFog()).MethodHandle;
			}
			if (GameFlowData.Get().activeOwnedActorData != this.m_owner)
			{
				return false;
			}
		}
		else if (this.m_ownerPlayer != GameFlowData.Get().LocalPlayerData)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			return false;
		}
		return true;
	}

	public void SetVisibleShadeOfAllSquares()
	{
		if (!this.IsClientFog())
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.SetVisibleShadeOfAllSquares()).MethodHandle;
			}
			return;
		}
		bool flag = false;
		Board board = Board.Get();
		for (int i = 0; i < board.GetMaxX(); i++)
		{
			for (int j = 0; j < board.GetMaxY(); j++)
			{
				BoardSquare boardSquare = board.GetBoardSquare(i, j);
				if (boardSquare != null)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_visibleSquares.ContainsKey(boardSquare))
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						boardSquare.SetVisibleShade(this.m_visibleSquares[boardSquare].m_visibleFlags, ref flag);
					}
					else
					{
						boardSquare.SetVisibleShade(0, ref flag);
					}
				}
				else
				{
					Log.Info(string.Format("Trying to set visible shade of square ({0}, {1}), but it's out of bounds", i, j), new object[0]);
				}
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		if (flag)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (GameEventManager.Get() != null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				GameEventManager.Get().FireEvent(GameEventManager.EventType.BoardSquareVisibleShadeChanged, null);
			}
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FogOfWar.OnDrawGizmos()).MethodHandle;
			}
			return;
		}
		if (GameFlowData.Get() != null && this.m_owner != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_owner == GameFlowData.Get().activeOwnedActorData)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				using (Dictionary<BoardSquare, FogOfWar.VisibleSquareEntry>.Enumerator enumerator = this.m_visibleSquares.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<BoardSquare, FogOfWar.VisibleSquareEntry> keyValuePair = enumerator.Current;
						BoardSquare key = keyValuePair.Key;
						if ((keyValuePair.Value.m_visibleFlags & 1) != 0)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							Gizmos.color = Color.gray;
						}
						else
						{
							Gizmos.color = Color.blue;
						}
						Gizmos.DrawWireCube(key.CameraBounds.center, key.CameraBounds.size * 0.7f);
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				Gizmos.color = Color.white;
				float num = 1f - Mathf.Clamp((Time.time - this.m_lastRecalcTime) * 2f, 0f, 1f);
				Color color = new Color(0f, num, num, num);
				Gizmos.color = color;
				Gizmos.DrawSphere(this.m_owner.GetNameplatePosition(10f), 0.5f);
			}
		}
	}

	private struct VisibleSquareEntry
	{
		public int m_visibleFlags;
	}
}
