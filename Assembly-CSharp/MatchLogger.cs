using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class MatchLogger : MonoBehaviour
{
	public bool m_logMatch;

	private static MatchLogger s_instance;

	private float m_matchStartTime;

	private float m_matchStartRealTime;

	private StreamWriter m_currentLogFile;

	public static MatchLogger Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	private void Start()
	{
	}

	public void NewMatch()
	{
		if (!NetworkServer.active || !m_logMatch)
		{
			return;
		}
		while (true)
		{
			string arg = new StringBuilder().Append("Match-").Append(HydrogenConfig.Get().ProcessCode).Append(".log").ToString();
			DateTime now = DateTime.Now;
			string arg2 = new StringBuilder().AppendFormat("{0:d4}", now.Year).Append("-").AppendFormat("{0:d2}", now.Month).Append("-").AppendFormat("{0:d2}", now.Day).ToString();
			string text = new StringBuilder().Append(HydrogenConfig.Get().MatchLogsPath).Append("/").Append(arg2).Append("/").Append(arg).ToString();
			FileInfo fileInfo = new FileInfo(text);
			fileInfo.Directory.Create();
			m_currentLogFile = new StreamWriter(text);
			m_currentLogFile.AutoFlush = true;
			Application.logMessageReceived -= HandleLog;
			Application.logMessageReceived += HandleLog;
			m_matchStartTime = Time.time;
			m_matchStartRealTime = Time.realtimeSinceStartup;
			Debug.Log(new StringBuilder().Append("matchlogger file at: ").Append(text).ToString());
			return;
		}
	}

	public void ResetMatchStartTime()
	{
		m_matchStartRealTime = Time.realtimeSinceStartup;
	}

	public string GetTimeForLogging(bool inSeconds)
	{
		if (inSeconds)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					return ((int)(Time.time - m_matchStartTime)).ToString();
				}
			}
		}
		float num = Time.time - m_matchStartTime;
		int num2 = Mathf.FloorToInt(num / 60f);
		int num3 = Mathf.FloorToInt(num - (float)num2 * 60f);
		return new StringBuilder().Append(num2).Append("m ").Append(num3).Append("s").ToString();
	}

	public float GetTimeSinceMatchStart()
	{
		return Time.realtimeSinceStartup - m_matchStartRealTime;
	}

	public string GetExactTimeForLogging()
	{
		float num = Time.realtimeSinceStartup - m_matchStartRealTime;
		int num2 = Mathf.FloorToInt(num * 1000f);
		return new StringBuilder().Append(num2).ToString();
	}

	public TimeSpan GetMatchTime()
	{
		float value = Time.realtimeSinceStartup - m_matchStartRealTime;
		return new TimeSpan(0, 0, Convert.ToInt32(value));
	}

	public void LogTime()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			if (!m_logMatch)
			{
				return;
			}
			while (true)
			{
				if (m_currentLogFile != null)
				{
					while (true)
					{
						string value = new StringBuilder().Append("Match time: ").Append(GetTimeForLogging(false)).ToString();
						m_currentLogFile.WriteLine(value);
						return;
					}
				}
				return;
			}
		}
	}

	public void Log(string text)
	{
		if (!NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			if (!m_logMatch)
			{
				return;
			}
			while (true)
			{
				if (m_currentLogFile != null)
				{
					while (true)
					{
						m_currentLogFile.WriteLine(text);
						return;
					}
				}
				return;
			}
		}
	}

	public void EndMatch()
	{
		if (!NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			if (m_currentLogFile != null && m_logMatch)
			{
				while (true)
				{
					m_currentLogFile.Close();
					m_currentLogFile = null;
					Application.logMessageReceived -= HandleLog;
					return;
				}
			}
			return;
		}
	}

	public void OnDisable()
	{
		EndMatch();
	}

	private void HandleLog(string logString, string stackTrace, LogType type)
	{
		m_currentLogFile.WriteLine(new StringBuilder().Append(type.ToString()).Append(":").Append(logString).ToString());
		m_currentLogFile.WriteLine(stackTrace);
	}
}
