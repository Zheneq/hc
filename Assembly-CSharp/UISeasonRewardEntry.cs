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
		Rewards = new List<UISeasonRewardDisplayInfo>();
		RepeatingRewards = new List<UISeasonRepeatingRewardInfo>();
	}

	public override bool Equals(object obj)
	{
		if (!(obj is UISeasonRewardEntry))
		{
			return false;
		}
		UISeasonRewardEntry uISeasonRewardEntry = (UISeasonRewardEntry)obj;
		int result;
		if (isLevelled == uISeasonRewardEntry.isLevelled)
		{
			if (LevelToGetReward == uISeasonRewardEntry.LevelToGetReward)
			{
				if (isCurrentLevel == uISeasonRewardEntry.isCurrentLevel && isPreviewingLevel == uISeasonRewardEntry.isPreviewingLevel)
				{
					result = ((Rewards.Count == uISeasonRewardEntry.Rewards.Count) ? 1 : 0);
					goto IL_008d;
				}
			}
		}
		result = 0;
		goto IL_008d;
		IL_008d:
		return (byte)result != 0;
	}

	public override int GetHashCode()
	{
		return isLevelled.GetHashCode() ^ LevelToGetReward.GetHashCode() ^ isCurrentLevel.GetHashCode() ^ isPreviewingLevel.GetHashCode();
	}

	public void Clear()
	{
		LevelToGetReward = 0;
		isLevelled = false;
		Rewards.Clear();
	}

	public void Init(int level, bool levelled, List<SeasonReward> rewards, List<UISeasonRepeatingRewardInfo> repeatingRewards, bool activeLevel)
	{
		LevelToGetReward = level;
		isLevelled = levelled;
		isCurrentLevel = activeLevel;
		if (rewards != null)
		{
			for (int i = 0; i < rewards.Count; i++)
			{
				UISeasonRewardDisplayInfo uISeasonRewardDisplayInfo = new UISeasonRewardDisplayInfo();
				uISeasonRewardDisplayInfo.Setup(rewards[i]);
				Rewards.Add(uISeasonRewardDisplayInfo);
			}
		}
		if (repeatingRewards != null)
		{
			for (int j = 0; j < repeatingRewards.Count; j++)
			{
				RepeatingRewards.Add(repeatingRewards[j]);
			}
		}
	}

	public int GetPrefabIndexToDisplay()
	{
		if (Rewards.Count == 0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return 0;
				}
			}
		}
		return 1;
	}

	public void Setup(int displayIndex, _LargeScrollListItemEntry UIEntry)
	{
		UISeasonsRewardEntry component = UIEntry.GetComponent<UISeasonsRewardEntry>();
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			if (isLevelled)
			{
				component.SetAsLevelledup();
			}
			else
			{
				component.SetAsNotLevelled();
			}
			component.DisplayAsCurrentLevel(isCurrentLevel, !isCurrentLevel);
			component.SetPreviewingLevel(isPreviewingLevel);
			component.m_btn.NotifyHoverStatusChange(false, true, 1f);
			component.SetLevelLabelsText(LevelToGetReward.ToString());
			component.SetupReward(Rewards, RepeatingRewards);
			component.DoRewardIconFade(!isLevelled);
			return;
		}
	}
}
