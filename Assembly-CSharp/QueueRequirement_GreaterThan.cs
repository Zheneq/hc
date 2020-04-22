using Newtonsoft.Json;
using System;

[Serializable]
public class QueueRequirement_GreaterThan : QueueRequirement
{
	private RequirementType m_requirementType;

	private bool m_anyGroupMember;

	public int MinValue
	{
		get;
		set;
	}

	public override bool AnyGroupMember => m_anyGroupMember;

	public override RequirementType Requirement => m_requirementType;

	public override bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		switch (Requirement)
		{
		default:
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				throw new Exception($"Unknown QueueRequirement_GreaterThan requirement: {Requirement}");
			}
		case RequirementType.TotalMatches:
			return applicant.TotalMatches >= MinValue;
		case RequirementType.CharacterMatches:
			return applicant.CharacterMatches >= MinValue;
		case RequirementType.VsHumanMatches:
			return applicant.VsHumanMatches >= MinValue;
		case RequirementType.TotalLevel:
			return applicant.GetReactorLevel(systemInfo.Seasons) >= MinValue;
		case RequirementType.SeasonLevel:
			return applicant.SeasonLevel >= MinValue;
		case RequirementType.AvailableCharacterCount:
		{
			int num = 0;
			for (int i = 0; i < 40; i++)
			{
				CharacterType characterType = (CharacterType)i;
				if (applicant.IsCharacterTypeAvailable(characterType))
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (systemInfo.IsCharacterAllowed(characterType, gameType, gameSubType))
					{
						num++;
					}
				}
			}
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				return num >= MinValue;
			}
		}
		}
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context, out QueueBlockOutReasonDetails Details)
	{
		LocalizationArg_Int32 localizationArg_Int = LocalizationArg_Int32.Create(MinValue);
		LocalizationArg_Handle localizedHandle = applicant.LocalizedHandle;
		LocalizationArg_Int32 localizationArg_Int2 = LocalizationArg_Int32.Create(applicant.VsHumanMatches);
		Details = new QueueBlockOutReasonDetails();
		switch (Requirement)
		{
		default:
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				if (1 == 0)
				{
					/*OpCode not supported: LdMemberToken*/;
				}
				throw new Exception($"Unknown requirement is failed: {Requirement}");
			}
		case RequirementType.TotalMatches:
		{
			if (context == RequirementMessageContext.SoloQueueing)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return LocalizationPayload.Create("NotEnoughGamesPlayedForQueue", "Matchmaking", localizedHandle, localizationArg_Int);
					}
				}
			}
			if (context == RequirementMessageContext.GroupQueueing)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return LocalizationPayload.Create("NoGroupMemberHasEnoughGamesPlayedForQueue", "Matchmaking", localizationArg_Int);
					}
				}
			}
			LocalizationArg_Int32 localizationArg_Int5 = LocalizationArg_Int32.Create(MinValue - applicant.TotalMatches);
			return LocalizationPayload.Create("YouHaveToPlayXMoreGames", "Requirement", localizationArg_Int5);
		}
		case RequirementType.CharacterMatches:
		{
			LocalizationArg_Freelancer localizationArg_Freelancer = LocalizationArg_Freelancer.Create(applicant.CharacterType);
			switch (context)
			{
			case RequirementMessageContext.SoloQueueing:
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					return LocalizationPayload.Create("NotEnoughCharacterGamesPlayedForQueue", "Matchmaking", localizedHandle, localizationArg_Int, localizationArg_Freelancer);
				}
			case RequirementMessageContext.GroupQueueing:
				return LocalizationPayload.Create("NoGroupMemberPlayedXGamesWithFreelancerYToQueue", "Requirement", localizationArg_Int, localizationArg_Freelancer);
			default:
			{
				LocalizationArg_Int32 localizationArg_Int4 = LocalizationArg_Int32.Create(MinValue - applicant.CharacterMatches);
				return LocalizationPayload.Create("YouHaveToPlayXMoreGamesWithFreelancerY", "Requirement", localizationArg_Int4, localizationArg_Freelancer);
			}
			}
		}
		case RequirementType.VsHumanMatches:
		{
			Details.RequirementTypeNotMet = Requirement;
			Details.Context = context;
			Details.NumGamesPlayed = applicant.VsHumanMatches;
			Details.NumGamesRequired = MinValue;
			if (context == RequirementMessageContext.SoloQueueing)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
						return LocalizationPayload.Create("NotEnoughGamesPlayedAgainstHumansForQueue", "Matchmaking", localizedHandle, localizationArg_Int, localizationArg_Int2);
					}
				}
			}
			if (context == RequirementMessageContext.GroupQueueing)
			{
				while (true)
				{
					switch (7)
					{
					case 0:
						break;
					default:
						return LocalizationPayload.Create("NoGroupMemberHasEnoughGamesPlayedAgainstHumansForQueue", "Matchmaking", localizationArg_Int);
					}
				}
			}
			LocalizationArg_Int32 localizationArg_Int3 = LocalizationArg_Int32.Create(MinValue - applicant.CharacterMatches);
			return LocalizationPayload.Create("YouHaveToPlayXMoreGamesAgainstHumans", "Requirement", localizationArg_Int3);
		}
		case RequirementType.TotalLevel:
			switch (context)
			{
			case RequirementMessageContext.SoloQueueing:
				while (true)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					return LocalizationPayload.Create("NotEnoughAccountlevelForQueue", "Matchmaking", localizedHandle, localizationArg_Int);
				}
			case RequirementMessageContext.GroupQueueing:
				return LocalizationPayload.Create("NoGroupMemberHasEnoughAccountLevelForQueue", "Matchmaking", localizationArg_Int);
			default:
				return LocalizationPayload.Create("YouHaveToBeTotalLevelX", "Requirement", localizationArg_Int);
			}
		case RequirementType.SeasonLevel:
			switch (context)
			{
			case RequirementMessageContext.SoloQueueing:
				return LocalizationPayload.Create("NotEnoughSeasonlevelForQueue", "Matchmaking", localizedHandle, localizationArg_Int);
			case RequirementMessageContext.GroupQueueing:
				while (true)
				{
					switch (3)
					{
					case 0:
						continue;
					}
					return LocalizationPayload.Create("NoGroupMemberHasEnoughSeasonLevelForQueue", "Matchmaking", localizationArg_Int);
				}
			default:
				return LocalizationPayload.Create("YouHaveToBeSeasonLevelX", "Requirement", localizationArg_Int);
			}
		case RequirementType.AvailableCharacterCount:
			if (context == RequirementMessageContext.SoloQueueing)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return LocalizationPayload.Create("NotEnoughAvailableCharactersForQueue", "Matchmaking", localizedHandle, localizationArg_Int);
					}
				}
			}
			if (context == RequirementMessageContext.GroupQueueing)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return LocalizationPayload.Create("NoGroupMemberHasEnoughAvailableCharactersForQueue", "Matchmaking", localizationArg_Int);
					}
				}
			}
			return LocalizationPayload.Create("YouHaveAccessToXFreelancers", "Requirement", localizationArg_Int);
		}
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context)
	{
		QueueBlockOutReasonDetails Details;
		return GenerateFailure(systemInfo, applicant, context, out Details);
	}

	public override void WriteToJson(JsonWriter writer)
	{
		writer.WritePropertyName("MinValue");
		writer.WriteValue(MinValue);
		writer.WritePropertyName("AnyGroupMember");
		writer.WriteValue(AnyGroupMember.ToString());
	}

	public static QueueRequirement Create(RequirementType reqType, JsonReader reader)
	{
		QueueRequirement_GreaterThan queueRequirement_GreaterThan = new QueueRequirement_GreaterThan();
		queueRequirement_GreaterThan.m_requirementType = reqType;
		reader.Read();
		queueRequirement_GreaterThan.MinValue = int.Parse(reader.Value.ToString());
		reader.Read();
		if (reader.TokenType == JsonToken.PropertyName && reader.Value != null)
		{
			while (true)
			{
				switch (5)
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
			if (reader.Value.ToString() == "AnyGroupMember")
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				reader.Read();
				queueRequirement_GreaterThan.m_anyGroupMember = bool.Parse(reader.Value.ToString());
				reader.Read();
				goto IL_00b3;
			}
		}
		queueRequirement_GreaterThan.m_anyGroupMember = false;
		goto IL_00b3;
		IL_00b3:
		return queueRequirement_GreaterThan;
	}
}
