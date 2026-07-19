using System.Collections.Generic;
using UnityEngine;

public class ClientActorHitResults
{
    private bool m_hasDamage;
    private bool m_hasHealing;
    private bool m_hasTechPointGain;
    private bool m_hasTechPointLoss;
    private bool m_hasTechPointGainOnCaster;
    private bool m_hasKnockback;

    private ActorData m_knockbackSourceActor;
    private int m_finalDamage;
    private int m_finalHealing;
    private int m_finalTechPointsGain;
    private int m_finalTechPointsLoss;
    private int m_finalTechPointGainOnCaster;

    private bool m_damageBoosted;
    private bool m_damageReduced;
    private bool m_isPartOfHealOverTime;
    private bool m_updateCasterLastKnownPos;
    private bool m_updateTargetLastKnownPos;
    private bool m_triggerCasterVisOnHitVisualOnly;
    private bool m_updateEffectHolderLastKnownPos;
    private ActorData m_effectHolderActor;
    private bool m_updateOtherLastKnownPos;
    private List<ActorData> m_otherActorsToUpdateVisibility;
    private bool m_targetInCoverWrtDamage;
    private Vector3 m_damageHitOrigin;
    private bool m_canBeReactedTo;
    private bool m_isCharacterSpecificAbility;

    private List<ClientEffectStartData> m_effectsToStart;
    private List<int> m_effectsToRemove;
    private List<ClientBarrierStartData> m_barriersToAdd;
    private List<int> m_barriersToRemove;
    private List<ServerClientUtils.SequenceEndData> m_sequencesToEnd;
    private List<ClientReactionResults> m_reactions;
    private List<int> m_powerupsToRemove;
    private List<ClientPowerupStealData> m_powerupsToSteal;
    private List<ClientMovementResults> m_directPowerupHits;
    private List<ClientGameModeEvent> m_gameModeEvents;
    private List<int> m_overconIds;

    public bool ExecutedHit { get; private set; }
    public bool IsMovementHit { get; set; }
    public bool HasKnockback => m_hasKnockback;
    public ActorData KnockbackSourceActor => m_knockbackSourceActor;

