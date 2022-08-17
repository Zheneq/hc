// ROGUES
// SERVER
using System;
using UnityEngine.Networking;

#if SERVER
[Serializable]
// added in rogues
public class PlayerDisconnectedNotification : AllianceMessageBase
{
    internal LobbySessionInfo SessionInfo;
    internal LobbyServerPlayerInfo PlayerInfo;

    public override void Serialize(NetworkWriter writer)
    {
        base.Serialize(writer);
        SerializeObject(SessionInfo, writer);
        SerializeObject(PlayerInfo, writer);
    }
}
#endif