using System;
using UnityEngine;

[Serializable]
public class ActivatableUI
{
	public ActivatableUI.UIElement m_uiElement;

	public ActivatableUI.ActivationAction m_activation;

	public void Activate()
	{
		GameEventManager.EventType eventType = GameEventManager.EventType.Invalid;
		AbilityData.ActionType actionType = AbilityData.ActionType.INVALID_ACTION;
		GameObject gameObject = null;
		switch (this.m_uiElement)
		{
		case ActivatableUI.UIElement.TopDisplayPanel:
		{
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.gameObject;
			bool flag;
			if (this.m_activation != ActivatableUI.ActivationAction.SetActive)
			{
				flag = (this.m_activation == ActivatableUI.ActivationAction.ToggleActive);
			}
			else
			{
				flag = true;
			}
			bool flag2 = flag;
			UIManager.SetGameObjectActive(UITimerPanel.Get(), flag2, null);
			if (ObjectivePoints.Get() != null)
			{
				ObjectivePoints.Get().SetVisible(flag2);
			}
			break;
		}
		case ActivatableUI.UIElement.DecisionTimer:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.gameObject;
			break;
		case ActivatableUI.UIElement.LockInCancelButton:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInCancelButton.gameObject;
			break;
		case ActivatableUI.UIElement.AbilityButton:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[0].gameObject;
			break;
		case ActivatableUI.UIElement.AbilityButton1:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[1].gameObject;
			break;
		case ActivatableUI.UIElement.AbilityButton2:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[2].gameObject;
			break;
		case ActivatableUI.UIElement.AbilityButton3:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[3].gameObject;
			break;
		case ActivatableUI.UIElement.AbilityButton4:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[4].gameObject;
			break;
		case ActivatableUI.UIElement.QueuedCard:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[0].gameObject;
			break;
		case ActivatableUI.UIElement.QueuedCard1:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[1].gameObject;
			break;
		case ActivatableUI.UIElement.QueuedCard2:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[2].gameObject;
			break;
		case ActivatableUI.UIElement.TopDisplayPanelBackground:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_background.gameObject;
			break;
		case ActivatableUI.UIElement.TopDisplayPanelCenterPiece:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_centerPiece.gameObject;
			break;
		case ActivatableUI.UIElement.TopDisplayPanelPlayerStatus:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_teamPlayerIcons[0].gameObject;
			break;
		case ActivatableUI.UIElement.TopDisplayPanelPlayerStatus1:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_teamPlayerIcons[1].gameObject;
			break;
		case ActivatableUI.UIElement.TopDisplayPanelPlayerStatus2:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_teamPlayerIcons[2].gameObject;
			break;
		case ActivatableUI.UIElement.TopDisplayPanelPlayerStatus3:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_enemyPlayerIcons[0].gameObject;
			break;
		case ActivatableUI.UIElement.TopDisplayPanelPlayerStatus4:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_enemyPlayerIcons[1].gameObject;
			break;
		case ActivatableUI.UIElement.TopDisplayPanelPlayerStatus5:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_enemyPlayerIcons[2].gameObject;
			break;
		case ActivatableUI.UIElement.ObjectivePanel:
		{
			Tutorial tutorial = Tutorial.Get();
			if (tutorial)
			{
				if (tutorial.m_tutorialPanel)
				{
					gameObject = tutorial.m_tutorialPanel.gameObject;
				}
			}
			break;
		}
		case ActivatableUI.UIElement.AbilityButtonGlow:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[0].m_borderGlow.gameObject;
			eventType = GameEventManager.EventType.UITutorialHighlightChanged;
			actionType = AbilityData.ActionType.ABILITY_0;
			break;
		case ActivatableUI.UIElement.AbilityButtonGlow1:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[1].m_borderGlow.gameObject;
			eventType = GameEventManager.EventType.UITutorialHighlightChanged;
			actionType = AbilityData.ActionType.ABILITY_1;
			break;
		case ActivatableUI.UIElement.AbilityButtonGlow2:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[2].m_borderGlow.gameObject;
			eventType = GameEventManager.EventType.UITutorialHighlightChanged;
			actionType = AbilityData.ActionType.ABILITY_2;
			break;
		case ActivatableUI.UIElement.AbilityButtonGlow3:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[3].m_borderGlow.gameObject;
			eventType = GameEventManager.EventType.UITutorialHighlightChanged;
			actionType = AbilityData.ActionType.ABILITY_3;
			break;
		case ActivatableUI.UIElement.AbilityButtonGlow4:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[4].m_borderGlow.gameObject;
			eventType = GameEventManager.EventType.UITutorialHighlightChanged;
			actionType = AbilityData.ActionType.ABILITY_4;
			break;
		case ActivatableUI.UIElement.AbilityButtonTutorialTip:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[0].m_tutorialTip.gameObject;
			break;
		case ActivatableUI.UIElement.AbilityButtonTutorialTip1:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[1].m_tutorialTip.gameObject;
			break;
		case ActivatableUI.UIElement.AbilityButtonTutorialTip2:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[2].m_tutorialTip.gameObject;
			break;
		case ActivatableUI.UIElement.AbilityButtonTutorialTip3:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[3].m_tutorialTip.gameObject;
			break;
		case ActivatableUI.UIElement.AbilityButtonTutorialTip4:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_abilityButtons[4].m_tutorialTip.gameObject;
			break;
		case ActivatableUI.UIElement.LockInButtonTutorialTip:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInTutorialTip.gameObject;
			break;
		case ActivatableUI.UIElement.CameraControlsTutorialPanel:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialCameraControlsPanel;
			break;
		case ActivatableUI.UIElement.EnergyGlow:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_characterProfile.m_tutorialEnergyGlow.gameObject;
			break;
		case ActivatableUI.UIElement.EnergyArrows:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_characterProfile.m_tutorialEnergyArrows;
			break;
		case ActivatableUI.UIElement.CombatPhaseTutorialPanel:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialCombatPhasePanel;
			break;
		case ActivatableUI.UIElement.DashPhaseTutorialPanel:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialDashPhasePanel;
			break;
		case ActivatableUI.UIElement.PrepPhaseTutorialPanel:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.m_tutorialPrepPhasePanel;
			break;
		case ActivatableUI.UIElement.NotificationPanel:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_notificationPanel.gameObject;
			break;
		case ActivatableUI.UIElement.BuffList:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_characterProfile.m_buffGrid.gameObject;
			break;
		case ActivatableUI.UIElement.FullScreenCombatPhaseTutorialPanel:
		{
			if (this.m_activation != ActivatableUI.ActivationAction.SetActive)
			{
				if (this.m_activation != ActivatableUI.ActivationAction.ToggleActive)
				{
					UITutorialFullscreenPanel.Get().ClearAllPanels();
					break;
				}
			}
			UITutorialPanel uitutorialPanel = UITutorialPanel.Get();
			
			uitutorialPanel.QueueAction(delegate()
				{
					UITutorialFullscreenPanel.Get().ShowCombatPhasePanel();
				});
			break;
		}
		case ActivatableUI.UIElement.FullScreenDashPhaseTutorialPanel:
		{
			if (this.m_activation != ActivatableUI.ActivationAction.SetActive)
			{
				if (this.m_activation != ActivatableUI.ActivationAction.ToggleActive)
				{
					UITutorialFullscreenPanel.Get().ClearAllPanels();
					break;
				}
			}
			UITutorialPanel uitutorialPanel2 = UITutorialPanel.Get();
			
			uitutorialPanel2.QueueAction(delegate()
				{
					UITutorialFullscreenPanel.Get().ShowDashPhasePanel();
				});
			break;
		}
		case ActivatableUI.UIElement.FullScreenPrepPhaseTutorialPanel:
		{
			if (this.m_activation != ActivatableUI.ActivationAction.SetActive)
			{
				if (this.m_activation != ActivatableUI.ActivationAction.ToggleActive)
				{
					UITutorialFullscreenPanel.Get().ClearAllPanels();
					break;
				}
			}
			UITutorialPanel uitutorialPanel3 = UITutorialPanel.Get();
			
			uitutorialPanel3.QueueAction(delegate()
				{
					UITutorialFullscreenPanel.Get().ShowPrepPhasePanel();
				});
			break;
		}
		case ActivatableUI.UIElement.LockInButtonTutorialTipImage:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInTutorialTipSubImage.gameObject;
			break;
		case ActivatableUI.UIElement.LockInButtonTutorialTipText:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInTutorialTip.GetComponent<UITextSizer>().m_border.gameObject;
			break;
		case ActivatableUI.UIElement.LockinPhaseDisplay:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInCancelButton.m_phaseMarkerContainer.gameObject;
			break;
		case ActivatableUI.UIElement.LockinPhaseTextDisplay:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInCancelButton.m_phaseLabelContainer.gameObject;
			break;
		case ActivatableUI.UIElement.LockinPhaseColorDisplay:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_lockInCancelButton.m_phaseColor.gameObject;
			break;
		case ActivatableUI.UIElement.CardButtonTutorialTip:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[0].m_tutorialTip.gameObject;
			eventType = GameEventManager.EventType.UITutorialHighlightChanged;
			actionType = AbilityData.ActionType.CARD_0;
			break;
		case ActivatableUI.UIElement.CardButtonTutorialTip1:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[1].m_tutorialTip.gameObject;
			eventType = GameEventManager.EventType.UITutorialHighlightChanged;
			actionType = AbilityData.ActionType.CARD_1;
			break;
		case ActivatableUI.UIElement.CardButtonTutorialTip2:
			gameObject = HUD_UI.Get().m_mainScreenPanel.m_cardBar.m_cardButtons[2].m_tutorialTip.gameObject;
			eventType = GameEventManager.EventType.UITutorialHighlightChanged;
			actionType = AbilityData.ActionType.CARD_2;
			break;
		case ActivatableUI.UIElement.StatusEffectTutorialPanel:
			if (this.m_activation != ActivatableUI.ActivationAction.SetActive)
			{
				if (this.m_activation != ActivatableUI.ActivationAction.ToggleActive)
				{
					UITutorialFullscreenPanel.Get().ClearAllPanels();
					break;
				}
			}
			UITutorialFullscreenPanel.Get().ShowStatusEffectPanel();
			break;
		case ActivatableUI.UIElement.TeammateTargetingTutorialPanel:
			if (this.m_activation != ActivatableUI.ActivationAction.SetActive)
			{
				if (this.m_activation != ActivatableUI.ActivationAction.ToggleActive)
				{
					UITutorialFullscreenPanel.Get().ClearAllPanels();
					break;
				}
			}
			UITutorialFullscreenPanel.Get().ShowTeammateTargetingPanel();
			break;
		case ActivatableUI.UIElement.EnergyAndUltimatesTutorialPanel:
			if (this.m_activation != ActivatableUI.ActivationAction.SetActive)
			{
				if (this.m_activation != ActivatableUI.ActivationAction.ToggleActive)
				{
					UITutorialFullscreenPanel.Get().ClearAllPanels();
					break;
				}
			}
			UITutorialFullscreenPanel.Get().ShowEnergyAndUltimatesPanel();
			break;
		case ActivatableUI.UIElement.FadeOutPanel:
			if (this.m_activation != ActivatableUI.ActivationAction.SetActive)
			{
				if (this.m_activation != ActivatableUI.ActivationAction.ToggleActive)
				{
					UITutorialFullscreenPanel.Get().FadeIn();
					break;
				}
			}
			UITutorialFullscreenPanel.Get().FadeOut();
			break;
		case ActivatableUI.UIElement.ResetMatchTimer:
			if (this.m_activation != ActivatableUI.ActivationAction.SetActive)
			{
				if (this.m_activation != ActivatableUI.ActivationAction.ToggleActive)
				{
					break;
				}
			}
			UITimerPanel.Get().SetMatchTime(0f);
			break;
		case ActivatableUI.UIElement.ResetMatchTurn:
			if (this.m_activation != ActivatableUI.ActivationAction.SetActive)
			{
				if (this.m_activation != ActivatableUI.ActivationAction.ToggleActive)
				{
					break;
				}
			}
			UITimerPanel.Get().SetMatchTurn(1);
			break;
		}
		if (gameObject != null)
		{
			bool active = false;
			if (this.m_activation == ActivatableUI.ActivationAction.SetActive)
			{
				UIManager.SetGameObjectActive(gameObject, true, null);
				active = true;
			}
			else if (this.m_activation == ActivatableUI.ActivationAction.ClearActive)
			{
				UIManager.SetGameObjectActive(gameObject, false, null);
				active = false;
			}
			else if (this.m_activation == ActivatableUI.ActivationAction.ToggleActive)
			{
				active = !gameObject.activeSelf;
				UIManager.SetGameObjectActive(gameObject, !gameObject.activeSelf, null);
			}
			if (actionType != AbilityData.ActionType.INVALID_ACTION)
			{
				GameEventManager.ActivationInfo activationInfo = new GameEventManager.ActivationInfo();
				activationInfo.actionType = actionType;
				activationInfo.active = active;
				GameEventManager.Get().FireEvent(eventType, activationInfo);
			}
			if (this.m_uiElement == ActivatableUI.UIElement.DecisionTimer)
			{
				if (SinglePlayerManager.Get())
				{
					SinglePlayerManager.Get().SetDecisionTimerForceOff(!gameObject.activeSelf);
				}
			}
			else if (this.m_uiElement == ActivatableUI.UIElement.LockInCancelButton)
			{
				if (SinglePlayerManager.Get())
				{
					SinglePlayerManager.Get().SetLockInCancelButtonForceOff(!gameObject.activeSelf);
				}
			}
			else if (this.m_uiElement == ActivatableUI.UIElement.NotificationPanel)
			{
				if (SinglePlayerManager.Get())
				{
					SinglePlayerManager.Get().SetNotificationPanelForceOff(!gameObject.activeSelf);
				}
			}
			else if (this.m_uiElement == ActivatableUI.UIElement.LockinPhaseDisplay)
			{
				if (SinglePlayerManager.Get())
				{
					SinglePlayerManager.Get().SetLockinPhaseDisplayForceOff(!gameObject.activeSelf);
				}
			}
			else if (this.m_uiElement == ActivatableUI.UIElement.LockinPhaseTextDisplay)
			{
				if (SinglePlayerManager.Get())
				{
					SinglePlayerManager.Get().SetLockinPhaseTextForceOff(!gameObject.activeSelf);
				}
			}
			else if (this.m_uiElement == ActivatableUI.UIElement.LockinPhaseColorDisplay)
			{
				if (SinglePlayerManager.Get())
				{
					SinglePlayerManager.Get().SetLockinPhaseColorForceOff(!gameObject.activeSelf);
				}
			}
			else if (this.m_uiElement == ActivatableUI.UIElement.TopDisplayPanelPlayerStatus)
			{
				if (SinglePlayerManager.Get())
				{
					SinglePlayerManager.Get().SetTeamPlayerIconForceOff(0, !gameObject.activeSelf);
				}
			}
			else if (this.m_uiElement == ActivatableUI.UIElement.TopDisplayPanelPlayerStatus1)
			{
				if (SinglePlayerManager.Get())
				{
					SinglePlayerManager.Get().SetTeamPlayerIconForceOff(1, !gameObject.activeSelf);
				}
			}
			else if (this.m_uiElement == ActivatableUI.UIElement.TopDisplayPanelPlayerStatus2)
			{
				if (SinglePlayerManager.Get())
				{
					SinglePlayerManager.Get().SetTeamPlayerIconForceOff(2, !gameObject.activeSelf);
				}
			}
			else if (this.m_uiElement == ActivatableUI.UIElement.TopDisplayPanelPlayerStatus3)
			{
				if (SinglePlayerManager.Get())
				{
					SinglePlayerManager.Get().SetEnemyPlayerIconForceOff(0, !gameObject.activeSelf);
				}
			}
			else if (this.m_uiElement == ActivatableUI.UIElement.TopDisplayPanelPlayerStatus4)
			{
				if (SinglePlayerManager.Get())
				{
					SinglePlayerManager.Get().SetEnemyPlayerIconForceOff(1, !gameObject.activeSelf);
				}
			}
			else if (this.m_uiElement == ActivatableUI.UIElement.TopDisplayPanelPlayerStatus5)
			{
				if (SinglePlayerManager.Get())
				{
					SinglePlayerManager.Get().SetEnemyPlayerIconForceOff(2, !gameObject.activeSelf);
				}
			}
		}
	}

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
}
