// SERVER
using System;
using UnityEngine.Networking;

// server-only, missing in reactor
// Custom AdminClearcooldownsRequest
#if SERVER
[Serializable]
public class AdminClearCooldownsRequest : AllianceMessageBase
{

}
#endif
