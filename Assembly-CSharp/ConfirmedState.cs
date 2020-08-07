using UnityEngine;
using UnityEngine.Networking;

public class ConfirmedState : TurnState
{
	public ConfirmedState(ActorTurnSM masterSM)
		: base(masterSM)
	{
	}

	public override void OnEnter()
	{
		if (HUD_UI.Get() != null)
		{
			if (m_SM.GetComponent<ActorData>() == GameFlowData.Get().activeOwnedActorData)
			{
				HUD_UI.Get().m_mainScreenPanel.m_notificationPanel.DisplayNotification(UINotificationPanel.GamePhaseDisplay.LockedIn);
			}
		}
		if ((bool)m_SM)
		{
			m_SM.OnActionsConfirmed();
		}
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		if (msg == TurnMessage.TURN_START)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
				{
					ActorData component = m_SM.GetComponent<ActorData>();
					if (component != null && !component.IsDead())
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								Log.Error(component.GetDebugName() + " handling TURN_START message in Confirmed state");
								m_SM.SetupForNewTurn();
								m_SM.NextState = TurnStateEnum.DECIDING;
								return;
							}
						}
					}
					return;
				}
				}
			}
		}
		if (msg == TurnMessage.BEGIN_RESOLVE)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					m_SM.NextState = TurnStateEnum.RESOLVING;
					return;
				}
			}
		}
		if (msg == TurnMessage.CANCEL_BUTTON_CLICKED)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					if ((bool)m_SM)
					{
						m_SM.OnActionsUnconfirmed();
					}
					ActorData component2 = m_SM.GetComponent<ActorData>();
					if (component2.RespawnPickedPositionSquare != null)
					{
						if (!component2.IsPickingRespawnSquare())
						{
							while (true)
							{
								switch (6)
								{
								case 0:
									break;
								default:
									component2.RespawnPickedPositionSquare = null;
									m_SM.NextState = TurnStateEnum.PICKING_RESPAWN;
									return;
								}
							}
						}
					}
					m_SM.NextState = TurnStateEnum.DECIDING;
					return;
				}
				}
			}
		}
		if (msg == TurnMessage.MOVEMENT_ACCEPTED)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					ActorData component3 = m_SM.GetComponent<ActorData>();
					if (component3.GetTimeBank().AllowUnconfirm())
					{
						m_SM.OnActionsUnconfirmed();
						m_SM.NextState = TurnStateEnum.DECIDING;
					}
					return;
				}
				}
			}
		}
		if (msg == TurnMessage.PICK_RESPAWN)
		{
			m_SM.NextState = TurnStateEnum.PICKING_RESPAWN;
			return;
		}
		if (msg == TurnMessage.RESPAWN)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					m_SM.NextState = TurnStateEnum.RESPAWNING;
					return;
				}
			}
		}
		if (msg == TurnMessage.MOVEMENT_RESOLVED)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Log.Error(m_SM.GetComponent<ActorData>().DisplayName + "Received a 'Movement Resolved' message in the Confirmed state, which is unexpected.");
					m_SM.NextState = TurnStateEnum.WAITING;
					return;
				}
			}
		}
		if (msg != TurnMessage.CLIENTS_RESOLVED_ABILITIES)
		{
			return;
		}
		while (true)
		{
			if (NetworkServer.active)
			{
				Log.Error(m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the Confirmed state, which is unexpected.");
			}
			else
			{
				Log.Warning(m_SM.GetComponent<ActorData>().DisplayName + "Received a 'CLIENTS_RESOLVED_ABILITIES' message in the Confirmed state, which is unexpected.");
			}
			m_SM.NextState = TurnStateEnum.WAITING;
			return;
		}
	}

	public override void OnExit()
	{
	}

	public override void Update()
	{
		ActorData component = m_SM.GetComponent<ActorData>();
		if (!(GameFlowData.Get().activeOwnedActorData == component) || GameFlowData.Get().gameState == GameState.EndingGame)
		{
			return;
		}
		if (Input.GetMouseButtonUp(1))
		{
			if (InterfaceManager.Get().ShouldHandleMouseClick() && !m_SM.HandledMouseInput)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						if (component.GetTimeBank().AllowUnconfirm())
						{
							m_SM.OnActionsUnconfirmed();
							m_SM.HandledMouseInput = true;
							m_SM.SelectMovementSquare();
						}
						return;
					}
				}
			}
		}
		if (!InputManager.Get().IsKeyBindingNewlyHeld(KeyPreference.LockIn))
		{
			return;
		}
		while (true)
		{
			if (!m_SM.HandledSpaceInput)
			{
				while (true)
				{
					m_SM.HandledSpaceInput = true;
					m_SM.RequestCancel(true);
					return;
				}
			}
			return;
		}
	}
}
