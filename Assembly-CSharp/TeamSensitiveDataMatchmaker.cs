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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		if (m_actorIndexToFriendlyData.ContainsKey(actor.ActorIndex))
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			actor.SetClientFriendlyTeamSensitiveData(m_actorIndexToFriendlyData[actor.ActorIndex]);
			m_actorIndexToFriendlyData.Remove(actor.ActorIndex);
		}
		if (!m_actorIndexToHostileData.ContainsKey(actor.ActorIndex))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
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
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			int num;
			if (m_actorIndexToFriendlyData != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (m_actorIndexToFriendlyData.Count > 0)
				{
					num = 1;
					goto IL_006f;
				}
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (m_actorIndexToHostileData != null)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
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
				switch (3)
				{
				case 0:
					continue;
				}
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
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return;
				}
			}
		}
		Dictionary<int, ActorTeamSensitiveData> dictionary;
		if (teamSensitiveData.m_typeObservingMe == ActorTeamSensitiveData.ObservedBy.Friendlies)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			dictionary = m_actorIndexToFriendlyData;
		}
		else if (teamSensitiveData.m_typeObservingMe == ActorTeamSensitiveData.ObservedBy.Hostiles)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
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
			switch (5)
			{
			case 0:
				continue;
			}
			dictionary.Add(teamSensitiveData.ActorIndex, teamSensitiveData);
			return;
		}
	}
}
