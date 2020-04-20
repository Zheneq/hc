using System;
using System.Collections.Generic;
using System.Linq;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRankedModeDraftScreen : UIScene
{
	public RectTransform m_draftScreenContainer;

	public TextMeshProUGUI m_MessageText;

	public Color m_blueTeamColor;

	public Color m_redTeamColor;

	public Color m_neutralColor;

	[Header("Countdown Numbers")]
	public TextMeshProUGUI m_gameCountdownTimer;

	public TextMeshProUGUI m_redCountdownTimer;

	public TextMeshProUGUI m_blueCountdownTimer;

	public Animator m_gameCountdownAC;

	public Animator m_redCountdownAC;

	public Animator m_blueCountdownAC;

	public UIRankedModeDraftCharacterEntry[] m_blueBans;

	public UIRankedModeDraftCharacterEntry[] m_redBans;

	public Image m_stageImage;

	public TextMeshProUGUI m_stageText;

	public Image m_introStageImage;

	public TextMeshProUGUI m_introStageText;

	public TextMeshProUGUI m_matchFoundText;

	public RectTransform m_introContainer;

	[Header("Subphase Notifications")]
	public Animator m_blueTeamTurnNotification;

	public Animator m_redTeamTurnNotification;

	public Animator m_singleSelectionAC;

	public Animator m_doubleSelectionAC;

	public Animator m_swapPhaseAC;

	public Animator m_loadoutPhaseAC;

	public Animator m_gameLoadingAC;

	public TextMeshProUGUI m_blueTeamTurnTextNotification;

	public TextMeshProUGUI m_redTeamTurnTextNotification;

	[Header("Single Select")]
	public RectTransform[] singleSelectionBlueTeam;

	public RectTransform[] singleSelectionRedTeam;

	public Image singleNoSelectionCharacter;

	public Image singleBrowseSelectionCharacter;

	public Image singleSelectionCharacter;

	public TextMeshProUGUI m_singleCharacterName;

	public Animator m_singleSelectionCharacterSelected;

	public Animator m_singleBlueSelectionCharacterSelected;

	public Animator m_singleRedTeamSelectionCharacterSelected;

	public TextMeshProUGUI m_singleBlueTeamSelectedCharacter;

	public TextMeshProUGUI m_singleRedTeamSelectedCharacter;

	public TextMeshProUGUI m_singleBlueTeamPlayerName;

	public TextMeshProUGUI m_singleRedTeamPlayerName;

	[Header("Double Select")]
	public RectTransform[] doubleSelectionBlueTeam;

	public RectTransform[] doubleSelectionRedTeam;

	public Image doubleNoSelectionLeftCharacter;

	public Image doubleBrowseSelectionLeftCharacter;

	public Image doubleSelectionLeftCharacter;

	public Image doubleNoSelectionRightCharacter;

	public Image doubleBrowseSelectionRightCharacter;

	public Image doubleSelectionRightCharacter;

	public TextMeshProUGUI m_leftCharacterName;

	public TextMeshProUGUI m_rightCharacterName;

	public Animator m_doubleLeftSelectionCharacterSelected;

	public Animator m_doubleLeftBlueSelectionCharacterSelected;

	public Animator m_doubleLeftRedTeamSelectionCharacterSelected;

	public TextMeshProUGUI m_doubleLeftBlueTeamSelectedCharacter;

	public TextMeshProUGUI m_doubleLeftRedTeamSelectedCharacter;

	public TextMeshProUGUI m_doubleLeftBlueTeamPlayerName;

	public TextMeshProUGUI m_doubleLeftRedTeamPlayerName;

	public Animator m_doubleRightSelectionCharacterSelected;

	public Animator m_doubleRightBlueSelectionCharacterSelected;

	public Animator m_doubleRightRedTeamSelectionCharacterSelected;

	public TextMeshProUGUI m_doubleRightBlueTeamSelectedCharacter;

	public TextMeshProUGUI m_doubleRightRedTeamSelectedCharacter;

	public TextMeshProUGUI m_doubleRightBlueTeamPlayerName;

	public TextMeshProUGUI m_doubleRightRedTeamPlayerName;

	public RectTransform m_swapContainer;

	public RectTransform m_versusContainer;

	public UIRankedModePlayerDraftEntry[] m_blueTeamMembers;

	public UIRankedModePlayerDraftEntry[] m_redTeamMembers;

	public _SelectableBtn m_skinsBtn;

	public _SelectableBtn m_abilitiesBtn;

	public _SelectableBtn m_catalystsBtn;

	public _SelectableBtn m_tauntsBtn;

	public LayoutGroup m_characterSelectContainer;

	public LayoutGroup m_firePowerLayoutGroup;

	public LayoutGroup m_supportLayoutGroup;

	public LayoutGroup m_frontlinerLayoutGroup;

	public UICharacterPanelSelectButton m_characterSelectBtnPrefab;

	public HorizontalLayoutGroup m_pagesContainer;

	public _SelectableBtn m_pageBtnPrefab;

	public HorizontalLayoutGroup m_searchFiltersContainer;

	public TMP_InputField m_searchInputField;

	public UICharacterSelectFactionFilter m_factionFilterPrefab;

	public UICharacterSelectFactionFilter m_notOnAFactionFilter;

	public RectTransform m_lockFreelancerContainer;

	public _SelectableBtn m_lockFreelancerBtn;

	public _SelectableBtn m_lockInBtn;

	public TextMeshProUGUI[] m_lockInText;

	public float m_timeForPageToSwap = 900f;

	public UIRankedCharacterSelectSettingsPanel m_rankedModeCharacterSettings;

	private List<UICharacterPanelSelectRankModeButton> m_characterListDisplayButtons = new List<UICharacterPanelSelectRankModeButton>();

	private List<_SelectableBtn> m_pageButtons = new List<_SelectableBtn>();

	private int m_currentCharacterPage;

	private int m_currentVisiblePage;

	private float m_startTime;

	private float m_journeyLength;

	private Vector2 m_startLocation;

	private Vector2 m_endLocation;

	private CharacterType m_assignedCharacterForGame;

	private CharacterType m_selectedSubPhaseCharacter;

	private CharacterType m_hoverCharacterForGame;

	private List<CharacterType> m_validCharacterTypes = new List<CharacterType>();

	private List<CharacterType> m_selectedCharacterTypes = new List<CharacterType>();

	private List<CharacterType> m_friendlyBannedCharacterTypes = new List<CharacterType>();

	private List<CharacterType> m_enemyBannedCharacterTypes = new List<CharacterType>();

	private Dictionary<int, CharacterType> m_playerIDsOnDeck = new Dictionary<int, CharacterType>();

	private List<int> m_playerIDsThatSelected = new List<int>();

	private bool m_initialized;

	private bool m_IsOnDeck;

	private FreelancerResolutionPhaseSubType m_lastSetupSelectionPhaseSubType;

	private FreelancerResolutionPhaseSubType m_lastPhaseForUpdateCenter;

	private int m_playerIDBeingAnimated;

	private Animator m_animatorCurrentlyAnimating;

	private LobbyGameInfo LastGameInfo;

	private LobbyTeamInfo LastTeamInfo;

	private LobbyPlayerInfo LastPlayerInfo;

	private EnterFreelancerResolutionPhaseNotification m_lastDraftNotification;

	private float m_phaseStartTime;

	private TimeSpan m_timeInPhase;

	private float m_loadoutSelectStartTime;

	private GameStatus m_lastGameStatus;

	private List<UIRankedModeDraftScreen.CenterNotification> m_stateQueues = new List<UIRankedModeDraftScreen.CenterNotification>();

	private UIRankedModeDraftScreen.CenterNotification m_currentState;

	private List<GameObject> m_centerStateObjects = new List<GameObject>();

	private CanvasGroup m_characterSelectContainerCanvasGroup;

	private Animator m_containerAC;

	private bool m_intendedLockInBtnStatus;

	private List<UICharacterSelectFactionFilter> m_filterButtons;

	private UICharacterSelectFactionFilter m_lastFilterBtnClicked;

	private static UIRankedModeDraftScreen s_instance;

	public bool IsVisible { get; private set; }

	public bool GameIsLaunching { get; private set; }

	public CharacterType HoveredCharacter
	{
		get
		{
			return this.m_hoverCharacterForGame;
		}
		private set
		{
			this.m_hoverCharacterForGame = value;
			if (value != CharacterType.None)
			{
				this.m_selectedSubPhaseCharacter = value;
				this.SetupCharacterSettings(value);
			}
		}
	}

	private void SetupCharacterSettings(CharacterType charType)
	{
		CharacterCardInfo characterCardInfo;
		CharacterVisualInfo characterVisualInfo;
		if (this.LastPlayerInfo.CharacterType == charType)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.SetupCharacterSettings(CharacterType)).MethodHandle;
			}
			characterCardInfo = this.LastPlayerInfo.CharacterInfo.CharacterCards;
			characterVisualInfo = this.LastPlayerInfo.CharacterInfo.CharacterSkin;
		}
		else
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(charType);
			characterCardInfo = playerCharacterData.CharacterComponent.LastCards;
			characterVisualInfo = playerCharacterData.CharacterComponent.LastSkin;
		}
		this.m_rankedModeCharacterSettings.UpdateSelectedCharType(charType);
		CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(charType);
		if (!this.m_rankedModeCharacterSettings.m_spellsSubPanel.GetDisplayedCardInfo().Equals(characterCardInfo))
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_rankedModeCharacterSettings.m_spellsSubPanel.Setup(charType, characterCardInfo, false, false);
		}
		if (!(this.m_rankedModeCharacterSettings.m_abilitiesSubPanel.GetDisplayedCharacter() == null))
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_rankedModeCharacterSettings.m_abilitiesSubPanel.GetDisplayedCharacter().m_characterType.Equals(characterResourceLink.m_characterType))
			{
				goto IL_143;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_rankedModeCharacterSettings.m_abilitiesSubPanel.Setup(characterResourceLink, false);
		IL_143:
		if (this.m_rankedModeCharacterSettings.m_skinsSubPanel.GetDisplayedCharacterType().Equals(characterResourceLink.m_characterType))
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_rankedModeCharacterSettings.m_skinsSubPanel.GetDisplayedVisualInfo().Equals(characterVisualInfo))
			{
				goto IL_1BF;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_rankedModeCharacterSettings.m_skinsSubPanel.Setup(characterResourceLink, characterVisualInfo, false);
		IL_1BF:
		if (!(this.m_rankedModeCharacterSettings.m_tauntsSubPanel.GetDisplayedCharacter() == null))
		{
			if (this.m_rankedModeCharacterSettings.m_tauntsSubPanel.GetDisplayedCharacter().m_characterType.Equals(characterResourceLink.m_characterType))
			{
				return;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_rankedModeCharacterSettings.m_tauntsSubPanel.Setup(characterResourceLink, false);
	}

	public CharacterType ClientClickedCharacter
	{
		get
		{
			return this.m_selectedSubPhaseCharacter;
		}
	}

	public CharacterType SelectedCharacter
	{
		get
		{
			return this.m_assignedCharacterForGame;
		}
		private set
		{
			if (value != CharacterType.None)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.set_SelectedCharacter(CharacterType)).MethodHandle;
				}
				if (this.LastGameInfo != null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_assignedCharacterForGame != value)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_assignedCharacterForGame = value;
						this.m_hoverCharacterForGame = value;
						this.m_selectedSubPhaseCharacter = value;
						this.SetupCharacterSettings(value);
					}
				}
			}
		}
	}

	public static UIRankedModeDraftScreen Get()
	{
		return UIRankedModeDraftScreen.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.RankDraft;
	}

	public override void Awake()
	{
		if (UIRankedModeDraftScreen.s_instance == null)
		{
			UIRankedModeDraftScreen.s_instance = this;
			ClientGameManager.Get().OnGameInfoNotification += this.HandleGameInfoNotification;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange += this.OnLobbyGameplayOverridesUpdated;
			this.m_lockInBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.LockPhaseButtonClicked);
			this.m_lockInBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.RankFreelancerSelectClick;
			this.m_lockFreelancerBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.LockFreelancerBtnClicked);
			this.m_lockFreelancerBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.RankFreelancerLockin;
			this.m_skinsBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SettingsButtonClicked);
			this.m_abilitiesBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SettingsButtonClicked);
			this.m_catalystsBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SettingsButtonClicked);
			this.m_tauntsBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SettingsButtonClicked);
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
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.Awake()).MethodHandle;
					}
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
			if (UIRankedModeDraftScreen.<>f__am$cache0 == null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				UIRankedModeDraftScreen.<>f__am$cache0 = delegate(UITooltipBase tooltip)
				{
					(tooltip as UISimpleTooltip).Setup(StringUtil.TR("Wildcard", "Global"));
					return true;
				};
			}
			component.Setup(tooltipType, UIRankedModeDraftScreen.<>f__am$cache0, null);
			this.m_centerStateObjects.Add(this.m_blueTeamTurnNotification.gameObject);
			this.m_centerStateObjects.Add(this.m_redTeamTurnNotification.gameObject);
			this.m_centerStateObjects.Add(this.m_singleSelectionAC.gameObject);
			this.m_centerStateObjects.Add(this.m_doubleSelectionAC.gameObject);
			this.m_centerStateObjects.Add(this.m_swapPhaseAC.gameObject);
			this.m_centerStateObjects.Add(this.m_loadoutPhaseAC.gameObject);
			this.m_centerStateObjects.Add(this.m_gameLoadingAC.gameObject);
			this.m_containerAC = this.m_draftScreenContainer.GetComponent<Animator>();
			base.Awake();
		}
	}

	private bool IsBanned(CharacterType characterType)
	{
		if (!this.m_friendlyBannedCharacterTypes.IsNullOrEmpty<CharacterType>())
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.IsBanned(CharacterType)).MethodHandle;
			}
			if (this.m_friendlyBannedCharacterTypes.Contains(characterType))
			{
				return true;
			}
		}
		bool result;
		if (!this.m_enemyBannedCharacterTypes.IsNullOrEmpty<CharacterType>())
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			result = this.m_enemyBannedCharacterTypes.Contains(characterType);
		}
		else
		{
			result = false;
		}
		return result;
	}

	public void EditedSearchInput(string input)
	{
		this.UpdateCharacterButtonHighlights();
	}

	public void ClickedOnFactionFilter(UICharacterSelectFactionFilter btn)
	{
		if (this.m_lastFilterBtnClicked != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.ClickedOnFactionFilter(UICharacterSelectFactionFilter)).MethodHandle;
			}
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
		for (int i = 0; i < this.m_characterListDisplayButtons.Count; i++)
		{
			if (this.m_characterListDisplayButtons[i] != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.UpdateCharacterButtonHighlights()).MethodHandle;
				}
				if (this.m_characterListDisplayButtons[i].GetComponent<CanvasGroup>() != null)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_characterListDisplayButtons[i].GetComponent<CanvasGroup>().alpha = 1f;
				}
			}
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		if (this.m_lastFilterBtnClicked != null && this.m_lastFilterBtnClicked.m_btn.IsSelected())
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			for (int j = 0; j < this.m_characterListDisplayButtons.Count; j++)
			{
				if (!this.m_lastFilterBtnClicked.IsAvailable(this.m_characterListDisplayButtons[j].m_characterType))
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					CanvasGroup component = this.m_characterListDisplayButtons[j].GetComponent<CanvasGroup>();
					if (component != null)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						component.alpha = 0.3f;
					}
				}
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (!this.m_searchInputField.text.IsNullOrEmpty())
		{
			for (int k = 0; k < this.m_characterListDisplayButtons.Count; k++)
			{
				string text = string.Empty;
				CharacterResourceLink characterResourceLink = this.m_characterListDisplayButtons[k].GetCharacterResourceLink();
				if (characterResourceLink != null)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					text = characterResourceLink.GetDisplayName();
				}
				if (!this.DoesSearchMatchDisplayName(this.m_searchInputField.text.ToLower(), text.ToLower()))
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_characterListDisplayButtons[k].GetComponent<CanvasGroup>().alpha = 0.3f;
				}
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	private bool DoesSearchMatchDisplayName(string searchText, string displayText)
	{
		for (int i = 0; i < searchText.Length; i++)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (i >= displayText.Length)
			{
				break;
			}
			if (searchText[i] != displayText[i])
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.DoesSearchMatchDisplayName(string, string)).MethodHandle;
				}
				return false;
			}
		}
		return true;
	}

	public void DoQueueState(UIRankedModeDraftScreen.CenterNotification notification)
	{
		UIManager.SetGameObjectActive(this.m_blueTeamTurnNotification, notification == UIRankedModeDraftScreen.CenterNotification.BlueTeamNotification, null);
		UIManager.SetGameObjectActive(this.m_redTeamTurnNotification, notification == UIRankedModeDraftScreen.CenterNotification.RedTeamNotification, null);
		UIManager.SetGameObjectActive(this.m_singleSelectionAC, notification == UIRankedModeDraftScreen.CenterNotification.BlueTeamSingleSelectStart || notification == UIRankedModeDraftScreen.CenterNotification.RedTeamSingleSelectStart, null);
		UIManager.SetGameObjectActive(this.m_doubleSelectionAC, notification == UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectStart || notification == UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectStart, null);
		UIManager.SetGameObjectActive(this.m_swapPhaseAC, notification == UIRankedModeDraftScreen.CenterNotification.TradePhase, null);
		UIManager.SetGameObjectActive(this.m_loadoutPhaseAC, notification == UIRankedModeDraftScreen.CenterNotification.LoadoutPhase, null);
		UIManager.SetGameObjectActive(this.m_gameLoadingAC, notification == UIRankedModeDraftScreen.CenterNotification.GameLoadPhase, null);
		if (this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
		{
			this.m_blueTeamTurnTextNotification.text = StringUtil.TR("BlueBans", "RankMode");
			this.m_redTeamTurnTextNotification.text = StringUtil.TR("RedBans", "RankMode");
		}
		else if (this.m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase())
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.DoQueueState(UIRankedModeDraftScreen.CenterNotification)).MethodHandle;
			}
			this.m_blueTeamTurnTextNotification.text = StringUtil.TR("BluePicks", "RankMode");
			this.m_redTeamTurnTextNotification.text = StringUtil.TR("RedPicks", "RankMode");
		}
		bool flag;
		if (notification != UIRankedModeDraftScreen.CenterNotification.BlueTeamNotification)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (notification != UIRankedModeDraftScreen.CenterNotification.BlueTeamSingleSelectStart)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				flag = (notification == UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectStart);
				goto IL_150;
			}
		}
		flag = true;
		IL_150:
		bool flag2 = flag;
		bool flag3;
		if (notification != UIRankedModeDraftScreen.CenterNotification.RedTeamNotification)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (notification != UIRankedModeDraftScreen.CenterNotification.RedTeamSingleSelectStart)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				flag3 = (notification == UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectStart);
				goto IL_175;
			}
		}
		flag3 = true;
		IL_175:
		bool flag4 = flag3;
		this.SetCenterBackground(flag2, flag4);
		if (notification != UIRankedModeDraftScreen.CenterNotification.BlueTeamNotification)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (notification != UIRankedModeDraftScreen.CenterNotification.RedTeamNotification)
			{
				return;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		RankedResolutionPhaseData? rankedData = this.m_lastDraftNotification.RankedData;
		this.UpdateCenterVisuals(rankedData.Value, flag2, flag4);
	}

	private bool IsDoubleSelectinReadyToAdvance()
	{
		if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectStart)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.IsDoubleSelectinReadyToAdvance()).MethodHandle;
			}
			if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectStart)
			{
				return false;
			}
		}
		if (this.m_stateQueues.Count > 0)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_stateQueues[0] != UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectEnd)
			{
				if (this.m_stateQueues[0] != UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectEnd)
				{
					return false;
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (!this.m_doubleRightSelectionCharacterSelected.gameObject.activeInHierarchy)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!this.m_doubleLeftSelectionCharacterSelected.gameObject.activeInHierarchy)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					return true;
				}
			}
		}
		return false;
	}

	private bool IsAnyCenterStateActive()
	{
		for (int i = 0; i < this.m_centerStateObjects.Count; i++)
		{
			if (this.m_centerStateObjects[i].gameObject.activeSelf)
			{
				return true;
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.IsAnyCenterStateActive()).MethodHandle;
		}
		return false;
	}

	private void ClearAllStates()
	{
		for (int i = 0; i < this.m_centerStateObjects.Count; i++)
		{
			UIManager.SetGameObjectActive(this.m_centerStateObjects[i], false, null);
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.ClearAllStates()).MethodHandle;
		}
	}

	private void QueueCenterState(UIRankedModeDraftScreen.CenterNotification notification)
	{
		if (notification == UIRankedModeDraftScreen.CenterNotification.None)
		{
			return;
		}
		if (notification != UIRankedModeDraftScreen.CenterNotification.LoadoutPhase)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.QueueCenterState(UIRankedModeDraftScreen.CenterNotification)).MethodHandle;
			}
			if (notification != UIRankedModeDraftScreen.CenterNotification.GameLoadPhase)
			{
				goto IL_3C;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.ClearAllStates();
		this.m_stateQueues.Clear();
		IL_3C:
		if (this.m_stateQueues.Count > 0)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_stateQueues[this.m_stateQueues.Count - 1] == notification)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				return;
			}
		}
		else if (this.m_currentState == notification)
		{
			return;
		}
		this.m_stateQueues.Add(notification);
	}

	public void SettingsButtonClicked(BaseEventData data)
	{
		UICharacterSelectCharacterSettingsPanel.TabPanel tab = UICharacterSelectCharacterSettingsPanel.TabPanel.None;
		if ((data as PointerEventData).pointerCurrentRaycast.gameObject == this.m_skinsBtn.spriteController.gameObject)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.SettingsButtonClicked(BaseEventData)).MethodHandle;
			}
			tab = UICharacterSelectCharacterSettingsPanel.TabPanel.Skins;
		}
		else if ((data as PointerEventData).pointerCurrentRaycast.gameObject == this.m_abilitiesBtn.spriteController.gameObject)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			tab = UICharacterSelectCharacterSettingsPanel.TabPanel.Abilities;
		}
		else if ((data as PointerEventData).pointerCurrentRaycast.gameObject == this.m_catalystsBtn.spriteController.gameObject)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			tab = UICharacterSelectCharacterSettingsPanel.TabPanel.Catalysts;
		}
		else if ((data as PointerEventData).pointerCurrentRaycast.gameObject == this.m_tauntsBtn.spriteController.gameObject)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			tab = UICharacterSelectCharacterSettingsPanel.TabPanel.Taunts;
		}
		UIRankedCharacterSelectSettingsPanel.Get().SetVisible(true, tab);
	}

	private void OnDestroy()
	{
		UIRankedModeDraftScreen.s_instance = null;
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnGameInfoNotification -= this.HandleGameInfoNotification;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= this.OnLobbyGameplayOverridesUpdated;
		}
	}

	private void SetFreelancerSettingButtonsVisible(bool visible)
	{
		UIManager.SetGameObjectActive(this.m_skinsBtn, visible, null);
		UIManager.SetGameObjectActive(this.m_abilitiesBtn, visible, null);
		UIManager.SetGameObjectActive(this.m_catalystsBtn, visible, null);
		UIManager.SetGameObjectActive(this.m_tauntsBtn, visible, null);
	}

	public void OnLobbyGameplayOverridesUpdated(LobbyGameplayOverrides gameplayOverrides)
	{
		this.CheckCharacterListValidity();
	}

	public void HandleGameInfoNotification(GameInfoNotification notification)
	{
		if (notification.GameInfo == null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.HandleGameInfoNotification(GameInfoNotification)).MethodHandle;
			}
			Log.Error("Why is GameInfo null?", new object[0]);
			return;
		}
		if (notification.PlayerInfo == null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			return;
		}
		if (notification.TeamInfo == null)
		{
			Log.Warning("Why GameInfoNotification.TeamInfo null?", new object[0]);
			return;
		}
		if (notification.TeamInfo.TeamPlayerInfo == null)
		{
			Log.Warning("Why GameInfoNotification.TeamInfo.TeamPlayerInfo null?", new object[0]);
			return;
		}
		if (notification.GameInfo.GameStatus == GameStatus.Stopped)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			AppState_GroupCharacterSelect.Get().Enter();
			return;
		}
		if (notification.PlayerInfo.AccountId != 0L)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (notification.TeamInfo.TeamPlayerInfo.Count != 0)
			{
				if (notification.GameInfo.GameStatus == GameStatus.LoadoutSelecting && this.m_lastGameStatus != GameStatus.LoadoutSelecting)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_loadoutSelectStartTime = Time.realtimeSinceStartup;
					this.SetFreelancerSettingButtonsVisible(true);
				}
				bool flag = false;
				if (this.LastGameInfo == null)
				{
					flag = true;
					this.m_phaseStartTime = Time.time;
				}
				this.LastGameInfo = notification.GameInfo;
				this.LastTeamInfo = notification.TeamInfo;
				this.LastPlayerInfo = notification.PlayerInfo;
				this.m_lastGameStatus = this.LastGameInfo.GameStatus;
				if (this.IsVisible)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					bool flag2 = false;
					if (notification.GameInfo.GameStatus == GameStatus.FreelancerSelecting)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						this.GameIsLaunching = false;
					}
					else
					{
						if (notification.GameInfo.GameStatus != GameStatus.Stopped)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (notification.GameInfo.GameStatus == GameStatus.LoadoutSelecting)
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								flag2 = true;
								this.UpdateGameLaunching(notification);
								goto IL_22F;
							}
						}
						if (notification.GameInfo.GameStatus != GameStatus.Stopped)
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							if (notification.GameInfo.GameStatus > GameStatus.LoadoutSelecting)
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!this.m_stateQueues.Contains(UIRankedModeDraftScreen.CenterNotification.GameLoadPhase))
								{
									for (;;)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.GameLoadPhase)
									{
										this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.GameLoadPhase);
									}
								}
								this.UpdateGameLaunching(notification);
							}
						}
					}
					IL_22F:
					if (flag2)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!this.m_stateQueues.Contains(UIRankedModeDraftScreen.CenterNotification.LoadoutPhase) && this.m_currentState != UIRankedModeDraftScreen.CenterNotification.LoadoutPhase)
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.LoadoutPhase);
						}
					}
					if (this.m_lastDraftNotification != null && this.m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.LastGameInfo.GameStatus == GameStatus.FreelancerSelecting && !this.m_stateQueues.Contains(UIRankedModeDraftScreen.CenterNotification.TradePhase))
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.TradePhase)
							{
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.TradePhase);
							}
						}
					}
					MapData mapData = GameWideData.Get().GetMapData(notification.GameInfo.GameConfig.Map);
					string mapDisplayName = GameWideData.Get().GetMapDisplayName(notification.GameInfo.GameConfig.Map);
					Sprite sprite;
					if (mapData != null)
					{
						sprite = (Resources.Load(mapData.ResourceImageSpriteLocation, typeof(Sprite)) as Sprite);
					}
					else
					{
						sprite = (Resources.Load("Stages/information_stage_image", typeof(Sprite)) as Sprite);
					}
					this.m_stageImage.sprite = sprite;
					this.m_introStageImage.sprite = sprite;
					this.m_stageText.text = mapDisplayName;
					this.m_introStageText.text = mapDisplayName;
					if (notification.GameInfo.GameConfig.GameType == GameType.Ranked)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_matchFoundText.text = StringUtil.TR("RankedMatchFound", "OverlayScreensScene");
					}
					else
					{
						this.m_matchFoundText.text = string.Format(StringUtil.TR("SubtypeFound", "Global"), StringUtil.TR(notification.GameInfo.GameConfig.InstanceSubType.LocalizedName));
					}
					this.SetupPlayerLists();
					this.UpdateNotification(this.m_lastDraftNotification, true && !flag);
				}
				return;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	private void SetupPlayerLists()
	{
		if (this.LastGameInfo != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.SetupPlayerLists()).MethodHandle;
			}
			if (this.m_lastDraftNotification != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_lastDraftNotification.RankedData != null)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					RankedResolutionPhaseData value = this.m_lastDraftNotification.RankedData.Value;
					Team team = this.LastPlayerInfo.TeamId;
					if (this.LastPlayerInfo.TeamId == Team.Spectator)
					{
						team = Team.TeamA;
					}
					int num = 0;
					int num2 = 0;
					for (int i = 0; i < value.PlayerIdByImporance.Count; i++)
					{
						int num3 = value.PlayerIdByImporance[i];
						IEnumerable<LobbyPlayerInfo> enumerable = this.LastTeamInfo.TeamInfo(team);
						foreach (LobbyPlayerInfo lobbyPlayerInfo in enumerable)
						{
							if (lobbyPlayerInfo.PlayerId == num3)
							{
								for (;;)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								if (num < this.m_blueTeamMembers.Length)
								{
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									this.m_blueTeamMembers[num].Setup(lobbyPlayerInfo, false);
									num++;
								}
								break;
							}
						}
						IEnumerable<LobbyPlayerInfo> enumerable2 = this.LastTeamInfo.TeamInfo(team.OtherTeam());
						IEnumerator<LobbyPlayerInfo> enumerator2 = enumerable2.GetEnumerator();
						try
						{
							while (enumerator2.MoveNext())
							{
								LobbyPlayerInfo lobbyPlayerInfo2 = enumerator2.Current;
								if (lobbyPlayerInfo2.PlayerId == num3)
								{
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									if (num2 < this.m_redTeamMembers.Length)
									{
										for (;;)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											break;
										}
										this.m_redTeamMembers[num2].Setup(lobbyPlayerInfo2, true);
										num2++;
									}
									goto IL_1B4;
								}
							}
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						finally
						{
							if (enumerator2 != null)
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								enumerator2.Dispose();
							}
						}
						IL_1B4:;
					}
				}
			}
		}
	}

	private void Update()
	{
		if (this.m_lastDraftNotification != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.Update()).MethodHandle;
			}
			if (!this.GameIsLaunching)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_lastDraftNotification.RankedData != null)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (UICharacterSelectWorldObjects.Get().IsVisible())
					{
						UICharacterSelectWorldObjects.Get().SetVisible(false);
					}
					if ((double)(Time.time - this.m_phaseStartTime) < this.m_timeInPhase.TotalSeconds)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						float num = (float)this.m_timeInPhase.TotalSeconds - Time.time + this.m_phaseStartTime;
						int num2 = Mathf.RoundToInt(num);
						RankedResolutionPhaseData value = this.m_lastDraftNotification.RankedData.Value;
						Team currentTeam = this.GetCurrentTeam(value);
						if (currentTeam != Team.TeamA)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (currentTeam != Team.TeamB)
							{
								for (;;)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								if (this.m_gameCountdownTimer.text != num2.ToString())
								{
									for (;;)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									this.m_gameCountdownTimer.text = num2.ToString();
									this.m_gameCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
								}
								this.m_redCountdownTimer.text = string.Empty;
								this.m_blueCountdownTimer.text = string.Empty;
								goto IL_28A;
							}
						}
						if (currentTeam == this.LastPlayerInfo.TeamId)
						{
							this.m_gameCountdownTimer.text = string.Empty;
							this.m_redCountdownTimer.text = string.Empty;
							if (this.m_blueCountdownTimer.text != num2.ToString())
							{
								for (;;)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								this.m_blueCountdownTimer.text = num2.ToString();
								this.m_blueCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
							}
						}
						else
						{
							this.m_gameCountdownTimer.text = string.Empty;
							if (this.m_redCountdownTimer.text != num2.ToString())
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								this.m_redCountdownTimer.text = num2.ToString();
								this.m_redCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
							}
							this.m_blueCountdownTimer.text = string.Empty;
						}
						IL_28A:
						if (num2 <= 5)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							if (Mathf.RoundToInt(num + Time.deltaTime) != num2)
							{
								UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeTimerTick);
							}
						}
					}
					if (this.SelectedCharacter != CharacterType.None)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.SelectedCharacter != this.ClientClickedCharacter)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							int playerId = GameManager.Get().PlayerInfo.PlayerId;
							if (!this.m_lastDraftNotification.RankedData.Value.\u001D(playerId))
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								this.SetupCharacterSettings(this.SelectedCharacter);
							}
						}
					}
				}
			}
		}
		if (this.GameIsLaunching)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_gameCountdownTimer.text = string.Empty;
			if (this.LastGameInfo != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.LastGameInfo.GameStatus == GameStatus.LoadoutSelecting)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					float num3 = Mathf.Max(0f, (float)this.LastGameInfo.LoadoutSelectTimeout.TotalSeconds - (Time.realtimeSinceStartup - this.m_loadoutSelectStartTime));
					int num4 = Mathf.RoundToInt(num3);
					if (this.m_gameCountdownTimer.text != num4.ToString())
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_gameCountdownTimer.text = num4.ToString();
						this.m_gameCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
					}
					if (num4 < 6 && Mathf.RoundToInt(num3 + Time.deltaTime) != num4)
					{
						UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeTimerTick);
					}
				}
			}
			this.m_redCountdownTimer.text = string.Empty;
			this.m_blueCountdownTimer.text = string.Empty;
		}
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis > 0f)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			this.SetPageIndex(this.m_currentCharacterPage + 1);
		}
		else if (axis < 0f)
		{
			this.SetPageIndex(this.m_currentCharacterPage - 1);
		}
		if (!this.m_introContainer.gameObject.activeSelf)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.IsAnyCenterStateActive())
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!this.IsDoubleSelectinReadyToAdvance())
				{
					goto IL_524;
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (this.m_stateQueues.Count > 0)
			{
				this.DoQueueState(this.m_stateQueues[0]);
				this.m_currentState = this.m_stateQueues[0];
				this.m_stateQueues.RemoveAt(0);
			}
		}
		IL_524:
		if (this.m_characterSelectContainerCanvasGroup == null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_characterSelectContainerCanvasGroup = this.m_characterSelectContainer.GetComponent<CanvasGroup>();
		}
		if (this.m_journeyLength <= 0f)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_currentCharacterPage == this.m_currentVisiblePage)
			{
				goto IL_7F3;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		float num5 = (Time.time - this.m_startTime) * this.m_timeForPageToSwap;
		float num6 = num5 / this.m_journeyLength;
		Vector2 anchoredPosition = Vector2.Lerp(this.m_startLocation, this.m_endLocation, num6);
		if (!float.IsNaN(anchoredPosition.x))
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!float.IsNaN(anchoredPosition.y))
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				(this.m_characterSelectContainer.transform as RectTransform).anchoredPosition = anchoredPosition;
				if (this.m_characterSelectContainerCanvasGroup != null)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_currentCharacterPage != this.m_currentVisiblePage)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_characterSelectContainerCanvasGroup.alpha = 1f - num6;
					}
					else
					{
						this.m_characterSelectContainerCanvasGroup.alpha = num6;
					}
				}
				if (num6 >= 1f)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_characterSelectContainerCanvasGroup.alpha <= 0f)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						Vector2 anchoredPosition2 = (this.m_characterSelectContainer.gameObject.transform as RectTransform).anchoredPosition;
						if (this.m_endLocation.x < 0f)
						{
							(this.m_characterSelectContainer.gameObject.transform as RectTransform).anchoredPosition = new Vector2(anchoredPosition2.x * -1f, anchoredPosition2.y);
							this.m_startTime = Time.time;
							this.m_startLocation = anchoredPosition2;
							this.m_endLocation = new Vector2(0f, anchoredPosition2.y);
							this.m_journeyLength = Vector2.Distance(this.m_startLocation, this.m_endLocation);
						}
						else if (this.m_endLocation.x > 0f)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							(this.m_characterSelectContainer.gameObject.transform as RectTransform).anchoredPosition = new Vector2(anchoredPosition2.x * -1f, anchoredPosition2.y);
							this.m_startTime = Time.time;
							this.m_startLocation = anchoredPosition2;
							this.m_endLocation = new Vector2(0f, anchoredPosition2.y);
							this.m_journeyLength = Vector2.Distance(this.m_startLocation, this.m_endLocation);
						}
						this.UpdateCharacterButtons();
					}
					else
					{
						this.m_journeyLength = 0f;
					}
				}
			}
		}
		IL_7F3:
		if (this.IsCenterSelectAnimating())
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(this.m_lockInBtn, false, null);
		}
		else
		{
			string text;
			if (this.m_lastDraftNotification != null && this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				text = StringUtil.TR("Ban", "OverlayScreensScene");
			}
			else
			{
				text = StringUtil.TR("LockIn", "OverlayScreensScene");
			}
			for (int i = 0; i < this.m_lockInText.Length; i++)
			{
				this.m_lockInText[i].text = text;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(this.m_lockInBtn, this.m_intendedLockInBtnStatus, null);
			this.m_lockInBtn.SetDisabled(this.m_selectedSubPhaseCharacter == this.SelectedCharacter);
		}
		if (this.m_containerAC == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_containerAC = this.m_draftScreenContainer.GetComponent<Animator>();
		}
		if (this.m_containerAC != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_containerAC.gameObject.activeInHierarchy)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_containerAC.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					this.DoCharacterSelectContainerActiveCheck();
				}
			}
		}
		UIManager.SetGameObjectActive(this.m_searchFiltersContainer, this.m_characterSelectContainer.gameObject.activeSelf, null);
	}

	private bool IsCenterSelectAnimating()
	{
		bool result = false;
		if (this.m_singleSelectionCharacterSelected.gameObject.activeInHierarchy)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.IsCenterSelectAnimating()).MethodHandle;
			}
			result = true;
		}
		else if (this.m_doubleRightSelectionCharacterSelected.gameObject.activeInHierarchy && this.doubleSelectionLeftCharacter.gameObject.activeInHierarchy)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			result = true;
		}
		else if (this.m_doubleLeftSelectionCharacterSelected.gameObject.activeInHierarchy)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.doubleSelectionRightCharacter.gameObject.activeInHierarchy)
			{
				result = true;
			}
		}
		return result;
	}

	public void LockFreelancerBtnClicked(BaseEventData data)
	{
		if (this.m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
		{
			UIManager.SetGameObjectActive(this.m_lockFreelancerContainer, false, null);
			ClientGameManager.Get().SendRankedTradeRequest_StopTrading();
		}
	}

	public void LockPhaseButtonClicked(BaseEventData data)
	{
		if (this.m_selectedSubPhaseCharacter != CharacterType.None)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.LockPhaseButtonClicked(BaseEventData)).MethodHandle;
			}
			if (this.SelectedCharacter != this.m_selectedSubPhaseCharacter)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
				{
					ClientGameManager.Get().SendRankedBanRequest(this.m_selectedSubPhaseCharacter);
				}
				else
				{
					ClientGameManager.Get().SendRankedSelectRequest(this.m_selectedSubPhaseCharacter);
				}
			}
		}
	}

	private bool DidPlayerLockInDuringSwapPhase(RankedResolutionPhaseData data, long playerID)
	{
		bool result = false;
		if (data.TradeActions != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.DidPlayerLockInDuringSwapPhase(RankedResolutionPhaseData, long)).MethodHandle;
			}
			using (List<RankedTradeData>.Enumerator enumerator = data.TradeActions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RankedTradeData rankedTradeData = enumerator.Current;
					if ((long)rankedTradeData.OfferingPlayerId == playerID)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (rankedTradeData.TradeAction == RankedTradeData.TradeActionType.\u0012)
						{
							return true;
						}
					}
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		return result;
	}

	private void UpdateHoverSelfStatus(RankedResolutionPhaseData data)
	{
		if (this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.UpdateHoverSelfStatus(RankedResolutionPhaseData)).MethodHandle;
			}
			return;
		}
		if (this.LastGameInfo != null)
		{
			int playerId = this.LastPlayerInfo.PlayerId;
			int i = 0;
			while (i < this.m_blueTeamMembers.Length)
			{
				if (this.m_blueTeamMembers[i].PlayerID == playerId)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.HoveredCharacter != CharacterType.None)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!this.m_selectedCharacterTypes.Contains(this.HoveredCharacter))
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!this.IsBanned(this.HoveredCharacter))
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(this.HoveredCharacter);
								this.m_blueTeamMembers[i].SetBrowseCharacterImageVisible(true);
								this.m_blueTeamMembers[i].SetHoverCharacter(characterResourceLink);
							}
						}
					}
					IL_FF:
					if (this.m_playerIDsOnDeck.Count == 1)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						KeyValuePair<int, CharacterType> selectedChar = new KeyValuePair<int, CharacterType>(playerId, this.HoveredCharacter);
						if (this.m_playerIDsOnDeck.ContainsKey(playerId))
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							this.SetupSelection(selectedChar, data, this.m_singleCharacterName, this.singleNoSelectionCharacter, this.singleBrowseSelectionCharacter, this.singleSelectionCharacter, this.m_singleSelectionCharacterSelected, this.m_singleBlueSelectionCharacterSelected, this.m_singleBlueTeamSelectedCharacter, this.m_singleBlueTeamPlayerName, true, false);
						}
						return;
					}
					if (this.m_playerIDsOnDeck.Count == 2)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						bool flag = true;
						using (Dictionary<int, CharacterType>.Enumerator enumerator = this.m_playerIDsOnDeck.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								KeyValuePair<int, CharacterType> selectedChar2 = enumerator.Current;
								if (flag)
								{
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									this.SetupSelection(selectedChar2, data, this.m_leftCharacterName, this.doubleNoSelectionLeftCharacter, this.doubleBrowseSelectionLeftCharacter, this.doubleSelectionLeftCharacter, this.m_doubleLeftSelectionCharacterSelected, this.m_doubleLeftBlueSelectionCharacterSelected, this.m_doubleLeftBlueTeamSelectedCharacter, this.m_doubleLeftBlueTeamPlayerName, true, false);
									flag = false;
								}
								else
								{
									this.SetupSelection(selectedChar2, data, this.m_rightCharacterName, this.doubleNoSelectionRightCharacter, this.doubleBrowseSelectionRightCharacter, this.doubleSelectionRightCharacter, this.m_doubleRightSelectionCharacterSelected, this.m_doubleRightBlueSelectionCharacterSelected, this.m_doubleRightBlueTeamSelectedCharacter, this.m_doubleRightBlueTeamPlayerName, true, false);
								}
							}
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						return;
					}
					return;
				}
				else
				{
					i++;
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				goto IL_FF;
			}
		}
	}

	private void UpdateHoverStatus(RankedResolutionPhaseData data)
	{
		bool flag = this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase();
		int i = 0;
		while (i < this.m_blueTeamMembers.Length)
		{
			if (!flag)
			{
				goto IL_77;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.UpdateHoverStatus(RankedResolutionPhaseData)).MethodHandle;
			}
			if (!this.m_playerIDsOnDeck.ContainsKey(this.OurPlayerId))
			{
				goto IL_77;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_blueTeamMembers[i].PlayerID != this.OurPlayerId)
			{
				goto IL_77;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			IL_330:
			i++;
			continue;
			IL_77:
			if (data.FriendlyTeamSelections.ContainsKey(this.m_blueTeamMembers[i].PlayerID))
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_blueTeamMembers[i].SetBrowseCharacterImageVisible(false);
				goto IL_330;
			}
			if (this.m_playerIDsOnDeck.ContainsKey(this.m_blueTeamMembers[i].PlayerID))
			{
				if (this.m_playerIDsOnDeck[this.m_blueTeamMembers[i].PlayerID] != CharacterType.None)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.IsBanned(this.m_playerIDsOnDeck[this.m_blueTeamMembers[i].PlayerID]) || this.m_selectedCharacterTypes.Contains(this.m_playerIDsOnDeck[this.m_blueTeamMembers[i].PlayerID]))
					{
						this.m_blueTeamMembers[i].SetBrowseCharacterImageVisible(false);
						UIManager.SetGameObjectActive(this.m_blueTeamMembers[i].m_noCharacterImage, true, null);
					}
					else if (this.OurPlayerId != this.m_blueTeamMembers[i].PlayerID)
					{
						CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(this.m_playerIDsOnDeck[this.m_blueTeamMembers[i].PlayerID]);
						this.m_blueTeamMembers[i].SetBrowseCharacterImageVisible(true);
						this.m_blueTeamMembers[i].SetHoverCharacter(characterResourceLink);
					}
					else if (this.HoveredCharacter != CharacterType.None)
					{
						CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(this.HoveredCharacter);
						this.m_blueTeamMembers[i].SetBrowseCharacterImageVisible(true);
						this.m_blueTeamMembers[i].SetHoverCharacter(characterResourceLink2);
					}
				}
				goto IL_330;
			}
			bool browseCharacterImageVisible = false;
			using (List<RankedResolutionPlayerState>.Enumerator enumerator = data.UnselectedPlayerStates.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RankedResolutionPlayerState rankedResolutionPlayerState = enumerator.Current;
					if (rankedResolutionPlayerState.PlayerId == this.m_blueTeamMembers[i].PlayerID)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (rankedResolutionPlayerState.Intention != CharacterType.None)
						{
							if (!this.IsBanned(rankedResolutionPlayerState.Intention))
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!this.m_selectedCharacterTypes.Contains(rankedResolutionPlayerState.Intention))
								{
									CharacterResourceLink characterResourceLink3 = GameWideData.Get().GetCharacterResourceLink(rankedResolutionPlayerState.Intention);
									if (characterResourceLink3 != null)
									{
										for (;;)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											break;
										}
										browseCharacterImageVisible = true;
										this.m_blueTeamMembers[i].SetHoverCharacter(characterResourceLink3);
										goto IL_2F7;
									}
									goto IL_2F7;
								}
							}
							this.m_blueTeamMembers[i].SetBrowseCharacterImageVisible(false);
							UIManager.SetGameObjectActive(this.m_blueTeamMembers[i].m_noCharacterImage, true, null);
						}
						IL_2F7:
						goto IL_321;
					}
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			IL_321:
			this.m_blueTeamMembers[i].SetBrowseCharacterImageVisible(browseCharacterImageVisible);
			goto IL_330;
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			break;
		}
		for (int j = 0; j < this.m_redTeamMembers.Length; j++)
		{
			if (data.EnemyTeamSelections.ContainsKey(this.m_redTeamMembers[j].PlayerID))
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_redTeamMembers[j].SetBrowseCharacterImageVisible(false);
			}
			else
			{
				bool flag2 = false;
				if (this.m_playerIDsOnDeck.ContainsKey(this.m_redTeamMembers[j].PlayerID) && this.m_playerIDsOnDeck[this.m_redTeamMembers[j].PlayerID] != CharacterType.None)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!this.IsBanned(this.m_playerIDsOnDeck[this.m_redTeamMembers[j].PlayerID]))
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						CharacterResourceLink characterResourceLink4 = GameWideData.Get().GetCharacterResourceLink(this.m_playerIDsOnDeck[this.m_redTeamMembers[j].PlayerID]);
						this.m_redTeamMembers[j].SetBrowseCharacterImageVisible(true);
						this.m_redTeamMembers[j].SetHoverCharacter(characterResourceLink4);
						flag2 = true;
					}
				}
				if (!flag2)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_redTeamMembers[j].SetBrowseCharacterImageVisible(false);
				}
			}
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void CheckSelectedCharForCenterPiece(bool isOnBlue, RankedResolutionPhaseData data)
	{
		if (this.m_playerIDsOnDeck.Count == 1)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.CheckSelectedCharForCenterPiece(bool, RankedResolutionPhaseData)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_singleSelectionCharacterSelected, true, null);
			using (Dictionary<int, CharacterType>.Enumerator enumerator = this.m_playerIDsOnDeck.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, CharacterType> keyValuePair = enumerator.Current;
					KeyValuePair<int, CharacterType> selectedChar = keyValuePair;
					TextMeshProUGUI singleCharacterName = this.m_singleCharacterName;
					Image noCharacter = this.singleNoSelectionCharacter;
					Image browseCharacter = this.singleBrowseSelectionCharacter;
					Image selectedCharacter = this.singleSelectionCharacter;
					Animator singleSelectionCharacterSelected = this.m_singleSelectionCharacterSelected;
					Animator selectedCharacterNameAnimator;
					if (isOnBlue)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						selectedCharacterNameAnimator = this.m_singleBlueSelectionCharacterSelected;
					}
					else
					{
						selectedCharacterNameAnimator = this.m_singleRedTeamSelectionCharacterSelected;
					}
					this.SetupSelection(selectedChar, data, singleCharacterName, noCharacter, browseCharacter, selectedCharacter, singleSelectionCharacterSelected, selectedCharacterNameAnimator, (!isOnBlue) ? this.m_singleRedTeamSelectedCharacter : this.m_singleBlueTeamSelectedCharacter, (!isOnBlue) ? this.m_singleRedTeamPlayerName : this.m_singleBlueTeamPlayerName, isOnBlue, !isOnBlue);
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		else
		{
			bool flag = !this.doubleSelectionLeftCharacter.gameObject.activeInHierarchy;
			using (Dictionary<int, CharacterType>.Enumerator enumerator2 = this.m_playerIDsOnDeck.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<int, CharacterType> keyValuePair2 = enumerator2.Current;
					if (flag)
					{
						flag = false;
						KeyValuePair<int, CharacterType> selectedChar2 = keyValuePair2;
						TextMeshProUGUI leftCharacterName = this.m_leftCharacterName;
						Image noCharacter2 = this.doubleNoSelectionLeftCharacter;
						Image browseCharacter2 = this.doubleBrowseSelectionLeftCharacter;
						Image selectedCharacter2 = this.doubleSelectionLeftCharacter;
						Animator doubleLeftSelectionCharacterSelected = this.m_doubleLeftSelectionCharacterSelected;
						Animator selectedCharacterNameAnimator2;
						if (isOnBlue)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							selectedCharacterNameAnimator2 = this.m_doubleLeftBlueSelectionCharacterSelected;
						}
						else
						{
							selectedCharacterNameAnimator2 = this.m_doubleLeftRedTeamSelectionCharacterSelected;
						}
						TextMeshProUGUI selectedCharacterText;
						if (isOnBlue)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							selectedCharacterText = this.m_doubleLeftBlueTeamSelectedCharacter;
						}
						else
						{
							selectedCharacterText = this.m_doubleLeftRedTeamSelectedCharacter;
						}
						this.SetupSelection(selectedChar2, data, leftCharacterName, noCharacter2, browseCharacter2, selectedCharacter2, doubleLeftSelectionCharacterSelected, selectedCharacterNameAnimator2, selectedCharacterText, (!isOnBlue) ? this.m_doubleLeftRedTeamPlayerName : this.m_doubleLeftBlueTeamPlayerName, isOnBlue, !isOnBlue);
					}
					else
					{
						KeyValuePair<int, CharacterType> selectedChar3 = keyValuePair2;
						TextMeshProUGUI rightCharacterName = this.m_rightCharacterName;
						Image noCharacter3 = this.doubleNoSelectionRightCharacter;
						Image browseCharacter3 = this.doubleBrowseSelectionRightCharacter;
						Image selectedCharacter3 = this.doubleSelectionRightCharacter;
						Animator doubleRightSelectionCharacterSelected = this.m_doubleRightSelectionCharacterSelected;
						Animator selectedCharacterNameAnimator3 = (!isOnBlue) ? this.m_doubleRightRedTeamSelectionCharacterSelected : this.m_doubleRightBlueSelectionCharacterSelected;
						TextMeshProUGUI selectedCharacterText2 = (!isOnBlue) ? this.m_doubleRightRedTeamSelectedCharacter : this.m_doubleRightBlueTeamSelectedCharacter;
						TextMeshProUGUI playerName;
						if (isOnBlue)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							playerName = this.m_doubleRightBlueTeamPlayerName;
						}
						else
						{
							playerName = this.m_doubleRightRedTeamPlayerName;
						}
						this.SetupSelection(selectedChar3, data, rightCharacterName, noCharacter3, browseCharacter3, selectedCharacter3, doubleRightSelectionCharacterSelected, selectedCharacterNameAnimator3, selectedCharacterText2, playerName, isOnBlue, !isOnBlue);
					}
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
	}

	private void UpdatePlayerSelecting(RankedResolutionPhaseData data)
	{
		for (int i = 0; i < this.m_blueTeamMembers.Length; i++)
		{
			this.m_blueTeamMembers[i].SetAsSelecting(data.\u001D(this.m_blueTeamMembers[i].PlayerID));
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.UpdatePlayerSelecting(RankedResolutionPhaseData)).MethodHandle;
		}
		for (int j = 0; j < this.m_redTeamMembers.Length; j++)
		{
			this.m_redTeamMembers[j].SetAsSelecting(data.\u001D(this.m_redTeamMembers[j].PlayerID));
		}
	}

	private void DoCharacterSelectContainerActiveCheck()
	{
		bool flag;
		if (this.m_lastDraftNotification != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.DoCharacterSelectContainerActiveCheck()).MethodHandle;
			}
			flag = this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase();
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		UIManager.SetGameObjectActive(this.m_characterSelectContainer, true, null);
		foreach (UICharacterPanelSelectRankModeButton uicharacterPanelSelectButton in this.m_characterSelectContainer.GetComponentsInChildren<UICharacterPanelSelectRankModeButton>(true))
		{
			bool clickable;
			if (!flag2)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				clickable = (this.SelectedCharacter == CharacterType.None);
			}
			else
			{
				clickable = true;
			}
			uicharacterPanelSelectButton.SetClickable(clickable);
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void UpdateRankData(RankedResolutionPhaseData data, bool updateFromGameInfoUpdate = false)
	{
		if (this.LastGameInfo != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.UpdateRankData(RankedResolutionPhaseData, bool)).MethodHandle;
			}
			this.m_timeInPhase = data.TimeLeftInSubPhase;
			if (!updateFromGameInfoUpdate)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_phaseStartTime = Time.time;
			}
			this.m_IsOnDeck = data.\u001D(this.OurPlayerId);
			this.DoCharacterSelectContainerActiveCheck();
			bool intendedLockInBtnStatus;
			if (this.m_IsOnDeck)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!this.IsBanned(this.HoveredCharacter) && !this.m_selectedCharacterTypes.Contains(this.HoveredCharacter))
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						intendedLockInBtnStatus = this.m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase();
					}
					else
					{
						intendedLockInBtnStatus = true;
					}
					goto IL_DA;
				}
			}
			intendedLockInBtnStatus = false;
			IL_DA:
			this.m_intendedLockInBtnStatus = intendedLockInBtnStatus;
			if (this.m_IsOnDeck)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!updateFromGameInfoUpdate)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					this.HoveredCharacter = data.PlayersOnDeck.Find((RankedResolutionPlayerState p) => p.PlayerId == this.OurPlayerId).Intention;
				}
			}
			else
			{
				this.HoveredCharacter = CharacterType.None;
			}
			this.SetFreelancerSettingButtonsVisible(this.m_currentState >= UIRankedModeDraftScreen.CenterNotification.LoadoutPhase);
			UIManager.SetGameObjectActive(this.m_lockFreelancerContainer, false, null);
			if (this.m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.LastGameInfo.GameStatus == GameStatus.FreelancerSelecting)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!this.m_stateQueues.Contains(UIRankedModeDraftScreen.CenterNotification.TradePhase))
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.TradePhase)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.TradePhase);
						}
					}
				}
			}
			if (!this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int i = 0; i < this.m_blueBans.Length; i++)
				{
					this.m_blueBans[i].SetAsSelecting(false);
					this.m_redBans[i].SetAsSelecting(false);
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			for (int j = 0; j < this.m_blueBans.Length; j++)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (j >= data.FriendlyBans.Count)
				{
					break;
				}
				CharacterType characterType = data.FriendlyBans[j];
				if (this.m_blueBans[j] != null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(characterType);
					if (characterResourceLink != null)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_blueBans[j].SetSelectedCharacterImageVisible(true);
						if (this.m_blueBans[j].GetSelectedCharacter() == CharacterType.None)
						{
							UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeBanPlayer);
							this.CheckSelectedCharForCenterPiece(true, data);
						}
						this.m_blueBans[j].SetCharacter(characterResourceLink);
					}
				}
				if (!this.m_friendlyBannedCharacterTypes.Contains(characterType))
				{
					this.m_friendlyBannedCharacterTypes.Add(characterType);
				}
			}
			for (int k = 0; k < this.m_redBans.Length; k++)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (k >= data.EnemyBans.Count)
				{
					break;
				}
				CharacterType characterType2 = data.EnemyBans[k];
				if (this.m_redBans[k] != null)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(characterType2);
					if (characterResourceLink2 != null)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						this.m_redBans[k].SetSelectedCharacterImageVisible(true);
						if (this.m_redBans[k].GetSelectedCharacter() == CharacterType.None)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeBanPlayer);
							this.CheckSelectedCharForCenterPiece(false, data);
						}
						this.m_redBans[k].SetCharacter(characterResourceLink2);
					}
				}
				if (!this.m_enemyBannedCharacterTypes.Contains(characterType2))
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_enemyBannedCharacterTypes.Add(characterType2);
				}
			}
			long accountId = ClientGameManager.Get().GetPlayerAccountData().AccountId;
			for (int l = 0; l < this.m_blueTeamMembers.Length; l++)
			{
				this.m_blueTeamMembers[l].CanBeTraded = false;
				bool selectedCharacterImageVisible = false;
				if (data.FriendlyTeamSelections.ContainsKey(this.m_blueTeamMembers[l].PlayerID))
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					CharacterResourceLink characterResourceLink3 = GameWideData.Get().GetCharacterResourceLink(data.FriendlyTeamSelections[this.m_blueTeamMembers[l].PlayerID]);
					if (characterResourceLink3 != null)
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						selectedCharacterImageVisible = true;
						if (this.m_blueTeamMembers[l].GetSelectedCharacter() == CharacterType.None)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModePickPlayer);
							this.CheckSelectedCharForCenterPiece(true, data);
						}
						this.m_blueTeamMembers[l].SetCharacter(characterResourceLink3);
						if (!this.IsBanned(characterResourceLink3.m_characterType))
						{
							this.m_friendlyBannedCharacterTypes.Add(characterResourceLink3.m_characterType);
						}
						this.m_blueTeamMembers[l].CanBeTraded = !this.DidPlayerLockInDuringSwapPhase(data, (long)this.m_blueTeamMembers[l].PlayerID);
					}
					if (this.m_blueTeamMembers[l].AccountID == accountId)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						this.SelectedCharacter = this.m_blueTeamMembers[l].GetSelectedCharacter();
					}
				}
				this.m_blueTeamMembers[l].SetTradePhase(this.m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE);
				if (this.m_lastDraftNotification.SubPhase != FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
				{
					this.m_blueTeamMembers[l].SetCharacterLocked(false);
				}
				UIManager.SetGameObjectActive(this.m_blueTeamMembers[l], true, null);
				this.m_blueTeamMembers[l].SetSelectedCharacterImageVisible(selectedCharacterImageVisible);
			}
			for (int m = 0; m < this.m_redTeamMembers.Length; m++)
			{
				bool selectedCharacterImageVisible2 = false;
				if (data.EnemyTeamSelections.ContainsKey(this.m_redTeamMembers[m].PlayerID))
				{
					CharacterResourceLink characterResourceLink4 = GameWideData.Get().GetCharacterResourceLink(data.EnemyTeamSelections[this.m_redTeamMembers[m].PlayerID]);
					if (characterResourceLink4 != null)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						selectedCharacterImageVisible2 = true;
						if (this.m_redTeamMembers[m].GetSelectedCharacter() == CharacterType.None)
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModePickPlayer);
							this.CheckSelectedCharForCenterPiece(false, data);
						}
						this.m_redTeamMembers[m].SetCharacter(characterResourceLink4);
						if (!this.IsBanned(characterResourceLink4.m_characterType))
						{
							this.m_enemyBannedCharacterTypes.Add(characterResourceLink4.m_characterType);
						}
					}
				}
				UIManager.SetGameObjectActive(this.m_redTeamMembers[m], true, null);
				this.m_redTeamMembers[m].SetSelectedCharacterImageVisible(selectedCharacterImageVisible2);
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_currentCharacterPage == -1)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				this.SetPageIndex(0);
			}
			this.CheckCharacterListValidity();
		}
	}

	public Team GetCurrentTeam(RankedResolutionPhaseData data)
	{
		if (this.LastTeamInfo != null && this.LastPlayerInfo != null)
		{
			foreach (LobbyPlayerInfo lobbyPlayerInfo in this.LastTeamInfo.TeamPlayerInfo)
			{
				if (data.\u001D(lobbyPlayerInfo.PlayerId))
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.GetCurrentTeam(RankedResolutionPhaseData)).MethodHandle;
					}
					return lobbyPlayerInfo.TeamId;
				}
			}
			if (this.m_lastDraftNotification == null)
			{
				return Team.Invalid;
			}
			if (!this.m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase())
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
				{
					return Team.Invalid;
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (this.LastPlayerInfo.TeamId == Team.Spectator)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				return Team.TeamA;
			}
			return this.LastPlayerInfo.TeamId.OtherTeam();
		}
		return Team.Invalid;
	}

	private void SetCenterBackground(bool isOnBlue, bool isOnRed)
	{
		for (int i = 0; i < this.singleSelectionBlueTeam.Length; i++)
		{
			UIManager.SetGameObjectActive(this.singleSelectionBlueTeam[i], isOnBlue, null);
		}
		for (int j = 0; j < this.singleSelectionRedTeam.Length; j++)
		{
			UIManager.SetGameObjectActive(this.singleSelectionRedTeam[j], isOnRed, null);
		}
		for (int k = 0; k < this.doubleSelectionBlueTeam.Length; k++)
		{
			UIManager.SetGameObjectActive(this.doubleSelectionBlueTeam[k], isOnBlue, null);
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.SetCenterBackground(bool, bool)).MethodHandle;
		}
		for (int l = 0; l < this.doubleSelectionRedTeam.Length; l++)
		{
			UIManager.SetGameObjectActive(this.doubleSelectionRedTeam[l], isOnRed, null);
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void PrintData(RankedResolutionPhaseData data)
	{
		string text = "LAST RANKED RESOLUTION PHASE DATA!\n";
		text += "Blue team info:\n";
		for (int i = 0; i < this.m_blueTeamMembers.Length; i++)
		{
			using (List<RankedResolutionPlayerState>.Enumerator enumerator = data.UnselectedPlayerStates.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RankedResolutionPlayerState rankedResolutionPlayerState = enumerator.Current;
					if (rankedResolutionPlayerState.PlayerId == this.m_blueTeamMembers[i].PlayerID)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.PrintData(RankedResolutionPhaseData)).MethodHandle;
						}
						text += string.Format("PlayerID is {0}, PlayerName is {1}, Is On deck: {2}\n", rankedResolutionPlayerState.PlayerId, this.m_blueTeamMembers[i].m_playerName, rankedResolutionPlayerState.OnDeckness);
						goto IL_BE;
					}
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			IL_BE:;
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		text += "Red team info:\n";
		for (int j = 0; j < this.m_redTeamMembers.Length; j++)
		{
			using (List<RankedResolutionPlayerState>.Enumerator enumerator2 = data.UnselectedPlayerStates.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					RankedResolutionPlayerState rankedResolutionPlayerState2 = enumerator2.Current;
					if (rankedResolutionPlayerState2.PlayerId == this.m_redTeamMembers[j].PlayerID)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						text += string.Format("PlayerID is {0}, PlayerName is {1}, Is On deck: {2}\n", rankedResolutionPlayerState2.PlayerId, this.m_redTeamMembers[j].m_playerName.text, rankedResolutionPlayerState2.OnDeckness);
						goto IL_195;
					}
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			IL_195:;
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		Debug.Log(text);
	}

	private void UpdateCenter(RankedResolutionPhaseData data, bool updateFromGameInfoUpdate)
	{
		if (this.LastGameInfo != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.UpdateCenter(RankedResolutionPhaseData, bool)).MethodHandle;
			}
			bool flag = false;
			bool flag2 = false;
			int i = 0;
			while (i < this.m_blueTeamMembers.Length)
			{
				if (data.\u001D(this.m_blueTeamMembers[i].PlayerID))
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = true;
					IL_67:
					for (int j = 0; j < this.m_redTeamMembers.Length; j++)
					{
						if (data.\u001D(this.m_redTeamMembers[j].PlayerID))
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							flag2 = true;
							IL_A8:
							bool flag3 = true;
							foreach (RankedResolutionPlayerState rankedResolutionPlayerState in data.PlayersOnDeck)
							{
								if (this.m_playerIDsOnDeck.ContainsKey(rankedResolutionPlayerState.PlayerId))
								{
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									flag3 = false;
								}
							}
							if (this.m_lastDraftNotification.SubPhase == this.m_lastPhaseForUpdateCenter)
							{
								if (!flag3)
								{
									foreach (RankedResolutionPlayerState rankedResolutionPlayerState2 in data.PlayersOnDeck)
									{
										this.m_playerIDsOnDeck[rankedResolutionPlayerState2.PlayerId] = rankedResolutionPlayerState2.Intention;
									}
									goto IL_39C;
								}
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							if (this.m_playerIDsOnDeck.Count > 0)
							{
								for (;;)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								bool flag4 = false;
								bool flag5 = false;
								for (int k = 0; k < this.m_blueTeamMembers.Length; k++)
								{
									if (this.m_playerIDsOnDeck.ContainsKey(this.m_blueTeamMembers[k].PlayerID))
									{
										flag4 = true;
										break;
									}
								}
								for (int l = 0; l < this.m_redTeamMembers.Length; l++)
								{
									if (this.m_playerIDsOnDeck.ContainsKey(this.m_redTeamMembers[l].PlayerID))
									{
										for (;;)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											break;
										}
										flag5 = true;
										break;
									}
								}
								if (flag5)
								{
									if (this.m_playerIDsOnDeck.Count == 1)
									{
										for (;;)
										{
											switch (2)
											{
											case 0:
												continue;
											}
											break;
										}
										this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.RedTeamSingleSelectEnd);
									}
									else
									{
										this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectEnd);
									}
								}
								else if (flag4)
								{
									if (this.m_playerIDsOnDeck.Count == 1)
									{
										this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.BlueTeamSingleSelectEnd);
									}
									else
									{
										this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectEnd);
									}
								}
							}
							if (!this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
							{
								if (this.m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase())
								{
									for (;;)
									{
										switch (3)
										{
										case 0:
											continue;
										}
										break;
									}
								}
								else
								{
									if (this.m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
									{
										this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.TradePhase);
										goto IL_2E7;
									}
									goto IL_2E7;
								}
							}
							UIRankedModeDraftScreen.CenterNotification centerNotification = UIRankedModeDraftScreen.CenterNotification.None;
							int count = data.PlayersOnDeck.Count;
							if (flag2)
							{
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.RedTeamNotification);
								if (count == 1)
								{
									centerNotification = UIRankedModeDraftScreen.CenterNotification.RedTeamSingleSelectStart;
								}
								else if (count == 2)
								{
									centerNotification = UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectStart;
								}
							}
							else if (flag)
							{
								this.QueueCenterState(UIRankedModeDraftScreen.CenterNotification.BlueTeamNotification);
								if (count == 1)
								{
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									centerNotification = UIRankedModeDraftScreen.CenterNotification.BlueTeamSingleSelectStart;
								}
								else if (count == 2)
								{
									for (;;)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									centerNotification = UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectStart;
								}
							}
							if (centerNotification != UIRankedModeDraftScreen.CenterNotification.None)
							{
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								this.QueueCenterState(centerNotification);
							}
							IL_2E7:
							this.m_playerIDsOnDeck.Clear();
							using (List<RankedResolutionPlayerState>.Enumerator enumerator3 = data.PlayersOnDeck.GetEnumerator())
							{
								while (enumerator3.MoveNext())
								{
									RankedResolutionPlayerState rankedResolutionPlayerState3 = enumerator3.Current;
									this.m_playerIDsOnDeck.Add(rankedResolutionPlayerState3.PlayerId, rankedResolutionPlayerState3.Intention);
								}
								for (;;)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							IL_39C:
							if (!updateFromGameInfoUpdate)
							{
								this.UpdateCenterVisuals(data, flag, flag2);
							}
							this.m_lastPhaseForUpdateCenter = this.m_lastDraftNotification.SubPhase;
							return;
						}
					}
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						goto IL_A8;
					}
				}
				else
				{
					i++;
				}
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				goto IL_67;
			}
		}
	}

	private void UpdateCenterVisuals(RankedResolutionPhaseData data, bool isOnBlueTeam, bool isOnRedTeam)
	{
		if (this.m_playerIDsOnDeck.Count == 1)
		{
			foreach (KeyValuePair<int, CharacterType> keyValuePair in this.m_playerIDsOnDeck)
			{
				KeyValuePair<int, CharacterType> selectedChar = keyValuePair;
				RankedResolutionPhaseData data2 = data;
				TextMeshProUGUI singleCharacterName = this.m_singleCharacterName;
				Image noCharacter = this.singleNoSelectionCharacter;
				Image browseCharacter = this.singleBrowseSelectionCharacter;
				Image selectedCharacter = this.singleSelectionCharacter;
				Animator singleSelectionCharacterSelected = this.m_singleSelectionCharacterSelected;
				Animator selectedCharacterNameAnimator = (!isOnBlueTeam) ? this.m_singleRedTeamSelectionCharacterSelected : this.m_singleBlueSelectionCharacterSelected;
				TextMeshProUGUI selectedCharacterText;
				if (isOnBlueTeam)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.UpdateCenterVisuals(RankedResolutionPhaseData, bool, bool)).MethodHandle;
					}
					selectedCharacterText = this.m_singleBlueTeamSelectedCharacter;
				}
				else
				{
					selectedCharacterText = this.m_singleRedTeamSelectedCharacter;
				}
				this.SetupSelection(selectedChar, data2, singleCharacterName, noCharacter, browseCharacter, selectedCharacter, singleSelectionCharacterSelected, selectedCharacterNameAnimator, selectedCharacterText, (!isOnBlueTeam) ? this.m_singleRedTeamPlayerName : this.m_singleBlueTeamPlayerName, isOnBlueTeam, isOnRedTeam);
			}
		}
		else if (this.m_playerIDsOnDeck.Count == 2)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			bool flag = true;
			using (Dictionary<int, CharacterType>.Enumerator enumerator2 = this.m_playerIDsOnDeck.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					KeyValuePair<int, CharacterType> keyValuePair2 = enumerator2.Current;
					if (flag)
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						KeyValuePair<int, CharacterType> selectedChar2 = keyValuePair2;
						RankedResolutionPhaseData data3 = data;
						TextMeshProUGUI leftCharacterName = this.m_leftCharacterName;
						Image noCharacter2 = this.doubleNoSelectionLeftCharacter;
						Image browseCharacter2 = this.doubleBrowseSelectionLeftCharacter;
						Image selectedCharacter2 = this.doubleSelectionLeftCharacter;
						Animator selectedCharacterAnimator;
						if (this.m_playerIDsOnDeck.Count == data.PlayersOnDeck.Count)
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							selectedCharacterAnimator = this.m_doubleLeftSelectionCharacterSelected;
						}
						else
						{
							selectedCharacterAnimator = null;
						}
						Animator selectedCharacterNameAnimator2;
						if (isOnBlueTeam)
						{
							for (;;)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							selectedCharacterNameAnimator2 = this.m_doubleLeftBlueSelectionCharacterSelected;
						}
						else
						{
							selectedCharacterNameAnimator2 = this.m_doubleLeftRedTeamSelectionCharacterSelected;
						}
						TextMeshProUGUI selectedCharacterText2;
						if (isOnBlueTeam)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							selectedCharacterText2 = this.m_doubleLeftBlueTeamSelectedCharacter;
						}
						else
						{
							selectedCharacterText2 = this.m_doubleLeftRedTeamSelectedCharacter;
						}
						this.SetupSelection(selectedChar2, data3, leftCharacterName, noCharacter2, browseCharacter2, selectedCharacter2, selectedCharacterAnimator, selectedCharacterNameAnimator2, selectedCharacterText2, (!isOnBlueTeam) ? this.m_doubleLeftRedTeamPlayerName : this.m_doubleLeftBlueTeamPlayerName, isOnBlueTeam, isOnRedTeam);
						flag = false;
					}
					else
					{
						KeyValuePair<int, CharacterType> selectedChar3 = keyValuePair2;
						RankedResolutionPhaseData data4 = data;
						TextMeshProUGUI rightCharacterName = this.m_rightCharacterName;
						Image noCharacter3 = this.doubleNoSelectionRightCharacter;
						Image browseCharacter3 = this.doubleBrowseSelectionRightCharacter;
						Image selectedCharacter3 = this.doubleSelectionRightCharacter;
						Animator selectedCharacterAnimator2;
						if (this.m_playerIDsOnDeck.Count == data.PlayersOnDeck.Count)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							selectedCharacterAnimator2 = this.m_doubleRightSelectionCharacterSelected;
						}
						else
						{
							selectedCharacterAnimator2 = null;
						}
						Animator selectedCharacterNameAnimator3;
						if (isOnBlueTeam)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							selectedCharacterNameAnimator3 = this.m_doubleRightBlueSelectionCharacterSelected;
						}
						else
						{
							selectedCharacterNameAnimator3 = this.m_doubleRightRedTeamSelectionCharacterSelected;
						}
						TextMeshProUGUI selectedCharacterText3;
						if (isOnBlueTeam)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							selectedCharacterText3 = this.m_doubleRightBlueTeamSelectedCharacter;
						}
						else
						{
							selectedCharacterText3 = this.m_doubleRightRedTeamSelectedCharacter;
						}
						this.SetupSelection(selectedChar3, data4, rightCharacterName, noCharacter3, browseCharacter3, selectedCharacter3, selectedCharacterAnimator2, selectedCharacterNameAnimator3, selectedCharacterText3, (!isOnBlueTeam) ? this.m_doubleRightRedTeamPlayerName : this.m_doubleRightBlueTeamPlayerName, isOnBlueTeam, isOnRedTeam);
					}
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
	}

	public void SetupFreelancerSelect(RankedResolutionPhaseData data)
	{
		if (this.LastGameInfo != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.SetupFreelancerSelect(RankedResolutionPhaseData)).MethodHandle;
			}
			if (this.HoveredCharacter != this.m_selectedSubPhaseCharacter)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_selectedSubPhaseCharacter != CharacterType.None)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (data.\u001D(this.LastPlayerInfo.PlayerId))
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!this.m_selectedCharacterTypes.Contains(this.m_selectedSubPhaseCharacter))
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!this.IsBanned(this.m_selectedSubPhaseCharacter))
							{
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								ClientGameManager.Get().UpdateSelectedCharacter(this.m_selectedSubPhaseCharacter, 0);
								ClientGameManager.Get().SendRankedHoverClickRequest(this.m_selectedSubPhaseCharacter);
								this.m_intendedLockInBtnStatus = true;
								this.HoveredCharacter = this.m_selectedSubPhaseCharacter;
							}
						}
					}
				}
			}
		}
	}

	private bool CharacterSelectAnimIsPlaying()
	{
		if (!this.m_doubleLeftSelectionCharacterSelected.gameObject.activeSelf)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.CharacterSelectAnimIsPlaying()).MethodHandle;
			}
			if (!this.m_doubleRightSelectionCharacterSelected.gameObject.activeSelf)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				return this.m_singleSelectionCharacterSelected.gameObject.activeSelf;
			}
		}
		return true;
	}

	private void SetupSelection(KeyValuePair<int, CharacterType> selectedChar, RankedResolutionPhaseData data, TextMeshProUGUI nameDisplay, Image NoCharacter, Image BrowseCharacter, Image SelectedCharacter, Animator SelectedCharacterAnimator, Animator SelectedCharacterNameAnimator, TextMeshProUGUI SelectedCharacterText, TextMeshProUGUI PlayerName, bool isFriendly, bool isEnemy)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = true;
		if (!this.m_selectedCharacterTypes.Contains(selectedChar.Value))
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.SetupSelection(KeyValuePair<int, CharacterType>, RankedResolutionPhaseData, TextMeshProUGUI, Image, Image, Image, Animator, Animator, TextMeshProUGUI, TextMeshProUGUI, bool, bool)).MethodHandle;
			}
			if (!this.IsBanned(selectedChar.Value) && !data.FriendlyBans.Contains(selectedChar.Value))
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!data.EnemyBans.Contains(selectedChar.Value))
				{
					goto IL_88;
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		flag3 = false;
		IL_88:
		if (isFriendly)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			bool flag4;
			if (this.m_lastSetupSelectionPhaseSubType.IsPickBanSubPhase())
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				flag4 = data.FriendlyBans.Contains(selectedChar.Value);
			}
			else if (this.m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				flag4 = data.FriendlyTeamSelections.ContainsKey(selectedChar.Key);
			}
			else
			{
				flag4 = false;
			}
			if (flag4)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				CharacterType value = selectedChar.Value;
				if (value != CharacterType.None)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = true;
					bool flag5 = true;
					if (this.m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
					{
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.m_playerIDsThatSelected.Contains(selectedChar.Key))
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							flag5 = false;
						}
					}
					if (flag5)
					{
						SelectedCharacterText.text = value.GetDisplayName();
						SelectedCharacter.sprite = GameWideData.Get().GetCharacterResourceLink(value).ActorDataPrefab.GetComponent<ActorData>().GetAliveHUDIcon();
						UIManager.SetGameObjectActive(NoCharacter, false, null);
						UIManager.SetGameObjectActive(BrowseCharacter, false, null);
						UIManager.SetGameObjectActive(SelectedCharacter, true, null);
						if (SelectedCharacterAnimator != null)
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							UIManager.SetGameObjectActive(SelectedCharacterAnimator, true, null);
							this.m_playerIDBeingAnimated = selectedChar.Key;
							this.m_animatorCurrentlyAnimating = SelectedCharacterAnimator;
						}
						UIManager.SetGameObjectActive(SelectedCharacterNameAnimator, true, null);
						UIManager.SetGameObjectActive(SelectedCharacterNameAnimator.transform.parent, true, null);
						if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectStart)
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.BlueTeamSingleSelectStart)
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!this.m_stateQueues.Contains(UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectStart))
								{
									for (;;)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									if (!this.m_stateQueues.Contains(UIRankedModeDraftScreen.CenterNotification.BlueTeamSingleSelectStart))
									{
										goto IL_2CA;
									}
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
								}
								while (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.BlueTeamDoubleSelectStart)
								{
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									if (this.m_currentState == UIRankedModeDraftScreen.CenterNotification.BlueTeamSingleSelectStart)
									{
										for (;;)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											goto IL_2BE;
										}
									}
									else
									{
										this.m_currentState = this.m_stateQueues[0];
										this.m_stateQueues.RemoveAt(0);
									}
								}
								IL_2BE:
								this.DoQueueState(this.m_currentState);
							}
						}
					}
					IL_2CA:
					if (this.m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!this.m_playerIDsThatSelected.Contains(selectedChar.Key))
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_playerIDsThatSelected.Add(selectedChar.Key);
							for (int i = 0; i < this.m_blueTeamMembers.Length; i++)
							{
								if (this.m_blueTeamMembers[i].PlayerID == selectedChar.Key)
								{
									for (;;)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									flag2 = true;
									PlayerName.text = this.m_blueTeamMembers[i].m_playerName.text;
								}
							}
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
						}
					}
				}
			}
		}
		if (isEnemy)
		{
			bool flag6;
			if (this.m_lastSetupSelectionPhaseSubType.IsPickBanSubPhase())
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				flag6 = data.EnemyBans.Contains(selectedChar.Value);
			}
			else if (this.m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				flag6 = data.EnemyTeamSelections.ContainsKey(selectedChar.Key);
			}
			else
			{
				flag6 = false;
			}
			if (flag6)
			{
				CharacterType value2 = selectedChar.Value;
				if (value2 != CharacterType.None)
				{
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = true;
					bool flag7 = true;
					if (this.m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.m_playerIDsThatSelected.Contains(selectedChar.Key))
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							flag7 = false;
						}
					}
					if (flag7)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						SelectedCharacterText.text = value2.GetDisplayName();
						SelectedCharacter.sprite = GameWideData.Get().GetCharacterResourceLink(value2).ActorDataPrefab.GetComponent<ActorData>().GetAliveHUDIcon();
						UIManager.SetGameObjectActive(NoCharacter, false, null);
						UIManager.SetGameObjectActive(BrowseCharacter, false, null);
						UIManager.SetGameObjectActive(SelectedCharacter, true, null);
						if (SelectedCharacterAnimator != null)
						{
							UIManager.SetGameObjectActive(SelectedCharacterAnimator, true, null);
							this.m_playerIDBeingAnimated = selectedChar.Key;
							this.m_animatorCurrentlyAnimating = SelectedCharacterAnimator;
						}
						UIManager.SetGameObjectActive(SelectedCharacterNameAnimator, true, null);
						UIManager.SetGameObjectActive(SelectedCharacterNameAnimator.transform.parent, true, null);
						if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectStart)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.RedTeamSingleSelectStart)
							{
								for (;;)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!this.m_stateQueues.Contains(UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectStart))
								{
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									if (!this.m_stateQueues.Contains(UIRankedModeDraftScreen.CenterNotification.RedTeamSingleSelectStart))
									{
										goto IL_5A8;
									}
								}
								while (this.m_currentState != UIRankedModeDraftScreen.CenterNotification.RedTeamDoubleSelectStart)
								{
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									if (this.m_currentState == UIRankedModeDraftScreen.CenterNotification.RedTeamSingleSelectStart)
									{
										for (;;)
										{
											switch (4)
											{
											case 0:
												continue;
											}
											goto IL_59C;
										}
									}
									else
									{
										this.m_currentState = this.m_stateQueues[0];
										this.m_stateQueues.RemoveAt(0);
									}
								}
								IL_59C:
								this.DoQueueState(this.m_currentState);
							}
						}
					}
					IL_5A8:
					if (this.m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase() && !this.m_playerIDsThatSelected.Contains(selectedChar.Key))
					{
						this.m_playerIDsThatSelected.Add(selectedChar.Key);
					}
				}
			}
		}
		if (!flag2)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			bool flag8 = true;
			if (this.m_playerIDsThatSelected.Count > 0 && this.m_playerIDsThatSelected[this.m_playerIDsThatSelected.Count - 1] == selectedChar.Key)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				flag8 = false;
			}
			if (this.m_animatorCurrentlyAnimating != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_animatorCurrentlyAnimating.gameObject.activeInHierarchy)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_lastSetupSelectionPhaseSubType == this.m_lastDraftNotification.SubPhase)
					{
						for (int j = 0; j < this.m_blueTeamMembers.Length; j++)
						{
							if (this.m_blueTeamMembers[j].PlayerID == this.m_playerIDBeingAnimated)
							{
								for (;;)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								flag8 = false;
								PlayerName.text = this.m_blueTeamMembers[j].m_playerName.text;
							}
						}
					}
				}
			}
			if (flag8)
			{
				PlayerName.text = string.Empty;
			}
		}
		this.m_lastSetupSelectionPhaseSubType = this.m_lastDraftNotification.SubPhase;
		if (!flag && !this.CharacterSelectAnimIsPlaying())
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (selectedChar.Value != CharacterType.None && flag3)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				BrowseCharacter.sprite = GameWideData.Get().GetCharacterResourceLink(selectedChar.Value).ActorDataPrefab.GetComponent<ActorData>().GetAliveHUDIcon();
				UIManager.SetGameObjectActive(NoCharacter, false, null);
				UIManager.SetGameObjectActive(BrowseCharacter, true, null);
				UIManager.SetGameObjectActive(SelectedCharacter, false, null);
			}
			else
			{
				UIManager.SetGameObjectActive(NoCharacter, true, null);
				UIManager.SetGameObjectActive(BrowseCharacter, false, null);
				UIManager.SetGameObjectActive(SelectedCharacter, false, null);
			}
		}
		if (!flag)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag3 && !this.CharacterSelectAnimIsPlaying())
			{
				UIManager.SetGameObjectActive(NoCharacter, true, null);
				UIManager.SetGameObjectActive(BrowseCharacter, false, null);
				UIManager.SetGameObjectActive(SelectedCharacter, false, null);
			}
		}
		nameDisplay.text = string.Empty;
	}

	public void SetupBanSelect(RankedResolutionPhaseData data)
	{
		if (this.LastGameInfo != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.SetupBanSelect(RankedResolutionPhaseData)).MethodHandle;
			}
			Team currentTeam = this.GetCurrentTeam(data);
			for (int i = 0; i < this.m_blueBans.Length; i++)
			{
				UIManager.SetGameObjectActive(this.m_blueBans[i], true, null);
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			for (int j = 0; j < this.m_redBans.Length; j++)
			{
				UIManager.SetGameObjectActive(this.m_redBans[j], true, null);
			}
			if (data.PlayersOnDeck.Count > 0)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				foreach (RankedResolutionPlayerState rankedResolutionPlayerState in data.PlayersOnDeck)
				{
					if (rankedResolutionPlayerState.Intention != CharacterType.None)
					{
						CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(rankedResolutionPlayerState.Intention);
						if (this.GetCurrentTeam(data) == this.LastPlayerInfo.TeamId)
						{
							goto IL_106;
						}
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (this.LastPlayerInfo.TeamId == Team.Spectator)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								goto IL_106;
							}
						}
						else if (data.EnemyBans.Count < this.m_redBans.Length)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_redBans[data.EnemyBans.Count].SetBrowseCharacterImageVisible(true);
							this.m_redBans[data.EnemyBans.Count].SetHoverCharacter(characterResourceLink);
						}
						continue;
						IL_106:
						if (data.FriendlyBans.Count < this.m_blueBans.Length)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_blueBans[data.FriendlyBans.Count].SetBrowseCharacterImageVisible(true);
							this.m_blueBans[data.FriendlyBans.Count].SetHoverCharacter(characterResourceLink);
						}
					}
					else
					{
						if (data.FriendlyBans.Count < this.m_blueBans.Length)
						{
							this.m_blueBans[data.FriendlyBans.Count].SetBrowseCharacterImageVisible(false);
						}
						if (data.EnemyBans.Count < this.m_redBans.Length)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_redBans[data.EnemyBans.Count].SetBrowseCharacterImageVisible(false);
						}
					}
				}
			}
			int k = 0;
			while (k < this.m_redBans.Length)
			{
				this.m_redBans[k].SetSelectedCharacterImageVisible(data.EnemyBans.Count > k);
				if (currentTeam == this.LastPlayerInfo.TeamId)
				{
					goto IL_29C;
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.LastPlayerInfo.TeamId == Team.Spectator)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						goto IL_29C;
					}
				}
				else
				{
					this.m_redBans[k].SetAsSelecting(data.EnemyBans.Count == k);
				}
				IL_2CD:
				k++;
				continue;
				IL_29C:
				this.m_redBans[k].SetAsSelecting(false);
				goto IL_2CD;
			}
			int l = 0;
			while (l < this.m_blueBans.Length)
			{
				this.m_blueBans[l].SetSelectedCharacterImageVisible(data.FriendlyBans.Count > l);
				if (currentTeam == this.LastPlayerInfo.TeamId)
				{
					goto IL_338;
				}
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.LastPlayerInfo.TeamId == Team.Spectator)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						goto IL_338;
					}
				}
				else
				{
					this.m_blueBans[l].SetAsSelecting(false);
				}
				IL_367:
				l++;
				continue;
				IL_338:
				this.m_blueBans[l].SetAsSelecting(data.FriendlyBans.Count == l);
				goto IL_367;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	private string SubphaseToDisplayName(FreelancerResolutionPhaseSubType subPhase, TeamType teamType, bool isSelf)
	{
		string result = string.Empty;
		switch (subPhase)
		{
		case FreelancerResolutionPhaseSubType.PICK_BANS1:
		case FreelancerResolutionPhaseSubType.PICK_BANS2:
			if (isSelf)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.SubphaseToDisplayName(FreelancerResolutionPhaseSubType, TeamType, bool)).MethodHandle;
				}
				result = StringUtil.TR("SelectFreelancerBan", "RankMode");
			}
			else if (teamType == TeamType.Ally)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				result = StringUtil.TR("WaitingBlueTeamBan", "RankMode");
			}
			else if (teamType == TeamType.Enemy)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				result = StringUtil.TR("WaitingRedTeamBan", "RankMode");
			}
			break;
		case FreelancerResolutionPhaseSubType.PICK_FREELANCER1:
		case FreelancerResolutionPhaseSubType.PICK_FREELANCER2:
			if (isSelf)
			{
				result = StringUtil.TR("SelectFreelancer", "RankMode");
			}
			else if (teamType == TeamType.Ally)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				result = StringUtil.TR("WaitingBlueTeamSelect", "RankMode");
			}
			else if (teamType == TeamType.Enemy)
			{
				result = StringUtil.TR("WaitingRedTeamSelect", "RankMode");
			}
			break;
		}
		return result;
	}

	internal int OurPlayerId
	{
		get
		{
			long accountId = ClientGameManager.Get().GetPlayerAccountData().AccountId;
			foreach (UIRankedModePlayerDraftEntry uirankedModePlayerDraftEntry in this.m_blueTeamMembers)
			{
				if (uirankedModePlayerDraftEntry.AccountID == accountId)
				{
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.get_OurPlayerId()).MethodHandle;
					}
					return uirankedModePlayerDraftEntry.PlayerID;
				}
			}
			return -1;
		}
	}

	public void SetupInstructions(RankedResolutionPhaseData data)
	{
		Team currentTeam = this.GetCurrentTeam(data);
		TeamType teamType = TeamType.Any;
		if (currentTeam != Team.TeamA)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.SetupInstructions(RankedResolutionPhaseData)).MethodHandle;
			}
			if (currentTeam != Team.TeamB)
			{
				this.m_MessageText.color = this.m_neutralColor;
				goto IL_9E;
			}
		}
		if (currentTeam != this.LastPlayerInfo.TeamId)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.LastPlayerInfo.TeamId == Team.Spectator)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (currentTeam == Team.TeamA)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						goto IL_76;
					}
				}
			}
			this.m_MessageText.color = this.m_redTeamColor;
			teamType = TeamType.Enemy;
			goto IL_9E;
		}
		IL_76:
		this.m_MessageText.color = this.m_blueTeamColor;
		teamType = TeamType.Ally;
		IL_9E:
		if (this.LastGameInfo != null && this.LastGameInfo.GameStatus != GameStatus.Stopped)
		{
			if (this.LastGameInfo.GameStatus > GameStatus.FreelancerSelecting)
			{
				return;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_MessageText.text = this.SubphaseToDisplayName(this.m_lastDraftNotification.SubPhase, teamType, data.\u001D(this.OurPlayerId));
	}

	public void NotifyFreelancerTrades(RankedResolutionPhaseData data)
	{
		long accountId = ClientGameManager.Get().GetPlayerAccountData().AccountId;
		int num = -1;
		bool selfLockedIn = false;
		for (int i = 0; i < this.m_blueTeamMembers.Length; i++)
		{
			if (this.m_blueTeamMembers[i].AccountID == accountId)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.NotifyFreelancerTrades(RankedResolutionPhaseData)).MethodHandle;
				}
				num = this.m_blueTeamMembers[i].PlayerID;
				selfLockedIn = this.DidPlayerLockInDuringSwapPhase(data, (long)this.m_blueTeamMembers[i].PlayerID);
			}
			this.m_blueTeamMembers[i].SetAsSelecting(false);
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		for (int j = 0; j < this.m_redTeamMembers.Length; j++)
		{
			this.m_redTeamMembers[j].SetAsSelecting(false);
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		for (int k = 0; k < this.m_blueTeamMembers.Length; k++)
		{
			UIRankedModePlayerDraftEntry.TradeStatus status = UIRankedModePlayerDraftEntry.TradeStatus.NoTrade;
			using (List<RankedTradeData>.Enumerator enumerator = data.TradeActions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RankedTradeData rankedTradeData = enumerator.Current;
					if (rankedTradeData.TradeAction == RankedTradeData.TradeActionType.\u001D)
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (rankedTradeData.AskedPlayerId == this.m_blueTeamMembers[k].PlayerID)
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							if (rankedTradeData.OfferingPlayerId == num)
							{
								for (;;)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								status = UIRankedModePlayerDraftEntry.TradeStatus.TradeRequestSent;
								goto IL_1F4;
							}
						}
						if (rankedTradeData.AskedPlayerId == num)
						{
							for (;;)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							if (rankedTradeData.OfferingPlayerId == this.m_blueTeamMembers[k].PlayerID)
							{
								for (;;)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								status = UIRankedModePlayerDraftEntry.TradeStatus.TradeRequestReceived;
								goto IL_1F4;
							}
						}
						continue;
					}
					if (rankedTradeData.TradeAction != RankedTradeData.TradeActionType.\u0012)
					{
						continue;
					}
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (rankedTradeData.OfferingPlayerId != this.m_blueTeamMembers[k].PlayerID)
					{
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						if (rankedTradeData.AskedPlayerId != this.m_blueTeamMembers[k].PlayerID)
						{
							continue;
						}
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					status = UIRankedModePlayerDraftEntry.TradeStatus.StopTrading;
					IL_1F4:
					goto IL_204;
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			IL_204:
			this.m_blueTeamMembers[k].SetTradeStatus(status, this.m_blueTeamMembers[k].PlayerID == num, selfLockedIn);
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private string GameStatusToDisplayString(GameStatus status)
	{
		string result = string.Empty;
		switch (status)
		{
		case GameStatus.Launching:
		case GameStatus.Launched:
		case GameStatus.Connecting:
		case GameStatus.Connected:
		case GameStatus.Authenticated:
		case GameStatus.Loading:
		case GameStatus.Loaded:
			result = StringUtil.TR("LaunchingGame", "RankMode");
			break;
		}
		return result;
	}

	private void UpdateGameLaunching(GameInfoNotification notification)
	{
		this.GameIsLaunching = true;
		this.m_MessageText.text = this.GameStatusToDisplayString(notification.GameInfo.GameStatus);
		for (int i = 0; i < this.m_blueTeamMembers.Length; i++)
		{
			this.m_blueTeamMembers[i].SetTradePhase(false);
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.UpdateGameLaunching(GameInfoNotification)).MethodHandle;
		}
		UIManager.SetGameObjectActive(this.m_lockFreelancerContainer, false, null);
		if (notification.GameInfo.GameStatus >= GameStatus.Launching && notification.GameInfo.GameStatus != GameStatus.Stopped)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			this.SetFreelancerSettingButtonsVisible(false);
			UIRankedCharacterSelectSettingsPanel.Get().SetVisible(false, UICharacterSelectCharacterSettingsPanel.TabPanel.None);
		}
	}

	public void UpdateNotification(EnterFreelancerResolutionPhaseNotification notification, bool updateFromGameInfoUpdate = false)
	{
		if (this.GameIsLaunching)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.UpdateNotification(EnterFreelancerResolutionPhaseNotification, bool)).MethodHandle;
			}
			return;
		}
		if (notification != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (notification.RankedData != null)
			{
				RankedResolutionPhaseData value = notification.RankedData.Value;
				this.SetupInstructions(value);
				this.UpdateRankData(value, updateFromGameInfoUpdate);
				this.UpdatePlayerSelecting(value);
				switch (notification.SubPhase)
				{
				case FreelancerResolutionPhaseSubType.PICK_BANS1:
				case FreelancerResolutionPhaseSubType.PICK_BANS2:
					this.SetupBanSelect(value);
					break;
				case FreelancerResolutionPhaseSubType.PICK_FREELANCER1:
				case FreelancerResolutionPhaseSubType.PICK_FREELANCER2:
					this.SetupFreelancerSelect(value);
					break;
				case FreelancerResolutionPhaseSubType.FREELANCER_TRADE:
					this.NotifyFreelancerTrades(value);
					break;
				}
				this.UpdateHoverStatus(value);
				this.UpdateCenter(value, updateFromGameInfoUpdate);
				return;
			}
		}
	}

	public void NotifyButtonClicked(UICharacterPanelSelectRankModeButton btn)
	{
		if (this.m_IsOnDeck)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.NotifyButtonClicked(UICharacterPanelSelectRankModeButton)).MethodHandle;
			}
			bool intendedLockInBtnStatus = false;
			this.m_selectedSubPhaseCharacter = CharacterType.None;
			for (int i = 0; i < this.m_characterListDisplayButtons.Count; i++)
			{
				if (this.m_characterListDisplayButtons[i] == btn)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_selectedSubPhaseCharacter = this.m_characterListDisplayButtons[i].m_characterType;
					this.m_characterListDisplayButtons[i].SetSelected(true);
					if (!this.m_selectedCharacterTypes.Contains(this.m_characterListDisplayButtons[i].m_characterType))
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!this.IsBanned(this.m_characterListDisplayButtons[i].m_characterType))
						{
							this.HoveredCharacter = this.m_selectedSubPhaseCharacter;
							RankedResolutionPhaseData? rankedData = this.m_lastDraftNotification.RankedData;
							RankedResolutionPhaseData value = rankedData.Value;
							if (this.m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase())
							{
								this.UpdateHoverSelfStatus(value);
							}
							if (!this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
							{
								ClientGameManager.Get().UpdateSelectedCharacter(this.m_selectedSubPhaseCharacter, 0);
							}
							ClientGameManager.Get().SendRankedHoverClickRequest(this.m_selectedSubPhaseCharacter);
							intendedLockInBtnStatus = true;
						}
					}
				}
				else
				{
					this.m_characterListDisplayButtons[i].SetSelected(false);
				}
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_intendedLockInBtnStatus = intendedLockInBtnStatus;
		}
		else
		{
			this.m_selectedSubPhaseCharacter = CharacterType.None;
			for (int j = 0; j < this.m_characterListDisplayButtons.Count; j++)
			{
				if (this.m_characterListDisplayButtons[j] == btn)
				{
					this.m_selectedSubPhaseCharacter = this.m_characterListDisplayButtons[j].m_characterType;
					this.m_characterListDisplayButtons[j].SetSelected(true);
					if (!this.m_selectedCharacterTypes.Contains(this.m_characterListDisplayButtons[j].m_characterType))
					{
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						if (!this.IsBanned(this.m_characterListDisplayButtons[j].m_characterType))
						{
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							ClientGameManager.Get().UpdateSelectedCharacter(this.m_selectedSubPhaseCharacter, 0);
							ClientGameManager.Get().SendRankedHoverClickRequest(this.m_selectedSubPhaseCharacter);
							this.SetupCharacterSettings(this.m_selectedSubPhaseCharacter);
							this.SetFreelancerSettingButtonsVisible(this.m_currentState >= UIRankedModeDraftScreen.CenterNotification.LoadoutPhase);
						}
					}
				}
				else
				{
					this.m_characterListDisplayButtons[j].SetSelected(false);
				}
			}
		}
	}

	public void HandleResolvingDuplicateFreelancerNotification(EnterFreelancerResolutionPhaseNotification notification)
	{
		this.Initialize();
		this.m_lastDraftNotification = notification;
		this.SetupPlayerLists();
		this.UpdateNotification(notification, false);
	}

	private void GetListOfVisibleCharacterTypes()
	{
		this.m_validCharacterTypes.Clear();
		GameManager gameManager = GameManager.Get();
		for (int i = 0; i < 0x28; i++)
		{
			CharacterType characterType = (CharacterType)i;
			if (gameManager.IsCharacterAllowedForPlayers(characterType))
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.GetListOfVisibleCharacterTypes()).MethodHandle;
				}
				if (gameManager.IsCharacterAllowedForGameType(characterType, GameType.Ranked, null, null))
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					this.m_validCharacterTypes.Add(characterType);
				}
			}
		}
	}

	private void Initialize()
	{
		if (this.m_initialized)
		{
			return;
		}
		this.GameIsLaunching = false;
		this.m_lastSetupSelectionPhaseSubType = FreelancerResolutionPhaseSubType.UNDEFINED;
		this.m_assignedCharacterForGame = CharacterType.None;
		this.m_hoverCharacterForGame = CharacterType.None;
		this.m_selectedSubPhaseCharacter = CharacterType.None;
		UICharacterSelectCharacterSettingsPanel uicharacterSelectCharacterSettingsPanel = UIRankedCharacterSelectSettingsPanel.Get();
		if (uicharacterSelectCharacterSettingsPanel != null)
		{
			uicharacterSelectCharacterSettingsPanel.SetVisible(false, UICharacterSelectCharacterSettingsPanel.TabPanel.None);
		}
		this.m_selectedCharacterTypes.Clear();
		this.m_enemyBannedCharacterTypes.Clear();
		this.m_friendlyBannedCharacterTypes.Clear();
		this.m_playerIDsThatSelected.Clear();
		this.GetListOfVisibleCharacterTypes();
		this.m_initialized = true;
		this.m_MessageText.text = string.Empty;
		this.m_gameCountdownTimer.text = string.Empty;
		this.m_redCountdownTimer.text = string.Empty;
		this.m_blueCountdownTimer.text = string.Empty;
		this.m_stageText.text = string.Empty;
		this.ClearAllStates();
		UIManager.SetGameObjectActive(this.m_pagesContainer, false, null);
		UIManager.SetGameObjectActive(this.m_lockFreelancerContainer, false, null);
		this.m_intendedLockInBtnStatus = false;
		this.m_currentCharacterPage = -1;
		this.m_currentVisiblePage = -1;
		for (int i = 0; i < this.m_blueBans.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_blueBans[i], false, null);
			this.m_blueBans[i].Init();
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.Initialize()).MethodHandle;
		}
		for (int j = 0; j < this.m_redBans.Length; j++)
		{
			UIManager.SetGameObjectActive(this.m_redBans[j], false, null);
			this.m_redBans[j].Init();
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		for (int k = 0; k < this.m_blueTeamMembers.Length; k++)
		{
			this.m_blueTeamMembers[k].Init();
			this.m_blueTeamMembers[k].SetTradePhase(false);
		}
		for (int l = 0; l < this.m_redTeamMembers.Length; l++)
		{
			this.m_redTeamMembers[l].Init();
			this.m_redTeamMembers[l].SetTradePhase(false);
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		for (int m = 0; m < this.m_characterListDisplayButtons.Count; m++)
		{
			UnityEngine.Object.Destroy(this.m_characterListDisplayButtons[m].gameObject);
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		this.m_pageButtons.Clear();
		this.m_characterListDisplayButtons.Clear();
		CharacterType[] array = (CharacterType[])Enum.GetValues(typeof(CharacterType));
		List<CharacterType> list = new List<CharacterType>();
		List<CharacterType> list2 = new List<CharacterType>();
		List<CharacterType> list3 = new List<CharacterType>();
		for (int n = 0; n < array.Length; n++)
		{
			try
			{
				if (array[n] != CharacterType.TestFreelancer1 && array[n] != CharacterType.TestFreelancer2)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (array[n] == CharacterType.None)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					else
					{
						CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(array[n]);
						if (characterResourceLink.m_characterRole == CharacterRole.Assassin)
						{
							list.Add(array[n]);
						}
						if (characterResourceLink.m_characterRole == CharacterRole.Support)
						{
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							list3.Add(array[n]);
						}
						if (characterResourceLink.m_characterRole == CharacterRole.Tank)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							list2.Add(array[n]);
						}
					}
				}
			}
			catch
			{
			}
		}
		list.Sort(new Comparison<CharacterType>(this.CompareCharacterTypeName));
		list2.Sort(new Comparison<CharacterType>(this.CompareCharacterTypeName));
		list3.Sort(new Comparison<CharacterType>(this.CompareCharacterTypeName));
		int num = Mathf.CeilToInt((float)list.Count / 2f);
		int num2 = Mathf.CeilToInt((float)list2.Count / 2f);
		int num3 = Mathf.CeilToInt((float)list3.Count / 2f);
		int num4 = 0;
		for (int num5 = 0; num5 < list.Count; num5++)
		{
			UICharacterPanelSelectRankModeButton uicharacterPanelSelectRankModeButton = UnityEngine.Object.Instantiate<UICharacterPanelSelectButton>(this.m_characterSelectBtnPrefab) as UICharacterPanelSelectRankModeButton;
			uicharacterPanelSelectRankModeButton.m_characterType = list[num5];
			if (num5 - num4 * num >= num)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				num4++;
			}
			UIManager.ReparentTransform(uicharacterPanelSelectRankModeButton.gameObject.transform, this.m_firePowerLayoutGroup.gameObject.transform);
			this.m_characterListDisplayButtons.Add(uicharacterPanelSelectRankModeButton);
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		num4 = 0;
		for (int num6 = 0; num6 < list2.Count; num6++)
		{
			UICharacterPanelSelectRankModeButton uicharacterPanelSelectRankModeButton2 = UnityEngine.Object.Instantiate<UICharacterPanelSelectButton>(this.m_characterSelectBtnPrefab) as UICharacterPanelSelectRankModeButton;
			uicharacterPanelSelectRankModeButton2.m_characterType = list2[num6];
			if (num6 - num4 * num2 >= num2)
			{
				num4++;
			}
			UIManager.ReparentTransform(uicharacterPanelSelectRankModeButton2.gameObject.transform, this.m_frontlinerLayoutGroup.gameObject.transform);
			this.m_characterListDisplayButtons.Add(uicharacterPanelSelectRankModeButton2);
		}
		num4 = 0;
		for (int num7 = 0; num7 < list3.Count; num7++)
		{
			UICharacterPanelSelectRankModeButton uicharacterPanelSelectRankModeButton3 = UnityEngine.Object.Instantiate<UICharacterPanelSelectButton>(this.m_characterSelectBtnPrefab) as UICharacterPanelSelectRankModeButton;
			uicharacterPanelSelectRankModeButton3.m_characterType = list3[num7];
			if (num7 - num4 * num3 >= num3)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				num4++;
			}
			UIManager.ReparentTransform(uicharacterPanelSelectRankModeButton3.gameObject.transform, this.m_supportLayoutGroup.gameObject.transform);
			this.m_characterListDisplayButtons.Add(uicharacterPanelSelectRankModeButton3);
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			break;
		}
		this.SetPageIndex(0);
		UIManager.SetGameObjectActive(this.m_introContainer, true, null);
		this.m_draftScreenContainer.GetComponent<CanvasGroup>().alpha = 1f;
	}

	private int CompareCharacterTypeName(CharacterType CharA, CharacterType CharB)
	{
		return CharA.GetDisplayName().CompareTo(CharB.GetDisplayName());
	}

	private void SetupPageButton(_SelectableBtn btn, int pageIndex)
	{
		TextMeshProUGUI[] componentsInChildren = btn.GetComponentsInChildren<TextMeshProUGUI>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].text = (pageIndex + 1).ToString();
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.SetupPageButton(_SelectableBtn, int)).MethodHandle;
		}
		btn.SetSelected(false, false, string.Empty, string.Empty);
		btn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PageClicked);
	}

	private void PageClicked(BaseEventData data)
	{
		for (int i = 0; i < this.m_pageButtons.Count; i++)
		{
			if (this.m_pageButtons[i].spriteController.m_hitBoxImage.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.PageClicked(BaseEventData)).MethodHandle;
				}
				this.SetPageIndex(i);
				return;
			}
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	private bool IsCharacterTypeSelectable(CharacterType type)
	{
		if (!this.m_selectedCharacterTypes.Contains(type))
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.IsCharacterTypeSelectable(CharacterType)).MethodHandle;
			}
			if (!this.IsBanned(type))
			{
				if (!GameManager.Get().IsCharacterAllowedForGameType(type, GameType.Ranked, null, null))
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					return false;
				}
				return true;
			}
		}
		return false;
	}

	private bool IsCharacterVisibleForPlayer(CharacterType type)
	{
		return GameManager.Get().IsCharacterAllowedForPlayers(type);
	}

	private bool IsCharacterAvailableForPlayer(CharacterType type)
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		PersistedCharacterData playerCharacterData = clientGameManager.GetPlayerCharacterData(type);
		if (playerCharacterData != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.IsCharacterAvailableForPlayer(CharacterType)).MethodHandle;
			}
			if (playerCharacterData.CharacterComponent.Unlocked)
			{
				return true;
			}
		}
		if (!clientGameManager.IsCharacterAvailable(type, GameType.Ranked))
		{
			return false;
		}
		return true;
	}

	private void CheckCharacterListValidity()
	{
		bool flag = this.m_lastDraftNotification != null && this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase();
		for (int i = 0; i < this.m_characterListDisplayButtons.Count; i++)
		{
			CharacterType characterType = this.m_characterListDisplayButtons[i].m_characterType;
			bool flag2 = this.IsCharacterAvailableForPlayer(characterType);
			bool flag3;
			if (this.IsCharacterTypeSelectable(characterType))
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.CheckCharacterListValidity()).MethodHandle;
				}
				if (!flag2)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					flag3 = flag;
				}
				else
				{
					flag3 = true;
				}
			}
			else
			{
				flag3 = false;
			}
			bool enabled = flag3;
			bool flag4 = this.IsCharacterVisibleForPlayer(characterType);
			if (flag4)
			{
				this.m_characterListDisplayButtons[i].SetEnabled(enabled, ClientGameManager.Get().GetPlayerCharacterData(characterType));
				this.m_characterListDisplayButtons[i].SetSelected(this.HoveredCharacter == this.m_characterListDisplayButtons[i].m_characterType || this.SelectedCharacter == this.m_characterListDisplayButtons[i].m_characterType || this.m_selectedSubPhaseCharacter == this.m_characterListDisplayButtons[i].m_characterType);
				UIManager.SetGameObjectActive(this.m_characterListDisplayButtons[i], true, null);
			}
			else
			{
				UIManager.SetGameObjectActive(this.m_characterListDisplayButtons[i], false, null);
			}
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void UpdateCharacterButtons()
	{
		this.m_currentVisiblePage = this.m_currentCharacterPage;
		bool flag;
		if (this.m_lastDraftNotification != null)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.UpdateCharacterButtons()).MethodHandle;
			}
			flag = this.m_lastDraftNotification.SubPhase.IsPickBanSubPhase();
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		for (int i = 0; i < this.m_characterListDisplayButtons.Count; i++)
		{
			CharacterType characterType = this.m_characterListDisplayButtons[i].m_characterType;
			bool flag3 = this.IsCharacterAvailableForPlayer(characterType);
			bool flag4;
			if (this.IsCharacterTypeSelectable(characterType))
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!flag3)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					flag4 = flag2;
				}
				else
				{
					flag4 = true;
				}
			}
			else
			{
				flag4 = false;
			}
			bool flag5 = flag4;
			bool flag6 = this.IsCharacterVisibleForPlayer(characterType);
			if (flag6)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				UICharacterPanelSelectButton uicharacterPanelSelectButton = this.m_characterListDisplayButtons[i];
				bool isAvailable = flag5;
				if (this.HoveredCharacter == this.m_characterListDisplayButtons[i].m_characterType)
				{
					goto IL_110;
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.SelectedCharacter == this.m_characterListDisplayButtons[i].m_characterType)
				{
					goto IL_110;
				}
				bool selected = this.m_selectedSubPhaseCharacter == this.m_characterListDisplayButtons[i].m_characterType;
				IL_111:
				uicharacterPanelSelectButton.Setup(isAvailable, selected);
				UIManager.SetGameObjectActive(this.m_characterListDisplayButtons[i], true, null);
				goto IL_13E;
				IL_110:
				selected = true;
				goto IL_111;
			}
			UIManager.SetGameObjectActive(this.m_characterListDisplayButtons[i], false, null);
			IL_13E:;
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	private void SetPageIndex(int index)
	{
		this.UpdateCharacterButtons();
		if (!this.IsVisible)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.SetPageIndex(int)).MethodHandle;
			}
			return;
		}
		this.Initialize();
	}

	public void DismantleRankDraft()
	{
		this.m_assignedCharacterForGame = CharacterType.None;
		this.m_hoverCharacterForGame = CharacterType.None;
		this.m_selectedSubPhaseCharacter = CharacterType.None;
		this.m_lastSetupSelectionPhaseSubType = FreelancerResolutionPhaseSubType.UNDEFINED;
		this.IsVisible = false;
		this.m_initialized = false;
		this.LastGameInfo = null;
		this.LastPlayerInfo = null;
		this.LastTeamInfo = null;
		this.m_lastDraftNotification = null;
		this.m_currentState = UIRankedModeDraftScreen.CenterNotification.None;
		this.m_stateQueues.Clear();
		this.m_playerIDsOnDeck.Clear();
		this.ClearAllStates();
		UIManager.SetGameObjectActive(UIFrontEnd.Get().m_frontEndNavPanel, true, null);
		UIManager.SetGameObjectActive(this.m_draftScreenContainer, false, null);
		UIRankedCharacterSelectSettingsPanel.Get().SetVisible(false, UICharacterSelectCharacterSettingsPanel.TabPanel.None);
		UIManager.SetGameObjectActive(this.m_singleSelectionCharacterSelected, false, null);
		UIManager.SetGameObjectActive(this.m_doubleRightSelectionCharacterSelected, false, null);
		UIManager.SetGameObjectActive(this.m_doubleLeftSelectionCharacterSelected, false, null);
		for (int i = 0; i < this.m_blueTeamMembers.Length; i++)
		{
			this.m_blueTeamMembers[i].Dismantle();
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.DismantleRankDraft()).MethodHandle;
		}
		for (int j = 0; j < this.m_redTeamMembers.Length; j++)
		{
			this.m_redTeamMembers[j].Dismantle();
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
	}

	public void SetupRankDraft()
	{
		UIPlayerProgressPanel.Get().SetVisible(false, true);
		this.IsVisible = true;
		UIFrontEnd.Get().m_frontEndNavPanel.SetNavButtonSelected(UIFrontEnd.Get().m_frontEndNavPanel.m_PlayBtn);
		UIStorePanel.Get().ClosePurchaseDialog();
		UIRankedModeSelectScreen.Get().SetVisible(false);
		UIManager.SetGameObjectActive(UIFrontEnd.Get().m_frontEndNavPanel, false, null);
		UIManager.SetGameObjectActive(this.m_draftScreenContainer, true, null);
		UIRAFProgramScreen.Get().SetVisible(false);
		this.Initialize();
	}

	public void SetDraftScreenVisible(bool visible)
	{
		if (!this.IsVisible)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankedModeDraftScreen.SetDraftScreenVisible(bool)).MethodHandle;
			}
			return;
		}
		UIManager.SetGameObjectActive(this.m_draftScreenContainer, visible, null);
		if (visible)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (this.m_containerAC == null)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				this.m_containerAC = this.m_draftScreenContainer.GetComponent<Animator>();
			}
			if (this.m_containerAC != null)
			{
				this.m_containerAC.Play("RankedModeSetup", 0, 1f);
			}
		}
	}

	[Serializable]
	public class BrowseCharacterImages
	{
		public Image m_unselected;

		public Image m_browsingCharacter;

		public Image m_selectedCharacter;
	}

	public enum CenterNotification
	{
		None,
		BlueTeamNotification,
		RedTeamNotification,
		BlueTeamSingleSelectStart,
		BlueTeamSingleSelectEnd,
		RedTeamSingleSelectStart,
		RedTeamSingleSelectEnd,
		BlueTeamDoubleSelectStart,
		BlueTeamDoubleSelectEnd,
		RedTeamDoubleSelectStart,
		RedTeamDoubleSelectEnd,
		TradePhase,
		LoadoutPhase,
		GameLoadPhase
	}
}
