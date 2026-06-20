// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SenseiBide : Ability
{
	[Header("-- Targeting --")]
	public bool m_targetingIgnoreLos;
	[Separator("Effect on Cast Target", "cyan")]
	public StandardActorEffectData m_onCastTargetEffectData;
	[Header("-- Additional Effect on targeted actor, for shielding, etc")]
	public StandardEffectInfo m_additionalTargetHitEffect;
	[Separator("For Explosion Hits", "cyan")]
	public float m_explosionRadius = 1.5f;
	public bool m_ignoreLos;
	[Header("-- Explosion Hit --")]
	public int m_maxDamage = 50;
	public int m_baseDamage;
	public float m_damageMult = 1f;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Heal portion of absorb remaining")]
	public float m_absorbMultForHeal;
	[Header("-- Damage portion of initial damage, on turns after")]
	public float m_multOnInitialDamageForSubseqHits;
	[Separator("Extra Heal on Heal AoE Ability")]
	public int m_extraHealOnHealAoeIfQueued;
	[Header("-- Animation --")]
	public int m_explosionAnimIndex;
	[Header("-- Sequences --")]
	public GameObject m_castSequencePrefab;
	[Header("    Used by effect when actual explosion happens")]
	public GameObject m_onExplosionSequencePrefab;

	private AbilityMod_SenseiBide m_abilityMod;
	private StandardActorEffectData m_cachedOnCastTargetEffectData;
	private StandardEffectInfo m_cachedAdditionalTargetHitEffect;
	private StandardEffectInfo m_cachedEnemyHitEffect;

#if SERVER
	// added in rogues
	private Passive_Sensei m_passive;
#endif

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "SenseiBide";
		}
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
#if SERVER
		// added in rogues
		m_passive = GetPassiveOfType(typeof(Passive_Sensei)) as Passive_Sensei;
