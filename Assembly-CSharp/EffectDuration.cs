using System;

public class EffectDuration
{
	public int duration;

	public int age;

	public bool ReadyToEnd()
	{
		bool result = false;
		if (this.duration > 0)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(EffectDuration.ReadyToEnd()).MethodHandle;
			}
			result = (this.age >= this.duration);
		}
		return result;
	}

	public string DisplayString()
	{
		string text = string.Empty;
		if (this.duration > 0)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(EffectDuration.DisplayString()).MethodHandle;
			}
			if (this.duration - this.age > 1)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				text += string.Format("{0} turns remaining", this.duration - this.age);
			}
			else if (this.duration - this.age == 1)
			{
				for (;;)
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
