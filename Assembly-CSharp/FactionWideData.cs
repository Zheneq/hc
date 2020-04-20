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
			if (factionGroup.ColorHex.Length == 8)
			{
				for (int i = 0; i < 4; i++)
				{
					array[i] = (float)Convert.ToByte(factionGroup.ColorHex.Substring(i * 2, 2), 0x10) / 255f;
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
				return this.m_factionGroups[i];
			}
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
					return result;
				}
				num++;
			}
		}
		return null;
	}

	public Faction GetFaction(int competitionId, int factionId)
	{
		FactionCompetition factionCompetition = this.GetFactionCompetition(competitionId);
		if (factionCompetition != null)
		{
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
			int num = 0;
			long num2 = 0L;
			foreach (FactionTier factionTier in faction.Tiers)
			{
				num2 += factionTier.ContributionToComplete;
				if (num2 <= factionScore)
				{
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
			factionTier.ContributionToComplete = contributionToComplete;
		}
	}

	public int PlayerFactionExperienceToLevel(int competitionId, int factionID, int currentLevel)
	{
		Faction faction = this.GetFaction(competitionId, factionID);
		if (faction != null)
		{
			if (currentLevel >= 1)
			{
				if (currentLevel <= faction.FactionPlayerProgressInfo.Length)
				{
					return faction.FactionPlayerProgressInfo[currentLevel - 1].ExperienceToNextLevel;
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
			if (factionCompetition.Factions != null)
			{
				int i = 0;
				while (i < factionCompetition.Factions.Count)
				{
					Faction faction = factionCompetition.Factions[i];
					if (faction != null)
					{
						if (faction.RibbonIds == null)
						{
						}
						else
						{
							for (int j = 0; j < faction.RibbonIds.Count; j++)
							{
								if (ribbonId == faction.RibbonIds[j])
								{
									return true;
								}
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
				if (t > Convert.ToDateTime(this.m_factionGroups[i].FilterDisplayStartTime))
				{
					if (t < Convert.ToDateTime(this.m_factionGroups[i].FilterDisplayEndTime))
					{
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
		return -1;
	}
}
