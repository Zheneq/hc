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
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		AbilityData component = m_actor.GetComponent<AbilityData>();
		if (component == null)
		{
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		if (GameFlowData.Get() != null)
		{
			if (!GameFlowData.Get().IsInDecisionState())
			{
				return;
			}
		}
		List<Ability> abilitiesAsList = component.GetAbilitiesAsList();
		int i = 0;
		UIManager.SetGameObjectActive(m_targetingAbilityIconsGrid, true);
		using (List<Ability>.Enumerator enumerator = abilitiesAsList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Ability current = enumerator.Current;
				AbilityData.ActionType actionTypeOfAbility = component.GetActionTypeOfAbility(current);
				bool flag = actionTypeOfAbility == AbilityData.ActionType.ABILITY_0;
				bool flag2 = actionTypeOfAbility == AbilityData.ActionType.ABILITY_4 && current.GetModdedCost() >= m_actor.GetMaxTechPoints();
				if (!component.HasQueuedAction(actionTypeOfAbility))
				{
					if (flag)
					{
						continue;
					}
					if (flag2)
					{
						continue;
					}
				}
				UpdateTargetingAbilityIndicator(current, actionTypeOfAbility, i);
				i++;
			}
		}
		for (int j = 0; j <= 3; j++)
		{
			AbilityData.ActionType actionType = (AbilityData.ActionType)(7 + j);
			Ability abilityOfActionType = component.GetAbilityOfActionType(actionType);
			if (component.HasQueuedAction(actionType) && abilityOfActionType != null)
			{
				UpdateTargetingAbilityIndicator(abilityOfActionType, actionType, i);
				i++;
			}
		}
		for (; i < m_targetingAbilityIndicators.Count; i++)
		{
			UIManager.SetGameObjectActive(m_targetingAbilityIndicators[i], false);
		}
		UpdateCatalysts(component.GetCachedCardAbilities());
	}

	public void UpdateCatalysts(List<Ability> cardAbilities)
	{
		if (m_catalsystPips == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		bool doActive = false;
		bool doActive2 = false;
		bool doActive3 = false;
		for (int i = 0; i < cardAbilities.Count; i++)
		{
			Ability ability = cardAbilities[i];
			if (!(ability != null))
			{
				continue;
			}
			AbilityRunPhase abilityRunPhase = Card.AbilityPriorityToRunPhase(ability.GetRunPriority());
			if (abilityRunPhase == AbilityRunPhase.Prep)
			{
				doActive = true;
			}
			else if (abilityRunPhase == AbilityRunPhase.Dash)
			{
				doActive2 = true;
			}
			else if (abilityRunPhase == AbilityRunPhase.Combat)
			{
				doActive3 = true;
			}
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_catalsystPips.m_PrepPhaseOn, doActive);
			UIManager.SetGameObjectActive(m_catalsystPips.m_DashPhaseOn, doActive2);
			UIManager.SetGameObjectActive(m_catalsystPips.m_BlastPhaseOn, doActive3);
			UIUtils.SetAsLastSiblingIfNeeded(m_catalsystPips.transform);
			return;
		}
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
		if (m_lockStatus == isLocked)
		{
			return;
		}
		while (true)
		{
			if (m_actor == null)
			{
				while (true)
				{
					switch (5)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			ActorTurnSM component = m_actor.GetComponent<ActorTurnSM>();
			if (component.AmStillDeciding())
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
			return;
		}
	}

	public Team GetTeam()
	{
		if (m_actor == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					return Team.Invalid;
				}
			}
		}
		return m_actor.GetTeam();
	}

	private void Update()
	{
		if (m_actor == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					UIManager.SetGameObjectActive(this, false);
					return;
				}
			}
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
		if (index < 8)
		{
			while (m_targetingAbilityIndicators.Count <= index)
			{
				GameObject gameObject = Object.Instantiate(m_targetingAbilityIndicatorPrefab);
				UITargetingAbilityIndicator component = gameObject.GetComponent<UITargetingAbilityIndicator>();
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
			m_catalsystPips = Object.Instantiate(m_catalystIndicatorPrefab);
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
