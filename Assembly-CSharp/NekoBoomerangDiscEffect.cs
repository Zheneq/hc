// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if SERVER
// custom
public class NekoBoomerangDiscEffect : Effect
{
    private List<BoardSquare> m_targetSquares;
    private float m_discReturnEndRadius;
    private int m_returnTripDamage;
    private bool m_returnTripIgnoreCover;
    private int m_extraReturnDamage;
    private int m_energyOnMissOnReturnTrip; // TODO NEKO
    
    private GameObject m_returnTripSequencePrefab;
    private GameObject m_persistentDiscSequencePrefab;
    
    private Neko_SyncComponent m_syncComponent;
    private NekoBoomerangDisc m_primaryAbility;
    private NekoHomingDisc m_homingDiscAbility;
    private NekoEnlargeDisc m_enlargeDiscAbility;

    private static readonly Vector3 HIT_POS = new Vector3(1, 1, 1);

    public NekoBoomerangDiscEffect(
        EffectSource parent,
        List<BoardSquare> targetSquares,
        ActorData caster,
        float discReturnEndRadius,
        int returnTripDamage,
        bool returnTripIgnoreCover,
        int extraReturnDamage,
        int energyOnMissOnReturnTrip,
        GameObject returnTripSequencePrefab,
        GameObject persistentDiscSequencePrefab)
        : base(parent, targetSquares[0], null, caster)
    {
        m_discReturnEndRadius = discReturnEndRadius;
        m_returnTripDamage = returnTripDamage;
        m_returnTripIgnoreCover = returnTripIgnoreCover;
        m_extraReturnDamage = extraReturnDamage;
        m_energyOnMissOnReturnTrip = energyOnMissOnReturnTrip;
        m_returnTripSequencePrefab = returnTripSequencePrefab;
        m_persistentDiscSequencePrefab = persistentDiscSequencePrefab;
        m_targetSquares = targetSquares;
        m_time.duration = 2;
        HitPhase = AbilityPriority.Combat_Damage;
    }

