using System;
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
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TooltipTokenHelper.AddTokenInt(List<TooltipTokenEntry>, string, int, string, bool)).MethodHandle;
			}
			if (!addIfNonPositive)
			{
				return;
			}
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		tokens.Add(new TooltipTokenInt(name, desc, value));
	}

	public static void AddTokenFloat(List<TooltipTokenEntry> tokens, string name, float value, string desc = "", bool addIfNonPositive = false)
	{
		if (value <= 0f)
		{
			if (!addIfNonPositive)
			{
				return;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(TooltipTokenHelper.AddTokenFloat(List<TooltipTokenEntry>, string, float, string, bool)).MethodHandle;
			}
		}
		tokens.Add(new TooltipTokenFloat(name, desc, value));
	}
}
