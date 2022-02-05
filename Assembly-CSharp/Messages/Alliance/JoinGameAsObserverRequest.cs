// ROGUES
// SERVER
using System;

// server-only, missing in reactor
#if SERVER
[Serializable]
public class JoinGameAsObserverRequest : AllianceMessageBase
{
	public long AccountId;
}
#endif
