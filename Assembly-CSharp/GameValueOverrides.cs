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
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
