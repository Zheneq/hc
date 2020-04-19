using System;
using System.Collections;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGameOverScreen : UIScene
{
	[Header("Setting Values")]
	public float VictoryDefeatDisplayTimeDuration = 3.5f;

	public float TopParticipantDisplayTimeDuration = 3.5f;

	public float PersonalHighlightsDisplayTimeDuration = 5f;

	public float StatsDisplayTimeDuration = 2.5f;

	public float TopPartipantFrontTimePadPercentage = 0.1f;

	public float TopPartipantEndTimePadPercentage = 0.1f;

	public float PersonalHighlightsFrontTimePadPercentage = 0.1f;

	public float PersonalHighlightsEndTimePadPercentage = 0.3f;

	public float PersonalHighlightsClickSkipPercentage = 0.8f;

	[Header("General Objects")]
	public RectTransform m_TopBottomBarsContainer;

	public RectTransform m_TopBarContainer;

	public RectTransform m_BottomBarContainer;

	public RectTransform m_MouseClickContainer;

	public TextMeshProUGUI m_gameTypeLabel;

	public TextMeshProUGUI m_turnTimeText;

	public TextMeshProUGUI m_blueTeamScore;

	public TextMeshProUGUI m_redTeamScore;

	public TextMeshProUGUI m_mapText;

	public TextMeshProUGUI m_objectiveText;

	public RectTransform m_redTeamVictoryContainer;

	public RectTransform m_blueTeamVictoryContainer;

	public _SelectableBtn m_AccoladesHeaderBtn;

	public _SelectableBtn m_StatsHeaderBtn;

	public _SelectableBtn m_RewardsHeaderBtn;

	public _SelectableBtn m_ScoreHeaderBtn;

	public UIGameOverBadgeWidget m_BadgePrefab;

	public TextMeshProUGUI m_rewardNumberText;

	[Header("GG Boost Screen Objects")]
	public RectTransform m_GGBoostContainer;

	public Animator GGBoostContainerAnimator;

	public UILoadscreenProfile[] m_blueTeamGGProfiles;

	public UILoadscreenProfile[] m_redTeamGGProfiles;

	public _ButtonSwapSprite m_worldGGBtnHitBox;

	public TextMeshProUGUI[] m_worldGGPackText;

	public TextMeshProUGUI m_GGPackCount;

	public Animator m_PercentageAnimator;

	public RectTransform[] m_ggButtonLevels;

	public Animator[] m_ggButtonLevelsAnims;

	public Image m_GGBoostTimer;

	[Header("Tutorial Ten Games Screen Objects")]
	public RectTransform m_tutorialLevelContainer;

	public TextMeshProUGUI m_tutorialLevelText;

	public HorizontalLayoutGroup m_tutorialLevelLayout;

	public UITutorialSeasonLevelBar m_tutorialLevelBarPrefab;

	public TextMeshProUGUI m_tutorialNextRewardLabel;

	public Image m_tutorialRewardIconImage;

	public Image m_tutorialRewardFgImage;

	public UITooltipHoverObject m_tutorialRewardTooltipObj;

	[Header("Accolades")]
	public Animator m_AccoladesAnimator;

	[Header("Top Participants Objects")]
	public Animator m_TopParticipantsAnimator;

	public UIGameOverTopParticipantWidget[] m_TopParticipantWidgets;

	[Header("Personal Highlights Objects")]
	public Animator m_PersonalHighlightsAnimator;

	public Image m_PersonalHighlightsCharacterImage;

	public UIGameOverStatWidget[] m_PersonalHighlightWidgets;

	public LayoutGroup m_PersonalHighlightBadgesContainer;

	[Header("Mission Notifications Objects")]
	public RectTransform TempPlaceHolderF;

	[Header("Stats Objects")]
	public RectTransform m_StatsContainer;

	public Animator m_StatsAnimator;

	public Image m_CharacterImage;

	public LayoutGroup m_freelancerStatGrid;

	public LayoutGroup m_generalStatGrid;

	public LayoutGroup m_firepowerStatGrid;

	public LayoutGroup m_supportStatGrid;

	public LayoutGroup m_frontlineStatGrid;

	public UIGameOverStatWidget m_freelancerStatPrefab;

	public UIGameOverStatWidget m_generalStatPrefab;

	public LayoutGroup m_StatPadgeBadgesContainer;

	[Header("Rewards Objects")]
	public RectTransform m_RewardsContainer;

	public LayoutGroup m_RewardsGrid;

	public ScrollRect m_RewardsScrollRect;

	public EndGameRewardItem m_endGameReward;

	[Header("Experience Bar Objects")]
	public Image m_characterExpBarImage;

	public Image m_bannerBG;

	public Image m_bannerFG;

	public UIGameOverScreen.XPDisplayInfo m_characterXPInfo = new UIGameOverScreen.XPDisplayInfo();

	public UIGameOverScreen.XPDisplayInfo m_playerXPInfo = new UIGameOverScreen.XPDisplayInfo();

	public TextMeshProUGUI m_seasonLevelText;

	public TextMeshProUGUI m_expGain;

	public Animator m_ggBonusController;

	public TextMeshProUGUI m_ggBonusXPAnimText;

	public RectTransform m_rewardsInfoContainer;

	public UITooltipHoverObject m_ggBonusTooltipObj;

	[Header("Currency Increase Objects")]
	public UIGameOverScreen.CurrencyDisplayInfo m_isoDisplay;

	public UIGameOverScreen.CurrencyDisplayInfo m_freelancerCurrencyDisplay;

	public UIGameOverScreen.CurrencyDisplayInfo m_rankedCurrencyDisplay;

	public UIGameOverScreen.CurrencyDisplayInfo m_influenceDisplay;

	[Header("Rank Mode Change")]
	public RectTransform m_rankModeLevelContainer;

	public Animator m_rankModeLevelAnimator;

	public TextMeshProUGUI m_rankUpDownText;

	public TextMeshProUGUI m_rankTeirText;

	public TextMeshProUGUI m_rankPointsText;

	public TextMeshProUGUI m_rankLevelText;

	public Image m_rankNormalBar;

	public Image m_rankDecreaseBar;

	public Image m_rankIncreaseBar;

	public Image m_rankIcon;

	[Header("Bottom Right Buttons")]
	public _SelectableBtn m_ContinueBtn;

	public _SelectableBtn m_shareFacebookButton;

	public _SelectableBtn m_feedbackButton;

	private static UIGameOverScreen s_instance;

	private MatchResultsNotification m_results;

	private GameResult m_gameResult;

	private GameType m_gameType;

	private int m_numSelfGGpacksUsed;

	private float m_GGPack_XPMult;

	private UIGameOverScreen.GameOverSubState m_currentSubState;

	private PersistedStats m_statsAtBeginningOfMatch;

	private float EstimatedNotificationArrivalTime = -1f;

	private int LastRankTierDisplayed = -0x64;

	private bool ShouldDisplayTierPoints;

	private bool RankLevelUpDownAnimating;

	private const float MIN_RANK_FILL_AMT = 0.082f;

	private const float MAX_RANK_FILL_AMT = 0.915f;

	private const float POINTS_PER_RANKED_TIER = 100f;

	private List<UITutorialSeasonLevelBar> m_tutorialLevelSliderBars;

	private List<RewardUtils.RewardData> m_tutorialRewards;

	private List<UIGameOverStatWidget> m_GameOverStatWidgets = new List<UIGameOverStatWidget>();

	private float ContinueBtnFailSafeTime = -1f;

	public bool IsVisible { get; private set; }

	public static UIGameOverScreen Get()
	{
		return UIGameOverScreen.s_instance;
	}

	public MatchResultsNotification Results
	{
		get
		{
			return this.m_results;
		}
	}

	public int NumRewardsEarned { get; private set; }

	public bool RequestedToUseGGPack { get; private set; }

	private bool BadgesAreSet { get; set; }

	private bool TalliedCurrencies { get; set; }

	private bool IsRewardsScreenSetup { get; set; }

	private bool NotificationArrived
	{
		get
		{
			return this.m_results != null;
		}
	}

	private bool IsRankedGame
	{
		get
		{
			return this.m_gameType == GameType.Ranked;
		}
	}

	private Team ClientTeam
	{
		get
		{
			return (GameManager.Get().PlayerInfo.TeamId != Team.TeamB) ? Team.TeamA : Team.TeamB;
		}
	}

	private Team FriendlyTeam
	{
		get
		{
			Team result;
			if (this.ClientTeam == Team.TeamB)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.get_FriendlyTeam()).MethodHandle;
				}
				result = Team.TeamB;
			}
			else
			{
				result = Team.TeamA;
			}
			return result;
		}
	}

	private bool SelfWon
	{
		get
		{
			bool result = false;
			if (GameManager.Get() != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.get_SelfWon()).MethodHandle;
				}
				if (GameManager.Get().PlayerInfo != null)
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
					Team team;
					if (GameManager.Get().PlayerInfo.TeamId == Team.TeamB)
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
						team = Team.TeamB;
					}
					else
					{
						team = Team.TeamA;
					}
					Team team2 = team;
					Team team3 = (team2 != Team.TeamB) ? Team.TeamA : Team.TeamB;
					if (this.m_gameResult != GameResult.TeamAWon || team3 != Team.TeamA)
					{
						if (this.m_gameResult != GameResult.TeamBWon)
						{
							return result;
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
						if (team3 != Team.TeamB)
						{
							return result;
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
					result = true;
				}
			}
			return result;
		}
	}

	private bool BadgesAreActive
	{
		get
		{
			if (this.NotificationArrived)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.get_BadgesAreActive()).MethodHandle;
				}
				if (!this.m_results.BadgeAndParticipantsInfo.IsNullOrEmpty<BadgeAndParticipantInfo>())
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
					return true;
				}
			}
			return false;
		}
	}

	public override void Awake()
	{
		UIGameOverScreen.s_instance = this;
		this.UpdateEndGameGGBonuses(0, 1f);
		ClientGameManager.Get().OnMatchResultsNotification += this.HandleMatchResultsNotification;
		ClientGameManager.Get().OnBankBalanceChange += this.HandleBankBalanceChange;
		ClientGameManager.Get().OnUseGGPackNotification += this.HandleGGPackUsed;
		this.m_tutorialLevelSliderBars = new List<UITutorialSeasonLevelBar>();
		this.m_tutorialLevelSliderBars.AddRange(this.m_tutorialLevelLayout.GetComponentsInChildren<UITutorialSeasonLevelBar>());
		this.m_ggBonusTooltipObj.Setup(TooltipType.Titled, new TooltipPopulateCall(this.GGBonusTooltipSetup), null);
		this.InitButtons();
		UIManager.SetGameObjectActive(this.m_TopParticipantsAnimator, false, null);
		UIManager.SetGameObjectActive(this.m_PersonalHighlightsAnimator, false, null);
		base.Awake();
	}

	private void InitButtons()
	{
		this.m_worldGGBtnHitBox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnWorldGGButtonClicked);
		this.m_ContinueBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnContinueClicked);
		this.m_ContinueBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.DialogBoxButton;
		this.m_feedbackButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.FeedbackButtonClicked);
		this.m_feedbackButton.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		this.m_shareFacebookButton.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ShareFacebookButtonClicked);
		this.m_shareFacebookButton.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		this.m_influenceDisplay.m_tooltipHitBox.Setup(TooltipType.MatchEndRewards, delegate(UITooltipBase tooltip)
		{
			this.GetRewardTooltip(tooltip).Setup(this.m_results.FactionContributionAmounts, UIGameOverRewardTooltip.RewardTooltipType.FactionInfo);
			return true;
		}, null);
		this.m_isoDisplay.m_tooltipHitBox.Setup(TooltipType.MatchEndRewards, (UITooltipBase tooltip) => this.SetupCurrencyTooltip(tooltip, UIGameOverRewardTooltip.RewardTooltipType.ISOAmount, this.m_isoDisplay), null);
		this.m_freelancerCurrencyDisplay.m_tooltipHitBox.Setup(TooltipType.MatchEndRewards, (UITooltipBase tooltip) => this.SetupCurrencyTooltip(tooltip, UIGameOverRewardTooltip.RewardTooltipType.FreelancerCurrencyAmount, this.m_freelancerCurrencyDisplay), null);
		this.m_rankedCurrencyDisplay.m_tooltipHitBox.Setup(TooltipType.MatchEndRewards, (UITooltipBase tooltip) => this.SetupCurrencyTooltip(tooltip, UIGameOverRewardTooltip.RewardTooltipType.RankedPointsAmount, this.m_rankedCurrencyDisplay), null);
		this.m_characterXPInfo.m_tooltipHitBox.Setup(TooltipType.MatchEndRewards, delegate(UITooltipBase tooltip)
		{
			this.GetRewardTooltip(tooltip).Setup(this.m_characterXPInfo, UIGameOverRewardTooltip.RewardTooltipType.CharacterInfo, this.m_results.NumCharactersPlayed);
			return true;
		}, null);
		UIEventTriggerUtils.AddListener(this.m_characterXPInfo.m_tooltipHitBox.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.DisplayCharacterXPInfo));
		UIEventTriggerUtils.AddListener(this.m_characterXPInfo.m_tooltipHitBox.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.HideXpInfo));
		this.m_playerXPInfo.m_tooltipHitBox.Setup(TooltipType.MatchEndRewards, delegate(UITooltipBase tooltip)
		{
			this.GetRewardTooltip(tooltip).Setup(this.m_playerXPInfo, UIGameOverRewardTooltip.RewardTooltipType.AccountInfo, 1);
			return true;
		}, null);
		UIEventTriggerUtils.AddListener(this.m_playerXPInfo.m_tooltipHitBox.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.DisplayPlayerXPInfo));
		UIEventTriggerUtils.AddListener(this.m_playerXPInfo.m_tooltipHitBox.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.HideXpInfo));
		this.m_characterXPInfo.Init();
		this.m_playerXPInfo.Init();
		this.m_AccoladesHeaderBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.NavButtonClicked);
		this.m_StatsHeaderBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.NavButtonClicked);
		this.m_RewardsHeaderBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.NavButtonClicked);
		this.m_ScoreHeaderBtn.spriteController.callback = new _ButtonSwapSprite.ButtonClickCallback(this.NavButtonClicked);
		UIManager.SetGameObjectActive(this.m_AccoladesHeaderBtn, true, null);
		UIManager.SetGameObjectActive(this.m_StatsHeaderBtn, true, null);
		UIManager.SetGameObjectActive(this.m_StatsHeaderBtn, true, null);
		UIManager.SetGameObjectActive(this.m_ScoreHeaderBtn, true, null);
	}

	private void OnDestroy()
	{
		if (UIScreenManager.Get() != null)
		{
			UIScreenManager.Get().EndAllLoopSounds();
		}
		UIGameOverScreen.s_instance = null;
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnMatchResultsNotification -= this.HandleMatchResultsNotification;
			ClientGameManager.Get().OnBankBalanceChange -= this.HandleBankBalanceChange;
			ClientGameManager.Get().OnUseGGPackNotification -= this.HandleGGPackUsed;
		}
	}

	private void HideXpInfo(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_characterXPInfo.m_XPLabel, false, null);
		UIManager.SetGameObjectActive(this.m_playerXPInfo.m_XPLabel, false, null);
	}

	private bool GGBonusTooltipSetup(UITooltipBase tooltip)
	{
		int num = 0;
		if (ClientGameManager.Get() != null && ClientGameManager.Get().PlayerWallet != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.GGBonusTooltipSetup(UITooltipBase)).MethodHandle;
			}
			num = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack);
		}
		string rightString = string.Empty;
		if (num > 0)
		{
			rightString = string.Format("<color=green>x{0}</color>", num);
		}
		else
		{
			rightString = string.Format("<color=#7f7f7f>x{0}</color>", num);
		}
		string text = StringUtil.TR("GGBoostUsageDescription", "GameOver");
		if (num == 0)
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
			text += StringUtil.TR("NoGGBoosts", "GameOver");
		}
		UITitledTooltip uititledTooltip = tooltip as UITitledTooltip;
		uititledTooltip.Setup(StringUtil.TR("GGBoosts", "Rewards"), text, rightString);
		return true;
	}

	private void DisplayCharacterXPInfo(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_characterXPInfo.m_XPLabel, true, null);
	}

	private void DisplayPlayerXPInfo(BaseEventData data)
	{
		SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason);
		UIManager.SetGameObjectActive(this.m_tutorialLevelContainer, seasonTemplate.IsTutorial, null);
		UIManager.SetGameObjectActive(this.m_playerXPInfo.m_XPLabel, !seasonTemplate.IsTutorial, null);
	}

	public void NotifyWidgetMouseOver(UIGameOverBadgeWidget widget, bool mouseOver)
	{
		if (mouseOver)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.NotifyWidgetMouseOver(UIGameOverBadgeWidget, bool)).MethodHandle;
			}
			for (int i = 0; i < this.m_GameOverStatWidgets.Count; i++)
			{
				if (widget.BadgeInfo.UsesFreelancerStats)
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
					if (this.m_GameOverStatWidgets[i].DisplayStatType == UIGameOverStatWidget.StatDisplayType.FreelancerStat)
					{
						this.m_GameOverStatWidgets[i].SetBadgeHighlight(true, true);
					}
					else
					{
						this.m_GameOverStatWidgets[i].SetBadgeHighlight(true, false);
					}
				}
				else if (this.m_GameOverStatWidgets[i].DisplayStatType == UIGameOverStatWidget.StatDisplayType.GeneralStat)
				{
					if (widget.BadgeInfo.StatsToHighlight.Contains(this.m_GameOverStatWidgets[i].GeneralStatType))
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
						this.m_GameOverStatWidgets[i].SetBadgeHighlight(true, true);
					}
					else
					{
						this.m_GameOverStatWidgets[i].SetBadgeHighlight(true, false);
					}
				}
				else
				{
					this.m_GameOverStatWidgets[i].SetBadgeHighlight(true, false);
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
		else
		{
			for (int j = 0; j < this.m_GameOverStatWidgets.Count; j++)
			{
				this.m_GameOverStatWidgets[j].SetBadgeHighlight(false, false);
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
	}

	public void ShareFacebookButtonClicked(BaseEventData data)
	{
		FacebookClientInterface facebookClientInterface = FacebookClientInterface.Get();
		if (UIGameOverScreen.<>f__am$cache0 == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.ShareFacebookButtonClicked(BaseEventData)).MethodHandle;
			}
			UIGameOverScreen.<>f__am$cache0 = delegate(Texture2D texture)
			{
				UILandingPageFullScreenMenus.Get().SetFacebookContainerVisible(true, texture);
			};
		}
		facebookClientInterface.TakeScreenshot(UIGameOverScreen.<>f__am$cache0);
	}

	public void NavButtonClicked(BaseEventData data)
	{
		if (this.m_currentSubState != null && this.m_currentSubState.SubStateType != UIGameOverScreen.GameOverScreenState.Stats)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.NavButtonClicked(BaseEventData)).MethodHandle;
			}
			if (this.m_currentSubState.SubStateType != UIGameOverScreen.GameOverScreenState.Rewards)
			{
				if (this.m_currentSubState.SubStateType != UIGameOverScreen.GameOverScreenState.Done)
				{
					return;
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
		if (this.m_currentSubState.SubStateType != UIGameOverScreen.GameOverScreenState.Done)
		{
			this.m_currentSubState = new UIGameOverScreen.GameOverSubState(UIGameOverScreen.GameOverScreenState.Done, 0f);
		}
		GameObject gameObject = (data as PointerEventData).pointerCurrentRaycast.gameObject;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		if (this.m_AccoladesHeaderBtn.spriteController.gameObject == gameObject)
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
		}
		else if (this.m_StatsHeaderBtn.spriteController.gameObject == gameObject)
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
			flag2 = true;
		}
		else if (this.m_RewardsHeaderBtn.spriteController.gameObject == gameObject)
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
			flag3 = true;
		}
		else if (this.m_ScoreHeaderBtn.spriteController.gameObject == gameObject)
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
			flag4 = true;
		}
		this.m_AccoladesHeaderBtn.SetSelected(flag, false, "SelectedIN", "SelectedOUT");
		this.m_StatsHeaderBtn.SetSelected(flag2, false, "SelectedIN", "SelectedOUT");
		this.m_RewardsHeaderBtn.SetSelected(flag3, false, "SelectedIN", "SelectedOUT");
		this.m_ScoreHeaderBtn.SetSelected(flag4, false, "SelectedIN", "SelectedOUT");
		this.RefreshHeaderButtonClickability();
		UIManager.SetGameObjectActive(this.m_AccoladesAnimator, flag, null);
		if (flag)
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
			UIManager.SetGameObjectActive(this.m_TopParticipantsAnimator, true, null);
			UIManager.SetGameObjectActive(this.m_PersonalHighlightsAnimator, true, null);
			UIAnimationEventManager.Get().PlayAnimation(this.m_TopParticipantsAnimator, "TopParticipantsGrpDefaultIDLE", null, string.Empty, 0, 0f, true, false, null, null);
			UIAnimationEventManager.Get().PlayAnimation(this.m_PersonalHighlightsAnimator, "PersonalHighlightsGrpDefaultIDLE", null, string.Empty, 0, 0f, true, false, null, null);
			for (int i = 0; i < this.m_PersonalHighlightWidgets.Length; i++)
			{
				UIManager.SetGameObjectActive(this.m_PersonalHighlightWidgets[i], true, null);
				this.m_PersonalHighlightWidgets[i].SetHighlight();
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
		UIManager.SetGameObjectActive(this.m_StatsContainer, flag2, null);
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
			UIAnimationEventManager.Get().PlayAnimation(this.m_StatsAnimator, "StatsScreenDefaultIDLE", null, string.Empty, 0, 0f, true, false, null, null);
		}
		UIManager.SetGameObjectActive(this.m_RewardsContainer, flag3, null);
		UIGameStatsWindow.Get().SetToggleStatsVisible(flag4, true);
	}

	public void FeedbackButtonClicked(BaseEventData data)
	{
		UILandingPageFullScreenMenus.Get().SetFeedbackContainerVisible(true);
	}

	public void OnWorldGGButtonClicked(BaseEventData data)
	{
		if (this.RequestedToUseGGPack)
		{
			return;
		}
		int currentAmount = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack);
		if (currentAmount != 0)
		{
			if (this.m_numSelfGGpacksUsed < 3)
			{
				for (int i = 0; i < this.m_ggButtonLevelsAnims.Length; i++)
				{
					if (this.m_ggButtonLevelsAnims[i].gameObject.activeInHierarchy)
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
						if (currentAmount > 1)
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
							UIAnimationEventManager.Get().PlayAnimation(this.m_ggButtonLevelsAnims[i], "GGBoostDefaultIN", null, string.Empty, 0, 0f, true, false, null, null);
						}
						else
						{
							UIAnimationEventManager.Get().PlayAnimation(this.m_ggButtonLevelsAnims[i], "GGBoostNoMoreDefaultIN", null, string.Empty, 0, 0f, true, false, null, null);
							this.m_worldGGBtnHitBox.SetClickable(false);
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
				ClientGameManager.Get().RequestToUseGGPack();
				this.m_numSelfGGpacksUsed++;
				for (int j = 0; j < this.m_ggButtonLevels.Length; j++)
				{
					UIManager.SetGameObjectActive(this.m_ggButtonLevels[j], j == this.m_numSelfGGpacksUsed, null);
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
				if (this.m_numSelfGGpacksUsed == 1)
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
					UIFrontEnd.PlaySound(FrontEndButtonSounds.GGButtonEndGameUsed);
				}
				else if (this.m_numSelfGGpacksUsed == 2)
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
					AudioManager.PostEvent("ui/endgame/ggboost_button_silver", null);
				}
				else if (this.m_numSelfGGpacksUsed == 3)
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
					AudioManager.PostEvent("ui/endgame/ggboost_button_gold", null);
				}
				if (GameOverWorldObjects.Get() != null)
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
					GameOverWorldObjects.Get().m_worldResultAnimController.Play("ResultGGPackPressAnimation");
				}
				this.RequestedToUseGGPack = true;
				if (currentAmount > 1)
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
					if (this.m_numSelfGGpacksUsed < 3)
					{
						return;
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
				for (int k = 0; k < this.m_ggButtonLevelsAnims.Length; k++)
				{
					if (this.m_ggButtonLevelsAnims[k].gameObject.activeInHierarchy)
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
						UIAnimationEventManager.Get().PlayAnimation(this.m_ggButtonLevelsAnims[k], "GGBoostNoMoreDefaultIN", null, string.Empty, 0, 0f, true, false, null, null);
					}
				}
				this.m_worldGGBtnHitBox.SetClickable(false);
				return;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.OnWorldGGButtonClicked(BaseEventData)).MethodHandle;
			}
		}
		this.m_worldGGBtnHitBox.SetClickable(false);
	}

	public void OnContinueClicked(BaseEventData data)
	{
		UINewReward.Get().Clear();
		UIGameStatsWindow.Get().SetVisible(false);
		FacebookClientInterface.Get().Reset();
		ClientGameManager.Get().LeaveGame(true, this.m_gameResult);
	}

	public static CurrencyType RewardTooltipTypeToCurrencyType(UIGameOverRewardTooltip.RewardTooltipType rewardType)
	{
		if (rewardType == UIGameOverRewardTooltip.RewardTooltipType.FreelancerCurrencyAmount)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.RewardTooltipTypeToCurrencyType(UIGameOverRewardTooltip.RewardTooltipType)).MethodHandle;
			}
			return CurrencyType.FreelancerCurrency;
		}
		if (rewardType == UIGameOverRewardTooltip.RewardTooltipType.ISOAmount)
		{
			return CurrencyType.ISO;
		}
		if (rewardType == UIGameOverRewardTooltip.RewardTooltipType.RankedPointsAmount)
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
			return CurrencyType.RankedCurrency;
		}
		return CurrencyType.NONE;
	}

	private bool SetupCurrencyTooltip(UITooltipBase tooltip, UIGameOverRewardTooltip.RewardTooltipType type, UIGameOverScreen.CurrencyDisplayInfo info)
	{
		this.GetRewardTooltip(tooltip).Setup(info.GetTotalBaseGained(UIGameOverScreen.RewardTooltipTypeToCurrencyType(type)), info.GetTotalGGGained(UIGameOverScreen.RewardTooltipTypeToCurrencyType(type)), info.GetTotalWinGained(UIGameOverScreen.RewardTooltipTypeToCurrencyType(type)), info.GetTotalQuestGained(UIGameOverScreen.RewardTooltipTypeToCurrencyType(type)), info.GetTotalLevelUpGained(UIGameOverScreen.RewardTooltipTypeToCurrencyType(type)), info.GetTotalEventGained(UIGameOverScreen.RewardTooltipTypeToCurrencyType(type)), type);
		return true;
	}

	private UIGameOverRewardTooltip GetRewardTooltip(UITooltipBase tooltip)
	{
		return tooltip as UIGameOverRewardTooltip;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.GameOver;
	}

	public static ActorData GetPlayersOriginalActorData()
	{
		LobbyPlayerInfo playerInfo = GameManager.Get().PlayerInfo;
		using (List<ActorData>.Enumerator enumerator = GameFlowData.Get().GetAllTeamMembers(playerInfo.TeamId).GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActorData actorData = enumerator.Current;
				if (actorData.m_characterType == playerInfo.CharacterType)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.GetPlayersOriginalActorData()).MethodHandle;
					}
					return actorData;
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
		return null;
	}

	public void SetWorldGGPackText(string text)
	{
		for (int i = 0; i < this.m_worldGGPackText.Length; i++)
		{
			this.m_worldGGPackText[i].text = text;
		}
	}

	private void SetupLabelText(int myTeamScore, int enemyTeamScore)
	{
		if (this.m_turnTimeText != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.SetupLabelText(int, int)).MethodHandle;
			}
			string arg = string.Empty;
			if (UITimerPanel.Get().GetSeconds() < 0xA)
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
				arg = string.Format(StringUtil.TR("TimeFormatLeadingZero", "Global"), UITimerPanel.Get().GetMinutes(), UITimerPanel.Get().GetSeconds());
			}
			else
			{
				arg = string.Format(StringUtil.TR("TimeFormat", "Global"), UITimerPanel.Get().GetMinutes(), UITimerPanel.Get().GetSeconds());
			}
			this.m_turnTimeText.text = string.Format(StringUtil.TR("TurnTime", "GameOver"), UITimerPanel.Get().GetTurn(), arg);
		}
		if (this.m_blueTeamScore != null)
		{
			this.m_blueTeamScore.text = string.Format(StringUtil.TR("BlueTeamScore", "GameOver"), myTeamScore.ToString());
		}
		if (this.m_redTeamScore != null)
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
			this.m_redTeamScore.text = string.Format(StringUtil.TR("RedTeamScore", "GameOver"), enemyTeamScore.ToString());
		}
		if (this.m_mapText != null)
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
			this.m_mapText.text = GameWideData.Get().GetMapDisplayName(GameManager.Get().GameInfo.GameConfig.Map);
		}
		if (this.m_gameTypeLabel != null)
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
			this.m_gameTypeLabel.text = string.Format(StringUtil.TR("DeathMatchLabel", "GameOver"), GameManager.Get().GameConfig.TeamAPlayers.ToString(), GameManager.Get().GameConfig.TeamBPlayers.ToString());
		}
		if (this.m_objectiveText != null)
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
			if (ObjectivePoints.Get() != null)
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
				this.m_objectiveText.text = string.Format(StringUtil.TR(ObjectivePoints.Get().m_victoryCondition), (ObjectivePoints.Get().m_timeLimitTurns - 1).ToString());
			}
		}
	}

	public void HandleBankBalanceChange(CurrencyData currencyData)
	{
		int currentAmount = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack);
		this.m_GGPackCount.text = string.Format("x{0}", currentAmount);
		for (int i = 0; i < this.m_ggButtonLevelsAnims.Length; i++)
		{
			if (this.m_ggButtonLevelsAnims[i].gameObject.activeInHierarchy)
			{
				if (currentAmount > 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.HandleBankBalanceChange(CurrencyData)).MethodHandle;
					}
					UIAnimationEventManager.Get().PlayAnimation(this.m_ggButtonLevelsAnims[i], "GGBoostDefaultIN", null, string.Empty, 0, 0f, true, false, null, null);
				}
				else
				{
					UIAnimationEventManager.Get().PlayAnimation(this.m_ggButtonLevelsAnims[i], "GGBoostNoMoreDefaultIN", null, string.Empty, 0, 0f, true, false, null, null);
				}
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

	public static List<SeasonReward> GetAccountRewardsForNextLevel(int currentLevel)
	{
		PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
		List<SeasonReward> allRewards = SeasonWideData.Get().GetSeasonTemplate(playerAccountData.QuestComponent.ActiveSeason).Rewards.GetAllRewards();
		List<SeasonReward> list = new List<SeasonReward>();
		int num = currentLevel + 1;
		for (int i = 0; i < allRewards.Count; i++)
		{
			if (allRewards[i].level == num)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.GetAccountRewardsForNextLevel(int)).MethodHandle;
				}
				if (allRewards[i].repeatEveryXLevels == 0)
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
					list.Add(allRewards[i]);
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
		for (int j = 0; j < allRewards.Count; j++)
		{
			if (allRewards[j].repeatEveryXLevels > 0)
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
				if ((num - allRewards[j].level) % allRewards[j].repeatEveryXLevels == 0)
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
					list.Add(allRewards[j]);
				}
			}
		}
		return list;
	}

	private bool SetupAccountRewardTooltip(int currentLevel)
	{
		this.m_playerXPInfo.PopulateRewardIcon(currentLevel, true, null);
		return true;
	}

	private void SetupExpBars()
	{
		int num = GameBalanceVars.Get().CharacterExperienceToLevel(this.m_results.CharacterLevelAtStart);
		this.m_characterXPInfo.m_barLevelLabel.text = this.m_results.CharacterLevelAtStart.ToString();
		this.m_characterXPInfo.m_GGXPSlider.fillAmount = 0f;
		this.m_characterXPInfo.m_NormalXPGainSlider.fillAmount = 0f;
		this.m_characterXPInfo.m_OldXPSlider.fillAmount = (float)this.m_results.CharacterXpAtStart / (float)num;
		this.m_characterXPInfo.m_QuestXPSlider.fillAmount = 0f;
		this.m_characterXPInfo.m_XPLabel.text = this.m_results.CharacterXpAtStart.ToString();
		this.m_characterXPInfo.m_LastLevelDisplayed = this.m_results.CharacterLevelAtStart;
		this.m_characterXPInfo.XPBarType = UIGameOverScreen.XPDisplayInfo.BarXPType.Character;
		UIManager.SetGameObjectActive(this.m_characterXPInfo.m_XPLabel, false, null);
		int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
		int seasonExperience = SeasonWideData.Get().GetSeasonExperience(activeSeason, this.m_results.SeasonLevelAtStart);
		this.m_playerXPInfo.m_barLevelLabel.text = this.m_results.SeasonLevelAtStart.ToString();
		this.m_playerXPInfo.m_GGXPSlider.fillAmount = 0f;
		this.m_playerXPInfo.m_NormalXPGainSlider.fillAmount = 0f;
		this.m_playerXPInfo.m_OldXPSlider.fillAmount = (float)this.m_results.SeasonXpAtStart / (float)seasonExperience;
		this.m_playerXPInfo.m_QuestXPSlider.fillAmount = 0f;
		this.m_playerXPInfo.m_XPLabel.text = this.m_results.SeasonXpAtStart.ToString();
		this.m_playerXPInfo.m_LastLevelDisplayed = this.m_results.SeasonLevelAtStart;
		this.m_playerXPInfo.XPBarType = UIGameOverScreen.XPDisplayInfo.BarXPType.Season;
		UIManager.SetGameObjectActive(this.m_playerXPInfo.m_XPLabel, false, null);
		this.m_expGain.text = "+0";
		this.SetupAccountRewardTooltip(this.m_results.SeasonLevelAtStart);
		ActorData playersOriginalActorData = UIGameOverScreen.GetPlayersOriginalActorData();
		if (playersOriginalActorData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.SetupExpBars()).MethodHandle;
			}
			this.SetupCharacterRewardTooltip(playersOriginalActorData.\u000E(), this.m_results.CharacterLevelAtStart);
		}
		if (GameFlowData.Get().activeOwnedActorData != null)
		{
			if (ReplayPlayManager.Get())
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
				if (ReplayPlayManager.Get().IsPlayback())
				{
					goto IL_2C4;
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
			UIManager.SetGameObjectActive(this.m_rewardsInfoContainer, true, null);
			goto IL_2D1;
		}
		IL_2C4:
		UIManager.SetGameObjectActive(this.m_rewardsInfoContainer, false, null);
		IL_2D1:
		SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason);
		UIManager.SetGameObjectActive(this.m_playerXPInfo.m_barLevelUpAnimator, !seasonTemplate.IsTutorial, null);
		UIManager.SetGameObjectActive(this.m_tutorialLevelContainer, seasonTemplate.IsTutorial, null);
		if (seasonTemplate.IsTutorial)
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
			int endLevel = QuestWideData.GetEndLevel(seasonTemplate.Prerequisites, seasonTemplate.Index);
			int num2 = this.m_results.SeasonLevelAtStart;
			if (this.m_gameType != GameType.Custom)
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
				if (this.m_gameType == GameType.Tutorial)
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
					if (num2 > 1)
					{
						goto IL_38F;
					}
				}
				num2++;
			}
			IL_38F:
			this.m_tutorialLevelText.text = num2 - 1 + "/" + (endLevel - 1);
			for (int i = this.m_tutorialLevelSliderBars.Count; i < endLevel - 1; i++)
			{
				UITutorialSeasonLevelBar uitutorialSeasonLevelBar = UnityEngine.Object.Instantiate<UITutorialSeasonLevelBar>(this.m_tutorialLevelBarPrefab);
				uitutorialSeasonLevelBar.transform.SetParent(this.m_tutorialLevelLayout.transform);
				uitutorialSeasonLevelBar.transform.localScale = Vector3.one;
				uitutorialSeasonLevelBar.transform.localPosition = Vector3.zero;
				this.m_tutorialLevelSliderBars.Add(uitutorialSeasonLevelBar);
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
			Queue<RewardUtils.RewardData> queue = new Queue<RewardUtils.RewardData>(RewardUtils.GetSeasonLevelRewards(-1));
			List<RewardUtils.RewardData> availableSeasonEndRewards = RewardUtils.GetAvailableSeasonEndRewards(seasonTemplate);
			if (availableSeasonEndRewards.Count > 0)
			{
				queue.Enqueue(availableSeasonEndRewards[0]);
			}
			for (int j = 0; j < this.m_tutorialLevelSliderBars.Count; j++)
			{
				int num3 = j + 1;
				this.m_tutorialLevelSliderBars[j].SetFilled(num3 < num2);
				UIManager.SetGameObjectActive(this.m_tutorialLevelSliderBars[j], num3 < endLevel, null);
				RewardUtils.RewardData rewardData = null;
				while (queue.Count > 0)
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
					if (rewardData != null)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							goto IL_517;
						}
					}
					else
					{
						int num4 = queue.Peek().Level - 1;
						if (num4 < num3)
						{
							queue.Dequeue();
						}
						else
						{
							if (num4 > num3)
							{
								break;
							}
							rewardData = queue.Dequeue();
						}
					}
				}
				IL_517:
				this.m_tutorialLevelSliderBars[j].SetReward(num3, rewardData);
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
			this.SetupTutorialRewards();
		}
	}

	private void SetupTutorialRewards()
	{
		SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason);
		this.m_tutorialRewards = RewardUtils.GetSeasonLevelRewards(-1);
		for (int i = 0; i < QuestWideData.GetEndLevel(seasonTemplate.Prerequisites, seasonTemplate.Index); i++)
		{
			using (List<RewardUtils.RewardData>.Enumerator enumerator = RewardUtils.GetAccountLevelRewards(i).GetEnumerator())
			{
				IL_C9:
				while (enumerator.MoveNext())
				{
					RewardUtils.RewardData rewardData = enumerator.Current;
					int index = 0;
					for (int j = 0; j < this.m_tutorialRewards.Count; j++)
					{
						if (rewardData.Level <= this.m_tutorialRewards[j].Level)
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
								RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.SetupTutorialRewards()).MethodHandle;
							}
							index = j;
							IL_BB:
							this.m_tutorialRewards.Insert(index, rewardData);
							goto IL_C9;
						}
					}
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						goto IL_BB;
					}
				}
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
		List<RewardUtils.RewardData> availableSeasonEndRewards = RewardUtils.GetAvailableSeasonEndRewards(seasonTemplate);
		this.m_tutorialRewards.AddRange(availableSeasonEndRewards);
		for (int k = 0; k < this.m_tutorialRewards.Count; k++)
		{
			this.m_tutorialRewards[k].Level--;
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
		if (this.m_tutorialRewards.Count == 0)
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
			UIManager.SetGameObjectActive(this.m_tutorialRewardTooltipObj, false, null);
			UIManager.SetGameObjectActive(this.m_tutorialNextRewardLabel, false, null);
			UIManager.SetGameObjectActive(this.m_tutorialRewardIconImage, false, null);
			return;
		}
		UIManager.SetGameObjectActive(this.m_tutorialRewardTooltipObj, true, null);
		UIManager.SetGameObjectActive(this.m_tutorialNextRewardLabel, true, null);
		UIManager.SetGameObjectActive(this.m_tutorialRewardIconImage, true, null);
		RewardUtils.RewardData rewardData2 = null;
		if (availableSeasonEndRewards.Count > 0)
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
			rewardData2 = availableSeasonEndRewards[0];
		}
		if (rewardData2 == null)
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
			UIManager.SetGameObjectActive(this.m_tutorialRewardTooltipObj, false, null);
			UIManager.SetGameObjectActive(this.m_tutorialNextRewardLabel, false, null);
			UIManager.SetGameObjectActive(this.m_tutorialRewardIconImage, false, null);
			return;
		}
		UIManager.SetGameObjectActive(this.m_tutorialRewardTooltipObj, true, null);
		UIManager.SetGameObjectActive(this.m_tutorialNextRewardLabel, true, null);
		UIManager.SetGameObjectActive(this.m_tutorialRewardIconImage, true, null);
		this.m_tutorialRewardIconImage.sprite = Resources.Load<Sprite>(rewardData2.SpritePath);
		UIManager.SetGameObjectActive(this.m_tutorialRewardFgImage, rewardData2.Foreground != null, null);
		this.m_tutorialRewardFgImage.sprite = rewardData2.Foreground;
		this.m_tutorialNextRewardLabel.text = rewardData2.Name;
		this.m_tutorialRewardTooltipObj.Setup(TooltipType.RewardList, delegate(UITooltipBase tooltip)
		{
			if (this.m_tutorialRewards != null)
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
					RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIGameOverScreen.<SetupTutorialRewards>m__7(UITooltipBase)).MethodHandle;
				}
				if (this.m_tutorialRewards.Count != 0)
				{
					UIRewardListTooltip uirewardListTooltip = tooltip as UIRewardListTooltip;
					uirewardListTooltip.Setup(this.m_tutorialRewards, this.m_results.SeasonLevelAtStart, UIRewardListTooltip.RewardsType.Tutorial, true);
					return true;
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
			return false;
		}, null);
	}

	private void SetupCharacterRewardTooltip(CharacterResourceLink charLink, int currentCharLevel)
	{
		if (GameBalanceVars.Get() == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.SetupCharacterRewardTooltip(CharacterResourceLink, int)).MethodHandle;
			}
			return;
		}
		this.m_characterXPInfo.PopulateRewardIcon(currentCharLevel, true, charLink);
	}

	private void SetupCurrencyAndInfluenceDisplays()
	{
		bool doActive = this.GetDisplayTotalCurrency(this.m_isoDisplay, CurrencyType.ISO) > 0;
		bool doActive2 = this.GetDisplayTotalCurrency(this.m_freelancerCurrencyDisplay, CurrencyType.FreelancerCurrency) > 0;
		bool doActive3 = this.GetDisplayTotalCurrency(this.m_rankedCurrencyDisplay, CurrencyType.RankedCurrency) > 0;
		if (!this.TalliedCurrencies && this.m_results.CurrencyRewards != null)
		{
			for (int i = 0; i < this.m_results.CurrencyRewards.Count; i++)
			{
				int num = this.m_results.CurrencyRewards[i].BaseGained + this.m_results.CurrencyRewards[i].WinGained + this.m_results.CurrencyRewards[i].GGGained + this.m_results.CurrencyRewards[i].QuestGained + this.m_results.CurrencyRewards[i].LevelUpGained + this.m_results.CurrencyRewards[i].EventGained;
				if (num > 0)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.SetupCurrencyAndInfluenceDisplays()).MethodHandle;
					}
					CurrencyType type = this.m_results.CurrencyRewards[i].Type;
					if (type != CurrencyType.ISO)
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
						if (type != CurrencyType.FreelancerCurrency)
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
							if (type != CurrencyType.RankedCurrency)
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
								doActive3 = true;
								this.m_rankedCurrencyDisplay.m_currencyReward.Add(this.m_results.CurrencyRewards[i]);
								this.m_rankedCurrencyDisplay.m_currencyGainText.text = "+0";
							}
						}
						else
						{
							doActive2 = true;
							this.m_freelancerCurrencyDisplay.m_currencyReward.Add(this.m_results.CurrencyRewards[i]);
							this.m_freelancerCurrencyDisplay.m_currencyGainText.text = "+0";
						}
					}
					else
					{
						doActive = true;
						this.m_isoDisplay.m_currencyReward.Add(this.m_results.CurrencyRewards[i]);
						this.m_isoDisplay.m_currencyGainText.text = "+0";
					}
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
			this.TalliedCurrencies = true;
		}
		UIManager.SetGameObjectActive(this.m_isoDisplay.m_container, doActive, null);
		UIManager.SetGameObjectActive(this.m_freelancerCurrencyDisplay.m_container, doActive2, null);
		UIManager.SetGameObjectActive(this.m_rankedCurrencyDisplay.m_container, doActive3, null);
		this.m_influenceDisplay.m_currencyGainText.text = "+0";
		FactionCompetition factionCompetition = FactionWideData.Get().GetFactionCompetition(this.m_results.FactionCompetitionId);
		if (factionCompetition != null && factionCompetition.ShouldShowcase)
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
			if (this.m_results.FactionId < factionCompetition.Factions.Count)
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
				FactionGroup factionGroup = FactionWideData.Get().GetFactionGroup(factionCompetition.Factions[this.m_results.FactionId].FactionGroupIDToUse);
				this.m_influenceDisplay.m_displayIcon.sprite = Resources.Load<Sprite>(factionGroup.IconPath);
				UIManager.SetGameObjectActive(this.m_influenceDisplay.m_container, factionCompetition.Enabled, null);
				return;
			}
		}
		UIManager.SetGameObjectActive(this.m_influenceDisplay.m_container, false, null);
	}

	private void SetupRewardsScreen()
	{
		if (this.NotificationArrived)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.SetupRewardsScreen()).MethodHandle;
			}
			if (!this.IsRewardsScreenSetup)
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
				this.IsRewardsScreenSetup = true;
				List<int> list = new List<int>();
				int i = this.GetNormalBarXPTotal() + this.m_results.GGXpGained + this.m_results.QuestXpGained;
				int num = this.m_results.SeasonLevelAtStart;
				int num2 = UIGameOverScreen.XPDisplayInfo.GetXPForType(UIGameOverScreen.XPDisplayInfo.BarXPType.Season, num) - this.m_results.SeasonXpAtStart;
				while (i >= num2)
				{
					i -= num2;
					num++;
					list.Add(num);
					num2 = UIGameOverScreen.XPDisplayInfo.GetXPForType(UIGameOverScreen.XPDisplayInfo.BarXPType.Season, num);
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
				List<RewardUtils.RewardData> list2 = new List<RewardUtils.RewardData>();
				List<RewardUtils.RewardData> list3 = new List<RewardUtils.RewardData>();
				for (int j = 0; j < list.Count; j++)
				{
					List<RewardUtils.RewardData> nextSeasonLevelRewards = RewardUtils.GetNextSeasonLevelRewards(list[j] - 1);
					foreach (RewardUtils.RewardData rewardData in nextSeasonLevelRewards)
					{
						if (rewardData.InventoryTemplate.Type == InventoryItemType.Currency)
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
							list2.Add(rewardData);
						}
					}
					if (ClientGameManager.Get().GetPlayerAccountData().QuestComponent.SeasonItemRewardsGranted.ContainsKey(list[j]))
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
						List<int> list4 = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.SeasonItemRewardsGranted[list[j]];
						using (List<int>.Enumerator enumerator2 = list4.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								int templateId = enumerator2.Current;
								InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(templateId);
								list2.Add(new RewardUtils.RewardData
								{
									Amount = 1,
									InventoryTemplate = itemTemplate,
									Level = list[j],
									Name = itemTemplate.DisplayName,
									SpritePath = InventoryWideData.GetSpritePath(itemTemplate),
									Foreground = InventoryWideData.GetItemFg(itemTemplate),
									Type = RewardUtils.GetRewardTypeFromInventoryItem(itemTemplate)
								});
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
				ActorData playersOriginalActorData = UIGameOverScreen.GetPlayersOriginalActorData();
				if (playersOriginalActorData != null)
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
					CharacterResourceLink characterResourceLink = playersOriginalActorData.\u000E();
					GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
					List<int> list5 = new List<int>();
					int k = this.GetNormalBarXPTotal() + this.m_results.ConsumableXpGained + this.m_results.GGXpGained;
					int num3 = this.m_results.CharacterLevelAtStart;
					int num4 = UIGameOverScreen.XPDisplayInfo.GetXPForType(UIGameOverScreen.XPDisplayInfo.BarXPType.Character, num3) - this.m_results.CharacterXpAtStart;
					while (k >= num4)
					{
						k -= num4;
						num3++;
						list5.Add(num3);
						num4 = UIGameOverScreen.XPDisplayInfo.GetXPForType(UIGameOverScreen.XPDisplayInfo.BarXPType.Character, num3);
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
					for (int l = 0; l < list5.Count; l++)
					{
						list3.AddRange(RewardUtils.GetNextCharacterRewards(characterResourceLink, list5[l] - 1));
						for (int m = 0; m < gameBalanceVars.RepeatingCharacterLevelRewards.Length; m++)
						{
							if (gameBalanceVars.RepeatingCharacterLevelRewards[m].charType == (int)characterResourceLink.m_characterType)
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
								if (gameBalanceVars.RepeatingCharacterLevelRewards[m].repeatingLevel > 0)
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
									if (list5[l] - 1 > gameBalanceVars.RepeatingCharacterLevelRewards[m].startLevel)
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
										if ((list5[l] - gameBalanceVars.RepeatingCharacterLevelRewards[m].startLevel) % gameBalanceVars.RepeatingCharacterLevelRewards[m].repeatingLevel == 0)
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
											RewardUtils.RewardData rewardData2 = new RewardUtils.RewardData();
											rewardData2.Amount = gameBalanceVars.RepeatingCharacterLevelRewards[m].reward.Amount;
											InventoryItemTemplate itemTemplate2 = InventoryWideData.Get().GetItemTemplate(gameBalanceVars.RepeatingCharacterLevelRewards[m].reward.ItemTemplateId);
											rewardData2.Name = itemTemplate2.GetDisplayName();
											rewardData2.SpritePath = itemTemplate2.IconPath;
											rewardData2.Level = gameBalanceVars.RepeatingCharacterLevelRewards[m].startLevel;
											rewardData2.InventoryTemplate = itemTemplate2;
											rewardData2.Type = RewardUtils.GetRewardTypeFromInventoryItem(itemTemplate2);
											rewardData2.isRepeating = true;
											rewardData2.repeatLevels = gameBalanceVars.RepeatingCharacterLevelRewards[m].repeatingLevel;
											list3.Add(rewardData2);
										}
									}
								}
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
				for (int n = 0; n < list2.Count; n++)
				{
					EndGameRewardItem endGameRewardItem = UnityEngine.Object.Instantiate<EndGameRewardItem>(this.m_endGameReward);
					UIManager.ReparentTransform(endGameRewardItem.transform, this.m_RewardsGrid.transform);
					UIManager.SetGameObjectActive(endGameRewardItem, true, null);
					endGameRewardItem.Setup(list2[n], CharacterType.None);
					_MouseEventPasser mouseEventPasser = endGameRewardItem.m_tooltipHoverObj.gameObject.AddComponent<_MouseEventPasser>();
					mouseEventPasser.AddNewHandler(this.m_RewardsScrollRect);
				}
				for (int num5 = 0; num5 < list3.Count; num5++)
				{
					EndGameRewardItem endGameRewardItem2 = UnityEngine.Object.Instantiate<EndGameRewardItem>(this.m_endGameReward);
					UIManager.ReparentTransform(endGameRewardItem2.transform, this.m_RewardsGrid.transform);
					UIManager.SetGameObjectActive(endGameRewardItem2, true, null);
					endGameRewardItem2.Setup(list3[num5], playersOriginalActorData.m_characterType);
					_MouseEventPasser mouseEventPasser2 = endGameRewardItem2.m_tooltipHoverObj.gameObject.AddComponent<_MouseEventPasser>();
					mouseEventPasser2.AddNewHandler(this.m_RewardsScrollRect);
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
				this.NumRewardsEarned = list2.Count + list3.Count;
				this.m_rewardNumberText.text = this.NumRewardsEarned.ToString();
			}
		}
	}

	private void SetupStatRecapScreen()
	{
		Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
		int playerId = GameManager.Get().PlayerInfo.PlayerId;
		MatchResultsStats stats = GameplayUtils.GenerateStatsFromGame(teamViewing, playerId);
		UIGameOverScreen.SetupTeamMemberList(stats);
		this.SetupExpBars();
		this.SetupCurrencyAndInfluenceDisplays();
		this.SetupRankMode();
		this.SetupRewardsScreen();
	}

	private void SetupBadges()
	{
		if (!this.BadgesAreSet)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.SetupBadges()).MethodHandle;
			}
			if (this.NotificationArrived)
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
				if (GameManager.Get() != null)
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
					if (GameManager.Get().PlayerInfo != null && !this.m_results.BadgeAndParticipantsInfo.IsNullOrEmpty<BadgeAndParticipantInfo>())
					{
						foreach (BadgeAndParticipantInfo badgeAndParticipantInfo in this.m_results.BadgeAndParticipantsInfo)
						{
							if (badgeAndParticipantInfo.PlayerId == GameManager.Get().PlayerInfo.PlayerId)
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
								if (badgeAndParticipantInfo.BadgesEarned != null)
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
									List<BadgeInfo> badgesEarned = badgeAndParticipantInfo.BadgesEarned;
									if (UIGameOverScreen.<>f__am$cache1 == null)
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
										UIGameOverScreen.<>f__am$cache1 = delegate(BadgeInfo x, BadgeInfo y)
										{
											if (x == null && y == null)
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
													RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIGameOverScreen.<SetupBadges>m__8(BadgeInfo, BadgeInfo)).MethodHandle;
												}
												return 0;
											}
											if (x == null)
											{
												return 1;
											}
											if (y == null)
											{
												return -1;
											}
											GameBalanceVars.GameResultBadge badgeInfo = GameResultBadgeData.Get().GetBadgeInfo(x.BadgeId);
											GameBalanceVars.GameResultBadge badgeInfo2 = GameResultBadgeData.Get().GetBadgeInfo(y.BadgeId);
											if (badgeInfo == null && badgeInfo2 == null)
											{
												return 0;
											}
											if (badgeInfo == null)
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
												return 1;
											}
											if (badgeInfo2 == null)
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
												return -1;
											}
											if (badgeInfo.Quality == badgeInfo2.Quality)
											{
												return 0;
											}
											if (badgeInfo.Quality > badgeInfo2.Quality)
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
												return -1;
											}
											if (badgeInfo.Quality < badgeInfo2.Quality)
											{
												return 1;
											}
											return 0;
										};
									}
									badgesEarned.Sort(UIGameOverScreen.<>f__am$cache1);
									for (int i = 0; i < badgeAndParticipantInfo.BadgesEarned.Count; i++)
									{
										UIGameOverBadgeWidget uigameOverBadgeWidget = UnityEngine.Object.Instantiate<UIGameOverBadgeWidget>(this.m_BadgePrefab);
										uigameOverBadgeWidget.Setup(badgeAndParticipantInfo.BadgesEarned[i], GameManager.Get().PlayerInfo.CharacterType, badgeAndParticipantInfo.GlobalPercentiles);
										UIManager.ReparentTransform(uigameOverBadgeWidget.transform, this.m_PersonalHighlightBadgesContainer.transform);
										UIGameOverBadgeWidget uigameOverBadgeWidget2 = UnityEngine.Object.Instantiate<UIGameOverBadgeWidget>(this.m_BadgePrefab);
										uigameOverBadgeWidget2.Setup(badgeAndParticipantInfo.BadgesEarned[i], GameManager.Get().PlayerInfo.CharacterType, badgeAndParticipantInfo.GlobalPercentiles);
										UIManager.ReparentTransform(uigameOverBadgeWidget2.transform, this.m_StatPadgeBadgesContainer.transform);
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
									this.BadgesAreSet = true;
								}
								break;
							}
						}
					}
				}
			}
		}
	}

	private void SetupTopParticipants()
	{
		if (this.NotificationArrived)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.SetupTopParticipants()).MethodHandle;
			}
			int num = 0;
			if (!this.m_results.BadgeAndParticipantsInfo.IsNullOrEmpty<BadgeAndParticipantInfo>())
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
				List<TopParticipantSlot> list = new List<TopParticipantSlot>();
				IEnumerator enumerator = Enum.GetValues(typeof(TopParticipantSlot)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						object obj = enumerator.Current;
						TopParticipantSlot item = (TopParticipantSlot)obj;
						list.Add(item);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
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
						disposable.Dispose();
					}
				}
				List<TopParticipantSlot> list2 = list;
				if (UIGameOverScreen.<>f__am$cache2 == null)
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
					UIGameOverScreen.<>f__am$cache2 = delegate(TopParticipantSlot x, TopParticipantSlot y)
					{
						int num2 = BadgeAndParticipantInfo.ParticipantOrderDisplayPriority(x);
						int num3 = BadgeAndParticipantInfo.ParticipantOrderDisplayPriority(y);
						if (num2 == num3)
						{
							return 0;
						}
						int result;
						if (num2 > num3)
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
								RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIGameOverScreen.<SetupTopParticipants>m__9(TopParticipantSlot, TopParticipantSlot)).MethodHandle;
							}
							result = -1;
						}
						else
						{
							result = 1;
						}
						return result;
					};
				}
				list2.Sort(UIGameOverScreen.<>f__am$cache2);
				using (List<TopParticipantSlot>.Enumerator enumerator2 = list.GetEnumerator())
				{
					IL_183:
					while (enumerator2.MoveNext())
					{
						TopParticipantSlot topParticipantSlot = enumerator2.Current;
						using (List<BadgeAndParticipantInfo>.Enumerator enumerator3 = this.m_results.BadgeAndParticipantsInfo.GetEnumerator())
						{
							while (enumerator3.MoveNext())
							{
								BadgeAndParticipantInfo badgeAndParticipantInfo = enumerator3.Current;
								if (!badgeAndParticipantInfo.TopParticipationEarned.IsNullOrEmpty<TopParticipantSlot>())
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
									if (badgeAndParticipantInfo.TopParticipationEarned.Contains(topParticipantSlot))
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
										if (num < this.m_TopParticipantWidgets.Length)
										{
											this.m_TopParticipantWidgets[num].Setup(topParticipantSlot, badgeAndParticipantInfo);
											num++;
										}
										goto IL_183;
									}
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
			}
			for (int i = num; i < this.m_TopParticipantWidgets.Length; i++)
			{
				UIManager.SetGameObjectActive(this.m_TopParticipantWidgets[i], false, null);
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

	private void SetupStatsScreen()
	{
		UIGameOverStatWidget[] componentsInChildren = this.m_freelancerStatGrid.GetComponentsInChildren<UIGameOverStatWidget>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
		}
		componentsInChildren = this.m_generalStatGrid.GetComponentsInChildren<UIGameOverStatWidget>(true);
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[j].gameObject);
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.SetupStatsScreen()).MethodHandle;
		}
		componentsInChildren = this.m_firepowerStatGrid.GetComponentsInChildren<UIGameOverStatWidget>(true);
		for (int k = 0; k < componentsInChildren.Length; k++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[k].gameObject);
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
		componentsInChildren = this.m_supportStatGrid.GetComponentsInChildren<UIGameOverStatWidget>(true);
		for (int l = 0; l < componentsInChildren.Length; l++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[l].gameObject);
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
		componentsInChildren = this.m_frontlineStatGrid.GetComponentsInChildren<UIGameOverStatWidget>(true);
		for (int m = 0; m < componentsInChildren.Length; m++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[m].gameObject);
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
		if (this.m_statsAtBeginningOfMatch == null)
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
			if (ReplayPlayManager.Get() != null)
			{
				PersistedCharacterMatchData playbackMatchData = ReplayPlayManager.Get().GetPlaybackMatchData();
				if (playbackMatchData != null)
				{
					CharacterType firstPlayerCharacter = playbackMatchData.MatchComponent.GetFirstPlayerCharacter();
					CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(firstPlayerCharacter);
					this.m_CharacterImage.sprite = characterResourceLink.GetCharacterSelectIcon();
					for (int n = 0; n < 4; n++)
					{
						UIGameOverStatWidget uigameOverStatWidget = UnityEngine.Object.Instantiate<UIGameOverStatWidget>(this.m_freelancerStatPrefab);
						AbilityData component = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
						uigameOverStatWidget.SetupReplayFreelancerStat(firstPlayerCharacter, playbackMatchData.MatchFreelancerStats, n, component);
						UIManager.ReparentTransform(uigameOverStatWidget.transform, this.m_freelancerStatGrid.transform);
						this.m_GameOverStatWidgets.Add(uigameOverStatWidget);
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
					foreach (StatDisplaySettings.StatType typeOfStat in StatDisplaySettings.GeneralStats)
					{
						UIGameOverStatWidget uigameOverStatWidget2 = UnityEngine.Object.Instantiate<UIGameOverStatWidget>(this.m_generalStatPrefab);
						uigameOverStatWidget2.SetupReplayStat(playbackMatchData.MatchFreelancerStats, typeOfStat, firstPlayerCharacter);
						UIManager.ReparentTransform(uigameOverStatWidget2.transform, this.m_generalStatGrid.transform);
						this.m_GameOverStatWidgets.Add(uigameOverStatWidget2);
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
					foreach (StatDisplaySettings.StatType typeOfStat2 in StatDisplaySettings.FirepowerStats)
					{
						UIGameOverStatWidget uigameOverStatWidget3 = UnityEngine.Object.Instantiate<UIGameOverStatWidget>(this.m_generalStatPrefab);
						uigameOverStatWidget3.SetupReplayStat(playbackMatchData.MatchFreelancerStats, typeOfStat2, firstPlayerCharacter);
						UIManager.ReparentTransform(uigameOverStatWidget3.transform, this.m_firepowerStatGrid.transform);
						this.m_GameOverStatWidgets.Add(uigameOverStatWidget3);
					}
					foreach (StatDisplaySettings.StatType typeOfStat3 in StatDisplaySettings.SupportStats)
					{
						UIGameOverStatWidget uigameOverStatWidget4 = UnityEngine.Object.Instantiate<UIGameOverStatWidget>(this.m_generalStatPrefab);
						uigameOverStatWidget4.SetupReplayStat(playbackMatchData.MatchFreelancerStats, typeOfStat3, firstPlayerCharacter);
						UIManager.ReparentTransform(uigameOverStatWidget4.transform, this.m_supportStatGrid.transform);
						this.m_GameOverStatWidgets.Add(uigameOverStatWidget4);
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
					foreach (StatDisplaySettings.StatType typeOfStat4 in StatDisplaySettings.FrontlinerStats)
					{
						UIGameOverStatWidget uigameOverStatWidget5 = UnityEngine.Object.Instantiate<UIGameOverStatWidget>(this.m_generalStatPrefab);
						uigameOverStatWidget5.SetupReplayStat(playbackMatchData.MatchFreelancerStats, typeOfStat4, firstPlayerCharacter);
						UIManager.ReparentTransform(uigameOverStatWidget5.transform, this.m_frontlineStatGrid.transform);
						this.m_GameOverStatWidgets.Add(uigameOverStatWidget5);
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
					if (playbackMatchData.MatchFreelancerStats == null)
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
						return;
					}
					ClientGameManager.Get().CalculateFreelancerStats(playbackMatchData.MatchFreelancerStats.PersistedStatBucket, firstPlayerCharacter, playbackMatchData.MatchFreelancerStats, delegate(CalculateFreelancerStatsResponse response)
					{
						if (response.Success)
						{
							using (List<UIGameOverStatWidget>.Enumerator enumerator = this.m_GameOverStatWidgets.GetEnumerator())
							{
								while (enumerator.MoveNext())
								{
									UIGameOverStatWidget uigameOverStatWidget11 = enumerator.Current;
									if (uigameOverStatWidget11.DisplayStatType == UIGameOverStatWidget.StatDisplayType.None)
									{
										uigameOverStatWidget11.UpdatePercentiles(null);
									}
									else if (uigameOverStatWidget11.DisplayStatType == UIGameOverStatWidget.StatDisplayType.GeneralStat && !response.GlobalPercentiles.IsNullOrEmpty<KeyValuePair<StatDisplaySettings.StatType, PercentileInfo>>())
									{
										uigameOverStatWidget11.UpdatePercentiles(response.GlobalPercentiles[uigameOverStatWidget11.GeneralStatType]);
									}
									else if (uigameOverStatWidget11.DisplayStatType == UIGameOverStatWidget.StatDisplayType.FreelancerStat && !response.FreelancerSpecificPercentiles.IsNullOrEmpty<KeyValuePair<int, PercentileInfo>>())
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
											RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIGameOverScreen.<SetupStatsScreen>m__A(CalculateFreelancerStatsResponse)).MethodHandle;
										}
										uigameOverStatWidget11.UpdatePercentiles(response.FreelancerSpecificPercentiles[uigameOverStatWidget11.FreelancerStat]);
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
							}
						}
						else if (response.LocalizedFailure != null)
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
							TextConsole.Get().Write(response.LocalizedFailure.ToString(), ConsoleMessageType.SystemMessage);
						}
					});
					return;
				}
			}
			return;
		}
		ActorData playersOriginalActorData = UIGameOverScreen.GetPlayersOriginalActorData();
		if (playersOriginalActorData != null)
		{
			this.m_CharacterImage.sprite = playersOriginalActorData.\u000E().GetCharacterSelectIcon();
			this.m_PersonalHighlightsCharacterImage.sprite = this.m_CharacterImage.sprite;
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_PersonalHighlightsCharacterImage, false, null);
			UIManager.SetGameObjectActive(this.m_CharacterImage, false, null);
		}
		this.m_GameOverStatWidgets.Clear();
		ActorData playersOriginalActorData2 = UIGameOverScreen.GetPlayersOriginalActorData();
		FreelancerStats freelancerStats = playersOriginalActorData2.\u000E();
		ActorBehavior actorBehavior = playersOriginalActorData2.\u000E();
		for (int num5 = 0; num5 < freelancerStats.GetNumStats(); num5++)
		{
			UIGameOverStatWidget uigameOverStatWidget6 = UnityEngine.Object.Instantiate<UIGameOverStatWidget>(this.m_freelancerStatPrefab);
			uigameOverStatWidget6.SetupForFreelancerStats(this.m_statsAtBeginningOfMatch, actorBehavior, freelancerStats, num5, playersOriginalActorData2.\u000E());
			uigameOverStatWidget6.UpdatePercentiles(this.GetFreelancerStatPercentiles(num5));
			UIManager.ReparentTransform(uigameOverStatWidget6.transform, this.m_freelancerStatGrid.transform);
			this.m_GameOverStatWidgets.Add(uigameOverStatWidget6);
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
		foreach (StatDisplaySettings.StatType typeOfStat5 in StatDisplaySettings.GeneralStats)
		{
			UIGameOverStatWidget uigameOverStatWidget7 = UnityEngine.Object.Instantiate<UIGameOverStatWidget>(this.m_generalStatPrefab);
			uigameOverStatWidget7.SetupForAStat(this.m_statsAtBeginningOfMatch, actorBehavior, typeOfStat5);
			uigameOverStatWidget7.UpdatePercentiles(this.GetStatPercentiles(uigameOverStatWidget7.GeneralStatType));
			UIManager.ReparentTransform(uigameOverStatWidget7.transform, this.m_generalStatGrid.transform);
			this.m_GameOverStatWidgets.Add(uigameOverStatWidget7);
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
		foreach (StatDisplaySettings.StatType typeOfStat6 in StatDisplaySettings.FirepowerStats)
		{
			UIGameOverStatWidget uigameOverStatWidget8 = UnityEngine.Object.Instantiate<UIGameOverStatWidget>(this.m_generalStatPrefab);
			uigameOverStatWidget8.SetupForAStat(this.m_statsAtBeginningOfMatch, actorBehavior, typeOfStat6);
			uigameOverStatWidget8.UpdatePercentiles(this.GetStatPercentiles(uigameOverStatWidget8.GeneralStatType));
			UIManager.ReparentTransform(uigameOverStatWidget8.transform, this.m_firepowerStatGrid.transform);
			this.m_GameOverStatWidgets.Add(uigameOverStatWidget8);
		}
		foreach (StatDisplaySettings.StatType typeOfStat7 in StatDisplaySettings.SupportStats)
		{
			UIGameOverStatWidget uigameOverStatWidget9 = UnityEngine.Object.Instantiate<UIGameOverStatWidget>(this.m_generalStatPrefab);
			uigameOverStatWidget9.SetupForAStat(this.m_statsAtBeginningOfMatch, actorBehavior, typeOfStat7);
			uigameOverStatWidget9.UpdatePercentiles(this.GetStatPercentiles(uigameOverStatWidget9.GeneralStatType));
			UIManager.ReparentTransform(uigameOverStatWidget9.transform, this.m_supportStatGrid.transform);
			this.m_GameOverStatWidgets.Add(uigameOverStatWidget9);
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
		foreach (StatDisplaySettings.StatType typeOfStat8 in StatDisplaySettings.FrontlinerStats)
		{
			UIGameOverStatWidget uigameOverStatWidget10 = UnityEngine.Object.Instantiate<UIGameOverStatWidget>(this.m_generalStatPrefab);
			uigameOverStatWidget10.SetupForAStat(this.m_statsAtBeginningOfMatch, actorBehavior, typeOfStat8);
			uigameOverStatWidget10.UpdatePercentiles(this.GetStatPercentiles(uigameOverStatWidget10.GeneralStatType));
			UIManager.ReparentTransform(uigameOverStatWidget10.transform, this.m_frontlineStatGrid.transform);
			this.m_GameOverStatWidgets.Add(uigameOverStatWidget10);
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
		List<UIGameOverStatWidget> list = new List<UIGameOverStatWidget>();
		int num10 = 0;
		while (list.Count < this.m_PersonalHighlightWidgets.Length)
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
			if (num10 >= 3)
			{
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					goto IL_888;
				}
			}
			else
			{
				for (int num11 = 0; num11 < this.m_GameOverStatWidgets.Count; num11++)
				{
					if (!list.Contains(this.m_GameOverStatWidgets[num11]))
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
						if (num10 == 0)
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
							if (this.m_GameOverStatWidgets[num11].BeatRecord())
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
								list.Add(this.m_GameOverStatWidgets[num11]);
								goto IL_82F;
							}
						}
						if (num10 == 1 && this.m_GameOverStatWidgets[num11].BeatAverage())
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
							list.Add(this.m_GameOverStatWidgets[num11]);
						}
						else if (num10 >= 2)
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
							list.Add(this.m_GameOverStatWidgets[num11]);
						}
					}
					IL_82F:;
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
				num10++;
			}
		}
		IL_888:
		for (int num12 = 0; num12 < this.m_PersonalHighlightWidgets.Length; num12++)
		{
			if (num12 < list.Count)
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
				if (list[num12].DisplayStatType == UIGameOverStatWidget.StatDisplayType.FreelancerStat)
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
					this.m_PersonalHighlightWidgets[num12].SetupForFreelancerStats(this.m_statsAtBeginningOfMatch, actorBehavior, freelancerStats, list[num12].FreelancerStat, playersOriginalActorData2.\u000E());
				}
				else if (list[num12].DisplayStatType == UIGameOverStatWidget.StatDisplayType.GeneralStat)
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
					this.m_PersonalHighlightWidgets[num12].SetupForAStat(this.m_statsAtBeginningOfMatch, actorBehavior, list[num12].GeneralStatType);
				}
			}
			UIManager.SetGameObjectActive(this.m_PersonalHighlightWidgets[num12], false, null);
		}
	}

	private void SetupRankMode()
	{
		bool flag = this.m_gameType == GameType.Ranked;
		bool flag2 = flag;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.SetupRankMode()).MethodHandle;
			}
			TierPlacement tierCurrent = ClientGameManager.Get().TierCurrent;
			if (tierCurrent != null)
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
				if (tierCurrent.Tier != -1)
				{
					goto IL_57;
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
			flag2 = false;
			IL_57:
			if (flag2)
			{
				UIManager.SetGameObjectActive(this.m_rankModeLevelAnimator, false, null);
				float fillAmount = this.GetRankFillAmt(tierCurrent.Points * 0.01f);
				if (tierCurrent.Tier != 1)
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
					if (tierCurrent.Tier != 2)
					{
						goto IL_A0;
					}
				}
				fillAmount = 1f;
				IL_A0:
				this.m_rankNormalBar.fillAmount = fillAmount;
				this.m_rankDecreaseBar.fillAmount = fillAmount;
				this.m_rankIncreaseBar.fillAmount = fillAmount;
				int tier = tierCurrent.Tier;
				this.SetupTierDisplay(tier, tierCurrent.Points);
				this.m_rankPointsText.text = "0.0";
				UIManager.SetGameObjectActive(this.m_rankDecreaseBar, !this.SelfWon, null);
				UIManager.SetGameObjectActive(this.m_rankIncreaseBar, this.SelfWon, null);
			}
		}
		UIManager.SetGameObjectActive(this.m_rankModeLevelContainer, flag2, null);
	}

	private void SetupGGBoostScreen()
	{
		this.UpdateGGBoostPlayerList(false);
		this.m_numSelfGGpacksUsed = HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.NumberGGPacksUsed(GameManager.Get().PlayerInfo.Handle);
		for (int i = 0; i < this.m_ggButtonLevels.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_ggButtonLevels[i], i == this.m_numSelfGGpacksUsed, null);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.SetupGGBoostScreen()).MethodHandle;
		}
		int currentAmount = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack);
		for (int j = 0; j < this.m_ggButtonLevelsAnims.Length; j++)
		{
			if (this.m_ggButtonLevelsAnims[j].gameObject.activeInHierarchy)
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
				if (currentAmount > 0)
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
					UIAnimationEventManager.Get().PlayAnimation(this.m_ggButtonLevelsAnims[j], "GGBoostDefaultIN", null, string.Empty, 0, 0f, true, false, null, null);
				}
				else
				{
					UIAnimationEventManager.Get().PlayAnimation(this.m_ggButtonLevelsAnims[j], "GGBoostNoMoreDefaultIN", null, string.Empty, 0, 0f, true, false, null, null);
				}
			}
		}
		this.HandleBankBalanceChange(null);
	}

	public void UpdateGGBoostPlayerList(bool setGGLevel = true)
	{
		Team teamId = GameManager.Get().PlayerInfo.TeamId;
		int num = 0;
		int num2 = 0;
		if (HUD_UI.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.UpdateGGBoostPlayerList(bool)).MethodHandle;
			}
			using (List<LobbyPlayerInfo>.Enumerator enumerator = GameManager.Get().TeamInfo.TeamPlayerInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
					if (lobbyPlayerInfo.CharacterType != CharacterType.None)
					{
						int num3 = HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.NumberGGPacksUsed(lobbyPlayerInfo.Handle);
						if (lobbyPlayerInfo.TeamId != teamId)
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
							if (teamId == Team.Spectator && lobbyPlayerInfo.TeamId == Team.TeamA)
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
								if (num2 < this.m_redTeamGGProfiles.Length)
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
									UIManager.SetGameObjectActive(this.m_redTeamGGProfiles[num2], true, null);
									this.m_redTeamGGProfiles[num2].Setup(GameWideData.Get().GetCharacterResourceLink(lobbyPlayerInfo.CharacterType), lobbyPlayerInfo, true, true);
									UILoadscreenProfile uiloadscreenProfile = this.m_redTeamGGProfiles[num2];
									int ggbuttonLevel;
									if (setGGLevel)
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
										ggbuttonLevel = num3;
									}
									else
									{
										ggbuttonLevel = 0;
									}
									uiloadscreenProfile.SetGGButtonLevel(ggbuttonLevel);
									num2++;
									continue;
								}
								continue;
							}
						}
						if (num < this.m_blueTeamGGProfiles.Length)
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
							UIManager.SetGameObjectActive(this.m_blueTeamGGProfiles[num], true, null);
							this.m_blueTeamGGProfiles[num].Setup(GameWideData.Get().GetCharacterResourceLink(lobbyPlayerInfo.CharacterType), lobbyPlayerInfo, false, true);
							UILoadscreenProfile uiloadscreenProfile2 = this.m_blueTeamGGProfiles[num];
							int ggbuttonLevel2;
							if (setGGLevel)
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
								ggbuttonLevel2 = num3;
							}
							else
							{
								ggbuttonLevel2 = 0;
							}
							uiloadscreenProfile2.SetGGButtonLevel(ggbuttonLevel2);
							num++;
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
			}
		}
		for (int i = num; i < this.m_blueTeamGGProfiles.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_blueTeamGGProfiles[i], false, null);
		}
		for (int j = num2; j < this.m_redTeamGGProfiles.Length; j++)
		{
			UIManager.SetGameObjectActive(this.m_redTeamGGProfiles[j], false, null);
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

	private float GetRankFillAmt(float percent)
	{
		return percent * 0.833f + 0.082f;
	}

	private bool SetupTierDisplay(int tier, float tierPoints)
	{
		if (this.LastRankTierDisplayed == tier)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.SetupTierDisplay(int, float)).MethodHandle;
			}
			return false;
		}
		this.LastRankTierDisplayed = tier;
		string tierIconResource = UIRankedModeSelectScreen.GetTierIconResource(tier);
		string tierName = ClientGameManager.Get().GetTierName(GameType.Ranked, tier);
		if (!tierIconResource.IsNullOrEmpty())
		{
			this.m_rankIcon.sprite = (Sprite)Resources.Load(tierIconResource, typeof(Sprite));
			if (tierIconResource.ToLower().Contains("bronze"))
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
				this.m_rankDecreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Bronze_decrease", typeof(Sprite));
				this.m_rankIncreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Bronze_increase", typeof(Sprite));
				this.m_rankNormalBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Bronze_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("silver"))
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
				this.m_rankDecreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Silver_decrease", typeof(Sprite));
				this.m_rankIncreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Silver_increase", typeof(Sprite));
				this.m_rankNormalBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Silver_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("gold"))
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
				this.m_rankDecreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Gold_decrease", typeof(Sprite));
				this.m_rankIncreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Gold_increase", typeof(Sprite));
				this.m_rankNormalBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Gold_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("platinum"))
			{
				this.m_rankDecreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Platinum_decrease", typeof(Sprite));
				this.m_rankIncreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Platinum_increase", typeof(Sprite));
				this.m_rankNormalBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Platinum_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("diamond"))
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
				this.m_rankDecreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Diamond_decrease", typeof(Sprite));
				this.m_rankIncreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Diamond_increase", typeof(Sprite));
				this.m_rankNormalBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Diamond_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("master"))
			{
				this.m_rankDecreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Master_decrease", typeof(Sprite));
				this.m_rankIncreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Master_increase", typeof(Sprite));
				this.m_rankNormalBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Master_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("contender"))
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
				this.m_rankDecreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Contender_decrease", typeof(Sprite));
				this.m_rankIncreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Contender_increase", typeof(Sprite));
				this.m_rankNormalBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Contender_fill", typeof(Sprite));
			}
		}
		else
		{
			Log.Warning("Did not find icon for tier: " + tier, new object[0]);
		}
		string[] array = tierName.Split(new char[]
		{
			' '
		});
		if (array != null)
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
			if (array.Length > 0)
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
				this.m_rankTeirText.text = array[0];
			}
			if (array.Length > 1)
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
				this.m_rankLevelText.text = array[1];
				this.ShouldDisplayTierPoints = false;
			}
			else
			{
				this.m_rankLevelText.text = Mathf.RoundToInt(tierPoints).ToString();
				this.ShouldDisplayTierPoints = true;
			}
		}
		else
		{
			this.m_rankTeirText.text = tierName;
			this.m_rankLevelText.text = string.Empty;
			this.ShouldDisplayTierPoints = false;
		}
		return true;
	}

	public void SetVisible(bool visible)
	{
		this.IsVisible = visible;
		if (!this.IsVisible)
		{
			UIManager.SetGameObjectActive(this.m_GGBoostContainer, false, null);
			UIManager.SetGameObjectActive(this.m_TopBottomBarsContainer, false, null);
			UIManager.SetGameObjectActive(this.m_MouseClickContainer, false, null);
		}
	}

	private void CheckPreGameStats()
	{
		if (GameManager.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.CheckPreGameStats()).MethodHandle;
			}
			if (GameManager.Get().GameStatus == GameStatus.Started)
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
				if (GameManager.Get().GameInfo != null)
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
					if (GameManager.Get().GameInfo.GameConfig != null)
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
						if (GameManager.Get().GameInfo.GameConfig.InstanceSubType != null)
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
							if (GameManager.Get().PlayerInfo != null)
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
								if (!GameManager.Get().PlayerInfo.IsSpectator)
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
									if (ClientGameManager.Get().IsPlayerAccountDataAvailable())
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
										if (ClientGameManager.Get().IsPlayerCharacterDataAvailable(CharacterType.None))
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
											if (this.m_statsAtBeginningOfMatch == null)
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
												PersistedStatBucket persistedStatBucket = GameManager.Get().GameInfo.GameConfig.InstanceSubType.PersistedStatBucket;
												Dictionary<PersistedStatBucket, PersistedStats> persistedStatsDictionary = ClientGameManager.Get().GetPlayerCharacterData(GameManager.Get().PlayerInfo.CharacterType).ExperienceComponent.PersistedStatsDictionary;
												PersistedStats persistedStats;
												if (persistedStatsDictionary.ContainsKey(persistedStatBucket))
												{
													persistedStats = persistedStatsDictionary[persistedStatBucket];
												}
												else
												{
													persistedStats = new PersistedStats();
												}
												this.m_statsAtBeginningOfMatch = (PersistedStats)persistedStats.Clone();
											}
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}

	public void HandleMatchResultsNotification(MatchResultsNotification notification)
	{
		this.m_results = notification;
		if (this.m_results.FirstWinOccured)
		{
			QuestCompletePanel.Get().AddSpecialQuestNotification(GameBalanceVars.Get().FirstWinOfDayQuestId);
			this.m_results.QuestXpGained += this.m_results.FirstWinXpGained;
		}
		if (GameManager.Get().GameConfig != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.HandleMatchResultsNotification(MatchResultsNotification)).MethodHandle;
			}
			GameType gameType = GameManager.Get().GameConfig.GameType;
			if (gameType != GameType.Custom)
			{
				if (gameType == GameType.Tutorial)
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
					if (this.m_results.SeasonLevelAtStart > 1)
					{
						goto IL_108;
					}
				}
				PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
				SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(playerAccountData.QuestComponent.ActiveSeason);
				if (seasonTemplate != null)
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
					if (seasonTemplate.IsTutorial)
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
						UITutorialSeasonInterstitial.Get().Setup(seasonTemplate, playerAccountData.QuestComponent.SeasonLevel - 1, true);
					}
				}
				goto IL_115;
			}
			IL_108:
			UITutorialSeasonInterstitial.Get().SetVisible(false);
		}
		IL_115:
		if (!notification.BadgeAndParticipantsInfo.IsNullOrEmpty<BadgeAndParticipantInfo>())
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
			int playerId = GameManager.Get().PlayerInfo.PlayerId;
			BadgeAndParticipantInfo badgeAndParticipantInfo = notification.BadgeAndParticipantsInfo.Find((BadgeAndParticipantInfo p) => p.PlayerId == playerId);
			if (badgeAndParticipantInfo != null)
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
				if (!badgeAndParticipantInfo.GlobalPercentiles.IsNullOrEmpty<KeyValuePair<StatDisplaySettings.StatType, PercentileInfo>>())
				{
					using (Dictionary<StatDisplaySettings.StatType, PercentileInfo>.Enumerator enumerator = badgeAndParticipantInfo.GlobalPercentiles.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<StatDisplaySettings.StatType, PercentileInfo> kvPair = enumerator.Current;
							UIGameOverStatWidget uigameOverStatWidget = this.m_GameOverStatWidgets.Find((UIGameOverStatWidget p) => p.GeneralStatType == kvPair.Key && p.DisplayStatType == UIGameOverStatWidget.StatDisplayType.GeneralStat);
							if (uigameOverStatWidget != null)
							{
								uigameOverStatWidget.UpdatePercentiles(kvPair.Value);
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
				if (!badgeAndParticipantInfo.FreelancerSpecificPercentiles.IsNullOrEmpty<KeyValuePair<int, PercentileInfo>>())
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
					using (Dictionary<int, PercentileInfo>.Enumerator enumerator2 = badgeAndParticipantInfo.FreelancerSpecificPercentiles.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							KeyValuePair<int, PercentileInfo> ivPair = enumerator2.Current;
							UIGameOverStatWidget uigameOverStatWidget2 = this.m_GameOverStatWidgets.Find(delegate(UIGameOverStatWidget p)
							{
								bool result;
								if (p.FreelancerStat == ivPair.Key)
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
										RuntimeMethodHandle runtimeMethodHandle2 = methodof(UIGameOverScreen.<HandleMatchResultsNotification>c__AnonStorey2.<>m__0(UIGameOverStatWidget)).MethodHandle;
									}
									result = (p.DisplayStatType == UIGameOverStatWidget.StatDisplayType.FreelancerStat);
								}
								else
								{
									result = false;
								}
								return result;
							});
							if (uigameOverStatWidget2 != null)
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
								uigameOverStatWidget2.UpdatePercentiles(ivPair.Value);
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
		}
	}

	public void NotifySelfGGPackUsed()
	{
		this.RequestedToUseGGPack = false;
		if (this.m_currentSubState != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.NotifySelfGGPackUsed()).MethodHandle;
			}
			this.UpdateGGBoostPlayerList(true);
		}
	}

	private void HandleGGPackUsed(UseGGPackNotification notification)
	{
		if (this.m_currentSubState != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.HandleGGPackUsed(UseGGPackNotification)).MethodHandle;
			}
			this.UpdateGGBoostPlayerList(true);
		}
	}

	public void UpdateEndGameGGBonuses(int iso, float xp)
	{
		this.m_GGPack_XPMult = xp;
		this.SetWorldGGPackText(string.Format(StringUtil.TR("EndGameGGBonuses", "GameOver"), Mathf.RoundToInt((this.m_GGPack_XPMult - 1f) * 100f)));
		if (this.m_GGPack_XPMult < 2f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.UpdateEndGameGGBonuses(int, float)).MethodHandle;
			}
			UIAnimationEventManager.Get().PlayAnimation(this.m_PercentageAnimator, "GGBoostBluePercentageDefaultIN", null, string.Empty, 0, 0f, true, false, null, null);
		}
		else if (this.m_GGPack_XPMult < 3f)
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
			UIAnimationEventManager.Get().PlayAnimation(this.m_PercentageAnimator, "GGBoostSilverPercentageDefaultIN", null, string.Empty, 0, 0f, true, false, null, null);
		}
		else if (this.m_GGPack_XPMult < 4f)
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
			UIAnimationEventManager.Get().PlayAnimation(this.m_PercentageAnimator, "GGBoostGoldPercentageDefaultIN", null, string.Empty, 0, 0f, true, false, null, null);
		}
		else
		{
			UIAnimationEventManager.Get().PlayAnimation(this.m_PercentageAnimator, "GGBoostMaxPercentageDefaultIN", null, string.Empty, 0, 0f, true, false, null, null);
		}
	}

	public static void SetupTeamMemberList(MatchResultsStats stats)
	{
		UIGameOverPlayerEntry.PreSetupInitialization();
		UIGameStatsWindow.Get().SetupTeamMemberList(stats);
	}

	public void Setup(GameType gameType, GameResult gameResult, int myTeamScore, int enemyTeamScore)
	{
		this.m_gameResult = gameResult;
		this.m_gameType = gameType;
		ActorData playersOriginalActorData = UIGameOverScreen.GetPlayersOriginalActorData();
		if (playersOriginalActorData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.Setup(GameType, GameResult, int, int)).MethodHandle;
			}
			this.m_characterExpBarImage.sprite = playersOriginalActorData.\u000E().GetCharacterSelectIcon();
		}
		GameBalanceVars.PlayerBanner currentBackgroundBanner = ClientGameManager.Get().GetCurrentBackgroundBanner();
		if (currentBackgroundBanner != null)
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
			this.m_bannerBG.sprite = (Sprite)Resources.Load(currentBackgroundBanner.m_resourceString, typeof(Sprite));
		}
		GameBalanceVars.PlayerBanner currentForegroundBanner = ClientGameManager.Get().GetCurrentForegroundBanner();
		if (currentForegroundBanner != null)
		{
			this.m_bannerFG.sprite = (Sprite)Resources.Load(currentForegroundBanner.m_resourceString, typeof(Sprite));
		}
		if (UIGameStatsWindow.Get() != null)
		{
			UIGameStatsWindow.Get().SetVisible(false);
		}
		GameOverWorldObjects.Get().Setup(gameResult, this.FriendlyTeam, this.m_GGPack_XPMult);
		UIManager.SetGameObjectActive(this.m_redTeamVictoryContainer, !this.SelfWon, null);
		UIManager.SetGameObjectActive(this.m_blueTeamVictoryContainer, this.SelfWon, null);
		LobbyGameplayOverrides gameplayOverrides = GameManager.Get().GameplayOverrides;
		bool doActive = gameplayOverrides != null && gameplayOverrides.EnableFacebook;
		UIManager.SetGameObjectActive(this.m_shareFacebookButton, doActive, null);
		this.SetVisible(true);
		this.SetupLabelText(myTeamScore, enemyTeamScore);
		this.EstimatedNotificationArrivalTime = Time.unscaledTime + GameBalanceVars.Get().GGPackEndGameUsageTimer;
		this.m_currentSubState = new UIGameOverScreen.GameOverSubState(UIGameOverScreen.GameOverScreenState.VictoryDefeat, this.VictoryDefeatDisplayTimeDuration);
		this.ContinueBtnFailSafeTime = Time.unscaledTime;
		if (GameOverWorldObjects.Get() != null)
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
			GameOverWorldObjects.Get().SetVisible(true);
		}
		UIManager.SetGameObjectActive(this.m_ContinueBtn, false, null);
		UIManager.SetGameObjectActive(UIChatBox.Get().m_overconsPanel, false, null);
		UIChatBox.Get().m_overconsPanel.SetPanelOpen(false);
	}

	private int GetTotalCurrencyGainedFromQuests()
	{
		int num = 0;
		if (this.m_results.CurrencyRewards != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.GetTotalCurrencyGainedFromQuests()).MethodHandle;
			}
			for (int i = 0; i < this.m_results.CurrencyRewards.Count; i++)
			{
				num += this.m_results.CurrencyRewards[i].QuestGained;
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
		return num;
	}

	private int GetNormalBarXPTotal()
	{
		if (this.m_results != null)
		{
			return this.m_results.BaseXpGained + this.m_results.EventBonusXpGained + this.m_results.PlayWithFriendXpGained + this.m_results.QueueTimeXpGained + this.m_results.WinXpGained + this.m_results.FreelancerOwnedXPGained;
		}
		return 0;
	}

	private IEnumerable<UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo> HandleUpdateExpSubStateComplete(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo)
	{
		UIScreenManager.Get().EndAllLoopSounds();
		this.ContinueBtnFailSafeTime = Time.unscaledTime;
		bool flag = false;
		if (UpdateInfo.UpdateType == UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.InitialPause)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.HandleUpdateExpSubStateComplete(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.m_ContinueBtn, true, null);
			if (this.GetNormalBarXPTotal() <= 0)
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
				if (!this.HasCurrencyToDisplay())
				{
					flag = true;
					goto IL_78;
				}
			}
			return new UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo[]
			{
				new UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.NormalExpBar, 1.5f)
			};
		}
		IL_78:
		if (!flag)
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
			if (UpdateInfo.UpdateType != UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.NormalExpBar)
			{
				goto IL_10C;
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
		if (this.m_results.GGXpGained > 0)
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
			this.m_ggBonusXPAnimText.text = string.Format(StringUtil.TR("GGBonusXP", "GameOver"), this.m_results.GGXpGained);
			this.m_ggBonusController.Play("ResultsBonusIconGGDefaultIN");
			return new UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo[]
			{
				new UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.GGExpAnim, 0f)
			};
		}
		flag = true;
		IL_10C:
		if (!flag)
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
			if (UpdateInfo.UpdateType != UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.GGExpAnim)
			{
				goto IL_15D;
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
		if (this.m_results.GGXpGained > 0)
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
			return new UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo[]
			{
				new UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.GGExpBar, 1.5f)
			};
		}
		flag = true;
		IL_15D:
		if (UpdateInfo.UpdateType == UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.GGExpBar)
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
			if (this.m_results.QuestXpGained <= 0)
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
				if (this.GetTotalCurrencyGainedFromQuests() <= 0)
				{
					flag = true;
					goto IL_1B2;
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
			return new UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo[]
			{
				new UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.QuestBarPause, 0f)
			};
		}
		IL_1B2:
		if (!flag)
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
			if (UpdateInfo.UpdateType != UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.QuestBarPause)
			{
				goto IL_216;
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
		if (this.m_results.QuestXpGained <= 0)
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
			if (this.GetTotalCurrencyGainedFromQuests() <= 0)
			{
				flag = true;
				goto IL_216;
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
		return new UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo[]
		{
			new UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.QuestExp, 1.5f)
		};
		IL_216:
		if (!flag)
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
			if (UpdateInfo.UpdateType != UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.QuestExp)
			{
				goto IL_301;
			}
		}
		if (this.IsRankedGame)
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
			if (ClientGameManager.Get().TierCurrent != null)
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
				if (ClientGameManager.Get().TierCurrent.Tier != -1)
				{
					TierPlacement tierPlacement;
					if (this.SelfWon)
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
						tierPlacement = ClientGameManager.Get().TierChangeMax;
					}
					else
					{
						tierPlacement = ClientGameManager.Get().TierChangeMin;
					}
					if (ClientGameManager.Get().TierCurrent.Tier == tierPlacement.Tier)
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
						if (ClientGameManager.Get().TierCurrent.Points == tierPlacement.Points)
						{
							goto IL_301;
						}
					}
					return new UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo[]
					{
						new UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.RankPoints, 3f)
					};
				}
			}
		}
		IL_301:
		return null;
	}

	private void HandleUpdateExpSubStateUpdate(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo)
	{
		this.UpdateRankPoints(UpdateInfo);
		this.UpdateInfluenceObjects(UpdateInfo);
		this.UpdateCurrencyObjects(UpdateInfo);
		this.UpdateExperienceObjects(UpdateInfo);
	}

	private void UpdateCurrencyHelper(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo, UIGameOverScreen.CurrencyDisplayInfo DisplayInfo, CurrencyType CurrencyType)
	{
		int num = 0;
		int num2 = 0;
		if (UpdateInfo.UpdateType == UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.NormalExpBar)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.UpdateCurrencyHelper(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo, UIGameOverScreen.CurrencyDisplayInfo, CurrencyType)).MethodHandle;
			}
			num2 = DisplayInfo.GetTotalNormalCurrencyReward(CurrencyType);
		}
		else if (UpdateInfo.UpdateType == UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.GGExpBar)
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
			num = DisplayInfo.GetTotalNormalCurrencyReward(CurrencyType);
			num2 = DisplayInfo.GetTotalGGBoostCurrencyReward(CurrencyType);
		}
		else if (UpdateInfo.UpdateType == UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.QuestExp)
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
			num = DisplayInfo.GetTotalNormalCurrencyReward(CurrencyType) + DisplayInfo.GetTotalGGBoostCurrencyReward(CurrencyType);
			num2 = DisplayInfo.GetTotalQuestCurrencyReward(CurrencyType);
		}
		int num3 = num + (int)((float)num2 * UpdateInfo.PercentageProgress);
		DisplayInfo.m_currencyGainText.text = "+" + num3.ToString();
	}

	private void UpdateExpSubStateHelper(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo, UIGameOverScreen.XPDisplayInfo DisplayInfo, int TotalXPToGain, int XPGainFromPreviousState, int StartLevel, int XPGainedSoFar)
	{
		SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason);
		if (seasonTemplate.IsTutorial)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.UpdateExpSubStateHelper(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo, UIGameOverScreen.XPDisplayInfo, int, int, int, int)).MethodHandle;
			}
			if (DisplayInfo.XPBarType != UIGameOverScreen.XPDisplayInfo.BarXPType.Character)
			{
				return;
			}
		}
		int i = XPGainFromPreviousState + XPGainedSoFar;
		int num = StartLevel;
		int xpforType = UIGameOverScreen.XPDisplayInfo.GetXPForType(DisplayInfo.XPBarType, num);
		while (i >= xpforType)
		{
			i -= xpforType;
			num++;
			xpforType = UIGameOverScreen.XPDisplayInfo.GetXPForType(DisplayInfo.XPBarType, num);
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
		if (DisplayInfo.m_LastLevelDisplayed != num)
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
			int xpforType2 = UIGameOverScreen.XPDisplayInfo.GetXPForType(DisplayInfo.XPBarType, num - 1);
			if (UpdateInfo.UpdateType == UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.NormalExpBar)
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
				DisplayInfo.m_NormalXPGainSlider.fillAmount = 1f;
			}
			else if (UpdateInfo.UpdateType == UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.GGExpBar)
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
				DisplayInfo.m_GGXPSlider.fillAmount = 1f;
			}
			else if (UpdateInfo.UpdateType == UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.QuestExp)
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
				DisplayInfo.m_GGXPSlider.fillAmount = 1f;
			}
			DisplayInfo.m_barLevelLabel.text = (num - 1).ToString();
			DisplayInfo.m_XPLabel.text = xpforType2 + " / " + xpforType2;
			DisplayInfo.m_playingLevelUp = true;
			UIAnimationEventManager.Get().PlayAnimation(DisplayInfo.m_barLevelUpAnimator, "resultsAccountLevelUpDefaultIN", new UIAnimationEventManager.AnimationDoneCallback(DisplayInfo.LevelUpAnimDone), "resultsAccountLevelUpDefaultIDLE", 0, 0f, true, false, null, null);
			UpdateInfo.SetPaused(true);
			DisplayInfo.m_LastLevelDisplayed = num;
			DisplayInfo.CheckForRewardDisplay(num);
		}
		else
		{
			if (UpdateInfo.UpdateType == UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.NormalExpBar)
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
				DisplayInfo.m_NormalXPGainSlider.fillAmount = (float)i / (float)xpforType;
			}
			else if (UpdateInfo.UpdateType == UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.GGExpBar)
			{
				DisplayInfo.m_GGXPSlider.fillAmount = (float)i / (float)xpforType;
			}
			else if (UpdateInfo.UpdateType == UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.QuestExp)
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
				DisplayInfo.m_QuestXPSlider.fillAmount = (float)i / (float)xpforType;
			}
			DisplayInfo.m_barLevelLabel.text = num.ToString();
			DisplayInfo.m_XPLabel.text = i + " / " + xpforType;
		}
	}

	private void UpdateInfluenceObjects(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo)
	{
		if (UpdateInfo.UpdateType == UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.NormalExpBar)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.UpdateInfluenceObjects(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo)).MethodHandle;
			}
			if (this.m_results.FactionContributionAmounts != null)
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
				if (!UpdateInfo.IsPaused)
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
					int num = 0;
					using (Dictionary<string, int>.Enumerator enumerator = this.m_results.FactionContributionAmounts.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<string, int> keyValuePair = enumerator.Current;
							num += keyValuePair.Value;
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
					int num2 = (int)((float)num * UpdateInfo.PercentageProgress);
					this.m_influenceDisplay.m_currencyGainText.text = "+" + num2.ToString();
				}
			}
		}
	}

	private int GetDisplayTotalCurrency(UIGameOverScreen.CurrencyDisplayInfo info, CurrencyType CurrencyType)
	{
		if (info != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.GetDisplayTotalCurrency(UIGameOverScreen.CurrencyDisplayInfo, CurrencyType)).MethodHandle;
			}
			return info.GetTotalNormalCurrencyReward(CurrencyType) + info.GetTotalGGBoostCurrencyReward(CurrencyType) + info.GetTotalQuestCurrencyReward(CurrencyType);
		}
		return 0;
	}

	private bool HasCurrencyToDisplay()
	{
		return this.GetDisplayTotalCurrency(this.m_isoDisplay, CurrencyType.ISO) + this.GetDisplayTotalCurrency(this.m_freelancerCurrencyDisplay, CurrencyType.FreelancerCurrency) + this.GetDisplayTotalCurrency(this.m_rankedCurrencyDisplay, CurrencyType.RankedCurrency) > 0;
	}

	private void UpdateCurrencyObjects(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo)
	{
		if (UpdateInfo.UpdateType != UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.NormalExpBar)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.UpdateCurrencyObjects(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo)).MethodHandle;
			}
			if (UpdateInfo.UpdateType != UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.GGExpBar)
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
				if (UpdateInfo.UpdateType != UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.QuestExp)
				{
					return;
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
		}
		if (!UpdateInfo.IsPaused)
		{
			this.UpdateCurrencyHelper(UpdateInfo, this.m_isoDisplay, CurrencyType.ISO);
			this.UpdateCurrencyHelper(UpdateInfo, this.m_freelancerCurrencyDisplay, CurrencyType.FreelancerCurrency);
			this.UpdateCurrencyHelper(UpdateInfo, this.m_rankedCurrencyDisplay, CurrencyType.RankedCurrency);
		}
	}

	private void UpdateExperienceObjects(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo)
	{
		if (UpdateInfo.UpdateType == UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.NormalExpBar)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.UpdateExperienceObjects(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo)).MethodHandle;
			}
			if (!UpdateInfo.IsPaused)
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
				UIScreenManager.Get().PlayNormalXPLoop(true);
				int normalBarXPTotal = this.GetNormalBarXPTotal();
				int num = normalBarXPTotal + this.m_results.ConsumableXpGained;
				int xpgainedSoFar = Mathf.RoundToInt(UpdateInfo.PercentageProgress * (float)normalBarXPTotal);
				int num2 = Mathf.RoundToInt(UpdateInfo.PercentageProgress * (float)num);
				this.m_expGain.text = "+" + xpgainedSoFar.ToString();
				this.UpdateExpSubStateHelper(UpdateInfo, this.m_playerXPInfo, normalBarXPTotal, this.m_results.SeasonXpAtStart, this.m_results.SeasonLevelAtStart, xpgainedSoFar);
				if (this.m_results.NumCharactersPlayed > 1)
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
					num /= this.m_results.NumCharactersPlayed;
					num2 /= this.m_results.NumCharactersPlayed;
				}
				this.UpdateExpSubStateHelper(UpdateInfo, this.m_characterXPInfo, num, this.m_results.CharacterXpAtStart, this.m_results.CharacterLevelAtStart, num2);
			}
			else
			{
				UIScreenManager.Get().EndNormalXPLoop();
				if (!this.m_playerXPInfo.m_playingLevelUp)
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
					if (!this.m_characterXPInfo.m_playingLevelUp && !UINewReward.Get().RewardIsBeingAnnounced())
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
						UpdateInfo.SetPaused(false);
					}
				}
			}
		}
		else if (UpdateInfo.UpdateType == UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.GGExpBar)
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
			if (!UpdateInfo.IsPaused)
			{
				UIScreenManager.Get().PlayGGBoostXPLoop(true);
				int ggxpGained = this.m_results.GGXpGained;
				int num3 = ggxpGained;
				int normalBarXPTotal2 = this.GetNormalBarXPTotal();
				int num4 = normalBarXPTotal2 + this.m_results.ConsumableXpGained;
				int num5 = Mathf.RoundToInt(UpdateInfo.PercentageProgress * (float)ggxpGained);
				this.m_expGain.text = "+" + (normalBarXPTotal2 + num5).ToString();
				this.UpdateExpSubStateHelper(UpdateInfo, this.m_playerXPInfo, ggxpGained, this.m_results.SeasonXpAtStart + normalBarXPTotal2, this.m_results.SeasonLevelAtStart, num5);
				if (this.m_results.NumCharactersPlayed > 1)
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
					num3 += num4;
					num3 /= this.m_results.NumCharactersPlayed;
					num4 /= this.m_results.NumCharactersPlayed;
					num3 -= num4;
				}
				int xpgainedSoFar2 = Mathf.RoundToInt(UpdateInfo.PercentageProgress * (float)num3);
				this.UpdateExpSubStateHelper(UpdateInfo, this.m_characterXPInfo, num3, this.m_results.CharacterXpAtStart + num4, this.m_results.CharacterLevelAtStart, xpgainedSoFar2);
			}
			else
			{
				UIScreenManager.Get().EndGGXPLoop();
				if (!this.m_playerXPInfo.m_playingLevelUp && !this.m_characterXPInfo.m_playingLevelUp)
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
					if (!UINewReward.Get().RewardIsBeingAnnounced())
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
						UpdateInfo.SetPaused(false);
					}
				}
			}
		}
		else if (UpdateInfo.UpdateType == UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.QuestExp)
		{
			if (!UpdateInfo.IsPaused)
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
				UIScreenManager.Get().PlayGGBoostXPLoop(true);
				int questXpGained = this.m_results.QuestXpGained;
				int num6 = this.GetNormalBarXPTotal() + this.m_results.GGXpGained;
				int num7 = Mathf.RoundToInt(UpdateInfo.PercentageProgress * (float)questXpGained);
				this.m_expGain.text = "+" + (num6 + num7).ToString();
				this.UpdateExpSubStateHelper(UpdateInfo, this.m_playerXPInfo, questXpGained, this.m_results.SeasonXpAtStart + num6, this.m_results.SeasonLevelAtStart, num7);
			}
			else
			{
				UIScreenManager.Get().EndGGXPLoop();
				if (!this.m_playerXPInfo.m_playingLevelUp)
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
					if (!this.m_characterXPInfo.m_playingLevelUp)
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
						if (!UINewReward.Get().RewardIsBeingAnnounced())
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
							UpdateInfo.SetPaused(false);
						}
					}
				}
			}
		}
	}

	private bool IsTierContenderOrMaster(int tier)
	{
		bool result;
		if (tier != 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.IsTierContenderOrMaster(int)).MethodHandle;
			}
			result = (tier == 1);
		}
		else
		{
			result = true;
		}
		return result;
	}

	private void RankModeAnimDone()
	{
		this.RankLevelUpDownAnimating = false;
		if (this.SelfWon)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.RankModeAnimDone()).MethodHandle;
			}
			this.m_rankNormalBar.fillAmount = 0f;
			this.m_rankDecreaseBar.fillAmount = 0f;
			this.m_rankIncreaseBar.fillAmount = 0f;
		}
		else
		{
			this.m_rankNormalBar.fillAmount = 1f;
			this.m_rankDecreaseBar.fillAmount = 1f;
			this.m_rankIncreaseBar.fillAmount = 1f;
		}
	}

	private void UpdateRankPointsWin(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo)
	{
		if (!UpdateInfo.IsPaused && ClientGameManager.Get().TierCurrent != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.UpdateRankPointsWin(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo)).MethodHandle;
			}
			TierPlacement tierCurrent = ClientGameManager.Get().TierCurrent;
			TierPlacement tierChangeMax = ClientGameManager.Get().TierChangeMax;
			float num = 0f;
			if (tierChangeMax.Tier == tierCurrent.Tier)
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
				num = tierChangeMax.Points - tierCurrent.Points;
			}
			else
			{
				int num2 = tierCurrent.Tier;
				while (num2 != tierChangeMax.Tier)
				{
					if (num2 == tierCurrent.Tier)
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
						if (this.IsTierContenderOrMaster(num2))
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
							throw new Exception("Increasing tiers from master or contender when winning doesn't make sense.");
						}
						num += Mathf.Abs(100f - tierCurrent.Points);
					}
					else
					{
						num += 100f;
					}
					if (tierCurrent.Tier < tierChangeMax.Tier)
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
						num2++;
					}
					else
					{
						num2--;
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
				num += tierChangeMax.Points;
			}
			float num3 = UpdateInfo.PercentageProgress * num;
			float num4 = ClientGameManager.Get().TierCurrent.Points + num3;
			int num5 = ClientGameManager.Get().TierCurrent.Tier;
			float num6;
			if (this.IsTierContenderOrMaster(num5))
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
				num6 = float.MaxValue;
			}
			else
			{
				num6 = 100f;
			}
			float num7 = num6;
			while (num4 >= num7)
			{
				num4 -= num7;
				if (tierCurrent.Tier < tierChangeMax.Tier)
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
					num5++;
				}
				else
				{
					num5--;
				}
				float num8;
				if (this.IsTierContenderOrMaster(num5))
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
					num8 = float.MaxValue;
				}
				else
				{
					num8 = 100f;
				}
				num7 = num8;
			}
			if (this.SetupTierDisplay(num5, num4))
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
				this.RankLevelUpDownAnimating = true;
				UpdateInfo.SetPaused(true);
				this.m_rankUpDownText.text = StringUtil.TR("RankUp", "GameOver");
				UIAnimationEventManager.Get().PlayAnimation(this.m_rankModeLevelAnimator, "RankedLvlUp", new UIAnimationEventManager.AnimationDoneCallback(this.RankModeAnimDone), string.Empty, 0, 0f, true, false, null, null);
			}
			else
			{
				this.m_rankPointsText.text = "+" + num3.ToString("F1");
				if (this.ShouldDisplayTierPoints)
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
					this.m_rankLevelText.text = Mathf.RoundToInt(num4).ToString();
				}
				if (this.IsTierContenderOrMaster(num5))
				{
					this.m_rankIncreaseBar.fillAmount = 1f;
				}
				else
				{
					this.m_rankIncreaseBar.fillAmount = this.GetRankFillAmt(num4 / 100f);
				}
			}
		}
		else if (!this.RankLevelUpDownAnimating)
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
			UpdateInfo.SetPaused(false);
		}
	}

	private void UpdateRankPointsLose(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo)
	{
		TierPlacement tierCurrent = ClientGameManager.Get().TierCurrent;
		TierPlacement tierChangeMin = ClientGameManager.Get().TierChangeMin;
		if (!UpdateInfo.IsPaused)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.UpdateRankPointsLose(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo)).MethodHandle;
			}
			if (tierCurrent != null)
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
				if (tierChangeMin != null)
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
					float num = 0f;
					if (tierChangeMin.Tier == tierCurrent.Tier)
					{
						num = tierCurrent.Points - tierChangeMin.Points;
					}
					else
					{
						int num2 = tierCurrent.Tier;
						while (num2 != tierChangeMin.Tier)
						{
							if (num2 == tierCurrent.Tier)
							{
								num += tierCurrent.Points;
							}
							else
							{
								if (this.IsTierContenderOrMaster(num2))
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
									throw new Exception("Decreasing tier to master or contender when losing doesn't make sense.");
								}
								num += 100f;
							}
							if (tierCurrent.Tier < tierChangeMin.Tier)
							{
								num2++;
							}
							else
							{
								num2--;
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
						num += Mathf.Abs(100f - tierChangeMin.Points);
					}
					float num3 = UpdateInfo.PercentageProgress * num;
					float num4 = ClientGameManager.Get().TierCurrent.Points - num3;
					int num5 = ClientGameManager.Get().TierCurrent.Tier;
					while (num4 <= 0f)
					{
						if (tierCurrent.Tier < tierChangeMin.Tier)
						{
							if (UIRankedModeSelectScreen.IsRatchetTier(tierCurrent.Tier))
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
								num4 = 0f;
								goto IL_1ED;
							}
							num5++;
						}
						else
						{
							if (tierCurrent.Tier <= tierChangeMin.Tier)
							{
								num4 = 0f;
								goto IL_1ED;
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
							num5--;
						}
						if (this.IsTierContenderOrMaster(num5))
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
							Log.Error("How did this happen? Should have been caught in an exception earlier", new object[0]);
						}
						num4 += 100f;
						continue;
						IL_1ED:
						if (this.SetupTierDisplay(num5, num4))
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
							this.RankLevelUpDownAnimating = true;
							UpdateInfo.SetPaused(true);
							this.m_rankUpDownText.text = StringUtil.TR("RankDown", "GameOver");
							UIAnimationEventManager.Get().PlayAnimation(this.m_rankModeLevelAnimator, "RankedLvlDown", new UIAnimationEventManager.AnimationDoneCallback(this.RankModeAnimDone), string.Empty, 0, 0f, true, false, null, null);
						}
						else
						{
							this.m_rankPointsText.text = "-" + num3.ToString("F1");
							if (this.ShouldDisplayTierPoints)
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
								this.m_rankLevelText.text = Mathf.RoundToInt(num4).ToString();
							}
							if (this.IsTierContenderOrMaster(num5))
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
								this.m_rankNormalBar.fillAmount = 1f;
							}
							else
							{
								this.m_rankNormalBar.fillAmount = this.GetRankFillAmt(num4 / 100f);
							}
						}
						return;
					}
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						goto IL_1ED;
					}
				}
			}
		}
		if (!this.RankLevelUpDownAnimating)
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
			UpdateInfo.SetPaused(false);
		}
	}

	private void UpdateRankPoints(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo)
	{
		if (UpdateInfo.UpdateType == UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.RankPoints)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.UpdateRankPoints(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo)).MethodHandle;
			}
			if (ClientGameManager.Get().TierCurrent != null && ClientGameManager.Get().TierCurrent.Tier > -1)
			{
				if (this.SelfWon)
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
					this.UpdateRankPointsWin(UpdateInfo);
				}
				else
				{
					this.UpdateRankPointsLose(UpdateInfo);
				}
			}
		}
	}

	public bool DoNextGGBoostRecapDisplay()
	{
		bool flag = false;
		for (int i = 0; i < this.m_blueTeamGGProfiles.Length; i++)
		{
			if (this.m_blueTeamGGProfiles[i].GetPlayerInfo() != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.DoNextGGBoostRecapDisplay()).MethodHandle;
				}
				int num = HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.NumberGGPacksUsed(this.m_blueTeamGGProfiles[i].GetPlayerInfo().Handle);
				if (!this.m_blueTeamGGProfiles[i].GetPlayerInfo().IsRemoteControlled)
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
					if (num > 0)
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
						if (this.m_blueTeamGGProfiles[i].CurrentGGPackLevel != num)
						{
							if (flag)
							{
								return true;
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
							flag = true;
							this.m_blueTeamGGProfiles[i].SetGGButtonLevel(num);
						}
					}
				}
			}
		}
		for (int j = 0; j < this.m_redTeamGGProfiles.Length; j++)
		{
			if (this.m_redTeamGGProfiles[j].GetPlayerInfo() != null)
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
				int num2 = HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.NumberGGPacksUsed(this.m_redTeamGGProfiles[j].GetPlayerInfo().Handle);
				if (!this.m_redTeamGGProfiles[j].GetPlayerInfo().IsRemoteControlled && num2 > 0)
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
					if (this.m_redTeamGGProfiles[j].CurrentGGPackLevel != num2)
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
						if (flag)
						{
							return true;
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
						flag = true;
						this.m_redTeamGGProfiles[j].SetGGButtonLevel(num2);
					}
				}
			}
		}
		return false;
	}

	private void HandleGGBoostSubStateUpdate(UIGameOverScreen.GameOverGGSubState.GGBoosts CurrentSubState, float percentToDisplay)
	{
		if (CurrentSubState == UIGameOverScreen.GameOverGGSubState.GGBoosts.UsageTimer)
		{
			this.m_GGBoostTimer.fillAmount = percentToDisplay;
		}
	}

	private void HandleNewGGBoostSubState(UIGameOverScreen.GameOverGGSubState SubState, UIGameOverScreen.GameOverGGSubState.GGBoosts NewSubState)
	{
		if (NewSubState == UIGameOverScreen.GameOverGGSubState.GGBoosts.UsageTimer)
		{
			this.GGBoostContainerAnimator.Play("GGBonusScreenDefaultIDLE");
			int currentAmount = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack);
			this.m_worldGGBtnHitBox.SetClickable(currentAmount > 0);
		}
		else if (NewSubState == UIGameOverScreen.GameOverGGSubState.GGBoosts.FadeOut)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.HandleNewGGBoostSubState(UIGameOverScreen.GameOverGGSubState, UIGameOverScreen.GameOverGGSubState.GGBoosts)).MethodHandle;
			}
			this.m_worldGGBtnHitBox.SetClickable(false);
			UIAnimationEventManager.Get().PlayAnimation(this.GGBoostContainerAnimator, "GGBonusScreenDefaultOUT", new UIAnimationEventManager.AnimationDoneCallback(SubState.FadeoutAnimDone), string.Empty, 0, 0f, true, false, null, null);
		}
	}

	public void UpdateGameOverSubStateObjects(UIGameOverScreen.GameOverSubState StateToBeUpdated)
	{
		if (StateToBeUpdated.SubStateType == UIGameOverScreen.GameOverScreenState.Stats)
		{
			int num = Mathf.FloorToInt((float)this.m_GameOverStatWidgets.Count * StateToBeUpdated.PercentageProgress);
			for (int i = 0; i < num; i++)
			{
				this.m_GameOverStatWidgets[i].SetHighlight();
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.UpdateGameOverSubStateObjects(UIGameOverScreen.GameOverSubState)).MethodHandle;
			}
		}
		if (StateToBeUpdated.SubStateType == UIGameOverScreen.GameOverScreenState.Accolades)
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
			int num2 = 0;
			if (this.m_results.BadgeAndParticipantsInfo != null)
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
				int num3 = 0;
				if (!this.m_results.BadgeAndParticipantsInfo.IsNullOrEmpty<BadgeAndParticipantInfo>())
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
					using (List<BadgeAndParticipantInfo>.Enumerator enumerator = this.m_results.BadgeAndParticipantsInfo.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BadgeAndParticipantInfo badgeAndParticipantInfo = enumerator.Current;
							if (!badgeAndParticipantInfo.TopParticipationEarned.IsNullOrEmpty<TopParticipantSlot>())
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
								num3 += badgeAndParticipantInfo.TopParticipationEarned.Count;
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
				num2 = Mathf.FloorToInt((float)num3 * Mathf.Clamp01((StateToBeUpdated.PercentageProgress - this.TopPartipantFrontTimePadPercentage) / (1f - (this.TopPartipantFrontTimePadPercentage + this.TopPartipantEndTimePadPercentage))));
			}
			for (int j = 0; j < this.m_TopParticipantWidgets.Length; j++)
			{
				if (j < num2)
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
					if (!this.m_TopParticipantWidgets[j].gameObject.activeSelf)
					{
						UIFrontEnd.PlaySound(FrontEndButtonSounds.EndGameBadgeBasic);
					}
					UIManager.SetGameObjectActive(this.m_TopParticipantWidgets[j], true, null);
				}
				else
				{
					UIManager.SetGameObjectActive(this.m_TopParticipantWidgets[j], false, null);
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
		if (StateToBeUpdated.SubStateType == UIGameOverScreen.GameOverScreenState.PersonalHighlights)
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
			int num4 = Mathf.FloorToInt((float)this.m_PersonalHighlightWidgets.Length * Mathf.Clamp01((StateToBeUpdated.PercentageProgress - this.PersonalHighlightsFrontTimePadPercentage) / (1f - (this.PersonalHighlightsFrontTimePadPercentage + this.PersonalHighlightsEndTimePadPercentage))));
			for (int k = 0; k < this.m_PersonalHighlightWidgets.Length; k++)
			{
				UIManager.SetGameObjectActive(this.m_PersonalHighlightWidgets[k], true, null);
				if (k < num4)
				{
					if (!this.m_PersonalHighlightWidgets[k].HighlightDone)
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
						UIFrontEnd.PlaySound(FrontEndButtonSounds.EndGameBadgeAchievement);
					}
					this.m_PersonalHighlightWidgets[k].SetHighlight();
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
		if (StateToBeUpdated.SubStateType == UIGameOverScreen.GameOverScreenState.MissionNotifications)
		{
			int num5 = Mathf.FloorToInt((float)QuestCompletePanel.Get().TotalQuestsToDisplayForGameOver() * StateToBeUpdated.PercentageProgress);
			for (int l = 0; l < num5; l++)
			{
				QuestCompletePanel.Get().DisplayGameOverQuestComplete(l);
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

	public static Sprite GetRewardSprite(RewardUtils.RewardData reward)
	{
		return (Sprite)Resources.Load(reward.SpritePath, typeof(Sprite));
	}

	public void NotifySeasonTutorialScreenClosed()
	{
		if (this.m_currentSubState is UIGameOverScreen.GameOverTutorialGames)
		{
			(this.m_currentSubState as UIGameOverScreen.GameOverTutorialGames).NotifyCloseClicked();
		}
	}

	private void DoGGBoostSubstate()
	{
		this.SetupGGBoostScreen();
		this.HandleBankBalanceChange(null);
		this.m_currentSubState = new UIGameOverScreen.GameOverGGSubState(this.EstimatedNotificationArrivalTime);
	}

	private void DoTutorialSeasonSubstate()
	{
		UITutorialSeasonInterstitial.Get().SetVisible(true);
		this.m_currentSubState = new UIGameOverScreen.GameOverTutorialGames();
	}

	private void DoAccoladesDisplay()
	{
		this.SetupStatRecapScreen();
		this.SetupStatsScreen();
		this.SetupTopParticipants();
		this.SetupBadges();
		this.m_currentSubState = new UIGameOverScreen.GameOverSubState(UIGameOverScreen.GameOverScreenState.ResultsScreenPause, 0.01f);
	}

	private void DoExperienceBarSubstate()
	{
		this.SetupStatRecapScreen();
		this.m_currentSubState = new UIGameOverScreen.GameOverExperienceUpdateSubState(new List<UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo>
		{
			new UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType.InitialPause, 0.01f)
		});
	}

	private void RefreshHeaderButtonClickability()
	{
		if (this.m_currentSubState != null && this.m_currentSubState.SubStateType == UIGameOverScreen.GameOverScreenState.Done)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.RefreshHeaderButtonClickability()).MethodHandle;
			}
			GameManager gameManager = GameManager.Get();
			bool flag;
			if (gameManager != null)
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
				if (gameManager.PlayerInfo != null)
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
					flag = gameManager.PlayerInfo.IsSpectator;
					goto IL_75;
				}
			}
			flag = false;
			IL_75:
			bool flag2 = flag;
			bool flag3;
			if (ReplayPlayManager.Get() != null)
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
				flag3 = ReplayPlayManager.Get().IsPlayback();
			}
			else
			{
				flag3 = false;
			}
			bool flag4;
			if (!flag3)
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
				flag4 = !flag2;
			}
			else
			{
				flag4 = true;
			}
			bool flag5 = flag4;
			bool flag6 = this.NumRewardsEarned > 0;
			if (this.BadgesAreActive)
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
				this.m_AccoladesHeaderBtn.spriteController.SetClickable(true);
				UIManager.SetGameObjectActive(this.m_AccoladesHeaderBtn.spriteController.m_defaultImage, true, null);
			}
			else
			{
				this.m_AccoladesHeaderBtn.spriteController.SetClickable(false);
				UIManager.SetGameObjectActive(this.m_AccoladesHeaderBtn.spriteController.m_defaultImage, false, null);
				UIManager.SetGameObjectActive(this.m_AccoladesHeaderBtn.spriteController.m_hoverImage, false, null);
				UIManager.SetGameObjectActive(this.m_AccoladesHeaderBtn.spriteController.m_pressedImage, false, null);
			}
			this.m_StatsHeaderBtn.spriteController.SetClickable(flag5);
			UIManager.SetGameObjectActive(this.m_StatsHeaderBtn.spriteController.m_defaultImage, flag5, null);
			UIManager.SetGameObjectActive(this.m_StatsHeaderBtn.spriteController.m_hoverImage, flag5, null);
			UIManager.SetGameObjectActive(this.m_StatsHeaderBtn.spriteController.m_pressedImage, flag5, null);
			this.m_RewardsHeaderBtn.spriteController.SetClickable(flag6);
			UIManager.SetGameObjectActive(this.m_RewardsHeaderBtn.spriteController.m_defaultImage, flag6, null);
			UIManager.SetGameObjectActive(this.m_RewardsHeaderBtn.spriteController.m_hoverImage, flag6, null);
			UIManager.SetGameObjectActive(this.m_RewardsHeaderBtn.spriteController.m_pressedImage, flag6, null);
			this.m_ScoreHeaderBtn.spriteController.SetClickable(true);
			UIManager.SetGameObjectActive(this.m_ScoreHeaderBtn.spriteController.m_defaultImage, true, null);
			UIManager.SetGameObjectActive(this.m_ScoreHeaderBtn.spriteController.m_hoverImage, true, null);
			UIManager.SetGameObjectActive(this.m_ScoreHeaderBtn.spriteController.m_pressedImage, true, null);
		}
		else
		{
			this.m_AccoladesHeaderBtn.spriteController.SetClickable(false);
			UIManager.SetGameObjectActive(this.m_AccoladesHeaderBtn.spriteController.m_defaultImage, false, null);
			UIManager.SetGameObjectActive(this.m_AccoladesHeaderBtn.spriteController.m_hoverImage, false, null);
			UIManager.SetGameObjectActive(this.m_AccoladesHeaderBtn.spriteController.m_pressedImage, false, null);
			this.m_StatsHeaderBtn.spriteController.SetClickable(false);
			UIManager.SetGameObjectActive(this.m_StatsHeaderBtn.spriteController.m_defaultImage, false, null);
			UIManager.SetGameObjectActive(this.m_StatsHeaderBtn.spriteController.m_hoverImage, false, null);
			UIManager.SetGameObjectActive(this.m_StatsHeaderBtn.spriteController.m_pressedImage, false, null);
			this.m_RewardsHeaderBtn.spriteController.SetClickable(false);
			UIManager.SetGameObjectActive(this.m_RewardsHeaderBtn.spriteController.m_defaultImage, false, null);
			UIManager.SetGameObjectActive(this.m_RewardsHeaderBtn.spriteController.m_hoverImage, false, null);
			UIManager.SetGameObjectActive(this.m_RewardsHeaderBtn.spriteController.m_pressedImage, false, null);
			this.m_ScoreHeaderBtn.spriteController.SetClickable(false);
			UIManager.SetGameObjectActive(this.m_ScoreHeaderBtn.spriteController.m_defaultImage, false, null);
			UIManager.SetGameObjectActive(this.m_ScoreHeaderBtn.spriteController.m_hoverImage, false, null);
			UIManager.SetGameObjectActive(this.m_ScoreHeaderBtn.spriteController.m_pressedImage, false, null);
		}
	}

	private void DoStatsSubstate()
	{
		this.SetupStatRecapScreen();
		this.SetupStatsScreen();
		this.SetupBadges();
		this.m_currentSubState = new UIGameOverScreen.GameOverSubState(UIGameOverScreen.GameOverScreenState.Stats, this.StatsDisplayTimeDuration);
		UITutorialPanel.Get().HideTutorialPassedStamp();
	}

	private void CheckIfCurrentStateIsDone()
	{
		if (this.m_currentSubState.IsDone())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.CheckIfCurrentStateIsDone()).MethodHandle;
			}
			bool visible = false;
			bool doActive = false;
			bool flag = false;
			bool flag2 = false;
			bool doActive2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			GameManager gameManager = GameManager.Get();
			bool flag6;
			if (gameManager != null)
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
				if (gameManager.PlayerInfo != null)
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
					flag6 = gameManager.PlayerInfo.IsSpectator;
					goto IL_76;
				}
			}
			flag6 = false;
			IL_76:
			bool flag7 = flag6;
			bool flag8 = ReplayPlayManager.Get() != null && ReplayPlayManager.Get().IsPlayback();
			if (this.m_currentSubState.SubStateType == UIGameOverScreen.GameOverScreenState.VictoryDefeat)
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
				bool flag9 = GameManager.IsGameTypeValidForGGPack(this.m_gameType);
				if (flag9)
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
					doActive = true;
					this.DoGGBoostSubstate();
				}
				else
				{
					this.m_currentSubState = new UIGameOverScreen.GameOverVictoryDefeatWaitingSubState();
				}
			}
			else if (this.m_currentSubState.SubStateType == UIGameOverScreen.GameOverScreenState.VictoryDefeatWaitingForNotification)
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
				doActive2 = true;
				SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason);
				if (seasonTemplate.IsTutorial)
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
					this.DoTutorialSeasonSubstate();
				}
				else if (this.BadgesAreActive)
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
					flag3 = true;
					this.DoAccoladesDisplay();
				}
				else
				{
					flag4 = (flag8 || !flag7);
					flag = true;
					flag2 = true;
					this.DoStatsSubstate();
				}
			}
			else if (this.m_currentSubState.SubStateType == UIGameOverScreen.GameOverScreenState.GGBoostUsage)
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
				doActive2 = true;
				SeasonTemplate seasonTemplate2 = SeasonWideData.Get().GetSeasonTemplate(ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason);
				if (seasonTemplate2.IsTutorial)
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
					this.DoTutorialSeasonSubstate();
				}
				else if (this.BadgesAreActive)
				{
					flag3 = true;
					this.DoAccoladesDisplay();
				}
				else
				{
					flag4 = (flag8 || !flag7);
					flag = true;
					flag2 = true;
					this.DoStatsSubstate();
				}
			}
			else if (this.m_currentSubState.SubStateType == UIGameOverScreen.GameOverScreenState.TutorialTenGames)
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
				doActive2 = true;
				if (this.BadgesAreActive)
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
					flag3 = true;
					this.DoAccoladesDisplay();
				}
				else
				{
					bool flag10;
					if (!flag8)
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
						flag10 = !flag7;
					}
					else
					{
						flag10 = true;
					}
					flag4 = flag10;
					flag = true;
					flag2 = true;
					this.DoStatsSubstate();
				}
			}
			else if (this.m_currentSubState.SubStateType == UIGameOverScreen.GameOverScreenState.ResultsScreenPause)
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
				doActive2 = true;
				flag3 = true;
				UIManager.SetGameObjectActive(this.m_TopParticipantsAnimator, true, null);
				this.m_AccoladesHeaderBtn.SetSelected(true, false, "SelectedIN", "SelectedOUT");
				this.m_StatsHeaderBtn.SetSelected(false, false, "SelectedIN", "SelectedOUT");
				this.m_RewardsHeaderBtn.SetSelected(false, false, "SelectedIN", "SelectedOUT");
				this.m_ScoreHeaderBtn.SetSelected(false, false, "SelectedIN", "SelectedOUT");
				this.m_currentSubState = new UIGameOverScreen.GameOverSubState(UIGameOverScreen.GameOverScreenState.Accolades, this.TopParticipantDisplayTimeDuration);
				UITutorialPanel.Get().HideTutorialPassedStamp();
			}
			else if (this.m_currentSubState.SubStateType == UIGameOverScreen.GameOverScreenState.Accolades)
			{
				doActive2 = true;
				flag3 = true;
				UIManager.SetGameObjectActive(this.m_PersonalHighlightsAnimator, true, null);
				this.m_currentSubState = new UIGameOverScreen.GameOverSubState(UIGameOverScreen.GameOverScreenState.PersonalHighlights, this.PersonalHighlightsDisplayTimeDuration);
				this.m_currentSubState.SetPercentageClickToSkip(this.PersonalHighlightsClickSkipPercentage);
			}
			else if (this.m_currentSubState.SubStateType == UIGameOverScreen.GameOverScreenState.PersonalHighlights)
			{
				flag4 = (flag8 || !flag7);
				flag = true;
				flag2 = true;
				this.DoStatsSubstate();
			}
			else if (this.m_currentSubState.SubStateType == UIGameOverScreen.GameOverScreenState.MissionNotifications)
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
				bool flag11;
				if (!flag8)
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
					flag11 = !flag7;
				}
				else
				{
					flag11 = true;
				}
				flag4 = flag11;
				flag = true;
				flag2 = true;
				this.DoExperienceBarSubstate();
			}
			else if (this.m_currentSubState.SubStateType == UIGameOverScreen.GameOverScreenState.Stats)
			{
				bool flag12;
				if (!flag8)
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
					flag12 = !flag7;
				}
				else
				{
					flag12 = true;
				}
				flag4 = flag12;
				flag = true;
				flag2 = true;
				if (QuestCompletePanel.Get().TotalQuestsToDisplayForGameOver() > 0)
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
					this.m_currentSubState = new UIGameOverScreen.GameOverSubState(UIGameOverScreen.GameOverScreenState.MissionNotifications, 1f);
				}
				else
				{
					this.DoExperienceBarSubstate();
				}
			}
			else if (this.m_currentSubState.SubStateType == UIGameOverScreen.GameOverScreenState.ExperienceBars)
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
				flag = true;
				flag2 = true;
				bool flag13;
				if (!flag8)
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
					flag13 = !flag7;
				}
				else
				{
					flag13 = true;
				}
				flag4 = flag13;
				flag5 = !flag4;
				UIManager.SetGameObjectActive(this.m_ContinueBtn, true, null);
				this.m_currentSubState = new UIGameOverScreen.GameOverSubState(UIGameOverScreen.GameOverScreenState.Done, 0f);
			}
			else if (this.m_currentSubState.SubStateType == UIGameOverScreen.GameOverScreenState.Rewards)
			{
				flag = true;
				flag2 = true;
			}
			if (GameOverWorldObjects.Get() != null)
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
				GameOverWorldObjects.Get().SetVisible(visible);
			}
			UIManager.SetGameObjectActive(this.m_GGBoostContainer, doActive, null);
			UIManager.SetGameObjectActive(this.m_MouseClickContainer, doActive2, null);
			Component topBottomBarsContainer = this.m_TopBottomBarsContainer;
			bool doActive3;
			if (!flag2)
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
				doActive3 = flag;
			}
			else
			{
				doActive3 = true;
			}
			UIManager.SetGameObjectActive(topBottomBarsContainer, doActive3, null);
			UIManager.SetGameObjectActive(this.m_TopBarContainer, flag, null);
			UIManager.SetGameObjectActive(this.m_BottomBarContainer, flag2, null);
			UIManager.SetGameObjectActive(this.m_AccoladesAnimator, flag3, null);
			UIManager.SetGameObjectActive(this.m_StatsContainer, flag4, null);
			UIGameStatsWindow.Get().SetToggleStatsVisible(flag5, false);
			this.m_AccoladesHeaderBtn.SetSelected(flag3, false, "SelectedIN", "SelectedOUT");
			this.m_StatsHeaderBtn.SetSelected(flag4, false, "SelectedIN", "SelectedOUT");
			this.m_RewardsHeaderBtn.SetSelected(false, false, "SelectedIN", "SelectedOUT");
			this.m_ScoreHeaderBtn.SetSelected(flag5, false, "SelectedIN", "SelectedOUT");
			this.ContinueBtnFailSafeTime = Time.unscaledTime;
			this.RefreshHeaderButtonClickability();
		}
	}

	private void Update()
	{
		this.CheckPreGameStats();
		if (this.m_currentSubState != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.Update()).MethodHandle;
			}
			if (this.m_currentSubState.SubStateType != UIGameOverScreen.GameOverScreenState.Done)
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
				this.m_currentSubState.Update();
				if (Input.GetMouseButtonDown(0))
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
					this.m_currentSubState.TryClickToSkip();
				}
				this.CheckIfCurrentStateIsDone();
				float num = 10f;
				if (this.m_currentSubState.SubStateType == UIGameOverScreen.GameOverScreenState.GGBoostUsage)
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
					num = GameBalanceVars.Get().GGPackEndGameUsageTimer + 10f;
				}
				if (this.ContinueBtnFailSafeTime > 0f)
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
					if (Time.unscaledTime > this.ContinueBtnFailSafeTime + num)
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
						this.ContinueBtnFailSafeTime = -1f;
						UIManager.SetGameObjectActive(this.m_ContinueBtn, true, null);
						string text = string.Format("Failsafe triggered on state {0}", this.m_currentSubState.SubStateType);
						UIGameOverScreen.GameOverExperienceUpdateSubState gameOverExperienceUpdateSubState = this.m_currentSubState as UIGameOverScreen.GameOverExperienceUpdateSubState;
						if (gameOverExperienceUpdateSubState != null)
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
							text += " - SubstateInfo updating ";
							for (int i = 0; i < gameOverExperienceUpdateSubState.CurrentInfos.Count; i++)
							{
								text += string.Format("{0} stuck at {1} and is paused: {2} ", gameOverExperienceUpdateSubState.CurrentInfos[i].UpdateType.ToString(), gameOverExperienceUpdateSubState.CurrentInfos[i].PercentageProgress, gameOverExperienceUpdateSubState.CurrentInfos[i].IsPaused);
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
						else
						{
							text = text + " - SubstateInfo updating stuck at: " + this.m_currentSubState.PercentageProgress;
						}
						Debug.LogError(text);
					}
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Tab))
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
			if (this.m_TopBottomBarsContainer.gameObject.activeInHierarchy)
			{
				UIGameStatsWindow.Get().ToggleStatsWindow();
			}
		}
		if (this.m_ContinueBtn.gameObject.activeInHierarchy)
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
			if (InputManager.Get().GetAcceptButtonDown())
			{
				this.OnContinueClicked(null);
			}
		}
	}

	private PercentileInfo GetStatPercentiles(StatDisplaySettings.StatType statType)
	{
		PercentileInfo result = null;
		if (this.m_results != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.GetStatPercentiles(StatDisplaySettings.StatType)).MethodHandle;
			}
			if (!this.m_results.BadgeAndParticipantsInfo.IsNullOrEmpty<BadgeAndParticipantInfo>())
			{
				int playerId = GameManager.Get().PlayerInfo.PlayerId;
				BadgeAndParticipantInfo badgeAndParticipantInfo = this.m_results.BadgeAndParticipantsInfo.Find((BadgeAndParticipantInfo p) => p.PlayerId == playerId);
				if (badgeAndParticipantInfo != null)
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
					if (!badgeAndParticipantInfo.GlobalPercentiles.IsNullOrEmpty<KeyValuePair<StatDisplaySettings.StatType, PercentileInfo>>())
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
						badgeAndParticipantInfo.GlobalPercentiles.TryGetValue(statType, out result);
					}
				}
			}
		}
		return result;
	}

	private PercentileInfo GetFreelancerStatPercentiles(int index)
	{
		PercentileInfo result = null;
		if (this.m_results != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.GetFreelancerStatPercentiles(int)).MethodHandle;
			}
			if (!this.m_results.BadgeAndParticipantsInfo.IsNullOrEmpty<BadgeAndParticipantInfo>())
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
				int playerId = GameManager.Get().PlayerInfo.PlayerId;
				BadgeAndParticipantInfo badgeAndParticipantInfo = this.m_results.BadgeAndParticipantsInfo.Find((BadgeAndParticipantInfo p) => p.PlayerId == playerId);
				if (badgeAndParticipantInfo != null)
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
					if (!badgeAndParticipantInfo.FreelancerSpecificPercentiles.IsNullOrEmpty<KeyValuePair<int, PercentileInfo>>())
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
						badgeAndParticipantInfo.FreelancerSpecificPercentiles.TryGetValue(index, out result);
					}
				}
			}
		}
		return result;
	}

	public class GameOverAccoladesUpdateSubState : UIGameOverScreen.GameOverSubState
	{
		public GameOverAccoladesUpdateSubState(float AutoTransitionTime) : base(UIGameOverScreen.GameOverScreenState.Accolades, AutoTransitionTime)
		{
		}

		public override void Update()
		{
		}
	}

	public class GameOverExperienceUpdateSubState : UIGameOverScreen.GameOverSubState
	{
		private List<UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo> TypesBeingUpdated = new List<UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo>();

		public GameOverExperienceUpdateSubState(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo StartInfo)
		{
			base.SubStateType = UIGameOverScreen.GameOverScreenState.ExperienceBars;
			this.TypesBeingUpdated.Add(StartInfo);
		}

		public GameOverExperienceUpdateSubState(IEnumerable<UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo> StartInfos)
		{
			base.SubStateType = UIGameOverScreen.GameOverScreenState.ExperienceBars;
			this.TypesBeingUpdated.AddRange(StartInfos);
		}

		public List<UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo> CurrentInfos
		{
			get
			{
				return this.TypesBeingUpdated;
			}
		}

		public override bool IsDone()
		{
			return this.TypesBeingUpdated.Count == 0;
		}

		public override void Update()
		{
			List<UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo> list = new List<UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo>();
			for (int i = 0; i < this.TypesBeingUpdated.Count; i++)
			{
				UIGameOverScreen.Get().HandleUpdateExpSubStateUpdate(this.TypesBeingUpdated[i]);
				if (!this.TypesBeingUpdated[i].IsPaused)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.GameOverExperienceUpdateSubState.Update()).MethodHandle;
					}
					if (this.TypesBeingUpdated[i].PercentageProgress >= 1f)
					{
						UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo updateInfo = this.TypesBeingUpdated[i];
						this.TypesBeingUpdated.RemoveAt(i);
						i--;
						IEnumerable<UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo> enumerable = UIGameOverScreen.Get().HandleUpdateExpSubStateComplete(updateInfo);
						if (enumerable != null)
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
							list.AddRange(enumerable);
						}
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
			if (list.Count > 0)
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
				this.TypesBeingUpdated.AddRange(list);
			}
		}

		public enum UpdatingType
		{
			None,
			InitialPause,
			NormalExpBar,
			GGExpAnim,
			GGExpBar,
			QuestBarPause,
			QuestExp,
			RankPoints
		}

		public class UpdatingInfo
		{
			private float PauseStartTime = -1f;

			private float PercentageFromPause;

			public UpdatingInfo(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType Type, float TimeToDoUpdate)
			{
				this.UpdateType = Type;
				this.TotalUpdateTime = TimeToDoUpdate;
				this.UpdateStartTime = Time.unscaledTime;
			}

			public UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingType UpdateType { get; private set; }

			public float UpdateStartTime { get; private set; }

			public float TotalUpdateTime { get; private set; }

			public bool IsPaused { get; private set; }

			public float PercentageProgress
			{
				get
				{
					if (this.IsPaused)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo.get_PercentageProgress()).MethodHandle;
						}
						return this.PercentageFromPause;
					}
					float num = Time.unscaledTime - this.UpdateStartTime;
					float num2 = num / this.TotalUpdateTime;
					float value = this.PercentageFromPause + (1f - this.PercentageFromPause) * num2;
					return Mathf.Clamp01(value);
				}
			}

			public void SetPaused(bool Paused)
			{
				if (Paused)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.GameOverExperienceUpdateSubState.UpdatingInfo.SetPaused(bool)).MethodHandle;
					}
					this.PauseStartTime = Time.unscaledTime;
					this.PercentageFromPause = this.PercentageProgress;
				}
				else if (this.PauseStartTime != -1f)
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
					this.TotalUpdateTime -= this.PauseStartTime - this.UpdateStartTime;
					this.UpdateStartTime = Time.unscaledTime;
					this.PauseStartTime = -1f;
				}
				this.IsPaused = Paused;
			}
		}
	}

	public class GameOverTutorialGames : UIGameOverScreen.GameOverSubState
	{
		public GameOverTutorialGames()
		{
			base.SubStateType = UIGameOverScreen.GameOverScreenState.TutorialTenGames;
			this.CloseClicked = false;
		}

		public bool CloseClicked { get; private set; }

		public override bool IsDone()
		{
			return this.CloseClicked;
		}

		public void NotifyCloseClicked()
		{
			this.CloseClicked = true;
		}
	}

	public class GameOverGGSubState : UIGameOverScreen.GameOverSubState
	{
		private UIGameOverScreen.GameOverGGSubState.GGBoosts CurrentGGBoostState;

		private float NextRecapDisplay = -1f;

		private float RecapDisplayInterval = 0.5f;

		private float FadeOutTime = -1f;

		private float StartTime = -1f;

		private float StartWaitingForNotificationTime = -1f;

		public GameOverGGSubState(float TimeToFadeOut)
		{
			base.SubStateType = UIGameOverScreen.GameOverScreenState.GGBoostUsage;
			this.CurrentGGBoostState = UIGameOverScreen.GameOverGGSubState.GGBoosts.Recapping;
			this.FadeOutTime = TimeToFadeOut;
		}

		public override bool IsDone()
		{
			if (base.SubStateType == UIGameOverScreen.GameOverScreenState.GGBoostUsage)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.GameOverGGSubState.IsDone()).MethodHandle;
				}
				return this.CurrentGGBoostState == UIGameOverScreen.GameOverGGSubState.GGBoosts.Done;
			}
			return false;
		}

		public void FadeoutAnimDone()
		{
			this.SetNewGGBoostState(UIGameOverScreen.GameOverGGSubState.GGBoosts.WaitingForNotification);
			this.StartWaitingForNotificationTime = Time.unscaledTime;
		}

		private void SetNewGGBoostState(UIGameOverScreen.GameOverGGSubState.GGBoosts newGGBoostState)
		{
			this.CurrentGGBoostState = newGGBoostState;
			if (this.CurrentGGBoostState == UIGameOverScreen.GameOverGGSubState.GGBoosts.UsageTimer)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.GameOverGGSubState.SetNewGGBoostState(UIGameOverScreen.GameOverGGSubState.GGBoosts)).MethodHandle;
				}
				this.StartTime = Time.unscaledTime;
			}
			UIGameOverScreen.Get().HandleNewGGBoostSubState(this, this.CurrentGGBoostState);
		}

		public override void Update()
		{
			if (this.CurrentGGBoostState == UIGameOverScreen.GameOverGGSubState.GGBoosts.Recapping)
			{
				if (Time.unscaledTime >= this.NextRecapDisplay)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.GameOverGGSubState.Update()).MethodHandle;
					}
					if (UIGameOverScreen.Get().DoNextGGBoostRecapDisplay())
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
						this.NextRecapDisplay = Time.unscaledTime + this.RecapDisplayInterval;
					}
					else
					{
						this.SetNewGGBoostState(UIGameOverScreen.GameOverGGSubState.GGBoosts.UsageTimer);
					}
				}
			}
			else if (this.CurrentGGBoostState == UIGameOverScreen.GameOverGGSubState.GGBoosts.UsageTimer)
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
				if (UIGameOverScreen.Get().NotificationArrived)
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
					this.FadeOutTime = Time.unscaledTime;
				}
				float num = 0f;
				if (this.StartTime >= 0f)
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
					num = (this.FadeOutTime - Time.unscaledTime) / (this.FadeOutTime - this.StartTime);
				}
				UIGameOverScreen.Get().HandleGGBoostSubStateUpdate(this.CurrentGGBoostState, num);
				if (this.StartTime >= 0f)
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
					if (num <= 0f)
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
						this.SetNewGGBoostState(UIGameOverScreen.GameOverGGSubState.GGBoosts.FadeOut);
					}
				}
			}
			else if (this.CurrentGGBoostState == UIGameOverScreen.GameOverGGSubState.GGBoosts.WaitingForNotification)
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
				if (UIGameOverScreen.Get().NotificationArrived)
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
					this.SetNewGGBoostState(UIGameOverScreen.GameOverGGSubState.GGBoosts.Done);
				}
				else if (this.StartWaitingForNotificationTime > 0f)
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
					if (Time.unscaledTime - this.StartWaitingForNotificationTime > 15f)
					{
						this.StartWaitingForNotificationTime = -1f;
						Log.Info("Timed out waiting for match result notification. forcing to open end game UI.", new object[0]);
						UIDialogPopupManager.OpenOneButtonDialog(string.Empty, string.Format(StringUtil.TR("PressOkToExit", "Global"), StringUtil.TR("TimeoutWaitingForMatchResult", "Global")), StringUtil.TR("Ok", "Global"), delegate(UIDialogBox UIDialogBox)
						{
							UIGameOverScreen.Get().OnContinueClicked(null);
						}, -1, false);
					}
				}
			}
		}

		public enum GGBoosts
		{
			None,
			Recapping,
			UsageTimer,
			FadeOut,
			WaitingForNotification,
			Done
		}
	}

	public class GameOverVictoryDefeatWaitingSubState : UIGameOverScreen.GameOverSubState
	{
		public GameOverVictoryDefeatWaitingSubState()
		{
			base.SubStateType = UIGameOverScreen.GameOverScreenState.VictoryDefeatWaitingForNotification;
		}

		public override bool IsDone()
		{
			return UIGameOverScreen.Get().NotificationArrived;
		}
	}

	public class GameOverSubState
	{
		protected float TimeToAutoTransition;

		private bool DidClickSkip;

		public GameOverSubState()
		{
			this.ProgressThatClickCanSkip = 2f;
			this.SubStateType = UIGameOverScreen.GameOverScreenState.None;
			this.SubStateStartTime = Time.unscaledTime;
		}

		public GameOverSubState(UIGameOverScreen.GameOverScreenState SubState, float AutoTransitionTime)
		{
			this.ProgressThatClickCanSkip = 2f;
			this.SubStateType = SubState;
			this.TimeToAutoTransition = Time.unscaledTime + AutoTransitionTime;
			this.SubStateStartTime = Time.unscaledTime;
		}

		public UIGameOverScreen.GameOverScreenState SubStateType { get; protected set; }

		private protected float SubStateStartTime { protected get; private set; }

		public float ProgressThatClickCanSkip { get; private set; }

		public float PercentageProgress
		{
			get
			{
				return Mathf.Clamp01((Time.unscaledTime - this.SubStateStartTime) / (this.TimeToAutoTransition - this.SubStateStartTime));
			}
		}

		public void TryClickToSkip()
		{
			if (this.PercentageProgress >= this.ProgressThatClickCanSkip)
			{
				this.DidClickSkip = true;
			}
		}

		public void SetPercentageClickToSkip(float percentage)
		{
			this.ProgressThatClickCanSkip = Mathf.Clamp01(percentage);
		}

		public virtual bool IsDone()
		{
			return Time.unscaledTime >= this.TimeToAutoTransition || this.DidClickSkip;
		}

		public virtual void Update()
		{
			UIGameOverScreen.Get().UpdateGameOverSubStateObjects(this);
		}
	}

	[Serializable]
	public class XPDisplayInfo
	{
		public TextMeshProUGUI m_XPLabel;

		public TextMeshProUGUI m_barLevelLabel;

		public Animator m_levelUpAnimController;

		public Animator m_barLevelUpAnimator;

		public ImageFilledSloped m_OldXPSlider;

		public ImageFilledSloped m_NormalXPGainSlider;

		public ImageFilledSloped m_GGXPSlider;

		public ImageFilledSloped m_QuestXPSlider;

		public Image m_NextRewardIcon;

		public Image m_NextRewardFG;

		public UITooltipHoverObject m_RewardBtn;

		public UITooltipHoverObject m_tooltipHitBox;

		[HideInInspector]
		public int m_LastLevelDisplayed;

		[HideInInspector]
		public bool m_playingLevelUp;

		[HideInInspector]
		public UIGameOverScreen.XPDisplayInfo.BarXPType XPBarType;

		public static int GetXPForType(UIGameOverScreen.XPDisplayInfo.BarXPType BarType, int Level)
		{
			if (BarType == UIGameOverScreen.XPDisplayInfo.BarXPType.Season)
			{
				int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
				return SeasonWideData.Get().GetSeasonExperience(activeSeason, Level);
			}
			if (BarType == UIGameOverScreen.XPDisplayInfo.BarXPType.Character)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.XPDisplayInfo.GetXPForType(UIGameOverScreen.XPDisplayInfo.BarXPType, int)).MethodHandle;
				}
				return GameBalanceVars.Get().CharacterExperienceToLevel(Level);
			}
			return 0;
		}

		public string RewardTooltip { get; private set; }

		public CharacterType LastCharType { get; private set; }

		public void Init()
		{
			this.LastCharType = CharacterType.None;
			this.m_RewardBtn.Setup(TooltipType.GameRewardItem, delegate(UITooltipBase tooltip)
			{
				if (this.RewardTooltip.IsNullOrEmpty())
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.XPDisplayInfo.<Init>m__0(UITooltipBase)).MethodHandle;
					}
					return false;
				}
				UIRewardItemTooltip uirewardItemTooltip = tooltip as UIRewardItemTooltip;
				uirewardItemTooltip.Setup(this.RewardTooltip);
				return true;
			}, null);
		}

		public void PlayLevelUpAnimation(string name)
		{
			UIAnimationEventManager.Get().PlayAnimation(this.m_levelUpAnimController, name, null, string.Empty, 0, 0f, true, false, null, null);
		}

		public void CheckForRewardDisplay(int NewLevel)
		{
		}

		public void PopulateRewardIcon(int CurrentLevel, bool IsInitialReward = false, CharacterResourceLink charLink = null)
		{
			int num = 0;
			int num2 = 0;
			if (this.XPBarType == UIGameOverScreen.XPDisplayInfo.BarXPType.Season)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.XPDisplayInfo.PopulateRewardIcon(int, bool, CharacterResourceLink)).MethodHandle;
				}
				List<SeasonReward> accountRewardsForNextLevel = UIGameOverScreen.GetAccountRewardsForNextLevel(CurrentLevel - 1);
				List<SeasonReward> accountRewardsForNextLevel2 = UIGameOverScreen.GetAccountRewardsForNextLevel(CurrentLevel);
				int num3 = 0;
				int i = 0;
				while (i < accountRewardsForNextLevel2.Count)
				{
					if (!(accountRewardsForNextLevel2[i] is SeasonItemReward))
					{
						goto IL_158;
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
					if (accountRewardsForNextLevel2[i].repeatEveryXLevels <= num3)
					{
						goto IL_158;
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
					SeasonItemReward seasonItemReward = accountRewardsForNextLevel2[i] as SeasonItemReward;
					if (seasonItemReward.Conditions.IsNullOrEmpty<QuestCondition>())
					{
						goto IL_C7;
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
					if (QuestWideData.AreConditionsMet(seasonItemReward.Conditions, seasonItemReward.LogicStatement, false))
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							goto IL_C7;
						}
					}
					goto IL_203;
					IL_C7:
					InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate((accountRewardsForNextLevel2[i] as SeasonItemReward).ItemReward.ItemTemplateId);
					Sprite sprite = (Sprite)Resources.Load(itemTemplate.IconPath, typeof(Sprite));
					this.RewardTooltip = itemTemplate.GetDisplayName();
					this.m_NextRewardIcon.sprite = sprite;
					this.m_NextRewardFG.sprite = null;
					UIManager.SetGameObjectActive(this.m_NextRewardFG, false, null);
					num3 = accountRewardsForNextLevel2[i].repeatEveryXLevels;
					IL_203:
					i++;
					continue;
					IL_158:
					if (accountRewardsForNextLevel2[i] is SeasonUnlockReward && accountRewardsForNextLevel2[i].repeatEveryXLevels > num3)
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
						QuestUnlockReward unlockReward = (accountRewardsForNextLevel2[i] as SeasonUnlockReward).UnlockReward;
						Sprite sprite2 = (Sprite)Resources.Load(unlockReward.resourceString, typeof(Sprite));
						this.RewardTooltip = string.Empty;
						this.m_NextRewardIcon.sprite = sprite2;
						this.m_NextRewardFG.sprite = null;
						UIManager.SetGameObjectActive(this.m_NextRewardFG, false, null);
						num3 = accountRewardsForNextLevel2[i].repeatEveryXLevels;
						goto IL_203;
					}
					goto IL_203;
				}
				num = accountRewardsForNextLevel2.Count;
				num2 = accountRewardsForNextLevel.Count;
			}
			else if (this.XPBarType == UIGameOverScreen.XPDisplayInfo.BarXPType.Character)
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
				List<RewardUtils.RewardData> list = new List<RewardUtils.RewardData>();
				List<RewardUtils.RewardData> list2 = new List<RewardUtils.RewardData>();
				if (charLink != null)
				{
					this.LastCharType = charLink.m_characterType;
					if (CurrentLevel < GameBalanceVars.Get().MaxCharacterLevel)
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
						list2 = RewardUtils.GetNextCharacterRewards(charLink, CurrentLevel);
					}
					if (0 < CurrentLevel - 1 && CurrentLevel - 1 < GameBalanceVars.Get().MaxCharacterLevel)
					{
						list = RewardUtils.GetNextCharacterRewards(charLink, CurrentLevel - 1);
					}
				}
				if (list2.Count > 0)
				{
					this.m_NextRewardIcon.sprite = UIGameOverScreen.GetRewardSprite(list2[0]);
					this.m_NextRewardFG.sprite = list2[0].Foreground;
					UIManager.SetGameObjectActive(this.m_NextRewardFG, this.m_NextRewardFG.sprite != null, null);
				}
				this.RewardTooltip = string.Empty;
				for (int j = 0; j < list2.Count; j++)
				{
					if (!this.RewardTooltip.IsNullOrEmpty())
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
						this.RewardTooltip += Environment.NewLine;
					}
					this.RewardTooltip += RewardUtils.GetDisplayString(list2[j], false);
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
				num = list2.Count;
				num2 = list.Count;
			}
			if (num == 0)
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
				if (IsInitialReward)
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
					this.PlayLevelUpAnimation("resultsRewardIconNoIDLE");
				}
				else if (num2 > 0)
				{
					this.PlayLevelUpAnimation("resultsRewardIconYEStoNO");
				}
			}
			else if (IsInitialReward)
			{
				this.PlayLevelUpAnimation("resultsRewardIconYesIDLE");
			}
			else if (num2 > 0)
			{
				this.PlayLevelUpAnimation("resultsRewardIconYEStoYES");
			}
			else
			{
				this.PlayLevelUpAnimation("resultsRewardIconNOtoYES");
			}
			this.m_RewardBtn.Refresh();
		}

		public void LevelUpAnimDone()
		{
			this.m_OldXPSlider.fillAmount = 0f;
			this.m_NormalXPGainSlider.fillAmount = 0f;
			this.m_QuestXPSlider.fillAmount = 0f;
			this.m_GGXPSlider.fillAmount = 0f;
			this.m_XPLabel.text = "0 / " + UIGameOverScreen.XPDisplayInfo.GetXPForType(this.XPBarType, this.m_LastLevelDisplayed);
			this.m_playingLevelUp = false;
			CharacterResourceLink charLink = null;
			if (this.LastCharType != CharacterType.None)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.XPDisplayInfo.LevelUpAnimDone()).MethodHandle;
				}
				charLink = GameWideData.Get().GetCharacterResourceLink(this.LastCharType);
			}
			this.PopulateRewardIcon(this.m_LastLevelDisplayed, false, charLink);
		}

		public enum BarXPType
		{
			Season,
			Character
		}
	}

	[Serializable]
	public class CurrencyDisplayInfo
	{
		public RectTransform m_container;

		public TextMeshProUGUI m_currencyGainText;

		public UITooltipHoverObject m_tooltipHitBox;

		public Image m_displayIcon;

		[HideInInspector]
		public List<MatchResultsNotification.CurrencyReward> m_currencyReward = new List<MatchResultsNotification.CurrencyReward>();

		public int GetTotalBaseGained(CurrencyType currencyType)
		{
			int num = 0;
			for (int i = 0; i < this.m_currencyReward.Count; i++)
			{
				if (this.m_currencyReward[i].Type == currencyType)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.CurrencyDisplayInfo.GetTotalBaseGained(CurrencyType)).MethodHandle;
					}
					num += this.m_currencyReward[i].BaseGained;
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
			return num;
		}

		public int GetTotalEventGained(CurrencyType currencyType)
		{
			int num = 0;
			for (int i = 0; i < this.m_currencyReward.Count; i++)
			{
				if (this.m_currencyReward[i].Type == currencyType)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.CurrencyDisplayInfo.GetTotalEventGained(CurrencyType)).MethodHandle;
					}
					num += this.m_currencyReward[i].EventGained;
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
			return num;
		}

		public int GetTotalWinGained(CurrencyType currencyType)
		{
			int num = 0;
			for (int i = 0; i < this.m_currencyReward.Count; i++)
			{
				if (this.m_currencyReward[i].Type == currencyType)
				{
					num += this.m_currencyReward[i].WinGained;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.CurrencyDisplayInfo.GetTotalWinGained(CurrencyType)).MethodHandle;
			}
			return num;
		}

		public int GetTotalQuestGained(CurrencyType currencyType)
		{
			int num = 0;
			for (int i = 0; i < this.m_currencyReward.Count; i++)
			{
				if (this.m_currencyReward[i].Type == currencyType)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.CurrencyDisplayInfo.GetTotalQuestGained(CurrencyType)).MethodHandle;
					}
					num += this.m_currencyReward[i].QuestGained;
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
			return num;
		}

		public int GetTotalGGGained(CurrencyType currencyType)
		{
			int num = 0;
			for (int i = 0; i < this.m_currencyReward.Count; i++)
			{
				if (this.m_currencyReward[i].Type == currencyType)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.CurrencyDisplayInfo.GetTotalGGGained(CurrencyType)).MethodHandle;
					}
					num += this.m_currencyReward[i].GGGained;
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
			return num;
		}

		public int GetTotalLevelUpGained(CurrencyType currencyType)
		{
			int num = 0;
			for (int i = 0; i < this.m_currencyReward.Count; i++)
			{
				if (this.m_currencyReward[i].Type == currencyType)
				{
					num += this.m_currencyReward[i].LevelUpGained;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.CurrencyDisplayInfo.GetTotalLevelUpGained(CurrencyType)).MethodHandle;
			}
			return num;
		}

		public int GetTotalNormalCurrencyReward(CurrencyType currencyType)
		{
			int num = 0;
			for (int i = 0; i < this.m_currencyReward.Count; i++)
			{
				if (this.m_currencyReward[i].Type == currencyType)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.CurrencyDisplayInfo.GetTotalNormalCurrencyReward(CurrencyType)).MethodHandle;
					}
					num += this.m_currencyReward[i].BaseGained + this.m_currencyReward[i].WinGained + this.m_currencyReward[i].EventGained + this.m_currencyReward[i].LevelUpGained;
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
			return num;
		}

		public int GetTotalGGBoostCurrencyReward(CurrencyType currencyType)
		{
			int num = 0;
			for (int i = 0; i < this.m_currencyReward.Count; i++)
			{
				if (this.m_currencyReward[i].Type == currencyType)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.CurrencyDisplayInfo.GetTotalGGBoostCurrencyReward(CurrencyType)).MethodHandle;
					}
					num += this.m_currencyReward[i].GGGained;
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
			return num;
		}

		public int GetTotalQuestCurrencyReward(CurrencyType currencyType)
		{
			int num = 0;
			for (int i = 0; i < this.m_currencyReward.Count; i++)
			{
				if (this.m_currencyReward[i].Type == currencyType)
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverScreen.CurrencyDisplayInfo.GetTotalQuestCurrencyReward(CurrencyType)).MethodHandle;
					}
					num += this.m_currencyReward[i].QuestGained;
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
			return num;
		}
	}

	public enum GameOverScreenState
	{
		None,
		VictoryDefeat,
		VictoryDefeatWaitingForNotification,
		GGBoostUsage,
		TutorialTenGames,
		ResultsScreenPause,
		Accolades,
		PersonalHighlights,
		MissionNotifications,
		ExperienceBars,
		Stats,
		Rewards,
		Done
	}
}
