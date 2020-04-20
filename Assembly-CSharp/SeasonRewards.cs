using System;
using System.Collections.Generic;

[Serializable]
public class SeasonRewards
{
	public List<SeasonCurrencyReward> CurrencyRewards;

	public List<SeasonUnlockReward> UnlockRewards;

	public List<SeasonItemReward> ItemRewards;

	public List<SeasonReward> GetAllRewards()
	{
		List<SeasonReward> list = new List<SeasonReward>();
		for (int i = 0; i < this.CurrencyRewards.Count; i++)
		{
			list.Add(this.CurrencyRewards[i]);
		}
		for (int j = 0; j < this.UnlockRewards.Count; j++)
		{
			list.Add(this.UnlockRewards[j]);
		}
		for (int k = 0; k < this.ItemRewards.Count; k++)
		{
			list.Add(this.ItemRewards[k]);
		}
		return list;
	}
}
