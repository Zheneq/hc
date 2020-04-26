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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		if (m_actorIndexToFriendlyData.ContainsKey(actor.ActorIndex))
		{
			actor.SetClientFriendlyTeamSensitiveData(m_actorIndexToFriendlyData[actor.ActorIndex]);
			m_actorIndexToFriendlyData.Remove(actor.ActorIndex);
		}
		if (!m_actorIndexToHostileData.ContainsKey(actor.ActorIndex))
		{
			return;
		}
		while (true)
		{
			actor.SetClientHostileTeamSensitiveData(m_actorIndexToHostileData[actor.ActorIndex]);
			m_actorIndexToHostileData.Remove(actor.ActorIndex);
			return;
		}
	}

	public void SetTeamSensitiveDataForUnhandledActors()
	{
		if (NetworkServer.active)
		{
			return;
		}
		while (true)
		{
			int num;
			if (m_actorIndexToFriendlyData != null)
			{
				if (m_actorIndexToFriendlyData.Count > 0)
				{
					num = 1;
					goto IL_006f;
				}
			}
			if (m_actorIndexToHostileData != null)
			{
				num = ((m_actorIndexToHostileData.Count > 0) ? 1 : 0);
			}
			else
			{
				num = 0;
			}
			goto IL_006f;
			IL_006f:
			bool flag = (byte)num != 0;
			if (!(GameFlowData.Get() != null))
			{
				return;
			}
			while (true)
			{
				if (flag)
				{
					List<ActorData> actors = GameFlowData.Get().GetActors();
					using (List<ActorData>.Enumerator enumerator = actors.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							ActorData current = enumerator.Current;
							SetTeamSensitiveDataForActor(current);
						}
						while (true)
						{
							switch (4)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
				}
				return;
			}
		}
	}

	public void OnTeamSensitiveDataStarted(ActorTeamSensitiveData teamSensitiveData)
	{
		if (NetworkServer.active)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					return;
				}
			}
		}
		Dictionary<int, ActorTeamSensitiveData> dictionary;
		if (teamSensitiveData.m_typeObservingMe == ActorTeamSensitiveData.ObservedBy.Friendlies)
		{
			dictionary = m_actorIndexToFriendlyData;
		}
		else if (teamSensitiveData.m_typeObservingMe == ActorTeamSensitiveData.ObservedBy.Hostiles)
		{
			dictionary = m_actorIndexToHostileData;
		}
		else
		{
			dictionary = null;
		}
		if (dictionary == null || dictionary.ContainsKey(teamSensitiveData.ActorIndex))
		{
			return;
		}
		while (true)
		{
			dictionary.Add(teamSensitiveData.ActorIndex, teamSensitiveData);
			return;
		}
	}
}
