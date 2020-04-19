using System;
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
		this.m_percentageText.text = num.ToString("0.0") + "%";
		this.m_percentageText.CalculateLayoutInputHorizontal();
		RectTransform rectTransform = base.transform as RectTransform;
		float num2 = 0f;
		while (rectTransform.parent != null)
		{
			rectTransform = (rectTransform.parent as RectTransform);
			num2 += rectTransform.sizeDelta.x;
			if (rectTransform.anchorMin.x == rectTransform.anchorMax.x)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UISeasonFactionPercentageBar.Setup(float, float, Color)).MethodHandle;
				}
				IL_D7:
				float num3 = num * num2 / 100f;
				if (this.m_percentageText.preferredWidth > num3)
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
					num3 = this.m_percentageText.preferredWidth / num2;
					endPortion = startPortion + num3;
					if (endPortion > 1f)
					{
						startPortion = 1f - num3;
						endPortion = 1f;
					}
				}
				this.m_bar.rectTransform.anchorMin = new Vector2(startPortion, this.m_bar.rectTransform.anchorMin.y);
				this.m_bar.rectTransform.anchorMax = new Vector2(endPortion, this.m_bar.rectTransform.anchorMax.y);
				Vector2 zero = Vector2.zero;
				Vector2 zero2 = Vector2.zero;
				if (startPortion > 0f)
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
					zero.x = -5f;
				}
				if (endPortion < 1f)
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
					zero2.x = 5f;
				}
				this.m_bar.rectTransform.offsetMin = zero;
				this.m_bar.rectTransform.offsetMax = zero2;
				this.m_bar.color = factionColor;
				return endPortion;
			}
		}
		for (;;)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			goto IL_D7;
		}
	}
}
