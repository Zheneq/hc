using Newtonsoft.Json;
using System;
using System.Collections.Generic;

[Serializable]
public class QueueRequirement_MaxLeavingPoints : QueueRequirement
{
	private bool m_anyGroupMember;

	public float MaxValue
	{
		get;
		set;
	}

	public override bool AnyGroupMember
	{
		get { return m_anyGroupMember; }
	}

	public override RequirementType Requirement
	{
		get { return RequirementType.MaxLeavingPoints; }
	}

	public override bool DoesApplicantPass(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, GameType gameType, GameSubType gameSubType)
	{
		return applicant.GameLeavingPoints <= MaxValue;
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context)
	{
		QueueBlockOutReasonDetails Details;
		return GenerateFailure(systemInfo, applicant, context, out Details);
	}

	public override LocalizationPayload GenerateFailure(IQueueRequirementSystemInfo systemInfo, IQueueRequirementApplicant applicant, RequirementMessageContext context, out QueueBlockOutReasonDetails Details)
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
					GameType current = enumerator.Current;
					GameLeavingPenalty gameLeavingPenaltyForGameType = systemInfo.GetGameLeavingPenaltyForGameType(current);
					if (gameLeavingPenaltyForGameType != null && gameLeavingPenaltyForGameType.PointsForgivenPerCompleteGameFinished > num)
					{
						bool flag = true;
						using (IEnumerator<QueueRequirement> enumerator2 = systemInfo.GetQueueRequirements(current).GetEnumerator())
						{
							while (true)
							{
								if (!enumerator2.MoveNext())
								{
									break;
								}
								QueueRequirement current2 = enumerator2.Current;
								if (flag)
								{
									if (!current2.DoesApplicantPass(systemInfo, applicant, current, null))
									{
										while (true)
										{
											switch (6)
											{
											case 0:
												break;
											default:
												flag = false;
												goto end_IL_0077;
											}
										}
									}
								}
							}
							end_IL_0077:;
						}
						if (flag)
						{
							num = gameLeavingPenaltyForGameType.PointsForgivenPerCompleteGameFinished;
							gameType = current;
						}
					}
				}
			}
			finally
			{
				if (enumerator != null)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							break;
						default:
							enumerator.Dispose();
							goto end_IL_00ee;
						}
					}
				}
				end_IL_00ee:;
			}
		}
		if (num > 0f)
		{
			float num2 = (applicant.GameLeavingPoints - MaxValue) / num;
			LocalizationArg_Int32 localizationArg_Int = LocalizationArg_Int32.Create((int)Math.Ceiling(num2));
			LocalizationArg_GameType localizationArg_GameType = LocalizationArg_GameType.Create(gameType);
			if (context == RequirementMessageContext.Generic)
			{
				return LocalizationPayload.Create("PlayerXHasTooMuchLeavingPenaltySoTheyNeedsToPlayYMoreGamesOfZ", "Requirement", applicant.LocalizedHandle, localizationArg_Int, localizationArg_GameType);
			}
			return LocalizationPayload.Create("LeftTooManyActiveGamesToQueueOfferAlternative", "Matchmaking", applicant.LocalizedHandle, localizationArg_Int, localizationArg_GameType);
		}
		if (context == RequirementMessageContext.SoloQueueing)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					return LocalizationPayload.Create("LeftTooManyActiveGamesToQueue", "Matchmaking", applicant.LocalizedHandle);
				}
			}
		}
		if (context == RequirementMessageContext.GroupQueueing)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
					return LocalizationPayload.Create("AllGroupMembersHaveLeftTooManyActiveGamesToQueue", "Matchmaking");
				}
			}
		}
		return LocalizationPayload.Create("PlayerHasTooMuchLeavingPenalty", "Requirement");
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
		if (reader.TokenType == JsonToken.PropertyName)
		{
			if (reader.Value != null)
			{
				if (reader.Value.ToString() == "AnyGroupMember")
				{
					reader.Read();
					queueRequirement_MaxLeavingPoints.m_anyGroupMember = bool.Parse(reader.Value.ToString());
					reader.Read();
					goto IL_00b8;
				}
			}
		}
		queueRequirement_MaxLeavingPoints.m_anyGroupMember = false;
		goto IL_00b8;
		IL_00b8:
		return queueRequirement_MaxLeavingPoints;
	}
}
