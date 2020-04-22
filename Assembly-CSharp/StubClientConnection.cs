using UnityEngine.Networking;

public class StubClientConnection : NetworkConnection
{
	private MyNetworkClient m_myNetworkClient;

	public override void Initialize(string networkAddress, int networkHostId, int networkConnectionId, HostTopology hostTopology)
	{
		base.Initialize(networkAddress, networkHostId, networkConnectionId, hostTopology);
		m_myNetworkClient = (NetworkManager.singleton.client as MyNetworkClient);
		m_myNetworkClient.IsConnected = true;
		connectionId = networkConnectionId;
		InvokeHandlerNoData(32);
	}

	public override bool TransportSend(byte[] bytes, int numBytes, int channelId, out byte error)
	{
		error = 0;
		return true;
	}
}
