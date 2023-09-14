using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

public static class TeamSensitiveUtils
{
  //   // custom
  //   public static bool OnRebuildObservers(HashSet<NetworkConnection> observers, bool initialize)
  //   {
		// foreach (NetworkConnection connection in NetworkServer.connections)
		// {
		// 	if (connection != null && connection.isReady && OnCheckObserver(connection))
		// 	{
		// 		observers.Add(connection);
		// 	}
		// }
		// foreach (NetworkConnection localConnection in NetworkServer.localConnections)
		// {
		// 	if (localConnection != null && localConnection.isReady && OnCheckObserver(localConnection))
		// 	{
		// 		observers.Add(localConnection);
		// 	}
		// }
		// return true;
  //   }
  //   
  //   // custom
  //   public static bool OnCheckObserver(NetworkConnection conn, ActorData owner, ActorTeamSensitiveData.ObservedBy typeObservingMe)
  //   {
	 //    Player player = GameFlow.Get().GetPlayerFromConnectionId(conn.connectionId);
		// GameFlow.Get().playerDetails.TryGetValue(player, out PlayerDetails details);
		// if (details == null || owner == null)
		// {
		// 	Log.Error($"OnCheckObserver {typeObservingMe} {owner?.m_displayName} by {details?.m_handle} {details?.m_accountId} {player}");
		// 	return false;
		// }
		//
		// bool isForFriendlies = typeObservingMe == ActorTeamSensitiveData.ObservedBy.Friendlies;
  //
		// HashSet<Team> observingTeams = new HashSet<Team>(details.AllServerPlayerInfos.Select(spi => spi.TeamId));
		// if (observingTeams.IsNullOrEmpty())
		// {
		// 	return false;
		// }
		// if (observingTeams.Contains(Team.TeamA) && observingTeams.Contains(Team.TeamB))
		// {
		// 	return isForFriendlies;
		// }
  //
		// Team observingTeam = observingTeams.First();
  //
		// Team replayRecorderTeam = ServerGameManager.GetReplayRecorderTeam(player.m_accountId);
		// if (replayRecorderTeam != Team.Invalid)
		// {
		// 	observingTeam = replayRecorderTeam;
		// }
		//
		// bool isAlly = observingTeam == owner.GetTeam() || observingTeam == Team.Spectator;
		// return isAlly == isForFriendlies;
  //   }
    
    // custom
    public static bool OnRebuildObservers_NotForReconnection(HashSet<NetworkConnection> observers, MonoBehaviour obj)
    {
	    foreach (NetworkConnection connection in NetworkServer.connections)
	    {
		    if (connection != null && connection.isReady && OnCheckObserver_NotForReconnection(connection, obj))
		    {
			    observers.Add(connection);
		    }
	    }
	    foreach (NetworkConnection localConnection in NetworkServer.localConnections)
	    {
		    if (localConnection != null && localConnection.isReady && OnCheckObserver_NotForReconnection(localConnection, obj))
		    {
			    observers.Add(localConnection);
		    }
	    }
	    return true;
    }
    
    // custom
    public static bool OnCheckObserver_NotForReconnection(NetworkConnection conn, MonoBehaviour obj)
    {
	    if (GameFlow.Get() == null)
	    {
		    Log.Info($"Failed to rebuild observers for {obj.GetType()}");
		    return true;
	    }

	    Player player = GameFlow.Get().GetPlayerFromConnectionId(conn.connectionId);
	    Team replayRecorderTeam = ServerGameManager.GetReplayRecorderTeam(player.m_accountId);
	    return replayRecorderTeam == Team.Invalid || replayRecorderTeam == Team.Spectator;
    }
}