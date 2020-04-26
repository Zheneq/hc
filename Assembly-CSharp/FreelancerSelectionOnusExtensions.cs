using System;

public static class FreelancerSelectionOnusExtensions
{
	public static QuestTriggerType ToGameCompletionQuestTriggerType(this FreelancerSelectionOnus onus)
	{
		switch (onus)
		{
		case FreelancerSelectionOnus.WillFill:
			return QuestTriggerType.WillFillCompletedGame;
		case FreelancerSelectionOnus.FreelancerCollision:
			return QuestTriggerType.FreelancerConflictCompletedGame;
		case FreelancerSelectionOnus.Assigned:
			return QuestTriggerType.AssignedFreelancerCompletedGame;
		default:
			throw new Exception($"No game completion trigger associated with FreelancerSelectionOnus.{onus}");
		}
	}
}