    public ClientActorHitResults(ref IBitStream stream)
    {
        byte bitField1 = 0;
        stream.Serialize(ref bitField1);
        ServerClientUtils.GetBoolsFromBitfield(
            bitField1,
            out m_hasDamage,
            out m_hasHealing,
            out m_hasTechPointGain,
            out m_hasTechPointLoss,
            out m_hasTechPointGainOnCaster,
            out m_hasKnockback,
            out m_targetInCoverWrtDamage,
            out m_canBeReactedTo);

        byte bitField2 = 0;
        stream.Serialize(ref bitField2);
        ServerClientUtils.GetBoolsFromBitfield(
            bitField2,
            out m_damageBoosted,
            out m_damageReduced,
            out m_updateCasterLastKnownPos,
            out m_updateTargetLastKnownPos,
            out m_updateEffectHolderLastKnownPos,
            out m_updateOtherLastKnownPos,
            out m_isPartOfHealOverTime,
            out m_triggerCasterVisOnHitVisualOnly);

        if (m_hasDamage)
        {
            short finalDamage = 0;
            stream.Serialize(ref finalDamage);
            m_finalDamage = finalDamage;
        }

        if (m_hasHealing)
        {
            short finalHealing = 0;
            stream.Serialize(ref finalHealing);
            m_finalHealing = finalHealing;
        }

        if (m_hasTechPointGain)
        {
            short finalTechPointsGain = 0;
            stream.Serialize(ref finalTechPointsGain);
            m_finalTechPointsGain = finalTechPointsGain;
        }

        if (m_hasTechPointLoss)
        {
            short finalTechPointsLoss = 0;
            stream.Serialize(ref finalTechPointsLoss);
            m_finalTechPointsLoss = finalTechPointsLoss;
        }

        if (m_hasTechPointGainOnCaster)
        {
            short finalTechPointGainOnCaster = 0;
            stream.Serialize(ref finalTechPointGainOnCaster);
            m_finalTechPointGainOnCaster = finalTechPointGainOnCaster;
        }

        if (m_hasKnockback)
        {
            short actorIndex = (short)ActorData.s_invalidActorIndex;
            stream.Serialize(ref actorIndex);
            m_knockbackSourceActor = actorIndex != ActorData.s_invalidActorIndex
                ? GameFlowData.Get().FindActorByActorIndex(actorIndex)
                : null;
        }

        if (m_hasDamage && m_targetInCoverWrtDamage || m_hasKnockback)
        {
            float damageHitOriginX = 0f;
            float damageHitOriginZ = 0f;
            stream.Serialize(ref damageHitOriginX);
            stream.Serialize(ref damageHitOriginZ);
            m_damageHitOrigin.x = damageHitOriginX;
            m_damageHitOrigin.y = 0f;
            m_damageHitOrigin.z = damageHitOriginZ;
        }

        if (m_updateEffectHolderLastKnownPos)
        {
            short effectHolderActor = (short)ActorData.s_invalidActorIndex;
            stream.Serialize(ref effectHolderActor);
            m_effectHolderActor = effectHolderActor != ActorData.s_invalidActorIndex
                ? GameFlowData.Get().FindActorByActorIndex(effectHolderActor)
                : null;
        }

        if (m_updateOtherLastKnownPos)
        {
            byte otherActorsToUpdateVisibilityNum = 0;
            stream.Serialize(ref otherActorsToUpdateVisibilityNum);
            m_otherActorsToUpdateVisibility = new List<ActorData>(otherActorsToUpdateVisibilityNum);
            for (int i = 0; i < otherActorsToUpdateVisibilityNum; i++)
            {
                short actorIndex = (short)ActorData.s_invalidActorIndex;
                stream.Serialize(ref actorIndex);
                if (actorIndex != ActorData.s_invalidActorIndex)
                {
                    ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
                    if (actorData != null)
                    {
                        m_otherActorsToUpdateVisibility.Add(actorData);
                    }
                }
            }
        }

        bool hasEffectsToStart = false;
        bool hasEffectsToRemove = false;
        bool hasBarriersToAdd = false;
        bool hasBarriersToRemove = false;
        bool hasSequencesToEnd = false;
        bool hasReactions = false;
        bool hasPowerupsToRemove = false;
        bool hasPowerupsToSteal = false;
        bool hasDirectPowerupHits = false;
        bool hasGameModeEvents = false;
        bool isCharacterSpecificAbility = false;
        bool hasOverconIds = false;
        byte bitField3 = 0;
        byte bitField4 = 0;
        stream.Serialize(ref bitField3);
        stream.Serialize(ref bitField4);
        ServerClientUtils.GetBoolsFromBitfield(
            bitField3,
            out hasEffectsToStart,
            out hasEffectsToRemove,
            out hasBarriersToRemove,
            out hasSequencesToEnd,
            out hasReactions,
            out hasPowerupsToRemove,
            out hasPowerupsToSteal,
            out hasDirectPowerupHits);
        ServerClientUtils.GetBoolsFromBitfield(
            bitField4,
            out hasGameModeEvents,
            out isCharacterSpecificAbility,
            out hasBarriersToAdd,
            out hasOverconIds);

        m_effectsToStart = hasEffectsToStart
            ? AbilityResultsUtils.DeSerializeEffectsToStartFromStream(ref stream)
            : new List<ClientEffectStartData>();
        m_effectsToRemove = hasEffectsToRemove
            ? AbilityResultsUtils.DeSerializeEffectsForRemovalFromStream(ref stream)
            : new List<int>();
        m_barriersToAdd = hasBarriersToAdd
            ? AbilityResultsUtils.DeSerializeBarriersToStartFromStream(ref stream)
            : new List<ClientBarrierStartData>();
        m_barriersToRemove = hasBarriersToRemove
            ? AbilityResultsUtils.DeSerializeBarriersForRemovalFromStream(ref stream)
            : new List<int>();
        m_sequencesToEnd = hasSequencesToEnd
            ? AbilityResultsUtils.DeSerializeSequenceEndDataListFromStream(ref stream)
            : new List<ServerClientUtils.SequenceEndData>();
        m_reactions = hasReactions
            ? AbilityResultsUtils.DeSerializeClientReactionResultsFromStream(ref stream)
            : new List<ClientReactionResults>();
        m_powerupsToRemove = hasPowerupsToRemove
            ? AbilityResultsUtils.DeSerializePowerupsToRemoveFromStream(ref stream)
            : new List<int>();
        m_powerupsToSteal = hasPowerupsToSteal
            ? AbilityResultsUtils.DeSerializePowerupsToStealFromStream(ref stream)
            : new List<ClientPowerupStealData>();
        m_directPowerupHits = hasDirectPowerupHits
            ? AbilityResultsUtils.DeSerializeClientMovementResultsListFromStream(ref stream)
            : new List<ClientMovementResults>();
        m_gameModeEvents = hasGameModeEvents
            ? AbilityResultsUtils.DeSerializeClientGameModeEventListFromStream(ref stream)
            : new List<ClientGameModeEvent>();
        m_overconIds = hasOverconIds
            ? AbilityResultsUtils.DeSerializeClientOverconListFromStream(ref stream)
            : new List<int>();
        m_isCharacterSpecificAbility = isCharacterSpecificAbility;

        IsMovementHit = false;
        ExecutedHit = false;
    }

