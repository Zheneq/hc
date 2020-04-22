using System.Collections.Generic;
using UnityEngine;

public class FirstTurnMovement : MonoBehaviour
{
	public enum RestrictedMovementState
	{
		Invalid = -1,
		Inactive,
		Active
	}

	private static FirstTurnMovement s_instance;

	public BoardRegion m_regionForTeamA;

	public BoardRegion m_regionForTeamB;

	public bool m_canWaypointOnFirstTurn;

	private void Awake()
	{
		if (s_instance != null)
		{
			Debug.LogError("FirstTurnMovement is supposed to be a singleton class, but an instance already existed when it awoke.  Make sure there are not two instances of FirstTurnMovement in the scene.");
		}
		s_instance = this;
	}

	private void Start()
	{
		m_regionForTeamA.Initialize();
		m_regionForTeamB.Initialize();
	}

	public static FirstTurnMovement Get()
	{
		return s_instance;
	}

	public static bool CanActorMoveToSquare(ActorData actor, BoardSquare square)
	{
		if (!(GameFlowData.Get() == null))
		{
			if (GameFlowData.Get().CurrentTurn <= 1)
			{
				if (actor == null)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				if (square == null)
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
				FirstTurnMovement firstTurnMovement = Get();
				if (firstTurnMovement == null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return true;
						}
					}
				}
				if (actor.GetTeam() == Team.TeamA)
				{
					if (firstTurnMovement.m_regionForTeamA != null)
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
						if (firstTurnMovement.m_regionForTeamA.HasNonZeroArea())
						{
							return firstTurnMovement.m_regionForTeamA.Contains(square.x, square.y);
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
					return true;
				}
				return true;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
		}
		return true;
	}

	public static bool ForceShowSprintRange(ActorData actor)
	{
		if (!(GameFlowData.Get() == null))
		{
			if (GameFlowData.Get().CurrentTurn <= 1)
			{
				FirstTurnMovement firstTurnMovement = Get();
				if (firstTurnMovement == null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				if (actor.GetTeam() == Team.TeamA)
				{
					if (firstTurnMovement.m_regionForTeamA != null)
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
						if (firstTurnMovement.m_regionForTeamA.HasNonZeroArea())
						{
							return true;
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
					return false;
				}
				if (actor.GetTeam() == Team.TeamB)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							if (firstTurnMovement.m_regionForTeamB != null)
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
								if (firstTurnMovement.m_regionForTeamB.HasNonZeroArea())
								{
									return true;
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
							return false;
						}
					}
				}
				return false;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
		}
		return false;
	}

	public static bool CanWaypoint()
	{
		if (s_instance != null)
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
			if (s_instance.GetRestrictedMovementState() == RestrictedMovementState.Active)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return s_instance.m_canWaypointOnFirstTurn;
					}
				}
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
		}
		List<ActorData> actors = GameFlowData.Get().GetActors();
		foreach (ActorData item in actors)
		{
			item.GetActorMovement().UpdateSquaresCanMoveTo();
		}
	}

	public RestrictedMovementState GetRestrictedMovementState()
	{
		if (GameFlowData.Get() != null)
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
			if (GameFlowData.Get().CurrentTurn > 1)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return RestrictedMovementState.Inactive;
					}
				}
			}
		}
		if (GameFlowData.Get() != null)
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
			if (GameFlowData.Get().CurrentTurn == 1)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						return RestrictedMovementState.Active;
					}
				}
			}
		}
		return RestrictedMovementState.Invalid;
	}
}
