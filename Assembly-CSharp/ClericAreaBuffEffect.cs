// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// custom
public class ClericAreaBuffEffect: Effect
{
	private ClericAreaBuff m_ability;
	public Cleric_SyncComponent m_syncComp;
	private List<ActorData> m_hitActors;
    
	public ClericAreaBuffEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		ClericAreaBuff ability,
		Cleric_SyncComponent syncComp,
		List<ActorData> initialHitActors,
		int duration)
		: base(parent, targetSquare, target, caster)
	{
		m_effectName = "Cleric Area Buff Effect";
		m_time.duration = 0;
		HitPhase = AbilityPriority.Prep_Offense;
		m_ability = ability;
		m_syncComp = syncComp;
		m_hitActors = initialHitActors;
		m_time.duration = duration;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_ability.m_persistentSequencePrefab,
				TargetSquare.ToVector3(),
				m_hitActors.ToArray(),
				Caster,
				SequenceSource)
		};
	}
	
	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_ability.m_pulseSequencePrefab,
				TargetSquare.ToVector3(),
				m_hitActors.ToArray(),
				Caster,
				SequenceSource)
		};
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		m_hitActors = new List<ActorData>();
	}

	public override void OnTurnEnd()
	{
		base.OnTurnEnd();
		if (m_syncComp != null)
		{
			m_syncComp.Networkm_turnsAreaBuffActive++;
			Log.Info($"ClericAreaBuffEffect on turn end: {m_syncComp.Networkm_turnsAreaBuffActive}");
		}
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (m_ability == null)
		{
			Log.Error("Failed to gather ClericAreaBuffEffect results: no ability");
			return;
		}
		
		BoardSquare gameplayRefSquare = Board.Get().GetSquare(Caster.GetGridPos());
		Vector3 freePos = gameplayRefSquare.ToVector3();
		Vector3 damageOrigin = AreaEffectUtils.GetCenterOfShape(m_ability.GetShape(), freePos, gameplayRefSquare);
		m_hitActors = AreaEffectUtils.GetActorsInShape(
			m_ability.GetShape(),
			freePos,
			gameplayRefSquare,
			m_ability.PenetrateLoS(),
			Caster,
			m_ability.GetAffectedTeams(Caster),
			null);
		ActorHitParameters casterHitParams = new ActorHitParameters(Caster, damageOrigin);
		ActorHitResults casterHitResults = new ActorHitResults(casterHitParams);
		if (m_syncComp.m_turnsAreaBuffActive > 0)
		{
			// initial cast cost is handled by the ability itself
			Log.Info($"ClericAreaBuff effect cost: {m_ability.GetPerTurnTechPointCost()}");
			casterHitResults.AddTechPointLoss(m_ability.GetPerTurnTechPointCost());
		}
		if (m_ability.IncludeCaster())
		{
			StandardEffectInfo effectOnCaster = m_ability.GetEffectOnCaster().GetShallowCopy();
			effectOnCaster.m_effectData.m_absorbAmount = m_ability.CalculateShieldAmount(Caster);
			casterHitResults.AddStandardEffectInfo(effectOnCaster); // in replay, all these effects use neither target square nor pos nor rotation, but StandardActorEffect::GetEffectStartSeqDataList uses square
			casterHitResults.AddBaseHealing(m_ability.GetHealAmount());
		}
		foreach (ActorData hitActor in m_hitActors)
		{
			if (hitActor == Caster)
			{
				continue;
			}
			if (hitActor.GetTeam() == Caster.GetTeam() && hitActor != Caster)
			{
				ActorHitParameters hitParams = new ActorHitParameters(hitActor, damageOrigin);
				ActorHitResults hitResults = new ActorHitResults(hitParams);
				StandardEffectInfo effectOnAlly = m_ability.GetEffectOnAllies().GetShallowCopy();
				effectOnAlly.m_effectData.m_absorbAmount = m_ability.CalculateShieldAmount(hitActor);
				hitResults.AddStandardEffectInfo(effectOnAlly);
				hitResults.AddBaseHealing(m_ability.GetHealAmount());
				hitResults.AddTechPointGain(m_ability.GetAllyTechPointGainPerTurnActive() * (m_syncComp.m_turnsAreaBuffActive + 1));
				effectResults.StoreActorHit(hitResults);
			}
			else if (m_ability.GetEffectOnEnemies().m_applyEffect)
			{
				ActorHitParameters hitParams = new ActorHitParameters(hitActor, damageOrigin);
				ActorHitResults hitResults = new ActorHitResults(m_ability.GetEffectOnEnemies(), hitParams);
				effectResults.StoreActorHit(hitResults);
			}
		}
		effectResults.StoreActorHit(casterHitResults);
	}

	public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		return true;
	}

	// public override void OnTurnEnd()
	// {
	// 	base.OnTurnEnd();
	//
	// 	if (Caster == null
	// 	    || Caster.IsDead()
	// 	    || m_ability == null
	// 	    || m_ability.GetPerTurnTechPointCost() <= Caster.TechPoints)
	// 	{
	// 		m_shouldEnd = true;
	// 	}
	// }

	// public override bool ShouldEndEarly()
	// {
	// 	return m_shouldEnd;  
	// }

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
