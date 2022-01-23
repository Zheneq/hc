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
			m_overchargeAbility = (component.GetAbilityOfType(typeof(BlasterOvercharge)) as BlasterOvercharge);
			m_dashAndBlastAbility = (component.GetAbilityOfType(typeof(BlasterDashAndBlast)) as BlasterDashAndBlast);
		}
		SetCachedFields();
		base.Targeter = new AbilityUtil_Targeter_StretchCone(this, GetMinLength(), GetMaxLength(), GetMinAngle(), GetMaxAngle(), m_stretchStyle, GetConeBackwardOffset(), PenetrateLineOfSight());
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
		StandardEffectInfo cachedNormalEnemyEffect;
		if ((bool)m_abilityMod)
		{
			cachedNormalEnemyEffect = m_abilityMod.m_normalEnemyEffectMod.GetModifiedValue(m_normalEnemyEffect);
		}
		else
		{
			cachedNormalEnemyEffect = m_normalEnemyEffect;
		}
		m_cachedNormalEnemyEffect = cachedNormalEnemyEffect;
		m_cachedOverchargedEnemyEffect = ((!m_abilityMod) ? m_overchargedEnemyEffect : m_abilityMod.m_overchargedEnemyEffectMod.GetModifiedValue(m_overchargedEnemyEffect));
		StandardEffectInfo cachedSingleEnemyHitEffect;
		if ((bool)m_abilityMod)
		{
			cachedSingleEnemyHitEffect = m_abilityMod.m_singleEnemyHitEffectMod.GetModifiedValue(m_singleEnemyHitEffect);
		}
		else
		{
			cachedSingleEnemyHitEffect = m_singleEnemyHitEffect;
		}
		m_cachedSingleEnemyHitEffect = cachedSingleEnemyHitEffect;
	}

	public float GetMinLength()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_minLengthMod.GetModifiedValue(m_minLength);
		}
		else
		{
			result = m_minLength;
		}
		return result;
	}

	public float GetMaxLength()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxLengthMod.GetModifiedValue(m_maxLength);
		}
		else
		{
			result = m_maxLength;
		}
		return result;
	}

	public float GetMinAngle()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_minAngleMod.GetModifiedValue(m_minAngle);
		}
		else
		{
			result = m_minAngle;
		}
		return result;
	}

	public float GetMaxAngle()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxAngleMod.GetModifiedValue(m_maxAngle);
		}
		else
		{
			result = m_maxAngle;
		}
		return result;
	}

	public float GetConeBackwardOffset()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneBackwardOffsetMod.GetModifiedValue(m_coneBackwardOffset);
		}
		else
		{
			result = m_coneBackwardOffset;
		}
		return result;
	}

	public bool PenetrateLineOfSight()
	{
		bool result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight);
		}
		else
		{
			result = m_penetrateLineOfSight;
		}
		return result;
	}

	public int GetDamageAmountNormal()
	{
		return (!m_abilityMod) ? m_damageAmountNormal : m_abilityMod.m_damageAmountNormalMod.GetModifiedValue(m_damageAmountNormal);
	}

	public int GetExtraDamageForSingleHit()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit);
		}
		else
		{
			result = m_extraDamageForSingleHit;
		}
		return result;
	}

	public float GetExtraDamagePerSquareDistanceFromEnemy()
	{
		float result;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_extraDamagePerSquareDistanceFromEnemyMod.GetModifiedValue(m_extraDamagePerSquareDistanceFromEnemy);
		}
		else
		{
			result = m_extraDamagePerSquareDistanceFromEnemy;
		}
		return result;
	}

	public int GetAnglesPerDamageChange()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_anglesPerDamageChangeMod.GetModifiedValue(m_anglesPerDamageChange);
		}
		else
		{
			result = m_anglesPerDamageChange;
		}
		return result;
	}

	public float GetDistPerDamageChange()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_distPerDamageChangeMod.GetModifiedValue(m_distPerDamageChange);
		}
		else
		{
			result = m_distPerDamageChange;
		}
		return result;
	}

	public int GetMaxDamageChange()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxDamageChangeMod.GetModifiedValue(m_maxDamageChange);
		}
		else
		{
			result = m_maxDamageChange;
		}
		return result;
	}

	public StandardEffectInfo GetNormalEnemyEffect()
	{
		StandardEffectInfo result;
		if (m_cachedNormalEnemyEffect != null)
		{
			result = m_cachedNormalEnemyEffect;
		}
		else
		{
			result = m_normalEnemyEffect;
		}
		return result;
	}

	public StandardEffectInfo GetOverchargedEnemyEffect()
	{
		StandardEffectInfo result;
		if (m_cachedOverchargedEnemyEffect != null)
		{
			result = m_cachedOverchargedEnemyEffect;
		}
		else
		{
			result = m_overchargedEnemyEffect;
		}
		return result;
	}

	public StandardEffectInfo GetSingleEnemyHitEffect()
	{
		return (m_cachedSingleEnemyHitEffect == null) ? m_singleEnemyHitEffect : m_cachedSingleEnemyHitEffect;
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
		if (m_syncComp != null)
		{
			if (m_syncComp.m_overchargeBuffs > 1)
			{
				if (m_overchargeAbility != null && m_overchargeAbility.GetExtraDamageForMultiCast() > 0)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							return m_overchargeAbility.GetExtraDamageForMultiCast();
						}
					}
				}
			}
		}
		return 0;
	}

	public int GetCurrentModdedDamage()
	{
		if (AmOvercharged(base.ActorData))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return GetDamageAmountNormal() + m_overchargeAbility.GetExtraDamage() + GetMultiStackOverchargeDamage();
				}
			}
		}
		return GetDamageAmountNormal();
	}

	public int GetExtraDamageFromAngle(float angleNow)
	{
		if (GetAnglesPerDamageChange() > 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
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
				}
			}
		}
		return 0;
	}

	public int GetExtraDamageFromRadius(float radiusInSquares)
	{
		if (GetDistPerDamageChange() > 0.1f)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
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
				}
			}
		}
		return 0;
	}

	public int GetExtraDamageForEnemy(ActorData caster, ActorData target)
	{
		if (GetExtraDamagePerSquareDistanceFromEnemy() > 0f)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					float num = VectorUtils.HorizontalPlaneDistInSquares(caster.GetFreePos(), target.GetFreePos()) - 1.4f;
					return Mathf.RoundToInt(GetExtraDamagePerSquareDistanceFromEnemy() * num);
				}
				}
			}
		}
		return 0;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_BlasterStretchingCone abilityMod_BlasterStretchingCone = modAsBase as AbilityMod_BlasterStretchingCone;
		AddTokenInt(tokens, "Damage", string.Empty, (!abilityMod_BlasterStretchingCone) ? m_damageAmountNormal : abilityMod_BlasterStretchingCone.m_damageAmountNormalMod.GetModifiedValue(m_damageAmountNormal));
		AddTokenFloat(tokens, "ExtraDamagePerSquareDistanceFromEnemy", string.Empty, m_extraDamagePerSquareDistanceFromEnemy);
		string empty = string.Empty;
		int val;
		if ((bool)abilityMod_BlasterStretchingCone)
		{
			val = abilityMod_BlasterStretchingCone.m_extraDamageForSingleHitMod.GetModifiedValue(m_extraDamageForSingleHit);
		}
		else
		{
			val = m_extraDamageForSingleHit;
		}
		AddTokenInt(tokens, "ExtraDamageForSingleHit", empty, val);
		string empty2 = string.Empty;
		int val2;
		if ((bool)abilityMod_BlasterStretchingCone)
		{
			val2 = abilityMod_BlasterStretchingCone.m_anglesPerDamageChangeMod.GetModifiedValue(m_anglesPerDamageChange);
		}
		else
		{
			val2 = m_anglesPerDamageChange;
		}
		AddTokenInt(tokens, "AnglesPerDamageChange", empty2, val2);
		string empty3 = string.Empty;
		int val3;
		if ((bool)abilityMod_BlasterStretchingCone)
		{
			val3 = abilityMod_BlasterStretchingCone.m_maxDamageChangeMod.GetModifiedValue(m_maxDamageChange);
		}
		else
		{
			val3 = m_maxDamageChange;
		}
		AddTokenInt(tokens, "MaxDamageChange", empty3, val3);
		AbilityMod.AddToken_EffectInfo(tokens, (!abilityMod_BlasterStretchingCone) ? m_normalEnemyEffect : abilityMod_BlasterStretchingCone.m_normalEnemyEffectMod.GetModifiedValue(m_normalEnemyEffect), "NormalEnemyEffect", m_normalEnemyEffect);
		StandardEffectInfo effectInfo;
		if ((bool)abilityMod_BlasterStretchingCone)
		{
			effectInfo = abilityMod_BlasterStretchingCone.m_overchargedEnemyEffectMod.GetModifiedValue(m_overchargedEnemyEffect);
		}
		else
		{
			effectInfo = m_overchargedEnemyEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo, "OverchargedEnemyEffect", m_overchargedEnemyEffect);
		StandardEffectInfo effectInfo2;
		if ((bool)abilityMod_BlasterStretchingCone)
		{
			effectInfo2 = abilityMod_BlasterStretchingCone.m_singleEnemyHitEffectMod.GetModifiedValue(m_singleEnemyHitEffect);
		}
		else
		{
			effectInfo2 = m_singleEnemyHitEffect;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectInfo2, "SingleEnemyHitEffect", m_singleEnemyHitEffect);
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
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null)
		{
			AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = base.Targeter as AbilityUtil_Targeter_StretchCone;
			int num = 0;
			if (abilityUtil_Targeter_StretchCone != null)
			{
				num += GetExtraDamageFromAngle(abilityUtil_Targeter_StretchCone.LastConeAngle);
				num += GetExtraDamageFromRadius(abilityUtil_Targeter_StretchCone.LastConeRadiusInSquares);
				num += GetExtraDamageForEnemy(base.ActorData, targetActor);
			}
			dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
			{
				int visibleActorsCountByTooltipSubject = base.Targeter.GetVisibleActorsCountByTooltipSubject(AbilityTooltipSubject.Enemy);
				int num2 = GetCurrentModdedDamage() + num;
				if (visibleActorsCountByTooltipSubject == 1)
				{
					num2 += GetExtraDamageForSingleHit();
				}
				dictionary[AbilityTooltipSymbol.Damage] = num2;
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
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_BlasterStretchingCone);
			Setup();
			if (!(m_dashAndBlastAbility != null))
			{
				return;
			}
			while (true)
			{
				if (m_dashAndBlastAbility.m_useConeParamsFromPrimary)
				{
					while (true)
					{
						m_dashAndBlastAbility.OnPrimaryAttackModChange();
						return;
					}
				}
				return;
			}
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		Setup();
		if (!(m_dashAndBlastAbility != null))
		{
			return;
		}
		while (true)
		{
			if (m_dashAndBlastAbility.m_useConeParamsFromPrimary)
			{
				while (true)
				{
					m_dashAndBlastAbility.OnPrimaryAttackModChange();
					return;
				}
			}
			return;
		}
	}
}
