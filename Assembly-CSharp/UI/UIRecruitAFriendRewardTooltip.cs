using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIRecruitAFriendRewardTooltip : UITooltipBase
{
	public TextMeshProUGUI m_tooltipTitle;

	public TextMeshProUGUI m_rewardDescriptionLabel;

	public Image m_rewardImage;

	public void Setup(string tooltipTitle, string tooltipText, string iconPath)
	{
		m_rewardDescriptionLabel.text = tooltipText;
		m_tooltipTitle.text = tooltipTitle;
		m_rewardImage.sprite = Resources.Load<Sprite>(iconPath);
	}
}
