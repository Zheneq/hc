// ROGUES
// SERVER
using System;

// custom
#if SERVER
public class ReplayRecorderNetworkConnection: MyNetworkClientConnection
{
    public event Action<byte[], int> OnMessage = delegate { };
    public bool ConsumeMessages = false;
    
    public override void TransportReceive(byte[] bytes, int numBytes, int channelId)
    {
        ReceiveMessage(bytes, numBytes);
        if (!ConsumeMessages)
        {
            base.TransportReceive(bytes, numBytes, channelId);
        }
    }

    public override void TransportRecieve(byte[] bytes, int numBytes, int channelId)
    {
        ReceiveMessage(bytes, numBytes);
        if (!ConsumeMessages)
        {
            base.TransportReceive(bytes, numBytes, channelId);
        }
    }

    private void ReceiveMessage(byte[] bytes, int numBytes)
    {
        OnMessage(bytes, numBytes);
    }
}
#endif
