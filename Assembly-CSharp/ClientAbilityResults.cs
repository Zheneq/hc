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

	public static bool LogMissingSequences => false;

	public static bool _000E => false;

	public ClientAbilityResults(int casterActorIndex, int abilityAction, List<ServerClientUtils.SequenceStartData> seqStartDataList, Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> posToHitResults)
	{
		m_casterActor = GameFlowData.Get().FindActorByActorIndex(casterActorIndex);
		if (m_casterActor == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
		if (m_seqStartDataList == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		if (m_seqStartDataList.Count == 0)
		{
			while (true)
			{
				switch (4)
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
				if (current != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (current.HasSequencePrefab())
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return false;
	}

	public bool ContainsSequenceSource(SequenceSource sequenceSource)
	{
		int result;
		if (sequenceSource != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int i = 0; i < m_seqStartDataList.Count; i++)
			{
				if (m_seqStartDataList[i].ContainsSequenceSourceID(id))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					result = true;
					break;
				}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					foreach (ServerClientUtils.SequenceStartData seqStartData in m_seqStartDataList)
					{
						seqStartData.CreateSequencesFromData(OnAbilityHitActor, OnAbilityHitPosition);
					}
					return;
				}
			}
		}
		if (LogMissingSequences)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			Log.Warning(s_clientHitResultHeader + GetDebugDescription() + ": no Sequence to start, executing results directly");
		}
		RunClientAbilityHits();
	}

	public void RunClientAbilityHits()
	{
		foreach (KeyValuePair<ActorData, ClientActorHitResults> actorToHitResult in m_actorToHitResults)
		{
			OnAbilityHitActor(actorToHitResult.Key);
		}
		using (Dictionary<Vector3, ClientPositionHitResults>.Enumerator enumerator2 = m_posToHitResults.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				OnAbilityHitPosition(enumerator2.Current.Key);
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
	}

	internal void OnAbilityHitActor(ActorData target)
	{
		if (m_actorToHitResults.ContainsKey(target))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					m_actorToHitResults[target].ExecuteActorHit(target, m_casterActor);
					return;
				}
			}
		}
		Debug.LogError("ClientAbilityResults error-- Sequence hitting actor " + target.GetDebugName() + ", but that actor isn't in our hit results.");
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
		using (Dictionary<ActorData, ClientActorHitResults>.ValueCollection.Enumerator enumerator = m_actorToHitResults.Values.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ClientActorHitResults current = enumerator.Current;
				current.IsMovementHit = true;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
	}

	internal string UnexecutedHitsDebugStr()
	{
		return ClientResolutionAction.AssembleUnexecutedHitsDebugStr(m_actorToHitResults, m_posToHitResults);
	}

	internal string GetSequenceStartDataDebugStr()
	{
		string text = string.Empty;
		if (m_seqStartDataList != null)
		{
			foreach (ServerClientUtils.SequenceStartData seqStartData in m_seqStartDataList)
			{
				if (seqStartData != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					string text2 = text;
					text = text2 + "SeqStartData Actors with prefab ID " + seqStartData.GetSequencePrefabId() + ": " + seqStartData.GetTargetActorsString() + "\n";
				}
			}
			return text;
		}
		return text;
	}

	public void AdjustKnockbackCounts_ClientAbilityResults(ref Dictionary<ActorData, int> outgoingKnockbacks, ref Dictionary<ActorData, int> incomingKnockbacks)
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
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
	}

	public string GetDebugDescription()
	{
		object obj;
		if (m_casterActor != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			obj = m_casterActor.GetDebugName();
		}
		else
		{
			obj = "(null actor)";
		}
		string str = (string)obj;
		object obj2;
		if (m_castedAbility != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			obj2 = m_castedAbility.m_abilityName;
		}
		else
		{
			obj2 = "(null ability)";
		}
		string str2 = (string)obj2;
		return str + "'s " + str2;
	}
}
