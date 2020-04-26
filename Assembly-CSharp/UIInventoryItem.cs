using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryItem : MonoBehaviour
{
	public Image m_itemImage;

	public Image m_itemFg;

	public Image m_ownedCheckmark;

	public CanvasGroup[] m_dimmables;

	public RectTransform m_amountContainer;

	public TextMeshProUGUI m_amount;

	public _ButtonSwapSprite m_hitbox;

	public Image m_bgCommon;

	public Image m_bgUncommon;

	public Image m_bgRare;

	public Image m_bgEpic;

	public Image m_bgLegendary;

	public Image m_shadowCommon;

	public Image m_shadowUncommon;

	public Image m_shadowRare;

	public Image m_shadowEpic;

	public Image m_shadowLegendary;

	public Image m_normalBorder;

	public Image m_redBorder;

	public TextMeshProUGUI m_buyButton3xLabel;

	public Animator m_mainAnimator;

	protected InventoryItemTemplate m_itemTemplate;

	private InventoryItem m_item;

	private UITooltipHoverObject m_tooltipHoverObj;

	private void Start()
	{
		m_tooltipHoverObj = m_hitbox.GetComponent<UITooltipHoverObject>();
		m_tooltipHoverObj.Setup(TooltipType.InventoryItem, TooltipSetup);
	}

	public InventoryItemTemplate GetItemTemplate()
	{
		return m_itemTemplate;
	}

	public void Setup(InventoryItemTemplate itemTemplate, InventoryItem item)
	{
		m_itemTemplate = itemTemplate;
		m_item = item;
		int amount = 1;
		if (item != null)
		{
			amount = item.Count;
		}
		if (m_mainAnimator != null && m_mainAnimator.isActiveAndEnabled)
		{
			m_mainAnimator.Play("InventoryItemSalvageIDLE");
		}
		MakeBorderError(false);
		SetDimmed(itemTemplate == null);
		UpdateItemCount(amount);
		string spritePath = InventoryWideData.GetSpritePath(itemTemplate);
		if (!spritePath.IsNullOrEmpty())
		{
			m_itemImage.sprite = (Sprite)Resources.Load(spritePath, typeof(Sprite));
		}
		UIManager.SetGameObjectActive(m_itemImage, !spritePath.IsNullOrEmpty());
		if (m_itemFg != null)
		{
			m_itemFg.sprite = InventoryWideData.GetItemFg(itemTemplate);
			UIManager.SetGameObjectActive(m_itemFg, m_itemFg.sprite != null);
		}
		Image bgCommon = m_bgCommon;
		int doActive;
		if (itemTemplate != null)
		{
			doActive = ((itemTemplate.Rarity == InventoryItemRarity.Common) ? 1 : 0);
		}
		else
		{
			doActive = 0;
		}
		UIManager.SetGameObjectActive(bgCommon, (byte)doActive != 0);
		Image bgUncommon = m_bgUncommon;
		int doActive2;
		if (itemTemplate != null)
		{
			doActive2 = ((itemTemplate.Rarity == InventoryItemRarity.Uncommon) ? 1 : 0);
		}
		else
		{
			doActive2 = 0;
		}
		UIManager.SetGameObjectActive(bgUncommon, (byte)doActive2 != 0);
		Image bgRare = m_bgRare;
		int doActive3;
		if (itemTemplate != null)
		{
			doActive3 = ((itemTemplate.Rarity == InventoryItemRarity.Rare) ? 1 : 0);
		}
		else
		{
			doActive3 = 0;
		}
		UIManager.SetGameObjectActive(bgRare, (byte)doActive3 != 0);
		UIManager.SetGameObjectActive(m_bgEpic, itemTemplate != null && itemTemplate.Rarity == InventoryItemRarity.Epic);
		Image bgLegendary = m_bgLegendary;
		int doActive4;
		if (itemTemplate != null)
		{
			doActive4 = ((itemTemplate.Rarity == InventoryItemRarity.Legendary) ? 1 : 0);
		}
		else
		{
			doActive4 = 0;
		}
		UIManager.SetGameObjectActive(bgLegendary, (byte)doActive4 != 0);
		if (m_shadowCommon != null)
		{
			UIManager.SetGameObjectActive(m_shadowCommon, itemTemplate != null && itemTemplate.Rarity == InventoryItemRarity.Common);
			Image shadowUncommon = m_shadowUncommon;
			int doActive5;
			if (itemTemplate != null)
			{
				doActive5 = ((itemTemplate.Rarity == InventoryItemRarity.Uncommon) ? 1 : 0);
			}
			else
			{
				doActive5 = 0;
			}
			UIManager.SetGameObjectActive(shadowUncommon, (byte)doActive5 != 0);
			Image shadowRare = m_shadowRare;
			int doActive6;
			if (itemTemplate != null)
			{
				doActive6 = ((itemTemplate.Rarity == InventoryItemRarity.Rare) ? 1 : 0);
			}
			else
			{
				doActive6 = 0;
			}
			UIManager.SetGameObjectActive(shadowRare, (byte)doActive6 != 0);
			Image shadowEpic = m_shadowEpic;
			int doActive7;
			if (itemTemplate != null)
			{
				doActive7 = ((itemTemplate.Rarity == InventoryItemRarity.Epic) ? 1 : 0);
			}
			else
			{
				doActive7 = 0;
			}
			UIManager.SetGameObjectActive(shadowEpic, (byte)doActive7 != 0);
			Image shadowLegendary = m_shadowLegendary;
			int doActive8;
			if (itemTemplate != null)
			{
				doActive8 = ((itemTemplate.Rarity == InventoryItemRarity.Legendary) ? 1 : 0);
			}
			else
			{
				doActive8 = 0;
			}
			UIManager.SetGameObjectActive(shadowLegendary, (byte)doActive8 != 0);
		}
		if (!(m_tooltipHoverObj != null))
		{
			return;
		}
		while (true)
		{
			m_tooltipHoverObj.Refresh();
			return;
		}
	}

	public void SetDimmed(bool isDimmed)
	{
		for (int i = 0; i < m_dimmables.Length; i++)
		{
			m_dimmables[i].alpha = ((!isDimmed) ? 1f : 0.25f);
		}
		while (true)
		{
			return;
		}
	}

	public void SetCollected(bool isCollected)
	{
		if (m_mainAnimator == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (!m_mainAnimator.isInitialized)
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
		m_mainAnimator.SetBool("IsCollected", isCollected);
		if (!isCollected)
		{
			m_mainAnimator.Play("InventoryItemCollectedDISABLE");
		}
		else
		{
			m_mainAnimator.Play("InventoryItemCollectedIDLE");
		}
	}

	private bool TooltipSetup(UITooltipBase tooltip)
	{
		if (m_itemTemplate == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return false;
				}
			}
		}
		(tooltip as UIInventoryItemTooltip).Setup(m_itemTemplate);
		return true;
	}

	public void MakeBorderError(bool makeError)
	{
		if (m_normalBorder != null)
		{
			UIManager.SetGameObjectActive(m_normalBorder, !makeError);
		}
		if (!(m_redBorder != null))
		{
			return;
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_redBorder, makeError);
			return;
		}
	}

	public int GetTemplateId()
	{
		if (m_itemTemplate == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return -1;
				}
			}
		}
		return m_itemTemplate.Index;
	}

	public int GetItemId()
	{
		if (m_item == null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return -1;
				}
			}
		}
		return m_item.Id;
	}

	public void UpdateItemData(InventoryItem item)
	{
		if (m_item == null)
		{
			return;
		}
		while (true)
		{
			if (item.TemplateId != m_item.TemplateId)
			{
				while (true)
				{
					switch (6)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			if (m_item.Count != item.Count)
			{
				while (true)
				{
					m_item.Count = item.Count;
					UpdateItemCount(item.Count);
					return;
				}
			}
			return;
		}
	}

	public void UpdateItemCount(int amount, bool includeX = true, bool showIf1 = false, bool capAt99 = false)
	{
		if (m_amountContainer == null)
		{
			m_amountContainer = m_amount.rectTransform;
		}
		if (amount > 99)
		{
			if (capAt99)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						TextMeshProUGUI amount2 = m_amount;
						object str;
						if (includeX)
						{
							str = "x";
						}
						else
						{
							str = string.Empty;
						}
						amount2.text = (string)str + "99+";
						UIManager.SetGameObjectActive(m_amountContainer, true);
						return;
					}
					}
				}
			}
		}
		if (amount <= 1)
		{
			if (showIf1)
			{
				if (amount == 1)
				{
					goto IL_00ad;
				}
			}
			UIManager.SetGameObjectActive(m_amountContainer, false);
			return;
		}
		goto IL_00ad;
		IL_00ad:
		TextMeshProUGUI amount3 = m_amount;
		object arg;
		if (includeX)
		{
			arg = "x";
		}
		else
		{
			arg = string.Empty;
		}
		amount3.text = string.Concat(arg, amount);
		UIManager.SetGameObjectActive(m_amountContainer, true);
	}
}
