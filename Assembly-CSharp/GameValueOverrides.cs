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
		if (overrideValueType != OverrideAbleGameValue.InitialTimeBankConsumables)
		{
			return;
		}
		while (true)
		{
			InitialTimeBankConsumables = value;
			return;
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
