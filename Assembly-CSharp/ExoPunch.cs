using System.Collections.Generic;
using UnityEngine;

public class ExoPunch : Ability
{
	[Header("-- Targeting")]
	public float m_coneWidthAngle = 180f;

	public float m_coneBackwardOffset;

	public float m_coneLength = 2.5f;

	public bool m_penetrateLineOfSight;

	public int m_maxTargets = 5;

	[Header("-- Knockback")]
	public float m_knockbackDistance;

	public KnockbackType m_knockbackType;

	[Header("-- On Hit Damage/Effect")]
	public int m_damageAmount = 20;

	public StandardEffectInfo m_targetHitEffect;

	[Header("-- Nearby Hit Bonus")]
	public float m_nearDistThreshold;

	public int m_nearEnemyExtraDamage;

	public StandardEffectInfo m_nearEnemyExtraEffect;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private AbilityMod_ExoPunch m_abilityMod;

	private StandardEffectInfo m_cachedTargetHitEffect;

	private StandardEffectInfo m_cachedNearEnemyExtraEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Exollent Hit";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		AbilityUtil_Targeter_StretchCone abilityUtil_Targeter_StretchCone = new AbilityUtil_Targeter_StretchCone(this, GetConeLength(), GetConeLength(), GetConeWidthAngle(), GetConeWidthAngle(), AreaEffectUtils.StretchConeStyle.Linear, GetConeBackwardOffset(), PenetrateLineOfSight());
		abilityUtil_Targeter_StretchCone.InitKnockbackData(GetKnockbackDistance(), GetKnockbackType(), 0f, KnockbackType.AwayFromSource);
		base.Targeter = abilityUtil_Targeter_StretchCone;
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return GetConeLength();
	}

	private void SetCachedFields()
	{
		m_cachedTargetHitEffect = ((!m_abilityMod) ? m_targetHitEffect : m_abilityMod.m_targetHitEffectMod.GetModifiedValue(m_targetHitEffect));
		StandardEffectInfo cachedNearEnemyExtraEffect;
		if ((bool)m_abilityMod)
		{
			cachedNearEnemyExtraEffect = m_abilityMod.m_nearEnemyExtraEffectMod.GetModifiedValue(m_nearEnemyExtraEffect);
		}
		else
		{
			cachedNearEnemyExtraEffect = m_nearEnemyExtraEffect;
		}
		m_cachedNearEnemyExtraEffect = cachedNearEnemyExtraEffect;
	}

	public float GetConeWidthAngle()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle);
		}
		else
		{
			result = m_coneWidthAngle;
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

	public float GetConeLength()
	{
		return (!m_abilityMod) ? m_coneLength : m_abilityMod.m_coneLengthMod.GetModifiedValue(m_coneLength);
	}

	public bool PenetrateLineOfSight()
	{
		return (!m_abilityMod) ? m_penetrateLineOfSight : m_abilityMod.m_penetrateLineOfSightMod.GetModifiedValue(m_penetrateLineOfSight);
	}

	public int GetMaxTargets()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets);
		}
		else
		{
			result = m_maxTargets;
		}
		return result;
	}

	public float GetKnockbackDistance()
	{
		return (!m_abilityMod) ? m_knockbackDistance : m_abilityMod.m_knockbackDistanceMod.GetModifiedValue(m_knockbackDistance);
	}

	public KnockbackType GetKnockbackType()
	{
		KnockbackType result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_knockbackTypeMod.GetModifiedValue(m_knockbackType);
		}
		else
		{
			result = m_knockbackType;
		}
		return result;
	}

	public int GetDamageAmount()
	{
		int result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_damageAmountMod.GetModifiedValue(m_damageAmount);
		}
		else
		{
			result = m_damageAmount;
		}
		return result;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		StandardEffectInfo result;
		if (m_cachedTargetHitEffect != null)
		{
			result = m_cachedTargetHitEffect;
		}
		else
		{
			result = m_targetHitEffect;
		}
		return result;
	}

	public float GetNearDistThreshold()
	{
		float result;
		if ((bool)m_abilityMod)
		{
			result = m_abilityMod.m_nearDistThresholdMod.GetModifiedValue(m_nearDistThreshold);
		}
		else
		{
			result = m_nearDistThreshold;
		}
		return result;
	}

	public int GetNearEnemyExtraDamage()
	{
		return (!m_abilityMod) ? m_nearEnemyExtraDamage : m_abilityMod.m_nearEnemyExtraDamageMod.GetModifiedValue(m_nearEnemyExtraDamage);
	}

	public StandardEffectInfo GetNearEnemyExtraEffect()
	{
		StandardEffectInfo result;
		if (m_cachedNearEnemyExtraEffect != null)
		{
			result = m_cachedNearEnemyExtraEffect;
		}
		else
		{
			result = m_nearEnemyExtraEffect;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, GetDamageAmount()));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		Dictionary<AbilityTooltipSymbol, int> dictionary = null;
		float num = GetNearDistThreshold() * Board.Get().squareSize;
		if (num > 0f)
		{
			ActorData actorData = base.ActorData;
			Vector3 vector = targetActor.GetFreePos() - actorData.GetFreePos();
			vector.y = 0f;
			if (vector.magnitude < num)
			{
				List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
				if (tooltipSubjectTypes != null)
				{
					if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Enemy))
					{
						if (actorData != null)
						{
							dictionary = new Dictionary<AbilityTooltipSymbol, int>();
							int num2 = GetDamageAmount();
							if (GetNearEnemyExtraDamage() > 0)
							{
								num2 += GetNearEnemyExtraDamage();
							}
							dictionary[AbilityTooltipSymbol.Damage] = num2;
						}
					}
				}
			}
		}
		return dictionary;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Damage", "damage in the cone", GetDamageAmount());
		AddTokenInt(tokens, "Knockback_Distance", "range of knockback for hit enemies", Mathf.RoundToInt(GetKnockbackDistance()));
		AddTokenInt(tokens, "Cone_Angle", "angle of the damage cone", (int)GetConeWidthAngle());
		AddTokenInt(tokens, "Cone_Length", "range of the damage cone", Mathf.RoundToInt(GetConeLength()));
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ExoPunch))
		{
			return;
		}
		while (true)
		{
			m_abilityMod = (abilityMod as AbilityMod_ExoPunch);
			SetupTargeter();
			return;
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}
}
