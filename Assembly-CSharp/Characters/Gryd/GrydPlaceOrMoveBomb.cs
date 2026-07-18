// SERVER
// ROGUES
using System.Collections.Generic;
using UnityEngine;

public class GrydPlaceOrMoveBomb : Ability
{
    [Header("-- Targeting")]
    public bool m_lockToCardinalDirsForPlace = true;
    public bool m_lockToCardinalDirsForMove = true;
    public int m_placeRange = 4;
    public int m_moveRange = 4;
    public bool m_moveIsFreeAction = true;
    [Header("-- Enemy direct hit")]
    public bool m_explodeThisTurnOnDirectHit;
    public bool m_explodeImmediatelyOnMove;
    [Header("-- Bomb explosion")]
    public int m_bombDuration;
    public int m_damageAmount;
    public float m_explosionLaserRange;
    public float m_explosionLaserWidth;
    public int m_cooldownAfterExplode = 2;
    [Header("-- Anims")]
    public ActorModelData.ActionAnimationType m_moveBombAnimIndex = ActorModelData.ActionAnimationType.Ability2;
    [Header("-- Sequences")]
    public GameObject m_castSequencePrefab;
    public GameObject m_persistentBombSequencePrefab;
    public GameObject m_explodeBombSequencePrefab;
    public GameObject m_moveBombSequencePrefab;

    private Gryd_SyncComponent m_syncComp;

    private void Start()
    {
        if (m_abilityName == "Base Ability")
        {
            m_abilityName = "Place/Move Bomb";
        }

        m_syncComp = GetComponent<Gryd_SyncComponent>();
        SetupTargeter();
    }

    private void SetupTargeter()
    {
        Targeter = new AbilityUtil_Targeter_GrydBomb(this, m_moveRange);
    }

    public override bool CustomTargetValidation(
        ActorData caster,
        AbilityTarget target,
        int targetIndex,
        List<AbilityTarget> currentTargets)
    {
        if (HasPlacedBomb())
        {
            return true;
        }

        GridPos casterGridPos = caster.GetGridPos();
        if (m_lockToCardinalDirsForPlace && !CardinallyAligned(casterGridPos, target.GridPos))
        {
            return false;
        }

        BoardSquare targetSquare = Board.Get().GetSquare(target.GridPos);
        return targetSquare != null
               && targetSquare.IsValidForGameplay()
               && Mathf.Abs(casterGridPos.x - target.GridPos.x) <= m_placeRange
               && Mathf.Abs(casterGridPos.y - target.GridPos.y) <= m_placeRange
               && base.CustomTargetValidation(caster, target, targetIndex, currentTargets);
    }

    protected override List<AbilityTooltipNumber> CalculateAbilityTooltipNumbers()
    {
        List<AbilityTooltipNumber> numbers = new List<AbilityTooltipNumber>();
        AbilityTooltipHelper.ReportDamage(ref numbers, AbilityTooltipSubject.Primary, m_damageAmount);
        return numbers;
    }

    public override bool IsFreeAction()
    {
        return HasPlacedBomb()
            ? m_moveIsFreeAction
            : base.IsFreeAction();
    }

    public override ActorModelData.ActionAnimationType GetActionAnimType()
    {
        return HasPlacedBomb()
            ? m_moveBombAnimIndex
            : base.GetActionAnimType();
    }

    private bool CardinallyAligned(GridPos start, GridPos end)
    {
        return !start.CoordsEqual(end) && (start.x == end.x || start.y == end.y);
    }

