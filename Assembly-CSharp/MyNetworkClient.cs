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
				while (true)
				{
					switch (1)
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
			while (true)
			{
				switch (5)
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
			MyNetworkClientConnection myNetworkClientConnection = m_Connection as MyNetworkClientConnection;
			if (myNetworkClientConnection != null)
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
				myNetworkClientConnection.Close();
			}
		}
		base.Disconnect();
	}
}
