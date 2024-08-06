using LobbyGameClientMessages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIPlayerProgressPanel : UIScene
{
	public Animator m_animationController;

	public RectTransform m_playerProgressClickBlocker;

	public _ButtonSwapSprite m_closeBtn;

	[Header("Top Bar")]
	public TextMeshProUGUI m_playerName;

	public TextMeshProUGUI m_playerTitle;

	public TextMeshProUGUI m_matchesPlayedText;

	public TextMeshProUGUI m_freelancerLevelsText;

	public TextMeshProUGUI m_reactorLevelText;

	public TextMeshProUGUI m_achievementPointsText;

	public TextMeshProUGUI m_ggBoostsUsedText;

	public TextMeshProUGUI m_totalWinsText;

	[Header("Tabs")]
	public UIContentNavButton m_banner;

	public UIContentNavButton m_overview;

	public UIContentNavButton m_stats;

	public UIContentNavButton m_badges;

	public UIContentNavButton m_achievements;

	public UIContentNavButton m_history;

	[Header("Panels")]
	public UIPlayerProgressBanners m_bannersPanel;

	public UIPlayerProgressOverview m_overviewPanel;

	public UIPlayerProgressStats m_statsPanel;

	public UIPlayerProgressBadges m_badgesPanel;

	public UIPlayerProgressAchievements m_achievementsPanel;

	public UIPlayerProgressHistory m_historyPanel;

	[Header("Drop-downs")]
	public UIPlayerProgressDropdownList m_freelancerDropdown;

	public UIPlayerProgressDropdownList m_gameModeDropdown;

	public UIPlayerProgressDropdownList m_seasonsDropdown;

	public UIPlayerProgressDropdownList m_achievementDropdown;

	private List<UIContentNavButton> m_menuButtons;

	private List<UIPlayerProgressSubPanel> m_subPanels;

	private CanvasGroup m_canvasGroup;

	private PersistedAccountData m_playerAccountData;

	private List<PersistedCharacterMatchData> m_matchHistory;

	private List<PersistedCharacterData> m_charactersList;

	private bool m_needToUpdateInfo;

	private int m_InfoUpdated;

	private bool m_isVisible;

	private bool m_isInTransition;

	private const int NUM_INFO_REQUESTS = 1;

	private Action<string> OnPlayerTitleChange;

	private static UIPlayerProgressPanel m_instance;

	private int m_originalSelectedTitleID
	{
		get;
		set;
	}

	private int m_originalSelectedForegroundBannerID
	{
		get;
		set;
	}

	private int m_originalSelectedBackgroundBannerID
	{
		get;
		set;
	}

	private int m_originalSelectedRibbonID
	{
		get;
		set;
	}

	public static UIPlayerProgressPanel Get()
	{
		return m_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.PlayerProgress;
	}

	public override void Awake()
	{
		m_instance = this;
		m_isVisible = base.gameObject.activeSelf;
		m_canvasGroup = GetComponent<CanvasGroup>();
		m_menuButtons = new List<UIContentNavButton>();
		m_menuButtons.Add(m_overview);
		m_menuButtons.Add(m_banner);
		m_menuButtons.Add(m_history);
		m_menuButtons.Add(m_stats);
		m_menuButtons.Add(m_badges);
		m_menuButtons.Add(m_achievements);
		for (int i = 0; i < m_menuButtons.Count; i++)
		{
			m_menuButtons[i].RegisterClickCallback(NotifyMenuButtonClicked);
		}
		while (true)
		{
			m_subPanels = new List<UIPlayerProgressSubPanel>();
			m_subPanels.Add(m_overviewPanel);
			m_subPanels.Add(m_bannersPanel);
			m_subPanels.Add(m_historyPanel);
			m_subPanels.Add(m_statsPanel);
			m_subPanels.Add(m_badgesPanel);
			m_subPanels.Add(m_achievementsPanel);
			UIManager.SetGameObjectActive(m_overviewPanel, false);
			UIManager.SetGameObjectActive(m_bannersPanel, false);
			UIManager.SetGameObjectActive(m_historyPanel, false);
			UIManager.SetGameObjectActive(m_statsPanel, false);
			UIManager.SetGameObjectActive(m_badgesPanel, false);
			UIManager.SetGameObjectActive(m_achievementsPanel, false);
			UIManager.SetGameObjectActive(m_freelancerDropdown, false);
			UIManager.SetGameObjectActive(m_gameModeDropdown, false);
			UIManager.SetGameObjectActive(m_seasonsDropdown, false);
			UIManager.SetGameObjectActive(m_achievementDropdown, false);
			UIManager.SetGameObjectActive(m_overviewPanel.m_freelancerComparisonDropdown, false);
			UIManager.SetGameObjectActive(m_overviewPanel.m_seasonBucketDropdown, false);
			m_closeBtn.callback = CloseBtnClicked;
			UIManager.SetGameObjectActive(m_animationController, false);
			m_isInTransition = false;
			UIManager.SetGameObjectActive(m_freelancerDropdown, false);
			OnPlayerTitleChange = delegate(string newTitle)
			{
				m_playerTitle.text = newTitle;
				UIManager.SetGameObjectActive(m_playerTitle, true);
			};
			ClientGameManager.Get().OnPlayerTitleChange += OnPlayerTitleChange;
			ClientGameManager.Get().OnAccountDataUpdated += OnAccountDataUpdated;
			ClientGameManager.Get().OnServerQueueConfigurationUpdateNotification += OnServerQueueConfigurationUpdateNotification;
			if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
				OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
			}
			OnServerQueueConfigurationUpdateNotification(null);
			base.Awake();
			return;
		}
	}

	private void OnDisable()
	{
		m_isInTransition = false;
	}

	private void OnDestroy()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager == null)
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
		clientGameManager.OnPlayerTitleChange -= OnPlayerTitleChange;
		clientGameManager.OnAccountDataUpdated -= OnAccountDataUpdated;
		clientGameManager.OnServerQueueConfigurationUpdateNotification -= OnServerQueueConfigurationUpdateNotification;
	}

	public void CloseProgressPanel(BaseEventData data)
	{
		SetVisible(false);
	}

	public bool IsVisible()
	{
		return m_isVisible;
	}

	public void SetVisible(bool visible, bool needToUpdateInfo = true)
	{
		if (m_isVisible == visible)
		{
			return;
		}
		while (true)
		{
			if (UILootMatrixScreen.Get() != null && UILootMatrixScreen.Get().IsOpening())
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
			if (m_isInTransition)
			{
				if (visible)
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
			HideDropdowns();
			UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
			{
				CustomGamePartyListHidden = visible
			});
			m_isVisible = visible;
			m_canvasGroup.blocksRaycasts = visible;
			if (visible)
			{
				UIFrontEnd.Get().m_frontEndNavPanel.NotifyCurrentPanelLoseFocus();
			}
			else
			{
				UICharacterStoreAndProgressWorldObjects.Get().SetVisible(false);
				UIFrontEnd.Get().m_frontEndNavPanel.NotifyCurrentPanelGetFocus();
			}
			UIManager.SetGameObjectActive(m_playerProgressClickBlocker, visible);
			if (m_isInTransition && !visible)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						UICharacterStoreAndProgressWorldObjects.Get().SetVisible(false);
						UIManager.SetGameObjectActive(base.gameObject, false);
						m_isInTransition = false;
						return;
					}
				}
			}
			if (visible)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						if (UIGameSettingsPanel.Get().m_lastVisible)
						{
							UIGameSettingsPanel.Get().CancelClicked(null);
						}
						m_isInTransition = true;
						m_animationController.Play("UI_ProfileDefaultIN");
						m_playerAccountData = null;
						m_needToUpdateInfo = needToUpdateInfo;
						m_InfoUpdated = 0;
						m_playerName.text = string.Empty;
						m_playerTitle.text = string.Empty;
						m_matchesPlayedText.text = string.Empty;
						m_freelancerLevelsText.text = string.Empty;
						m_reactorLevelText.text = string.Empty;
						m_ggBoostsUsedText.text = string.Empty;
						m_totalWinsText.text = string.Empty;
						UIManager.SetGameObjectActive(m_overviewPanel, false);
						UIManager.SetGameObjectActive(m_bannersPanel, false);
						UIManager.SetGameObjectActive(m_historyPanel, false);
						UIManager.SetGameObjectActive(m_statsPanel, false);
						m_overview.SetSelected(true);
						m_banner.SetSelected(false);
						m_history.SetSelected(false);
						m_stats.SetSelected(false);
						m_originalSelectedTitleID = -1;
						m_originalSelectedForegroundBannerID = -1;
						m_originalSelectedBackgroundBannerID = -1;
						m_originalSelectedRibbonID = -1;
						m_charactersList = ClientGameManager.Get().GetAllPlayerCharacterData().Values.ToList();
						OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
						if (m_matchHistory == null)
						{
							while (true)
							{
								switch (3)
								{
								case 0:
									break;
								default:
									ClientGameManager.Get().QueryPlayerMatchData(delegate(PlayerMatchDataResponse response)
									{
										if (response.Success)
										{
											m_InfoUpdated++;
											m_matchHistory = response.MatchData;
											m_historyPanel.SetMatchHistory(response.MatchData);
											m_overviewPanel.Setup(m_playerAccountData, m_charactersList);
											if (m_isVisible)
											{
												UIManager.SetGameObjectActive(base.gameObject, true);
											}
										}
										UIRAFProgramScreen.Get().SetVisible(false);
										UIGGBoostPurchaseScreen.Get().SetVisible(false);
									});
									return;
								}
							}
						}
						UIManager.SetGameObjectActive(base.gameObject, true);
						UIRAFProgramScreen.Get().SetVisible(false);
						UIGGBoostPurchaseScreen.Get().SetVisible(false);
						return;
					}
				}
			}
			LogPlayerChanges();
			m_isInTransition = m_animationController.isActiveAndEnabled;
			m_animationController.Play("UI_ProfileDefaultOUT");
			return;
		}
	}

	public void MarkTransitionFinished()
	{
		m_isInTransition = false;
	}

	public void Update()
	{
		if (m_needToUpdateInfo && m_InfoUpdated >= 1)
		{
			NotifyMenuButtonClicked(m_overview);
			m_needToUpdateInfo = false;
			m_InfoUpdated = 0;
		}
		if (!Input.GetKeyDown(KeyCode.Escape))
		{
			return;
		}
		while (true)
		{
			if (!IsVisible())
			{
				return;
			}
			while (true)
			{
				if (UIStorePanel.Get().IsWaitingForPurchaseRequest || UIFrontEnd.Get().m_frontEndChatConsole.EscapeJustPressed())
				{
					return;
				}
				while (true)
				{
					if (!(EventSystem.current.currentSelectedGameObject == null))
					{
						if (!(EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() == null))
						{
							return;
						}
					}
					if (UIStorePanel.Get().IsPurchaseDialogOpen())
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								break;
							default:
								UIStorePanel.Get().ClosePurchaseDialog();
								return;
							}
						}
					}
					UIFrontEnd.Get().TogglePlayerProgressScreenVisibility();
					return;
				}
			}
		}
	}

	public void LogPlayerChanges()
	{
		if (m_playerAccountData == null)
		{
			return;
		}
		while (true)
		{
			if (m_playerAccountData.AccountComponent.SelectedTitleID == m_originalSelectedTitleID)
			{
				if (m_playerAccountData.AccountComponent.SelectedRibbonID == m_originalSelectedRibbonID)
				{
					if (m_playerAccountData.AccountComponent.SelectedForegroundBannerID == m_originalSelectedForegroundBannerID)
					{
						if (m_playerAccountData.AccountComponent.SelectedBackgroundBannerID == m_originalSelectedBackgroundBannerID)
						{
							return;
						}
					}
				}
			}
			ClientGameManager.Get().PlayerPanelUpdated(m_originalSelectedTitleID, m_originalSelectedForegroundBannerID, m_originalSelectedBackgroundBannerID, m_originalSelectedRibbonID);
			return;
		}
	}

	private void OnAccountDataUpdated(PersistedAccountData newData)
	{
		m_playerAccountData = newData;
		m_playerName.text = HydrogenConfig.Get().Ticket.GetFormattedHandle(Mathf.FloorToInt(m_playerName.fontSize * 0.7f));
		m_InfoUpdated++;
#if !VANILLA && !SERVER
        // Custom titles
        m_playerTitle.text = GameBalanceVars.Get().GetTitle(newData.AccountComponent.SelectedTitleID, newData.Handle, string.Empty);
#else
		m_playerTitle.text = GameBalanceVars.Get().GetTitle(newData.AccountComponent.SelectedTitleID, string.Empty);
#endif
		UIManager.SetGameObjectActive(m_playerTitle, true);
		m_overviewPanel.Setup(m_playerAccountData, m_charactersList);
		m_achievementsPanel.Setup();
		int num = 0;
		using (Dictionary<CharacterType, PersistedCharacterData>.Enumerator enumerator = ClientGameManager.Get().GetAllPlayerCharacterData().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<CharacterType, PersistedCharacterData> current = enumerator.Current;
				if (current.Key.IsValidForHumanGameplay())
				{
					num += current.Value.ExperienceComponent.Level;
				}
			}
		}
		int num2 = 0;
		List<QuestTemplate> quests = QuestWideData.Get().m_quests;
		for (int i = 0; i < quests.Count; i++)
		{
			QuestTemplate questTemplate = quests[i];
			if (questTemplate.AchievmentType == AchievementType.None)
			{
				continue;
			}
			if (questTemplate.Enabled && newData.QuestComponent.GetOrCreateQuestMetaData(questTemplate.Index).CompletedCount > 0)
			{
				num2 += questTemplate.AchievementPoints;
			}
		}
		while (true)
		{
			m_matchesPlayedText.text = UIStorePanel.FormatIntToString(newData.ExperienceComponent.Matches, true);
			m_freelancerLevelsText.text = UIStorePanel.FormatIntToString(num, true);
			m_reactorLevelText.text = UIStorePanel.FormatIntToString(newData.GetReactorLevel(SeasonWideData.Get().m_seasons), true);
			m_achievementPointsText.text = UIStorePanel.FormatIntToString(num2, true);
			int totalSpent = newData.BankComponent.CurrentAmounts.GetValue(CurrencyType.GGPack).m_TotalSpent;
			m_ggBoostsUsedText.text = UIStorePanel.FormatIntToString(totalSpent, true);
			int wins = newData.ExperienceComponent.Wins;
			m_totalWinsText.text = UIStorePanel.FormatIntToString(wins, true);
			m_originalSelectedTitleID = newData.AccountComponent.SelectedTitleID;
			m_originalSelectedForegroundBannerID = newData.AccountComponent.SelectedForegroundBannerID;
			m_originalSelectedBackgroundBannerID = newData.AccountComponent.SelectedBackgroundBannerID;
			m_originalSelectedRibbonID = newData.AccountComponent.SelectedRibbonID;
			return;
		}
	}

	private void OnServerQueueConfigurationUpdateNotification(ServerQueueConfigurationUpdateNotification notification)
	{
		m_badges.m_hitbox.selectableButton.SetDisabled(!ClientGameManager.Get().AllowBadges);
	}

	public void CloseBtnClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.Close);
		SetVisible(false);
	}

	public void ClickedOnPage(UIPageIndicator pageIndicator)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
		for (int i = 0; i < m_subPanels.Count; i++)
		{
			if (m_subPanels[i].IsActive)
			{
				m_subPanels[i].ClickedOnPageIndicator(pageIndicator);
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

	public void NotifyMenuButtonClicked(UIContentNavButton clickedButton)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		for (int i = 0; i < m_menuButtons.Count; i++)
		{
			UIContentNavButton uIContentNavButton = m_menuButtons[i];
			bool flag = uIContentNavButton == clickedButton;
			uIContentNavButton.SetSelected(flag);
			if (i < m_subPanels.Count)
			{
				UIManager.SetGameObjectActive(m_subPanels[i], flag);
			}
		}
		while (true)
		{
			UICharacterStoreAndProgressWorldObjects.Get().SetVisible(false);
			return;
		}
	}

	private void SetupDropdown(UIPlayerProgressDropdownList dropdown, int selectedValue, Action<int> callback, Transform parentSlot)
	{
		dropdown.SetSelectCallback(callback);
		dropdown.Toggle();
		dropdown.transform.SetParent(parentSlot);
		dropdown.transform.localScale = Vector3.one;
		dropdown.transform.localPosition = Vector3.zero;
		dropdown.HighlightCurrentOption(selectedValue);
	}

	public void OpenFreelancerDropdown(CharacterType selectedFreelancer, Action<int> callback, Transform parentSlot, bool withRoles, CharacterRole role = CharacterRole.None)
	{
		if (m_freelancerDropdown.Initialize())
		{
			m_freelancerDropdown.AddOption(0, StringUtil.TR("AllFreelancers", "Global"));
			IEnumerator enumerator = Enum.GetValues(typeof(CharacterRole)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					CharacterRole characterRole = (CharacterRole)enumerator.Current;
					if (characterRole == CharacterRole.None)
					{
					}
					else
					{
						m_freelancerDropdown.AddOption(0 - characterRole, StringUtil.TR("CharacterRole_" + characterRole, "Global"), characterRole);
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							disposable.Dispose();
							goto end_IL_00bd;
						}
					}
				}
				end_IL_00bd:;
			}
			bool flag = GameManager.Get() != null && GameManager.Get().GameplayOverrides.EnableHiddenCharacters;
			List<CharacterResourceLink> list = new List<CharacterResourceLink>();
			CharacterResourceLink[] characterResourceLinks = GameWideData.Get().m_characterResourceLinks;
			foreach (CharacterResourceLink characterResourceLink in characterResourceLinks)
			{
				if (!flag)
				{
					if (characterResourceLink.m_isHidden)
					{
						continue;
					}
				}
				if (!characterResourceLink.m_characterType.IsValidForHumanGameplay())
				{
				}
				else
				{
					CharacterConfig characterConfig = GameManager.Get().GameplayOverrides.GetCharacterConfig(characterResourceLink.m_characterType);
					if (characterConfig.AllowForPlayers && !characterConfig.IsHidden)
					{
						list.Add(characterResourceLink);
					}
				}
			}
			
			list.Sort(((CharacterResourceLink x, CharacterResourceLink y) => x.GetDisplayName().CompareTo(y.GetDisplayName())));
			using (List<CharacterResourceLink>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CharacterResourceLink current = enumerator2.Current;
					if (current.m_characterType.IsValidForHumanGameplay())
					{
						m_freelancerDropdown.AddOption((int)current.m_characterType, current.GetDisplayName(), current.m_characterType);
					}
				}
			}
			m_freelancerDropdown.AddHitbox(m_statsPanel.m_freelancerDropdownBtn.m_button.spriteController.gameObject);
			m_freelancerDropdown.AddHitbox(m_achievementsPanel.m_freelancerDropdownBtn.m_button.spriteController.gameObject);
			m_freelancerDropdown.AddHitbox(m_badgesPanel.m_freelancerDropdownBtn.m_button.spriteController.gameObject);
		}
		int num = 0;
		if (withRoles)
		{
			UIPlayerProgressDropdownList freelancerDropdown = m_freelancerDropdown;
			
			freelancerDropdown.CheckOptionDisplayState(((int x) => true));
			num = ((role == CharacterRole.None) ? ((int)selectedFreelancer) : (0 - role));
		}
		else
		{
			UIPlayerProgressDropdownList freelancerDropdown2 = m_freelancerDropdown;
			
			freelancerDropdown2.CheckOptionDisplayState(((int x) => x >= 0));
			num = (int)selectedFreelancer;
		}
		SetupDropdown(m_freelancerDropdown, num, callback, parentSlot);
	}

	public void OpenGameModeDropdown(PersistedStatBucket selectedBucket, Action<int> callback, Transform parentSlot)
	{
		if (m_gameModeDropdown.Initialize())
		{
			IEnumerator enumerator = Enum.GetValues(typeof(PersistedStatBucket)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					PersistedStatBucket persistedStatBucket = (PersistedStatBucket)enumerator.Current;
					if (persistedStatBucket.IsTracked())
					{
						m_gameModeDropdown.AddOption((int)persistedStatBucket, StringUtil.TR_PersistedStatBucketName(persistedStatBucket));
					}
				}
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
				{
					disposable.Dispose();
				}
			}
			m_gameModeDropdown.AddHitbox(m_statsPanel.m_gameModeDropdownBtn.m_button.spriteController.gameObject);
			m_gameModeDropdown.AddHitbox(m_badgesPanel.m_gameModeDropdownBtn.m_button.spriteController.gameObject);
		}
		SetupDropdown(m_gameModeDropdown, (int)selectedBucket, callback, parentSlot);
	}

	public void OpenSeasonsDropdown(int selectedSeason, Action<int> callback, UIPlayerProgressDropdownBtn.ShouldShow shouldShow, Transform parentSlot)
	{
		if (m_seasonsDropdown.Initialize())
		{
			int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
			List<SeasonTemplate> list = new List<SeasonTemplate>();
			for (int i = 0; i < SeasonWideData.Get().m_seasons.Count; i++)
			{
				if (SeasonWideData.Get().m_seasons[i].DisplayStats)
				{
					list.Add(SeasonWideData.Get().m_seasons[i]);
				}
			}
			list.Reverse();
			using (List<SeasonTemplate>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SeasonTemplate current = enumerator.Current;
					string text;
					if (current.Index == activeSeason)
					{
						text = StringUtil.TR("CurrentSeason", "Global");
					}
					else
					{
						text = current.GetDisplayName();
					}
					m_seasonsDropdown.AddOption(current.Index, text);
				}
			}
			m_seasonsDropdown.AddHitbox(m_statsPanel.m_seasonsDropdownBtn.m_button.spriteController.gameObject);
			m_seasonsDropdown.AddHitbox(m_badgesPanel.m_seasonsDropdownBtn.m_button.spriteController.gameObject);
		}
		m_seasonsDropdown.CheckOptionDisplayState(shouldShow);
		SetupDropdown(m_seasonsDropdown, selectedSeason, callback, parentSlot);
	}

	public void OpenAchievementDropdown(AchievementType selected, Action<int> callback, Transform parentSlot)
	{
		if (m_achievementDropdown.Initialize())
		{
			List<AchievementType> list = new List<AchievementType>();
			list.Add(AchievementType.None);
			for (int i = 0; i < QuestWideData.Get().m_quests.Count; i++)
			{
				AchievementType achievmentType = QuestWideData.Get().m_quests[i].AchievmentType;
				if (achievmentType != 0)
				{
					if (!list.Contains(achievmentType))
					{
						list.Add(achievmentType);
					}
				}
			}
			list.Sort();
			using (List<AchievementType>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AchievementType current = enumerator.Current;
					m_achievementDropdown.AddOption((int)current, StringUtil.TR("AchievementCategory_" + current, "Global"));
				}
			}
			m_achievementDropdown.AddHitbox(m_achievementsPanel.m_categoryDropdownBtn.m_button.spriteController.gameObject);
		}
		SetupDropdown(m_achievementDropdown, (int)selected, callback, parentSlot);
	}

	public void HideDropdowns()
	{
		m_freelancerDropdown.SetVisible(false);
		m_gameModeDropdown.SetVisible(false);
		m_seasonsDropdown.SetVisible(false);
		m_achievementDropdown.SetVisible(false);
	}
}
