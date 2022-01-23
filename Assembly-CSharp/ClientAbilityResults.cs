using System.Collections.Generic;
using UnityEngine;

public class ClientAbilityResults
{
	private ActorData m_casterActor;
	private Ability m_castedAbility;
	private AbilityData.ActionType m_actionType;
	private List<ServerClientUtils.SequenceStartData> m_seqStartDataList;
	private Dictionary<ActorData, ClientActorHitResults> m_actorToHitResults;
	private Dictionary<Vector3, ClientPositionHitResults> m_posToHitResults;

	public static string s_storeActorHitHeader = "<color=cyan>Storing ClientActorHitResult: </color>";
	public static string s_storePositionHitHeader = "<color=cyan>Storing ClientPositionHitResult: </color>";
	public static string s_executeActorHitHeader = "<color=green>Executing ClientActorHitResult: </color>";
	public static string s_executePositionHitHeader = "<color=green>Executing ClientPositionHitResults: </color>";
	public static string s_clientResolutionNetMsgHeader = "<color=white>ClientResolution NetworkMessage: </color>";
	public static string s_clientHitResultHeader = "<color=yellow>ClientHitResults: </color>";

	public static bool DebugTraceOn => false;
	public static bool DebugSerializeSizeOn => false;

	public ClientAbilityResults(int casterActorIndex, int abilityAction, List<ServerClientUtils.SequenceStartData> seqStartDataList, Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> posToHitResults)
	{
		m_casterActor = GameFlowData.Get().FindActorByActorIndex(casterActorIndex);
		if (m_casterActor == null)
		{
			Debug.LogError("ClientAbilityResults error: Actor with index " + casterActorIndex + " is null.");
			m_castedAbility = null;
			m_actionType = AbilityData.ActionType.INVALID_ACTION;
		}
		else
		{
			m_castedAbility = m_casterActor.GetAbilityData().GetAbilityOfActionType((AbilityData.ActionType)abilityAction);
			m_actionType = (AbilityData.ActionType)abilityAction;
		}
		m_seqStartDataList = seqStartDataList;
		m_actorToHitResults = actorToHitResults;
		m_posToHitResults = posToHitResults;
	}

	public ActorData GetCaster()
	{
		return m_casterActor;
	}

	public AbilityData.ActionType GetSourceActionType()
	{
		return m_actionType;
	}

