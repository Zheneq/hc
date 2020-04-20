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
			return;
		}
		if (!this.m_isDisabled)
		{
			UIRankedModeDraftScreen.Get().NotifyButtonClicked(this);
		}
	}
}
