// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class MartyrHealingExplosion : MartyrLaserBase
{
	[Header("-- Targeting")]
	public LaserTargetingInfo m_laserInfo;
	public StandardEffectInfo m_laserHitEffect;
	public float m_explosionRadius = 2.5f;
	public bool m_laserCanHitAllies;
	public bool m_laserCanHitEnemies = true;
	public bool m_forceMaxLaserDistance = true;
	public bool m_explodeOnlyOnLaserHit = true;
	public bool m_explosionCanHitCaster;
	[Header("-- Damage, Healing & Crystal Bonuses")]
	public int m_baseLaserDamage = 20;
	public int m_baseExplosionHealing = 15;
	public int m_additionalDamagePerCrystalSpent;
	public int m_additionalHealingPerCrystalSpent;
	public float m_additionalRadiusPerCrystalSpent = 0.25f;
	public List<MartyrBasicAttackThreshold> m_thresholdBasedCrystalBonuses;
	[Header("-- Sequences")]
	public GameObject m_projectileSequence;

	private Martyr_SyncComponent m_syncComponent;
	private LaserTargetingInfo m_cachedLaserInfo;
	private StandardEffectInfo m_cachedLaserHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Martyr Healing Explosion";
		}
		m_syncComponent = GetComponent<Martyr_SyncComponent>();
		SetCachedFields();
		SetupTargeter();
	}

	protected override Martyr_SyncComponent GetSyncComponent()
	{
		return m_syncComponent;
	}

	protected void SetupTargeter()
	{
		AbilityUtil_Targeter_MartyrLaser abilityUtil_Targeter_MartyrLaser = new AbilityUtil_Targeter_MartyrLaser(this, GetCurrentLaserWidth(), GetCurrentLaserRange(), GetCurrentLaserPenetrateLoS(), GetCurrentLaserMaxTargets(), m_laserCanHitEnemies, m_laserCanHitAllies, false, !m_forceMaxLaserDistance, m_explodeOnlyOnLaserHit, GetCurrentExplosionRadius(), GetCurrentInnerExplosionRadius(), true, false, m_explosionCanHitCaster)
		{
			m_delegateLaserWidth = GetCurrentLaserWidth,
			m_delegateLaserRange = GetCurrentLaserRange,
			m_delegatePenetrateLos = GetCurrentLaserPenetrateLoS,
			m_delegateMaxTargets = GetCurrentLaserMaxTargets,
			m_delegateConeRadius = GetCurrentExplosionRadius,
			m_delegateInnerConeRadius = GetCurrentInnerExplosionRadius
		};
		Targeter = abilityUtil_Targeter_MartyrLaser;
	}

	private void SetCachedFields()
	{
		m_cachedLaserInfo = m_laserInfo;
		m_cachedLaserHitEffect = m_laserHitEffect;
	}

	public override LaserTargetingInfo GetLaserInfo()
	{
		return m_cachedLaserInfo ?? m_laserInfo;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		return m_cachedLaserHitEffect ?? m_laserHitEffect;
	}

	public float GetBaseExplosionRadius()
	{
		return m_explosionRadius;
	}

	public float GetBonusRadiusPerCrystalSpent()
	{
		return m_additionalRadiusPerCrystalSpent;
	}

	public int GetBaseDamage()
	{
		return m_baseLaserDamage;
	}

	public int GetBaseExplosionHealing()
	{
		return m_baseExplosionHealing;
	}

	public int GetBonusDamagePerCrystalSpent()
	{
		return m_additionalDamagePerCrystalSpent;
	}

	public int GetBonusHealingPerCrystalSpent()
	{
		return m_additionalHealingPerCrystalSpent;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod.AddToken_EffectInfo(tokens, m_laserHitEffect, "LaserHitEffect", m_laserHitEffect);
		tokens.Add(new TooltipTokenInt("BaseLaserDamage", "Damage from laser hits with no crystal bonus", GetBaseDamage()));
		tokens.Add(new TooltipTokenInt("BaseExplosionHealing", "Healing from explosion hits with no crystal bonus", GetBaseExplosionHealing()));
		tokens.Add(new TooltipTokenInt("DamagePerCrystal", "Damage added per crystal spent", GetBonusDamagePerCrystalSpent()));
		tokens.Add(new TooltipTokenInt("HealingPerCrystal", "Healing added per crystal spent", GetBonusHealingPerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("WidthPerCrystal", "Width added per crystal spent", GetBonusWidthPerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("LengthPerCrystal", "Length added per crystal spent", GetBonusLengthPerCrystalSpent()));
		tokens.Add(new TooltipTokenFloat("RadiusPerCrystal", "Explosion radius added per crystal spent", GetBonusRadiusPerCrystalSpent()));
	}

	protected override List<MartyrLaserThreshold> GetThresholdBasedCrystalBonusList()
	{
		List<MartyrLaserThreshold> list = new List<MartyrLaserThreshold>();
		foreach (MartyrBasicAttackThreshold thresholdBasedCrystalBonuse in m_thresholdBasedCrystalBonuses)
		{
			list.Add(thresholdBasedCrystalBonuse);
		}
		return list;
	}

	private int GetCurrentLaserDamage(ActorData caster)
	{
		int additionalDamage = (GetCurrentPowerEntry(caster) as MartyrBasicAttackThreshold)?.m_additionalDamage ?? 0;
		return GetBaseDamage()
			+ m_syncComponent.SpentDamageCrystals(caster) * GetBonusDamagePerCrystalSpent()
			+ additionalDamage;
	}

	private int GetCurrentExplosionHealing(ActorData caster)
	{
		MartyrHealingExplosionThreshold martyrHealingExplosionThreshold = GetCurrentPowerEntry(caster) as MartyrHealingExplosionThreshold;
		int additionalHealing = martyrHealingExplosionThreshold != null ? martyrHealingExplosionThreshold.m_additionalHealing : 0;
		return GetBaseExplosionHealing()
			+ m_syncComponent.SpentDamageCrystals(caster) * GetBonusHealingPerCrystalSpent()
			+ additionalHealing;
	}

	public override float GetCurrentExplosionRadius()
	{
		MartyrBasicAttackThreshold martyrBasicAttackThreshold = GetCurrentPowerEntry(ActorData) as MartyrBasicAttackThreshold;
		float additionalRadius = martyrBasicAttackThreshold != null ? martyrBasicAttackThreshold.m_additionalRadius : 0f;
		return GetBaseExplosionRadius()
			+ m_syncComponent.SpentDamageCrystals(ActorData) * GetBonusRadiusPerCrystalSpent()
			+ additionalRadius;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetBaseDamage());
		m_laserHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Secondary, GetBaseExplosionHealing());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, GetCurrentLaserDamage(ActorData));
		AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, GetCurrentExplosionHealing(ActorData), AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Secondary);
		return symbolToValue;
	}

