using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public abstract class QueueRequirement
{
	public abstract QueueRequirement.RequirementType Requirement { get; }

	public abstract bool AnyGroupMember { get; }

	public abstract void WriteToJson(JsonWriter writer);

	public abstract bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType);

	public abstract LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context);

	public abstract LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context, out QueueBlockOutReasonDetails Details);

	public static IEnumerable<Type> MessageTypes
	{
		get
		{
			return new Type[]
			{
				typeof(QueueRequirement_AccessLevel),
				typeof(QueueRequirement_Character),
				typeof(QueueRequirement_DateTime),
				typeof(QueueRequirement_Environement),
				typeof(QueueRequirement_GreaterThan),
				typeof(QueueRequirement_MaxLeavingPoints),
				typeof(QueueRequirement_Never),
				typeof(QueueRequirement_TimeOfWeek)
			};
		}
	}

	internal static QueueRequirement ExtractRequirementFromReader(JsonReader reader)
	{
		QueueRequirement result = null;
		try
		{
			reader.Read();
			string text = reader.Value.ToString();
			QueueRequirement.RequirementType reqType = (QueueRequirement.RequirementType)Enum.Parse(typeof(QueueRequirement.RequirementType), text, true);
			reader.Read();
			reader.Read();
			switch (reqType)
			{
			case QueueRequirement.RequirementType.TotalMatches:
			case QueueRequirement.RequirementType.CharacterMatches:
			case QueueRequirement.RequirementType.VsHumanMatches:
			case QueueRequirement.RequirementType.TotalLevel:
			case QueueRequirement.RequirementType.SeasonLevel:
			case QueueRequirement.RequirementType.AvailableCharacterCount:
				result = QueueRequirement_GreaterThan.Create(reqType, reader);
				goto IL_EF;
			case QueueRequirement.RequirementType.AccessLevel:
				result = QueueRequirement_AccessLevel.Create(reader);
				goto IL_EF;
			case QueueRequirement.RequirementType.MaxLeavingPoints:
				result = QueueRequirement_MaxLeavingPoints.Create(reader);
				goto IL_EF;
			case QueueRequirement.RequirementType.TimeOfWeek:
				result = QueueRequirement_TimeOfWeek.Create(reader);
				goto IL_EF;
			case QueueRequirement.RequirementType.AdminDisabled:
				result = QueueRequirement_Never.Create(reqType, reader);
				goto IL_EF;
			case QueueRequirement.RequirementType.BeforeDate:
			case QueueRequirement.RequirementType.AfterDate:
				result = QueueRequirement_DateTime.Create(reqType, reader);
				goto IL_EF;
			case QueueRequirement.RequirementType.ProhibitEnvironment:
				result = QueueRequirement_Environement.Create(reader);
				goto IL_EF;
			case QueueRequirement.RequirementType.HasUnlockedCharacter:
				result = QueueRequirement_Character.Create(reqType, reader);
				goto IL_EF;
			}
			throw new Exception(string.Format("{0} is not a valid BogusRequirement type", text));
			IL_EF:
			reader.Read();
		}
		catch (Exception ex)
		{
			Log.Error("Failure reading QueueRequirement at {0}: {1}={2}", new object[]
			{
				reader.Path,
				reader.TokenType,
				reader.Value
			});
			throw ex;
		}
		return result;
	}

	public enum RequirementType
	{
		ERROR,
		TotalMatches,
		CharacterMatches,
		VsHumanMatches,
		AccessLevel,
		TotalLevel,
		SeasonLevel,
		MaxLeavingPoints,
		TimeOfWeek,
		AdminDisabled,
		BeforeDate,
		AfterDate,
		ProhibitEnvironment,
		AvailableCharacterCount,
		HasUnlockedCharacter
	}
}
