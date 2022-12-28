using System.Collections.Generic;
using UnityEngine;

public class ArcherHealingDebuffArrow : Ability
{
	[Header("-- Hit")]
	public StandardEffectInfo m_laserHitEffect;
	[Header("-- Reaction For Allies Hitting Target")]
	public int m_reactionHealing;
	public int m_reactionHealingOnSelf;
	public int m_healsPerAlly = 1;
	[Tooltip("tech points gained by Archer")]
	public int m_techPointsPerHeal;
	public StandardEffectInfo m_reactionEffect;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_reactionProjectilePrefab;
	
	private AbilityMod_ArcherHealingDebuffArrow m_abilityMod;
	private StandardEffectInfo m_cachedLaserHitEffect;
	private StandardEffectInfo m_cachedExtraModEffect;
	private StandardEffectInfo m_cachedReactionEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Bacta Arrow";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			AbilityAreaShape.SingleSquare,
			false,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			true,
			false,
			AbilityUtil_Targeter.AffectsActor.Never);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return HasTargetableActorsInDecision(
			       caster,
			       true,
			       false,
			       false,
			       ValidateCheckPath.Ignore,
			       m_targetData[0].m_checkLineOfSight,
			       false)
		       && base.CustomCanCastValidation(caster);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		return targetSquare != null
		       && targetSquare.OccupantActor != null
		       && CanTargetActorInDecision(
			       caster, targetSquare.OccupantActor, true, false, false, ValidateCheckPath.Ignore, true, false);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		m_laserHitEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Primary);
		return numbers;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod.AddToken_EffectInfo(tokens, m_laserHitEffect, "LaserHitEffect", m_laserHitEffect);
		AddTokenInt(tokens, "ReactionHealing", string.Empty, m_reactionHealing);
		AddTokenInt(tokens, "ReactionHealingOnSelf", string.Empty, m_reactionHealingOnSelf);
		AddTokenInt(tokens, "TechPointsPerHeal", string.Empty, m_techPointsPerHeal);
		AbilityMod.AddToken_EffectInfo(tokens, m_reactionEffect, "ReactionEffect", m_reactionEffect);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ArcherHealingDebuffArrow))
		{
			m_abilityMod = abilityMod as AbilityMod_ArcherHealingDebuffArrow;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private void SetCachedFields()
	{
		m_cachedLaserHitEffect = m_abilityMod != null
			? m_abilityMod.m_laserHitEffectMod.GetModifiedValue(m_laserHitEffect)
			: m_laserHitEffect;
		m_cachedExtraModEffect = m_abilityMod != null
			? m_abilityMod.m_extraHitEffectMod.GetModifiedValue(null)
			: null;
		m_cachedReactionEffect = m_abilityMod != null
			? m_abilityMod.m_reactionEffectMod.GetModifiedValue(m_reactionEffect)
			: m_reactionEffect;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		return m_cachedLaserHitEffect ?? m_laserHitEffect;
	}

	public StandardEffectInfo GetExtraModEffect()
	{
		return m_cachedExtraModEffect;
	}

	public int GetReactionHealing()
	{
		return m_abilityMod != null
			? m_abilityMod.m_reactionHealingMod.GetModifiedValue(m_reactionHealing)
			: m_reactionHealing;
	}

	public int GetReactionHealingOnSelf()
	{
		return m_abilityMod != null
			? m_abilityMod.m_reactionHealingOnSelfMod.GetModifiedValue(m_reactionHealingOnSelf)
			: m_reactionHealingOnSelf;
	}

	public int GetLessHealingOnSubsequentReactions()
	{
		return m_abilityMod != null
			? m_abilityMod.m_lessHealingOnSubsequentReactions.GetModifiedValue(0)
			: 0;
	}

	public int GetHealsPerAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healsPerAllyMod.GetModifiedValue(m_healsPerAlly)
			: m_healsPerAlly;
	}

	public int GetTechPointsPerHeal()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointsPerHealMod.GetModifiedValue(m_techPointsPerHeal)
			: m_techPointsPerHeal;
	}

	public StandardEffectInfo GetReactionEffect()
	{
		return m_cachedReactionEffect ?? m_reactionEffect;
	}

	public int GetExtraHealOnShieldGeneratorTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraHealForShieldGeneratorTargets.GetModifiedValue(0)
			: 0;
	}

	public AbilityModCooldownReduction GetCooldownReductionIfNoHeals()
	{
		return m_abilityMod != null
			? m_abilityMod.m_cooldownReductionIfNoHeals
			: null;
	}

	public int GetExtraHealBelowHealthThreshold()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraHealBelowHealthThresholdMod.GetModifiedValue(0)
			: 0;
	}

	public float GetHealthThresholdForExtraHeal()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healthThresholdMod.GetModifiedValue(0f)
			: 0f;
	}

	public int GetExtraDamageToThisTargetFromCaster()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageToThisTargetFromCasterMod.GetModifiedValue(0)
			: 0;
	}
}
