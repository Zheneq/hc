// ROGUES
// SERVER
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_Cone : GenericAbility_TargetSelectBase
{
    [Separator("Input Params")]
    public ConeTargetingInfo m_coneInfo;
    [Separator("Sequences")]
    public GameObject m_coneSequencePrefab;

    private TargetSelectMod_Cone m_targetSelMod;
    private ConeTargetingInfo m_cachedConeInfo;

    public override string GetUsageForEditor()
    {
        return GetContextUsageStr(
            ContextKeys.s_DistFromStart.GetName(),
            "distance from start of cone position, in squares");
    }

    public override void ListContextNamesForEditor(List<string> names)
    {
        names.Add(ContextKeys.s_DistFromStart.GetName());
    }

    public override void Initialize()
    {
        SetCachedFields();
        ConeTargetingInfo coneInfo = GetConeInfo();
        coneInfo.m_affectsAllies = IncludeAllies();
        coneInfo.m_affectsEnemies = IncludeEnemies();
        coneInfo.m_affectsCaster = IncludeCaster();
        coneInfo.m_penetrateLos = IgnoreLos();
    }

    public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
    {
        ConeTargetingInfo coneInfo = GetConeInfo();
        return new List<AbilityUtil_Targeter>
        {
            new AbilityUtil_Targeter_DirectionCone(
                ability,
                coneInfo.m_widthAngleDeg,
                coneInfo.m_radiusInSquares,
                coneInfo.m_backwardsOffset,
                coneInfo.m_penetrateLos,
                true,
                coneInfo.m_affectsEnemies,
                coneInfo.m_affectsAllies,
                coneInfo.m_affectsCaster)
        };
    }

    private void SetCachedFields()
    {
        m_cachedConeInfo = m_targetSelMod != null
            ? m_targetSelMod.m_coneInfoMod.GetModifiedValue(m_coneInfo)
            : m_coneInfo;
    }

    public ConeTargetingInfo GetConeInfo()
    {
        return m_cachedConeInfo ?? m_coneInfo;
    }

    protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
    {
        m_targetSelMod = modBase as TargetSelectMod_Cone;
    }

    protected override void OnTargetSelModRemoved()
    {
        m_targetSelMod = null;
    }
    
#if SERVER
    // rogues
    public override List<ServerClientUtils.SequenceStartData> CreateSequenceStartData(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData,
        Sequence.IExtraSequenceParams[] extraSequenceParams = null)
    {
        ConeTargetingInfo coneInfo = GetConeInfo();
        List<Sequence.IExtraSequenceParams> extraParams = new List<Sequence.IExtraSequenceParams>();
        if (extraSequenceParams != null)
        {
            extraParams.AddRange(extraSequenceParams);
        }

        extraParams.Add(
            new BlasterStretchConeSequence.ExtraParams
            {
                angleInDegrees = coneInfo.m_widthAngleDeg,
                forwardAngle = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection),
                lengthInSquares = coneInfo.m_radiusInSquares
            });
        return new List<ServerClientUtils.SequenceStartData>
        {
            new ServerClientUtils.SequenceStartData(
                m_coneSequencePrefab,
                caster.GetCurrentBoardSquare(),
                additionalData.m_abilityResults.HitActorsArray(),
                caster,
                additionalData.m_sequenceSource,
                extraParams.ToArray())
        };
    }

    // rogues
    public override void CalcHitTargets(
        List<AbilityTarget> targets,
        ActorData caster,
        List<NonActorTargetInfo> nonActorTargetInfo)
    {
        ResetContextData();
        base.CalcHitTargets(targets, caster, nonActorTargetInfo);
        List<ActorData> coneHitActors = GetConeHitActors(targets, caster, nonActorTargetInfo);
        Vector3 loSCheckPos = caster.GetLoSCheckPos();
        foreach (ActorData hitActor in coneHitActors)
        {
            AddHitActor(hitActor, loSCheckPos);
            Vector3 vector = hitActor.GetLoSCheckPos() - loSCheckPos;
            vector.y = 0f;
            float distFromStartInSquares = vector.magnitude / Board.SquareSizeStatic;
            SetActorContext(hitActor, ContextKeys.s_DistFromStart.GetKey(), distFromStartInSquares);
        }
    }

    // rogues
    private List<ActorData> GetConeHitActors(
        List<AbilityTarget> targets,
        ActorData caster,
        List<NonActorTargetInfo> nonActorTargetInfo)
    {
        ConeTargetingInfo coneInfo = GetConeInfo();
        float aimAngle = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection);
        Vector3 loSCheckPos = caster.GetLoSCheckPos();
        List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(
            loSCheckPos,
            aimAngle,
            coneInfo.m_widthAngleDeg,
            coneInfo.m_radiusInSquares,
            coneInfo.m_backwardsOffset,
            coneInfo.m_penetrateLos,
            caster,
            TargeterUtils.GetRelevantTeams(caster, coneInfo.m_affectsAllies, coneInfo.m_affectsEnemies),
            nonActorTargetInfo);
        if (coneInfo.m_affectsCaster && !actorsInCone.Contains(caster))
        {
            actorsInCone.Add(caster);
        }

        TargeterUtils.SortActorsByDistanceToPos(ref actorsInCone, loSCheckPos);
        return actorsInCone;
    }
#endif
}