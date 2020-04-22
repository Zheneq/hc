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
		if (m_isInitialized)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_isInitialized = true;
		m_purchaseableItems = GetPurchasableItems();
		if (HitchDetector.Get() != null)
		{
			HitchDetector.Get().AddNewLayoutGroup(m_itemsGrid);
		}
		m_pageNum = 0;
		m_itemBtns = m_itemsGrid.GetComponentsInChildren<UICashShopItemBtn>(true);
		m_numberOfPages = m_purchaseableItems.Length / m_itemBtns.Length;
		if (m_purchaseableItems.Length % m_itemBtns.Length > 0)
		{
			m_numberOfPages++;
		}
		if (m_pageMarkers == null)
		{
			m_pageMarkers = new List<UIStorePageIndicator>();
		}
		for (int i = m_pageMarkers.Count; i < m_numberOfPages; i++)
		{
			UIStorePageIndicator uIStorePageIndicator = Object.Instantiate(m_pageItemPrefab);
			uIStorePageIndicator.transform.SetParent(m_pageListContainer.transform);
			uIStorePageIndicator.transform.localScale = Vector3.one;
			uIStorePageIndicator.transform.localPosition = Vector3.zero;
			uIStorePageIndicator.SetSelected(i == 0);
			uIStorePageIndicator.m_hitbox.callback = PageClicked;
			uIStorePageIndicator.m_hitbox.RegisterScrollListener(OnScroll);
			uIStorePageIndicator.SetPageNumber(i + 1);
			m_pageMarkers.Add(uIStorePageIndicator);
		}
		int num;
		for (num = m_numberOfPages; num < m_pageMarkers.Count; num++)
		{
			Object.Destroy(m_pageMarkers[num].gameObject);
			m_pageMarkers.RemoveAt(num);
			num--;
		}
		m_nextPage.transform.parent.SetAsLastSibling();
		m_prevPage.callback = ClickedOnPrevPage;
		m_nextPage.callback = ClickedOnNextPage;
		UIManager.SetGameObjectActive(m_pageListContainer, m_numberOfPages > 1);
		for (int j = 0; j < m_itemBtns.Length; j++)
		{
			m_itemBtns[j].m_selectableBtn.spriteController.RegisterScrollListener(OnScroll);
			StaggerComponent.SetStaggerComponent(m_itemBtns[j].gameObject, true);
			UICashShopItemBtn obj = m_itemBtns[j];
			object item;
			if (j < m_purchaseableItems.Length)
			{
				item = m_purchaseableItems[j];
			}
			else
			{
				item = null;
			}
			obj.Setup((UIPurchaseableItem)item);
		}
	}

	protected void Reinitialize()
	{
		m_isInitialized = false;
		Initialize();
	}

	protected virtual void Start()
	{
		Initialize();
		Image component = GetComponent<Image>();
		if (!(component != null))
		{
			return;
		}
		while (true)
		{
			UIEventTriggerUtils.AddListener(component.gameObject, EventTriggerType.Scroll, OnScroll);
			return;
		}
	}

	private void OnScroll(BaseEventData data)
	{
		PointerEventData pointerEventData = data as PointerEventData;
		Vector2 scrollDelta = pointerEventData.scrollDelta;
		if (scrollDelta.y > 0f)
		{
			ClickedOnPrevPage(null);
			return;
		}
		Vector2 scrollDelta2 = pointerEventData.scrollDelta;
		if (!(scrollDelta2.y < 0f))
		{
			return;
		}
		while (true)
		{
			ClickedOnNextPage(null);
			return;
		}
	}

	public void ClickedOnPrevPage(BaseEventData data)
	{
		if (m_pageNum - 1 < 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		m_pageNum--;
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
		ShowPage(m_pageNum);
	}

	public void ClickedOnNextPage(BaseEventData data)
	{
		if (m_pageNum + 1 >= m_numberOfPages)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
		m_pageNum++;
		ShowPage(m_pageNum);
	}

	private void ShowPage(int pageNum)
	{
		for (int i = 0; i < m_pageMarkers.Count; i++)
		{
			m_pageMarkers[i].SetSelected(i == pageNum);
		}
		while (true)
		{
			m_pageNum = pageNum;
			int num = m_itemBtns.Length * pageNum;
			for (int j = 0; j < m_itemBtns.Length; j++)
			{
				UIPurchaseableItem item = null;
				int num2 = j + num;
				if (num2 < m_purchaseableItems.Length)
				{
					item = m_purchaseableItems[num2];
				}
				m_itemBtns[j].Setup(item);
			}
			return;
		}
	}

	public void PageClicked(BaseEventData data)
	{
		UIFrontEnd.PlaySound(FrontEndButtonSounds.GenericSmall);
		int num = 0;
		while (true)
		{
			if (num < m_pageMarkers.Count)
			{
				if (m_pageMarkers[num].m_hitbox.gameObject == (data as PointerEventData).pointerCurrentRaycast.gameObject)
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		ShowPage(num);
	}

	protected void RefreshPage()
	{
		ShowPage(m_pageNum);
	}
}
