// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine;

#if SERVER
// custom
public class FireborgReactLasersEffect : Effect
{
    private readonly Vector3 m_aimDirection;
    private readonly float m_laserRange;
    private readonly float m_laserWidth;
    private readonly bool m_ignoreLos;

    private readonly OnHitAuthoredData m_onHitDataForFirstLaser;
    private readonly OnHitAuthoredData m_onHitDataForSecondLaser;
    private readonly int m_shieldPerHitReceivedForNextTurn;
    private readonly FireborgReactLasers.HitEffectApplySetting m_ignitedApplySetting;
    private readonly FireborgReactLasers.HitEffectApplySetting m_groundFireApplySetting;
    private readonly int m_mainLaserAnimationIndex;
    private readonly GameObject m_persistentSeqPrefab; // TODO FIREBORG unused, null
    private readonly GameObject m_onTriggerSeqPrefab;
    private readonly GameObject m_reactionAnimTriggerSeqPrefab;
    private readonly float m_onTriggerProjectileSeqStartDelay;

    private readonly Fireborg_SyncComponent m_syncComp;

    private bool m_doneHittingFirst;
    private bool m_doneHittingFirst_Fake;
    private bool m_doneHittingReaction;
    private bool m_doneHittingReaction_Fake;
    private bool m_hitsCanBeReactedTo;

    public FireborgReactLasersEffect(
        EffectSource parent,
        BoardSquare targetSquare,
        ActorData target,
        ActorData caster,
        Vector3 aimDirection,
        float laserRange,
        float laserWidth,
        bool ignoreLos,
        OnHitAuthoredData onHitDataForFirstLaser,
        OnHitAuthoredData onHitDataForSecondLaser,
        int shieldPerHitReceivedForNextTurn,
        FireborgReactLasers.HitEffectApplySetting ignitedApplySetting,
        FireborgReactLasers.HitEffectApplySetting groundFireApplySetting,
        int mainLaserAnimationIndex,
        GameObject persistentSeqPrefab,
        GameObject onTriggerSeqPrefab,
        GameObject reactionAnimTriggerSeqPrefab,
        float onTriggerProjectileSeqStartDelay,
        Fireborg_SyncComponent syncComp)
        : base(parent, targetSquare, target, caster)
    {
        m_aimDirection = aimDirection;
        m_laserRange = laserRange;
        m_laserWidth = laserWidth;
        m_ignoreLos = ignoreLos;
        m_onHitDataForFirstLaser = onHitDataForFirstLaser;
        m_onHitDataForSecondLaser = onHitDataForSecondLaser;
        m_shieldPerHitReceivedForNextTurn = shieldPerHitReceivedForNextTurn;
        m_ignitedApplySetting = ignitedApplySetting;
        m_groundFireApplySetting = groundFireApplySetting;
        m_mainLaserAnimationIndex = mainLaserAnimationIndex;
        m_persistentSeqPrefab = persistentSeqPrefab;
        m_onTriggerSeqPrefab = onTriggerSeqPrefab;
        m_reactionAnimTriggerSeqPrefab = reactionAnimTriggerSeqPrefab;
        m_onTriggerProjectileSeqStartDelay = onTriggerProjectileSeqStartDelay;
        m_syncComp = syncComp;
        HitPhase = AbilityPriority.Combat_Damage;
        m_time.duration = 1;
    }

    public override int GetCasterAnimationIndex(AbilityPriority phaseIndex)
    {
        return phaseIndex == HitPhase
            ? m_mainLaserAnimationIndex
            : base.GetCasterAnimationIndex(phaseIndex);
    }

    public override bool HitsCanBeReactedTo()
    {
        return m_hitsCanBeReactedTo;
    }

    public override ServerClientUtils.SequenceStartData GetEffectStartSeqData()
    {
        // m_persistentSeqPrefab is null anyway
        return base.GetEffectStartSeqData();
    }

    public override bool AddActorAnimEntryIfHasHits(AbilityPriority phaseIndex)
    {
        return HitPhase == phaseIndex;
    }

