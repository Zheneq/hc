using System;
using System.Collections.Generic;
using UnityEngine;

public class Card_ExtendBuffDuration_Ability : Ability
{
	[Header("-- Status to extend duration --")]
	public int m_extendAmount = 1;

	public List<StatusType> m_buffsToExtend;

	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(Card_ExtendBuffDuration_Ability.Start()).MethodHandle;
			}
			this.m_abilityName = "Buff Enhancer";
		}
		this.Setup();
	}

	private void Setup()
	{
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
		base.Targeter.ShowArcToShape = false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "ExtendAmount", string.Empty, this.m_extendAmount, false);
	}
}
