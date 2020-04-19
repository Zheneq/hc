using System;
using UnityEngine;
using UnityEngine.Networking;

public class DecidingState : TurnState
{
	public DecidingState(ActorTurnSM masterSM) : base(masterSM)
	{
	}

	public override void OnEnter()
	{
		ActorMovement component = this.m_SM.GetComponent<ActorMovement>();
		component.OnGameStateChange(true);
		component.ResetChargeCycleFlag();
		AbilityData component2 = this.m_SM.GetComponent<AbilityData>();
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(DecidingState.OnEnter()).MethodHandle;
			}
			component2.ClearSelectedAbility();
		}
		ActorData component3 = this.m_SM.GetComponent<ActorData>();
		if (component3.\u000E())
		{
			if (SpawnPointManager.Get().m_playersSelectRespawn && component3.NextRespawnTurn > GameFlowData.Get().CurrentTurn)
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
				if (component3.RespawnPickedPositionSquare == null)
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
					this.m_SM.NextState = TurnStateEnum.PICKING_RESPAWN;
					return;
				}
			}
			this.m_SM.NextState = TurnStateEnum.CONFIRMED;
		}
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		AbilityData component = this.m_SM.GetComponent<AbilityData>();
		switch (msg)
		{
		case TurnMessage.BEGIN_RESOLVE:
			this.m_SM.NextState = TurnStateEnum.RESOLVING;
			break;
		case TurnMessage.MOVEMENT_RESOLVED:
			Log.Error(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'Movement Resolved' message in the Deciding state, which is unexpected.", new object[0]);
			break;
		case TurnMessage.CLIENTS_RESOLVED_ABILITIES:
			if (NetworkServer.active)
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
				Log.Error(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the Deciding state, which is unexpected.", new object[0]);
			}
			else
			{
				Log.Error(this.m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the Deciding state, which is unexpected.", new object[0]);
			}
			this.m_SM.NextState = TurnStateEnum.WAITING;
			break;
		case TurnMessage.SELECTED_ABILITY:
			if (component != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(DecidingState.OnMsg(TurnMessage, int)).MethodHandle;
				}
				if (component.GetSelectedAbility() != null)
				{
					this.m_SM.NextState = TurnStateEnum.TARGETING_ACTION;
				}
			}
			break;
		case TurnMessage.RESPAWN:
			this.m_SM.NextState = TurnStateEnum.RESPAWNING;
			break;
		case TurnMessage.MOVE_BUTTON_CLICKED:
			this.m_SM.NextState = TurnStateEnum.DECIDING_MOVEMENT;
			break;
		case TurnMessage.DONE_BUTTON_CLICKED:
			if (NetworkServer.active)
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
				if (SinglePlayerManager.Get() != null)
				{
					SinglePlayerManager.Get().RecalcCanEndTurn();
				}
			}
			if (SinglePlayerManager.CanEndTurn(this.m_SM.GetComponent<ActorData>()))
			{
				this.m_SM.NextState = TurnStateEnum.CONFIRMED;
				if (SinglePlayerManager.Get())
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
					SinglePlayerManager.Get().OnActorLockInEntered(this.m_SM.GetComponent<ActorData>());
				}
			}
			break;
		case TurnMessage.DISCONNECTED:
			this.m_SM.NextState = TurnStateEnum.CONFIRMED;
			break;
		}
	}

	public override void Update()
	{
		ActorData component = this.m_SM.GetComponent<ActorData>();
		if (GameFlowData.Get().activeOwnedActorData == component)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(DecidingState.Update()).MethodHandle;
			}
			if (GameFlowData.Get().gameState != GameState.EndingGame)
			{
				if (!Input.GetMouseButtonUp(1))
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
					if (!InputManager.Get().GetAcceptButtonDown())
					{
						goto IL_B7;
					}
				}
				if (InterfaceManager.Get().ShouldHandleMouseClick())
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
					if (!this.m_SM.HandledMouseInput)
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
						this.m_SM.HandledMouseInput = true;
						this.m_SM.SelectMovementSquare();
					}
				}
			}
		}
		IL_B7:
		this.m_SM.UpdateEndTurnKey();
	}
}
