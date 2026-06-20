using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIPlayerStatus : MonoBehaviour
{
    [Range(0f, 1f)]
    public float m_greenTimeValue;
    [Range(0f, 1f)]
    public float m_glowTimeValue;
    public int m_numSteps;

    public Button m_hitbox;
    public GridLayoutGroup m_targetingAbilityIconsGrid;
    public GameObject m_targetingAbilityIndicatorPrefab;
    public UITargetingAbilityCatalystPipContainer m_catalystIndicatorPrefab;
    public Image m_lockIcon;
    public Image m_characterIcon;
    public TextMeshProUGUI m_deathText;
    public TextMeshProUGUI m_botIndicator;
    public RectTransform m_timerContainer;
    public Image m_background;
    public Image m_border;
    public Image m_overlay;
    public Image m_skullIcon;
    public Animator m_animationController;

#if EVOS
    public GameObject m_abilityBackground;
    private bool isExtendedCooldownViewApplied;
    private Vector3 m_originalPos;
    private Vector3 m_originalScale;
#endif
    
    private const float rotationSpeed = 1f;
    private ActorData m_actor;
    private bool m_lockStatus;
    private List<UITargetingAbilityIndicator> m_targetingAbilityIndicators = new List<UITargetingAbilityIndicator>();
    private UITargetingAbilityCatalystPipContainer m_catalsystPips;

    public const int c_maxNumTargetingAbilityIndicators = 8;

    public ActorData ActorDataRef => m_actor;

    private void Start()
    {
        UIManager.SetGameObjectActive(m_lockIcon, false);
        UIEventTriggerUtils.AddListener(m_hitbox.gameObject, EventTriggerType.PointerEnter, MouseEntered);
        UIEventTriggerUtils.AddListener(m_hitbox.gameObject, EventTriggerType.PointerExit, MouseExited);
        GameFlowData.s_onGameStateChanged += OnGameStateChanged;
        
#if EVOS
        m_originalPos = gameObject.transform.position;
        m_originalScale = gameObject.transform.localScale;
#endif
    }
    
#if EVOS
    public static bool IsExtendedCooldownViewEnabled() => EvosOptions.Get().GetOption(EvosOptions.ExtendedCooldownView);

    public void UpdateExtendedCooldownView()
    {
        if (IsExtendedCooldownViewEnabled())
        {
            EnableExtendedCooldownView();
        }
        else
        {
            DisableExtendedCooldownView();
        }
    }

    private void EnableExtendedCooldownView()
    {
        if (m_actor == null || isExtendedCooldownViewApplied)
        {
            return;
        }

        // make everything a little more compact
        var scale = m_originalScale;
        scale *= 0.8f;
        gameObject.transform.localScale = scale;
        
        // spread player icons to accomodate ability icons
        var pos = m_originalPos;
        var shift = 1.5f;
        if (Mathf.Abs(m_originalPos.x) > 3.62f) // fifth player
        {
            shift += 7.98f;
            pos.y = 4.1f;
        }
        pos.x = pos.x * 2.52f + (m_originalPos.x > 0 ? -1 : 1) * shift;
        gameObject.transform.position = pos;
        
        // we need to fit it all on the screen
        m_targetingAbilityIconsGrid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        m_targetingAbilityIconsGrid.constraintCount = 3;
        
        // add background
        Image background = UITooltipManager.Get()
            .m_tooltipPrefabs[(int)TooltipType.Ability]
            ?.gameObject
            .FindInChildren("MainTooltip")
            ?.GetComponentInChildren<Image>();
        if (background != null)
        {
            m_abilityBackground = new GameObject("Ability Background");
            m_abilityBackground.transform.SetParent(gameObject.GetComponent<Transform>(), false);
            m_abilityBackground.GetComponent<Transform>().SetAsFirstSibling();
                    
            Image newImage = m_abilityBackground.AddComponent<Image>();
            newImage.sprite = background.sprite;
            newImage.type = Image.Type.Sliced;
            newImage.rectTransform.sizeDelta = new Vector2(210, 120);
            newImage.rectTransform.anchoredPosition = new Vector2(0, -77);
            newImage.color = new Color(1f, 1f, 1f, 0.5f);

            m_abilityBackground.SetActive(false);
        }
        else
        {
            Log.Error("Failed to draw UIPlayerStatus ability bg");
        }
        
        // update UI when the turn state machine is updated
        if (m_actor != null
            && m_actor.GetActorTurnSM() != null
            && m_actor.GetAbilityData() != null)
        {
            m_actor.GetActorTurnSM().onStateTransition += OnStateTransition;
            m_actor.GetAbilityData().onQueuedAbilitiesChanged += OnQueuedAbilitiesChanged;
            Log.Info($"Hooked into UIPlayerStatus: {m_actor}");
        }
        else
        {
            Log.Error("Failed to hook into UIPlayerStatus: "
                      + $"{m_actor}{m_actor != null}/{m_actor?.GetActorTurnSM() != null}/{m_actor?.GetAbilityData() != null}");
        }

        isExtendedCooldownViewApplied = true;
        ShowAbilities();
    }

    private void DisableExtendedCooldownView()
    {
        if (!isExtendedCooldownViewApplied)
        {
            return;
        }
        
        gameObject.transform.position = m_originalPos;
        gameObject.transform.localScale = m_originalScale;
        m_targetingAbilityIconsGrid.constraint = GridLayoutGroup.Constraint.FixedRowCount;
        m_targetingAbilityIconsGrid.constraintCount = 1;
        
        if (m_abilityBackground != null)
        {
            Destroy(m_abilityBackground);
            m_abilityBackground = null;
        }
        RemoveHooks();
        
        isExtendedCooldownViewApplied = false;
        HideAbilities();
    }
#endif

    private void OnDestroy()
    {
        GameFlowData.s_onGameStateChanged -= OnGameStateChanged;
#if EVOS
        RemoveHooks();
#endif
    }

#if EVOS
    private void RemoveHooks()
    {
        if (m_actor != null)
        {
            if (m_actor.GetActorTurnSM() != null)
            {
                m_actor.GetActorTurnSM().onStateTransition -= OnStateTransition;
            }

            if (m_actor.GetAbilityData() != null)
            {
                m_actor.GetAbilityData().onQueuedAbilitiesChanged -= OnQueuedAbilitiesChanged;
            }
        }
    }
#endif

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.BothTeams_Resolve)
        {
#if EVOS
            if (!isExtendedCooldownViewApplied)
            {
#endif
                UIManager.SetGameObjectActive(m_targetingAbilityIconsGrid, false);
#if EVOS
            }
#endif
        }
