// ROGUES
// SERVER
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_FanCones : GenericAbility_TargetSelectBase
{
    [Separator("Targeting Properties")]
    public ConeTargetingInfo m_coneInfo;
    [Space(10f)]
    public int m_coneCount = 3;
    
    // reactor
    [Header("Starting offset, move towards forward/aim direction")]
    public float m_coneStartOffsetInAimDir;
    [Header("Starting offset, move towards left/right")]
    public float m_coneStartOffsetToSides;
    [Header("Starting offset, move towards each cone's direction")]
    public float m_coneStartOffsetInConeDir;
    // rogues
    // public float m_coneStartOffset;
    
    [Header("-- If Fixed Angle")]
    public float m_angleInBetween = 10f;
    [Header("-- If Interpolating Angle")]
    public bool m_changeAngleByCursorDistance = true;
    public float m_targeterMinAngle;
    public float m_targeterMaxAngle = 180f;
    public float m_startAngleOffset;
    [Space(10f)]
    public float m_targeterMinInterpDistance = 0.5f;
    public float m_targeterMaxInterpDistance = 4f;
    [Separator("Sequences")]
    public GameObject m_castSequencePrefab;

    private TargetSelectMod_FanCones m_targetSelMod; // removed in rogues
    private ConeTargetingInfo m_cachedConeInfo; // removed in rogues

    public override string GetUsageForEditor()
    {
        return GetContextUsageStr(
            ContextKeys.s_HitCount.GetName(),
            "on every hit actor, number of cone hits on target"); // laser hits on target in rogues
    }

    public override void ListContextNamesForEditor(List<string> names)
    {
        names.Add(ContextKeys.s_HitCount.GetName());
    }


    // reactor
    public override void Initialize()
    {
        SetCachedFields();
        ConeTargetingInfo coneInfo = GetConeInfo();
        coneInfo.m_affectsAllies = IncludeAllies();
        coneInfo.m_affectsEnemies = IncludeEnemies();
        coneInfo.m_affectsCaster = IncludeCaster();
        coneInfo.m_penetrateLos = IgnoreLos();
    }
    // rogues
    // public override void Initialize()
    // {
    //     m_coneInfo.m_affectsAllies = m_includeAllies;
    //     m_coneInfo.m_affectsEnemies = m_includeEnemies;
    //     m_coneInfo.m_affectsCaster = m_includeCaster;
    //     m_coneInfo.m_penetrateLos = m_ignoreLos;
    // }

    // removed in rogues
    private void SetCachedFields()
    {
        m_cachedConeInfo = m_targetSelMod != null
            ? m_targetSelMod.m_coneInfoMod.GetModifiedValue(m_coneInfo)
            : m_coneInfo;
    }

    // removed in rogues
    public ConeTargetingInfo GetConeInfo()
    {
        return m_cachedConeInfo ?? m_coneInfo;
    }

    // removed in rogues
    public int GetConeCount()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_coneCountMod.GetModifiedValue(m_coneCount)
            : m_coneCount;
    }
    // rogues
    // public int GetNumCones()
    // {
    //     return m_coneCount;
    // }

    // removed in rogues
    public float GetConeStartOffsetInAimDir()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_coneStartOffsetInAimDirMod.GetModifiedValue(m_coneStartOffsetInAimDir)
            : m_coneStartOffsetInAimDir;
    }

    // removed in rogues
    public float GetConeStartOffsetToSides()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_coneStartOffsetToSidesMod.GetModifiedValue(m_coneStartOffsetToSides)
            : m_coneStartOffsetToSides;
    }

    // removed in rogues
    public float GetConeStartOffsetInConeDir()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_coneStartOffsetInConeDirMod.GetModifiedValue(m_coneStartOffsetInConeDir)
            : m_coneStartOffsetInConeDir;
    }

    // removed in rogues
    public float GetAngleInBetween()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_angleInBetweenMod.GetModifiedValue(m_angleInBetween)
            : m_angleInBetween;
    }

    // removed in rogues
    public bool ChangeAngleByCursorDistance()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_changeAngleByCursorDistanceMod.GetModifiedValue(m_changeAngleByCursorDistance)
            : m_changeAngleByCursorDistance;
    }

    // removed in rogues
    public float GetTargeterMinAngle()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_targeterMinAngleMod.GetModifiedValue(m_targeterMinAngle)
            : m_targeterMinAngle;
    }

    // removed in rogues
    public float GetTargeterMaxAngle()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_targeterMaxAngleMod.GetModifiedValue(m_targeterMaxAngle)
            : m_targeterMaxAngle;
    }

    // removed in rogues
    public float GetStartAngleOffset()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_startAngleOffsetMod.GetModifiedValue(m_startAngleOffset)
            : m_startAngleOffset;
    }

    protected virtual bool UseCasterPosForLoS()
    {
        return false;
    }

    protected virtual bool CustomLoS(ActorData actor, ActorData caster)
    {
        return true;
    }

    public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
    {
        AbilityUtil_Targeter_TricksterCones targeter = new AbilityUtil_Targeter_TricksterCones(
            ability,
            GetConeInfo(), // m_coneInfo in rogues
            GetConeCount(), // m_coneCount in rogues
            GetConeCount, // GetNumCones in rogues
            GetConeOrigins,
            GetConeDirections,
            GetFreePosForAim,
            false,
            UseCasterPosForLoS())
        {
            m_customDamageOriginDelegate = GetDamageOriginForTargeter
        };
        return new List<AbilityUtil_Targeter> { targeter };
    }

    private Vector3 GetDamageOriginForTargeter(
        AbilityTarget currentTarget,
        Vector3 defaultOrigin,
        ActorData actorToAdd,
        ActorData caster)
    {
        return caster.GetFreePos();
    }

    public Vector3 GetFreePosForAim(AbilityTarget currentTarget, ActorData caster)
    {
        return currentTarget.FreePos;
    }

    // reactor
    public virtual List<Vector3> GetConeOrigins(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
    {
        List<Vector3> list = new List<Vector3>();
        Vector3 losCheckPos = caster.GetLoSCheckPos();
        Vector3 aimDirection = currentTarget.AimDirection;
        Vector3 normalized = Vector3.Cross(aimDirection, Vector3.up).normalized;
        int coneCount = GetConeCount();
        int halfConeCount = coneCount / 2;
        bool evenCones = coneCount % 2 == 0;
        float aimDirOffset = GetConeStartOffsetInAimDir() * Board.SquareSizeStatic;
        float sideOffsetDir = GetConeStartOffsetToSides() * Board.SquareSizeStatic;
        for (int i = 0; i < coneCount; i++)
        {
            Vector3 b = Vector3.zero;
            if (aimDirOffset != 0f)
            {
                b = aimDirOffset * aimDirection;
            }

            if (sideOffsetDir > 0f)
            {
                if (evenCones)
                {
                    if (i < halfConeCount)
                    {
                        b -= (halfConeCount - i) * sideOffsetDir * normalized;
                    }
                    else
                    {
                        b += (i - halfConeCount + 1) * sideOffsetDir * normalized;
                    }
                }
                else if (i < halfConeCount)
                {
                    b -= (halfConeCount - i) * sideOffsetDir * normalized;
                }
                else if (i > halfConeCount)
                {
                    b += (i - halfConeCount) * sideOffsetDir * normalized;
                }
            }

            list.Add(losCheckPos + b);
        }

        if (GetConeStartOffsetInConeDir() > 0f)
        {
            List<Vector3> coneDirections = GetConeDirections(currentTarget, targeterFreePos, caster);
            float d = GetConeStartOffsetInConeDir() * Board.SquareSizeStatic;
            for (int i = 0; i < coneDirections.Count; i++)
            {
                list[i] += d * coneDirections[i];
            }
        }

        return list;
    }
    // rogues
    // public virtual List<Vector3> GetConeOrigins(AbilityTarget currentTarget, Vector3 targeterFreePos, ActorData caster)
    // {
    //     List<Vector3> list = new List<Vector3>();
    //     Vector3 aimDirection = caster.GetLoSCheckPos();
    //     
    //     if (m_coneStartOffset > 0f)
    //     {
    //         List<Vector3> coneDirections = GetConeDirections(currentTarget, targeterFreePos, caster);
    //         float d = m_coneStartOffset * Board.SquareSizeStatic;
    //         for (int i = 0; i < coneDirections.Count; i++)
    //         {
    //             list.Add(aimDirection + d * coneDirections[i]);
    //         }
    //     }
    //     else
    //     {
    //         for (int j = 0; j < m_coneCount; j++)
    //         {
    //             list.Add(aimDirection);
    //         }
    //     }
    //
    //     return list;
    // }

    public virtual List<Vector3> GetConeDirections(
        AbilityTarget currentTarget,
        Vector3 targeterFreePos,
        ActorData caster)
    {
        List<Vector3> list = new List<Vector3>();
        float angleInBetween = GetAngleInBetween(); // m_angleInBetween in rogues
        int coneCount = GetConeCount(); // m_coneCount in rogues
        if (ChangeAngleByCursorDistance()) // m_changeAngleByCursorDistance in rogues
        {
            float angleTotal = coneCount <= 1
                ? 0f
                : AbilityCommon_FanLaser.CalculateFanAngleDegrees(
                    currentTarget,
                    caster,
                    GetTargeterMinAngle(), // m_targeterMinAngle in rogues
                    GetTargeterMaxAngle(), // m_targeterMaxAngle in rogues
                    m_targeterMinInterpDistance,
                    m_targeterMaxInterpDistance,
                    0f);

            angleInBetween = coneCount > 1
                ? angleTotal / (coneCount - 1)
                : 0f;
        }

        float aimAngle = VectorUtils.HorizontalAngle_Deg(currentTarget.AimDirection) + GetStartAngleOffset(); // + m_startAngleOffset in rogues
        float startAngle = aimAngle - 0.5f * (coneCount - 1) * angleInBetween;
        for (int i = 0; i < coneCount; i++)
        {
            list.Add(VectorUtils.AngleDegreesToVector(startAngle + i * angleInBetween));
        }

        return list;
    }

    // removed in rogues
    protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
    {
        m_targetSelMod = modBase as TargetSelectMod_FanCones;
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
        Dictionary<ActorData, int> hitActorsAndHitCount = GetHitActorsAndHitCount(
            targets,
            caster,
            out _,
            out _,
            out _,
            out _,
            nonActorTargetInfo);
        foreach (ActorData actorData in hitActorsAndHitCount.Keys)
        {
            AddHitActor(actorData, caster.GetLoSCheckPos());
            SetActorContext(actorData, ContextKeys.s_HitCount.GetKey(), hitActorsAndHitCount[actorData]);
        }
    }

    // rogues
    protected Dictionary<ActorData, int> GetHitActorsAndHitCount(
        List<AbilityTarget> targets,
        ActorData caster,
        out List<List<ActorData>> actorsForSequence,
        out List<Vector3> coneEndPosList,
        out List<Vector3> coneStartPosList,
        out int numConesWithHits,
        List<NonActorTargetInfo> nonActorTargetInfo)
    {
        actorsForSequence = new List<List<ActorData>>();
        coneEndPosList = new List<Vector3>();
        numConesWithHits = 0;
        Dictionary<ActorData, int> dictionary = new Dictionary<ActorData, int>();
        List<Vector3> coneDirections = GetConeDirections(targets[0], targets[0].FreePos, caster);
        List<Vector3> coneOrigins = GetConeOrigins(targets[0], targets[0].FreePos, caster);
        coneStartPosList = coneOrigins;
        ConeTargetingInfo coneInfo = GetConeInfo(); // m_coneInfo in rogues
        for (int i = 0; i < coneDirections.Count; i++)
        {
            Vector3 coneDirection = coneDirections[i];
            Vector3 coneOrigin = coneOrigins[i];
            List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(
                coneOrigin,
                VectorUtils.HorizontalAngle_Deg(coneDirection),
                coneInfo.m_widthAngleDeg,
                coneInfo.m_radiusInSquares,
                coneInfo.m_backwardsOffset,
                coneInfo.m_penetrateLos,
                caster,
                TargeterUtils.GetRelevantTeams(caster, coneInfo.m_affectsAllies, coneInfo.m_affectsEnemies),
                nonActorTargetInfo);
            if (coneInfo.m_affectsCaster && i == 0)
            {
                actorsInCone.Add(caster);
            }

            foreach (ActorData actorData in actorsInCone.ToArray())
            {
                if (!CustomLoS(actorData, caster))
                {
                    actorsInCone.Remove(actorData);
                }
            }

            actorsForSequence.Add(actorsInCone);
            coneEndPosList.Add(coneOrigin + coneInfo.m_radiusInSquares * Board.SquareSizeStatic * coneDirection);
            if (actorsInCone.Count > 0)
            {
                numConesWithHits++;
            }

            foreach (ActorData actorData in actorsInCone)
            {
                if (dictionary.ContainsKey(actorData))
                {
                    dictionary[actorData]++;
                }
                else
                {
                    dictionary[actorData] = 1;
                }
            }
        }

        return dictionary;
    }

    // rogues
    public override List<ServerClientUtils.SequenceStartData> CreateSequenceStartData(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData,
        Sequence.IExtraSequenceParams[] extraSequenceParams = null)
    {
        List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
        bool containsSelf = additionalData.m_abilityResults.HitActorList().Contains(caster);
        GetHitActorsAndHitCount(
            targets,
            caster,
            out List<List<ActorData>> actorsForSequence,
            out List<Vector3> coneEndPosList,
            out List<Vector3> coneStartPosList,
            out _,
            null);
        for (int i = 0; i < actorsForSequence.Count; i++)
        {
            List<Sequence.IExtraSequenceParams> sequenceParams = new List<Sequence.IExtraSequenceParams>();
            if (extraSequenceParams != null)
            {
                sequenceParams.AddRange(extraSequenceParams);
            }

            sequenceParams.AddRange(CreateConeSequenceExtraParam(coneStartPosList[i], coneEndPosList[i]));
            list.Add(
                new ServerClientUtils.SequenceStartData(
                    m_castSequencePrefab,
                    coneStartPosList[i],
                    actorsForSequence[i].ToArray(),
                    caster,
                    additionalData.m_sequenceSource,
                    sequenceParams.ToArray()));
        }

        if (containsSelf)
        {
            list.Add(
                new ServerClientUtils.SequenceStartData(
                    SequenceLookup.Get().GetSimpleHitSequencePrefab(),
                    caster.GetFreePos(),
                    caster.AsArray(),
                    caster,
                    additionalData.m_sequenceSource));
        }

        return list;
    }

    // rogues
    public virtual Sequence.IExtraSequenceParams[] CreateConeSequenceExtraParam(
        Vector3 coneStartPos,
        Vector3 coneEndPos)
    {
        ConeTargetingInfo coneInfo = GetConeInfo(); // m_coneInfo in rogues
        return new BlasterStretchConeSequence.ExtraParams
        {
            lengthInSquares = coneInfo.m_radiusInSquares,
            angleInDegrees = coneInfo.m_widthAngleDeg,
            forwardAngle = VectorUtils.HorizontalAngle_Deg(coneEndPos - coneStartPos)
        }.ToArray();
    }
    
    // custom
    public Dictionary<BoardSquare, int> GetHitSquareToHitCount(List<AbilityTarget> targets, ActorData caster, out Vector3 posForHit) 
    {
        Dictionary<BoardSquare, int> result = new Dictionary<BoardSquare, int>();
        List<Vector3> coneDirections = GetConeDirections(targets[0], targets[0].FreePos, caster);
        List<Vector3> coneOrigins = GetConeOrigins(targets[0], targets[0].FreePos, caster);
        posForHit = coneOrigins[0];
        ConeTargetingInfo coneInfo = GetConeInfo();
        for (int i = 0; i < coneDirections.Count; i++)
        {
            List<BoardSquare> squaresInCone = GameWideData.Get().UseActorRadiusForCone()
                ? AreaEffectUtils.GetSquaresInConeByActorRadius(
                    coneOrigins[i],
                    VectorUtils.HorizontalAngle_Deg(coneDirections[i]),
                    coneInfo.m_widthAngleDeg,
                    coneInfo.m_radiusInSquares,
                    coneInfo.m_backwardsOffset,
                    coneInfo.m_penetrateLos,
                    caster)
                : AreaEffectUtils.GetSquaresInCone(
                    coneOrigins[i],
                    VectorUtils.HorizontalAngle_Deg(coneDirections[i]),
                    coneInfo.m_widthAngleDeg,
                    coneInfo.m_radiusInSquares,
                    coneInfo.m_backwardsOffset,
                    coneInfo.m_penetrateLos,
                    caster);
            foreach (BoardSquare hitSquare in squaresInCone)
            {
                if (result.ContainsKey(hitSquare))
                {
                    result[hitSquare]++;
                }
                else
                {
                    result[hitSquare] = 1;
                }
            }
        }

        return result;
    }
#endif
}