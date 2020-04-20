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
			this.Index = 0;
		}
		this.DisplayName = string.Empty;
		this.Description = string.Empty;
		this.FlavorText = string.Empty;
		this.TypeDisplayName = string.Empty;
		this.LongDescription = string.Empty;
		this.IconFilename = string.Empty;
		this.ChallengeIconFileName = string.Empty;
		this.Enabled = false;
		this.LocalizeWhenDisabled = false;
		this.OneTimeOnly = false;
		this.AbandonDateTime = string.Empty;
		this.CantManuallyAbandon = false;
		this.BroadcastFirstCompletions = false;
		this.HideCompletion = false;
		this.AssociatedRibbon = 0;
		this.DisplayRewardNotification = false;
		this.Prerequisites = new QuestPrerequisites();
		this.Objectives = new List<QuestObjective>();
		this.RequiredObjectiveCount = 0;
		this.CosmeticRequiredObjectiveCount = 0;
		this.ObjectiveCountType = RequiredObjectiveCountType.SumCompletedObjectivesOnly;
		this.Rewards = new QuestRewards();
		this.ConditionalRewards = new ConditionalQuestRewards[0];
		this.AchievmentType = AchievementType.None;
		this.AchievementRarity = AchievementRarity.Common;
		this.AchievementPoints = 0;
		this.AchievementPrevious = 0;
	}
}
