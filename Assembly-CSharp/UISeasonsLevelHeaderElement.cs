using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISeasonsLevelHeaderElement : MonoBehaviour
{
	public HorizontalLayoutGroup m_gridContainer;

	public TextMeshProUGUI m_headerTitle;

	public Image[] m_rewardImages;

	public UISeasonRepeatingRewardInfo[] infos;

	private void Awake()
	{
		for (int i = 0; i < this.m_rewardImages.Length; i++)
		{
			int index = i;
			this.m_rewardImages[i].GetComponent<UITooltipHoverObject>().Setup(TooltipType.InventoryItem, (UITooltipBase tooltip) => this.TooltipSetup(tooltip, index), null);
		}
	}

	public void Setup(int imageIndex, UISeasonRepeatingRewardInfo rewardInfo)
	{
		if (this.infos == null)
		{
			this.infos = new UISeasonRepeatingRewardInfo[this.m_rewardImages.Length];
		}
		if (-1 < imageIndex)
		{
			if (imageIndex < this.m_rewardImages.Length)
			{
				this.m_rewardImages[imageIndex].sprite = rewardInfo.GetDisplaySprite();
				this.infos[imageIndex] = rewardInfo;
			}
		}
	}

	private bool TooltipSetup(UITooltipBase tooltip, int index)
	{
		SeasonReward seasonRewardReference = this.infos[index].GetSeasonRewardReference();
		if (seasonRewardReference is SeasonItemReward)
		{
			InventoryItemTemplate itemTemplate = InventoryWideData.Get().GetItemTemplate((seasonRewardReference as SeasonItemReward).ItemReward.ItemTemplateId);
			(tooltip as UIInventoryItemTooltip).Setup(itemTemplate);
			return true;
		}
		return false;
	}
}
