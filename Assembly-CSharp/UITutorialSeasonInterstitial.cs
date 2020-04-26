using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
		return s_instance;
	}

	public override SceneType GetSceneType()
	{
		return SceneType.TutorialInterstitial;
	}

	public override void Awake()
	{
		m_normalBars = new List<UITutorialSeasonLevelBar>();
		m_normalBars.AddRange(m_barLayout.GetComponentsInChildren<UITutorialSeasonLevelBar>(true));
		m_normalBars.Remove(m_seasonEndBar);
		m_unanimated = new Queue<UITutorialSeasonLevelBar>();
		m_btnClose.spriteController.callback = delegate
		{
			SetVisible(false);
			if (UIGameOverScreen.Get() != null)
			{
				UIGameOverScreen.Get().NotifySeasonTutorialScreenClosed();
			}
			UINewUserFlowManager.HighlightQueued();
		};
		s_instance = this;
		base.Awake();
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public void SetVisible(bool visible)
	{
		m_isVisible = visible;
		UIManager.SetGameObjectActive(m_container, visible);
		if (!visible)
		{
			return;
		}
		while (true)
		{
			m_hasBeenViewed = true;
			return;
		}
	}

	public bool IsVisible()
	{
		return m_isVisible;
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
			m_playXMoreGamesText.text = string.Empty;
		}
		else
		{
			m_playXMoreGamesText.text = string.Format(StringUtil.TR("PlayXMoreGames", "OverlayScreensScene"), num);
		}
		Queue<RewardUtils.RewardData> queue = new Queue<RewardUtils.RewardData>(RewardUtils.GetSeasonLevelRewards());
		for (int i = 1; i < endLevel - 1; i++)
		{
			int num2 = i - 1;
			UITutorialSeasonLevelBar uITutorialSeasonLevelBar;
			if (num2 < m_normalBars.Count)
			{
				uITutorialSeasonLevelBar = m_normalBars[num2];
			}
			else
			{
				uITutorialSeasonLevelBar = Object.Instantiate(m_normalBarPrefab);
				uITutorialSeasonLevelBar.transform.SetParent(m_barLayout.transform);
				uITutorialSeasonLevelBar.transform.localPosition = Vector3.zero;
				uITutorialSeasonLevelBar.transform.localScale = Vector3.one;
				m_normalBars.Add(uITutorialSeasonLevelBar);
			}
			RewardUtils.RewardData rewardData = null;
			while (queue.Count > 0)
			{
				if (rewardData == null)
				{
					int num3 = queue.Peek().Level - 1;
					if (num3 < i)
					{
						queue.Dequeue();
						continue;
					}
					if (num3 > i)
					{
						break;
					}
					rewardData = queue.Dequeue();
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(uITutorialSeasonLevelBar, true);
			uITutorialSeasonLevelBar.SetReward(i, rewardData);
			if (!uITutorialSeasonLevelBar.SetFilled(currentLevel > i))
			{
				m_unanimated.Enqueue(uITutorialSeasonLevelBar);
			}
			if (!isMatchEnd)
			{
				continue;
			}
			if (currentLevel == i)
			{
				m_toLevel = uITutorialSeasonLevelBar;
				m_toLevelReward = rewardData;
				m_timeToLevel = Time.time + 1f;
				m_isFinal = false;
			}
		}
		for (int j = endLevel - 2; j < m_normalBars.Count; j++)
		{
			UIManager.SetGameObjectActive(m_normalBars[j], false);
		}
		m_seasonEndBar.transform.SetAsLastSibling();
		List<RewardUtils.RewardData> availableSeasonEndRewards = RewardUtils.GetAvailableSeasonEndRewards(season);
		UITutorialSeasonLevelBar seasonEndBar = m_seasonEndBar;
		int level = endLevel - 1;
		object reward;
		if (availableSeasonEndRewards.Count > 0)
		{
			reward = availableSeasonEndRewards[0];
		}
		else
		{
			reward = null;
		}
		seasonEndBar.SetReward(level, (RewardUtils.RewardData)reward);
		if (!m_seasonEndBar.SetFilled(currentLevel >= endLevel))
		{
			m_unanimated.Enqueue(m_seasonEndBar);
		}
		if (isMatchEnd)
		{
			if (currentLevel + 1 == endLevel)
			{
				m_toLevel = m_seasonEndBar;
				object toLevelReward;
				if (availableSeasonEndRewards.Count > 0)
				{
					toLevelReward = availableSeasonEndRewards[0];
				}
				else
				{
					toLevelReward = null;
				}
				m_toLevelReward = (RewardUtils.RewardData)toLevelReward;
				m_timeToLevel = Time.time + 1f;
				m_isFinal = true;
			}
		}
		UIManager.SetGameObjectActive(m_earnFreelancerTokenText, !ClientGameManager.Get().HasPurchasedGame);
	}

	public bool HasBeenViewed()
	{
		return m_hasBeenViewed;
	}

	private void Update()
	{
		while (true)
		{
			if (!m_unanimated.IsNullOrEmpty())
			{
				UITutorialSeasonLevelBar uITutorialSeasonLevelBar = m_unanimated.Dequeue();
				if (!uITutorialSeasonLevelBar.AnimateFill())
				{
					m_unanimated.Enqueue(uITutorialSeasonLevelBar);
					break;
				}
				continue;
			}
			break;
		}
		if (!(m_toLevel != null))
		{
			return;
		}
		while (true)
		{
			if (!(m_timeToLevel < Time.time))
			{
				return;
			}
			while (true)
			{
				m_toLevel.SetFilled(true);
				m_toLevel = null;
				if (m_toLevelReward != null)
				{
					UINewReward.Get().NotifyNewRewardReceived(m_toLevelReward);
					m_toLevelReward = null;
				}
				if (m_isFinal)
				{
					UIFrontEnd.PlaySound(FrontEndButtonSounds.FirstTenGamesPregressComplete);
				}
				else
				{
					UIFrontEnd.PlaySound(FrontEndButtonSounds.FirstTenGamesProgressIncrement);
				}
				return;
			}
		}
	}
}
