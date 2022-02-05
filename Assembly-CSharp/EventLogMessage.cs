// ROGUES
// SERVER
using System;
//using System.Runtime.Serialization;

// server-only
// TODO fix serialization?
#if SERVER
public class EventLogMessage
{
	private EventLogData m_data;
	private EventLogData m_metadata;

	public EventLogMessage(string logType, string eventType)
	{
		DateTime utcNow = DateTime.UtcNow;
		LogType = logType.ToLower();
		EventType = eventType;
		Timestamp = utcNow.ToString("u");
		LogDir = string.Format("{0}/{1:d2}-{2:d2}-{3:d2}", new object[]
		{
			EventLogger.Get().EnvironmentName,
			utcNow.Year,
			utcNow.Month,
			utcNow.Day
		});
		m_metadata = new EventLogData();
		m_data = new EventLogData();
		AddMetadata("Timestamp", Timestamp);
		AddMetadata("Environment", EventLogger.Get().EnvironmentName);
		AddMetadata("EventType", EventType);
	}

	public EventLogMessage ShallowCopy()
	{
		return (EventLogMessage)base.MemberwiseClone();
	}

	public string LogType { get; private set; }

	public string EventType { get; private set; }

	public string Timestamp { get; private set; }

	public string LogDir { get; private set; }

	//[IgnoreDataMember]
	public string DataString
	{
		get
		{
			return m_data.ToString();
		}
	}

	public EventLogData Data
	{
		get
		{
			return m_data;
		}
	}

	//[IgnoreDataMember]
	public string MetadataString
	{
		get
		{
			return m_metadata.ToString();
		}
	}

	public EventLogData Metadata
	{
		get
		{
			return m_metadata;
		}
	}

	public void AddData(EventLogData values)
	{
		m_data.AddData(values);
	}

	public void AddData(string key, object value)
	{
		m_data.AddData(key, value);
	}

	public void AddMetadata(EventLogData values)
	{
		m_metadata.AddData(values);
	}

	public void AddMetadata(string key, object value)
	{
		m_metadata.AddData(key, value);
	}

	public void Write()
	{
		EventLogger.Get().Write(this);
	}
}
#endif
