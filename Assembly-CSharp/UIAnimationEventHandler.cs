using System;
using UnityEngine;

public class UIAnimationEventHandler : MonoBehaviour
{
	public void NotifyAnimationEvent(UIAnimationEventHandler.UIAnimationEventType eventType)
	{
		switch (eventType)
		{
		case UIAnimationEventHandler.UIAnimationEventType.GGButtonPressedFadeDone:
			HUD_UI.Get().m_mainScreenPanel.m_characterProfile.GGPackAnimDone();
			break;
		case UIAnimationEventHandler.UIAnimationEventType.GGPackNotificationAnimFadeOutDone:
			HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.GGPackAnimListOutAnimDone();
			break;
		case UIAnimationEventHandler.UIAnimationEventType.RewardAnimationDonePlaying:
			UINewReward.Get().AutoPlayNextAnimation();
			break;
		case UIAnimationEventHandler.UIAnimationEventType.TempAnimEvent:
			Log.Warning("Temporary Animation Event called!", new object[0]);
			break;
		case UIAnimationEventHandler.UIAnimationEventType.ChararacterSelectReadyCancelButtonDone:
			UICharacterSelectScreenController.Get().CancelButtonAnimDone();
			break;
		case UIAnimationEventHandler.UIAnimationEventType.SystemMessagePanelDone:
			if (SystemMenuBroadcast.Get() != null)
			{
				SystemMenuBroadcast.Get().NotifySystemMessageOutDone();
			}
			break;
		case UIAnimationEventHandler.UIAnimationEventType.StatusQueueAnimDone:
			NavigationBar.Get().NotifyStatusQueueAnimDone();
			break;
		case UIAnimationEventHandler.UIAnimationEventType.GameOverRewardIconSwapEvent:
			if (UIGameOverScreen.Get() != null)
			{
			}
			break;
		case UIAnimationEventHandler.UIAnimationEventType.NanoChestOpenFinished:
			if (UILootMatrixScreen.Get() != null)
			{
				UILootMatrixScreen.Get().DoOpenChestAnimationEvent();
			}
			break;
		case UIAnimationEventHandler.UIAnimationEventType.NanoChestRewardFinished:
			if (UILootMatrixScreen.Get() != null)
			{
				UILootMatrixScreen.Get().FinishRewardAnimation();
			}
			break;
		}
	}

	public void PlaySound(FrontEndButtonSounds sound)
	{
		UIFrontEnd.PlaySound(sound);
	}

	public enum UIAnimationEventType
	{
		GGButtonPressedFadeDone,
		GGPackNotificationAnimFadeOutDone,
		GGPackBonusGameOverPanelAnimStartIncrement,
		RewardAnimationDonePlaying,
		TempAnimEvent,
		ChararacterSelectReadyCancelButtonDone,
		SystemMessagePanelDone,
		StatusQueueAnimDone,
		GameOverRewardIconSwapEvent,
		NanoChestOpenFinished,
		NanoChestRewardFinished
	}
}
