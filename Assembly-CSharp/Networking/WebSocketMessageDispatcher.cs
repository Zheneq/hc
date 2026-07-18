using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

public class WebSocketMessageDispatcher<TSession> where TSession : class
{
	private struct WebSocketEnvelope
	{
		public WebSocketMessage Message;

		public TSession Session;

		public MessageHandler Handler;
	}

	private class MessageHandler
	{
		public int ResponseId;

		public TSession Session;

		public object Target;

		public Action<TSession, WebSocketMessage> InvokerDelegate;

		public MethodInfo MethodInfo;
	}

	private class MessageTypeHandler
	{
		public Type MessageType;

		public List<MessageHandler> MessageHandlers;

		public Dictionary<int, MessageHandler> ResponseMessageHandlers;
	}

	private Dictionary<Type, MessageTypeHandler> m_messageTypeHandlers;

	private int m_nextRequestId;

	public SynchronizationContext SynchronizationContext
	{
		get;
		set;
	}

	public bool AllowNewMessageDelegates
	{
		get;
		set;
	}

	public WebSocketMessageDispatcher()
	{
		m_messageTypeHandlers = new Dictionary<Type, MessageTypeHandler>();
		m_nextRequestId = 0;
		SynchronizationContext = SynchronizationContext.Current;
		AllowNewMessageDelegates = true;
	}

	public void RegisterMessageDelegate<TMessage>(Action<TMessage> messageDelegate) where TMessage : WebSocketMessage, new()
	{
		RegisterMessageDelegateInternal(messageDelegate, 0, messageDelegate.Method);
	}

	public void RegisterMessageDelegate<TMessage>(Action<TMessage> messageDelegate, int responseId) where TMessage : WebSocketMessage, new()
	{
		RegisterMessageDelegateInternal(messageDelegate, responseId, messageDelegate.Method);
	}

	public void RegisterMessageDelegate<TMessage>(Action<TSession, TMessage> messageDelegate) where TMessage : WebSocketMessage, new()
	{
		RegisterMessageDelegateInternal((TSession)null, messageDelegate, 0, messageDelegate.Method);
	}

	public void RegisterMessageDelegate<TMessage>(Action<TSession, TMessage> messageDelegate, int responseId) where TMessage : WebSocketMessage, new()
	{
		RegisterMessageDelegateInternal((TSession)null, messageDelegate, responseId, messageDelegate.Method);
	}

	public void RegisterMessageDelegate<TMessage>(Action<TSession, TMessage> messageDelegate, TSession session) where TMessage : WebSocketMessage, new()
	{
		RegisterMessageDelegateInternal(session, messageDelegate, 0, messageDelegate.Method);
	}

	public void UnregisterMessageDelegate<TMessage>(Action<TMessage> messageDelegate) where TMessage : WebSocketMessage, new()
	{
		UnregisterMessageDelegateInternal<TMessage>(messageDelegate.Method, null, (TSession)null);
	}

	public void UnregisterMessageDelegate<TMessage>(Action<TSession, TMessage> messageDelegate) where TMessage : WebSocketMessage, new()
	{
		UnregisterMessageDelegateInternal<TMessage>(messageDelegate.Method, messageDelegate.Target, (TSession)null);
	}

	public void UnregisterMessageDelegate<TMessage>(Action<TSession, TMessage> messageDelegate, TSession session) where TMessage : WebSocketMessage, new()
	{
		UnregisterMessageDelegateInternal<TMessage>(messageDelegate.Method, messageDelegate.Target, session);
	}

	public void UnregisterMessageDelegate<TMessage>(int responseId, TSession session) where TMessage : WebSocketMessage, new()
	{
		UnregisterMessageDelegateInternal<TMessage>(responseId, session);
	}

	public void UnregisterMessageDelegate<TMessage>(int responseId) where TMessage : WebSocketMessage, new()
	{
		UnregisterMessageDelegateInternal<TMessage>(responseId, (TSession)null);
	}

