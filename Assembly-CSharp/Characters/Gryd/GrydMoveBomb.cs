// SERVER
// ROGUES
using System.Collections.Generic;
using UnityEngine;

public class GrydMoveBomb : Ability
{
    [Header("-- Enemy direct hit")]
    public bool m_explodeThisTurnOnDirectHit;
    [Header("-- Targeting")]
    public int m_moveRange = 4;
    public bool m_selectBombsThroughLoS = true;
    public bool m_moveBombsThroughLoS;
    [Header("-- Sequences")]
    public GameObject m_castSequencePrefab;

    private GrydPlaceBomb m_placeBombAbility;

    private void Start()
    {
        if (m_abilityName == "Base Ability")
        {
            m_abilityName = "Move Bomb";
        }

        m_placeBombAbility = ActorData.GetAbilityData().GetAbilityOfType(typeof(GrydPlaceBomb)) as GrydPlaceBomb;
        SetupTargeter();
    }

    private void SetupTargeter()
    {
        Targeters.Clear();
        Targeters.Add(new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, m_selectBombsThroughLoS));
        Targeters.Add(new AbilityUtil_Targeter_Shape(this, AbilityAreaShape.SingleSquare, m_moveBombsThroughLoS));
    }

    public override int GetExpectedNumberOfTargeters()
    {
        return 2;
    }

    public override bool CustomCanCastValidation(ActorData caster)
    {
#if SERVER
        // rogues
        return !ServerEffectManager.Get().GetWorldEffectsByCaster(caster, typeof(GrydBombEffect)).IsNullOrEmpty();
#else
        return true;
#endif
    }

    public override bool CustomTargetValidation(
        ActorData caster,
        AbilityTarget target,
        int targetIndex,
        List<AbilityTarget> currentTargets)
    {
#if SERVER
        // rogues
        BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
        GrydBombEffect bombOnSquare = GetBombOnSquare(targetSquare, caster);
        if (targetIndex == 0)
        {
            if (bombOnSquare == null)
            {
                return false;
            }
        }
        else
        {
            if (bombOnSquare != null)
            {
                return false;
            }

            BoardSquare firstTargetPos = Board.Get().GetSquare(currentTargets[0].GridPos);
            if (firstTargetPos.HorizontalDistanceInSquaresTo(targetSquare) > m_moveRange)
            {
                return false;
            }

            if (!m_moveBombsThroughLoS && !firstTargetPos.GetLOS(targetSquare.x, targetSquare.y))
            {
                return false;
            }
        }
#endif

        return true;
    }

#if SERVER
    // added in rogues
    private GrydBombEffect GetBombOnSquare(BoardSquare targetSquare, ActorData caster)
    {
        foreach (Effect effect in ServerEffectManager.Get().GetWorldEffectsByCaster(caster, typeof(GrydBombEffect)))
        {
            if (targetSquare == effect.TargetSquare)
            {
                return effect as GrydBombEffect;
            }
        }

        return null;
    }

    // added in rogues
    public override ServerClientUtils.SequenceStartData GetAbilityRunSequenceStartData(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        return new ServerClientUtils.SequenceStartData(
            m_castSequencePrefab,
            Board.Get().GetSquare(targets[1].GridPos),
            additionalData.m_abilityResults.HitActorsArray(),
            caster,
            additionalData.m_sequenceSource);
    }

    // added in rogues
    public override void GatherAbilityResults(
        List<AbilityTarget> targets,
        ActorData caster,
        ref AbilityResults abilityResults)
    {
        GrydBombEffect bombOnSquare = GetBombOnSquare(Board.Get().GetSquare(targets[0].GridPos), caster);
        if (bombOnSquare == null)
        {
            return;
        }

        PositionHitResults positionHitResults =
            new PositionHitResults(new PositionHitParameters(targets[1].FreePos));
        positionHitResults.AddEffectForRemoval(bombOnSquare, ServerEffectManager.Get().GetWorldEffects());
        BoardSquare targetSquare = Board.Get().GetSquare(targets[1].GridPos);
        bool explodeFirstTurn = m_placeBombAbility.m_explodeThisTurnOnDirectHit
                                && targetSquare.OccupantActor != null
                                && targetSquare.OccupantActor.GetTeam() != caster.GetTeam();
        positionHitResults.AddEffect(
            new GrydBombEffect(
                AsEffectSource(),
                targetSquare,
                caster,
                m_placeBombAbility.m_damageAmount,
                m_placeBombAbility.m_explosionLaserRange,
                m_placeBombAbility.m_explosionLaserWidth,
                explodeFirstTurn,
                m_placeBombAbility.m_bombDuration,
                m_placeBombAbility.m_persistentBombSequencePrefab,
                m_placeBombAbility.m_explodeBombSequencePrefab,
                0));
        abilityResults.StorePositionHit(positionHitResults);
    }
#endif
}