// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class MantaCreateBarriersChainFinal : Ability
{
	[Header("-- Ground effect")]
	public StandardGroundEffectInfo m_groundEffectInfo;
	public int m_damageOnCast = 30;
	[Header("-- Sequences -------------------------------------------------")]
	public GameObject m_castSequencePrefab;

	private void Start()
	{
		if (m_abilityName == "Base Ability")
		{
			m_abilityName = "Lair chained ability";
		}
	}

	public int GetDamageOnCast()
	{
		return m_damageOnCast;
	}
	
#if SERVER
	// added in rogues
	public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(List<AbilityTarget> targets, ActorData caster, ServerAbilityUtils.AbilityRunData additionalData)
	{
		List<ServerClientUtils.SequenceStartData> abilityRunSequenceStartDataList =
			base.GetAbilityRunSequenceStartDataList(targets, caster, additionalData);
		abilityRunSequenceStartDataList.Add(new ServerClientUtils.SequenceStartData(
			m_castSequencePrefab,
			caster.GetCurrentBoardSquare(),
			additionalData.m_abilityResults.HitActorsArray(),
			caster,
			additionalData.m_sequenceSource));
		return abilityRunSequenceStartDataList;
	}

	// added in rogues
	public override void GatherAbilityResults(List<AbilityTarget> targets, ActorData caster, ref AbilityResults abilityResults)
	{
		BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		GroundEffectField groundEffectData = m_groundEffectInfo.m_groundEffectData;
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
			m_groundEffectInfo.m_groundEffectData.shape, targets[0], true, caster, caster.GetOtherTeams(), nonActorTargetInfo);
		Vector3 centerOfShape = AreaEffectUtils.GetCenterOfShape(groundEffectData.shape, targets[0]);
		foreach (ActorData actorData in actorsInShape)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, centerOfShape));
			if (m_groundEffectInfo.m_applyGroundEffect)
			{
				m_groundEffectInfo.SetupActorHitResult(ref actorHitResults, caster, actorData.GetCurrentBoardSquare());
			}
			actorHitResults.AddBaseDamage(GetDamageOnCast());
			abilityResults.StoreActorHit(actorHitResults);
		}
		if (m_groundEffectInfo.m_applyGroundEffect)
		{
			ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
			StandardGroundEffect standardGroundEffect = new StandardGroundEffect(
				AsEffectSource(), targetSquare, centerOfShape, null, caster, groundEffectData);
			standardGroundEffect.AddToActorsHitThisTurn(actorsInShape);
			casterHitResults.AddEffect(standardGroundEffect);
			abilityResults.StoreActorHit(casterHitResults);
		}
		abilityResults.StoreNonActorTargetInfo(nonActorTargetInfo);
	}
#endif
}
