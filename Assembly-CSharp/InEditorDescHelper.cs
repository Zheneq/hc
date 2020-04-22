using System;
using System.Collections.Generic;
using UnityEngine;

public static class InEditorDescHelper
{
	public delegate bool ShowFieldDelegateBool(bool input);

	public delegate bool ShowFieldDelegateFloat(float input);

	public delegate string GetListEntryStrDelegate<T>(T item);

	public static string ColoredString(string input, string color = "cyan", bool bold = false)
	{
		string[] obj = new string[5]
		{
			"<color=",
			color,
			">",
			null,
			null
		};
		string text;
		if (bold)
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
			text = "<b>" + input + "</b>";
		}
		else
		{
			text = input;
		}
		obj[3] = text;
		obj[4] = "</color>";
		return string.Concat(obj);
	}

	public static string ColoredString(float input, string color = "cyan", bool bold = false)
	{
		return ColoredString(input.ToString(), color, bold);
	}

	public static string BoldedStirng(string input)
	{
		return "<b>" + input + "</b>";
	}

	public static string DiffColorStr(string input)
	{
		return ColoredString(input, "orange");
	}

	public static string ContextVarName(string name, bool actorContext = true)
	{
		object color;
		if (actorContext)
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
			color = "orange";
		}
		else
		{
			color = "white";
		}
		string str = "CVar[ " + ColoredString(name, (string)color) + " ]";
		if (!actorContext)
		{
			return "General_" + str;
		}
		return "Actor_" + str;
	}

	public static string AssembleFieldWithDiff(string header, string indent, string otherSep, float myVal, bool showOther, float otherVal, ShowFieldDelegateFloat showFieldDelegate = null)
	{
		string text = string.Empty;
		int num;
		if (showOther)
		{
			while (true)
			{
				switch (5)
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
			num = ((myVal != otherVal) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		bool num2;
		if (showFieldDelegate == null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			num2 = (myVal > 0f);
		}
		else
		{
			num2 = showFieldDelegate(myVal);
		}
		if (num2 || flag)
		{
			text = text + indent + header + ColoredString(myVal.ToString());
		}
		if (flag)
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
			string text2 = text;
			text = text2 + DiffColorStr(otherSep + otherVal) + " ( diff = " + Mathf.Abs(otherVal - myVal) + " )";
		}
		if (text.Length > 0)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			text += "\n";
		}
		return text;
	}

	public static string AssembleFieldWithDiff(string header, string indent, string otherSep, bool myVal, bool showOther, bool otherVal, ShowFieldDelegateBool showFieldDelegate = null)
	{
		string text = string.Empty;
		bool flag = showOther && myVal != otherVal;
		if (!(showFieldDelegate?.Invoke(myVal) ?? true))
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
			if (!flag)
			{
				goto IL_0072;
			}
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		text = text + indent + header + ColoredString(myVal.ToString());
		goto IL_0072;
		IL_0072:
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			text += DiffColorStr(otherSep + otherVal);
		}
		if (text.Length > 0)
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
			text += "\n";
		}
		return text;
	}

	public static string AssembleFieldWithDiff(string header, string indent, string otherSep, Enum myVal, bool showOther, Enum otherVal)
	{
		string text = string.Empty;
		int num;
		if (showOther)
		{
			while (true)
			{
				switch (3)
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
			num = ((Convert.ToInt32(myVal) != Convert.ToInt32(otherVal)) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		if (Convert.ToInt32(myVal) <= 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag)
			{
				goto IL_0075;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		text = text + indent + header + ColoredString(myVal.ToString());
		goto IL_0075;
		IL_0075:
		if (flag)
		{
			text += DiffColorStr(otherSep + otherVal);
		}
		if (text.Length > 0)
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
			text += "\n";
		}
		return text;
	}

	public static string AssembleFieldWithDiff(string header, string indent, string otherSep, GameObject myVal, bool showOther, GameObject otherVal)
	{
		int num;
		if (showOther)
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
			num = ((myVal != otherVal) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		string text = string.Empty;
		if (!(myVal != null))
		{
			if (!flag)
			{
				goto IL_0089;
			}
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		string text2 = text;
		text = text2 + indent + header + "\n" + indent + "    " + ColoredString(GetGameObjectEntryStr(myVal));
		goto IL_0089;
		IL_0089:
		if (flag)
		{
			text += DiffColorStr(otherSep + GetGameObjectEntryStr(otherVal));
		}
		if (text.Length > 0)
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
			text += "\n";
		}
		return text;
	}

	public static string GetGameObjectEntryStr(GameObject obj)
	{
		if (obj == null)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					break;
				default:
					if (1 == 0)
					{
						/*OpCode not supported: LdMemberToken*/;
					}
					return "NULL";
				}
			}
		}
		return "< " + obj.name + " >";
	}

	private static bool HasDifference<T>(T[] myObjList, T[] otherObjList)
	{
		int num;
		if (myObjList != null)
		{
			while (true)
			{
				switch (3)
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
			num = myObjList.Length;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		int num3;
		if (otherObjList != null)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			num3 = otherObjList.Length;
		}
		else
		{
			num3 = 0;
		}
		int num4 = num3;
		bool result = false;
		if (num2 != num4)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			result = true;
		}
		else if (num2 > 0)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (num2 == num4)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				int num5 = 0;
				while (true)
				{
					if (num5 < myObjList.Length)
					{
						T x = myObjList[num5];
						bool flag = false;
						int num6 = 0;
						while (true)
						{
							if (num6 < otherObjList.Length)
							{
								T y = otherObjList[num6];
								if (EqualityComparer<T>.Default.Equals(x, y))
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
									flag = true;
									break;
								}
								num6++;
								continue;
							}
							while (true)
							{
								switch (6)
								{
								case 0:
									continue;
								}
								break;
							}
							break;
						}
						if (!flag)
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
							result = true;
							break;
						}
						num5++;
						continue;
					}
					while (true)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					break;
				}
			}
		}
		return result;
	}

	public static string GetListDiffString<T>(string header, string indent, T[] myObjList, bool showDiff, T[] otherObjList, GetListEntryStrDelegate<T> stringFormatter = null)
	{
		string text = string.Empty;
		string str = "\t\t\t | ";
		int num;
		if (myObjList != null)
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
			num = myObjList.Length;
		}
		else
		{
			num = 0;
		}
		int num2 = num;
		int num3;
		if (otherObjList != null)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			num3 = otherObjList.Length;
		}
		else
		{
			num3 = 0;
		}
		int num4 = num3;
		bool flag = false;
		if (showDiff)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			flag = HasDifference(myObjList, otherObjList);
		}
		if (num2 <= 0)
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
			if (!flag)
			{
				goto IL_02b6;
			}
		}
		text = text + indent + header;
		if (flag)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			text = text + "    \t\t | " + header + " in base";
		}
		text += "\n";
		int num5;
		if (flag)
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
			num5 = Mathf.Max(num2, num4);
		}
		else
		{
			num5 = num2;
		}
		int num6 = num5;
		for (int i = 0; i < num6; i++)
		{
			string empty = string.Empty;
			if (i < num2)
			{
				string text2 = "NULL_ENTRY";
				if (myObjList[i] != null)
				{
					while (true)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
					if (stringFormatter == null)
					{
						while (true)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						text2 = ColoredString(myObjList[i].ToString());
						if (typeof(T).IsEnum)
						{
							while (true)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							text2 = "[ " + text2 + " ] ";
						}
					}
					else
					{
						text2 = ColoredString(stringFormatter(myObjList[i]));
					}
				}
				empty = empty + indent + "    " + text2;
			}
			else
			{
				empty = empty + indent + "    (none)          ";
			}
			text += empty;
			if (flag)
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
				if (i < num4)
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
					string str2 = "NULL_ENTRY";
					if (otherObjList[i] != null)
					{
						while (true)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						if (stringFormatter == null)
						{
							str2 = otherObjList[i].ToString();
							if (typeof(T).IsEnum)
							{
								while (true)
								{
									switch (5)
									{
									case 0:
										continue;
									}
									break;
								}
								str2 = "[ " + str2 + " ] ";
							}
						}
						else
						{
							str2 = stringFormatter(otherObjList[i]);
						}
					}
					text += DiffColorStr(str + str2);
				}
				else
				{
					text += DiffColorStr(str + "    (none)");
				}
			}
			text += "\n";
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		goto IL_02b6;
		IL_02b6:
		return text;
	}
}
