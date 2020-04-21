using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITutorialSeasonInterstitial : UIScene
{
	public RectTransform m_container;

	public TextMeshProUGUI m_playXMoreGamesText;

	public UITutorialSeasonLevelBar m_seasonEndBar;

	public UITutorialSeasonLevelBar m_normalBarPrefab;

	public HorizontalLayoutGroup m_barLayout;

	public _SelectableBtn m_btnClose;

	public TextMeshProUGUI m_earnFreelancerTokenText;

	private List<UITutorialSeasonLevelBar> m_normalBars;

	private Queue<UITutorialSeasonLevelBar> m_unanimated;

	private UITutorialSeasonLevelBar m_toLevel;

	private RewardUtils.RewardData m_toLevelReward;

	private float m_timeToLevel;

	private bool m_isFinal;

	private bool m_isVisible;

	private static bool m_hasBeenViewed;

	private const float kLevelDelay = 1f;

	private static UITutorialSeasonInterstitial s_instance;

	public static UITutorialSeasonInterstitial Get()
	{
		return UITutorialSeasonInterstitial.s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.TutorialInterstitial;
	}

	public override void Awake()
	{
		this.m_normalBars = new List<UITutorialSeasonLevelBar>();
		this.m_normalBars.AddRange(this.m_barLayout.GetComponentsInChildren<UITutorialSeasonLevelBar>(true));
		this.m_normalBars.Remove(this.m_seasonEndBar);
		this.m_unanimated = new Queue<UITutorialSeasonLevelBar>();
		this.m_btnClose.spriteController.callback = delegate(BaseEventData baseEventData)
		{
			this.SetVisible(false);
			if (UIGameOverScreen.Get() != null)
			{
				UIGameOverScreen.Get().NotifySeasonTutorialScreenClosed();
			}
			UINewUserFlowManager.HighlightQueued();
		};
		UITutorialSeasonInterstitial.s_instance = this;
		base.Awake();
	}

	private void OnDestroy()
	{
		UITutorialSeasonInterstitial.s_instance = null;
	}

	public void SetVisible(bool visible)
	{
		this.m_isVisible = visible;
		UIManager.SetGameObjectActive(this.m_container, visible, null);
		if (visible)
		{
			UITutorialSeasonInterstitial.m_hasBeenViewed = true;
		}
	}

	public bool IsVisible()
	{
		return this.m_isVisible;
	}

	public void Setup(SeasonTemplate season, int currentLevel, bool isMatchEnd)
	{
		int endLevel = QuestWideData.GetEndLevel(season.Prerequisites, season.Index);
		int num = endLevel - currentLevel;
		if (isMatchEnd)
		{
			num--;
		}
		if (num == 0)
		{
			this.m_playXMoreGamesText.text = string.Empty;
		}
		else
		{
			this.m_playXMoreGamesText.text = string.Format(StringUtil.TR("PlayXMoreGames", "OverlayScreensScene"), num);
		}
		Queue<RewardUtils.RewardData> queue = new Queue<RewardUtils.RewardData>(RewardUtils.GetSeasonLevelRewards(-1));
		for (int i = 1; i < endLevel - 1; i++)
		{
			int num2 = i - 1;
			UITutorialSeasonLevelBar uitutorialSeasonLevelBar;
			if (num2 < this.m_normalBars.Count)
			{
				uitutorialSeasonLevelBar = this.m_normalBars[num2];
			}
			else
			{
				uitutorialSeasonLevelBar = UnityEngine.Object.Instantiate<UITutorialSeasonLevelBar>(this.m_normalBarPrefab);
				uitutorialSeasonLevelBar.transform.SetParent(this.m_barLayout.transform);
				uitutorialSeasonLevelBar.transform.localPosition = Vector3.zero;
				uitutorialSeasonLevelBar.transform.localScale = Vector3.one;
				this.m_normalBars.Add(uitutorialSeasonLevelBar);
			}
			RewardUtils.RewardData rewardData = null;
			while (queue.Count > 0)
			{
				if (rewardData != null)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						goto IL_185;
					}
				}
				else
				{
					int num3 = queue.Peek().Level - 1;
					if (num3 < i)
					{
						queue.Dequeue();
					}
					else
					{
						if (num3 > i)
						{
							break;
						}
						rewardData = queue.Dequeue();
					}
				}
			}
			IL_185:
			UIManager.SetGameObjectActive(uitutorialSeasonLevelBar, true, null);
			uitutorialSeasonLevelBar.SetReward(i, rewardData);
			if (!uitutorialSeasonLevelBar.SetFilled(currentLevel > i))
			{
				this.m_unanimated.Enqueue(uitutorialSeasonLevelBar);
			}
			if (isMatchEnd)
			{
				if (currentLevel == i)
				{
					this.m_toLevel = uitutorialSeasonLevelBar;
					this.m_toLevelReward = rewardData;
					this.m_timeToLevel = Time.time + 1f;
					this.m_isFinal = false;
				}
			}
		}
		for (int j = endLevel - 2; j < this.m_normalBars.Count; j++)
		{
			UIManager.SetGameObjectActive(this.m_normalBars[j], false, null);
		}
		this.m_seasonEndBar.transform.SetAsLastSibling();
		List<RewardUtils.RewardData> availableSeasonEndRewards = RewardUtils.GetAvailableSeasonEndRewards(season);
		UITutorialSeasonLevelBar seasonEndBar = this.m_seasonEndBar;
		int level = endLevel - 1;
		RewardUtils.RewardData reward;
		if (availableSeasonEndRewards.Count > 0)
		{
			reward = availableSeasonEndRewards[0];
		}
		else
		{
			reward = null;
		}
		seasonEndBar.SetReward(level, reward);
		if (!this.m_seasonEndBar.SetFilled(currentLevel >= endLevel))
		{
			this.m_unanimated.Enqueue(this.m_seasonEndBar);
		}
		if (isMatchEnd)
		{
			if (currentLevel + 1 == endLevel)
			{
				this.m_toLevel = this.m_seasonEndBar;
				RewardUtils.RewardData toLevelReward;
				if (availableSeasonEndRewards.Count > 0)
				{
					toLevelReward = availableSeasonEndRewards[0];
				}
				else
				{
					toLevelReward = null;
				}
				this.m_toLevelReward = toLevelReward;
				this.m_timeToLevel = Time.time + 1f;
				this.m_isFinal = true;
			}
		}
		UIManager.SetGameObjectActive(this.m_earnFreelancerTokenText, !ClientGameManager.Get().HasPurchasedGame, null);
	}

	public bool HasBeenViewed()
	{
		return UITutorialSeasonInterstitial.m_hasBeenViewed;
	}

	private void Update()
	{
		while (!this.m_unanimated.IsNullOrEmpty<UITutorialSeasonLevelBar>())
		{
			UITutorialSeasonLevelBar uitutorialSeasonLevelBar = this.m_unanimated.Dequeue();
			if (!uitutorialSeasonLevelBar.AnimateFill())
			{
				this.m_unanimated.Enqueue(uitutorialSeasonLevelBar);
				break;
			}
		}
		if (this.m_toLevel != null)
		{
			if (this.m_timeToLevel < Time.time)
			{
				this.m_toLevel.SetFilled(true);
				this.m_toLevel = null;
				if (this.m_toLevelReward != null)
				{
					UINewReward.Get().NotifyNewRewardReceived(this.m_toLevelReward, CharacterType.None, -1, -1);
					this.m_toLevelReward = null;
				}
				if (this.m_isFinal)
				{
					UIFrontEnd.PlaySound(FrontEndButtonSounds.FirstTenGamesPregressComplete);
				}
				else
				{
					UIFrontEnd.PlaySound(FrontEndButtonSounds.FirstTenGamesProgressIncrement);
				}
			}
		}
	}
}
