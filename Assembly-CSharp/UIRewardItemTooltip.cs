using System;
using TMPro;

public class UIRewardItemTooltip : UITooltipBase
{
	public TextMeshProUGUI m_rewardsLabel;

	public void Setup(string rewardText)
	{
		this.m_rewardsLabel.text = rewardText;
	}
}
