using System;
using System.Collections.Generic;

[Serializable]
public class QuestTemplate
{
	public int Index;

	public string DisplayName;

	public string Description;

	public string FlavorText;

	public string TypeDisplayName;

	public string LongDescription;

	public string IconFilename;

	public string UnfinishedIconFilename;

	public string ChallengeIconFileName;

	public bool Enabled;

	public bool LocalizeWhenDisabled;

	public bool OneTimeOnly;

	public string AbandonDateTime;

	public bool CantManuallyAbandon;

	public bool BroadcastFirstCompletions;

	public bool HideCompletion;

	public int AssociatedRibbon;

	public int SortOrder;

	public bool DisplayRewardNotification;

	public QuestPrerequisites Prerequisites;

	public List<QuestObjective> Objectives;

	public int RequiredObjectiveCount;

	public int CosmeticRequiredObjectiveCount;

	public RequiredObjectiveCountType ObjectiveCountType;

	public QuestRewards Rewards;

	public ConditionalQuestRewards[] ConditionalRewards;

	public AchievementType AchievmentType;

	public AchievementRarity AchievementRarity;

	public int AchievementPoints;

	public int AchievementPrevious;

	public void ResetToDefaultValuesForNewEntry(bool resetIndex)
	{
		if (resetIndex)
		{
			Index = 0;
		}
		DisplayName = string.Empty;
		Description = string.Empty;
		FlavorText = string.Empty;
		TypeDisplayName = string.Empty;
		LongDescription = string.Empty;
		IconFilename = string.Empty;
		ChallengeIconFileName = string.Empty;
		Enabled = false;
		LocalizeWhenDisabled = false;
		OneTimeOnly = false;
		AbandonDateTime = string.Empty;
		CantManuallyAbandon = false;
		BroadcastFirstCompletions = false;
		HideCompletion = false;
		AssociatedRibbon = 0;
		DisplayRewardNotification = false;
		Prerequisites = new QuestPrerequisites();
		Objectives = new List<QuestObjective>();
		RequiredObjectiveCount = 0;
		CosmeticRequiredObjectiveCount = 0;
		ObjectiveCountType = RequiredObjectiveCountType.SumCompletedObjectivesOnly;
		Rewards = new QuestRewards();
		ConditionalRewards = new ConditionalQuestRewards[0];
		AchievmentType = AchievementType.None;
		AchievementRarity = AchievementRarity.Common;
		AchievementPoints = 0;
		AchievementPrevious = 0;
	}
}
