using UnityEngine.Networking;

public class ResolvingState : TurnState
{
	private float m_stateTime;

	public ResolvingState(ActorTurnSM masterSM)
		: base(masterSM)
	{
	}

	public override void OnEnter()
	{
		ActorData component = m_SM.GetComponent<ActorData>();
		if (HUD_UI.Get() != null)
		{
			if (component == GameFlowData.Get().activeOwnedActorData)
			{
				InterfaceManager.Get().CancelAlert(StringUtil.TR("PostRespawnMovement", "Global"));
				UINotificationPanel notificationPanel = HUD_UI.Get().m_mainScreenPanel.m_notificationPanel;
				notificationPanel.DisplayNotification(UINotificationPanel.GamePhaseDisplay.Resolving);
			}
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateNameplateUntargeted(component, true);
		}
		HighlightUtils.Get().SetCursorType(HighlightUtils.CursorType.NoCursorType);
		ActorMovement actorMovement = component.GetActorMovement();
		actorMovement.OnGameStateChange(false);
		m_stateTime = 0f;
		bool flag = component.HasQueuedMovement();
		AbilityData component2 = m_SM.GetComponent<AbilityData>();
		bool flag2;
		if ((bool)component2)
		{
			flag2 = component2.HasQueuedAbilities();
			component2.ClearSelectedAbility();
			component2.ClearLastSelectedAbility();
		}
		else
		{
			flag2 = false;
		}
		if (flag)
		{
			return;
		}
		while (true)
		{
			if (!flag2)
			{
				while (true)
				{
					m_SM.NextState = TurnStateEnum.WAITING;
					return;
				}
			}
			return;
		}
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		if (msg == TurnMessage.TURN_START)
		{
			ActorData component = m_SM.GetComponent<ActorData>();
			object str;
			if (component != null)
			{
				str = component.DebugNameString();
			}
			else
			{
				str = "NULL actor";
			}
			Log.Error((string)str + " handling TURN_START message in Resolving state");
			m_SM.SetupForNewTurn();
			m_SM.NextState = TurnStateEnum.DECIDING;
			return;
		}
		if (msg == TurnMessage.MOVEMENT_RESOLVED)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_SM.NextState = TurnStateEnum.WAITING;
					return;
				}
			}
		}
		if (msg == TurnMessage.CLIENTS_RESOLVED_ABILITIES)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_SM.NextState = TurnStateEnum.WAITING;
					return;
				}
			}
		}
		if (msg != TurnMessage.RESPAWN)
		{
			return;
		}
		while (true)
		{
			m_SM.NextState = TurnStateEnum.RESPAWNING;
			return;
		}
	}

	public override void OnExit()
	{
		if ((bool)m_SM.GetComponent<AbilityData>())
		{
			m_SM.GetComponent<AbilityData>().ClearSelectedAbility();
		}
	}

	public override void Update()
	{
		ActorData component = m_SM.GetComponent<ActorData>();
		if (component != null)
		{
			if (component.IsDead())
			{
				m_SM.NextState = TurnStateEnum.WAITING;
			}
		}
		if (m_stateTime >= GameFlowData.Get().m_resolveTimeoutLimit * 0.9f)
		{
			if (!GameplayUtils.IsMinion(m_SM))
			{
				object[] obj = new object[3]
				{
					(!NetworkServer.active) ? "Client" : "Server",
					m_SM.GetComponent<ActorData>().DisplayName,
					null
				};
				object obj2;
				if (GameFlowData.Get().activeOwnedActorData == null)
				{
					obj2 = "NULL";
				}
				else
				{
					obj2 = GameFlowData.Get().activeOwnedActorData.DisplayName;
				}
				obj[2] = obj2;
				Log.Error("{0} timed out {1} of resolving state (owned actor = {2})", obj);
				m_SM.NextState = TurnStateEnum.WAITING;
			}
		}
		if (!(GameFlowData.Get() == null))
		{
			if (GameFlowData.Get().IsResolutionPaused())
			{
				return;
			}
		}
		m_stateTime += GameTime.deltaTime;
	}
}
