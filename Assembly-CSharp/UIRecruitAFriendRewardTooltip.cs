using System;
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
		this.m_rewardDescriptionLabel.text = tooltipText;
		this.m_tooltipTitle.text = tooltipTitle;
		this.m_rewardImage.sprite = Resources.Load<Sprite>(iconPath);
	}
}
