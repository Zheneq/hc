using System;
using UnityEngine;

[Serializable]
public class ActivatableUI
{
	public enum ActivationAction
	{
		SetActive,
		ClearActive,
		ToggleActive
	}

	public enum UIElement
	{
		TopDisplayPanel,
		DecisionTimer,
		LockInCancelButton,
		AbilityButton,
		AbilityButton1,
		AbilityButton2,
		AbilityButton3,
		AbilityButton4,
		QueuedCard,
		QueuedCard1,
		QueuedCard2,
		Taunt,
		TopDisplayPanelBackground,
		TopDisplayPanelCenterPiece,
		TopDisplayPanelPlayerStatus,
		TopDisplayPanelPlayerStatus1,
		TopDisplayPanelPlayerStatus2,
		TopDisplayPanelPlayerStatus3,
		TopDisplayPanelPlayerStatus4,
		TopDisplayPanelPlayerStatus5,
		ObjectivePanel,
		AbilityButtonGlow,
		AbilityButtonGlow1,
		AbilityButtonGlow2,
		AbilityButtonGlow3,
		AbilityButtonGlow4,
		AbilityButtonTutorialTip,
		AbilityButtonTutorialTip1,
		AbilityButtonTutorialTip2,
		AbilityButtonTutorialTip3,
		AbilityButtonTutorialTip4,
		LockInButtonTutorialTip,
		CameraControlsTutorialPanel,
		EnergyGlow,
		EnergyArrows,
		CombatPhaseTutorialPanel,
		DashPhaseTutorialPanel,
		PrepPhaseTutorialPanel,
		NotificationPanel,
		BuffList,
		FullScreenCombatPhaseTutorialPanel,
		FullScreenDashPhaseTutorialPanel,
		FullScreenPrepPhaseTutorialPanel,
		LockInButtonTutorialTipImage,
		LockInButtonTutorialTipText,
		LockinPhaseDisplay,
		LockinPhaseTextDisplay,
		LockinPhaseColorDisplay,
		CardButtonTutorialTip,
		CardButtonTutorialTip1,
		CardButtonTutorialTip2,
		StatusEffectTutorialPanel,
		TeammateTargetingTutorialPanel,
		EnergyAndUltimatesTutorialPanel,
		FadeOutPanel,
		ResetMatchTimer,
		ResetMatchTurn
	}

	public UIElement m_uiElement;

	public ActivationAction m_activation;

