// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// server-only, added in rogues
#if SERVER
public class FishManBubbleEffect : StandardActorEffect
{
	private int m_turnsTillFirstExplosion;
	private int m_numExplosionsBeforeEnding;
	private AbilityAreaShape m_explosionShape;
	private bool m_explosionIgnoresLineOfSight;
	private bool m_explosionCanAffectEffectHolder;
	private GameObject m_effectSequencePrefab;
	private GameObject m_explosionSequencePrefab;
	private float m_persistentSeqRemoveDelay;
	private int m_explosionHealingToAllies;
	private int m_explosionDamageToEnemies;
	private StandardEffectInfo m_explosionEffectToAllies;
	private StandardEffectInfo m_explosionEffectToEnemies;

	public FishManBubbleEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData target,
		ActorData caster,
		StandardActorEffectData effectData,
		int turnsTillFirstExplosion,
		int numExplosionsBeforeEnding,
		GameObject effectSequencePrefab,
		float persistentSeqRemoveDelay,
		AbilityAreaShape explosionShape,
		bool explosionIgnoresLineOfSight,
		bool explosionCanAffectEffectHolder,
		GameObject explosionSequencePrefab,
		int explosionHealingToAllies,
		int explosionDamageToEnemies,
		StandardEffectInfo explosionEffectToAllies,
		StandardEffectInfo explosionEffectToEnemies)
		: base(parent, targetSquare, target, caster, effectData)
	{
		m_turnsTillFirstExplosion = turnsTillFirstExplosion;
		m_numExplosionsBeforeEnding = numExplosionsBeforeEnding;
		m_explosionShape = explosionShape;
		m_explosionIgnoresLineOfSight = explosionIgnoresLineOfSight;
		m_explosionCanAffectEffectHolder = explosionCanAffectEffectHolder;
		m_effectSequencePrefab = effectSequencePrefab;
		m_explosionSequencePrefab = explosionSequencePrefab;
		m_persistentSeqRemoveDelay = persistentSeqRemoveDelay;
		m_explosionHealingToAllies = explosionHealingToAllies;
		m_explosionDamageToEnemies = explosionDamageToEnemies;
		m_explosionEffectToAllies = explosionEffectToAllies;
		m_explosionEffectToEnemies = explosionEffectToEnemies;
		m_time.duration = turnsTillFirstExplosion + numExplosionsBeforeEnding;
		HitPhase = AbilityPriority.Combat_Damage;
	}

	private bool ShouldExplodeThisTurn()
	{
		return m_numExplosionsBeforeEnding > 0 
		       & m_time.age >= m_turnsTillFirstExplosion;
	}

	// TODO FISHMAN unused
	private bool IsLastExplosion()
	{
		return m_time.age == m_time.duration - 1;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(m_effectSequencePrefab, TargetSquare, Target.AsArray(), Caster, SequenceSource)
		};
		foreach (GameObject prefab in m_data.m_sequencePrefabs)
		{
			if (prefab != null)
			{
				list.Add(new ServerClientUtils.SequenceStartData(prefab, TargetSquare, null, Target, SequenceSource));
			}
		}
		return list;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		if (!ShouldExplodeThisTurn())
		{
			return base.GetEffectHitSeqDataList();
		}
		
		SequenceSource sequenceSource = SequenceSource.GetShallowCopy();
		if (AddActorAnimEntryIfHasHits(HitPhase))
		{
			sequenceSource.SetWaitForClientEnable(true);
		}
		return new List<ServerClientUtils.SequenceStartData>
		{
			new ServerClientUtils.SequenceStartData(
				m_explosionSequencePrefab,
				Target.GetCurrentBoardSquare(),
				m_effectResults.HitActorsArray(),
				Target,
				sequenceSource),
			new ServerClientUtils.SequenceStartData(
				SequenceLookup.Get().GetSimpleHitSequencePrefab(),
				Vector3.zero,
				null,
				Caster,
				sequenceSource,
				new SimpleTimingSequence.ExtraParams
				{
					hitDelayTime = m_persistentSeqRemoveDelay
				}.ToArray())
		};
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (!ShouldExplodeThisTurn())
		{
			base.GatherEffectResults(ref effectResults, isReal);
			return;
		}
		
		bool affectsEnemies = m_explosionDamageToEnemies > 0 || m_explosionEffectToEnemies.m_applyEffect;
		bool affectsAllies = m_explosionHealingToAllies > 0 || m_explosionEffectToAllies.m_applyEffect;
		List<Team> list = new List<Team>();
		if (affectsEnemies)
		{
			list.AddRange(Caster.GetOtherTeams());
		}
		if (affectsAllies)
		{
			list.Add(Caster.GetTeam());
		}

		List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
		List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
			m_explosionShape,
			Target.GetFreePos(),
			Target.GetCurrentBoardSquare(),
			m_explosionIgnoresLineOfSight,
			Caster,
			list,
			nonActorTargetInfo);
		if (!m_explosionCanAffectEffectHolder && actorsInShape.Contains(Target))
		{
			actorsInShape.Remove(Target);
		}

		foreach (ActorData actorData in actorsInShape)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorData, Target.GetFreePos()));
			if (actorData.GetTeam() == Caster.GetTeam())
			{
				actorHitResults.AddBaseHealing(m_explosionHealingToAllies);
				actorHitResults.AddStandardEffectInfo(m_explosionEffectToAllies);
			}
			else
			{
				actorHitResults.AddBaseDamage(m_explosionDamageToEnemies);
				actorHitResults.AddStandardEffectInfo(m_explosionEffectToEnemies);
			}

			effectResults.StoreActorHit(actorHitResults);
		}

		if (actorsInShape.Count == 0)
		{
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Target, Target.GetFreePos()));
			actorHitResults.SetIgnoreTechpointInteractionForHit(true);
			effectResults.StoreActorHit(actorHitResults);
		}

		effectResults.StoreNonActorTargetInfo(nonActorTargetInfo);
		effectResults.StorePositionHit(new PositionHitResults(new PositionHitParameters(Vector3.zero)));
	}

	public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
	{
		return Target != null
		       && ShouldExplodeThisTurn()
		       && HitPhase == phaseIndex;
	}

	public override ActorData GetActorAnimationActor()
	{
		return Target;
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() != Caster.GetTeam())
		{
			List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(
				m_explosionShape,
				Target.GetFreePos(),
				Target.GetCurrentBoardSquare(),
				true,
				Caster);
			squaresToAvoid.UnionWith(squaresInShape);
		}
	}
}
#endif