    private GridPos GetPushEndPos(Vector3 targetPos, out bool hitActor)
    {
        GridPos placedBomb = GetPlacedBomb();
        BoardSquare placedBombSquare = Board.Get().GetSquare(placedBomb);
        Vector3 placedBombPos = placedBombSquare.ToVector3();
        Vector3 dir = targetPos - placedBombPos;
        if (m_lockToCardinalDirsForMove)
        {
            dir = VectorUtils.HorizontalAngleToClosestCardinalDirection(
                Mathf.RoundToInt(VectorUtils.HorizontalAngle_Deg(dir)));
        }

        hitActor = false;
        Vector3 abilityLineEndpoint = BarrierManager.Get().GetAbilityLineEndpoint(
            ActorData,
            placedBombPos,
            placedBombPos + dir * m_moveRange * Board.Get().squareSize,
            out bool collision,
            out _);
        BoardSquare hitSquare = null;
        if (collision)
        {
            hitSquare = Board.Get().GetSquareFromVec3(abilityLineEndpoint);
        }

        GridPos result = placedBomb;
        if (dir.x > 0.1f)
        {
            for (int i = placedBomb.x + 1; i <= placedBomb.x + m_moveRange; i++)
            {
                BoardSquare square = Board.Get().GetSquareFromIndex(i, placedBomb.y);
                if (square == null
                    || !square.IsValidForGameplay()
                    || !placedBombSquare.GetLOS(i, placedBomb.y)
                    || hitSquare != null && hitSquare.x < i)
                {
                    break;
                }

                result = square.GetGridPos();
                if (square.OccupantActor != null && square.OccupantActor.GetTeam() != ActorData.GetTeam())
                {
                    hitActor = true;
                    break;
                }
            }
        }
        else if (dir.x < -0.1f)
        {
            for (int i = placedBomb.x - 1; i >= placedBomb.x - m_moveRange; i--)
            {
                BoardSquare square = Board.Get().GetSquareFromIndex(i, placedBomb.y);
                if (square == null
                    || !square.IsValidForGameplay()
                    || !placedBombSquare.GetLOS(i, placedBomb.y)
                    || hitSquare != null && hitSquare.x > i)
                {
                    break;
                }

                result = square.GetGridPos();
                if (square.OccupantActor != null && square.OccupantActor.GetTeam() != ActorData.GetTeam())
                {
                    hitActor = true;
                    break;
                }
            }
        }
        else if (dir.z > 0.1f)
        {
            for (int i = placedBomb.y + 1; i <= placedBomb.y + m_moveRange; i++)
            {
                BoardSquare square = Board.Get().GetSquareFromIndex(placedBomb.x, i);
                if (square == null
                    || !square.IsValidForGameplay()
                    || !placedBombSquare.GetLOS(placedBomb.x, i)
                    || hitSquare != null && hitSquare.y < i)
                {
                    break;
                }

                result = square.GetGridPos();
                if (square.OccupantActor != null && square.OccupantActor.GetTeam() != ActorData.GetTeam())
                {
                    hitActor = true;
                    break;
                }
            }
        }
        else if (dir.z < -0.1f)
        {
            for (int i = placedBomb.y - 1; i >= placedBomb.y - m_moveRange; i--)
            {
                BoardSquare square = Board.Get().GetSquareFromIndex(placedBomb.x, i);
                if (square == null
                    || !square.IsValidForGameplay()
                    || !placedBombSquare.GetLOS(placedBomb.x, i)
                    || hitSquare != null && hitSquare.y > i)
                {
                    break;
                }

                result = square.GetGridPos();
                if (square.OccupantActor != null && square.OccupantActor.GetTeam() != ActorData.GetTeam())
                {
                    hitActor = true;
                    break;
                }
            }
        }

        return result;
    }

    public bool HasPlacedBomb()
    {
        return GetPlacedBomb().x > 0 && GetPlacedBomb().y > 0;
    }

    public GridPos GetPlacedBomb()
    {
        return m_syncComp != null
            ? m_syncComp.m_bombLocation
            : GridPos.s_invalid;
    }

#if SERVER
    // added in rogues
    private GrydBombEffect GetBombEffect(GridPos targetPos, ActorData caster)
    {
        foreach (Effect effect in ServerEffectManager.Get().GetWorldEffectsByCaster(caster, typeof(GrydBombEffect)))
        {
            if (targetPos.CoordsEqual(effect.TargetSquare.GetGridPos()))
            {
                return effect as GrydBombEffect;
            }
        }

        return null;
    }

    // added in rogues
    public override void Run(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        base.Run(targets, caster, additionalData);

        if (m_syncComp == null)
        {
            return;
        }

        if (HasPlacedBomb())
        {
            if (m_explodeImmediatelyOnMove)
            {
                m_syncComp.m_bombLocation = GridPos.s_invalid;
            }
            else
            {
                m_syncComp.m_bombLocation = GetPushEndPos(targets[0].FreePos, out bool hitActor);
                if (m_explodeImmediatelyOnMove || hitActor)
                {
                    m_syncComp.m_bombLocation = GridPos.s_invalid;
                }
            }
        }
        else
        {
            ActorData occupantActor = Board.Get().GetSquare(targets[0].GridPos).OccupantActor;
            if (occupantActor != null && occupantActor.GetTeam() != caster.GetTeam())
            {
                m_syncComp.m_bombLocation = GridPos.s_invalid;
            }
            else
            {
                m_syncComp.m_bombLocation = targets[0].GridPos;
            }
        }
    }

