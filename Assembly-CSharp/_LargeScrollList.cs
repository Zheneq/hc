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

        initialized = true;
        m_scrollRectComponent = gameObject.GetComponent<ScrollRect>();
        m_scrollBar = m_scrollRectComponent.verticalScrollbar;
        RectTransform rectTransform = m_scrollRectComponent.content.gameObject.transform as RectTransform;
        rectTransform.anchorMin = new Vector2(0.5f, 1f);
        rectTransform.anchorMax = new Vector2(0.5f, 1f);
        rectTransform.pivot = new Vector2(0.5f, 1f);
        m_prefabBankList = new List<List<ScrollListItemEntry>>();
        m_activeEntryList = new List<ScrollListItemEntry>();
        ScrollListItemEntry item = default(ScrollListItemEntry);
        for (int i = 0; i < scrollListPrefabTypes.Length; i++)
        {
            List<ScrollListItemEntry> list = new List<ScrollListItemEntry>();
            _LargeScrollListItemEntry largeScrollListItemEntry = Instantiate(scrollListPrefabTypes[i]);
            largeScrollListItemEntry.transform.SetParent(m_scrollRectComponent.content);
            UIManager.SetGameObjectActive(largeScrollListItemEntry, false);
            item.entryIndex = -1;
            item.prefabTypeIndex = i;
            item.m_theEntry = largeScrollListItemEntry;
            list.Add(item);
            m_prefabBankList.Add(list);
        }

        m_scrollBar.onValueChanged.AddListener(ScrollValueChanged);
        m_scrollRectComponent.elasticity = 0.01f;
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
            if (m_activeEntryList[i].entryIndex == index)
            {
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

        float num = m_totalHeightOfList - m_totalHeightOfList * value;
        float height = num - m_totalHeightOfViewArea;
        float height2 = num + m_totalHeightOfViewArea;
        int indexFromHeight = GetIndexFromHeight(height);
        int indexFromHeight2 = GetIndexFromHeight(height2);
        indexFromHeight = Mathf.Max(indexFromHeight, 0);
        indexFromHeight2 = Mathf.Min(indexFromHeight2, m_listReference.Count - 1);
        for (int i = 0; i < m_activeEntryList.Count; i++)
        {
            ScrollListItemEntry entry = m_activeEntryList[i];
            int prefabEntryIndexFromItemListIndex = GetPrefabEntryIndexFromItemListIndex(entry.entryIndex);
            if (entry.entryIndex >= indexFromHeight
                && entry.entryIndex <= indexFromHeight2
                && entry.prefabTypeIndex == prefabEntryIndexFromItemListIndex)
            {
                continue;
            }

            m_activeEntryList.RemoveAt(i);
            i--;
            entry.m_theEntry.SetVisible(false);
            entry.entryIndex = -1;
            m_prefabBankList[entry.prefabTypeIndex].Add(entry);
        }

        for (int i = indexFromHeight; i < indexFromHeight2 + 1; i++)
        {
            if (!IsEntryDisplayed(i))
            {
                int prefabEntryIndexFromItemListIndex2 = GetPrefabEntryIndexFromItemListIndex(i);
                if (m_prefabBankList[prefabEntryIndexFromItemListIndex2].Count == 0)
                {
                    ScrollListItemEntry item2 = default(ScrollListItemEntry);
                    item2.entryIndex = -1;
                    item2.prefabTypeIndex = prefabEntryIndexFromItemListIndex2;
                    item2.m_theEntry = Instantiate(scrollListPrefabTypes[prefabEntryIndexFromItemListIndex2]);
                    m_prefabBankList[prefabEntryIndexFromItemListIndex2].Add(item2);
                }

                if (m_prefabBankList[prefabEntryIndexFromItemListIndex2].Count > 0)
                {
                    ScrollListItemEntry item3 = m_prefabBankList[prefabEntryIndexFromItemListIndex2][0];
                    item3.m_theEntry.SetVisible(true);
                    item3.m_theEntry.SetParent(m_scrollRectComponent.content);
                    m_listReference[i].Setup(i, item3.m_theEntry);
                    item3.m_theEntry.SetAnchoredPosition(new Vector2(0f, GetHeightFromItemIndex(i) * -1f));
                    m_prefabBankList[prefabEntryIndexFromItemListIndex2].Remove(item3);
                    item3.entryIndex = i;
                    m_activeEntryList.Add(item3);
                }
            }
            else
            {
                for (int j = 0; j < m_activeEntryList.Count; j++)
                {
                    if (m_activeEntryList[j].entryIndex == i)
                    {
                        ScrollListItemEntry entry = m_activeEntryList[j];
                        entry.m_theEntry.SetVisible(true);
                        m_listReference[i].Setup(i, entry.m_theEntry);
                        entry.m_theEntry.SetAnchoredPosition(new Vector2(0f, GetHeightFromItemIndex(i) * -1f));
                    }
                }
            }
        }
    }

    private int GetIndexFromHeight(float height)
    {
        float num = 0f;
        int result = 0;
        for (int i = 0; i < m_listReference.Count; i++)
        {
            result = i;
            if (num >= height)
            {
                break;
            }

            if (i != 0)
            {
                num += m_spacing;
            }

            num += GetHeightOfEntry(i);
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
        if (-1 < index && index < m_listReference.Count)
        {
            return m_listReference[index].GetPrefabIndexToDisplay();
        }

        return -100;
    }

    private float GetHeightOfEntry(int index)
    {
        int prefabEntryIndexFromItemListIndex = GetPrefabEntryIndexFromItemListIndex(index);
        if (-1 < prefabEntryIndexFromItemListIndex && prefabEntryIndexFromItemListIndex < scrollListPrefabTypes.Length)
        {
            return scrollListPrefabTypes[prefabEntryIndexFromItemListIndex].GetHeight();
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
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        ScrollValueChanged(m_scrollBar.value);
    }

    public void SetViewToEntryIndex(int index)
    {
        if (!gameObject.activeInHierarchy)
        {
            return;
        }

        float value = Mathf.Clamp01(1f - GetHeightFromItemIndex(index) / m_totalHeightOfList);
        if (m_scrollBar.value == value)
        {
            ScrollValueChanged(value);
            return;
        }

        m_scrollBar.value = value;
    }
}
