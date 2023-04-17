// ROGUES
// SERVER
using System.Collections.Generic;

public class ThiefStealthGenerator : Ability
{
	public GroundEffectField m_stealthGeneratorInfo;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Stealth Generator";
		}
		// rogues
		// if (m_stealthGeneratorInfo == null)
		// {
		// 	m_stealthGeneratorInfo = ScriptableObject.CreateInstance<GroundEffectField>();
		// }
		Targeter = new AbilityUtil_Targeter_Shape(
			this,
			m_stealthGeneratorInfo.shape,
			m_stealthGeneratorInfo.penetrateLos,
			AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
			m_stealthGeneratorInfo.IncludeEnemies(),
			m_stealthGeneratorInfo.IncludeAllies(),
			m_stealthGeneratorInfo.canIncludeCaster
				? AbilityUtil_Targeter.AffectsActor.Possible
				: AbilityUtil_Targeter.AffectsActor.Never);
	}

	protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
	{
		List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
		// rogues
		// if (m_stealthGeneratorInfo == null)
		// {
		// 	m_stealthGeneratorInfo = ScriptableObject.CreateInstance<GroundEffectField>();
		// }
		m_stealthGeneratorInfo.ReportAbilityTooltipNumbers(ref numbers, AbilityTooltipSubject.Enemy, AbilityTooltipSubject.Ally);
		return numbers;
	}

#if SERVER
	// added in rogues
	public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		return new ServerClientUtils.SequenceStartData(
			AsEffectSource().GetSequencePrefab(),
			AreaEffectUtils.GetCenterOfShape(m_stealthGeneratorInfo.shape, targets[0]),
			m_stealthGeneratorInfo.GetAffectableActorsInField(targets[0], caster, null).ToArray(),
			caster,
			additionalData.m_sequenceSource);
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> affectableActorsInField = m_stealthGeneratorInfo.GetAffectableActorsInField(targets[0], caster, nonActorTargetInfo);
		foreach (ActorData hitActor in affectableActorsInField)
		{
			ActorHitResults hitResults = new ActorHitResults(new ActorHitParameters(hitActor, hitActor.GetFreePos()));
			m_stealthGeneratorInfo.SetupActorHitResult(ref hitResults, caster, targetSquare);
			abilityResults.StoreActorHit(hitResults);
		}
		PositionHitResults positionHitResults = new PositionHitResults(
			new PositionHitParameters(AreaEffectUtils.GetCenterOfShape(m_stealthGeneratorInfo.shape, targets[0])));
		StandardGroundEffect standardGroundEffect = new StandardGroundEffect(
			AsEffectSource(),
			targetSquare,
			targets[0].FreePos,
			null,
			caster,
			m_stealthGeneratorInfo);
		standardGroundEffect.AddToActorsHitThisTurn(affectableActorsInField);
		positionHitResults.AddEffect(standardGroundEffect);
		abilityResults.StorePositionHit(positionHitResults);
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
#endif
}
