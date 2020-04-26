using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class WebSocketMessagePublisher : IMessagePublisher
{
	private class SubscriberToken
	{
		public Type Type
		{
			get;
			set;
		}

		public WebSocket Subscriber
		{
			get;
			set;
		}
	}

	private object RwLock = new object();

	private readonly List<SubscriberToken> SubscriberTokens = new List<SubscriberToken>();

	public static Type GetType(string typeName)
	{
		Type type = Type.GetType(typeName);
		if (type == null)
		{
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			foreach (Assembly assembly in assemblies)
			{
				type = assembly.GetType(typeName);
				if (type != null)
				{
					break;
				}
			}
		}
		return type;
	}

	public void Subscribe(string typeName, WebSocket subscriber)
	{
		Type type = GetType(typeName);
		if (type == null)
		{
			return;
		}
		while (true)
		{
			Subscribe(type, subscriber);
			return;
		}
	}

	public void Subscribe<TMessageType>(WebSocket subscriber)
	{
		Subscribe(typeof(TMessageType), subscriber);
	}

	public void Subscribe(Type messageType, WebSocket subscriber)
	{
		lock (RwLock)
		{
			if (!SubscriberTokens.Any(delegate(SubscriberToken s)
			{
				int result;
				if (s.Type == messageType)
				{
					result = (s.Subscriber.Equals(subscriber) ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				return (byte)result != 0;
			}))
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						SubscriberToken subscriberToken = new SubscriberToken();
						subscriberToken.Type = messageType;
						subscriberToken.Subscriber = subscriber;
						SubscriberToken item = subscriberToken;
						SubscriberTokens.Add(item);
						return;
					}
					}
				}
			}
		}
	}

	public void Unsubscribe(WebSocket subscriber)
	{
		lock (RwLock)
		{
			List<SubscriberToken> list = SubscriberTokens.Where((SubscriberToken s) => s.Subscriber.Equals(subscriber)).ToList();
			list.ForEach(delegate(SubscriberToken s)
			{
				SubscriberTokens.Remove(s);
			});
		}
	}

	public void Unsubscribe<TMessageType>(WebSocket subscriber)
	{
		Unsubscribe(typeof(TMessageType), subscriber);
	}

	public void Unsubscribe(string typeName, WebSocket subscriber)
	{
		Type type = GetType(typeName);
		if (type == null)
		{
			return;
		}
		while (true)
		{
			Unsubscribe(type, subscriber);
			return;
		}
	}

	public void Unsubscribe(Type type, WebSocket subscriber)
	{
		lock (RwLock)
		{
			List<SubscriberToken> list = SubscriberTokens.Where(delegate(SubscriberToken s)
			{
				int result;
				if (s.Subscriber.Equals(subscriber))
				{
					result = ((s.Type == type) ? 1 : 0);
				}
				else
				{
					result = 0;
				}
				return (byte)result != 0;
			}).ToList();
			list.ForEach(delegate(SubscriberToken s)
			{
				SubscriberTokens.Remove(s);
			});
		}
	}

	public void Publish<TMessageType>(TMessageType data)
	{
		List<WebSocket> list = new List<WebSocket>(SubscriberTokens.Count);
		lock (RwLock)
		{
			using (List<SubscriberToken>.Enumerator enumerator = SubscriberTokens.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					SubscriberToken current = enumerator.Current;
					if (current.Type == typeof(TMessageType))
					{
						list.Add(current.Subscriber);
					}
				}
			}
		}
		WebSocket.Broadcast(data as WebSocketMessage, list);
	}
}
