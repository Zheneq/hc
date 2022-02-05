// ROGUES
// SERVER
//using System;
using System.Collections.Generic;
//using Mirror;
using UnityEngine;
using UnityEngine.Networking;

public class ActorMovement : MonoBehaviour, IGameEventListener
{
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

	internal ActorData m_actor;

	private float m_moveTimeoutTime;
	private List<SquaresCanMoveToCacheEntry> m_squaresCanMoveToCache;

	private static int s_maxSquaresCanMoveToCacheCount = 4;

	private HashSet<BoardSquare> m_squaresCanMoveTo;
	private HashSet<BoardSquare> m_squareCanMoveToWithQueuedAbility;
	public float m_brushTransitionAnimationSpeed;
	public float m_brushTransitionAnimationSpeedEaseTime = 0.4f;
	private EasedOutFloat m_brushTransitionAnimationSpeedEased = new EasedOutFloat(1f);

	// removed in rogues
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
	// end removed in rogues

	// rogues?
#if SERVER
	private bool m_ignoreCantSprintStatus;
#endif

	// rogues?
#if SERVER
	private float m_moveRangeCompensation;
#endif

	private MoveState m_curMoveState;
	// removed in rogues
	private Dictionary<BoardSquare, BoardSquarePathInfo> m_tempClosedSquares = new Dictionary<BoardSquare, BoardSquarePathInfo>();
	private BoardSquarePathInfo m_gameplayPath;
	private BoardSquarePathInfo m_aestheticPath;

	internal HashSet<BoardSquare> SquaresCanMoveTo => m_squaresCanMoveTo;
	internal HashSet<BoardSquare> SquaresCanMoveToWithQueuedAbility => m_squareCanMoveToWithQueuedAbility;

	// rogues?
#if SERVER
	public bool IgnoreCantSprintStatus
	{
		get
		{
			return m_ignoreCantSprintStatus;
		}
		set
		{
			m_ignoreCantSprintStatus = value;
		}
	}
#endif

	// rogues?
#if SERVER
	public float MoveRangeCompensation
	{
		get
		{
			return Mathf.Max(0f, m_moveRangeCompensation);
		}
		set
		{
			m_moveRangeCompensation = value;
		}
	}
#endif

	public string GetCurrentMoveStateStr()
	{
		if (m_curMoveState != null)
		{
			return m_curMoveState.stateName;
		}
		return "None";
	}

