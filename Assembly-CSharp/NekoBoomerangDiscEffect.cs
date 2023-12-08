// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if SERVER
// custom
public class NekoBoomerangDiscEffect : NekoAbstractDiscEffect
{
    private int m_energyOnMissOnReturnTrip;

    public NekoBoomerangDiscEffect(
        EffectSource parent,
        List<BoardSquare> targetSquares,
        ActorData caster,
        float discReturnEndRadius,
        int returnTripDamage,
        int returnTripSubsequentHitDamage,
        bool returnTripIgnoreCover,
        int energyOnMissOnReturnTrip,
        GameObject returnTripSequencePrefab,
        GameObject persistentDiscSequencePrefab)
        : base(
            parent,
            targetSquares,
            null,
            caster,
            discReturnEndRadius,
            returnTripDamage,
            returnTripSubsequentHitDamage,
            0,
            null,
            returnTripIgnoreCover,
            returnTripSequencePrefab,
            persistentDiscSequencePrefab)
    {
        m_energyOnMissOnReturnTrip = energyOnMissOnReturnTrip;
        m_time.duration = 2;
        HitPhase = AbilityPriority.Combat_Damage;
    }

    public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
    {
        List<ServerClientUtils.SequenceStartData> effectHitSeqDataList = base.GetEffectHitSeqDataList();
        if (m_time.age >= 1)
        {
            Vector3 endPos = GetCasterPos();

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

            List<List<ActorData>> hitActorsPerDisc = GetHitActors(out _, out _, out int enlargedDiscIndex);
            for (int i = 0; i < m_targetSquares.Count; i++)
            {
                BoardSquare targetSquare = m_targetSquares[i];
                Vector3 startPos = targetSquare.GetOccupantLoSPos();
                bool isFarthestDisc = targetSquare == farthestSquare;
                bool isEnlargedDisc = i == enlargedDiscIndex;

                List<ActorData> hitActors = hitActorsPerDisc[i];
                if (isFarthestDisc && !hitActors.Contains(Caster))
                {
                    hitActors.Add(Caster);
                }

                effectHitSeqDataList.Add(new ServerClientUtils.SequenceStartData(
                    isEnlargedDisc
                        ? m_enlargeDiscAbility.m_discReturnOverrideSequencePrefab
                        : m_returnTripSequencePrefab,
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
                            setAnimParamForNormalDisc = SetAnimParamForNormalDisc()
                        }
                    }));
            }

            effectHitSeqDataList.Add(new ServerClientUtils.SequenceStartData(
                SequenceLookup.Get().GetSimpleHitSequencePrefab(),
                HIT_POS,
                new ActorData[] { },
                Caster,
                seqSource));
        }

        return effectHitSeqDataList;
    }

    protected override bool SetAnimParamForNormalDisc()
    {
        return ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Caster, typeof(NekoBoomerangDisc));
    }

    public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
    {
        if (m_time.age < 1)
        {
            return;
        }
        
        List<List<ActorData>> hitActorsPerDisc = GetHitActors(out List<Vector3> startLosPosList, out Vector3 endLosPos, out int enlargedDiscIndex);

        ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(Caster, HIT_POS));
        
        List<ActorData> processedActors = new List<ActorData>();
        if (enlargedDiscIndex >= 0)
        {
            List<ActorData> discHitActors = hitActorsPerDisc[enlargedDiscIndex];
            ProcessHits(effectResults, discHitActors, startLosPosList[enlargedDiscIndex], endLosPos, true);
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
            ProcessHits(effectResults, discHitActors, startLosPosList[i], endLosPos, false);
            processedActors.AddRange(discHitActors);
        }

        if (enlargedDiscIndex >= 0)
        {
            ProcessAdditionalEnlargedEffects(hitActorsPerDisc[enlargedDiscIndex], casterHitResults);
        }

        effectResults.StoreActorHit(casterHitResults);
        
        PositionHitParameters positionHitParams = new PositionHitParameters(HIT_POS);
        PositionHitResults positionHitResults = new PositionHitResults(positionHitParams);
        positionHitResults.AddEffectSequenceToEnd(m_persistentDiscSequencePrefab, m_guid);
        effectResults.StorePositionHit(positionHitResults);
    }

    public override List<ActorData> GetHitActors()
    {
        return GetHitActors(out _, out _, out _).SelectMany(x => x).ToList();
    }

    private List<List<ActorData>> GetHitActors(out List<Vector3> startLosPosList, out Vector3 endLosPos, out int enlargedDiscIndex)
    {
        float losHeight = Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
        endLosPos = GetCasterPos();
        endLosPos.y = losHeight;
        startLosPosList = new List<Vector3>();

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
        for (var i = 0; i < m_targetSquares.Count; i++)
        {
            BoardSquare targetSquare = m_targetSquares[i];
            Vector3 startLosPos = targetSquare.ToVector3();
            startLosPos.y = losHeight;
            startLosPosList.Add(startLosPos);

            float returnDiskLaserWidth = m_syncComponent.m_discReturnTripLaserWidthInSquares;
            float aoeStartRadius = m_syncComponent.m_discReturnTripAoeRadiusAtlaserStart;
            float returnDiskEndRadius = m_discReturnEndRadius;
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

    public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
    {
        return true;
    }
    
    public override int GetCasterAnimationIndex(AbilityPriority phaseIndex)
    {
        if (phaseIndex != AbilityPriority.Combat_Damage || m_time.age < 1 || Caster.IsDead())
        {
            return base.GetCasterAnimationIndex(phaseIndex);
        }
        
        return ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Caster, typeof(NekoEnlargeDisc))
            ? m_syncComponent.m_animIndexForPoweredUpDiscReturn
            : m_syncComponent.m_animIndexForStartOfDiscReturn;
    }

    public override void OnAbilityPhaseEnd(AbilityPriority phase)
    {
        if (m_time.age < 1 || phase != AbilityPriority.Combat_Damage)
        {
            return;
        }
        
        int discsWithEnemyHits = GetHitActors(out _, out _, out _)
            .Count(x => x.Count(ad => ad.GetTeam() != Caster.GetTeam()) > 0);
        Caster.GetFreelancerStats().AddToValueOfStat(
            FreelancerStats.NekoStats.NormalDiscNumDistinctEnemiesHitByReturn,
            discsWithEnemyHits);
    }
}
#endif
