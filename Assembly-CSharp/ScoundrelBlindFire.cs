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
		base.Targeter = new AbilityUtil_Targeter_Blindfire(this, ModdedConeWidthAngle(), m_coneLength, m_coneBackwardOffset, ModdedPenetrateLineOfSight(), m_restrictWithinCover, m_includeTargetsInCover, m_maxTargets);
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
			m_abilityMod = (abilityMod as AbilityMod_ScoundrelBlindFire);
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
		float result = m_coneWidthAngle;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle);
		}
		return result;
	}

	public int ModdedDamageAmount()
	{
		int result = m_damageAmount;
		if (m_abilityMod != null)
		{
			result = m_abilityMod.m_damageMod.GetModifiedValue(m_damageAmount);
		}
		return result;
	}

	public bool ModdedPenetrateLineOfSight()
	{
		if (m_abilityMod != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return m_abilityMod.m_penetrateLineOfSight.GetModifiedValue(m_penetrateLineOfSight);
				}
			}
		}
		return m_penetrateLineOfSight;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> list = new List<AbilityTooltipNumber>();
		list.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, m_damageAmount));
		return list;
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null && tooltipSubjectTypes.Count > 0)
		{
			Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				dictionary[AbilityTooltipSymbol.Damage] = ModdedDamageAmount();
				return dictionary;
			}
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AddTokenInt(tokens, "DamageAmount", string.Empty, m_damageAmount);
		AddTokenInt(tokens, "MaxTargets", string.Empty, m_maxTargets);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (m_restrictWithinCover)
		{
			ActorCover component = caster.GetComponent<ActorCover>();
			return component.HasAnyCover();
		}
		return true;
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		Vector3 aimDirection = targets[0].AimDirection;
		Vector3 travelBoardSquareWorldPositionForLos = caster.GetLoSCheckPos();
		list.Add(travelBoardSquareWorldPositionForLos + m_coneLength * Board.Get().squareSize * aimDirection);
		return list;
	}
}
