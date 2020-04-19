using System;
using System.Collections.Generic;

[Serializable]
public class Faction
{
	public int FactionGroupIDToUse;

	public int RewardLootTableID;

	public List<FactionTier> Tiers;

	public PlayerFactionProgressInfo[] FactionPlayerProgressInfo;

	public List<int> RibbonIds;

	public List<int> BannerIds;

	public List<FactionStyleBonus> FactionStyleBonuses;

	public List<FactionContributionsData> Contributions;

	public bool DisplayPersonalContribution;

	public static string GetDisplayName(int factionCompletionId, int factionId)
	{
		return StringUtil.TR_FactionName(factionCompletionId, factionId);
	}

	public static string GetLongName(int factionCompletionId, int factionId)
	{
		return StringUtil.TR_FactionLongName(factionCompletionId, factionId);
	}

	public static string GetLoreDescription(int factionCompletionId, int factionId)
	{
		return StringUtil.TR_FactionLoreDescription(factionCompletionId, factionId);
	}
}
