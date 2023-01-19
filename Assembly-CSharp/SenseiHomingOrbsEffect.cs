// ROGUES
// SERVER

using System.Collections.Generic;
using UnityEngine;

#if SERVER
// added in rogues
public class SenseiHomingOrbsEffect : Effect
{
	private Sensei_SyncComponent m_syncComp;
	private StandardEffectInfo m_effectOnAllyTargets;
	private StandardEffectInfo m_effectOnEnemyTargets;
	private GameObject m_buffSequencePrefab;
	private GameObject m_orbSequencePrefab;
	private int m_launchAnimationIndex;
	private float m_radius;
	private bool m_penetrateLoS;
	private bool m_includeInvisibles;
	private int m_damage;
	private int m_healing;
	private int m_healOnSelfPerHit;
	private List<Team> m_targetTeams;
	private int m_maxHitsPerVolley;
	private int m_remainingHits;
	private int m_cinematicsRequested;
	private int m_numRemainingOnTurnStart;

	public SenseiHomingOrbsEffect(
		EffectSource parent,
		BoardSquare targetSquare,
		ActorData caster,
		string effectName,
		Sensei_SyncComponent syncComp,
		float radius,
		bool penetrateLoS,
		bool includeInvisibles,
		bool hitEnemies,
		bool hitAllies,
		int damage,
		int healing,
		int healOnSelfPerHit,
		StandardEffectInfo effectOnAllyTargets,
		StandardEffectInfo effectOnEnemyTargets,
		int totalHits,
		int maxHitsPerVolley,
		int turnsActive,
		GameObject buffSequencePrefab,
		GameObject orbSequencePrefab,
		int launchAnimationIndex,
		int cinematicsRequested)
		: base(parent, targetSquare, caster, caster)
	{
		m_effectName = effectName;
		m_syncComp = syncComp;
		m_radius = radius;
		m_penetrateLoS = penetrateLoS;
		m_includeInvisibles = includeInvisibles;
		m_damage = damage;
		m_healing = healing;
		m_healOnSelfPerHit = healOnSelfPerHit;
		totalHits = Mathf.Max(0, totalHits);
		m_remainingHits = totalHits;
		m_numRemainingOnTurnStart = totalHits;
		m_maxHitsPerVolley = maxHitsPerVolley;
		m_effectOnAllyTargets = effectOnAllyTargets;
		m_effectOnEnemyTargets = effectOnEnemyTargets;
		m_time.duration = turnsActive;
		m_buffSequencePrefab = buffSequencePrefab;
		m_orbSequencePrefab = orbSequencePrefab;
		m_launchAnimationIndex = launchAnimationIndex;
		m_cinematicsRequested = cinematicsRequested;
		m_targetTeams = new List<Team>();
		if (hitEnemies)
		{
			m_targetTeams.AddRange(caster.GetOtherTeams());
		}
		if (hitAllies)
		{
			m_targetTeams.Add(caster.GetTeam());
		}
		HitPhase = AbilityPriority.Combat_Damage;
	}

	public override void OnStart()
	{
		base.OnStart();
		if (m_syncComp != null)
		{
			Sensei_SyncComponent syncComp = m_syncComp;
			syncComp.Networkm_syncCurrentNumOrbs = (sbyte)(syncComp.m_syncCurrentNumOrbs + Mathf.Max(0, m_remainingHits));
		}
	}

	public override void OnEnd()
	{
		base.OnEnd();
		if (m_syncComp != null)
		{
			Sensei_SyncComponent syncComp = m_syncComp;
			syncComp.Networkm_syncCurrentNumOrbs = (sbyte)(syncComp.m_syncCurrentNumOrbs - Mathf.Max(0, m_remainingHits));
		}
	}

	public override void OnTurnStart()
	{
		base.OnTurnStart();
		m_numRemainingOnTurnStart = m_remainingHits;
	}

	public override void OnAbilityPhaseEnd(AbilityPriority phase)
	{
		base.OnAbilityPhaseEnd(phase);
		if (phase == HitPhase)
		{
			int num = m_numRemainingOnTurnStart - m_remainingHits;
			if (num > 0 && m_syncComp != null)
			{
				sbyte networkm_syncCurrentNumOrbs = (sbyte)Mathf.Max(0, m_syncComp.m_syncCurrentNumOrbs - num);
				m_syncComp.Networkm_syncCurrentNumOrbs = networkm_syncCurrentNumOrbs;
			}
		}
	}

	public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
	{
		if (m_buffSequencePrefab != null)
		{
			return new ServerClientUtils.SequenceStartData(m_buffSequencePrefab, Caster.GetCurrentBoardSquare(), Caster.AsArray(), Caster, SequenceSource);
		}
		return null;
	}

