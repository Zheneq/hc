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
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return 4;
				}
			}
		}
		if (slot == TopParticipantSlot.Deadliest)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return 3;
				}
			}
		}
		switch (slot)
		{
		case TopParticipantSlot.Supportiest:
			return 2;
		case TopParticipantSlot.Tankiest:
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				return 1;
			}
		default:
			return 0;
		}
	}
}
