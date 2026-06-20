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
					if (characterResourceLink.m_characterRole == CharacterRole.Assassin)
					{
						assassins.Add(characterType);
					}

					if (characterResourceLink.m_characterRole == CharacterRole.Support)
					{
						supports.Add(characterType);
					}

					if (characterResourceLink.m_characterRole == CharacterRole.Tank)
					{
						tanks.Add(characterType);
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
				using (Dictionary<ushort, GameSubType>.Enumerator enumerator2 = gameTypeSubTypes2.GetEnumerator())
				{
					if (enumerator2.MoveNext())
					{
						KeyValuePair<ushort, GameSubType> keyValuePair2 = enumerator2.Current;
						newMask = keyValuePair2.Key;
					}
				}
			}
		}
		ushort exclusiveModBitsOfGameTypeToDisplay = Parameters.ExclusiveModBitsOfGameTypeToDisplay;
		ushort num = 0;
		if (exclusiveModBitsOfGameTypeToDisplay != 0)
		{
			for (int j = 0; j < m_gameSubTypeBtns.Count; j++)
			{
				if (m_gameSubTypeBtns[j].btn.IsChecked())
				{
					if ((exclusiveModBitsOfGameTypeToDisplay | m_gameSubTypeBtns[j].SubTypeBit) != 0)
					{
						num = m_gameSubTypeBtns[j].SubTypeBit;
						break;
					}
				}
			}
			if (num != 0)
			{
				newMask = num;
			}
		}
		if (num == 0)
		{
			for (int k = 0; k < m_gameSubTypeBtns.Count; k++)
			{
				if (m_gameSubTypeBtns[k].btn.IsChecked())
				{
					newMask = (ushort)(m_gameSubTypeBtns[k].SubTypeBit | newMask);
				}
			}
		}
		ushort num2 = 0;
		for (int l = 0; l < m_gameSubTypeBtns.Count; l++)
		{
			num2 = (ushort)(m_gameSubTypeBtns[l].SubTypeBit | num2);
		}
		if (num2 != 0)
		{
			if ((newMask & num2) == 0)
			{
				m_gameSubTypeBtns[0].btn.SetOn(true);
				newMask = m_gameSubTypeBtns[0].SubTypeBit;
			}
		}
		Parameters.SelectedSubTypeMask = newMask;
		UpdateSubTypeMaskChecks(newMask);
		if (!sendMaskUpdate)
		{
			if (SentInitialSubTypes)
			{
				goto IL_4EE;
			}
		}
		SentInitialSubTypes = true;
		if (ClientGameManager.Get().GroupInfo.InAGroup)
		{
			if (ClientGameManager.Get().GroupInfo.IsLeader)
			{
				ClientGameManager.Get().SetGameTypeSubMasks(Parameters.GameTypeToDisplay, newMask, delegate(SetGameSubTypeResponse r)
				{
					if (!r.Success)
					{
						string format = "Failed to select game modes: {0}";
						object arg;
						if (r.LocalizedFailure == null)
						{
							arg = r.ErrorMessage;
						}
						else
						{
							arg = r.LocalizedFailure.ToString();
						}
						string text = string.Format(format, arg);
						Log.Warning(text);
						UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("Error", "Global"), text, StringUtil.TR("Ok", "Global"));
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
					string text = string.Format("Failed to select game modes: {0}", (r.LocalizedFailure != null) ? r.LocalizedFailure.ToString() : r.ErrorMessage);
					Log.Warning(text);
					UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("Error", "Global"), text, StringUtil.TR("Ok", "Global"));
				}
				else
				{
					ClientGameManager.Get().SetSoloSubGameMask(Parameters.GameTypeToDisplay, newMask);
					UpdateSubTypeMaskChecks(newMask);
				}
			});
		}
		IL_4EE:
		UpdateWillFillVisibility();
		DoRefreshFunctions((ushort)RefreshFunctionType.RefreshPartyList);
	}

	public void EditedSearchInput(string input)
	{
		UpdateCharacterButtonHighlights();
	}

	public void ClickedOnFactionFilter(UICharacterSelectFactionFilter btn)
	{
		if (m_lastFilterBtnClicked != null)
		{
			if (m_lastFilterBtnClicked != btn)
			{
				m_lastFilterBtnClicked.m_btn.SetSelected(false, false, string.Empty, string.Empty);
			}
		}
		m_lastFilterBtnClicked = btn;
		UpdateCharacterButtonHighlights();
	}

	private void UpdateCharacterButtonHighlights()
	{
		for (int i = 0; i < CharacterSelectButtons.Count; i++)
		{
			if (CharacterSelectButtons[i] != null)
			{
				if (CharacterSelectButtons[i].GetComponent<CanvasGroup>() != null)
				{
					CanvasGroup component = CharacterSelectButtons[i].GetComponent<CanvasGroup>();
					if (component != null)
					{
						component.alpha = 1f;
					}
				}
			}
		}
		if (m_lastFilterBtnClicked != null)
		{
			if (m_lastFilterBtnClicked.m_btn.IsSelected())
			{
				for (int j = 0; j < CharacterSelectButtons.Count; j++)
				{
					if (!m_lastFilterBtnClicked.IsAvailable(CharacterSelectButtons[j].m_characterType))
					{
						CanvasGroup component2 = CharacterSelectButtons[j].GetComponent<CanvasGroup>();
						if (component2 != null)
						{
							component2.alpha = 0.3f;
						}
					}
				}
			}
		}
		if (!m_searchInputField.text.IsNullOrEmpty())
		{
			for (int k = 0; k < CharacterSelectButtons.Count; k++)
			{
				CharacterResourceLink characterResourceLink = CharacterSelectButtons[k].GetCharacterResourceLink();
				if (characterResourceLink != null)
				{
					string displayName = characterResourceLink.GetDisplayName();
					if (!DoesSearchMatchDisplayName(m_searchInputField.text.ToLower(), displayName.ToLower()))
					{
						CanvasGroup component3 = CharacterSelectButtons[k].GetComponent<CanvasGroup>();
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
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					return true;
				}
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
		for (int i = 0; i < SkinSubButtons.Length; i++)
		{
			if (SkinSubButtons[i] != m_skinsBtn.spriteController)
			{
				m_skinsBtn.spriteController.AddSubButton(SkinSubButtons[i]);
			}
		}
		for (int j = 0; j < AbilitySubButtons.Length; j++)
		{
			if (AbilitySubButtons[j] != m_AbilitiesBtn.spriteController)
			{
				m_AbilitiesBtn.spriteController.AddSubButton(AbilitySubButtons[j]);
			}
		}
		for (int k = 0; k < CatalystSubButtons.Length; k++)
		{
			if (CatalystSubButtons[k] != m_CatalystBtn.spriteController)
			{
				m_CatalystBtn.spriteController.AddSubButton(CatalystSubButtons[k]);
			}
		}
		m_skinsBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, tooltip => SideMenuOpen(tooltip, m_skinsBtn));
		m_AbilitiesBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, tooltip => SideMenuOpen(tooltip, m_AbilitiesBtn));
		m_CatalystBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, tooltip => SideMenuOpen(tooltip, m_CatalystBtn));
		for (int l = 0; l < m_AbilityMouseOverBtns.Length; l++)
		{
			m_AbilityMouseOverBtns[l].spriteController.callback = ClickedAbilityIcon;
			int index = l;
			m_AbilityMouseOverBtns[l].spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, tooltip => SetupAbilitySideBtnTooltip(tooltip, index));
		}
		for (int m = 0; m < m_AbilityModIcons.Length; m++)
		{
			UIManager.SetGameObjectActive(m_AbilityModIcons[m], false);
		}
		for (int n = 0; n < m_CatalsytBtns.Length; n++)
		{
			m_CatalsytBtns[n].spriteController.callback = ClickedCatalystIcon;
			AbilityRunPhase phase = n + AbilityRunPhase.Prep;
			m_CatalsytBtns[n].spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, delegate(UITooltipBase tooltip)
			{
				if (SelectedCatalysts.ContainsKey(phase))
				{
					return SetupCatalystSideBtnTooltip(tooltip, SelectedCatalysts[phase]);
				}
				return false;
			});
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
		UICharacterSelectSkinPanel uicharacterSelectSkinPanel = UICharacterSelectCharacterSettingsPanel.Get().m_skinsSubPanel.m_selectHandler as UICharacterSelectSkinPanel;
		if (uicharacterSelectSkinPanel != null)
		{
			GameWideData gameWideData = GameWideData.Get();
			CharacterType? clientSelectedCharacter = GetCurrentSpecificState().ClientSelectedCharacter;
			CharacterResourceLink characterResourceLink = gameWideData.GetCharacterResourceLink(clientSelectedCharacter.Value);
			CharacterVisualInfo? clientSelectedVisualInfo = GetCurrentSpecificState().ClientSelectedVisualInfo;
			CharacterVisualInfo value = clientSelectedVisualInfo.Value;
			if (!(characterResourceLink == null))
			{
				if (characterResourceLink.m_skins.Count > value.skinIndex && characterResourceLink.m_skins[value.skinIndex].m_patterns.Count > value.patternIndex)
				{
					if (characterResourceLink.m_skins[value.skinIndex].m_patterns[value.patternIndex].m_colors.Count > value.colorIndex)
					{
						string patternColorName = characterResourceLink.GetPatternColorName(value.skinIndex, value.patternIndex, value.colorIndex);
						UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
						uititledTooltip.Setup(characterResourceLink.GetDisplayName(), string.Format(StringUtil.TR("SelectedStyle", "Global"), patternColorName), string.Empty);
						return true;
					}
				}
			}
			return false;
		}
		return false;
	}

	private bool SetupCatalystSideBtnTooltip(UITooltipBase tooltip, Card card)
	{
		if (!m_CatalystBtn.IsHover)
		{
			return false;
		}
		string text = card.GetDisplayName();
		if (!card.m_useAbility.m_flavorText.IsNullOrEmpty())
		{
			string text2 = text;
			text = string.Concat(text2, Environment.NewLine, "<i>", card.m_useAbility.m_flavorText, "</i>");
		}
		UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
		uititledTooltip.Setup(string.Format(StringUtil.TR("CatalystTitle", "Global"), card.m_useAbility.GetPhaseString()), text, string.Empty);
		return true;
	}

	private bool SetupAbilitySideBtnTooltip(UITooltipBase tooltip, int i)
	{
		if (!m_AbilitiesBtn.IsHover)
		{
			return false;
		}
		AbilityData.AbilityEntry abilityEntry = SelectedAbilityData[i].GetAbilityEntry();
		if (abilityEntry != null)
		{
			if (!(abilityEntry.ability == null))
			{
				UIAbilityTooltip uiabilityTooltip = (UIAbilityTooltip)tooltip;
				string movieAssetName = "Video/AbilityPreviews/" + abilityEntry.ability.m_previewVideo;
				uiabilityTooltip.Setup(abilityEntry.ability, SelectedAbilityData[i].GetSelectedMod(), movieAssetName);
				return true;
			}
		}
		return false;
	}

	public void UpdateCatalystIcons(Dictionary<AbilityRunPhase, Card> phaseToCards)
	{
		SelectedCatalysts = phaseToCards;
		using (Dictionary<AbilityRunPhase, Card>.Enumerator enumerator = SelectedCatalysts.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<AbilityRunPhase, Card> keyValuePair = enumerator.Current;
				Card value = keyValuePair.Value;
				if (-1 < value.GetAbilityRunPhase() - AbilityRunPhase.Prep)
				{
					if (value.GetAbilityRunPhase() - AbilityRunPhase.Prep < m_CatalystIcons.Length)
					{
						UIManager.SetGameObjectActive(m_CatalystIcons[value.GetAbilityRunPhase() - AbilityRunPhase.Prep], true);
						m_CatalystIcons[value.GetAbilityRunPhase() - AbilityRunPhase.Prep].sprite = value.GetIconSprite();
					}
				}
				if (-1 < value.GetAbilityRunPhase() - AbilityRunPhase.Prep)
				{
					if (value.GetAbilityRunPhase() - AbilityRunPhase.Prep < m_CatalystHoverIcons.Length)
					{
						UIManager.SetGameObjectActive(m_CatalystHoverIcons[value.GetAbilityRunPhase() - AbilityRunPhase.Prep], true);
						m_CatalystHoverIcons[value.GetAbilityRunPhase() - AbilityRunPhase.Prep].sprite = value.GetIconSprite();
					}
				}
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
				else if (m_currentState != null)
				{
					if (m_currentState.CharacterTypeToDisplay != CharacterType.PendingWillFill)
					{
						Log.Warning("Ability entry has no ability!");
					}
				}
			}
			else
			{
				Log.Warning("AbilityButton has no Ability Entry!");
			}
			Color color = Color.gray;
			if (uiphase == UIQueueListPanel.UIPhase.Prep)
			{
				color = prepColor;
			}
			else if (uiphase == UIQueueListPanel.UIPhase.Evasion)
			{
				color = dashColor;
			}
			else if (uiphase == UIQueueListPanel.UIPhase.Combat)
			{
				color = combatColor;
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
		int num = -1;
		for (int i = 0; i < m_AbilityMouseOverBtns.Length; i++)
		{
			if (m_AbilityMouseOverBtns[i].spriteController.gameObject == (data as PointerEventData).selectedObject)
			{
				num = i;
				break;
			}
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		UICharacterSelectCharacterSettingsPanel.Get().SetVisible(true, UICharacterSelectCharacterSettingsPanel.TabPanel.Abilities);
		if (num > -1)
		{
			UICharacterSelectCharacterSettingsPanel.Get().m_abilitiesSubPanel.AbilityButtonSelected(num);
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
			UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
			uititledTooltip.Setup(
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
		for (int i = 0; i < SkinSubButtons.Length; i++)
		{
			if (SkinSubButtons[i] != m_skinsBtn.spriteController)
			{
				SkinSubButtons[i].SetClickable(true);
			}
		}
	}

	public void SkinMouseExit(BaseEventData data)
	{
		for (int i = 0; i < SkinSubButtons.Length; i++)
		{
			if (SkinSubButtons[i] != m_skinsBtn.spriteController)
			{
				SkinSubButtons[i].SetClickable(false);
			}
		}
	}

	public void AbilityMouseOver(BaseEventData data)
	{
		for (int i = 0; i < AbilitySubButtons.Length; i++)
		{
			if (AbilitySubButtons[i] != m_AbilitiesBtn.spriteController)
			{
				AbilitySubButtons[i].SetClickable(true);
			}
		}
	}

	public void AbilityMouseExit(BaseEventData data)
	{
		for (int i = 0; i < AbilitySubButtons.Length; i++)
		{
			if (AbilitySubButtons[i] != m_AbilitiesBtn.spriteController)
			{
				AbilitySubButtons[i].SetClickable(false);
			}
		}
	}

	public void CatalystMouseOver(BaseEventData data)
	{
		for (int i = 0; i < CatalystSubButtons.Length; i++)
		{
			if (CatalystSubButtons[i] != m_CatalystBtn.spriteController)
			{
				CatalystSubButtons[i].SetClickable(true);
			}
		}
	}

	public void CatalystMouseExit(BaseEventData data)
	{
		for (int i = 0; i < CatalystSubButtons.Length; i++)
		{
			if (CatalystSubButtons[i] != m_CatalystBtn.spriteController)
			{
				CatalystSubButtons[i].SetClickable(false);
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
		if (response.Success)
		{
			if (response.CharacterInfo != null)
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
	}

	private bool IsCharacterValidForSelection(CharacterType characterType)
	{
		bool flag = false;
		if (characterType == CharacterType.None)
		{
			return flag;
		}
		GameType gameTypeToDisplay = GetCurrentSpecificState().GameTypeToDisplay;
		if (GameManager.Get() != null)
		{
			if (GameManager.Get().IsValidForHumanPreGameSelection(characterType))
			{
				GameType gameType;
				if (GameManager.Get().GameConfig != null && GameManager.Get().GameStatus != GameStatus.Stopped)
				{
					gameType = GameManager.Get().GameConfig.GameType;
				}
				else
				{
					gameType = ClientGameManager.Get().GroupInfo.SelectedQueueType;
				}
				GameType gameType2 = gameType;
				flag = GameManager.Get().IsCharacterAllowedForGameType(characterType, gameType2, null, null);
			}
		}
		if (flag)
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(characterType);
			bool flag2 = playerCharacterData != null && playerCharacterData.CharacterComponent != null && playerCharacterData.CharacterComponent.Unlocked;
			bool flag3;
			if (SceneStateParameters.IsInGameLobby)
			{
				flag3 = GameManager.Get().IsCharacterAllowedForPlayers(characterType);
			}
			else
			{
				flag3 = GameManager.Get().IsValidForHumanPreGameSelection(characterType);
			}
			bool flag4;
			if (flag3)
			{
				flag4 = GameManager.Get().IsCharacterAllowedForGameType(characterType, gameTypeToDisplay, null, null);
			}
			else
			{
				flag4 = false;
			}
			bool flag5 = flag4;
			bool flag6 = ClientGameManager.Get().IsCharacterAvailable(characterType, gameTypeToDisplay);
			if (flag5)
			{
				if (flag2 || flag6)
				{
					goto IL_17F;
				}
			}
			flag = false;
		}
		IL_17F:
		if (flag && SceneStateParameters.IsInGameLobby)
		{
			if (GameManager.Get().TeamInfo != null)
			{
				if (gameTypeToDisplay != GameType.Custom)
				{
					LobbyPlayerInfo playerInfo = GameManager.Get().PlayerInfo;
					Team team = playerInfo.TeamId;
					if (team == Team.Spectator)
					{
						team = Team.TeamA;
					}
					List<LobbyPlayerInfo> list = (from ti in GameManager.Get().TeamInfo.TeamInfo(team)
					orderby (ti.PlayerId != playerInfo.PlayerId) ? 1 : 0
					select ti).ToList();
					for (int i = 0; i < list.Count; i++)
					{
						if (list[i].PlayerId != playerInfo.PlayerId)
						{
							if (list[i].CharacterType == characterType)
							{
								if (GameManager.Get().IsFreelancerConflictPossible(list[i].TeamId == playerInfo.TeamId))
								{
									if (!list[i].IsNPCBot)
									{
										return false;
									}
								}
							}
						}
					}
				}
			}
		}
		return flag;
	}

	public void ReceivedGameTypeChangeResponse()
	{
		GetCurrentSpecificState().ClientRequestedGameType = null;
		DoRefreshFunctions((ushort)RefreshFunctionType.RefreshSelectedGameType);
	}

	private void SetDropdownText(string text)
	{
		TextMeshProUGUI[] componentsInChildren = m_dropdownBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].text = text;
		}
	}

	public void RefreshPartyList()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		UIManager.SetGameObjectActive(m_partyListPanel, currentSpecificState.PartyListVisbility);
		m_partyListPanel.SetVisible(currentSpecificState.PartyListVisbility);
		if ((!currentSpecificState.CustomGamePartyIsVisible || currentSpecificState.CustomGamePartyIsHidden) && currentSpecificState.PartyListVisbility)
		{
			if (currentSpecificState.GameTypeToDisplay != GameType.Custom)
			{
				bool isDuplicateCharsAllowed = false;
				int num = -1;
				using (Dictionary<ushort, GameSubType>.ValueCollection.Enumerator enumerator = currentSpecificState.SelectedGameSubTypes.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameSubType gameSubType = enumerator.Current;
						if (gameSubType.HasMod(GameSubType.SubTypeMods.ControlAllBots))
						{
							if (gameSubType.TeamAPlayers > num)
							{
								num = gameSubType.TeamAPlayers;
							}
						}
					}
				}
				if (num < 0)
				{
					GameType selectedQueueType = ClientGameManager.Get().GroupInfo.SelectedQueueType;
					num = ClientGameManager.Get().GameTypeAvailabilies[selectedQueueType].TeamAPlayers;
				}
				m_partyListPanel.SetupForOutOfGame(num, isDuplicateCharsAllowed);
			}
		}
	}

	public void RefreshGameSubTypes()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		ushort num = 0;
		bool sendMaskUpdate = false;
		if (currentSpecificState.ClientRequestAllyBotTeammates != null)
		{
			for (int i = 0; i < m_gameSubTypeBtns.Count; i++)
			{
				if (m_gameSubTypeBtns[i].btn.IsChecked())
				{
					num |= m_gameSubTypeBtns[i].SubTypeBit;
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
			if (!validGameSubTypes.IsNullOrEmpty())
			{
				if (validGameSubTypes.Count > 1)
				{
					ushort num2 = 0;
					if (ClientGameManager.Get().GroupInfo.InAGroup && !ClientGameManager.Get().GroupInfo.IsLeader)
					{
						num2 = ClientGameManager.Get().GroupInfo.SubTypeMask;
					}
					else
					{
						num2 = ClientGameManager.Get().GetSoloSubGameMask(gameTypeToDisplay);
					}
					ushort num3 = 0;
					using (Dictionary<ushort, GameSubType>.Enumerator enumerator = validGameSubTypes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<ushort, GameSubType> keyValuePair = enumerator.Current;
							bool flag = IsGameSubTypeActive(gameTypeToDisplay, keyValuePair.Value);
							if (flag && keyValuePair.Value.HasMod(GameSubType.SubTypeMods.NotAllowedForGroups))
							{
								flag = !inAGroup;
							}
							if (!flag || !keyValuePair.Value.HasMod(GameSubType.SubTypeMods.CanBeConsolidated))
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
								UIManager.SetGameObjectActive(m_gameTypeButtons[j], flag);
								TextMeshProUGUI componentInChildren = m_gameTypeButtons[j].GetComponentInChildren<TextMeshProUGUI>(true);
								componentInChildren.text = StringUtil.TR(keyValuePair.Value.LocalizedName);
								m_gameTypeButtons[j].changedNotify = SubTypeClicked;
								if (!flag)
								{
									goto IL_392;
								}
								if ((num2 & keyValuePair.Key) == 0)
								{
									goto IL_392;
								}
								m_gameTypeButtons[j].SetOn(true);
								IL_3A8:
								j++;
								continue;
								IL_392:
								m_gameTypeButtons[j].SetOn(false);
								goto IL_3A8;
							}
							num3 |= keyValuePair.Key;
						}
					}
					if (num3 != 0)
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
							SubTypeBit = num3
						});
						UIManager.SetGameObjectActive(m_gameTypeButtons[j], true);
						if ((num2 & num3) != 0)
						{
							m_gameTypeButtons[j].SetOn(true);
						}
						else
						{
							m_gameTypeButtons[j].SetOn(false);
						}
						TextMeshProUGUI componentInChildren2 = m_gameTypeButtons[j].GetComponentInChildren<TextMeshProUGUI>(true);
						componentInChildren2.text = StringUtil.TR("ConsolidatedGameSubTypes", "SubTypes");
						m_gameTypeButtons[j].changedNotify = SubTypeClicked;
						j++;
					}
				}
			}
			while (j < m_gameTypeButtons.Count)
			{
				UIManager.SetGameObjectActive(m_gameTypeButtons[j], false);
				j++;
			}
		}
		UIManager.SetGameObjectActive(m_GameTypeContainer, currentSpecificState.GameSubTypesVisible);
		CheckSubTypeSelection(sendMaskUpdate, num);
	}

	private void SendBotDifficultyUpdateToServer(BotDifficulty? AllyDifficulty, BotDifficulty? EnemyDifficulty)
	{
		if (GameManager.Get().TeamInfo != null)
		{
			if (!GameManager.Get().TeamInfo.TeamBPlayerInfo.IsNullOrEmpty())
			{
				goto IL_95;
			}
		}
		if (GameManager.Get().QueueInfo != null)
		{
			if (EnemyDifficulty != null)
			{
				ClientGameManager.Get().LobbyInterface.UpdateQueueEnemyBotDifficulty(GameManager.Get().QueueInfo, EnemyDifficulty.Value);
			}
			return;
		}
		IL_95:
		if (ClientGameManager.Get().GroupInfo != null && ClientGameManager.Get().GroupInfo.InAGroup)
		{
			if (ClientGameManager.Get().GroupInfo.IsLeader)
			{
				ClientGameManager.Get().UpdateBotDifficulty(AllyDifficulty, EnemyDifficulty);
				return;
			}
		}
		if (GameManager.Get().TeamInfo != null)
		{
			using (IEnumerator<LobbyPlayerInfo> enumerator = GameManager.Get().TeamInfo.TeamBPlayerInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
					ClientGameManager.Get().UpdateBotDifficulty(AllyDifficulty, EnemyDifficulty, lobbyPlayerInfo.PlayerId);
				}
			}
		}
	}

	public void RefreshBotSkillPanel()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		GameType gameTypeToDisplay = currentSpecificState.GameTypeToDisplay;
		bool flag = !SceneStateParameters.IsInGameLobby && !SceneStateParameters.IsInQueue && !SceneStateParameters.IsGroupSubordinate;
		m_teamBotStars.SetClickable(flag);
		m_enemyBotStars.SetClickable(flag);
		m_teamBotsToggle.SetClickable(flag);
		for (int i = 0; i < m_gameTypeButtons.Count; i++)
		{
			m_gameTypeButtons[i].SetClickable(flag);
		}
		m_simpleCogBtn.spriteController.SetClickable(flag);
		m_advancedCogBtn.spriteController.SetClickable(flag);
		m_dropdownBtn.spriteController.SetClickable(flag);
		if (!flag)
		{
			UIManager.SetGameObjectActive(m_DifficultyListDropdown, false);
		}
		bool displayAllyBotTeammates = currentSpecificState.DisplayAllyBotTeammates;
		m_teamBotsToggle.SetOn(displayAllyBotTeammates);
		if (gameTypeToDisplay != GameType.Solo)
		{
			if (gameTypeToDisplay != GameType.Coop)
			{
				goto IL_217;
			}
		}
		string text;
		if (currentSpecificState.DisplayAllyBotTeammates)
		{
			text = "SoloEnemyDifficulty";
		}
		else
		{
			text = "CoopDifficulty";
		}
		string key = text;
		int enemyBotDifficultyToDisplay = currentSpecificState.EnemyBotDifficultyToDisplay;
		m_enemyBotStars.SetCurrentValue(enemyBotDifficultyToDisplay + 1);
		int? clientRequestedEnemyBotDifficulty = currentSpecificState.ClientRequestedEnemyBotDifficulty;
		if (clientRequestedEnemyBotDifficulty != null)
		{
			PlayerPrefs.SetInt(key, enemyBotDifficultyToDisplay);
			SendBotDifficultyUpdateToServer(null, (BotDifficulty)enemyBotDifficultyToDisplay);
			currentSpecificState.SelectedEnemyBotDifficulty = currentSpecificState.ClientRequestedEnemyBotDifficulty;
			currentSpecificState.ClientRequestedEnemyBotDifficulty = null;
		}
		int allyBotDifficultyToDisplay = currentSpecificState.AllyBotDifficultyToDisplay;
		m_teamBotStars.SetCurrentValue(allyBotDifficultyToDisplay + 1);
		int? clientRequestedAllyBotDifficulty = currentSpecificState.ClientRequestedAllyBotDifficulty;
		if (clientRequestedAllyBotDifficulty != null)
		{
			PlayerPrefs.SetInt("SoloAllyDifficulty", allyBotDifficultyToDisplay);
			SendBotDifficultyUpdateToServer((BotDifficulty)allyBotDifficultyToDisplay, null);
			currentSpecificState.SelectedAllyBotDifficulty = currentSpecificState.ClientRequestedAllyBotDifficulty;
			currentSpecificState.ClientRequestedAllyBotDifficulty = null;
		}
		IL_217:
		UIManager.SetGameObjectActive(m_botSkillPanel, currentSpecificState.BotSkillPanelVisible);
		UIManager.SetGameObjectActive(m_DifficultyListDropdown, false);
		bool flag2 = currentSpecificState.BotDifficultyViewTypeToDisplay == CharacterSelectSceneStateParameters.BotDifficultyViewType.Simple;
		if (flag2)
		{
			UIManager.SetGameObjectActive(m_simpleCogBtn, false);
			UIManager.SetGameObjectActive(m_advancedCogBtn, true);
			UIManager.SetGameObjectActive(m_dropdownBtn, true);
			UIManager.SetGameObjectActive(m_difficultyListContainer, true);
			CharacterSelectSceneStateParameters.SimpleBotSettingValue simpleBotSettingValueToDisplay = currentSpecificState.SimpleBotSettingValueToDisplay;
			m_easyBtn.SetSelected(simpleBotSettingValueToDisplay == CharacterSelectSceneStateParameters.SimpleBotSettingValue.Easy, false, string.Empty, string.Empty);
			m_mediumBtn.SetSelected(simpleBotSettingValueToDisplay == CharacterSelectSceneStateParameters.SimpleBotSettingValue.Medium, false, string.Empty, string.Empty);
			m_hardBtn.SetSelected(simpleBotSettingValueToDisplay == CharacterSelectSceneStateParameters.SimpleBotSettingValue.Hard, false, string.Empty, string.Empty);
			if (simpleBotSettingValueToDisplay != CharacterSelectSceneStateParameters.SimpleBotSettingValue.Easy)
			{
				if (simpleBotSettingValueToDisplay != CharacterSelectSceneStateParameters.SimpleBotSettingValue.Medium)
				{
					if (simpleBotSettingValueToDisplay != CharacterSelectSceneStateParameters.SimpleBotSettingValue.Hard)
					{
					}
					else
					{
						SetDropdownText(StringUtil.TR("Hard", "Global"));
						m_enemyBotStars.SetCurrentValue(4);
						m_teamBotStars.SetCurrentValue(2);
						if (currentSpecificState.ClientRequestedSimpleBotSettingValue != null)
						{
							SendBotDifficultyUpdateToServer(BotDifficulty.Easy, BotDifficulty.Hard);
						}
					}
				}
				else
				{
					SetDropdownText(StringUtil.TR("Medium", "Global"));
					m_enemyBotStars.SetCurrentValue(2);
					m_teamBotStars.SetCurrentValue(3);
					if (currentSpecificState.ClientRequestedSimpleBotSettingValue != null)
					{
						SendBotDifficultyUpdateToServer(BotDifficulty.Medium, BotDifficulty.Easy);
					}
				}
			}
			else
			{
				SetDropdownText(StringUtil.TR("Easy", "Global"));
				m_enemyBotStars.SetCurrentValue(1);
				m_teamBotStars.SetCurrentValue(4);
				if (currentSpecificState.ClientRequestedSimpleBotSettingValue != null)
				{
					SendBotDifficultyUpdateToServer(BotDifficulty.Hard, BotDifficulty.Stupid);
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
		UIManager.SetGameObjectActive(m_enemyBotSkillPanel, currentSpecificState.BotSkillPanelVisible && !flag2);
		GameObject teamBotSkillPanel = m_teamBotSkillPanel;
		bool doActive;
		if (currentSpecificState.BotSkillPanelVisible)
		{
			doActive = !flag2;
		}
		else
		{
			doActive = false;
		}
		UIManager.SetGameObjectActive(teamBotSkillPanel, doActive);
		UIManager.SetGameObjectActive(m_teamBotsToggle, true);
	}

	public void RefreshSelectedGameType()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		if (currentSpecificState.ClientRequestedGameType != null)
		{
			GameType? clientRequestedGameType = currentSpecificState.ClientRequestedGameType;
			GameType value = clientRequestedGameType.Value;
			if (ClientGameManager.Get().GroupInfo.InAGroup)
			{
				if (ClientGameManager.Get().GroupInfo.SelectedQueueType != value)
				{
					ClientGameManager.Get().UpdateSelectedGameMode(value);
				}
			}
			else
			{
				GetCurrentSpecificState().ClientRequestedGameType = null;
				SentInitialSubTypes = false;
				ClientGameManager.Get().GroupInfo.SelectedQueueType = value;
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
			GameTypeAvailability gameTypeAvailability;
			int num;
			if (ClientGameManager.Get().GameTypeAvailabilies.TryGetValue(currentSpecificState.GameTypeToDisplay, out gameTypeAvailability))
			{
				num = gameTypeAvailability.MaxWillFillPerTeam;
			}
			else
			{
				num = 0;
			}
			int num2 = num;
			using (Dictionary<ushort, GameSubType>.ValueCollection.Enumerator enumerator = currentSpecificState.SelectedGameSubTypes.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					GameSubType gameSubType = enumerator.Current;
					if (gameSubType.HasMod(GameSubType.SubTypeMods.ControlAllBots))
					{
						num2 = 0;
					}
				}
			}
			UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_miscCharSelectButtons, num2 > 0);
			if (num2 == 0)
			{
				if (GetCurrentSpecificState().CharacterTypeToDisplay.IsWillFill())
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
			}
			else
			{
				CharacterType characterType = characterResourceLink.m_characterType;
				bool flag = true;
				if (!IsCharacterValidForSelection(characterType))
				{
					flag = false;
				}
				else if (characterType == GetCurrentSpecificState().CharacterTypeToDisplay)
				{
					UICharacterSelectScreenController.Get().UpdateBuyButtons();
				}
				PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(characterType);
				bool practiceGameTypeSelectedForQueue = SceneStateParameters.PracticeGameTypeSelectedForQueue;
				bool flag2;
				if (GameManager.Get().IsCharacterAllowedForPlayers(characterType))
				{
					flag2 = GameManager.Get().IsCharacterAllowedForGameType(characterType, GetCurrentSpecificState().GameTypeToDisplay, null, null);
				}
				else
				{
					flag2 = false;
				}
				bool flag3 = flag2;
				UICharacterPanelSelectButton uicharacterPanelSelectButton2 = uicharacterPanelSelectButton;
				bool enabled;
				if (!flag)
				{
					if (practiceGameTypeSelectedForQueue)
					{
						enabled = flag3;
					}
					else
					{
						enabled = false;
					}
				}
				else
				{
					enabled = true;
				}
				uicharacterPanelSelectButton2.SetEnabled(enabled, playerCharacterData);
				uicharacterPanelSelectButton.UpdateFreeRotationIcon();
			}
		}
	}

	public void RefreshSelectedCharacterButton()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		if (currentSpecificState.ClientRequestToServerSelectCharacter != null)
		{
			CharacterType? clientRequestToServerSelectCharacter = currentSpecificState.ClientRequestToServerSelectCharacter;
			CharacterType value = clientRequestToServerSelectCharacter.Value;
			if (!IsCharacterValidForSelection(value))
			{
				if (SceneStateParameters.IsInGameLobby)
				{
					GetCurrentSpecificState().ClientRequestToServerSelectCharacter = null;
					DoCharButtonSelection(currentSpecificState.CharacterTypeToDisplay);
					goto IL_85;
				}
			}
			DoCharButtonSelection(value);
			IL_85:;
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
			CharacterType? clientRequestToServerSelectCharacter = currentSpecificState.ClientRequestToServerSelectCharacter;
			characterType = clientRequestToServerSelectCharacter.Value;
		}
		if (characterType != CharacterType.None)
		{
			if (!IsCharacterValidForSelection(characterType))
			{
				if (SceneStateParameters.IsInGameLobby)
				{
					goto IL_136;
				}
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
				if (UILandingPageScreen.Get() != null && UILandingPageScreen.Get().CharacterInfoClicked != null)
				{
					if (UILandingPageScreen.Get().CharacterInfoClicked.Value == characterType)
					{
						ClientGameManager.Get().UpdateSelectedCharacter(characterType);
						goto IL_134;
					}
				}
				GetCurrentSpecificState().ClientRequestToServerSelectCharacter = null;
			}
			IL_134:
			return;
		}
		IL_136:
		GetCurrentSpecificState().ClientRequestToServerSelectCharacter = null;
	}

	public void RefreshSideButtonsVisibility()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		bool sideButtonsVisibility = currentSpecificState.SideButtonsVisibility;
		UIManager.SetGameObjectActive(m_sideBtnContainer, sideButtonsVisibility);
		if (sideButtonsVisibility)
		{
			if (currentSpecificState.ClientSelectedCharacter != null)
			{
				CharacterType? clientSelectedCharacter = currentSpecificState.ClientSelectedCharacter;
				if (clientSelectedCharacter.Value.IsWillFill())
				{
					UIManager.SetGameObjectActive(m_bioBtn, false);
					UIManager.SetGameObjectActive(m_skinsBtn, true);
					UIManager.SetGameObjectActive(m_AbilitiesBtn, false);
					UIManager.SetGameObjectActive(m_CatalystBtn, false);
					UIManager.SetGameObjectActive(m_TauntsBtn, false);
					return;
				}
			}
			UIManager.SetGameObjectActive(m_bioBtn, true);
			UIManager.SetGameObjectActive(m_skinsBtn, true);
			UIManager.SetGameObjectActive(m_AbilitiesBtn, true);
			UIManager.SetGameObjectActive(m_CatalystBtn, true);
			UIManager.SetGameObjectActive(m_TauntsBtn, true);
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
		if (!gst.Requirements.IsNullOrEmpty())
		{
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
				if (gameManager != null)
				{
					if (gameManager.GameStatus != GameStatus.Stopped)
					{
						if (gameManager.GameStatus != GameStatus.None)
						{
							if (gameManager.GameInfo != null)
							{
								return gameManager.GameInfo.GameConfig.InstanceSubType.HasMod(GameSubType.SubTypeMods.ControlAllBots);
							}
						}
					}
				}
				using (Dictionary<ushort, GameSubType>.ValueCollection.Enumerator enumerator = SelectedGameSubTypes.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameSubType gameSubType = enumerator.Current;
						if (gameSubType.HasMod(GameSubType.SubTypeMods.ControlAllBots))
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		public bool CustomGamePartyIsVisible
		{
			get
			{
				if (CustomGamePartyListVisible != null)
				{
					if (CustomGamePartyListVisible.Value)
					{
						return true;
					}
				}
				return false;
			}
		}

		public bool CustomGamePartyIsHidden
		{
			get
			{
				if (CustomGamePartyListHidden != null)
				{
					return CustomGamePartyListHidden.Value;
				}
				return true;
			}
		}

		public bool SideButtonsVisibility
		{
			get
			{
				if (UIGameSettingsPanel.Get() != null)
				{
					if (UIGameSettingsPanel.Get().m_lastVisible)
					{
						return false;
					}
				}
				if (SideButtonsVisible != null)
				{
					return SideButtonsVisible.Value;
				}
				return false;
			}
		}

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
					if (SimpleBotSettingValueToDisplay == SimpleBotSettingValue.Easy)
					{
						return 3;
					}
					if (SimpleBotSettingValueToDisplay == SimpleBotSettingValue.Medium)
					{
						return 2;
					}
					if (SimpleBotSettingValueToDisplay == SimpleBotSettingValue.Hard)
					{
						return 1;
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
					if (SimpleBotSettingValueToDisplay == SimpleBotSettingValue.Easy)
					{
						return 0;
					}
					if (SimpleBotSettingValueToDisplay == SimpleBotSettingValue.Medium)
					{
						return 1;
					}
					if (SimpleBotSettingValueToDisplay == SimpleBotSettingValue.Hard)
					{
						return 3;
					}
				}
				return PlayerPrefs.GetInt("SoloEnemyDifficulty", 2);
			}
		}

		public ushort ExclusiveModBitsOfGameTypeToDisplay
		{
			get
			{
				ushort num = 0;
				Dictionary<ushort, GameSubType> validGameSubTypes = ValidGameSubTypes;
				using (Dictionary<ushort, GameSubType>.Enumerator enumerator = validGameSubTypes.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						KeyValuePair<ushort, GameSubType> keyValuePair = enumerator.Current;
						if (keyValuePair.Value.HasMod(GameSubType.SubTypeMods.Exclusive))
						{
							num |= keyValuePair.Key;
						}
					}
				}
				return num;
			}
		}

		public Dictionary<ushort, GameSubType> ValidGameSubTypes
		{
			get
			{
				if (GameTypeToDisplay == GameType.Coop)
				{
					Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(GameTypeToDisplay);
					Dictionary<ushort, GameSubType> dictionary = new Dictionary<ushort, GameSubType>();
					Dictionary<ushort, GameSubType> dictionary2 = new Dictionary<ushort, GameSubType>();
					using (Dictionary<ushort, GameSubType>.Enumerator enumerator = gameTypeSubTypes.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<ushort, GameSubType> keyValuePair = enumerator.Current;
							if (keyValuePair.Value.HasMod(GameSubType.SubTypeMods.ShowWithAITeammates))
							{
								dictionary2[keyValuePair.Key] = keyValuePair.Value;
							}
							else
							{
								dictionary[keyValuePair.Key] = keyValuePair.Value;
							}
						}
					}
					Dictionary<ushort, GameSubType> result;
					if (DisplayAllyBotTeammates)
					{
						result = dictionary2;
					}
					else
					{
						result = dictionary;
					}
					return result;
				}
				return ClientGameManager.Get().GetGameTypeSubTypes(GameTypeToDisplay);
			}
		}

		public bool GameSubTypesVisible
		{
			get
			{
				if (GameTypeToDisplay == GameType.Custom)
				{
					return false;
				}
				bool result;
				if (!ValidGameSubTypes.IsNullOrEmpty())
				{
					result = (ValidGameSubTypes.Count > 1);
				}
				else
				{
					result = false;
				}
				return result;
			}
		}

		public Dictionary<ushort, GameSubType> SelectedGameSubTypes
		{
			get
			{
				Dictionary<ushort, GameSubType> dictionary = new Dictionary<ushort, GameSubType>();
				foreach (KeyValuePair<ushort, GameSubType> keyValuePair in ValidGameSubTypes)
				{
					ushort? selectedSubTypeMask = SelectedSubTypeMask;
					int? num;
					if (selectedSubTypeMask != null)
					{
						num = selectedSubTypeMask.Value;
					}
					else
					{
						num = null;
					}
					int? num2 = num;
					int? num3;
					if (num2 != null)
					{
						num3 = keyValuePair.Key & num2.GetValueOrDefault();
					}
					else
					{
						num3 = null;
					}
					int? num4 = num3;
					bool flag;
					if (num4.GetValueOrDefault() == 0)
					{
						flag = (num4 == null);
					}
					else
					{
						flag = true;
					}
					if (flag)
					{
						dictionary[keyValuePair.Key] = keyValuePair.Value;
					}
				}
				return dictionary;
			}
		}

		public bool DisplayAllyBotTeammates
		{
			get
			{
				if (ClientRequestAllyBotTeammates != null)
				{
					return ClientRequestAllyBotTeammates.Value;
				}
				return AllyBotTeammatesSelected != null && AllyBotTeammatesSelected.Value;
			}
		}

		public SimpleBotSettingValue SimpleBotSettingValueToDisplay
		{
			get
			{
				if (ClientRequestedSimpleBotSettingValue != null)
				{
					return ClientRequestedSimpleBotSettingValue.Value;
				}
				if (SimpleBotSetting != null)
				{
					return SimpleBotSetting.Value;
				}
				return SimpleBotSettingValue.Easy;
			}
		}

		public BotDifficultyViewType BotDifficultyViewTypeToDisplay
		{
			get
			{
				if (!IsInGameLobby)
				{
					if (!IsInQueue)
					{
						if (IsGroupSubordinate)
						{
							return BotDifficultyViewType.Advanced;
						}
					}
				}
				if (BotDifficultyView != null)
				{
					BotDifficultyViewType? botDifficultyView = BotDifficultyView;
					return botDifficultyView.Value;
				}
				return BotDifficultyViewType.Simple;
			}
		}

		public bool BotSkillPanelVisible
		{
			get
			{
				return GameTypeToDisplay == GameType.Solo || GameTypeToDisplay == GameType.Coop;
			}
		}

		public GameType GameTypeToDisplay
		{
			get
			{
				GameType gameType;
				if (ClientRequestedGameType != null)
				{
					GameType? clientRequestedGameType = ClientRequestedGameType;
					gameType = clientRequestedGameType.Value;
				}
				else
				{
					LobbyGameConfig lobbyGameConfig;
					if (GameManager.Get() != null)
					{
						lobbyGameConfig = GameManager.Get().GameConfig;
					}
					else
					{
						lobbyGameConfig = null;
					}
					LobbyGameConfig lobbyGameConfig2 = lobbyGameConfig;
					if (IsInGameLobby && lobbyGameConfig2 != null)
					{
						gameType = lobbyGameConfig2.GameType;
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
				if (blockedExperienceAlternativeGameType != GameType.None)
				{
					if (clientGameManager.GetPlayerAccountData().ExperienceComponent.Matches < clientGameManager.NewPlayerPvPQueueDuration)
					{
						if (blockedExperienceEntries != null)
						{
							if (blockedExperienceEntries.Contains(MatchmakingQueueConfig.QueueEntryExperience.NewPlayer))
							{
								return blockedExperienceAlternativeGameType;
							}
						}
					}
				}
				return gameType;
			}
		}

		public CharacterResourceLink CharacterResourceLinkOfCharacterTypeToDisplay
		{
			get
			{
				if (CharacterTypeToDisplay.IsValidForHumanPreGameSelection())
				{
					return GameWideData.Get().GetCharacterResourceLink(CharacterTypeToDisplay);
				}
				return null;
			}
		}

		public CharacterType CharacterTypeToDisplay
		{
			get
			{
				if (AppState.GetCurrent() == AppState_CharacterSelect.Get()
				    && SelectedCharacterFromGameInfo.IsValidForHumanPreGameSelection())
				{
					return SelectedCharacterFromGameInfo;
				}
				if (ClientRequestToServerSelectCharacter != null)
				{
					return ClientRequestToServerSelectCharacter.Value;
				}
				if (SelectedCharacterInGroup.IsValidForHumanPreGameSelection())
				{
					return SelectedCharacterInGroup;
				}
				if (ClientSelectedCharacter != null)
				{
					return ClientSelectedCharacter.Value;
				}
				return SelectedCharacterFromPlayerData;
			}
		}

		public CharacterVisualInfo CharacterVisualInfoToDisplay
		{
			get
			{
				if (ClientSelectedVisualInfo != null)
				{
					return ClientSelectedVisualInfo.Value;
				}
				if (ClientGameManager.Get() != null)
				{
					if (ClientGameManager.Get().GroupInfo != null)
					{
						if (ClientGameManager.Get().GroupInfo.ChararacterInfo != null)
						{
							return ClientGameManager.Get().GroupInfo.ChararacterInfo.CharacterSkin;
						}
					}
				}
				return default(CharacterVisualInfo);
			}
		}

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
		RefreshSideButtonVisibility = 1,
		RefreshSideButtonClickability,
		RefreshSelectedCharacterButton = 4,
		SendRequestToServerForCharacterSelect = 8,
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
