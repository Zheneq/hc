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
		int num = 0;
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null)
		{
			ActorCinematicRequests component = activeOwnedActorData.GetComponent<ActorCinematicRequests>();
			AbilityData abilityData = activeOwnedActorData.GetAbilityData();
			int i = 0;
			List<AbilityData.ActionType> autoQueuedRequestActionTypes = activeOwnedActorData.GetActorTurnSM().GetAutoQueuedRequestActionTypes();
			for (int j = 0; j < autoQueuedRequestActionTypes.Count; j++)
			{
				if (i < m_tauntButtons.Length)
				{
					AbilityData.ActionType actionType = autoQueuedRequestActionTypes[j];
					if (UICharacterProfile.CanTauntForAction(activeOwnedActorData, abilityData, component, actionType))
					{
						List<CameraShotSequence> tauntListForActionTypeForPlayer = abilityData.GetTauntListForActionTypeForPlayer(ClientGameManager.Get().GetPlayerCharacterData(activeOwnedActorData.m_characterType), activeOwnedActorData.GetCharacterResourceLink(), actionType);
						using (List<CameraShotSequence>.Enumerator enumerator = tauntListForActionTypeForPlayer.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								CameraShotSequence current = enumerator.Current;
								if (i < m_tauntButtons.Length)
								{
									if (component.NumRequestsLeft(current.m_uniqueTauntID) > 0)
									{
										UIManager.SetGameObjectActive(m_tauntButtons[i], true);
										m_tauntButtons[i].SetupTaunt(actionType, abilityData.GetAbilityEntryOfActionType(actionType), current);
										num++;
										i++;
									}
								}
							}
						}
					}
					continue;
				}
				break;
			}
			List<ActorTurnSM.ActionRequestForUndo> requestStackForUndo = activeOwnedActorData.GetActorTurnSM().GetRequestStackForUndo();
			for (int k = 0; k < requestStackForUndo.Count; k++)
			{
				if (i < m_tauntButtons.Length)
				{
					AbilityData.ActionType action = requestStackForUndo[k].m_action;
					if (UICharacterProfile.CanTauntForAction(activeOwnedActorData, abilityData, component, action))
					{
						List<CameraShotSequence> tauntListForActionTypeForPlayer2 = abilityData.GetTauntListForActionTypeForPlayer(ClientGameManager.Get().GetPlayerCharacterData(activeOwnedActorData.m_characterType), activeOwnedActorData.GetCharacterResourceLink(), action);
						using (List<CameraShotSequence>.Enumerator enumerator2 = tauntListForActionTypeForPlayer2.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								CameraShotSequence current2 = enumerator2.Current;
								if (i < m_tauntButtons.Length && component.NumRequestsLeft(current2.m_uniqueTauntID) > 0)
								{
									UIManager.SetGameObjectActive(m_tauntButtons[i], true);
									m_tauntButtons[i].SetupTaunt(action, abilityData.GetAbilityEntryOfActionType(action), current2);
									num++;
									i++;
								}
							}
						}
					}
					continue;
				}
				break;
			}
			float y = 50f + (float)i * 93.6f;
			RectTransform obj = base.gameObject.transform as RectTransform;
			Vector2 sizeDelta = (base.gameObject.transform as RectTransform).sizeDelta;
			obj.sizeDelta = new Vector2(sizeDelta.x, y);
			for (; i < m_tauntButtons.Length; i++)
			{
				UIManager.SetGameObjectActive(m_tauntButtons[i], false);
			}
		}
		return num;
	}
}
