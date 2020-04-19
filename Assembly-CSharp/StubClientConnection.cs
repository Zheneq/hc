using System;
using UnityEngine.Networking;

public class StubClientConnection : NetworkConnection
{
	private MyNetworkClient m_myNetworkClient;

	public override void Initialize(string networkAddress, int networkHostId, int networkConnectionId, HostTopology hostTopology)
	{
		base.Initialize(networkAddress, networkHostId, networkConnectionId, hostTopology);
		this.m_myNetworkClient = (NetworkManager.singleton.client as MyNetworkClient);
		this.m_myNetworkClient.IsConnected = true;
		this.connectionId = networkConnectionId;
		base.InvokeHandlerNoData(0x20);
	}

	public override bool TransportSend(byte[] bytes, int numBytes, int channelId, out byte error)
	{
		error = 0;
		return true;
	}
}
