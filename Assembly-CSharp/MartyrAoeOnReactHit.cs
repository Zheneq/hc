// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class MartyrAoeOnReactHit : Ability
{
	[Header("-- Targeting --")]
	public bool m_canTargetEnemy = true;
	public bool m_canTargetAlly = true;
	public bool m_canTargetSelf = true;
	[Space(10f)]
	public bool m_targetingIgnoreLos;
	[Header("-- Base Effect Data")]
	public StandardActorEffectData m_enemyBaseEffectData;
	public StandardActorEffectData m_allyBaseEffectData;
	[Header("-- Extra Shielding for Allies")]
	public int m_extraAbsorbPerCrystal;
	[Header("-- For React Area --")]
	public float m_reactBaseRadius = 1.5f;
	public float m_reactRadiusPerCrystal = 0.25f;
	public bool m_reactOnlyOncePerTurn;
	public bool m_reactPenetrateLos;
	public bool m_reactIncludeEffectTarget = true;
	[Header("-- On React Hit --")]
	public int m_reactAoeDamage = 10;
	public int m_reactDamagePerCrystal = 3;
	public StandardEffectInfo m_reactEnemyHitEffect;
	public int m_reactHealOnTarget;
	public int m_reactEnergyOnCasterPerReact;
	[Header("-- Cooldown reduction if no reacts")]
	public int m_cdrIfNoReactionTriggered;
	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;
	public GameObject m_onReactTriggerSequencePrefab;

	private Martyr_SyncComponent m_syncComp;
	private AbilityMod_MartyrAoeOnReactHit m_abilityMod;
	private StandardActorEffectData m_cachedEnemyBaseEffectData;
	private StandardActorEffectData m_cachedAllyBaseEffectData;
	private StandardEffectInfo m_cachedReactEnemyHitEffect;

	// added in rogues
#if SERVER
	private Passive_Martyr m_passive;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "MartyrAoeOnReactHit";
		}
		Setup();
	}

	private void Setup()
	{
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Martyr_SyncComponent>();
		}
#if SERVER
		if (m_passive == null)
		{
			PassiveData component = GetComponent<PassiveData>();
			if (component != null)
			{
				m_passive = component.GetPassiveOfType(typeof(Passive_Martyr)) as Passive_Martyr;
			}
		}
