using System.Collections.Generic;
using UnityEngine;

public class SparkBasicAttack : Ability
{
	[Space(10f)]
	public int m_laserDamageAmount = 5;
	public int m_healOnCasterOnTick;
	public bool m_healCasterOnIniialAttach;
	public LaserTargetingInfo m_laserInfo;
	public StandardEffectInfo m_laserHitEffect;
	[Header("-- Energy on Caster Per Turn")]
	public int m_energyOnCasterPerTurn = 5;
	[Separator("Tether", true)]
	public float m_tetherDistance = 5f;
	[Header("-- Tether Duration")]
	public int m_tetherDuration;
	public int m_additionalEnergizedDamage = 2;
	[Header("-- Extra Energy Gain On Caster --")]
	public int m_maxBonusEnergyFromGrowingGain;
	public int m_bonusEnergyGrowthRate;
	[Header("-- Animation on Pulse")]
	public int m_pulseAnimIndex;
	public int m_energizedPulseAnimIndex;
	[Header("-- Sequences")]
	public GameObject m_castSequence;
	public GameObject m_pulseSequence;
	public GameObject m_energizedPulseSequence;
	public GameObject m_beamSequence;
	public GameObject m_targetPersistentSequence;

	private AbilityMod_SparkBasicAttack m_abilityMod;
	private SparkEnergized m_energizedAbility;
	private LaserTargetingInfo m_cachedLaserInfo;
	private StandardActorEffectData m_cachedEffectData;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Spark Damage Beam";
		}
		Setup();
	}

	public void Setup()
	{
		SetCachedFields();
		if (m_energizedAbility == null)
		{
			AbilityData abilityData = GetComponent<AbilityData>();
			if (abilityData != null)
			{
				m_energizedAbility = abilityData.GetAbilityOfType(typeof(SparkEnergized)) as SparkEnergized;
			}
		}
		if (Targeter != null)
		{
			Targeter.ResetTargeter(true);
		}
		AbilityUtil_Targeter_Laser targeter = new AbilityUtil_Targeter_Laser(this, GetLaserInfo());
		bool affectsCaster = m_healCasterOnIniialAttach && GetHealOnCasterPerTurn() > 0;
		targeter.SetAffectedGroups(true, false, affectsCaster);
		targeter.m_affectCasterDelegate = ((ActorData caster, List<ActorData> actorsSoFar) => actorsSoFar.Count > 0);
		Targeter = targeter;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserInfo().range;
	}

	public int GetInitialDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_initialDamageMod.GetModifiedValue(m_laserDamageAmount)
			: m_laserDamageAmount;
	}

	public int GetPerTurnDamage()
	{
		return GetEnemyTetherEffectData().m_damagePerTurn;
	}

	public int GetAdditionalDamageOnRadiated()
	{
		return m_abilityMod != null
			? m_abilityMod.m_additionalDamageOnRadiatedMod.GetModifiedValue(m_additionalEnergizedDamage)
			: m_additionalEnergizedDamage;
	}

	public int GetEnergyOnCasterPerTurn()
	{
		int energy = m_abilityMod != null
			? m_abilityMod.m_energyOnCasterPerTurnMod.GetModifiedValue(m_energyOnCasterPerTurn)
			: m_energyOnCasterPerTurn;
		return m_energizedAbility != null
			? m_energizedAbility.CalcEnergyOnSelfPerTurn(energy)
			: energy;
	}

	public int GetHealOnCasterPerTurn()
	{
		int heal = m_abilityMod != null
			? m_abilityMod.m_healOnCasterOnTickMod.GetModifiedValue(m_healOnCasterOnTick)
			: m_healOnCasterOnTick;
		return m_energizedAbility != null
			? m_energizedAbility.CalcHealOnSelfPerTurn(heal)
			: heal;
	}

	public float GetTetherDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_tetherDistanceMod.GetModifiedValue(m_tetherDistance)
			: m_tetherDistance;
	}

	public int GetTetherDuration()
	{
		return m_abilityMod != null
			? m_abilityMod.m_tetherDurationMod.GetModifiedValue(m_tetherDuration)
			: m_tetherDuration;
	}

	public bool UseBonusDamageOverTime()
	{
		return m_abilityMod != null && m_abilityMod.m_useBonusDamageOverTime;
	}

	public int GetBonusDamageGrowRate()
	{
		return m_abilityMod != null
			? m_abilityMod.m_bonusDamageIncreaseRateMod.GetModifiedValue(0)
			: 0;
	}

	public int GetMaxBonusDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxBonusDamageAmountMod.GetModifiedValue(0)
			: 0;
	}

	public int GetBonusDamageFromTetherAge(int age)
	{
		int num = 0;
		if (UseBonusDamageOverTime())
		{
			int maxBonusDamage = GetMaxBonusDamage();
			int bonusDamageGrowRate = GetBonusDamageGrowRate();
			if (bonusDamageGrowRate > 0)
			{
				num = age * bonusDamageGrowRate;
			}
			if (maxBonusDamage > 0)
			{
				num = Mathf.Min(num, maxBonusDamage);
			}
		}
		return num;
	}

	public int GetEnergyGainCyclePeriod()
	{
		int num = m_abilityMod != null
			? m_abilityMod.m_energyGainCyclePeriod.GetModifiedValue(0)
			: 1;
		return Mathf.Max(1, num);
	}

	public int GetEnergyGainPerCycle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_energyGainPerCycle.GetModifiedValue(0) 
			: 0;
	}

	public int GetMaxBonusEnergyFromGrowingGain()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxBonusEnergyFromGrowingGainMod.GetModifiedValue(m_maxBonusEnergyFromGrowingGain)
			: m_maxBonusEnergyFromGrowingGain;
	}

	public int GetBonusEnergyGrowthRate()
	{
		return m_abilityMod != null
			? m_abilityMod.m_bonusEnergyGrowthRateMod.GetModifiedValue(m_bonusEnergyGrowthRate)
			: m_bonusEnergyGrowthRate;
	}

	private void SetCachedFields()
	{
		m_cachedLaserInfo = m_laserInfo.GetModifiedCopy(m_abilityMod?.m_laserInfoMod);
		StandardEffectInfo effectInfo = m_abilityMod != null
			? m_abilityMod.m_tetherBaseEffectOverride.GetModifiedValue(m_laserHitEffect)
			: m_laserHitEffect.GetShallowCopy();
		m_cachedEffectData = effectInfo.m_effectData;
		m_cachedEffectData.m_sequencePrefabs = new GameObject[2]
		{
			m_targetPersistentSequence,
			m_beamSequence
		};
	}

	public StandardActorEffectData GetEnemyTetherEffectData()
	{
		return m_cachedEffectData ?? m_laserHitEffect.m_effectData;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		return m_cachedLaserInfo ?? m_laserInfo;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Damage_FirstTurn", "damage on initial attach", m_laserDamageAmount);
		AddTokenInt(tokens, "Damage_PerTurnAfterFirst", "damage on subsequent turns", m_laserHitEffect.m_effectData.m_damagePerTurn);
		AddTokenInt(tokens, "Damage_AdditionalOnRadiated", "additional damage on radiated", m_additionalEnergizedDamage);
		AddTokenInt(tokens, "Heal_OnCasterPerTurn", "heal on caster per turn", m_healOnCasterOnTick);
		AddTokenInt(tokens, "EnergyOnCasterPerTurn", "", m_energyOnCasterPerTurn);
		AddTokenInt(tokens, "MaxBonusEnergyFromGrowingGain", "", m_maxBonusEnergyFromGrowingGain);
		AddTokenInt(tokens, "BonusEnergyGrowthRate", "", m_bonusEnergyGrowthRate);
		AddTokenInt(tokens, "TetherDuration", "", m_tetherDuration);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		if (GetInitialDamage() > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref number, AbilityTooltipSubject.Primary, GetInitialDamage());
		}
		GetEnemyTetherEffectData().ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Primary);
		if (m_healCasterOnIniialAttach && GetHealOnCasterPerTurn() > 0)
		{
			AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Self, GetHealOnCasterPerTurn());
		}
		return number;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy) > 0)
		{
			return GetEnergyOnCasterPerTurn();
		}
		return 0;
	}

	public override List<int> Debug_GetExpectedNumbersInTooltip()
	{
		List<int> list = base.Debug_GetExpectedNumbersInTooltip();
		list.Add(m_laserHitEffect.m_effectData.m_damagePerTurn);
		return list;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SparkBasicAttack))
		{
			m_abilityMod = (abilityMod as AbilityMod_SparkBasicAttack);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
}
