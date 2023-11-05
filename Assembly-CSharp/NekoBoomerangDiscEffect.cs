// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if SERVER
// custom
public class NekoBoomerangDiscEffect : Effect
{
    private BoardSquare m_targetSquare;
    private float m_discReturnEndRadius;
    private int m_returnTripDamage;
    private bool m_returnTripIgnoreCover;
    private int m_extraReturnDamage;
    
    private GameObject m_returnTripSequencePrefab;
    private GameObject m_persistentDiscSequencePrefab;
    
    private Neko_SyncComponent m_syncComponent;
    private NekoEnlargeDisc m_enlargeDiscAbility;

    private static readonly Vector3 HIT_POS = new Vector3(1, 1, 1);

    public NekoBoomerangDiscEffect(
        EffectSource parent,
        BoardSquare targetSquare,
        ActorData caster,
        float discReturnEndRadius,
        int returnTripDamage,
        bool returnTripIgnoreCover,
        int extraReturnDamage,
        GameObject returnTripSequencePrefab,
        GameObject persistentDiscSequencePrefab)
        : base(parent, targetSquare, null, caster)
    {
        m_discReturnEndRadius = discReturnEndRadius;
        m_returnTripDamage = returnTripDamage;
        m_returnTripIgnoreCover = returnTripIgnoreCover;
        m_extraReturnDamage = extraReturnDamage;
        m_returnTripSequencePrefab = returnTripSequencePrefab;
        m_persistentDiscSequencePrefab = persistentDiscSequencePrefab;
        m_targetSquare = targetSquare;
        m_time.duration = 2;
        HitPhase = AbilityPriority.Combat_Damage;
    }

    public override void OnStart()
    {
        m_syncComponent = Caster.GetComponent<Neko_SyncComponent>();
        m_enlargeDiscAbility = Caster.GetAbilityData().GetAbilityOfType<NekoEnlargeDisc>();
        m_syncComponent.AddDisk(m_targetSquare);
    }

    public override void OnEnd()
    {
        m_syncComponent.RemoveDisk(m_targetSquare);
    }

    public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
    {
        return new ServerClientUtils.SequenceStartData(
            m_persistentDiscSequencePrefab,
            m_targetSquare.ToVector3(),
            null,
            Caster,
            SequenceSource);
    }

