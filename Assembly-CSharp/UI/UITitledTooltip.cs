using TMPro;

public class UITitledTooltip : UITooltipBase
{
	public TextMeshProUGUI m_title;

	public TextMeshProUGUI m_rightTitle;

	public TextMeshProUGUI m_text;

	public void Setup(string tooltipTitle, string tooltipText, string rightString = "")
	{
		m_text.text = tooltipText;
		m_title.text = tooltipTitle;
		m_rightTitle.text = rightString;
	}
}
