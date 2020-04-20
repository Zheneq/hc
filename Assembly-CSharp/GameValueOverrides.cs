using System;

[Serializable]
public class GameValueOverrides
{
	public int? InitialTimeBankConsumables;

	public TimeSpan? TurnTimeSpan;

	public void SetIntOverride(GameValueOverrides.OverrideAbleGameValue overrideValueType, int? value)
	{
		if (overrideValueType == GameValueOverrides.OverrideAbleGameValue.InitialTimeBankConsumables)
		{
			this.InitialTimeBankConsumables = value;
		}
	}

	public void SetTimeSpanOverride(GameValueOverrides.OverrideAbleGameValue overrideValueType, TimeSpan? value)
	{
		if (overrideValueType == GameValueOverrides.OverrideAbleGameValue.TurnTimeSpan)
		{
			this.TurnTimeSpan = value;
		}
	}

	public enum OverrideAbleGameValue
	{
		None,
		InitialTimeBankConsumables,
		TurnTimeSpan
	}
}
