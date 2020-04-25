using System;
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
		this.stateID = StateID.Wander;
		this.WanderData.MaxWaitTurns = Mathf.Clamp(this.WanderData.MaxWaitTurns, 0, this.WanderData.MaxWaitTurns);
		this.WanderData.MinWaitTurns = Mathf.Clamp(this.WanderData.MinWaitTurns, 0, this.WanderData.MaxWaitTurns);
		this.WanderData.MaxWanderDistanceInSquares = Mathf.Clamp(this.WanderData.MaxWanderDistanceInSquares, 0, this.WanderData.MaxWanderDistanceInSquares);
		this.WanderData.MinWanderDistanceInSquares = Mathf.Clamp(this.WanderData.MinWanderDistanceInSquares, 0, this.WanderData.MaxWanderDistanceInSquares);
		if (this.WanderData.MinWanderDistanceInSquares == this.WanderData.MaxWanderDistanceInSquares)
		{
			Log.Warning(string.Concat(new object[]
			{
				"Min and Max wander distance of ",
				this.WanderData.MaxWanderDistanceInSquares,
				" are the same for ",
				base.MyActorData.name
			}), new object[0]);
		}
	}

	private bool PickNewWanderPoint(NPCBrain thisBrain)
	{
		BoardSquare currentBoardSquare = base.MyActorData.GetCurrentBoardSquare();
		GameObject gameObject = (!(this.WanderData.WanderRealativeTo != null)) ? thisBrain.gameObject : this.WanderData.WanderRealativeTo;
		Vector3 size = new Vector3((float)this.WanderData.MinWanderDistanceInSquares * Board.Get().squareSize * 2f, 2f, (float)this.WanderData.MinWanderDistanceInSquares * Board.Get().squareSize * 2f);
		Vector3 size2 = new Vector3((float)this.WanderData.MaxWanderDistanceInSquares * Board.Get().squareSize * 2f, 2f, (float)this.WanderData.MaxWanderDistanceInSquares * Board.Get().squareSize * 2f);
		Vector3 position = gameObject.transform.position;
		position.y = 0f;
		Bounds minBounds = new Bounds(position, size);
		Bounds u001D = new Bounds(position, size2);
		List<BoardSquare> list = Board.Get()._000E(u001D, delegate(BoardSquare x)
		{
			if (x.OccupantActor == null)
			{
				if (x.IsBaselineHeight())
				{
					return !minBounds.Contains(x.transform.position);
				}
			}
			return false;
		});
		if (list != null && list.Count > 0)
		{
			GameEventManager.WanderStateArgs wanderStateArgs = new GameEventManager.WanderStateArgs
			{
				characterActor = base.MyActorData
			};
			this.currentDestination = list[GameplayRandom.Range(0, list.Count)];
			wanderStateArgs.pathLength = currentBoardSquare.HorizontalDistanceInSquaresTo(this.currentDestination);
			this.totalLengthTravelled += wanderStateArgs.pathLength;
			wanderStateArgs.totalLengthTravelled = this.totalLengthTravelled;
			wanderStateArgs.turnsWandering = this.totalTurnsTravelling;
			wanderStateArgs.destinationSquare = this.currentDestination;
			this.turnsToDelayRemaining = -1;
			GameEventManager.Get().FireEvent(GameEventManager.EventType.WanderStateEvent, wanderStateArgs);
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
		if (this.currentDestination == null)
		{
			if (!this.PickNewWanderPoint(thisBrain))
			{
				Log.Info(string.Concat(new object[]
				{
					"Could not find a valid point to wander to. Waiting a turn! Min/Max: ",
					this.WanderData.MinWanderDistanceInSquares,
					", ",
					this.WanderData.MaxWanderDistanceInSquares
				}), new object[0]);
				yield break;
			}
		}
		float num = this.currentDestination.HorizontalDistanceInSquaresTo(base.MyActorData.GetCurrentBoardSquare());
		float remainingHorizontalMovement = base.MyActorData.RemainingHorizontalMovement;
		if (this.turnsToDelayRemaining == -1)
		{
			if (!(currentBoardSquare == this.currentDestination))
			{
				if (num >= remainingHorizontalMovement || !(this.currentDestination.OccupantActor != null))
				{
					goto IL_228;
				}
			}
			this.turnsToDelayRemaining = GameplayRandom.Range(this.WanderData.MinWaitTurns, this.WanderData.MaxWaitTurns);
			if (this.turnsToDelayRemaining <= 0 && !this.PickNewWanderPoint(thisBrain))
			{
				Log.Info(string.Concat(new object[]
				{
					"Could not find a valid point to wander to. Waiting a turn! Min/Max: ",
					this.WanderData.MinWanderDistanceInSquares,
					", ",
					this.WanderData.MaxWanderDistanceInSquares
				}), new object[0]);
				yield break;
			}
		}
		IL_228:
		if (this.turnsToDelayRemaining > 0)
		{
			this.turnsToDelayRemaining--;
			if (this.turnsToDelayRemaining <= 0)
			{
				if (!this.PickNewWanderPoint(thisBrain))
				{
					Log.Info(string.Concat(new object[]
					{
						"Could not find a valid point to wander to. Waiting a turn! Min/Max: ",
						this.WanderData.MinWanderDistanceInSquares,
						", ",
						this.WanderData.MaxWanderDistanceInSquares
					}), new object[0]);
					yield break;
				}
			}
		}
		else if (this.currentDestination)
		{
			ActorTurnSM component = base.MyFSMBrain.GetComponent<ActorTurnSM>();
			this.totalTurnsTravelling++;
			component.SelectMovementSquareForMovement(this.currentDestination);
		}
		yield break;
	}

	public override void OnEnter(NPCBrain thisBrain, StateID previousState)
	{
		this.currentDestination = null;
		this.totalLengthTravelled = 0f;
		this.totalTurnsTravelling = 0;
		this.turnsToDelayRemaining = 0;
		this.turnsToDelayRemaining = -1;
		base.OnEnter(thisBrain, previousState);
	}

	public override void OnExit(NPCBrain thisBrain, StateID nextState)
	{
		base.OnExit(thisBrain, nextState);
	}
}
