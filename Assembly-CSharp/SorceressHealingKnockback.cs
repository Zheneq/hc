using System;
using System.Collections.Generic;
using UnityEngine;

public class SorceressHealingKnockback : Ability
{
	[Header("-- On Cast")]
	public int m_onCastHealAmount;

	public int m_onCastAllyEnergyGain;

	[Header("-- On Detonate")]
	public int m_onDetonateDamageAmount;

	public StandardEffectInfo m_onDetonateEnemyEffect;

	public float m_knockbackDistance;

	public bool m_penetrateLoS;

	public AbilityAreaShape m_aoeShape;

	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	public GameObject m_effectSequence;

	public GameObject m_detonateSequence;

	public GameObject m_detonateGameplayHitSequence;

	private AbilityMod_SorceressHealingKnockback m_abilityMod;

	private void Start()
	{
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		AbilityUtil_Targeter.AffectsActor affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
		AbilityUtil_Targeter.AffectsActor affectsBestTarget = AbilityUtil_Targeter.AffectsActor.Always;
		base.Targeter = new AbilityUtil_Targeter_HealingKnockback(this, this.m_aoeShape, this.m_penetrateLoS, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, true, true, affectsCaster, affectsBestTarget, this.GetKnockbackDistance(), this.m_knockbackType);
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		if (this.m_onCastHealAmount > 0)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Primary, this.m_onCastHealAmount));
		}
		if (this.m_onDetonateDamageAmount > 0)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.m_onDetonateDamageAmount));
		}
		return list;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		if (this.m_onCastHealAmount > 0)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Primary, this.m_onCastHealAmount));
		}
		if (this.GetDamageAmount() > 0)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, this.GetDamageAmount()));
		}
		if (this.GetOnCastAllyEnergyGain() > 0)
		{
			AbilityTooltipHelper.ReportEnergy(ref list, AbilityTooltipSubject.Ally, this.GetOnCastAllyEnergyGain());
		}
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				dictionary[AbilityTooltipSymbol.Healing] = this.GetHealAmount(targetActor);
			}
		}
		return dictionary;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return base.CanTargetActorInDecision(caster, currentBestActorTarget, false, true, true, Ability.ValidateCheckPath.Ignore, true, true, false);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SorceressHealingKnockback abilityMod_SorceressHealingKnockback = modAsBase as AbilityMod_SorceressHealingKnockback;
		string name = "OnCastHealAmount_Normal";
		string empty = string.Empty;
		int val;
		if (abilityMod_SorceressHealingKnockback)
		{
			val = abilityMod_SorceressHealingKnockback.m_normalHealingMod.GetModifiedValue(this.m_onCastHealAmount);
		}
		else
		{
			val = this.m_onCastHealAmount;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		string name2 = "OnCastHealAmount_LowHealth";
		string empty2 = string.Empty;
		int val2;
		if (abilityMod_SorceressHealingKnockback)
		{
			val2 = abilityMod_SorceressHealingKnockback.m_lowHealthHealingMod.GetModifiedValue(this.m_onCastHealAmount);
		}
		else
		{
			val2 = this.m_onCastHealAmount;
		}
		base.AddTokenInt(tokens, name2, empty2, val2, false);
		string name3 = "OnCastAllyEnergyGain";
		string empty3 = string.Empty;
		int val3;
		if (abilityMod_SorceressHealingKnockback)
		{
			val3 = abilityMod_SorceressHealingKnockback.m_onCastAllyEnergyGainMod.GetModifiedValue(this.m_onCastAllyEnergyGain);
		}
		else
		{
			val3 = this.m_onCastAllyEnergyGain;
		}
		base.AddTokenInt(tokens, name3, empty3, val3, false);
		string name4 = "OnDetonateDamageAmount";
		string empty4 = string.Empty;
		int val4;
		if (abilityMod_SorceressHealingKnockback)
		{
			val4 = abilityMod_SorceressHealingKnockback.m_damageMod.GetModifiedValue(this.m_onDetonateDamageAmount);
		}
		else
		{
			val4 = this.m_onDetonateDamageAmount;
		}
		base.AddTokenInt(tokens, name4, empty4, val4, false);
		StandardEffectInfo effectInfo;
		if (abilityMod_SorceressHealingKnockback)
		{
			effectInfo = abilityMod_SorceressHealingKnockback.m_enemyHitEffectOverride.GetModifiedValue(this.m_onDetonateEnemyEffect);
		}
		else
		{
			effectInfo = this.m_onDetonateEnemyEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "OnDetonateEnemyEffect", this.m_onDetonateEnemyEffect, true);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SorceressHealingKnockback))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_SorceressHealingKnockback);
			this.SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	private int GetHealAmount(ActorData target)
	{
		int result = this.m_onCastHealAmount;
		if (this.m_abilityMod != null)
		{
			float num = (float)target.HitPoints / (float)target.GetMaxHitPoints();
			if (num < this.m_abilityMod.m_lowHealthThreshold)
			{
				result = this.m_abilityMod.m_lowHealthHealingMod.GetModifiedValue(this.m_onCastHealAmount);
			}
			else
			{
				result = this.m_abilityMod.m_normalHealingMod.GetModifiedValue(this.m_onCastHealAmount);
			}
		}
		return result;
	}

	public int GetOnCastAllyEnergyGain()
	{
		int result;
		if (this.m_abilityMod)
		{
			result = this.m_abilityMod.m_onCastAllyEnergyGainMod.GetModifiedValue(this.m_onCastAllyEnergyGain);
		}
		else
		{
			result = this.m_onCastAllyEnergyGain;
		}
		return result;
	}

	private int GetDamageAmount()
	{
		int result;
		if (this.m_abilityMod == null)
		{
			result = this.m_onDetonateDamageAmount;
		}
		else
		{
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_onDetonateDamageAmount);
		}
		return result;
	}

	private StandardEffectInfo GetOnDetonateEnemyEffect()
	{
		StandardEffectInfo result;
		if (this.m_abilityMod == null)
		{
			result = this.m_onDetonateEnemyEffect;
		}
		else
		{
			result = this.m_abilityMod.m_enemyHitEffectOverride.GetModifiedValue(this.m_onDetonateEnemyEffect);
		}
		return result;
	}

	private float GetKnockbackDistance()
	{
		float result;
		if (this.m_abilityMod == null)
		{
			result = this.m_knockbackDistance;
		}
		else
		{
			result = this.m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(this.m_knockbackDistance);
		}
		return result;
	}
}