#endif
		SetCachedFields();
		AbilityUtil_Targeter_AoE_AroundActor targeter = new AbilityUtil_Targeter_AoE_AroundActor(this, 1f, ReactPenetrateLos(), true, false, -1, CanTargetEnemy(), CanTargetAlly(), CanTargetSelf());
		targeter.m_customRadiusDelegate = GetRadiusForTargeter;
		Targeter = targeter;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		m_cachedEnemyBaseEffectData = m_abilityMod
			? m_abilityMod.m_enemyBaseEffectDataMod.GetModifiedValue(m_enemyBaseEffectData)
			: m_enemyBaseEffectData;
		m_cachedAllyBaseEffectData = m_abilityMod
			? m_abilityMod.m_allyBaseEffectDataMod.GetModifiedValue(m_allyBaseEffectData)
			: m_allyBaseEffectData;
		m_cachedReactEnemyHitEffect = m_abilityMod
			? m_abilityMod.m_reactEnemyHitEffectMod.GetModifiedValue(m_reactEnemyHitEffect)
			: m_reactEnemyHitEffect;
	}

	public bool CanTargetEnemy()
	{
		return m_abilityMod
			? m_abilityMod.m_canTargetEnemyMod.GetModifiedValue(m_canTargetEnemy)
			: m_canTargetEnemy;
	}

	public bool CanTargetAlly()
	{
		return m_abilityMod
			? m_abilityMod.m_canTargetAllyMod.GetModifiedValue(m_canTargetAlly)
			: m_canTargetAlly;
	}

	public bool CanTargetSelf()
	{
		return m_abilityMod
			? m_abilityMod.m_canTargetSelfMod.GetModifiedValue(m_canTargetSelf)
			: m_canTargetSelf;
	}

	public bool TargetingIgnoreLos()
	{
		return m_abilityMod
			? m_abilityMod.m_targetingIgnoreLosMod.GetModifiedValue(m_targetingIgnoreLos)
			: m_targetingIgnoreLos;
	}

	public StandardActorEffectData GetEnemyBaseEffectData()
	{
		return m_cachedEnemyBaseEffectData ?? m_enemyBaseEffectData;
	}

	public StandardActorEffectData GetAllyBaseEffectData()
	{
		return m_cachedAllyBaseEffectData ?? m_allyBaseEffectData;
	}

	public int GetExtraAbsorbPerCrystal()
	{
		return m_abilityMod
			? m_abilityMod.m_extraAbsorbPerCrystalMod.GetModifiedValue(m_extraAbsorbPerCrystal)
			: m_extraAbsorbPerCrystal;
	}

	public float GetReactBaseRadius()
	{
		return m_abilityMod
			? m_abilityMod.m_reactBaseRadiusMod.GetModifiedValue(m_reactBaseRadius)
			: m_reactBaseRadius;
	}

	public float GetReactRadiusPerCrystal()
	{
		return m_abilityMod
			? m_abilityMod.m_reactRadiusPerCrystalMod.GetModifiedValue(m_reactRadiusPerCrystal)
			: m_reactRadiusPerCrystal;
	}

	public bool ReactOnlyOncePerTurn()
	{
		return m_abilityMod
			? m_abilityMod.m_reactOnlyOncePerTurnMod.GetModifiedValue(m_reactOnlyOncePerTurn)
			: m_reactOnlyOncePerTurn;
	}

	public bool ReactPenetrateLos()
	{
		return m_abilityMod
			? m_abilityMod.m_reactPenetrateLosMod.GetModifiedValue(m_reactPenetrateLos)
			: m_reactPenetrateLos;
	}

	public bool ReactIncludeEffectTarget()
	{
		return (!m_abilityMod) ? m_reactIncludeEffectTarget : m_abilityMod.m_reactIncludeEffectTargetMod.GetModifiedValue(m_reactIncludeEffectTarget);
	}

	public int GetReactAoeDamage()
	{
		return m_abilityMod
			? m_abilityMod.m_reactAoeDamageMod.GetModifiedValue(m_reactAoeDamage)
			: m_reactAoeDamage;
	}

	public int GetReactDamagePerCrystal()
	{
		return m_abilityMod
			? m_abilityMod.m_reactDamagePerCrystalMod.GetModifiedValue(m_reactDamagePerCrystal)
			: m_reactDamagePerCrystal;
	}

	public StandardEffectInfo GetReactEnemyHitEffect()
	{
		return m_cachedReactEnemyHitEffect ?? m_reactEnemyHitEffect;
	}

	public int GetReactHealOnTarget()
	{
		return m_abilityMod
			? m_abilityMod.m_reactHealOnTargetMod.GetModifiedValue(m_reactHealOnTarget)
			: m_reactHealOnTarget;
	}

	public int GetReactEnergyOnCasterPerReact()
	{
		return m_abilityMod
			? m_abilityMod.m_reactEnergyOnCasterPerReactMod.GetModifiedValue(m_reactEnergyOnCasterPerReact)
			: m_reactEnergyOnCasterPerReact;
	}

	public int GetCdrIfNoReactionTriggered()
	{
		return m_abilityMod
			? m_abilityMod.m_cdrIfNoReactionTriggeredMod.GetModifiedValue(m_cdrIfNoReactionTriggered)
			: m_cdrIfNoReactionTriggered;
	}

	public float GetRadiusForTargeter(AbilityTarget currentTarget, ActorData targetingActor)
	{
		return GetCurrentRadius();
	}

	public float GetCurrentRadius()
	{
		float num = GetReactBaseRadius();
		if (m_syncComp != null && GetReactRadiusPerCrystal() > 0f)
		{
			num += GetReactRadiusPerCrystal() * Mathf.Max(0, m_syncComp.DamageCrystals);
		}
		return num;
	}

	public int GetTotalDamage()
	{
		int num = GetReactAoeDamage();
		if (m_syncComp != null && GetReactDamagePerCrystal() > 0)
		{
			num += GetReactDamagePerCrystal() * Mathf.Max(0, m_syncComp.DamageCrystals);
		}
		return num;
	}

	public int GetCurrentExtraAbsorb(ActorData caster)
	{
		int num = 0;
		if (m_syncComp != null && GetExtraAbsorbPerCrystal() > 0)
		{
			num += m_syncComp.SpentDamageCrystals(caster) * GetExtraAbsorbPerCrystal();
		}
		return num;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		return CanTargetActorInDecision(caster, target.GetCurrentBestActorTarget(), CanTargetEnemy(), CanTargetAlly(), CanTargetSelf(), ValidateCheckPath.Ignore, !TargetingIgnoreLos(), true);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return HasTargetableActorsInDecision(caster, CanTargetEnemy(), CanTargetAlly(), CanTargetSelf(), ValidateCheckPath.Ignore, !TargetingIgnoreLos(), true);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Ally, 1);
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, 1);
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, 1);
		return numbers;
	}

	public override bool GetCustomTargeterNumbers(ActorData targetActor, int currentTargeterIndex, TargetingNumberUpdateScratch results)
	{
		results.m_absorb = 0;
		results.m_damage = 0;
		results.m_healing = 0;
		if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Ally) > 0
			|| Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Self) > 0)
		{
			results.m_absorb = GetAllyBaseEffectData().m_absorbAmount + GetCurrentExtraAbsorb(ActorData);
		}
		else if (Targeter.GetTooltipSubjectCountOnActor(targetActor, AbilityTooltipSubject.Enemy) > 0)
		{
			results.m_damage = GetTotalDamage();
		}
		return true;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_MartyrAoeOnReactHit abilityMod_MartyrAoeOnReactHit = modAsBase as AbilityMod_MartyrAoeOnReactHit;
		StandardActorEffectData enemyEffectData = abilityMod_MartyrAoeOnReactHit
			? abilityMod_MartyrAoeOnReactHit.m_enemyBaseEffectDataMod.GetModifiedValue(m_enemyBaseEffectData)
			: m_enemyBaseEffectData;
		enemyEffectData.AddTooltipTokens(tokens, "EnemyBaseEffectData", abilityMod_MartyrAoeOnReactHit != null, m_enemyBaseEffectData);
		StandardActorEffectData allayEffectData = abilityMod_MartyrAoeOnReactHit
			? abilityMod_MartyrAoeOnReactHit.m_allyBaseEffectDataMod.GetModifiedValue(m_allyBaseEffectData)
			: m_allyBaseEffectData;
		allayEffectData.AddTooltipTokens(tokens, "AllyBaseEffectData", abilityMod_MartyrAoeOnReactHit != null, m_allyBaseEffectData);
		int extraAbsorbPerCrystal = abilityMod_MartyrAoeOnReactHit
			? abilityMod_MartyrAoeOnReactHit.m_extraAbsorbPerCrystalMod.GetModifiedValue(m_extraAbsorbPerCrystal)
			: m_extraAbsorbPerCrystal;
		AddTokenInt(tokens, "ExtraAbsorbPerCrystal", "", extraAbsorbPerCrystal);
		int reactAoeDamage = abilityMod_MartyrAoeOnReactHit
			? abilityMod_MartyrAoeOnReactHit.m_reactAoeDamageMod.GetModifiedValue(m_reactAoeDamage)
			: m_reactAoeDamage;
		AddTokenInt(tokens, "ReactAoeDamage", "", reactAoeDamage);
		int reactDamagePerCrystal = abilityMod_MartyrAoeOnReactHit
			? abilityMod_MartyrAoeOnReactHit.m_reactDamagePerCrystalMod.GetModifiedValue(m_reactDamagePerCrystal)
			: m_reactDamagePerCrystal;
		AddTokenInt(tokens, "ReactDamagePerCrystal", "", reactDamagePerCrystal);
		StandardEffectInfo reactEnemyHitEffect = abilityMod_MartyrAoeOnReactHit
			? abilityMod_MartyrAoeOnReactHit.m_reactEnemyHitEffectMod.GetModifiedValue(m_reactEnemyHitEffect)
			: m_reactEnemyHitEffect;
		AbilityMod.AddToken_EffectInfo(tokens, reactEnemyHitEffect, "ReactEnemyHitEffect", m_reactEnemyHitEffect);
		int reactHealOnTarget = abilityMod_MartyrAoeOnReactHit
			? abilityMod_MartyrAoeOnReactHit.m_reactHealOnTargetMod.GetModifiedValue(m_reactHealOnTarget)
			: m_reactHealOnTarget;
		AddTokenInt(tokens, "ReactHealOnTarget", "", reactHealOnTarget);
		int reactEnergyOnCasterPerReact = abilityMod_MartyrAoeOnReactHit
			? abilityMod_MartyrAoeOnReactHit.m_reactEnergyOnCasterPerReactMod.GetModifiedValue(m_reactEnergyOnCasterPerReact)
			: m_reactEnergyOnCasterPerReact;
		AddTokenInt(tokens, "ReactEnergyOnCasterPerReact", "", reactEnergyOnCasterPerReact);
		int cdrIfNoReactionTriggered = abilityMod_MartyrAoeOnReactHit
			? abilityMod_MartyrAoeOnReactHit.m_cdrIfNoReactionTriggeredMod.GetModifiedValue(m_cdrIfNoReactionTriggered)
			: m_cdrIfNoReactionTriggered;
		AddTokenInt(tokens, "CdrIfNoReactionTriggered", "", cdrIfNoReactionTriggered);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_MartyrAoeOnReactHit))
		{
			m_abilityMod = (abilityMod as AbilityMod_MartyrAoeOnReactHit);
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
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		ActorData hitActor = GetHitActor(targets, caster);
		ServerClientUtils.SequenceStartData item = new ServerClientUtils.SequenceStartData(m_castSequencePrefab, square, (hitActor == null) ? new ActorData[0] : hitActor.AsArray(), caster, additionalData.m_sequenceSource, null);
		list.Add(item);
		return list;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		ActorData hitActor = GetHitActor(targets, caster);
		if (hitActor != null)
		{
			bool isAlly = hitActor.GetTeam() == caster.GetTeam();
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, caster.GetFreePos()));
			StandardActorEffectData standardActorEffectData = isAlly ? GetAllyBaseEffectData() : GetEnemyBaseEffectData();
			int currentExtraAbsorb = GetCurrentExtraAbsorb(caster);
			if (isAlly && currentExtraAbsorb > 0)
			{
				standardActorEffectData = GetAllyBaseEffectData().GetShallowCopy();
				standardActorEffectData.m_absorbAmount += currentExtraAbsorb;
			}
			MartyrAoeOnReactHitEffect effect = new MartyrAoeOnReactHitEffect(AsEffectSource(), square, hitActor, caster, standardActorEffectData, m_syncComp, m_passive, GetCurrentRadius(), ReactPenetrateLos(), GetTotalDamage(), GetReactEnemyHitEffect(), GetReactHealOnTarget(), GetReactEnergyOnCasterPerReact(), ReactIncludeEffectTarget(), ReactOnlyOncePerTurn(), m_onReactTriggerSequencePrefab);
			actorHitResults.AddEffect(effect);
			abilityResults.StoreActorHit(actorHitResults);
		}
	}

	// added in rogues
	private ActorData GetHitActor(List<AbilityTarget> targets, ActorData caster)
	{
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		ActorData actorData = (square != null) ? square.OccupantActor : null;
		if (actorData != null && !actorData.IgnoreForAbilityHits)
		{
			bool flag = actorData.GetTeam() == caster.GetTeam();
			if ((CanTargetSelf() && actorData == caster) || (CanTargetAlly() && flag) || (CanTargetEnemy() && !flag))
			{
				return actorData;
			}
		}
		return null;
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.MartyrStats.EnemiesDamagedByAoeOnHitEffect);
		}
	}
#endif
}