	private void Awake()
	{
		m_actor = GetComponent<ActorData>();
		m_squaresCanMoveToCache = new List<SquaresCanMoveToCacheEntry>();
		m_squaresCanMoveTo = new HashSet<BoardSquare>();
		m_squareCanMoveToWithQueuedAbility = new HashSet<BoardSquare>();

		// removed in rogues
		animDistToGoal = Animator.StringToHash("DistToGoal");
		animDistToWaypoint = Animator.StringToHash("DistToWayPoint");
		animNextChargeCycleType = Animator.StringToHash("NextChargeCycleType");
		animChargeCycleType = Animator.StringToHash("ChargeCycleType");
		animChargeEndType = Animator.StringToHash("ChargeEndType");
		animNextLinkType = Animator.StringToHash("NextLinkType");
		animCurLinkType = Animator.StringToHash("CurLinkType");
		animTimeInAnim = Animator.StringToHash("TimeInAnim");
		animForceIdle = Animator.StringToHash("ForceIdle");
		animMoveSegmentStart = Animator.StringToHash("MoveSegmentStart");
		animDecisionPhase = Animator.StringToHash("DecisionPhase");
		animIdleType = Animator.StringToHash("IdleType");
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
		UpdatePosition();
		if (NetworkClient.active)
		{
			if (m_actor != null && m_actor.GetModelAnimator() != null)
			{
				float num = m_brushTransitionAnimationSpeedEased;
				Animator modelAnimator = m_actor.GetModelAnimator();
				if (modelAnimator.speed != num)
				{
					modelAnimator.speed = num;
				}
			}
			if (m_actor == GameFlowData.Get().activeOwnedActorData
				&& (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.MovementWaypointModifier)
					|| InputManager.Get().IsKeyBindingNewlyReleased(KeyPreference.MovementWaypointModifier)))
			{
				UpdateSquaresCanMoveTo();
			}
		}
	}

	public void UpdateSquaresCanMoveTo()
	{
		if (NetworkServer.active)  // server-only
		{
#if SERVER
			float maxMoveDistServer = CalculateMaxHorizontalMovement(false, false);
			float innerMoveDistServer = Mathf.Min(maxMoveDistServer, CalculateMaxHorizontalMovement(true, false));
			if (ServerActionBuffer.Get() == null)
			{
				return;
			}
			ServerActionBuffer.Get().GatherMovementInfo(
				m_actor, out BoardSquare moveFromBoardSquare, out float queuedMovementAmount, out bool isChasing);
			if (isChasing)
			{
				m_actor.RemainingHorizontalMovement = maxMoveDistServer;
				m_actor.RemainingMovementWithQueuedAbility = innerMoveDistServer;
				m_actor.MoveFromBoardSquare = m_actor.InitialMoveStartSquare;
			}
			else
			{
				// rogues
				//if (!m_actor.GetActorTurnSM().HasRemainingMovement())
				//{
				//	m_actor.RemainingHorizontalMovement = 0f;
				//	m_actor.RemainingMovementWithQueuedAbility = 0f;
				//}
				//else
				//{
				m_actor.RemainingHorizontalMovement = Mathf.Max(maxMoveDistServer - queuedMovementAmount, 0f);
				m_actor.RemainingMovementWithQueuedAbility = Mathf.Max(innerMoveDistServer - queuedMovementAmount, 0f);
				//}
				m_actor.MoveFromBoardSquare = moveFromBoardSquare;
			}
#endif
		}
		float maxMoveDist = m_actor.RemainingHorizontalMovement;
		float innerMoveDist = m_actor.RemainingMovementWithQueuedAbility;
		BoardSquare squareToStartFrom = m_actor.MoveFromBoardSquare;
		// reactor
		if (Options_UI.Get() != null &&
			Options_UI.Get().GetShiftClickForMovementWaypoints() != InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier)
				|| !FirstTurnMovement.CanWaypoint())
		{
			if (m_actor == GameFlowData.Get().activeOwnedActorData)
			{
				maxMoveDist = CalculateMaxHorizontalMovement();
				innerMoveDist = CalculateMaxHorizontalMovement(true);
				squareToStartFrom = m_actor.InitialMoveStartSquare;
			}
		}
		// rogues
		//if (!InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier)
		//	&& m_actor == GameFlowData.Get().activeOwnedActorData)
		//{
		//	maxMoveDist = this.CalculateMaxHorizontalMovement(false, false);
		//	innerMoveDist = Mathf.Min(maxMoveDist, this.CalculateMaxHorizontalMovement(true, false));
		//	float num6 = ActorMovement.CalcMoveAdjustFromExecutedActions(this.m_actor);
		//	if (!this.m_actor.GetActorTurnSM().HasRemainingMovement())
		//	{
		//		maxMoveDist = 0f;
		//		innerMoveDist = 0f;
		//	}
		//	else
		//	{
		//		maxMoveDist = Mathf.Max(maxMoveDist - num6, 0f);
		//		innerMoveDist = Mathf.Max(innerMoveDist - num6, 0f);
		//	}
		//	squareToStartFrom = m_actor.InitialMoveStartSquare;
		//}

		GetSquaresCanMoveTo_InnerOuter(squareToStartFrom, maxMoveDist, innerMoveDist, out m_squaresCanMoveTo, out m_squareCanMoveToWithQueuedAbility);
		if (Board.Get() != null)
		{
			Board.Get().MarkForUpdateValidSquares();
		}
		if (m_actor == GameFlowData.Get().activeOwnedActorData)
		{
			LineData component = m_actor.GetComponent<LineData>();
			if (component != null)
			{
				component.OnCanMoveToSquaresUpdated();
			}
		}
	}

	// rogues
	//public static float CalcMoveAdjustFromExecutedActions(ActorData actor)
	//{
	//	float num = (float)actor.GetActorTurnSM().GetPveNumMoveActionsUsed() * actor.GetAbilityMovementCost();
	//	num = actor.GetActorMovement().GetAdjustedMovementFromBuffAndDebuff(num, false, false);
	//	if (!actor.GetActorTurnSM().HasRemainingMovement())
	//	{
	//		num = 100000f;
	//	}
	//	return Mathf.Max(actor.GetActorTurnSM().GetPveMovementCostUsed(), num);
	//}

	public bool CanMoveToBoardSquare(int x, int y)
	{
		BoardSquare boardSquare = Board.Get().GetSquareFromIndex(x, y);
		if (boardSquare)
		{
			return CanMoveToBoardSquare(boardSquare);
		}
		return false;
	}

	public bool CanMoveToBoardSquare(BoardSquare dest)
	{
		return m_squaresCanMoveTo.Contains(dest);
	}

	// reactor
	public BoardSquare GetClosestMoveableSquareTo(BoardSquare selectedSquare, bool alwaysIncludeMoverSquare = true) //, bool useNonSprintSquares = false, bool excludeDestSquare = false)  in rogues
	{
		BoardSquare closestSquare = alwaysIncludeMoverSquare ? m_actor.GetCurrentBoardSquare() : null;
		float minDist = alwaysIncludeMoverSquare ? closestSquare.HorizontalDistanceOnBoardTo(selectedSquare) : 100000f;
		if (selectedSquare != null)
		{
			foreach (BoardSquare square in m_squaresCanMoveTo)
			{
				float dist = square.HorizontalDistanceOnBoardTo(selectedSquare);
				if (dist <= minDist)
				{
					minDist = dist;
					closestSquare = square;
				}
			}
			return closestSquare;
		}
		Log.Error("Trying to find the closest moveable square to a null square.  Code error-- tell Danny.");
		return closestSquare;
	}

	// rogues
	// TODO LOW check, might be a bugfix
	//public BoardSquare GetClosestMoveableSquareTo(BoardSquare selectedSquare, bool alwaysIncludeMoverSquare = true, bool useNonSprintSquares = false, bool excludeDestSquare = false)  // useNonSprintSquares & excludeDestSquare added in rogues
	//{
	//    HashSet<BoardSquare> squaresCanMoveTo = useNonSprintSquares
	//        ? m_squareCanMoveToWithQueuedAbility
	//        : m_squaresCanMoveTo;
	//    BoardSquare currentBoardSquare = m_actor.GetCurrentBoardSquare();
	//    BoardSquare closestSquare = alwaysIncludeMoverSquare ? currentBoardSquare : null;
	//    float minDist = alwaysIncludeMoverSquare ? closestSquare.HorizontalDistanceOnBoardTo(selectedSquare) : 100000f;
	//    float minDistInSquares = alwaysIncludeMoverSquare ? closestSquare.HorizontalDistanceInSquaresTo(currentBoardSquare) : 100000f;
	//    if (selectedSquare != null)
	//    {
	//        foreach (BoardSquare square in squaresCanMoveTo)
	//        {
	//            if (!excludeDestSquare || selectedSquare != square)
	//            {
	//                float dist = square.HorizontalDistanceOnBoardTo(selectedSquare);
	//                float distInSquares = square.HorizontalDistanceInSquaresTo(currentBoardSquare);
	//                if (dist < minDist || (dist == minDist && distInSquares < minDistInSquares))
	//                {
	//                    minDist = dist;
	//                    closestSquare = square;
	//                    minDistInSquares = distInSquares;
	//                }
	//            }
	//        }
	//        return closestSquare;
	//    }
	//    Log.Error("Trying to find the closest moveable square to a null square.  Code error-- tell Danny.");
	//    return closestSquare;
	//}

	public float CalculateMaxHorizontalMovement(bool forcePostAbility = false, bool calculateAsIfSnared = false)
	{
		float result = 0f;
		if (!m_actor.IsDead())
		{
			// rogues
			//EquipmentStats equipmentStats = m_actor.GetEquipmentStats();
			result = m_actor.GetActorStats().GetModifiedStatInt(StatType.Movement_Horizontal);
			// rogues
			//result = equipmentStats.GetTotalStatValueForSlot(GearStatType.MovementRangeAdjustment, result, -1, m_actor);

			// rogues?
#if SERVER
			if (NetworkServer.active) // server-only
			{
				result += MoveRangeCompensation;
			}
#endif
			AbilityData abilityData = m_actor.GetAbilityData();
			if (abilityData)
			{
				if (abilityData.GetQueuedAbilitiesAllowMovement())
				{
					float num = forcePostAbility
						? -1f * m_actor.GetAbilityMovementCost()
						: abilityData.GetQueuedAbilitiesMovementAdjust();
					result += num;
					result = GetAdjustedMovementFromBuffAndDebuff(result, forcePostAbility, calculateAsIfSnared);
				}
				else
				{
					result = 0f;
				}
			}
			else
			{
				result = GetAdjustedMovementFromBuffAndDebuff(result, forcePostAbility, calculateAsIfSnared);
			}
			result = Mathf.Clamp(result, 0f, 99f);
		}
		return result;
	}

	public float GetAdjustedMovementFromBuffAndDebuff(float movement, bool forcePostAbility, bool calculateAsIfSnared = false)
	{
		float result = movement;
		ActorStatus actorStatus = m_actor.GetActorStatus();
		if (actorStatus.HasStatus(StatusType.RecentlySpawned))
		{
			result += GameplayData.Get().m_recentlySpawnedBonusMovement;
		}
		if (actorStatus.HasStatus(StatusType.RecentlyRespawned))
		{
			result += GameplayData.Get().m_recentlyRespawnedBonusMovement;
		}

		List<StatusType> queuedStatuses = m_actor.GetAbilityData().GetQueuedAbilitiesOnRequestStatuses();
		bool debuffSuppressed =
			actorStatus.HasStatus(StatusType.MovementDebuffSuppression)
			|| queuedStatuses.Contains(StatusType.MovementDebuffSuppression);
		bool debuffImmune =
			actorStatus.IsMovementDebuffImmune()
			|| queuedStatuses.Contains(StatusType.MovementDebuffImmunity)
			|| queuedStatuses.Contains(StatusType.Unstoppable);
		bool debuff = !debuffSuppressed && !debuffImmune;
		bool cantSprintUnlessUnstoppable =
			actorStatus.HasStatus(StatusType.CantSprint_UnlessUnstoppable)
			|| queuedStatuses.Contains(StatusType.CantSprint_UnlessUnstoppable);
		bool cantSprintAbsolute =
			actorStatus.HasStatus(StatusType.CantSprint_Absolute)
			|| queuedStatuses.Contains(StatusType.CantSprint_Absolute);
		bool cantSprint = (cantSprintUnlessUnstoppable && debuff) || cantSprintAbsolute;

		// rogues?
#if SERVER
		if (NetworkServer.active)
		{
			cantSprint = (cantSprint && !IgnoreCantSprintStatus);
		}
#endif

		if (debuff && actorStatus.HasStatus(StatusType.Rooted))
		{
			return 0f;
		}
		if (actorStatus.HasStatus(StatusType.AnchoredNoMovement))
		{
			return 0f;
		}
		if (debuff && actorStatus.HasStatus(StatusType.CrippledMovement))
		{
			return Mathf.Clamp(result, 0f, 1f);
		}

		if (cantSprint
			&& !forcePostAbility
			&& m_actor.GetAbilityData() != null
			&& m_actor.GetAbilityData().GetQueuedAbilitiesMovementAdjustType() == Ability.MovementAdjustment.FullMovement)
		{
			result -= m_actor.GetAbilityMovementCost();
		}

		bool snared = actorStatus.HasStatus(StatusType.Snared) || queuedStatuses.Contains(StatusType.Snared);
		bool hasted = actorStatus.HasStatus(StatusType.Hasted) || queuedStatuses.Contains(StatusType.Hasted);
		if ((debuff && snared && !hasted) || calculateAsIfSnared)
		{
			CalcSnaredMovementAdjustments(out float snaredMult, out int halfMoveAdjust, out int fullMoveAdjust);
			if (forcePostAbility)
			{
				result = Mathf.Clamp(result + halfMoveAdjust, 0f, 99f);
			}
			else
			{
				int moveAdjust = m_actor.GetAbilityData() != null
					&& m_actor.GetAbilityData().GetQueuedAbilitiesMovementAdjustType() == Ability.MovementAdjustment.ReducedMovement
					? halfMoveAdjust
					: fullMoveAdjust;
				result = Mathf.Clamp(result + moveAdjust, 0f, 99f);
			}
			result *= snaredMult;
			result = MovementUtils.RoundToNearestHalf(result);
		}
		else if (hasted && (!debuff || !snared))
		{
			CalcHastedMovementAdjustments(out float mult, out int halfMoveAdjust, out int fullMoveAdjust);
			if (forcePostAbility)
			{
				result = Mathf.Clamp(result + (float)halfMoveAdjust, 0f, 99f);
			}
			else
			{
				int moveAdjust = m_actor.GetAbilityData() != null && m_actor.GetAbilityData().GetQueuedAbilitiesMovementAdjustType() == Ability.MovementAdjustment.ReducedMovement
					? halfMoveAdjust
					: fullMoveAdjust;
				result = Mathf.Clamp(result + (float)moveAdjust, 0f, 99f);
			}
			result *= mult;
			result = MovementUtils.RoundToNearestHalf(result);
		}
		return result;
	}

	public static void CalcHastedMovementAdjustments(out float mult, out int halfMoveAdjustment, out int fullMoveAdjustment)
	{
		if (GameplayMutators.Get() != null && GameplayMutators.Get().m_useHasteOverride)
		{
			halfMoveAdjustment = GameplayMutators.Get().m_hasteHalfMovementAdjustAmount;
			fullMoveAdjustment = GameplayMutators.Get().m_hasteFullMovementAdjustAmount;
			mult = GameplayMutators.Get().m_hasteMovementMultiplier;
		}
		else
		{
			halfMoveAdjustment = GameWideData.Get().m_hasteHalfMovementAdjustAmount;
			fullMoveAdjustment = GameWideData.Get().m_hasteFullMovementAdjustAmount;
			mult = GameWideData.Get().m_hasteMovementMultiplier;
		}
	}

	public static void CalcSnaredMovementAdjustments(out float mult, out int halfMoveAdjust, out int fullMoveAdjust)
	{
		if (GameplayMutators.Get() != null && GameplayMutators.Get().m_useSlowOverride)
		{
			halfMoveAdjust = GameplayMutators.Get().m_slowHalfMovementAdjustAmount;
			fullMoveAdjust = GameplayMutators.Get().m_slowFullMovementAdjustAmount;
			mult = GameplayMutators.Get().m_slowMovementMultiplier;
		}
		else
		{
			halfMoveAdjust = GameWideData.Get().m_slowHalfMovementAdjustAmount;
			fullMoveAdjust = GameWideData.Get().m_slowFullMovementAdjustAmount;
			mult = GameWideData.Get().m_slowMovementMultiplier;
		}
	}

	public void BuildSquaresCanMoveTo_InnerOuter(BoardSquare squareToStartFrom, float maxHorizontalMovement, float innerMaxMove, out HashSet<BoardSquare> eligibleSquares, out HashSet<BoardSquare> innerSquares)
	{
		eligibleSquares = new HashSet<BoardSquare>();
		innerSquares = new HashSet<BoardSquare>();
		if (squareToStartFrom == null || maxHorizontalMovement == 0f)
		{
			return;
		}

		eligibleSquares.Add(squareToStartFrom);
		if (innerMaxMove > 0f)
		{
			innerSquares.Add(squareToStartFrom);
		}
		LinkedList<BoardSquareMovementInfo> linkedList = new LinkedList<BoardSquareMovementInfo>();
		HashSet<BoardSquare> hashSet = new HashSet<BoardSquare>();
		BoardSquareMovementInfo value = new BoardSquareMovementInfo
		{
			square = squareToStartFrom,
			cost = 0f,
			prevCost = 0f
		};
		linkedList.AddLast(value);
		bool cannotExceedMax = GameplayData.Get() != null
			&& GameplayData.Get().m_movementMaximumType == GameplayData.MovementMaximumType.CannotExceedMax;
		Board board = Board.Get();
		while (linkedList.Count > 0)
		{
			BoardSquareMovementInfo value2 = linkedList.First.Value;
			BoardSquare square = value2.square;
			int x = square.x;
			int y = square.y;
			for (int i = -1; i <= 1; i++)
			{
				for (int j = -1; j <= 1; j++)
				{
					if (i == 0 && j == 0)
					{
						continue;
					}
					BoardSquare boardSquare = board.GetSquareFromIndex(x + i, y + j);
					if (boardSquare == null || hashSet.Contains(boardSquare))
					{
						continue;
					}
					bool isDiagonalStep = board.GetSquaresAreDiagonallyAdjacent(square, boardSquare);
					float nextCost = isDiagonalStep
						? value2.cost + 1.5f
						: value2.cost + 1f;
					bool isValidMove = cannotExceedMax
						? nextCost <= maxHorizontalMovement
						: value2.cost < maxHorizontalMovement;
					if (isValidMove)
					{
						DiagonalCalcFlag diagonalFlag = isDiagonalStep ? DiagonalCalcFlag.IsDiagonal : DiagonalCalcFlag.NotDiagonal;
						if (CanCrossToAdjacentSquare(square, boardSquare, false, diagonalFlag))
						{
							BoardSquareMovementInfo value3 = new BoardSquareMovementInfo
							{
								square = boardSquare,
								cost = nextCost,
								prevCost = value2.cost
							};
							bool flag3 = false;

							for (LinkedListNode<BoardSquareMovementInfo> linkedListNode = linkedList.First; linkedListNode != linkedList.Last; linkedListNode = linkedListNode.Next)
							{
								BoardSquareMovementInfo value4 = linkedListNode.Value;
								if (value4.square == boardSquare)
								{
									flag3 = true;
									if (value4.cost > nextCost)
									{
										linkedListNode.Value = value3;
									}
									else if (value4.cost == nextCost && value3.prevCost < value4.prevCost)
									{
										linkedListNode.Value = value3;
									}
									break;
								}
							}
							if (!flag3 && FirstTurnMovement.CanActorMoveToSquare(m_actor, boardSquare))
							{
								linkedList.AddLast(value3);
							}
						}
					}
				}
			}
			if (MovementUtils.CanStopOnSquare(square)
				&& SinglePlayerManager.IsDestinationAllowed(m_actor, square)
				&& FirstTurnMovement.CanActorMoveToSquare(m_actor, square))
			{
				if (!eligibleSquares.Contains(square))
				{
					eligibleSquares.Add(square);
				}
				if (innerMaxMove > 0f && !innerSquares.Contains(square))
				{
					bool isValidMove = cannotExceedMax
						? value2.cost <= innerMaxMove
						: value2.prevCost < innerMaxMove;
					if (isValidMove)
					{
						innerSquares.Add(square);
					}
				}
			}
			hashSet.Add(square);
			linkedList.RemoveFirst();
		}
	}

	// removed in rogues
	public HashSet<BoardSquare> BuildSquaresCanMoveTo(BoardSquare squareToStartFrom, float maxHorizontalMovement)
	{
		HashSet<BoardSquare> hashSet = new HashSet<BoardSquare>();
		if (squareToStartFrom == null || maxHorizontalMovement == 0f)
		{
			return hashSet;
		}
		hashSet.Add(squareToStartFrom);
		LinkedList<BoardSquareMovementInfo> linkedList = new LinkedList<BoardSquareMovementInfo>();
		HashSet<BoardSquare> hashSet2 = new HashSet<BoardSquare>();
		BoardSquareMovementInfo value = new BoardSquareMovementInfo
		{
			square = squareToStartFrom,
			cost = 0f,
			prevCost = 0f
		};
		linkedList.AddLast(value);
		List<BoardSquare> result = new List<BoardSquare>(8);
		while (linkedList.Count > 0)
		{
			BoardSquareMovementInfo value2 = linkedList.First.Value;
			BoardSquare square = value2.square;
			result.Clear();
			if (GameplayData.Get() != null && GameplayData.Get().m_diagonalMovement == GameplayData.DiagonalMovement.Disabled)
			{
				Board.Get().GetCardinalAdjacentSquares(square.x, square.y, ref result);
			}
			else
			{
				Board.Get().GetAllAdjacentSquares(square.x, square.y, ref result);
			}
			for (int i = 0; i < result.Count; i++)
			{
				BoardSquare boardSquare = result[i];
				if (hashSet2.Contains(boardSquare))
				{
					continue;
				}
				bool isDiagonalStep = Board.Get().GetSquaresAreDiagonallyAdjacent(square, boardSquare);
				float nextCost = isDiagonalStep
					? value2.cost + 1.5f
					: value2.cost + 1f;
				bool cannotExceedMax = GameplayData.Get() != null
					&& GameplayData.Get().m_movementMaximumType == GameplayData.MovementMaximumType.CannotExceedMax;
				bool isValidMove = cannotExceedMax
					? nextCost <= maxHorizontalMovement
					: value2.cost < maxHorizontalMovement;
				if (isValidMove)
				{
					BoardSquareMovementInfo value3 = new BoardSquareMovementInfo
					{
						square = boardSquare,
						cost = nextCost,
						prevCost = value2.cost
					};
					bool flag3 = false;

					for (LinkedListNode<BoardSquareMovementInfo> linkedListNode = linkedList.First; linkedListNode != linkedList.Last; linkedListNode = linkedListNode.Next)
					{
						BoardSquareMovementInfo value4 = linkedListNode.Value;
						if (value4.square == boardSquare)
						{
							flag3 = true;
							if (value4.cost > nextCost)
							{
								linkedListNode.Value = value3;
							}
							break;
						}
					}
					DiagonalCalcFlag diagonalCalcFlag = isDiagonalStep ? DiagonalCalcFlag.IsDiagonal : DiagonalCalcFlag.NotDiagonal;
					if (!flag3
						&& CanCrossToAdjacentSquare(square, boardSquare, false, diagonalCalcFlag)
						&& FirstTurnMovement.CanActorMoveToSquare(m_actor, boardSquare))
					{
						linkedList.AddLast(value3);
					}
				}
			}
			if (!hashSet.Contains(square)
				&& MovementUtils.CanStopOnSquare(square)
				&& SinglePlayerManager.IsDestinationAllowed(m_actor, square)
				&& FirstTurnMovement.CanActorMoveToSquare(m_actor, square))
			{
				hashSet.Add(square);
			}
			hashSet2.Add(square);
			linkedList.RemoveFirst();
		}
		return hashSet;
	}

	// added in rogues
