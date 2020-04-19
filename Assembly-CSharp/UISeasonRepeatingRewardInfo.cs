using System;
using UnityEngine;

public class UISeasonRepeatingRewardInfo : IEquatable<UISeasonRepeatingRewardInfo>, IComparable<UISeasonRepeatingRewardInfo>
{
	public int StartLevel;

	public int RepeatEveryXLevels;

	private SeasonReward SeasonRewardRef;

	public UISeasonRepeatingRewardInfo(SeasonReward reward)
	{
		this.SeasonRewardRef = reward;
		this.StartLevel = reward.level;
		this.RepeatEveryXLevels = reward.repeatEveryXLevels;
	}

	public SeasonReward GetSeasonRewardReference()
	{
		return this.SeasonRewardRef;
	}

	public Sprite GetDisplaySprite()
	{
		if (this.SeasonRewardRef is SeasonItemReward)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonRepeatingRewardInfo.GetDisplaySprite()).MethodHandle;
			}
			InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate((this.SeasonRewardRef as SeasonItemReward).ItemReward.ItemTemplateId);
			if (itemTemplate != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				return Resources.Load(itemTemplate.IconPath, typeof(Sprite)) as Sprite;
			}
		}
		else
		{
			if (this.SeasonRewardRef is SeasonUnlockReward)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				return Resources.Load((this.SeasonRewardRef as SeasonUnlockReward).UnlockReward.resourceString, typeof(Sprite)) as Sprite;
			}
			if (this.SeasonRewardRef is SeasonCurrencyReward)
			{
				string spritePath = InventoryWideData.GetSpritePath((this.SeasonRewardRef as SeasonCurrencyReward).GetItemTemplate());
				return Resources.Load(spritePath, typeof(Sprite)) as Sprite;
			}
		}
		return null;
	}

	public bool Equals(UISeasonRepeatingRewardInfo other)
	{
		return other != null && this.GetSeasonRewardReference() == other.GetSeasonRewardReference();
	}

	public int CompareTo(UISeasonRepeatingRewardInfo other)
	{
		if (other == null)
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonRepeatingRewardInfo.CompareTo(UISeasonRepeatingRewardInfo)).MethodHandle;
			}
			return -1;
		}
		if (other.RepeatEveryXLevels > this.RepeatEveryXLevels)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			return -1;
		}
		if (other.RepeatEveryXLevels < this.RepeatEveryXLevels)
		{
			return 1;
		}
		return 0;
	}
}
