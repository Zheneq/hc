using Newtonsoft.Json;
using System;

[Serializable]
public class QueueRequirement_DateTime : QueueRequirement
{
	private RequirementType m_requirementType;

	private DateTime m_dateTime;

	public override bool AnyGroupMember => false;

	public override RequirementType Requirement => m_requirementType;

	public override bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		RequirementType requirement = Requirement;
		if (requirement != RequirementType.BeforeDate)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (requirement != RequirementType.AfterDate)
					{
						while (true)
						{
							switch (1)
							{
							case 0:
								break;
							default:
								throw new Exception($"Unknown QueueRequirement_DateTime requirement: {Requirement}");
							}
						}
					}
					return systemInfo.GetCurrentUTCTime() > m_dateTime;
				}
			}
		}
		return systemInfo.GetCurrentUTCTime() < m_dateTime;
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
		if (requirement != RequirementType.BeforeDate)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					if (requirement != RequirementType.AfterDate)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								break;
							default:
								throw new Exception($"Unknown requirement is failed: {Requirement}");
							}
						}
					}
					TimeSpan span = m_dateTime - systemInfo.GetCurrentUTCTime();
					LocalizationArg_TimeSpan localizationArg_TimeSpan = LocalizationArg_TimeSpan.Create(span);
					return LocalizationPayload.Create("WillBeAccessibleInTimeSpan", "Matchmaking", localizationArg_TimeSpan);
				}
				}
			}
		}
		TimeSpan span2 = systemInfo.GetCurrentUTCTime() - m_dateTime;
		LocalizationArg_TimeSpan localizationArg_TimeSpan2 = LocalizationArg_TimeSpan.Create(span2);
		return LocalizationPayload.Create("ClosedTimeSpanAgo", "Matchmaking", localizationArg_TimeSpan2);
	}

	public override void WriteToJson(JsonWriter writer)
	{
		writer.WritePropertyName("Value");
		writer.WriteValue(m_dateTime);
	}

	public static QueueRequirement Create(RequirementType reqType, JsonReader reader)
	{
		QueueRequirement_DateTime queueRequirement_DateTime = new QueueRequirement_DateTime();
		queueRequirement_DateTime.m_requirementType = reqType;
		reader.Read();
		if (reader.TokenType == JsonToken.Date)
		{
			queueRequirement_DateTime.m_dateTime = (DateTime)reader.Value;
			reader.Read();
		}
		else
		{
			string s = reader.Value as string;
			queueRequirement_DateTime.m_dateTime = DateTime.Parse(s);
			reader.Read();
		}
		return queueRequirement_DateTime;
	}
}
