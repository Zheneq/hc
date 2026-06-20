using System.Collections.Generic;

public class UISeasonCommunityRankRewardEntry
{
	public int CommunityRankRequired;

	public int CommunitySeasonLevelsRequired;

	public List<UISeasonRewardDisplayInfo> CommunityRewards;

	public void Clear()
	{
		CommunityRewards.Clear();
	}
}
