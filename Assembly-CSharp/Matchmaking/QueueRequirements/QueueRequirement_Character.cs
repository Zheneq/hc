using Newtonsoft.Json;
using System;

[Serializable]
public class QueueRequirement_Character : QueueRequirement
{
    private CharacterType CharacterType;
    private RequirementType m_requirementType = RequirementType.HasUnlockedCharacter;
    private bool m_anyGroupMember;

    public override RequirementType Requirement => m_requirementType;
    public override bool AnyGroupMember => m_anyGroupMember;

    public override bool DoesApplicantPass(
        IQueueRequirementSystemInfo systemInfo,
        IQueueRequirementApplicant applicant,
        GameType gameType,
        GameSubType gameSubType)
    {
        bool result = false;
        if (m_requirementType != RequirementType.HasUnlockedCharacter)
        {
            throw new Exception($"Unknown QueueRequirement_Character requirement: {Requirement}");
        }

        for (int i = 0; i < (int)CharacterType.Last; i++)
        {
            CharacterType characterType = (CharacterType)i;
            if (applicant.IsCharacterTypeAvailable(characterType) && CharacterType == characterType)
            {
                result = true;
                break;
            }
        }

        return result;
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
        if (Requirement != RequirementType.HasUnlockedCharacter)
        {
            throw new Exception($"Unknown requirement is failed: {Requirement}");
        }

        LocalizationArg_Freelancer localizationArgFreelancer = LocalizationArg_Freelancer.Create(CharacterType);
        switch (context)
        {
            case RequirementMessageContext.GroupQueueing:
            {
                return LocalizationPayload.Create(
                    "NoGroupMemberHasNotUnlockedFreelancerX",
                    "Matchmaking",
                    localizationArgFreelancer);
            }
            default:
            {
                return LocalizationPayload.Create(
                    "UserXHasNotUnlockedFreelancerX",
                    "Matchmaking",
                    applicant.LocalizedHandle,
                    localizationArgFreelancer);
            }
        }
    }

    public override void WriteToJson(JsonWriter writer)
    {
        writer.WritePropertyName("Character");
        writer.WriteValue(CharacterType.ToString());
        writer.WritePropertyName("AnyGroupMember");
        writer.WriteValue(AnyGroupMember.ToString());
    }

    public static QueueRequirement Create(RequirementType reqType, JsonReader reader)
    {
        QueueRequirement_Character queueRequirementCharacter = new QueueRequirement_Character
        {
            m_requirementType = reqType
        };
        reader.Read();
        string value = reader.Value.ToString();
        queueRequirementCharacter.CharacterType = (CharacterType)Enum.Parse(typeof(CharacterType), value, true);
        reader.Read();
        if (reader.TokenType == JsonToken.PropertyName
            && reader.Value != null
            && reader.Value.ToString() == "AnyGroupMember")
        {
            reader.Read();
            queueRequirementCharacter.m_anyGroupMember = bool.Parse(reader.Value.ToString());
            reader.Read();
        }
        else
        {
            queueRequirementCharacter.m_anyGroupMember = false;
        }

        return queueRequirementCharacter;
    }
}