using System.Collections.Generic;
using UnityEngine;

// Echo Boost
public class Card_ExtendBuffDuration_Ability : Ability
{
	[Header("-- Status to extend duration --")]
	public int m_extendAmount = 1;
	public List<StatusType> m_buffsToExtend;
	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Buff Enhancer";
		}
		Setup();
	}

	private void Setup()
	{
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			AbilityAreaShape.SingleSquare,
			false,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false);
		Targeter.ShowArcToShape = false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "ExtendAmount", string.Empty, m_extendAmount);
	}
}
