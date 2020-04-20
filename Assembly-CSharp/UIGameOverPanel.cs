using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverPanel : UIScene
{
	public RectTransform[] m_seasonLevelContainer;

	public RectTransform[] m_accountLevelContainers;

	public InventoryItemTemplate seasonLevelRewardItem;

	private const float c_continueBtnFailSafeTime = 10f;

	private int factionGain;

	private float GGPack_XPMult;

	private bool m_waitingForXpBonusAnimFinished;

	private bool m_isXpBonusAnimPlaying;

	private int m_currentDisplayInfluenceAmt;

	private float m_continueBtnFailsafeTime;

	private SeasonTemplate m_currentSeason;

	private bool m_IsLevellingSeasons;

	private bool m_notificationHasArrived;

	private bool m_wonRankGame;

	private float m_startRankPoints;

	private float m_endRankPoints;

	private float m_rankStartTime;

	private float m_rankJourneyLength;

	private float m_currentRankPointDisplay;

	private float m_previousLevelTotalPoints;

	private int m_rankLevelDiff;

	private const float MIN_RANK_FILL_AMT = 0.082f;

	private const float MAX_RANK_FILL_AMT = 0.915f;

	private GameResult m_gameResult;

	private const float c_transititionGGPackOutTime = 2f;

	private float m_timerToDisplayGGBoosts = -1f;

	private float m_timerToDisplayGGBoostUsage = -1f;

	private float m_timerToTransitionOutGGBoosts = -1f;

	private float m_nextAvailableGGBoostDisplayTime = -1f;

	private int m_numSelfGGpacksUsed;

	private UIGameOverPanel.GameOverScreenState m_currentState;

	private const float m_worldAnimationTime = 3.5f;

	private const float m_GGPackRecapTime = 3f;

	private const float m_GGPackRecapInterval = 0.2f;

	private float m_timeBannerDisplayed;

	private bool m_isSetup;

	private static UIGameOverPanel s_instance;

	private UIGameOverPanel.UpdateXPStage m_updateXpStage;

	private float m_updateXpStartTime;

	private bool failsafeTriggered;

	public UIGameOverPanel.UpdateXPStage XPStage
	{
		get
		{
			return this.m_updateXpStage;
		}
	}

	public override SceneType GetSceneType()
	{
		return SceneType.GameOver;
	}

	private void Start()
	{
		UIGameOverPanel.s_instance = this;
		this.m_notificationHasArrived = false;
	}

	public static UIGameOverPanel Get()
	{
		return UIGameOverPanel.s_instance;
	}

	public void CancelCallback(UIDialogBox boxReference)
	{
		ClientGameManager.Get().LeaveGame(true, this.m_gameResult);
	}

	public void RatingCallback(UIDialogBox boxReference)
	{
		UIRatingDialogBox uiratingDialogBox = boxReference as UIRatingDialogBox;
		if (GameFlowData.Get() != null)
		{
			if (uiratingDialogBox.GetRating() > -1)
			{
				PlayerFeedbackData playerFeedbackData = new PlayerFeedbackData();
				playerFeedbackData.CharacterType = GameManager.Get().PlayerInfo.CharacterType;
				playerFeedbackData.Rating = uiratingDialogBox.GetRating() + 1;
				ClientGameManager.Get().SendPlayerCharacterFeedback(playerFeedbackData);
			}
		}
		GameManager.Get().StopGame(GameResult.NoResult);
	}

	public int SeasonExperienceToLevel(int seasonIndex, int currentLevel)
	{
		return SeasonWideData.Get().GetSeasonExperience(seasonIndex, currentLevel);
	}

	private Sprite GetRewardSprite(RewardUtils.RewardData reward)
	{
		return (Sprite)Resources.Load(reward.SpritePath, typeof(Sprite));
	}

	private RewardUtils.RewardData GetRewardToUseForIconDisplay(List<RewardUtils.RewardData> possibleRewards, int curLevel)
	{
		List<RewardUtils.RewardData> list = new List<RewardUtils.RewardData>();
		int num = -1;
		int i = 0;
		while (i < possibleRewards.Count)
		{
			if (possibleRewards[i].Level > curLevel)
			{
				goto IL_41;
			}
			if (possibleRewards[i].isRepeating)
			{
				goto IL_41;
			}
			IL_89:
			i++;
			continue;
			IL_41:
			if (list.Count == 0)
			{
				list.Add(possibleRewards[i]);
				num = possibleRewards[i].Level;
				goto IL_89;
			}
			if (num == possibleRewards[i].Level)
			{
				list.Add(possibleRewards[i]);
				goto IL_89;
			}
			goto IL_89;
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

	private void DelaySetup(GameType gameType, GameResult gameResult, int myTeamScore, int enemyTeamScore)
	{
		if (GameBalanceVars.Get() == null)
		{
			return;
		}
		this.m_IsLevellingSeasons = true;
		for (int i = 0; i < this.m_seasonLevelContainer.Length; i++)
		{
			UIManager.SetGameObjectActive(this.m_seasonLevelContainer[i], this.m_IsLevellingSeasons, null);
		}
		for (int j = 0; j < this.m_accountLevelContainers.Length; j++)
		{
			UIManager.SetGameObjectActive(this.m_accountLevelContainers[j], !this.m_IsLevellingSeasons, null);
		}
	}

	private void SetupRankMode(GameType gameType, bool wonMatch)
	{
	}

	private void SetupTierDisplay(int tier)
	{
	}

	public void Setup(GameType gameType, GameResult gameResult, int myTeamScore, int enemyTeamScore)
	{
		this.m_gameResult = gameResult;
		this.m_updateXpStage = UIGameOverPanel.UpdateXPStage.Normal;
		this.m_waitingForXpBonusAnimFinished = false;
		Team team = (GameManager.Get().PlayerInfo.TeamId != Team.TeamB) ? Team.TeamA : Team.TeamB;
		Team team2;
		if (team == Team.TeamB)
		{
			team2 = Team.TeamB;
		}
		else
		{
			team2 = Team.TeamA;
		}
		Team team3 = team2;
		bool wonMatch = false;
		if (gameResult == GameResult.TeamAWon)
		{
			if (team3 == Team.TeamA)
			{
				goto IL_7B;
			}
		}
		if (gameResult != GameResult.TeamBWon)
		{
			goto IL_7D;
		}
		if (team3 != Team.TeamB)
		{
			goto IL_7D;
		}
		IL_7B:
		wonMatch = true;
		IL_7D:
		this.SetupRankMode(gameType, wonMatch);
		this.m_isSetup = true;
	}

	private void Update()
	{
		if (!this.m_isSetup)
		{
			return;
		}
		bool flag;
		if (this.m_notificationHasArrived)
		{
			if (!(UINewReward.Get() == null))
			{
				if (UINewReward.Get().RewardIsBeingAnnounced())
				{
					goto IL_69;
				}
			}
			flag = (Time.unscaledTime >= this.m_continueBtnFailsafeTime);
			goto IL_6A;
		}
		IL_69:
		flag = false;
		IL_6A:
		bool flag2 = flag;
		if (this.m_updateXpStage < UIGameOverPanel.UpdateXPStage.Done)
		{
			if (flag2)
			{
				this.failsafeTriggered = true;
			}
		}
		if (!UINewReward.Get().RewardIsBeingAnnounced() && this.m_updateXpStage < UIGameOverPanel.UpdateXPStage.Done)
		{
			if (this.m_waitingForXpBonusAnimFinished)
			{
				Animator animator = null;
				if (this.m_waitingForXpBonusAnimFinished)
				{
					if (animator == null)
					{
						this.m_isXpBonusAnimPlaying = false;
						this.m_updateXpStage++;
						this.m_continueBtnFailsafeTime = Time.unscaledTime + 10f;
					}
					else if (!this.m_isXpBonusAnimPlaying)
					{
						this.m_isXpBonusAnimPlaying = true;
						animator.Play("ResultsBonusIcon" + this.m_updateXpStage.ToString() + "DefaultIN");
						this.m_updateXpStartTime = Time.unscaledTime;
					}
					else if (Time.unscaledTime > animator.GetCurrentAnimatorStateInfo(0).length + this.m_updateXpStartTime)
					{
						this.m_waitingForXpBonusAnimFinished = false;
					}
				}
				if (this.failsafeTriggered)
				{
					this.failsafeTriggered = false;
					string message = string.Format("Fail safe for continue button triggered and stuck on waiting for xp bonus anim! GameName: {0}, PlayerHandle: {1}, XPStage: {2}", (GameManager.Get().GameInfo == null) ? "Game game info null!" : GameManager.Get().GameInfo.Name, ClientGameManager.Get().PlayerInfo.Handle, this.m_updateXpStage);
					Log.Error(message, new object[0]);
					this.m_updateXpStage = UIGameOverPanel.UpdateXPStage.Done;
				}
			}
			else
			{
				bool flag3 = this.UpdateInfluence();
				bool flag4 = this.UpdateRankPoints();
				bool flag5 = false;
				if (!flag5)
				{
					if (!flag3)
					{
						if (!flag4)
						{
							this.m_waitingForXpBonusAnimFinished = true;
							this.m_isXpBonusAnimPlaying = false;
							this.m_updateXpStage++;
							this.m_continueBtnFailsafeTime = Time.unscaledTime + 10f;
							return;
						}
					}
				}
				if (this.failsafeTriggered)
				{
					this.failsafeTriggered = false;
					string format = "Fail safe for continue button triggered! charUpdating: {0}, playerUpdating: {1}, currencyUpdating: {2}, influenceupdating: {3}, rankpointsupdating: {4}, GameName: {5}, PlayerHandle: {6}, XPStage: {7}";
					object[] array = new object[8];
					array[0] = false;
					array[1] = flag5;
					array[2] = false;
					array[3] = flag3;
					array[4] = flag4;
					int num = 5;
					object obj;
					if (GameManager.Get().GameInfo != null)
					{
						obj = GameManager.Get().GameInfo.Name;
					}
					else
					{
						obj = "Game game info null!";
					}
					array[num] = obj;
					array[6] = ClientGameManager.Get().PlayerInfo.Handle;
					array[7] = this.m_updateXpStage;
					string message2 = string.Format(format, array);
					Log.Error(message2, new object[0]);
					this.m_updateXpStage = UIGameOverPanel.UpdateXPStage.Done;
				}
			}
		}
	}

	private void SetTooltipClickable(UIGameOverPanel.XPDisplayInfo info, bool clickable)
	{
		if (info.m_levelUpAnimController != null)
		{
			_SelectableBtn component = info.m_levelUpAnimController.GetComponent<_SelectableBtn>();
			if (component != null)
			{
				component.spriteController.SetClickable(clickable);
			}
		}
	}

	private void InitializeRewardIcon(UIGameOverPanel.XPDisplayInfo info)
	{
	}

	private void DoLevelUpAnim(UIGameOverPanel.XPDisplayInfo dispInfo, int newLevel)
	{
	}

	private bool UpdateInfluence()
	{
		return false;
	}

	private float GetRankFillAmt(float percent)
	{
		return percent * 0.833f + 0.082f;
	}

	private bool UpdateRankPoints()
	{
		return false;
	}

	public static bool HasFriendInMatch()
	{
		bool result = false;
		FriendList friendList = ClientGameManager.Get().FriendList;
		LobbyPlayerInfo playerInfo = GameManager.Get().PlayerInfo;
		if (friendList != null && GameManager.Get().TeamInfo != null && GameManager.Get().TeamInfo.TeamPlayerInfo != null)
		{
			List<LobbyPlayerInfo> teamPlayerInfo = GameManager.Get().TeamInfo.TeamPlayerInfo;
			using (List<LobbyPlayerInfo>.Enumerator enumerator = teamPlayerInfo.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LobbyPlayerInfo lobbyPlayerInfo = enumerator.Current;
					if (!lobbyPlayerInfo.IsNPCBot)
					{
						if (lobbyPlayerInfo.AccountId > 0L)
						{
							if (playerInfo.AccountId != lobbyPlayerInfo.AccountId && friendList.Friends.ContainsKey(lobbyPlayerInfo.AccountId))
							{
								return true;
							}
						}
					}
				}
			}
		}
		return result;
	}

	[Serializable]
	public class XPDisplayInfo
	{
		public TextMeshProUGUI m_barNameLabel;

		public TextMeshProUGUI m_XPLabel;

		public TextMeshProUGUI m_barLevelLabel;

		public Animator m_levelUpAnimController;

		public Animator m_barLevelUpAnimator;

		public ImageFilledSloped m_OldXPSlider;

		public ImageFilledSloped m_NormalXPGainSlider;

		public ImageFilledSloped m_GGXPSlider;

		public ImageFilledSloped m_QuestXPSlider;

		public Button m_tooltipHitBox;

		[HideInInspector]
		public int m_visibleLevel = -1;

		[HideInInspector]
		public int m_maxVisibleLevel = -1;

		[HideInInspector]
		public int m_startXP;

		[HideInInspector]
		public int m_normalXPToIncrease;

		[HideInInspector]
		public int m_winBonusXPToIncrease;

		[HideInInspector]
		public int m_ggPackXPIncrease;

		[HideInInspector]
		public int m_xpRemainingToNextLevel;

		[HideInInspector]
		public int m_xpTotalToNextLevel;

		[HideInInspector]
		public int m_playWithFriendXpIncrease;

		[HideInInspector]
		public int m_questXpIncrease;

		[HideInInspector]
		public int m_queueTimeXpIncrease;

		[HideInInspector]
		public int m_freelancerOwnedXpIncrease;

		[HideInInspector]
		public int m_eventBonusXpToIncrease;

		[HideInInspector]
		public int m_previousLevelXPShown;

		[HideInInspector]
		public float m_xpShownAdded;

		[HideInInspector]
		public bool m_ready;

		[HideInInspector]
		public bool m_startedOnLevelUpYesReward;

		[HideInInspector]
		public int m_normalXPInitial;

		[HideInInspector]
		public int m_winBonusXPInitial;

		[HideInInspector]
		public int m_ggPackXPInitial;

		[HideInInspector]
		public int m_playWithFriendXpInitial;

		[HideInInspector]
		public int m_questXpInitial;

		[HideInInspector]
		public int m_queueTimeXpInitial;

		[HideInInspector]
		public int m_freelancerOwnedXpInitial;

		[HideInInspector]
		public int m_eventBonusXpInitial;

		public void Clear(bool haveRewardNextLevel = true)
		{
			this.m_ready = false;
			this.m_visibleLevel = -1;
			this.m_normalXPToIncrease = 0;
			this.m_winBonusXPToIncrease = 0;
			this.m_ggPackXPIncrease = 0;
			this.m_xpRemainingToNextLevel = 0;
			this.m_xpTotalToNextLevel = 0;
			this.m_playWithFriendXpIncrease = 0;
			this.m_questXpIncrease = 0;
			this.m_queueTimeXpIncrease = 0;
			this.m_freelancerOwnedXpIncrease = 0;
			this.m_eventBonusXpToIncrease = 0;
			this.m_xpShownAdded = 0f;
			this.m_previousLevelXPShown = 0;
			this.m_OldXPSlider.fillAmount = 0f;
			this.m_NormalXPGainSlider.fillAmount = 0f;
			this.m_GGXPSlider.fillAmount = 0f;
			this.m_QuestXPSlider.fillAmount = 0f;
			this.m_normalXPInitial = 0;
			this.m_winBonusXPInitial = 0;
			this.m_ggPackXPInitial = 0;
			this.m_playWithFriendXpInitial = 0;
			this.m_questXpInitial = 0;
			this.m_queueTimeXpInitial = 0;
			this.m_freelancerOwnedXpInitial = 0;
			this.m_eventBonusXpInitial = 0;
			this.m_startXP = 0;
			this.m_startedOnLevelUpYesReward = haveRewardNextLevel;
		}

		public int GetTotalExpIncrease()
		{
			return this.m_normalXPToIncrease + this.m_winBonusXPToIncrease + this.m_ggPackXPIncrease + this.m_playWithFriendXpIncrease + this.m_questXpIncrease + this.m_queueTimeXpIncrease + this.m_freelancerOwnedXpIncrease + this.m_eventBonusXpToIncrease;
		}

		public int GetTotalExpInitial()
		{
			return this.m_normalXPInitial + this.m_winBonusXPInitial + this.m_ggPackXPInitial + this.m_playWithFriendXpInitial + this.m_questXpInitial + this.m_queueTimeXpInitial + this.m_freelancerOwnedXpInitial + this.m_eventBonusXpInitial;
		}
	}

	[Serializable]
	public class CurrencyDisplayInfo
	{
		public RectTransform m_container;

		public TextMeshProUGUI m_currencyGainText;

		public UITooltipHoverObject m_tooltipHitBox;

		[HideInInspector]
		public MatchResultsNotification.CurrencyReward m_currencyReward;

		[HideInInspector]
		public int m_currentDisplayAmt;
	}

	public enum GameOverScreenState
	{
		None,
		VictoryDefeat,
		GGBoostInGameRecap,
		GGBoostUsage,
		TutorialTenGames,
		TopParticipants,
		PersonalHighlights,
		MissionNotifications,
		ExperienceBars,
		Stats,
		Rewards
	}

	public enum UpdateXPStage
	{
		Normal,
		EventBonus,
		QueueTime,
		FreelancerOwned,
		WinBonus,
		WillFill,
		Party,
		GG,
		Quest,
		Done
	}

	internal class TeamELOs
	{
		internal float totalUs;

		internal float totalThem;

		internal float countUs;

		internal float countThem;

		internal Team ourTeam = Team.Invalid;

		internal TeamELOs(Team team)
		{
			this.ourTeam = team;
		}

		private string GetHTMLColorElement(int imbal, int bal, float howBalanced)
		{
			howBalanced = howBalanced * howBalanced * howBalanced;
			float num = (float)(bal - imbal);
			int num2 = (int)((0.49f + num) * howBalanced);
			return string.Format("{0:X02}", num2);
		}

		private string HTMLColor
		{
			get
			{
				float num = Mathf.Min(this.totalUs, this.totalThem);
				if (num <= 0f)
				{
					return "A0A0A0";
				}
				float num2 = Mathf.Max(this.totalUs, this.totalThem) / num;
				num2 = Mathf.Min(2f, num2);
				num2 = Mathf.Max(1f, num2);
				num2 = 2f - num2;
				if (this.totalUs > this.totalThem)
				{
					return this.GetHTMLColorElement(0, 0xFF, num2) + "FF" + this.GetHTMLColorElement(0, 0xFF, num2);
				}
				return "FF" + this.GetHTMLColorElement(0, 0xFF, num2) + this.GetHTMLColorElement(0, 0xFF, num2);
			}
		}

		internal string ToHTML(string prefix)
		{
			if (this.countUs > 0f)
			{
				if (this.countThem > 0f)
				{
					if (this.totalUs > 0f)
					{
						if (this.totalThem > 0f)
						{
							return string.Format("{0}: <color=#{1}>{2:F0} v {3:F0}</color>", new object[]
							{
								prefix,
								this.HTMLColor,
								this.totalUs / this.countUs,
								this.totalThem / this.countThem
							});
						}
					}
				}
			}
			return string.Format("{0}: ERR", prefix);
		}

		internal void AddPlayer(Team team, float elo)
		{
			if (team == this.ourTeam)
			{
				this.totalUs += elo;
				this.countUs += 1f;
			}
			else if (team == this.ourTeam.OtherTeam())
			{
				this.totalThem += elo;
				this.countThem += 1f;
			}
		}
	}
}
