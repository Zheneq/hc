using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIQueuedAction : MonoBehaviour
{
	public RectTransform m_background;

	public RectTransform m_backgroundGlow;

	public Image[] m_queueLines;

	public UIQueuedAbilityButton m_abilityTransform;

	public UIQueuedCardButton m_cardTransform;

	public Button TauntCancelButton;

	public Animator m_animationController;

	public Color m_activeQueueColor;

	public Color m_pastQueueColor;

	private UIQueuedAction.ActionType m_queueType;

	private AbilityData.AbilityEntry m_myAbility;

	private AbilityData.ActionType m_actionType;

	private AbilityData m_abilityData;

	private bool m_refreshingAction;

	private bool m_glowState;

	private bool m_queueLineVisible;

	private bool m_lockedIn;

	private float m_queueLineOpacity;

	private Color m_queueLineColor;

	private float m_colorchangeStartTime;

	private float m_colorDistance;

	private bool m_abilityTransformVisibility;

	private bool m_cardTransformVisibility;

	public void SetGlow(bool glow)
	{
		this.m_glowState = glow;
	}

	public void SetAbilityTransformVisible(bool visible)
	{
		this.m_abilityTransformVisibility = visible;
	}

	public void SetCardTransformVisible(bool visible)
	{
		this.m_cardTransformVisibility = visible;
	}

	private void SetQueueLineColor(Color c)
	{
		for (int i = 0; i < this.m_queueLines.Length; i++)
		{
			Color color = this.m_queueLines[i].color;
			color.r = c.r;
			color.g = c.g;
			color.b = c.b;
			this.m_queueLines[i].color = color;
		}
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
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueuedAction.SetQueueLineColor(Color)).MethodHandle;
		}
		this.m_queueLineColor = c;
	}

	private void SetQueueLineOpacity(float f)
	{
		for (int i = 0; i < this.m_queueLines.Length; i++)
		{
			Color color = this.m_queueLines[i].color;
			color.a = f;
			this.m_queueLines[i].color = color;
		}
		this.m_queueLineOpacity = f;
	}

	public void PlayAnimation(string animName, int animLayer)
	{
		this.SetQueueLineColor(Color.white);
	}

	public void PlayAnimation(string animName)
	{
		this.SetQueueLineColor(Color.white);
	}

	public void NotifyLockedIn(bool isLockedIn)
	{
		if (isLockedIn)
		{
			this.PlayAnimation("QueueItem_LockedIn", 1);
		}
		this.m_abilityTransform.NotifyLockedIn(isLockedIn);
		this.m_cardTransform.NotifyLockedIn(isLockedIn);
		this.m_lockedIn = isLockedIn;
		this.m_colorchangeStartTime = Time.time;
		if (this.m_lockedIn)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueuedAction.NotifyLockedIn(bool)).MethodHandle;
			}
			this.m_colorDistance = Vector3.Distance(new Vector3(this.m_queueLineColor.r, this.m_queueLineColor.g, this.m_queueLineColor.b), Vector3.one);
		}
		else
		{
			Color color = Color.white;
			if (this.m_glowState)
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
				color = this.m_activeQueueColor;
			}
			else
			{
				color = this.m_pastQueueColor;
			}
			this.m_colorDistance = Vector3.Distance(new Vector3(this.m_queueLineColor.r, this.m_queueLineColor.g, this.m_queueLineColor.b), new Vector3(color.r, color.g, color.b));
		}
	}

	public bool IsActiveQueued()
	{
		return this.m_queueType != UIQueuedAction.ActionType.None;
	}

	public AbilityData.ActionType GetActionType()
	{
		return this.m_actionType;
	}

	public AbilityData.AbilityEntry GetQueuedAbility()
	{
		return this.m_myAbility;
	}

	public bool IsMovement()
	{
		return this.m_queueType == UIQueuedAction.ActionType.Movement;
	}

	public void SetupAbility(AbilityData.AbilityEntry entry, AbilityData.ActionType actionType, AbilityData abilityData, bool bDoNotAnimate)
	{
		this.ClearAction(false, bDoNotAnimate);
		this.m_queueType = UIQueuedAction.ActionType.None;
		if (AbilityData.ActionType.CARD_0 <= actionType)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueuedAction.SetupAbility(AbilityData.AbilityEntry, AbilityData.ActionType, AbilityData, bool)).MethodHandle;
			}
			if (actionType <= AbilityData.ActionType.CARD_2)
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
				this.m_queueType = UIQueuedAction.ActionType.Card;
				goto IL_58;
			}
		}
		if (AbilityData.ActionType.ABILITY_0 <= actionType && actionType <= AbilityData.ActionType.ABILITY_6)
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
			this.m_queueType = UIQueuedAction.ActionType.Ability;
		}
		IL_58:
		this.m_actionType = actionType;
		this.m_abilityData = abilityData;
		this.m_myAbility = entry;
		this.UpdateVisibility(bDoNotAnimate);
	}

	public void SetupCard()
	{
		this.ClearAction(false, false);
		this.m_queueType = UIQueuedAction.ActionType.Card;
		this.UpdateVisibility(false);
	}

	public void SetUpMovement(bool bDoNotAnimate)
	{
		this.ClearAction(false, false);
		this.m_queueType = UIQueuedAction.ActionType.Movement;
		this.UpdateVisibility(bDoNotAnimate);
	}

	public void ClearAction()
	{
		this.ClearAction(true, false);
	}

	public void ClearAction(bool updateVisible, bool doNotAnimate)
	{
		this.m_queueType = UIQueuedAction.ActionType.None;
		this.m_myAbility = null;
		this.m_actionType = AbilityData.ActionType.INVALID_ACTION;
		this.m_abilityData = null;
		if (updateVisible)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueuedAction.ClearAction(bool, bool)).MethodHandle;
			}
			this.UpdateVisibility(doNotAnimate);
		}
	}

	public void CardCanceled()
	{
		this.m_cardTransform.DoCancel();
		this.m_queueLineVisible = false;
		this.SetQueueLineOpacity(1f);
		this.PlayAnimation("QueueItem_LeaveQueue", 0);
		HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.SetCancelAnimPlaying(true);
	}

	public void AbilityCanceled()
	{
		this.m_abilityTransform.DoCancel();
		this.m_queueLineVisible = false;
		this.SetQueueLineOpacity(1f);
		this.PlayAnimation("QueueItem_LeaveQueue", 0);
		HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.SetCancelAnimPlaying(true);
	}

	public void EnterQueueAnimationDone()
	{
	}

	public void LeaveQueueAnimationDone()
	{
		if (!this.m_refreshingAction)
		{
			this.SetCardTransformVisible(false);
			this.SetAbilityTransformVisible(false);
		}
		HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.AbilityCancelAnimDone();
		this.m_refreshingAction = false;
	}

	private void UpdateVisibility(bool bDoNotPlayAnim = false)
	{
		if (this.m_queueType == UIQueuedAction.ActionType.None)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueuedAction.UpdateVisibility(bool)).MethodHandle;
			}
			UIManager.SetGameObjectActive(this.TauntCancelButton, false, null);
			if (!bDoNotPlayAnim)
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
				if (!this.m_cardTransform.gameObject.activeSelf)
				{
					if (!this.m_abilityTransform.gameObject.activeSelf)
					{
						goto IL_A1;
					}
					for (;;)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.SetCancelAnimPlaying(true);
				this.m_queueLineVisible = false;
				this.SetQueueLineOpacity(1f);
				this.PlayAnimation("QueueItem_LeaveQueue", 0);
				goto IL_A7;
			}
			IL_A1:
			this.LeaveQueueAnimationDone();
			IL_A7:;
		}
		else
		{
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null && this.m_actionType != AbilityData.ActionType.INVALID_ACTION)
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
				ActorCinematicRequests component = activeOwnedActorData.GetComponent<ActorCinematicRequests>();
				if (component.IsAbilityCinematicRequested(this.m_actionType))
				{
					UIManager.SetGameObjectActive(this.TauntCancelButton, true, null);
				}
			}
			this.m_queueLineVisible = true;
			if (!bDoNotPlayAnim)
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
				this.SetQueueLineOpacity(0f);
				this.PlayAnimation("QueueItem_EnterQueue", 0);
			}
			this.m_refreshingAction = true;
		}
		UIQueuedAction.ActionType queueType = this.m_queueType;
		if (queueType != UIQueuedAction.ActionType.Ability)
		{
			if (queueType != UIQueuedAction.ActionType.Card)
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
				if (queueType != UIQueuedAction.ActionType.Movement)
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
				}
				else
				{
					this.SetCardTransformVisible(false);
					this.SetAbilityTransformVisible(true);
					this.m_abilityTransform.SetupMovement(this);
				}
			}
			else
			{
				this.SetCardTransformVisible(true);
				this.SetAbilityTransformVisible(false);
				this.m_cardTransform.Setup(this.m_myAbility, this.m_actionType, this.m_abilityData, this);
			}
		}
		else
		{
			this.SetCardTransformVisible(false);
			this.SetAbilityTransformVisible(true);
			this.m_abilityTransform.Setup(this.m_myAbility, this.m_actionType, this.m_abilityData, this);
		}
	}

	public void EnabledCancelTauntButton(bool enable)
	{
		UIManager.SetGameObjectActive(this.TauntCancelButton, enable, null);
	}

	public void CanceledTaunt(BaseEventData data)
	{
		if (this.m_actionType == AbilityData.ActionType.INVALID_ACTION)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueuedAction.CanceledTaunt(BaseEventData)).MethodHandle;
			}
			return;
		}
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null && this.m_actionType != AbilityData.ActionType.INVALID_ACTION)
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
			ActorCinematicRequests component = activeOwnedActorData.GetComponent<ActorCinematicRequests>();
			if (component.IsAbilityCinematicRequested(this.m_actionType))
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
				activeOwnedActorData.GetComponent<ActorCinematicRequests>().SendAbilityCinematicRequest(this.m_actionType, false, -1, -1);
				UIManager.SetGameObjectActive(this.TauntCancelButton, false, null);
			}
		}
	}

	private void Start()
	{
		this.ClearAction();
		if (this.TauntCancelButton != null)
		{
			UIManager.SetGameObjectActive(this.TauntCancelButton, false, null);
			UIEventTriggerUtils.AddListener(this.TauntCancelButton.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.CanceledTaunt));
		}
		this.SetCardTransformVisible(false);
		this.SetAbilityTransformVisible(false);
		this.SetQueueLineColor(Color.white);
	}

	private void Update()
	{
		this.m_refreshingAction = false;
		if (this.m_glowState)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueuedAction.Update()).MethodHandle;
			}
			for (int i = 0; i < this.m_queueLines.Length; i++)
			{
				this.m_queueLines[i].color = this.m_activeQueueColor;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		else
		{
			for (int j = 0; j < this.m_queueLines.Length; j++)
			{
				this.m_queueLines[j].color = this.m_pastQueueColor;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		UIManager.SetGameObjectActive(this.m_cardTransform, this.m_cardTransformVisibility, null);
		UIManager.SetGameObjectActive(this.m_abilityTransform, this.m_abilityTransformVisibility, null);
		if (this.m_lockedIn)
		{
			Vector3 a = new Vector3(this.m_queueLineColor.r, this.m_queueLineColor.g, this.m_queueLineColor.b);
			float num = Time.time - this.m_colorchangeStartTime;
			float t = num / this.m_colorDistance;
			a = Vector3.Lerp(a, Vector3.one, t);
			this.m_queueLineColor.r = a.x;
			this.m_queueLineColor.g = a.y;
			this.m_queueLineColor.b = a.z;
			this.SetQueueLineColor(this.m_queueLineColor);
		}
		else
		{
			Vector3 a2 = new Vector3(this.m_queueLineColor.r, this.m_queueLineColor.g, this.m_queueLineColor.b);
			Color color = Color.black;
			if (this.m_glowState)
			{
				color = this.m_activeQueueColor;
			}
			else
			{
				color = this.m_pastQueueColor;
			}
			float num2 = Time.time - this.m_colorchangeStartTime;
			float t2 = num2 / this.m_colorDistance;
			a2 = Vector3.Lerp(a2, new Vector3(color.r, color.g, color.b), t2);
			this.m_queueLineColor.r = a2.x;
			this.m_queueLineColor.g = a2.y;
			this.m_queueLineColor.b = a2.z;
			this.SetQueueLineColor(this.m_queueLineColor);
		}
		if (this.m_queueLineVisible)
		{
			this.SetQueueLineOpacity(this.m_queueLineOpacity + Time.deltaTime);
		}
		else
		{
			this.SetQueueLineOpacity(this.m_queueLineOpacity - Time.deltaTime);
		}
	}

	private void OnEnable()
	{
		this.Update();
	}

	private enum ActionType
	{
		None,
		Ability,
		Card,
		Movement
	}
}
