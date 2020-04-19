using System;
using System.Collections.Generic;
using LobbyGameClientMessages;
using TMPro;
using UnityEngine;

public class UIGameOverRewardTooltip : UITooltipBase
{
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

	public void Setup(int normalCurrencyGain, int ggCurrencyGain, int winCurrencyGain, int questCurrencyGain, int levelUpGain, int eventGain, UIGameOverRewardTooltip.RewardTooltipType tooltipType)
	{
		if (tooltipType == UIGameOverRewardTooltip.RewardTooltipType.RankedPointsAmount)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverRewardTooltip.Setup(int, int, int, int, int, int, UIGameOverRewardTooltip.RewardTooltipType)).MethodHandle;
			}
			this.m_baseTypeLabel.text = StringUtil.TR("BaseRankedPoints", "GameOver");
			this.m_totalTypeLabel.text = StringUtil.TR("TotalRankedPoints", "GameOver");
		}
		else if (tooltipType == UIGameOverRewardTooltip.RewardTooltipType.ISOAmount)
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
			this.m_baseTypeLabel.text = StringUtil.TR("BaseISO", "GameOver");
			this.m_totalTypeLabel.text = StringUtil.TR("TotalISO", "GameOver");
		}
		else if (tooltipType == UIGameOverRewardTooltip.RewardTooltipType.FreelancerCurrencyAmount)
		{
			this.m_baseTypeLabel.text = StringUtil.TR("BaseFreelancerCurrency", "GameOver");
			this.m_totalTypeLabel.text = StringUtil.TR("TotalFreelancerCurrency", "GameOver");
		}
		this.m_baseAmountGain.text = normalCurrencyGain.ToString();
		this.m_consumableXPGain.text = "0";
		this.m_freelancerExpBonus = false;
		this.m_ggAmountGain.text = ggCurrencyGain.ToString();
		this.m_winAmountGain.text = winCurrencyGain.ToString();
		this.m_partyAmountGain.text = "0";
		this.m_levelUpAmountGain.text = levelUpGain.ToString();
		this.m_questAmountGain.text = questCurrencyGain.ToString();
		this.m_queueAmountGain.text = "0";
		this.m_eventBonusGain.text = eventGain.ToString();
		this.m_totalAmountGain.text = (normalCurrencyGain + ggCurrencyGain + winCurrencyGain + questCurrencyGain + levelUpGain + eventGain).ToString();
		this.UpdateDisplayLabels(tooltipType);
	}

	public void Setup(UIGameOverScreen.XPDisplayInfo displayInfo, UIGameOverRewardTooltip.RewardTooltipType tooltipType, int numEarners)
	{
		this.m_baseTypeLabel.text = StringUtil.TR("BaseExp", "GameOver");
		this.m_totalTypeLabel.text = StringUtil.TR("TotalExp", "GameOver");
		MatchResultsNotification results = UIGameOverScreen.Get().Results;
		if (results == null)
		{
			this.m_baseAmountGain.text = "0";
			this.m_consumableXPGain.text = "0";
			this.m_freelancerExpBonus = false;
			this.m_winAmountGain.text = "0";
			this.m_ggAmountGain.text = "0";
			this.m_partyAmountGain.text = "0";
			this.m_questAmountGain.text = "0";
			this.m_levelUpAmountGain.text = "0";
			this.m_freelancerOwnedGain.text = "0";
			this.m_eventBonusGain.text = "0";
			this.m_queueAmountGain.text = "0";
			this.m_totalAmountGain.text = "0";
		}
		else
		{
			this.m_freelancerExpBonus = (results.ConsumableXpGained > 0);
			int num = 0;
			if (tooltipType == UIGameOverRewardTooltip.RewardTooltipType.CharacterInfo)
			{
				num = results.ConsumableXpGained;
			}
			int num2 = results.BaseXpGained + results.WinXpGained + results.GGXpGained + results.PlayWithFriendXpGained + results.QuestXpGained + results.FreelancerOwnedXPGained + results.EventBonusXpGained + results.QueueTimeXpGained + num;
			if (numEarners > 1)
			{
				int splitTotal = num2 / numEarners;
				this.m_baseAmountGain.text = this.GetSplitExpAmount(results.BaseXpGained, num2, splitTotal);
				this.m_consumableXPGain.text = this.GetSplitExpAmount(results.ConsumableXpGained, num2, splitTotal);
				this.m_winAmountGain.text = this.GetSplitExpAmount(results.WinXpGained, num2, splitTotal);
				this.m_ggAmountGain.text = this.GetSplitExpAmount(results.GGXpGained, num2, splitTotal);
				this.m_partyAmountGain.text = this.GetSplitExpAmount(results.PlayWithFriendXpGained, num2, splitTotal);
				this.m_questAmountGain.text = this.GetSplitExpAmount(results.QuestXpGained, num2, splitTotal);
				this.m_levelUpAmountGain.text = "0";
				this.m_freelancerOwnedGain.text = this.GetSplitExpAmount(results.FreelancerOwnedXPGained, num2, splitTotal);
				this.m_eventBonusGain.text = this.GetSplitExpAmount(results.EventBonusXpGained, num2, splitTotal);
				this.m_queueAmountGain.text = this.GetSplitExpAmount(results.QueueTimeXpGained, num2, splitTotal);
				this.m_totalAmountGain.text = splitTotal.ToString();
			}
			else
			{
				this.m_baseAmountGain.text = results.BaseXpGained.ToString();
				this.m_consumableXPGain.text = results.ConsumableXpGained.ToString();
				this.m_winAmountGain.text = results.WinXpGained.ToString();
				this.m_ggAmountGain.text = results.GGXpGained.ToString();
				this.m_partyAmountGain.text = results.PlayWithFriendXpGained.ToString();
				this.m_questAmountGain.text = results.QuestXpGained.ToString();
				this.m_levelUpAmountGain.text = "0";
				this.m_freelancerOwnedGain.text = results.FreelancerOwnedXPGained.ToString();
				this.m_eventBonusGain.text = results.EventBonusXpGained.ToString();
				this.m_queueAmountGain.text = results.QueueTimeXpGained.ToString();
				this.m_totalAmountGain.text = num2.ToString();
			}
		}
		this.UpdateDisplayLabels(tooltipType);
	}

	private string GetSplitExpAmount(int normalExp, int normalTotal, int splitTotal)
	{
		float num = (float)splitTotal;
		return (num * ((float)normalExp / (float)normalTotal)).ToString("0.##");
	}

	public void Setup(UIGameOverPanel.XPDisplayInfo displayInfo, UIGameOverRewardTooltip.RewardTooltipType tooltipType)
	{
		this.m_baseTypeLabel.text = StringUtil.TR("BaseExp", "GameOver");
		this.m_totalTypeLabel.text = StringUtil.TR("TotalExp", "GameOver");
		this.m_baseAmountGain.text = (displayInfo.m_normalXPInitial - displayInfo.m_queueTimeXpInitial).ToString();
		this.m_consumableXPGain.text = "0";
		this.m_freelancerExpBonus = false;
		this.m_winAmountGain.text = displayInfo.m_winBonusXPInitial.ToString();
		this.m_ggAmountGain.text = displayInfo.m_ggPackXPInitial.ToString();
		this.m_partyAmountGain.text = displayInfo.m_playWithFriendXpInitial.ToString();
		this.m_questAmountGain.text = displayInfo.m_questXpInitial.ToString();
		this.m_levelUpAmountGain.text = "0";
		this.m_freelancerOwnedGain.text = displayInfo.m_freelancerOwnedXpInitial.ToString();
		this.m_eventBonusGain.text = displayInfo.m_eventBonusXpInitial.ToString();
		this.m_queueAmountGain.text = displayInfo.m_queueTimeXpInitial.ToString();
		this.m_totalAmountGain.text = displayInfo.GetTotalExpInitial().ToString();
		this.UpdateDisplayLabels(tooltipType);
	}

	public void Setup(Dictionary<string, int> factionInfo, UIGameOverRewardTooltip.RewardTooltipType tooltipType)
	{
		this.m_baseTypeLabel.text = StringUtil.TR("FactionContributionMatch", "TrustWar");
		int num;
		factionInfo.TryGetValue("Match", out num);
		int num2;
		factionInfo.TryGetValue("Win", out num2);
		int num3;
		factionInfo.TryGetValue("Banner", out num3);
		int num4;
		factionInfo.TryGetValue("Emblem", out num4);
		int num5;
		factionInfo.TryGetValue("Skin", out num5);
		this.m_baseAmountGain.text = num.ToString();
		this.m_winAmountGain.text = num2.ToString();
		this.m_skinAmountGain.text = num5.ToString();
		this.m_bannerAmountGain.text = num3.ToString();
		this.m_emblemAmountGain.text = num4.ToString();
		this.UpdateDisplayLabels(tooltipType);
	}

	private void UpdateDisplayLabels(UIGameOverRewardTooltip.RewardTooltipType tooltipType)
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
		if (tooltipType == UIGameOverRewardTooltip.RewardTooltipType.AccountInfo)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIGameOverRewardTooltip.UpdateDisplayLabels(UIGameOverRewardTooltip.RewardTooltipType)).MethodHandle;
			}
			flag = false;
			doActive3 = false;
			doActive8 = false;
			doActive9 = false;
			doActive10 = false;
		}
		else if (tooltipType == UIGameOverRewardTooltip.RewardTooltipType.CharacterInfo)
		{
			doActive7 = false;
			doActive8 = false;
			doActive9 = false;
			doActive10 = false;
		}
		else if (tooltipType == UIGameOverRewardTooltip.RewardTooltipType.RankedPointsAmount)
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
			if (tooltipType != UIGameOverRewardTooltip.RewardTooltipType.ISOAmount)
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
				if (tooltipType != UIGameOverRewardTooltip.RewardTooltipType.FreelancerCurrencyAmount)
				{
					if (tooltipType == UIGameOverRewardTooltip.RewardTooltipType.FactionInfo)
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
						goto IL_C9;
					}
					goto IL_C9;
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
		IL_C9:
		UIManager.SetGameObjectActive(this.m_baseTypeLabel, doActive, null);
		Component consumableXPLabel = this.m_consumableXPLabel;
		bool doActive13;
		if (flag)
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
			doActive13 = this.m_freelancerExpBonus;
		}
		else
		{
			doActive13 = false;
		}
		UIManager.SetGameObjectActive(consumableXPLabel, doActive13, null);
		UIManager.SetGameObjectActive(this.m_partyAmountLabel, doActive6, null);
		UIManager.SetGameObjectActive(this.m_ggAmountLabel, doActive5, null);
		UIManager.SetGameObjectActive(this.m_winAmountLabel, doActive4, null);
		UIManager.SetGameObjectActive(this.m_totalTypeLabel, doActive2, null);
		UIManager.SetGameObjectActive(this.m_questAmountLabel, doActive7, null);
		UIManager.SetGameObjectActive(this.m_levelUpLabel, doActive3, null);
		UIManager.SetGameObjectActive(this.m_skinAmountLabel, doActive8, null);
		UIManager.SetGameObjectActive(this.m_bannerAmountLabel, doActive9, null);
		UIManager.SetGameObjectActive(this.m_emblemAmountLabel, doActive10, null);
		UIManager.SetGameObjectActive(this.m_freelancerOwnedLabel, flag2, null);
		UIManager.SetGameObjectActive(this.m_eventBonusLabel, doActive11, null);
		UIManager.SetGameObjectActive(this.m_queueAmountLabel, doActive12, null);
		this.m_totalTypeLabel.transform.SetAsLastSibling();
		if (flag2)
		{
			int num = 0;
			int num2 = 0;
			bool hasPurchasedGame = ClientGameManager.Get().HasPurchasedGame;
			int i = 0;
			while (i < GameWideData.Get().m_characterResourceLinks.Length)
			{
				CharacterResourceLink characterResourceLink = GameWideData.Get().m_characterResourceLinks[i];
				if (!(characterResourceLink == null) && !characterResourceLink.m_characterType.IsWillFill() && !characterResourceLink.m_isHidden)
				{
					if (!characterResourceLink.m_allowForPlayers)
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
					}
					else
					{
						num2++;
						if (!hasPurchasedGame)
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
							if (!ClientGameManager.Get().GetPlayerCharacterData(characterResourceLink.m_characterType).CharacterComponent.Unlocked)
							{
								goto IL_261;
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
						num++;
					}
				}
				IL_261:
				i++;
				continue;
				goto IL_261;
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
			this.m_freelancerOwnedLabel.text = string.Format(StringUtil.TR("FreelancersOwned", "HUDScene"), num, num2);
		}
	}

	public void Setup(string title, string description, RectTransform guiElement, UIGameOverRewardTooltip.RewardTooltipType tooltipType)
	{
		this.m_baseTypeLabel.text = title;
		this.m_totalTypeLabel.text = description;
		this.m_baseAmountGain.text = string.Empty;
		this.m_consumableXPGain.text = string.Empty;
		this.m_freelancerExpBonus = false;
		this.m_winAmountGain.text = string.Empty;
		this.m_ggAmountGain.text = string.Empty;
		this.m_partyAmountGain.text = string.Empty;
		this.m_questAmountGain.text = string.Empty;
		this.m_levelUpAmountGain.text = string.Empty;
		this.m_totalAmountGain.text = string.Empty;
		this.m_skinAmountGain.text = string.Empty;
		this.m_bannerAmountGain.text = string.Empty;
		this.m_emblemAmountGain.text = string.Empty;
		this.m_freelancerOwnedGain.text = string.Empty;
		this.m_eventBonusGain.text = string.Empty;
		this.m_queueAmountGain.text = string.Empty;
		this.UpdateDisplayLabels(tooltipType);
	}

	public enum RewardTooltipType
	{
		AccountInfo,
		CharacterInfo,
		ISOAmount,
		RankedPointsAmount,
		FactionInfo,
		FreelancerCurrencyAmount
	}
}
