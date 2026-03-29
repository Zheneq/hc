// SERVER
// ROGUES

using System.Collections.Generic;
using UnityEngine;

public class GrydCardinalBomb : Ability
{
    [Separator("Targeting")]
    public float m_maxTrunkDist = 8.5f;
    public float m_maxBranchDist = 5f;
    public bool m_splitOnWall = true;
    public bool m_splitOnActor;
    public bool m_trunkContinueAfterActorHit;
    public int m_maxNumSplits = 1;
    [Separator("On Hit")]
    public int m_baseDamage = 20;
    public int m_subseqHitDamage = 10;
    public StandardEffectInfo m_enemyHitEffect;
    [Separator("Sequences")]
    public GameObject m_projectileSequencePrefab;

    private void Start()
    {
        if (m_abilityName == "Base Ability")
        {
            m_abilityName = "GrydCardinalBomb";
        }

        Setup();
    }

    private void Setup()
    {
        Targeter = new AbilityUtil_Targeter_GrydCardinalBomb(
            this,
            m_maxTrunkDist,
            m_maxBranchDist,
            m_splitOnWall,
            m_splitOnActor,
            m_trunkContinueAfterActorHit,
            m_maxNumSplits);
    }

    protected override void AddSpecificTooltipTokens(List<TooltipTokenEntry> tokens, AbilityMod modAsBase)
    {
        AddTokenInt(tokens, "MaxNumSplits", string.Empty, m_maxNumSplits);
        AddTokenInt(tokens, "BaseDamage", string.Empty, m_baseDamage);
        AddTokenInt(tokens, "SubseqHitDamage", string.Empty, m_subseqHitDamage);
        AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffect, "EnemyHitEffect", m_enemyHitEffect);
    }

    protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
    {
        List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
        AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_baseDamage);
        return numbers;
    }

    public override bool GetCustomTargeterNumbers(
        ActorData targetActor,
        int currentTargeterIndex,
        TargetingNumberUpdateScratch results)
    {
        AbilityUtil_Targeter_GrydCardinalBomb targeter = Targeter as AbilityUtil_Targeter_GrydCardinalBomb;
        if (targeter != null && targeter.m_actorToHitContext.ContainsKey(targetActor))
        {
            int numHits = targeter.m_actorToHitContext[targetActor].m_numHits;
            int numHitsFromCover = targeter.m_actorToHitContext[targetActor].m_numHitsFromCover;
            results.m_damage = ActorMultiHitContext.CalcDamageFromNumHits(
                numHits,
                numHitsFromCover,
                m_baseDamage,
                m_subseqHitDamage);
            return true;
        }

        return false;
    }

