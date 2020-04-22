using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

public class SystemPerformanceMetrics
{
	public struct PsApiPerformanceInformation
	{
		public int Size;

		public IntPtr CommitTotal;

		public IntPtr CommitLimit;

		public IntPtr CommitPeak;

		public IntPtr PhysicalTotal;

		public IntPtr PhysicalAvailable;

		public IntPtr SystemCache;

		public IntPtr KernelTotal;

		public IntPtr KernelPaged;

		public IntPtr KernelNonPaged;

		public IntPtr PageSize;

		public int HandlesCount;

		public int ProcessCount;

		public int ThreadCount;
	}

	private struct _IMAGE_FILE_HEADER
	{
		public ushort Machine;

		public ushort NumberOfSections;

		public uint TimeDateStamp;

		public uint PointerToSymbolTable;

		public uint NumberOfSymbols;

		public ushort SizeOfOptionalHeader;

		public ushort Characteristics;

		public void Reset()
		{
			Machine = 0;
			NumberOfSections = 0;
			TimeDateStamp = 0u;
			PointerToSymbolTable = 0u;
			NumberOfSymbols = 0u;
			SizeOfOptionalHeader = 0;
			Characteristics = 0;
		}
	}

	private DateTime m_lastUpdate;

	private PerformanceCounter m_cpuUsage;

	private static Regex s_memoryTotalRegex = new Regex("^MemTotal:\\s+(\\d+)");

	private static Regex s_memoryFreeRegex = new Regex("^MemFree:\\s+(\\d+)");

	private static Regex s_memoryCachedRegex = new Regex("^Cached:\\s+(\\d+)");

	private static Regex s_memoryBuffersRegex = new Regex("^Buffers:\\s+(\\d+)");

	private static Regex s_swapTotalRegex = new Regex("^SwapTotal:\\s+(\\d+)");

	private static Regex s_swapFreeRegex = new Regex("^SwapFree:\\s+(\\d+)");

	public float CpuUsedPercent
	{
		get;
		private set;
	}

	public float PhysicalMemoryUsedPercent
	{
		get;
		private set;
	}

	public float PhysicalMemoryUsedMb
	{
		get;
		private set;
	}

	public float PhysicalMemoryAvailableMb
	{
		get;
		private set;
	}

	public float PhysicalMemoryTotalMb
	{
		get;
		private set;
	}

	public float VirtualMemoryUsedPercent
	{
		get;
		private set;
	}

	public float VirtualMemoryUsedMb
	{
		get;
		private set;
	}

	public float VirtualMemoryAvailableMb
	{
		get;
		private set;
	}

	public float VirtualMemoryTotalMb
	{
		get;
		private set;
	}

	public SystemPerformanceMetrics()
	{
		m_cpuUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total");
	}

	public void Update()
	{
		DateTime utcNow = DateTime.UtcNow;
		if ((utcNow - m_lastUpdate).TotalSeconds < 1.0)
		{
			return;
		}
		m_lastUpdate = utcNow;
		CpuUsedPercent = m_cpuUsage.NextValue();
		if (Environment.OSVersion.Platform == PlatformID.Win32NT)
		{
			PsApiPerformanceInformation PerformanceInformation = default(PsApiPerformanceInformation);
			if (GetPerformanceInfo(out PerformanceInformation, Marshal.SizeOf(PerformanceInformation)))
			{
				long num = PerformanceInformation.PageSize.ToInt64();
				PhysicalMemoryTotalMb = PerformanceInformation.PhysicalTotal.ToInt64() * num / 1024 / 1024;
				PhysicalMemoryAvailableMb = PerformanceInformation.PhysicalAvailable.ToInt64() * num / 1024 / 1024;
				PhysicalMemoryUsedMb = PhysicalMemoryTotalMb - PhysicalMemoryAvailableMb;
				VirtualMemoryTotalMb = (float)(PerformanceInformation.CommitLimit.ToInt64() * num / 1024 / 1024) - PhysicalMemoryTotalMb;
				VirtualMemoryUsedMb = (float)(PerformanceInformation.CommitTotal.ToInt64() * num / 1024 / 1024) - PhysicalMemoryUsedMb;
				VirtualMemoryAvailableMb = VirtualMemoryTotalMb - VirtualMemoryUsedMb;
			}
		}
		else if (Environment.OSVersion.Platform == PlatformID.Unix)
		{
			string[] array = File.ReadAllLines("/proc/meminfo");
			float value = 0f;
			float value2 = 0f;
			float value3 = 0f;
			float value4 = 0f;
			float value5 = 0f;
			float value6 = 0f;
			string[] array2 = array;
			foreach (string memInfoLine in array2)
			{
				CheckMemInfoValue(memInfoLine, s_memoryTotalRegex, ref value);
				CheckMemInfoValue(memInfoLine, s_memoryFreeRegex, ref value2);
				CheckMemInfoValue(memInfoLine, s_memoryCachedRegex, ref value3);
				CheckMemInfoValue(memInfoLine, s_memoryBuffersRegex, ref value4);
				CheckMemInfoValue(memInfoLine, s_swapTotalRegex, ref value5);
				CheckMemInfoValue(memInfoLine, s_swapFreeRegex, ref value6);
			}
			PhysicalMemoryTotalMb = value;
			PhysicalMemoryAvailableMb = value2 + value3 + value4;
			PhysicalMemoryUsedMb = PhysicalMemoryTotalMb - PhysicalMemoryAvailableMb;
			VirtualMemoryTotalMb = value5;
			VirtualMemoryAvailableMb = value6;
			VirtualMemoryUsedMb = VirtualMemoryTotalMb - VirtualMemoryAvailableMb;
		}
		if (PhysicalMemoryTotalMb != 0f)
		{
			PhysicalMemoryUsedPercent = 100f * PhysicalMemoryUsedMb / PhysicalMemoryTotalMb;
		}
		if (VirtualMemoryTotalMb == 0f)
		{
			return;
		}
		while (true)
		{
			VirtualMemoryUsedPercent = 100f * VirtualMemoryUsedMb / VirtualMemoryTotalMb;
			return;
		}
	}

	private void CheckMemInfoValue(string memInfoLine, Regex regex, ref float value)
	{
		Match match = regex.Match(memInfoLine);
		if (!match.Groups[1].Success)
		{
			return;
		}
		while (true)
		{
			value = (float)Convert.ToInt64(match.Groups[1].Value) / 1024f;
			return;
		}
	}

	[DllImport("psapi.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool GetPerformanceInfo(out PsApiPerformanceInformation PerformanceInformation, [In] int Size);
}
