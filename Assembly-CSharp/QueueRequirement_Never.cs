using Newtonsoft.Json;
using System;
using System.Text;

[Serializable]
public class QueueRequirement_Never : QueueRequirement
{
	private RequirementType m_requirementType = RequirementType.AdminDisabled;

	public override bool AnyGroupMember
	{
		get { return false; }
	}

	public override RequirementType Requirement
	{
		get { return m_requirementType; }
	}

	public static QueueRequirement CreateAdminDisabled()
	{
		QueueRequirement_Never queueRequirement_Never = new QueueRequirement_Never();
		queueRequirement_Never.m_requirementType = RequirementType.AdminDisabled;
		return queueRequirement_Never;
	}

	public override bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		return false;
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context)
	{
		QueueBlockOutReasonDetails Details;
		return GenerateFailure(systemInfo, applicant, context, out Details);
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context, out QueueBlockOutReasonDetails Details)
	{
		Details = new QueueBlockOutReasonDetails();
		RequirementType requirement = Requirement;
		if (requirement == RequirementType.AdminDisabled)
		{
			return LocalizationPayload.Create("DisabledByAdmin", "Matchmaking");
		}
		throw new Exception(new StringBuilder().Append("Unknown requirement is failed: ").Append(Requirement).ToString());
	}

	public override void WriteToJson(JsonWriter writer)
	{
	}

	public static QueueRequirement Create(RequirementType reqType, JsonReader reader)
	{
		QueueRequirement_Never queueRequirement_Never = new QueueRequirement_Never();
		queueRequirement_Never.m_requirementType = reqType;
		return queueRequirement_Never;
	}
}
