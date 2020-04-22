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
		InPlacement = (entry.Tier < 1);
		int inMasterOrContender;
		if (entry.Tier != 1)
		{
			while (true)
			{
				switch (3)
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
			inMasterOrContender = ((entry.Tier == 2) ? 1 : 0);
		}
		else
		{
			inMasterOrContender = 1;
		}
		InMasterOrContender = ((byte)inMasterOrContender != 0);
		UIRankedModeSelectScreen.Get().GetTierLocalizedName(entry.Tier, entry.InstanceId, groupSize, out TierName, out InstanceName);
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
		if (entry.Tier != -1)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (entry.YesterdaysTier != -1)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				if (entry.TierPoints >= 0f)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (entry.YesterdaysPoints >= 0)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (t < rankedLeaderboardExpirationTime)
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
								{
									int num = (int)(entry.TierPoints + 0.5f) - entry.YesterdaysPoints;
									int num2 = Math.Max(entry.YesterdaysTier, 2) - Math.Max(entry.Tier, 2);
									Change = num + 100 * num2;
									return;
								}
								}
							}
						}
					}
				}
			}
		}
		Change = 0;
	}

	public int GetPrefabIndexToDisplay()
	{
		return 0;
	}

	public void Setup(int displayIndex, _LargeScrollListItemEntry UIEntry)
	{
		UIRankListDisplayEntry component = UIEntry.GetComponent<UIRankListDisplayEntry>();
		if (!(component != null))
		{
			return;
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
			TimeSpan difference = ClientGameManager.Get().UtcNow() - LastMatch;
			string timeDifferenceText = StringUtil.GetTimeDifferenceText(difference);
			timeDifferenceText = string.Format(StringUtil.TR("MatchTimeDifference", "Global"), timeDifferenceText);
			component.LastMatchText.text = timeDifferenceText;
			if (StreakLength > 0)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				component.StreakText.text = StreakLength.ToString();
			}
			else
			{
				component.StreakText.text = "-";
			}
			component.TotalMatchesText.text = NumMatches.ToString();
			component.NameText.text = PlayerDisplayName;
			component.DivisionText.text = TierName;
			component.AccountHandle = PlayerDisplayName;
			component.AccountID = AccountID;
			UIManager.SetGameObjectActive(component.m_selfHighlight, AccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId);
			if (TierPoints < 0f)
			{
				component.RankText.text = StringUtil.TR("Unranked", "RankMode");
				UIManager.SetGameObjectActive(component.ChangeText, false);
			}
			else
			{
				if (InMasterOrContender)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					component.RankText.text = StringUtil.GetLocalizedFloat(TierPoints, "####.#");
				}
				else
				{
					component.RankText.text = ((int)(TierPoints + 0.5f)).ToString();
				}
				if (Change > 0)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
					if (difference.TotalDays < 1.0)
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
						component.ChangeText.text = $"<sprite={6}>{Change}";
					}
					else
					{
						component.ChangeText.text = $"<sprite={8}>{Change}";
					}
				}
				else if (Change < 0)
				{
					while (true)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					if (difference.TotalDays < 1.0)
					{
						component.ChangeText.text = $"<sprite={7}>{Change}";
					}
					else
					{
						component.ChangeText.text = $"<sprite={9}>{Change}";
					}
				}
				UIManager.SetGameObjectActive(component.ChangeText, Change != 0);
			}
			ScrollRect componentInParent = component.GetComponentInParent<ScrollRect>();
			if (!(componentInParent != null))
			{
				return;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (component.m_theBtn.spriteController.gameObject.GetComponent<_MouseEventPasser>() == null)
				{
					_MouseEventPasser mouseEventPasser = component.m_theBtn.spriteController.gameObject.AddComponent<_MouseEventPasser>();
					mouseEventPasser.AddNewHandler(componentInParent);
				}
				return;
			}
		}
	}
}
