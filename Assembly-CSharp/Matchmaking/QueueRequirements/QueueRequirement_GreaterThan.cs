using Newtonsoft.Json;
using System;

[Serializable]
public class QueueRequirement_GreaterThan : QueueRequirement
{
    private RequirementType m_requirementType;
    private bool m_anyGroupMember;

    public int MinValue { get; set; }

    public override bool AnyGroupMember => m_anyGroupMember;
    public override RequirementType Requirement => m_requirementType;

    public override bool DoesApplicantPass(
        IQueueRequirementSystemInfo systemInfo,
        IQueueRequirementApplicant applicant,
        GameType gameType,
        GameSubType gameSubType)
    {
        switch (Requirement)
        {
            case RequirementType.TotalMatches:
            {
                return applicant.TotalMatches >= MinValue;
            }
            case RequirementType.CharacterMatches:
            {
                return applicant.CharacterMatches >= MinValue;
            }
            case RequirementType.VsHumanMatches:
            {
                return applicant.VsHumanMatches >= MinValue;
            }
            case RequirementType.TotalLevel:
            {
                return applicant.GetReactorLevel(systemInfo.Seasons) >= MinValue;
            }
            case RequirementType.SeasonLevel:
            {
                return applicant.SeasonLevel >= MinValue;
            }
            case RequirementType.AvailableCharacterCount:
            {
                int num = 0;
                for (int i = 0; i < (int)CharacterType.Last; i++)
                {
                    CharacterType characterType = (CharacterType)i;
                    if (applicant.IsCharacterTypeAvailable(characterType)
                        && systemInfo.IsCharacterAllowed(characterType, gameType, gameSubType))
                    {
                        num++;
                    }
                }

                return num >= MinValue;
            }
            default:
            {
                throw new Exception($"Unknown QueueRequirement_GreaterThan requirement: {Requirement}");
            }
        }
    }

