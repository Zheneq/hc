using System;
using System.Collections.Generic;
using UnityEngine;

public class ClericBasicAttack : Ability
{
	[Header("-- Targeting")]
	public bool m_penetrateLineOfSight;

	public float m_coneAngle = 180f;

	public float m_coneLengthInner = 1.5f;

	public float m_coneLength = 2.5f;

	public float m_coneBackwardOffset;

	public int m_maxTargets = 1;

	[Header("-- On Hit Damage/Effect")]
	public int m_damageAmountInner = 0x1C;

	public int m_damageAmount = 0x14;

	public StandardEffectInfo m_targetHitEffectInner;

	public StandardEffectInfo m_targetHitEffect;

	public AbilityModCooldownReduction m_cooldownReduction;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private Cleric_SyncComponent m_syncComp;

	private AbilityMod_ClericBasicAttack m_abilityMod;

	private StandardEffectInfo m_cachedTargetHitEffectInner;

	private StandardEffectInfo m_cachedTargetHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Cleric Bash";
		}
		this.m_syncComp = base.GetComponent<Cleric_SyncComponent>();
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		float coneAngle = this.GetConeAngle();
		base.Targeter = new AbilityUtil_Targeter_MultipleCones(this, new List<AbilityUtil_Targeter_MultipleCones.ConeDimensions>
		{
			new AbilityUtil_Targeter_MultipleCones.ConeDimensions(coneAngle, this.GetConeLengthInner()),
			new AbilityUtil_Targeter_MultipleCones.ConeDimensions(coneAngle, this.GetConeLength())
		}, this.m_coneBackwardOffset, this.PenetrateLineOfSight(), true, true, false, false);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ClericBasicAttack))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ClericBasicAttack);
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
		StandardEffectInfo cachedTargetHitEffectInner;
		if (this.m_abilityMod)
		{
			cachedTargetHitEffectInner = this.m_abilityMod.m_targetHitEffectInnerMod.GetModifiedValue(this.m_targetHitEffectInner);
		}
		else
		{
			cachedTargetHitEffectInner = this.m_targetHitEffectInner;
		}
		this.m_cachedTargetHitEffectInner = cachedTargetHitEffectInner;
		StandardEffectInfo cachedTargetHitEffect;
		if (this.m_abilityMod)
		{
			cachedTargetHitEffect = this.m_abilityMod.m_targetHitEffectMod.GetModifiedValue(this.m_targetHitEffect);
		}
		else
		{
			cachedTargetHitEffect = this.m_targetHitEffect;
		}
		this.m_cachedTargetHitEffect = cachedTargetHitEffect;
	}

	public bool PenetrateLineOfSight()
	{
		bool result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(this.m_penetrateLineOfSight);
		}
		else
		{
			result = this.m_penetrateLineOfSight;
		}
		return result;
	}

	public float GetConeAngle()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_coneAngleMod.GetModifiedValue(this.m_coneAngle);
		}
		else
		{
			result = this.m_coneAngle;
		}
		return result;
	}

	public float GetConeLengthInner()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_coneLengthInnerMod.GetModifiedValue(this.m_coneLengthInner);
		}
		else
		{
			result = this.m_coneLengthInner;
		}
		return result;
	}

	public float GetConeLength()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_coneLengthMod.GetModifiedValue(this.m_coneLength);
		}
		else
		{
			result = this.m_coneLength;
		}
		return result;
	}

	public float GetConeBackwardOffset()
	{
		float result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(this.m_coneBackwardOffset);
		}
		else
		{
			result = this.m_coneBackwardOffset;
		}
		return result;
	}

	public int GetMaxTargets()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_maxTargetsMod.GetModifiedValue(this.m_maxTargets);
		}
		else
		{
			result = this.m_maxTargets;
		}
		return result;
	}

	public int GetDamageAmountInner()
	{
		return (!this.m_abilityMod) ? this.m_damageAmountInner : this.m_abilityMod.m_damageAmountInnerMod.GetModifiedValue(this.m_damageAmountInner);
	}

	public int GetDamageAmount()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_damageAmountMod.GetModifiedValue(this.m_damageAmount);
		}
		else
		{
			result = this.m_damageAmount;
		}
		return result;
	}

	public StandardEffectInfo GetTargetHitEffectInner()
	{
		return (this.m_cachedTargetHitEffectInner == null) ? this.m_targetHitEffectInner : this.m_cachedTargetHitEffectInner;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		return (this.m_cachedTargetHitEffect == null) ? this.m_targetHitEffect : this.m_cachedTargetHitEffect;
	}

	public int GetExtraDamageToTargetsWhoEvaded()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraDamageToTargetsWhoEvaded.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	public AbilityModCooldownReduction GetCooldownReduction()
	{
		if (this.m_abilityMod)
		{
			if (this.m_abilityMod.m_useCooldownReductionOverride)
			{
				return this.m_abilityMod.m_cooldownReductionOverrideMod;
			}
		}
		return this.m_cooldownReduction;
	}

	public int GetHitsToIgnoreForCooldownReductionMultiplier()
	{
		if (this.m_abilityMod)
		{
			if (this.m_abilityMod.m_useCooldownReductionOverride)
			{
				return this.m_abilityMod.m_hitsToIgnoreForCooldownReductionMultiplier.GetModifiedValue(0);
			}
		}
		return 0;
	}

	public int GetExtraTechPointGainInAreaBuff()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_extraTechPointGainInAreaBuff.GetModifiedValue(0);
		}
		else
		{
			result = 0;
		}
		return result;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
		base.AddTokenInt(tokens, "DamageAmountInner", string.Empty, this.m_damageAmountInner, false);
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, this.m_damageAmount, false);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_targetHitEffectInner, "TargetHitEffectInner", this.m_targetHitEffectInner, true);
		AbilityMod.AddToken_EffectInfo(tokens, this.m_targetHitEffect, "TargetHitEffect", this.m_targetHitEffect, true);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Near, this.GetDamageAmountInner()),
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Far, this.GetDamageAmount())
		};
	}

	private bool InsideNearRadius(ActorData targetActor, Vector3 damageOrigin)
	{
		float num = this.GetConeLengthInner() * Board.Get().squareSize;
		Vector3 vector = targetActor.GetTravelBoardSquareWorldPosition() - damageOrigin;
		vector.y = 0f;
		float num2 = vector.magnitude;
		if (GameWideData.Get().UseActorRadiusForCone())
		{
			num2 -= GameWideData.Get().m_actorTargetingRadiusInSquares * Board.Get().squareSize;
		}
		return num2 <= num;
	}

	public override bool DoesTargetActorMatchTooltipSubject(AbilityTooltipSubject subjectType, ActorData targetActor, Vector3 damageOrigin, ActorData targetingActor)
	{
		if (subjectType != AbilityTooltipSubject.Near)
		{
			if (subjectType != AbilityTooltipSubject.Far)
			{
				return base.DoesTargetActorMatchTooltipSubject(subjectType, targetActor, damageOrigin, targetingActor);
			}
		}
		bool result;
		if (this.InsideNearRadius(targetActor, damageOrigin))
		{
			result = (subjectType == AbilityTooltipSubject.Near);
		}
		else
		{
			result = (subjectType == AbilityTooltipSubject.Far);
		}
		return result;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (caster.GetAbilityData().HasQueuedAbilityOfType(typeof(ClericAreaBuff)))
		{
			return this.GetExtraTechPointGainInAreaBuff() * base.Targeter.GetNumActorsInRange();
		}
		return base.GetAdditionalTechPointGainForNameplateItem(caster, currentTargeterIndex);
	}
}
