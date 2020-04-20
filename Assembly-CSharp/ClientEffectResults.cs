using System;
using System.Collections.Generic;
using UnityEngine;

public class ClientEffectResults
{
	private int m_effectGUID;

	private ActorData m_effectCaster;

	private AbilityData.ActionType m_sourceAbilityActionType;

	private List<ServerClientUtils.SequenceStartData> m_seqStartDataList;

	private Dictionary<ActorData, ClientActorHitResults> m_actorToHitResults;

	private Dictionary<Vector3, ClientPositionHitResults> m_posToHitResults;

	public ClientEffectResults(int effectGUID, ActorData effectCaster, AbilityData.ActionType sourceAbilityActionType, List<ServerClientUtils.SequenceStartData> seqStartDataList, Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> posToHitResults)
	{
		this.m_effectGUID = effectGUID;
		this.m_sourceAbilityActionType = sourceAbilityActionType;
		this.m_effectCaster = effectCaster;
		this.m_seqStartDataList = seqStartDataList;
		this.m_actorToHitResults = actorToHitResults;
		this.m_posToHitResults = posToHitResults;
	}

	public ActorData GetCaster()
	{
		return this.m_effectCaster;
	}

	public AbilityData.ActionType GetSourceActionType()
	{
		return this.m_sourceAbilityActionType;
	}

