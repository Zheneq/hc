// SERVER
using System;
using UnityEngine.Networking;

// server-only, missing in reactor
// Custom AdminShutdownGame
#if SERVER
[Serializable]
public class AdminShutdownGameRequest : AllianceMessageBase
{
    internal GameResult GameResult;

    public override void Deserialize(NetworkReader reader)
    {
        base.Deserialize(reader);
        this.GameResult = (GameResult)reader.ReadByte();
    }
}
#endif
