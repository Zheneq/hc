// ROGUES
// SERVER

using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if SERVER
// custom
public abstract class NekoAbstractDiscEffect : Effect
{
    protected readonly List<BoardSquare> m_targetSquares;
    protected readonly float m_discReturnEndRadius;
    public readonly int m_returnTripDamage;
    protected readonly int m_returnTripSubsequentHitDamage;
    protected readonly float m_returnTripExtraDamagePerDist;
    protected readonly StandardEffectInfo m_returnTripEnemyEffect;
    protected readonly bool m_returnTripIgnoreCover;
    
    protected readonly GameObject m_returnTripSequencePrefab;
    protected readonly GameObject m_persistentDiscSequencePrefab;
    
    protected Neko_SyncComponent m_syncComponent;
    protected NekoFlipDash m_dashAbility;
    protected NekoEnlargeDisc m_enlargeDiscAbility;

    protected static readonly Vector3 HIT_POS = new Vector3(1, 1, 1);

    public NekoAbstractDiscEffect(
        EffectSource parent,
        List<BoardSquare> targetSquares,
        ActorData target,
        ActorData caster,
        float discReturnEndRadius,
        int returnTripDamage,
        int returnTripSubsequentHitDamage,
        float returnTripExtraDamagePerDist,
        StandardEffectInfo returnTripEnemyEffect,
        bool returnTripIgnoreCover,
        GameObject returnTripSequencePrefab,
        GameObject persistentDiscSequencePrefab)
        : base(parent, targetSquares[0], target, caster)
    {
        m_discReturnEndRadius = discReturnEndRadius;
        m_returnTripDamage = returnTripDamage;
        m_returnTripSubsequentHitDamage = returnTripSubsequentHitDamage;
        m_returnTripExtraDamagePerDist = returnTripExtraDamagePerDist;
        m_returnTripEnemyEffect = returnTripEnemyEffect;
        m_returnTripIgnoreCover = returnTripIgnoreCover;
        m_returnTripSequencePrefab = returnTripSequencePrefab;
        m_persistentDiscSequencePrefab = persistentDiscSequencePrefab;
        m_targetSquares = targetSquares;
        m_time.duration = 2;
        HitPhase = AbilityPriority.Combat_Damage;
    }

    public override void OnStart()
    {
        m_syncComponent = Caster.GetComponent<Neko_SyncComponent>();
        m_enlargeDiscAbility = Caster.GetAbilityData().GetAbilityOfType<NekoEnlargeDisc>();
        m_dashAbility = Caster.GetAbilityData().GetAbilityOfType<NekoFlipDash>();
        foreach (BoardSquare targetSquare in m_targetSquares)
        {
            m_syncComponent.AddDisk(targetSquare);
        }
    }

    public override void OnEnd()
    {
        foreach (BoardSquare targetSquare in m_targetSquares)
        {
            m_syncComponent.RemoveDisk(targetSquare);
        }
    }

    public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
    {
        return m_targetSquares
            .Select(targetSquare => new ServerClientUtils.SequenceStartData(
                m_persistentDiscSequencePrefab,
                targetSquare.GetPosAtBaselineHeight(),
                Target?.AsArray(),
                Caster,
                SequenceSource))
            .ToList();
    }

    protected virtual bool SetAnimParamForNormalDisc()
    {
        return false;
    }

    public override int GetTheatricsSortPriority(AbilityData.ActionType actionType)
    {
        return 2;
    }

    public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
    {
        if (forActor.GetTeam() != Caster.GetTeam())
        {
            foreach (BoardSquare targetSquare in m_targetSquares)
            {
                squaresToAvoid.Add(targetSquare);
            }
        }
    }

    public abstract List<ActorData> GetHitActors();

    protected void ProcessHits(
        EffectResults effectResults,
        List<ActorData> discHitActorsSorted,
        Vector3 startLosPos,
        Vector3 endLosPos,
        bool isEnlarged)
    {
        bool isDashing = ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Caster, typeof(NekoFlipDash));
        int extraDamage = Mathf.RoundToInt(m_returnTripExtraDamagePerDist 
            * (endLosPos - startLosPos).magnitude / Board.Get().squareSize);

