using System.Collections.Generic;

public class TooltipTokenHelper
{
	public const string COLOR_STRING = "#FFC000";

	public const string STRONG_START = "<color=#FFC000>";

	public const string STRONG_END = "</color>";

	public const string STRONG_START_BOLD = "<b><color=#FFC000>";

	public const string STRONG_END_BOLD = "</color></b>";

	public const string TEMP_DOUBLE_PCT_SEARCH_0 = "%</color><color=#FFC000>%</color>";

	public const string TEMP_DOUBLE_PCT_SEARCH_1 = "%</color>%";

	public const string TEMP_DOUBLE_PCT_REPLACE = "%</color>";

	public static void AddTokenInt(List<TooltipTokenEntry> tokens, string name, int value, string desc = "", bool addIfNonPositive = false)
	{
		if (value <= 0)
		{
			if (!addIfNonPositive)
			{
				return;
			}
		}
		tokens.Add(new TooltipTokenInt(name, desc, value));
	}

	public static void AddTokenFloat(List<TooltipTokenEntry> tokens, string name, float value, string desc = "", bool addIfNonPositive = false)
	{
		if (!(value > 0f))
		{
			if (!addIfNonPositive)
			{
				return;
			}
		}
		tokens.Add(new TooltipTokenFloat(name, desc, value));
	}
}
