using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FactionWideData : MonoBehaviour
{
	private static FactionWideData s_instance;

	[Header("Faction Groups")]
	public List<FactionGroup> m_factionGroups;

	[Header("Faction Competitions")]
	public List<FactionCompetition> m_factionCompetitions;

	public static FactionWideData Get()
	{
		return FactionWideData.s_instance;
	}

	public float[] GetRBGA(Faction faction)
	{
		float[] array = new float[4];
		FactionGroup factionGroup = FactionWideData.Get().GetFactionGroup(faction.FactionGroupIDToUse);
		if (factionGroup.ColorHex != null)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FactionWideData.GetRBGA(Faction)).MethodHandle;
			}
			if (factionGroup.ColorHex.Length == 8)
			{
				for (int i = 0; i < 4; i++)
				{
					array[i] = (float)Convert.ToByte(factionGroup.ColorHex.Substring(i * 2, 2), 0x10) / 255f;
				}
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				return array;
			}
		}
		return array;
	}

	public FactionGroup GetFactionGroup(int GroupID)
	{
		for (int i = 0; i < this.m_factionGroups.Count; i++)
		{
			if (GroupID == this.m_factionGroups[i].FactionGroupID)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(FactionWideData.GetFactionGroup(int)).MethodHandle;
				}
				return this.m_factionGroups[i];
			}
		}
		for (;;)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			break;
		}
		return new FactionGroup();
	}

	public FactionCompetition GetFactionCompetition(int index)
	{
		int num = 1;
		using (List<FactionCompetition>.Enumerator enumerator = this.m_factionCompetitions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				FactionCompetition result = enumerator.Current;
				if (num == index)
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
					if (!true)
					{
						RuntimeMethodHandle runtimeMethodHandle = methodof(FactionWideData.GetFactionCompetition(int)).MethodHandle;
					}
					return result;
				}
				num++;
			}
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		return null;
	}

	public Faction GetFaction(int competitionId, int factionId)
	{
		FactionCompetition factionCompetition = this.GetFactionCompetition(competitionId);
		if (factionCompetition != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FactionWideData.GetFaction(int, int)).MethodHandle;
			}
			int num = 0;
			using (List<Faction>.Enumerator enumerator = factionCompetition.Factions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Faction result = enumerator.Current;
					if (num == factionId)
					{
						return result;
					}
					num++;
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		return null;
	}

	public FactionTier GetFactionTier(int competitionId, int factionId, int tierId)
	{
		Faction faction = this.GetFaction(competitionId, factionId);
		if (faction != null)
		{
			return faction.Tiers.ElementAt(tierId);
		}
		return null;
	}

	public int GetCompetitionFactionTierReached(int competitionId, int factionId, long factionScore)
	{
		Faction faction = this.GetFaction(competitionId, factionId);
		if (faction != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FactionWideData.GetCompetitionFactionTierReached(int, int, long)).MethodHandle;
			}
			int num = 0;
			long num2 = 0L;
			foreach (FactionTier factionTier in faction.Tiers)
			{
				num2 += factionTier.ContributionToComplete;
				if (num2 <= factionScore)
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
					num++;
				}
			}
			return num;
		}
		return 0;
	}

	public void SetCompetitionFactionTierInfo(int competitionId, int factionId, int tierId, long contributionToComplete)
	{
		FactionTier factionTier = this.GetFactionTier(competitionId, factionId, tierId);
		if (factionTier != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(FactionWideData.SetCompetitionFactionTierInfo(int, int, int, long)).MethodHandle;
			}
			factionTier.ContributionToComplete = contributionToComplete;
		}
	}

	public int PlayerFactionExperienceToLevel(int competitionId, int factionID, int currentLevel)
	{
		Faction faction = this.GetFaction(competitionId, factionID);
		if (faction != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FactionWideData.PlayerFactionExperienceToLevel(int, int, int)).MethodHandle;
			}
			if (currentLevel >= 1)
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
				if (currentLevel <= faction.FactionPlayerProgressInfo.Length)
				{
					return faction.FactionPlayerProgressInfo[currentLevel - 1].ExperienceToNextLevel;
				}
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			throw new ArgumentException(string.Format("Current level {0} is outside the player faction level range {1}-{2}", currentLevel, 1, faction.FactionPlayerProgressInfo.Length));
		}
		return -1;
	}

	public bool IsRibbonInCompetition(int ribbonId, int competitionId)
	{
		FactionCompetition factionCompetition = this.GetFactionCompetition(competitionId);
		if (factionCompetition != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FactionWideData.IsRibbonInCompetition(int, int)).MethodHandle;
			}
			if (factionCompetition.Factions != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				int i = 0;
				while (i < factionCompetition.Factions.Count)
				{
					Faction faction = factionCompetition.Factions[i];
					if (faction != null)
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
						if (faction.RibbonIds == null)
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
						}
						else
						{
							for (int j = 0; j < faction.RibbonIds.Count; j++)
							{
								if (ribbonId == faction.RibbonIds[j])
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
									return true;
								}
							}
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
						}
					}
					IL_AC:
					i++;
					continue;
					goto IL_AC;
				}
			}
		}
		return false;
	}

	private void Awake()
	{
		FactionWideData.s_instance = this;
		if (this.m_factionCompetitions.Count == 0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(FactionWideData.Awake()).MethodHandle;
			}
			throw new Exception("FactionWideData failed to load");
		}
	}

	private void OnDestroy()
	{
		FactionWideData.s_instance = null;
	}

	public List<FactionGroup> FactionGroupsToDisplayFilter()
	{
		List<FactionGroup> list = new List<FactionGroup>();
		DateTime t = ClientGameManager.Get().PacificNow();
		for (int i = 0; i < this.m_factionGroups.Count; i++)
		{
			if (!this.m_factionGroups[i].FilterDisplayStartTime.IsNullOrEmpty() && !this.m_factionGroups[i].FilterDisplayEndTime.IsNullOrEmpty())
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(FactionWideData.FactionGroupsToDisplayFilter()).MethodHandle;
				}
				if (t > Convert.ToDateTime(this.m_factionGroups[i].FilterDisplayStartTime))
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
					if (t < Convert.ToDateTime(this.m_factionGroups[i].FilterDisplayEndTime))
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
						list.Add(this.m_factionGroups[i]);
					}
				}
			}
		}
		return list;
	}

	public int GetCurrentFactionCompetition()
	{
		DateTime t = ClientGameManager.Get().PacificNow();
		for (int i = 0; i < this.m_factionCompetitions.Count; i++)
		{
			if (t > Convert.ToDateTime(this.m_factionCompetitions[i].StartTime) && t < Convert.ToDateTime(this.m_factionCompetitions[i].EndTime))
			{
				return i + 1;
			}
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
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(FactionWideData.GetCurrentFactionCompetition()).MethodHandle;
		}
		return -1;
	}
}
