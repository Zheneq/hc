using System;
using UnityEngine;

public class UISeasonRewardDisplayInfo
{
	public string Reward;

	public bool RequiresCommunityRank;

	private SeasonReward m_seasonRewardRef;

	public SeasonReward GetReward()
	{
		return this.m_seasonRewardRef;
	}

	public void Setup(SeasonReward seasonReward)
	{
		this.m_seasonRewardRef = seasonReward;
		this.RequiresCommunityRank = (seasonReward.requiredCommunityGoalIndex > 0);
		this.Reward = string.Format(StringUtil.TR("RewardNumber", "Seasons"), Mathf.FloorToInt(UnityEngine.Random.value * 100f));
	}
}
