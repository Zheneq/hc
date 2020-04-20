using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UISkinBrowserPanel : MonoBehaviour
{
	public UISkinSelectButton m_skinButtonPrefab;

	public GridLayoutGroup m_skinGrid;

	public UIButtonColorSelect m_colorButtonPrefab;

	public GridLayoutGroup m_colorGrid;

	public UISkinBrowserPanel.HandlerType m_handlerType;

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
		if (this.m_handlerType == UISkinBrowserPanel.HandlerType.CharacterSelect)
		{
			this.m_selectHandler = new UICharacterSelectSkinPanel();
		}
		else if (this.m_handlerType == UISkinBrowserPanel.HandlerType.CashShop)
		{
			this.m_selectHandler = new UISkinBrowserProgressHandler();
		}
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().AddNewLayoutGroup(this.m_skinGrid);
			HitchDetector.Get().AddNewLayoutGroup(this.m_colorGrid);
		}
	}

	private void Start()
	{
		this.m_purchasePanel.m_isoButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PurchaseWithIso);
		this.m_purchasePanel.m_freelancerCurrencyButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PurchaseWithFreelancerCurrency);
		this.m_purchasePanel.m_realCurrencyButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PurchaseWithRealCurrency);
		if (!this.m_characterDataListenerInitialized)
		{
			if (ClientGameManager.Get() != null)
			{
				ClientGameManager.Get().OnCharacterDataUpdated += this.OnCharacterDataUpdated;
				this.m_characterDataListenerInitialized = true;
			}
		}
		if (this.m_selectHandler != null)
		{
			this.m_selectHandler.OnStart(this);
		}
	}

	private void OnDestroy()
	{
		if (this.m_characterDataListenerInitialized)
		{
			if (ClientGameManager.Get() != null)
			{
				ClientGameManager.Get().OnCharacterDataUpdated -= this.OnCharacterDataUpdated;
				ClientGameManager.Get().OnLobbyGameplayOverridesChange -= this.OnLobbyGameplayOverridesUpdated;
			}
		}
		if (this.m_selectHandler != null)
		{
			this.m_selectHandler.OnDestroy(this);
		}
	}

	private void OnEnable()
	{
		if (!this.m_characterDataListenerInitialized)
		{
			if (ClientGameManager.Get() != null)
			{
				ClientGameManager.Get().OnCharacterDataUpdated += this.OnCharacterDataUpdated;
				ClientGameManager.Get().OnLobbyGameplayOverridesChange += this.OnLobbyGameplayOverridesUpdated;
				this.m_characterDataListenerInitialized = true;
			}
		}
		if (UICharacterSelectWorldObjects.Get())
		{
			UICharacterSelectWorldObjects.Get().PlayCameraAnimation("CamCloseupOUT");
		}
	}

	private void OnDisable()
	{
		if (this.m_selectHandler != null)
		{
			this.m_selectHandler.OnDisabled(this);
		}
		UIManager.SetGameObjectActive(this.m_purchasePanel, false, null);
	}

	public void OnCharacterDataUpdated(PersistedCharacterData newData)
	{
		if (this.m_currentCharacter == null)
		{
			return;
		}
		if (newData.CharacterType == this.m_currentCharacter.m_characterType)
		{
			this.PopulateCharacterData(this.m_currentCharacter, newData);
			PlayerSkinData skin = newData.CharacterComponent.GetSkin(this.m_currentSelection.skinIndex);
			if (!skin.Unlocked)
			{
				if (!skin.GetPattern(this.m_currentSelection.patternIndex).GetColor(this.m_currentSelection.skinIndex).Unlocked)
				{
					return;
				}
			}
			UIManager.SetGameObjectActive(this.m_warningContainer, false, null);
			UIManager.SetGameObjectActive(this.m_purchasePanel, false, null);
			this.m_purchasePanel.SetDisabled(false);
		}
	}

	public void RefreshUI()
	{
		this.RePopulateCharacterData();
		AppState_CharacterSelect.Get().UpdateSelectedSkin(this.m_currentSelection);
	}

	public void OnLobbyGameplayOverridesUpdated(LobbyGameplayOverrides gameplayOverrides)
	{
		this.RefreshUI();
	}

	public CharacterVisualInfo GetCurrentSelection()
	{
		return this.m_currentSelection;
	}

	public void Setup(CharacterType characterType, CharacterVisualInfo visualInfo)
	{
		this.Setup(GameWideData.Get().GetCharacterResourceLink(characterType), visualInfo, false);
	}

	public CharacterType GetDisplayedCharacterType()
	{
		if (this.m_currentCharacter != null)
		{
			return this.m_currentCharacter.m_characterType;
		}
		return CharacterType.None;
	}

	public CharacterVisualInfo GetDisplayedVisualInfo()
	{
		return this.m_displayedVisualInfo;
	}

	public void Setup(CharacterResourceLink characterLink, CharacterVisualInfo visualInfo, bool sameCharacter = false)
	{
		if (this.m_currentCharacter != null)
		{
			if (this.m_currentCharacter.m_characterType == characterLink.m_characterType)
			{
				if (this.m_currentSelection.Equals(visualInfo))
				{
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(this.m_warningContainer, false, null);
		UIManager.SetGameObjectActive(this.m_purchasePanel, false, null);
		this.m_purchasePanel.SetDisabled(false);
		this.m_displayedVisualInfo = visualInfo;
		UIManager.SetGameObjectActive(this.m_warningContainer, false, null);
		UIManager.SetGameObjectActive(this.m_purchasePanel, false, null);
		this.m_purchasePanel.SetDisabled(false);
		if (this.m_skinButtons.Count == 0)
		{
			this.m_skinButtons.AddRange(this.m_skinGrid.GetComponentsInChildren<UISkinSelectButton>(true));
		}
		if (this.m_colorButtons.Count == 0)
		{
			this.m_colorButtons.AddRange(this.m_colorGrid.GetComponentsInChildren<UIButtonColorSelect>(true));
		}
		this.m_currentCharacter = characterLink;
		PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(characterLink.m_characterType);
		this.m_currentSelection = visualInfo;
		this.PopulateCharacterData(this.m_currentCharacter, playerCharacterData);
	}

	public void RePopulateCharacterData()
	{
		if (this.m_currentCharacter != null)
		{
			this.PopulateCharacterData(this.m_currentCharacter, ClientGameManager.Get().GetPlayerCharacterData(this.m_currentCharacter.m_characterType));
			List<UIPatternData> list = new List<UIPatternData>();
			int num = 0;
			using (List<UISkinSelectButton>.Enumerator enumerator = this.m_skinButtons.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					UISkinSelectButton uiskinSelectButton = enumerator.Current;
					if (uiskinSelectButton.isSelected)
					{
						for (int i = 0; i < uiskinSelectButton.m_skinData.m_possiblePatterns.Length; i++)
						{
							UIPatternData item = uiskinSelectButton.m_skinData.m_possiblePatterns[i];
							if (item.m_isVisible)
							{
								list.Add(item);
							}
						}
						this.LoadPatternInfo(list, num, true);
						return;
					}
					num++;
				}
			}
		}
	}

	public void PopulateCharacterData(CharacterResourceLink selectedCharacter, PersistedCharacterData characterData)
	{
		if (!(selectedCharacter == null))
		{
			if (characterData != null)
			{
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
				UISkinSelectButton uiskinSelectButton = null;
				for (int i = 0; i < selectedCharacter.m_skins.Count; i++)
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
					bool flag;
					if (!characterSkin.m_isHidden)
					{
						flag = !GameBalanceVarsExtensions.MeetsVisibilityConditions(skinUnlockData);
					}
					else
					{
						flag = true;
					}
					bool flag2 = flag;
					bool flag3;
					if (flag2)
					{
						flag3 = skin.Unlocked;
					}
					else
					{
						flag3 = true;
					}
					bool flag4 = flag3;
					if (!flag4)
					{
						num5++;
					}
					UISkinData skinData = new UISkinData
					{
						m_skinImage = (Sprite)Resources.Load(characterSkin.m_skinSelectionIconPath, typeof(Sprite))
					};
					skinData.m_gameCurrencyCost = 0;
					skinData.m_isAvailable = skin.Unlocked;
					skinData.m_isVisible = flag4;
					if (skinUnlockData != null)
					{
						skinData.m_unlockCharacterLevel = skinUnlockData.GetUnlockCharacterLevel(characterUnlockData.character, false);
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
					float num8 = 0f;
					float num9 = 0f;
					bool flag5 = false;
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
						bool flag6;
						if (!characterPattern.m_isHidden)
						{
							flag6 = !GameBalanceVarsExtensions.MeetsVisibilityConditions(patternUnlockData);
						}
						else
						{
							flag6 = true;
						}
						bool flag7 = flag6;
						bool flag8;
						if (flag7)
						{
							flag8 = pattern.Unlocked;
						}
						else
						{
							flag8 = true;
						}
						bool flag9 = flag8;
						if (!flag9)
						{
							num6++;
						}
						UIPatternData uipatternData = default(UIPatternData);
						uipatternData.m_buttonColor = characterPattern.m_UIDisplayColor;
						bool isAvailable;
						if (pattern.Unlocked)
						{
							isAvailable = skin.Unlocked;
						}
						else
						{
							isAvailable = false;
						}
						uipatternData.m_isAvailable = isAvailable;
						uipatternData.m_isVisible = flag9;
						uipatternData.m_textureIndex = j;
						UIPatternData uipatternData2 = uipatternData;
						uipatternData2.m_possibleColors = new UIColorData[characterPattern.m_colors.Count];
						if (patternUnlockData != null)
						{
							uipatternData2.m_unlockCharacterLevel = patternUnlockData.GetUnlockCharacterLevel(characterUnlockData.character, false);
							uipatternData2.m_unlockData = patternUnlockData.m_unlockData;
							if (uipatternData2.m_unlockCharacterLevel == num)
							{
								num3++;
							}
						}
						int patternsUnlocked = skinData.m_patternsUnlocked;
						int num10;
						if (pattern.Unlocked)
						{
							num10 = 1;
						}
						else
						{
							num10 = 0;
						}
						skinData.m_patternsUnlocked = patternsUnlocked + num10;
						int k = 0;
						while (k < characterPattern.m_colors.Count)
						{
							GameBalanceVars.ColorUnlockData colorUnlockData = null;
							if (patternUnlockData != null)
							{
								if (k < patternUnlockData.colorUnlockData.Length)
								{
									colorUnlockData = patternUnlockData.colorUnlockData[k];
								}
							}
							CharacterColor characterColor = characterPattern.m_colors[k];
							PlayerColorData color = pattern.GetColor(k);
							bool flag10;
							if (!characterColor.m_isHidden)
							{
								flag10 = !GameBalanceVarsExtensions.MeetsVisibilityConditions(colorUnlockData);
							}
							else
							{
								flag10 = true;
							}
							bool flag11 = flag10;
							bool flag12 = gameplayOverrides.IsColorAllowed(selectedCharacter.m_characterType, i, j, k);
							if (!flag11)
							{
								goto IL_476;
							}
							bool flag13;
							if (color.Unlocked)
							{
								for (;;)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									goto IL_476;
								}
							}
							else
							{
								flag13 = false;
							}
							IL_47B:
							bool flag14 = flag13;
							if (!flag14)
							{
								num7++;
							}
							else
							{
								flag5 = true;
							}
							uipatternData2.m_possibleColors[k] = new UIColorData
							{
								m_buttonColor = characterColor.m_UIDisplayColor,
								m_isAvailable = (color.Unlocked && num > characterColor.m_requiredLevelForEquip),
								m_isOwned = color.Unlocked,
								m_isSkinAvailable = skin.Unlocked,
								m_isVisible = flag14,
								m_styleLevelType = characterColor.m_styleLevel,
								m_rarity = characterColor.m_colorUnlockData.Rarity
							};
							if (colorUnlockData != null)
							{
								uipatternData2.m_possibleColors[k].m_name = selectedCharacter.GetPatternColorName(i, j, k);
								uipatternData2.m_possibleColors[k].m_description = selectedCharacter.GetPatternColorDescription(i, j, k);
								UIColorData[] possibleColors = uipatternData2.m_possibleColors;
								int num11 = k;
								int sortOrder;
								if (characterColor.m_sortOrder == 0)
								{
									sortOrder = 0x270F;
								}
								else
								{
									sortOrder = characterColor.m_sortOrder;
								}
								possibleColors[num11].m_sortOrder = sortOrder;
								uipatternData2.m_possibleColors[k].m_unlockCharacterLevel = colorUnlockData.GetUnlockCharacterLevel(characterUnlockData.character, false);
								uipatternData2.m_possibleColors[k].m_isoCurrencyCost = colorUnlockData.GetUnlockISOPrice();
								uipatternData2.m_possibleColors[k].m_freelancerCurrencyCost = colorUnlockData.GetUnlockFreelancerCurrencyPrice();
								uipatternData2.m_possibleColors[k].m_realCurrencyCost = CommerceClient.Get().GetStylePrice(characterUnlockData.character, i, j, k, HydrogenConfig.Get().Ticket.AccountCurrency);
								uipatternData2.m_possibleColors[k].m_skinIndex = i;
								uipatternData2.m_possibleColors[k].m_patternIndex = j;
								uipatternData2.m_possibleColors[k].m_colorIndex = k;
								uipatternData2.m_possibleColors[k].m_flavorText = selectedCharacter.GetPatternColorFlavor(i, j, k);
								uipatternData2.m_possibleColors[k].m_requiredLevelForEquip = characterColor.m_requiredLevelForEquip;
								uipatternData2.m_possibleColors[k].m_characterType = selectedCharacter.m_characterType;
								uipatternData2.m_possibleColors[k].m_unlockData = colorUnlockData.m_unlockData;
								uipatternData2.m_possibleColors[k].m_colorUnlockable = colorUnlockData;
								if (colorUnlockData.UnlockAutomaticallyWithParentSkin)
								{
									skinData.m_defaultPatternIndexForSkin = j;
									skinData.m_defaultColorIndexForSkin = k;
								}
								if (uipatternData2.m_possibleColors[k].m_unlockCharacterLevel == num)
								{
									num4++;
								}
							}
							int colorsUnlocked = uipatternData2.m_colorsUnlocked;
							int num12;
							if (color.Unlocked)
							{
								num12 = 1;
							}
							else
							{
								num12 = 0;
							}
							uipatternData2.m_colorsUnlocked = colorsUnlocked + num12;
							k++;
							continue;
							IL_476:
							flag13 = flag12;
							goto IL_47B;
						}
						num8 += (float)uipatternData2.m_colorsUnlocked;
						num9 += (float)(characterPattern.m_colors.Count - num7);
						skinData.m_possiblePatterns[j] = uipatternData2;
					}
					num9 += (float)(characterSkin.m_patterns.Count - num6);
					num8 += (float)skinData.m_patternsUnlocked;
					skinData.m_progressPct = ((num9 <= 0f) ? 0f : (num8 / num9));
					if (!flag5)
					{
						skinData.m_isVisible = false;
						flag4 = false;
					}
					while (i >= this.m_skinButtons.Count)
					{
						UISkinSelectButton uiskinSelectButton2 = UnityEngine.Object.Instantiate<UISkinSelectButton>(this.m_skinButtonPrefab);
						uiskinSelectButton2.transform.SetParent(this.m_skinGrid.transform);
						uiskinSelectButton2.transform.localScale = Vector3.one;
						uiskinSelectButton2.transform.localEulerAngles = Vector3.zero;
						uiskinSelectButton2.transform.localPosition = Vector3.zero;
						this.m_skinButtons.Add(uiskinSelectButton2);
						if (HitchDetector.Get() != null)
						{
							HitchDetector.Get().AddNewLayoutGroup(this.m_skinGrid);
						}
					}
					UISkinSelectButton uiskinSelectButton3 = this.m_skinButtons[i];
					uiskinSelectButton3.Setup(selectedCharacter, skinData, i, this);
					if (i == this.m_currentSelection.skinIndex)
					{
						uiskinSelectButton = uiskinSelectButton3;
					}
					else
					{
						uiskinSelectButton3.SetSelected(false);
					}
					StaggerComponent.SetStaggerComponent(uiskinSelectButton3.gameObject, flag4, true);
					if (i == selectedCharacter.m_skins.Count - 1)
					{
						for (i++; i < this.m_skinButtons.Count; i++)
						{
							StaggerComponent.SetStaggerComponent(this.m_skinButtons[i].gameObject, false, true);
						}
					}
				}
				if (uiskinSelectButton)
				{
					this.SelectSkin(uiskinSelectButton, true);
				}
				return;
			}
		}
	}

	public void SkinClicked(UISkinSelectButton clickedSkin)
	{
		this.SelectSkin(clickedSkin, true);
		this.SetupSkinPurchaseButtons(clickedSkin.m_skinData);
		if (this.m_selectHandler != null)
		{
			ISkinBrowserSelectHandler selectHandler = this.m_selectHandler;
			CharacterResourceLink currentCharacter = this.m_currentCharacter;
			CharacterVisualInfo currentSelection = this.m_currentSelection;
			bool isUnlocked;
			if (!this.m_skinLocked)
			{
				if (!this.m_colorLocked)
				{
					isUnlocked = UICharacterSelectScreenController.Get().IsCharacterSelectable(this.m_currentCharacter);
					goto IL_7B;
				}
			}
			isUnlocked = false;
			IL_7B:
			selectHandler.OnSkinClick(this, currentCharacter, currentSelection, isUnlocked);
		}
		else
		{
			Log.Warning("No handler selected for skin browser", new object[0]);
		}
	}

	private void SetupSkinPurchaseButtons(UISkinData skinData)
	{
		UIManager.SetGameObjectActive(this.m_warningContainer, false, null);
		this.m_purchasePanel.SetDisabled(false);
		if (!skinData.m_isAvailable)
		{
			if (AppState_CharacterSelect.Get() != AppState.GetCurrent())
			{
				this.m_purchaseType = PurchaseItemType.Skin;
				if (ClientGameManager.Get().AreUnlockConditionsMet(skinData.m_unlockData, true))
				{
					this.m_purchasePanel.Setup(skinData.m_gameCurrencyCost, 0, 0f, false);
					this.m_purchasePanel.m_unlockText.text = StringUtil.TR("UnlockSkin", "Global");
					UIManager.SetGameObjectActive(this.m_purchasePanel, true, null);
				}
				else
				{
					UIManager.SetGameObjectActive(this.m_purchasePanel, false, null);
				}
				return;
			}
		}
		UIManager.SetGameObjectActive(this.m_purchasePanel, false, null);
	}

	public void SkinClicked(int skinIndex)
	{
		foreach (UISkinSelectButton uiskinSelectButton in this.m_skinButtons)
		{
			if (uiskinSelectButton.m_skinIndex == skinIndex)
			{
				this.SkinClicked(uiskinSelectButton);
				break;
			}
		}
	}

	public void SelectSkin(UISkinSelectButton clickedSkin, bool selectColor = true)
	{
		int num = 0;
		using (List<UISkinSelectButton>.Enumerator enumerator = this.m_skinButtons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UISkinSelectButton uiskinSelectButton = enumerator.Current;
				if (uiskinSelectButton == clickedSkin)
				{
					uiskinSelectButton.SetSelected(true);
					this.m_skinLocked = !uiskinSelectButton.m_skinData.m_isAvailable;
					if (this.m_currentSelection.skinIndex != num)
					{
						this.m_currentSelection.skinIndex = num;
						this.m_currentSelection.patternIndex = uiskinSelectButton.m_skinData.m_defaultPatternIndexForSkin;
						this.m_currentSelection.colorIndex = uiskinSelectButton.m_skinData.m_defaultColorIndexForSkin;
					}
					if (this.m_currentSelection.patternIndex >= clickedSkin.m_skinData.m_possiblePatterns.Length)
					{
						this.m_currentSelection.patternIndex = clickedSkin.m_skinData.m_possiblePatterns.Length - 1;
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
					this.LoadPatternInfo(list, num, true);
				}
				else
				{
					uiskinSelectButton.SetSelected(false);
				}
				num++;
			}
		}
		this.SetMainSelectedButton(clickedSkin);
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().AddNewLayoutGroup(this.m_colorGrid);
		}
	}

	private void LoadPatternInfo(List<UIPatternData> textureInfo, int skinIndex, bool selectColor)
	{
		for (int i = 0; i < this.m_colorButtons.Count; i++)
		{
			UIManager.SetGameObjectActive(this.m_colorButtons[i], false, null);
			this.m_colorButtons[i].m_transformPriority = -1;
		}
		if (this.m_currentCharacter == null)
		{
			return;
		}
		LobbyGameplayOverrides gameplayOverrides = GameManager.Get().GameplayOverrides;
		CharacterType characterType = this.m_currentCharacter.m_characterType;
		UIButtonColorSelect uibuttonColorSelect = null;
		UIButtonColorSelect uibuttonColorSelect2 = null;
		bool flag = false;
		int j = 0;
		for (int k = 0; k < textureInfo.Count; k++)
		{
			for (int l = 0; l < textureInfo[k].m_possibleColors.Length; l++)
			{
				UIColorData colorData = textureInfo[k].m_possibleColors[l];
				bool flag2 = gameplayOverrides.IsColorAllowed(characterType, skinIndex, k, l);
				colorData.m_isVisible = (colorData.m_isVisible && flag2);
				while (j >= this.m_colorButtons.Count)
				{
					UIButtonColorSelect uibuttonColorSelect3 = UnityEngine.Object.Instantiate<UIButtonColorSelect>(this.m_colorButtonPrefab);
					uibuttonColorSelect3.transform.SetParent(this.m_colorGrid.transform);
					uibuttonColorSelect3.transform.localScale = Vector3.one;
					uibuttonColorSelect3.transform.localEulerAngles = Vector3.zero;
					uibuttonColorSelect3.transform.localPosition = Vector3.zero;
					this.m_colorButtons.Add(uibuttonColorSelect3);
					if (HitchDetector.Get() != null)
					{
						HitchDetector.Get().AddNewLayoutGroup(this.m_colorGrid);
					}
				}
				UIButtonColorSelect uibuttonColorSelect4 = this.m_colorButtons[j];
				StaggerComponent.SetStaggerComponent(uibuttonColorSelect4.gameObject, colorData.m_isVisible, true);
				if (!colorData.m_isVisible)
				{
					UIManager.SetGameObjectActive(uibuttonColorSelect4, false, null);
				}
				uibuttonColorSelect4.Setup(colorData, skinIndex, k, l, this);
				if (selectColor)
				{
					if (colorData.m_isVisible)
					{
						if (l == this.m_currentSelection.colorIndex)
						{
							if (k == this.m_currentSelection.patternIndex)
							{
								uibuttonColorSelect = uibuttonColorSelect4;
								uibuttonColorSelect2 = null;
								flag = true;
								goto IL_260;
							}
						}
						if (!flag)
						{
							if (uibuttonColorSelect2 == null)
							{
								uibuttonColorSelect2 = uibuttonColorSelect4;
							}
						}
					}
					IL_260:;
				}
				else
				{
					uibuttonColorSelect4.SetSelected(false);
				}
				j++;
			}
		}
		while (j < this.m_colorButtons.Count)
		{
			UIManager.SetGameObjectActive(this.m_colorButtons[j], false, null);
			StaggerComponent.SetStaggerComponent(this.m_colorButtons[j].gameObject, false, true);
			j++;
		}
		if (uibuttonColorSelect != null)
		{
			this.SelectColor(uibuttonColorSelect, skinIndex);
		}
		else if (uibuttonColorSelect2 != null)
		{
			this.m_currentSelection.colorIndex = uibuttonColorSelect2.m_tintIndex;
			this.m_currentSelection.patternIndex = uibuttonColorSelect2.m_patternIndex;
			this.SelectColor(uibuttonColorSelect2, skinIndex);
		}
		UIButtonColorSelect.SortTransform(this.m_colorButtons.ConvertAll<ITransformSortOrder>(new Converter<UIButtonColorSelect, ITransformSortOrder>(UIButtonColorSelect.ButtonToTransformSortOrder)));
	}

	public void ColorClicked(UIButtonColorSelect clickedBtn, int skinIndex)
	{
		if (skinIndex >= this.m_currentCharacter.m_skins.Count)
		{
			return;
		}
		this.SelectColor(clickedBtn, skinIndex);
		this.SetupColorPurchaseButtons(clickedBtn.m_colorData);
		if (this.m_selectHandler != null)
		{
			GameType selectedQueueType = ClientGameManager.Get().GroupInfo.SelectedQueueType;
			bool flag = GameManager.Get().IsValidForHumanPreGameSelection(this.m_currentCharacter.m_characterType);
			bool flag2;
			if (flag)
			{
				flag2 = GameManager.Get().IsCharacterAllowedForGameType(this.m_currentCharacter.m_characterType, selectedQueueType, null, null);
			}
			else
			{
				flag2 = false;
			}
			flag = (flag2 || UICharacterSelectScreenController.Get().IsCharacterSelectable(this.m_currentCharacter));
			bool flag3;
			if (!this.m_skinLocked)
			{
				flag3 = !this.m_colorLocked;
			}
			else
			{
				flag3 = false;
			}
			bool flag4 = flag3;
			ISkinBrowserSelectHandler selectHandler = this.m_selectHandler;
			CharacterResourceLink currentCharacter = this.m_currentCharacter;
			CharacterVisualInfo currentSelection = this.m_currentSelection;
			bool isUnlocked;
			if (flag4)
			{
				isUnlocked = flag;
			}
			else
			{
				isUnlocked = false;
			}
			selectHandler.OnColorClick(this, currentCharacter, currentSelection, isUnlocked);
		}
		else
		{
			Log.Warning("No color select handler for skin browser", new object[0]);
		}
	}

	private void SetupColorPurchaseButtons(UIColorData colorData)
	{
		if (GameManager.Get().GameStatus != GameStatus.LoadoutSelecting)
		{
			if (!colorData.m_isOwned)
			{
				if (ClientGameManager.Get().AreUnlockConditionsMet(colorData.m_unlockData, true))
				{
					if (colorData.m_isoCurrencyCost <= 0 && colorData.m_freelancerCurrencyCost <= 0)
					{
						if (colorData.m_realCurrencyCost <= 0f)
						{
							goto IL_1A1;
						}
					}
					this.m_purchaseType = PurchaseItemType.Tint;
					bool flag = colorData.m_requiredLevelForEquip > ClientGameManager.Get().GetPlayerCharacterLevel(colorData.m_characterType);
					bool flag2 = GameBalanceVarsExtensions.MeetsPurchaseabilityConditions(colorData.m_colorUnlockable);
					this.m_purchasePanel.Setup(colorData.m_isoCurrencyCost, colorData.m_freelancerCurrencyCost, colorData.m_realCurrencyCost, false);
					this.m_purchasePanel.m_unlockText.text = StringUtil.TR("UnlockStyle", "Global");
					UIManager.SetGameObjectActive(this.m_purchasePanel, true, null);
					UIPurchasePanel purchasePanel = this.m_purchasePanel;
					bool disabled;
					if (!this.m_skinLocked)
					{
						if (!flag)
						{
							disabled = !flag2;
							goto IL_12F;
						}
					}
					disabled = true;
					IL_12F:
					purchasePanel.SetDisabled(disabled);
					string tooltipDescription = null;
					if (!flag2 && !colorData.m_colorUnlockable.PurchaseDescription.IsNullOrEmpty())
					{
						tooltipDescription = colorData.m_colorUnlockable.GetPurchaseDescription();
					}
					this.m_purchasePanel.SetupTooltip(tooltipDescription);
					this.m_warningText.text = StringUtil.TR("SkinRequiredForItem", "Global");
					UIManager.SetGameObjectActive(this.m_warningContainer, this.m_skinLocked, null);
					return;
				}
			}
		}
		IL_1A1:
		UIManager.SetGameObjectActive(this.m_purchasePanel, false, null);
		UIManager.SetGameObjectActive(this.m_warningContainer, false, null);
	}

	public void SelectColor(UIButtonColorSelect clickedBtn, int skinIndex)
	{
		int num = 0;
		using (List<UIButtonColorSelect>.Enumerator enumerator = this.m_colorButtons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UIButtonColorSelect uibuttonColorSelect = enumerator.Current;
				if (uibuttonColorSelect == clickedBtn)
				{
					if (!uibuttonColorSelect.isSelected)
					{
						uibuttonColorSelect.SetSelected(true);
						this.m_colorLocked = !uibuttonColorSelect.m_colorData.m_isAvailable;
						this.m_currentSelection.colorIndex = clickedBtn.m_tintIndex;
						this.m_currentSelection.patternIndex = clickedBtn.m_patternIndex;
						if (this.m_selectHandler != null)
						{
							this.m_selectHandler.OnColorSelect(uibuttonColorSelect.m_colorIcon.color);
						}
					}
				}
				else
				{
					uibuttonColorSelect.SetSelected(false);
				}
				num++;
			}
		}
		this.SetMainSelectedButton(clickedBtn);
		if (this.m_selectHandler != null && clickedBtn != null && this.m_currentSelection.skinIndex == skinIndex)
		{
			if (this.m_currentSelection.patternIndex == clickedBtn.m_patternIndex)
			{
				if (this.m_currentSelection.colorIndex == clickedBtn.m_tintIndex)
				{
					ISkinBrowserSelectHandler selectHandler = this.m_selectHandler;
					CharacterResourceLink currentCharacter = this.m_currentCharacter;
					CharacterVisualInfo currentSelection = this.m_currentSelection;
					bool isUnlocked;
					if (!this.m_skinLocked)
					{
						isUnlocked = !this.m_colorLocked;
					}
					else
					{
						isUnlocked = false;
					}
					selectHandler.OnSelect(this, currentCharacter, currentSelection, isUnlocked);
				}
			}
		}
	}

	public void SetMainSelectedButton(UICharacterVisualsSelectButton clickedButton)
	{
		for (int i = 0; i < this.m_skinButtons.Count; i++)
		{
			this.m_skinButtons[i].SetMainSelected(this.m_skinButtons[i] == clickedButton);
		}
		for (int j = 0; j < this.m_colorButtons.Count; j++)
		{
			this.m_colorButtons[j].SetMainSelected(this.m_colorButtons[j] == clickedButton);
		}
	}

	public void Select(CharacterVisualInfo newSelection)
	{
		this.m_currentSelection = newSelection;
		for (int i = 0; i < this.m_skinButtons.Count; i++)
		{
			if (this.m_skinButtons[i].m_skinIndex == this.m_currentSelection.skinIndex)
			{
				this.SelectSkin(this.m_skinButtons[i], false);
				this.SetupSkinPurchaseButtons(this.m_skinButtons[i].m_skinData);
				break;
			}
		}
		for (int j = 0; j < this.m_colorButtons.Count; j++)
		{
			if (this.m_colorButtons[j].m_patternIndex == this.m_currentSelection.patternIndex)
			{
				if (this.m_colorButtons[j].m_tintIndex == this.m_currentSelection.colorIndex)
				{
					this.SelectColor(this.m_colorButtons[j], newSelection.skinIndex);
					this.SetupColorPurchaseButtons(this.m_colorButtons[j].m_colorData);
					break;
				}
			}
		}
	}

	private void PurchaseWithIso(BaseEventData data)
	{
		this.Purchase(CurrencyType.ISO);
	}

	private void PurchaseWithFreelancerCurrency(BaseEventData data)
	{
		this.Purchase(CurrencyType.FreelancerCurrency);
	}

	private void Purchase(CurrencyType currencyType)
	{
		UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
		uipurchaseableItem.m_itemType = this.m_purchaseType;
		uipurchaseableItem.m_charLink = this.m_currentCharacter;
		uipurchaseableItem.m_skinIndex = this.m_currentSelection.skinIndex;
		uipurchaseableItem.m_textureIndex = this.m_currentSelection.patternIndex;
		uipurchaseableItem.m_tintIndex = this.m_currentSelection.colorIndex;
		uipurchaseableItem.m_currencyType = currencyType;
		UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem);
	}

	private void PurchaseWithRealCurrency(BaseEventData data)
	{
		UIPurchaseableItem uipurchaseableItem = new UIPurchaseableItem();
		uipurchaseableItem.m_purchaseForCash = true;
		uipurchaseableItem.m_itemType = this.m_purchaseType;
		uipurchaseableItem.m_charLink = this.m_currentCharacter;
		uipurchaseableItem.m_skinIndex = this.m_currentSelection.skinIndex;
		uipurchaseableItem.m_textureIndex = this.m_currentSelection.patternIndex;
		uipurchaseableItem.m_tintIndex = this.m_currentSelection.colorIndex;
		UIStorePanel.Get().OpenPurchaseDialog(uipurchaseableItem);
	}

	public enum HandlerType
	{
		None,
		CharacterSelect,
		CashShop
	}
}
