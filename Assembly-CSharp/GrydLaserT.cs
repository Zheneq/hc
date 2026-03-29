// SERVER
// ROGUES
using System.Collections.Generic;
using UnityEngine;

public class GrydLaserT : Ability
{
    [Header("-- Targeting")]
    public bool m_lockToCardinalDirections = true;
    public bool m_discreteStepsForRange = true;
    public float m_minForwardLength = 2.5f;
    public float m_maxForwardLength = 4.5f;
    public float m_laserWidth = 1f;
    public float m_branchLength = 2f;
    [Tooltip("Calculated multiplying against current distance, but only subtracted in whole integer increments")]
    public float m_branchLengthDecreaseOverDistance;
    public int m_maxTargets = 1;
    [Header("-- Damage")]
    public int m_damageAmount = 30;
    [Header("-- Sequences")]
    public GameObject m_castSequencePrefab;

    private void Start()
    {
        if (m_abilityName == "Base Ability")
        {
            m_abilityName = "Laser T";
        }

        SetupTargeter();
    }

    private void SetupTargeter()
    {
        Targeter = new AbilityUtil_Targeter_Cross(
            this,
            GetMinForwardLength(),
            GetMaxForwardLength(),
            GetBranchLength(),
            GetLaserWidth(),
            false,
            m_maxTargets,
            m_lockToCardinalDirections,
            m_discreteStepsForRange,
            false,
            false,
            m_branchLengthDecreaseOverDistance);
    }

    public float GetMinForwardLength()
    {
        return m_minForwardLength;
    }

    public float GetMaxForwardLength()
    {
        return m_maxForwardLength;
    }

    public float GetLaserWidth()
    {
        return m_laserWidth;
    }

    public float GetBranchLength()
    {
        return m_branchLength;
    }

    public int GetDamageAmount()
    {
        return m_damageAmount;
    }

    protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
    {
        List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
        AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, GetDamageAmount());
        return numbers;
    }

    private Vector3 GetClampedTargeterRange(
        AbilityTarget currentTarget,
        Vector3 startPos,
        Vector3 aimDir,
        ref float distInWorld,
        ref float branchLengthInWorld)
    {
        return GetClampedTargeterRangeStatic(
            currentTarget,
            startPos,
            aimDir,
            GetMinForwardLength(),
            GetMaxForwardLength(),
            m_discreteStepsForRange,
            m_branchLengthDecreaseOverDistance,
            ref distInWorld,
            ref branchLengthInWorld);
    }

    public static Vector3 GetClampedTargeterRangeStatic(
        AbilityTarget currentTarget,
        Vector3 startPos,
        Vector3 aimDir,
        float minForwardLenInSquares,
        float maxForwardLenInSquares,
        bool discreteStepsForRange,
        float branchLenDecreaseOverDist,
        ref float dist,
        ref float branchLengthInWorld)
    {
        Vector3 targetPos = currentTarget.FreePos;
        float squareSize = Board.Get().squareSize;
        float minForwardLen = minForwardLenInSquares * squareSize;
        float maxForwardLen = maxForwardLenInSquares * squareSize;
        Vector3 vector = targetPos - startPos;
        vector.y = 0f;
        dist = vector.magnitude;
        if (dist < minForwardLen)
        {
            targetPos = startPos + aimDir * minForwardLen;
        }
        else if (dist > maxForwardLen)
        {
            targetPos = startPos + aimDir * maxForwardLen;
            int lenRangeInSquares = Mathf.RoundToInt(maxForwardLenInSquares - minForwardLenInSquares);
            float branchLenDecrease = Mathf.Floor(branchLenDecreaseOverDist * lenRangeInSquares) * squareSize;
            branchLengthInWorld = Mathf.Max(0f, branchLengthInWorld - branchLenDecrease);
        }
        else if (discreteStepsForRange)
        {
            float lenAboveMin = Mathf.Max(0f, dist - minForwardLen);
            int lenAboveMinInSquares = Mathf.RoundToInt(lenAboveMin / squareSize);
            targetPos = startPos + aimDir * (minForwardLen + lenAboveMinInSquares * squareSize);
            lenAboveMinInSquares -= lenAboveMinInSquares % 2;
            float branchLenDecrease = Mathf.Floor(branchLenDecreaseOverDist * lenAboveMinInSquares) * squareSize;
            branchLengthInWorld = Mathf.Max(0f, branchLengthInWorld - branchLenDecrease);
        }

        vector = targetPos - startPos;
        vector.y = 0f;
        dist = vector.magnitude;
        return targetPos;
    }