    // added in rogues
    public override List<ServerClientUtils.SequenceStartData> GetAbilityRunSequenceStartDataList(
        List<AbilityTarget> targets,
        ActorData caster,
        ServerAbilityUtils.AbilityRunData additionalData)
    {
        List<ServerClientUtils.SequenceStartData> result = new List<ServerClientUtils.SequenceStartData>();
        BoardSquare square = Board.Get().GetSquare(targets[0].GridPos);
        GrydBombEffect bombEffect = GetBombEffect(GetPlacedBomb(), caster);
        GameObject prefab = m_castSequencePrefab;
        SplineProjectileSequence.DelayedProjectileExtraParams delayedProjectileExtraParams =
            new SplineProjectileSequence.DelayedProjectileExtraParams();
        bool isExploding;
        if (bombEffect != null)
        {
            result.Add(
                new ServerClientUtils.SequenceStartData(
                    null,
                    bombEffect.TargetSquare.ToVector3(),
                    new ActorData[0],
                    caster,
                    additionalData.m_sequenceSource));
            square = Board.Get().GetSquare(GetPushEndPos(targets[0].FreePos, out bool hitActor));
            prefab = m_moveBombSequencePrefab;
            delayedProjectileExtraParams.useOverrideStartPos = true;
            delayedProjectileExtraParams.overrideStartPos = bombEffect.TargetSquare.ToVector3();
            isExploding = m_explodeImmediatelyOnMove || hitActor;
        }
        else
        {
            isExploding = m_explodeThisTurnOnDirectHit
                          && square.OccupantActor != null
                          && square.OccupantActor.GetTeam() != caster.GetTeam();
        }

        result.Add(
            new ServerClientUtils.SequenceStartData(
                prefab,
                square,
                new ActorData[0],
                caster,
                additionalData.m_sequenceSource,
                delayedProjectileExtraParams.ToArray()));
        if (isExploding)
        {
            float explosionRangeInWorld = m_explosionLaserRange * Board.Get().squareSize;
            result.Add(
                new ServerClientUtils.SequenceStartData(
                    m_explodeBombSequencePrefab,
                    square.ToVector3(),
                    Quaternion.LookRotation(new Vector3(explosionRangeInWorld, 0f, 0f)),
                    additionalData.m_abilityResults.HitActorsArray(),
                    caster,
                    additionalData.m_sequenceSource));
            result.Add(
                new ServerClientUtils.SequenceStartData(
                    m_explodeBombSequencePrefab,
                    square.ToVector3(),
                    Quaternion.LookRotation(new Vector3(-explosionRangeInWorld, 0f, 0f)),
                    new ActorData[0],
                    caster,
                    additionalData.m_sequenceSource));
            result.Add(
                new ServerClientUtils.SequenceStartData(
                    m_explodeBombSequencePrefab,
                    square.ToVector3(),
                    Quaternion.LookRotation(new Vector3(0f, 0f, explosionRangeInWorld)),
                    new ActorData[0],
                    caster,
                    additionalData.m_sequenceSource));
            result.Add(
                new ServerClientUtils.SequenceStartData(
                    m_explodeBombSequencePrefab,
                    square.ToVector3(),
                    Quaternion.LookRotation(new Vector3(0f, 0f, -explosionRangeInWorld)),
                    new ActorData[0],
                    caster,
                    additionalData.m_sequenceSource));
        }

        return result;
    }

