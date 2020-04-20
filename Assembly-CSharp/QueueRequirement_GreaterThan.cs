using System;
using Newtonsoft.Json;

[Serializable]
public class QueueRequirement_GreaterThan : QueueRequirement
{
	private QueueRequirement.RequirementType m_requirementType;

	private bool m_anyGroupMember;

	public int MinValue { get; set; }

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
			return this.m_requirementType;
		}
	}

	public override bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		QueueRequirement.RequirementType requirement = this.Requirement;
		switch (requirement)
		{
		case QueueRequirement.RequirementType.TotalMatches:
			return applicant.TotalMatches >= this.MinValue;
		case QueueRequirement.RequirementType.CharacterMatches:
			return applicant.CharacterMatches >= this.MinValue;
		case QueueRequirement.RequirementType.VsHumanMatches:
			return applicant.VsHumanMatches >= this.MinValue;
		default:
		{
			if (requirement != QueueRequirement.RequirementType.AvailableCharacterCount)
			{
				throw new Exception(string.Format("Unknown QueueRequirement_GreaterThan requirement: {0}", this.Requirement));
			}
			int num = 0;
			for (int i = 0; i < 0x28; i++)
			{
				CharacterType characterType = (CharacterType)i;
				if (applicant.IsCharacterTypeAvailable(characterType))
				{
					if (systemInfo.IsCharacterAllowed(characterType, gameType, gameSubType))
					{
						num++;
					}
				}
			}
			return num >= this.MinValue;
		}
		case QueueRequirement.RequirementType.TotalLevel:
			return applicant.GetReactorLevel(systemInfo.Seasons) >= this.MinValue;
		case QueueRequirement.RequirementType.SeasonLevel:
			return applicant.SeasonLevel >= this.MinValue;
		}
	}

	public unsafe override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context, out QueueBlockOutReasonDetails Details)
	{
		LocalizationArg_Int32 localizationArg_Int = LocalizationArg_Int32.Create(this.MinValue);
		LocalizationArg_Handle localizedHandle = applicant.LocalizedHandle;
		LocalizationArg_Int32 localizationArg_Int2 = LocalizationArg_Int32.Create(applicant.VsHumanMatches);
		Details = new QueueBlockOutReasonDetails();
		QueueRequirement.RequirementType requirement = this.Requirement;
		switch (requirement)
		{
		case QueueRequirement.RequirementType.TotalMatches:
		{
			if (context == RequirementMessageContext.SoloQueueing)
			{
				return LocalizationPayload.Create("NotEnoughGamesPlayedForQueue", "Matchmaking", new LocalizationArg[]
				{
					localizedHandle,
					localizationArg_Int
				});
			}
			if (context == RequirementMessageContext.GroupQueueing)
			{
				return LocalizationPayload.Create("NoGroupMemberHasEnoughGamesPlayedForQueue", "Matchmaking", new LocalizationArg[]
				{
					localizationArg_Int
				});
			}
			LocalizationArg_Int32 localizationArg_Int3 = LocalizationArg_Int32.Create(this.MinValue - applicant.TotalMatches);
			return LocalizationPayload.Create("YouHaveToPlayXMoreGames", "Requirement", new LocalizationArg[]
			{
				localizationArg_Int3
			});
		}
		case QueueRequirement.RequirementType.CharacterMatches:
		{
			LocalizationArg_Freelancer localizationArg_Freelancer = LocalizationArg_Freelancer.Create(applicant.CharacterType);
			if (context == RequirementMessageContext.SoloQueueing)
			{
				return LocalizationPayload.Create("NotEnoughCharacterGamesPlayedForQueue", "Matchmaking", new LocalizationArg[]
				{
					localizedHandle,
					localizationArg_Int,
					localizationArg_Freelancer
				});
			}
			if (context == RequirementMessageContext.GroupQueueing)
			{
				return LocalizationPayload.Create("NoGroupMemberPlayedXGamesWithFreelancerYToQueue", "Requirement", new LocalizationArg[]
				{
					localizationArg_Int,
					localizationArg_Freelancer
				});
			}
			LocalizationArg_Int32 localizationArg_Int4 = LocalizationArg_Int32.Create(this.MinValue - applicant.CharacterMatches);
			return LocalizationPayload.Create("YouHaveToPlayXMoreGamesWithFreelancerY", "Requirement", new LocalizationArg[]
			{
				localizationArg_Int4,
				localizationArg_Freelancer
			});
		}
		case QueueRequirement.RequirementType.VsHumanMatches:
		{
			Details.RequirementTypeNotMet = new QueueRequirement.RequirementType?(this.Requirement);
			Details.Context = new RequirementMessageContext?(context);
			Details.NumGamesPlayed = new int?(applicant.VsHumanMatches);
			Details.NumGamesRequired = new int?(this.MinValue);
			if (context == RequirementMessageContext.SoloQueueing)
			{
				return LocalizationPayload.Create("NotEnoughGamesPlayedAgainstHumansForQueue", "Matchmaking", new LocalizationArg[]
				{
					localizedHandle,
					localizationArg_Int,
					localizationArg_Int2
				});
			}
			if (context == RequirementMessageContext.GroupQueueing)
			{
				return LocalizationPayload.Create("NoGroupMemberHasEnoughGamesPlayedAgainstHumansForQueue", "Matchmaking", new LocalizationArg[]
				{
					localizationArg_Int
				});
			}
			LocalizationArg_Int32 localizationArg_Int5 = LocalizationArg_Int32.Create(this.MinValue - applicant.CharacterMatches);
			return LocalizationPayload.Create("YouHaveToPlayXMoreGamesAgainstHumans", "Requirement", new LocalizationArg[]
			{
				localizationArg_Int5
			});
		}
		default:
			if (requirement != QueueRequirement.RequirementType.AvailableCharacterCount)
			{
				throw new Exception(string.Format("Unknown requirement is failed: {0}", this.Requirement));
			}
			if (context == RequirementMessageContext.SoloQueueing)
			{
				return LocalizationPayload.Create("NotEnoughAvailableCharactersForQueue", "Matchmaking", new LocalizationArg[]
				{
					localizedHandle,
					localizationArg_Int
				});
			}
			if (context == RequirementMessageContext.GroupQueueing)
			{
				return LocalizationPayload.Create("NoGroupMemberHasEnoughAvailableCharactersForQueue", "Matchmaking", new LocalizationArg[]
				{
					localizationArg_Int
				});
			}
			return LocalizationPayload.Create("YouHaveAccessToXFreelancers", "Requirement", new LocalizationArg[]
			{
				localizationArg_Int
			});
		case QueueRequirement.RequirementType.TotalLevel:
			if (context == RequirementMessageContext.SoloQueueing)
			{
				return LocalizationPayload.Create("NotEnoughAccountlevelForQueue", "Matchmaking", new LocalizationArg[]
				{
					localizedHandle,
					localizationArg_Int
				});
			}
			if (context == RequirementMessageContext.GroupQueueing)
			{
				return LocalizationPayload.Create("NoGroupMemberHasEnoughAccountLevelForQueue", "Matchmaking", new LocalizationArg[]
				{
					localizationArg_Int
				});
			}
			return LocalizationPayload.Create("YouHaveToBeTotalLevelX", "Requirement", new LocalizationArg[]
			{
				localizationArg_Int
			});
		case QueueRequirement.RequirementType.SeasonLevel:
			if (context == RequirementMessageContext.SoloQueueing)
			{
				return LocalizationPayload.Create("NotEnoughSeasonlevelForQueue", "Matchmaking", new LocalizationArg[]
				{
					localizedHandle,
					localizationArg_Int
				});
			}
			if (context == RequirementMessageContext.GroupQueueing)
			{
				return LocalizationPayload.Create("NoGroupMemberHasEnoughSeasonLevelForQueue", "Matchmaking", new LocalizationArg[]
				{
					localizationArg_Int
				});
			}
			return LocalizationPayload.Create("YouHaveToBeSeasonLevelX", "Requirement", new LocalizationArg[]
			{
				localizationArg_Int
			});
		}
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context)
	{
		QueueBlockOutReasonDetails queueBlockOutReasonDetails;
		return this.GenerateFailure(systemInfo, applicant, context, out queueBlockOutReasonDetails);
	}

	public override void WriteToJson(JsonWriter writer)
	{
		writer.WritePropertyName("MinValue");
		writer.WriteValue(this.MinValue);
		writer.WritePropertyName("AnyGroupMember");
		writer.WriteValue(this.AnyGroupMember.ToString());
	}

	public static QueueRequirement Create(QueueRequirement.RequirementType reqType, JsonReader reader)
	{
		QueueRequirement_GreaterThan queueRequirement_GreaterThan = new QueueRequirement_GreaterThan();
		queueRequirement_GreaterThan.m_requirementType = reqType;
		reader.Read();
		queueRequirement_GreaterThan.MinValue = int.Parse(reader.Value.ToString());
		reader.Read();
		if (reader.TokenType == JsonToken.PropertyName && reader.Value != null)
		{
			if (reader.Value.ToString() == "AnyGroupMember")
			{
				reader.Read();
				queueRequirement_GreaterThan.m_anyGroupMember = bool.Parse(reader.Value.ToString());
				reader.Read();
				return queueRequirement_GreaterThan;
			}
		}
		queueRequirement_GreaterThan.m_anyGroupMember = false;
		return queueRequirement_GreaterThan;
	}
}
