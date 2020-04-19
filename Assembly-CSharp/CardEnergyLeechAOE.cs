using System;
using System.Collections.Generic;
using UnityEngine;

public class CardEnergyLeechAOE : Ability
{
	public bool m_penetrateLineOfSight;

	public int m_energyLeeched = 0xA;

	public AbilityAreaShape m_shape = AbilityAreaShape.Three_x_Three_NoCorners;

	[Header("-- For sequences --")]
	public GameObject m_aoeSequence;

	public GameObject m_individualLeechSequence;

	private void Start()
	{
		base.Targeter = new AbilityUtil_Targeter_Shape(this, this.m_shape, this.m_penetrateLineOfSight, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Possible);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "EnergyLeeched", string.Empty, this.m_energyLeeched, false);
	}
}
