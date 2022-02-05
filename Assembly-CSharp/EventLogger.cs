// ROGUES
// SERVER
using System;
using System.Collections.Generic;

// server-only
#if SERVER
public class EventLogger
{
	private static EventLogger s_instance;

	private List<EventLogger> m_childLoggers;
	protected object m_lock;

	public static EventLogger Get()
	{
		if (s_instance == null)
		{
			s_instance = new EventLogger();
		}
		return s_instance;
	}

	~EventLogger()
	{
		s_instance = null;
	}

	public string EnvironmentName { get; set; }

	public string HostName { get; set; }

	public EventLogger()
	{
		m_lock = new object();
		HostName = NetUtil.GetHostName().ToLower();
	}

	public void AddChildLogger(EventLogger logger)
	{
		object @lock = m_lock;
		lock (@lock)
		{
			if (m_childLoggers == null)
			{
				m_childLoggers = new List<EventLogger>();
			}
			m_childLoggers.Add(logger);
		}
	}

	public void RemoveChildLogger(EventLogger logger)
	{
		object @lock = m_lock;
		lock (@lock)
		{
			if (m_childLoggers != null)
			{
				m_childLoggers.Remove(logger);
			}
		}
	}

	public virtual void Write(EventLogMessage message)
	{
		try
		{
			object @lock = m_lock;
			lock (@lock)
			{
				if (m_childLoggers != null)
				{
					foreach (EventLogger eventLogger in m_childLoggers)
					{
						eventLogger.Write(message);
					}
				}
			}
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
	}

	public virtual void Shutdown(TimeSpan timeout = default(TimeSpan))
	{
		try
		{
			if (m_childLoggers != null)
			{
				EventLogger[] array = null;
				object @lock = m_lock;
				lock (@lock)
				{
					array = m_childLoggers.ToArray();
					m_childLoggers.Clear();
				}
				if (array != null)
				{
					EventLogger[] array2 = array;
					for (int i = 0; i < array2.Length; i++)
					{
						array2[i].Shutdown(timeout);
					}
				}
			}
		}
		catch (Exception exception)
		{
			Log.Exception(exception);
		}
	}
}
#endif
