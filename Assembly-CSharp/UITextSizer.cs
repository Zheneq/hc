using System;
using TMPro;
using UnityEngine;

public class UITextSizer : MonoBehaviour
{
	private const float m_extraDynamicWidth = 10f;

	private const float m_extraDynamicHeight = 10f;

	public TextMeshProUGUI m_textControl;

	public GameObject m_border;

	public RectTransform[] m_childrenToSize;

	public bool m_doNotContractWidth;

	public bool m_calcTextHeight;

	[HideInInspector]
	private float m_maxWidth;

	private float m_minHeight;

	private float m_extraWidth;

	private float m_extraHeight;

	private string m_lastStringValue;

	private bool doItAgain;

	private void Start()
	{
		RectTransform rectTransform = this.m_border.transform as RectTransform;
		this.m_maxWidth = rectTransform.rect.width;
		this.m_minHeight = rectTransform.rect.height;
		RectTransform rectTransform2 = this.m_textControl.transform as RectTransform;
		this.m_extraWidth = 30f;
		this.m_extraHeight = rectTransform.rect.height - rectTransform2.rect.height * rectTransform2.localScale.y;
		this.m_lastStringValue = "Uninitialized";
	}

	private void Update()
	{
		if (this.m_textControl != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UITextSizer.Update()).MethodHandle;
			}
			if (this.m_lastStringValue != this.m_textControl.text)
			{
				goto IL_54;
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
		}
		if (!this.doItAgain)
		{
			return;
		}
		IL_54:
		RectTransform rectTransform = this.m_textControl.transform as RectTransform;
		this.doItAgain = !this.doItAgain;
		this.m_lastStringValue = this.m_textControl.text;
		this.m_textControl.CalculateLayoutInputHorizontal();
		this.m_textControl.CalculateLayoutInputVertical();
		float x;
		float num;
		if (this.m_textControl.preferredWidth / 2f < this.m_maxWidth - this.m_extraWidth)
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
			x = this.m_textControl.preferredWidth * rectTransform.localScale.x + this.m_extraWidth + 10f;
			num = this.m_minHeight;
		}
		else
		{
			x = this.m_maxWidth;
			num = this.m_textControl.preferredHeight * rectTransform.localScale.y + this.m_extraHeight + 10f;
		}
		if (this.m_doNotContractWidth)
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
			x = this.m_maxWidth;
		}
		if (this.m_calcTextHeight)
		{
			(this.m_textControl.transform as RectTransform).sizeDelta = new Vector2((this.m_textControl.transform as RectTransform).sizeDelta.x, num * 2f - this.m_extraHeight - 10f);
		}
		RectTransform rectTransform2 = this.m_border.transform as RectTransform;
		Vector2 sizeDelta = new Vector2(x, num);
		rectTransform2.sizeDelta = sizeDelta;
		if (this.m_childrenToSize != null)
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
			for (int i = 0; i < this.m_childrenToSize.Length; i++)
			{
				this.m_childrenToSize[i].sizeDelta = sizeDelta;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
	}
}
