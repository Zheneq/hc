using System;
using UnityEngine.Networking;

public class MyNetworkClient : NetworkClient
{
	public string UserHandle { get; set; }

	public bool UseSSL { get; set; }

	public bool IsConnected
	{
		get
		{
			return this.m_AsyncConnect == NetworkClient.ConnectState.Connected;
		}
		set
		{
			NetworkClient.ConnectState asyncConnect;
			if (value)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkClient.set_IsConnected(bool)).MethodHandle;
				}
				asyncConnect = NetworkClient.ConnectState.Connected;
			}
			else
			{
				asyncConnect = NetworkClient.ConnectState.Disconnected;
			}
			this.m_AsyncConnect = asyncConnect;
		}
	}

	public override void Disconnect()
	{
		if (this.m_Connection != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MyNetworkClient.Disconnect()).MethodHandle;
			}
			MyNetworkClientConnection myNetworkClientConnection = this.m_Connection as MyNetworkClientConnection;
			if (myNetworkClientConnection != null)
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
				myNetworkClientConnection.Close();
			}
		}
		base.Disconnect();
	}
}
