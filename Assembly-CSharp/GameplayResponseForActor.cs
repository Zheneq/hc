using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GameplayResponseForActor
{
	public StandardEffectInfo m_effect;

	[Space(5f)]
	public int m_credits;

	public int m_healing;

	public int m_damage;

	public int m_techPoints;

	public AbilityStatMod[] m_permanentStatMods;

	public StatusType[] m_permanentStatusChanges;

	public GameObject m_sequenceToPlay;

	public unsafe virtual void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject subject)
	{
		if (this.m_damage != 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayResponseForActor.ReportAbilityTooltipNumbers(List<AbilityTooltipNumber>*, AbilityTooltipSubject)).MethodHandle;
			}
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, subject, this.m_damage));
		}
		if (this.m_healing != 0)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, subject, this.m_healing));
		}
		this.m_effect.ReportAbilityTooltipNumbers(ref numbers, subject);
	}

	public GameplayResponseForActor GetShallowCopy()
	{
		return (GameplayResponseForActor)base.MemberwiseClone();
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens, string name, bool addCompare, GameplayResponseForActor other)
	{
		bool flag;
		if (addCompare)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayResponseForActor.AddTooltipTokens(List<TooltipTokenEntry>, string, bool, GameplayResponseForActor)).MethodHandle;
			}
			flag = (other != null);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		AbilityMod.AddToken_IntDiff(tokens, name + "_Damage", "damage on response", this.m_damage, flag2, (!flag2) ? 0 : other.m_damage);
		string name2 = name + "_Healing";
		string desc = "healing on response";
		int healing = this.m_healing;
		bool addDiff = flag2;
		int otherVal;
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
			otherVal = other.m_healing;
		}
		else
		{
			otherVal = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name2, desc, healing, addDiff, otherVal);
		StandardEffectInfo effect = this.m_effect;
		string tokenName = name + "_Effect";
		StandardEffectInfo baseVal;
		if (flag2)
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
			baseVal = other.m_effect;
		}
		else
		{
			baseVal = null;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effect, tokenName, baseVal, true);
	}

	public string GetInEditorDescription(string header = "- Response -", string indent = "", bool showDiff = false, GameplayResponseForActor other = null)
	{
		bool flag = showDiff && other != null;
		string text = "\t        \t | in base  =";
		string text2 = "\n";
		string text3 = text2 + InEditorDescHelper.BoldedStirng(header) + text2;
		string str = text3;
		string header2 = "[ Credits ] = ";
		string otherSep = text;
		float myVal = (float)this.m_credits;
		bool showOther = flag;
		float num;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayResponseForActor.GetInEditorDescription(string, string, bool, GameplayResponseForActor)).MethodHandle;
			}
			num = (float)other.m_credits;
		}
		else
		{
			num = (float)0;
		}
		float otherVal = num;
		if (GameplayResponseForActor.<>f__am$cache0 == null)
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
			GameplayResponseForActor.<>f__am$cache0 = ((float f) => f != 0f);
		}
		text3 = str + InEditorDescHelper.AssembleFieldWithDiff(header2, indent, otherSep, myVal, showOther, otherVal, GameplayResponseForActor.<>f__am$cache0);
		string str2 = text3;
		string header3 = "[ Healing ] = ";
		string otherSep2 = text;
		float myVal2 = (float)this.m_healing;
		bool showOther2 = flag;
		float num2;
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
			num2 = (float)other.m_healing;
		}
		else
		{
			num2 = (float)0;
		}
		text3 = str2 + InEditorDescHelper.AssembleFieldWithDiff(header3, indent, otherSep2, myVal2, showOther2, num2, null);
		string str3 = text3;
		string header4 = "[ Damage ] = ";
		string otherSep3 = text;
		float myVal3 = (float)this.m_damage;
		bool showOther3 = flag;
		float num3;
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
			num3 = (float)other.m_damage;
		}
		else
		{
			num3 = (float)0;
		}
		text3 = str3 + InEditorDescHelper.AssembleFieldWithDiff(header4, indent, otherSep3, myVal3, showOther3, num3, null);
		string str4 = text3;
		string header5 = "[ TechPoints ] = ";
		string otherSep4 = text;
		float myVal4 = (float)this.m_techPoints;
		bool showOther4 = flag;
		float num4;
		if (flag)
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
			num4 = (float)other.m_techPoints;
		}
		else
		{
			num4 = (float)0;
		}
		float otherVal2 = num4;
		if (GameplayResponseForActor.<>f__am$cache1 == null)
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
			GameplayResponseForActor.<>f__am$cache1 = ((float f) => f != 0f);
		}
		text3 = str4 + InEditorDescHelper.AssembleFieldWithDiff(header5, indent, otherSep4, myVal4, showOther4, otherVal2, GameplayResponseForActor.<>f__am$cache1);
		string str5 = text3;
		StandardEffectInfo effect = this.m_effect;
		string prefix = "{ Effect on Moved-Through }";
		bool useBaseVal = flag;
		StandardEffectInfo baseVal;
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
			baseVal = other.m_effect;
		}
		else
		{
			baseVal = null;
		}
		text3 = str5 + AbilityModHelper.GetModEffectInfoDesc(effect, prefix, indent, useBaseVal, baseVal);
		string str6 = text3;
		string header6 = "Permanent Stat Mods:";
		AbilityStatMod[] permanentStatMods = this.m_permanentStatMods;
		bool showDiff2 = flag;
		AbilityStatMod[] otherObjList;
		if (flag)
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
			otherObjList = other.m_permanentStatMods;
		}
		else
		{
			otherObjList = null;
		}
		text3 = str6 + InEditorDescHelper.GetListDiffString<AbilityStatMod>(header6, indent, permanentStatMods, showDiff2, otherObjList, null);
		string str7 = text3;
		string header7 = "Permanent Status Changes:";
		StatusType[] permanentStatusChanges = this.m_permanentStatusChanges;
		bool showDiff3 = flag;
		StatusType[] otherObjList2;
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
			otherObjList2 = other.m_permanentStatusChanges;
		}
		else
		{
			otherObjList2 = null;
		}
		text3 = str7 + InEditorDescHelper.GetListDiffString<StatusType>(header7, indent, permanentStatusChanges, showDiff3, otherObjList2, null);
		string str8 = text3;
		string header8 = "Response Hit Sequence";
		string otherSep5 = text;
		GameObject sequenceToPlay = this.m_sequenceToPlay;
		bool showOther5 = flag;
		GameObject otherVal3;
		if (flag)
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
			otherVal3 = other.m_sequenceToPlay;
		}
		else
		{
			otherVal3 = null;
		}
		text3 = str8 + InEditorDescHelper.AssembleFieldWithDiff(header8, indent, otherSep5, sequenceToPlay, showOther5, otherVal3);
		return text3 + indent + "-- END of Move-Through Response Output --\n";
	}

	public bool HasResponse()
	{
		bool flag = false;
		flag |= this.m_effect.m_applyEffect;
		flag |= (this.m_credits != 0);
		flag |= (this.m_healing != 0);
		flag |= (this.m_damage != 0);
		flag |= (this.m_techPoints != 0);
		flag |= (this.m_permanentStatMods != null && this.m_permanentStatMods.Length > 0);
		bool flag2 = flag;
		bool flag3;
		if (this.m_permanentStatusChanges != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(GameplayResponseForActor.HasResponse()).MethodHandle;
			}
			flag3 = (this.m_permanentStatusChanges.Length > 0);
		}
		else
		{
			flag3 = false;
		}
		flag = (flag2 || flag3);
		return flag | this.m_sequenceToPlay != null;
	}
}
