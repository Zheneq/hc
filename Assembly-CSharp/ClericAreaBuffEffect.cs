// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// custom
public class ClericAreaBuffEffect: StandardActorEffect
{
	private ClericAreaBuff m_ability;
	public Cleric_SyncComponent m_syncComp;
    
	public ClericAreaBuffEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData data,
		ClericAreaBuff ability,
		Cleric_SyncComponent syncComp)
		: base(parent, targetSquare, target, caster, data)
	{
		m_effectName = "Cleric Area Buff Effect";
		m_time.duration = 0;
		HitPhase = AbilityPriority.Prep_Offense;
		m_ability = ability;
		m_syncComp = syncComp;
	}

	// public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	// {
	// 	return new List<ServerClientUtils.SequenceStartData>
	// 	{
	// 		new ServerClientUtils.SequenceStartData(
	// 			m_persistentSequencePrefab,
	// 			TargetSquare,
	// 			null,
	// 			Caster,
	// 			SequenceSource)
	// 	};
	// }
	//
	// public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	// {
	// 	return new List<ServerClientUtils.SequenceStartData>
	// 	{
	// 		new ServerClientUtils.SequenceStartData(
	// 			m_pulseSequencePrefab,
	// 			actorData.GetFreePos(),
	// 			actorData.AsArray(),
	// 			Caster,
	// 			sequenceSource,
	// 			null)
	// 	};
	// }
	
	

	public override void OnStart()
	{
		base.OnStart();
		if (m_syncComp != null)
		{
			m_syncComp.m_turnsAreaBuffActive = 1;
		}
	}

	public override void OnEnd()
	{
		base.OnEnd();
		if (m_syncComp != null)
		{
			m_syncComp.m_turnsAreaBuffActive = 0;
		}
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		if (m_syncComp != null)
		{
			m_syncComp.m_turnsAreaBuffActive++;
		}
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (m_ability == null)
		{
			return;
		}
		
		BoardSquare gameplayRefSquare = Board.Get().GetSquare(Caster.GetGridPos());
		Vector3 freePos = gameplayRefSquare.ToVector3();
		Vector3 damageOrigin = AreaEffectUtils.GetCenterOfShape(m_ability.GetShape(), freePos, gameplayRefSquare);
		List<ActorData> actors = AreaEffectUtils.GetActorsInShape(
			m_ability.GetShape(),
			freePos,
			gameplayRefSquare,
			m_ability.PenetrateLoS(),
			Caster,
			m_ability.GetAffectedTeams(Caster),
			null);
		foreach (ActorData hitActor in actors)
		{
			if (hitActor == Caster && m_ability.IncludeCaster())
			{
				ActorHitParameters hitParams = new ActorHitParameters(hitActor, damageOrigin);
				ActorHitResults hitResults = new ActorHitResults(hitParams);
				StandardEffectInfo effectOnCaster = m_ability.GetEffectOnCaster().GetShallowCopy();
				effectOnCaster.m_effectData.m_absorbAmount = m_ability.CalculateShieldAmount(hitActor);
				hitResults.AddStandardEffectInfo(effectOnCaster);
				hitResults.AddBaseHealing(m_ability.GetHealAmount());
				effectResults.StoreActorHit(hitResults);
			}
			else if (hitActor.GetTeam() == Caster.GetTeam())
			{
				ActorHitParameters hitParams = new ActorHitParameters(hitActor, damageOrigin);
				ActorHitResults hitResults = new ActorHitResults(hitParams);
				StandardEffectInfo effectOnAlly = m_ability.GetEffectOnAllies().GetShallowCopy();
				effectOnAlly.m_effectData.m_absorbAmount = m_ability.CalculateShieldAmount(hitActor);
				hitResults.AddStandardEffectInfo(effectOnAlly);
				hitResults.AddBaseHealing(m_ability.GetHealAmount());
				hitResults.AddTechPointGain(m_ability.GetAllyTechPointGainPerTurnActive());  // TODO CLERIC mod desc says increasing by two each turn...
				// hitResults.AddTechPointGain(1);  // TODO CLERIC ????
				effectResults.StoreActorHit(hitResults);
			}
			else if (m_ability.GetEffectOnEnemies().m_applyEffect)
			{
				ActorHitParameters hitParams = new ActorHitParameters(hitActor, damageOrigin);
				ActorHitResults hitResults = new ActorHitResults(m_ability.GetEffectOnEnemies(), hitParams);
				effectResults.StoreActorHit(hitResults);
			}
		}
	}

	public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		return true;
	}

	public override bool ShouldEndEarly()
	{
		return Caster.IsDead();  // TODO CLERIC or if no energy to continue
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (m_ability != null && forActor.GetTeam() != Caster.GetTeam())
		{
			List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(m_ability.GetShape(), TargetSquare.ToVector3(), TargetSquare, false, Caster);
			squaresToAvoid.UnionWith(squaresInShape);
		}
	}
}
#endif
