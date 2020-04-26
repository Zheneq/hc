using UnityEngine;

public interface ISkinBrowserSelectHandler
{
	void OnStart(UISkinBrowserPanel browserPanel);

	void OnDestroy(UISkinBrowserPanel browserPanel);

	void OnSelect(UISkinBrowserPanel browserPanel, CharacterResourceLink selectedCharacter, CharacterVisualInfo selectedVisualInfo, bool isUnlocked);

	void OnDisabled(UISkinBrowserPanel browserPanel);

	void OnColorSelect(Color UIColorDisplay);

	void OnSkinClick(UISkinBrowserPanel browserPanel, CharacterResourceLink selectedCharacter, CharacterVisualInfo selectedVisualInfo, bool isUnlocked);

	void OnColorClick(UISkinBrowserPanel browserPanel, CharacterResourceLink selectedCharacter, CharacterVisualInfo selectedVisualInfo, bool isUnlocked);
}
