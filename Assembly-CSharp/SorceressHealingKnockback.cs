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
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_HealingKnockback(
			this,
			m_aoeShape,
			m_penetrateLoS,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			true, 
			true, 
			AbilityUtil_Targeter.AffectsActor.Possible,
			AbilityUtil_Targeter.AffectsActor.Always,
			GetKnockbackDistance(),
			m_knockbackType);
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return base.GetTargetableRadiusInSquares(caster) - 0.5f;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		if (m_onCastHealAmount > 0)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Primary, m_onCastHealAmount));
		}
		if (m_onDetonateDamageAmount > 0)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_onDetonateDamageAmount));
		}
		return list;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		if (m_onCastHealAmount > 0)
		{
			number.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Primary, m_onCastHealAmount));
		}
		if (GetDamageAmount() > 0)
		{
			number.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, GetDamageAmount()));
		}
		if (GetOnCastAllyEnergyGain() > 0)
		{
			AbilityTooltipHelper.ReportEnergy(ref number, AbilityTooltipSubject.Ally, GetOnCastAllyEnergyGain());
		}
		return number;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				dictionary[AbilityTooltipSymbol.Healing] = GetHealAmount(targetActor);
			}
		}
		return dictionary;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return CanTargetActorInDecision(
			caster,
			currentBestActorTarget,
			false, 
			true, 
			true,
			ValidateCheckPath.Ignore,
			true,
			true);
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SorceressHealingKnockback abilityMod_SorceressHealingKnockback = modAsBase as AbilityMod_SorceressHealingKnockback;
		AddTokenInt(tokens, "OnCastHealAmount_Normal", string.Empty, abilityMod_SorceressHealingKnockback != null
			? abilityMod_SorceressHealingKnockback.m_normalHealingMod.GetModifiedValue(m_onCastHealAmount)
			: m_onCastHealAmount);
		AddTokenInt(tokens, "OnCastHealAmount_LowHealth", string.Empty, abilityMod_SorceressHealingKnockback != null
			? abilityMod_SorceressHealingKnockback.m_lowHealthHealingMod.GetModifiedValue(m_onCastHealAmount)
			: m_onCastHealAmount);
		AddTokenInt(tokens, "OnCastAllyEnergyGain", string.Empty, abilityMod_SorceressHealingKnockback != null
			? abilityMod_SorceressHealingKnockback.m_onCastAllyEnergyGainMod.GetModifiedValue(m_onCastAllyEnergyGain)
			: m_onCastAllyEnergyGain);
		AddTokenInt(tokens, "OnDetonateDamageAmount", string.Empty, abilityMod_SorceressHealingKnockback != null
			? abilityMod_SorceressHealingKnockback.m_damageMod.GetModifiedValue(m_onDetonateDamageAmount)
			: m_onDetonateDamageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SorceressHealingKnockback != null
			? abilityMod_SorceressHealingKnockback.m_enemyHitEffectOverride.GetModifiedValue(m_onDetonateEnemyEffect)
			: m_onDetonateEnemyEffect, "OnDetonateEnemyEffect", m_onDetonateEnemyEffect);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SorceressHealingKnockback))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_SorceressHealingKnockback;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private int GetHealAmount(ActorData target)
	{
		int result = m_onCastHealAmount;
		if (m_abilityMod != null)
		{
			float num = target.HitPoints / (float)target.GetMaxHitPoints();
			if (num < m_abilityMod.m_lowHealthThreshold)
			{
				result = m_abilityMod.m_lowHealthHealingMod.GetModifiedValue(m_onCastHealAmount);
			}
			else
			{
				result = m_abilityMod.m_normalHealingMod.GetModifiedValue(m_onCastHealAmount);
			}
		}
		return result;
	}

	public int GetOnCastAllyEnergyGain()
	{
		return m_abilityMod != null
			? m_abilityMod.m_onCastAllyEnergyGainMod.GetModifiedValue(m_onCastAllyEnergyGain)
			: m_onCastAllyEnergyGain;
	}

	private int GetDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_onDetonateDamageAmount)
			: m_onDetonateDamageAmount;
	}

	private StandardEffectInfo GetOnDetonateEnemyEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectOverride.GetModifiedValue(m_onDetonateEnemyEffect)
			: m_onDetonateEnemyEffect;
	}

	private float GetKnockbackDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance)
			: m_knockbackDistance;
	}
}
