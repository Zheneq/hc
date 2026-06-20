#if EVOS
using Evos.ActorStatus;
#endif
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
#if EVOS
    private EvosActorStatusType m_evosStatusType;
#endif
    private int m_duration;

    private void Start()
    {
        if (m_tooltipObject != null)
        {
            m_tooltipObject.Setup(TooltipType.StatusEffect, PopulateTooltip);
        }
    }

    private bool PopulateTooltip(UITooltipBase tooltip)
    {
        UIBuffTooltip uIBuffTooltip = tooltip as UIBuffTooltip;
#if EVOS
        if (m_statusType == StatusType.INVALID)
        {
            uIBuffTooltip.Setup(m_evosStatusType);
        }
        else
        {
            uIBuffTooltip.Setup(m_statusType, m_duration);
        }
#else
        uIBuffTooltip.Setup(m_statusType, m_duration);
#endif
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
            m_buffGainedIcon.sprite = iconForStatusType.icon;
            m_debuffGainedIcon.sprite = iconForStatusType.icon;
            UIManager.SetGameObjectActive(m_buffContainer, !iconForStatusType.isDebuff);
            UIManager.SetGameObjectActive(m_debuffContainer, iconForStatusType.isDebuff);
        }
        else
        {
            UIManager.SetGameObjectActive(m_buffContainer, false);
            UIManager.SetGameObjectActive(m_debuffContainer, false);
        }
    }
    
#if EVOS
    public void Setup(EvosActorStatusType statusType)
    {
        m_statusType = StatusType.INVALID;
        m_evosStatusType = statusType;
        m_duration = -1;
        UIManager.SetGameObjectActive(m_buffGainedMiniIcon, false);
        UIManager.SetGameObjectActive(m_debuffGainedMiniIcon, false);
        UIManager.SetGameObjectActive(m_debuffContainer, false);
        UIManager.SetGameObjectActive(m_buffContainer, false);
        HUD_UIResources.StatusTypeIcon iconForStatusType = EvosActorStatusRepo.GetIconForStatusType(statusType);
        if (iconForStatusType.displayIcon)
        {
            m_buffGainedIcon.sprite = iconForStatusType.icon;
            m_debuffGainedIcon.sprite = iconForStatusType.icon;
            UIManager.SetGameObjectActive(m_buffContainer, !iconForStatusType.isDebuff);
            UIManager.SetGameObjectActive(m_debuffContainer, iconForStatusType.isDebuff);
        }
        else
        {
            UIManager.SetGameObjectActive(m_buffContainer, false);
            UIManager.SetGameObjectActive(m_debuffContainer, false);
        }
    }
#endif
}