    public override void OnStart()
    {
        m_syncComponent = Caster.GetComponent<Neko_SyncComponent>();
        m_primaryAbility = Caster.GetAbilityData().GetAbilityOfType<NekoBoomerangDisc>();
        m_enlargeDiscAbility = Caster.GetAbilityData().GetAbilityOfType<NekoEnlargeDisc>();
        m_homingDiscAbility = Caster.GetAbilityData().GetAbilityOfType<NekoHomingDisc>();
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
                targetSquare.ToVector3(),
                null,
                Caster,
                SequenceSource))
            .ToList();
    }

    public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
    {
        List<ServerClientUtils.SequenceStartData> effectHitSeqDataList = base.GetEffectHitSeqDataList();
        float height = Board.Get().LosCheckHeight;
        Vector3 endPos = GetCasterPos();
        endPos.y = height;
        
        SequenceSource seqSource = SequenceSource.GetShallowCopy();
        if (GetCasterAnimationIndex(HitPhase) > 0 || AddActorAnimEntryIfHasHits(HitPhase))
        {
            seqSource.SetWaitForClientEnable(true);
        }

        BoardSquare farthestSquare = m_targetSquares[0];
        float maxDistSqr = (farthestSquare.ToVector3() - endPos).sqrMagnitude;
        foreach (BoardSquare targetSquare in m_targetSquares)
        {
            float distSqr = (targetSquare.ToVector3() - endPos).sqrMagnitude;
            if (distSqr > maxDistSqr)
            {
                farthestSquare = targetSquare;
                maxDistSqr = distSqr;
            }
        }
        
        List<List<ActorData>> hitActorsPerDisc = GetHitActors(out List<BoardSquare> activeDiscSquares, out _, out int enlargedDiscIndex);
        for (int i = 0; i < activeDiscSquares.Count; i++)
        {
            BoardSquare targetSquare = activeDiscSquares[i];
            Vector3 startPos = targetSquare.ToVector3();
            startPos.y = height;
            bool isFarthestDisc = targetSquare == farthestSquare;
            bool isEnlargedDisc = i == enlargedDiscIndex;

            List<ActorData> hitActors = hitActorsPerDisc[i];
            if (isFarthestDisc && !hitActors.Contains(Caster))
            {
                hitActors.Add(Caster);
            }

            effectHitSeqDataList.Add(new ServerClientUtils.SequenceStartData(
                isEnlargedDisc ? m_enlargeDiscAbility.m_discReturnOverrideSequencePrefab : m_returnTripSequencePrefab,  // TODO m_enlargeDiscAbility.m_prepDiscReturnOverrideSequencePrefab
                endPos,
                hitActors.ToArray(),
                Caster,
                seqSource,
                new Sequence.IExtraSequenceParams[]
                {
                    new SplineProjectileSequence.DelayedProjectileExtraParams
                    {
                        useOverrideStartPos = true,
                        overrideStartPos = startPos
                    },
                    new NekoDiscReturnProjectileSequence.DiscReturnProjectileExtraParams
                    {
                        setAnimDistParamWithThisProjectile = isFarthestDisc,
                        setAnimParamForNormalDisc = true // TODO NEKO check. false when dead? or not casting a new disc? false when embiggify?
                    } // TODO NEKO waitForClientEnable = true??
                }));
        }

        effectHitSeqDataList.Add(new ServerClientUtils.SequenceStartData(
            SequenceLookup.Get().GetSimpleHitSequencePrefab(),
            HIT_POS,
            new ActorData[] {},
            Caster,
            seqSource));
        return effectHitSeqDataList;
    }

    public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
    {
        List<List<ActorData>> hitActorsPerDisc = GetHitActors(out _, out Vector3 endLosPos, out int enlargedDiscIndex);

        ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(Caster, Caster.GetFreePos()));
        
        List<ActorData> processedActors = new List<ActorData>();
        if (enlargedDiscIndex >= 0)
        {
            List<ActorData> discHitActors = hitActorsPerDisc[enlargedDiscIndex];
            ProcessHits(effectResults, discHitActors, endLosPos, true);
        }
        for (int i = 0; i < hitActorsPerDisc.Count; i++)
        {
            if (hitActorsPerDisc[i].Count == 0)
            {
                casterHitResults.AddTechPointGainOnCaster(m_energyOnMissOnReturnTrip);
            }

            if (i == enlargedDiscIndex)
            {
                continue;
            }
            
            List<ActorData> discHitActors = hitActorsPerDisc[i].Where(ha => !processedActors.Contains(ha)).ToList();
            ProcessHits(effectResults, discHitActors, endLosPos, false);
            processedActors.AddRange(discHitActors);
        }
        
        if (enlargedDiscIndex >= 0)
        {
            int enlargeEnemiesHit = hitActorsPerDisc[enlargedDiscIndex].Count(ha => ha.GetTeam() != Caster.GetTeam());
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
        effectResults.StoreActorHit(casterHitResults);
        
        PositionHitParameters positionHitParams = new PositionHitParameters(HIT_POS);
        PositionHitResults positionHitResults = new PositionHitResults(positionHitParams);
        positionHitResults.AddEffectSequenceToEnd(m_persistentDiscSequencePrefab, m_guid);
        effectResults.StorePositionHit(positionHitResults);
    }

    private List<List<ActorData>> GetHitActors(out List<BoardSquare> activeDiscSquares, out Vector3 endLosPos, out int enlargedDiscIndex)
    {
        float losHeight = Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
        endLosPos = GetCasterPos();
        endLosPos.y = losHeight;

        BoardSquare enlargedDiscSquare = null;
        if (ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Caster, typeof(NekoEnlargeDisc)))
        {
            AbilityRequest request = ServerActionBuffer.Get().GetAllStoredAbilityRequests()
                .FirstOrDefault(ar => ar.m_ability == m_enlargeDiscAbility);
            if (request != null && !request.m_targets.IsNullOrEmpty())
            {
                enlargedDiscSquare = Board.Get().GetSquare(request.m_targets[0].GridPos);
            }
        }

        enlargedDiscIndex = -1;
        List<List<ActorData>> hitActorsPerDisc = new List<List<ActorData>>();
        activeDiscSquares = m_syncComponent.GetActiveDiscSquares();
        for (var i = 0; i < activeDiscSquares.Count; i++)
        {
            BoardSquare targetSquare = activeDiscSquares[i];
            Vector3 startLosPos = targetSquare.ToVector3();
            startLosPos.y = losHeight;

            float returnDiskLaserWidth = m_syncComponent.m_discReturnTripLaserWidthInSquares;
            float aoeStartRadius = m_syncComponent.m_discReturnTripAoeRadiusAtlaserStart;
            float returnDiskEndRadius = m_primaryAbility.GetDiscReturnEndRadius();
            bool isDiscEnlarged = targetSquare == enlargedDiscSquare;
            if (isDiscEnlarged)
            {
                returnDiskLaserWidth = m_enlargeDiscAbility.GetLaserWidth();
                aoeStartRadius = m_enlargeDiscAbility.GetAoeRadius();
                returnDiskEndRadius = Mathf.Max(returnDiskEndRadius, m_enlargeDiscAbility.GetReturnEndAoeRadius());
                enlargedDiscIndex = i;
            }

            hitActorsPerDisc.Add(GetActorsInDiscPath(
                startLosPos,
                endLosPos,
                returnDiskLaserWidth,
                aoeStartRadius,
                returnDiskEndRadius,
                isDiscEnlarged));
        }

        return hitActorsPerDisc;
    }

    private void ProcessHits(
        EffectResults effectResults,
        List<ActorData> discHitActors,
        Vector3 endLosPos,
        bool isEnlarged)
    {
        foreach (ActorData hitActor in discHitActors)
        {
            Vector3 origin = m_returnTripIgnoreCover ? hitActor.GetFreePos() : endLosPos; // TODO NEKO 0,0,0 in vanilla
            ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, origin));
            if (hitActor.GetTeam() != Caster.GetTeam())
            {
                actorHitResults.AddBaseDamage(m_returnTripDamage + m_extraReturnDamage);
                if (isEnlarged)
                {
                    actorHitResults.AddBaseDamage(m_enlargeDiscAbility.GetAdditionalDamageAmount());
                    actorHitResults.AddStandardEffectInfo(m_enlargeDiscAbility.GetEffectOnEnemies());
                }
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

    // TODO NEKO basically repeats SyncComponent.UpdateActorsInDiscPath
    private List<ActorData> GetActorsInDiscPath(
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
    private Vector3 GetCasterPos()
    {
        BoardSquare casterSquare = Caster.GetCurrentBoardSquare() ?? Caster.GetMostRecentDeathSquare();
        return casterSquare != null
            ? casterSquare.ToVector3()
            : Vector3.zero;
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

    public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
    {
        return true;
    }
    
    public override int GetCasterAnimationIndex(AbilityPriority phaseIndex)
    {
        if (phaseIndex != AbilityPriority.Combat_Damage)
        {
            return base.GetCasterAnimationIndex(phaseIndex);
        }
        
        // TODO NEKO doesnt work?
        return ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Caster, typeof(NekoEnlargeDisc))
            ? 11   // catch with both hands
            : 10;  // catch with one hand
    }

    // TODO NEKO freelancer stats
    // public override void OnTurnStart()
    // {
    //     base.OnTurnStart();
    //     if (m_time.age < m_turnsBeforeExploding)
    //     {
    //         return;
    //     }
    //     m_targetsOnHitTurnStart = AreaEffectUtils.GetActorsInShape(
    //         m_shape,
    //         m_targetSquare.ToVector3(),
    //         m_targetSquare,
    //         true,
    //         Caster,
    //         Caster.GetOtherTeams(),
    //         null);
    // }
    //
    // public override void OnExecutedEffectResults(EffectResults effectResults)
    // {
    //     base.OnExecutedEffectResults(effectResults);
    //     int num = m_targetsOnHitTurnStart.Count(ad => !effectResults.StoredHitForActor(ad));
    //     effectResults.Caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.BazookaGirlStats.DashesOutOfBigOne, num);
    // }
}
#endif
