using System;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

public class ProcessPerformanceMetrics
{
	private static Regex s_cpuRegex = new Regex("^\\d+ \\(\\w+\\) \\w+ \\d+ \\d+ \\d+ \\d+ -?\\d+ \\d+ \\d+ \\d+ \\d+ \\d+ (\\d+) (\\d+)");

	private bool CLRMemoryCountersExist;

	private bool WorkItemsCountersExist;

	private DateTime m_lastUpdate;

	private double m_lastTotalCpuUsageSeconds;

	private double m_lastMainThreadTotalCpuUsageSeconds;

	private PerformanceCounter m_workItemsAddedPerSec;

	private PerformanceCounter m_ioWorkItemsAddedPerSec;

	private PerformanceCounter m_gen0Collections;

	private PerformanceCounter m_gen1Collections;

	private PerformanceCounter m_gen2Collections;

	private PerformanceCounter m_timeInGC;

	private PerformanceCounter m_allocatedBytesPerSec;

	public ProcessPerformanceMetrics()
	{
		string instanceName;
		if (Environment.OSVersion.Platform == PlatformID.Win32NT)
		{
			instanceName = Process.GetCurrentProcess().ProcessName;
			this.WorkItemsCountersExist = false;
			try
			{
				this.CLRMemoryCountersExist = PerformanceCounterCategory.InstanceExists(instanceName, ".NET CLR Memory");
			}
			catch (Exception exception)
			{
				this.CLRMemoryCountersExist = false;
				Log.Exception(exception);
			}
		}
		else
		{
			if (Environment.OSVersion.Platform != PlatformID.Unix)
			{
				throw new Exception("Unrecognized OS");
			}
			instanceName = Process.GetCurrentProcess().Id.ToString();
			this.m_workItemsAddedPerSec = new PerformanceCounter("Mono Threadpool", "Work Items Added/Sec", instanceName, true);
			this.m_ioWorkItemsAddedPerSec = new PerformanceCounter("Mono Threadpool", "IO Work Items Added/Sec", instanceName, true);
			this.WorkItemsCountersExist = true;
			this.CLRMemoryCountersExist = true;
		}
		if (this.CLRMemoryCountersExist)
		{
			this.m_gen0Collections = new PerformanceCounter(".NET CLR Memory", "# Gen 0 Collections", instanceName, true);
			this.m_gen1Collections = new PerformanceCounter(".NET CLR Memory", "# Gen 1 Collections", instanceName, true);
			this.m_gen2Collections = new PerformanceCounter(".NET CLR Memory", "# Gen 2 Collections", instanceName, true);
			this.m_allocatedBytesPerSec = new PerformanceCounter(".NET CLR Memory", "Allocated Bytes/sec", instanceName, true);
			this.m_timeInGC = new PerformanceCounter(".NET CLR Memory", "% Time in GC", instanceName, true);
		}
	}

	public float CpuUsedPercent { get; private set; }

	public float MainThreadCpuUsedPercent { get; private set; }

	public float MemoryUsedMb { get; private set; }

	public int WorkerThreadsMax { get; private set; }

	public int WorkerThreadsActive { get; private set; }

	public float WorkItemsAddedPerSec { get; private set; }

	public int IOWorkerThreadsMax { get; private set; }

	public int IOWorkerThreadsActive { get; private set; }

	public float IOWorkItemsAddedPerSec { get; private set; }

	public float Gen0Collections { get; private set; }

	public float Gen1Collections { get; private set; }

	public float Gen2Collections { get; private set; }

	public float AllocatedBytesPerSec { get; private set; }

	public float PercentTimeSpentInGC { get; private set; }

