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
			if (this.ConditionType == questCondition.ConditionType)
			{
				switch (this.ConditionType)
				{
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
					return this.typeSpecificData == questCondition.typeSpecificData;
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
					return this.typeSpecificData == questCondition.typeSpecificData && this.typeSpecificData2 == questCondition.typeSpecificData2;
				case QuestConditionType.HasDateTimePassed:
					if (this.typeSpecificDate.Count != questCondition.typeSpecificDate.Count)
					{
						return false;
					}
					for (int i = 0; i < this.typeSpecificDate.Count; i++)
					{
						if (this.typeSpecificDate[i] != questCondition.typeSpecificDate[i])
						{
							return false;
						}
					}
					return true;
				case QuestConditionType.HasTitleCountInMatch:
				case QuestConditionType.UsingVisualInfo:
				case QuestConditionType.HasBannerCountInMatch:
				case QuestConditionType.HasUnlockedStyle:
					if (this.typeSpecificData == questCondition.typeSpecificData && this.typeSpecificData2 == questCondition.typeSpecificData2)
					{
						if (this.typeSpecificData3 == questCondition.typeSpecificData3)
						{
							return this.typeSpecificData4 == questCondition.typeSpecificData4;
						}
					}
					return false;
				case QuestConditionType.UsingCommonGameTypes:
				case QuestConditionType.UsingQueuedGameTypes:
					return true;
				case QuestConditionType.FactionTierReached:
				{
					bool result;
					if (this.typeSpecificData == questCondition.typeSpecificData && this.typeSpecificData2 == questCondition.typeSpecificData2)
					{
						result = (this.typeSpecificData3 == questCondition.typeSpecificData3);
					}
					else
					{
						result = false;
					}
					return result;
				}
				}
				return false;
			}
		}
		return false;
	}
}