	public void Activate()
	{
		GameEventManager.EventType eventType = GameEventManager.EventType.Invalid;
		AbilityData.ActionType actionType = AbilityData.ActionType.INVALID_ACTION;
		GameObject gameObject = null;
		switch (m_uiElement)
		{
		case UIElement.TopDisplayPanel:
		{
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.gameObject;
			int num;
			if (m_activation != 0)
			{
				num = ((m_activation == ActivationAction.ToggleActive) ? 1 : 0);
			}
			else
			{
				num = 1;
			}
			bool flag = (byte)num != 0;
			UIManager.SetGameObjectActive(UITimerPanel.Get(), flag);
			if (ObjectivePoints.Get() != null)
			{
				ObjectivePoints.Get().SetVisible(flag);
			}
			break;
		}
		case UIElement.DecisionTimer:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.gameObject;
			break;
		case UIElement.LockInCancelButton:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInCancelButton.gameObject;
			break;
		case UIElement.AbilityButton:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[0].gameObject;
			break;
		case UIElement.AbilityButton1:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[1].gameObject;
			break;
		case UIElement.AbilityButton2:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[2].gameObject;
			break;
		case UIElement.AbilityButton3:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[3].gameObject;
			break;
		case UIElement.AbilityButton4:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[4].gameObject;
			break;
		case UIElement.QueuedCard:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[0].gameObject;
			break;
		case UIElement.QueuedCard1:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[1].gameObject;
			break;
		case UIElement.QueuedCard2:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[2].gameObject;
			break;
		case UIElement.TopDisplayPanelBackground:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_background.gameObject;
			break;
		case UIElement.TopDisplayPanelCenterPiece:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_centerPiece.gameObject;
			break;
		case UIElement.TopDisplayPanelPlayerStatus:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_teamPlayerIcons[0].gameObject;
			break;
		case UIElement.TopDisplayPanelPlayerStatus1:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_teamPlayerIcons[1].gameObject;
			break;
		case UIElement.TopDisplayPanelPlayerStatus2:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_teamPlayerIcons[2].gameObject;
			break;
		case UIElement.TopDisplayPanelPlayerStatus3:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_enemyPlayerIcons[0].gameObject;
			break;
		case UIElement.TopDisplayPanelPlayerStatus4:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_enemyPlayerIcons[1].gameObject;
			break;
		case UIElement.TopDisplayPanelPlayerStatus5:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_enemyPlayerIcons[2].gameObject;
			break;
		case UIElement.ObjectivePanel:
		{
			Tutorial tutorial = Tutorial.Get();
			if (!tutorial)
			{
				break;
			}
			if ((bool)tutorial.m_tutorialPanel)
			{
				gameObject = tutorial.m_tutorialPanel.gameObject;
			}
			break;
		}
		case UIElement.AbilityButtonGlow:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[0].m_borderGlow.gameObject;
			eventType = GameEventManager.EventType.UITutorialHighlightChanged;
			actionType = AbilityData.ActionType.ABILITY_0;
			break;
		case UIElement.AbilityButtonGlow1:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[1].m_borderGlow.gameObject;
			eventType = GameEventManager.EventType.UITutorialHighlightChanged;
			actionType = AbilityData.ActionType.ABILITY_1;
			break;
		case UIElement.AbilityButtonGlow2:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[2].m_borderGlow.gameObject;
			eventType = GameEventManager.EventType.UITutorialHighlightChanged;
			actionType = AbilityData.ActionType.ABILITY_2;
			break;
		case UIElement.AbilityButtonGlow3:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[3].m_borderGlow.gameObject;
			eventType = GameEventManager.EventType.UITutorialHighlightChanged;
			actionType = AbilityData.ActionType.ABILITY_3;
			break;
		case UIElement.AbilityButtonGlow4:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[4].m_borderGlow.gameObject;
			eventType = GameEventManager.EventType.UITutorialHighlightChanged;
			actionType = AbilityData.ActionType.ABILITY_4;
			break;
		case UIElement.AbilityButtonTutorialTip:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[0].m_tutorialTip.gameObject;
			break;
		case UIElement.AbilityButtonTutorialTip1:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[1].m_tutorialTip.gameObject;
			break;
		case UIElement.AbilityButtonTutorialTip2:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[2].m_tutorialTip.gameObject;
			break;
		case UIElement.AbilityButtonTutorialTip3:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[3].m_tutorialTip.gameObject;
			break;
		case UIElement.AbilityButtonTutorialTip4:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[4].m_tutorialTip.gameObject;
			break;
		case UIElement.LockInButtonTutorialTip:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInTutorialTip.gameObject;
			break;
		case UIElement.CameraControlsTutorialPanel:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialCameraControlsPanel;
			break;
		case UIElement.EnergyGlow:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_characterProfile.m_tutorialEnergyGlow.gameObject;
			break;
		case UIElement.EnergyArrows:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_characterProfile.m_tutorialEnergyArrows;
			break;
		case UIElement.CombatPhaseTutorialPanel:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialCombatPhasePanel;
			break;
		case UIElement.DashPhaseTutorialPanel:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialDashPhasePanel;
			break;
		case UIElement.PrepPhaseTutorialPanel:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialPrepPhasePanel;
			break;
		case UIElement.NotificationPanel:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_notificationPanel.gameObject;
			break;
		case UIElement.BuffList:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_characterProfile.m_buffGrid.gameObject;
			break;
		case UIElement.FullScreenCombatPhaseTutorialPanel:
		{
			if (m_activation != 0)
			{
				if (m_activation != ActivationAction.ToggleActive)
				{
					UITutorialFullscreenPanel.Get().ClearAllPanels();
					break;
				}
			}
			UITutorialPanel uITutorialPanel = UITutorialPanel.Get();
			if (_003C_003Ef__am_0024cache0 == null)
			{
				_003C_003Ef__am_0024cache0 = delegate
				{
					UITutorialFullscreenPanel.Get().ShowCombatPhasePanel();
				};
			}
			uITutorialPanel.QueueAction(_003C_003Ef__am_0024cache0);
			break;
		}
		case UIElement.FullScreenDashPhaseTutorialPanel:
		{
			if (m_activation != 0)
			{
				if (m_activation != ActivationAction.ToggleActive)
				{
					UITutorialFullscreenPanel.Get().ClearAllPanels();
					break;
				}
			}
			UITutorialPanel uITutorialPanel3 = UITutorialPanel.Get();
			if (_003C_003Ef__am_0024cache1 == null)
			{
				_003C_003Ef__am_0024cache1 = delegate
				{
					UITutorialFullscreenPanel.Get().ShowDashPhasePanel();
				};
			}
			uITutorialPanel3.QueueAction(_003C_003Ef__am_0024cache1);
			break;
		}
		case UIElement.FullScreenPrepPhaseTutorialPanel:
		{
			if (m_activation != 0)
			{
				if (m_activation != ActivationAction.ToggleActive)
				{
					UITutorialFullscreenPanel.Get().ClearAllPanels();
					break;
				}
			}
			UITutorialPanel uITutorialPanel2 = UITutorialPanel.Get();
			if (_003C_003Ef__am_0024cache2 == null)
			{
				_003C_003Ef__am_0024cache2 = delegate
				{
					UITutorialFullscreenPanel.Get().ShowPrepPhasePanel();
				};
			}
			uITutorialPanel2.QueueAction(_003C_003Ef__am_0024cache2);
			break;
		}
		case UIElement.FadeOutPanel:
			if (m_activation != 0)
			{
				if (m_activation != ActivationAction.ToggleActive)
				{
					UITutorialFullscreenPanel.Get().FadeIn();
					break;
				}
			}
			UITutorialFullscreenPanel.Get().FadeOut();
			break;
		case UIElement.ResetMatchTimer:
			if (m_activation != 0)
			{
				if (m_activation != ActivationAction.ToggleActive)
				{
					break;
				}
			}
			UITimerPanel.Get().SetMatchTime(0f);
			break;
		case UIElement.ResetMatchTurn:
			if (m_activation != 0)
			{
				if (m_activation != ActivationAction.ToggleActive)
				{
					break;
				}
			}
			UITimerPanel.Get().SetMatchTurn(1);
			break;
		case UIElement.LockInButtonTutorialTipText:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInTutorialTip.GetComponent<UITextSizer>().m_border.gameObject;
			break;
		case UIElement.LockInButtonTutorialTipImage:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInTutorialTipSubImage.gameObject;
			break;
		case UIElement.LockinPhaseDisplay:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInCancelButton.m_phaseMarkerContainer.gameObject;
			break;
		case UIElement.LockinPhaseTextDisplay:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInCancelButton.m_phaseLabelContainer.gameObject;
			break;
		case UIElement.LockinPhaseColorDisplay:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInCancelButton.m_phaseColor.gameObject;
			break;
		case UIElement.CardButtonTutorialTip:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[0].m_tutorialTip.gameObject;
			eventType = GameEventManager.EventType.UITutorialHighlightChanged;
			actionType = AbilityData.ActionType.CARD_0;
			break;
		case UIElement.CardButtonTutorialTip1:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[1].m_tutorialTip.gameObject;
			eventType = GameEventManager.EventType.UITutorialHighlightChanged;
			actionType = AbilityData.ActionType.CARD_1;
			break;
		case UIElement.CardButtonTutorialTip2:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[2].m_tutorialTip.gameObject;
			eventType = GameEventManager.EventType.UITutorialHighlightChanged;
			actionType = AbilityData.ActionType.CARD_2;
			break;
		case UIElement.StatusEffectTutorialPanel:
			if (m_activation != 0)
			{
				if (m_activation != ActivationAction.ToggleActive)
				{
					UITutorialFullscreenPanel.Get().ClearAllPanels();
					break;
				}
			}
			UITutorialFullscreenPanel.Get().ShowStatusEffectPanel();
			break;
		case UIElement.TeammateTargetingTutorialPanel:
			if (m_activation != 0)
			{
				if (m_activation != ActivationAction.ToggleActive)
				{
					UITutorialFullscreenPanel.Get().ClearAllPanels();
					break;
				}
			}
			UITutorialFullscreenPanel.Get().ShowTeammateTargetingPanel();
			break;
		case UIElement.EnergyAndUltimatesTutorialPanel:
			if (m_activation != 0)
			{
				if (m_activation != ActivationAction.ToggleActive)
				{
					UITutorialFullscreenPanel.Get().ClearAllPanels();
					break;
				}
			}
			UITutorialFullscreenPanel.Get().ShowEnergyAndUltimatesPanel();
			break;
		}
		if (!(gameObject != null))
		{
			return;
		}
		while (true)
		{
			bool active = false;
			if (m_activation == ActivationAction.SetActive)
			{
				UIManager.SetGameObjectActive(gameObject, true);
				active = true;
			}
			else if (m_activation == ActivationAction.ClearActive)
			{
				UIManager.SetGameObjectActive(gameObject, false);
				active = false;
			}
			else if (m_activation == ActivationAction.ToggleActive)
			{
				active = !gameObject.activeSelf;
				UIManager.SetGameObjectActive(gameObject, !gameObject.activeSelf);
			}
			if (actionType != AbilityData.ActionType.INVALID_ACTION)
			{
				GameEventManager.ActivationInfo activationInfo = new GameEventManager.ActivationInfo();
				activationInfo.actionType = actionType;
				activationInfo.active = active;
				GameEventManager.Get().FireEvent(eventType, activationInfo);
			}
			if (m_uiElement == UIElement.DecisionTimer)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						if ((bool)SinglePlayerManager.Get())
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									SinglePlayerManager.Get().SetDecisionTimerForceOff(!gameObject.activeSelf);
									return;
								}
							}
						}
						return;
					}
				}
			}
			if (m_uiElement == UIElement.LockInCancelButton)
			{
				if ((bool)SinglePlayerManager.Get())
				{
					while (true)
					{
						SinglePlayerManager.Get().SetLockInCancelButtonForceOff(!gameObject.activeSelf);
						return;
					}
				}
				return;
			}
			if (m_uiElement == UIElement.NotificationPanel)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						if ((bool)SinglePlayerManager.Get())
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									SinglePlayerManager.Get().SetNotificationPanelForceOff(!gameObject.activeSelf);
									return;
								}
							}
						}
						return;
					}
				}
			}
			if (m_uiElement == UIElement.LockinPhaseDisplay)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if ((bool)SinglePlayerManager.Get())
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									SinglePlayerManager.Get().SetLockinPhaseDisplayForceOff(!gameObject.activeSelf);
									return;
								}
							}
						}
						return;
					}
				}
			}
			if (m_uiElement == UIElement.LockinPhaseTextDisplay)
			{
				if ((bool)SinglePlayerManager.Get())
				{
					SinglePlayerManager.Get().SetLockinPhaseTextForceOff(!gameObject.activeSelf);
				}
				return;
			}
			if (m_uiElement == UIElement.LockinPhaseColorDisplay)
			{
				if (!SinglePlayerManager.Get())
				{
					return;
				}
				while (true)
				{
					SinglePlayerManager.Get().SetLockinPhaseColorForceOff(!gameObject.activeSelf);
					return;
				}
			}
			if (m_uiElement == UIElement.TopDisplayPanelPlayerStatus)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if ((bool)SinglePlayerManager.Get())
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									SinglePlayerManager.Get().SetTeamPlayerIconForceOff(0, !gameObject.activeSelf);
									return;
								}
							}
						}
						return;
					}
				}
			}
			if (m_uiElement == UIElement.TopDisplayPanelPlayerStatus1)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						if ((bool)SinglePlayerManager.Get())
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									SinglePlayerManager.Get().SetTeamPlayerIconForceOff(1, !gameObject.activeSelf);
									return;
								}
							}
						}
						return;
					}
				}
			}
			if (m_uiElement == UIElement.TopDisplayPanelPlayerStatus2)
			{
				if (!SinglePlayerManager.Get())
				{
					return;
				}
				while (true)
				{
					SinglePlayerManager.Get().SetTeamPlayerIconForceOff(2, !gameObject.activeSelf);
					return;
				}
			}
			if (m_uiElement == UIElement.TopDisplayPanelPlayerStatus3)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if ((bool)SinglePlayerManager.Get())
						{
							SinglePlayerManager.Get().SetEnemyPlayerIconForceOff(0, !gameObject.activeSelf);
						}
						return;
					}
				}
			}
			if (m_uiElement == UIElement.TopDisplayPanelPlayerStatus4)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						if ((bool)SinglePlayerManager.Get())
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									SinglePlayerManager.Get().SetEnemyPlayerIconForceOff(1, !gameObject.activeSelf);
									return;
								}
							}
						}
						return;
					}
				}
			}
			if (m_uiElement != UIElement.TopDisplayPanelPlayerStatus5)
			{
				return;
			}
			while (true)
			{
				if ((bool)SinglePlayerManager.Get())
				{
					while (true)
					{
						SinglePlayerManager.Get().SetEnemyPlayerIconForceOff(2, !gameObject.activeSelf);
						return;
					}
				}
				return;
			}
		}
	}
}
