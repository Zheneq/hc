using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIQueueListPanel : MonoBehaviour
{
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

	public static UIQueueListPanel.UIPhase GetUIPhaseFromAbilityPriority(AbilityPriority priority)
	{
		UIQueueListPanel.UIPhase result = UIQueueListPanel.UIPhase.None;
		if (priority != AbilityPriority.Prep_Defense)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueueListPanel.GetUIPhaseFromAbilityPriority(AbilityPriority)).MethodHandle;
			}
			if (priority != AbilityPriority.Prep_Offense)
			{
				if (priority == AbilityPriority.Evasion)
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
					return UIQueueListPanel.UIPhase.Evasion;
				}
				if (priority != AbilityPriority.Combat_Damage)
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
					if (priority != AbilityPriority.DEPRICATED_Combat_Charge)
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
						if (priority != AbilityPriority.Combat_Knockback)
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
							if (priority != AbilityPriority.Combat_Final)
							{
								return result;
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
					}
				}
				return UIQueueListPanel.UIPhase.Combat;
			}
		}
		result = UIQueueListPanel.UIPhase.Prep;
		return result;
	}

	private void SetQueueLineColor(Color c)
	{
		Color color = this.m_queueLabel.color;
		color.r = c.r;
		color.g = c.g;
		color.b = c.b;
		this.m_queueLabel.color = color;
		this.queueLabelColor = c;
	}

	private void SetQueueLineOpacity(float f)
	{
		Color color = this.m_queueLabel.color;
		color.a = f;
		this.m_queueLabel.color = color;
		this.queueLabelOpacity = f;
	}

	private void Start()
	{
		if (this.m_CancelAllButton != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueueListPanel.Start()).MethodHandle;
			}
			UIEventTriggerUtils.AddListener(this.m_CancelAllButton.gameObject, EventTriggerType.PointerClick, new UIEventTriggerUtils.EventDelegate(this.OnCancelAllClick));
			UIEventTriggerUtils.AddListener(this.m_CancelAllButton.gameObject, EventTriggerType.PointerEnter, new UIEventTriggerUtils.EventDelegate(this.OnCancelAllMouseEnter));
			UIEventTriggerUtils.AddListener(this.m_CancelAllButton.gameObject, EventTriggerType.PointerExit, new UIEventTriggerUtils.EventDelegate(this.OnCancelAllMouseExit));
		}
		if (this.m_CancelAllHover != null)
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
			UIManager.SetGameObjectActive(this.m_CancelAllHover, false, null);
		}
		this.DoneQueueFadeAnim();
		this.SetQueueLineColor(this.m_labelColor);
	}

	public void NotifyDecisionTimerShow()
	{
		this.OnCancelAllClick(null);
	}

	public void OnCancelAllClick(BaseEventData data)
	{
		if (GameFlowData.Get().IsInDecisionState())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueueListPanel.OnCancelAllClick(BaseEventData)).MethodHandle;
			}
			if (GameFlowData.Get().activeOwnedActorData != null)
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
				AbilityData component = GameFlowData.Get().activeOwnedActorData.GetComponent<AbilityData>();
				ActorTurnSM component2 = component.GetComponent<ActorTurnSM>();
				for (int i = 0; i < 0xE; i++)
				{
					AbilityData.ActionType actionType = (AbilityData.ActionType)i;
					bool flag = component.HasQueuedAction(actionType);
					if (flag)
					{
						HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.RemovedAbility(actionType);
						component2.RequestCancelAction(actionType, false);
					}
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				component2.RequestCancelMovement();
			}
			HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.RefreshQueueList();
		}
	}

	public void OnCancelAllMouseEnter(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_CancelAllHover, true, null);
	}

	public void OnCancelAllMouseExit(BaseEventData data)
	{
		UIManager.SetGameObjectActive(this.m_CancelAllHover, false, null);
	}

	private void AddMovementToQueue()
	{
		for (int i = 0; i < 6; i++)
		{
			if (!this.m_queuedActions[i].IsActiveQueued())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueueListPanel.AddMovementToQueue()).MethodHandle;
				}
				this.m_queuedActions[i].SetUpMovement(this.m_clickCancelled);
				this.m_movementQueueIndex = i;
				this.m_movementQueueIndexToChangeTo = i;
				this.m_changedDisplay = true;
				return;
			}
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	private void RemoveMovementFromQueue()
	{
		for (int i = 0; i < 6; i++)
		{
			if (this.m_queuedActions[i].IsMovement())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueueListPanel.RemoveMovementFromQueue()).MethodHandle;
				}
				bool flag;
				if (i != 5)
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
					flag = !this.m_queuedActions[i + 1].IsActiveQueued();
				}
				else
				{
					flag = true;
				}
				bool flag2 = flag;
				if (!flag2)
				{
					this.m_abilityListRefreshed = true;
				}
				this.m_abilityCancelRequested = true;
				this.m_queuedActions[i].ClearAction(flag2, !flag2);
				this.m_changedDisplay = true;
				break;
			}
		}
	}

	private void UpdateQueuedMovement()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueueListPanel.UpdateQueuedMovement()).MethodHandle;
			}
			if (activeOwnedActorData.HasQueuedMovement())
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
				if (!this.m_isMovementQueued)
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
					this.AddMovementToQueue();
				}
				this.m_isMovementQueued = true;
			}
			else
			{
				if (this.m_isMovementQueued)
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
					this.RemoveMovementFromQueue();
				}
				this.m_isMovementQueued = false;
			}
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
				if (abilityData.GetActionTypeOfAbility(queuedOrAimingAbilities[i].ability) == actionType)
				{
					if (this.m_isMovementQueued)
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueueListPanel.RemovedAbility(AbilityData.ActionType)).MethodHandle;
						}
						if (i < this.m_movementQueueIndex)
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
							this.m_movementQueueIndexToChangeTo = this.m_movementQueueIndex - 1;
						}
					}
					break;
				}
			}
		}
		this.m_changedDisplay = true;
	}

	public void NotifyLockedIn(bool isLockedIn)
	{
		for (int i = 0; i < 6; i++)
		{
			this.m_queuedActions[i].NotifyLockedIn(isLockedIn);
		}
		this.m_lockedIn = isLockedIn;
		this.m_colorchangeStartTime = Time.time;
		if (this.m_lockedIn)
		{
			this.m_colorDistance = Vector3.Distance(new Vector3(this.queueLabelColor.r, this.queueLabelColor.g, this.queueLabelColor.b), Vector3.one);
		}
		else
		{
			this.m_colorDistance = Vector3.Distance(new Vector3(this.queueLabelColor.r, this.queueLabelColor.g, this.queueLabelColor.b), new Vector3(this.m_labelColor.r, this.m_labelColor.g, this.m_labelColor.b));
		}
	}

	public void SelectedTaunt(AbilityData.ActionType entry)
	{
		for (int i = 0; i < 6; i++)
		{
			if (this.m_queuedActions[i].IsActiveQueued())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueueListPanel.SelectedTaunt(AbilityData.ActionType)).MethodHandle;
				}
				if (this.m_queuedActions[i].GetActionType() == entry)
				{
					this.m_queuedActions[i].EnabledCancelTauntButton(true);
					return;
				}
			}
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			return;
		}
	}

	public void RefreshQueueList()
	{
		for (int i = 0; i < 6; i++)
		{
			this.m_queuedActions[i].ClearAction(true, this.m_clickCancelled);
		}
		if (this.m_isMovementQueued)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueueListPanel.RefreshQueueList()).MethodHandle;
			}
			this.m_queuedActions[this.m_movementQueueIndex].SetUpMovement(this.m_clickCancelled);
		}
		this.m_clickCancelled = false;
		this.m_abilityListRefreshed = true;
		this.m_changedDisplay = true;
	}

	private void UpdateQueuedAbilities()
	{
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueueListPanel.UpdateQueuedAbilities()).MethodHandle;
			}
			AbilityData abilityData = activeOwnedActorData.GetAbilityData();
			List<AbilityData.AbilityEntry> list = new List<AbilityData.AbilityEntry>();
			int num = 0;
			List<AbilityData.ActionType> autoQueuedRequestActionTypes = activeOwnedActorData.GetActorTurnSM().GetAutoQueuedRequestActionTypes();
			for (int i = 0; i < autoQueuedRequestActionTypes.Count; i++)
			{
				if (abilityData.GetAbilityEntryOfActionType(autoQueuedRequestActionTypes[i]) != null)
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
					list.Add(abilityData.GetAbilityEntryOfActionType(autoQueuedRequestActionTypes[i]));
				}
				num++;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			List<ActorTurnSM.ActionRequestForUndo> requestStackForUndo = activeOwnedActorData.GetActorTurnSM().GetRequestStackForUndo();
			for (int j = 0; j < requestStackForUndo.Count; j++)
			{
				if (abilityData.GetAbilityEntryOfActionType(requestStackForUndo[j].m_action) != null)
				{
					list.Add(abilityData.GetAbilityEntryOfActionType(requestStackForUndo[j].m_action));
				}
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			int num2 = 0;
			for (int k = 0; k < 6; k++)
			{
				if (this.m_queuedActions[k].IsMovement())
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
				}
				else
				{
					if (num2 < list.Count)
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
						if (this.m_queuedActions[k].GetQueuedAbility() != null)
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
							if (this.m_queuedActions[k].GetQueuedAbility().ability == list[num2].ability)
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
								num2++;
								goto IL_2FC;
							}
						}
					}
					if (num2 < list.Count)
					{
						if (this.m_queuedActions[k].GetQueuedAbility() == null)
						{
							goto IL_211;
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
						if (this.m_queuedActions[k].GetQueuedAbility().ability != list[num2].ability)
						{
							for (;;)
							{
								switch (5)
								{
								case 0:
									continue;
								}
								goto IL_211;
							}
						}
						else if (this.m_queuedActions[k].GetQueuedAbility().ability != list[num2].ability)
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
							this.m_queuedActions[k].ClearAction(true, this.m_abilityListRefreshed);
							this.m_changedDisplay = true;
						}
						goto IL_2FC;
						IL_211:
						this.m_queuedActions[k].SetupAbility(list[num2], abilityData.GetActionTypeOfAbility(list[num2].ability), abilityData, this.m_abilityListRefreshed);
						this.m_queuedActions[k].m_abilityTransform.m_canCancel = (num2 >= num);
						num2++;
						this.m_changedDisplay = true;
					}
					else if (this.m_queuedActions[k].IsActiveQueued())
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
						this.m_changedDisplay = true;
						this.m_queuedActions[k].ClearAction(true, this.m_abilityListRefreshed);
					}
				}
				IL_2FC:;
			}
			this.m_abilityListRefreshed = false;
		}
	}

	private void ProcessQueuedAbilities()
	{
		if (!this.m_abilityCancelRequested)
		{
			this.UpdateQueuedAbilities();
		}
	}

	public void DoneQueueFadeAnim()
	{
		bool doActive = false;
		for (int i = 0; i < 6; i++)
		{
			if (this.m_queuedActions[i].IsActiveQueued())
			{
				doActive = true;
			}
		}
		UIManager.SetGameObjectActive(this.m_queueLabel, doActive, null);
	}

	public void JustClickCancelled()
	{
		this.m_clickCancelled = true;
	}

	public void AbilityCancelAnimDone()
	{
		this.m_abilityCancelRequested = false;
		this.m_abilityListRefreshed = true;
		if (this.m_movementQueueIndexToChangeTo != this.m_movementQueueIndex)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueueListPanel.AbilityCancelAnimDone()).MethodHandle;
			}
			this.m_queuedActions[this.m_movementQueueIndex].ClearAction(false, true);
			this.m_queuedActions[this.m_movementQueueIndex].SetCardTransformVisible(false);
			this.m_queuedActions[this.m_movementQueueIndex].SetAbilityTransformVisible(false);
			this.m_queuedActions[this.m_movementQueueIndexToChangeTo].SetUpMovement(true);
			bool flag = false;
			if (this.m_movementQueueIndexToChangeTo != this.m_queuedActions.Length - 1)
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
				if (this.m_queuedActions[this.m_movementQueueIndexToChangeTo + 1].IsActiveQueued())
				{
					goto IL_C1;
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
			flag = true;
			IL_C1:
			if (flag)
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
				this.m_queuedActions[this.m_movementQueueIndexToChangeTo].SetGlow(true);
			}
			this.m_movementQueueIndex = this.m_movementQueueIndexToChangeTo;
		}
		this.SetCancelAnimPlaying(false);
	}

	public bool IsAnimCancelPlaying()
	{
		return this.m_cancelAnimationPlaying;
	}

	public void SetCancelAnimPlaying(bool isPlaying)
	{
		this.m_cancelAnimationPlaying = isPlaying;
	}

	public void CancelAbilityRequest(AbilityData.ActionType actionType)
	{
		for (int i = 0; i < 6; i++)
		{
			this.m_queuedActions[i].SetGlow(false);
			if (this.m_queuedActions[i].IsActiveQueued())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueueListPanel.CancelAbilityRequest(AbilityData.ActionType)).MethodHandle;
				}
				if (this.m_queuedActions[i].GetActionType() == actionType)
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
					this.m_queuedActions[i].m_animationController.Play("QueueItem_LeaveQueue");
					this.SetCancelAnimPlaying(true);
				}
			}
		}
		for (;;)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			break;
		}
		this.RemovedAbility(actionType);
		this.m_abilityCancelRequested = true;
	}

	private void Update()
	{
		if (GameFlowData.Get() != null && GameFlowData.Get().IsInDecisionState())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIQueueListPanel.Update()).MethodHandle;
			}
			if (this.m_changedDisplay)
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
				this.m_changedDisplay = false;
				bool flag = false;
				int num = 0;
				for (int i = 0; i < 6; i++)
				{
					this.m_queuedActions[i].SetGlow(false);
					if (this.m_queuedActions[i].IsActiveQueued())
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
						if (i != num + 1)
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
							if (i == 0)
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
							}
							else
							{
								if (!this.m_cancelAnimationPlaying)
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
									this.m_queuedActions[i].ClearAction();
									goto IL_C5;
								}
								goto IL_C5;
							}
						}
						flag = true;
						num = i;
					}
					IL_C5:;
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
				this.m_queuedActions[num].SetGlow(true);
				if (!flag)
				{
					this.m_animationController.Play("QueuePanel_EmptyQueue");
					this.m_labelVisible = false;
					this.queueLabelOpacity = 1f;
					this.m_cancelAnimationPlaying = false;
				}
				else if (!this.m_queueLabel.gameObject.activeSelf)
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
					UIManager.SetGameObjectActive(this.m_queueLabel, true, null);
					this.queueLabelOpacity = 0f;
					this.m_labelVisible = true;
					this.m_animationController.Play("QueuePanel_FirstQueue");
				}
			}
			this.UpdateQueuedMovement();
			this.ProcessQueuedAbilities();
		}
		if (this.m_labelVisible)
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
			this.SetQueueLineOpacity(this.queueLabelOpacity + Time.deltaTime);
		}
		else
		{
			this.SetQueueLineOpacity(this.queueLabelOpacity - Time.deltaTime);
		}
		if (this.m_lockedIn)
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
			if (this.m_colorDistance != 0f)
			{
				Vector3 a = new Vector3(this.queueLabelColor.r, this.queueLabelColor.g, this.queueLabelColor.b);
				float num2 = Time.time - this.m_colorchangeStartTime;
				float t = num2 / this.m_colorDistance;
				a = Vector3.Lerp(a, Vector3.one, t);
				this.queueLabelColor.r = a.x;
				this.queueLabelColor.g = a.y;
				this.queueLabelColor.b = a.z;
				this.SetQueueLineColor(this.queueLabelColor);
			}
		}
		else if (this.m_colorDistance != 0f)
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
			Vector3 a2 = new Vector3(this.queueLabelColor.r, this.queueLabelColor.g, this.queueLabelColor.b);
			float num3 = Time.time - this.m_colorchangeStartTime;
			float t2 = num3 / this.m_colorDistance;
			a2 = Vector3.Lerp(a2, new Vector3(this.m_labelColor.r, this.m_labelColor.g, this.m_labelColor.b), t2);
			this.queueLabelColor.r = a2.x;
			this.queueLabelColor.g = a2.y;
			this.queueLabelColor.b = a2.z;
			this.SetQueueLineColor(this.queueLabelColor);
		}
	}

	private void OnEnable()
	{
		this.Update();
	}

	public enum UIPhase
	{
		Prep,
		Evasion,
		Combat,
		Movement,
		None
	}
}
