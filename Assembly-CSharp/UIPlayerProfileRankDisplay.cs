using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
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
		_SelectableBtn componentInChildren = base.GetComponentInChildren<_SelectableBtn>(true);
		if (componentInChildren != null)
		{
			componentInChildren.spriteController.SetClickable(false);
		}
	}

	public void SetRankDisplayText(string text)
	{
		for (int i = 0; i < this.m_rankDisplayTitle.Length; i++)
		{
			this.m_rankDisplayTitle[i].text = text;
		}
	}

	public void SetAsActiveQueue(bool isActive)
	{
		UIManager.SetGameObjectActive(this.m_selectedQueueRankContainer, isActive, null);
		TextMeshProUGUI[] componentsInChildren = this.m_InPlacementMatchesContainer.GetComponentsInChildren<TextMeshProUGUI>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (isActive)
			{
				componentsInChildren[i].color = Color.white;
			}
			else
			{
				componentsInChildren[i].color = Color.gray;
			}
		}
	}

	private void SetTierIcon(int tier)
	{
		string tierIconResource = UIRankedModeSelectScreen.GetTierIconResource(tier);
		if (!tierIconResource.IsNullOrEmpty())
		{
			UIManager.SetGameObjectActive(this.m_currentRankImage, true, null);
			this.m_currentRankImage.sprite = (Sprite)Resources.Load(tierIconResource, typeof(Sprite));
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_currentRankImage, false, null);
		}
	}

	private void SetBarColor(int tier)
	{
		string tierIconResource = UIRankedModeSelectScreen.GetTierIconResource(tier);
		if (!tierIconResource.IsNullOrEmpty())
		{
			if (tierIconResource.ToLower().Contains("bronze"))
			{
				this.m_rankFillBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Bronze_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("silver"))
			{
				this.m_rankFillBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Silver_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("gold"))
			{
				this.m_rankFillBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Gold_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("platinum"))
			{
				this.m_rankFillBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Platinum_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("diamond"))
			{
				this.m_rankFillBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Diamond_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("master"))
			{
				this.m_rankFillBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Master_fill", typeof(Sprite));
			}
			else if (tierIconResource.ToLower().Contains("contender"))
			{
				this.m_rankFillBar.sprite = (Sprite)Resources.Load("RankFillBars/ranked_Contender_fill", typeof(Sprite));
			}
		}
		else
		{
			Log.Warning("Did not find icon for tier: " + tier, new object[0]);
		}
	}

	private int GetGroupSize(UIRankDisplayType type)
	{
		int result = 0;
		if (type == UIRankDisplayType.Solo)
		{
			result = 1;
			this.SetRankDisplayText(StringUtil.TR("SOLORANKED", "RankMode"));
		}
		else if (type == UIRankDisplayType.Duo)
		{
			result = 2;
			this.SetRankDisplayText(StringUtil.TR("DUORANKED", "RankMode"));
		}
		else if (type == UIRankDisplayType.FullTeam)
		{
			result = 4;
			this.SetRankDisplayText(StringUtil.TR("TEAMRANKED", "RankMode"));
		}
		return result;
	}

	public void Setup(UIRankDisplayType type, Dictionary<int, PerGroupSizeTierInfo> tierInfos)
	{
		int groupSize = this.GetGroupSize(type);
		PerGroupSizeTierInfo perGroupSizeTierInfo = tierInfos[groupSize];
		int gamesPlayed = 0;
		int tier = 0;
		int instanceId = 0;
		float tierPoints = 0f;
		if (perGroupSizeTierInfo.OurEntry != null)
		{
			tier = perGroupSizeTierInfo.OurEntry.Value.Tier;
			instanceId = perGroupSizeTierInfo.OurEntry.Value.InstanceId;
			gamesPlayed = perGroupSizeTierInfo.OurEntry.Value.MatchCount;
			tierPoints = perGroupSizeTierInfo.OurEntry.Value.TierPoints;
		}
		else if (ClientGameManager.Get().GameTypeAvailabilies[GameType.Ranked].MinMatchesToAppearOnLeaderboard == 0)
		{
			tier = 0x19;
			tierPoints = 1f;
			Log.Error("Did not receive Tier Info when there are 0 placement matches!", new object[0]);
		}
		this.Setup(groupSize, tier, instanceId, gamesPlayed, tierPoints);
	}

	public void Setup(int groupSize, int tier, int instanceId, int gamesPlayed, float tierPoints)
	{
		UIManager.SetGameObjectActive(this.m_InPlacementMatchesContainer, true, null);
		UIManager.SetGameObjectActive(this.m_HasRankAlreadyContainer, false, null);
		if (groupSize > 0)
		{
			bool flag = false;
			if (tier > 0)
			{
				flag = true;
				UIManager.SetGameObjectActive(this.m_InPlacementMatchesContainer, false, null);
				UIManager.SetGameObjectActive(this.m_HasRankAlreadyContainer, true, null);
				string text;
				string text2;
				UIRankedModeSelectScreen.Get().GetTierLocalizedName(tier, instanceId, groupSize, out text, out text2);
				string text3 = ((int)(tierPoints + 0.5f)).ToString();
				string str = string.Empty;
				string arg;
				if (tier != 1 && tier != 2)
				{
					str = " / 100";
					arg = text3;
				}
				else
				{
					arg = string.Format("{0:F1}", tierPoints);
				}
				string[] array = text.Split(new char[]
				{
					' '
				});
				if (array.Length > 1)
				{
					this.m_DivisionText.text = array[0];
					this.m_TierLevelText.text = (tier - 2).ToString();
					this.m_rankFillBar.fillAmount = UIPlayerProfileRankDisplay.GetRankFillAmt(tierPoints / 100f);
				}
				else
				{
					this.m_DivisionText.text = text;
					this.m_TierLevelText.text = text3;
					this.m_rankFillBar.fillAmount = 1f;
				}
				this.SetBarColor(tier);
				if (this.m_GamesPlayed != null)
				{
					this.m_GamesPlayed.text = string.Format(StringUtil.TR("ELONumber", "RankMode"), arg) + str;
				}
				this.SetTierIcon(tier);
			}
			if (!flag)
			{
				this.m_DivisionText.text = StringUtil.TR("Placement", "RankMode");
				UIManager.SetGameObjectActive(this.m_InPlacementMatchesContainer, true, null);
				UIManager.SetGameObjectActive(this.m_HasRankAlreadyContainer, false, null);
				int num = ClientGameManager.Get().GameTypeAvailabilies[GameType.Ranked].MinMatchesToAppearOnLeaderboard - gamesPlayed;
				num = Mathf.Max(num, 0);
				if (this.m_PlacementGamesLeftText != null)
				{
					this.m_PlacementGamesLeftText.text = string.Format(StringUtil.TR("PlayMoreGames", "RankMode"), num);
				}
				this.SetTierIcon(0);
			}
		}
	}
}
