using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class _LargeScrollList : MonoBehaviour
{
	public _LargeScrollListItemEntry[] scrollListPrefabTypes;

	public float m_spacing;

	private ScrollRect m_scrollRectComponent;

	private Scrollbar m_scrollBar;

	private List<IDataEntry> m_listReference;

	private float m_totalHeightOfList;

	private float m_totalHeightOfViewArea;

	private List<List<_LargeScrollList.ScrollListItemEntry>> m_prefabBankList;

	private List<_LargeScrollList.ScrollListItemEntry> m_activeEntryList;

	private bool initialized;

	private bool m_scrollable = true;

	private void Init()
	{
		if (!this.initialized)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(_LargeScrollList.Init()).MethodHandle;
			}
			this.initialized = true;
			this.m_scrollRectComponent = base.gameObject.GetComponent<ScrollRect>();
			this.m_scrollBar = this.m_scrollRectComponent.verticalScrollbar;
			RectTransform rectTransform = this.m_scrollRectComponent.content.gameObject.transform as RectTransform;
			rectTransform.anchorMin = new Vector2(0.5f, 1f);
			rectTransform.anchorMax = new Vector2(0.5f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			this.m_prefabBankList = new List<List<_LargeScrollList.ScrollListItemEntry>>();
			this.m_activeEntryList = new List<_LargeScrollList.ScrollListItemEntry>();
			for (int i = 0; i < this.scrollListPrefabTypes.Length; i++)
			{
				List<_LargeScrollList.ScrollListItemEntry> list = new List<_LargeScrollList.ScrollListItemEntry>();
				_LargeScrollListItemEntry largeScrollListItemEntry = UnityEngine.Object.Instantiate<_LargeScrollListItemEntry>(this.scrollListPrefabTypes[i]);
				largeScrollListItemEntry.transform.SetParent(this.m_scrollRectComponent.content);
				UIManager.SetGameObjectActive(largeScrollListItemEntry, false, null);
				_LargeScrollList.ScrollListItemEntry item;
				item.entryIndex = -1;
				item.prefabTypeIndex = i;
				item.m_theEntry = largeScrollListItemEntry;
				list.Add(item);
				this.m_prefabBankList.Add(list);
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			this.m_scrollBar.onValueChanged.AddListener(new UnityAction<float>(this.ScrollValueChanged));
			this.m_scrollRectComponent.elasticity = 0.01f;
		}
	}

	public ScrollRect GetScrollRect()
	{
		this.Init();
		return this.m_scrollRectComponent;
	}

	private void Start()
	{
		this.Init();
	}

	public void SetScrollable(bool scrollable)
	{
		this.m_scrollable = scrollable;
	}

	public List<_LargeScrollList.ScrollListItemEntry> GetVisibleListEntries()
	{
		return this.m_activeEntryList;
	}

	private bool IsEntryDisplayed(int index)
	{
		for (int i = 0; i < this.m_activeEntryList.Count; i++)
		{
			if (this.m_activeEntryList[i].entryIndex == index)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(_LargeScrollList.IsEntryDisplayed(int)).MethodHandle;
				}
				return true;
			}
		}
		return false;
	}

	public void ScrollValueChanged(float value)
	{
		if (!this.m_scrollable)
		{
			return;
		}
		if (this.m_listReference != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(_LargeScrollList.ScrollValueChanged(float)).MethodHandle;
			}
			float num = this.m_totalHeightOfList - this.m_totalHeightOfList * value;
			float height = num - this.m_totalHeightOfViewArea;
			float height2 = num + this.m_totalHeightOfViewArea;
			int num2 = this.GetIndexFromHeight(height);
			int num3 = this.GetIndexFromHeight(height2);
			num2 = Mathf.Max(num2, 0);
			num3 = Mathf.Min(num3, this.m_listReference.Count - 1);
			int i = 0;
			while (i < this.m_activeEntryList.Count)
			{
				int prefabEntryIndexFromItemListIndex = this.GetPrefabEntryIndexFromItemListIndex(this.m_activeEntryList[i].entryIndex);
				if (this.m_activeEntryList[i].entryIndex < num2 || this.m_activeEntryList[i].entryIndex > num3)
				{
					goto IL_10F;
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_activeEntryList[i].prefabTypeIndex != prefabEntryIndexFromItemListIndex)
				{
					for (;;)
					{
						switch (1)
						{
						case 0:
							continue;
						}
						goto IL_10F;
					}
				}
				IL_163:
				i++;
				continue;
				IL_10F:
				_LargeScrollList.ScrollListItemEntry item = this.m_activeEntryList[i];
				this.m_activeEntryList.RemoveAt(i);
				i--;
				item.m_theEntry.SetVisible(false);
				item.entryIndex = -1;
				this.m_prefabBankList[item.prefabTypeIndex].Add(item);
				goto IL_163;
			}
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			for (int j = num2; j < num3 + 1; j++)
			{
				if (!this.IsEntryDisplayed(j))
				{
					int prefabEntryIndexFromItemListIndex2 = this.GetPrefabEntryIndexFromItemListIndex(j);
					if (this.m_prefabBankList[prefabEntryIndexFromItemListIndex2].Count == 0)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						_LargeScrollListItemEntry theEntry = UnityEngine.Object.Instantiate<_LargeScrollListItemEntry>(this.scrollListPrefabTypes[prefabEntryIndexFromItemListIndex2]);
						_LargeScrollList.ScrollListItemEntry item2;
						item2.entryIndex = -1;
						item2.prefabTypeIndex = prefabEntryIndexFromItemListIndex2;
						item2.m_theEntry = theEntry;
						this.m_prefabBankList[prefabEntryIndexFromItemListIndex2].Add(item2);
					}
					if (this.m_prefabBankList[prefabEntryIndexFromItemListIndex2].Count > 0)
					{
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
						_LargeScrollList.ScrollListItemEntry item3 = this.m_prefabBankList[prefabEntryIndexFromItemListIndex2][0];
						item3.m_theEntry.SetVisible(true);
						item3.m_theEntry.SetParent(this.m_scrollRectComponent.content);
						this.m_listReference[j].Setup(j, item3.m_theEntry);
						float heightFromItemIndex = this.GetHeightFromItemIndex(j);
						item3.m_theEntry.SetAnchoredPosition(new Vector2(0f, heightFromItemIndex * -1f));
						this.m_prefabBankList[prefabEntryIndexFromItemListIndex2].Remove(item3);
						item3.entryIndex = j;
						this.m_activeEntryList.Add(item3);
					}
				}
				else
				{
					for (int k = 0; k < this.m_activeEntryList.Count; k++)
					{
						if (this.m_activeEntryList[k].entryIndex == j)
						{
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
							this.m_activeEntryList[k].m_theEntry.SetVisible(true);
							this.m_listReference[j].Setup(j, this.m_activeEntryList[k].m_theEntry);
							float heightFromItemIndex2 = this.GetHeightFromItemIndex(j);
							this.m_activeEntryList[k].m_theEntry.SetAnchoredPosition(new Vector2(0f, heightFromItemIndex2 * -1f));
						}
					}
				}
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}

	private int GetIndexFromHeight(float height)
	{
		float num = 0f;
		int result = 0;
		for (int i = 0; i < this.m_listReference.Count; i++)
		{
			result = i;
			if (num >= height)
			{
				return result;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(_LargeScrollList.GetIndexFromHeight(float)).MethodHandle;
			}
			if (i != 0)
			{
				num += this.m_spacing;
			}
			num += this.GetHeightOfEntry(i);
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			return result;
		}
	}

	private float GetViewHeight()
	{
		return (this.m_scrollRectComponent.transform as RectTransform).sizeDelta.y;
	}

	private float GetHeightFromItemIndex(int index)
	{
		return this.GetHeightDifferenceFromItemIndices(0, index);
	}

	private float GetHeightDifferenceFromItemIndices(int startIndex, int endIndex)
	{
		float num = 0f;
		for (int i = startIndex; i < endIndex; i++)
		{
			num += this.m_spacing;
			num += this.GetHeightOfEntry(i);
		}
		return num;
	}

	public int GetNumTotalEntries()
	{
		return this.m_listReference.Count;
	}

	private int GetPrefabEntryIndexFromItemListIndex(int index)
	{
		if (-1 < index)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(_LargeScrollList.GetPrefabEntryIndexFromItemListIndex(int)).MethodHandle;
			}
			if (index < this.m_listReference.Count)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				return this.m_listReference[index].GetPrefabIndexToDisplay();
			}
		}
		return -0x64;
	}

	private float GetHeightOfEntry(int index)
	{
		int prefabEntryIndexFromItemListIndex = this.GetPrefabEntryIndexFromItemListIndex(index);
		if (-1 < prefabEntryIndexFromItemListIndex)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(_LargeScrollList.GetHeightOfEntry(int)).MethodHandle;
			}
			if (prefabEntryIndexFromItemListIndex < this.scrollListPrefabTypes.Length)
			{
				return this.scrollListPrefabTypes[prefabEntryIndexFromItemListIndex].GetHeight();
			}
		}
		return 0f;
	}

	public IDataEntry GetDataEntry(int index)
	{
		return this.m_listReference[index];
	}

	public void Setup(List<IDataEntry> itemList, int indexToSetView = 0)
	{
		this.Init();
		this.m_listReference = itemList;
		this.m_totalHeightOfList = this.GetHeightFromItemIndex(itemList.Count - 1) + this.GetHeightOfEntry(itemList.Count - 1);
		this.m_totalHeightOfViewArea = (this.m_scrollRectComponent.transform as RectTransform).rect.height * 1.5f;
		this.m_scrollRectComponent.content.sizeDelta = new Vector2(this.m_scrollRectComponent.content.sizeDelta.x, this.m_totalHeightOfList);
		this.SetViewToEntryIndex(indexToSetView);
	}

	public void RefreshEntries()
	{
		if (base.gameObject.activeInHierarchy)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(_LargeScrollList.RefreshEntries()).MethodHandle;
			}
			this.ScrollValueChanged(this.m_scrollBar.value);
		}
	}

	public void SetViewToEntryIndex(int index)
	{
		if (base.gameObject.activeInHierarchy)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(_LargeScrollList.SetViewToEntryIndex(int)).MethodHandle;
			}
			float num = 1f - this.GetHeightFromItemIndex(index) / this.m_totalHeightOfList;
			num = Mathf.Clamp01(num);
			if (this.m_scrollBar.value == num)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				this.ScrollValueChanged(num);
			}
			else
			{
				this.m_scrollBar.value = num;
			}
		}
	}

	public struct ScrollListItemEntry
	{
		public _LargeScrollListItemEntry m_theEntry;

		public int prefabTypeIndex;

		public int entryIndex;
	}
}
