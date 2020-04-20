using System;
using System.Collections.Generic;

public abstract class TooltipTokenEntry
{
	public string m_name;

	public string m_desc;

	private static Dictionary<StatusType, List<string>> s_statusTypeToKeywords = new Dictionary<StatusType, List<string>>
	{
		{
			StatusType.Snared,
			new List<string>
			{
				"[^slow"
			}
		},
		{
			StatusType.Energized,
			new List<string>
			{
				"[^energize"
			}
		},
		{
			StatusType.Revealed,
			new List<string>
			{
				"[^reveal"
			}
		},
		{
			StatusType.KnockedBack,
			new List<string>
			{
				"[^pull",
				"[^knock"
			}
		},
		{
			StatusType.Rooted,
			new List<string>
			{
				"[^root"
			}
		},
		{
			StatusType.Unstoppable,
			new List<string>
			{
				"[^unstop"
			}
		},
		{
			StatusType.Weakened,
			new List<string>
			{
				"[^weak"
			}
		},
		{
			StatusType.Hasted,
			new List<string>
			{
				"[^haste"
			}
		},
		{
			StatusType.Empowered,
			new List<string>
			{
				"[^might"
			}
		},
		{
			StatusType.InvisibleToEnemies,
			new List<string>
			{
				"[^invis"
			}
		},
		{
			StatusType.SilencedNonbasicPlayerAbilities,
			new List<string>
			{
				"[^scram"
			}
		}
	};

	protected TooltipTokenEntry(string name, string desc)
	{
		this.m_name = name;
		this.m_desc = desc;
	}

	public abstract string GetStringToReplace();

	public abstract string GetReplacementString();

	public abstract string GetInEditorValuePreview();

	public static string GetTooltipWithSubstitutes(string tooltipNow, List<TooltipTokenEntry> tokensToReplace, bool boldCaretToken = false)
	{
		if (tooltipNow == null)
		{
			return string.Empty;
		}
		string tooltipNow2 = tooltipNow;
		string toReplace = "[^";
		string substitute;
		if (boldCaretToken)
		{
			substitute = "<b><color=#FFC000>";
		}
		else
		{
			substitute = "<color=#FFC000>";
		}
		tooltipNow = TooltipTokenEntry.GetStringWithReplacements(tooltipNow2, toReplace, substitute);
		string tooltipNow3 = tooltipNow;
		string toReplace2 = "^]";
		string substitute2;
		if (boldCaretToken)
		{
			substitute2 = "</color></b>";
		}
		else
		{
			substitute2 = "</color>";
		}
		tooltipNow = TooltipTokenEntry.GetStringWithReplacements(tooltipNow3, toReplace2, substitute2);
		if (tokensToReplace != null && tokensToReplace.Count > 0)
		{
			using (List<TooltipTokenEntry>.Enumerator enumerator = tokensToReplace.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TooltipTokenEntry tooltipTokenEntry = enumerator.Current;
					string stringToReplace = tooltipTokenEntry.GetStringToReplace();
					if (stringToReplace.Length != 0)
					{
						string replacementString = tooltipTokenEntry.GetReplacementString();
						tooltipNow = TooltipTokenEntry.GetStringWithReplacements(tooltipNow, stringToReplace, replacementString);
					}
				}
			}
		}
		if (tooltipNow.IndexOf("%") >= 0)
		{
			tooltipNow = TooltipTokenEntry.GetStringWithReplacements(tooltipNow, "%</color><color=#FFC000>%</color>", "%</color>");
			tooltipNow = TooltipTokenEntry.GetStringWithReplacements(tooltipNow, "%</color>%", "%</color>");
		}
		return tooltipNow;
	}

	public static string GetStringWithReplacements(string tooltipNow, string toReplace, string substitute)
	{
		int length = toReplace.Length;
		if (length != 0)
		{
			if (tooltipNow != null)
			{
				int num = 0;
				int num2;
				while ((num2 = tooltipNow.IndexOf(toReplace, StringComparison.OrdinalIgnoreCase)) >= 0 && num < 0x64)
				{
					tooltipNow = tooltipNow.Substring(0, num2) + substitute + tooltipNow.Substring(num2 + length);
					num++;
				}
				return tooltipNow;
			}
		}
		return tooltipNow;
	}

	private static string GetStringWithoutBoldTags(string input)
	{
		string stringWithReplacements = TooltipTokenEntry.GetStringWithReplacements(input, "<b>", string.Empty);
		return TooltipTokenEntry.GetStringWithReplacements(stringWithReplacements, "</b>", string.Empty);
	}

	private static string ReplaceColorTagsWithShorthand(string tooltipNow)
	{
		int num = 0;
		string text = "<color=";
		int length = text.Length;
		int num2;
		while ((num2 = tooltipNow.IndexOf(text, StringComparison.OrdinalIgnoreCase)) >= 0)
		{
			if (num >= 0x64)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					goto IL_8A;
				}
			}
			else
			{
				int num3 = tooltipNow.IndexOf(">", num2);
				int startIndex = num2 + length;
				if (num3 > 0)
				{
					startIndex = num3 + 1;
				}
				tooltipNow = tooltipNow.Substring(0, num2) + "[^" + tooltipNow.Substring(startIndex);
				num++;
			}
		}
		IL_8A:
		tooltipNow = TooltipTokenEntry.GetStringWithReplacements(tooltipNow, "</color>", "^]");
		return tooltipNow;
	}

	public static string GetTooltipWithUnprocessedTokens(string tooltipNow)
	{
		tooltipNow = TooltipTokenEntry.GetStringWithoutBoldTags(tooltipNow);
		tooltipNow = TooltipTokenEntry.ReplaceColorTagsWithShorthand(tooltipNow);
		return tooltipNow;
	}

	public static List<StatusType> GetStatusTypesFromTooltip(string input)
	{
		List<StatusType> list = new List<StatusType>();
		if (TooltipTokenEntry.s_statusTypeToKeywords != null)
		{
			if (!string.IsNullOrEmpty(input) && input.IndexOf("[^", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				foreach (KeyValuePair<StatusType, List<string>> keyValuePair in TooltipTokenEntry.s_statusTypeToKeywords)
				{
					List<string> value = keyValuePair.Value;
					for (int i = 0; i < value.Count; i++)
					{
						if (input.IndexOf(value[i], StringComparison.OrdinalIgnoreCase) >= 0)
						{
							list.Add(keyValuePair.Key);
							break;
						}
					}
				}
			}
		}
		return list;
	}
}
