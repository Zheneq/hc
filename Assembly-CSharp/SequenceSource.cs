using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class SequenceSource
{
    internal delegate void ActorDelegate(ActorData target);

    internal delegate void Vector3Delegate(Vector3 position);

    private static uint s_nextID = 1u;
    private static Dictionary<uint, List<SequenceSource>> s_idsToSrcs = new Dictionary<uint, List<SequenceSource>>();

    private uint _rootID;
    private ActorDelegate m_onHitActor;
    private Vector3Delegate m_onHitPosition;
    private HashSet<Vector3> m_hitPositions = new HashSet<Vector3>();
    private HashSet<ActorData> m_hitActors = new HashSet<ActorData>();
    private int m_hitTurn = -1;
    private AbilityPriority m_hitPhase = AbilityPriority.INVALID;

    internal uint RootID
    {
        get => _rootID;
        private set
        {
            if (_rootID == 0 && value != 0)
            {
                List<SequenceSource> sequenceSources = s_idsToSrcs.TryGetValue(value, out var src)
                    ? src
                    : new List<SequenceSource>();
                sequenceSources.Add(this);
                s_idsToSrcs[value] = sequenceSources;
            }

            _rootID = value;
        }
    }

    internal bool RemoveAtEndOfTurn { get; set; }

    internal bool WaitForClientEnable { get; private set; }

    internal SequenceSource()
    {
    }

    internal SequenceSource(
        ActorDelegate onHitActor,
        Vector3Delegate onHitPosition,
        bool removeAtEndOfTurn = true,
        SequenceSource parentSource = null,
        IBitStream stream = null)
    {
        m_onHitActor = onHitActor;
        m_onHitPosition = onHitPosition;
        RemoveAtEndOfTurn = removeAtEndOfTurn;
        WaitForClientEnable = false;
        if (stream == null)
        {
            RootID = parentSource != null
                ? parentSource.RootID
                : AllocateID();
        }
        else
        {
            OnSerializeHelper(stream);
        }
    }

    internal SequenceSource(
        ActorDelegate onHitActor,
        Vector3Delegate onHitPosition,
        uint rootID,
        bool removeAtEndOfTurn)
    {
        m_onHitActor = onHitActor;
        m_onHitPosition = onHitPosition;
        RootID = rootID;
        RemoveAtEndOfTurn = removeAtEndOfTurn;
    }

    private static uint AllocateID()
    {
        if (!NetworkServer.active && NetworkClient.active)
        {
            Log.Error("Code Error: SequenceSource IDs should only be allocated on the server");
        }

        return s_nextID++;
    }

    internal void SetWaitForClientEnable(bool value)
    {
        WaitForClientEnable = value;
    }

    internal SequenceSource GetShallowCopy()
    {
        return (SequenceSource)MemberwiseClone();
    }

    ~SequenceSource()
    {
        if (s_idsToSrcs.TryGetValue(_rootID, out var list))
        {
            list.Remove(this);
        }
    }

    internal static void ClearStaticData()
    {
        s_idsToSrcs.Clear();
    }

    internal void OnSerializeHelper(NetworkWriter stream)
    {
        OnSerializeHelper(new NetworkWriterAdapter(stream));
    }

    internal void OnSerializeHelper(IBitStream stream)
    {
        uint rootID = RootID;
        bool removeAtEndOfTurn = RemoveAtEndOfTurn;
        bool waitForClientEnable = WaitForClientEnable;
        stream.Serialize(ref rootID);
        stream.Serialize(ref removeAtEndOfTurn);
        stream.Serialize(ref waitForClientEnable);
        if (RootID != rootID)
        {
            RootID = rootID;
        }

        if (RemoveAtEndOfTurn != removeAtEndOfTurn)
        {
            RemoveAtEndOfTurn = removeAtEndOfTurn;
        }

        if (WaitForClientEnable != waitForClientEnable)
        {
            WaitForClientEnable = waitForClientEnable;
        }
    }

    internal void OnSequenceHit(
        Sequence seq,
        ActorData target,
        ActorModelData.ImpulseInfo impulseInfo,
        ActorModelData.RagdollActivation ragdollActivation = ActorModelData.RagdollActivation.HealthBased,
        bool tryHitReactIfAlreadyHit = true)
    {
        AbilityPriority currentAbilityPhase = ServerClientUtils.GetCurrentAbilityPhase();
        if (m_hitTurn != GameFlowData.Get().CurrentTurn || m_hitPhase != currentAbilityPhase)
        {
            m_hitTurn = GameFlowData.Get().CurrentTurn;
            m_hitPhase = currentAbilityPhase;
            m_hitPositions.Clear();
            m_hitActors.Clear();
        }

        bool alreadyHit = false;
        if (!m_hitActors.Contains(target))
        {
            if (m_onHitActor != null)
            {
                m_onHitActor(target);
            }
        }
        else
        {
            alreadyHit = true;
        }

        m_hitActors.Add(target);
        if (seq != null && (tryHitReactIfAlreadyHit || !alreadyHit))
        {
            TheatricsManager.Get().OnSequenceHit(seq, target, impulseInfo, ragdollActivation);
        }

        if (SequenceManager.SequenceDebugTraceOn)
        {
            Debug.LogWarning(
                "<color=yellow>Sequence Actor Hit: </color>"
                + $"<<color=lightblue>{seq.gameObject.name} | {seq.GetType()}</color>> \n"
                + $"hit on: {target.DebugNameString("white")} @time= {Time.time}");
        }
    }

    internal void OnSequenceHit(Sequence seq, Vector3 position, ActorModelData.ImpulseInfo impulseInfo = null)
    {
        AbilityPriority currentAbilityPhase = ServerClientUtils.GetCurrentAbilityPhase();
        if (m_hitTurn != GameFlowData.Get().CurrentTurn || m_hitPhase != currentAbilityPhase)
        {
            m_hitTurn = GameFlowData.Get().CurrentTurn;
            m_hitPhase = currentAbilityPhase;
            m_hitPositions.Clear();
            m_hitActors.Clear();
        }

        if (!m_hitPositions.Contains(position))
        {
            m_hitPositions.Add(position);
            if (m_onHitPosition != null)
            {
                m_onHitPosition(position);
            }
        }

        if (SequenceManager.SequenceDebugTraceOn)
        {
            Debug.LogWarning(
                "<color=yellow>Sequence Position Hit: </color>"
                + $"<<color=lightblue>{seq.gameObject.name} | {seq.GetType()}</color>> \n"
                + $"hit at: {position} @time= {Time.time}");
        }
    }

    internal static bool DidSequenceHit(SequenceSource src, ActorData target)
    {
        if (s_idsToSrcs.TryGetValue(src.RootID, out var sources))
        {
            foreach (SequenceSource source in sources)
            {
                if (source.m_hitActors.Contains(target))
                {
                    return true;
                }
            }
        }

        return false;
    }

    internal static bool DidSequenceHit(SequenceSource src, Vector3 position)
    {
        if (s_idsToSrcs.TryGetValue(src.RootID, out var sources))
        {
            foreach (SequenceSource source in sources)
            {
                if (source.m_hitPositions.Contains(position))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public string GetHitActorsString()
    {
        string text = string.Empty;
        foreach (ActorData hitActor in m_hitActors)
        {
            if (hitActor != null)
            {
                if (text.Length > 0)
                {
                    text += " | ";
                }

                text += hitActor.ActorIndex;
            }
        }

        return "Did Hit Actor IDs: " + (text.Length > 0 ? text : "(none)");
    }

    public string GetHitPositionsString()
    {
        string text = string.Empty;
        foreach (Vector3 hitPos in m_hitPositions)
        {
            text = text + "\t" + hitPos + "\n";
        }

        return text;
    }

    public override bool Equals(object obj)
    {
        SequenceSource sequenceSource = obj as SequenceSource;
        return (object)sequenceSource != null && RootID == sequenceSource.RootID;
    }

    public bool Equals(SequenceSource p)
    {
        return RootID == p.RootID;
    }

    public static bool operator ==(SequenceSource a, SequenceSource b)
    {
        if (ReferenceEquals(a, b))
        {
            return true;
        }

        if ((object)a != null && (object)b != null)
        {
            return a.RootID == b.RootID;
        }

        return false;
    }

    public static bool operator !=(SequenceSource a, SequenceSource b)
    {
        return !(a == b);
    }

    public override int GetHashCode()
    {
        return (int)RootID;
    }
}