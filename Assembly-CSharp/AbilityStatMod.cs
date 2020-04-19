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
		return (AbilityStatMod)base.MemberwiseClone();
	}

	public override string ToString()
	{
		return string.Concat(new string[]
		{
			"[",
			this.stat.ToString(),
			"] ",
			this.modType.ToString(),
			" ",
			this.modValue.ToString()
		});
	}

	public string GetOperationsString()
	{
		char c;
		if (this.modType == ModType.Multiplier)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityStatMod.GetOperationsString()).MethodHandle;
			}
			c = 'x';
		}
		else if (this.modValue >= 0f)
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
			c = '+';
		}
		else
		{
			c = '-';
		}
		string text;
		if (this.modType != ModType.BaseAdd)
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
			if (this.modType != ModType.BonusAdd)
			{
				text = this.modValue.ToString("F2");
				goto IL_97;
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
		text = ((int)Mathf.Abs(this.modValue)).ToString();
		IL_97:
		char c2;
		if (this.modType == ModType.BonusAdd)
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
			c2 = '^';
		}
		else if (this.modType == ModType.BaseAdd)
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
			c2 = '_';
		}
		else if (this.modType == ModType.PercentAdd)
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
			c2 = 'x';
		}
		else
		{
			c2 = '\0';
		}
		return string.Format("{0} {1}{2}{3}", new object[]
		{
			this.stat.ToString(),
			c,
			text,
			c2
		});
	}

	public string GetInEditorDescription(string header = "- StatMod -", string indent = "", bool showDiff = false, AbilityStatMod other = null)
	{
		bool flag;
		if (showDiff)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityStatMod.GetInEditorDescription(string, string, bool, AbilityStatMod)).MethodHandle;
			}
			flag = (other != null);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		string text = "\t        \t | in base  =";
		string text2 = "\n";
		string text3 = InEditorDescHelper.BoldedStirng(header) + " " + this.modType.ToString() + text2;
		string str = text3;
		string header2 = "[ " + this.stat.ToString() + " ] = ";
		string otherSep = text;
		float myVal = this.modValue;
		bool showOther = flag2;
		float otherVal;
		if (flag2)
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
			otherVal = other.modValue;
		}
		else
		{
			otherVal = 0f;
		}
		if (AbilityStatMod.<>f__am$cache0 == null)
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
			AbilityStatMod.<>f__am$cache0 = ((float f) => f != 0f);
		}
		text3 = str + InEditorDescHelper.AssembleFieldWithDiff(header2, indent, otherSep, myVal, showOther, otherVal, AbilityStatMod.<>f__am$cache0);
		return text3 + text2;
	}
}