#if SERVER
    // added in rogues
    public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        Vector3 aimDir = targets[0].AimDirection;
        if (m_lockToCardinalDirections)
        {
            aimDir = VectorUtils.HorizontalAngleToClosestCardinalDirection(
                Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(aimDir)));
        }

        float dist = 1f;
        float branchLength = GetBranchLength();
        Vector3 clampedTargeterRange = GetClampedTargeterRange(
            targets[0],
            caster.GetLoSCheckPos(),
            aimDir,
            ref dist,
            ref branchLength);
        Vector3 right = Vector3.Cross(aimDir, Vector3.up).normalized;
        Vector3 segmentLeft = clampedTargeterRange - right * 0.5f * branchLength;
        Vector3 segmentRight = clampedTargeterRange + right * 0.5f * branchLength;
        BouncingShotSequence.ExtraParams extraParams = new BouncingShotSequence.ExtraParams
        {
            laserTargets = new Dictionary<ActorData, AreaEffectUtils.BouncingLaserInfo>(),
            segmentPts = new List<Vector3>
            {
                caster.GetLoSCheckPos(),
                clampedTargeterRange,
                segmentLeft,
                segmentRight
            },
            useOriginalSegmentStartPos = true
        };
        return new ServerClientUtils.SequenceStartData(
            m_castSequencePrefab,
            clampedTargeterRange,
            additionalData.m_abilityResults.HitActorsArray(),
            caster,
            additionalData.m_sequenceSource,
            extraParams.ToArray());
    }

    // added in rogues
    public override void GatherAbilityResults(
        List<AbilityTarget> targets,
        ActorData caster,
        ref AbilityResults abilityResults)
    {
        List<NonActorTargetInfo> nonActorTargets = new List<NonActorTargetInfo>();
        List<ActorData> hitActors = GetHitActors(
            targets,
            caster,
            out Dictionary<ActorData, Vector3> dictionary,
            nonActorTargets);
        foreach (ActorData actorData in hitActors)
        {
            ActorHitResults actorHitResults =
                new ActorHitResults(new ActorHitParameters(actorData, dictionary[actorData]));
            actorHitResults.AddBaseDamage(GetDamageAmount());
            abilityResults.StoreActorHit(actorHitResults);
        }
    }

    // added in rogues
    private List<ActorData> GetHitActors(
        List<AbilityTarget> targets,
        ActorData caster,
        out Dictionary<ActorData, Vector3> damageOrigins,
        List<NonActorTargetInfo> nonActorTargets)
    {
        damageOrigins = new Dictionary<ActorData, Vector3>();
        float squareSize = Board.Get().squareSize;
        Vector3 vector = targets[0].AimDirection;
        Vector3 loSCheckPos = caster.GetLoSCheckPos();
        if (m_lockToCardinalDirections)
        {
            vector = VectorUtils.HorizontalAngleToClosestCardinalDirection(
                Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(vector)));
        }

        float dist = 1f;
        float branchLength = GetBranchLength() * squareSize;
        Vector3 clampedTargeterRange = GetClampedTargeterRange(
            targets[0],
            loSCheckPos,
            vector,
            ref dist,
            ref branchLength);
        Vector3 normalized = Vector3.Cross(vector, Vector3.up).normalized;
        List<ActorData> hitActors = AreaEffectUtils.GetActorsInLaser(
            loSCheckPos,
            vector,
            dist / squareSize,
            GetLaserWidth(),
            caster,
            caster.GetOtherTeams(),
            false,
            m_maxTargets,
            false,
            true,
            out _,
            nonActorTargets);
        foreach (ActorData actor in hitActors)
        {
            damageOrigins[actor] = loSCheckPos;
        }

        BoardSquare square = Board.Get().GetSquareFromVec3(clampedTargeterRange);
        if (square != null
            && square.height <= Board.Get().BaselineHeight
            && caster.GetCurrentBoardSquare().GetLOS(square.x, square.y))
        {
            BarrierManager.Get().GetAbilityLineEndpoint(
                caster,
                loSCheckPos,
                clampedTargeterRange,
                out bool collision,
                out _);

            if (!collision)
            {
                float laserRangeInSquares = 0.5f * (branchLength / squareSize);
                List<ActorData> branchHitActors = AreaEffectUtils.GetActorsInLaser(
                    clampedTargeterRange,
                    normalized,
                    laserRangeInSquares,
                    GetLaserWidth(),
                    caster,
                    caster.GetOtherTeams(),
                    false,
                    m_maxTargets,
                    false,
                    true,
                    out _,
                    nonActorTargets,
                    hitActors);
                foreach (ActorData actor in branchHitActors)
                {
                    if (damageOrigins.Count < m_maxTargets)
                    {
                        damageOrigins[actor] = clampedTargeterRange;
                    }
                }

                hitActors.AddRange(branchHitActors);
                branchHitActors = AreaEffectUtils.GetActorsInLaser(
                    clampedTargeterRange,
                    -1f * normalized,
                    laserRangeInSquares,
                    GetLaserWidth(),
                    caster,
                    caster.GetOtherTeams(),
                    false,
                    m_maxTargets,
                    false,
                    true,
                    out _,
                    nonActorTargets,
                    hitActors);
                foreach (ActorData actor in branchHitActors)
                {
                    if (damageOrigins.Count < m_maxTargets)
                    {
                        damageOrigins[actor] = clampedTargeterRange;
                    }
                }

                hitActors.AddRange(branchHitActors);
                TargeterUtils.LimitActorsToMaxNumber(ref hitActors, m_maxTargets);
            }
        }

        return hitActors;
    }
#endif
}