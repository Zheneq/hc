using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class WebSocketMessagePublisher : IMessagePublisher
{
	private object RwLock = new object();

	private readonly List<WebSocketMessagePublisher.SubscriberToken> SubscriberTokens = new List<WebSocketMessagePublisher.SubscriberToken>();

	public static Type GetType(string typeName)
	{
		Type type = Type.GetType(typeName);
		if (type == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketMessagePublisher.GetType(string)).MethodHandle;
			}
			foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
			{
				type = assembly.GetType(typeName);
				if (type != null)
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
					break;
				}
			}
		}
		return type;
	}

	public void Subscribe(string typeName, WebSocket subscriber)
	{
		Type type = WebSocketMessagePublisher.GetType(typeName);
		if (type != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketMessagePublisher.Subscribe(string, WebSocket)).MethodHandle;
			}
			this.Subscribe(type, subscriber);
		}
	}

	public void Subscribe<TMessageType>(WebSocket subscriber)
	{
		this.Subscribe(typeof(TMessageType), subscriber);
	}

	public void Subscribe(Type messageType, WebSocket subscriber)
	{
		object rwLock = this.RwLock;
		lock (rwLock)
		{
			if (!this.SubscriberTokens.Any(delegate(WebSocketMessagePublisher.SubscriberToken s)
			{
				bool result;
				if (s.Type == messageType)
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
						RuntimeMethodHandle runtimeMethodHandle2 = methodof(WebSocketMessagePublisher.<Subscribe>c__AnonStorey0.<>m__0(WebSocketMessagePublisher.SubscriberToken)).MethodHandle;
					}
					result = s.Subscriber.Equals(subscriber);
				}
				else
				{
					result = false;
				}
				return result;
			}))
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketMessagePublisher.Subscribe(Type, WebSocket)).MethodHandle;
				}
				WebSocketMessagePublisher.SubscriberToken item = new WebSocketMessagePublisher.SubscriberToken
				{
					Type = messageType,
					Subscriber = subscriber
				};
				this.SubscriberTokens.Add(item);
			}
		}
	}

	public void Unsubscribe(WebSocket subscriber)
	{
		object rwLock = this.RwLock;
		lock (rwLock)
		{
			List<WebSocketMessagePublisher.SubscriberToken> list = (from s in this.SubscriberTokens
			where s.Subscriber.Equals(subscriber)
			select s).ToList<WebSocketMessagePublisher.SubscriberToken>();
			list.ForEach(delegate(WebSocketMessagePublisher.SubscriberToken s)
			{
				this.SubscriberTokens.Remove(s);
			});
		}
	}

	public void Unsubscribe<TMessageType>(WebSocket subscriber)
	{
		this.Unsubscribe(typeof(TMessageType), subscriber);
	}

	public void Unsubscribe(string typeName, WebSocket subscriber)
	{
		Type type = WebSocketMessagePublisher.GetType(typeName);
		if (type != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketMessagePublisher.Unsubscribe(string, WebSocket)).MethodHandle;
			}
			this.Unsubscribe(type, subscriber);
		}
	}

	public void Unsubscribe(Type type, WebSocket subscriber)
	{
		object rwLock = this.RwLock;
		lock (rwLock)
		{
			List<WebSocketMessagePublisher.SubscriberToken> list = this.SubscriberTokens.Where(delegate(WebSocketMessagePublisher.SubscriberToken s)
			{
				bool result;
				if (s.Subscriber.Equals(subscriber))
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
						RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketMessagePublisher.<Unsubscribe>c__AnonStorey2.<>m__0(WebSocketMessagePublisher.SubscriberToken)).MethodHandle;
					}
					result = (s.Type == type);
				}
				else
				{
					result = false;
				}
				return result;
			}).ToList<WebSocketMessagePublisher.SubscriberToken>();
			list.ForEach(delegate(WebSocketMessagePublisher.SubscriberToken s)
			{
				this.SubscriberTokens.Remove(s);
			});
		}
	}

	public void Publish<TMessageType>(TMessageType data)
	{
		List<WebSocket> list = new List<WebSocket>(this.SubscriberTokens.Count);
		object rwLock = this.RwLock;
		lock (rwLock)
		{
			using (List<WebSocketMessagePublisher.SubscriberToken>.Enumerator enumerator = this.SubscriberTokens.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					WebSocketMessagePublisher.SubscriberToken subscriberToken = enumerator.Current;
					if (subscriberToken.Type == typeof(TMessageType))
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
							RuntimeMethodHandle runtimeMethodHandle = methodof(WebSocketMessagePublisher.Publish(TMessageType)).MethodHandle;
						}
						list.Add(subscriberToken.Subscriber);
					}
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		WebSocket.Broadcast(data as WebSocketMessage, list);
	}

	private class SubscriberToken
	{
		public Type Type { get; set; }

		public WebSocket Subscriber { get; set; }
	}
}
