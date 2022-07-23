// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class SorceressAbsorbShield : Ability
{
	public int m_duration = 3;
	public int m_absorbAmount = 50;
	public AbilityAreaShape m_shape;

	private void Start()
	{
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			m_shape,
			true,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			false, 
			true);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		return new List<AbilityTooltipNumber>
		{
			new AbilityTooltipNumber(AbilityTooltipSymbol.Absorb, AbilityTooltipSubject.Primary, m_absorbAmount)
		};
	}

	public override bool CustomTargetValidation(ActorData caster, AbilityTarget target, int targetIndex, List<AbilityTarget> currentTargets)
	{
		ActorData currentBestActorTarget = target.GetCurrentBestActorTarget();
		return CanTargetActorInDecision(
			caster,
			currentBestActorTarget,
			false,
			true, 
			true,
			ValidateCheckPath.Ignore,
			true,
			true);
	}

#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			square,
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(m_shape, targets[0], true, caster, caster.GetTeam(), null);
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(m_shape, targets[0]);
		foreach (ActorData actorData in actorsInShape)
		{
			ActorHitParameters hitParams = new ActorHitParameters(actorData, centerOfShape);
			SorceressAbsorbShieldEffect effect = new SorceressAbsorbShieldEffect(
				AsEffectSource(),
				actorData.GetCurrentBoardSquare(),
				actorData,
				caster,
				m_duration,
				m_absorbAmount,
				abilityResults.SequenceSource);
			abilityResults.StoreActorHit(new ActorHitResults(effect, hitParams));
		}
	}
#endif
}
