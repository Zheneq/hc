using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIQueuedAbilityButton : UIBaseButton
{
	public RectTransform m_close;

	public Image m_closeHover;

	public Image m_closeDown;

	public Image m_movementSprite;

	public RectTransform m_phaseIndicatorContainer;

	public bool m_canCancel = true;

	private UIQueuedAction m_parentReference;

	private bool m_isLockedIn;

	public override void Start()
	{
		base.Start();
		if (theButton != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					UIEventTriggerUtils.AddListener(theButton.gameObject, EventTriggerType.PointerDown, OnPointerDown);
					UIEventTriggerUtils.AddListener(theButton.gameObject, EventTriggerType.PointerClick, OnPointerClick);
					UIEventTriggerUtils.AddListener(theButton.gameObject, EventTriggerType.PointerEnter, OnPointerEnter);
					UIEventTriggerUtils.AddListener(theButton.gameObject, EventTriggerType.PointerExit, OnPointerExit);
					return;
				}
			}
		}
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerDown, OnPointerDown);
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerClick, OnPointerClick);
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerEnter, OnPointerEnter);
		UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerExit, OnPointerExit);
	}

	public void Setup(AbilityData.AbilityEntry abilityEntry, AbilityData.ActionType actionType, AbilityData abilityData, UIQueuedAction reference)
	{
		m_abilityEntry = abilityEntry;
		m_actionType = actionType;
		m_abilityData = abilityData;
		m_turnSM = m_abilityData.GetComponent<ActorTurnSM>();
		UIManager.SetGameObjectActive(m_movementSprite, false);
		m_parentReference = reference;
		if (m_abilityEntry != null)
		{
			if (m_abilityEntry.ability != null)
			{
				UIManager.SetGameObjectActive(m_theSprite, true);
				m_theSprite.sprite = m_abilityEntry.ability.sprite;
				goto IL_00dd;
			}
		}
		if (m_abilityEntry != null)
		{
			UIManager.SetGameObjectActive(m_theSprite, false);
			theButton.enabled = false;
		}
		else
		{
			UIManager.SetGameObjectActive(m_theSprite, false);
			theButton.enabled = false;
		}
		goto IL_00dd;
		IL_00dd:
		int num = UIBaseButton.PhaseIndexForUIPhase(UIQueueListPanel.GetUIPhaseFromAbilityPriority(m_abilityEntry.ability.RunPriority));
		for (int i = 0; i < m_phaseIndicators.Length; i++)
		{
			Image component = m_phaseIndicators[i];
			int doActive;
			if (i == num)
			{
				doActive = ((num != -1) ? 1 : 0);
			}
			else
			{
				doActive = 0;
			}
			UIManager.SetGameObjectActive(component, (byte)doActive != 0);
		}
		while (true)
		{
			UIManager.SetGameObjectActive(m_freeActionSprite, abilityEntry.ability.IsFreeAction());
			UIManager.SetGameObjectActive(m_phaseIndicatorContainer, true);
			UIManager.SetGameObjectActive(m_close, false);
			return;
		}
	}

	public void SetupMovement(UIQueuedAction reference)
	{
		m_parentReference = reference;
		m_abilityEntry = null;
		m_actionType = AbilityData.ActionType.INVALID_ACTION;
		m_abilityData = null;
		UIManager.SetGameObjectActive(m_theSprite, false);
		UIManager.SetGameObjectActive(m_movementSprite, true);
		UIManager.SetGameObjectActive(m_freeActionSprite, false);
		UIManager.SetGameObjectActive(m_phaseIndicatorContainer, false);
		UIManager.SetGameObjectActive(m_close, false);
	}

	public void OnPointerExit(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_close, false);
	}

	public void OnPointerEnter(BaseEventData data)
	{
		if (!m_canCancel)
		{
			return;
		}
		while (true)
		{
			if (!SinglePlayerManager.IsCancelDisabled())
			{
				while (true)
				{
					UIManager.SetGameObjectActive(m_close, true);
					UIManager.SetGameObjectActive(m_closeHover, true);
					UIManager.SetGameObjectActive(m_closeDown, false);
					return;
				}
			}
			return;
		}
	}

	public void OnPointerDown(BaseEventData data)
	{
		if (!m_canCancel)
		{
			return;
		}
		while (true)
		{
			if (!SinglePlayerManager.IsCancelDisabled())
			{
				while (true)
				{
					UIManager.SetGameObjectActive(m_close, true);
					UIManager.SetGameObjectActive(m_closeHover, false);
					UIManager.SetGameObjectActive(m_closeDown, true);
					return;
				}
			}
			return;
		}
	}

	public void DoCancel()
	{
		if (SinglePlayerManager.IsCancelDisabled())
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
		if (!GameFlowData.Get().IsInDecisionState())
		{
			return;
		}
		while (true)
		{
			AbilityData component = GameFlowData.Get().activeOwnedActorData.GetComponent<AbilityData>();
			ActorTurnSM component2 = component.GetComponent<ActorTurnSM>();
			if (m_abilityEntry != null)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						if (component.HasQueuedAction(m_actionType))
						{
							while (true)
							{
								switch (4)
								{
								case 0:
									break;
								default:
									HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.RemovedAbility(m_actionType);
									component2.RequestCancelAction(m_actionType, false);
									return;
								}
							}
						}
						return;
					}
				}
			}
			component2.RequestCancelMovement();
			return;
		}
	}

	public void NotifyLockedIn(bool isLockedIn)
	{
		m_isLockedIn = isLockedIn;
	}

	public void OnPointerClick(BaseEventData data)
	{
		if (!m_canCancel)
		{
			return;
		}
		while (true)
		{
			if (SinglePlayerManager.IsCancelDisabled())
			{
				return;
			}
			while (true)
			{
				if (!HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.IsAnimCancelPlaying() && !m_isLockedIn)
				{
					m_parentReference.AbilityCanceled();
				}
				return;
			}
		}
	}
}
