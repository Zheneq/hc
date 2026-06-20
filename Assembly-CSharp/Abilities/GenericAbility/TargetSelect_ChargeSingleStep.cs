// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

public class TargetSelect_ChargeSingleStep : GenericAbility_TargetSelectBase
{
    // rogues
    // public const string c_isDirectHit = "IsDirectHit";
    // public static ContextNameKeyPair s_isDirectHit = new ContextNameKeyPair(c_isDirectHit);
    //
    // public int m_maxTargets;
    // public int m_maxRange = 1;
    // public float m_chargeWidth = 1f;
    // public bool m_chargeThroughInvalidSquares;
    // public bool m_bounceHitTargeting;
    // [Tooltip("Relevant if bouncesSequence flag is set to true")]
    // private bool m_directHitIgnoreCover;
    // [Tooltip("Relevant if bounceHitTargeting flag is set to true")]
    // private bool m_trackNumHits;
    // public ActorData.MovementType m_movementType = ActorData.MovementType.Charge;
    // [Tooltip("Whether to set up charge like battlemonk charge with pivots and recovery")]
    // public bool m_chargeWithPivotAndRecovery;
    // [Tooltip("Only relevant if using pivot and recovery charge setup")]
    // public float m_recoveryTime = 0.5f;
    [Separator("Targeting Properties")]
    public AbilityAreaShape m_destShape;
    [Separator("Sequences")]
    public GameObject m_castSequencePrefab;
    // public GameObject m_chargeSequencePrefab; // rogues
    // public GameObject m_aoeSequencePrefab; // rogues

    private TargetSelectMod_ChargeSingleStep m_targetSelMod;

    public override string GetUsageForEditor()
    {
        return "Intended for single click charge abilities. Can add shape field to hit targets on destination.";
        // rogues
        // + "\n"
        // + GetContextUsageStr(
        //     s_isDirectHit.GetName(),
        //     "1 if hit by the charge laser (Bounce Hit Targeting only), 0 if hit by the AoE")
        // + GetContextUsageStr(
        //     ContextKeys.s_directHitSquareCount.GetName(),
        //     "the number of squares touched by the charge laser");
    }

    // rogues
    // public override void ListContextNamesForEditor(List<string> names)
    // {
    //     names.Add(s_isDirectHit.GetName());
    //     names.Add(ContextKeys.s_directHitSquareCount.GetName());
    // }

    public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
    {
        // rogues
        // if (m_bounceHitTargeting)
        // {
        //     AbilityUtil_Targeter_ClaymoreCharge targeter = new AbilityUtil_Targeter_ClaymoreCharge(
        //         ability,
        //         m_chargeWidth,
        //         m_maxRange,
        //         GetDestShape(),
        //         m_directHitIgnoreCover);
        //     if (m_trackNumHits)
        //     {
        //         targeter.m_affectCasterDelegate = (caster, actorsSoFar) => actorsSoFar.Count > 0;
        //     }
        //
        //     return new List<AbilityUtil_Targeter>
        //     {
        //         targeter
        //     };
        // }
        // else
        // {
            return new List<AbilityUtil_Targeter>
            {
                new AbilityUtil_Targeter_Charge(
                    ability,
                    GetDestShape(),
                    IgnoreLos(),
                    AbilityUtil_Targeter_Shape.DamageOriginType.CenterOfShape,
                    IncludeEnemies(),
                    IncludeAllies())
            };
        // }
    }

    protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
    {
        m_targetSelMod = modBase as TargetSelectMod_ChargeSingleStep;
    }

    protected override void OnTargetSelModRemoved()
    {
        m_targetSelMod = null;
    }

