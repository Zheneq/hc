using System;
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
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Spark Damage Beam";
		}
		this.Setup();
	}

	public void Setup()
	{
		this.SetCachedFields();
		if (this.m_energizedAbility == null)
		{
			AbilityData component = base.GetComponent<AbilityData>();
			if (component != null)
			{
				this.m_energizedAbility = (component.GetAbilityOfType(typeof(SparkEnergized)) as SparkEnergized);
			}
		}
		if (base.Targeter != null)
		{
			base.Targeter.ResetTargeter(true);
		}
		AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser = new AbilityUtil_Targeter_Laser(this, this.GetLaserInfo());
		AbilityUtil_Targeter abilityUtil_Targeter = abilityUtil_Targeter_Laser;
		bool affectsEnemies = true;
		bool affectsAllies = false;
		bool affectsCaster;
		if (this.m_healCasterOnIniialAttach)
		{
			affectsCaster = (this.GetHealOnCasterPerTurn() > 0);
		}
		else
		{
			affectsCaster = false;
		}
		abilityUtil_Targeter.SetAffectedGroups(affectsEnemies, affectsAllies, affectsCaster);
		AbilityUtil_Targeter_Laser abilityUtil_Targeter_Laser2 = abilityUtil_Targeter_Laser;
		
		abilityUtil_Targeter_Laser2.m_affectCasterDelegate = ((ActorData caster, List<ActorData> actorsSoFar) => actorsSoFar.Count > 0);
		base.Targeter = abilityUtil_Targeter_Laser;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.GetLaserInfo().range;
	}

	public int GetInitialDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_initialDamageMod.GetModifiedValue(this.m_laserDamageAmount);
		}
		else
		{
			result = this.m_laserDamageAmount;
		}
		return result;
	}

	public int GetPerTurnDamage()
	{
		return this.GetEnemyTetherEffectData().m_damagePerTurn;
	}

	public int GetAdditionalDamageOnRadiated()
	{
		return (!this.m_abilityMod) ? this.m_additionalEnergizedDamage : this.m_abilityMod.m_additionalDamageOnRadiatedMod.GetModifiedValue(this.m_additionalEnergizedDamage);
	}

	public int GetEnergyOnCasterPerTurn()
	{
		int num;
		if (this.m_abilityMod)
		{
			num = this.m_abilityMod.m_energyOnCasterPerTurnMod.GetModifiedValue(this.m_energyOnCasterPerTurn);
		}
		else
		{
			num = this.m_energyOnCasterPerTurn;
		}
		int num2 = num;
		if (this.m_energizedAbility != null)
		{
			num2 = this.m_energizedAbility.CalcEnergyOnSelfPerTurn(num2);
		}
		return num2;
	}

	public int GetHealOnCasterPerTurn()
	{
		int num = (!this.m_abilityMod) ? this.m_healOnCasterOnTick : this.m_abilityMod.m_healOnCasterOnTickMod.GetModifiedValue(this.m_healOnCasterOnTick);
		if (this.m_energizedAbility != null)
		{
			num = this.m_energizedAbility.CalcHealOnSelfPerTurn(num);
		}
		return num;
	}

	public float GetTetherDistance()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_tetherDistanceMod.GetModifiedValue(this.m_tetherDistance);
		}
		else
		{
			result = this.m_tetherDistance;
		}
		return result;
	}

	public int GetTetherDuration()
	{
		int result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_tetherDurationMod.GetModifiedValue(this.m_tetherDuration);
		}
		else
		{
			result = this.m_tetherDuration;
		}
		return result;
	}

	public bool UseBonusDamageOverTime()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_useBonusDamageOverTime;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public int GetBonusDamageGrowRate()
	{
		return (!this.m_abilityMod) ? 0 : this.m_abilityMod.m_bonusDamageIncreaseRateMod.GetModifiedValue(0);
	}

	public int GetMaxBonusDamage()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxBonusDamageAmountMod.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public int GetBonusDamageFromTetherAge(int age)
	{
		int num = 0;
		if (this.UseBonusDamageOverTime())
		{
			int maxBonusDamage = this.GetMaxBonusDamage();
			int bonusDamageGrowRate = this.GetBonusDamageGrowRate();
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
		int num;
		if (this.m_abilityMod)
		{
			num = this.m_abilityMod.m_energyGainCyclePeriod.GetModifiedValue(0);
		}
		else
		{
			num = 1;
		}
		int b = num;
		return Mathf.Max(1, b);
	}

	public int GetEnergyGainPerCycle()
	{
		return (!this.m_abilityMod) ? 0 : this.m_abilityMod.m_energyGainPerCycle.GetModifiedValue(0);
	}

	public int GetMaxBonusEnergyFromGrowingGain()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxBonusEnergyFromGrowingGainMod.GetModifiedValue(this.m_maxBonusEnergyFromGrowingGain);
		}
		else
		{
			result = this.m_maxBonusEnergyFromGrowingGain;
		}
		return result;
	}

	public int GetBonusEnergyGrowthRate()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_bonusEnergyGrowthRateMod.GetModifiedValue(this.m_bonusEnergyGrowthRate);
		}
		else
		{
			result = this.m_bonusEnergyGrowthRate;
		}
		return result;
	}

	private void SetCachedFields()
	{
		LaserTargetingInfo laserInfo = this.m_laserInfo;
		AbilityModPropertyLaserInfo mod;
		if (this.m_abilityMod)
		{
			mod = this.m_abilityMod.m_laserInfoMod;
		}
		else
		{
			mod = null;
		}
		this.m_cachedLaserInfo = laserInfo.GetModifiedCopy(mod);
		StandardEffectInfo standardEffectInfo;
		if (this.m_abilityMod)
		{
			standardEffectInfo = this.m_abilityMod.m_tetherBaseEffectOverride.GetModifiedValue(this.m_laserHitEffect);
		}
		else
		{
			standardEffectInfo = this.m_laserHitEffect.GetShallowCopy();
		}
		StandardEffectInfo standardEffectInfo2 = standardEffectInfo;
		this.m_cachedEffectData = standardEffectInfo2.m_effectData;
		this.m_cachedEffectData.m_sequencePrefabs = new GameObject[]
		{
			this.m_targetPersistentSequence,
			this.m_beamSequence
		};
	}

	public StandardActorEffectData GetEnemyTetherEffectData()
	{
		StandardActorEffectData result;
		if (this.m_cachedEffectData != null)
		{
			result = this.m_cachedEffectData;
		}
		else
		{
			result = this.m_laserHitEffect.m_effectData;
		}
		return result;
	}

	public LaserTargetingInfo GetLaserInfo()
	{
		LaserTargetingInfo result;
		if (this.m_cachedLaserInfo != null)
		{
			result = this.m_cachedLaserInfo;
		}
		else
		{
			result = this.m_laserInfo;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "Damage_FirstTurn", "damage on initial attach", this.m_laserDamageAmount, false);
		base.AddTokenInt(tokens, "Damage_PerTurnAfterFirst", "damage on subsequent turns", this.m_laserHitEffect.m_effectData.m_damagePerTurn, false);
		base.AddTokenInt(tokens, "Damage_AdditionalOnRadiated", "additional damage on radiated", this.m_additionalEnergizedDamage, false);
		base.AddTokenInt(tokens, "Heal_OnCasterPerTurn", "heal on caster per turn", this.m_healOnCasterOnTick, false);
		base.AddTokenInt(tokens, "EnergyOnCasterPerTurn", string.Empty, this.m_energyOnCasterPerTurn, false);
		base.AddTokenInt(tokens, "MaxBonusEnergyFromGrowingGain", string.Empty, this.m_maxBonusEnergyFromGrowingGain, false);
		base.AddTokenInt(tokens, "BonusEnergyGrowthRate", string.Empty, this.m_bonusEnergyGrowthRate, false);
		base.AddTokenInt(tokens, "TetherDuration", string.Empty, this.m_tetherDuration, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		if (this.GetInitialDamage() > 0)
		{
			AbilityTooltipHelper.ReportDamage(ref result, AbilityTooltipSubject.Primary, this.GetInitialDamage());
		}
		this.GetEnemyTetherEffectData().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		if (this.m_healCasterOnIniialAttach)
		{
			if (this.GetHealOnCasterPerTurn() > 0)
			{
				AbilityTooltipHelper.ReportHealing(ref result, AbilityTooltipSubject.Self, this.GetHealOnCasterPerTurn());
			}
		}
		return result;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
		int result;
		if (visibleActorsCountByTooltipSubject > 0)
		{
			result = this.GetEnergyOnCasterPerTurn();
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public override List<int> symbol_001D()
	{
		List<int> list = base.symbol_001D();
		list.Add(this.m_laserHitEffect.m_effectData.m_damagePerTurn);
		return list;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SparkBasicAttack))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SparkBasicAttack);
			this.Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.Setup();
	}
}
