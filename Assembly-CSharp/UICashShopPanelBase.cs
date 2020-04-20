using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class UICashShopPanelBase : UIStoreBasePanel
{
	public GridLayoutGroup m_itemsGrid;

	public UIStorePageIndicator m_pageItemPrefab;

	public GridLayoutGroup m_pageListContainer;

	public _ButtonSwapSprite m_prevPage;

	public _ButtonSwapSprite m_nextPage;

	private UIPurchaseableItem[] m_purchaseableItems;

	private UICashShopItemBtn[] m_itemBtns;

	private int m_numOwned;

	private int m_numTotal;

	private int m_pageNum;

	private int m_numberOfPages;

	private List<UIStorePageIndicator> m_pageMarkers;

	private bool m_isInitialized;

	protected abstract UIPurchaseableItem[] GetPurchasableItems();

	private void Initialize()
	{
		if (this.m_isInitialized)
		{
			return;
		}
		this.m_isInitialized = true;
		this.m_purchaseableItems = this.GetPurchasableItems();
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().AddNewLayoutGroup(this.m_itemsGrid);
		}
		this.m_pageNum = 0;
		this.m_itemBtns = this.m_itemsGrid.GetComponentsInChildren<UICashShopItemBtn>(true);
		this.m_numberOfPages = this.m_purchaseableItems.Length / this.m_itemBtns.Length;
		if (this.m_purchaseableItems.Length % this.m_itemBtns.Length > 0)
		{
			this.m_numberOfPages++;
		}
		if (this.m_pageMarkers == null)
		{
			this.m_pageMarkers = new List<UIStorePageIndicator>();
		}
		for (int i = this.m_pageMarkers.Count; i < this.m_numberOfPages; i++)
		{
			UIStorePageIndicator uistorePageIndicator = UnityEngine.Object.Instantiate<UIStorePageIndicator>(this.m_pageItemPrefab);
			uistorePageIndicator.transform.SetParent(this.m_pageListContainer.transform);
			uistorePageIndicator.transform.localScale = Vector3.one;
			uistorePageIndicator.transform.localPosition = Vector3.zero;
			uistorePageIndicator.SetSelected(i == 0);
			uistorePageIndicator.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.PageClicked);
			uistorePageIndicator.m_hitbox.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
			uistorePageIndicator.SetPageNumber(i + 1);
			this.m_pageMarkers.Add(uistorePageIndicator);
		}
		for (int j = this.m_numberOfPages; j < this.m_pageMarkers.Count; j++)
		{
			UnityEngine.Object.Destroy(this.m_pageMarkers[j].gameObject);
			this.m_pageMarkers.RemoveAt(j);
			j--;
		}
		this.m_nextPage.transform.parent.SetAsLastSibling();
		this.m_prevPage.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ClickedOnPrevPage);
		this.m_nextPage.callback = new _ButtonSwapSprite.ButtonClickCallback(this.ClickedOnNextPage);
		UIManager.SetGameObjectActive(this.m_pageListContainer, this.m_numberOfPages > 1, null);
		for (int k = 0; k < this.m_itemBtns.Length; k++)
		{
			this.m_itemBtns[k].m_selectableBtn.spriteController.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
			StaggerComponent.SetStaggerComponent(this.m_itemBtns[k].gameObject, true, true);
			UICashShopItemBtn uicashShopItemBtn = this.m_itemBtns[k];
			UIPurchaseableItem item;
			if (k < this.m_purchaseableItems.Length)
			{
				item = this.m_purchaseableItems[k];
			}
			else
			{
				item = null;
			}
			uicashShopItemBtn.Setup(item);
		}
	}

	protected void Reinitialize()
	{
		this.m_isInitialized = false;
		this.Initialize();
	}

	protected virtual void Start()
	{
		this.Initialize();
		Image component = base.GetComponent<Image>();
		if (component != null)
		{
			UIEventTriggerUtils.AddListener(component.gameObject, EventTriggerType.Scroll, new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
	}

	private void OnScroll(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		if (pointerEventData.scrollDelta.y > 0f)
		{
			this.ClickedOnPrevPage(null);
		}
		else if (pointerEventData.scrollDelta.y < 0f)
		{
			this.ClickedOnNextPage(null);
		}
	}

	public void ClickedOnPrevPage(BaseEventData data)
	{
		if (this.m_pageNum - 1 < 0)
		{
			return;
		}
		this.m_pageNum--;
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
		this.ShowPage(this.m_pageNum);
	}

	public void ClickedOnNextPage(BaseEventData data)
	{
		if (this.m_pageNum + 1 >= this.m_numberOfPages)
		{
			return;
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
		this.m_pageNum++;
		this.ShowPage(this.m_pageNum);
	}

	private void ShowPage(int pageNum)
	{
		for (int i = 0; i < this.m_pageMarkers.Count; i++)
		{
			this.m_pageMarkers[i].SetSelected(i == pageNum);
		}
		this.m_pageNum = pageNum;
		int num = this.m_itemBtns.Length * pageNum;
		for (int j = 0; j < this.m_itemBtns.Length; j++)
		{
			UIPurchaseableItem item = null;
			int num2 = j + num;
			if (num2 < this.m_purchaseableItems.Length)
			{
				item = this.m_purchaseableItems[num2];
			}
			this.m_itemBtns[j].Setup(item);
		}
	}

	public void PageClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
		for (int i = 0; i < this.m_pageMarkers.Count; i++)
		{
			if (this.m_pageMarkers[i].m_hitbox.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
			{
				this.ShowPage(i);
				break;
			}
		}
	}

	protected void RefreshPage()
	{
		this.ShowPage(this.m_pageNum);
	}
}
