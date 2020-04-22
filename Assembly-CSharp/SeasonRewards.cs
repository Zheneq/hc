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
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
				switch (5)
				{
				case 0:
					continue;
				}
				return list;
			}
		}
	}
}
