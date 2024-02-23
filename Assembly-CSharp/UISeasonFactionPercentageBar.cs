using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UISeasonFactionPercentageBar : MonoBehaviour
{
	public Image m_bar;

	public TextMeshProUGUI m_percentageText;

	public float Setup(float startPortion, float endPortion, Color factionColor)
	{
		if (endPortion > 1f)
		{
			endPortion = 1f;
		}
		float num = (endPortion - startPortion) * 100f;
		m_percentageText.text = new StringBuilder().Append(num.ToString("0.0")).Append("%").ToString();
		m_percentageText.CalculateLayoutInputHorizontal();
		RectTransform rectTransform = base.transform as RectTransform;
		float num2 = 0f;
		while (true)
		{
			if (rectTransform.parent != null)
			{
				rectTransform = (rectTransform.parent as RectTransform);
				float num3 = num2;
				Vector2 sizeDelta = rectTransform.sizeDelta;
				num2 = num3 + sizeDelta.x;
				Vector2 anchorMin = rectTransform.anchorMin;
				float x = anchorMin.x;
				Vector2 anchorMax = rectTransform.anchorMax;
				if (x == anchorMax.x)
				{
					break;
				}
				continue;
			}
			break;
		}
		float num4 = num * num2 / 100f;
		if (m_percentageText.preferredWidth > num4)
		{
			num4 = m_percentageText.preferredWidth / num2;
			endPortion = startPortion + num4;
			if (endPortion > 1f)
			{
				startPortion = 1f - num4;
				endPortion = 1f;
			}
		}
		RectTransform rectTransform2 = m_bar.rectTransform;
		float x2 = startPortion;
		Vector2 anchorMin2 = m_bar.rectTransform.anchorMin;
		rectTransform2.anchorMin = new Vector2(x2, anchorMin2.y);
		RectTransform rectTransform3 = m_bar.rectTransform;
		float x3 = endPortion;
		Vector2 anchorMax2 = m_bar.rectTransform.anchorMax;
		rectTransform3.anchorMax = new Vector2(x3, anchorMax2.y);
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		if (startPortion > 0f)
		{
			zero.x = -5f;
		}
		if (endPortion < 1f)
		{
			zero2.x = 5f;
		}
		m_bar.rectTransform.offsetMin = zero;
		m_bar.rectTransform.offsetMax = zero2;
		m_bar.color = factionColor;
		return endPortion;
	}
}
