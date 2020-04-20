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
			result = (this.age >= this.duration);
		}
		return result;
	}

	public string DisplayString()
	{
		string text = string.Empty;
		if (this.duration > 0)
		{
			if (this.duration - this.age > 1)
			{
				text += string.Format("{0} turns remaining", this.duration - this.age);
			}
			else if (this.duration - this.age == 1)
			{
				text += "1 turn remaining";
			}
		}
		return text;
	}
}
