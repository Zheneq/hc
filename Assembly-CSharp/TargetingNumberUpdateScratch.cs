public class TargetingNumberUpdateScratch
{
	public int m_damage = -1;

	public int m_healing = -1;

	public int m_absorb = -1;

	public int m_energy = -1;

	public void ResetForCalc()
	{
		m_damage = -1;
		m_healing = -1;
		m_absorb = -1;
		m_energy = -1;
	}

	public bool HasOverride(AbilityTooltipSymbol symbol)
	{
		bool result = false;
		switch (symbol)
		{
		case AbilityTooltipSymbol.Damage:
			result = (m_damage >= 0);
			break;
		case AbilityTooltipSymbol.Healing:
			result = (m_healing >= 0);
			break;
		case AbilityTooltipSymbol.Absorb:
			result = (m_absorb >= 0);
			break;
		case AbilityTooltipSymbol.Energy:
			result = (m_energy >= 0);
			break;
		}
		return result;
	}

	public int GetOverrideValue(AbilityTooltipSymbol symbol)
	{
		int result = 0;
		switch (symbol)
		{
		case AbilityTooltipSymbol.Damage:
			result = m_damage;
			break;
		case AbilityTooltipSymbol.Healing:
			result = m_healing;
			break;
		case AbilityTooltipSymbol.Absorb:
			result = m_absorb;
			break;
		case AbilityTooltipSymbol.Energy:
			result = m_energy;
			break;
		}
		return result;
	}
}
