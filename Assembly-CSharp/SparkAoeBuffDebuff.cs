using System.Collections.Generic;
using UnityEngine;

public class SparkAoeBuffDebuff : Ability
{
	public enum TargetingType
	{
		UseShape,
		UseRadius
	}

	[Header("-- Targeting")]
	public TargetingType m_TargetingType;
	public bool m_penetrateLos;
	[Header("-- Shape")]
	public AbilityAreaShape m_shape = AbilityAreaShape.Five_x_Five_NoCorners;
	[Header("-- Radius")]
	public float m_radius = 6f;
	[Header("-- Damage and Healing")]
	public int m_damageAmount;
	public int m_allyHealAmount = 10;
	[Header("-- Self Heal Per Hit")]
	public int m_baseSelfHeal;
	public int m_selfHealAmountPerHit;
	public bool m_selfHealCountEnemyHit = true;
	public bool m_selfHealCountAllyHit = true;
	[Header("-- Normal Hit Effects")]
	public StandardEffectInfo m_selfHitEffect;
	public StandardEffectInfo m_allyHitEffect;
	public StandardEffectInfo m_enemyHitEffect;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_sequenceOnEnemies;
	public GameObject m_sequenceOnAllies;

	private AbilityMod_SparkAoeBuffDebuff m_abilityMod;
	private StandardEffectInfo m_cachedSelfHitEffect;
	private StandardEffectInfo m_cachedAllyHitEffect;
	private StandardEffectInfo m_cachedEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Spark Aoe Buff Debuff";
		}
		SetupTargeter();
	}

	public float GetTargetingRadius()
	{
		if (m_abilityMod != null)
		{
			return m_abilityMod.m_radiusMod.GetModifiedValue(m_radius);
		}
		return m_radius;
	}

	public AbilityAreaShape GetHitShape()
	{
		return m_shape;
	}

	public bool ShouldIgnoreLos()
	{
		if (m_abilityMod != null)
		{
			return m_abilityMod.m_ignoreLosMod.GetModifiedValue(m_penetrateLos);
		}
		return m_penetrateLos;
	}

	public int GetAllyHeal(AbilityMod_SparkAoeBuffDebuff mod)
	{
		if (mod != null)
		{
			mod.m_allyHealMod.GetModifiedValue(m_allyHealAmount);
		}
		return m_allyHealAmount;
	}

	public int GetBaseSelfHeal(AbilityMod_SparkAoeBuffDebuff mod)
	{
		if (mod != null)
		{
			return mod.m_baseSelfHealMod.GetModifiedValue(m_baseSelfHeal);
		}
		return m_baseSelfHeal;
	}

	public int GetSelfHealPerHit(AbilityMod_SparkAoeBuffDebuff mod)
	{
		if (mod != null)
		{
			return mod.m_selfHealPerHitMod.GetModifiedValue(m_selfHealAmountPerHit);
		}
		return m_selfHealAmountPerHit;
	}

	public bool SelfHealCountAllyHit()
	{
		if (m_abilityMod != null)
		{
			return m_abilityMod.m_selfHealHitCountAlly.GetModifiedValue(m_selfHealCountAllyHit);
		}
		return m_selfHealCountAllyHit;
	}

	public bool SelfHealCountEnemyHit()
	{
		if (m_abilityMod != null)
		{
			return m_abilityMod.m_selfHealHitCountEnemy.GetModifiedValue(m_selfHealCountEnemyHit);
		}
		return m_selfHealCountEnemyHit;
	}

	public int GetShieldOnSelfPerAllyHit()
	{
		if (m_abilityMod != null)
		{
			return m_abilityMod.m_shieldOnSelfPerAllyHitMod.GetModifiedValue(0);
		}
		return 0;
	}

	public int GetShieldOnSelfDuration()
	{
		if (m_abilityMod != null)
		{
			return m_abilityMod.m_shieldOnSelfDuration;
		}
		return 1;
	}

	public bool IncludeCaster()
	{
		return GetSelfHitEffect().m_applyEffect
			|| GetSelfHealPerHit(m_abilityMod) > 0
			|| GetBaseSelfHeal(m_abilityMod) > 0
			|| GetShieldOnSelfPerAllyHit() > 0;
	}

	public bool IncludeAllies()
	{
		return GetAllyHitEffect().m_applyEffect
			|| GetAllyHeal(m_abilityMod) > 0;
	}

	public bool IncludeEnemies()
	{
		return GetEnemyHitEffect().m_applyEffect
			|| m_damageAmount > 0;
	}

	public StandardEffectInfo GetSelfHitEffect()
	{
		if (m_cachedSelfHitEffect != null)
		{
			return m_cachedSelfHitEffect;
		}
		return m_selfHitEffect;
	}

	public StandardEffectInfo GetAllyHitEffect()
	{
		if (m_cachedAllyHitEffect != null)
		{
			return m_cachedAllyHitEffect;
		}
		return m_allyHitEffect;
	}

	public StandardEffectInfo GetEnemyHitEffect()
	{
		if (m_cachedEnemyHitEffect != null)
		{
			return m_cachedEnemyHitEffect;
		}
		return m_enemyHitEffect;
	}

	private void SetCachedFields()
	{
		m_cachedSelfHitEffect = m_abilityMod != null
			? m_abilityMod.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect)
			: m_selfHitEffect;
		m_cachedAllyHitEffect = m_abilityMod != null
			? m_abilityMod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect)
			: m_allyHitEffect;
		m_cachedEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect)
			: m_enemyHitEffect;
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		if (m_TargetingType == TargetingType.UseShape)
		{
			AbilityUtil_Targeter.AffectsActor affectsCaster = IncludeCaster()
				? AbilityUtil_Targeter.AffectsActor.Possible
				: AbilityUtil_Targeter.AffectsActor.Never;
			Targeter = new AbilityUtil_Targeter_Shape(this, GetHitShape(), ShouldIgnoreLos(), AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, IncludeEnemies(), IncludeAllies(), affectsCaster);
		}
		else
		{
			Targeter = new AbilityUtil_Targeter_AoE_Smooth(this, GetTargetingRadius(), ShouldIgnoreLos(), IncludeEnemies(), IncludeAllies());
		}
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_SparkAoeBuffDebuff mod = modAsBase as AbilityMod_SparkAoeBuffDebuff;
		AddTokenInt(tokens, "Heal_OnAlly", "heal on ally", GetAllyHeal(mod));
		AddTokenInt(tokens, "Heal_OnSelfBase", "heal on self, base amount", GetBaseSelfHeal(mod));
		AddTokenInt(tokens, "Heal_OnSelfPerHit", "heal on self, per hit", GetSelfHealPerHit(mod));
		AbilityMod.AddToken_EffectInfo(tokens, mod != null ? mod.m_allyHitEffectMod.GetModifiedValue(m_allyHitEffect) : m_allyHitEffect, "EffectOnAlly", m_allyHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, mod != null ? mod.m_selfHitEffectMod.GetModifiedValue(m_selfHitEffect) : m_selfHitEffect, "EffectOnSelf", m_selfHitEffect);
		AbilityMod.AddToken_EffectInfo(tokens, mod != null ? mod.m_enemyHitEffectMod.GetModifiedValue(m_enemyHitEffect) : m_enemyHitEffect, "EffectOnEnemy", m_enemyHitEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetSelfHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		GetAllyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		GetEnemyHitEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetBaseSelfHeal(m_abilityMod) + GetSelfHealPerHit(m_abilityMod));
		AbilityTooltipHelper.ReportAbsorb(ref numbers, AbilityTooltipSubject.Self, GetShieldOnSelfPerAllyHit());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetAllyHeal(m_abilityMod));
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, m_damageAmount);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		if (GetSelfHealPerHit(m_abilityMod) <= 0 && GetBaseSelfHeal(m_abilityMod) <= 0 && GetShieldOnSelfPerAllyHit() <= 0)
		{
			return null;
		}
		Dictionary<AbilityTooltipSymbol, int> result = new Dictionary<AbilityTooltipSymbol, int>();
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null && tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
		{
			List<ActorData> primaryTargets = Targeter.GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Primary);
			int enemyHits = 0;
			int allyHits = 0;
			for (int i = 0; i < primaryTargets.Count; i++)
			{
				if (primaryTargets[i].GetTeam() != targetActor.GetTeam())
				{
					enemyHits++;
				}
				else if (primaryTargets[i] != targetActor)
				{
					allyHits++;
				}
			}
			result[AbilityTooltipSymbol.Healing] = CalcSelfHealAmountFromHits(allyHits, enemyHits);
			if (GetShieldOnSelfPerAllyHit() > 0)
			{
				StandardEffectInfo selfHitEffect = GetSelfHitEffect();
				int absorb = selfHitEffect.m_applyEffect && selfHitEffect.m_effectData.m_absorbAmount > 0
					? selfHitEffect.m_effectData.m_absorbAmount
					: 0;
				result[AbilityTooltipSymbol.Absorb] = absorb + allyHits * GetShieldOnSelfPerAllyHit();
			}
		}
		return result;
	}

	private int CalcSelfHealAmountFromHits(int allyHits, int enemyHits)
	{
		if (GetSelfHealPerHit(m_abilityMod) > 0
			|| GetBaseSelfHeal(m_abilityMod) > 0)
		{
			int hits = 0;
			if (SelfHealCountAllyHit())
			{
				hits += allyHits;
			}
			if (SelfHealCountEnemyHit())
			{
				hits += enemyHits;
			}
			return GetBaseSelfHeal(m_abilityMod) + hits * GetSelfHealPerHit(m_abilityMod);
		}
		return 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_SparkAoeBuffDebuff))
		{
			m_abilityMod = (abilityMod as AbilityMod_SparkAoeBuffDebuff);
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
