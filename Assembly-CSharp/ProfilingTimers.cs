using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

public class ProfilingTimers
{
	private static ProfilingTimers s_instance;

	private readonly Dictionary<MethodBase, ProfilingTimer> m_methodTimers;

	private readonly Dictionary<string, ProfilingTimer> m_messageTimers;

	private readonly Dictionary<string, ProfilingTimer> m_databasesTimers;

	private readonly Dictionary<string, UnknownMethod> m_unknownMethods;

	private Stopwatch m_stopwatch;

	private int m_lastId;

	public ProfilingTimers()
	{
		this.m_methodTimers = new Dictionary<MethodBase, ProfilingTimer>();
		this.m_messageTimers = new Dictionary<string, ProfilingTimer>();
		this.m_databasesTimers = new Dictionary<string, ProfilingTimer>();
		this.m_unknownMethods = new Dictionary<string, UnknownMethod>();
		this.m_stopwatch = new Stopwatch();
		this.m_stopwatch.Start();
		this.DefaultMethod = base.GetType().GetMethod("UnknownMethod");
		this.m_lastId = 0;
	}

	public static ProfilingTimers Get()
	{
		if (ProfilingTimers.s_instance == null)
		{
			ProfilingTimers.s_instance = new ProfilingTimers();
		}
		return ProfilingTimers.s_instance;
	}

	~ProfilingTimers()
	{
		ProfilingTimers.s_instance = null;
	}

	public ProfilingTimer[] MethodTimers
	{
		get
		{
			object methodTimers = this.m_methodTimers;
			ProfilingTimer[] result;
			lock (methodTimers)
			{
				result = this.m_methodTimers.Values.ToArray<ProfilingTimer>();
			}
			return result;
		}
	}

	public ProfilingTimer[] MessageTimers
	{
		get
		{
			object messageTimers = this.m_messageTimers;
			ProfilingTimer[] result;
			lock (messageTimers)
			{
				result = this.m_messageTimers.Values.ToArray<ProfilingTimer>();
			}
			return result;
		}
	}

	public ProfilingTimer[] DatabaseTimers
	{
		get
		{
			object databasesTimers = this.m_databasesTimers;
			ProfilingTimer[] result;
			lock (databasesTimers)
			{
				result = this.m_databasesTimers.Values.ToArray<ProfilingTimer>();
			}
			return result;
		}
	}

	private MethodBase DefaultMethod { get; set; }

	public long RecentElapsedTicks { get; private set; }

	public long Frequency
	{
		get
		{
			return Stopwatch.Frequency;
		}
	}

	public void OnMethodExecuted(string methodName, long ticks)
	{
		object methodTimers = this.m_methodTimers;
		lock (methodTimers)
		{
			UnknownMethod unknownMethod = this.m_unknownMethods.TryGetValue(methodName);
			if (unknownMethod == null)
			{
				unknownMethod = new UnknownMethod();
				this.m_unknownMethods.Add(methodName, unknownMethod);
			}
			ProfilingTimer profilingTimer = this.m_methodTimers.TryGetValue(unknownMethod);
			if (profilingTimer == null)
			{
				profilingTimer = new ProfilingTimer(methodName, this.m_lastId++);
				this.m_methodTimers.Add(unknownMethod, profilingTimer);
			}
			float num = (float)ticks / (float)this.Frequency;
			if (DateTime.UtcNow - profilingTimer.LastWarning > TimeSpan.FromMinutes(1.0))
			{
				if (num >= 1f)
				{
					Log.Warning("Method {0} ran for {1} seconds", new object[]
					{
						profilingTimer.Name,
						num
					});
					profilingTimer.LastWarning = DateTime.UtcNow;
				}
			}
			profilingTimer.Increment(ticks, 0L, false);
		}
	}

