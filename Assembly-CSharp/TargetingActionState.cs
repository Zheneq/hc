using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class TargetingActionState : TurnState
{
    public int TargetIndex
    {
        get { return m_SM.GetAbilityTargets().Count; }
    }

    public TargetingActionState(ActorTurnSM masterSM)
        : base(masterSM)
    {
    }

    public override void OnEnter()
    {
        SetupTargeting();
    }

    private void SetupTargeting()
    {
        m_SM.ClearAbilityTargets();
        if (GameFlowData.Get().activeOwnedActorData == null
            || GameFlowData.Get().activeOwnedActorData.GetComponent<ActorTurnSM>() != m_SM)
        {
            return;
        }

        ActorData actorData = m_SM.GetComponent<ActorData>();
        AbilityData abilityData = m_SM.GetComponent<AbilityData>();
        BoardSquare autoSelectTarget = abilityData.GetAutoSelectTarget();
        if (autoSelectTarget == null)
        {
            return;
        }

        AbilityTarget newTarget = AbilityTarget.CreateAbilityTargetFromBoardSquare(autoSelectTarget, actorData.GetFreePos());
        m_SM.AddAbilityTarget(newTarget);
        AbilityData.ActionType selectedActionType = abilityData.GetSelectedActionType();
        m_SM.OnQueueAbilityRequest(selectedActionType);
        if (!NetworkServer.active)
        {
            m_SM.SendCastAbility(selectedActionType);
            m_SM.NextState = TurnStateEnum.VALIDATING_ACTION_REQUEST;
        }
    }

    private bool SelectTarget(AbilityTarget abilityTargetToUse)
    {
        bool success = m_SM.SelectTarget(abilityTargetToUse);
        if (success)
        {
            m_SM.NextState = TurnStateEnum.VALIDATING_ACTION_REQUEST;
        }

        return success;
    }

    public override void OnSelectedAbilityChanged()
    {
        SetupTargeting();
    }

    public override void OnMsg(TurnMessage msg, int extraData)
    {
        AbilityData component = m_SM.GetComponent<AbilityData>();
        switch (msg)
        {
            case TurnMessage.SELECTED_ABILITY:
            case TurnMessage.MOVEMENT_ACCEPTED:
            case TurnMessage.MOVEMENT_REJECTED:
            case TurnMessage.RESPAWN:
            case TurnMessage.PICK_RESPAWN:
            case TurnMessage.PICKED_RESPAWN:
            case TurnMessage.CANCEL_SINGLE_ABILITY:
            case TurnMessage.CANCEL_MOVEMENT:
                break;
            case TurnMessage.ABILITY_REQUEST_ACCEPTED:
                m_SM.NextState = TurnStateEnum.DECIDING;
                break;
            case TurnMessage.BEGIN_RESOLVE:
                m_SM.NextState = TurnStateEnum.WAITING;
                break;
            case TurnMessage.CANCEL_BUTTON_CLICKED:
                component.ClearSelectedAbility();
                component.ClearActionsToCancelOnTargetingComplete();
                if (NetworkClient.active)
                {
                    UISounds.GetUISounds().Play("ui/ingame/v1/action_undo");
                }

                m_SM.NextState = TurnStateEnum.DECIDING;
                break;
            case TurnMessage.DISCONNECTED:
                m_SM.NextState = TurnStateEnum.CONFIRMED;
                break;
            case TurnMessage.ABILITY_REQUEST_REJECTED:
                component.ClearSelectedAbility();
                m_SM.NextState = TurnStateEnum.DECIDING;
                break;
            case TurnMessage.MOVE_BUTTON_CLICKED:
                m_SM.NextState = TurnStateEnum.DECIDING_MOVEMENT;
                break;
            case TurnMessage.MOVEMENT_RESOLVED:
                Log.Error(new StringBuilder().Append(m_SM.GetComponent<ActorData>().DisplayName).Append("Received a 'Movement Resolved' message in the TargetingAction state, which is unexpected.").ToString());
                m_SM.NextState = TurnStateEnum.WAITING;
                break;
            case TurnMessage.CLIENTS_RESOLVED_ABILITIES:
                Log.Warning(new StringBuilder().Append(m_SM.GetComponent<ActorData>().DisplayName).Append("Received a 'CLIENTS_RESOLVED_ABILITIES' message in the TargetingAction state, which is unexpected.").ToString());
                m_SM.NextState = TurnStateEnum.WAITING;
                break;
            case TurnMessage.DONE_BUTTON_CLICKED:
                m_SM.NextState = TurnStateEnum.CONFIRMED;
                if (SinglePlayerManager.Get())
                {
                    SinglePlayerManager.Get().OnActorLockInEntered(m_SM.GetComponent<ActorData>());
                }

                break;
        }
    }

    public override void Update()
    {
        if (GameFlowData.Get().activeOwnedActorData != null
            && GameFlowData.Get().activeOwnedActorData.GetComponent<ActorTurnSM>() == m_SM)
        {
            AbilityData abilityData = m_SM.GetComponent<AbilityData>();
            ActorData actorData = m_SM.GetComponent<ActorData>();
            if (abilityData != null && actorData != null)
            {
                Ability selectedAbility = abilityData.GetSelectedAbility();
                if (selectedAbility != null)
                {
                    Ability.TargetingParadigm targetingParadigm = selectedAbility.GetTargetingParadigm(TargetIndex);
                    bool isDirectionOnPosition = targetingParadigm == Ability.TargetingParadigm.Direction
                                                 || targetingParadigm == Ability.TargetingParadigm.Position;
                    if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.CycleTarget) && isDirectionOnPosition)
                    {
                        abilityData.NextSoftTarget();
                    }
                }

                if ((Input.GetMouseButtonUp(0)
                     || Input.GetMouseButtonUp(1)
                     || InputManager.Get().GetAcceptButtonDown())
                    && InterfaceManager.Get().ShouldHandleMouseClick()
                    && !m_SM.HandledMouseInput)
                {
                    m_SM.HandledMouseInput = true;
                    AbilityTarget abilityTargetForTargeterUpdate = AbilityTarget.GetAbilityTargetForTargeterUpdate();
                    if (selectedAbility != null
                        && abilityData.ValidateAbilityOnTarget(selectedAbility, abilityTargetForTargeterUpdate,
                            TargetIndex))
                    {
                        if (Input.GetMouseButtonUp(1) && !Options_UI.Get().GetRightClickingConfirmsAbilityTargets())
                        {
                            m_SM.RequestCancel();
                        }
                        else
                        {
                            bool isTargetSelected = SelectTarget(abilityTargetForTargeterUpdate);
                            bool canOverrideMoveStartSquare =
                                selectedAbility.GetRunPriority() == AbilityPriority.Evasion
                                && selectedAbility.CanOverrideMoveStartSquare();
                            if (isTargetSelected && !canOverrideMoveStartSquare)
                            {
                                UISounds.GetUISounds().Play("ui/ingame/v1/action");
                            }
                            else
                            {
                                UISounds.GetUISounds().Play("ui/ingame/v1/action_target_selected");
                            }
                        }
                    }
                }
            }
        }

        m_SM.UpdateEndTurnKey();
    }
}