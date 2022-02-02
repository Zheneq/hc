using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientClashManager : MonoBehaviour
{
	private enum MessageHandlersState
	{
		NotYetRegistered,
		Registered,
		Unregistered
	}

	[Header("-- Visual --")]
	public GameObject m_vfxPrefab;
	[Header("-- Timing --")]
	public float m_timeTillClashExpires = 1f;
	public float m_timeTillNewClashOnSameSquare = 0.2f;
	[AudioEvent(false)]
	[Header("-- Audio --")]
	public string m_onClashAudioEvent = "";

	private static ClientClashManager s_instance;

	private MessageHandlersState m_currentMessageHandlersState;
	private List<ClashAtEndOfEvade> m_postEvadeClashes;
	private List<ActiveClashVfx> m_activeClashVfxList;

	public static ClientClashManager Get()
	{
		return s_instance;
	}

	public void OnActorMoved_ClientClashManager(ActorData mover, BoardSquarePathInfo curPath)
	{
		if (curPath.m_moverClashesHere && (curPath.next == null || curPath.prev == null))
		{
			OnMidMovementClash(curPath.square);
		}
		else if (curPath.prev != null && curPath.prev.m_moverClashesHere)
		{
			OnMidMovementClash(curPath.prev.square);
		}
		else if (curPath.next == null)
		{
			List<ClashAtEndOfEvade> clashes = new List<ClashAtEndOfEvade>();
			foreach (ClashAtEndOfEvade current in m_postEvadeClashes)
			{
				if (current.m_participants.Contains(mover))
				{
					clashes.Add(current);
					OnMidMovementClash(current.m_clashSquare);
				}
			}
			foreach (ClashAtEndOfEvade clash in clashes)
			{
				m_postEvadeClashes.Remove(clash);
			}
		}
	}

	public void OnMidMovementClash(BoardSquare clashSquare)
	{
		bool flag = true;
		foreach (ActiveClashVfx vfx in m_activeClashVfxList)
		{
			if (vfx.m_square == clashSquare && Time.time < vfx.m_timeCreated + m_timeTillNewClashOnSameSquare)
			{
				flag = false;
			}
		}
		if (flag)
		{
			float timeToExpire = Time.time + m_timeTillClashExpires;
			ActiveClashVfx item = new ActiveClashVfx(clashSquare, m_vfxPrefab, timeToExpire, m_onClashAudioEvent);
			m_activeClashVfxList.Add(item);
		}
	}

	private void Awake()
	{
		s_instance = this;
		m_postEvadeClashes = new List<ClashAtEndOfEvade>();
		m_activeClashVfxList = new List<ActiveClashVfx>();
		m_currentMessageHandlersState = MessageHandlersState.NotYetRegistered;
		RegisterHandler();
	}

	private void OnDestroy()
	{
		UnregisterHandlers();
		s_instance = null;
	}

	private void RegisterHandler()
	{
		if (m_currentMessageHandlersState == MessageHandlersState.NotYetRegistered
			&& ClientGameManager.Get() != null
			&& ClientGameManager.Get().Client != null)
		{
			ClientGameManager.Get().Client.RegisterHandler((int)MyMsgType.ClashesAtEndOfMovement, MsgClashesAtEndOfMovement);
			m_currentMessageHandlersState = MessageHandlersState.Registered;
		}
	}

	public void UnregisterHandlers()
	{
		if (m_currentMessageHandlersState == MessageHandlersState.Registered
			&& ClientGameManager.Get() != null
			&& ClientGameManager.Get().Client != null)
		{
			ClientGameManager.Get().Client.UnregisterHandler((int)MyMsgType.ClashesAtEndOfMovement);
			m_currentMessageHandlersState = MessageHandlersState.Unregistered;
		}
	}

	public void OnTurnStart()
	{
		m_postEvadeClashes.Clear();
	}

	internal void MsgClashesAtEndOfMovement(NetworkMessage netMsg)
	{
		NetworkReader reader = netMsg.reader;
		m_postEvadeClashes.Clear();
		sbyte num = reader.ReadSByte();
		for (int i = 0; i < num; i++)
		{
			sbyte x = reader.ReadSByte();
			sbyte y = reader.ReadSByte();
			BoardSquare boardSquare = Board.Get().GetSquareFromIndex(x, y);
			List<ActorData> list = new List<ActorData>();
			sbyte actorNum = reader.ReadSByte();
			for (int j = 0; j < actorNum; j++)
			{
				sbyte actorIndex = reader.ReadSByte();
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(actorIndex);
				if (actorData != null)
				{
					list.Add(actorData);
				}
			}
			ClashAtEndOfEvade item = new ClashAtEndOfEvade(list, boardSquare);
			m_postEvadeClashes.Add(item);
		}
	}

	private void Update()
	{
		List<ActiveClashVfx> expired = null;
		foreach (ActiveClashVfx vfx in m_activeClashVfxList)
		{
			if (Time.time > vfx.m_timeCreated + m_timeTillClashExpires)
			{
				if (expired == null)
				{
					expired = new List<ActiveClashVfx>();
				}
				expired.Add(vfx);
			}
		}
		if (expired != null)
		{
			foreach (ActiveClashVfx vfx in expired)
			{
				vfx.OnEnd();
				m_activeClashVfxList.Remove(vfx);
			}
		}
	}
}
