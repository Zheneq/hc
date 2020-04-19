using System;
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

	public ClientAbilityResults(int casterActorIndex, int abilityAction, List<ServerClientUtils.SequenceStartData> seqStartDataList, Dictionary<ActorData, ClientActorHitResults> actorToHitResults, Dictionary<Vector3, ClientPositionHitResults> posToHitResults)
	{
		this.m_casterActor = GameFlowData.Get().FindActorByActorIndex(casterActorIndex);
		if (this.m_casterActor == null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientAbilityResults..ctor(int, int, List<ServerClientUtils.SequenceStartData>, Dictionary<ActorData, ClientActorHitResults>, Dictionary<Vector3, ClientPositionHitResults>)).MethodHandle;
			}
			Debug.LogError("ClientAbilityResults error: Actor with index " + casterActorIndex + " is null.");
			this.m_castedAbility = null;
			this.m_actionType = AbilityData.ActionType.INVALID_ACTION;
		}
		else
		{
			this.m_castedAbility = this.m_casterActor.\u000E().GetAbilityOfActionType((AbilityData.ActionType)abilityAction);
			this.m_actionType = (AbilityData.ActionType)abilityAction;
		}
		this.m_seqStartDataList = seqStartDataList;
		this.m_actorToHitResults = actorToHitResults;
		this.m_posToHitResults = posToHitResults;
	}

	public ActorData GetCaster()
	{
		return this.m_casterActor;
	}

	public AbilityData.ActionType GetSourceActionType()
	{
		return this.m_actionType;
	}

	public bool HasSequencesToStart()
	{
		if (this.m_seqStartDataList == null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientAbilityResults.HasSequencesToStart()).MethodHandle;
			}
			return false;
		}
		if (this.m_seqStartDataList.Count == 0)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			return false;
		}
		using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator = this.m_seqStartDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ServerClientUtils.SequenceStartData sequenceStartData = enumerator.Current;
				if (sequenceStartData != null)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (sequenceStartData.HasSequencePrefab())
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						return true;
					}
				}
			}
			for (;;)
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
		bool result;
		if (sequenceSource != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientAbilityResults.ContainsSequenceSource(SequenceSource)).MethodHandle;
			}
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientAbilityResults.ContainsSequenceSourceID(uint)).MethodHandle;
			}
			for (int i = 0; i < this.m_seqStartDataList.Count; i++)
			{
				if (this.m_seqStartDataList[i].ContainsSequenceSourceID(id))
				{
					for (;;)
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

	public void StartSequences()
	{
		if (this.HasSequencesToStart())
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientAbilityResults.StartSequences()).MethodHandle;
			}
			foreach (ServerClientUtils.SequenceStartData sequenceStartData in this.m_seqStartDataList)
			{
				sequenceStartData.CreateSequencesFromData(new SequenceSource.ActorDelegate(this.OnAbilityHitActor), new SequenceSource.Vector3Delegate(this.OnAbilityHitPosition));
			}
		}
		else
		{
			if (ClientAbilityResults.\u001D)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				Log.Warning(ClientAbilityResults.s_clientHitResultHeader + this.GetDebugDescription() + ": no Sequence to start, executing results directly", new object[0]);
			}
			this.RunClientAbilityHits();
		}
	}

	public void RunClientAbilityHits()
	{
		foreach (KeyValuePair<ActorData, ClientActorHitResults> keyValuePair in this.m_actorToHitResults)
		{
			this.OnAbilityHitActor(keyValuePair.Key);
		}
		using (Dictionary<Vector3, ClientPositionHitResults>.Enumerator enumerator2 = this.m_posToHitResults.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<Vector3, ClientPositionHitResults> keyValuePair2 = enumerator2.Current;
				this.OnAbilityHitPosition(keyValuePair2.Key);
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientAbilityResults.RunClientAbilityHits()).MethodHandle;
			}
		}
	}

	internal void OnAbilityHitActor(ActorData target)
	{
		if (this.m_actorToHitResults.ContainsKey(target))
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientAbilityResults.OnAbilityHitActor(ActorData)).MethodHandle;
			}
			this.m_actorToHitResults[target].ExecuteActorHit(target, this.m_casterActor);
		}
		else
		{
			Debug.LogError("ClientAbilityResults error-- Sequence hitting actor " + target.\u0018() + ", but that actor isn't in our hit results.");
		}
	}

	internal void OnAbilityHitPosition(Vector3 position)
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
		ClientResolutionAction.ExecuteUnexecutedHits(this.m_actorToHitResults, this.m_posToHitResults, this.m_casterActor);
	}

	internal void ExecuteReactionHitsWithExtraFlagsOnActor(ActorData targetActor, ActorData caster, bool hasDamage, bool hasHealing)
	{
		ClientResolutionAction.ExecuteReactionHitsWithExtraFlagsOnActorAux(this.m_actorToHitResults, targetActor, caster, hasDamage, hasHealing);
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
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientAbilityResults.MarkActorHitsAsMovementHits()).MethodHandle;
			}
		}
	}

	internal string UnexecutedHitsDebugStr()
	{
		return ClientResolutionAction.AssembleUnexecutedHitsDebugStr(this.m_actorToHitResults, this.m_posToHitResults);
	}

	internal string GetSequenceStartDataDebugStr()
	{
		string text = string.Empty;
		if (this.m_seqStartDataList != null)
		{
			foreach (ServerClientUtils.SequenceStartData sequenceStartData in this.m_seqStartDataList)
			{
				if (sequenceStartData != null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(ClientAbilityResults.GetSequenceStartDataDebugStr()).MethodHandle;
					}
					string text2 = text;
					text = string.Concat(new object[]
					{
						text2,
						"SeqStartData Actors with prefab ID ",
						sequenceStartData.GetSequencePrefabId(),
						": ",
						sequenceStartData.GetTargetActorsString(),
						"\n"
					});
				}
			}
		}
		return text;
	}

	public unsafe void AdjustKnockbackCounts_ClientAbilityResults(ref Dictionary<ActorData, int> outgoingKnockbacks, ref Dictionary<ActorData, int> incomingKnockbacks)
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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientAbilityResults.AdjustKnockbackCounts_ClientAbilityResults(Dictionary<ActorData, int>*, Dictionary<ActorData, int>*)).MethodHandle;
			}
		}
	}

	public string GetDebugDescription()
	{
		string text;
		if (this.m_casterActor != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientAbilityResults.GetDebugDescription()).MethodHandle;
			}
			text = this.m_casterActor.\u0018();
		}
		else
		{
			text = "(null actor)";
		}
		string str = text;
		string text2;
		if (this.m_castedAbility != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			text2 = this.m_castedAbility.m_abilityName;
		}
		else
		{
			text2 = "(null ability)";
		}
		string str2 = text2;
		return str + "'s " + str2;
	}

	public static bool \u001D
	{
		get
		{
			return false;
		}
	}

	public static bool \u000E
	{
		get
		{
			return false;
		}
	}
}
