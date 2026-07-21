// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimalStealth : Ability
{
	public StandardActorEffectData m_selfEffect;
	public bool m_useCharge;
	[TextArea(1, 10)]
	public string m_notes;

	private AbilityMod_RobotAnimalStealth m_abilityMod;

	private void Start()
	{
		if (m_useCharge && GetNumTargets() == 0)
		{
			Debug.LogError("Robot Animal Stealth cannot use charge if there is no targeter targets specified");
			m_useCharge = false;
		}
		SetupTargeter();
	}

	public override int GetExpectedNumberOfTargeters()
	{
		return UseCharge()
			? Mathf.Clamp(GetNumTargets(), 1, 2)
			: 1;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		GetModdedStealthEffect().ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Self);
		return numbers;
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		if (!UseCharge())
		{
			return true;
		}
		if (targetIndex == 0)
		{
			BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
			return targetSquare != null
			       && targetSquare.IsValidForGameplay()
			       && KnockbackUtils.BuildStraightLineChargePath(caster, targetSquare) != null;
		}
		else
		{
			BoardSquare firstTargetSquare = Board.Get().GetSquare(currentTargets[0].GridPos);
			BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
			return targetSquare != null
			       && firstTargetSquare != targetSquare
			       && targetSquare.IsValidForGameplay()
			       && KnockbackUtils.BuildStraightLineChargePath(caster, targetSquare, firstTargetSquare, false) != null;
		}
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		m_selfEffect.AddTooltipTokens(tokens, "SelfEffect");
	}

	private void SetupTargeter()
	{
		if (!UseCharge())
		{
			Targeter = new AbilityUtil_Targeter_Shape(
				this,
				AbilityAreaShape.SingleSquare,
				true,
				AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
				false,
				false,
				AbilityUtil_Targeter.AffectsActor.Always);
			Targeter.ShowArcToShape = false;
		}
		else if (GetExpectedNumberOfTargeters() < 2)
		{
			Targeter = new AbilityUtil_Targeter_ChargeAoE(
				this, 
				0f,
				0f, 
				0f, 
				-1, 
				false,
				false);
		}
		else
		{
			ClearTargeters();
			for (int i = 0; i < GetExpectedNumberOfTargeters(); i++)
			{
				Targeters.Add(new AbilityUtil_Targeter_ChargeAoE(
					this, 
					0f,
					0f, 
					0f, 
					-1, 
					false, 
					false));
				Targeters[i].SetUseMultiTargetUpdate(true);
			}
		}
	}

	protected override void OnApplyAbilityMod(AbilityMod abilityMod)
	{
		if (abilityMod.GetType() != typeof(AbilityMod_RobotAnimalStealth))
		{
			Debug.LogError("Trying to apply wrong type of ability mod");
			return;
		}
		m_abilityMod = abilityMod as AbilityMod_RobotAnimalStealth;
		SetupTargeter();
	}

	protected override void OnRemoveAbilityMod()
	{
		m_abilityMod = null;
		SetupTargeter();
	}

	public StandardActorEffectData GetModdedStealthEffect()
	{
		return m_abilityMod != null && m_abilityMod.m_selfEffectOverride.m_applyEffect
			? m_abilityMod.m_selfEffectOverride.m_effectData
			: m_selfEffect;
	}

	public bool ShouldApplyEffectOnNextDamageAttack()
	{
		return m_abilityMod != null && m_abilityMod.m_effectOnNextDamageAttack.m_applyEffect;
	}

	public StandardEffectInfo GetEffectOnNextDamageAttack()
	{
		return m_abilityMod != null
			? m_abilityMod.m_effectOnNextDamageAttack
			: new StandardEffectInfo();
	}

	public int GetExtraDamageNextAttack()
	{
		return m_abilityMod != null
			? m_abilityMod.m_extraDamageNextAttack
			: 0;
	}

	public bool UseCharge()
	{
		return m_abilityMod != null
			? GetNumTargets() > 0
			  && m_abilityMod.m_useChainAbilityOverrides
			  && m_abilityMod.m_chainAbilityOverrides.Length > 0
			: m_useCharge;
	}
	
#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			caster.GetFreePos(),
			caster.AsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void Run(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		if (m_abilityMod != null)
		{
			m_abilityMod.m_cooldownModOnCast.ModifyCooldown(caster.GetAbilityData());
		}
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorHitParameters hitParams = new ActorHitParameters(caster, caster.GetFreePos());
		StandardActorEffect effect = new RobotAnimalStealthEffect(
			AsEffectSource(),
			caster.GetCurrentBoardSquare(),
			caster,
			caster,
			GetModdedStealthEffect());
		abilityResults.StoreActorHit(new ActorHitResults(effect, hitParams));
	}
#endif
}
