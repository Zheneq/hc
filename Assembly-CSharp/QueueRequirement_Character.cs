using System;
using Newtonsoft.Json;

[Serializable]
public class QueueRequirement_Character : QueueRequirement
{
	private CharacterType CharacterType;

	private QueueRequirement.RequirementType m_requirementType = QueueRequirement.RequirementType.HasUnlockedCharacter;

	private bool m_anyGroupMember;

	public override QueueRequirement.RequirementType Requirement
	{
		get
		{
			return this.m_requirementType;
		}
	}

	public override bool AnyGroupMember
	{
		get
		{
			return this.m_anyGroupMember;
		}
	}

	public override bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		bool result = false;
		QueueRequirement.RequirementType requirementType = this.m_requirementType;
		if (requirementType != QueueRequirement.RequirementType.HasUnlockedCharacter)
		{
			throw new Exception(string.Format("Unknown QueueRequirement_Character requirement: {0}", this.Requirement));
		}
		for (int i = 0; i < 0x28; i++)
		{
			CharacterType characterType = (CharacterType)i;
			if (applicant.IsCharacterTypeAvailable(characterType))
			{
				if (this.CharacterType == characterType)
				{
					result = true;
					return result;
				}
			}
		}
		for (;;)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			return result;
		}
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
		if (requirement != QueueRequirement.RequirementType.HasUnlockedCharacter)
		{
			throw new Exception(string.Format("Unknown requirement is failed: {0}", this.Requirement));
		}
		LocalizationArg_Freelancer localizationArg_Freelancer = LocalizationArg_Freelancer.Create(this.CharacterType);
		if (context == RequirementMessageContext.GroupQueueing)
		{
			return LocalizationPayload.Create("NoGroupMemberHasNotUnlockedFreelancerX", "Matchmaking", new LocalizationArg[]
			{
				localizationArg_Freelancer
			});
		}
		return LocalizationPayload.Create("UserXHasNotUnlockedFreelancerX", "Matchmaking", new LocalizationArg[]
		{
			applicant.LocalizedHandle,
			localizationArg_Freelancer
		});
	}

	public override void WriteToJson(JsonWriter writer)
	{
		writer.WritePropertyName("Character");
		writer.WriteValue(this.CharacterType.ToString());
		writer.WritePropertyName("AnyGroupMember");
		writer.WriteValue(this.AnyGroupMember.ToString());
	}

	public static QueueRequirement Create(QueueRequirement.RequirementType reqType, JsonReader reader)
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
					return queueRequirement_Character;
				}
			}
		}
		queueRequirement_Character.m_anyGroupMember = false;
		return queueRequirement_Character;
	}
}
