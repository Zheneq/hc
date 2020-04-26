using System;
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
		return ClientPerformanceCollector.s_instance;
	}

	private void Start()
	{
		ClientPerformanceCollector.s_instance = this;
		this.m_performanceInfo = new LobbyGameClientPerformanceInfo();
		this.m_lastFrameCount = Time.frameCount;
		this.m_lastRealtimeSinceStartup = Time.realtimeSinceStartup;
		this.m_cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
		this.m_ramCounter = new PerformanceCounter("Memory", "Available MBytes");
		this.m_collectCoroutine = this.CollectInternal();
	}

	private void OnDestroy()
	{
	}

	public void StartCollecting()
	{
		if (!this.m_collect)
		{
			this.m_collect = true;
			base.StartCoroutine(this.m_collectCoroutine);
		}
	}

	public void StopCollecting()
	{
		if (this.m_collect)
		{
			this.m_collect = false;
			base.StopCoroutine(this.m_collectCoroutine);
		}
	}

	private IEnumerator CollectInternal()
	{
		while (this.m_collect)
		{
			yield return new WaitForSeconds(ClientPerformanceCollector.c_collectFrequency);
			int frameCount = Time.frameCount - this.m_lastFrameCount;
			float timeSpan = Time.realtimeSinceStartup - this.m_lastRealtimeSinceStartup;
			this.m_lastFrameCount = Time.frameCount;
			this.m_lastRealtimeSinceStartup = Time.realtimeSinceStartup;
			this.m_performanceInfo.AvgFPS = (float)Mathf.RoundToInt((float)frameCount / timeSpan);
			this.m_performanceInfo.CurrentFPS = (float)Mathf.RoundToInt((float)Time.frameCount / Time.time);
			this.m_performanceInfo.CurrentCpuUsage = this.m_cpuCounter.NextValue();
			this.m_performanceInfo.CurrentMemoryUsage = this.m_ramCounter.NextValue();
			this.m_performanceInfo.AvgCpuUsage = this.GetAverage(this.m_performanceInfo.AvgCpuUsage, this.m_cpuCounterSamples++, this.m_performanceInfo.CurrentCpuUsage);
			this.m_performanceInfo.AvgMemoryUsage = this.GetAverage(this.m_performanceInfo.AvgMemoryUsage, this.m_ramCounterSamples++, this.m_performanceInfo.CurrentMemoryUsage);
			if (this.m_observedWebSocket != null)
			{
				this.m_performanceInfo.CurrentLatency = (float)this.m_observedWebSocket.RoundtripTime;
				this.m_performanceInfo.AvgLatency = this.GetAverage(this.m_performanceInfo.AvgLatency, this.m_roundtripSamples++, this.m_performanceInfo.CurrentLatency);
			}
		}
		yield break;
	}

	private float GetAverage(float prevAvg, int prevTotalSamples, float newSampleVal)
	{
		return (prevAvg * (float)prevTotalSamples + newSampleVal) / (float)(prevTotalSamples + 1);
	}

	public void ObserveRTT(WebSocket webSocket)
	{
		this.m_observedWebSocket = webSocket;
	}

	public LobbyGameClientPerformanceInfo Collect()
	{
		if (this.m_collect)
		{
			this.m_cpuCounterSamples = 0;
			this.m_ramCounterSamples = 0;
			this.m_roundtripSamples = 0;
			return this.m_performanceInfo;
		}
		return null;
	}
}