    public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
    {
        List<ServerClientUtils.SequenceStartData> effectHitSeqDataList = base.GetEffectHitSeqDataList();
        float height = Board.Get().LosCheckHeight;
        Vector3 endPos = GetCasterPos();
        endPos.y = height;
        Vector3 startPos = m_targetSquare.ToVector3();
        startPos.y = height;
        
        SequenceSource seqSource = SequenceSource.GetShallowCopy();
        if (GetCasterAnimationIndex(HitPhase) > 0 || AddActorAnimEntryIfHasHits(HitPhase))
        {
            seqSource.SetWaitForClientEnable(true);
        }

        effectHitSeqDataList.Add(new ServerClientUtils.SequenceStartData(
            m_returnTripSequencePrefab,
            endPos,
            m_effectResults.HitActorsArray(),
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
                    setAnimDistParamWithThisProjectile = true,  // TODO NEKO check
                    setAnimParamForNormalDisc = true  // TODO NEKO check. false when dead? or not casting a new disc?
                } // TODO NEKO waitForClientEnable = true??
            }));
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
        float losHeight = Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
        Vector3 startLosPos = m_targetSquare.ToVector3();
        startLosPos.y = losHeight;
        Vector3 endLosPos = GetCasterPos();
        endLosPos.y = losHeight;
        
        // float returnDiskLaserWidth = m_syncComponent.m_discReturnTripLaserWidthInSquares;
        // float aoeStartRadius = m_syncComponent.m_discReturnTripAoeRadiusAtlaserStart;
        // float returnDiskEndRadius = m_discReturnEndRadius;
        // bool isDiscEnlarged = false; // IsDiscAtPosEnlarged(m_boardX[i], m_boardY[i], out bool enlargeDiscUsed); // TODO NEKO embiggify
        // if (isDiscEnlarged)
        // {
        //     returnDiskLaserWidth = m_enlargeDiscAbility.GetLaserWidth();
        //     aoeStartRadius = m_enlargeDiscAbility.GetAoeRadius();
        //     returnDiskEndRadius = Mathf.Max(returnDiskEndRadius, m_enlargeDiscAbility.GetReturnEndAoeRadius());
        // }
        //
        // List<ActorData> hitActors = GetActorsInDiscPath(
        //     startLosPos,
        //     endLosPos,
        //     returnDiskLaserWidth,
        //     aoeStartRadius,
        //     returnDiskEndRadius,
        //     isDiscEnlarged);
        List<ActorData> hitActors = m_syncComponent.ActorsTargetedByReturningDiscs;
        
        foreach (ActorData hitActor in hitActors)
        {
            Vector3 origin = m_returnTripIgnoreCover ? hitActor.GetFreePos() : endLosPos; // TODO NEKO 0,0,0 in vanilla
            ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(hitActor, origin));
            actorHitResults.AddBaseDamage(m_returnTripDamage + m_extraReturnDamage); // TODO NEKO embiggify
            // actorHitResults.SetTechPointGainOnCaster(GetEnergyGainPerMarkedHit());
            // actorHitResults.AddStandardEffectInfo(shapeToHitInfo.m_onExplosionEffect);
            effectResults.StoreActorHit(actorHitResults);
        }
        
        // TODO NEKO caster hit?
        ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(Caster, Caster.GetFreePos()));
        effectResults.StoreActorHit(casterHitResults);
        
        PositionHitParameters positionHitParams = new PositionHitParameters(HIT_POS);
        PositionHitResults positionHitResults = new PositionHitResults(positionHitParams);
        positionHitResults.AddEffectSequenceToEnd(m_persistentDiscSequencePrefab, m_guid);
        effectResults.StorePositionHit(positionHitResults);
    }
    
    // TODO NEKO basically repeats SyncComponent.UpdateActorsInDiscPath
    // private List<ActorData> GetActorsInDiscPath(
    //     Vector3 startLosPos,
    //     Vector3 endLosPos,
    //     float laserWidth,
    //     float aoeStartRadius,
    //     float aoeEndRadius,
    //     bool usingEnlargeDiscAbility)
    // {
    //     List<ActorData> hitActors = new List<ActorData>();
    //     
    //     List<Team> relevantTeams = Caster.GetEnemyTeamAsList();
    //     if (usingEnlargeDiscAbility
    //         && m_enlargeDiscAbility != null
    //         && m_enlargeDiscAbility.CanIncludeAlliesOnReturn())
    //     {
    //         relevantTeams.Add(Caster.GetTeam());
    //     }
    //
    //     Vector3 dir = endLosPos - startLosPos;
    //     dir.y = 0f;
    //     float laserRangeInSquares = dir.magnitude / Board.Get().squareSize;
    //     List<ActorData> actorsInStartRadius = AreaEffectUtils.GetActorsInRadius(
    //         startLosPos, aoeStartRadius, true, Caster, relevantTeams, null);
    //     foreach (ActorData actor in actorsInStartRadius)
    //     {
    //         if (!hitActors.Contains(actor))
    //         {
    //             hitActors.Add(actor);
    //         }
    //     }
    //
    //     if (laserRangeInSquares <= 0f)
    //     {
    //         return hitActors;
    //     }
    //
    //     List<ActorData> actorsInLaser = AreaEffectUtils.GetActorsInLaser(
    //         startLosPos,
    //         dir,
    //         laserRangeInSquares,
    //         laserWidth,
    //         Caster,
    //         relevantTeams,
    //         true,
    //         0,
    //         true,
    //         true,
    //         out _,
    //         null);
    //     foreach (ActorData item in actorsInLaser)
    //     {
    //         if (!hitActors.Contains(item))
    //         {
    //             hitActors.Add(item);
    //         }
    //     }
    //
    //     if (aoeEndRadius > 0f)
    //     {
    //         List<ActorData> actorsInEndRadius = AreaEffectUtils.GetActorsInRadius(
    //             endLosPos, aoeEndRadius, true, Caster, relevantTeams, null);
    //         foreach (ActorData actor in actorsInEndRadius)
    //         {
    //             if (!hitActors.Contains(actor))
    //             {
    //                 hitActors.Add(actor);
    //             }
    //         }
    //     }
    //
    //     return hitActors;
    // }
    
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
            squaresToAvoid.Add(m_targetSquare);
        }
    }

    public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
    {
        return true;
    }
    
    public override int GetCasterAnimationIndex(AbilityPriority phaseIndex)
    {
        return phaseIndex == AbilityPriority.Combat_Damage
            ? 10 // TODO NEKO 10 default, 11 embiggify
            : base.GetCasterAnimationIndex(phaseIndex);
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