	private List<ActorData> FindHitActors()
	{
		List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(Caster.GetFreePos(), m_radius, m_penetrateLoS, Caster, m_targetTeams, null);
		List<ActorData> list = new List<ActorData>(actorsInRadius.Count);
		foreach (ActorData actorData in actorsInRadius)
		{
			if (actorData != Caster && (m_includeInvisibles || actorData.IsActorVisibleToActor(Caster)))
			{
				list.Add(actorData);
			}
		}
		int num = Mathf.Min(m_maxHitsPerVolley, m_remainingHits);
		if (list.Count > num)
		{
			BoardSquare currentBoardSquare = Caster.GetCurrentBoardSquare();
			TargeterUtils.SortActorsByDistanceToPos(ref list, currentBoardSquare.ToVector3());
			TargeterUtils.LimitActorsToMaxNumber(ref list, num);
		}
		return list;
	}

	public override int GetCasterAnimationIndex(AbilityPriority phaseIndex)
	{
		if (phaseIndex == HitPhase && (m_cinematicsRequested > 0 || (m_effectResults.GatheredResults && m_effectResults.HitActorsArray().Length != 0)))
		{
			return m_launchAnimationIndex;
		}
		return 0;
	}

	public override int GetCinematicRequested(AbilityPriority phaseIndex)
	{
		if (GetCasterAnimationIndex(phaseIndex) > 0)
		{
			return m_cinematicsRequested;
		}
		return -1;
	}

	public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ActorData[] array = m_effectResults.HitActorsArray();
		for (int i = 0; i < array.Length; i++)
		{
			ActorData actorData = array[i];
			list.Add(new ServerClientUtils.SequenceStartData(
				m_orbSequencePrefab,
				actorData.GetCurrentBoardSquare(),
				actorData.AsArray(),
				Caster,
				SequenceSource,
				new Sequence.IExtraSequenceParams[] {
					new SplineProjectileSequence.DelayedProjectileExtraParams
					{
						startDelay = 0.25f * i,
						curIndex = i,
						maxIndex = m_maxHitsPerVolley
					}
				}));
		}
		return list;
	}

	public override bool HitsCanBeReactedTo()
	{
		return true;
	}

	public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
	{
		if (m_remainingHits > 0)
		{
			List<ActorData> list = FindHitActors();
			if (isReal)
			{
				m_remainingHits -= list.Count;
			}
			Vector3 origin = Caster.GetCurrentBoardSquare().ToVector3();
			foreach (ActorData actorData in list)
			{
				ActorHitParameters hitParams = new ActorHitParameters(actorData, origin);
				effectResults.StoreActorHit(actorData.GetTeam() == Caster.GetTeam()
					? new ActorHitResults(m_healing, HitActionType.Healing, m_effectOnAllyTargets, hitParams)
					: new ActorHitResults(m_damage, HitActionType.Damage, m_effectOnEnemyTargets, hitParams));
			}
		}
	}

	public override void GatherResultsInResponseToOutgoingActorHit(ActorHitResults outgoingHit, ref List<AbilityResults_Reaction> reactions, bool isReal)
	{
		if (m_healOnSelfPerHit > 0
		    && Caster != null
		    && outgoingHit.m_hitParameters.Effect != null
		    && outgoingHit.m_hitParameters.Effect == this
		    && outgoingHit.m_hitParameters.Target != Caster)
		{
			AbilityResults_Reaction abilityResults = new AbilityResults_Reaction();
			ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Caster, Caster.GetFreePos()));
			actorHitResults.SetBaseHealing(m_healOnSelfPerHit);
			abilityResults.SetupGameplayData(this, actorHitResults, outgoingHit.m_reactionDepth, null, isReal, outgoingHit);
			abilityResults.SetupSequenceData(SequenceLookup.Get().GetSimpleHitSequencePrefab(), Caster.GetCurrentBoardSquare(), SequenceSource);
			reactions.Add(abilityResults);
		}
	}

	public override bool ShouldEndEarly()
	{
		return base.ShouldEndEarly() || m_remainingHits <= 0;
	}

	public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
	{
		if (forActor.GetTeam() != Caster.GetTeam() && m_targetTeams.Contains(forActor.GetTeam()))
		{
			Vector3 freePos = Caster.GetFreePos();
			BoardSquare squareFromVec = Board.Get().GetSquareFromVec3(freePos);
			if (squareFromVec != null)
			{
				List<BoardSquare> squaresInRadius = AreaEffectUtils.GetSquaresInRadius(squareFromVec, m_radius + 0.5f, true, Caster);
				squaresToAvoid.UnionWith(squaresInRadius);
			}
		}
	}
}
#endif
