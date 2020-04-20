using System;
using System.Collections.Generic;
using UnityEngine.Networking;

public class TeamSensitiveDataMatchmaker
{
	private static TeamSensitiveDataMatchmaker s_instance;

	private Dictionary<int, ActorTeamSensitiveData> m_actorIndexToFriendlyData;

	private Dictionary<int, ActorTeamSensitiveData> m_actorIndexToHostileData;

	private TeamSensitiveDataMatchmaker()
	{
		this.m_actorIndexToFriendlyData = new Dictionary<int, ActorTeamSensitiveData>();
		this.m_actorIndexToHostileData = new Dictionary<int, ActorTeamSensitiveData>();
	}

	public static TeamSensitiveDataMatchmaker Get()
	{
		if (TeamSensitiveDataMatchmaker.s_instance == null)
		{
			TeamSensitiveDataMatchmaker.s_instance = new TeamSensitiveDataMatchmaker();
		}
		return TeamSensitiveDataMatchmaker.s_instance;
	}

	public void SetTeamSensitiveDataForActor(ActorData actor)
	{
		if (NetworkServer.active)
		{
			return;
		}
		if (this.m_actorIndexToFriendlyData.ContainsKey(actor.ActorIndex))
		{
			actor.SetClientFriendlyTeamSensitiveData(this.m_actorIndexToFriendlyData[actor.ActorIndex]);
			this.m_actorIndexToFriendlyData.Remove(actor.ActorIndex);
		}
		if (this.m_actorIndexToHostileData.ContainsKey(actor.ActorIndex))
		{
			actor.SetClientHostileTeamSensitiveData(this.m_actorIndexToHostileData[actor.ActorIndex]);
			this.m_actorIndexToHostileData.Remove(actor.ActorIndex);
		}
	}

	public void SetTeamSensitiveDataForUnhandledActors()
	{
		if (!NetworkServer.active)
		{
			bool flag;
			if (this.m_actorIndexToFriendlyData != null)
			{
				if (this.m_actorIndexToFriendlyData.Count > 0)
				{
					flag = true;
					goto IL_6F;
				}
			}
			if (this.m_actorIndexToHostileData != null)
			{
				flag = (this.m_actorIndexToHostileData.Count > 0);
			}
			else
			{
				flag = false;
			}
			IL_6F:
			bool flag2 = flag;
			if (GameFlowData.Get() != null)
			{
				if (flag2)
				{
					List<ActorData> actors = GameFlowData.Get().GetActors();
					using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData teamSensitiveDataForActor = enumerator.Current;
							this.SetTeamSensitiveDataForActor(teamSensitiveDataForActor);
						}
					}
				}
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
		if (teamSensitiveData.m_typeObservingMe == ActorTeamSensitiveData.ObservedBy.Friendlies)
		{
			dictionary = this.m_actorIndexToFriendlyData;
		}
		else if (teamSensitiveData.m_typeObservingMe == ActorTeamSensitiveData.ObservedBy.Hostiles)
		{
			dictionary = this.m_actorIndexToHostileData;
		}
		else
		{
			dictionary = null;
		}
		if (dictionary != null && !dictionary.ContainsKey(teamSensitiveData.ActorIndex))
		{
			dictionary.Add(teamSensitiveData.ActorIndex, teamSensitiveData);
		}
	}
}