    public AbilityAreaShape GetDestShape()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_destShapeMod.GetModifiedValue(m_destShape)
            : m_destShape;
    }

    public override bool HandleCustomTargetValidation(
        Ability ability,
        ActorData caster,
        AbilityTarget target,
        int targetIndex,
        List<AbilityTarget> currentTargets)
    {
        // rogues
        // if (m_bounceHitTargeting)
        // {
        //     return true;
        // }

        BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
        if (targetSquare != null
            && targetSquare.IsValidForGameplay()
            && targetSquare != caster.GetCurrentBoardSquare())
        {
            // reactor
            bool passThroughInvalidSquares = false;
            // rogues
            // ActorData.MovementType movementType = GetMovementType();
            // bool passThroughInvalidSquares = movementType == ActorData.MovementType.Flight
            //                                  || movementType == ActorData.MovementType.Teleport;
            return KnockbackUtils.CanBuildStraightLineChargePath(
                caster,
                targetSquare,
                caster.GetCurrentBoardSquare(),
                passThroughInvalidSquares,
                out _);
        }

        return false;
    }

    public override ActorData.MovementType GetMovementType()
    {
        // reactor
        return ActorData.MovementType.Charge;
        // rogues
        // return m_movementType;
    }
    
#if SERVER
    // rogues
    // public override BoardSquare GetValidChargeTestSourceSquare(ServerEvadeUtils.ChargeSegment[] chargeSegments)
    // {
    //     if (m_chargeWithPivotAndRecovery)
    //     {
    //         return chargeSegments[chargeSegments.Length - 1].m_pos;
    //     }
    //
    //     return base.GetValidChargeTestSourceSquare(chargeSegments);
    // }

    // rogues
    // public override Vector3 GetChargeBestSquareTestVector(ServerEvadeUtils.ChargeSegment[] chargeSegments)
    // {
    //     if (m_chargeWithPivotAndRecovery)
    //     {
    //         return ServerEvadeUtils.GetChargeBestSquareTestDirection(chargeSegments);
    //     }
    //
    //     return base.GetChargeBestSquareTestVector(chargeSegments);
    // }

    // rogues
    // public override bool GetChargeThroughInvalidSquares()
    // {
    //     return m_chargeThroughInvalidSquares;
    // }

    // rogues
    public override BoardSquare GetIdealDestination(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        // custom
        return Board.Get().GetSquare(targets[0].GridPos);
        // rogues
        // return GetPathDestinationAndEndPoints(targets, caster, out _);
    }

    // rogues
    public override void ApplyMovementSpeed(ServerEvadeUtils.ChargeSegment[] chargePath, float movementSpeed)
    {
        foreach (ServerEvadeUtils.ChargeSegment segment in chargePath)
        {
            if (segment.m_cycle == BoardSquarePathInfo.ChargeCycleType.Movement)
            {
                segment.m_segmentMovementSpeed = movementSpeed;
            }
        }
    }

    // rogues
    public override List<ServerClientUtils.SequenceStartData> CreateSequenceStartData(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData,
        Sequence.IExtraSequenceParams[] extraSequenceParams = null)
    {
        List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
        // if (m_bounceHitTargeting)
        // {
        //     Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
        //     List<ActorData> bounceHitActors = GetBounceHitActors(targets, loSCheckPos, caster, out Vector3 bounceEndPoint, null);
        //     List<Sequence.IExtraSequenceParams> sequenceExtraParams = new List<Sequence.IExtraSequenceParams>();
        //     if (extraSequenceParams != null)
        //     {
        //         sequenceExtraParams.AddRange(extraSequenceParams);
        //     }
        //
        //     BouncingShotSequence.ExtraParams extraParams = new BouncingShotSequence.ExtraParams
        //     {
        //         laserTargets = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>()
        //     };
        //     foreach (ActorData hitActor in bounceHitActors)
        //     {
        //         extraParams.laserTargets.Add(hitActor, new AreaEffectUtils.BouncingLaserInfo(loSCheckPos, 0));
        //     }
        //     extraParams.segmentPts = new List<Vector3> { bounceEndPoint };
        //     
        //     sequenceExtraParams.Add(extraParams);
        //     list.Add(new ServerClientUtils.SequenceStartData(
        //         m_chargeSequencePrefab,
        //         caster.GetCurrentBoardSquare(),
        //         new ActorData[0],
        //         caster,
        //         additionalData.m_sequenceSource,
        //         sequenceExtraParams.ToArray()));
        //     if (bounceHitActors.Count > 0)
        //     {
        //         List<ActorData> nonBounceHitActors = additionalData.m_abilityResults.HitActorList();
        //         foreach (ActorData hitActor in bounceHitActors)
        //         {
        //             nonBounceHitActors.Remove(hitActor);
        //         }
        //
        //         list.Add(new ServerClientUtils.SequenceStartData(
        //             m_aoeSequencePrefab,
        //             caster.GetFreePos(),
        //             additionalData.m_abilityResults.HitActorsArray(),
        //             caster,
        //             additionalData.m_sequenceSource));
        //     }
        // }
        // else
        // {
            list.Add(new ServerClientUtils.SequenceStartData(
                m_castSequencePrefab,
                Board.Get().GetSquare(targets[0].GridPos),
                additionalData.m_abilityResults.HitActorsArray(),
                caster,
                additionalData.m_sequenceSource));
        // }

        return list;
    }

    // rogues
    public override void CalcHitTargets(
        List<AbilityTarget> targets,
        ActorData caster,
        List<NonActorTargetInfo> nonActorTargetInfo)
    {
        ResetContextData();
        base.CalcHitTargets(targets, caster, nonActorTargetInfo);
        // if (m_bounceHitTargeting)
        // {
        //     Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
        //     List<ActorData> bounceHitActors = GetBounceHitActors(
        //         targets,
        //         loSCheckPos,
        //         caster,
        //         out Vector3 hitOrigin,
        //         nonActorTargetInfo);
        //     List<ActorData> nonBounceHitActors;
        //     if (bounceHitActors.Count > 0)
        //     {
        //         nonBounceHitActors = GetHitActorsInShape(targets, caster, nonActorTargetInfo);
        //         foreach (ActorData bounceHitActor in bounceHitActors)
        //         {
        //             nonBounceHitActors.Remove(bounceHitActor);
        //         }
        //     }
        //     else
        //     {
        //         nonBounceHitActors = new List<ActorData>();
        //     }
        //
        //     int numSquaresInProcessedEvade = ServerActionBuffer.Get().GetNumSquaresInProcessedEvade(caster);
        //     GetNonActorSpecificContext().SetValue(
        //         ContextKeys.s_directHitSquareCount.GetKey(),
        //         Mathf.Max(0, numSquaresInProcessedEvade - 1));
        //     foreach (ActorData actor in bounceHitActors)
        //     {
        //         AddHitActor(actor, loSCheckPos);
        //         SetActorContext(actor, s_isDirectHit.GetKey(), 1);
        //     }
        //
        //     foreach (ActorData actor in nonBounceHitActors)
        //     {
        //         AddHitActor(actor, hitOrigin);
        //         SetActorContext(actor, s_isDirectHit.GetKey(), 0);
        //     }
        //
        //     return;
        // }

        Vector3 hitOrigin = Board.Get().GetSquare(targets[0].GridPos).ToVector3();
        foreach (ActorData actor in GetHitActorsInShape(targets, caster, nonActorTargetInfo))
        {
            AddHitActor(actor, hitOrigin);
        }
    }

    // rogues
    private List<ActorData> GetHitActorsInShape(
        List<AbilityTarget> targets,
        ActorData caster,
        List<NonActorTargetInfo> nonActorTargetInfo)
    {
        List<ActorData> actorsInShape = AreaEffectUtils.GetActorsInShape(
            GetDestShape(),
            targets[0],
            IgnoreLos(),
            caster,
            TargeterUtils.GetRelevantTeams(caster, IncludeAllies(), IncludeEnemies()),
            nonActorTargetInfo);
        ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInShape);
        return actorsInShape;
    }

    // rogues
    // private List<ActorData> GetBounceHitActors(
    //     List<AbilityTarget> targets,
    //     Vector3 startPos,
    //     ActorData caster,
    //     out Vector3 bounceEndPoint,
    //     List<NonActorTargetInfo> nonActorTargetInfoInSegments)
    // {
    //     AbilityTarget abilityTarget = targets[0];
    //     Vector3 aimDirection = abilityTarget.AimDirection;
    //     float magnitude = (abilityTarget.FreePos - startPos).magnitude;
    //     bounceEndPoint = VectorUtils.GetLaserEndPoint(
    //         startPos,
    //         aimDirection,
    //         magnitude,
    //         false,
    //         caster,
    //         nonActorTargetInfoInSegments);
    //     magnitude = (bounceEndPoint - startPos).magnitude;
    //     List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
    //         startPos,
    //         aimDirection,
    //         magnitude / Board.Get().squareSize,
    //         m_chargeWidth,
    //         caster,
    //         caster.GetOtherTeams(),
    //         true,
    //         0,
    //         true,
    //         true,
    //         out _,
    //         nonActorTargetInfoInSegments);
    //     ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInLaser);
    //     TargeterUtils.SortActorsByDistanceToPos(ref actorsInLaser, startPos);
    //     TargeterUtils.LimitActorsToMaxNumber(ref actorsInLaser, m_maxTargets);
    //     if (!actorsInLaser.IsNullOrEmpty())
    //     {
    //         bounceEndPoint = actorsInLaser[0].GetLoSCheckPos();
    //     }
    //
    //     return actorsInLaser;
    // }

    // rogues
    // public static BoardSquare GetClosestSquareToLaser(BoardSquare currentDest, Vector3 startPos, Vector3 endPos)
    // {
    //     Vector3 vector = endPos - startPos;
    //     vector.y = 0f;
    //     vector.Normalize();
    //     List<BoardSquare> adjacentSquares = new List<BoardSquare>();
    //     Board.Get().GetAllAdjacentSquares(currentDest.x, currentDest.y, ref adjacentSquares);
    //     Vector3 vector2 = currentDest.ToVector3() - startPos;
    //     vector2.y = 0f;
    //     Vector3 vector3 = Vector3.Dot(vector2, vector) * vector + startPos;
    //     Vector3 vector4 = currentDest.ToVector3();
    //     vector4.y = vector3.y;
    //     float magnitude = (vector4 - vector3).magnitude;
    //     BoardSquare result = currentDest;
    //     foreach (BoardSquare boardSquare in adjacentSquares)
    //     {
    //         if (boardSquare.IsValidForGameplay())
    //         {
    //             Vector3 vector5 = boardSquare.ToVector3();
    //             vector5.y = vector3.y;
    //             if ((vector5 - vector3).magnitude < magnitude)
    //             {
    //                 result = boardSquare;
    //             }
    //         }
    //     }
    //
    //     return result;
    // }

    // rogues
    // private BoardSquare GetPathDestinationAndEndPoints(
    //     List<AbilityTarget> targets,
    //     ActorData caster,
    //     out Vector3 endPoint)
    // {
    //     Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
    //     ActorData.MovementType movementType = GetMovementType();
    //     BoardSquare boardSquare;
    //     Vector3 vector;
    //     if (movementType == ActorData.MovementType.Teleport || movementType == ActorData.MovementType.Flight)
    //     {
    //         boardSquare = Board.Get().GetSquare(targets[0].GridPos);
    //         boardSquare = GetClosestSquareToLaser(boardSquare, loSCheckPos, targets[0].GetWorldGridPos());
    //         vector = boardSquare.ToVector3();
    //     }
    //     else
    //     {
    //         List<ActorData> bounceHitActors = GetBounceHitActors(targets, loSCheckPos, caster, out vector, null);
    //         float magnitude = (vector - loSCheckPos).magnitude;
    //         float num = Mathf.Min(0.5f, magnitude / 2f);
    //         Vector3 end = vector - targets[0].AimDirection * num;
    //         if (m_maxTargets > 0 && bounceHitActors.Count >= m_maxTargets)
    //         {
    //             boardSquare = bounceHitActors[bounceHitActors.Count - 1].GetCurrentBoardSquare();
    //         }
    //         else
    //         {
    //             boardSquare = KnockbackUtils.GetLastValidBoardSquareInLine(loSCheckPos, end, true);
    //             boardSquare = GetClosestSquareToLaser(boardSquare, loSCheckPos, vector);
    //             BoardSquarePathInfo path = KnockbackUtils.BuildStraightLineChargePath(
    //                 caster,
    //                 boardSquare,
    //                 caster.GetSquareAtPhaseStart(),
    //                 true);
    //             if (path != null && path.next != null)
    //             {
    //                 BoardSquarePathInfo step = path;
    //                 int i = 0;
    //                 while (step.next != null)
    //                 {
    //                     BoardSquare square = step.next.square;
    //                     if (step.square.IsValidForKnockbackAndCharge()
    //                         && !square.IsValidForKnockbackAndCharge()
    //                         && i > 0)
    //                     {
    //                         boardSquare = step.square;
    //                         break;
    //                     }
    //
    //                     step = step.next;
    //                     i++;
    //                 }
    //             }
    //         }
    //     }
    //
    //     endPoint = vector;
    //     return boardSquare;
    // }

    // rogues
    public override ServerEvadeUtils.ChargeSegment[] GetChargePath(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        // rogues
        // BoardSquare pathDestinationAndEndPoints = GetPathDestinationAndEndPoints(targets, caster, out Vector3 endPoint);
        // if (m_chargeWithPivotAndRecovery)
        // {
        //     return ServerEvadeUtils.GetChargeSegmentForStopOnTargetHit(
        //         caster,
        //         new List<Vector3> { endPoint },
        //         pathDestinationAndEndPoints,
        //         m_recoveryTime);
        // }

        ServerEvadeUtils.ChargeSegment[] array = {
            new ServerEvadeUtils.ChargeSegment
            {
                m_pos = caster.GetCurrentBoardSquare(),
                m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
                m_end = BoardSquarePathInfo.ChargeEndType.Impact
            },
            new ServerEvadeUtils.ChargeSegment
            {
                m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
                m_pos = Board.Get().GetSquare(targets[0].GridPos)
            }
        };
        // rogues
        // Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
        // List<ActorData> bounceHitActors = GetBounceHitActors(targets, loSCheckPos, caster, out _, null);
        // array[array.Length - 1].m_end = bounceHitActors.Count > 0
        //     ? BoardSquarePathInfo.ChargeEndType.Impact
        //     : BoardSquarePathInfo.ChargeEndType.Miss;
        // array[array.Length - 1].m_pos = pathDestinationAndEndPoints;
        return array;
    }

    // rogues
    public override List<Vector3> CalcPointsOfInterestForCamera(List<AbilityTarget> targets, ActorData caster)
    {
        List<Vector3> list = new List<Vector3>();
        // rogues
        // Vector3 freePos = caster.GetFreePos(caster.GetSquareAtPhaseStart());
        // List<ActorData> bounceHitActors = GetBounceHitActors(targets, freePos, caster, out Vector3 bounceEndPoint, null);
        // list.Add(bounceEndPoint);
        // if (bounceHitActors != null)
        // {
        //     foreach (ActorData hitActor in bounceHitActors)
        //     {
        //         list.Add(hitActor.GetFreePos());
        //     }
        // }

        foreach (AbilityTarget target in targets)
        {
            list.Add(target.FreePos);
        }

        return list;
    }
#endif
}