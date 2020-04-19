using System;

public struct ReplayTimestamp
{
	public int turn;

	public AbilityPriority phase;

	public static ReplayTimestamp Current()
	{
		ReplayTimestamp result = default(ReplayTimestamp);
		result.turn = GameFlowData.Get().CurrentTurn;
		if (GameFlowData.Get().gameState == GameState.BothTeams_Decision)
		{
			result.phase = AbilityPriority.INVALID;
		}
		else
		{
			result.phase = ServerClientUtils.GetCurrentAbilityPhase();
		}
		return result;
	}

	public ReplayTimestamp Increment()
	{
		ReplayTimestamp result = this;
		AbilityPriority abilityPriority = result.phase;
		switch (abilityPriority + 1)
		{
		case AbilityPriority.Prep_Defense:
			result.phase = AbilityPriority.Prep_Defense;
			break;
		case AbilityPriority.Prep_Offense:
		case AbilityPriority.Evasion:
			result.phase = AbilityPriority.Evasion;
			break;
		case AbilityPriority.Combat_Damage:
			result.phase = AbilityPriority.Combat_Damage;
			break;
		case AbilityPriority.DEPRICATED_Combat_Charge:
		case AbilityPriority.Combat_Knockback:
		case AbilityPriority.Combat_Final:
			result.phase = AbilityPriority.Combat_Final;
			break;
		case AbilityPriority.NumAbilityPriorities:
			result.turn++;
			result.phase = AbilityPriority.INVALID;
			break;
		}
		return result;
	}

	public override string ToString()
	{
		return string.Format("{0}/{1}", this.turn, this.phase);
	}

	public override bool Equals(object obj)
	{
		bool result;
		if (obj is ReplayTimestamp)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ReplayTimestamp.Equals(object)).MethodHandle;
			}
			result = (this == (ReplayTimestamp)obj);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public override int GetHashCode()
	{
		return this.turn.GetHashCode() * this.phase.GetHashCode();
	}

	public static bool operator ==(ReplayTimestamp lhs, ReplayTimestamp rhs)
	{
		return lhs.turn == rhs.turn && lhs.phase == rhs.phase;
	}

	public static bool operator !=(ReplayTimestamp lhs, ReplayTimestamp rhs)
	{
		bool result;
		if (lhs.turn == rhs.turn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ReplayTimestamp != ReplayTimestamp).MethodHandle;
			}
			result = (lhs.phase != rhs.phase);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public static bool operator <(ReplayTimestamp lhs, ReplayTimestamp rhs)
	{
		if (lhs.turn != rhs.turn)
		{
			return lhs.turn < rhs.turn;
		}
		return lhs.phase < rhs.phase;
	}

	public static bool operator <=(ReplayTimestamp lhs, ReplayTimestamp rhs)
	{
		bool result;
		if (!(lhs < rhs))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ReplayTimestamp <= ReplayTimestamp).MethodHandle;
			}
			result = (lhs == rhs);
		}
		else
		{
			result = true;
		}
		return result;
	}

	public static bool operator >(ReplayTimestamp lhs, ReplayTimestamp rhs)
	{
		return rhs < lhs;
	}

	public static bool operator >=(ReplayTimestamp lhs, ReplayTimestamp rhs)
	{
		return rhs <= lhs;
	}
}
