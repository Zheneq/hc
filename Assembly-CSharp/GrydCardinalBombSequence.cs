using System.Collections.Generic;
using UnityEngine;

public class GrydCardinalBombSequence : Sequence
{
    public class SegmentExtraParams : IExtraSequenceParams
    {
        public List<SegmentDataEntry> m_segmentData;
        public List<HitActorEntry> m_hitActors;

        public override void XSP_SerializeToStream(IBitStream stream)
        {
            sbyte segmentNum = (sbyte)(m_segmentData != null ? m_segmentData.Count : 0);
            stream.Serialize(ref segmentNum);
            for (int i = 0; i < segmentNum; i++)
            {
                SegmentDataEntry.Serialize(stream, m_segmentData[i]);
            }

            sbyte hitActorNum = (sbyte)(m_hitActors != null ? m_hitActors.Count : 0);
            stream.Serialize(ref hitActorNum);
            for (int i = 0; i < hitActorNum; i++)
            {
                HitActorEntry.Serialize(stream, m_hitActors[i]);
            }
        }

        public override void XSP_DeserializeFromStream(IBitStream stream)
        {
            sbyte segmentNum = 0;
            stream.Serialize(ref segmentNum);
            m_segmentData = new List<SegmentDataEntry>();
            for (int i = 0; i < segmentNum; i++)
            {
                SegmentDataEntry segmentDataEntry = new SegmentDataEntry();
                SegmentDataEntry.Deserialize(stream, segmentDataEntry);
                m_segmentData.Add(segmentDataEntry);
            }

            sbyte hitActorNum = 0;
            stream.Serialize(ref hitActorNum);
            m_hitActors = new List<HitActorEntry>();
            for (int i = 0; i < hitActorNum; i++)
            {
                HitActorEntry hitActorEntry = new HitActorEntry();
                HitActorEntry.Deserialize(stream, hitActorEntry);
                m_hitActors.Add(hitActorEntry);
            }
        }
    }

    public class SegmentDataEntry
    {
        public sbyte m_segmentIndex;
        public sbyte m_prevSegmentIndex;
        public BoardSquare m_startSquare;
        public BoardSquare m_endSquare;
        public bool m_markedChildrenToStart;
        public GenericSequenceProjectileInfo m_projectileContainer;

        public static void Serialize(IBitStream stream, SegmentDataEntry entry)
        {
            stream.Serialize(ref entry.m_segmentIndex);
            stream.Serialize(ref entry.m_prevSegmentIndex);
            sbyte startX = (sbyte)(entry.m_startSquare != null ? entry.m_startSquare.x : 0);
            sbyte startY = (sbyte)(entry.m_startSquare != null ? entry.m_startSquare.y : 0);
            sbyte endX = (sbyte)(entry.m_endSquare != null ? entry.m_endSquare.x : 0);
            sbyte endY = (sbyte)(entry.m_endSquare != null ? entry.m_endSquare.y : 0);
            stream.Serialize(ref startX);
            stream.Serialize(ref startY);
            stream.Serialize(ref endX);
            stream.Serialize(ref endY);
        }

        public static void Deserialize(IBitStream stream, SegmentDataEntry entry)
        {
            stream.Serialize(ref entry.m_segmentIndex);
            stream.Serialize(ref entry.m_prevSegmentIndex);
            sbyte startX = 0;
            sbyte startY = 0;
            sbyte endX = 0;
            sbyte endY = 0;
            stream.Serialize(ref startX);
            stream.Serialize(ref startY);
            stream.Serialize(ref endX);
            stream.Serialize(ref endY);
            entry.m_startSquare = Board.Get().GetSquareFromIndex(startX, startY);
            entry.m_endSquare = Board.Get().GetSquareFromIndex(endX, endY);
        }
    }

    public class HitActorEntry
    {
        public sbyte m_segmentIndex;
        public sbyte m_actorIndex;

        public static void Serialize(IBitStream stream, HitActorEntry entry)
        {
            stream.Serialize(ref entry.m_segmentIndex);
            stream.Serialize(ref entry.m_actorIndex);
        }

        public static void Deserialize(IBitStream stream, HitActorEntry entry)
        {
            stream.Serialize(ref entry.m_segmentIndex);
            stream.Serialize(ref entry.m_actorIndex);
        }
    }

    [Separator("Initial Projectile for spawn, ground portion starts when it lands")]
    public GenericSequenceProjectileAuthoredInfo m_initiatingProjectileInfo;
    [Separator("Projectile for hit area (joints not used)")]
    public GenericSequenceProjectileAuthoredInfo m_groundProjectileInfo;
    [Tooltip("Animation event (if any) to wait for before starting the sequence. Search project for EventObjects.")]
    [AnimEventPicker]
    public Object m_startEvent;

    private bool m_startedInitialProjectile;
    private bool m_startedGroundProjectile;
    private bool m_didFinalPosHit;

    private List<SegmentDataEntry> m_segmentData = new List<SegmentDataEntry>();
    private Dictionary<int, List<ActorData>> m_indexToHitActors = new Dictionary<int, List<ActorData>>();
    private GenericSequenceProjectileInfo m_initialProjectileContainer;
    private List<GenericSequenceProjectileInfo> m_projectContainers = new List<GenericSequenceProjectileInfo>();

