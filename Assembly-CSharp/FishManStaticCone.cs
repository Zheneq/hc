using System.Collections.Generic;
using UnityEngine;

public class FishManStaticCone : Ability
{
	[Header("-- Cone Data")]
	public float m_coneWidthAngle = 60f;
	public float m_coneWidthAngleMin = 5f;
	public float m_coneLength = 5f;
	public float m_coneBackwardOffset;
	public bool m_penetrateLineOfSight;
	public int m_maxTargets;
	[Header("-- (for stretch cone only)")]
	public bool m_useDiscreteAngleChange;
	public float m_stretchInterpMinDist = 2.5f;
	public float m_stretchInterpRange = 4f;
	[Header("-- On Hit Target")]
	public int m_damageToEnemies;
	public int m_damageToEnemiesMax;
	public StandardEffectInfo m_effectToEnemies;
	[Space(10f)]
	public int m_healingToAllies = 15;
	public int m_healingToAlliesMax = 25;
	public StandardEffectInfo m_effectToAllies;
	public int m_extraAllyHealForSingleHit;
	public StandardEffectInfo m_extraEffectOnClosestAlly;
	[Header("-- Self-Healing")]
	public int m_healToCasterOnCast;
	public int m_healToCasterPerEnemyHit;
	public int m_healToCasterPerAllyHit;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_FishManStaticCone m_abilityMod;
	private FishMan_SyncComponent m_syncComp;
	private FishManCone m_damageConeAbility;
	private AreaEffectUtils.StretchConeStyle m_stretchConeStyle;
	private StandardEffectInfo m_cachedEffectToEnemies;
	private StandardEffectInfo m_cachedEffectToAllies;
	private StandardEffectInfo m_cachedExtraEffectOnClosestAlly;

	private void Start()
	{
		Setup();
	}

	private void Setup()
	{
		SetCachedFields();
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<FishMan_SyncComponent>();
		}
		if (m_damageConeAbility == null)
		{
			AbilityData abilityData = GetComponent<AbilityData>();
			if (abilityData != null)
			{
				m_damageConeAbility = abilityData.GetAbilityOfType(typeof(FishManCone)) as FishManCone;
			}
		}

