using LobbyGameClientMessages;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerProfileRankDisplay : MonoBehaviour
{
	public TextMeshProUGUI[] m_rankDisplayTitle;

	public TextMeshProUGUI m_DivisionText;

	public TextMeshProUGUI m_TierLevelText;

	public TextMeshProUGUI m_GamesPlayed;

	public TextMeshProUGUI m_PlacementGamesLeftText;

	public Image m_currentRankImage;

	public Image m_rankFillBar;

	private const float MIN_RANK_FILL_AMT = 0.082f;

	private const float MAX_RANK_FILL_AMT = 0.915f;

	public RectTransform m_selectedQueueRankContainer;

	public RectTransform m_InPlacementMatchesContainer;

	public RectTransform m_HasRankAlreadyContainer;

	public static float GetRankFillAmt(float percent)
	{
		return percent * 0.833f + 0.082f;
	}

	private void Awake()
	{
		_SelectableBtn componentInChildren = GetComponentInChildren<_SelectableBtn>(true);
		if (!(componentInChildren != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			componentInChildren.spriteController.SetClickable(false);
			return;
		}
	}

	public void SetRankDisplayText(string text)
	{
		for (int i = 0; i < m_rankDisplayTitle.Length; i++)
		{
			m_rankDisplayTitle[i].text = text;
		}
	}

	public void SetAsActiveQueue(bool isActive)
	{
		UIManager.SetGameObjectActive(m_selectedQueueRankContainer, isActive);
		TextMeshProUGUI[] componentsInChildren = m_InPlacementMatchesContainer.GetComponentsInChildren<TextMeshProUGUI>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (isActive)
			{
				while (true)
				{
					switch (1)
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
				componentsInChildren[i].color = Color.white;
			}
			else
			{
				componentsInChildren[i].color = Color.gray;
			}
		}
		while (true)
		{
			switch (7)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void SetTierIcon(int tier)
	{
		string tierIconResource = UIRankedModeSelectScreen.GetTierIconResource(tier);
		if (!tierIconResource.IsNullOrEmpty())
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
					UIManager.SetGameObjectActive(m_currentRankImage, true);
					m_currentRankImage.sprite = (Sprite)Resources.Load(tierIconResource, typeof(Sprite));
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_currentRankImage, false);
	}

	private void SetBarColor(int tier)
	{
		string tierIconResource = UIRankedModeSelectScreen.GetTierIconResource(tier);
		if (!tierIconResource.IsNullOrEmpty())
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (tierIconResource.ToLower().Contains("bronze"))
					{
						m_rankFillBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Bronze_fill", typeof(Sprite));
					}
					else if (tierIconResource.ToLower().Contains("silver"))
					{
						m_rankFillBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Silver_fill", typeof(Sprite));
					}
					else
					{
						if (tierIconResource.ToLower().Contains("gold"))
						{
							while (true)
							{
								switch (7)
								{
								case 0:
									break;
								default:
									m_rankFillBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Gold_fill", typeof(Sprite));
									return;
								}
							}
						}
						if (tierIconResource.ToLower().Contains("platinum"))
						{
							m_rankFillBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Platinum_fill", typeof(Sprite));
						}
						else
						{
							if (tierIconResource.ToLower().Contains("diamond"))
							{
								while (true)
								{
									switch (1)
									{
									case 0:
										break;
									default:
										m_rankFillBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Diamond_fill", typeof(Sprite));
										return;
									}
								}
							}
							if (tierIconResource.ToLower().Contains("master"))
							{
								while (true)
								{
									switch (2)
									{
									case 0:
										break;
									default:
										m_rankFillBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Master_fill", typeof(Sprite));
										return;
									}
								}
							}
							if (tierIconResource.ToLower().Contains("contender"))
							{
								m_rankFillBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Contender_fill", typeof(Sprite));
							}
						}
					}
					return;
				}
			}
		}
		Log.Warning("Did not find icon for tier: " + tier);
	}

	private int GetGroupSize(UIRankDisplayType type)
	{
		int result = 0;
		if (type == UIRankDisplayType.Solo)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = 1;
			SetRankDisplayText(StringUtil.TR("SOLORANKED", "RankMode"));
		}
		else if (type == UIRankDisplayType.Duo)
		{
			result = 2;
			SetRankDisplayText(StringUtil.TR("DUORANKED", "RankMode"));
		}
		else if (type == UIRankDisplayType.FullTeam)
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
			result = 4;
			SetRankDisplayText(StringUtil.TR("TEAMRANKED", "RankMode"));
		}
		return result;
	}

	public void Setup(UIRankDisplayType type, Dictionary<int, PerGroupSizeTierInfo> tierInfos)
	{
		int groupSize = GetGroupSize(type);
		PerGroupSizeTierInfo perGroupSizeTierInfo = tierInfos[groupSize];
		int gamesPlayed = 0;
		int tier = 0;
		int instanceId = 0;
		float tierPoints = 0f;
		if (perGroupSizeTierInfo.OurEntry.HasValue)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			RankedScoreboardEntry value = perGroupSizeTierInfo.OurEntry.Value;
			tier = value.Tier;
			RankedScoreboardEntry value2 = perGroupSizeTierInfo.OurEntry.Value;
			instanceId = value2.InstanceId;
			RankedScoreboardEntry value3 = perGroupSizeTierInfo.OurEntry.Value;
			gamesPlayed = value3.MatchCount;
			RankedScoreboardEntry value4 = perGroupSizeTierInfo.OurEntry.Value;
			tierPoints = value4.TierPoints;
		}
		else if (ClientGameManager.Get().GameTypeAvailabilies[GameType.Ranked].MinMatchesToAppearOnLeaderboard == 0)
		{
			tier = 25;
			tierPoints = 1f;
			Log.Error("Did not receive Tier Info when there are 0 placement matches!");
		}
		Setup(groupSize, tier, instanceId, gamesPlayed, tierPoints);
	}

	public void Setup(int groupSize, int tier, int instanceId, int gamesPlayed, float tierPoints)
	{
		UIManager.SetGameObjectActive(m_InPlacementMatchesContainer, true);
		UIManager.SetGameObjectActive(m_HasRankAlreadyContainer, false);
		if (groupSize <= 0)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			bool flag = false;
			if (tier > 0)
			{
				flag = true;
				UIManager.SetGameObjectActive(m_InPlacementMatchesContainer, false);
				UIManager.SetGameObjectActive(m_HasRankAlreadyContainer, true);
				UIRankedModeSelectScreen.Get().GetTierLocalizedName(tier, instanceId, groupSize, out string tierName, out string _);
				string text = ((int)(tierPoints + 0.5f)).ToString();
				string str = string.Empty;
				string arg;
				if (tier != 1 && tier != 2)
				{
					str = " / 100";
					arg = text;
				}
				else
				{
					arg = $"{tierPoints:F1}";
				}
				string[] array = tierName.Split(' ');
				if (array.Length > 1)
				{
					m_DivisionText.text = array[0];
					m_TierLevelText.text = (tier - 2).ToString();
					m_rankFillBar.fillAmount = GetRankFillAmt(tierPoints / 100f);
				}
				else
				{
					m_DivisionText.text = tierName;
					m_TierLevelText.text = text;
					m_rankFillBar.fillAmount = 1f;
				}
				SetBarColor(tier);
				if (m_GamesPlayed != null)
				{
					while (true)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						break;
					}
					m_GamesPlayed.text = string.Format(StringUtil.TR("ELONumber", "RankMode"), arg) + str;
				}
				SetTierIcon(tier);
			}
			if (flag)
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				m_DivisionText.text = StringUtil.TR("Placement", "RankMode");
				UIManager.SetGameObjectActive(m_InPlacementMatchesContainer, true);
				UIManager.SetGameObjectActive(m_HasRankAlreadyContainer, false);
				int a = ClientGameManager.Get().GameTypeAvailabilies[GameType.Ranked].MinMatchesToAppearOnLeaderboard - gamesPlayed;
				a = Mathf.Max(a, 0);
				if (m_PlacementGamesLeftText != null)
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
					m_PlacementGamesLeftText.text = string.Format(StringUtil.TR("PlayMoreGames", "RankMode"), a);
				}
				SetTierIcon(0);
				return;
			}
		}
	}
}
