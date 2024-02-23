using System;
using System.Collections.Generic;
using System.Text;
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
		return this.m_displayIndex;
	}

	private void Awake()
	{
		this.m_factionRewardBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.FactionReward, new TooltipPopulateCall(this.FactionRewardTooltipSetup), null);
		this.m_personalRewardBtn.spriteController.GetComponent<UITooltipHoverObject>().Setup(TooltipType.FactionReward, new TooltipPopulateCall(this.PersonalRewardTooltipSetup), null);
		this.m_logo.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Titled, new TooltipPopulateCall(this.SetupLogoTooltip), null);
	}

	private bool SetupLogoTooltip(UITooltipBase tooltip)
	{
		int activeFactionCompetition = ClientGameManager.Get().ActiveFactionCompetition;
		string longName = Faction.GetLongName(activeFactionCompetition, this.m_displayIndex + 1);
		string loreDescription = Faction.GetLoreDescription(activeFactionCompetition, this.m_displayIndex + 1);
		if (!longName.IsNullOrEmpty())
		{
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
		UISeasonPanelFactionRewardTooltip uiseasonPanelFactionRewardTooltip = tooltip as UISeasonPanelFactionRewardTooltip;
		uiseasonPanelFactionRewardTooltip.SetPersonalRewardVisible(true);
		uiseasonPanelFactionRewardTooltip.SetupPersonalReward(this.m_personalLevel, this.m_currentFaction);
		return true;
	}

	private bool FactionRewardTooltipSetup(UITooltipBase tooltip)
	{
		UISeasonPanelFactionRewardTooltip uiseasonPanelFactionRewardTooltip = tooltip as UISeasonPanelFactionRewardTooltip;
		if (!uiseasonPanelFactionRewardTooltip.SetupFactionReward(this.m_currentFaction, this.m_currentFactionTier, this.m_isFactionComplete))
		{
			return false;
		}
		uiseasonPanelFactionRewardTooltip.SetCommunityFactionVisible(true);
		return true;
	}

	public void UpdateContributionBar(FactionPlayerData data)
	{
		if (this.m_currentFaction != null)
		{
			if (data != null)
			{
				int currentLevel = data.GetCurrentLevel(this.m_currentFaction.FactionPlayerProgressInfo, 0);
				Sprite sprite = null;
				if (currentLevel - 1 == this.m_currentFaction.FactionPlayerProgressInfo.Length)
				{
					this.m_personalBar.m_ProgressFillBar.fillAmount = 1f;
					this.m_personalLevelLabel.text = string.Format(StringUtil.TR("PersonalLevel", "TrustWar"), this.m_currentFaction.FactionPlayerProgressInfo.Length + 1);
					UIManager.SetGameObjectActive(this.m_personalBar.m_CompletedBar, true, null);
					this.m_personalExpLabel.text = data.TotalXP.ToString();
				}
				else
				{
					int xpthroughCurrentLevel = data.GetXPThroughCurrentLevel(this.m_currentFaction.FactionPlayerProgressInfo);
					float fillAmount = 0f;
					if (currentLevel - 1 < this.m_currentFaction.FactionPlayerProgressInfo.Length)
					{
						fillAmount = (float)xpthroughCurrentLevel / (float)this.m_currentFaction.FactionPlayerProgressInfo[currentLevel - 1].ExperienceToNextLevel;
						this.m_personalExpLabel.text = string.Format("{0} / {1}", xpthroughCurrentLevel, this.m_currentFaction.FactionPlayerProgressInfo[currentLevel - 1].ExperienceToNextLevel);
					}
					if (this.m_currentFaction.FactionPlayerProgressInfo[currentLevel - 1].LevelUpRewards != null)
					{
						List<FactionReward> allRewards = this.m_currentFaction.FactionPlayerProgressInfo[currentLevel - 1].LevelUpRewards.GetAllRewards();
						using (List<FactionReward>.Enumerator enumerator = allRewards.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								FactionReward factionReward = enumerator.Current;
								FactionItemReward factionItemReward = factionReward as FactionItemReward;
								if (factionItemReward != null)
								{
									InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(factionItemReward.ItemReward.ItemTemplateId);
									sprite = Resources.Load<Sprite>(InventoryWideData.GetSpritePath(itemTemplate));
								}
								FactionCurrencyReward factionCurrencyReward = factionReward as FactionCurrencyReward;
								if (factionCurrencyReward != null)
								{
									RewardUtils.RewardType rewardType;
									string currencyIconPath = RewardUtils.GetCurrencyIconPath(factionCurrencyReward.CurrencyReward.Type, out rewardType);
									sprite = Resources.Load<Sprite>(currencyIconPath);
								}
								FactionUnlockReward factionUnlockReward = factionReward as FactionUnlockReward;
								if (factionUnlockReward != null)
								{
									sprite = Resources.Load<Sprite>(factionUnlockReward.UnlockReward.resourceString);
								}
								if (sprite != null)
								{
									goto IL_292;
								}
							}
						}
					}
					IL_292:
					this.m_personalBar.m_ProgressFillBar.fillAmount = fillAmount;
					this.m_personalLevelLabel.text = string.Format(StringUtil.TR("PersonalLevel", "TrustWar"), currentLevel);
					UIManager.SetGameObjectActive(this.m_personalBar.m_CompletedBar, false, null);
				}
				this.m_personalLevel = currentLevel;
				if (sprite != null)
				{
					UIManager.SetGameObjectActive(this.m_personalRewardBtn, true, null);
					this.m_personalRewardIcon.sprite = sprite;
				}
				else
				{
					UIManager.SetGameObjectActive(this.m_personalRewardBtn, false, null);
				}
				return;
			}
		}
	}

	public void SetupBars(Faction faction, int tierIndex, long remainingScore, bool showText)
	{
		float fillAmount;
		if (faction.Tiers[tierIndex].ContributionToComplete > remainingScore)
		{
			UIManager.SetGameObjectActive(this.m_factionBars[tierIndex].m_CompletedBar, false, null);
			UIManager.SetGameObjectActive(this.m_factionBars[tierIndex].m_ProgressFillBar, true, null);
			fillAmount = (float)remainingScore / (float)faction.Tiers[tierIndex].ContributionToComplete;
			float[] rbga = FactionWideData.Get().GetRBGA(faction);
			this.m_factionBars[tierIndex].m_ProgressFillBar.color = new Color(rbga[0], rbga[1], rbga[2], rbga[3]);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_factionBars[tierIndex].m_CompletedBar, true, null);
			UIManager.SetGameObjectActive(this.m_factionBars[tierIndex].m_ProgressFillBar, false, null);
			fillAmount = 1f;
			float[] rbga2 = FactionWideData.Get().GetRBGA(faction);
			this.m_factionBars[tierIndex].m_CompletedBar.color = new Color(rbga2[0], rbga2[1], rbga2[2], rbga2[3]);
		}
		this.m_factionBars[tierIndex].m_ProgressFillBar.fillAmount = fillAmount;
	}

	public void Setup(Faction faction, long score, int DisplayIndex)
	{
		this.m_currentFaction = faction;
		this.m_displayIndex = DisplayIndex;
		if (this.m_factionBars != null)
		{
			if (this.m_factionBars.Length == faction.Tiers.Count)
			{
				goto IL_22E;
			}
		}
		if (this.m_factionBars != null)
		{
			for (int i = 0; i < this.m_factionBars.Length; i++)
			{
				UnityEngine.Object.Destroy(this.m_factionBars[i].gameObject);
			}
		}
		this.m_factionBars = new UIFactionProgressBar[faction.Tiers.Count];
		float preferredWidth = (this.m_FactionLevelContainer.transform as RectTransform).sizeDelta.x / (float)this.m_factionBars.Length;
		for (int j = 0; j < this.m_factionBars.Length; j++)
		{
			this.m_factionBars[j] = UnityEngine.Object.Instantiate<UIFactionProgressBar>(this.m_ProgressBarPrefab);
			UIManager.ReparentTransform(this.m_factionBars[j].transform, this.m_FactionLevelContainer.gameObject.transform);
			this.m_factionBars[j].m_LayoutElement.preferredWidth = preferredWidth;
		}
		if (this.m_factionBars.Length == 1)
		{
			this.m_factionBars[0].m_EmptyBar.type = Image.Type.Sliced;
			this.m_factionBars[0].m_CompletedBar.type = Image.Type.Sliced;
			this.m_factionBars[0].m_ProgressFillBar.fillSlope = 3.9f;
			this.m_factionBars[0].m_ProgressFillBar.m_FillMin = 0.1f;
			this.m_factionBars[0].m_ProgressFillBar.m_FillMax = 0.9f;
			this.m_factionBars[0].m_ProgressFillBar.fillStart = 0f;
			(this.m_factionBars[0].m_ProgressFillBar.gameObject.transform as RectTransform).sizeDelta = new Vector2(72f, (this.m_factionBars[0].m_ProgressFillBar.gameObject.transform as RectTransform).sizeDelta.y);
		}
		IL_22E:
		if (this.m_personalBar == null)
		{
			this.m_personalBar = UnityEngine.Object.Instantiate<UIFactionProgressBar>(this.m_ProgressBarPrefab);
			UIManager.ReparentTransform(this.m_personalBar.transform, this.m_PersonalLevelContainer.gameObject.transform);
			this.m_personalBar.m_LayoutElement.preferredWidth = (this.m_PersonalLevelContainer.transform as RectTransform).sizeDelta.x;
			this.m_personalBar.m_EmptyBar.type = Image.Type.Sliced;
			this.m_personalBar.m_CompletedBar.type = Image.Type.Sliced;
			this.m_personalBar.m_ProgressFillBar.fillSlope = 3.9f;
			this.m_personalBar.m_ProgressFillBar.m_FillMin = 0.1f;
			this.m_personalBar.m_ProgressFillBar.m_FillMax = 0.9f;
			this.m_personalBar.m_ProgressFillBar.fillStart = 0f;
			(this.m_personalBar.m_ProgressFillBar.gameObject.transform as RectTransform).sizeDelta = new Vector2(72f, (this.m_personalBar.m_ProgressFillBar.gameObject.transform as RectTransform).sizeDelta.y);
		}
		UIManager.SetGameObjectActive(this.m_PersonalInfoContainer, faction.DisplayPersonalContribution, null);
		FactionGroup factionGroup = FactionWideData.Get().GetFactionGroup(faction.FactionGroupIDToUse);
		this.m_logo.sprite = Resources.Load<Sprite>(factionGroup.BannerPath);
		long num = score;
		bool flag = false;
		int k = 0;
		while (k < faction.Tiers.Count)
		{
			if (k == faction.Tiers.Count - 1)
			{
				goto IL_432;
			}
			if (score < faction.Tiers[k].ContributionToComplete)
			{
				goto IL_432;
			}
			bool flag2 = score < faction.Tiers[k].ContributionToComplete + faction.Tiers[k + 1].ContributionToComplete;
			IL_433:
			bool showText = flag2;
			this.SetupBars(faction, k, (score >= 0L) ? score : 0L, showText);
			if (score >= 0L)
			{
				if (score < faction.Tiers[k].ContributionToComplete)
				{
					this.m_currentFactionTier = k;
					this.m_isFactionComplete = false;
				}
				else if (score >= faction.Tiers[k].ContributionToComplete)
				{
					this.m_currentFactionTier = k;
					this.m_isFactionComplete = true;
				}
			}
			if (score >= 0L)
			{
				if (score < faction.Tiers[k].ContributionToComplete)
				{
					if (this.m_expLabel != null)
					{
						if (this.m_expLabel != null)
						{
							this.m_expLabel.text = new StringBuilder().Append(score).Append(" / ").Append(faction.Tiers[k].ContributionToComplete).ToString();
							flag = true;
						}
					}
				}
			}
			score -= faction.Tiers[k].ContributionToComplete;
			k++;
			continue;
			IL_432:
			flag2 = true;
			goto IL_433;
		}
		int activeFactionCompetition = ClientGameManager.Get().ActiveFactionCompetition;
		this.m_factionLevelLabel.text = string.Format(StringUtil.TR("FactionLevel", "TrustWar"), Faction.GetDisplayName(activeFactionCompetition, this.m_displayIndex + 1), (this.m_currentFactionTier + 1).ToString());
		bool flag3 = false;
		int l = this.m_currentFactionTier;
		while (l < faction.Tiers.Count)
		{
			if (flag3)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					goto IL_7C7;
				}
			}
			else
			{
				foreach (FactionItemReward factionItemReward in faction.Tiers[l].Rewards.ItemRewards)
				{
					InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate(factionItemReward.ItemReward.ItemTemplateId);
					if (!flag3)
					{
						this.m_factionRewardIcon.sprite = Resources.Load<Sprite>(InventoryWideData.GetSpritePath(itemTemplate));
						flag3 = true;
						break;
					}
				}
				if (!flag3)
				{
					using (List<FactionCurrencyReward>.Enumerator enumerator2 = faction.Tiers[l].Rewards.CurrencyRewards.GetEnumerator())
					{
						if (!enumerator2.MoveNext())
						{
						}
						else
						{
							FactionCurrencyReward factionCurrencyReward = enumerator2.Current;
							RewardUtils.RewardType rewardType;
							string currencyIconPath = RewardUtils.GetCurrencyIconPath(factionCurrencyReward.CurrencyReward.Type, out rewardType);
							this.m_factionRewardIcon.sprite = Resources.Load<Sprite>(currencyIconPath);
							flag3 = true;
						}
					}
				}
				if (!flag3)
				{
					using (List<FactionUnlockReward>.Enumerator enumerator3 = faction.Tiers[l].Rewards.UnlockRewards.GetEnumerator())
					{
						if (!enumerator3.MoveNext())
						{
						}
						else
						{
							FactionUnlockReward factionUnlockReward = enumerator3.Current;
							this.m_factionRewardIcon.sprite = Resources.Load<Sprite>(factionUnlockReward.UnlockReward.resourceString);
							flag3 = true;
						}
					}
				}
				l++;
			}
		}
		IL_7C7:
		UIManager.SetGameObjectActive(this.m_factionRewardBtn, flag3, null);
		if (!flag)
		{
			if (this.m_expLabel != null)
			{
				this.m_expLabel.text = num.ToString();
			}
		}
	}
}
