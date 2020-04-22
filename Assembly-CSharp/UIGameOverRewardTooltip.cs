using LobbyGameClientMessages;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIGameOverRewardTooltip : UITooltipBase
{
	public enum RewardTooltipType
	{
		AccountInfo,
		CharacterInfo,
		ISOAmount,
		RankedPointsAmount,
		FactionInfo,
		FreelancerCurrencyAmount
	}

	[Header("Labels")]
	public TextMeshProUGUI m_baseTypeLabel;

	public TextMeshProUGUI m_consumableXPLabel;

	public TextMeshProUGUI m_totalTypeLabel;

	public TextMeshProUGUI m_levelUpLabel;

	public TextMeshProUGUI m_winAmountLabel;

	public TextMeshProUGUI m_ggAmountLabel;

	public TextMeshProUGUI m_partyAmountLabel;

	public TextMeshProUGUI m_skinAmountLabel;

	public TextMeshProUGUI m_bannerAmountLabel;

	public TextMeshProUGUI m_emblemAmountLabel;

	public TextMeshProUGUI m_questAmountLabel;

	public TextMeshProUGUI m_freelancerOwnedLabel;

	public TextMeshProUGUI m_eventBonusLabel;

	public TextMeshProUGUI m_queueAmountLabel;

	[Header("Numbers")]
	public TextMeshProUGUI m_baseAmountGain;

	public TextMeshProUGUI m_consumableXPGain;

	public TextMeshProUGUI m_winAmountGain;

	public TextMeshProUGUI m_ggAmountGain;

	public TextMeshProUGUI m_partyAmountGain;

	public TextMeshProUGUI m_questAmountGain;

	public TextMeshProUGUI m_levelUpAmountGain;

	public TextMeshProUGUI m_skinAmountGain;

	public TextMeshProUGUI m_bannerAmountGain;

	public TextMeshProUGUI m_emblemAmountGain;

	public TextMeshProUGUI m_freelancerOwnedGain;

	public TextMeshProUGUI m_totalAmountGain;

	public TextMeshProUGUI m_eventBonusGain;

	public TextMeshProUGUI m_queueAmountGain;

	private bool m_freelancerExpBonus;

	public void Setup(int normalCurrencyGain, int ggCurrencyGain, int winCurrencyGain, int questCurrencyGain, int levelUpGain, int eventGain, RewardTooltipType tooltipType)
	{
		if (tooltipType == RewardTooltipType.RankedPointsAmount)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_baseTypeLabel.text = StringUtil.TR("BaseRankedPoints", "GameOver");
			m_totalTypeLabel.text = StringUtil.TR("TotalRankedPoints", "GameOver");
		}
		else if (tooltipType == RewardTooltipType.ISOAmount)
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
			m_baseTypeLabel.text = StringUtil.TR("BaseISO", "GameOver");
			m_totalTypeLabel.text = StringUtil.TR("TotalISO", "GameOver");
		}
		else if (tooltipType == RewardTooltipType.FreelancerCurrencyAmount)
		{
			m_baseTypeLabel.text = StringUtil.TR("BaseFreelancerCurrency", "GameOver");
			m_totalTypeLabel.text = StringUtil.TR("TotalFreelancerCurrency", "GameOver");
		}
		m_baseAmountGain.text = normalCurrencyGain.ToString();
		m_consumableXPGain.text = "0";
		m_freelancerExpBonus = false;
		m_ggAmountGain.text = ggCurrencyGain.ToString();
		m_winAmountGain.text = winCurrencyGain.ToString();
		m_partyAmountGain.text = "0";
		m_levelUpAmountGain.text = levelUpGain.ToString();
		m_questAmountGain.text = questCurrencyGain.ToString();
		m_queueAmountGain.text = "0";
		m_eventBonusGain.text = eventGain.ToString();
		m_totalAmountGain.text = (normalCurrencyGain + ggCurrencyGain + winCurrencyGain + questCurrencyGain + levelUpGain + eventGain).ToString();
		UpdateDisplayLabels(tooltipType);
	}

	public void Setup(UIGameOverScreen.XPDisplayInfo displayInfo, RewardTooltipType tooltipType, int numEarners)
	{
		m_baseTypeLabel.text = StringUtil.TR("BaseExp", "GameOver");
		m_totalTypeLabel.text = StringUtil.TR("TotalExp", "GameOver");
		MatchResultsNotification results = UIGameOverScreen.Get().Results;
		if (results == null)
		{
			m_baseAmountGain.text = "0";
			m_consumableXPGain.text = "0";
			m_freelancerExpBonus = false;
			m_winAmountGain.text = "0";
			m_ggAmountGain.text = "0";
			m_partyAmountGain.text = "0";
			m_questAmountGain.text = "0";
			m_levelUpAmountGain.text = "0";
			m_freelancerOwnedGain.text = "0";
			m_eventBonusGain.text = "0";
			m_queueAmountGain.text = "0";
			m_totalAmountGain.text = "0";
		}
		else
		{
			m_freelancerExpBonus = (results.ConsumableXpGained > 0);
			int num = 0;
			if (tooltipType == RewardTooltipType.CharacterInfo)
			{
				num = results.ConsumableXpGained;
			}
			int num2 = results.BaseXpGained + results.WinXpGained + results.GGXpGained + results.PlayWithFriendXpGained + results.QuestXpGained + results.FreelancerOwnedXPGained + results.EventBonusXpGained + results.QueueTimeXpGained + num;
			if (numEarners > 1)
			{
				int splitTotal = num2 / numEarners;
				m_baseAmountGain.text = GetSplitExpAmount(results.BaseXpGained, num2, splitTotal);
				m_consumableXPGain.text = GetSplitExpAmount(results.ConsumableXpGained, num2, splitTotal);
				m_winAmountGain.text = GetSplitExpAmount(results.WinXpGained, num2, splitTotal);
				m_ggAmountGain.text = GetSplitExpAmount(results.GGXpGained, num2, splitTotal);
				m_partyAmountGain.text = GetSplitExpAmount(results.PlayWithFriendXpGained, num2, splitTotal);
				m_questAmountGain.text = GetSplitExpAmount(results.QuestXpGained, num2, splitTotal);
				m_levelUpAmountGain.text = "0";
				m_freelancerOwnedGain.text = GetSplitExpAmount(results.FreelancerOwnedXPGained, num2, splitTotal);
				m_eventBonusGain.text = GetSplitExpAmount(results.EventBonusXpGained, num2, splitTotal);
				m_queueAmountGain.text = GetSplitExpAmount(results.QueueTimeXpGained, num2, splitTotal);
				m_totalAmountGain.text = splitTotal.ToString();
			}
			else
			{
				m_baseAmountGain.text = results.BaseXpGained.ToString();
				m_consumableXPGain.text = results.ConsumableXpGained.ToString();
				m_winAmountGain.text = results.WinXpGained.ToString();
				m_ggAmountGain.text = results.GGXpGained.ToString();
				m_partyAmountGain.text = results.PlayWithFriendXpGained.ToString();
				m_questAmountGain.text = results.QuestXpGained.ToString();
				m_levelUpAmountGain.text = "0";
				m_freelancerOwnedGain.text = results.FreelancerOwnedXPGained.ToString();
				m_eventBonusGain.text = results.EventBonusXpGained.ToString();
				m_queueAmountGain.text = results.QueueTimeXpGained.ToString();
				m_totalAmountGain.text = num2.ToString();
			}
		}
		UpdateDisplayLabels(tooltipType);
	}

	private string GetSplitExpAmount(int normalExp, int normalTotal, int splitTotal)
	{
		float num = splitTotal;
		return (num * ((float)normalExp / (float)normalTotal)).ToString("0.##");
	}

	public void Setup(UIGameOverPanel.XPDisplayInfo displayInfo, RewardTooltipType tooltipType)
	{
		m_baseTypeLabel.text = StringUtil.TR("BaseExp", "GameOver");
		m_totalTypeLabel.text = StringUtil.TR("TotalExp", "GameOver");
		m_baseAmountGain.text = (displayInfo.m_normalXPInitial - displayInfo.m_queueTimeXpInitial).ToString();
		m_consumableXPGain.text = "0";
		m_freelancerExpBonus = false;
		m_winAmountGain.text = displayInfo.m_winBonusXPInitial.ToString();
		m_ggAmountGain.text = displayInfo.m_ggPackXPInitial.ToString();
		m_partyAmountGain.text = displayInfo.m_playWithFriendXpInitial.ToString();
		m_questAmountGain.text = displayInfo.m_questXpInitial.ToString();
		m_levelUpAmountGain.text = "0";
		m_freelancerOwnedGain.text = displayInfo.m_freelancerOwnedXpInitial.ToString();
		m_eventBonusGain.text = displayInfo.m_eventBonusXpInitial.ToString();
		m_queueAmountGain.text = displayInfo.m_queueTimeXpInitial.ToString();
		m_totalAmountGain.text = displayInfo.GetTotalExpInitial().ToString();
		UpdateDisplayLabels(tooltipType);
	}

	public void Setup(Dictionary<string, int> factionInfo, RewardTooltipType tooltipType)
	{
		m_baseTypeLabel.text = StringUtil.TR("FactionContributionMatch", "TrustWar");
		factionInfo.TryGetValue("Match", out int value);
		factionInfo.TryGetValue("Win", out int value2);
		factionInfo.TryGetValue("Banner", out int value3);
		factionInfo.TryGetValue("Emblem", out int value4);
		factionInfo.TryGetValue("Skin", out int value5);
		m_baseAmountGain.text = value.ToString();
		m_winAmountGain.text = value2.ToString();
		m_skinAmountGain.text = value5.ToString();
		m_bannerAmountGain.text = value3.ToString();
		m_emblemAmountGain.text = value4.ToString();
		UpdateDisplayLabels(tooltipType);
	}

	private void UpdateDisplayLabels(RewardTooltipType tooltipType)
	{
		bool doActive = true;
		bool flag = true;
		bool doActive2 = true;
		bool doActive3 = true;
		bool doActive4 = true;
		bool doActive5 = true;
		bool doActive6 = true;
		bool doActive7 = true;
		bool doActive8 = true;
		bool doActive9 = true;
		bool doActive10 = true;
		bool flag2 = true;
		bool doActive11 = true;
		bool doActive12 = true;
		if (tooltipType == RewardTooltipType.AccountInfo)
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
			flag = false;
			doActive3 = false;
			doActive8 = false;
			doActive9 = false;
			doActive10 = false;
		}
		else if (tooltipType == RewardTooltipType.CharacterInfo)
		{
			doActive7 = false;
			doActive8 = false;
			doActive9 = false;
			doActive10 = false;
		}
		else if (tooltipType == RewardTooltipType.RankedPointsAmount)
		{
			flag = false;
			doActive3 = false;
			doActive5 = false;
			doActive6 = false;
			doActive7 = false;
			doActive8 = false;
			doActive9 = false;
			doActive10 = false;
			flag2 = false;
			doActive11 = false;
			doActive12 = false;
		}
		else
		{
			if (tooltipType != RewardTooltipType.ISOAmount)
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
				if (tooltipType != RewardTooltipType.FreelancerCurrencyAmount)
				{
					if (tooltipType == RewardTooltipType.FactionInfo)
					{
						flag = false;
						doActive2 = false;
						doActive3 = false;
						doActive5 = false;
						doActive6 = false;
						doActive7 = false;
						flag2 = false;
						doActive11 = false;
						doActive12 = false;
					}
					goto IL_00c9;
				}
			}
			flag = false;
			doActive6 = false;
			doActive8 = false;
			doActive9 = false;
			doActive10 = false;
			flag2 = false;
			doActive12 = false;
		}
		goto IL_00c9;
		IL_00c9:
		UIManager.SetGameObjectActive(m_baseTypeLabel, doActive);
		TextMeshProUGUI consumableXPLabel = m_consumableXPLabel;
		int doActive13;
		if (flag)
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
			doActive13 = (m_freelancerExpBonus ? 1 : 0);
		}
		else
		{
			doActive13 = 0;
		}
		UIManager.SetGameObjectActive(consumableXPLabel, (byte)doActive13 != 0);
		UIManager.SetGameObjectActive(m_partyAmountLabel, doActive6);
		UIManager.SetGameObjectActive(m_ggAmountLabel, doActive5);
		UIManager.SetGameObjectActive(m_winAmountLabel, doActive4);
		UIManager.SetGameObjectActive(m_totalTypeLabel, doActive2);
		UIManager.SetGameObjectActive(m_questAmountLabel, doActive7);
		UIManager.SetGameObjectActive(m_levelUpLabel, doActive3);
		UIManager.SetGameObjectActive(m_skinAmountLabel, doActive8);
		UIManager.SetGameObjectActive(m_bannerAmountLabel, doActive9);
		UIManager.SetGameObjectActive(m_emblemAmountLabel, doActive10);
		UIManager.SetGameObjectActive(m_freelancerOwnedLabel, flag2);
		UIManager.SetGameObjectActive(m_eventBonusLabel, doActive11);
		UIManager.SetGameObjectActive(m_queueAmountLabel, doActive12);
		m_totalTypeLabel.transform.SetAsLastSibling();
		if (!flag2)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		bool hasPurchasedGame = ClientGameManager.Get().HasPurchasedGame;
		for (int i = 0; i < GameWideData.Get().m_characterResourceLinks.Length; i++)
		{
			CharacterResourceLink characterResourceLink = GameWideData.Get().m_characterResourceLinks[i];
			if (characterResourceLink == null || characterResourceLink.m_characterType.IsWillFill() || characterResourceLink.m_isHidden)
			{
				continue;
			}
			if (!characterResourceLink.m_allowForPlayers)
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
				continue;
			}
			num2++;
			if (!hasPurchasedGame)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!ClientGameManager.Get().GetPlayerCharacterData(characterResourceLink.m_characterType).CharacterComponent.Unlocked)
				{
					continue;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			num++;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			m_freelancerOwnedLabel.text = string.Format(StringUtil.TR("FreelancersOwned", "HUDScene"), num, num2);
			return;
		}
	}

	public void Setup(string title, string description, RectTransform guiElement, RewardTooltipType tooltipType)
	{
		m_baseTypeLabel.text = title;
		m_totalTypeLabel.text = description;
		m_baseAmountGain.text = string.Empty;
		m_consumableXPGain.text = string.Empty;
		m_freelancerExpBonus = false;
		m_winAmountGain.text = string.Empty;
		m_ggAmountGain.text = string.Empty;
		m_partyAmountGain.text = string.Empty;
		m_questAmountGain.text = string.Empty;
		m_levelUpAmountGain.text = string.Empty;
		m_totalAmountGain.text = string.Empty;
		m_skinAmountGain.text = string.Empty;
		m_bannerAmountGain.text = string.Empty;
		m_emblemAmountGain.text = string.Empty;
		m_freelancerOwnedGain.text = string.Empty;
		m_eventBonusGain.text = string.Empty;
		m_queueAmountGain.text = string.Empty;
		UpdateDisplayLabels(tooltipType);
	}
}
