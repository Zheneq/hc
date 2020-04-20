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
		UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_cardBar, false, null);
	}

	private void UpdateElementsVisibility(bool isResolving)
	{
		bool flag = false;
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null)
		{
			flag = activeOwnedActorData.IsDead();
		}
		bool flag2;
		if (isResolving)
		{
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
			if (!flag)
			{
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
			if (SinglePlayerManager.Get().GetDecisionTimerForceOff())
			{
				flag5 = false;
			}
			if (SinglePlayerManager.Get().GetLockInCancelButtonForceOff())
			{
				flag7 = false;
			}
		}
		if (flag3)
		{
			this.m_theTimer.SetGameObjectActive(flag5);
			if (flag5)
			{
				HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.NotifyDecisionTimerShow();
				HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.NotifyDecisionTimerShow();
			}
			UIManager.SetGameObjectActive(this.m_lockInCancelButton, flag7, null);
			if (this.m_timerBackground != null)
			{
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
			flag = false;
		}
		if (flag)
		{
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
			UIManager.SetGameObjectActive(this.m_timerBackground, flag2, null);
		}
	}

	public void RefreshHotkeys()
	{
		for (int i = 0; i < 5; i++)
		{
			this.m_abilityButtons[i].RefreshHotkey();
		}
	}

	private void ProcessLowTimePulses()
	{
		TimeBank component = this.m_abilityData.GetComponent<TimeBank>();
		float f = component.TimeToDisplay();
		int num = Mathf.FloorToInt(f);
		if (num != this.m_lastSecondDisplayed)
		{
			if (num != 0)
			{
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
			return;
		}
		if (this.m_abilityData == null)
		{
			return;
		}
		if (!isResolving)
		{
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
				if (component.GetTimeSaved() > 0f)
				{
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
			if (!isResolving)
			{
				HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.ClearAllAbilties(UIQueueListPanel.UIPhase.None);
				ActorTurnSM turnSM = this.m_turnSM;
				this.m_lockInCancelButton.EnableLockIn(turnSM.CurrentState != TurnStateEnum.CONFIRMED, !this.m_turnSM.AmTargetingAction());
				this.m_lockInCancelButton.SetDecisionContainerVisible(true, this.m_actorData.IsDead());
				this.m_theTimer.NotifyPhase(true);
				UIManager.SetGameObjectActive(this.m_lockInCancelButton.m_phaseColor, false, null);
			}
			else
			{
				this.m_lockInCancelButton.EnableLockIn(false, true);
				this.m_lockInCancelButton.SetDecisionContainerVisible(false, this.m_actorData.IsDead());
				this.m_theTimer.NotifyPhase(false);
				this.m_lockInCancelButton.UpdatePhase();
				if (SinglePlayerManager.Get() != null)
				{
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
			return;
		}
		bool isResolving = this.IsResolving();
		if (GameFlowData.Get().activeOwnedActorData != this.m_actorData)
		{
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
			this.m_turnSM.RequestCancel(false);
			this.m_lockInCancelButton.CancelClicked();
		}
	}

	private void OnEndTurnClick(BaseEventData data)
	{
		if (this.m_turnSM.CheckStateForEndTurnRequestFromInput())
		{
			UISounds.GetUISounds().Play("ui/ingame/v1/hud/lockin");
			this.m_turnSM.RequestEndTurn();
			this.m_lockInCancelButton.LockedInClicked();
		}
		else
		{
			if (this.m_turnSM.CurrentState != TurnStateEnum.VALIDATING_ACTION_REQUEST)
			{
				if (this.m_turnSM.CurrentState != TurnStateEnum.VALIDATING_MOVE_REQUEST)
				{
					return;
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
				this.m_abilityButtons[i].OnAbilityButtonClick(null);
			}
		}
		HUD_UI.Get().m_mainScreenPanel.m_cardBar.DoAbilityButtonClick(abilitySelectDown);
	}
}
