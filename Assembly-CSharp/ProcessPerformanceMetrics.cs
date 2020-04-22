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

	public float CpuUsedPercent
	{
		get;
		private set;
	}

	public float MainThreadCpuUsedPercent
	{
		get;
		private set;
	}

	public float MemoryUsedMb
	{
		get;
		private set;
	}

	public int WorkerThreadsMax
	{
		get;
		private set;
	}

	public int WorkerThreadsActive
	{
		get;
		private set;
	}

	public float WorkItemsAddedPerSec
	{
		get;
		private set;
	}

	public int IOWorkerThreadsMax
	{
		get;
		private set;
	}

	public int IOWorkerThreadsActive
	{
		get;
		private set;
	}

	public float IOWorkItemsAddedPerSec
	{
		get;
		private set;
	}

	public float Gen0Collections
	{
		get;
		private set;
	}

	public float Gen1Collections
	{
		get;
		private set;
	}

	public float Gen2Collections
	{
		get;
		private set;
	}

	public float AllocatedBytesPerSec
	{
		get;
		private set;
	}

	public float PercentTimeSpentInGC
	{
		get;
		private set;
	}

	public ProcessPerformanceMetrics()
	{
		string instanceName;
		if (Environment.OSVersion.Platform == PlatformID.Win32NT)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			instanceName = Process.GetCurrentProcess().ProcessName;
			WorkItemsCountersExist = false;
			try
			{
				CLRMemoryCountersExist = PerformanceCounterCategory.InstanceExists(instanceName, ".NET CLR Memory");
			}
			catch (Exception exception)
			{
				CLRMemoryCountersExist = false;
				Log.Exception(exception);
			}
		}
		else
		{
			if (Environment.OSVersion.Platform != PlatformID.Unix)
			{
				throw new Exception("Unrecognized OS");
			}
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			instanceName = Process.GetCurrentProcess().Id.ToString();
			m_workItemsAddedPerSec = new PerformanceCounter("Mono Threadpool", "Work Items Added/Sec", instanceName, true);
			m_ioWorkItemsAddedPerSec = new PerformanceCounter("Mono Threadpool", "IO Work Items Added/Sec", instanceName, true);
			WorkItemsCountersExist = true;
			CLRMemoryCountersExist = true;
		}
		if (!CLRMemoryCountersExist)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			m_gen0Collections = new PerformanceCounter(".NET CLR Memory", "# Gen 0 Collections", instanceName, true);
			m_gen1Collections = new PerformanceCounter(".NET CLR Memory", "# Gen 1 Collections", instanceName, true);
			m_gen2Collections = new PerformanceCounter(".NET CLR Memory", "# Gen 2 Collections", instanceName, true);
			m_allocatedBytesPerSec = new PerformanceCounter(".NET CLR Memory", "Allocated Bytes/sec", instanceName, true);
			m_timeInGC = new PerformanceCounter(".NET CLR Memory", "% Time in GC", instanceName, true);
			return;
		}
	}

	public void Update()
	{
		DateTime utcNow = DateTime.UtcNow;
		TimeSpan timeSpan = utcNow - m_lastUpdate;
		if (timeSpan.TotalSeconds < 1.0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		m_lastUpdate = utcNow;
		Process currentProcess = Process.GetCurrentProcess();
		double num = GetCpuUsageSeconds(false);
		if (m_lastTotalCpuUsageSeconds != 0.0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			double num2 = num - m_lastTotalCpuUsageSeconds;
			CpuUsedPercent = (float)(num2 * 100.0 / timeSpan.TotalSeconds);
		}
		m_lastTotalCpuUsageSeconds = num;
		double num3 = GetCpuUsageSeconds(true);
		if (m_lastMainThreadTotalCpuUsageSeconds != 0.0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			double num4 = num3 - m_lastMainThreadTotalCpuUsageSeconds;
			MainThreadCpuUsedPercent = (float)(num4 * 100.0 / timeSpan.TotalSeconds);
		}
		m_lastMainThreadTotalCpuUsageSeconds = num3;
		MemoryUsedMb = (float)currentProcess.WorkingSet64 / 1024f / 1024f;
		ThreadPool.GetMaxThreads(out int workerThreads, out int completionPortThreads);
		ThreadPool.GetAvailableThreads(out int workerThreads2, out int completionPortThreads2);
		WorkerThreadsMax = workerThreads;
		WorkerThreadsActive = workerThreads - workerThreads2;
		IOWorkerThreadsMax = completionPortThreads;
		IOWorkerThreadsActive = completionPortThreads - completionPortThreads2;
		if (WorkItemsCountersExist)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			WorkItemsAddedPerSec = m_workItemsAddedPerSec.NextValue();
			IOWorkItemsAddedPerSec = m_ioWorkItemsAddedPerSec.NextValue();
		}
		if (!CLRMemoryCountersExist)
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			float gen0Collections;
			if (m_gen0Collections != null)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
				gen0Collections = m_gen0Collections.NextValue();
			}
			else
			{
				gen0Collections = 0f;
			}
			Gen0Collections = gen0Collections;
			Gen1Collections = ((m_gen1Collections == null) ? 0f : m_gen1Collections.NextValue());
			float gen2Collections;
			if (m_gen2Collections != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				gen2Collections = m_gen2Collections.NextValue();
			}
			else
			{
				gen2Collections = 0f;
			}
			Gen2Collections = gen2Collections;
			float allocatedBytesPerSec;
			if (m_allocatedBytesPerSec != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				allocatedBytesPerSec = m_allocatedBytesPerSec.NextValue();
			}
			else
			{
				allocatedBytesPerSec = 0f;
			}
			AllocatedBytesPerSec = allocatedBytesPerSec;
			PercentTimeSpentInGC = ((m_timeInGC == null) ? 0f : m_timeInGC.NextValue());
			return;
		}
	}

	private float GetCpuUsageSeconds(bool mainThread)
	{
		if (Environment.OSVersion.Platform == PlatformID.Win32NT)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					Process currentProcess = Process.GetCurrentProcess();
					if (mainThread)
					{
						while (true)
						{
							switch (6)
							{
							case 0:
								break;
							default:
							{
								ProcessThread processThread = currentProcess.Threads[Thread.CurrentThread.ManagedThreadId - 1];
								return (float)processThread.TotalProcessorTime.TotalSeconds;
							}
							}
						}
					}
					return (float)currentProcess.TotalProcessorTime.TotalSeconds;
				}
				}
			}
		}
		if (Environment.OSVersion.Platform == PlatformID.Unix)
		{
			int id = Process.GetCurrentProcess().Id;
			string path;
			if (mainThread)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				path = string.Format("/proc/{0}/task/{0}/stat", id);
			}
			else
			{
				path = $"/proc/{id}/stat";
			}
			string[] array = File.ReadAllLines(path);
			Match match = s_cpuRegex.Match(array[0]);
			if (match.Groups[1].Success)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (match.Groups[2].Success)
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							break;
						default:
						{
							float num = Convert.ToInt64(match.Groups[1].Value);
							float num2 = Convert.ToInt64(match.Groups[2].Value);
							return (num + num2) / 100f;
						}
						}
					}
				}
			}
			throw new Exception("Failed to parse /proc/pid/stat");
		}
		throw new Exception("Invalid OS");
	}
}
