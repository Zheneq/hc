using System;
using UnityEngine;

[Serializable]
public class AbilityStatMod
{
	public StatType stat;

	public ModType modType;

	public float modValue;

	public AbilityStatMod GetShallowCopy()
	{
		return (AbilityStatMod)MemberwiseClone();
	}

	public override string ToString()
	{
		return "[" + stat.ToString() + "] " + modType.ToString() + " " + modValue;
	}

	public string GetOperationsString()
	{
		char c;
		if (modType == ModType.Multiplier)
		{
			c = 'x';
		}
		else if (modValue >= 0f)
		{
			c = '+';
		}
		else
		{
			c = '-';
		}
		string text;
		if (modType != 0)
		{
			if (modType != ModType.BonusAdd)
			{
				text = modValue.ToString("F2");
				goto IL_0097;
			}
		}
		text = ((int)Mathf.Abs(modValue)).ToString();
		goto IL_0097;
		IL_0097:
		char c2;
		if (modType == ModType.BonusAdd)
		{
			c2 = '^';
		}
		else if (modType == ModType.BaseAdd)
		{
			c2 = '_';
		}
		else if (modType == ModType.PercentAdd)
		{
			c2 = 'x';
		}
		else
		{
			c2 = '\0';
		}
		return $"{stat.ToString()} {c}{text}{c2}";
	}

	public string GetInEditorDescription(string header = "- StatMod -", string indent = "", bool showDiff = false, AbilityStatMod other = null)
	{
		int num;
		if (showDiff)
		{
			num = ((other != null) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		string otherSep = "\t        \t | in base  =";
		string text = "\n";
		string text2 = InEditorDescHelper.BoldedStirng(header) + " " + modType.ToString() + text;
		string str = text2;
		string header2 = "[ " + stat.ToString() + " ] = ";
		float myVal = modValue;
		float otherVal;
		if (flag)
		{
			otherVal = other.modValue;
		}
		else
		{
			otherVal = 0f;
		}
		if (_003C_003Ef__am_0024cache0 == null)
		{
			_003C_003Ef__am_0024cache0 = ((float f) => f != 0f);
		}
		text2 = str + InEditorDescHelper.AssembleFieldWithDiff(header2, indent, otherSep, myVal, flag, otherVal, _003C_003Ef__am_0024cache0);
		return text2 + text;
	}
}
