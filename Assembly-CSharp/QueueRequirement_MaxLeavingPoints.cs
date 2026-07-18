using System;
using Newtonsoft.Json;

[Serializable]
public class QueueRequirement_MaxLeavingPoints : QueueRequirement
{
    private bool m_anyGroupMember;

    public float MaxValue { get; set; }

    public override bool AnyGroupMember => m_anyGroupMember;
    public override RequirementType Requirement => RequirementType.MaxLeavingPoints;

    public override bool DoesApplicantPass(
        IQueueRequirementSystemInfo systemInfo,
        IQueueRequirementApplicant applicant,
        GameType gameType,
        GameSubType gameSubType)
    {
        return applicant.GameLeavingPoints <= MaxValue;
    }

    public override LocalizationPayload GenerateFailure(
        IQueueRequirementSystemInfo systemInfo,
        IQueueRequirementApplicant applicant,
        RequirementMessageContext context)
    {
        return GenerateFailure(systemInfo, applicant, context, out _);
    }

    public override LocalizationPayload GenerateFailure(
        IQueueRequirementSystemInfo systemInfo,
        IQueueRequirementApplicant applicant,
        RequirementMessageContext context,
        out QueueBlockOutReasonDetails Details)
    {
        Details = new QueueBlockOutReasonDetails();
        float pointsForgiven = 0f;
        GameType gameType = GameType.None;
        if (systemInfo != null)
        {
            foreach (GameType testGameType in systemInfo.GetGameTypes())
            {
                GameLeavingPenalty gameLeavingPenaltyForGameType =
                    systemInfo.GetGameLeavingPenaltyForGameType(testGameType);
                if (gameLeavingPenaltyForGameType != null
                    && gameLeavingPenaltyForGameType.PointsForgivenPerCompleteGameFinished > pointsForgiven)
                {
                    bool doesPassQueueRequirements = true;
                    foreach (QueueRequirement requirement in systemInfo.GetQueueRequirements(testGameType))
                    {
                        if (!requirement.DoesApplicantPass(systemInfo, applicant, testGameType, null))
                        {
                            doesPassQueueRequirements = false;
                            break;
                        }
                    }

                    if (doesPassQueueRequirements)
                    {
                        pointsForgiven = gameLeavingPenaltyForGameType.PointsForgivenPerCompleteGameFinished;
                        gameType = testGameType;
                    }
                }
            }
        }

        if (pointsForgiven > 0f)
        {
            float games = (applicant.GameLeavingPoints - MaxValue) / pointsForgiven;
            LocalizationArg_Int32 localizationArgGames = LocalizationArg_Int32.Create((int)Math.Ceiling(games));
            LocalizationArg_GameType localizationArgGameType = LocalizationArg_GameType.Create(gameType);
            if (context == RequirementMessageContext.Generic)
            {
                return LocalizationPayload.Create(
                    "PlayerXHasTooMuchLeavingPenaltySoTheyNeedsToPlayYMoreGamesOfZ",
                    "Requirement",
                    applicant.LocalizedHandle,
                    localizationArgGames,
                    localizationArgGameType);
            }

            return LocalizationPayload.Create(
                "LeftTooManyActiveGamesToQueueOfferAlternative",
                "Matchmaking",
                applicant.LocalizedHandle,
                localizationArgGames,
                localizationArgGameType);
        }

        switch (context)
        {
            case RequirementMessageContext.SoloQueueing:
                return LocalizationPayload.Create(
                    "LeftTooManyActiveGamesToQueue",
                    "Matchmaking",
                    applicant.LocalizedHandle);
            case RequirementMessageContext.GroupQueueing:
            {
                return LocalizationPayload.Create(
                    "AllGroupMembersHaveLeftTooManyActiveGamesToQueue",
                    "Matchmaking");
            }
            default:
            {
                return LocalizationPayload.Create(
                    "PlayerHasTooMuchLeavingPenalty",
                    "Requirement");
            }
        }
    }

    public override void WriteToJson(JsonWriter writer)
    {
        writer.WritePropertyName("MaxValue");
        writer.WriteValue(MaxValue);
        writer.WritePropertyName("AnyGroupMember");
        writer.WriteValue(AnyGroupMember.ToString());
    }

    public static QueueRequirement Create(JsonReader reader)
    {
        QueueRequirement_MaxLeavingPoints queueRequirement_MaxLeavingPoints = new QueueRequirement_MaxLeavingPoints();
        reader.Read();
        queueRequirement_MaxLeavingPoints.MaxValue = float.Parse(reader.Value.ToString());
        reader.Read();
        if (reader.TokenType == JsonToken.PropertyName
            && reader.Value != null
            && reader.Value.ToString() == "AnyGroupMember")
        {
            reader.Read();
            queueRequirement_MaxLeavingPoints.m_anyGroupMember = bool.Parse(reader.Value.ToString());
            reader.Read();
        }
        else
        {
            queueRequirement_MaxLeavingPoints.m_anyGroupMember = false;
        }

        return queueRequirement_MaxLeavingPoints;
    }
}