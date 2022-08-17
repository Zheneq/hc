// ROGUES
// SERVER

using System;
using UnityEngine.Networking;

#if SERVER
[Serializable]
public class ServerGameSummaryNotification : AllianceMessageBase
{
    internal LobbyGameSummary GameSummary;

    internal LobbyGameSummaryOverrides GameSummaryOverrides;

    public override void Serialize(NetworkWriter writer)
    {
        base.Serialize(writer);
        SerializeObject(GameSummary, writer);
        SerializeObject(GameSummaryOverrides, writer);
    }
}
#endif