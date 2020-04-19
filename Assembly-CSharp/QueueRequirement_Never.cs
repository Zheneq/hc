using System;
using Newtonsoft.Json;

[Serializable]
public class QueueRequirement_Never : QueueRequirement
{
	private QueueRequirement.RequirementType m_requirementType = QueueRequirement.RequirementType.AdminDisabled;

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
			return this.m_requirementType;
		}
	}

	public static QueueRequirement CreateAdminDisabled()
	{
		return new QueueRequirement_Never
		{
			m_requirementType = QueueRequirement.RequirementType.AdminDisabled
		};
	}

	public override bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		return false;
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context)
	{
		QueueBlockOutReasonDetails queueBlockOutReasonDetails;
		return this.GenerateFailure(systemInfo, applicant, context, out queueBlockOutReasonDetails);
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context, out QueueBlockOutReasonDetails Details)
	{
		Details = new QueueBlockOutReasonDetails();
		QueueRequirement.RequirementType requirement = this.Requirement;
		if (requirement != QueueRequirement.RequirementType.AdminDisabled)
		{
			throw new Exception(string.Format("Unknown requirement is failed: {0}", this.Requirement));
		}
		return LocalizationPayload.Create("DisabledByAdmin", "Matchmaking");
	}

	public override void WriteToJson(JsonWriter writer)
	{
	}

	public static QueueRequirement Create(QueueRequirement.RequirementType reqType, JsonReader reader)
	{
		return new QueueRequirement_Never
		{
			m_requirementType = reqType
		};
	}
}
