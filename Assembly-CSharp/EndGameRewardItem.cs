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
			UINewReward.SeasonReward seasonReward = default(UINewReward.SeasonReward);
			seasonReward.m_itemTemplate = Data.InventoryTemplate;
			seasonReward.m_amount = Data.Amount;
			seasonReward.m_unlockLevel = Data.Level;
			m_template = Data.InventoryTemplate;
			m_rewardItemName.text = seasonReward.m_itemTemplate.GetDisplayName();
			if (Data.isRepeating)
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
				m_rewardSubText.text = string.Format(StringUtil.TR("AwardedEverySeasonLevels", "Global"), Data.repeatLevels);
			}
			else
			{
				m_rewardSubText.text = string.Format(StringUtil.TR("SeasonLevel", "Seasons"), Data.Level);
			}
			bool doActive = false;
			bool doActive2 = false;
			bool doActive3 = false;
			Sprite sprite = (Sprite)Resources.Load(InventoryWideData.GetSpritePath(seasonReward.m_itemTemplate), typeof(Sprite));
			if (seasonReward.m_itemTemplate.Type == InventoryItemType.Taunt)
			{
				m_rewardImage.sprite = sprite;
				m_tauntAbilityIcon.sprite = InventoryWideData.GetItemFg(seasonReward.m_itemTemplate);
				doActive2 = true;
				doActive3 = true;
			}
			else if (seasonReward.m_itemTemplate.Type == InventoryItemType.BannerID)
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
				m_bannerReward.sprite = sprite;
				doActive = true;
			}
			else
			{
				m_rewardImage.sprite = sprite;
				doActive2 = true;
			}
			UIManager.SetGameObjectActive(m_bannerReward, doActive);
			UIManager.SetGameObjectActive(m_rewardImage, doActive2);
			UIManager.SetGameObjectActive(m_tauntAbilityIcon, doActive3);
			if (!(m_tooltipHoverObj != null))
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
				m_tooltipHoverObj.Setup(TooltipType.InventoryItem, SetupTooltip);
				m_tooltipHoverObj.Refresh();
				return;
			}
		}
		m_rewardItemName.text = RewardUtils.GetDisplayString(Data, true);
		string str = string.Empty;
		if (Data.isRepeating)
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
			str = string.Format(StringUtil.TR("AwardedEveryCharacterLevels", "Global"), Data.repeatLevels, GameWideData.Get().GetCharacterResourceLink(CharType).GetDisplayName());
		}
		else
		{
			if (Data.Level > 0)
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
				str = string.Format(StringUtil.TR("LevelRequirement", "Rewards"), Data.Level);
			}
			str = str + " " + GameWideData.Get().GetCharacterResourceLink(CharType).GetDisplayName();
		}
		str = str.Trim();
		m_rewardSubText.text = str;
		if (Data.SpritePath == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					m_rewardImage.sprite = (Sprite)Resources.Load("QuestRewards/general", typeof(Sprite));
					return;
				}
			}
		}
		bool doActive4 = false;
		bool doActive5 = false;
		bool doActive6 = false;
		Sprite sprite2 = (Sprite)Resources.Load(Data.SpritePath, typeof(Sprite));
		if (Data.Type == RewardUtils.RewardType.Banner)
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
			doActive4 = true;
			m_bannerReward.sprite = sprite2;
		}
		else if (Data.Foreground != null)
		{
			doActive5 = true;
			doActive6 = true;
			m_rewardImage.sprite = sprite2;
			m_tauntAbilityIcon.sprite = Data.Foreground;
		}
		else
		{
			doActive5 = true;
			m_rewardImage.sprite = sprite2;
		}
		UIManager.SetGameObjectActive(m_bannerReward, doActive4);
		UIManager.SetGameObjectActive(m_rewardImage, doActive5);
		UIManager.SetGameObjectActive(m_tauntAbilityIcon, doActive6);
	}

	private bool SetupTooltip(UITooltipBase tooltip)
	{
		if (tooltip is UIInventoryItemTooltip)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_template != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						(tooltip as UIInventoryItemTooltip).Setup(m_template);
						return true;
					}
				}
			}
		}
		return false;
	}
}
