// ROGUES
// SERVER
using System;
using UnityEngine.Networking;

#if SERVER
[Serializable]
// added in rogues
public class ServerGameMetricsNotification : AllianceMessageBase
{
    internal ServerGameMetrics GameMetrics { get; set; }

    public override void Serialize(NetworkWriter writer)
    {
        base.Serialize(writer);
        SerializeObject(GameMetrics, writer);
    }
}
#endif