#if SERVER
	public void ClearSquaresCanMoveToCache()
	{
		m_squaresCanMoveToCache.Clear();
	}
#endif

	public HashSet<BoardSquare> CheckSquareCanMoveToCache(BoardSquare squareToStartFrom, float maxHorizontalMovement)
	{
		HashSet<BoardSquare> result = null;
		int squaresCanMoveToSinglePlayerState = -1;
		if (SinglePlayerManager.Get() != null && m_actor.SpawnerId == -1)
		{
			squaresCanMoveToSinglePlayerState = SinglePlayerManager.Get().GetCurrentScriptIndex();
		}
		int squaresCanMoveToBarrierState = -1;
		if (BarrierManager.Get() != null)
		{
			squaresCanMoveToBarrierState = BarrierManager.Get().GetMovementStateChangesFor(m_actor);
		}
		FirstTurnMovement.RestrictedMovementState squaresCanMoveToFirstTurnState = FirstTurnMovement.RestrictedMovementState.Invalid;
		if (FirstTurnMovement.Get() != null)
		{
			squaresCanMoveToFirstTurnState = FirstTurnMovement.Get().GetRestrictedMovementState();
		}
		SquaresCanMoveToCacheEntry squaresCanMoveToCacheEntry = new SquaresCanMoveToCacheEntry
		{
			m_squaresCanMoveToOrigin = squareToStartFrom,
			m_squaresCanMoveToHorizontalAllowed = maxHorizontalMovement,
			m_squaresCanMoveToSinglePlayerState = squaresCanMoveToSinglePlayerState,
			m_squaresCanMoveToBarrierState = squaresCanMoveToBarrierState,
			m_squaresCanMoveToFirstTurnState = squaresCanMoveToFirstTurnState
		};
		int num = 0;
		SquaresCanMoveToCacheEntry item = null;

		for (int num2 = 0; num2 < m_squaresCanMoveToCache.Count; num2++)
		{
			SquaresCanMoveToCacheEntry squaresCanMoveToCacheEntry2 = m_squaresCanMoveToCache[num2];
			if (squaresCanMoveToCacheEntry2.m_squaresCanMoveToOrigin == squaresCanMoveToCacheEntry.m_squaresCanMoveToOrigin
				&& squaresCanMoveToCacheEntry2.m_squaresCanMoveToHorizontalAllowed == squaresCanMoveToCacheEntry.m_squaresCanMoveToHorizontalAllowed
				&& squaresCanMoveToCacheEntry2.m_squaresCanMoveToSinglePlayerState == squaresCanMoveToCacheEntry.m_squaresCanMoveToSinglePlayerState
				&& squaresCanMoveToCacheEntry2.m_squaresCanMoveToBarrierState == squaresCanMoveToCacheEntry.m_squaresCanMoveToBarrierState
				&& squaresCanMoveToCacheEntry2.m_squaresCanMoveToFirstTurnState == squaresCanMoveToCacheEntry.m_squaresCanMoveToFirstTurnState)
			{
				result = squaresCanMoveToCacheEntry2.m_squaresCanMoveTo;
				item = squaresCanMoveToCacheEntry2;
				num = num2;
				break;
			}
		}
		if (num != 0)
		{
			m_squaresCanMoveToCache.RemoveAt(num);
			m_squaresCanMoveToCache.Insert(0, item);
		}
		return result;
	}

	public void AddToSquareCanMoveToCache(BoardSquare squareToStartFrom, float maxHorizontalMovement, HashSet<BoardSquare> squaresCanMoveTo)
	{
		int squaresCanMoveToBarrierState = -1;
		int squaresCanMoveToSinglePlayerState = -1;
		FirstTurnMovement.RestrictedMovementState squaresCanMoveToFirstTurnState = FirstTurnMovement.RestrictedMovementState.Invalid;
		if (SinglePlayerManager.Get() != null && m_actor.SpawnerId == -1)
		{
			squaresCanMoveToSinglePlayerState = SinglePlayerManager.Get().GetCurrentScriptIndex();
		}
		if (BarrierManager.Get() != null)
		{
			squaresCanMoveToBarrierState = BarrierManager.Get().GetMovementStateChangesFor(m_actor);
		}
		if (FirstTurnMovement.Get() != null)
		{
			squaresCanMoveToFirstTurnState = FirstTurnMovement.Get().GetRestrictedMovementState();
		}
		SquaresCanMoveToCacheEntry squaresCanMoveToCacheEntry = new SquaresCanMoveToCacheEntry
		{
			m_squaresCanMoveToOrigin = squareToStartFrom,
			m_squaresCanMoveToHorizontalAllowed = maxHorizontalMovement,
			m_squaresCanMoveToSinglePlayerState = squaresCanMoveToSinglePlayerState,
			m_squaresCanMoveToBarrierState = squaresCanMoveToBarrierState,
			m_squaresCanMoveToFirstTurnState = squaresCanMoveToFirstTurnState,
			m_squaresCanMoveTo = squaresCanMoveTo
		};
		if (m_squaresCanMoveToCache.Count >= s_maxSquaresCanMoveToCacheCount)
		{
			m_squaresCanMoveToCache.RemoveAt(m_squaresCanMoveToCache.Count - 1);
		}
		m_squaresCanMoveToCache.Insert(0, squaresCanMoveToCacheEntry);
	}

	// removed in rogues
	public HashSet<BoardSquare> GetSquaresCanMoveTo(BoardSquare squareToStartFrom, float maxHorizontalMovement)
	{
		HashSet<BoardSquare> hashSet = CheckSquareCanMoveToCache(squareToStartFrom, maxHorizontalMovement);
		if (hashSet != null)
		{
			return hashSet;
		}
		hashSet = BuildSquaresCanMoveTo(squareToStartFrom, maxHorizontalMovement);
		AddToSquareCanMoveToCache(squareToStartFrom, maxHorizontalMovement, hashSet);
		return hashSet;
	}

	public void GetSquaresCanMoveTo_InnerOuter(BoardSquare squareToStartFrom, float maxMoveDist, float innerMoveDist, out HashSet<BoardSquare> outMaxMoveSquares, out HashSet<BoardSquare> outInnerMoveSquares)
	{
		HashSet<BoardSquare> eligibleSquares = CheckSquareCanMoveToCache(squareToStartFrom, maxMoveDist);
		HashSet<BoardSquare> innerSquares = CheckSquareCanMoveToCache(squareToStartFrom, innerMoveDist);
		if (eligibleSquares == null && innerSquares == null)
		{
			BuildSquaresCanMoveTo_InnerOuter(squareToStartFrom, maxMoveDist, innerMoveDist, out eligibleSquares, out innerSquares);
			AddToSquareCanMoveToCache(squareToStartFrom, maxMoveDist, eligibleSquares);
			AddToSquareCanMoveToCache(squareToStartFrom, innerMoveDist, innerSquares);
		}
		else if (eligibleSquares == null)
		{
			BuildSquaresCanMoveTo_InnerOuter(squareToStartFrom, maxMoveDist, 0f, out eligibleSquares, out HashSet<BoardSquare> _);
			AddToSquareCanMoveToCache(squareToStartFrom, maxMoveDist, eligibleSquares);
		}
		else if (innerSquares == null)
		{
			BuildSquaresCanMoveTo_InnerOuter(squareToStartFrom, innerMoveDist, 0f, out innerSquares, out HashSet<BoardSquare> _);
			AddToSquareCanMoveToCache(squareToStartFrom, innerMoveDist, innerSquares);
		}
		outMaxMoveSquares = eligibleSquares;
		outInnerMoveSquares = innerSquares;
	}

	public bool CanCrossToAdjacentSquare(BoardSquare src, BoardSquare dest, bool ignoreBarriers, DiagonalCalcFlag diagonalFlag = DiagonalCalcFlag.Unknown)
	{
		return CanCrossToAdjacentSingleSquare(src, dest, ignoreBarriers, true, diagonalFlag);
	}

	private bool CanCrossToAdjacentSingleSquare(BoardSquare src, BoardSquare dest, bool ignoreBarriers, bool knownAdjacent = false, DiagonalCalcFlag diagonalFlag = DiagonalCalcFlag.Unknown)
	{
		if (dest == null || !dest.IsValidForGameplay())
		{
			return false;
		}
		ThinCover.CoverType thinCover = src.GetThinCover(VectorUtils.GetCoverDirection(src, dest));
		if (thinCover == ThinCover.CoverType.Full
			// rogues
			//|| thinCover == ThinCover.CoverType.HalfThick
			//|| thinCover == ThinCover.CoverType.FullThick
			)
		{
			return false;
		}
		if (!ignoreBarriers
			&& BarrierManager.Get() != null
			&& BarrierManager.Get().IsMovementBlocked(m_actor, src, dest))
		{
			return false;
		}
		if (!knownAdjacent && !Board.Get().GetSquaresAreAdjacent(src, dest))
		{
			return false;
		}
		bool result = true;
		if (diagonalFlag == DiagonalCalcFlag.IsDiagonal ||
			diagonalFlag == DiagonalCalcFlag.Unknown && Board.Get().GetSquaresAreDiagonallyAdjacent(src, dest))
		{
			BoardSquare boardSquare = Board.Get().GetSquareFromIndex(src.x, dest.y);
			BoardSquare boardSquare2 = Board.Get().GetSquareFromIndex(dest.x, src.y);
			if (result)
			{
				result &= CanCrossToAdjacentSingleSquare(src, boardSquare, ignoreBarriers, true, DiagonalCalcFlag.NotDiagonal);
			}
			if (result)
			{
				result &= CanCrossToAdjacentSingleSquare(src, boardSquare2, ignoreBarriers, true, DiagonalCalcFlag.NotDiagonal);
			}
			if (result)
			{
				result &= CanCrossToAdjacentSingleSquare(boardSquare, dest, ignoreBarriers, true, DiagonalCalcFlag.NotDiagonal);
			}
			if (result)
			{
				result &= CanCrossToAdjacentSingleSquare(boardSquare2, dest, ignoreBarriers, true, DiagonalCalcFlag.NotDiagonal);
			}
		}

		return result;
	}

	public void ClearPath()
	{
		if (m_gameplayPath != null)
		{
			m_gameplayPath = null;
		}
		Client_ClearAestheticPath();
	}

	private void Client_ClearAestheticPath()
	{
		if (m_aestheticPath != null)
		{
			m_aestheticPath = null;
		}
	}

	public Vector3 GetGroundPosition(Vector3 testPos)
	{
		Vector3 result = testPos;
		int layerMask = 1 << LayerMask.NameToLayer("LineOfSight");
		Vector3 origin = testPos;
		origin.y += 4f;
		float radius = 0.1f;
		if (Physics.SphereCast(origin, radius, Vector3.down, out RaycastHit hitInfo, 100f, layerMask))
		{
			result.y = hitInfo.point.y;
		}
		return result;
	}

	public bool ShouldSetForceIdle()
	{
		Animator modelAnimator = m_actor.GetModelAnimator();
		if (modelAnimator == null || modelAnimator.layerCount < 1)
		{
			return false;
		}
		AnimatorStateInfo currentAnimatorStateInfo = modelAnimator.GetCurrentAnimatorStateInfo(0);
		return !currentAnimatorStateInfo.IsName("Run_Vault")
			&& !currentAnimatorStateInfo.IsName("Run_End")
			&& !currentAnimatorStateInfo.IsTag("Run_End")
			&& !currentAnimatorStateInfo.IsName("KnockbackEnd")
			&& !currentAnimatorStateInfo.IsTag("Knockdown")
			&& !currentAnimatorStateInfo.IsTag("Damage")
			&& !m_actor.GetActorModelData().IsPlayingIdleAnim();
	}

	internal void OnGameStateChange(bool decisionState)
	{
		Animator modelAnimator = m_actor.GetModelAnimator();
		if (!decisionState && m_brushTransitionAnimationSpeedEased.GetEndValue() != 1f)
		{
			m_brushTransitionAnimationSpeedEased.EaseTo(1f, 0f);
		}
		while (m_gameplayPath != null && m_gameplayPath.next != null && !m_actor.IsDead())
		{
			AdvanceGameplayPath();
		}
		if (m_curMoveState != null)
		{
			m_curMoveState = null;
			StopMovementAnimator();
		}
		if (modelAnimator != null && modelAnimator.GetInteger(animIdleType) < 0)
		{
			modelAnimator.SetInteger(animIdleType, 0);
		}
		if (ShouldSetForceIdle())
		{
			modelAnimator.SetTrigger(animForceIdle);
		}
		if (m_actor.GetCurrentBoardSquare() != null)
		{
			m_actor.SetTransformPositionToSquare(m_actor.GetCurrentBoardSquare());
		}
		Client_ClearAestheticPath();
		m_moveTimeoutTime = -1f;
	}

	public void ResetChargeCycleFlag()
	{
		Animator modelAnimator = m_actor.GetModelAnimator();
		if (modelAnimator != null)
		{
			modelAnimator.SetInteger(animNextChargeCycleType, 0);
			modelAnimator.SetInteger(animChargeCycleType, 0);
			modelAnimator.SetInteger(animChargeEndType, 4);
			modelAnimator.SetFloat(animDistToWaypoint, 0f);
		}
	}

	// added in rogues
