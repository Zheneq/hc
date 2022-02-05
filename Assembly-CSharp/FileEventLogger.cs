// ROGUES
//using System;
using System.Collections.Generic;
using System.IO;

// server-only
#if SERVER
public class FileEventLogger : EventLogger
{
	private Dictionary<string, StreamWriter> m_files;

	public FileEventLogger()
	{
		m_files = new Dictionary<string, StreamWriter>();
	}

	public string BaseName { get; set; }

	public string LogDir { get; set; }

	public void Close()
	{
		foreach (StreamWriter streamWriter in m_files.Values)
		{
			streamWriter.Close();
		}
		m_files.Clear();
	}

	public override void Write(EventLogMessage message)
	{
		lock (m_lock)
		{
			string text = Path.Combine(LogDir, message.LogDir);
			string path;
			if (!BaseName.IsNullOrEmpty())
			{
				path = string.Format("{0}/{1}.event.log", text, BaseName);
			}
			else
			{
				path = string.Format("{0}/{1}.log", text, message.LogType);
			}
			Directory.CreateDirectory(text);
			StreamWriter file = GetFile(path);
			string value = string.Format("{0}\tLogType={1}\tEventType={2}\t{3}\r\n", new object[]
			{
				message.Timestamp,
				message.LogType,
				message.EventType,
				message.DataString
			});
			file.Write(value);
			file.Flush();
		}
	}

	private StreamWriter GetFile(string path)
	{
		StreamWriter streamWriter = m_files.TryGetValue(path);
		if (streamWriter != null)
		{
			return streamWriter;
		}
		streamWriter = new StreamWriter(new FileStream(path, FileMode.Append, FileAccess.Write, FileShare.ReadWrite));
		m_files.Add(path, streamWriter);
		return streamWriter;
	}
}
#endif
