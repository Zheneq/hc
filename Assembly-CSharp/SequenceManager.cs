using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SequenceManager : MonoBehaviour
{
	internal bool m_sequencesWaitForClientEnable;

	private int m_curId;

	private List<Sequence> m_sequences = new List<Sequence>(0x10);

	private static SequenceManager s_instance;

	private AbilityPriority m_lastHandledAbilityPriority;

	private bool m_quitting;

	public const bool c_clientOnlySequences = true;

	private static bool m_forceActorsAsInvisible;

	private int GetNewId()
	{
		this.m_curId++;
		return this.m_curId;
	}

	private void Awake()
	{
		SequenceManager.s_instance = this;
	}

	private void OnDestroy()
	{
		SequenceManager.s_instance = null;
	}

	private void OnApplicationQuit()
	{
		this.m_quitting = true;
	}

	internal static SequenceManager Get()
	{
		return SequenceManager.s_instance;
	}

	internal void HandleOnGameStopped()
	{
		HashSet<GameObject> hashSet = new HashSet<GameObject>();
		using (List<Sequence>.Enumerator enumerator = this.m_sequences.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Sequence sequence = enumerator.Current;
				if (sequence != null)
				{
					hashSet.Add(sequence.gameObject);
				}
			}
		}
		this.m_sequences.Clear();
		foreach (GameObject obj in hashSet)
		{
			UnityEngine.Object.Destroy(obj);
		}
	}

	internal void ClientOnTurnResolveEnd()
	{
		for (int i = this.m_sequences.Count - 1; i >= 0; i--)
		{
			if (this.m_sequences[i] == null)
			{
				this.m_sequences.RemoveAt(i);
			}
		}
		List<Sequence> sequences = this.m_sequences;
		
		List<Sequence> list = sequences.FindAll(((Sequence sequence) => sequence.MarkedForRemoval || sequence.RemoveAtTurnEnd));
		HashSet<GameObject> objectsToDestroy = new HashSet<GameObject>();
		using (List<Sequence>.Enumerator enumerator = list.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Sequence sequence2 = enumerator.Current;
				objectsToDestroy.Add(sequence2.gameObject);
			}
		}
		list = this.m_sequences.FindAll((Sequence sequence) => objectsToDestroy.Contains(sequence.gameObject));
		this.m_sequences.RemoveAll((Sequence sequence) => objectsToDestroy.Contains(sequence.gameObject));
		foreach (GameObject obj in objectsToDestroy)
		{
			UnityEngine.Object.Destroy(obj);
		}
	}

	internal void MarkSequenceToEndBySourceId(int sequencePrefabLookupId, int seqSourceId, Vector3 targetPos)
	{
		for (int i = 0; i < this.m_sequences.Count; i++)
		{
			Sequence sequence = this.m_sequences[i];
			if (sequence != null)
			{
				if (!sequence.MarkedForRemoval)
				{
					if (sequence.Source.RootID == (uint)seqSourceId)
					{
						if ((int)sequence.PrefabLookupId == sequencePrefabLookupId)
						{
							if (sequence.TargetPos == targetPos)
							{
								sequence.MarkForRemoval();
								return;
							}
						}
					}
				}
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	internal void OnDestroySequence(Sequence seq)
	{
	}

	internal void OnTurnStart(int currentTurn)
	{
		for (int i = 0; i < this.m_sequences.Count; i++)
		{
			Sequence sequence = this.m_sequences[i];
			if (sequence == null)
			{
				Log.Error("Null sequence in list, index {0}", new object[]
				{
					i
				});
			}
			else
			{
				sequence.AgeInTurns++;
				sequence.OnTurnStart(currentTurn);
			}
		}
		this.SendAbilityPhaseStart(AbilityPriority.INVALID);
		this.m_lastHandledAbilityPriority = AbilityPriority.INVALID;
		if (this.m_sequences.Count > 0xC8)
		{
			Debug.LogError("More than " + 0xC8 + " sequences tracked concurrently");
			this.symbol_001D();
		}
	}

	public void ClearAllSequences()
	{
		using (List<Sequence>.Enumerator enumerator = this.m_sequences.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Sequence sequence = enumerator.Current;
				UnityEngine.Object.Destroy(sequence.gameObject);
			}
		}
		this.m_sequences.Clear();
	}

	private void symbol_001D()
	{
		Dictionary<string, int> dictionary = new Dictionary<string, int>();
		for (int i = 0; i < this.m_sequences.Count; i++)
		{
			string text = string.Empty;
			Sequence sequence = this.m_sequences[i];
			if (sequence != null)
			{
				text = sequence.name;
			}
			else
			{
				text = "NULL";
			}
			if (dictionary.ContainsKey(text))
			{
				Dictionary<string, int> dictionary2;
				string key;
				(dictionary2 = dictionary)[key = text] = dictionary2[key] + 1;
			}
			else
			{
				dictionary[text] = 1;
			}
		}
		string text2 = string.Empty;
		using (Dictionary<string, int>.Enumerator enumerator = dictionary.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<string, int> keyValuePair = enumerator.Current;
				string text3 = text2;
				text2 = string.Concat(new object[]
				{
					text3,
					"[ ",
					keyValuePair.Key,
					" ] count = ",
					keyValuePair.Value,
					"\n"
				});
			}
		}
		Log.Error(text2, new object[0]);
	}

	internal void SendAbilityPhaseStart(AbilityPriority abilityPhase)
	{
		for (int i = 0; i < this.m_sequences.Count; i++)
		{
			Sequence sequence = this.m_sequences[i];
			if (sequence == null)
			{
				Log.Error("Null sequence in list, index {0}", new object[]
				{
					i
				});
			}
			else
			{
				sequence.OnAbilityPhaseStart(abilityPhase);
			}
		}
	}

	internal void OnAbilityPhaseStart(AbilityPriority abilityPhase)
	{
		for (int i = (int)(this.m_lastHandledAbilityPriority + 1); i <= (int)abilityPhase; i++)
		{
			this.SendAbilityPhaseStart((AbilityPriority)i);
		}
		this.m_lastHandledAbilityPriority = abilityPhase;
	}

	internal Sequence[] CreateClientSequences(GameObject prefab, BoardSquare targetSquare, Vector3 targetPos, Quaternion targetRotation, ActorData[] targets, ActorData caster, SequenceSource source, Sequence.IExtraSequenceParams[] extraParams)
	{
		short baseSequenceLookupId = (!(SequenceLookup.Get() != null)) ? -1 : SequenceLookup.Get().GetSequenceIdOfPrefab(prefab);
		if (source == null)
		{
			Log.Error("Code error: sequences must always be created with a SequenceSource (typically, AbilityRunData.m_sequenceSource, Effect.SequenceSource, or PowerUp.SequenceSource.", new object[0]);
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
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab, Vector3.zero, Quaternion.identity);
			if (gameObject)
			{
				array = gameObject.GetComponents<Sequence>();
				foreach (Sequence sequence in array)
				{
					sequence.BaseInitialize_Client(targetSquare, targetPos, targetRotation, targets, caster, this.GetNewId(), prefab, baseSequenceLookupId, source, extraParams);
					this.m_sequences.Add(sequence);
				}
			}
		}
		else if (source != null)
		{
			if (Application.isEditor)
			{
				Log.Warning("Creating sequences for null prefab.  Hitting immediately (without informing theatrics)...", new object[0]);
			}
			Sequence seq = null;
			if (targets != null)
			{
				foreach (ActorData target in targets)
				{
					source.OnSequenceHit(seq, target, null, ActorModelData.RagdollActivation.HealthBased, true);
				}
			}
			source.OnSequenceHit(seq, targetPos, null);
		}
		return array;
	}

	internal Sequence[] CreateClientSequences(GameObject prefab, Vector3 targetPos, ActorData[] targets, ActorData caster, SequenceSource source, Sequence.IExtraSequenceParams[] extraParams)
	{
		return this.CreateClientSequences(prefab, null, targetPos, Quaternion.identity, targets, caster, source, extraParams);
	}

	internal Sequence[] CreateClientSequences(GameObject prefab, Vector3 targetPos, Quaternion targetRotation, ActorData[] targets, ActorData caster, SequenceSource source, Sequence.IExtraSequenceParams[] extraParams)
	{
		return this.CreateClientSequences(prefab, null, targetPos, targetRotation, targets, caster, source, extraParams);
	}

	internal Sequence[] CreateClientSequences(GameObject prefab, BoardSquare targetSquare, ActorData[] targets, ActorData caster, SequenceSource source, Sequence.IExtraSequenceParams[] extraParams)
	{
		Vector3 targetPos = (!(targetSquare != null)) ? Vector3.zero : targetSquare.ToVector3();
		return this.CreateClientSequences(prefab, targetSquare, targetPos, Quaternion.identity, targets, caster, source, extraParams);
	}

	public static bool UsingClientOnlySequences()
	{
		return true;
	}

	public GameObject FindTempSatellite(SequenceSource seqSource)
	{
		GameObject result = null;
		for (int i = 0; i < this.m_sequences.Count; i++)
		{
			if (this.m_sequences[i] == null)
			{
				Log.Error("Null sequence in list, index {0}", new object[]
				{
					i
				});
			}
			else if (this.m_sequences[i].Source == seqSource)
			{
				TempSatelliteSequence tempSatelliteSequence = this.m_sequences[i] as TempSatelliteSequence;
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
		return this.m_sequences.Find((Sequence entry) => entry.Id == sequenceId);
	}

	public void OnAnimationEvent(ActorData animatedActor, UnityEngine.Object eventObject, GameObject sourceObject, SequenceSource source)
	{
		for (int i = 0; i < this.m_sequences.Count; i++)
		{
			Sequence sequence = this.m_sequences[i];
			if (sequence == null)
			{
				Log.Error("Null sequence in list, index {0}", new object[]
				{
					i
				});
			}
			else
			{
				SequenceSource source2 = sequence.Source;
				bool flag = source2 == source;
				if (flag)
				{
					sequence.AnimationEvent(eventObject, sourceObject);
				}
			}
		}
	}

	public List<ActorData> FindSequenceTargets(ActorData caster)
	{
		List<ActorData> list = new List<ActorData>();
		using (List<Sequence>.Enumerator enumerator = this.m_sequences.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Sequence sequence = enumerator.Current;
				if (sequence.Caster == caster && sequence.Targets != null)
				{
					list.AddRange(sequence.Targets);
				}
			}
		}
		return list;
	}

	internal void DoClientEnable(SequenceSource source)
	{
		if (!NetworkClient.active)
		{
			Log.Error("Attempted to call client only method without client.", new object[0]);
			return;
		}
		for (int i = 0; i < this.m_sequences.Count; i++)
		{
			Sequence sequence = this.m_sequences[i];
			if (sequence != null)
			{
				if (sequence.Source == source)
				{
					sequence.OnDoClientEnable();
				}
			}
		}
	}

	public string GetSequenceHitsSeenDebugString(SequenceSource source, bool justFirstSequence = true)
	{
		string text = string.Empty;
		if (source != null)
		{
			for (int i = 0; i < this.m_sequences.Count; i++)
			{
				Sequence sequence = this.m_sequences[i];
				if (sequence != null)
				{
					if (sequence.Source == source)
					{
						string text2 = text;
						text = string.Concat(new object[]
						{
							text2,
							"* Sequence hits seen on sequence <",
							sequence.name,
							">, MarkedForRemoval = ",
							sequence.MarkedForRemoval,
							", active = ",
							sequence.enabled,
							", SourceRootID = ",
							sequence.Source.RootID,
							":\n\t",
							sequence.Source.GetHitActorsString(),
							"\n",
							sequence.Source.GetHitPositionsString(),
							"* Sequence Target IDs: ",
							sequence.GetTargetsString(),
							"\n"
						});
						text = text + "* Has Received Anim Event before initialized: " + sequence.HasReceivedAnimEventBeforeReady.ToString() + "\n";
						if (justFirstSequence)
						{
							break;
						}
					}
				}
			}
		}
		return text;
	}

	public static bool SequenceDebugTraceOn
	{
		get
		{
			return false;
		}
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
}