    public bool HasUnexecutedReactionOnActor(ActorData actor)
    {
        foreach (ClientReactionResults reaction in m_reactions)
        {
            if (reaction.HasUnexecutedReactionOnActor(actor))
            {
                return true;
            }
        }

        return false;
    }

    public bool HasUnexecutedReactionHits()
    {
        foreach (ClientReactionResults reaction in m_reactions)
        {
            if (!reaction.ReactionHitsDone())
            {
                return true;
            }
        }

        return false;
    }

    public bool HasReactionHitByCaster(ActorData caster)
    {
        foreach (ClientReactionResults reaction in m_reactions)
        {
            if (reaction.GetCaster() == caster)
            {
                return true;
            }
        }

        return false;
    }

    public void GetReactionHitResultsByCaster(
        ActorData caster,
        out Dictionary<ActorData, ClientActorHitResults> actorHits,
        out Dictionary<Vector3, ClientPositionHitResults> posHits)
    {
        actorHits = null;
        posHits = null;
        foreach (ClientReactionResults reaction in m_reactions)
        {
            if (reaction.GetCaster() == caster)
            {
                actorHits = reaction.GetActorHitResults();
                posHits = reaction.GetPosHitResults();
                return;
            }
        }
    }

    public void ExecuteReactionHitsWithExtraFlagsOnActor(
        ActorData targetActor,
        ActorData caster,
        bool hasDamage,
        bool hasHealing)
    {
        foreach (ClientReactionResults clientReactionResults in m_reactions)
        {
            byte extraFlags = clientReactionResults.GetExtraFlags();
            if (clientReactionResults.PlayedReaction())
            {
                continue;
            }

            if ((extraFlags & (byte)ClientReactionResults.ExtraFlags.ClientExecuteOnFirstDamagingHit) != 0
                && hasDamage
                && clientReactionResults.HasUnexecutedReactionOnActor(targetActor)
                || (extraFlags & (byte)ClientReactionResults.ExtraFlags.TriggerOnFirstDamageIfReactOnAttacker) != 0
                && hasDamage
                && clientReactionResults.HasUnexecutedReactionOnActor(caster)
                || (extraFlags & (byte)ClientReactionResults.ExtraFlags.TriggerOnFirstDamageOnReactionCaster) != 0
                && hasDamage
                && clientReactionResults.GetCaster() == targetActor)
            {
                if (ClientAbilityResults.DebugTraceOn)
                {
                    Log.Warning(
                        ClientAbilityResults.s_clientHitResultHeader
                        + clientReactionResults.GetDebugDescription()
                        + " executing reaction hit on first damaging hit");
                }

                clientReactionResults.PlayReaction();
            }
        }
    }

