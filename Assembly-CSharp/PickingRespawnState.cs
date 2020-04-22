using UnityEngine;
using UnityEngine.Networking;

public class PickingRespawnState : TurnState
{
	public bool focusedCameraYet;

	public PickingRespawnState(ActorTurnSM masterSM)
		: base(masterSM)
	{
	}

	public override void OnEnter()
	{
		ActorData component = m_SM.GetComponent<ActorData>();
		if (!(component == GameFlowData.Get().activeOwnedActorData))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			GameObject gameObject = null;
			if (component.RespawnPickedPositionSquare != null)
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
				gameObject = component.RespawnPickedPositionSquare.gameObject;
			}
			else if (!component.respawnSquares.IsNullOrEmpty())
			{
				gameObject = component.respawnSquares[0].gameObject;
			}
			focusedCameraYet = false;
			if (gameObject != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					CameraManager.Get().SetTargetObject(gameObject, CameraManager.CameraTargetReason.MustSelectRespawnLoc);
					focusedCameraYet = true;
					InterfaceManager.Get().DisplayAlert(StringUtil.TR("PickRespawnLocation", "Global"), BoardSquare.s_respawnOptionHighlightColor, 60f, true);
					return;
				}
			}
			return;
		}
	}

	public override void OnExit()
	{
		if (m_SM.GetComponent<ActorData>() == GameFlowData.Get().activeOwnedActorData)
		{
			InterfaceManager.Get().CancelAlert(StringUtil.TR("PickRespawnLocation", "Global"));
		}
	}

	public override void OnMsg(TurnMessage msg, int extraData)
	{
		ActorData component = m_SM.GetComponent<ActorData>();
		switch (msg)
		{
		case TurnMessage.MOVEMENT_RESOLVED:
			Log.Error("Received a 'Movement Resolved' message in the PickingRespawn state, which is unexpected.");
			m_SM.NextState = TurnStateEnum.WAITING;
			return;
		case TurnMessage.CLIENTS_RESOLVED_ABILITIES:
			if (NetworkServer.active)
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
				Log.Error("Received a 'CLIENTS_RESOLVED_ABILITIES' message in the PickingRespawn state, which is unexpected.");
			}
			else
			{
				Log.Warning("Received a 'CLIENTS_RESOLVED_ABILITIES' message in the PickingRespawn state, which is unexpected.");
			}
			m_SM.NextState = TurnStateEnum.WAITING;
			return;
		case TurnMessage.BEGIN_RESOLVE:
		case TurnMessage.DISCONNECTED:
			m_SM.NextState = TurnStateEnum.WAITING;
			if (!(component.RespawnPickedPositionSquare == null) || component.respawnSquares.IsNullOrEmpty())
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				component.RespawnPickedPositionSquare = component.respawnSquares[0];
				return;
			}
		case TurnMessage.DONE_BUTTON_CLICKED:
			m_SM.NextState = TurnStateEnum.CONFIRMED;
			if (component.RespawnPickedPositionSquare == null && !component.respawnSquares.IsNullOrEmpty())
			{
				component.RespawnPickedPositionSquare = component.respawnSquares[0];
			}
			return;
		case TurnMessage.CANCEL_BUTTON_CLICKED:
			component.RespawnPickedPositionSquare = null;
			if (m_SM.GetComponent<ActorData>() == GameFlowData.Get().activeOwnedActorData)
			{
				InterfaceManager.Get().DisplayAlert(StringUtil.TR("PickRespawnLocation", "Global"), BoardSquare.s_respawnOptionHighlightColor, 60f, true);
			}
			return;
		case TurnMessage.PICK_RESPAWN:
			if (!(m_SM.GetComponent<ActorData>() == GameFlowData.Get().activeOwnedActorData))
			{
				return;
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				InterfaceManager.Get().CancelAlert(StringUtil.TR("PickRespawnLocation", "Global"));
				return;
			}
		case TurnMessage.RESPAWN:
			m_SM.NextState = TurnStateEnum.DECIDING;
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
			return;
		}
	}

	public override void Update()
	{
		ActorData component = m_SM.GetComponent<ActorData>();
		if (!focusedCameraYet)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (component == GameFlowData.Get().activeOwnedActorData)
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
				GameObject gameObject = null;
				if (component.RespawnPickedPositionSquare != null)
				{
					gameObject = component.RespawnPickedPositionSquare.gameObject;
				}
				else if (!component.respawnSquares.IsNullOrEmpty())
				{
					gameObject = component.respawnSquares[0].gameObject;
				}
				if (gameObject != null)
				{
					CameraManager.Get().SetTargetObject(gameObject, CameraManager.CameraTargetReason.MustSelectRespawnLoc);
					focusedCameraYet = true;
					InterfaceManager.Get().DisplayAlert(StringUtil.TR("PickRespawnLocation", "Global"), BoardSquare.s_respawnOptionHighlightColor, 60f, true);
				}
			}
		}
		if (component.NextRespawnTurn <= GameFlowData.Get().CurrentTurn)
		{
			m_SM.NextState = TurnStateEnum.DECIDING;
		}
		m_SM.UpdateEndTurnKey();
	}
}
