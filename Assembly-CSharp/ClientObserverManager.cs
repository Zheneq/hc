using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ClientObserverManager : MonoBehaviour, IGameEventListener
{
	private List<Replay.Message> m_observerMessages;

	private int m_nextMessage;

	private float m_initialGameTime;

	public void Awake()
	{
		GameEventManager.Get().AddListener(this, GameEventManager.EventType.ReconnectReplayStateChanged);
	}

	public void OnDestroy()
	{
		GameEventManager.Get().RemoveListener(this, GameEventManager.EventType.ReconnectReplayStateChanged);
	}

	public void ConnectingToGameServer()
	{
		NetworkManager.singleton.client.RegisterHandler(57, HandleObserverMessage);
		m_observerMessages = new List<Replay.Message>();
		m_nextMessage = 0;
		m_initialGameTime = GameTime.time;
	}

	public void HandleGameStopped()
	{
		m_observerMessages = null;
	}

	private void Update()
	{
		if (m_observerMessages == null)
		{
			while (true)
			{
				switch (5)
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
		while (!AsyncPump.Current.BreakRequested())
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (m_nextMessage >= m_observerMessages.Count)
				{
					return;
				}
				Replay.Message message = m_observerMessages[m_nextMessage];
				if (!(message.timestamp <= GameTime.time - m_initialGameTime))
				{
					return;
				}
				Replay.Message message2 = m_observerMessages[m_nextMessage];
				if (ClientGameManager.Get() != null && ClientGameManager.Get().Connection != null)
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
					ClientGameManager.Get().Connection.TransportReceive(message2.data, message2.data.Length, 0);
				}
				m_nextMessage++;
				goto IL_008d;
			}
			IL_008d:;
		}
	}

	private void HandleObserverMessage(NetworkMessage msg)
	{
		GameManager.ObserverMessage observerMessage = msg.ReadMessage<GameManager.ObserverMessage>();
		if (observerMessage == null)
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
					return;
				}
			}
		}
		if (m_observerMessages == null)
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
			m_observerMessages = new List<Replay.Message>();
		}
		m_observerMessages.Add(observerMessage.Message);
		if (GameTime.time - m_initialGameTime > observerMessage.Message.timestamp + 1f)
		{
			m_initialGameTime = GameTime.time - observerMessage.Message.timestamp;
		}
		Update();
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (args == null || eventType != GameEventManager.EventType.ReconnectReplayStateChanged)
		{
			return;
		}
		GameEventManager.ReconnectReplayStateChangedArgs reconnectReplayStateChangedArgs = (GameEventManager.ReconnectReplayStateChangedArgs)args;
		if (reconnectReplayStateChangedArgs.m_newReconnectReplayState)
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
			if (m_observerMessages.Count == 0)
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
				m_initialGameTime = GameTime.time;
			}
			else
			{
				float time = GameTime.time;
				Replay.Message message = m_observerMessages[m_observerMessages.Count - 1];
				m_initialGameTime = time - message.timestamp;
			}
			Update();
			return;
		}
	}
}