    internal override void Initialize(IExtraSequenceParams[] extraParams)
    {
        base.Initialize(extraParams);
        foreach (IExtraSequenceParams extraSequenceParams in extraParams)
        {
            if (!(extraSequenceParams is SegmentExtraParams segmentExtraParams))
            {
                continue;
            }

            if (segmentExtraParams.m_segmentData != null)
            {
                foreach (SegmentDataEntry segment in segmentExtraParams.m_segmentData)
                {
                    if (segment.m_startSquare != null
                        && segment.m_endSquare != null
                        && segment.m_startSquare != segment.m_endSquare)
                    {
                        m_segmentData.Add(segment);
                    }
                    else if (Application.isEditor)
                    {
                        Debug.LogError(name + " has bad segment data for projectile");
                    }
                }
            }

            if (segmentExtraParams.m_hitActors != null)
            {
                foreach (HitActorEntry hitActor in segmentExtraParams.m_hitActors)
                {
                    int segmentIndex = hitActor.m_segmentIndex;
                    int actorIndex = hitActor.m_actorIndex;
                    ActorData actor = GameFlowData.Get().FindActorByActorIndex(actorIndex);
                    if (!m_indexToHitActors.ContainsKey(segmentIndex))
                    {
                        m_indexToHitActors[segmentIndex] = new List<ActorData>();
                    }

                    m_indexToHitActors[segmentIndex].Add(actor);
                }
            }

            return;
        }
    }

    private void OnDisable()
    {
        foreach (GenericSequenceProjectileInfo projectileContainer in m_projectContainers)
        {
            if (projectileContainer != null)
            {
                projectileContainer.OnSequenceDisable();
            }
        }
    }

    public override void FinishSetup()
    {
        base.FinishSetup();
        if (m_startEvent != null)
        {
            return;
        }

        m_startedInitialProjectile = true;
        if (m_initiatingProjectileInfo.m_fxPrefab != null)
        {
            StartInitialProjectile();
        }
        else
        {
            StartGroundProjectileChain();
        }
    }

    protected override void OnAnimationEvent(Object parameter, GameObject sourceObject)
    {
        if (m_startEvent != parameter)
        {
            return;
        }

        m_startedInitialProjectile = true;
        if (m_initiatingProjectileInfo.m_fxPrefab != null)
        {
            StartInitialProjectile();
        }
        else
        {
            StartGroundProjectileChain();
        }
    }

    private void Update()
    {
        ProcessSequenceVisibility();
        if (!m_startedInitialProjectile || m_didFinalPosHit)
        {
            return;
        }

        if (m_initialProjectileContainer != null)
        {
            m_initialProjectileContainer.OnUpdate();
        }

        if (!m_startedGroundProjectile
            && (m_initialProjectileContainer == null || m_initialProjectileContainer.m_finished))
        {
            StartGroundProjectileChain();
        }

        if (!m_startedGroundProjectile)
        {
            return;
        }

        bool isWaitingForHit = false;
        foreach (SegmentDataEntry segment in m_segmentData)
        {
            if (!segment.m_markedChildrenToStart
                && segment.m_projectileContainer != null
                && segment.m_projectileContainer.m_finished)
            {
                StartChildrenProjectile(segment);
            }

            if (segment.m_projectileContainer == null
                || !segment.m_projectileContainer.m_finished)
            {
                isWaitingForHit = true;
            }
        }

        foreach (GenericSequenceProjectileInfo projectileContainer in m_projectContainers)
        {
            projectileContainer.OnUpdate();
        }

        if (!isWaitingForHit)
        {
            Source.OnSequenceHit(this, TargetPos);
            m_didFinalPosHit = true;
        }
    }

    private void StartInitialProjectile()
    {
        if (m_initialProjectileContainer != null)
        {
            return;
        }

        GameObject referenceModel = GetReferenceModel(Caster, m_initiatingProjectileInfo.m_jointReferenceType);
        Vector3 startPos = Caster.GetFreePos();
        Vector3 targetPos = TargetPos;
        if (referenceModel != null)
        {
            m_initiatingProjectileInfo.m_fxJoint.Initialize(referenceModel);
            startPos = m_initiatingProjectileInfo.m_fxJoint.m_jointObject.transform.position;
        }

        m_initialProjectileContainer = new GenericSequenceProjectileInfo(
            this,
            m_initiatingProjectileInfo,
            startPos,
            targetPos,
            null)
        {
            m_positionForSequenceHit = targetPos + Vector3.up
        };
    }

    private void StartGroundProjectileChain()
    {
        foreach (SegmentDataEntry segment in m_segmentData)
        {
            if (segment.m_projectileContainer == null
                && segment.m_prevSegmentIndex < 0)
            {
                SpawnProjectileForSegment(segment);
            }
        }

        m_startedGroundProjectile = true;
    }

    private void StartChildrenProjectile(SegmentDataEntry parentSegment)
    {
        foreach (SegmentDataEntry segment in m_segmentData)
        {
            if (segment.m_projectileContainer == null
                && segment.m_prevSegmentIndex == parentSegment.m_segmentIndex)
            {
                SpawnProjectileForSegment(segment);
            }
        }

        parentSegment.m_markedChildrenToStart = true;
    }

    private void SpawnProjectileForSegment(SegmentDataEntry segEntry)
    {
        Vector3 startPos = segEntry.m_startSquare.ToVector3();
        Vector3 endPos = segEntry.m_endSquare.ToVector3();
        ActorData[] targetActors = null;
        if (m_indexToHitActors.ContainsKey(segEntry.m_segmentIndex))
        {
            targetActors = m_indexToHitActors[segEntry.m_segmentIndex].ToArray();
        }

        segEntry.m_projectileContainer = new GenericSequenceProjectileInfo(
            this,
            m_groundProjectileInfo,
            startPos,
            endPos,
            targetActors);
        m_projectContainers.Add(segEntry.m_projectileContainer);
    }
}