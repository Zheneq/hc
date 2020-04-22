using Newtonsoft.Json;
using System;

[Serializable]
public class QueueRequirement_TimeOfWeek : QueueRequirement
{
	public TimeSpan Start
	{
		get;
		set;
	}

	public TimeSpan End
	{
		get;
		set;
	}

	public override RequirementType Requirement => RequirementType.TimeOfWeek;

	public override bool AnyGroupMember => false;

	public override bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		DateTime currentUTCTime = systemInfo.GetCurrentUTCTime();
		DateTime date = currentUTCTime.AddDays(0.0 - (double)currentUTCTime.DayOfWeek).Date;
		DateTime t = date + Start;
		if (currentUTCTime > t)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					break;
				default:
				{
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					DateTime dateTime = date + End;
					if (dateTime < t)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							break;
						}
						dateTime = dateTime.AddDays(7.0);
					}
					return currentUTCTime < dateTime;
				}
				}
			}
		}
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
		DateTime currentUTCTime = systemInfo.GetCurrentUTCTime();
		DateTime date = currentUTCTime.AddDays(0.0 - (double)currentUTCTime.DayOfWeek).Date;
		DateTime dateTime = date + Start;
		if (dateTime < currentUTCTime)
		{
			while (true)
			{
				switch (1)
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
			dateTime = dateTime.AddDays(7.0);
		}
		LocalizationArg_TimeSpan localizationArg_TimeSpan = LocalizationArg_TimeSpan.Create(dateTime - currentUTCTime);
		if (context == RequirementMessageContext.Generic)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return LocalizationPayload.Create("WillBeEnabledInXTimeSpan", "Requirement", localizationArg_TimeSpan);
				}
			}
		}
		return LocalizationPayload.Create("QueueWillTurnOnLaterInTheWeek", "Matchmaking", localizationArg_TimeSpan);
	}

	public override void WriteToJson(JsonWriter writer)
	{
		writer.WritePropertyName("Start");
		writer.WriteValue(Start);
		writer.WritePropertyName("End");
		writer.WriteValue(End);
	}

	public static QueueRequirement Create(JsonReader reader)
	{
		reader.Read();
		string s = reader.Value as string;
		reader.Read();
		reader.Read();
		string s2 = reader.Value as string;
		reader.Read();
		QueueRequirement_TimeOfWeek queueRequirement_TimeOfWeek = new QueueRequirement_TimeOfWeek();
		queueRequirement_TimeOfWeek.Start = TimeSpan.Parse(s);
		queueRequirement_TimeOfWeek.End = TimeSpan.Parse(s2);
		return queueRequirement_TimeOfWeek;
	}
}
