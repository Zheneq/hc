// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

// server-only
#if SERVER
public class AbilityResults_Reaction
{
	// all private in rogues
	protected List<GameObject> m_sequencePrefabs;
	protected BoardSquare m_targetSquare;
	protected ActorData m_caster;
	protected ActorData m_sequenceCaster;
	protected SequenceSource m_sequenceSource;
	protected Sequence.IExtraSequenceParams[] m_extraParams;
	protected EffectResults m_results;
	protected byte m_extraFlags;

	public AbilityResults_Reaction(
        Effect reactingEffect,
        ActorHitResults reactionHitResults,
        GameObject sequencePrefab,
        BoardSquare sequenceTargetSquare,
        SequenceSource parentSequenceSource,
        int triggeringHitDepth,
        bool isReal,
        ActorHitResults triggeringHitResults,
        Sequence.IExtraSequenceParams[] extraParams = null)
	{
		SetupSequenceData(sequencePrefab, sequenceTargetSquare, parentSequenceSource, extraParams);
		SetupGameplayData(reactingEffect, reactionHitResults, triggeringHitDepth, null, isReal, triggeringHitResults);
	}

	public AbilityResults_Reaction()
	{
	}

	public void SetupSequenceData(GameObject sequencePrefab, BoardSquare sequenceTargetSquare, SequenceSource parentSequenceSource, Sequence.IExtraSequenceParams[] extraParams = null)
	{
		m_sequencePrefabs = new List<GameObject>();
		m_sequencePrefabs.Add(sequencePrefab);
		m_targetSquare = sequenceTargetSquare;
		m_sequenceSource = new SequenceSource(new SequenceSource.ActorDelegate(OnReactionHitActor), new SequenceSource.Vector3Delegate(OnReactionHitPosition), true, parentSequenceSource, null);
		m_sequenceSource.SetWaitForClientEnable(false);
		m_extraParams = extraParams;
	}

	public void SetupGameplayData(Effect reactingEffect, ActorHitResults reactionHitResults, int triggeringHitDepth, ActorData casterOverride, bool isReal, ActorHitResults triggeringHit)
	{
		m_results = new EffectResults(reactingEffect, casterOverride, isReal, false);
		reactionHitResults.IsReaction = true;
		reactionHitResults.CanBeReactedTo = false;
		reactionHitResults.TriggeringHit = triggeringHit;
		reactionHitResults.m_reactionDepth = triggeringHitDepth + 1;
		m_results.StoreActorHit(reactionHitResults);
		m_results.GatheredResults = true;
		m_caster = reactingEffect.Caster;
		if (casterOverride != null)
		{
			m_caster = casterOverride;
		}
	}

	public void SetupGameplayData(Effect reactingEffect, List<ActorHitResults> reactionHitResultsList, int triggeringHitDepth, bool isReal)
	{
		m_results = new EffectResults(reactingEffect, null, isReal, false);
		foreach (ActorHitResults actorHitResults in reactionHitResultsList)
		{
			actorHitResults.CanBeReactedTo = false;
			actorHitResults.m_reactionDepth = triggeringHitDepth + 1;
			m_results.StoreActorHit(actorHitResults);
		}
		m_results.GatheredResults = true;
		m_caster = reactingEffect.Caster;
	}

	public void SetSequenceCaster(ActorData sequenceCaster)
	{
		m_sequenceCaster = sequenceCaster;
	}

	public Dictionary<ActorData, int> GetReactionDamageResults()
	{
		return m_results.DamageResults;
	}

	public Dictionary<ActorData, int> GetReactionDamageResults_Gross()
	{
		return m_results.DamageResults_Gross;
	}

	public virtual List<ServerClientUtils.SequenceStartData> GetReactionSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ActorData[] targetActorArray = m_results.HitActorsArray();
		ActorData actorData = m_sequenceCaster;
		if (actorData == null)
		{
			actorData = m_caster;
		}
		foreach (GameObject prefab in m_sequencePrefabs)
		{
			list.Add(new ServerClientUtils.SequenceStartData(prefab, m_targetSquare, targetActorArray, actorData, m_sequenceSource, m_extraParams));
		}
		return list;
	}

	public EffectResults GetReactionResults()
	{
		return m_results;
	}

	public void SetExtraFlag(ClientReactionResults.ExtraFlags bitToSet)
	{
		m_extraFlags |= (byte)bitToSet;
	}

	public byte GetExtraFlags()
	{
		return m_extraFlags;
	}

	public void OnReactionHitActor(ActorData target)
	{
		if (AbilityResults.DebugTraceOn)
		{
			Debug.LogWarning("<color=orange>Executing React Hit (Actor): </color>");
		}
		m_results.ExecuteForActor(target);
	}

	public void OnReactionHitPosition(Vector3 pos)
	{
		if (AbilityResults.DebugTraceOn)
		{
			Debug.LogWarning("<color=orange>Executing React Hit (Position): </color>");
		}
		m_results.ExecuteForPosition(pos);
	}

	public bool ReactionHitsDone()
	{
		return m_results.HitsDoneExecuting();
	}

	public void ExecuteUnexecutedReactionHits(bool asFailsafe)
	{
		m_results.ExecuteUnexecutedEffectHits(asFailsafe);
	}

	public void AddHitActorIds(HashSet<int> hitActorIds)
	{
		m_results.AddtHitActorIds(hitActorIds);
	}

	public string GetDebugDescription()
	{
		return "Reaction from Effect " + m_results.Effect.GetDebugIdentifier();
	}
}

// custom
// TODO looks a bit too complicated
public class AbilityResults_Reaction_PositionHitSequence : AbilityResults_Reaction
{
	private Vector3 m_targetPos;
	
	public void SetupSequenceData(
		GameObject sequencePrefab,
		Vector3 sequenceTargetPos,
		SequenceSource parentSequenceSource,
		Sequence.IExtraSequenceParams[] extraParams = null)
	{
		SetupSequenceData(sequencePrefab, null, parentSequenceSource, extraParams);
		m_targetPos = sequenceTargetPos;
	}
	
	public override List<ServerClientUtils.SequenceStartData> GetReactionSeqDataList()
	{
		List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
		ActorData[] targetActorArray = m_results.HitActorsArray();
		ActorData actorData = m_sequenceCaster;
		if (actorData == null)
		{
			actorData = m_caster;
		}
		foreach (GameObject prefab in m_sequencePrefabs)
		{
			list.Add(
				new ServerClientUtils.SequenceStartData(
					prefab,
					m_targetPos,
					targetActorArray,
					actorData,
					m_sequenceSource,
					m_extraParams));
		}
		return list;
	}
}
#endif
