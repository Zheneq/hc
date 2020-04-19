using System;
using UnityEngine.Networking;

internal class NetworkGroups
{
	internal static void RemoveRPCs()
	{
	}

	internal static void OnTeamChange(PlayerController netPlayer, Team team)
	{
		if (!NetworkServer.active)
		{
			Log.Warning("Server only function called with no client is active", new object[0]);
			return;
		}
	}
}
