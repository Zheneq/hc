using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISeasonPanelViewEntry : MonoBehaviour
{
	public TextMeshProUGUI m_expLabel;

	public TextMeshProUGUI m_personalExpLabel;

	public Image m_logo;

	public LayoutGroup m_FactionLevelContainer;

	public LayoutGroup m_PersonalLevelContainer;

	public RectTransform m_FactionInfoContainer;

	public RectTransform m_PersonalInfoContainer;

	public _SelectableBtn m_factionRewardBtn;

	public _SelectableBtn m_personalRewardBtn;

	public TextMeshProUGUI m_factionLevelLabel;

	public TextMeshProUGUI m_personalLevelLabel;

	public Image m_factionRewardIcon;

	public Image m_personalRewardIcon;

	public UIFactionProgressBar m_ProgressBarPrefab;

	private Faction m_currentFaction;

	private int m_displayIndex;

	private int m_currentFactionTier;

	private int m_personalLevel;

	private bool m_isFactionComplete;

	private UIFactionProgressBar[] m_factionBars;

	private UIFactionProgressBar m_personalBar;

	public int GetFactionDisplayIndex()
	{
		return m_displayIndex;
	}

	private void Awake()
	{
		m_factionRewardBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.FactionReward, FactionRewardTooltipSetup);
		m_personalRewardBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.FactionReward, PersonalRewardTooltipSetup);
		m_logo.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, SetupLogoTooltip);
	}

	private bool SetupLogoTooltip(UITooltipBase tooltip)
	{
		int activeFactionCompetition = ClientGameManager.Get().ActiveFactionCompetition;
		string longName = Faction.GetLongName(activeFactionCompetition, m_displayIndex + 1);
		string loreDescription = Faction.GetLoreDescription(activeFactionCompetition, m_displayIndex + 1);
		if (!longName.IsNullOrEmpty())
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
			if (!loreDescription.IsNullOrEmpty())
			{
				(tooltip as UITitledTooltip).Setup(longName, loreDescription, string.Empty);
				return true;
			}
		}
		return false;
	}

	private bool PersonalRewardTooltipSetup(UITooltipBase tooltip)
	{
		UISeasonPanelFactionRewardTooltip uISeasonPanelFactionRewardTooltip = tooltip as UISeasonPanelFactionRewardTooltip;
		uISeasonPanelFactionRewardTooltip.SetPersonalRewardVisible(true);
		uISeasonPanelFactionRewardTooltip.SetupPersonalReward(m_personalLevel, m_currentFaction);
		return true;
	}

	private bool FactionRewardTooltipSetup(UITooltipBase tooltip)
	{
		UISeasonPanelFactionRewardTooltip uISeasonPanelFactionRewardTooltip = tooltip as UISeasonPanelFactionRewardTooltip;
		if (!uISeasonPanelFactionRewardTooltip.SetupFactionReward(m_currentFaction, m_currentFactionTier, m_isFactionComplete))
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return false;
				}
			}
		}
		uISeasonPanelFactionRewardTooltip.SetCommunityFactionVisible(true);
		return true;
	}

	public void UpdateContributionBar(FactionPlayerData data)
	{
		if (m_currentFaction == null)
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
			if (data == null)
			{
				while (true)
				{
					switch (1)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			int currentLevel = data.GetCurrentLevel(m_currentFaction.FactionPlayerProgressInfo);
			Sprite sprite = null;
			if (currentLevel - 1 == m_currentFaction.FactionPlayerProgressInfo.Length)
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
				m_personalBar.m_ProgressFillBar.fillAmount = 1f;
				m_personalLevelLabel.text = string.Format(StringUtil.TR("PersonalLevel", "TrustWar"), m_currentFaction.FactionPlayerProgressInfo.Length + 1);
				UIManager.SetGameObjectActive(m_personalBar.m_CompletedBar, true);
				m_personalExpLabel.text = data.TotalXP.ToString();
			}
			else
			{
				int xPThroughCurrentLevel = data.GetXPThroughCurrentLevel(m_currentFaction.FactionPlayerProgressInfo);
				float fillAmount = 0f;
				if (currentLevel - 1 < m_currentFaction.FactionPlayerProgressInfo.Length)
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
					fillAmount = (float)xPThroughCurrentLevel / (float)m_currentFaction.FactionPlayerProgressInfo[currentLevel - 1].ExperienceToNextLevel;
					m_personalExpLabel.text = $"{xPThroughCurrentLevel} / {m_currentFaction.FactionPlayerProgressInfo[currentLevel - 1].ExperienceToNextLevel}";
				}
				if (m_currentFaction.FactionPlayerProgressInfo[currentLevel - 1].LevelUpRewards != null)
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
					List<FactionReward> allRewards = m_currentFaction.FactionPlayerProgressInfo[currentLevel - 1].LevelUpRewards.GetAllRewards();
					using (List<FactionReward>.Enumerator enumerator = allRewards.GetEnumerator())
					{
						do
						{
							if (!enumerator.MoveNext())
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
								break;
							}
							FactionReward current = enumerator.Current;
							FactionItemReward factionItemReward = current as FactionItemReward;
							if (factionItemReward != null)
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
								InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(factionItemReward.ItemReward.ItemTemplateId);
								sprite = Resources.Load<Sprite>(InventoryWideData.GetSpritePath(itemTemplate));
							}
							FactionCurrencyReward factionCurrencyReward = current as FactionCurrencyReward;
							if (factionCurrencyReward != null)
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
								RewardUtils.RewardType rewardType;
								string currencyIconPath = RewardUtils.GetCurrencyIconPath(factionCurrencyReward.CurrencyReward.Type, out rewardType);
								sprite = Resources.Load<Sprite>(currencyIconPath);
							}
							FactionUnlockReward factionUnlockReward = current as FactionUnlockReward;
							if (factionUnlockReward != null)
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
								sprite = Resources.Load<Sprite>(factionUnlockReward.UnlockReward.resourceString);
							}
						}
						while (!(sprite != null));
					}
				}
				m_personalBar.m_ProgressFillBar.fillAmount = fillAmount;
				m_personalLevelLabel.text = string.Format(StringUtil.TR("PersonalLevel", "TrustWar"), currentLevel);
				UIManager.SetGameObjectActive(m_personalBar.m_CompletedBar, false);
			}
			m_personalLevel = currentLevel;
			if (sprite != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						UIManager.SetGameObjectActive(m_personalRewardBtn, true);
						m_personalRewardIcon.sprite = sprite;
						return;
					}
				}
			}
			UIManager.SetGameObjectActive(m_personalRewardBtn, false);
			return;
		}
	}

	public void SetupBars(Faction faction, int tierIndex, long remainingScore, bool showText)
	{
		float num = 0f;
		if (faction.Tiers[tierIndex].ContributionToComplete > remainingScore)
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
			UIManager.SetGameObjectActive(m_factionBars[tierIndex].m_CompletedBar, false);
			UIManager.SetGameObjectActive(m_factionBars[tierIndex].m_ProgressFillBar, true);
			num = (float)remainingScore / (float)faction.Tiers[tierIndex].ContributionToComplete;
			float[] rBGA = FactionWideData.Get().GetRBGA(faction);
			m_factionBars[tierIndex].m_ProgressFillBar.color = new Color(rBGA[0], rBGA[1], rBGA[2], rBGA[3]);
		}
		else
		{
			UIManager.SetGameObjectActive(m_factionBars[tierIndex].m_CompletedBar, true);
			UIManager.SetGameObjectActive(m_factionBars[tierIndex].m_ProgressFillBar, false);
			num = 1f;
			float[] rBGA2 = FactionWideData.Get().GetRBGA(faction);
			m_factionBars[tierIndex].m_CompletedBar.color = new Color(rBGA2[0], rBGA2[1], rBGA2[2], rBGA2[3]);
		}
		m_factionBars[tierIndex].m_ProgressFillBar.fillAmount = num;
	}

	public void Setup(Faction faction, long score, int DisplayIndex)
	{
		m_currentFaction = faction;
		m_displayIndex = DisplayIndex;
		if (m_factionBars != null)
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
			if (m_factionBars.Length == faction.Tiers.Count)
			{
				goto IL_022e;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		if (m_factionBars != null)
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
			for (int i = 0; i < m_factionBars.Length; i++)
			{
				Object.Destroy(m_factionBars[i].gameObject);
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_factionBars = new UIFactionProgressBar[faction.Tiers.Count];
		Vector2 sizeDelta = (m_FactionLevelContainer.transform as RectTransform).sizeDelta;
		float preferredWidth = sizeDelta.x / (float)m_factionBars.Length;
		for (int j = 0; j < m_factionBars.Length; j++)
		{
			m_factionBars[j] = Object.Instantiate(m_ProgressBarPrefab);
			UIManager.ReparentTransform(m_factionBars[j].transform, m_FactionLevelContainer.gameObject.transform);
			m_factionBars[j].m_LayoutElement.preferredWidth = preferredWidth;
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
		if (m_factionBars.Length == 1)
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
			m_factionBars[0].m_EmptyBar.type = Image.Type.Sliced;
			m_factionBars[0].m_CompletedBar.type = Image.Type.Sliced;
			m_factionBars[0].m_ProgressFillBar.fillSlope = 3.9f;
			m_factionBars[0].m_ProgressFillBar.m_FillMin = 0.1f;
			m_factionBars[0].m_ProgressFillBar.m_FillMax = 0.9f;
			m_factionBars[0].m_ProgressFillBar.fillStart = 0f;
			RectTransform obj = m_factionBars[0].m_ProgressFillBar.gameObject.transform as RectTransform;
			Vector2 sizeDelta2 = (m_factionBars[0].m_ProgressFillBar.gameObject.transform as RectTransform).sizeDelta;
			obj.sizeDelta = new Vector2(72f, sizeDelta2.y);
		}
		goto IL_022e;
		IL_022e:
		if (m_personalBar == null)
		{
			m_personalBar = Object.Instantiate(m_ProgressBarPrefab);
			UIManager.ReparentTransform(m_personalBar.transform, m_PersonalLevelContainer.gameObject.transform);
			LayoutElement layoutElement = m_personalBar.m_LayoutElement;
			Vector2 sizeDelta3 = (m_PersonalLevelContainer.transform as RectTransform).sizeDelta;
			layoutElement.preferredWidth = sizeDelta3.x;
			m_personalBar.m_EmptyBar.type = Image.Type.Sliced;
			m_personalBar.m_CompletedBar.type = Image.Type.Sliced;
			m_personalBar.m_ProgressFillBar.fillSlope = 3.9f;
			m_personalBar.m_ProgressFillBar.m_FillMin = 0.1f;
			m_personalBar.m_ProgressFillBar.m_FillMax = 0.9f;
			m_personalBar.m_ProgressFillBar.fillStart = 0f;
			RectTransform obj2 = m_personalBar.m_ProgressFillBar.gameObject.transform as RectTransform;
			Vector2 sizeDelta4 = (m_personalBar.m_ProgressFillBar.gameObject.transform as RectTransform).sizeDelta;
			obj2.sizeDelta = new Vector2(72f, sizeDelta4.y);
		}
		UIManager.SetGameObjectActive(m_PersonalInfoContainer, faction.DisplayPersonalContribution);
		FactionGroup factionGroup = FactionWideData.Get().GetFactionGroup(faction.FactionGroupIDToUse);
		m_logo.sprite = Resources.Load<Sprite>(factionGroup.BannerPath);
		long num = score;
		bool flag = false;
		for (int num2 = 0; num2 < faction.Tiers.Count; num2++)
		{
			int num3;
			if (num2 != faction.Tiers.Count - 1)
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
				if (score >= faction.Tiers[num2].ContributionToComplete)
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
					num3 = ((score < faction.Tiers[num2].ContributionToComplete + faction.Tiers[num2 + 1].ContributionToComplete) ? 1 : 0);
					goto IL_0433;
				}
			}
			num3 = 1;
			goto IL_0433;
			IL_0433:
			bool showText = (byte)num3 != 0;
			SetupBars(faction, num2, (score >= 0) ? score : 0, showText);
			if (score >= 0)
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
				if (score < faction.Tiers[num2].ContributionToComplete)
				{
					m_currentFactionTier = num2;
					m_isFactionComplete = false;
				}
				else if (score >= faction.Tiers[num2].ContributionToComplete)
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
					m_currentFactionTier = num2;
					m_isFactionComplete = true;
				}
			}
			if (score >= 0)
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
				if (score < faction.Tiers[num2].ContributionToComplete)
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
					if (m_expLabel != null)
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
						if (m_expLabel != null)
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
							m_expLabel.text = score + " / " + faction.Tiers[num2].ContributionToComplete;
							flag = true;
						}
					}
				}
			}
			score -= faction.Tiers[num2].ContributionToComplete;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			int activeFactionCompetition = ClientGameManager.Get().ActiveFactionCompetition;
			m_factionLevelLabel.text = string.Format(StringUtil.TR("FactionLevel", "TrustWar"), Faction.GetDisplayName(activeFactionCompetition, m_displayIndex + 1), (m_currentFactionTier + 1).ToString());
			bool flag2 = false;
			for (int k = m_currentFactionTier; k < faction.Tiers.Count; k++)
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
				if (!flag2)
				{
					foreach (FactionItemReward itemReward in faction.Tiers[k].Rewards.ItemRewards)
					{
						InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(itemReward.ItemReward.ItemTemplateId);
						if (!flag2)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									m_factionRewardIcon.sprite = Resources.Load<Sprite>(InventoryWideData.GetSpritePath(itemTemplate));
									flag2 = true;
									goto end_IL_0616;
								}
							}
						}
					}
					if (!flag2)
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
						using (List<FactionCurrencyReward>.Enumerator enumerator2 = faction.Tiers[k].Rewards.CurrencyRewards.GetEnumerator())
						{
							if (enumerator2.MoveNext())
							{
								FactionCurrencyReward current2 = enumerator2.Current;
								RewardUtils.RewardType rewardType;
								string currencyIconPath = RewardUtils.GetCurrencyIconPath(current2.CurrencyReward.Type, out rewardType);
								m_factionRewardIcon.sprite = Resources.Load<Sprite>(currencyIconPath);
								flag2 = true;
							}
							else
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
							}
						}
					}
					if (!flag2)
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
						using (List<FactionUnlockReward>.Enumerator enumerator3 = faction.Tiers[k].Rewards.UnlockRewards.GetEnumerator())
						{
							if (enumerator3.MoveNext())
							{
								FactionUnlockReward current3 = enumerator3.Current;
								m_factionRewardIcon.sprite = Resources.Load<Sprite>(current3.UnlockReward.resourceString);
								flag2 = true;
							}
							else
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
							}
						}
					}
					continue;
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				break;
			}
			UIManager.SetGameObjectActive(m_factionRewardBtn, flag2);
			if (flag)
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
				if (m_expLabel != null)
				{
					m_expLabel.text = num.ToString();
				}
				return;
			}
		}
	}
}
