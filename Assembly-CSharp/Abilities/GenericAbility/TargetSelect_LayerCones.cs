// ROGUES
// SERVER
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_LayerCones : GenericAbility_TargetSelectBase
{
    public delegate int NumActiveLayerDelegate(int maxLayers);

    [Separator("Targeting Properties")]
    public float m_coneWidthAngle = 90f;
    // public float m_backwardsDistanceOffset; // rogues
    public List<float> m_coneRadiusList;
    [Separator("Sequences")]
    public GameObject m_coneSequencePrefab;
    public NumActiveLayerDelegate m_delegateNumActiveLayers;

    private TargetSelectMod_LayerCones m_targetSelMod;
    private List<float> m_cachedRadiusList = new List<float>();

    public override string GetUsageForEditor()
    {
        return GetContextUsageStr(
                   ContextKeys.s_Layer.GetName(),
                   "on every hit actor, 0-based index of smallest cone with a hit, with smallest cone first")
               + GetContextUsageStr(
                   ContextKeys.s_LayersActive.GetName(),
                   "Non-actor specific context, number of layers active",
                   false);
    }

    public override void ListContextNamesForEditor(List<string> names)
    {
        names.Add(ContextKeys.s_Layer.GetName());
        names.Add(ContextKeys.s_LayersActive.GetName());
    }

    public override void Initialize()
    {
        base.Initialize();
        m_cachedRadiusList = m_targetSelMod != null && m_targetSelMod.m_useConeRadiusOverrides
            ? new List<float>(m_targetSelMod.m_coneRadiusOverrides)
            : new List<float>(m_coneRadiusList);
        m_cachedRadiusList.Sort();
    }

    public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
    {
        AbilityUtil_Targeter_LayerCones targeter = new AbilityUtil_Targeter_LayerCones(
            ability,
            GetConeWidthAngle(),
            m_cachedRadiusList,
            0f, // m_backwardsDistanceOffset in rogues
            IgnoreLos());
        targeter.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeCaster());
        return new List<AbilityUtil_Targeter> { targeter };
    }

    public float GetConeWidthAngle()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_coneWidthAngleMod.GetModifiedValue(m_coneWidthAngle)
            : m_coneWidthAngle;
    }

    public float GetMaxConeRadius()
    {
        float result = 0f;
        int numActiveLayers = GetNumActiveLayers();
        if (numActiveLayers > 0)
        {
            result = m_cachedRadiusList[numActiveLayers - 1];
        }

        return result;
    }

    public int GetNumActiveLayers()
    {
        return m_delegateNumActiveLayers?.Invoke(m_cachedRadiusList.Count) ?? m_cachedRadiusList.Count;
    }

    public int GetLayerCount()
    {
        return m_cachedRadiusList.Count;
    }

    protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
    {
        m_targetSelMod = modBase as TargetSelectMod_LayerCones;
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
        List<Sequence.IExtraSequenceParams> sequenceExtraParams = new List<Sequence.IExtraSequenceParams>();
        if (extraSequenceParams != null)
        {
            sequenceExtraParams.AddRange(extraSequenceParams);
        }

        BlasterStretchConeSequence.ExtraParams extraParams = new BlasterStretchConeSequence.ExtraParams
        {
            angleInDegrees = GetConeWidthAngle(),
            forwardAngle = VectorUtils.HorizontalAngle_Deg(targets[0].AimDirection),
            lengthInSquares = GetMaxConeRadius()
        };
        
        // rogues, offset is 0 in reactor
        // if (!Mathf.Approximately(m_backwardsDistanceOffset, 0f))
        // {
        //     extraParams.useStartPosOverride = true;
        //     extraParams.startPosOverride =
        //         targets[0].FreePos - targets[0].AimDirection.normalized * m_backwardsDistanceOffset;
        // }

        sequenceExtraParams.Add(extraParams);
        return new List<ServerClientUtils.SequenceStartData>
        {
            new ServerClientUtils.SequenceStartData(
                m_coneSequencePrefab,
                caster.GetCurrentBoardSquare(),
                additionalData.m_abilityResults.HitActorsArray(),
                caster,
                additionalData.m_sequenceSource,
                sequenceExtraParams.ToArray())
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
        Vector3 vector = caster.GetLoSCheckPos();
        Vector3 aimDirection = targets[0].AimDirection;
        
        // rogues, offset is 0 in reactor
        // if (!Mathf.Approximately(m_backwardsDistanceOffset, 0f))
        // {
        //     vector = caster.GetLoSCheckPos() - aimDirection.normalized * m_backwardsDistanceOffset;
        // }

        float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(aimDirection);
        int numActiveLayers = GetNumActiveLayers();
        GetNonActorSpecificContext().SetValue(ContextKeys.s_LayersActive.GetKey(), numActiveLayers);
        foreach (ActorData actorData in GetConeHitActors(targets, caster, nonActorTargetInfo))
        {
            AddHitActor(actorData, vector);
            for (int i = 0; i < m_cachedRadiusList.Count && i < numActiveLayers; i++)
            {
                if (AreaEffectUtils.IsSquareInConeByActorRadius(
                        actorData.GetCurrentBoardSquare(),
                        vector,
                        coneCenterAngleDegrees,
                        GetConeWidthAngle(),
                        m_cachedRadiusList[i],
                        0f,
                        IgnoreLos(),
                        caster))
                {
                    SetActorContext(actorData, ContextKeys.s_Layer.GetKey(), i);
                    break;
                }
            }
        }
    }

    // rogues
    private List<ActorData> GetConeHitActors(
        List<AbilityTarget> targets,
        ActorData caster,
        List<NonActorTargetInfo> nonActorTargetInfo)
    {
        Vector3 aimDirection = targets[0].AimDirection;
        float coneCenterAngleDegrees = VectorUtils.HorizontalAngle_Deg(aimDirection);
        Vector3 vector = caster.GetLoSCheckPos();
        
        // rogues, offset is 0 in reactor
        // if (!Mathf.Approximately(m_backwardsDistanceOffset, 0f))
        // {
        //     vector = caster.GetLoSCheckPos() - aimDirection.normalized * m_backwardsDistanceOffset;
        // }

        List<ActorData> actorsInCone = AreaEffectUtils.GetActorsInCone(
            vector,
            coneCenterAngleDegrees,
            GetConeWidthAngle(),
            GetMaxConeRadius(),
            0f,
            IgnoreLos(),
            caster,
            TargeterUtils.GetRelevantTeams(caster, IncludeAllies(), IncludeEnemies()),
            nonActorTargetInfo);
        if (IncludeCaster() && !actorsInCone.Contains(caster))
        {
            actorsInCone.Add(caster);
        }

        TargeterUtils.SortActorsByDistanceToPos(ref actorsInCone, vector);
        return actorsInCone;
    }
#endif
}