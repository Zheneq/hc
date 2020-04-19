using System;

public class QueueBlockOutReasonDetails
{
	public QueueRequirement.RequirementType? RequirementTypeNotMet;

	public RequirementMessageContext? Context;

	public int? NumGamesRequired;

	public int? NumGamesPlayed;

	public bool? CausedBySelf;
}
