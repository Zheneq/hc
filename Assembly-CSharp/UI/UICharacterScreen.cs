using System;
using System.Collections.Generic;
using System.Linq;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = System.Random;

public class UICharacterScreen : UIScene
{
	[Header("Base Objects")]
	public CloseObjectInfo[] MouseClickCloseObjects;
	
	[Header("Character Select Buttons")]
	public UICharacterPanelSelectButton m_CharacterButtonSelectPrefab;
	public UICharacterPanelSelectButton[] m_SelectWillFillBtns;
	public LayoutGroup[] m_FirepowerRows;
	public LayoutGroup[] m_FrontlineRows;
	public LayoutGroup[] m_SupportRows;
	public Animator m_characterSelectAnimController;
	
	[Header("Character Select Filter Buttons")]
	public HorizontalLayoutGroup m_searchFiltersContainer;
	public TMP_InputField m_searchInputField;
	public UICharacterSelectFactionFilter m_notOnAFactionFilter;
	public UICharacterSelectFactionFilter m_factionFilterPrefab;
	
	[Header("Game Types")]
	public LayoutGroup m_GameTypeContainer;
	public _ToggleSwap m_GameTypePrefab;
	
	[Header("Bot Settings Panel")]
	public _SelectableBtn m_simpleCogBtn;
	public _SelectableBtn m_advancedCogBtn;
	public _SelectableBtn m_dropdownBtn;
	public RectTransform m_DifficultyListDropdown;
	public GridLayoutGroup m_difficultyListContainer;
	public _SelectableBtn m_easyBtn;
	public _SelectableBtn m_mediumBtn;
	public _SelectableBtn m_hardBtn;
	public GameObject m_botSkillPanel;
	public GameObject m_enemyBotSkillPanel;
	public GameObject m_teamBotSkillPanel;
	public UIStarsPanel m_enemyBotStars;
	public UIStarsPanel m_teamBotStars;
	public _ToggleSwap m_teamBotsToggle;

	[Header("Side Buttons")]
	public RectTransform m_sideBtnContainer;
	public _SelectableBtn m_bioBtn;
	public _SelectableBtn m_skinsBtn;
	public _SelectableBtn m_AbilitiesBtn;
	public _SelectableBtn m_CatalystBtn;
	public _SelectableBtn m_TauntsBtn;
	public UICharacterSelectPartyList m_partyListPanel;

	[Header("Ability Side Buttons Mouse Over")]
	public _SelectableBtn[] m_AbilityMouseOverBtns;
	public Image[] m_AbilityIcons;
	public Image[] m_AbilityModIcons;
	public Image[] m_AbilityPhaseColors;
	public Image[] m_AbilityPhaseColorsGradient;

	[Header("Catalyst Side Buttons Mouse Over")]
	public _SelectableBtn[] m_CatalsytBtns;
	public Image[] m_CatalystIcons;
	public Image[] m_CatalystHoverIcons;

	[Header("Skin Side Button Mouse Over")]
	public _SelectableBtn m_selectedSkinColorBtn;
	public Image m_selectedSkinColor;
	
	private static UICharacterScreen s_instance;
	
	private _ButtonSwapSprite[] SkinSubButtons;
	private _ButtonSwapSprite[] AbilitySubButtons;
	private _ButtonSwapSprite[] CatalystSubButtons;
	private UIAbilityButtonModPanel[] SelectedAbilityData;
	private Dictionary<AbilityRunPhase, Card> SelectedCatalysts;
	private List<UICharacterPanelSelectButton> CharacterSelectButtons = new List<UICharacterPanelSelectButton>();
	private List<UICharacterSelectFactionFilter> m_filterButtons;
	private UICharacterSelectFactionFilter m_lastFilterBtnClicked;
	private List<GameSubTypeState> m_gameSubTypeBtns = new List<GameSubTypeState>();
	private List<_ToggleSwap> m_gameTypeButtons = new List<_ToggleSwap>();
	private bool SentInitialSubTypes;
	private static CharacterSelectSceneStateParameters m_currentState;

	public static UICharacterScreen Get()
	{
		return s_instance;
	}

	public override CloseObjectInfo[] GetMouseClickObjects()
	{
		return MouseClickCloseObjects;
	}

