using System;
using System.Collections.Generic;

[Serializable]
public class BadgeAndParticipantInfo
{
	public int PlayerId;

	public Team TeamId;

	public int TeamSlot;

	public List<BadgeInfo> BadgesEarned;

	public List<TopParticipantSlot> TopParticipationEarned;

	public Dictionary<StatDisplaySettings.StatType, PercentileInfo> GlobalPercentiles;

	public Dictionary<int, PercentileInfo> FreelancerSpecificPercentiles;

	public CharacterType FreelancerPlayed;

	public static int ParticipantOrderDisplayPriority(TopParticipantSlot slot)
	{
		if (slot == TopParticipantSlot.MostDecorated)
		{
			return 4;
		}
		if (slot == TopParticipantSlot.Deadliest)
		{
			return 3;
		}
		if (slot == TopParticipantSlot.Supportiest)
		{
			return 2;
		}
		if (slot == TopParticipantSlot.Tankiest)
		{
			return 1;
		}
		return 0;
	}
}
