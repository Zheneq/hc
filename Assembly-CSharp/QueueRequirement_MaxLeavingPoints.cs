using System;
using System.Collections.Generic;
using Newtonsoft.Json;

[Serializable]
public class QueueRequirement_MaxLeavingPoints : QueueRequirement
{
	private bool m_anyGroupMember;

	public float MaxValue { get; set; }

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
			return QueueRequirement.RequirementType.MaxLeavingPoints;
		}
	}

	public override bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		return applicant.GameLeavingPoints <= this.MaxValue;
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context)
	{
		QueueBlockOutReasonDetails queueBlockOutReasonDetails;
		return this.GenerateFailure(systemInfo, applicant, context, out queueBlockOutReasonDetails);
	}

	public unsafe override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context, out QueueBlockOutReasonDetails Details)
	{
		Details = new QueueBlockOutReasonDetails();
		float num = 0f;
		GameType gameType = GameType.None;
		if (systemInfo != null)
		{
			IEnumerator<GameType> enumerator = systemInfo.GetGameTypes().GetEnumerator();
			try
			{
				while (enumerator.MoveNext())
				{
					GameType gameType2 = enumerator.Current;
					GameLeavingPenalty gameLeavingPenaltyForGameType = systemInfo.GetGameLeavingPenaltyForGameType(gameType2);
					if (gameLeavingPenaltyForGameType != null && gameLeavingPenaltyForGameType.PointsForgivenPerCompleteGameFinished > num)
					{
						bool flag = true;
						using (IEnumerator<QueueRequirement> enumerator2 = systemInfo.GetQueueRequirements(gameType2).GetEnumerator())
						{
							while (enumerator2.MoveNext())
							{
								QueueRequirement queueRequirement = enumerator2.Current;
								if (flag)
								{
									if (!queueRequirement.DoesApplicantPass(systemInfo, applicant, gameType2, null))
									{
										flag = false;
										goto IL_D1;
									}
								}
							}
						}
						IL_D1:
						if (flag)
						{
							num = gameLeavingPenaltyForGameType.PointsForgivenPerCompleteGameFinished;
							gameType = gameType2;
						}
					}
				}
			}
			finally
			{
				if (enumerator != null)
				{
					enumerator.Dispose();
				}
			}
		}
		if (num > 0f)
		{
			float num2 = (applicant.GameLeavingPoints - this.MaxValue) / num;
			LocalizationArg_Int32 localizationArg_Int = LocalizationArg_Int32.Create((int)Math.Ceiling((double)num2));
			LocalizationArg_GameType localizationArg_GameType = LocalizationArg_GameType.Create(gameType);
			if (context == RequirementMessageContext.Generic)
			{
				return LocalizationPayload.Create("PlayerXHasTooMuchLeavingPenaltySoTheyNeedsToPlayYMoreGamesOfZ", "Requirement", new LocalizationArg[]
				{
					applicant.LocalizedHandle,
					localizationArg_Int,
					localizationArg_GameType
				});
			}
			return LocalizationPayload.Create("LeftTooManyActiveGamesToQueueOfferAlternative", "Matchmaking", new LocalizationArg[]
			{
				applicant.LocalizedHandle,
				localizationArg_Int,
				localizationArg_GameType
			});
		}
		else
		{
			if (context == RequirementMessageContext.SoloQueueing)
			{
				return LocalizationPayload.Create("LeftTooManyActiveGamesToQueue", "Matchmaking", new LocalizationArg[]
				{
					applicant.LocalizedHandle
				});
			}
			if (context == RequirementMessageContext.GroupQueueing)
			{
				return LocalizationPayload.Create("AllGroupMembersHaveLeftTooManyActiveGamesToQueue", "Matchmaking");
			}
			return LocalizationPayload.Create("PlayerHasTooMuchLeavingPenalty", "Requirement");
		}
	}

	public override void WriteToJson(JsonWriter writer)
	{
		writer.WritePropertyName("MaxValue");
		writer.WriteValue(this.MaxValue);
		writer.WritePropertyName("AnyGroupMember");
		writer.WriteValue(this.AnyGroupMember.ToString());
	}

	public static QueueRequirement Create(JsonReader reader)
	{
		QueueRequirement_MaxLeavingPoints queueRequirement_MaxLeavingPoints = new QueueRequirement_MaxLeavingPoints();
		reader.Read();
		queueRequirement_MaxLeavingPoints.MaxValue = float.Parse(reader.Value.ToString());
		reader.Read();
		if (reader.TokenType == JsonToken.PropertyName)
		{
			if (reader.Value != null)
			{
				if (reader.Value.ToString() == "AnyGroupMember")
				{
					reader.Read();
					queueRequirement_MaxLeavingPoints.m_anyGroupMember = bool.Parse(reader.Value.ToString());
					reader.Read();
					return queueRequirement_MaxLeavingPoints;
				}
			}
		}
		queueRequirement_MaxLeavingPoints.m_anyGroupMember = false;
		return queueRequirement_MaxLeavingPoints;
	}
}
