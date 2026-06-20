// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using AbilityContextNamespace;
using UnityEngine;

public class TargetSelect_Laser : GenericAbility_TargetSelectBase
{
    [Separator("Targeting Properties")]
    public float m_laserRange = 5f;
    public float m_laserWidth = 1f;
    public int m_maxTargets;
    [Separator("AoE around start")]
    public float m_aoeRadiusAroundStart; // TODO GENERICABILITY removed in rogues
    [Separator("Sequences")]
    public GameObject m_castSequencePrefab;
    public GameObject m_aoeAtStartSequencePrefab; // TODO GENERICABILITY removed in rogues

    private TargetSelectMod_Laser m_targetSelMod; // removed in rogues

    // private static readonly string c_hitOrder = ContextKeys.s_HitOrder.GetName(); // cached in rogues
    
    public override string GetUsageForEditor()
    {
        // reactor
        return GetContextUsageStr(
                   ContextKeys.s_HitOrder.GetName(),
                   "on every non-caster hit actor, order in which they are hit in laser")
               + GetContextUsageStr(
                   ContextKeys.s_DistFromStart.GetName(),
                   "on every non-caster hit actor, distance from caster");
        // rogues
        // return GetContextUsageStr(
        //     ContextKeys.s_HitOrder.GetName(),
        //     "on every enemy, order in which they are hit.");
    }

    public override void ListContextNamesForEditor(List<string> names)
    {
        names.Add(ContextKeys.s_HitOrder.GetName());
        names.Add(ContextKeys.s_DistFromStart.GetName()); // removed in rogues
    }

    // reactor
    public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
    {
        AbilityUtil_Targeter targeter;
        if (GetAoeRadiusAroundStart() <= 0f)
        {
            targeter = new AbilityUtil_Targeter_Laser(
                ability,
                GetLaserWidth(),
                GetLaserRange(),
                IgnoreLos(),
                GetMaxTargets(),
                IncludeAllies(),
                IncludeCaster());
        }
        else
        {
            targeter = new AbilityUtil_Targeter_ClaymoreSlam(
                ability,
                GetLaserRange(),
                GetLaserWidth(),
                GetMaxTargets(),
                360f,
                GetAoeRadiusAroundStart(),
                0f,
                IgnoreLos());
        }
        targeter.SetAffectedGroups(IncludeEnemies(), IncludeAllies(), IncludeCaster());
        return new List<AbilityUtil_Targeter> { targeter };
    }
    // rogues
    // public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
    // {
    //     return new List<AbilityUtil_Targeter>
    //     {
    //         new AbilityUtil_Targeter_Laser(
    //             ability,
    //             m_laserWidth,
    //             m_laserRange,
    //             m_ignoreLos,
    //             m_maxTargets,
    //             m_includeAllies,
    //             m_includeCaster)
    //     };
    // }


    // removed in rogues
    public float GetLaserRange()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_laserRangeMod.GetModifiedValue(m_laserRange)
            : m_laserRange;
    }

    // removed in rogues
    public float GetLaserWidth()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_laserWidthMod.GetModifiedValue(m_laserWidth)
            : m_laserWidth;
    }

    // removed in rogues
    public int GetMaxTargets()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
            : m_maxTargets;
    }

    // removed in rogues
    public float GetAoeRadiusAroundStart()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_aoeRadiusAroundStartMod.GetModifiedValue(m_aoeRadiusAroundStart)
            : m_aoeRadiusAroundStart;
    }

    // removed in rogues
    public override bool CanShowTargeterRangePreview(TargetData[] targetData)
    {
        return true;
    }

    // removed in rogues
    public override float GetTargeterRangePreviewRadius(Ability ability, ActorData caster)
    {
        return GetLaserRange();
    }

    // removed in rogues
    protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
    {
        m_targetSelMod = modBase as TargetSelectMod_Laser;
    }

    // removed in rogues
    protected override void OnTargetSelModRemoved()
    {
        m_targetSelMod = null;
    }
    
