using System;
using Newtonsoft.Json;

[Serializable]
public class QueueRequirement_AccessLevel : QueueRequirement
{
	private bool m_anyGroupMember;

	public ClientAccessLevel AccessLevel { get; set; }

	public override bool AnyGroupMember
	{
		get
		{
			return this.m_anyGroupMember;
		}
	}

	public override QueueRequirement.RequirementType Requirement
	{
		get
		{
			return QueueRequirement.RequirementType.AccessLevel;
		}
	}

	public override bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		return applicant.AccessLevel >= this.AccessLevel;
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context)
	{
		QueueBlockOutReasonDetails queueBlockOutReasonDetails;
		return this.GenerateFailure(systemInfo, applicant, context, out queueBlockOutReasonDetails);
	}

	public unsafe override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context, out QueueBlockOutReasonDetails Details)
	{
		Details = new QueueBlockOutReasonDetails();
		LocalizationArg_AccessLevel localizationArg_AccessLevel = LocalizationArg_AccessLevel.Create(this.AccessLevel);
		if (context == RequirementMessageContext.SoloQueueing)
		{
			LocalizationArg_AccessLevel localizationArg_AccessLevel2 = LocalizationArg_AccessLevel.Create(applicant.AccessLevel);
			return LocalizationPayload.Create("PlayerNeedsAccessLevelToQueue", "Matchmaking", new LocalizationArg[]
			{
				localizationArg_AccessLevel,
				applicant.LocalizedHandle,
				localizationArg_AccessLevel2
			});
		}
		if (context == RequirementMessageContext.GroupQueueing)
		{
			return LocalizationPayload.Create("OneGroupMemberNeedsAccessLevelToQueue", "Matchmaking", new LocalizationArg[]
			{
				localizationArg_AccessLevel
			});
		}
		LocalizationArg_AccessLevel localizationArg_AccessLevel3 = LocalizationArg_AccessLevel.Create(applicant.AccessLevel);
		return LocalizationPayload.Create("YouHaveAccessLevelXButNeedAccessLevelY", "Matchmaking", new LocalizationArg[]
		{
			localizationArg_AccessLevel3,
			localizationArg_AccessLevel
		});
	}

	public override void WriteToJson(JsonWriter writer)
	{
		writer.WritePropertyName("AccessLevel");
		writer.WriteValue(this.AccessLevel.ToString());
		writer.WritePropertyName("AnyGroupMember");
		writer.WriteValue(this.AnyGroupMember.ToString());
	}

	public static QueueRequirement Create(JsonReader reader)
	{
		QueueRequirement_AccessLevel queueRequirement_AccessLevel = new QueueRequirement_AccessLevel();
		reader.Read();
		string value = reader.Value.ToString();
		queueRequirement_AccessLevel.AccessLevel = (ClientAccessLevel)Enum.Parse(typeof(ClientAccessLevel), value, true);
		reader.Read();
		if (reader.TokenType == JsonToken.PropertyName)
		{
			if (reader.Value != null && reader.Value.ToString() == "AnyGroupMember")
			{
				reader.Read();
				queueRequirement_AccessLevel.m_anyGroupMember = bool.Parse(reader.Value.ToString());
				reader.Read();
				return queueRequirement_AccessLevel;
			}
		}
		queueRequirement_AccessLevel.m_anyGroupMember = false;
		return queueRequirement_AccessLevel;
	}
}
