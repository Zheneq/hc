using System;
using System.Collections.Generic;

[Serializable]
public class QuestCondition
{
	public QuestConditionType ConditionType;

	public int typeSpecificData;

	public int typeSpecificData2;

	public int typeSpecificData3;

	public int typeSpecificData4;

	public List<int> typeSpecificDate;

	public string typeSpecificString;

	public override int GetHashCode()
	{
		return base.GetHashCode();
	}

	public override bool Equals(object obj)
	{
		QuestCondition questCondition = obj as QuestCondition;
		if (questCondition != null)
		{
			if (ConditionType == questCondition.ConditionType)
			{
				int result;
				switch (ConditionType)
				{
				case QuestConditionType.UsingCommonGameTypes:
				case QuestConditionType.UsingQueuedGameTypes:
					return true;
				case QuestConditionType.HasCompletedQuest:
				case QuestConditionType.HasUnlockedCharacter:
				case QuestConditionType.UsingCharacter:
				case QuestConditionType.UsingCharacterRole:
				case QuestConditionType.HasFriendCountOnTeam:
				case QuestConditionType.HasFriendCountOrMore:
				case QuestConditionType.UsingGameType:
				case QuestConditionType.HasAccountLevel:
				case QuestConditionType.UsingBanner:
				case QuestConditionType.UsingEmblem:
				case QuestConditionType.UsingTitle:
				case QuestConditionType.UsingItemTemplateId:
				case QuestConditionType.UsingItemType:
				case QuestConditionType.UsingItemRarity:
				case QuestConditionType.UsingCatalyst:
				case QuestConditionType.UsingSkinId:
				case QuestConditionType.UsingPatternId:
				case QuestConditionType.UsingColorId:
				case QuestConditionType.HasSeasonAccess:
				case QuestConditionType.UsingQuest:
				case QuestConditionType.HasQuestBonusLevel:
				case QuestConditionType.HasAbilityNumber:
				case QuestConditionType.DaysSinceLastProgress:
				case QuestConditionType.UsedGG:
				case QuestConditionType.HasEnemyTeamTitle:
				case QuestConditionType.HasEnemyTeamEmblem:
				case QuestConditionType.HasEnemyTeamBanner:
				case QuestConditionType.UsingStyleGroup:
				case QuestConditionType.HasUnlockedTitle:
				case QuestConditionType.HasUnlockedEmblem:
				case QuestConditionType.HasUnlockedBanner:
				case QuestConditionType.IsFactionCompetitionActive:
				case QuestConditionType.IsFreelancerAvailable:
				case QuestConditionType.UsingMap:
				case QuestConditionType.CurrencyType:
				case QuestConditionType.UsingStatBucket:
				case QuestConditionType.QueueGroupSize:
				case QuestConditionType.IsFreelancerPlayable:
				case QuestConditionType.BadgeEarned:
				case QuestConditionType.HasUnlockedOvercon:
				case QuestConditionType.HasUnlockedChatEmoji:
					return typeSpecificData == questCondition.typeSpecificData;
				case QuestConditionType.HasCharacterLevel:
				case QuestConditionType.HasSeasonAndChapterAccess:
				case QuestConditionType.HasSeasonLevel:
				case QuestConditionType.HasCharacterElo:
				case QuestConditionType.UsedAbilityCount:
				case QuestConditionType.UsingCharacterFaction:
				case QuestConditionType.HasFriendCountInMatch:
				case QuestConditionType.UsingFactionBanner:
				case QuestConditionType.UsingFactionRibbon:
				case QuestConditionType.UsedOvercon:
				case QuestConditionType.HasUnlockedTaunt:
					return typeSpecificData == questCondition.typeSpecificData && typeSpecificData2 == questCondition.typeSpecificData2;
				case QuestConditionType.FactionTierReached:
				{
					int result2;
					if (typeSpecificData == questCondition.typeSpecificData && typeSpecificData2 == questCondition.typeSpecificData2)
					{
						result2 = ((typeSpecificData3 == questCondition.typeSpecificData3) ? 1 : 0);
					}
					else
					{
						result2 = 0;
					}
					return (byte)result2 != 0;
				}
				case QuestConditionType.HasTitleCountInMatch:
				case QuestConditionType.UsingVisualInfo:
				case QuestConditionType.HasBannerCountInMatch:
				case QuestConditionType.HasUnlockedStyle:
					if (typeSpecificData == questCondition.typeSpecificData && typeSpecificData2 == questCondition.typeSpecificData2)
					{
						if (typeSpecificData3 == questCondition.typeSpecificData3)
						{
							result = ((typeSpecificData4 == questCondition.typeSpecificData4) ? 1 : 0);
							goto IL_021d;
						}
					}
					result = 0;
					goto IL_021d;
				case QuestConditionType.HasDateTimePassed:
				{
					if (typeSpecificDate.Count != questCondition.typeSpecificDate.Count)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								return false;
							}
						}
					}
					for (int i = 0; i < typeSpecificDate.Count; i++)
					{
						if (typeSpecificDate[i] == questCondition.typeSpecificDate[i])
						{
							continue;
						}
						while (true)
						{
							return false;
						}
					}
					while (true)
					{
						return true;
					}
				}
				default:
					{
						return false;
					}
					IL_021d:
					return (byte)result != 0;
				}
			}
		}
		return false;
	}
}
