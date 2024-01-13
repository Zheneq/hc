using System;
using System.Collections;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

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
		m_seasonRewardTooltipObj.Setup(TooltipType.RewardList, tooltip =>
		{
			if (m_playerRewards == null || m_playerRewards.Count == 0)
			{
				return false;
			}

			UIRewardListTooltip t = tooltip as UIRewardListTooltip;
			t.Setup(m_playerRewards, m_curLevel + 1, UIRewardListTooltip.RewardsType.Seasons);
			return true;
		});
		m_charRewardTooltipObj.Setup(TooltipType.RewardList, tooltip =>
		{
			if (m_charRewards == null || m_charRewards.Count == 0)
			{
				return false;
			}

			UIRewardListTooltip t = tooltip as UIRewardListTooltip;
			t.Setup(m_charRewards, m_charLevel, UIRewardListTooltip.RewardsType.Character);
			return true;
		});
		m_tutorialRewardTooltipObj.Setup(TooltipType.RewardList, tooltip =>
		{
			if (m_playerRewards == null || m_playerRewards.Count == 0)
			{
				return false;
			}

			UIRewardListTooltip t = tooltip as UIRewardListTooltip;
			t.Setup(m_playerRewards, m_curLevel - 1, UIRewardListTooltip.RewardsType.Tutorial);
			return true;
		});
		m_charMoreInfoBtn.spriteController.callback = CharMoreInfoClicked;
		TextMeshProUGUI[] componentsInChildren = m_ForumsBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
		foreach (TextMeshProUGUI text in componentsInChildren)
		{
			text.text = "FORUMS";
		}
		m_ForumsBtn.SetRecordMetricClick(true, "CLICK: Landing Page Forums Button");
		m_ForumsBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.GenericSmall;
		if (m_MoreInfoBtn != null)
		{
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
			ClientGameManager clientGameManager = ClientGameManager.Get();
			clientGameManager.OnAccountDataUpdated += OnAccountDataUpdated;
			clientGameManager.OnFactionCompetitionNotification += OnFactionCompetitionNotification;
			clientGameManager.OnLobbyGameplayOverridesChange += HandleLobbyGameplayOverridesChange;
			clientGameManager.OnCharacterDataUpdated += OnCharacterDataUpdated;
			if (clientGameManager.IsPlayerAccountDataAvailable())
			{
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
				UIFrontEnd.Get().m_frontEndNavPanel.SetShopVisible(GameManager.Get().GameplayOverrides.EnableShop);
				UIFrontEnd.Get().m_frontEndNavPanel.CheckSeasonsVisibility();
			}
		}
		UIManager.SetGameObjectActive(m_timedMessage, false);
		foreach (UISeasonFactionEntry factionEntry in m_factionsLayout.GetComponentsInChildren<UISeasonFactionEntry>(true))
		{
			m_factionsClickBox.spriteController.AddSubButton(factionEntry.m_hitbox);
			factionEntry.m_hitbox.callback = m_factionsClickBox.spriteController.callback;
		}
	}

	private void HYDRO10288Hack()
	{
		if (m_hackInitialized
		    || ClientGameManager.Get() == null
		    || !ClientGameManager.Get().IsPlayerAccountDataAvailable())
		{
			return;
		}
		
		OnFactionCompetitionNotification(null);
		OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
		m_hackInitialized = true;
	}

	private void SetUpButtons(
		_SelectableBtn[] btns,
		_ButtonSwapSprite.ButtonClickCallback clickCallback,
		string metricClickString,
		FrontEndButtonSounds clickSound)
	{
		foreach (_SelectableBtn btn in btns)
		{
			btn.spriteController.callback = clickCallback;
			btn.SetRecordMetricClick(true, metricClickString);
			btn.spriteController.m_soundToPlay = clickSound;
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
		Dictionary<int, long> scores;
		if (notification == null)
		{
			if (ClientGameManager.Get() == null)
			{
				yield break;
			}
			activeIndex = ClientGameManager.Get().ActiveFactionCompetition;
			scores = ClientGameManager.Get().FactionScores;
		}
		else
		{
			activeIndex = notification.ActiveIndex;
			scores = notification.Scores;
		}
		FactionCompetition factionCompetition = FactionWideData.Get().GetFactionCompetition(activeIndex);
		bool canViewSeasons = UISeasonsPanel.CheckSeasonsVisibility(out _);
		if (factionCompetition != null)
		{
			if (factionCompetition.Enabled && factionCompetition.ShouldShowcase && canViewSeasons)
			{
				UIManager.SetGameObjectActive(m_factionsContainer, true);
				List<UISeasonFactionEntry> entries = new List<UISeasonFactionEntry>();
				entries.AddRange(m_factionsLayout.GetComponentsInChildren<UISeasonFactionEntry>(true));
				for (int i = entries.Count - 1; i >= factionCompetition.Factions.Count; i--)
				{
					Destroy(entries[i].gameObject);
					entries.RemoveAt(i);
				}
				for (int j = entries.Count; j < factionCompetition.Factions.Count; j++)
				{
					UISeasonFactionEntry factionEntry = Instantiate(m_factionPrefab);
					Transform tr;
					(tr = factionEntry.transform).SetParent(m_factionsLayout.transform);
					tr.localScale = Vector3.one;
					tr.localPosition = Vector3.zero;
					entries.Add(factionEntry);
					m_factionsClickBox.spriteController.AddSubButton(factionEntry.m_hitbox);
					factionEntry.m_hitbox.callback = m_factionsClickBox.spriteController.callback;
				}
				yield return 0;
				List<UISeasonFactionPercentageBar> bars = new List<UISeasonFactionPercentageBar>();
				bars.AddRange(m_factionPercentContainer.GetComponentsInChildren<UISeasonFactionPercentageBar>(true));
				for (int k = bars.Count - 1; k >= factionCompetition.Factions.Count; k--)
				{
					Destroy(bars[k].gameObject);
					bars.RemoveAt(k);
				}
				for (int l = bars.Count; l < factionCompetition.Factions.Count; l++)
				{
					UISeasonFactionPercentageBar factionPercentageBar = Instantiate(m_factionPercentPrefab);
					Transform tr;
					(tr = factionPercentageBar.transform).SetParent(m_factionPercentContainer.transform);
					tr.localScale = Vector3.one;
					tr.localPosition = Vector3.zero;
					bars.Add(factionPercentageBar);
				}
				long totalScore = 0L;
				for (int m = 0; m < factionCompetition.Factions.Count; m++)
				{
					scores.TryGetValue(m, out long num);
					entries[m].Setup(factionCompetition.Factions[m], num, m + 1);
					totalScore += num;
				}
				float lastPortion = 0f;
				for (int n = 0; n < factionCompetition.Factions.Count; n++)
				{
					if (totalScore == 0L)
					{
						UIManager.SetGameObjectActive(bars[n], false);
					}
					else
					{
						scores.TryGetValue(n, out long num2);
						if (num2 > 0L)
						{
							float num3 = num2 / (float)totalScore;
							UIManager.SetGameObjectActive(bars[n], true);
							float[] rbga = FactionWideData.Get().GetRBGA(factionCompetition.Factions[n]);
							Color factionColor = new Color(rbga[0], rbga[1], rbga[2], rbga[3]);
							lastPortion = bars[n].Setup(lastPortion, num3 + lastPortion, factionColor);
						}
						else
						{
							UIManager.SetGameObjectActive(bars[n], false);
						}
					}
				}
				yield break;
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
		m_SideShowcaseMoreInfoText.text = StringUtil.TR(
			UISeasonsPanel.CheckSeasonsVisibility(out _)
				? "LandingPageChapterInfoLabel"
				: "MoreInfo",
			"NewFrontEndScene");
	}

	public void SideShowcaseMoreInfoButtonClicked(BaseEventData data)
	{
		bool areSeasonsEnabled = UISeasonsPanel.CheckSeasonsVisibility(out _);
		FrontEndFullScreenAnnouncements frontEndFullScreenAnnouncements = FrontEndFullScreenAnnouncements.Get();
		AccountComponent.UIStateIdentifier introductionScreenToDisplay = IntroductionScreenToDisplay;
		frontEndFullScreenAnnouncements.SetIntroductionVisible(introductionScreenToDisplay, areSeasonsEnabled ? 0 : 1);
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
			UIDialogPopupManager.OpenOneButtonDialog(
				StringUtil.TR("ServerIsLocked", "Global"),
				StringUtil.TR("CannotStartTutorial", "Global"),
				StringUtil.TR("Ok", "Global"));
		}
		else if (!ClientGameManager.Get().GroupInfo.InAGroup
		         && AppState.GetCurrent() != AppState_CharacterSelect.Get()
		         && !AppState_GroupCharacterSelect.Get().InQueue())
		{
			AppState_LandingPage.Get().OnTutorial1Clicked();
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
			return;
		}
		m_visible = visible;
		foreach (RectTransform container in m_containers)
		{
			UIManager.SetGameObjectActive(container, m_visible);
		}
		UIManager.Get().SetSceneVisible(GetSceneType(), visible, new SceneVisibilityParameters());
		if (visible)
		{
			UIFrontEnd.Get().m_frontEndNavPanel.SetNavButtonSelected(UIFrontEnd.Get().m_frontEndNavPanel.m_landingPageBtn);
			UpdateMatchData();
			ClientGameManager clientGameManager = ClientGameManager.Get();
			if (clientGameManager != null)
			{
				if (clientGameManager.IsConnectedToLobbyServer)
				{
					clientGameManager.SendCheckAccountStatusRequest(AppState_LandingPage.Get().HandleCheckAccountStatusResponse);
					clientGameManager.SendCheckRAFStatusRequest(false);
				}
				CheckForTrustWarEnd();
			}
			CheckTutorialButton();
			if (!UITutorialSeasonInterstitial.Get().HasBeenViewed())
			{
				PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
				SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(playerAccountData.QuestComponent.ActiveSeason);
				if (seasonTemplate != null && seasonTemplate.IsTutorial)
				{
					UITutorialSeasonInterstitial.Get().Setup(seasonTemplate, playerAccountData.QuestComponent.SeasonLevel, false);
					UITutorialSeasonInterstitial.Get().SetVisible(true);
				}
			}
		}
	}

	private void CheckForTrustWarEnd()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager == null || !clientGameManager.IsPlayerAccountDataAvailable())
		{
			return;
		}
		PersistedAccountData playerAccountData = clientGameManager.GetPlayerAccountData();
		AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.HasSeenTrustWarEndPopup;
		if (playerAccountData.AccountComponent.GetUIState(uiState) == 0 && playerAccountData.QuestComponent.ActiveSeason == 1)
		{
			ClientGameManager.Get().RequestUpdateUIState(uiState, 1, null);
			UIDialogPopupManager.OpenTrustWarEndDialog();
		}
	}

	private void CheckTutorialButton()
	{
		if (ClientGameManager.Get() != null
		    && ClientGameManager.Get().IsPlayerAccountDataAvailable()
		    && ClientGameManager.Get().GetPlayerAccountData().ExperienceComponent.EnteredTutorial == 0)
		{
			SetBtnVisible(m_TutorialBtns, true);
			SetBtnVisible(m_VideoBtns, false);
		}
		else
		{
			SetBtnVisible(m_TutorialBtns, false);
			SetBtnVisible(m_VideoBtns, true);
		}
	}

	public void UpdateMatchData()
	{
	}

	public string GrabRandomFlavorText()
	{
		string str = "<color=#ffc000>";
		int num = Mathf.FloorToInt(Random.value * m_flavorTexts.Length);
		str += m_flavorTexts[num];
		return str + "</color>";
	}

	public void ShowMOTD()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager == null)
		{
			return;
		}
		string language = HydrogenConfig.Get().Language;
		m_motd.text = clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.MOTDText, language);
		UIManager.SetGameObjectActive(m_motdBg, !m_motd.text.IsNullOrEmpty());
		SetArrayText(m_ReleaseNotesBtnDescriptions, clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.ReleaseNotesDescription, language));
		SetArrayText(m_ReleaseNotesBtnTitles, clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.ReleaseNotesHeader, language));
		SetArrayText(m_WhatsNewBtnDescriptions, clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.WhatsNewDescription, language));
		SetArrayText(m_WhatsNewBtnTitles, clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.WhatsNewHeader, language));
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
			if (!UILandingPageFullScreenMenus.Get().IsMessageContainerVisible())
			{
				UILandingPageFullScreenMenus.Get().DisplayMessage(
					StringUtil.TR("ServerIsLocked", "Global"),
					m_lockedReason,
					() => { AppState_Shutdown.Get().Enter(); });
			}
		}
		else if (UILandingPageFullScreenMenus.Get().IsMessageContainerVisible())
		{
			UILandingPageFullScreenMenus.Get().SetMessageContainerVisible(false);
		}
	}

	private void OnAccountDataUpdated(PersistedAccountData accountData)
	{
		m_currentSeason = SeasonWideData.Get().GetSeasonTemplate(accountData.QuestComponent.ActiveSeason);
		UIManager.SetGameObjectActive(m_tutorialLevelContainer, m_currentSeason != null && m_currentSeason.IsTutorial);
		bool isActiveSeason = m_currentSeason != null && !m_currentSeason.IsTutorial;
		UIManager.SetGameObjectActive(m_seasonLevelContainer, isActiveSeason);
		CheckSeasonsVisibility();
		CharacterType characterType = accountData.AccountComponent.LastCharacter;
		if (!characterType.IsValidForHumanGameplay())
		{
			characterType = CharacterType.Scoundrel;
		}
		Sprite sprite = (Sprite)Resources.Load("Characters/full_" + characterType, typeof(Sprite));
		m_characterImage.sprite = sprite != null
			? sprite
			: (Sprite)Resources.Load("Characters/full_SpaceMarine", typeof(Sprite));
		m_characterIcon.sprite = Resources.Load<Sprite>(GameWideData.Get().GetCharacterResourceLink(characterType).m_characterSelectIconResourceString);
		if (accountData.QuestComponent.SeasonExperience.ContainsKey(accountData.QuestComponent.ActiveSeason))
		{
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
			m_curLevel = m_endLevel;
			int endLevel = QuestWideData.GetEndLevel(m_currentSeason.Prerequisites, m_currentSeason.Index);
			m_tutorialLevelText.text = m_curLevel - 1 + "/" + (endLevel - 1);
			for (int i = m_tutorialLevelSliderBars.Count; i < endLevel - 1; i++)
			{
				UITutorialSeasonLevelBar seasonLevelBar = Instantiate(m_tutorialLevelBarPrefab);
				Transform tr = seasonLevelBar.transform;
				tr.SetParent(m_tutorialLevelLayout.transform);
				tr.localScale = Vector3.one;
				tr.localPosition = Vector3.zero;
				m_tutorialLevelSliderBars.Add(seasonLevelBar);
			}
			Queue<RewardUtils.RewardData> queue = new Queue<RewardUtils.RewardData>(RewardUtils.GetSeasonLevelRewards());
			List<RewardUtils.RewardData> availableSeasonEndRewards = RewardUtils.GetAvailableSeasonEndRewards(m_currentSeason);
			if (availableSeasonEndRewards.Count > 0)
			{
				queue.Enqueue(availableSeasonEndRewards[0]);
			}
			for (int i = 0; i < m_tutorialLevelSliderBars.Count; i++)
			{
				int num = i + 1;
				m_tutorialLevelSliderBars[i].SetFilled(num < m_curLevel);
				UIManager.SetGameObjectActive(m_tutorialLevelSliderBars[i], num < endLevel);
				RewardUtils.RewardData rewardData = null;
				while (queue.Count > 0 && rewardData == null)
				{
					int num2 = queue.Peek().Level - 1;
					if (num2 < num)
					{
						queue.Dequeue();
					}
					else if (num2 <= num)
					{
						rewardData = queue.Dequeue();
					}
					else
					{
						break;
					}
				}
				m_tutorialLevelSliderBars[i].SetReward(num, rewardData);
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
			m_expPerSecond = m_endLevel == m_curLevel
				? m_endExp - m_curExp
				: m_expToLevel;
			m_expPerSecond = (int)(m_expPerSecond / 1f);
			if (m_expPerSecond < 1)
			{
				m_expPerSecond = 1;
			}
			if (m_curLevel > m_endLevel)
			{
				m_curLevel = m_endLevel;
				m_curExp = m_endExp;
				m_expPerSecond = 1;
			}
			m_seasonLevelSlider.fillAmount = m_curExp / (float)m_expToLevel;
			SetupNextSeasonReward();
		}
		m_lastCharacterType = characterType;
		if (ClientGameManager.Get().IsPlayerCharacterDataAvailable(m_lastCharacterType))
		{
			OnCharacterDataUpdated(ClientGameManager.Get().GetPlayerCharacterData(m_lastCharacterType));
		}
		if (IsVisible())
		{
			CheckForTrustWarEnd();
		}
	}

	private void SetArrayText(TextMeshProUGUI[] textMeshes, string text)
	{
		foreach (TextMeshProUGUI textMesh in textMeshes)
		{
			textMesh.text = text;
		}
	}

	private void SetBtnVisible(_SelectableBtn[] btns, bool visible)
	{
		foreach (_SelectableBtn btn in btns)
		{
			UIManager.SetGameObjectActive(btn, visible);
		}
	}

	private void Update()
	{
		HYDRO10288Hack();
		bool flag = m_visible && !UIFrontendLoadingScreen.Get().IsVisible();
		if (m_landingPageIntroAnimator == null)
		{
			m_landingPageIntroAnimator = m_LandingPageOpenAnimationContainer.GetComponentInChildren<Animator>(true);
			UIManager.SetGameObjectActive(m_LandingPageOpenAnimationContainer, true);
		}

		string str = LandingPageOpenPrefixAnimName;
		UIManager.SetGameObjectActive(m_SideShowcase, flag);
		if (m_landingPageIntroAnimator != null)
		{
			UIManager.SetGameObjectActive(m_landingPageIntroAnimator, true);
			if (flag
			    && m_landingPageIntroAnimator.gameObject.activeInHierarchy
			    && !m_landingPageIntroAnimator.GetCurrentAnimatorStateInfo(0).IsName(str + "IN")
			    && !m_landingPageIntroAnimator.GetCurrentAnimatorStateInfo(0).IsName(str + "IDLE"))
			{
				UIAnimationEventManager.Get().PlayAnimation(m_landingPageIntroAnimator, str + "IN", null, string.Empty);
			}
		}
		if (m_timedMessage != null
		    && !m_timedMessageEndTime.IsNullOrEmpty()
		    && !m_timedMessageStartTime.IsNullOrEmpty())
		{
			DateTime startTime = Convert.ToDateTime(m_timedMessageStartTime);
			DateTime endTime = Convert.ToDateTime(m_timedMessageEndTime);
			ClientGameManager clientGameManager = ClientGameManager.Get();
			bool doActive = clientGameManager != null
			                && clientGameManager.PacificNow() < endTime
			                && clientGameManager.PacificNow() >= startTime;
			UIManager.SetGameObjectActive(m_timedMessage, doActive);
		}
		if (m_currentSeason == null)
		{
			Debug.LogError(GetType() + " m_currentSeason is null, please fix me");
		}
		if (m_currentSeason != null
		    && !m_currentSeason.IsTutorial
		    && m_expPerSecond != 0
		    && UIFrontEnd.Get() != null
		    && UIFrontEnd.Get().m_playerPanel != null
		    && !UIFrontEnd.Get().m_playerPanel.IsPlayingLevelUpAnim())
		{
			if (m_curLevel > m_endLevel)
			{
				m_curLevel = m_endLevel;
				SetupNextSeasonReward();
			}
			int num = (int)(m_expPerSecond * Time.deltaTime);
			if (num < 1)
			{
				num = 1;
			}
			m_curExp += num;
			if (m_curLevel == m_endLevel && m_curExp >= m_endExp)
			{
				m_curExp = m_endExp;
				m_expPerSecond = 0;
			}
			else if (m_curExp >= m_expToLevel)
			{
				m_curExp = 0;
				m_curLevel++;
				int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
				int seasonExperience = SeasonWideData.Get().GetSeasonExperience(
					activeSeason,
					ClientGameManager.Get().GetPlayerAccountData().QuestComponent.GetSeasonExperienceComponent(activeSeason).Level);
				m_expPerSecond = m_expToLevel = seasonExperience;
				SetupNextSeasonReward();
			}
			m_seasonLevelSlider.fillAmount = m_curExp / (float)m_expToLevel;
			UIManager.SetGameObjectActive(m_seasonLevelSlider, true);
			m_seasonExpAmountText.text = UIStorePanel.FormatIntToString(m_curExp) + " / " + UIStorePanel.FormatIntToString(m_expToLevel);
			m_seasonLevelText.text = m_curLevel.ToString();
		}
	}

	private void SetupNextSeasonReward()
	{
		m_playerRewards = RewardUtils.GetNextSeasonLevelRewards(m_curLevel);
		if (m_playerRewards.Count == 0)
		{
			UIManager.SetGameObjectActive(m_seasonRewardTooltipObj, false);
			UIManager.SetGameObjectActive(m_seasonRewardIconImage, false);
			UIManager.SetGameObjectActive(m_seasonNextRewardLabel, false);
			return;
		}
		UIManager.SetGameObjectActive(m_seasonRewardTooltipObj, true);
		UIManager.SetGameObjectActive(m_seasonRewardIconImage, true);
		UIManager.SetGameObjectActive(m_seasonNextRewardLabel, true);
		string text = "QuestRewards/general";
		foreach (RewardUtils.RewardData rewardData in m_playerRewards)
		{
			if (!rewardData.isRepeating
			    && rewardData.Type != RewardUtils.RewardType.Lockbox
			    && (rewardData.InventoryTemplate == null || rewardData.InventoryTemplate.Index != UILootMatrixScreen.kDefaultLootMatrix))
			{
				text = rewardData.SpritePath;
				break;
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
			foreach (RewardUtils.RewardData rewardData in RewardUtils.GetAccountLevelRewards(i))
			{
				int index = 0;
				for (int j = 0; j < m_playerRewards.Count; j++)
				{
					if (rewardData.Level <= m_playerRewards[j].Level)
					{
						index = j;
						break;
					}
				}
				m_playerRewards.Insert(index, rewardData);
			}
		}
		List<RewardUtils.RewardData> availableSeasonEndRewards = RewardUtils.GetAvailableSeasonEndRewards(m_currentSeason);
		m_playerRewards.AddRange(availableSeasonEndRewards);
		foreach (RewardUtils.RewardData playerReward in m_playerRewards)
		{
			playerReward.Level--;
		}
		RewardUtils.RewardData nextRewardData = null;
		if (availableSeasonEndRewards.Count > 0)
		{
			nextRewardData = availableSeasonEndRewards[0];
		}
		if (nextRewardData == null)
		{
			UIManager.SetGameObjectActive(m_seasonRewardTooltipObj, false);
			UIManager.SetGameObjectActive(m_seasonRewardIconImage, false);
			UIManager.SetGameObjectActive(m_seasonNextRewardLabel, false);
		}
		else
		{
			UIManager.SetGameObjectActive(m_seasonRewardTooltipObj, true);
			UIManager.SetGameObjectActive(m_seasonRewardIconImage, true);
			UIManager.SetGameObjectActive(m_seasonNextRewardLabel, true);
			m_tutorialRewardIconImage.sprite = Resources.Load<Sprite>(nextRewardData.SpritePath);
			UIManager.SetGameObjectActive(m_tutorialRewardFgImage, nextRewardData.Foreground != null);
			m_tutorialRewardFgImage.sprite = nextRewardData.Foreground;
			m_tutorialNextRewardLabel.text = nextRewardData.Name;
			m_seasonRewardTooltipObj.Refresh();
		}
	}

	public void OnCharacterDataUpdated(PersistedCharacterData newData)
	{
		if (newData == null || m_lastCharacterType != newData.CharacterType)
		{
			return;
		}
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(m_lastCharacterType);
		m_charLevelText.text = newData.ExperienceComponent.Level.ToString();
		int num = gameBalanceVars.CharacterExperienceToLevel(newData.ExperienceComponent.Level);
		m_charLevelSlider.fillAmount = newData.ExperienceComponent.XPProgressThroughLevel / (float)num;
		UIManager.SetGameObjectActive(m_charLevelSlider, true);
		m_charExpAmountText.text = newData.ExperienceComponent.XPProgressThroughLevel + " / " + num;
		m_charRewards = RewardUtils.GetCharacterRewards(characterResourceLink);
		foreach (GameBalanceVars.CharacterLevelReward characterLevelReward in gameBalanceVars.RepeatingCharacterLevelRewards)
		{
			if (characterLevelReward.charType == (int)m_lastCharacterType
			    && characterLevelReward.repeatingLevel > 0)
			{
				InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(characterLevelReward.reward.ItemTemplateId);
				m_charRewards.Add(new RewardUtils.RewardData
				{
					Amount = characterLevelReward.reward.Amount,
					Name = itemTemplate.GetDisplayName(),
					SpritePath = itemTemplate.IconPath,
					Level = characterLevelReward.startLevel,
					InventoryTemplate = itemTemplate,
					repeatLevels = characterLevelReward.repeatingLevel,
					isRepeating = true
				});
			}
		}
		m_charLevel = newData.ExperienceComponent.Level;
		RewardUtils.RewardData rewardToUseForDisplay = GetRewardToUseForDisplay(m_charRewards, m_charLevel);
		m_charRewardIconImage.sprite =
			rewardToUseForDisplay != null && !string.IsNullOrEmpty(rewardToUseForDisplay.SpritePath)
				? Resources.Load<Sprite>(rewardToUseForDisplay.SpritePath)
				: null;
		m_charRewardTooltipObj.Refresh();
	}

	private RewardUtils.RewardData GetRewardToUseForDisplay(List<RewardUtils.RewardData> possibleRewards, int curLevel)
	{
		List<RewardUtils.RewardData> nextRewards = new List<RewardUtils.RewardData>();
		int minRewardLevel = -1;
		
		foreach (RewardUtils.RewardData reward in possibleRewards)
		{
			if (reward.Level <= curLevel && !reward.isRepeating)
			{
				continue;
			}
			if (nextRewards.Count == 0)
			{
				nextRewards.Add(reward);
				minRewardLevel = reward.Level;
			}
			else if (minRewardLevel == reward.Level)
			{
				nextRewards.Add(reward);
			}
		}
		foreach (RewardUtils.RewardType rewardType in GameBalanceVars.Get().RewardDisplayPriorityOrder)
		{
			foreach (RewardUtils.RewardData reward in nextRewards)
			{
				if (rewardType == reward.Type)
				{
					return reward;
				}
			}
		}
		if (nextRewards.Count > 0)
		{
			return nextRewards[0];
		}
		return null;
	}
}
