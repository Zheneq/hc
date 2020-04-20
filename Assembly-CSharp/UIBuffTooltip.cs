using System;
using TMPro;
using UnityEngine.UI;

public class UIBuffTooltip : UITooltipBase
{
	public Image m_buffIcon;

	public TextMeshProUGUI m_buffNameLabel;

	public TextMeshProUGUI m_buffDescriptionLabel;

	public void Setup(StatusType status, int duration)
	{
		HUD_UIResources.StatusTypeIcon iconForStatusType = HUD_UIResources.GetIconForStatusType(status);
		this.m_buffIcon.sprite = iconForStatusType.icon;
		this.m_buffNameLabel.text = iconForStatusType.buffName;
		string text = iconForStatusType.buffDescription;
		if (duration > 1)
		{
			text = string.Format(StringUtil.TR("TurnsRemaining", "Buffs"), iconForStatusType.buffDescription, duration);
		}
		else if (duration == 1)
		{
			text = string.Format(StringUtil.TR("OneTurnRemaining", "Buffs"), iconForStatusType.buffDescription);
		}
		this.m_buffDescriptionLabel.text = text;
	}
}
