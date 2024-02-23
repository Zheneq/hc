using UnityEngine.Networking;

public class MyNetworkClient : NetworkClient
{
	public string UserHandle
	{
		get;
		set;
	}

	public bool UseSSL
	{
		get;
		set;
	}

	public bool IsConnected
	{
		get { return m_AsyncConnect == ConnectState.Connected; }
		set { m_AsyncConnect = value ? ConnectState.Connected : ConnectState.Disconnected; }
	}

	public override void Disconnect()
	{
		if (m_Connection is MyNetworkClientConnection myNetworkClientConnection)
		{
			myNetworkClientConnection.Close();
		}
		base.Disconnect();
	}
}