	private void RegisterMessageDelegateInternal<TMessage>(Action<TMessage> messageDelegate, int responseId, MethodInfo method) where TMessage : WebSocketMessage, new()
	{
		if (messageDelegate == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					throw new ArgumentNullException("MessageDelegate");
				}
			}
		}
		Type type = messageDelegate.Target.GetType();
		if (!typeof(TSession).IsAssignableFrom(type))
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					throw new ArgumentException("This version of RegisterMessageDelegate can only be used to bind to methods on session objects");
				}
			}
		}
		Action<TSession, TMessage> messageDelegate2 = (Action<TSession, TMessage>)Delegate.CreateDelegate(typeof(Action<TSession, TMessage>), messageDelegate.Method);
		RegisterMessageDelegateInternal((TSession)null, messageDelegate2, responseId, method);
	}

	private void RegisterMessageDelegateInternal<TMessage>(TSession session, Action<TSession, TMessage> messageDelegate, int responseId, MethodInfo method) where TMessage : WebSocketMessage, new()
	{
		if (messageDelegate == null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
					throw new ArgumentNullException("MessageDelegate");
				}
			}
		}
		if (responseId == 0)
		{
			if (!AllowNewMessageDelegates)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						throw new Exception("Cannot add new message delegates");
					}
				}
			}
		}
		lock (m_messageTypeHandlers)
		{
			Action<TSession, WebSocketMessage> invokerDelegate = delegate(TSession _session, WebSocketMessage message)
			{
				messageDelegate(_session, message as TMessage);
			};
			MessageTypeHandler messageTypeHandler = SetupMessageType<TMessage>();
			MessageHandler messageHandler = new MessageHandler();
			messageHandler.InvokerDelegate = invokerDelegate;
			messageHandler.MethodInfo = method;
			messageHandler.Target = messageDelegate.Target;
			messageHandler.Session = session;
			messageHandler.ResponseId = responseId;
			MessageHandler messageHandler2 = messageHandler;
			if (responseId != 0)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						if (messageTypeHandler.ResponseMessageHandlers == null)
						{
							messageTypeHandler.ResponseMessageHandlers = new Dictionary<int, MessageHandler>();
						}
						messageTypeHandler.ResponseMessageHandlers.Add(responseId, messageHandler2);
						return;
					}
				}
			}
			if (messageTypeHandler.MessageHandlers == null)
			{
				messageTypeHandler.MessageHandlers = new List<MessageHandler>();
			}
			messageTypeHandler.MessageHandlers.Add(messageHandler2);
		}
	}

	private void UnregisterMessageDelegateInternal<TMessage>(MethodInfo method, object target, TSession session) where TMessage : WebSocketMessage, new()
	{
		lock (m_messageTypeHandlers)
		{
			MessageTypeHandler messageTypeHandler = SetupMessageType<TMessage>();
			if (messageTypeHandler.MessageHandlers != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						messageTypeHandler.MessageHandlers.RemoveAll(delegate(MessageHandler m)
						{
							int result;
							if (m.MethodInfo == method)
							{
								if (m.Target == target)
								{
									result = ((m.Session == session) ? 1 : 0);
									goto IL_004a;
								}
							}
							result = 0;
							goto IL_004a;
							IL_004a:
							return (byte)result != 0;
						});
						return;
					}
				}
			}
		}
	}

	private void UnregisterMessageDelegateInternal<TMessage>(int responseId, TSession session) where TMessage : WebSocketMessage, new()
	{
		lock (m_messageTypeHandlers)
		{
			MessageTypeHandler messageTypeHandler = SetupMessageType<TMessage>();
			if (messageTypeHandler.ResponseMessageHandlers != null)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						break;
					default:
						messageTypeHandler.ResponseMessageHandlers.Remove(responseId);
						return;
					}
				}
			}
		}
	}

	private MessageTypeHandler SetupMessageType<TMessage>() where TMessage : WebSocketMessage, new()
	{
		lock (m_messageTypeHandlers)
		{
			Type typeFromHandle = typeof(TMessage);
			if (!m_messageTypeHandlers.TryGetValue(typeFromHandle, out MessageTypeHandler value))
			{
				value = new MessageTypeHandler();
				value.MessageType = typeof(TMessage);
				m_messageTypeHandlers.Add(typeFromHandle, value);
			}
			return value;
		}
	}

	private void Invoke(WebSocketEnvelope envelope)
	{
		if (SynchronizationContext != null)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					SynchronizationContext.Post(InvokeAsync, envelope, envelope.Handler.MethodInfo);
					return;
				}
			}
		}
		InvokeAsync(envelope);
	}

	private void InvokeAsync(object _envelope)
	{
		WebSocketEnvelope webSocketEnvelope = (WebSocketEnvelope)_envelope;
		ProfilingTimers.Get().OnMessageDeserialized(webSocketEnvelope.Message);
		webSocketEnvelope.Handler.InvokerDelegate(webSocketEnvelope.Session, webSocketEnvelope.Message);
	}

	public void HandleMessage(TSession session, WebSocketMessage message)
	{
		lock (m_messageTypeHandlers)
		{
			if (!m_messageTypeHandlers.TryGetValue(message.GetType(), out MessageTypeHandler value))
			{
				Log.Warning("Received a message of type '{0}', but no registered type handler found", message.GetType());
				return;
			}
			WebSocketEnvelope webSocketEnvelope;
			if (message.ResponseId == 0)
			{
				if (!value.MessageHandlers.IsNullOrEmpty())
				{
					using (List<MessageHandler>.Enumerator enumerator = value.MessageHandlers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							MessageHandler current = enumerator.Current;
							if (current.Session != null)
							{
								if (current.Session != session)
								{
									continue;
								}
							}
							webSocketEnvelope = default(WebSocketEnvelope);
							webSocketEnvelope.Message = message;
							webSocketEnvelope.Session = session;
							webSocketEnvelope.Handler = current;
							WebSocketEnvelope envelope = webSocketEnvelope;
							Invoke(envelope);
						}
						return;
					}
				}
				Log.Warning("Received a message of type '{0}', but no registered handler found", message.GetType().Name);
				return;
			}
			if (message.ResponseId != 0)
			{
				if (value.ResponseMessageHandlers != null)
				{
					if (value.ResponseMessageHandlers.TryGetValue(message.ResponseId, out MessageHandler value2))
					{
						webSocketEnvelope = default(WebSocketEnvelope);
						webSocketEnvelope.Message = message;
						webSocketEnvelope.Session = session;
						webSocketEnvelope.Handler = value2;
						WebSocketEnvelope envelope2 = webSocketEnvelope;
						Invoke(envelope2);
						value.ResponseMessageHandlers.Remove(message.ResponseId);
						return;
					}
					Log.Warning("Received a message of type '{0}', but no registered response handler found", message.GetType().Name);
					return;
				}
				return;
			}
		}
	}

	public int GetRequestId()
	{
		return ++m_nextRequestId;
	}
}
