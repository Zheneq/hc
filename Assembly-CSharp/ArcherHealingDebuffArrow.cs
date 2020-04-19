using System;
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
		if (this.m_abilityName == "Base Ability")
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherHealingDebuffArrow.Start()).MethodHandle;
			}
			this.m_abilityName = "Bacta Arrow";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, false, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, false, AbilityUtil_Targeter.AffectsActor.Never, AbilityUtil_Targeter.AffectsActor.Possible);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return base.HasTargetableActorsInDecision(caster, true, false, false, Ability.ValidateCheckPath.Ignore, this.m_targetData[0].m_checkLineOfSight, false, false) && base.CustomCanCastValidation(caster);
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquare = Board.\u000E().\u000E(target.GridPos);
		if (boardSquare != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherHealingDebuffArrow.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			if (boardSquare.OccupantActor != null && base.CanTargetActorInDecision(caster, boardSquare.OccupantActor, true, false, false, Ability.ValidateCheckPath.Ignore, true, false, false))
			{
				return true;
			}
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.m_laserHitEffect.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Primary);
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod.AddToken_EffectInfo(tokens, this.m_laserHitEffect, "LaserHitEffect", this.m_laserHitEffect, true);
		base.AddTokenInt(tokens, "ReactionHealing", string.Empty, this.m_reactionHealing, false);
		base.AddTokenInt(tokens, "ReactionHealingOnSelf", string.Empty, this.m_reactionHealingOnSelf, false);
		base.AddTokenInt(tokens, "TechPointsPerHeal", string.Empty, this.m_techPointsPerHeal, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_reactionEffect, "ReactionEffect", this.m_reactionEffect, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ArcherHealingDebuffArrow))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ArcherHealingDebuffArrow);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	private void SetCachedFields()
	{
		StandardEffectInfo cachedLaserHitEffect;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherHealingDebuffArrow.SetCachedFields()).MethodHandle;
			}
			cachedLaserHitEffect = this.m_abilityMod.m_laserHitEffectMod.GetModifiedValue(this.m_laserHitEffect);
		}
		else
		{
			cachedLaserHitEffect = this.m_laserHitEffect;
		}
		this.m_cachedLaserHitEffect = cachedLaserHitEffect;
		StandardEffectInfo cachedExtraModEffect;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedExtraModEffect = this.m_abilityMod.m_extraHitEffectMod.GetModifiedValue(null);
		}
		else
		{
			cachedExtraModEffect = null;
		}
		this.m_cachedExtraModEffect = cachedExtraModEffect;
		StandardEffectInfo cachedReactionEffect;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			cachedReactionEffect = this.m_abilityMod.m_reactionEffectMod.GetModifiedValue(this.m_reactionEffect);
		}
		else
		{
			cachedReactionEffect = this.m_reactionEffect;
		}
		this.m_cachedReactionEffect = cachedReactionEffect;
	}

	public StandardEffectInfo GetLaserHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedLaserHitEffect != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherHealingDebuffArrow.GetLaserHitEffect()).MethodHandle;
			}
			result = this.m_cachedLaserHitEffect;
		}
		else
		{
			result = this.m_laserHitEffect;
		}
		return result;
	}

	public StandardEffectInfo GetExtraModEffect()
	{
		return this.m_cachedExtraModEffect;
	}

	public int GetReactionHealing()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherHealingDebuffArrow.GetReactionHealing()).MethodHandle;
			}
			result = this.m_abilityMod.m_reactionHealingMod.GetModifiedValue(this.m_reactionHealing);
		}
		else
		{
			result = this.m_reactionHealing;
		}
		return result;
	}

	public int GetReactionHealingOnSelf()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherHealingDebuffArrow.GetReactionHealingOnSelf()).MethodHandle;
			}
			result = this.m_abilityMod.m_reactionHealingOnSelfMod.GetModifiedValue(this.m_reactionHealingOnSelf);
		}
		else
		{
			result = this.m_reactionHealingOnSelf;
		}
		return result;
	}

	public int GetLessHealingOnSubsequentReactions()
	{
		return (!this.m_abilityMod) ? 0 : this.m_abilityMod.m_lessHealingOnSubsequentReactions.GetModifiedValue(0);
	}

	public int GetHealsPerAlly()
	{
		return (!this.m_abilityMod) ? this.m_healsPerAlly : this.m_abilityMod.m_healsPerAllyMod.GetModifiedValue(this.m_healsPerAlly);
	}

	public int GetTechPointsPerHeal()
	{
		return (!this.m_abilityMod) ? this.m_techPointsPerHeal : this.m_abilityMod.m_techPointsPerHealMod.GetModifiedValue(this.m_techPointsPerHeal);
	}

	public StandardEffectInfo GetReactionEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedReactionEffect != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherHealingDebuffArrow.GetReactionEffect()).MethodHandle;
			}
			result = this.m_cachedReactionEffect;
		}
		else
		{
			result = this.m_reactionEffect;
		}
		return result;
	}

	public int GetExtraHealOnShieldGeneratorTargets()
	{
		return (!this.m_abilityMod) ? 0 : this.m_abilityMod.m_extraHealForShieldGeneratorTargets.GetModifiedValue(0);
	}

	public AbilityModCooldownReduction GetCooldownReductionIfNoHeals()
	{
		AbilityModCooldownReduction result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherHealingDebuffArrow.GetCooldownReductionIfNoHeals()).MethodHandle;
			}
			result = this.m_abilityMod.m_cooldownReductionIfNoHeals;
		}
		else
		{
			result = null;
		}
		return result;
	}

	public int GetExtraHealBelowHealthThreshold()
	{
		int result;
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherHealingDebuffArrow.GetExtraHealBelowHealthThreshold()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraHealBelowHealthThresholdMod.GetModifiedValue(0);
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
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherHealingDebuffArrow.GetHealthThresholdForExtraHeal()).MethodHandle;
			}
			result = this.m_abilityMod.m_healthThresholdMod.GetModifiedValue(0f);
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
		if (this.m_abilityMod)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ArcherHealingDebuffArrow.GetExtraDamageToThisTargetFromCaster()).MethodHandle;
			}
			result = this.m_abilityMod.m_extraDamageToThisTargetFromCasterMod.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}
}
