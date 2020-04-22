using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderState : FSMState
{
	internal BoardSquare currentDestination;

	internal float totalLengthTravelled;

	internal int totalTurnsTravelling;

	internal int turnsToDelayRemaining = -1;

	[Tooltip("How do you want me to wander! Fill me in")]
	public WanderStateParameters WanderData;

	private void Start()
	{
		stateID = StateID.Wander;
		WanderData.MaxWaitTurns = Mathf.Clamp(WanderData.MaxWaitTurns, 0, WanderData.MaxWaitTurns);
		WanderData.MinWaitTurns = Mathf.Clamp(WanderData.MinWaitTurns, 0, WanderData.MaxWaitTurns);
		WanderData.MaxWanderDistanceInSquares = Mathf.Clamp(WanderData.MaxWanderDistanceInSquares, 0, WanderData.MaxWanderDistanceInSquares);
		WanderData.MinWanderDistanceInSquares = Mathf.Clamp(WanderData.MinWanderDistanceInSquares, 0, WanderData.MaxWanderDistanceInSquares);
		if (WanderData.MinWanderDistanceInSquares != WanderData.MaxWanderDistanceInSquares)
		{
			return;
		}
		while (true)
		{
			Log.Warning("Min and Max wander distance of " + WanderData.MaxWanderDistanceInSquares + " are the same for " + base.MyActorData.name);
			return;
		}
	}

	private bool PickNewWanderPoint(NPCBrain thisBrain)
	{
		BoardSquare currentBoardSquare = base.MyActorData.GetCurrentBoardSquare();
		GameObject gameObject = (!(WanderData.WanderRealativeTo != null)) ? thisBrain.gameObject : WanderData.WanderRealativeTo;
		Vector3 size = new Vector3((float)WanderData.MinWanderDistanceInSquares * Board.Get().squareSize * 2f, 2f, (float)WanderData.MinWanderDistanceInSquares * Board.Get().squareSize * 2f);
		Vector3 size2 = new Vector3((float)WanderData.MaxWanderDistanceInSquares * Board.Get().squareSize * 2f, 2f, (float)WanderData.MaxWanderDistanceInSquares * Board.Get().squareSize * 2f);
		Vector3 position = gameObject.transform.position;
		position.y = 0f;
		Bounds minBounds = new Bounds(position, size);
		Bounds bounds = new Bounds(position, size2);
		List<BoardSquare> list = Board.Get()._000E(bounds, delegate(BoardSquare x)
		{
			int result;
			if (x.OccupantActor == null)
			{
				if (x.IsBaselineHeight())
				{
					result = ((!minBounds.Contains(x.transform.position)) ? 1 : 0);
					goto IL_0057;
				}
			}
			result = 0;
			goto IL_0057;
			IL_0057:
			return (byte)result != 0;
		});
		if (list != null && list.Count > 0)
		{
			GameEventManager.WanderStateArgs wanderStateArgs = new GameEventManager.WanderStateArgs();
			wanderStateArgs.characterActor = base.MyActorData;
			GameEventManager.WanderStateArgs wanderStateArgs2 = wanderStateArgs;
			currentDestination = list[GameplayRandom.Range(0, list.Count)];
			wanderStateArgs2.pathLength = currentBoardSquare.HorizontalDistanceInSquaresTo(currentDestination);
			totalLengthTravelled += wanderStateArgs2.pathLength;
			wanderStateArgs2.totalLengthTravelled = totalLengthTravelled;
			wanderStateArgs2.turnsWandering = totalTurnsTravelling;
			wanderStateArgs2.destinationSquare = currentDestination;
			turnsToDelayRemaining = -1;
			GameEventManager.Get().FireEvent(GameEventManager.EventType.WanderStateEvent, wanderStateArgs2);
			return true;
		}
		return false;
	}

	public override void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		base.OnGameEvent(eventType, args);
	}

	public override IEnumerator OnTurn(NPCBrain thisBrain)
	{
		BoardSquare currentBoardSquare = base.MyActorData.GetCurrentBoardSquare();
		if (currentDestination == null)
		{
			if (!PickNewWanderPoint(thisBrain))
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						Log.Info("Could not find a valid point to wander to. Waiting a turn! Min/Max: " + WanderData.MinWanderDistanceInSquares + ", " + WanderData.MaxWanderDistanceInSquares);
						yield break;
					}
				}
			}
		}
		float num = currentDestination.HorizontalDistanceInSquaresTo(base.MyActorData.GetCurrentBoardSquare());
		float remainingHorizontalMovement = base.MyActorData.RemainingHorizontalMovement;
		if (turnsToDelayRemaining == -1)
		{
			if (!(currentBoardSquare == currentDestination))
			{
				if (!(num < remainingHorizontalMovement) || !(currentDestination.OccupantActor != null))
				{
					goto IL_0228;
				}
			}
			turnsToDelayRemaining = GameplayRandom.Range(WanderData.MinWaitTurns, WanderData.MaxWaitTurns);
			if (turnsToDelayRemaining <= 0 && !PickNewWanderPoint(thisBrain))
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						Log.Info("Could not find a valid point to wander to. Waiting a turn! Min/Max: " + WanderData.MinWanderDistanceInSquares + ", " + WanderData.MaxWanderDistanceInSquares);
						yield break;
					}
				}
			}
		}
		goto IL_0228;
		IL_0228:
		if (turnsToDelayRemaining > 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					turnsToDelayRemaining--;
					if (turnsToDelayRemaining <= 0)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								if (!PickNewWanderPoint(thisBrain))
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											break;
										default:
											Log.Info("Could not find a valid point to wander to. Waiting a turn! Min/Max: " + WanderData.MinWanderDistanceInSquares + ", " + WanderData.MaxWanderDistanceInSquares);
											yield break;
										}
									}
								}
								yield break;
							}
						}
					}
					yield break;
				}
			}
		}
		if (!currentDestination)
		{
			yield break;
		}
		while (true)
		{
			ActorTurnSM component = base.MyFSMBrain.GetComponent<ActorTurnSM>();
			totalTurnsTravelling++;
			component.SelectMovementSquareForMovement(currentDestination);
			yield break;
		}
	}

	public override void OnEnter(NPCBrain thisBrain, StateID previousState)
	{
		currentDestination = null;
		totalLengthTravelled = 0f;
		totalTurnsTravelling = 0;
		turnsToDelayRemaining = 0;
		turnsToDelayRemaining = -1;
		base.OnEnter(thisBrain, previousState);
	}

	public override void OnExit(NPCBrain thisBrain, StateID nextState)
	{
		base.OnExit(thisBrain, nextState);
	}
}
