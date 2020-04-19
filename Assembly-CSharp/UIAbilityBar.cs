using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIAbilityBar : MonoBehaviour
{
	private const int NUM_DISPLAYED_ABILITIES = 5;

	public UIAbilityButton[] m_abilityButtons;

	public UILockCancelButton m_lockInCancelButton;

	public Animator m_animController;

	public GameObject m_timerBackground;

	public UIDecisionTimer m_theTimer;

	public GameObject m_lockInTutorialTip;

	public GameObject m_lockInTutorialTipSubImage;

	public float m_finishedDecisionHeight = 0.45f;

	public float m_makingDecisionHeight = 1f;

	private AbilityData m_abilityData;

	private ActorTurnSM m_turnSM;

	private ActorData m_actorData;

	private float m_lastSavedStartTime;

	private int m_lastSecondDisplayed;

	public void Awake()
	{
		UIManager.SetGameObjectActive(this.m_lockInTutorialTip, false, null);
	}

	private void Start()
	{
		this.m_lastSecondDisplayed = 0x3E8;
		this.m_lockInCancelButton.SetClickCallback(new UIEventTriggerUtils.EventDelegate(this.OnLockCancelClick));
		for (int i = 5; i < 7; i++)
		{
			UIManager.SetGameObjectActive(this.m_abilityButtons[i], false, null);
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityBar.Start()).MethodHandle;
		}
		UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_cardBar, false, null);
	}

	private void UpdateElementsVisibility(bool isResolving)
	{
		bool flag = false;
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityBar.UpdateElementsVisibility(bool)).MethodHandle;
			}
			flag = activeOwnedActorData.\u000E();
		}
		bool flag2;
		if (isResolving)
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
			flag2 = !flag;
		}
		else
		{
			flag2 = false;
		}
		bool flag3 = flag2;
		bool flag4;
		if (isResolving)
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
			if (!flag)
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
				flag4 = (activeOwnedActorData != null);
				goto IL_73;
			}
		}
		flag4 = false;
		IL_73:
		bool flag5 = flag4;
		bool flag6;
		if (isResolving)
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
			if (!flag)
			{
				flag6 = (activeOwnedActorData != null);
				goto IL_90;
			}
		}
		flag6 = false;
		IL_90:
		bool flag7 = flag6;
		bool doActive = flag5 || flag7;
		if (SinglePlayerManager.Get() != null)
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
			if (SinglePlayerManager.Get().GetDecisionTimerForceOff())
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
				flag5 = false;
			}
			if (SinglePlayerManager.Get().GetLockInCancelButtonForceOff())
			{
				flag7 = false;
			}
		}
		if (flag3)
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
			this.m_theTimer.SetGameObjectActive(flag5);
			if (flag5)
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
				HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.NotifyDecisionTimerShow();
				HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.NotifyDecisionTimerShow();
			}
			UIManager.SetGameObjectActive(this.m_lockInCancelButton, flag7, null);
			if (this.m_timerBackground != null)
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
				UIManager.SetGameObjectActive(this.m_timerBackground, doActive, null);
			}
		}
	}

	public void Setup(ActorData actorData)
	{
		bool flag = CardManager.Get().ShowingInGameCardUI;
		bool flag2 = actorData != null;
		this.m_actorData = actorData;
		this.m_abilityData = null;
		this.m_turnSM = null;
		if (actorData != null)
		{
			this.m_abilityData = actorData.GetComponent<AbilityData>();
			this.m_turnSM = actorData.GetComponent<ActorTurnSM>();
			AbilityData.AbilityEntry[] abilityEntries = this.m_abilityData.abilityEntries;
			for (int i = 0; i < 5; i++)
			{
				AbilityData.ActionType actionType = (AbilityData.ActionType)i;
				UIManager.SetGameObjectActive(this.m_abilityButtons[i], true, null);
				this.m_abilityButtons[i].Setup(abilityEntries[i], actionType, this.m_abilityData);
			}
		}
		else
		{
			for (int j = 0; j < 5; j++)
			{
				UIManager.SetGameObjectActive(this.m_abilityButtons[j], false, null);
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityBar.Setup(ActorData)).MethodHandle;
			}
			flag = false;
		}
		if (flag)
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
			UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_cardBar, true, null);
			HUD_UI.Get().m_mainScreenPanel.m_cardBar.Setup(this.m_abilityData);
		}
		else
		{
			UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_cardBar, false, null);
		}
		this.m_theTimer.SetGameObjectActive(flag2);
		UIManager.SetGameObjectActive(this.m_lockInCancelButton, flag2, null);
		if (this.m_timerBackground != null)
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
			UIManager.SetGameObjectActive(this.m_timerBackground, flag2, null);
		}
	}

	public void RefreshHotkeys()
	{
		for (int i = 0; i < 5; i++)
		{
			this.m_abilityButtons[i].RefreshHotkey();
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityBar.RefreshHotkeys()).MethodHandle;
		}
	}

	private void ProcessLowTimePulses()
	{
		TimeBank component = this.m_abilityData.GetComponent<TimeBank>();
		float f = component.TimeToDisplay();
		int num = Mathf.FloorToInt(f);
		if (num != this.m_lastSecondDisplayed)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityBar.ProcessLowTimePulses()).MethodHandle;
			}
			if (num != 0)
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
				if ((double)num != 1.0)
				{
					if (num < 0)
					{
						HUD_UI.Get().m_mainScreenPanel.m_alertDisplay.TriggerLowTimePulse(UIAlertDisplay.LowTimePulseType.UsingTimeBank);
						goto IL_B3;
					}
					if (num != 0xA)
					{
						if (num > 5)
						{
							goto IL_B3;
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
					}
					HUD_UI.Get().m_mainScreenPanel.m_alertDisplay.TriggerLowTimePulse(UIAlertDisplay.LowTimePulseType.Standard);
					goto IL_B3;
				}
			}
			HUD_UI.Get().m_mainScreenPanel.m_alertDisplay.TriggerLowTimePulse(UIAlertDisplay.LowTimePulseType.TurnEndWarning);
			IL_B3:
			this.m_lastSecondDisplayed = num;
		}
	}

	private void UpdateTimer(bool isResolving)
	{
		if (this.m_theTimer == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityBar.UpdateTimer(bool)).MethodHandle;
			}
			return;
		}
		if (this.m_abilityData == null)
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
			return;
		}
		if (!isResolving)
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
			TimeBank component = this.m_abilityData.GetComponent<TimeBank>();
			ActorTurnSM component2 = this.m_abilityData.GetComponent<ActorTurnSM>();
			float num = component.TimeToDisplay();
			this.ProcessLowTimePulses();
			float num2 = GameFlowData.Get().m_maxTurnTime - GameWideData.Get().m_tbConsumableDuration;
			float num3 = num / num2;
			this.m_theTimer.UpdateSliderAmount(num3);
			this.m_theTimer.UpdateTimeGainedAmount(this.m_lastSavedStartTime, num3, !isResolving);
			this.m_theTimer.UpdateTimeLabels(this.m_theTimer.m_secondsLabel, this.m_theTimer.m_millisecondsLabel, component.TimeToDisplay());
			if (!component2.AmStillDeciding())
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
				if (component.GetTimeSaved() > 0f)
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
					float num4 = component.GetTimeSaved() / num2;
					this.m_theTimer.SetGlowingTimer(true);
					this.m_theTimer.UpdateSavedTimerPosition(num4);
					this.m_lastSavedStartTime = num4;
					return;
				}
			}
			this.m_theTimer.SetGlowingTimer(false);
		}
	}

	private void UpdateLockInButton(bool isResolving)
	{
		if (this.m_turnSM != null && this.m_abilityData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityBar.UpdateLockInButton(bool)).MethodHandle;
			}
			if (!isResolving)
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
				HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.ClearAllAbilties(UIQueueListPanel.UIPhase.None);
				ActorTurnSM turnSM = this.m_turnSM;
				this.m_lockInCancelButton.EnableLockIn(turnSM.CurrentState != TurnStateEnum.CONFIRMED, !this.m_turnSM.AmTargetingAction());
				this.m_lockInCancelButton.SetDecisionContainerVisible(true, this.m_actorData.\u000E());
				this.m_theTimer.NotifyPhase(true);
				UIManager.SetGameObjectActive(this.m_lockInCancelButton.m_phaseColor, false, null);
			}
			else
			{
				this.m_lockInCancelButton.EnableLockIn(false, true);
				this.m_lockInCancelButton.SetDecisionContainerVisible(false, this.m_actorData.\u000E());
				this.m_theTimer.NotifyPhase(false);
				this.m_lockInCancelButton.UpdatePhase();
				if (SinglePlayerManager.Get() != null)
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
					if (SinglePlayerManager.Get().GetLockinPhaseColorForceOff())
					{
						UIManager.SetGameObjectActive(this.m_lockInCancelButton.m_phaseColor, false, null);
						goto IL_164;
					}
				}
				UIManager.SetGameObjectActive(this.m_lockInCancelButton.m_phaseColor, true, null);
			}
			IL_164:;
		}
		else
		{
			this.m_theTimer.NotifyPhase(true);
		}
	}

	private void Update()
	{
		if (GameFlowData.Get() == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityBar.Update()).MethodHandle;
			}
			return;
		}
		bool isResolving = this.IsResolving();
		if (GameFlowData.Get().activeOwnedActorData != this.m_actorData)
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
			this.Setup(GameFlowData.Get().activeOwnedActorData);
		}
		this.UpdateLockInButton(isResolving);
		this.UpdateElementsVisibility(isResolving);
		this.UpdateTimer(isResolving);
	}

	private bool IsResolving()
	{
		return GameFlowData.Get().gameState == GameState.BothTeams_Resolve;
	}

	private void OnLockCancelClick(BaseEventData data)
	{
		if (this.m_lockInCancelButton.IsShowingLockIn())
		{
			this.OnEndTurnClick(data);
		}
		else
		{
			this.OnCancelClick(data);
		}
	}

	private void OnCancelClick(BaseEventData data)
	{
		if (this.m_turnSM != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityBar.OnCancelClick(BaseEventData)).MethodHandle;
			}
			this.m_turnSM.RequestCancel(false);
			this.m_lockInCancelButton.CancelClicked();
		}
	}

	private void OnEndTurnClick(BaseEventData data)
	{
		if (this.m_turnSM.CheckStateForEndTurnRequestFromInput())
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityBar.OnEndTurnClick(BaseEventData)).MethodHandle;
			}
			UISounds.GetUISounds().Play("ui/ingame/v1/hud/lockin");
			this.m_turnSM.RequestEndTurn();
			this.m_lockInCancelButton.LockedInClicked();
		}
		else
		{
			if (this.m_turnSM.CurrentState != TurnStateEnum.VALIDATING_ACTION_REQUEST)
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
				if (this.m_turnSM.CurrentState != TurnStateEnum.VALIDATING_MOVE_REQUEST)
				{
					return;
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
			}
			this.m_turnSM.LockInBuffered = true;
		}
	}

	public void DoAbilityButtonClick(KeyPreference abilitySelectDown)
	{
		for (int i = 0; i < 5; i++)
		{
			if (this.m_abilityButtons[i].GetKeyPreference() == abilitySelectDown)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(UIAbilityBar.DoAbilityButtonClick(KeyPreference)).MethodHandle;
				}
				this.m_abilityButtons[i].OnAbilityButtonClick(null);
			}
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
		HUD_UI.Get().m_mainScreenPanel.m_cardBar.DoAbilityButtonClick(abilitySelectDown);
	}
}
