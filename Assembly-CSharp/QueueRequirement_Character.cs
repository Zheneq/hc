using Newtonsoft.Json;
using System;
using System.Text;

[Serializable]
public class QueueRequirement_Character : QueueRequirement
{
	private CharacterType CharacterType;

	private RequirementType m_requirementType = RequirementType.HasUnlockedCharacter;

	private bool m_anyGroupMember;

	public override RequirementType Requirement
	{
		get { return m_requirementType; }
	}

	public override bool AnyGroupMember
	{
		get { return m_anyGroupMember; }
	}

	public override bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		bool result = false;
		RequirementType requirementType = m_requirementType;
		if (requirementType != RequirementType.HasUnlockedCharacter)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					throw new Exception(new StringBuilder().Append("Unknown QueueRequirement_Character requirement: ").Append(Requirement).ToString());
				}
			}
		}
		int num = 0;
		while (true)
		{
			if (num < 40)
			{
				CharacterType characterType = (CharacterType)num;
				if (applicant.IsCharacterTypeAvailable(characterType))
				{
					if (CharacterType == characterType)
					{
						result = true;
						break;
					}
				}
				num++;
				continue;
			}
			break;
		}
		return result;
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context)
	{
		QueueBlockOutReasonDetails Details;
		return GenerateFailure(systemInfo, applicant, context, out Details);
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context, out QueueBlockOutReasonDetails Details)
	{
		Details = new QueueBlockOutReasonDetails();
		RequirementType requirement = Requirement;
		if (requirement != RequirementType.HasUnlockedCharacter)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					break;
				default:
					throw new Exception(new StringBuilder().Append("Unknown requirement is failed: ").Append(Requirement).ToString());
				}
			}
		}
		LocalizationArg_Freelancer localizationArg_Freelancer = LocalizationArg_Freelancer.Create(CharacterType);
		if (context == RequirementMessageContext.GroupQueueing)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return LocalizationPayload.Create("NoGroupMemberHasNotUnlockedFreelancerX", "Matchmaking", localizationArg_Freelancer);
				}
			}
		}
		return LocalizationPayload.Create("UserXHasNotUnlockedFreelancerX", "Matchmaking", applicant.LocalizedHandle, localizationArg_Freelancer);
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
		QueueRequirement_Character queueRequirement_Character = new QueueRequirement_Character();
		queueRequirement_Character.m_requirementType = reqType;
		reader.Read();
		string value = reader.Value.ToString();
		queueRequirement_Character.CharacterType = (CharacterType)Enum.Parse(typeof(CharacterType), value, true);
		reader.Read();
		if (reader.TokenType == JsonToken.PropertyName)
		{
			if (reader.Value != null)
			{
				if (reader.Value.ToString() == "AnyGroupMember")
				{
					reader.Read();
					queueRequirement_Character.m_anyGroupMember = bool.Parse(reader.Value.ToString());
					reader.Read();
					goto IL_00da;
				}
			}
		}
		queueRequirement_Character.m_anyGroupMember = false;
		goto IL_00da;
		IL_00da:
		return queueRequirement_Character;
	}
}