        int baseDamage = m_returnTripDamage;
        foreach (ActorData hitActor in discHitActorsSorted)
        {
            Vector3 origin = m_returnTripIgnoreCover ? hitActor.GetFreePos() : endLosPos; // TODO NEKO Vector3.zero in vanilla -- we need custom logic for creating DamageSource for effect with customizable ignore cover flag
            ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, origin));
            if (hitActor.GetTeam() != Caster.GetTeam())
            {
                actorHitResults.AddStandardEffectInfo(m_returnTripEnemyEffect);
                actorHitResults.AddBaseDamage(baseDamage + extraDamage);
                if (isDashing)
                {
                    actorHitResults.AddBaseDamage(m_dashAbility.GetDiscsReturningThisTurnExtraDamage());
                }
                if (isEnlarged)
                {
                    actorHitResults.AddBaseDamage(m_enlargeDiscAbility.GetAdditionalDamageAmount());
                    actorHitResults.AddStandardEffectInfo(m_enlargeDiscAbility.GetEffectOnEnemies());
                }
                baseDamage = m_returnTripSubsequentHitDamage;
            }
            else
            {
                if (isEnlarged)
                {
                    actorHitResults.AddBaseHealing(m_enlargeDiscAbility.GetAllyHeal());
                    actorHitResults.AddStandardEffectInfo(m_enlargeDiscAbility.GetAllyHitEffect());
                }
            }
            effectResults.StoreActorHit(actorHitResults);
        }
    }

    protected void ProcessAdditionalEnlargedEffects(List<ActorData> hitActors, ActorHitResults casterHitResults)
    {
        int enlargeEnemiesHit = hitActors.Count(ha => ha.GetTeam() != Caster.GetTeam());
        if (enlargeEnemiesHit > 0)
        {
            StandardActorEffectData shieldEffectData = m_enlargeDiscAbility.GetShieldEffectData().GetShallowCopy();
            shieldEffectData.m_nextTurnAbsorbAmount = m_enlargeDiscAbility.GetShieldPerTargetHitOnReturn() * enlargeEnemiesHit;
            casterHitResults.AddEffect(new StandardActorEffect(
                Parent, Caster.GetCurrentBoardSquare(), Caster, Caster, shieldEffectData));
        }
        else
        {
            casterHitResults.AddMiscHitEvent(new MiscHitEventData_AddToCasterCooldown(
                AbilityData.ActionType.ABILITY_1, -m_enlargeDiscAbility.GetCdrIfHitNoOne()));
        }
    }

    // TODO NEKO basically repeats SyncComponent.UpdateActorsInDiscPath
    protected List<ActorData> GetActorsInDiscPath(
        Vector3 startLosPos,
        Vector3 endLosPos,
        float laserWidth,
        float aoeStartRadius,
        float aoeEndRadius,
        bool usingEnlargeDiscAbility)
    {
        List<ActorData> hitActors = new List<ActorData>();
        
        List<Team> relevantTeams = Caster.GetEnemyTeamAsList();
        if (usingEnlargeDiscAbility
            && m_enlargeDiscAbility != null
            && m_enlargeDiscAbility.CanIncludeAlliesOnReturn())
        {
            relevantTeams.Add(Caster.GetTeam());
        }
    
        Vector3 dir = endLosPos - startLosPos;
        dir.y = 0f;
        float laserRangeInSquares = dir.magnitude / Board.Get().squareSize;
        List<ActorData> actorsInStartRadius = AreaEffectUtils.GetActorsInRadius(
            startLosPos, aoeStartRadius, true, Caster, relevantTeams, null);
        foreach (ActorData actor in actorsInStartRadius)
        {
            if (!hitActors.Contains(actor))
            {
                hitActors.Add(actor);
            }
        }
    
        if (laserRangeInSquares <= 0f)
        {
            return hitActors;
        }
    
        List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
            startLosPos,
            dir,
            laserRangeInSquares,
            laserWidth,
            Caster,
            relevantTeams,
            true,
            0,
            true,
            true,
            out _,
            null);
        foreach (ActorData item in actorsInLaser)
        {
            if (!hitActors.Contains(item))
            {
                hitActors.Add(item);
            }
        }
    
        if (aoeEndRadius > 0f)
        {
            List<ActorData> actorsInEndRadius = AreaEffectUtils.GetActorsInRadius(
                endLosPos, aoeEndRadius, true, Caster, relevantTeams, null);
            foreach (ActorData actor in actorsInEndRadius)
            {
                if (!hitActors.Contains(actor))
                {
                    hitActors.Add(actor);
                }
            }
        }
    
        return hitActors;
    }
    
    // TODO NEKO see Neko_SyncComponent.GetCasterPos
    protected Vector3 GetCasterPos()
    {
        BoardSquare casterSquare = Caster.GetCurrentBoardSquare() ?? Caster.GetMostRecentDeathSquare();
        return casterSquare != null
            ? casterSquare.GetOccupantLoSPos()
            : Vector3.zero;
    }

    public void ProcessEnlargeDiscExtraDamageFreelancerStat(ActorData hitCaster, ActorHitResults results)
    {
        if (ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Caster, typeof(NekoEnlargeDisc)))
        {
            int extraDamage = Mathf.RoundToInt((float)m_enlargeDiscAbility.GetAdditionalDamageAmount() 
                              * results.FinalDamage / results.BaseDamage);
            hitCaster.GetFreelancerStats().AddToValueOfStat(
                FreelancerStats.NekoStats.EnlargeDiscExtraDamage,
                extraDamage);
        }
    }
}
#endif
