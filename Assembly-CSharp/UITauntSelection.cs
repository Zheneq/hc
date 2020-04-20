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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITauntSelection.SetupTauntList()).MethodHandle;
			}
			ActorCinematicRequests component = activeOwnedActorData.GetComponent<ActorCinematicRequests>();
			AbilityData abilityData = activeOwnedActorData.GetAbilityData();
			int i = 0;
			List<AbilityData.ActionType> autoQueuedRequestActionTypes = activeOwnedActorData.GetActorTurnSM().GetAutoQueuedRequestActionTypes();
			int j = 0;
			while (j < autoQueuedRequestActionTypes.Count)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
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
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						List<CameraShotSequence> tauntListForActionTypeForPlayer = abilityData.GetTauntListForActionTypeForPlayer(ClientGameManager.Get().GetPlayerCharacterData(activeOwnedActorData.m_characterType), activeOwnedActorData.GetCharacterResourceLink(), actionType);
						using (List<CameraShotSequence>.Enumerator enumerator = tauntListForActionTypeForPlayer.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								CameraShotSequence cameraShotSequence = enumerator.Current;
								if (i < this.m_tauntButtons.Length)
								{
									for (;;)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									if (component.NumRequestsLeft(cameraShotSequence.m_uniqueTauntID) > 0)
									{
										for (;;)
										{
											switch (1)
											{
											case 0:
												continue;
											}
											break;
										}
										UIManager.SetGameObjectActive(this.m_tauntButtons[i], true, null);
										this.m_tauntButtons[i].SetupTaunt(actionType, abilityData.GetAbilityEntryOfActionType(actionType), cameraShotSequence);
										num++;
										i++;
									}
								}
							}
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
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
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						List<CameraShotSequence> tauntListForActionTypeForPlayer2 = abilityData.GetTauntListForActionTypeForPlayer(ClientGameManager.Get().GetPlayerCharacterData(activeOwnedActorData.m_characterType), activeOwnedActorData.GetCharacterResourceLink(), action);
						using (List<CameraShotSequence>.Enumerator enumerator2 = tauntListForActionTypeForPlayer2.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								CameraShotSequence cameraShotSequence2 = enumerator2.Current;
								if (i < this.m_tauntButtons.Length && component.NumRequestsLeft(cameraShotSequence2.m_uniqueTauntID) > 0)
								{
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									UIManager.SetGameObjectActive(this.m_tauntButtons[i], true, null);
									this.m_tauntButtons[i].SetupTaunt(action, abilityData.GetAbilityEntryOfActionType(action), cameraShotSequence2);
									num++;
									i++;
								}
							}
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return num;
	}
}
