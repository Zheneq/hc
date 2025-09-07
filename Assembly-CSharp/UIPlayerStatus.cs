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
    }

    private void OnDestroy()
    {
        GameFlowData.s_onGameStateChanged -= OnGameStateChanged;
    }

    private void OnGameStateChanged(GameState newState)
    {
        if (newState == GameState.BothTeams_Resolve)
        {
            UIManager.SetGameObjectActive(m_targetingAbilityIconsGrid, false);
        }
    }

    public void MouseEntered(BaseEventData data)
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

        if (GameFlowData.Get() != null && !GameFlowData.Get().IsInDecisionState())
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
            if (abilityData.HasQueuedAction(actionTypeOfAbility) || (!isPrimary && !isUnavailableUltimate))
            {
                UpdateTargetingAbilityIndicator(ability, actionTypeOfAbility, i);
                i++;
            }
        }

        for (int j = 0; j <= 3; j++)
        {
            AbilityData.ActionType actionType = AbilityData.ActionType.CARD_0 + j;
            Ability ability = abilityData.GetAbilityOfActionType(actionType);
            if (abilityData.HasQueuedAction(actionType) && ability != null)
            {
                UpdateTargetingAbilityIndicator(ability, actionType, i);
                i++;
            }
        }

        for (; i < m_targetingAbilityIndicators.Count; i++)
        {
            UIManager.SetGameObjectActive(m_targetingAbilityIndicators[i], false);
        }

        UpdateCatalysts(abilityData.GetCachedCardAbilities());
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