		Targeter = new AbilityUtil_Targeter_StretchCone(
			this,
			GetConeLength(),
			GetConeLength(),
			GetConeWidthAngleMin(),
			GetConeWidthAngle(),
			m_stretchConeStyle,
			GetConeBackwardOffset(),
			PenetrateLineOfSight())
		{
			m_includeEnemies = AffectsEnemies(),
			m_includeAllies = AffectsAllies(),
			m_includeCaster = AffectsCaster(),
			m_interpMinDistOverride = m_stretchInterpMinDist,
			m_interpRangeOverride = m_stretchInterpRange,
			m_discreteWidthAngleChange = m_useDiscreteAngleChange,
			m_numDiscreteWidthChanges = GetMaxHealingToAllies() - GetHealingToAllies()
		};
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetConeLength();
	}

	private bool AffectsEnemies()
	{
		return GetDamageToEnemies() > 0
		       || GetEffectToEnemies().m_applyEffect;
	}

	private bool AffectsAllies()
	{
		return GetHealingToAllies() > 0
		       || GetEffectToAllies().m_applyEffect;
	}

	private bool AffectsCaster()
	{
		return GetHealToCasterOnCast() > 0
		       || GetHealToCasterPerAllyHit() > 0
		       || GetHealToCasterPerEnemyHit() > 0;
	}

	private void SetCachedFields()
	{
		m_cachedEffectToEnemies = m_abilityMod != null
			? m_abilityMod.m_effectToEnemiesMod.GetModifiedValue(m_effectToEnemies)
			: m_effectToEnemies;
		m_cachedEffectToAllies = m_abilityMod != null
			? m_abilityMod.m_effectToAlliesMod.GetModifiedValue(m_effectToAllies)
			: m_effectToAllies;
		m_cachedExtraEffectOnClosestAlly = m_abilityMod != null
			? m_abilityMod.m_extraEffectOnClosestAllyMod.GetModifiedValue(m_extraEffectOnClosestAlly)
			: m_extraEffectOnClosestAlly;
	}

	public float GetConeWidthAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle)
			: m_coneWidthAngle;
	}

	public float GetConeWidthAngleMin()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneWidthAngleMinMod.GetModifiedValue(m_coneWidthAngleMin)
			: m_coneWidthAngleMin;
	}

	public float GetConeLength()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength)
			: m_coneLength;
	}

	public float GetConeBackwardOffset()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset)
			: m_coneBackwardOffset;
	}

	public bool PenetrateLineOfSight()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight)
			: m_penetrateLineOfSight;
	}

	public int GetMaxTargets()
	{
		return m_abilityMod != null
			? m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets;
	}

	public int GetDamageToEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageToEnemiesMod.GetModifiedValue(m_damageToEnemies)
			: m_damageToEnemies;
	}

	public int GetMaxDamageToEnemies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageToEnemiesMaxMod.GetModifiedValue(m_damageToEnemiesMax)
			: m_damageToEnemiesMax;
	}

	public StandardEffectInfo GetEffectToEnemies()
	{
		return m_cachedEffectToEnemies ?? m_effectToEnemies;
	}

	public int GetHealingToAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healingToAlliesMod.GetModifiedValue(m_healingToAllies)
			: m_healingToAllies;
	}

	public int GetMaxHealingToAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healingToAlliesMaxMod.GetModifiedValue(m_healingToAlliesMax)
			: m_healingToAlliesMax;
	}

	public StandardEffectInfo GetEffectToAllies()
	{
		return m_cachedEffectToAllies ?? m_effectToAllies;
	}

	public int GetExtraAllyHealForSingleHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraAllyHealForSingleHitMod.GetModifiedValue(m_extraAllyHealForSingleHit)
			: m_extraAllyHealForSingleHit;
	}

	public StandardEffectInfo GetExtraEffectOnClosestAlly()
	{
		return m_cachedExtraEffectOnClosestAlly ?? m_extraEffectOnClosestAlly;
	}

	public int GetHealToCasterOnCast()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healToCasterOnCastMod.GetModifiedValue(m_healToCasterOnCast)
			: m_healToCasterOnCast;
	}

	public int GetHealToCasterPerEnemyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healToCasterPerEnemyHitMod.GetModifiedValue(m_healToCasterPerEnemyHit)
			: m_healToCasterPerEnemyHit;
	}

	public int GetHealToCasterPerAllyHit()
	{
		return m_abilityMod != null
			? m_abilityMod.m_healToCasterPerAllyHitMod.GetModifiedValue(m_healToCasterPerAllyHit)
			: m_healToCasterPerAllyHit;
	}

	public int GetExtraAllyHealFromBasicAttack()
	{
		return m_damageConeAbility != null
		       && m_syncComp != null
		       && m_syncComp.m_lastBasicAttackEnemyHitCount > 0
		       && m_damageConeAbility.GetExtraHealPerEnemyHitForNextHealCone() > 0
			? m_syncComp.m_lastBasicAttackEnemyHitCount * m_damageConeAbility.GetExtraHealPerEnemyHitForNextHealCone()
			: 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_FishManStaticCone abilityMod_FishManStaticCone = modAsBase as AbilityMod_FishManStaticCone;
		AddTokenInt(tokens, "DamageToEnemies", string.Empty, abilityMod_FishManStaticCone != null
			? abilityMod_FishManStaticCone.m_damageToEnemiesMod.GetModifiedValue(m_damageToEnemies)
			: m_damageToEnemies);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManStaticCone != null
			? abilityMod_FishManStaticCone.m_effectToEnemiesMod.GetModifiedValue(m_effectToEnemies)
			: m_effectToEnemies, "EffectToEnemies", m_effectToEnemies);
		AddTokenInt(tokens, "HealingToAllies", string.Empty, abilityMod_FishManStaticCone != null
			? abilityMod_FishManStaticCone.m_healingToAlliesMod.GetModifiedValue(m_healingToAllies)
			: m_healingToAllies);
		AddTokenInt(tokens, "HealingToAlliesMax", string.Empty, abilityMod_FishManStaticCone != null
			? abilityMod_FishManStaticCone.m_healingToAlliesMaxMod.GetModifiedValue(m_healingToAlliesMax)
			: m_healingToAlliesMax);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManStaticCone != null
			? abilityMod_FishManStaticCone.m_effectToAlliesMod.GetModifiedValue(m_effectToAllies)
			: m_effectToAllies, "EffectToAllies", m_effectToAllies);
		AddTokenInt(tokens, "ExtraAllyHealForSingleHit", string.Empty, abilityMod_FishManStaticCone != null
			? abilityMod_FishManStaticCone.m_extraAllyHealForSingleHitMod.GetModifiedValue(m_extraAllyHealForSingleHit)
			: m_extraAllyHealForSingleHit);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_FishManStaticCone != null
			? abilityMod_FishManStaticCone.m_extraEffectOnClosestAllyMod.GetModifiedValue(m_extraEffectOnClosestAlly)
			: m_extraEffectOnClosestAlly, "ExtraEffectOnClosestAlly", m_extraEffectOnClosestAlly);
		AddTokenInt(tokens, "MaxTargets", string.Empty, abilityMod_FishManStaticCone != null
			? abilityMod_FishManStaticCone.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
			: m_maxTargets);
		AddTokenInt(tokens, "HealToCasterOnCast", string.Empty, abilityMod_FishManStaticCone != null
			? abilityMod_FishManStaticCone.m_healToCasterOnCastMod.GetModifiedValue(m_healToCasterOnCast)
			: m_healToCasterOnCast);
		AddTokenInt(tokens, "HealToCasterPerEnemyHit", string.Empty, abilityMod_FishManStaticCone != null
			? abilityMod_FishManStaticCone.m_healToCasterPerEnemyHitMod.GetModifiedValue(m_healToCasterPerEnemyHit)
			: m_healToCasterPerEnemyHit);
		AddTokenInt(tokens, "HealToCasterPerAllyHit", string.Empty, abilityMod_FishManStaticCone != null
			? abilityMod_FishManStaticCone.m_healToCasterPerAllyHitMod.GetModifiedValue(m_healToCasterPerAllyHit)
			: m_healToCasterPerAllyHit);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetDamageToEnemies());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Ally, GetHealingToAllies());
		AbilityTooltipHelper.ReportHealing(ref numbers, AbilityTooltipSubject.Self, GetHealToCasterOnCast() + GetHealToCasterPerEnemyHit() + GetHealToCasterPerAllyHit());
		GetEffectToAllies().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Ally);
		GetEffectToEnemies().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy);
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeters[currentTargeterIndex].GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes == null)
		{
			return null;
		}
		AbilityUtil_Targeter_StretchCone targeter = Targeter as AbilityUtil_Targeter_StretchCone;
		Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
		if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Self))
		{
			int enemyNum = Targeters[currentTargeterIndex].GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
			int allyNum = Targeters[currentTargeterIndex].GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
			int healing = GetHealToCasterOnCast() + GetHealToCasterPerEnemyHit() * enemyNum + GetHealToCasterPerAllyHit() * allyNum;
			dictionary[AbilityTooltipSymbol.Healing] = Mathf.RoundToInt(healing);
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
		{
			dictionary[AbilityTooltipSymbol.Damage] = GetDamageForSweepAngle(targeter.LastConeAngle);
		}
		else if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Ally))
		{
			int allyNum = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Ally);
			int healing = GetHealingForSweepAngle(targeter.LastConeAngle);
			healing += GetExtraAllyHealFromBasicAttack();
			if (allyNum == 1)
			{
				healing += GetExtraAllyHealForSingleHit();
			}
			dictionary[AbilityTooltipSymbol.Healing] = healing;
		}
		return dictionary;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_FishManStaticCone))
		{
			m_abilityMod = abilityMod as AbilityMod_FishManStaticCone;
			Setup();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
	}

	private int GetDamageForSweepAngle(float sweepAngle)
	{
		float damageRange = GetMaxDamageToEnemies() - GetDamageToEnemies();
		float angleRange = GetConeWidthAngle() - GetConeWidthAngleMin();
		float share = angleRange > 0f
			? 1f - (sweepAngle - GetConeWidthAngleMin()) / angleRange
			: 1f;
		share = Mathf.Clamp(share, 0f, 1f);
		return GetDamageToEnemies() + Mathf.RoundToInt(damageRange * share);
	}

	private int GetHealingForSweepAngle(float sweepAngle)
	{
		float healingRange = GetMaxHealingToAllies() - GetHealingToAllies();
		float angleRange = GetConeWidthAngle() - GetConeWidthAngleMin();
		float share = angleRange > 0f
			? 1f - (sweepAngle - GetConeWidthAngleMin()) / angleRange
			: 1f;
		share = Mathf.Clamp(share, 0f, 1f);
		return GetHealingToAllies() + Mathf.RoundToInt(healingRange * share);
	}
}
