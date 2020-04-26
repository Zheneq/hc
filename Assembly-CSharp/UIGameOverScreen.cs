using LobbyGameClientMessages;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIGameOverScreen : UIScene
{
	public class GameOverAccoladesUpdateSubState : GameOverSubState
	{
		public GameOverAccoladesUpdateSubState(float AutoTransitionTime)
			: base(GameOverScreenState.Accolades, AutoTransitionTime)
		{
		}

		public override void Update()
		{
		}
	}

	public class GameOverExperienceUpdateSubState : GameOverSubState
	{
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

			public UpdatingType UpdateType
			{
				get;
				private set;
			}

			public float UpdateStartTime
			{
				get;
				private set;
			}

			public float TotalUpdateTime
			{
				get;
				private set;
			}

			public bool IsPaused
			{
				get;
				private set;
			}

			public float PercentageProgress
			{
				get
				{
					if (IsPaused)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								return PercentageFromPause;
							}
						}
					}
					float num = Time.unscaledTime - UpdateStartTime;
					float num2 = num / TotalUpdateTime;
					float value = PercentageFromPause + (1f - PercentageFromPause) * num2;
					return Mathf.Clamp01(value);
				}
			}

			public UpdatingInfo(UpdatingType Type, float TimeToDoUpdate)
			{
				UpdateType = Type;
				TotalUpdateTime = TimeToDoUpdate;
				UpdateStartTime = Time.unscaledTime;
			}

			public void SetPaused(bool Paused)
			{
				if (Paused)
				{
					PauseStartTime = Time.unscaledTime;
					PercentageFromPause = PercentageProgress;
				}
				else if (PauseStartTime != -1f)
				{
					TotalUpdateTime -= PauseStartTime - UpdateStartTime;
					UpdateStartTime = Time.unscaledTime;
					PauseStartTime = -1f;
				}
				IsPaused = Paused;
			}
		}

		private List<UpdatingInfo> TypesBeingUpdated = new List<UpdatingInfo>();

		public List<UpdatingInfo> CurrentInfos => TypesBeingUpdated;

		public GameOverExperienceUpdateSubState(UpdatingInfo StartInfo)
		{
			base.SubStateType = GameOverScreenState.ExperienceBars;
			TypesBeingUpdated.Add(StartInfo);
		}

		public GameOverExperienceUpdateSubState(IEnumerable<UpdatingInfo> StartInfos)
		{
			base.SubStateType = GameOverScreenState.ExperienceBars;
			TypesBeingUpdated.AddRange(StartInfos);
		}

		public override bool IsDone()
		{
			return TypesBeingUpdated.Count == 0;
		}

		public override void Update()
		{
			List<UpdatingInfo> list = new List<UpdatingInfo>();
			for (int i = 0; i < TypesBeingUpdated.Count; i++)
			{
				Get().HandleUpdateExpSubStateUpdate(TypesBeingUpdated[i]);
				if (TypesBeingUpdated[i].IsPaused)
				{
					continue;
				}
				if (!(TypesBeingUpdated[i].PercentageProgress >= 1f))
				{
					continue;
				}
				UpdatingInfo updateInfo = TypesBeingUpdated[i];
				TypesBeingUpdated.RemoveAt(i);
				i--;
				IEnumerable<UpdatingInfo> enumerable = Get().HandleUpdateExpSubStateComplete(updateInfo);
				if (enumerable != null)
				{
					list.AddRange(enumerable);
				}
			}
			while (true)
			{
				if (list.Count > 0)
				{
					while (true)
					{
						TypesBeingUpdated.AddRange(list);
						return;
					}
				}
				return;
			}
		}
	}

	public class GameOverTutorialGames : GameOverSubState
	{
		public bool CloseClicked
		{
			get;
			private set;
		}

		public GameOverTutorialGames()
		{
			base.SubStateType = GameOverScreenState.TutorialTenGames;
			CloseClicked = false;
		}

		public override bool IsDone()
		{
			return CloseClicked;
		}

		public void NotifyCloseClicked()
		{
			CloseClicked = true;
		}
	}

	public class GameOverGGSubState : GameOverSubState
	{
		public enum GGBoosts
		{
			None,
			Recapping,
			UsageTimer,
			FadeOut,
			WaitingForNotification,
			Done
		}

		private GGBoosts CurrentGGBoostState;

		private float NextRecapDisplay = -1f;

		private float RecapDisplayInterval = 0.5f;

		private float FadeOutTime = -1f;

		private float StartTime = -1f;

		private float StartWaitingForNotificationTime = -1f;

		public GameOverGGSubState(float TimeToFadeOut)
		{
			base.SubStateType = GameOverScreenState.GGBoostUsage;
			CurrentGGBoostState = GGBoosts.Recapping;
			FadeOutTime = TimeToFadeOut;
		}

		public override bool IsDone()
		{
			if (base.SubStateType == GameOverScreenState.GGBoostUsage)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return CurrentGGBoostState == GGBoosts.Done;
					}
				}
			}
			return false;
		}

		public void FadeoutAnimDone()
		{
			SetNewGGBoostState(GGBoosts.WaitingForNotification);
			StartWaitingForNotificationTime = Time.unscaledTime;
		}

		private void SetNewGGBoostState(GGBoosts newGGBoostState)
		{
			CurrentGGBoostState = newGGBoostState;
			if (CurrentGGBoostState == GGBoosts.UsageTimer)
			{
				StartTime = Time.unscaledTime;
			}
			Get().HandleNewGGBoostSubState(this, CurrentGGBoostState);
		}

		public override void Update()
		{
			if (CurrentGGBoostState == GGBoosts.Recapping)
			{
				if (!(Time.unscaledTime >= NextRecapDisplay))
				{
					return;
				}
				while (true)
				{
					if (Get().DoNextGGBoostRecapDisplay())
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
								NextRecapDisplay = Time.unscaledTime + RecapDisplayInterval;
								return;
							}
						}
					}
					SetNewGGBoostState(GGBoosts.UsageTimer);
					return;
				}
			}
			if (CurrentGGBoostState == GGBoosts.UsageTimer)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						if (Get().NotificationArrived)
						{
							FadeOutTime = Time.unscaledTime;
						}
						float num = 0f;
						if (StartTime >= 0f)
						{
							num = (FadeOutTime - Time.unscaledTime) / (FadeOutTime - StartTime);
						}
						Get().HandleGGBoostSubStateUpdate(CurrentGGBoostState, num);
						if (StartTime >= 0f)
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									if (num <= 0f)
									{
										while (true)
										{
											switch (3)
											{
											case 0:
												break;
											default:
												SetNewGGBoostState(GGBoosts.FadeOut);
												return;
											}
										}
									}
									return;
								}
							}
						}
						return;
					}
					}
				}
			}
			if (CurrentGGBoostState != GGBoosts.WaitingForNotification)
			{
				return;
			}
			while (true)
			{
				if (Get().NotificationArrived)
				{
					while (true)
					{
						switch (4)
						{
						case 0:
							break;
						default:
							SetNewGGBoostState(GGBoosts.Done);
							return;
						}
					}
				}
				if (!(StartWaitingForNotificationTime > 0f))
				{
					return;
				}
				while (true)
				{
					if (Time.unscaledTime - StartWaitingForNotificationTime > 15f)
					{
						StartWaitingForNotificationTime = -1f;
						Log.Info("Timed out waiting for match result notification. forcing to open end game UI.");
						UIDialogPopupManager.OpenOneButtonDialog(string.Empty, string.Format(StringUtil.TR("PressOkToExit", "Global"), StringUtil.TR("TimeoutWaitingForMatchResult", "Global")), StringUtil.TR("Ok", "Global"), delegate
						{
							Get().OnContinueClicked(null);
						});
					}
					return;
				}
			}
		}
	}

	public class GameOverVictoryDefeatWaitingSubState : GameOverSubState
	{
		public GameOverVictoryDefeatWaitingSubState()
		{
			base.SubStateType = GameOverScreenState.VictoryDefeatWaitingForNotification;
		}

		public override bool IsDone()
		{
			return Get().NotificationArrived;
		}
	}

	public class GameOverSubState
	{
		protected float TimeToAutoTransition;

		private bool DidClickSkip;

		public GameOverScreenState SubStateType
		{
			get;
			protected set;
		}

		protected float SubStateStartTime
		{
			get;
			private set;
		}

		public float ProgressThatClickCanSkip
		{
			get;
			private set;
		}

		public float PercentageProgress => Mathf.Clamp01((Time.unscaledTime - SubStateStartTime) / (TimeToAutoTransition - SubStateStartTime));

		public GameOverSubState()
		{
			ProgressThatClickCanSkip = 2f;
			SubStateType = GameOverScreenState.None;
			SubStateStartTime = Time.unscaledTime;
		}

		public GameOverSubState(GameOverScreenState SubState, float AutoTransitionTime)
		{
			ProgressThatClickCanSkip = 2f;
			SubStateType = SubState;
			TimeToAutoTransition = Time.unscaledTime + AutoTransitionTime;
			SubStateStartTime = Time.unscaledTime;
		}

		public void TryClickToSkip()
		{
			if (PercentageProgress >= ProgressThatClickCanSkip)
			{
				DidClickSkip = true;
			}
		}

		public void SetPercentageClickToSkip(float percentage)
		{
			ProgressThatClickCanSkip = Mathf.Clamp01(percentage);
		}

		public virtual bool IsDone()
		{
			return Time.unscaledTime >= TimeToAutoTransition || DidClickSkip;
		}

		public virtual void Update()
		{
			Get().UpdateGameOverSubStateObjects(this);
		}
	}

	[Serializable]
	public class XPDisplayInfo
	{
		public enum BarXPType
		{
			Season,
			Character
		}

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
		public BarXPType XPBarType;

		public string RewardTooltip
		{
			get;
			private set;
		}

		public CharacterType LastCharType
		{
			get;
			private set;
		}

		public static int GetXPForType(BarXPType BarType, int Level)
		{
			switch (BarType)
			{
			case BarXPType.Season:
			{
				int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
				return SeasonWideData.Get().GetSeasonExperience(activeSeason, Level);
			}
			case BarXPType.Character:
				while (true)
				{
					return GameBalanceVars.Get().CharacterExperienceToLevel(Level);
				}
			default:
				return 0;
			}
		}

		public void Init()
		{
			LastCharType = CharacterType.None;
			m_RewardBtn.Setup(TooltipType.GameRewardItem, delegate(UITooltipBase tooltip)
			{
				if (RewardTooltip.IsNullOrEmpty())
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
				UIRewardItemTooltip uIRewardItemTooltip = tooltip as UIRewardItemTooltip;
				uIRewardItemTooltip.Setup(RewardTooltip);
				return true;
			});
		}

		public void PlayLevelUpAnimation(string name)
		{
			UIAnimationEventManager.Get().PlayAnimation(m_levelUpAnimController, name, null, string.Empty);
		}

		public void CheckForRewardDisplay(int NewLevel)
		{
		}

		public void PopulateRewardIcon(int CurrentLevel, bool IsInitialReward = false, CharacterResourceLink charLink = null)
		{
			int num = 0;
			int num2 = 0;
			if (XPBarType == BarXPType.Season)
			{
				List<SeasonReward> accountRewardsForNextLevel = GetAccountRewardsForNextLevel(CurrentLevel - 1);
				List<SeasonReward> accountRewardsForNextLevel2 = GetAccountRewardsForNextLevel(CurrentLevel);
				int num3 = 0;
				for (int i = 0; i < accountRewardsForNextLevel2.Count; i++)
				{
					if (accountRewardsForNextLevel2[i] is SeasonItemReward)
					{
						if (accountRewardsForNextLevel2[i].repeatEveryXLevels > num3)
						{
							SeasonItemReward seasonItemReward = accountRewardsForNextLevel2[i] as SeasonItemReward;
							if (!seasonItemReward.Conditions.IsNullOrEmpty())
							{
								if (!QuestWideData.AreConditionsMet(seasonItemReward.Conditions, seasonItemReward.LogicStatement))
								{
									continue;
								}
							}
							InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate((accountRewardsForNextLevel2[i] as SeasonItemReward).ItemReward.ItemTemplateId);
							Sprite sprite = (Sprite)Resources.Load(itemTemplate.IconPath, typeof(Sprite));
							RewardTooltip = itemTemplate.GetDisplayName();
							m_NextRewardIcon.sprite = sprite;
							m_NextRewardFG.sprite = null;
							UIManager.SetGameObjectActive(m_NextRewardFG, false);
							num3 = accountRewardsForNextLevel2[i].repeatEveryXLevels;
							continue;
						}
					}
					if (accountRewardsForNextLevel2[i] is SeasonUnlockReward && accountRewardsForNextLevel2[i].repeatEveryXLevels > num3)
					{
						QuestUnlockReward unlockReward = (accountRewardsForNextLevel2[i] as SeasonUnlockReward).UnlockReward;
						Sprite sprite2 = (Sprite)Resources.Load(unlockReward.resourceString, typeof(Sprite));
						RewardTooltip = string.Empty;
						m_NextRewardIcon.sprite = sprite2;
						m_NextRewardFG.sprite = null;
						UIManager.SetGameObjectActive(m_NextRewardFG, false);
						num3 = accountRewardsForNextLevel2[i].repeatEveryXLevels;
					}
				}
				num = accountRewardsForNextLevel2.Count;
				num2 = accountRewardsForNextLevel.Count;
			}
			else if (XPBarType == BarXPType.Character)
			{
				List<RewardUtils.RewardData> list = new List<RewardUtils.RewardData>();
				List<RewardUtils.RewardData> list2 = new List<RewardUtils.RewardData>();
				if (charLink != null)
				{
					LastCharType = charLink.m_characterType;
					if (CurrentLevel < GameBalanceVars.Get().MaxCharacterLevel)
					{
						list2 = RewardUtils.GetNextCharacterRewards(charLink, CurrentLevel);
					}
					if (0 < CurrentLevel - 1 && CurrentLevel - 1 < GameBalanceVars.Get().MaxCharacterLevel)
					{
						list = RewardUtils.GetNextCharacterRewards(charLink, CurrentLevel - 1);
					}
				}
				if (list2.Count > 0)
				{
					m_NextRewardIcon.sprite = GetRewardSprite(list2[0]);
					m_NextRewardFG.sprite = list2[0].Foreground;
					UIManager.SetGameObjectActive(m_NextRewardFG, m_NextRewardFG.sprite != null);
				}
				RewardTooltip = string.Empty;
				for (int j = 0; j < list2.Count; j++)
				{
					if (!RewardTooltip.IsNullOrEmpty())
					{
						RewardTooltip += Environment.NewLine;
					}
					RewardTooltip += RewardUtils.GetDisplayString(list2[j]);
				}
				num = list2.Count;
				num2 = list.Count;
			}
			if (num == 0)
			{
				if (IsInitialReward)
				{
					PlayLevelUpAnimation("resultsRewardIconNoIDLE");
				}
				else if (num2 > 0)
				{
					PlayLevelUpAnimation("resultsRewardIconYEStoNO");
				}
			}
			else if (IsInitialReward)
			{
				PlayLevelUpAnimation("resultsRewardIconYesIDLE");
			}
			else if (num2 > 0)
			{
				PlayLevelUpAnimation("resultsRewardIconYEStoYES");
			}
			else
			{
				PlayLevelUpAnimation("resultsRewardIconNOtoYES");
			}
			m_RewardBtn.Refresh();
		}

		public void LevelUpAnimDone()
		{
			m_OldXPSlider.fillAmount = 0f;
			m_NormalXPGainSlider.fillAmount = 0f;
			m_QuestXPSlider.fillAmount = 0f;
			m_GGXPSlider.fillAmount = 0f;
			m_XPLabel.text = "0 / " + GetXPForType(XPBarType, m_LastLevelDisplayed);
			m_playingLevelUp = false;
			CharacterResourceLink charLink = null;
			if (LastCharType != 0)
			{
				charLink = GameWideData.Get().GetCharacterResourceLink(LastCharType);
			}
			PopulateRewardIcon(m_LastLevelDisplayed, false, charLink);
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
			for (int i = 0; i < m_currencyReward.Count; i++)
			{
				if (m_currencyReward[i].Type == currencyType)
				{
					num += m_currencyReward[i].BaseGained;
				}
			}
			while (true)
			{
				return num;
			}
		}

		public int GetTotalEventGained(CurrencyType currencyType)
		{
			int num = 0;
			for (int i = 0; i < m_currencyReward.Count; i++)
			{
				if (m_currencyReward[i].Type == currencyType)
				{
					num += m_currencyReward[i].EventGained;
				}
			}
			while (true)
			{
				return num;
			}
		}

		public int GetTotalWinGained(CurrencyType currencyType)
		{
			int num = 0;
			for (int i = 0; i < m_currencyReward.Count; i++)
			{
				if (m_currencyReward[i].Type == currencyType)
				{
					num += m_currencyReward[i].WinGained;
				}
			}
			while (true)
			{
				return num;
			}
		}

		public int GetTotalQuestGained(CurrencyType currencyType)
		{
			int num = 0;
			for (int i = 0; i < m_currencyReward.Count; i++)
			{
				if (m_currencyReward[i].Type == currencyType)
				{
					num += m_currencyReward[i].QuestGained;
				}
			}
			while (true)
			{
				return num;
			}
		}

		public int GetTotalGGGained(CurrencyType currencyType)
		{
			int num = 0;
			for (int i = 0; i < m_currencyReward.Count; i++)
			{
				if (m_currencyReward[i].Type == currencyType)
				{
					num += m_currencyReward[i].GGGained;
				}
			}
			while (true)
			{
				return num;
			}
		}

		public int GetTotalLevelUpGained(CurrencyType currencyType)
		{
			int num = 0;
			for (int i = 0; i < m_currencyReward.Count; i++)
			{
				if (m_currencyReward[i].Type == currencyType)
				{
					num += m_currencyReward[i].LevelUpGained;
				}
			}
			while (true)
			{
				return num;
			}
		}

		public int GetTotalNormalCurrencyReward(CurrencyType currencyType)
		{
			int num = 0;
			for (int i = 0; i < m_currencyReward.Count; i++)
			{
				if (m_currencyReward[i].Type == currencyType)
				{
					num += m_currencyReward[i].BaseGained + m_currencyReward[i].WinGained + m_currencyReward[i].EventGained + m_currencyReward[i].LevelUpGained;
				}
			}
			while (true)
			{
				return num;
			}
		}

		public int GetTotalGGBoostCurrencyReward(CurrencyType currencyType)
		{
			int num = 0;
			for (int i = 0; i < m_currencyReward.Count; i++)
			{
				if (m_currencyReward[i].Type == currencyType)
				{
					num += m_currencyReward[i].GGGained;
				}
			}
			while (true)
			{
				return num;
			}
		}

		public int GetTotalQuestCurrencyReward(CurrencyType currencyType)
		{
			int num = 0;
			for (int i = 0; i < m_currencyReward.Count; i++)
			{
				if (m_currencyReward[i].Type == currencyType)
				{
					num += m_currencyReward[i].QuestGained;
				}
			}
			while (true)
			{
				return num;
			}
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

	public XPDisplayInfo m_characterXPInfo = new XPDisplayInfo();

	public XPDisplayInfo m_playerXPInfo = new XPDisplayInfo();

	public TextMeshProUGUI m_seasonLevelText;

	public TextMeshProUGUI m_expGain;

	public Animator m_ggBonusController;

	public TextMeshProUGUI m_ggBonusXPAnimText;

	public RectTransform m_rewardsInfoContainer;

	public UITooltipHoverObject m_ggBonusTooltipObj;

	[Header("Currency Increase Objects")]
	public CurrencyDisplayInfo m_isoDisplay;

	public CurrencyDisplayInfo m_freelancerCurrencyDisplay;

	public CurrencyDisplayInfo m_rankedCurrencyDisplay;

	public CurrencyDisplayInfo m_influenceDisplay;

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

	private GameOverSubState m_currentSubState;

	private PersistedStats m_statsAtBeginningOfMatch;

	private float EstimatedNotificationArrivalTime = -1f;

	private int LastRankTierDisplayed = -100;

	private bool ShouldDisplayTierPoints;

	private bool RankLevelUpDownAnimating;

	private const float MIN_RANK_FILL_AMT = 0.082f;

	private const float MAX_RANK_FILL_AMT = 0.915f;

	private const float POINTS_PER_RANKED_TIER = 100f;

	private List<UITutorialSeasonLevelBar> m_tutorialLevelSliderBars;

	private List<RewardUtils.RewardData> m_tutorialRewards;

	private List<UIGameOverStatWidget> m_GameOverStatWidgets = new List<UIGameOverStatWidget>();

	private float ContinueBtnFailSafeTime = -1f;

	public bool IsVisible
	{
		get;
		private set;
	}

	public MatchResultsNotification Results => m_results;

	public int NumRewardsEarned
	{
		get;
		private set;
	}

	public bool RequestedToUseGGPack
	{
		get;
		private set;
	}

	private bool BadgesAreSet
	{
		get;
		set;
	}

	private bool TalliedCurrencies
	{
		get;
		set;
	}

	private bool IsRewardsScreenSetup
	{
		get;
		set;
	}

	private bool NotificationArrived => m_results != null;

	private bool IsRankedGame => m_gameType == GameType.Ranked;

	private Team ClientTeam => (GameManager.Get().PlayerInfo.TeamId == Team.TeamB) ? Team.TeamB : Team.TeamA;

	private Team FriendlyTeam
	{
		get
		{
			int result;
			if (ClientTeam == Team.TeamB)
			{
				result = 1;
			}
			else
			{
				result = 0;
			}
			return (Team)result;
		}
	}

	private bool SelfWon
	{
		get
		{
			bool result = false;
			if (GameManager.Get() != null)
			{
				if (GameManager.Get().PlayerInfo != null)
				{
					int num;
					if (GameManager.Get().PlayerInfo.TeamId == Team.TeamB)
					{
						num = 1;
					}
					else
					{
						num = 0;
					}
					Team team = (Team)num;
					Team team2 = (team == Team.TeamB) ? Team.TeamB : Team.TeamA;
					if (m_gameResult == GameResult.TeamAWon && team2 == Team.TeamA)
					{
						goto IL_009c;
					}
					if (m_gameResult == GameResult.TeamBWon)
					{
						if (team2 == Team.TeamB)
						{
							goto IL_009c;
						}
					}
				}
			}
			goto IL_009e;
			IL_009e:
			return result;
			IL_009c:
			result = true;
			goto IL_009e;
		}
	}

	private bool BadgesAreActive
	{
		get
		{
			if (NotificationArrived)
			{
				if (!m_results.BadgeAndParticipantsInfo.IsNullOrEmpty())
				{
					while (true)
					{
						switch (3)
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

	public static UIGameOverScreen Get()
	{
		return s_instance;
	}

	public override void Awake()
	{
		s_instance = this;
		UpdateEndGameGGBonuses(0, 1f);
		ClientGameManager.Get().OnMatchResultsNotification += HandleMatchResultsNotification;
		ClientGameManager.Get().OnBankBalanceChange += HandleBankBalanceChange;
		ClientGameManager.Get().OnUseGGPackNotification += HandleGGPackUsed;
		m_tutorialLevelSliderBars = new List<UITutorialSeasonLevelBar>();
		m_tutorialLevelSliderBars.AddRange(m_tutorialLevelLayout.GetComponentsInChildren<UITutorialSeasonLevelBar>());
		m_ggBonusTooltipObj.Setup(TooltipType.Titled, GGBonusTooltipSetup);
		InitButtons();
		UIManager.SetGameObjectActive(m_TopParticipantsAnimator, false);
		UIManager.SetGameObjectActive(m_PersonalHighlightsAnimator, false);
		base.Awake();
	}

	private void InitButtons()
	{
		m_worldGGBtnHitBox.callback = OnWorldGGButtonClicked;
		m_ContinueBtn.spriteController.callback = OnContinueClicked;
		m_ContinueBtn.spriteController.m_soundToPlay = FrontEndButtonSounds.DialogBoxButton;
		m_feedbackButton.spriteController.callback = FeedbackButtonClicked;
		m_feedbackButton.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		m_shareFacebookButton.spriteController.callback = ShareFacebookButtonClicked;
		m_shareFacebookButton.spriteController.m_soundToPlay = FrontEndButtonSounds.MenuChoice;
		m_influenceDisplay.m_tooltipHitBox.Setup(TooltipType.MatchEndRewards, delegate(UITooltipBase tooltip)
		{
			GetRewardTooltip(tooltip).Setup(m_results.FactionContributionAmounts, UIGameOverRewardTooltip.RewardTooltipType.FactionInfo);
			return true;
		});
		m_isoDisplay.m_tooltipHitBox.Setup(TooltipType.MatchEndRewards, (UITooltipBase tooltip) => SetupCurrencyTooltip(tooltip, UIGameOverRewardTooltip.RewardTooltipType.ISOAmount, m_isoDisplay));
		m_freelancerCurrencyDisplay.m_tooltipHitBox.Setup(TooltipType.MatchEndRewards, (UITooltipBase tooltip) => SetupCurrencyTooltip(tooltip, UIGameOverRewardTooltip.RewardTooltipType.FreelancerCurrencyAmount, m_freelancerCurrencyDisplay));
		m_rankedCurrencyDisplay.m_tooltipHitBox.Setup(TooltipType.MatchEndRewards, (UITooltipBase tooltip) => SetupCurrencyTooltip(tooltip, UIGameOverRewardTooltip.RewardTooltipType.RankedPointsAmount, m_rankedCurrencyDisplay));
		m_characterXPInfo.m_tooltipHitBox.Setup(TooltipType.MatchEndRewards, delegate(UITooltipBase tooltip)
		{
			GetRewardTooltip(tooltip).Setup(m_characterXPInfo, UIGameOverRewardTooltip.RewardTooltipType.CharacterInfo, m_results.NumCharactersPlayed);
			return true;
		});
		UIEventTriggerUtils.AddListener(m_characterXPInfo.m_tooltipHitBox.gameObject, EventTriggerType.PointerEnter, DisplayCharacterXPInfo);
		UIEventTriggerUtils.AddListener(m_characterXPInfo.m_tooltipHitBox.gameObject, EventTriggerType.PointerExit, HideXpInfo);
		m_playerXPInfo.m_tooltipHitBox.Setup(TooltipType.MatchEndRewards, delegate(UITooltipBase tooltip)
		{
			GetRewardTooltip(tooltip).Setup(m_playerXPInfo, UIGameOverRewardTooltip.RewardTooltipType.AccountInfo, 1);
			return true;
		});
		UIEventTriggerUtils.AddListener(m_playerXPInfo.m_tooltipHitBox.gameObject, EventTriggerType.PointerEnter, DisplayPlayerXPInfo);
		UIEventTriggerUtils.AddListener(m_playerXPInfo.m_tooltipHitBox.gameObject, EventTriggerType.PointerExit, HideXpInfo);
		m_characterXPInfo.Init();
		m_playerXPInfo.Init();
		m_AccoladesHeaderBtn.spriteController.callback = NavButtonClicked;
		m_StatsHeaderBtn.spriteController.callback = NavButtonClicked;
		m_RewardsHeaderBtn.spriteController.callback = NavButtonClicked;
		m_ScoreHeaderBtn.spriteController.callback = NavButtonClicked;
		UIManager.SetGameObjectActive(m_AccoladesHeaderBtn, true);
		UIManager.SetGameObjectActive(m_StatsHeaderBtn, true);
		UIManager.SetGameObjectActive(m_StatsHeaderBtn, true);
		UIManager.SetGameObjectActive(m_ScoreHeaderBtn, true);
	}

	private void OnDestroy()
	{
		if (UIScreenManager.Get() != null)
		{
			UIScreenManager.Get().EndAllLoopSounds();
		}
		s_instance = null;
		if (ClientGameManager.Get() != null)
		{
			ClientGameManager.Get().OnMatchResultsNotification -= HandleMatchResultsNotification;
			ClientGameManager.Get().OnBankBalanceChange -= HandleBankBalanceChange;
			ClientGameManager.Get().OnUseGGPackNotification -= HandleGGPackUsed;
		}
	}

	private void HideXpInfo(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_characterXPInfo.m_XPLabel, false);
		UIManager.SetGameObjectActive(m_playerXPInfo.m_XPLabel, false);
	}

	private bool GGBonusTooltipSetup(UITooltipBase tooltip)
	{
		int num = 0;
		if (ClientGameManager.Get() != null && ClientGameManager.Get().PlayerWallet != null)
		{
			num = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack);
		}
		string empty = string.Empty;
		empty = ((num <= 0) ? $"<color=#7f7f7f>x{num}</color>" : $"<color=green>x{num}</color>");
		string text = StringUtil.TR("GGBoostUsageDescription", "GameOver");
		if (num == 0)
		{
			text += StringUtil.TR("NoGGBoosts", "GameOver");
		}
		UITitledTooltip uITitledTooltip = tooltip as UITitledTooltip;
		uITitledTooltip.Setup(StringUtil.TR("GGBoosts", "Rewards"), text, empty);
		return true;
	}

	private void DisplayCharacterXPInfo(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_characterXPInfo.m_XPLabel, true);
	}

	private void DisplayPlayerXPInfo(BaseEventData data)
	{
		SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason);
		UIManager.SetGameObjectActive(m_tutorialLevelContainer, seasonTemplate.IsTutorial);
		UIManager.SetGameObjectActive(m_playerXPInfo.m_XPLabel, !seasonTemplate.IsTutorial);
	}

	public void NotifyWidgetMouseOver(UIGameOverBadgeWidget widget, bool mouseOver)
	{
		if (mouseOver)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					for (int i = 0; i < m_GameOverStatWidgets.Count; i++)
					{
						if (widget.BadgeInfo.UsesFreelancerStats)
						{
							if (m_GameOverStatWidgets[i].DisplayStatType == UIGameOverStatWidget.StatDisplayType.FreelancerStat)
							{
								m_GameOverStatWidgets[i].SetBadgeHighlight(true, true);
							}
							else
							{
								m_GameOverStatWidgets[i].SetBadgeHighlight(true, false);
							}
						}
						else if (m_GameOverStatWidgets[i].DisplayStatType == UIGameOverStatWidget.StatDisplayType.GeneralStat)
						{
							if (widget.BadgeInfo.StatsToHighlight.Contains(m_GameOverStatWidgets[i].GeneralStatType))
							{
								m_GameOverStatWidgets[i].SetBadgeHighlight(true, true);
							}
							else
							{
								m_GameOverStatWidgets[i].SetBadgeHighlight(true, false);
							}
						}
						else
						{
							m_GameOverStatWidgets[i].SetBadgeHighlight(true, false);
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
			}
		}
		for (int j = 0; j < m_GameOverStatWidgets.Count; j++)
		{
			m_GameOverStatWidgets[j].SetBadgeHighlight(false, false);
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

	public void ShareFacebookButtonClicked(BaseEventData data)
	{
		FacebookClientInterface facebookClientInterface = FacebookClientInterface.Get();
		
		facebookClientInterface.TakeScreenshot(delegate(Texture2D texture)
			{
				UILandingPageFullScreenMenus.Get().SetFacebookContainerVisible(true, texture);
			});
	}

	public void NavButtonClicked(BaseEventData data)
	{
		if (m_currentSubState != null && m_currentSubState.SubStateType != GameOverScreenState.Stats)
		{
			if (m_currentSubState.SubStateType != GameOverScreenState.Rewards)
			{
				if (m_currentSubState.SubStateType != GameOverScreenState.Done)
				{
					return;
				}
			}
		}
		if (m_currentSubState.SubStateType != GameOverScreenState.Done)
		{
			m_currentSubState = new GameOverSubState(GameOverScreenState.Done, 0f);
		}
		GameObject gameObject = (data as PointerEventData).pointerCurrentRaycast.gameObject;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		bool flag4 = false;
		if (m_AccoladesHeaderBtn.spriteController.gameObject == gameObject)
		{
			flag = true;
		}
		else if (m_StatsHeaderBtn.spriteController.gameObject == gameObject)
		{
			flag2 = true;
		}
		else if (m_RewardsHeaderBtn.spriteController.gameObject == gameObject)
		{
			flag3 = true;
		}
		else if (m_ScoreHeaderBtn.spriteController.gameObject == gameObject)
		{
			flag4 = true;
		}
		m_AccoladesHeaderBtn.SetSelected(flag, false, "SelectedIN", "SelectedOUT");
		m_StatsHeaderBtn.SetSelected(flag2, false, "SelectedIN", "SelectedOUT");
		m_RewardsHeaderBtn.SetSelected(flag3, false, "SelectedIN", "SelectedOUT");
		m_ScoreHeaderBtn.SetSelected(flag4, false, "SelectedIN", "SelectedOUT");
		RefreshHeaderButtonClickability();
		UIManager.SetGameObjectActive(m_AccoladesAnimator, flag);
		if (flag)
		{
			UIManager.SetGameObjectActive(m_TopParticipantsAnimator, true);
			UIManager.SetGameObjectActive(m_PersonalHighlightsAnimator, true);
			UIAnimationEventManager.Get().PlayAnimation(m_TopParticipantsAnimator, "TopParticipantsGrpDefaultIDLE", null, string.Empty);
			UIAnimationEventManager.Get().PlayAnimation(m_PersonalHighlightsAnimator, "PersonalHighlightsGrpDefaultIDLE", null, string.Empty);
			for (int i = 0; i < m_PersonalHighlightWidgets.Length; i++)
			{
				UIManager.SetGameObjectActive(m_PersonalHighlightWidgets[i], true);
				m_PersonalHighlightWidgets[i].SetHighlight();
			}
		}
		UIManager.SetGameObjectActive(m_StatsContainer, flag2);
		if (flag2)
		{
			UIAnimationEventManager.Get().PlayAnimation(m_StatsAnimator, "StatsScreenDefaultIDLE", null, string.Empty);
		}
		UIManager.SetGameObjectActive(m_RewardsContainer, flag3);
		UIGameStatsWindow.Get().SetToggleStatsVisible(flag4);
	}

	public void FeedbackButtonClicked(BaseEventData data)
	{
		UILandingPageFullScreenMenus.Get().SetFeedbackContainerVisible(true);
	}

	public void OnWorldGGButtonClicked(BaseEventData data)
	{
		if (RequestedToUseGGPack)
		{
			return;
		}
		int currentAmount = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack);
		if (currentAmount != 0)
		{
			if (m_numSelfGGpacksUsed < 3)
			{
				for (int i = 0; i < m_ggButtonLevelsAnims.Length; i++)
				{
					if (!m_ggButtonLevelsAnims[i].gameObject.activeInHierarchy)
					{
						continue;
					}
					if (currentAmount > 1)
					{
						UIAnimationEventManager.Get().PlayAnimation(m_ggButtonLevelsAnims[i], "GGBoostDefaultIN", null, string.Empty);
					}
					else
					{
						UIAnimationEventManager.Get().PlayAnimation(m_ggButtonLevelsAnims[i], "GGBoostNoMoreDefaultIN", null, string.Empty);
						m_worldGGBtnHitBox.SetClickable(false);
					}
				}
				while (true)
				{
					ClientGameManager.Get().RequestToUseGGPack();
					m_numSelfGGpacksUsed++;
					for (int j = 0; j < m_ggButtonLevels.Length; j++)
					{
						UIManager.SetGameObjectActive(m_ggButtonLevels[j], j == m_numSelfGGpacksUsed);
					}
					while (true)
					{
						if (m_numSelfGGpacksUsed == 1)
						{
							UIFrontEnd.PlaySound(FrontEndButtonSounds.GGButtonEndGameUsed);
						}
						else if (m_numSelfGGpacksUsed == 2)
						{
							AudioManager.PostEvent("ui/endgame/ggboost_button_silver");
						}
						else if (m_numSelfGGpacksUsed == 3)
						{
							AudioManager.PostEvent("ui/endgame/ggboost_button_gold");
						}
						if (GameOverWorldObjects.Get() != null)
						{
							GameOverWorldObjects.Get().m_worldResultAnimController.Play("ResultGGPackPressAnimation");
						}
						RequestedToUseGGPack = true;
						if (currentAmount > 1)
						{
							if (m_numSelfGGpacksUsed < 3)
							{
								return;
							}
						}
						for (int k = 0; k < m_ggButtonLevelsAnims.Length; k++)
						{
							if (m_ggButtonLevelsAnims[k].gameObject.activeInHierarchy)
							{
								UIAnimationEventManager.Get().PlayAnimation(m_ggButtonLevelsAnims[k], "GGBoostNoMoreDefaultIN", null, string.Empty);
							}
						}
						m_worldGGBtnHitBox.SetClickable(false);
						return;
					}
				}
			}
		}
		m_worldGGBtnHitBox.SetClickable(false);
	}

	public void OnContinueClicked(BaseEventData data)
	{
		UINewReward.Get().Clear();
		UIGameStatsWindow.Get().SetVisible(false);
		FacebookClientInterface.Get().Reset();
		ClientGameManager.Get().LeaveGame(true, m_gameResult);
	}

	public static CurrencyType RewardTooltipTypeToCurrencyType(UIGameOverRewardTooltip.RewardTooltipType rewardType)
	{
		if (rewardType == UIGameOverRewardTooltip.RewardTooltipType.FreelancerCurrencyAmount)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return CurrencyType.FreelancerCurrency;
				}
			}
		}
		switch (rewardType)
		{
		case UIGameOverRewardTooltip.RewardTooltipType.ISOAmount:
			return CurrencyType.ISO;
		case UIGameOverRewardTooltip.RewardTooltipType.RankedPointsAmount:
			while (true)
			{
				return CurrencyType.RankedCurrency;
			}
		default:
			return CurrencyType.NONE;
		}
	}

	private bool SetupCurrencyTooltip(UITooltipBase tooltip, UIGameOverRewardTooltip.RewardTooltipType type, CurrencyDisplayInfo info)
	{
		GetRewardTooltip(tooltip).Setup(info.GetTotalBaseGained(RewardTooltipTypeToCurrencyType(type)), info.GetTotalGGGained(RewardTooltipTypeToCurrencyType(type)), info.GetTotalWinGained(RewardTooltipTypeToCurrencyType(type)), info.GetTotalQuestGained(RewardTooltipTypeToCurrencyType(type)), info.GetTotalLevelUpGained(RewardTooltipTypeToCurrencyType(type)), info.GetTotalEventGained(RewardTooltipTypeToCurrencyType(type)), type);
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
				ActorData current = enumerator.Current;
				if (current.m_characterType == playerInfo.CharacterType)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return current;
						}
					}
				}
			}
		}
		return null;
	}

	public void SetWorldGGPackText(string text)
	{
		for (int i = 0; i < m_worldGGPackText.Length; i++)
		{
			m_worldGGPackText[i].text = text;
		}
	}

	private void SetupLabelText(int myTeamScore, int enemyTeamScore)
	{
		if (m_turnTimeText != null)
		{
			string empty = string.Empty;
			if (UITimerPanel.Get().GetSeconds() < 10)
			{
				empty = string.Format(StringUtil.TR("TimeFormatLeadingZero", "Global"), UITimerPanel.Get().GetMinutes(), UITimerPanel.Get().GetSeconds());
			}
			else
			{
				empty = string.Format(StringUtil.TR("TimeFormat", "Global"), UITimerPanel.Get().GetMinutes(), UITimerPanel.Get().GetSeconds());
			}
			m_turnTimeText.text = string.Format(StringUtil.TR("TurnTime", "GameOver"), UITimerPanel.Get().GetTurn(), empty);
		}
		if (m_blueTeamScore != null)
		{
			m_blueTeamScore.text = string.Format(StringUtil.TR("BlueTeamScore", "GameOver"), myTeamScore.ToString());
		}
		if (m_redTeamScore != null)
		{
			m_redTeamScore.text = string.Format(StringUtil.TR("RedTeamScore", "GameOver"), enemyTeamScore.ToString());
		}
		if (m_mapText != null)
		{
			m_mapText.text = GameWideData.Get().GetMapDisplayName(GameManager.Get().GameInfo.GameConfig.Map);
		}
		if (m_gameTypeLabel != null)
		{
			m_gameTypeLabel.text = string.Format(StringUtil.TR("DeathMatchLabel", "GameOver"), GameManager.Get().GameConfig.TeamAPlayers.ToString(), GameManager.Get().GameConfig.TeamBPlayers.ToString());
		}
		if (!(m_objectiveText != null))
		{
			return;
		}
		while (true)
		{
			if (ObjectivePoints.Get() != null)
			{
				while (true)
				{
					m_objectiveText.text = string.Format(StringUtil.TR(ObjectivePoints.Get().m_victoryCondition), (ObjectivePoints.Get().m_timeLimitTurns - 1).ToString());
					return;
				}
			}
			return;
		}
	}

	public void HandleBankBalanceChange(CurrencyData currencyData)
	{
		int currentAmount = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack);
		m_GGPackCount.text = $"x{currentAmount}";
		for (int i = 0; i < m_ggButtonLevelsAnims.Length; i++)
		{
			if (!m_ggButtonLevelsAnims[i].gameObject.activeInHierarchy)
			{
				continue;
			}
			if (currentAmount > 0)
			{
				UIAnimationEventManager.Get().PlayAnimation(m_ggButtonLevelsAnims[i], "GGBoostDefaultIN", null, string.Empty);
			}
			else
			{
				UIAnimationEventManager.Get().PlayAnimation(m_ggButtonLevelsAnims[i], "GGBoostNoMoreDefaultIN", null, string.Empty);
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

	public static List<SeasonReward> GetAccountRewardsForNextLevel(int currentLevel)
	{
		PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
		List<SeasonReward> allRewards = SeasonWideData.Get().GetSeasonTemplate(playerAccountData.QuestComponent.ActiveSeason).Rewards.GetAllRewards();
		List<SeasonReward> list = new List<SeasonReward>();
		int num = currentLevel + 1;
		for (int i = 0; i < allRewards.Count; i++)
		{
			if (allRewards[i].level != num)
			{
				continue;
			}
			if (allRewards[i].repeatEveryXLevels == 0)
			{
				list.Add(allRewards[i]);
			}
		}
		while (true)
		{
			for (int j = 0; j < allRewards.Count; j++)
			{
				if (allRewards[j].repeatEveryXLevels <= 0)
				{
					continue;
				}
				if ((num - allRewards[j].level) % allRewards[j].repeatEveryXLevels == 0)
				{
					list.Add(allRewards[j]);
				}
			}
			return list;
		}
	}

	private bool SetupAccountRewardTooltip(int currentLevel)
	{
		m_playerXPInfo.PopulateRewardIcon(currentLevel, true);
		return true;
	}

	private void SetupExpBars()
	{
		int num = GameBalanceVars.Get().CharacterExperienceToLevel(m_results.CharacterLevelAtStart);
		m_characterXPInfo.m_barLevelLabel.text = m_results.CharacterLevelAtStart.ToString();
		m_characterXPInfo.m_GGXPSlider.fillAmount = 0f;
		m_characterXPInfo.m_NormalXPGainSlider.fillAmount = 0f;
		m_characterXPInfo.m_OldXPSlider.fillAmount = (float)m_results.CharacterXpAtStart / (float)num;
		m_characterXPInfo.m_QuestXPSlider.fillAmount = 0f;
		m_characterXPInfo.m_XPLabel.text = m_results.CharacterXpAtStart.ToString();
		m_characterXPInfo.m_LastLevelDisplayed = m_results.CharacterLevelAtStart;
		m_characterXPInfo.XPBarType = XPDisplayInfo.BarXPType.Character;
		UIManager.SetGameObjectActive(m_characterXPInfo.m_XPLabel, false);
		int activeSeason = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason;
		int seasonExperience = SeasonWideData.Get().GetSeasonExperience(activeSeason, m_results.SeasonLevelAtStart);
		m_playerXPInfo.m_barLevelLabel.text = m_results.SeasonLevelAtStart.ToString();
		m_playerXPInfo.m_GGXPSlider.fillAmount = 0f;
		m_playerXPInfo.m_NormalXPGainSlider.fillAmount = 0f;
		m_playerXPInfo.m_OldXPSlider.fillAmount = (float)m_results.SeasonXpAtStart / (float)seasonExperience;
		m_playerXPInfo.m_QuestXPSlider.fillAmount = 0f;
		m_playerXPInfo.m_XPLabel.text = m_results.SeasonXpAtStart.ToString();
		m_playerXPInfo.m_LastLevelDisplayed = m_results.SeasonLevelAtStart;
		m_playerXPInfo.XPBarType = XPDisplayInfo.BarXPType.Season;
		UIManager.SetGameObjectActive(m_playerXPInfo.m_XPLabel, false);
		m_expGain.text = "+0";
		SetupAccountRewardTooltip(m_results.SeasonLevelAtStart);
		ActorData playersOriginalActorData = GetPlayersOriginalActorData();
		if (playersOriginalActorData != null)
		{
			SetupCharacterRewardTooltip(playersOriginalActorData.GetCharacterResourceLink(), m_results.CharacterLevelAtStart);
		}
		if (!(GameFlowData.Get().activeOwnedActorData != null))
		{
			goto IL_02c4;
		}
		if ((bool)ReplayPlayManager.Get())
		{
			if (ReplayPlayManager.Get().IsPlayback())
			{
				goto IL_02c4;
			}
		}
		UIManager.SetGameObjectActive(m_rewardsInfoContainer, true);
		goto IL_02d1;
		IL_02c4:
		UIManager.SetGameObjectActive(m_rewardsInfoContainer, false);
		goto IL_02d1;
		IL_02d1:
		SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason);
		UIManager.SetGameObjectActive(m_playerXPInfo.m_barLevelUpAnimator, !seasonTemplate.IsTutorial);
		UIManager.SetGameObjectActive(m_tutorialLevelContainer, seasonTemplate.IsTutorial);
		if (!seasonTemplate.IsTutorial)
		{
			return;
		}
		while (true)
		{
			int endLevel = QuestWideData.GetEndLevel(seasonTemplate.Prerequisites, seasonTemplate.Index);
			int num2 = m_results.SeasonLevelAtStart;
			if (m_gameType != 0)
			{
				if (m_gameType == GameType.Tutorial)
				{
					if (num2 > 1)
					{
						goto IL_038f;
					}
				}
				num2++;
			}
			goto IL_038f;
			IL_038f:
			m_tutorialLevelText.text = num2 - 1 + "/" + (endLevel - 1);
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
				Queue<RewardUtils.RewardData> queue = new Queue<RewardUtils.RewardData>(RewardUtils.GetSeasonLevelRewards());
				List<RewardUtils.RewardData> availableSeasonEndRewards = RewardUtils.GetAvailableSeasonEndRewards(seasonTemplate);
				if (availableSeasonEndRewards.Count > 0)
				{
					queue.Enqueue(availableSeasonEndRewards[0]);
				}
				for (int j = 0; j < m_tutorialLevelSliderBars.Count; j++)
				{
					int num3 = j + 1;
					m_tutorialLevelSliderBars[j].SetFilled(num3 < num2);
					UIManager.SetGameObjectActive(m_tutorialLevelSliderBars[j], num3 < endLevel);
					RewardUtils.RewardData rewardData = null;
					while (queue.Count > 0)
					{
						if (rewardData == null)
						{
							int num4 = queue.Peek().Level - 1;
							if (num4 < num3)
							{
								queue.Dequeue();
								continue;
							}
							if (num4 > num3)
							{
								break;
							}
							rewardData = queue.Dequeue();
							continue;
						}
						break;
					}
					m_tutorialLevelSliderBars[j].SetReward(num3, rewardData);
				}
				while (true)
				{
					SetupTutorialRewards();
					return;
				}
			}
		}
	}

	private void SetupTutorialRewards()
	{
		SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason);
		m_tutorialRewards = RewardUtils.GetSeasonLevelRewards();
		for (int i = 0; i < QuestWideData.GetEndLevel(seasonTemplate.Prerequisites, seasonTemplate.Index); i++)
		{
			foreach (RewardUtils.RewardData accountLevelReward in RewardUtils.GetAccountLevelRewards(i))
			{
				int index = 0;
				int num = 0;
				while (true)
				{
					if (num >= m_tutorialRewards.Count)
					{
						break;
					}
					if (accountLevelReward.Level <= m_tutorialRewards[num].Level)
					{
						index = num;
						break;
					}
					num++;
				}
				m_tutorialRewards.Insert(index, accountLevelReward);
			}
		}
		while (true)
		{
			List<RewardUtils.RewardData> availableSeasonEndRewards = RewardUtils.GetAvailableSeasonEndRewards(seasonTemplate);
			m_tutorialRewards.AddRange(availableSeasonEndRewards);
			for (int j = 0; j < m_tutorialRewards.Count; j++)
			{
				m_tutorialRewards[j].Level--;
			}
			while (true)
			{
				if (m_tutorialRewards.Count == 0)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							UIManager.SetGameObjectActive(m_tutorialRewardTooltipObj, false);
							UIManager.SetGameObjectActive(m_tutorialNextRewardLabel, false);
							UIManager.SetGameObjectActive(m_tutorialRewardIconImage, false);
							return;
						}
					}
				}
				UIManager.SetGameObjectActive(m_tutorialRewardTooltipObj, true);
				UIManager.SetGameObjectActive(m_tutorialNextRewardLabel, true);
				UIManager.SetGameObjectActive(m_tutorialRewardIconImage, true);
				RewardUtils.RewardData rewardData = null;
				if (availableSeasonEndRewards.Count > 0)
				{
					rewardData = availableSeasonEndRewards[0];
				}
				if (rewardData == null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							UIManager.SetGameObjectActive(m_tutorialRewardTooltipObj, false);
							UIManager.SetGameObjectActive(m_tutorialNextRewardLabel, false);
							UIManager.SetGameObjectActive(m_tutorialRewardIconImage, false);
							return;
						}
					}
				}
				UIManager.SetGameObjectActive(m_tutorialRewardTooltipObj, true);
				UIManager.SetGameObjectActive(m_tutorialNextRewardLabel, true);
				UIManager.SetGameObjectActive(m_tutorialRewardIconImage, true);
				m_tutorialRewardIconImage.sprite = Resources.Load<Sprite>(rewardData.SpritePath);
				UIManager.SetGameObjectActive(m_tutorialRewardFgImage, rewardData.Foreground != null);
				m_tutorialRewardFgImage.sprite = rewardData.Foreground;
				m_tutorialNextRewardLabel.text = rewardData.Name;
				m_tutorialRewardTooltipObj.Setup(TooltipType.RewardList, delegate(UITooltipBase tooltip)
				{
					if (m_tutorialRewards != null)
					{
						if (m_tutorialRewards.Count != 0)
						{
							UIRewardListTooltip uIRewardListTooltip = tooltip as UIRewardListTooltip;
							uIRewardListTooltip.Setup(m_tutorialRewards, m_results.SeasonLevelAtStart, UIRewardListTooltip.RewardsType.Tutorial, true);
							return true;
						}
					}
					return false;
				});
				return;
			}
		}
	}

	private void SetupCharacterRewardTooltip(CharacterResourceLink charLink, int currentCharLevel)
	{
		GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
		if (gameBalanceVars == null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_characterXPInfo.PopulateRewardIcon(currentCharLevel, true, charLink);
	}

	private void SetupCurrencyAndInfluenceDisplays()
	{
		bool doActive = GetDisplayTotalCurrency(m_isoDisplay, CurrencyType.ISO) > 0;
		bool doActive2 = GetDisplayTotalCurrency(m_freelancerCurrencyDisplay, CurrencyType.FreelancerCurrency) > 0;
		bool doActive3 = GetDisplayTotalCurrency(m_rankedCurrencyDisplay, CurrencyType.RankedCurrency) > 0;
		if (!TalliedCurrencies && m_results.CurrencyRewards != null)
		{
			for (int i = 0; i < m_results.CurrencyRewards.Count; i++)
			{
				int num = m_results.CurrencyRewards[i].BaseGained + m_results.CurrencyRewards[i].WinGained + m_results.CurrencyRewards[i].GGGained + m_results.CurrencyRewards[i].QuestGained + m_results.CurrencyRewards[i].LevelUpGained + m_results.CurrencyRewards[i].EventGained;
				if (num <= 0)
				{
					continue;
				}
				CurrencyType type = m_results.CurrencyRewards[i].Type;
				if (type != 0)
				{
					if (type != CurrencyType.FreelancerCurrency)
					{
						if (type != CurrencyType.RankedCurrency)
						{
						}
						else
						{
							doActive3 = true;
							m_rankedCurrencyDisplay.m_currencyReward.Add(m_results.CurrencyRewards[i]);
							m_rankedCurrencyDisplay.m_currencyGainText.text = "+0";
						}
					}
					else
					{
						doActive2 = true;
						m_freelancerCurrencyDisplay.m_currencyReward.Add(m_results.CurrencyRewards[i]);
						m_freelancerCurrencyDisplay.m_currencyGainText.text = "+0";
					}
				}
				else
				{
					doActive = true;
					m_isoDisplay.m_currencyReward.Add(m_results.CurrencyRewards[i]);
					m_isoDisplay.m_currencyGainText.text = "+0";
				}
			}
			TalliedCurrencies = true;
		}
		UIManager.SetGameObjectActive(m_isoDisplay.m_container, doActive);
		UIManager.SetGameObjectActive(m_freelancerCurrencyDisplay.m_container, doActive2);
		UIManager.SetGameObjectActive(m_rankedCurrencyDisplay.m_container, doActive3);
		m_influenceDisplay.m_currencyGainText.text = "+0";
		FactionCompetition factionCompetition = FactionWideData.Get().GetFactionCompetition(m_results.FactionCompetitionId);
		if (factionCompetition != null && factionCompetition.ShouldShowcase)
		{
			if (m_results.FactionId < factionCompetition.Factions.Count)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
					{
						FactionGroup factionGroup = FactionWideData.Get().GetFactionGroup(factionCompetition.Factions[m_results.FactionId].FactionGroupIDToUse);
						m_influenceDisplay.m_displayIcon.sprite = Resources.Load<Sprite>(factionGroup.IconPath);
						UIManager.SetGameObjectActive(m_influenceDisplay.m_container, factionCompetition.Enabled);
						return;
					}
					}
				}
			}
		}
		UIManager.SetGameObjectActive(m_influenceDisplay.m_container, false);
	}

	private void SetupRewardsScreen()
	{
		if (!NotificationArrived)
		{
			return;
		}
		while (true)
		{
			if (IsRewardsScreenSetup)
			{
				return;
			}
			while (true)
			{
				IsRewardsScreenSetup = true;
				List<int> list = new List<int>();
				int num = GetNormalBarXPTotal() + m_results.GGXpGained + m_results.QuestXpGained;
				int num2 = m_results.SeasonLevelAtStart;
				int num3 = XPDisplayInfo.GetXPForType(XPDisplayInfo.BarXPType.Season, num2) - m_results.SeasonXpAtStart;
				while (num >= num3)
				{
					num -= num3;
					num2++;
					list.Add(num2);
					num3 = XPDisplayInfo.GetXPForType(XPDisplayInfo.BarXPType.Season, num2);
				}
				while (true)
				{
					List<RewardUtils.RewardData> list2 = new List<RewardUtils.RewardData>();
					List<RewardUtils.RewardData> list3 = new List<RewardUtils.RewardData>();
					for (int i = 0; i < list.Count; i++)
					{
						List<RewardUtils.RewardData> nextSeasonLevelRewards = RewardUtils.GetNextSeasonLevelRewards(list[i] - 1);
						foreach (RewardUtils.RewardData item in nextSeasonLevelRewards)
						{
							if (item.InventoryTemplate.Type == InventoryItemType.Currency)
							{
								list2.Add(item);
							}
						}
						if (ClientGameManager.Get().GetPlayerAccountData().QuestComponent.SeasonItemRewardsGranted.ContainsKey(list[i]))
						{
							List<int> list4 = ClientGameManager.Get().GetPlayerAccountData().QuestComponent.SeasonItemRewardsGranted[list[i]];
							using (List<int>.Enumerator enumerator2 = list4.GetEnumerator())
							{
								while (enumerator2.MoveNext())
								{
									int current2 = enumerator2.Current;
									InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(current2);
									RewardUtils.RewardData rewardData = new RewardUtils.RewardData();
									rewardData.Amount = 1;
									rewardData.InventoryTemplate = itemTemplate;
									rewardData.Level = list[i];
									rewardData.Name = itemTemplate.DisplayName;
									rewardData.SpritePath = InventoryWideData.GetSpritePath(itemTemplate);
									rewardData.Foreground = InventoryWideData.GetItemFg(itemTemplate);
									rewardData.Type = RewardUtils.GetRewardTypeFromInventoryItem(itemTemplate);
									list2.Add(rewardData);
								}
							}
						}
					}
					while (true)
					{
						ActorData playersOriginalActorData = GetPlayersOriginalActorData();
						if (playersOriginalActorData != null)
						{
							CharacterResourceLink characterResourceLink = playersOriginalActorData.GetCharacterResourceLink();
							GameBalanceVars gameBalanceVars = GameBalanceVars.Get();
							List<int> list5 = new List<int>();
							int num4 = GetNormalBarXPTotal() + m_results.ConsumableXpGained + m_results.GGXpGained;
							int num5 = m_results.CharacterLevelAtStart;
							int num6 = XPDisplayInfo.GetXPForType(XPDisplayInfo.BarXPType.Character, num5) - m_results.CharacterXpAtStart;
							while (num4 >= num6)
							{
								num4 -= num6;
								num5++;
								list5.Add(num5);
								num6 = XPDisplayInfo.GetXPForType(XPDisplayInfo.BarXPType.Character, num5);
							}
							for (int j = 0; j < list5.Count; j++)
							{
								list3.AddRange(RewardUtils.GetNextCharacterRewards(characterResourceLink, list5[j] - 1));
								for (int k = 0; k < gameBalanceVars.RepeatingCharacterLevelRewards.Length; k++)
								{
									if (gameBalanceVars.RepeatingCharacterLevelRewards[k].charType != (int)characterResourceLink.m_characterType)
									{
										continue;
									}
									if (gameBalanceVars.RepeatingCharacterLevelRewards[k].repeatingLevel <= 0)
									{
										continue;
									}
									if (list5[j] - 1 <= gameBalanceVars.RepeatingCharacterLevelRewards[k].startLevel)
									{
										continue;
									}
									if ((list5[j] - gameBalanceVars.RepeatingCharacterLevelRewards[k].startLevel) % gameBalanceVars.RepeatingCharacterLevelRewards[k].repeatingLevel == 0)
									{
										RewardUtils.RewardData rewardData2 = new RewardUtils.RewardData();
										rewardData2.Amount = gameBalanceVars.RepeatingCharacterLevelRewards[k].reward.Amount;
										InventoryItemTemplate itemTemplate2 = InventoryWideData.Get().GetItemTemplate(gameBalanceVars.RepeatingCharacterLevelRewards[k].reward.ItemTemplateId);
										rewardData2.Name = itemTemplate2.GetDisplayName();
										rewardData2.SpritePath = itemTemplate2.IconPath;
										rewardData2.Level = gameBalanceVars.RepeatingCharacterLevelRewards[k].startLevel;
										rewardData2.InventoryTemplate = itemTemplate2;
										rewardData2.Type = RewardUtils.GetRewardTypeFromInventoryItem(itemTemplate2);
										rewardData2.isRepeating = true;
										rewardData2.repeatLevels = gameBalanceVars.RepeatingCharacterLevelRewards[k].repeatingLevel;
										list3.Add(rewardData2);
									}
								}
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
										goto end_IL_04c2;
									}
									continue;
									end_IL_04c2:
									break;
								}
							}
						}
						for (int l = 0; l < list2.Count; l++)
						{
							EndGameRewardItem endGameRewardItem = UnityEngine.Object.Instantiate(m_endGameReward);
							UIManager.ReparentTransform(endGameRewardItem.transform, m_RewardsGrid.transform);
							UIManager.SetGameObjectActive(endGameRewardItem, true);
							endGameRewardItem.Setup(list2[l], CharacterType.None);
							_MouseEventPasser mouseEventPasser = endGameRewardItem.m_tooltipHoverObj.gameObject.AddComponent<_MouseEventPasser>();
							mouseEventPasser.AddNewHandler(m_RewardsScrollRect);
						}
						for (int m = 0; m < list3.Count; m++)
						{
							EndGameRewardItem endGameRewardItem2 = UnityEngine.Object.Instantiate(m_endGameReward);
							UIManager.ReparentTransform(endGameRewardItem2.transform, m_RewardsGrid.transform);
							UIManager.SetGameObjectActive(endGameRewardItem2, true);
							endGameRewardItem2.Setup(list3[m], playersOriginalActorData.m_characterType);
							_MouseEventPasser mouseEventPasser2 = endGameRewardItem2.m_tooltipHoverObj.gameObject.AddComponent<_MouseEventPasser>();
							mouseEventPasser2.AddNewHandler(m_RewardsScrollRect);
						}
						while (true)
						{
							NumRewardsEarned = list2.Count + list3.Count;
							m_rewardNumberText.text = NumRewardsEarned.ToString();
							return;
						}
					}
				}
			}
		}
	}

	private void SetupStatRecapScreen()
	{
		Team teamViewing = GameFlowData.Get().LocalPlayerData.GetTeamViewing();
		int playerId = GameManager.Get().PlayerInfo.PlayerId;
		MatchResultsStats stats = GameplayUtils.GenerateStatsFromGame(teamViewing, playerId);
		SetupTeamMemberList(stats);
		SetupExpBars();
		SetupCurrencyAndInfluenceDisplays();
		SetupRankMode();
		SetupRewardsScreen();
	}

	private void SetupBadges()
	{
		if (BadgesAreSet)
		{
			return;
		}
		while (true)
		{
			if (!NotificationArrived)
			{
				return;
			}
			while (true)
			{
				if (!(GameManager.Get() != null))
				{
					return;
				}
				while (true)
				{
					if (GameManager.Get().PlayerInfo != null && !m_results.BadgeAndParticipantsInfo.IsNullOrEmpty())
					{
						foreach (BadgeAndParticipantInfo item in m_results.BadgeAndParticipantsInfo)
						{
							if (item.PlayerId == GameManager.Get().PlayerInfo.PlayerId)
							{
								while (true)
								{
									switch (3)
									{
									case 0:
										break;
									default:
										if (item.BadgesEarned != null)
										{
											while (true)
											{
												switch (6)
												{
												case 0:
													break;
												default:
												{
													List<BadgeInfo> badgesEarned = item.BadgesEarned;
													
													badgesEarned.Sort(delegate(BadgeInfo x, BadgeInfo y)
														{
															if (x == null && y == null)
															{
																while (true)
																{
																	switch (6)
																	{
																	case 0:
																		break;
																	default:
																		return 0;
																	}
																}
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
																while (true)
																{
																	switch (6)
																	{
																	case 0:
																		break;
																	default:
																		return 1;
																	}
																}
															}
															if (badgeInfo2 == null)
															{
																while (true)
																{
																	switch (4)
																	{
																	case 0:
																		break;
																	default:
																		return -1;
																	}
																}
															}
															if (badgeInfo.Quality == badgeInfo2.Quality)
															{
																return 0;
															}
															if (badgeInfo.Quality > badgeInfo2.Quality)
															{
																while (true)
																{
																	switch (3)
																	{
																	case 0:
																		break;
																	default:
																		return -1;
																	}
																}
															}
															return (badgeInfo.Quality < badgeInfo2.Quality) ? 1 : 0;
														});
													for (int i = 0; i < item.BadgesEarned.Count; i++)
													{
														UIGameOverBadgeWidget uIGameOverBadgeWidget = UnityEngine.Object.Instantiate(m_BadgePrefab);
														uIGameOverBadgeWidget.Setup(item.BadgesEarned[i], GameManager.Get().PlayerInfo.CharacterType, item.GlobalPercentiles);
														UIManager.ReparentTransform(uIGameOverBadgeWidget.transform, m_PersonalHighlightBadgesContainer.transform);
														UIGameOverBadgeWidget uIGameOverBadgeWidget2 = UnityEngine.Object.Instantiate(m_BadgePrefab);
														uIGameOverBadgeWidget2.Setup(item.BadgesEarned[i], GameManager.Get().PlayerInfo.CharacterType, item.GlobalPercentiles);
														UIManager.ReparentTransform(uIGameOverBadgeWidget2.transform, m_StatPadgeBadgesContainer.transform);
													}
													while (true)
													{
														switch (3)
														{
														case 0:
															break;
														default:
															BadgesAreSet = true;
															return;
														}
													}
												}
												}
											}
										}
										return;
									}
								}
							}
						}
					}
					return;
				}
			}
		}
	}

	private void SetupTopParticipants()
	{
		if (!NotificationArrived)
		{
			return;
		}
		while (true)
		{
			int num = 0;
			if (!m_results.BadgeAndParticipantsInfo.IsNullOrEmpty())
			{
				List<TopParticipantSlot> list = new List<TopParticipantSlot>();
				IEnumerator enumerator = Enum.GetValues(typeof(TopParticipantSlot)).GetEnumerator();
				try
				{
					while (enumerator.MoveNext())
					{
						TopParticipantSlot item = (TopParticipantSlot)enumerator.Current;
						list.Add(item);
					}
				}
				finally
				{
					IDisposable disposable;
					if ((disposable = (enumerator as IDisposable)) != null)
					{
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								disposable.Dispose();
								goto end_IL_0083;
							}
						}
					}
					end_IL_0083:;
				}
				
				list.Sort(delegate(TopParticipantSlot x, TopParticipantSlot y)
					{
						int num2 = BadgeAndParticipantInfo.ParticipantOrderDisplayPriority(x);
						int num3 = BadgeAndParticipantInfo.ParticipantOrderDisplayPriority(y);
						if (num2 != num3)
						{
							int result;
							if (num2 > num3)
							{
								result = -1;
							}
							else
							{
								result = 1;
							}
							return result;
						}
						return 0;
					});
				using (List<TopParticipantSlot>.Enumerator enumerator2 = list.GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						TopParticipantSlot current = enumerator2.Current;
						using (List<BadgeAndParticipantInfo>.Enumerator enumerator3 = m_results.BadgeAndParticipantsInfo.GetEnumerator())
						{
							while (true)
							{
								if (!enumerator3.MoveNext())
								{
									break;
								}
								BadgeAndParticipantInfo current2 = enumerator3.Current;
								if (!current2.TopParticipationEarned.IsNullOrEmpty())
								{
									if (current2.TopParticipationEarned.Contains(current))
									{
										while (true)
										{
											switch (1)
											{
											case 0:
												break;
											default:
												if (num < m_TopParticipantWidgets.Length)
												{
													m_TopParticipantWidgets[num].Setup(current, current2);
													num++;
												}
												goto end_IL_00fb;
											}
										}
									}
								}
							}
							end_IL_00fb:;
						}
					}
				}
			}
			for (int i = num; i < m_TopParticipantWidgets.Length; i++)
			{
				UIManager.SetGameObjectActive(m_TopParticipantWidgets[i], false);
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

	private void SetupStatsScreen()
	{
		UIGameOverStatWidget[] componentsInChildren = m_freelancerStatGrid.GetComponentsInChildren<UIGameOverStatWidget>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[i].gameObject);
		}
		componentsInChildren = m_generalStatGrid.GetComponentsInChildren<UIGameOverStatWidget>(true);
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			UnityEngine.Object.Destroy(componentsInChildren[j].gameObject);
		}
		while (true)
		{
			componentsInChildren = m_firepowerStatGrid.GetComponentsInChildren<UIGameOverStatWidget>(true);
			for (int k = 0; k < componentsInChildren.Length; k++)
			{
				UnityEngine.Object.Destroy(componentsInChildren[k].gameObject);
			}
			while (true)
			{
				componentsInChildren = m_supportStatGrid.GetComponentsInChildren<UIGameOverStatWidget>(true);
				for (int l = 0; l < componentsInChildren.Length; l++)
				{
					UnityEngine.Object.Destroy(componentsInChildren[l].gameObject);
				}
				while (true)
				{
					componentsInChildren = m_frontlineStatGrid.GetComponentsInChildren<UIGameOverStatWidget>(true);
					for (int m = 0; m < componentsInChildren.Length; m++)
					{
						UnityEngine.Object.Destroy(componentsInChildren[m].gameObject);
					}
					while (true)
					{
						if (m_statsAtBeginningOfMatch == null)
						{
							while (true)
							{
								switch (5)
								{
								case 0:
									break;
								default:
									if (ReplayPlayManager.Get() != null)
									{
										PersistedCharacterMatchData playbackMatchData = ReplayPlayManager.Get().GetPlaybackMatchData();
										if (playbackMatchData != null)
										{
											CharacterType firstPlayerCharacter = playbackMatchData.MatchComponent.GetFirstPlayerCharacter();
											CharacterResourceLink characterResourceLink = GameWideData.Get().GetCharacterResourceLink(firstPlayerCharacter);
											m_CharacterImage.sprite = characterResourceLink.GetCharacterSelectIcon();
											for (int n = 0; n < 4; n++)
											{
												UIGameOverStatWidget uIGameOverStatWidget = UnityEngine.Object.Instantiate(m_freelancerStatPrefab);
												AbilityData component = characterResourceLink.ActorDataPrefab.GetComponent<AbilityData>();
												uIGameOverStatWidget.SetupReplayFreelancerStat(firstPlayerCharacter, playbackMatchData.MatchFreelancerStats, n, component);
												UIManager.ReparentTransform(uIGameOverStatWidget.transform, m_freelancerStatGrid.transform);
												m_GameOverStatWidgets.Add(uIGameOverStatWidget);
											}
											while (true)
											{
												switch (1)
												{
												case 0:
													break;
												default:
												{
													StatDisplaySettings.StatType[] generalStats = StatDisplaySettings.GeneralStats;
													foreach (StatDisplaySettings.StatType typeOfStat in generalStats)
													{
														UIGameOverStatWidget uIGameOverStatWidget2 = UnityEngine.Object.Instantiate(m_generalStatPrefab);
														uIGameOverStatWidget2.SetupReplayStat(playbackMatchData.MatchFreelancerStats, typeOfStat, firstPlayerCharacter);
														UIManager.ReparentTransform(uIGameOverStatWidget2.transform, m_generalStatGrid.transform);
														m_GameOverStatWidgets.Add(uIGameOverStatWidget2);
													}
													while (true)
													{
														switch (6)
														{
														case 0:
															break;
														default:
														{
															StatDisplaySettings.StatType[] firepowerStats = StatDisplaySettings.FirepowerStats;
															foreach (StatDisplaySettings.StatType typeOfStat2 in firepowerStats)
															{
																UIGameOverStatWidget uIGameOverStatWidget3 = UnityEngine.Object.Instantiate(m_generalStatPrefab);
																uIGameOverStatWidget3.SetupReplayStat(playbackMatchData.MatchFreelancerStats, typeOfStat2, firstPlayerCharacter);
																UIManager.ReparentTransform(uIGameOverStatWidget3.transform, m_firepowerStatGrid.transform);
																m_GameOverStatWidgets.Add(uIGameOverStatWidget3);
															}
															StatDisplaySettings.StatType[] supportStats = StatDisplaySettings.SupportStats;
															foreach (StatDisplaySettings.StatType typeOfStat3 in supportStats)
															{
																UIGameOverStatWidget uIGameOverStatWidget4 = UnityEngine.Object.Instantiate(m_generalStatPrefab);
																uIGameOverStatWidget4.SetupReplayStat(playbackMatchData.MatchFreelancerStats, typeOfStat3, firstPlayerCharacter);
																UIManager.ReparentTransform(uIGameOverStatWidget4.transform, m_supportStatGrid.transform);
																m_GameOverStatWidgets.Add(uIGameOverStatWidget4);
															}
															while (true)
															{
																switch (7)
																{
																case 0:
																	break;
																default:
																{
																	StatDisplaySettings.StatType[] frontlinerStats = StatDisplaySettings.FrontlinerStats;
																	foreach (StatDisplaySettings.StatType typeOfStat4 in frontlinerStats)
																	{
																		UIGameOverStatWidget uIGameOverStatWidget5 = UnityEngine.Object.Instantiate(m_generalStatPrefab);
																		uIGameOverStatWidget5.SetupReplayStat(playbackMatchData.MatchFreelancerStats, typeOfStat4, firstPlayerCharacter);
																		UIManager.ReparentTransform(uIGameOverStatWidget5.transform, m_frontlineStatGrid.transform);
																		m_GameOverStatWidgets.Add(uIGameOverStatWidget5);
																	}
																	while (true)
																	{
																		switch (2)
																		{
																		case 0:
																			break;
																		default:
																			if (playbackMatchData.MatchFreelancerStats == null)
																			{
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
																			ClientGameManager.Get().CalculateFreelancerStats(playbackMatchData.MatchFreelancerStats.PersistedStatBucket, firstPlayerCharacter, playbackMatchData.MatchFreelancerStats, delegate(CalculateFreelancerStatsResponse response)
																			{
																				if (response.Success)
																				{
																					using (List<UIGameOverStatWidget>.Enumerator enumerator = m_GameOverStatWidgets.GetEnumerator())
																					{
																						while (enumerator.MoveNext())
																						{
																							UIGameOverStatWidget current = enumerator.Current;
																							if (current.DisplayStatType == UIGameOverStatWidget.StatDisplayType.None)
																							{
																								current.UpdatePercentiles(null);
																							}
																							else if (current.DisplayStatType == UIGameOverStatWidget.StatDisplayType.GeneralStat && !response.GlobalPercentiles.IsNullOrEmpty())
																							{
																								current.UpdatePercentiles(response.GlobalPercentiles[current.GeneralStatType]);
																							}
																							else if (current.DisplayStatType == UIGameOverStatWidget.StatDisplayType.FreelancerStat && !response.FreelancerSpecificPercentiles.IsNullOrEmpty())
																							{
																								current.UpdatePercentiles(response.FreelancerSpecificPercentiles[current.FreelancerStat]);
																							}
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
																				if (response.LocalizedFailure != null)
																				{
																					while (true)
																					{
																						switch (6)
																						{
																						case 0:
																							break;
																						default:
																							TextConsole.Get().Write(response.LocalizedFailure.ToString());
																							return;
																						}
																					}
																				}
																			});
																			return;
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
										}
									}
									return;
								}
							}
						}
						ActorData playersOriginalActorData = GetPlayersOriginalActorData();
						if (playersOriginalActorData != null)
						{
							m_CharacterImage.sprite = playersOriginalActorData.GetCharacterResourceLink().GetCharacterSelectIcon();
							m_PersonalHighlightsCharacterImage.sprite = m_CharacterImage.sprite;
						}
						else
						{
							UIManager.SetGameObjectActive(m_PersonalHighlightsCharacterImage, false);
							UIManager.SetGameObjectActive(m_CharacterImage, false);
						}
						m_GameOverStatWidgets.Clear();
						ActorData playersOriginalActorData2 = GetPlayersOriginalActorData();
						FreelancerStats freelancerStats = playersOriginalActorData2.GetFreelancerStats();
						ActorBehavior actorBehavior = playersOriginalActorData2.GetActorBehavior();
						for (int num5 = 0; num5 < freelancerStats.GetNumStats(); num5++)
						{
							UIGameOverStatWidget uIGameOverStatWidget6 = UnityEngine.Object.Instantiate(m_freelancerStatPrefab);
							uIGameOverStatWidget6.SetupForFreelancerStats(m_statsAtBeginningOfMatch, actorBehavior, freelancerStats, num5, playersOriginalActorData2.GetAbilityData());
							uIGameOverStatWidget6.UpdatePercentiles(GetFreelancerStatPercentiles(num5));
							UIManager.ReparentTransform(uIGameOverStatWidget6.transform, m_freelancerStatGrid.transform);
							m_GameOverStatWidgets.Add(uIGameOverStatWidget6);
						}
						while (true)
						{
							StatDisplaySettings.StatType[] generalStats2 = StatDisplaySettings.GeneralStats;
							foreach (StatDisplaySettings.StatType typeOfStat5 in generalStats2)
							{
								UIGameOverStatWidget uIGameOverStatWidget7 = UnityEngine.Object.Instantiate(m_generalStatPrefab);
								uIGameOverStatWidget7.SetupForAStat(m_statsAtBeginningOfMatch, actorBehavior, typeOfStat5);
								uIGameOverStatWidget7.UpdatePercentiles(GetStatPercentiles(uIGameOverStatWidget7.GeneralStatType));
								UIManager.ReparentTransform(uIGameOverStatWidget7.transform, m_generalStatGrid.transform);
								m_GameOverStatWidgets.Add(uIGameOverStatWidget7);
							}
							while (true)
							{
								StatDisplaySettings.StatType[] firepowerStats2 = StatDisplaySettings.FirepowerStats;
								foreach (StatDisplaySettings.StatType typeOfStat6 in firepowerStats2)
								{
									UIGameOverStatWidget uIGameOverStatWidget8 = UnityEngine.Object.Instantiate(m_generalStatPrefab);
									uIGameOverStatWidget8.SetupForAStat(m_statsAtBeginningOfMatch, actorBehavior, typeOfStat6);
									uIGameOverStatWidget8.UpdatePercentiles(GetStatPercentiles(uIGameOverStatWidget8.GeneralStatType));
									UIManager.ReparentTransform(uIGameOverStatWidget8.transform, m_firepowerStatGrid.transform);
									m_GameOverStatWidgets.Add(uIGameOverStatWidget8);
								}
								StatDisplaySettings.StatType[] supportStats2 = StatDisplaySettings.SupportStats;
								foreach (StatDisplaySettings.StatType typeOfStat7 in supportStats2)
								{
									UIGameOverStatWidget uIGameOverStatWidget9 = UnityEngine.Object.Instantiate(m_generalStatPrefab);
									uIGameOverStatWidget9.SetupForAStat(m_statsAtBeginningOfMatch, actorBehavior, typeOfStat7);
									uIGameOverStatWidget9.UpdatePercentiles(GetStatPercentiles(uIGameOverStatWidget9.GeneralStatType));
									UIManager.ReparentTransform(uIGameOverStatWidget9.transform, m_supportStatGrid.transform);
									m_GameOverStatWidgets.Add(uIGameOverStatWidget9);
								}
								while (true)
								{
									StatDisplaySettings.StatType[] frontlinerStats2 = StatDisplaySettings.FrontlinerStats;
									foreach (StatDisplaySettings.StatType typeOfStat8 in frontlinerStats2)
									{
										UIGameOverStatWidget uIGameOverStatWidget10 = UnityEngine.Object.Instantiate(m_generalStatPrefab);
										uIGameOverStatWidget10.SetupForAStat(m_statsAtBeginningOfMatch, actorBehavior, typeOfStat8);
										uIGameOverStatWidget10.UpdatePercentiles(GetStatPercentiles(uIGameOverStatWidget10.GeneralStatType));
										UIManager.ReparentTransform(uIGameOverStatWidget10.transform, m_frontlineStatGrid.transform);
										m_GameOverStatWidgets.Add(uIGameOverStatWidget10);
									}
									while (true)
									{
										List<UIGameOverStatWidget> list = new List<UIGameOverStatWidget>();
										int num10 = 0;
										while (list.Count < m_PersonalHighlightWidgets.Length)
										{
											if (num10 < 3)
											{
												for (int num11 = 0; num11 < m_GameOverStatWidgets.Count; num11++)
												{
													if (list.Contains(m_GameOverStatWidgets[num11]))
													{
														continue;
													}
													if (num10 == 0)
													{
														if (m_GameOverStatWidgets[num11].BeatRecord())
														{
															list.Add(m_GameOverStatWidgets[num11]);
															continue;
														}
													}
													if (num10 == 1 && m_GameOverStatWidgets[num11].BeatAverage())
													{
														list.Add(m_GameOverStatWidgets[num11]);
													}
													else if (num10 >= 2)
													{
														list.Add(m_GameOverStatWidgets[num11]);
													}
												}
												while (true)
												{
													switch (1)
													{
													case 0:
														break;
													default:
														goto end_IL_0849;
													}
													continue;
													end_IL_0849:
													break;
												}
												num10++;
												continue;
											}
											break;
										}
										for (int num12 = 0; num12 < m_PersonalHighlightWidgets.Length; num12++)
										{
											if (num12 < list.Count)
											{
												if (list[num12].DisplayStatType == UIGameOverStatWidget.StatDisplayType.FreelancerStat)
												{
													m_PersonalHighlightWidgets[num12].SetupForFreelancerStats(m_statsAtBeginningOfMatch, actorBehavior, freelancerStats, list[num12].FreelancerStat, playersOriginalActorData2.GetAbilityData());
												}
												else if (list[num12].DisplayStatType == UIGameOverStatWidget.StatDisplayType.GeneralStat)
												{
													m_PersonalHighlightWidgets[num12].SetupForAStat(m_statsAtBeginningOfMatch, actorBehavior, list[num12].GeneralStatType);
												}
											}
											UIManager.SetGameObjectActive(m_PersonalHighlightWidgets[num12], false);
										}
										return;
									}
								}
							}
						}
					}
				}
			}
		}
	}

	private void SetupRankMode()
	{
		bool flag = m_gameType == GameType.Ranked;
		bool flag2 = flag;
		TierPlacement tierCurrent;
		if (flag)
		{
			tierCurrent = ClientGameManager.Get().TierCurrent;
			if (tierCurrent != null)
			{
				if (tierCurrent.Tier != -1)
				{
					goto IL_0057;
				}
			}
			flag2 = false;
			goto IL_0057;
		}
		goto IL_0118;
		IL_0118:
		UIManager.SetGameObjectActive(m_rankModeLevelContainer, flag2);
		return;
		IL_0057:
		float fillAmount;
		if (flag2)
		{
			UIManager.SetGameObjectActive(m_rankModeLevelAnimator, false);
			fillAmount = GetRankFillAmt(tierCurrent.Points * 0.01f);
			if (tierCurrent.Tier != 1)
			{
				if (tierCurrent.Tier != 2)
				{
					goto IL_00a0;
				}
			}
			fillAmount = 1f;
			goto IL_00a0;
		}
		goto IL_0118;
		IL_00a0:
		m_rankNormalBar.fillAmount = fillAmount;
		m_rankDecreaseBar.fillAmount = fillAmount;
		m_rankIncreaseBar.fillAmount = fillAmount;
		int tier = tierCurrent.Tier;
		SetupTierDisplay(tier, tierCurrent.Points);
		m_rankPointsText.text = "0.0";
		UIManager.SetGameObjectActive(m_rankDecreaseBar, !SelfWon);
		UIManager.SetGameObjectActive(m_rankIncreaseBar, SelfWon);
		goto IL_0118;
	}

	private void SetupGGBoostScreen()
	{
		UpdateGGBoostPlayerList(false);
		m_numSelfGGpacksUsed = HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.NumberGGPacksUsed(GameManager.Get().PlayerInfo.Handle);
		for (int i = 0; i < m_ggButtonLevels.Length; i++)
		{
			UIManager.SetGameObjectActive(m_ggButtonLevels[i], i == m_numSelfGGpacksUsed);
		}
		while (true)
		{
			int currentAmount = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack);
			for (int j = 0; j < m_ggButtonLevelsAnims.Length; j++)
			{
				if (!m_ggButtonLevelsAnims[j].gameObject.activeInHierarchy)
				{
					continue;
				}
				if (currentAmount > 0)
				{
					UIAnimationEventManager.Get().PlayAnimation(m_ggButtonLevelsAnims[j], "GGBoostDefaultIN", null, string.Empty);
				}
				else
				{
					UIAnimationEventManager.Get().PlayAnimation(m_ggButtonLevelsAnims[j], "GGBoostNoMoreDefaultIN", null, string.Empty);
				}
			}
			HandleBankBalanceChange(null);
			return;
		}
	}

	public void UpdateGGBoostPlayerList(bool setGGLevel = true)
	{
		Team teamId = GameManager.Get().PlayerInfo.TeamId;
		int num = 0;
		int num2 = 0;
		if (HUD_UI.Get() != null)
		{
			using (List<LobbyPlayerInfo>.Enumerator enumerator = GameManager.Get().TeamInfo.TeamPlayerInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LobbyPlayerInfo current = enumerator.Current;
					if (current.CharacterType != 0)
					{
						int num3 = HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.NumberGGPacksUsed(current.Handle);
						if (current.TeamId != teamId)
						{
							if (teamId != Team.Spectator || current.TeamId != 0)
							{
								if (num2 < m_redTeamGGProfiles.Length)
								{
									UIManager.SetGameObjectActive(m_redTeamGGProfiles[num2], true);
									m_redTeamGGProfiles[num2].Setup(GameWideData.Get().GetCharacterResourceLink(current.CharacterType), current, true, true);
									UILoadscreenProfile obj = m_redTeamGGProfiles[num2];
									int gGButtonLevel;
									if (setGGLevel)
									{
										gGButtonLevel = num3;
									}
									else
									{
										gGButtonLevel = 0;
									}
									obj.SetGGButtonLevel(gGButtonLevel);
									num2++;
								}
								continue;
							}
						}
						if (num < m_blueTeamGGProfiles.Length)
						{
							UIManager.SetGameObjectActive(m_blueTeamGGProfiles[num], true);
							m_blueTeamGGProfiles[num].Setup(GameWideData.Get().GetCharacterResourceLink(current.CharacterType), current, false, true);
							UILoadscreenProfile obj2 = m_blueTeamGGProfiles[num];
							int gGButtonLevel2;
							if (setGGLevel)
							{
								gGButtonLevel2 = num3;
							}
							else
							{
								gGButtonLevel2 = 0;
							}
							obj2.SetGGButtonLevel(gGButtonLevel2);
							num++;
						}
					}
				}
			}
		}
		for (int i = num; i < m_blueTeamGGProfiles.Length; i++)
		{
			UIManager.SetGameObjectActive(m_blueTeamGGProfiles[i], false);
		}
		for (int j = num2; j < m_redTeamGGProfiles.Length; j++)
		{
			UIManager.SetGameObjectActive(m_redTeamGGProfiles[j], false);
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

	private float GetRankFillAmt(float percent)
	{
		return percent * 0.833f + 0.082f;
	}

	private bool SetupTierDisplay(int tier, float tierPoints)
	{
		if (LastRankTierDisplayed == tier)
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
		LastRankTierDisplayed = tier;
		string tierIconResource = UIRankedModeSelectScreen.GetTierIconResource(tier);
		string tierName = ClientGameManager.Get().GetTierName(GameType.Ranked, tier);
		if (!tierIconResource.IsNullOrEmpty())
		{
			m_rankIcon.sprite = (Sprite)Resources.Load(tierIconResource, typeof(Sprite));
			if (tierIconResource.ToLower().Contains("bronze"))
			{
				m_rankDecreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Bronze_decrease", typeof(Sprite));
				m_rankIncreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Bronze_increase", typeof(Sprite));
				m_rankNormalBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Bronze_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("silver"))
			{
				m_rankDecreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Silver_decrease", typeof(Sprite));
				m_rankIncreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Silver_increase", typeof(Sprite));
				m_rankNormalBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Silver_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("gold"))
			{
				m_rankDecreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Gold_decrease", typeof(Sprite));
				m_rankIncreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Gold_increase", typeof(Sprite));
				m_rankNormalBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Gold_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("platinum"))
			{
				m_rankDecreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Platinum_decrease", typeof(Sprite));
				m_rankIncreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Platinum_increase", typeof(Sprite));
				m_rankNormalBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Platinum_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("diamond"))
			{
				m_rankDecreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Diamond_decrease", typeof(Sprite));
				m_rankIncreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Diamond_increase", typeof(Sprite));
				m_rankNormalBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Diamond_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("master"))
			{
				m_rankDecreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Master_decrease", typeof(Sprite));
				m_rankIncreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Master_increase", typeof(Sprite));
				m_rankNormalBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Master_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("contender"))
			{
				m_rankDecreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Contender_decrease", typeof(Sprite));
				m_rankIncreaseBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Contender_increase", typeof(Sprite));
				m_rankNormalBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Contender_fill", typeof(Sprite));
			}
		}
		else
		{
			Log.Warning("Did not find icon for tier: " + tier);
		}
		string[] array = tierName.Split(' ');
		if (array != null)
		{
			if (array.Length > 0)
			{
				m_rankTeirText.text = array[0];
			}
			if (array.Length > 1)
			{
				m_rankLevelText.text = array[1];
				ShouldDisplayTierPoints = false;
			}
			else
			{
				m_rankLevelText.text = Mathf.RoundToInt(tierPoints).ToString();
				ShouldDisplayTierPoints = true;
			}
		}
		else
		{
			m_rankTeirText.text = tierName;
			m_rankLevelText.text = string.Empty;
			ShouldDisplayTierPoints = false;
		}
		return true;
	}

	public void SetVisible(bool visible)
	{
		IsVisible = visible;
		if (!IsVisible)
		{
			UIManager.SetGameObjectActive(m_GGBoostContainer, false);
			UIManager.SetGameObjectActive(m_TopBottomBarsContainer, false);
			UIManager.SetGameObjectActive(m_MouseClickContainer, false);
		}
	}

	private void CheckPreGameStats()
	{
		if (!(GameManager.Get() != null))
		{
			return;
		}
		while (true)
		{
			if (GameManager.Get().GameStatus != GameStatus.Started)
			{
				return;
			}
			while (true)
			{
				if (GameManager.Get().GameInfo == null)
				{
					return;
				}
				while (true)
				{
					if (GameManager.Get().GameInfo.GameConfig == null)
					{
						return;
					}
					while (true)
					{
						if (GameManager.Get().GameInfo.GameConfig.InstanceSubType == null)
						{
							return;
						}
						while (true)
						{
							if (GameManager.Get().PlayerInfo == null)
							{
								return;
							}
							while (true)
							{
								if (GameManager.Get().PlayerInfo.IsSpectator)
								{
									return;
								}
								while (true)
								{
									if (!ClientGameManager.Get().IsPlayerAccountDataAvailable())
									{
										return;
									}
									while (true)
									{
										if (!ClientGameManager.Get().IsPlayerCharacterDataAvailable())
										{
											return;
										}
										while (true)
										{
											if (m_statsAtBeginningOfMatch == null)
											{
												while (true)
												{
													PersistedStatBucket persistedStatBucket = GameManager.Get().GameInfo.GameConfig.InstanceSubType.PersistedStatBucket;
													Dictionary<PersistedStatBucket, PersistedStats> persistedStatsDictionary = ClientGameManager.Get().GetPlayerCharacterData(GameManager.Get().PlayerInfo.CharacterType).ExperienceComponent.PersistedStatsDictionary;
													PersistedStats persistedStats = null;
													persistedStats = ((!persistedStatsDictionary.ContainsKey(persistedStatBucket)) ? new PersistedStats() : persistedStatsDictionary[persistedStatBucket]);
													m_statsAtBeginningOfMatch = (PersistedStats)persistedStats.Clone();
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
				}
			}
		}
	}

	public void HandleMatchResultsNotification(MatchResultsNotification notification)
	{
		m_results = notification;
		if (m_results.FirstWinOccured)
		{
			QuestCompletePanel.Get().AddSpecialQuestNotification(GameBalanceVars.Get().FirstWinOfDayQuestId);
			m_results.QuestXpGained += m_results.FirstWinXpGained;
		}
		if (GameManager.Get().GameConfig != null)
		{
			GameType gameType = GameManager.Get().GameConfig.GameType;
			if (gameType == GameType.Custom)
			{
				goto IL_0108;
			}
			if (gameType == GameType.Tutorial)
			{
				if (m_results.SeasonLevelAtStart > 1)
				{
					goto IL_0108;
				}
			}
			PersistedAccountData playerAccountData = ClientGameManager.Get().GetPlayerAccountData();
			SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(playerAccountData.QuestComponent.ActiveSeason);
			if (seasonTemplate != null)
			{
				if (seasonTemplate.IsTutorial)
				{
					UITutorialSeasonInterstitial.Get().Setup(seasonTemplate, playerAccountData.QuestComponent.SeasonLevel - 1, true);
				}
			}
		}
		goto IL_0115;
		IL_0115:
		if (notification.BadgeAndParticipantsInfo.IsNullOrEmpty())
		{
			return;
		}
		int playerId;
		while (true)
		{
			playerId = GameManager.Get().PlayerInfo.PlayerId;
			BadgeAndParticipantInfo badgeAndParticipantInfo = notification.BadgeAndParticipantsInfo.Find((BadgeAndParticipantInfo p) => p.PlayerId == playerId);
			if (badgeAndParticipantInfo == null)
			{
				return;
			}
			while (true)
			{
				if (!badgeAndParticipantInfo.GlobalPercentiles.IsNullOrEmpty())
				{
					using (Dictionary<StatDisplaySettings.StatType, PercentileInfo>.Enumerator enumerator = badgeAndParticipantInfo.GlobalPercentiles.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							KeyValuePair<StatDisplaySettings.StatType, PercentileInfo> kvPair = enumerator.Current;
							UIGameOverStatWidget uIGameOverStatWidget = m_GameOverStatWidgets.Find((UIGameOverStatWidget p) => p.GeneralStatType == kvPair.Key && p.DisplayStatType == UIGameOverStatWidget.StatDisplayType.GeneralStat);
							if (uIGameOverStatWidget != null)
							{
								uIGameOverStatWidget.UpdatePercentiles(kvPair.Value);
							}
						}
					}
				}
				if (!badgeAndParticipantInfo.FreelancerSpecificPercentiles.IsNullOrEmpty())
				{
					while (true)
					{
						using (Dictionary<int, PercentileInfo>.Enumerator enumerator2 = badgeAndParticipantInfo.FreelancerSpecificPercentiles.GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								KeyValuePair<int, PercentileInfo> ivPair = enumerator2.Current;
								UIGameOverStatWidget uIGameOverStatWidget2 = m_GameOverStatWidgets.Find(delegate(UIGameOverStatWidget p)
								{
									int result;
									if (p.FreelancerStat == ivPair.Key)
									{
										result = ((p.DisplayStatType == UIGameOverStatWidget.StatDisplayType.FreelancerStat) ? 1 : 0);
									}
									else
									{
										result = 0;
									}
									return (byte)result != 0;
								});
								if (uIGameOverStatWidget2 != null)
								{
									uIGameOverStatWidget2.UpdatePercentiles(ivPair.Value);
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
				return;
			}
		}
		IL_0108:
		UITutorialSeasonInterstitial.Get().SetVisible(false);
		goto IL_0115;
	}

	public void NotifySelfGGPackUsed()
	{
		RequestedToUseGGPack = false;
		if (m_currentSubState == null)
		{
			return;
		}
		while (true)
		{
			UpdateGGBoostPlayerList();
			return;
		}
	}

	private void HandleGGPackUsed(UseGGPackNotification notification)
	{
		if (m_currentSubState == null)
		{
			return;
		}
		while (true)
		{
			UpdateGGBoostPlayerList();
			return;
		}
	}

	public void UpdateEndGameGGBonuses(int iso, float xp)
	{
		m_GGPack_XPMult = xp;
		SetWorldGGPackText(string.Format(StringUtil.TR("EndGameGGBonuses", "GameOver"), Mathf.RoundToInt((m_GGPack_XPMult - 1f) * 100f)));
		if (m_GGPack_XPMult < 2f)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					UIAnimationEventManager.Get().PlayAnimation(m_PercentageAnimator, "GGBoostBluePercentageDefaultIN", null, string.Empty);
					return;
				}
			}
		}
		if (m_GGPack_XPMult < 3f)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					UIAnimationEventManager.Get().PlayAnimation(m_PercentageAnimator, "GGBoostSilverPercentageDefaultIN", null, string.Empty);
					return;
				}
			}
		}
		if (m_GGPack_XPMult < 4f)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					UIAnimationEventManager.Get().PlayAnimation(m_PercentageAnimator, "GGBoostGoldPercentageDefaultIN", null, string.Empty);
					return;
				}
			}
		}
		UIAnimationEventManager.Get().PlayAnimation(m_PercentageAnimator, "GGBoostMaxPercentageDefaultIN", null, string.Empty);
	}

	public static void SetupTeamMemberList(MatchResultsStats stats)
	{
		UIGameOverPlayerEntry.PreSetupInitialization();
		UIGameStatsWindow.Get().SetupTeamMemberList(stats);
	}

	public void Setup(GameType gameType, GameResult gameResult, int myTeamScore, int enemyTeamScore)
	{
		m_gameResult = gameResult;
		m_gameType = gameType;
		ActorData playersOriginalActorData = GetPlayersOriginalActorData();
		if (playersOriginalActorData != null)
		{
			m_characterExpBarImage.sprite = playersOriginalActorData.GetCharacterResourceLink().GetCharacterSelectIcon();
		}
		GameBalanceVars.PlayerBanner currentBackgroundBanner = ClientGameManager.Get().GetCurrentBackgroundBanner();
		if (currentBackgroundBanner != null)
		{
			m_bannerBG.sprite = (Sprite)Resources.Load(currentBackgroundBanner.m_resourceString, typeof(Sprite));
		}
		GameBalanceVars.PlayerBanner currentForegroundBanner = ClientGameManager.Get().GetCurrentForegroundBanner();
		if (currentForegroundBanner != null)
		{
			m_bannerFG.sprite = (Sprite)Resources.Load(currentForegroundBanner.m_resourceString, typeof(Sprite));
		}
		if (UIGameStatsWindow.Get() != null)
		{
			UIGameStatsWindow.Get().SetVisible(false);
		}
		GameOverWorldObjects.Get().Setup(gameResult, FriendlyTeam, m_GGPack_XPMult);
		UIManager.SetGameObjectActive(m_redTeamVictoryContainer, !SelfWon);
		UIManager.SetGameObjectActive(m_blueTeamVictoryContainer, SelfWon);
		bool doActive = GameManager.Get().GameplayOverrides?.EnableFacebook ?? false;
		UIManager.SetGameObjectActive(m_shareFacebookButton, doActive);
		SetVisible(true);
		SetupLabelText(myTeamScore, enemyTeamScore);
		EstimatedNotificationArrivalTime = Time.unscaledTime + GameBalanceVars.Get().GGPackEndGameUsageTimer;
		m_currentSubState = new GameOverSubState(GameOverScreenState.VictoryDefeat, VictoryDefeatDisplayTimeDuration);
		ContinueBtnFailSafeTime = Time.unscaledTime;
		if (GameOverWorldObjects.Get() != null)
		{
			GameOverWorldObjects.Get().SetVisible(true);
		}
		UIManager.SetGameObjectActive(m_ContinueBtn, false);
		UIManager.SetGameObjectActive(UIChatBox.Get().m_overconsPanel, false);
		UIChatBox.Get().m_overconsPanel.SetPanelOpen(false);
	}

	private int GetTotalCurrencyGainedFromQuests()
	{
		int num = 0;
		if (m_results.CurrencyRewards != null)
		{
			for (int i = 0; i < m_results.CurrencyRewards.Count; i++)
			{
				num += m_results.CurrencyRewards[i].QuestGained;
			}
		}
		return num;
	}

	private int GetNormalBarXPTotal()
	{
		if (m_results != null)
		{
			return m_results.BaseXpGained + m_results.EventBonusXpGained + m_results.PlayWithFriendXpGained + m_results.QueueTimeXpGained + m_results.WinXpGained + m_results.FreelancerOwnedXPGained;
		}
		return 0;
	}

	private IEnumerable<GameOverExperienceUpdateSubState.UpdatingInfo> HandleUpdateExpSubStateComplete(GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo)
	{
		UIScreenManager.Get().EndAllLoopSounds();
		ContinueBtnFailSafeTime = Time.unscaledTime;
		bool flag = false;
		if (UpdateInfo.UpdateType == GameOverExperienceUpdateSubState.UpdatingType.InitialPause)
		{
			UIManager.SetGameObjectActive(m_ContinueBtn, true);
			if (GetNormalBarXPTotal() <= 0)
			{
				if (!HasCurrencyToDisplay())
				{
					flag = true;
					goto IL_0078;
				}
			}
			return new GameOverExperienceUpdateSubState.UpdatingInfo[1]
			{
				new GameOverExperienceUpdateSubState.UpdatingInfo(GameOverExperienceUpdateSubState.UpdatingType.NormalExpBar, 1.5f)
			};
		}
		goto IL_0078;
		IL_0216:
		if (!flag)
		{
			if (UpdateInfo.UpdateType != GameOverExperienceUpdateSubState.UpdatingType.QuestExp)
			{
				goto IL_0301;
			}
		}
		if (IsRankedGame)
		{
			if (ClientGameManager.Get().TierCurrent != null)
			{
				if (ClientGameManager.Get().TierCurrent.Tier != -1)
				{
					TierPlacement tierPlacement;
					if (SelfWon)
					{
						tierPlacement = ClientGameManager.Get().TierChangeMax;
					}
					else
					{
						tierPlacement = ClientGameManager.Get().TierChangeMin;
					}
					if (ClientGameManager.Get().TierCurrent.Tier == tierPlacement.Tier)
					{
						if (ClientGameManager.Get().TierCurrent.Points == tierPlacement.Points)
						{
							goto IL_0301;
						}
					}
					return new GameOverExperienceUpdateSubState.UpdatingInfo[1]
					{
						new GameOverExperienceUpdateSubState.UpdatingInfo(GameOverExperienceUpdateSubState.UpdatingType.RankPoints, 3f)
					};
				}
			}
		}
		goto IL_0301;
		IL_010c:
		if (!flag)
		{
			if (UpdateInfo.UpdateType != GameOverExperienceUpdateSubState.UpdatingType.GGExpAnim)
			{
				goto IL_015d;
			}
		}
		if (m_results.GGXpGained > 0)
		{
			while (true)
			{
				return new GameOverExperienceUpdateSubState.UpdatingInfo[1]
				{
					new GameOverExperienceUpdateSubState.UpdatingInfo(GameOverExperienceUpdateSubState.UpdatingType.GGExpBar, 1.5f)
				};
			}
		}
		flag = true;
		goto IL_015d;
		IL_01b2:
		if (!flag)
		{
			if (UpdateInfo.UpdateType != GameOverExperienceUpdateSubState.UpdatingType.QuestBarPause)
			{
				goto IL_0216;
			}
		}
		if (m_results.QuestXpGained <= 0)
		{
			if (GetTotalCurrencyGainedFromQuests() <= 0)
			{
				flag = true;
				goto IL_0216;
			}
		}
		return new GameOverExperienceUpdateSubState.UpdatingInfo[1]
		{
			new GameOverExperienceUpdateSubState.UpdatingInfo(GameOverExperienceUpdateSubState.UpdatingType.QuestExp, 1.5f)
		};
		IL_0078:
		if (!flag)
		{
			if (UpdateInfo.UpdateType != GameOverExperienceUpdateSubState.UpdatingType.NormalExpBar)
			{
				goto IL_010c;
			}
		}
		if (m_results.GGXpGained > 0)
		{
			while (true)
			{
				m_ggBonusXPAnimText.text = string.Format(StringUtil.TR("GGBonusXP", "GameOver"), m_results.GGXpGained);
				m_ggBonusController.Play("ResultsBonusIconGGDefaultIN");
				return new GameOverExperienceUpdateSubState.UpdatingInfo[1]
				{
					new GameOverExperienceUpdateSubState.UpdatingInfo(GameOverExperienceUpdateSubState.UpdatingType.GGExpAnim, 0f)
				};
			}
		}
		flag = true;
		goto IL_010c;
		IL_015d:
		if (UpdateInfo.UpdateType == GameOverExperienceUpdateSubState.UpdatingType.GGExpBar)
		{
			if (m_results.QuestXpGained <= 0)
			{
				if (GetTotalCurrencyGainedFromQuests() <= 0)
				{
					flag = true;
					goto IL_01b2;
				}
			}
			return new GameOverExperienceUpdateSubState.UpdatingInfo[1]
			{
				new GameOverExperienceUpdateSubState.UpdatingInfo(GameOverExperienceUpdateSubState.UpdatingType.QuestBarPause, 0f)
			};
		}
		goto IL_01b2;
		IL_0301:
		return null;
	}

	private void HandleUpdateExpSubStateUpdate(GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo)
	{
		UpdateRankPoints(UpdateInfo);
		UpdateInfluenceObjects(UpdateInfo);
		UpdateCurrencyObjects(UpdateInfo);
		UpdateExperienceObjects(UpdateInfo);
	}

	private void UpdateCurrencyHelper(GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo, CurrencyDisplayInfo DisplayInfo, CurrencyType CurrencyType)
	{
		int num = 0;
		int num2 = 0;
		if (UpdateInfo.UpdateType == GameOverExperienceUpdateSubState.UpdatingType.NormalExpBar)
		{
			num2 = DisplayInfo.GetTotalNormalCurrencyReward(CurrencyType);
		}
		else if (UpdateInfo.UpdateType == GameOverExperienceUpdateSubState.UpdatingType.GGExpBar)
		{
			num = DisplayInfo.GetTotalNormalCurrencyReward(CurrencyType);
			num2 = DisplayInfo.GetTotalGGBoostCurrencyReward(CurrencyType);
		}
		else if (UpdateInfo.UpdateType == GameOverExperienceUpdateSubState.UpdatingType.QuestExp)
		{
			num = DisplayInfo.GetTotalNormalCurrencyReward(CurrencyType) + DisplayInfo.GetTotalGGBoostCurrencyReward(CurrencyType);
			num2 = DisplayInfo.GetTotalQuestCurrencyReward(CurrencyType);
		}
		int num3 = num + (int)((float)num2 * UpdateInfo.PercentageProgress);
		DisplayInfo.m_currencyGainText.text = "+" + num3;
	}

	private void UpdateExpSubStateHelper(GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo, XPDisplayInfo DisplayInfo, int TotalXPToGain, int XPGainFromPreviousState, int StartLevel, int XPGainedSoFar)
	{
		SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason);
		if (seasonTemplate.IsTutorial)
		{
			if (DisplayInfo.XPBarType != XPDisplayInfo.BarXPType.Character)
			{
				return;
			}
		}
		int num = XPGainFromPreviousState + XPGainedSoFar;
		int num2 = StartLevel;
		int xPForType = XPDisplayInfo.GetXPForType(DisplayInfo.XPBarType, num2);
		while (num >= xPForType)
		{
			num -= xPForType;
			num2++;
			xPForType = XPDisplayInfo.GetXPForType(DisplayInfo.XPBarType, num2);
		}
		while (true)
		{
			if (DisplayInfo.m_LastLevelDisplayed != num2)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
					{
						int xPForType2 = XPDisplayInfo.GetXPForType(DisplayInfo.XPBarType, num2 - 1);
						if (UpdateInfo.UpdateType == GameOverExperienceUpdateSubState.UpdatingType.NormalExpBar)
						{
							DisplayInfo.m_NormalXPGainSlider.fillAmount = 1f;
						}
						else if (UpdateInfo.UpdateType == GameOverExperienceUpdateSubState.UpdatingType.GGExpBar)
						{
							DisplayInfo.m_GGXPSlider.fillAmount = 1f;
						}
						else if (UpdateInfo.UpdateType == GameOverExperienceUpdateSubState.UpdatingType.QuestExp)
						{
							DisplayInfo.m_GGXPSlider.fillAmount = 1f;
						}
						DisplayInfo.m_barLevelLabel.text = (num2 - 1).ToString();
						DisplayInfo.m_XPLabel.text = xPForType2 + " / " + xPForType2;
						DisplayInfo.m_playingLevelUp = true;
						UIAnimationEventManager.Get().PlayAnimation(DisplayInfo.m_barLevelUpAnimator, "resultsAccountLevelUpDefaultIN", DisplayInfo.LevelUpAnimDone, "resultsAccountLevelUpDefaultIDLE");
						UpdateInfo.SetPaused(true);
						DisplayInfo.m_LastLevelDisplayed = num2;
						DisplayInfo.CheckForRewardDisplay(num2);
						return;
					}
					}
				}
			}
			if (UpdateInfo.UpdateType == GameOverExperienceUpdateSubState.UpdatingType.NormalExpBar)
			{
				DisplayInfo.m_NormalXPGainSlider.fillAmount = (float)num / (float)xPForType;
			}
			else if (UpdateInfo.UpdateType == GameOverExperienceUpdateSubState.UpdatingType.GGExpBar)
			{
				DisplayInfo.m_GGXPSlider.fillAmount = (float)num / (float)xPForType;
			}
			else if (UpdateInfo.UpdateType == GameOverExperienceUpdateSubState.UpdatingType.QuestExp)
			{
				DisplayInfo.m_QuestXPSlider.fillAmount = (float)num / (float)xPForType;
			}
			DisplayInfo.m_barLevelLabel.text = num2.ToString();
			DisplayInfo.m_XPLabel.text = num + " / " + xPForType;
			return;
		}
	}

	private void UpdateInfluenceObjects(GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo)
	{
		if (UpdateInfo.UpdateType != GameOverExperienceUpdateSubState.UpdatingType.NormalExpBar)
		{
			return;
		}
		while (true)
		{
			if (m_results.FactionContributionAmounts == null)
			{
				return;
			}
			while (true)
			{
				if (!UpdateInfo.IsPaused)
				{
					while (true)
					{
						int num = 0;
						using (Dictionary<string, int>.Enumerator enumerator = m_results.FactionContributionAmounts.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								num += enumerator.Current.Value;
							}
						}
						int num2 = (int)((float)num * UpdateInfo.PercentageProgress);
						m_influenceDisplay.m_currencyGainText.text = "+" + num2;
						return;
					}
				}
				return;
			}
		}
	}

	private int GetDisplayTotalCurrency(CurrencyDisplayInfo info, CurrencyType CurrencyType)
	{
		if (info != null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return info.GetTotalNormalCurrencyReward(CurrencyType) + info.GetTotalGGBoostCurrencyReward(CurrencyType) + info.GetTotalQuestCurrencyReward(CurrencyType);
				}
			}
		}
		return 0;
	}

	private bool HasCurrencyToDisplay()
	{
		return GetDisplayTotalCurrency(m_isoDisplay, CurrencyType.ISO) + GetDisplayTotalCurrency(m_freelancerCurrencyDisplay, CurrencyType.FreelancerCurrency) + GetDisplayTotalCurrency(m_rankedCurrencyDisplay, CurrencyType.RankedCurrency) > 0;
	}

	private void UpdateCurrencyObjects(GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo)
	{
		if (UpdateInfo.UpdateType != GameOverExperienceUpdateSubState.UpdatingType.NormalExpBar)
		{
			if (UpdateInfo.UpdateType != GameOverExperienceUpdateSubState.UpdatingType.GGExpBar)
			{
				if (UpdateInfo.UpdateType != GameOverExperienceUpdateSubState.UpdatingType.QuestExp)
				{
					return;
				}
			}
		}
		if (!UpdateInfo.IsPaused)
		{
			UpdateCurrencyHelper(UpdateInfo, m_isoDisplay, CurrencyType.ISO);
			UpdateCurrencyHelper(UpdateInfo, m_freelancerCurrencyDisplay, CurrencyType.FreelancerCurrency);
			UpdateCurrencyHelper(UpdateInfo, m_rankedCurrencyDisplay, CurrencyType.RankedCurrency);
		}
	}

	private void UpdateExperienceObjects(GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo)
	{
		if (UpdateInfo.UpdateType == GameOverExperienceUpdateSubState.UpdatingType.NormalExpBar)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (!UpdateInfo.IsPaused)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
							{
								UIScreenManager.Get().PlayNormalXPLoop(true);
								int normalBarXPTotal = GetNormalBarXPTotal();
								int num = normalBarXPTotal + m_results.ConsumableXpGained;
								int xPGainedSoFar = Mathf.RoundToInt(UpdateInfo.PercentageProgress * (float)normalBarXPTotal);
								int num2 = Mathf.RoundToInt(UpdateInfo.PercentageProgress * (float)num);
								m_expGain.text = "+" + xPGainedSoFar;
								UpdateExpSubStateHelper(UpdateInfo, m_playerXPInfo, normalBarXPTotal, m_results.SeasonXpAtStart, m_results.SeasonLevelAtStart, xPGainedSoFar);
								if (m_results.NumCharactersPlayed > 1)
								{
									num /= m_results.NumCharactersPlayed;
									num2 /= m_results.NumCharactersPlayed;
								}
								UpdateExpSubStateHelper(UpdateInfo, m_characterXPInfo, num, m_results.CharacterXpAtStart, m_results.CharacterLevelAtStart, num2);
								return;
							}
							}
						}
					}
					UIScreenManager.Get().EndNormalXPLoop();
					if (!m_playerXPInfo.m_playingLevelUp)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								if (!m_characterXPInfo.m_playingLevelUp && !UINewReward.Get().RewardIsBeingAnnounced())
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											break;
										default:
											UpdateInfo.SetPaused(false);
											return;
										}
									}
								}
								return;
							}
						}
					}
					return;
				}
			}
		}
		if (UpdateInfo.UpdateType == GameOverExperienceUpdateSubState.UpdatingType.GGExpBar)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (!UpdateInfo.IsPaused)
					{
						UIScreenManager.Get().PlayGGBoostXPLoop(true);
						int gGXpGained = m_results.GGXpGained;
						int num3 = gGXpGained;
						int normalBarXPTotal2 = GetNormalBarXPTotal();
						int num4 = normalBarXPTotal2 + m_results.ConsumableXpGained;
						int num5 = Mathf.RoundToInt(UpdateInfo.PercentageProgress * (float)gGXpGained);
						m_expGain.text = "+" + (normalBarXPTotal2 + num5);
						UpdateExpSubStateHelper(UpdateInfo, m_playerXPInfo, gGXpGained, m_results.SeasonXpAtStart + normalBarXPTotal2, m_results.SeasonLevelAtStart, num5);
						if (m_results.NumCharactersPlayed > 1)
						{
							num3 += num4;
							num3 /= m_results.NumCharactersPlayed;
							num4 /= m_results.NumCharactersPlayed;
							num3 -= num4;
						}
						int xPGainedSoFar2 = Mathf.RoundToInt(UpdateInfo.PercentageProgress * (float)num3);
						UpdateExpSubStateHelper(UpdateInfo, m_characterXPInfo, num3, m_results.CharacterXpAtStart + num4, m_results.CharacterLevelAtStart, xPGainedSoFar2);
					}
					else
					{
						UIScreenManager.Get().EndGGXPLoop();
						if (!m_playerXPInfo.m_playingLevelUp && !m_characterXPInfo.m_playingLevelUp)
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
									if (!UINewReward.Get().RewardIsBeingAnnounced())
									{
										while (true)
										{
											switch (3)
											{
											case 0:
												break;
											default:
												UpdateInfo.SetPaused(false);
												return;
											}
										}
									}
									return;
								}
							}
						}
					}
					return;
				}
			}
		}
		if (UpdateInfo.UpdateType != GameOverExperienceUpdateSubState.UpdatingType.QuestExp)
		{
			return;
		}
		if (!UpdateInfo.IsPaused)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					UIScreenManager.Get().PlayGGBoostXPLoop(true);
					int questXpGained = m_results.QuestXpGained;
					int num6 = GetNormalBarXPTotal() + m_results.GGXpGained;
					int num7 = Mathf.RoundToInt(UpdateInfo.PercentageProgress * (float)questXpGained);
					m_expGain.text = "+" + (num6 + num7);
					UpdateExpSubStateHelper(UpdateInfo, m_playerXPInfo, questXpGained, m_results.SeasonXpAtStart + num6, m_results.SeasonLevelAtStart, num7);
					return;
				}
				}
			}
		}
		UIScreenManager.Get().EndGGXPLoop();
		if (m_playerXPInfo.m_playingLevelUp)
		{
			return;
		}
		while (true)
		{
			if (m_characterXPInfo.m_playingLevelUp)
			{
				return;
			}
			while (true)
			{
				if (!UINewReward.Get().RewardIsBeingAnnounced())
				{
					while (true)
					{
						UpdateInfo.SetPaused(false);
						return;
					}
				}
				return;
			}
		}
	}

	private bool IsTierContenderOrMaster(int tier)
	{
		int result;
		if (tier != 0)
		{
			result = ((tier == 1) ? 1 : 0);
		}
		else
		{
			result = 1;
		}
		return (byte)result != 0;
	}

	private void RankModeAnimDone()
	{
		RankLevelUpDownAnimating = false;
		if (SelfWon)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_rankNormalBar.fillAmount = 0f;
					m_rankDecreaseBar.fillAmount = 0f;
					m_rankIncreaseBar.fillAmount = 0f;
					return;
				}
			}
		}
		m_rankNormalBar.fillAmount = 1f;
		m_rankDecreaseBar.fillAmount = 1f;
		m_rankIncreaseBar.fillAmount = 1f;
	}

	private void UpdateRankPointsWin(GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo)
	{
		if (!UpdateInfo.IsPaused && ClientGameManager.Get().TierCurrent != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					TierPlacement tierCurrent = ClientGameManager.Get().TierCurrent;
					TierPlacement tierChangeMax = ClientGameManager.Get().TierChangeMax;
					float num = 0f;
					if (tierChangeMax.Tier == tierCurrent.Tier)
					{
						num = tierChangeMax.Points - tierCurrent.Points;
					}
					else
					{
						int num2 = tierCurrent.Tier;
						while (num2 != tierChangeMax.Tier)
						{
							if (num2 == tierCurrent.Tier)
							{
								if (IsTierContenderOrMaster(num2))
								{
									while (true)
									{
										switch (3)
										{
										case 0:
											break;
										default:
											throw new Exception("Increasing tiers from master or contender when winning doesn't make sense.");
										}
									}
								}
								num += Mathf.Abs(100f - tierCurrent.Points);
							}
							else
							{
								num += 100f;
							}
							if (tierCurrent.Tier < tierChangeMax.Tier)
							{
								num2++;
							}
							else
							{
								num2--;
							}
						}
						num += tierChangeMax.Points;
					}
					float num3 = UpdateInfo.PercentageProgress * num;
					float num4 = ClientGameManager.Get().TierCurrent.Points + num3;
					int num5 = ClientGameManager.Get().TierCurrent.Tier;
					float num6;
					if (IsTierContenderOrMaster(num5))
					{
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
							num5++;
						}
						else
						{
							num5--;
						}
						float num8;
						if (IsTierContenderOrMaster(num5))
						{
							num8 = float.MaxValue;
						}
						else
						{
							num8 = 100f;
						}
						num7 = num8;
					}
					if (SetupTierDisplay(num5, num4))
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								RankLevelUpDownAnimating = true;
								UpdateInfo.SetPaused(true);
								m_rankUpDownText.text = StringUtil.TR("RankUp", "GameOver");
								UIAnimationEventManager.Get().PlayAnimation(m_rankModeLevelAnimator, "RankedLvlUp", RankModeAnimDone, string.Empty);
								return;
							}
						}
					}
					m_rankPointsText.text = "+" + num3.ToString("F1");
					if (ShouldDisplayTierPoints)
					{
						m_rankLevelText.text = Mathf.RoundToInt(num4).ToString();
					}
					if (IsTierContenderOrMaster(num5))
					{
						m_rankIncreaseBar.fillAmount = 1f;
					}
					else
					{
						m_rankIncreaseBar.fillAmount = GetRankFillAmt(num4 / 100f);
					}
					return;
				}
				}
			}
		}
		if (RankLevelUpDownAnimating)
		{
			return;
		}
		while (true)
		{
			UpdateInfo.SetPaused(false);
			return;
		}
	}

	private void UpdateRankPointsLose(GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo)
	{
		TierPlacement tierCurrent = ClientGameManager.Get().TierCurrent;
		TierPlacement tierChangeMin = ClientGameManager.Get().TierChangeMin;
		if (!UpdateInfo.IsPaused)
		{
			if (tierCurrent != null)
			{
				if (tierChangeMin != null)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
						{
							float num = 0f;
							if (tierChangeMin.Tier == tierCurrent.Tier)
							{
								num = tierCurrent.Points - tierChangeMin.Points;
							}
							else
							{
								for (int num2 = tierCurrent.Tier; num2 != tierChangeMin.Tier; num2 = ((tierCurrent.Tier >= tierChangeMin.Tier) ? (num2 - 1) : (num2 + 1)))
								{
									if (num2 == tierCurrent.Tier)
									{
										num += tierCurrent.Points;
									}
									else
									{
										if (IsTierContenderOrMaster(num2))
										{
											while (true)
											{
												switch (6)
												{
												case 0:
													break;
												default:
													throw new Exception("Decreasing tier to master or contender when losing doesn't make sense.");
												}
											}
										}
										num += 100f;
									}
								}
								num += Mathf.Abs(100f - tierChangeMin.Points);
							}
							float num3 = UpdateInfo.PercentageProgress * num;
							float num4 = ClientGameManager.Get().TierCurrent.Points - num3;
							int num5 = ClientGameManager.Get().TierCurrent.Tier;
							while (true)
							{
								if (!(num4 <= 0f))
								{
									break;
								}
								if (tierCurrent.Tier < tierChangeMin.Tier)
								{
									if (UIRankedModeSelectScreen.IsRatchetTier(tierCurrent.Tier))
									{
										num4 = 0f;
										break;
									}
									num5++;
								}
								else
								{
									if (tierCurrent.Tier <= tierChangeMin.Tier)
									{
										num4 = 0f;
										break;
									}
									num5--;
								}
								if (IsTierContenderOrMaster(num5))
								{
									Log.Error("How did this happen? Should have been caught in an exception earlier");
								}
								num4 += 100f;
							}
							if (SetupTierDisplay(num5, num4))
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
										RankLevelUpDownAnimating = true;
										UpdateInfo.SetPaused(true);
										m_rankUpDownText.text = StringUtil.TR("RankDown", "GameOver");
										UIAnimationEventManager.Get().PlayAnimation(m_rankModeLevelAnimator, "RankedLvlDown", RankModeAnimDone, string.Empty);
										return;
									}
								}
							}
							m_rankPointsText.text = "-" + num3.ToString("F1");
							if (ShouldDisplayTierPoints)
							{
								m_rankLevelText.text = Mathf.RoundToInt(num4).ToString();
							}
							if (IsTierContenderOrMaster(num5))
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
										m_rankNormalBar.fillAmount = 1f;
										return;
									}
								}
							}
							m_rankNormalBar.fillAmount = GetRankFillAmt(num4 / 100f);
							return;
						}
						}
					}
				}
			}
		}
		if (RankLevelUpDownAnimating)
		{
			return;
		}
		while (true)
		{
			UpdateInfo.SetPaused(false);
			return;
		}
	}

	private void UpdateRankPoints(GameOverExperienceUpdateSubState.UpdatingInfo UpdateInfo)
	{
		if (UpdateInfo.UpdateType != GameOverExperienceUpdateSubState.UpdatingType.RankPoints)
		{
			return;
		}
		while (true)
		{
			if (ClientGameManager.Get().TierCurrent == null || ClientGameManager.Get().TierCurrent.Tier <= -1)
			{
				return;
			}
			if (SelfWon)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						UpdateRankPointsWin(UpdateInfo);
						return;
					}
				}
			}
			UpdateRankPointsLose(UpdateInfo);
			return;
		}
	}

	public bool DoNextGGBoostRecapDisplay()
	{
		bool flag = false;
		for (int i = 0; i < m_blueTeamGGProfiles.Length; i++)
		{
			if (m_blueTeamGGProfiles[i].GetPlayerInfo() == null)
			{
				continue;
			}
			int num = HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.NumberGGPacksUsed(m_blueTeamGGProfiles[i].GetPlayerInfo().Handle);
			if (m_blueTeamGGProfiles[i].GetPlayerInfo().IsRemoteControlled)
			{
				continue;
			}
			if (num <= 0)
			{
				continue;
			}
			if (m_blueTeamGGProfiles[i].CurrentGGPackLevel != num)
			{
				if (flag)
				{
					return true;
				}
				flag = true;
				m_blueTeamGGProfiles[i].SetGGButtonLevel(num);
			}
		}
		for (int j = 0; j < m_redTeamGGProfiles.Length; j++)
		{
			if (m_redTeamGGProfiles[j].GetPlayerInfo() == null)
			{
				continue;
			}
			int num2 = HUD_UI.Get().m_mainScreenPanel.m_sideNotificationsPanel.NumberGGPacksUsed(m_redTeamGGProfiles[j].GetPlayerInfo().Handle);
			if (m_redTeamGGProfiles[j].GetPlayerInfo().IsRemoteControlled || num2 <= 0)
			{
				continue;
			}
			if (m_redTeamGGProfiles[j].CurrentGGPackLevel != num2)
			{
				if (flag)
				{
					return true;
				}
				flag = true;
				m_redTeamGGProfiles[j].SetGGButtonLevel(num2);
			}
		}
		return false;
	}

	private void HandleGGBoostSubStateUpdate(GameOverGGSubState.GGBoosts CurrentSubState, float percentToDisplay)
	{
		if (CurrentSubState == GameOverGGSubState.GGBoosts.UsageTimer)
		{
			m_GGBoostTimer.fillAmount = percentToDisplay;
		}
	}

	private void HandleNewGGBoostSubState(GameOverGGSubState SubState, GameOverGGSubState.GGBoosts NewSubState)
	{
		switch (NewSubState)
		{
		default:
			return;
		case GameOverGGSubState.GGBoosts.UsageTimer:
		{
			GGBoostContainerAnimator.Play("GGBonusScreenDefaultIDLE");
			int currentAmount = ClientGameManager.Get().PlayerWallet.GetCurrentAmount(CurrencyType.GGPack);
			m_worldGGBtnHitBox.SetClickable(currentAmount > 0);
			return;
		}
		case GameOverGGSubState.GGBoosts.FadeOut:
			break;
		}
		while (true)
		{
			m_worldGGBtnHitBox.SetClickable(false);
			UIAnimationEventManager.Get().PlayAnimation(GGBoostContainerAnimator, "GGBonusScreenDefaultOUT", SubState.FadeoutAnimDone, string.Empty);
			return;
		}
	}

	public void UpdateGameOverSubStateObjects(GameOverSubState StateToBeUpdated)
	{
		if (StateToBeUpdated.SubStateType == GameOverScreenState.Stats)
		{
			int num = Mathf.FloorToInt((float)m_GameOverStatWidgets.Count * StateToBeUpdated.PercentageProgress);
			for (int i = 0; i < num; i++)
			{
				m_GameOverStatWidgets[i].SetHighlight();
			}
		}
		if (StateToBeUpdated.SubStateType == GameOverScreenState.Accolades)
		{
			int num2 = 0;
			if (m_results.BadgeAndParticipantsInfo != null)
			{
				int num3 = 0;
				if (!m_results.BadgeAndParticipantsInfo.IsNullOrEmpty())
				{
					using (List<BadgeAndParticipantInfo>.Enumerator enumerator = m_results.BadgeAndParticipantsInfo.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							BadgeAndParticipantInfo current = enumerator.Current;
							if (!current.TopParticipationEarned.IsNullOrEmpty())
							{
								num3 += current.TopParticipationEarned.Count;
							}
						}
					}
				}
				num2 = Mathf.FloorToInt((float)num3 * Mathf.Clamp01((StateToBeUpdated.PercentageProgress - TopPartipantFrontTimePadPercentage) / (1f - (TopPartipantFrontTimePadPercentage + TopPartipantEndTimePadPercentage))));
			}
			for (int j = 0; j < m_TopParticipantWidgets.Length; j++)
			{
				if (j < num2)
				{
					if (!m_TopParticipantWidgets[j].gameObject.activeSelf)
					{
						UIFrontEnd.PlaySound(FrontEndButtonSounds.EndGameBadgeBasic);
					}
					UIManager.SetGameObjectActive(m_TopParticipantWidgets[j], true);
				}
				else
				{
					UIManager.SetGameObjectActive(m_TopParticipantWidgets[j], false);
				}
			}
		}
		if (StateToBeUpdated.SubStateType == GameOverScreenState.PersonalHighlights)
		{
			int num4 = Mathf.FloorToInt((float)m_PersonalHighlightWidgets.Length * Mathf.Clamp01((StateToBeUpdated.PercentageProgress - PersonalHighlightsFrontTimePadPercentage) / (1f - (PersonalHighlightsFrontTimePadPercentage + PersonalHighlightsEndTimePadPercentage))));
			for (int k = 0; k < m_PersonalHighlightWidgets.Length; k++)
			{
				UIManager.SetGameObjectActive(m_PersonalHighlightWidgets[k], true);
				if (k >= num4)
				{
					continue;
				}
				if (!m_PersonalHighlightWidgets[k].HighlightDone)
				{
					UIFrontEnd.PlaySound(FrontEndButtonSounds.EndGameBadgeAchievement);
				}
				m_PersonalHighlightWidgets[k].SetHighlight();
			}
		}
		if (StateToBeUpdated.SubStateType == GameOverScreenState.MissionNotifications)
		{
			int num5 = Mathf.FloorToInt((float)QuestCompletePanel.Get().TotalQuestsToDisplayForGameOver() * StateToBeUpdated.PercentageProgress);
			for (int l = 0; l < num5; l++)
			{
				QuestCompletePanel.Get().DisplayGameOverQuestComplete(l);
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

	public static Sprite GetRewardSprite(RewardUtils.RewardData reward)
	{
		return (Sprite)Resources.Load(reward.SpritePath, typeof(Sprite));
	}

	public void NotifySeasonTutorialScreenClosed()
	{
		if (m_currentSubState is GameOverTutorialGames)
		{
			(m_currentSubState as GameOverTutorialGames).NotifyCloseClicked();
		}
	}

	private void DoGGBoostSubstate()
	{
		SetupGGBoostScreen();
		HandleBankBalanceChange(null);
		m_currentSubState = new GameOverGGSubState(EstimatedNotificationArrivalTime);
	}

	private void DoTutorialSeasonSubstate()
	{
		UITutorialSeasonInterstitial.Get().SetVisible(true);
		m_currentSubState = new GameOverTutorialGames();
	}

	private void DoAccoladesDisplay()
	{
		SetupStatRecapScreen();
		SetupStatsScreen();
		SetupTopParticipants();
		SetupBadges();
		m_currentSubState = new GameOverSubState(GameOverScreenState.ResultsScreenPause, 0.01f);
	}

	private void DoExperienceBarSubstate()
	{
		SetupStatRecapScreen();
		List<GameOverExperienceUpdateSubState.UpdatingInfo> list = new List<GameOverExperienceUpdateSubState.UpdatingInfo>();
		list.Add(new GameOverExperienceUpdateSubState.UpdatingInfo(GameOverExperienceUpdateSubState.UpdatingType.InitialPause, 0.01f));
		m_currentSubState = new GameOverExperienceUpdateSubState(list);
	}

	private void RefreshHeaderButtonClickability()
	{
		if (m_currentSubState != null && m_currentSubState.SubStateType == GameOverScreenState.Done)
		{
			while (true)
			{
				int num;
				bool flag;
				int num2;
				int num3;
				bool flag2;
				bool flag3;
				switch (5)
				{
				case 0:
					break;
				default:
					{
						GameManager gameManager = GameManager.Get();
						if (gameManager != null)
						{
							if (gameManager.PlayerInfo != null)
							{
								num = (gameManager.PlayerInfo.IsSpectator ? 1 : 0);
								goto IL_0075;
							}
						}
						num = 0;
						goto IL_0075;
					}
					IL_0075:
					flag = ((byte)num != 0);
					if (ReplayPlayManager.Get() != null)
					{
						num2 = (ReplayPlayManager.Get().IsPlayback() ? 1 : 0);
					}
					else
					{
						num2 = 0;
					}
					if (num2 == 0)
					{
						num3 = ((!flag) ? 1 : 0);
					}
					else
					{
						num3 = 1;
					}
					flag2 = ((byte)num3 != 0);
					flag3 = (NumRewardsEarned > 0);
					if (BadgesAreActive)
					{
						m_AccoladesHeaderBtn.spriteController.SetClickable(true);
						UIManager.SetGameObjectActive(m_AccoladesHeaderBtn.spriteController.m_defaultImage, true);
					}
					else
					{
						m_AccoladesHeaderBtn.spriteController.SetClickable(false);
						UIManager.SetGameObjectActive(m_AccoladesHeaderBtn.spriteController.m_defaultImage, false);
						UIManager.SetGameObjectActive(m_AccoladesHeaderBtn.spriteController.m_hoverImage, false);
						UIManager.SetGameObjectActive(m_AccoladesHeaderBtn.spriteController.m_pressedImage, false);
					}
					m_StatsHeaderBtn.spriteController.SetClickable(flag2);
					UIManager.SetGameObjectActive(m_StatsHeaderBtn.spriteController.m_defaultImage, flag2);
					UIManager.SetGameObjectActive(m_StatsHeaderBtn.spriteController.m_hoverImage, flag2);
					UIManager.SetGameObjectActive(m_StatsHeaderBtn.spriteController.m_pressedImage, flag2);
					m_RewardsHeaderBtn.spriteController.SetClickable(flag3);
					UIManager.SetGameObjectActive(m_RewardsHeaderBtn.spriteController.m_defaultImage, flag3);
					UIManager.SetGameObjectActive(m_RewardsHeaderBtn.spriteController.m_hoverImage, flag3);
					UIManager.SetGameObjectActive(m_RewardsHeaderBtn.spriteController.m_pressedImage, flag3);
					m_ScoreHeaderBtn.spriteController.SetClickable(true);
					UIManager.SetGameObjectActive(m_ScoreHeaderBtn.spriteController.m_defaultImage, true);
					UIManager.SetGameObjectActive(m_ScoreHeaderBtn.spriteController.m_hoverImage, true);
					UIManager.SetGameObjectActive(m_ScoreHeaderBtn.spriteController.m_pressedImage, true);
					return;
				}
			}
		}
		m_AccoladesHeaderBtn.spriteController.SetClickable(false);
		UIManager.SetGameObjectActive(m_AccoladesHeaderBtn.spriteController.m_defaultImage, false);
		UIManager.SetGameObjectActive(m_AccoladesHeaderBtn.spriteController.m_hoverImage, false);
		UIManager.SetGameObjectActive(m_AccoladesHeaderBtn.spriteController.m_pressedImage, false);
		m_StatsHeaderBtn.spriteController.SetClickable(false);
		UIManager.SetGameObjectActive(m_StatsHeaderBtn.spriteController.m_defaultImage, false);
		UIManager.SetGameObjectActive(m_StatsHeaderBtn.spriteController.m_hoverImage, false);
		UIManager.SetGameObjectActive(m_StatsHeaderBtn.spriteController.m_pressedImage, false);
		m_RewardsHeaderBtn.spriteController.SetClickable(false);
		UIManager.SetGameObjectActive(m_RewardsHeaderBtn.spriteController.m_defaultImage, false);
		UIManager.SetGameObjectActive(m_RewardsHeaderBtn.spriteController.m_hoverImage, false);
		UIManager.SetGameObjectActive(m_RewardsHeaderBtn.spriteController.m_pressedImage, false);
		m_ScoreHeaderBtn.spriteController.SetClickable(false);
		UIManager.SetGameObjectActive(m_ScoreHeaderBtn.spriteController.m_defaultImage, false);
		UIManager.SetGameObjectActive(m_ScoreHeaderBtn.spriteController.m_hoverImage, false);
		UIManager.SetGameObjectActive(m_ScoreHeaderBtn.spriteController.m_pressedImage, false);
	}

	private void DoStatsSubstate()
	{
		SetupStatRecapScreen();
		SetupStatsScreen();
		SetupBadges();
		m_currentSubState = new GameOverSubState(GameOverScreenState.Stats, StatsDisplayTimeDuration);
		UITutorialPanel.Get().HideTutorialPassedStamp();
	}

	private void CheckIfCurrentStateIsDone()
	{
		if (!m_currentSubState.IsDone())
		{
			return;
		}
		while (true)
		{
			bool visible = false;
			bool doActive = false;
			bool flag = false;
			bool flag2 = false;
			bool doActive2 = false;
			bool flag3 = false;
			bool flag4 = false;
			bool flag5 = false;
			GameManager gameManager = GameManager.Get();
			int num;
			if (gameManager != null)
			{
				if (gameManager.PlayerInfo != null)
				{
					num = (gameManager.PlayerInfo.IsSpectator ? 1 : 0);
					goto IL_0076;
				}
			}
			num = 0;
			goto IL_0076;
			IL_0076:
			bool flag6 = (byte)num != 0;
			bool flag7 = ReplayPlayManager.Get() != null && ReplayPlayManager.Get().IsPlayback();
			if (m_currentSubState.SubStateType == GameOverScreenState.VictoryDefeat)
			{
				if (GameManager.IsGameTypeValidForGGPack(m_gameType))
				{
					doActive = true;
					DoGGBoostSubstate();
				}
				else
				{
					m_currentSubState = new GameOverVictoryDefeatWaitingSubState();
				}
			}
			else if (m_currentSubState.SubStateType == GameOverScreenState.VictoryDefeatWaitingForNotification)
			{
				doActive2 = true;
				SeasonTemplate seasonTemplate = SeasonWideData.Get().GetSeasonTemplate(ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason);
				if (seasonTemplate.IsTutorial)
				{
					DoTutorialSeasonSubstate();
				}
				else if (BadgesAreActive)
				{
					flag3 = true;
					DoAccoladesDisplay();
				}
				else
				{
					flag4 = (flag7 || !flag6);
					flag = true;
					flag2 = true;
					DoStatsSubstate();
				}
			}
			else if (m_currentSubState.SubStateType == GameOverScreenState.GGBoostUsage)
			{
				doActive2 = true;
				SeasonTemplate seasonTemplate2 = SeasonWideData.Get().GetSeasonTemplate(ClientGameManager.Get().GetPlayerAccountData().QuestComponent.ActiveSeason);
				if (seasonTemplate2.IsTutorial)
				{
					DoTutorialSeasonSubstate();
				}
				else if (BadgesAreActive)
				{
					flag3 = true;
					DoAccoladesDisplay();
				}
				else
				{
					flag4 = (flag7 || !flag6);
					flag = true;
					flag2 = true;
					DoStatsSubstate();
				}
			}
			else if (m_currentSubState.SubStateType == GameOverScreenState.TutorialTenGames)
			{
				doActive2 = true;
				if (BadgesAreActive)
				{
					flag3 = true;
					DoAccoladesDisplay();
				}
				else
				{
					int num2;
					if (!flag7)
					{
						num2 = ((!flag6) ? 1 : 0);
					}
					else
					{
						num2 = 1;
					}
					flag4 = ((byte)num2 != 0);
					flag = true;
					flag2 = true;
					DoStatsSubstate();
				}
			}
			else if (m_currentSubState.SubStateType == GameOverScreenState.ResultsScreenPause)
			{
				doActive2 = true;
				flag3 = true;
				UIManager.SetGameObjectActive(m_TopParticipantsAnimator, true);
				m_AccoladesHeaderBtn.SetSelected(true, false, "SelectedIN", "SelectedOUT");
				m_StatsHeaderBtn.SetSelected(false, false, "SelectedIN", "SelectedOUT");
				m_RewardsHeaderBtn.SetSelected(false, false, "SelectedIN", "SelectedOUT");
				m_ScoreHeaderBtn.SetSelected(false, false, "SelectedIN", "SelectedOUT");
				m_currentSubState = new GameOverSubState(GameOverScreenState.Accolades, TopParticipantDisplayTimeDuration);
				UITutorialPanel.Get().HideTutorialPassedStamp();
			}
			else if (m_currentSubState.SubStateType == GameOverScreenState.Accolades)
			{
				doActive2 = true;
				flag3 = true;
				UIManager.SetGameObjectActive(m_PersonalHighlightsAnimator, true);
				m_currentSubState = new GameOverSubState(GameOverScreenState.PersonalHighlights, PersonalHighlightsDisplayTimeDuration);
				m_currentSubState.SetPercentageClickToSkip(PersonalHighlightsClickSkipPercentage);
			}
			else if (m_currentSubState.SubStateType == GameOverScreenState.PersonalHighlights)
			{
				flag4 = (flag7 || !flag6);
				flag = true;
				flag2 = true;
				DoStatsSubstate();
			}
			else if (m_currentSubState.SubStateType == GameOverScreenState.MissionNotifications)
			{
				int num3;
				if (!flag7)
				{
					num3 = ((!flag6) ? 1 : 0);
				}
				else
				{
					num3 = 1;
				}
				flag4 = ((byte)num3 != 0);
				flag = true;
				flag2 = true;
				DoExperienceBarSubstate();
			}
			else if (m_currentSubState.SubStateType == GameOverScreenState.Stats)
			{
				int num4;
				if (!flag7)
				{
					num4 = ((!flag6) ? 1 : 0);
				}
				else
				{
					num4 = 1;
				}
				flag4 = ((byte)num4 != 0);
				flag = true;
				flag2 = true;
				if (QuestCompletePanel.Get().TotalQuestsToDisplayForGameOver() > 0)
				{
					m_currentSubState = new GameOverSubState(GameOverScreenState.MissionNotifications, 1f);
				}
				else
				{
					DoExperienceBarSubstate();
				}
			}
			else if (m_currentSubState.SubStateType == GameOverScreenState.ExperienceBars)
			{
				flag = true;
				flag2 = true;
				int num5;
				if (!flag7)
				{
					num5 = ((!flag6) ? 1 : 0);
				}
				else
				{
					num5 = 1;
				}
				flag4 = ((byte)num5 != 0);
				flag5 = !flag4;
				UIManager.SetGameObjectActive(m_ContinueBtn, true);
				m_currentSubState = new GameOverSubState(GameOverScreenState.Done, 0f);
			}
			else if (m_currentSubState.SubStateType == GameOverScreenState.Rewards)
			{
				flag = true;
				flag2 = true;
			}
			if (GameOverWorldObjects.Get() != null)
			{
				GameOverWorldObjects.Get().SetVisible(visible);
			}
			UIManager.SetGameObjectActive(m_GGBoostContainer, doActive);
			UIManager.SetGameObjectActive(m_MouseClickContainer, doActive2);
			RectTransform topBottomBarsContainer = m_TopBottomBarsContainer;
			int doActive3;
			if (!flag2)
			{
				doActive3 = (flag ? 1 : 0);
			}
			else
			{
				doActive3 = 1;
			}
			UIManager.SetGameObjectActive(topBottomBarsContainer, (byte)doActive3 != 0);
			UIManager.SetGameObjectActive(m_TopBarContainer, flag);
			UIManager.SetGameObjectActive(m_BottomBarContainer, flag2);
			UIManager.SetGameObjectActive(m_AccoladesAnimator, flag3);
			UIManager.SetGameObjectActive(m_StatsContainer, flag4);
			UIGameStatsWindow.Get().SetToggleStatsVisible(flag5, false);
			m_AccoladesHeaderBtn.SetSelected(flag3, false, "SelectedIN", "SelectedOUT");
			m_StatsHeaderBtn.SetSelected(flag4, false, "SelectedIN", "SelectedOUT");
			m_RewardsHeaderBtn.SetSelected(false, false, "SelectedIN", "SelectedOUT");
			m_ScoreHeaderBtn.SetSelected(flag5, false, "SelectedIN", "SelectedOUT");
			ContinueBtnFailSafeTime = Time.unscaledTime;
			RefreshHeaderButtonClickability();
			return;
		}
	}

	private void Update()
	{
		CheckPreGameStats();
		if (m_currentSubState != null)
		{
			if (m_currentSubState.SubStateType != GameOverScreenState.Done)
			{
				m_currentSubState.Update();
				if (Input.GetMouseButtonDown(0))
				{
					m_currentSubState.TryClickToSkip();
				}
				CheckIfCurrentStateIsDone();
				float num = 10f;
				if (m_currentSubState.SubStateType == GameOverScreenState.GGBoostUsage)
				{
					num = GameBalanceVars.Get().GGPackEndGameUsageTimer + 10f;
				}
				if (ContinueBtnFailSafeTime > 0f)
				{
					if (Time.unscaledTime > ContinueBtnFailSafeTime + num)
					{
						ContinueBtnFailSafeTime = -1f;
						UIManager.SetGameObjectActive(m_ContinueBtn, true);
						string text = $"Failsafe triggered on state {m_currentSubState.SubStateType}";
						GameOverExperienceUpdateSubState gameOverExperienceUpdateSubState = m_currentSubState as GameOverExperienceUpdateSubState;
						if (gameOverExperienceUpdateSubState != null)
						{
							text += " - SubstateInfo updating ";
							for (int i = 0; i < gameOverExperienceUpdateSubState.CurrentInfos.Count; i++)
							{
								text += $"{gameOverExperienceUpdateSubState.CurrentInfos[i].UpdateType.ToString()} stuck at {gameOverExperienceUpdateSubState.CurrentInfos[i].PercentageProgress} and is paused: {gameOverExperienceUpdateSubState.CurrentInfos[i].IsPaused} ";
							}
						}
						else
						{
							text = text + " - SubstateInfo updating stuck at: " + m_currentSubState.PercentageProgress;
						}
						Debug.LogError(text);
					}
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.Tab))
		{
			if (m_TopBottomBarsContainer.gameObject.activeInHierarchy)
			{
				UIGameStatsWindow.Get().ToggleStatsWindow();
			}
		}
		if (!m_ContinueBtn.gameObject.activeInHierarchy)
		{
			return;
		}
		while (true)
		{
			if (InputManager.Get().GetAcceptButtonDown())
			{
				OnContinueClicked(null);
			}
			return;
		}
	}

	private PercentileInfo GetStatPercentiles(StatDisplaySettings.StatType statType)
	{
		PercentileInfo value = null;
		if (m_results != null)
		{
			if (!m_results.BadgeAndParticipantsInfo.IsNullOrEmpty())
			{
				int playerId = GameManager.Get().PlayerInfo.PlayerId;
				BadgeAndParticipantInfo badgeAndParticipantInfo = m_results.BadgeAndParticipantsInfo.Find((BadgeAndParticipantInfo p) => p.PlayerId == playerId);
				if (badgeAndParticipantInfo != null)
				{
					if (!badgeAndParticipantInfo.GlobalPercentiles.IsNullOrEmpty())
					{
						badgeAndParticipantInfo.GlobalPercentiles.TryGetValue(statType, out value);
					}
				}
			}
		}
		return value;
	}

	private PercentileInfo GetFreelancerStatPercentiles(int index)
	{
		PercentileInfo value = null;
		if (m_results != null)
		{
			if (!m_results.BadgeAndParticipantsInfo.IsNullOrEmpty())
			{
				int playerId = GameManager.Get().PlayerInfo.PlayerId;
				BadgeAndParticipantInfo badgeAndParticipantInfo = m_results.BadgeAndParticipantsInfo.Find((BadgeAndParticipantInfo p) => p.PlayerId == playerId);
				if (badgeAndParticipantInfo != null)
				{
					if (!badgeAndParticipantInfo.FreelancerSpecificPercentiles.IsNullOrEmpty())
					{
						badgeAndParticipantInfo.FreelancerSpecificPercentiles.TryGetValue(index, out value);
					}
				}
			}
		}
		return value;
	}
}