    // added in rogues
    public override void GatherAbilityResults(
        List<AbilityTarget> targets,
        ActorData caster,
        ref AbilityResults abilityResults)
    {
        GrydBombEffect bombEffect = GetBombEffect(GetPlacedBomb(), caster);
        Vector3 vector;
        BoardSquare square;
        bool isExploding;
        if (bombEffect != null)
        {
            vector = bombEffect.TargetSquare.ToVector3();
            PositionHitResults positionHitResults = new PositionHitResults(new PositionHitParameters(vector));
            positionHitResults.AddEffectForRemoval(bombEffect, ServerEffectManager.Get().GetWorldEffects());
            abilityResults.StorePositionHit(positionHitResults);
            GridPos pushEndPos = GetPushEndPos(targets[0].FreePos, out bool hitActor);
            square = Board.Get().GetSquare(pushEndPos);
            isExploding = m_explodeImmediatelyOnMove || hitActor;
        }
        else
        {
            square = Board.Get().GetSquare(targets[0].GridPos);
            isExploding = m_explodeThisTurnOnDirectHit
                          && square.OccupantActor != null
                          && square.OccupantActor.GetTeam() != caster.GetTeam();
            vector = square.ToVector3();
        }

        if (isExploding)
        {
            List<NonActorTargetInfo> nonActorTargetInfos = new List<NonActorTargetInfo>();
            foreach (ActorData target in GetHitActors(caster, square, nonActorTargetInfos))
            {
                ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(target, vector));
                actorHitResults.AddBaseDamage(m_damageAmount);
                abilityResults.StoreActorHit(actorHitResults);
            }

            ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(caster, caster.GetFreePos()));
            casterHitResults.AddMiscHitEvent(
                new MiscHitEventData_AddToCasterCooldown(
                    caster.GetAbilityData().GetActionTypeOfAbility(this),
                    m_cooldownAfterExplode)
                {
                    m_ignoreCooldownMax = true
                });
            abilityResults.StoreActorHit(casterHitResults);
            abilityResults.StoreNonActorTargetInfo(nonActorTargetInfos);
        }
        else
        {
            PositionHitResults positionHitResults =
                new PositionHitResults(new PositionHitParameters(square.ToVector3()));
            positionHitResults.AddEffect(
                new GrydBombEffect(
                    AsEffectSource(),
                    square,
                    caster,
                    m_damageAmount,
                    m_explosionLaserRange,
                    m_explosionLaserWidth,
                    false,
                    m_bombDuration,
                    m_persistentBombSequencePrefab,
                    m_explodeBombSequencePrefab,
                    m_cooldownAfterExplode));
            abilityResults.StorePositionHit(positionHitResults);
        }
    }

    // added in rogues
    private List<ActorData> GetHitActors(
        ActorData caster,
        BoardSquare targetSquare,
        List<NonActorTargetInfo> nonActorTargets)
    {
        Vector3 targetPos = caster.GetLoSCheckPos(targetSquare);
        List<ActorData> result;
        if (m_explosionLaserRange <= 0f)
        {
            result = new List<ActorData>();
            if (targetSquare.OccupantActor != null
                && targetSquare.OccupantActor.GetTeam() != caster.GetTeam())
            {
                result.Add(targetSquare.OccupantActor);
            }
        }
        else
        {
            List<Team> otherTeams = caster.GetOtherTeams();
            result = AreaEffectUtils.GetActorsInLaser(
                targetPos,
                new Vector3(1f, 0f, 0f),
                m_explosionLaserRange,
                m_explosionLaserWidth,
                caster,
                otherTeams,
                false,
                0,
                false,
                true,
                out _,
                nonActorTargets);
            result.AddRange(
                AreaEffectUtils.GetActorsInLaser(
                    targetPos,
                    new Vector3(-1f, 0f, 0f),
                    m_explosionLaserRange,
                    m_explosionLaserWidth,
                    caster,
                    otherTeams,
                    false,
                    0,
                    false,
                    true,
                    out _,
                    nonActorTargets,
                    result));
            result.AddRange(
                AreaEffectUtils.GetActorsInLaser(
                    targetPos,
                    new Vector3(0f, 0f, 1f),
                    m_explosionLaserRange,
                    m_explosionLaserWidth,
                    caster,
                    otherTeams,
                    false,
                    0,
                    false,
                    true,
                    out _,
                    nonActorTargets,
                    result));
            result.AddRange(
                AreaEffectUtils.GetActorsInLaser(
                    targetPos,
                    new Vector3(0f, 0f, -1f),
                    m_explosionLaserRange,
                    m_explosionLaserWidth,
                    caster,
                    otherTeams,
                    false,
                    0,
                    false,
                    true,
                    out _,
                    nonActorTargets,
                    result));
        }

        return result;
    }
#endif
}