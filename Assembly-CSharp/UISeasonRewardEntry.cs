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
			if (this.LevelToGetReward == uiseasonRewardEntry.LevelToGetReward)
			{
				if (this.isCurrentLevel == uiseasonRewardEntry.isCurrentLevel && this.isPreviewingLevel == uiseasonRewardEntry.isPreviewingLevel)
				{
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
			for (int i = 0; i < rewards.Count; i++)
			{
				UISeasonRewardDisplayInfo uiseasonRewardDisplayInfo = new UISeasonRewardDisplayInfo();
				uiseasonRewardDisplayInfo.Setup(rewards[i]);
				this.Rewards.Add(uiseasonRewardDisplayInfo);
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
			return 0;
		}
		return 1;
	}

	public void Setup(int displayIndex, _LargeScrollListItemEntry UIEntry)
	{
		UISeasonsRewardEntry component = UIEntry.GetComponent<UISeasonsRewardEntry>();
		if (component != null)
		{
			if (this.isLevelled)
			{
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
