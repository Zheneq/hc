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
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Anvil Slam";
		}
		NanoSmithBoltInfoComponent component = GetComponent<NanoSmithBoltInfoComponent>();
		if ((bool)component)
		{
			m_boltInfo = component.m_boltInfo.GetShallowCopy();
			if (component.m_anvilSlamRangeOverride > 0f)
			{
				m_boltInfo.range = component.m_anvilSlamRangeOverride;
			}
		}
		else
		{
			Debug.LogError("No bolt info component found for NanoSmith ability");
			m_boltInfo = new NanoSmithBoltInfo();
		}
		ResetTooltipAndTargetingNumbers();
		base.Targeter = new AbilityUtil_Targeter_AnvilSlam(this, m_dashWidth, m_dashMaxDistance, m_boltCount, m_boltAngleRelativeToAim, m_boltAngleOffset, m_boltInfo);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_dashDamageAmount);
		m_dashEffectOnHit.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		if (m_boltCount > 0)
		{
			if (m_boltInfo != null)
			{
				m_boltInfo.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Secondary);
			}
		}
		return numbers;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}
}