#if EVOS
        else if (newState == GameState.BothTeams_Decision && isExtendedCooldownViewApplied)
        {
            m_abilityBackground?.SetActive(true);
            ShowAbilities();
        }
#endif
    }
    
#if EVOS
    private void OnStateTransition(TurnStateEnum newState)
    {
        ShowAbilities();
    }
    
    private void OnQueuedAbilitiesChanged()
    {
        ShowAbilities();
    }

    public void UpdateAbilityVisibility(bool isVisible)
    {
        if (!isExtendedCooldownViewApplied)
        {
            return;
        }
        
        if (isVisible)
        {
            ShowAbilities();
        }
        else
        {
            HideAbilities();
        }
    }
#endif

    public void MouseEntered(BaseEventData data)
    {
        ShowAbilities();
    }

    public void ShowAbilities()
    {
        if (m_actor == null)
        {
            return;
        }

        AbilityData abilityData = m_actor.GetComponent<AbilityData>();
        if (abilityData == null)
        {
            return;
        }

        if (GameFlowData.Get() != null
            && !GameFlowData.Get().IsInDecisionState()
#if EVOS
            && !isExtendedCooldownViewApplied)
#else
        )
#endif
        {
            return;
        }

        int i = 0;
        UIManager.SetGameObjectActive(m_targetingAbilityIconsGrid, true);
        foreach (Ability ability in abilityData.GetAbilitiesAsList())
        {
            AbilityData.ActionType actionTypeOfAbility = abilityData.GetActionTypeOfAbility(ability);
            bool isPrimary = actionTypeOfAbility == AbilityData.ActionType.ABILITY_0;
            bool isUnavailableUltimate = actionTypeOfAbility == AbilityData.ActionType.ABILITY_4
                                         && ability.GetModdedCost() >= m_actor.GetMaxTechPoints();
            bool isAlwaysVisible = !isPrimary && !isUnavailableUltimate;
#if EVOS
            isAlwaysVisible |= isExtendedCooldownViewApplied;
            m_abilityBackground?.SetActive(true);
#endif
            if (abilityData.HasQueuedAction(actionTypeOfAbility) || isAlwaysVisible)
            {
                UpdateTargetingAbilityIndicator(ability, actionTypeOfAbility, i);
                i++;
            }
        }
