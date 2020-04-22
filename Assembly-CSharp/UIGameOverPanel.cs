using LobbyGameClientMessages;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIGameOverPanel : UIScene
{
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
			m_ready = false;
			m_visibleLevel = -1;
			m_normalXPToIncrease = 0;
			m_winBonusXPToIncrease = 0;
			m_ggPackXPIncrease = 0;
			m_xpRemainingToNextLevel = 0;
			m_xpTotalToNextLevel = 0;
			m_playWithFriendXpIncrease = 0;
			m_questXpIncrease = 0;
			m_queueTimeXpIncrease = 0;
			m_freelancerOwnedXpIncrease = 0;
			m_eventBonusXpToIncrease = 0;
			m_xpShownAdded = 0f;
			m_previousLevelXPShown = 0;
			m_OldXPSlider.fillAmount = 0f;
			m_NormalXPGainSlider.fillAmount = 0f;
			m_GGXPSlider.fillAmount = 0f;
			m_QuestXPSlider.fillAmount = 0f;
			m_normalXPInitial = 0;
			m_winBonusXPInitial = 0;
			m_ggPackXPInitial = 0;
			m_playWithFriendXpInitial = 0;
			m_questXpInitial = 0;
			m_queueTimeXpInitial = 0;
			m_freelancerOwnedXpInitial = 0;
			m_eventBonusXpInitial = 0;
			m_startXP = 0;
			m_startedOnLevelUpYesReward = haveRewardNextLevel;
		}

		public int GetTotalExpIncrease()
		{
			return m_normalXPToIncrease + m_winBonusXPToIncrease + m_ggPackXPIncrease + m_playWithFriendXpIncrease + m_questXpIncrease + m_queueTimeXpIncrease + m_freelancerOwnedXpIncrease + m_eventBonusXpToIncrease;
		}

		public int GetTotalExpInitial()
		{
			return m_normalXPInitial + m_winBonusXPInitial + m_ggPackXPInitial + m_playWithFriendXpInitial + m_questXpInitial + m_queueTimeXpInitial + m_freelancerOwnedXpInitial + m_eventBonusXpInitial;
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

		private string HTMLColor
		{
			get
			{
				float num = Mathf.Min(totalUs, totalThem);
				if (num <= 0f)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							return "A0A0A0";
						}
					}
				}
				float b = Mathf.Max(totalUs, totalThem) / num;
				b = Mathf.Min(2f, b);
				b = Mathf.Max(1f, b);
				b = 2f - b;
				if (totalUs > totalThem)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							break;
						default:
							return GetHTMLColorElement(0, 255, b) + "FF" + GetHTMLColorElement(0, 255, b);
						}
					}
				}
				return "FF" + GetHTMLColorElement(0, 255, b) + GetHTMLColorElement(0, 255, b);
			}
		}

		internal TeamELOs(Team team)
		{
			ourTeam = team;
		}

		private string GetHTMLColorElement(int imbal, int bal, float howBalanced)
		{
			howBalanced = howBalanced * howBalanced * howBalanced;
			float num = bal - imbal;
			int num2 = (int)((0.49f + num) * howBalanced);
			return $"{num2:X02}";
		}

		internal string ToHTML(string prefix)
		{
			if (!(countUs <= 0f))
			{
				if (!(countThem <= 0f))
				{
					if (!(totalUs <= 0f))
					{
						if (!(totalThem <= 0f))
						{
							return $"{prefix}: <color=#{HTMLColor}>{totalUs / countUs:F0} v {totalThem / countThem:F0}</color>";
						}
					}
				}
			}
			return $"{prefix}: ERR";
		}

		internal void AddPlayer(Team team, float elo)
		{
			if (team == ourTeam)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						totalUs += elo;
						countUs += 1f;
						return;
					}
				}
			}
			if (team != ourTeam.OtherTeam())
			{
				return;
			}
			while (true)
			{
				totalThem += elo;
				countThem += 1f;
				return;
			}
		}
	}

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

	private GameOverScreenState m_currentState;

	private const float m_worldAnimationTime = 3.5f;

	private const float m_GGPackRecapTime = 3f;

	private const float m_GGPackRecapInterval = 0.2f;

	private float m_timeBannerDisplayed;

	private bool m_isSetup;

	private static UIGameOverPanel s_instance;

	private UpdateXPStage m_updateXpStage;

	private float m_updateXpStartTime;

	private bool failsafeTriggered;

	public UpdateXPStage XPStage => m_updateXpStage;

	public override SceneType GetSceneType()
	{
		return SceneType.GameOver;
	}

	private void Start()
	{
		s_instance = this;
		m_notificationHasArrived = false;
	}

	public static UIGameOverPanel Get()
	{
		return s_instance;
	}

	public void CancelCallback(UIDialogBox boxReference)
	{
		ClientGameManager.Get().LeaveGame(true, m_gameResult);
	}

	public void RatingCallback(UIDialogBox boxReference)
	{
		UIRatingDialogBox uIRatingDialogBox = boxReference as UIRatingDialogBox;
		if (GameFlowData.Get() != null)
		{
			if (uIRatingDialogBox.GetRating() > -1)
			{
				PlayerFeedbackData playerFeedbackData = new PlayerFeedbackData();
				playerFeedbackData.CharacterType = GameManager.Get().PlayerInfo.CharacterType;
				playerFeedbackData.Rating = uIRatingDialogBox.GetRating() + 1;
				ClientGameManager.Get().SendPlayerCharacterFeedback(playerFeedbackData);
			}
		}
		GameManager.Get().StopGame();
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
		for (int i = 0; i < possibleRewards.Count; i++)
		{
			if (possibleRewards[i].Level <= curLevel)
			{
				if (!possibleRewards[i].isRepeating)
				{
					continue;
				}
			}
			if (list.Count == 0)
			{
				list.Add(possibleRewards[i]);
				num = possibleRewards[i].Level;
			}
			else if (num == possibleRewards[i].Level)
			{
				list.Add(possibleRewards[i]);
			}
		}
		while (true)
		{
			for (int j = 0; j < GameBalanceVars.Get().RewardDisplayPriorityOrder.Length; j++)
			{
				for (int k = 0; k < list.Count; k++)
				{
					if (GameBalanceVars.Get().RewardDisplayPriorityOrder[j] != list[k].Type)
					{
						continue;
					}
					while (true)
					{
						return list[k];
					}
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						goto end_IL_00f0;
					}
					continue;
					end_IL_00f0:
					break;
				}
			}
			if (list.Count > 0)
			{
				return list[0];
			}
			return null;
		}
	}

	private void DelaySetup(GameType gameType, GameResult gameResult, int myTeamScore, int enemyTeamScore)
	{
		if (GameBalanceVars.Get() == null)
		{
			return;
		}
		m_IsLevellingSeasons = true;
		for (int i = 0; i < m_seasonLevelContainer.Length; i++)
		{
			UIManager.SetGameObjectActive(m_seasonLevelContainer[i], m_IsLevellingSeasons);
		}
		while (true)
		{
			for (int j = 0; j < m_accountLevelContainers.Length; j++)
			{
				UIManager.SetGameObjectActive(m_accountLevelContainers[j], !m_IsLevellingSeasons);
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
	}

	private void SetupRankMode(GameType gameType, bool wonMatch)
	{
	}

	private void SetupTierDisplay(int tier)
	{
	}

	public void Setup(GameType gameType, GameResult gameResult, int myTeamScore, int enemyTeamScore)
	{
		m_gameResult = gameResult;
		m_updateXpStage = UpdateXPStage.Normal;
		m_waitingForXpBonusAnimFinished = false;
		Team team = (GameManager.Get().PlayerInfo.TeamId == Team.TeamB) ? Team.TeamB : Team.TeamA;
		int num;
		if (team == Team.TeamB)
		{
			num = 1;
		}
		else
		{
			num = 0;
		}
		Team team2 = (Team)num;
		bool wonMatch = false;
		if (gameResult == GameResult.TeamAWon)
		{
			if (team2 == Team.TeamA)
			{
				goto IL_007b;
			}
		}
		if (gameResult == GameResult.TeamBWon)
		{
			if (team2 == Team.TeamB)
			{
				goto IL_007b;
			}
		}
		goto IL_007d;
		IL_007d:
		SetupRankMode(gameType, wonMatch);
		m_isSetup = true;
		return;
		IL_007b:
		wonMatch = true;
		goto IL_007d;
	}

	private void Update()
	{
		if (!m_isSetup)
		{
			while (true)
			{
				return;
			}
		}
		if (!m_notificationHasArrived)
		{
			goto IL_0069;
		}
		if (!(UINewReward.Get() == null))
		{
			if (UINewReward.Get().RewardIsBeingAnnounced())
			{
				goto IL_0069;
			}
		}
		int num = (Time.unscaledTime >= m_continueBtnFailsafeTime) ? 1 : 0;
		goto IL_006a;
		IL_006a:
		bool flag = (byte)num != 0;
		if (m_updateXpStage < UpdateXPStage.Done)
		{
			if (flag)
			{
				failsafeTriggered = true;
			}
		}
		if (UINewReward.Get().RewardIsBeingAnnounced() || m_updateXpStage >= UpdateXPStage.Done)
		{
			return;
		}
		while (true)
		{
			if (m_waitingForXpBonusAnimFinished)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
					{
						Animator animator = null;
						if (m_waitingForXpBonusAnimFinished)
						{
							if (animator == null)
							{
								m_isXpBonusAnimPlaying = false;
								m_updateXpStage++;
								m_continueBtnFailsafeTime = Time.unscaledTime + 10f;
							}
							else if (!m_isXpBonusAnimPlaying)
							{
								m_isXpBonusAnimPlaying = true;
								animator.Play("ResultsBonusIcon" + m_updateXpStage.ToString() + "DefaultIN");
								m_updateXpStartTime = Time.unscaledTime;
							}
							else if (Time.unscaledTime > animator.GetCurrentAnimatorStateInfo(0).length + m_updateXpStartTime)
							{
								m_waitingForXpBonusAnimFinished = false;
							}
						}
						if (failsafeTriggered)
						{
							failsafeTriggered = false;
							string message = string.Format("Fail safe for continue button triggered and stuck on waiting for xp bonus anim! GameName: {0}, PlayerHandle: {1}, XPStage: {2}", (GameManager.Get().GameInfo == null) ? "Game game info null!" : GameManager.Get().GameInfo.Name, ClientGameManager.Get().PlayerInfo.Handle, m_updateXpStage);
							Log.Error(message);
							m_updateXpStage = UpdateXPStage.Done;
						}
						return;
					}
					}
				}
			}
			bool flag2 = UpdateInfluence();
			bool flag3 = UpdateRankPoints();
			bool flag4 = false;
			if (!flag4)
			{
				if (!flag2)
				{
					if (!flag3)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								m_waitingForXpBonusAnimFinished = true;
								m_isXpBonusAnimPlaying = false;
								m_updateXpStage++;
								m_continueBtnFailsafeTime = Time.unscaledTime + 10f;
								return;
							}
						}
					}
				}
			}
			if (!failsafeTriggered)
			{
				return;
			}
			while (true)
			{
				failsafeTriggered = false;
				object[] obj = new object[8]
				{
					false,
					flag4,
					false,
					flag2,
					flag3,
					null,
					null,
					null
				};
				object obj2;
				if (GameManager.Get().GameInfo != null)
				{
					obj2 = GameManager.Get().GameInfo.Name;
				}
				else
				{
					obj2 = "Game game info null!";
				}
				obj[5] = obj2;
				obj[6] = ClientGameManager.Get().PlayerInfo.Handle;
				obj[7] = m_updateXpStage;
				string message2 = string.Format("Fail safe for continue button triggered! charUpdating: {0}, playerUpdating: {1}, currencyUpdating: {2}, influenceupdating: {3}, rankpointsupdating: {4}, GameName: {5}, PlayerHandle: {6}, XPStage: {7}", obj);
				Log.Error(message2);
				m_updateXpStage = UpdateXPStage.Done;
				return;
			}
		}
		IL_0069:
		num = 0;
		goto IL_006a;
	}

	private void SetTooltipClickable(XPDisplayInfo info, bool clickable)
	{
		if (!(info.m_levelUpAnimController != null))
		{
			return;
		}
		while (true)
		{
			_SelectableBtn component = info.m_levelUpAnimController.GetComponent<_SelectableBtn>();
			if (component != null)
			{
				component.spriteController.SetClickable(clickable);
			}
			return;
		}
	}

	private void InitializeRewardIcon(XPDisplayInfo info)
	{
	}

	private void DoLevelUpAnim(XPDisplayInfo dispInfo, int newLevel)
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
					LobbyPlayerInfo current = enumerator.Current;
					if (!current.IsNPCBot)
					{
						if (current.AccountId > 0)
						{
							if (playerInfo.AccountId != current.AccountId && friendList.Friends.ContainsKey(current.AccountId))
							{
								while (true)
								{
									switch (1)
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
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						return result;
					}
				}
			}
		}
		return result;
	}
}
