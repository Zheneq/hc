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
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(SeasonRewards.GetAllRewards()).MethodHandle;
		}
		for (int j = 0; j < this.UnlockRewards.Count; j++)
		{
			list.Add(this.UnlockRewards[j]);
		}
		for (int k = 0; k < this.ItemRewards.Count; k++)
		{
			list.Add(this.ItemRewards[k]);
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		return list;
	}
}