#if SERVER
    //rogues
    private void GetHitActors(
        List<AbilityTarget> targets,
        ActorData caster,
        out List<ActorData> actorsForSequence,
        out List<Vector3> targetPosForSequences,
        List<NonActorTargetInfo> nonActorTargetInfo,
        out Vector3 endPos)
    {
        actorsForSequence = new List<ActorData>();
        targetPosForSequences = new List<Vector3>();
        actorsForSequence = AreaEffectUtils.GetActorsInLaser(
            caster.GetLoSCheckPos(),
            targets[0].AimDirection,
            GetLaserRange(), // m_laserRange in rogues
            GetLaserWidth(), // m_laserWidth in rogues
            caster,
            TargeterUtils.GetRelevantTeams(caster, IncludeAllies(), IncludeEnemies()), //  m_includeAllies, m_includeEnemies in rogues
            IgnoreLos(), // m_ignoreLos in rogues
            GetMaxTargets(), // m_maxTargets in rogues
            false,
            true,
            out endPos,
            nonActorTargetInfo);
        targetPosForSequences.Add(endPos);
        if (actorsForSequence.Any())
        {
            Vector3 knockbackOriginFromLaser = AreaEffectUtils.GetKnockbackOriginFromLaser(
                actorsForSequence,
                caster,
                targets[0].AimDirection,
                endPos);
            GetNonActorSpecificContext().SetValue(ContextKeys.s_KnockbackOrigin.GetKey(), knockbackOriginFromLaser);
        }

        if (IncludeCaster() && !actorsForSequence.Contains(caster))
        {
            actorsForSequence.Add(caster);
        }
    }

    //rogues
    public override void CalcHitTargets(
        List<AbilityTarget> targets,
        ActorData caster,
        List<NonActorTargetInfo> nonActorTargetInfo)
    {
        ResetContextData();
        base.CalcHitTargets(targets, caster, nonActorTargetInfo);
        GetHitActors(targets, caster, out List<ActorData> actorsForSequence, out _, nonActorTargetInfo, out _);
        for (int i = 0; i < actorsForSequence.Count; i++)
        {
            ActorData actor = actorsForSequence[i];
            AddHitActor(actor, caster.GetLoSCheckPos());
            SetActorContext(actor, ContextKeys.s_HitOrder.GetKey(), i);
        }
    }

    //rogues
    public override List<ServerClientUtils.SequenceStartData> CreateSequenceStartData(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData,
        Sequence.IExtraSequenceParams[] extraSequenceParams = null)
    {
        List<ServerClientUtils.SequenceStartData> list = new List<ServerClientUtils.SequenceStartData>();
        GetHitActors(targets, caster, out List<ActorData> actorsForSequence, out _, null, out Vector3 endPos);
        
        // rogues
        // List<Sequence.IExtraSequenceParams> extraParamsList = new List<Sequence.IExtraSequenceParams>();
        // ExtraParams extraParams = new ExtraParams();
        // extraParams.endPos = endPos;
        // if (extraParams != null)
        // {
        //     extraParamsList.Add(extraParams);
        // }
        
        list.Add(new ServerClientUtils.SequenceStartData(
            m_castSequencePrefab,
            Board.Get().GetSquareFromVec3(endPos), // Board.Get().GetSquare(targets[0].GridPos) in rogues
            actorsForSequence.ToArray(),
            caster,
            additionalData.m_sequenceSource,
            extraSequenceParams)); // extraParamsList in rogues
        return list;
    }

    // rogues
    // public class ExtraParams : Sequence.IExtraSequenceParams
    // {
    //     public Vector3 endPos;
    //     
    //     public override void XSP_SerializeToStream(NetworkWriter writer)
    //     {
    //         writer.Write(endPos);
    //     }
    //
    //     public override void XSP_DeserializeFromStream(NetworkReader reader)
    //     {
    //         endPos = reader.ReadVector3();
    //     }
    // }
#endif
}