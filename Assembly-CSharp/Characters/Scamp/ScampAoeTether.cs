// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class ScampAoeTether : Ability
{
	[Separator("Targeting")]
	public float m_aoeRadius = 5f;
	public bool m_ignoreLos;
	[Header("-- if > 0, will use this as distance to check whether tether should break")]
	public float m_tetherBreakDistanceOverride = -1f;
	[Separator("Whether to pull towards caster if target is out of range. If true, no longer do movement hits")]
	public bool m_pullToCasterInKnockback;
	public float m_maxKnockbackDist = 10f;
	[Separator("Disable in Shield Down mode?")]
	public bool m_disableIfShieldDown = true;
	[Separator("On Tether Apply")]
	public StandardEffectInfo m_tetherApplyEnemyEffect;
	[Separator("On Tether Break")]
	public int m_tetherBreakDamage = 10;
	public StandardEffectInfo m_tetherBreakEnemyEffecf;
	[Separator("Cdr if not triggered")]
	public int m_cdrIfNoTetherTrigger;
	[Separator("Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_tetherBreakTriggerSequencePrefab;

	private Scamp_SyncComponent m_syncComp;
	private AbilityMod_ScampAoeTether m_abilityMod;
	private StandardEffectInfo m_cachedTetherApplyEnemyEffect;
	private StandardEffectInfo m_cachedTetherBreakEnemyEffecf;
	
#if SERVER
	private Passive_Scamp m_passive; // custom
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "ScampAoeTether";
		}
		Setup();
	}

	private void Setup()
	{
		m_syncComp = GetComponent<Scamp_SyncComponent>();
#if SERVER
		m_passive = GetPassiveOfType(typeof(Passive_Scamp)) as Passive_Scamp; // custom
#endif
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, GetAoeRadius(), IgnoreLos());
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod.AddToken_EffectInfo(tokens, m_tetherApplyEnemyEffect, "TetherApplyEnemyEffect", m_tetherApplyEnemyEffect);
		AddTokenInt(tokens, "TetherBreakDamage", string.Empty, m_tetherBreakDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_tetherBreakEnemyEffecf, "TetherBreakEnemyEffecf", m_tetherBreakEnemyEffecf);
		AddTokenInt(tokens, "CdrIfNoTetherTrigger", string.Empty, m_cdrIfNoTetherTrigger);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return m_syncComp != null && m_syncComp.m_suitWasActiveOnTurnStart
		       || !DisableIfShieldDown();
	}

	private void SetCachedFields()
	{
		m_cachedTetherApplyEnemyEffect = m_abilityMod != null
			? m_abilityMod.m_tetherApplyEnemyEffectMod.GetModifiedValue(m_tetherApplyEnemyEffect)
			: m_tetherApplyEnemyEffect;
		m_cachedTetherBreakEnemyEffecf = m_abilityMod != null
			? m_abilityMod.m_tetherBreakEnemyEffecfMod.GetModifiedValue(m_tetherBreakEnemyEffecf)
			: m_tetherBreakEnemyEffecf;
	}

	public float GetAoeRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_aoeRadiusMod.GetModifiedValue(m_aoeRadius)
			: m_aoeRadius;
	}

	public bool IgnoreLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_ignoreLosMod.GetModifiedValue(m_ignoreLos)
			: m_ignoreLos;
	}

	public float GetTetherBreakDistanceOverride()
	{
		return m_abilityMod != null
			? m_abilityMod.m_tetherBreakDistanceOverrideMod.GetModifiedValue(m_tetherBreakDistanceOverride)
			: m_tetherBreakDistanceOverride;
	}

	public bool PullToCasterInKnockback()
	{
		return m_abilityMod != null
			? m_abilityMod.m_pullToCasterInKnockbackMod.GetModifiedValue(m_pullToCasterInKnockback)
			: m_pullToCasterInKnockback;
	}

	public float GetMaxKnockbackDist()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxKnockbackDistMod.GetModifiedValue(m_maxKnockbackDist)
			: m_maxKnockbackDist;
	}

	public bool DisableIfShieldDown()
	{
		return m_abilityMod != null
			? m_abilityMod.m_disableIfShieldDownMod.GetModifiedValue(m_disableIfShieldDown)
			: m_disableIfShieldDown;
	}

	public StandardEffectInfo GetTetherApplyEnemyEffect()
	{
		return m_cachedTetherApplyEnemyEffect ?? m_tetherApplyEnemyEffect;
	}

	public int GetTetherBreakDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_tetherBreakDamageMod.GetModifiedValue(m_tetherBreakDamage)
			: m_tetherBreakDamage;
	}

	public StandardEffectInfo GetTetherBreakEnemyEffecf()
	{
		return m_cachedTetherBreakEnemyEffecf ?? m_tetherBreakEnemyEffecf;
	}

	public int GetCdrIfNoTetherTrigger()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cdrIfNoTetherTriggerMod.GetModifiedValue(m_cdrIfNoTetherTrigger)
			: m_cdrIfNoTetherTrigger;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>();
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetAoeRadius();
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ScampAoeTether))
		{
			m_abilityMod = abilityMod as AbilityMod_ScampAoeTether;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}
	
#if SERVER
	// custom
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		base.Run(targets, caster, additionalData);

		if (GetCdrIfNoTetherTrigger() > 0)
		{
			m_passive.SetPendingCdrOnTether(GetCdrIfNoTetherTrigger());
		}
	}

	// custom
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			caster.GetCurrentBoardSquare().ToVector3(),
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}
	
	// custom
	public override void GatherAbilityResults(
		List<AbilityTarget> targets,
		ActorData caster,
		ref AbilityResults abilityResults)
	{
		var nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> hitActors = AreaEffectUtils.GetActorsInRadius(
			caster.GetFreePos(),
			GetAoeRadius(),
			IgnoreLos(),
			caster,
			caster.GetOtherTeams(),
			nonActorTargetInfo);
		foreach (ActorData hitActor in hitActors)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, caster.GetFreePos()));
			actorHitResults.AddEffect(new ScampAoeTetherEffect(
				AsEffectSource(),
				hitActor,
				caster,
				GetTetherApplyEnemyEffect().m_effectData,
				GetTetherBreakDistanceOverride(),
				PullToCasterInKnockback(),
				GetMaxKnockbackDist(),
				GetTetherBreakDamage(),
				GetTetherBreakEnemyEffecf(),
				m_tetherBreakTriggerSequencePrefab,
				m_passive,
				GetAoeRadius()));
			abilityResults.StoreActorHit(actorHitResults);
		}
	}
	
	// custom
	public override void OnExecutedActorHit_Effect(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (caster.GetTeam() != target.GetTeam()
		    && results.HasKnockback
		    && !target.GetActorStatus().HasStatus(StatusType.Unstoppable))
		{
			caster.GetFreelancerStats().IncrementValueOfStat(FreelancerStats.ScampStats.TetherNumKnockbacks);
		}
	}
#endif
}
