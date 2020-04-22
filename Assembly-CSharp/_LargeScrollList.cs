using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class _LargeScrollList : MonoBehaviour
{
	public struct ScrollListItemEntry
	{
		public _LargeScrollListItemEntry m_theEntry;

		public int prefabTypeIndex;

		public int entryIndex;
	}

	public _LargeScrollListItemEntry[] scrollListPrefabTypes;

	public float m_spacing;

	private ScrollRect m_scrollRectComponent;

	private Scrollbar m_scrollBar;

	private List<IDataEntry> m_listReference;

	private float m_totalHeightOfList;

	private float m_totalHeightOfViewArea;

	private List<List<ScrollListItemEntry>> m_prefabBankList;

	private List<ScrollListItemEntry> m_activeEntryList;

	private bool initialized;

	private bool m_scrollable = true;

	private void Init()
	{
		if (initialized)
		{
			return;
		}
		ScrollListItemEntry item = default(ScrollListItemEntry);
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			initialized = true;
			m_scrollRectComponent = base.gameObject.GetComponent<ScrollRect>();
			m_scrollBar = m_scrollRectComponent.verticalScrollbar;
			RectTransform rectTransform = m_scrollRectComponent.content.gameObject.transform as RectTransform;
			rectTransform.anchorMin = new Vector2(0.5f, 1f);
			rectTransform.anchorMax = new Vector2(0.5f, 1f);
			rectTransform.pivot = new Vector2(0.5f, 1f);
			m_prefabBankList = new List<List<ScrollListItemEntry>>();
			m_activeEntryList = new List<ScrollListItemEntry>();
			for (int i = 0; i < scrollListPrefabTypes.Length; i++)
			{
				List<ScrollListItemEntry> list = new List<ScrollListItemEntry>();
				_LargeScrollListItemEntry largeScrollListItemEntry = Object.Instantiate(scrollListPrefabTypes[i]);
				largeScrollListItemEntry.transform.SetParent(m_scrollRectComponent.content);
				UIManager.SetGameObjectActive(largeScrollListItemEntry, false);
				item.entryIndex = -1;
				item.prefabTypeIndex = i;
				item.m_theEntry = largeScrollListItemEntry;
				list.Add(item);
				m_prefabBankList.Add(list);
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				m_scrollBar.onValueChanged.AddListener(ScrollValueChanged);
				m_scrollRectComponent.elasticity = 0.01f;
				return;
			}
		}
	}

	public ScrollRect GetScrollRect()
	{
		Init();
		return m_scrollRectComponent;
	}

	private void Start()
	{
		Init();
	}

	public void SetScrollable(bool scrollable)
	{
		m_scrollable = scrollable;
	}

	public List<ScrollListItemEntry> GetVisibleListEntries()
	{
		return m_activeEntryList;
	}

	private bool IsEntryDisplayed(int index)
	{
		for (int i = 0; i < m_activeEntryList.Count; i++)
		{
			ScrollListItemEntry scrollListItemEntry = m_activeEntryList[i];
			if (scrollListItemEntry.entryIndex != index)
			{
				continue;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return true;
			}
		}
		return false;
	}

	public void ScrollValueChanged(float value)
	{
		if (!m_scrollable || m_listReference == null)
		{
			return;
		}
		ScrollListItemEntry item2 = default(ScrollListItemEntry);
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			float num = m_totalHeightOfList - m_totalHeightOfList * value;
			float height = num - m_totalHeightOfViewArea;
			float height2 = num + m_totalHeightOfViewArea;
			int indexFromHeight = GetIndexFromHeight(height);
			int indexFromHeight2 = GetIndexFromHeight(height2);
			indexFromHeight = Mathf.Max(indexFromHeight, 0);
			indexFromHeight2 = Mathf.Min(indexFromHeight2, m_listReference.Count - 1);
			for (int i = 0; i < m_activeEntryList.Count; i++)
			{
				ScrollListItemEntry scrollListItemEntry = m_activeEntryList[i];
				int prefabEntryIndexFromItemListIndex = GetPrefabEntryIndexFromItemListIndex(scrollListItemEntry.entryIndex);
				ScrollListItemEntry scrollListItemEntry2 = m_activeEntryList[i];
				if (scrollListItemEntry2.entryIndex >= indexFromHeight)
				{
					ScrollListItemEntry scrollListItemEntry3 = m_activeEntryList[i];
					if (scrollListItemEntry3.entryIndex <= indexFromHeight2)
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
						ScrollListItemEntry scrollListItemEntry4 = m_activeEntryList[i];
						if (scrollListItemEntry4.prefabTypeIndex == prefabEntryIndexFromItemListIndex)
						{
							continue;
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
				}
				ScrollListItemEntry item = m_activeEntryList[i];
				m_activeEntryList.RemoveAt(i);
				i--;
				item.m_theEntry.SetVisible(false);
				item.entryIndex = -1;
				m_prefabBankList[item.prefabTypeIndex].Add(item);
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				for (int j = indexFromHeight; j < indexFromHeight2 + 1; j++)
				{
					if (!IsEntryDisplayed(j))
					{
						int prefabEntryIndexFromItemListIndex2 = GetPrefabEntryIndexFromItemListIndex(j);
						if (m_prefabBankList[prefabEntryIndexFromItemListIndex2].Count == 0)
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
							_LargeScrollListItemEntry theEntry = Object.Instantiate(scrollListPrefabTypes[prefabEntryIndexFromItemListIndex2]);
							item2.entryIndex = -1;
							item2.prefabTypeIndex = prefabEntryIndexFromItemListIndex2;
							item2.m_theEntry = theEntry;
							m_prefabBankList[prefabEntryIndexFromItemListIndex2].Add(item2);
						}
						if (m_prefabBankList[prefabEntryIndexFromItemListIndex2].Count > 0)
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
							ScrollListItemEntry item3 = m_prefabBankList[prefabEntryIndexFromItemListIndex2][0];
							item3.m_theEntry.SetVisible(true);
							item3.m_theEntry.SetParent(m_scrollRectComponent.content);
							m_listReference[j].Setup(j, item3.m_theEntry);
							float heightFromItemIndex = GetHeightFromItemIndex(j);
							item3.m_theEntry.SetAnchoredPosition(new Vector2(0f, heightFromItemIndex * -1f));
							m_prefabBankList[prefabEntryIndexFromItemListIndex2].Remove(item3);
							item3.entryIndex = j;
							m_activeEntryList.Add(item3);
						}
						continue;
					}
					for (int k = 0; k < m_activeEntryList.Count; k++)
					{
						ScrollListItemEntry scrollListItemEntry5 = m_activeEntryList[k];
						if (scrollListItemEntry5.entryIndex == j)
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
							ScrollListItemEntry scrollListItemEntry6 = m_activeEntryList[k];
							scrollListItemEntry6.m_theEntry.SetVisible(true);
							IDataEntry dataEntry = m_listReference[j];
							int displayIndex = j;
							ScrollListItemEntry scrollListItemEntry7 = m_activeEntryList[k];
							dataEntry.Setup(displayIndex, scrollListItemEntry7.m_theEntry);
							float heightFromItemIndex2 = GetHeightFromItemIndex(j);
							ScrollListItemEntry scrollListItemEntry8 = m_activeEntryList[k];
							scrollListItemEntry8.m_theEntry.SetAnchoredPosition(new Vector2(0f, heightFromItemIndex2 * -1f));
						}
					}
				}
				while (true)
				{
					switch (2)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	private int GetIndexFromHeight(float height)
	{
		float num = 0f;
		int result = 0;
		int num2 = 0;
		while (true)
		{
			IL_0043:
			if (num2 < m_listReference.Count)
			{
				result = num2;
				if (!(num < height))
				{
					break;
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (num2 != 0)
					{
						num += m_spacing;
					}
					num += GetHeightOfEntry(num2);
					num2++;
					goto IL_0043;
				}
			}
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			break;
		}
		return result;
	}

	private float GetViewHeight()
	{
		Vector2 sizeDelta = (m_scrollRectComponent.transform as RectTransform).sizeDelta;
		return sizeDelta.y;
	}

	private float GetHeightFromItemIndex(int index)
	{
		return GetHeightDifferenceFromItemIndices(0, index);
	}

	private float GetHeightDifferenceFromItemIndices(int startIndex, int endIndex)
	{
		float num = 0f;
		for (int i = startIndex; i < endIndex; i++)
		{
			num += m_spacing;
			num += GetHeightOfEntry(i);
		}
		return num;
	}

	public int GetNumTotalEntries()
	{
		return m_listReference.Count;
	}

	private int GetPrefabEntryIndexFromItemListIndex(int index)
	{
		if (-1 < index)
		{
			while (true)
			{
				switch (6)
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
			if (index < m_listReference.Count)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return m_listReference[index].GetPrefabIndexToDisplay();
					}
				}
			}
		}
		return -100;
	}

	private float GetHeightOfEntry(int index)
	{
		int prefabEntryIndexFromItemListIndex = GetPrefabEntryIndexFromItemListIndex(index);
		if (-1 < prefabEntryIndexFromItemListIndex)
		{
			while (true)
			{
				switch (6)
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
			if (prefabEntryIndexFromItemListIndex < scrollListPrefabTypes.Length)
			{
				return scrollListPrefabTypes[prefabEntryIndexFromItemListIndex].GetHeight();
			}
		}
		return 0f;
	}

	public IDataEntry GetDataEntry(int index)
	{
		return m_listReference[index];
	}

	public void Setup(List<IDataEntry> itemList, int indexToSetView = 0)
	{
		Init();
		m_listReference = itemList;
		m_totalHeightOfList = GetHeightFromItemIndex(itemList.Count - 1) + GetHeightOfEntry(itemList.Count - 1);
		m_totalHeightOfViewArea = (m_scrollRectComponent.transform as RectTransform).rect.height * 1.5f;
		RectTransform content = m_scrollRectComponent.content;
		Vector2 sizeDelta = m_scrollRectComponent.content.sizeDelta;
		content.sizeDelta = new Vector2(sizeDelta.x, m_totalHeightOfList);
		SetViewToEntryIndex(indexToSetView);
	}

	public void RefreshEntries()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			ScrollValueChanged(m_scrollBar.value);
			return;
		}
	}

	public void SetViewToEntryIndex(int index)
	{
		if (!base.gameObject.activeInHierarchy)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			float value = 1f - GetHeightFromItemIndex(index) / m_totalHeightOfList;
			value = Mathf.Clamp01(value);
			if (m_scrollBar.value == value)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						ScrollValueChanged(value);
						return;
					}
				}
			}
			m_scrollBar.value = value;
			return;
		}
	}
}