#endif
		
		AbilityUtil_Targeter_AoE_AroundActor abilityUtil_Targeter_AoE_AroundActor = new AbilityUtil_Targeter_AoE_AroundActor(this, GetExplosionRadius(), IgnoreLos());
		abilityUtil_Targeter_AoE_AroundActor.SetAffectedGroups(true, false, false);
		abilityUtil_Targeter_AoE_AroundActor.m_allyOccupantSubject = AbilityTooltipSubject.Tertiary;
		abilityUtil_Targeter_AoE_AroundActor.m_enemyOccupantSubject = AbilityTooltipSubject.Quaternary;
		Targeter = abilityUtil_Targeter_AoE_AroundActor;
		Targeter.SetShowArcToShape(true);
	}

	public override string GetSetupNotesForEditor()
	{
		return "<color=cyan>-- For Art --</color>\nFor Persistent sequence, specify on " +
		       SetupNoteVarName("On Cast Target Effect Data") + "'s sequence field";
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	private void SetCachedFields()
	{
		m_cachedOnCastTargetEffectData = m_abilityMod != null
			? m_abilityMod.m_onCastTargetEffectDataMod.GetModifiedValue(m_onCastTargetEffectData)
			: m_onCastTargetEffectData;
		m_cachedAdditionalTargetHitEffect = m_abilityMod != null
			? m_abilityMod.m_additionalTargetHitEffectMod.GetModifiedValue(m_additionalTargetHitEffect)
			: m_additionalTargetHitEffect;
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
	}

	public bool TargetingIgnoreLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_targetingIgnoreLosMod.GetModifiedValue(m_targetingIgnoreLos)
			: m_targetingIgnoreLos;
	}

	public StandardActorEffectData GetOnCastTargetEffectData()
	{
		return m_cachedOnCastTargetEffectData ?? m_onCastTargetEffectData;
	}

	public StandardEffectInfo GetAdditionalTargetHitEffect()
	{
		return m_cachedAdditionalTargetHitEffect ?? m_additionalTargetHitEffect;
	}

	public float GetExplosionRadius()
	{
		return m_abilityMod != null
			? m_abilityMod.m_explosionRadiusMod.GetModifiedValue(m_explosionRadius)
			: m_explosionRadius;
	}

	public bool IgnoreLos()
	{
		return m_abilityMod != null
			? m_abilityMod.m_ignoreLosMod.GetModifiedValue(m_ignoreLos)
			: m_ignoreLos;
	}

	public int GetMaxDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxDamageMod.GetModifiedValue(m_maxDamage)
			: m_maxDamage;
	}

	public int GetBaseDamage()
	{
		return m_abilityMod != null
			? m_abilityMod.m_baseDamageMod.GetModifiedValue(m_baseDamage)
			: m_baseDamage;
	}

	public float GetDamageMult()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMultMod.GetModifiedValue(m_damageMult)
			: m_damageMult;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		return m_cachedEnemyHitEffect ?? m_enemyHitEffect;
	}

	public float GetAbsorbMultForHeal()
	{
		return m_abilityMod != null
			? m_abilityMod.m_absorbMultForHealMod.GetModifiedValue(m_absorbMultForHeal)
			: m_absorbMultForHeal;
	}

	public float GetMultOnInitialDamageForSubseqHits()
	{
		return m_abilityMod != null
			? m_abilityMod.m_multOnInitialDamageForSubseqHitsMod.GetModifiedValue(m_multOnInitialDamageForSubseqHits)
			: m_multOnInitialDamageForSubseqHits;
	}

	public int GetExtraHealOnHealAoeIfQueued()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraHealOnHealAoeIfQueuedMod.GetModifiedValue(m_extraHealOnHealAoeIfQueued)
			: m_extraHealOnHealAoeIfQueued;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetOnCastTargetEffectData().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Tertiary);
		GetAdditionalTargetHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Tertiary);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		m_onCastTargetEffectData.AddTooltipTokens(tokens, "OnCastTargetEffectData");
		AbilityMod.AddToken_EffectInfo(tokens, m_additionalTargetHitEffect, "AdditionalTargetHitEffect", m_additionalTargetHitEffect);
		AddTokenInt(tokens, "MaxDamage", string.Empty, m_maxDamage);
		AddTokenInt(tokens, "BaseDamage", string.Empty, m_baseDamage);
		AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "ExtraHealOnHealAoeIfQueued", string.Empty, m_extraHealOnHealAoeIfQueued);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		return CanTargetActorInDecision(
			caster,
			target.GetCurrentBestActorTarget(),
			false,
			true,
			true,
			ValidateCheckPath.Ignore,
			TargetingIgnoreLos(),
			true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SenseiBide))
		{
			m_abilityMod = abilityMod as AbilityMod_SenseiBide;
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
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
		List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_castSequencePrefab,
				targets[0].FreePos,
				additionalData.m_abilityResults.HitActorsArray(),
				caster,
				additionalData.m_sequenceSource)
		};
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorData hitActor = GetHitActor(targets, caster);
		if (hitActor != null)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, hitActor.GetFreePos()));
			actorHitResults.AddEffect(new SenseiBideEffect(
				AsEffectSource(),
				caster.GetCurrentBoardSquare(),
				hitActor,
				caster,
				GetOnCastTargetEffectData(),
				m_passive,
				GetExplosionRadius(),
				IgnoreLos(),
				GetMaxDamage(),
				GetBaseDamage(),
				GetDamageMult(),
				GetEnemyHitEffect(),
				GetAbsorbMultForHeal(),
				GetMultOnInitialDamageForSubseqHits(),
				m_onExplosionSequencePrefab));
			actorHitResults.AddEffect(new SenseiBideAbsorbShieldEffect(
				AsEffectSource(),
				hitActor.GetCurrentBoardSquare(),
				hitActor,
				caster,
				GetAdditionalTargetHitEffect().m_effectData,
				m_passive));
			abilityResults.StoreActorHit(actorHitResults);
		}
	}

	// added in rogues
	private ActorData GetHitActor(List<AbilityTarget> targets, ActorData caster)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
		if (targetSquare != null
		    && targetSquare.OccupantActor != null
		    && !targetSquare.OccupantActor.IgnoreForAbilityHits)
		{
			return targetSquare.OccupantActor;
		}
		return null;
	}

	// added in rogues
	public override void OnExecutedActorHit_General(ActorData caster, ActorData target, ActorHitResults results)
	{
		if (results.FinalDamage > 0)
		{
			caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.SenseiStats.DamageDoneByUlt, results.FinalDamage);
		}
	}
#endif
}
