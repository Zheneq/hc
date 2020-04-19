using System;
using System.Collections.Generic;
using UnityEngine;

public class NanoSmithAnvilSlam : Ability
{
	public int m_dashDamageAmount = 5;

	public float m_dashMaxDistance = 5f;

	public float m_dashWidth = 1f;

	public StandardEffectInfo m_dashEffectOnHit;

	public float m_dashRecoveryTime = 0.5f;

	[Header("-- additional bolt info now specified in NanoSmithBoltInfoComponent")]
	public int m_boltCount = 8;

	public float m_boltAngleOffset;

	public bool m_boltAngleRelativeToAim;

	private NanoSmithBoltInfo m_boltInfo;

	[Header("-- Sequences -----------------------------------------------")]
	public GameObject m_slamSequencePrefab;

	public GameObject m_boltSequencePrefab;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithAnvilSlam.Start()).MethodHandle;
			}
			this.m_abilityName = "Anvil Slam";
		}
		NanoSmithBoltInfoComponent component = base.GetComponent<NanoSmithBoltInfoComponent>();
		if (component)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_boltInfo = component.m_boltInfo.GetShallowCopy();
			if (component.m_anvilSlamRangeOverride > 0f)
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
				this.m_boltInfo.range = component.m_anvilSlamRangeOverride;
			}
		}
		else
		{
			Debug.LogError("No bolt info component found for NanoSmith ability");
			this.m_boltInfo = new NanoSmithBoltInfo();
		}
		base.ResetTooltipAndTargetingNumbers();
		base.Targeter = new AbilityUtil_Targeter_AnvilSlam(this, this.m_dashWidth, this.m_dashMaxDistance, this.m_boltCount, this.m_boltAngleRelativeToAim, this.m_boltAngleOffset, this.m_boltInfo);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.m_dashDamageAmount);
		this.m_dashEffectOnHit.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		if (this.m_boltCount > 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(NanoSmithAnvilSlam.CalculateAbilityTooltipNumbers()).MethodHandle;
			}
			if (this.m_boltInfo != null)
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
				this.m_boltInfo.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Secondary, AbilityTooltipSubject.Ally);
			}
		}
		return result;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}
}
