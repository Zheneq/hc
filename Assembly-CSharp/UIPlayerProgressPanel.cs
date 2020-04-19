using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using LobbyGameClientMessages;
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

	private int m_originalSelectedTitleID { get; set; }

	private int m_originalSelectedForegroundBannerID { get; set; }

	private int m_originalSelectedBackgroundBannerID { get; set; }

	private int m_originalSelectedRibbonID { get; set; }

	public static UIPlayerProgressPanel Get()
	{
		return UIPlayerProgressPanel.m_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.PlayerProgress;
	}

	public override void Awake()
	{
		UIPlayerProgressPanel.m_instance = this;
		this.m_isVisible = base.gameObject.activeSelf;
		this.m_canvasGroup = base.GetComponent<CanvasGroup>();
		this.m_menuButtons = new List<UIContentNavButton>();
		this.m_menuButtons.Add(this.m_overview);
		this.m_menuButtons.Add(this.m_banner);
		this.m_menuButtons.Add(this.m_history);
		this.m_menuButtons.Add(this.m_stats);
		this.m_menuButtons.Add(this.m_badges);
		this.m_menuButtons.Add(this.m_achievements);
		for (int i = 0; i < this.m_menuButtons.Count; i++)
		{
			this.m_menuButtons[i].RegisterClickCallback(new Action<UIContentNavButton>(this.NotifyMenuButtonClicked));
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressPanel.Awake()).MethodHandle;
		}
		this.m_subPanels = new List<UIPlayerProgressSubPanel>();
		this.m_subPanels.Add(this.m_overviewPanel);
		this.m_subPanels.Add(this.m_bannersPanel);
		this.m_subPanels.Add(this.m_historyPanel);
		this.m_subPanels.Add(this.m_statsPanel);
		this.m_subPanels.Add(this.m_badgesPanel);
		this.m_subPanels.Add(this.m_achievementsPanel);
		UIManager.SetGameObjectActive(this.m_overviewPanel, false, null);
		UIManager.SetGameObjectActive(this.m_bannersPanel, false, null);
		UIManager.SetGameObjectActive(this.m_historyPanel, false, null);
		UIManager.SetGameObjectActive(this.m_statsPanel, false, null);
		UIManager.SetGameObjectActive(this.m_badgesPanel, false, null);
		UIManager.SetGameObjectActive(this.m_achievementsPanel, false, null);
		UIManager.SetGameObjectActive(this.m_freelancerDropdown, false, null);
		UIManager.SetGameObjectActive(this.m_gameModeDropdown, false, null);
		UIManager.SetGameObjectActive(this.m_seasonsDropdown, false, null);
		UIManager.SetGameObjectActive(this.m_achievementDropdown, false, null);
		UIManager.SetGameObjectActive(this.m_overviewPanel.m_freelancerComparisonDropdown, false, null);
		UIManager.SetGameObjectActive(this.m_overviewPanel.m_seasonBucketDropdown, false, null);
		this.m_closeBtn.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CloseBtnClicked);
		UIManager.SetGameObjectActive(this.m_animationController, false, null);
		this.m_isInTransition = false;
		UIManager.SetGameObjectActive(this.m_freelancerDropdown, false, null);
		this.OnPlayerTitleChange = delegate(string newTitle)
		{
			this.m_playerTitle.text = newTitle;
			UIManager.SetGameObjectActive(this.m_playerTitle, true, null);
		};
		ClientGameManager.Get().OnPlayerTitleChange += this.OnPlayerTitleChange;
		ClientGameManager.Get().OnAccountDataUpdated += this.OnAccountDataUpdated;
		ClientGameManager.Get().OnServerQueueConfigurationUpdateNotification += this.OnServerQueueConfigurationUpdateNotification;
		if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
			this.OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
		}
		this.OnServerQueueConfigurationUpdateNotification(null);
		base.Awake();
	}

	private void OnDisable()
	{
		this.m_isInTransition = false;
	}

	private void OnDestroy()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressPanel.OnDestroy()).MethodHandle;
			}
			return;
		}
		clientGameManager.OnPlayerTitleChange -= this.OnPlayerTitleChange;
		clientGameManager.OnAccountDataUpdated -= this.OnAccountDataUpdated;
		clientGameManager.OnServerQueueConfigurationUpdateNotification -= this.OnServerQueueConfigurationUpdateNotification;
	}

	public void CloseProgressPanel(BaseEventData data)
	{
		this.SetVisible(false, true);
	}

	public bool IsVisible()
	{
		return this.m_isVisible;
	}

	public void SetVisible(bool visible, bool needToUpdateInfo = true)
	{
		if (this.m_isVisible != visible)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressPanel.SetVisible(bool, bool)).MethodHandle;
			}
			if (UILootMatrixScreen.Get() != null && UILootMatrixScreen.Get().IsOpening())
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
			}
			else
			{
				if (this.m_isInTransition)
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
					if (visible)
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
				this.HideDropdowns();
				UIManager.Get().HandleNewSceneStateParameter(new UICharacterScreen.CharacterSelectSceneStateParameters
				{
					CustomGamePartyListHidden = new bool?(visible)
				});
				this.m_isVisible = visible;
				this.m_canvasGroup.blocksRaycasts = visible;
				if (visible)
				{
					UIFrontEnd.Get().m_frontEndNavPanel.NotifyCurrentPanelLoseFocus();
				}
				else
				{
					UICharacterStoreAndProgressWorldObjects.Get().SetVisible(false);
					UIFrontEnd.Get().m_frontEndNavPanel.NotifyCurrentPanelGetFocus();
				}
				UIManager.SetGameObjectActive(this.m_playerProgressClickBlocker, visible, null);
				if (this.m_isInTransition && !visible)
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
					UICharacterStoreAndProgressWorldObjects.Get().SetVisible(false);
					UIManager.SetGameObjectActive(base.gameObject, false, null);
					this.m_isInTransition = false;
					return;
				}
				if (visible)
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
					if (UIGameSettingsPanel.Get().m_lastVisible)
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
						UIGameSettingsPanel.Get().CancelClicked(null);
					}
					this.m_isInTransition = true;
					this.m_animationController.Play("UI_ProfileDefaultIN");
					this.m_playerAccountData = null;
					this.m_needToUpdateInfo = needToUpdateInfo;
					this.m_InfoUpdated = 0;
					this.m_playerName.text = string.Empty;
					this.m_playerTitle.text = string.Empty;
					this.m_matchesPlayedText.text = string.Empty;
					this.m_freelancerLevelsText.text = string.Empty;
					this.m_reactorLevelText.text = string.Empty;
					this.m_ggBoostsUsedText.text = string.Empty;
					this.m_totalWinsText.text = string.Empty;
					UIManager.SetGameObjectActive(this.m_overviewPanel, false, null);
					UIManager.SetGameObjectActive(this.m_bannersPanel, false, null);
					UIManager.SetGameObjectActive(this.m_historyPanel, false, null);
					UIManager.SetGameObjectActive(this.m_statsPanel, false, null);
					this.m_overview.SetSelected(true);
					this.m_banner.SetSelected(false);
					this.m_history.SetSelected(false);
					this.m_stats.SetSelected(false);
					this.m_originalSelectedTitleID = -1;
					this.m_originalSelectedForegroundBannerID = -1;
					this.m_originalSelectedBackgroundBannerID = -1;
					this.m_originalSelectedRibbonID = -1;
					this.m_charactersList = ClientGameManager.Get().GetAllPlayerCharacterData().Values.ToList<PersistedCharacterData>();
					this.OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
					if (this.m_matchHistory == null)
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
						ClientGameManager.Get().QueryPlayerMatchData(delegate(PlayerMatchDataResponse response)
						{
							if (response.Success)
							{
								this.m_InfoUpdated++;
								this.m_matchHistory = response.MatchData;
								this.m_historyPanel.SetMatchHistory(response.MatchData);
								this.m_overviewPanel.Setup(this.m_playerAccountData, this.m_charactersList);
								if (this.m_isVisible)
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
										RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIPlayerProgressPanel.<SetVisible>m__1(PlayerMatchDataResponse)).MethodHandle;
									}
									UIManager.SetGameObjectActive(base.gameObject, true, null);
								}
							}
							UIRAFProgramScreen.Get().SetVisible(false);
							UIGGBoostPurchaseScreen.Get().SetVisible(false);
						});
					}
					else
					{
						UIManager.SetGameObjectActive(base.gameObject, true, null);
						UIRAFProgramScreen.Get().SetVisible(false);
						UIGGBoostPurchaseScreen.Get().SetVisible(false);
					}
				}
				else
				{
					this.LogPlayerChanges();
					this.m_isInTransition = this.m_animationController.isActiveAndEnabled;
					this.m_animationController.Play("UI_ProfileDefaultOUT");
				}
				return;
			}
		}
	}

	public void MarkTransitionFinished()
	{
		this.m_isInTransition = false;
	}

	public void Update()
	{
		if (this.m_needToUpdateInfo && this.m_InfoUpdated >= 1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressPanel.Update()).MethodHandle;
			}
			this.NotifyMenuButtonClicked(this.m_overview);
			this.m_needToUpdateInfo = false;
			this.m_InfoUpdated = 0;
		}
		if (Input.GetKeyDown(KeyCode.Escape))
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
			if (this.IsVisible())
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
				if (!UIStorePanel.Get().IsWaitingForPurchaseRequest && !UIFrontEnd.Get().m_frontEndChatConsole.EscapeJustPressed())
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
					if (!(EventSystem.current.currentSelectedGameObject == null))
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
						if (!(EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() == null))
						{
							return;
						}
					}
					if (UIStorePanel.Get().IsPurchaseDialogOpen())
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
						UIStorePanel.Get().ClosePurchaseDialog();
					}
					else
					{
						UIFrontEnd.Get().TogglePlayerProgressScreenVisibility(true);
					}
				}
			}
		}
	}

	public void LogPlayerChanges()
	{
		if (this.m_playerAccountData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressPanel.LogPlayerChanges()).MethodHandle;
			}
			if (this.m_playerAccountData.AccountComponent.SelectedTitleID == this.m_originalSelectedTitleID)
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
				if (this.m_playerAccountData.AccountComponent.SelectedRibbonID == this.m_originalSelectedRibbonID)
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
					if (this.m_playerAccountData.AccountComponent.SelectedForegroundBannerID == this.m_originalSelectedForegroundBannerID)
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
						if (this.m_playerAccountData.AccountComponent.SelectedBackgroundBannerID == this.m_originalSelectedBackgroundBannerID)
						{
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
			}
			ClientGameManager.Get().PlayerPanelUpdated(this.m_originalSelectedTitleID, this.m_originalSelectedForegroundBannerID, this.m_originalSelectedBackgroundBannerID, this.m_originalSelectedRibbonID);
		}
	}

	private void OnAccountDataUpdated(PersistedAccountData newData)
	{
		this.m_playerAccountData = newData;
		this.m_playerName.text = HydrogenConfig.Get().Ticket.GetFormattedHandle(Mathf.FloorToInt(this.m_playerName.fontSize * 0.7f));
		this.m_InfoUpdated++;
		this.m_playerTitle.text = GameBalanceVars.Get().GetTitle(newData.AccountComponent.SelectedTitleID, string.Empty, -1);
		UIManager.SetGameObjectActive(this.m_playerTitle, true, null);
		this.m_overviewPanel.Setup(this.m_playerAccountData, this.m_charactersList);
		this.m_achievementsPanel.Setup();
		int num = 0;
		using (Dictionary<CharacterType, PersistedCharacterData>.Enumerator enumerator = ClientGameManager.Get().GetAllPlayerCharacterData().GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				KeyValuePair<CharacterType, PersistedCharacterData> keyValuePair = enumerator.Current;
				if (keyValuePair.Key.IsValidForHumanGameplay())
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressPanel.OnAccountDataUpdated(PersistedAccountData)).MethodHandle;
					}
					num += keyValuePair.Value.ExperienceComponent.Level;
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
		int num2 = 0;
		List<QuestTemplate> quests = QuestWideData.Get().m_quests;
		int i = 0;
		while (i < quests.Count)
		{
			QuestTemplate questTemplate = quests[i];
			if (questTemplate.AchievmentType != AchievementType.None)
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
				if (questTemplate.Enabled)
				{
					if (newData.QuestComponent.GetOrCreateQuestMetaData(questTemplate.Index).CompletedCount > 0)
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
						num2 += questTemplate.AchievementPoints;
					}
				}
			}
			IL_198:
			i++;
			continue;
			goto IL_198;
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
		this.m_matchesPlayedText.text = UIStorePanel.FormatIntToString(newData.ExperienceComponent.Matches, true);
		this.m_freelancerLevelsText.text = UIStorePanel.FormatIntToString(num, true);
		this.m_reactorLevelText.text = UIStorePanel.FormatIntToString(newData.GetReactorLevel(SeasonWideData.Get().m_seasons), true);
		this.m_achievementPointsText.text = UIStorePanel.FormatIntToString(num2, true);
		int totalSpent = newData.BankComponent.CurrentAmounts.GetValue(CurrencyType.GGPack).m_TotalSpent;
		this.m_ggBoostsUsedText.text = UIStorePanel.FormatIntToString(totalSpent, true);
		int wins = newData.ExperienceComponent.Wins;
		this.m_totalWinsText.text = UIStorePanel.FormatIntToString(wins, true);
		this.m_originalSelectedTitleID = newData.AccountComponent.SelectedTitleID;
		this.m_originalSelectedForegroundBannerID = newData.AccountComponent.SelectedForegroundBannerID;
		this.m_originalSelectedBackgroundBannerID = newData.AccountComponent.SelectedBackgroundBannerID;
		this.m_originalSelectedRibbonID = newData.AccountComponent.SelectedRibbonID;
	}

	private void OnServerQueueConfigurationUpdateNotification(ServerQueueConfigurationUpdateNotification notification)
	{
		this.m_badges.m_hitbox.selectableButton.SetDisabled(!ClientGameManager.Get().AllowBadges);
	}

	public void CloseBtnClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.Close);
		this.SetVisible(false, true);
	}

	public void ClickedOnPage(UIPageIndicator pageIndicator)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.CharacterSelectModAdd);
		for (int i = 0; i < this.m_subPanels.Count; i++)
		{
			if (this.m_subPanels[i].IsActive)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressPanel.ClickedOnPage(UIPageIndicator)).MethodHandle;
				}
				this.m_subPanels[i].ClickedOnPageIndicator(pageIndicator);
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

	public void NotifyMenuButtonClicked(UIContentNavButton clickedButton)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.PlayCategorySelect);
		for (int i = 0; i < this.m_menuButtons.Count; i++)
		{
			UIContentNavButton uicontentNavButton = this.m_menuButtons[i];
			bool flag = uicontentNavButton == clickedButton;
			uicontentNavButton.SetSelected(flag);
			if (i < this.m_subPanels.Count)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressPanel.NotifyMenuButtonClicked(UIContentNavButton)).MethodHandle;
				}
				UIManager.SetGameObjectActive(this.m_subPanels[i], flag, null);
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
		UICharacterStoreAndProgressWorldObjects.Get().SetVisible(false);
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
		if (this.m_freelancerDropdown.Initialize())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressPanel.OpenFreelancerDropdown(CharacterType, Action<int>, Transform, bool, CharacterRole)).MethodHandle;
			}
			this.m_freelancerDropdown.AddOption(0, StringUtil.TR("AllFreelancers", "Global"), CharacterType.None);
			IEnumerator enumerator = Enum.GetValues(typeof(CharacterRole)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					CharacterRole characterRole = (CharacterRole)obj;
					if (characterRole == CharacterRole.None)
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
					}
					else
					{
						this.m_freelancerDropdown.AddOption((int)(-(int)characterRole), StringUtil.TR("CharacterRole_" + characterRole, "Global"), characterRole);
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
			}
			finally
			{
				IDisposable disposable;
				if ((disposable = (enumerator as IDisposable)) != null)
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
					disposable.Dispose();
				}
			}
			bool flag = GameManager.Get() != null && GameManager.Get().GameplayOverrides.EnableHiddenCharacters;
			List<CharacterResourceLink> list = new List<CharacterResourceLink>();
			CharacterResourceLink[] characterResourceLinks = GameWideData.Get().m_characterResourceLinks;
			int i = 0;
			while (i < characterResourceLinks.Length)
			{
				CharacterResourceLink characterResourceLink = characterResourceLinks[i];
				if (flag)
				{
					goto IL_136;
				}
				if (!characterResourceLink.m_isHidden)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						goto IL_136;
					}
				}
				IL_18B:
				i++;
				continue;
				IL_150:
				goto IL_18B;
				IL_136:
				if (!characterResourceLink.m_characterType.IsValidForHumanGameplay())
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						goto IL_150;
					}
				}
				else
				{
					CharacterConfig characterConfig = GameManager.Get().GameplayOverrides.GetCharacterConfig(characterResourceLink.m_characterType);
					if (!characterConfig.AllowForPlayers || characterConfig.IsHidden)
					{
						goto IL_18B;
					}
					list.Add(characterResourceLink);
					goto IL_18B;
				}
			}
			List<CharacterResourceLink> list2 = list;
			if (UIPlayerProgressPanel.<>f__am$cache0 == null)
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
				UIPlayerProgressPanel.<>f__am$cache0 = ((CharacterResourceLink x, CharacterResourceLink y) => x.GetDisplayName().CompareTo(y.GetDisplayName()));
			}
			list2.Sort(UIPlayerProgressPanel.<>f__am$cache0);
			using (List<CharacterResourceLink>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					CharacterResourceLink characterResourceLink2 = enumerator2.Current;
					if (characterResourceLink2.m_characterType.IsValidForHumanGameplay())
					{
						this.m_freelancerDropdown.AddOption((int)characterResourceLink2.m_characterType, characterResourceLink2.GetDisplayName(), characterResourceLink2.m_characterType);
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
			this.m_freelancerDropdown.AddHitbox(this.m_statsPanel.m_freelancerDropdownBtn.m_button.spriteController.gameObject);
			this.m_freelancerDropdown.AddHitbox(this.m_achievementsPanel.m_freelancerDropdownBtn.m_button.spriteController.gameObject);
			this.m_freelancerDropdown.AddHitbox(this.m_badgesPanel.m_freelancerDropdownBtn.m_button.spriteController.gameObject);
		}
		int selectedValue;
		if (withRoles)
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
			UIPlayerProgressDropdownList freelancerDropdown = this.m_freelancerDropdown;
			if (UIPlayerProgressPanel.<>f__am$cache1 == null)
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
				UIPlayerProgressPanel.<>f__am$cache1 = ((int x) => true);
			}
			freelancerDropdown.CheckOptionDisplayState(UIPlayerProgressPanel.<>f__am$cache1);
			if (role != CharacterRole.None)
			{
				selectedValue = (int)(-(int)role);
			}
			else
			{
				selectedValue = (int)selectedFreelancer;
			}
		}
		else
		{
			UIPlayerProgressDropdownList freelancerDropdown2 = this.m_freelancerDropdown;
			if (UIPlayerProgressPanel.<>f__am$cache2 == null)
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
				UIPlayerProgressPanel.<>f__am$cache2 = ((int x) => x >= 0);
			}
			freelancerDropdown2.CheckOptionDisplayState(UIPlayerProgressPanel.<>f__am$cache2);
			selectedValue = (int)selectedFreelancer;
		}
		this.SetupDropdown(this.m_freelancerDropdown, selectedValue, callback, parentSlot);
	}

	public void OpenGameModeDropdown(PersistedStatBucket selectedBucket, Action<int> callback, Transform parentSlot)
	{
		if (this.m_gameModeDropdown.Initialize())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressPanel.OpenGameModeDropdown(PersistedStatBucket, Action<int>, Transform)).MethodHandle;
			}
			IEnumerator enumerator = Enum.GetValues(typeof(PersistedStatBucket)).GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					object obj = enumerator.Current;
					PersistedStatBucket persistedStatBucket = (PersistedStatBucket)obj;
					if (persistedStatBucket.IsTracked())
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
						this.m_gameModeDropdown.AddOption((int)persistedStatBucket, StringUtil.TR_PersistedStatBucketName(persistedStatBucket), CharacterType.None);
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
			this.m_gameModeDropdown.AddHitbox(this.m_statsPanel.m_gameModeDropdownBtn.m_button.spriteController.gameObject);
			this.m_gameModeDropdown.AddHitbox(this.m_badgesPanel.m_gameModeDropdownBtn.m_button.spriteController.gameObject);
		}
		this.SetupDropdown(this.m_gameModeDropdown, (int)selectedBucket, callback, parentSlot);
	}

	public void OpenSeasonsDropdown(int selectedSeason, Action<int> callback, UIPlayerProgressDropdownBtn.ShouldShow shouldShow, Transform parentSlot)
	{
		if (this.m_seasonsDropdown.Initialize())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressPanel.OpenSeasonsDropdown(int, Action<int>, UIPlayerProgressDropdownBtn.ShouldShow, Transform)).MethodHandle;
			}
			int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
			List<SeasonTemplate> list = new List<SeasonTemplate>();
			for (int i = 0; i < SeasonWideData.Get().m_seasons.Count; i++)
			{
				if (SeasonWideData.Get().m_seasons[i].DisplayStats)
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
					list.Add(SeasonWideData.Get().m_seasons[i]);
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
			list.Reverse();
			using (List<SeasonTemplate>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SeasonTemplate seasonTemplate = enumerator.Current;
					string text;
					if (seasonTemplate.Index == activeSeason)
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
						text = StringUtil.TR("CurrentSeason", "Global");
					}
					else
					{
						text = seasonTemplate.GetDisplayName();
					}
					this.m_seasonsDropdown.AddOption(seasonTemplate.Index, text, CharacterType.None);
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
			}
			this.m_seasonsDropdown.AddHitbox(this.m_statsPanel.m_seasonsDropdownBtn.m_button.spriteController.gameObject);
			this.m_seasonsDropdown.AddHitbox(this.m_badgesPanel.m_seasonsDropdownBtn.m_button.spriteController.gameObject);
		}
		this.m_seasonsDropdown.CheckOptionDisplayState(shouldShow);
		this.SetupDropdown(this.m_seasonsDropdown, selectedSeason, callback, parentSlot);
	}

	public void OpenAchievementDropdown(AchievementType selected, Action<int> callback, Transform parentSlot)
	{
		if (this.m_achievementDropdown.Initialize())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIPlayerProgressPanel.OpenAchievementDropdown(AchievementType, Action<int>, Transform)).MethodHandle;
			}
			List<AchievementType> list = new List<AchievementType>();
			list.Add(AchievementType.None);
			for (int i = 0; i < QuestWideData.Get().m_quests.Count; i++)
			{
				AchievementType achievmentType = QuestWideData.Get().m_quests[i].AchievmentType;
				if (achievmentType != AchievementType.None)
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
					if (!list.Contains(achievmentType))
					{
						list.Add(achievmentType);
					}
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
			list.Sort();
			using (List<AchievementType>.Enumerator enumerator = list.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AchievementType achievementType = enumerator.Current;
					this.m_achievementDropdown.AddOption((int)achievementType, StringUtil.TR("AchievementCategory_" + achievementType, "Global"), CharacterType.None);
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
			this.m_achievementDropdown.AddHitbox(this.m_achievementsPanel.m_categoryDropdownBtn.m_button.spriteController.gameObject);
		}
		this.SetupDropdown(this.m_achievementDropdown, (int)selected, callback, parentSlot);
	}

	public void HideDropdowns()
	{
		this.m_freelancerDropdown.SetVisible(false);
		this.m_gameModeDropdown.SetVisible(false);
		this.m_seasonsDropdown.SetVisible(false);
		this.m_achievementDropdown.SetVisible(false);
	}
}
