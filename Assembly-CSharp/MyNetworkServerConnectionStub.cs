using System.Collections.Generic;
using UnityEngine.Networking;

#if SERVER
// custom
public class MyNetworkServerConnectionStub : MyNetworkServerConnection
{
    public MyNetworkServerConnectionStub(MyNetworkServerConnection oldConnection)
        : base (true, oldConnection.GetMessages())
    {
        // Log.Info($"Replay recorder connecting");
        // MyNetworkClient myNetworkClient = new MyNetworkClient
        // {
        //     UserHandle = "reconnection_replay_recorder",
        //     UseSSL = false
        // };
        // myNetworkClient.SetNetworkConnectionClass<ReplayRecorderNetworkConnection>();
        // m_client = myNetworkClient;
        // MyNetworkManager.Get().UseExternalClient(m_client);
        // MyNetworkManager.Get().m_OnClientConnect += HandleConnected;
        // m_client.RegisterHandler((short)MyMsgType.LoginResponse, HandleLoginResponse);
        // NetworkManager.singleton.networkAddress = "127.0.0.1";
        // m_client.Connect("127.0.0.1", HydrogenConfig.Get().PublicPort);
    }
    
    public void Init()
    {
        Initialize(address, NetworkServer.serverHostId, NetworkServer.connections.Count, NetworkServer.hostTopology);
        // NetworkServer.SetConnectionAtIndex(this);
        isReady = true;
    }
}
#endif
