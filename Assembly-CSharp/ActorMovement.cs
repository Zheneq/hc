using System.Collections.Generic;
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

	internal HashSet<BoardSquare> SquaresCanMoveTo => m_squaresCanMoveTo;

	internal HashSet<BoardSquare> SquaresCanMoveToWithQueuedAbility => m_squareCanMoveToWithQueuedAbility;

	public string GetCurrentMoveStateStr()
	{
		if (m_curMoveState != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return m_curMoveState.stateName;
				}
			}
		}
		return "None";
	}

	private void Awake()
	{
		m_actor = GetComponent<ActorData>();
		m_squaresCanMoveToCache = new List<SquaresCanMoveToCacheEntry>();
		m_squaresCanMoveTo = new HashSet<BoardSquare>();
		m_squareCanMoveToWithQueuedAbility = new HashSet<BoardSquare>();
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
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_actor != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_actor.GetModelAnimator() != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					float num = m_brushTransitionAnimationSpeedEased;
					Animator modelAnimator = m_actor.GetModelAnimator();
					if (modelAnimator.speed != num)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						modelAnimator.speed = num;
					}
				}
			}
			if (!(m_actor == GameFlowData.Get().activeOwnedActorData))
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (!InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.MovementWaypointModifier))
				{
					if (!InputManager.Get().IsKeyBindingNewlyReleased(KeyPreference.MovementWaypointModifier))
					{
						return;
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				UpdateSquaresCanMoveTo();
				return;
			}
		}
	}

	public void UpdateSquaresCanMoveTo()
	{
		if (NetworkServer.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
		}
		float maxMoveDist = m_actor.RemainingHorizontalMovement;
		float innerMoveDist = m_actor.RemainingMovementWithQueuedAbility;
		BoardSquare squareToStartFrom = m_actor.MoveFromBoardSquare;
		int num;
		if (Options_UI.Get() != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (Options_UI.Get().GetShiftClickForMovementWaypoints() != InputManager.Get().IsKeyBindingHeld(KeyPreference.MovementWaypointModifier))
			{
				num = 1;
				goto IL_0097;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		num = ((!FirstTurnMovement.CanWaypoint()) ? 1 : 0);
		goto IL_0097;
		IL_0097:
		if (num != 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_actor == GameFlowData.Get().activeOwnedActorData)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				maxMoveDist = CalculateMaxHorizontalMovement();
				innerMoveDist = CalculateMaxHorizontalMovement(true);
				squareToStartFrom = m_actor.InitialMoveStartSquare;
			}
		}
		GetSquaresCanMoveTo_InnerOuter(squareToStartFrom, maxMoveDist, innerMoveDist, out m_squaresCanMoveTo, out m_squareCanMoveToWithQueuedAbility);
		if (Board.Get() != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			Board.Get().MarkForUpdateValidSquares();
		}
		if (!(m_actor == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			LineData component = m_actor.GetComponent<LineData>();
			if (component != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					component.OnCanMoveToSquaresUpdated();
					return;
				}
			}
			return;
		}
	}

	public bool CanMoveToBoardSquare(int x, int y)
	{
		bool result = false;
		BoardSquare boardSquare = Board.Get().GetBoardSquare(x, y);
		if ((bool)boardSquare)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = CanMoveToBoardSquare(boardSquare);
		}
		return result;
	}

	public bool CanMoveToBoardSquare(BoardSquare dest)
	{
		return m_squaresCanMoveTo.Contains(dest);
	}

	public BoardSquare GetClosestMoveableSquareTo(BoardSquare selectedSquare, bool alwaysIncludeMoverSquare = true)
	{
		object obj;
		if (alwaysIncludeMoverSquare)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			obj = m_actor.GetCurrentBoardSquare();
		}
		else
		{
			obj = null;
		}
		BoardSquare boardSquare = (BoardSquare)obj;
		float num;
		if (alwaysIncludeMoverSquare)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			num = boardSquare.HorizontalDistanceOnBoardTo(selectedSquare);
		}
		else
		{
			num = 100000f;
		}
		float num2 = num;
		if (selectedSquare != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					using (HashSet<BoardSquare>.Enumerator enumerator = m_squaresCanMoveTo.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BoardSquare current = enumerator.Current;
							float num3 = current.HorizontalDistanceOnBoardTo(selectedSquare);
							if (num3 <= num2)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								num2 = num3;
								boardSquare = current;
							}
						}
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								return boardSquare;
							}
						}
					}
				}
				}
			}
		}
		Log.Error("Trying to find the closest moveable square to a null square.  Code error-- tell Danny.");
		return boardSquare;
	}

	public float CalculateMaxHorizontalMovement(bool forcePostAbility = false, bool calculateAsIfSnared = false)
	{
		float result = 0f;
		if (!m_actor.IsDead())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = m_actor.GetActorStats().GetModifiedStatInt(StatType.Movement_Horizontal);
			AbilityData abilityData = m_actor.GetAbilityData();
			if ((bool)abilityData)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (abilityData.GetQueuedAbilitiesAllowMovement())
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					float num = 0f;
					num = ((!forcePostAbility) ? abilityData.GetQueuedAbilitiesMovementAdjust() : (-1f * m_actor.GetPostAbilityHorizontalMovementChange()));
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
		float num = movement;
		ActorStatus actorStatus = m_actor.GetActorStatus();
		if (actorStatus.HasStatus(StatusType.RecentlySpawned))
		{
			num += (float)GameplayData.Get().m_recentlySpawnedBonusMovement;
		}
		if (actorStatus.HasStatus(StatusType.RecentlyRespawned))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num += (float)GameplayData.Get().m_recentlyRespawnedBonusMovement;
		}
		List<StatusType> queuedAbilitiesOnRequestStatuses = m_actor.GetAbilityData().GetQueuedAbilitiesOnRequestStatuses();
		bool flag = actorStatus.HasStatus(StatusType.MovementDebuffSuppression) || queuedAbilitiesOnRequestStatuses.Contains(StatusType.MovementDebuffSuppression);
		int num2;
		if (!actorStatus.IsMovementDebuffImmune() && !queuedAbilitiesOnRequestStatuses.Contains(StatusType.MovementDebuffImmunity))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			num2 = (queuedAbilitiesOnRequestStatuses.Contains(StatusType.Unstoppable) ? 1 : 0);
		}
		else
		{
			num2 = 1;
		}
		bool flag2 = (byte)num2 != 0;
		bool flag3 = !flag && !flag2;
		int num3;
		if (!actorStatus.HasStatus(StatusType.CantSprint_UnlessUnstoppable))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			num3 = (queuedAbilitiesOnRequestStatuses.Contains(StatusType.CantSprint_UnlessUnstoppable) ? 1 : 0);
		}
		else
		{
			num3 = 1;
		}
		bool flag4 = (byte)num3 != 0;
		int num4;
		if (!actorStatus.HasStatus(StatusType.CantSprint_Absolute))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			num4 = (queuedAbilitiesOnRequestStatuses.Contains(StatusType.CantSprint_Absolute) ? 1 : 0);
		}
		else
		{
			num4 = 1;
		}
		bool flag5 = (byte)num4 != 0;
		int num5;
		if (flag4)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (flag3)
			{
				num5 = 1;
				goto IL_0129;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		num5 = (flag5 ? 1 : 0);
		goto IL_0129;
		IL_0129:
		bool flag6 = (byte)num5 != 0;
		if (flag3)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (actorStatus.HasStatus(StatusType.Rooted))
			{
				goto IL_0165;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (actorStatus.HasStatus(StatusType.AnchoredNoMovement))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			goto IL_0165;
		}
		if (flag3)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (actorStatus.HasStatus(StatusType.CrippledMovement))
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				num = Mathf.Clamp(num, 0f, 1f);
				goto IL_03df;
			}
		}
		if (flag6 && !forcePostAbility && m_actor.GetAbilityData() != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_actor.GetAbilityData().GetQueuedAbilitiesMovementAdjustType() == Ability.MovementAdjustment.FullMovement)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				num -= m_actor.GetPostAbilityHorizontalMovementChange();
			}
		}
		int num6;
		if (!actorStatus.HasStatus(StatusType.Snared))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			num6 = (queuedAbilitiesOnRequestStatuses.Contains(StatusType.Snared) ? 1 : 0);
		}
		else
		{
			num6 = 1;
		}
		bool flag7 = (byte)num6 != 0;
		int num7;
		if (!actorStatus.HasStatus(StatusType.Hasted))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			num7 = (queuedAbilitiesOnRequestStatuses.Contains(StatusType.Hasted) ? 1 : 0);
		}
		else
		{
			num7 = 1;
		}
		bool flag8 = (byte)num7 != 0;
		if (flag3)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (flag7)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!flag8)
				{
					goto IL_028a;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		if (calculateAsIfSnared)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			goto IL_028a;
		}
		if (flag8)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (flag3)
			{
				if (flag7)
				{
					goto IL_03df;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			CalcHastedMovementAdjustments(out float mult, out int halfMoveAdjustment, out int fullMoveAdjustment);
			if (forcePostAbility)
			{
				num = Mathf.Clamp(num + (float)halfMoveAdjustment, 0f, 99f);
			}
			else
			{
				int num8 = fullMoveAdjustment;
				if (m_actor.GetAbilityData() != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					Ability.MovementAdjustment queuedAbilitiesMovementAdjustType = m_actor.GetAbilityData().GetQueuedAbilitiesMovementAdjustType();
					if (queuedAbilitiesMovementAdjustType == Ability.MovementAdjustment.ReducedMovement)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						num8 = halfMoveAdjustment;
					}
				}
				num = Mathf.Clamp(num + (float)num8, 0f, 99f);
			}
			num *= mult;
			num = MovementUtils.RoundToNearestHalf(num);
		}
		goto IL_03df;
		IL_03df:
		return num;
		IL_028a:
		CalcSnaredMovementAdjustments(out float mult2, out int halfMoveAdjust, out int fullMoveAdjust);
		if (forcePostAbility)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			num = Mathf.Clamp(num + (float)halfMoveAdjust, 0f, 99f);
		}
		else
		{
			int num9 = fullMoveAdjust;
			if (m_actor.GetAbilityData() != null)
			{
				Ability.MovementAdjustment queuedAbilitiesMovementAdjustType2 = m_actor.GetAbilityData().GetQueuedAbilitiesMovementAdjustType();
				if (queuedAbilitiesMovementAdjustType2 == Ability.MovementAdjustment.ReducedMovement)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					num9 = halfMoveAdjust;
				}
			}
			num = Mathf.Clamp(num + (float)num9, 0f, 99f);
		}
		num *= mult2;
		num = MovementUtils.RoundToNearestHalf(num);
		goto IL_03df;
		IL_0165:
		num = 0f;
		goto IL_03df;
	}

	public static void CalcHastedMovementAdjustments(out float mult, out int halfMoveAdjustment, out int fullMoveAdjustment)
	{
		if (!(GameplayMutators.Get() == null))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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

	public static void CalcSnaredMovementAdjustments(out float mult, out int halfMoveAdjust, out int fullMoveAdjust)
	{
		if (!(GameplayMutators.Get() == null))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (GameplayMutators.Get().m_useSlowOverride)
			{
				halfMoveAdjust = GameplayMutators.Get().m_slowHalfMovementAdjustAmount;
				fullMoveAdjust = GameplayMutators.Get().m_slowFullMovementAdjustAmount;
				mult = GameplayMutators.Get().m_slowMovementMultiplier;
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		halfMoveAdjust = GameWideData.Get().m_slowHalfMovementAdjustAmount;
		fullMoveAdjust = GameWideData.Get().m_slowFullMovementAdjustAmount;
		mult = GameWideData.Get().m_slowMovementMultiplier;
	}

	public void BuildSquaresCanMoveTo_InnerOuter(BoardSquare squareToStartFrom, float maxHorizontalMovement, float innerMaxMove, out HashSet<BoardSquare> eligibleSquares, out HashSet<BoardSquare> innerSquares)
	{
		eligibleSquares = new HashSet<BoardSquare>();
		innerSquares = new HashSet<BoardSquare>();
		if (squareToStartFrom == null)
		{
			return;
		}
		BoardSquareMovementInfo value = default(BoardSquareMovementInfo);
		BoardSquareMovementInfo value3 = default(BoardSquareMovementInfo);
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (maxHorizontalMovement == 0f)
			{
				while (true)
				{
					switch (3)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			eligibleSquares.Add(squareToStartFrom);
			if (innerMaxMove > 0f)
			{
				innerSquares.Add(squareToStartFrom);
			}
			LinkedList<BoardSquareMovementInfo> linkedList = new LinkedList<BoardSquareMovementInfo>();
			HashSet<BoardSquare> hashSet = new HashSet<BoardSquare>();
			value.square = squareToStartFrom;
			value.cost = 0f;
			value.prevCost = 0f;
			linkedList.AddLast(value);
			int num;
			if (GameplayData.Get() != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				num = ((GameplayData.Get().m_movementMaximumType == GameplayData.MovementMaximumType.CannotExceedMax) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			bool flag = (byte)num != 0;
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
						if (i == 0)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (j == 0)
							{
								continue;
							}
						}
						BoardSquare boardSquare = board.GetBoardSquare(x + i, y + j);
						if (boardSquare == null)
						{
							continue;
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (hashSet.Contains(boardSquare))
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							continue;
						}
						bool flag2 = board._0015(square, boardSquare);
						float num2;
						if (flag2)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							num2 = value2.cost + 1.5f;
						}
						else
						{
							num2 = value2.cost + 1f;
						}
						if (!((!flag) ? (value2.cost < maxHorizontalMovement) : (num2 <= maxHorizontalMovement)))
						{
							continue;
						}
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						int diagonalFlag;
						if (flag2)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							diagonalFlag = 1;
						}
						else
						{
							diagonalFlag = 2;
						}
						if (!CanCrossToAdjacentSquare(square, boardSquare, false, (DiagonalCalcFlag)diagonalFlag))
						{
							continue;
						}
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						value3.square = boardSquare;
						value3.cost = num2;
						value3.prevCost = value2.cost;
						bool flag3 = false;
						LinkedListNode<BoardSquareMovementInfo> linkedListNode = linkedList.First;
						while (true)
						{
							if (linkedListNode != linkedList.Last)
							{
								BoardSquareMovementInfo value4 = linkedListNode.Value;
								if (value4.square == boardSquare)
								{
									flag3 = true;
									if (value4.cost > num2)
									{
										while (true)
										{
											switch (7)
											{
											case 0:
												continue;
											}
											break;
										}
										linkedListNode.Value = value3;
									}
									else if (value4.cost == num2 && value3.prevCost < value4.prevCost)
									{
										while (true)
										{
											switch (7)
											{
											case 0:
												continue;
											}
											break;
										}
										linkedListNode.Value = value3;
									}
									break;
								}
								linkedListNode = linkedListNode.Next;
								continue;
							}
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							break;
						}
						if (!flag3 && FirstTurnMovement.CanActorMoveToSquare(m_actor, boardSquare))
						{
							linkedList.AddLast(value3);
						}
					}
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					if (MovementUtils.CanStopOnSquare(square) && SinglePlayerManager.IsDestinationAllowed(m_actor, square))
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (FirstTurnMovement.CanActorMoveToSquare(m_actor, square))
						{
							if (!eligibleSquares.Contains(square))
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								eligibleSquares.Add(square);
							}
							if (innerMaxMove > 0f && !innerSquares.Contains(square))
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								bool flag4 = false;
								if (flag)
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									flag4 = (value2.cost <= innerMaxMove);
								}
								else
								{
									flag4 = (value2.prevCost < innerMaxMove);
								}
								if (flag4)
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										break;
									}
									innerSquares.Add(square);
								}
							}
						}
					}
					hashSet.Add(square);
					linkedList.RemoveFirst();
					goto IL_03e8;
				}
				IL_03e8:;
			}
			while (true)
			{
				switch (6)
				{
				default:
					return;
				case 0:
					break;
				}
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
		LinkedList<BoardSquareMovementInfo> linkedList = new LinkedList<BoardSquareMovementInfo>();
		HashSet<BoardSquare> hashSet2 = new HashSet<BoardSquare>();
		BoardSquareMovementInfo value = default(BoardSquareMovementInfo);
		value.square = squareToStartFrom;
		value.cost = 0f;
		value.prevCost = 0f;
		linkedList.AddLast(value);
		List<BoardSquare> result = new List<BoardSquare>(8);
		BoardSquareMovementInfo value3 = default(BoardSquareMovementInfo);
		while (linkedList.Count > 0)
		{
			BoardSquareMovementInfo value2 = linkedList.First.Value;
			BoardSquare square = value2.square;
			result.Clear();
			if (GameplayData.Get() != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (GameplayData.Get().m_diagonalMovement == GameplayData.DiagonalMovement.Disabled)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					Board.Get().GetStraightAdjacentSquares(square.x, square.y, ref result);
					goto IL_00fb;
				}
			}
			Board.Get().GetAllAdjacentSquares(square.x, square.y, ref result);
			goto IL_00fb;
			IL_00fb:
			for (int i = 0; i < result.Count; i++)
			{
				BoardSquare boardSquare = result[i];
				if (hashSet2.Contains(boardSquare))
				{
					continue;
				}
				bool flag = Board.Get()._0015(square, boardSquare);
				float num;
				if (flag)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					num = value2.cost + 1.5f;
				}
				else
				{
					num = value2.cost + 1f;
				}
				bool flag2;
				if (GameplayData.Get() != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (GameplayData.Get().m_movementMaximumType == GameplayData.MovementMaximumType.CannotExceedMax)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						flag2 = (num <= maxHorizontalMovement);
						goto IL_01a6;
					}
				}
				flag2 = (value2.cost < maxHorizontalMovement);
				goto IL_01a6;
				IL_01a6:
				if (!flag2)
				{
					continue;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				value3.square = boardSquare;
				value3.cost = num;
				value3.prevCost = value2.cost;
				bool flag3 = false;
				LinkedListNode<BoardSquareMovementInfo> linkedListNode = linkedList.First;
				while (true)
				{
					if (linkedListNode != linkedList.Last)
					{
						BoardSquareMovementInfo value4 = linkedListNode.Value;
						if (value4.square == boardSquare)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							flag3 = true;
							if (value4.cost > num)
							{
								linkedListNode.Value = value3;
							}
							break;
						}
						linkedListNode = linkedListNode.Next;
						continue;
					}
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					break;
				}
				if (flag3)
				{
					continue;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (CanCrossToAdjacentSquare(square, boardSquare, false, flag ? DiagonalCalcFlag.IsDiagonal : DiagonalCalcFlag.NotDiagonal) && FirstTurnMovement.CanActorMoveToSquare(m_actor, boardSquare))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					linkedList.AddLast(value3);
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (!hashSet.Contains(square))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (MovementUtils.CanStopOnSquare(square))
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (SinglePlayerManager.IsDestinationAllowed(m_actor, square))
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							if (FirstTurnMovement.CanActorMoveToSquare(m_actor, square))
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								hashSet.Add(square);
							}
						}
					}
				}
				hashSet2.Add(square);
				linkedList.RemoveFirst();
				goto IL_032c;
			}
			IL_032c:;
		}
		return hashSet;
	}

	public HashSet<BoardSquare> CheckSquareCanMoveToCache(BoardSquare squareToStartFrom, float maxHorizontalMovement)
	{
		HashSet<BoardSquare> result = null;
		int squaresCanMoveToSinglePlayerState = -1;
		if (SinglePlayerManager.Get() != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_actor.SpawnerId == -1)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				squaresCanMoveToSinglePlayerState = SinglePlayerManager.Get().GetCurrentScriptIndex();
			}
		}
		int squaresCanMoveToBarrierState = -1;
		if (BarrierManager.Get() != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			squaresCanMoveToBarrierState = BarrierManager.Get().GetMovementStateChangesFor(m_actor);
		}
		FirstTurnMovement.RestrictedMovementState squaresCanMoveToFirstTurnState = FirstTurnMovement.RestrictedMovementState.Invalid;
		if (FirstTurnMovement.Get() != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			squaresCanMoveToFirstTurnState = FirstTurnMovement.Get().GetRestrictedMovementState();
		}
		SquaresCanMoveToCacheEntry squaresCanMoveToCacheEntry = new SquaresCanMoveToCacheEntry();
		squaresCanMoveToCacheEntry.m_squaresCanMoveToOrigin = squareToStartFrom;
		squaresCanMoveToCacheEntry.m_squaresCanMoveToHorizontalAllowed = maxHorizontalMovement;
		squaresCanMoveToCacheEntry.m_squaresCanMoveToSinglePlayerState = squaresCanMoveToSinglePlayerState;
		squaresCanMoveToCacheEntry.m_squaresCanMoveToBarrierState = squaresCanMoveToBarrierState;
		squaresCanMoveToCacheEntry.m_squaresCanMoveToFirstTurnState = squaresCanMoveToFirstTurnState;
		int num = 0;
		SquaresCanMoveToCacheEntry item = null;
		int num2 = 0;
		while (true)
		{
			if (num2 < m_squaresCanMoveToCache.Count)
			{
				SquaresCanMoveToCacheEntry squaresCanMoveToCacheEntry2 = m_squaresCanMoveToCache[num2];
				if (squaresCanMoveToCacheEntry2.m_squaresCanMoveToOrigin == squaresCanMoveToCacheEntry.m_squaresCanMoveToOrigin)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (squaresCanMoveToCacheEntry2.m_squaresCanMoveToHorizontalAllowed == squaresCanMoveToCacheEntry.m_squaresCanMoveToHorizontalAllowed)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (squaresCanMoveToCacheEntry2.m_squaresCanMoveToSinglePlayerState == squaresCanMoveToCacheEntry.m_squaresCanMoveToSinglePlayerState)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							if (squaresCanMoveToCacheEntry2.m_squaresCanMoveToBarrierState == squaresCanMoveToCacheEntry.m_squaresCanMoveToBarrierState && squaresCanMoveToCacheEntry2.m_squaresCanMoveToFirstTurnState == squaresCanMoveToCacheEntry.m_squaresCanMoveToFirstTurnState)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								result = squaresCanMoveToCacheEntry2.m_squaresCanMoveTo;
								item = squaresCanMoveToCacheEntry2;
								num = num2;
								break;
							}
						}
					}
				}
				num2++;
				continue;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			break;
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
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			squaresCanMoveToSinglePlayerState = SinglePlayerManager.Get().GetCurrentScriptIndex();
		}
		if (BarrierManager.Get() != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			squaresCanMoveToBarrierState = BarrierManager.Get().GetMovementStateChangesFor(m_actor);
		}
		if (FirstTurnMovement.Get() != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			squaresCanMoveToFirstTurnState = FirstTurnMovement.Get().GetRestrictedMovementState();
		}
		SquaresCanMoveToCacheEntry squaresCanMoveToCacheEntry = new SquaresCanMoveToCacheEntry();
		squaresCanMoveToCacheEntry.m_squaresCanMoveToOrigin = squareToStartFrom;
		squaresCanMoveToCacheEntry.m_squaresCanMoveToHorizontalAllowed = maxHorizontalMovement;
		squaresCanMoveToCacheEntry.m_squaresCanMoveToSinglePlayerState = squaresCanMoveToSinglePlayerState;
		squaresCanMoveToCacheEntry.m_squaresCanMoveToBarrierState = squaresCanMoveToBarrierState;
		squaresCanMoveToCacheEntry.m_squaresCanMoveToFirstTurnState = squaresCanMoveToFirstTurnState;
		squaresCanMoveToCacheEntry.m_squaresCanMoveTo = squaresCanMoveTo;
		if (m_squaresCanMoveToCache.Count >= s_maxSquaresCanMoveToCacheCount)
		{
			m_squaresCanMoveToCache.RemoveAt(m_squaresCanMoveToCache.Count - 1);
		}
		m_squaresCanMoveToCache.Insert(0, squaresCanMoveToCacheEntry);
	}

	public HashSet<BoardSquare> GetSquaresCanMoveTo(BoardSquare squareToStartFrom, float maxHorizontalMovement)
	{
		HashSet<BoardSquare> hashSet = CheckSquareCanMoveToCache(squareToStartFrom, maxHorizontalMovement);
		if (hashSet != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return hashSet;
				}
			}
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
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			BuildSquaresCanMoveTo_InnerOuter(squareToStartFrom, maxMoveDist, innerMoveDist, out eligibleSquares, out innerSquares);
			AddToSquareCanMoveToCache(squareToStartFrom, maxMoveDist, eligibleSquares);
			AddToSquareCanMoveToCache(squareToStartFrom, innerMoveDist, innerSquares);
		}
		else if (eligibleSquares == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
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
		bool flag;
		if (!(dest == null))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (dest.IsBaselineHeight())
			{
				if (src.GetCoverInDirection(VectorUtils.GetCoverDirection(src, dest)) == ThinCover.CoverType.Full)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						return false;
					}
				}
				if (!ignoreBarriers)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (BarrierManager.Get() != null)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (BarrierManager.Get().IsMovementBlocked(m_actor, src, dest))
						{
							return false;
						}
					}
				}
				int num;
				if (!knownAdjacent)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					num = (Board.Get()._000E(src, dest) ? 1 : 0);
				}
				else
				{
					num = 1;
				}
				if (num == 0)
				{
					return false;
				}
				flag = true;
				if (diagonalFlag == DiagonalCalcFlag.IsDiagonal)
				{
					goto IL_00f6;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (diagonalFlag == DiagonalCalcFlag.Unknown)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (Board.Get()._0015(src, dest))
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						goto IL_00f6;
					}
				}
				goto IL_018a;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
		IL_00f6:
		BoardSquare boardSquare = Board.Get().GetBoardSquare(src.x, dest.y);
		BoardSquare boardSquare2 = Board.Get().GetBoardSquare(dest.x, src.y);
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			flag &= CanCrossToAdjacentSingleSquare(src, boardSquare, ignoreBarriers, true, DiagonalCalcFlag.NotDiagonal);
		}
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			flag &= CanCrossToAdjacentSingleSquare(src, boardSquare2, ignoreBarriers, true, DiagonalCalcFlag.NotDiagonal);
		}
		if (flag)
		{
			flag &= CanCrossToAdjacentSingleSquare(boardSquare, dest, ignoreBarriers, true, DiagonalCalcFlag.NotDiagonal);
		}
		if (flag)
		{
			flag &= CanCrossToAdjacentSingleSquare(boardSquare2, dest, ignoreBarriers, true, DiagonalCalcFlag.NotDiagonal);
		}
		goto IL_018a;
		IL_018a:
		if (!flag)
		{
			while (true)
			{
				switch (4)
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

	public void ClearPath()
	{
		if (m_gameplayPath != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Vector3 point = hitInfo.point;
			result.y = point.y;
		}
		return result;
	}

	public bool ShouldSetForceIdle()
	{
		Animator modelAnimator = m_actor.GetModelAnimator();
		int result;
		if (!(modelAnimator == null))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (modelAnimator.layerCount >= 1)
			{
				AnimatorStateInfo currentAnimatorStateInfo = modelAnimator.GetCurrentAnimatorStateInfo(0);
				if (!currentAnimatorStateInfo.IsName("Run_Vault"))
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!currentAnimatorStateInfo.IsName("Run_End"))
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!currentAnimatorStateInfo.IsTag("Run_End"))
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!currentAnimatorStateInfo.IsName("KnockbackEnd") && !currentAnimatorStateInfo.IsTag("Knockdown") && !currentAnimatorStateInfo.IsTag("Damage"))
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								result = ((!m_actor.GetActorModelData().IsPlayingIdleAnim()) ? 1 : 0);
								goto IL_00f2;
							}
						}
					}
				}
				result = 0;
				goto IL_00f2;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
		IL_00f2:
		return (byte)result != 0;
	}

	internal void OnGameStateChange(bool decisionState)
	{
		Animator modelAnimator = m_actor.GetModelAnimator();
		if (!decisionState)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_brushTransitionAnimationSpeedEased.GetEndValue() != 1f)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				m_brushTransitionAnimationSpeedEased.EaseTo(1f, 0f);
			}
		}
		while (m_gameplayPath != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_gameplayPath.next == null || m_actor.IsDead())
			{
				break;
			}
			AdvanceGameplayPath();
		}
		if (m_curMoveState != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			m_curMoveState = null;
			StopMovementAnimator();
		}
		if (modelAnimator != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (modelAnimator.GetInteger(animIdleType) < 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				modelAnimator.SetInteger(animIdleType, 0);
			}
		}
		if (ShouldSetForceIdle())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			modelAnimator.SetTrigger(animForceIdle);
		}
		if (m_actor.GetCurrentBoardSquare() != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			m_actor.SetTransformPositionToSquare(m_actor.GetCurrentBoardSquare());
		}
		Client_ClearAestheticPath();
		m_moveTimeoutTime = -1f;
	}

	public void ResetChargeCycleFlag()
	{
		Animator modelAnimator = m_actor.GetModelAnimator();
		if (!(modelAnimator != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			modelAnimator.SetInteger(animNextChargeCycleType, 0);
			modelAnimator.SetInteger(animChargeCycleType, 0);
			modelAnimator.SetInteger(animChargeEndType, 4);
			modelAnimator.SetFloat(animDistToWaypoint, 0f);
			return;
		}
	}

	private void UpdatePath()
	{
		bool flag = false;
		if (m_aestheticPath == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				Log.Error($"{m_actor.DisplayName} trying to UpdatePath with a null aesthetic path; exiting.");
				return;
			}
		}
		if (m_aestheticPath.next == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			while (m_gameplayPath.next != null)
			{
				AdvanceGameplayPath();
				int num;
				if (Application.isEditor && DebugParameters.Get() != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					num = (DebugParameters.Get().GetParameterAsBool("SkipFogOfWarUpdateOnMovement") ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				bool flag2 = (byte)num != 0;
				flag = !flag2;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			ActorCover actorCover = m_actor.GetActorCover();
			actorCover.RecalculateCover();
		}
		if (m_aestheticPath != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_aestheticPath.next != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				m_aestheticPath = m_aestheticPath.next;
				goto IL_0108;
			}
		}
		m_aestheticPath = null;
		goto IL_0108;
		IL_0108:
		if (!flag)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			m_actor.GetFogOfWar().MarkForRecalculateVisibility();
			UpdateClientFogOfWarIfNeeded();
			return;
		}
	}

	public bool AmMoving()
	{
		bool flag;
		if (m_curMoveState != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_curMoveState is ChargeState)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				ChargeState chargeState = m_curMoveState as ChargeState;
				if (chargeState.DoneMoving())
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = false;
				}
				else
				{
					flag = true;
				}
			}
			else if (m_curMoveState.m_done)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
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
		int result;
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			result = ((!ShouldPauseAnimator()) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool InChargeState()
	{
		return m_curMoveState != null && m_curMoveState is ChargeState;
	}

	internal float FindDistanceToEnd()
	{
		float result;
		if (AmMoving())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_gameplayPath != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				result = m_gameplayPath.FindDistanceToEnd();
				goto IL_003f;
			}
		}
		result = 0f;
		goto IL_003f;
		IL_003f:
		return result;
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
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						if (1 == 0)
						{
							/*OpCode not supported: LdMemberToken*/;
						}
						return true;
					}
				}
			}
		}
		return false;
	}

	internal bool OnPathType(BoardSquarePathInfo.ConnectionType type)
	{
		int result;
		if (m_aestheticPath != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = ((m_aestheticPath.connectionType == type) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public void OnMidMovementDeath()
	{
		UpdatePosition();
		if (m_curMoveState != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			BoardSquarePathInfo gameplayPathClosestTo = GetGameplayPathClosestTo(m_actor.transform.position);
			bool flag = false;
			while (m_gameplayPath != gameplayPathClosestTo)
			{
				AdvanceGameplayPath();
				int num;
				if (Application.isEditor && DebugParameters.Get() != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					num = (DebugParameters.Get().GetParameterAsBool("SkipFogOfWarUpdateOnMovement") ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				bool flag2 = (byte)num != 0;
				flag = !flag2;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (flag)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					m_actor.GetFogOfWar().MarkForRecalculateVisibility();
					UpdateClientFogOfWarIfNeeded();
				}
				int num2;
				if (!m_actor.IsDead())
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					num2 = ((!m_actor.IsModelAnimatorDisabled()) ? 1 : 0);
				}
				else
				{
					num2 = 0;
				}
				if (num2 == 0)
				{
					return;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					bool updatePath = m_curMoveState.m_updatePath;
					m_curMoveState.Update();
					UpdateValuesAnimator();
					if (!updatePath)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (m_curMoveState.m_updatePath)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							UpdatePath();
						}
					}
					if (m_curMoveState != null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!m_curMoveState.m_done)
						{
							return;
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					UpdateMovementState();
					UpdateValuesAnimator();
					if (m_curMoveState == null)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							StopMovementAnimator();
							return;
						}
					}
					return;
				}
			}
		}
	}

	private void UpdateClientFogOfWarIfNeeded()
	{
		if (!NetworkClient.active)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(FogOfWar.GetClientFog() != null))
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (!(GameFlowData.Get() != null))
				{
					return;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					if (!(GameFlowData.Get().LocalPlayerData != null))
					{
						return;
					}
					Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
					int num;
					if (m_actor.GetActorStatus() != null)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						num = (m_actor.GetActorStatus().HasStatus(StatusType.Revealed) ? 1 : 0);
					}
					else
					{
						num = 0;
					}
					bool flag = (byte)num != 0;
					bool flag2 = CaptureTheFlag.IsActorRevealedByFlag_Client(m_actor);
					if (m_actor.GetTeam() == teamViewing)
					{
						return;
					}
					if (!flag)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!flag2)
						{
							return;
						}
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					FogOfWar.GetClientFog().MarkForRecalculateVisibility();
					return;
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
		Animator modelAnimator = m_actor.GetModelAnimator();
		if (modelAnimator == null)
		{
			return;
		}
		if (ShouldPauseAnimator())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_brushTransitionAnimationSpeedEased.GetEndValue() != m_brushTransitionAnimationSpeed)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
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
		if (!(m_actor != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (m_actor.GetActorModelData() != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					m_actor.GetActorModelData().OnMovementAnimatorStop();
					return;
				}
			}
			return;
		}
	}

	private float GetDistToPathSquare(BoardSquare goalGridSquare)
	{
		Vector3 position = m_actor.transform.position;
		Vector3 worldPosition = goalGridSquare.GetWorldPosition();
		Vector3 vector = worldPosition - position;
		vector.y = 0f;
		return vector.magnitude;
	}

	public Vector3 GetCurrentMovementDir()
	{
		Vector3 result;
		if (m_actor != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_actor.transform != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_aestheticPath != null && m_aestheticPath.square != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					Vector3 position = m_actor.transform.position;
					Vector3 worldPosition = m_aestheticPath.square.GetWorldPosition();
					result = worldPosition - position;
					result.y = 0f;
					result.Normalize();
					goto IL_00b9;
				}
			}
		}
		result = Vector3.zero;
		goto IL_00b9;
		IL_00b9:
		return result;
	}

	private void UpdateValuesAnimator()
	{
		Animator modelAnimator = m_actor.GetModelAnimator();
		if (modelAnimator == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		float num;
		if (ShouldPauseAnimator())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			num = m_brushTransitionAnimationSpeed;
		}
		else
		{
			num = 1f;
		}
		float num2 = num;
		if (m_brushTransitionAnimationSpeedEased.GetEndValue() != num2)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			EasedOutFloat brushTransitionAnimationSpeedEased = m_brushTransitionAnimationSpeedEased;
			float duration;
			if (num2 == 1f)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				duration = 0f;
			}
			else
			{
				duration = m_brushTransitionAnimationSpeedEaseTime;
			}
			brushTransitionAnimationSpeedEased.EaseTo(num2, duration);
		}
		if (m_curMoveState != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			modelAnimator.SetFloat(animDistToGoal, GetDistToGoal());
			modelAnimator.SetFloat(animTimeInAnim, m_curMoveState.m_timeInAnim);
			if (m_curMoveState.m_forceAnimReset)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (ShouldSetForceIdle())
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					modelAnimator.SetTrigger(animForceIdle);
				}
			}
		}
		modelAnimator.SetInteger(animCurLinkType, MovementUtils.GetLinkType(m_aestheticPath));
		if (m_aestheticPath == null)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			modelAnimator.SetInteger(animNextLinkType, MovementUtils.GetLinkType(m_aestheticPath.next));
			modelAnimator.SetInteger(animChargeCycleType, (int)m_aestheticPath.chargeCycleType);
			modelAnimator.SetInteger(animChargeEndType, (int)m_aestheticPath.chargeEndType);
			modelAnimator.SetFloat(animDistToWaypoint, GetDistToPathSquare(m_aestheticPath.square));
			if (m_aestheticPath.next != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					modelAnimator.SetInteger(animNextChargeCycleType, (int)m_aestheticPath.next.chargeCycleType);
					return;
				}
			}
			return;
		}
	}

	private bool EnemyRunningIntoBrush(BoardSquarePathInfo pathEndInfo)
	{
		if (!m_actor.VisibleTillEndOfPhase && !m_actor.CurrentlyVisibleForAbilityCast && !m_actor.GetActorStatus().HasStatus(StatusType.Revealed, false))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!CaptureTheFlag.IsActorRevealedByFlag_Client(m_actor))
			{
				if (pathEndInfo == null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!m_actor.IsVisibleToClient())
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (m_actor.IsHiddenInBrush())
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (m_actor.GetActorMovement().IsPast2ndToLastSquare())
							{
								goto IL_013b;
							}
							while (true)
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
				if (pathEndInfo != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!pathEndInfo.m_visibleToEnemies)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!pathEndInfo.m_moverHasGameplayHitHere && pathEndInfo.square != null)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							if (BrushCoordinator.Get() != null && BrushCoordinator.Get().IsRegionFunctioning(pathEndInfo.square.BrushRegion))
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								goto IL_013b;
							}
						}
					}
				}
			}
		}
		int result = 0;
		goto IL_015a;
		IL_015a:
		return (byte)result != 0;
		IL_013b:
		result = ((ServerClientUtils.GetCurrentActionPhase() >= ActionBufferPhase.Movement || GameFlowData.Get().gameState == GameState.BothTeams_Decision) ? 1 : 0);
		goto IL_015a;
	}

	private float GetDistToGoal()
	{
		float num = 0f;
		Vector3 b = base.transform.position;
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
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
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
		Vector3 position = m_actor.transform.position;
		return position.y - groundPosition.y;
	}

	private void OnDeath()
	{
		UpdateMovementState();
	}

	private void UpdateMovementState()
	{
		if (m_actor.IsDead())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Client_ClearAestheticPath();
		}
		if (m_actor.IsModelAnimatorDisabled())
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			Client_ClearAestheticPath();
		}
		if (m_aestheticPath != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					{
						if (m_curMoveState != null)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							if (m_curMoveState.m_connectionType == BoardSquarePathInfo.ConnectionType.Run)
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								if (m_aestheticPath.connectionType == BoardSquarePathInfo.ConnectionType.Run)
								{
									goto IL_00ca;
								}
								while (true)
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
						Animator modelAnimator = m_actor.GetModelAnimator();
						if (modelAnimator != null)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							modelAnimator.SetTrigger(animMoveSegmentStart);
						}
						goto IL_00ca;
					}
					IL_00ca:
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
					if (m_actor != null)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								if (m_actor.GetActorModelData() != null)
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											break;
										default:
											m_actor.GetActorModelData().OnMovementAnimatorUpdate(m_aestheticPath.connectionType);
											return;
										}
									}
								}
								return;
							}
						}
					}
					return;
				}
			}
		}
		if (m_curMoveState == null)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			Animator modelAnimator2 = m_actor.GetModelAnimator();
			if (modelAnimator2 != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				modelAnimator2.SetTrigger(animMoveSegmentStart);
			}
			m_curMoveState = null;
			StopMovementAnimator();
			return;
		}
	}

	private BoardSquarePathInfo GetGameplayPathClosestTo(Vector3 pos)
	{
		Vector3 b = new Vector3(pos.x, 0f, pos.z);
		object obj;
		if (m_gameplayPath == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			obj = null;
		}
		else
		{
			obj = m_gameplayPath.next;
		}
		BoardSquarePathInfo boardSquarePathInfo = (BoardSquarePathInfo)obj;
		BoardSquarePathInfo result = m_gameplayPath;
		if (m_gameplayPath != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			Vector3 position = m_gameplayPath.square.transform.position;
			float x = position.x;
			Vector3 position2 = m_gameplayPath.square.transform.position;
			Vector3 a = new Vector3(x, 0f, position2.z);
			float num = (a - b).sqrMagnitude;
			while (boardSquarePathInfo != null)
			{
				Vector3 position3 = boardSquarePathInfo.square.transform.position;
				float x2 = position3.x;
				Vector3 position4 = boardSquarePathInfo.square.transform.position;
				Vector3 a2 = new Vector3(x2, 0f, position4.z);
				float sqrMagnitude = (a2 - b).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					result = boardSquarePathInfo;
					num = sqrMagnitude;
				}
				boardSquarePathInfo = boardSquarePathInfo.next;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return result;
	}

	private void AdvanceGameplayPath()
	{
		m_gameplayPath = m_gameplayPath.next;
		BoardSquare square = m_gameplayPath.square;
		bool flag = m_gameplayPath.next == null;
		m_actor.ActorData_OnActorMoved(square, m_gameplayPath.m_visibleToEnemies, m_gameplayPath.m_updateLastKnownPos);
		ActorVFX actorVFX = m_actor.GetActorVFX();
		if (actorVFX != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			actorVFX.OnMove(m_gameplayPath, m_gameplayPath.prev);
		}
		CameraManager.Get().OnActorMoved(m_actor);
		ClientClashManager.Get().OnActorMoved_ClientClashManager(m_actor, m_gameplayPath);
		ClientResolutionManager.Get().OnActorMoved_ClientResolutionManager(m_actor, m_gameplayPath);
		if (m_actor != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_actor.GetActorModelData() != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				m_actor.GetActorModelData().OnMovementAnimatorUpdate(m_aestheticPath.connectionType);
			}
		}
		if (!flag)
		{
			return;
		}
		m_actor.UpdateFacingAfterMovement();
		if (GameFlowData.Get().gameState != GameState.BothTeams_Resolve || !(HighlightUtils.Get() != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (HighlightUtils.Get().m_coverDirIndicatorTiming != 0 || !HighlightUtils.Get().m_showMoveIntoCoverIndicators)
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
				if (!(activeOwnedActorData != null))
				{
					return;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					if (!(activeOwnedActorData == m_actor))
					{
						return;
					}
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						if (!(m_actor.GetActorCover() != null))
						{
							return;
						}
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							if (m_actor.IsVisibleToClient())
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									m_actor.GetActorCover().RecalculateCover();
									m_actor.GetActorCover().StartShowMoveIntoCoverIndicator();
									return;
								}
							}
							return;
						}
					}
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
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
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
					break;
				}
				Vector3 vector = a - boardSquarePathInfo.square.ToVector3();
				float num;
				if (boardSquarePathInfo.segmentMovementSpeed > 0f)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					num = boardSquarePathInfo.segmentMovementSpeed;
				}
				else
				{
					num = actor.m_runSpeed;
				}
				float num2 = num;
				m_moveTimeoutTime += vector.magnitude / num2 * 1.5f + 3f;
				break;
			}
			}
			boardSquarePathInfo = boardSquarePathInfo.next;
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_actor.ForceUpdateIsVisibleToClientCache();
			m_actor.ForceUpdateActorModelVisibility();
		}
		m_aestheticPath = m_gameplayPath.Clone(null);
		if (movementType == ActorData.MovementType.Normal)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
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

	public void BeginChargeOrKnockback(BoardSquare src, BoardSquare dest, BoardSquarePathInfo gameplayPath, ActorData.MovementType movementType)
	{
		if (movementType == ActorData.MovementType.Knockback)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			GameEventManager.Get().FireEvent(GameEventManager.EventType.ActorKnockback, new GameEventManager.ActorKnockback
			{
				m_target = m_actor
			});
		}
		if (src == null && dest != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			src = dest;
		}
		else if (src != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (dest == null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				dest = src;
			}
		}
		if (!(src != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (dest != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					m_actor.SetTransformPositionToSquare(src);
					m_gameplayPath = gameplayPath;
					m_aestheticPath = m_gameplayPath.Clone(null);
					MovementUtils.CreateUnskippableAestheticPath(ref m_aestheticPath, movementType);
					CalculateMoveTimeout();
					m_actor.SetTransformPositionToSquare(src);
					UpdateMovementState();
					return;
				}
			}
			return;
		}
	}

	public BoardSquarePathInfo BuildPathTo(BoardSquare src, BoardSquare dest)
	{
		float maxHorizontalMovement = CalculateMaxHorizontalMovement();
		return BuildPathTo(src, dest, maxHorizontalMovement, false, null);
	}

	public BoardSquarePathInfo BuildPathTo_IgnoreBarriers(BoardSquare src, BoardSquare dest)
	{
		float maxHorizontalMovement = CalculateMaxHorizontalMovement();
		return BuildPathTo(src, dest, maxHorizontalMovement, true, null);
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
				m_tempClosedSquares.Clear();
				Dictionary<BoardSquare, BoardSquarePathInfo> tempClosedSquares = m_tempClosedSquares;
				BoardSquarePathInfo allocatedNode = normalPathBuildScratchPool.GetAllocatedNode();
				allocatedNode.square = src;
				normalPathNodeHeap.Insert(allocatedNode);
				List<BoardSquare> result = new List<BoardSquare>(8);
				bool flag = GameplayData.Get().m_diagonalMovement != GameplayData.DiagonalMovement.Disabled;
				bool flag2 = GameplayData.Get().m_movementMaximumType != GameplayData.MovementMaximumType.CannotExceedMax;
				int num;
				if (claimedSquares != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					num = (claimedSquares.Contains(dest) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				bool flag3 = (byte)num != 0;
				while (!normalPathNodeHeap.IsEmpty())
				{
					BoardSquarePathInfo boardSquarePathInfo2 = normalPathNodeHeap.ExtractTop();
					if (boardSquarePathInfo2.square == dest)
					{
						boardSquarePathInfo = boardSquarePathInfo2;
						break;
					}
					tempClosedSquares[boardSquarePathInfo2.square] = boardSquarePathInfo2;
					result.Clear();
					if (!flag)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						board.GetStraightAdjacentSquares(boardSquarePathInfo2.square.x, boardSquarePathInfo2.square.y, ref result);
					}
					else
					{
						board.GetAllAdjacentSquares(boardSquarePathInfo2.square.x, boardSquarePathInfo2.square.y, ref result);
					}
					for (int i = 0; i < result.Count; i++)
					{
						BoardSquare boardSquare = result[i];
						bool flag4 = board._0015(boardSquarePathInfo2.square, boardSquare);
						float num2;
						if (flag4)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							num2 = boardSquarePathInfo2.moveCost + 1.5f;
						}
						else
						{
							num2 = boardSquarePathInfo2.moveCost + 1f;
						}
						if (!(flag2 ? (boardSquarePathInfo2.moveCost < maxHorizontalMovement) : (num2 <= maxHorizontalMovement)))
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							continue;
						}
						BoardSquare square = boardSquarePathInfo2.square;
						int diagonalFlag;
						if (flag4)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							diagonalFlag = 1;
						}
						else
						{
							diagonalFlag = 2;
						}
						if (!CanCrossToAdjacentSquare(square, boardSquare, ignoreBarriers, (DiagonalCalcFlag)diagonalFlag))
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							continue;
						}
						if (!FirstTurnMovement.CanActorMoveToSquare(m_actor, boardSquare))
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							continue;
						}
						BoardSquarePathInfo allocatedNode2 = normalPathBuildScratchPool.GetAllocatedNode();
						allocatedNode2.square = boardSquare;
						allocatedNode2.moveCost = num2;
						if (claimedSquares != null && flag3 && allocatedNode2.square == dest)
						{
							int num3 = 1;
							BoardSquarePathInfo boardSquarePathInfo3 = boardSquarePathInfo2;
							while (boardSquarePathInfo3 != null)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								if (claimedSquares.Contains(boardSquarePathInfo3.square))
								{
									num3++;
									boardSquarePathInfo3 = boardSquarePathInfo3.prev;
									continue;
								}
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								break;
							}
							allocatedNode2.m_expectedBackupNum = num3;
						}
						float squareSize = board.squareSize;
						if (!flag2)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							allocatedNode2.heuristicCost = (boardSquare.transform.position - dest.transform.position).magnitude / squareSize;
						}
						else
						{
							float num4 = Mathf.Abs(boardSquare.x - dest.x);
							float num5 = Mathf.Abs(boardSquare.y - dest.y);
							float num6 = num4 + num5 - 0.5f * Mathf.Min(num4, num5);
							float num7 = allocatedNode2.heuristicCost = Mathf.Max(0f, num6 - 1.01f);
						}
						allocatedNode2.prev = boardSquarePathInfo2;
						bool flag5 = false;
						if (tempClosedSquares.ContainsKey(allocatedNode2.square))
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							flag5 = (allocatedNode2.F_cost > tempClosedSquares[allocatedNode2.square].F_cost);
						}
						if (flag5)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							continue;
						}
						bool flag6 = false;
						BoardSquarePathInfo boardSquarePathInfo4 = normalPathNodeHeap.TryGetNodeInHeapBySquare(allocatedNode2.square);
						if (boardSquarePathInfo4 != null)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							flag6 = true;
							if (allocatedNode2.F_cost < boardSquarePathInfo4.F_cost)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								normalPathNodeHeap.UpdatePriority(allocatedNode2);
							}
						}
						if (!flag6)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							normalPathNodeHeap.Insert(allocatedNode2);
						}
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (boardSquarePathInfo != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					while (boardSquarePathInfo.prev != null)
					{
						boardSquarePathInfo.prev.next = boardSquarePathInfo;
						boardSquarePathInfo = boardSquarePathInfo.prev;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					boardSquarePathInfo = boardSquarePathInfo.Clone(null);
					normalPathBuildScratchPool.ResetAvailableNodeIndex();
				}
				return boardSquarePathInfo;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
		}
		return boardSquarePathInfo;
	}

	public BoardSquarePathInfo BuildPathTo_Orig(BoardSquare src, BoardSquare dest, float maxHorizontalMovement, bool ignoreBarriers, List<BoardSquare> claimedSquares)
	{
		BoardSquarePathInfo boardSquarePathInfo = null;
		if (!(src == null))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!(dest == null))
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
				List<BoardSquare> result = new List<BoardSquare>(8);
				bool flag = GameplayData.Get().m_diagonalMovement != GameplayData.DiagonalMovement.Disabled;
				bool flag2 = GameplayData.Get().m_movementMaximumType != GameplayData.MovementMaximumType.CannotExceedMax;
				int num;
				if (claimedSquares != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					num = (claimedSquares.Contains(dest) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				bool flag3 = (byte)num != 0;
				while (true)
				{
					if (list.Count > 0)
					{
						list.Sort(delegate(BoardSquarePathInfo p1, BoardSquarePathInfo p2)
						{
							if (Mathf.Approximately(p1.F_cost, p2.F_cost))
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										break;
									default:
									{
										if (1 == 0)
										{
											/*OpCode not supported: LdMemberToken*/;
										}
										Vector3 from = p1.square.ToVector3() - tieBreakTestPos;
										Vector3 from2 = p2.square.ToVector3() - tieBreakTestPos;
										return Vector3.Angle(from, tieBreakDir).CompareTo(Vector3.Angle(from2, tieBreakDir));
									}
									}
								}
							}
							return p1.F_cost.CompareTo(p2.F_cost);
						});
						BoardSquarePathInfo boardSquarePathInfo2 = list[0];
						tieBreakTestPos = boardSquarePathInfo2.square.ToVector3();
						if (boardSquarePathInfo2.prev != null)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							tieBreakDir = boardSquarePathInfo2.square.ToVector3() - boardSquarePathInfo2.prev.square.ToVector3();
						}
						if (boardSquarePathInfo2.square == dest)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							boardSquarePathInfo = boardSquarePathInfo2;
							break;
						}
						dictionary[boardSquarePathInfo2.square] = boardSquarePathInfo2;
						list.Remove(boardSquarePathInfo2);
						result.Clear();
						if (!flag)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							Board.Get().GetStraightAdjacentSquares(boardSquarePathInfo2.square.x, boardSquarePathInfo2.square.y, ref result);
						}
						else
						{
							Board.Get().GetAllAdjacentSquares(boardSquarePathInfo2.square.x, boardSquarePathInfo2.square.y, ref result);
						}
						for (int i = 0; i < result.Count; i++)
						{
							BoardSquare boardSquare = result[i];
							bool flag4 = Board.Get()._0015(boardSquarePathInfo2.square, boardSquare);
							float num2;
							if (flag4)
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								num2 = boardSquarePathInfo2.moveCost + 1.5f;
							}
							else
							{
								num2 = boardSquarePathInfo2.moveCost + 1f;
							}
							bool flag5;
							if (!flag2)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								flag5 = (num2 <= maxHorizontalMovement);
							}
							else
							{
								flag5 = (boardSquarePathInfo2.moveCost < maxHorizontalMovement);
							}
							if (!flag5)
							{
								continue;
							}
							BoardSquare square = boardSquarePathInfo2.square;
							int diagonalFlag;
							if (flag4)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								diagonalFlag = 1;
							}
							else
							{
								diagonalFlag = 2;
							}
							if (!CanCrossToAdjacentSquare(square, boardSquare, ignoreBarriers, (DiagonalCalcFlag)diagonalFlag))
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								continue;
							}
							if (!FirstTurnMovement.CanActorMoveToSquare(m_actor, boardSquare))
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								continue;
							}
							BoardSquarePathInfo allocatedNode2 = normalPathBuildScratchPool.GetAllocatedNode();
							allocatedNode2.square = boardSquare;
							allocatedNode2.moveCost = num2;
							if (claimedSquares != null && flag3)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								if (allocatedNode2.square == dest)
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									int num3 = 1;
									BoardSquarePathInfo boardSquarePathInfo3 = boardSquarePathInfo2;
									while (boardSquarePathInfo3 != null)
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										if (claimedSquares.Contains(boardSquarePathInfo3.square))
										{
											num3++;
											boardSquarePathInfo3 = boardSquarePathInfo3.prev;
											continue;
										}
										while (true)
										{
											switch (5)
											{
											case 0:
												continue;
											}
											break;
										}
										break;
									}
									allocatedNode2.m_expectedBackupNum = num3;
								}
							}
							float squareSize = Board.Get().squareSize;
							if (!flag2)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								allocatedNode2.heuristicCost = (boardSquare.transform.position - dest.transform.position).magnitude / squareSize;
							}
							else
							{
								float num4 = Mathf.Abs(boardSquare.x - dest.x);
								float num5 = Mathf.Abs(boardSquare.y - dest.y);
								float num6 = num4 + num5 - 0.5f * Mathf.Min(num4, num5);
								float num7 = allocatedNode2.heuristicCost = Mathf.Max(0f, num6 - 1.01f);
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
								if (!(boardSquarePathInfo4.square == allocatedNode2.square))
								{
									continue;
								}
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								flag7 = true;
								if (allocatedNode2.F_cost < boardSquarePathInfo4.F_cost)
								{
									while (true)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									boardSquarePathInfo4.heuristicCost = allocatedNode2.heuristicCost;
									boardSquarePathInfo4.moveCost = allocatedNode2.moveCost;
									boardSquarePathInfo4.prev = allocatedNode2.prev;
									boardSquarePathInfo4.m_expectedBackupNum = allocatedNode2.m_expectedBackupNum;
								}
								break;
							}
							if (!flag7)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								list.Add(allocatedNode2);
							}
						}
						continue;
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					break;
				}
				if (boardSquarePathInfo != null)
				{
					while (boardSquarePathInfo.prev != null)
					{
						boardSquarePathInfo.prev.next = boardSquarePathInfo;
						boardSquarePathInfo = boardSquarePathInfo.prev;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					boardSquarePathInfo = boardSquarePathInfo.Clone(null);
					normalPathBuildScratchPool.ResetAvailableNodeIndex();
				}
				return boardSquarePathInfo;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return boardSquarePathInfo;
	}

	public BoardSquarePathInfo GetAestheticPath()
	{
		object result;
		if (m_curMoveState == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = null;
		}
		else
		{
			result = m_curMoveState.GetAestheticPath();
		}
		return (BoardSquarePathInfo)result;
	}

	public BoardSquare GetTravelBoardSquare()
	{
		BoardSquare boardSquare = null;
		if (m_gameplayPath != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
		BoardSquarePathInfo result;
		if (m_gameplayPath != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_gameplayPath.prev != null)
			{
				result = m_gameplayPath.prev;
				goto IL_0045;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		result = m_gameplayPath;
		goto IL_0045;
		IL_0045:
		return result;
	}

	internal BoardSquarePathInfo GetNextTravelBoardSquarePathInfo()
	{
		BoardSquarePathInfo result;
		if (m_gameplayPath != null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_gameplayPath.next != null)
			{
				result = m_gameplayPath.next;
				goto IL_003b;
			}
		}
		result = m_gameplayPath;
		goto IL_003b;
		IL_003b:
		return result;
	}

	public void EncapsulatePathInBound(ref Bounds bound)
	{
		if (m_gameplayPath == null)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_actor != null)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					TheatricsManager.EncapsulatePathInBound(ref bound, m_gameplayPath, m_actor);
					return;
				}
			}
			return;
		}
	}

	public bool IsOnLastSegment()
	{
		int result;
		if (m_aestheticPath != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = ((m_aestheticPath.next == null) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool IsYetToCompleteGameplayPath()
	{
		return m_gameplayPath != null && !m_gameplayPath.IsPathEndpoint();
	}

	internal bool IsPast2ndToLastSquare()
	{
		int result;
		if (m_gameplayPath != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = ((m_gameplayPath.next == null) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	public void OnAssignedToInitialBoardSquare()
	{
		Vector3 spawnFacing = SpawnPointManager.Get().GetSpawnFacing(base.transform.position);
		m_actor.TurnToDirection(spawnFacing);
		Animator modelAnimator = m_actor.GetModelAnimator();
		if (!(modelAnimator != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			modelAnimator.SetBool(animDecisionPhase, true);
			return;
		}
	}

	private void OnDrawGizmos()
	{
		if (!CameraManager.ShouldDrawGizmosForCurrentCamera())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		Gizmos.color = Color.black;
		Gizmos.DrawWireCube(m_actor.transform.position, new Vector3(1.5f, 2.5f, 1.5f));
		Color[] array = new Color[6]
		{
			Color.white,
			Color.magenta,
			Color.yellow,
			Color.red,
			Color.blue,
			Color.black
		};
		float num = 0.4f;
		for (BoardSquarePathInfo boardSquarePathInfo = m_aestheticPath; boardSquarePathInfo != null; boardSquarePathInfo = boardSquarePathInfo.next)
		{
			Gizmos.color = array[(int)boardSquarePathInfo.connectionType];
			if (boardSquarePathInfo.square != null)
			{
				Gizmos.DrawWireSphere(boardSquarePathInfo.square.GetWorldPosition(), num);
			}
			if (boardSquarePathInfo.prev != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (boardSquarePathInfo.prev.square != null)
				{
					Gizmos.DrawLine(boardSquarePathInfo.square.GetWorldPosition() + Vector3.up * num, boardSquarePathInfo.prev.square.GetWorldPosition() + Vector3.up * num);
				}
			}
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			Gizmos.color = Color.black;
			for (BoardSquarePathInfo boardSquarePathInfo2 = m_gameplayPath; boardSquarePathInfo2 != null; boardSquarePathInfo2 = boardSquarePathInfo2.next)
			{
				if (boardSquarePathInfo2.square != null)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					Gizmos.DrawSphere(boardSquarePathInfo2.square.GetWorldPosition(), 0.15f);
				}
				if (boardSquarePathInfo2.prev != null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (boardSquarePathInfo2.prev.square != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						Gizmos.DrawLine(boardSquarePathInfo2.square.GetWorldPosition(), boardSquarePathInfo2.prev.square.GetWorldPosition());
					}
				}
			}
			BoardSquare travelBoardSquare = GetTravelBoardSquare();
			if (travelBoardSquare != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					Gizmos.color = Color.cyan;
					Gizmos.DrawWireCube(travelBoardSquare.CameraBounds.center, travelBoardSquare.CameraBounds.size);
					return;
				}
			}
			return;
		}
	}
}
