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
		switch (slot)
		{
			case TopParticipantSlot.MostDecorated:
				return 4;
			case TopParticipantSlot.Deadliest:
				return 3;
			case TopParticipantSlot.Supportiest:
				return 2;
			case TopParticipantSlot.Tankiest:
				return 1;
			default:
				return 0;
		}
	}
}
