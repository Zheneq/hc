using System;
using System.Collections.Generic;

[Serializable]
public class DiscordServerInstance
{
	public DiscordServerInfo Server;

	public Dictionary<long, DiscordServerMemberInfo> Members;
}
