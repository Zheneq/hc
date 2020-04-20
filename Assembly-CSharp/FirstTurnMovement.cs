using System;
using System.Collections.Generic;
using UnityEngine;

public class FirstTurnMovement : MonoBehaviour
{
	private static FirstTurnMovement s_instance;

	public BoardRegion m_regionForTeamA;

	public BoardRegion m_regionForTeamB;

	public bool m_canWaypointOnFirstTurn;

	private void Awake()
	{
		if (FirstTurnMovement.s_instance != null)
		{
			Debug.LogError("FirstTurnMovement is supposed to be a singleton class, but an instance already existed when it awoke.  Make sure there are not two instances of FirstTurnMovement in the scene.");
		}
		FirstTurnMovement.s_instance = this;
	}

	private void Start()
	{
		this.m_regionForTeamA.Initialize();
		this.m_regionForTeamB.Initialize();
	}

	public static FirstTurnMovement Get()
	{
		return FirstTurnMovement.s_instance;
	}

	public static bool CanActorMoveToSquare(ActorData actor, BoardSquare square)
	{
		if (!(GameFlowData.Get() == null))
		{
			if (GameFlowData.Get().CurrentTurn > 1)
			{
			}
			else
			{
				if (actor == null)
				{
					return false;
				}
				if (square == null)
				{
					return false;
				}
				FirstTurnMovement firstTurnMovement = FirstTurnMovement.Get();
				if (firstTurnMovement == null)
				{
					return true;
				}
				if (actor.GetTeam() == Team.TeamA)
				{
					if (firstTurnMovement.m_regionForTeamA != null)
					{
						if (firstTurnMovement.m_regionForTeamA.HasNonZeroArea())
						{
							return firstTurnMovement.m_regionForTeamA.Contains(square.x, square.y);
						}
					}
					return true;
				}
				if (actor.GetTeam() == Team.TeamB)
				{
					if (firstTurnMovement.m_regionForTeamB != null)
					{
						if (firstTurnMovement.m_regionForTeamB.HasNonZeroArea())
						{
							return firstTurnMovement.m_regionForTeamB.Contains(square.x, square.y);
						}
					}
					return true;
				}
				return true;
			}
		}
		return true;
	}

	public static bool ForceShowSprintRange(ActorData actor)
	{
		if (!(GameFlowData.Get() == null))
		{
			if (GameFlowData.Get().CurrentTurn > 1)
			{
			}
			else
			{
				FirstTurnMovement firstTurnMovement = FirstTurnMovement.Get();
				if (firstTurnMovement == null)
				{
					return false;
				}
				if (actor.GetTeam() == Team.TeamA)
				{
					if (firstTurnMovement.m_regionForTeamA != null)
					{
						if (firstTurnMovement.m_regionForTeamA.HasNonZeroArea())
						{
							return true;
						}
					}
					return false;
				}
				if (actor.GetTeam() == Team.TeamB)
				{
					if (firstTurnMovement.m_regionForTeamB != null)
					{
						if (firstTurnMovement.m_regionForTeamB.HasNonZeroArea())
						{
							return true;
						}
					}
					return false;
				}
				return false;
			}
		}
		return false;
	}

	public static bool CanWaypoint()
	{
		if (FirstTurnMovement.s_instance != null)
		{
			if (FirstTurnMovement.s_instance.GetRestrictedMovementState() == FirstTurnMovement.RestrictedMovementState.Active)
			{
				return FirstTurnMovement.s_instance.m_canWaypointOnFirstTurn;
			}
		}
		return true;
	}

	public void OnTurnTick()
	{
		if (!(GameFlowData.Get() == null))
		{
			if (GameFlowData.Get().CurrentTurn != 2)
			{
				return;
			}
		}
		List<ActorData> actors = GameFlowData.Get().GetActors();
		foreach (ActorData actorData in actors)
		{
			actorData.GetActorMovement().UpdateSquaresCanMoveTo();
		}
	}

	public FirstTurnMovement.RestrictedMovementState GetRestrictedMovementState()
	{
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().CurrentTurn > 1)
			{
				return FirstTurnMovement.RestrictedMovementState.Inactive;
			}
		}
		if (GameFlowData.Get() != null)
		{
			if (GameFlowData.Get().CurrentTurn == 1)
			{
				return FirstTurnMovement.RestrictedMovementState.Active;
			}
		}
		return FirstTurnMovement.RestrictedMovementState.Invalid;
	}

	public enum RestrictedMovementState
	{
		Invalid = -1,
		Inactive,
		Active
	}
}
