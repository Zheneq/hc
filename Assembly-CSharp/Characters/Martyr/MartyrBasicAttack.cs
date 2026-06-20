// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class MartyrBasicAttack : MartyrLaserBase
{
	[Header("-- Targeting")]
	public LaserTargetingInfo m_laserInfo;
	public StandardEffectInfo m_laserHitEffect;
	public float m_explosionRadius = 2.5f;
	public float m_additionalRadiusPerCrystalSpent = 0.25f;
	[Header("-- Damage & Crystal Bonuses")]
	public int m_baseLaserDamage = 20;
	public int m_baseExplosionDamage = 15;
	public int m_additionalDamagePerCrystalSpent;
	[Space(5f)]
	public int m_extraDamageIfSingleHit;
	[Header("-- Inner Ring Radius and Damage")]
	public float m_innerRingRadius;
	public float m_innerRingExtraRadiusPerCrystal;
	[Space(5f)]
	public int m_innerRingDamage = 20;
	public int m_innerRingDamagePerCrystal;
	public List<MartyrBasicAttackThreshold> m_thresholdBasedCrystalBonuses;
	[Header("-- Sequences")]
	public GameObject m_projectileSequence;

	private Martyr_SyncComponent m_syncComponent;
	private AbilityMod_MartyrBasicAttack m_abilityMod;
	private LaserTargetingInfo m_cachedLaserInfo;
	private StandardEffectInfo m_cachedLaserHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Martyr Hit";
		}
		Setup();
	}

	protected override Martyr_SyncComponent GetSyncComponent()
	{
		return m_syncComponent;
	}

	protected void Setup()
	{
		SetCachedFields();
		m_syncComponent = GetComponent<Martyr_SyncComponent>();
		AbilityUtil_Targeter_MartyrLaser abilityUtil_Targeter_MartyrLaser = new AbilityUtil_Targeter_MartyrLaser(this, GetCurrentLaserWidth(), GetCurrentLaserRange(), GetCurrentLaserPenetrateLoS(), GetCurrentLaserMaxTargets(), true, false, false, true, false, GetCurrentExplosionRadius(), GetCurrentInnerExplosionRadius(), false, true, false)
		{
			m_delegateLaserWidth = GetCurrentLaserWidth,
			m_delegateLaserRange = GetCurrentLaserRange,
			m_delegatePenetrateLos = GetCurrentLaserPenetrateLoS,
			m_delegateMaxTargets = GetCurrentLaserMaxTargets,
			m_delegateConeRadius = GetCurrentExplosionRadius,
			m_delegateInnerConeRadius = GetCurrentInnerExplosionRadius
		};
		Targeter = abilityUtil_Targeter_MartyrLaser;
		Targeter.SetShowArcToShape(true);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetCurrentLaserRange() + GetCurrentExplosionRadius();
	}

	private void SetCachedFields()
	{
		m_cachedLaserInfo = m_abilityMod
			? m_abilityMod.m_laserInfoMod.GetModifiedValue(m_laserInfo)
			: m_laserInfo;
		m_cachedLaserHitEffect = m_abilityMod
			? m_abilityMod.m_laserHitEffectMod.GetModifiedValue(m_laserHitEffect)
			: m_laserHitEffect;
	}

	public override LaserTargetingInfo GetLaserInfo()
	{
		return m_cachedLaserInfo ?? m_laserInfo;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		return m_cachedLaserHitEffect ?? m_laserHitEffect;
	}

	public float GetExplosionRadius()
	{
		return m_abilityMod
			? m_abilityMod.m_explosionRadiusMod.GetModifiedValue(m_explosionRadius)
			: m_explosionRadius;
	}

	public int GetBaseLaserDamage()
	{
		return m_abilityMod
			? m_abilityMod.m_baseLaserDamageMod.GetModifiedValue(m_baseLaserDamage)
			: m_baseLaserDamage;
	}

	public int GetBaseExplosionDamage()
	{
		return m_abilityMod
			? m_abilityMod.m_baseExplosionDamageMod.GetModifiedValue(m_baseExplosionDamage)
			: m_baseExplosionDamage;
	}

	public int GetAdditionalDamagePerCrystalSpent()
	{
		return m_abilityMod
			? m_abilityMod.m_additionalDamagePerCrystalSpentMod.GetModifiedValue(m_additionalDamagePerCrystalSpent)
			: m_additionalDamagePerCrystalSpent;
	}

	public float GetAdditionalRadiusPerCrystalSpent()
	{
		return m_abilityMod
			? m_abilityMod.m_additionalRadiusPerCrystalSpentMod.GetModifiedValue(m_additionalRadiusPerCrystalSpent)
			: m_additionalRadiusPerCrystalSpent;
	}

	public int GetExtraDamageIfSingleHit()
	{
		return m_abilityMod
			? m_abilityMod.m_extraDamageIfSingleHitMod.GetModifiedValue(m_extraDamageIfSingleHit)
			: m_extraDamageIfSingleHit;
	}

	public float GetInnerRingRadius()
	{
		return m_abilityMod
			? m_abilityMod.m_innerRingRadiusMod.GetModifiedValue(m_innerRingRadius)
			: m_innerRingRadius;
	}

	public float GetInnerRingExtraRadiusPerCrystal()
	{
		return m_abilityMod
			? m_abilityMod.m_innerRingExtraRadiusPerCrystalMod.GetModifiedValue(m_innerRingExtraRadiusPerCrystal)
			: m_innerRingExtraRadiusPerCrystal;
	}

	public int GetInnerRingDamage()
	{
		return m_abilityMod
			? m_abilityMod.m_innerRingDamageMod.GetModifiedValue(m_innerRingDamage)
			: m_innerRingDamage;
	}

	public int GetInnerRingDamagePerCrystal()
	{
		return m_abilityMod
			? m_abilityMod.m_innerRingDamagePerCrystalMod.GetModifiedValue(m_innerRingDamagePerCrystal)
			: m_innerRingDamagePerCrystal;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_MartyrBasicAttack abilityMod_MartyrBasicAttack = modAsBase as AbilityMod_MartyrBasicAttack;
		StandardEffectInfo laserHitEffect = abilityMod_MartyrBasicAttack
			? abilityMod_MartyrBasicAttack.m_laserHitEffectMod.GetModifiedValue(m_laserHitEffect)
			: m_laserHitEffect;
		AbilityMod.AddToken_EffectInfo(tokens, laserHitEffect, "LaserHitEffect", m_laserHitEffect);
		int baseLaserDamage = abilityMod_MartyrBasicAttack
			? abilityMod_MartyrBasicAttack.m_baseLaserDamageMod.GetModifiedValue(m_baseLaserDamage)
			: m_baseLaserDamage;
		AddTokenInt(tokens, "BaseLaserDamage", "", baseLaserDamage);
		int baseExplosionDamage = abilityMod_MartyrBasicAttack
			? abilityMod_MartyrBasicAttack.m_baseExplosionDamageMod.GetModifiedValue(m_baseExplosionDamage)
			: m_baseExplosionDamage;
		AddTokenInt(tokens, "BaseExplosionDamage", "", baseExplosionDamage);
		int additionalDamagePerCrystalSpent = abilityMod_MartyrBasicAttack
			? abilityMod_MartyrBasicAttack.m_additionalDamagePerCrystalSpentMod.GetModifiedValue(m_additionalDamagePerCrystalSpent)
			: m_additionalDamagePerCrystalSpent;
		AddTokenInt(tokens, "AdditionalDamagePerCrystalSpent", "", additionalDamagePerCrystalSpent);
		float additionalRadiusPerCrystalSpent = abilityMod_MartyrBasicAttack
			? abilityMod_MartyrBasicAttack.m_additionalRadiusPerCrystalSpentMod.GetModifiedValue(m_additionalRadiusPerCrystalSpent)
			: m_additionalRadiusPerCrystalSpent;
		AddTokenFloat(tokens, "AdditionalRadiusPerCrystalSpent", "", additionalRadiusPerCrystalSpent);
		int extraDamageIfSingleHit = abilityMod_MartyrBasicAttack
			? abilityMod_MartyrBasicAttack.m_extraDamageIfSingleHitMod.GetModifiedValue(m_extraDamageIfSingleHit)
			: m_extraDamageIfSingleHit;
		AddTokenInt(tokens, "ExtraDamageIfSingleHit", "", extraDamageIfSingleHit);
		int innerRingDamage = abilityMod_MartyrBasicAttack
			? abilityMod_MartyrBasicAttack.m_innerRingDamageMod.GetModifiedValue(m_innerRingDamage)
			: m_innerRingDamage;
		AddTokenInt(tokens, "InnerRingDamage", "", innerRingDamage);
		int innerRingDamagePerCrystal = abilityMod_MartyrBasicAttack
			? abilityMod_MartyrBasicAttack.m_innerRingDamagePerCrystalMod.GetModifiedValue(m_innerRingDamagePerCrystal)
			: m_innerRingDamagePerCrystal;
		AddTokenInt(tokens, "InnerRingDamagePerCrystal", "", innerRingDamagePerCrystal);
	}

	protected override List<MartyrLaserThreshold> GetThresholdBasedCrystalBonusList()
	{
		List<MartyrLaserThreshold> list = new List<MartyrLaserThreshold>();
		foreach (MartyrBasicAttackThreshold current in m_thresholdBasedCrystalBonuses)
		{
			list.Add(current);
		}
		return list;
	}

	private int GetCurrentLaserDamage(ActorData caster)
	{
		MartyrBasicAttackThreshold martyrBasicAttackThreshold = GetCurrentPowerEntry(caster) as MartyrBasicAttackThreshold;
		int additionalDamage = martyrBasicAttackThreshold != null ? martyrBasicAttackThreshold.m_additionalDamage : 0;
		return GetBaseLaserDamage()
			+ m_syncComponent.SpentDamageCrystals(caster) * GetAdditionalDamagePerCrystalSpent()
			+ additionalDamage;
	}

	private int GetCurrentExplosionDamage(ActorData caster)
	{
		MartyrBasicAttackThreshold martyrBasicAttackThreshold = GetCurrentPowerEntry(caster) as MartyrBasicAttackThreshold;
		int additionalDamage = martyrBasicAttackThreshold != null ? martyrBasicAttackThreshold.m_additionalDamage : 0;
		return GetBaseExplosionDamage()
			+ m_syncComponent.SpentDamageCrystals(caster) * GetAdditionalDamagePerCrystalSpent()
			+ additionalDamage;
	}

	public override float GetCurrentExplosionRadius()
	{
		MartyrBasicAttackThreshold martyrBasicAttackThreshold = GetCurrentPowerEntry(ActorData) as MartyrBasicAttackThreshold;
		float additionalRadius = martyrBasicAttackThreshold != null ? martyrBasicAttackThreshold.m_additionalRadius : 0f;
		return GetExplosionRadius()
			+ m_syncComponent.SpentDamageCrystals(ActorData) * GetAdditionalRadiusPerCrystalSpent()
			+ additionalRadius;
	}

	public int GetCurrentInnerExplosionDamage(ActorData caster)
	{
		return GetInnerRingDamage() + m_syncComponent.SpentDamageCrystals(caster) * GetInnerRingDamagePerCrystal();
	}

	public override float GetCurrentInnerExplosionRadius()
	{
		return GetInnerRingRadius() + m_syncComponent.SpentDamageCrystals(ActorData) * GetInnerRingExtraRadiusPerCrystal();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetBaseLaserDamage());
		m_laserHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Secondary, GetBaseExplosionDamage());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		int extraDamage = 0;
		if (GetExtraDamageIfSingleHit() > 0)
		{
			int visibleActorsCountByTooltipSubject = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			if (visibleActorsCountByTooltipSubject == 1)
			{
				extraDamage = GetExtraDamageIfSingleHit();
			}
		}
		AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, GetCurrentLaserDamage(ActorData) + extraDamage);
		ActorData actorData = ActorData;
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Secondary))
		{
			bool isInnerDamge = false;
			float currentInnerExplosionRadius = GetCurrentInnerExplosionRadius();
			if (currentInnerExplosionRadius > 0f && Targeter is AbilityUtil_Targeter_MartyrLaser)
			{
				AbilityUtil_Targeter_MartyrLaser abilityUtil_Targeter_MartyrLaser = Targeter as AbilityUtil_Targeter_MartyrLaser;
				isInnerDamge = AreaEffectUtils.IsSquareInConeByActorRadius(targetActor.GetCurrentBoardSquare(), abilityUtil_Targeter_MartyrLaser.m_lastLaserEndPos, 0f, 360f, currentInnerExplosionRadius, 0f, true, actorData);
			}
			int baseDamage = isInnerDamge
				? GetCurrentInnerExplosionDamage(actorData)
				: GetCurrentExplosionDamage(actorData);
			symbolToValue[AbilityTooltipSymbol.Damage] = baseDamage + extraDamage;
		}
		return symbolToValue;
	}

	public override TargetingParadigm GetControlpadTargetingParadigm(int targetIndex)
	{
		return TargetingParadigm.Position;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MartyrBasicAttack))
		{
			m_abilityMod = (abilityMod as AbilityMod_MartyrBasicAttack);
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		VectorUtils.LaserCoords laserCoords;
		GetHitActors(targets, caster, out laserCoords, nonActorTargetInfo, true);
		Sequence.IExtraSequenceParams[] adjustableRingSequenceParams = AbilityCommon_LayeredRings.GetAdjustableRingSequenceParams(GetCurrentExplosionRadius());
		list.Add(new ServerClientUtils.SequenceStartData(m_projectileSequence, laserCoords.end, additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource, adjustableRingSequenceParams));
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(targets, caster, out VectorUtils.LaserCoords laserCoords, nonActorTargetInfo, true);
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(laserCoords.end, GetCurrentExplosionRadius(), GetLaserInfo().penetrateLos, caster, caster.GetOtherTeams(), nonActorTargetInfo);
		int extraDamage = 0;
		if (GetExtraDamageIfSingleHit() > 0 && hitActors.Count + actorsInRadius.Count == 1)
		{
			extraDamage = GetExtraDamageIfSingleHit();
		}
		foreach (ActorData target in hitActors)
		{
			ActorHitParameters hitParams = new ActorHitParameters(target, caster.GetFreePos());
			ActorHitResults hitResults = new ActorHitResults(GetCurrentLaserDamage(caster) + extraDamage, HitActionType.Damage, GetLaserHitEffect(), hitParams);
			abilityResults.StoreActorHit(hitResults);
		}
		foreach (ActorData actorData in actorsInRadius)
		{
			if (!hitActors.Contains(actorData))
			{
				bool isInnerDamage = false;
				if (GetCurrentInnerExplosionRadius() > 0f)
				{
					isInnerDamage = AreaEffectUtils.IsSquareInConeByActorRadius(actorData.GetCurrentBoardSquare(), laserCoords.end, 0f, 360f, GetCurrentInnerExplosionRadius(), 0f, true, caster);
				}
				ActorHitParameters hitParams = new ActorHitParameters(actorData, laserCoords.end);
				int baseDamage = isInnerDamage
					? GetCurrentInnerExplosionDamage(actorData)
					: GetCurrentExplosionDamage(actorData);
				ActorHitResults hitResults = new ActorHitResults(baseDamage + extraDamage, HitActionType.Damage, GetLaserHitEffect(), hitParams);
				abilityResults.StoreActorHit(hitResults);
			}
		}
		if (!hitActors.Contains(caster) && !actorsInRadius.Contains(caster))
		{
			ActorHitResults hitResults3 = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			abilityResults.StoreActorHit(hitResults3);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, out VectorUtils.LaserCoords endPoints, List<NonActorTargetInfo> nonActorTargetInfo, bool clampToCursorPos = false)
	{
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, GetLaserInfo().affectsAllies, GetLaserInfo().affectsEnemies);
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		float num = GetCurrentLaserRange();
		if (clampToCursorPos)
		{
			num = Mathf.Min(VectorUtils.HorizontalPlaneDistInSquares(caster.GetFreePos(), targets[0].FreePos), num);
		}
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, targets[0].AimDirection, num, GetCurrentLaserWidth(), caster, relevantTeams, GetCurrentLaserPenetrateLoS(), GetCurrentLaserMaxTargets(), false, true, out laserCoords.end, nonActorTargetInfo, null, false, true);
		endPoints = laserCoords;
		return actorsInLaser;
	}
#endif
}