    public override ServerClientUtils.SequenceStartData GetEffectHitSeqData()
    {
        SequenceSource sequenceSource = SequenceSource.GetShallowCopy();
        if (AddActorAnimEntryIfHasHits(HitPhase))
        {
            sequenceSource.SetWaitForClientEnable(true);
        }

        GetHitActors(null, out Vector3 endPos);
        return new ServerClientUtils.SequenceStartData(
            m_onTriggerSeqPrefab,
            endPos,
            m_effectResults.HitActorsArray(),
            Target,
            sequenceSource);
    }

    public override void GatherEffectResults(ref EffectResults effectResults, bool isReal)
    {
        m_hitsCanBeReactedTo = true;
        GatherResults(
            isReal,
            out List<ActorHitResults> actorHitResultsList,
            out PositionHitResults posHitResults,
            out List<NonActorTargetInfo> nonActorTargetInfo,
            out _);

        if (posHitResults != null)
        {
            effectResults.StorePositionHit(posHitResults);
        }

        foreach (ActorHitResults actorHitResults in actorHitResultsList)
        {
            effectResults.StoreActorHit(actorHitResults);
        }
        
        effectResults.StoreNonActorTargetInfo(nonActorTargetInfo);
        m_hitsCanBeReactedTo = false;
    }

    public override void GatherResultsInResponseToActorHit(
        ActorHitResults incomingHit,
        ref List<AbilityResults_Reaction> reactions,
        bool isReal)
    {
        bool doneHittingReaction = isReal ? m_doneHittingReaction : m_doneHittingReaction_Fake;
        if (!incomingHit.HasDamage)
        {
            return;
        }

        if (isReal)
        {
            m_syncComp.AddPendingShield(m_shieldPerHitReceivedForNextTurn);
        }

        if (doneHittingReaction)
        {
            return;
        }

        AbilityResults_Reaction_PositionHitSequence abilityResults_Reaction =
            new AbilityResults_Reaction_PositionHitSequence();

        GatherResults(
            isReal,
            out List<ActorHitResults> actorHitResultsList,
            out PositionHitResults posHitResults,
            out List<NonActorTargetInfo> _, // reactions can't have non-actor hits
            out Vector3 endPos);

        foreach (ActorHitResults actorHitResults in actorHitResultsList)
        {
            actorHitResults.IsReaction = true;
            actorHitResults.TriggeringHit = incomingHit;
        }

        abilityResults_Reaction.SetupGameplayData(
            this,
            actorHitResultsList,
            incomingHit.m_reactionDepth,
            isReal);

        if (posHitResults != null)
        {
            abilityResults_Reaction.GetReactionResults().StorePositionHit(posHitResults);
        }

        abilityResults_Reaction.SetupSequenceData(
            m_onTriggerSeqPrefab,
            endPos,
            SequenceSource,
            new Sequence.IExtraSequenceParams[]
            {
                new SimpleVFXAtTargetPosSequence.IgnoreStartEventExtraParam
                {
                    ignoreStartEvent = true
                },
                new SplineProjectileSequence.DelayedProjectileExtraParams
                {
                    startDelay = m_onTriggerProjectileSeqStartDelay
                }
            });
        abilityResults_Reaction.SetExtraFlag(ClientReactionResults.ExtraFlags.TriggerOnFirstDamageOnReactionCaster);
        reactions.Add(abilityResults_Reaction);

        AbilityResults_Reaction abilityResults_CasterAnimation = new AbilityResults_Reaction();
        ActorHitParameters casterHitParameters = new ActorHitParameters(Target, Target.GetFreePos());
        ActorHitResults casterHitResults = new ActorHitResults(casterHitParameters);
        abilityResults_CasterAnimation.SetupGameplayData(
            this,
            casterHitResults,
            incomingHit.m_reactionDepth,
            null,
            isReal,
            incomingHit);
        abilityResults_CasterAnimation.SetupSequenceData(
            m_reactionAnimTriggerSeqPrefab,
            Target.GetCurrentBoardSquare(),
            SequenceSource);
        abilityResults_CasterAnimation.SetExtraFlag(ClientReactionResults.ExtraFlags.TriggerOnFirstDamageOnReactionCaster);
        reactions.Add(abilityResults_CasterAnimation);
        
        if (isReal)
        {
            m_doneHittingReaction = true;
        }
        else
        {
            m_doneHittingReaction_Fake = true;
        }
    }

