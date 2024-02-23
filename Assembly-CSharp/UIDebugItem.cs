using System.Text;
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
		UIEventTriggerUtils.AddListener(m_increaseButton.gameObject, EventTriggerType.PointerClick, OnIncreaseClick);
		UIEventTriggerUtils.AddListener(m_decreaseButton.gameObject, EventTriggerType.PointerClick, OnDecreaseClick);
		UIEventTriggerUtils.AddListener(m_increaseButton.gameObject, EventTriggerType.Scroll, OnScroll);
		UIEventTriggerUtils.AddListener(m_decreaseButton.gameObject, EventTriggerType.Scroll, OnScroll);
	}

	public void Setup(DebugCommand listener, ScrollRect scrollRect)
	{
		m_listener = listener;
		m_itemName.text = listener.GetDebugItemName();
		if (listener._001D() != 0)
		{
			TextMeshProUGUI itemName = m_itemName;
			string text = itemName.text;
			itemName.text = new StringBuilder().Append(text).Append(" (").Append(GetModifierKeyPrefix(listener)).Append(listener._001D().ToString()).Append(")").ToString();
		}
		m_itemValue.text = listener.GetDebugItemValue();
		m_increaseLabel.text = listener.GetIncreaseString();
		m_decreaseLabel.text = listener._0013();
		m_increaseButton.gameObject.SetActive(m_listener._0016());
		m_decreaseButton.gameObject.SetActive(m_listener.DisplayDecreaseButton());
		m_scrollRect = scrollRect;
	}

	private void OnScroll(BaseEventData data)
	{
		m_scrollRect.OnScroll((PointerEventData)data);
	}

	private string GetModifierKeyPrefix(DebugCommand listener)
	{
		string text = string.Empty;
		if (listener != null)
		{
			if (listener._000E())
			{
				text += "Ctrl+";
			}
			if (listener._0012())
			{
				text += "Alt+";
			}
			if (listener._0015())
			{
				text += "Shift+";
			}
		}
		return text;
	}

	private void Update()
	{
		if (m_listener == null)
		{
			return;
		}
		while (true)
		{
			m_itemValue.text = m_listener.GetDebugItemValue();
			return;
		}
	}

	private void OnIncreaseClick(BaseEventData data)
	{
		if (m_listener == null)
		{
			return;
		}
		while (true)
		{
			if (DebugCommands.Get() != null)
			{
				DebugCommands.Get().OnIncreaseClick(m_listener);
			}
			return;
		}
	}

	private void OnDecreaseClick(BaseEventData data)
	{
		if (m_listener == null)
		{
			return;
		}
		while (true)
		{
			if (DebugCommands.Get() != null)
			{
				while (true)
				{
					DebugCommands.Get().OnDecreaseClick(m_listener);
					return;
				}
			}
			return;
		}
	}
}
