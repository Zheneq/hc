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
				switch (5)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return;
			}
		}
		string text = InventoryWideData.TypeDisplayString(item);
		m_titleText.text = item.GetDisplayName();
		m_rarityText.text = item.Rarity.GetRarityString() + " " + text;
		if (item.Type != InventoryItemType.Experience)
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
			if (item.Type != InventoryItemType.FreelancerExpBonus)
			{
				goto IL_008f;
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
		m_rarityText.text = text;
		goto IL_008f;
		IL_008f:
		string colorHexString = item.Rarity.GetColorHexString();
		if (!colorHexString.IsNullOrEmpty())
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
			m_titleText.text = "<color=" + colorHexString + ">" + m_titleText.text + "</color>";
			m_rarityText.text = "<color=" + colorHexString + ">" + m_rarityText.text + "</color>";
		}
		m_desciptionText.text = item.GetDescription();
		UIManager.SetGameObjectActive(m_descriptionDivider, !item.GetDescription().IsNullOrEmpty());
		m_obtainedText.text = item.GetObtainDescription();
		UIManager.SetGameObjectActive(m_obtainedDivider, !item.GetObtainDescription().IsNullOrEmpty());
		m_flavorText.text = "<i>" + item.GetFlavorText() + "</i>";
		UIManager.SetGameObjectActive(m_flavorDivider, !item.GetFlavorText().IsNullOrEmpty());
		UIManager.SetGameObjectActive(m_flavorText, !item.GetFlavorText().IsNullOrEmpty());
	}
}
