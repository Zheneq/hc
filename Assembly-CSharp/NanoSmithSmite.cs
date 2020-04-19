using System;
using System.Collections.Generic;
using UnityEngine;

public class NanoSmithSmite : Ability
{
	public float m_coneWidthAngle = 270f;

	public float m_coneLength = 1.5f;

	public float m_coneBackwardOffset;

	public int m_coneDamageAmount = 0xA;

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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Smite";
		}
		NanoSmithBoltInfoComponent component = base.GetComponent<NanoSmithBoltInfoComponent>();
		if (component)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithSmite.Start()).MethodHandle;
			}
			this.m_boltInfo = component.m_boltInfo.GetShallowCopy();
			if (component.m_smiteRangeOverride > 0f)
			{
				this.m_boltInfo.range = component.m_smiteRangeOverride;
			}
		}
		else
		{
			Debug.LogError("No bolt info component found for NanoSmith ability");
			this.m_boltInfo = new NanoSmithBoltInfo();
		}
		base.ResetTooltipAndTargetingNumbers();
		base.Targeter = new AbilityUtil_Targeter_Smite(this, this.m_coneWidthAngle, this.m_coneLength, this.m_coneBackwardOffset, this.m_conePenetrateLineOfSight, this.m_boltInfo, this.m_boltAngle, this.m_boltCount);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_coneDamageAmount);
		if (this.m_boltCount > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithSmite.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			if (this.m_boltInfo != null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_boltInfo.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Secondary, AbilityTooltipSubject.Ally);
			}
		}
		return result;
	}
}
