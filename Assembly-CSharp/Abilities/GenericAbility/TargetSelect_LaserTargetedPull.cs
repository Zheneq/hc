// ROGUES
// SERVER
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;
using UnityEngine.Networking;

public class TargetSelect_LaserTargetedPull : GenericAbility_TargetSelectBase
{
    [Separator("Targeting Properties")]
    public float m_laserRange = 5f;
    public float m_laserWidth = 1f;
    public int m_maxTargets = 1;
    public float m_maxKnockbackDist = 50f;
    [Separator("For Pull Destination")]
    public bool m_casterSquareValidForKnockback = true; // removed in rogues
    public float m_squareRangeFromCaster = 3f;
    public float m_destinationAngleDegWithBack = 360f;
    public bool m_destRequireLosFromCaster = true;
    [Separator("Sequences")]
    public GameObject m_castSequencePrefab;

    private TargetSelectMod_LaserTargetedPull m_targetSelMod;

    public override string GetUsageForEditor()
    {
        return string.Empty;
    }

    public override List<AbilityUtil_Targeter> CreateTargeters(Ability ability)
    {
        
        AbilityUtil_Targeter_Laser targeterLaser = new AbilityUtil_Targeter_Laser(
            ability,
            GetLaserWidth(),
            GetLaserRange(),
            false,
            GetMaxTargets(),
            IncludeAllies(),
            IncludeCaster());
        targeterLaser.SetUseMultiTargetUpdate(true);
        
        AbilityUtil_Targeter_RampartGrab targeterGrab = new AbilityUtil_Targeter_RampartGrab(
            ability,
            AbilityAreaShape.SingleSquare,
            GetMaxKnockbackDist(),
            KnockbackType.PullToSource,
            GetLaserRange(),
            m_laserWidth,
            false,
            GetMaxTargets());
        targeterGrab.SetUseMultiTargetUpdate(true);

        return new List<AbilityUtil_Targeter>
        {
            targeterLaser,
            targeterGrab
        };
    }

    protected override void OnTargetSelModApplied(TargetSelectModBase modBase)
    {
        m_targetSelMod = modBase as TargetSelectMod_LaserTargetedPull;
    }

    protected override void OnTargetSelModRemoved()
    {
        m_targetSelMod = null;
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

    public int GetMaxTargets()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_maxTargetsMod.GetModifiedValue(m_maxTargets)
            : m_maxTargets;
    }

    public float GetMaxKnockbackDist()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_maxKnockbackDistMod.GetModifiedValue(m_maxKnockbackDist)
            : m_maxKnockbackDist;
    }

    public float GetSquareRangeFromCaster()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_squareRangeFromCasterMod.GetModifiedValue(m_squareRangeFromCaster)
            : m_squareRangeFromCaster;
    }

    public float GetDestinationAngleDegWithBack()
    {
        return m_targetSelMod != null
            ? m_targetSelMod.m_destinationAngleDegWithBackMod.GetModifiedValue(m_destinationAngleDegWithBack)
            : m_destinationAngleDegWithBack;
    }

    public override bool HandleCustomTargetValidation(
        Ability ability,
        ActorData caster,
        AbilityTarget target,
        int targetIndex,
        List<AbilityTarget> currentTargets)
    {
        if (targetIndex > 0)
        {
            BoardSquare boardSquareSafe = Board.Get().GetSquare(target.GridPos);
            BoardSquare currentBoardSquare = caster.GetCurrentBoardSquare();
            if (boardSquareSafe == currentBoardSquare)
            {
                return m_casterSquareValidForKnockback; // false in rogues
            }

            if (GetSquareRangeFromCaster() > 0f
                && currentBoardSquare.HorizontalDistanceInSquaresTo(boardSquareSafe) > GetSquareRangeFromCaster())
            {
                return false;
            }

            if (m_destRequireLosFromCaster
                && !currentBoardSquare.GetLOS(boardSquareSafe.x, boardSquareSafe.y))
            {
                return false;
            }

            Vector3 from = -1f * currentTargets[0].AimDirection;
            Vector3 to = boardSquareSafe.ToVector3() - caster.GetFreePos();
            from.y = 0f;
            to.y = 0f;
            int num = Mathf.RoundToInt(Vector3.Angle(from, to));
            if (num > GetDestinationAngleDegWithBack())
            {
                return false;
            }

            if (NetworkClient.active)
            {
                List<ActorData> visibleActorsInRangeByTooltipSubject = ability.Targeters[0]
                    .GetVisibleActorsInRangeByTooltipSubject(AbilityTooltipSubject.Enemy);
                if (visibleActorsInRangeByTooltipSubject.Count > 0)
                {
                    bool flag = false;
                    foreach (var actorData in visibleActorsInRangeByTooltipSubject)
                    {
                        if (flag)
                        {
                            break;
                        }

                        BoardSquare actorSquare = actorData.GetCurrentBoardSquare();
                        flag = KnockbackUtils.CanBuildStraightLineChargePath(
                            caster,
                            boardSquareSafe,
                            actorSquare,
                            false,
                            out int _);
                    }

                    if (!flag)
                    {
                        return false;
                    }
                }
            }
        }

        return base.HandleCustomTargetValidation(ability, caster, target, targetIndex, currentTargets);
    }

    private List<ActorData> GetHitActors(
        List<AbilityTarget> targets,
        ActorData caster,
        out List<Vector3> targetPosForSequences,
        List<NonActorTargetInfo> nonActorTargetInfo)
    {
        targetPosForSequences = new List<Vector3>();
        List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
            caster.GetLoSCheckPos(),
            targets[0].AimDirection,
            GetLaserRange(),
            GetLaserWidth(),
            caster,
            TargeterUtils.GetRelevantTeams(caster, IncludeAllies(), IncludeEnemies()),
            false,
            GetMaxTargets(),
            false,
            true,
            out Vector3 laserEndPos,
            nonActorTargetInfo);
        
#if SERVER
        // rogues
        if (actorsInLaser.Count > 0)
        {
            Vector3 knockbackOriginFromLaser = AreaEffectUtils.GetKnockbackOriginFromLaser(
                actorsInLaser,
                caster,
                targets[0].AimDirection,
                laserEndPos);
            GetNonActorSpecificContext().SetValue(ContextKeys.s_KnockbackOrigin.GetKey(), knockbackOriginFromLaser);
        }
#endif

        targetPosForSequences.Add(laserEndPos);
        return actorsInLaser;
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
        foreach (ActorData actor in GetHitActors(targets, caster, out _, nonActorTargetInfo))
        {
            AddHitActor(actor, caster.GetLoSCheckPos());
        }
    }

    // rogues
    public override List<ServerClientUtils.SequenceStartData> CreateSequenceStartData(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData,
        Sequence.IExtraSequenceParams[] extraSequenceParams = null)
    {
        List<ActorData> hitActors = GetHitActors(targets, caster, out List<Vector3> targetPosForSequences, null);
        TargeterUtils.SortActorsByDistanceToPos(ref hitActors, targetPosForSequences[0]);
        if (additionalData.m_abilityResults.HasHitOnActor(caster) && !hitActors.Contains(caster))
        {
            hitActors.Add(caster);
        }

        return new List<ServerClientUtils.SequenceStartData>
        {
            new ServerClientUtils.SequenceStartData(
                m_castSequencePrefab,
                targetPosForSequences[0],
                hitActors.ToArray(),
                caster,
                additionalData.m_sequenceSource,
                extraSequenceParams)
        };
    }
#endif
}