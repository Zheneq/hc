using System.Collections.Generic;
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

	public static bool SequenceDebugTraceOn => false;

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
					while (true)
					{
						switch (4)
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
					hashSet.Add(current.gameObject);
				}
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
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
				m_sequences.RemoveAt(num);
			}
		}
		List<Sequence> sequences = m_sequences;
		if (_003C_003Ef__am_0024cache0 == null)
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
			_003C_003Ef__am_0024cache0 = ((Sequence sequence) => sequence.MarkedForRemoval || sequence.RemoveAtTurnEnd);
		}
		List<Sequence> list = sequences.FindAll(_003C_003Ef__am_0024cache0);
		HashSet<GameObject> objectsToDestroy = new HashSet<GameObject>();
		using (List<Sequence>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Sequence current = enumerator.Current;
				objectsToDestroy.Add(current.gameObject);
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
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
			while (true)
			{
				switch (3)
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
			if (sequence.MarkedForRemoval)
			{
				continue;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (sequence.Source.RootID != (uint)seqSourceId)
			{
				continue;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (sequence.PrefabLookupId != sequencePrefabLookupId)
			{
				continue;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!(sequence.TargetPos == targetPos))
			{
				continue;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
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
				while (true)
				{
					switch (3)
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
			switch (6)
			{
			case 0:
				continue;
			}
			SendAbilityPhaseStart(AbilityPriority.INVALID);
			m_lastHandledAbilityPriority = AbilityPriority.INVALID;
			if (m_sequences.Count > 200)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					Debug.LogError("More than " + 200 + " sequences tracked concurrently");
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
				while (true)
				{
					switch (1)
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
				empty = sequence.name;
			}
			else
			{
				empty = "NULL";
			}
			if (dictionary.ContainsKey(empty))
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
				dictionary[empty]++;
			}
			else
			{
				dictionary[empty] = 1;
			}
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			string text = string.Empty;
			using (Dictionary<string, int>.Enumerator enumerator = dictionary.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<string, int> current = enumerator.Current;
					string text2 = text;
					text = text2 + "[ " + current.Key + " ] count = " + current.Value + "\n";
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
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
				while (true)
				{
					switch (4)
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
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			while (true)
			{
				switch (1)
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
			prefab = caster.ReplaceSequence(prefab);
		}
		if (prefab == null)
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
			if (SequenceLookup.Get() != null)
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
				prefab = SequenceLookup.Get().GetSimpleHitSequencePrefab();
			}
		}
		Sequence[] array = null;
		if (prefab != null)
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
			GameObject gameObject = Object.Instantiate(prefab, Vector3.zero, Quaternion.identity);
			if ((bool)gameObject)
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
				array = gameObject.GetComponents<Sequence>();
				Sequence[] array2 = array;
				foreach (Sequence sequence in array2)
				{
					sequence.BaseInitialize_Client(targetSquare, targetPos, targetRotation, targets, caster, GetNewId(), prefab, baseSequenceLookupId, source, extraParams);
					m_sequences.Add(sequence);
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		else if (source != null)
		{
			if (Application.isEditor)
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
				Log.Warning("Creating sequences for null prefab.  Hitting immediately (without informing theatrics)...");
			}
			Sequence seq = null;
			if (targets != null)
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
				foreach (ActorData target in targets)
				{
					source.OnSequenceHit(seq, target, null);
				}
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
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
				while (true)
				{
					switch (4)
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
				Log.Error("Null sequence in list, index {0}", i);
			}
			else
			{
				if (!(m_sequences[i].Source == seqSource))
				{
					continue;
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
				TempSatelliteSequence tempSatelliteSequence = m_sequences[i] as TempSatelliteSequence;
				if (tempSatelliteSequence != null)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
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
				while (true)
				{
					switch (3)
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
				Log.Error("Null sequence in list, index {0}", i);
				continue;
			}
			SequenceSource source2 = sequence.Source;
			if (source2 == source)
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
					while (true)
					{
						switch (2)
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (sequence.Source == source)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
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
			while (true)
			{
				switch (1)
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
			for (int i = 0; i < m_sequences.Count; i++)
			{
				Sequence sequence = m_sequences[i];
				if (!(sequence != null))
				{
					continue;
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
				if (sequence.Source == source)
				{
					string text2 = text;
					text = text2 + "* Sequence hits seen on sequence <" + sequence.name + ">, MarkedForRemoval = " + sequence.MarkedForRemoval + ", active = " + sequence.enabled + ", SourceRootID = " + sequence.Source.RootID + ":\n\t" + sequence.Source.GetHitActorsString() + "\n" + sequence.Source.GetHitPositionsString() + "* Sequence Target IDs: " + sequence.GetTargetsString() + "\n";
					text = text + "* Has Received Anim Event before initialized: " + sequence.HasReceivedAnimEventBeforeReady + "\n";
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
