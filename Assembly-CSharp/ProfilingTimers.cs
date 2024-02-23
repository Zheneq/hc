using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;

public class ProfilingTimers
{
	private static ProfilingTimers s_instance;

	private readonly Dictionary<MethodBase, ProfilingTimer> m_methodTimers;

	private readonly Dictionary<string, ProfilingTimer> m_messageTimers;

	private readonly Dictionary<string, ProfilingTimer> m_databasesTimers;

	private readonly Dictionary<string, UnknownMethod> m_unknownMethods;

	private Stopwatch m_stopwatch;

	private int m_lastId;

	public ProfilingTimer[] MethodTimers
	{
		get
		{
			lock (m_methodTimers)
			{
				return m_methodTimers.Values.ToArray();
			}
		}
	}

	public ProfilingTimer[] MessageTimers
	{
		get
		{
			lock (m_messageTimers)
			{
				return m_messageTimers.Values.ToArray();
			}
		}
	}

	public ProfilingTimer[] DatabaseTimers
	{
		get
		{
			lock (m_databasesTimers)
			{
				return m_databasesTimers.Values.ToArray();
			}
		}
	}

	private MethodBase DefaultMethod
	{
		get;
		set;
	}

	public long RecentElapsedTicks
	{
		get;
		private set;
	}

	public long Frequency
	{
		get { return Stopwatch.Frequency; }
	}

	public ProfilingTimers()
	{
		m_methodTimers = new Dictionary<MethodBase, ProfilingTimer>();
		m_messageTimers = new Dictionary<string, ProfilingTimer>();
		m_databasesTimers = new Dictionary<string, ProfilingTimer>();
		m_unknownMethods = new Dictionary<string, UnknownMethod>();
		m_stopwatch = new Stopwatch();
		m_stopwatch.Start();
		DefaultMethod = GetType().GetMethod("UnknownMethod");
		m_lastId = 0;
	}

	public static ProfilingTimers Get()
	{
		if (s_instance == null)
		{
			s_instance = new ProfilingTimers();
		}
		return s_instance;
	}

	~ProfilingTimers()
	{
		s_instance = null;
	}

	public void OnMethodExecuted(string methodName, long ticks)
	{
		lock (m_methodTimers)
		{
			UnknownMethod unknownMethod = m_unknownMethods.TryGetValue(methodName);
			if (unknownMethod == null)
			{
				unknownMethod = new UnknownMethod();
				m_unknownMethods.Add(methodName, unknownMethod);
			}
			ProfilingTimer profilingTimer = m_methodTimers.TryGetValue(unknownMethod);
			if (profilingTimer == null)
			{
				profilingTimer = new ProfilingTimer(methodName, m_lastId++);
				m_methodTimers.Add(unknownMethod, profilingTimer);
			}
			float num = (float)ticks / (float)Frequency;
			if (DateTime.UtcNow - profilingTimer.LastWarning > TimeSpan.FromMinutes(1.0))
			{
				if (num >= 1f)
				{
					Log.Warning("Method {0} ran for {1} seconds", profilingTimer.Name, num);
					profilingTimer.LastWarning = DateTime.UtcNow;
				}
			}
			profilingTimer.Increment(ticks, 0L);
		}
	}

	public void OnMethodExecuted(MethodBase methodInfo, long ticks)
	{
		if (methodInfo == null)
		{
			methodInfo = DefaultMethod;
		}
		lock (m_methodTimers)
		{
			ProfilingTimer profilingTimer = m_methodTimers.TryGetValue(methodInfo);
			if (profilingTimer == null)
			{
				profilingTimer = new ProfilingTimer(GetMethodName(methodInfo), m_lastId++);
				m_methodTimers.Add(methodInfo, profilingTimer);
			}
			float num = (float)ticks / (float)Frequency;
			if (DateTime.UtcNow - profilingTimer.LastWarning > TimeSpan.FromMinutes(1.0))
			{
				if (num >= 1f)
				{
					Log.Warning("Method {0} ran for {1} seconds", profilingTimer.Name, num);
					profilingTimer.LastWarning = DateTime.UtcNow;
				}
			}
			profilingTimer.Increment(ticks, 0L);
		}
	}

	public void OnMessageDeserialized(WebSocketMessage message)
	{
		if (message == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		string name = message.GetType().Name;
		lock (m_messageTimers)
		{
			ProfilingTimer profilingTimer = m_messageTimers.TryGetValue(name);
			if (profilingTimer == null)
			{
				profilingTimer = new ProfilingTimer(name, m_lastId++);
				m_messageTimers.Add(name, profilingTimer);
			}
			profilingTimer.Increment(message.DeserializationTicks, message.SerializedLength);
		}
	}

	public void OnDatabaseQueryExecuted(string queryType, long executionTicks, long queryResultSize, bool skipped)
	{
		lock (m_databasesTimers)
		{
			ProfilingTimer profilingTimer = m_databasesTimers.TryGetValue(queryType);
			if (profilingTimer == null)
			{
				profilingTimer = new ProfilingTimer(queryType, m_lastId++);
				m_databasesTimers.Add(queryType, profilingTimer);
			}
			profilingTimer.Increment(executionTicks, queryResultSize, skipped);
		}
	}

	public void Update()
	{
		lock (m_methodTimers)
		{
			using (Dictionary<MethodBase, ProfilingTimer>.ValueCollection.Enumerator enumerator = m_methodTimers.Values.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					ProfilingTimer current = enumerator.Current;
					current.Update();
				}
				while (true)
				{
					switch (6)
					{
					case 0:
						break;
					default:
						goto end_IL_0020;
					}
				}
				end_IL_0020:;
			}
			using (Dictionary<string, ProfilingTimer>.ValueCollection.Enumerator enumerator2 = m_messageTimers.Values.GetEnumerator())
			{
				while (enumerator2.MoveNext())
				{
					ProfilingTimer current2 = enumerator2.Current;
					current2.Update();
				}
			}
			using (Dictionary<string, ProfilingTimer>.ValueCollection.Enumerator enumerator3 = m_databasesTimers.Values.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					ProfilingTimer current3 = enumerator3.Current;
					current3.Update();
				}
			}
			RecentElapsedTicks = m_stopwatch.ElapsedTicks;
			m_stopwatch.Reset();
			m_stopwatch.Start();
		}
	}

	public static string GetMethodName(MethodBase method)
	{
		string fullName = method.ReflectedType.FullName;
		int num = fullName.IndexOf("+<");
		string arg;
		string arg2;
		if (num >= 0)
		{
			arg = fullName.Substring(0, num);
			int num2 = num + 2;
			int num3 = fullName.IndexOf(">", num2);
			if (num3 != num2)
			{
				arg2 = ((num3 < 0) ? fullName.Substring(num2) : fullName.Substring(num2, num3 - num2));
			}
			else
			{
				ParameterInfo[] parameters = method.GetParameters();
				
				string arg3 = string.Join(",", parameters.Select(((ParameterInfo p) => p.ParameterType.Name)).ToArray());
				arg2 = new StringBuilder().Append("UnknownMethod(").Append(arg3).Append(")").ToString();
			}
		}
		else
		{
			arg = method.ReflectedType.Name;
			arg2 = method.Name;
		}
		return new StringBuilder().Append(arg).Append(".").Append(arg2).ToString();
	}

	public void UnknownMethod()
	{
	}
}
