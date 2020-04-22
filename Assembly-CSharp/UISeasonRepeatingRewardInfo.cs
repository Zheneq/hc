using System;
using UnityEngine;

public class UISeasonRepeatingRewardInfo : IEquatable<UISeasonRepeatingRewardInfo>, IComparable<UISeasonRepeatingRewardInfo>
{
	public int StartLevel;

	public int RepeatEveryXLevels;

	private SeasonReward SeasonRewardRef;

	public UISeasonRepeatingRewardInfo(SeasonReward reward)
	{
		SeasonRewardRef = reward;
		StartLevel = reward.level;
		RepeatEveryXLevels = reward.repeatEveryXLevels;
	}

	public SeasonReward GetSeasonRewardReference()
	{
		return SeasonRewardRef;
	}

	public Sprite GetDisplaySprite()
	{
		if (SeasonRewardRef is SeasonItemReward)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate((SeasonRewardRef as SeasonItemReward).ItemReward.ItemTemplateId);
			if (itemTemplate != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						return Resources.Load(itemTemplate.IconPath, typeof(Sprite)) as Sprite;
					}
				}
			}
		}
		else
		{
			if (SeasonRewardRef is SeasonUnlockReward)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					return Resources.Load((SeasonRewardRef as SeasonUnlockReward).UnlockReward.resourceString, typeof(Sprite)) as Sprite;
				}
			}
			if (SeasonRewardRef is SeasonCurrencyReward)
			{
				string spritePath = InventoryWideData.GetSpritePath((SeasonRewardRef as SeasonCurrencyReward).GetItemTemplate());
				return Resources.Load(spritePath, typeof(Sprite)) as Sprite;
			}
		}
		return null;
	}

	public bool Equals(UISeasonRepeatingRewardInfo other)
	{
		if (other == null)
		{
			return false;
		}
		return GetSeasonRewardReference() == other.GetSeasonRewardReference();
	}

	public int CompareTo(UISeasonRepeatingRewardInfo other)
	{
		if (other == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return -1;
				}
			}
		}
		if (other.RepeatEveryXLevels > RepeatEveryXLevels)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return -1;
				}
			}
		}
		if (other.RepeatEveryXLevels < RepeatEveryXLevels)
		{
			return 1;
		}
		return 0;
	}
}
