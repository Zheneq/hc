using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAutocompleteList : MonoBehaviour
{
	public GridLayoutGroup m_gridContent;

	public UIAutocompleteEntry m_entryPrefab;

	public CanvasGroup m_canvasGroup;

	public RectTransform m_selectionBox;

	private const int kMaxViewportSize = 10;

	private UITextConsole m_parent;

	private ScrollRect m_scrollRect;

	private RectTransform m_rectTransform;

	private string m_beforeAutocomplete;

	private bool m_isInitialized;

	private int m_caratPosition;

	private int m_selectionIndex;

	private List<UIAutocompleteEntry> m_slots;

	private Queue<UIAutocompleteEntry> m_visibilityQueue;

	private void Initialize()
	{
		if (m_isInitialized)
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
		m_isInitialized = true;
		m_rectTransform = (base.transform as RectTransform);
		m_scrollRect = GetComponent<ScrollRect>();
		m_visibilityQueue = new Queue<UIAutocompleteEntry>();
		UIAutocompleteEntry[] componentsInChildren = m_gridContent.GetComponentsInChildren<UIAutocompleteEntry>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].m_hitbox.callback = OnClick;
			componentsInChildren[i].m_hitbox.RegisterScrollListener(OnScroll);
		}
		while (true)
		{
			m_slots = new List<UIAutocompleteEntry>(componentsInChildren);
			return;
		}
	}

	public void SetVisible(bool visible)
	{
		base.gameObject.SetActive(visible);
	}

	public bool IsVisible()
	{
		return base.gameObject.activeSelf;
	}

	public void Setup(UITextConsole parent, List<string> autocompleteEntries, string beforeAutocomplete)
	{
		m_parent = parent;
		m_beforeAutocomplete = beforeAutocomplete;
		m_caratPosition = m_parent.m_textInput.caretPosition;
		Initialize();
		for (int i = 0; i < m_slots.Count; i++)
		{
			m_slots[i].gameObject.SetActive(false);
		}
		while (true)
		{
			m_visibilityQueue.Clear();
			m_scrollRect.verticalScrollbar.value = 1f;
			for (int j = 0; j < autocompleteEntries.Count; j++)
			{
				UIAutocompleteEntry uIAutocompleteEntry;
				if (j < m_slots.Count)
				{
					uIAutocompleteEntry = m_slots[j];
				}
				else
				{
					uIAutocompleteEntry = Object.Instantiate(m_entryPrefab);
					uIAutocompleteEntry.transform.SetParent(m_gridContent.transform);
					uIAutocompleteEntry.transform.localPosition = Vector3.zero;
					uIAutocompleteEntry.transform.localScale = Vector3.one;
					uIAutocompleteEntry.m_hitbox.callback = OnClick;
					uIAutocompleteEntry.m_hitbox.RegisterScrollListener(OnScroll);
					uIAutocompleteEntry.gameObject.SetActive(false);
					m_slots.Add(uIAutocompleteEntry);
				}
				m_visibilityQueue.Enqueue(uIAutocompleteEntry);
				uIAutocompleteEntry.m_hitbox.ResetMouseState();
				uIAutocompleteEntry.Setup(autocompleteEntries[j], autocompleteEntries[j]);
			}
			Vector3 v = m_rectTransform.sizeDelta;
			float num = m_gridContent.padding.vertical;
			Vector2 cellSize = m_gridContent.cellSize;
			v.y = num + cellSize.y * (float)Mathf.Min(autocompleteEntries.Count, 10);
			m_rectTransform.sizeDelta = v;
			m_selectionIndex = 0;
			m_selectionBox.gameObject.SetActive(false);
			return;
		}
	}

	private void SelectEntry(UIAutocompleteEntry entry)
	{
		string text = m_parent.m_textInput.text.Substring(m_parent.m_textInput.selectionFocusPosition);
		string text2 = new StringBuilder().Append(entry.GetTextValue().TrimEnd()).Append(" ").Append(text.Trim()).ToString();
		if (!m_beforeAutocomplete.IsNullOrEmpty())
		{
			text2 = new StringBuilder().Append(m_beforeAutocomplete).Append(" ").Append(text2).ToString();
		}
		m_parent.m_textInput.text = text2;
		m_parent.m_textInput.Select();
		m_parent.m_textInput.ActivateInputField();
		m_parent.DehighlightTextAndPositionCarat();
		m_parent.Show();
		SetVisible(false);
	}

	public void SelectCurrent()
	{
		SelectEntry(m_slots[m_selectionIndex]);
	}

	private void OnClick(BaseEventData data)
	{
		m_parent.m_textInput.Select();
		m_parent.m_textInput.ActivateInputField();
		m_parent.DehighlightTextAndPositionCarat();
		m_parent.Show();
		SetVisible(false);
	}

	private void OnScroll(BaseEventData data)
	{
		m_scrollRect.OnScroll((PointerEventData)data);
	}

	private void Update()
	{
		if (!m_selectionBox.gameObject.activeSelf)
		{
			m_selectionBox.gameObject.SetActive(true);
			SetupCurrentSelection();
		}
		int num = 3;
		while (m_visibilityQueue.Count > 0)
		{
			m_visibilityQueue.Dequeue().gameObject.SetActive(true);
			if (--num <= 0)
			{
				return;
			}
		}
		while (true)
		{
			return;
		}
	}

	public void SelectUp()
	{
		if (--m_selectionIndex < 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_selectionIndex = 0;
					return;
				}
			}
		}
		SetupCurrentSelection();
	}

	public void SelectDown()
	{
		if (++m_selectionIndex >= m_slots.Count)
		{
			m_selectionIndex = m_slots.Count - 1;
		}
		while (!m_slots[m_selectionIndex].isActiveAndEnabled)
		{
			m_selectionIndex--;
		}
		while (true)
		{
			SetupCurrentSelection();
			return;
		}
	}

	private void SetupCurrentSelection()
	{
		UIAutocompleteEntry uIAutocompleteEntry = m_slots[m_selectionIndex];
		RectTransform rectTransform = uIAutocompleteEntry.transform as RectTransform;
		m_selectionBox.position = rectTransform.position;
		m_selectionBox.sizeDelta = rectTransform.sizeDelta;
		int caratPosition = m_caratPosition;
		caratPosition = Mathf.Min(m_parent.m_textInput.text.Length, caratPosition);
		string text = m_parent.m_textInput.text.Substring(0, caratPosition);
		string str = m_parent.m_textInput.text.Substring(m_parent.m_textInput.selectionFocusPosition);
		string text2 = uIAutocompleteEntry.GetTextValue().Trim();
		string text3 = string.Empty;
		while (text2.Length > 0)
		{
			if (!text.ToLower().EndsWith(text2.ToLower()))
			{
				text3 = new StringBuilder().Append(text2[text2.Length - 1]).Append(text3).ToString();
				text2 = text2.Remove(text2.Length - 1, 1);
				continue;
			}
			break;
		}
		m_parent.IgnoreNextTextChange();
		m_parent.m_textInput.text = new StringBuilder().Append(text).Append(text3).Append(" ").Append(str).ToString();
		m_parent.m_textInput.selectionAnchorPosition = m_caratPosition;
		m_parent.m_textInput.selectionFocusPosition = m_caratPosition + text3.Length + 1;
	}
}
