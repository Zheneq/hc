using System;
using TMPro;

public class UITitledTooltip : UITooltipBase
{
	public TextMeshProUGUI m_title;

	public TextMeshProUGUI m_rightTitle;

	public TextMeshProUGUI m_text;

	public void Setup(string tooltipTitle, string tooltipText, string rightString = "")
	{
		this.m_text.text = tooltipText;
		this.m_title.text = tooltipTitle;
		this.m_rightTitle.text = rightString;
	}
}
