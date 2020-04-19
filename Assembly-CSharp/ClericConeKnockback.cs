using System;
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
	public int m_damageAmount = 0x14;

	public StandardEffectInfo m_targetHitEffect;

	[Header("-- Sequences")]
	public GameObject m_castSequencePrefab;

	private StandardEffectInfo m_cachedTargetHitEffect;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Cone Knockback";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		this.SetCachedFields();
		for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
		{
			AbilityUtil_Targeter_ClericConeKnockback abilityUtil_Targeter_ClericConeKnockback = new AbilityUtil_Targeter_ClericConeKnockback(this, this.GetConeLength(), this.GetConeWidthAngle(), this.GetConeBackwardOffset(), this.PenetrateLineOfSight(), this.GetKnockbackDistance(), this.GetKnockbackType());
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
		return this.GetConeLength();
	}

	private void SetCachedFields()
	{
		this.m_cachedTargetHitEffect = this.m_targetHitEffect;
	}

	public float GetConeWidthAngle()
	{
		return this.m_coneWidthAngle;
	}

	public float GetConeBackwardOffset()
	{
		return this.m_coneBackwardOffset;
	}

	public float GetConeLength()
	{
		return this.m_coneLength;
	}

	public bool PenetrateLineOfSight()
	{
		return this.m_penetrateLineOfSight;
	}

	public int GetMaxTargets()
	{
		return this.m_maxTargets;
	}

	public float GetKnockbackDistance()
	{
		return this.m_knockbackDistance;
	}

	public KnockbackType GetKnockbackType()
	{
		return this.m_knockbackType;
	}

	public int GetDamageAmount()
	{
		return this.m_damageAmount;
	}

	public StandardEffectInfo GetTargetHitEffect()
	{
		StandardEffectInfo result;
		if (this.m_cachedTargetHitEffect != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClericConeKnockback.GetTargetHitEffect()).MethodHandle;
			}
			result = this.m_cachedTargetHitEffect;
		}
		else
		{
			result = this.m_targetHitEffect;
		}
		return result;
	}

	protected override List<AbilityTooltipNumber> CalculateNameplateTargetingNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.GetDamageAmount())
		};
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "Damage", "damage in the cone", this.GetDamageAmount(), false);
		base.AddTokenInt(tokens, "Knockback_Distance", "range of knockback for hit enemies", Mathf.RoundToInt(this.GetKnockbackDistance()), false);
		base.AddTokenInt(tokens, "Cone_Angle", "angle of the damage cone", (int)this.GetConeWidthAngle(), false);
		base.AddTokenInt(tokens, "Cone_Length", "range of the damage cone", Mathf.RoundToInt(this.GetConeLength()), false);
	}
}
