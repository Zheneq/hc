using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIQueuedCardButton : UIBaseButton
{
	public RectTransform m_close;

	public Image m_closeHover;

	public Image m_closeDown;

	public bool m_canCancel = true;

	private bool m_isLockedIn;

	private UIQueuedAction m_parentReference;

	public override void Start()
	{
		base.Start();
		if (theButton != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
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

	public void OnPointerExit(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_close, false);
	}

	public void OnPointerEnter(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_close, true);
		UIManager.SetGameObjectActive(m_closeHover, true);
		UIManager.SetGameObjectActive(m_closeDown, false);
	}

	public void OnPointerDown(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_close, true);
		UIManager.SetGameObjectActive(m_closeHover, false);
		UIManager.SetGameObjectActive(m_closeDown, true);
	}

	public void DoCancel()
	{
		if (!GameFlowData.Get().IsInDecisionState())
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityData component = GameFlowData.Get().activeOwnedActorData.GetComponent<AbilityData>();
			ActorTurnSM component2 = component.GetComponent<ActorTurnSM>();
			if (m_abilityEntry != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						if (component.HasQueuedAction(m_actionType))
						{
							HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.RemovedAbility(m_actionType);
							component2.RequestCancelAction(m_actionType, false);
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
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (SinglePlayerManager.IsCancelDisabled())
			{
				return;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				if (HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.IsAnimCancelPlaying())
				{
					return;
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					if (!m_isLockedIn)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							m_parentReference.CardCanceled();
							return;
						}
					}
					return;
				}
			}
		}
	}

	public void Setup(AbilityData.AbilityEntry abilityEntry, AbilityData.ActionType actionType, AbilityData abilityData, UIQueuedAction reference)
	{
		m_abilityEntry = abilityEntry;
		m_actionType = actionType;
		m_abilityData = abilityData;
		m_turnSM = m_abilityData.GetComponent<ActorTurnSM>();
		m_parentReference = reference;
		if (m_abilityEntry != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_abilityEntry.ability != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(m_theSprite, true);
				m_theSprite.sprite = m_abilityEntry.ability.sprite;
				goto IL_00d2;
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
		goto IL_00d2;
		IL_00d2:
		int num = UIBaseButton.PhaseIndexForUIPhase(UIQueueListPanel.GetUIPhaseFromAbilityPriority(m_abilityEntry.ability.RunPriority));
		for (int i = 0; i < m_phaseIndicators.Length; i++)
		{
			Image component = m_phaseIndicators[i];
			int doActive;
			if (i == num)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				doActive = ((num != -1) ? 1 : 0);
			}
			else
			{
				doActive = 0;
			}
			UIManager.SetGameObjectActive(component, (byte)doActive != 0);
		}
		UIManager.SetGameObjectActive(m_freeActionSprite, abilityEntry.ability.IsFreeAction());
		UIManager.SetGameObjectActive(m_close, false);
	}
}
