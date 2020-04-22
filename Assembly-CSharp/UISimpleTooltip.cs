using TMPro;

public class UISimpleTooltip : UITooltipBase
{
	public TextMeshProUGUI m_text;

	public void Setup(string text)
	{
		m_text.text = text;
	}
}
