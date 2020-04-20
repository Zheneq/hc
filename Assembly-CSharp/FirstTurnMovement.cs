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
					RuntimeMethodHandle runtimeMethodHandle = methodof(FirstTurnMovement.CanActorMoveToSquare(ActorData, BoardSquare)).MethodHandle;
				}
			}
			else
			{
				if (actor == null)
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
					return false;
				}
				if (square == null)
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
					return false;
				}
				FirstTurnMovement firstTurnMovement = FirstTurnMovement.Get();
				if (firstTurnMovement == null)
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
					return true;
				}
				if (actor.GetTeam() == Team.TeamA)
				{
					if (firstTurnMovement.m_regionForTeamA != null)
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
						if (firstTurnMovement.m_regionForTeamA.HasNonZeroArea())
						{
							return firstTurnMovement.m_regionForTeamA.Contains(square.x, square.y);
						}
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(FirstTurnMovement.ForceShowSprintRange(ActorData)).MethodHandle;
				}
			}
			else
			{
				FirstTurnMovement firstTurnMovement = FirstTurnMovement.Get();
				if (firstTurnMovement == null)
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
					return false;
				}
				if (actor.GetTeam() == Team.TeamA)
				{
					if (firstTurnMovement.m_regionForTeamA != null)
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
						if (firstTurnMovement.m_regionForTeamA.HasNonZeroArea())
						{
							return true;
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
					return false;
				}
				if (actor.GetTeam() == Team.TeamB)
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
					if (firstTurnMovement.m_regionForTeamB != null)
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
						if (firstTurnMovement.m_regionForTeamB.HasNonZeroArea())
						{
							return true;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FirstTurnMovement.CanWaypoint()).MethodHandle;
			}
			if (FirstTurnMovement.s_instance.GetRestrictedMovementState() == FirstTurnMovement.RestrictedMovementState.Active)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FirstTurnMovement.OnTurnTick()).MethodHandle;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FirstTurnMovement.GetRestrictedMovementState()).MethodHandle;
			}
			if (GameFlowData.Get().CurrentTurn > 1)
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
				return FirstTurnMovement.RestrictedMovementState.Inactive;
			}
		}
		if (GameFlowData.Get() != null)
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
			if (GameFlowData.Get().CurrentTurn == 1)
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
