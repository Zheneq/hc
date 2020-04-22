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
		for (int i = 0; i < CurrencyRewards.Count; i++)
		{
			list.Add(CurrencyRewards[i]);
		}
		while (true)
		{
			switch (1)
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
				switch (7)
				{
				case 0:
					continue;
				}
				return list;
			}
		}
	}
}
