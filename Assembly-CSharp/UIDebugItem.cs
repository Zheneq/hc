using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIDebugItem : MonoBehaviour
{
	public Button m_increaseButton;

	public Button m_decreaseButton;

	public TextMeshProUGUI m_itemName;

	public TextMeshProUGUI m_itemValue;

	private DebugCommand m_listener;

	public TextMeshProUGUI m_increaseLabel;

	public TextMeshProUGUI m_decreaseLabel;

	public ScrollRect m_scrollRect;

	private void Start()
	{
		UIEventTriggerUtils.AddListener(this.m_increaseButton.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnIncreaseClick));
		UIEventTriggerUtils.AddListener(this.m_decreaseButton.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnDecreaseClick));
		UIEventTriggerUtils.AddListener(this.m_increaseButton.gameObject, EventTriggerType.Scroll, new UIEventTriggerUtils.EventDelegate(this.OnScroll));
		UIEventTriggerUtils.AddListener(this.m_decreaseButton.gameObject, EventTriggerType.Scroll, new UIEventTriggerUtils.EventDelegate(this.OnScroll));
	}

	public void Setup(DebugCommand listener, ScrollRect scrollRect)
	{
		this.m_listener = listener;
		this.m_itemName.text = listener.GetDebugItemName();
		if (listener.symbol_001D() != KeyCode.None)
		{
			TextMeshProUGUI itemName = this.m_itemName;
			string text = itemName.text;
			itemName.text = string.Concat(new string[]
			{
				text,
				" (",
				this.GetModifierKeyPrefix(listener),
				listener.symbol_001D().ToString(),
				")"
			});
		}
		this.m_itemValue.text = listener.GetDebugItemValue();
		this.m_increaseLabel.text = listener.GetIncreaseString();
		this.m_decreaseLabel.text = listener.symbol_0013();
		this.m_increaseButton.gameObject.SetActive(this.m_listener.symbol_0016());
		this.m_decreaseButton.gameObject.SetActive(this.m_listener.DisplayDecreaseButton());
		this.m_scrollRect = scrollRect;
	}

	private void OnScroll(BaseEventData data)
	{
		this.m_scrollRect.OnScroll((PointerEventData)data);
	}

	private string GetModifierKeyPrefix(DebugCommand listener)
	{
		string text = string.Empty;
		if (listener != null)
		{
			if (listener.symbol_000E())
			{
				text += "Ctrl+";
			}
			if (listener.symbol_0012())
			{
				text += "Alt+";
			}
			if (listener.symbol_0015())
			{
				text += "Shift+";
			}
		}
		return text;
	}

	private void Update()
	{
		if (this.m_listener != null)
		{
			this.m_itemValue.text = this.m_listener.GetDebugItemValue();
		}
	}

	private void OnIncreaseClick(BaseEventData data)
	{
		if (this.m_listener != null)
		{
			if (DebugCommands.Get() != null)
			{
				DebugCommands.Get().OnIncreaseClick(this.m_listener);
			}
		}
	}

	private void OnDecreaseClick(BaseEventData data)
	{
		if (this.m_listener != null)
		{
			if (DebugCommands.Get() != null)
			{
				DebugCommands.Get().OnDecreaseClick(this.m_listener);
			}
		}
	}
}
