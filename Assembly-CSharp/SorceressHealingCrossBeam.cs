using System.Collections.Generic;
using UnityEngine;

public class SorceressHealingCrossBeam : Ability
{
	[Header("-- Enemy Hit Damage and Effect")]
	public int m_damageAmount = 10;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Ally Hit Heal and Effect")]
	public int m_healAmount = 5;
	public StandardEffectInfo m_allyHitEffect;
	[Header("-- Targeting")]
	public float m_width = 1f;
	public float m_distance = 15f;
	public int m_numLasers = 4;
	public bool m_alsoHealSelf = true;
	public bool m_penetrateLineOfSight;
	[Header("-- Sequences -------------------------------------------------")]
	public GameObject m_beamSequencePrefab;
	public GameObject m_centerSequencePrefab;
	public GameObject m_healSequencePrefab;

	private AbilityUtil_Targeter_CrossBeam m_customTargeter;
	private AbilityMod_SorceressHealingCrossBeam m_abilityMod;

	private void Start()
	{
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		m_customTargeter = new AbilityUtil_Targeter_CrossBeam(
			this,
			GetNumLasers(),
			GetLaserRange(),
			GetLaserWidth(), 
			m_penetrateLineOfSight,
			true, 
			m_alsoHealSelf);
		m_customTargeter.SetKnockbackParams(GetKnockbackDistance(), GetKnockbackType(), GetKnockbackThresholdDistance());
		Targeter = m_customTargeter;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetLaserRange();
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Enemy, m_damageAmount));
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Ally, m_healAmount));
		if (m_alsoHealSelf)
		{
			list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, AbilityTooltipSubject.Self, m_healAmount));
		}
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityUtil_Targeter_CrossBeam.HitActorContext> hitActorContext = m_customTargeter.GetHitActorContext();
		int numTargetsInLaser = 0;
		foreach (AbilityUtil_Targeter_CrossBeam.HitActorContext hitActor in hitActorContext)
		{
			if (hitActor.actor == targetActor)
			{
				numTargetsInLaser = hitActor.totalTargetsInLaser;
				break;
			}
		}
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
		{
			dictionary[AbilityTooltipSymbol.Damage] = GetDamageAmount(numTargetsInLaser);
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
		{
			dictionary[AbilityTooltipSymbol.Healing] = GetHealAmount(numTargetsInLaser);
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
		{
			dictionary[AbilityTooltipSymbol.Healing] = GetHealAmount(numTargetsInLaser);
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SorceressHealingCrossBeam abilityMod_SorceressHealingCrossBeam = modAsBase as AbilityMod_SorceressHealingCrossBeam;
		AddTokenInt(tokens, "DamageAmount", string.Empty, abilityMod_SorceressHealingCrossBeam != null
			? abilityMod_SorceressHealingCrossBeam.m_normalDamageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SorceressHealingCrossBeam != null
			? abilityMod_SorceressHealingCrossBeam.m_enemyEffectOverride.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
		AddTokenInt(tokens, "HealAmount", string.Empty, abilityMod_SorceressHealingCrossBeam != null
			? abilityMod_SorceressHealingCrossBeam.m_normalHealingMod.GetModifiedValue(m_healAmount)
			: m_healAmount);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_SorceressHealingCrossBeam != null
			? abilityMod_SorceressHealingCrossBeam.m_allyEffectOverride.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect, "AllyHitEffect", m_allyHitEffect);
		AddTokenInt(tokens, "NumLasers", string.Empty, abilityMod_SorceressHealingCrossBeam != null
			? abilityMod_SorceressHealingCrossBeam.m_laserNumberMod.GetModifiedValue(m_numLasers)
			: m_numLasers);
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_SorceressHealingCrossBeam))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_SorceressHealingCrossBeam;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	private int GetNumLasers()
	{
		return m_abilityMod != null
			? Mathf.Max(1, m_abilityMod.m_laserNumberMod.GetModifiedValue(m_numLasers))
			: m_numLasers;
	}

	private int GetDamageAmount(int numTargetsInLaser)
	{
		return m_abilityMod != null
			? numTargetsInLaser == 1 && m_abilityMod.m_useSingleTargetHitMods
				? m_abilityMod.m_singleTargetDamageMod.GetModifiedValue(m_damageAmount)
				: m_abilityMod.m_normalDamageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	private int GetHealAmount(int numTargetsInLaser)
	{
		return m_abilityMod != null
			? numTargetsInLaser == 1 && m_abilityMod.m_useSingleTargetHitMods
				? m_abilityMod.m_singleTargetHealingMod.GetModifiedValue(m_healAmount)
				: m_abilityMod.m_normalHealingMod.GetModifiedValue(m_healAmount)
			: m_healAmount;
	}

	private StandardEffectInfo GetEnemyHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_enemyEffectOverride.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
	}

	private StandardEffectInfo GetAllyHitEffect()
	{
		return m_abilityMod != null
			? m_abilityMod.m_allyEffectOverride.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
	}

	private float GetLaserWidth()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserWidthMod.GetModifiedValue(m_width)
			: m_width;
	}

	private float GetLaserRange()
	{
		return m_abilityMod != null
			? m_abilityMod.m_laserRangeMod.GetModifiedValue(m_distance)
			: m_distance;
	}

	private float GetKnockbackDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackDistance
			: 0f;
	}

	private KnockbackType GetKnockbackType()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackType
			: KnockbackType.AwayFromSource;
	}

	private float GetKnockbackThresholdDistance()
	{
		return m_abilityMod != null
			? m_abilityMod.m_knockbackThresholdDistance
			: -1f;
	}
}
