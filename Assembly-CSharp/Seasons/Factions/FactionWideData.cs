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
		if (factionGroup.ColorHex != null && factionGroup.ColorHex.Length == 8)
		{
			for (int i = 0; i < 4; i++)
			{
				array[i] = Convert.ToByte(factionGroup.ColorHex.Substring(i * 2, 2), 16) / 255f;
			}
		}
		return array;
	}

	public FactionGroup GetFactionGroup(int GroupID)
	{
		foreach (FactionGroup group in m_factionGroups)
		{
			if (GroupID == group.FactionGroupID)
			{
				return group;
			}
		}

		return new FactionGroup();
	}

	public FactionCompetition GetFactionCompetition(int index)
	{
		int num = 1;
		foreach (FactionCompetition comp in m_factionCompetitions)
		{
			if (num == index)
			{
				return comp;
			}
			num++;
		}
		return null;
	}

	public Faction GetFaction(int competitionId, int factionId)
	{
		FactionCompetition factionCompetition = GetFactionCompetition(competitionId);
		if (factionCompetition == null)
		{
			return null;
		}
		int num = 0;
		foreach (Faction current in factionCompetition.Factions)
		{
			if (num == factionId)
			{
				return current;
			}
			num++;
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
			int tierId = 0;
			long tierScore = 0L;
			{
				foreach (FactionTier tier in faction.Tiers)
				{
					tierScore += tier.ContributionToComplete;
					if (tierScore <= factionScore)
					{
						tierId++;
					}
				}
				return tierId;
			}
		}
		return 0;
	}

	public void SetCompetitionFactionTierInfo(int competitionId, int factionId, int tierId, long contributionToComplete)
	{
		FactionTier factionTier = GetFactionTier(competitionId, factionId, tierId);
		if (factionTier != null)
		{
			factionTier.ContributionToComplete = contributionToComplete;
		}
	}

	public int PlayerFactionExperienceToLevel(int competitionId, int factionID, int currentLevel)
	{
		Faction faction = GetFaction(competitionId, factionID);
		if (faction != null)
		{
			if (currentLevel >= 1 && currentLevel <= faction.FactionPlayerProgressInfo.Length)
			{
				return faction.FactionPlayerProgressInfo[currentLevel - 1].ExperienceToNextLevel;
			}
			throw new ArgumentException($"Current level {currentLevel} is outside the player faction level range {1}-{faction.FactionPlayerProgressInfo.Length}");
		}
		return -1;
	}

	public bool IsRibbonInCompetition(int ribbonId, int competitionId)
	{
		FactionCompetition factionCompetition = GetFactionCompetition(competitionId);
		if (factionCompetition == null || factionCompetition.Factions == null)
		{
			return false;
		}
		foreach (Faction faction in factionCompetition.Factions)
		{
			if (faction == null || faction.RibbonIds == null)
			{
				continue;
			}
			foreach (int id in faction.RibbonIds)
			{
				if (ribbonId == id)
				{
					return true;
				}
			}
		}
		return false;
	}

	private void Awake()
	{
		s_instance = this;
		if (m_factionCompetitions.Count == 0)
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
		foreach (FactionGroup factionGroup in m_factionGroups)
		{
			if (!factionGroup.FilterDisplayStartTime.IsNullOrEmpty()
			    && !factionGroup.FilterDisplayEndTime.IsNullOrEmpty()
			    && t > Convert.ToDateTime(factionGroup.FilterDisplayStartTime)
			    && t < Convert.ToDateTime(factionGroup.FilterDisplayEndTime))
			{
				list.Add(factionGroup);
			}
		}
		return list;
	}

	public int GetCurrentFactionCompetition()
	{
		DateTime t = ClientGameManager.Get().PacificNow();
		for (int i = 0; i < m_factionCompetitions.Count; i++)
		{
			if (t > Convert.ToDateTime(m_factionCompetitions[i].StartTime)
			    && t < Convert.ToDateTime(m_factionCompetitions[i].EndTime))
			{
				return i + 1;
			}
		}
		return -1;
	}
}
