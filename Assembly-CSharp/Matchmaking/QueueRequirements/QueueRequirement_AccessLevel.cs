using Newtonsoft.Json;
using System;

[Serializable]
public class QueueRequirement_AccessLevel : QueueRequirement
{
    private bool m_anyGroupMember;

    public ClientAccessLevel AccessLevel { get; set; }

    public override bool AnyGroupMember => m_anyGroupMember;
    public override RequirementType Requirement => RequirementType.AccessLevel;

    public override bool DoesApplicantPass(
        IQueueRequirementSystemInfo systemInfo,
        IQueueRequirementApplicant applicant,
        GameType gameType,
        GameSubType gameSubType)
    {
        return applicant.AccessLevel >= AccessLevel;
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
        LocalizationArg_AccessLevel localizationArgAccessLevel = LocalizationArg_AccessLevel.Create(AccessLevel);
        switch (context)
        {
            case RequirementMessageContext.SoloQueueing:
            {
                return LocalizationPayload.Create(
                    "PlayerNeedsAccessLevelToQueue",
                    "Matchmaking",
                    localizationArgAccessLevel,
                    applicant.LocalizedHandle,
                    LocalizationArg_AccessLevel.Create(applicant.AccessLevel));
            }
            case RequirementMessageContext.GroupQueueing:
            {
                return LocalizationPayload.Create(
                    "OneGroupMemberNeedsAccessLevelToQueue",
                    "Matchmaking",
                    localizationArgAccessLevel);
            }
            default:
            {
                return LocalizationPayload.Create(
                    "YouHaveAccessLevelXButNeedAccessLevelY",
                    "Matchmaking",
                    LocalizationArg_AccessLevel.Create(applicant.AccessLevel),
                    localizationArgAccessLevel);
            }
        }
    }

    public override void WriteToJson(JsonWriter writer)
    {
        writer.WritePropertyName("AccessLevel");
        writer.WriteValue(AccessLevel.ToString());
        writer.WritePropertyName("AnyGroupMember");
        writer.WriteValue(AnyGroupMember.ToString());
    }

    public static QueueRequirement Create(JsonReader reader)
    {
        QueueRequirement_AccessLevel queueRequirement_AccessLevel = new QueueRequirement_AccessLevel();
        reader.Read();
        string value = reader.Value.ToString();
        queueRequirement_AccessLevel.AccessLevel =
            (ClientAccessLevel)Enum.Parse(typeof(ClientAccessLevel), value, true);
        reader.Read();
        if (reader.TokenType == JsonToken.PropertyName
            && reader.Value != null
            && reader.Value.ToString() == "AnyGroupMember")
        {
            reader.Read();
            queueRequirement_AccessLevel.m_anyGroupMember = bool.Parse(reader.Value.ToString());
            reader.Read();
        }
        else
        {
            queueRequirement_AccessLevel.m_anyGroupMember = false;
        }

        return queueRequirement_AccessLevel;
    }
}