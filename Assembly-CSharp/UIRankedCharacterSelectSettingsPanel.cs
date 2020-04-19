using System;

public class UIRankedCharacterSelectSettingsPanel : UICharacterSelectCharacterSettingsPanel
{
	protected override FrontEndButtonSounds GetTabClickSound()
	{
		return FrontEndButtonSounds.RankFreelancerSettingTab;
	}

	protected override void Awake()
	{
		if (this.m_abilitiesSubPanel.m_modloadouts != null)
		{
			UIManager.SetGameObjectActive(this.m_abilitiesSubPanel.m_modloadouts.transform.parent, true, null);
		}
		base.Awake();
	}

	public void Refresh()
	{
		if (UIRankedModeDraftScreen.Get().IsVisible)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedCharacterSelectSettingsPanel.Refresh()).MethodHandle;
			}
			if (UIRankedModeDraftScreen.Get().SelectedCharacter != CharacterType.None)
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
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(UIRankedModeDraftScreen.Get().SelectedCharacter);
				base.Refresh(characterResourceLink, false, false);
			}
		}
	}

	public void UpdateSelectedCharType(CharacterType charType)
	{
		this.m_selectedCharType = charType;
	}

	public new static UIRankedCharacterSelectSettingsPanel Get()
	{
		if (UIRankedModeDraftScreen.Get() != null)
		{
			return UIRankedModeDraftScreen.Get().m_rankedModeCharacterSettings;
		}
		return null;
	}

	protected override void DoVisible(bool visible, UICharacterSelectCharacterSettingsPanel.TabPanel tab = UICharacterSelectCharacterSettingsPanel.TabPanel.None)
	{
		this.m_isVisible = visible;
		float alpha = 0f;
		if (visible)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedCharacterSelectSettingsPanel.DoVisible(bool, UICharacterSelectCharacterSettingsPanel.TabPanel)).MethodHandle;
			}
			base.OpenTab(tab, true);
			alpha = 1f;
		}
		UIManager.SetGameObjectActive(this, visible, null);
		this.m_CanvasGroup.alpha = alpha;
		this.m_CanvasGroup.interactable = visible;
		this.m_CanvasGroup.blocksRaycasts = visible;
	}
}
