using System;
using System.Collections.Generic;

public class UISeasonRewardEntry : IDataEntry
{
	public int LevelToGetReward;

	public bool isLevelled;

	public bool isCurrentLevel;

	public bool isPreviewingLevel;

	public float previewAlpha;

	public List<UISeasonRewardDisplayInfo> Rewards;

	public List<UISeasonRepeatingRewardInfo> RepeatingRewards;

	public UISeasonRewardEntry()
	{
		this.Rewards = new List<UISeasonRewardDisplayInfo>();
		this.RepeatingRewards = new List<UISeasonRepeatingRewardInfo>();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is UISeasonRewardEntry))
		{
			return false;
		}
		UISeasonRewardEntry uiseasonRewardEntry = (UISeasonRewardEntry)obj;
		if (this.isLevelled == uiseasonRewardEntry.isLevelled)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonRewardEntry.Equals(object)).MethodHandle;
			}
			if (this.LevelToGetReward == uiseasonRewardEntry.LevelToGetReward)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.isCurrentLevel == uiseasonRewardEntry.isCurrentLevel && this.isPreviewingLevel == uiseasonRewardEntry.isPreviewingLevel)
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
					return this.Rewards.Count == uiseasonRewardEntry.Rewards.Count;
				}
			}
		}
		return false;
	}

	public override int GetHashCode()
	{
		return this.isLevelled.GetHashCode() ^ this.LevelToGetReward.GetHashCode() ^ this.isCurrentLevel.GetHashCode() ^ this.isPreviewingLevel.GetHashCode();
	}

	public void Clear()
	{
		this.LevelToGetReward = 0;
		this.isLevelled = false;
		this.Rewards.Clear();
	}

	public void Init(int level, bool levelled, List<SeasonReward> rewards, List<UISeasonRepeatingRewardInfo> repeatingRewards, bool activeLevel)
	{
		this.LevelToGetReward = level;
		this.isLevelled = levelled;
		this.isCurrentLevel = activeLevel;
		if (rewards != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonRewardEntry.Init(int, bool, List<SeasonReward>, List<UISeasonRepeatingRewardInfo>, bool)).MethodHandle;
			}
			for (int i = 0; i < rewards.Count; i++)
			{
				UISeasonRewardDisplayInfo uiseasonRewardDisplayInfo = new UISeasonRewardDisplayInfo();
				uiseasonRewardDisplayInfo.Setup(rewards[i]);
				this.Rewards.Add(uiseasonRewardDisplayInfo);
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
		}
		if (repeatingRewards != null)
		{
			for (int j = 0; j < repeatingRewards.Count; j++)
			{
				this.RepeatingRewards.Add(repeatingRewards[j]);
			}
		}
	}

	public int GetPrefabIndexToDisplay()
	{
		if (this.Rewards.Count == 0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonRewardEntry.GetPrefabIndexToDisplay()).MethodHandle;
			}
			return 0;
		}
		return 1;
	}

	public void Setup(int displayIndex, _LargeScrollListItemEntry UIEntry)
	{
		UISeasonsRewardEntry component = UIEntry.GetComponent<UISeasonsRewardEntry>();
		if (component != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonRewardEntry.Setup(int, _LargeScrollListItemEntry)).MethodHandle;
			}
			if (this.isLevelled)
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
				component.SetAsLevelledup();
			}
			else
			{
				component.SetAsNotLevelled();
			}
			component.DisplayAsCurrentLevel(this.isCurrentLevel, !this.isCurrentLevel);
			component.SetPreviewingLevel(this.isPreviewingLevel);
			component.m_btn.NotifyHoverStatusChange(false, true, 1f);
			component.SetLevelLabelsText(this.LevelToGetReward.ToString());
			component.SetupReward(this.Rewards, this.RepeatingRewards);
			component.DoRewardIconFade(!this.isLevelled);
		}
	}
}
