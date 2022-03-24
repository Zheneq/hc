using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientEffectBarrierManager : MonoBehaviour, IGameEventListener
{
	private static ClientEffectBarrierManager s_instance;

	private Dictionary<int, ClientEffectData> m_effectGuidToData = new Dictionary<int, ClientEffectData>();
	private Dictionary<int, List<Sequence>> m_barrierGuidToSequences = new Dictionary<int, List<Sequence>>();

	public static ClientEffectBarrierManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReplayRestart);
	}

	private void OnDestroy()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ReplayRestart);
		s_instance = null;
	}

	public void ExecuteEffectStart(ClientEffectStartData effectData)
	{
		ActorData effectTarget = effectData.m_effectTarget;
		ActorData caster = effectData.m_caster;
		bool canApply = true;
		if (effectTarget != null && caster != null)
		{
			ActorStatus actorStatus = effectTarget.GetActorStatus();
			if (actorStatus.HasStatus(StatusType.EffectImmune))
			{
				canApply = false;
			}
			else if (effectTarget != caster
				&& effectTarget.GetTeam() == caster.GetTeam()
				&& actorStatus.HasStatus(StatusType.CantBeHelpedByTeam))
			{
				canApply = false;
			}
			else if (actorStatus.HasStatus(StatusType.BuffImmune) && effectData.m_isBuff)
			{
				canApply = false;
			}
			else if (actorStatus.HasStatus(StatusType.DebuffImmune) && effectData.m_isDebuff)
			{
				canApply = false;
			}
		}
		if (!canApply)
		{
			if (EffectDebugConfig.TracingAddAndRemove())
			{
				Log.Warning("<color=green>Effect</color>: Skipping CLIENT effect application for GUID " + effectData.m_effectGUID);
			}
			return;
		}
		if (EffectDebugConfig.TracingAddAndRemove() && effectData != null)
		{
			string text = "<color=green>Effect</color>: CLIENT Effect Start for guid [" + effectData.m_effectGUID + "]\n";
			foreach (ServerClientUtils.SequenceStartData seqStartData in effectData.m_sequenceStartDataList)
			{
				text += "SeqPrefabId " + seqStartData.GetSequencePrefabId() + "\n";
			}
			Log.Warning(text);
		}
		List<Sequence> sequences = new List<Sequence>();
		List<StatusType> statuses = new List<StatusType>();
		List<StatusType> statusesOnTurnStart = new List<StatusType>();
		foreach (ServerClientUtils.SequenceStartData seqStartData in effectData.m_sequenceStartDataList)
		{
			Sequence[] sequncesFromData = seqStartData.CreateSequencesFromData(
				effectData.OnClientEffectStartSequenceHitActor,
				effectData.OnClientEffectStartSequenceHitPosition);
			if (sequncesFromData != null)
			{
				foreach (Sequence item in sequncesFromData)
				{
					sequences.Add(item);
				}
			}
		}
		if (effectData.m_effectTarget != null
			&& effectData.m_caster != null
			&& effectData.m_effectTarget.GetActorBehavior() != null)
		{
			effectData.m_effectTarget.GetActorBehavior().Client_RecordEffectFromActor(effectData.m_caster);
		}
		bool flag3 = ClientResolutionManager.Get().IsInResolutionState();
		if (flag3 && effectData.m_effectTarget != null)
		{
			if (effectData.m_statuses != null && effectData.m_statuses.Count > 0)
			{
				ActorStatus actorStatus = effectData.m_effectTarget.GetActorStatus();
				foreach (StatusType status in effectData.m_statuses)
				{
					actorStatus.ClientAddStatus(status);
					statuses.Add(status);
				}
				foreach (StatusType status in effectData.m_statusesOnTurnStart)
				{
					statusesOnTurnStart.Add(status);
				}
			}
			if (effectData.m_absorb != 0)
			{
				effectData.m_effectTarget.ClientUnresolvedAbsorb += effectData.m_absorb;
			}
			if (effectData.m_expectedHoT > 0)
			{
				effectData.m_effectTarget.ClientExpectedHoTTotalAdjust += effectData.m_expectedHoT;
			}
		}
		if (!m_effectGuidToData.ContainsKey(effectData.m_effectGUID))
		{
			ClientEffectData value = new ClientEffectData(sequences, effectData.m_effectTarget, statuses);
			m_effectGuidToData.Add(effectData.m_effectGUID, value);
		}
		else
		{
			Debug.LogError("ClientEffectBarrierManager calling ExecuteEffectStart for effect start data with guid " + effectData.m_effectGUID + ", but that guid is already in m_effectGuidToData.");
			m_effectGuidToData[effectData.m_effectGUID].m_sequences.AddRange(sequences);
		}
	}

	public void EndEffect(int effectGuid)
	{
		if (m_effectGuidToData.ContainsKey(effectGuid))
		{
			ClientEffectData clientEffectData = m_effectGuidToData[effectGuid];
			Log.Warning($"EndEffect: {DefaultJsonSerializer.Serialize(clientEffectData)}");
			List<Sequence> sequences = clientEffectData.m_sequences;
			foreach (Sequence sequence in sequences)
			{
				if (sequence != null && !sequence.MarkedForRemoval)
				{
					sequence.MarkForRemoval();
				}
			}
			sequences.Clear();
			if (NetworkClient.active
				&& clientEffectData.m_target != null
				&& clientEffectData.m_statuses != null)
			{
				ActorStatus actorStatus = clientEffectData.m_target.GetActorStatus();
				foreach (StatusType status in clientEffectData.m_statuses)
				{
					actorStatus.ClientRemoveStatus(status);
				}
			}
			clientEffectData.m_statuses.Clear();
			m_effectGuidToData.Remove(effectGuid);
			if (EffectDebugConfig.TracingAddAndRemove())
			{
				Log.Warning("<color=green>Effect</color>: CLIENT Effect Remove, GUID [" + effectGuid + "]");
			}
		}
		else
		{
			Log.Warning("<color=red>Effect</color>: CLIENT Effect Remove, GUID [" + effectGuid + "] not found!");
		}
	}

	public void ExecuteBarrierStart(ClientBarrierStartData barrierData)
	{
		List<Sequence> sequences = new List<Sequence>();
		foreach (ServerClientUtils.SequenceStartData seqStartData in barrierData.m_sequenceStartDataList)
		{
			Sequence[] sequncesFromData = seqStartData.CreateSequencesFromData(
				barrierData.OnClientBarrierStartSequenceHitActor,
				barrierData.OnClientBarrierStartSequenceHitPosition);
			if (sequncesFromData != null)
			{
				foreach (Sequence sequence in sequncesFromData)
				{
					sequences.Add(sequence);
				}
			}
		}
		barrierData.m_barrierGameplayInfo.m_clientSequenceStartAttempted = true;
		if (!m_barrierGuidToSequences.ContainsKey(barrierData.m_barrierGUID))
		{
			m_barrierGuidToSequences.Add(barrierData.m_barrierGUID, sequences);
		}
		else
		{
			Debug.LogError("ClientEffectBarrierManager calling ExecuteBarrierStart for barrier start data with guid " + barrierData.m_barrierGUID + ", but that guid is already in m_barrierGuidToSequences.");
			m_barrierGuidToSequences[barrierData.m_barrierGUID].AddRange(sequences);
		}
	}

	public void EndBarrier(int barrierGuid)
	{
		if (m_barrierGuidToSequences.ContainsKey(barrierGuid))
		{
			List<Sequence> sequences = m_barrierGuidToSequences[barrierGuid];
			foreach (Sequence sequence in sequences)
			{
				if (sequence != null && !sequence.MarkedForRemoval)
				{
					sequence.MarkForRemoval();
				}
			}
			sequences.Clear();
			BarrierManager.Get().RemoveClientBarrierInfo(barrierGuid);
			m_barrierGuidToSequences.Remove(barrierGuid);
		}
	}

	public void EndSequenceOfEffect(int sequencePrefabLookupId, int effectGuid, Vector3 targetPos)
	{
		if (m_effectGuidToData.ContainsKey(effectGuid))
		{
			List<Sequence> sequencesToRemove = new List<Sequence>();
			List<Sequence> sequences = m_effectGuidToData[effectGuid].m_sequences;
			foreach (Sequence sequence in sequences)
			{
				if (sequence != null
					&& !sequence.MarkedForRemoval
					&& sequence.PrefabLookupId == sequencePrefabLookupId)
				{
					if (targetPos == Vector3.zero || targetPos == sequence.TargetPos)
					{
						sequencesToRemove.Add(sequence);
					}
				}
			}
			foreach (Sequence sequence in sequencesToRemove)
			{
				sequence.MarkForRemoval();
				sequences.Remove(sequence);
			}
		}
	}

	public void EndSequenceOfBarrier(int sequencePrefabLookupId, int barrierGuid, Vector3 targetPos)
	{
		if (m_barrierGuidToSequences.ContainsKey(barrierGuid))
		{
			List<Sequence> sequencesToRemove = new List<Sequence>();
			List<Sequence> sequences = m_barrierGuidToSequences[barrierGuid];
			foreach (Sequence sequence in sequences)
			{
				if (sequence != null
					&& !sequence.MarkedForRemoval
					&& sequence.PrefabLookupId == sequencePrefabLookupId)
				{
					if (targetPos == Vector3.zero || targetPos == sequence.TargetPos)
					{
						sequencesToRemove.Add(sequence);
					}
				}
			}
			foreach (Sequence sequence in sequencesToRemove)
			{
				sequence.MarkForRemoval();
				sequences.Remove(sequence);
			}
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.ReplayRestart)
		{
			m_effectGuidToData.Clear();
			m_barrierGuidToSequences.Clear();
		}
	}
}
