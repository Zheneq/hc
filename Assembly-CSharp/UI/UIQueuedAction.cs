using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIQueuedAction : MonoBehaviour
{
	private enum ActionType
	{
		None,
		Ability,
		Card,
		Movement
	}

	public RectTransform m_background;

	public RectTransform m_backgroundGlow;

	public Image[] m_queueLines;

	public UIQueuedAbilityButton m_abilityTransform;

	public UIQueuedCardButton m_cardTransform;

	public Button TauntCancelButton;

	public Animator m_animationController;

	public Color m_activeQueueColor;

	public Color m_pastQueueColor;

	private ActionType m_queueType;

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
		m_glowState = glow;
	}

	public void SetAbilityTransformVisible(bool visible)
	{
		m_abilityTransformVisibility = visible;
	}

	public void SetCardTransformVisible(bool visible)
	{
		m_cardTransformVisibility = visible;
	}

	private void SetQueueLineColor(Color c)
	{
		for (int i = 0; i < m_queueLines.Length; i++)
		{
			Color color = m_queueLines[i].color;
			color.r = c.r;
			color.g = c.g;
			color.b = c.b;
			m_queueLines[i].color = color;
		}
		while (true)
		{
			m_queueLineColor = c;
			return;
		}
	}

	private void SetQueueLineOpacity(float f)
	{
		for (int i = 0; i < m_queueLines.Length; i++)
		{
			Color color = m_queueLines[i].color;
			color.a = f;
			m_queueLines[i].color = color;
		}
		m_queueLineOpacity = f;
	}

	public void PlayAnimation(string animName, int animLayer)
	{
		SetQueueLineColor(Color.white);
	}

	public void PlayAnimation(string animName)
	{
		SetQueueLineColor(Color.white);
	}

	public void NotifyLockedIn(bool isLockedIn)
	{
		if (isLockedIn)
		{
			PlayAnimation("QueueItem_LockedIn", 1);
		}
		m_abilityTransform.NotifyLockedIn(isLockedIn);
		m_cardTransform.NotifyLockedIn(isLockedIn);
		m_lockedIn = isLockedIn;
		m_colorchangeStartTime = Time.time;
		if (m_lockedIn)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					m_colorDistance = Vector3.Distance(new Vector3(m_queueLineColor.r, m_queueLineColor.g, m_queueLineColor.b), Vector3.one);
					return;
				}
			}
		}
		Color white = Color.white;
		if (m_glowState)
		{
			white = m_activeQueueColor;
		}
		else
		{
			white = m_pastQueueColor;
		}
		m_colorDistance = Vector3.Distance(new Vector3(m_queueLineColor.r, m_queueLineColor.g, m_queueLineColor.b), new Vector3(white.r, white.g, white.b));
	}

	public bool IsActiveQueued()
	{
		return m_queueType != ActionType.None;
	}

	public AbilityData.ActionType GetActionType()
	{
		return m_actionType;
	}

	public AbilityData.AbilityEntry GetQueuedAbility()
	{
		return m_myAbility;
	}

	public bool IsMovement()
	{
		return m_queueType == ActionType.Movement;
	}

	public void SetupAbility(AbilityData.AbilityEntry entry, AbilityData.ActionType actionType, AbilityData abilityData, bool bDoNotAnimate)
	{
		ClearAction(false, bDoNotAnimate);
		m_queueType = ActionType.None;
		if (AbilityData.ActionType.CARD_0 <= actionType)
		{
			if (actionType <= AbilityData.ActionType.CARD_2)
			{
				m_queueType = ActionType.Card;
				goto IL_0058;
			}
		}
		if (AbilityData.ActionType.ABILITY_0 <= actionType && actionType <= AbilityData.ActionType.ABILITY_6)
		{
			m_queueType = ActionType.Ability;
		}
		goto IL_0058;
		IL_0058:
		m_actionType = actionType;
		m_abilityData = abilityData;
		m_myAbility = entry;
		UpdateVisibility(bDoNotAnimate);
	}

	public void SetupCard()
	{
		ClearAction(false, false);
		m_queueType = ActionType.Card;
		UpdateVisibility();
	}

	public void SetUpMovement(bool bDoNotAnimate)
	{
		ClearAction(false, false);
		m_queueType = ActionType.Movement;
		UpdateVisibility(bDoNotAnimate);
	}

	public void ClearAction()
	{
		ClearAction(true, false);
	}

	public void ClearAction(bool updateVisible, bool doNotAnimate)
	{
		m_queueType = ActionType.None;
		m_myAbility = null;
		m_actionType = AbilityData.ActionType.INVALID_ACTION;
		m_abilityData = null;
		if (!updateVisible)
		{
			return;
		}
		while (true)
		{
			UpdateVisibility(doNotAnimate);
			return;
		}
	}

	public void CardCanceled()
	{
		m_cardTransform.DoCancel();
		m_queueLineVisible = false;
		SetQueueLineOpacity(1f);
		PlayAnimation("QueueItem_LeaveQueue", 0);
		HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.SetCancelAnimPlaying(true);
	}

	public void AbilityCanceled()
	{
		m_abilityTransform.DoCancel();
		m_queueLineVisible = false;
		SetQueueLineOpacity(1f);
		PlayAnimation("QueueItem_LeaveQueue", 0);
		HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.SetCancelAnimPlaying(true);
	}

	public void EnterQueueAnimationDone()
	{
	}

	public void LeaveQueueAnimationDone()
	{
		if (!m_refreshingAction)
		{
			SetCardTransformVisible(false);
			SetAbilityTransformVisible(false);
		}
		HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.AbilityCancelAnimDone();
		m_refreshingAction = false;
	}

	private void UpdateVisibility(bool bDoNotPlayAnim = false)
	{
		if (m_queueType == ActionType.None)
		{
			UIManager.SetGameObjectActive(TauntCancelButton, false);
			if (bDoNotPlayAnim)
			{
				goto IL_00a1;
			}
			if (!m_cardTransform.gameObject.activeSelf)
			{
				if (!m_abilityTransform.gameObject.activeSelf)
				{
					goto IL_00a1;
				}
			}
			HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.SetCancelAnimPlaying(true);
			m_queueLineVisible = false;
			SetQueueLineOpacity(1f);
			PlayAnimation("QueueItem_LeaveQueue", 0);
		}
		else
		{
			ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
			if (activeOwnedActorData != null && m_actionType != AbilityData.ActionType.INVALID_ACTION)
			{
				ActorCinematicRequests component = activeOwnedActorData.GetComponent<ActorCinematicRequests>();
				if (component.IsAbilityCinematicRequested(m_actionType))
				{
					UIManager.SetGameObjectActive(TauntCancelButton, true);
				}
			}
			m_queueLineVisible = true;
			if (!bDoNotPlayAnim)
			{
				SetQueueLineOpacity(0f);
				PlayAnimation("QueueItem_EnterQueue", 0);
			}
			m_refreshingAction = true;
		}
		goto IL_0131;
		IL_00a1:
		LeaveQueueAnimationDone();
		goto IL_0131;
		IL_0131:
		ActionType queueType = m_queueType;
		switch (queueType)
		{
		case ActionType.Ability:
			SetCardTransformVisible(false);
			SetAbilityTransformVisible(true);
			m_abilityTransform.Setup(m_myAbility, m_actionType, m_abilityData, this);
			return;
		case ActionType.Card:
			SetCardTransformVisible(true);
			SetAbilityTransformVisible(false);
			m_cardTransform.Setup(m_myAbility, m_actionType, m_abilityData, this);
			return;
		}
		while (true)
		{
			if (queueType != ActionType.Movement)
			{
				while (true)
				{
					switch (6)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
			SetCardTransformVisible(false);
			SetAbilityTransformVisible(true);
			m_abilityTransform.SetupMovement(this);
			return;
		}
	}

	public void EnabledCancelTauntButton(bool enable)
	{
		UIManager.SetGameObjectActive(TauntCancelButton, enable);
	}

	public void CanceledTaunt(BaseEventData data)
	{
		if (m_actionType == AbilityData.ActionType.INVALID_ACTION)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (!(activeOwnedActorData != null) || m_actionType == AbilityData.ActionType.INVALID_ACTION)
		{
			return;
		}
		while (true)
		{
			ActorCinematicRequests component = activeOwnedActorData.GetComponent<ActorCinematicRequests>();
			if (component.IsAbilityCinematicRequested(m_actionType))
			{
				while (true)
				{
					activeOwnedActorData.GetComponent<ActorCinematicRequests>().SendAbilityCinematicRequest(m_actionType, false, -1, -1);
					UIManager.SetGameObjectActive(TauntCancelButton, false);
					return;
				}
			}
			return;
		}
	}

	private void Start()
	{
		ClearAction();
		if (TauntCancelButton != null)
		{
			UIManager.SetGameObjectActive(TauntCancelButton, false);
			UIEventTriggerUtils.AddListener(TauntCancelButton.gameObject, EventTriggerType.PointerClick, CanceledTaunt);
		}
		SetCardTransformVisible(false);
		SetAbilityTransformVisible(false);
		SetQueueLineColor(Color.white);
	}

	private void Update()
	{
		m_refreshingAction = false;
		if (m_glowState)
		{
			for (int i = 0; i < m_queueLines.Length; i++)
			{
				m_queueLines[i].color = m_activeQueueColor;
			}
		}
		else
		{
			for (int j = 0; j < m_queueLines.Length; j++)
			{
				m_queueLines[j].color = m_pastQueueColor;
			}
		}
		UIManager.SetGameObjectActive(m_cardTransform, m_cardTransformVisibility);
		UIManager.SetGameObjectActive(m_abilityTransform, m_abilityTransformVisibility);
		if (m_lockedIn)
		{
			Vector3 a = new Vector3(m_queueLineColor.r, m_queueLineColor.g, m_queueLineColor.b);
			float num = Time.time - m_colorchangeStartTime;
			float t = num / m_colorDistance;
			a = Vector3.Lerp(a, Vector3.one, t);
			m_queueLineColor.r = a.x;
			m_queueLineColor.g = a.y;
			m_queueLineColor.b = a.z;
			SetQueueLineColor(m_queueLineColor);
		}
		else
		{
			Vector3 a2 = new Vector3(m_queueLineColor.r, m_queueLineColor.g, m_queueLineColor.b);
			Color black = Color.black;
			black = ((!m_glowState) ? m_pastQueueColor : m_activeQueueColor);
			float num2 = Time.time - m_colorchangeStartTime;
			float t2 = num2 / m_colorDistance;
			a2 = Vector3.Lerp(a2, new Vector3(black.r, black.g, black.b), t2);
			m_queueLineColor.r = a2.x;
			m_queueLineColor.g = a2.y;
			m_queueLineColor.b = a2.z;
			SetQueueLineColor(m_queueLineColor);
		}
		if (m_queueLineVisible)
		{
			SetQueueLineOpacity(m_queueLineOpacity + Time.deltaTime);
		}
		else
		{
			SetQueueLineOpacity(m_queueLineOpacity - Time.deltaTime);
		}
	}

	private void OnEnable()
	{
		Update();
	}
}