	public void OnMethodExecuted(MethodBase methodInfo, long ticks)
	{
		if (methodInfo == null)
		{
			methodInfo = this.DefaultMethod;
		}
		object methodTimers = this.m_methodTimers;
		lock (methodTimers)
		{
			ProfilingTimer profilingTimer = this.m_methodTimers.TryGetValue(methodInfo);
			if (profilingTimer == null)
			{
				profilingTimer = new ProfilingTimer(ProfilingTimers.GetMethodName(methodInfo), this.m_lastId++);
				this.m_methodTimers.Add(methodInfo, profilingTimer);
			}
			float num = (float)ticks / (float)this.Frequency;
			if (DateTime.UtcNow - profilingTimer.LastWarning > TimeSpan.FromMinutes(1.0))
			{
				if (num >= 1f)
				{
					Log.Warning("Method {0} ran for {1} seconds", new object[]
					{
						profilingTimer.Name,
						num
					});
					profilingTimer.LastWarning = DateTime.UtcNow;
				}
			}
			profilingTimer.Increment(ticks, 0L, false);
		}
	}

	public void OnMessageDeserialized(WebSocketMessage message)
	{
		if (message == null)
		{
			return;
		}
		string name = message.GetType().Name;
		object messageTimers = this.m_messageTimers;
		lock (messageTimers)
		{
			ProfilingTimer profilingTimer = this.m_messageTimers.TryGetValue(name);
			if (profilingTimer == null)
			{
				profilingTimer = new ProfilingTimer(name, this.m_lastId++);
				this.m_messageTimers.Add(name, profilingTimer);
			}
			profilingTimer.Increment(message.DeserializationTicks, message.SerializedLength, false);
		}
	}

	public void OnDatabaseQueryExecuted(string queryType, long executionTicks, long queryResultSize, bool skipped)
	{
		object databasesTimers = this.m_databasesTimers;
		lock (databasesTimers)
		{
			ProfilingTimer profilingTimer = this.m_databasesTimers.TryGetValue(queryType);
			if (profilingTimer == null)
			{
				profilingTimer = new ProfilingTimer(queryType, this.m_lastId++);
				this.m_databasesTimers.Add(queryType, profilingTimer);
			}
			profilingTimer.Increment(executionTicks, queryResultSize, skipped);
		}
	}

	public void Update()
	{
		object methodTimers = this.m_methodTimers;
		lock (methodTimers)
		{
			using (Dictionary<MethodBase, ProfilingTimer>.ValueCollection.Enumerator enumerator = this.m_methodTimers.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ProfilingTimer profilingTimer = enumerator.Current;
					profilingTimer.Update();
				}
			}
			using (Dictionary<string, ProfilingTimer>.ValueCollection.Enumerator enumerator2 = this.m_messageTimers.Values.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ProfilingTimer profilingTimer2 = enumerator2.Current;
					profilingTimer2.Update();
				}
			}
			using (Dictionary<string, ProfilingTimer>.ValueCollection.Enumerator enumerator3 = this.m_databasesTimers.Values.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					ProfilingTimer profilingTimer3 = enumerator3.Current;
					profilingTimer3.Update();
				}
			}
			this.RecentElapsedTicks = this.m_stopwatch.ElapsedTicks;
			this.m_stopwatch.Reset();
			this.m_stopwatch.Start();
		}
	}

	public static string GetMethodName(MethodBase method)
	{
		string fullName = method.ReflectedType.FullName;
		int num = fullName.IndexOf("+<");
		string arg;
		string arg3;
		if (num >= 0)
		{
			arg = fullName.Substring(0, num);
			int num2 = num + 2;
			int num3 = fullName.IndexOf(">", num2);
			if (num3 == num2)
			{
				string separator = ",";
				IEnumerable<ParameterInfo> parameters = method.GetParameters();
				
				string arg2 = string.Join(separator, parameters.Select(((ParameterInfo p) => p.ParameterType.Name)).ToArray<string>());
				arg3 = string.Format("UnknownMethod({0})", arg2);
			}
			else if (num3 >= 0)
			{
				arg3 = fullName.Substring(num2, num3 - num2);
			}
			else
			{
				arg3 = fullName.Substring(num2);
			}
		}
		else
		{
			arg = method.ReflectedType.Name;
			arg3 = method.Name;
		}
		return string.Format("{0}.{1}", arg, arg3);
	}

	public void UnknownMethod()
	{
	}
}