#if SERVER
    // added in rogues
    public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
        List<GrydCardinalSegmentInfo> segmentsAndHitActors = GetSegmentsAndHitActors(targets, caster, out _);
        foreach (GrydCardinalSegmentInfo segment in segmentsAndHitActors)
        {
            if (!segment.IsValidSegment())
            {
                continue;
            }

            GrydCardinalBombSequence.SegmentExtraParams segmentExtraParams =
                new GrydCardinalBombSequence.SegmentExtraParams
                {
                    m_segmentData = new List<GrydCardinalBombSequence.SegmentDataEntry>()
                };
            GrydCardinalSegmentInfo.AssembleSequenceParamData(
                segment,
                segmentExtraParams.m_segmentData);
            segmentExtraParams.m_hitActors = new List<GrydCardinalBombSequence.HitActorEntry>();
            foreach (ActorData actorData in segment.m_hitActorsMap.Keys)
            {
                AreaEffectUtils.BouncingLaserInfo bouncingLaserInfo =
                    segment.m_hitActorsMap[actorData];
                segmentExtraParams.m_hitActors.Add(
                    new GrydCardinalBombSequence.HitActorEntry
                    {
                        m_actorIndex = (sbyte)actorData.ActorIndex,
                        m_segmentIndex = (sbyte)bouncingLaserInfo.m_endpointIndex
                    });
            }

            list.Add(
                new ServerClientUtils.SequenceStartData(
                    m_projectileSequencePrefab,
                    targets[0].FreePos,
                    null,
                    caster,
                    additionalData.m_sequenceSource,
                    segmentExtraParams.ToArray()));
        }

        return list;
    }

    // added in rogues
    public override void GatherAbilityResults(
        List<AbilityTarget> targets,
        ActorData caster,
        ref AbilityResults abilityResults)
    {
        List<GrydCardinalSegmentInfo> segmentsAndHitActors = GetSegmentsAndHitActors(
            targets,
            caster,
            out Dictionary<ActorData, ActorMultiHitContext> actorToHitContext);
        foreach (ActorData actorData in actorToHitContext.Keys)
        {
            Vector3 dir = actorToHitContext[actorData].m_hitOrigin - actorData.GetFreePos();
            dir.y = 0f;
            if (dir.magnitude > 0f)
            {
                dir.Normalize();
            }

            ActorHitResults actorHitResults = MakeActorHitRes(actorData, actorData.GetFreePos() + 0.1f * dir);
            int numHits = actorToHitContext[actorData].m_numHits;
            int numHitsFromCover = actorToHitContext[actorData].m_numHitsFromCover;
            int baseDamage = ActorMultiHitContext.CalcDamageFromNumHits(
                numHits,
                numHitsFromCover,
                m_baseDamage,
                m_subseqHitDamage);
            actorHitResults.SetBaseDamage(baseDamage);
            actorHitResults.AddStandardEffectInfo(m_enemyHitEffect);
            if (actorToHitContext[actorData].m_numHitsFromCover > 0)
            {
                actorHitResults.OverrideAsInCover();
            }

            abilityResults.StoreActorHit(actorHitResults);
        }

        List<Barrier> processedBlocks = new List<Barrier>();
        Dictionary<Vector3, PositionHitResults> posToHitResults = new Dictionary<Vector3, PositionHitResults>();
        foreach (GrydCardinalSegmentInfo grydCardinalSegmentInfo in segmentsAndHitActors)
        {
            if (!grydCardinalSegmentInfo.IsValidSegment())
            {
                continue;
            }

            Vector3 segmentEndSquare = grydCardinalSegmentInfo.m_endSquare.ToVector3();
            List<NonActorTargetInfo> nonActorTargetInfos = grydCardinalSegmentInfo.m_nonActorTargetInfo;
            for (int i = nonActorTargetInfos.Count - 1; i >= 0; i--)
            {
                NonActorTargetInfo nonActorTargetInfo = nonActorTargetInfos[i];
                if (!(nonActorTargetInfo is NonActorTargetInfo_BarrierBlock block))
                {
                    continue;
                }

                PositionHitResults posHitRes = posToHitResults.TryGetValue(segmentEndSquare, out var hitResult)
                    ? hitResult
                    : MakePosHitRes(segmentEndSquare);

                posToHitResults[segmentEndSquare] = posHitRes; // custom

                if (block.m_barrier != null && !processedBlocks.Contains(block.m_barrier))
                {
                    block.AddPositionReactionHitToAbilityResults(
                        caster,
                        posHitRes,
                        abilityResults,
                        true);
                    processedBlocks.Add(block.m_barrier);
                }

                nonActorTargetInfos.RemoveAt(i);
            }
        }

        foreach (Vector3 hitPos in posToHitResults.Keys)
        {
            abilityResults.StorePositionHit(posToHitResults[hitPos]);
        }

        foreach (GrydCardinalSegmentInfo segment in segmentsAndHitActors)
        {
            if (segment.IsValidSegment())
            {
                abilityResults.StoreNonActorTargetInfo(segment.m_nonActorTargetInfo);
            }
        }
    }

    // added in rogues
    private List<GrydCardinalSegmentInfo> GetSegmentsAndHitActors(
        List<AbilityTarget> targets,
        ActorData caster,
        out Dictionary<ActorData, ActorMultiHitContext> actorToHitContext)
    {
        actorToHitContext = new Dictionary<ActorData, ActorMultiHitContext>();
        List<GrydCardinalSegmentInfo> result = new List<GrydCardinalSegmentInfo>();
        BoardSquare targetSquare = Board.Get().GetSquare(targets[0].GridPos);
        if (targetSquare == null)
        {
            return result;
        }

        result.Add(new GrydCardinalSegmentInfo(targetSquare, Vector3.forward));
        result.Add(new GrydCardinalSegmentInfo(targetSquare, Vector3.back));
        result.Add(new GrydCardinalSegmentInfo(targetSquare, Vector3.left));
        result.Add(new GrydCardinalSegmentInfo(targetSquare, Vector3.right));
        ActorData targetActor = AreaEffectUtils.GetTargetableActorOnSquare(targetSquare, true, false, caster);
        List<ActorData> actorsToExclude = new List<ActorData>();
        foreach (GrydCardinalSegmentInfo segment in result)
        {
            segment.m_hitActorsMap = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>();
            segment.m_nonActorTargetInfo = new List<NonActorTargetInfo>();
            actorsToExclude.Clear();
            GrydCardinalSegmentInfo.CalculateSegmentInfo(
                segment,
                m_maxTrunkDist,
                m_maxBranchDist,
                m_maxNumSplits,
                m_splitOnWall,
                m_splitOnActor,
                m_trunkContinueAfterActorHit,
                true,
                0,
                caster,
                caster.GetOtherTeams(),
                segment.m_nonActorTargetInfo,
                segment.m_hitActorsMap,
                actorsToExclude);
            segment.TrackActorHitInfo(actorToHitContext);
        }

        if (targetActor == null)
        {
            return result;
        }


        foreach (GrydCardinalSegmentInfo segment in result)
        {
            if (segment.IsValidSegment())
            {
                if (!segment.m_hitActorsMap.ContainsKey(targetActor))
                {
                    segment.m_hitActorsMap[targetActor] =
                        new AreaEffectUtils.BouncingLaserInfo(segment.m_startSquare.ToVector3(), 0);
                }

                break;
            }
        }

        if (actorToHitContext.ContainsKey(targetActor))
        {
            actorToHitContext[targetActor].m_numHits++;
        }
        else
        {
            actorToHitContext[targetActor] = new ActorMultiHitContext
            {
                m_numHits = 1,
                m_numHitsFromCover = 0,
                m_hitOrigin = targetActor.GetFreePos()
            };
        }

        return result;
    }

    // added in rogues
    public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
    {
        List<Vector3> result = new List<Vector3>();
        Bounds bounds = new Bounds(caster.GetFreePos(), Vector3.one);
        List<GrydCardinalSegmentInfo> segmentsAndHitActors = GetSegmentsAndHitActors(targets, caster, out _);
        foreach (GrydCardinalSegmentInfo segment in segmentsAndHitActors)
        {
            GrydCardinalSegmentInfo.EncapsulateBoundForCamPos(segment, ref bounds);
        }

        result.Add(bounds.center + bounds.extents);
        result.Add(bounds.center - bounds.extents);
        return result;
    }
#endif
}