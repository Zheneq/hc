using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using LobbyGameClientMessages;
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
		return UILandingPageScreen.s_instance;
	}

	public override void Awake()
	{
		UILandingPageScreen.s_instance = this;
		base.Awake();
	}

	private void Start()
	{
		this.m_inCustomGame = false;
		this.m_ForumsBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ForumsBtnClicked);
		this.m_factionsMoreInfoButton.callback = new _ButtonSwapSprite.ButtonClickCallback(this.FactionsMoreInfoClicked);
		this.m_factionsClickBox.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.FactionsAreaClicked);
		this.m_LootMatrixButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.LootMatrixButtonClicked);
		this.m_SideShowcaseMoreInfoButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.SideShowcaseMoreInfoButtonClicked);
		this.m_seasonRewardTooltipObj.Setup(TooltipType.RewardList, delegate(UITooltipBase tooltip)
		{
			if (this.m_playerRewards != null)
			{
				if (this.m_playerRewards.Count != 0)
				{
					UIRewardListTooltip uirewardListTooltip = tooltip as UIRewardListTooltip;
					uirewardListTooltip.Setup(this.m_playerRewards, this.m_curLevel + 1, UIRewardListTooltip.RewardsType.Seasons, false);
					return true;
				}
			}
			return false;
		}, null);
		this.m_charRewardTooltipObj.Setup(TooltipType.RewardList, delegate(UITooltipBase tooltip)
		{
			if (this.m_charRewards != null)
			{
				if (this.m_charRewards.Count != 0)
				{
					UIRewardListTooltip uirewardListTooltip = tooltip as UIRewardListTooltip;
					uirewardListTooltip.Setup(this.m_charRewards, this.m_charLevel, UIRewardListTooltip.RewardsType.Character, false);
					return true;
				}
			}
			return false;
		}, null);
		this.m_tutorialRewardTooltipObj.Setup(TooltipType.RewardList, delegate(UITooltipBase tooltip)
		{
			if (this.m_playerRewards != null)
			{
				if (this.m_playerRewards.Count != 0)
				{
					UIRewardListTooltip uirewardListTooltip = tooltip as UIRewardListTooltip;
					uirewardListTooltip.Setup(this.m_playerRewards, this.m_curLevel - 1, UIRewardListTooltip.RewardsType.Tutorial, false);
					return true;
				}
			}
			return false;
		}, null);
		this.m_charMoreInfoBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.CharMoreInfoClicked);
		TextMeshProUGUI[] componentsInChildren = this.m_ForumsBtn.GetComponentsInChildren<TextMeshProUGUI>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].text = "FORUMS";
		}
		this.m_ForumsBtn.SetRecordMetricClick(true, "CLICK: Landing Page Forums Button");
		this.m_ForumsBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.GenericSmall;
		if (this.m_MoreInfoBtn != null)
		{
			this.m_MoreInfoBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.MoreInfoClicked);
			this.m_MoreInfoBtn.SetRecordMetricClick(true, "CLICK: Landing Page MoreInfo Button");
			this.m_MoreInfoBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.GenericSmall;
		}
		this.SetUpButtons(this.m_NewsBtns, new _ButtonSwapSprite.ButtonClickCallback(this.NewsBtnClicked), "CLICK: Landing Page News Button", FrontEndButtonSounds.CharacterSelectModAdd);
		this.SetUpButtons(this.m_PatchNotesBtns, new _ButtonSwapSprite.ButtonClickCallback(this.PatchNotesBtnClicked), "CLICK: Landing Page Patch Notes Button", FrontEndButtonSounds.CharacterSelectModAdd);
		this.SetUpButtons(this.m_VideoBtns, new _ButtonSwapSprite.ButtonClickCallback(this.VideoBtnClicked), "CLICK: Landing Page Video Button", FrontEndButtonSounds.CharacterSelectModAdd);
		this.SetUpButtons(this.m_TutorialBtns, new _ButtonSwapSprite.ButtonClickCallback(this.TutorialBtnClicked), "CLICK: Landing Page Tutorial Button", FrontEndButtonSounds.CharacterSelectModAdd);
		this.SetUpButtons(this.m_FeedbackBtns, new _ButtonSwapSprite.ButtonClickCallback(this.FeedbackBtnClicked), "CLICK: Landing Page Feedback Button", FrontEndButtonSounds.CharacterSelectModAdd);
		this.OnFactionCompetitionNotification(null);
		this.m_tutorialLevelSliderBars = new List<UITutorialSeasonLevelBar>();
		this.m_tutorialLevelSliderBars.AddRange(this.m_tutorialLevelLayout.GetComponentsInChildren<UITutorialSeasonLevelBar>());
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager clientGameManager = ClientGameManager.Get();
			clientGameManager.OnAccountDataUpdated += this.OnAccountDataUpdated;
			clientGameManager.OnFactionCompetitionNotification += this.OnFactionCompetitionNotification;
			clientGameManager.OnLobbyGameplayOverridesChange += this.HandleLobbyGameplayOverridesChange;
			clientGameManager.OnCharacterDataUpdated += this.OnCharacterDataUpdated;
			if (clientGameManager.IsPlayerAccountDataAvailable())
			{
				this.OnAccountDataUpdated(clientGameManager.GetPlayerAccountData());
			}
		}
		this.m_isLocked = false;
		this.m_lockedReason = string.Empty;
		this.m_motd.text = string.Empty;
		UIManager.SetGameObjectActive(this.m_motdBg, false, null);
		if (AppState_LandingPage.Get() != null && AppState_LandingPage.Get().ReceivedLobbyStatusInfo)
		{
			this.ShowMOTD();
			this.SetServerIsLocked(ClientGameManager.Get().IsServerLocked);
			if (UIFrontEnd.Get().m_frontEndNavPanel != null)
			{
				UIFrontEnd.Get().m_frontEndNavPanel.SetShopVisible(GameManager.Get().GameplayOverrides.EnableShop);
				UIFrontEnd.Get().m_frontEndNavPanel.CheckSeasonsVisibility();
			}
		}
		UIManager.SetGameObjectActive(this.m_timedMessage, false, null);
		foreach (UISeasonFactionEntry uiseasonFactionEntry in this.m_factionsLayout.GetComponentsInChildren<UISeasonFactionEntry>(true))
		{
			this.m_factionsClickBox.spriteController.AddSubButton(uiseasonFactionEntry.m_hitbox);
			uiseasonFactionEntry.m_hitbox.callback = this.m_factionsClickBox.spriteController.callback;
		}
	}

	private void HYDRO10288Hack()
	{
		if (!this.m_hackInitialized)
		{
			if (!(ClientGameManager.Get() == null))
			{
				if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
				{
					this.OnFactionCompetitionNotification(null);
					this.OnAccountDataUpdated(ClientGameManager.Get().GetPlayerAccountData());
					this.m_hackInitialized = true;
					return;
				}
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
	}

	public override SceneType GetSceneType()
	{
		return SceneType.LandingPage;
	}

	private void OnDestroy()
	{
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnAccountDataUpdated -= this.OnAccountDataUpdated;
			ClientGameManager.Get().OnFactionCompetitionNotification -= this.OnFactionCompetitionNotification;
			ClientGameManager.Get().OnLobbyGameplayOverridesChange -= this.HandleLobbyGameplayOverridesChange;
			ClientGameManager.Get().OnCharacterDataUpdated -= this.OnCharacterDataUpdated;
		}
	}

	private void HandleLobbyGameplayOverridesChange(LobbyGameplayOverrides gameplayOverrides)
	{
		this.OnFactionCompetitionNotification(null);
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
		FactionCompetition factionCompetiton = FactionWideData.Get().GetFactionCompetition(activeIndex);
		SeasonLockoutReason reason;
		bool canViewSeasons = UISeasonsPanel.CheckSeasonsVisibility(out reason);
		if (factionCompetiton != null)
		{
			if (factionCompetiton.Enabled && factionCompetiton.ShouldShowcase && canViewSeasons)
			{
				UIManager.SetGameObjectActive(this.m_factionsContainer, true, null);
				List<UISeasonFactionEntry> entries = new List<UISeasonFactionEntry>();
				entries.AddRange(this.m_factionsLayout.GetComponentsInChildren<UISeasonFactionEntry>(true));
				for (int i = entries.Count - 1; i >= factionCompetiton.Factions.Count; i--)
				{
					UnityEngine.Object.Destroy(entries[i].gameObject);
					entries.RemoveAt(i);
				}
				for (int j = entries.Count; j < factionCompetiton.Factions.Count; j++)
				{
					UISeasonFactionEntry uiseasonFactionEntry = UnityEngine.Object.Instantiate<UISeasonFactionEntry>(this.m_factionPrefab);
					uiseasonFactionEntry.transform.SetParent(this.m_factionsLayout.transform);
					uiseasonFactionEntry.transform.localScale = Vector3.one;
					uiseasonFactionEntry.transform.localPosition = Vector3.zero;
					entries.Add(uiseasonFactionEntry);
					this.m_factionsClickBox.spriteController.AddSubButton(uiseasonFactionEntry.m_hitbox);
					uiseasonFactionEntry.m_hitbox.callback = this.m_factionsClickBox.spriteController.callback;
				}
				yield return 0;
				List<UISeasonFactionPercentageBar> bars = new List<UISeasonFactionPercentageBar>();
				bars.AddRange(this.m_factionPercentContainer.GetComponentsInChildren<UISeasonFactionPercentageBar>(true));
				for (int k = bars.Count - 1; k >= factionCompetiton.Factions.Count; k--)
				{
					UnityEngine.Object.Destroy(bars[k].gameObject);
					bars.RemoveAt(k);
				}
				for (int l = bars.Count; l < factionCompetiton.Factions.Count; l++)
				{
					UISeasonFactionPercentageBar uiseasonFactionPercentageBar = UnityEngine.Object.Instantiate<UISeasonFactionPercentageBar>(this.m_factionPercentPrefab);
					uiseasonFactionPercentageBar.transform.SetParent(this.m_factionPercentContainer.transform);
					uiseasonFactionPercentageBar.transform.localScale = Vector3.one;
					uiseasonFactionPercentageBar.transform.localPosition = Vector3.zero;
					bars.Add(uiseasonFactionPercentageBar);
				}
				long totalScore = 0L;
				for (int m = 0; m < factionCompetiton.Factions.Count; m++)
				{
					long num;
					scores.TryGetValue(m, out num);
					entries[m].Setup(factionCompetiton.Factions[m], num, m + 1);
					totalScore += num;
				}
				float lastPortion = 0f;
				for (int n = 0; n < factionCompetiton.Factions.Count; n++)
				{
					if (totalScore == 0L)
					{
						UIManager.SetGameObjectActive(bars[n], false, null);
					}
					else
					{
						long num2;
						scores.TryGetValue(n, out num2);
						if (num2 > 0L)
						{
							float num3 = (float)num2 / (float)totalScore;
							UIManager.SetGameObjectActive(bars[n], true, null);
							float[] rbga = FactionWideData.Get().GetRBGA(factionCompetiton.Factions[n]);
							Color factionColor = new Color(rbga[0], rbga[1], rbga[2], rbga[3]);
							lastPortion = bars[n].Setup(lastPortion, num3 + lastPortion, factionColor);
						}
						else
						{
							UIManager.SetGameObjectActive(bars[n], false, null);
						}
					}
				}
				yield break;
			}
		}
		UIManager.SetGameObjectActive(this.m_factionsContainer, false, null);
		yield break;
	}

	private void OnFactionCompetitionNotification(FactionCompetitionNotification notification)
	{
		base.StartCoroutine(this.SetupFactionCompetitionBars(notification));
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
		this.CharacterInfoClicked = new CharacterType?(this.m_CharMoreInfoBtnCharacterTypeLink);
		UICharacterScreen.CharacterSelectSceneStateParameters characterSelectSceneStateParameters = new UICharacterScreen.CharacterSelectSceneStateParameters();
		characterSelectSceneStateParameters.ClientRequestToServerSelectCharacter = new CharacterType?(this.m_CharMoreInfoBtnCharacterTypeLink);
		characterSelectSceneStateParameters.SideButtonsVisible = new bool?(true);
		UIManager.Get().HandleNewSceneStateParameter(characterSelectSceneStateParameters);
		UIFrontEnd.Get().m_frontEndNavPanel.PlayBtnClicked(null);
		UICharacterSelectCharacterSettingsPanel.Get().SetVisible(true, UICharacterSelectCharacterSettingsPanel.TabPanel.General);
	}

	public void CheckSeasonsVisibility()
	{
		SeasonLockoutReason seasonLockoutReason;
		bool flag = UISeasonsPanel.CheckSeasonsVisibility(out seasonLockoutReason);
		if (flag)
		{
			this.m_SideShowcaseMoreInfoText.text = StringUtil.TR("LandingPageChapterInfoLabel", "NewFrontEndScene");
		}
		else
		{
			this.m_SideShowcaseMoreInfoText.text = StringUtil.TR("MoreInfo", "NewFrontEndScene");
		}
	}

	public void SideShowcaseMoreInfoButtonClicked(BaseEventData data)
	{
		SeasonLockoutReason seasonLockoutReason;
		bool flag = UISeasonsPanel.CheckSeasonsVisibility(out seasonLockoutReason);
		FrontEndFullScreenAnnouncements frontEndFullScreenAnnouncements = FrontEndFullScreenAnnouncements.Get();
		AccountComponent.UIStateIdentifier introductionScreenToDisplay = this.IntroductionScreenToDisplay;
		int pageNum;
		if (flag)
		{
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
			UIDialogPopupManager.OpenOneButtonDialog(StringUtil.TR("ServerIsLocked", "Global"), StringUtil.TR("CannotStartTutorial", "Global"), StringUtil.TR("Ok", "Global"), null, -1, false);
		}
		else
		{
			bool flag = true;
			if (ClientGameManager.Get().GroupInfo.InAGroup)
			{
				flag = false;
			}
			if (AppState.GetCurrent() == AppState_CharacterSelect.Get())
			{
				flag = false;
			}
			if (AppState_GroupCharacterSelect.Get().InQueue())
			{
				flag = false;
			}
			if (flag)
			{
				AppState_LandingPage.Get().OnTutorial1Clicked();
			}
		}
	}

	public void QuickPlayButtonClicked(BaseEventData data)
	{
		if (!this.m_isLocked)
		{
			UIFrontEnd.PlaySound(FrontEndButtonSounds.Generic);
			AppState_LandingPage.Get().OnQuickPlayClicked();
		}
	}

	public bool IsVisible()
	{
		return this.m_visible;
	}

	public void SetVisible(bool visible)
	{
		if (this.m_visible == visible)
		{
			return;
		}
		this.m_visible = visible;
		for (int i = 0; i < this.m_containers.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_containers[i], this.m_visible, null);
		}
		UIManager.Get().SetSceneVisible(this.GetSceneType(), visible, new SceneVisibilityParameters());
		if (visible)
		{
			UIFrontEnd.Get().m_frontEndNavPanel.SetNavButtonSelected(UIFrontEnd.Get().m_frontEndNavPanel.m_landingPageBtn);
			this.UpdateMatchData();
			ClientGameManager clientGameManager = ClientGameManager.Get();
			if (clientGameManager != null)
			{
				if (clientGameManager.IsConnectedToLobbyServer)
				{
					clientGameManager.SendCheckAccountStatusRequest(new Action<CheckAccountStatusResponse>(AppState_LandingPage.Get().HandleCheckAccountStatusResponse));
					clientGameManager.SendCheckRAFStatusRequest(false, null);
				}
				this.CheckForTrustWarEnd();
			}
			this.CheckTutorialButton();
			if (!UITutorialSeasonInterstitial.Get().HasBeenViewed())
			{
				PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
				SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(playerAccountData.QuestComponent.ActiveSeason);
				if (seasonTemplate != null)
				{
					if (seasonTemplate.IsTutorial)
					{
						UITutorialSeasonInterstitial.Get().Setup(seasonTemplate, playerAccountData.QuestComponent.SeasonLevel, false);
						UITutorialSeasonInterstitial.Get().SetVisible(true);
					}
				}
			}
		}
	}

	private void CheckForTrustWarEnd()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (!(clientGameManager == null))
		{
			if (clientGameManager.IsPlayerAccountDataAvailable())
			{
				PersistedAccountData playerAccountData = clientGameManager.GetPlayerAccountData();
				AccountComponent.UIStateIdentifier uiState = AccountComponent.UIStateIdentifier.HasSeenTrustWarEndPopup;
				if (playerAccountData.AccountComponent.GetUIState(uiState) == 0 && playerAccountData.QuestComponent.ActiveSeason == 1)
				{
					ClientGameManager.Get().RequestUpdateUIState(uiState, 1, null);
					UIDialogPopupManager.OpenTrustWarEndDialog();
				}
				return;
			}
		}
	}

	private void CheckTutorialButton()
	{
		if (ClientGameManager.Get() != null)
		{
			if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
			{
				if (ClientGameManager.Get().GetPlayerAccountData().ExperienceComponent.EnteredTutorial == 0)
				{
					this.SetBtnVisible(this.m_TutorialBtns, true);
					this.SetBtnVisible(this.m_VideoBtns, false);
					return;
				}
			}
		}
		this.SetBtnVisible(this.m_TutorialBtns, false);
		this.SetBtnVisible(this.m_VideoBtns, true);
	}

	public void UpdateMatchData()
	{
	}

	public string GrabRandomFlavorText()
	{
		string str = "<color=#ffc000>";
		int num = Mathf.FloorToInt(UnityEngine.Random.value * (float)this.m_flavorTexts.Length);
		str += this.m_flavorTexts[num];
		return new StringBuilder().Append(str).Append("</color>").ToString();
	}

	public void ShowMOTD()
	{
		ClientGameManager clientGameManager = ClientGameManager.Get();
		if (clientGameManager != null)
		{
			string language = HydrogenConfig.Get().Language;
			this.m_motd.text = clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.MOTDText, language);
			UIManager.SetGameObjectActive(this.m_motdBg, !this.m_motd.text.IsNullOrEmpty(), null);
			this.SetArrayText(this.m_ReleaseNotesBtnDescriptions, clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.ReleaseNotesDescription, language));
			this.SetArrayText(this.m_ReleaseNotesBtnTitles, clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.ReleaseNotesHeader, language));
			this.SetArrayText(this.m_WhatsNewBtnDescriptions, clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.WhatsNewDescription, language));
			this.SetArrayText(this.m_WhatsNewBtnTitles, clientGameManager.ServerMessageOverrides.GetValueOrDefault(ServerMessageType.WhatsNewHeader, language));
		}
	}

	public void SetServerIsLocked(bool isLocked)
	{
		this.m_isLocked = isLocked;
		string language = HydrogenConfig.Get().Language;
		this.m_lockedReason = ClientGameManager.Get().ServerMessageOverrides.GetValueOrDefault(ServerMessageType.LockScreenText, language);
		this.UpdateUIForAccessChange();
		if (UIStorePanel.Get() != null)
		{
			UIStorePanel.Get().ClosePurchaseDialog();
		}
	}

	public void UpdateUIForAccessChange()
	{
		if (this.m_isLocked)
		{
			if (!UILandingPageFullScreenMenus.Get().IsMessageContainerVisible())
			{
				UILandingPageFullScreenMenus uilandingPageFullScreenMenus = UILandingPageFullScreenMenus.Get();
				string title = StringUtil.TR("ServerIsLocked", "Global");
				string lockedReason = this.m_lockedReason;
				
				uilandingPageFullScreenMenus.DisplayMessage(title, lockedReason, delegate()
					{
						AppState_Shutdown.Get().Enter();
					});
			}
		}
		else if (UILandingPageFullScreenMenus.Get().IsMessageContainerVisible())
		{
			UILandingPageFullScreenMenus.Get().SetMessageContainerVisible(false);
		}
	}

	private void OnAccountDataUpdated(PersistedAccountData accountData)
	{
		this.m_currentSeason = SeasonWideData.Get().GetSeasonTemplate(accountData.QuestComponent.ActiveSeason);
		UIManager.SetGameObjectActive(this.m_tutorialLevelContainer, this.m_currentSeason != null && this.m_currentSeason.IsTutorial, null);
		Component seasonLevelContainer = this.m_seasonLevelContainer;
		bool doActive;
		if (this.m_currentSeason != null)
		{
			doActive = !this.m_currentSeason.IsTutorial;
		}
		else
		{
			doActive = false;
		}
		UIManager.SetGameObjectActive(seasonLevelContainer, doActive, null);
		this.CheckSeasonsVisibility();
		CharacterType characterType = accountData.AccountComponent.LastCharacter;
		if (!characterType.IsValidForHumanGameplay())
		{
			characterType = CharacterType.Scoundrel;
		}
		Sprite sprite = (Sprite)Resources.Load(new StringBuilder().Append("Characters/full_").Append(characterType).ToString(), typeof(Sprite));
		if (sprite != null)
		{
			this.m_characterImage.sprite = sprite;
		}
		else
		{
			this.m_characterImage.sprite = (Sprite)Resources.Load("Characters/full_SpaceMarine", typeof(Sprite));
		}
		this.m_characterIcon.sprite = Resources.Load<Sprite>(GameWideData.Get().GetCharacterResourceLink(characterType).m_characterSelectIconResourceString);
		if (accountData.QuestComponent.SeasonExperience.ContainsKey(accountData.QuestComponent.ActiveSeason))
		{
			this.m_endLevel = 0;
			this.m_endLevel = accountData.QuestComponent.SeasonExperience[accountData.QuestComponent.ActiveSeason].Level;
			this.m_endExp = accountData.QuestComponent.SeasonExperience[accountData.QuestComponent.ActiveSeason].XPProgressThroughLevel;
		}
		else
		{
			this.m_endLevel = 1;
			this.m_endExp = 0;
		}
		if (this.m_curLevel < 0)
		{
			this.m_curLevel = this.m_endLevel;
			this.m_curExp = this.m_endExp;
		}
		this.m_expPerSecond = 0;
		if (this.m_currentSeason != null && this.m_currentSeason.IsTutorial)
		{
			this.m_curLevel = this.m_endLevel;
			int endLevel = QuestWideData.GetEndLevel(this.m_currentSeason.Prerequisites, this.m_currentSeason.Index);
			this.m_tutorialLevelText.text = new StringBuilder().Append(this.m_curLevel - 1).Append("/").Append(endLevel - 1).ToString();
			for (int i = this.m_tutorialLevelSliderBars.Count; i < endLevel - 1; i++)
			{
				UITutorialSeasonLevelBar uitutorialSeasonLevelBar = UnityEngine.Object.Instantiate<UITutorialSeasonLevelBar>(this.m_tutorialLevelBarPrefab);
				uitutorialSeasonLevelBar.transform.SetParent(this.m_tutorialLevelLayout.transform);
				uitutorialSeasonLevelBar.transform.localScale = Vector3.one;
				uitutorialSeasonLevelBar.transform.localPosition = Vector3.zero;
				this.m_tutorialLevelSliderBars.Add(uitutorialSeasonLevelBar);
			}
			Queue<RewardUtils.RewardData> queue = new Queue<RewardUtils.RewardData>(RewardUtils.GetSeasonLevelRewards(-1));
			List<RewardUtils.RewardData> availableSeasonEndRewards = RewardUtils.GetAvailableSeasonEndRewards(this.m_currentSeason);
			if (availableSeasonEndRewards.Count > 0)
			{
				queue.Enqueue(availableSeasonEndRewards[0]);
			}
			for (int j = 0; j < this.m_tutorialLevelSliderBars.Count; j++)
			{
				int num = j + 1;
				this.m_tutorialLevelSliderBars[j].SetFilled(num < this.m_curLevel);
				UIManager.SetGameObjectActive(this.m_tutorialLevelSliderBars[j], num < endLevel, null);
				RewardUtils.RewardData rewardData = null;
				while (queue.Count > 0 && rewardData == null)
				{
					int num2 = queue.Peek().Level - 1;
					if (num2 < num)
					{
						queue.Dequeue();
					}
					else
					{
						if (num2 > num)
						{
							break;
						}
						rewardData = queue.Dequeue();
					}
				}
				this.m_tutorialLevelSliderBars[j].SetReward(num, rewardData);
			}
			this.SetupTutorialRewards();
		}
		else
		{
			try
			{
				this.m_expToLevel = SeasonWideData.Get().GetSeasonExperience(accountData.QuestComponent.ActiveSeason, this.m_curLevel);
			}
			catch (ArgumentException)
			{
				this.m_expToLevel = 0;
			}
			if (this.m_endLevel == this.m_curLevel)
			{
				this.m_expPerSecond = this.m_endExp - this.m_curExp;
			}
			else
			{
				this.m_expPerSecond = this.m_expToLevel;
			}
			this.m_expPerSecond = (int)((float)this.m_expPerSecond / 1f);
			if (this.m_expPerSecond < 1)
			{
				this.m_expPerSecond = 1;
			}
			if (this.m_curLevel > this.m_endLevel)
			{
				this.m_curLevel = this.m_endLevel;
				this.m_curExp = this.m_endExp;
				this.m_expPerSecond = 1;
			}
			this.m_seasonLevelSlider.fillAmount = (float)this.m_curExp / (float)this.m_expToLevel;
			this.SetupNextSeasonReward();
		}
		this.m_lastCharacterType = characterType;
		if (ClientGameManager.Get().IsPlayerCharacterDataAvailable(this.m_lastCharacterType))
		{
			this.OnCharacterDataUpdated(ClientGameManager.Get().GetPlayerCharacterData(this.m_lastCharacterType));
		}
		if (this.IsVisible())
		{
			this.CheckForTrustWarEnd();
		}
	}

	private void SetArrayText(TextMeshProUGUI[] textMeshes, string text)
	{
		for (int i = 0; i < textMeshes.Length; i++)
		{
			textMeshes[i].text = text;
		}
	}

	private void SetBtnVisible(_SelectableBtn[] btns, bool visible)
	{
		for (int i = 0; i < btns.Length; i++)
		{
			UIManager.SetGameObjectActive(btns[i], visible, null);
		}
	}

	private void Update()
	{
		this.HYDRO10288Hack();
		bool flag = false;
		if (this.m_visible)
		{
			if (!UIFrontendLoadingScreen.Get().IsVisible())
			{
				flag = true;
			}
		}
		if (this.m_landingPageIntroAnimator == null)
		{
			this.m_landingPageIntroAnimator = this.m_LandingPageOpenAnimationContainer.GetComponentInChildren<Animator>(true);
			UIManager.SetGameObjectActive(this.m_LandingPageOpenAnimationContainer, true, null);
		}
		string str = string.Empty;
		str = this.LandingPageOpenPrefixAnimName;
		UIManager.SetGameObjectActive(this.m_SideShowcase, flag, null);
		if (this.m_landingPageIntroAnimator != null)
		{
			UIManager.SetGameObjectActive(this.m_landingPageIntroAnimator, true, null);
			if (flag && this.m_landingPageIntroAnimator.gameObject.activeInHierarchy)
			{
				if (!this.m_landingPageIntroAnimator.GetCurrentAnimatorStateInfo(0).IsName(new StringBuilder().Append(str).Append("IN").ToString()))
				{
					if (!this.m_landingPageIntroAnimator.GetCurrentAnimatorStateInfo(0).IsName(new StringBuilder().Append(str).Append("IDLE").ToString()))
					{
						UIAnimationEventManager.Get().PlayAnimation(this.m_landingPageIntroAnimator, new StringBuilder().Append(str).Append("IN").ToString(), null, string.Empty, 0, 0f, true, false, null, null);
					}
				}
			}
		}
		if (this.m_timedMessage != null)
		{
			if (!this.m_timedMessageEndTime.IsNullOrEmpty())
			{
				if (!this.m_timedMessageStartTime.IsNullOrEmpty())
				{
					DateTime t = Convert.ToDateTime(this.m_timedMessageStartTime);
					DateTime t2 = Convert.ToDateTime(this.m_timedMessageEndTime);
					ClientGameManager clientGameManager = ClientGameManager.Get();
					Component timedMessage = this.m_timedMessage;
					bool doActive;
					if (clientGameManager != null && clientGameManager.PacificNow() < t2)
					{
						doActive = (clientGameManager.PacificNow() >= t);
					}
					else
					{
						doActive = false;
					}
					UIManager.SetGameObjectActive(timedMessage, doActive, null);
				}
			}
		}
		if (this.m_currentSeason == null)
		{
			Debug.LogError(new StringBuilder().Append(base.GetType()).Append(" m_currentSeason is null, please fix me").ToString());
		}
		if (this.m_currentSeason != null)
		{
			if (!this.m_currentSeason.IsTutorial)
			{
				if (this.m_expPerSecond != 0)
				{
					if (!(UIFrontEnd.Get() == null))
					{
						if (!(UIFrontEnd.Get().m_playerPanel == null))
						{
							if (!UIFrontEnd.Get().m_playerPanel.IsPlayingLevelUpAnim())
							{
								if (this.m_curLevel > this.m_endLevel)
								{
									this.m_curLevel = this.m_endLevel;
									this.SetupNextSeasonReward();
								}
								int num = (int)((float)this.m_expPerSecond * Time.deltaTime);
								if (num < 1)
								{
									num = 1;
								}
								this.m_curExp += num;
								if (this.m_curLevel == this.m_endLevel)
								{
									if (this.m_curExp >= this.m_endExp)
									{
										this.m_curExp = this.m_endExp;
										this.m_expPerSecond = 0;
										goto IL_408;
									}
								}
								if (this.m_curExp >= this.m_expToLevel)
								{
									this.m_curExp = 0;
									this.m_curLevel++;
									int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
									int seasonExperience = SeasonWideData.Get().GetSeasonExperience(activeSeason, ClientGameManager.Get().GetPlayerAccountData().QuestComponent.GetSeasonExperienceComponent(activeSeason).Level);
									this.m_expPerSecond = (this.m_expToLevel = seasonExperience);
									this.SetupNextSeasonReward();
								}
								IL_408:
								this.m_seasonLevelSlider.fillAmount = (float)this.m_curExp / (float)this.m_expToLevel;
								UIManager.SetGameObjectActive(this.m_seasonLevelSlider, true, null);
								this.m_seasonExpAmountText.text = new StringBuilder().Append(UIStorePanel.FormatIntToString(this.m_curExp, false)).Append(" / ").Append(UIStorePanel.FormatIntToString(this.m_expToLevel, false)).ToString();
								this.m_seasonLevelText.text = this.m_curLevel.ToString();
								return;
							}
						}
					}
				}
				return;
			}
		}
	}

	private void SetupNextSeasonReward()
	{
		this.m_playerRewards = RewardUtils.GetNextSeasonLevelRewards(this.m_curLevel);
		if (this.m_playerRewards.Count == 0)
		{
			UIManager.SetGameObjectActive(this.m_seasonRewardTooltipObj, false, null);
			UIManager.SetGameObjectActive(this.m_seasonRewardIconImage, false, null);
			UIManager.SetGameObjectActive(this.m_seasonNextRewardLabel, false, null);
			return;
		}
		UIManager.SetGameObjectActive(this.m_seasonRewardTooltipObj, true, null);
		UIManager.SetGameObjectActive(this.m_seasonRewardIconImage, true, null);
		UIManager.SetGameObjectActive(this.m_seasonNextRewardLabel, true, null);
		string text = "QuestRewards/general";
		using (List<RewardUtils.RewardData>.Enumerator enumerator = this.m_playerRewards.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				RewardUtils.RewardData rewardData = enumerator.Current;
				if (!rewardData.isRepeating && rewardData.Type != RewardUtils.RewardType.Lockbox)
				{
					if (rewardData.InventoryTemplate != null)
					{
						if (rewardData.InventoryTemplate.Index == 0x203)
						{
							continue;
						}
					}
					text = rewardData.SpritePath;
					goto IL_110;
				}
			}
		}
		IL_110:
		if (text == "QuestRewards/general")
		{
			text = this.m_playerRewards[0].SpritePath;
		}
		this.m_seasonRewardIconImage.sprite = (Sprite)Resources.Load(text, typeof(Sprite));
		this.m_seasonRewardTooltipObj.Refresh();
	}

	private void SetupTutorialRewards()
	{
		this.m_playerRewards = RewardUtils.GetSeasonLevelRewards(-1);
		for (int i = 0; i < QuestWideData.GetEndLevel(this.m_currentSeason.Prerequisites, this.m_currentSeason.Index); i++)
		{
			using (List<RewardUtils.RewardData>.Enumerator enumerator = RewardUtils.GetAccountLevelRewards(i).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					RewardUtils.RewardData rewardData = enumerator.Current;
					int index = 0;
					for (int j = 0; j < this.m_playerRewards.Count; j++)
					{
						if (rewardData.Level <= this.m_playerRewards[j].Level)
						{
							index = j;
							break;
						}
					}
					this.m_playerRewards.Insert(index, rewardData);
				}
			}
		}
		List<RewardUtils.RewardData> availableSeasonEndRewards = RewardUtils.GetAvailableSeasonEndRewards(this.m_currentSeason);
		this.m_playerRewards.AddRange(availableSeasonEndRewards);
		for (int k = 0; k < this.m_playerRewards.Count; k++)
		{
			this.m_playerRewards[k].Level--;
		}
		RewardUtils.RewardData rewardData2 = null;
		if (availableSeasonEndRewards.Count > 0)
		{
			rewardData2 = availableSeasonEndRewards[0];
		}
		if (rewardData2 == null)
		{
			UIManager.SetGameObjectActive(this.m_seasonRewardTooltipObj, false, null);
			UIManager.SetGameObjectActive(this.m_seasonRewardIconImage, false, null);
			UIManager.SetGameObjectActive(this.m_seasonNextRewardLabel, false, null);
			return;
		}
		UIManager.SetGameObjectActive(this.m_seasonRewardTooltipObj, true, null);
		UIManager.SetGameObjectActive(this.m_seasonRewardIconImage, true, null);
		UIManager.SetGameObjectActive(this.m_seasonNextRewardLabel, true, null);
		this.m_tutorialRewardIconImage.sprite = Resources.Load<Sprite>(rewardData2.SpritePath);
		UIManager.SetGameObjectActive(this.m_tutorialRewardFgImage, rewardData2.Foreground != null, null);
		this.m_tutorialRewardFgImage.sprite = rewardData2.Foreground;
		this.m_tutorialNextRewardLabel.text = rewardData2.Name;
		this.m_seasonRewardTooltipObj.Refresh();
	}

	public void OnCharacterDataUpdated(PersistedCharacterData newData)
	{
		if (newData == null)
		{
			return;
		}
		if (this.m_lastCharacterType != newData.CharacterType)
		{
			return;
		}
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(this.m_lastCharacterType);
		this.m_charLevelText.text = newData.ExperienceComponent.Level.ToString();
		int num = gameBalanceVars.CharacterExperienceToLevel(newData.ExperienceComponent.Level);
		this.m_charLevelSlider.fillAmount = (float)newData.ExperienceComponent.XPProgressThroughLevel / (float)num;
		UIManager.SetGameObjectActive(this.m_charLevelSlider, true, null);
		this.m_charExpAmountText.text = new StringBuilder().Append(newData.ExperienceComponent.XPProgressThroughLevel).Append(" / ").Append(num).ToString();
		this.m_charRewards = RewardUtils.GetCharacterRewards(characterResourceLink, null);
		for (int i = 0; i < gameBalanceVars.RepeatingCharacterLevelRewards.Length; i++)
		{
			if (gameBalanceVars.RepeatingCharacterLevelRewards[i].charType == (int)this.m_lastCharacterType && gameBalanceVars.RepeatingCharacterLevelRewards[i].repeatingLevel > 0)
			{
				RewardUtils.RewardData rewardData = new RewardUtils.RewardData();
				rewardData.Amount = gameBalanceVars.RepeatingCharacterLevelRewards[i].reward.Amount;
				InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(gameBalanceVars.RepeatingCharacterLevelRewards[i].reward.ItemTemplateId);
				rewardData.Name = itemTemplate.GetDisplayName();
				rewardData.SpritePath = itemTemplate.IconPath;
				rewardData.Level = gameBalanceVars.RepeatingCharacterLevelRewards[i].startLevel;
				rewardData.InventoryTemplate = itemTemplate;
				rewardData.repeatLevels = gameBalanceVars.RepeatingCharacterLevelRewards[i].repeatingLevel;
				rewardData.isRepeating = true;
				this.m_charRewards.Add(rewardData);
			}
		}
		this.m_charLevel = newData.ExperienceComponent.Level;
		RewardUtils.RewardData rewardToUseForDisplay = this.GetRewardToUseForDisplay(this.m_charRewards, this.m_charLevel);
		if (rewardToUseForDisplay != null)
		{
			if (!string.IsNullOrEmpty(rewardToUseForDisplay.SpritePath))
			{
				this.m_charRewardIconImage.sprite = Resources.Load<Sprite>(rewardToUseForDisplay.SpritePath);
				goto IL_26A;
			}
		}
		this.m_charRewardIconImage.sprite = null;
		IL_26A:
		this.m_charRewardTooltipObj.Refresh();
	}

	private RewardUtils.RewardData GetRewardToUseForDisplay(List<RewardUtils.RewardData> possibleRewards, int curLevel)
	{
		List<RewardUtils.RewardData> list = new List<RewardUtils.RewardData>();
		int num = -1;
		int i = 0;
		while (i < possibleRewards.Count)
		{
			if (possibleRewards[i].Level > curLevel)
			{
				goto IL_4D;
			}
			if (possibleRewards[i].isRepeating)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					goto IL_4D;
				}
			}
			IL_AB:
			i++;
			continue;
			IL_4D:
			if (list.Count == 0)
			{
				list.Add(possibleRewards[i]);
				num = possibleRewards[i].Level;
				goto IL_AB;
			}
			if (num == possibleRewards[i].Level)
			{
				list.Add(possibleRewards[i]);
				goto IL_AB;
			}
			goto IL_AB;
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
		}
		if (list.Count > 0)
		{
			return list[0];
		}
		return null;
	}
}
