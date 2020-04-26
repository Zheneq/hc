public class EffectDuration
{
	public int duration;

	public int age;

	public bool ReadyToEnd()
	{
		bool result = false;
		if (duration > 0)
		{
			result = (age >= duration);
		}
		return result;
	}

	public string DisplayString()
	{
		string text = string.Empty;
		if (duration > 0)
		{
			if (duration - age > 1)
			{
				text += $"{duration - age} turns remaining";
			}
			else if (duration - age == 1)
			{
				text += "1 turn remaining";
			}
		}
		return text;
	}
}
