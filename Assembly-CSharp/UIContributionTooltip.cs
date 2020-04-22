using TMPro;

public class UIContributionTooltip : UITooltipBase
{
	public TextMeshProUGUI m_nameLabel;

	public TextMeshProUGUI m_descriptionLabel;

	public void Setup(string name, string description)
	{
		m_nameLabel.text = name;
		m_descriptionLabel.text = description;
	}
}
