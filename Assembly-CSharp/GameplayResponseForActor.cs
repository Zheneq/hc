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

	public virtual void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject subject)
	{
		if (m_damage != 0)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Damage, subject, m_damage));
		}
		if (m_healing != 0)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Healing, subject, m_healing));
		}
		m_effect.ReportAbilityTooltipNumbers(ref numbers, subject);
	}

	public GameplayResponseForActor GetShallowCopy()
	{
		return (GameplayResponseForActor)MemberwiseClone();
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens, string name, bool addCompare, GameplayResponseForActor other)
	{
		int num;
		if (addCompare)
		{
			num = ((other != null) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		AbilityMod.AddToken_IntDiff(tokens, name + "_Damage", "damage on response", m_damage, flag, flag ? other.m_damage : 0);
		string name2 = name + "_Healing";
		int healing = m_healing;
		int otherVal;
		if (flag)
		{
			otherVal = other.m_healing;
		}
		else
		{
			otherVal = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name2, "healing on response", healing, flag, otherVal);
		StandardEffectInfo effect = m_effect;
		string tokenName = name + "_Effect";
		object baseVal;
		if (flag)
		{
			baseVal = other.m_effect;
		}
		else
		{
			baseVal = null;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effect, tokenName, (StandardEffectInfo)baseVal);
	}

	public string GetInEditorDescription(string header = "- Response -", string indent = "", bool showDiff = false, GameplayResponseForActor other = null)
	{
		bool flag = showDiff && other != null;
		string otherSep = "\t        \t | in base  =";
		string text = "\n";
		string text2 = text + InEditorDescHelper.BoldedStirng(header) + text;
		string str = text2;
		float myVal = m_credits;
		int num;
		if (flag)
		{
			num = other.m_credits;
		}
		else
		{
			num = 0;
		}
		float otherVal = num;
		
		text2 = str + InEditorDescHelper.AssembleFieldWithDiff("[ Credits ] = ", indent, otherSep, myVal, flag, otherVal, ((float f) => f != 0f));
		string str2 = text2;
		float myVal2 = m_healing;
		int num2;
		if (flag)
		{
			num2 = other.m_healing;
		}
		else
		{
			num2 = 0;
		}
		text2 = str2 + InEditorDescHelper.AssembleFieldWithDiff("[ Healing ] = ", indent, otherSep, myVal2, flag, num2);
		string str3 = text2;
		float myVal3 = m_damage;
		int num3;
		if (flag)
		{
			num3 = other.m_damage;
		}
		else
		{
			num3 = 0;
		}
		text2 = str3 + InEditorDescHelper.AssembleFieldWithDiff("[ Damage ] = ", indent, otherSep, myVal3, flag, num3);
		string str4 = text2;
		float myVal4 = m_techPoints;
		int num4;
		if (flag)
		{
			num4 = other.m_techPoints;
		}
		else
		{
			num4 = 0;
		}
		float otherVal2 = num4;
		
		text2 = str4 + InEditorDescHelper.AssembleFieldWithDiff("[ TechPoints ] = ", indent, otherSep, myVal4, flag, otherVal2, ((float f) => f != 0f));
		string str5 = text2;
		StandardEffectInfo effect = m_effect;
		object baseVal;
		if (flag)
		{
			baseVal = other.m_effect;
		}
		else
		{
			baseVal = null;
		}
		text2 = str5 + AbilityModHelper.GetModEffectInfoDesc(effect, "{ Effect on Moved-Through }", indent, flag, (StandardEffectInfo)baseVal);
		string str6 = text2;
		AbilityStatMod[] permanentStatMods = m_permanentStatMods;
		object otherObjList;
		if (flag)
		{
			otherObjList = other.m_permanentStatMods;
		}
		else
		{
			otherObjList = null;
		}
		text2 = str6 + InEditorDescHelper.GetListDiffString("Permanent Stat Mods:", indent, permanentStatMods, flag, (AbilityStatMod[])otherObjList);
		string str7 = text2;
		StatusType[] permanentStatusChanges = m_permanentStatusChanges;
		object otherObjList2;
		if (flag)
		{
			otherObjList2 = other.m_permanentStatusChanges;
		}
		else
		{
			otherObjList2 = null;
		}
		text2 = str7 + InEditorDescHelper.GetListDiffString("Permanent Status Changes:", indent, permanentStatusChanges, flag, (StatusType[])otherObjList2);
		string str8 = text2;
		GameObject sequenceToPlay = m_sequenceToPlay;
		object otherVal3;
		if (flag)
		{
			otherVal3 = other.m_sequenceToPlay;
		}
		else
		{
			otherVal3 = null;
		}
		text2 = str8 + InEditorDescHelper.AssembleFieldWithDiff("Response Hit Sequence", indent, otherSep, sequenceToPlay, flag, (GameObject)otherVal3);
		return text2 + indent + "-- END of Move-Through Response Output --\n";
	}

	public bool HasResponse()
	{
		bool flag = false;
		flag |= m_effect.m_applyEffect;
		flag |= (m_credits != 0);
		flag |= (m_healing != 0);
		flag |= (m_damage != 0);
		flag |= (m_techPoints != 0);
		flag |= (m_permanentStatMods != null && m_permanentStatMods.Length > 0);
		bool num = flag;
		int num2;
		if (m_permanentStatusChanges != null)
		{
			num2 = ((m_permanentStatusChanges.Length > 0) ? 1 : 0);
		}
		else
		{
			num2 = 0;
		}
		flag = ((byte)((num ? 1 : 0) | num2) != 0);
		return flag | (m_sequenceToPlay != null);
	}
}
