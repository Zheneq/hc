using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInputField : InputField
{
	private bool m_unhighlightEnteredText;

	[SerializeField]
	public TextMeshProUGUI m_textField;

	[SerializeField]
	public TextMeshProUGUI m_placeholderField;

	protected override void Awake()
	{
		base.Awake();
		if (!(m_textField != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			TextMeshProUGUI textField = m_textField;
			Color color = m_TextComponent.color;
			float r = color.r;
			Color color2 = m_TextComponent.color;
			float g = color2.g;
			Color color3 = m_TextComponent.color;
			textField.color = new Color(r, g, color3.b, 1f);
			Text textComponent = m_TextComponent;
			Color color4 = m_TextComponent.color;
			float r2 = color4.r;
			Color color5 = m_TextComponent.color;
			float g2 = color5.g;
			Color color6 = m_TextComponent.color;
			textComponent.color = new Color(r2, g2, color6.b, 0f);
			return;
		}
	}

	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		m_unhighlightEnteredText = true;
	}

	public void UpdateTextFieldProperties()
	{
		if (m_textField != null)
		{
			TextMeshProUGUI textField = m_textField;
			Color color = m_TextComponent.color;
			float r = color.r;
			Color color2 = m_TextComponent.color;
			float g = color2.g;
			Color color3 = m_TextComponent.color;
			textField.color = new Color(r, g, color3.b, 1f);
			m_textField.text = m_TextComponent.text;
			(m_textField.transform as RectTransform).anchoredPosition = (m_TextComponent.transform as RectTransform).anchoredPosition;
			(m_textField.transform as RectTransform).sizeDelta = (m_TextComponent.transform as RectTransform).sizeDelta;
			Text textComponent = m_TextComponent;
			Color color4 = m_TextComponent.color;
			float r2 = color4.r;
			Color color5 = m_TextComponent.color;
			float g2 = color5.g;
			Color color6 = m_TextComponent.color;
			textComponent.color = new Color(r2, g2, color6.b, 0f);
		}
		if (!(m_placeholderField != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			(m_placeholderField.transform as RectTransform).anchoredPosition = (m_Placeholder.transform as RectTransform).anchoredPosition;
			(m_placeholderField.transform as RectTransform).sizeDelta = (m_Placeholder.transform as RectTransform).sizeDelta;
			return;
		}
	}

	protected override void LateUpdate()
	{
		base.LateUpdate();
		if (m_unhighlightEnteredText)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			MoveTextEnd(false);
			m_unhighlightEnteredText = false;
		}
		if (m_textField != null)
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
			if (m_textField.text != m_TextComponent.text)
			{
				m_textField.text = m_TextComponent.text;
			}
		}
		if (!(m_placeholderField != null) || m_placeholderField.enabled == m_Placeholder.enabled)
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			m_placeholderField.enabled = m_Placeholder.enabled;
			m_Placeholder.color = Color.clear;
			return;
		}
	}
}
