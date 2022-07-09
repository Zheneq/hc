using System.Collections.Generic;
using UnityEngine;

public class BlasterStretchingCone : Ability
{
	public enum DamageChangeMode
	{
		IncreaseFromMin,
		IncreaseFromMax
	}

	[Header("-- Cone Limits")]
	public float m_minLength;
	public float m_maxLength;
	public float m_minAngle;
	public float m_maxAngle;
	public AreaEffectUtils.StretchConeStyle m_stretchStyle = AreaEffectUtils.StretchConeStyle.DistanceSquared;
	public float m_coneBackwardOffset;
	public bool m_penetrateLineOfSight;
	[Header("-- On Hit")]
	public int m_damageAmountNormal;
	public int m_extraDamageForSingleHit;
	public bool m_removeOverchargeEffectOnCast;
	[Header("-- Damage scaling by distance from enemy")]
	public float m_extraDamagePerSquareDistanceFromEnemy;
	[Header("-- Damage Change by Angle and distance")]
	public DamageChangeMode m_angleDamageChangeMode;
	public int m_anglesPerDamageChange;
	public DamageChangeMode m_distDamageChangeMode = DamageChangeMode.IncreaseFromMax;
	public float m_distPerDamageChange;
	public int m_maxDamageChange;
	[Header("-- Effects On Hit")]
	public StandardEffectInfo m_normalEnemyEffect;
	public StandardEffectInfo m_overchargedEnemyEffect;
	public StandardEffectInfo m_singleEnemyHitEffect;
	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;
	public GameObject m_overchargedCastSequencePrefab;

