using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActorMovement : MonoBehaviour, IGameEventListener
{
	internal ActorData m_actor;

	private float m_moveTimeoutTime;

	private List<ActorMovement.SquaresCanMoveToCacheEntry> m_squaresCanMoveToCache;

	private static int s_maxSquaresCanMoveToCacheCount = 4;

	private HashSet<BoardSquare> m_squaresCanMoveTo;

	private HashSet<BoardSquare> m_squareCanMoveToWithQueuedAbility;

	public float m_brushTransitionAnimationSpeed;

	public float m_brushTransitionAnimationSpeedEaseTime = 0.4f;

	private EasedOutFloat m_brushTransitionAnimationSpeedEased = new EasedOutFloat(1f);

	private static int animDistToGoal;

	private static int animDistToWaypoint;

	private static int animNextChargeCycleType;

	private static int animChargeCycleType;

	private static int animChargeEndType;

	private static int animNextLinkType;

	private static int animCurLinkType;

	private static int animTimeInAnim;

	private static int animForceIdle;

	private static int animMoveSegmentStart;

	private static int animDecisionPhase;

	private static int animIdleType;

	private MoveState m_curMoveState;

	private Dictionary<BoardSquare, BoardSquarePathInfo> m_tempClosedSquares = new Dictionary<BoardSquare, BoardSquarePathInfo>();

	private BoardSquarePathInfo m_gameplayPath;

	private BoardSquarePathInfo m_aestheticPath;

	public string GetCurrentMoveStateStr()
	{
		if (this.m_curMoveState != null)
		{
			return this.m_curMoveState.stateName;
		}
		return "None";
	}

	private void Awake()
	{
		this.m_actor = base.GetComponent<ActorData>();
		this.m_squaresCanMoveToCache = new List<ActorMovement.SquaresCanMoveToCacheEntry>();
		this.m_squaresCanMoveTo = new HashSet<BoardSquare>();
		this.m_squareCanMoveToWithQueuedAbility = new HashSet<BoardSquare>();
		ActorMovement.animDistToGoal = Animator.StringToHash("DistToGoal");
		ActorMovement.animDistToWaypoint = Animator.StringToHash("DistToWayPoint");
		ActorMovement.animNextChargeCycleType = Animator.StringToHash("NextChargeCycleType");
		ActorMovement.animChargeCycleType = Animator.StringToHash("ChargeCycleType");
		ActorMovement.animChargeEndType = Animator.StringToHash("ChargeEndType");
		ActorMovement.animNextLinkType = Animator.StringToHash("NextLinkType");
		ActorMovement.animCurLinkType = Animator.StringToHash("CurLinkType");
		ActorMovement.animTimeInAnim = Animator.StringToHash("TimeInAnim");
		ActorMovement.animForceIdle = Animator.StringToHash("ForceIdle");
		ActorMovement.animMoveSegmentStart = Animator.StringToHash("MoveSegmentStart");
		ActorMovement.animDecisionPhase = Animator.StringToHash("DecisionPhase");
		ActorMovement.animIdleType = Animator.StringToHash("IdleType");
	}

	private void Start()
	{
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.ActiveControlChangedToEnemyTeam);
	}

	private void OnDestroy()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ActiveControlChangedToEnemyTeam);
	}

	private void Update()
	{
		this.UpdatePosition();
		if (NetworkClient.active)
		{
			if (this.m_actor != null)
			{
				if (this.m_actor.GetModelAnimator() != null)
				{
					float num = this.m_brushTransitionAnimationSpeedEased;
					Animator modelAnimator = this.m_actor.GetModelAnimator();
					if (modelAnimator.speed != num)
					{
						modelAnimator.speed = num;
					}
				}
			}
			if (this.m_actor == GameFlowData.Get().activeOwnedActorData)
			{
				if (!InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.MovementWaypointModifier))
				{
					if (!InputManager.Get().IsKeyBindingNewlyReleased(KeyPreference.MovementWaypointModifier))
					{
						return;
					}
				}
				this.UpdateSquaresCanMoveTo();
			}
		}
	}

	internal HashSet<BoardSquare> SquaresCanMoveTo
	{
		get
		{
			return this.m_squaresCanMoveTo;
		}
	}

	internal HashSet<BoardSquare> SquaresCanMoveToWithQueuedAbility
	{
		get
		{
			return this.m_squareCanMoveToWithQueuedAbility;
		}
	}

	public void UpdateSquaresCanMoveTo()
	{
		if (NetworkServer.active)
		{
		}
		float maxMoveDist = this.m_actor.RemainingHorizontalMovement;
		float innerMoveDist = this.m_actor.RemainingMovementWithQueuedAbility;
		BoardSquare squareToStartFrom = this.m_actor.MoveFromBoardSquare;
		bool flag;
		if (Options_UI.Get() != null)
		{
			if (Options_UI.Get().GetShiftClickForMovementWaypoints() != InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier))
			{
				flag = true;
				goto IL_97;
			}
		}
		flag = !FirstTurnMovement.CanWaypoint();
		IL_97:
		bool flag2 = flag;
		if (flag2)
		{
			if (this.m_actor == GameFlowData.Get().activeOwnedActorData)
			{
				maxMoveDist = this.CalculateMaxHorizontalMovement(false, false);
				innerMoveDist = this.CalculateMaxHorizontalMovement(true, false);
				squareToStartFrom = this.m_actor.InitialMoveStartSquare;
			}
		}
		this.GetSquaresCanMoveTo_InnerOuter(squareToStartFrom, maxMoveDist, innerMoveDist, out this.m_squaresCanMoveTo, out this.m_squareCanMoveToWithQueuedAbility);
		if (Board.Get() != null)
		{
			Board.Get().MarkForUpdateValidSquares(true);
		}
		if (this.m_actor == GameFlowData.Get().activeOwnedActorData)
		{
			LineData component = this.m_actor.GetComponent<LineData>();
			if (component != null)
			{
				component.OnCanMoveToSquaresUpdated();
			}
		}
	}

	public bool CanMoveToBoardSquare(int x, int y)
	{
		bool result = false;
		BoardSquare boardSquare = Board.Get().GetBoardSquare(x, y);
		if (boardSquare)
		{
			result = this.CanMoveToBoardSquare(boardSquare);
		}
		return result;
	}

	public bool CanMoveToBoardSquare(BoardSquare dest)
	{
		return this.m_squaresCanMoveTo.Contains(dest);
	}

	public BoardSquare GetClosestMoveableSquareTo(BoardSquare selectedSquare, bool alwaysIncludeMoverSquare = true)
	{
		BoardSquare boardSquare;
		if (alwaysIncludeMoverSquare)
		{
			boardSquare = this.m_actor.GetCurrentBoardSquare();
		}
		else
		{
			boardSquare = null;
		}
		BoardSquare boardSquare2 = boardSquare;
		float num;
		if (alwaysIncludeMoverSquare)
		{
			num = boardSquare2.HorizontalDistanceOnBoardTo(selectedSquare);
		}
		else
		{
			num = 100000f;
		}
		float num2 = num;
		if (selectedSquare != null)
		{
			using (HashSet<BoardSquare>.Enumerator enumerator = this.m_squaresCanMoveTo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BoardSquare boardSquare3 = enumerator.Current;
					float num3 = boardSquare3.HorizontalDistanceOnBoardTo(selectedSquare);
					if (num3 <= num2)
					{
						num2 = num3;
						boardSquare2 = boardSquare3;
					}
				}
			}
		}
		else
		{
			Log.Error("Trying to find the closest moveable square to a null square.  Code error-- tell Danny.", new object[0]);
		}
		return boardSquare2;
	}

	public float CalculateMaxHorizontalMovement(bool forcePostAbility = false, bool calculateAsIfSnared = false)
	{
		float num = 0f;
		if (!this.m_actor.IsDead())
		{
			num = (float)this.m_actor.GetActorStats().GetModifiedStatInt(StatType.Movement_Horizontal);
			AbilityData abilityData = this.m_actor.GetAbilityData();
			if (abilityData)
			{
				if (abilityData.GetQueuedAbilitiesAllowMovement())
				{
					float num2;
					if (forcePostAbility)
					{
						num2 = -1f * this.m_actor.GetPostAbilityHorizontalMovementChange();
					}
					else
					{
						num2 = abilityData.GetQueuedAbilitiesMovementAdjust();
					}
					num += num2;
					num = this.GetAdjustedMovementFromBuffAndDebuff(num, forcePostAbility, calculateAsIfSnared);
				}
				else
				{
					num = 0f;
				}
			}
			else
			{
				num = this.GetAdjustedMovementFromBuffAndDebuff(num, forcePostAbility, calculateAsIfSnared);
			}
			num = Mathf.Clamp(num, 0f, 99f);
		}
		return num;
	}

	public float GetAdjustedMovementFromBuffAndDebuff(float movement, bool forcePostAbility, bool calculateAsIfSnared = false)
	{
		float num = movement;
		ActorStatus actorStatus = this.m_actor.GetActorStatus();
		if (actorStatus.HasStatus(StatusType.RecentlySpawned, true))
		{
			num += (float)GameplayData.Get().m_recentlySpawnedBonusMovement;
		}
		if (actorStatus.HasStatus(StatusType.RecentlyRespawned, true))
		{
			num += (float)GameplayData.Get().m_recentlyRespawnedBonusMovement;
		}
		List<StatusType> queuedAbilitiesOnRequestStatuses = this.m_actor.GetAbilityData().GetQueuedAbilitiesOnRequestStatuses();
		bool flag = actorStatus.HasStatus(StatusType.MovementDebuffSuppression, true) || queuedAbilitiesOnRequestStatuses.Contains(StatusType.MovementDebuffSuppression);
		bool flag2;
		if (!actorStatus.IsMovementDebuffImmune(true) && !queuedAbilitiesOnRequestStatuses.Contains(StatusType.MovementDebuffImmunity))
		{
			flag2 = queuedAbilitiesOnRequestStatuses.Contains(StatusType.Unstoppable);
		}
		else
		{
			flag2 = true;
		}
		bool flag3 = flag2;
		bool flag4 = !flag && !flag3;
		bool flag5;
		if (!actorStatus.HasStatus(StatusType.CantSprint_UnlessUnstoppable, true))
		{
			flag5 = queuedAbilitiesOnRequestStatuses.Contains(StatusType.CantSprint_UnlessUnstoppable);
		}
		else
		{
			flag5 = true;
		}
		bool flag6 = flag5;
		bool flag7;
		if (!actorStatus.HasStatus(StatusType.CantSprint_Absolute, true))
		{
			flag7 = queuedAbilitiesOnRequestStatuses.Contains(StatusType.CantSprint_Absolute);
		}
		else
		{
			flag7 = true;
		}
		bool flag8 = flag7;
		bool flag9;
		if (flag6)
		{
			if (flag4)
			{
				flag9 = true;
				goto IL_129;
			}
		}
		flag9 = flag8;
		IL_129:
		bool flag10 = flag9;
		if (flag4)
		{
			if (actorStatus.HasStatus(StatusType.Rooted, true))
			{
				goto IL_165;
			}
		}
		if (!actorStatus.HasStatus(StatusType.AnchoredNoMovement, true))
		{
			if (flag4)
			{
				if (actorStatus.HasStatus(StatusType.CrippledMovement, true))
				{
					return Mathf.Clamp(num, 0f, 1f);
				}
			}
			if (flag10 && !forcePostAbility && this.m_actor.GetAbilityData() != null)
			{
				if (this.m_actor.GetAbilityData().GetQueuedAbilitiesMovementAdjustType() == Ability.MovementAdjustment.FullMovement)
				{
					num -= this.m_actor.GetPostAbilityHorizontalMovementChange();
				}
			}
			bool flag11;
			if (!actorStatus.HasStatus(StatusType.Snared, true))
			{
				flag11 = queuedAbilitiesOnRequestStatuses.Contains(StatusType.Snared);
			}
			else
			{
				flag11 = true;
			}
			bool flag12 = flag11;
			bool flag13;
			if (!actorStatus.HasStatus(StatusType.Hasted, true))
			{
				flag13 = queuedAbilitiesOnRequestStatuses.Contains(StatusType.Hasted);
			}
			else
			{
				flag13 = true;
			}
			bool flag14 = flag13;
			if (flag4)
			{
				if (flag12)
				{
					if (!flag14)
					{
						goto IL_28A;
					}
				}
			}
			if (calculateAsIfSnared)
			{
			}
			else
			{
				if (flag14)
				{
					if (flag4)
					{
						if (flag12)
						{
							return num;
						}
					}
					float num2;
					int num3;
					int num4;
					ActorMovement.CalcHastedMovementAdjustments(out num2, out num3, out num4);
					if (forcePostAbility)
					{
						num = Mathf.Clamp(num + (float)num3, 0f, 99f);
					}
					else
					{
						int num5 = num4;
						if (this.m_actor.GetAbilityData() != null)
						{
							Ability.MovementAdjustment queuedAbilitiesMovementAdjustType = this.m_actor.GetAbilityData().GetQueuedAbilitiesMovementAdjustType();
							if (queuedAbilitiesMovementAdjustType == Ability.MovementAdjustment.ReducedMovement)
							{
								num5 = num3;
							}
						}
						num = Mathf.Clamp(num + (float)num5, 0f, 99f);
					}
					num *= num2;
					return MovementUtils.RoundToNearestHalf(num);
				}
				return num;
			}
			IL_28A:
			float num6;
			int num7;
			int num8;
			ActorMovement.CalcSnaredMovementAdjustments(out num6, out num7, out num8);
			if (forcePostAbility)
			{
				num = Mathf.Clamp(num + (float)num7, 0f, 99f);
			}
			else
			{
				int num9 = num8;
				if (this.m_actor.GetAbilityData() != null)
				{
					Ability.MovementAdjustment queuedAbilitiesMovementAdjustType2 = this.m_actor.GetAbilityData().GetQueuedAbilitiesMovementAdjustType();
					if (queuedAbilitiesMovementAdjustType2 == Ability.MovementAdjustment.ReducedMovement)
					{
						num9 = num7;
					}
				}
				num = Mathf.Clamp(num + (float)num9, 0f, 99f);
			}
			num *= num6;
			return MovementUtils.RoundToNearestHalf(num);
		}
		IL_165:
		num = 0f;
		return num;
	}

	public unsafe static void CalcHastedMovementAdjustments(out float mult, out int halfMoveAdjustment, out int fullMoveAdjustment)
	{
		if (!(GameplayMutators.Get() == null))
		{
			if (GameplayMutators.Get().m_useHasteOverride)
			{
				halfMoveAdjustment = GameplayMutators.Get().m_hasteHalfMovementAdjustAmount;
				fullMoveAdjustment = GameplayMutators.Get().m_hasteFullMovementAdjustAmount;
				mult = GameplayMutators.Get().m_hasteMovementMultiplier;
				return;
			}
		}
		halfMoveAdjustment = GameWideData.Get().m_hasteHalfMovementAdjustAmount;
		fullMoveAdjustment = GameWideData.Get().m_hasteFullMovementAdjustAmount;
		mult = GameWideData.Get().m_hasteMovementMultiplier;
	}

	public unsafe static void CalcSnaredMovementAdjustments(out float mult, out int halfMoveAdjust, out int fullMoveAdjust)
	{
		if (!(GameplayMutators.Get() == null))
		{
			if (GameplayMutators.Get().m_useSlowOverride)
			{
				halfMoveAdjust = GameplayMutators.Get().m_slowHalfMovementAdjustAmount;
				fullMoveAdjust = GameplayMutators.Get().m_slowFullMovementAdjustAmount;
				mult = GameplayMutators.Get().m_slowMovementMultiplier;
				return;
			}
		}
		halfMoveAdjust = GameWideData.Get().m_slowHalfMovementAdjustAmount;
		fullMoveAdjust = GameWideData.Get().m_slowFullMovementAdjustAmount;
		mult = GameWideData.Get().m_slowMovementMultiplier;
	}

	public unsafe void BuildSquaresCanMoveTo_InnerOuter(BoardSquare squareToStartFrom, float maxHorizontalMovement, float innerMaxMove, out HashSet<BoardSquare> eligibleSquares, out HashSet<BoardSquare> innerSquares)
	{
		eligibleSquares = new HashSet<BoardSquare>();
		innerSquares = new HashSet<BoardSquare>();
		if (!(squareToStartFrom == null))
		{
			if (maxHorizontalMovement != 0f)
			{
				eligibleSquares.Add(squareToStartFrom);
				if (innerMaxMove > 0f)
				{
					innerSquares.Add(squareToStartFrom);
				}
				LinkedList<ActorMovement.BoardSquareMovementInfo> linkedList = new LinkedList<ActorMovement.BoardSquareMovementInfo>();
				HashSet<BoardSquare> hashSet = new HashSet<BoardSquare>();
				ActorMovement.BoardSquareMovementInfo value;
				value.square = squareToStartFrom;
				value.cost = 0f;
				value.prevCost = 0f;
				linkedList.AddLast(value);
				bool flag;
				if (GameplayData.Get() != null)
				{
					flag = (GameplayData.Get().m_movementMaximumType == GameplayData.MovementMaximumType.CannotExceedMax);
				}
				else
				{
					flag = false;
				}
				bool flag2 = flag;
				Board board = Board.Get();
				while (linkedList.Count > 0)
				{
					ActorMovement.BoardSquareMovementInfo value2 = linkedList.First.Value;
					BoardSquare square = value2.square;
					int x = square.x;
					int y = square.y;
					for (int i = -1; i <= 1; i++)
					{
						int j = -1;
						while (j <= 1)
						{
							if (i != 0)
							{
								goto IL_126;
							}
							if (j != 0)
							{
								goto IL_126;
							}
							IL_2E5:
							j++;
							continue;
							IL_126:
							BoardSquare boardSquare = board.GetBoardSquare(x + i, y + j);
							if (!(boardSquare == null))
							{
								if (hashSet.Contains(boardSquare))
								{
								}
								else
								{
									bool flag3 = board.symbol_0015(square, boardSquare);
									float num;
									if (flag3)
									{
										num = value2.cost + 1.5f;
									}
									else
									{
										num = value2.cost + 1f;
									}
									bool flag4;
									if (flag2)
									{
										flag4 = (num <= maxHorizontalMovement);
									}
									else
									{
										flag4 = (value2.cost < maxHorizontalMovement);
									}
									if (flag4)
									{
										BoardSquare src = square;
										BoardSquare dest = boardSquare;
										bool ignoreBarriers = false;
										ActorMovement.DiagonalCalcFlag diagonalFlag;
										if (flag3)
										{
											diagonalFlag = ActorMovement.DiagonalCalcFlag.IsDiagonal;
										}
										else
										{
											diagonalFlag = ActorMovement.DiagonalCalcFlag.NotDiagonal;
										}
										if (this.CanCrossToAdjacentSquare(src, dest, ignoreBarriers, diagonalFlag))
										{
											ActorMovement.BoardSquareMovementInfo value3;
											value3.square = boardSquare;
											value3.cost = num;
											value3.prevCost = value2.cost;
											bool flag5 = false;
											LinkedListNode<ActorMovement.BoardSquareMovementInfo> linkedListNode = linkedList.First;
											while (linkedListNode != linkedList.Last)
											{
												ActorMovement.BoardSquareMovementInfo value4 = linkedListNode.Value;
												if (value4.square == boardSquare)
												{
													flag5 = true;
													if (value4.cost > num)
													{
														linkedListNode.Value = value3;
													}
													else if (value4.cost == num && value3.prevCost < value4.prevCost)
													{
														linkedListNode.Value = value3;
													}
													IL_2C5:
													if (!flag5 && FirstTurnMovement.CanActorMoveToSquare(this.m_actor, boardSquare))
													{
														linkedList.AddLast(value3);
														goto IL_2E5;
													}
													goto IL_2E5;
												}
												else
												{
													linkedListNode = linkedListNode.Next;
												}
											}
											for (;;)
											{
												switch (3)
												{
												case 0:
													continue;
												}
												goto IL_2C5;
											}
										}
									}
								}
							}
							goto IL_2E5;
						}
					}
					if (MovementUtils.CanStopOnSquare(square) && SinglePlayerManager.IsDestinationAllowed(this.m_actor, square, true))
					{
						if (FirstTurnMovement.CanActorMoveToSquare(this.m_actor, square))
						{
							if (!eligibleSquares.Contains(square))
							{
								eligibleSquares.Add(square);
							}
							if (innerMaxMove > 0f && !innerSquares.Contains(square))
							{
								bool flag6;
								if (flag2)
								{
									flag6 = (value2.cost <= innerMaxMove);
								}
								else
								{
									flag6 = (value2.prevCost < innerMaxMove);
								}
								if (flag6)
								{
									innerSquares.Add(square);
								}
							}
						}
					}
					hashSet.Add(square);
					linkedList.RemoveFirst();
				}
				return;
			}
		}
	}

	public HashSet<BoardSquare> BuildSquaresCanMoveTo(BoardSquare squareToStartFrom, float maxHorizontalMovement)
	{
		HashSet<BoardSquare> hashSet = new HashSet<BoardSquare>();
		if (squareToStartFrom == null || maxHorizontalMovement == 0f)
		{
			return hashSet;
		}
		hashSet.Add(squareToStartFrom);
		LinkedList<ActorMovement.BoardSquareMovementInfo> linkedList = new LinkedList<ActorMovement.BoardSquareMovementInfo>();
		HashSet<BoardSquare> hashSet2 = new HashSet<BoardSquare>();
		ActorMovement.BoardSquareMovementInfo value;
		value.square = squareToStartFrom;
		value.cost = 0f;
		value.prevCost = 0f;
		linkedList.AddLast(value);
		List<BoardSquare> list = new List<BoardSquare>(8);
		while (linkedList.Count > 0)
		{
			ActorMovement.BoardSquareMovementInfo value2 = linkedList.First.Value;
			BoardSquare square = value2.square;
			list.Clear();
			if (!(GameplayData.Get() != null))
			{
				goto IL_DB;
			}
			if (GameplayData.Get().m_diagonalMovement != GameplayData.DiagonalMovement.Disabled)
			{
				goto IL_DB;
			}
			Board.Get().GetStraightAdjacentSquares(square.x, square.y, ref list);
			IL_FB:
			for (int i = 0; i < list.Count; i++)
			{
				BoardSquare boardSquare = list[i];
				if (!hashSet2.Contains(boardSquare))
				{
					bool flag = Board.Get().symbol_0015(square, boardSquare);
					float num;
					if (flag)
					{
						num = value2.cost + 1.5f;
					}
					else
					{
						num = value2.cost + 1f;
					}
					if (!(GameplayData.Get() != null))
					{
						goto IL_19A;
					}
					if (GameplayData.Get().m_movementMaximumType != GameplayData.MovementMaximumType.CannotExceedMax)
					{
						goto IL_19A;
					}
					bool flag2 = num <= maxHorizontalMovement;
					IL_1A6:
					if (!flag2)
					{
						goto IL_290;
					}
					ActorMovement.BoardSquareMovementInfo value3;
					value3.square = boardSquare;
					value3.cost = num;
					value3.prevCost = value2.cost;
					bool flag3 = false;
					LinkedListNode<ActorMovement.BoardSquareMovementInfo> linkedListNode = linkedList.First;
					while (linkedListNode != linkedList.Last)
					{
						ActorMovement.BoardSquareMovementInfo value4 = linkedListNode.Value;
						if (value4.square == boardSquare)
						{
							flag3 = true;
							if (value4.cost > num)
							{
								linkedListNode.Value = value3;
							}
							IL_245:
							if (flag3)
							{
								goto IL_290;
							}
							if (this.CanCrossToAdjacentSquare(square, boardSquare, false, (!flag) ? ActorMovement.DiagonalCalcFlag.NotDiagonal : ActorMovement.DiagonalCalcFlag.IsDiagonal) && FirstTurnMovement.CanActorMoveToSquare(this.m_actor, boardSquare))
							{
								linkedList.AddLast(value3);
								goto IL_290;
							}
							goto IL_290;
						}
						else
						{
							linkedListNode = linkedListNode.Next;
						}
					}
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						goto IL_245;
					}
					IL_19A:
					flag2 = (value2.cost < maxHorizontalMovement);
					goto IL_1A6;
				}
				IL_290:;
			}
			if (!hashSet.Contains(square))
			{
				if (MovementUtils.CanStopOnSquare(square))
				{
					if (SinglePlayerManager.IsDestinationAllowed(this.m_actor, square, true))
					{
						if (FirstTurnMovement.CanActorMoveToSquare(this.m_actor, square))
						{
							hashSet.Add(square);
						}
					}
				}
			}
			hashSet2.Add(square);
			linkedList.RemoveFirst();
			continue;
			IL_DB:
			Board.Get().GetAllAdjacentSquares(square.x, square.y, ref list);
			goto IL_FB;
		}
		return hashSet;
	}

	public HashSet<BoardSquare> CheckSquareCanMoveToCache(BoardSquare squareToStartFrom, float maxHorizontalMovement)
	{
		HashSet<BoardSquare> result = null;
		int squaresCanMoveToSinglePlayerState = -1;
		if (SinglePlayerManager.Get() != null)
		{
			if (this.m_actor.SpawnerId == -1)
			{
				squaresCanMoveToSinglePlayerState = SinglePlayerManager.Get().GetCurrentScriptIndex();
			}
		}
		int squaresCanMoveToBarrierState = -1;
		if (BarrierManager.Get() != null)
		{
			squaresCanMoveToBarrierState = BarrierManager.Get().GetMovementStateChangesFor(this.m_actor);
		}
		FirstTurnMovement.RestrictedMovementState squaresCanMoveToFirstTurnState = FirstTurnMovement.RestrictedMovementState.Invalid;
		if (FirstTurnMovement.Get() != null)
		{
			squaresCanMoveToFirstTurnState = FirstTurnMovement.Get().GetRestrictedMovementState();
		}
		ActorMovement.SquaresCanMoveToCacheEntry squaresCanMoveToCacheEntry = new ActorMovement.SquaresCanMoveToCacheEntry();
		squaresCanMoveToCacheEntry.m_squaresCanMoveToOrigin = squareToStartFrom;
		squaresCanMoveToCacheEntry.m_squaresCanMoveToHorizontalAllowed = maxHorizontalMovement;
		squaresCanMoveToCacheEntry.m_squaresCanMoveToSinglePlayerState = squaresCanMoveToSinglePlayerState;
		squaresCanMoveToCacheEntry.m_squaresCanMoveToBarrierState = squaresCanMoveToBarrierState;
		squaresCanMoveToCacheEntry.m_squaresCanMoveToFirstTurnState = squaresCanMoveToFirstTurnState;
		int num = 0;
		ActorMovement.SquaresCanMoveToCacheEntry item = null;
		for (int i = 0; i < this.m_squaresCanMoveToCache.Count; i++)
		{
			ActorMovement.SquaresCanMoveToCacheEntry squaresCanMoveToCacheEntry2 = this.m_squaresCanMoveToCache[i];
			if (squaresCanMoveToCacheEntry2.m_squaresCanMoveToOrigin == squaresCanMoveToCacheEntry.m_squaresCanMoveToOrigin)
			{
				if (squaresCanMoveToCacheEntry2.m_squaresCanMoveToHorizontalAllowed == squaresCanMoveToCacheEntry.m_squaresCanMoveToHorizontalAllowed)
				{
					if (squaresCanMoveToCacheEntry2.m_squaresCanMoveToSinglePlayerState == squaresCanMoveToCacheEntry.m_squaresCanMoveToSinglePlayerState)
					{
						if (squaresCanMoveToCacheEntry2.m_squaresCanMoveToBarrierState == squaresCanMoveToCacheEntry.m_squaresCanMoveToBarrierState && squaresCanMoveToCacheEntry2.m_squaresCanMoveToFirstTurnState == squaresCanMoveToCacheEntry.m_squaresCanMoveToFirstTurnState)
						{
							result = squaresCanMoveToCacheEntry2.m_squaresCanMoveTo;
							item = squaresCanMoveToCacheEntry2;
							num = i;
							IL_1A7:
							if (num != 0)
							{
								this.m_squaresCanMoveToCache.RemoveAt(num);
								this.m_squaresCanMoveToCache.Insert(0, item);
							}
							return result;
						}
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
			goto IL_1A7;
		}
	}

	public void AddToSquareCanMoveToCache(BoardSquare squareToStartFrom, float maxHorizontalMovement, HashSet<BoardSquare> squaresCanMoveTo)
	{
		int squaresCanMoveToBarrierState = -1;
		int squaresCanMoveToSinglePlayerState = -1;
		FirstTurnMovement.RestrictedMovementState squaresCanMoveToFirstTurnState = FirstTurnMovement.RestrictedMovementState.Invalid;
		if (SinglePlayerManager.Get() != null && this.m_actor.SpawnerId == -1)
		{
			squaresCanMoveToSinglePlayerState = SinglePlayerManager.Get().GetCurrentScriptIndex();
		}
		if (BarrierManager.Get() != null)
		{
			squaresCanMoveToBarrierState = BarrierManager.Get().GetMovementStateChangesFor(this.m_actor);
		}
		if (FirstTurnMovement.Get() != null)
		{
			squaresCanMoveToFirstTurnState = FirstTurnMovement.Get().GetRestrictedMovementState();
		}
		ActorMovement.SquaresCanMoveToCacheEntry squaresCanMoveToCacheEntry = new ActorMovement.SquaresCanMoveToCacheEntry();
		squaresCanMoveToCacheEntry.m_squaresCanMoveToOrigin = squareToStartFrom;
		squaresCanMoveToCacheEntry.m_squaresCanMoveToHorizontalAllowed = maxHorizontalMovement;
		squaresCanMoveToCacheEntry.m_squaresCanMoveToSinglePlayerState = squaresCanMoveToSinglePlayerState;
		squaresCanMoveToCacheEntry.m_squaresCanMoveToBarrierState = squaresCanMoveToBarrierState;
		squaresCanMoveToCacheEntry.m_squaresCanMoveToFirstTurnState = squaresCanMoveToFirstTurnState;
		squaresCanMoveToCacheEntry.m_squaresCanMoveTo = squaresCanMoveTo;
		if (this.m_squaresCanMoveToCache.Count >= ActorMovement.s_maxSquaresCanMoveToCacheCount)
		{
			this.m_squaresCanMoveToCache.RemoveAt(this.m_squaresCanMoveToCache.Count - 1);
		}
		this.m_squaresCanMoveToCache.Insert(0, squaresCanMoveToCacheEntry);
	}

	public HashSet<BoardSquare> GetSquaresCanMoveTo(BoardSquare squareToStartFrom, float maxHorizontalMovement)
	{
		HashSet<BoardSquare> hashSet = this.CheckSquareCanMoveToCache(squareToStartFrom, maxHorizontalMovement);
		if (hashSet != null)
		{
			return hashSet;
		}
		hashSet = this.BuildSquaresCanMoveTo(squareToStartFrom, maxHorizontalMovement);
		this.AddToSquareCanMoveToCache(squareToStartFrom, maxHorizontalMovement, hashSet);
		return hashSet;
	}

	public unsafe void GetSquaresCanMoveTo_InnerOuter(BoardSquare squareToStartFrom, float maxMoveDist, float innerMoveDist, out HashSet<BoardSquare> outMaxMoveSquares, out HashSet<BoardSquare> outInnerMoveSquares)
	{
		HashSet<BoardSquare> hashSet = this.CheckSquareCanMoveToCache(squareToStartFrom, maxMoveDist);
		HashSet<BoardSquare> hashSet2 = this.CheckSquareCanMoveToCache(squareToStartFrom, innerMoveDist);
		if (hashSet == null && hashSet2 == null)
		{
			this.BuildSquaresCanMoveTo_InnerOuter(squareToStartFrom, maxMoveDist, innerMoveDist, out hashSet, out hashSet2);
			this.AddToSquareCanMoveToCache(squareToStartFrom, maxMoveDist, hashSet);
			this.AddToSquareCanMoveToCache(squareToStartFrom, innerMoveDist, hashSet2);
		}
		else if (hashSet == null)
		{
			HashSet<BoardSquare> hashSet3;
			this.BuildSquaresCanMoveTo_InnerOuter(squareToStartFrom, maxMoveDist, 0f, out hashSet, out hashSet3);
			this.AddToSquareCanMoveToCache(squareToStartFrom, maxMoveDist, hashSet);
		}
		else if (hashSet2 == null)
		{
			HashSet<BoardSquare> hashSet4;
			this.BuildSquaresCanMoveTo_InnerOuter(squareToStartFrom, innerMoveDist, 0f, out hashSet2, out hashSet4);
			this.AddToSquareCanMoveToCache(squareToStartFrom, innerMoveDist, hashSet2);
		}
		outMaxMoveSquares = hashSet;
		outInnerMoveSquares = hashSet2;
	}

	public bool CanCrossToAdjacentSquare(BoardSquare src, BoardSquare dest, bool ignoreBarriers, ActorMovement.DiagonalCalcFlag diagonalFlag = ActorMovement.DiagonalCalcFlag.Unknown)
	{
		return this.CanCrossToAdjacentSingleSquare(src, dest, ignoreBarriers, true, diagonalFlag);
	}

	private bool CanCrossToAdjacentSingleSquare(BoardSquare src, BoardSquare dest, bool ignoreBarriers, bool knownAdjacent = false, ActorMovement.DiagonalCalcFlag diagonalFlag = ActorMovement.DiagonalCalcFlag.Unknown)
	{
		if (!(dest == null))
		{
			if (!dest.IsBaselineHeight())
			{
			}
			else
			{
				if (src.GetCoverInDirection(VectorUtils.GetCoverDirection(src, dest)) == ThinCover.CoverType.Full)
				{
					return false;
				}
				if (!ignoreBarriers)
				{
					if (BarrierManager.Get() != null)
					{
						if (BarrierManager.Get().IsMovementBlocked(this.m_actor, src, dest))
						{
							return false;
						}
					}
				}
				bool flag;
				if (!knownAdjacent)
				{
					flag = Board.Get().symbol_000E(src, dest);
				}
				else
				{
					flag = true;
				}
				if (!flag)
				{
					return false;
				}
				bool flag2 = true;
				if (diagonalFlag != ActorMovement.DiagonalCalcFlag.IsDiagonal)
				{
					if (diagonalFlag != ActorMovement.DiagonalCalcFlag.Unknown)
					{
						goto IL_18A;
					}
					if (!Board.Get().symbol_0015(src, dest))
					{
						goto IL_18A;
					}
				}
				BoardSquare boardSquare = Board.Get().GetBoardSquare(src.x, dest.y);
				BoardSquare boardSquare2 = Board.Get().GetBoardSquare(dest.x, src.y);
				if (flag2)
				{
					flag2 &= this.CanCrossToAdjacentSingleSquare(src, boardSquare, ignoreBarriers, true, ActorMovement.DiagonalCalcFlag.NotDiagonal);
				}
				if (flag2)
				{
					flag2 &= this.CanCrossToAdjacentSingleSquare(src, boardSquare2, ignoreBarriers, true, ActorMovement.DiagonalCalcFlag.NotDiagonal);
				}
				if (flag2)
				{
					flag2 &= this.CanCrossToAdjacentSingleSquare(boardSquare, dest, ignoreBarriers, true, ActorMovement.DiagonalCalcFlag.NotDiagonal);
				}
				if (flag2)
				{
					flag2 &= this.CanCrossToAdjacentSingleSquare(boardSquare2, dest, ignoreBarriers, true, ActorMovement.DiagonalCalcFlag.NotDiagonal);
				}
				IL_18A:
				if (!flag2)
				{
					return false;
				}
				return true;
			}
		}
		return false;
	}

	public void ClearPath()
	{
		if (this.m_gameplayPath != null)
		{
			this.m_gameplayPath = null;
		}
		this.Client_ClearAestheticPath();
	}

	private void Client_ClearAestheticPath()
	{
		if (this.m_aestheticPath != null)
		{
			this.m_aestheticPath = null;
		}
	}

	public Vector3 GetGroundPosition(Vector3 testPos)
	{
		Vector3 result = testPos;
		int layerMask = 1 << LayerMask.NameToLayer("LineOfSight");
		Vector3 origin = testPos;
		origin.y += 4f;
		float radius = 0.1f;
		RaycastHit raycastHit;
		if (Physics.SphereCast(origin, radius, Vector3.down, out raycastHit, 100f, layerMask))
		{
			result.y = raycastHit.point.y;
		}
		return result;
	}

	public bool ShouldSetForceIdle()
	{
		Animator modelAnimator = this.m_actor.GetModelAnimator();
		if (!(modelAnimator == null))
		{
			if (modelAnimator.layerCount >= 1)
			{
				AnimatorStateInfo currentAnimatorStateInfo = modelAnimator.GetCurrentAnimatorStateInfo(0);
				if (!currentAnimatorStateInfo.IsName("Run_Vault"))
				{
					if (!currentAnimatorStateInfo.IsName("Run_End"))
					{
						if (!currentAnimatorStateInfo.IsTag("Run_End"))
						{
							if (!currentAnimatorStateInfo.IsName("KnockbackEnd") && !currentAnimatorStateInfo.IsTag("Knockdown") && !currentAnimatorStateInfo.IsTag("Damage"))
							{
								return !this.m_actor.GetActorModelData().IsPlayingIdleAnim(false);
							}
						}
					}
				}
				return false;
			}
		}
		return false;
	}

	internal void OnGameStateChange(bool decisionState)
	{
		Animator modelAnimator = this.m_actor.GetModelAnimator();
		if (!decisionState)
		{
			if (this.m_brushTransitionAnimationSpeedEased.GetEndValue() != 1f)
			{
				this.m_brushTransitionAnimationSpeedEased.EaseTo(1f, 0f);
			}
		}
		while (this.m_gameplayPath != null)
		{
			if (this.m_gameplayPath.next == null || this.m_actor.IsDead())
			{
				break;
			}
			this.AdvanceGameplayPath();
		}
		if (this.m_curMoveState != null)
		{
			this.m_curMoveState = null;
			this.StopMovementAnimator();
		}
		if (modelAnimator != null)
		{
			if (modelAnimator.GetInteger(ActorMovement.animIdleType) < 0)
			{
				modelAnimator.SetInteger(ActorMovement.animIdleType, 0);
			}
		}
		if (this.ShouldSetForceIdle())
		{
			modelAnimator.SetTrigger(ActorMovement.animForceIdle);
		}
		if (this.m_actor.GetCurrentBoardSquare() != null)
		{
			this.m_actor.SetTransformPositionToSquare(this.m_actor.GetCurrentBoardSquare());
		}
		this.Client_ClearAestheticPath();
		this.m_moveTimeoutTime = -1f;
	}

	public void ResetChargeCycleFlag()
	{
		Animator modelAnimator = this.m_actor.GetModelAnimator();
		if (modelAnimator != null)
		{
			modelAnimator.SetInteger(ActorMovement.animNextChargeCycleType, 0);
			modelAnimator.SetInteger(ActorMovement.animChargeCycleType, 0);
			modelAnimator.SetInteger(ActorMovement.animChargeEndType, 4);
			modelAnimator.SetFloat(ActorMovement.animDistToWaypoint, 0f);
		}
	}

	private void UpdatePath()
	{
		bool flag = false;
		if (this.m_aestheticPath == null)
		{
			Log.Error(string.Format("{0} trying to UpdatePath with a null aesthetic path; exiting.", this.m_actor.DisplayName), new object[0]);
			return;
		}
		if (this.m_aestheticPath.next == null)
		{
			while (this.m_gameplayPath.next != null)
			{
				this.AdvanceGameplayPath();
				bool flag2;
				if (Application.isEditor && DebugParameters.Get() != null)
				{
					flag2 = DebugParameters.Get().GetParameterAsBool("SkipFogOfWarUpdateOnMovement");
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				flag = !flag3;
			}
			ActorCover actorCover = this.m_actor.GetActorCover();
			actorCover.RecalculateCover();
		}
		if (this.m_aestheticPath != null)
		{
			if (this.m_aestheticPath.next != null)
			{
				this.m_aestheticPath = this.m_aestheticPath.next;
				goto IL_108;
			}
		}
		this.m_aestheticPath = null;
		IL_108:
		if (flag)
		{
			this.m_actor.GetFogOfWar().MarkForRecalculateVisibility();
			this.UpdateClientFogOfWarIfNeeded();
		}
	}

	public bool AmMoving()
	{
		bool flag;
		if (this.m_curMoveState != null)
		{
			if (this.m_curMoveState is ChargeState)
			{
				ChargeState chargeState = this.m_curMoveState as ChargeState;
				if (chargeState.DoneMoving())
				{
					flag = false;
				}
				else
				{
					flag = true;
				}
			}
			else if (this.m_curMoveState.m_done)
			{
				flag = false;
			}
			else
			{
				flag = true;
			}
		}
		else
		{
			flag = false;
		}
		bool result;
		if (flag)
		{
			result = !this.ShouldPauseAnimator();
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool InChargeState()
	{
		return this.m_curMoveState != null && this.m_curMoveState is ChargeState;
	}

	internal float FindDistanceToEnd()
	{
		if (this.AmMoving())
		{
			if (this.m_gameplayPath != null)
			{
				return this.m_gameplayPath.FindDistanceToEnd();
			}
		}
		return 0f;
	}

	internal bool FindIsVisibleToClient()
	{
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (this.m_gameplayPath == null || clientFog == null)
		{
			return false;
		}
		for (BoardSquarePathInfo boardSquarePathInfo = this.m_gameplayPath; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			if (clientFog.IsVisible(boardSquarePathInfo.square))
			{
				return true;
			}
		}
		return false;
	}

	internal bool OnPathType(BoardSquarePathInfo.ConnectionType type)
	{
		bool result;
		if (this.m_aestheticPath != null)
		{
			result = (this.m_aestheticPath.connectionType == type);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public void OnMidMovementDeath()
	{
		this.UpdatePosition();
		if (this.m_curMoveState != null)
		{
			this.m_curMoveState.Update();
		}
		this.StopMovementAnimator();
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.ActiveControlChangedToEnemyTeam)
		{
			this.m_brushTransitionAnimationSpeedEased.EaseTo(1f, 0f);
		}
	}

	public void UpdatePosition()
	{
		if (this.m_curMoveState != null)
		{
			BoardSquarePathInfo gameplayPathClosestTo = this.GetGameplayPathClosestTo(this.m_actor.transform.position);
			bool flag = false;
			while (this.m_gameplayPath != gameplayPathClosestTo)
			{
				this.AdvanceGameplayPath();
				bool flag2;
				if (Application.isEditor && DebugParameters.Get() != null)
				{
					flag2 = DebugParameters.Get().GetParameterAsBool("SkipFogOfWarUpdateOnMovement");
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				flag = !flag3;
			}
			if (flag)
			{
				this.m_actor.GetFogOfWar().MarkForRecalculateVisibility();
				this.UpdateClientFogOfWarIfNeeded();
			}
			bool flag4;
			if (!this.m_actor.IsDead())
			{
				flag4 = !this.m_actor.IsModelAnimatorDisabled();
			}
			else
			{
				flag4 = false;
			}
			bool flag5 = flag4;
			if (flag5)
			{
				bool updatePath = this.m_curMoveState.m_updatePath;
				this.m_curMoveState.Update();
				this.UpdateValuesAnimator();
				if (!updatePath)
				{
					if (this.m_curMoveState.m_updatePath)
					{
						this.UpdatePath();
					}
				}
				if (this.m_curMoveState != null)
				{
					if (!this.m_curMoveState.m_done)
					{
						return;
					}
				}
				this.UpdateMovementState();
				this.UpdateValuesAnimator();
				if (this.m_curMoveState == null)
				{
					this.StopMovementAnimator();
				}
			}
		}
	}

	private void UpdateClientFogOfWarIfNeeded()
	{
		if (NetworkClient.active)
		{
			if (FogOfWar.GetClientFog() != null)
			{
				if (GameFlowData.Get() != null)
				{
					if (GameFlowData.Get().LocalPlayerData != null)
					{
						Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
						bool flag;
						if (this.m_actor.GetActorStatus() != null)
						{
							flag = this.m_actor.GetActorStatus().HasStatus(StatusType.Revealed, true);
						}
						else
						{
							flag = false;
						}
						bool flag2 = flag;
						bool flag3 = CaptureTheFlag.IsActorRevealedByFlag_Client(this.m_actor);
						if (this.m_actor.GetTeam() != teamViewing)
						{
							if (!flag2)
							{
								if (!flag3)
								{
									return;
								}
							}
							FogOfWar.GetClientFog().MarkForRecalculateVisibility();
						}
					}
				}
			}
		}
	}

	private bool ShouldPauseAnimator()
	{
		return false;
	}

	private void StopMovementAnimator()
	{
		Animator modelAnimator = this.m_actor.GetModelAnimator();
		if (modelAnimator == null)
		{
			return;
		}
		if (this.ShouldPauseAnimator())
		{
			if (this.m_brushTransitionAnimationSpeedEased.GetEndValue() != this.m_brushTransitionAnimationSpeed)
			{
				this.m_brushTransitionAnimationSpeedEased.EaseTo(this.m_brushTransitionAnimationSpeed, this.m_brushTransitionAnimationSpeedEaseTime);
			}
		}
		else
		{
			modelAnimator.SetInteger(ActorMovement.animNextLinkType, 0);
			modelAnimator.SetInteger(ActorMovement.animCurLinkType, 0);
			modelAnimator.SetFloat(ActorMovement.animDistToGoal, 0f);
			modelAnimator.SetFloat(ActorMovement.animTimeInAnim, 0f);
		}
		if (this.m_actor != null)
		{
			if (this.m_actor.GetActorModelData() != null)
			{
				this.m_actor.GetActorModelData().OnMovementAnimatorStop();
			}
		}
	}

	private float GetDistToPathSquare(BoardSquare goalGridSquare)
	{
		Vector3 position = this.m_actor.transform.position;
		Vector3 worldPosition = goalGridSquare.GetWorldPosition();
		Vector3 vector = worldPosition - position;
		vector.y = 0f;
		return vector.magnitude;
	}

	public Vector3 GetCurrentMovementDir()
	{
		Vector3 result;
		if (this.m_actor != null)
		{
			if (this.m_actor.transform != null)
			{
				if (this.m_aestheticPath != null && this.m_aestheticPath.square != null)
				{
					Vector3 position = this.m_actor.transform.position;
					Vector3 worldPosition = this.m_aestheticPath.square.GetWorldPosition();
					result = worldPosition - position;
					result.y = 0f;
					result.Normalize();
					return result;
				}
			}
		}
		result = Vector3.zero;
		return result;
	}

	private void UpdateValuesAnimator()
	{
		Animator modelAnimator = this.m_actor.GetModelAnimator();
		if (modelAnimator == null)
		{
			return;
		}
		float num;
		if (this.ShouldPauseAnimator())
		{
			num = this.m_brushTransitionAnimationSpeed;
		}
		else
		{
			num = 1f;
		}
		float num2 = num;
		if (this.m_brushTransitionAnimationSpeedEased.GetEndValue() != num2)
		{
			Eased<float> brushTransitionAnimationSpeedEased = this.m_brushTransitionAnimationSpeedEased;
			float endValue = num2;
			float duration;
			if (num2 == 1f)
			{
				duration = 0f;
			}
			else
			{
				duration = this.m_brushTransitionAnimationSpeedEaseTime;
			}
			brushTransitionAnimationSpeedEased.EaseTo(endValue, duration);
		}
		if (this.m_curMoveState != null)
		{
			modelAnimator.SetFloat(ActorMovement.animDistToGoal, this.GetDistToGoal());
			modelAnimator.SetFloat(ActorMovement.animTimeInAnim, this.m_curMoveState.m_timeInAnim);
			if (this.m_curMoveState.m_forceAnimReset)
			{
				if (this.ShouldSetForceIdle())
				{
					modelAnimator.SetTrigger(ActorMovement.animForceIdle);
				}
			}
		}
		modelAnimator.SetInteger(ActorMovement.animCurLinkType, MovementUtils.GetLinkType(this.m_aestheticPath));
		if (this.m_aestheticPath != null)
		{
			modelAnimator.SetInteger(ActorMovement.animNextLinkType, MovementUtils.GetLinkType(this.m_aestheticPath.next));
			modelAnimator.SetInteger(ActorMovement.animChargeCycleType, (int)this.m_aestheticPath.chargeCycleType);
			modelAnimator.SetInteger(ActorMovement.animChargeEndType, (int)this.m_aestheticPath.chargeEndType);
			modelAnimator.SetFloat(ActorMovement.animDistToWaypoint, this.GetDistToPathSquare(this.m_aestheticPath.square));
			if (this.m_aestheticPath.next != null)
			{
				modelAnimator.SetInteger(ActorMovement.animNextChargeCycleType, (int)this.m_aestheticPath.next.chargeCycleType);
			}
		}
	}

	private bool EnemyRunningIntoBrush(BoardSquarePathInfo pathEndInfo)
	{
		if (!this.m_actor.VisibleTillEndOfPhase && !this.m_actor.CurrentlyVisibleForAbilityCast && !this.m_actor.GetActorStatus().HasStatus(StatusType.Revealed, false))
		{
			if (!CaptureTheFlag.IsActorRevealedByFlag_Client(this.m_actor))
			{
				if (pathEndInfo == null)
				{
					if (!this.m_actor.IsVisibleToClient())
					{
						if (this.m_actor.IsHiddenInBrush())
						{
							if (this.m_actor.GetActorMovement().IsPast2ndToLastSquare())
							{
								goto IL_13B;
							}
						}
					}
				}
				if (pathEndInfo == null)
				{
					goto IL_159;
				}
				if (pathEndInfo.m_visibleToEnemies)
				{
					goto IL_159;
				}
				if (pathEndInfo.m_moverHasGameplayHitHere || !(pathEndInfo.square != null))
				{
					goto IL_159;
				}
				if (!(BrushCoordinator.Get() != null) || !BrushCoordinator.Get().IsRegionFunctioning(pathEndInfo.square.BrushRegion))
				{
					goto IL_159;
				}
				IL_13B:
				return ServerClientUtils.GetCurrentActionPhase() >= ActionBufferPhase.Movement || GameFlowData.Get().gameState == GameState.BothTeams_Decision;
			}
		}
		IL_159:
		return false;
	}

	private float GetDistToGoal()
	{
		float num = 0f;
		Vector3 b = base.transform.position;
		BoardSquarePathInfo boardSquarePathInfo = this.m_aestheticPath;
		BoardSquarePathInfo pathEndInfo = this.m_aestheticPath;
		while (boardSquarePathInfo != null)
		{
			Vector3 vector = boardSquarePathInfo.square.ToVector3();
			num += (vector - b).magnitude;
			b = vector;
			boardSquarePathInfo = boardSquarePathInfo.next;
			if (boardSquarePathInfo != null)
			{
				pathEndInfo = boardSquarePathInfo;
			}
		}
		if (this.EnemyRunningIntoBrush(pathEndInfo))
		{
			num += 999f;
		}
		return num;
	}

	private float GetDistToGround()
	{
		Vector3 groundPosition = this.GetGroundPosition(this.m_actor.transform.position);
		return this.m_actor.transform.position.y - groundPosition.y;
	}

	private void OnDeath()
	{
		this.UpdateMovementState();
	}

	private void UpdateMovementState()
	{
		if (this.m_actor.IsDead())
		{
			this.Client_ClearAestheticPath();
		}
		if (this.m_actor.IsModelAnimatorDisabled())
		{
			this.Client_ClearAestheticPath();
		}
		if (this.m_aestheticPath != null)
		{
			if (this.m_curMoveState != null)
			{
				if (this.m_curMoveState.m_connectionType == BoardSquarePathInfo.ConnectionType.Run)
				{
					if (this.m_aestheticPath.connectionType == BoardSquarePathInfo.ConnectionType.Run)
					{
						goto IL_CA;
					}
				}
			}
			Animator modelAnimator = this.m_actor.GetModelAnimator();
			if (modelAnimator != null)
			{
				modelAnimator.SetTrigger(ActorMovement.animMoveSegmentStart);
			}
			IL_CA:
			switch (this.m_aestheticPath.connectionType)
			{
			case BoardSquarePathInfo.ConnectionType.Run:
			case BoardSquarePathInfo.ConnectionType.Vault:
				this.m_curMoveState = new RunState(this, this.m_aestheticPath);
				break;
			case BoardSquarePathInfo.ConnectionType.Knockback:
				this.m_curMoveState = new KnockbackState(this, this.m_aestheticPath);
				break;
			case BoardSquarePathInfo.ConnectionType.Charge:
			case BoardSquarePathInfo.ConnectionType.Flight:
				this.m_curMoveState = new ChargeState(this, this.m_aestheticPath);
				break;
			}
			if (this.m_actor != null)
			{
				if (this.m_actor.GetActorModelData() != null)
				{
					this.m_actor.GetActorModelData().OnMovementAnimatorUpdate(this.m_aestheticPath.connectionType);
				}
			}
		}
		else if (this.m_curMoveState != null)
		{
			Animator modelAnimator2 = this.m_actor.GetModelAnimator();
			if (modelAnimator2 != null)
			{
				modelAnimator2.SetTrigger(ActorMovement.animMoveSegmentStart);
			}
			this.m_curMoveState = null;
			this.StopMovementAnimator();
		}
	}

	private BoardSquarePathInfo GetGameplayPathClosestTo(Vector3 pos)
	{
		Vector3 b = new Vector3(pos.x, 0f, pos.z);
		BoardSquarePathInfo boardSquarePathInfo;
		if (this.m_gameplayPath == null)
		{
			boardSquarePathInfo = null;
		}
		else
		{
			boardSquarePathInfo = this.m_gameplayPath.next;
		}
		BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo;
		BoardSquarePathInfo result = this.m_gameplayPath;
		if (this.m_gameplayPath != null)
		{
			Vector3 a = new Vector3(this.m_gameplayPath.square.transform.position.x, 0f, this.m_gameplayPath.square.transform.position.z);
			float num = (a - b).sqrMagnitude;
			while (boardSquarePathInfo2 != null)
			{
				Vector3 a2 = new Vector3(boardSquarePathInfo2.square.transform.position.x, 0f, boardSquarePathInfo2.square.transform.position.z);
				float sqrMagnitude = (a2 - b).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					result = boardSquarePathInfo2;
					num = sqrMagnitude;
				}
				boardSquarePathInfo2 = boardSquarePathInfo2.next;
			}
		}
		return result;
	}

	private void AdvanceGameplayPath()
	{
		this.m_gameplayPath = this.m_gameplayPath.next;
		BoardSquare square = this.m_gameplayPath.square;
		bool flag = this.m_gameplayPath.next == null;
		this.m_actor.ActorData_OnActorMoved(square, this.m_gameplayPath.m_visibleToEnemies, this.m_gameplayPath.m_updateLastKnownPos);
		ActorVFX actorVFX = this.m_actor.GetActorVFX();
		if (actorVFX != null)
		{
			actorVFX.OnMove(this.m_gameplayPath, this.m_gameplayPath.prev);
		}
		CameraManager.Get().OnActorMoved(this.m_actor);
		ClientClashManager.Get().OnActorMoved_ClientClashManager(this.m_actor, this.m_gameplayPath);
		ClientResolutionManager.Get().OnActorMoved_ClientResolutionManager(this.m_actor, this.m_gameplayPath);
		if (this.m_actor != null)
		{
			if (this.m_actor.GetActorModelData() != null)
			{
				this.m_actor.GetActorModelData().OnMovementAnimatorUpdate(this.m_aestheticPath.connectionType);
			}
		}
		if (flag)
		{
			this.m_actor.UpdateFacingAfterMovement();
			if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve && HighlightUtils.Get() != null)
			{
				if (HighlightUtils.Get().m_coverDirIndicatorTiming == HighlightUtils.MoveIntoCoverIndicatorTiming.ShowOnMoveEnd && HighlightUtils.Get().m_showMoveIntoCoverIndicators)
				{
					ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
					if (activeOwnedActorData != null)
					{
						if (activeOwnedActorData == this.m_actor)
						{
							if (this.m_actor.GetActorCover() != null)
							{
								if (this.m_actor.IsVisibleToClient())
								{
									this.m_actor.GetActorCover().RecalculateCover();
									this.m_actor.GetActorCover().StartShowMoveIntoCoverIndicator();
								}
							}
						}
					}
				}
			}
		}
	}

	private void CalculateMoveTimeout()
	{
		this.m_moveTimeoutTime = 0f;
		BoardSquarePathInfo boardSquarePathInfo = this.m_gameplayPath;
		ActorData actor = this.m_actor;
		while (boardSquarePathInfo != null)
		{
			BoardSquarePathInfo prev = boardSquarePathInfo.prev;
			Vector3 a;
			if (prev == null)
			{
				a = this.m_actor.transform.position;
			}
			else
			{
				a = prev.square.ToVector3();
			}
			switch (boardSquarePathInfo.connectionType)
			{
			case BoardSquarePathInfo.ConnectionType.Run:
			case BoardSquarePathInfo.ConnectionType.Knockback:
			case BoardSquarePathInfo.ConnectionType.Charge:
			case BoardSquarePathInfo.ConnectionType.Vault:
				if (boardSquarePathInfo.segmentMovementDuration > 0f)
				{
					this.m_moveTimeoutTime += boardSquarePathInfo.segmentMovementDuration * 1.5f + 3f;
				}
				else
				{
					Vector3 vector = a - boardSquarePathInfo.square.ToVector3();
					float num;
					if (boardSquarePathInfo.segmentMovementSpeed > 0f)
					{
						num = boardSquarePathInfo.segmentMovementSpeed;
					}
					else
					{
						num = actor.m_runSpeed;
					}
					float num2 = num;
					this.m_moveTimeoutTime += vector.magnitude / num2 * 1.5f + 3f;
				}
				break;
			}
			boardSquarePathInfo = boardSquarePathInfo.next;
		}
	}

	public void BeginTravellingAlongPath(BoardSquarePathInfo gameplayPath, ActorData.MovementType movementType)
	{
		this.m_actor.SetTransformPositionToSquare(gameplayPath.square);
		this.m_actor.m_endVisibilityForHitTime = 0f;
		BoardSquare travelBoardSquare = this.GetTravelBoardSquare();
		this.m_gameplayPath = gameplayPath;
		if (travelBoardSquare != this.GetTravelBoardSquare())
		{
			this.m_actor.ForceUpdateIsVisibleToClientCache();
			this.m_actor.ForceUpdateActorModelVisibility();
		}
		this.m_aestheticPath = this.m_gameplayPath.Clone(null);
		if (movementType == ActorData.MovementType.Normal)
		{
			MovementUtils.CreateRunAndVaultAestheticPath(ref this.m_aestheticPath, this.m_actor);
		}
		else
		{
			MovementUtils.CreateUnskippableAestheticPath(ref this.m_aestheticPath, movementType);
		}
		this.CalculateMoveTimeout();
		this.m_actor.SetTransformPositionToSquare(gameplayPath.square);
		this.UpdateMovementState();
	}

	public void BeginChargeOrKnockback(BoardSquare src, BoardSquare dest, BoardSquarePathInfo gameplayPath, ActorData.MovementType movementType)
	{
		if (movementType == ActorData.MovementType.Knockback)
		{
			GameEventManager.Get().FireEvent(GameEventManager.EventType.ActorKnockback, new GameEventManager.ActorKnockback
			{
				m_target = this.m_actor
			});
		}
		if (src == null && dest != null)
		{
			src = dest;
		}
		else if (src != null)
		{
			if (dest == null)
			{
				dest = src;
			}
		}
		if (src != null)
		{
			if (dest != null)
			{
				this.m_actor.SetTransformPositionToSquare(src);
				this.m_gameplayPath = gameplayPath;
				this.m_aestheticPath = this.m_gameplayPath.Clone(null);
				MovementUtils.CreateUnskippableAestheticPath(ref this.m_aestheticPath, movementType);
				this.CalculateMoveTimeout();
				this.m_actor.SetTransformPositionToSquare(src);
				this.UpdateMovementState();
			}
		}
	}

	public BoardSquarePathInfo BuildPathTo(BoardSquare src, BoardSquare dest)
	{
		float maxHorizontalMovement = this.CalculateMaxHorizontalMovement(false, false);
		return this.BuildPathTo(src, dest, maxHorizontalMovement, false, null);
	}

	public BoardSquarePathInfo BuildPathTo_IgnoreBarriers(BoardSquare src, BoardSquare dest)
	{
		float maxHorizontalMovement = this.CalculateMaxHorizontalMovement(false, false);
		return this.BuildPathTo(src, dest, maxHorizontalMovement, true, null);
	}

	public BoardSquarePathInfo BuildPathTo(BoardSquare src, BoardSquare dest, float maxHorizontalMovement, bool ignoreBarriers, List<BoardSquare> claimedSquares)
	{
		BoardSquarePathInfo boardSquarePathInfo = null;
		if (!(src == null))
		{
			if (!(dest == null))
			{
				Board board = Board.Get();
				BuildNormalPathNodePool normalPathBuildScratchPool = board.m_normalPathBuildScratchPool;
				normalPathBuildScratchPool.ResetAvailableNodeIndex();
				Vector3 tieBreakerDir = dest.ToVector3() - src.ToVector3();
				Vector3 tieBreakerPos = src.ToVector3();
				BuildNormalPathHeap normalPathNodeHeap = board.m_normalPathNodeHeap;
				normalPathNodeHeap.Clear();
				normalPathNodeHeap.SetTieBreakerDirAndPos(tieBreakerDir, tieBreakerPos);
				this.m_tempClosedSquares.Clear();
				Dictionary<BoardSquare, BoardSquarePathInfo> tempClosedSquares = this.m_tempClosedSquares;
				BoardSquarePathInfo allocatedNode = normalPathBuildScratchPool.GetAllocatedNode();
				allocatedNode.square = src;
				normalPathNodeHeap.Insert(allocatedNode);
				List<BoardSquare> list = new List<BoardSquare>(8);
				bool flag = GameplayData.Get().m_diagonalMovement != GameplayData.DiagonalMovement.Disabled;
				bool flag2 = GameplayData.Get().m_movementMaximumType != GameplayData.MovementMaximumType.CannotExceedMax;
				bool flag3;
				if (claimedSquares != null)
				{
					flag3 = claimedSquares.Contains(dest);
				}
				else
				{
					flag3 = false;
				}
				bool flag4 = flag3;
				while (!normalPathNodeHeap.IsEmpty())
				{
					BoardSquarePathInfo boardSquarePathInfo2 = normalPathNodeHeap.ExtractTop();
					if (boardSquarePathInfo2.square == dest)
					{
						boardSquarePathInfo = boardSquarePathInfo2;
						break;
					}
					tempClosedSquares[boardSquarePathInfo2.square] = boardSquarePathInfo2;
					list.Clear();
					if (!flag)
					{
						board.GetStraightAdjacentSquares(boardSquarePathInfo2.square.x, boardSquarePathInfo2.square.y, ref list);
					}
					else
					{
						board.GetAllAdjacentSquares(boardSquarePathInfo2.square.x, boardSquarePathInfo2.square.y, ref list);
					}
					for (int i = 0; i < list.Count; i++)
					{
						BoardSquare boardSquare = list[i];
						bool flag5 = board.symbol_0015(boardSquarePathInfo2.square, boardSquare);
						float num;
						if (flag5)
						{
							num = boardSquarePathInfo2.moveCost + 1.5f;
						}
						else
						{
							num = boardSquarePathInfo2.moveCost + 1f;
						}
						bool flag6;
						if (!flag2)
						{
							flag6 = (num <= maxHorizontalMovement);
						}
						else
						{
							flag6 = (boardSquarePathInfo2.moveCost < maxHorizontalMovement);
						}
						if (!flag6)
						{
						}
						else
						{
							BoardSquare square = boardSquarePathInfo2.square;
							BoardSquare dest2 = boardSquare;
							ActorMovement.DiagonalCalcFlag diagonalFlag;
							if (flag5)
							{
								diagonalFlag = ActorMovement.DiagonalCalcFlag.IsDiagonal;
							}
							else
							{
								diagonalFlag = ActorMovement.DiagonalCalcFlag.NotDiagonal;
							}
							if (!this.CanCrossToAdjacentSquare(square, dest2, ignoreBarriers, diagonalFlag))
							{
							}
							else if (!FirstTurnMovement.CanActorMoveToSquare(this.m_actor, boardSquare))
							{
							}
							else
							{
								BoardSquarePathInfo allocatedNode2 = normalPathBuildScratchPool.GetAllocatedNode();
								allocatedNode2.square = boardSquare;
								allocatedNode2.moveCost = num;
								if (claimedSquares != null && flag4 && allocatedNode2.square == dest)
								{
									int num2 = 1;
									BoardSquarePathInfo boardSquarePathInfo3 = boardSquarePathInfo2;
									while (boardSquarePathInfo3 != null)
									{
										if (!claimedSquares.Contains(boardSquarePathInfo3.square))
										{
											for (;;)
											{
												switch (2)
												{
												case 0:
													continue;
												}
												goto IL_2D8;
											}
										}
										else
										{
											num2++;
											boardSquarePathInfo3 = boardSquarePathInfo3.prev;
										}
									}
									IL_2D8:
									allocatedNode2.m_expectedBackupNum = num2;
								}
								float squareSize = board.squareSize;
								if (!flag2)
								{
									allocatedNode2.heuristicCost = (boardSquare.transform.position - dest.transform.position).magnitude / squareSize;
								}
								else
								{
									float num3 = (float)Mathf.Abs(boardSquare.x - dest.x);
									float num4 = (float)Mathf.Abs(boardSquare.y - dest.y);
									float num5 = num3 + num4 - 0.5f * Mathf.Min(num3, num4);
									float heuristicCost = Mathf.Max(0f, num5 - 1.01f);
									allocatedNode2.heuristicCost = heuristicCost;
								}
								allocatedNode2.prev = boardSquarePathInfo2;
								bool flag7 = false;
								if (tempClosedSquares.ContainsKey(allocatedNode2.square))
								{
									flag7 = (allocatedNode2.F_cost > tempClosedSquares[allocatedNode2.square].F_cost);
								}
								if (flag7)
								{
								}
								else
								{
									bool flag8 = false;
									BoardSquarePathInfo boardSquarePathInfo4 = normalPathNodeHeap.TryGetNodeInHeapBySquare(allocatedNode2.square);
									if (boardSquarePathInfo4 != null)
									{
										flag8 = true;
										if (allocatedNode2.F_cost < boardSquarePathInfo4.F_cost)
										{
											normalPathNodeHeap.UpdatePriority(allocatedNode2);
										}
									}
									if (!flag8)
									{
										normalPathNodeHeap.Insert(allocatedNode2);
									}
								}
							}
						}
					}
				}
				if (boardSquarePathInfo != null)
				{
					while (boardSquarePathInfo.prev != null)
					{
						boardSquarePathInfo.prev.next = boardSquarePathInfo;
						boardSquarePathInfo = boardSquarePathInfo.prev;
					}
					boardSquarePathInfo = boardSquarePathInfo.Clone(null);
					normalPathBuildScratchPool.ResetAvailableNodeIndex();
				}
				return boardSquarePathInfo;
			}
		}
		return boardSquarePathInfo;
	}

	public BoardSquarePathInfo BuildPathTo_Orig(BoardSquare src, BoardSquare dest, float maxHorizontalMovement, bool ignoreBarriers, List<BoardSquare> claimedSquares)
	{
		BoardSquarePathInfo boardSquarePathInfo = null;
		if (!(src == null))
		{
			if (dest == null)
			{
			}
			else
			{
				Vector3 tieBreakDir = dest.ToVector3() - src.ToVector3();
				Vector3 tieBreakTestPos = src.ToVector3();
				BuildNormalPathNodePool normalPathBuildScratchPool = Board.Get().m_normalPathBuildScratchPool;
				normalPathBuildScratchPool.ResetAvailableNodeIndex();
				List<BoardSquarePathInfo> list = new List<BoardSquarePathInfo>();
				Dictionary<BoardSquare, BoardSquarePathInfo> dictionary = new Dictionary<BoardSquare, BoardSquarePathInfo>();
				BoardSquarePathInfo allocatedNode = normalPathBuildScratchPool.GetAllocatedNode();
				allocatedNode.square = src;
				list.Add(allocatedNode);
				List<BoardSquare> list2 = new List<BoardSquare>(8);
				bool flag = GameplayData.Get().m_diagonalMovement != GameplayData.DiagonalMovement.Disabled;
				bool flag2 = GameplayData.Get().m_movementMaximumType != GameplayData.MovementMaximumType.CannotExceedMax;
				bool flag3;
				if (claimedSquares != null)
				{
					flag3 = claimedSquares.Contains(dest);
				}
				else
				{
					flag3 = false;
				}
				bool flag4 = flag3;
				while (list.Count > 0)
				{
					list.Sort(delegate(BoardSquarePathInfo p1, BoardSquarePathInfo p2)
					{
						if (Mathf.Approximately(p1.F_cost, p2.F_cost))
						{
							Vector3 from = p1.square.ToVector3() - tieBreakTestPos;
							Vector3 from2 = p2.square.ToVector3() - tieBreakTestPos;
							return Vector3.Angle(from, tieBreakDir).CompareTo(Vector3.Angle(from2, tieBreakDir));
						}
						return p1.F_cost.CompareTo(p2.F_cost);
					});
					BoardSquarePathInfo boardSquarePathInfo2 = list[0];
					tieBreakTestPos = boardSquarePathInfo2.square.ToVector3();
					if (boardSquarePathInfo2.prev != null)
					{
						tieBreakDir = boardSquarePathInfo2.square.ToVector3() - boardSquarePathInfo2.prev.square.ToVector3();
					}
					if (boardSquarePathInfo2.square == dest)
					{
						boardSquarePathInfo = boardSquarePathInfo2;
						IL_56F:
						if (boardSquarePathInfo != null)
						{
							while (boardSquarePathInfo.prev != null)
							{
								boardSquarePathInfo.prev.next = boardSquarePathInfo;
								boardSquarePathInfo = boardSquarePathInfo.prev;
							}
							boardSquarePathInfo = boardSquarePathInfo.Clone(null);
							normalPathBuildScratchPool.ResetAvailableNodeIndex();
						}
						return boardSquarePathInfo;
					}
					dictionary[boardSquarePathInfo2.square] = boardSquarePathInfo2;
					list.Remove(boardSquarePathInfo2);
					list2.Clear();
					if (!flag)
					{
						Board.Get().GetStraightAdjacentSquares(boardSquarePathInfo2.square.x, boardSquarePathInfo2.square.y, ref list2);
					}
					else
					{
						Board.Get().GetAllAdjacentSquares(boardSquarePathInfo2.square.x, boardSquarePathInfo2.square.y, ref list2);
					}
					for (int i = 0; i < list2.Count; i++)
					{
						BoardSquare boardSquare = list2[i];
						bool flag5 = Board.Get().symbol_0015(boardSquarePathInfo2.square, boardSquare);
						float num;
						if (flag5)
						{
							num = boardSquarePathInfo2.moveCost + 1.5f;
						}
						else
						{
							num = boardSquarePathInfo2.moveCost + 1f;
						}
						bool flag6;
						if (!flag2)
						{
							flag6 = (num <= maxHorizontalMovement);
						}
						else
						{
							flag6 = (boardSquarePathInfo2.moveCost < maxHorizontalMovement);
						}
						if (flag6)
						{
							BoardSquare square = boardSquarePathInfo2.square;
							BoardSquare dest2 = boardSquare;
							ActorMovement.DiagonalCalcFlag diagonalFlag;
							if (flag5)
							{
								diagonalFlag = ActorMovement.DiagonalCalcFlag.IsDiagonal;
							}
							else
							{
								diagonalFlag = ActorMovement.DiagonalCalcFlag.NotDiagonal;
							}
							if (!this.CanCrossToAdjacentSquare(square, dest2, ignoreBarriers, diagonalFlag))
							{
							}
							else if (!FirstTurnMovement.CanActorMoveToSquare(this.m_actor, boardSquare))
							{
							}
							else
							{
								BoardSquarePathInfo allocatedNode2 = normalPathBuildScratchPool.GetAllocatedNode();
								allocatedNode2.square = boardSquare;
								allocatedNode2.moveCost = num;
								if (claimedSquares != null && flag4)
								{
									if (allocatedNode2.square == dest)
									{
										int num2 = 1;
										BoardSquarePathInfo boardSquarePathInfo3 = boardSquarePathInfo2;
										while (boardSquarePathInfo3 != null)
										{
											if (!claimedSquares.Contains(boardSquarePathInfo3.square))
											{
												for (;;)
												{
													switch (5)
													{
													case 0:
														continue;
													}
													goto IL_372;
												}
											}
											else
											{
												num2++;
												boardSquarePathInfo3 = boardSquarePathInfo3.prev;
											}
										}
										IL_372:
										allocatedNode2.m_expectedBackupNum = num2;
									}
								}
								float squareSize = Board.Get().squareSize;
								if (!flag2)
								{
									allocatedNode2.heuristicCost = (boardSquare.transform.position - dest.transform.position).magnitude / squareSize;
								}
								else
								{
									float num3 = (float)Mathf.Abs(boardSquare.x - dest.x);
									float num4 = (float)Mathf.Abs(boardSquare.y - dest.y);
									float num5 = num3 + num4 - 0.5f * Mathf.Min(num3, num4);
									float heuristicCost = Mathf.Max(0f, num5 - 1.01f);
									allocatedNode2.heuristicCost = heuristicCost;
								}
								allocatedNode2.prev = boardSquarePathInfo2;
								bool flag7 = false;
								if (dictionary.ContainsKey(allocatedNode2.square))
								{
									flag7 = (allocatedNode2.F_cost > dictionary[allocatedNode2.square].F_cost);
								}
								if (!flag7)
								{
									bool flag8 = false;
									for (int j = 0; j < list.Count; j++)
									{
										BoardSquarePathInfo boardSquarePathInfo4 = list[j];
										if (boardSquarePathInfo4.square == allocatedNode2.square)
										{
											flag8 = true;
											if (allocatedNode2.F_cost < boardSquarePathInfo4.F_cost)
											{
												boardSquarePathInfo4.heuristicCost = allocatedNode2.heuristicCost;
												boardSquarePathInfo4.moveCost = allocatedNode2.moveCost;
												boardSquarePathInfo4.prev = allocatedNode2.prev;
												boardSquarePathInfo4.m_expectedBackupNum = allocatedNode2.m_expectedBackupNum;
											}
											break;
										}
									}
									if (!flag8)
									{
										list.Add(allocatedNode2);
									}
								}
							}
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
					goto IL_56F;
				}
			}
		}
		return boardSquarePathInfo;
	}

	public BoardSquarePathInfo GetAestheticPath()
	{
		BoardSquarePathInfo result;
		if (this.m_curMoveState == null)
		{
			result = null;
		}
		else
		{
			result = this.m_curMoveState.GetAestheticPath();
		}
		return result;
	}

	public BoardSquare GetTravelBoardSquare()
	{
		BoardSquare boardSquare = null;
		if (this.m_gameplayPath != null)
		{
			boardSquare = this.m_gameplayPath.square;
		}
		if (boardSquare == null)
		{
			boardSquare = this.m_actor.GetCurrentBoardSquare();
		}
		return boardSquare;
	}

	internal BoardSquarePathInfo GetPreviousTravelBoardSquarePathInfo()
	{
		if (this.m_gameplayPath != null)
		{
			if (this.m_gameplayPath.prev != null)
			{
				return this.m_gameplayPath.prev;
			}
		}
		return this.m_gameplayPath;
	}

	internal BoardSquarePathInfo GetNextTravelBoardSquarePathInfo()
	{
		if (this.m_gameplayPath != null)
		{
			if (this.m_gameplayPath.next != null)
			{
				return this.m_gameplayPath.next;
			}
		}
		return this.m_gameplayPath;
	}

	public unsafe void EncapsulatePathInBound(ref Bounds bound)
	{
		if (this.m_gameplayPath != null)
		{
			if (this.m_actor != null)
			{
				TheatricsManager.EncapsulatePathInBound(ref bound, this.m_gameplayPath, this.m_actor);
			}
		}
	}

	public bool IsOnLastSegment()
	{
		bool result;
		if (this.m_aestheticPath != null)
		{
			result = (this.m_aestheticPath.next == null);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool IsYetToCompleteGameplayPath()
	{
		return this.m_gameplayPath != null && !this.m_gameplayPath.IsPathEndpoint();
	}

	internal bool IsPast2ndToLastSquare()
	{
		bool result;
		if (this.m_gameplayPath != null)
		{
			result = (this.m_gameplayPath.next == null);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public void OnAssignedToInitialBoardSquare()
	{
		Vector3 spawnFacing = SpawnPointManager.Get().GetSpawnFacing(base.transform.position);
		this.m_actor.TurnToDirection(spawnFacing);
		Animator modelAnimator = this.m_actor.GetModelAnimator();
		if (modelAnimator != null)
		{
			modelAnimator.SetBool(ActorMovement.animDecisionPhase, true);
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		Gizmos.color = Color.black;
		Gizmos.DrawWireCube(this.m_actor.transform.position, new Vector3(1.5f, 2.5f, 1.5f));
		Color[] array = new Color[]
		{
			Color.white,
			Color.magenta,
			Color.yellow,
			Color.red,
			Color.blue,
			Color.black
		};
		float num = 0.4f;
		for (BoardSquarePathInfo boardSquarePathInfo = this.m_aestheticPath; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			Gizmos.color = array[(int)boardSquarePathInfo.connectionType];
			if (boardSquarePathInfo.square != null)
			{
				Gizmos.DrawWireSphere(boardSquarePathInfo.square.GetWorldPosition(), num);
			}
			if (boardSquarePathInfo.prev != null)
			{
				if (boardSquarePathInfo.prev.square != null)
				{
					Gizmos.DrawLine(boardSquarePathInfo.square.GetWorldPosition() + Vector3.up * num, boardSquarePathInfo.prev.square.GetWorldPosition() + Vector3.up * num);
				}
			}
		}
		Gizmos.color = Color.black;
		for (BoardSquarePathInfo boardSquarePathInfo2 = this.m_gameplayPath; boardSquarePathInfo2 != null; boardSquarePathInfo2 = boardSquarePathInfo2.next)
		{
			if (boardSquarePathInfo2.square != null)
			{
				Gizmos.DrawSphere(boardSquarePathInfo2.square.GetWorldPosition(), 0.15f);
			}
			if (boardSquarePathInfo2.prev != null)
			{
				if (boardSquarePathInfo2.prev.square != null)
				{
					Gizmos.DrawLine(boardSquarePathInfo2.square.GetWorldPosition(), boardSquarePathInfo2.prev.square.GetWorldPosition());
				}
			}
		}
		BoardSquare travelBoardSquare = this.GetTravelBoardSquare();
		if (travelBoardSquare != null)
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube(travelBoardSquare.CameraBounds.center, travelBoardSquare.CameraBounds.size);
		}
	}

	public enum IdleType
	{
		Default,
		Special1
	}

	public class SquaresCanMoveToCacheEntry
	{
		public BoardSquare m_squaresCanMoveToOrigin;

		public float m_squaresCanMoveToHorizontalAllowed;

		public int m_squaresCanMoveToSinglePlayerState = -1;

		public int m_squaresCanMoveToBarrierState = -1;

		public FirstTurnMovement.RestrictedMovementState m_squaresCanMoveToFirstTurnState = FirstTurnMovement.RestrictedMovementState.Invalid;

		public HashSet<BoardSquare> m_squaresCanMoveTo;
	}

	private struct BoardSquareMovementInfo
	{
		public BoardSquare square;

		public float cost;

		public float prevCost;
	}

	public enum DiagonalCalcFlag
	{
		Unknown,
		IsDiagonal,
		NotDiagonal
	}
}
