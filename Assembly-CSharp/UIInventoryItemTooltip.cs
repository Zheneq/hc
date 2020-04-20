using System;
using TMPro;
using UnityEngine;

public class UIInventoryItemTooltip : UITooltipBase
{
	public TextMeshProUGUI m_titleText;

	public TextMeshProUGUI m_rarityText;

	public TextMeshProUGUI m_desciptionText;

	public RectTransform m_descriptionDivider;

	public TextMeshProUGUI m_obtainedText;

	public RectTransform m_obtainedDivider;

	public TextMeshProUGUI m_flavorText;

	public RectTransform m_flavorDivider;

	private InventoryItemTemplate m_itemRef;

	public InventoryItemTemplate GetItemRef()
	{
		return this.m_itemRef;
	}

	public void Setup(InventoryItemTemplate item)
	{
		this.m_itemRef = item;
		if (item == null)
		{
			return;
		}
		string text = InventoryWideData.TypeDisplayString(item);
		this.m_titleText.text = item.GetDisplayName();
		this.m_rarityText.text = item.Rarity.GetRarityString() + " " + text;
		if (item.Type != InventoryItemType.Experience)
		{
			if (item.Type != InventoryItemType.FreelancerExpBonus)
			{
				goto IL_8F;
			}
		}
		this.m_rarityText.text = text;
		IL_8F:
		string colorHexString = item.Rarity.GetColorHexString();
		if (!colorHexString.IsNullOrEmpty())
		{
			this.m_titleText.text = string.Concat(new string[]
			{
				"<color=",
				colorHexString,
				">",
				this.m_titleText.text,
				"</color>"
			});
			this.m_rarityText.text = string.Concat(new string[]
			{
				"<color=",
				colorHexString,
				">",
				this.m_rarityText.text,
				"</color>"
			});
		}
		this.m_desciptionText.text = item.GetDescription();
		UIManager.SetGameObjectActive(this.m_descriptionDivider, !item.GetDescription().IsNullOrEmpty(), null);
		this.m_obtainedText.text = item.GetObtainDescription();
		UIManager.SetGameObjectActive(this.m_obtainedDivider, !item.GetObtainDescription().IsNullOrEmpty(), null);
		this.m_flavorText.text = "<i>" + item.GetFlavorText() + "</i>";
		UIManager.SetGameObjectActive(this.m_flavorDivider, !item.GetFlavorText().IsNullOrEmpty(), null);
		UIManager.SetGameObjectActive(this.m_flavorText, !item.GetFlavorText().IsNullOrEmpty(), null);
	}
}
