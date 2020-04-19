using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIAutocompleteList : MonoBehaviour
{
	public GridLayoutGroup m_gridContent;

	public UIAutocompleteEntry m_entryPrefab;

	public CanvasGroup m_canvasGroup;

	public RectTransform m_selectionBox;

	private const int kMaxViewportSize = 0xA;

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
		if (this.m_isInitialized)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAutocompleteList.Initialize()).MethodHandle;
			}
			return;
		}
		this.m_isInitialized = true;
		this.m_rectTransform = (base.transform as RectTransform);
		this.m_scrollRect = base.GetComponent<ScrollRect>();
		this.m_visibilityQueue = new Queue<UIAutocompleteEntry>();
		UIAutocompleteEntry[] componentsInChildren = this.m_gridContent.GetComponentsInChildren<UIAutocompleteEntry>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnClick);
			componentsInChildren[i].m_hitbox.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		this.m_slots = new List<UIAutocompleteEntry>(componentsInChildren);
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
		this.m_parent = parent;
		this.m_beforeAutocomplete = beforeAutocomplete;
		this.m_caratPosition = this.m_parent.m_textInput.caretPosition;
		this.Initialize();
		for (int i = 0; i < this.m_slots.Count; i++)
		{
			this.m_slots[i].gameObject.SetActive(false);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIAutocompleteList.Setup(UITextConsole, List<string>, string)).MethodHandle;
		}
		this.m_visibilityQueue.Clear();
		this.m_scrollRect.verticalScrollbar.value = 1f;
		for (int j = 0; j < autocompleteEntries.Count; j++)
		{
			UIAutocompleteEntry uiautocompleteEntry;
			if (j < this.m_slots.Count)
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
				uiautocompleteEntry = this.m_slots[j];
			}
			else
			{
				uiautocompleteEntry = UnityEngine.Object.Instantiate<UIAutocompleteEntry>(this.m_entryPrefab);
				uiautocompleteEntry.transform.SetParent(this.m_gridContent.transform);
				uiautocompleteEntry.transform.localPosition = Vector3.zero;
				uiautocompleteEntry.transform.localScale = Vector3.one;
				uiautocompleteEntry.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnClick);
				uiautocompleteEntry.m_hitbox.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
				uiautocompleteEntry.gameObject.SetActive(false);
				this.m_slots.Add(uiautocompleteEntry);
			}
			this.m_visibilityQueue.Enqueue(uiautocompleteEntry);
			uiautocompleteEntry.m_hitbox.ResetMouseState();
			uiautocompleteEntry.Setup(autocompleteEntries[j], autocompleteEntries[j]);
		}
		Vector3 v = this.m_rectTransform.sizeDelta;
		v.y = (float)this.m_gridContent.padding.vertical + this.m_gridContent.cellSize.y * (float)Mathf.Min(autocompleteEntries.Count, 0xA);
		this.m_rectTransform.sizeDelta = v;
		this.m_selectionIndex = 0;
		this.m_selectionBox.gameObject.SetActive(false);
	}

	private void SelectEntry(UIAutocompleteEntry entry)
	{
		string text = this.m_parent.m_textInput.text.Substring(this.m_parent.m_textInput.selectionFocusPosition);
		string text2 = entry.GetTextValue().TrimEnd(new char[0]) + " " + text.Trim();
		if (!this.m_beforeAutocomplete.IsNullOrEmpty())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAutocompleteList.SelectEntry(UIAutocompleteEntry)).MethodHandle;
			}
			text2 = this.m_beforeAutocomplete + " " + text2;
		}
		this.m_parent.m_textInput.text = text2;
		this.m_parent.m_textInput.Select();
		this.m_parent.m_textInput.ActivateInputField();
		this.m_parent.DehighlightTextAndPositionCarat();
		this.m_parent.Show();
		this.SetVisible(false);
	}

	public void SelectCurrent()
	{
		this.SelectEntry(this.m_slots[this.m_selectionIndex]);
	}

	private void OnClick(BaseEventData data)
	{
		this.m_parent.m_textInput.Select();
		this.m_parent.m_textInput.ActivateInputField();
		this.m_parent.DehighlightTextAndPositionCarat();
		this.m_parent.Show();
		this.SetVisible(false);
	}

	private void OnScroll(BaseEventData data)
	{
		this.m_scrollRect.OnScroll((PointerEventData)data);
	}

	private void Update()
	{
		if (!this.m_selectionBox.gameObject.activeSelf)
		{
			this.m_selectionBox.gameObject.SetActive(true);
			this.SetupCurrentSelection();
		}
		int num = 3;
		while (this.m_visibilityQueue.Count > 0)
		{
			this.m_visibilityQueue.Dequeue().gameObject.SetActive(true);
			if (--num <= 0)
			{
				return;
			}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIAutocompleteList.Update()).MethodHandle;
		}
	}

	public void SelectUp()
	{
		if (--this.m_selectionIndex < 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAutocompleteList.SelectUp()).MethodHandle;
			}
			this.m_selectionIndex = 0;
		}
		else
		{
			this.SetupCurrentSelection();
		}
	}

	public void SelectDown()
	{
		if (++this.m_selectionIndex >= this.m_slots.Count)
		{
			this.m_selectionIndex = this.m_slots.Count - 1;
		}
		while (!this.m_slots[this.m_selectionIndex].isActiveAndEnabled)
		{
			this.m_selectionIndex--;
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIAutocompleteList.SelectDown()).MethodHandle;
		}
		this.SetupCurrentSelection();
	}

	private void SetupCurrentSelection()
	{
		UIAutocompleteEntry uiautocompleteEntry = this.m_slots[this.m_selectionIndex];
		RectTransform rectTransform = uiautocompleteEntry.transform as RectTransform;
		this.m_selectionBox.position = rectTransform.position;
		this.m_selectionBox.sizeDelta = rectTransform.sizeDelta;
		int num = this.m_caratPosition;
		num = Mathf.Min(this.m_parent.m_textInput.text.Length, num);
		string text = this.m_parent.m_textInput.text.Substring(0, num);
		string str = this.m_parent.m_textInput.text.Substring(this.m_parent.m_textInput.selectionFocusPosition);
		string text2 = uiautocompleteEntry.GetTextValue().Trim();
		string text3 = string.Empty;
		while (text2.Length > 0)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAutocompleteList.SetupCurrentSelection()).MethodHandle;
			}
			if (text.ToLower().EndsWith(text2.ToLower()))
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					goto IL_144;
				}
			}
			else
			{
				text3 = text2[text2.Length - 1] + text3;
				text2 = text2.Remove(text2.Length - 1, 1);
			}
		}
		IL_144:
		this.m_parent.IgnoreNextTextChange();
		this.m_parent.m_textInput.text = text + text3 + " " + str;
		this.m_parent.m_textInput.selectionAnchorPosition = this.m_caratPosition;
		this.m_parent.m_textInput.selectionFocusPosition = this.m_caratPosition + text3.Length + 1;
	}
}
