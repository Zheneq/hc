// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if SERVER
// custom
public class NekoHomingDiscEffect : NekoAbstractDiscEffect
{
    private ActorData m_homingTarget;
    private int m_noHitsCdr;
    private Vector3 m_discEndPos;
    
    public NekoHomingDiscEffect(
        EffectSource parent,
        BoardSquare targetSquare,
        ActorData target,
        ActorData caster,
        float discReturnEndRadius,
        int returnTripDamage,
        float returnTripExtraDamagePerDist,
        StandardEffectInfo returnTripEnemyEffect,
        int noHitsCdr,
        bool returnTripIgnoreCover,
        GameObject returnTripSequencePrefab,
        GameObject persistentDiscSequencePrefab)
        : base(
            parent,
            new List<BoardSquare>{targetSquare},
            null,
            caster,
            discReturnEndRadius,
            returnTripDamage,
            returnTripDamage,
            returnTripExtraDamagePerDist,
            returnTripEnemyEffect,
            returnTripIgnoreCover,
            returnTripSequencePrefab,
            persistentDiscSequencePrefab)
    {
        m_homingTarget = target;
        m_noHitsCdr = noHitsCdr;
    }

    public override void OnAbilityPhaseStart(AbilityPriority phase)
    {
        if (phase == AbilityPriority.Prep_Defense)
        {
            m_discEndPos = m_syncComponent.GetHomingActorPos();
            Log.Info($"HOMING DISC on {m_homingTarget} to {m_discEndPos}");
        }
    }

    public override void OnStart()
    {
        base.OnStart();
        
        Log.Info($"HOMING DISC on {m_homingTarget} from {m_homingTarget.GetCurrentBoardSquare().ToVector3()}");
        m_syncComponent.Networkm_homingActorIndex = m_homingTarget != null
            ? m_homingTarget.ActorIndex
            : ActorData.s_invalidActorIndex;
    }

    public override void OnEnd()
    {
        base.OnEnd();
        m_syncComponent.Networkm_homingActorIndex = ActorData.s_invalidActorIndex;
    }

    public override List<ServerClientUtils.SequenceStartData> GetEffectStartSeqDataList()
    {
        // TODO NEKO CHECK anim, it lags sometimes
        return m_targetSquares
            .Select(targetSquare => new ServerClientUtils.SequenceStartData(
                m_persistentDiscSequencePrefab,
                targetSquare.ToVector3(),
                m_homingTarget.AsArray(),
                Caster,
                SequenceSource))
            .ToList();
    }

    public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
    {
        List<ServerClientUtils.SequenceStartData> effectHitSeqDataList = base.GetEffectHitSeqDataList();
        if (m_time.age >= 1)
        {
            float height = Board.Get().LosCheckHeight;
            Vector3 endPos = m_discEndPos;
            endPos.y = height;
        
            SequenceSource seqSource = SequenceSource.GetShallowCopy();
            if (GetCasterAnimationIndex(HitPhase) > 0 || AddActorAnimEntryIfHasHits(HitPhase))
            {
                seqSource.SetWaitForClientEnable(true);
            }
        
            List<ActorData> hitActors = GetHitActors();
            BoardSquare targetSquare = m_targetSquares[0];
            Vector3 startPos = targetSquare.ToVector3();
            startPos.y = height;
            bool isEnlargedDisc = IsDiscEnlarged();

            if (!hitActors.Contains(Caster))
            {
                hitActors.Add(Caster);
            }

            effectHitSeqDataList.Add(new ServerClientUtils.SequenceStartData(
                isEnlargedDisc ? m_enlargeDiscAbility.m_prepDiscReturnOverrideSequencePrefab : m_returnTripSequencePrefab,
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
                        setAnimDistParamWithThisProjectile = true,
                        setAnimParamForNormalDisc = true // TODO NEKO CHECK false when dead? or not casting a new disc? false when embiggify?
                    } // TODO NEKO CHECK waitForClientEnable = true??
                }));

            effectHitSeqDataList.Add(new ServerClientUtils.SequenceStartData(
                SequenceLookup.Get().GetSimpleHitSequencePrefab(),
                HIT_POS,
                new ActorData[] {},
                Caster,
                seqSource));
        }
        
        return effectHitSeqDataList;
    }

    public override List<ActorData> GetHitActors()
    {
        float losHeight = Board.Get().BaselineHeight + BoardSquare.s_LoSHeightOffset;
        bool isDiscEnlarged = IsDiscEnlarged();
        
        Vector3 startLosPos = Neko_SyncComponent.HomingDiscStartFromCaster() ? GetCasterPos() : m_targetSquares[0].ToVector3();
        startLosPos.y = losHeight;
        Vector3 endLosPos = m_discEndPos;
        endLosPos.y = losHeight;
            
        float returnDiskLaserWidth = m_syncComponent.m_discReturnTripLaserWidthInSquares;
        float aoeStartRadius = m_syncComponent.m_discReturnTripAoeRadiusAtlaserStart;
        float returnDiskEndRadius = m_discReturnEndRadius;
        if (isDiscEnlarged)
        {
            returnDiskLaserWidth = m_enlargeDiscAbility.GetLaserWidth();
            aoeStartRadius = m_enlargeDiscAbility.GetAoeRadius();
            returnDiskEndRadius = Mathf.Max(returnDiskEndRadius, m_enlargeDiscAbility.GetReturnEndAoeRadius());
        }

        return GetActorsInDiscPath(
            startLosPos,
            endLosPos,
            returnDiskLaserWidth,
            aoeStartRadius,
            returnDiskEndRadius,
            isDiscEnlarged);
    }

    private bool IsDiscEnlarged()
    {
        return ServerActionBuffer.Get().HasStoredAbilityRequestOfType(Caster, typeof(NekoEnlargeDisc));
    }

    public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
    {
        if (m_time.age < 1)
        {
            return;
        }
        List<ActorData> hitActors = GetHitActors();

        bool isDiscEnlarged = IsDiscEnlarged();

        ProcessHits(effectResults, hitActors, m_discEndPos, m_targetSquares[0].GetOccupantLoSPos(), isDiscEnlarged);
        
        ActorHitResults casterHitResults = new ActorHitResults(new ActorHitParameters(Caster, Caster.GetFreePos()));
        if (isDiscEnlarged)
        {
            ProcessAdditionalEnlargedEffects(hitActors, casterHitResults);
        }

        if (m_noHitsCdr > 0 && hitActors.All(ha => ha.GetTeam() == Caster.GetTeam()))
        {
            casterHitResults.AddMiscHitEvent(new MiscHitEventData_AddToCasterCooldown(
                AbilityData.ActionType.ABILITY_2, -m_noHitsCdr));
        }
        effectResults.StoreActorHit(casterHitResults);
        
        PositionHitParameters positionHitParams = new PositionHitParameters(HIT_POS);
        PositionHitResults positionHitResults = new PositionHitResults(positionHitParams);
        positionHitResults.AddEffectSequenceToEnd(m_persistentDiscSequencePrefab, m_guid);
        effectResults.StorePositionHit(positionHitResults);
    }

    public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
    {
        return true;
    }
}
#endif