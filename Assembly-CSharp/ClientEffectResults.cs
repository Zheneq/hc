using System.Collections.Generic;
using UnityEngine;

public class ClientEffectResults
{
	public int m_effectGUID;

	public ActorData m_effectCaster;

	public AbilityData.ActionType m_sourceAbilityActionType;

	public List<ServerClientUtils.SequenceStartData> m_seqStartDataList;

	public Dictionary<ActorData, ClientActorHitResults> m_actorToHitResults;

	public Dictionary<Vector3, ClientPositionHitResults> m_posToHitResults;

	public ClientEffectResults(int effectGUID, ActorData effectCaster, AbilityData.ActionType sourceAbilityActionType, List<ServerClientUtils.SequenceStartData> seqStartDataList, Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> posToHitResults)
	{
		m_effectGUID = effectGUID;
		m_sourceAbilityActionType = sourceAbilityActionType;
		m_effectCaster = effectCaster;
		m_seqStartDataList = seqStartDataList;
		m_actorToHitResults = actorToHitResults;
		m_posToHitResults = posToHitResults;
	}

	public ActorData GetCaster()
	{
		return m_effectCaster;
	}

	public AbilityData.ActionType GetSourceActionType()
	{
		return m_sourceAbilityActionType;
	}

	public bool HasSequencesToStart()
	{
		if (m_seqStartDataList == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		if (m_seqStartDataList.Count == 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator = m_seqStartDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ServerClientUtils.SequenceStartData current = enumerator.Current;
				if (current != null && current.HasSequencePrefab())
				{
					return true;
				}
			}
		}
		return false;
	}

	public void StartSequences()
	{
		if (HasSequencesToStart())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					foreach (ServerClientUtils.SequenceStartData seqStartData in m_seqStartDataList)
					{
						seqStartData.CreateSequencesFromData(OnEffectHitActor, OnEffectHitPosition);
					}
					return;
				}
			}
		}
		if (ClientAbilityResults.DebugTraceOn)
		{
			Log.Warning(ClientAbilityResults.s_clientHitResultHeader + GetDebugDescription() + ": no Sequence to start, executing results directly");
		}
		RunClientEffectHits();
	}

	public void RunClientEffectHits()
	{
		using (Dictionary<ActorData, ClientActorHitResults>.Enumerator enumerator = m_actorToHitResults.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				OnEffectHitActor(enumerator.Current.Key);
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					goto end_IL_000e;
				}
			}
			end_IL_000e:;
		}
		using (Dictionary<Vector3, ClientPositionHitResults>.Enumerator enumerator2 = m_posToHitResults.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				OnEffectHitPosition(enumerator2.Current.Key);
			}
			while (true)
			{
				switch (5)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	internal void OnEffectHitActor(ActorData target)
	{
		if (m_actorToHitResults.ContainsKey(target))
		{
			m_actorToHitResults[target].ExecuteActorHit(target, m_effectCaster);
		}
		else
		{
			Debug.LogError("ClientEffectResults error-- Sequence hitting actor " + target.DebugNameString() + ", but that actor isn't in our hit results.");
		}
	}

	internal void OnEffectHitPosition(Vector3 position)
	{
		if (!m_posToHitResults.ContainsKey(position))
		{
			return;
		}
		while (true)
		{
			m_posToHitResults[position].ExecutePositionHit();
			return;
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
		ClientResolutionAction.ExecuteUnexecutedHits(m_actorToHitResults, m_posToHitResults, m_effectCaster);
	}

	internal void ExecuteReactionHitsWithExtraFlagsOnActor(ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		ClientResolutionAction.ExecuteReactionHitsWithExtraFlagsOnActorAux(m_actorToHitResults, targetActor, caster, hasDamage, hasHealing);
	}

	public void AdjustKnockbackCounts_ClientEffectResults(ref Dictionary<ActorData, int> outgoingKnockbacks, ref Dictionary<ActorData, int> incomingKnockbacks)
	{
		using (Dictionary<ActorData, ClientActorHitResults>.Enumerator enumerator = m_actorToHitResults.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<ActorData, ClientActorHitResults> current = enumerator.Current;
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
			while (true)
			{
				switch (5)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public bool ContainsSequenceSource(SequenceSource sequenceSource)
	{
		int result;
		if (sequenceSource != null)
		{
			result = (ContainsSequenceSourceID(sequenceSource.RootID) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		return (byte)result != 0;
	}

	public bool ContainsSequenceSourceID(uint id)
	{
		bool result = false;
		if (m_seqStartDataList != null)
		{
			int num = 0;
			while (true)
			{
				if (num < m_seqStartDataList.Count)
				{
					if (m_seqStartDataList[num].ContainsSequenceSourceID(id))
					{
						result = true;
						break;
					}
					num++;
					continue;
				}
				break;
			}
		}
		return result;
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

	public void MarkActorHitsAsMovementHits()
	{
		using (Dictionary<ActorData, ClientActorHitResults>.ValueCollection.Enumerator enumerator = m_actorToHitResults.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ClientActorHitResults current = enumerator.Current;
				current.IsMovementHit = true;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
	}

	public string GetDebugDescription()
	{
		return m_effectCaster.DebugNameString() + "'s effect, guid = " + m_effectGUID;
	}

	internal string UnexecutedHitsDebugStr()
	{
		return ClientResolutionAction.AssembleUnexecutedHitsDebugStr(m_actorToHitResults, m_posToHitResults);
	}
}
