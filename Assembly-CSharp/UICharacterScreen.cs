using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterScreen : UIScene
{
	[Header("Base Objects")]
	public UIScene.CloseObjectInfo[] MouseClickCloseObjects;

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

	private List<UICharacterScreen.GameSubTypeState> m_gameSubTypeBtns = new List<UICharacterScreen.GameSubTypeState>();

	private List<_ToggleSwap> m_gameTypeButtons = new List<_ToggleSwap>();

	private bool SentInitialSubTypes;

	private static UICharacterScreen.CharacterSelectSceneStateParameters m_currentState;

	public static UICharacterScreen Get()
	{
		return UICharacterScreen.s_instance;
	}

	public override UIScene.CloseObjectInfo[] GetMouseClickObjects()
	{
		return this.MouseClickCloseObjects;
	}

	public override void Awake()
	{
		UICharacterScreen.s_instance = this;
		this.RebuildCalls[1] = new UIScene.RebuildDelegate(this.RefreshSideButtonsVisibility);
		this.RebuildCalls[2] = new UIScene.RebuildDelegate(this.RefreshSideButtonsClickability);
		this.RebuildCalls[4] = new UIScene.RebuildDelegate(this.RefreshSelectedCharacterButton);
		this.RebuildCalls[8] = new UIScene.RebuildDelegate(this.SendRequestToServerForCharacterSelect);
		this.RebuildCalls[0x10] = new UIScene.RebuildDelegate(this.RefreshCharacterButtonsVisibility);
		this.RebuildCalls[0x20] = new UIScene.RebuildDelegate(this.RefreshSelectedGameType);
		this.RebuildCalls[0x40] = new UIScene.RebuildDelegate(this.RefreshCharacterButtons);
		this.RebuildCalls[0x80] = new UIScene.RebuildDelegate(this.RefreshBotSkillPanel);
		this.RebuildCalls[0x100] = new UIScene.RebuildDelegate(this.RefreshGameSubTypes);
		this.RebuildCalls[0x200] = new UIScene.RebuildDelegate(this.RefreshPartyList);
		this.SetupButtons();
		this.SetupCharacterButtons();
		this.SetupCharacterFilterButtons();
		_ToggleSwap[] componentsInChildren = this.m_GameTypeContainer.GetComponentsInChildren<_ToggleSwap>(true);
		this.m_gameTypeButtons.AddRange(componentsInChildren);
		ClientGameManager.Get().OnLobbyGameplayOverridesChange += this.OnLobbyGameplayOverridesUpdated;
		ClientGameManager.Get().OnGroupUpdateNotification += this.RefreshGameSubTypes;
		base.Awake();
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= this.OnLobbyGameplayOverridesUpdated;
			ClientGameManager.Get().OnGroupUpdateNotification -= this.RefreshGameSubTypes;
		}
	}

	public void OnLobbyGameplayOverridesUpdated(LobbyGameplayOverrides gameplayOverrides)
	{
		this.SetupCharacterButtons();
	}

	private void SetupCharacterFilterButtons()
	{
		this.m_searchInputField.onValueChanged.AddListener(new UnityAction<string>(this.EditedSearchInput));
		this.m_filterButtons = new List<UICharacterSelectFactionFilter>();
		List<CharacterType> list = new List<CharacterType>();
		list.AddRange((CharacterType[])Enum.GetValues(typeof(CharacterType)));
		this.m_filterButtons.Add(this.m_notOnAFactionFilter);
		List<FactionGroup> list2 = FactionWideData.Get().FactionGroupsToDisplayFilter();
		for (int i = 0; i < list2.Count; i++)
		{
			FactionGroup groupFilter = list2[i];
			UICharacterSelectFactionFilter uicharacterSelectFactionFilter = UnityEngine.Object.Instantiate<UICharacterSelectFactionFilter>(this.m_factionFilterPrefab);
			uicharacterSelectFactionFilter.transform.SetParent(this.m_searchFiltersContainer.transform);
			uicharacterSelectFactionFilter.transform.localPosition = Vector3.zero;
			uicharacterSelectFactionFilter.transform.localScale = Vector3.one;
			uicharacterSelectFactionFilter.Setup(groupFilter, new Action<UICharacterSelectFactionFilter>(this.ClickedOnFactionFilter));
			this.m_filterButtons.Add(uicharacterSelectFactionFilter);
			if (groupFilter.Characters != null)
			{
				list = list.Except(groupFilter.Characters).ToList<CharacterType>();
			}
			uicharacterSelectFactionFilter.m_btn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
			{
				(tooltip as UISimpleTooltip).Setup(FactionGroup.GetDisplayName(groupFilter.FactionGroupID));
				return true;
			}, null);
		}
		this.m_notOnAFactionFilter.Setup(list, new Action<UICharacterSelectFactionFilter>(this.ClickedOnFactionFilter));
		UITooltipObject component = this.m_notOnAFactionFilter.m_btn.spriteController.GetComponent<UITooltipHoverObject>();
		TooltipType tooltipType = TooltipType.Simple;
		
		component.Setup(tooltipType, delegate(UITooltipBase tooltip)
			{
				(tooltip as UISimpleTooltip).Setup(StringUtil.TR("Wildcard", "Global"));
				return true;
			}, null);
	}

	private void SetupCharacterButtons()
	{
		using (List<UICharacterPanelSelectButton>.Enumerator enumerator = this.CharacterSelectButtons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UICharacterPanelSelectButton uicharacterPanelSelectButton = enumerator.Current;
				if (!this.m_SelectWillFillBtns.Contains(uicharacterPanelSelectButton))
				{
					UnityEngine.Object.Destroy(uicharacterPanelSelectButton.gameObject);
				}
			}
		}
		this.CharacterSelectButtons.Clear();
		bool flag = GameManager.Get() != null && GameManager.Get().GameplayOverrides.EnableHiddenCharacters;
		CharacterType[] array = (CharacterType[])Enum.GetValues(typeof(CharacterType));
		List<CharacterType> list = new List<CharacterType>();
		List<CharacterType> list2 = new List<CharacterType>();
		List<CharacterType> list3 = new List<CharacterType>();
		for (int i = 0; i < array.Length; i++)
		{
			try
			{
				if (array[i] != CharacterType.TestFreelancer1)
				{
					if (array[i] != CharacterType.TestFreelancer2)
					{
						if (array[i] == CharacterType.None)
						{
						}
						else
						{
							CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(array[i]);
							if (!flag)
							{
								if (characterResourceLink.m_isHidden)
								{
									goto IL_193;
								}
							}
							if (characterResourceLink.m_characterRole == CharacterRole.Assassin)
							{
								list.Add(array[i]);
							}
							if (characterResourceLink.m_characterRole == CharacterRole.Support)
							{
								list3.Add(array[i]);
							}
							if (characterResourceLink.m_characterRole == CharacterRole.Tank)
							{
								list2.Add(array[i]);
							}
						}
					}
				}
			}
			catch
			{
			}
			IL_193:;
		}
		list.Sort(new Comparison<CharacterType>(this.CompareCharacterTypeName));
		list2.Sort(new Comparison<CharacterType>(this.CompareCharacterTypeName));
		list3.Sort(new Comparison<CharacterType>(this.CompareCharacterTypeName));
		int num = Mathf.CeilToInt((float)list.Count / 2f);
		int num2 = Mathf.CeilToInt((float)list2.Count / 2f);
		int num3 = Mathf.CeilToInt((float)list3.Count / 2f);
		int num4 = 0;
		for (int j = 0; j < list.Count; j++)
		{
			UICharacterPanelSelectButton uicharacterPanelSelectButton2 = UnityEngine.Object.Instantiate<UICharacterPanelSelectButton>(this.m_CharacterButtonSelectPrefab);
			uicharacterPanelSelectButton2.m_characterType = list[j];
			if (j - num4 * num >= num)
			{
				num4++;
			}
			UIManager.ReparentTransform(uicharacterPanelSelectButton2.gameObject.transform, this.m_FirepowerRows[num4].gameObject.transform);
			this.CharacterSelectButtons.Add(uicharacterPanelSelectButton2);
		}
		num4 = 0;
		for (int k = 0; k < list2.Count; k++)
		{
			UICharacterPanelSelectButton uicharacterPanelSelectButton3 = UnityEngine.Object.Instantiate<UICharacterPanelSelectButton>(this.m_CharacterButtonSelectPrefab);
			uicharacterPanelSelectButton3.m_characterType = list2[k];
			if (k - num4 * num2 >= num2)
			{
				num4++;
			}
			UIManager.ReparentTransform(uicharacterPanelSelectButton3.gameObject.transform, this.m_FrontlineRows[num4].gameObject.transform);
			this.CharacterSelectButtons.Add(uicharacterPanelSelectButton3);
		}
		num4 = 0;
		for (int l = 0; l < list3.Count; l++)
		{
			UICharacterPanelSelectButton uicharacterPanelSelectButton4 = UnityEngine.Object.Instantiate<UICharacterPanelSelectButton>(this.m_CharacterButtonSelectPrefab);
			uicharacterPanelSelectButton4.m_characterType = list3[l];
			if (l - num4 * num3 >= num3)
			{
				num4++;
			}
			UIManager.ReparentTransform(uicharacterPanelSelectButton4.gameObject.transform, this.m_SupportRows[num4].gameObject.transform);
			this.CharacterSelectButtons.Add(uicharacterPanelSelectButton4);
		}
		if (!this.m_SelectWillFillBtns.IsNullOrEmpty<UICharacterPanelSelectButton>())
		{
			foreach (UICharacterPanelSelectButton item in this.m_SelectWillFillBtns)
			{
				this.CharacterSelectButtons.Add(item);
			}
		}
		foreach (UICharacterPanelSelectButton uicharacterPanelSelectButton5 in this.CharacterSelectButtons)
		{
			uicharacterPanelSelectButton5.Setup(false, false);
		}
	}

	private int CompareCharacterTypeName(CharacterType CharA, CharacterType CharB)
	{
		return CharA.GetDisplayName().CompareTo(CharB.GetDisplayName());
	}

	public override SceneStateParameters GetCurrentState()
	{
		if (UICharacterScreen.m_currentState == null)
		{
			UICharacterScreen.m_currentState = new UICharacterScreen.CharacterSelectSceneStateParameters();
		}
		return UICharacterScreen.m_currentState;
	}

	public static UICharacterScreen.CharacterSelectSceneStateParameters GetCurrentSpecificState()
	{
		if (UICharacterScreen.m_currentState == null)
		{
			UICharacterScreen.m_currentState = new UICharacterScreen.CharacterSelectSceneStateParameters();
		}
		return UICharacterScreen.m_currentState;
	}

	public override bool DoesHandleParameter(SceneStateParameters parameters)
	{
		return parameters is UICharacterScreen.CharacterSelectSceneStateParameters;
	}

	public void DoRefreshFunctions(ushort RefreshBits)
	{
		using (Dictionary<int, UIScene.RebuildDelegate>.Enumerator enumerator = this.RebuildCalls.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<int, UIScene.RebuildDelegate> keyValuePair = enumerator.Current;
				if ((keyValuePair.Key & (int)RefreshBits) != 0)
				{
					keyValuePair.Value();
				}
			}
		}
	}

	public override void HandleNewSceneStateParameter(SceneStateParameters parameters)
	{
		UICharacterScreen.CharacterSelectSceneStateParameters characterSelectSceneStateParameters = parameters as UICharacterScreen.CharacterSelectSceneStateParameters;
		ushort num = 0;
		if (characterSelectSceneStateParameters != null)
		{
			if (characterSelectSceneStateParameters.ClientSelectedCharacter != null)
			{
				UICharacterScreen.m_currentState.ClientSelectedCharacter = characterSelectSceneStateParameters.ClientSelectedCharacter;
				num |= 1;
				num |= 4;
				num |= 0x40;
			}
			if (characterSelectSceneStateParameters.SideButtonsVisible != null)
			{
				UICharacterScreen.m_currentState.SideButtonsVisible = characterSelectSceneStateParameters.SideButtonsVisible;
				num |= 1;
			}
			if (characterSelectSceneStateParameters.SideButtonsClickable != null)
			{
				UICharacterScreen.m_currentState.SideButtonsClickable = characterSelectSceneStateParameters.SideButtonsClickable;
				num |= 2;
			}
			if (characterSelectSceneStateParameters.ClientSelectedVisualInfo != null)
			{
				UICharacterScreen.m_currentState.ClientSelectedVisualInfo = characterSelectSceneStateParameters.ClientSelectedVisualInfo;
			}
			if (characterSelectSceneStateParameters.ClientRequestToServerSelectCharacter != null)
			{
				UICharacterScreen.m_currentState.ClientRequestToServerSelectCharacter = characterSelectSceneStateParameters.ClientRequestToServerSelectCharacter;
				num |= 4;
				num |= 8;
			}
			if (characterSelectSceneStateParameters.CharacterSelectButtonsVisible != null)
			{
				UICharacterScreen.m_currentState.CharacterSelectButtonsVisible = characterSelectSceneStateParameters.CharacterSelectButtonsVisible;
				num |= 0x10;
			}
			if (characterSelectSceneStateParameters.ClientRequestedGameType != null)
			{
				UICharacterScreen.m_currentState.ClientRequestedGameType = characterSelectSceneStateParameters.ClientRequestedGameType;
				num |= 0x20;
				num |= 0x40;
				num |= 0x80;
				num |= 0x100;
			}
			if (characterSelectSceneStateParameters.BotDifficultyView != null)
			{
				UICharacterScreen.m_currentState.BotDifficultyView = characterSelectSceneStateParameters.BotDifficultyView;
				num |= 0x80;
			}
			if (characterSelectSceneStateParameters.AllyBotTeammatesSelected != null)
			{
				UICharacterScreen.m_currentState.AllyBotTeammatesSelected = characterSelectSceneStateParameters.AllyBotTeammatesSelected;
				num |= 0x80;
				num |= 0x100;
			}
			if (characterSelectSceneStateParameters.AllyBotTeammatesClickable != null)
			{
				UICharacterScreen.m_currentState.AllyBotTeammatesClickable = characterSelectSceneStateParameters.AllyBotTeammatesClickable;
				num |= 0x80;
			}
			if (characterSelectSceneStateParameters.BotsCanTauntCheckboxEnabled != null)
			{
				UICharacterScreen.m_currentState.BotsCanTauntCheckboxEnabled = characterSelectSceneStateParameters.BotsCanTauntCheckboxEnabled;
				num |= 0x80;
			}
			if (characterSelectSceneStateParameters.SimpleBotSetting != null)
			{
				UICharacterScreen.m_currentState.SimpleBotSetting = characterSelectSceneStateParameters.SimpleBotSetting;
				num |= 0x80;
			}
			if (characterSelectSceneStateParameters.ClientRequestedSimpleBotSettingValue != null)
			{
				UICharacterScreen.m_currentState.ClientRequestedSimpleBotSettingValue = characterSelectSceneStateParameters.ClientRequestedSimpleBotSettingValue;
				num |= 0x80;
			}
			if (characterSelectSceneStateParameters.ClientRequestAllyBotTeammates != null)
			{
				UICharacterScreen.m_currentState.ClientRequestAllyBotTeammates = characterSelectSceneStateParameters.ClientRequestAllyBotTeammates;
				num |= 0x80;
				num |= 0x100;
			}
			if (characterSelectSceneStateParameters.SelectedAllyBotDifficulty != null)
			{
				UICharacterScreen.m_currentState.SelectedAllyBotDifficulty = characterSelectSceneStateParameters.SelectedAllyBotDifficulty;
				num |= 0x80;
			}
			if (characterSelectSceneStateParameters.SelectedEnemyBotDifficulty != null)
			{
				UICharacterScreen.m_currentState.SelectedEnemyBotDifficulty = characterSelectSceneStateParameters.SelectedEnemyBotDifficulty;
				num |= 0x80;
			}
			if (characterSelectSceneStateParameters.ClientRequestedAllyBotDifficulty != null)
			{
				UICharacterScreen.m_currentState.ClientRequestedAllyBotDifficulty = characterSelectSceneStateParameters.ClientRequestedAllyBotDifficulty;
				num |= 0x80;
			}
			if (characterSelectSceneStateParameters.ClientRequestedEnemyBotDifficulty != null)
			{
				UICharacterScreen.m_currentState.ClientRequestedEnemyBotDifficulty = characterSelectSceneStateParameters.ClientRequestedEnemyBotDifficulty;
				num |= 0x80;
			}
			if (characterSelectSceneStateParameters.CustomGamePartyListVisible != null)
			{
				UICharacterScreen.m_currentState.CustomGamePartyListVisible = characterSelectSceneStateParameters.CustomGamePartyListVisible;
				num |= 0x200;
			}
			if (characterSelectSceneStateParameters.CustomGamePartyListHidden != null)
			{
				UICharacterScreen.m_currentState.CustomGamePartyListHidden = characterSelectSceneStateParameters.CustomGamePartyListHidden;
				num |= 0x200;
			}
		}
		this.DoRefreshFunctions(num);
		base.HandleNewSceneStateParameter(parameters);
	}

	public void UpdateSubTypeMaskChecks(ushort subTypeMask)
	{
		for (int i = 0; i < this.m_gameSubTypeBtns.Count; i++)
		{
			if ((this.m_gameSubTypeBtns[i].SubTypeBit & subTypeMask) != 0)
			{
				this.m_gameSubTypeBtns[i].btn.SetOn(true, false);
			}
			else
			{
				this.m_gameSubTypeBtns[i].btn.SetOn(false, false);
			}
		}
	}

	private void CheckSubTypeSelection(bool sendMaskUpdate = false, ushort oldMask = 0)
	{
		ushort newMask = 0;
		UICharacterScreen.CharacterSelectSceneStateParameters Parameters = UICharacterScreen.GetCurrentSpecificState();
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
					for (int i = 0; i < this.m_gameSubTypeBtns.Count; i++)
					{
						if (this.m_gameSubTypeBtns[i].btn.IsChecked())
						{
							newMask = (ushort)(this.m_gameSubTypeBtns[i].SubTypeBit | newMask);
						}
					}
				}
			}
			else
			{
				Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(GameType.Coop);
				if (!gameTypeSubTypes.IsNullOrEmpty<KeyValuePair<ushort, GameSubType>>())
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
			if (!gameTypeSubTypes2.IsNullOrEmpty<KeyValuePair<ushort, GameSubType>>())
			{
				using (Dictionary<ushort, GameSubType>.Enumerator enumerator2 = gameTypeSubTypes2.GetEnumerator())
				{
					if (!enumerator2.MoveNext())
					{
					}
					else
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
			for (int j = 0; j < this.m_gameSubTypeBtns.Count; j++)
			{
				if (this.m_gameSubTypeBtns[j].btn.IsChecked())
				{
					if ((exclusiveModBitsOfGameTypeToDisplay | this.m_gameSubTypeBtns[j].SubTypeBit) != 0)
					{
						num = this.m_gameSubTypeBtns[j].SubTypeBit;
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
			for (int k = 0; k < this.m_gameSubTypeBtns.Count; k++)
			{
				if (this.m_gameSubTypeBtns[k].btn.IsChecked())
				{
					newMask = (ushort)(this.m_gameSubTypeBtns[k].SubTypeBit | newMask);
				}
			}
		}
		ushort num2 = 0;
		for (int l = 0; l < this.m_gameSubTypeBtns.Count; l++)
		{
			num2 = (ushort)(this.m_gameSubTypeBtns[l].SubTypeBit | num2);
		}
		if (num2 != 0)
		{
			if ((newMask & num2) == 0)
			{
				this.m_gameSubTypeBtns[0].btn.SetOn(true, false);
				newMask = this.m_gameSubTypeBtns[0].SubTypeBit;
			}
		}
		Parameters.SelectedSubTypeMask = new ushort?(newMask);
		this.UpdateSubTypeMaskChecks(newMask);
		if (!sendMaskUpdate)
		{
			if (this.SentInitialSubTypes)
			{
				goto IL_4EE;
			}
		}
		this.SentInitialSubTypes = true;
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
						Log.Warning(text, new object[0]);
						UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("Error", "Global"), text, StringUtil.TR("Ok", "Global"), null, -1, false);
					}
					else
					{
						ClientGameManager.Get().SetSoloSubGameMask(Parameters.GameTypeToDisplay, newMask);
						this.UpdateSubTypeMaskChecks(newMask);
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
					Log.Warning(text, new object[0]);
					UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("Error", "Global"), text, StringUtil.TR("Ok", "Global"), null, -1, false);
				}
				else
				{
					ClientGameManager.Get().SetSoloSubGameMask(Parameters.GameTypeToDisplay, newMask);
					this.UpdateSubTypeMaskChecks(newMask);
				}
			});
		}
		IL_4EE:
		this.UpdateWillFillVisibility();
		this.DoRefreshFunctions(0x200);
	}

	public void EditedSearchInput(string input)
	{
		this.UpdateCharacterButtonHighlights();
	}

	public void ClickedOnFactionFilter(UICharacterSelectFactionFilter btn)
	{
		if (this.m_lastFilterBtnClicked != null)
		{
			if (this.m_lastFilterBtnClicked != btn)
			{
				this.m_lastFilterBtnClicked.m_btn.SetSelected(false, false, string.Empty, string.Empty);
			}
		}
		this.m_lastFilterBtnClicked = btn;
		this.UpdateCharacterButtonHighlights();
	}

	private void UpdateCharacterButtonHighlights()
	{
		for (int i = 0; i < this.CharacterSelectButtons.Count; i++)
		{
			if (this.CharacterSelectButtons[i] != null)
			{
				if (this.CharacterSelectButtons[i].GetComponent<CanvasGroup>() != null)
				{
					CanvasGroup component = this.CharacterSelectButtons[i].GetComponent<CanvasGroup>();
					if (component != null)
					{
						component.alpha = 1f;
					}
				}
			}
		}
		if (this.m_lastFilterBtnClicked != null)
		{
			if (this.m_lastFilterBtnClicked.m_btn.IsSelected())
			{
				for (int j = 0; j < this.CharacterSelectButtons.Count; j++)
				{
					if (!this.m_lastFilterBtnClicked.IsAvailable(this.CharacterSelectButtons[j].m_characterType))
					{
						CanvasGroup component2 = this.CharacterSelectButtons[j].GetComponent<CanvasGroup>();
						if (component2 != null)
						{
							component2.alpha = 0.3f;
						}
					}
				}
			}
		}
		if (!this.m_searchInputField.text.IsNullOrEmpty())
		{
			for (int k = 0; k < this.CharacterSelectButtons.Count; k++)
			{
				CharacterResourceLink characterResourceLink = this.CharacterSelectButtons[k].GetCharacterResourceLink();
				if (characterResourceLink != null)
				{
					string displayName = characterResourceLink.GetDisplayName();
					if (!this.DoesSearchMatchDisplayName(this.m_searchInputField.text.ToLower(), displayName.ToLower()))
					{
						CanvasGroup component3 = this.CharacterSelectButtons[k].GetComponent<CanvasGroup>();
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
			else
			{
				if (searchText[i] != displayText[i])
				{
					return false;
				}
				i++;
			}
		}
		return true;
	}

	public void SubTypeClicked(_ToggleSwap btn)
	{
		this.CheckSubTypeSelection(true, 0);
	}

	private void SetupButtons()
	{
		this.m_bioBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.BioBtnClicked);
		this.m_skinsBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SkinsBtnClicked);
		this.m_AbilitiesBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.AbilitiesBtnClicked);
		this.m_CatalystBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CatalystsBtnClicked);
		this.m_TauntsBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.TauntsBtnClicked);
		this.m_skinsBtn.spriteController.pointerEnterCallback = new _ButtonSwapSprite.ButtonClickCallback(this.SkinMouseOver);
		this.m_skinsBtn.spriteController.pointerExitCallback = new _ButtonSwapSprite.ButtonClickCallback(this.SkinMouseExit);
		this.m_AbilitiesBtn.spriteController.pointerEnterCallback = new _ButtonSwapSprite.ButtonClickCallback(this.AbilityMouseOver);
		this.m_AbilitiesBtn.spriteController.pointerExitCallback = new _ButtonSwapSprite.ButtonClickCallback(this.AbilityMouseExit);
		this.m_CatalystBtn.spriteController.pointerEnterCallback = new _ButtonSwapSprite.ButtonClickCallback(this.CatalystMouseOver);
		this.m_CatalystBtn.spriteController.pointerExitCallback = new _ButtonSwapSprite.ButtonClickCallback(this.CatalystMouseExit);
		this.SkinSubButtons = this.m_skinsBtn.GetComponentsInChildren<_ButtonSwapSprite>(true);
		this.AbilitySubButtons = this.m_AbilitiesBtn.GetComponentsInChildren<_ButtonSwapSprite>(true);
		this.CatalystSubButtons = this.m_CatalystBtn.GetComponentsInChildren<_ButtonSwapSprite>(true);
		for (int i = 0; i < this.SkinSubButtons.Length; i++)
		{
			if (this.SkinSubButtons[i] != this.m_skinsBtn.spriteController)
			{
				this.m_skinsBtn.spriteController.AddSubButton(this.SkinSubButtons[i]);
			}
		}
		for (int j = 0; j < this.AbilitySubButtons.Length; j++)
		{
			if (this.AbilitySubButtons[j] != this.m_AbilitiesBtn.spriteController)
			{
				this.m_AbilitiesBtn.spriteController.AddSubButton(this.AbilitySubButtons[j]);
			}
		}
		for (int k = 0; k < this.CatalystSubButtons.Length; k++)
		{
			if (this.CatalystSubButtons[k] != this.m_CatalystBtn.spriteController)
			{
				this.m_CatalystBtn.spriteController.AddSubButton(this.CatalystSubButtons[k]);
			}
		}
		this.m_skinsBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, (UITooltipBase tooltip) => this.SideMenuOpen(tooltip, this.m_skinsBtn), null);
		this.m_AbilitiesBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, (UITooltipBase tooltip) => this.SideMenuOpen(tooltip, this.m_AbilitiesBtn), null);
		this.m_CatalystBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, (UITooltipBase tooltip) => this.SideMenuOpen(tooltip, this.m_CatalystBtn), null);
		for (int l = 0; l < this.m_AbilityMouseOverBtns.Length; l++)
		{
			this.m_AbilityMouseOverBtns[l].spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ClickedAbilityIcon);
			int index = l;
			this.m_AbilityMouseOverBtns[l].spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, (UITooltipBase tooltip) => this.SetupAbilitySideBtnTooltip(tooltip, index), null);
		}
		for (int m = 0; m < this.m_AbilityModIcons.Length; m++)
		{
			UIManager.SetGameObjectActive(this.m_AbilityModIcons[m], false, null);
		}
		for (int n = 0; n < this.m_CatalsytBtns.Length; n++)
		{
			this.m_CatalsytBtns[n].spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ClickedCatalystIcon);
			AbilityRunPhase phase = n + AbilityRunPhase.Prep;
			this.m_CatalsytBtns[n].spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, delegate(UITooltipBase tooltip)
			{
				if (this.SelectedCatalysts.ContainsKey(phase))
				{
					return this.SetupCatalystSideBtnTooltip(tooltip, this.SelectedCatalysts[phase]);
				}
				return false;
			}, null);
		}
		if (this.m_selectedSkinColorBtn != null)
		{
			this.m_selectedSkinColorBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ClickedSkinIcon);
			this.m_selectedSkinColorBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.SetupSkinTooltip), null);
		}
		this.m_easyBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.EasyClicked);
		this.m_mediumBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.MediumClicked);
		this.m_hardBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.HardClicked);
		this.m_teamBotsToggle.changedNotify = new _ToggleSwap.NotifyChanged(this.AllyBotsToggleEvent);
		this.m_simpleCogBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SimpleBtnClicked);
		this.m_advancedCogBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.AdvancedBtnClicked);
		this.m_dropdownBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.DropdownClicked);
	}

	public void DropdownClicked(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_DifficultyListDropdown, !this.m_DifficultyListDropdown.gameObject.activeSelf, null);
	}

	public void SimpleBtnClicked(BaseEventData data)
	{
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			BotDifficultyView = new UICharacterScreen.CharacterSelectSceneStateParameters.BotDifficultyViewType?(UICharacterScreen.CharacterSelectSceneStateParameters.BotDifficultyViewType.Simple)
		});
	}

	public void AdvancedBtnClicked(BaseEventData data)
	{
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			BotDifficultyView = new UICharacterScreen.CharacterSelectSceneStateParameters.BotDifficultyViewType?(UICharacterScreen.CharacterSelectSceneStateParameters.BotDifficultyViewType.Advanced)
		});
	}

	public void AllyBotsToggleEvent(_ToggleSwap btn)
	{
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			ClientRequestAllyBotTeammates = new bool?(btn.IsChecked())
		});
	}

	public void EasyClicked(BaseEventData data)
	{
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			ClientRequestedSimpleBotSettingValue = new UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue?(UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue.Easy)
		});
	}

	public void MediumClicked(BaseEventData data)
	{
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			ClientRequestedSimpleBotSettingValue = new UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue?(UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue.Medium)
		});
	}

	public void HardClicked(BaseEventData data)
	{
		UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
		{
			ClientRequestedSimpleBotSettingValue = new UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue?(UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue.Hard)
		});
	}

	private bool SetupSkinTooltip(UITooltipBase tooltip)
	{
		UICharacterSelectSkinPanel uicharacterSelectSkinPanel = UICharacterSelectCharacterSettingsPanel.Get().m_skinsSubPanel.m_selectHandler as UICharacterSelectSkinPanel;
		if (uicharacterSelectSkinPanel != null)
		{
			GameWideData gameWideData = GameWideData.Get();
			CharacterType? clientSelectedCharacter = UICharacterScreen.GetCurrentSpecificState().ClientSelectedCharacter;
			CharacterResourceLink characterResourceLink = gameWideData.GetCharacterResourceLink(clientSelectedCharacter.Value);
			CharacterVisualInfo? clientSelectedVisualInfo = UICharacterScreen.GetCurrentSpecificState().ClientSelectedVisualInfo;
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
		if (!this.m_CatalystBtn.IsHover)
		{
			return false;
		}
		string text = card.GetDisplayName();
		if (!card.m_useAbility.m_flavorText.IsNullOrEmpty())
		{
			string text2 = text;
			text = string.Concat(new string[]
			{
				text2,
				Environment.NewLine,
				"<i>",
				card.m_useAbility.m_flavorText,
				"</i>"
			});
		}
		UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
		uititledTooltip.Setup(string.Format(StringUtil.TR("CatalystTitle", "Global"), card.m_useAbility.GetPhaseString()), text, string.Empty);
		return true;
	}

	private bool SetupAbilitySideBtnTooltip(UITooltipBase tooltip, int i)
	{
		if (!this.m_AbilitiesBtn.IsHover)
		{
			return false;
		}
		AbilityData.AbilityEntry abilityEntry = this.SelectedAbilityData[i].GetAbilityEntry();
		if (abilityEntry != null)
		{
			if (!(abilityEntry.ability == null))
			{
				UIAbilityTooltip uiabilityTooltip = (UIAbilityTooltip)tooltip;
				string movieAssetName = new StringBuilder().Append("Video/AbilityPreviews/").Append(abilityEntry.ability.m_previewVideo).ToString();
				uiabilityTooltip.Setup(abilityEntry.ability, this.SelectedAbilityData[i].GetSelectedMod(), movieAssetName);
				return true;
			}
		}
		return false;
	}

	public void UpdateCatalystIcons(Dictionary<AbilityRunPhase, Card> phaseToCards)
	{
		this.SelectedCatalysts = phaseToCards;
		using (Dictionary<AbilityRunPhase, Card>.Enumerator enumerator = this.SelectedCatalysts.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<AbilityRunPhase, Card> keyValuePair = enumerator.Current;
				Card value = keyValuePair.Value;
				if (-1 < value.GetAbilityRunPhase() - AbilityRunPhase.Prep)
				{
					if (value.GetAbilityRunPhase() - AbilityRunPhase.Prep < this.m_CatalystIcons.Length)
					{
						UIManager.SetGameObjectActive(this.m_CatalystIcons[value.GetAbilityRunPhase() - AbilityRunPhase.Prep], true, null);
						this.m_CatalystIcons[value.GetAbilityRunPhase() - AbilityRunPhase.Prep].sprite = value.GetIconSprite();
					}
				}
				if (-1 < value.GetAbilityRunPhase() - AbilityRunPhase.Prep)
				{
					if (value.GetAbilityRunPhase() - AbilityRunPhase.Prep < this.m_CatalystHoverIcons.Length)
					{
						UIManager.SetGameObjectActive(this.m_CatalystHoverIcons[value.GetAbilityRunPhase() - AbilityRunPhase.Prep], true, null);
						this.m_CatalystHoverIcons[value.GetAbilityRunPhase() - AbilityRunPhase.Prep].sprite = value.GetIconSprite();
					}
				}
			}
		}
	}

	public void UpdateModIcons(UIAbilityButtonModPanel[] SelectedAbilities, Color prepColor, Color dashColor, Color combatColor)
	{
		this.SelectedAbilityData = SelectedAbilities;
		for (int i = 0; i < this.m_AbilityMouseOverBtns.Length; i++)
		{
			UIAbilityButtonModPanel uiabilityButtonModPanel = SelectedAbilities[i];
			this.m_AbilityIcons[i].sprite = uiabilityButtonModPanel.m_abilityIcon[0].sprite;
			UIQueueListPanel.UIPhase uiphase = UIQueueListPanel.UIPhase.None;
			AbilityData.AbilityEntry abilityEntry = uiabilityButtonModPanel.GetAbilityEntry();
			if (abilityEntry != null)
			{
				if (abilityEntry.ability != null)
				{
					uiphase = UIQueueListPanel.GetUIPhaseFromAbilityPriority(abilityEntry.ability.RunPriority);
				}
				else if (UICharacterScreen.m_currentState != null)
				{
					if (UICharacterScreen.m_currentState.CharacterTypeToDisplay != CharacterType.PendingWillFill)
					{
						Log.Warning("Ability entry has no ability!", new object[0]);
					}
				}
			}
			else
			{
				Log.Warning("AbilityButton has no Ability Entry!", new object[0]);
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
			if (i < this.m_AbilityPhaseColors.Length)
			{
				this.m_AbilityPhaseColors[i].color = color;
			}
			if (i < this.m_AbilityPhaseColorsGradient.Length)
			{
				this.m_AbilityPhaseColorsGradient[i].color = color;
			}
			if (uiabilityButtonModPanel.GetSelectedMod() != null)
			{
				UIManager.SetGameObjectActive(this.m_AbilityModIcons[i], true, null);
				this.m_AbilityModIcons[i].sprite = uiabilityButtonModPanel.GetSelectedMod().m_iconSprite;
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_AbilityModIcons[i], false, null);
				this.m_AbilityModIcons[i].sprite = null;
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
		for (int i = 0; i < this.m_AbilityMouseOverBtns.Length; i++)
		{
			if (this.m_AbilityMouseOverBtns[i].spriteController.gameObject == (data as PointerEventData).selectedObject)
			{
				num = i;
				break;
			}
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		UICharacterSelectCharacterSettingsPanel.Get().SetVisible(true, UICharacterSelectCharacterSettingsPanel.TabPanel.Abilities);
		if (num > -1)
		{
			UICharacterSelectCharacterSettingsPanel.Get().m_abilitiesSubPanel.AbilityButtonSelected(num, true);
		}
	}

	private bool SideMenuOpen(UITooltipBase tooltip, _SelectableBtn btn)
	{
		if (btn == this.m_CatalystBtn)
		{
			if (!GameManager.Get().GameplayOverrides.EnableCards)
			{
				this.m_CatalystBtn.spriteController.SetClickable(false);
				this.m_CatalystBtn.spriteController.SetForceHovercallback(true);
				this.m_CatalystBtn.spriteController.SetForceExitCallback(true);
				UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
				uititledTooltip.Setup(StringUtil.TR("Disabled", "Global"), StringUtil.TR("CatalystsAreDisabled", "Global"), string.Empty);
				return true;
			}
			this.m_CatalystBtn.spriteController.SetClickable(true);
			this.m_CatalystBtn.spriteController.SetForceHovercallback(false);
			this.m_CatalystBtn.spriteController.SetForceExitCallback(false);
		}
		return false;
	}

	public void SkinMouseOver(BaseEventData data)
	{
		for (int i = 0; i < this.SkinSubButtons.Length; i++)
		{
			if (this.SkinSubButtons[i] != this.m_skinsBtn.spriteController)
			{
				this.SkinSubButtons[i].SetClickable(true);
			}
		}
	}

	public void SkinMouseExit(BaseEventData data)
	{
		for (int i = 0; i < this.SkinSubButtons.Length; i++)
		{
			if (this.SkinSubButtons[i] != this.m_skinsBtn.spriteController)
			{
				this.SkinSubButtons[i].SetClickable(false);
			}
		}
	}

	public void AbilityMouseOver(BaseEventData data)
	{
		for (int i = 0; i < this.AbilitySubButtons.Length; i++)
		{
			if (this.AbilitySubButtons[i] != this.m_AbilitiesBtn.spriteController)
			{
				this.AbilitySubButtons[i].SetClickable(true);
			}
		}
	}

	public void AbilityMouseExit(BaseEventData data)
	{
		for (int i = 0; i < this.AbilitySubButtons.Length; i++)
		{
			if (this.AbilitySubButtons[i] != this.m_AbilitiesBtn.spriteController)
			{
				this.AbilitySubButtons[i].SetClickable(false);
			}
		}
	}

	public void CatalystMouseOver(BaseEventData data)
	{
		for (int i = 0; i < this.CatalystSubButtons.Length; i++)
		{
			if (this.CatalystSubButtons[i] != this.m_CatalystBtn.spriteController)
			{
				this.CatalystSubButtons[i].SetClickable(true);
			}
		}
	}

	public void CatalystMouseExit(BaseEventData data)
	{
		for (int i = 0; i < this.CatalystSubButtons.Length; i++)
		{
			if (this.CatalystSubButtons[i] != this.m_CatalystBtn.spriteController)
			{
				this.CatalystSubButtons[i].SetClickable(false);
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
		UICharacterScreen.GetCurrentSpecificState().ClientRequestToServerSelectCharacter = null;
		if (response.Success)
		{
			if (response.CharacterInfo != null)
			{
				UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
				{
					ClientSelectedCharacter = new CharacterType?(response.CharacterInfo.CharacterType)
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
		GameType gameTypeToDisplay = UICharacterScreen.GetCurrentSpecificState().GameTypeToDisplay;
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
					select ti).ToList<LobbyPlayerInfo>();
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
		UICharacterScreen.GetCurrentSpecificState().ClientRequestedGameType = null;
		this.DoRefreshFunctions(0x20);
	}

	private void SetDropdownText(string text)
	{
		TextMeshProUGUI[] componentsInChildren = this.m_dropdownBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].text = text;
		}
	}

	public void RefreshPartyList()
	{
		UICharacterScreen.CharacterSelectSceneStateParameters currentSpecificState = UICharacterScreen.GetCurrentSpecificState();
		UIManager.SetGameObjectActive(this.m_partyListPanel, currentSpecificState.PartyListVisbility, null);
		this.m_partyListPanel.SetVisible(currentSpecificState.PartyListVisbility, false);
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
				this.m_partyListPanel.SetupForOutOfGame(num, isDuplicateCharsAllowed);
			}
		}
	}

	public void RefreshGameSubTypes()
	{
		UICharacterScreen.CharacterSelectSceneStateParameters currentSpecificState = UICharacterScreen.GetCurrentSpecificState();
		ushort num = 0;
		bool sendMaskUpdate = false;
		if (currentSpecificState.ClientRequestAllyBotTeammates != null)
		{
			for (int i = 0; i < this.m_gameSubTypeBtns.Count; i++)
			{
				if (this.m_gameSubTypeBtns[i].btn.IsChecked())
				{
					num |= this.m_gameSubTypeBtns[i].SubTypeBit;
				}
			}
			currentSpecificState.AllyBotTeammatesSelected = new bool?(currentSpecificState.ClientRequestAllyBotTeammates.Value);
			sendMaskUpdate = true;
			currentSpecificState.ClientRequestAllyBotTeammates = null;
		}
		if (currentSpecificState.ClientRequestedGameType != null)
		{
			sendMaskUpdate = true;
		}
		this.m_gameSubTypeBtns.Clear();
		GameType gameTypeToDisplay = currentSpecificState.GameTypeToDisplay;
		if (ClientGameManager.Get().GameTypeAvailabilies.ContainsKey(gameTypeToDisplay))
		{
			bool inAGroup = ClientGameManager.Get().GroupInfo.InAGroup;
			int j = 0;
			Dictionary<ushort, GameSubType> validGameSubTypes = currentSpecificState.ValidGameSubTypes;
			if (!validGameSubTypes.IsNullOrEmpty<KeyValuePair<ushort, GameSubType>>())
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
							bool flag = UICharacterScreen.IsGameSubTypeActive(gameTypeToDisplay, keyValuePair.Value);
							if (flag && keyValuePair.Value.HasMod(GameSubType.SubTypeMods.NotAllowedForGroups))
							{
								flag = !inAGroup;
							}
							if (!flag || !keyValuePair.Value.HasMod(GameSubType.SubTypeMods.CanBeConsolidated))
							{
								while (j >= this.m_gameTypeButtons.Count)
								{
									_ToggleSwap toggleSwap = UnityEngine.Object.Instantiate<_ToggleSwap>(this.m_GameTypePrefab);
									toggleSwap.transform.SetParent(this.m_GameTypeContainer.transform);
									toggleSwap.transform.localEulerAngles = Vector3.zero;
									toggleSwap.transform.localScale = Vector3.one;
									toggleSwap.transform.localPosition = Vector3.zero;
									this.m_gameTypeButtons.Add(toggleSwap);
								}
								this.m_gameSubTypeBtns.Add(new UICharacterScreen.GameSubTypeState
								{
									btn = this.m_gameTypeButtons[j],
									SubTypeBit = keyValuePair.Key
								});
								UIManager.SetGameObjectActive(this.m_gameTypeButtons[j], flag, null);
								TextMeshProUGUI componentInChildren = this.m_gameTypeButtons[j].GetComponentInChildren<TextMeshProUGUI>(true);
								componentInChildren.text = StringUtil.TR(keyValuePair.Value.LocalizedName);
								this.m_gameTypeButtons[j].changedNotify = new _ToggleSwap.NotifyChanged(this.SubTypeClicked);
								if (!flag)
								{
									goto IL_392;
								}
								if ((num2 & keyValuePair.Key) == 0)
								{
									goto IL_392;
								}
								this.m_gameTypeButtons[j].SetOn(true, false);
								IL_3A8:
								j++;
								continue;
								IL_392:
								this.m_gameTypeButtons[j].SetOn(false, false);
								goto IL_3A8;
							}
							num3 |= keyValuePair.Key;
						}
					}
					if (num3 != 0)
					{
						while (j >= this.m_gameTypeButtons.Count)
						{
							_ToggleSwap toggleSwap2 = UnityEngine.Object.Instantiate<_ToggleSwap>(this.m_GameTypePrefab);
							toggleSwap2.transform.SetParent(this.m_GameTypeContainer.transform);
							toggleSwap2.transform.localEulerAngles = Vector3.zero;
							toggleSwap2.transform.localScale = Vector3.one;
							toggleSwap2.transform.localPosition = Vector3.zero;
							this.m_gameTypeButtons.Add(toggleSwap2);
						}
						this.m_gameSubTypeBtns.Add(new UICharacterScreen.GameSubTypeState
						{
							btn = this.m_gameTypeButtons[j],
							SubTypeBit = num3
						});
						UIManager.SetGameObjectActive(this.m_gameTypeButtons[j], true, null);
						if ((num2 & num3) != 0)
						{
							this.m_gameTypeButtons[j].SetOn(true, false);
						}
						else
						{
							this.m_gameTypeButtons[j].SetOn(false, false);
						}
						TextMeshProUGUI componentInChildren2 = this.m_gameTypeButtons[j].GetComponentInChildren<TextMeshProUGUI>(true);
						componentInChildren2.text = StringUtil.TR("ConsolidatedGameSubTypes", "SubTypes");
						this.m_gameTypeButtons[j].changedNotify = new _ToggleSwap.NotifyChanged(this.SubTypeClicked);
						j++;
					}
				}
			}
			while (j < this.m_gameTypeButtons.Count)
			{
				UIManager.SetGameObjectActive(this.m_gameTypeButtons[j], false, null);
				j++;
			}
		}
		UIManager.SetGameObjectActive(this.m_GameTypeContainer, currentSpecificState.GameSubTypesVisible, null);
		this.CheckSubTypeSelection(sendMaskUpdate, num);
	}

	private void SendBotDifficultyUpdateToServer(BotDifficulty? AllyDifficulty, BotDifficulty? EnemyDifficulty)
	{
		if (GameManager.Get().TeamInfo != null)
		{
			if (!GameManager.Get().TeamInfo.TeamBPlayerInfo.IsNullOrEmpty<LobbyPlayerInfo>())
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
				ClientGameManager.Get().UpdateBotDifficulty(AllyDifficulty, EnemyDifficulty, 0);
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
		UICharacterScreen.CharacterSelectSceneStateParameters currentSpecificState = UICharacterScreen.GetCurrentSpecificState();
		GameType gameTypeToDisplay = currentSpecificState.GameTypeToDisplay;
		bool flag = !SceneStateParameters.IsInGameLobby && !SceneStateParameters.IsInQueue && !SceneStateParameters.IsGroupSubordinate;
		this.m_teamBotStars.SetClickable(flag);
		this.m_enemyBotStars.SetClickable(flag);
		this.m_teamBotsToggle.SetClickable(flag);
		for (int i = 0; i < this.m_gameTypeButtons.Count; i++)
		{
			this.m_gameTypeButtons[i].SetClickable(flag);
		}
		this.m_simpleCogBtn.spriteController.SetClickable(flag);
		this.m_advancedCogBtn.spriteController.SetClickable(flag);
		this.m_dropdownBtn.spriteController.SetClickable(flag);
		if (!flag)
		{
			UIManager.SetGameObjectActive(this.m_DifficultyListDropdown, false, null);
		}
		bool displayAllyBotTeammates = currentSpecificState.DisplayAllyBotTeammates;
		this.m_teamBotsToggle.SetOn(displayAllyBotTeammates, false);
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
		this.m_enemyBotStars.SetCurrentValue(enemyBotDifficultyToDisplay + 1);
		int? clientRequestedEnemyBotDifficulty = currentSpecificState.ClientRequestedEnemyBotDifficulty;
		if (clientRequestedEnemyBotDifficulty != null)
		{
			PlayerPrefs.SetInt(key, enemyBotDifficultyToDisplay);
			this.SendBotDifficultyUpdateToServer(null, new BotDifficulty?((BotDifficulty)enemyBotDifficultyToDisplay));
			currentSpecificState.SelectedEnemyBotDifficulty = currentSpecificState.ClientRequestedEnemyBotDifficulty;
			currentSpecificState.ClientRequestedEnemyBotDifficulty = null;
		}
		int allyBotDifficultyToDisplay = currentSpecificState.AllyBotDifficultyToDisplay;
		this.m_teamBotStars.SetCurrentValue(allyBotDifficultyToDisplay + 1);
		int? clientRequestedAllyBotDifficulty = currentSpecificState.ClientRequestedAllyBotDifficulty;
		if (clientRequestedAllyBotDifficulty != null)
		{
			PlayerPrefs.SetInt("SoloAllyDifficulty", allyBotDifficultyToDisplay);
			this.SendBotDifficultyUpdateToServer(new BotDifficulty?((BotDifficulty)allyBotDifficultyToDisplay), null);
			currentSpecificState.SelectedAllyBotDifficulty = currentSpecificState.ClientRequestedAllyBotDifficulty;
			currentSpecificState.ClientRequestedAllyBotDifficulty = null;
		}
		IL_217:
		UIManager.SetGameObjectActive(this.m_botSkillPanel, currentSpecificState.BotSkillPanelVisible, null);
		UIManager.SetGameObjectActive(this.m_DifficultyListDropdown, false, null);
		bool flag2 = currentSpecificState.BotDifficultyViewTypeToDisplay == UICharacterScreen.CharacterSelectSceneStateParameters.BotDifficultyViewType.Simple;
		if (flag2)
		{
			UIManager.SetGameObjectActive(this.m_simpleCogBtn, false, null);
			UIManager.SetGameObjectActive(this.m_advancedCogBtn, true, null);
			UIManager.SetGameObjectActive(this.m_dropdownBtn, true, null);
			UIManager.SetGameObjectActive(this.m_difficultyListContainer, true, null);
			UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue simpleBotSettingValueToDisplay = currentSpecificState.SimpleBotSettingValueToDisplay;
			this.m_easyBtn.SetSelected(simpleBotSettingValueToDisplay == UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue.Easy, false, string.Empty, string.Empty);
			this.m_mediumBtn.SetSelected(simpleBotSettingValueToDisplay == UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue.Medium, false, string.Empty, string.Empty);
			this.m_hardBtn.SetSelected(simpleBotSettingValueToDisplay == UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue.Hard, false, string.Empty, string.Empty);
			if (simpleBotSettingValueToDisplay != UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue.Easy)
			{
				if (simpleBotSettingValueToDisplay != UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue.Medium)
				{
					if (simpleBotSettingValueToDisplay != UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue.Hard)
					{
					}
					else
					{
						this.SetDropdownText(StringUtil.TR("Hard", "Global"));
						this.m_enemyBotStars.SetCurrentValue(4);
						this.m_teamBotStars.SetCurrentValue(2);
						if (currentSpecificState.ClientRequestedSimpleBotSettingValue != null)
						{
							this.SendBotDifficultyUpdateToServer(new BotDifficulty?(BotDifficulty.Easy), new BotDifficulty?(BotDifficulty.Hard));
						}
					}
				}
				else
				{
					this.SetDropdownText(StringUtil.TR("Medium", "Global"));
					this.m_enemyBotStars.SetCurrentValue(2);
					this.m_teamBotStars.SetCurrentValue(3);
					if (currentSpecificState.ClientRequestedSimpleBotSettingValue != null)
					{
						this.SendBotDifficultyUpdateToServer(new BotDifficulty?(BotDifficulty.Medium), new BotDifficulty?(BotDifficulty.Easy));
					}
				}
			}
			else
			{
				this.SetDropdownText(StringUtil.TR("Easy", "Global"));
				this.m_enemyBotStars.SetCurrentValue(1);
				this.m_teamBotStars.SetCurrentValue(4);
				if (currentSpecificState.ClientRequestedSimpleBotSettingValue != null)
				{
					this.SendBotDifficultyUpdateToServer(new BotDifficulty?(BotDifficulty.Hard), new BotDifficulty?(BotDifficulty.Stupid));
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
			UIManager.SetGameObjectActive(this.m_simpleCogBtn, true, null);
			UIManager.SetGameObjectActive(this.m_advancedCogBtn, false, null);
			UIManager.SetGameObjectActive(this.m_dropdownBtn, false, null);
			UIManager.SetGameObjectActive(this.m_difficultyListContainer, false, null);
		}
		UIManager.SetGameObjectActive(this.m_enemyBotSkillPanel, currentSpecificState.BotSkillPanelVisible && !flag2, null);
		GameObject teamBotSkillPanel = this.m_teamBotSkillPanel;
		bool doActive;
		if (currentSpecificState.BotSkillPanelVisible)
		{
			doActive = !flag2;
		}
		else
		{
			doActive = false;
		}
		UIManager.SetGameObjectActive(teamBotSkillPanel, doActive, null);
		UIManager.SetGameObjectActive(this.m_teamBotsToggle, true, null);
	}

	public void RefreshSelectedGameType()
	{
		UICharacterScreen.CharacterSelectSceneStateParameters currentSpecificState = UICharacterScreen.GetCurrentSpecificState();
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
				UICharacterScreen.GetCurrentSpecificState().ClientRequestedGameType = null;
				this.SentInitialSubTypes = false;
				ClientGameManager.Get().GroupInfo.SelectedQueueType = value;
			}
		}
		this.UpdateWillFillVisibility();
		this.m_partyListPanel.SetVisible(false, false);
	}

	public void UpdateWillFillVisibility()
	{
		if (UICharacterSelectScreenController.Get() != null)
		{
			UICharacterScreen.CharacterSelectSceneStateParameters currentSpecificState = UICharacterScreen.GetCurrentSpecificState();
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
			UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_miscCharSelectButtons, num2 > 0, null);
			if (num2 == 0)
			{
				if (UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay.IsWillFill())
				{
					CharacterType characterType = ClientGameManager.Get().QueueRequirementApplicant.AvailableCharacters.Shuffled(new System.Random()).First<CharacterType>();
					CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(characterType);
					UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
					{
						ClientRequestToServerSelectCharacter = new CharacterType?(characterResourceLink.m_characterType)
					});
				}
			}
		}
	}

	private void DoCharButtonSelection(CharacterType charTypeToMatch)
	{
		foreach (UICharacterPanelSelectButton uicharacterPanelSelectButton in this.CharacterSelectButtons)
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
		foreach (UICharacterPanelSelectButton uicharacterPanelSelectButton in this.CharacterSelectButtons)
		{
			CharacterResourceLink characterResourceLink = uicharacterPanelSelectButton.GetCharacterResourceLink();
			if (characterResourceLink == null)
			{
			}
			else
			{
				CharacterType characterType = characterResourceLink.m_characterType;
				bool flag = true;
				if (!this.IsCharacterValidForSelection(characterType))
				{
					flag = false;
				}
				else if (characterType == UICharacterScreen.GetCurrentSpecificState().CharacterTypeToDisplay)
				{
					UICharacterSelectScreenController.Get().UpdateBuyButtons();
				}
				PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(characterType);
				bool practiceGameTypeSelectedForQueue = SceneStateParameters.PracticeGameTypeSelectedForQueue;
				bool flag2;
				if (GameManager.Get().IsCharacterAllowedForPlayers(characterType))
				{
					flag2 = GameManager.Get().IsCharacterAllowedForGameType(characterType, UICharacterScreen.GetCurrentSpecificState().GameTypeToDisplay, null, null);
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
		UICharacterScreen.CharacterSelectSceneStateParameters currentSpecificState = UICharacterScreen.GetCurrentSpecificState();
		if (currentSpecificState.ClientRequestToServerSelectCharacter != null)
		{
			CharacterType? clientRequestToServerSelectCharacter = currentSpecificState.ClientRequestToServerSelectCharacter;
			CharacterType value = clientRequestToServerSelectCharacter.Value;
			if (!this.IsCharacterValidForSelection(value))
			{
				if (SceneStateParameters.IsInGameLobby)
				{
					UICharacterScreen.GetCurrentSpecificState().ClientRequestToServerSelectCharacter = null;
					this.DoCharButtonSelection(currentSpecificState.CharacterTypeToDisplay);
					goto IL_85;
				}
			}
			this.DoCharButtonSelection(value);
			IL_85:;
		}
		else
		{
			this.DoCharButtonSelection(currentSpecificState.CharacterTypeToDisplay);
		}
	}

	public void RefreshCharacterButtonsVisibility()
	{
		UICharacterScreen.CharacterSelectSceneStateParameters currentSpecificState = UICharacterScreen.GetCurrentSpecificState();
		bool? characterSelectButtonsVisible = currentSpecificState.CharacterSelectButtonsVisible;
		bool value = characterSelectButtonsVisible.Value;
		if (value)
		{
			UIManager.SetGameObjectActive(this.m_characterSelectAnimController, true, null);
			UIAnimationEventManager.Get().PlayAnimation(this.m_characterSelectAnimController, "CharacterSelectionIN", null, string.Empty, 0, 0f, true, true, null, null);
			UICharacterSelectScreenController.Get().m_charSettingsPanel.SetVisible(false, UICharacterSelectCharacterSettingsPanel.TabPanel.None);
			UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectOpen);
			UINewUserFlowManager.OnCharacterSelectDisplayed();
			UICharacterSelectWorldObjects.Get().PlayCameraAnimation("CamCloseupIN");
		}
		else
		{
			if (this.m_characterSelectAnimController.gameObject.activeSelf)
			{
				UIAnimationEventManager.Get().PlayAnimation(this.m_characterSelectAnimController, "CharacterSelectionOUT", null, string.Empty, 0, 0f, true, true, null, null);
			}
			UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectClose);
			UICharacterSelectWorldObjects.Get().PlayCameraAnimation("CamCloseupOUT");
		}
	}

	public void SendRequestToServerForCharacterSelect()
	{
		UICharacterScreen.CharacterSelectSceneStateParameters currentSpecificState = UICharacterScreen.GetCurrentSpecificState();
		CharacterType characterType = CharacterType.None;
		if (currentSpecificState.ClientRequestToServerSelectCharacter != null)
		{
			CharacterType? clientRequestToServerSelectCharacter = currentSpecificState.ClientRequestToServerSelectCharacter;
			characterType = clientRequestToServerSelectCharacter.Value;
		}
		if (characterType != CharacterType.None)
		{
			if (!this.IsCharacterValidForSelection(characterType))
			{
				if (SceneStateParameters.IsInGameLobby)
				{
					goto IL_136;
				}
			}
			if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
			{
				ClientGameManager.Get().UpdateSelectedCharacter(characterType, 0);
			}
			else if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
			{
				ClientGameManager.Get().UpdateSelectedCharacter(characterType, 0);
			}
			else
			{
				if (UILandingPageScreen.Get() != null && UILandingPageScreen.Get().CharacterInfoClicked != null)
				{
					if (UILandingPageScreen.Get().CharacterInfoClicked.Value == characterType)
					{
						ClientGameManager.Get().UpdateSelectedCharacter(characterType, 0);
						goto IL_134;
					}
				}
				UICharacterScreen.GetCurrentSpecificState().ClientRequestToServerSelectCharacter = null;
			}
			IL_134:
			return;
		}
		IL_136:
		UICharacterScreen.GetCurrentSpecificState().ClientRequestToServerSelectCharacter = null;
	}

	public void RefreshSideButtonsVisibility()
	{
		UICharacterScreen.CharacterSelectSceneStateParameters currentSpecificState = UICharacterScreen.GetCurrentSpecificState();
		bool sideButtonsVisibility = currentSpecificState.SideButtonsVisibility;
		UIManager.SetGameObjectActive(this.m_sideBtnContainer, sideButtonsVisibility, null);
		if (sideButtonsVisibility)
		{
			if (currentSpecificState.ClientSelectedCharacter != null)
			{
				CharacterType? clientSelectedCharacter = currentSpecificState.ClientSelectedCharacter;
				if (clientSelectedCharacter.Value.IsWillFill())
				{
					UIManager.SetGameObjectActive(this.m_bioBtn, false, null);
					UIManager.SetGameObjectActive(this.m_skinsBtn, true, null);
					UIManager.SetGameObjectActive(this.m_AbilitiesBtn, false, null);
					UIManager.SetGameObjectActive(this.m_CatalystBtn, false, null);
					UIManager.SetGameObjectActive(this.m_TauntsBtn, false, null);
					return;
				}
			}
			UIManager.SetGameObjectActive(this.m_bioBtn, true, null);
			UIManager.SetGameObjectActive(this.m_skinsBtn, true, null);
			UIManager.SetGameObjectActive(this.m_AbilitiesBtn, true, null);
			UIManager.SetGameObjectActive(this.m_CatalystBtn, true, null);
			UIManager.SetGameObjectActive(this.m_TauntsBtn, true, null);
		}
	}

	public void RefreshSideButtonsClickability()
	{
		UICharacterScreen.CharacterSelectSceneStateParameters currentSpecificState = UICharacterScreen.GetCurrentSpecificState();
		bool? sideButtonsClickable = currentSpecificState.SideButtonsClickable;
		bool value = sideButtonsClickable.Value;
		this.m_bioBtn.spriteController.SetClickable(value);
		this.m_skinsBtn.spriteController.SetClickable(value);
		this.m_AbilitiesBtn.spriteController.SetClickable(value);
		this.m_CatalystBtn.spriteController.SetClickable(value);
		this.m_TauntsBtn.spriteController.SetClickable(value);
		if (!value)
		{
			this.m_bioBtn.spriteController.ResetMouseState();
			this.m_skinsBtn.spriteController.ResetMouseState();
			this.m_AbilitiesBtn.spriteController.ResetMouseState();
			this.m_CatalystBtn.spriteController.ResetMouseState();
			this.m_TauntsBtn.spriteController.ResetMouseState();
		}
	}

	public override SceneType GetSceneType()
	{
		return SceneType.CharacterSelect;
	}

	public static bool IsGameSubTypeActive(GameType gameType, GameSubType gst)
	{
		if (!gst.Requirements.IsNullOrEmpty<QueueRequirement>())
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

		public UICharacterScreen.CharacterSelectSceneStateParameters.BotDifficultyViewType? BotDifficultyView;

		public UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue? SimpleBotSetting;

		public CharacterType? ClientSelectedCharacter;

		public CharacterVisualInfo? ClientSelectedVisualInfo;

		public UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue? ClientRequestedSimpleBotSettingValue;

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
				if (SceneStateParameters.IsHUDHidden)
				{
					return false;
				}
				if (this.CustomGamePartyIsVisible)
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
				using (Dictionary<ushort, GameSubType>.ValueCollection.Enumerator enumerator = this.SelectedGameSubTypes.Values.GetEnumerator())
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
				if (this.CustomGamePartyListVisible != null)
				{
					if (this.CustomGamePartyListVisible.Value)
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
				if (this.CustomGamePartyListHidden != null)
				{
					return this.CustomGamePartyListHidden.Value;
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
				if (this.SideButtonsVisible != null)
				{
					return this.SideButtonsVisible.Value;
				}
				return false;
			}
		}

		public int AllyBotDifficultyToDisplay
		{
			get
			{
				if (this.BotDifficultyViewTypeToDisplay == UICharacterScreen.CharacterSelectSceneStateParameters.BotDifficultyViewType.Advanced)
				{
					if (this.ClientRequestedAllyBotDifficulty != null)
					{
						return this.ClientRequestedAllyBotDifficulty.Value;
					}
					if (this.SelectedAllyBotDifficulty != null)
					{
						return this.SelectedAllyBotDifficulty.Value;
					}
				}
				else
				{
					if (this.SimpleBotSettingValueToDisplay == UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue.Easy)
					{
						return 3;
					}
					if (this.SimpleBotSettingValueToDisplay == UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue.Medium)
					{
						return 2;
					}
					if (this.SimpleBotSettingValueToDisplay == UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue.Hard)
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
				if (this.BotDifficultyViewTypeToDisplay == UICharacterScreen.CharacterSelectSceneStateParameters.BotDifficultyViewType.Advanced)
				{
					if (this.ClientRequestedEnemyBotDifficulty != null)
					{
						return this.ClientRequestedEnemyBotDifficulty.Value;
					}
					if (this.SelectedEnemyBotDifficulty != null)
					{
						return this.SelectedEnemyBotDifficulty.Value;
					}
				}
				else
				{
					if (this.SimpleBotSettingValueToDisplay == UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue.Easy)
					{
						return 0;
					}
					if (this.SimpleBotSettingValueToDisplay == UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue.Medium)
					{
						return 1;
					}
					if (this.SimpleBotSettingValueToDisplay == UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue.Hard)
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
				Dictionary<ushort, GameSubType> validGameSubTypes = this.ValidGameSubTypes;
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
				if (this.GameTypeToDisplay == GameType.Coop)
				{
					Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(this.GameTypeToDisplay);
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
					if (this.DisplayAllyBotTeammates)
					{
						result = dictionary2;
					}
					else
					{
						result = dictionary;
					}
					return result;
				}
				return ClientGameManager.Get().GetGameTypeSubTypes(this.GameTypeToDisplay);
			}
		}

		public bool GameSubTypesVisible
		{
			get
			{
				if (this.GameTypeToDisplay == GameType.Custom)
				{
					return false;
				}
				bool result;
				if (!this.ValidGameSubTypes.IsNullOrEmpty<KeyValuePair<ushort, GameSubType>>())
				{
					result = (this.ValidGameSubTypes.Count > 1);
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
				foreach (KeyValuePair<ushort, GameSubType> keyValuePair in this.ValidGameSubTypes)
				{
					ushort? selectedSubTypeMask = this.SelectedSubTypeMask;
					int? num;
					if (selectedSubTypeMask != null)
					{
						num = new int?((int)selectedSubTypeMask.Value);
					}
					else
					{
						num = null;
					}
					int? num2 = num;
					int? num3;
					if (num2 != null)
					{
						num3 = new int?((int)keyValuePair.Key & num2.GetValueOrDefault());
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
				if (this.ClientRequestAllyBotTeammates != null)
				{
					return this.ClientRequestAllyBotTeammates.Value;
				}
				return this.AllyBotTeammatesSelected != null && this.AllyBotTeammatesSelected.Value;
			}
		}

		public UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue SimpleBotSettingValueToDisplay
		{
			get
			{
				if (this.ClientRequestedSimpleBotSettingValue != null)
				{
					return this.ClientRequestedSimpleBotSettingValue.Value;
				}
				if (this.SimpleBotSetting != null)
				{
					return this.SimpleBotSetting.Value;
				}
				return UICharacterScreen.CharacterSelectSceneStateParameters.SimpleBotSettingValue.Easy;
			}
		}

		public UICharacterScreen.CharacterSelectSceneStateParameters.BotDifficultyViewType BotDifficultyViewTypeToDisplay
		{
			get
			{
				if (!SceneStateParameters.IsInGameLobby)
				{
					if (!SceneStateParameters.IsInQueue)
					{
						if (SceneStateParameters.IsGroupSubordinate)
						{
							return UICharacterScreen.CharacterSelectSceneStateParameters.BotDifficultyViewType.Advanced;
						}
					}
				}
				if (this.BotDifficultyView != null)
				{
					UICharacterScreen.CharacterSelectSceneStateParameters.BotDifficultyViewType? botDifficultyView = this.BotDifficultyView;
					return botDifficultyView.Value;
				}
				return UICharacterScreen.CharacterSelectSceneStateParameters.BotDifficultyViewType.Simple;
			}
		}

		public bool BotSkillPanelVisible
		{
			get
			{
				return this.GameTypeToDisplay == GameType.Solo || this.GameTypeToDisplay == GameType.Coop;
			}
		}

		public GameType GameTypeToDisplay
		{
			get
			{
				GameType gameType;
				if (this.ClientRequestedGameType != null)
				{
					GameType? clientRequestedGameType = this.ClientRequestedGameType;
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
					if (SceneStateParameters.IsInGameLobby && lobbyGameConfig2 != null)
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
				if (this.CharacterTypeToDisplay.IsValidForHumanPreGameSelection())
				{
					return GameWideData.Get().GetCharacterResourceLink(this.CharacterTypeToDisplay);
				}
				return null;
			}
		}

		public CharacterType CharacterTypeToDisplay
		{
			get
			{
				if (AppState.GetCurrent() == AppState_CharacterSelect.Get())
				{
					if (SceneStateParameters.SelectedCharacterFromGameInfo.IsValidForHumanPreGameSelection())
					{
						return SceneStateParameters.SelectedCharacterFromGameInfo;
					}
				}
				if (this.ClientRequestToServerSelectCharacter != null)
				{
					CharacterType? clientRequestToServerSelectCharacter = this.ClientRequestToServerSelectCharacter;
					return clientRequestToServerSelectCharacter.Value;
				}
				if (SceneStateParameters.SelectedCharacterInGroup.IsValidForHumanPreGameSelection())
				{
					return SceneStateParameters.SelectedCharacterInGroup;
				}
				if (this.ClientSelectedCharacter != null)
				{
					CharacterType? clientSelectedCharacter = this.ClientSelectedCharacter;
					return clientSelectedCharacter.Value;
				}
				return SceneStateParameters.SelectedCharacterFromPlayerData;
			}
		}

		public CharacterVisualInfo CharacterVisualInfoToDisplay
		{
			get
			{
				if (this.ClientSelectedVisualInfo != null)
				{
					return this.ClientSelectedVisualInfo.Value;
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
