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
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Never);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (HasTargetableActorsInDecision(caster, true, false, false, ValidateCheckPath.Ignore, m_targetData[0].m_checkLineOfSight, false))
		{
			return base.CustomCanCastValidation(caster);
		}
		return false;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (boardSquareSafe != null)
		{
			if (boardSquareSafe.OccupantActor != null && CanTargetActorInDecision(caster, boardSquareSafe.OccupantActor, true, false, false, ValidateCheckPath.Ignore, true, false))
			{
				return true;
			}
		}
		return false;
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
			m_abilityMod = (abilityMod as AbilityMod_ArcherHealingDebuffArrow);
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
		StandardEffectInfo cachedLaserHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedLaserHitEffect = m_abilityMod.m_laserHitEffectMod.GetModifiedValue(m_laserHitEffect);
		}
		else
		{
			cachedLaserHitEffect = m_laserHitEffect;
		}
		m_cachedLaserHitEffect = cachedLaserHitEffect;
		object cachedExtraModEffect;
		if ((bool)m_abilityMod)
		{
			cachedExtraModEffect = m_abilityMod.m_extraHitEffectMod.GetModifiedValue(null);
		}
		else
		{
			cachedExtraModEffect = null;
		}
		m_cachedExtraModEffect = (StandardEffectInfo)cachedExtraModEffect;
		StandardEffectInfo cachedReactionEffect;
		if ((bool)m_abilityMod)
		{
			cachedReactionEffect = m_abilityMod.m_reactionEffectMod.GetModifiedValue(m_reactionEffect);
		}
		else
		{
			cachedReactionEffect = m_reactionEffect;
		}
		m_cachedReactionEffect = cachedReactionEffect;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedLaserHitEffect != null)
		{
			result = m_cachedLaserHitEffect;
		}
		else
		{
			result = m_laserHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetExtraModEffect()
	{
		return m_cachedExtraModEffect;
	}

	public int GetReactionHealing()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_reactionHealingMod.GetModifiedValue(m_reactionHealing);
		}
		else
		{
			result = m_reactionHealing;
		}
		return result;
	}

	public int GetReactionHealingOnSelf()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_reactionHealingOnSelfMod.GetModifiedValue(m_reactionHealingOnSelf);
		}
		else
		{
			result = m_reactionHealingOnSelf;
		}
		return result;
	}

	public int GetLessHealingOnSubsequentReactions()
	{
		return m_abilityMod ? m_abilityMod.m_lessHealingOnSubsequentReactions.GetModifiedValue(0) : 0;
	}

	public int GetHealsPerAlly()
	{
		return (!m_abilityMod) ? m_healsPerAlly : m_abilityMod.m_healsPerAllyMod.GetModifiedValue(m_healsPerAlly);
	}

	public int GetTechPointsPerHeal()
	{
		return (!m_abilityMod) ? m_techPointsPerHeal : m_abilityMod.m_techPointsPerHealMod.GetModifiedValue(m_techPointsPerHeal);
	}

	public StandardEffectInfo GetReactionEffect()
	{
		StandardEffectInfo result;
		if (m_cachedReactionEffect != null)
		{
			result = m_cachedReactionEffect;
		}
		else
		{
			result = m_reactionEffect;
		}
		return result;
	}

	public int GetExtraHealOnShieldGeneratorTargets()
	{
		return m_abilityMod ? m_abilityMod.m_extraHealForShieldGeneratorTargets.GetModifiedValue(0) : 0;
	}

	public AbilityModCooldownReduction GetCooldownReductionIfNoHeals()
	{
		object result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_cooldownReductionIfNoHeals;
		}
		else
		{
			result = null;
		}
		return (AbilityModCooldownReduction)result;
	}

	public int GetExtraHealBelowHealthThreshold()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraHealBelowHealthThresholdMod.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public float GetHealthThresholdForExtraHeal()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_healthThresholdMod.GetModifiedValue(0f);
		}
		else
		{
			result = 0f;
		}
		return result;
	}

	public int GetExtraDamageToThisTargetFromCaster()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageToThisTargetFromCasterMod.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}
}
