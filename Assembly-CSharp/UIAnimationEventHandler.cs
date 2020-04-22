using UnityEngine;

public class UIAnimationEventHandler : MonoBehaviour
{
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

	public void NotifyAnimationEvent(UIAnimationEventType eventType)
	{
		switch (eventType)
		{
		case UIAnimationEventType.GGPackBonusGameOverPanelAnimStartIncrement:
			break;
		case UIAnimationEventType.GGButtonPressedFadeDone:
			HUD_UI.Get().m_mainScreenPanel.m_characterProfile.GGPackAnimDone();
			break;
		case UIAnimationEventType.GGPackNotificationAnimFadeOutDone:
			HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.GGPackAnimListOutAnimDone();
			break;
		case UIAnimationEventType.RewardAnimationDonePlaying:
			UINewReward.Get().AutoPlayNextAnimation();
			break;
		case UIAnimationEventType.ChararacterSelectReadyCancelButtonDone:
			UICharacterSelectScreenController.Get().CancelButtonAnimDone();
			break;
		case UIAnimationEventType.SystemMessagePanelDone:
			if (!(SystemMenuBroadcast.Get() != null))
			{
				break;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				SystemMenuBroadcast.Get().NotifySystemMessageOutDone();
				return;
			}
		case UIAnimationEventType.StatusQueueAnimDone:
			NavigationBar.Get().NotifyStatusQueueAnimDone();
			break;
		case UIAnimationEventType.GameOverRewardIconSwapEvent:
			if (UIGameOverScreen.Get() != null)
			{
				while (true)
				{
					switch (6)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			break;
		case UIAnimationEventType.NanoChestOpenFinished:
			if (!(UILootMatrixScreen.Get() != null))
			{
				break;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				UILootMatrixScreen.Get().DoOpenChestAnimationEvent();
				return;
			}
		case UIAnimationEventType.NanoChestRewardFinished:
			if (!(UILootMatrixScreen.Get() != null))
			{
				break;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				UILootMatrixScreen.Get().FinishRewardAnimation();
				return;
			}
		case UIAnimationEventType.TempAnimEvent:
			Log.Warning("Temporary Animation Event called!");
			break;
		}
	}

	public void PlaySound(FrontEndButtonSounds sound)
	{
		UIFrontEnd.PlaySound(sound);
	}
}
