using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UICharacterScreen : UIScene
{
	public class CharacterSelectSceneStateParameters : SceneStateParameters
	{
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
				if (SceneStateParameters.IsHUDHidden)
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
						if (gameManager.GameStatus != 0)
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
						GameSubType current = enumerator.Current;
						if (current.HasMod(GameSubType.SubTypeMods.ControlAllBots))
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									return true;
								}
							}
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
				if (CustomGamePartyListVisible.HasValue)
				{
					if (CustomGamePartyListVisible.Value)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								return true;
							}
						}
					}
				}
				return false;
			}
		}

		public bool CustomGamePartyIsHidden
		{
			get
			{
				if (CustomGamePartyListHidden.HasValue)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return CustomGamePartyListHidden.Value;
						}
					}
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
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								return false;
							}
						}
					}
				}
				if (SideButtonsVisible.HasValue)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return SideButtonsVisible.Value;
						}
					}
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
					if (ClientRequestedAllyBotDifficulty.HasValue)
					{
						while (true)
						{
							return ClientRequestedAllyBotDifficulty.Value;
						}
					}
					if (SelectedAllyBotDifficulty.HasValue)
					{
						return SelectedAllyBotDifficulty.Value;
					}
				}
				else
				{
					if (SimpleBotSettingValueToDisplay == SimpleBotSettingValue.Easy)
					{
						while (true)
						{
							return 3;
						}
					}
					if (SimpleBotSettingValueToDisplay == SimpleBotSettingValue.Medium)
					{
						while (true)
						{
							return 2;
						}
					}
					if (SimpleBotSettingValueToDisplay == SimpleBotSettingValue.Hard)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								return 1;
							}
						}
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
					if (ClientRequestedEnemyBotDifficulty.HasValue)
					{
						return ClientRequestedEnemyBotDifficulty.Value;
					}
					if (SelectedEnemyBotDifficulty.HasValue)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								break;
							default:
								return SelectedEnemyBotDifficulty.Value;
							}
						}
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
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								return 3;
							}
						}
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
						KeyValuePair<ushort, GameSubType> current = enumerator.Current;
						if (current.Value.HasMod(GameSubType.SubTypeMods.Exclusive))
						{
							num = (ushort)(num | current.Key);
						}
					}
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return num;
						}
					}
				}
			}
		}

		public Dictionary<ushort, GameSubType> ValidGameSubTypes
		{
			get
			{
				if (GameTypeToDisplay == GameType.Coop)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
						{
							Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(GameTypeToDisplay);
							Dictionary<ushort, GameSubType> dictionary = new Dictionary<ushort, GameSubType>();
							Dictionary<ushort, GameSubType> dictionary2 = new Dictionary<ushort, GameSubType>();
							using (Dictionary<ushort, GameSubType>.Enumerator enumerator = gameTypeSubTypes.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									KeyValuePair<ushort, GameSubType> current = enumerator.Current;
									if (current.Value.HasMod(GameSubType.SubTypeMods.ShowWithAITeammates))
									{
										dictionary2[current.Key] = current.Value;
									}
									else
									{
										dictionary[current.Key] = current.Value;
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
						}
					}
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
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
				int result;
				if (!ValidGameSubTypes.IsNullOrEmpty())
				{
					result = ((ValidGameSubTypes.Count > 1) ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				return (byte)result != 0;
			}
		}

		public Dictionary<ushort, GameSubType> SelectedGameSubTypes
		{
			get
			{
				Dictionary<ushort, GameSubType> dictionary = new Dictionary<ushort, GameSubType>();
				foreach (KeyValuePair<ushort, GameSubType> validGameSubType in ValidGameSubTypes)
				{
					ushort? selectedSubTypeMask = SelectedSubTypeMask;
					int? obj;
					if (selectedSubTypeMask.HasValue)
					{
						obj = selectedSubTypeMask.Value;
					}
					else
					{
						obj = null;
					}
					int? num = obj;
					int? obj2;
					if (num.HasValue)
					{
						obj2 = (validGameSubType.Key & num.GetValueOrDefault());
					}
					else
					{
						obj2 = null;
					}
					int? num2 = obj2;
					int num3;
					if (num2.GetValueOrDefault() == 0)
					{
						num3 = ((!num2.HasValue) ? 1 : 0);
					}
					else
					{
						num3 = 1;
					}
					if (num3 != 0)
					{
						dictionary[validGameSubType.Key] = validGameSubType.Value;
					}
				}
				return dictionary;
			}
		}

		public bool DisplayAllyBotTeammates
		{
			get
			{
				if (ClientRequestAllyBotTeammates.HasValue)
				{
					return ClientRequestAllyBotTeammates.Value;
				}
				if (AllyBotTeammatesSelected.HasValue)
				{
					return AllyBotTeammatesSelected.Value;
				}
				return false;
			}
		}

		public SimpleBotSettingValue SimpleBotSettingValueToDisplay
		{
			get
			{
				if (ClientRequestedSimpleBotSettingValue.HasValue)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							return ClientRequestedSimpleBotSettingValue.Value;
						}
					}
				}
				if (SimpleBotSetting.HasValue)
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
				if (!SceneStateParameters.IsInGameLobby)
				{
					if (!SceneStateParameters.IsInQueue)
					{
						if (SceneStateParameters.IsGroupSubordinate)
						{
							return BotDifficultyViewType.Advanced;
						}
					}
				}
				if (BotDifficultyView.HasValue)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
						{
							BotDifficultyViewType? botDifficultyView = BotDifficultyView;
							return botDifficultyView.Value;
						}
						}
					}
				}
				return BotDifficultyViewType.Simple;
			}
		}

		public bool BotSkillPanelVisible => GameTypeToDisplay == GameType.Solo || GameTypeToDisplay == GameType.Coop;

		public GameType GameTypeToDisplay
		{
			get
			{
				GameType gameType;
				if (ClientRequestedGameType.HasValue)
				{
					GameType? clientRequestedGameType = ClientRequestedGameType;
					gameType = clientRequestedGameType.Value;
				}
				else
				{
					object obj;
					if (GameManager.Get() != null)
					{
						obj = GameManager.Get().GameConfig;
					}
					else
					{
						obj = null;
					}
					LobbyGameConfig lobbyGameConfig = (LobbyGameConfig)obj;
					if (SceneStateParameters.IsInGameLobby && lobbyGameConfig != null)
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
				if (blockedExperienceAlternativeGameType != GameType.None)
				{
					if (clientGameManager.GetPlayerAccountData().ExperienceComponent.Matches < clientGameManager.NewPlayerPvPQueueDuration)
					{
						if (blockedExperienceEntries != null)
						{
							if (blockedExperienceEntries.Contains(MatchmakingQueueConfig.QueueEntryExperience.NewPlayer))
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
										return blockedExperienceAlternativeGameType;
									}
								}
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
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return GameWideData.Get().GetCharacterResourceLink(CharacterTypeToDisplay);
						}
					}
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
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								return SceneStateParameters.SelectedCharacterFromGameInfo;
							}
						}
					}
				}
				if (ClientRequestToServerSelectCharacter.HasValue)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
						{
							CharacterType? clientRequestToServerSelectCharacter = ClientRequestToServerSelectCharacter;
							return clientRequestToServerSelectCharacter.Value;
						}
						}
					}
				}
				if (SceneStateParameters.SelectedCharacterInGroup.IsValidForHumanPreGameSelection())
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							return SceneStateParameters.SelectedCharacterInGroup;
						}
					}
				}
				if (ClientSelectedCharacter.HasValue)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
						{
							CharacterType? clientSelectedCharacter = ClientSelectedCharacter;
							return clientSelectedCharacter.Value;
						}
						}
					}
				}
				return SceneStateParameters.SelectedCharacterFromPlayerData;
			}
		}

		public CharacterVisualInfo CharacterVisualInfoToDisplay
		{
			get
			{
				if (ClientSelectedVisualInfo.HasValue)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return ClientSelectedVisualInfo.Value;
						}
					}
				}
				if (ClientGameManager.Get() != null)
				{
					if (ClientGameManager.Get().GroupInfo != null)
					{
						if (ClientGameManager.Get().GroupInfo.ChararacterInfo != null)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									return ClientGameManager.Get().GroupInfo.ChararacterInfo.CharacterSkin;
								}
							}
						}
					}
				}
				return default(CharacterVisualInfo);
			}
		}
	}

	public enum RefreshFunctionType
	{
		RefreshSideButtonVisibility = 1,
		RefreshSideButtonClickability = 2,
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
		RebuildCalls[1] = RefreshSideButtonsVisibility;
		RebuildCalls[2] = RefreshSideButtonsClickability;
		RebuildCalls[4] = RefreshSelectedCharacterButton;
		RebuildCalls[8] = SendRequestToServerForCharacterSelect;
		RebuildCalls[16] = RefreshCharacterButtonsVisibility;
		RebuildCalls[32] = RefreshSelectedGameType;
		RebuildCalls[64] = RefreshCharacterButtons;
		RebuildCalls[128] = RefreshBotSkillPanel;
		RebuildCalls[256] = RefreshGameSubTypes;
		RebuildCalls[512] = RefreshPartyList;
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
		if (!(ClientGameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= OnLobbyGameplayOverridesUpdated;
			ClientGameManager.Get().OnGroupUpdateNotification -= RefreshGameSubTypes;
			return;
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
		List<CharacterType> list = new List<CharacterType>();
		list.AddRange((CharacterType[])Enum.GetValues(typeof(CharacterType)));
		m_filterButtons.Add(m_notOnAFactionFilter);
		List<FactionGroup> list2 = FactionWideData.Get().FactionGroupsToDisplayFilter();
		for (int i = 0; i < list2.Count; i++)
		{
			FactionGroup groupFilter = list2[i];
			UICharacterSelectFactionFilter uICharacterSelectFactionFilter = UnityEngine.Object.Instantiate(m_factionFilterPrefab);
			uICharacterSelectFactionFilter.transform.SetParent(m_searchFiltersContainer.transform);
			uICharacterSelectFactionFilter.transform.localPosition = Vector3.zero;
			uICharacterSelectFactionFilter.transform.localScale = Vector3.one;
			uICharacterSelectFactionFilter.Setup(groupFilter, ClickedOnFactionFilter);
			m_filterButtons.Add(uICharacterSelectFactionFilter);
			if (groupFilter.Characters != null)
			{
				list = list.Except(groupFilter.Characters).ToList();
			}
			uICharacterSelectFactionFilter.m_btn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
			{
				(tooltip as UISimpleTooltip).Setup(FactionGroup.GetDisplayName(groupFilter.FactionGroupID));
				return true;
			});
		}
		while (true)
		{
			m_notOnAFactionFilter.Setup(list, ClickedOnFactionFilter);
			UITooltipHoverObject component = m_notOnAFactionFilter.m_btn.spriteController.GetComponent<UITooltipHoverObject>();
			
			component.Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
				{
					(tooltip as UISimpleTooltip).Setup(StringUtil.TR("Wildcard", "Global"));
					return true;
				});
			return;
		}
	}

	private void SetupCharacterButtons()
	{
		using (List<UICharacterPanelSelectButton>.Enumerator enumerator = CharacterSelectButtons.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				UICharacterPanelSelectButton current = enumerator.Current;
				if (!m_SelectWillFillBtns.Contains(current))
				{
					UnityEngine.Object.Destroy(current.gameObject);
				}
			}
		}
		CharacterSelectButtons.Clear();
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
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							if (array[i] != CharacterType.TestFreelancer2)
							{
								while (true)
								{
									CharacterResourceLink characterResourceLink;
									switch (5)
									{
									case 0:
										break;
									default:
										{
											if (array[i] == CharacterType.None)
											{
											}
											else
											{
												characterResourceLink = GameWideData.Get().GetCharacterResourceLink(array[i]);
												if (flag)
												{
													goto IL_0131;
												}
												if (!characterResourceLink.m_isHidden)
												{
													goto IL_0131;
												}
											}
											goto end_IL_00ce;
										}
										IL_0131:
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
											while (true)
											{
												switch (1)
												{
												case 0:
													break;
												default:
													list2.Add(array[i]);
													goto end_IL_00ce;
												}
											}
										}
										goto end_IL_00ce;
									}
								}
							}
							goto end_IL_00ce;
						}
					}
				}
				end_IL_00ce:;
			}
			catch
			{
			}
		}
		while (true)
		{
			list.Sort(CompareCharacterTypeName);
			list2.Sort(CompareCharacterTypeName);
			list3.Sort(CompareCharacterTypeName);
			int num = Mathf.CeilToInt((float)list.Count / 2f);
			int num2 = Mathf.CeilToInt((float)list2.Count / 2f);
			int num3 = Mathf.CeilToInt((float)list3.Count / 2f);
			int num4 = 0;
			for (int j = 0; j < list.Count; j++)
			{
				UICharacterPanelSelectButton uICharacterPanelSelectButton = UnityEngine.Object.Instantiate(m_CharacterButtonSelectPrefab);
				uICharacterPanelSelectButton.m_characterType = list[j];
				if (j - num4 * num >= num)
				{
					num4++;
				}
				UIManager.ReparentTransform(uICharacterPanelSelectButton.gameObject.transform, m_FirepowerRows[num4].gameObject.transform);
				CharacterSelectButtons.Add(uICharacterPanelSelectButton);
			}
			while (true)
			{
				num4 = 0;
				for (int k = 0; k < list2.Count; k++)
				{
					UICharacterPanelSelectButton uICharacterPanelSelectButton2 = UnityEngine.Object.Instantiate(m_CharacterButtonSelectPrefab);
					uICharacterPanelSelectButton2.m_characterType = list2[k];
					if (k - num4 * num2 >= num2)
					{
						num4++;
					}
					UIManager.ReparentTransform(uICharacterPanelSelectButton2.gameObject.transform, m_FrontlineRows[num4].gameObject.transform);
					CharacterSelectButtons.Add(uICharacterPanelSelectButton2);
				}
				while (true)
				{
					num4 = 0;
					for (int l = 0; l < list3.Count; l++)
					{
						UICharacterPanelSelectButton uICharacterPanelSelectButton3 = UnityEngine.Object.Instantiate(m_CharacterButtonSelectPrefab);
						uICharacterPanelSelectButton3.m_characterType = list3[l];
						if (l - num4 * num3 >= num3)
						{
							num4++;
						}
						UIManager.ReparentTransform(uICharacterPanelSelectButton3.gameObject.transform, m_SupportRows[num4].gameObject.transform);
						CharacterSelectButtons.Add(uICharacterPanelSelectButton3);
					}
					while (true)
					{
						if (!m_SelectWillFillBtns.IsNullOrEmpty())
						{
							UICharacterPanelSelectButton[] selectWillFillBtns = m_SelectWillFillBtns;
							foreach (UICharacterPanelSelectButton item in selectWillFillBtns)
							{
								CharacterSelectButtons.Add(item);
							}
						}
						foreach (UICharacterPanelSelectButton characterSelectButton in CharacterSelectButtons)
						{
							characterSelectButton.Setup(false);
						}
						return;
					}
				}
			}
		}
	}

	private int CompareCharacterTypeName(CharacterType CharA, CharacterType CharB)
	{
		return CharA.GetDisplayName().CompareTo(CharB.GetDisplayName());
	}

	public override SceneStateParameters GetCurrentState()
	{
		if (m_currentState == null)
		{
			m_currentState = new CharacterSelectSceneStateParameters();
		}
		return m_currentState;
	}

	public static CharacterSelectSceneStateParameters GetCurrentSpecificState()
	{
		if (m_currentState == null)
		{
			m_currentState = new CharacterSelectSceneStateParameters();
		}
		return m_currentState;
	}

	public override bool DoesHandleParameter(SceneStateParameters parameters)
	{
		return parameters is CharacterSelectSceneStateParameters;
	}

	public void DoRefreshFunctions(ushort RefreshBits)
	{
		using (Dictionary<int, RebuildDelegate>.Enumerator enumerator = RebuildCalls.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<int, RebuildDelegate> current = enumerator.Current;
				if ((current.Key & RefreshBits) != 0)
				{
					current.Value();
				}
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public override void HandleNewSceneStateParameter(SceneStateParameters parameters)
	{
		CharacterSelectSceneStateParameters characterSelectSceneStateParameters = parameters as CharacterSelectSceneStateParameters;
		ushort num = 0;
		if (characterSelectSceneStateParameters != null)
		{
			if (characterSelectSceneStateParameters.ClientSelectedCharacter.HasValue)
			{
				m_currentState.ClientSelectedCharacter = characterSelectSceneStateParameters.ClientSelectedCharacter;
				num = (ushort)(num | 1);
				num = (ushort)(num | 4);
				num = (ushort)(num | 0x40);
			}
			if (characterSelectSceneStateParameters.SideButtonsVisible.HasValue)
			{
				m_currentState.SideButtonsVisible = characterSelectSceneStateParameters.SideButtonsVisible;
				num = (ushort)(num | 1);
			}
			if (characterSelectSceneStateParameters.SideButtonsClickable.HasValue)
			{
				m_currentState.SideButtonsClickable = characterSelectSceneStateParameters.SideButtonsClickable;
				num = (ushort)(num | 2);
			}
			if (characterSelectSceneStateParameters.ClientSelectedVisualInfo.HasValue)
			{
				m_currentState.ClientSelectedVisualInfo = characterSelectSceneStateParameters.ClientSelectedVisualInfo;
			}
			if (characterSelectSceneStateParameters.ClientRequestToServerSelectCharacter.HasValue)
			{
				m_currentState.ClientRequestToServerSelectCharacter = characterSelectSceneStateParameters.ClientRequestToServerSelectCharacter;
				num = (ushort)(num | 4);
				num = (ushort)(num | 8);
			}
			if (characterSelectSceneStateParameters.CharacterSelectButtonsVisible.HasValue)
			{
				m_currentState.CharacterSelectButtonsVisible = characterSelectSceneStateParameters.CharacterSelectButtonsVisible;
				num = (ushort)(num | 0x10);
			}
			if (characterSelectSceneStateParameters.ClientRequestedGameType.HasValue)
			{
				m_currentState.ClientRequestedGameType = characterSelectSceneStateParameters.ClientRequestedGameType;
				num = (ushort)(num | 0x20);
				num = (ushort)(num | 0x40);
				num = (ushort)(num | 0x80);
				num = (ushort)(num | 0x100);
			}
			if (characterSelectSceneStateParameters.BotDifficultyView.HasValue)
			{
				m_currentState.BotDifficultyView = characterSelectSceneStateParameters.BotDifficultyView;
				num = (ushort)(num | 0x80);
			}
			if (characterSelectSceneStateParameters.AllyBotTeammatesSelected.HasValue)
			{
				m_currentState.AllyBotTeammatesSelected = characterSelectSceneStateParameters.AllyBotTeammatesSelected;
				num = (ushort)(num | 0x80);
				num = (ushort)(num | 0x100);
			}
			if (characterSelectSceneStateParameters.AllyBotTeammatesClickable.HasValue)
			{
				m_currentState.AllyBotTeammatesClickable = characterSelectSceneStateParameters.AllyBotTeammatesClickable;
				num = (ushort)(num | 0x80);
			}
			if (characterSelectSceneStateParameters.BotsCanTauntCheckboxEnabled.HasValue)
			{
				m_currentState.BotsCanTauntCheckboxEnabled = characterSelectSceneStateParameters.BotsCanTauntCheckboxEnabled;
				num = (ushort)(num | 0x80);
			}
			if (characterSelectSceneStateParameters.SimpleBotSetting.HasValue)
			{
				m_currentState.SimpleBotSetting = characterSelectSceneStateParameters.SimpleBotSetting;
				num = (ushort)(num | 0x80);
			}
			if (characterSelectSceneStateParameters.ClientRequestedSimpleBotSettingValue.HasValue)
			{
				m_currentState.ClientRequestedSimpleBotSettingValue = characterSelectSceneStateParameters.ClientRequestedSimpleBotSettingValue;
				num = (ushort)(num | 0x80);
			}
			if (characterSelectSceneStateParameters.ClientRequestAllyBotTeammates.HasValue)
			{
				m_currentState.ClientRequestAllyBotTeammates = characterSelectSceneStateParameters.ClientRequestAllyBotTeammates;
				num = (ushort)(num | 0x80);
				num = (ushort)(num | 0x100);
			}
			if (characterSelectSceneStateParameters.SelectedAllyBotDifficulty.HasValue)
			{
				m_currentState.SelectedAllyBotDifficulty = characterSelectSceneStateParameters.SelectedAllyBotDifficulty;
				num = (ushort)(num | 0x80);
			}
			if (characterSelectSceneStateParameters.SelectedEnemyBotDifficulty.HasValue)
			{
				m_currentState.SelectedEnemyBotDifficulty = characterSelectSceneStateParameters.SelectedEnemyBotDifficulty;
				num = (ushort)(num | 0x80);
			}
			if (characterSelectSceneStateParameters.ClientRequestedAllyBotDifficulty.HasValue)
			{
				m_currentState.ClientRequestedAllyBotDifficulty = characterSelectSceneStateParameters.ClientRequestedAllyBotDifficulty;
				num = (ushort)(num | 0x80);
			}
			if (characterSelectSceneStateParameters.ClientRequestedEnemyBotDifficulty.HasValue)
			{
				m_currentState.ClientRequestedEnemyBotDifficulty = characterSelectSceneStateParameters.ClientRequestedEnemyBotDifficulty;
				num = (ushort)(num | 0x80);
			}
			if (characterSelectSceneStateParameters.CustomGamePartyListVisible.HasValue)
			{
				m_currentState.CustomGamePartyListVisible = characterSelectSceneStateParameters.CustomGamePartyListVisible;
				num = (ushort)(num | 0x200);
			}
			if (characterSelectSceneStateParameters.CustomGamePartyListHidden.HasValue)
			{
				m_currentState.CustomGamePartyListHidden = characterSelectSceneStateParameters.CustomGamePartyListHidden;
				num = (ushort)(num | 0x200);
			}
		}
		DoRefreshFunctions(num);
		base.HandleNewSceneStateParameter(parameters);
	}

	public void UpdateSubTypeMaskChecks(ushort subTypeMask)
	{
		for (int i = 0; i < m_gameSubTypeBtns.Count; i++)
		{
			if ((m_gameSubTypeBtns[i].SubTypeBit & subTypeMask) != 0)
			{
				m_gameSubTypeBtns[i].btn.SetOn(true);
			}
			else
			{
				m_gameSubTypeBtns[i].btn.SetOn(false);
			}
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
					for (int i = 0; i < m_gameSubTypeBtns.Count; i++)
					{
						if (m_gameSubTypeBtns[i].btn.IsChecked())
						{
							newMask = (ushort)(m_gameSubTypeBtns[i].SubTypeBit | newMask);
						}
					}
				}
			}
			else
			{
				Dictionary<ushort, GameSubType> gameTypeSubTypes = ClientGameManager.Get().GetGameTypeSubTypes(GameType.Coop);
				if (!gameTypeSubTypes.IsNullOrEmpty())
				{
					foreach (KeyValuePair<ushort, GameSubType> item in gameTypeSubTypes)
					{
						bool? allyBotTeammatesSelected = Parameters.AllyBotTeammatesSelected;
						if (allyBotTeammatesSelected.GetValueOrDefault() == item.Value.HasMod(GameSubType.SubTypeMods.AntiSocial) && allyBotTeammatesSelected.HasValue)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									newMask = item.Key;
									goto end_IL_0121;
								}
							}
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
						newMask = enumerator2.Current.Key;
					}
					else
					{
					}
				}
			}
		}
		ushort exclusiveModBitsOfGameTypeToDisplay = Parameters.ExclusiveModBitsOfGameTypeToDisplay;
		ushort num = 0;
		if (exclusiveModBitsOfGameTypeToDisplay != 0)
		{
			int num2 = 0;
			while (true)
			{
				if (num2 < m_gameSubTypeBtns.Count)
				{
					if (m_gameSubTypeBtns[num2].btn.IsChecked())
					{
						if ((exclusiveModBitsOfGameTypeToDisplay | m_gameSubTypeBtns[num2].SubTypeBit) != 0)
						{
							num = m_gameSubTypeBtns[num2].SubTypeBit;
							break;
						}
					}
					num2++;
					continue;
				}
				break;
			}
			if (num != 0)
			{
				newMask = num;
			}
		}
		if (num == 0)
		{
			for (int j = 0; j < m_gameSubTypeBtns.Count; j++)
			{
				if (m_gameSubTypeBtns[j].btn.IsChecked())
				{
					newMask = (ushort)(m_gameSubTypeBtns[j].SubTypeBit | newMask);
				}
			}
		}
		ushort num3 = 0;
		for (int k = 0; k < m_gameSubTypeBtns.Count; k++)
		{
			num3 = (ushort)(m_gameSubTypeBtns[k].SubTypeBit | num3);
		}
		if (num3 != 0)
		{
			if ((newMask & num3) == 0)
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
				goto IL_04ee;
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
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
							{
								string arg;
								if (r.LocalizedFailure == null)
								{
									arg = r.ErrorMessage;
								}
								else
								{
									arg = r.LocalizedFailure.ToString();
								}
								string text2 = $"Failed to select game modes: {arg}";
								Log.Warning(text2);
								UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("Error", "Global"), text2, StringUtil.TR("Ok", "Global"));
								return;
							}
							}
						}
					}
					ClientGameManager.Get().SetSoloSubGameMask(Parameters.GameTypeToDisplay, newMask);
					UpdateSubTypeMaskChecks(newMask);
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
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
						{
							string text = $"Failed to select game modes: {((r.LocalizedFailure != null) ? r.LocalizedFailure.ToString() : r.ErrorMessage)}";
							Log.Warning(text);
							UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("Error", "Global"), text, StringUtil.TR("Ok", "Global"));
							return;
						}
						}
					}
				}
				ClientGameManager.Get().SetSoloSubGameMask(Parameters.GameTypeToDisplay, newMask);
				UpdateSubTypeMaskChecks(newMask);
			});
		}
		goto IL_04ee;
		IL_04ee:
		UpdateWillFillVisibility();
		DoRefreshFunctions(512);
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
			if (!(CharacterSelectButtons[i] != null))
			{
				continue;
			}
			if (!(CharacterSelectButtons[i].GetComponent<CanvasGroup>() != null))
			{
				continue;
			}
			CanvasGroup component = CharacterSelectButtons[i].GetComponent<CanvasGroup>();
			if (component != null)
			{
				component.alpha = 1f;
			}
		}
		while (true)
		{
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
			if (m_searchInputField.text.IsNullOrEmpty())
			{
				return;
			}
			for (int k = 0; k < CharacterSelectButtons.Count; k++)
			{
				CharacterResourceLink characterResourceLink = CharacterSelectButtons[k].GetCharacterResourceLink();
				if (!(characterResourceLink != null))
				{
					continue;
				}
				string displayName = characterResourceLink.GetDisplayName();
				if (DoesSearchMatchDisplayName(m_searchInputField.text.ToLower(), displayName.ToLower()))
				{
					continue;
				}
				CanvasGroup component3 = CharacterSelectButtons[k].GetComponent<CanvasGroup>();
				if (component3 != null)
				{
					component3.alpha = 0.3f;
				}
			}
			return;
		}
	}

	private bool DoesSearchMatchDisplayName(string searchText, string displayText)
	{
		for (int i = 0; i < searchText.Length; i++)
		{
			if (i < displayText.Length)
			{
				if (searchText[i] == displayText[i])
				{
					continue;
				}
				while (true)
				{
					return false;
				}
			}
			break;
		}
		return true;
	}

	public void SubTypeClicked(_ToggleSwap btn)
	{
		CheckSubTypeSelection(true, 0);
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
		while (true)
		{
			for (int j = 0; j < AbilitySubButtons.Length; j++)
			{
				if (AbilitySubButtons[j] != m_AbilitiesBtn.spriteController)
				{
					m_AbilitiesBtn.spriteController.AddSubButton(AbilitySubButtons[j]);
				}
			}
			while (true)
			{
				for (int k = 0; k < CatalystSubButtons.Length; k++)
				{
					if (CatalystSubButtons[k] != m_CatalystBtn.spriteController)
					{
						m_CatalystBtn.spriteController.AddSubButton(CatalystSubButtons[k]);
					}
				}
				m_skinsBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, (UITooltipBase tooltip) => SideMenuOpen(tooltip, m_skinsBtn));
				m_AbilitiesBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, (UITooltipBase tooltip) => SideMenuOpen(tooltip, m_AbilitiesBtn));
				m_CatalystBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, (UITooltipBase tooltip) => SideMenuOpen(tooltip, m_CatalystBtn));
				for (int l = 0; l < m_AbilityMouseOverBtns.Length; l++)
				{
					m_AbilityMouseOverBtns[l].spriteController.callback = ClickedAbilityIcon;
					int index = l;
					m_AbilityMouseOverBtns[l].spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, (UITooltipBase tooltip) => SetupAbilitySideBtnTooltip(tooltip, index));
				}
				for (int m = 0; m < m_AbilityModIcons.Length; m++)
				{
					UIManager.SetGameObjectActive(m_AbilityModIcons[m], false);
				}
				while (true)
				{
					for (int n = 0; n < m_CatalsytBtns.Length; n++)
					{
						m_CatalsytBtns[n].spriteController.callback = ClickedCatalystIcon;
						AbilityRunPhase phase = (AbilityRunPhase)(n + 1);
						m_CatalsytBtns[n].spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, delegate(UITooltipBase tooltip)
						{
							if (SelectedCatalysts.ContainsKey(phase))
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										break;
									default:
										return SetupCatalystSideBtnTooltip(tooltip, SelectedCatalysts[phase]);
									}
								}
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
					return;
				}
			}
		}
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
		UICharacterSelectSkinPanel uICharacterSelectSkinPanel = UICharacterSelectCharacterSettingsPanel.Get().m_skinsSubPanel.m_selectHandler as UICharacterSelectSkinPanel;
		if (uICharacterSelectSkinPanel != null)
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
						UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
						uITitledTooltip.Setup(characterResourceLink.GetDisplayName(), string.Format(StringUtil.TR("SelectedStyle", "Global"), patternColorName), string.Empty);
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
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		string text = card.GetDisplayName();
		if (!card.m_useAbility.m_flavorText.IsNullOrEmpty())
		{
			string text2 = text;
			text = text2 + Environment.NewLine + "<i>" + card.m_useAbility.m_flavorText + "</i>";
		}
		UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
		uITitledTooltip.Setup(string.Format(StringUtil.TR("CatalystTitle", "Global"), card.m_useAbility.GetPhaseString()), text, string.Empty);
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
				UIAbilityTooltip uIAbilityTooltip = (UIAbilityTooltip)tooltip;
				string movieAssetName = "Video/AbilityPreviews/" + abilityEntry.ability.m_previewVideo;
				uIAbilityTooltip.Setup(abilityEntry.ability, SelectedAbilityData[i].GetSelectedMod(), movieAssetName);
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
				Card value = enumerator.Current.Value;
				if ((AbilityRunPhase)(-1) < value.GetAbilityRunPhase() - 1)
				{
					if ((int)(value.GetAbilityRunPhase() - 1) < m_CatalystIcons.Length)
					{
						UIManager.SetGameObjectActive(m_CatalystIcons[(int)(value.GetAbilityRunPhase() - 1)], true);
						m_CatalystIcons[(int)(value.GetAbilityRunPhase() - 1)].sprite = value.GetIconSprite();
					}
				}
				if ((AbilityRunPhase)(-1) < value.GetAbilityRunPhase() - 1)
				{
					if ((int)(value.GetAbilityRunPhase() - 1) < m_CatalystHoverIcons.Length)
					{
						UIManager.SetGameObjectActive(m_CatalystHoverIcons[(int)(value.GetAbilityRunPhase() - 1)], true);
						m_CatalystHoverIcons[(int)(value.GetAbilityRunPhase() - 1)].sprite = value.GetIconSprite();
					}
				}
			}
			while (true)
			{
				switch (3)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
	}

	public void UpdateModIcons(UIAbilityButtonModPanel[] SelectedAbilities, Color prepColor, Color dashColor, Color combatColor)
	{
		SelectedAbilityData = SelectedAbilities;
		for (int i = 0; i < m_AbilityMouseOverBtns.Length; i++)
		{
			UIAbilityButtonModPanel uIAbilityButtonModPanel = SelectedAbilities[i];
			m_AbilityIcons[i].sprite = uIAbilityButtonModPanel.m_abilityIcon[0].sprite;
			UIQueueListPanel.UIPhase uIPhase = UIQueueListPanel.UIPhase.None;
			AbilityData.AbilityEntry abilityEntry = uIAbilityButtonModPanel.GetAbilityEntry();
			if (abilityEntry != null)
			{
				if (abilityEntry.ability != null)
				{
					uIPhase = UIQueueListPanel.GetUIPhaseFromAbilityPriority(abilityEntry.ability.RunPriority);
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
			if (uIPhase == UIQueueListPanel.UIPhase.Prep)
			{
				color = prepColor;
			}
			else if (uIPhase == UIQueueListPanel.UIPhase.Evasion)
			{
				color = dashColor;
			}
			else if (uIPhase == UIQueueListPanel.UIPhase.Combat)
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
			if (uIAbilityButtonModPanel.GetSelectedMod() != null)
			{
				UIManager.SetGameObjectActive(m_AbilityModIcons[i], true);
				m_AbilityModIcons[i].sprite = uIAbilityButtonModPanel.GetSelectedMod().m_iconSprite;
			}
			else
			{
				UIManager.SetGameObjectActive(m_AbilityModIcons[i], false);
				m_AbilityModIcons[i].sprite = null;
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
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
		if (num <= -1)
		{
			return;
		}
		while (true)
		{
			UICharacterSelectCharacterSettingsPanel.Get().m_abilitiesSubPanel.AbilityButtonSelected(num);
			return;
		}
	}

	private bool SideMenuOpen(UITooltipBase tooltip, _SelectableBtn btn)
	{
		if (btn == m_CatalystBtn)
		{
			if (!GameManager.Get().GameplayOverrides.EnableCards)
			{
				m_CatalystBtn.spriteController.SetClickable(false);
				m_CatalystBtn.spriteController.SetForceHovercallback(true);
				m_CatalystBtn.spriteController.SetForceExitCallback(true);
				UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
				uITitledTooltip.Setup(StringUtil.TR("Disabled", "Global"), StringUtil.TR("CatalystsAreDisabled", "Global"), string.Empty);
				return true;
			}
			m_CatalystBtn.spriteController.SetClickable(true);
			m_CatalystBtn.spriteController.SetForceHovercallback(false);
			m_CatalystBtn.spriteController.SetForceExitCallback(false);
		}
		return false;
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
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
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
		while (true)
		{
			return;
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
		while (true)
		{
			return;
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
		while (true)
		{
			switch (2)
			{
			default:
				return;
			case 0:
				break;
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
		while (true)
		{
			switch (6)
			{
			default:
				return;
			case 0:
				break;
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
		if (!response.Success)
		{
			return;
		}
		while (true)
		{
			if (response.CharacterInfo == null)
			{
				return;
			}
			while (true)
			{
				UIManager.Get().HandleNewSceneStateParameter(new CharacterSelectSceneStateParameters
				{
					ClientSelectedCharacter = response.CharacterInfo.CharacterType
				});
				UICharacterSelectScreenController uICharacterSelectScreenController = UICharacterSelectScreenController.Get();
				if (uICharacterSelectScreenController != null)
				{
					while (true)
					{
						uICharacterSelectScreenController.UpdatePrimaryCharacter(response.CharacterInfo);
						return;
					}
				}
				return;
			}
		}
	}

	private bool IsCharacterValidForSelection(CharacterType characterType)
	{
		bool flag = false;
		if (characterType == CharacterType.None)
		{
			while (true)
			{
				return flag;
			}
		}
		GameType gameTypeToDisplay = GetCurrentSpecificState().GameTypeToDisplay;
		if (GameManager.Get() != null)
		{
			if (GameManager.Get().IsValidForHumanPreGameSelection(characterType))
			{
				GameType num;
				if (GameManager.Get().GameConfig != null && GameManager.Get().GameStatus != GameStatus.Stopped)
				{
					num = GameManager.Get().GameConfig.GameType;
				}
				else
				{
					num = ClientGameManager.Get().GroupInfo.SelectedQueueType;
				}
				GameType gameType = num;
				flag = GameManager.Get().IsCharacterAllowedForGameType(characterType, gameType, null, null);
			}
		}
		if (flag)
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(characterType);
			bool flag2 = playerCharacterData != null && playerCharacterData.CharacterComponent != null && playerCharacterData.CharacterComponent.Unlocked;
			bool num2;
			if (SceneStateParameters.IsInGameLobby)
			{
				num2 = GameManager.Get().IsCharacterAllowedForPlayers(characterType);
			}
			else
			{
				num2 = GameManager.Get().IsValidForHumanPreGameSelection(characterType);
			}
			int num3;
			if (num2)
			{
				num3 = (GameManager.Get().IsCharacterAllowedForGameType(characterType, gameTypeToDisplay, null, null) ? 1 : 0);
			}
			else
			{
				num3 = 0;
			}
			bool flag3 = (byte)num3 != 0;
			bool flag4 = ClientGameManager.Get().IsCharacterAvailable(characterType, gameTypeToDisplay);
			if (flag3)
			{
				if (flag2 || flag4)
				{
					goto IL_017f;
				}
			}
			flag = false;
		}
		goto IL_017f;
		IL_017f:
		if (flag && SceneStateParameters.IsInGameLobby)
		{
			if (GameManager.Get().TeamInfo != null)
			{
				if (gameTypeToDisplay != 0)
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
					int num4 = 0;
					while (true)
					{
						if (num4 < list.Count)
						{
							if (list[num4].PlayerId != playerInfo.PlayerId)
							{
								if (list[num4].CharacterType == characterType)
								{
									if (GameManager.Get().IsFreelancerConflictPossible(list[num4].TeamId == playerInfo.TeamId))
									{
										if (!list[num4].IsNPCBot)
										{
											flag = false;
											break;
										}
									}
								}
							}
							num4++;
							continue;
						}
						break;
					}
				}
			}
		}
		return flag;
	}

	public void ReceivedGameTypeChangeResponse()
	{
		GetCurrentSpecificState().ClientRequestedGameType = null;
		DoRefreshFunctions(32);
	}

	private void SetDropdownText(string text)
	{
		TextMeshProUGUI[] componentsInChildren = m_dropdownBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].text = text;
		}
		while (true)
		{
			return;
		}
	}

	public void RefreshPartyList()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		UIManager.SetGameObjectActive(m_partyListPanel, currentSpecificState.PartyListVisbility);
		m_partyListPanel.SetVisible(currentSpecificState.PartyListVisbility);
		if ((currentSpecificState.CustomGamePartyIsVisible && !currentSpecificState.CustomGamePartyIsHidden) || !currentSpecificState.PartyListVisbility)
		{
			return;
		}
		while (true)
		{
			if (currentSpecificState.GameTypeToDisplay == GameType.Custom)
			{
				return;
			}
			while (true)
			{
				bool isDuplicateCharsAllowed = false;
				int num = -1;
				using (Dictionary<ushort, GameSubType>.ValueCollection.Enumerator enumerator = currentSpecificState.SelectedGameSubTypes.Values.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						GameSubType current = enumerator.Current;
						if (current.HasMod(GameSubType.SubTypeMods.ControlAllBots))
						{
							if (current.TeamAPlayers > num)
							{
								num = current.TeamAPlayers;
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
				return;
			}
		}
	}

	public void RefreshGameSubTypes()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		ushort num = 0;
		bool sendMaskUpdate = false;
		if (currentSpecificState.ClientRequestAllyBotTeammates.HasValue)
		{
			for (int i = 0; i < m_gameSubTypeBtns.Count; i++)
			{
				if (m_gameSubTypeBtns[i].btn.IsChecked())
				{
					num = (ushort)(num | m_gameSubTypeBtns[i].SubTypeBit);
				}
			}
			currentSpecificState.AllyBotTeammatesSelected = currentSpecificState.ClientRequestAllyBotTeammates.Value;
			sendMaskUpdate = true;
			currentSpecificState.ClientRequestAllyBotTeammates = null;
		}
		if (currentSpecificState.ClientRequestedGameType.HasValue)
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
							KeyValuePair<ushort, GameSubType> current = enumerator.Current;
							bool flag = IsGameSubTypeActive(gameTypeToDisplay, current.Value);
							if (flag && current.Value.HasMod(GameSubType.SubTypeMods.NotAllowedForGroups))
							{
								flag = !inAGroup;
							}
							if (flag && current.Value.HasMod(GameSubType.SubTypeMods.CanBeConsolidated))
							{
								num3 = (ushort)(num3 | current.Key);
								continue;
							}
							while (j >= m_gameTypeButtons.Count)
							{
								_ToggleSwap toggleSwap = UnityEngine.Object.Instantiate(m_GameTypePrefab);
								toggleSwap.transform.SetParent(m_GameTypeContainer.transform);
								toggleSwap.transform.localEulerAngles = Vector3.zero;
								toggleSwap.transform.localScale = Vector3.one;
								toggleSwap.transform.localPosition = Vector3.zero;
								m_gameTypeButtons.Add(toggleSwap);
							}
							m_gameSubTypeBtns.Add(new GameSubTypeState
							{
								btn = m_gameTypeButtons[j],
								SubTypeBit = current.Key
							});
							UIManager.SetGameObjectActive(m_gameTypeButtons[j], flag);
							TextMeshProUGUI componentInChildren = m_gameTypeButtons[j].GetComponentInChildren<TextMeshProUGUI>(true);
							componentInChildren.text = StringUtil.TR(current.Value.LocalizedName);
							m_gameTypeButtons[j].changedNotify = SubTypeClicked;
							if (flag)
							{
								if ((num2 & current.Key) != 0)
								{
									m_gameTypeButtons[j].SetOn(true);
									goto IL_03a8;
								}
							}
							m_gameTypeButtons[j].SetOn(false);
							goto IL_03a8;
							IL_03a8:
							j++;
						}
					}
					if (num3 != 0)
					{
						while (j >= m_gameTypeButtons.Count)
						{
							_ToggleSwap toggleSwap2 = UnityEngine.Object.Instantiate(m_GameTypePrefab);
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
			for (; j < m_gameTypeButtons.Count; j++)
			{
				UIManager.SetGameObjectActive(m_gameTypeButtons[j], false);
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
				goto IL_0095;
			}
		}
		if (GameManager.Get().QueueInfo != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (EnemyDifficulty.HasValue)
					{
						ClientGameManager.Get().LobbyInterface.UpdateQueueEnemyBotDifficulty(GameManager.Get().QueueInfo, EnemyDifficulty.Value);
					}
					return;
				}
			}
		}
		goto IL_0095;
		IL_0095:
		if (ClientGameManager.Get().GroupInfo != null && ClientGameManager.Get().GroupInfo.InAGroup)
		{
			if (ClientGameManager.Get().GroupInfo.IsLeader)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						ClientGameManager.Get().UpdateBotDifficulty(AllyDifficulty, EnemyDifficulty);
						return;
					}
				}
			}
		}
		if (GameManager.Get().TeamInfo == null)
		{
			return;
		}
		while (true)
		{
			using (IEnumerator<LobbyPlayerInfo> enumerator = GameManager.Get().TeamInfo.TeamBPlayerInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LobbyPlayerInfo current = enumerator.Current;
					ClientGameManager.Get().UpdateBotDifficulty(AllyDifficulty, EnemyDifficulty, current.PlayerId);
				}
				while (true)
				{
					switch (6)
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
		while (true)
		{
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
					goto IL_0217;
				}
			}
			object obj;
			if (currentSpecificState.DisplayAllyBotTeammates)
			{
				obj = "SoloEnemyDifficulty";
			}
			else
			{
				obj = "CoopDifficulty";
			}
			string key = (string)obj;
			int enemyBotDifficultyToDisplay = currentSpecificState.EnemyBotDifficultyToDisplay;
			m_enemyBotStars.SetCurrentValue(enemyBotDifficultyToDisplay + 1);
			int? clientRequestedEnemyBotDifficulty = currentSpecificState.ClientRequestedEnemyBotDifficulty;
			if (clientRequestedEnemyBotDifficulty.HasValue)
			{
				PlayerPrefs.SetInt(key, enemyBotDifficultyToDisplay);
				SendBotDifficultyUpdateToServer(null, (BotDifficulty)enemyBotDifficultyToDisplay);
				currentSpecificState.SelectedEnemyBotDifficulty = currentSpecificState.ClientRequestedEnemyBotDifficulty;
				currentSpecificState.ClientRequestedEnemyBotDifficulty = null;
			}
			int allyBotDifficultyToDisplay = currentSpecificState.AllyBotDifficultyToDisplay;
			m_teamBotStars.SetCurrentValue(allyBotDifficultyToDisplay + 1);
			int? clientRequestedAllyBotDifficulty = currentSpecificState.ClientRequestedAllyBotDifficulty;
			if (clientRequestedAllyBotDifficulty.HasValue)
			{
				PlayerPrefs.SetInt("SoloAllyDifficulty", allyBotDifficultyToDisplay);
				SendBotDifficultyUpdateToServer((BotDifficulty)allyBotDifficultyToDisplay, null);
				currentSpecificState.SelectedAllyBotDifficulty = currentSpecificState.ClientRequestedAllyBotDifficulty;
				currentSpecificState.ClientRequestedAllyBotDifficulty = null;
			}
			goto IL_0217;
			IL_0217:
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
				if (simpleBotSettingValueToDisplay != 0)
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
							if (currentSpecificState.ClientRequestedSimpleBotSettingValue.HasValue)
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
						if (currentSpecificState.ClientRequestedSimpleBotSettingValue.HasValue)
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
					if (currentSpecificState.ClientRequestedSimpleBotSettingValue.HasValue)
					{
						SendBotDifficultyUpdateToServer(BotDifficulty.Hard, BotDifficulty.Stupid);
					}
				}
				if (currentSpecificState.ClientRequestedSimpleBotSettingValue.HasValue)
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
			int doActive;
			if (currentSpecificState.BotSkillPanelVisible)
			{
				doActive = ((!flag2) ? 1 : 0);
			}
			else
			{
				doActive = 0;
			}
			UIManager.SetGameObjectActive(teamBotSkillPanel, (byte)doActive != 0);
			UIManager.SetGameObjectActive(m_teamBotsToggle, true);
			return;
		}
	}

	public void RefreshSelectedGameType()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		if (currentSpecificState.ClientRequestedGameType.HasValue)
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
		if (!(UICharacterSelectScreenController.Get() != null))
		{
			return;
		}
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		int num;
		if (ClientGameManager.Get().GameTypeAvailabilies.TryGetValue(currentSpecificState.GameTypeToDisplay, out GameTypeAvailability value))
		{
			num = value.MaxWillFillPerTeam;
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
				GameSubType current = enumerator.Current;
				if (current.HasMod(GameSubType.SubTypeMods.ControlAllBots))
				{
					num2 = 0;
				}
			}
		}
		UIManager.SetGameObjectActive(UICharacterSelectScreenController.Get().m_miscCharSelectButtons, num2 > 0);
		if (num2 != 0)
		{
			return;
		}
		while (true)
		{
			if (GetCurrentSpecificState().CharacterTypeToDisplay.IsWillFill())
			{
				while (true)
				{
					CharacterType characterType = ClientGameManager.Get().QueueRequirementApplicant.AvailableCharacters.Shuffled(new System.Random()).First();
					CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(characterType);
					UIManager.Get().HandleNewSceneStateParameter(new CharacterSelectSceneStateParameters
					{
						ClientRequestToServerSelectCharacter = characterResourceLink.m_characterType
					});
					return;
				}
			}
			return;
		}
	}

	private void DoCharButtonSelection(CharacterType charTypeToMatch)
	{
		foreach (UICharacterPanelSelectButton characterSelectButton in CharacterSelectButtons)
		{
			if (characterSelectButton.m_characterType == charTypeToMatch)
			{
				characterSelectButton.SetSelected(true);
				UICharacterSelectScreenController.Get().UpdateBuyButtons();
			}
			else
			{
				characterSelectButton.SetSelected(false);
			}
		}
	}

	public void RefreshCharacterButtons()
	{
		foreach (UICharacterPanelSelectButton characterSelectButton in CharacterSelectButtons)
		{
			CharacterResourceLink characterResourceLink = characterSelectButton.GetCharacterResourceLink();
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
				int num;
				if (GameManager.Get().IsCharacterAllowedForPlayers(characterType))
				{
					num = (GameManager.Get().IsCharacterAllowedForGameType(characterType, GetCurrentSpecificState().GameTypeToDisplay, null, null) ? 1 : 0);
				}
				else
				{
					num = 0;
				}
				bool flag2 = (byte)num != 0;
				int enabled;
				if (!flag)
				{
					if (practiceGameTypeSelectedForQueue)
					{
						enabled = (flag2 ? 1 : 0);
					}
					else
					{
						enabled = 0;
					}
				}
				else
				{
					enabled = 1;
				}
				characterSelectButton.SetEnabled((byte)enabled != 0, playerCharacterData);
				characterSelectButton.UpdateFreeRotationIcon();
			}
		}
	}

	public void RefreshSelectedCharacterButton()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		if (currentSpecificState.ClientRequestToServerSelectCharacter.HasValue)
		{
			CharacterType? clientRequestToServerSelectCharacter = currentSpecificState.ClientRequestToServerSelectCharacter;
			CharacterType value = clientRequestToServerSelectCharacter.Value;
			if (!IsCharacterValidForSelection(value))
			{
				if (SceneStateParameters.IsInGameLobby)
				{
					GetCurrentSpecificState().ClientRequestToServerSelectCharacter = null;
					DoCharButtonSelection(currentSpecificState.CharacterTypeToDisplay);
					return;
				}
			}
			DoCharButtonSelection(value);
		}
		else
		{
			DoCharButtonSelection(currentSpecificState.CharacterTypeToDisplay);
		}
	}

	public void RefreshCharacterButtonsVisibility()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		bool? characterSelectButtonsVisible = currentSpecificState.CharacterSelectButtonsVisible;
		if (characterSelectButtonsVisible.Value)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(m_characterSelectAnimController, true);
					UIAnimationEventManager.Get().PlayAnimation(m_characterSelectAnimController, "CharacterSelectionIN", null, string.Empty, 0, 0f, true, true);
					UICharacterSelectScreenController.Get().m_charSettingsPanel.SetVisible(false);
					UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectOpen);
					UINewUserFlowManager.OnCharacterSelectDisplayed();
					UICharacterSelectWorldObjects.Get().PlayCameraAnimation("CamCloseupIN");
					return;
				}
			}
		}
		if (m_characterSelectAnimController.gameObject.activeSelf)
		{
			UIAnimationEventManager.Get().PlayAnimation(m_characterSelectAnimController, "CharacterSelectionOUT", null, string.Empty, 0, 0f, true, true);
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectClose);
		UICharacterSelectWorldObjects.Get().PlayCameraAnimation("CamCloseupOUT");
	}

	public void SendRequestToServerForCharacterSelect()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		CharacterType characterType = CharacterType.None;
		if (currentSpecificState.ClientRequestToServerSelectCharacter.HasValue)
		{
			CharacterType? clientRequestToServerSelectCharacter = currentSpecificState.ClientRequestToServerSelectCharacter;
			characterType = clientRequestToServerSelectCharacter.Value;
		}
		if (characterType != 0)
		{
			if (!IsCharacterValidForSelection(characterType))
			{
				if (SceneStateParameters.IsInGameLobby)
				{
					goto IL_0136;
				}
			}
			if (AppState_GroupCharacterSelect.Get() == AppState.GetCurrent())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						ClientGameManager.Get().UpdateSelectedCharacter(characterType);
						return;
					}
				}
			}
			if (AppState_CharacterSelect.Get() == AppState.GetCurrent())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						ClientGameManager.Get().UpdateSelectedCharacter(characterType);
						return;
					}
				}
			}
			if (UILandingPageScreen.Get() != null && UILandingPageScreen.Get().CharacterInfoClicked.HasValue)
			{
				if (UILandingPageScreen.Get().CharacterInfoClicked.Value == characterType)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
							ClientGameManager.Get().UpdateSelectedCharacter(characterType);
							return;
						}
					}
				}
			}
			GetCurrentSpecificState().ClientRequestToServerSelectCharacter = null;
			return;
		}
		goto IL_0136;
		IL_0136:
		GetCurrentSpecificState().ClientRequestToServerSelectCharacter = null;
	}

	public void RefreshSideButtonsVisibility()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		bool sideButtonsVisibility = currentSpecificState.SideButtonsVisibility;
		UIManager.SetGameObjectActive(m_sideBtnContainer, sideButtonsVisibility);
		if (!sideButtonsVisibility)
		{
			return;
		}
		if (currentSpecificState.ClientSelectedCharacter.HasValue)
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

	public void RefreshSideButtonsClickability()
	{
		CharacterSelectSceneStateParameters currentSpecificState = GetCurrentSpecificState();
		bool? sideButtonsClickable = currentSpecificState.SideButtonsClickable;
		bool value = sideButtonsClickable.Value;
		m_bioBtn.spriteController.SetClickable(value);
		m_skinsBtn.spriteController.SetClickable(value);
		m_AbilitiesBtn.spriteController.SetClickable(value);
		m_CatalystBtn.spriteController.SetClickable(value);
		m_TauntsBtn.spriteController.SetClickable(value);
		if (value)
		{
			return;
		}
		while (true)
		{
			m_bioBtn.spriteController.ResetMouseState();
			m_skinsBtn.spriteController.ResetMouseState();
			m_AbilitiesBtn.spriteController.ResetMouseState();
			m_CatalystBtn.spriteController.ResetMouseState();
			m_TauntsBtn.spriteController.ResetMouseState();
			return;
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
			foreach (QueueRequirement requirement in gst.Requirements)
			{
				if (!requirement.DoesApplicantPass(clientGameManager.QueueRequirementSystemInfo, clientGameManager.QueueRequirementApplicant, gameType, gst))
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
				}
			}
		}
		return true;
	}
}
