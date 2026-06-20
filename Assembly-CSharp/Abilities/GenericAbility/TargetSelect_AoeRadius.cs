// ROGUES
// SERVER
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_AoeRadius : GenericAbility_TargetSelectBase
{
    [Separator("Targeting Properties")]
    public float m_radius = 1f;
    [Space(10f)]
    public bool m_useSquareCenterPos;
    [Separator("Sequences")]
    public GameObject m_castSequencePrefab;

    private TargetSelectMod_AoeRadius m_targetSelMod;

    public override string GetUsageForEditor()
    {
        return GetContextUsageStr(
            ContextKeys.s_DistFromStart.GetName(),
            "on every hit actor, distance from center of AoE, in squares");
    }

    public override void ListContextNamesForEditor(List<string> names)
    {
        names.Add(ContextKeys.s_DistFromStart.GetName());
    }

    public override void Initialize()
    {
        m_commonProperties.SetValue(ContextKeys.s_Radius.GetKey(), GetRadius());
    }

    public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
    {
        AbilityUtil_Targeter_AoE_Smooth targeter =
            new AbilityUtil_Targeter_AoE_Smooth(ability, GetRadius(), IgnoreLos());
        targeter.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeCaster());
        targeter.m_customCenterPosDelegate = GetCenterPos;
        targeter.SetShowArcToShape(ability.GetTargetData().Length != 0);
        return new List<AbilityUtil_Targeter> { targeter };
    }

    public Vector3 GetCenterPos(ActorData caster, AbilityTarget currentTarget)
    {
        if (UseSquareCenterPos())
        {
            BoardSquare square = Board.Get().GetSquare(currentTarget.GridPos);
            if (square != null)
            {
                return square.ToVector3();
            }
        }

        return currentTarget.FreePos;
    }

    public float GetRadius()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_radiusMod.GetModifiedValue(m_radius)
            : m_radius;
    }

    public bool UseSquareCenterPos()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_useSquareCenterPosMod.GetModifiedValue(m_useSquareCenterPos)
            : m_useSquareCenterPos;
    }

    public override bool CanShowTargeterRangePreview(TargetData[] targetData)
    {
        return true;
    }

    public override float GetTargeterRangePreviewRadius(Ability ability, ActorData caster)
    {
        return GetRadius();
    }

    protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
    {
        m_targetSelMod = modBase as TargetSelectMod_AoeRadius;
    }

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
        m_contextCalcData.m_nonActorSpecificContext.SetValue(ContextKeys.s_Radius.GetKey(), GetRadius());
        Vector3 centerPos = GetCenterPos(caster, targets[0]);
        foreach (ActorData actorData in GetHitActors(targets, caster, nonActorTargetInfo))
        {
            AddHitActor(actorData, caster.GetLoSCheckPos());
            float value = VectorUtils.HorizontalPlaneDistInSquares(centerPos, actorData.GetFreePos());
            SetActorContext(actorData, ContextKeys.s_DistFromStart.GetKey(), value);
        }
    }

    // rogues
    public List<ActorData> GetHitActors(
        List<AbilityTarget> targets,
        ActorData caster,
        List<NonActorTargetInfo> nonActorTargetInfo)
    {
        List<ActorData> actorsInRadius = AreaEffectUtils.GetActorsInRadius(
            GetCenterPos(caster, targets[0]),
            GetRadius(),
            IgnoreLos(),
            caster,
            TargeterUtils.GetRelevantTeams(caster, IncludeAllies(), IncludeEnemies()),
            nonActorTargetInfo);
        if (IncludeCaster() && !actorsInRadius.Contains(caster))
        {
            actorsInRadius.Add(caster);
        }

        return actorsInRadius;
    }

    // rogues
    public override List<ServerClientUtils.SequenceStartData> CreateSequenceStartData(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData,
        Sequence.IExtraSequenceParams[] extraSequenceParams = null)
    {
        List<Sequence.IExtraSequenceParams> sequenceParams = new List<Sequence.IExtraSequenceParams>();
        if (extraSequenceParams != null)
        {
            sequenceParams.AddRange(extraSequenceParams);
        }

        sequenceParams.AddRange(AbilityCommon_LayeredRings.GetAdjustableRingSequenceParams(GetRadius()));
        return new List<ServerClientUtils.SequenceStartData>
        {
            new ServerClientUtils.SequenceStartData(
                m_castSequencePrefab,
                GetCenterPos(caster, targets[0]),
                additionalData.m_abilityResults.HitActorsArray(),
                caster,
                additionalData.m_sequenceSource,
                sequenceParams.ToArray())
        };
    }
#endif
}