using UnityEngine;

public class UISeasonRewardDisplayInfo
{
	public string Reward;

	public bool RequiresCommunityRank;

	private SeasonReward m_seasonRewardRef;

	public SeasonReward GetReward()
	{
		return m_seasonRewardRef;
	}

	public void Setup(SeasonReward seasonReward)
	{
		m_seasonRewardRef = seasonReward;
		RequiresCommunityRank = (seasonReward.requiredCommunityGoalIndex > 0);
		Reward = string.Format(StringUtil.TR("RewardNumber", "Seasons"), Mathf.FloorToInt(Random.value * 100f));
	}
}
