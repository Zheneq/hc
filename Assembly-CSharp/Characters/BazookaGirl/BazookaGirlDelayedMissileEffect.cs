// ROGUES
// SERVER
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if SERVER
// custom
public class BazookaGirlDelayedMissileEffect : Effect
{
    private AbilityAreaShape m_shape;
    private List<BazookaGirlDelayedMissile.ShapeToHitInfo> m_shapeToHitInfo;
    private GameObject m_impactSequencePrefab;
    private GameObject m_markerSequencePrefab;
    private int m_turnsBeforeExploding;
    private BoardSquare m_targetSquare;
    private StandardEffectInfo m_effectOnHit;
    private int m_explosionAnimationIndex;
    
    private HashSet<BoardSquare> m_affectedSquares;
    private List<ActorData> m_targetsOnHitTurnStart;
    
    private BazookaGirl_SyncComponent m_syncComponent;

    public AbilityAreaShape Shape => m_shape;
    public BoardSquare TargetSquare => m_targetSquare;
    public List<BazookaGirlDelayedMissile.ShapeToHitInfo> ShapeToHitInfo => m_shapeToHitInfo;

    public BazookaGirlDelayedMissileEffect(
        EffectSource parent,
        BoardSquare targetSquare,
        ActorData caster,
        AbilityAreaShape shape,
        List<BazookaGirlDelayedMissile.ShapeToHitInfo> shapeToHitInfo,
        int turnsBeforeExploding,
        StandardEffectInfo effectOnHit,
        GameObject markerSequencePrefab,
        GameObject impactSequencePrefab,
        int explosionAnimationIndex)
        : base(parent, targetSquare, null, caster)
    {
        m_shape = shape;
        m_shapeToHitInfo = shapeToHitInfo;
        m_markerSequencePrefab = markerSequencePrefab;
        m_impactSequencePrefab = impactSequencePrefab;
        m_targetSquare = targetSquare;
        m_effectOnHit = effectOnHit;
        m_time.duration = Mathf.Max(turnsBeforeExploding + 1, m_time.duration);
        m_turnsBeforeExploding = turnsBeforeExploding;
        m_explosionAnimationIndex = explosionAnimationIndex;
        HitPhase = AbilityPriority.Combat_Damage;
        m_effectName = "Zuki - Delayed Missile";
        m_affectedSquares = new HashSet<BoardSquare>();
        m_targetsOnHitTurnStart = new List<ActorData>();
    }

    public override void OnStart()
    {
        m_syncComponent = Caster.GetComponent<BazookaGirl_SyncComponent>();
        foreach (BazookaGirlDelayedMissile.ShapeToHitInfo shapeToHitInfo in m_shapeToHitInfo)
        {
            List<BoardSquare> squaresInShape = AreaEffectUtils.GetSquaresInShape(
                shapeToHitInfo.m_shape,
                m_targetSquare.ToVector3(),
                m_targetSquare,
                true,
                Caster);
            m_affectedSquares.UnionWith(squaresInShape);
        }
    }

    public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
    {
        return new ServerClientUtils.SequenceStartData(
            m_markerSequencePrefab,
            m_targetSquare.ToVector3(),
            null,
            Caster,
            SequenceSource);
    }

    public override List<ServerClientUtils.SequenceStartData> GetEffectHitSeqDataList()
    {
        List<ServerClientUtils.SequenceStartData> effectHitSeqDataList = base.GetEffectHitSeqDataList();
        if (m_time.age >= m_turnsBeforeExploding && m_impactSequencePrefab != null) {
            SequenceSource source = SequenceSource.GetShallowCopy();
            if (AddActorAnimEntryIfHasHits(HitPhase))
            {
                source.SetWaitForClientEnable(true);
            }
            effectHitSeqDataList.Add(new ServerClientUtils.SequenceStartData(
                m_impactSequencePrefab,
                m_targetSquare.ToVector3(),
                m_effectResults.HitActorsArray(),
                Caster,
                source, 
                new Sequence.IExtraSequenceParams[]
                {
                    new ExplosionSequence.ExtraParams
                    {
                        radius = 2.5f,
                        team = Caster.GetTeam()
                    }
                }));
        }
        return effectHitSeqDataList;
    }

