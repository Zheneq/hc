using System;
using UnityEngine.EventSystems;

public class UICharacterPanelSelectRankModeButton : UICharacterPanelSelectButton
{
	public override void SetEnabled(bool enabled, PersistedCharacterData playerCharacterData)
	{
		base.SetEnabled(enabled, playerCharacterData);
		UIManager.SetGameObjectActive(this.m_MasterExpBarContainer, false, null);
		UIManager.SetGameObjectActive(this.m_NormalExpBarContainer, false, null);
		UIManager.SetGameObjectActive(this.m_UnavailableExpBarContainer, false, null);
	}

	public override void Setup(bool isAvailable, bool selected = false)
	{
		base.Setup(isAvailable, selected);
		UIManager.SetGameObjectActive(this.m_MasterExpBarContainer, false, null);
	}

	protected override FrontEndButtonSounds SoundToPlayOnClick()
	{
		return FrontEndButtonSounds.RankFreelancerClick;
	}

	protected override void OnButtonClicked(BaseEventData data)
	{
		if (!this.m_button.spriteController.IsClickable())
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UICharacterPanelSelectRankModeButton.OnButtonClicked(BaseEventData)).MethodHandle;
			}
			return;
		}
		if (!this.m_isDisabled)
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
			UIRankedModeDraftScreen.Get().NotifyButtonClicked(this);
		}
	}
}
