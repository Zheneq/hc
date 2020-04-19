using System;
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
		NetworkManager.singleton.client.RegisterHandler(0x39, new NetworkMessageDelegate(this.HandleObserverMessage));
		this.m_observerMessages = new List<Replay.Message>();
		this.m_nextMessage = 0;
		this.m_initialGameTime = GameTime.time;
	}

	public void HandleGameStopped()
	{
		this.m_observerMessages = null;
	}

	private void Update()
	{
		if (this.m_observerMessages == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientObserverManager.Update()).MethodHandle;
			}
			return;
		}
		while (!AsyncPump.Current.BreakRequested())
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
			if (this.m_nextMessage >= this.m_observerMessages.Count || this.m_observerMessages[this.m_nextMessage].timestamp > GameTime.time - this.m_initialGameTime)
			{
				break;
			}
			Replay.Message message = this.m_observerMessages[this.m_nextMessage];
			if (ClientGameManager.Get() != null && ClientGameManager.Get().Connection != null)
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
				ClientGameManager.Get().Connection.TransportReceive(message.data, message.data.Length, 0);
			}
			this.m_nextMessage++;
		}
	}

	private void HandleObserverMessage(NetworkMessage msg)
	{
		GameManager.ObserverMessage observerMessage = msg.ReadMessage<GameManager.ObserverMessage>();
		if (observerMessage == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ClientObserverManager.HandleObserverMessage(NetworkMessage)).MethodHandle;
			}
			return;
		}
		if (this.m_observerMessages == null)
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
			this.m_observerMessages = new List<Replay.Message>();
		}
		this.m_observerMessages.Add(observerMessage.Message);
		if (GameTime.time - this.m_initialGameTime > observerMessage.Message.timestamp + 1f)
		{
			this.m_initialGameTime = GameTime.time - observerMessage.Message.timestamp;
		}
		this.Update();
	}

	public void OnGameEvent(GameEventManager.EventType eventType, GameEventManager.GameEventArgs args)
	{
		if (args == null)
		{
			return;
		}
		if (eventType == GameEventManager.EventType.ReconnectReplayStateChanged)
		{
			GameEventManager.ReconnectReplayStateChangedArgs reconnectReplayStateChangedArgs = (GameEventManager.ReconnectReplayStateChangedArgs)args;
			if (!reconnectReplayStateChangedArgs.m_newReconnectReplayState)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ClientObserverManager.OnGameEvent(GameEventManager.EventType, GameEventManager.GameEventArgs)).MethodHandle;
				}
				if (this.m_observerMessages.Count == 0)
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
					this.m_initialGameTime = GameTime.time;
				}
				else
				{
					this.m_initialGameTime = GameTime.time - this.m_observerMessages[this.m_observerMessages.Count - 1].timestamp;
				}
				this.Update();
			}
		}
	}
}
