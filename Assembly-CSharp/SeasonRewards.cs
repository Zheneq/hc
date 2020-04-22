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
		for (int i = 0; i < CurrencyRewards.Count; i++)
		{
			list.Add(CurrencyRewards[i]);
		}
		while (true)
		{
			for (int j = 0; j < UnlockRewards.Count; j++)
			{
				list.Add(UnlockRewards[j]);
			}
			for (int k = 0; k < ItemRewards.Count; k++)
			{
				list.Add(ItemRewards[k]);
			}
			while (true)
			{
				return list;
			}
		}
	}
}
