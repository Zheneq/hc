// ROGUES
// SERVER
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_ChargeAoE : GenericAbility_TargetSelectBase
{
    [Separator("Targeting Properties")]
    public float m_radiusAroundStart = 2f;
    public float m_radiusAroundEnd = 2f;
    public float m_rangeFromLine = 2f;
    public bool m_trimPathOnTargetHit; // removed in rogues
    // rogues
    // [Tooltip("Speed of charge or evasion movement. Ignored if Movement Duration is greater than zero.")]
    // public float m_movementSpeed = 8f;
    // rogues
    // [Tooltip("Duration, in seconds, of charge or evasion movement. Ignored if less than or equal to zero.")]
    // public float m_movementDuration;
    // rogues
    // public ActorData.MovementType m_movementType = ActorData.MovementType.Charge;
    [Separator("Sequences")]
    public GameObject m_castSequencePrefab;
    public bool m_seqUseTrimmedDestAsTargetPos; // removed in rogues

    private int m_maxTargets; // public in rogues
    private TargetSelectMod_ChargeAoE m_targetSelMod; // removed in rogues

    // rogues
    // public static ContextNameKeyPair s_cvarInStart = new ContextNameKeyPair("InStart");
    // public static ContextNameKeyPair s_cvarInEnd = new ContextNameKeyPair("InEnd");

    public override string GetUsageForEditor()
    {
        // reactor
        return "Intended for single click charge abilities, with line and AoE on either end.\n"
               + GetContextUsageStr(
                   ContextKeys.s_InEndAoe.GetName(),
                   "on hit actor, 1 if in AoE near end of laser, 0 otherwise")
               + GetContextUsageStr(
                   ContextKeys.s_ChargeEndPos.GetName(),
                   "non-actor specific, charge end position",
                   false);
        // rogues
        // return "Indended for single click charge abilities. Can add shape field to hit targets on destination."
        //        + GetContextUsageStr(
        //            s_cvarInStart.GetName(),
        //            "is hit by the start location aoe")
        //        + GetContextUsageStr(
        //            s_cvarInEnd.GetName(),
        //            "is hit by the end location aoe");
    }

    public override void ListContextNamesForEditor(List<string> names)
    {
        // reactor
        names.Add(ContextKeys.s_InEndAoe.GetName());
        names.Add(ContextKeys.s_ChargeEndPos.GetName());
        // rogues
        // names.Add(new ContextNameKeyPair("InStart").GetName());
        // names.Add(new ContextNameKeyPair("InEnd").GetName());
    }

    // removed in rogues
    public float GetRadiusAroundStart()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_radiusAroundStartMod.GetModifiedValue(m_radiusAroundStart)
            : m_radiusAroundStart;
    }

    // removed in rogues
    public float GetRadiusAroundEnd()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_radiusAroundEndMod.GetModifiedValue(m_radiusAroundEnd)
            : m_radiusAroundEnd;
    }

    // removed in rogues
    public float GetRangeFromLine()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_rangeFromLineMod.GetModifiedValue(m_rangeFromLine)
            : m_rangeFromLine;
    }

    // removed in rogues
    public bool TrimPathOnTargetHit()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_trimPathOnTargetHitMod.GetModifiedValue(m_trimPathOnTargetHit)
            : m_trimPathOnTargetHit;
    }

    public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
    {
        AbilityUtil_Targeter_ChargeAoE targeter = new AbilityUtil_Targeter_ChargeAoE(
            ability,
            GetRadiusAroundStart(), // m_radiusAroundStart in rogues
            GetRadiusAroundEnd(), // m_radiusAroundEnd in rogues
            GetRangeFromLine(), // m_rangeFromLine in rogues
            m_maxTargets,
            false,
            IgnoreLos()); // m_ignoreLos in rogues
        
        // rogues
        // if (m_movementType == ActorData.MovementType.Flight
        //     || m_movementType == ActorData.MovementType.Teleport)
        // {
        //     targeter.ShowTeleportLines = true;
        // }

        // removed in rogues
        targeter.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeCaster());
        targeter.TrimPathOnTargetHit = TrimPathOnTargetHit();
        targeter.ForceAddTargetingActor = IncludeCaster();
        // end removed in rogues
        
        return new List<AbilityUtil_Targeter> { targeter };
    }

    public override bool HandleCustomTargetValidation(
        Ability ability,
        ActorData caster,
        AbilityTarget target,
        int targetIndex,
        List<AbilityTarget> currentTargets)
    {
        BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
        return targetSquare != null
               && targetSquare.IsValidForGameplay()
               && targetSquare != caster.GetCurrentBoardSquare()
               && KnockbackUtils.CanBuildStraightLineChargePath(
                   caster,
                   targetSquare,
                   caster.GetCurrentBoardSquare(),
                   false,
                   out _);
    }

    public override ActorData.MovementType GetMovementType()
    {
        return ActorData.MovementType.Charge; // m_movementType in rogues
    }

    // removed in rogues
    public static BoardSquare GetTrimOnHitDestination(
        AbilityTarget currentTarget,
        BoardSquare startSquare,
        float lineHalfWidthInSquares,
        ActorData caster,
        List<Team> relevantTeams,
        bool forServer)
    {
        BoardSquare targetSquare = Board.Get().GetSquare(currentTarget.GridPos);
        Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(
            caster,
            startSquare.ToVector3(),
            targetSquare.ToVector3(),
            out bool collision,
            out _);
        if (collision)
        {
            targetSquare = KnockbackUtils.GetLastValidBoardSquareInLine(startSquare.ToVector3(), abilityLineEndpoint);
        }

        BoardSquarePathInfo chargePath = KnockbackUtils.BuildStraightLineChargePath(
            caster, 
            targetSquare,
            startSquare,
            false);
        TrimChargePathOnActorHit(
            chargePath,
            startSquare,
            lineHalfWidthInSquares,
            caster,
            relevantTeams,
            forServer,
            out BoardSquare destSquare);
        return destSquare;
    }

    // removed in rogues
    public static void TrimChargePathOnActorHit(
        BoardSquarePathInfo chargePath,
        BoardSquare startSquare,
        float lineHalfWidthInSquares,
        ActorData caster,
        List<Team> relevantTeams,
        bool forServer,
        out BoardSquare destSquare)
    {
        destSquare = startSquare;
        if (chargePath == null || chargePath.next == null || lineHalfWidthInSquares <= 0f)
        {
            return;
        }
                
        destSquare = chargePath.GetPathEndpoint().square;
        Vector3 startLosCheckPos = startSquare.GetOccupantLoSPos();
        Vector3 destLosCheckPos = destSquare.GetOccupantLoSPos();
        List<ActorData> actors = AreaEffectUtils.GetActorsInBoxByActorRadius(
            startLosCheckPos,
            destLosCheckPos,
            2f * lineHalfWidthInSquares,
            false,
            caster,
            relevantTeams);
        actors.Remove(caster);
        if (!forServer)
        {
            TargeterUtils.RemoveActorsInvisibleToClient(ref actors);
        }
        else
        {
#if SERVER
            ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actors); // custom
#endif
        }

        Vector3 vector = destLosCheckPos - startLosCheckPos;
        vector.y = 0f;
        vector.Normalize();
        TargeterUtils.SortActorsByDistanceToPos(ref actors, startLosCheckPos, vector);
        if (actors.Count <= 0)
        {
            return;
        }

        Vector3 projectionPoint = VectorUtils.GetProjectionPoint(
            vector,
            startLosCheckPos,
            actors[0].GetLoSCheckPos());
        BoardSquarePathInfo step = chargePath.next;
        float dist = VectorUtils.HorizontalPlaneDistInWorld(projectionPoint, step.square.ToVector3());
        while (step.next != null)
        {
            float nextDist = VectorUtils.HorizontalPlaneDistInWorld(
                projectionPoint,
                step.next.square.ToVector3());
            if (nextDist > dist && step.square.IsValidForGameplay())
            {
                step.next.prev = null;
                step.next = null;
                destSquare = step.square;
                return;
            }

            dist = nextDist;
            step = step.next;
        }
    }

    // removed in rogues
    protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
    {
        m_targetSelMod = modBase as TargetSelectMod_ChargeAoE;
    }

    // removed in rogues
    protected override void OnTargetSelModRemoved()
    {
        m_targetSelMod = null;
    }

