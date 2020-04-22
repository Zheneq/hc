using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class StandardActorEffectData
{
	public enum InvisibilityBreakMode
	{
		RemoveInvisAndEndEarly,
		SuppressOnly
	}

	public enum StatusDelayMode
	{
		DefaultBehavior,
		AllStatusesDelayToTurnStart,
		NoStatusesDelayToTurnStart
	}

	public string m_effectName;

	public int m_duration = 1;

	public int m_maxStackSize;

	[Space(5f)]
	public int m_damagePerTurn;

	public int m_healingPerTurn;

	public ServerCombatManager.HealingType m_healingType = ServerCombatManager.HealingType.Effect;

	[Space(5f)]
	public int m_perTurnHitDelayTurns;

	[Space(5f)]
	public int m_absorbAmount;

	public int m_nextTurnAbsorbAmount;

	public bool m_dontEndEarlyOnShieldDeplete;

	[Space(5f)]
	public int m_damagePerMoveSquare;

	public int m_healPerMoveSquare;

	public int m_techPointLossPerMoveSquare;

	public int m_techPointGainPerMoveSquare;

	[Space(5f)]
	public int m_techPointChangeOnStart;

	public int m_techPointGainPerTurn;

	public int m_techPointLossPerTurn;

	public InvisibilityBreakMode m_invisBreakMode;

	public bool m_removeInvisibilityOnLastResolveStart;

	[Space(5f)]
	public bool m_removeRevealedOnLastResolveStart;

	public AbilityStatMod[] m_statMods;

	public StatusType[] m_statusChanges;

	public StatusDelayMode m_statusDelayMode;

	public EffectEndTag[] m_endTriggers;

	public GameObject[] m_sequencePrefabs;

	public GameObject m_tickSequencePrefab;

	[CompilerGenerated]
	private static InEditorDescHelper.GetListEntryStrDelegate<GameObject> _003C_003Ef__mg_0024cache0;

	public void InitWithDefaultValues()
	{
		SetValues(string.Empty, 1, 0, 0, 0, ServerCombatManager.HealingType.Effect, 0, 0, new AbilityStatMod[0], new StatusType[0], StatusDelayMode.DefaultBehavior);
	}

	public void SetValues(string effectName, int duration, int maxStackSize, int damagePerTurn, int healingPerTurn, ServerCombatManager.HealingType healingType, int perTurnHitDelayTurns, int absorbAmount, AbilityStatMod[] statMods, StatusType[] statusChanges, StatusDelayMode statusDelayMode)
	{
		m_effectName = effectName;
		m_duration = duration;
		m_maxStackSize = maxStackSize;
		m_damagePerTurn = damagePerTurn;
		m_healingPerTurn = healingPerTurn;
		m_healingType = healingType;
		m_perTurnHitDelayTurns = perTurnHitDelayTurns;
		m_absorbAmount = absorbAmount;
		m_statMods = statMods;
		m_statusChanges = statusChanges;
		m_statusDelayMode = statusDelayMode;
		m_endTriggers = new EffectEndTag[0];
		m_sequencePrefabs = new GameObject[0];
	}

	public virtual void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject subject)
	{
		if (m_absorbAmount == 0)
		{
			return;
		}
		while (true)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Absorb, subject, m_absorbAmount));
			return;
		}
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> entries, string name, bool addCompare = false, StandardActorEffectData other = null)
	{
		bool flag = addCompare && other != null;
		string name2 = name + "_Duration";
		string desc = "duration of " + name;
		int duration = m_duration;
		int otherVal;
		if (flag)
		{
			otherVal = other.m_duration;
		}
		else
		{
			otherVal = 0;
		}
		AbilityMod.AddToken_IntDiff(entries, name2, desc, duration, flag, otherVal);
		if (m_duration > 1)
		{
			AbilityMod.AddToken_IntDiff(entries, name + "_Duration_MinusOne", "duration - 1", m_duration - 1, false, 0);
		}
		string name3 = name + "_Shield";
		int absorbAmount = m_absorbAmount;
		int otherVal2;
		if (flag)
		{
			otherVal2 = other.m_absorbAmount;
		}
		else
		{
			otherVal2 = 0;
		}
		AbilityMod.AddToken_IntDiff(entries, name3, "shield amount", absorbAmount, flag, otherVal2);
		AbilityMod.AddToken_IntDiff(entries, name + "_Delayed_Shield", "delayed shield amount", m_nextTurnAbsorbAmount, flag, flag ? other.m_nextTurnAbsorbAmount : 0);
		string name4 = name + "_HealPerTurn";
		int healingPerTurn = m_healingPerTurn;
		int otherVal3;
		if (flag)
		{
			otherVal3 = other.m_healingPerTurn;
		}
		else
		{
			otherVal3 = 0;
		}
		AbilityMod.AddToken_IntDiff(entries, name4, "healing per turn", healingPerTurn, flag, otherVal3);
		string name5 = name + "_DamagePerTurn";
		int damagePerTurn = m_damagePerTurn;
		int otherVal4;
		if (flag)
		{
			otherVal4 = other.m_damagePerTurn;
		}
		else
		{
			otherVal4 = 0;
		}
		AbilityMod.AddToken_IntDiff(entries, name5, "damage per turn", damagePerTurn, flag, otherVal4);
	}

	public StandardActorEffectData GetShallowCopy()
	{
		return (StandardActorEffectData)MemberwiseClone();
	}

	public string GetEffectName()
	{
		object result;
		if (m_effectName != null)
		{
			if (m_effectName.Length > 0)
			{
				result = m_effectName;
				goto IL_0040;
			}
		}
		result = "No_Name";
		goto IL_0040;
		IL_0040:
		return (string)result;
	}

	public string GetInEditorDescription(string initialIndent = "", bool showDivider = true, bool diff = false, StandardActorEffectData other = null)
	{
		int num;
		if (diff)
		{
			num = ((other != null) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		string text = initialIndent + "    ";
		string text2 = "\n";
		string text3 = "\t        \t | in base  =";
		string effectName = GetEffectName();
		string str = initialIndent + "Effect [ " + effectName + " ]";
		if (flag)
		{
			if (effectName != other.GetEffectName())
			{
				str += DiffColorStr(text3 + "Effect [ " + other.GetEffectName() + " ]");
			}
		}
		str += text2;
		object obj;
		if (m_duration > 0)
		{
			obj = "Duration: " + InEditorDescHelper.ColoredString(m_duration.ToString()) + " turn(s).";
		}
		else
		{
			obj = "<color=white>WARNING: IS PERMANENT on Target (duration <= 0). Woof Woof Woof Woof</color>";
		}
		string text4 = (string)obj;
		if (flag)
		{
			if (m_duration != other.m_duration)
			{
				text4 += DiffColorStr(text3 + other.m_duration);
			}
		}
		str = str + text + text4 + text2;
		string str2 = str;
		int maxStackSize = m_maxStackSize;
		int otherVal;
		if (flag)
		{
			otherVal = other.m_maxStackSize;
		}
		else
		{
			otherVal = 0;
		}
		str = str2 + AssembleFieldWithDiff("Max Sack Size = ", text, text3, maxStackSize, flag, otherVal);
		string str3 = str;
		int damagePerTurn = m_damagePerTurn;
		int otherVal2;
		if (flag)
		{
			otherVal2 = other.m_damagePerTurn;
		}
		else
		{
			otherVal2 = 0;
		}
		str = str3 + AssembleFieldWithDiff("Damage Per Turn (start of Combat) = ", text, text3, damagePerTurn, flag, otherVal2);
		string str4 = str;
		int healingPerTurn = m_healingPerTurn;
		int otherVal3;
		if (flag)
		{
			otherVal3 = other.m_healingPerTurn;
		}
		else
		{
			otherVal3 = 0;
		}
		str = str4 + AssembleFieldWithDiff("Healing Per Turn (start of Combat) = ", text, text3, healingPerTurn, flag, otherVal3);
		string str5 = str;
		int perTurnHitDelayTurns = m_perTurnHitDelayTurns;
		int otherVal4;
		if (flag)
		{
			otherVal4 = other.m_perTurnHitDelayTurns;
		}
		else
		{
			otherVal4 = 0;
		}
		str = str5 + AssembleFieldWithDiff("Damage/Healing Per Turn Delay = ", text, text3, perTurnHitDelayTurns, flag, otherVal4);
		string str6 = str;
		int absorbAmount = m_absorbAmount;
		int otherVal5;
		if (flag)
		{
			otherVal5 = other.m_absorbAmount;
		}
		else
		{
			otherVal5 = 0;
		}
		str = str6 + AssembleFieldWithDiff("Absorb(shield) = ", text, text3, absorbAmount, flag, otherVal5);
		string str7 = str;
		int nextTurnAbsorbAmount = m_nextTurnAbsorbAmount;
		int otherVal6;
		if (flag)
		{
			otherVal6 = other.m_nextTurnAbsorbAmount;
		}
		else
		{
			otherVal6 = 0;
		}
		str = str7 + AssembleFieldWithDiff("Absorb(shield) applied next turn = ", text, text3, nextTurnAbsorbAmount, flag, otherVal6);
		string str8 = str;
		int techPointChangeOnStart = m_techPointChangeOnStart;
		int otherVal7;
		if (flag)
		{
			otherVal7 = other.m_techPointChangeOnStart;
		}
		else
		{
			otherVal7 = 0;
		}
		str = str8 + AssembleFieldWithDiff("TechPoint Change on Apply = ", text, text3, techPointChangeOnStart, flag, otherVal7);
		string str9 = str;
		int techPointGainPerTurn = m_techPointGainPerTurn;
		int otherVal8;
		if (flag)
		{
			otherVal8 = other.m_techPointGainPerTurn;
		}
		else
		{
			otherVal8 = 0;
		}
		str = str9 + AssembleFieldWithDiff("TechPoint Gain Per Turn = ", text, text3, techPointGainPerTurn, flag, otherVal8);
		str += AssembleFieldWithDiff("TechPoint Loss Per Turn = ", text, text3, m_techPointLossPerTurn, flag, flag ? other.m_techPointLossPerTurn : 0);
		string str10 = str;
		bool removeInvisibilityOnLastResolveStart = m_removeInvisibilityOnLastResolveStart;
		int otherVal9;
		if (flag)
		{
			otherVal9 = (other.m_removeInvisibilityOnLastResolveStart ? 1 : 0);
		}
		else
		{
			otherVal9 = 0;
		}
		if (_003C_003Ef__am_0024cache0 == null)
		{
			_003C_003Ef__am_0024cache0 = ((bool b) => b);
		}
		str = str10 + InEditorDescHelper.AssembleFieldWithDiff("Remove Invisibility On End of Decision of Last Turn = ", text, text3, removeInvisibilityOnLastResolveStart, flag, (byte)otherVal9 != 0, _003C_003Ef__am_0024cache0);
		string str11 = str;
		bool removeRevealedOnLastResolveStart = m_removeRevealedOnLastResolveStart;
		int otherVal10;
		if (flag)
		{
			otherVal10 = (other.m_removeRevealedOnLastResolveStart ? 1 : 0);
		}
		else
		{
			otherVal10 = 0;
		}
		if (_003C_003Ef__am_0024cache1 == null)
		{
			_003C_003Ef__am_0024cache1 = ((bool b) => b);
		}
		str = str11 + InEditorDescHelper.AssembleFieldWithDiff("Remove Revealed On End of Decision of Last Turn = ", text, text3, removeRevealedOnLastResolveStart, flag, (byte)otherVal10 != 0, _003C_003Ef__am_0024cache1);
		string str12 = str;
		int damagePerMoveSquare = m_damagePerMoveSquare;
		int otherVal11;
		if (flag)
		{
			otherVal11 = other.m_damagePerMoveSquare;
		}
		else
		{
			otherVal11 = 0;
		}
		str = str12 + AssembleFieldWithDiff("Damage Per Movement Square = ", text, text3, damagePerMoveSquare, flag, otherVal11);
		str += AssembleFieldWithDiff("Healing Per Movement Square = ", text, text3, m_healPerMoveSquare, flag, flag ? other.m_healPerMoveSquare : 0);
		str += AssembleFieldWithDiff("Tech Point Loss Per Movement Square = ", text, text3, m_techPointLossPerMoveSquare, flag, flag ? other.m_techPointLossPerMoveSquare : 0);
		string str13 = str;
		int techPointGainPerMoveSquare = m_techPointGainPerMoveSquare;
		int otherVal12;
		if (flag)
		{
			otherVal12 = other.m_techPointGainPerMoveSquare;
		}
		else
		{
			otherVal12 = 0;
		}
		str = str13 + AssembleFieldWithDiff("Tech Point Gain Per Movement Square = ", text, text3, techPointGainPerMoveSquare, flag, otherVal12);
		string str14 = str;
		AbilityStatMod[] statMods = m_statMods;
		object otherObjList;
		if (flag)
		{
			otherObjList = other.m_statMods;
		}
		else
		{
			otherObjList = null;
		}
		str = str14 + InEditorDescHelper.GetListDiffString("Stat Mods:", text, statMods, flag, (AbilityStatMod[])otherObjList);
		string str15 = str;
		StatusType[] statusChanges = m_statusChanges;
		object otherObjList2;
		if (flag)
		{
			otherObjList2 = other.m_statusChanges;
		}
		else
		{
			otherObjList2 = null;
		}
		str = str15 + InEditorDescHelper.GetListDiffString("Status Changes:", text, statusChanges, flag, (StatusType[])otherObjList2);
		if (flag)
		{
			if (m_statusDelayMode != other.m_statusDelayMode)
			{
				goto IL_0572;
			}
		}
		if (m_statusDelayMode != 0)
		{
			goto IL_0572;
		}
		goto IL_05b4;
		IL_0572:
		string str16 = str;
		object myVal = m_statusDelayMode;
		StatusDelayMode statusDelayMode;
		if (flag)
		{
			statusDelayMode = other.m_statusDelayMode;
		}
		else
		{
			statusDelayMode = m_statusDelayMode;
		}
		str = str16 + InEditorDescHelper.AssembleFieldWithDiff("Status Delay: ", text, text3, (Enum)myVal, flag, statusDelayMode);
		goto IL_05b4;
		IL_05b4:
		string str17 = str;
		EffectEndTag[] endTriggers = m_endTriggers;
		object otherObjList3;
		if (flag)
		{
			otherObjList3 = other.m_endTriggers;
		}
		else
		{
			otherObjList3 = null;
		}
		str = str17 + InEditorDescHelper.GetListDiffString("End Triggers:", text, endTriggers, flag, (EffectEndTag[])otherObjList3);
		string str18 = str;
		GameObject[] sequencePrefabs = m_sequencePrefabs;
		object otherObjList4;
		if (flag)
		{
			otherObjList4 = other.m_sequencePrefabs;
		}
		else
		{
			otherObjList4 = null;
		}
		if (_003C_003Ef__mg_0024cache0 == null)
		{
			_003C_003Ef__mg_0024cache0 = InEditorDescHelper.GetGameObjectEntryStr;
		}
		str = str18 + InEditorDescHelper.GetListDiffString("Effect Sequence Prefabs:\t", text, sequencePrefabs, flag, (GameObject[])otherObjList4, _003C_003Ef__mg_0024cache0);
		string str19 = str;
		GameObject tickSequencePrefab = m_tickSequencePrefab;
		object otherVal13;
		if (flag)
		{
			otherVal13 = other.m_tickSequencePrefab;
		}
		else
		{
			otherVal13 = null;
		}
		str = str19 + InEditorDescHelper.AssembleFieldWithDiff("Tick Sequence Prefab:", text, text3, tickSequencePrefab, flag, (GameObject)otherVal13);
		if (showDivider)
		{
			str = str + initialIndent + "-  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -\n";
		}
		return str;
	}

	private string DiffColorStr(string input)
	{
		return InEditorDescHelper.ColoredString(input, "orange");
	}

	private string AssembleFieldWithDiff(string header, string indent, string otherSep, int myVal, bool showOther, int otherVal)
	{
		return InEditorDescHelper.AssembleFieldWithDiff(header, indent, otherSep, myVal, showOther, otherVal);
	}
}
