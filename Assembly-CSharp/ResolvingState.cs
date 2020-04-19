using System;
using UnityEngine.Networking;

public class ResolvingState : TurnState
{
	private float m_stateTime;

	public ResolvingState(ActorTurnSM masterSM) : base(masterSM)
	{
	}

	public override void OnEnter()
	{
		ActorData component = this.m_SM.GetComponent<ActorData>();
		if (HUD_UI.Get() != null)
		{
			if (component == GameFlowData.Get().activeOwnedActorData)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ResolvingState.OnEnter()).MethodHandle;
				}
				InterfaceManager.Get().CancelAlert(StringUtil.TR("PostRespawnMovement", "Global"));
				UINotificationPanel notificationPanel = HUD_UI.Get().m_mainScreenPanel.m_notificationPanel;
				notificationPanel.DisplayNotification(UINotificationPanel.GamePhaseDisplay.Resolving);
			}
			HUD_UI.Get().m_mainScreenPanel.m_nameplatePanel.UpdateNameplateUntargeted(component, true);
		}
		HighlightUtils.Get().SetCursorType(HighlightUtils.CursorType.NoCursorType);
		ActorMovement actorMovement = component.\u000E();
		actorMovement.OnGameStateChange(false);
		this.m_stateTime = 0f;
		bool flag = component.HasQueuedMovement();
		AbilityData component2 = this.m_SM.GetComponent<AbilityData>();
		bool flag2;
		if (component2)
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
			flag2 = component2.HasQueuedAbilities();
			component2.ClearSelectedAbility();
			component2.ClearLastSelectedAbility();
		}
		else
		{
			flag2 = false;
		}
		if (!flag)
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
			if (!flag2)
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
				this.m_SM.NextState = TurnStateEnum.WAITING;
			}
		}
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		if (msg == TurnMessage.TURN_START)
		{
			ActorData component = this.m_SM.GetComponent<ActorData>();
			string str;
			if (component != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ResolvingState.OnMsg(TurnMessage, int)).MethodHandle;
				}
				str = component.\u0018();
			}
			else
			{
				str = "NULL actor";
			}
			Log.Error(str + " handling TURN_START message in Resolving state", new object[0]);
			this.m_SM.SetupForNewTurn();
			this.m_SM.NextState = TurnStateEnum.DECIDING;
		}
		else if (msg == TurnMessage.MOVEMENT_RESOLVED)
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
			this.m_SM.NextState = TurnStateEnum.WAITING;
		}
		else if (msg == TurnMessage.CLIENTS_RESOLVED_ABILITIES)
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
			this.m_SM.NextState = TurnStateEnum.WAITING;
		}
		else if (msg == TurnMessage.RESPAWN)
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
			this.m_SM.NextState = TurnStateEnum.RESPAWNING;
		}
	}

	public override void OnExit()
	{
		if (this.m_SM.GetComponent<AbilityData>())
		{
			this.m_SM.GetComponent<AbilityData>().ClearSelectedAbility();
		}
	}

	public override void Update()
	{
		ActorData component = this.m_SM.GetComponent<ActorData>();
		if (component != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ResolvingState.Update()).MethodHandle;
			}
			if (component.\u000E())
			{
				this.m_SM.NextState = TurnStateEnum.WAITING;
			}
		}
		bool flag = this.m_stateTime >= GameFlowData.Get().m_resolveTimeoutLimit * 0.9f;
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
			if (!GameplayUtils.IsMinion(this.m_SM))
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
				string message = "{0} timed out {1} of resolving state (owned actor = {2})";
				object[] array = new object[3];
				array[0] = ((!NetworkServer.active) ? "Client" : "Server");
				array[1] = this.m_SM.GetComponent<ActorData>().DisplayName;
				int num = 2;
				object obj;
				if (GameFlowData.Get().activeOwnedActorData == null)
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
					obj = "NULL";
				}
				else
				{
					obj = GameFlowData.Get().activeOwnedActorData.DisplayName;
				}
				array[num] = obj;
				Log.Error(message, array);
				this.m_SM.NextState = TurnStateEnum.WAITING;
			}
		}
		if (!(GameFlowData.Get() == null))
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
			if (GameFlowData.Get().IsResolutionPaused())
			{
				return;
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
		}
		this.m_stateTime += GameTime.deltaTime;
	}
}
