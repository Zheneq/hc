using System.Collections;
using UnityEngine;

public class PatrolState : FSMState
{
	private int turnsToDelayRemaining = -1;

	[Tooltip("A PatrolPath is a collection of waypoints that the NPC will follow")]
	public PatrolPath m_PatrolPath;

	private void Start()
	{
		m_PatrolPath.Initialze();
		stateID = StateID.Patrol;
	}

	public override void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		base.OnGameEvent(eventType, args);
	}

	public override void OnEnter(NPCBrain thisBrain, StateID previousState)
	{
		base.OnEnter(thisBrain, previousState);
		m_PatrolPath.WaypointsVisitedThisCycle = 0;
		m_PatrolPath.PatrolCyclesCompleted = 0;
		if (!(m_PatrolPath.m_currentWayPoint == null))
		{
			return;
		}
		m_PatrolPath.m_currentWayPoint = m_PatrolPath.GetInitalWaypoint();
		if (m_PatrolPath.m_currentWayPoint == null)
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
			Log.Error("Could not find a waypoint to travel to. Did you forget to add in waypoints to a patrol path for NPC " + base.MyBrain.name);
		}
		GameEventManager.PatrolPointArgs args = new GameEventManager.PatrolPointArgs(GameEventManager.PatrolPointArgs.WhatHappenedType.MovingToNextPoint, base.MyActorData, m_PatrolPath.m_currentWayPoint, m_PatrolPath.mWayPoints.IndexOf(m_PatrolPath.m_currentWayPoint), m_PatrolPath, m_PatrolPath.m_AlternateDestination == null);
		GameEventManager.Get().FireEvent(GameEventManager.EventType.PatrolPointEvent, args);
	}

	public override IEnumerator OnTurn(NPCBrain thisBrain)
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
			NPCBrain_StateMachine nPCBrain_StateMachine = thisBrain as NPCBrain_StateMachine;
			AbilityData component = thisBrain.GetComponent<AbilityData>();
			ActorData component2 = nPCBrain_StateMachine.GetComponent<ActorData>();
			ActorTurnSM component3 = nPCBrain_StateMachine.GetComponent<ActorTurnSM>();
			BotController component4 = nPCBrain_StateMachine.GetComponent<BotController>();
			if (!component)
			{
				yield break;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (!component2 || !component3 || !component4)
				{
					yield break;
				}
				if (m_PatrolPath == null)
				{
					while (true)
					{
						switch (6)
						{
						default:
							yield break;
						case 0:
							break;
						}
					}
				}
				WayPoint currentWayPoint = m_PatrolPath.m_currentWayPoint;
				Board board = Board.Get();
				Vector3 position = currentWayPoint.transform.position;
				float x2 = position.x;
				Vector3 position2 = currentWayPoint.transform.position;
				BoardSquare boardSquare = board.GetBoardSquareSafe(x2, position2.z);
				BoardSquare currentBoardSquare = component2.GetCurrentBoardSquare();
				int num = 10;
				float num2 = boardSquare.HorizontalDistanceInSquaresTo(base.MyActorData.GetCurrentBoardSquare());
				float remainingHorizontalMovement = base.MyActorData.RemainingHorizontalMovement;
				if (!currentWayPoint.MustArriveAtWayPointToContinue && currentBoardSquare != boardSquare)
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
					if (num2 < remainingHorizontalMovement)
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
						if (m_PatrolPath.m_AlternateDestination == null)
						{
							while (true)
							{
								if (!(boardSquare.OccupantActor != null))
								{
									if (!(boardSquare.OccupantActor == base.MyActorData))
									{
										break;
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
								if (--num <= 0)
								{
									break;
								}
								boardSquare = Board.Get()._0018(boardSquare, boardSquare);
								m_PatrolPath.m_AlternateDestination = boardSquare;
							}
						}
					}
				}
				if (!(currentBoardSquare == boardSquare))
				{
					if (!(currentBoardSquare == m_PatrolPath.m_AlternateDestination))
					{
						component3.SelectMovementSquareForMovement(boardSquare);
						yield break;
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
				if (turnsToDelayRemaining == -1)
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
					Debug.Log("Arrived at point: " + boardSquare);
					GameEventManager.PatrolPointArgs args = new GameEventManager.PatrolPointArgs(GameEventManager.PatrolPointArgs.WhatHappenedType.PointReached, base.MyActorData, currentWayPoint, m_PatrolPath.mWayPoints.IndexOf(currentWayPoint), m_PatrolPath, m_PatrolPath.m_AlternateDestination == null);
					GameEventManager.Get().FireEvent(GameEventManager.EventType.PatrolPointEvent, args);
					turnsToDelayRemaining = currentWayPoint.TurnsToDelay;
				}
				if (turnsToDelayRemaining <= 0)
				{
					turnsToDelayRemaining = -1;
					currentWayPoint = (m_PatrolPath.m_currentWayPoint = m_PatrolPath.IncremementWayPoint(delegate(PatrolPath.IncrementWaypointResult x)
					{
						switch (x)
						{
						case PatrolPath.IncrementWaypointResult.Incremented:
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									if (1 == 0)
									{
										/*OpCode not supported: LdMemberToken*/;
									}
									m_PatrolPath.WaypointsVisitedThisCycle++;
									return;
								}
							}
						case PatrolPath.IncrementWaypointResult.CycleCompleted:
							m_PatrolPath.PatrolCyclesCompleted++;
							m_PatrolPath.WaypointsVisitedThisCycle = 0;
							break;
						}
					}));
					GameEventManager.PatrolPointArgs args2 = new GameEventManager.PatrolPointArgs(GameEventManager.PatrolPointArgs.WhatHappenedType.MovingToNextPoint, base.MyActorData, currentWayPoint, m_PatrolPath.mWayPoints.IndexOf(currentWayPoint), m_PatrolPath, m_PatrolPath.m_AlternateDestination == null);
					GameEventManager.Get().FireEvent(GameEventManager.EventType.PatrolPointEvent, args2);
					m_PatrolPath.m_AlternateDestination = null;
					Board board2 = Board.Get();
					Vector3 position3 = currentWayPoint.transform.position;
					float x3 = position3.x;
					Vector3 position4 = currentWayPoint.transform.position;
					BoardSquare boardSquare2 = board2._0013(x3, position4.z);
					Debug.Log("Traveling to: " + boardSquare2);
					component3.SelectMovementSquareForMovement(boardSquare2);
				}
				else
				{
					turnsToDelayRemaining--;
					Debug.Log("Delayed - Turns remaining:  " + turnsToDelayRemaining);
				}
				yield break;
			}
		}
	}

	public override void OnExit(NPCBrain thisBrain, StateID nextState)
	{
		turnsToDelayRemaining = -1;
		m_PatrolPath.m_AlternateDestination = null;
		base.OnExit(thisBrain, nextState);
	}
}
