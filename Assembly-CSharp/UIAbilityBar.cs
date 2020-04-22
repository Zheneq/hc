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
		UIManager.SetGameObjectActive(m_lockInTutorialTip, false);
	}

	private void Start()
	{
		m_lastSecondDisplayed = 1000;
		m_lockInCancelButton.SetClickCallback(OnLockCancelClick);
		for (int i = 5; i < 7; i++)
		{
			UIManager.SetGameObjectActive(m_abilityButtons[i], false);
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_cardBar, false);
			return;
		}
	}

	private void UpdateElementsVisibility(bool isResolving)
	{
		bool flag = false;
		ActorData activeOwnedActorData = GameFlowData.Get().activeOwnedActorData;
		if (activeOwnedActorData != null)
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
			flag = activeOwnedActorData.IsDead();
		}
		int num;
		if (isResolving)
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
			num = ((!flag) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag2 = (byte)num != 0;
		int num2;
		if (isResolving)
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
			if (!flag)
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
				num2 = ((activeOwnedActorData != null) ? 1 : 0);
				goto IL_0073;
			}
		}
		num2 = 0;
		goto IL_0073;
		IL_0090:
		int num3;
		bool flag3 = (byte)num3 != 0;
		bool flag4;
		bool doActive = flag4 || flag3;
		if (SinglePlayerManager.Get() != null)
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
			if (SinglePlayerManager.Get().GetDecisionTimerForceOff())
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				flag4 = false;
			}
			if (SinglePlayerManager.Get().GetLockInCancelButtonForceOff())
			{
				flag3 = false;
			}
		}
		if (!flag2)
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
			m_theTimer.SetGameObjectActive(flag4);
			if (flag4)
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
				HUD_UI.Get().m_mainScreenPanel.m_playerDisplayPanel.NotifyDecisionTimerShow();
				HUD_UI.Get().m_mainScreenPanel.m_queueListPanel.NotifyDecisionTimerShow();
			}
			UIManager.SetGameObjectActive(m_lockInCancelButton, flag3);
			if (m_timerBackground != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					UIManager.SetGameObjectActive(m_timerBackground, doActive);
					return;
				}
			}
			return;
		}
		IL_0073:
		flag4 = ((byte)num2 != 0);
		if (isResolving)
		{
			while (true)
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
				num3 = ((activeOwnedActorData != null) ? 1 : 0);
				goto IL_0090;
			}
		}
		num3 = 0;
		goto IL_0090;
	}

	public void Setup(ActorData actorData)
	{
		bool flag = CardManager.Get().ShowingInGameCardUI;
		bool flag2 = actorData != null;
		m_actorData = actorData;
		m_abilityData = null;
		m_turnSM = null;
		if (actorData != null)
		{
			m_abilityData = actorData.GetComponent<AbilityData>();
			m_turnSM = actorData.GetComponent<ActorTurnSM>();
			AbilityData.AbilityEntry[] abilityEntries = m_abilityData.abilityEntries;
			for (int i = 0; i < 5; i++)
			{
				AbilityData.ActionType actionType = (AbilityData.ActionType)i;
				UIManager.SetGameObjectActive(m_abilityButtons[i], true);
				m_abilityButtons[i].Setup(abilityEntries[i], actionType, m_abilityData);
			}
		}
		else
		{
			for (int j = 0; j < 5; j++)
			{
				UIManager.SetGameObjectActive(m_abilityButtons[j], false);
			}
			while (true)
			{
				switch (7)
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
			flag = false;
		}
		if (flag)
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
			UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_cardBar, true);
			HUD_UI.Get().m_mainScreenPanel.m_cardBar.Setup(m_abilityData);
		}
		else
		{
			UIManager.SetGameObjectActive(HUD_UI.Get().m_mainScreenPanel.m_cardBar, false);
		}
		m_theTimer.SetGameObjectActive(flag2);
		UIManager.SetGameObjectActive(m_lockInCancelButton, flag2);
		if (!(m_timerBackground != null))
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
			UIManager.SetGameObjectActive(m_timerBackground, flag2);
			return;
		}
	}

	public void RefreshHotkeys()
	{
		for (int i = 0; i < 5; i++)
		{
			m_abilityButtons[i].RefreshHotkey();
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			return;
		}
	}

	private void ProcessLowTimePulses()
	{
		TimeBank component = m_abilityData.GetComponent<TimeBank>();
		float f = component.TimeToDisplay();
		int num = Mathf.FloorToInt(f);
		if (num == m_lastSecondDisplayed)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (num != 0)
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
				if ((double)num != 1.0)
				{
					if (num < 0)
					{
						HUD_UI.Get().m_mainScreenPanel.m_alertDisplay.TriggerLowTimePulse(UIAlertDisplay.LowTimePulseType.UsingTimeBank);
					}
					else
					{
						if (num != 10)
						{
							if (num > 5)
							{
								goto IL_00b3;
							}
							while (true)
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
					}
					goto IL_00b3;
				}
			}
			HUD_UI.Get().m_mainScreenPanel.m_alertDisplay.TriggerLowTimePulse(UIAlertDisplay.LowTimePulseType.TurnEndWarning);
			goto IL_00b3;
			IL_00b3:
			m_lastSecondDisplayed = num;
			return;
		}
	}

	private void UpdateTimer(bool isResolving)
	{
		if (m_theTimer == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		if (m_abilityData == null)
		{
			while (true)
			{
				switch (7)
				{
				default:
					return;
				case 0:
					break;
				}
			}
		}
		if (isResolving)
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
			TimeBank component = m_abilityData.GetComponent<TimeBank>();
			ActorTurnSM component2 = m_abilityData.GetComponent<ActorTurnSM>();
			float num = component.TimeToDisplay();
			ProcessLowTimePulses();
			float num2 = GameFlowData.Get().m_maxTurnTime - GameWideData.Get().m_tbConsumableDuration;
			float num3 = num / num2;
			m_theTimer.UpdateSliderAmount(num3);
			m_theTimer.UpdateTimeGainedAmount(m_lastSavedStartTime, num3, !isResolving);
			m_theTimer.UpdateTimeLabels(m_theTimer.m_secondsLabel, m_theTimer.m_millisecondsLabel, component.TimeToDisplay());
			if (!component2.AmStillDeciding())
			{
				while (true)
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
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
						{
							float num4 = component.GetTimeSaved() / num2;
							m_theTimer.SetGlowingTimer(true);
							m_theTimer.UpdateSavedTimerPosition(num4);
							m_lastSavedStartTime = num4;
							return;
						}
						}
					}
				}
			}
			m_theTimer.SetGlowingTimer(false);
			return;
		}
	}

	private void UpdateLockInButton(bool isResolving)
	{
		if (m_turnSM != null && m_abilityData != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (!isResolving)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								break;
							default:
							{
								HUD_UI.Get().m_mainScreenPanel.m_abilityBar.m_theTimer.m_abilityUsedTracker.ClearAllAbilties();
								ActorTurnSM turnSM = m_turnSM;
								m_lockInCancelButton.EnableLockIn(turnSM.CurrentState != TurnStateEnum.CONFIRMED, !m_turnSM.AmTargetingAction());
								m_lockInCancelButton.SetDecisionContainerVisible(true, m_actorData.IsDead());
								m_theTimer.NotifyPhase(true);
								UIManager.SetGameObjectActive(m_lockInCancelButton.m_phaseColor, false);
								return;
							}
							}
						}
					}
					m_lockInCancelButton.EnableLockIn(false, true);
					m_lockInCancelButton.SetDecisionContainerVisible(false, m_actorData.IsDead());
					m_theTimer.NotifyPhase(false);
					m_lockInCancelButton.UpdatePhase();
					if (SinglePlayerManager.Get() != null)
					{
						while (true)
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
							UIManager.SetGameObjectActive(m_lockInCancelButton.m_phaseColor, false);
							return;
						}
					}
					UIManager.SetGameObjectActive(m_lockInCancelButton.m_phaseColor, true);
					return;
				}
			}
		}
		m_theTimer.NotifyPhase(true);
	}

	private void Update()
	{
		if (GameFlowData.Get() == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		bool isResolving = IsResolving();
		if (GameFlowData.Get().activeOwnedActorData != m_actorData)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			Setup(GameFlowData.Get().activeOwnedActorData);
		}
		UpdateLockInButton(isResolving);
		UpdateElementsVisibility(isResolving);
		UpdateTimer(isResolving);
	}

	private bool IsResolving()
	{
		return GameFlowData.Get().gameState == GameState.BothTeams_Resolve;
	}

	private void OnLockCancelClick(BaseEventData data)
	{
		if (m_lockInCancelButton.IsShowingLockIn())
		{
			OnEndTurnClick(data);
		}
		else
		{
			OnCancelClick(data);
		}
	}

	private void OnCancelClick(BaseEventData data)
	{
		if (!(m_turnSM != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			m_turnSM.RequestCancel();
			m_lockInCancelButton.CancelClicked();
			return;
		}
	}

	private void OnEndTurnClick(BaseEventData data)
	{
		if (m_turnSM.CheckStateForEndTurnRequestFromInput())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					UISounds.GetUISounds().Play("ui/ingame/v1/hud/lockin");
					m_turnSM.RequestEndTurn();
					m_lockInCancelButton.LockedInClicked();
					return;
				}
			}
		}
		if (m_turnSM.CurrentState != TurnStateEnum.VALIDATING_ACTION_REQUEST)
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
			if (m_turnSM.CurrentState != TurnStateEnum.VALIDATING_MOVE_REQUEST)
			{
				return;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_turnSM.LockInBuffered = true;
	}

	public void DoAbilityButtonClick(KeyPreference abilitySelectDown)
	{
		for (int i = 0; i < 5; i++)
		{
			if (m_abilityButtons[i].GetKeyPreference() == abilitySelectDown)
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
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				m_abilityButtons[i].OnAbilityButtonClick(null);
			}
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			HUD_UI.Get().m_mainScreenPanel.m_cardBar.DoAbilityButtonClick(abilitySelectDown);
			return;
		}
	}
}