	private AbilityMod_BlasterStretchingCone m_abilityMod;
	private BlasterOvercharge m_overchargeAbility;
	private BlasterDashAndBlast m_dashAndBlastAbility;
	private Blaster_SyncComponent m_syncComp;
	private StandardEffectInfo m_cachedNormalEnemyEffect;
	private StandardEffectInfo m_cachedOverchargedEnemyEffect;
	private StandardEffectInfo m_cachedSingleEnemyHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Stretching Cone";
		}
		Setup();
	}

	private void Setup()
	{
		m_syncComp = GetComponent<Blaster_SyncComponent>();
		AbilityData component = GetComponent<AbilityData>();
		if (component != null)
		{
			m_overchargeAbility = component.GetAbilityOfType(typeof(BlasterOvercharge)) as BlasterOvercharge;
			m_dashAndBlastAbility = component.GetAbilityOfType(typeof(BlasterDashAndBlast)) as BlasterDashAndBlast;
		}
		SetCachedFields();
		Targeter = new AbilityUtil_Targeter_StretchCone(this, GetMinLength(), GetMaxLength(), GetMinAngle(), GetMaxAngle(), m_stretchStyle, GetConeBackwardOffset(), PenetrateLineOfSight());
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetMaxLength();
	}

	private void SetCachedFields()
	{
		m_cachedNormalEnemyEffect = m_abilityMod != null
			? m_abilityMod.m_normalEnemyEffectMod.GetModifiedValue(m_normalEnemyEffect)
			: m_normalEnemyEffect;
		m_cachedOverchargedEnemyEffect = m_abilityMod != null
			? m_abilityMod.m_overchargedEnemyEffectMod.GetModifiedValue(m_overchargedEnemyEffect)
			: m_overchargedEnemyEffect;
		m_cachedSingleEnemyHitEffect = m_abilityMod != null
			? m_abilityMod.m_singleEnemyHitEffectMod.GetModifiedValue(m_singleEnemyHitEffect)
			: m_singleEnemyHitEffect;
	}

	public float GetMinLength()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_minLengthMod.GetModifiedValue(m_minLength) 
			: m_minLength;
	}

	public float GetMaxLength()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_maxLengthMod.GetModifiedValue(m_maxLength) 
			: m_maxLength;
	}

	public float GetMinAngle()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_minAngleMod.GetModifiedValue(m_minAngle) 
			: m_minAngle;
	}

	public float GetMaxAngle()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_maxAngleMod.GetModifiedValue(m_maxAngle) 
			: m_maxAngle;
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

	public int GetDamageAmountNormal()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_damageAmountNormalMod.GetModifiedValue(m_damageAmountNormal)
			: m_damageAmountNormal;
	}

	public int GetExtraDamageForSingleHit()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit) 
			: m_extraDamageForSingleHit;
	}

	public float GetExtraDamagePerSquareDistanceFromEnemy()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_extraDamagePerSquareDistanceFromEnemyMod.GetModifiedValue(m_extraDamagePerSquareDistanceFromEnemy) 
			: m_extraDamagePerSquareDistanceFromEnemy;
	}

	public int GetAnglesPerDamageChange()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_anglesPerDamageChangeMod.GetModifiedValue(m_anglesPerDamageChange) 
			: m_anglesPerDamageChange;
	}

	public float GetDistPerDamageChange()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_distPerDamageChangeMod.GetModifiedValue(m_distPerDamageChange) 
			: m_distPerDamageChange;
	}

	public int GetMaxDamageChange()
	{
		return m_abilityMod != null 
			? m_abilityMod.m_maxDamageChangeMod.GetModifiedValue(m_maxDamageChange) 
			: m_maxDamageChange;
	}

	public StandardEffectInfo GetNormalEnemyEffect()
	{
		return m_cachedNormalEnemyEffect ?? m_normalEnemyEffect;
	}

	public StandardEffectInfo GetOverchargedEnemyEffect()
	{
		return m_cachedOverchargedEnemyEffect ?? m_overchargedEnemyEffect;
	}

	public StandardEffectInfo GetSingleEnemyHitEffect()
	{
		return m_cachedSingleEnemyHitEffect ?? m_singleEnemyHitEffect;
	}

	private bool AmOvercharged(ActorData caster)
	{
		if (m_syncComp == null)
		{
			m_syncComp = GetComponent<Blaster_SyncComponent>();
		}
		return m_syncComp.m_overchargeBuffs > 0;
	}

	private int GetMultiStackOverchargeDamage()
	{
		return m_syncComp != null
		       && m_syncComp.m_overchargeBuffs > 1
		       && m_overchargeAbility != null
		       && m_overchargeAbility.GetExtraDamageForMultiCast() > 0
			? m_overchargeAbility.GetExtraDamageForMultiCast()
			: 0;
	}

	public int GetCurrentModdedDamage()
	{
		if (AmOvercharged(ActorData))
		{
			return GetDamageAmountNormal() + m_overchargeAbility.GetExtraDamage() + GetMultiStackOverchargeDamage();
		}
		return GetDamageAmountNormal();
	}

	public int GetExtraDamageFromAngle(float angleNow)
	{
		if (GetAnglesPerDamageChange() > 0)
		{
			int num = 0;
			if (m_angleDamageChangeMode == DamageChangeMode.IncreaseFromMin)
			{
				num = Mathf.Max((int)(angleNow - GetMinAngle()), 0);
			}
			else
			{
				num = Mathf.Max((int)(GetMaxAngle() - angleNow), 0);
			}
			int num2 = num / GetAnglesPerDamageChange();
			if (GetMaxDamageChange() > 0)
			{
				num2 = Mathf.Clamp(num2, 0, GetMaxDamageChange());
			}
			return num2;
		}
		return 0;
	}

	public int GetExtraDamageFromRadius(float radiusInSquares)
	{
		if (GetDistPerDamageChange() > 0.1f)
		{
			float num = 0f;
			if (m_distDamageChangeMode == DamageChangeMode.IncreaseFromMin)
			{
				num = Mathf.Max(radiusInSquares - GetMinLength(), 0f);
			}
			else
			{
				num = Mathf.Max(GetMaxLength() - radiusInSquares, 0f);
			}
			int num2 = Mathf.RoundToInt(num / GetDistPerDamageChange());
			if (GetMaxDamageChange() > 0)
			{
				num2 = Mathf.Clamp(num2, 0, GetMaxDamageChange());
			}
			return num2;
		}
		return 0;
	}

	public int GetExtraDamageForEnemy(ActorData caster, ActorData target)
	{
		if (GetExtraDamagePerSquareDistanceFromEnemy() > 0f)
		{
			float num = VectorUtils.HorizontalPlaneDistInSquares(caster.GetFreePos(), target.GetFreePos()) - 1.4f;
			return Mathf.RoundToInt(GetExtraDamagePerSquareDistanceFromEnemy() * num);
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BlasterStretchingCone abilityMod_BlasterStretchingCone = modAsBase as AbilityMod_BlasterStretchingCone;
		AddTokenInt(tokens, "Damage", string.Empty, abilityMod_BlasterStretchingCone != null
			? abilityMod_BlasterStretchingCone.m_damageAmountNormalMod.GetModifiedValue(m_damageAmountNormal)
			: m_damageAmountNormal);
		AddTokenFloat(tokens, "ExtraDamagePerSquareDistanceFromEnemy", string.Empty, m_extraDamagePerSquareDistanceFromEnemy);
		AddTokenInt(tokens, "ExtraDamageForSingleHit", string.Empty, abilityMod_BlasterStretchingCone != null
			? abilityMod_BlasterStretchingCone.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit)
			: m_extraDamageForSingleHit);
		AddTokenInt(tokens, "AnglesPerDamageChange", string.Empty, abilityMod_BlasterStretchingCone != null
			? abilityMod_BlasterStretchingCone.m_anglesPerDamageChangeMod.GetModifiedValue(m_anglesPerDamageChange)
			: m_anglesPerDamageChange);
		AddTokenInt(tokens, "MaxDamageChange", string.Empty, abilityMod_BlasterStretchingCone != null
			? abilityMod_BlasterStretchingCone.m_maxDamageChangeMod.GetModifiedValue(m_maxDamageChange)
			: m_maxDamageChange);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BlasterStretchingCone != null
			? abilityMod_BlasterStretchingCone.m_normalEnemyEffectMod.GetModifiedValue(m_normalEnemyEffect)
			: m_normalEnemyEffect, "NormalEnemyEffect", m_normalEnemyEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BlasterStretchingCone != null
			? abilityMod_BlasterStretchingCone.m_overchargedEnemyEffectMod.GetModifiedValue(m_overchargedEnemyEffect)
			: m_overchargedEnemyEffect, "OverchargedEnemyEffect", m_overchargedEnemyEffect);
		AbilityMod.AddToken_EffectInfo(tokens, abilityMod_BlasterStretchingCone != null
			? abilityMod_BlasterStretchingCone.m_singleEnemyHitEffectMod.GetModifiedValue(m_singleEnemyHitEffect)
			: m_singleEnemyHitEffect, "SingleEnemyHitEffect", m_singleEnemyHitEffect);
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Enemy, GetCurrentModdedDamage());
		return numbers;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = Targeter as AbilityUtil_Targeter_StretchCone;
			int baseDamage = 0;
			if (abilityUtil_Targeter_StretchCone != null)
			{
				baseDamage += GetExtraDamageFromAngle(abilityUtil_Targeter_StretchCone.LastConeAngle);
				baseDamage += GetExtraDamageFromRadius(abilityUtil_Targeter_StretchCone.LastConeRadiusInSquares);
				baseDamage += GetExtraDamageForEnemy(ActorData, targetActor);
			}
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				int visibleActorsCountByTooltipSubject = Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				int damage = GetCurrentModdedDamage() + baseDamage;
				if (visibleActorsCountByTooltipSubject == 1)
				{
					damage += GetExtraDamageForSingleHit();
				}
				dictionary[AbilityTooltipSymbol.Damage] = damage;
			}
		}
		return dictionary;
	}

	public override bool HasRestrictedFreePosDistance(ActorData aimingActor, int targetIndex, List<AbilityTarget> targetsSoFar, out float min, out float max)
	{
		min = GetMinLength() * Board.Get().squareSize;
		max = GetMaxLength() * Board.Get().squareSize;
		return true;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_BlasterStretchingCone))
		{
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_BlasterStretchingCone;
		Setup();
		if (m_dashAndBlastAbility != null && m_dashAndBlastAbility.m_useConeParamsFromPrimary)
		{
			m_dashAndBlastAbility.OnPrimaryAttackModChange();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
		if (m_dashAndBlastAbility != null && m_dashAndBlastAbility.m_useConeParamsFromPrimary)
		{
			m_dashAndBlastAbility.OnPrimaryAttackModChange();
		}
	}
}
