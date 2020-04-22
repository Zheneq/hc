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
		get
		{
			return m_AsyncConnect == ConnectState.Connected;
		}
		set
		{
			int asyncConnect;
			if (value)
			{
				asyncConnect = 4;
			}
			else
			{
				asyncConnect = 5;
			}
			m_AsyncConnect = (ConnectState)asyncConnect;
		}
	}

	public override void Disconnect()
	{
		if (m_Connection != null)
		{
			MyNetworkClientConnection myNetworkClientConnection = m_Connection as MyNetworkClientConnection;
			if (myNetworkClientConnection != null)
			{
				myNetworkClientConnection.Close();
			}
		}
		base.Disconnect();
	}
}
