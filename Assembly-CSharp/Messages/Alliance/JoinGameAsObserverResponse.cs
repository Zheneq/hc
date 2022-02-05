// ROGUES
// SERVER
using System;
using UnityEngine.Networking;

// server-only, missing in reactor
#if SERVER
[Serializable]
public class JoinGameAsObserverResponse : AllianceResponseBase
{
	internal LobbyGameplayOverrides GameplayOverrides;
	internal LobbyGameInfo GameInfo;
	internal LobbyTeamInfo TeamInfo;
	internal LobbyPlayerInfo PlayerInfo;

	public override void Serialize(NetworkWriter writer)
	{
		Serialize(writer);
		SerializeObject(GameplayOverrides, writer);
		SerializeObject(GameInfo, writer);
		SerializeObject(TeamInfo, writer);
		SerializeObject(PlayerInfo, writer);
	}
}
#endif
