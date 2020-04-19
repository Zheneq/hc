using System;
using UnityEngine;
using UnityEngine.Networking;

public class ConfirmedState : TurnState
{
	public ConfirmedState(ActorTurnSM masterSM) : base(masterSM)
	{
	}

	public override void OnEnter()
	{
		if (HUD_UI.Get() != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ConfirmedState.OnEnter()).MethodHandle;
			}
			if (this.m_SM.GetComponent<ActorData>() == GameFlowData.Get().activeOwnedActorData)
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
				HUD_UI.Get().m_mainScreenPanel.m_notificationPanel.DisplayNotification(UINotificationPanel.GamePhaseDisplay.LockedIn);
			}
		}
		if (this.m_SM)
		{
			this.m_SM.OnActionsConfirmed();
		}
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		if (msg == TurnMessage.TURN_START)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ConfirmedState.OnMsg(TurnMessage, int)).MethodHandle;
			}
			ActorData component = this.m_SM.GetComponent<ActorData>();
			if (component != null && !component.\u000E())
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
				Log.Error(component.\u0018() + " handling TURN_START message in Confirmed state", new object[0]);
				this.m_SM.SetupForNewTurn();
				this.m_SM.NextState = TurnStateEnum.DECIDING;
			}
		}
		else if (msg == TurnMessage.BEGIN_RESOLVE)
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
			this.m_SM.NextState = TurnStateEnum.RESOLVING;
		}
		else if (msg == TurnMessage.CANCEL_BUTTON_CLICKED)
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
			if (this.m_SM)
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
				this.m_SM.OnActionsUnconfirmed();
			}
			ActorData component2 = this.m_SM.GetComponent<ActorData>();
			if (component2.RespawnPickedPositionSquare != null)
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
				if (!component2.\u0015())
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
					component2.RespawnPickedPositionSquare = null;
					this.m_SM.NextState = TurnStateEnum.PICKING_RESPAWN;
					goto IL_131;
				}
			}
			this.m_SM.NextState = TurnStateEnum.DECIDING;
			IL_131:;
		}
		else if (msg == TurnMessage.MOVEMENT_ACCEPTED)
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
			ActorData component3 = this.m_SM.GetComponent<ActorData>();
			if (component3.\u000E().AllowUnconfirm())
			{
				this.m_SM.OnActionsUnconfirmed();
				this.m_SM.NextState = TurnStateEnum.DECIDING;
			}
		}
		else if (msg == TurnMessage.PICK_RESPAWN)
		{
			this.m_SM.NextState = TurnStateEnum.PICKING_RESPAWN;
		}
		else if (msg == TurnMessage.RESPAWN)
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
			this.m_SM.NextState = TurnStateEnum.RESPAWNING;
		}
		else if (msg == TurnMessage.MOVEMENT_RESOLVED)
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
			Log.Error(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'Movement Resolved' message in the Confirmed state, which is unexpected.", new object[0]);
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
			if (NetworkServer.active)
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
				Log.Error(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the Confirmed state, which is unexpected.", new object[0]);
			}
			else
			{
				Log.Warning(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the Confirmed state, which is unexpected.", new object[0]);
			}
			this.m_SM.NextState = TurnStateEnum.WAITING;
		}
	}

	public override void OnExit()
	{
	}

	public override void Update()
	{
		ActorData component = this.m_SM.GetComponent<ActorData>();
		if (GameFlowData.Get().activeOwnedActorData == component && GameFlowData.Get().gameState != GameState.EndingGame)
		{
			if (Input.GetMouseButtonUp(1))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ConfirmedState.Update()).MethodHandle;
				}
				if (InterfaceManager.Get().ShouldHandleMouseClick() && !this.m_SM.HandledMouseInput)
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
					if (component.\u000E().AllowUnconfirm())
					{
						this.m_SM.OnActionsUnconfirmed();
						this.m_SM.HandledMouseInput = true;
						this.m_SM.SelectMovementSquare();
					}
					return;
				}
			}
			if (InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.LockIn))
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
				if (!this.m_SM.HandledSpaceInput)
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
					this.m_SM.HandledSpaceInput = true;
					this.m_SM.RequestCancel(true);
				}
			}
		}
	}
}
