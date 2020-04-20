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
			MyNetworkClientConnection myNetworkClientConnection = this.m_Connection as MyNetworkClientConnection;
			if (myNetworkClientConnection != null)
			{
				myNetworkClientConnection.Close();
			}
		}
		base.Disconnect();
	}
}
