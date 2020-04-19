using System;
using Newtonsoft.Json;

[Serializable]
public class QueueRequirement_DateTime : QueueRequirement
{
	private QueueRequirement.RequirementType m_requirementType;

	private DateTime m_dateTime;

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

	public override bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		QueueRequirement.RequirementType requirement = this.Requirement;
		if (requirement == QueueRequirement.RequirementType.BeforeDate)
		{
			return systemInfo.GetCurrentUTCTime() < this.m_dateTime;
		}
		for (;;)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(QueueRequirement_DateTime.DoesApplicantPass(IQueueRequirementSystemInfo, IQueueRequirementApplicant, GameType, GameSubType)).MethodHandle;
		}
		if (requirement != QueueRequirement.RequirementType.AfterDate)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			throw new Exception(string.Format("Unknown QueueRequirement_DateTime requirement: {0}", this.Requirement));
		}
		return systemInfo.GetCurrentUTCTime() > this.m_dateTime;
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context)
	{
		QueueBlockOutReasonDetails queueBlockOutReasonDetails;
		return this.GenerateFailure(systemInfo, applicant, context, out queueBlockOutReasonDetails);
	}

	public unsafe override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context, out QueueBlockOutReasonDetails Details)
	{
		Details = new QueueBlockOutReasonDetails();
		QueueRequirement.RequirementType requirement = this.Requirement;
		if (requirement == QueueRequirement.RequirementType.BeforeDate)
		{
			TimeSpan span = systemInfo.GetCurrentUTCTime() - this.m_dateTime;
			LocalizationArg_TimeSpan localizationArg_TimeSpan = LocalizationArg_TimeSpan.Create(span);
			return LocalizationPayload.Create("ClosedTimeSpanAgo", "Matchmaking", new LocalizationArg[]
			{
				localizationArg_TimeSpan
			});
		}
		for (;;)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			break;
		}
		if (!true)
		{
			RuntimeMethodHandle runtimeMethodHandle = methodof(QueueRequirement_DateTime.GenerateFailure(IQueueRequirementSystemInfo, IQueueRequirementApplicant, RequirementMessageContext, QueueBlockOutReasonDetails*)).MethodHandle;
		}
		if (requirement != QueueRequirement.RequirementType.AfterDate)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			throw new Exception(string.Format("Unknown requirement is failed: {0}", this.Requirement));
		}
		TimeSpan span2 = this.m_dateTime - systemInfo.GetCurrentUTCTime();
		LocalizationArg_TimeSpan localizationArg_TimeSpan2 = LocalizationArg_TimeSpan.Create(span2);
		return LocalizationPayload.Create("WillBeAccessibleInTimeSpan", "Matchmaking", new LocalizationArg[]
		{
			localizationArg_TimeSpan2
		});
	}

	public override void WriteToJson(JsonWriter writer)
	{
		writer.WritePropertyName("Value");
		writer.WriteValue(this.m_dateTime);
	}

	public static QueueRequirement Create(QueueRequirement.RequirementType reqType, JsonReader reader)
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