#if EVOS
        bool isCatalystSelected = false;
#endif

        for (int j = 0; j <= 3; j++)
        {
            AbilityData.ActionType actionType = AbilityData.ActionType.CARD_0 + j;
            Ability ability = abilityData.GetAbilityOfActionType(actionType);
            bool hasQueuedAction = abilityData.HasQueuedAction(actionType);
            if (hasQueuedAction && ability != null)
            {
                UpdateTargetingAbilityIndicator(ability, actionType, i);
                i++;
#if EVOS
                isCatalystSelected = true;
#endif
            }
        }

        for (; i < m_targetingAbilityIndicators.Count; i++)
        {
            UIManager.SetGameObjectActive(m_targetingAbilityIndicators[i], false);
        }

#if EVOS
        // space is limited - either show pips or selected cata
        if (isExtendedCooldownViewApplied)
        {
            if (isCatalystSelected)
            {
                UIManager.SetGameObjectActive(m_catalsystPips, false);
            }
            else
            {
                UpdateCatalysts(abilityData.GetCachedCardAbilities());
                UIManager.SetGameObjectActive(m_catalsystPips, true);
            }
        }
        else
        {
            UpdateCatalysts(abilityData.GetCachedCardAbilities());
        }
#else
        UpdateCatalysts(abilityData.GetCachedCardAbilities());