    public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
    {
        if (m_time.age < m_turnsBeforeExploding)
        {
            return;
        }
        List<ActorData> processedActors = new List<ActorData>();
        foreach (BazookaGirlDelayedMissile.ShapeToHitInfo shapeToHitInfo in m_shapeToHitInfo)
        {
            Log.Info($"Processing shape {shapeToHitInfo.m_shape} at {m_targetSquare.GetGridPos()}  - {shapeToHitInfo.m_damage} damage");
            List<ActorData> actors = AreaEffectUtils.GetActorsInShape(
                shapeToHitInfo.m_shape,
                m_targetSquare.ToVector3(),
                m_targetSquare,
                true,
                Caster,
                Caster.GetOtherTeams(),
                null);
            Log.Info($"{actors.Count} in shape");
            foreach (ActorData actorInShape in actors)
            {
                Log.Info($"Processing {actorInShape.m_displayName} {actorInShape.PlayerIndex}");
                if (processedActors.Contains(actorInShape))
                {
                    Log.Info($"Already processed {actorInShape.m_displayName} {actorInShape.PlayerIndex}");
                    continue;
                }
                if (actorInShape.GetTeam() != Caster.GetTeam())
                {
                    Log.Info($"{actorInShape.m_displayName} {actorInShape.PlayerIndex} is an enemy");
                    ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(actorInShape, m_targetSquare.ToVector3()));
                    actorHitResults.AddBaseDamage(shapeToHitInfo.m_damage);
                    actorHitResults.AddStandardEffectInfo(shapeToHitInfo.m_onExplosionEffect);
                    effectResults.StoreActorHit(actorHitResults);
                    processedActors.Add(actorInShape);
                }
            }
        }

        if (processedActors.Count == 0)
        {
            ActorHitResults actorHitResults = new ActorHitResults(new ActorHitParameters(Caster, m_targetSquare.ToVector3()));
            if (Parent.Ability is BazookaGirlDelayedMissile ability
                && ability.GetCooldownReductionsWhenNoHits() != null
                && ability.GetCooldownReductionsWhenNoHits().HasCooldownReduction())
            {
                ability.GetCooldownReductionsWhenNoHits().AppendCooldownMiscEvents(actorHitResults, true, 0, 0);
            }
            effectResults.StoreActorHit(actorHitResults);
        }
        
        PositionHitParameters positionHitParams = new PositionHitParameters(m_targetSquare.ToVector3());
        PositionHitResults positionHitResults = new PositionHitResults(positionHitParams);
        positionHitResults.AddEffectSequenceToEnd(m_markerSequencePrefab, m_guid);
        effectResults.StorePositionHit(positionHitResults);
    }

    public override void AddToSquaresToAvoidForRespawn(HashSet<BoardSquare> squaresToAvoid, ActorData forActor)
    {
        if (forActor.GetTeam() != Caster.GetTeam() && m_affectedSquares != null)
        {
            squaresToAvoid.UnionWith(m_affectedSquares);
        }
    }
    
    public override int GetCasterAnimationIndex(AbilityPriority phaseIndex)
    {
        Log.Info($"BazookaGirlDelayedMissileEffect::GetCasterAnimationIndex {phaseIndex} (HitPhase={HitPhase}, age={m_time.age})");
        if (phaseIndex == HitPhase && m_time.age >= m_turnsBeforeExploding && !Caster.IsDead())
        {
            return m_explosionAnimationIndex;
        }
        return base.GetCasterAnimationIndex(phaseIndex);
    }

    public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
    {
        return true;
    }

    public override int GetCinematicRequested(AbilityPriority phaseIndex)
    {
        return phaseIndex == HitPhase
               && m_time.age >= m_turnsBeforeExploding
               && m_syncComponent != null
            ? m_syncComponent.m_lastCinematicRequested
            : -1;
    }

    public override void OnTurnStart()
    {
        base.OnTurnStart();
        if (m_time.age < m_turnsBeforeExploding)
        {
            return;
        }
        m_targetsOnHitTurnStart = AreaEffectUtils.GetActorsInShape(
            m_shape,
            m_targetSquare.ToVector3(),
            m_targetSquare,
            true,
            Caster,
            Caster.GetOtherTeams(),
            null);
    }

    public override void OnExecutedEffectResults(EffectResults effectResults)
    {
        base.OnExecutedEffectResults(effectResults);
        int num = m_targetsOnHitTurnStart.Count(ad => !effectResults.StoredHitForActor(ad));
        effectResults.Caster.GetFreelancerStats().AddToValueOfStat(FreelancerStats.BazookaGirlStats.DashesOutOfBigOne, num);
    }
}
#endif
