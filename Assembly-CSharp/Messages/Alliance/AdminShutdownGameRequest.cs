// SERVER
// ROGUES
using System;
using UnityEngine.Networking;

// custom
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
