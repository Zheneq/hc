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
		bool flag = true;
		if (effectTarget != null && caster != null)
		{
			bool flag2 = effectTarget.GetTeam() == caster.GetTeam();
			ActorStatus actorStatus = effectTarget.GetActorStatus();
			if (actorStatus.HasStatus(StatusType.EffectImmune))
			{
				flag = false;
			}
			else
			{
				if (effectTarget != caster)
				{
					if (flag2)
					{
						if (actorStatus.HasStatus(StatusType.CantBeHelpedByTeam))
						{
							flag = false;
							goto IL_00ef;
						}
					}
				}
				if (actorStatus.HasStatus(StatusType.BuffImmune))
				{
					if (effectData.m_isBuff)
					{
						flag = false;
						goto IL_00ef;
					}
				}
				if (actorStatus.HasStatus(StatusType.DebuffImmune) && effectData.m_isDebuff)
				{
					flag = false;
				}
			}
		}
		goto IL_00ef;
		IL_00ef:
		if (!flag)
		{
			if (EffectDebugConfig.TracingAddAndRemove())
			{
				Log.Warning("<color=green>Effect</color>: Skipping CLIENT effect application for GUID " + effectData.m_effectGUID);
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
						ServerClientUtils.SequenceStartData current = enumerator.Current;
						string text2 = text;
						text = text2 + "SeqPrefabId " + current.GetSequencePrefabId() + "\n";
					}
				}
				Log.Warning(text);
			}
		}
		List<Sequence> list = new List<Sequence>();
		List<StatusType> list2 = new List<StatusType>();
		List<StatusType> list3 = new List<StatusType>();
		using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator2 = effectData.m_sequenceStartDataList.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				ServerClientUtils.SequenceStartData current2 = enumerator2.Current;
				Sequence[] array = current2.CreateSequencesFromData(effectData.OnClientEffectStartSequenceHitActor, effectData.OnClientEffectStartSequenceHitPosition);
				if (array != null)
				{
					Sequence[] array2 = array;
					foreach (Sequence item in array2)
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
								StatusType current3 = enumerator3.Current;
								actorStatus2.ClientAddStatus(current3);
								list2.Add(current3);
							}
						}
						using (List<StatusType>.Enumerator enumerator4 = effectData.m_statusesOnTurnStart.GetEnumerator())
						{
							while (enumerator4.MoveNext())
							{
								StatusType current4 = enumerator4.Current;
								list3.Add(current4);
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
		if (m_effectGuidToData.ContainsKey(effectData.m_effectGUID))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					Debug.LogError("ClientEffectBarrierManager calling ExecuteEffectStart for effect start data with guid " + effectData.m_effectGUID + ", but that guid is already in m_effectGuidToData.");
					m_effectGuidToData[effectData.m_effectGUID].m_sequences.AddRange(list);
					return;
				}
			}
		}
		ClientEffectData value = new ClientEffectData(list, effectData.m_effectTarget, list2);
		m_effectGuidToData.Add(effectData.m_effectGUID, value);
	}

	public void EndEffect(int effectGuid)
	{
		if (!m_effectGuidToData.ContainsKey(effectGuid))
		{
			return;
		}
		while (true)
		{
			ClientEffectData clientEffectData = m_effectGuidToData[effectGuid];
			List<Sequence> sequences = clientEffectData.m_sequences;
			using (List<Sequence>.Enumerator enumerator = sequences.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Sequence current = enumerator.Current;
					if (current != null && !current.MarkedForRemoval)
					{
						current.MarkForRemoval();
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
			m_effectGuidToData.Remove(effectGuid);
			if (EffectDebugConfig.TracingAddAndRemove())
			{
				Log.Warning("<color=green>Effect</color>: CLIENT Effect Remove, GUID [" + effectGuid + "]");
			}
			return;
		}
	}

	public void ExecuteBarrierStart(ClientBarrierStartData barrierData)
	{
		List<Sequence> list = new List<Sequence>();
		using (List<ServerClientUtils.SequenceStartData>.Enumerator enumerator = barrierData.m_sequenceStartDataList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ServerClientUtils.SequenceStartData current = enumerator.Current;
				Sequence[] array = current.CreateSequencesFromData(barrierData.OnClientBarrierStartSequenceHitActor, barrierData.OnClientBarrierStartSequenceHitPosition);
				if (array != null)
				{
					Sequence[] array2 = array;
					foreach (Sequence item in array2)
					{
						list.Add(item);
					}
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					goto end_IL_0012;
				}
			}
			end_IL_0012:;
		}
		barrierData.m_barrierGameplayInfo.m_clientSequenceStartAttempted = true;
		if (m_barrierGuidToSequences.ContainsKey(barrierData.m_barrierGUID))
		{
			Debug.LogError("ClientEffectBarrierManager calling ExecuteBarrierStart for barrier start data with guid " + barrierData.m_barrierGUID + ", but that guid is already in m_barrierGuidToSequences.");
			m_barrierGuidToSequences[barrierData.m_barrierGUID].AddRange(list);
		}
		else
		{
			m_barrierGuidToSequences.Add(barrierData.m_barrierGUID, list);
		}
	}

	public void EndBarrier(int barrierGuid)
	{
		if (!m_barrierGuidToSequences.ContainsKey(barrierGuid))
		{
			return;
		}
		while (true)
		{
			List<Sequence> list = m_barrierGuidToSequences[barrierGuid];
			using (List<Sequence>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Sequence current = enumerator.Current;
					if (current != null)
					{
						if (!current.MarkedForRemoval)
						{
							current.MarkForRemoval();
						}
					}
				}
			}
			list.Clear();
			BarrierManager.Get().RemoveClientBarrierInfo(barrierGuid);
			m_barrierGuidToSequences.Remove(barrierGuid);
			return;
		}
	}

	public void EndSequenceOfEffect(int sequencePrefabLookupId, int effectGuid, Vector3 targetPos)
	{
		if (!m_effectGuidToData.ContainsKey(effectGuid))
		{
			return;
		}
		while (true)
		{
			List<Sequence> list = new List<Sequence>();
			List<Sequence> sequences = m_effectGuidToData[effectGuid].m_sequences;
			using (List<Sequence>.Enumerator enumerator = sequences.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Sequence current = enumerator.Current;
					if (current != null)
					{
						if (!current.MarkedForRemoval && current.PrefabLookupId == sequencePrefabLookupId)
						{
							if (!(targetPos == Vector3.zero))
							{
								if (!(targetPos == current.TargetPos))
								{
									continue;
								}
							}
							list.Add(current);
						}
					}
				}
			}
			using (List<Sequence>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Sequence current2 = enumerator2.Current;
					current2.MarkForRemoval();
					sequences.Remove(current2);
				}
				while (true)
				{
					switch (7)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	public void EndSequenceOfBarrier(int sequencePrefabLookupId, int barrierGuid, Vector3 targetPos)
	{
		if (!m_barrierGuidToSequences.ContainsKey(barrierGuid))
		{
			return;
		}
		while (true)
		{
			List<Sequence> list = new List<Sequence>();
			List<Sequence> list2 = m_barrierGuidToSequences[barrierGuid];
			foreach (Sequence item in list2)
			{
				if (item != null)
				{
					if (!item.MarkedForRemoval)
					{
						if (item.PrefabLookupId == sequencePrefabLookupId)
						{
							if (!(targetPos == Vector3.zero))
							{
								if (!(targetPos == item.TargetPos))
								{
									continue;
								}
							}
							list.Add(item);
						}
					}
				}
			}
			using (List<Sequence>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					Sequence current2 = enumerator2.Current;
					current2.MarkForRemoval();
					list2.Remove(current2);
				}
				while (true)
				{
					switch (6)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (eventType != GameEventManager.EventType.ReplayRestart)
		{
			return;
		}
		while (true)
		{
			m_effectGuidToData.Clear();
			m_barrierGuidToSequences.Clear();
			return;
		}
	}
}
