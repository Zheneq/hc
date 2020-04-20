using System;
using System.Collections.Generic;
using UnityEngine;

public class MinionManager : MonoBehaviour
{
	private static MinionManager s_instance;

	public MinionManager.MinionTheshold[] m_minionPrefabs;

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
		return MinionManager.s_instance;
	}

	private void Awake()
	{
		MinionManager.s_instance = this;
		this.m_allMinions = new List<ActorData>();
		this.m_teamAMinions = new List<ActorData>();
		this.m_teamBMinions = new List<ActorData>();
		this.m_waveSpawnFrequency = Mathf.Max(1, this.m_waveSpawnFrequency);
	}

	private void OnDestroy()
	{
		MinionManager.s_instance = null;
	}

	public List<ActorData> GetMinions()
	{
		return this.m_allMinions;
	}

	public List<ActorData> GetMinionsForTeam(Team minionTeam)
	{
		List<ActorData> result = null;
		if (minionTeam == Team.TeamA)
		{
			result = this.m_teamAMinions;
		}
		else if (minionTeam == Team.TeamB)
		{
			result = this.m_teamBMinions;
		}
		else
		{
			Log.Error(string.Format("Trying to access minions of an invalid team '{0}'.", minionTeam.ToString()), new object[0]);
		}
		return result;
	}

	public void AddMinion(ActorData minionActor)
	{
		if (minionActor == null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(MinionManager.AddMinion(ActorData)).MethodHandle;
			}
			Log.Error("MinionManager trying to add a null actor.", new object[0]);
			return;
		}
		if (!this.m_allMinions.Contains(minionActor))
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
			this.m_allMinions.Add(minionActor);
		}
		else
		{
			Log.Error("MinionManager being told to add already-known minion.", new object[0]);
		}
		if (minionActor.GetTeam() == Team.TeamA)
		{
			if (!this.m_teamAMinions.Contains(minionActor))
			{
				this.m_teamAMinions.Add(minionActor);
			}
			else
			{
				Log.Error("MinionManager being told to add (to Team A) an already-known (by Team A) minion.", new object[0]);
			}
		}
		else if (minionActor.GetTeam() == Team.TeamB)
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
			if (!this.m_teamBMinions.Contains(minionActor))
			{
				this.m_teamBMinions.Add(minionActor);
			}
			else
			{
				Log.Error("MinionManager being told to add (to Team B) an already-known (by Team B) minion.", new object[0]);
			}
		}
	}

	public void RemoveMinion(ActorData minionActor)
	{
		if (minionActor == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(MinionManager.RemoveMinion(ActorData)).MethodHandle;
			}
			Log.Error("MinionManager trying to remove a null actor.", new object[0]);
			return;
		}
		if (this.m_allMinions.Contains(minionActor))
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
			this.m_allMinions.Remove(minionActor);
		}
		else
		{
			Log.Error("MinionManager being told to remove a missing minion.", new object[0]);
		}
		if (minionActor.GetTeam() == Team.TeamA)
		{
			if (this.m_teamAMinions.Contains(minionActor))
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
				this.m_teamAMinions.Remove(minionActor);
			}
			else
			{
				Log.Error("MinionManager being told to remove (from Team A) a missing (from Team A) minion.", new object[0]);
			}
		}
		else if (minionActor.GetTeam() == Team.TeamB)
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
			if (this.m_teamBMinions.Contains(minionActor))
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
				this.m_teamBMinions.Remove(minionActor);
			}
			else
			{
				Log.Error("MinionManager being told to remove (from Team B) a missing (from Team B) minion.", new object[0]);
			}
		}
	}

	[Serializable]
	public class MinionTheshold
	{
		public Transform m_minionPrefab;

		public int m_turnToStartUsing;
	}
}
