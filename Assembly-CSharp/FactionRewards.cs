using System;
using System.Collections.Generic;

[Serializable]
public class FactionRewards
{
	public List<FactionCurrencyReward> CurrencyRewards;

	public List<FactionUnlockReward> UnlockRewards;

	public List<FactionItemReward> ItemRewards;

	public List<FactionReward> GetAllRewards()
	{
		List<FactionReward> list = new List<FactionReward>();
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
