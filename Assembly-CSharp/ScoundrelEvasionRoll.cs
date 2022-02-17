using System.Collections.Generic;
using UnityEngine;

public class ScoundrelEvasionRoll : Ability
{
	[Header("-- Energy gain per step")]
	public int m_extraEnergyPerStep;

	private AbilityMod_ScoundrelEvasionRoll m_abilityMod;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Evasion Roll";
		}
		SetupTargeter();
	}

	private void SetupTargeter()
	{
		Targeter = new AbilityUtil_Targeter_ScoundrelEvasionRoll(this, ShouldCreateTrapWire(), GetTrapwirePattern());
		if (HasAdditionalEffectFromMod())
		{
			(Targeter as AbilityUtil_Targeter_ScoundrelEvasionRoll).m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Always;
		}
		else if (EffectForLandingInBrush() != null)
		{
			(Targeter as AbilityUtil_Targeter_ScoundrelEvasionRoll).m_affectsCaster = AbilityUtil_Targeter.AffectsActor.Possible;
		}
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
		return targetSquare != null
			&& targetSquare.IsValidForGameplay()
			&& targetSquare != caster.GetCurrentBoardSquare()
			&& KnockbackUtils.BuildStraightLineChargePath(caster, targetSquare) != null;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		StandardEffectInfo brushEffect = EffectForLandingInBrush();
		if (brushEffect != null)
		{
			brushEffect.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		}
		if (HasAdditionalEffectFromMod())
		{
			m_abilityMod.m_additionalEffectOnStart.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		}
		return numbers;
	}

	public override int GetAdditionalTechPointGainForNameplateItem(ActorData caster, int currentTargeterIndex)
	{
		if (GetExtraEnergyPerStep() > 0 && base.Targeter is AbilityUtil_Targeter_ScoundrelEvasionRoll)
		{
			AbilityUtil_Targeter_ScoundrelEvasionRoll targeter = Targeter as AbilityUtil_Targeter_ScoundrelEvasionRoll;
			if (targeter.m_numNodesInPath > 1)
			{
				return GetExtraEnergyPerStep() * (targeter.m_numNodesInPath - 1);
			}
		}
		return 0;
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_ScoundrelEvasionRoll))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}

		m_abilityMod = (abilityMod as AbilityMod_ScoundrelEvasionRoll);
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public int GetExtraEnergyPerStep()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraEnergyPerStepMod.GetModifiedValue(m_extraEnergyPerStep)
			: m_extraEnergyPerStep;
	}

	private bool ShouldCreateTrapWire()
	{
		return m_abilityMod != null
			&& m_abilityMod.m_dropTrapWireOnStart
			&& m_abilityMod.m_trapwirePattern != AbilityGridPattern.NoPattern;
	}

	private bool HasAdditionalEffectFromMod()
	{
		return m_abilityMod != null
			&& m_abilityMod.m_additionalEffectOnStart.m_applyEffect;
	}

	private AbilityGridPattern GetTrapwirePattern()
	{
		return m_abilityMod != null
			? m_abilityMod.m_trapwirePattern
			: AbilityGridPattern.NoPattern;
	}

	private int TechPointGainPerAdjacentAlly()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointGainPerAdjacentAlly
			: 0;
	}

	private int TechPointGrantedToAdjacentAllies()
	{
		return m_abilityMod != null
			? m_abilityMod.m_techPointGrantedToAdjacentAllies
			: 0;
	}

	private StandardEffectInfo EffectForLandingInBrush()
	{
		if (m_abilityMod != null
			&& m_abilityMod.m_effectToSelfForLandingInBrush != null
			&& m_abilityMod.m_effectToSelfForLandingInBrush.m_applyEffect)
		{
			return m_abilityMod.m_effectToSelfForLandingInBrush;
		}
		return null;
	}

	internal override ActorData.MovementType GetMovementType()
	{
		return ActorData.MovementType.Charge;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		AbilityMod_ScoundrelEvasionRoll mod = modAsBase as AbilityMod_ScoundrelEvasionRoll;
		AddTokenInt(tokens, "ExtraEnergyPerStep", "", mod != null
			? mod.m_extraEnergyPerStepMod.GetModifiedValue(m_extraEnergyPerStep)
			: m_extraEnergyPerStep);
		Passive_StickAndMove stickAndMove = GetComponent<Passive_StickAndMove>();
		AddTokenInt(tokens, "CDR_OnDamageTaken", "", stickAndMove != null && stickAndMove.m_damageToAdvanceCooldown > 0 ? 1 : 0);
	}
}
