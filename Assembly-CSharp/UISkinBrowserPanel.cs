using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkinBrowserPanel : MonoBehaviour
{
	public enum HandlerType
	{
		None,
		CharacterSelect,
		CashShop
	}

	public UISkinSelectButton m_skinButtonPrefab;

	public GridLayoutGroup m_skinGrid;

	public UIButtonColorSelect m_colorButtonPrefab;

	public GridLayoutGroup m_colorGrid;

	public HandlerType m_handlerType;

	public UIPurchasePanel m_purchasePanel;

	public GameObject m_warningContainer;

	public TextMeshProUGUI m_warningText;

	public bool m_characterDataListenerInitialized;

	public ISkinBrowserSelectHandler m_selectHandler;

	private List<UISkinSelectButton> m_skinButtons = new List<UISkinSelectButton>();

	private List<UIButtonColorSelect> m_colorButtons = new List<UIButtonColorSelect>();

	private CharacterResourceLink m_currentCharacter;

	private CharacterVisualInfo m_currentSelection;

	private CharacterVisualInfo m_displayedVisualInfo;

	private bool m_skinLocked;

	private bool m_colorLocked;

	private PurchaseItemType m_purchaseType;

	private void Awake()
	{
		if (m_handlerType == HandlerType.CharacterSelect)
		{
			m_selectHandler = new UICharacterSelectSkinPanel();
		}
		else if (m_handlerType == HandlerType.CashShop)
		{
			m_selectHandler = new UISkinBrowserProgressHandler();
		}
		if (!(HitchDetector.Get() != null))
		{
			return;
		}
		while (true)
		{
			HitchDetector.Get().AddNewLayoutGroup(m_skinGrid);
			HitchDetector.Get().AddNewLayoutGroup(m_colorGrid);
			return;
		}
	}

	private void Start()
	{
		m_purchasePanel.m_isoButton.spriteController.callback = PurchaseWithIso;
		m_purchasePanel.m_freelancerCurrencyButton.spriteController.callback = PurchaseWithFreelancerCurrency;
		m_purchasePanel.m_realCurrencyButton.spriteController.callback = PurchaseWithRealCurrency;
		if (!m_characterDataListenerInitialized)
		{
			if (ClientGameManager.Get() != null)
			{
				ClientGameManager.Get().OnCharacterDataUpdated += OnCharacterDataUpdated;
				m_characterDataListenerInitialized = true;
			}
		}
		if (m_selectHandler == null)
		{
			return;
		}
		while (true)
		{
			m_selectHandler.OnStart(this);
			return;
		}
	}

	private void OnDestroy()
	{
		if (m_characterDataListenerInitialized)
		{
			if (ClientGameManager.Get() != null)
			{
				ClientGameManager.Get().OnCharacterDataUpdated -= OnCharacterDataUpdated;
				ClientGameManager.Get().OnLobbyGameplayOverridesChange -= OnLobbyGameplayOverridesUpdated;
			}
		}
		if (m_selectHandler == null)
		{
			return;
		}
		while (true)
		{
			m_selectHandler.OnDestroy(this);
			return;
		}
	}

	private void OnEnable()
	{
		if (!m_characterDataListenerInitialized)
		{
			if (ClientGameManager.Get() != null)
			{
				ClientGameManager.Get().OnCharacterDataUpdated += OnCharacterDataUpdated;
				ClientGameManager.Get().OnLobbyGameplayOverridesChange += OnLobbyGameplayOverridesUpdated;
				m_characterDataListenerInitialized = true;
			}
		}
		if (!UICharacterSelectWorldObjects.Get())
		{
			return;
		}
		while (true)
		{
			UICharacterSelectWorldObjects.Get().PlayCameraAnimation("CamCloseupOUT");
			return;
		}
	}

	private void OnDisable()
	{
		if (m_selectHandler != null)
		{
			m_selectHandler.OnDisabled(this);
		}
		UIManager.SetGameObjectActive(m_purchasePanel, false);
	}

	public void OnCharacterDataUpdated(PersistedCharacterData newData)
	{
		if (m_currentCharacter == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (newData.CharacterType != m_currentCharacter.m_characterType)
		{
			return;
		}
		while (true)
		{
			PopulateCharacterData(m_currentCharacter, newData);
			PlayerSkinData skin = newData.CharacterComponent.GetSkin(m_currentSelection.skinIndex);
			if (!skin.Unlocked)
			{
				if (!skin.GetPattern(m_currentSelection.patternIndex).GetColor(m_currentSelection.skinIndex).Unlocked)
				{
					return;
				}
			}
			UIManager.SetGameObjectActive(m_warningContainer, false);
			UIManager.SetGameObjectActive(m_purchasePanel, false);
			m_purchasePanel.SetDisabled(false);
			return;
		}
	}

	public void RefreshUI()
	{
		RePopulateCharacterData();
		AppState_CharacterSelect.Get().UpdateSelectedSkin(m_currentSelection);
	}

	public void OnLobbyGameplayOverridesUpdated(LobbyGameplayOverrides gameplayOverrides)
	{
		RefreshUI();
	}

	public CharacterVisualInfo GetCurrentSelection()
	{
		return m_currentSelection;
	}

	public void Setup(CharacterType characterType, CharacterVisualInfo visualInfo)
	{
		Setup(GameWideData.Get().GetCharacterResourceLink(characterType), visualInfo);
	}

	public CharacterType GetDisplayedCharacterType()
	{
		if (m_currentCharacter != null)
		{
			return m_currentCharacter.m_characterType;
		}
		return CharacterType.None;
	}

	public CharacterVisualInfo GetDisplayedVisualInfo()
	{
		return m_displayedVisualInfo;
	}

	public void Setup(CharacterResourceLink characterLink, CharacterVisualInfo visualInfo, bool sameCharacter = false)
	{
		if (m_currentCharacter != null)
		{
			if (m_currentCharacter.m_characterType == characterLink.m_characterType)
			{
				if (m_currentSelection.Equals(visualInfo))
				{
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_warningContainer, false);
		UIManager.SetGameObjectActive(m_purchasePanel, false);
		m_purchasePanel.SetDisabled(false);
		m_displayedVisualInfo = visualInfo;
		UIManager.SetGameObjectActive(m_warningContainer, false);
		UIManager.SetGameObjectActive(m_purchasePanel, false);
		m_purchasePanel.SetDisabled(false);
		if (m_skinButtons.Count == 0)
		{
			m_skinButtons.AddRange(m_skinGrid.GetComponentsInChildren<UISkinSelectButton>(true));
		}
		if (m_colorButtons.Count == 0)
		{
			m_colorButtons.AddRange(m_colorGrid.GetComponentsInChildren<UIButtonColorSelect>(true));
		}
		m_currentCharacter = characterLink;
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(characterLink.m_characterType);
		m_currentSelection = visualInfo;
		PopulateCharacterData(m_currentCharacter, playerCharacterData);
	}

	public void RePopulateCharacterData()
	{
		if (!(m_currentCharacter != null))
		{
			return;
		}
		while (true)
		{
			PopulateCharacterData(m_currentCharacter, ClientGameManager.Get().GetPlayerCharacterData(m_currentCharacter.m_characterType));
			List<UIPatternData> list = new List<UIPatternData>();
			int num = 0;
			using (List<UISkinSelectButton>.Enumerator enumerator = m_skinButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UISkinSelectButton current = enumerator.Current;
					if (current.isSelected)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
							{
								for (int i = 0; i < current.m_skinData.m_possiblePatterns.Length; i++)
								{
									UIPatternData item = current.m_skinData.m_possiblePatterns[i];
									if (item.m_isVisible)
									{
										list.Add(item);
									}
								}
								LoadPatternInfo(list, num, true);
								return;
							}
							}
						}
					}
					num++;
				}
				while (true)
				{
					switch (5)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	public void PopulateCharacterData(CharacterResourceLink selectedCharacter, PersistedCharacterData characterData)
	{
		if (selectedCharacter == null)
		{
			return;
		}
		while (true)
		{
			if (characterData == null)
			{
				while (true)
				{
					switch (5)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
			GameBalanceVars.CharacterUnlockData characterUnlockData = gameBalanceVars.GetCharacterUnlockData(selectedCharacter.m_characterType);
			LobbyGameplayOverrides gameplayOverrides = GameManager.Get().GameplayOverrides;
			int num = characterData.ExperienceComponent.Level + 1;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			UISkinSelectButton uISkinSelectButton = null;
			int i = 0;
			while (i < selectedCharacter.m_skins.Count)
			{
				GameBalanceVars.SkinUnlockData skinUnlockData = null;
				if (characterUnlockData != null)
				{
					if (i < characterUnlockData.skinUnlockData.Length)
					{
						skinUnlockData = characterUnlockData.skinUnlockData[i];
					}
				}
				CharacterSkin characterSkin = selectedCharacter.m_skins[i];
				PlayerSkinData skin = characterData.CharacterComponent.GetSkin(i);
				int num8;
				if (!characterSkin.m_isHidden)
				{
					num8 = ((!GameBalanceVarsExtensions.MeetsVisibilityConditions(skinUnlockData)) ? 1 : 0);
				}
				else
				{
					num8 = 1;
				}
				int num9;
				if (num8 != 0)
				{
					num9 = (skin.Unlocked ? 1 : 0);
				}
				else
				{
					num9 = 1;
				}
				bool flag = (byte)num9 != 0;
				if (!flag)
				{
					num5++;
				}
				UISkinData uISkinData = default(UISkinData);
				uISkinData.m_skinImage = (Sprite)Resources.Load(characterSkin.m_skinSelectionIconPath, typeof(Sprite));
				UISkinData skinData = uISkinData;
				skinData.m_gameCurrencyCost = 0;
				skinData.m_isAvailable = skin.Unlocked;
				skinData.m_isVisible = flag;
				if (skinUnlockData != null)
				{
					skinData.m_unlockCharacterLevel = skinUnlockData.GetUnlockCharacterLevel(characterUnlockData.character);
					skinData.m_gameCurrencyCost = skinUnlockData.GetUnlockISOPrice();
					skinData.m_characterType = selectedCharacter.m_characterType;
					skinData.m_flavorText = selectedCharacter.GetSkinFlavorText(i);
					skinData.m_skinIndex = i;
					skinData.m_unlockData = skinUnlockData.m_unlockData;
					if (skinData.m_unlockCharacterLevel == num)
					{
						num2++;
					}
				}
				float num10 = 0f;
				float num11 = 0f;
				bool flag2 = false;
				skinData.m_defaultPatternIndexForSkin = 0;
				skinData.m_defaultColorIndexForSkin = 0;
				skinData.m_possiblePatterns = new UIPatternData[characterSkin.m_patterns.Count];
				for (int j = 0; j < characterSkin.m_patterns.Count; j++)
				{
					GameBalanceVars.PatternUnlockData patternUnlockData = null;
					if (skinUnlockData != null)
					{
						if (j < skinUnlockData.patternUnlockData.Length)
						{
							patternUnlockData = skinUnlockData.patternUnlockData[j];
						}
					}
					CharacterPattern characterPattern = characterSkin.m_patterns[j];
					PlayerPatternData pattern = skin.GetPattern(j);
					int num12;
					if (!characterPattern.m_isHidden)
					{
						num12 = ((!GameBalanceVarsExtensions.MeetsVisibilityConditions(patternUnlockData)) ? 1 : 0);
					}
					else
					{
						num12 = 1;
					}
					int num13;
					if (num12 != 0)
					{
						num13 = (pattern.Unlocked ? 1 : 0);
					}
					else
					{
						num13 = 1;
					}
					bool flag3 = (byte)num13 != 0;
					if (!flag3)
					{
						num6++;
					}
					UIPatternData uIPatternData = default(UIPatternData);
					uIPatternData.m_buttonColor = characterPattern.m_UIDisplayColor;
					int isAvailable;
					if (pattern.Unlocked)
					{
						isAvailable = (skin.Unlocked ? 1 : 0);
					}
					else
					{
						isAvailable = 0;
					}
					uIPatternData.m_isAvailable = ((byte)isAvailable != 0);
					uIPatternData.m_isVisible = flag3;
					uIPatternData.m_textureIndex = j;
					UIPatternData uIPatternData2 = uIPatternData;
					uIPatternData2.m_possibleColors = new UIColorData[characterPattern.m_colors.Count];
					if (patternUnlockData != null)
					{
						uIPatternData2.m_unlockCharacterLevel = patternUnlockData.GetUnlockCharacterLevel(characterUnlockData.character);
						uIPatternData2.m_unlockData = patternUnlockData.m_unlockData;
						if (uIPatternData2.m_unlockCharacterLevel == num)
						{
							num3++;
						}
					}
					int patternsUnlocked = skinData.m_patternsUnlocked;
					int num14;
					if (pattern.Unlocked)
					{
						num14 = 1;
					}
					else
					{
						num14 = 0;
					}
					skinData.m_patternsUnlocked = patternsUnlocked + num14;
					for (int num15 = 0; num15 < characterPattern.m_colors.Count; num15++)
					{
						GameBalanceVars.ColorUnlockData colorUnlockData = null;
						if (patternUnlockData != null)
						{
							if (num15 < patternUnlockData.colorUnlockData.Length)
							{
								colorUnlockData = patternUnlockData.colorUnlockData[num15];
							}
						}
						CharacterColor characterColor = characterPattern.m_colors[num15];
						PlayerColorData color = pattern.GetColor(num15);
						int num16;
						if (!characterColor.m_isHidden)
						{
							num16 = ((!GameBalanceVarsExtensions.MeetsVisibilityConditions(colorUnlockData)) ? 1 : 0);
						}
						else
						{
							num16 = 1;
						}
						bool flag4 = (byte)num16 != 0;
						bool flag5 = gameplayOverrides.IsColorAllowed(selectedCharacter.m_characterType, i, j, num15);
						int num17;
						if (flag4)
						{
							if (!color.Unlocked)
							{
								num17 = 0;
								goto IL_047b;
							}
						}
						num17 = (flag5 ? 1 : 0);
						goto IL_047b;
						IL_047b:
						bool flag6 = (byte)num17 != 0;
						if (!flag6)
						{
							num7++;
						}
						else
						{
							flag2 = true;
						}
						uIPatternData2.m_possibleColors[num15] = new UIColorData
						{
							m_buttonColor = characterColor.m_UIDisplayColor,
							m_isAvailable = (color.Unlocked && num > characterColor.m_requiredLevelForEquip),
							m_isOwned = color.Unlocked,
							m_isSkinAvailable = skin.Unlocked,
							m_isVisible = flag6,
							m_styleLevelType = characterColor.m_styleLevel,
							m_rarity = characterColor.m_colorUnlockData.Rarity
						};
						if (colorUnlockData != null)
						{
							uIPatternData2.m_possibleColors[num15].m_name = selectedCharacter.GetPatternColorName(i, j, num15);
							uIPatternData2.m_possibleColors[num15].m_description = selectedCharacter.GetPatternColorDescription(i, j, num15);
							int sortOrder;
							if (characterColor.m_sortOrder == 0)
							{
								sortOrder = 9999;
							}
							else
							{
								sortOrder = characterColor.m_sortOrder;
							}
							uIPatternData2.m_possibleColors[num15].m_sortOrder = sortOrder;
							uIPatternData2.m_possibleColors[num15].m_unlockCharacterLevel = colorUnlockData.GetUnlockCharacterLevel(characterUnlockData.character);
							uIPatternData2.m_possibleColors[num15].m_isoCurrencyCost = colorUnlockData.GetUnlockISOPrice();
							uIPatternData2.m_possibleColors[num15].m_freelancerCurrencyCost = colorUnlockData.GetUnlockFreelancerCurrencyPrice();
							uIPatternData2.m_possibleColors[num15].m_realCurrencyCost = CommerceClient.Get().GetStylePrice(characterUnlockData.character, i, j, num15, HydrogenConfig.Get().Ticket.AccountCurrency);
							uIPatternData2.m_possibleColors[num15].m_skinIndex = i;
							uIPatternData2.m_possibleColors[num15].m_patternIndex = j;
							uIPatternData2.m_possibleColors[num15].m_colorIndex = num15;
							uIPatternData2.m_possibleColors[num15].m_flavorText = selectedCharacter.GetPatternColorFlavor(i, j, num15);
							uIPatternData2.m_possibleColors[num15].m_requiredLevelForEquip = characterColor.m_requiredLevelForEquip;
							uIPatternData2.m_possibleColors[num15].m_characterType = selectedCharacter.m_characterType;
							uIPatternData2.m_possibleColors[num15].m_unlockData = colorUnlockData.m_unlockData;
							uIPatternData2.m_possibleColors[num15].m_colorUnlockable = colorUnlockData;
							if (colorUnlockData.UnlockAutomaticallyWithParentSkin)
							{
								skinData.m_defaultPatternIndexForSkin = j;
								skinData.m_defaultColorIndexForSkin = num15;
							}
							if (uIPatternData2.m_possibleColors[num15].m_unlockCharacterLevel == num)
							{
								num4++;
							}
						}
						int colorsUnlocked = uIPatternData2.m_colorsUnlocked;
						int num18;
						if (color.Unlocked)
						{
							num18 = 1;
						}
						else
						{
							num18 = 0;
						}
						uIPatternData2.m_colorsUnlocked = colorsUnlocked + num18;
					}
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							goto end_IL_0785;
						}
						continue;
						end_IL_0785:
						break;
					}
					num10 += (float)uIPatternData2.m_colorsUnlocked;
					num11 += (float)(characterPattern.m_colors.Count - num7);
					skinData.m_possiblePatterns[j] = uIPatternData2;
				}
				while (true)
				{
					num11 += (float)(characterSkin.m_patterns.Count - num6);
					num10 += (float)skinData.m_patternsUnlocked;
					skinData.m_progressPct = ((!(num11 > 0f)) ? 0f : (num10 / num11));
					if (!flag2)
					{
						skinData.m_isVisible = false;
						flag = false;
					}
					while (i >= m_skinButtons.Count)
					{
						UISkinSelectButton uISkinSelectButton2 = Object.Instantiate(m_skinButtonPrefab);
						uISkinSelectButton2.transform.SetParent(m_skinGrid.transform);
						uISkinSelectButton2.transform.localScale = Vector3.one;
						uISkinSelectButton2.transform.localEulerAngles = Vector3.zero;
						uISkinSelectButton2.transform.localPosition = Vector3.zero;
						m_skinButtons.Add(uISkinSelectButton2);
						if (HitchDetector.Get() != null)
						{
							HitchDetector.Get().AddNewLayoutGroup(m_skinGrid);
						}
					}
					UISkinSelectButton uISkinSelectButton3 = m_skinButtons[i];
					uISkinSelectButton3.Setup(selectedCharacter, skinData, i, this);
					if (i == m_currentSelection.skinIndex)
					{
						uISkinSelectButton = uISkinSelectButton3;
					}
					else
					{
						uISkinSelectButton3.SetSelected(false);
					}
					StaggerComponent.SetStaggerComponent(uISkinSelectButton3.gameObject, flag);
					if (i == selectedCharacter.m_skins.Count - 1)
					{
						for (i++; i < m_skinButtons.Count; i++)
						{
							StaggerComponent.SetStaggerComponent(m_skinButtons[i].gameObject, false);
						}
					}
					i++;
					goto IL_09b1;
				}
				IL_09b1:;
			}
			while (true)
			{
				if ((bool)uISkinSelectButton)
				{
					while (true)
					{
						SelectSkin(uISkinSelectButton);
						return;
					}
				}
				return;
			}
		}
	}

	public void SkinClicked(UISkinSelectButton clickedSkin)
	{
		SelectSkin(clickedSkin);
		SetupSkinPurchaseButtons(clickedSkin.m_skinData);
		if (m_selectHandler != null)
		{
			while (true)
			{
				ISkinBrowserSelectHandler selectHandler;
				CharacterResourceLink currentCharacter;
				CharacterVisualInfo currentSelection;
				int isUnlocked;
				switch (6)
				{
				case 0:
					break;
				default:
					{
						selectHandler = m_selectHandler;
						currentCharacter = m_currentCharacter;
						currentSelection = m_currentSelection;
						if (!m_skinLocked)
						{
							if (!m_colorLocked)
							{
								isUnlocked = (UICharacterSelectScreenController.Get().IsCharacterSelectable(m_currentCharacter) ? 1 : 0);
								goto IL_007b;
							}
						}
						isUnlocked = 0;
						goto IL_007b;
					}
					IL_007b:
					selectHandler.OnSkinClick(this, currentCharacter, currentSelection, (byte)isUnlocked != 0);
					return;
				}
			}
		}
		Log.Warning("No handler selected for skin browser");
	}

	private void SetupSkinPurchaseButtons(UISkinData skinData)
	{
		UIManager.SetGameObjectActive(m_warningContainer, false);
		m_purchasePanel.SetDisabled(false);
		if (!skinData.m_isAvailable)
		{
			if (AppState_CharacterSelect.Get() != AppState.GetCurrent())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						m_purchaseType = PurchaseItemType.Skin;
						if (ClientGameManager.Get().AreUnlockConditionsMet(skinData.m_unlockData, true))
						{
							m_purchasePanel.Setup(skinData.m_gameCurrencyCost, 0, 0f);
							m_purchasePanel.m_unlockText.text = StringUtil.TR("UnlockSkin", "Global");
							UIManager.SetGameObjectActive(m_purchasePanel, true);
						}
						else
						{
							UIManager.SetGameObjectActive(m_purchasePanel, false);
						}
						return;
					}
				}
			}
		}
		UIManager.SetGameObjectActive(m_purchasePanel, false);
	}

	public void SkinClicked(int skinIndex)
	{
		foreach (UISkinSelectButton skinButton in m_skinButtons)
		{
			if (skinButton.m_skinIndex == skinIndex)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						SkinClicked(skinButton);
						return;
					}
				}
			}
		}
	}

	public void SelectSkin(UISkinSelectButton clickedSkin, bool selectColor = true)
	{
		int num = 0;
		using (List<UISkinSelectButton>.Enumerator enumerator = m_skinButtons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UISkinSelectButton current = enumerator.Current;
				if (current == clickedSkin)
				{
					current.SetSelected(true);
					m_skinLocked = !current.m_skinData.m_isAvailable;
					if (m_currentSelection.skinIndex != num)
					{
						m_currentSelection.skinIndex = num;
						m_currentSelection.patternIndex = current.m_skinData.m_defaultPatternIndexForSkin;
						m_currentSelection.colorIndex = current.m_skinData.m_defaultColorIndexForSkin;
					}
					if (m_currentSelection.patternIndex >= clickedSkin.m_skinData.m_possiblePatterns.Length)
					{
						m_currentSelection.patternIndex = clickedSkin.m_skinData.m_possiblePatterns.Length - 1;
					}
					List<UIPatternData> list = new List<UIPatternData>();
					for (int i = 0; i < clickedSkin.m_skinData.m_possiblePatterns.Length; i++)
					{
						UIPatternData item = clickedSkin.m_skinData.m_possiblePatterns[i];
						if (item.m_isVisible)
						{
							list.Add(item);
						}
					}
					LoadPatternInfo(list, num, true);
				}
				else
				{
					current.SetSelected(false);
				}
				num++;
			}
		}
		SetMainSelectedButton(clickedSkin);
		if (!(HitchDetector.Get() != null))
		{
			return;
		}
		while (true)
		{
			HitchDetector.Get().AddNewLayoutGroup(m_colorGrid);
			return;
		}
	}

	private void LoadPatternInfo(List<UIPatternData> textureInfo, int skinIndex, bool selectColor)
	{
		for (int i = 0; i < m_colorButtons.Count; i++)
		{
			UIManager.SetGameObjectActive(m_colorButtons[i], false);
			m_colorButtons[i].m_transformPriority = -1;
		}
		while (true)
		{
			if (m_currentCharacter == null)
			{
				return;
			}
			LobbyGameplayOverrides gameplayOverrides = GameManager.Get().GameplayOverrides;
			CharacterType characterType = m_currentCharacter.m_characterType;
			UIButtonColorSelect uIButtonColorSelect = null;
			UIButtonColorSelect uIButtonColorSelect2 = null;
			bool flag = false;
			int j = 0;
			for (int k = 0; k < textureInfo.Count; k++)
			{
				int num = 0;
				while (true)
				{
					IL_0276:
					int num2 = num;
					UIPatternData uIPatternData = textureInfo[k];
					if (num2 >= uIPatternData.m_possibleColors.Length)
					{
						break;
					}
					UIPatternData uIPatternData2 = textureInfo[k];
					UIColorData colorData = uIPatternData2.m_possibleColors[num];
					bool flag2 = gameplayOverrides.IsColorAllowed(characterType, skinIndex, k, num);
					colorData.m_isVisible &= flag2;
					while (j >= m_colorButtons.Count)
					{
						UIButtonColorSelect uIButtonColorSelect3 = Object.Instantiate(m_colorButtonPrefab);
						uIButtonColorSelect3.transform.SetParent(m_colorGrid.transform);
						uIButtonColorSelect3.transform.localScale = Vector3.one;
						uIButtonColorSelect3.transform.localEulerAngles = Vector3.zero;
						uIButtonColorSelect3.transform.localPosition = Vector3.zero;
						m_colorButtons.Add(uIButtonColorSelect3);
						if (HitchDetector.Get() != null)
						{
							HitchDetector.Get().AddNewLayoutGroup(m_colorGrid);
						}
					}
					while (true)
					{
						UIButtonColorSelect uIButtonColorSelect4 = m_colorButtons[j];
						StaggerComponent.SetStaggerComponent(uIButtonColorSelect4.gameObject, colorData.m_isVisible);
						if (!colorData.m_isVisible)
						{
							UIManager.SetGameObjectActive(uIButtonColorSelect4, false);
						}
						uIButtonColorSelect4.Setup(colorData, skinIndex, k, num, this);
						if (selectColor)
						{
							if (colorData.m_isVisible)
							{
								if (num == m_currentSelection.colorIndex)
								{
									if (k == m_currentSelection.patternIndex)
									{
										uIButtonColorSelect = uIButtonColorSelect4;
										uIButtonColorSelect2 = null;
										flag = true;
										goto IL_026a;
									}
								}
								if (!flag)
								{
									if (uIButtonColorSelect2 == null)
									{
										uIButtonColorSelect2 = uIButtonColorSelect4;
									}
								}
							}
						}
						else
						{
							uIButtonColorSelect4.SetSelected(false);
						}
						goto IL_026a;
						IL_026a:
						j++;
						num++;
						goto IL_0276;
					}
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						goto end_IL_0292;
					}
					continue;
					end_IL_0292:
					break;
				}
			}
			for (; j < m_colorButtons.Count; j++)
			{
				UIManager.SetGameObjectActive(m_colorButtons[j], false);
				StaggerComponent.SetStaggerComponent(m_colorButtons[j].gameObject, false);
			}
			while (true)
			{
				if (uIButtonColorSelect != null)
				{
					SelectColor(uIButtonColorSelect, skinIndex);
				}
				else if (uIButtonColorSelect2 != null)
				{
					m_currentSelection.colorIndex = uIButtonColorSelect2.m_tintIndex;
					m_currentSelection.patternIndex = uIButtonColorSelect2.m_patternIndex;
					SelectColor(uIButtonColorSelect2, skinIndex);
				}
				UIButtonColorSelect.SortTransform(m_colorButtons.ConvertAll<ITransformSortOrder>(UIButtonColorSelect.ButtonToTransformSortOrder));
				return;
			}
		}
	}

	public void ColorClicked(UIButtonColorSelect clickedBtn, int skinIndex)
	{
		if (skinIndex >= m_currentCharacter.m_skins.Count)
		{
			return;
		}
		SelectColor(clickedBtn, skinIndex);
		SetupColorPurchaseButtons(clickedBtn.m_colorData);
		if (m_selectHandler != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					GameType selectedQueueType = ClientGameManager.Get().GroupInfo.SelectedQueueType;
					int num;
					if (GameManager.Get().IsValidForHumanPreGameSelection(m_currentCharacter.m_characterType))
					{
						num = (GameManager.Get().IsCharacterAllowedForGameType(m_currentCharacter.m_characterType, selectedQueueType, null, null) ? 1 : 0);
					}
					else
					{
						num = 0;
					}
					bool flag = num != 0 || UICharacterSelectScreenController.Get().IsCharacterSelectable(m_currentCharacter);
					int num2;
					if (!m_skinLocked)
					{
						num2 = ((!m_colorLocked) ? 1 : 0);
					}
					else
					{
						num2 = 0;
					}
					bool flag2 = (byte)num2 != 0;
					ISkinBrowserSelectHandler selectHandler = m_selectHandler;
					CharacterResourceLink currentCharacter = m_currentCharacter;
					CharacterVisualInfo currentSelection = m_currentSelection;
					int isUnlocked;
					if (flag2)
					{
						isUnlocked = (flag ? 1 : 0);
					}
					else
					{
						isUnlocked = 0;
					}
					selectHandler.OnColorClick(this, currentCharacter, currentSelection, (byte)isUnlocked != 0);
					return;
				}
				}
			}
		}
		Log.Warning("No color select handler for skin browser");
	}

	private void SetupColorPurchaseButtons(UIColorData colorData)
	{
		bool flag2;
		UIPurchasePanel purchasePanel;
		int disabled;
		if (GameManager.Get().GameStatus != GameStatus.LoadoutSelecting)
		{
			if (!colorData.m_isOwned)
			{
				if (ClientGameManager.Get().AreUnlockConditionsMet(colorData.m_unlockData, true))
				{
					if (colorData.m_isoCurrencyCost <= 0 && colorData.m_freelancerCurrencyCost <= 0)
					{
						if (!(colorData.m_realCurrencyCost > 0f))
						{
							goto IL_01a1;
						}
					}
					m_purchaseType = PurchaseItemType.Tint;
					bool flag = colorData.m_requiredLevelForEquip > ClientGameManager.Get().GetPlayerCharacterLevel(colorData.m_characterType);
					flag2 = GameBalanceVarsExtensions.MeetsPurchaseabilityConditions(colorData.m_colorUnlockable);
					m_purchasePanel.Setup(colorData.m_isoCurrencyCost, colorData.m_freelancerCurrencyCost, colorData.m_realCurrencyCost);
					m_purchasePanel.m_unlockText.text = StringUtil.TR("UnlockStyle", "Global");
					UIManager.SetGameObjectActive(m_purchasePanel, true);
					purchasePanel = m_purchasePanel;
					if (!m_skinLocked)
					{
						if (!flag)
						{
							disabled = ((!flag2) ? 1 : 0);
							goto IL_012f;
						}
					}
					disabled = 1;
					goto IL_012f;
				}
			}
		}
		goto IL_01a1;
		IL_012f:
		purchasePanel.SetDisabled((byte)disabled != 0);
		string tooltipDescription = null;
		if (!flag2 && !colorData.m_colorUnlockable.PurchaseDescription.IsNullOrEmpty())
		{
			tooltipDescription = colorData.m_colorUnlockable.GetPurchaseDescription();
		}
		m_purchasePanel.SetupTooltip(tooltipDescription);
		m_warningText.text = StringUtil.TR("SkinRequiredForItem", "Global");
		UIManager.SetGameObjectActive(m_warningContainer, m_skinLocked);
		return;
		IL_01a1:
		UIManager.SetGameObjectActive(m_purchasePanel, false);
		UIManager.SetGameObjectActive(m_warningContainer, false);
	}

	public void SelectColor(UIButtonColorSelect clickedBtn, int skinIndex)
	{
		int num = 0;
		using (List<UIButtonColorSelect>.Enumerator enumerator = m_colorButtons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIButtonColorSelect current = enumerator.Current;
				if (current == clickedBtn)
				{
					if (!current.isSelected)
					{
						current.SetSelected(true);
						m_colorLocked = !current.m_colorData.m_isAvailable;
						m_currentSelection.colorIndex = clickedBtn.m_tintIndex;
						m_currentSelection.patternIndex = clickedBtn.m_patternIndex;
						if (m_selectHandler != null)
						{
							m_selectHandler.OnColorSelect(current.m_colorIcon.color);
						}
					}
				}
				else
				{
					current.SetSelected(false);
				}
				num++;
			}
		}
		SetMainSelectedButton(clickedBtn);
		if (m_selectHandler == null || !(clickedBtn != null) || m_currentSelection.skinIndex != skinIndex)
		{
			return;
		}
		while (true)
		{
			if (m_currentSelection.patternIndex != clickedBtn.m_patternIndex)
			{
				return;
			}
			while (true)
			{
				if (m_currentSelection.colorIndex != clickedBtn.m_tintIndex)
				{
					return;
				}
				while (true)
				{
					ISkinBrowserSelectHandler selectHandler = m_selectHandler;
					CharacterResourceLink currentCharacter = m_currentCharacter;
					CharacterVisualInfo currentSelection = m_currentSelection;
					int isUnlocked;
					if (!m_skinLocked)
					{
						isUnlocked = ((!m_colorLocked) ? 1 : 0);
					}
					else
					{
						isUnlocked = 0;
					}
					selectHandler.OnSelect(this, currentCharacter, currentSelection, (byte)isUnlocked != 0);
					return;
				}
			}
		}
	}

	public void SetMainSelectedButton(UICharacterVisualsSelectButton clickedButton)
	{
		for (int i = 0; i < m_skinButtons.Count; i++)
		{
			m_skinButtons[i].SetMainSelected(m_skinButtons[i] == clickedButton);
		}
		while (true)
		{
			for (int j = 0; j < m_colorButtons.Count; j++)
			{
				m_colorButtons[j].SetMainSelected(m_colorButtons[j] == clickedButton);
			}
			while (true)
			{
				switch (1)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void Select(CharacterVisualInfo newSelection)
	{
		m_currentSelection = newSelection;
		int num = 0;
		while (true)
		{
			if (num < m_skinButtons.Count)
			{
				if (m_skinButtons[num].m_skinIndex == m_currentSelection.skinIndex)
				{
					SelectSkin(m_skinButtons[num], false);
					SetupSkinPurchaseButtons(m_skinButtons[num].m_skinData);
					break;
				}
				num++;
				continue;
			}
			break;
		}
		int num2 = 0;
		while (true)
		{
			if (num2 >= m_colorButtons.Count)
			{
				return;
			}
			if (m_colorButtons[num2].m_patternIndex == m_currentSelection.patternIndex)
			{
				if (m_colorButtons[num2].m_tintIndex == m_currentSelection.colorIndex)
				{
					break;
				}
			}
			num2++;
		}
		while (true)
		{
			SelectColor(m_colorButtons[num2], newSelection.skinIndex);
			SetupColorPurchaseButtons(m_colorButtons[num2].m_colorData);
			return;
		}
	}

	private void PurchaseWithIso(BaseEventData data)
	{
		Purchase(CurrencyType.ISO);
	}

	private void PurchaseWithFreelancerCurrency(BaseEventData data)
	{
		Purchase(CurrencyType.FreelancerCurrency);
	}

	private void Purchase(CurrencyType currencyType)
	{
		UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
		uIPurchaseableItem.m_itemType = m_purchaseType;
		uIPurchaseableItem.m_charLink = m_currentCharacter;
		uIPurchaseableItem.m_skinIndex = m_currentSelection.skinIndex;
		uIPurchaseableItem.m_textureIndex = m_currentSelection.patternIndex;
		uIPurchaseableItem.m_tintIndex = m_currentSelection.colorIndex;
		uIPurchaseableItem.m_currencyType = currencyType;
		UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem);
	}

	private void PurchaseWithRealCurrency(BaseEventData data)
	{
		UIPurchaseableItem uIPurchaseableItem = new UIPurchaseableItem();
		uIPurchaseableItem.m_purchaseForCash = true;
		uIPurchaseableItem.m_itemType = m_purchaseType;
		uIPurchaseableItem.m_charLink = m_currentCharacter;
		uIPurchaseableItem.m_skinIndex = m_currentSelection.skinIndex;
		uIPurchaseableItem.m_textureIndex = m_currentSelection.patternIndex;
		uIPurchaseableItem.m_tintIndex = m_currentSelection.colorIndex;
		UIStorePanel.Get().OpenPurchaseDialog(uIPurchaseableItem);
	}
}
