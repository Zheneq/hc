#if EVOS
using Newtonsoft.Json;
using System;

// custom
[Serializable]
public class QueueRequirement_MinClientVersion : QueueRequirement
{
	// Field names are part of the protocol, must match EvoS
	private int m_major;
	private int m_minor;
	private int m_patch;

	public override bool AnyGroupMember => false;

	public override RequirementType Requirement => RequirementType.MinClientVersion;

	// The client only knows its own version, so every applicant (including group members) is checked against it
	public override bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		return BuildVersion.ClientVersion.CompareTo(new Version(m_major, m_minor, m_patch)) >= 0;
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context)
	{
		return GenerateFailure(systemInfo, applicant, context, out _);
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context, out QueueBlockOutReasonDetails Details)
	{
		Details = new QueueBlockOutReasonDetails();
		return LocalizationPayload.Create(
			"ClientVersionTooOld",
			"Matchmaking",
			LocalizationArg_Int32.Create(m_major),
			LocalizationArg_Int32.Create(m_minor),
			LocalizationArg_Int32.Create(m_patch));
	}

	public override void WriteToJson(JsonWriter writer)
	{
		writer.WritePropertyName("Version");
		writer.WriteValue($"{m_major}.{m_minor}.{m_patch}");
	}

	public static QueueRequirement Create(JsonReader reader)
	{
		reader.Read();
		Version version = new Version(reader.Value?.ToString() ?? "999.999.999");
		reader.Read();
		return new QueueRequirement_MinClientVersion
		{
			m_major = version.Major,
			m_minor = version.Minor,
			m_patch = Math.Max(version.Build, 0)
		};
	}
}
#endif
