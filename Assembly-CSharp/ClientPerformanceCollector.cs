using System.Collections;
using System.Diagnostics;
using UnityEngine;

public class ClientPerformanceCollector : MonoBehaviour
{
	private static ClientPerformanceCollector s_instance;

	private LobbyGameClientPerformanceInfo m_performanceInfo;

	private bool m_collect;

	private IEnumerator m_collectCoroutine;

	private static readonly float c_collectFrequency = 5f;

	private int m_lastFrameCount;

	private float m_lastRealtimeSinceStartup;

	private PerformanceCounter m_cpuCounter;

	private PerformanceCounter m_ramCounter;

	private WebSocket m_observedWebSocket;

	private int m_cpuCounterSamples;

	private int m_ramCounterSamples;

	private int m_roundtripSamples;

	internal static ClientPerformanceCollector Get()
	{
		return s_instance;
	}

	private void Start()
	{
		s_instance = this;
		m_performanceInfo = new LobbyGameClientPerformanceInfo();
		m_lastFrameCount = Time.frameCount;
		m_lastRealtimeSinceStartup = Time.realtimeSinceStartup;
		m_cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
		m_ramCounter = new PerformanceCounter("Memory", "Available MBytes");
		m_collectCoroutine = CollectInternal();
	}

	private void OnDestroy()
	{
	}

	public void StartCollecting()
	{
		if (!m_collect)
		{
			m_collect = true;
			StartCoroutine(m_collectCoroutine);
		}
	}

	public void StopCollecting()
	{
		if (!m_collect)
		{
			return;
		}
		while (true)
		{
			m_collect = false;
			StopCoroutine(m_collectCoroutine);
			return;
		}
	}

	private IEnumerator CollectInternal()
	{
		while (m_collect)
		{
			yield return new WaitForSeconds(c_collectFrequency);
			int frameCount = Time.frameCount - m_lastFrameCount;
			float timeSpan = Time.realtimeSinceStartup - m_lastRealtimeSinceStartup;
			m_lastFrameCount = Time.frameCount;
			m_lastRealtimeSinceStartup = Time.realtimeSinceStartup;
			m_performanceInfo.AvgFPS = Mathf.RoundToInt((float)frameCount / timeSpan);
			m_performanceInfo.CurrentFPS = Mathf.RoundToInt((float)Time.frameCount / Time.time);
			m_performanceInfo.CurrentCpuUsage = m_cpuCounter.NextValue();
			m_performanceInfo.CurrentMemoryUsage = m_ramCounter.NextValue();
			m_performanceInfo.AvgCpuUsage = GetAverage(m_performanceInfo.AvgCpuUsage, m_cpuCounterSamples++, m_performanceInfo.CurrentCpuUsage);
			m_performanceInfo.AvgMemoryUsage = GetAverage(m_performanceInfo.AvgMemoryUsage, m_ramCounterSamples++, m_performanceInfo.CurrentMemoryUsage);
			if (m_observedWebSocket != null)
			{
				m_performanceInfo.CurrentLatency = m_observedWebSocket.RoundtripTime;
				m_performanceInfo.AvgLatency = GetAverage(m_performanceInfo.AvgLatency, m_roundtripSamples++, m_performanceInfo.CurrentLatency);
			}
		}
		while (true)
		{
			yield break;
		}
	}

	private float GetAverage(float prevAvg, int prevTotalSamples, float newSampleVal)
	{
		return (prevAvg * (float)prevTotalSamples + newSampleVal) / (float)(prevTotalSamples + 1);
	}

	public void ObserveRTT(WebSocket webSocket)
	{
		m_observedWebSocket = webSocket;
	}

	public LobbyGameClientPerformanceInfo Collect()
	{
		if (m_collect)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					m_cpuCounterSamples = 0;
					m_ramCounterSamples = 0;
					m_roundtripSamples = 0;
					return m_performanceInfo;
				}
			}
		}
		return null;
	}
}
