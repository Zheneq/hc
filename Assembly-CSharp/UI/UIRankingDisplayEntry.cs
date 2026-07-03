using LobbyGameClientMessages;
using System;
using UnityEngine.UI;

public class UIRankingDisplayEntry : IDataEntry
{
    public string TierName;
    public string InstanceName;
    public string PlayerDisplayName;
    public string StreakType;
    public int StreakLength;
    public int NumWins;
    public int NumMatches;
    public float TierPoints;
    public int GroupSize;
    public long AccountID;
    public DateTime LastMatch;
    public bool InPlacement;
    public bool InMasterOrContender;
    public int Change;

    public UIRankingDisplayEntry(RankedScoreboardEntry entry, int groupSize)
    {
        InPlacement = entry.Tier < 1;
        InMasterOrContender = entry.Tier == 1 || entry.Tier == 2;
        UIRankedModeSelectScreen.Get().GetTierLocalizedName(
            entry.Tier,
            entry.InstanceId,
            groupSize,
            out TierName,
            out InstanceName);
        LastMatch = entry.LastMatch;
        PlayerDisplayName = entry.Handle;
        AccountID = entry.AccountID;
        StreakType = StringUtil.TR("WinStreak", "Global");
        StreakLength = entry.WinStreak;
        NumWins = entry.WinCount;
        NumMatches = entry.MatchCount;
        TierPoints = entry.TierPoints;
        GroupSize = groupSize;
        TimeSpan t = ClientGameManager.Get().UtcNow() - LastMatch;
        TimeSpan rankedLeaderboardExpirationTime = GameManager.Get().GameplayOverrides.RankedLeaderboardExpirationTime;
        if (entry.Tier != -1
            && entry.YesterdaysTier != -1
            && entry.TierPoints >= 0f
            && entry.YesterdaysPoints >= 0
            && t < rankedLeaderboardExpirationTime)
        {
            int pointChange = (int)(entry.TierPoints + 0.5f) - entry.YesterdaysPoints;
            int tierChange = Math.Max(entry.YesterdaysTier, 2) - Math.Max(entry.Tier, 2);
            Change = pointChange + 100 * tierChange;
        }
        else
        {
            Change = 0;
        }
    }

    public int GetPrefabIndexToDisplay()
    {
        return 0;
    }

    public void Setup(int displayIndex, _LargeScrollListItemEntry UIEntry)
    {
        UIRankListDisplayEntry component = UIEntry.GetComponent<UIRankListDisplayEntry>();
        if (component == null)
        {
            return;
        }

        TimeSpan difference = ClientGameManager.Get().UtcNow() - LastMatch;
        string timeDifferenceText = StringUtil.GetTimeDifferenceText(difference);
        timeDifferenceText = string.Format(StringUtil.TR("MatchTimeDifference", "Global"), timeDifferenceText);
        component.LastMatchText.text = timeDifferenceText;
        component.StreakText.text = StreakLength > 0 ? StreakLength.ToString() : "-";
        component.TotalMatchesText.text = NumMatches.ToString();
        component.NameText.text = PlayerDisplayName;
        component.DivisionText.text = TierName;
        component.AccountHandle = PlayerDisplayName;
        component.AccountID = AccountID;
        UIManager.SetGameObjectActive(
            component.m_selfHighlight,
            AccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId);
        if (TierPoints < 0f)
        {
            component.RankText.text = StringUtil.TR("Unranked", "RankMode");
            UIManager.SetGameObjectActive(component.ChangeText, false);
        }
        else
        {
            component.RankText.text = InMasterOrContender
                ? StringUtil.GetLocalizedFloat(TierPoints, "####.#")
                : ((int)(TierPoints + 0.5f)).ToString();
            if (Change > 0)
            {
                int sprite = difference.TotalDays < 1.0 ? 6 : 8;
                component.ChangeText.text = $"<sprite={sprite}>{Change}";
            }
            else if (Change < 0)
            {
                int sprite = difference.TotalDays < 1.0 ? 7 : 9;
                component.ChangeText.text = $"<sprite={sprite}>{Change}";
            }

            UIManager.SetGameObjectActive(component.ChangeText, Change != 0);
        }

        ScrollRect componentInParent = component.GetComponentInParent<ScrollRect>();
        if (componentInParent != null
            && component.m_theBtn.spriteController.gameObject.GetComponent<_MouseEventPasser>() == null)
        {
            _MouseEventPasser mouseEventPasser =
                component.m_theBtn.spriteController.gameObject.AddComponent<_MouseEventPasser>();
            mouseEventPasser.AddNewHandler(componentInParent);
        }
    }

#if EVOS
    public string GetSortingKey()
    {
        return TierPoints.ToString("D10");
    }
#endif
}