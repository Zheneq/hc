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
		return s_instance;
	}

	public float[] GetRBGA(Faction faction)
	{
		float[] array = new float[4];
		FactionGroup factionGroup = Get().GetFactionGroup(faction.FactionGroupIDToUse);
		if (factionGroup.ColorHex != null)
		{
			if (factionGroup.ColorHex.Length == 8)
			{
				for (int i = 0; i < 4; i++)
				{
					array[i] = (float)(int)Convert.ToByte(factionGroup.ColorHex.Substring(i * 2, 2), 16) / 255f;
				}
				while (true)
				{
					return array;
				}
			}
		}
		return array;
	}

	public FactionGroup GetFactionGroup(int GroupID)
	{
		for (int i = 0; i < m_factionGroups.Count; i++)
		{
			if (GroupID != m_factionGroups[i].FactionGroupID)
			{
				continue;
			}
			while (true)
			{
				return m_factionGroups[i];
			}
		}
		while (true)
		{
			return new FactionGroup();
		}
	}

	public FactionCompetition GetFactionCompetition(int index)
	{
		int num = 1;
		using (List<FactionCompetition>.Enumerator enumerator = m_factionCompetitions.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				FactionCompetition current = enumerator.Current;
				if (num == index)
				{
					while (true)
					{
						switch (6)
						{
						case 0:
							break;
						default:
							return current;
						}
					}
				}
				num++;
			}
		}
		return null;
	}

	public Faction GetFaction(int competitionId, int factionId)
	{
		FactionCompetition factionCompetition = GetFactionCompetition(competitionId);
		if (factionCompetition != null)
		{
			int num = 0;
			using (List<Faction>.Enumerator enumerator = factionCompetition.Factions.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					Faction current = enumerator.Current;
					if (num == factionId)
					{
						return current;
					}
					num++;
				}
			}
		}
		return null;
	}

	public FactionTier GetFactionTier(int competitionId, int factionId, int tierId)
	{
		return GetFaction(competitionId, factionId)?.Tiers.ElementAt(tierId);
	}

	public int GetCompetitionFactionTierReached(int competitionId, int factionId, long factionScore)
	{
		Faction faction = GetFaction(competitionId, factionId);
		if (faction != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					int num = 0;
					long num2 = 0L;
					{
						foreach (FactionTier tier in faction.Tiers)
						{
							num2 += tier.ContributionToComplete;
							if (num2 <= factionScore)
							{
								num++;
							}
						}
						return num;
					}
				}
				}
			}
		}
		return 0;
	}

	public void SetCompetitionFactionTierInfo(int competitionId, int factionId, int tierId, long contributionToComplete)
	{
		FactionTier factionTier = GetFactionTier(competitionId, factionId, tierId);
		if (factionTier == null)
		{
			return;
		}
		while (true)
		{
			factionTier.ContributionToComplete = contributionToComplete;
			return;
		}
	}

	public int PlayerFactionExperienceToLevel(int competitionId, int factionID, int currentLevel)
	{
		Faction faction = GetFaction(competitionId, factionID);
		if (faction != null)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					if (currentLevel >= 1)
					{
						if (currentLevel <= faction.FactionPlayerProgressInfo.Length)
						{
							return faction.FactionPlayerProgressInfo[currentLevel - 1].ExperienceToNextLevel;
						}
					}
					throw new ArgumentException($"Current level {currentLevel} is outside the player faction level range {1}-{faction.FactionPlayerProgressInfo.Length}");
				}
			}
		}
		return -1;
	}

	public bool IsRibbonInCompetition(int ribbonId, int competitionId)
	{
		FactionCompetition factionCompetition = GetFactionCompetition(competitionId);
		if (factionCompetition != null)
		{
			if (factionCompetition.Factions != null)
			{
				for (int i = 0; i < factionCompetition.Factions.Count; i++)
				{
					Faction faction = factionCompetition.Factions[i];
					if (faction == null)
					{
						continue;
					}
					if (faction.RibbonIds == null)
					{
						continue;
					}
					for (int j = 0; j < faction.RibbonIds.Count; j++)
					{
						if (ribbonId != faction.RibbonIds[j])
						{
							continue;
						}
						while (true)
						{
							return true;
						}
					}
				}
			}
		}
		return false;
	}

	private void Awake()
	{
		s_instance = this;
		if (m_factionCompetitions.Count != 0)
		{
			return;
		}
		while (true)
		{
			throw new Exception("FactionWideData failed to load");
		}
	}

	private void OnDestroy()
	{
		s_instance = null;
	}

	public List<FactionGroup> FactionGroupsToDisplayFilter()
	{
		List<FactionGroup> list = new List<FactionGroup>();
		DateTime t = ClientGameManager.Get().PacificNow();
		for (int i = 0; i < m_factionGroups.Count; i++)
		{
			if (m_factionGroups[i].FilterDisplayStartTime.IsNullOrEmpty() || m_factionGroups[i].FilterDisplayEndTime.IsNullOrEmpty())
			{
				continue;
			}
			if (!(t > Convert.ToDateTime(m_factionGroups[i].FilterDisplayStartTime)))
			{
				continue;
			}
			if (t < Convert.ToDateTime(m_factionGroups[i].FilterDisplayEndTime))
			{
				list.Add(m_factionGroups[i]);
			}
		}
		return list;
	}

	public int GetCurrentFactionCompetition()
	{
		DateTime t = ClientGameManager.Get().PacificNow();
		for (int i = 0; i < m_factionCompetitions.Count; i++)
		{
			if (t > Convert.ToDateTime(m_factionCompetitions[i].StartTime) && t < Convert.ToDateTime(m_factionCompetitions[i].EndTime))
			{
				return i + 1;
			}
		}
		while (true)
		{
			return -1;
		}
	}
}
