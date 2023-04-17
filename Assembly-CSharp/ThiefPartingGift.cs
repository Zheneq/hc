using System.Collections.Generic;
using UnityEngine;

public class ThiefPartingGift : Ability
{
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Parting Gift";
		}
		m_sequencePrefab = m_castSequencePrefab;
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			AbilityAreaShape.SingleSquare,
			false,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false,
			false,
			AbilityUtil_Targeter.AffectsActor.Always);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}
}