	public void Update()
	{
		DateTime utcNow = DateTime.UtcNow;
		TimeSpan timeSpan = utcNow - this.m_lastUpdate;
		if (timeSpan.TotalSeconds < 1.0)
		{
			return;
		}
		this.m_lastUpdate = utcNow;
		Process currentProcess = Process.GetCurrentProcess();
		double num = (double)this.GetCpuUsageSeconds(false);
		if (this.m_lastTotalCpuUsageSeconds != 0.0)
		{
			double num2 = num - this.m_lastTotalCpuUsageSeconds;
			this.CpuUsedPercent = (float)(num2 * 100.0 / timeSpan.TotalSeconds);
		}
		this.m_lastTotalCpuUsageSeconds = num;
		double num3 = (double)this.GetCpuUsageSeconds(true);
		if (this.m_lastMainThreadTotalCpuUsageSeconds != 0.0)
		{
			double num4 = num3 - this.m_lastMainThreadTotalCpuUsageSeconds;
			this.MainThreadCpuUsedPercent = (float)(num4 * 100.0 / timeSpan.TotalSeconds);
		}
		this.m_lastMainThreadTotalCpuUsageSeconds = num3;
		this.MemoryUsedMb = (float)currentProcess.WorkingSet64 / 1024f / 1024f;
		int num5;
		int num6;
		ThreadPool.GetMaxThreads(out num5, out num6);
		int num7;
		int num8;
		ThreadPool.GetAvailableThreads(out num7, out num8);
		this.WorkerThreadsMax = num5;
		this.WorkerThreadsActive = num5 - num7;
		this.IOWorkerThreadsMax = num6;
		this.IOWorkerThreadsActive = num6 - num8;
		if (this.WorkItemsCountersExist)
		{
			this.WorkItemsAddedPerSec = this.m_workItemsAddedPerSec.NextValue();
			this.IOWorkItemsAddedPerSec = this.m_ioWorkItemsAddedPerSec.NextValue();
		}
		if (this.CLRMemoryCountersExist)
		{
			float gen0Collections;
			if (this.m_gen0Collections != null)
			{
				gen0Collections = this.m_gen0Collections.NextValue();
			}
			else
			{
				gen0Collections = 0f;
			}
			this.Gen0Collections = gen0Collections;
			this.Gen1Collections = ((this.m_gen1Collections == null) ? 0f : this.m_gen1Collections.NextValue());
			float gen2Collections;
			if (this.m_gen2Collections != null)
			{
				gen2Collections = this.m_gen2Collections.NextValue();
			}
			else
			{
				gen2Collections = 0f;
			}
			this.Gen2Collections = gen2Collections;
			float allocatedBytesPerSec;
			if (this.m_allocatedBytesPerSec != null)
			{
				allocatedBytesPerSec = this.m_allocatedBytesPerSec.NextValue();
			}
			else
			{
				allocatedBytesPerSec = 0f;
			}
			this.AllocatedBytesPerSec = allocatedBytesPerSec;
			this.PercentTimeSpentInGC = ((this.m_timeInGC == null) ? 0f : this.m_timeInGC.NextValue());
		}
	}

	private float GetCpuUsageSeconds(bool mainThread)
	{
		if (Environment.OSVersion.Platform == PlatformID.Win32NT)
		{
			Process currentProcess = Process.GetCurrentProcess();
			if (mainThread)
			{
				ProcessThread processThread = currentProcess.Threads[Thread.CurrentThread.ManagedThreadId - 1];
				return (float)processThread.TotalProcessorTime.TotalSeconds;
			}
			return (float)currentProcess.TotalProcessorTime.TotalSeconds;
		}
		else
		{
			if (Environment.OSVersion.Platform == PlatformID.Unix)
			{
				int id = Process.GetCurrentProcess().Id;
				string path;
				if (mainThread)
				{
					path = string.Format("/proc/{0}/task/{0}/stat", id);
				}
				else
				{
					path = string.Format("/proc/{0}/stat", id);
				}
				string[] array = File.ReadAllLines(path);
				Match match = ProcessPerformanceMetrics.s_cpuRegex.Match(array[0]);
				if (match.Groups[1].Success)
				{
					if (match.Groups[2].Success)
					{
						float num = (float)Convert.ToInt64(match.Groups[1].Value);
						float num2 = (float)Convert.ToInt64(match.Groups[2].Value);
						return (num + num2) / 100f;
					}
				}
				throw new Exception("Failed to parse /proc/pid/stat");
			}
			throw new Exception("Invalid OS");
		}
	}
}
