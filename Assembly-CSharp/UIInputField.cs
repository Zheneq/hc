using System;
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
		if (this.m_textField != null)
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIInputField.Awake()).MethodHandle;
			}
			this.m_textField.color = new Color(this.m_TextComponent.color.r, this.m_TextComponent.color.g, this.m_TextComponent.color.b, 1f);
			this.m_TextComponent.color = new Color(this.m_TextComponent.color.r, this.m_TextComponent.color.g, this.m_TextComponent.color.b, 0f);
		}
	}

	public override void OnSelect(BaseEventData eventData)
	{
		base.OnSelect(eventData);
		this.m_unhighlightEnteredText = true;
	}

	public void UpdateTextFieldProperties()
	{
		if (this.m_textField != null)
		{
			this.m_textField.color = new Color(this.m_TextComponent.color.r, this.m_TextComponent.color.g, this.m_TextComponent.color.b, 1f);
			this.m_textField.text = this.m_TextComponent.text;
			(this.m_textField.transform as RectTransform).anchoredPosition = (this.m_TextComponent.transform as RectTransform).anchoredPosition;
			(this.m_textField.transform as RectTransform).sizeDelta = (this.m_TextComponent.transform as RectTransform).sizeDelta;
			this.m_TextComponent.color = new Color(this.m_TextComponent.color.r, this.m_TextComponent.color.g, this.m_TextComponent.color.b, 0f);
		}
		if (this.m_placeholderField != null)
		{
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIInputField.UpdateTextFieldProperties()).MethodHandle;
			}
			(this.m_placeholderField.transform as RectTransform).anchoredPosition = (this.m_Placeholder.transform as RectTransform).anchoredPosition;
			(this.m_placeholderField.transform as RectTransform).sizeDelta = (this.m_Placeholder.transform as RectTransform).sizeDelta;
		}
	}

	protected override void LateUpdate()
	{
		base.LateUpdate();
		if (this.m_unhighlightEnteredText)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIInputField.LateUpdate()).MethodHandle;
			}
			base.MoveTextEnd(false);
			this.m_unhighlightEnteredText = false;
		}
		if (this.m_textField != null)
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
			if (this.m_textField.text != this.m_TextComponent.text)
			{
				this.m_textField.text = this.m_TextComponent.text;
			}
		}
		if (this.m_placeholderField != null && this.m_placeholderField.enabled != this.m_Placeholder.enabled)
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
			this.m_placeholderField.enabled = this.m_Placeholder.enabled;
			this.m_Placeholder.color = Color.clear;
		}
	}
}
