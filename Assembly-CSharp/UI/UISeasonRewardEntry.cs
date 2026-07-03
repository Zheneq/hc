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
        return obj is UISeasonRewardEntry uISeasonRewardEntry
               && isLevelled == uISeasonRewardEntry.isLevelled
               && LevelToGetReward == uISeasonRewardEntry.LevelToGetReward
               && isCurrentLevel == uISeasonRewardEntry.isCurrentLevel
               && isPreviewingLevel == uISeasonRewardEntry.isPreviewingLevel
               && Rewards.Count == uISeasonRewardEntry.Rewards.Count;
    }

    public override int GetHashCode()
    {
        return isLevelled.GetHashCode()
               ^ LevelToGetReward.GetHashCode()
               ^ isCurrentLevel.GetHashCode()
               ^ isPreviewingLevel.GetHashCode();
    }

    public void Clear()
    {
        LevelToGetReward = 0;
        isLevelled = false;
        Rewards.Clear();
    }

    public void Init(
        int level,
        bool levelled,
        List<SeasonReward> rewards,
        List<UISeasonRepeatingRewardInfo> repeatingRewards,
        bool activeLevel)
    {
        LevelToGetReward = level;
        isLevelled = levelled;
        isCurrentLevel = activeLevel;
        if (rewards != null)
        {
            foreach (SeasonReward reward in rewards)
            {
                UISeasonRewardDisplayInfo uISeasonRewardDisplayInfo = new UISeasonRewardDisplayInfo();
                uISeasonRewardDisplayInfo.Setup(reward);
                Rewards.Add(uISeasonRewardDisplayInfo);
            }
        }

        if (repeatingRewards != null)
        {
            foreach (UISeasonRepeatingRewardInfo reward in repeatingRewards)
            {
                RepeatingRewards.Add(reward);
            }
        }
    }

    public int GetPrefabIndexToDisplay()
    {
        return Rewards.Count == 0 ? 0 : 1;
    }

    public void Setup(int displayIndex, _LargeScrollListItemEntry UIEntry)
    {
        UISeasonsRewardEntry component = UIEntry.GetComponent<UISeasonsRewardEntry>();
        if (component == null)
        {
            return;
        }

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
    }

#if EVOS
    public string GetSortingKey()
    {
        return LevelToGetReward.ToString("D10");
    }
#endif
}