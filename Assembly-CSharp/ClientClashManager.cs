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
	public string m_onClashAudioEvent = string.Empty;

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
		if (curPath.m_moverClashesHere)
		{
			if (curPath.next != null)
			{
				if (curPath.prev != null)
				{
					goto IL_0050;
				}
			}
			OnMidMovementClash(curPath.square);
			return;
		}
		goto IL_0050;
		IL_0050:
		if (curPath.prev != null)
		{
			if (curPath.prev.m_moverClashesHere)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						OnMidMovementClash(curPath.prev.square);
						return;
					}
				}
			}
		}
		if (curPath.next != null)
		{
			return;
		}
		while (true)
		{
			List<ClashAtEndOfEvade> list = new List<ClashAtEndOfEvade>();
			using (List<ClashAtEndOfEvade>.Enumerator enumerator = m_postEvadeClashes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ClashAtEndOfEvade current = enumerator.Current;
					if (current.m_participants.Contains(mover))
					{
						list.Add(current);
						OnMidMovementClash(current.m_clashSquare);
					}
				}
			}
			using (List<ClashAtEndOfEvade>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ClashAtEndOfEvade current2 = enumerator2.Current;
					m_postEvadeClashes.Remove(current2);
				}
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
		}
	}

	public void OnMidMovementClash(BoardSquare clashSquare)
	{
		bool flag = true;
		using (List<ActiveClashVfx>.Enumerator enumerator = m_activeClashVfxList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActiveClashVfx current = enumerator.Current;
				if (current.m_square == clashSquare)
				{
					if (Time.time < current.m_timeCreated + m_timeTillNewClashOnSameSquare)
					{
						flag = false;
					}
				}
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
		if (m_currentMessageHandlersState != 0)
		{
			return;
		}
		while (true)
		{
			if (!(ClientGameManager.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (ClientGameManager.Get().Client != null)
				{
					ClientGameManager.Get().Client.RegisterHandler(72, MsgClashesAtEndOfMovement);
					m_currentMessageHandlersState = MessageHandlersState.Registered;
				}
				return;
			}
		}
	}

	public void UnregisterHandlers()
	{
		if (m_currentMessageHandlersState != MessageHandlersState.Registered)
		{
			return;
		}
		while (true)
		{
			if (!(ClientGameManager.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (ClientGameManager.Get().Client != null)
				{
					while (true)
					{
						ClientGameManager.Get().Client.UnregisterHandler(72);
						m_currentMessageHandlersState = MessageHandlersState.Unregistered;
						return;
					}
				}
				return;
			}
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
		sbyte b = reader.ReadSByte();
		for (int i = 0; i < b; i++)
		{
			sbyte b2 = reader.ReadSByte();
			sbyte b3 = reader.ReadSByte();
			BoardSquare boardSquare = Board.Get().GetBoardSquare(b2, b3);
			List<ActorData> list = new List<ActorData>();
			sbyte b4 = reader.ReadSByte();
			for (int j = 0; j < b4; j++)
			{
				sbyte b5 = reader.ReadSByte();
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex(b5);
				if (actorData != null)
				{
					list.Add(actorData);
				}
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					goto end_IL_00ac;
				}
				continue;
				end_IL_00ac:
				break;
			}
			ClashAtEndOfEvade item = new ClashAtEndOfEvade(list, boardSquare);
			m_postEvadeClashes.Add(item);
		}
		while (true)
		{
			switch (1)
			{
			default:
				return;
			case 0:
				break;
			}
		}
	}

	private void Update()
	{
		List<ActiveClashVfx> list = null;
		using (List<ActiveClashVfx>.Enumerator enumerator = m_activeClashVfxList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActiveClashVfx current = enumerator.Current;
				if (Time.time > current.m_timeCreated + m_timeTillClashExpires)
				{
					if (list == null)
					{
						list = new List<ActiveClashVfx>();
					}
					list.Add(current);
				}
			}
		}
		if (list == null)
		{
			return;
		}
		while (true)
		{
			using (List<ActiveClashVfx>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActiveClashVfx current2 = enumerator2.Current;
					current2.OnEnd();
					m_activeClashVfxList.Remove(current2);
				}
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
		}
	}
}
