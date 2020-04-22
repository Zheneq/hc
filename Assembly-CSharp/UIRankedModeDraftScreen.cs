using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRankedModeDraftScreen : UIScene
{
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

	private List<CenterNotification> m_stateQueues = new List<CenterNotification>();

	private CenterNotification m_currentState;

	private List<GameObject> m_centerStateObjects = new List<GameObject>();

	private CanvasGroup m_characterSelectContainerCanvasGroup;

	private Animator m_containerAC;

	private bool m_intendedLockInBtnStatus;

	private List<UICharacterSelectFactionFilter> m_filterButtons;

	private UICharacterSelectFactionFilter m_lastFilterBtnClicked;

	private static UIRankedModeDraftScreen s_instance;

	public bool IsVisible
	{
		get;
		private set;
	}

	public bool GameIsLaunching
	{
		get;
		private set;
	}

	public CharacterType HoveredCharacter
	{
		get
		{
			return m_hoverCharacterForGame;
		}
		private set
		{
			m_hoverCharacterForGame = value;
			if (value != 0)
			{
				m_selectedSubPhaseCharacter = value;
				SetupCharacterSettings(value);
			}
		}
	}

	public CharacterType ClientClickedCharacter => m_selectedSubPhaseCharacter;

	public CharacterType SelectedCharacter
	{
		get
		{
			return m_assignedCharacterForGame;
		}
		private set
		{
			if (value == CharacterType.None)
			{
				return;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (LastGameInfo == null)
				{
					return;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					if (m_assignedCharacterForGame != value)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							m_assignedCharacterForGame = value;
							m_hoverCharacterForGame = value;
							m_selectedSubPhaseCharacter = value;
							SetupCharacterSettings(value);
							return;
						}
					}
					return;
				}
			}
		}
	}

	internal int OurPlayerId
	{
		get
		{
			long accountId = ClientGameManager.Get().GetPlayerAccountData().AccountId;
			UIRankedModePlayerDraftEntry[] blueTeamMembers = m_blueTeamMembers;
			foreach (UIRankedModePlayerDraftEntry uIRankedModePlayerDraftEntry in blueTeamMembers)
			{
				if (uIRankedModePlayerDraftEntry.AccountID != accountId)
				{
					continue;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return uIRankedModePlayerDraftEntry.PlayerID;
				}
			}
			return -1;
		}
	}

	private void SetupCharacterSettings(CharacterType charType)
	{
		CharacterCardInfo characterCardInfo;
		CharacterVisualInfo characterVisualInfo;
		if (LastPlayerInfo.CharacterType == charType)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			characterCardInfo = LastPlayerInfo.CharacterInfo.CharacterCards;
			characterVisualInfo = LastPlayerInfo.CharacterInfo.CharacterSkin;
		}
		else
		{
			PersistedCharacterData playerCharacterData = ClientGameManager.Get().GetPlayerCharacterData(charType);
			characterCardInfo = playerCharacterData.CharacterComponent.LastCards;
			characterVisualInfo = playerCharacterData.CharacterComponent.LastSkin;
		}
		m_rankedModeCharacterSettings.UpdateSelectedCharType(charType);
		CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(charType);
		if (!m_rankedModeCharacterSettings.m_spellsSubPanel.GetDisplayedCardInfo().Equals(characterCardInfo))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			m_rankedModeCharacterSettings.m_spellsSubPanel.Setup(charType, characterCardInfo);
		}
		if (!(m_rankedModeCharacterSettings.m_abilitiesSubPanel.GetDisplayedCharacter() == null))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_rankedModeCharacterSettings.m_abilitiesSubPanel.GetDisplayedCharacter().m_characterType.Equals(characterResourceLink.m_characterType))
			{
				goto IL_0143;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_rankedModeCharacterSettings.m_abilitiesSubPanel.Setup(characterResourceLink);
		goto IL_0143;
		IL_01bf:
		if (!(m_rankedModeCharacterSettings.m_tauntsSubPanel.GetDisplayedCharacter() == null))
		{
			if (m_rankedModeCharacterSettings.m_tauntsSubPanel.GetDisplayedCharacter().m_characterType.Equals(characterResourceLink.m_characterType))
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_rankedModeCharacterSettings.m_tauntsSubPanel.Setup(characterResourceLink);
		return;
		IL_0143:
		if (m_rankedModeCharacterSettings.m_skinsSubPanel.GetDisplayedCharacterType().Equals(characterResourceLink.m_characterType))
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_rankedModeCharacterSettings.m_skinsSubPanel.GetDisplayedVisualInfo().Equals(characterVisualInfo))
			{
				goto IL_01bf;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_rankedModeCharacterSettings.m_skinsSubPanel.Setup(characterResourceLink, characterVisualInfo);
		goto IL_01bf;
	}

	public static UIRankedModeDraftScreen Get()
	{
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.RankDraft;
	}

	public override void Awake()
	{
		if (!(s_instance == null))
		{
			return;
		}
		s_instance = this;
		ClientGameManager.Get().OnGameInfoNotification += HandleGameInfoNotification;
		ClientGameManager.Get().OnLobbyGameplayOverridesChange += OnLobbyGameplayOverridesUpdated;
		m_lockInBtn.spriteController.callback = LockPhaseButtonClicked;
		m_lockInBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.RankFreelancerSelectClick;
		m_lockFreelancerBtn.spriteController.callback = LockFreelancerBtnClicked;
		m_lockFreelancerBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.RankFreelancerLockin;
		m_skinsBtn.spriteController.callback = SettingsButtonClicked;
		m_abilitiesBtn.spriteController.callback = SettingsButtonClicked;
		m_catalystsBtn.spriteController.callback = SettingsButtonClicked;
		m_tauntsBtn.spriteController.callback = SettingsButtonClicked;
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
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				list = list.Except(groupFilter.Characters).ToList();
			}
			uICharacterSelectFactionFilter.m_btn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Simple, delegate(UITooltipBase tooltip)
			{
				(tooltip as UISimpleTooltip).Setup(FactionGroup.GetDisplayName(groupFilter.FactionGroupID));
				return true;
			});
		}
		m_notOnAFactionFilter.Setup(list, ClickedOnFactionFilter);
		UITooltipHoverObject component = m_notOnAFactionFilter.m_btn.spriteController.GetComponent<UITooltipHoverObject>();
		if (_003C_003Ef__am_0024cache0 == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			_003C_003Ef__am_0024cache0 = delegate(UITooltipBase tooltip)
			{
				(tooltip as UISimpleTooltip).Setup(StringUtil.TR("Wildcard", "Global"));
				return true;
			};
		}
		component.Setup(TooltipType.Simple, _003C_003Ef__am_0024cache0);
		m_centerStateObjects.Add(m_blueTeamTurnNotification.gameObject);
		m_centerStateObjects.Add(m_redTeamTurnNotification.gameObject);
		m_centerStateObjects.Add(m_singleSelectionAC.gameObject);
		m_centerStateObjects.Add(m_doubleSelectionAC.gameObject);
		m_centerStateObjects.Add(m_swapPhaseAC.gameObject);
		m_centerStateObjects.Add(m_loadoutPhaseAC.gameObject);
		m_centerStateObjects.Add(m_gameLoadingAC.gameObject);
		m_containerAC = m_draftScreenContainer.GetComponent<Animator>();
		base.Awake();
	}

	private bool IsBanned(CharacterType characterType)
	{
		int result;
		if (!m_friendlyBannedCharacterTypes.IsNullOrEmpty())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_friendlyBannedCharacterTypes.Contains(characterType))
			{
				result = 1;
				goto IL_005b;
			}
		}
		if (!m_enemyBannedCharacterTypes.IsNullOrEmpty())
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			result = (m_enemyBannedCharacterTypes.Contains(characterType) ? 1 : 0);
		}
		else
		{
			result = 0;
		}
		goto IL_005b;
		IL_005b:
		return (byte)result != 0;
	}

	public void EditedSearchInput(string input)
	{
		UpdateCharacterButtonHighlights();
	}

	public void ClickedOnFactionFilter(UICharacterSelectFactionFilter btn)
	{
		if (m_lastFilterBtnClicked != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
		for (int i = 0; i < m_characterListDisplayButtons.Count; i++)
		{
			if (!(m_characterListDisplayButtons[i] != null))
			{
				continue;
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_characterListDisplayButtons[i].GetComponent<CanvasGroup>() != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				m_characterListDisplayButtons[i].GetComponent<CanvasGroup>().alpha = 1f;
			}
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (m_lastFilterBtnClicked != null && m_lastFilterBtnClicked.m_btn.IsSelected())
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int j = 0; j < m_characterListDisplayButtons.Count; j++)
				{
					if (m_lastFilterBtnClicked.IsAvailable(m_characterListDisplayButtons[j].m_characterType))
					{
						continue;
					}
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					CanvasGroup component = m_characterListDisplayButtons[j].GetComponent<CanvasGroup>();
					if (component != null)
					{
						while (true)
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
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (m_searchInputField.text.IsNullOrEmpty())
			{
				return;
			}
			for (int k = 0; k < m_characterListDisplayButtons.Count; k++)
			{
				string text = string.Empty;
				CharacterResourceLink characterResourceLink = m_characterListDisplayButtons[k].GetCharacterResourceLink();
				if (characterResourceLink != null)
				{
					while (true)
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
				if (!DoesSearchMatchDisplayName(m_searchInputField.text.ToLower(), text.ToLower()))
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					m_characterListDisplayButtons[k].GetComponent<CanvasGroup>().alpha = 0.3f;
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
	}

	private bool DoesSearchMatchDisplayName(string searchText, string displayText)
	{
		for (int i = 0; i < searchText.Length; i++)
		{
			while (true)
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
			if (searchText[i] == displayText[i])
			{
				continue;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return false;
			}
		}
		return true;
	}

	public void DoQueueState(CenterNotification notification)
	{
		UIManager.SetGameObjectActive(m_blueTeamTurnNotification, notification == CenterNotification.BlueTeamNotification);
		UIManager.SetGameObjectActive(m_redTeamTurnNotification, notification == CenterNotification.RedTeamNotification);
		UIManager.SetGameObjectActive(m_singleSelectionAC, notification == CenterNotification.BlueTeamSingleSelectStart || notification == CenterNotification.RedTeamSingleSelectStart);
		UIManager.SetGameObjectActive(m_doubleSelectionAC, notification == CenterNotification.BlueTeamDoubleSelectStart || notification == CenterNotification.RedTeamDoubleSelectStart);
		UIManager.SetGameObjectActive(m_swapPhaseAC, notification == CenterNotification.TradePhase);
		UIManager.SetGameObjectActive(m_loadoutPhaseAC, notification == CenterNotification.LoadoutPhase);
		UIManager.SetGameObjectActive(m_gameLoadingAC, notification == CenterNotification.GameLoadPhase);
		if (m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
		{
			m_blueTeamTurnTextNotification.text = StringUtil.TR("BlueBans", "RankMode");
			m_redTeamTurnTextNotification.text = StringUtil.TR("RedBans", "RankMode");
		}
		else if (m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase())
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_blueTeamTurnTextNotification.text = StringUtil.TR("BluePicks", "RankMode");
			m_redTeamTurnTextNotification.text = StringUtil.TR("RedPicks", "RankMode");
		}
		int num;
		if (notification != CenterNotification.BlueTeamNotification)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (notification != CenterNotification.BlueTeamSingleSelectStart)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				num = ((notification == CenterNotification.BlueTeamDoubleSelectStart) ? 1 : 0);
				goto IL_0150;
			}
		}
		num = 1;
		goto IL_0150;
		IL_0175:
		int num2;
		bool flag = (byte)num2 != 0;
		bool flag2;
		SetCenterBackground(flag2, flag);
		if (notification != CenterNotification.BlueTeamNotification)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (notification != CenterNotification.RedTeamNotification)
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		RankedResolutionPhaseData? rankedData = m_lastDraftNotification.RankedData;
		UpdateCenterVisuals(rankedData.Value, flag2, flag);
		return;
		IL_0150:
		flag2 = ((byte)num != 0);
		if (notification != CenterNotification.RedTeamNotification)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (notification != CenterNotification.RedTeamSingleSelectStart)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				num2 = ((notification == CenterNotification.RedTeamDoubleSelectStart) ? 1 : 0);
				goto IL_0175;
			}
		}
		num2 = 1;
		goto IL_0175;
	}

	private bool IsDoubleSelectinReadyToAdvance()
	{
		if (m_currentState != CenterNotification.BlueTeamDoubleSelectStart)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_currentState != CenterNotification.RedTeamDoubleSelectStart)
			{
				goto IL_00b3;
			}
		}
		if (m_stateQueues.Count > 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_stateQueues[0] != CenterNotification.RedTeamDoubleSelectEnd)
			{
				if (m_stateQueues[0] != CenterNotification.BlueTeamDoubleSelectEnd)
				{
					goto IL_00b3;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (!m_doubleRightSelectionCharacterSelected.gameObject.activeInHierarchy)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!m_doubleLeftSelectionCharacterSelected.gameObject.activeInHierarchy)
				{
					while (true)
					{
						switch (2)
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
		goto IL_00b3;
		IL_00b3:
		return false;
	}

	private bool IsAnyCenterStateActive()
	{
		for (int i = 0; i < m_centerStateObjects.Count; i++)
		{
			if (m_centerStateObjects[i].gameObject.activeSelf)
			{
				return true;
			}
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return false;
		}
	}

	private void ClearAllStates()
	{
		for (int i = 0; i < m_centerStateObjects.Count; i++)
		{
			UIManager.SetGameObjectActive(m_centerStateObjects[i], false);
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	private void QueueCenterState(CenterNotification notification)
	{
		if (notification == CenterNotification.None)
		{
			return;
		}
		if (notification != CenterNotification.LoadoutPhase)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (notification != CenterNotification.GameLoadPhase)
			{
				goto IL_003c;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		ClearAllStates();
		m_stateQueues.Clear();
		goto IL_003c;
		IL_003c:
		if (m_stateQueues.Count > 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_stateQueues[m_stateQueues.Count - 1] == notification)
			{
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
		}
		else if (m_currentState == notification)
		{
			return;
		}
		m_stateQueues.Add(notification);
	}

	public void SettingsButtonClicked(BaseEventData data)
	{
		UICharacterSelectCharacterSettingsPanel.TabPanel tab = UICharacterSelectCharacterSettingsPanel.TabPanel.None;
		if ((data as PointerEventData).pointerCurrentRaycast.gameObject == m_skinsBtn.spriteController.gameObject)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			tab = UICharacterSelectCharacterSettingsPanel.TabPanel.Skins;
		}
		else if ((data as PointerEventData).pointerCurrentRaycast.gameObject == m_abilitiesBtn.spriteController.gameObject)
		{
			while (true)
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
		else if ((data as PointerEventData).pointerCurrentRaycast.gameObject == m_catalystsBtn.spriteController.gameObject)
		{
			while (true)
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
		else if ((data as PointerEventData).pointerCurrentRaycast.gameObject == m_tauntsBtn.spriteController.gameObject)
		{
			while (true)
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
		s_instance = null;
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnGameInfoNotification -= HandleGameInfoNotification;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= OnLobbyGameplayOverridesUpdated;
		}
	}

	private void SetFreelancerSettingButtonsVisible(bool visible)
	{
		UIManager.SetGameObjectActive(m_skinsBtn, visible);
		UIManager.SetGameObjectActive(m_abilitiesBtn, visible);
		UIManager.SetGameObjectActive(m_catalystsBtn, visible);
		UIManager.SetGameObjectActive(m_tauntsBtn, visible);
	}

	public void OnLobbyGameplayOverridesUpdated(LobbyGameplayOverrides gameplayOverrides)
	{
		CheckCharacterListValidity();
	}

	public void HandleGameInfoNotification(GameInfoNotification notification)
	{
		if (notification.GameInfo == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Log.Error("Why is GameInfo null?");
					return;
				}
			}
		}
		if (notification.PlayerInfo == null)
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
		if (notification.TeamInfo == null)
		{
			Log.Warning("Why GameInfoNotification.TeamInfo null?");
			return;
		}
		if (notification.TeamInfo.TeamPlayerInfo == null)
		{
			Log.Warning("Why GameInfoNotification.TeamInfo.TeamPlayerInfo null?");
			return;
		}
		if (notification.GameInfo.GameStatus == GameStatus.Stopped)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					AppState_GroupCharacterSelect.Get().Enter();
					return;
				}
			}
		}
		if (notification.PlayerInfo.AccountId == 0)
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (notification.TeamInfo.TeamPlayerInfo.Count == 0)
			{
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
			if (notification.GameInfo.GameStatus == GameStatus.LoadoutSelecting && m_lastGameStatus != GameStatus.LoadoutSelecting)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				m_loadoutSelectStartTime = Time.realtimeSinceStartup;
				SetFreelancerSettingButtonsVisible(true);
			}
			bool flag = false;
			if (LastGameInfo == null)
			{
				flag = true;
				m_phaseStartTime = Time.time;
			}
			LastGameInfo = notification.GameInfo;
			LastTeamInfo = notification.TeamInfo;
			LastPlayerInfo = notification.PlayerInfo;
			m_lastGameStatus = LastGameInfo.GameStatus;
			if (!IsVisible)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				bool flag2 = false;
				if (notification.GameInfo.GameStatus == GameStatus.FreelancerSelecting)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					GameIsLaunching = false;
				}
				else
				{
					if (notification.GameInfo.GameStatus != GameStatus.Stopped)
					{
						while (true)
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
							while (true)
							{
								switch (4)
								{
								case 0:
									continue;
								}
								break;
							}
							flag2 = true;
							UpdateGameLaunching(notification);
							goto IL_022f;
						}
					}
					if (notification.GameInfo.GameStatus != GameStatus.Stopped)
					{
						while (true)
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
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (!m_stateQueues.Contains(CenterNotification.GameLoadPhase))
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								if (m_currentState != CenterNotification.GameLoadPhase)
								{
									QueueCenterState(CenterNotification.GameLoadPhase);
								}
							}
							UpdateGameLaunching(notification);
						}
					}
				}
				goto IL_022f;
				IL_022f:
				if (flag2)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!m_stateQueues.Contains(CenterNotification.LoadoutPhase) && m_currentState != CenterNotification.LoadoutPhase)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						QueueCenterState(CenterNotification.LoadoutPhase);
					}
				}
				if (m_lastDraftNotification != null && m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (LastGameInfo.GameStatus == GameStatus.FreelancerSelecting && !m_stateQueues.Contains(CenterNotification.TradePhase))
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (m_currentState != CenterNotification.TradePhase)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							QueueCenterState(CenterNotification.TradePhase);
						}
					}
				}
				MapData mapData = GameWideData.Get().GetMapData(notification.GameInfo.GameConfig.Map);
				Sprite sprite = null;
				string mapDisplayName = GameWideData.Get().GetMapDisplayName(notification.GameInfo.GameConfig.Map);
				sprite = ((mapData == null) ? (Resources.Load("Stages/information_stage_image", typeof(Sprite)) as Sprite) : (Resources.Load(mapData.ResourceImageSpriteLocation, typeof(Sprite)) as Sprite));
				m_stageImage.sprite = sprite;
				m_introStageImage.sprite = sprite;
				m_stageText.text = mapDisplayName;
				m_introStageText.text = mapDisplayName;
				if (notification.GameInfo.GameConfig.GameType == GameType.Ranked)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					m_matchFoundText.text = StringUtil.TR("RankedMatchFound", "OverlayScreensScene");
				}
				else
				{
					m_matchFoundText.text = string.Format(StringUtil.TR("SubtypeFound", "Global"), StringUtil.TR(notification.GameInfo.GameConfig.InstanceSubType.LocalizedName));
				}
				SetupPlayerLists();
				UpdateNotification(m_lastDraftNotification, true && !flag);
				return;
			}
		}
	}

	private void SetupPlayerLists()
	{
		if (LastGameInfo == null)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_lastDraftNotification == null)
			{
				return;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				if (!m_lastDraftNotification.RankedData.HasValue)
				{
					return;
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					RankedResolutionPhaseData value = m_lastDraftNotification.RankedData.Value;
					Team team = LastPlayerInfo.TeamId;
					if (LastPlayerInfo.TeamId == Team.Spectator)
					{
						team = Team.TeamA;
					}
					int num = 0;
					int num2 = 0;
					for (int i = 0; i < value.PlayerIdByImporance.Count; i++)
					{
						int num3 = value.PlayerIdByImporance[i];
						IEnumerable<LobbyPlayerInfo> enumerable = LastTeamInfo.TeamInfo(team);
						foreach (LobbyPlayerInfo item in enumerable)
						{
							if (item.PlayerId == num3)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										break;
									default:
										if (num < m_blueTeamMembers.Length)
										{
											while (true)
											{
												switch (7)
												{
												case 0:
													break;
												default:
													m_blueTeamMembers[num].Setup(item);
													num++;
													goto end_IL_00b6;
												}
											}
										}
										goto end_IL_00b6;
									}
								}
							}
						}
						IEnumerable<LobbyPlayerInfo> enumerable2 = LastTeamInfo.TeamInfo(team.OtherTeam());
						IEnumerator<LobbyPlayerInfo> enumerator2 = enumerable2.GetEnumerator();
						try
						{
							while (true)
							{
								if (!enumerator2.MoveNext())
								{
									while (true)
									{
										switch (5)
										{
										case 0:
											continue;
										}
										break;
									}
									break;
								}
								LobbyPlayerInfo current2 = enumerator2.Current;
								if (current2.PlayerId == num3)
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											break;
										default:
											if (num2 < m_redTeamMembers.Length)
											{
												while (true)
												{
													switch (4)
													{
													case 0:
														break;
													default:
														m_redTeamMembers[num2].Setup(current2, true);
														num2++;
														goto end_IL_013c;
													}
												}
											}
											goto end_IL_013c;
										}
									}
								}
							}
							end_IL_013c:;
						}
						finally
						{
							if (enumerator2 != null)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
										enumerator2.Dispose();
										goto end_IL_019e;
									}
								}
							}
							end_IL_019e:;
						}
					}
					return;
				}
			}
		}
	}

	private void Update()
	{
		float num;
		int num2;
		if (m_lastDraftNotification != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!GameIsLaunching)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_lastDraftNotification.RankedData.HasValue)
				{
					while (true)
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
					if ((double)(Time.time - m_phaseStartTime) < m_timeInPhase.TotalSeconds)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						num = (float)m_timeInPhase.TotalSeconds - Time.time + m_phaseStartTime;
						num2 = Mathf.RoundToInt(num);
						RankedResolutionPhaseData value = m_lastDraftNotification.RankedData.Value;
						Team currentTeam = GetCurrentTeam(value);
						if (currentTeam != 0)
						{
							while (true)
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
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								if (m_gameCountdownTimer.text != num2.ToString())
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									m_gameCountdownTimer.text = num2.ToString();
									m_gameCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
								}
								m_redCountdownTimer.text = string.Empty;
								m_blueCountdownTimer.text = string.Empty;
								goto IL_028a;
							}
						}
						if (currentTeam == LastPlayerInfo.TeamId)
						{
							m_gameCountdownTimer.text = string.Empty;
							m_redCountdownTimer.text = string.Empty;
							if (m_blueCountdownTimer.text != num2.ToString())
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								m_blueCountdownTimer.text = num2.ToString();
								m_blueCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
							}
						}
						else
						{
							m_gameCountdownTimer.text = string.Empty;
							if (m_redCountdownTimer.text != num2.ToString())
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								m_redCountdownTimer.text = num2.ToString();
								m_redCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
							}
							m_blueCountdownTimer.text = string.Empty;
						}
						goto IL_028a;
					}
					goto IL_02b2;
				}
			}
		}
		goto IL_032a;
		IL_028a:
		if (num2 <= 5)
		{
			while (true)
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
		goto IL_02b2;
		IL_07f3:
		if (IsCenterSelectAnimating())
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(m_lockInBtn, false);
		}
		else
		{
			string text;
			if (m_lastDraftNotification != null && m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
			{
				while (true)
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
			for (int i = 0; i < m_lockInText.Length; i++)
			{
				m_lockInText[i].text = text;
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(m_lockInBtn, m_intendedLockInBtnStatus);
			m_lockInBtn.SetDisabled(m_selectedSubPhaseCharacter == SelectedCharacter);
		}
		if (m_containerAC == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			m_containerAC = m_draftScreenContainer.GetComponent<Animator>();
		}
		if (m_containerAC != null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_containerAC.gameObject.activeInHierarchy)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_containerAC.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					DoCharacterSelectContainerActiveCheck();
				}
			}
		}
		UIManager.SetGameObjectActive(m_searchFiltersContainer, m_characterSelectContainer.gameObject.activeSelf);
		return;
		IL_02b2:
		if (SelectedCharacter != 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (SelectedCharacter != ClientClickedCharacter)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				int playerId = GameManager.Get().PlayerInfo.PlayerId;
				if (!m_lastDraftNotification.RankedData.Value._001D(playerId))
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					SetupCharacterSettings(SelectedCharacter);
				}
			}
		}
		goto IL_032a;
		IL_032a:
		if (GameIsLaunching)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			m_gameCountdownTimer.text = string.Empty;
			if (LastGameInfo != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (LastGameInfo.GameStatus == GameStatus.LoadoutSelecting)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					float num3 = Mathf.Max(0f, (float)LastGameInfo.LoadoutSelectTimeout.TotalSeconds - (Time.realtimeSinceStartup - m_loadoutSelectStartTime));
					int num4 = Mathf.RoundToInt(num3);
					if (m_gameCountdownTimer.text != num4.ToString())
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						m_gameCountdownTimer.text = num4.ToString();
						m_gameCountdownAC.Play("RankedNumberTextCountdown", 1, 0f);
					}
					if (num4 < 6 && Mathf.RoundToInt(num3 + Time.deltaTime) != num4)
					{
						UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeTimerTick);
					}
				}
			}
			m_redCountdownTimer.text = string.Empty;
			m_blueCountdownTimer.text = string.Empty;
		}
		float axis = Input.GetAxis("Mouse ScrollWheel");
		if (axis > 0f)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			SetPageIndex(m_currentCharacterPage + 1);
		}
		else if (axis < 0f)
		{
			SetPageIndex(m_currentCharacterPage - 1);
		}
		if (!m_introContainer.gameObject.activeSelf)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (IsAnyCenterStateActive())
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!IsDoubleSelectinReadyToAdvance())
				{
					goto IL_0524;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (m_stateQueues.Count > 0)
			{
				DoQueueState(m_stateQueues[0]);
				m_currentState = m_stateQueues[0];
				m_stateQueues.RemoveAt(0);
			}
		}
		goto IL_0524;
		IL_0524:
		if (m_characterSelectContainerCanvasGroup == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			m_characterSelectContainerCanvasGroup = m_characterSelectContainer.GetComponent<CanvasGroup>();
		}
		if (!(m_journeyLength > 0f))
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (m_currentCharacterPage == m_currentVisiblePage)
			{
				goto IL_07f3;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		float num5 = (Time.time - m_startTime) * m_timeForPageToSwap;
		float num6 = num5 / m_journeyLength;
		Vector2 anchoredPosition = Vector2.Lerp(m_startLocation, m_endLocation, num6);
		if (!float.IsNaN(anchoredPosition.x))
		{
			while (true)
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
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				(m_characterSelectContainer.transform as RectTransform).anchoredPosition = anchoredPosition;
				if (m_characterSelectContainerCanvasGroup != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_currentCharacterPage != m_currentVisiblePage)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						m_characterSelectContainerCanvasGroup.alpha = 1f - num6;
					}
					else
					{
						m_characterSelectContainerCanvasGroup.alpha = num6;
					}
				}
				if (num6 >= 1f)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_characterSelectContainerCanvasGroup.alpha <= 0f)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						Vector2 anchoredPosition2 = (m_characterSelectContainer.gameObject.transform as RectTransform).anchoredPosition;
						if (m_endLocation.x < 0f)
						{
							(m_characterSelectContainer.gameObject.transform as RectTransform).anchoredPosition = new Vector2(anchoredPosition2.x * -1f, anchoredPosition2.y);
							m_startTime = Time.time;
							m_startLocation = anchoredPosition2;
							m_endLocation = new Vector2(0f, anchoredPosition2.y);
							m_journeyLength = Vector2.Distance(m_startLocation, m_endLocation);
						}
						else if (m_endLocation.x > 0f)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								break;
							}
							(m_characterSelectContainer.gameObject.transform as RectTransform).anchoredPosition = new Vector2(anchoredPosition2.x * -1f, anchoredPosition2.y);
							m_startTime = Time.time;
							m_startLocation = anchoredPosition2;
							m_endLocation = new Vector2(0f, anchoredPosition2.y);
							m_journeyLength = Vector2.Distance(m_startLocation, m_endLocation);
						}
						UpdateCharacterButtons();
					}
					else
					{
						m_journeyLength = 0f;
					}
				}
			}
		}
		goto IL_07f3;
	}

	private bool IsCenterSelectAnimating()
	{
		bool result = false;
		if (m_singleSelectionCharacterSelected.gameObject.activeInHierarchy)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = true;
		}
		else if (m_doubleRightSelectionCharacterSelected.gameObject.activeInHierarchy && doubleSelectionLeftCharacter.gameObject.activeInHierarchy)
		{
			while (true)
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
		else if (m_doubleLeftSelectionCharacterSelected.gameObject.activeInHierarchy)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (doubleSelectionRightCharacter.gameObject.activeInHierarchy)
			{
				result = true;
			}
		}
		return result;
	}

	public void LockFreelancerBtnClicked(BaseEventData data)
	{
		if (m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
		{
			UIManager.SetGameObjectActive(m_lockFreelancerContainer, false);
			ClientGameManager.Get().SendRankedTradeRequest_StopTrading();
		}
	}

	public void LockPhaseButtonClicked(BaseEventData data)
	{
		if (m_selectedSubPhaseCharacter == CharacterType.None)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (SelectedCharacter == m_selectedSubPhaseCharacter)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
				{
					ClientGameManager.Get().SendRankedBanRequest(m_selectedSubPhaseCharacter);
				}
				else
				{
					ClientGameManager.Get().SendRankedSelectRequest(m_selectedSubPhaseCharacter);
				}
				return;
			}
		}
	}

	private bool DidPlayerLockInDuringSwapPhase(RankedResolutionPhaseData data, long playerID)
	{
		bool result = false;
		if (data.TradeActions != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					using (List<RankedTradeData>.Enumerator enumerator = data.TradeActions.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							RankedTradeData current = enumerator.Current;
							if (current.OfferingPlayerId == playerID)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								if (current.TradeAction == RankedTradeData.TradeActionType._0012)
								{
									return true;
								}
							}
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								return result;
							}
						}
					}
				}
				}
			}
		}
		return result;
	}

	private void UpdateHoverSelfStatus(RankedResolutionPhaseData data)
	{
		if (m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		if (LastGameInfo == null)
		{
			return;
		}
		int playerId = LastPlayerInfo.PlayerId;
		int num = 0;
		while (true)
		{
			if (num < m_blueTeamMembers.Length)
			{
				if (m_blueTeamMembers[num].PlayerID == playerId)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (HoveredCharacter == CharacterType.None)
					{
						break;
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_selectedCharacterTypes.Contains(HoveredCharacter))
					{
						break;
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!IsBanned(HoveredCharacter))
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(HoveredCharacter);
						m_blueTeamMembers[num].SetBrowseCharacterImageVisible(true);
						m_blueTeamMembers[num].SetHoverCharacter(characterResourceLink);
					}
					break;
				}
				num++;
				continue;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			break;
		}
		if (m_playerIDsOnDeck.Count == 1)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					KeyValuePair<int, CharacterType> selectedChar = new KeyValuePair<int, CharacterType>(playerId, HoveredCharacter);
					if (m_playerIDsOnDeck.ContainsKey(playerId))
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								SetupSelection(selectedChar, data, m_singleCharacterName, singleNoSelectionCharacter, singleBrowseSelectionCharacter, singleSelectionCharacter, m_singleSelectionCharacterSelected, m_singleBlueSelectionCharacterSelected, m_singleBlueTeamSelectedCharacter, m_singleBlueTeamPlayerName, true, false);
								return;
							}
						}
					}
					return;
				}
				}
			}
		}
		if (m_playerIDsOnDeck.Count != 2)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			bool flag = true;
			using (Dictionary<int, CharacterType>.Enumerator enumerator = m_playerIDsOnDeck.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					KeyValuePair<int, CharacterType> current = enumerator.Current;
					if (flag)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						SetupSelection(current, data, m_leftCharacterName, doubleNoSelectionLeftCharacter, doubleBrowseSelectionLeftCharacter, doubleSelectionLeftCharacter, m_doubleLeftSelectionCharacterSelected, m_doubleLeftBlueSelectionCharacterSelected, m_doubleLeftBlueTeamSelectedCharacter, m_doubleLeftBlueTeamPlayerName, true, false);
						flag = false;
					}
					else
					{
						SetupSelection(current, data, m_rightCharacterName, doubleNoSelectionRightCharacter, doubleBrowseSelectionRightCharacter, doubleSelectionRightCharacter, m_doubleRightSelectionCharacterSelected, m_doubleRightBlueSelectionCharacterSelected, m_doubleRightBlueTeamSelectedCharacter, m_doubleRightBlueTeamPlayerName, true, false);
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
		}
	}

	private void UpdateHoverStatus(RankedResolutionPhaseData data)
	{
		bool flag = m_lastDraftNotification.SubPhase.IsPickBanSubPhase();
		for (int i = 0; i < m_blueTeamMembers.Length; i++)
		{
			if (flag)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (m_playerIDsOnDeck.ContainsKey(OurPlayerId))
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_blueTeamMembers[i].PlayerID == OurPlayerId)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						continue;
					}
				}
			}
			if (data.FriendlyTeamSelections.ContainsKey(m_blueTeamMembers[i].PlayerID))
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				m_blueTeamMembers[i].SetBrowseCharacterImageVisible(false);
			}
			else if (m_playerIDsOnDeck.ContainsKey(m_blueTeamMembers[i].PlayerID))
			{
				if (m_playerIDsOnDeck[m_blueTeamMembers[i].PlayerID] != 0)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					if (IsBanned(m_playerIDsOnDeck[m_blueTeamMembers[i].PlayerID]) || m_selectedCharacterTypes.Contains(m_playerIDsOnDeck[m_blueTeamMembers[i].PlayerID]))
					{
						m_blueTeamMembers[i].SetBrowseCharacterImageVisible(false);
						UIManager.SetGameObjectActive(m_blueTeamMembers[i].m_noCharacterImage, true);
					}
					else if (OurPlayerId != m_blueTeamMembers[i].PlayerID)
					{
						CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(m_playerIDsOnDeck[m_blueTeamMembers[i].PlayerID]);
						m_blueTeamMembers[i].SetBrowseCharacterImageVisible(true);
						m_blueTeamMembers[i].SetHoverCharacter(characterResourceLink);
					}
					else if (HoveredCharacter != 0)
					{
						CharacterResourceLink characterResourceLink2 = GameWideData.Get().GetCharacterResourceLink(HoveredCharacter);
						m_blueTeamMembers[i].SetBrowseCharacterImageVisible(true);
						m_blueTeamMembers[i].SetHoverCharacter(characterResourceLink2);
					}
				}
			}
			else
			{
				bool browseCharacterImageVisible = false;
				using (List<RankedResolutionPlayerState>.Enumerator enumerator = data.UnselectedPlayerStates.GetEnumerator())
				{
					while (true)
					{
						if (!enumerator.MoveNext())
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							break;
						}
						RankedResolutionPlayerState current = enumerator.Current;
						if (current.PlayerId == m_blueTeamMembers[i].PlayerID)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									break;
								default:
									{
										if (current.Intention != 0)
										{
											if (IsBanned(current.Intention))
											{
												goto IL_0296;
											}
											while (true)
											{
												switch (7)
												{
												case 0:
													continue;
												}
												break;
											}
											if (m_selectedCharacterTypes.Contains(current.Intention))
											{
												goto IL_0296;
											}
											CharacterResourceLink characterResourceLink3 = GameWideData.Get().GetCharacterResourceLink(current.Intention);
											if (characterResourceLink3 != null)
											{
												while (true)
												{
													switch (2)
													{
													case 0:
														break;
													default:
														browseCharacterImageVisible = true;
														m_blueTeamMembers[i].SetHoverCharacter(characterResourceLink3);
														goto end_IL_0224;
													}
												}
											}
										}
										goto end_IL_0224;
									}
									IL_0296:
									m_blueTeamMembers[i].SetBrowseCharacterImageVisible(false);
									UIManager.SetGameObjectActive(m_blueTeamMembers[i].m_noCharacterImage, true);
									goto end_IL_0224;
								}
							}
						}
					}
					end_IL_0224:;
				}
				m_blueTeamMembers[i].SetBrowseCharacterImageVisible(browseCharacterImageVisible);
			}
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			for (int j = 0; j < m_redTeamMembers.Length; j++)
			{
				if (data.EnemyTeamSelections.ContainsKey(m_redTeamMembers[j].PlayerID))
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					m_redTeamMembers[j].SetBrowseCharacterImageVisible(false);
					continue;
				}
				bool flag2 = false;
				if (m_playerIDsOnDeck.ContainsKey(m_redTeamMembers[j].PlayerID) && m_playerIDsOnDeck[m_redTeamMembers[j].PlayerID] != 0)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!IsBanned(m_playerIDsOnDeck[m_redTeamMembers[j].PlayerID]))
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						CharacterResourceLink characterResourceLink4 = GameWideData.Get().GetCharacterResourceLink(m_playerIDsOnDeck[m_redTeamMembers[j].PlayerID]);
						m_redTeamMembers[j].SetBrowseCharacterImageVisible(true);
						m_redTeamMembers[j].SetHoverCharacter(characterResourceLink4);
						flag2 = true;
					}
				}
				if (!flag2)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					m_redTeamMembers[j].SetBrowseCharacterImageVisible(false);
				}
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

	private void CheckSelectedCharForCenterPiece(bool isOnBlue, RankedResolutionPhaseData data)
	{
		if (m_playerIDsOnDeck.Count == 1)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					UIManager.SetGameObjectActive(m_singleSelectionCharacterSelected, true);
					using (Dictionary<int, CharacterType>.Enumerator enumerator = m_playerIDsOnDeck.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<int, CharacterType> current = enumerator.Current;
							TextMeshProUGUI singleCharacterName = m_singleCharacterName;
							Image noCharacter = singleNoSelectionCharacter;
							Image browseCharacter = singleBrowseSelectionCharacter;
							Image selectedCharacter = singleSelectionCharacter;
							Animator singleSelectionCharacterSelected = m_singleSelectionCharacterSelected;
							Animator selectedCharacterNameAnimator;
							if (isOnBlue)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								selectedCharacterNameAnimator = m_singleBlueSelectionCharacterSelected;
							}
							else
							{
								selectedCharacterNameAnimator = m_singleRedTeamSelectionCharacterSelected;
							}
							SetupSelection(current, data, singleCharacterName, noCharacter, browseCharacter, selectedCharacter, singleSelectionCharacterSelected, selectedCharacterNameAnimator, (!isOnBlue) ? m_singleRedTeamSelectedCharacter : m_singleBlueTeamSelectedCharacter, (!isOnBlue) ? m_singleRedTeamPlayerName : m_singleBlueTeamPlayerName, isOnBlue, !isOnBlue);
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
				}
				}
			}
		}
		bool flag = !doubleSelectionLeftCharacter.gameObject.activeInHierarchy;
		using (Dictionary<int, CharacterType>.Enumerator enumerator2 = m_playerIDsOnDeck.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				KeyValuePair<int, CharacterType> current2 = enumerator2.Current;
				if (flag)
				{
					flag = false;
					TextMeshProUGUI leftCharacterName = m_leftCharacterName;
					Image noCharacter2 = doubleNoSelectionLeftCharacter;
					Image browseCharacter2 = doubleBrowseSelectionLeftCharacter;
					Image selectedCharacter2 = doubleSelectionLeftCharacter;
					Animator doubleLeftSelectionCharacterSelected = m_doubleLeftSelectionCharacterSelected;
					Animator selectedCharacterNameAnimator2;
					if (isOnBlue)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						selectedCharacterNameAnimator2 = m_doubleLeftBlueSelectionCharacterSelected;
					}
					else
					{
						selectedCharacterNameAnimator2 = m_doubleLeftRedTeamSelectionCharacterSelected;
					}
					TextMeshProUGUI selectedCharacterText;
					if (isOnBlue)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						selectedCharacterText = m_doubleLeftBlueTeamSelectedCharacter;
					}
					else
					{
						selectedCharacterText = m_doubleLeftRedTeamSelectedCharacter;
					}
					SetupSelection(current2, data, leftCharacterName, noCharacter2, browseCharacter2, selectedCharacter2, doubleLeftSelectionCharacterSelected, selectedCharacterNameAnimator2, selectedCharacterText, (!isOnBlue) ? m_doubleLeftRedTeamPlayerName : m_doubleLeftBlueTeamPlayerName, isOnBlue, !isOnBlue);
				}
				else
				{
					TextMeshProUGUI rightCharacterName = m_rightCharacterName;
					Image noCharacter3 = doubleNoSelectionRightCharacter;
					Image browseCharacter3 = doubleBrowseSelectionRightCharacter;
					Image selectedCharacter3 = doubleSelectionRightCharacter;
					Animator doubleRightSelectionCharacterSelected = m_doubleRightSelectionCharacterSelected;
					Animator selectedCharacterNameAnimator3 = (!isOnBlue) ? m_doubleRightRedTeamSelectionCharacterSelected : m_doubleRightBlueSelectionCharacterSelected;
					TextMeshProUGUI selectedCharacterText2 = (!isOnBlue) ? m_doubleRightRedTeamSelectedCharacter : m_doubleRightBlueTeamSelectedCharacter;
					TextMeshProUGUI playerName;
					if (isOnBlue)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						playerName = m_doubleRightBlueTeamPlayerName;
					}
					else
					{
						playerName = m_doubleRightRedTeamPlayerName;
					}
					SetupSelection(current2, data, rightCharacterName, noCharacter3, browseCharacter3, selectedCharacter3, doubleRightSelectionCharacterSelected, selectedCharacterNameAnimator3, selectedCharacterText2, playerName, isOnBlue, !isOnBlue);
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
	}

	private void UpdatePlayerSelecting(RankedResolutionPhaseData data)
	{
		for (int i = 0; i < m_blueTeamMembers.Length; i++)
		{
			m_blueTeamMembers[i].SetAsSelecting(data._001D(m_blueTeamMembers[i].PlayerID));
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int j = 0; j < m_redTeamMembers.Length; j++)
			{
				m_redTeamMembers[j].SetAsSelecting(data._001D(m_redTeamMembers[j].PlayerID));
			}
			return;
		}
	}

	private void DoCharacterSelectContainerActiveCheck()
	{
		int num;
		if (m_lastDraftNotification != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = (m_lastDraftNotification.SubPhase.IsPickBanSubPhase() ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		UIManager.SetGameObjectActive(m_characterSelectContainer, true);
		UICharacterPanelSelectRankModeButton[] componentsInChildren = m_characterSelectContainer.GetComponentsInChildren<UICharacterPanelSelectRankModeButton>(true);
		foreach (UICharacterPanelSelectRankModeButton obj in componentsInChildren)
		{
			int clickable;
			if (!flag)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				clickable = ((SelectedCharacter == CharacterType.None) ? 1 : 0);
			}
			else
			{
				clickable = 1;
			}
			obj.SetClickable((byte)clickable != 0);
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

	private void UpdateRankData(RankedResolutionPhaseData data, bool updateFromGameInfoUpdate = false)
	{
		if (LastGameInfo == null)
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_timeInPhase = data.TimeLeftInSubPhase;
			if (!updateFromGameInfoUpdate)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				m_phaseStartTime = Time.time;
			}
			m_IsOnDeck = data._001D(OurPlayerId);
			DoCharacterSelectContainerActiveCheck();
			int intendedLockInBtnStatus;
			if (m_IsOnDeck)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!IsBanned(HoveredCharacter) && !m_selectedCharacterTypes.Contains(HoveredCharacter))
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						intendedLockInBtnStatus = (m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase() ? 1 : 0);
					}
					else
					{
						intendedLockInBtnStatus = 1;
					}
					goto IL_00da;
				}
			}
			intendedLockInBtnStatus = 0;
			goto IL_00da;
			IL_00da:
			m_intendedLockInBtnStatus = ((byte)intendedLockInBtnStatus != 0);
			if (m_IsOnDeck)
			{
				while (true)
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
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					RankedResolutionPlayerState rankedResolutionPlayerState = data.PlayersOnDeck.Find((RankedResolutionPlayerState p) => p.PlayerId == OurPlayerId);
					HoveredCharacter = rankedResolutionPlayerState.Intention;
				}
			}
			else
			{
				HoveredCharacter = CharacterType.None;
			}
			SetFreelancerSettingButtonsVisible(m_currentState >= CenterNotification.LoadoutPhase);
			UIManager.SetGameObjectActive(m_lockFreelancerContainer, false);
			if (m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (LastGameInfo.GameStatus == GameStatus.FreelancerSelecting)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!m_stateQueues.Contains(CenterNotification.TradePhase))
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						if (m_currentState != CenterNotification.TradePhase)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							QueueCenterState(CenterNotification.TradePhase);
						}
					}
				}
			}
			if (!m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				for (int i = 0; i < m_blueBans.Length; i++)
				{
					m_blueBans[i].SetAsSelecting(false);
					m_redBans[i].SetAsSelecting(false);
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			for (int j = 0; j < m_blueBans.Length; j++)
			{
				while (true)
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
				if (m_blueBans[j] != null)
				{
					while (true)
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
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						m_blueBans[j].SetSelectedCharacterImageVisible(true);
						if (m_blueBans[j].GetSelectedCharacter() == CharacterType.None)
						{
							UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeBanPlayer);
							CheckSelectedCharForCenterPiece(true, data);
						}
						m_blueBans[j].SetCharacter(characterResourceLink);
					}
				}
				if (!m_friendlyBannedCharacterTypes.Contains(characterType))
				{
					m_friendlyBannedCharacterTypes.Add(characterType);
				}
			}
			for (int k = 0; k < m_redBans.Length; k++)
			{
				while (true)
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
				if (m_redBans[k] != null)
				{
					while (true)
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
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						m_redBans[k].SetSelectedCharacterImageVisible(true);
						if (m_redBans[k].GetSelectedCharacter() == CharacterType.None)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModeBanPlayer);
							CheckSelectedCharForCenterPiece(false, data);
						}
						m_redBans[k].SetCharacter(characterResourceLink2);
					}
				}
				if (!m_enemyBannedCharacterTypes.Contains(characterType2))
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					m_enemyBannedCharacterTypes.Add(characterType2);
				}
			}
			long accountId = ClientGameManager.Get().GetPlayerAccountData().AccountId;
			for (int l = 0; l < m_blueTeamMembers.Length; l++)
			{
				m_blueTeamMembers[l].CanBeTraded = false;
				bool selectedCharacterImageVisible = false;
				if (data.FriendlyTeamSelections.ContainsKey(m_blueTeamMembers[l].PlayerID))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					CharacterResourceLink characterResourceLink3 = GameWideData.Get().GetCharacterResourceLink(data.FriendlyTeamSelections[m_blueTeamMembers[l].PlayerID]);
					if (characterResourceLink3 != null)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						selectedCharacterImageVisible = true;
						if (m_blueTeamMembers[l].GetSelectedCharacter() == CharacterType.None)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModePickPlayer);
							CheckSelectedCharForCenterPiece(true, data);
						}
						m_blueTeamMembers[l].SetCharacter(characterResourceLink3);
						if (!IsBanned(characterResourceLink3.m_characterType))
						{
							m_friendlyBannedCharacterTypes.Add(characterResourceLink3.m_characterType);
						}
						m_blueTeamMembers[l].CanBeTraded = !DidPlayerLockInDuringSwapPhase(data, m_blueTeamMembers[l].PlayerID);
					}
					if (m_blueTeamMembers[l].AccountID == accountId)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						SelectedCharacter = m_blueTeamMembers[l].GetSelectedCharacter();
					}
				}
				m_blueTeamMembers[l].SetTradePhase(m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE);
				if (m_lastDraftNotification.SubPhase != FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
				{
					m_blueTeamMembers[l].SetCharacterLocked(false);
				}
				UIManager.SetGameObjectActive(m_blueTeamMembers[l], true);
				m_blueTeamMembers[l].SetSelectedCharacterImageVisible(selectedCharacterImageVisible);
			}
			for (int m = 0; m < m_redTeamMembers.Length; m++)
			{
				bool selectedCharacterImageVisible2 = false;
				if (data.EnemyTeamSelections.ContainsKey(m_redTeamMembers[m].PlayerID))
				{
					CharacterResourceLink characterResourceLink4 = GameWideData.Get().GetCharacterResourceLink(data.EnemyTeamSelections[m_redTeamMembers[m].PlayerID]);
					if (characterResourceLink4 != null)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						selectedCharacterImageVisible2 = true;
						if (m_redTeamMembers[m].GetSelectedCharacter() == CharacterType.None)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							UIFrontEnd.PlaySound(FrontEndButtonSounds.RankModePickPlayer);
							CheckSelectedCharForCenterPiece(false, data);
						}
						m_redTeamMembers[m].SetCharacter(characterResourceLink4);
						if (!IsBanned(characterResourceLink4.m_characterType))
						{
							m_enemyBannedCharacterTypes.Add(characterResourceLink4.m_characterType);
						}
					}
				}
				UIManager.SetGameObjectActive(m_redTeamMembers[m], true);
				m_redTeamMembers[m].SetSelectedCharacterImageVisible(selectedCharacterImageVisible2);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (m_currentCharacterPage == -1)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					SetPageIndex(0);
				}
				CheckCharacterListValidity();
				return;
			}
		}
	}

	public Team GetCurrentTeam(RankedResolutionPhaseData data)
	{
		if (LastTeamInfo != null && LastPlayerInfo != null)
		{
			foreach (LobbyPlayerInfo item in LastTeamInfo.TeamPlayerInfo)
			{
				if (data._001D(item.PlayerId))
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							if (1 == 0)
							{
								/*OpCode not supported: LdMemberToken*/;
							}
							return item.TeamId;
						}
					}
				}
			}
			if (m_lastDraftNotification != null)
			{
				if (!m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase())
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					if (!m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
					{
						goto IL_00ed;
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				if (LastPlayerInfo.TeamId == Team.Spectator)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							break;
						default:
							return Team.TeamA;
						}
					}
				}
				return LastPlayerInfo.TeamId.OtherTeam();
			}
		}
		goto IL_00ed;
		IL_00ed:
		return Team.Invalid;
	}

	private void SetCenterBackground(bool isOnBlue, bool isOnRed)
	{
		for (int i = 0; i < singleSelectionBlueTeam.Length; i++)
		{
			UIManager.SetGameObjectActive(singleSelectionBlueTeam[i], isOnBlue);
		}
		for (int j = 0; j < singleSelectionRedTeam.Length; j++)
		{
			UIManager.SetGameObjectActive(singleSelectionRedTeam[j], isOnRed);
		}
		for (int k = 0; k < doubleSelectionBlueTeam.Length; k++)
		{
			UIManager.SetGameObjectActive(doubleSelectionBlueTeam[k], isOnBlue);
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int l = 0; l < doubleSelectionRedTeam.Length; l++)
			{
				UIManager.SetGameObjectActive(doubleSelectionRedTeam[l], isOnRed);
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
	}

	private void PrintData(RankedResolutionPhaseData data)
	{
		string str = "LAST RANKED RESOLUTION PHASE DATA!\n";
		str += "Blue team info:\n";
		for (int i = 0; i < m_blueTeamMembers.Length; i++)
		{
			using (List<RankedResolutionPlayerState>.Enumerator enumerator = data.UnselectedPlayerStates.GetEnumerator())
			{
				while (true)
				{
					if (!enumerator.MoveNext())
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						break;
					}
					RankedResolutionPlayerState current = enumerator.Current;
					if (current.PlayerId == m_blueTeamMembers[i].PlayerID)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
								if (1 == 0)
								{
									/*OpCode not supported: LdMemberToken*/;
								}
								str += $"PlayerID is {current.PlayerId}, PlayerName is {m_blueTeamMembers[i].m_playerName}, Is On deck: {current.OnDeckness}\n";
								goto end_IL_002a;
							}
						}
					}
				}
				end_IL_002a:;
			}
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			str += "Red team info:\n";
			for (int j = 0; j < m_redTeamMembers.Length; j++)
			{
				using (List<RankedResolutionPlayerState>.Enumerator enumerator2 = data.UnselectedPlayerStates.GetEnumerator())
				{
					while (true)
					{
						if (!enumerator2.MoveNext())
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							break;
						}
						RankedResolutionPlayerState current2 = enumerator2.Current;
						if (current2.PlayerId == m_redTeamMembers[j].PlayerID)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									str += $"PlayerID is {current2.PlayerId}, PlayerName is {m_redTeamMembers[j].m_playerName.text}, Is On deck: {current2.OnDeckness}\n";
									goto end_IL_0100;
								}
							}
						}
					}
					end_IL_0100:;
				}
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				Debug.Log(str);
				return;
			}
		}
	}

	private void UpdateCenter(RankedResolutionPhaseData data, bool updateFromGameInfoUpdate)
	{
		if (LastGameInfo == null)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			bool flag = false;
			bool flag2 = false;
			int num = 0;
			while (true)
			{
				if (num < m_blueTeamMembers.Length)
				{
					if (data._001D(m_blueTeamMembers[num].PlayerID))
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						flag = true;
						break;
					}
					num++;
					continue;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
			int num2 = 0;
			while (true)
			{
				if (num2 < m_redTeamMembers.Length)
				{
					if (data._001D(m_redTeamMembers[num2].PlayerID))
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						flag2 = true;
						break;
					}
					num2++;
					continue;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
			bool flag3 = true;
			foreach (RankedResolutionPlayerState item in data.PlayersOnDeck)
			{
				if (m_playerIDsOnDeck.ContainsKey(item.PlayerId))
				{
					while (true)
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
			if (m_lastDraftNotification.SubPhase == m_lastPhaseForUpdateCenter)
			{
				if (!flag3)
				{
					foreach (RankedResolutionPlayerState item2 in data.PlayersOnDeck)
					{
						m_playerIDsOnDeck[item2.PlayerId] = item2.Intention;
					}
					goto IL_039c;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (m_playerIDsOnDeck.Count > 0)
			{
				while (true)
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
				for (int i = 0; i < m_blueTeamMembers.Length; i++)
				{
					if (m_playerIDsOnDeck.ContainsKey(m_blueTeamMembers[i].PlayerID))
					{
						flag4 = true;
						break;
					}
				}
				for (int j = 0; j < m_redTeamMembers.Length; j++)
				{
					if (m_playerIDsOnDeck.ContainsKey(m_redTeamMembers[j].PlayerID))
					{
						while (true)
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
					if (m_playerIDsOnDeck.Count == 1)
					{
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						QueueCenterState(CenterNotification.RedTeamSingleSelectEnd);
					}
					else
					{
						QueueCenterState(CenterNotification.RedTeamDoubleSelectEnd);
					}
				}
				else if (flag4)
				{
					if (m_playerIDsOnDeck.Count == 1)
					{
						QueueCenterState(CenterNotification.BlueTeamSingleSelectEnd);
					}
					else
					{
						QueueCenterState(CenterNotification.BlueTeamDoubleSelectEnd);
					}
				}
			}
			if (!m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
			{
				if (!m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase())
				{
					if (m_lastDraftNotification.SubPhase == FreelancerResolutionPhaseSubType.FREELANCER_TRADE)
					{
						QueueCenterState(CenterNotification.TradePhase);
					}
					goto IL_02e7;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			CenterNotification centerNotification = CenterNotification.None;
			int count = data.PlayersOnDeck.Count;
			if (flag2)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				QueueCenterState(CenterNotification.RedTeamNotification);
				switch (count)
				{
				case 1:
					centerNotification = CenterNotification.RedTeamSingleSelectStart;
					break;
				case 2:
					centerNotification = CenterNotification.RedTeamDoubleSelectStart;
					break;
				}
			}
			else if (flag)
			{
				QueueCenterState(CenterNotification.BlueTeamNotification);
				if (count == 1)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					centerNotification = CenterNotification.BlueTeamSingleSelectStart;
				}
				else if (count == 2)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					centerNotification = CenterNotification.BlueTeamDoubleSelectStart;
				}
			}
			if (centerNotification != 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				QueueCenterState(centerNotification);
			}
			goto IL_02e7;
			IL_039c:
			if (!updateFromGameInfoUpdate)
			{
				UpdateCenterVisuals(data, flag, flag2);
			}
			m_lastPhaseForUpdateCenter = m_lastDraftNotification.SubPhase;
			return;
			IL_02e7:
			m_playerIDsOnDeck.Clear();
			using (List<RankedResolutionPlayerState>.Enumerator enumerator3 = data.PlayersOnDeck.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					RankedResolutionPlayerState current3 = enumerator3.Current;
					m_playerIDsOnDeck.Add(current3.PlayerId, current3.Intention);
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			goto IL_039c;
		}
	}

	private void UpdateCenterVisuals(RankedResolutionPhaseData data, bool isOnBlueTeam, bool isOnRedTeam)
	{
		if (m_playerIDsOnDeck.Count == 1)
		{
			foreach (KeyValuePair<int, CharacterType> item in m_playerIDsOnDeck)
			{
				RankedResolutionPhaseData data2 = data;
				TextMeshProUGUI singleCharacterName = m_singleCharacterName;
				Image noCharacter = singleNoSelectionCharacter;
				Image browseCharacter = singleBrowseSelectionCharacter;
				Image selectedCharacter = singleSelectionCharacter;
				Animator singleSelectionCharacterSelected = m_singleSelectionCharacterSelected;
				Animator selectedCharacterNameAnimator = (!isOnBlueTeam) ? m_singleRedTeamSelectionCharacterSelected : m_singleBlueSelectionCharacterSelected;
				TextMeshProUGUI selectedCharacterText;
				if (isOnBlueTeam)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					selectedCharacterText = m_singleBlueTeamSelectedCharacter;
				}
				else
				{
					selectedCharacterText = m_singleRedTeamSelectedCharacter;
				}
				SetupSelection(item, data2, singleCharacterName, noCharacter, browseCharacter, selectedCharacter, singleSelectionCharacterSelected, selectedCharacterNameAnimator, selectedCharacterText, (!isOnBlueTeam) ? m_singleRedTeamPlayerName : m_singleBlueTeamPlayerName, isOnBlueTeam, isOnRedTeam);
			}
		}
		else
		{
			if (m_playerIDsOnDeck.Count != 2)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				bool flag = true;
				using (Dictionary<int, CharacterType>.Enumerator enumerator2 = m_playerIDsOnDeck.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						KeyValuePair<int, CharacterType> current2 = enumerator2.Current;
						if (flag)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							RankedResolutionPhaseData data3 = data;
							TextMeshProUGUI leftCharacterName = m_leftCharacterName;
							Image noCharacter2 = doubleNoSelectionLeftCharacter;
							Image browseCharacter2 = doubleBrowseSelectionLeftCharacter;
							Image selectedCharacter2 = doubleSelectionLeftCharacter;
							object selectedCharacterAnimator;
							if (m_playerIDsOnDeck.Count == data.PlayersOnDeck.Count)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								selectedCharacterAnimator = m_doubleLeftSelectionCharacterSelected;
							}
							else
							{
								selectedCharacterAnimator = null;
							}
							Animator selectedCharacterNameAnimator2;
							if (isOnBlueTeam)
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								selectedCharacterNameAnimator2 = m_doubleLeftBlueSelectionCharacterSelected;
							}
							else
							{
								selectedCharacterNameAnimator2 = m_doubleLeftRedTeamSelectionCharacterSelected;
							}
							TextMeshProUGUI selectedCharacterText2;
							if (isOnBlueTeam)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								selectedCharacterText2 = m_doubleLeftBlueTeamSelectedCharacter;
							}
							else
							{
								selectedCharacterText2 = m_doubleLeftRedTeamSelectedCharacter;
							}
							SetupSelection(current2, data3, leftCharacterName, noCharacter2, browseCharacter2, selectedCharacter2, (Animator)selectedCharacterAnimator, selectedCharacterNameAnimator2, selectedCharacterText2, (!isOnBlueTeam) ? m_doubleLeftRedTeamPlayerName : m_doubleLeftBlueTeamPlayerName, isOnBlueTeam, isOnRedTeam);
							flag = false;
						}
						else
						{
							RankedResolutionPhaseData data4 = data;
							TextMeshProUGUI rightCharacterName = m_rightCharacterName;
							Image noCharacter3 = doubleNoSelectionRightCharacter;
							Image browseCharacter3 = doubleBrowseSelectionRightCharacter;
							Image selectedCharacter3 = doubleSelectionRightCharacter;
							object selectedCharacterAnimator2;
							if (m_playerIDsOnDeck.Count == data.PlayersOnDeck.Count)
							{
								while (true)
								{
									switch (6)
									{
									case 0:
										continue;
									}
									break;
								}
								selectedCharacterAnimator2 = m_doubleRightSelectionCharacterSelected;
							}
							else
							{
								selectedCharacterAnimator2 = null;
							}
							Animator selectedCharacterNameAnimator3;
							if (isOnBlueTeam)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								selectedCharacterNameAnimator3 = m_doubleRightBlueSelectionCharacterSelected;
							}
							else
							{
								selectedCharacterNameAnimator3 = m_doubleRightRedTeamSelectionCharacterSelected;
							}
							TextMeshProUGUI selectedCharacterText3;
							if (isOnBlueTeam)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								selectedCharacterText3 = m_doubleRightBlueTeamSelectedCharacter;
							}
							else
							{
								selectedCharacterText3 = m_doubleRightRedTeamSelectedCharacter;
							}
							SetupSelection(current2, data4, rightCharacterName, noCharacter3, browseCharacter3, selectedCharacter3, (Animator)selectedCharacterAnimator2, selectedCharacterNameAnimator3, selectedCharacterText3, (!isOnBlueTeam) ? m_doubleRightRedTeamPlayerName : m_doubleRightBlueTeamPlayerName, isOnBlueTeam, isOnRedTeam);
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
			}
		}
	}

	public void SetupFreelancerSelect(RankedResolutionPhaseData data)
	{
		if (LastGameInfo == null)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (HoveredCharacter == m_selectedSubPhaseCharacter)
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (m_selectedSubPhaseCharacter == CharacterType.None)
				{
					return;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					if (!data._001D(LastPlayerInfo.PlayerId))
					{
						return;
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						if (m_selectedCharacterTypes.Contains(m_selectedSubPhaseCharacter))
						{
							return;
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							if (!IsBanned(m_selectedSubPhaseCharacter))
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									ClientGameManager.Get().UpdateSelectedCharacter(m_selectedSubPhaseCharacter);
									ClientGameManager.Get().SendRankedHoverClickRequest(m_selectedSubPhaseCharacter);
									m_intendedLockInBtnStatus = true;
									HoveredCharacter = m_selectedSubPhaseCharacter;
									return;
								}
							}
							return;
						}
					}
				}
			}
		}
	}

	private bool CharacterSelectAnimIsPlaying()
	{
		int result;
		if (!m_doubleLeftSelectionCharacterSelected.gameObject.activeSelf)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!m_doubleRightSelectionCharacterSelected.gameObject.activeSelf)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				result = (m_singleSelectionCharacterSelected.gameObject.activeSelf ? 1 : 0);
				goto IL_005a;
			}
		}
		result = 1;
		goto IL_005a;
		IL_005a:
		return (byte)result != 0;
	}

	private void SetupSelection(KeyValuePair<int, CharacterType> selectedChar, RankedResolutionPhaseData data, TextMeshProUGUI nameDisplay, Image NoCharacter, Image BrowseCharacter, Image SelectedCharacter, Animator SelectedCharacterAnimator, Animator SelectedCharacterNameAnimator, TextMeshProUGUI SelectedCharacterText, TextMeshProUGUI PlayerName, bool isFriendly, bool isEnemy)
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = true;
		if (!m_selectedCharacterTypes.Contains(selectedChar.Value))
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!IsBanned(selectedChar.Value) && !data.FriendlyBans.Contains(selectedChar.Value))
			{
				while (true)
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
					goto IL_0088;
				}
				while (true)
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
		goto IL_0088;
		IL_05a8:
		if (m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase() && !m_playerIDsThatSelected.Contains(selectedChar.Key))
		{
			m_playerIDsThatSelected.Add(selectedChar.Key);
		}
		goto IL_05e1;
		IL_05e1:
		if (!flag2)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			bool flag4 = true;
			if (m_playerIDsThatSelected.Count > 0 && m_playerIDsThatSelected[m_playerIDsThatSelected.Count - 1] == selectedChar.Key)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				flag4 = false;
			}
			if (m_animatorCurrentlyAnimating != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_animatorCurrentlyAnimating.gameObject.activeInHierarchy)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (m_lastSetupSelectionPhaseSubType == m_lastDraftNotification.SubPhase)
					{
						for (int i = 0; i < m_blueTeamMembers.Length; i++)
						{
							if (m_blueTeamMembers[i].PlayerID == m_playerIDBeingAnimated)
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								flag4 = false;
								PlayerName.text = m_blueTeamMembers[i].m_playerName.text;
							}
						}
					}
				}
			}
			if (flag4)
			{
				PlayerName.text = string.Empty;
			}
		}
		m_lastSetupSelectionPhaseSubType = m_lastDraftNotification.SubPhase;
		if (!flag && !CharacterSelectAnimIsPlaying())
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (selectedChar.Value != 0 && flag3)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				BrowseCharacter.sprite = GameWideData.Get().GetCharacterResourceLink(selectedChar.Value).ActorDataPrefab.GetComponent<ActorData>().GetAliveHUDIcon();
				UIManager.SetGameObjectActive(NoCharacter, false);
				UIManager.SetGameObjectActive(BrowseCharacter, true);
				UIManager.SetGameObjectActive(SelectedCharacter, false);
			}
			else
			{
				UIManager.SetGameObjectActive(NoCharacter, true);
				UIManager.SetGameObjectActive(BrowseCharacter, false);
				UIManager.SetGameObjectActive(SelectedCharacter, false);
			}
		}
		if (!flag)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag3 && !CharacterSelectAnimIsPlaying())
			{
				UIManager.SetGameObjectActive(NoCharacter, true);
				UIManager.SetGameObjectActive(BrowseCharacter, false);
				UIManager.SetGameObjectActive(SelectedCharacter, false);
			}
		}
		nameDisplay.text = string.Empty;
		return;
		IL_0088:
		if (isFriendly)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			bool flag5;
			if (m_lastSetupSelectionPhaseSubType.IsPickBanSubPhase())
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				flag5 = data.FriendlyBans.Contains(selectedChar.Value);
			}
			else if (m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				flag5 = data.FriendlyTeamSelections.ContainsKey(selectedChar.Key);
			}
			else
			{
				flag5 = false;
			}
			if (flag5)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				CharacterType value = selectedChar.Value;
				if (value != 0)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = true;
					bool flag6 = true;
					if (m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						if (m_playerIDsThatSelected.Contains(selectedChar.Key))
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							flag6 = false;
						}
					}
					if (flag6)
					{
						SelectedCharacterText.text = value.GetDisplayName();
						SelectedCharacter.sprite = GameWideData.Get().GetCharacterResourceLink(value).ActorDataPrefab.GetComponent<ActorData>().GetAliveHUDIcon();
						UIManager.SetGameObjectActive(NoCharacter, false);
						UIManager.SetGameObjectActive(BrowseCharacter, false);
						UIManager.SetGameObjectActive(SelectedCharacter, true);
						if (SelectedCharacterAnimator != null)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							UIManager.SetGameObjectActive(SelectedCharacterAnimator, true);
							m_playerIDBeingAnimated = selectedChar.Key;
							m_animatorCurrentlyAnimating = SelectedCharacterAnimator;
						}
						UIManager.SetGameObjectActive(SelectedCharacterNameAnimator, true);
						UIManager.SetGameObjectActive(SelectedCharacterNameAnimator.transform.parent, true);
						if (m_currentState != CenterNotification.BlueTeamDoubleSelectStart)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
							if (m_currentState != CenterNotification.BlueTeamSingleSelectStart)
							{
								while (true)
								{
									switch (4)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!m_stateQueues.Contains(CenterNotification.BlueTeamDoubleSelectStart))
								{
									while (true)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									if (!m_stateQueues.Contains(CenterNotification.BlueTeamSingleSelectStart))
									{
										goto IL_02ca;
									}
									while (true)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
								}
								while (m_currentState != CenterNotification.BlueTeamDoubleSelectStart)
								{
									while (true)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									if (m_currentState != CenterNotification.BlueTeamSingleSelectStart)
									{
										m_currentState = m_stateQueues[0];
										m_stateQueues.RemoveAt(0);
										continue;
									}
									while (true)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									break;
								}
								DoQueueState(m_currentState);
							}
						}
					}
					goto IL_02ca;
				}
			}
		}
		goto IL_037f;
		IL_02ca:
		if (m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!m_playerIDsThatSelected.Contains(selectedChar.Key))
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				m_playerIDsThatSelected.Add(selectedChar.Key);
				for (int j = 0; j < m_blueTeamMembers.Length; j++)
				{
					if (m_blueTeamMembers[j].PlayerID == selectedChar.Key)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
						flag2 = true;
						PlayerName.text = m_blueTeamMembers[j].m_playerName.text;
					}
				}
				while (true)
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
		goto IL_037f;
		IL_037f:
		if (isEnemy)
		{
			bool flag7;
			if (m_lastSetupSelectionPhaseSubType.IsPickBanSubPhase())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				flag7 = data.EnemyBans.Contains(selectedChar.Value);
			}
			else if (m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				flag7 = data.EnemyTeamSelections.ContainsKey(selectedChar.Key);
			}
			else
			{
				flag7 = false;
			}
			if (flag7)
			{
				CharacterType value2 = selectedChar.Value;
				if (value2 != 0)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					flag = true;
					bool flag8 = true;
					if (m_lastSetupSelectionPhaseSubType.IsPickFreelancerSubPhase())
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (m_playerIDsThatSelected.Contains(selectedChar.Key))
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							flag8 = false;
						}
					}
					if (flag8)
					{
						while (true)
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
						UIManager.SetGameObjectActive(NoCharacter, false);
						UIManager.SetGameObjectActive(BrowseCharacter, false);
						UIManager.SetGameObjectActive(SelectedCharacter, true);
						if (SelectedCharacterAnimator != null)
						{
							UIManager.SetGameObjectActive(SelectedCharacterAnimator, true);
							m_playerIDBeingAnimated = selectedChar.Key;
							m_animatorCurrentlyAnimating = SelectedCharacterAnimator;
						}
						UIManager.SetGameObjectActive(SelectedCharacterNameAnimator, true);
						UIManager.SetGameObjectActive(SelectedCharacterNameAnimator.transform.parent, true);
						if (m_currentState != CenterNotification.RedTeamDoubleSelectStart)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							if (m_currentState != CenterNotification.RedTeamSingleSelectStart)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!m_stateQueues.Contains(CenterNotification.RedTeamDoubleSelectStart))
								{
									while (true)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
									if (!m_stateQueues.Contains(CenterNotification.RedTeamSingleSelectStart))
									{
										goto IL_05a8;
									}
								}
								while (m_currentState != CenterNotification.RedTeamDoubleSelectStart)
								{
									while (true)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									if (m_currentState != CenterNotification.RedTeamSingleSelectStart)
									{
										m_currentState = m_stateQueues[0];
										m_stateQueues.RemoveAt(0);
										continue;
									}
									while (true)
									{
										switch (4)
										{
										case 0:
											continue;
										}
										break;
									}
									break;
								}
								DoQueueState(m_currentState);
							}
						}
					}
					goto IL_05a8;
				}
			}
		}
		goto IL_05e1;
	}

	public void SetupBanSelect(RankedResolutionPhaseData data)
	{
		if (LastGameInfo == null)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			Team currentTeam = GetCurrentTeam(data);
			for (int i = 0; i < m_blueBans.Length; i++)
			{
				UIManager.SetGameObjectActive(m_blueBans[i], true);
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				for (int j = 0; j < m_redBans.Length; j++)
				{
					UIManager.SetGameObjectActive(m_redBans[j], true);
				}
				if (data.PlayersOnDeck.Count > 0)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					foreach (RankedResolutionPlayerState item in data.PlayersOnDeck)
					{
						if (item.Intention != 0)
						{
							CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(item.Intention);
							if (GetCurrentTeam(data) != LastPlayerInfo.TeamId)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								if (LastPlayerInfo.TeamId != Team.Spectator)
								{
									if (data.EnemyBans.Count < m_redBans.Length)
									{
										while (true)
										{
											switch (7)
											{
											case 0:
												continue;
											}
											break;
										}
										m_redBans[data.EnemyBans.Count].SetBrowseCharacterImageVisible(true);
										m_redBans[data.EnemyBans.Count].SetHoverCharacter(characterResourceLink);
									}
									continue;
								}
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
							}
							if (data.FriendlyBans.Count < m_blueBans.Length)
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										continue;
									}
									break;
								}
								m_blueBans[data.FriendlyBans.Count].SetBrowseCharacterImageVisible(true);
								m_blueBans[data.FriendlyBans.Count].SetHoverCharacter(characterResourceLink);
							}
						}
						else
						{
							if (data.FriendlyBans.Count < m_blueBans.Length)
							{
								m_blueBans[data.FriendlyBans.Count].SetBrowseCharacterImageVisible(false);
							}
							if (data.EnemyBans.Count < m_redBans.Length)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								m_redBans[data.EnemyBans.Count].SetBrowseCharacterImageVisible(false);
							}
						}
					}
				}
				for (int k = 0; k < m_redBans.Length; k++)
				{
					m_redBans[k].SetSelectedCharacterImageVisible(data.EnemyBans.Count > k);
					if (currentTeam != LastPlayerInfo.TeamId)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							break;
						}
						if (LastPlayerInfo.TeamId != Team.Spectator)
						{
							m_redBans[k].SetAsSelecting(data.EnemyBans.Count == k);
							continue;
						}
						while (true)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					m_redBans[k].SetAsSelecting(false);
				}
				for (int l = 0; l < m_blueBans.Length; l++)
				{
					m_blueBans[l].SetSelectedCharacterImageVisible(data.FriendlyBans.Count > l);
					if (currentTeam != LastPlayerInfo.TeamId)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						if (LastPlayerInfo.TeamId != Team.Spectator)
						{
							m_blueBans[l].SetAsSelecting(false);
							continue;
						}
						while (true)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
					}
					m_blueBans[l].SetAsSelecting(data.FriendlyBans.Count == l);
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
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				result = StringUtil.TR("SelectFreelancerBan", "RankMode");
			}
			else if (teamType == TeamType.Ally)
			{
				while (true)
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
				while (true)
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
				while (true)
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

	public void SetupInstructions(RankedResolutionPhaseData data)
	{
		Team currentTeam = GetCurrentTeam(data);
		TeamType teamType = TeamType.Any;
		if (currentTeam != 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (currentTeam != Team.TeamB)
			{
				m_MessageText.color = m_neutralColor;
				goto IL_009e;
			}
		}
		if (currentTeam == LastPlayerInfo.TeamId)
		{
			goto IL_0076;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		if (LastPlayerInfo.TeamId == Team.Spectator)
		{
			while (true)
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
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				goto IL_0076;
			}
		}
		m_MessageText.color = m_redTeamColor;
		teamType = TeamType.Enemy;
		goto IL_009e;
		IL_009e:
		if (LastGameInfo != null && LastGameInfo.GameStatus != GameStatus.Stopped)
		{
			if (LastGameInfo.GameStatus > GameStatus.FreelancerSelecting)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_MessageText.text = SubphaseToDisplayName(m_lastDraftNotification.SubPhase, teamType, data._001D(OurPlayerId));
		return;
		IL_0076:
		m_MessageText.color = m_blueTeamColor;
		teamType = TeamType.Ally;
		goto IL_009e;
	}

	public void NotifyFreelancerTrades(RankedResolutionPhaseData data)
	{
		long accountId = ClientGameManager.Get().GetPlayerAccountData().AccountId;
		int num = -1;
		bool selfLockedIn = false;
		for (int i = 0; i < m_blueTeamMembers.Length; i++)
		{
			if (m_blueTeamMembers[i].AccountID == accountId)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				num = m_blueTeamMembers[i].PlayerID;
				selfLockedIn = DidPlayerLockInDuringSwapPhase(data, m_blueTeamMembers[i].PlayerID);
			}
			m_blueTeamMembers[i].SetAsSelecting(false);
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			for (int j = 0; j < m_redTeamMembers.Length; j++)
			{
				m_redTeamMembers[j].SetAsSelecting(false);
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				for (int k = 0; k < m_blueTeamMembers.Length; k++)
				{
					UIRankedModePlayerDraftEntry.TradeStatus status = UIRankedModePlayerDraftEntry.TradeStatus.NoTrade;
					using (List<RankedTradeData>.Enumerator enumerator = data.TradeActions.GetEnumerator())
					{
						while (true)
						{
							if (!enumerator.MoveNext())
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										continue;
									}
									break;
								}
								break;
							}
							RankedTradeData current = enumerator.Current;
							if (current.TradeAction == RankedTradeData.TradeActionType._001D)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								if (current.AskedPlayerId == m_blueTeamMembers[k].PlayerID)
								{
									while (true)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
									if (current.OfferingPlayerId == num)
									{
										while (true)
										{
											switch (7)
											{
											case 0:
												break;
											default:
												status = UIRankedModePlayerDraftEntry.TradeStatus.TradeRequestSent;
												goto end_IL_00d9;
											}
										}
									}
								}
								if (current.AskedPlayerId == num)
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									if (current.OfferingPlayerId == m_blueTeamMembers[k].PlayerID)
									{
										while (true)
										{
											switch (5)
											{
											case 0:
												break;
											default:
												status = UIRankedModePlayerDraftEntry.TradeStatus.TradeRequestReceived;
												goto end_IL_00d9;
											}
										}
									}
								}
							}
							else if (current.TradeAction == RankedTradeData.TradeActionType._0012)
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										continue;
									}
									break;
								}
								if (current.OfferingPlayerId != m_blueTeamMembers[k].PlayerID)
								{
									while (true)
									{
										switch (6)
										{
										case 0:
											continue;
										}
										break;
									}
									if (current.AskedPlayerId != m_blueTeamMembers[k].PlayerID)
									{
										continue;
									}
									while (true)
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
								break;
							}
						}
						end_IL_00d9:;
					}
					m_blueTeamMembers[k].SetTradeStatus(status, m_blueTeamMembers[k].PlayerID == num, selfLockedIn);
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
		GameIsLaunching = true;
		m_MessageText.text = GameStatusToDisplayString(notification.GameInfo.GameStatus);
		for (int i = 0; i < m_blueTeamMembers.Length; i++)
		{
			m_blueTeamMembers[i].SetTradePhase(false);
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIManager.SetGameObjectActive(m_lockFreelancerContainer, false);
			if (notification.GameInfo.GameStatus >= GameStatus.Launching && notification.GameInfo.GameStatus != GameStatus.Stopped)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					SetFreelancerSettingButtonsVisible(false);
					UIRankedCharacterSelectSettingsPanel.Get().SetVisible(false);
					return;
				}
			}
			return;
		}
	}

	public void UpdateNotification(EnterFreelancerResolutionPhaseNotification notification, bool updateFromGameInfoUpdate = false)
	{
		if (GameIsLaunching)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		if (notification == null)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (notification.RankedData.HasValue)
			{
				RankedResolutionPhaseData value = notification.RankedData.Value;
				SetupInstructions(value);
				UpdateRankData(value, updateFromGameInfoUpdate);
				UpdatePlayerSelecting(value);
				switch (notification.SubPhase)
				{
				case FreelancerResolutionPhaseSubType.FREELANCER_TRADE:
					NotifyFreelancerTrades(value);
					break;
				case FreelancerResolutionPhaseSubType.PICK_BANS1:
				case FreelancerResolutionPhaseSubType.PICK_BANS2:
					SetupBanSelect(value);
					break;
				case FreelancerResolutionPhaseSubType.PICK_FREELANCER1:
				case FreelancerResolutionPhaseSubType.PICK_FREELANCER2:
					SetupFreelancerSelect(value);
					break;
				}
				UpdateHoverStatus(value);
				UpdateCenter(value, updateFromGameInfoUpdate);
			}
			return;
		}
	}

	public void NotifyButtonClicked(UICharacterPanelSelectRankModeButton btn)
	{
		if (m_IsOnDeck)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					bool intendedLockInBtnStatus = false;
					m_selectedSubPhaseCharacter = CharacterType.None;
					for (int i = 0; i < m_characterListDisplayButtons.Count; i++)
					{
						if (m_characterListDisplayButtons[i] == btn)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							m_selectedSubPhaseCharacter = m_characterListDisplayButtons[i].m_characterType;
							m_characterListDisplayButtons[i].SetSelected(true);
							if (!m_selectedCharacterTypes.Contains(m_characterListDisplayButtons[i].m_characterType))
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								if (!IsBanned(m_characterListDisplayButtons[i].m_characterType))
								{
									HoveredCharacter = m_selectedSubPhaseCharacter;
									RankedResolutionPhaseData? rankedData = m_lastDraftNotification.RankedData;
									RankedResolutionPhaseData value = rankedData.Value;
									if (m_lastDraftNotification.SubPhase.IsPickFreelancerSubPhase())
									{
										UpdateHoverSelfStatus(value);
									}
									if (!m_lastDraftNotification.SubPhase.IsPickBanSubPhase())
									{
										ClientGameManager.Get().UpdateSelectedCharacter(m_selectedSubPhaseCharacter);
									}
									ClientGameManager.Get().SendRankedHoverClickRequest(m_selectedSubPhaseCharacter);
									intendedLockInBtnStatus = true;
								}
							}
						}
						else
						{
							m_characterListDisplayButtons[i].SetSelected(false);
						}
					}
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							m_intendedLockInBtnStatus = intendedLockInBtnStatus;
							return;
						}
					}
				}
				}
			}
		}
		m_selectedSubPhaseCharacter = CharacterType.None;
		for (int j = 0; j < m_characterListDisplayButtons.Count; j++)
		{
			if (m_characterListDisplayButtons[j] == btn)
			{
				m_selectedSubPhaseCharacter = m_characterListDisplayButtons[j].m_characterType;
				m_characterListDisplayButtons[j].SetSelected(true);
				if (m_selectedCharacterTypes.Contains(m_characterListDisplayButtons[j].m_characterType))
				{
					continue;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!IsBanned(m_characterListDisplayButtons[j].m_characterType))
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					ClientGameManager.Get().UpdateSelectedCharacter(m_selectedSubPhaseCharacter);
					ClientGameManager.Get().SendRankedHoverClickRequest(m_selectedSubPhaseCharacter);
					SetupCharacterSettings(m_selectedSubPhaseCharacter);
					SetFreelancerSettingButtonsVisible(m_currentState >= CenterNotification.LoadoutPhase);
				}
			}
			else
			{
				m_characterListDisplayButtons[j].SetSelected(false);
			}
		}
	}

	public void HandleResolvingDuplicateFreelancerNotification(EnterFreelancerResolutionPhaseNotification notification)
	{
		Initialize();
		m_lastDraftNotification = notification;
		SetupPlayerLists();
		UpdateNotification(notification);
	}

	private void GetListOfVisibleCharacterTypes()
	{
		m_validCharacterTypes.Clear();
		GameManager gameManager = GameManager.Get();
		for (int i = 0; i < 40; i++)
		{
			CharacterType characterType = (CharacterType)i;
			if (!gameManager.IsCharacterAllowedForPlayers(characterType))
			{
				continue;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (gameManager.IsCharacterAllowedForGameType(characterType, GameType.Ranked, null, null))
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				m_validCharacterTypes.Add(characterType);
			}
		}
	}

	private void Initialize()
	{
		if (m_initialized)
		{
			return;
		}
		GameIsLaunching = false;
		m_lastSetupSelectionPhaseSubType = FreelancerResolutionPhaseSubType.UNDEFINED;
		m_assignedCharacterForGame = CharacterType.None;
		m_hoverCharacterForGame = CharacterType.None;
		m_selectedSubPhaseCharacter = CharacterType.None;
		UICharacterSelectCharacterSettingsPanel uICharacterSelectCharacterSettingsPanel = UIRankedCharacterSelectSettingsPanel.Get();
		if (uICharacterSelectCharacterSettingsPanel != null)
		{
			uICharacterSelectCharacterSettingsPanel.SetVisible(false);
		}
		m_selectedCharacterTypes.Clear();
		m_enemyBannedCharacterTypes.Clear();
		m_friendlyBannedCharacterTypes.Clear();
		m_playerIDsThatSelected.Clear();
		GetListOfVisibleCharacterTypes();
		m_initialized = true;
		m_MessageText.text = string.Empty;
		m_gameCountdownTimer.text = string.Empty;
		m_redCountdownTimer.text = string.Empty;
		m_blueCountdownTimer.text = string.Empty;
		m_stageText.text = string.Empty;
		ClearAllStates();
		UIManager.SetGameObjectActive(m_pagesContainer, false);
		UIManager.SetGameObjectActive(m_lockFreelancerContainer, false);
		m_intendedLockInBtnStatus = false;
		m_currentCharacterPage = -1;
		m_currentVisiblePage = -1;
		for (int i = 0; i < m_blueBans.Length; i++)
		{
			UIManager.SetGameObjectActive(m_blueBans[i], false);
			m_blueBans[i].Init();
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int j = 0; j < m_redBans.Length; j++)
			{
				UIManager.SetGameObjectActive(m_redBans[j], false);
				m_redBans[j].Init();
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				for (int k = 0; k < m_blueTeamMembers.Length; k++)
				{
					m_blueTeamMembers[k].Init();
					m_blueTeamMembers[k].SetTradePhase(false);
				}
				for (int l = 0; l < m_redTeamMembers.Length; l++)
				{
					m_redTeamMembers[l].Init();
					m_redTeamMembers[l].SetTradePhase(false);
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					for (int m = 0; m < m_characterListDisplayButtons.Count; m++)
					{
						UnityEngine.Object.Destroy(m_characterListDisplayButtons[m].gameObject);
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						m_pageButtons.Clear();
						m_characterListDisplayButtons.Clear();
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
									while (true)
									{
										switch (6)
										{
										case 0:
											break;
										default:
											if (array[n] == CharacterType.None)
											{
												while (true)
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
													while (true)
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
													while (true)
													{
														switch (7)
														{
														case 0:
															break;
														default:
															list2.Add(array[n]);
															goto end_IL_0276;
														}
													}
												}
											}
											goto end_IL_0276;
										}
									}
								}
								end_IL_0276:;
							}
							catch
							{
							}
						}
						list.Sort(CompareCharacterTypeName);
						list2.Sort(CompareCharacterTypeName);
						list3.Sort(CompareCharacterTypeName);
						int num = Mathf.CeilToInt((float)list.Count / 2f);
						int num2 = Mathf.CeilToInt((float)list2.Count / 2f);
						int num3 = Mathf.CeilToInt((float)list3.Count / 2f);
						int num4 = 0;
						for (int num5 = 0; num5 < list.Count; num5++)
						{
							UICharacterPanelSelectRankModeButton uICharacterPanelSelectRankModeButton = UnityEngine.Object.Instantiate(m_characterSelectBtnPrefab) as UICharacterPanelSelectRankModeButton;
							uICharacterPanelSelectRankModeButton.m_characterType = list[num5];
							if (num5 - num4 * num >= num)
							{
								while (true)
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
							UIManager.ReparentTransform(uICharacterPanelSelectRankModeButton.gameObject.transform, m_firePowerLayoutGroup.gameObject.transform);
							m_characterListDisplayButtons.Add(uICharacterPanelSelectRankModeButton);
						}
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							num4 = 0;
							for (int num6 = 0; num6 < list2.Count; num6++)
							{
								UICharacterPanelSelectRankModeButton uICharacterPanelSelectRankModeButton2 = UnityEngine.Object.Instantiate(m_characterSelectBtnPrefab) as UICharacterPanelSelectRankModeButton;
								uICharacterPanelSelectRankModeButton2.m_characterType = list2[num6];
								if (num6 - num4 * num2 >= num2)
								{
									num4++;
								}
								UIManager.ReparentTransform(uICharacterPanelSelectRankModeButton2.gameObject.transform, m_frontlinerLayoutGroup.gameObject.transform);
								m_characterListDisplayButtons.Add(uICharacterPanelSelectRankModeButton2);
							}
							num4 = 0;
							for (int num7 = 0; num7 < list3.Count; num7++)
							{
								UICharacterPanelSelectRankModeButton uICharacterPanelSelectRankModeButton3 = UnityEngine.Object.Instantiate(m_characterSelectBtnPrefab) as UICharacterPanelSelectRankModeButton;
								uICharacterPanelSelectRankModeButton3.m_characterType = list3[num7];
								if (num7 - num4 * num3 >= num3)
								{
									while (true)
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
								UIManager.ReparentTransform(uICharacterPanelSelectRankModeButton3.gameObject.transform, m_supportLayoutGroup.gameObject.transform);
								m_characterListDisplayButtons.Add(uICharacterPanelSelectRankModeButton3);
							}
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								SetPageIndex(0);
								UIManager.SetGameObjectActive(m_introContainer, true);
								m_draftScreenContainer.GetComponent<CanvasGroup>().alpha = 1f;
								return;
							}
						}
					}
				}
			}
		}
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
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			btn.SetSelected(false, false, string.Empty, string.Empty);
			btn.spriteController.callback = PageClicked;
			return;
		}
	}

	private void PageClicked(BaseEventData data)
	{
		for (int i = 0; i < m_pageButtons.Count; i++)
		{
			if (!(m_pageButtons[i].spriteController.m_hitBoxImage.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject))
			{
				continue;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				SetPageIndex(i);
				return;
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

	private bool IsCharacterTypeSelectable(CharacterType type)
	{
		if (!m_selectedCharacterTypes.Contains(type))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!IsBanned(type))
			{
				if (!GameManager.Get().IsCharacterAllowedForGameType(type, GameType.Ranked, null, null))
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return false;
						}
					}
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
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (playerCharacterData.CharacterComponent.Unlocked)
			{
				goto IL_0044;
			}
		}
		if (!clientGameManager.IsCharacterAvailable(type, GameType.Ranked))
		{
			return false;
		}
		goto IL_0044;
		IL_0044:
		return true;
	}

	private void CheckCharacterListValidity()
	{
		bool flag = m_lastDraftNotification != null && m_lastDraftNotification.SubPhase.IsPickBanSubPhase();
		for (int i = 0; i < m_characterListDisplayButtons.Count; i++)
		{
			CharacterType characterType = m_characterListDisplayButtons[i].m_characterType;
			bool flag2 = IsCharacterAvailableForPlayer(characterType);
			int num;
			if (IsCharacterTypeSelectable(characterType))
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				if (!flag2)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					num = (flag ? 1 : 0);
				}
				else
				{
					num = 1;
				}
			}
			else
			{
				num = 0;
			}
			bool enabled = (byte)num != 0;
			if (IsCharacterVisibleForPlayer(characterType))
			{
				m_characterListDisplayButtons[i].SetEnabled(enabled, ClientGameManager.Get().GetPlayerCharacterData(characterType));
				m_characterListDisplayButtons[i].SetSelected(HoveredCharacter == m_characterListDisplayButtons[i].m_characterType || SelectedCharacter == m_characterListDisplayButtons[i].m_characterType || m_selectedSubPhaseCharacter == m_characterListDisplayButtons[i].m_characterType);
				UIManager.SetGameObjectActive(m_characterListDisplayButtons[i], true);
			}
			else
			{
				UIManager.SetGameObjectActive(m_characterListDisplayButtons[i], false);
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

	private void UpdateCharacterButtons()
	{
		m_currentVisiblePage = m_currentCharacterPage;
		int num;
		if (m_lastDraftNotification != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			num = (m_lastDraftNotification.SubPhase.IsPickBanSubPhase() ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		for (int i = 0; i < m_characterListDisplayButtons.Count; i++)
		{
			CharacterType characterType = m_characterListDisplayButtons[i].m_characterType;
			bool flag2 = IsCharacterAvailableForPlayer(characterType);
			int num2;
			if (IsCharacterTypeSelectable(characterType))
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!flag2)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					num2 = (flag ? 1 : 0);
				}
				else
				{
					num2 = 1;
				}
			}
			else
			{
				num2 = 0;
			}
			bool isAvailable = (byte)num2 != 0;
			UICharacterPanelSelectRankModeButton uICharacterPanelSelectRankModeButton;
			int selected;
			if (IsCharacterVisibleForPlayer(characterType))
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				uICharacterPanelSelectRankModeButton = m_characterListDisplayButtons[i];
				if (HoveredCharacter != m_characterListDisplayButtons[i].m_characterType)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (SelectedCharacter != m_characterListDisplayButtons[i].m_characterType)
					{
						selected = ((m_selectedSubPhaseCharacter == m_characterListDisplayButtons[i].m_characterType) ? 1 : 0);
						goto IL_0111;
					}
				}
				selected = 1;
				goto IL_0111;
			}
			UIManager.SetGameObjectActive(m_characterListDisplayButtons[i], false);
			continue;
			IL_0111:
			uICharacterPanelSelectRankModeButton.Setup(isAvailable, (byte)selected != 0);
			UIManager.SetGameObjectActive(m_characterListDisplayButtons[i], true);
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

	private void SetPageIndex(int index)
	{
		UpdateCharacterButtons();
		if (!IsVisible)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		Initialize();
	}

	public void DismantleRankDraft()
	{
		m_assignedCharacterForGame = CharacterType.None;
		m_hoverCharacterForGame = CharacterType.None;
		m_selectedSubPhaseCharacter = CharacterType.None;
		m_lastSetupSelectionPhaseSubType = FreelancerResolutionPhaseSubType.UNDEFINED;
		IsVisible = false;
		m_initialized = false;
		LastGameInfo = null;
		LastPlayerInfo = null;
		LastTeamInfo = null;
		m_lastDraftNotification = null;
		m_currentState = CenterNotification.None;
		m_stateQueues.Clear();
		m_playerIDsOnDeck.Clear();
		ClearAllStates();
		UIManager.SetGameObjectActive(UIFrontEnd.Get().m_frontEndNavPanel, true);
		UIManager.SetGameObjectActive(m_draftScreenContainer, false);
		UIRankedCharacterSelectSettingsPanel.Get().SetVisible(false);
		UIManager.SetGameObjectActive(m_singleSelectionCharacterSelected, false);
		UIManager.SetGameObjectActive(m_doubleRightSelectionCharacterSelected, false);
		UIManager.SetGameObjectActive(m_doubleLeftSelectionCharacterSelected, false);
		for (int i = 0; i < m_blueTeamMembers.Length; i++)
		{
			m_blueTeamMembers[i].Dismantle();
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			for (int j = 0; j < m_redTeamMembers.Length; j++)
			{
				m_redTeamMembers[j].Dismantle();
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
	}

	public void SetupRankDraft()
	{
		UIPlayerProgressPanel.Get().SetVisible(false);
		IsVisible = true;
		UIFrontEnd.Get().m_frontEndNavPanel.SetNavButtonSelected(UIFrontEnd.Get().m_frontEndNavPanel.m_PlayBtn);
		UIStorePanel.Get().ClosePurchaseDialog();
		UIRankedModeSelectScreen.Get().SetVisible(false);
		UIManager.SetGameObjectActive(UIFrontEnd.Get().m_frontEndNavPanel, false);
		UIManager.SetGameObjectActive(m_draftScreenContainer, true);
		UIRAFProgramScreen.Get().SetVisible(false);
		Initialize();
	}

	public void SetDraftScreenVisible(bool visible)
	{
		if (!IsVisible)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_draftScreenContainer, visible);
		if (!visible)
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (m_containerAC == null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				m_containerAC = m_draftScreenContainer.GetComponent<Animator>();
			}
			if (m_containerAC != null)
			{
				m_containerAC.Play("RankedModeSetup", 0, 1f);
			}
			return;
		}
	}
}
