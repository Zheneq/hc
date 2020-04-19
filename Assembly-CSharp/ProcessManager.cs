using System;
using System.Net;

public class ProcessManager
{
	private static ProcessManager s_instance;

	private string m_hostCode;

	public ProcessManager()
	{
		ProcessManager.s_instance = this;
	}

	public static ProcessManager Get()
	{
		if (ProcessManager.s_instance == null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProcessManager.Get()).MethodHandle;
			}
			ProcessManager.s_instance = new ProcessManager();
		}
		return ProcessManager.s_instance;
	}

	~ProcessManager()
	{
		ProcessManager.s_instance = null;
	}

	public int TimeCode { get; set; }

	public string GetHostCode(IPAddress address)
	{
		byte[] addressBytes = address.GetAddressBytes();
		return string.Format("{0:x2}{1:x2}{2:x2}{3:x2}", new object[]
		{
			addressBytes[0],
			addressBytes[1],
			addressBytes[2],
			addressBytes[3]
		});
	}

	public string GetHostCode(bool useFallbackResolver = false)
	{
		if (this.m_hostCode == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProcessManager.GetHostCode(bool)).MethodHandle;
			}
			IPAddress address = null;
			try
			{
				address = NetUtil.GetIPv4Address(NetUtil.GetHostName());
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
				if (!useFallbackResolver)
				{
					throw ex;
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				address = IPAddress.Loopback;
			}
			this.m_hostCode = this.GetHostCode(address);
		}
		return this.m_hostCode;
	}

	public string GetNextTimeCode()
	{
		string result;
		lock (this)
		{
			int num = (int)DateTime.UtcNow.Subtract(new DateTime(0x7B2, 1, 1)).TotalSeconds;
			if (num <= this.TimeCode)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(ProcessManager.GetNextTimeCode()).MethodHandle;
				}
				num = this.TimeCode + 1;
			}
			this.TimeCode = num;
			int num2 = num >> 0x10;
			int num3 = num & 0xFFFF;
			result = string.Format("{0:x}-{1:x4}", num2, num3);
		}
		return result;
	}

	public string GetNextProcessCode(IPAddress host = null, bool useFallbackResolver = false)
	{
		string hostCode;
		if (host != null)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(ProcessManager.GetNextProcessCode(IPAddress, bool)).MethodHandle;
			}
			hostCode = this.GetHostCode(host);
		}
		else
		{
			hostCode = this.GetHostCode(useFallbackResolver);
		}
		string arg = hostCode;
		return string.Format("{0}-{1}", arg, this.GetNextTimeCode());
	}
}