#if SERVER
    // rogues
    public override void CalcHitTargets(
        List<AbilityTarget> targets,
        ActorData caster,
        List<NonActorTargetInfo> nonActorTargetInfo)
    {
        ResetContextData();
        base.CalcHitTargets(targets, caster, nonActorTargetInfo);
        Vector3 startPos = caster.GetSquareAtPhaseStart().ToVector3();
        
        // custom
        Vector3 targetPos = GetChangeEndSquare(targets, caster).GetOccupantLoSPos();
        List<ActorData> actorsInEndAoe = AreaEffectUtils.GetActorsInRadius(
            targetPos,
            m_radiusAroundEnd,
            m_ignoreLos,
            caster,
            TargeterUtils.GetRelevantTeams(caster, m_includeAllies, m_includeEnemies),
            null);
        // rogues
        // Vector3 targetPos = Board.Get().GetSquare(targets[0].GridPos).GetOccupantLoSPos();
        
        Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
        foreach (ActorData actorData in GetHitActorsInShape(loSCheckPos, targetPos, caster, nonActorTargetInfo))
        {
            AddHitActor(actorData, startPos);
            // custom
            bool inEndAoe = actorsInEndAoe.Contains(actorData);
            SetActorContext(actorData, ContextKeys.s_InEndAoe.GetKey(), inEndAoe ? 1 : 0);
            // rogues
            // float sqrDitToStart = (actorData.GetLoSCheckPos() - startPos).sqrMagnitude;
            // float sqrDistToEnd = (actorData.GetLoSCheckPos() - targetPos).sqrMagnitude;
            // bool inStartAoe = sqrDitToStart <= m_radiusAroundStart * m_radiusAroundStart;
            // bool inEndAoe = sqrDistToEnd <= m_radiusAroundEnd * m_radiusAroundEnd;
            // SetActorContext(actorData, s_cvarInStart.GetKey(), inStartAoe ? 1 : 0);
            // SetActorContext(actorData, s_cvarInEnd.GetKey(), inEndAoe ? 1 : 0);
        }
        
        GetNonActorSpecificContext().SetValue(ContextKeys.s_ChargeEndPos.GetKey(), targetPos); // custom

        if (IncludeCaster())
        {
            AddHitActor(caster, caster.GetCurrentBoardSquare().ToVector3());
        }
    }

    // rogues
    protected List<ActorData> GetHitActorsInShape(
        Vector3 startPos,
        Vector3 endPos,
        ActorData caster,
        List<NonActorTargetInfo> nonActorTargetInfo)
    {
        List<ActorData> actorsInRadiusOfLine = AreaEffectUtils.GetActorsInRadiusOfLine(
            startPos,
            endPos,
            m_radiusAroundStart,
            m_radiusAroundEnd,
            m_rangeFromLine,
            m_ignoreLos,
            caster,
            TargeterUtils.GetRelevantTeams(caster, m_includeAllies, m_includeEnemies),
            nonActorTargetInfo);
        ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInRadiusOfLine);
        return actorsInRadiusOfLine;
    }

    // rogues
    public override List<ServerClientUtils.SequenceStartData> CreateSequenceStartData(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData,
        Sequence.IExtraSequenceParams[] extraSequenceParams = null)
    {
        return new List<ServerClientUtils.SequenceStartData>
        {
            new ServerClientUtils.SequenceStartData(
                m_castSequencePrefab,
                Board.Get().GetSquare(targets[0].GridPos),
                additionalData.m_abilityResults.HitActorsArray(),
                caster,
                additionalData.m_sequenceSource,
                extraSequenceParams)
        };
    }

    // rogues
    // protected float GetEvadeDistance(ServerEvadeUtils.ChargeSegment[] chargeSegments)
    // {
    //     float num = 0f;
    //     for (int i = 0; i < chargeSegments.Length - 1; i++)
    //     {
    //         num += chargeSegments[i].m_pos.HorizontalDistanceOnBoardTo(chargeSegments[i + 1].m_pos);
    //     }
    //
    //     return num;
    // }

    // rogues
    // internal float CalcMovementSpeed(float distance)
    // {
    //     if (m_movementDuration <= 0f)
    //     {
    //         return m_movementSpeed;
    //     }
    //
    //     return distance / m_movementDuration;
    // }

    // custom
    public override ServerEvadeUtils.ChargeSegment[] GetChargePath(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        m_chargeSegments.Clear();
        BoardSquare changeEndSquare = GetChangeEndSquare(targets, caster);
        AddChargeSegment(
            caster,
            changeEndSquare,
            BoardSquarePathInfo.ChargeCycleType.Movement,
            0, 
            true);

        return m_chargeSegments.ToArray();
    }
    // rogues
    // public override ServerEvadeUtils.ChargeSegment[] GetChargePath(
    //     List<AbilityTarget> targets,
    //     ActorData caster,
    //     ServerAbilityUtils.AbilityRunData additionalData)
    // {
    //     // float distance = caster.GetCurrentBoardSquare()
    //     //     .HorizontalDistanceOnBoardTo(Board.Get().GetSquare(targets[0].GridPos));
    //     // CalcMovementSpeed(distance); // rogues
    //     m_chargeSegments.Clear();
    //     for (int i = 0; i < targets.Count; i++)
    //     {
    //         AbilityTarget abilityTarget = targets[i];
    //         AddChargeSegment(
    //             caster,
    //             Board.Get().GetSquare(abilityTarget.GridPos),
    //             BoardSquarePathInfo.ChargeCycleType.Movement,
    //             0, // m_movementSpeed in rogues
    //             i + 1 == targets.Count);
    //     }
    //
    //     return m_chargeSegments.ToArray();
    // }

    // rogues
    public override void AddChargeSegment(
        ActorData caster,
        BoardSquare targetSquare,
        BoardSquarePathInfo.ChargeCycleType targetCycle,
        float moveSpeed,
        bool finalSegment)
    {
        if (m_chargeSegments.Count == 0 && caster != null)
        {
            m_chargeSegments.Add(new ServerEvadeUtils.ChargeSegment
            {
                m_pos = caster.GetCurrentBoardSquare(),
                m_cycle = targetCycle,
                m_end = BoardSquarePathInfo.ChargeEndType.Pivot,
                m_segmentMovementSpeed = moveSpeed
            });
        }

        m_chargeSegments.Add(new ServerEvadeUtils.ChargeSegment
        {
            m_pos = targetSquare,
            m_cycle = targetCycle,
            m_segmentMovementSpeed = moveSpeed,
            m_end = finalSegment
                ? BoardSquarePathInfo.ChargeEndType.Miss
                : BoardSquarePathInfo.ChargeEndType.Pivot
        });
    }

    // custom
    private BoardSquare GetChangeEndSquare(List<AbilityTarget> targets, ActorData caster)
    {
        return TrimPathOnTargetHit()
            ? GetTrimOnHitDestination(
                targets[0],
                caster.GetSquareAtPhaseStart(),
                GetRangeFromLine(),
                caster,
                TargeterUtils.GetRelevantTeams(caster, IncludeAllies(), IncludeEnemies()),
                true)
            : Board.Get().GetSquare(targets[0].GridPos);
    }
#endif
}