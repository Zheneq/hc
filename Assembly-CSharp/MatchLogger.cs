using System;
using System.IO;
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
		return MatchLogger.s_instance;
	}

	private void Awake()
	{
		MatchLogger.s_instance = this;
	}

	private void OnDestroy()
	{
		MatchLogger.s_instance = null;
	}

	private void Start()
	{
	}

	public void NewMatch()
	{
		if (NetworkServer.active && this.m_logMatch)
		{
			string arg = string.Format("Match-{0}.log", HydrogenConfig.Get().ProcessCode);
			DateTime now = DateTime.Now;
			string arg2 = string.Format("{0:d4}-{1:d2}-{2:d2}", now.Year, now.Month, now.Day);
			string text = string.Format("{0}/{1}/{2}", HydrogenConfig.Get().MatchLogsPath, arg2, arg);
			FileInfo fileInfo = new FileInfo(text);
			fileInfo.Directory.Create();
			this.m_currentLogFile = new StreamWriter(text);
			this.m_currentLogFile.AutoFlush = true;
			Application.logMessageReceived -= this.HandleLog;
			Application.logMessageReceived += this.HandleLog;
			this.m_matchStartTime = Time.time;
			this.m_matchStartRealTime = Time.realtimeSinceStartup;
			Debug.Log("matchlogger file at: " + text);
		}
	}

	public void ResetMatchStartTime()
	{
		this.m_matchStartRealTime = Time.realtimeSinceStartup;
	}

	public string GetTimeForLogging(bool inSeconds)
	{
		if (inSeconds)
		{
			return ((int)(Time.time - this.m_matchStartTime)).ToString();
		}
		float num = Time.time - this.m_matchStartTime;
		int num2 = Mathf.FloorToInt(num / 60f);
		int num3 = Mathf.FloorToInt(num - (float)num2 * 60f);
		return string.Format("{0}m {1}s", num2, num3);
	}

	public float GetTimeSinceMatchStart()
	{
		return Time.realtimeSinceStartup - this.m_matchStartRealTime;
	}

	public string GetExactTimeForLogging()
	{
		float num = Time.realtimeSinceStartup - this.m_matchStartRealTime;
		int num2 = Mathf.FloorToInt(num * 1000f);
		return string.Format("{0}", num2);
	}

	public TimeSpan GetMatchTime()
	{
		float value = Time.realtimeSinceStartup - this.m_matchStartRealTime;
		return new TimeSpan(0, 0, Convert.ToInt32(value));
	}

	public void LogTime()
	{
		if (NetworkServer.active)
		{
			if (this.m_logMatch)
			{
				if (this.m_currentLogFile != null)
				{
					string value = "Match time: " + this.GetTimeForLogging(false);
					this.m_currentLogFile.WriteLine(value);
				}
			}
		}
	}

	public void Log(string text)
	{
		if (NetworkServer.active)
		{
			if (this.m_logMatch)
			{
				if (this.m_currentLogFile != null)
				{
					this.m_currentLogFile.WriteLine(text);
				}
			}
		}
	}

	public void EndMatch()
	{
		if (NetworkServer.active)
		{
			if (this.m_currentLogFile != null && this.m_logMatch)
			{
				this.m_currentLogFile.Close();
				this.m_currentLogFile = null;
				Application.logMessageReceived -= this.HandleLog;
			}
		}
	}

	public void OnDisable()
	{
		this.EndMatch();
	}

	private void HandleLog(string logString, string stackTrace, LogType type)
	{
		this.m_currentLogFile.WriteLine(string.Format("{0}:{1}", type.ToString(), logString));
		this.m_currentLogFile.WriteLine(stackTrace);
	}
}
