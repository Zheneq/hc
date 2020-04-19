using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

public class WebSocketMessageDispatcher<TSession> where TSession : class
{
	private Dictionary<Type, WebSocketMessageDispatcher<TSession>.MessageTypeHandler> m_messageTypeHandlers;

	private int m_nextRequestId;

	public WebSocketMessageDispatcher()
	{
		this.m_messageTypeHandlers = new Dictionary<Type, WebSocketMessageDispatcher<TSession>.MessageTypeHandler>();
		this.m_nextRequestId = 0;
		this.SynchronizationContext = SynchronizationContext.Current;
		this.AllowNewMessageDelegates = true;
	}

	public SynchronizationContext SynchronizationContext { get; set; }

	public bool AllowNewMessageDelegates { get; set; }

	public void RegisterMessageDelegate<TMessage>(Action<TMessage> messageDelegate) where TMessage : WebSocketMessage, new()
	{
		this.RegisterMessageDelegateInternal<TMessage>(messageDelegate, 0, messageDelegate.Method);
	}

	public void RegisterMessageDelegate<TMessage>(Action<TMessage> messageDelegate, int responseId) where TMessage : WebSocketMessage, new()
	{
		this.RegisterMessageDelegateInternal<TMessage>(messageDelegate, responseId, messageDelegate.Method);
	}

	public void RegisterMessageDelegate<TMessage>(Action<TSession, TMessage> messageDelegate) where TMessage : WebSocketMessage, new()
	{
		this.RegisterMessageDelegateInternal<TMessage>((TSession)((object)null), messageDelegate, 0, messageDelegate.Method);
	}

	public void RegisterMessageDelegate<TMessage>(Action<TSession, TMessage> messageDelegate, int responseId) where TMessage : WebSocketMessage, new()
	{
		this.RegisterMessageDelegateInternal<TMessage>((TSession)((object)null), messageDelegate, responseId, messageDelegate.Method);
	}

	public void RegisterMessageDelegate<TMessage>(Action<TSession, TMessage> messageDelegate, TSession session) where TMessage : WebSocketMessage, new()
	{
		this.RegisterMessageDelegateInternal<TMessage>(session, messageDelegate, 0, messageDelegate.Method);
	}

	public void UnregisterMessageDelegate<TMessage>(Action<TMessage> messageDelegate) where TMessage : WebSocketMessage, new()
	{
		this.UnregisterMessageDelegateInternal<TMessage>(messageDelegate.Method, null, (TSession)((object)null));
	}

	public void UnregisterMessageDelegate<TMessage>(Action<TSession, TMessage> messageDelegate) where TMessage : WebSocketMessage, new()
	{
		this.UnregisterMessageDelegateInternal<TMessage>(messageDelegate.Method, messageDelegate.Target, (TSession)((object)null));
	}

	public void UnregisterMessageDelegate<TMessage>(Action<TSession, TMessage> messageDelegate, TSession session) where TMessage : WebSocketMessage, new()
	{
		this.UnregisterMessageDelegateInternal<TMessage>(messageDelegate.Method, messageDelegate.Target, session);
	}

	public void UnregisterMessageDelegate<TMessage>(int responseId, TSession session) where TMessage : WebSocketMessage, new()
	{
		this.UnregisterMessageDelegateInternal<TMessage>(responseId, session);
	}

	public void UnregisterMessageDelegate<TMessage>(int responseId) where TMessage : WebSocketMessage, new()
	{
		this.UnregisterMessageDelegateInternal<TMessage>(responseId, (TSession)((object)null));
	}

