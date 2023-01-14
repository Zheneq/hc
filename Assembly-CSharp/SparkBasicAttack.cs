// ROGUES
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

	[Separator("Tether", true)] // reactor
	//[Space(10f)] // rogues
	public float m_tetherDistance = 5f;

	// removed in rogues
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
			Targeter.ResetTargeter(true);  // no params in rogues
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

	// removed in rogues
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
		// removed in rogues
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

#if SERVER
	public SparkBasicAttackEffect CreateDamageTetherEffect(ActorData caster, ActorData hitActor)
	{
		StandardActorEffectData enemyTetherEffectData = GetEnemyTetherEffectData();
		return new SparkBasicAttackEffect(
			AsEffectSource(),
			caster.GetCurrentBoardSquare(),
			hitActor,
			caster,
			enemyTetherEffectData,
			GetTetherDistance(),
			GetTetherDuration(), // custom
			GetHealOnCasterPerTurn(),
			GetAdditionalDamageOnRadiated(),
			GetEnergyOnCasterPerTurn(),
			m_pulseAnimIndex,
			m_energizedPulseAnimIndex,
			m_pulseSequence,
			m_energizedPulseSequence);
	}
#endif

#if SERVER
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		VectorUtils.LaserCoords laserCoords;
		GetHitActors(targets, caster, out laserCoords, null);
		ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(m_castSequence, laserCoords.end, additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource, null);
		list.Add(item);
		return list;
	}
#endif

#if SERVER
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		Log.Info($"Gathering results for {caster.DisplayName}'s SparkBasicAttack");
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		VectorUtils.LaserCoords laserCoords;
		List<ActorData> hitActors = GetHitActors(targets, caster, out laserCoords, nonActorTargetInfo);
		foreach (ActorData actorData in hitActors)
		{
			ActorHitParameters hitParams = new ActorHitParameters(actorData, caster.GetFreePos());
			ActorHitResults actorHitResults = new ActorHitResults(GetInitialDamage(), HitActionType.Damage, hitParams);
			actorHitResults.SetTechPointGainOnCaster(GetEnergyOnCasterPerTurn());
			SetExistingEffectsForRemoval(caster, actorHitResults);
			SparkBasicAttackEffect effect = CreateDamageTetherEffect(caster, actorData);
			actorHitResults.AddEffect(effect);
			abilityResults.StoreActorHit(actorHitResults);
		}
		if (hitActors.Count > 0 && m_healCasterOnIniialAttach && GetHealOnCasterPerTurn() > 0)
		{
			ActorHitResults actorHitResults2 = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			actorHitResults2.SetBaseHealing(GetHealOnCasterPerTurn());
			abilityResults.StoreActorHit(actorHitResults2);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
#endif

#if SERVER
	protected virtual List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, out VectorUtils.LaserCoords endPoints, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, GetLaserInfo().affectsAllies, GetLaserInfo().affectsEnemies);
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, targets[0].AimDirection, GetLaserInfo().range, GetLaserInfo().width, caster, relevantTeams, GetLaserInfo().penetrateLos, GetLaserInfo().maxTargets, false, true, out laserCoords.end, nonActorTargetInfo, null, false, true);
		endPoints = laserCoords;
		return actorsInLaser;
	}
#endif

#if SERVER
	public void SetExistingEffectsForRemoval(ActorData caster, ActorHitResults hitResult)
	{
		foreach (int actorIndex in caster.GetComponent<SparkBeamTrackerComponent>().GetBeamActorIndices())
		{
			ActorData actorOfActorIndex = GameplayUtils.GetActorOfActorIndex(actorIndex);
			if (actorOfActorIndex != null)
			{
				foreach (Effect effect in ServerEffectManager.Get().GetEffectsOnTargetByCaster(actorOfActorIndex, caster, typeof(SparkBasicAttackEffect)))
				{
					SparkBasicAttackEffect effect2 = effect as SparkBasicAttackEffect;
					hitResult.AddEffectForRemoval(effect2, ServerEffectManager.Get().GetActorEffects(actorOfActorIndex));
				}
			}
		}
	}
#endif
}
