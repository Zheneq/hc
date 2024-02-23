using System.Text;
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
		return m_itemRef;
	}

	public void Setup(InventoryItemTemplate item)
	{
		m_itemRef = item;
		if (item == null)
		{
			while (true)
			{
				return;
			}
		}
		string text = InventoryWideData.TypeDisplayString(item);
		m_titleText.text = item.GetDisplayName();
		m_rarityText.text = new StringBuilder().Append(item.Rarity.GetRarityString()).Append(" ").Append(text).ToString();
		if (item.Type != InventoryItemType.Experience)
		{
			if (item.Type != InventoryItemType.FreelancerExpBonus)
			{
				goto IL_008f;
			}
		}
		m_rarityText.text = text;
		goto IL_008f;
		IL_008f:
		string colorHexString = item.Rarity.GetColorHexString();
		if (!colorHexString.IsNullOrEmpty())
		{
			m_titleText.text = new StringBuilder().Append("<color=").Append(colorHexString).Append(">").Append(m_titleText.text).Append("</color>").ToString();
			m_rarityText.text = new StringBuilder().Append("<color=").Append(colorHexString).Append(">").Append(m_rarityText.text).Append("</color>").ToString();
		}
		m_desciptionText.text = item.GetDescription();
		UIManager.SetGameObjectActive(m_descriptionDivider, !item.GetDescription().IsNullOrEmpty());
		m_obtainedText.text = item.GetObtainDescription();
		UIManager.SetGameObjectActive(m_obtainedDivider, !item.GetObtainDescription().IsNullOrEmpty());
		m_flavorText.text = new StringBuilder().Append("<i>").Append(item.GetFlavorText()).Append("</i>").ToString();
		UIManager.SetGameObjectActive(m_flavorDivider, !item.GetFlavorText().IsNullOrEmpty());
		UIManager.SetGameObjectActive(m_flavorText, !item.GetFlavorText().IsNullOrEmpty());
	}
}
