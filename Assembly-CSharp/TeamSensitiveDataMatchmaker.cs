// ROGUES
// SERVER
using System.Collections.Generic;
using UnityEngine.Networking;

public class TeamSensitiveDataMatchmaker
{
	private static TeamSensitiveDataMatchmaker s_instance;

	private Dictionary<int, ActorTeamSensitiveData> m_actorIndexToFriendlyData;
	private Dictionary<int, ActorTeamSensitiveData> m_actorIndexToHostileData;

	private TeamSensitiveDataMatchmaker()
	{
		m_actorIndexToFriendlyData = new Dictionary<int, ActorTeamSensitiveData>();
		m_actorIndexToHostileData = new Dictionary<int, ActorTeamSensitiveData>();
	}

	public static TeamSensitiveDataMatchmaker Get()
	{
		if (s_instance == null)
		{
			s_instance = new TeamSensitiveDataMatchmaker();
		}
		return s_instance;
	}

	public void SetTeamSensitiveDataForActor(ActorData actor)
	{
		if (NetworkServer.active)
		{
			return;
		}
		
		if (m_actorIndexToFriendlyData.ContainsKey(actor.ActorIndex))
		{
			actor.SetClientFriendlyTeamSensitiveData(m_actorIndexToFriendlyData[actor.ActorIndex]);
			m_actorIndexToFriendlyData.Remove(actor.ActorIndex);
		}
		if (m_actorIndexToHostileData.ContainsKey(actor.ActorIndex))
		{
			// rogues
			// Debug.LogError("Trying to handle hostile team sensitive data, should have been removed");
			actor.SetClientHostileTeamSensitiveData(m_actorIndexToHostileData[actor.ActorIndex]);  // removed in rogues
			m_actorIndexToHostileData.Remove(actor.ActorIndex);
		}
	}

	public void SetTeamSensitiveDataForUnhandledActors()
	{
		if (NetworkServer.active)
		{
			return;
		}
		if (GameFlowData.Get() == null)
		{
			return;
		}
		if (m_actorIndexToFriendlyData != null && m_actorIndexToFriendlyData.Count > 0
		    || m_actorIndexToHostileData != null && m_actorIndexToHostileData.Count > 0)
		{
			foreach (ActorData actor in GameFlowData.Get().GetActors())
			{
				SetTeamSensitiveDataForActor(actor);
			}
		}
	}

	public void OnTeamSensitiveDataStarted(ActorTeamSensitiveData teamSensitiveData)
	{
		if (NetworkServer.active)
		{
			return;
		}
		
		Dictionary<int, ActorTeamSensitiveData> dictionary;
		switch (teamSensitiveData.m_typeObservingMe)
		{
			case ActorTeamSensitiveData.ObservedBy.Friendlies:
				dictionary = m_actorIndexToFriendlyData;
				break;
			case ActorTeamSensitiveData.ObservedBy.Hostiles:  // removed in rogues
				dictionary = m_actorIndexToHostileData;
				break;
			default:
				dictionary = null;
				break;
		}
		if (dictionary != null && !dictionary.ContainsKey(teamSensitiveData.ActorIndex))
		{
			dictionary.Add(teamSensitiveData.ActorIndex, teamSensitiveData);
		}
	}
}
