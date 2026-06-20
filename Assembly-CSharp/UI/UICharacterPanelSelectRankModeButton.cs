using UnityEngine.EventSystems;

public class UICharacterPanelSelectRankModeButton : UICharacterPanelSelectButton
{
	public override void SetEnabled(bool enabled, PersistedCharacterData playerCharacterData)
	{
		base.SetEnabled(enabled, playerCharacterData);
		UIManager.SetGameObjectActive(m_MasterExpBarContainer, false);
		UIManager.SetGameObjectActive(m_NormalExpBarContainer, false);
		UIManager.SetGameObjectActive(m_UnavailableExpBarContainer, false);
	}

	public override void Setup(bool isAvailable, bool selected = false)
	{
		base.Setup(isAvailable, selected);
		UIManager.SetGameObjectActive(m_MasterExpBarContainer, false);
	}

	protected override FrontEndButtonSounds SoundToPlayOnClick()
	{
		return FrontEndButtonSounds.RankFreelancerClick;
	}

	protected override void OnButtonClicked(BaseEventData data)
	{
		if (!m_button.spriteController.IsClickable())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (m_isDisabled)
		{
			return;
		}
		while (true)
		{
			UIRankedModeDraftScreen.Get().NotifyButtonClicked(this);
			return;
		}
	}
}
