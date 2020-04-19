using System;
using TMPro;

public class UISimpleTooltip : UITooltipBase
{
	public TextMeshProUGUI m_text;

	public void Setup(string text)
	{
		this.m_text.text = text;
	}
}
