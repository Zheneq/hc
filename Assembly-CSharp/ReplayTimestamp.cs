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
		switch (result.phase)
		{
		case AbilityPriority.INVALID:
			result.phase = AbilityPriority.Prep_Defense;
			break;
		case AbilityPriority.Prep_Defense:
		case AbilityPriority.Prep_Offense:
			result.phase = AbilityPriority.Evasion;
			break;
		case AbilityPriority.Evasion:
			result.phase = AbilityPriority.Combat_Damage;
			break;
		case AbilityPriority.Combat_Damage:
		case AbilityPriority.DEPRICATED_Combat_Charge:
		case AbilityPriority.Combat_Knockback:
			result.phase = AbilityPriority.Combat_Final;
			break;
		case AbilityPriority.Combat_Final:
			result.turn++;
			result.phase = AbilityPriority.INVALID;
			break;
		}
		return result;
	}

	public override string ToString()
	{
		return $"{turn}/{phase}";
	}

	public override bool Equals(object obj)
	{
		int result;
		if (obj is ReplayTimestamp)
		{
			result = ((this == (ReplayTimestamp)obj) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public override int GetHashCode()
	{
		return turn.GetHashCode() * phase.GetHashCode();
	}

	public static bool operator ==(ReplayTimestamp lhs, ReplayTimestamp rhs)
	{
		return lhs.turn == rhs.turn && lhs.phase == rhs.phase;
	}

	public static bool operator !=(ReplayTimestamp lhs, ReplayTimestamp rhs)
	{
		int result;
		if (lhs.turn == rhs.turn)
		{
			result = ((lhs.phase != rhs.phase) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
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
		int result;
		if (!(lhs < rhs))
		{
			result = ((lhs == rhs) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
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
