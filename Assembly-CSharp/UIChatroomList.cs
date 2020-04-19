using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIChatroomList : MonoBehaviour
{
	public GridLayoutGroup m_gridContent;

	public UIAutocompleteEntry m_entryPrefab;

	public CanvasGroup m_canvasGroup;

	private const int kMaxViewportSize = 0xA;

	private UITextConsole m_parent;

	private ScrollRect m_scrollRect;

	private RectTransform m_rectTransform;

	private bool m_isInitialized;

	private List<GameObject> m_clickListenerExceptions;

	private void Initialize()
	{
		if (this.m_isInitialized)
		{
			return;
		}
		this.m_isInitialized = true;
		this.m_rectTransform = (base.transform as RectTransform);
		this.m_scrollRect = base.GetComponent<ScrollRect>();
		this.m_clickListenerExceptions = new List<GameObject>();
		this.m_clickListenerExceptions.Add(this.m_parent.m_chatroomHitbox.gameObject);
		this.m_clickListenerExceptions.Add(base.gameObject);
		UIAutocompleteEntry[] componentsInChildren = this.m_gridContent.GetComponentsInChildren<UIAutocompleteEntry>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnClick);
			componentsInChildren[i].m_hitbox.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
			this.m_clickListenerExceptions.Add(componentsInChildren[i].m_hitbox.gameObject);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIChatroomList.Initialize()).MethodHandle;
		}
	}

	private void OnDestroy()
	{
		if (UIClickListener.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIChatroomList.OnDestroy()).MethodHandle;
			}
			UIClickListener.Get().Disable();
		}
	}

	public bool IsVisible()
	{
		return base.gameObject.activeSelf;
	}

	public void SetVisible(bool visible)
	{
		UIManager.SetGameObjectActive(base.gameObject, visible, null);
		if (UIClickListener.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIChatroomList.SetVisible(bool)).MethodHandle;
			}
			if (!visible)
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
				UIClickListener.Get().Disable();
			}
			else
			{
				UIClickListener.Get().Enable(this.m_clickListenerExceptions, delegate
				{
					this.SetVisible(false);
				});
			}
		}
	}

	public void Setup(UITextConsole parent)
	{
		this.m_parent = parent;
		this.Initialize();
		List<string> availableChatRooms = parent.GetAvailableChatRooms();
		UIAutocompleteEntry[] componentsInChildren = this.m_gridContent.GetComponentsInChildren<UIAutocompleteEntry>(true);
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			UIManager.SetGameObjectActive(componentsInChildren[i], false, null);
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIChatroomList.Setup(UITextConsole)).MethodHandle;
		}
		for (int j = 0; j < availableChatRooms.Count; j++)
		{
			UIAutocompleteEntry uiautocompleteEntry;
			if (j < componentsInChildren.Length)
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
				uiautocompleteEntry = componentsInChildren[j];
			}
			else
			{
				uiautocompleteEntry = UnityEngine.Object.Instantiate<UIAutocompleteEntry>(this.m_entryPrefab);
				uiautocompleteEntry.transform.SetParent(this.m_gridContent.transform);
				uiautocompleteEntry.transform.localPosition = Vector3.zero;
				uiautocompleteEntry.transform.localScale = Vector3.one;
				uiautocompleteEntry.m_hitbox.callback = new _ButtonSwapSprite.ButtonClickCallback(this.OnClick);
				uiautocompleteEntry.m_hitbox.RegisterScrollListener(new UIEventTriggerUtils.EventDelegate(this.OnScroll));
				this.m_clickListenerExceptions.Add(uiautocompleteEntry.m_hitbox.gameObject);
			}
			UIManager.SetGameObjectActive(uiautocompleteEntry, true, null);
			uiautocompleteEntry.m_hitbox.ResetMouseState();
			uiautocompleteEntry.Setup(availableChatRooms[j].Substring(1).ToInitialCharUpper(), availableChatRooms[j]);
		}
		Vector3 v = this.m_rectTransform.sizeDelta;
		v.y = (float)this.m_gridContent.padding.vertical + this.m_gridContent.cellSize.y * (float)Mathf.Min(availableChatRooms.Count, 0xA);
		this.m_rectTransform.sizeDelta = v;
	}

	private void OnClick(BaseEventData data)
	{
		PointerEventData pointerEventData = (PointerEventData)data;
		if (pointerEventData.button != PointerEventData.InputButton.Left)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIChatroomList.OnClick(BaseEventData)).MethodHandle;
			}
			return;
		}
		UIAutocompleteEntry componentInParent = data.selectedObject.GetComponentInParent<UIAutocompleteEntry>();
		this.m_parent.m_textInput.text = componentInParent.GetTextValue() + " " + this.m_parent.m_textInput.text;
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
}
