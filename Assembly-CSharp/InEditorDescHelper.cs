using System;
using System.Collections.Generic;
using UnityEngine;

public static class InEditorDescHelper
{
	public static string ColoredString(string input, string color = "cyan", bool bold = false)
	{
		string[] array = new string[5];
		array[0] = "<color=";
		array[1] = color;
		array[2] = ">";
		int num = 3;
		string text;
		if (bold)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InEditorDescHelper.ColoredString(string, string, bool)).MethodHandle;
			}
			text = "<b>" + input + "</b>";
		}
		else
		{
			text = input;
		}
		array[num] = text;
		array[4] = "</color>";
		return string.Concat(array);
	}

	public static string ColoredString(float input, string color = "cyan", bool bold = false)
	{
		return InEditorDescHelper.ColoredString(input.ToString(), color, bold);
	}

	public static string BoldedStirng(string input)
	{
		return "<b>" + input + "</b>";
	}

	public static string DiffColorStr(string input)
	{
		return InEditorDescHelper.ColoredString(input, "orange", false);
	}

	public static string ContextVarName(string name, bool actorContext = true)
	{
		string str = "CVar[ ";
		string color;
		if (actorContext)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InEditorDescHelper.ContextVarName(string, bool)).MethodHandle;
			}
			color = "orange";
		}
		else
		{
			color = "white";
		}
		string str2 = str + InEditorDescHelper.ColoredString(name, color, false) + " ]";
		if (!actorContext)
		{
			return "General_" + str2;
		}
		return "Actor_" + str2;
	}

	public static string AssembleFieldWithDiff(string header, string indent, string otherSep, float myVal, bool showOther, float otherVal, InEditorDescHelper.ShowFieldDelegateFloat showFieldDelegate = null)
	{
		string text = string.Empty;
		bool flag;
		if (showOther)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(InEditorDescHelper.AssembleFieldWithDiff(string, string, string, float, bool, float, InEditorDescHelper.ShowFieldDelegateFloat)).MethodHandle;
			}
			flag = (myVal != otherVal);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		bool flag3;
		if (showFieldDelegate == null)
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
			flag3 = (myVal > 0f);
		}
		else
		{
			flag3 = showFieldDelegate(myVal);
		}
		if (flag3 || flag2)
		{
			text = text + indent + header + InEditorDescHelper.ColoredString(myVal.ToString(), "cyan", false);
		}
		if (flag2)
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
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				InEditorDescHelper.DiffColorStr(otherSep + otherVal),
				" ( diff = ",
				Mathf.Abs(otherVal - myVal),
				" )"
			});
		}
		if (text.Length > 0)
		{
			for (;;)
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

	public static string AssembleFieldWithDiff(string header, string indent, string otherSep, bool myVal, bool showOther, bool otherVal, InEditorDescHelper.ShowFieldDelegateBool showFieldDelegate = null)
	{
		string text = string.Empty;
		bool flag = showOther && myVal != otherVal;
		if (showFieldDelegate != null && !showFieldDelegate(myVal))
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InEditorDescHelper.AssembleFieldWithDiff(string, string, string, bool, bool, bool, InEditorDescHelper.ShowFieldDelegateBool)).MethodHandle;
			}
			if (!flag)
			{
				goto IL_72;
			}
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		text = text + indent + header + InEditorDescHelper.ColoredString(myVal.ToString(), "cyan", false);
		IL_72:
		if (flag)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			text += InEditorDescHelper.DiffColorStr(otherSep + otherVal);
		}
		if (text.Length > 0)
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
			text += "\n";
		}
		return text;
	}

	public static string AssembleFieldWithDiff(string header, string indent, string otherSep, Enum myVal, bool showOther, Enum otherVal)
	{
		string text = string.Empty;
		bool flag;
		if (showOther)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InEditorDescHelper.AssembleFieldWithDiff(string, string, string, Enum, bool, Enum)).MethodHandle;
			}
			flag = (Convert.ToInt32(myVal) != Convert.ToInt32(otherVal));
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		if (Convert.ToInt32(myVal) <= 0)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!flag2)
			{
				goto IL_75;
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
		}
		text = text + indent + header + InEditorDescHelper.ColoredString(myVal.ToString(), "cyan", false);
		IL_75:
		if (flag2)
		{
			text += InEditorDescHelper.DiffColorStr(otherSep + otherVal);
		}
		if (text.Length > 0)
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
			text += "\n";
		}
		return text;
	}

	public static string AssembleFieldWithDiff(string header, string indent, string otherSep, GameObject myVal, bool showOther, GameObject otherVal)
	{
		bool flag;
		if (showOther)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InEditorDescHelper.AssembleFieldWithDiff(string, string, string, GameObject, bool, GameObject)).MethodHandle;
			}
			flag = (myVal != otherVal);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		string text = string.Empty;
		if (!(myVal != null))
		{
			if (!flag2)
			{
				goto IL_89;
			}
			for (;;)
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
		text = string.Concat(new string[]
		{
			text2,
			indent,
			header,
			"\n",
			indent,
			"    ",
			InEditorDescHelper.ColoredString(InEditorDescHelper.GetGameObjectEntryStr(myVal), "cyan", false)
		});
		IL_89:
		if (flag2)
		{
			text += InEditorDescHelper.DiffColorStr(otherSep + InEditorDescHelper.GetGameObjectEntryStr(otherVal));
		}
		if (text.Length > 0)
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
			text += "\n";
		}
		return text;
	}

	public static string GetGameObjectEntryStr(GameObject obj)
	{
		if (obj == null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InEditorDescHelper.GetGameObjectEntryStr(GameObject)).MethodHandle;
			}
			return "NULL";
		}
		return "< " + obj.name + " >";
	}

	private static bool HasDifference<T>(T[] myObjList, T[] otherObjList)
	{
		int num;
		if (myObjList != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InEditorDescHelper.HasDifference(T[], T[])).MethodHandle;
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
			for (;;)
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
			for (;;)
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
			for (;;)
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
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				int i = 0;
				IL_E5:
				while (i < myObjList.Length)
				{
					T x = myObjList[i];
					bool flag = false;
					int j = 0;
					while (j < otherObjList.Length)
					{
						T y = otherObjList[j];
						if (EqualityComparer<T>.Default.Equals(x, y))
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
							flag = true;
							IL_CD:
							if (!flag)
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
								return true;
							}
							i++;
							goto IL_E5;
						}
						else
						{
							j++;
						}
					}
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						goto IL_CD;
					}
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
		}
		return result;
	}

	public static string GetListDiffString<T>(string header, string indent, T[] myObjList, bool showDiff, T[] otherObjList, InEditorDescHelper.GetListEntryStrDelegate<T> stringFormatter = null)
	{
		string text = string.Empty;
		string str = "\t\t\t | ";
		int num;
		if (myObjList != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(InEditorDescHelper.GetListDiffString(string, string, T[], bool, T[], InEditorDescHelper.GetListEntryStrDelegate<T>)).MethodHandle;
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
			for (;;)
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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			flag = InEditorDescHelper.HasDifference<T>(myObjList, otherObjList);
		}
		if (num2 <= 0)
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
			if (!flag)
			{
				return text;
			}
		}
		text = text + indent + header;
		if (flag)
		{
			for (;;)
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
			for (;;)
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
			string text2 = string.Empty;
			if (i < num2)
			{
				string text3 = "NULL_ENTRY";
				if (myObjList[i] != null)
				{
					for (;;)
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
						for (;;)
						{
							switch (5)
							{
							case 0:
								continue;
							}
							break;
						}
						text3 = InEditorDescHelper.ColoredString(myObjList[i].ToString(), "cyan", false);
						if (typeof(T).IsEnum)
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							text3 = "[ " + text3 + " ] ";
						}
					}
					else
					{
						text3 = InEditorDescHelper.ColoredString(stringFormatter(myObjList[i]), "cyan", false);
					}
				}
				text2 = text2 + indent + "    " + text3;
			}
			else
			{
				text2 = text2 + indent + "    (none)          ";
			}
			text += text2;
			if (flag)
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
				if (i < num4)
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
					string str2 = "NULL_ENTRY";
					if (otherObjList[i] != null)
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
						if (stringFormatter == null)
						{
							str2 = otherObjList[i].ToString();
							if (typeof(T).IsEnum)
							{
								for (;;)
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
					text += InEditorDescHelper.DiffColorStr(str + str2);
				}
				else
				{
					text += InEditorDescHelper.DiffColorStr(str + "    (none)");
				}
			}
			text += "\n";
		}
		for (;;)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			break;
		}
		return text;
	}

	public delegate bool ShowFieldDelegateBool(bool input);

	public delegate bool ShowFieldDelegateFloat(float input);

	public delegate string GetListEntryStrDelegate<T>(T item);
}
