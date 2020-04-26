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
		RectTransform rectTransform = m_border.transform as RectTransform;
		m_maxWidth = rectTransform.rect.width;
		m_minHeight = rectTransform.rect.height;
		RectTransform rectTransform2 = m_textControl.transform as RectTransform;
		m_extraWidth = 30f;
		float height = rectTransform.rect.height;
		float height2 = rectTransform2.rect.height;
		Vector3 localScale = rectTransform2.localScale;
		m_extraHeight = height - height2 * localScale.y;
		m_lastStringValue = "Uninitialized";
	}

	private void Update()
	{
		if (m_textControl != null)
		{
			if (m_lastStringValue != m_textControl.text)
			{
				goto IL_0054;
			}
		}
		if (!doItAgain)
		{
			return;
		}
		goto IL_0054;
		IL_0054:
		RectTransform rectTransform = m_textControl.transform as RectTransform;
		doItAgain = !doItAgain;
		m_lastStringValue = m_textControl.text;
		m_textControl.CalculateLayoutInputHorizontal();
		m_textControl.CalculateLayoutInputVertical();
		float num = 0f;
		float num2 = 0f;
		if (m_textControl.preferredWidth / 2f < m_maxWidth - m_extraWidth)
		{
			float preferredWidth = m_textControl.preferredWidth;
			Vector3 localScale = rectTransform.localScale;
			num = preferredWidth * localScale.x + m_extraWidth + 10f;
			num2 = m_minHeight;
		}
		else
		{
			num = m_maxWidth;
			float preferredHeight = m_textControl.preferredHeight;
			Vector3 localScale2 = rectTransform.localScale;
			num2 = preferredHeight * localScale2.y + m_extraHeight + 10f;
		}
		if (m_doNotContractWidth)
		{
			num = m_maxWidth;
		}
		if (m_calcTextHeight)
		{
			RectTransform obj = m_textControl.transform as RectTransform;
			Vector2 sizeDelta = (m_textControl.transform as RectTransform).sizeDelta;
			obj.sizeDelta = new Vector2(sizeDelta.x, num2 * 2f - m_extraHeight - 10f);
		}
		RectTransform rectTransform2 = m_border.transform as RectTransform;
		Vector2 sizeDelta2 = rectTransform2.sizeDelta = new Vector2(num, num2);
		if (m_childrenToSize == null)
		{
			return;
		}
		while (true)
		{
			for (int i = 0; i < m_childrenToSize.Length; i++)
			{
				m_childrenToSize[i].sizeDelta = sizeDelta2;
			}
			while (true)
			{
				switch (7)
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
