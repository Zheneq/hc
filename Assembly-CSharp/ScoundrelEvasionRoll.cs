using System;
using System.Collections.Generic;
using UnityEngine;

public class ScoundrelEvasionRoll : Ability
{
	[Header("-- Energy gain per step")]
	public int m_extraEnergyPerStep;

	private AbilityMod_ScoundrelEvasionRoll m_abilityMod;

	private void Start()
	{
		if (this.m_abilityName == "Base Ability")
		{
			this.m_abilityName = "Evasion Roll";
		}
		this.SetupTargeter();
	}

	private void SetupTargeter()
	{
		base.Targeter = new AbilityUtil_Targeter_ScoundrelEvasionRoll(this, this.ShouldCreateTrapWire(), this.GetTrapwirePattern());
		if (this.HasAdditionalEffectFromMod())
		{
			(base.Targeter as AbilityUtil_Targeter_ScoundrelEvasionRoll).m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Always;
		}
		else if (this.EffectForLandingInBrush() != null)
		{
			(base.Targeter as AbilityUtil_Targeter_ScoundrelEvasionRoll).m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
		}
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare boardSquareSafe = Board.Get().GetBoardSquareSafe(target.GridPos);
		if (boardSquareSafe != null)
		{
			if (boardSquareSafe.IsBaselineHeight())
			{
				if (boardSquareSafe != caster.GetCurrentBoardSquare())
				{
					return KnockbackUtils.BuildStraightLineChargePath(caster, boardSquareSafe) != null;
				}
			}
		}
		return false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> result = new List<AbilityTooltipNumber>();
		StandardEffectInfo standardEffectInfo = this.EffectForLandingInBrush();
		if (standardEffectInfo != null)
		{
			standardEffectInfo.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		}
		if (this.HasAdditionalEffectFromMod())
		{
			this.m_abilityMod.m_additionalEffectOnStart.ReportAbilityTooltipNumbers(ref result, AbilityTooltipSubject.Self);
		}
		return result;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (this.GetExtraEnergyPerStep() > 0 && base.Targeter is AbilityUtil_Targeter_ScoundrelEvasionRoll)
		{
			AbilityUtil_Targeter_ScoundrelEvasionRoll abilityUtil_Targeter_ScoundrelEvasionRoll = base.Targeter as AbilityUtil_Targeter_ScoundrelEvasionRoll;
			int numNodesInPath = abilityUtil_Targeter_ScoundrelEvasionRoll.m_numNodesInPath;
			if (numNodesInPath > 1)
			{
				return this.GetExtraEnergyPerStep() * (numNodesInPath - 1);
			}
		}
		return 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() == typeof(AbilityMod_ScoundrelEvasionRoll))
		{
			this.m_abilityMod = (abilityMod as AbilityMod_ScoundrelEvasionRoll);
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

	public int GetExtraEnergyPerStep()
	{
		return (!this.m_abilityMod) ? this.m_extraEnergyPerStep : this.m_abilityMod.m_extraEnergyPerStepMod.GetModifiedValue(this.m_extraEnergyPerStep);
	}

	private bool ShouldCreateTrapWire()
	{
		if (this.m_abilityMod != null)
		{
			if (this.m_abilityMod.m_dropTrapWireOnStart)
			{
				return this.m_abilityMod.m_trapwirePattern != AbilityGridPattern.NoPattern;
			}
		}
		return false;
	}

	private bool HasAdditionalEffectFromMod()
	{
		bool result;
		if (this.m_abilityMod != null)
		{
			result = this.m_abilityMod.m_additionalEffectOnStart.m_applyEffect;
		}
		else
		{
			result = false;
		}
		return result;
	}

	private AbilityGridPattern GetTrapwirePattern()
	{
		return (!(this.m_abilityMod == null)) ? this.m_abilityMod.m_trapwirePattern : AbilityGridPattern.NoPattern;
	}

	private int TechPointGainPerAdjacentAlly()
	{
		return (!(this.m_abilityMod != null)) ? 0 : this.m_abilityMod.m_techPointGainPerAdjacentAlly;
	}

	private int TechPointGrantedToAdjacentAllies()
	{
		return (!(this.m_abilityMod != null)) ? 0 : this.m_abilityMod.m_techPointGrantedToAdjacentAllies;
	}

	private StandardEffectInfo EffectForLandingInBrush()
	{
		if (this.m_abilityMod != null)
		{
			if (this.m_abilityMod.m_effectToSelfForLandingInBrush != null)
			{
				if (this.m_abilityMod.m_effectToSelfForLandingInBrush.m_applyEffect)
				{
					return this.m_abilityMod.m_effectToSelfForLandingInBrush;
				}
			}
		}
		return null;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ScoundrelEvasionRoll abilityMod_ScoundrelEvasionRoll = modAsBase as AbilityMod_ScoundrelEvasionRoll;
		string name = "ExtraEnergyPerStep";
		string empty = string.Empty;
		int val;
		if (abilityMod_ScoundrelEvasionRoll)
		{
			val = abilityMod_ScoundrelEvasionRoll.m_extraEnergyPerStepMod.GetModifiedValue(this.m_extraEnergyPerStep);
		}
		else
		{
			val = this.m_extraEnergyPerStep;
		}
		base.AddTokenInt(tokens, name, empty, val, false);
		Passive_StickAndMove component = base.GetComponent<Passive_StickAndMove>();
		string name2 = "CDR_OnDamageTaken";
		string empty2 = string.Empty;
		int val2;
		if (component != null)
		{
			if (component.m_damageToAdvanceCooldown > 0)
			{
				val2 = 1;
				goto IL_88;
			}
		}
		val2 = 0;
		IL_88:
		base.AddTokenInt(tokens, name2, empty2, val2, false);
	}
}
