using System;
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

	public ActorData ActorDataRef
	{
		get
		{
			return this.m_actor;
		}
	}

	private void Start()
	{
		UIManager.SetGameObjectActive(this.m_lockIcon, false, null);
		UIEventTriggerUtils.AddListener(this.m_hitbox.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.MouseEntered));
		UIEventTriggerUtils.AddListener(this.m_hitbox.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.MouseExited));
		GameFlowData.s_onGameStateChanged += this.OnGameStateChanged;
	}

	private void OnDestroy()
	{
		GameFlowData.s_onGameStateChanged -= this.OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState newState)
	{
		if (newState == GameState.BothTeams_Resolve)
		{
			UIManager.SetGameObjectActive(this.m_targetingAbilityIconsGrid, false, null);
		}
	}

	public void MouseEntered(BaseEventData data)
	{
		if (this.m_actor == null)
		{
			return;
		}
		AbilityData component = this.m_actor.GetComponent<AbilityData>();
		if (component == null)
		{
			return;
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
		UIManager.SetGameObjectActive(this.m_targetingAbilityIconsGrid, true, null);
		using (List<Ability>.Enumerator enumerator = abilitiesAsList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				Ability ability = enumerator.Current;
				AbilityData.ActionType actionTypeOfAbility = component.GetActionTypeOfAbility(ability);
				bool flag = actionTypeOfAbility == AbilityData.ActionType.ABILITY_0;
				bool flag2 = actionTypeOfAbility == AbilityData.ActionType.ABILITY_4 && ability.GetModdedCost() >= this.m_actor.GetActualMaxTechPoints();
				if (!component.HasQueuedAction(actionTypeOfAbility))
				{
					if (!flag)
					{
						if (!flag2)
						{
							goto IL_FF;
						}
					}
					continue;
				}
				IL_FF:
				this.UpdateTargetingAbilityIndicator(ability, actionTypeOfAbility, i);
				i++;
			}
		}
		for (int j = 0; j <= 3; j++)
		{
			AbilityData.ActionType actionType = AbilityData.ActionType.CARD_0 + j;
			Ability abilityOfActionType = component.GetAbilityOfActionType(actionType);
			if (component.HasQueuedAction(actionType))
			{
				if (abilityOfActionType != null)
				{
					this.UpdateTargetingAbilityIndicator(abilityOfActionType, actionType, i);
					i++;
				}
			}
		}
		while (i < this.m_targetingAbilityIndicators.Count)
		{
			UIManager.SetGameObjectActive(this.m_targetingAbilityIndicators[i], false, null);
			i++;
		}
		this.UpdateCatalysts(component.GetCachedCardAbilities());
	}

	public void UpdateCatalysts(List<Ability> cardAbilities)
	{
		if (this.m_catalsystPips == null)
		{
			return;
		}
		bool doActive = false;
		bool doActive2 = false;
		bool doActive3 = false;
		for (int i = 0; i < cardAbilities.Count; i++)
		{
			Ability ability = cardAbilities[i];
			if (ability != null)
			{
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
		}
		UIManager.SetGameObjectActive(this.m_catalsystPips.m_PrepPhaseOn, doActive, null);
		UIManager.SetGameObjectActive(this.m_catalsystPips.m_DashPhaseOn, doActive2, null);
		UIManager.SetGameObjectActive(this.m_catalsystPips.m_BlastPhaseOn, doActive3, null);
		UIUtils.SetAsLastSiblingIfNeeded(this.m_catalsystPips.transform);
	}

	public void MouseExited(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_targetingAbilityIconsGrid, false, null);
	}

	public bool IsActiveDisplay()
	{
		return this.m_actor != null;
	}

	public void Setup(ActorData actor)
	{
		this.m_actor = actor;
		if (!this.IsActiveDisplay())
		{
			UIManager.SetGameObjectActive(this, false, null);
		}
	}

	public void NotifyLockedIn(bool isLocked)
	{
		if (this.m_lockStatus != isLocked)
		{
			if (!(this.m_actor == null))
			{
				ActorTurnSM component = this.m_actor.GetComponent<ActorTurnSM>();
				if (component.AmStillDeciding())
				{
					UIManager.SetGameObjectActive(this.m_lockIcon, false, null);
				}
				else if (this.m_actor.IsDead())
				{
					UIManager.SetGameObjectActive(this.m_lockIcon, false, null);
				}
				else if (!this.m_lockIcon.gameObject.activeSelf)
				{
					UIManager.SetGameObjectActive(this.m_lockIcon, true, null);
					this.m_animationController.Play("PlayerStatusLockIn");
				}
				this.m_lockStatus = isLocked;
				return;
			}
		}
	}

	public Team GetTeam()
	{
		if (this.m_actor == null)
		{
			return Team.Invalid;
		}
		return this.m_actor.GetTeam();
	}

	private void Update()
	{
		if (this.m_actor == null)
		{
			UIManager.SetGameObjectActive(this, false, null);
			return;
		}
		if (this.m_actor.IsDead())
		{
			this.m_deathText.text = string.Empty;
			UIManager.SetGameObjectActive(this.m_border, true, null);
			UIManager.SetGameObjectActive(this.m_overlay, true, null);
			UIManager.SetGameObjectActive(this.m_skullIcon, true, null);
		}
		else
		{
			this.m_deathText.text = string.Empty;
			UIManager.SetGameObjectActive(this.m_border, false, null);
			UIManager.SetGameObjectActive(this.m_overlay, false, null);
			UIManager.SetGameObjectActive(this.m_skullIcon, false, null);
		}
		this.UpdateInfo();
	}

	private void UpdateInfo()
	{
		if (this.m_actor != null)
		{
			this.m_characterIcon.sprite = this.m_actor.GetCharacterResourceLink().GetCharacterSelectIcon();
			bool hasBotController = this.m_actor.HasBotController;
			UIManager.SetGameObjectActive(this.m_botIndicator, hasBotController, null);
		}
	}

	private void OnEnable()
	{
		this.Update();
	}

	public void UpdateTargetingAbilityIndicator(Ability ability, AbilityData.ActionType action, int index)
	{
		if (index < 8)
		{
			while (this.m_targetingAbilityIndicators.Count <= index)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.m_targetingAbilityIndicatorPrefab);
				UITargetingAbilityIndicator component = gameObject.GetComponent<UITargetingAbilityIndicator>();
				component.transform.SetParent(this.m_targetingAbilityIconsGrid.transform);
				component.transform.localScale = Vector3.one;
				component.transform.localPosition = Vector3.zero;
				component.transform.localEulerAngles = Vector3.zero;
				this.m_targetingAbilityIndicators.Add(component);
			}
			this.m_targetingAbilityIndicators[index].Setup(this.m_actor, ability, action);
			if (!this.m_targetingAbilityIndicators[index].gameObject.activeSelf)
			{
				UIManager.SetGameObjectActive(this.m_targetingAbilityIndicators[index], true, null);
			}
		}
		if (this.m_catalsystPips == null)
		{
			this.m_catalsystPips = UnityEngine.Object.Instantiate<UITargetingAbilityCatalystPipContainer>(this.m_catalystIndicatorPrefab);
			this.m_catalsystPips.transform.SetParent(this.m_targetingAbilityIconsGrid.transform);
			this.m_catalsystPips.transform.localScale = Vector3.one;
			this.m_catalsystPips.transform.localPosition = Vector3.zero;
			this.m_catalsystPips.transform.localEulerAngles = Vector3.zero;
		}
		UIManager.SetGameObjectActive(this.m_catalsystPips, true, null);
		UIUtils.SetAsLastSiblingIfNeeded(this.m_catalsystPips.transform);
	}

	public void TurnOffTargetingAbilityIndicator(int fromIndex)
	{
		for (int i = fromIndex; i < this.m_targetingAbilityIndicators.Count; i++)
		{
			if (this.m_targetingAbilityIndicators[i].gameObject.activeSelf)
			{
				UIManager.SetGameObjectActive(this.m_targetingAbilityIndicators[i], false, null);
			}
		}
		if (fromIndex == 0)
		{
			UIManager.SetGameObjectActive(this.m_catalsystPips, false, null);
		}
	}
}
