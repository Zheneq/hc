using TMPro;

public class UIRewardItemTooltip : UITooltipBase
{
	public TextMeshProUGUI m_rewardsLabel;

	public void Setup(string rewardText)
	{
		m_rewardsLabel.text = rewardText;
	}
}