	public bool HasSequencesToStart()
	{
		if (m_seqStartDataList != null && m_seqStartDataList.Count != 0)
		{
			foreach (ServerClientUtils.SequenceStartData current in m_seqStartDataList)
			{
				if (current != null && current.HasSequencePrefab())
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool ContainsSequenceSource(SequenceSource sequenceSource)
	{
		return sequenceSource != null && ContainsSequenceSourceID(sequenceSource.RootID);
	}

	public bool ContainsSequenceSourceID(uint id)
	{
		if (m_seqStartDataList != null)
		{
			foreach (ServerClientUtils.SequenceStartData ssd in m_seqStartDataList)
			{
				if (ssd.ContainsSequenceSourceID(id))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool HasReactionByCaster(ActorData caster)
	{
		return ClientResolutionAction.HasReactionHitByCaster(caster, m_actorToHitResults);
	}

	public void GetReactionHitResultsByCaster(ActorData caster, out Dictionary<ActorData, ClientActorHitResults> reactionActorHits, out Dictionary<Vector3, ClientPositionHitResults> reactionPosHits)
	{
		ClientResolutionAction.GetReactionHitResultsByCaster(caster, m_actorToHitResults, out reactionActorHits, out reactionPosHits);
	}

	public Dictionary<ActorData, ClientActorHitResults> GetActorHitResults()
	{
		return m_actorToHitResults;
	}

	public Dictionary<Vector3, ClientPositionHitResults> GetPosHitResults()
	{
		return m_posToHitResults;
	}

	public void StartSequences()
	{
		if (HasSequencesToStart())
		{
			foreach (ServerClientUtils.SequenceStartData seqStartData in m_seqStartDataList)
			{
				seqStartData.CreateSequencesFromData(OnAbilityHitActor, OnAbilityHitPosition);
			}
		}
		else
		{
			if (DebugTraceOn)
			{
				Log.Warning(s_clientHitResultHeader + GetDebugDescription() + ": no Sequence to start, executing results directly");
			}
			RunClientAbilityHits();
		}
	}

	public void RunClientAbilityHits()
	{
		foreach (ActorData target in m_actorToHitResults.Keys)
		{
			OnAbilityHitActor(target);
		}
		foreach (Vector3 position in m_posToHitResults.Keys)
		{
			OnAbilityHitPosition(position);
		}
	}

	internal void OnAbilityHitActor(ActorData target)
	{
		if (m_actorToHitResults.ContainsKey(target))
		{
			m_actorToHitResults[target].ExecuteActorHit(target, m_casterActor);
		}
		else
		{
			Debug.LogError("ClientAbilityResults error-- Sequence hitting actor " + target.DebugNameString() + ", but that actor isn't in our hit results.");
		}
	}

	internal void OnAbilityHitPosition(Vector3 position)
	{
		if (m_posToHitResults.ContainsKey(position))
		{
			m_posToHitResults[position].ExecutePositionHit();
		}
	}

	internal bool DoneHitting()
	{
		return ClientResolutionAction.DoneHitting(m_actorToHitResults, m_posToHitResults);
	}

	internal bool HasUnexecutedHitOnActor(ActorData targetActor)
	{
		return ClientResolutionAction.HasUnexecutedHitOnActor(targetActor, m_actorToHitResults);
	}

	internal void ExecuteUnexecutedClientHits()
	{
		ClientResolutionAction.ExecuteUnexecutedHits(m_actorToHitResults, m_posToHitResults, m_casterActor);
	}

	internal void ExecuteReactionHitsWithExtraFlagsOnActor(ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		ClientResolutionAction.ExecuteReactionHitsWithExtraFlagsOnActorAux(m_actorToHitResults, targetActor, caster, hasDamage, hasHealing);
	}

	public void MarkActorHitsAsMovementHits()
	{
		foreach (ClientActorHitResults current in m_actorToHitResults.Values)
		{
			current.IsMovementHit = true;
		}
	}

	internal string UnexecutedHitsDebugStr()
	{
		return ClientResolutionAction.AssembleUnexecutedHitsDebugStr(m_actorToHitResults, m_posToHitResults);
	}

	internal string GetSequenceStartDataDebugStr()
	{
		string text = "";
		if (m_seqStartDataList != null)
		{
			foreach (ServerClientUtils.SequenceStartData seqStartData in m_seqStartDataList)
			{
				if (seqStartData != null)
				{
					text += "SeqStartData Actors with prefab ID " + seqStartData.GetSequencePrefabId() + ": " + seqStartData.GetTargetActorsString() + "\n";
				}
			}
		}
		return text;
	}

	public void AdjustKnockbackCounts_ClientAbilityResults(ref Dictionary<ActorData, int> outgoingKnockbacks, ref Dictionary<ActorData, int> incomingKnockbacks)
	{
		foreach (KeyValuePair<ActorData, ClientActorHitResults> current in m_actorToHitResults)
		{
			ActorData key = current.Key;
			ClientActorHitResults value = current.Value;
			if (value.HasKnockback)
			{
				if (!incomingKnockbacks.ContainsKey(key))
				{
					incomingKnockbacks.Add(key, 1);
				}
				else
				{
					incomingKnockbacks[key]++;
				}
				if (value.KnockbackSourceActor != null)
				{
					if (!outgoingKnockbacks.ContainsKey(value.KnockbackSourceActor))
					{
						outgoingKnockbacks.Add(value.KnockbackSourceActor, 1);
					}
					else
					{
						outgoingKnockbacks[value.KnockbackSourceActor]++;
					}
				}
			}
		}
	}

	public string GetDebugDescription()
	{
		string actor = m_casterActor != null ? m_casterActor.DebugNameString() : "(null actor)";
		string ability = m_castedAbility != null ? m_castedAbility.m_abilityName : "(null ability)";
		return actor + "'s " + ability;
	}
}
