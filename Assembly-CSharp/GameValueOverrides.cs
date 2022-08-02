using System;

[Serializable]
public class GameValueOverrides
{
	public enum OverrideAbleGameValue
	{
		None,
		InitialTimeBankConsumables,
		TurnTimeSpan
	}

	public int? InitialTimeBankConsumables;
	public TimeSpan? TurnTimeSpan;

	public void SetIntOverride(OverrideAbleGameValue overrideValueType, int? value)
	{
		if (overrideValueType == OverrideAbleGameValue.InitialTimeBankConsumables)
		{
			InitialTimeBankConsumables = value;
		}
	}

	public void SetTimeSpanOverride(OverrideAbleGameValue overrideValueType, TimeSpan? value)
	{
		if (overrideValueType == OverrideAbleGameValue.TurnTimeSpan)
		{
			TurnTimeSpan = value;
		}
	}
}
