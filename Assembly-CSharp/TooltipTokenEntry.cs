using System;
using System.Collections.Generic;
using System.Text;

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
		m_name = name;
		m_desc = desc;
	}

	public abstract string GetStringToReplace();

	public abstract string GetReplacementString();

	public abstract string GetInEditorValuePreview();

	public static string GetTooltipWithSubstitutes(string tooltipNow, List<TooltipTokenEntry> tokensToReplace, bool boldCaretToken = false)
	{
		if (tooltipNow == null)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					break;
				default:
					return string.Empty;
				}
			}
		}
		string tooltipNow2 = tooltipNow;
		object substitute;
		if (boldCaretToken)
		{
			substitute = "<b><color=#FFC000>";
		}
		else
		{
			substitute = "<color=#FFC000>";
		}
		tooltipNow = GetStringWithReplacements(tooltipNow2, "[^", (string)substitute);
		string tooltipNow3 = tooltipNow;
		object substitute2;
		if (boldCaretToken)
		{
			substitute2 = "</color></b>";
		}
		else
		{
			substitute2 = "</color>";
		}
		tooltipNow = GetStringWithReplacements(tooltipNow3, "^]", (string)substitute2);
		if (tokensToReplace != null && tokensToReplace.Count > 0)
		{
			using (List<TooltipTokenEntry>.Enumerator enumerator = tokensToReplace.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					TooltipTokenEntry current = enumerator.Current;
					string stringToReplace = current.GetStringToReplace();
					if (stringToReplace.Length != 0)
					{
						string replacementString = current.GetReplacementString();
						tooltipNow = GetStringWithReplacements(tooltipNow, stringToReplace, replacementString);
					}
				}
			}
		}
		if (tooltipNow.IndexOf("%") >= 0)
		{
			tooltipNow = GetStringWithReplacements(tooltipNow, "%</color><color=#FFC000>%</color>", "%</color>");
			tooltipNow = GetStringWithReplacements(tooltipNow, "%</color>%", "%</color>");
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
				int num2 = -1;
				while ((num2 = tooltipNow.IndexOf(toReplace, StringComparison.OrdinalIgnoreCase)) >= 0 && num < 100)
				{
					tooltipNow = new StringBuilder().Append(tooltipNow, 0, num2).Append(substitute).Append(tooltipNow.Substring(num2 + length)).ToString();
					num++;
				}
				return tooltipNow;
			}
		}
		return tooltipNow;
	}

	private static string GetStringWithoutBoldTags(string input)
	{
		string stringWithReplacements = GetStringWithReplacements(input, "<b>", string.Empty);
		return GetStringWithReplacements(stringWithReplacements, "</b>", string.Empty);
	}

	private static string ReplaceColorTagsWithShorthand(string tooltipNow)
	{
		int num = 0;
		int num2 = -1;
		string text = "<color=";
		int length = text.Length;
		while ((num2 = tooltipNow.IndexOf(text, StringComparison.OrdinalIgnoreCase)) >= 0)
		{
			if (num < 100)
			{
				int num3 = tooltipNow.IndexOf(">", num2);
				int startIndex = num2 + length;
				if (num3 > 0)
				{
					startIndex = num3 + 1;
				}

				tooltipNow = new StringBuilder().Append(tooltipNow, 0, num2).Append("[^").Append(tooltipNow.Substring(startIndex)).ToString();
				num++;
				continue;
			}
			break;
		}
		tooltipNow = GetStringWithReplacements(tooltipNow, "</color>", "^]");
		return tooltipNow;
	}

	public static string GetTooltipWithUnprocessedTokens(string tooltipNow)
	{
		tooltipNow = GetStringWithoutBoldTags(tooltipNow);
		tooltipNow = ReplaceColorTagsWithShorthand(tooltipNow);
		return tooltipNow;
	}

	public static List<StatusType> GetStatusTypesFromTooltip(string input)
	{
		List<StatusType> list = new List<StatusType>();
		if (s_statusTypeToKeywords != null)
		{
			if (!string.IsNullOrEmpty(input) && input.IndexOf("[^", StringComparison.OrdinalIgnoreCase) >= 0)
			{
				while (true)
				{
					switch (3)
					{
					case 0:
						break;
					default:
					{
						foreach (KeyValuePair<StatusType, List<string>> s_statusTypeToKeyword in s_statusTypeToKeywords)
						{
							List<string> value = s_statusTypeToKeyword.Value;
							for (int i = 0; i < value.Count; i++)
							{
								if (input.IndexOf(value[i], StringComparison.OrdinalIgnoreCase) >= 0)
								{
									list.Add(s_statusTypeToKeyword.Key);
									break;
								}
							}
						}
						return list;
					}
					}
				}
			}
		}
		return list;
	}
}