#endif
    }

    public void UpdateCatalysts(List<Ability> cardAbilities)
    {
        if (m_catalsystPips == null)
        {
            return;
        }

        bool prepActive = false;
        bool dashActive = false;
        bool blastActive = false;
        foreach (Ability ability in cardAbilities)
        {
            if (ability == null)
            {
                continue;
            }

            AbilityRunPhase abilityRunPhase = Card.AbilityPriorityToRunPhase(ability.GetRunPriority());
            switch (abilityRunPhase)
            {
                case AbilityRunPhase.Prep:
                    prepActive = true;
                    break;
                case AbilityRunPhase.Dash:
                    dashActive = true;
                    break;
                case AbilityRunPhase.Combat:
                    blastActive = true;
                    break;
            }
        }

        UIManager.SetGameObjectActive(m_catalsystPips.m_PrepPhaseOn, prepActive);
        UIManager.SetGameObjectActive(m_catalsystPips.m_DashPhaseOn, dashActive);
        UIManager.SetGameObjectActive(m_catalsystPips.m_BlastPhaseOn, blastActive);
        UIUtils.SetAsLastSiblingIfNeeded(m_catalsystPips.transform);
    }

    public void MouseExited(BaseEventData data)
    {
#if EVOS
        if (!isExtendedCooldownViewApplied)
        {
            HideAbilities();
        }
#else
        HideAbilities();
#endif
    }
    
    public void HideAbilities()
    {
        UIManager.SetGameObjectActive(m_targetingAbilityIconsGrid, false);
    }

    public bool IsActiveDisplay()
    {
        return m_actor != null;
    }

    public void Setup(ActorData actor)
    {
        m_actor = actor;
        if (!IsActiveDisplay())
        {
            UIManager.SetGameObjectActive(this, false);
        }
    }

    public void NotifyLockedIn(bool isLocked)
    {
        if (m_lockStatus == isLocked || m_actor == null)
        {
            return;
        }

        ActorTurnSM actorTurnSM = m_actor.GetComponent<ActorTurnSM>();
        if (actorTurnSM.AmStillDeciding())
        {
            UIManager.SetGameObjectActive(m_lockIcon, false);
        }
        else if (m_actor.IsDead())
        {
            UIManager.SetGameObjectActive(m_lockIcon, false);
        }
        else if (!m_lockIcon.gameObject.activeSelf)
        {
            UIManager.SetGameObjectActive(m_lockIcon, true);
            m_animationController.Play("PlayerStatusLockIn");
        }

        m_lockStatus = isLocked;
    }

    public Team GetTeam()
    {
        return m_actor != null ? m_actor.GetTeam() : Team.Invalid;
    }

    private void Update()
    {
        if (m_actor == null)
        {
            UIManager.SetGameObjectActive(this, false);
            return;
        }

        if (m_actor.IsDead())
        {
            m_deathText.text = string.Empty;
            UIManager.SetGameObjectActive(m_border, true);
            UIManager.SetGameObjectActive(m_overlay, true);
            UIManager.SetGameObjectActive(m_skullIcon, true);
        }
        else
        {
            m_deathText.text = string.Empty;
            UIManager.SetGameObjectActive(m_border, false);
            UIManager.SetGameObjectActive(m_overlay, false);
            UIManager.SetGameObjectActive(m_skullIcon, false);
        }

        UpdateInfo();
    }

    private void UpdateInfo()
    {
        if (m_actor != null)
        {
            m_characterIcon.sprite = m_actor.GetCharacterResourceLink().GetCharacterSelectIcon();
            bool hasBotController = m_actor.HasBotController;
            UIManager.SetGameObjectActive(m_botIndicator, hasBotController);
        }
    }

    private void OnEnable()
    {
        Update();
    }

    public void UpdateTargetingAbilityIndicator(Ability ability, AbilityData.ActionType action, int index)
    {
        if (index < c_maxNumTargetingAbilityIndicators)
        {
            while (m_targetingAbilityIndicators.Count <= index)
            {
                GameObject indicator = Instantiate(m_targetingAbilityIndicatorPrefab);
                UITargetingAbilityIndicator component = indicator.GetComponent<UITargetingAbilityIndicator>();
                component.transform.SetParent(m_targetingAbilityIconsGrid.transform);
                component.transform.localScale = Vector3.one;
                component.transform.localPosition = Vector3.zero;
                component.transform.localEulerAngles = Vector3.zero;
                m_targetingAbilityIndicators.Add(component);
            }

            m_targetingAbilityIndicators[index].Setup(m_actor, ability, action);
            if (!m_targetingAbilityIndicators[index].gameObject.activeSelf)
            {
                UIManager.SetGameObjectActive(m_targetingAbilityIndicators[index], true);
            }
        }

        if (m_catalsystPips == null)
        {
            m_catalsystPips = Instantiate(m_catalystIndicatorPrefab);
            m_catalsystPips.transform.SetParent(m_targetingAbilityIconsGrid.transform);
            m_catalsystPips.transform.localScale = Vector3.one;
            m_catalsystPips.transform.localPosition = Vector3.zero;
            m_catalsystPips.transform.localEulerAngles = Vector3.zero;
        }

        UIManager.SetGameObjectActive(m_catalsystPips, true);
        UIUtils.SetAsLastSiblingIfNeeded(m_catalsystPips.transform);
    }

    public void TurnOffTargetingAbilityIndicator(int fromIndex)
    {
        for (int i = fromIndex; i < m_targetingAbilityIndicators.Count; i++)
        {
            if (m_targetingAbilityIndicators[i].gameObject.activeSelf)
            {
                UIManager.SetGameObjectActive(m_targetingAbilityIndicators[i], false);
            }
        }

        if (fromIndex == 0)
        {
            UIManager.SetGameObjectActive(m_catalsystPips, false);
        }
    }
}
