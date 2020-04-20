using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoundrelBlindFire : Ability
{
	public float m_coneWidthAngle = 90f;

	public float m_coneLength = 10f;

	public float m_coneBackwardOffset;

	public int m_damageAmount = 0x14;

	public bool m_penetrateLineOfSight;

	public bool m_restrictWithinCover;

	public int m_maxTargets = 2;

	public bool m_includeTargetsInCover = true;

	private AbilityMod_ScoundrelBlindFire m_abilityMod;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScoundrelBlindFire.Start()).MethodHandle;
			}
			this.m_abilityName = "Quick Draw";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_Blindfire(this, this.ModdedConeWidthAngle(), this.m_coneLength, this.m_coneBackwardOffset, this.ModdedPenetrateLineOfSight(), this.m_restrictWithinCover, this.m_includeTargetsInCover, this.m_maxTargets);
	}

	public override bool CanShowTargetableRadiusPreview()
	{
		return true;
	}

	public override float GetTargetableRadiusInSquares(ActorData caster)
	{
		return this.m_coneLength;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ScoundrelBlindFire))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ScoundrelBlindFire);
			this.SetupTargeter();
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	public float ModdedConeWidthAngle()
	{
		float result = this.m_coneWidthAngle;
		if (this.m_abilityMod != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScoundrelBlindFire.ModdedConeWidthAngle()).MethodHandle;
			}
			result = this.m_abilityMod.m_coneWidthAngleMod.GetModifiedValue(this.m_coneWidthAngle);
		}
		return result;
	}

	public int ModdedDamageAmount()
	{
		int result = this.m_damageAmount;
		if (this.m_abilityMod != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScoundrelBlindFire.ModdedDamageAmount()).MethodHandle;
			}
			result = this.m_abilityMod.m_damageMod.GetModifiedValue(this.m_damageAmount);
		}
		return result;
	}

	public bool ModdedPenetrateLineOfSight()
	{
		if (this.m_abilityMod != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScoundrelBlindFire.ModdedPenetrateLineOfSight()).MethodHandle;
			}
			return this.m_abilityMod.m_penetrateLineOfSight.GetModifiedValue(this.m_penetrateLineOfSight);
		}
		return this.m_penetrateLineOfSight;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, AbilityTooltipSubject.Primary, this.m_damageAmount)
		};
	}

	public override Dictionary<AbilityTooltipSymbol, int> GetCustomNameplateItemTooltipValues(ActorData targetActor, int currentTargeterIndex)
	{
		List<AbilityTooltipSubject> tooltipSubjectTypes = base.Targeter.GetTooltipSubjectTypes(targetActor);
		if (tooltipSubjectTypes != null && tooltipSubjectTypes.Count > 0)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ScoundrelBlindFire.GetCustomNameplateItemTooltipValues(ActorData, int)).MethodHandle;
			}
			Dictionary<AbilityTooltipSymbol, int> dictionary = new Dictionary<AbilityTooltipSymbol, int>();
			if (tooltipSubjectTypes.Contains(AbilityTooltipSubject.Primary))
			{
				dictionary[AbilityTooltipSymbol.Damage] = this.ModdedDamageAmount();
				return dictionary;
			}
		}
		return null;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		base.AddTokenInt(tokens, "DamageAmount", string.Empty, this.m_damageAmount, false);
		base.AddTokenInt(tokens, "MaxTargets", string.Empty, this.m_maxTargets, false);
	}

	public override bool CustomCanCastValidation(ActorData caster)
	{
		if (this.m_restrictWithinCover)
		{
			ActorCover component = caster.GetComponent<ActorCover>();
			return component.HasAnyCover(false);
		}
		return true;
	}

	public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
	{
		List<Vector3> list = new List<Vector3>();
		Vector3 aimDirection = targets[0].AimDirection;
		Vector3 travelBoardSquareWorldPositionForLos = caster.GetTravelBoardSquareWorldPositionForLos();
		list.Add(travelBoardSquareWorldPositionForLos + this.m_coneLength * Board.Get().squareSize * aimDirection);
		return list;
	}
}
