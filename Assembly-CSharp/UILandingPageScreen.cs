using LobbyGameClientMessages;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UILandingPageScreen : UIScene
{
	public TextMeshProUGUI m_motd;

	public RectTransform m_motdBg;

	public TextMeshProUGUI[] m_ReleaseNotesBtnTitles;

	public TextMeshProUGUI[] m_ReleaseNotesBtnDescriptions;

	public TextMeshProUGUI[] m_WhatsNewBtnTitles;

	public TextMeshProUGUI[] m_WhatsNewBtnDescriptions;

	public _SelectableBtn[] m_NewsBtns;

	public _SelectableBtn[] m_PatchNotesBtns;

	public _SelectableBtn[] m_VideoBtns;

	public _SelectableBtn[] m_TutorialBtns;

	public _SelectableBtn[] m_FeedbackBtns;

	public _SelectableBtn m_ForumsBtn;

	public RectTransform m_SideShowcase;

	public Image m_characterImage;

	public RectTransform[] m_containers;

	public string[] m_flavorTexts;

	private bool m_isLocked;

	private string m_lockedReason;

	private bool m_visible;

	public bool m_inCustomGame;

	public TextMeshProUGUI m_timedMessage;

	public string m_timedMessageStartTime;

	public string m_timedMessageEndTime;

	[Header("Landing Page Settings")]
	public RectTransform m_LandingPageOpenAnimationContainer;

	public string LandingPageOpenPrefixAnimName;

	public _SelectableBtn m_MoreInfoBtn;

	public _SelectableBtn m_LootMatrixButton;

	public _SelectableBtn m_SideShowcaseMoreInfoButton;

	public AccountComponent.UIStateIdentifier IntroductionScreenToDisplay;

	public TextMeshProUGUI m_SideShowcaseMoreInfoText;

	public _SelectableBtn m_charMoreInfoBtn;

	public CharacterType m_CharMoreInfoBtnCharacterTypeLink;

	[Header("Factions")]
	public RectTransform m_factionsContainer;

	public UISeasonFactionPercentageBar m_factionPercentPrefab;

	public RectTransform m_factionPercentContainer;

	public UISeasonFactionEntry m_factionPrefab;

	public HorizontalLayoutGroup m_factionsLayout;

	public _ButtonSwapSprite m_factionsMoreInfoButton;

	public _SelectableBtn m_factionsClickBox;

	[Header("Tutorial Level")]
	public RectTransform m_tutorialLevelContainer;

	public TextMeshProUGUI m_tutorialLevelText;

	public HorizontalLayoutGroup m_tutorialLevelLayout;

	public UITutorialSeasonLevelBar m_tutorialLevelBarPrefab;

	public TextMeshProUGUI m_tutorialNextRewardLabel;

	public Image m_tutorialRewardIconImage;

	public Image m_tutorialRewardFgImage;

	public UITooltipHoverObject m_tutorialRewardTooltipObj;

	[Header("Season Level")]
	public RectTransform m_seasonLevelContainer;

	public TextMeshProUGUI m_seasonLevelText;

	public ImageFilledSloped m_seasonLevelSlider;

	public TextMeshProUGUI m_seasonExpAmountText;

	public TextMeshProUGUI m_seasonNextRewardLabel;

	public Image m_seasonRewardIconImage;

	public UITooltipHoverObject m_seasonRewardTooltipObj;

	[Header("Character Level")]
	public TextMeshProUGUI m_charLevelText;

	public ImageFilledSloped m_charLevelSlider;

	public TextMeshProUGUI m_charExpAmountText;

	public Image m_charRewardIconImage;

	public Image m_characterIcon;

	public UITooltipHoverObject m_charRewardTooltipObj;

	private static UILandingPageScreen s_instance;

	public CharacterType? CharacterInfoClicked;

	private const float kSecondsToUpdateExperience = 1f;

	private int m_expPerSecond;

	private int m_curLevel = -1;

	private int m_endLevel = -1;

	private int m_curExp = -1;

	private int m_endExp = -1;

	private int m_expToLevel = -1;

	private int m_charLevel;

	private CharacterType m_lastCharacterType;

	private bool m_hackInitialized;

	private List<RewardUtils.RewardData> m_charRewards;

	private List<RewardUtils.RewardData> m_playerRewards;

	private Animator m_landingPageIntroAnimator;

	private SeasonTemplate m_currentSeason;

	private List<UITutorialSeasonLevelBar> m_tutorialLevelSliderBars;

	public static UILandingPageScreen Get()
	{
		return s_instance;
	}

	public override void Awake()
	{
		s_instance = this;
		base.Awake();
	}

	private void Start()
	{
		m_inCustomGame = false;
		m_ForumsBtn.spriteController.callback = ForumsBtnClicked;
		m_factionsMoreInfoButton.callback = FactionsMoreInfoClicked;
		m_factionsClickBox.spriteController.callback = FactionsAreaClicked;
		m_LootMatrixButton.spriteController.callback = LootMatrixButtonClicked;
		m_SideShowcaseMoreInfoButton.spriteController.callback = SideShowcaseMoreInfoButtonClicked;
		m_seasonRewardTooltipObj.Setup(TooltipType.RewardList, delegate(UITooltipBase tooltip)
		{
			if (m_playerRewards != null)
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
				if (m_playerRewards.Count != 0)
				{
					UIRewardListTooltip uIRewardListTooltip3 = tooltip as UIRewardListTooltip;
					uIRewardListTooltip3.Setup(m_playerRewards, m_curLevel + 1, UIRewardListTooltip.RewardsType.Seasons);
					return true;
				}
			}
			return false;
		});
		m_charRewardTooltipObj.Setup(TooltipType.RewardList, delegate(UITooltipBase tooltip)
		{
			if (m_charRewards != null)
			{
				if (m_charRewards.Count != 0)
				{
					UIRewardListTooltip uIRewardListTooltip2 = tooltip as UIRewardListTooltip;
					uIRewardListTooltip2.Setup(m_charRewards, m_charLevel, UIRewardListTooltip.RewardsType.Character);
					return true;
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
			}
			return false;
		});
		m_tutorialRewardTooltipObj.Setup(TooltipType.RewardList, delegate(UITooltipBase tooltip)
		{
			if (m_playerRewards != null)
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
				if (m_playerRewards.Count != 0)
				{
					UIRewardListTooltip uIRewardListTooltip = tooltip as UIRewardListTooltip;
					uIRewardListTooltip.Setup(m_playerRewards, m_curLevel - 1, UIRewardListTooltip.RewardsType.Tutorial);
					return true;
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
			return false;
		});
		m_charMoreInfoBtn.spriteController.callback = CharMoreInfoClicked;
		TextMeshProUGUI[] componentsInChildren = m_ForumsBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].text = "FORUMS";
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
			m_ForumsBtn.SetRecordMetricClick(true, "CLICK: Landing Page Forums Button");
			m_ForumsBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.GenericSmall;
			if (m_MoreInfoBtn != null)
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
				m_MoreInfoBtn.spriteController.callback = MoreInfoClicked;
				m_MoreInfoBtn.SetRecordMetricClick(true, "CLICK: Landing Page MoreInfo Button");
				m_MoreInfoBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.GenericSmall;
			}
			SetUpButtons(m_NewsBtns, NewsBtnClicked, "CLICK: Landing Page News Button", FrontEndButtonSounds.CharacterSelectModAdd);
			SetUpButtons(m_PatchNotesBtns, PatchNotesBtnClicked, "CLICK: Landing Page Patch Notes Button", FrontEndButtonSounds.CharacterSelectModAdd);
			SetUpButtons(m_VideoBtns, VideoBtnClicked, "CLICK: Landing Page Video Button", FrontEndButtonSounds.CharacterSelectModAdd);
			SetUpButtons(m_TutorialBtns, TutorialBtnClicked, "CLICK: Landing Page Tutorial Button", FrontEndButtonSounds.CharacterSelectModAdd);
			SetUpButtons(m_FeedbackBtns, FeedbackBtnClicked, "CLICK: Landing Page Feedback Button", FrontEndButtonSounds.CharacterSelectModAdd);
			OnFactionCompetitionNotification(null);
			m_tutorialLevelSliderBars = new List<UITutorialSeasonLevelBar>();
			m_tutorialLevelSliderBars.AddRange(m_tutorialLevelLayout.GetComponentsInChildren<UITutorialSeasonLevelBar>());
			if (ClientGameManager.Get() != null)
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
				ClientGameManager clientGameManager = ClientGameManager.Get();
				clientGameManager.OnAccountDataUpdated += OnAccountDataUpdated;
				clientGameManager.OnFactionCompetitionNotification += OnFactionCompetitionNotification;
				clientGameManager.OnLobbyGameplayOverridesChange += HandleLobbyGameplayOverridesChange;
				clientGameManager.OnCharacterDataUpdated += OnCharacterDataUpdated;
				if (clientGameManager.IsPlayerAccountDataAvailable())
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
					OnAccountDataUpdated(clientGameManager.GetPlayerAccountData());
				}
			}
			m_isLocked = false;
			m_lockedReason = string.Empty;
			m_motd.text = string.Empty;
			UIManager.SetGameObjectActive(m_motdBg, false);
			if (AppState_LandingPage.Get() != null && AppState_LandingPage.Get().ReceivedLobbyStatusInfo)
			{
				ShowMOTD();
				SetServerIsLocked(ClientGameManager.Get().IsServerLocked);
				if (UIFrontEnd.Get().m_frontEndNavPanel != null)
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
					UIFrontEnd.Get().m_frontEndNavPanel.SetShopVisible(GameManager.Get().GameplayOverrides.EnableShop);
					UIFrontEnd.Get().m_frontEndNavPanel.CheckSeasonsVisibility();
				}
			}
			UIManager.SetGameObjectActive(m_timedMessage, false);
			UISeasonFactionEntry[] componentsInChildren2 = m_factionsLayout.GetComponentsInChildren<UISeasonFactionEntry>(true);
			foreach (UISeasonFactionEntry uISeasonFactionEntry in componentsInChildren2)
			{
				m_factionsClickBox.spriteController.AddSubButton(uISeasonFactionEntry.m_hitbox);
				uISeasonFactionEntry.m_hitbox.callback = m_factionsClickBox.spriteController.callback;
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

	private void HYDRO10288Hack()
	{
		if (m_hackInitialized)
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
			if (ClientGameManager.Get() == null)
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
				if (!ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
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
				OnFactionCompetitionNotification(null);
				OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
				m_hackInitialized = true;
				return;
			}
		}
	}

	private void SetUpButtons(_SelectableBtn[] btns, _ButtonSwapSprite.ButtonClickCallback clickCallback, string metricClickString, FrontEndButtonSounds clickSound)
	{
		for (int i = 0; i < btns.Length; i++)
		{
			btns[i].spriteController.callback = clickCallback;
			btns[i].SetRecordMetricClick(true, metricClickString);
			btns[i].spriteController.m_soundToPlay = clickSound;
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
			return;
		}
	}

	public override SceneType GetSceneType()
	{
		return SceneType.LandingPage;
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnAccountDataUpdated -= OnAccountDataUpdated;
			ClientGameManager.Get().OnFactionCompetitionNotification -= OnFactionCompetitionNotification;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= HandleLobbyGameplayOverridesChange;
			ClientGameManager.Get().OnCharacterDataUpdated -= OnCharacterDataUpdated;
		}
	}

	private void HandleLobbyGameplayOverridesChange(LobbyGameplayOverrides gameplayOverrides)
	{
		OnFactionCompetitionNotification(null);
	}

	public IEnumerator SetupFactionCompetitionBars(FactionCompetitionNotification notification)
	{
		int activeIndex;
		if (notification == null)
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
			if (ClientGameManager.Get() == null)
			{
				while (true)
				{
					switch (5)
					{
					default:
						yield break;
					case 0:
						break;
					}
				}
			}
			activeIndex = ClientGameManager.Get().ActiveFactionCompetition;
			Dictionary<int, long> scores2 = ClientGameManager.Get().FactionScores;
		}
		else
		{
			activeIndex = notification.ActiveIndex;
			Dictionary<int, long> scores2 = notification.Scores;
		}
		FactionCompetition factionCompetiton = FactionWideData.Get().GetFactionCompetition(activeIndex);
		SeasonLockoutReason reason;
		bool canViewSeasons = UISeasonsPanel.CheckSeasonsVisibility(out reason);
		if (factionCompetiton != null)
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
			if (factionCompetiton.Enabled && factionCompetiton.ShouldShowcase && canViewSeasons)
			{
				UIManager.SetGameObjectActive(m_factionsContainer, true);
				List<UISeasonFactionEntry> entries = new List<UISeasonFactionEntry>();
				entries.AddRange(m_factionsLayout.GetComponentsInChildren<UISeasonFactionEntry>(true));
				for (int num = entries.Count - 1; num >= factionCompetiton.Factions.Count; num--)
				{
					UnityEngine.Object.Destroy(entries[num].gameObject);
					entries.RemoveAt(num);
				}
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					for (int i = entries.Count; i < factionCompetiton.Factions.Count; i++)
					{
						UISeasonFactionEntry uISeasonFactionEntry = UnityEngine.Object.Instantiate(m_factionPrefab);
						uISeasonFactionEntry.transform.SetParent(m_factionsLayout.transform);
						uISeasonFactionEntry.transform.localScale = Vector3.one;
						uISeasonFactionEntry.transform.localPosition = Vector3.zero;
						entries.Add(uISeasonFactionEntry);
						m_factionsClickBox.spriteController.AddSubButton(uISeasonFactionEntry.m_hitbox);
						uISeasonFactionEntry.m_hitbox.callback = m_factionsClickBox.spriteController.callback;
					}
					yield return 0;
					/*Error: Unable to find new state assignment for yield return*/;
				}
			}
		}
		UIManager.SetGameObjectActive(m_factionsContainer, false);
	}

	private void OnFactionCompetitionNotification(FactionCompetitionNotification notification)
	{
		StartCoroutine(SetupFactionCompetitionBars(notification));
	}

	public void FactionsAreaClicked(BaseEventData data)
	{
		UIFrontEnd.Get().m_frontEndNavPanel.DoSeasonsBtnClicked(true, false);
	}

	public void MoreInfoClicked(BaseEventData data)
	{
	}

	public void FactionsMoreInfoClicked(BaseEventData data)
	{
		UIFactionsIntroduction.Get().SetupIntro(null);
	}

	public void CharMoreInfoClicked(BaseEventData data)
	{
		CharacterInfoClicked = m_CharMoreInfoBtnCharacterTypeLink;
		UICharacterScreen.CharacterSelectSceneStateParameters characterSelectSceneStateParameters = new UICharacterScreen.CharacterSelectSceneStateParameters();
		characterSelectSceneStateParameters.ClientRequestToServerSelectCharacter = m_CharMoreInfoBtnCharacterTypeLink;
		characterSelectSceneStateParameters.SideButtonsVisible = true;
		UIManager.Get().HandleNewSceneStateParameter(characterSelectSceneStateParameters);
		UIFrontEnd.Get().m_frontEndNavPanel.PlayBtnClicked(null);
		UICharacterSelectCharacterSettingsPanel.Get().SetVisible(true, UICharacterSelectCharacterSettingsPanel.TabPanel.General);
	}

	public void CheckSeasonsVisibility()
	{
		if (UISeasonsPanel.CheckSeasonsVisibility(out SeasonLockoutReason _))
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
					m_SideShowcaseMoreInfoText.text = StringUtil.TR("LandingPageChapterInfoLabel", "NewFrontEndScene");
					return;
				}
			}
		}
		m_SideShowcaseMoreInfoText.text = StringUtil.TR("MoreInfo", "NewFrontEndScene");
	}

	public void SideShowcaseMoreInfoButtonClicked(BaseEventData data)
	{
		SeasonLockoutReason lockoutReason;
		bool flag = UISeasonsPanel.CheckSeasonsVisibility(out lockoutReason);
		FrontEndFullScreenAnnouncements frontEndFullScreenAnnouncements = FrontEndFullScreenAnnouncements.Get();
		AccountComponent.UIStateIdentifier introductionScreenToDisplay = IntroductionScreenToDisplay;
		int pageNum;
		if (flag)
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
			pageNum = 0;
		}
		else
		{
			pageNum = 1;
		}
		frontEndFullScreenAnnouncements.SetIntroductionVisible(introductionScreenToDisplay, pageNum);
		UINewUserFlowManager.OnChapterMoreInfoClicked();
	}

	public void LootMatrixButtonClicked(BaseEventData data)
	{
		FrontEndNavPanel.Get().CashShopBtnClicked(data);
	}

	public void NewsBtnClicked(BaseEventData data)
	{
		UILandingPageFullScreenMenus.Get().DisplayWhatsNew();
	}

	public void PatchNotesBtnClicked(BaseEventData data)
	{
		UILandingPageFullScreenMenus.Get().DisplayPatchNotes();
	}

	public void VideoBtnClicked(BaseEventData data)
	{
		UILandingPageFullScreenMenus.Get().DisplayVideo("Video/HowTo", "How to play");
	}

	public void FeedbackBtnClicked(BaseEventData data)
	{
		UILandingPageFullScreenMenus.Get().ToggleFeedbackContainerVisible();
	}

	public void ForumsBtnClicked(BaseEventData data)
	{
		Application.OpenURL("http://forums.atlasreactorgame.com/");
	}

	public void TutorialBtnClicked(BaseEventData data)
	{
		if (ClientGameManager.Get().IsServerLocked)
		{
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("ServerIsLocked", "Global"), StringUtil.TR("CannotStartTutorial", "Global"), StringUtil.TR("Ok", "Global"));
			return;
		}
		bool flag = true;
		if (ClientGameManager.Get().GroupInfo.InAGroup)
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
			flag = false;
		}
		if (AppState.GetCurrent() == AppState_CharacterSelect.Get())
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
			flag = false;
		}
		if (AppState_GroupCharacterSelect.Get().InQueue())
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
			flag = false;
		}
		if (!flag)
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
			AppState_LandingPage.Get().OnTutorial1Clicked();
			return;
		}
	}

	public void QuickPlayButtonClicked(BaseEventData data)
	{
		if (!m_isLocked)
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.Generic);
			AppState_LandingPage.Get().OnQuickPlayClicked();
		}
	}

	public bool IsVisible()
	{
		return m_visible;
	}

	public void SetVisible(bool visible)
	{
		if (m_visible == visible)
		{
			while (true)
			{
				switch (5)
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
		m_visible = visible;
		for (int i = 0; i < m_containers.Length; i++)
		{
			UIManager.SetGameObjectActive(m_containers[i], m_visible);
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			UIManager.Get().SetSceneVisible(GetSceneType(), visible, new SceneVisibilityParameters());
			if (!visible)
			{
				return;
			}
			UIFrontEnd.Get().m_frontEndNavPanel.SetNavButtonSelected(UIFrontEnd.Get().m_frontEndNavPanel.m_landingPageBtn);
			UpdateMatchData();
			ClientGameManager clientGameManager = ClientGameManager.Get();
			if (clientGameManager != null)
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
				if (clientGameManager.IsConnectedToLobbyServer)
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
					clientGameManager.SendCheckAccountStatusRequest(AppState_LandingPage.Get().HandleCheckAccountStatusResponse);
					clientGameManager.SendCheckRAFStatusRequest(false);
				}
				CheckForTrustWarEnd();
			}
			CheckTutorialButton();
			if (UITutorialSeasonInterstitial.Get().HasBeenViewed())
			{
				return;
			}
			PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
			SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(playerAccountData.QuestComponent.ActiveSeason);
			if (seasonTemplate == null)
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
				if (seasonTemplate.IsTutorial)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						UITutorialSeasonInterstitial.Get().Setup(seasonTemplate, playerAccountData.QuestComponent.SeasonLevel, false);
						UITutorialSeasonInterstitial.Get().SetVisible(true);
						return;
					}
				}
				return;
			}
		}
	}

	private void CheckForTrustWarEnd()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager == null)
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
			if (!clientGameManager.IsPlayerAccountDataAvailable())
			{
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
			PersistedAccountData playerAccountData = clientGameManager.GetPlayerAccountData();
			AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.HasSeenTrustWarEndPopup;
			if (playerAccountData.AccountComponent.GetUIState(uiState) == 0 && playerAccountData.QuestComponent.ActiveSeason == 1)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					ClientGameManager.Get().RequestUpdateUIState(uiState, 1, null);
					UIDialogPopupManager.OpenTrustWarEndDialog();
					return;
				}
			}
			return;
		}
	}

	private void CheckTutorialButton()
	{
		if (ClientGameManager.Get() != null)
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
			if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
				if (ClientGameManager.Get().GetPlayerAccountData().ExperienceComponent.EnteredTutorial == 0)
				{
					SetBtnVisible(m_TutorialBtns, true);
					SetBtnVisible(m_VideoBtns, false);
					return;
				}
			}
		}
		SetBtnVisible(m_TutorialBtns, false);
		SetBtnVisible(m_VideoBtns, true);
	}

	public void UpdateMatchData()
	{
	}

	public string GrabRandomFlavorText()
	{
		string str = "<color=#ffc000>";
		int num = Mathf.FloorToInt(UnityEngine.Random.value * (float)m_flavorTexts.Length);
		str += m_flavorTexts[num];
		return str + "</color>";
	}

	public void ShowMOTD()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (!(clientGameManager != null))
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
			string language = HydrogenConfig.Get().Language;
			m_motd.text = clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.MOTDText, language);
			UIManager.SetGameObjectActive(m_motdBg, !m_motd.text.IsNullOrEmpty());
			SetArrayText(m_ReleaseNotesBtnDescriptions, clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.ReleaseNotesDescription, language));
			SetArrayText(m_ReleaseNotesBtnTitles, clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.ReleaseNotesHeader, language));
			SetArrayText(m_WhatsNewBtnDescriptions, clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.WhatsNewDescription, language));
			SetArrayText(m_WhatsNewBtnTitles, clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.WhatsNewHeader, language));
			return;
		}
	}

	public void SetServerIsLocked(bool isLocked)
	{
		m_isLocked = isLocked;
		string language = HydrogenConfig.Get().Language;
		m_lockedReason = ClientGameManager.Get().ServerMessageOverrides.GetValueOrDefault(ServerMessageType.LockScreenText, language);
		UpdateUIForAccessChange();
		if (UIStorePanel.Get() != null)
		{
			UIStorePanel.Get().ClosePurchaseDialog();
		}
	}

	public void UpdateUIForAccessChange()
	{
		if (m_isLocked)
		{
			if (UILandingPageFullScreenMenus.Get().IsMessageContainerVisible())
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
				UILandingPageFullScreenMenus uILandingPageFullScreenMenus = UILandingPageFullScreenMenus.Get();
				string title = StringUtil.TR("ServerIsLocked", "Global");
				string lockedReason = m_lockedReason;
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
					_003C_003Ef__am_0024cache0 = delegate
					{
						AppState_Shutdown.Get().Enter();
					};
				}
				uILandingPageFullScreenMenus.DisplayMessage(title, lockedReason, _003C_003Ef__am_0024cache0);
				return;
			}
		}
		if (!UILandingPageFullScreenMenus.Get().IsMessageContainerVisible())
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
			UILandingPageFullScreenMenus.Get().SetMessageContainerVisible(false);
			return;
		}
	}

	private void OnAccountDataUpdated(PersistedAccountData accountData)
	{
		m_currentSeason = SeasonWideData.Get().GetSeasonTemplate(accountData.QuestComponent.ActiveSeason);
		UIManager.SetGameObjectActive(m_tutorialLevelContainer, m_currentSeason != null && m_currentSeason.IsTutorial);
		RectTransform seasonLevelContainer = m_seasonLevelContainer;
		int doActive;
		if (m_currentSeason != null)
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
			doActive = ((!m_currentSeason.IsTutorial) ? 1 : 0);
		}
		else
		{
			doActive = 0;
		}
		UIManager.SetGameObjectActive(seasonLevelContainer, (byte)doActive != 0);
		CheckSeasonsVisibility();
		CharacterType characterType = accountData.AccountComponent.LastCharacter;
		if (!characterType.IsValidForHumanGameplay())
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
			characterType = CharacterType.Scoundrel;
		}
		Sprite sprite = (Sprite)Resources.Load("Characters/full_" + characterType, typeof(Sprite));
		if (sprite != null)
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
			m_characterImage.sprite = sprite;
		}
		else
		{
			m_characterImage.sprite = (Sprite)Resources.Load("Characters/full_SpaceMarine", typeof(Sprite));
		}
		m_characterIcon.sprite = Resources.Load<Sprite>(GameWideData.Get().GetCharacterResourceLink(characterType).m_characterSelectIconResourceString);
		if (accountData.QuestComponent.SeasonExperience.ContainsKey(accountData.QuestComponent.ActiveSeason))
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
			m_endLevel = 0;
			m_endLevel = accountData.QuestComponent.SeasonExperience[accountData.QuestComponent.ActiveSeason].Level;
			m_endExp = accountData.QuestComponent.SeasonExperience[accountData.QuestComponent.ActiveSeason].XPProgressThroughLevel;
		}
		else
		{
			m_endLevel = 1;
			m_endExp = 0;
		}
		if (m_curLevel < 0)
		{
			m_curLevel = m_endLevel;
			m_curExp = m_endExp;
		}
		m_expPerSecond = 0;
		if (m_currentSeason != null && m_currentSeason.IsTutorial)
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
			m_curLevel = m_endLevel;
			int endLevel = QuestWideData.GetEndLevel(m_currentSeason.Prerequisites, m_currentSeason.Index);
			m_tutorialLevelText.text = m_curLevel - 1 + "/" + (endLevel - 1);
			for (int i = m_tutorialLevelSliderBars.Count; i < endLevel - 1; i++)
			{
				UITutorialSeasonLevelBar uITutorialSeasonLevelBar = UnityEngine.Object.Instantiate(m_tutorialLevelBarPrefab);
				uITutorialSeasonLevelBar.transform.SetParent(m_tutorialLevelLayout.transform);
				uITutorialSeasonLevelBar.transform.localScale = Vector3.one;
				uITutorialSeasonLevelBar.transform.localPosition = Vector3.zero;
				m_tutorialLevelSliderBars.Add(uITutorialSeasonLevelBar);
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
			Queue<RewardUtils.RewardData> queue = new Queue<RewardUtils.RewardData>(RewardUtils.GetSeasonLevelRewards());
			List<RewardUtils.RewardData> availableSeasonEndRewards = RewardUtils.GetAvailableSeasonEndRewards(m_currentSeason);
			if (availableSeasonEndRewards.Count > 0)
			{
				queue.Enqueue(availableSeasonEndRewards[0]);
			}
			for (int j = 0; j < m_tutorialLevelSliderBars.Count; j++)
			{
				int num = j + 1;
				m_tutorialLevelSliderBars[j].SetFilled(num < m_curLevel);
				UIManager.SetGameObjectActive(m_tutorialLevelSliderBars[j], num < endLevel);
				RewardUtils.RewardData rewardData = null;
				while (queue.Count > 0 && rewardData == null)
				{
					int num2 = queue.Peek().Level - 1;
					if (num2 < num)
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
						queue.Dequeue();
						continue;
					}
					if (num2 > num)
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
						break;
					}
					rewardData = queue.Dequeue();
				}
				m_tutorialLevelSliderBars[j].SetReward(num, rewardData);
			}
			SetupTutorialRewards();
		}
		else
		{
			try
			{
				m_expToLevel = SeasonWideData.Get().GetSeasonExperience(accountData.QuestComponent.ActiveSeason, m_curLevel);
			}
			catch (ArgumentException)
			{
				m_expToLevel = 0;
			}
			if (m_endLevel == m_curLevel)
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
				m_expPerSecond = m_endExp - m_curExp;
			}
			else
			{
				m_expPerSecond = m_expToLevel;
			}
			m_expPerSecond = (int)((float)m_expPerSecond / 1f);
			if (m_expPerSecond < 1)
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
				m_expPerSecond = 1;
			}
			if (m_curLevel > m_endLevel)
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
				m_curLevel = m_endLevel;
				m_curExp = m_endExp;
				m_expPerSecond = 1;
			}
			m_seasonLevelSlider.fillAmount = (float)m_curExp / (float)m_expToLevel;
			SetupNextSeasonReward();
		}
		m_lastCharacterType = characterType;
		if (ClientGameManager.Get().IsPlayerCharacterDataAvailable(m_lastCharacterType))
		{
			OnCharacterDataUpdated(ClientGameManager.Get().GetPlayerCharacterData(m_lastCharacterType));
		}
		if (!IsVisible())
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
			CheckForTrustWarEnd();
			return;
		}
	}

	private void SetArrayText(TextMeshProUGUI[] textMeshes, string text)
	{
		for (int i = 0; i < textMeshes.Length; i++)
		{
			textMeshes[i].text = text;
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
			return;
		}
	}

	private void SetBtnVisible(_SelectableBtn[] btns, bool visible)
	{
		for (int i = 0; i < btns.Length; i++)
		{
			UIManager.SetGameObjectActive(btns[i], visible);
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
			return;
		}
	}

	private void Update()
	{
		HYDRO10288Hack();
		bool flag = false;
		if (m_visible)
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
			if (!UIFrontendLoadingScreen.Get().IsVisible())
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
				flag = true;
			}
		}
		if (m_landingPageIntroAnimator == null)
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
			m_landingPageIntroAnimator = m_LandingPageOpenAnimationContainer.GetComponentInChildren<Animator>(true);
			UIManager.SetGameObjectActive(m_LandingPageOpenAnimationContainer, true);
		}
		string empty = string.Empty;
		empty = LandingPageOpenPrefixAnimName;
		UIManager.SetGameObjectActive(m_SideShowcase, flag);
		if (m_landingPageIntroAnimator != null)
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
			UIManager.SetGameObjectActive(m_landingPageIntroAnimator, true);
			if (flag && m_landingPageIntroAnimator.gameObject.activeInHierarchy)
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
				if (!m_landingPageIntroAnimator.GetCurrentAnimatorStateInfo(0).IsName(empty + "IN"))
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
					if (!m_landingPageIntroAnimator.GetCurrentAnimatorStateInfo(0).IsName(empty + "IDLE"))
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
						UIAnimationEventManager.Get().PlayAnimation(m_landingPageIntroAnimator, empty + "IN", null, string.Empty);
					}
				}
			}
		}
		if (m_timedMessage != null)
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
			if (!m_timedMessageEndTime.IsNullOrEmpty())
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
				if (!m_timedMessageStartTime.IsNullOrEmpty())
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
					DateTime t = Convert.ToDateTime(m_timedMessageStartTime);
					DateTime t2 = Convert.ToDateTime(m_timedMessageEndTime);
					ClientGameManager clientGameManager = ClientGameManager.Get();
					TextMeshProUGUI timedMessage = m_timedMessage;
					int doActive;
					if (clientGameManager != null && clientGameManager.PacificNow() < t2)
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
						doActive = ((clientGameManager.PacificNow() >= t) ? 1 : 0);
					}
					else
					{
						doActive = 0;
					}
					UIManager.SetGameObjectActive(timedMessage, (byte)doActive != 0);
				}
			}
		}
		if (m_currentSeason == null)
		{
			Debug.LogError(string.Concat(GetType(), " m_currentSeason is null, please fix me"));
		}
		if (m_currentSeason == null)
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
			if (m_currentSeason.IsTutorial || m_expPerSecond == 0)
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
				if (UIFrontEnd.Get() == null)
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
					if (UIFrontEnd.Get().m_playerPanel == null)
					{
						return;
					}
					if (UIFrontEnd.Get().m_playerPanel.IsPlayingLevelUpAnim())
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
					if (m_curLevel > m_endLevel)
					{
						m_curLevel = m_endLevel;
						SetupNextSeasonReward();
					}
					int num = (int)((float)m_expPerSecond * Time.deltaTime);
					if (num < 1)
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
						num = 1;
					}
					m_curExp += num;
					if (m_curLevel == m_endLevel)
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
						if (m_curExp >= m_endExp)
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
							m_curExp = m_endExp;
							m_expPerSecond = 0;
							goto IL_0408;
						}
					}
					if (m_curExp >= m_expToLevel)
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
						m_curExp = 0;
						m_curLevel++;
						int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
						int seasonExperience = SeasonWideData.Get().GetSeasonExperience(activeSeason, ClientGameManager.Get().GetPlayerAccountData().QuestComponent.GetSeasonExperienceComponent(activeSeason).Level);
						m_expPerSecond = (m_expToLevel = seasonExperience);
						SetupNextSeasonReward();
					}
					goto IL_0408;
					IL_0408:
					m_seasonLevelSlider.fillAmount = (float)m_curExp / (float)m_expToLevel;
					UIManager.SetGameObjectActive(m_seasonLevelSlider, true);
					m_seasonExpAmountText.text = UIStorePanel.FormatIntToString(m_curExp) + " / " + UIStorePanel.FormatIntToString(m_expToLevel);
					m_seasonLevelText.text = m_curLevel.ToString();
					return;
				}
			}
		}
	}

	private void SetupNextSeasonReward()
	{
		m_playerRewards = RewardUtils.GetNextSeasonLevelRewards(m_curLevel);
		if (m_playerRewards.Count == 0)
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
					UIManager.SetGameObjectActive(m_seasonRewardTooltipObj, false);
					UIManager.SetGameObjectActive(m_seasonRewardIconImage, false);
					UIManager.SetGameObjectActive(m_seasonNextRewardLabel, false);
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_seasonRewardTooltipObj, true);
		UIManager.SetGameObjectActive(m_seasonRewardIconImage, true);
		UIManager.SetGameObjectActive(m_seasonNextRewardLabel, true);
		string text = "QuestRewards/general";
		using (List<RewardUtils.RewardData>.Enumerator enumerator = m_playerRewards.GetEnumerator())
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
				RewardUtils.RewardData current = enumerator.Current;
				if (!current.isRepeating && current.Type != RewardUtils.RewardType.Lockbox)
				{
					if (current.InventoryTemplate != null)
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
						if (current.InventoryTemplate.Index == 515)
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
					text = current.SpritePath;
					break;
				}
			}
		}
		if (text == "QuestRewards/general")
		{
			text = m_playerRewards[0].SpritePath;
		}
		m_seasonRewardIconImage.sprite = (Sprite)Resources.Load(text, typeof(Sprite));
		m_seasonRewardTooltipObj.Refresh();
	}

	private void SetupTutorialRewards()
	{
		m_playerRewards = RewardUtils.GetSeasonLevelRewards();
		for (int i = 0; i < QuestWideData.GetEndLevel(m_currentSeason.Prerequisites, m_currentSeason.Index); i++)
		{
			using (List<RewardUtils.RewardData>.Enumerator enumerator = RewardUtils.GetAccountLevelRewards(i).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RewardUtils.RewardData current = enumerator.Current;
					int index = 0;
					int num = 0;
					while (true)
					{
						if (num >= m_playerRewards.Count)
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
						if (current.Level <= m_playerRewards[num].Level)
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
							index = num;
							break;
						}
						num++;
					}
					m_playerRewards.Insert(index, current);
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
			}
		}
		List<RewardUtils.RewardData> availableSeasonEndRewards = RewardUtils.GetAvailableSeasonEndRewards(m_currentSeason);
		m_playerRewards.AddRange(availableSeasonEndRewards);
		for (int j = 0; j < m_playerRewards.Count; j++)
		{
			m_playerRewards[j].Level--;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			RewardUtils.RewardData rewardData = null;
			if (availableSeasonEndRewards.Count > 0)
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
				rewardData = availableSeasonEndRewards[0];
			}
			if (rewardData == null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						UIManager.SetGameObjectActive(m_seasonRewardTooltipObj, false);
						UIManager.SetGameObjectActive(m_seasonRewardIconImage, false);
						UIManager.SetGameObjectActive(m_seasonNextRewardLabel, false);
						return;
					}
				}
			}
			UIManager.SetGameObjectActive(m_seasonRewardTooltipObj, true);
			UIManager.SetGameObjectActive(m_seasonRewardIconImage, true);
			UIManager.SetGameObjectActive(m_seasonNextRewardLabel, true);
			m_tutorialRewardIconImage.sprite = Resources.Load<Sprite>(rewardData.SpritePath);
			UIManager.SetGameObjectActive(m_tutorialRewardFgImage, rewardData.Foreground != null);
			m_tutorialRewardFgImage.sprite = rewardData.Foreground;
			m_tutorialNextRewardLabel.text = rewardData.Name;
			m_seasonRewardTooltipObj.Refresh();
			return;
		}
	}

	public void OnCharacterDataUpdated(PersistedCharacterData newData)
	{
		if (newData == null)
		{
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
				return;
			}
		}
		if (m_lastCharacterType != newData.CharacterType)
		{
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
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(m_lastCharacterType);
		m_charLevelText.text = newData.ExperienceComponent.Level.ToString();
		int num = gameBalanceVars.CharacterExperienceToLevel(newData.ExperienceComponent.Level);
		m_charLevelSlider.fillAmount = (float)newData.ExperienceComponent.XPProgressThroughLevel / (float)num;
		UIManager.SetGameObjectActive(m_charLevelSlider, true);
		m_charExpAmountText.text = newData.ExperienceComponent.XPProgressThroughLevel + " / " + num;
		m_charRewards = RewardUtils.GetCharacterRewards(characterResourceLink);
		for (int i = 0; i < gameBalanceVars.RepeatingCharacterLevelRewards.Length; i++)
		{
			if (gameBalanceVars.RepeatingCharacterLevelRewards[i].charType == (int)m_lastCharacterType && gameBalanceVars.RepeatingCharacterLevelRewards[i].repeatingLevel > 0)
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
				RewardUtils.RewardData rewardData = new RewardUtils.RewardData();
				rewardData.Amount = gameBalanceVars.RepeatingCharacterLevelRewards[i].reward.Amount;
				InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(gameBalanceVars.RepeatingCharacterLevelRewards[i].reward.ItemTemplateId);
				rewardData.Name = itemTemplate.GetDisplayName();
				rewardData.SpritePath = itemTemplate.IconPath;
				rewardData.Level = gameBalanceVars.RepeatingCharacterLevelRewards[i].startLevel;
				rewardData.InventoryTemplate = itemTemplate;
				rewardData.repeatLevels = gameBalanceVars.RepeatingCharacterLevelRewards[i].repeatingLevel;
				rewardData.isRepeating = true;
				m_charRewards.Add(rewardData);
			}
		}
		m_charLevel = newData.ExperienceComponent.Level;
		RewardUtils.RewardData rewardToUseForDisplay = GetRewardToUseForDisplay(m_charRewards, m_charLevel);
		if (rewardToUseForDisplay != null)
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
			if (!string.IsNullOrEmpty(rewardToUseForDisplay.SpritePath))
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
				m_charRewardIconImage.sprite = Resources.Load<Sprite>(rewardToUseForDisplay.SpritePath);
				goto IL_026a;
			}
		}
		m_charRewardIconImage.sprite = null;
		goto IL_026a;
		IL_026a:
		m_charRewardTooltipObj.Refresh();
	}

	private RewardUtils.RewardData GetRewardToUseForDisplay(List<RewardUtils.RewardData> possibleRewards, int curLevel)
	{
		List<RewardUtils.RewardData> list = new List<RewardUtils.RewardData>();
		int num = -1;
		for (int i = 0; i < possibleRewards.Count; i++)
		{
			if (possibleRewards[i].Level <= curLevel)
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
				if (!possibleRewards[i].isRepeating)
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
					break;
				}
			}
			if (list.Count == 0)
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
				list.Add(possibleRewards[i]);
				num = possibleRewards[i].Level;
			}
			else if (num == possibleRewards[i].Level)
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
				list.Add(possibleRewards[i]);
			}
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			for (int j = 0; j < GameBalanceVars.Get().RewardDisplayPriorityOrder.Length; j++)
			{
				for (int k = 0; k < list.Count; k++)
				{
					if (GameBalanceVars.Get().RewardDisplayPriorityOrder[j] == list[k].Type)
					{
						return list[k];
					}
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						goto end_IL_0106;
					}
					continue;
					end_IL_0106:
					break;
				}
			}
			if (list.Count > 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return list[0];
					}
				}
			}
			return null;
		}
	}
}
