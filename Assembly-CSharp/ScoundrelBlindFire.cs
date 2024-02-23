using System.Collections.Generic;
using UnityEngine;

public class ScoundrelBlindFire : Ability
{
	public float m_coneWidthAngle = 90f;
	public float m_coneLength = 10f;
	public float m_coneBackwardOffset;
	public int m_damageAmount = 20;
	public bool m_penetrateLineOfSight;
	public bool m_restrictWithinCover;
	public int m_maxTargets = 2;
	public bool m_includeTargetsInCover = true;

	private AbilityMod_ScoundrelBlindFire m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Quick Draw";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_Blindfire(this, ModdedConeWidthAngle(), m_coneLength, m_coneBackwardOffset, ModdedPenetrateLineOfSight(), m_restrictWithinCover, m_includeTargetsInCover, m_maxTargets);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return m_coneLength;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ScoundrelBlindFire))
		{
			m_abilityMod = abilityMod as AbilityMod_ScoundrelBlindFire;
			SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public float ModdedConeWidthAngle()
	{
		return m_abilityMod != null
			? m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle)
			: m_coneWidthAngle;
	}

	public int ModdedDamageAmount()
	{
		return m_abilityMod != null
			? m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount)
			: m_damageAmount;
	}

	public bool ModdedPenetrateLineOfSight()
	{
		return m_abilityMod != null
			? m_abilityMod.m_penetrateLineOfSight.GetModifiedValue(m_penetrateLineOfSight)
			: m_penetrateLineOfSight;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_damageAmount)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null
			&& tooltipSubjectTypes.Count > 0
			&& tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
		{
			var ints = new Dictionary<AbilityTooltipSymbol, int>();
			ints[AbilityTooltipSymbol.Damage] = ModdedDamageAmount();
			return ints;
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DamageAmount", "", m_damageAmount);
		AddTokenInt(tokens, "MaxTargets", "", m_maxTargets);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		return !m_restrictWithinCover || caster.GetComponent<ActorCover>().HasAnyCover();
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		return new List<Vector3>
		{
			caster.GetLoSCheckPos()+ m_coneLength * Board.Get().squareSize * targets[0].AimDirection
		};
	}
}
