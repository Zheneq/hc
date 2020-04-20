using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EndGameRewardItem : MonoBehaviour
{
	public TextMeshProUGUI m_rewardItemName;

	public TextMeshProUGUI m_rewardSubText;

	public Image m_bannerReward;

	public Image m_rewardImage;

	public Image m_tauntAbilityIcon;

	public UITooltipHoverObject m_tooltipHoverObj;

	private InventoryItemTemplate m_template;

	public void Setup(RewardUtils.RewardData Data, CharacterType CharType)
	{
		if (CharType == CharacterType.None)
		{
			UINewReward.SeasonReward seasonReward;
			seasonReward.m_itemTemplate = Data.InventoryTemplate;
			seasonReward.m_amount = Data.Amount;
			seasonReward.m_unlockLevel = Data.Level;
			this.m_template = Data.InventoryTemplate;
			this.m_rewardItemName.text = seasonReward.m_itemTemplate.GetDisplayName();
			if (Data.isRepeating)
			{
				this.m_rewardSubText.text = string.Format(StringUtil.TR("AwardedEverySeasonLevels", "Global"), Data.repeatLevels);
			}
			else
			{
				this.m_rewardSubText.text = string.Format(StringUtil.TR("SeasonLevel", "Seasons"), Data.Level);
			}
			bool doActive = false;
			bool doActive2 = false;
			bool doActive3 = false;
			Sprite sprite = (Sprite)Resources.Load(InventoryWideData.GetSpritePath(seasonReward.m_itemTemplate), typeof(Sprite));
			if (seasonReward.m_itemTemplate.Type == InventoryItemType.Taunt)
			{
				this.m_rewardImage.sprite = sprite;
				this.m_tauntAbilityIcon.sprite = InventoryWideData.GetItemFg(seasonReward.m_itemTemplate);
				doActive2 = true;
				doActive3 = true;
			}
			else if (seasonReward.m_itemTemplate.Type == InventoryItemType.BannerID)
			{
				this.m_bannerReward.sprite = sprite;
				doActive = true;
			}
			else
			{
				this.m_rewardImage.sprite = sprite;
				doActive2 = true;
			}
			UIManager.SetGameObjectActive(this.m_bannerReward, doActive, null);
			UIManager.SetGameObjectActive(this.m_rewardImage, doActive2, null);
			UIManager.SetGameObjectActive(this.m_tauntAbilityIcon, doActive3, null);
			if (this.m_tooltipHoverObj != null)
			{
				this.m_tooltipHoverObj.Setup(TooltipType.InventoryItem, new TooltipPopulateCall(this.SetupTooltip), null);
				this.m_tooltipHoverObj.Refresh();
			}
		}
		else
		{
			this.m_rewardItemName.text = RewardUtils.GetDisplayString(Data, true);
			string text = string.Empty;
			if (Data.isRepeating)
			{
				text = string.Format(StringUtil.TR("AwardedEveryCharacterLevels", "Global"), Data.repeatLevels, GameWideData.Get().GetCharacterResourceLink(CharType).GetDisplayName());
			}
			else
			{
				if (Data.Level > 0)
				{
					text = string.Format(StringUtil.TR("LevelRequirement", "Rewards"), Data.Level);
				}
				text = text + " " + GameWideData.Get().GetCharacterResourceLink(CharType).GetDisplayName();
			}
			text = text.Trim();
			this.m_rewardSubText.text = text;
			if (Data.SpritePath == null)
			{
				this.m_rewardImage.sprite = (Sprite)Resources.Load("QuestRewards/general", typeof(Sprite));
			}
			else
			{
				bool doActive4 = false;
				bool doActive5 = false;
				bool doActive6 = false;
				Sprite sprite2 = (Sprite)Resources.Load(Data.SpritePath, typeof(Sprite));
				if (Data.Type == RewardUtils.RewardType.Banner)
				{
					doActive4 = true;
					this.m_bannerReward.sprite = sprite2;
				}
				else if (Data.Foreground != null)
				{
					doActive5 = true;
					doActive6 = true;
					this.m_rewardImage.sprite = sprite2;
					this.m_tauntAbilityIcon.sprite = Data.Foreground;
				}
				else
				{
					doActive5 = true;
					this.m_rewardImage.sprite = sprite2;
				}
				UIManager.SetGameObjectActive(this.m_bannerReward, doActive4, null);
				UIManager.SetGameObjectActive(this.m_rewardImage, doActive5, null);
				UIManager.SetGameObjectActive(this.m_tauntAbilityIcon, doActive6, null);
			}
		}
	}

	private bool SetupTooltip(UITooltipBase tooltip)
	{
		if (tooltip is UIInventoryItemTooltip)
		{
			if (this.m_template != null)
			{
				(tooltip as UIInventoryItemTooltip).Setup(this.m_template);
				return true;
			}
		}
		return false;
	}
}
