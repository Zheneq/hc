using System;
using System.Collections;
using UnityEngine;

public class PatrolState : FSMState
{
	private int turnsToDelayRemaining = -1;

	[Tooltip("A PatrolPath is a collection of waypoints that the NPC will follow")]
	public PatrolPath m_PatrolPath;

	private void Start()
	{
		this.m_PatrolPath.Initialze();
		this.stateID = StateID.Patrol;
	}

	public override void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		base.OnGameEvent(eventType, args);
	}

	public override void OnEnter(NPCBrain thisBrain, StateID previousState)
	{
		base.OnEnter(thisBrain, previousState);
		this.m_PatrolPath.WaypointsVisitedThisCycle = 0;
		this.m_PatrolPath.PatrolCyclesCompleted = 0;
		if (this.m_PatrolPath.m_currentWayPoint == null)
		{
			this.m_PatrolPath.m_currentWayPoint = this.m_PatrolPath.GetInitalWaypoint();
			if (this.m_PatrolPath.m_currentWayPoint == null)
			{
				Log.Error("Could not find a waypoint to travel to. Did you forget to add in waypoints to a patrol path for NPC " + base.MyBrain.name, new object[0]);
			}
			GameEventManager.PatrolPointArgs args = new GameEventManager.PatrolPointArgs(GameEventManager.PatrolPointArgs.WhatHappenedType.MovingToNextPoint, base.MyActorData, this.m_PatrolPath.m_currentWayPoint, this.m_PatrolPath.mWayPoints.IndexOf(this.m_PatrolPath.m_currentWayPoint), this.m_PatrolPath, this.m_PatrolPath.m_AlternateDestination == null);
			GameEventManager.Get().FireEvent(GameEventManager.EventType.PatrolPointEvent, args);
		}
	}

	public override IEnumerator OnTurn(NPCBrain thisBrain)
	{
		NPCBrain_StateMachine npcbrain_StateMachine = thisBrain as NPCBrain_StateMachine;
		AbilityData component = thisBrain.GetComponent<AbilityData>();
		ActorData component2 = npcbrain_StateMachine.GetComponent<ActorData>();
		ActorTurnSM component3 = npcbrain_StateMachine.GetComponent<ActorTurnSM>();
		BotController component4 = npcbrain_StateMachine.GetComponent<BotController>();
		if (component)
		{
			if (component2 && component3 && component4)
			{
				if (this.m_PatrolPath == null)
				{
					yield break;
				}
				WayPoint wayPoint = this.m_PatrolPath.m_currentWayPoint;
				BoardSquare boardSquare = Board.Get().GetBoardSquareSafe(wayPoint.transform.position.x, wayPoint.transform.position.z);
				BoardSquare currentBoardSquare = component2.GetCurrentBoardSquare();
				int num = 0xA;
				float num2 = boardSquare.HorizontalDistanceInSquaresTo(base.MyActorData.GetCurrentBoardSquare());
				float remainingHorizontalMovement = base.MyActorData.RemainingHorizontalMovement;
				if (!wayPoint.MustArriveAtWayPointToContinue && currentBoardSquare != boardSquare)
				{
					if (num2 < remainingHorizontalMovement)
					{
						if (this.m_PatrolPath.m_AlternateDestination == null)
						{
							for (;;)
							{
								if (!(boardSquare.OccupantActor != null))
								{
									if (!(boardSquare.OccupantActor == base.MyActorData))
									{
										break;
									}
								}
								if (--num <= 0)
								{
									break;
								}
								boardSquare = Board.Get().symbol_0018(boardSquare, boardSquare);
								this.m_PatrolPath.m_AlternateDestination = boardSquare;
							}
						}
					}
				}
				if (!(currentBoardSquare == boardSquare))
				{
					if (!(currentBoardSquare == this.m_PatrolPath.m_AlternateDestination))
					{
						component3.SelectMovementSquareForMovement(boardSquare);
						yield break;
					}
				}
				if (this.turnsToDelayRemaining == -1)
				{
					Debug.Log("Arrived at point: " + boardSquare);
					GameEventManager.PatrolPointArgs args = new GameEventManager.PatrolPointArgs(GameEventManager.PatrolPointArgs.WhatHappenedType.PointReached, base.MyActorData, wayPoint, this.m_PatrolPath.mWayPoints.IndexOf(wayPoint), this.m_PatrolPath, this.m_PatrolPath.m_AlternateDestination == null);
					GameEventManager.Get().FireEvent(GameEventManager.EventType.PatrolPointEvent, args);
					this.turnsToDelayRemaining = wayPoint.TurnsToDelay;
				}
				if (this.turnsToDelayRemaining <= 0)
				{
					this.turnsToDelayRemaining = -1;
					wayPoint = (this.m_PatrolPath.m_currentWayPoint = this.m_PatrolPath.IncremementWayPoint(delegate(PatrolPath.IncrementWaypointResult x)
					{
						if (x == PatrolPath.IncrementWaypointResult.Incremented)
						{
							this.m_PatrolPath.WaypointsVisitedThisCycle++;
						}
						else if (x == PatrolPath.IncrementWaypointResult.CycleCompleted)
						{
							this.m_PatrolPath.PatrolCyclesCompleted++;
							this.m_PatrolPath.WaypointsVisitedThisCycle = 0;
						}
					}));
					GameEventManager.PatrolPointArgs args2 = new GameEventManager.PatrolPointArgs(GameEventManager.PatrolPointArgs.WhatHappenedType.MovingToNextPoint, base.MyActorData, wayPoint, this.m_PatrolPath.mWayPoints.IndexOf(wayPoint), this.m_PatrolPath, this.m_PatrolPath.m_AlternateDestination == null);
					GameEventManager.Get().FireEvent(GameEventManager.EventType.PatrolPointEvent, args2);
					this.m_PatrolPath.m_AlternateDestination = null;
					BoardSquare boardSquare2 = Board.Get().symbol_0013(wayPoint.transform.position.x, wayPoint.transform.position.z);
					Debug.Log("Traveling to: " + boardSquare2);
					component3.SelectMovementSquareForMovement(boardSquare2);
				}
				else
				{
					this.turnsToDelayRemaining--;
					Debug.Log("Delayed - Turns remaining:  " + this.turnsToDelayRemaining);
				}
				yield break;
			}
		}
		yield break;
	}

	public override void OnExit(NPCBrain thisBrain, StateID nextState)
	{
		this.turnsToDelayRemaining = -1;
		this.m_PatrolPath.m_AlternateDestination = null;
		base.OnExit(thisBrain, nextState);
	}
}
