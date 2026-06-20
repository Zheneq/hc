using System.Collections.Generic;
using UnityEngine;

public class CardEnergyLeechAOE : Ability
{
	public bool m_penetrateLineOfSight;

	public int m_energyLeeched = 10;

	public AbilityAreaShape m_shape = AbilityAreaShape.Three_x_Three_NoCorners;

	[Header("-- For sequences --")]
	public GameObject m_aoeSequence;

	public GameObject m_individualLeechSequence;

	private void Start()
	{
		base.Targeter = new AbilityUtil_Targeter_Shape(this, m_shape, m_penetrateLineOfSight);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "EnergyLeeched", string.Empty, m_energyLeeched);
	}
}
