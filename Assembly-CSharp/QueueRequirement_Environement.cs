using System;
using Newtonsoft.Json;

[Serializable]
public class QueueRequirement_Environement : QueueRequirement
{
	private EnvironmentType Environment;

	public override bool AnyGroupMember
	{
		get
		{
			return false;
		}
	}

	public override QueueRequirement.RequirementType Requirement
	{
		get
		{
			return QueueRequirement.RequirementType.ProhibitEnvironment;
		}
	}

	public override bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		return systemInfo.GetEnvironmentType() != this.Environment;
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context)
	{
		QueueBlockOutReasonDetails queueBlockOutReasonDetails;
		return this.GenerateFailure(systemInfo, applicant, context, out queueBlockOutReasonDetails);
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context, out QueueBlockOutReasonDetails Details)
	{
		Details = new QueueBlockOutReasonDetails();
		return LocalizationPayload.Create("NotPermittedOnThisEnvironment", "Matchmaking");
	}

	public override void WriteToJson(JsonWriter writer)
	{
		writer.WritePropertyName("EnvironmentType");
		writer.WriteValue(this.Environment.ToString());
	}

	public static QueueRequirement Create(JsonReader reader)
	{
		QueueRequirement_Environement queueRequirement_Environement = new QueueRequirement_Environement();
		reader.Read();
		string value = reader.Value.ToString();
		queueRequirement_Environement.Environment = (EnvironmentType)Enum.Parse(typeof(EnvironmentType), value, true);
		reader.Read();
		return queueRequirement_Environement;
	}
}
