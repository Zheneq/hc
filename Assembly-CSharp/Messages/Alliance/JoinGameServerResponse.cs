// ROGUES
// SERVER
using System;
using UnityEngine.Networking;

// server-only
#if SERVER
[Serializable]
public class JoinGameServerResponse : AllianceResponseBase
{
	internal int OrigRequestId;
	internal LobbyServerPlayerInfo PlayerInfo;
	internal string GameServerProcessCode;

	public override void Serialize(NetworkWriter writer)
	{
		base.Serialize(writer);
		writer.Write(this.OrigRequestId);
		AllianceMessageBase.SerializeObject(this.PlayerInfo, writer);
		writer.Write(this.GameServerProcessCode);
	}
}
#endif
