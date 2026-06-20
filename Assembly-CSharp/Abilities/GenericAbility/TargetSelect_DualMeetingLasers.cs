// ROGUES
// SERVER
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_DualMeetingLasers : GenericAbility_TargetSelectBase
{
    public delegate int LaserCountDelegate(AbilityTarget currentTarget, ActorData caster);

    public delegate float ExtraAoeRadiusDelegate(
        AbilityTarget currentTarget,
        ActorData targetingActor,
        float baseRadius);

    [Separator("Targeting - Laser")]
    public float m_laserWidth = 0.5f;
    public float m_minMeetingDistFromCaster = 1f;
    public float m_maxMeetingDistFromCaster = 8f;
    public float m_laserStartForwardOffset;
    public float m_laserStartSideOffset = 0.5f;
    [Separator("Targeting - AoE")]
    public float m_aoeBaseRadius = 2.5f;
    public float m_aoeMinRadius;
    public float m_aoeMaxRadius = -1f;
    public float m_aoeRadiusChangePerUnitFromMin = 0.1f;
    [Header("-- Multiplier on radius if not all lasers meet")]
    public float m_radiusMultIfPartialBlock = 1f;
    [Space(10f)]
    public bool m_aoeIgnoreMinCoverDist = true;
    [Separator("Sequences")]
    public GameObject m_laserSequencePrefab;
    [Header("-- Use if laser doesn't have impact FX that spawns on end of laser, or for temp testing")]
    public GameObject m_aoeSequencePrefab;
    public LaserCountDelegate m_delegateLaserCount;
    public ExtraAoeRadiusDelegate m_delegateExtraAoeRadius;

    private TargetSelectMod_DualMeetingLasers m_targetSelMod;

    public override string GetUsageForEditor()
    {
        return GetContextUsageStr(
                   ContextKeys.s_InAoe.GetName(),
                   "on every hit actor, 1 if in AoE, 0 otherwise")
               + GetContextUsageStr(
                   ContextKeys.s_DistFromMin.GetName(),
                   "on every actor, distance of cursor pos from min distance, for interpolation");
    }

    public override void ListContextNamesForEditor(List<string> names)
    {
        names.Add(ContextKeys.s_InAoe.GetName());
        names.Add(ContextKeys.s_DistFromMin.GetName());
    }

    public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
    {
        AbilityUtil_Targeter_ScampDualLasers targeter = new AbilityUtil_Targeter_ScampDualLasers(
            ability,
            GetLaserWidth(),
            GetMinMeetingDistFromCaster(),
            GetMaxMeetingDistFromCaster(),
            GetLaserStartForwardOffset(),
            GetLaserStartSideOffset(),
            GetAoeBaseRadius(),
            GetAoeMinRadius(),
            GetAoeMaxRadius(),
            GetAoeRadiusChangePerUnitFromMin(),
            GetRadiusMultIfPartialBlock(),
            AoeIgnoreMinCoverDist());
        targeter.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeCaster());
        return new List<AbilityUtil_Targeter> { targeter };
    }

    public override bool CanShowTargeterRangePreview(TargetData[] targetData)
    {
        return true;
    }

    public override float GetTargeterRangePreviewRadius(Ability ability, ActorData caster)
    {
        return GetMaxMeetingDistFromCaster() + GetAoeMinRadius();
    }

    protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
    {
        m_targetSelMod = modBase as TargetSelectMod_DualMeetingLasers;
    }

    protected override void OnTargetSelModRemoved()
    {
        m_targetSelMod = null;
    }

    public float GetLaserWidth()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_laserWidthMod.GetModifiedValue(m_laserWidth)
            : m_laserWidth;
    }

    public float GetMinMeetingDistFromCaster()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_minMeetingDistFromCasterMod.GetModifiedValue(m_minMeetingDistFromCaster)
            : m_minMeetingDistFromCaster;
    }

    public float GetMaxMeetingDistFromCaster()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_maxMeetingDistFromCasterMod.GetModifiedValue(m_maxMeetingDistFromCaster)
            : m_maxMeetingDistFromCaster;
    }

    public float GetLaserStartForwardOffset()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_laserStartForwardOffsetMod.GetModifiedValue(m_laserStartForwardOffset)
            : m_laserStartForwardOffset;
    }

    public float GetLaserStartSideOffset()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_laserStartSideOffsetMod.GetModifiedValue(m_laserStartSideOffset)
            : m_laserStartSideOffset;
    }

    public float GetAoeBaseRadius()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_aoeBaseRadiusMod.GetModifiedValue(m_aoeBaseRadius)
            : m_aoeBaseRadius;
    }

    public float GetAoeMinRadius()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_aoeMinRadiusMod.GetModifiedValue(m_aoeMinRadius)
            : m_aoeMinRadius;
    }

    public float GetAoeMaxRadius()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_aoeMaxRadiusMod.GetModifiedValue(m_aoeMaxRadius)
            : m_aoeMaxRadius;
    }

    public float GetAoeRadiusChangePerUnitFromMin()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_aoeRadiusChangePerUnitFromMinMod.GetModifiedValue(m_aoeRadiusChangePerUnitFromMin)
            : m_aoeRadiusChangePerUnitFromMin;
    }

    public float GetRadiusMultIfPartialBlock()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_radiusMultIfPartialBlockMod.GetModifiedValue(m_radiusMultIfPartialBlock)
            : m_radiusMultIfPartialBlock;
    }

    public bool AoeIgnoreMinCoverDist()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_aoeIgnoreMinCoverDistMod.GetModifiedValue(m_aoeIgnoreMinCoverDist)
            : m_aoeIgnoreMinCoverDist;
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
        GetHitActors(
            targets,
            caster,
            nonActorTargetInfo,
            out List<List<ActorData>> laserHitActors,
            out List<Vector3> laserStartPosList,
            out List<Vector3> laserEndPosList,
            out Vector3 aimAtPos,
            out int aoeEndPosIndex,
            out _,
            out List<ActorData> aoeHitActors);
        float value = AbilityCommon_DualMeetingLasers.CalcMeetingPosDistFromMin(
            caster.GetLoSCheckPos(),
            aimAtPos,
            GetMinMeetingDistFromCaster());
        List<ActorData> processedHits = new List<ActorData>();
        if (aoeEndPosIndex >= 0)
        {
            foreach (ActorData actor in aoeHitActors)
            {
                AddHitActor(actor, laserEndPosList[aoeEndPosIndex], AoeIgnoreMinCoverDist());
                SetActorContext(actor, ContextKeys.s_InAoe.GetKey(), 1);
                SetActorContext(actor, ContextKeys.s_DistFromMin.GetKey(), value);
                processedHits.Add(actor);
            }
        }

        for (int i = 0; i < laserHitActors.Count; i++)
        {
            foreach (ActorData actor in laserHitActors[i])
            {
                if (processedHits.Contains(actor))
                {
                    continue;
                }

                AddHitActor(actor, laserStartPosList[i]);
                SetActorContext(actor, ContextKeys.s_InAoe.GetKey(), 0);
                SetActorContext(actor, ContextKeys.s_DistFromMin.GetKey(), value);
                processedHits.Add(actor);
            }
        }
    }

    // rogues
    private void GetHitActors(
        List<AbilityTarget> targets,
        ActorData caster,
        List<NonActorTargetInfo> nonActorTargetInfo,
        out List<List<ActorData>> laserHitActors,
        out List<Vector3> laserStartPosList,
        out List<Vector3> laserEndPosList,
        out Vector3 aimAtPos,
        out int aoeEndPosIndex,
        out float aoeRadius,
        out List<ActorData> aoeHitActors)
    {
        Vector3 loSCheckPos = caster.GetLoSCheckPos();
        AbilityTarget abilityTarget = targets[0];
        int laserCount = m_delegateLaserCount?.Invoke(targets[0], caster) ?? 2;
        if (laserCount > 1)
        {
            laserStartPosList = AbilityCommon_DualMeetingLasers.CalcStartingPositions(
                loSCheckPos,
                abilityTarget.FreePos,
                GetLaserStartForwardOffset(),
                GetLaserStartSideOffset());
        }
        else
        {
            laserStartPosList = new List<Vector3> { loSCheckPos };
        }

        aimAtPos = AbilityCommon_DualMeetingLasers.CalcClampedMeetingPos(
            loSCheckPos,
            abilityTarget.FreePos,
            GetMinMeetingDistFromCaster(),
            GetMaxMeetingDistFromCaster());
        float baseAoeRadius = AbilityCommon_DualMeetingLasers.CalcAoeRadius(
            loSCheckPos,
            aimAtPos,
            GetAoeBaseRadius(),
            GetMinMeetingDistFromCaster(),
            GetAoeRadiusChangePerUnitFromMin(),
            GetAoeMinRadius(),
            GetAoeMaxRadius());
        if (m_delegateExtraAoeRadius != null)
        {
            baseAoeRadius += m_delegateExtraAoeRadius(abilityTarget, caster, GetAoeBaseRadius());
        }

        aoeRadius = baseAoeRadius;
        AbilityCommon_DualMeetingLasers.CalcHitActors(
            aimAtPos,
            laserStartPosList,
            GetLaserWidth(),
            baseAoeRadius,
            GetRadiusMultIfPartialBlock(),
            caster,
            TargeterUtils.GetRelevantTeams(caster, IncludeAllies(), IncludeEnemies()),
            true,
            nonActorTargetInfo,
            out laserHitActors,
            out laserEndPosList,
            out aoeEndPosIndex,
            out aoeRadius,
            out aoeHitActors);
    }

    // rogues
    public override List<ServerClientUtils.SequenceStartData> CreateSequenceStartData(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData,
        Sequence.IExtraSequenceParams[] extraSequenceParams = null)
    {
        List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
        List<NonActorTargetInfo> nonActorTargetInfo = new List<NonActorTargetInfo>();
        GetHitActors(
            targets,
            caster,
            nonActorTargetInfo,
            out List<List<ActorData>> laserHitActors,
            out List<Vector3> laserStartPosList,
            out List<Vector3> laserEndPosList,
            out _,
            out int aoeEndPosIndex,
            out float radiusInSquares,
            out List<ActorData> aoeHitActors);
        for (int i = 0; i < laserStartPosList.Count; i++)
        {
            List<Sequence.IExtraSequenceParams> sequenceParams = new List<Sequence.IExtraSequenceParams>();
            if (extraSequenceParams != null)
            {
                sequenceParams.AddRange(extraSequenceParams);
            }

            sequenceParams.Add(
                new SplineProjectileSequence.DelayedProjectileExtraParams
                {
                    skipImpactFx = i != aoeEndPosIndex,
                    useOverrideStartPos = true,
                    overrideStartPos = laserStartPosList[i]
                });
            if (i == 1)
            {
                sequenceParams.Add(
                    new Sequence.GenericIntParam
                    {
                        m_fieldIdentifier = Sequence.GenericIntParam.FieldIdentifier.Index,
                        m_value = 1
                    });
            }

            if (i == aoeEndPosIndex)
            {
                sequenceParams.Add(
                    new Sequence.FxAttributeParam
                    {
                        m_paramNameCode = Sequence.FxAttributeParam.ParamNameCode.ScaleControl,
                        m_paramTarget = Sequence.FxAttributeParam.ParamTarget.ImpactVfx,
                        m_paramValue = 2f * radiusInSquares
                    });
                if (m_aoeSequencePrefab == null)
                {
                    laserHitActors[i].AddRange(aoeHitActors);
                }
            }

            list.Add(
                new ServerClientUtils.SequenceStartData(
                    m_laserSequencePrefab,
                    laserEndPosList[i],
                    laserHitActors[i].ToArray(),
                    caster,
                    additionalData.m_sequenceSource,
                    sequenceParams.ToArray()));
        }

        if (aoeEndPosIndex >= 0 && m_aoeSequencePrefab != null)
        {
            List<Sequence.IExtraSequenceParams> sequenceParams = new List<Sequence.IExtraSequenceParams>();
            if (extraSequenceParams != null)
            {
                sequenceParams.AddRange(extraSequenceParams);
            }

            sequenceParams.AddRange(AbilityCommon_LayeredRings.GetAdjustableRingSequenceParams(radiusInSquares));
            list.Add(
                new ServerClientUtils.SequenceStartData(
                    m_aoeSequencePrefab,
                    laserEndPosList[aoeEndPosIndex],
                    aoeHitActors.ToArray(),
                    caster,
                    additionalData.m_sequenceSource,
                    sequenceParams.ToArray()));
        }

        return list;
    }
#endif
}