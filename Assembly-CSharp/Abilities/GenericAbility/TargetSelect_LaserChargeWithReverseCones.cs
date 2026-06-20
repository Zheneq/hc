// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_LaserChargeWithReverseCones : GenericAbility_TargetSelectBase
{
    [Separator("Targeting Properties")]
    public float m_laserRange = 5f;
    public float m_laserWidth = 1f;
    [Header("Cone Properties")]
    public ConeTargetingInfo m_coneInfo;
    [Space(10f)]
    public int m_coneCount = 3;
    public float m_coneStartOffset;
    public float m_perConeHorizontalOffset;
    public float m_angleInBetween = 10f;
    [Separator("Sequences")]
    public GameObject m_castSequencePrefab;
    public GameObject m_coneSequencePrefab;

    private const string c_directChargeHit = "DirectChargeHit";
    public static ContextNameKeyPair s_cvarDirectChargeHit = new ContextNameKeyPair(c_directChargeHit);

    private TargetSelectMod_LaserChargeWithReverseCones m_targetSelMod;
    private ConeTargetingInfo m_cachedConeInfo;

    public override string GetUsageForEditor()
    {
        return GetContextUsageStr(
            s_cvarDirectChargeHit.GetName(),
            "whether this is a direct charge hit or not (if not, it's a cone hit)");
    }

    public override void ListContextNamesForEditor(List<string> names)
    {
        names.Add(c_directChargeHit);
    }

    public override void Initialize()
    {
        SetCachedFields();

        // removed in rogues
        ConeTargetingInfo coneInfo = GetConeInfo();
        coneInfo.m_affectsAllies = IncludeAllies();
        coneInfo.m_affectsEnemies = IncludeEnemies();
        coneInfo.m_affectsCaster = IncludeCaster();
        coneInfo.m_penetrateLos = IgnoreLos();
        // end removed in rogues
    }

    public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
    {
        return new List<AbilityUtil_Targeter>
        {
            new AbilityUtil_Targeter_LaserChargeReverseCones(
                ability,
                GetLaserWidth(),
                GetLaserRange(),
                GetConeInfo(),
                GetConeCount(),
                GetConeStartOffset(),
                GetPerConeHorizontalOffset(),
                GetAngleInBetween(),
                GetConeOrigins,
                GetConeDirections)
            {
                m_coneLosCheckDelegate = CustomLosForCone // removed in rogues
            }
        };
    }

    protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
    {
        m_targetSelMod = modBase as TargetSelectMod_LaserChargeWithReverseCones;
    }

    protected override void OnTargetSelModRemoved()
    {
        m_targetSelMod = null;
    }

    private void SetCachedFields()
    {
        m_cachedConeInfo = m_targetSelMod != null
            ? m_targetSelMod.m_coneInfoMod.GetModifiedValue(m_coneInfo)
            : m_coneInfo;
    }

    public float GetLaserRange()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_laserRangeMod.GetModifiedValue(m_laserRange)
            : m_laserRange;
    }

    public float GetLaserWidth()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_laserWidthMod.GetModifiedValue(m_laserWidth)
            : m_laserWidth;
    }

    public ConeTargetingInfo GetConeInfo()
    {
        return m_cachedConeInfo ?? m_coneInfo;
    }

    public int GetConeCount()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_coneCountMod.GetModifiedValue(m_coneCount)
            : m_coneCount;
    }

    public float GetConeStartOffset()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_coneStartOffsetMod.GetModifiedValue(m_coneStartOffset)
            : m_coneStartOffset;
    }

    public float GetPerConeHorizontalOffset()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_perConeHorizontalOffsetMod.GetModifiedValue(m_perConeHorizontalOffset)
            : m_perConeHorizontalOffset;
    }

    public float GetAngleInBetween()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_angleInBetweenMod.GetModifiedValue(m_angleInBetween)
            : m_angleInBetween;
    }

    public override ActorData.MovementType GetMovementType()
    {
        return ActorData.MovementType.Charge;
    }

    protected List<Vector3> GetConeOrigins(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
    {
        List<Vector3> list = new List<Vector3>();
        List<Vector3> coneDirections = GetConeDirections(currentTarget, targeterFreePos, caster);

        Vector3 reverseConeDir = -currentTarget.AimDirection;
        reverseConeDir.Normalize();
        Vector3 reverseConeRight = Vector3.Cross(reverseConeDir, Vector3.up).normalized;

        float coneStartOffset = GetConeStartOffset() * Board.SquareSizeStatic;
        Vector3 coneEndPos = targeterFreePos + coneStartOffset * reverseConeDir;
        for (int i = 0; i < coneDirections.Count; i++)
        {
            float offset = GetPerConeHorizontalOffset() * (i - coneDirections.Count / 2);
            list.Add(coneEndPos
                     + reverseConeRight * offset
                     - GetConeInfo().m_radiusInSquares * Board.SquareSizeStatic * coneDirections[i]);
        }

        return list;
    }

    public virtual List<Vector3> GetConeDirections(
        AbilityTarget currentTarget,
        Vector3 targeterFreePos,
        ActorData caster)
    {
        List<Vector3> list = new List<Vector3>();
        int coneCount = GetConeCount();
        float angleInBetween = GetAngleInBetween();
        float aimAngle = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection);
        float startAngle = aimAngle + 0.5f * (coneCount - 1) * angleInBetween;
        for (int i = 0; i < coneCount; i++)
        {
            list.Add(-VectorUtils.AngleDegreesToVector(startAngle - i * angleInBetween));
        }

        return list;
    }

    // removed in rogues
    public static bool CustomLosForCone(
        ActorData actor,
        ActorData caster,
        Vector3 chargeEndPos,
        List<NonActorTargetInfo> nonActorTargetInfo)
    {
        BoardSquare chargeEndSquare = Board.Get().GetSquareFromVec3(chargeEndPos);
        return chargeEndSquare != null && AreaEffectUtils.SquaresHaveLoSForAbilities(
            chargeEndSquare,
            actor.GetCurrentBoardSquare(),
            caster,
            true,
            nonActorTargetInfo);
    }

