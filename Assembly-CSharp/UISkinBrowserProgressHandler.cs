using UnityEngine;

public class UISkinBrowserProgressHandler : ISkinBrowserSelectHandler
{
	public void OnStart(UISkinBrowserPanel browserPanel)
	{
	}

	public void OnDestroy(UISkinBrowserPanel browserPanel)
	{
	}

	public void OnSelect(UISkinBrowserPanel browserPanel, CharacterResourceLink selectedCharacter, CharacterVisualInfo selectedVisualInfo, bool isUnlocked)
	{
		if (!UICashShopPanel.Get().IsVisible() || UICashShopPanel.Get().m_characterBrowser.GetCharacterType() != selectedCharacter.m_characterType)
		{
			return;
		}
		while (true)
		{
			UICharacterStoreAndProgressWorldObjects.Get().LoadCharacterIntoSlot(selectedCharacter, 0, string.Empty, selectedVisualInfo, false, true);
			return;
		}
	}

	public void OnDisabled(UISkinBrowserPanel browserPanel)
	{
	}

	public void OnColorSelect(Color UIColorDisplay)
	{
	}

	public void OnSkinClick(UISkinBrowserPanel browserPanel, CharacterResourceLink selectedCharacter, CharacterVisualInfo selectedVisualInfo, bool isUnlocked)
	{
	}

	public void OnColorClick(UISkinBrowserPanel browserPanel, CharacterResourceLink selectedCharacter, CharacterVisualInfo selectedVisualInfo, bool isUnlocked)
	{
	}
}
