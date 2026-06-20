using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UITauntSelection : MonoBehaviour
{
    public Button m_closeSelectionButton;
    public Image m_background;
    public UITauntSelectionButton[] m_tauntButtons;

    public int SetupTauntList()
    {
        ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
        if (activeOwnedActorData == null)
        {
            return 0;
        }

        int numTauntsAvailable = 0;
        ActorCinematicRequests actorCinematicRequests = activeOwnedActorData.GetComponent<ActorCinematicRequests>();
        AbilityData abilityData = activeOwnedActorData.GetAbilityData();

        int i = 0;
        List<AbilityData.ActionType> autoQueuedRequestActionTypes =
            activeOwnedActorData.GetActorTurnSM().GetAutoQueuedRequestActionTypes();
        foreach (AbilityData.ActionType autoQueuedActionType in autoQueuedRequestActionTypes)
        {
            if (i >= m_tauntButtons.Length)
            {
                break;
            }

            AbilityData.ActionType actionType = autoQueuedActionType;
            if (UICharacterProfile.CanTauntForAction(
                    activeOwnedActorData,
                    abilityData,
                    actorCinematicRequests,
                    actionType))
            {
                PersistedCharacterData persistedCharacterData =
                    ClientGameManager.Get().GetPlayerCharacterData(activeOwnedActorData.m_characterType);
                CharacterResourceLink characterResourceLink = activeOwnedActorData.GetCharacterResourceLink();
                List<CameraShotSequence> tauntList =
                    abilityData.GetTauntListForActionTypeForPlayer(
                        persistedCharacterData,
                        characterResourceLink,
                        actionType);

                foreach (CameraShotSequence taunt in tauntList)
                {
                    if (i < m_tauntButtons.Length && actorCinematicRequests.NumRequestsLeft(taunt.m_uniqueTauntID) > 0)
                    {
                        UIManager.SetGameObjectActive(m_tauntButtons[i], true);
                        m_tauntButtons[i].SetupTaunt(
                            actionType,
                            abilityData.GetAbilityEntryOfActionType(actionType),
                            taunt);
                        numTauntsAvailable++;
                        i++;
                    }
                }
            }
        }

        List<ActorTurnSM.ActionRequestForUndo> requestStackForUndo =
            activeOwnedActorData.GetActorTurnSM().GetRequestStackForUndo();
        foreach (ActorTurnSM.ActionRequestForUndo requestForUndo in requestStackForUndo)
        {
            if (i >= m_tauntButtons.Length)
            {
                break;
            }

            AbilityData.ActionType action = requestForUndo.m_action;
            if (UICharacterProfile.CanTauntForAction(activeOwnedActorData, abilityData, actorCinematicRequests, action))
            {
                PersistedCharacterData persistedCharacterData =
                    ClientGameManager.Get().GetPlayerCharacterData(activeOwnedActorData.m_characterType);
                CharacterResourceLink characterResourceLink = activeOwnedActorData.GetCharacterResourceLink();
                List<CameraShotSequence> tauntList =
                    abilityData.GetTauntListForActionTypeForPlayer(
                        persistedCharacterData,
                        characterResourceLink,
                        action);

                foreach (CameraShotSequence taunt in tauntList)
                {
                    if (i < m_tauntButtons.Length && actorCinematicRequests.NumRequestsLeft(taunt.m_uniqueTauntID) > 0)
                    {
                        UIManager.SetGameObjectActive(m_tauntButtons[i], true);
                        m_tauntButtons[i].SetupTaunt(action, abilityData.GetAbilityEntryOfActionType(action), taunt);
                        numTauntsAvailable++;
                        i++;
                    }
                }
            }
        }

        float y = 50f + i * 93.6f;
        RectTransform rectTransform = gameObject.transform as RectTransform;
        Vector2 sizeDelta = rectTransform.sizeDelta;
        rectTransform.sizeDelta = new Vector2(sizeDelta.x, y);

        for (; i < m_tauntButtons.Length; i++)
        {
            UIManager.SetGameObjectActive(m_tauntButtons[i], false);
        }

        return numTauntsAvailable;
    }
}