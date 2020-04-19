using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;

public class SystemPerformanceMetrics
{
	private DateTime m_lastUpdate;

	private PerformanceCounter m_cpuUsage;

	private static Regex s_memoryTotalRegex = new Regex("^MemTotal:\\s+(\\d+)");

	private static Regex s_memoryFreeRegex = new Regex("^MemFree:\\s+(\\d+)");

	private static Regex s_memoryCachedRegex = new Regex("^Cached:\\s+(\\d+)");

	private static Regex s_memoryBuffersRegex = new Regex("^Buffers:\\s+(\\d+)");

	private static Regex s_swapTotalRegex = new Regex("^SwapTotal:\\s+(\\d+)");

	private static Regex s_swapFreeRegex = new Regex("^SwapFree:\\s+(\\d+)");

	public SystemPerformanceMetrics()
	{
		this.m_cpuUsage = new PerformanceCounter("Processor", "% Processor Time", "_Total");
	}

	public float CpuUsedPercent { get; private set; }

	public float PhysicalMemoryUsedPercent { get; private set; }

	public float PhysicalMemoryUsedMb { get; private set; }

	public float PhysicalMemoryAvailableMb { get; private set; }

	public float PhysicalMemoryTotalMb { get; private set; }

	public float VirtualMemoryUsedPercent { get; private set; }

	public float VirtualMemoryUsedMb { get; private set; }

	public float VirtualMemoryAvailableMb { get; private set; }

	public float VirtualMemoryTotalMb { get; private set; }

	public void Update()
	{
		DateTime utcNow = DateTime.UtcNow;
		if ((utcNow - this.m_lastUpdate).TotalSeconds < 1.0)
		{
			return;
		}
		this.m_lastUpdate = utcNow;
		this.CpuUsedPercent = this.m_cpuUsage.NextValue();
		if (Environment.OSVersion.Platform == PlatformID.Win32NT)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SystemPerformanceMetrics.Update()).MethodHandle;
			}
			SystemPerformanceMetrics.PsApiPerformanceInformation psApiPerformanceInformation = default(SystemPerformanceMetrics.PsApiPerformanceInformation);
			if (SystemPerformanceMetrics.GetPerformanceInfo(out psApiPerformanceInformation, Marshal.SizeOf(psApiPerformanceInformation)))
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				long num = psApiPerformanceInformation.PageSize.ToInt64();
				this.PhysicalMemoryTotalMb = (float)(psApiPerformanceInformation.PhysicalTotal.ToInt64() * num / 0x400L / 0x400L);
				this.PhysicalMemoryAvailableMb = (float)(psApiPerformanceInformation.PhysicalAvailable.ToInt64() * num / 0x400L / 0x400L);
				this.PhysicalMemoryUsedMb = this.PhysicalMemoryTotalMb - this.PhysicalMemoryAvailableMb;
				this.VirtualMemoryTotalMb = (float)(psApiPerformanceInformation.CommitLimit.ToInt64() * num / 0x400L / 0x400L) - this.PhysicalMemoryTotalMb;
				this.VirtualMemoryUsedMb = (float)(psApiPerformanceInformation.CommitTotal.ToInt64() * num / 0x400L / 0x400L) - this.PhysicalMemoryUsedMb;
				this.VirtualMemoryAvailableMb = this.VirtualMemoryTotalMb - this.VirtualMemoryUsedMb;
			}
		}
		else if (Environment.OSVersion.Platform == PlatformID.Unix)
		{
			string[] array = File.ReadAllLines("/proc/meminfo");
			float physicalMemoryTotalMb = 0f;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float virtualMemoryTotalMb = 0f;
			float virtualMemoryAvailableMb = 0f;
			foreach (string memInfoLine in array)
			{
				this.CheckMemInfoValue(memInfoLine, SystemPerformanceMetrics.s_memoryTotalRegex, ref physicalMemoryTotalMb);
				this.CheckMemInfoValue(memInfoLine, SystemPerformanceMetrics.s_memoryFreeRegex, ref num2);
				this.CheckMemInfoValue(memInfoLine, SystemPerformanceMetrics.s_memoryCachedRegex, ref num3);
				this.CheckMemInfoValue(memInfoLine, SystemPerformanceMetrics.s_memoryBuffersRegex, ref num4);
				this.CheckMemInfoValue(memInfoLine, SystemPerformanceMetrics.s_swapTotalRegex, ref virtualMemoryTotalMb);
				this.CheckMemInfoValue(memInfoLine, SystemPerformanceMetrics.s_swapFreeRegex, ref virtualMemoryAvailableMb);
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			this.PhysicalMemoryTotalMb = physicalMemoryTotalMb;
			this.PhysicalMemoryAvailableMb = num2 + num3 + num4;
			this.PhysicalMemoryUsedMb = this.PhysicalMemoryTotalMb - this.PhysicalMemoryAvailableMb;
			this.VirtualMemoryTotalMb = virtualMemoryTotalMb;
			this.VirtualMemoryAvailableMb = virtualMemoryAvailableMb;
			this.VirtualMemoryUsedMb = this.VirtualMemoryTotalMb - this.VirtualMemoryAvailableMb;
		}
		if (this.PhysicalMemoryTotalMb != 0f)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			this.PhysicalMemoryUsedPercent = 100f * this.PhysicalMemoryUsedMb / this.PhysicalMemoryTotalMb;
		}
		if (this.VirtualMemoryTotalMb != 0f)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			this.VirtualMemoryUsedPercent = 100f * this.VirtualMemoryUsedMb / this.VirtualMemoryTotalMb;
		}
	}

	private unsafe void CheckMemInfoValue(string memInfoLine, Regex regex, ref float value)
	{
		Match match = regex.Match(memInfoLine);
		if (match.Groups[1].Success)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(SystemPerformanceMetrics.CheckMemInfoValue(string, Regex, float*)).MethodHandle;
			}
			value = (float)Convert.ToInt64(match.Groups[1].Value) / 1024f;
		}
	}

	[DllImport("psapi.dll", SetLastError = true)]
	[return: MarshalAs(UnmanagedType.Bool)]
	private static extern bool GetPerformanceInfo(out SystemPerformanceMetrics.PsApiPerformanceInformation PerformanceInformation, [In] int Size);

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
			this.Machine = 0;
			this.NumberOfSections = 0;
			this.TimeDateStamp = 0U;
			this.PointerToSymbolTable = 0U;
			this.NumberOfSymbols = 0U;
			this.SizeOfOptionalHeader = 0;
			this.Characteristics = 0;
		}
	}
}
