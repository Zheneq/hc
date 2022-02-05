// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine.Networking;

// server-only, missing in reactor
#if SERVER
public class LaunchGameRequest : AllianceMessageBase
{
	internal LobbyGameInfo GameInfo;
	internal LobbyServerTeamInfo TeamInfo;
	internal Dictionary<int, LobbySessionInfo> SessionInfo;
	internal LobbyGameplayOverrides GameplayOverrides;

	public override void Deserialize(NetworkReader reader)
	{
		base.Deserialize(reader);
		DeserializeObject<LobbyGameInfo>(out this.GameInfo, reader);
		DeserializeObject<LobbyServerTeamInfo>(out this.TeamInfo, reader);
		int num = reader.ReadInt32();
		SessionInfo = new Dictionary<int, LobbySessionInfo>(num);
		for (int i = 0; i < num; i++)
		{
			int key = reader.ReadInt32();
			LobbySessionInfo lobbySessionInfo = new LobbySessionInfo();
			lobbySessionInfo.Deserialize(reader);
			SessionInfo[key] = lobbySessionInfo;
		}
		DeserializeObject<LobbyGameplayOverrides>(out this.GameplayOverrides, reader);
	}
}
#endif
