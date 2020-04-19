using System;
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
		if (this.theButton != null)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueuedAbilityButton.Start()).MethodHandle;
			}
			UIEventTriggerUtils.AddListener(this.theButton.gameObject, EventTriggerType.PointerDown, new UIEventTriggerUtils.EventDelegate(this.OnPointerDown));
			UIEventTriggerUtils.AddListener(this.theButton.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnPointerClick));
			UIEventTriggerUtils.AddListener(this.theButton.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.OnPointerEnter));
			UIEventTriggerUtils.AddListener(this.theButton.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.OnPointerExit));
		}
		else
		{
			UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerDown, new UIEventTriggerUtils.EventDelegate(this.OnPointerDown));
			UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnPointerClick));
			UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.OnPointerEnter));
			UIEventTriggerUtils.AddListener(base.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.OnPointerExit));
		}
	}

	public void Setup(AbilityData.AbilityEntry abilityEntry, AbilityData.ActionType actionType, AbilityData abilityData, UIQueuedAction reference)
	{
		this.m_abilityEntry = abilityEntry;
		this.m_actionType = actionType;
		this.m_abilityData = abilityData;
		this.m_turnSM = this.m_abilityData.GetComponent<ActorTurnSM>();
		UIManager.SetGameObjectActive(this.m_movementSprite, false, null);
		this.m_parentReference = reference;
		if (this.m_abilityEntry != null)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueuedAbilityButton.Setup(AbilityData.AbilityEntry, AbilityData.ActionType, AbilityData, UIQueuedAction)).MethodHandle;
			}
			if (this.m_abilityEntry.ability != null)
			{
				UIManager.SetGameObjectActive(this.m_theSprite, true, null);
				this.m_theSprite.sprite = this.m_abilityEntry.ability.sprite;
				goto IL_DD;
			}
		}
		if (this.m_abilityEntry != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			UIManager.SetGameObjectActive(this.m_theSprite, false, null);
			this.theButton.enabled = false;
		}
		else
		{
			UIManager.SetGameObjectActive(this.m_theSprite, false, null);
			this.theButton.enabled = false;
		}
		IL_DD:
		int num = UIBaseButton.PhaseIndexForUIPhase(UIQueueListPanel.GetUIPhaseFromAbilityPriority(this.m_abilityEntry.ability.RunPriority));
		for (int i = 0; i < this.m_phaseIndicators.Length; i++)
		{
			Component component = this.m_phaseIndicators[i];
			bool doActive;
			if (i == num)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				doActive = (num != -1);
			}
			else
			{
				doActive = false;
			}
			UIManager.SetGameObjectActive(component, doActive, null);
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		UIManager.SetGameObjectActive(this.m_freeActionSprite, abilityEntry.ability.IsFreeAction(), null);
		UIManager.SetGameObjectActive(this.m_phaseIndicatorContainer, true, null);
		UIManager.SetGameObjectActive(this.m_close, false, null);
	}

	public void SetupMovement(UIQueuedAction reference)
	{
		this.m_parentReference = reference;
		this.m_abilityEntry = null;
		this.m_actionType = AbilityData.ActionType.INVALID_ACTION;
		this.m_abilityData = null;
		UIManager.SetGameObjectActive(this.m_theSprite, false, null);
		UIManager.SetGameObjectActive(this.m_movementSprite, true, null);
		UIManager.SetGameObjectActive(this.m_freeActionSprite, false, null);
		UIManager.SetGameObjectActive(this.m_phaseIndicatorContainer, false, null);
		UIManager.SetGameObjectActive(this.m_close, false, null);
	}

	public void OnPointerExit(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_close, false, null);
	}

	public void OnPointerEnter(BaseEventData data)
	{
		if (this.m_canCancel)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueuedAbilityButton.OnPointerEnter(BaseEventData)).MethodHandle;
			}
			if (!SinglePlayerManager.IsCancelDisabled())
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(this.m_close, true, null);
				UIManager.SetGameObjectActive(this.m_closeHover, true, null);
				UIManager.SetGameObjectActive(this.m_closeDown, false, null);
			}
		}
	}

	public void OnPointerDown(BaseEventData data)
	{
		if (this.m_canCancel)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueuedAbilityButton.OnPointerDown(BaseEventData)).MethodHandle;
			}
			if (!SinglePlayerManager.IsCancelDisabled())
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				UIManager.SetGameObjectActive(this.m_close, true, null);
				UIManager.SetGameObjectActive(this.m_closeHover, false, null);
				UIManager.SetGameObjectActive(this.m_closeDown, true, null);
			}
		}
	}

	public void DoCancel()
	{
		if (SinglePlayerManager.IsCancelDisabled())
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueuedAbilityButton.DoCancel()).MethodHandle;
			}
			return;
		}
		if (GameFlowData.Get().IsInDecisionState())
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			AbilityData component = GameFlowData.Get().activeOwnedActorData.GetComponent<AbilityData>();
			ActorTurnSM component2 = component.GetComponent<ActorTurnSM>();
			if (this.m_abilityEntry != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				bool flag = component.HasQueuedAction(this.m_actionType);
				if (flag)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.RemovedAbility(this.m_actionType);
					component2.RequestCancelAction(this.m_actionType, false);
				}
			}
			else
			{
				component2.RequestCancelMovement();
			}
		}
	}

	public void NotifyLockedIn(bool isLockedIn)
	{
		this.m_isLockedIn = isLockedIn;
	}

	public void OnPointerClick(BaseEventData data)
	{
		if (this.m_canCancel)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueuedAbilityButton.OnPointerClick(BaseEventData)).MethodHandle;
			}
			if (!SinglePlayerManager.IsCancelDisabled())
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.IsAnimCancelPlaying() && !this.m_isLockedIn)
				{
					this.m_parentReference.AbilityCanceled();
				}
			}
		}
	}
}
