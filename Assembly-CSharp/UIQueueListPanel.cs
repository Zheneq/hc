using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIQueueListPanel : MonoBehaviour
{
	public enum UIPhase
	{
		Prep,
		Evasion,
		Combat,
		Movement,
		None
	}

	public Button m_CancelAllButton;

	public Image m_CancelAllHover;

	public const int MAX_QUEUEDACTIONS = 6;

	public UIQueuedAction[] m_queuedActions;

	public Animator m_animationController;

	public TextMeshProUGUI m_queueLabel;

	public Color m_labelColor;

	private bool m_isMovementQueued;

	private int m_movementQueueIndex;

	private int m_movementQueueIndexToChangeTo;

	private bool m_changedDisplay;

	private bool m_clickCancelled;

	private bool m_abilityListRefreshed;

	private bool m_abilityCancelRequested;

	private bool m_cancelAnimationPlaying;

	private Color queueLabelColor;

	private float queueLabelOpacity;

	private bool m_labelVisible;

	private bool m_lockedIn;

	private float m_colorchangeStartTime;

	private float m_colorDistance;

	public static UIPhase GetUIPhaseFromAbilityPriority(AbilityPriority priority)
	{
		UIPhase result = UIPhase.None;
		if (priority != 0)
		{
			if (priority != AbilityPriority.Prep_Offense)
			{
				if (priority == AbilityPriority.Evasion)
				{
					result = UIPhase.Evasion;
				}
				else
				{
					if (priority != AbilityPriority.Combat_Damage)
					{
						if (priority != AbilityPriority.DEPRICATED_Combat_Charge)
						{
							if (priority != AbilityPriority.Combat_Knockback)
							{
								if (priority != AbilityPriority.Combat_Final)
								{
									goto IL_006c;
								}
							}
						}
					}
					result = UIPhase.Combat;
				}
				goto IL_006c;
			}
		}
		result = UIPhase.Prep;
		goto IL_006c;
		IL_006c:
		return result;
	}

	private void SetQueueLineColor(Color c)
	{
		Color color = m_queueLabel.color;
		color.r = c.r;
		color.g = c.g;
		color.b = c.b;
		m_queueLabel.color = color;
		queueLabelColor = c;
	}

	private void SetQueueLineOpacity(float f)
	{
		Color color = m_queueLabel.color;
		color.a = f;
		m_queueLabel.color = color;
		queueLabelOpacity = f;
	}

	private void Start()
	{
		if (m_CancelAllButton != null)
		{
			UIEventTriggerUtils.AddListener(m_CancelAllButton.gameObject, EventTriggerType.PointerClick, OnCancelAllClick);
			UIEventTriggerUtils.AddListener(m_CancelAllButton.gameObject, EventTriggerType.PointerEnter, OnCancelAllMouseEnter);
			UIEventTriggerUtils.AddListener(m_CancelAllButton.gameObject, EventTriggerType.PointerExit, OnCancelAllMouseExit);
		}
		if (m_CancelAllHover != null)
		{
			UIManager.SetGameObjectActive(m_CancelAllHover, false);
		}
		DoneQueueFadeAnim();
		SetQueueLineColor(m_labelColor);
	}

	public void NotifyDecisionTimerShow()
	{
		OnCancelAllClick(null);
	}

	public void OnCancelAllClick(BaseEventData data)
	{
		if (!GameFlowData.Get().IsInDecisionState())
		{
			return;
		}
		while (true)
		{
			if (GameFlowData.Get().activeOwnedActorData != null)
			{
				AbilityData component = GameFlowData.Get().activeOwnedActorData.GetComponent<AbilityData>();
				ActorTurnSM component2 = component.GetComponent<ActorTurnSM>();
				for (int i = 0; i < 14; i++)
				{
					AbilityData.ActionType actionType = (AbilityData.ActionType)i;
					if (component.HasQueuedAction(actionType))
					{
						HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.RemovedAbility(actionType);
						component2.RequestCancelAction(actionType, false);
					}
				}
				component2.RequestCancelMovement();
			}
			HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.RefreshQueueList();
			return;
		}
	}

	public void OnCancelAllMouseEnter(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_CancelAllHover, true);
	}

	public void OnCancelAllMouseExit(BaseEventData data)
	{
		UIManager.SetGameObjectActive(m_CancelAllHover, false);
	}

	private void AddMovementToQueue()
	{
		for (int i = 0; i < 6; i++)
		{
			if (m_queuedActions[i].IsActiveQueued())
			{
				continue;
			}
			while (true)
			{
				m_queuedActions[i].SetUpMovement(m_clickCancelled);
				m_movementQueueIndex = i;
				m_movementQueueIndexToChangeTo = i;
				m_changedDisplay = true;
				return;
			}
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void RemoveMovementFromQueue()
	{
		int num = 0;
		while (true)
		{
			if (num < 6)
			{
				if (m_queuedActions[num].IsMovement())
				{
					break;
				}
				num++;
				continue;
			}
			return;
		}
		while (true)
		{
			int num2;
			if (num != 5)
			{
				num2 = ((!m_queuedActions[num + 1].IsActiveQueued()) ? 1 : 0);
			}
			else
			{
				num2 = 1;
			}
			bool flag = (byte)num2 != 0;
			if (!flag)
			{
				m_abilityListRefreshed = true;
			}
			m_abilityCancelRequested = true;
			m_queuedActions[num].ClearAction(flag, !flag);
			m_changedDisplay = true;
			return;
		}
	}

	private void UpdateQueuedMovement()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (!(activeOwnedActorData != null))
		{
			return;
		}
		while (true)
		{
			if (activeOwnedActorData.HasQueuedMovement())
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						if (!m_isMovementQueued)
						{
							AddMovementToQueue();
						}
						m_isMovementQueued = true;
						return;
					}
				}
			}
			if (m_isMovementQueued)
			{
				RemoveMovementFromQueue();
			}
			m_isMovementQueued = false;
			return;
		}
	}

	public void RemovedAbility(AbilityData.ActionType actionType)
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null)
		{
			AbilityData abilityData = activeOwnedActorData.GetAbilityData();
			List<AbilityData.AbilityEntry> queuedOrAimingAbilities = abilityData.GetQueuedOrAimingAbilities();
			for (int i = 0; i < queuedOrAimingAbilities.Count; i++)
			{
				if (abilityData.GetActionTypeOfAbility(queuedOrAimingAbilities[i].ability) != actionType)
				{
					continue;
				}
				if (!m_isMovementQueued)
				{
					break;
				}
				if (i < m_movementQueueIndex)
				{
					m_movementQueueIndexToChangeTo = m_movementQueueIndex - 1;
				}
				break;
			}
		}
		m_changedDisplay = true;
	}

	public void NotifyLockedIn(bool isLockedIn)
	{
		for (int i = 0; i < 6; i++)
		{
			m_queuedActions[i].NotifyLockedIn(isLockedIn);
		}
		m_lockedIn = isLockedIn;
		m_colorchangeStartTime = Time.time;
		if (m_lockedIn)
		{
			m_colorDistance = Vector3.Distance(new Vector3(queueLabelColor.r, queueLabelColor.g, queueLabelColor.b), Vector3.one);
		}
		else
		{
			m_colorDistance = Vector3.Distance(new Vector3(queueLabelColor.r, queueLabelColor.g, queueLabelColor.b), new Vector3(m_labelColor.r, m_labelColor.g, m_labelColor.b));
		}
	}

	public void SelectedTaunt(AbilityData.ActionType entry)
	{
		for (int i = 0; i < 6; i++)
		{
			if (m_queuedActions[i].IsActiveQueued())
			{
				if (m_queuedActions[i].GetActionType() == entry)
				{
					m_queuedActions[i].EnabledCancelTauntButton(true);
					return;
				}
			}
		}
		while (true)
		{
			switch (4)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	public void RefreshQueueList()
	{
		for (int i = 0; i < 6; i++)
		{
			m_queuedActions[i].ClearAction(true, m_clickCancelled);
		}
		if (m_isMovementQueued)
		{
			m_queuedActions[m_movementQueueIndex].SetUpMovement(m_clickCancelled);
		}
		m_clickCancelled = false;
		m_abilityListRefreshed = true;
		m_changedDisplay = true;
	}

	private void UpdateQueuedAbilities()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (!(activeOwnedActorData != null))
		{
			return;
		}
		while (true)
		{
			AbilityData abilityData = activeOwnedActorData.GetAbilityData();
			List<AbilityData.AbilityEntry> list = new List<AbilityData.AbilityEntry>();
			int num = 0;
			List<AbilityData.ActionType> autoQueuedRequestActionTypes = activeOwnedActorData.GetActorTurnSM().GetAutoQueuedRequestActionTypes();
			for (int i = 0; i < autoQueuedRequestActionTypes.Count; i++)
			{
				if (abilityData.GetAbilityEntryOfActionType(autoQueuedRequestActionTypes[i]) != null)
				{
					list.Add(abilityData.GetAbilityEntryOfActionType(autoQueuedRequestActionTypes[i]));
				}
				num++;
			}
			while (true)
			{
				List<ActorTurnSM.ActionRequestForUndo> requestStackForUndo = activeOwnedActorData.GetActorTurnSM().GetRequestStackForUndo();
				for (int j = 0; j < requestStackForUndo.Count; j++)
				{
					if (abilityData.GetAbilityEntryOfActionType(requestStackForUndo[j].m_action) != null)
					{
						list.Add(abilityData.GetAbilityEntryOfActionType(requestStackForUndo[j].m_action));
					}
				}
				while (true)
				{
					int num2 = 0;
					for (int k = 0; k < 6; k++)
					{
						if (m_queuedActions[k].IsMovement())
						{
							continue;
						}
						if (num2 < list.Count)
						{
							if (m_queuedActions[k].GetQueuedAbility() != null)
							{
								if (m_queuedActions[k].GetQueuedAbility().ability == list[num2].ability)
								{
									num2++;
									continue;
								}
							}
						}
						if (num2 < list.Count)
						{
							if (m_queuedActions[k].GetQueuedAbility() != null)
							{
								if (!(m_queuedActions[k].GetQueuedAbility().ability != list[num2].ability))
								{
									if (m_queuedActions[k].GetQueuedAbility().ability != list[num2].ability)
									{
										m_queuedActions[k].ClearAction(true, m_abilityListRefreshed);
										m_changedDisplay = true;
									}
									continue;
								}
							}
							m_queuedActions[k].SetupAbility(list[num2], abilityData.GetActionTypeOfAbility(list[num2].ability), abilityData, m_abilityListRefreshed);
							m_queuedActions[k].m_abilityTransform.m_canCancel = (num2 >= num);
							num2++;
							m_changedDisplay = true;
						}
						else if (m_queuedActions[k].IsActiveQueued())
						{
							m_changedDisplay = true;
							m_queuedActions[k].ClearAction(true, m_abilityListRefreshed);
						}
					}
					m_abilityListRefreshed = false;
					return;
				}
			}
		}
	}

	private void ProcessQueuedAbilities()
	{
		if (!m_abilityCancelRequested)
		{
			UpdateQueuedAbilities();
		}
	}

	public void DoneQueueFadeAnim()
	{
		bool doActive = false;
		for (int i = 0; i < 6; i++)
		{
			if (m_queuedActions[i].IsActiveQueued())
			{
				doActive = true;
			}
		}
		UIManager.SetGameObjectActive(m_queueLabel, doActive);
	}

	public void JustClickCancelled()
	{
		m_clickCancelled = true;
	}

	public void AbilityCancelAnimDone()
	{
		m_abilityCancelRequested = false;
		m_abilityListRefreshed = true;
		bool flag;
		if (m_movementQueueIndexToChangeTo != m_movementQueueIndex)
		{
			m_queuedActions[m_movementQueueIndex].ClearAction(false, true);
			m_queuedActions[m_movementQueueIndex].SetCardTransformVisible(false);
			m_queuedActions[m_movementQueueIndex].SetAbilityTransformVisible(false);
			m_queuedActions[m_movementQueueIndexToChangeTo].SetUpMovement(true);
			flag = false;
			if (m_movementQueueIndexToChangeTo != m_queuedActions.Length - 1)
			{
				if (m_queuedActions[m_movementQueueIndexToChangeTo + 1].IsActiveQueued())
				{
					goto IL_00c1;
				}
			}
			flag = true;
			goto IL_00c1;
		}
		goto IL_00ed;
		IL_00c1:
		if (flag)
		{
			m_queuedActions[m_movementQueueIndexToChangeTo].SetGlow(true);
		}
		m_movementQueueIndex = m_movementQueueIndexToChangeTo;
		goto IL_00ed;
		IL_00ed:
		SetCancelAnimPlaying(false);
	}

	public bool IsAnimCancelPlaying()
	{
		return m_cancelAnimationPlaying;
	}

	public void SetCancelAnimPlaying(bool isPlaying)
	{
		m_cancelAnimationPlaying = isPlaying;
	}

	public void CancelAbilityRequest(AbilityData.ActionType actionType)
	{
		for (int i = 0; i < 6; i++)
		{
			m_queuedActions[i].SetGlow(false);
			if (!m_queuedActions[i].IsActiveQueued())
			{
				continue;
			}
			if (m_queuedActions[i].GetActionType() == actionType)
			{
				m_queuedActions[i].m_animationController.Play("QueueItem_LeaveQueue");
				SetCancelAnimPlaying(true);
			}
		}
		while (true)
		{
			RemovedAbility(actionType);
			m_abilityCancelRequested = true;
			return;
		}
	}

	private void Update()
	{
		if (GameFlowData.Get() != null && GameFlowData.Get().IsInDecisionState())
		{
			if (m_changedDisplay)
			{
				m_changedDisplay = false;
				bool flag = false;
				int num = 0;
				for (int i = 0; i < 6; i++)
				{
					m_queuedActions[i].SetGlow(false);
					if (!m_queuedActions[i].IsActiveQueued())
					{
						continue;
					}
					if (i != num + 1)
					{
						if (i != 0)
						{
							if (!m_cancelAnimationPlaying)
							{
								m_queuedActions[i].ClearAction();
							}
							continue;
						}
					}
					flag = true;
					num = i;
				}
				m_queuedActions[num].SetGlow(true);
				if (!flag)
				{
					m_animationController.Play("QueuePanel_EmptyQueue");
					m_labelVisible = false;
					queueLabelOpacity = 1f;
					m_cancelAnimationPlaying = false;
				}
				else if (!m_queueLabel.gameObject.activeSelf)
				{
					UIManager.SetGameObjectActive(m_queueLabel, true);
					queueLabelOpacity = 0f;
					m_labelVisible = true;
					m_animationController.Play("QueuePanel_FirstQueue");
				}
			}
			UpdateQueuedMovement();
			ProcessQueuedAbilities();
		}
		if (m_labelVisible)
		{
			SetQueueLineOpacity(queueLabelOpacity + Time.deltaTime);
		}
		else
		{
			SetQueueLineOpacity(queueLabelOpacity - Time.deltaTime);
		}
		if (m_lockedIn)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (m_colorDistance != 0f)
					{
						Vector3 a = new Vector3(queueLabelColor.r, queueLabelColor.g, queueLabelColor.b);
						float num2 = Time.time - m_colorchangeStartTime;
						float t = num2 / m_colorDistance;
						a = Vector3.Lerp(a, Vector3.one, t);
						queueLabelColor.r = a.x;
						queueLabelColor.g = a.y;
						queueLabelColor.b = a.z;
						SetQueueLineColor(queueLabelColor);
					}
					return;
				}
			}
		}
		if (m_colorDistance == 0f)
		{
			return;
		}
		while (true)
		{
			Vector3 a2 = new Vector3(queueLabelColor.r, queueLabelColor.g, queueLabelColor.b);
			float num3 = Time.time - m_colorchangeStartTime;
			float t2 = num3 / m_colorDistance;
			a2 = Vector3.Lerp(a2, new Vector3(m_labelColor.r, m_labelColor.g, m_labelColor.b), t2);
			queueLabelColor.r = a2.x;
			queueLabelColor.g = a2.y;
			queueLabelColor.b = a2.z;
			SetQueueLineColor(queueLabelColor);
			return;
		}
	}

	private void OnEnable()
	{
		Update();
	}
}
