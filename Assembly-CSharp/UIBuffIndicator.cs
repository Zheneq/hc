using System;
using UnityEngine;
using UnityEngine.UI;

public class UIBuffIndicator : MonoBehaviour
{
	public CanvasGroup m_cGroup;

	public RectTransform m_buffContainer;

	public RectTransform m_debuffContainer;

	public Image m_buffGainedIcon;

	public Image m_debuffGainedIcon;

	public Image m_buffGainedMiniIcon;

	public Image m_debuffGainedMiniIcon;

	public UITooltipHoverObject m_tooltipObject;

	private StatusType m_statusType;

	private int m_duration;

	private void Start()
	{
		if (this.m_tooltipObject != null)
		{
			this.m_tooltipObject.Setup(TooltipType.StatusEffect, new TooltipPopulateCall(this.PopulateTooltip), null);
		}
	}

	private bool PopulateTooltip(UITooltipBase tooltip)
	{
		UIBuffTooltip uibuffTooltip = tooltip as UIBuffTooltip;
		uibuffTooltip.Setup(this.m_statusType, this.m_duration);
		return true;
	}

	public void Setup(StatusType statusType, int duration)
	{
		this.m_statusType = statusType;
		this.m_duration = duration;
		UIManager.SetGameObjectActive(this.m_buffGainedMiniIcon, false, null);
		UIManager.SetGameObjectActive(this.m_debuffGainedMiniIcon, false, null);
		UIManager.SetGameObjectActive(this.m_debuffContainer, false, null);
		UIManager.SetGameObjectActive(this.m_buffContainer, false, null);
		HUD_UIResources.StatusTypeIcon iconForStatusType = HUD_UIResources.GetIconForStatusType(statusType);
		if (iconForStatusType.displayIcon)
		{
			this.m_buffGainedIcon.sprite = iconForStatusType.icon;
			this.m_debuffGainedIcon.sprite = iconForStatusType.icon;
			UIManager.SetGameObjectActive(this.m_buffContainer, !iconForStatusType.isDebuff, null);
			UIManager.SetGameObjectActive(this.m_debuffContainer, iconForStatusType.isDebuff, null);
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_buffContainer, false, null);
			UIManager.SetGameObjectActive(this.m_debuffContainer, false, null);
		}
	}
}
