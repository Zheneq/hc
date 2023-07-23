using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

public static class NetUtil
{
	private static Dictionary<string, IPAddress> s_hostCache = new Dictionary<string, IPAddress>();
	private static string s_hostName;

	public static string GetHostName()
	{
		if (!s_hostName.IsNullOrEmpty())
		{
			return s_hostName;
		}
		try
		{
			s_hostName = Dns.GetHostName();
			if (s_hostName == null)
			{
				throw new Exception("GetHostName() returned null");
			}
		}
		catch (Exception ex)
		{
			Log.Warning("Failed to get hostname ({0}), will use localhost", ex.Message);
			s_hostName = "localhost";
		}
		return s_hostName;
	}

	public static IPAddress GetIPv4Address(string host)
	{
		if (host.EqualsIgnoreCase("localhost"))
		{
			return IPAddress.Parse("127.0.0.1");
		}
		IPAddress cachedAddress = GetCachedAddress(host);
		if (cachedAddress != null)
		{
			return cachedAddress;
		}

		int num = 0;
		while (num < 3)
		{
			try
			{
				num++;
				IPAddress[] hostAddresses = Dns.GetHostAddresses(host);
				if (hostAddresses != null)
				{
					foreach (IPAddress ipAddress in hostAddresses)
					{
						if (ipAddress.AddressFamily == AddressFamily.InterNetwork)
						{
							SetCachedAddress(host, ipAddress);
							return ipAddress;
						}
					}
				}
			}
			catch (Exception)
			{
			}
			if (num < 3)
			{
				Thread.Sleep(100);
			}
		}
		throw new Exception($"Could not resolve {host}");
	}

	public static string GetIPv4Url(string url)
	{
		UriBuilder uriBuilder = new UriBuilder(url);
		IPAddress iPv4Address = GetIPv4Address(uriBuilder.Host);
		uriBuilder.Host = iPv4Address.ToString();
		return uriBuilder.ToString();
	}

	public static IPAddress GetCachedAddress(string host)
	{
		lock (s_hostCache)
		{
			return s_hostCache.TryGetValue(host);
		}
	}

	public static void SetCachedAddress(string host, IPAddress address)
	{
		lock (s_hostCache)
		{
			s_hostCache[host] = address;
		}
	}

	public static void ClearCachedAddresses()
	{
		s_hostCache.Clear();
	}

	public static bool IsInternalAddress(this IPAddress address)
	{
		if (address.AddressFamily != AddressFamily.InterNetwork)
		{
			throw new Exception("This method works for IPv4 addresses only");
		}
		byte[] addressBytes = address.GetAddressBytes();
		return addressBytes[0] == 127
		       || addressBytes[0] == 192 && addressBytes[1] == 168
		       || addressBytes[0] == 10;
	}

	public static string GetHostName(string address)
	{
		if (!address.Contains("://"))
		{
			address = "tcp://" + address;
		}
		Uri uri = new Uri(address);
		return uri.Host;
	}

	public static bool IsInternalAddress(string address)
	{
		return IPAddress.Parse(GetHostName(address)).IsInternalAddress();
	}

	public static IPAddress HexToIPv4Address(string hex)
	{
		if (hex.Length % 2 != 0)
		{
			hex = "0" + hex;
		}
		int length = hex.Length;
		byte[] array = new byte[length / 2];
		for (int i = 0; i < length; i += 2)
		{
			array[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
		}
		return new IPAddress(array);
	}
}
