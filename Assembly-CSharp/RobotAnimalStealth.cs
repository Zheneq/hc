using System;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimalStealth : Ability
{
	public StandardActorEffectData m_selfEffect;

	public bool m_useCharge;

	[TextArea(1, 0xA)]
	public string m_notes;

	private AbilityMod_RobotAnimalStealth m_abilityMod;

	private void Start()
	{
		if (this.m_useCharge)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalStealth.Start()).MethodHandle;
			}
			if (base.GetNumTargets() == 0)
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
				Debug.LogError("Robot Animal Stealth cannot use charge if there is no targeter targets specified");
				this.m_useCharge = false;
			}
		}
		this.SetupTargeter();
	}

	public override int GetExpectedNumberOfTargeters()
	{
		if (this.UseCharge())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalStealth.GetExpectedNumberOfTargeters()).MethodHandle;
			}
			return Mathf.Clamp(base.GetNumTargets(), 1, 2);
		}
		return 1;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		this.GetModdedStealthEffect().ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		return result;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (!this.UseCharge())
		{
			return true;
		}
		if (targetIndex == 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalStealth.CustomTargetValidation(ActorData, AbilityTarget, int, List<AbilityTarget>)).MethodHandle;
			}
			BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
			if (boardSquareSafe != null)
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
				if (boardSquareSafe.IsBaselineHeight())
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
					return KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe) != null;
				}
			}
			return false;
		}
		BoardSquare boardSquareSafe2 = Board.Get().GetBoardSquareSafe(currentTargets[0].GridPos);
		BoardSquare boardSquareSafe3 = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (boardSquareSafe3 != null)
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
			if (boardSquareSafe2 != boardSquareSafe3)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (boardSquareSafe3.IsBaselineHeight() && KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe3, boardSquareSafe2, false) != null)
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
					return true;
				}
			}
		}
		return false;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		this.m_selfEffect.AddTooltipTokens(tokens, "SelfEffect", false, null);
	}

	private void SetupTargeter()
	{
		if (this.UseCharge())
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalStealth.SetupTargeter()).MethodHandle;
			}
			if (this.GetExpectedNumberOfTargeters() < 2)
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
				base.Targeter = new AbilityUtil_Targeter_ChargeAoE(this, 0f, 0f, 0f, -1, false, false);
			}
			else
			{
				base.ClearTargeters();
				for (int i = 0; i < this.GetExpectedNumberOfTargeters(); i++)
				{
					base.Targeters.Add(new AbilityUtil_Targeter_ChargeAoE(this, 0f, 0f, 0f, -1, false, false));
					base.Targeters[i].SetUseMultiTargetUpdate(true);
				}
			}
		}
		else
		{
			base.Targeter = new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, true, AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape, false, false, AbilityUtil_Targeter.AffectsActor.Always, AbilityUtil_Targeter.AffectsActor.Possible);
			base.Targeter.ShowArcToShape = false;
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_RobotAnimalStealth))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_RobotAnimalStealth);
			this.SetupTargeter();
		}
		else
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
		}
	}

	protected override void OnRemoveAbilityMod()
	{
		this.m_abilityMod = null;
		this.SetupTargeter();
	}

	public StandardActorEffectData GetModdedStealthEffect()
	{
		if (!(this.m_abilityMod == null))
		{
			if (this.m_abilityMod.m_selfEffectOverride.m_applyEffect)
			{
				return this.m_abilityMod.m_selfEffectOverride.m_effectData;
			}
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalStealth.GetModdedStealthEffect()).MethodHandle;
			}
		}
		return this.m_selfEffect;
	}

	public bool ShouldApplyEffectOnNextDamageAttack()
	{
		return !(this.m_abilityMod == null) && this.m_abilityMod.m_effectOnNextDamageAttack.m_applyEffect;
	}

	public StandardEffectInfo GetEffectOnNextDamageAttack()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_effectOnNextDamageAttack : new StandardEffectInfo();
	}

	public int GetExtraDamageNextAttack()
	{
		int result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalStealth.GetExtraDamageNextAttack()).MethodHandle;
			}
			result = 0;
		}
		else
		{
			result = this.m_abilityMod.m_extraDamageNextAttack;
		}
		return result;
	}

	public bool UseCharge()
	{
		bool result;
		if (this.m_abilityMod == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(RobotAnimalStealth.UseCharge()).MethodHandle;
			}
			result = this.m_useCharge;
		}
		else if (base.GetNumTargets() > 0)
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
			result = (this.m_abilityMod.m_useChainAbilityOverrides && this.m_abilityMod.m_chainAbilityOverrides.Length > 0);
		}
		else
		{
			result = false;
		}
		return result;
	}
}