    public override LocalizationPayload GenerateFailure(
        IQueueRequirementSystemInfo systemInfo,
        IQueueRequirementApplicant applicant,
        RequirementMessageContext context,
        out QueueBlockOutReasonDetails Details)
    {
        LocalizationArg_Int32 localizationArgMinValue = LocalizationArg_Int32.Create(MinValue);
        LocalizationArg_Handle localizedHandle = applicant.LocalizedHandle;
        Details = new QueueBlockOutReasonDetails();
        switch (Requirement)
        {
            case RequirementType.TotalMatches:
            {
                switch (context)
                {
                    case RequirementMessageContext.SoloQueueing:
                    {
                        return LocalizationPayload.Create(
                            "NotEnoughGamesPlayedForQueue",
                            "Matchmaking",
                            localizedHandle,
                            localizationArgMinValue);
                    }
                    case RequirementMessageContext.GroupQueueing:
                    {
                        return LocalizationPayload.Create(
                            "NoGroupMemberHasEnoughGamesPlayedForQueue",
                            "Matchmaking",
                            localizationArgMinValue);
                    }
                    default:
                    {
                        return LocalizationPayload.Create(
                            "YouHaveToPlayXMoreGames",
                            "Requirement",
                            LocalizationArg_Int32.Create(MinValue - applicant.TotalMatches));
                    }
                }
            }
            case RequirementType.CharacterMatches:
            {
                LocalizationArg_Freelancer localizationArgFreelancer =
                    LocalizationArg_Freelancer.Create(applicant.CharacterType);
                switch (context)
                {
                    case RequirementMessageContext.SoloQueueing:
                    {
                        return LocalizationPayload.Create(
                            "NotEnoughCharacterGamesPlayedForQueue",
                            "Matchmaking",
                            localizedHandle,
                            localizationArgMinValue,
                            localizationArgFreelancer);
                    }
                    case RequirementMessageContext.GroupQueueing:
                    {
                        return LocalizationPayload.Create(
                            "NoGroupMemberPlayedXGamesWithFreelancerYToQueue",
                            "Requirement",
                            localizationArgMinValue,
                            localizationArgFreelancer);
                    }
                    default:
                    {
                        return LocalizationPayload.Create(
                            "YouHaveToPlayXMoreGamesWithFreelancerY",
                            "Requirement",
                            LocalizationArg_Int32.Create(MinValue - applicant.CharacterMatches),
                            localizationArgFreelancer);
                    }
                }
            }
            case RequirementType.VsHumanMatches:
            {
                Details.RequirementTypeNotMet = Requirement;
                Details.Context = context;
                Details.NumGamesPlayed = applicant.VsHumanMatches;
                Details.NumGamesRequired = MinValue;
                switch (context)
                {
                    case RequirementMessageContext.SoloQueueing:
                    {
                        return LocalizationPayload.Create(
                            "NotEnoughGamesPlayedAgainstHumansForQueue",
                            "Matchmaking",
                            localizedHandle,
                            localizationArgMinValue,
                            LocalizationArg_Int32.Create(applicant.VsHumanMatches));
                    }
                    case RequirementMessageContext.GroupQueueing:
                    {
                        return LocalizationPayload.Create(
                            "NoGroupMemberHasEnoughGamesPlayedAgainstHumansForQueue",
                            "Matchmaking",
                            localizationArgMinValue);
                    }
                    default:
                    {
                        return LocalizationPayload.Create(
                            "YouHaveToPlayXMoreGamesAgainstHumans",
                            "Requirement",
                            LocalizationArg_Int32.Create(MinValue - applicant.CharacterMatches));
                    }
                }
            }
            case RequirementType.TotalLevel:
            {
                switch (context)
                {
                    case RequirementMessageContext.SoloQueueing:
                    {
                        return LocalizationPayload.Create(
                            "NotEnoughAccountlevelForQueue",
                            "Matchmaking",
                            localizedHandle,
                            localizationArgMinValue);
                    }
                    case RequirementMessageContext.GroupQueueing:
                    {
                        return LocalizationPayload.Create(
                            "NoGroupMemberHasEnoughAccountLevelForQueue",
                            "Matchmaking",
                            localizationArgMinValue);
                    }
                    default:
                    {
                        return LocalizationPayload.Create(
                            "YouHaveToBeTotalLevelX",
                            "Requirement",
                            localizationArgMinValue);
                    }
                }
            }
            case RequirementType.SeasonLevel:
            {
                switch (context)
                {
                    case RequirementMessageContext.SoloQueueing:
                    {
                        return LocalizationPayload.Create(
                            "NotEnoughSeasonlevelForQueue",
                            "Matchmaking",
                            localizedHandle,
                            localizationArgMinValue);
                    }
                    case RequirementMessageContext.GroupQueueing:
                    {
                        return LocalizationPayload.Create(
                            "NoGroupMemberHasEnoughSeasonLevelForQueue",
                            "Matchmaking",
                            localizationArgMinValue);
                    }
                    default:
                    {
                        return LocalizationPayload.Create(
                            "YouHaveToBeSeasonLevelX",
                            "Requirement",
                            localizationArgMinValue);
                    }
                }
            }
            case RequirementType.AvailableCharacterCount:
            {
                switch (context)
                {
                    case RequirementMessageContext.SoloQueueing:
                    {
                        return LocalizationPayload.Create(
                            "NotEnoughAvailableCharactersForQueue",
                            "Matchmaking",
                            localizedHandle,
                            localizationArgMinValue);
                    }
                    case RequirementMessageContext.GroupQueueing:
                    {
                        return LocalizationPayload.Create(
                            "NoGroupMemberHasEnoughAvailableCharactersForQueue",
                            "Matchmaking",
                            localizationArgMinValue);
                    }
                    default:
                    {
                        return LocalizationPayload.Create(
                            "YouHaveAccessToXFreelancers",
                            "Requirement",
                            localizationArgMinValue);
                    }
                }
            }
            default:
            {
                throw new Exception($"Unknown requirement is failed: {Requirement}");
            }
        }
    }

    public override LocalizationPayload GenerateFailure(
        IQueueRequirementSystemInfo systemInfo,
        IQueueRequirementApplicant applicant,
        RequirementMessageContext context)
    {
        return GenerateFailure(systemInfo, applicant, context, out _);
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
        QueueRequirement_GreaterThan queueRequirementGreaterThan = new QueueRequirement_GreaterThan
        {
            m_requirementType = reqType
        };
        reader.Read();
        queueRequirementGreaterThan.MinValue = int.Parse(reader.Value.ToString());
        reader.Read();
        if (reader.TokenType == JsonToken.PropertyName
            && reader.Value != null
            && reader.Value.ToString() == "AnyGroupMember")
        {
            reader.Read();
            queueRequirementGreaterThan.m_anyGroupMember = bool.Parse(reader.Value.ToString());
            reader.Read();
        }
        else
        {
            queueRequirementGreaterThan.m_anyGroupMember = false;
        }

        return queueRequirementGreaterThan;
    }
}