#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		GetHitActors(targets, caster, out VectorUtils.LaserCoords laserCoords, null, !m_forceMaxLaserDistance);
		Sequence.IExtraSequenceParams[] adjustableRingSequenceParams = AbilityCommon_LayeredRings.GetAdjustableRingSequenceParams(GetCurrentExplosionRadius());
		list.Add(new ServerClientUtils.SequenceStartData(m_projectileSequence, laserCoords.end, additionalData.m_abilityResults.HitActorsArray(), caster, additionalData.m_sequenceSource, adjustableRingSequenceParams));
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(targets, caster, out VectorUtils.LaserCoords laserCoords, nonActorTargetInfo, !m_forceMaxLaserDistance);
		List<ActorData> actorInRadius = new List<ActorData>();
		if (hitActors.Count > 0 || !m_explodeOnlyOnLaserHit)
		{
			actorInRadius = AreaEffectUtils.GetActorsInRadius(laserCoords.end, GetCurrentExplosionRadius(), GetLaserInfo().penetrateLos, caster, caster.GetTeam(), nonActorTargetInfo);
			if (!m_explosionCanHitCaster && actorInRadius.Contains(caster))
			{
				actorInRadius.Remove(caster);
			}
		}
		foreach (ActorData target in hitActors)
		{
			ActorHitParameters hitParams = new ActorHitParameters(target, caster.GetFreePos());
			ActorHitResults hitResults = new ActorHitResults(GetCurrentLaserDamage(caster), HitActionType.Damage, GetLaserHitEffect(), hitParams);
			abilityResults.StoreActorHit(hitResults);
		}
		foreach (ActorData target in actorInRadius)
		{
			ActorHitParameters hitParams = new ActorHitParameters(target, caster.GetFreePos());
			ActorHitResults hitResults = new ActorHitResults(GetCurrentExplosionHealing(caster), HitActionType.Healing, hitParams);
			abilityResults.StoreActorHit(hitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	// added in rogues
	private List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, out VectorUtils.LaserCoords endPoints, List<NonActorTargetInfo> nonActorTargetInfo, bool clampToCursorPos = false)
	{
		List<Team> relevantTeams = TargeterUtils.GetRelevantTeams(caster, m_laserCanHitAllies, m_laserCanHitEnemies);
		VectorUtils.LaserCoords laserCoords;
		laserCoords.start = caster.GetLoSCheckPos();
		float laserRangeInSquares = GetCurrentLaserRange();
		if (clampToCursorPos)
		{
			laserRangeInSquares = Mathf.Min(VectorUtils.HorizontalPlaneDistInSquares(caster.GetFreePos(), targets[0].FreePos), laserRangeInSquares);
		}
		List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(laserCoords.start, targets[0].AimDirection, laserRangeInSquares, GetCurrentLaserWidth(), caster, relevantTeams, GetCurrentLaserPenetrateLoS(), GetCurrentLaserMaxTargets(), false, true, out laserCoords.end, nonActorTargetInfo, null, false, true);
		endPoints = laserCoords;
		return actorsInLaser;
	}
#endif
}
