using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientClashManager : MonoBehaviour
{
	[Header("-- Visual --")]
	public GameObject m_vfxPrefab;

	[Header("-- Timing --")]
	public float m_timeTillClashExpires = 1f;

	public float m_timeTillNewClashOnSameSquare = 0.2f;

	[AudioEvent(false)]
	[Header("-- Audio --")]
	public string m_onClashAudioEvent = string.Empty;

	private static ClientClashManager s_instance;

	private ClientClashManager.MessageHandlersState m_currentMessageHandlersState;

	private List<ClashAtEndOfEvade> m_postEvadeClashes;

	private List<ActiveClashVfx> m_activeClashVfxList;

	public static ClientClashManager Get()
	{
		return ClientClashManager.s_instance;
	}

	public void OnActorMoved_ClientClashManager(ActorData mover, BoardSquarePathInfo curPath)
	{
		if (curPath.m_moverClashesHere)
		{
			if (curPath.next != null)
			{
				if (curPath.prev != null)
				{
					goto IL_50;
				}
			}
			this.OnMidMovementClash(curPath.square);
			return;
		}
		IL_50:
		if (curPath.prev != null)
		{
			if (curPath.prev.m_moverClashesHere)
			{
				this.OnMidMovementClash(curPath.prev.square);
				return;
			}
		}
		if (curPath.next == null)
		{
			List<ClashAtEndOfEvade> list = new List<ClashAtEndOfEvade>();
			using (List<ClashAtEndOfEvade>.Enumerator enumerator = this.m_postEvadeClashes.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ClashAtEndOfEvade clashAtEndOfEvade = enumerator.Current;
					if (clashAtEndOfEvade.m_participants.Contains(mover))
					{
						list.Add(clashAtEndOfEvade);
						this.OnMidMovementClash(clashAtEndOfEvade.m_clashSquare);
					}
				}
			}
			using (List<ClashAtEndOfEvade>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ClashAtEndOfEvade item = enumerator2.Current;
					this.m_postEvadeClashes.Remove(item);
				}
			}
		}
	}

	public void OnMidMovementClash(BoardSquare clashSquare)
	{
		bool flag = true;
		using (List<ActiveClashVfx>.Enumerator enumerator = this.m_activeClashVfxList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActiveClashVfx activeClashVfx = enumerator.Current;
				if (activeClashVfx.m_square == clashSquare)
				{
					if (Time.time < activeClashVfx.m_timeCreated + this.m_timeTillNewClashOnSameSquare)
					{
						flag = false;
					}
				}
			}
		}
		if (flag)
		{
			float timeToExpire = Time.time + this.m_timeTillClashExpires;
			ActiveClashVfx item = new ActiveClashVfx(clashSquare, this.m_vfxPrefab, timeToExpire, this.m_onClashAudioEvent);
			this.m_activeClashVfxList.Add(item);
		}
	}

	private void Awake()
	{
		ClientClashManager.s_instance = this;
		this.m_postEvadeClashes = new List<ClashAtEndOfEvade>();
		this.m_activeClashVfxList = new List<ActiveClashVfx>();
		this.m_currentMessageHandlersState = ClientClashManager.MessageHandlersState.NotYetRegistered;
		this.RegisterHandler();
	}

	private void OnDestroy()
	{
		this.UnregisterHandlers();
		ClientClashManager.s_instance = null;
	}

	private void RegisterHandler()
	{
		if (this.m_currentMessageHandlersState == ClientClashManager.MessageHandlersState.NotYetRegistered)
		{
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().Client != null)
				{
					ClientGameManager.Get().Client.RegisterHandler(0x48, new NetworkMessageDelegate(this.MsgClashesAtEndOfMovement));
					this.m_currentMessageHandlersState = ClientClashManager.MessageHandlersState.Registered;
				}
			}
		}
	}

	public void UnregisterHandlers()
	{
		if (this.m_currentMessageHandlersState == ClientClashManager.MessageHandlersState.Registered)
		{
			if (ClientGameManager.Get() != null)
			{
				if (ClientGameManager.Get().Client != null)
				{
					ClientGameManager.Get().Client.UnregisterHandler(0x48);
					this.m_currentMessageHandlersState = ClientClashManager.MessageHandlersState.Unregistered;
				}
			}
		}
	}

	public void OnTurnStart()
	{
		this.m_postEvadeClashes.Clear();
	}

	internal void MsgClashesAtEndOfMovement(NetworkMessage netMsg)
	{
		NetworkReader reader = netMsg.reader;
		this.m_postEvadeClashes.Clear();
		sbyte b = reader.ReadSByte();
		for (int i = 0; i < (int)b; i++)
		{
			sbyte b2 = reader.ReadSByte();
			sbyte b3 = reader.ReadSByte();
			BoardSquare boardSquare = Board.Get().GetBoardSquare((int)b2, (int)b3);
			List<ActorData> list = new List<ActorData>();
			sbyte b4 = reader.ReadSByte();
			for (int j = 0; j < (int)b4; j++)
			{
				sbyte b5 = reader.ReadSByte();
				ActorData actorData = GameFlowData.Get().FindActorByActorIndex((int)b5);
				if (actorData != null)
				{
					list.Add(actorData);
				}
			}
			ClashAtEndOfEvade item = new ClashAtEndOfEvade(list, boardSquare);
			this.m_postEvadeClashes.Add(item);
		}
	}

	private void Update()
	{
		List<ActiveClashVfx> list = null;
		using (List<ActiveClashVfx>.Enumerator enumerator = this.m_activeClashVfxList.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				ActiveClashVfx activeClashVfx = enumerator.Current;
				if (Time.time > activeClashVfx.m_timeCreated + this.m_timeTillClashExpires)
				{
					if (list == null)
					{
						list = new List<ActiveClashVfx>();
					}
					list.Add(activeClashVfx);
				}
			}
		}
		if (list != null)
		{
			using (List<ActiveClashVfx>.Enumerator enumerator2 = list.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ActiveClashVfx activeClashVfx2 = enumerator2.Current;
					activeClashVfx2.OnEnd();
					this.m_activeClashVfxList.Remove(activeClashVfx2);
				}
			}
		}
	}

	private enum MessageHandlersState
	{
		NotYetRegistered,
		Registered,
		Unregistered
	}
}