    private void GatherResults(
        bool isReal,
        out List<ActorHitResults> actorHitResultsList,
        out PositionHitResults posHitResults,
        out List<NonActorTargetInfo> nonActorTargetInfo,
        out Vector3 endPos)
    {
        bool isFirst = isReal ? !m_doneHittingFirst : !m_doneHittingFirst_Fake;

        bool isSuperheated = m_syncComp.InSuperheatMode();
        bool applyGroundFire = m_groundFireApplySetting.ShouldApply(isFirst, isSuperheated);
        bool applyIgnited = m_ignitedApplySetting.ShouldApply(isFirst, isSuperheated);

        nonActorTargetInfo = new List<NonActorTargetInfo>();
        List<ActorData> hitActors = GetHitActors(nonActorTargetInfo, out endPos);
        actorHitResultsList = new List<ActorHitResults>(hitActors.Count);
        posHitResults = null;

        if (applyGroundFire)
        {
            Vector3 posForHit = Caster.GetLoSCheckPos();
            VectorUtils.GetAdjustedStartPosWithOffset(
                posForHit,
                endPos,
                GameWideData.Get().m_actorTargetingRadiusInSquares);
            posHitResults =
                m_syncComp
                    .MakeGroundFireEffectResults(
                        Parent.Ability,
                        Caster,
                        GetHitSquares(endPos),
                        endPos,
                        1,
                        isReal,
                        out FireborgGroundFireEffect groundFireEffect,
                        out List<FireborgGroundFireEffect> oldEffectsForRemoval);
            posHitResults.AddEffect(groundFireEffect);
            oldEffectsForRemoval.ForEach(posHitResults.AddEffectForRemoval);
        }

        OnHitAuthoredData onHitData = isFirst ? m_onHitDataForFirstLaser : m_onHitDataForSecondLaser;
        foreach (ActorData hitActor in hitActors)
        {
            ActorHitParameters hitParameters = new ActorHitParameters(hitActor, Target.GetFreePos());
            ActorHitResults actorHitResults = new ActorHitResults(hitParameters);
            GenericAbility_Container.ApplyActorHitData(Caster, hitActor, actorHitResults, onHitData);
            if (applyIgnited)
            {
                FireborgIgnitedEffect fireborgIgnitedEffect = m_syncComp.MakeIgnitedEffect(Parent, Caster, hitActor);
                if (fireborgIgnitedEffect != null)
                {
                    actorHitResults.AddEffect(fireborgIgnitedEffect);
                }
            }

            actorHitResultsList.Add(actorHitResults);
        }
        
        if (isReal)
        {
            m_doneHittingFirst = true;
        }
        else
        {
            m_doneHittingFirst_Fake = true;
        }
    }

    public List<ActorData> GetHitActors(List<NonActorTargetInfo> nonActorTargetInfo, out Vector3 endPos)
    {
        return AreaEffectUtils.GetActorsInLaser(
            Caster.GetLoSCheckPos(),
            m_aimDirection,
            m_laserRange,
            m_laserWidth,
            Caster,
            Caster.GetOtherTeams(),
            m_ignoreLos,
            0,
            false,
            true,
            out endPos,
            nonActorTargetInfo);
    }

    private List<BoardSquare> GetHitSquares(Vector3 endPos)
    {
        return AreaEffectUtils.GetSquaresInBoxByActorRadius(
            Caster.GetLoSCheckPos(),
            endPos,
            m_laserWidth,
            m_ignoreLos,
            Caster);
    }

    public int GetBaseDamage()
    {
        return m_onHitDataForFirstLaser.GetFirstDamageValue();
    }
}
#endif