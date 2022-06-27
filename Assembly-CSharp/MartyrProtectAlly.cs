// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class MartyrProtectAlly : MartyrLaserBase
{
	[Header("-- Damage reduction and redirection")]
	public float m_damageReductionOnTarget = 0.5f;
	public float m_damageRedirectToCaster = 0.5f;
	public int m_techPointGainPerRedirect = 3;
	public StandardEffectInfo m_laserHitEffect;
	[Space(10f)]
	public bool m_affectsEnemies;
	public bool m_affectsAllies = true;
	public bool m_penetratesLoS;
	[Header("-- Thorns effect on protected ally")]
	public StandardEffectInfo m_thornsEffect;
	public StandardEffectInfo m_returnEffectOnEnemy;
	public int m_thornsDamagePerHit;
	[Header("-- Absorb & Crystal Bonuses, Self")]
	public StandardEffectInfo m_effectOnSelf;
	public int m_baseAbsorb;
	public int m_absorbPerCrystalSpent = 5;
	[Header("-- Absorb on Ally")]
	public int m_baseAbsorbOnAlly;
	public int m_absorbOnAllyPerCrystalSpent = 5;
	public List<MartyrProtectAllyThreshold> m_thresholdBasedCrystalBonuses;
	[Header("-- Extra Energy per damage redirect")]
	public float m_extraEnergyPerRedirectDamage;
	[Header("-- Heal per damage redirect on next turn")]
	public float m_healOnTurnStartPerRedirectDamage;
	[Header("-- Sequences")]
	public GameObject m_allyShieldSequence;
	public GameObject m_projectileSequence;
	public GameObject m_redirectProjectileSequence;
	public GameObject m_thornsProjectileSequence;
	[Tooltip("Ignored if no effect or absorb is applied on the caster")]
	public GameObject m_selfShieldSequence;

	private Martyr_SyncComponent m_syncComponent;
	private AbilityMod_MartyrProtectAlly m_abilityMod;
	private StandardEffectInfo m_cachedLaserHitEffect;
	private StandardEffectInfo m_cachedThornsEffect;
	private StandardEffectInfo m_cachedReturnEffectOnEnemy;
	private StandardEffectInfo m_cachedEffectOnSelf;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Martyr Protect Ally";
		}
		Setup();
		ResetTooltipAndTargetingNumbers();
	}

	protected override Martyr_SyncComponent GetSyncComponent()
	{
		return m_syncComponent;
	}

	protected void Setup()
	{
		m_syncComponent = GetComponent<Martyr_SyncComponent>();
		SetCachedFields();
		AbilityUtil_Targeter_Shape targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, PenetratesLoS(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, AffectsEnemies(), AffectsAllies(), AbilityUtil_Targeter.AffectsActor.Possible, AbilityUtil_Targeter.AffectsActor.Always);
		targeter.m_affectCasterDelegate = delegate(ActorData caster, List<ActorData> actorsSoFar, bool casterInShape)
		{
			int currentAbsorb = GetCurrentAbsorb(caster);
			return currentAbsorb > 0;
		};
		Targeter = targeter;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		m_cachedLaserHitEffect = m_abilityMod
			? m_abilityMod.m_laserHitEffectMod.GetModifiedValue(m_laserHitEffect)
			: m_laserHitEffect;
		m_cachedThornsEffect = m_abilityMod
			? m_abilityMod.m_thornsEffectMod.GetModifiedValue(m_thornsEffect)
			: m_thornsEffect;
		m_cachedReturnEffectOnEnemy = m_abilityMod
			? m_abilityMod.m_returnEffectOnEnemyMod.GetModifiedValue(m_returnEffectOnEnemy)
			: m_returnEffectOnEnemy;
		m_cachedEffectOnSelf = m_abilityMod
			? m_abilityMod.m_effectOnSelfMod.GetModifiedValue(m_effectOnSelf)
			: m_effectOnSelf;
	}

	public float GetDamageReductionOnTarget()
	{
		return m_abilityMod
			? m_abilityMod.m_damageReductionOnTargetMod.GetModifiedValue(m_damageReductionOnTarget)
			: m_damageReductionOnTarget;
	}

	public float GetDamageRedirectToCaster()
	{
		return m_abilityMod
			? m_abilityMod.m_damageRedirectToCasterMod.GetModifiedValue(m_damageRedirectToCaster)
			: m_damageRedirectToCaster;
	}

	public int GetTechPointGainPerRedirect()
	{
		return m_abilityMod
			? m_abilityMod.m_techPointGainPerRedirectMod.GetModifiedValue(m_techPointGainPerRedirect)
			: m_techPointGainPerRedirect;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		return m_cachedLaserHitEffect ?? m_laserHitEffect;
	}

	public bool AffectsEnemies()
	{
		return m_abilityMod
			? m_abilityMod.m_affectsEnemiesMod.GetModifiedValue(m_affectsEnemies)
			: m_affectsEnemies;
	}

	public bool AffectsAllies()
	{
		return m_abilityMod
			? m_abilityMod.m_affectsAlliesMod.GetModifiedValue(m_affectsAllies)
			: m_affectsAllies;
	}

	public bool PenetratesLoS()
	{
		return m_abilityMod
			? m_abilityMod.m_penetratesLoSMod.GetModifiedValue(m_penetratesLoS)
			: m_penetratesLoS;
	}

	public StandardEffectInfo GetThornsEffect()
	{
		return m_cachedThornsEffect ?? m_thornsEffect;
	}

	public StandardEffectInfo GetReturnEffectOnEnemy()
	{
		return m_cachedReturnEffectOnEnemy ?? m_returnEffectOnEnemy;
	}

	public int GetThornsDamagePerHit()
	{
		return m_abilityMod
			? m_abilityMod.m_thornsDamagePerHitMod.GetModifiedValue(m_thornsDamagePerHit)
			: m_thornsDamagePerHit;
	}

	public StandardEffectInfo GetEffectOnSelf()
	{
		return m_cachedEffectOnSelf ?? m_effectOnSelf;
	}

	public int GetBaseAbsorb()
	{
		return m_abilityMod
			? m_abilityMod.m_baseAbsorbMod.GetModifiedValue(m_baseAbsorb)
			: m_baseAbsorb;
	}

	public int GetAbsorbPerCrystalSpent()
	{
		return m_abilityMod
			? m_abilityMod.m_absorbPerCrystalSpentMod.GetModifiedValue(m_absorbPerCrystalSpent)
			: m_absorbPerCrystalSpent;
	}

	public int GetBaseAbsorbOnAlly()
	{
		return m_abilityMod
			? m_abilityMod.m_baseAbsorbOnAllyMod.GetModifiedValue(m_baseAbsorbOnAlly)
			: m_baseAbsorbOnAlly;
	}

	public int GetAbsorbOnAllyPerCrystalSpent()
	{
		return m_abilityMod
			? m_abilityMod.m_absorbOnAllyPerCrystalSpentMod.GetModifiedValue(m_absorbOnAllyPerCrystalSpent)
			: m_absorbOnAllyPerCrystalSpent;
	}

	public float GetExtraEnergyPerRedirectDamage()
	{
		return m_abilityMod
			? m_abilityMod.m_extraEnergyPerRedirectDamageMod.GetModifiedValue(m_extraEnergyPerRedirectDamage)
			: m_extraEnergyPerRedirectDamage;
	}

	public float GetHealOnTurnStartPerRedirectDamage()
	{
		return m_abilityMod
			? m_abilityMod.m_healOnTurnStartPerRedirectDamageMod.GetModifiedValue(m_healOnTurnStartPerRedirectDamage)
			: m_healOnTurnStartPerRedirectDamage;
	}

	private int GetCurrentAbsorb(ActorData caster)
	{
		MartyrProtectAllyThreshold martyrProtectAllyThreshold = GetCurrentPowerEntry(caster) as MartyrProtectAllyThreshold;
		int additionalAbsorb = martyrProtectAllyThreshold != null ? martyrProtectAllyThreshold.m_additionalAbsorb : 0;
		return GetBaseAbsorb()
			+ m_syncComponent.SpentDamageCrystals(caster) * GetAbsorbPerCrystalSpent()
			+ additionalAbsorb;
	}

	private int GetCurrentAbsorbForAlly(ActorData caster)
	{
		MartyrProtectAllyThreshold martyrProtectAllyThreshold = GetCurrentPowerEntry(caster) as MartyrProtectAllyThreshold;
		int additionalAbsorbOnAlly = martyrProtectAllyThreshold != null ? martyrProtectAllyThreshold.m_additionalAbsorbOnAlly : 0;
		return GetBaseAbsorbOnAlly()
			+ m_syncComponent.SpentDamageCrystals(caster) * GetAbsorbOnAllyPerCrystalSpent()
			+ additionalAbsorbOnAlly;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddSpecificTooltipTokens(tokens, modAsBase);
		AbilityMod_MartyrProtectAlly abilityMod_MartyrProtectAlly = modAsBase as AbilityMod_MartyrProtectAlly;
		float damageReductionOnTarget_Pct = abilityMod_MartyrProtectAlly
			? abilityMod_MartyrProtectAlly.m_damageReductionOnTargetMod.GetModifiedValue(m_damageReductionOnTarget)
			: m_damageReductionOnTarget;
		AddTokenFloatAsPct(tokens, "DamageReductionOnTarget_Pct", "", damageReductionOnTarget_Pct);
		float damageRedirectToCaster_Pct = abilityMod_MartyrProtectAlly
			? abilityMod_MartyrProtectAlly.m_damageRedirectToCasterMod.GetModifiedValue(m_damageRedirectToCaster)
			: m_damageRedirectToCaster;
		AddTokenFloatAsPct(tokens, "DamageRedirectToCaster_Pct", "", damageRedirectToCaster_Pct);
		int techPointGainPerRedirect = abilityMod_MartyrProtectAlly
			? abilityMod_MartyrProtectAlly.m_techPointGainPerRedirectMod.GetModifiedValue(m_techPointGainPerRedirect)
			: m_techPointGainPerRedirect;
		AddTokenInt(tokens, "TechPointGainPerRedirect", "", techPointGainPerRedirect);
		StandardEffectInfo laserHitEffect = abilityMod_MartyrProtectAlly
			? abilityMod_MartyrProtectAlly.m_laserHitEffectMod.GetModifiedValue(m_laserHitEffect)
			: m_laserHitEffect;
		AbilityMod.AddToken_EffectInfo(tokens, laserHitEffect, "LaserHitEffect", m_laserHitEffect);
		StandardEffectInfo thornsEffect = abilityMod_MartyrProtectAlly
			? abilityMod_MartyrProtectAlly.m_thornsEffectMod.GetModifiedValue(m_thornsEffect)
			: m_thornsEffect;
		AbilityMod.AddToken_EffectInfo(tokens, thornsEffect, "ThornsEffect", m_thornsEffect);
		StandardEffectInfo returnEffectOnEnemy = abilityMod_MartyrProtectAlly
			? abilityMod_MartyrProtectAlly.m_returnEffectOnEnemyMod.GetModifiedValue(m_returnEffectOnEnemy)
			: m_returnEffectOnEnemy;
		AbilityMod.AddToken_EffectInfo(tokens, returnEffectOnEnemy, "ReturnEffectOnEnemy", m_returnEffectOnEnemy);
		int thornsDamagePerHit = abilityMod_MartyrProtectAlly
			? abilityMod_MartyrProtectAlly.m_thornsDamagePerHitMod.GetModifiedValue(m_thornsDamagePerHit)
			: m_thornsDamagePerHit;
		AddTokenInt(tokens, "ThornsDamagePerHit", "", thornsDamagePerHit);
		StandardEffectInfo effectOnSelf = abilityMod_MartyrProtectAlly
			? abilityMod_MartyrProtectAlly.m_effectOnSelfMod.GetModifiedValue(m_effectOnSelf)
			: m_effectOnSelf;
		AbilityMod.AddToken_EffectInfo(tokens, effectOnSelf, "EffectOnSelf", m_effectOnSelf);
		int baseAbsorb = abilityMod_MartyrProtectAlly
			? abilityMod_MartyrProtectAlly.m_baseAbsorbMod.GetModifiedValue(m_baseAbsorb)
			: m_baseAbsorb;
		AddTokenInt(tokens, "BaseAbsorb", "", baseAbsorb);
		int absorbPerCrystalSpent = abilityMod_MartyrProtectAlly
			? abilityMod_MartyrProtectAlly.m_absorbPerCrystalSpentMod.GetModifiedValue(m_absorbPerCrystalSpent)
			: m_absorbPerCrystalSpent;
		AddTokenInt(tokens, "AbsorbPerCrystalSpent", "", absorbPerCrystalSpent);
		int baseAbsorbOnAlly = abilityMod_MartyrProtectAlly
			? abilityMod_MartyrProtectAlly.m_baseAbsorbOnAllyMod.GetModifiedValue(m_baseAbsorbOnAlly)
			: m_baseAbsorbOnAlly;
		AddTokenInt(tokens, "BaseAbsorbOnAlly", "", baseAbsorbOnAlly);
		int absorbOnAllyPerCrystalSpent = abilityMod_MartyrProtectAlly
			? abilityMod_MartyrProtectAlly.m_absorbOnAllyPerCrystalSpentMod.GetModifiedValue(m_absorbOnAllyPerCrystalSpent)
			: m_absorbOnAllyPerCrystalSpent;
		AddTokenInt(tokens, "AbsorbOnAllyPerCrystalSpent", "", absorbOnAllyPerCrystalSpent);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetLaserHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		GetEffectOnSelf().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = base.CalculateNameplateTargetingNumbers();
		GetLaserHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Ally, 1);
		GetEffectOnSelf().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, 1);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> symbolToValue = new Dictionary<AbilityTooltipSymbol, int>();
		if (targetActor == ActorData)
		{
			AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, GetCurrentAbsorb(ActorData), AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Self);
		}
		else
		{
			AddNameplateValueForSingleHit(ref symbolToValue, Targeter, targetActor, GetCurrentAbsorbForAlly(ActorData), AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Ally);
		}
		return symbolToValue;
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return HasTargetableActorsInDecision(caster, AffectsEnemies(), AffectsAllies(), false, ValidateCheckPath.Ignore, !PenetratesLoS(), false);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		return CanTargetActorInDecision(caster, target.GetCurrentBestActorTarget(), AffectsEnemies(), AffectsAllies(), false, ValidateCheckPath.Ignore, !PenetratesLoS(), false);
	}

	protected override List<MartyrLaserThreshold> GetThresholdBasedCrystalBonusList()
	{
		List<MartyrLaserThreshold> list = new List<MartyrLaserThreshold>();
		foreach (MartyrProtectAllyThreshold bonus in m_thresholdBasedCrystalBonuses)
		{
			list.Add(bonus);
		}
		return list;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MartyrProtectAlly))
		{
			m_abilityMod = (abilityMod as AbilityMod_MartyrProtectAlly);
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
		List<ActorData> hitActors = GetHitActors(targets, caster, out VectorUtils.LaserCoords laserCoords, nonActorTargetInfo);
		list.Add(new ServerClientUtils.SequenceStartData(m_projectileSequence, laserCoords.end, hitActors.ToArray(), caster, additionalData.m_sequenceSource, new Sequence.IExtraSequenceParams[0]));
		if (GetEffectOnSelf().m_applyEffect || GetCurrentAbsorb(caster) > 0)
		{
			list.Add(new ServerClientUtils.SequenceStartData(m_selfShieldSequence, laserCoords.end, new ActorData[]
			{
				caster
			}, caster, additionalData.m_sequenceSource, new Sequence.IExtraSequenceParams[0]));
		}
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = GetHitActors(targets, caster, out VectorUtils.LaserCoords laserCoords, nonActorTargetInfo);
		foreach (ActorData hitActor in hitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, caster.GetFreePos()));
			StandardActorEffectData effectData = GetLaserHitEffect().m_effectData;
			effectData.m_absorbAmount = 0;
			MartyrDamageRedirectEffect damageRedirectEffect = new MartyrDamageRedirectEffect(AsEffectSource(), hitActor.GetCurrentBoardSquare(), hitActor, caster, true, new List<ActorData>
			{
				caster
			}, effectData, GetDamageReductionOnTarget(), GetDamageRedirectToCaster(), GetTechPointGainPerRedirect(), 0f, m_allyShieldSequence, m_redirectProjectileSequence);
			actorHitResults.AddEffect(damageRedirectEffect);

			if (GetCurrentAbsorbForAlly(caster) > 0)
			{
				StandardActorEffectData standardActorEffectData = new StandardActorEffectData();
				standardActorEffectData.SetValues("Martyr Ally Shield", effectData.m_duration, 0, 0, 0, ServerCombatManager.HealingType.Effect, 0, GetCurrentAbsorbForAlly(caster), new AbilityStatMod[0], new StatusType[0], StandardActorEffectData.StatusDelayMode.DefaultBehavior);
				StandardActorEffect allyShieldEffect = new StandardActorEffect(AsEffectSource(), hitActor.GetCurrentBoardSquare(), hitActor, caster, standardActorEffectData);
				actorHitResults.AddEffect(allyShieldEffect);
			}

			StandardEffectInfo thornsEffectInfo = GetThornsEffect();
			if (thornsEffectInfo != null && thornsEffectInfo.m_applyEffect)
			{
				BattleMonkThornsEffect thornsEffect = new BattleMonkThornsEffect(AsEffectSource(), caster.GetCurrentBoardSquare(), hitActor, caster, thornsEffectInfo.m_effectData, GetThornsDamagePerHit(), GetReturnEffectOnEnemy(), m_thornsProjectileSequence);
				actorHitResults.AddEffect(thornsEffect);
			}
			abilityResults.StoreActorHit(actorHitResults);
		}
		int currentAbsorb = GetCurrentAbsorb(caster);
		if (GetEffectOnSelf().m_applyEffect || currentAbsorb > 0)
		{
			StandardEffectInfo shallowCopy = GetEffectOnSelf().GetShallowCopy();
			shallowCopy.m_effectData = shallowCopy.m_effectData.GetShallowCopy();
			shallowCopy.m_effectData.m_absorbAmount = currentAbsorb;
			shallowCopy.m_applyEffect = true;
			ActorHitParameters hitParams = new ActorHitParameters(caster, caster.GetFreePos());
			ActorHitResults hitResults = new ActorHitResults(shallowCopy, hitParams);
			abilityResults.StoreActorHit(hitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}

	protected List<ActorData> GetHitActors(List<AbilityTarget> targets, ActorData caster, out VectorUtils.LaserCoords endPoints, List<NonActorTargetInfo> nonActorTargetInfo)
	{
		endPoints = default(VectorUtils.LaserCoords);
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		if (square != null
			&& square.OccupantActor != null
			&& !square.OccupantActor.IgnoreForAbilityHits
			&& (AffectsEnemies() || square.OccupantActor.GetTeam() == caster.GetTeam()))
		{
			return new List<ActorData>
			{
				square.OccupantActor
			};
		}
		return new List<ActorData>();
	}

	public override void OnExecutedActorHit_Effect(ActorData caster, ActorData target, ActorHitResults results)
	{
		ActorData effectCaster = results.m_hitParameters.Effect.Caster;
		if (effectCaster == target && results.FinalDamage > 0)
		{
			effectCaster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.MartyrStats.DamageRedirected, results.FinalDamage);
		}
	}
#endif
}
