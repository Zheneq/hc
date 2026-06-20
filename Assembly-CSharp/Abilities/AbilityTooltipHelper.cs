using System.Collections.Generic;

public static class AbilityTooltipHelper
{
	public static void ReportDamage(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject subject, int amount)
	{
		if (amount > 0)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, subject, amount));
		}
	}

	public static void ReportAbsorb(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject subject, int amount)
	{
		if (amount <= 0)
		{
			return;
		}
		while (true)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Absorb, subject, amount));
			return;
		}
	}

	public static void ReportHealing(ref List<AbilityTooltipNumber> number, AbilityTooltipSubject subject, int amount)
	{
		if (amount > 0)
		{
			number.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, subject, amount));
		}
	}

	public static void ReportEnergy(ref List<AbilityTooltipNumber> number, AbilityTooltipSubject subject, int amount)
	{
		if (amount == 0)
		{
			return;
		}
		while (true)
		{
			number.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Energy, subject, amount));
			return;
		}
	}
}
