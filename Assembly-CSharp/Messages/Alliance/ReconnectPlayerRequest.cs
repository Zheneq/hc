// ROGUES
// SERVER
using System;
using UnityEngine.Networking;

// server-only, missing in reactor
#if SERVER
[Serializable]
public class ReconnectPlayerRequest : AllianceMessageBase
{
	internal long AccountId;

	internal long NewSessionId;

	public override void Deserialize(NetworkReader reader)
	{
		base.Deserialize(reader);
		this.AccountId = reader.ReadInt64();
		this.NewSessionId = reader.ReadInt64();
	}
}
#endif
