using System;
using UnityEngine;

public class UICharacterSelectSkinPanel : ISkinBrowserSelectHandler
{
	private CharacterVisualInfo m_lastUnlockedData;

	public CharacterResourceLink GetSelectedCharacter()
	{
		GameWideData gameWideData = GameWideData.Get();
		CharacterType? clientSelectedCharacter = UICharacterScreen.GetCurrentSpecificState().ClientSelectedCharacter;
		return gameWideData.GetCharacterResourceLink(clientSelectedCharacter.Value);
	}

	public CharacterVisualInfo GetCharacterVisualInfo()
	{
		CharacterVisualInfo? clientSelectedVisualInfo = UICharacterScreen.GetCurrentSpecificState().ClientSelectedVisualInfo;
		return clientSelectedVisualInfo.Value;
	}

	public void OnStart(UISkinBrowserPanel browserPanel)
	{
	}

	public void OnDestroy(UISkinBrowserPanel browserPanel)
	{
	}

	public void OnSelect(UISkinBrowserPanel browserPanel, CharacterResourceLink selectedCharacter, CharacterVisualInfo selectedVisualInfo, bool isUnlocked)
	{
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			ClientSelectedCharacter = new CharacterType?(selectedCharacter.m_characterType),
			ClientSelectedVisualInfo = new CharacterVisualInfo?(selectedVisualInfo)
		});
		if (isUnlocked)
		{
			this.m_lastUnlockedData = selectedVisualInfo;
		}
	}

	public void OnColorSelect(Color UIColorDisplay)
	{
		UICharacterScreen.Get().m_selectedSkinColor.color = UIColorDisplay;
	}

	public void OnDisabled(UISkinBrowserPanel browserPanel)
	{
		if (UICharacterScreen.GetCurrentSpecificState().ClientSelectedVisualInfo != null)
		{
			if (!this.GetCharacterVisualInfo().Equals(this.m_lastUnlockedData))
			{
				browserPanel.Select(this.m_lastUnlockedData);
				if (UICharacterSelectScreenController.Get().IsCharacterSelectable(this.GetSelectedCharacter()))
				{
					UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(this.GetSelectedCharacter(), 0, string.Empty, this.m_lastUnlockedData, false, true);
				}
				else
				{
					UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(this.GetSelectedCharacter(), 0, string.Empty, default(CharacterVisualInfo), false, true);
				}
			}
		}
	}

	public void OnSkinClick(UISkinBrowserPanel browserPanel, CharacterResourceLink selectedCharacter, CharacterVisualInfo selectedVisualInfo, bool isUnlocked)
	{
		selectedVisualInfo = this.ValidateVisualInfo(selectedCharacter.m_characterType, selectedVisualInfo);
		bool flag;
		if (!selectedCharacter.HasAudioEventReplacements(selectedVisualInfo))
		{
			flag = (UIFrontEnd.GetVisibleCharacters() != null && selectedCharacter.HasAudioEventReplacements(UIFrontEnd.GetVisibleCharacters().CharacterVisualInfoInSlot(0)));
		}
		else
		{
			flag = true;
		}
		bool playSelectionChatterCue = flag;
		UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(selectedCharacter, 0, string.Empty, selectedVisualInfo, false, playSelectionChatterCue);
		if (isUnlocked)
		{
			AppState_CharacterSelect.Get().UpdateSelectedSkin(selectedVisualInfo);
		}
	}

	public void OnColorClick(UISkinBrowserPanel browserPanel, CharacterResourceLink selectedCharacter, CharacterVisualInfo selectedVisualInfo, bool isUnlocked)
	{
		selectedVisualInfo = this.ValidateVisualInfo(selectedCharacter.m_characterType, selectedVisualInfo);
		UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(selectedCharacter, 0, string.Empty, selectedVisualInfo, false, false);
		if (isUnlocked)
		{
			int playerCharacterLevel = ClientGameManager.Get().GetPlayerCharacterLevel(selectedCharacter.m_characterType);
			int requiredLevelForEquip = selectedCharacter.m_skins[selectedVisualInfo.skinIndex].m_patterns[selectedVisualInfo.patternIndex].m_colors[selectedVisualInfo.colorIndex].m_requiredLevelForEquip;
			if (playerCharacterLevel >= requiredLevelForEquip)
			{
				AppState_CharacterSelect.Get().UpdateSelectedSkin(selectedVisualInfo);
			}
		}
	}

	private CharacterVisualInfo ValidateVisualInfo(CharacterType charType, CharacterVisualInfo visualInfo)
	{
		if (GameManager.Get() == null)
		{
			return new CharacterVisualInfo(0, 0, 0);
		}
		LobbyGameplayOverrides gameplayOverrides = GameManager.Get().GameplayOverrides;
		if (!gameplayOverrides.IsColorAllowed(charType, visualInfo.skinIndex, visualInfo.patternIndex, visualInfo.colorIndex))
		{
			return new CharacterVisualInfo(0, 0, 0);
		}
		GameBalanceVars.CharacterUnlockData characterUnlockData = GameBalanceVars.Get().GetCharacterUnlockData(charType);
		if (visualInfo.skinIndex < characterUnlockData.skinUnlockData.Length && visualInfo.patternIndex < characterUnlockData.skinUnlockData[visualInfo.skinIndex].patternUnlockData.Length)
		{
			if (visualInfo.colorIndex >= characterUnlockData.skinUnlockData[visualInfo.skinIndex].patternUnlockData[visualInfo.patternIndex].colorUnlockData.Length)
			{
			}
			else
			{
				if (gameplayOverrides.EnableHiddenCharacters)
				{
					return visualInfo;
				}
				bool flag = false;
				bool flag2 = false;
				if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
					CharacterComponent characterComponent = ClientGameManager.Get().GetPlayerCharacterData(charType).CharacterComponent;
					flag = characterComponent.Skins[visualInfo.skinIndex].Unlocked;
					flag2 = characterComponent.Skins[visualInfo.skinIndex].Patterns[visualInfo.patternIndex].Colors[visualInfo.colorIndex].Unlocked;
				}
				GameBalanceVars.SkinUnlockData skinUnlockData = characterUnlockData.skinUnlockData[visualInfo.skinIndex];
				GameBalanceVars.PatternUnlockData patternUnlockData = skinUnlockData.patternUnlockData[visualInfo.patternIndex];
				GameBalanceVars.ColorUnlockData colorUnlockData = patternUnlockData.colorUnlockData[visualInfo.colorIndex];
				if (!flag)
				{
					if (skinUnlockData.m_isHidden)
					{
						goto IL_239;
					}
					if (!GameBalanceVarsExtensions.MeetsVisibilityConditions(skinUnlockData))
					{
						goto IL_239;
					}
				}
				if (!flag2)
				{
					if (colorUnlockData.m_isHidden)
					{
						goto IL_239;
					}
					if (!GameBalanceVarsExtensions.MeetsVisibilityConditions(colorUnlockData))
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							goto IL_239;
						}
					}
				}
				return visualInfo;
				IL_239:
				return new CharacterVisualInfo(0, 0, 0);
			}
		}
		return new CharacterVisualInfo(0, 0, 0);
	}
}
