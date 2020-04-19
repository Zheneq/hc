using System;

public class AbilityTooltipNumber
{
	public AbilityTooltipSymbol m_symbol;

	public AbilityTooltipSubject m_subject;

	public int m_value;

	public AbilityTooltipNumber(AbilityTooltipSymbol symbol, AbilityTooltipSubject subject, int value)
	{
		this.m_symbol = symbol;
		this.m_subject = subject;
		this.m_value = value;
	}
}
