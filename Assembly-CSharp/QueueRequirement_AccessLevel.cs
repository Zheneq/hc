using Newtonsoft.Json;
using System;

[Serializable]
public class QueueRequirement_AccessLevel : QueueRequirement
{
	private bool m_anyGroupMember;

	public ClientAccessLevel AccessLevel
	{
		get;
		set;
	}

	public override bool AnyGroupMember => m_anyGroupMember;

	public override RequirementType Requirement => RequirementType.AccessLevel;

	public override bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		return applicant.AccessLevel >= AccessLevel;
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context)
	{
		QueueBlockOutReasonDetails Details;
		return GenerateFailure(systemInfo, applicant, context, out Details);
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context, out QueueBlockOutReasonDetails Details)
	{
		Details = new QueueBlockOutReasonDetails();
		LocalizationArg_AccessLevel localizationArg_AccessLevel = LocalizationArg_AccessLevel.Create(AccessLevel);
		switch (context)
		{
		case RequirementMessageContext.SoloQueueing:
		{
			LocalizationArg_AccessLevel localizationArg_AccessLevel3 = LocalizationArg_AccessLevel.Create(applicant.AccessLevel);
			return LocalizationPayload.Create("PlayerNeedsAccessLevelToQueue", "Matchmaking", localizationArg_AccessLevel, applicant.LocalizedHandle, localizationArg_AccessLevel3);
		}
		case RequirementMessageContext.GroupQueueing:
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				return LocalizationPayload.Create("OneGroupMemberNeedsAccessLevelToQueue", "Matchmaking", localizationArg_AccessLevel);
			}
		default:
		{
			LocalizationArg_AccessLevel localizationArg_AccessLevel2 = LocalizationArg_AccessLevel.Create(applicant.AccessLevel);
			return LocalizationPayload.Create("YouHaveAccessLevelXButNeedAccessLevelY", "Matchmaking", localizationArg_AccessLevel2, localizationArg_AccessLevel);
		}
		}
	}

	public override void WriteToJson(JsonWriter writer)
	{
		writer.WritePropertyName("AccessLevel");
		writer.WriteValue(AccessLevel.ToString());
		writer.WritePropertyName("AnyGroupMember");
		writer.WriteValue(AnyGroupMember.ToString());
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
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (reader.Value != null && reader.Value.ToString() == "AnyGroupMember")
			{
				reader.Read();
				queueRequirement_AccessLevel.m_anyGroupMember = bool.Parse(reader.Value.ToString());
				reader.Read();
				goto IL_00bc;
			}
		}
		queueRequirement_AccessLevel.m_anyGroupMember = false;
		goto IL_00bc;
		IL_00bc:
		return queueRequirement_AccessLevel;
	}
}
