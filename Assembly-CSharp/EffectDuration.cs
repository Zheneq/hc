public class EffectDuration
{
	public int duration;

	public int age;

	public bool ReadyToEnd()
	{
		bool result = false;
		if (duration > 0)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = (age >= duration);
		}
		return result;
	}

	public string DisplayString()
	{
		string text = string.Empty;
		if (duration > 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (duration - age > 1)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				text += $"{duration - age} turns remaining";
			}
			else if (duration - age == 1)
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				text += "1 turn remaining";
			}
		}
		return text;
	}
}
