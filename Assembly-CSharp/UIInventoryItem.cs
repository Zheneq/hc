using System;
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
		this.m_tooltipHoverObj = this.m_hitbox.GetComponent<UITooltipHoverObject>();
		this.m_tooltipHoverObj.Setup(TooltipType.InventoryItem, new TooltipPopulateCall(this.TooltipSetup), null);
	}

	public InventoryItemTemplate GetItemTemplate()
	{
		return this.m_itemTemplate;
	}

	public void Setup(InventoryItemTemplate itemTemplate, InventoryItem item)
	{
		this.m_itemTemplate = itemTemplate;
		this.m_item = item;
		int amount = 1;
		if (item != null)
		{
			amount = item.Count;
		}
		if (this.m_mainAnimator != null && this.m_mainAnimator.isActiveAndEnabled)
		{
			this.m_mainAnimator.Play("InventoryItemSalvageIDLE");
		}
		this.MakeBorderError(false);
		this.SetDimmed(itemTemplate == null);
		this.UpdateItemCount(amount, true, false, false);
		string spritePath = InventoryWideData.GetSpritePath(itemTemplate);
		if (!spritePath.IsNullOrEmpty())
		{
			this.m_itemImage.sprite = (Sprite)Resources.Load(spritePath, typeof(Sprite));
		}
		UIManager.SetGameObjectActive(this.m_itemImage, !spritePath.IsNullOrEmpty(), null);
		if (this.m_itemFg != null)
		{
			this.m_itemFg.sprite = InventoryWideData.GetItemFg(itemTemplate);
			UIManager.SetGameObjectActive(this.m_itemFg, this.m_itemFg.sprite != null, null);
		}
		Component bgCommon = this.m_bgCommon;
		bool doActive;
		if (itemTemplate != null)
		{
			doActive = (itemTemplate.Rarity == InventoryItemRarity.Common);
		}
		else
		{
			doActive = false;
		}
		UIManager.SetGameObjectActive(bgCommon, doActive, null);
		Component bgUncommon = this.m_bgUncommon;
		bool doActive2;
		if (itemTemplate != null)
		{
			doActive2 = (itemTemplate.Rarity == InventoryItemRarity.Uncommon);
		}
		else
		{
			doActive2 = false;
		}
		UIManager.SetGameObjectActive(bgUncommon, doActive2, null);
		Component bgRare = this.m_bgRare;
		bool doActive3;
		if (itemTemplate != null)
		{
			doActive3 = (itemTemplate.Rarity == InventoryItemRarity.Rare);
		}
		else
		{
			doActive3 = false;
		}
		UIManager.SetGameObjectActive(bgRare, doActive3, null);
		UIManager.SetGameObjectActive(this.m_bgEpic, itemTemplate != null && itemTemplate.Rarity == InventoryItemRarity.Epic, null);
		Component bgLegendary = this.m_bgLegendary;
		bool doActive4;
		if (itemTemplate != null)
		{
			doActive4 = (itemTemplate.Rarity == InventoryItemRarity.Legendary);
		}
		else
		{
			doActive4 = false;
		}
		UIManager.SetGameObjectActive(bgLegendary, doActive4, null);
		if (this.m_shadowCommon != null)
		{
			UIManager.SetGameObjectActive(this.m_shadowCommon, itemTemplate != null && itemTemplate.Rarity == InventoryItemRarity.Common, null);
			Component shadowUncommon = this.m_shadowUncommon;
			bool doActive5;
			if (itemTemplate != null)
			{
				doActive5 = (itemTemplate.Rarity == InventoryItemRarity.Uncommon);
			}
			else
			{
				doActive5 = false;
			}
			UIManager.SetGameObjectActive(shadowUncommon, doActive5, null);
			Component shadowRare = this.m_shadowRare;
			bool doActive6;
			if (itemTemplate != null)
			{
				doActive6 = (itemTemplate.Rarity == InventoryItemRarity.Rare);
			}
			else
			{
				doActive6 = false;
			}
			UIManager.SetGameObjectActive(shadowRare, doActive6, null);
			Component shadowEpic = this.m_shadowEpic;
			bool doActive7;
			if (itemTemplate != null)
			{
				doActive7 = (itemTemplate.Rarity == InventoryItemRarity.Epic);
			}
			else
			{
				doActive7 = false;
			}
			UIManager.SetGameObjectActive(shadowEpic, doActive7, null);
			Component shadowLegendary = this.m_shadowLegendary;
			bool doActive8;
			if (itemTemplate != null)
			{
				doActive8 = (itemTemplate.Rarity == InventoryItemRarity.Legendary);
			}
			else
			{
				doActive8 = false;
			}
			UIManager.SetGameObjectActive(shadowLegendary, doActive8, null);
		}
		if (this.m_tooltipHoverObj != null)
		{
			this.m_tooltipHoverObj.Refresh();
		}
	}

	public void SetDimmed(bool isDimmed)
	{
		for (int i = 0; i < this.m_dimmables.Length; i++)
		{
			this.m_dimmables[i].alpha = ((!isDimmed) ? 1f : 0.25f);
		}
	}

	public void SetCollected(bool isCollected)
	{
		if (this.m_mainAnimator == null)
		{
			return;
		}
		if (!this.m_mainAnimator.isInitialized)
		{
		}
		else
		{
			this.m_mainAnimator.SetBool("IsCollected", isCollected);
			if (!isCollected)
			{
				this.m_mainAnimator.Play("InventoryItemCollectedDISABLE");
			}
			else
			{
				this.m_mainAnimator.Play("InventoryItemCollectedIDLE");
			}
		}
	}

	private bool TooltipSetup(UITooltipBase tooltip)
	{
		if (this.m_itemTemplate == null)
		{
			return false;
		}
		(tooltip as UIInventoryItemTooltip).Setup(this.m_itemTemplate);
		return true;
	}

	public void MakeBorderError(bool makeError)
	{
		if (this.m_normalBorder != null)
		{
			UIManager.SetGameObjectActive(this.m_normalBorder, !makeError, null);
		}
		if (this.m_redBorder != null)
		{
			UIManager.SetGameObjectActive(this.m_redBorder, makeError, null);
		}
	}

	public int GetTemplateId()
	{
		if (this.m_itemTemplate == null)
		{
			return -1;
		}
		return this.m_itemTemplate.Index;
	}

	public int GetItemId()
	{
		if (this.m_item == null)
		{
			return -1;
		}
		return this.m_item.Id;
	}

	public void UpdateItemData(InventoryItem item)
	{
		if (this.m_item != null)
		{
			if (item.TemplateId == this.m_item.TemplateId)
			{
				if (this.m_item.Count != item.Count)
				{
					this.m_item.Count = item.Count;
					this.UpdateItemCount(item.Count, true, false, false);
				}
				return;
			}
		}
	}

	public void UpdateItemCount(int amount, bool includeX = true, bool showIf1 = false, bool capAt99 = false)
	{
		if (this.m_amountContainer == null)
		{
			this.m_amountContainer = this.m_amount.rectTransform;
		}
		if (amount > 0x63)
		{
			if (capAt99)
			{
				TMP_Text amount2 = this.m_amount;
				string str;
				if (includeX)
				{
					str = "x";
				}
				else
				{
					str = string.Empty;
				}
				amount2.text = str + "99+";
				UIManager.SetGameObjectActive(this.m_amountContainer, true, null);
				return;
			}
		}
		if (amount <= 1)
		{
			if (showIf1)
			{
				if (amount == 1)
				{
					goto IL_AD;
				}
			}
			UIManager.SetGameObjectActive(this.m_amountContainer, false, null);
			return;
		}
		IL_AD:
		TMP_Text amount3 = this.m_amount;
		string arg;
		if (includeX)
		{
			arg = "x";
		}
		else
		{
			arg = string.Empty;
		}
		amount3.text = arg + amount;
		UIManager.SetGameObjectActive(this.m_amountContainer, true, null);
	}
}
