// ROGUES
// SERVER
using System;
using UnityEngine.Networking;

// server-only
#if SERVER
[Serializable]
public class LaunchGameResponse : AllianceResponseBase
{
	internal LobbyGameInfo GameInfo;

	internal string GameServerAddress;

	public override void Serialize(NetworkWriter writer)
	{
		base.Serialize(writer);
		AllianceMessageBase.SerializeObject(this.GameInfo, writer);
		writer.Write(this.GameServerAddress);
	}
}
#endif
