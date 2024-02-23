using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

[Serializable]
public abstract class QueueRequirement
{
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

	public abstract RequirementType Requirement { get; }
	public abstract bool AnyGroupMember { get; }

	public static IEnumerable<Type> MessageTypes
	{
		get
		{
			return new Type[8]
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

	public abstract void WriteToJson(JsonWriter writer);

	public abstract bool DoesApplicantPass(
		IQueueRequirementSystemInfo systemInfo,
		IQueueRequirementApplicant applicant,
		GameType gameType,
		GameSubType gameSubType);

	public abstract LocalizationPayload GenerateFailure(
		IQueueRequirementSystemInfo systemInfo,
		IQueueRequirementApplicant applicant,
		RequirementMessageContext context);

	public abstract LocalizationPayload GenerateFailure(
		IQueueRequirementSystemInfo systemInfo,
		IQueueRequirementApplicant applicant,
		RequirementMessageContext context,
		out QueueBlockOutReasonDetails Details);

	internal static QueueRequirement ExtractRequirementFromReader(JsonReader reader)
	{
		QueueRequirement queueRequirement = null;
		try
		{
			reader.Read();
			string text = reader.Value.ToString();
			RequirementType requirementType = (RequirementType)Enum.Parse(typeof(RequirementType), text, true);
			reader.Read();
			reader.Read();
			switch (requirementType)
			{
			case RequirementType.TotalMatches:
			case RequirementType.CharacterMatches:
			case RequirementType.VsHumanMatches:
			case RequirementType.TotalLevel:
			case RequirementType.SeasonLevel:
			case RequirementType.AvailableCharacterCount:
				queueRequirement = QueueRequirement_GreaterThan.Create(requirementType, reader);
				break;
			case RequirementType.HasUnlockedCharacter:
				queueRequirement = QueueRequirement_Character.Create(requirementType, reader);
				break;
			case RequirementType.AccessLevel:
				queueRequirement = QueueRequirement_AccessLevel.Create(reader);
				break;
			case RequirementType.MaxLeavingPoints:
				queueRequirement = QueueRequirement_MaxLeavingPoints.Create(reader);
				break;
			case RequirementType.TimeOfWeek:
				queueRequirement = QueueRequirement_TimeOfWeek.Create(reader);
				break;
			case RequirementType.AdminDisabled:
				queueRequirement = QueueRequirement_Never.Create(requirementType, reader);
				break;
			case RequirementType.BeforeDate:
			case RequirementType.AfterDate:
				queueRequirement = QueueRequirement_DateTime.Create(requirementType, reader);
				break;
			case RequirementType.ProhibitEnvironment:
				queueRequirement = QueueRequirement_Environement.Create(reader);
				break;
			default:
				throw new Exception(new StringBuilder().Append(text).Append(" is not a valid BogusRequirement type").ToString());
			}
			reader.Read();
			return queueRequirement;
		}
		catch (Exception ex)
		{
			Log.Error("Failure reading QueueRequirement at {0}: {1}={2}", reader.Path, reader.TokenType, reader.Value);
			throw ex;
		}
	}
}
