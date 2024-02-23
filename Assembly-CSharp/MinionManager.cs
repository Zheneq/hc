using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MinionManager : MonoBehaviour
{
	[Serializable]
	public class MinionTheshold
	{
		public Transform m_minionPrefab;

		public int m_turnToStartUsing;
	}

	private static MinionManager s_instance;

	public MinionTheshold[] m_minionPrefabs;

	public int m_minionsPerWave = 1;

	public TeamLanes m_lanesTeamA;

	public TeamLanes m_lanesTeamB;

	public int m_waveSpawnFrequency = 5;

	public int m_firstSpawnTurn = 2;

	private List<ActorData> m_allMinions;

	private List<ActorData> m_teamAMinions;

	private List<ActorData> m_teamBMinions;

	public static MinionManager Get()
	{
		return s_instance;
	}

	private void Awake()
	{
		s_instance = this;
		m_allMinions = new List<ActorData>();
		m_teamAMinions = new List<ActorData>();
		m_teamBMinions = new List<ActorData>();
		m_waveSpawnFrequency = Mathf.Max(1, m_waveSpawnFrequency);
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public List<ActorData> GetMinions()
	{
		return m_allMinions;
	}

	public List<ActorData> GetMinionsForTeam(Team minionTeam)
	{
		List<ActorData> result = null;
		switch (minionTeam)
		{
		case Team.TeamA:
			result = m_teamAMinions;
			break;
		case Team.TeamB:
			result = m_teamBMinions;
			break;
		default:
			Log.Error(new StringBuilder().Append("Trying to access minions of an invalid team '").Append(minionTeam.ToString()).Append("'.").ToString());
			break;
		}
		return result;
	}

	public void AddMinion(ActorData minionActor)
	{
		if (minionActor == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
					Log.Error("MinionManager trying to add a null actor.");
					return;
				}
			}
		}
		if (!m_allMinions.Contains(minionActor))
		{
			m_allMinions.Add(minionActor);
		}
		else
		{
			Log.Error("MinionManager being told to add already-known minion.");
		}
		if (minionActor.GetTeam() == Team.TeamA)
		{
			if (!m_teamAMinions.Contains(minionActor))
			{
				m_teamAMinions.Add(minionActor);
			}
			else
			{
				Log.Error("MinionManager being told to add (to Team A) an already-known (by Team A) minion.");
			}
		}
		else
		{
			if (minionActor.GetTeam() != Team.TeamB)
			{
				return;
			}
			while (true)
			{
				if (!m_teamBMinions.Contains(minionActor))
				{
					m_teamBMinions.Add(minionActor);
				}
				else
				{
					Log.Error("MinionManager being told to add (to Team B) an already-known (by Team B) minion.");
				}
				return;
			}
		}
	}

	public void RemoveMinion(ActorData minionActor)
	{
		if (minionActor == null)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					break;
				default:
					Log.Error("MinionManager trying to remove a null actor.");
					return;
				}
			}
		}
		if (m_allMinions.Contains(minionActor))
		{
			m_allMinions.Remove(minionActor);
		}
		else
		{
			Log.Error("MinionManager being told to remove a missing minion.");
		}
		if (minionActor.GetTeam() == Team.TeamA)
		{
			if (m_teamAMinions.Contains(minionActor))
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						break;
					default:
						m_teamAMinions.Remove(minionActor);
						return;
					}
				}
			}
			Log.Error("MinionManager being told to remove (from Team A) a missing (from Team A) minion.");
		}
		else
		{
			if (minionActor.GetTeam() != Team.TeamB)
			{
				return;
			}
			while (true)
			{
				if (m_teamBMinions.Contains(minionActor))
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							m_teamBMinions.Remove(minionActor);
							return;
						}
					}
				}
				Log.Error("MinionManager being told to remove (from Team B) a missing (from Team B) minion.");
				return;
			}
		}
	}
}
