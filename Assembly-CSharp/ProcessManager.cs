using System;
using System.Net;

public class ProcessManager
{
	private static ProcessManager s_instance;

	private string m_hostCode;

	public int TimeCode
	{
		get;
		set;
	}

	public ProcessManager()
	{
		s_instance = this;
	}

	public static ProcessManager Get()
	{
		if (s_instance == null)
		{
			s_instance = new ProcessManager();
		}
		return s_instance;
	}

	~ProcessManager()
	{
		s_instance = null;
	}

	public string GetHostCode(IPAddress address)
	{
		byte[] addressBytes = address.GetAddressBytes();
		return $"{addressBytes[0]:x2}{addressBytes[1]:x2}{addressBytes[2]:x2}{addressBytes[3]:x2}";
	}

	public string GetHostCode(bool useFallbackResolver = false)
	{
		if (m_hostCode == null)
		{
			IPAddress iPAddress = null;
			try
			{
				iPAddress = NetUtil.GetIPv4Address(NetUtil.GetHostName());
			}
			catch (Exception ex)
			{
				Log.Exception(ex);
				if (useFallbackResolver)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							break;
						default:
							iPAddress = IPAddress.Loopback;
							goto end_IL_002c;
						}
					}
				}
				throw ex;
				end_IL_002c:;
			}
			m_hostCode = GetHostCode(iPAddress);
		}
		return m_hostCode;
	}

	public string GetNextTimeCode()
	{
		lock (this)
		{
			int num = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
			if (num <= TimeCode)
			{
				num = TimeCode + 1;
			}
			TimeCode = num;
			int num2 = num >> 16;
			int num3 = num & 0xFFFF;
			return $"{num2:x}-{num3:x4}";
		}
	}

	public string GetNextProcessCode(IPAddress host = null, bool useFallbackResolver = false)
	{
		string hostCode;
		if (host != null)
		{
			hostCode = GetHostCode(host);
		}
		else
		{
			hostCode = GetHostCode(useFallbackResolver);
		}
		string arg = hostCode;
		return $"{arg}-{GetNextTimeCode()}";
	}
}
