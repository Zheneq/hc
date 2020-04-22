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
		if (!(m_tooltipObject != null))
		{
			return;
		}
		while (true)
		{
			m_tooltipObject.Setup(TooltipType.StatusEffect, PopulateTooltip);
			return;
		}
	}

	private bool PopulateTooltip(UITooltipBase tooltip)
	{
		UIBuffTooltip uIBuffTooltip = tooltip as UIBuffTooltip;
		uIBuffTooltip.Setup(m_statusType, m_duration);
		return true;
	}

	public void Setup(StatusType statusType, int duration)
	{
		m_statusType = statusType;
		m_duration = duration;
		UIManager.SetGameObjectActive(m_buffGainedMiniIcon, false);
		UIManager.SetGameObjectActive(m_debuffGainedMiniIcon, false);
		UIManager.SetGameObjectActive(m_debuffContainer, false);
		UIManager.SetGameObjectActive(m_buffContainer, false);
		HUD_UIResources.StatusTypeIcon iconForStatusType = HUD_UIResources.GetIconForStatusType(statusType);
		if (iconForStatusType.displayIcon)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					m_buffGainedIcon.sprite = iconForStatusType.icon;
					m_debuffGainedIcon.sprite = iconForStatusType.icon;
					UIManager.SetGameObjectActive(m_buffContainer, !iconForStatusType.isDebuff);
					UIManager.SetGameObjectActive(m_debuffContainer, iconForStatusType.isDebuff);
					return;
				}
			}
		}
		UIManager.SetGameObjectActive(m_buffContainer, false);
		UIManager.SetGameObjectActive(m_debuffContainer, false);
	}
}
