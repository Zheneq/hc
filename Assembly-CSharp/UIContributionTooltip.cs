using System;
using TMPro;

public class UIContributionTooltip : UITooltipBase
{
	public TextMeshProUGUI m_nameLabel;

	public TextMeshProUGUI m_descriptionLabel;

	public void Setup(string name, string description)
	{
		this.m_nameLabel.text = name;
		this.m_descriptionLabel.text = description;
	}
}
