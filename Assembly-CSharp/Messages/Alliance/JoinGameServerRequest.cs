// ROGUES
// SERVER
using System;
using UnityEngine.Networking;

// server-only, missing in reactor
#if SERVER
[Serializable]
public class JoinGameServerRequest : AllianceMessageBase
{
	internal int OrigRequestId;
	internal LobbySessionInfo SessionInfo;
	internal LobbyServerPlayerInfo PlayerInfo;
	internal string GameServerProcessCode;

	public override void Deserialize(NetworkReader reader)
	{
		base.Deserialize(reader);
		this.OrigRequestId = reader.ReadInt32();
		AllianceMessageBase.DeserializeObject<LobbySessionInfo>(out this.SessionInfo, reader);
		AllianceMessageBase.DeserializeObject<LobbyServerPlayerInfo>(out this.PlayerInfo, reader);
		this.GameServerProcessCode = reader.ReadString();
	}
}
#endif
