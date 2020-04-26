using System.Collections.Generic;
using UnityEngine;

public class ClericConeKnockback : Ability
{
	[Header("-- Targeting")]
	public float m_coneWidthAngle = 180f;

	public float m_coneBackwardOffset;

	public float m_coneLength = 2.5f;

	public bool m_penetrateLineOfSight;

	public int m_maxTargets = 5;

	[Header("-- Knockback")]
	public float m_knockbackDistance;

	public KnockbackType m_knockbackType = KnockbackType.PerpendicularAwayFromAimDir;

	[Header("-- On Hit Damage/Effect")]
	public int m_damageAmount = 20;

	public StandardEffectInfo m_targetHitEffect;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private StandardEffectInfo m_cachedTargetHitEffect;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Cone Knockback";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		SetCachedFields();
		for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_ClericConeKnockback abilityUtil_Targeter_ClericConeKnockback = new AbilityUtil_Targeter_ClericConeKnockback(this, GetConeLength(), GetConeWidthAngle(), GetConeBackwardOffset(), PenetrateLineOfSight(), GetKnockbackDistance(), GetKnockbackType());
			abilityUtil_Targeter_ClericConeKnockback.SetUseMultiTargetUpdate(true);
			base.Targeters.Add(abilityUtil_Targeter_ClericConeKnockback);
		}
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return 2;
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
		m_cachedTargetHitEffect = m_targetHitEffect;
	}

	public float GetConeWidthAngle()
	{
		return m_coneWidthAngle;
	}

	public float GetConeBackwardOffset()
	{
		return m_coneBackwardOffset;
	}

	public float GetConeLength()
	{
		return m_coneLength;
	}

	public bool PenetrateLineOfSight()
	{
		return m_penetrateLineOfSight;
	}

	public int GetMaxTargets()
	{
		return m_maxTargets;
	}

	public float GetKnockbackDistance()
	{
		return m_knockbackDistance;
	}

	public KnockbackType GetKnockbackType()
	{
		return m_knockbackType;
	}

	public int GetDamageAmount()
	{
		return m_damageAmount;
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

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, GetDamageAmount()));
		return list;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "Damage", "damage in the cone", GetDamageAmount());
		AddTokenInt(tokens, "Knockback_Distance", "range of knockback for hit enemies", Mathf.RoundToInt(GetKnockbackDistance()));
		AddTokenInt(tokens, "Cone_Angle", "angle of the damage cone", (int)GetConeWidthAngle());
		AddTokenInt(tokens, "Cone_Length", "range of the damage cone", Mathf.RoundToInt(GetConeLength()));
	}
}