	public bool HasSequencesToStart()
	{
		if (this.m_seqStartDataList == null)
		{
			return false;
		}
		if (this.m_seqStartDataList.Count == 0)
		{
			return false;
		}
		using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator = this.m_seqStartDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ServerClientUtils.SequenceStartData sequenceStartData = enumerator.Current;
				if (sequenceStartData != null && sequenceStartData.HasSequencePrefab())
				{
					return true;
				}
			}
		}
		return false;
	}

	public void StartSequences()
	{
		if (this.HasSequencesToStart())
		{
			foreach (ServerClientUtils.SequenceStartData sequenceStartData in this.m_seqStartDataList)
			{
				sequenceStartData.CreateSequencesFromData(new SequenceSource.ActorDelegate(this.OnEffectHitActor), new SequenceSource.Vector3Delegate(this.OnEffectHitPosition));
			}
		}
		else
		{
			if (ClientAbilityResults.LogMissingSequences)
			{
				Log.Warning(ClientAbilityResults.s_clientHitResultHeader + this.GetDebugDescription() + ": no Sequence to start, executing results directly", new object[0]);
			}
			this.RunClientEffectHits();
		}
	}

	public void RunClientEffectHits()
	{
		using (Dictionary<ActorData, ClientActorHitResults>.Enumerator enumerator = this.m_actorToHitResults.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, ClientActorHitResults> keyValuePair = enumerator.Current;
				this.OnEffectHitActor(keyValuePair.Key);
			}
		}
		using (Dictionary<Vector3, ClientPositionHitResults>.Enumerator enumerator2 = this.m_posToHitResults.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<Vector3, ClientPositionHitResults> keyValuePair2 = enumerator2.Current;
				this.OnEffectHitPosition(keyValuePair2.Key);
			}
		}
	}

	internal void OnEffectHitActor(ActorData target)
	{
		if (this.m_actorToHitResults.ContainsKey(target))
		{
			this.m_actorToHitResults[target].ExecuteActorHit(target, this.m_effectCaster);
		}
		else
		{
			Debug.LogError("ClientEffectResults error-- Sequence hitting actor " + target.GetDebugName() + ", but that actor isn't in our hit results.");
		}
	}

	internal void OnEffectHitPosition(Vector3 position)
	{
		if (this.m_posToHitResults.ContainsKey(position))
		{
			this.m_posToHitResults[position].ExecutePositionHit();
		}
	}

	internal bool DoneHitting()
	{
		return ClientResolutionAction.DoneHitting(this.m_actorToHitResults, this.m_posToHitResults);
	}

	internal bool HasUnexecutedHitOnActor(ActorData targetActor)
	{
		return ClientResolutionAction.HasUnexecutedHitOnActor(targetActor, this.m_actorToHitResults);
	}

	internal void ExecuteUnexecutedClientHits()
	{
		ClientResolutionAction.ExecuteUnexecutedHits(this.m_actorToHitResults, this.m_posToHitResults, this.m_effectCaster);
	}

	internal void ExecuteReactionHitsWithExtraFlagsOnActor(ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		ClientResolutionAction.ExecuteReactionHitsWithExtraFlagsOnActorAux(this.m_actorToHitResults, targetActor, caster, hasDamage, hasHealing);
	}

	public unsafe void AdjustKnockbackCounts_ClientEffectResults(ref Dictionary<ActorData, int> outgoingKnockbacks, ref Dictionary<ActorData, int> incomingKnockbacks)
	{
		using (Dictionary<ActorData, ClientActorHitResults>.Enumerator enumerator = this.m_actorToHitResults.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, ClientActorHitResults> keyValuePair = enumerator.Current;
				ActorData key = keyValuePair.Key;
				ClientActorHitResults value = keyValuePair.Value;
				if (value.HasKnockback)
				{
					if (!incomingKnockbacks.ContainsKey(key))
					{
						incomingKnockbacks.Add(key, 1);
					}
					else
					{
						Dictionary<ActorData, int> dictionary;
						ActorData key2;
						(dictionary = incomingKnockbacks)[key2 = key] = dictionary[key2] + 1;
					}
					if (value.KnockbackSourceActor != null)
					{
						if (!outgoingKnockbacks.ContainsKey(value.KnockbackSourceActor))
						{
							outgoingKnockbacks.Add(value.KnockbackSourceActor, 1);
						}
						else
						{
							Dictionary<ActorData, int> dictionary;
							ActorData knockbackSourceActor;
							(dictionary = outgoingKnockbacks)[knockbackSourceActor = value.KnockbackSourceActor] = dictionary[knockbackSourceActor] + 1;
						}
					}
				}
			}
		}
	}

	public bool ContainsSequenceSource(SequenceSource sequenceSource)
	{
		bool result;
		if (sequenceSource != null)
		{
			result = this.ContainsSequenceSourceID(sequenceSource.RootID);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public bool ContainsSequenceSourceID(uint id)
	{
		bool result = false;
		if (this.m_seqStartDataList != null)
		{
			for (int i = 0; i < this.m_seqStartDataList.Count; i++)
			{
				if (this.m_seqStartDataList[i].ContainsSequenceSourceID(id))
				{
					return true;
				}
			}
		}
		return result;
	}

	public bool HasReactionByCaster(ActorData caster)
	{
		return ClientResolutionAction.HasReactionHitByCaster(caster, this.m_actorToHitResults);
	}

	public void GetReactionHitResultsByCaster(ActorData caster, out Dictionary<ActorData, ClientActorHitResults> reactionActorHits, out Dictionary<Vector3, ClientPositionHitResults> reactionPosHits)
	{
		ClientResolutionAction.GetReactionHitResultsByCaster(caster, this.m_actorToHitResults, out reactionActorHits, out reactionPosHits);
	}

	public Dictionary<ActorData, ClientActorHitResults> GetActorHitResults()
	{
		return this.m_actorToHitResults;
	}

	public Dictionary<Vector3, ClientPositionHitResults> GetPosHitResults()
	{
		return this.m_posToHitResults;
	}

	public void MarkActorHitsAsMovementHits()
	{
		using (Dictionary<ActorData, ClientActorHitResults>.ValueCollection.Enumerator enumerator = this.m_actorToHitResults.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ClientActorHitResults clientActorHitResults = enumerator.Current;
				clientActorHitResults.IsMovementHit = true;
			}
		}
	}

	public string GetDebugDescription()
	{
		return this.m_effectCaster.GetDebugName() + "'s effect, guid = " + this.m_effectGUID;
	}

	internal string UnexecutedHitsDebugStr()
	{
		return ClientResolutionAction.AssembleUnexecutedHitsDebugStr(this.m_actorToHitResults, this.m_posToHitResults);
	}
}
