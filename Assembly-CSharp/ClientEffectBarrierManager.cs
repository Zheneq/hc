using System;
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
		return ClientEffectBarrierManager.s_instance;
	}

	private void Awake()
	{
		ClientEffectBarrierManager.s_instance = this;
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReplayRestart);
	}

	private void OnDestroy()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ReplayRestart);
		ClientEffectBarrierManager.s_instance = null;
	}

	public void ExecuteEffectStart(ClientEffectStartData effectData)
	{
		ActorData effectTarget = effectData.m_effectTarget;
		ActorData caster = effectData.m_caster;
		bool flag = true;
		if (effectTarget != null && caster != null)
		{
			bool flag2 = effectTarget.GetTeam() == caster.GetTeam();
			ActorStatus actorStatus = effectTarget.GetActorStatus();
			if (actorStatus.HasStatus(StatusType.EffectImmune, true))
			{
				flag = false;
			}
			else
			{
				if (effectTarget != caster)
				{
					if (flag2)
					{
						if (actorStatus.HasStatus(StatusType.CantBeHelpedByTeam, true))
						{
							flag = false;
							goto IL_EF;
						}
					}
				}
				if (actorStatus.HasStatus(StatusType.BuffImmune, true))
				{
					if (effectData.m_isBuff)
					{
						flag = false;
						goto IL_EF;
					}
				}
				if (actorStatus.HasStatus(StatusType.DebuffImmune, true) && effectData.m_isDebuff)
				{
					flag = false;
				}
			}
		}
		IL_EF:
		if (!flag)
		{
			if (EffectDebugConfig.TracingAddAndRemove())
			{
				Log.Warning("<color=green>Effect</color>: Skipping CLIENT effect application for GUID " + effectData.m_effectGUID, new object[0]);
			}
			return;
		}
		if (EffectDebugConfig.TracingAddAndRemove())
		{
			if (effectData != null)
			{
				string text = "<color=green>Effect</color>: CLIENT Effect Start for guid [" + effectData.m_effectGUID + "]\n";
				using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator = effectData.m_sequenceStartDataList.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						ServerClientUtils.SequenceStartData sequenceStartData = enumerator.Current;
						string text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							"SeqPrefabId ",
							sequenceStartData.GetSequencePrefabId(),
							"\n"
						});
					}
				}
				Log.Warning(text, new object[0]);
			}
		}
		List<Sequence> list = new List<Sequence>();
		List<StatusType> list2 = new List<StatusType>();
		List<StatusType> list3 = new List<StatusType>();
		using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator2 = effectData.m_sequenceStartDataList.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				ServerClientUtils.SequenceStartData sequenceStartData2 = enumerator2.Current;
				Sequence[] array = sequenceStartData2.CreateSequencesFromData(new SequenceSource.ActorDelegate(effectData.OnClientEffectStartSequenceHitActor), new SequenceSource.Vector3Delegate(effectData.OnClientEffectStartSequenceHitPosition));
				if (array != null)
				{
					foreach (Sequence item in array)
					{
						list.Add(item);
					}
				}
			}
		}
		if (effectData.m_effectTarget != null && effectData.m_caster != null)
		{
			if (effectData.m_effectTarget.GetActorBehavior() != null)
			{
				effectData.m_effectTarget.GetActorBehavior().Client_RecordEffectFromActor(effectData.m_caster);
			}
		}
		bool flag3 = ClientResolutionManager.Get().IsInResolutionState();
		if (flag3)
		{
			if (effectData.m_effectTarget != null)
			{
				if (effectData.m_statuses != null)
				{
					if (effectData.m_statuses.Count > 0)
					{
						ActorStatus actorStatus2 = effectData.m_effectTarget.GetActorStatus();
						using (List<StatusType>.Enumerator enumerator3 = effectData.m_statuses.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								StatusType statusType = enumerator3.Current;
								actorStatus2.ClientAddStatus(statusType);
								list2.Add(statusType);
							}
						}
						using (List<StatusType>.Enumerator enumerator4 = effectData.m_statusesOnTurnStart.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								StatusType item2 = enumerator4.Current;
								list3.Add(item2);
							}
						}
					}
				}
			}
		}
		if (flag3)
		{
			if (effectData.m_effectTarget != null)
			{
				if (effectData.m_absorb != 0)
				{
					effectData.m_effectTarget.ClientUnresolvedAbsorb += effectData.m_absorb;
				}
				if (effectData.m_expectedHoT > 0)
				{
					effectData.m_effectTarget.ClientExpectedHoTTotalAdjust += effectData.m_expectedHoT;
				}
			}
		}
		if (this.m_effectGuidToData.ContainsKey(effectData.m_effectGUID))
		{
			Debug.LogError("ClientEffectBarrierManager calling ExecuteEffectStart for effect start data with guid " + effectData.m_effectGUID + ", but that guid is already in m_effectGuidToData.");
			this.m_effectGuidToData[effectData.m_effectGUID].m_sequences.AddRange(list);
		}
		else
		{
			ClientEffectData value = new ClientEffectData(list, effectData.m_effectTarget, list2);
			this.m_effectGuidToData.Add(effectData.m_effectGUID, value);
		}
	}

	public void EndEffect(int effectGuid)
	{
		if (this.m_effectGuidToData.ContainsKey(effectGuid))
		{
			ClientEffectData clientEffectData = this.m_effectGuidToData[effectGuid];
			List<Sequence> sequences = clientEffectData.m_sequences;
			using (List<Sequence>.Enumerator enumerator = sequences.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Sequence sequence = enumerator.Current;
					if (sequence != null && !sequence.MarkedForRemoval)
					{
						sequence.MarkForRemoval();
					}
				}
			}
			sequences.Clear();
			if (NetworkClient.active)
			{
				if (clientEffectData.m_target != null)
				{
					if (clientEffectData.m_statuses != null)
					{
						ActorStatus actorStatus = clientEffectData.m_target.GetActorStatus();
						foreach (StatusType status in clientEffectData.m_statuses)
						{
							actorStatus.ClientRemoveStatus(status);
						}
					}
				}
			}
			clientEffectData.m_statuses.Clear();
			this.m_effectGuidToData.Remove(effectGuid);
			if (EffectDebugConfig.TracingAddAndRemove())
			{
				Log.Warning("<color=green>Effect</color>: CLIENT Effect Remove, GUID [" + effectGuid.ToString() + "]", new object[0]);
			}
		}
	}

	public void ExecuteBarrierStart(ClientBarrierStartData barrierData)
	{
		List<Sequence> list = new List<Sequence>();
		using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator = barrierData.m_sequenceStartDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ServerClientUtils.SequenceStartData sequenceStartData = enumerator.Current;
				Sequence[] array = sequenceStartData.CreateSequencesFromData(new SequenceSource.ActorDelegate(barrierData.OnClientBarrierStartSequenceHitActor), new SequenceSource.Vector3Delegate(barrierData.OnClientBarrierStartSequenceHitPosition));
				if (array != null)
				{
					foreach (Sequence item in array)
					{
						list.Add(item);
					}
				}
			}
		}
		barrierData.m_barrierGameplayInfo.m_clientSequenceStartAttempted = true;
		if (this.m_barrierGuidToSequences.ContainsKey(barrierData.m_barrierGUID))
		{
			Debug.LogError("ClientEffectBarrierManager calling ExecuteBarrierStart for barrier start data with guid " + barrierData.m_barrierGUID + ", but that guid is already in m_barrierGuidToSequences.");
			this.m_barrierGuidToSequences[barrierData.m_barrierGUID].AddRange(list);
		}
		else
		{
			this.m_barrierGuidToSequences.Add(barrierData.m_barrierGUID, list);
		}
	}

	public void EndBarrier(int barrierGuid)
	{
		if (this.m_barrierGuidToSequences.ContainsKey(barrierGuid))
		{
			List<Sequence> list = this.m_barrierGuidToSequences[barrierGuid];
			using (List<Sequence>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Sequence sequence = enumerator.Current;
					if (sequence != null)
					{
						if (!sequence.MarkedForRemoval)
						{
							sequence.MarkForRemoval();
						}
					}
				}
			}
			list.Clear();
			BarrierManager.Get().RemoveClientBarrierInfo(barrierGuid);
			this.m_barrierGuidToSequences.Remove(barrierGuid);
		}
	}

	public void EndSequenceOfEffect(int sequencePrefabLookupId, int effectGuid, Vector3 targetPos)
	{
		if (this.m_effectGuidToData.ContainsKey(effectGuid))
		{
			List<Sequence> list = new List<Sequence>();
			List<Sequence> sequences = this.m_effectGuidToData[effectGuid].m_sequences;
			using (List<Sequence>.Enumerator enumerator = sequences.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Sequence sequence = enumerator.Current;
					if (sequence != null)
					{
						if (!sequence.MarkedForRemoval && (int)sequence.PrefabLookupId == sequencePrefabLookupId)
						{
							if (!(targetPos == Vector3.zero))
							{
								if (!(targetPos == sequence.TargetPos))
								{
									continue;
								}
							}
							list.Add(sequence);
						}
					}
				}
			}
			using (List<Sequence>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Sequence sequence2 = enumerator2.Current;
					sequence2.MarkForRemoval();
					sequences.Remove(sequence2);
				}
			}
		}
	}

	public void EndSequenceOfBarrier(int sequencePrefabLookupId, int barrierGuid, Vector3 targetPos)
	{
		if (this.m_barrierGuidToSequences.ContainsKey(barrierGuid))
		{
			List<Sequence> list = new List<Sequence>();
			List<Sequence> list2 = this.m_barrierGuidToSequences[barrierGuid];
			foreach (Sequence sequence in list2)
			{
				if (sequence != null)
				{
					if (!sequence.MarkedForRemoval)
					{
						if ((int)sequence.PrefabLookupId == sequencePrefabLookupId)
						{
							if (!(targetPos == Vector3.zero))
							{
								if (!(targetPos == sequence.TargetPos))
								{
									continue;
								}
							}
							list.Add(sequence);
						}
					}
				}
			}
			using (List<Sequence>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Sequence sequence2 = enumerator2.Current;
					sequence2.MarkForRemoval();
					list2.Remove(sequence2);
				}
			}
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType == GameEventManager.EventType.ReplayRestart)
		{
			this.m_effectGuidToData.Clear();
			this.m_barrierGuidToSequences.Clear();
		}
	}
}