#if SERVER
	internal void Server_ForceCompletePath(ref bool movementHappened)
	{
		bool flag = false;
		while (m_gameplayPath != null && m_gameplayPath.next != null)
		{
			AdvanceGameplayPath();
			flag = true;
		}
		if (flag)
		{
			m_actor.GetActorCover().RecalculateCover();
			movementHappened = true;
		}
		ClearPath();
	}
#endif

	private void UpdatePath()
	{
		bool needFogOfWarUpdate = false;
		if (m_aestheticPath == null)
		{
			Log.Error($"{m_actor.DisplayName} trying to UpdatePath with a null aesthetic path; exiting.");
			return;
		}
		if (m_aestheticPath.next == null)
		{
			while (m_gameplayPath.next != null)
			{
				AdvanceGameplayPath();
				needFogOfWarUpdate = !Application.isEditor
					|| DebugParameters.Get() == null
					|| !DebugParameters.Get().GetParameterAsBool("SkipFogOfWarUpdateOnMovement");
			}
			m_actor.GetActorCover().RecalculateCover();
		}
		if (m_aestheticPath != null && m_aestheticPath.next != null)
		{
			m_aestheticPath = m_aestheticPath.next;
		}
		else
		{
			m_aestheticPath = null;
		}
		if (needFogOfWarUpdate)
		{
			m_actor.GetFogOfWar().MarkForRecalculateVisibility();
			UpdateClientFogOfWarIfNeeded();
		}
	}

	public bool AmMoving()
	{
		bool flag;
		if (m_curMoveState != null)
		{
			if (m_curMoveState is ChargeState)
			{
				flag = !(m_curMoveState as ChargeState).DoneMoving();
			}
			else
			{
				flag = !m_curMoveState.m_done;
			}
		}
		else
		{
			flag = false;
		}
		return flag && !ShouldPauseAnimator();
	}

	public bool InChargeState()
	{
		return m_curMoveState != null && m_curMoveState is ChargeState;
	}

	internal float FindDistanceToEnd()
	{
		if (AmMoving() && m_gameplayPath != null)
		{
			return m_gameplayPath.FindDistanceToEnd();
		}
		return 0f;
	}

	internal bool FindIsVisibleToClient()
	{
		FogOfWar clientFog = FogOfWar.GetClientFog();
		if (m_gameplayPath == null || clientFog == null)
		{
			return false;
		}
		for (BoardSquarePathInfo boardSquarePathInfo = m_gameplayPath; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
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
		return m_aestheticPath != null && m_aestheticPath.connectionType == type;
	}

	public void OnMidMovementDeath()
	{
		UpdatePosition();
		if (m_curMoveState != null)
		{
			m_curMoveState.Update();
		}
		StopMovementAnimator();
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.ActiveControlChangedToEnemyTeam)
		{
			m_brushTransitionAnimationSpeedEased.EaseTo(1f, 0f);
		}
	}

	public void UpdatePosition()
	{
		if (m_curMoveState == null)
		{
			return;
		}
		BoardSquarePathInfo gameplayPathClosestTo = GetGameplayPathClosestTo(m_actor.transform.position);
		bool needFogOfWarUpdate = false;
		while (m_gameplayPath != gameplayPathClosestTo)
		{
			AdvanceGameplayPath();
			needFogOfWarUpdate = !Application.isEditor
				|| DebugParameters.Get() == null
				|| !DebugParameters.Get().GetParameterAsBool("SkipFogOfWarUpdateOnMovement");
		}
		if (needFogOfWarUpdate)
		{
			m_actor.GetFogOfWar().MarkForRecalculateVisibility();
			UpdateClientFogOfWarIfNeeded();
		}
		if (!m_actor.IsDead() && !m_actor.IsInRagdoll())
		{
			bool updatePath = m_curMoveState.m_updatePath;
			m_curMoveState.Update();
			UpdateValuesAnimator();
			if (!updatePath && m_curMoveState.m_updatePath)
			{
				UpdatePath();
			}
			if (m_curMoveState == null || m_curMoveState.m_done)
			{
				UpdateMovementState();
				UpdateValuesAnimator();
				if (m_curMoveState == null)
				{
					StopMovementAnimator();
				}
			}
		}
	}

	private void UpdateClientFogOfWarIfNeeded()
	{
		if (NetworkClient.active
			&& FogOfWar.GetClientFog() != null
			&& GameFlowData.Get() != null
			&& GameFlowData.Get().LocalPlayerData != null)
		{
			Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
			bool isRevealed = m_actor.GetActorStatus() != null && m_actor.GetActorStatus().HasStatus(StatusType.Revealed);
			bool isRevealedByFlag = CaptureTheFlag.IsActorRevealedByFlag_Client(m_actor);
			if (m_actor.GetTeam() != teamViewing && (isRevealed || isRevealedByFlag))
			{
				FogOfWar.GetClientFog().MarkForRecalculateVisibility();
			}
		}
	}

	private bool ShouldPauseAnimator()
	{
		return false;
	}

	private void StopMovementAnimator()
	{
		Animator modelAnimator = m_actor.GetModelAnimator();
		if (modelAnimator == null)
		{
			return;
		}
		if (ShouldPauseAnimator())
		{
			if (m_brushTransitionAnimationSpeedEased.GetEndValue() != m_brushTransitionAnimationSpeed)
			{
				m_brushTransitionAnimationSpeedEased.EaseTo(m_brushTransitionAnimationSpeed, m_brushTransitionAnimationSpeedEaseTime);
			}
		}
		else
		{
			modelAnimator.SetInteger(animNextLinkType, 0);
			modelAnimator.SetInteger(animCurLinkType, 0);
			modelAnimator.SetFloat(animDistToGoal, 0f);
			modelAnimator.SetFloat(animTimeInAnim, 0f);
		}
		if (m_actor != null && m_actor.GetActorModelData() != null)
		{
			m_actor.GetActorModelData().OnMovementAnimatorStop();
		}
	}

	private float GetDistToPathSquare(BoardSquare goalGridSquare)
	{
		Vector3 position = m_actor.transform.position;
		Vector3 vector = goalGridSquare.GetOccupantRefPos() - position;
		vector.y = 0f;
		return vector.magnitude;
	}

	public Vector3 GetCurrentMovementDir()
	{
		Vector3 result;
		if (m_actor != null
			&& m_actor.transform != null
			&& m_aestheticPath != null
			&& m_aestheticPath.square != null)
		{
			Vector3 position = m_actor.transform.position;
			result = m_aestheticPath.square.GetOccupantRefPos() - position;
			result.y = 0f;
			result.Normalize();
			return result;
		}
		return Vector3.zero;
	}

	private void UpdateValuesAnimator()
	{
		Animator modelAnimator = m_actor.GetModelAnimator();
		if (modelAnimator == null)
		{
			return;
		}
		float brushTransitionSpeed = ShouldPauseAnimator() ? m_brushTransitionAnimationSpeed : 1f;
		if (m_brushTransitionAnimationSpeedEased.GetEndValue() != brushTransitionSpeed)
		{
			float duration = brushTransitionSpeed == 1f ? 0f : m_brushTransitionAnimationSpeedEaseTime;
			m_brushTransitionAnimationSpeedEased.EaseTo(brushTransitionSpeed, duration);
		}
		if (m_curMoveState != null)
		{
			modelAnimator.SetFloat(animDistToGoal, GetDistToGoal());
			modelAnimator.SetFloat(animTimeInAnim, m_curMoveState.m_timeInAnim);
			if (m_curMoveState.m_forceAnimReset && ShouldSetForceIdle())
			{
				modelAnimator.SetTrigger(animForceIdle);
			}
		}
		modelAnimator.SetInteger(animCurLinkType, MovementUtils.GetLinkType(m_aestheticPath));
		if (m_aestheticPath != null)
		{
			modelAnimator.SetInteger(animNextLinkType, MovementUtils.GetLinkType(m_aestheticPath.next));
			modelAnimator.SetInteger(animChargeCycleType, (int)m_aestheticPath.chargeCycleType);
			modelAnimator.SetInteger(animChargeEndType, (int)m_aestheticPath.chargeEndType);
			modelAnimator.SetFloat(animDistToWaypoint, GetDistToPathSquare(m_aestheticPath.square));
			if (m_aestheticPath.next != null)
			{
				modelAnimator.SetInteger(animNextChargeCycleType, (int)m_aestheticPath.next.chargeCycleType);
			}
		}
	}

	private bool EnemyRunningIntoBrush(BoardSquarePathInfo pathEndInfo)
	{
		if (m_actor.VisibleTillEndOfPhase
			|| m_actor.CurrentlyVisibleForAbilityCast // removed in rogues
			|| m_actor.GetActorStatus().HasStatus(StatusType.Revealed, false)
			|| CaptureTheFlag.IsActorRevealedByFlag_Client(m_actor))
		{
			return false;
		}
		if (pathEndInfo == null)
		{
			if (m_actor.IsActorVisibleToClient()
				|| !m_actor.IsInBrush()
				|| !m_actor.GetActorMovement().IsPast2ndToLastSquare())
			{
				return false;
			}

		}
		else
		{
			if (pathEndInfo.m_visibleToEnemies
				|| pathEndInfo.m_moverHasGameplayHitHere
				|| pathEndInfo.square == null
				|| BrushCoordinator.Get() == null
				|| !BrushCoordinator.Get().IsRegionFunctioning(pathEndInfo.square.BrushRegion))
			{
				return false;
			}
		}
		// reactor
		return ServerClientUtils.GetCurrentActionPhase() >= ActionBufferPhase.Movement
			|| GameFlowData.Get().gameState == GameState.BothTeams_Decision;
		// rogues
		//return true;
	}

	private float GetDistToGoal()
	{
		float num = 0f;
		Vector3 b = transform.position;
		BoardSquarePathInfo boardSquarePathInfo = m_aestheticPath;
		BoardSquarePathInfo pathEndInfo = m_aestheticPath;
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
		if (EnemyRunningIntoBrush(pathEndInfo))
		{
			num += 999f;
		}
		return num;
	}

	private float GetDistToGround()
	{
		Vector3 groundPosition = GetGroundPosition(m_actor.transform.position);
		return m_actor.transform.position.y - groundPosition.y;
	}

	private void OnDeath()
	{
		UpdateMovementState();
	}

	private void UpdateMovementState()
	{
		if (m_actor.IsDead())
		{
			Client_ClearAestheticPath();
		}
		if (m_actor.IsInRagdoll())
		{
			Client_ClearAestheticPath();
		}
		if (m_aestheticPath != null)
		{
			if (m_curMoveState == null
				|| m_curMoveState.m_connectionType != BoardSquarePathInfo.ConnectionType.Run
				|| m_aestheticPath.connectionType != BoardSquarePathInfo.ConnectionType.Run)
			{
				Animator modelAnimator = m_actor.GetModelAnimator();
				if (modelAnimator != null)
				{
					modelAnimator.SetTrigger(animMoveSegmentStart);
				}
			}
			switch (m_aestheticPath.connectionType)
			{
				case BoardSquarePathInfo.ConnectionType.Charge:
				case BoardSquarePathInfo.ConnectionType.Flight:
					m_curMoveState = new ChargeState(this, m_aestheticPath);
					break;
				case BoardSquarePathInfo.ConnectionType.Knockback:
					m_curMoveState = new KnockbackState(this, m_aestheticPath);
					break;
				case BoardSquarePathInfo.ConnectionType.Run:
				case BoardSquarePathInfo.ConnectionType.Vault:
					m_curMoveState = new RunState(this, m_aestheticPath);
					break;
			}
			if (m_actor != null && m_actor.GetActorModelData() != null)
			{
				m_actor.GetActorModelData().OnMovementAnimatorUpdate(m_aestheticPath.connectionType);
			}
		}
		else if (m_curMoveState != null)
		{
			Animator modelAnimator2 = m_actor.GetModelAnimator();
			if (modelAnimator2 != null)
			{
				modelAnimator2.SetTrigger(animMoveSegmentStart);
			}
			m_curMoveState = null;
			StopMovementAnimator();
		}
	}

	private BoardSquarePathInfo GetGameplayPathClosestTo(Vector3 pos)
	{
		Vector3 b = new Vector3(pos.x, 0f, pos.z);
		BoardSquarePathInfo boardSquarePathInfo = m_gameplayPath?.next;
		BoardSquarePathInfo result = m_gameplayPath;
		if (m_gameplayPath != null)
		{
			Vector3 a = new Vector3(
				m_gameplayPath.square.transform.position.x,
				0f,
				m_gameplayPath.square.transform.position.z);
			float num = (a - b).sqrMagnitude;
			while (boardSquarePathInfo != null)
			{
				Vector3 a2 = new Vector3(
					boardSquarePathInfo.square.transform.position.x,
					0f,
					boardSquarePathInfo.square.transform.position.z);
				float sqrMagnitude = (a2 - b).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					result = boardSquarePathInfo;
					num = sqrMagnitude;
				}
				boardSquarePathInfo = boardSquarePathInfo.next;
			}
		}
		return result;
	}

	private void AdvanceGameplayPath()
	{
		m_gameplayPath = m_gameplayPath.next;
#if SERVER
		if (NetworkServer.active)  // server-only
		{
			GameFlowData.Get().OnServerActorMoved(m_actor, m_gameplayPath.prev, m_gameplayPath);
			if (m_gameplayPath.connectionType == BoardSquarePathInfo.ConnectionType.Teleport
				|| m_gameplayPath.connectionType == BoardSquarePathInfo.ConnectionType.Flight)
			{
				List<BoardSquare> squaresInBox = AreaEffectUtils.GetSquaresInBox(m_gameplayPath.prev.square.ToVector3(), m_gameplayPath.square.ToVector3(), 0.5f, true, m_actor);
				GameFlowData.Get().OnServerActorTeleported(m_actor, m_gameplayPath.prev, m_gameplayPath, squaresInBox);
			}
		}
#endif
		BoardSquare square = m_gameplayPath.square;
		bool isEnd = m_gameplayPath.next == null;
		m_actor.ActorData_OnActorMoved(square, m_gameplayPath.m_visibleToEnemies, m_gameplayPath.m_updateLastKnownPos);
		ActorVFX actorVFX = m_actor.GetActorVFX();
		if (actorVFX != null)
		{
			// reactor
			actorVFX.OnMove(m_gameplayPath, m_gameplayPath.prev);
			// rogues
			//actorVFX.OnMove(square);
		}
		CameraManager.Get().OnActorMoved(m_actor);
		ClientClashManager.Get().OnActorMoved_ClientClashManager(m_actor, m_gameplayPath);
		ClientResolutionManager.Get().OnActorMoved_ClientResolutionManager(m_actor, m_gameplayPath);
		if (m_actor != null && m_actor.GetActorModelData() != null && m_aestheticPath != null) // m_aestheticPath check added in rogues
		{
			m_actor.GetActorModelData().OnMovementAnimatorUpdate(m_aestheticPath.connectionType);
		}
		if (isEnd)
		{
			m_actor.UpdateFacingAfterMovement();
			// rogues
			//m_actor.UpdateAllyReviveButtons();
			if (GameFlowData.Get().gameState == GameState.BothTeams_Resolve
				&& HighlightUtils.Get() != null
				&& HighlightUtils.Get().m_coverDirIndicatorTiming == HighlightUtils.MoveIntoCoverIndicatorTiming.ShowOnMoveEnd
				&& HighlightUtils.Get().m_showMoveIntoCoverIndicators)
			{
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (activeOwnedActorData != null
					&& activeOwnedActorData == m_actor
					&& m_actor.GetActorCover() != null
					&& m_actor.IsActorVisibleToClient())
				{
					m_actor.GetActorCover().RecalculateCover();
					m_actor.GetActorCover().StartShowMoveIntoCoverIndicator();
				}
			}
		}
	}

	private void CalculateMoveTimeout()
	{
		m_moveTimeoutTime = 0f;
		BoardSquarePathInfo boardSquarePathInfo = m_gameplayPath;
		ActorData actor = m_actor;
		while (boardSquarePathInfo != null)
		{
			BoardSquarePathInfo prev = boardSquarePathInfo.prev;
			Vector3 a;
			if (prev == null)
			{
				a = m_actor.transform.position;
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
					{
						if (boardSquarePathInfo.segmentMovementDuration > 0f)
						{
							m_moveTimeoutTime += boardSquarePathInfo.segmentMovementDuration * 1.5f + 3f;
						}
						else
						{
							Vector3 vector = a - boardSquarePathInfo.square.ToVector3();
							float speed = boardSquarePathInfo.segmentMovementSpeed > 0f
								? boardSquarePathInfo.segmentMovementSpeed
								: actor.m_runSpeed;
							m_moveTimeoutTime += vector.magnitude / speed * 1.5f + 3f;
						}
						break;
					}
			}
			boardSquarePathInfo = boardSquarePathInfo.next;
		}
	}

	public void BeginTravellingAlongPath(BoardSquarePathInfo gameplayPath, ActorData.MovementType movementType)
	{
		m_actor.SetTransformPositionToSquare(gameplayPath.square);
		m_actor.m_endVisibilityForHitTime = 0f;
		BoardSquare travelBoardSquare = GetTravelBoardSquare();
		m_gameplayPath = gameplayPath;
		if (travelBoardSquare != GetTravelBoardSquare())
		{
			m_actor.ForceUpdateIsVisibleToClientCache();
			// removed in rogues
			m_actor.ForceUpdateActorModelVisibility();
		}
		m_aestheticPath = m_gameplayPath.Clone(null);
		if (movementType == ActorData.MovementType.Normal)
		{
			MovementUtils.CreateRunAndVaultAestheticPath(ref m_aestheticPath, m_actor);
		}
		else
		{
			MovementUtils.CreateUnskippableAestheticPath(ref m_aestheticPath, movementType);
		}
		CalculateMoveTimeout();
		m_actor.SetTransformPositionToSquare(gameplayPath.square);
		UpdateMovementState();
	}

	public void BeginChargeOrKnockback(BoardSquare src, BoardSquare dest, BoardSquarePathInfo gameplayPath, ActorData.MovementType movementType) // BeginKnockback in rogues
	{
		if (movementType == ActorData.MovementType.Knockback)  // unconditional in rogues
		{
			GameEventManager.Get().FireEvent(GameEventManager.EventType.ActorKnockback, new GameEventManager.ActorKnockback
			{
				m_target = m_actor
			});
		}
		if (src == null && dest != null)
		{
			src = dest;
		}
		else if (src != null && dest == null)
		{
			dest = src;
		}
		if (src != null && dest != null)
		{
			m_actor.SetTransformPositionToSquare(src);
			m_gameplayPath = gameplayPath;
			m_aestheticPath = m_gameplayPath.Clone(null);
			MovementUtils.CreateUnskippableAestheticPath(ref m_aestheticPath, movementType);
			CalculateMoveTimeout();
			m_actor.SetTransformPositionToSquare(src);
			UpdateMovementState();
		}
	}

	public BoardSquarePathInfo BuildPathTo(BoardSquare src, BoardSquare dest)
	{
		return BuildPathTo(src, dest, CalculateMaxHorizontalMovement(), false, null);
	}

	public BoardSquarePathInfo BuildPathTo_IgnoreBarriers(BoardSquare src, BoardSquare dest)
	{
		return BuildPathTo(src, dest, CalculateMaxHorizontalMovement(), true, null);
	}

	// removed in rogues
	public BoardSquarePathInfo BuildPathTo(BoardSquare src, BoardSquare dest, float maxHorizontalMovement, bool ignoreBarriers, List<BoardSquare> claimedSquares)
	{
		if (src == null || dest == null)
		{
			return null;
		}

		BoardSquarePathInfo boardSquarePathInfo = null;
		BuildNormalPathNodePool normalPathBuildScratchPool = Board.Get().m_normalPathBuildScratchPool;
		normalPathBuildScratchPool.ResetAvailableNodeIndex();
		Vector3 tieBreakerDir = dest.ToVector3() - src.ToVector3();
		Vector3 tieBreakerPos = src.ToVector3();
		BuildNormalPathHeap normalPathNodeHeap = Board.Get().m_normalPathNodeHeap;
		normalPathNodeHeap.Clear();
		normalPathNodeHeap.SetTieBreakerDirAndPos(tieBreakerDir, tieBreakerPos);
		m_tempClosedSquares.Clear();
		Dictionary<BoardSquare, BoardSquarePathInfo> tempClosedSquares = m_tempClosedSquares;
		BoardSquarePathInfo allocatedNode = normalPathBuildScratchPool.GetAllocatedNode();
		allocatedNode.square = src;
		normalPathNodeHeap.Insert(allocatedNode);
		bool diagMovementAllowed = GameplayData.Get().m_diagonalMovement != GameplayData.DiagonalMovement.Disabled;
		bool canExceedMaxMovement = GameplayData.Get().m_movementMaximumType != GameplayData.MovementMaximumType.CannotExceedMax;
		bool destClaimed = claimedSquares?.Contains(dest) ?? false;

		List<BoardSquare> neighbours = new List<BoardSquare>(8);
		while (!normalPathNodeHeap.IsEmpty())
		{
			BoardSquarePathInfo pathNode = normalPathNodeHeap.ExtractTop();
			if (pathNode.square == dest)
			{
				boardSquarePathInfo = pathNode;
				break;
			}
			tempClosedSquares[pathNode.square] = pathNode;
			neighbours.Clear();
			if (!diagMovementAllowed)
			{
				Board.Get().GetCardinalAdjacentSquares(pathNode.square.x, pathNode.square.y, ref neighbours);
			}
			else
			{
				Board.Get().GetAllAdjacentSquares(pathNode.square.x, pathNode.square.y, ref neighbours);
			}
			foreach (BoardSquare neighbour in neighbours)
			{
				bool isDiagonalStep = Board.Get().GetSquaresAreDiagonallyAdjacent(pathNode.square, neighbour);
				float nextCost = pathNode.moveCost + (isDiagonalStep ? 1.5f : 1f);
				bool isValidMove = canExceedMaxMovement
					? pathNode.moveCost < maxHorizontalMovement
					: nextCost <= maxHorizontalMovement;
				DiagonalCalcFlag diagonalCalcFlag = isDiagonalStep ? DiagonalCalcFlag.IsDiagonal : DiagonalCalcFlag.NotDiagonal;
				if (isValidMove
					&& CanCrossToAdjacentSquare(pathNode.square, neighbour, ignoreBarriers, diagonalCalcFlag)
					&& FirstTurnMovement.CanActorMoveToSquare(m_actor, neighbour))
				{
					BoardSquarePathInfo allocatedNode2 = normalPathBuildScratchPool.GetAllocatedNode();
					allocatedNode2.square = neighbour;
					allocatedNode2.moveCost = nextCost;
					if (claimedSquares != null && destClaimed && allocatedNode2.square == dest)
					{
						int num3 = 1;
						BoardSquarePathInfo boardSquarePathInfo3 = pathNode;
						while (boardSquarePathInfo3 != null)
						{
							if (!claimedSquares.Contains(boardSquarePathInfo3.square))
							{
								break;
							}
							num3++;
							boardSquarePathInfo3 = boardSquarePathInfo3.prev;
						}
						allocatedNode2.m_expectedBackupNum = num3;
					}
					float squareSize = Board.Get().squareSize;
					if (!canExceedMaxMovement)
					{
						allocatedNode2.heuristicCost = (neighbour.transform.position - dest.transform.position).magnitude / squareSize;
					}
					else
					{
						float num4 = Mathf.Abs(neighbour.x - dest.x);
						float num5 = Mathf.Abs(neighbour.y - dest.y);
						float num6 = num4 + num5 - 0.5f * Mathf.Min(num4, num5);
						allocatedNode2.heuristicCost = Mathf.Max(0f, num6 - 1.01f);
					}
					allocatedNode2.prev = pathNode;
					bool flag5 = false;
					if (tempClosedSquares.ContainsKey(allocatedNode2.square))
					{
						flag5 = (allocatedNode2.F_cost > tempClosedSquares[allocatedNode2.square].F_cost);
					}
					if (!flag5)
					{
						bool flag6 = false;
						BoardSquarePathInfo boardSquarePathInfo4 = normalPathNodeHeap.TryGetNodeInHeapBySquare(allocatedNode2.square);
						if (boardSquarePathInfo4 != null)
						{
							flag6 = true;
							if (allocatedNode2.F_cost < boardSquarePathInfo4.F_cost)
							{
								normalPathNodeHeap.UpdatePriority(allocatedNode2);
							}
						}
						if (!flag6)
						{
							normalPathNodeHeap.Insert(allocatedNode2);
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

	public BoardSquarePathInfo BuildPathTo_Orig(BoardSquare src, BoardSquare dest, float maxHorizontalMovement, bool ignoreBarriers, List<BoardSquare> claimedSquares)  // BuildPathTo in rogues
	{
		BoardSquarePathInfo boardSquarePathInfo = null;
		if (src == null || dest == null)
		{
			return null;
		}
		Vector3 tieBreakDir = dest.ToVector3() - src.ToVector3();
		Vector3 tieBreakTestPos = src.ToVector3();
		BuildNormalPathNodePool normalPathBuildScratchPool = Board.Get().m_normalPathBuildScratchPool;
		normalPathBuildScratchPool.ResetAvailableNodeIndex();
		List<BoardSquarePathInfo> list = new List<BoardSquarePathInfo>();
		Dictionary<BoardSquare, BoardSquarePathInfo> dictionary = new Dictionary<BoardSquare, BoardSquarePathInfo>();
		BoardSquarePathInfo allocatedNode = normalPathBuildScratchPool.GetAllocatedNode();
		allocatedNode.square = src;
		list.Add(allocatedNode);
		List<BoardSquare> result = new List<BoardSquare>(8);
		bool isDiagonalEnabled = GameplayData.Get().m_diagonalMovement != GameplayData.DiagonalMovement.Disabled;
		bool canExceedMax = GameplayData.Get().m_movementMaximumType != GameplayData.MovementMaximumType.CannotExceedMax;
		bool isDestClaimed = claimedSquares != null && claimedSquares.Contains(dest);
		while (list.Count > 0)
		{
			list.Sort(delegate (BoardSquarePathInfo p1, BoardSquarePathInfo p2)
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
				break;
			}
			dictionary[boardSquarePathInfo2.square] = boardSquarePathInfo2;
			list.Remove(boardSquarePathInfo2);
			result.Clear();
			if (!isDiagonalEnabled)
			{
				Board.Get().GetCardinalAdjacentSquares(boardSquarePathInfo2.square.x, boardSquarePathInfo2.square.y, ref result);
			}
			else
			{
				Board.Get().GetAllAdjacentSquares(boardSquarePathInfo2.square.x, boardSquarePathInfo2.square.y, ref result);
			}
			for (int i = 0; i < result.Count; i++)
			{
				BoardSquare boardSquare = result[i];
				bool isDiagonalStep = Board.Get().GetSquaresAreDiagonallyAdjacent(boardSquarePathInfo2.square, boardSquare);
				float nextCost = isDiagonalStep
					? boardSquarePathInfo2.moveCost + 1.5f
					: boardSquarePathInfo2.moveCost + 1f;
				bool isValidMove = !canExceedMax
					? nextCost <= maxHorizontalMovement
					: boardSquarePathInfo2.moveCost < maxHorizontalMovement;
				if (!isValidMove)
				{
					continue;
				}
				BoardSquare square = boardSquarePathInfo2.square;
				DiagonalCalcFlag diagonalFlag = isDiagonalStep ? DiagonalCalcFlag.IsDiagonal : DiagonalCalcFlag.NotDiagonal;
				if (CanCrossToAdjacentSquare(square, boardSquare, ignoreBarriers, diagonalFlag) && FirstTurnMovement.CanActorMoveToSquare(m_actor, boardSquare))
				{
					BoardSquarePathInfo allocatedNode2 = normalPathBuildScratchPool.GetAllocatedNode();
					allocatedNode2.square = boardSquare;
					allocatedNode2.moveCost = nextCost;
					if (claimedSquares != null && isDestClaimed && allocatedNode2.square == dest)
					{
						int num3 = 1;
						BoardSquarePathInfo boardSquarePathInfo3 = boardSquarePathInfo2;
						while (boardSquarePathInfo3 != null && claimedSquares.Contains(boardSquarePathInfo3.square))
						{
							num3++;
							boardSquarePathInfo3 = boardSquarePathInfo3.prev;
						}
						allocatedNode2.m_expectedBackupNum = num3;
					}
					float squareSize = Board.Get().squareSize;
					if (!canExceedMax)
					{
						allocatedNode2.heuristicCost = (boardSquare.transform.position - dest.transform.position).magnitude / squareSize;
					}
					else
					{
						float num4 = Mathf.Abs(boardSquare.x - dest.x);
						float num5 = Mathf.Abs(boardSquare.y - dest.y);
						float num6 = num4 + num5 - 0.5f * Mathf.Min(num4, num5);
						allocatedNode2.heuristicCost = Mathf.Max(0f, num6 - 1.01f);
					}
					allocatedNode2.prev = boardSquarePathInfo2;
					bool flag6 = false;
					if (dictionary.ContainsKey(allocatedNode2.square))
					{
						flag6 = (allocatedNode2.F_cost > dictionary[allocatedNode2.square].F_cost);
					}
					if (flag6)
					{
						continue;
					}
					bool flag7 = false;
					for (int j = 0; j < list.Count; j++)
					{
						BoardSquarePathInfo boardSquarePathInfo4 = list[j];
						if (boardSquarePathInfo4.square == allocatedNode2.square)
						{
							flag7 = true;
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
					if (!flag7)
					{
						list.Add(allocatedNode2);
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

	// added in rogues
#if SERVER
	public BoardSquarePathInfo BuildCompletePathTo(BoardSquare src, BoardSquare dest, bool ignoreBarriers, List<BoardSquare> claimedSquares)
	{
		float maxHorizontalMovement = 12345f;
		return BuildPathTo(src, dest, maxHorizontalMovement, ignoreBarriers, claimedSquares);
	}
#endif

	// server-only
#if SERVER
	public bool AppendToPath(BoardSquarePathInfo pathSoFar, BoardSquare destination)
	{
		if (pathSoFar == null)
		{
			Log.Error("Attempted to AppendToPath with a null pathSoFar");
			return false;
		}
		float num = CalculateMaxHorizontalMovement(false, false);
		float num2 = pathSoFar.FindMoveCostToEnd();
		float num3 = num - num2;
		return num3 > 0f && AppendToPath(pathSoFar, destination, num2, num3);
	}
#endif


	// server-only
#if SERVER
	public bool AppendToPath(BoardSquarePathInfo pathSoFar, BoardSquare destination, float moveCostSoFar, float horizontalMovementRemaining)
	{
		if (pathSoFar == null)
		{
			Log.Error("Attempted to AppendToPath with a null pathSoFar");
			return false;
		}
		BoardSquarePathInfo pathEndpoint = pathSoFar.GetPathEndpoint();
		BoardSquarePathInfo boardSquarePathInfo = BuildPathTo(pathEndpoint.square, destination, horizontalMovementRemaining, false, null);
		if (boardSquarePathInfo != null)
		{
			boardSquarePathInfo = boardSquarePathInfo.next;
		}
		bool result;
		if (boardSquarePathInfo != null)
		{
			pathEndpoint.next = boardSquarePathInfo;
			pathEndpoint.next.prev = pathEndpoint;
			pathEndpoint.m_unskippable = true;
			for (BoardSquarePathInfo boardSquarePathInfo2 = boardSquarePathInfo; boardSquarePathInfo2 != null; boardSquarePathInfo2 = boardSquarePathInfo2.next)
			{
				boardSquarePathInfo2.moveCost += moveCostSoFar;
			}
			result = true;
		}
		else
		{
			result = false;
		}
		return result;
	}
#endif

	public BoardSquarePathInfo GetAestheticPath()
	{
		return m_curMoveState?.GetAestheticPath();
	}

	public BoardSquare GetTravelBoardSquare()
	{
		BoardSquare boardSquare = null;
		if (m_gameplayPath != null)
		{
			boardSquare = m_gameplayPath.square;
		}
		if (boardSquare == null)
		{
			boardSquare = m_actor.GetCurrentBoardSquare();
		}
		return boardSquare;
	}

	internal BoardSquarePathInfo GetPreviousTravelBoardSquarePathInfo()
	{
		if (m_gameplayPath != null && m_gameplayPath.prev != null)
		{
			return m_gameplayPath.prev;
		}
		return m_gameplayPath;
	}

	internal BoardSquarePathInfo GetNextTravelBoardSquarePathInfo()
	{
		if (m_gameplayPath != null && m_gameplayPath.next != null)
		{
			return m_gameplayPath.next;
		}
		return m_gameplayPath;
	}

	public void EncapsulatePathInBound(ref Bounds bound)
	{
		if (m_gameplayPath != null && m_actor != null)
		{
			TheatricsManager.EncapsulatePathInBound(ref bound, m_gameplayPath, m_actor);
		}
	}

	public bool IsOnLastSegment()
	{
		return m_aestheticPath != null && m_aestheticPath.next == null;
	}

	public bool IsYetToCompleteGameplayPath()
	{
		return m_gameplayPath != null && !m_gameplayPath.IsPathEndpoint();
	}

	internal bool IsPast2ndToLastSquare()
	{
		return m_gameplayPath == null || m_gameplayPath.next == null;
	}

	public void OnAssignedToInitialBoardSquare()
	{
		Vector3 spawnFacing = SpawnPointManager.Get().GetSpawnFacing(transform.position);
		m_actor.TurnToDirection(spawnFacing);

		// removed in rogues
		Animator modelAnimator = m_actor.GetModelAnimator();
		if (modelAnimator != null)
		{
			modelAnimator.SetBool(animDecisionPhase, true);
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			return;
		}
		Gizmos.color = Color.black;
		Gizmos.DrawWireCube(m_actor.transform.position, new Vector3(1.5f, 2.5f, 1.5f));
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
		for (BoardSquarePathInfo pi = m_aestheticPath; pi != null; pi = pi.next)
		{
			Gizmos.color = array[(int)pi.connectionType];
			if (pi.square != null)
			{
				Gizmos.DrawWireSphere(pi.square.GetOccupantRefPos(), num);
			}
			if (pi.prev != null && pi.prev.square != null)
			{
				Gizmos.DrawLine(pi.square.GetOccupantRefPos() + Vector3.up * num, pi.prev.square.GetOccupantRefPos() + Vector3.up * num);
			}
		}
		Gizmos.color = Color.black;
		for (BoardSquarePathInfo pi = m_gameplayPath; pi != null; pi = pi.next)
		{
			if (pi.square != null)
			{
				Gizmos.DrawSphere(pi.square.GetOccupantRefPos(), 0.15f);
			}
			if (pi.prev != null && pi.prev.square != null)
			{
				Gizmos.DrawLine(pi.square.GetOccupantRefPos(), pi.prev.square.GetOccupantRefPos());
			}
		}
		BoardSquare travelBoardSquare = GetTravelBoardSquare();
		if (travelBoardSquare != null)
		{
			Gizmos.color = Color.cyan;
			Gizmos.DrawWireCube(travelBoardSquare.CameraBounds.center, travelBoardSquare.CameraBounds.size);
		}
	}
}
