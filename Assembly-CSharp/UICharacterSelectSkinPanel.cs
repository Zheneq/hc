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
			ClientSelectedCharacter = selectedCharacter.m_characterType,
			ClientSelectedVisualInfo = selectedVisualInfo
		});
		if (!isUnlocked)
		{
			return;
		}
		while (true)
		{
			m_lastUnlockedData = selectedVisualInfo;
			return;
		}
	}

	public void OnColorSelect(Color UIColorDisplay)
	{
		UICharacterScreen.Get().m_selectedSkinColor.color = UIColorDisplay;
	}

	public void OnDisabled(UISkinBrowserPanel browserPanel)
	{
		if (!UICharacterScreen.GetCurrentSpecificState().ClientSelectedVisualInfo.HasValue)
		{
			return;
		}
		while (true)
		{
			if (GetCharacterVisualInfo().Equals(m_lastUnlockedData))
			{
				return;
			}
			while (true)
			{
				browserPanel.Select(m_lastUnlockedData);
				if (UICharacterSelectScreenController.Get().IsCharacterSelectable(GetSelectedCharacter()))
				{
					UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(GetSelectedCharacter(), 0, string.Empty, m_lastUnlockedData, false, true);
				}
				else
				{
					UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(GetSelectedCharacter(), 0, string.Empty, default(CharacterVisualInfo), false, true);
				}
				return;
			}
		}
	}

	public void OnSkinClick(UISkinBrowserPanel browserPanel, CharacterResourceLink selectedCharacter, CharacterVisualInfo selectedVisualInfo, bool isUnlocked)
	{
		selectedVisualInfo = ValidateVisualInfo(selectedCharacter.m_characterType, selectedVisualInfo);
		int num;
		if (!selectedCharacter.HasAudioEventReplacements(selectedVisualInfo))
		{
			num = ((UIFrontEnd.GetVisibleCharacters() != null && selectedCharacter.HasAudioEventReplacements(UIFrontEnd.GetVisibleCharacters().CharacterVisualInfoInSlot(0))) ? 1 : 0);
		}
		else
		{
			num = 1;
		}
		bool playSelectionChatterCue = (byte)num != 0;
		UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(selectedCharacter, 0, string.Empty, selectedVisualInfo, false, playSelectionChatterCue);
		if (!isUnlocked)
		{
			return;
		}
		while (true)
		{
			AppState_CharacterSelect.Get().UpdateSelectedSkin(selectedVisualInfo);
			return;
		}
	}

	public void OnColorClick(UISkinBrowserPanel browserPanel, CharacterResourceLink selectedCharacter, CharacterVisualInfo selectedVisualInfo, bool isUnlocked)
	{
		selectedVisualInfo = ValidateVisualInfo(selectedCharacter.m_characterType, selectedVisualInfo);
		UICharacterSelectWorldObjects.Get().LoadCharacterIntoSlot(selectedCharacter, 0, string.Empty, selectedVisualInfo, false, false);
		if (!isUnlocked)
		{
			return;
		}
		while (true)
		{
			int playerCharacterLevel = ClientGameManager.Get().GetPlayerCharacterLevel(selectedCharacter.m_characterType);
			int requiredLevelForEquip = selectedCharacter.m_skins[selectedVisualInfo.skinIndex].m_patterns[selectedVisualInfo.patternIndex].m_colors[selectedVisualInfo.colorIndex].m_requiredLevelForEquip;
			if (playerCharacterLevel >= requiredLevelForEquip)
			{
				AppState_CharacterSelect.Get().UpdateSelectedSkin(selectedVisualInfo);
			}
			return;
		}
	}

	private CharacterVisualInfo ValidateVisualInfo(CharacterType charType, CharacterVisualInfo visualInfo)
	{
		if (GameManager.Get() == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return new CharacterVisualInfo(0, 0, 0);
				}
			}
		}
		LobbyGameplayOverrides gameplayOverrides = GameManager.Get().GameplayOverrides;
		if (!gameplayOverrides.IsColorAllowed(charType, visualInfo.skinIndex, visualInfo.patternIndex, visualInfo.colorIndex))
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return new CharacterVisualInfo(0, 0, 0);
				}
			}
		}
		GameBalanceVars.CharacterUnlockData characterUnlockData = GameBalanceVars.Get().GetCharacterUnlockData(charType);
		bool flag2;
		GameBalanceVars.ColorUnlockData colorUnlockData;
		if (visualInfo.skinIndex < characterUnlockData.skinUnlockData.Length && visualInfo.patternIndex < characterUnlockData.skinUnlockData[visualInfo.skinIndex].patternUnlockData.Length)
		{
			if (visualInfo.colorIndex < characterUnlockData.skinUnlockData[visualInfo.skinIndex].patternUnlockData[visualInfo.patternIndex].colorUnlockData.Length)
			{
				if (gameplayOverrides.EnableHiddenCharacters)
				{
					while (true)
					{
						return visualInfo;
					}
				}
				bool flag = false;
				flag2 = false;
				if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
					CharacterComponent characterComponent = ClientGameManager.Get().GetPlayerCharacterData(charType).CharacterComponent;
					flag = characterComponent.Skins[visualInfo.skinIndex].Unlocked;
					flag2 = characterComponent.Skins[visualInfo.skinIndex].Patterns[visualInfo.patternIndex].Colors[visualInfo.colorIndex].Unlocked;
				}
				GameBalanceVars.SkinUnlockData skinUnlockData = characterUnlockData.skinUnlockData[visualInfo.skinIndex];
				GameBalanceVars.PatternUnlockData patternUnlockData = skinUnlockData.patternUnlockData[visualInfo.patternIndex];
				colorUnlockData = patternUnlockData.colorUnlockData[visualInfo.colorIndex];
				if (flag)
				{
					goto IL_0204;
				}
				if (!skinUnlockData.m_isHidden)
				{
					if (GameBalanceVarsExtensions.MeetsVisibilityConditions(skinUnlockData))
					{
						goto IL_0204;
					}
				}
				goto IL_0239;
			}
		}
		return new CharacterVisualInfo(0, 0, 0);
		IL_0204:
		if (!flag2)
		{
			if (!colorUnlockData.m_isHidden)
			{
				if (GameBalanceVarsExtensions.MeetsVisibilityConditions(colorUnlockData))
				{
					goto IL_0242;
				}
			}
			goto IL_0239;
		}
		goto IL_0242;
		IL_0239:
		return new CharacterVisualInfo(0, 0, 0);
		IL_0242:
		return visualInfo;
	}
}
