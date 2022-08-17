// ROGUES
// SERVER
using System;

#if SERVER
[Serializable]
// added in rogues
public class MonitorHeartbeatNotification : AllianceMessageBase
{
}
#endif