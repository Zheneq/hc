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
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TeamSensitiveDataMatchmaker.Get()).MethodHandle;
			}
			TeamSensitiveDataMatchmaker.s_instance = new TeamSensitiveDataMatchmaker();
		}
		return TeamSensitiveDataMatchmaker.s_instance;
	}

	public void SetTeamSensitiveDataForActor(ActorData actor)
	{
		if (NetworkServer.active)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TeamSensitiveDataMatchmaker.SetTeamSensitiveDataForActor(ActorData)).MethodHandle;
			}
			return;
		}
		if (this.m_actorIndexToFriendlyData.ContainsKey(actor.ActorIndex))
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			actor.SetClientFriendlyTeamSensitiveData(this.m_actorIndexToFriendlyData[actor.ActorIndex]);
			this.m_actorIndexToFriendlyData.Remove(actor.ActorIndex);
		}
		if (this.m_actorIndexToHostileData.ContainsKey(actor.ActorIndex))
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			actor.SetClientHostileTeamSensitiveData(this.m_actorIndexToHostileData[actor.ActorIndex]);
			this.m_actorIndexToHostileData.Remove(actor.ActorIndex);
		}
	}

	public void SetTeamSensitiveDataForUnhandledActors()
	{
		if (!NetworkServer.active)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TeamSensitiveDataMatchmaker.SetTeamSensitiveDataForUnhandledActors()).MethodHandle;
			}
			bool flag;
			if (this.m_actorIndexToFriendlyData != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				if (this.m_actorIndexToFriendlyData.Count > 0)
				{
					flag = true;
					goto IL_6F;
				}
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (this.m_actorIndexToHostileData != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
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
				for (;;)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					break;
				}
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
						for (;;)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
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
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TeamSensitiveDataMatchmaker.OnTeamSensitiveDataStarted(ActorTeamSensitiveData)).MethodHandle;
			}
			return;
		}
		Dictionary<int, ActorTeamSensitiveData> dictionary;
		if (teamSensitiveData.m_typeObservingMe == ActorTeamSensitiveData.ObservedBy.Friendlies)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			dictionary = this.m_actorIndexToFriendlyData;
		}
		else if (teamSensitiveData.m_typeObservingMe == ActorTeamSensitiveData.ObservedBy.Hostiles)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			dictionary = this.m_actorIndexToHostileData;
		}
		else
		{
			dictionary = null;
		}
		if (dictionary != null && !dictionary.ContainsKey(teamSensitiveData.ActorIndex))
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			dictionary.Add(teamSensitiveData.ActorIndex, teamSensitiveData);
		}
	}
}