#if SERVER
    // custom
    public override bool GetChargeThroughInvalidSquares()
    {
        return true;
    }
    
    // rogues
    public override ServerEvadeUtils.ChargeSegment[] GetChargePath(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        // custom
        BoardSquare chargeEndSquare = GetChargeEndSquare(targets, caster);
        // rogues
        // Vector3 chargeEndPos = GetChargeEndPos(targets, caster, out _);
        return new[]
        {
            new ServerEvadeUtils.ChargeSegment
            {
                m_pos = caster.GetCurrentBoardSquare(),
                m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
                m_end = BoardSquarePathInfo.ChargeEndType.Impact
            },
            new ServerEvadeUtils.ChargeSegment
            {
                m_cycle = BoardSquarePathInfo.ChargeCycleType.Movement,
                // custom
                m_pos = chargeEndSquare
                // rogues
                // m_pos = Board.Get().GetSquareFromVec3(chargeEndPos)
            }
        };
    }
    
    // custom
    private BoardSquare GetChargeEndSquare(List<AbilityTarget> targets, ActorData caster)
    {
        Vector3 destPos = GetChargeEndPos(targets, caster, out ActorData directHitActor);
        BoardSquare destSquare = ClaymoreCharge.GetChargeDestinationSquare(
            caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart()),
            destPos,
            directHitActor,
            null,
            caster,
            false);
        BoardSquarePathInfo boardSquarePathInfo = KnockbackUtils.BuildStraightLineChargePath(
            caster,
            destSquare,
            caster.GetSquareAtPhaseStart(),
            true);
        if (destSquare != null
            && destSquare.OccupantActor != null
            && destSquare.OccupantActor != caster
            && !ServerActionBuffer.Get().ActorIsEvading(destSquare.OccupantActor))
        {
            destSquare = AbilityUtil_Targeter_ClaymoreCharge.GetChargeDestination(caster, destSquare, boardSquarePathInfo);
        }

        return destSquare;
    }

    // rogues
    private Vector3 GetChargeEndPos(List<AbilityTarget> targets, ActorData caster, out ActorData directHitActor)
    {
        Vector3 loSCheckPos = caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart());
        Vector3 laserEndPoint = VectorUtils.GetLaserEndPoint(
            loSCheckPos,
            targets[0].AimDirection,
            GetLaserRange() * Board.Get().squareSize,
            false,
            caster);
        float magnitude = (laserEndPoint - loSCheckPos).magnitude;
        // custom
        magnitude = ClaymoreCharge.GetMaxPotentialChargeDistance(
            loSCheckPos,
            laserEndPoint,
            targets[0].AimDirection,
            magnitude,
            caster,
            out BoardSquare pathEndSquare);
        BoardSquarePathInfo path = KnockbackUtils.BuildStraightLineChargePath(
            caster,
            pathEndSquare,
            caster.GetSquareAtPhaseStart(),
            true);
        List<ActorData> actorsOnPath = ClaymoreCharge.GetActorsOnPath(path, caster.GetOtherTeams(), caster);
        List<ActorData> evaders = GameFlowData.Get().GetActors().Where(ServerActionBuffer.Get().ActorIsEvading).ToList();
        // end custom
        List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
            loSCheckPos,
            targets[0].AimDirection,
            magnitude / Board.Get().squareSize,
            GetLaserWidth(),
            caster,
            caster.GetOtherTeams(),
            true, // false in targeter, but doesn't really matter as we offset laser range
            1,
            true,
            true,
            out laserEndPoint,
            null,
            evaders); // custom, null in rogues
        // custom
        actorsInLaser.AddRange(actorsOnPath);
        TargeterUtils.SortActorsByDistanceToPos(ref actorsInLaser, loSCheckPos);
        // end custom
        ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInLaser);
        directHitActor = actorsInLaser.IsNullOrEmpty() ? null : actorsInLaser[0];
        // custom
        if (directHitActor != null)
        {
            Vector3 lhs = directHitActor.GetFreePos() - loSCheckPos;
            lhs.y = 0f;
            laserEndPoint = loSCheckPos + Vector3.Dot(lhs, targets[0].AimDirection) * targets[0].AimDirection;
        }
        // end custom
        Vector3 vector = laserEndPoint - loSCheckPos;
        vector.y = 0f;
        float magnitude2 = vector.magnitude;
        vector.Normalize();
        return laserEndPoint - Mathf.Min(0.5f, magnitude2 / 2f) * vector;
    }

    // rogues
    public override List<ServerClientUtils.SequenceStartData> CreateSequenceStartData(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData,
        Sequence.IExtraSequenceParams[] extraSequenceParams = null)
    {
        List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
        bool includeCaster = additionalData.m_abilityResults.HitActorList().Contains(caster);
        GetHitActorsAndHitCount(
            targets,
            caster,
            out List<List<ActorData>> actorsForSequence,
            out List<Vector3> coneEndPosList,
            out List<Vector3> coneStartPosList,
            out _,
            out ActorData directChargeHit,
            out Vector3 chargeEndPos,
            null);
        list.Add(
            new ServerClientUtils.SequenceStartData(
                m_castSequencePrefab,
                chargeEndPos,
                directChargeHit != null ? new[] { directChargeHit } : null,
                caster,
                additionalData.m_sequenceSource,
                extraSequenceParams));
        ConeTargetingInfo coneInfo = GetConeInfo();
        for (int i = 0; i < actorsForSequence.Count; i++)
        {
            list.Add(
                new ServerClientUtils.SequenceStartData(
                    m_coneSequencePrefab,
                    coneStartPosList[i],
                    actorsForSequence[i].ToArray(),
                    caster,
                    additionalData.m_sequenceSource,
                    new List<Sequence.IExtraSequenceParams>
                    {
                        new BlasterStretchConeSequence.ExtraParams
                        {
                            lengthInSquares = coneInfo.m_radiusInSquares,
                            angleInDegrees = coneInfo.m_widthAngleDeg,
                            forwardAngle = VectorUtils.HorizontalAngle_Deg(coneEndPosList[i] - coneStartPosList[i]),
                            useStartPosOverride = true,
                            startPosOverride = coneStartPosList[i]
                        }
                    }.ToArray()));
        }

        if (includeCaster)
        {
            list.Add(
                new ServerClientUtils.SequenceStartData(
                    SequenceLookup.Get().GetSimpleHitSequencePrefab(),
                    caster.GetFreePos(),
                    caster.AsArray(),
                    caster,
                    additionalData.m_sequenceSource,
                    extraSequenceParams));
        }

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
        Dictionary<ActorData, int> hitActorsAndHitCount = GetHitActorsAndHitCount(
            targets,
            caster,
            out _,
            out _,
            out _,
            out _,
            out ActorData actorData,
            out Vector3 chargeEndPos,
            nonActorTargetInfo);
        if (actorData != null)
        {
            AddHitActor(actorData, actorData.GetLoSCheckPos());
            SetActorContext(actorData, s_cvarDirectChargeHit.GetKey(), 1);
        }

        foreach (ActorData hitActor in hitActorsAndHitCount.Keys)
        {
            AddHitActor(hitActor, chargeEndPos); // caster.GetLoSCheckPos(caster.GetSquareAtPhaseStart()) in rogues
            SetActorContext(hitActor, s_cvarDirectChargeHit.GetKey(), 0);
            SetActorContext(hitActor, ContextKeys.s_HitCount.GetKey(), hitActorsAndHitCount[hitActor]);
        }
    }

    // rogues
    private Dictionary<ActorData, int> GetHitActorsAndHitCount(
        List<AbilityTarget> targets,
        ActorData caster,
        out List<List<ActorData>> actorsForSequence,
        out List<Vector3> coneEndPosList,
        out List<Vector3> coneStartPosList,
        out int numConesWithHits,
        out ActorData directChargeHit,
        out Vector3 chargeEndPos,
        List<NonActorTargetInfo> nonActorTargetInfo)
    {
        actorsForSequence = new List<List<ActorData>>();
        coneEndPosList = new List<Vector3>();
        numConesWithHits = 0;
        Dictionary<ActorData, int> actorToHitCount = new Dictionary<ActorData, int>();
        chargeEndPos = GetChargeEndPos(targets, caster, out directChargeHit);
        GetNonActorSpecificContext().SetValue(ContextKeys.s_KnockbackOrigin.GetKey(), chargeEndPos);
        List<Vector3> coneDirections = GetConeDirections(targets[0], chargeEndPos, caster);
        coneStartPosList = GetConeOrigins(targets[0], chargeEndPos, caster);
        ConeTargetingInfo coneInfo = GetConeInfo();
        for (int i = 0; i < coneDirections.Count; i++)
        {
            Vector3 coneDir = coneDirections[i];
            Vector3 coneOrigin = coneStartPosList[i];
            List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(
                coneOrigin,
                VectorUtils.HorizontalAngle_Deg(coneDir),
                coneInfo.m_widthAngleDeg,
                coneInfo.m_radiusInSquares,
                coneInfo.m_backwardsOffset,
                coneInfo.m_penetrateLos,
                caster,
                TargeterUtils.GetRelevantTeams(caster, coneInfo.m_affectsAllies, coneInfo.m_affectsEnemies),
                nonActorTargetInfo);
            ServerAbilityUtils.RemoveEvadersFromHitTargets(ref actorsInCone);
            if (directChargeHit != null)
            {
                actorsInCone.Remove(directChargeHit);
            }

            if (coneInfo.m_affectsCaster && i == 0)
            {
                actorsInCone.Add(caster);
            }

            foreach (ActorData actorData in actorsInCone.ToArray())
            {
                // custom
                if (!CustomLosForCone(actorData, caster, chargeEndPos, nonActorTargetInfo))
                // rogues
                // if (!CustomLosForCone(actorData, caster, nonActorTargetInfo))
                {
                    actorsInCone.Remove(actorData);
                }
            }

            actorsForSequence.Add(actorsInCone);
            coneEndPosList.Add(coneOrigin + coneInfo.m_radiusInSquares * Board.SquareSizeStatic * coneDir);
            if (actorsInCone.Count > 0)
            {
                numConesWithHits++;
            }

            foreach (ActorData actorData in actorsInCone)
            {
                if (actorToHitCount.ContainsKey(actorData))
                {
                    actorToHitCount[actorData]++;
                }
                else
                {
                    actorToHitCount[actorData] = 1;
                }
            }
        }

        return actorToHitCount;
    }

    // rogues
    protected bool CustomLosForCone(
        ActorData actor,
        ActorData caster,
        List<NonActorTargetInfo> nonActorTargetInfo)
    {
        BoardSquare actorSquare = actor.GetCurrentBoardSquare();
        BoardSquare casterSquare = caster.GetCurrentBoardSquare();
        return casterSquare.GetLOS(actorSquare.x, actorSquare.y)
               && !BarrierManager.Get().AreAbilitiesBlocked(caster, casterSquare, actorSquare, nonActorTargetInfo);
    }
#endif
}