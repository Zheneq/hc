using System.Collections.Generic;
using UnityEngine;

public class ClientActionBuffer : MonoBehaviour
{
    private static ClientActionBuffer s_instance;

    private ActionBufferPhase m_actionPhase;
    private AbilityPriority m_abilityPhase;
    private bool m_ignoreOldAbilityPhase;

    public ActionBufferPhase CurrentActionPhase
    {
        get => m_actionPhase;
        private set
        {
            if (m_actionPhase != value)
            {
                SetActionPhase(value);
            }
        }
    }

    public AbilityPriority AbilityPhase
    {
        get => m_abilityPhase;
        private set
        {
            if (m_abilityPhase == value)
            {
                return;
            }

            SetAbilityPhase(value);
            SequenceManager.Get().OnAbilityPhaseStart(m_abilityPhase);
            ClientResolutionManager.Get().OnAbilityPhaseStart(m_abilityPhase);
            foreach (ActorData actor in GameFlowData.Get().GetActors())
            {
                if (actor != null)
                {
                    actor.CurrentlyVisibleForAbilityCast = false;
                    actor.MovedForEvade = false;
                }
            }

            if (ClientAbilityResults.DebugTraceOn)
            {
                Log.Warning(
                    "On Ability Phase Start: <color=magenta>" + m_abilityPhase + "</color>\n@time = " + Time.time);
            }
        }
    }

    public static ClientActionBuffer Get()
    {
        return s_instance;
    }

    private void Awake()
    {
        s_instance = this;
        m_ignoreOldAbilityPhase = true;
        m_actionPhase = ActionBufferPhase.Done;
    }

    private void OnDestroy()
    {
        s_instance = null;
    }

    private void SetActionPhase(ActionBufferPhase nextActionPhase)
    {
        ActionBufferPhase actionPhase = m_actionPhase;
        m_actionPhase = nextActionPhase;
        if ((actionPhase == ActionBufferPhase.Abilities
             || actionPhase == ActionBufferPhase.AbilitiesWait
             || actionPhase == ActionBufferPhase.Done)
            && (nextActionPhase == ActionBufferPhase.Movement
                || nextActionPhase == ActionBufferPhase.MovementChase
                || nextActionPhase == ActionBufferPhase.MovementWait))
        {
            GameEventManager.Get().FireEvent(GameEventManager.EventType.UIPhaseStartedMovement, null);
            AnnouncerSounds.GetAnnouncerSounds().PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.MovementPhase);
            m_ignoreOldAbilityPhase = true;
        }
        else if (actionPhase != ActionBufferPhase.Done && nextActionPhase == ActionBufferPhase.Done)
        {
            GameEventManager.Get().FireEvent(GameEventManager.EventType.UIPhaseStartedDecision, null);
            m_ignoreOldAbilityPhase = true;
        }
        else if ((nextActionPhase == ActionBufferPhase.Abilities || nextActionPhase == ActionBufferPhase.AbilitiesWait)
                 && actionPhase != ActionBufferPhase.AbilitiesWait
                 && actionPhase != ActionBufferPhase.Abilities)
        {
            SetAbilityPhase(m_abilityPhase);
        }

        if ((actionPhase == ActionBufferPhase.Abilities
             || actionPhase == ActionBufferPhase.AbilitiesWait
             || actionPhase == ActionBufferPhase.MovementWait)
            && nextActionPhase == ActionBufferPhase.Done
            && TheatricsManager.Get() != null)
        {
            TheatricsManager.Get().SetAnimatorParamOnAllActors("DecisionPhase", true);
        }

        if (nextActionPhase == ActionBufferPhase.AbilitiesWait
            || nextActionPhase == ActionBufferPhase.Movement
            || nextActionPhase == ActionBufferPhase.MovementChase)
        {
            if (actionPhase == ActionBufferPhase.Abilities)
            {
                CameraManager.Get().SetTargetForMovementIfNeeded();
            }

            CameraManager.Get().SwitchCameraForMovement();
        }

        if ((nextActionPhase == ActionBufferPhase.AbilitiesWait || nextActionPhase == ActionBufferPhase.MovementWait)
            && GameplayMutators.Get() != null
            && GameFlowData.Get().HasPotentialGameMutatorVisibilityChanges(false)
            && FogOfWar.GetClientFog() != null)
        {
            FogOfWar.GetClientFog().UpdateVisibilityOfSquares();
        }
    }

    private void SetAbilityPhase(AbilityPriority value)
    {
        AbilityPriority abilityPhase = m_abilityPhase;
        m_abilityPhase = value;
        if (m_actionPhase != ActionBufferPhase.Abilities)
        {
            return;
        }

        switch (value)
        {
            case AbilityPriority.Prep_Defense:
            case AbilityPriority.Prep_Offense:
            {
                if (!m_ignoreOldAbilityPhase && (abilityPhase == AbilityPriority.Prep_Defense
                                                 || abilityPhase == AbilityPriority.Prep_Offense
                                                 || abilityPhase == AbilityPriority.Evasion
                                                 || abilityPhase == AbilityPriority.DEPRICATED_Combat_Charge
                                                 || abilityPhase == AbilityPriority.Combat_Damage
                                                 || abilityPhase == AbilityPriority.Combat_Final
                                                 || abilityPhase == AbilityPriority.Combat_Knockback))
                {
                    return;
                }
                
                m_ignoreOldAbilityPhase = false;
                GameEventManager.Get().FireEvent(GameEventManager.EventType.UIPhaseStartedPrep, null);
                AnnouncerSounds.GetAnnouncerSounds()
                    .PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.PrepPhase);

                return;
            }
            case AbilityPriority.Evasion when m_ignoreOldAbilityPhase || abilityPhase != AbilityPriority.Evasion:
            {
                m_ignoreOldAbilityPhase = false;
                GameEventManager.Get().FireEvent(GameEventManager.EventType.UIPhaseStartedEvasion, null);
                AnnouncerSounds.GetAnnouncerSounds()
                    .PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.DashPhase);
                return;
            }

            case AbilityPriority.DEPRICATED_Combat_Charge:
            case AbilityPriority.Combat_Damage:
            case AbilityPriority.Combat_Final:
            case AbilityPriority.Combat_Knockback:
            {
                if (!m_ignoreOldAbilityPhase && (abilityPhase == AbilityPriority.DEPRICATED_Combat_Charge
                                                 || abilityPhase == AbilityPriority.Combat_Damage
                                                 || abilityPhase == AbilityPriority.Combat_Final
                                                 || abilityPhase == AbilityPriority.Combat_Knockback))
                {
                    return;
                }
                
                m_ignoreOldAbilityPhase = false;
                GameEventManager.Get().FireEvent(GameEventManager.EventType.UIPhaseStartedCombat, null);
                AnnouncerSounds.GetAnnouncerSounds()
                    .PlayAnnouncementByEnum(AnnouncerSounds.AnnouncerEvent.BlastPhase);

                return;
            }
        }
    }

    public void SetDataFromShared(ActionBufferPhase actionPhase, AbilityPriority abilityPhase)
    {
        CurrentActionPhase = actionPhase;
        AbilityPhase = abilityPhase;
    }
}