// ROGUES
// SERVER
using System;
using UnityEngine.Networking;

#if SERVER
[Serializable]
// added in rogues
public class ServerGameStatusNotification : AllianceMessageBase
{
    internal GameStatus GameStatus { get; set; }

    public override void Serialize(NetworkWriter writer)
    {
        base.Serialize(writer);
        writer.Write((byte)GameStatus);
    }
}
#endif