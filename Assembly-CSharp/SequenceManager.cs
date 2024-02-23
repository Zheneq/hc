using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class SequenceManager : MonoBehaviour
{
	internal bool m_sequencesWaitForClientEnable;

	private int m_curId;

	private List<Sequence> m_sequences = new List<Sequence>(16);

	private static SequenceManager s_instance;

	private AbilityPriority m_lastHandledAbilityPriority;

	private bool m_quitting;

	public const bool c_clientOnlySequences = true;

	private static bool m_forceActorsAsInvisible;

	public static bool SequenceDebugTraceOn
	{
		get { return false; }
	}

	public static bool SequenceForceActorsAsInvisible
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	private int GetNewId()
	{
		m_curId++;
		return m_curId;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void OnApplicationQuit()
	{
		m_quitting = true;
	}

	internal static SequenceManager Get()
	{
		return s_instance;
	}

	internal void HandleOnGameStopped()
	{
		HashSet<GameObject> hashSet = new HashSet<GameObject>();
		using (List<Sequence>.Enumerator enumerator = m_sequences.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Sequence current = enumerator.Current;
				if (current != null)
				{
					hashSet.Add(current.gameObject);
				}
			}
		}
		m_sequences.Clear();
		foreach (GameObject item in hashSet)
		{
			Object.Destroy(item);
		}
	}

	internal void ClientOnTurnResolveEnd()
	{
		for (int num = m_sequences.Count - 1; num >= 0; num--)
		{
			if (m_sequences[num] == null)
			{
				m_sequences.RemoveAt(num);
			}
		}
		List<Sequence> sequences = m_sequences;
		
		List<Sequence> list = sequences.FindAll(((Sequence sequence) => sequence.MarkedForRemoval || sequence.RemoveAtTurnEnd));
		HashSet<GameObject> objectsToDestroy = new HashSet<GameObject>();
		using (List<Sequence>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Sequence current = enumerator.Current;
				objectsToDestroy.Add(current.gameObject);
			}
		}
		list = m_sequences.FindAll((Sequence sequence) => objectsToDestroy.Contains(sequence.gameObject));
		m_sequences.RemoveAll((Sequence sequence) => objectsToDestroy.Contains(sequence.gameObject));
		foreach (GameObject item in objectsToDestroy)
		{
			Object.Destroy(item);
		}
	}

	internal void MarkSequenceToEndBySourceId(int sequencePrefabLookupId, int seqSourceId, Vector3 targetPos)
	{
		for (int i = 0; i < m_sequences.Count; i++)
		{
			Sequence sequence = m_sequences[i];
			if (!(sequence != null))
			{
				continue;
			}
			if (sequence.MarkedForRemoval)
			{
				continue;
			}
			if (sequence.Source.RootID != (uint)seqSourceId)
			{
				continue;
			}
			if (sequence.PrefabLookupId != sequencePrefabLookupId)
			{
				continue;
			}
			if (!(sequence.TargetPos == targetPos))
			{
				continue;
			}
			while (true)
			{
				sequence.MarkForRemoval();
				return;
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	internal void OnDestroySequence(Sequence seq)
	{
	}

	internal void OnTurnStart(int currentTurn)
	{
		for (int i = 0; i < m_sequences.Count; i++)
		{
			Sequence sequence = m_sequences[i];
			if (sequence == null)
			{
				Log.Error("Null sequence in list, index {0}", i);
			}
			else
			{
				sequence.AgeInTurns++;
				sequence.OnTurnStart(currentTurn);
			}
		}
		while (true)
		{
			SendAbilityPhaseStart(AbilityPriority.INVALID);
			m_lastHandledAbilityPriority = AbilityPriority.INVALID;
			if (m_sequences.Count > 200)
			{
				while (true)
				{
					Debug.LogError(new StringBuilder().Append("More than ").Append(200).Append(" sequences tracked concurrently").ToString());
					_001D();
					return;
				}
			}
			return;
		}
	}

	public void ClearAllSequences()
	{
		using (List<Sequence>.Enumerator enumerator = m_sequences.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Sequence current = enumerator.Current;
				Object.Destroy(current.gameObject);
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					goto end_IL_000e;
				}
			}
			end_IL_000e:;
		}
		m_sequences.Clear();
	}

	private void _001D()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int i = 0; i < m_sequences.Count; i++)
		{
			string empty = string.Empty;
			Sequence sequence = m_sequences[i];
			if (sequence != null)
			{
				empty = sequence.name;
			}
			else
			{
				empty = "NULL";
			}
			if (dictionary.ContainsKey(empty))
			{
				dictionary[empty]++;
			}
			else
			{
				dictionary[empty] = 1;
			}
		}
		while (true)
		{
			string text = string.Empty;
			using (Dictionary<string, int>.Enumerator enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, int> current = enumerator.Current;
					string text2 = text;
					text = new StringBuilder().Append(text2).Append("[ ").Append(current.Key).Append(" ] count = ").Append(current.Value).Append("\n").ToString();
				}
			}
			Log.Error(text);
			return;
		}
	}

	internal void SendAbilityPhaseStart(AbilityPriority abilityPhase)
	{
		for (int i = 0; i < m_sequences.Count; i++)
		{
			Sequence sequence = m_sequences[i];
			if (sequence == null)
			{
				Log.Error("Null sequence in list, index {0}", i);
			}
			else
			{
				sequence.OnAbilityPhaseStart(abilityPhase);
			}
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	internal void OnAbilityPhaseStart(AbilityPriority abilityPhase)
	{
		for (int i = (int)(m_lastHandledAbilityPriority + 1); i <= (int)abilityPhase; i++)
		{
			SendAbilityPhaseStart((AbilityPriority)i);
		}
		while (true)
		{
			m_lastHandledAbilityPriority = abilityPhase;
			return;
		}
	}

	internal Sequence[] CreateClientSequences(GameObject prefab, BoardSquare targetSquare, Vector3 targetPos, Quaternion targetRotation, ActorData[] targets, ActorData caster, SequenceSource source, Sequence.IExtraSequenceParams[] extraParams)
	{
		short baseSequenceLookupId = (short)((!(SequenceLookup.Get() != null)) ? (-1) : SequenceLookup.Get().GetSequenceIdOfPrefab(prefab));
		if (source == null)
		{
			Log.Error("Code error: sequences must always be created with a SequenceSource (typically, AbilityRunData.m_sequenceSource, Effect.SequenceSource, or PowerUp.SequenceSource.");
		}
		if (caster != null)
		{
			prefab = caster.ReplaceSequence(prefab);
		}
		if (prefab == null)
		{
			if (SequenceLookup.Get() != null)
			{
				prefab = SequenceLookup.Get().GetSimpleHitSequencePrefab();
			}
		}
		Sequence[] array = null;
		if (prefab != null)
		{
			GameObject gameObject = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
			if ((bool)gameObject)
			{
				array = gameObject.GetComponents<Sequence>();
				Sequence[] array2 = array;
				foreach (Sequence sequence in array2)
				{
					sequence.BaseInitialize_Client(targetSquare, targetPos, targetRotation, targets, caster, GetNewId(), prefab, baseSequenceLookupId, source, extraParams);
					m_sequences.Add(sequence);
				}
			}
		}
		else if (source != null)
		{
			if (Application.isEditor)
			{
				Log.Warning("Creating sequences for null prefab.  Hitting immediately (without informing theatrics)...");
			}
			Sequence seq = null;
			if (targets != null)
			{
				foreach (ActorData target in targets)
				{
					source.OnSequenceHit(seq, target, null);
				}
			}
			source.OnSequenceHit(seq, targetPos);
		}
		return array;
	}

	internal Sequence[] CreateClientSequences(GameObject prefab, Vector3 targetPos, ActorData[] targets, ActorData caster, SequenceSource source, Sequence.IExtraSequenceParams[] extraParams)
	{
		return CreateClientSequences(prefab, null, targetPos, Quaternion.identity, targets, caster, source, extraParams);
	}

	internal Sequence[] CreateClientSequences(GameObject prefab, Vector3 targetPos, Quaternion targetRotation, ActorData[] targets, ActorData caster, SequenceSource source, Sequence.IExtraSequenceParams[] extraParams)
	{
		return CreateClientSequences(prefab, null, targetPos, targetRotation, targets, caster, source, extraParams);
	}

	internal Sequence[] CreateClientSequences(GameObject prefab, BoardSquare targetSquare, ActorData[] targets, ActorData caster, SequenceSource source, Sequence.IExtraSequenceParams[] extraParams)
	{
		Vector3 targetPos = (!(targetSquare != null)) ? Vector3.zero : targetSquare.ToVector3();
		return CreateClientSequences(prefab, targetSquare, targetPos, Quaternion.identity, targets, caster, source, extraParams);
	}

	public static bool UsingClientOnlySequences()
	{
		return true;
	}

	public GameObject FindTempSatellite(SequenceSource seqSource)
	{
		GameObject result = null;
		for (int i = 0; i < m_sequences.Count; i++)
		{
			if (m_sequences[i] == null)
			{
				Log.Error("Null sequence in list, index {0}", i);
			}
			else
			{
				if (!(m_sequences[i].Source == seqSource))
				{
					continue;
				}
				TempSatelliteSequence tempSatelliteSequence = m_sequences[i] as TempSatelliteSequence;
				if (tempSatelliteSequence != null)
				{
					result = tempSatelliteSequence.GetTempSatellite();
					break;
				}
			}
		}
		return result;
	}

	internal Sequence FindSequence(int sequenceId)
	{
		return m_sequences.Find((Sequence entry) => entry.Id == sequenceId);
	}

	public void OnAnimationEvent(ActorData animatedActor, Object eventObject, GameObject sourceObject, SequenceSource source)
	{
		for (int i = 0; i < m_sequences.Count; i++)
		{
			Sequence sequence = m_sequences[i];
			if (sequence == null)
			{
				Log.Error("Null sequence in list, index {0}", i);
				continue;
			}
			SequenceSource source2 = sequence.Source;
			if (source2 == source)
			{
				sequence.AnimationEvent(eventObject, sourceObject);
			}
		}
		while (true)
		{
			switch (3)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public List<ActorData> FindSequenceTargets(ActorData caster)
	{
		List<ActorData> list = new List<ActorData>();
		using (List<Sequence>.Enumerator enumerator = m_sequences.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Sequence current = enumerator.Current;
				if (current.Caster == caster && current.Targets != null)
				{
					list.AddRange(current.Targets);
				}
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return list;
				}
			}
		}
	}

	internal void DoClientEnable(SequenceSource source)
	{
		if (!NetworkClient.active)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					Log.Error("Attempted to call client only method without client.");
					return;
				}
			}
		}
		for (int i = 0; i < m_sequences.Count; i++)
		{
			Sequence sequence = m_sequences[i];
			if (!(sequence != null))
			{
				continue;
			}
			if (sequence.Source == source)
			{
				sequence.OnDoClientEnable();
			}
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public string GetSequenceHitsSeenDebugString(SequenceSource source, bool justFirstSequence = true)
	{
		string text = string.Empty;
		if (source != null)
		{
			for (int i = 0; i < m_sequences.Count; i++)
			{
				Sequence sequence = m_sequences[i];
				if (!(sequence != null))
				{
					continue;
				}
				if (sequence.Source == source)
				{
					string text2 = text;
					text = new StringBuilder().Append(text2).Append("* Sequence hits seen on sequence <").Append(sequence.name).Append(">, MarkedForRemoval = ").Append(sequence.MarkedForRemoval).Append(", active = ").Append(sequence.enabled).Append(", SourceRootID = ").Append(sequence.Source.RootID).Append(":\n\t").Append(sequence.Source.GetHitActorsString()).Append("\n").Append(sequence.Source.GetHitPositionsString()).Append("* Sequence Target IDs: ").Append(sequence.GetTargetsString()).Append("\n").ToString();
					text = new StringBuilder().Append(text).Append("* Has Received Anim Event before initialized: ").Append(sequence.HasReceivedAnimEventBeforeReady).Append("\n").ToString();
					if (justFirstSequence)
					{
						break;
					}
				}
			}
		}
		return text;
	}
}
