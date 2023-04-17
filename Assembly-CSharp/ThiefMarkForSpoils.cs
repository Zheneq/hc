using System.Collections.Generic;
using UnityEngine;

public class ThiefMarkForSpoils : Ability
{
	[Header("-- Spoil Spawn Info")]
	public SpoilsSpawnData m_spoilSpawnData;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_persistentSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Mark For Spoils";
		}
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
