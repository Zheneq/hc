using System;
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

	public unsafe static void ReportAbsorb(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject subject, int amount)
	{
		if (amount > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityTooltipHelper.ReportAbsorb(List<AbilityTooltipNumber>*, AbilityTooltipSubject, int)).MethodHandle;
			}
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Absorb, subject, amount));
		}
	}

	public static void ReportHealing(ref List<AbilityTooltipNumber> number, AbilityTooltipSubject subject, int amount)
	{
		if (amount > 0)
		{
			number.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, subject, amount));
		}
	}

	public unsafe static void ReportEnergy(ref List<AbilityTooltipNumber> number, AbilityTooltipSubject subject, int amount)
	{
		if (amount != 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityTooltipHelper.ReportEnergy(List<AbilityTooltipNumber>*, AbilityTooltipSubject, int)).MethodHandle;
			}
			number.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Energy, subject, amount));
		}
	}
}
