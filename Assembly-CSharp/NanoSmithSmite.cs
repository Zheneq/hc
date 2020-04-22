using System.Collections.Generic;
using UnityEngine;

public class NanoSmithSmite : Ability
{
	public float m_coneWidthAngle = 270f;

	public float m_coneLength = 1.5f;

	public float m_coneBackwardOffset;

	public int m_coneDamageAmount = 10;

	public int m_coneMaxTargets;

	public bool m_conePenetrateLineOfSight;

	public StandardEffectInfo m_coneEffectOnEnemyHit;

	[Header("-- additional bolt info now specified in NanoSmithBoltInfoComponent")]
	public float m_boltAngle = 45f;

	public int m_boltCount = 3;

	private NanoSmithBoltInfo m_boltInfo;

	[Header("-- Sequences -----------------------------------")]
	public GameObject m_coneSequencePrefab;

	public GameObject m_boltSequencePrefab;

	[TextArea(1, 5)]
	public string m_sequenceNotes;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Smite";
		}
		NanoSmithBoltInfoComponent component = GetComponent<NanoSmithBoltInfoComponent>();
		if ((bool)component)
		{
			m_boltInfo = component.m_boltInfo.GetShallowCopy();
			if (component.m_smiteRangeOverride > 0f)
			{
				m_boltInfo.range = component.m_smiteRangeOverride;
			}
		}
		else
		{
			Debug.LogError("No bolt info component found for NanoSmith ability");
			m_boltInfo = new NanoSmithBoltInfo();
		}
		ResetTooltipAndTargetingNumbers();
		base.Targeter = new AbilityUtil_Targeter_Smite(this, m_coneWidthAngle, m_coneLength, m_coneBackwardOffset, m_conePenetrateLineOfSight, m_boltInfo, m_boltAngle, m_boltCount);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_coneDamageAmount);
		if (m_boltCount > 0)
		{
			if (m_boltInfo != null)
			{
				m_boltInfo.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
			}
		}
		return numbers;
	}
}