    public void ExecuteActorHit(ActorData target, ActorData caster)
    {
        if (ExecutedHit)
        {
            return;
        }

        if (ClientAbilityResults.DebugTraceOn)
        {
            Debug.LogWarning(
                ClientAbilityResults.s_executeActorHitHeader
                + " Target: " + target.DebugNameString()
                + " Caster: " + caster.DebugNameString());
        }

        bool isInResolutionState = ClientResolutionManager.Get().IsInResolutionState();
        if (m_triggerCasterVisOnHitVisualOnly && !m_updateCasterLastKnownPos)
        {
            caster.TriggerVisibilityForHit(IsMovementHit, false);
        }

        if (m_updateCasterLastKnownPos)
        {
            caster.TriggerVisibilityForHit(IsMovementHit);
        }

        if (m_updateTargetLastKnownPos)
        {
            target.TriggerVisibilityForHit(IsMovementHit);
        }

        if (m_updateEffectHolderLastKnownPos && m_effectHolderActor != null)
        {
            m_effectHolderActor.TriggerVisibilityForHit(IsMovementHit);
        }

        if (m_updateOtherLastKnownPos && m_otherActorsToUpdateVisibility != null)
        {
            foreach (ActorData actor in m_otherActorsToUpdateVisibility)
            {
                actor.TriggerVisibilityForHit(IsMovementHit);
            }
        }

        if (m_hasDamage)
        {
            if (isInResolutionState)
            {
                target.ClientUnresolvedDamage += m_finalDamage;
                CaptureTheFlag.OnActorDamaged_Client(target, m_finalDamage);
            }

            target.AddCombatText(
                m_finalDamage + (m_targetInCoverWrtDamage ? "|C" : "|N"),
                string.Empty,
                CombatTextCategory.Damage,
                m_damageBoosted
                    ? BuffIconToDisplay.BoostedDamage
                    : m_damageReduced
                        ? BuffIconToDisplay.ReducedDamage
                        : BuffIconToDisplay.None);
            if (m_targetInCoverWrtDamage)
            {
                target.OnHitWhileInCover(m_damageHitOrigin, caster);
            }

            if (target.GetActorBehavior() != null)
            {
                target.GetActorBehavior().Client_RecordDamageFromActor(caster);
            }

            GameEventManager.Get().FireEvent(
                GameEventManager.EventType.ActorDamaged_Client,
                new GameEventManager.ActorHitHealthChangeArgs(
                    GameEventManager.ActorHitHealthChangeArgs.ChangeType.Damage,
                    m_finalDamage,
                    target,
                    caster,
                    m_isCharacterSpecificAbility));
        }

        if (m_hasHealing)
        {
            if (isInResolutionState)
            {
                target.ClientUnresolvedHealing += m_finalHealing;
                if (m_isPartOfHealOverTime)
                {
                    target.ClientAppliedHoTThisTurn += m_finalHealing;
                }
            }

            target.AddCombatText(
                m_finalHealing.ToString(),
                string.Empty,
                CombatTextCategory.Healing,
                BuffIconToDisplay.None);
            if (target.GetActorBehavior() != null)
            {
                target.GetActorBehavior().Client_RecordHealingFromActor(caster);
            }

            GameEventManager.CharacterHealBuffArgs characterHealBuffArgs = new GameEventManager.CharacterHealBuffArgs
            {
                targetCharacter = target,
                casterActor = caster,
                healed = true
            };
            GameEventManager.Get().FireEvent(GameEventManager.EventType.CharacterHealedOrBuffed, characterHealBuffArgs);
            GameEventManager.Get().FireEvent(
                GameEventManager.EventType.ActorHealed_Client,
                new GameEventManager.ActorHitHealthChangeArgs(
                    GameEventManager.ActorHitHealthChangeArgs.ChangeType.Healing,
                    m_finalHealing,
                    target,
                    caster,
                    m_isCharacterSpecificAbility));
        }

        if (m_hasTechPointGain)
        {
            if (isInResolutionState)
            {
                target.ClientUnresolvedTechPointGain += m_finalTechPointsGain;
            }

            target.AddCombatText(
                m_finalTechPointsGain.ToString(),
                string.Empty,
                CombatTextCategory.TP_Recovery,
                BuffIconToDisplay.None);
        }

        if (m_hasTechPointLoss)
        {
            if (isInResolutionState)
            {
                target.ClientUnresolvedTechPointLoss += m_finalTechPointsLoss;
            }

            target.AddCombatText(
                m_finalTechPointsLoss.ToString(),
                string.Empty,
                CombatTextCategory.TP_Damage,
                BuffIconToDisplay.None);
        }

        if (m_hasTechPointGainOnCaster)
        {
            if (isInResolutionState)
            {
                caster.ClientUnresolvedTechPointGain += m_finalTechPointGainOnCaster;
            }

            caster.AddCombatText(
                m_finalTechPointGainOnCaster.ToString(),
                string.Empty,
                CombatTextCategory.TP_Recovery,
                BuffIconToDisplay.None);
        }

        if (m_hasKnockback)
        {
            ClientKnockbackManager.Get().OnKnockbackHit(m_knockbackSourceActor, target);
            if (caster != target
                && target.GetActorStatus() != null
                && target.GetActorStatus().IsKnockbackImmune())
            {
                target.OnKnockbackWhileUnstoppable(m_damageHitOrigin, caster);
            }
        }

        int absorb = 0;
        foreach (ClientEffectStartData effectToStart in m_effectsToStart)
        {
            absorb += effectToStart.m_absorb;
            ClientEffectBarrierManager.Get().ExecuteEffectStart(effectToStart);
        }

        if (absorb > 0)
        {
            target.AddCombatText(absorb.ToString(), string.Empty, CombatTextCategory.Absorb, BuffIconToDisplay.None);
            GameEventManager.Get().FireEvent(
                GameEventManager.EventType.ActorGainedAbsorb_Client,
                new GameEventManager.ActorHitHealthChangeArgs(
                    GameEventManager.ActorHitHealthChangeArgs.ChangeType.Absorb,
                    absorb,
                    target,
                    caster,
                    m_isCharacterSpecificAbility));
        }

        foreach (int effectToRemove in m_effectsToRemove)
        {
            ClientEffectBarrierManager.Get().EndEffect(effectToRemove);
        }

        foreach (ClientBarrierStartData barrierToEnd in m_barriersToAdd)
        {
            ClientEffectBarrierManager.Get().ExecuteBarrierStart(barrierToEnd);
        }

        foreach (int barrierToRemove in m_barriersToRemove)
        {
            ClientEffectBarrierManager.Get().EndBarrier(barrierToRemove);
        }

        foreach (ServerClientUtils.SequenceEndData sequenceToEnd in m_sequencesToEnd)
        {
            sequenceToEnd.EndClientSequences();
        }

        foreach (ClientReactionResults reaction in m_reactions)
        {
            reaction.PlayReaction();
        }

        foreach (int powerupToRemove in m_powerupsToRemove)
        {
            PowerUp powerUp = PowerUpManager.Get().GetPowerUpOfGuid(powerupToRemove);
            if (powerUp != null)
            {
                powerUp.Client_OnPickedUp(target.ActorIndex);
            }
        }

        foreach (ClientPowerupStealData powerupToSteal in m_powerupsToSteal)
        {
            powerupToSteal.m_powerupResults.RunResults();
            PowerUp powerUp = PowerUpManager.Get().GetPowerUpOfGuid(powerupToSteal.m_powerupGuid);
            if (powerUp != null)
            {
                powerUp.Client_OnSteal(target.ActorIndex);
            }
        }

        foreach (ClientMovementResults directPowerupHit in m_directPowerupHits)
        {
            directPowerupHit.ReactToMovement();
        }

        foreach (ClientGameModeEvent gameModeEvent in m_gameModeEvents)
        {
            gameModeEvent.ExecuteClientGameModeEvent();
        }

        foreach (int overcon in m_overconIds)
        {
            if (UIOverconData.Get() != null)
            {
                UIOverconData.Get().UseOvercon(overcon, caster.ActorIndex, true);
            }
        }

        ExecutedHit = true;
        ClientResolutionManager.Get().UpdateLastEventTime();
        ClientResolutionManager.Get().OnHitExecutedOnActor(target, caster, m_hasDamage, m_hasHealing, m_canBeReactedTo);
    }

    public void ShowDamage(ActorData target)
    {
        target.ShowDamage(string.Empty);
    }

    public int GetNumEffectsToStart()
    {
        return m_effectsToStart?.Count ?? 0;
    }

    public void SwapEffectsToStart(ClientActorHitResults other)
    {
        List<ClientEffectStartData> effectsToStart = m_effectsToStart;
        m_effectsToStart = other.m_effectsToStart;
        other.m_effectsToStart = effectsToStart;
    }
}