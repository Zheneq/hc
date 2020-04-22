using System.Collections.Generic;
using UnityEngine;

public class Card_MaxHealthIncrease_Ability : Ability
{
	public float m_maxHealthIncreasePercent = 0.1f;

	[Tooltip("The number of turns to divide the percentage increase, and regen by the divided amount on each of those turns.")]
	public int m_turnsToHealAddedMax = 2;

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "TurnsToHealAddedMax", string.Empty, m_turnsToHealAddedMax);
	}
}
