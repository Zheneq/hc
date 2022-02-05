// ROGUES
// SERVER
using System;
using UnityEngine.Networking;

// server-only, missing in reactor
#if SERVER
[Serializable]
public class DisconnectPlayerRequest : AllianceMessageBase
{
	internal LobbySessionInfo SessionInfo;
	internal LobbyServerPlayerInfo PlayerInfo;
	internal GameResult GameResult;

	public override void Deserialize(NetworkReader reader)
	{
		base.Deserialize(reader);
		AllianceMessageBase.DeserializeObject<LobbySessionInfo>(out this.SessionInfo, reader);
		AllianceMessageBase.DeserializeObject<LobbyServerPlayerInfo>(out this.PlayerInfo, reader);
		this.GameResult = (GameResult)reader.ReadByte();
	}
}
#endif
