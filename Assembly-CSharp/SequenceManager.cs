using System.Collections.Generic;
using System.Linq;
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

    public static bool SequenceForceActorsAsInvisible => false;

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
        HashSet<GameObject> sequenceObjects = new HashSet<GameObject>();
        foreach (Sequence sequence in m_sequences)
        {
            if (sequence != null)
            {
                sequenceObjects.Add(sequence.gameObject);
            }
        }

        m_sequences.Clear();
        foreach (GameObject sequenceObject in sequenceObjects)
        {
            Destroy(sequenceObject);
        }
    }

    internal void ClientOnTurnResolveEnd()
    {
        for (int i = m_sequences.Count - 1; i >= 0; i--)
        {
            if (m_sequences[i] == null)
            {
                m_sequences.RemoveAt(i);
            }
        }

        List<Sequence> sequences = m_sequences;

        List<Sequence> sequencesToRemove =
            sequences.FindAll(sequence => sequence.MarkedForRemoval || sequence.RemoveAtTurnEnd);
        HashSet<GameObject> objectsToDestroy = new HashSet<GameObject>();
        foreach (Sequence sequence in sequencesToRemove)
        {
            objectsToDestroy.Add(sequence.gameObject);
        }

        sequencesToRemove = m_sequences.FindAll(sequence => objectsToDestroy.Contains(sequence.gameObject));
        m_sequences.RemoveAll(sequence => objectsToDestroy.Contains(sequence.gameObject));
        foreach (GameObject sequenceObject in objectsToDestroy)
        {
            Destroy(sequenceObject);
        }
    }

    internal void MarkSequenceToEndBySourceId(int sequencePrefabLookupId, int seqSourceId, Vector3 targetPos)
    {
        foreach (Sequence sequence in m_sequences)
        {
            if (sequence != null
                && !sequence.MarkedForRemoval
                && sequence.Source.RootID == (uint)seqSourceId
                && sequence.PrefabLookupId == sequencePrefabLookupId
                && sequence.TargetPos == targetPos)
            {
                sequence.MarkForRemoval();
                return;
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

        SendAbilityPhaseStart(AbilityPriority.INVALID);
        m_lastHandledAbilityPriority = AbilityPriority.INVALID;
        if (m_sequences.Count > 200)
        {
            Debug.LogError("More than " + 200 + " sequences tracked concurrently");
            DebugLogExistingSequences();
        }
    }

    public void ClearAllSequences()
    {
        foreach (Sequence current in m_sequences)
        {
            Destroy(current.gameObject);
        }

        m_sequences.Clear();
    }

    private void DebugLogExistingSequences()
    {
        Dictionary<string, int> counts = new Dictionary<string, int>();
        foreach (Sequence sequence in m_sequences)
        {
            string sequenceName = sequence != null ? sequence.name : "NULL";
            if (counts.ContainsKey(sequenceName))
            {
                counts[sequenceName]++;
            }
            else
            {
                counts[sequenceName] = 1;
            }
        }

        string text = string.Empty;
        foreach (KeyValuePair<string, int> nameToCount in counts)
        {
            text += "[ " + nameToCount.Key + " ] count = " + nameToCount.Value + "\n";
        }

        Log.Error(text);
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
    }

    internal void OnAbilityPhaseStart(AbilityPriority abilityPhase)
    {
        for (int i = (int)(m_lastHandledAbilityPriority + 1); i <= (int)abilityPhase; i++)
        {
            SendAbilityPhaseStart((AbilityPriority)i);
        }

        m_lastHandledAbilityPriority = abilityPhase;
    }

    internal Sequence[] CreateClientSequences(
        GameObject prefab,
        BoardSquare targetSquare,
        Vector3 targetPos,
        Quaternion targetRotation,
        ActorData[] targets,
        ActorData caster,
        SequenceSource source,
        Sequence.IExtraSequenceParams[] extraParams)
    {
        short baseSequenceLookupId =
            (short)(SequenceLookup.Get() != null ? SequenceLookup.Get().GetSequenceIdOfPrefab(prefab) : -1);
        if (source == null)
        {
            Log.Error(
                "Code error: sequences must always be created with a SequenceSource (typically, AbilityRunData.m_sequenceSource, Effect.SequenceSource, or PowerUp.SequenceSource.");
        }

        if (caster != null)
        {
            prefab = caster.ReplaceSequence(prefab);
        }

        if (prefab == null && SequenceLookup.Get() != null)
        {
            prefab = SequenceLookup.Get().GetSimpleHitSequencePrefab();
        }

        Sequence[] sequences = null;
        if (prefab != null)
        {
            GameObject sequenceObject = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            if ((bool)sequenceObject)
            {
                sequences = sequenceObject.GetComponents<Sequence>();
                foreach (Sequence sequence in sequences)
                {
                    sequence.BaseInitialize_Client(
                        targetSquare,
                        targetPos,
                        targetRotation,
                        targets,
                        caster,
                        GetNewId(),
                        prefab,
                        baseSequenceLookupId,
                        source,
                        extraParams);
                    m_sequences.Add(sequence);
                }
            }
        }
        else if (source != null)
        {
            if (Application.isEditor)
            {
                Log.Warning(
                    "Creating sequences for null prefab.  Hitting immediately (without informing theatrics)...");
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

        return sequences;
    }

    internal Sequence[] CreateClientSequences(
        GameObject prefab,
        Vector3 targetPos,
        ActorData[] targets,
        ActorData caster,
        SequenceSource source,
        Sequence.IExtraSequenceParams[] extraParams)
    {
        return CreateClientSequences(
            prefab,
            null,
            targetPos,
            Quaternion.identity,
            targets,
            caster,
            source,
            extraParams);
    }

    internal Sequence[] CreateClientSequences(
        GameObject prefab,
        Vector3 targetPos,
        Quaternion targetRotation,
        ActorData[] targets,
        ActorData caster,
        SequenceSource source,
        Sequence.IExtraSequenceParams[] extraParams)
    {
        return CreateClientSequences(prefab, null, targetPos, targetRotation, targets, caster, source, extraParams);
    }

    internal Sequence[] CreateClientSequences(
        GameObject prefab,
        BoardSquare targetSquare,
        ActorData[] targets,
        ActorData caster,
        SequenceSource source,
        Sequence.IExtraSequenceParams[] extraParams)
    {
        Vector3 targetPos = !(targetSquare != null) ? Vector3.zero : targetSquare.ToVector3();
        return CreateClientSequences(
            prefab,
            targetSquare,
            targetPos,
            Quaternion.identity,
            targets,
            caster,
            source,
            extraParams);
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
                continue;
            }

            if (m_sequences[i].Source == seqSource)
            {
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
        return m_sequences.Find(entry => entry.Id == sequenceId);
    }

    public void OnAnimationEvent(
        ActorData animatedActor,
        Object eventObject,
        GameObject sourceObject,
        SequenceSource source)
    {
        for (int i = 0; i < m_sequences.Count; i++)
        {
            Sequence sequence = m_sequences[i];
            if (sequence == null)
            {
                Log.Error("Null sequence in list, index {0}", i);
                continue;
            }

            if (sequence.Source == source)
            {
                sequence.AnimationEvent(eventObject, sourceObject);
            }
        }
    }

    public List<ActorData> FindSequenceTargets(ActorData caster)
    {
        List<ActorData> targets = new List<ActorData>();
        foreach (Sequence sequence in m_sequences)
        {
            if (sequence.Caster == caster && sequence.Targets != null)
            {
                targets.AddRange(sequence.Targets);
            }
        }

        return targets;
    }

    internal void DoClientEnable(SequenceSource source)
    {
        if (!NetworkClient.active)
        {
            Log.Error("Attempted to call client only method without client.");
            return;
        }

        for (int i = 0; i < m_sequences.Count; i++) // we can add new sequences while iterating
        {
            Sequence sequence = m_sequences[i];
            if (sequence != null && sequence.Source == source)
            {
                sequence.OnDoClientEnable();
            }
        }
    }

    public string GetSequenceHitsSeenDebugString(SequenceSource source, bool justFirstSequence = true)
    {
        if (source == null)
        {
            return string.Empty;
        }

        string text = string.Empty;
        foreach (Sequence sequence in m_sequences)
        {
            if (sequence != null && sequence.Source == source)
            {
                text += "* Sequence hits seen on sequence <" + sequence.name + ">, MarkedForRemoval = "
                        + sequence.MarkedForRemoval + ", active = " + sequence.enabled + ", SourceRootID = "
                        + sequence.Source.RootID + ":\n\t" + sequence.Source.GetHitActorsString() + "\n"
                        + sequence.Source.GetHitPositionsString() + "* Sequence Target IDs: "
                        + sequence.GetTargetsString() + "\n";
                text += "* Has Received Anim Event before initialized: "
                        + sequence.HasReceivedAnimEventBeforeReady + "\n";
                if (justFirstSequence)
                {
                    break;
                }
            }
        }

        return text;
    }
    
#if EVOS
    public List<Sequence> GetSequencesForSource(SequenceSource source)
    {
        return m_sequences.Where(sequence => sequence != null && sequence.Source == source).ToList();
    }
#endif
}