// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// Critical Shot, Adrenaline, Overload, Second Wind
public class Card_Standard_Ability : Ability
{
	[Header("-- Targeting --")]
	public bool m_targetCenterAtCaster;
	[Header("-- On Hit --")]
	public int m_healAmount = 30;
	public int m_techPointsAmount;
	
	// removed in rogues
	[Tooltip("Credits to give to the actor who used the card.")]
	public int m_personalCredits;
	[Tooltip("Credits to give to each actor on the team of the actor who used the card (including the card user).")]
	public int m_teamCredits;
	// end removed in rogues
	
	public bool m_applyEffect;
	public StandardActorEffectData m_effect;
	public bool m_overrideEffectRunPhase;
	public AbilityPriority m_phaseOverrideValue = AbilityPriority.Combat_Damage;

	private void Start()
	{
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			AbilityAreaShape.SingleSquare,
			true,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false);
		Targeter.ShowArcToShape = false;
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> number = new List<AbilityTooltipNumber>();
		AbilityTooltipHelper.ReportHealing(ref number, AbilityTooltipSubject.Self, m_healAmount);
		AbilityTooltipHelper.ReportEnergy(ref number, AbilityTooltipSubject.Self, m_techPointsAmount);
		if (m_applyEffect)
		{
			m_effect.ReportAbilityTooltipNumbers(ref number, AbilityTooltipSubject.Self);
		}
		return number;
	}

	protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
	{
		int val = Mathf.Max(0, m_healAmount + m_effect.m_healingPerTurn * Mathf.Max(0, m_effect.m_duration - 1));
		AddTokenInt(tokens, "TotalHeal", string.Empty, val);
		AddTokenInt(tokens, "HealAmount", string.Empty, m_healAmount);
		AddTokenInt(tokens, "TechPointsAmount", string.Empty, m_techPointsAmount);
		
		// removed in rogues
		AddTokenInt(tokens, "PersonalCredits", string.Empty, m_personalCredits);
		AddTokenInt(tokens, "TeamCredits", string.Empty, m_teamCredits);
		// end removed in rogues
		
		m_effect.AddTooltipTokens(tokens, "Effect");
	}

#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
		List<AbilityTarget> targets,
		ActorData caster,
		ServerAbilityUtils.AbilityRunData additionalData)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
		if (m_targetCenterAtCaster && caster.GetCurrentBoardSquare() != null)
		{
			targetSquare = caster.GetCurrentBoardSquare();
		}
		return new ServerClientUtils.SequenceStartData(
			m_sequencePrefab,
			targetSquare,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		ActorData targetActor = targets[0].GetCurrentBestActorTarget();
		if (m_targetCenterAtCaster)
		{
			targetActor = caster;
		}
		if (targetActor == null)
		{
			return;
		}
		ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(targetActor, caster.GetFreePos()));
		actorHitResults.SetBaseHealing(m_healAmount);
		if (m_techPointsAmount > 0)
		{
			actorHitResults.SetTechPointGain(m_techPointsAmount);
		}
		else if (m_techPointsAmount < 0)
		{
			actorHitResults.SetTechPointLoss(m_techPointsAmount);
		}
		if (m_applyEffect)
		{
			StandardActorEffect standardActorEffect = new StandardActorEffect(
				AsEffectSource(),
				targetActor.GetCurrentBoardSquare(),
				targetActor,
				caster,
				m_effect);
			if (m_overrideEffectRunPhase && m_phaseOverrideValue != AbilityPriority.INVALID)
			{
				standardActorEffect.SetHitPhaseBeforeStart(m_phaseOverrideValue);
			}
			actorHitResults.AddEffect(standardActorEffect);
		}
		abilityResults.StoreActorHit(actorHitResults);
	}
#endif
}
