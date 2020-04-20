using System;
using Newtonsoft.Json;

[Serializable]
public class QueueRequirement_TimeOfWeek : QueueRequirement
{
	public TimeSpan Start { get; set; }

	public TimeSpan End { get; set; }

	public override QueueRequirement.RequirementType Requirement
	{
		get
		{
			return QueueRequirement.RequirementType.TimeOfWeek;
		}
	}

	public override bool AnyGroupMember
	{
		get
		{
			return false;
		}
	}

	public override bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		DateTime currentUTCTime = systemInfo.GetCurrentUTCTime();
		DateTime date = currentUTCTime.AddDays(0.0 - (double)currentUTCTime.DayOfWeek).Date;
		DateTime t = date + this.Start;
		if (currentUTCTime > t)
		{
			DateTime dateTime = date + this.End;
			if (dateTime < t)
			{
				dateTime = dateTime.AddDays(7.0);
			}
			return currentUTCTime < dateTime;
		}
		return false;
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context)
	{
		QueueBlockOutReasonDetails queueBlockOutReasonDetails;
		return this.GenerateFailure(systemInfo, applicant, context, out queueBlockOutReasonDetails);
	}

	public unsafe override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context, out QueueBlockOutReasonDetails Details)
	{
		Details = new QueueBlockOutReasonDetails();
		DateTime currentUTCTime = systemInfo.GetCurrentUTCTime();
		DateTime date = currentUTCTime.AddDays(0.0 - (double)currentUTCTime.DayOfWeek).Date;
		DateTime dateTime = date + this.Start;
		if (dateTime < currentUTCTime)
		{
			dateTime = dateTime.AddDays(7.0);
		}
		LocalizationArg_TimeSpan localizationArg_TimeSpan = LocalizationArg_TimeSpan.Create(dateTime - currentUTCTime);
		if (context == RequirementMessageContext.Generic)
		{
			return LocalizationPayload.Create("WillBeEnabledInXTimeSpan", "Requirement", new LocalizationArg[]
			{
				localizationArg_TimeSpan
			});
		}
		return LocalizationPayload.Create("QueueWillTurnOnLaterInTheWeek", "Matchmaking", new LocalizationArg[]
		{
			localizationArg_TimeSpan
		});
	}

	public override void WriteToJson(JsonWriter writer)
	{
		writer.WritePropertyName("Start");
		writer.WriteValue(this.Start);
		writer.WritePropertyName("End");
		writer.WriteValue(this.End);
	}

	public static QueueRequirement Create(JsonReader reader)
	{
		reader.Read();
		string s = reader.Value as string;
		reader.Read();
		reader.Read();
		string s2 = reader.Value as string;
		reader.Read();
		return new QueueRequirement_TimeOfWeek
		{
			Start = TimeSpan.Parse(s),
			End = TimeSpan.Parse(s2)
		};
	}
}