	public override void Awake()
	{
		s_instance = this;
		RebuildCalls[(int)RefreshFunctionType.RefreshSideButtonVisibility] = RefreshSideButtonsVisibility;
		RebuildCalls[(int)RefreshFunctionType.RefreshSideButtonClickability] = RefreshSideButtonsClickability;
		RebuildCalls[(int)RefreshFunctionType.RefreshSelectedCharacterButton] = RefreshSelectedCharacterButton;
		RebuildCalls[(int)RefreshFunctionType.SendRequestToServerForCharacterSelect] = SendRequestToServerForCharacterSelect;
		RebuildCalls[(int)RefreshFunctionType.RefreshCharacterButtonVisibility] = RefreshCharacterButtonsVisibility;
		RebuildCalls[(int)RefreshFunctionType.RefreshSelectedGameType] = RefreshSelectedGameType;
		RebuildCalls[(int)RefreshFunctionType.RefreshCharacterButtons] = RefreshCharacterButtons;
		RebuildCalls[(int)RefreshFunctionType.RefreshBotSkillPanel] = RefreshBotSkillPanel;
		RebuildCalls[(int)RefreshFunctionType.RefreshGameSubTypes] = RefreshGameSubTypes;
		RebuildCalls[(int)RefreshFunctionType.RefreshPartyList] = RefreshPartyList;
		SetupButtons();
		SetupCharacterButtons();
		SetupCharacterFilterButtons();
		_ToggleSwap[] componentsInChildren = m_GameTypeContainer.GetComponentsInChildren<_ToggleSwap>(true);
		m_gameTypeButtons.AddRange(componentsInChildren);
		ClientGameManager.Get().OnLobbyGameplayOverridesChange += OnLobbyGameplayOverridesUpdated;
		ClientGameManager.Get().OnGroupUpdateNotification += RefreshGameSubTypes;
		base.Awake();
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= OnLobbyGameplayOverridesUpdated;
			ClientGameManager.Get().OnGroupUpdateNotification -= RefreshGameSubTypes;
		}
	}

	public void OnLobbyGameplayOverridesUpdated(LobbyGameplayOverrides gameplayOverrides)
	{
		SetupCharacterButtons();
	}

	private void SetupCharacterFilterButtons()
	{
		m_searchInputField.onValueChanged.AddListener(EditedSearchInput);
		m_filterButtons = new List<UICharacterSelectFactionFilter>();
		List<CharacterType> filteredCharacters = new List<CharacterType>();
		filteredCharacters.AddRange((CharacterType[])Enum.GetValues(typeof(CharacterType)));
		m_filterButtons.Add(m_notOnAFactionFilter);
		foreach (FactionGroup groupFilter in FactionWideData.Get().FactionGroupsToDisplayFilter())
		{
			UICharacterSelectFactionFilter filter = Instantiate(m_factionFilterPrefab);
			filter.transform.SetParent(m_searchFiltersContainer.transform);
			filter.transform.localPosition = Vector3.zero;
			filter.transform.localScale = Vector3.one;
			filter.Setup(groupFilter, ClickedOnFactionFilter);
			m_filterButtons.Add(filter);
			if (groupFilter.Characters != null)
			{
				filteredCharacters = filteredCharacters.Except(groupFilter.Characters).ToList();
			}

			filter.m_btn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
			{
				(tooltip as UISimpleTooltip).Setup(FactionGroup.GetDisplayName(groupFilter.FactionGroupID));
				return true;
			});
		}
		m_notOnAFactionFilter.Setup(filteredCharacters, ClickedOnFactionFilter);
		UITooltipObject component = m_notOnAFactionFilter.m_btn.spriteController.GetComponent<UITooltipHoverObject>();

		component.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
			{
				(tooltip as UISimpleTooltip).Setup(StringUtil.TR("Wildcard", "Global"));
				return true;
			});
	}

	private void SetupCharacterButtons()
	{
		foreach (UICharacterPanelSelectButton characterBtn in CharacterSelectButtons)
		{
			if (!m_SelectWillFillBtns.Contains(characterBtn))
			{
				Destroy(characterBtn.gameObject);
			}
		}
		CharacterSelectButtons.Clear();
		bool enableHiddenCharacters = GameManager.Get() != null && GameManager.Get().GameplayOverrides.EnableHiddenCharacters;
		List<CharacterType> assassins = new List<CharacterType>();
		List<CharacterType> tanks = new List<CharacterType>();
		List<CharacterType> supports = new List<CharacterType>();
		foreach (CharacterType characterType in (CharacterType[])Enum.GetValues(typeof(CharacterType)))
		{
			try
			{
				if (characterType == CharacterType.TestFreelancer1
				    || characterType == CharacterType.TestFreelancer2
				    || characterType == CharacterType.None)
				{
					continue;
				}
				
				CharacterResourceLink characterResourceLink =
					GameWideData.Get().GetCharacterResourceLink(characterType);
				if (enableHiddenCharacters || !characterResourceLink.m_isHidden)
				{
					switch (characterResourceLink.m_characterRole)
					{
						case CharacterRole.Assassin:
							assassins.Add(characterType);
							break;
						case CharacterRole.Support:
							supports.Add(characterType);
							break;
						case CharacterRole.Tank:
							tanks.Add(characterType);
							break;
					}
				}
			}
			catch
			{
			}
		}
		assassins.Sort(CompareCharacterTypeName);
		tanks.Sort(CompareCharacterTypeName);
		supports.Sort(CompareCharacterTypeName);
		int widthAssassins = Mathf.CeilToInt(assassins.Count / 2f);
		int widthTanks = Mathf.CeilToInt(tanks.Count / 2f);
		int widthSupports = Mathf.CeilToInt(supports.Count / 2f);
		int rowIndex = 0;
		for (int i = 0; i < assassins.Count; i++)
		{
			UICharacterPanelSelectButton characterBtn = Instantiate(m_CharacterButtonSelectPrefab);
			characterBtn.m_characterType = assassins[i];
			if (i - rowIndex * widthAssassins >= widthAssassins)
			{
				rowIndex++;
			}
			UIManager.ReparentTransform(characterBtn.gameObject.transform, m_FirepowerRows[rowIndex].gameObject.transform);
			CharacterSelectButtons.Add(characterBtn);
		}
		rowIndex = 0;
		for (int i = 0; i < tanks.Count; i++)
		{
			UICharacterPanelSelectButton characterBtn = Instantiate(m_CharacterButtonSelectPrefab);
			characterBtn.m_characterType = tanks[i];
			if (i - rowIndex * widthTanks >= widthTanks)
			{
				rowIndex++;
			}
			UIManager.ReparentTransform(characterBtn.gameObject.transform, m_FrontlineRows[rowIndex].gameObject.transform);
			CharacterSelectButtons.Add(characterBtn);
		}
		rowIndex = 0;
		for (int i = 0; i < supports.Count; i++)
		{
			UICharacterPanelSelectButton characterBtn = Instantiate(m_CharacterButtonSelectPrefab);
			characterBtn.m_characterType = supports[i];
			if (i - rowIndex * widthSupports >= widthSupports)
			{
				rowIndex++;
			}
			UIManager.ReparentTransform(characterBtn.gameObject.transform, m_SupportRows[rowIndex].gameObject.transform);
			CharacterSelectButtons.Add(characterBtn);
		}
		if (!m_SelectWillFillBtns.IsNullOrEmpty())
		{
			foreach (UICharacterPanelSelectButton characterBtn in m_SelectWillFillBtns)
			{
				CharacterSelectButtons.Add(characterBtn);
			}
		}
		foreach (UICharacterPanelSelectButton characterBtn in CharacterSelectButtons)
		{
			characterBtn.Setup(false);
		}
	}

	private int CompareCharacterTypeName(CharacterType CharA, CharacterType CharB)
	{
		return CharA.GetDisplayName().CompareTo(CharB.GetDisplayName());
	}

	public override SceneStateParameters GetCurrentState()
	{
		return m_currentState ?? (m_currentState = new CharacterSelectSceneStateParameters());
	}

	public static CharacterSelectSceneStateParameters GetCurrentSpecificState()
	{
		return m_currentState ?? (m_currentState = new CharacterSelectSceneStateParameters());
	}

	public override bool DoesHandleParameter(SceneStateParameters parameters)
	{
		return parameters is CharacterSelectSceneStateParameters;
	}

	public void DoRefreshFunctions(ushort RefreshBits)
	{
		foreach (KeyValuePair<int, RebuildDelegate> keyValuePair in RebuildCalls)
		{
			if ((keyValuePair.Key & RefreshBits) != 0)
			{
				keyValuePair.Value();
			}
		}
	}

	public override void HandleNewSceneStateParameter(SceneStateParameters parameters)
	{
		CharacterSelectSceneStateParameters stateParams = parameters as CharacterSelectSceneStateParameters;
		ushort flags = 0;
		if (stateParams != null)
		{
			if (stateParams.ClientSelectedCharacter != null)
			{
				m_currentState.ClientSelectedCharacter = stateParams.ClientSelectedCharacter;
				flags |= (ushort)RefreshFunctionType.RefreshSideButtonVisibility;
				flags |= (ushort)RefreshFunctionType.RefreshSelectedCharacterButton;
				flags |= (ushort)RefreshFunctionType.RefreshCharacterButtons;
			}
			if (stateParams.SideButtonsVisible != null)
			{
				m_currentState.SideButtonsVisible = stateParams.SideButtonsVisible;
				flags |= (ushort)RefreshFunctionType.RefreshSideButtonVisibility;
			}
			if (stateParams.SideButtonsClickable != null)
			{
				m_currentState.SideButtonsClickable = stateParams.SideButtonsClickable;
				flags |= (ushort)RefreshFunctionType.RefreshSideButtonClickability;
			}
			if (stateParams.ClientSelectedVisualInfo != null)
			{
				m_currentState.ClientSelectedVisualInfo = stateParams.ClientSelectedVisualInfo;
			}
			if (stateParams.ClientRequestToServerSelectCharacter != null)
			{
				m_currentState.ClientRequestToServerSelectCharacter = stateParams.ClientRequestToServerSelectCharacter;
				flags |= (ushort)RefreshFunctionType.RefreshSelectedCharacterButton;
				flags |= (ushort)RefreshFunctionType.SendRequestToServerForCharacterSelect;
			}
			if (stateParams.CharacterSelectButtonsVisible != null)
			{
				m_currentState.CharacterSelectButtonsVisible = stateParams.CharacterSelectButtonsVisible;
				flags |= (ushort)RefreshFunctionType.RefreshCharacterButtonVisibility;
			}
			if (stateParams.ClientRequestedGameType != null)
			{
				m_currentState.ClientRequestedGameType = stateParams.ClientRequestedGameType;
				flags |= (ushort)RefreshFunctionType.RefreshSelectedGameType;
				flags |= (ushort)RefreshFunctionType.RefreshCharacterButtons;
				flags |= (ushort)RefreshFunctionType.RefreshBotSkillPanel;
				flags |= (ushort)RefreshFunctionType.RefreshGameSubTypes;
			}
			if (stateParams.BotDifficultyView != null)
			{
				m_currentState.BotDifficultyView = stateParams.BotDifficultyView;
				flags |= (ushort)RefreshFunctionType.RefreshBotSkillPanel;
			}
			if (stateParams.AllyBotTeammatesSelected != null)
			{
				m_currentState.AllyBotTeammatesSelected = stateParams.AllyBotTeammatesSelected;
				flags |= (ushort)RefreshFunctionType.RefreshBotSkillPanel;
				flags |= (ushort)RefreshFunctionType.RefreshGameSubTypes;
			}
			if (stateParams.AllyBotTeammatesClickable != null)
			{
				m_currentState.AllyBotTeammatesClickable = stateParams.AllyBotTeammatesClickable;
				flags |= (ushort)RefreshFunctionType.RefreshBotSkillPanel;
			}
			if (stateParams.BotsCanTauntCheckboxEnabled != null)
			{
				m_currentState.BotsCanTauntCheckboxEnabled = stateParams.BotsCanTauntCheckboxEnabled;
				flags |= (ushort)RefreshFunctionType.RefreshBotSkillPanel;
			}
			if (stateParams.SimpleBotSetting != null)
			{
				m_currentState.SimpleBotSetting = stateParams.SimpleBotSetting;
				flags |= (ushort)RefreshFunctionType.RefreshBotSkillPanel;
			}
			if (stateParams.ClientRequestedSimpleBotSettingValue != null)
			{
				m_currentState.ClientRequestedSimpleBotSettingValue = stateParams.ClientRequestedSimpleBotSettingValue;
				flags |= (ushort)RefreshFunctionType.RefreshBotSkillPanel;
			}
			if (stateParams.ClientRequestAllyBotTeammates != null)
			{
				m_currentState.ClientRequestAllyBotTeammates = stateParams.ClientRequestAllyBotTeammates;
				flags |= (ushort)RefreshFunctionType.RefreshBotSkillPanel;
				flags |= (ushort)RefreshFunctionType.RefreshGameSubTypes;
			}
			if (stateParams.SelectedAllyBotDifficulty != null)
			{
				m_currentState.SelectedAllyBotDifficulty = stateParams.SelectedAllyBotDifficulty;
				flags |= (ushort)RefreshFunctionType.RefreshBotSkillPanel;
			}
			if (stateParams.SelectedEnemyBotDifficulty != null)
			{
				m_currentState.SelectedEnemyBotDifficulty = stateParams.SelectedEnemyBotDifficulty;
				flags |= (ushort)RefreshFunctionType.RefreshBotSkillPanel;
			}
			if (stateParams.ClientRequestedAllyBotDifficulty != null)
			{
				m_currentState.ClientRequestedAllyBotDifficulty = stateParams.ClientRequestedAllyBotDifficulty;
				flags |= (ushort)RefreshFunctionType.RefreshBotSkillPanel;
			}
			if (stateParams.ClientRequestedEnemyBotDifficulty != null)
			{
				m_currentState.ClientRequestedEnemyBotDifficulty = stateParams.ClientRequestedEnemyBotDifficulty;
				flags |= (ushort)RefreshFunctionType.RefreshBotSkillPanel;
			}
			if (stateParams.CustomGamePartyListVisible != null)
			{
				m_currentState.CustomGamePartyListVisible = stateParams.CustomGamePartyListVisible;
				flags |= (ushort)RefreshFunctionType.RefreshPartyList;
			}
			if (stateParams.CustomGamePartyListHidden != null)
			{
				m_currentState.CustomGamePartyListHidden = stateParams.CustomGamePartyListHidden;
				flags |= (ushort)RefreshFunctionType.RefreshPartyList;
			}
		}
		DoRefreshFunctions(flags);
		base.HandleNewSceneStateParameter(parameters);
	}

	public void UpdateSubTypeMaskChecks(ushort subTypeMask)
	{
		foreach (GameSubTypeState gameSubTypeBtn in m_gameSubTypeBtns)
		{
			gameSubTypeBtn.btn.SetOn((gameSubTypeBtn.SubTypeBit & subTypeMask) != 0);
		}
	}

	private void CheckSubTypeSelection(bool sendMaskUpdate = false, ushort oldMask = 0)
	{
		ushort newMask = 0;
		CharacterSelectSceneStateParameters Parameters = GetCurrentSpecificState();
		if (Parameters.GameTypeToDisplay == GameType.Coop)
		{
			if (Parameters.GameSubTypesVisible)
			{
				if (oldMask != 0)
				{
					newMask = ClientGameManager.Get().GenerateGameSubTypeMaskForToggledAntiSocial(Parameters.GameTypeToDisplay, oldMask);
				}
				else
				{
					foreach (GameSubTypeState subTypeBtn in m_gameSubTypeBtns)
					{
						if (subTypeBtn.btn.IsChecked())
						{
							newMask = (ushort)(subTypeBtn.SubTypeBit | newMask);
						}
					}
				}
			}
			else
			{
				Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(GameType.Coop);
				if (!gameTypeSubTypes.IsNullOrEmpty())
				{
					foreach (KeyValuePair<ushort, GameSubType> keyValuePair in gameTypeSubTypes)
					{
						if (Parameters.AllyBotTeammatesSelected == keyValuePair.Value.HasMod(GameSubType.SubTypeMods.AntiSocial))
						{
							newMask = keyValuePair.Key;
							break;
						}
					}
				}
			}
		}
		else if (!Parameters.GameSubTypesVisible)
		{
			Dictionary<ushort, GameSubType> gameTypeSubTypes2 = ClientGameManager.Get().GetGameTypeSubTypes(Parameters.GameTypeToDisplay);
			if (!gameTypeSubTypes2.IsNullOrEmpty())
			{
				newMask = gameTypeSubTypes2.Keys.First();
			}
		}
		ushort exclusiveModBitsOfGameTypeToDisplay = Parameters.ExclusiveModBitsOfGameTypeToDisplay;
		ushort checkedExclusiveBit = 0;
		if (exclusiveModBitsOfGameTypeToDisplay != 0)
		{
			foreach (GameSubTypeState subTypeBtn in m_gameSubTypeBtns)
			{
				if (subTypeBtn.btn.IsChecked()
				    && (exclusiveModBitsOfGameTypeToDisplay | subTypeBtn.SubTypeBit) != 0)
				{
					checkedExclusiveBit = subTypeBtn.SubTypeBit;
					break;
				}
			}

			if (checkedExclusiveBit != 0)
			{
				newMask = checkedExclusiveBit;
			}
		}
		if (checkedExclusiveBit == 0)
		{
			foreach (GameSubTypeState subTypeBtn in m_gameSubTypeBtns)
			{
				if (subTypeBtn.btn.IsChecked())
				{
					newMask = (ushort)(subTypeBtn.SubTypeBit | newMask);
				}
			}
		}
		ushort allSubTypeBits = 0;
		foreach (GameSubTypeState subTypeBtn in m_gameSubTypeBtns)
		{
			allSubTypeBits = (ushort)(subTypeBtn.SubTypeBit | allSubTypeBits);
		}
		if (allSubTypeBits != 0 && (newMask & allSubTypeBits) == 0)
		{
			m_gameSubTypeBtns[0].btn.SetOn(true);
			newMask = m_gameSubTypeBtns[0].SubTypeBit;
		}
		Parameters.SelectedSubTypeMask = newMask;
		UpdateSubTypeMaskChecks(newMask);
		if (sendMaskUpdate || !SentInitialSubTypes)
		{
			SentInitialSubTypes = true;
			if (ClientGameManager.Get().GroupInfo.InAGroup)
			{
				if (ClientGameManager.Get().GroupInfo.IsLeader)
				{
					ClientGameManager.Get().SetGameTypeSubMasks(Parameters.GameTypeToDisplay, newMask, delegate(SetGameSubTypeResponse r)
					{
						if (!r.Success)
						{
							string text = $"Failed to select game modes: {(r.LocalizedFailure == null ? r.ErrorMessage : r.LocalizedFailure.ToString())}";
							Log.Warning(text);
							UIDialogPopupManager.OpenOneButtonDialog(
								StringUtil.TR("Error", "Global"),
								text, 
								StringUtil.TR("Ok", "Global"));
						}
						else
						{
							ClientGameManager.Get().SetSoloSubGameMask(Parameters.GameTypeToDisplay, newMask);
							UpdateSubTypeMaskChecks(newMask);
						}
					});
				}
			}
			else
			{
				HydrogenConfig.Get().SaveGameTypeSubMaskPreference(Parameters.GameTypeToDisplay, newMask, ClientGameManager.Get().GameTypeAvailabilies);
				ClientGameManager.Get().SetGameTypeSubMasks(Parameters.GameTypeToDisplay, newMask, delegate(SetGameSubTypeResponse r)
				{
					if (!r.Success)
					{
						string text = $"Failed to select game modes: {(r.LocalizedFailure != null ? r.LocalizedFailure.ToString() : r.ErrorMessage)}";
						Log.Warning(text);
						UIDialogPopupManager.OpenOneButtonDialog(
							StringUtil.TR("Error", "Global"),
							text,
							StringUtil.TR("Ok", "Global"));
					}
					else
					{
						ClientGameManager.Get().SetSoloSubGameMask(Parameters.GameTypeToDisplay, newMask);
						UpdateSubTypeMaskChecks(newMask);
					}
				});
			}
		}
		UpdateWillFillVisibility();
		DoRefreshFunctions((ushort)RefreshFunctionType.RefreshPartyList);
	}

	public void EditedSearchInput(string input)
	{
		UpdateCharacterButtonHighlights();
	}

	public void ClickedOnFactionFilter(UICharacterSelectFactionFilter btn)
	{
		if (m_lastFilterBtnClicked != null && m_lastFilterBtnClicked != btn)
		{
			m_lastFilterBtnClicked.m_btn.SetSelected(false, false, string.Empty, string.Empty);
		}
		m_lastFilterBtnClicked = btn;
		UpdateCharacterButtonHighlights();
	}

	private void UpdateCharacterButtonHighlights()
	{
		foreach (UICharacterPanelSelectButton characterSelectButton in CharacterSelectButtons)
		{
			if (characterSelectButton != null && characterSelectButton.GetComponent<CanvasGroup>() != null)
			{
				CanvasGroup component = characterSelectButton.GetComponent<CanvasGroup>();
				if (component != null)
				{
					component.alpha = 1f;
				}
			}
		}
		if (m_lastFilterBtnClicked != null && m_lastFilterBtnClicked.m_btn.IsSelected())
		{
			foreach (UICharacterPanelSelectButton characterSelectButton in CharacterSelectButtons)
			{
				if (!m_lastFilterBtnClicked.IsAvailable(characterSelectButton.m_characterType))
				{
					CanvasGroup component2 = characterSelectButton.GetComponent<CanvasGroup>();
					if (component2 != null)
					{
						component2.alpha = 0.3f;
					}
				}
			}
		}
		if (!m_searchInputField.text.IsNullOrEmpty())
		{
			foreach (UICharacterPanelSelectButton characterSelectButton in CharacterSelectButtons)
			{
				CharacterResourceLink characterResourceLink = characterSelectButton.GetCharacterResourceLink();
				if (characterResourceLink != null)
				{
					string displayName = characterResourceLink.GetDisplayName();
					if (!DoesSearchMatchDisplayName(m_searchInputField.text.ToLower(), displayName.ToLower()))
					{
						CanvasGroup component3 = characterSelectButton.GetComponent<CanvasGroup>();
						if (component3 != null)
						{
							component3.alpha = 0.3f;
						}
					}
				}
			}
		}
	}

	private bool DoesSearchMatchDisplayName(string searchText, string displayText)
	{
		int i = 0;
		while (i < searchText.Length)
		{
			if (i >= displayText.Length)
			{
				return true;
			}
			if (searchText[i] != displayText[i])
			{
				return false;
			}
			i++;
		}
		return true;
	}

	public void SubTypeClicked(_ToggleSwap btn)
	{
		CheckSubTypeSelection(true);
	}

	private void SetupButtons()
	{
		m_bioBtn.spriteController.callback = BioBtnClicked;
		m_skinsBtn.spriteController.callback = SkinsBtnClicked;
		m_AbilitiesBtn.spriteController.callback = AbilitiesBtnClicked;
		m_CatalystBtn.spriteController.callback = CatalystsBtnClicked;
		m_TauntsBtn.spriteController.callback = TauntsBtnClicked;
		m_skinsBtn.spriteController.pointerEnterCallback = SkinMouseOver;
		m_skinsBtn.spriteController.pointerExitCallback = SkinMouseExit;
		m_AbilitiesBtn.spriteController.pointerEnterCallback = AbilityMouseOver;
		m_AbilitiesBtn.spriteController.pointerExitCallback = AbilityMouseExit;
		m_CatalystBtn.spriteController.pointerEnterCallback = CatalystMouseOver;
		m_CatalystBtn.spriteController.pointerExitCallback = CatalystMouseExit;
		SkinSubButtons = m_skinsBtn.GetComponentsInChildren<_ButtonSwapSprite>(true);
		AbilitySubButtons = m_AbilitiesBtn.GetComponentsInChildren<_ButtonSwapSprite>(true);
		CatalystSubButtons = m_CatalystBtn.GetComponentsInChildren<_ButtonSwapSprite>(true);
		foreach (_ButtonSwapSprite skinSubButton in SkinSubButtons)
		{
			if (skinSubButton != m_skinsBtn.spriteController)
			{
				m_skinsBtn.spriteController.AddSubButton(skinSubButton);
			}
		}
		foreach (_ButtonSwapSprite abilitySubButton in AbilitySubButtons)
		{
			if (abilitySubButton != m_AbilitiesBtn.spriteController)
			{
				m_AbilitiesBtn.spriteController.AddSubButton(abilitySubButton);
			}
		}
		foreach (_ButtonSwapSprite catalystSubButton in CatalystSubButtons)
		{
			if (catalystSubButton != m_CatalystBtn.spriteController)
			{
				m_CatalystBtn.spriteController.AddSubButton(catalystSubButton);
			}
		}
		m_skinsBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, tooltip => SideMenuOpen(tooltip, m_skinsBtn));
		m_AbilitiesBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, tooltip => SideMenuOpen(tooltip, m_AbilitiesBtn));
		m_CatalystBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, tooltip => SideMenuOpen(tooltip, m_CatalystBtn));
		for (int i = 0; i < m_AbilityMouseOverBtns.Length; i++)
		{
			m_AbilityMouseOverBtns[i].spriteController.callback = ClickedAbilityIcon;
			int index = i;
			m_AbilityMouseOverBtns[i].spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, tooltip => SetupAbilitySideBtnTooltip(tooltip, index));
		}
		foreach (Image abilityModIcon in m_AbilityModIcons)
		{
			UIManager.SetGameObjectActive(abilityModIcon, false);
		}
		for (int i = 0; i < m_CatalsytBtns.Length; i++)
		{
			m_CatalsytBtns[i].spriteController.callback = ClickedCatalystIcon;
			AbilityRunPhase phase = i + AbilityRunPhase.Prep;
			m_CatalsytBtns[i].spriteController.GetComponent<UITooltipHoverObject>().Setup(
				TooltipType.Titled,
				tooltip => SelectedCatalysts.ContainsKey(phase)
				           && SetupCatalystSideBtnTooltip(tooltip, SelectedCatalysts[phase]));
		}
		if (m_selectedSkinColorBtn != null)
		{
			m_selectedSkinColorBtn.spriteController.callback = ClickedSkinIcon;
			m_selectedSkinColorBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, SetupSkinTooltip);
		}
		m_easyBtn.spriteController.callback = EasyClicked;
		m_mediumBtn.spriteController.callback = MediumClicked;
		m_hardBtn.spriteController.callback = HardClicked;
		m_teamBotsToggle.changedNotify = AllyBotsToggleEvent;
		m_simpleCogBtn.spriteController.callback = SimpleBtnClicked;
		m_advancedCogBtn.spriteController.callback = AdvancedBtnClicked;
		m_dropdownBtn.spriteController.callback = DropdownClicked;
	}

	public void DropdownClicked(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_DifficultyListDropdown, !m_DifficultyListDropdown.gameObject.activeSelf);
	}

	public void SimpleBtnClicked(BaseEventData data)
	{
		UIManager.Get().HandleNewSceneStateParameter(new CharacterSelectSceneStateParameters
		{
			BotDifficultyView = CharacterSelectSceneStateParameters.BotDifficultyViewType.Simple
		});
	}

	public void AdvancedBtnClicked(BaseEventData data)
	{
		UIManager.Get().HandleNewSceneStateParameter(new CharacterSelectSceneStateParameters
		{
			BotDifficultyView = CharacterSelectSceneStateParameters.BotDifficultyViewType.Advanced
		});
	}

	public void AllyBotsToggleEvent(_ToggleSwap btn)
	{
		UIManager.Get().HandleNewSceneStateParameter(new CharacterSelectSceneStateParameters
		{
			ClientRequestAllyBotTeammates = btn.IsChecked()
		});
	}

	public void EasyClicked(BaseEventData data)
	{
		UIManager.Get().HandleNewSceneStateParameter(new CharacterSelectSceneStateParameters
		{
			ClientRequestedSimpleBotSettingValue = CharacterSelectSceneStateParameters.SimpleBotSettingValue.Easy
		});
	}

	public void MediumClicked(BaseEventData data)
	{
		UIManager.Get().HandleNewSceneStateParameter(new CharacterSelectSceneStateParameters
		{
			ClientRequestedSimpleBotSettingValue = CharacterSelectSceneStateParameters.SimpleBotSettingValue.Medium
		});
	}

	public void HardClicked(BaseEventData data)
	{
		UIManager.Get().HandleNewSceneStateParameter(new CharacterSelectSceneStateParameters
		{
			ClientRequestedSimpleBotSettingValue = CharacterSelectSceneStateParameters.SimpleBotSettingValue.Hard
		});
	}

	private bool SetupSkinTooltip(UITooltipBase tooltip)
	{
		UICharacterSelectSkinPanel skinPanel = UICharacterSelectCharacterSettingsPanel.Get().m_skinsSubPanel.m_selectHandler as UICharacterSelectSkinPanel;
		if (skinPanel == null)
		{
			return false;
		}
		
		GameWideData gameWideData = GameWideData.Get();
		CharacterType characterType = GetCurrentSpecificState().ClientSelectedCharacter.Value;
		CharacterResourceLink characterResourceLink = gameWideData.GetCharacterResourceLink(characterType);
		CharacterVisualInfo characterVisual = GetCurrentSpecificState().ClientSelectedVisualInfo.Value;
		
		if (characterResourceLink == null
		    || characterResourceLink.m_skins.Count <= characterVisual.skinIndex
		    || characterResourceLink.m_skins[characterVisual.skinIndex].m_patterns.Count <= characterVisual.patternIndex
		    || characterResourceLink.m_skins[characterVisual.skinIndex].m_patterns[characterVisual.patternIndex].m_colors.Count
		    <= characterVisual.colorIndex)
		{
			return false;
		}
		string patternColorName = characterResourceLink.GetPatternColorName(
			characterVisual.skinIndex,
			characterVisual.patternIndex,
			characterVisual.colorIndex);
		(tooltip as UITitledTooltip).Setup(
			characterResourceLink.GetDisplayName(),
			string.Format(StringUtil.TR("SelectedStyle", "Global"), patternColorName),
			string.Empty);
		return true;
	}

	private bool SetupCatalystSideBtnTooltip(UITooltipBase tooltip, Card card)
	{
		if (!m_CatalystBtn.IsHover)
		{
			return false;
		}
		
		string tooltipText = card.GetDisplayName();
		if (!card.m_useAbility.m_flavorText.IsNullOrEmpty())
		{
			tooltipText = string.Concat(tooltipText, Environment.NewLine, "<i>", card.m_useAbility.m_flavorText, "</i>");
		}

		(tooltip as UITitledTooltip).Setup(
			string.Format(StringUtil.TR("CatalystTitle", "Global"),
				card.m_useAbility.GetPhaseString()),
			tooltipText, 
			string.Empty);
		return true;
	}

	private bool SetupAbilitySideBtnTooltip(UITooltipBase tooltip, int i)
	{
		if (!m_AbilitiesBtn.IsHover)
		{
			return false;
		}
		
		AbilityData.AbilityEntry abilityEntry = SelectedAbilityData[i].GetAbilityEntry();
		if (abilityEntry == null || abilityEntry.ability == null)
		{
			return false;
		}
		
		UIAbilityTooltip uiabilityTooltip = (UIAbilityTooltip)tooltip;
		string movieAssetName = "Video/AbilityPreviews/" + abilityEntry.ability.m_previewVideo;
		uiabilityTooltip.Setup(abilityEntry.ability, SelectedAbilityData[i].GetSelectedMod(), movieAssetName);
		return true;
	}

	public void UpdateCatalystIcons(Dictionary<AbilityRunPhase, Card> phaseToCards)
	{
		SelectedCatalysts = phaseToCards;
		foreach (Card card in SelectedCatalysts.Values)
		{
			int phaseIndex = card.GetAbilityRunPhase() - AbilityRunPhase.Prep;
			if (0 <= phaseIndex && phaseIndex < m_CatalystIcons.Length)
			{
				UIManager.SetGameObjectActive(m_CatalystIcons[phaseIndex], true);
				m_CatalystIcons[phaseIndex].sprite = card.GetIconSprite();
			}
			if (0 <= phaseIndex && phaseIndex < m_CatalystHoverIcons.Length)
			{
				UIManager.SetGameObjectActive(m_CatalystHoverIcons[phaseIndex], true);
				m_CatalystHoverIcons[phaseIndex].sprite = card.GetIconSprite();
			}
		}
	}

	public void UpdateModIcons(UIAbilityButtonModPanel[] SelectedAbilities, Color prepColor, Color dashColor, Color combatColor)
	{
		SelectedAbilityData = SelectedAbilities;
		for (int i = 0; i < m_AbilityMouseOverBtns.Length; i++)
		{
			UIAbilityButtonModPanel uiabilityButtonModPanel = SelectedAbilities[i];
			m_AbilityIcons[i].sprite = uiabilityButtonModPanel.m_abilityIcon[0].sprite;
			UIQueueListPanel.UIPhase uiphase = UIQueueListPanel.UIPhase.None;
			AbilityData.AbilityEntry abilityEntry = uiabilityButtonModPanel.GetAbilityEntry();
			if (abilityEntry != null)
			{
				if (abilityEntry.ability != null)
				{
					uiphase = UIQueueListPanel.GetUIPhaseFromAbilityPriority(abilityEntry.ability.RunPriority);
				}
				else if (m_currentState != null && m_currentState.CharacterTypeToDisplay != CharacterType.PendingWillFill)
				{
					Log.Warning("Ability entry has no ability!");
				}
			}
			else
			{
				Log.Warning("AbilityButton has no Ability Entry!");
			}
			
			Color color;
			switch (uiphase)
			{
				case UIQueueListPanel.UIPhase.Prep:
					color = prepColor;
					break;
				case UIQueueListPanel.UIPhase.Evasion:
					color = dashColor;
					break;
				case UIQueueListPanel.UIPhase.Combat:
					color = combatColor;
					break;
				default:
					color = Color.gray;
					break;
			}
			
			if (i < m_AbilityPhaseColors.Length)
			{
				m_AbilityPhaseColors[i].color = color;
			}
			if (i < m_AbilityPhaseColorsGradient.Length)
			{
				m_AbilityPhaseColorsGradient[i].color = color;
			}
			if (uiabilityButtonModPanel.GetSelectedMod() != null)
			{
				UIManager.SetGameObjectActive(m_AbilityModIcons[i], true);
				m_AbilityModIcons[i].sprite = uiabilityButtonModPanel.GetSelectedMod().m_iconSprite;
			}
			else
			{
				UIManager.SetGameObjectActive(m_AbilityModIcons[i], false);
				m_AbilityModIcons[i].sprite = null;
			}
		}
	}

	public void ClickedSkinIcon(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		UICharacterSelectCharacterSettingsPanel.Get().SetVisible(true, UICharacterSelectCharacterSettingsPanel.TabPanel.Skins);
	}

	public void ClickedCatalystIcon(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		UICharacterSelectCharacterSettingsPanel.Get().SetVisible(true, UICharacterSelectCharacterSettingsPanel.TabPanel.Catalysts);
	}

	public void ClickedAbilityIcon(BaseEventData data)
	{
		int clickedIndex = -1;
		for (int i = 0; i < m_AbilityMouseOverBtns.Length; i++)
		{
			if (m_AbilityMouseOverBtns[i].spriteController.gameObject == (data as PointerEventData).selectedObject)
			{
				clickedIndex = i;
				break;
			}
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		UICharacterSelectCharacterSettingsPanel.Get().SetVisible(true, UICharacterSelectCharacterSettingsPanel.TabPanel.Abilities);
		if (clickedIndex > -1)
		{
			UICharacterSelectCharacterSettingsPanel.Get().m_abilitiesSubPanel.AbilityButtonSelected(clickedIndex);
		}
	}

	private bool SideMenuOpen(UITooltipBase tooltip, _SelectableBtn btn)
	{
		if (btn != m_CatalystBtn)
		{
			return false;
		}
		if (!GameManager.Get().GameplayOverrides.EnableCards)
		{
			m_CatalystBtn.spriteController.SetClickable(false);
			m_CatalystBtn.spriteController.SetForceHovercallback(true);
			m_CatalystBtn.spriteController.SetForceExitCallback(true);
			(tooltip as UITitledTooltip).Setup(
				StringUtil.TR("Disabled", "Global"),
				StringUtil.TR("CatalystsAreDisabled", "Global"),
				string.Empty);
			return true;
		}
		else
		{
			m_CatalystBtn.spriteController.SetClickable(true);
			m_CatalystBtn.spriteController.SetForceHovercallback(false);
			m_CatalystBtn.spriteController.SetForceExitCallback(false);
			return false;
		}
	}

	public void SkinMouseOver(BaseEventData data)
	{
		foreach (_ButtonSwapSprite skinSubButton in SkinSubButtons)
		{
			if (skinSubButton != m_skinsBtn.spriteController)
			{
				skinSubButton.SetClickable(true);
			}
		}
	}

	public void SkinMouseExit(BaseEventData data)
	{
		foreach (_ButtonSwapSprite skinSubButton in SkinSubButtons)
		{
			if (skinSubButton != m_skinsBtn.spriteController)
			{
				skinSubButton.SetClickable(false);
			}
		}
	}

	public void AbilityMouseOver(BaseEventData data)
	{
		foreach (_ButtonSwapSprite abilitySubButton in AbilitySubButtons)
		{
			if (abilitySubButton != m_AbilitiesBtn.spriteController)
			{
				abilitySubButton.SetClickable(true);
			}
		}
	}

	public void AbilityMouseExit(BaseEventData data)
	{
		foreach (_ButtonSwapSprite abilitySubButton in AbilitySubButtons)
		{
			if (abilitySubButton != m_AbilitiesBtn.spriteController)
			{
				abilitySubButton.SetClickable(false);
			}
		}
	}

	public void CatalystMouseOver(BaseEventData data)
	{
		foreach (_ButtonSwapSprite catalystSubButton in CatalystSubButtons)
		{
			if (catalystSubButton != m_CatalystBtn.spriteController)
			{
				catalystSubButton.SetClickable(true);
			}
		}
	}

	public void CatalystMouseExit(BaseEventData data)
	{
		foreach (_ButtonSwapSprite catalystSubButton in CatalystSubButtons)
		{
			if (catalystSubButton != m_CatalystBtn.spriteController)
			{
				catalystSubButton.SetClickable(false);
			}
		}
	}

	public void BioBtnClicked(BaseEventData data)
	{
		UICharacterSelectCharacterSettingsPanel.Get().SetVisible(true, UICharacterSelectCharacterSettingsPanel.TabPanel.General);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectOpen);
	}

	public void SkinsBtnClicked(BaseEventData data)
	{
		UICharacterSelectCharacterSettingsPanel.Get().SetVisible(true, UICharacterSelectCharacterSettingsPanel.TabPanel.Skins);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectOpen);
	}

	public void AbilitiesBtnClicked(BaseEventData data)
	{
		UICharacterSelectCharacterSettingsPanel.Get().SetVisible(true, UICharacterSelectCharacterSettingsPanel.TabPanel.Abilities);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectOpen);
	}

	public void CatalystsBtnClicked(BaseEventData data)
	{
		UICharacterSelectCharacterSettingsPanel.Get().SetVisible(true, UICharacterSelectCharacterSettingsPanel.TabPanel.Catalysts);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectOpen);
	}

	public void TauntsBtnClicked(BaseEventData data)
	{
		UICharacterSelectCharacterSettingsPanel.Get().SetVisible(true, UICharacterSelectCharacterSettingsPanel.TabPanel.Taunts);
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectOpen);
	}

	public void CharacterSelectionResponseHandler(PlayerInfoUpdateResponse response)
	{
		GetCurrentSpecificState().ClientRequestToServerSelectCharacter = null;
		if (response.Success && response.CharacterInfo != null)
		{
			UIManager.Get().HandleNewSceneStateParameter(new CharacterSelectSceneStateParameters
			{
				ClientSelectedCharacter = response.CharacterInfo.CharacterType
			});
			UICharacterSelectScreenController uicharacterSelectScreenController = UICharacterSelectScreenController.Get();
			if (uicharacterSelectScreenController != null)
			{
				uicharacterSelectScreenController.UpdatePrimaryCharacter(response.CharacterInfo);
			}
		}
	}

	private bool IsCharacterValidForSelection(CharacterType characterType)
	{
		bool result = false;
		if (characterType == CharacterType.None)
		{
			return false;
		}
		GameType gameTypeToDisplay = GetCurrentSpecificState().GameTypeToDisplay;
		if (GameManager.Get() != null && GameManager.Get().IsValidForHumanPreGameSelection(characterType))
		{
			GameType gameType = GameManager.Get().GameConfig != null
			                    && GameManager.Get().GameStatus != GameStatus.Stopped
				? GameManager.Get().GameConfig.GameType
				: ClientGameManager.Get().GroupInfo.SelectedQueueType;
			result = GameManager.Get().IsCharacterAllowedForGameType(characterType, gameType, null, null);
		}
		if (result)
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(characterType);
			bool isUnlocked = playerCharacterData != null
			             && playerCharacterData.CharacterComponent != null
			             && playerCharacterData.CharacterComponent.Unlocked;
			bool isValid = SceneStateParameters.IsInGameLobby
				? GameManager.Get().IsCharacterAllowedForPlayers(characterType)
				: GameManager.Get().IsValidForHumanPreGameSelection(characterType);
			bool isAllowed = GameManager.Get().IsCharacterAllowedForGameType(characterType, gameTypeToDisplay, null, null);
			bool isAvailable = ClientGameManager.Get().IsCharacterAvailable(characterType, gameTypeToDisplay);
			result = isValid && isAllowed && (isUnlocked || isAvailable);
		}
		if (result
		    && SceneStateParameters.IsInGameLobby
		    && GameManager.Get().TeamInfo != null
		    && gameTypeToDisplay != GameType.Custom)
		{
			LobbyPlayerInfo playerInfo = GameManager.Get().PlayerInfo;
			Team team = playerInfo.TeamId;
			if (team == Team.Spectator)
			{
				team = Team.TeamA;
			}
			List<LobbyPlayerInfo> players = (from ti in GameManager.Get().TeamInfo.TeamInfo(team) 
				orderby ti.PlayerId != playerInfo.PlayerId ? 1 : 0 
				select ti)
				.ToList();
			foreach (LobbyPlayerInfo player in players)
			{
				if (player.PlayerId != playerInfo.PlayerId
				    && player.CharacterType == characterType
				    && GameManager.Get().IsFreelancerConflictPossible(player.TeamId == playerInfo.TeamId)
				    && !player.IsNPCBot)
				{
					return false;
				}
			}
		}
		return result;
	}

	public void ReceivedGameTypeChangeResponse()
	{
		GetCurrentSpecificState().ClientRequestedGameType = null;
		DoRefreshFunctions((ushort)RefreshFunctionType.RefreshSelectedGameType);
	}

	private void SetDropdownText(string text)
	{
		TextMeshProUGUI[] componentsInChildren = m_dropdownBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
		foreach (TextMeshProUGUI dropdownText in componentsInChildren)
		{
			dropdownText.text = text;
		}
	}

	public void RefreshPartyList()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		UIManager.SetGameObjectActive(m_partyListPanel, currentSpecificState.PartyListVisbility);
		m_partyListPanel.SetVisible(currentSpecificState.PartyListVisbility);
		if ((!currentSpecificState.CustomGamePartyIsVisible || currentSpecificState.CustomGamePartyIsHidden)
		    && currentSpecificState.PartyListVisbility
		    && currentSpecificState.GameTypeToDisplay != GameType.Custom)
		{
			bool isDuplicateCharsAllowed = false;
			int maxTeamAPlayers = -1;
			foreach (GameSubType gameSubType in currentSpecificState.SelectedGameSubTypes.Values)
			{
				if (gameSubType.HasMod(GameSubType.SubTypeMods.ControlAllBots) && gameSubType.TeamAPlayers > maxTeamAPlayers)
				{
					maxTeamAPlayers = gameSubType.TeamAPlayers;
				}
			}
			if (maxTeamAPlayers < 0)
			{
				GameType selectedQueueType = ClientGameManager.Get().GroupInfo.SelectedQueueType;
				maxTeamAPlayers = ClientGameManager.Get().GameTypeAvailabilies[selectedQueueType].TeamAPlayers;
			}
			m_partyListPanel.SetupForOutOfGame(maxTeamAPlayers, isDuplicateCharsAllowed);
		}
	}

	public void RefreshGameSubTypes()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		ushort prevCheckedMask = 0;
		bool sendMaskUpdate = false;
		if (currentSpecificState.ClientRequestAllyBotTeammates != null)
		{
			foreach (GameSubTypeState gameSubTypeBtn in m_gameSubTypeBtns)
			{
				if (gameSubTypeBtn.btn.IsChecked())
				{
					prevCheckedMask |= gameSubTypeBtn.SubTypeBit;
				}
			}
			currentSpecificState.AllyBotTeammatesSelected = currentSpecificState.ClientRequestAllyBotTeammates.Value;
			sendMaskUpdate = true;
			currentSpecificState.ClientRequestAllyBotTeammates = null;
		}
		if (currentSpecificState.ClientRequestedGameType != null)
		{
			sendMaskUpdate = true;
		}
		m_gameSubTypeBtns.Clear();
		GameType gameTypeToDisplay = currentSpecificState.GameTypeToDisplay;
		if (ClientGameManager.Get().GameTypeAvailabilies.ContainsKey(gameTypeToDisplay))
		{
			bool inAGroup = ClientGameManager.Get().GroupInfo.InAGroup;
			int j = 0;
			Dictionary<ushort, GameSubType> validGameSubTypes = currentSpecificState.ValidGameSubTypes;
			if (!validGameSubTypes.IsNullOrEmpty() && validGameSubTypes.Count > 1)
			{
				ushort selectedMask = ClientGameManager.Get().GroupInfo.InAGroup && !ClientGameManager.Get().GroupInfo.IsLeader
					? ClientGameManager.Get().GroupInfo.SubTypeMask
					: ClientGameManager.Get().GetSoloSubGameMask(gameTypeToDisplay);
				ushort consolidatedBit = 0;
				foreach (KeyValuePair<ushort, GameSubType> keyValuePair in validGameSubTypes)
				{
					bool isActive = IsGameSubTypeActive(gameTypeToDisplay, keyValuePair.Value);
					if (isActive && keyValuePair.Value.HasMod(GameSubType.SubTypeMods.NotAllowedForGroups))
					{
						isActive = !inAGroup;
					}
					if (!isActive || !keyValuePair.Value.HasMod(GameSubType.SubTypeMods.CanBeConsolidated))
					{
						while (j >= m_gameTypeButtons.Count)
						{
							_ToggleSwap toggleSwap = Instantiate(m_GameTypePrefab);
							toggleSwap.transform.SetParent(m_GameTypeContainer.transform);
							toggleSwap.transform.localEulerAngles = Vector3.zero;
							toggleSwap.transform.localScale = Vector3.one;
							toggleSwap.transform.localPosition = Vector3.zero;
							m_gameTypeButtons.Add(toggleSwap);
						}
						m_gameSubTypeBtns.Add(new GameSubTypeState
						{
							btn = m_gameTypeButtons[j],
							SubTypeBit = keyValuePair.Key
						});
						UIManager.SetGameObjectActive(m_gameTypeButtons[j], isActive);
						TextMeshProUGUI componentInChildren = m_gameTypeButtons[j].GetComponentInChildren<TextMeshProUGUI>(true);
						componentInChildren.text = StringUtil.TR(keyValuePair.Value.LocalizedName);
						m_gameTypeButtons[j].changedNotify = SubTypeClicked;
						m_gameTypeButtons[j].SetOn(isActive && (selectedMask & keyValuePair.Key) != 0);
						j++;
					}
					else
					{
						consolidatedBit |= keyValuePair.Key;
					}
				}
				if (consolidatedBit != 0)
				{
					while (j >= m_gameTypeButtons.Count)
					{
						_ToggleSwap toggleSwap2 = Instantiate(m_GameTypePrefab);
						toggleSwap2.transform.SetParent(m_GameTypeContainer.transform);
						toggleSwap2.transform.localEulerAngles = Vector3.zero;
						toggleSwap2.transform.localScale = Vector3.one;
						toggleSwap2.transform.localPosition = Vector3.zero;
						m_gameTypeButtons.Add(toggleSwap2);
					}
					m_gameSubTypeBtns.Add(new GameSubTypeState
					{
						btn = m_gameTypeButtons[j],
						SubTypeBit = consolidatedBit
					});
					UIManager.SetGameObjectActive(m_gameTypeButtons[j], true);
					m_gameTypeButtons[j].SetOn((selectedMask & consolidatedBit) != 0);
					TextMeshProUGUI componentInChildren2 = m_gameTypeButtons[j].GetComponentInChildren<TextMeshProUGUI>(true);
					componentInChildren2.text = StringUtil.TR("ConsolidatedGameSubTypes", "SubTypes");
					m_gameTypeButtons[j].changedNotify = SubTypeClicked;
					j++;
				}
			}
			while (j < m_gameTypeButtons.Count)
			{
				UIManager.SetGameObjectActive(m_gameTypeButtons[j], false);
				j++;
			}
		}
		UIManager.SetGameObjectActive(m_GameTypeContainer, currentSpecificState.GameSubTypesVisible);
		CheckSubTypeSelection(sendMaskUpdate, prevCheckedMask);
	}

	private void SendBotDifficultyUpdateToServer(BotDifficulty? AllyDifficulty, BotDifficulty? EnemyDifficulty)
	{
		if (GameManager.Get().TeamInfo == null || GameManager.Get().TeamInfo.TeamBPlayerInfo.IsNullOrEmpty())
		{
			if (GameManager.Get().QueueInfo != null && EnemyDifficulty != null)
			{
				ClientGameManager.Get().LobbyInterface.UpdateQueueEnemyBotDifficulty(GameManager.Get().QueueInfo, EnemyDifficulty.Value);
			}
			return;
		}
		if (ClientGameManager.Get().GroupInfo != null
		    && ClientGameManager.Get().GroupInfo.InAGroup
		    && ClientGameManager.Get().GroupInfo.IsLeader)
		{
			ClientGameManager.Get().UpdateBotDifficulty(AllyDifficulty, EnemyDifficulty);
			return;
		}
		if (GameManager.Get().TeamInfo != null)
		{
			foreach (LobbyPlayerInfo lobbyPlayerInfo in GameManager.Get().TeamInfo.TeamBPlayerInfo)
			{
				ClientGameManager.Get().UpdateBotDifficulty(AllyDifficulty, EnemyDifficulty, lobbyPlayerInfo.PlayerId);
			}
		}
	}

	public void RefreshBotSkillPanel()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		GameType gameTypeToDisplay = currentSpecificState.GameTypeToDisplay;
		bool isEditable = !SceneStateParameters.IsInGameLobby && !SceneStateParameters.IsInQueue && !SceneStateParameters.IsGroupSubordinate;
		m_teamBotStars.SetClickable(isEditable);
		m_enemyBotStars.SetClickable(isEditable);
		m_teamBotsToggle.SetClickable(isEditable);
		foreach (_ToggleSwap gameTypeButton in m_gameTypeButtons)
		{
			gameTypeButton.SetClickable(isEditable);
		}
		m_simpleCogBtn.spriteController.SetClickable(isEditable);
		m_advancedCogBtn.spriteController.SetClickable(isEditable);
		m_dropdownBtn.spriteController.SetClickable(isEditable);
		if (!isEditable)
		{
			UIManager.SetGameObjectActive(m_DifficultyListDropdown, false);
		}
		bool displayAllyBotTeammates = currentSpecificState.DisplayAllyBotTeammates;
		m_teamBotsToggle.SetOn(displayAllyBotTeammates);
		if (gameTypeToDisplay == GameType.Solo || gameTypeToDisplay == GameType.Coop)
		{
			string key = currentSpecificState.DisplayAllyBotTeammates ? "SoloEnemyDifficulty" : "CoopDifficulty";
			int enemyBotDifficultyToDisplay = currentSpecificState.EnemyBotDifficultyToDisplay;
			m_enemyBotStars.SetCurrentValue(enemyBotDifficultyToDisplay + 1);
			if (currentSpecificState.ClientRequestedEnemyBotDifficulty != null)
			{
				PlayerPrefs.SetInt(key, enemyBotDifficultyToDisplay);
				SendBotDifficultyUpdateToServer(null, (BotDifficulty)enemyBotDifficultyToDisplay);
				currentSpecificState.SelectedEnemyBotDifficulty = currentSpecificState.ClientRequestedEnemyBotDifficulty;
				currentSpecificState.ClientRequestedEnemyBotDifficulty = null;
			}
			int allyBotDifficultyToDisplay = currentSpecificState.AllyBotDifficultyToDisplay;
			m_teamBotStars.SetCurrentValue(allyBotDifficultyToDisplay + 1);
			if (currentSpecificState.ClientRequestedAllyBotDifficulty != null)
			{
				PlayerPrefs.SetInt("SoloAllyDifficulty", allyBotDifficultyToDisplay);
				SendBotDifficultyUpdateToServer((BotDifficulty)allyBotDifficultyToDisplay, null);
				currentSpecificState.SelectedAllyBotDifficulty = currentSpecificState.ClientRequestedAllyBotDifficulty;
				currentSpecificState.ClientRequestedAllyBotDifficulty = null;
			}
		}
		UIManager.SetGameObjectActive(m_botSkillPanel, currentSpecificState.BotSkillPanelVisible);
		UIManager.SetGameObjectActive(m_DifficultyListDropdown, false);
		bool isSimpleView = currentSpecificState.BotDifficultyViewTypeToDisplay == CharacterSelectSceneStateParameters.BotDifficultyViewType.Simple;
		if (isSimpleView)
		{
			UIManager.SetGameObjectActive(m_simpleCogBtn, false);
			UIManager.SetGameObjectActive(m_advancedCogBtn, true);
			UIManager.SetGameObjectActive(m_dropdownBtn, true);
			UIManager.SetGameObjectActive(m_difficultyListContainer, true);
			CharacterSelectSceneStateParameters.SimpleBotSettingValue simpleBotSettingValueToDisplay = currentSpecificState.SimpleBotSettingValueToDisplay;
			m_easyBtn.SetSelected(simpleBotSettingValueToDisplay == CharacterSelectSceneStateParameters.SimpleBotSettingValue.Easy, false, string.Empty, string.Empty);
			m_mediumBtn.SetSelected(simpleBotSettingValueToDisplay == CharacterSelectSceneStateParameters.SimpleBotSettingValue.Medium, false, string.Empty, string.Empty);
			m_hardBtn.SetSelected(simpleBotSettingValueToDisplay == CharacterSelectSceneStateParameters.SimpleBotSettingValue.Hard, false, string.Empty, string.Empty);
			switch (simpleBotSettingValueToDisplay)
			{
				case CharacterSelectSceneStateParameters.SimpleBotSettingValue.Easy:
				{
					SetDropdownText(StringUtil.TR("Easy", "Global"));
					m_enemyBotStars.SetCurrentValue(1);
					m_teamBotStars.SetCurrentValue(4);
					if (currentSpecificState.ClientRequestedSimpleBotSettingValue != null)
					{
						SendBotDifficultyUpdateToServer(BotDifficulty.Hard, BotDifficulty.Stupid);
					}

					break;
				}
				case CharacterSelectSceneStateParameters.SimpleBotSettingValue.Medium:
				{
					SetDropdownText(StringUtil.TR("Medium", "Global"));
					m_enemyBotStars.SetCurrentValue(2);
					m_teamBotStars.SetCurrentValue(3);
					if (currentSpecificState.ClientRequestedSimpleBotSettingValue != null)
					{
						SendBotDifficultyUpdateToServer(BotDifficulty.Medium, BotDifficulty.Easy);
					}

					break;
				}
				case CharacterSelectSceneStateParameters.SimpleBotSettingValue.Hard:
				{
					SetDropdownText(StringUtil.TR("Hard", "Global"));
					m_enemyBotStars.SetCurrentValue(4);
					m_teamBotStars.SetCurrentValue(2);
					if (currentSpecificState.ClientRequestedSimpleBotSettingValue != null)
					{
						SendBotDifficultyUpdateToServer(BotDifficulty.Easy, BotDifficulty.Hard);
					}

					break;
				}
			}

			if (currentSpecificState.ClientRequestedSimpleBotSettingValue != null)
			{
				currentSpecificState.SimpleBotSetting = currentSpecificState.ClientRequestedSimpleBotSettingValue;
				currentSpecificState.ClientRequestedSimpleBotSettingValue = null;
			}
		}
		else
		{
			UIManager.SetGameObjectActive(m_simpleCogBtn, true);
			UIManager.SetGameObjectActive(m_advancedCogBtn, false);
			UIManager.SetGameObjectActive(m_dropdownBtn, false);
			UIManager.SetGameObjectActive(m_difficultyListContainer, false);
		}
		UIManager.SetGameObjectActive(m_enemyBotSkillPanel, currentSpecificState.BotSkillPanelVisible && !isSimpleView);
		UIManager.SetGameObjectActive(m_teamBotSkillPanel, currentSpecificState.BotSkillPanelVisible && !isSimpleView);
		UIManager.SetGameObjectActive(m_teamBotsToggle, true);
	}

	public void RefreshSelectedGameType()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		if (currentSpecificState.ClientRequestedGameType != null)
		{
			GameType requestedGameType = currentSpecificState.ClientRequestedGameType.Value;
			if (ClientGameManager.Get().GroupInfo.InAGroup)
			{
				if (ClientGameManager.Get().GroupInfo.SelectedQueueType != requestedGameType)
				{
					ClientGameManager.Get().UpdateSelectedGameMode(requestedGameType);
				}
			}
			else
			{
				GetCurrentSpecificState().ClientRequestedGameType = null;
				SentInitialSubTypes = false;
				ClientGameManager.Get().GroupInfo.SelectedQueueType = requestedGameType;
			}
		}
		UpdateWillFillVisibility();
		m_partyListPanel.SetVisible(false);
	}

	public void UpdateWillFillVisibility()
	{
		if (UICharacterSelectScreenController.Get() != null)
		{
			CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
			int maxWillFill = ClientGameManager.Get().GameTypeAvailabilies.TryGetValue(currentSpecificState.GameTypeToDisplay, out var gameTypeAvailability)
				? gameTypeAvailability.MaxWillFillPerTeam
				: 0;
			foreach (GameSubType gameSubType in currentSpecificState.SelectedGameSubTypes.Values)
			{
				if (gameSubType.HasMod(GameSubType.SubTypeMods.ControlAllBots))
				{
					maxWillFill = 0;
				}
			}
			UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_miscCharSelectButtons, maxWillFill > 0);
			if (maxWillFill == 0 && GetCurrentSpecificState().CharacterTypeToDisplay.IsWillFill())
			{
				CharacterType characterType = ClientGameManager.Get().QueueRequirementApplicant.AvailableCharacters.Shuffled(new Random()).First();
				CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(characterType);
				UIManager.Get().HandleNewSceneStateParameter(new CharacterSelectSceneStateParameters
				{
					ClientRequestToServerSelectCharacter = characterResourceLink.m_characterType
				});
			}
		}
	}

	private void DoCharButtonSelection(CharacterType charTypeToMatch)
	{
		foreach (UICharacterPanelSelectButton uicharacterPanelSelectButton in CharacterSelectButtons)
		{
			if (uicharacterPanelSelectButton.m_characterType == charTypeToMatch)
			{
				uicharacterPanelSelectButton.SetSelected(true);
				UICharacterSelectScreenController.Get().UpdateBuyButtons();
			}
			else
			{
				uicharacterPanelSelectButton.SetSelected(false);
			}
		}
	}

	public void RefreshCharacterButtons()
	{
		foreach (UICharacterPanelSelectButton uicharacterPanelSelectButton in CharacterSelectButtons)
		{
			CharacterResourceLink characterResourceLink = uicharacterPanelSelectButton.GetCharacterResourceLink();
			if (characterResourceLink == null)
			{
				continue;
			}
			
			CharacterType characterType = characterResourceLink.m_characterType;
			bool isSelectable = IsCharacterValidForSelection(characterType);
			if (isSelectable && characterType == GetCurrentSpecificState().CharacterTypeToDisplay)
			{
				UICharacterSelectScreenController.Get().UpdateBuyButtons();
			}

			PersistedCharacterData playerCharacterData =
				ClientGameManager.Get().GetPlayerCharacterData(characterType);
			bool practiceGameTypeSelectedForQueue = SceneStateParameters.PracticeGameTypeSelectedForQueue;

			bool isAllowedForGameType = GameManager.Get().IsCharacterAllowedForPlayers(characterType)
			             && GameManager.Get()
				             .IsCharacterAllowedForGameType(
					             characterType,
					             GetCurrentSpecificState().GameTypeToDisplay,
					             null,
					             null);
			uicharacterPanelSelectButton.SetEnabled(isSelectable || practiceGameTypeSelectedForQueue && isAllowedForGameType, playerCharacterData);
			uicharacterPanelSelectButton.UpdateFreeRotationIcon();
		}
	}

	public void RefreshSelectedCharacterButton()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		if (currentSpecificState.ClientRequestToServerSelectCharacter != null)
		{
			CharacterType value = currentSpecificState.ClientRequestToServerSelectCharacter.Value;
			if (!IsCharacterValidForSelection(value) && SceneStateParameters.IsInGameLobby)
			{
				GetCurrentSpecificState().ClientRequestToServerSelectCharacter = null;
				DoCharButtonSelection(currentSpecificState.CharacterTypeToDisplay);
			}
			else
			{
				DoCharButtonSelection(value);
			}
		}
		else
		{
			DoCharButtonSelection(currentSpecificState.CharacterTypeToDisplay);
		}
	}

	public void RefreshCharacterButtonsVisibility()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		if (currentSpecificState.CharacterSelectButtonsVisible.Value)
		{
			UIManager.SetGameObjectActive(m_characterSelectAnimController, true);
			UIAnimationEventManager.Get().PlayAnimation(m_characterSelectAnimController, "CharacterSelectionIN", null, string.Empty, 0, 0f, true, true);
			UICharacterSelectScreenController.Get().m_charSettingsPanel.SetVisible(false);
			UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectOpen);
			UINewUserFlowManager.OnCharacterSelectDisplayed();
			UICharacterSelectWorldObjects.Get().PlayCameraAnimation("CamCloseupIN");
		}
		else
		{
			if (m_characterSelectAnimController.gameObject.activeSelf)
			{
				UIAnimationEventManager.Get().PlayAnimation(m_characterSelectAnimController, "CharacterSelectionOUT", null, string.Empty, 0, 0f, true, true);
			}
			UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectClose);
			UICharacterSelectWorldObjects.Get().PlayCameraAnimation("CamCloseupOUT");
		}
	}

	public void SendRequestToServerForCharacterSelect()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		CharacterType characterType = CharacterType.None;
		if (currentSpecificState.ClientRequestToServerSelectCharacter != null)
		{
			characterType = currentSpecificState.ClientRequestToServerSelectCharacter.Value;
		}
		if (characterType != CharacterType.None)
		{
			if (!IsCharacterValidForSelection(characterType) && SceneStateParameters.IsInGameLobby)
			{
				GetCurrentSpecificState().ClientRequestToServerSelectCharacter = null;
				return;
			}
			if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
			{
				ClientGameManager.Get().UpdateSelectedCharacter(characterType);
			}
			else if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
			{
				ClientGameManager.Get().UpdateSelectedCharacter(characterType);
			}
			else
			{
				if (UILandingPageScreen.Get() != null
					&& UILandingPageScreen.Get().CharacterInfoClicked != null
					&& UILandingPageScreen.Get().CharacterInfoClicked.Value == characterType)
				{
					ClientGameManager.Get().UpdateSelectedCharacter(characterType);
					return;
				}
				GetCurrentSpecificState().ClientRequestToServerSelectCharacter = null;
			}
		}
		else
		{
			GetCurrentSpecificState().ClientRequestToServerSelectCharacter = null;
		}
	}

	public void RefreshSideButtonsVisibility()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		bool sideButtonsVisibility = currentSpecificState.SideButtonsVisibility;
		UIManager.SetGameObjectActive(m_sideBtnContainer, sideButtonsVisibility);
		if (sideButtonsVisibility)
		{
			if (currentSpecificState.ClientSelectedCharacter != null
			    && currentSpecificState.ClientSelectedCharacter.Value.IsWillFill())
			{
				UIManager.SetGameObjectActive(m_bioBtn, false);
				UIManager.SetGameObjectActive(m_skinsBtn, true);
				UIManager.SetGameObjectActive(m_AbilitiesBtn, false);
				UIManager.SetGameObjectActive(m_CatalystBtn, false);
				UIManager.SetGameObjectActive(m_TauntsBtn, false);
			}
			else
			{
				UIManager.SetGameObjectActive(m_bioBtn, true);
				UIManager.SetGameObjectActive(m_skinsBtn, true);
				UIManager.SetGameObjectActive(m_AbilitiesBtn, true);
				UIManager.SetGameObjectActive(m_CatalystBtn, true);
				UIManager.SetGameObjectActive(m_TauntsBtn, true);
			}
		}
	}

	public void RefreshSideButtonsClickability()
	{
		bool isClickable = GetCurrentSpecificState().SideButtonsClickable.Value;
		m_bioBtn.spriteController.SetClickable(isClickable);
		m_skinsBtn.spriteController.SetClickable(isClickable);
		m_AbilitiesBtn.spriteController.SetClickable(isClickable);
		m_CatalystBtn.spriteController.SetClickable(isClickable);
		m_TauntsBtn.spriteController.SetClickable(isClickable);
		if (!isClickable)
		{
			m_bioBtn.spriteController.ResetMouseState();
			m_skinsBtn.spriteController.ResetMouseState();
			m_AbilitiesBtn.spriteController.ResetMouseState();
			m_CatalystBtn.spriteController.ResetMouseState();
			m_TauntsBtn.spriteController.ResetMouseState();
		}
	}

	public override SceneType GetSceneType()
	{
		return SceneType.CharacterSelect;
	}

	public static bool IsGameSubTypeActive(GameType gameType, GameSubType gst)
	{
		if (gst.Requirements.IsNullOrEmpty())
		{
			return true;
		}
		ClientGameManager clientGameManager = ClientGameManager.Get();
		foreach (QueueRequirement queueRequirement in gst.Requirements)
		{
			if (!queueRequirement.DoesApplicantPass(clientGameManager.QueueRequirementSystemInfo, clientGameManager.QueueRequirementApplicant, gameType, gst))
			{
				return false;
			}
		}
		return true;
	}

	public class CharacterSelectSceneStateParameters : SceneStateParameters
	{
		public const int SimpleEnemyBotEasyDifficulty = 0;
		public const int SimpleAllyBotEasyDifficulty = 3;
		public const int SimpleEnemyBotMediumDifficulty = 1;
		public const int SimpleAllyBotMediumDifficulty = 2;
		public const int SimpleEnemyBotHardDifficulty = 3;
		public const int SimpleAllyBotHardDifficulty = 1;

		public bool? SideButtonsVisible;
		public bool? SideButtonsClickable;
		public bool? CharacterSelectButtonsVisible;
		public bool? AllyBotTeammatesSelected;
		public bool? AllyBotTeammatesClickable;
		public bool? BotsCanTauntCheckboxEnabled;
		public bool? CustomGamePartyListVisible;
		public bool? CustomGamePartyListHidden;
		public int? SelectedAllyBotDifficulty;
		public int? SelectedEnemyBotDifficulty;
		public BotDifficultyViewType? BotDifficultyView;
		public SimpleBotSettingValue? SimpleBotSetting;
		public CharacterType? ClientSelectedCharacter;
		public CharacterVisualInfo? ClientSelectedVisualInfo;
		public SimpleBotSettingValue? ClientRequestedSimpleBotSettingValue;
		public int? ClientRequestedAllyBotDifficulty;
		public int? ClientRequestedEnemyBotDifficulty;
		public bool? ClientRequestAllyBotTeammates;
		public CharacterType? ClientRequestToServerSelectCharacter;
		public GameType? ClientRequestedGameType;
		public ushort? SelectedSubTypeMask;

		public bool PartyListVisbility
		{
			get
			{
				if (IsHUDHidden)
				{
					return false;
				}
				if (CustomGamePartyIsVisible)
				{
					return true;
				}
				GameManager gameManager = GameManager.Get();
				if (gameManager != null
				    && gameManager.GameStatus != GameStatus.Stopped
				    && gameManager.GameStatus != GameStatus.None
				    && gameManager.GameInfo != null)
				{
					return gameManager.GameInfo.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.ControlAllBots);
				}
				foreach (GameSubType gameSubType in SelectedGameSubTypes.Values)
				{
					if (gameSubType.HasMod(GameSubType.SubTypeMods.ControlAllBots))
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool CustomGamePartyIsVisible => CustomGamePartyListVisible != null && CustomGamePartyListVisible.Value;

		public bool CustomGamePartyIsHidden => CustomGamePartyListHidden == null || CustomGamePartyListHidden.Value;

		public bool SideButtonsVisibility =>
			(UIGameSettingsPanel.Get() == null || !UIGameSettingsPanel.Get().m_lastVisible)
			&& SideButtonsVisible != null
			&& SideButtonsVisible.Value;

		public int AllyBotDifficultyToDisplay
		{
			get
			{
				if (BotDifficultyViewTypeToDisplay == BotDifficultyViewType.Advanced)
				{
					if (ClientRequestedAllyBotDifficulty != null)
					{
						return ClientRequestedAllyBotDifficulty.Value;
					}
					if (SelectedAllyBotDifficulty != null)
					{
						return SelectedAllyBotDifficulty.Value;
					}
				}
				else
				{
					switch (SimpleBotSettingValueToDisplay)
					{
						case SimpleBotSettingValue.Easy:
							return SimpleAllyBotEasyDifficulty;
						case SimpleBotSettingValue.Medium:
							return SimpleAllyBotMediumDifficulty;
						case SimpleBotSettingValue.Hard:
							return SimpleAllyBotHardDifficulty;
					}
				}
				return PlayerPrefs.GetInt("SoloAllyDifficulty", 4);
			}
		}

		public int EnemyBotDifficultyToDisplay
		{
			get
			{
				if (BotDifficultyViewTypeToDisplay == BotDifficultyViewType.Advanced)
				{
					if (ClientRequestedEnemyBotDifficulty != null)
					{
						return ClientRequestedEnemyBotDifficulty.Value;
					}
					if (SelectedEnemyBotDifficulty != null)
					{
						return SelectedEnemyBotDifficulty.Value;
					}
				}
				else
				{
					switch (SimpleBotSettingValueToDisplay)
					{
						case SimpleBotSettingValue.Easy:
							return SimpleEnemyBotEasyDifficulty;
						case SimpleBotSettingValue.Medium:
							return SimpleEnemyBotMediumDifficulty;
						case SimpleBotSettingValue.Hard:
							return SimpleEnemyBotHardDifficulty;
					}
				}
				return PlayerPrefs.GetInt("SoloEnemyDifficulty", SimpleAllyBotMediumDifficulty);
			}
		}

		public ushort ExclusiveModBitsOfGameTypeToDisplay
		{
			get
			{
				ushort exclusiveBits = 0;
				Dictionary<ushort, GameSubType> validGameSubTypes = ValidGameSubTypes;
				foreach (KeyValuePair<ushort, GameSubType> keyValuePair in validGameSubTypes)
				{
					if (keyValuePair.Value.HasMod(GameSubType.SubTypeMods.Exclusive))
					{
						exclusiveBits |= keyValuePair.Key;
					}
				}
				return exclusiveBits;
			}
		}

		public Dictionary<ushort, GameSubType> ValidGameSubTypes
		{
			get
			{
				if (GameTypeToDisplay != GameType.Coop)
				{
					return ClientGameManager.Get().GetGameTypeSubTypes(GameTypeToDisplay);
				}
				
				Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(GameTypeToDisplay);
				Dictionary<ushort, GameSubType> dictionary = new Dictionary<ushort, GameSubType>();
				Dictionary<ushort, GameSubType> dictionary2 = new Dictionary<ushort, GameSubType>();
				foreach (KeyValuePair<ushort, GameSubType> keyValuePair in gameTypeSubTypes)
				{
					if (keyValuePair.Value.HasMod(GameSubType.SubTypeMods.ShowWithAITeammates))
					{
						dictionary2[keyValuePair.Key] = keyValuePair.Value;
					}
					else
					{
						dictionary[keyValuePair.Key] = keyValuePair.Value;
					}
				}

				return DisplayAllyBotTeammates ? dictionary2 : dictionary;
			}
		}

		public bool GameSubTypesVisible =>
			GameTypeToDisplay != GameType.Custom
			&& !ValidGameSubTypes.IsNullOrEmpty()
			&& ValidGameSubTypes.Count > 1;

		public Dictionary<ushort, GameSubType> SelectedGameSubTypes
		{
			get
			{
				Dictionary<ushort, GameSubType> dictionary = new Dictionary<ushort, GameSubType>();
				foreach (KeyValuePair<ushort, GameSubType> keyValuePair in ValidGameSubTypes)
				{
					if (SelectedSubTypeMask == null
					    || (keyValuePair.Key & SelectedSubTypeMask.Value) != 0)
					{
						dictionary[keyValuePair.Key] = keyValuePair.Value;
					}
				}
				return dictionary;
			}
		}

		public bool DisplayAllyBotTeammates =>
			ClientRequestAllyBotTeammates != null
				? ClientRequestAllyBotTeammates.Value
				: AllyBotTeammatesSelected != null && AllyBotTeammatesSelected.Value;

		public SimpleBotSettingValue SimpleBotSettingValueToDisplay =>
			ClientRequestedSimpleBotSettingValue != null
				? ClientRequestedSimpleBotSettingValue.Value
				: SimpleBotSetting != null
					? SimpleBotSetting.Value
					: SimpleBotSettingValue.Easy;

		public BotDifficultyViewType BotDifficultyViewTypeToDisplay =>
			!IsInGameLobby
			&& !IsInQueue
			&& IsGroupSubordinate
				? BotDifficultyViewType.Advanced
				: BotDifficultyView != null
					? BotDifficultyView.Value
					: BotDifficultyViewType.Simple;

		public bool BotSkillPanelVisible => GameTypeToDisplay == GameType.Solo || GameTypeToDisplay == GameType.Coop;

		public GameType GameTypeToDisplay
		{
			get
			{
				GameType gameType;
				if (ClientRequestedGameType != null)
				{
					gameType = ClientRequestedGameType.Value;
				}
				else
				{
					var gameManager = GameManager.Get();
					LobbyGameConfig lobbyGameConfig = gameManager != null ? gameManager.GameConfig : null;
					if (IsInGameLobby && lobbyGameConfig != null)
					{
						gameType = lobbyGameConfig.GameType;
					}
					else
					{
						if (ClientGameManager.Get().GroupInfo == null)
						{
							return GameType.None;
						}
						gameType = ClientGameManager.Get().GroupInfo.SelectedQueueType;
					}
				}
				ClientGameManager clientGameManager = ClientGameManager.Get();
				GameType blockedExperienceAlternativeGameType = ClientGameManager.Get().GameTypeAvailabilies[gameType].BlockedExperienceAlternativeGameType;
				List<MatchmakingQueueConfig.QueueEntryExperience> blockedExperienceEntries = clientGameManager.GameTypeAvailabilies[gameType].BlockedExperienceEntries;
				if (blockedExperienceAlternativeGameType != GameType.None
				    && clientGameManager.GetPlayerAccountData().ExperienceComponent.Matches < clientGameManager.NewPlayerPvPQueueDuration
				    && blockedExperienceEntries != null
				    && blockedExperienceEntries.Contains(MatchmakingQueueConfig.QueueEntryExperience.NewPlayer))
				{
					return blockedExperienceAlternativeGameType;
				}
				return gameType;
			}
		}

		public CharacterResourceLink CharacterResourceLinkOfCharacterTypeToDisplay =>
			CharacterTypeToDisplay.IsValidForHumanPreGameSelection()
				? GameWideData.Get().GetCharacterResourceLink(CharacterTypeToDisplay)
				: null;

		public CharacterType CharacterTypeToDisplay =>
			AppState.GetCurrent() == AppState_CharacterSelect.Get()
			&& SelectedCharacterFromGameInfo.IsValidForHumanPreGameSelection()
				? SelectedCharacterFromGameInfo
				: ClientRequestToServerSelectCharacter != null
					? ClientRequestToServerSelectCharacter.Value
					: SelectedCharacterInGroup.IsValidForHumanPreGameSelection()
						? SelectedCharacterInGroup
						: ClientSelectedCharacter != null
							? ClientSelectedCharacter.Value
							: SelectedCharacterFromPlayerData;

		public CharacterVisualInfo CharacterVisualInfoToDisplay =>
			ClientSelectedVisualInfo != null
				? ClientSelectedVisualInfo.Value
				: ClientGameManager.Get() != null
				  && ClientGameManager.Get().GroupInfo != null
				  && ClientGameManager.Get().GroupInfo.ChararacterInfo != null
					? ClientGameManager.Get().GroupInfo.ChararacterInfo.CharacterSkin
					: default(CharacterVisualInfo);

		public enum BotDifficultyViewType
		{
			Simple,
			Advanced
		}

		public enum SimpleBotSettingValue
		{
			Easy,
			Medium,
			Hard
		}
	}

	public enum RefreshFunctionType
	{
		RefreshSideButtonVisibility = 0x1,
		RefreshSideButtonClickability = 0x2,
		RefreshSelectedCharacterButton = 0x4,
		SendRequestToServerForCharacterSelect = 0x8,
		RefreshCharacterButtonVisibility = 0x10,
		RefreshSelectedGameType = 0x20,
		RefreshCharacterButtons = 0x40,
		RefreshBotSkillPanel = 0x80,
		RefreshGameSubTypes = 0x100,
		RefreshPartyList = 0x200
	}

	public class GameSubTypeState
	{
		public _ToggleSwap btn;
		public ushort SubTypeBit;
	}
}
