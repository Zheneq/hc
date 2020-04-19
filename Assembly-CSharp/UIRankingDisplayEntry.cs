using System;
using LobbyGameClientMessages;
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
		this.InPlacement = (entry.Tier < 1);
		bool inMasterOrContender;
		if (entry.Tier != 1)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankingDisplayEntry..ctor(RankedScoreboardEntry, int)).MethodHandle;
			}
			inMasterOrContender = (entry.Tier == 2);
		}
		else
		{
			inMasterOrContender = true;
		}
		this.InMasterOrContender = inMasterOrContender;
		UIRankedModeSelectScreen.Get().GetTierLocalizedName(entry.Tier, entry.InstanceId, groupSize, out this.TierName, out this.InstanceName);
		this.LastMatch = entry.LastMatch;
		this.PlayerDisplayName = entry.Handle;
		this.AccountID = entry.AccountID;
		this.StreakType = StringUtil.TR("WinStreak", "Global");
		this.StreakLength = entry.WinStreak;
		this.NumWins = entry.WinCount;
		this.NumMatches = entry.MatchCount;
		this.TierPoints = entry.TierPoints;
		this.GroupSize = groupSize;
		TimeSpan t = ClientGameManager.Get().UtcNow() - this.LastMatch;
		TimeSpan rankedLeaderboardExpirationTime = GameManager.Get().GameplayOverrides.RankedLeaderboardExpirationTime;
		if (entry.Tier != -1)
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
			if (entry.YesterdaysTier != -1)
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
				if (entry.TierPoints >= 0f)
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
					if (entry.YesterdaysPoints >= 0)
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
						if (t < rankedLeaderboardExpirationTime)
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
							int num = (int)(entry.TierPoints + 0.5f) - entry.YesterdaysPoints;
							int num2 = Math.Max(entry.YesterdaysTier, 2) - Math.Max(entry.Tier, 2);
							this.Change = num + 0x64 * num2;
							return;
						}
					}
				}
			}
		}
		this.Change = 0;
	}

	public int GetPrefabIndexToDisplay()
	{
		return 0;
	}

	public void Setup(int displayIndex, _LargeScrollListItemEntry UIEntry)
	{
		UIRankListDisplayEntry component = UIEntry.GetComponent<UIRankListDisplayEntry>();
		if (component != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIRankingDisplayEntry.Setup(int, _LargeScrollListItemEntry)).MethodHandle;
			}
			TimeSpan difference = ClientGameManager.Get().UtcNow() - this.LastMatch;
			string text = StringUtil.GetTimeDifferenceText(difference, false);
			text = string.Format(StringUtil.TR("MatchTimeDifference", "Global"), text);
			component.LastMatchText.text = text;
			if (this.StreakLength > 0)
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
				component.StreakText.text = this.StreakLength.ToString();
			}
			else
			{
				component.StreakText.text = "-";
			}
			component.TotalMatchesText.text = this.NumMatches.ToString();
			component.NameText.text = this.PlayerDisplayName;
			component.DivisionText.text = this.TierName;
			component.AccountHandle = this.PlayerDisplayName;
			component.AccountID = this.AccountID;
			UIManager.SetGameObjectActive(component.m_selfHighlight, this.AccountID == ClientGameManager.Get().GetPlayerAccountData().AccountId, null);
			if (this.TierPoints < 0f)
			{
				component.RankText.text = StringUtil.TR("Unranked", "RankMode");
				UIManager.SetGameObjectActive(component.ChangeText, false, null);
			}
			else
			{
				if (this.InMasterOrContender)
				{
					for (;;)
					{
						switch (2)
						{
						case 0:
							continue;
						}
						break;
					}
					component.RankText.text = StringUtil.GetLocalizedFloat(this.TierPoints, "####.#");
				}
				else
				{
					component.RankText.text = ((int)(this.TierPoints + 0.5f)).ToString();
				}
				if (this.Change > 0)
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
					if (difference.TotalDays < 1.0)
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
						component.ChangeText.text = string.Format("<sprite={0}>{1}", 6, this.Change);
					}
					else
					{
						component.ChangeText.text = string.Format("<sprite={0}>{1}", 8, this.Change);
					}
				}
				else if (this.Change < 0)
				{
					for (;;)
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
						component.ChangeText.text = string.Format("<sprite={0}>{1}", 7, this.Change);
					}
					else
					{
						component.ChangeText.text = string.Format("<sprite={0}>{1}", 9, this.Change);
					}
				}
				UIManager.SetGameObjectActive(component.ChangeText, this.Change != 0, null);
			}
			ScrollRect componentInParent = component.GetComponentInParent<ScrollRect>();
			if (componentInParent != null)
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
				if (component.m_theBtn.spriteController.gameObject.GetComponent<_MouseEventPasser>() == null)
				{
					_MouseEventPasser mouseEventPasser = component.m_theBtn.spriteController.gameObject.AddComponent<_MouseEventPasser>();
					mouseEventPasser.AddNewHandler(componentInParent);
				}
			}
		}
	}
}