	private void RegisterMessageDelegateInternal<TMessage>(Action<TMessage> messageDelegate, int responseId, MethodInfo method) where TMessage : WebSocketMessage, new()
	{
		if (messageDelegate == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketMessageDispatcher.RegisterMessageDelegateInternal(Action<TMessage>, int, MethodInfo)).MethodHandle;
			}
			throw new ArgumentNullException("MessageDelegate");
		}
		Type type = messageDelegate.Target.GetType();
		if (!typeof(TSession).IsAssignableFrom(type))
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
			throw new ArgumentException("This version of RegisterMessageDelegate can only be used to bind to methods on session objects");
		}
		Action<TSession, TMessage> messageDelegate2 = (Action<TSession, TMessage>)Delegate.CreateDelegate(typeof(Action<TSession, TMessage>), messageDelegate.Method);
		this.RegisterMessageDelegateInternal<TMessage>((TSession)((object)null), messageDelegate2, responseId, method);
	}

	private void RegisterMessageDelegateInternal<TMessage>(TSession session, Action<TSession, TMessage> messageDelegate, int responseId, MethodInfo method) where TMessage : WebSocketMessage, new()
	{
		if (messageDelegate == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketMessageDispatcher.RegisterMessageDelegateInternal(TSession, Action<TSession, TMessage>, int, MethodInfo)).MethodHandle;
			}
			throw new ArgumentNullException("MessageDelegate");
		}
		if (responseId == 0)
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
			if (!this.AllowNewMessageDelegates)
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
				throw new Exception("Cannot add new message delegates");
			}
		}
		object messageTypeHandlers = this.m_messageTypeHandlers;
		lock (messageTypeHandlers)
		{
			Action<TSession, WebSocketMessage> invokerDelegate = delegate(TSession _session, WebSocketMessage message)
			{
				messageDelegate(_session, message as TMessage);
			};
			WebSocketMessageDispatcher<TSession>.MessageTypeHandler messageTypeHandler = this.SetupMessageType<TMessage>();
			WebSocketMessageDispatcher<TSession>.MessageHandler messageHandler = new WebSocketMessageDispatcher<TSession>.MessageHandler
			{
				InvokerDelegate = invokerDelegate,
				MethodInfo = method,
				Target = messageDelegate.Target,
				Session = session,
				ResponseId = responseId
			};
			if (responseId != 0)
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
				if (messageTypeHandler.ResponseMessageHandlers == null)
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
					messageTypeHandler.ResponseMessageHandlers = new Dictionary<int, WebSocketMessageDispatcher<TSession>.MessageHandler>();
				}
				messageTypeHandler.ResponseMessageHandlers.Add(responseId, messageHandler);
			}
			else
			{
				if (messageTypeHandler.MessageHandlers == null)
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
					messageTypeHandler.MessageHandlers = new List<WebSocketMessageDispatcher<TSession>.MessageHandler>();
				}
				messageTypeHandler.MessageHandlers.Add(messageHandler);
			}
		}
	}

	private void UnregisterMessageDelegateInternal<TMessage>(MethodInfo method, object target, TSession session) where TMessage : WebSocketMessage, new()
	{
		object messageTypeHandlers = this.m_messageTypeHandlers;
		lock (messageTypeHandlers)
		{
			WebSocketMessageDispatcher<TSession>.MessageTypeHandler messageTypeHandler = this.SetupMessageType<TMessage>();
			if (messageTypeHandler.MessageHandlers != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketMessageDispatcher.UnregisterMessageDelegateInternal(MethodInfo, object, TSession)).MethodHandle;
				}
				messageTypeHandler.MessageHandlers.RemoveAll(delegate(WebSocketMessageDispatcher<TSession>.MessageHandler m)
				{
					if (m.MethodInfo == method)
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
							RuntimeMethodHandle runtimeMethodHandle2 = methodof(WebSocketMessageDispatcher.<UnregisterMessageDelegateInternal>c__AnonStorey1.<>m__0(WebSocketMessageDispatcher<TSession>.MessageHandler)).MethodHandle;
						}
						if (m.Target == target)
						{
							return m.Session == session;
						}
					}
					return false;
				});
			}
		}
	}

	private void UnregisterMessageDelegateInternal<TMessage>(int responseId, TSession session) where TMessage : WebSocketMessage, new()
	{
		object messageTypeHandlers = this.m_messageTypeHandlers;
		lock (messageTypeHandlers)
		{
			WebSocketMessageDispatcher<TSession>.MessageTypeHandler messageTypeHandler = this.SetupMessageType<TMessage>();
			if (messageTypeHandler.ResponseMessageHandlers != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketMessageDispatcher.UnregisterMessageDelegateInternal(int, TSession)).MethodHandle;
				}
				messageTypeHandler.ResponseMessageHandlers.Remove(responseId);
			}
		}
	}

	private WebSocketMessageDispatcher<TSession>.MessageTypeHandler SetupMessageType<TMessage>() where TMessage : WebSocketMessage, new()
	{
		object messageTypeHandlers = this.m_messageTypeHandlers;
		WebSocketMessageDispatcher<TSession>.MessageTypeHandler result;
		lock (messageTypeHandlers)
		{
			Type typeFromHandle = typeof(TMessage);
			WebSocketMessageDispatcher<TSession>.MessageTypeHandler messageTypeHandler;
			if (!this.m_messageTypeHandlers.TryGetValue(typeFromHandle, out messageTypeHandler))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketMessageDispatcher.SetupMessageType()).MethodHandle;
				}
				messageTypeHandler = new WebSocketMessageDispatcher<TSession>.MessageTypeHandler();
				messageTypeHandler.MessageType = typeof(TMessage);
				this.m_messageTypeHandlers.Add(typeFromHandle, messageTypeHandler);
			}
			result = messageTypeHandler;
		}
		return result;
	}

	private void Invoke(WebSocketMessageDispatcher<TSession>.WebSocketEnvelope envelope)
	{
		if (this.SynchronizationContext != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketMessageDispatcher.Invoke(WebSocketMessageDispatcher<TSession>.WebSocketEnvelope)).MethodHandle;
			}
			this.SynchronizationContext.Post(new SendOrPostCallback(this.InvokeAsync), envelope, envelope.Handler.MethodInfo);
		}
		else
		{
			this.InvokeAsync(envelope);
		}
	}

	private void InvokeAsync(object _envelope)
	{
		WebSocketMessageDispatcher<TSession>.WebSocketEnvelope webSocketEnvelope = (WebSocketMessageDispatcher<TSession>.WebSocketEnvelope)_envelope;
		ProfilingTimers.Get().OnMessageDeserialized(webSocketEnvelope.Message);
		webSocketEnvelope.Handler.InvokerDelegate(webSocketEnvelope.Session, webSocketEnvelope.Message);
	}

	public void HandleMessage(TSession session, WebSocketMessage message)
	{
		object messageTypeHandlers = this.m_messageTypeHandlers;
		lock (messageTypeHandlers)
		{
			WebSocketMessageDispatcher<TSession>.MessageTypeHandler messageTypeHandler;
			if (!this.m_messageTypeHandlers.TryGetValue(message.GetType(), out messageTypeHandler))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketMessageDispatcher.HandleMessage(TSession, WebSocketMessage)).MethodHandle;
				}
				Log.Warning("Received a message of type '{0}', but no registered type handler found", new object[]
				{
					message.GetType()
				});
			}
			else if (message.ResponseId == 0)
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
				if (!messageTypeHandler.MessageHandlers.IsNullOrEmpty<WebSocketMessageDispatcher<TSession>.MessageHandler>())
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
					using (List<WebSocketMessageDispatcher<TSession>.MessageHandler>.Enumerator enumerator = messageTypeHandler.MessageHandlers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							WebSocketMessageDispatcher<TSession>.MessageHandler messageHandler = enumerator.Current;
							if (messageHandler.Session != null)
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
								if (messageHandler.Session != session)
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
									continue;
								}
							}
							WebSocketMessageDispatcher<TSession>.WebSocketEnvelope envelope = new WebSocketMessageDispatcher<TSession>.WebSocketEnvelope
							{
								Message = message,
								Session = session,
								Handler = messageHandler
							};
							this.Invoke(envelope);
						}
						for (;;)
						{
							switch (7)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
				else
				{
					Log.Warning("Received a message of type '{0}', but no registered handler found", new object[]
					{
						message.GetType().Name
					});
				}
			}
			else if (message.ResponseId != 0)
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
				if (messageTypeHandler.ResponseMessageHandlers != null)
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
					WebSocketMessageDispatcher<TSession>.MessageHandler handler;
					if (messageTypeHandler.ResponseMessageHandlers.TryGetValue(message.ResponseId, out handler))
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
						WebSocketMessageDispatcher<TSession>.WebSocketEnvelope envelope2 = new WebSocketMessageDispatcher<TSession>.WebSocketEnvelope
						{
							Message = message,
							Session = session,
							Handler = handler
						};
						this.Invoke(envelope2);
						messageTypeHandler.ResponseMessageHandlers.Remove(message.ResponseId);
					}
					else
					{
						Log.Warning("Received a message of type '{0}', but no registered response handler found", new object[]
						{
							message.GetType().Name
						});
					}
				}
			}
		}
	}

	public int GetRequestId()
	{
		return ++this.m_nextRequestId;
	}

	private struct WebSocketEnvelope
	{
		public WebSocketMessage Message;

		public TSession Session;

		public WebSocketMessageDispatcher<TSession>.MessageHandler Handler;
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

		public List<WebSocketMessageDispatcher<TSession>.MessageHandler> MessageHandlers;

		public Dictionary<int, WebSocketMessageDispatcher<TSession>.MessageHandler> ResponseMessageHandlers;
	}
}
