using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UITargetingAbilityIndicator : MonoBehaviour
{
    public Button m_mouseHitBox;
    public Image m_abilityIconImage;
    public TextMeshProUGUI m_cooldownLabel;

    public GameObject m_cooldownState;
    public GameObject m_defaultState;
    public GameObject m_hoverState;
    public GameObject m_queuedState;
    public GameObject m_targetingState;

    public Image m_phaseColorImage;
    public Image m_targetingPhaseColorImage;
    public Color m_prepPhaseColor = Color.green;
    public Color m_evasionPhaseColor = Color.yellow;
    public Color m_combatPhaseColor = Color.red;

    private Ability m_ability;
    private CanvasGroup m_canvasGroup;
    private int m_cachedCooldown;
    private int m_cachedStocksRemaining;
    private int m_cachedStockRefreshCountdown;
    private bool m_cachedAbilityUseAllowed;
    private bool m_cachedIsQueued;
    private bool m_cachedIsTargeting;
    private bool m_cachedEnergyOk;

    private void Awake()
    {
        m_canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        if (m_mouseHitBox != null)
        {
            UIEventTriggerUtils.AddListener(m_mouseHitBox.gameObject, EventTriggerType.PointerEnter, MouseEnter);
            UIEventTriggerUtils.AddListener(m_mouseHitBox.gameObject, EventTriggerType.PointerExit, MouseExit);
            m_mouseHitBox.GetComponent<UITooltipHoverObject>().Setup(TooltipType.Ability, ShowTooltip);
        }
    }

    public void SetCanvasGroupVisibility(bool visible)
    {
        if (m_canvasGroup != null)
        {
            if (visible)
            {
                m_canvasGroup.alpha = 1f;
                m_canvasGroup.blocksRaycasts = true;
                m_canvasGroup.interactable = true;
            }
            else
            {
                m_canvasGroup.alpha = 0f;
                m_canvasGroup.blocksRaycasts = false;
                m_canvasGroup.interactable = false;
            }
        }
    }

    private bool ShowTooltip(UITooltipBase tooltip)
    {
        if (m_ability == null)
        {
            return false;
        }

        UIAbilityTooltip uIAbilityTooltip = (UIAbilityTooltip)tooltip;
        uIAbilityTooltip.Setup(m_ability, m_ability.CurrentAbilityMod);
        return true;
    }

    public void MouseEnter(BaseEventData data)
    {
        if (m_hoverState != null)
        {
            UIManager.SetGameObjectActive(m_hoverState, true);
            m_hoverState.GetComponent<CanvasGroup>().alpha = 1f;
        }
    }

    public void MouseExit(BaseEventData data)
    {
        if (m_hoverState != null)
        {
            UIManager.SetGameObjectActive(m_hoverState, false);
            m_hoverState.GetComponent<CanvasGroup>().alpha = 0f;
        }
    }

    public void OnDisable()
    {
        if (m_hoverState != null)
        {
            UIManager.SetGameObjectActive(m_hoverState, false);
            m_hoverState.GetComponent<CanvasGroup>().alpha = 0f;
        }
    }

    public void Setup(ActorData actor, Ability ability, AbilityData.ActionType action)
    {
        if (actor == null
            || actor.GetAbilityData() == null
            || ability == null)
        {
            return;
        }

        AbilityData abilityData = actor.GetAbilityData();
        bool isAlly = GameFlowData.Get().LocalPlayerData.IsViewingTeam(actor.GetTeam());
        bool isSpectator = ClientGameManager.Get().PlayerInfo != null
                           && ClientGameManager.Get().PlayerInfo.IsSpectator
                           && ClientGameManager.Get().SpectatorHideAbilityTargeter;

        bool isQueued = !isSpectator && abilityData.HasQueuedAction(action);
        bool isTargeting = !isSpectator && abilityData.GetSelectedActionTypeForTargeting() == action;
        bool isAllowed = abilityData.IsAbilityAllowedByUnlockTurns(action);
        int cooldownRemaining = abilityData.GetCooldownRemaining(action);
        int stocksRemaining = abilityData.GetStocksRemaining(action);
        int stockRefreshCountdown = abilityData.GetStockRefreshCountdown(action);
        bool isEnergyOk = ability.GetModdedCost() <= actor.GetTechPointsToDisplay();

        if (m_ability == ability
            && m_cachedIsQueued == isQueued
            && m_cachedIsTargeting == isTargeting
            && m_cachedAbilityUseAllowed == isAllowed
            && m_cachedCooldown == cooldownRemaining
            && m_cachedStocksRemaining == stocksRemaining
            && m_cachedStockRefreshCountdown == stockRefreshCountdown
            && m_cachedEnergyOk == isEnergyOk)
        {
            return;
        }

        m_ability = ability;
        m_cachedIsQueued = isQueued;
        m_cachedIsTargeting = isTargeting;
        m_cachedAbilityUseAllowed = isAllowed;
        m_cachedCooldown = cooldownRemaining;
        m_cachedStocksRemaining = stocksRemaining;
        m_cachedStockRefreshCountdown = stockRefreshCountdown;
        m_cachedEnergyOk = isEnergyOk;

        if (m_queuedState != null)
        {
            UIManager.SetGameObjectActive(m_queuedState, false);
        }

        ShowDisabledIndicator(false);
        if (m_targetingState != null)
        {
            UIManager.SetGameObjectActive(m_targetingState, false);
        }

        if (m_defaultState != null)
        {
            UIManager.SetGameObjectActive(m_defaultState, false);
        }

        m_abilityIconImage.sprite = ability.sprite;
        m_cooldownLabel.text = string.Empty;
        if (isAlly && isQueued)
        {
            if (m_queuedState != null)
            {
                UIManager.SetGameObjectActive(m_queuedState, true);
            }

            if (m_phaseColorImage != null)
            {
                SetPhaseImageColor(m_phaseColorImage, m_ability.GetRunPriority());
            }

            m_abilityIconImage.color = HighlightUtils.Get().m_allyAvailableAbilityIconColor;
            return;
        }

        if (isAlly && isTargeting)
        {
            if (m_targetingState != null)
            {
                UIManager.SetGameObjectActive(m_targetingState, true);
            }

            if (m_targetingPhaseColorImage != null)
            {
                SetPhaseImageColor(m_targetingPhaseColorImage, m_ability.GetRunPriority());
            }

            m_abilityIconImage.color = HighlightUtils.Get().m_allyTargetingAbilityIconColor;
            return;
        }

        bool abilityHasStocks = m_ability.GetModdedMaxStocks() > 0;
        if (cooldownRemaining > 0 && !abilityHasStocks)
        {
            ShowDisabledIndicator(true);
            m_abilityIconImage.color = HighlightUtils.Get().m_allyCooldownAbilityIconColor;
            m_cooldownLabel.text = cooldownRemaining.ToString();
            return;
        }

        if (!isAllowed)
        {
            ShowDisabledIndicator(true);
            m_abilityIconImage.color = HighlightUtils.Get().m_allyCooldownAbilityIconColor;
            m_cooldownLabel.text = "1";
            return;
        }

        if (abilityHasStocks && stocksRemaining == 0)
        {
            ShowDisabledIndicator(true);
            m_abilityIconImage.color = HighlightUtils.Get().m_allyCooldownAbilityIconColor;
            m_cooldownLabel.text = stockRefreshCountdown.ToString();
            return;
        }

        if (!isEnergyOk)
        {
            ShowDisabledIndicator(true);
            m_abilityIconImage.color = HighlightUtils.Get().m_allyCooldownAbilityIconColor;
            m_cooldownLabel.text = string.Empty;
            return;
        }

        m_abilityIconImage.color = HighlightUtils.Get().m_allyAvailableAbilityIconColor;
        if (m_defaultState != null)
        {
            UIManager.SetGameObjectActive(m_defaultState, true);
        }
    }

    private void ShowDisabledIndicator(bool show)
    {
        if (m_cooldownState != null)
        {
            UIManager.SetGameObjectActive(m_cooldownState, show);
            CanvasGroup component = m_cooldownState.GetComponent<CanvasGroup>();
            component.alpha = show ? 1 : 0;
        }
    }

    private void SetPhaseImageColor(Image phaseImage, AbilityPriority abilityRunPhase)
    {
        if (abilityRunPhase <= AbilityPriority.Prep_Offense)
        {
            phaseImage.color = m_prepPhaseColor;
        }
        else if (abilityRunPhase <= AbilityPriority.Evasion)
        {
            phaseImage.color = m_evasionPhaseColor;
        }
        else
        {
            phaseImage.color = m_combatPhaseColor;
        }
    }
}