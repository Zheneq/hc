using System;
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
			int j = 0;
			while (j < autoQueuedRequestActionTypes.Count)
			{
				if (i >= this.m_tauntButtons.Length)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						goto IL_183;
					}
				}
				else
				{
					AbilityData.ActionType actionType = autoQueuedRequestActionTypes[j];
					bool flag = UICharacterProfile.CanTauntForAction(activeOwnedActorData, abilityData, component, actionType);
					if (flag)
					{
						List<CameraShotSequence> tauntListForActionTypeForPlayer = abilityData.GetTauntListForActionTypeForPlayer(ClientGameManager.Get().GetPlayerCharacterData(activeOwnedActorData.m_characterType), activeOwnedActorData.GetCharacterResourceLink(), actionType);
						using (List<CameraShotSequence>.Enumerator enumerator = tauntListForActionTypeForPlayer.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								CameraShotSequence cameraShotSequence = enumerator.Current;
								if (i < this.m_tauntButtons.Length)
								{
									if (component.NumRequestsLeft(cameraShotSequence.m_uniqueTauntID) > 0)
									{
										UIManager.SetGameObjectActive(this.m_tauntButtons[i], true, null);
										this.m_tauntButtons[i].SetupTaunt(actionType, abilityData.GetAbilityEntryOfActionType(actionType), cameraShotSequence);
										num++;
										i++;
									}
								}
							}
						}
					}
					j++;
				}
			}
			IL_183:
			List<ActorTurnSM.ActionRequestForUndo> requestStackForUndo = activeOwnedActorData.GetActorTurnSM().GetRequestStackForUndo();
			int k = 0;
			while (k < requestStackForUndo.Count)
			{
				if (i >= this.m_tauntButtons.Length)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						goto IL_2C0;
					}
				}
				else
				{
					AbilityData.ActionType action = requestStackForUndo[k].m_action;
					bool flag2 = UICharacterProfile.CanTauntForAction(activeOwnedActorData, abilityData, component, action);
					if (flag2)
					{
						List<CameraShotSequence> tauntListForActionTypeForPlayer2 = abilityData.GetTauntListForActionTypeForPlayer(ClientGameManager.Get().GetPlayerCharacterData(activeOwnedActorData.m_characterType), activeOwnedActorData.GetCharacterResourceLink(), action);
						using (List<CameraShotSequence>.Enumerator enumerator2 = tauntListForActionTypeForPlayer2.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								CameraShotSequence cameraShotSequence2 = enumerator2.Current;
								if (i < this.m_tauntButtons.Length && component.NumRequestsLeft(cameraShotSequence2.m_uniqueTauntID) > 0)
								{
									UIManager.SetGameObjectActive(this.m_tauntButtons[i], true, null);
									this.m_tauntButtons[i].SetupTaunt(action, abilityData.GetAbilityEntryOfActionType(action), cameraShotSequence2);
									num++;
									i++;
								}
							}
						}
					}
					k++;
				}
			}
			IL_2C0:
			float y = 50f + (float)i * 93.6f;
			(base.gameObject.transform as RectTransform).sizeDelta = new Vector2((base.gameObject.transform as RectTransform).sizeDelta.x, y);
			while (i < this.m_tauntButtons.Length)
			{
				UIManager.SetGameObjectActive(this.m_tauntButtons[i], false, null);
				i++;
			}
		}
		return num;
	}
}
