using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

[Serializable]
public class StandardActorEffectData
{
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

	public StandardActorEffectData.InvisibilityBreakMode m_invisBreakMode;

	public bool m_removeInvisibilityOnLastResolveStart;

	[Space(5f)]
	public bool m_removeRevealedOnLastResolveStart;

	public AbilityStatMod[] m_statMods;

	public StatusType[] m_statusChanges;

	public StandardActorEffectData.StatusDelayMode m_statusDelayMode;

	public EffectEndTag[] m_endTriggers;

	public GameObject[] m_sequencePrefabs;

	public GameObject m_tickSequencePrefab;

	[CompilerGenerated]
	private static InEditorDescHelper.GetListEntryStrDelegate<GameObject> f__mg_cache0;

	public void InitWithDefaultValues()
	{
		this.SetValues(string.Empty, 1, 0, 0, 0, ServerCombatManager.HealingType.Effect, 0, 0, new AbilityStatMod[0], new StatusType[0], StandardActorEffectData.StatusDelayMode.DefaultBehavior);
	}

	public void SetValues(string effectName, int duration, int maxStackSize, int damagePerTurn, int healingPerTurn, ServerCombatManager.HealingType healingType, int perTurnHitDelayTurns, int absorbAmount, AbilityStatMod[] statMods, StatusType[] statusChanges, StandardActorEffectData.StatusDelayMode statusDelayMode)
	{
		this.m_effectName = effectName;
		this.m_duration = duration;
		this.m_maxStackSize = maxStackSize;
		this.m_damagePerTurn = damagePerTurn;
		this.m_healingPerTurn = healingPerTurn;
		this.m_healingType = healingType;
		this.m_perTurnHitDelayTurns = perTurnHitDelayTurns;
		this.m_absorbAmount = absorbAmount;
		this.m_statMods = statMods;
		this.m_statusChanges = statusChanges;
		this.m_statusDelayMode = statusDelayMode;
		this.m_endTriggers = new EffectEndTag[0];
		this.m_sequencePrefabs = new GameObject[0];
	}

	public unsafe virtual void ReportAbilityTooltipNumbers(ref List<AbilityTooltipNumber> numbers, AbilityTooltipSubject subject)
	{
		if (this.m_absorbAmount != 0)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Absorb, subject, this.m_absorbAmount));
		}
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> entries, string name, bool addCompare = false, StandardActorEffectData other = null)
	{
		bool flag = addCompare && other != null;
		string name2 = new StringBuilder().Append(name).Append("_Duration").ToString();
		string desc = new StringBuilder().Append("duration of ").Append(name).ToString();
		int duration = this.m_duration;
		bool addDiff = flag;
		int otherVal;
		if (flag)
		{
			otherVal = other.m_duration;
		}
		else
		{
			otherVal = 0;
		}
		AbilityMod.AddToken_IntDiff(entries, name2, desc, duration, addDiff, otherVal);
		if (this.m_duration > 1)
		{
			AbilityMod.AddToken_IntDiff(entries, new StringBuilder().Append(name).Append("_Duration_MinusOne").ToString(), "duration - 1", this.m_duration - 1, false, 0);
		}
		string name3 = new StringBuilder().Append(name).Append("_Shield").ToString();
		string desc2 = "shield amount";
		int absorbAmount = this.m_absorbAmount;
		bool addDiff2 = flag;
		int otherVal2;
		if (flag)
		{
			otherVal2 = other.m_absorbAmount;
		}
		else
		{
			otherVal2 = 0;
		}
		AbilityMod.AddToken_IntDiff(entries, name3, desc2, absorbAmount, addDiff2, otherVal2);
		AbilityMod.AddToken_IntDiff(entries, new StringBuilder().Append(name).Append("_Delayed_Shield").ToString(), "delayed shield amount", this.m_nextTurnAbsorbAmount, flag, (!flag) ? 0 : other.m_nextTurnAbsorbAmount);
		string name4 = new StringBuilder().Append(name).Append("_HealPerTurn").ToString();
		string desc3 = "healing per turn";
		int healingPerTurn = this.m_healingPerTurn;
		bool addDiff3 = flag;
		int otherVal3;
		if (flag)
		{
			otherVal3 = other.m_healingPerTurn;
		}
		else
		{
			otherVal3 = 0;
		}
		AbilityMod.AddToken_IntDiff(entries, name4, desc3, healingPerTurn, addDiff3, otherVal3);
		string name5 = new StringBuilder().Append(name).Append("_DamagePerTurn").ToString();
		string desc4 = "damage per turn";
		int damagePerTurn = this.m_damagePerTurn;
		bool addDiff4 = flag;
		int otherVal4;
		if (flag)
		{
			otherVal4 = other.m_damagePerTurn;
		}
		else
		{
			otherVal4 = 0;
		}
		AbilityMod.AddToken_IntDiff(entries, name5, desc4, damagePerTurn, addDiff4, otherVal4);
	}

	public StandardActorEffectData GetShallowCopy()
	{
		return (StandardActorEffectData)base.MemberwiseClone();
	}

	public string GetEffectName()
	{
		if (this.m_effectName != null)
		{
			if (this.m_effectName.Length > 0)
			{
				return this.m_effectName;
			}
		}
		return "No_Name";
	}

	public string GetInEditorDescription(string initialIndent = "", bool showDivider = true, bool diff = false, StandardActorEffectData other = null)
	{
		bool flag;
		if (diff)
		{
			flag = (other != null);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		string text = new StringBuilder().Append(initialIndent).Append("    ").ToString();
		string text2 = "\n";
		string text3 = "\t        \t | in base  =";
		string effectName = this.GetEffectName();
		string text4 = new StringBuilder().Append(initialIndent).Append("Effect [ ").Append(effectName).Append(" ]").ToString();
		if (flag2)
		{
			if (effectName != other.GetEffectName())
			{
				text4 += this.DiffColorStr(new StringBuilder().Append(text3).Append("Effect [ ").Append(other.GetEffectName()).Append(" ]").ToString());
			}
		}
		text4 += text2;
		string text5;
		if (this.m_duration > 0)
		{
			text5 = new StringBuilder().Append("Duration: ").Append(InEditorDescHelper.ColoredString(this.m_duration.ToString(), "cyan", false)).Append(" turn(s).").ToString();
		}
		else
		{
			text5 = "<color=white>WARNING: IS PERMANENT on Target (duration <= 0). Woof Woof Woof Woof</color>";
		}
		string text6 = text5;
		if (flag2)
		{
			if (this.m_duration != other.m_duration)
			{
				text6 += this.DiffColorStr(new StringBuilder().Append(text3).Append(other.m_duration).ToString());
			}
		}

		text4 = new StringBuilder().Append(text4).Append(text).Append(text6).Append(text2).ToString();
		string str = text4;
		string header = "Max Sack Size = ";
		string indent = text;
		string otherSep = text3;
		int maxStackSize = this.m_maxStackSize;
		bool showOther = flag2;
		int otherVal;
		if (flag2)
		{
			otherVal = other.m_maxStackSize;
		}
		else
		{
			otherVal = 0;
		}

		text4 = new StringBuilder().Append(str).Append(this.AssembleFieldWithDiff(header, indent, otherSep, maxStackSize, showOther, otherVal)).ToString();
		string str2 = text4;
		string header2 = "Damage Per Turn (start of Combat) = ";
		string indent2 = text;
		string otherSep2 = text3;
		int damagePerTurn = this.m_damagePerTurn;
		bool showOther2 = flag2;
		int otherVal2;
		if (flag2)
		{
			otherVal2 = other.m_damagePerTurn;
		}
		else
		{
			otherVal2 = 0;
		}

		text4 = new StringBuilder().Append(str2).Append(this.AssembleFieldWithDiff(header2, indent2, otherSep2, damagePerTurn, showOther2, otherVal2)).ToString();
		string str3 = text4;
		string header3 = "Healing Per Turn (start of Combat) = ";
		string indent3 = text;
		string otherSep3 = text3;
		int healingPerTurn = this.m_healingPerTurn;
		bool showOther3 = flag2;
		int otherVal3;
		if (flag2)
		{
			otherVal3 = other.m_healingPerTurn;
		}
		else
		{
			otherVal3 = 0;
		}

		text4 = new StringBuilder().Append(str3).Append(this.AssembleFieldWithDiff(header3, indent3, otherSep3, healingPerTurn, showOther3, otherVal3)).ToString();
		string str4 = text4;
		string header4 = "Damage/Healing Per Turn Delay = ";
		string indent4 = text;
		string otherSep4 = text3;
		int perTurnHitDelayTurns = this.m_perTurnHitDelayTurns;
		bool showOther4 = flag2;
		int otherVal4;
		if (flag2)
		{
			otherVal4 = other.m_perTurnHitDelayTurns;
		}
		else
		{
			otherVal4 = 0;
		}

		text4 = new StringBuilder().Append(str4).Append(this.AssembleFieldWithDiff(header4, indent4, otherSep4, perTurnHitDelayTurns, showOther4, otherVal4)).ToString();
		string str5 = text4;
		string header5 = "Absorb(shield) = ";
		string indent5 = text;
		string otherSep5 = text3;
		int absorbAmount = this.m_absorbAmount;
		bool showOther5 = flag2;
		int otherVal5;
		if (flag2)
		{
			otherVal5 = other.m_absorbAmount;
		}
		else
		{
			otherVal5 = 0;
		}

		text4 = new StringBuilder().Append(str5).Append(this.AssembleFieldWithDiff(header5, indent5, otherSep5, absorbAmount, showOther5, otherVal5)).ToString();
		string str6 = text4;
		string header6 = "Absorb(shield) applied next turn = ";
		string indent6 = text;
		string otherSep6 = text3;
		int nextTurnAbsorbAmount = this.m_nextTurnAbsorbAmount;
		bool showOther6 = flag2;
		int otherVal6;
		if (flag2)
		{
			otherVal6 = other.m_nextTurnAbsorbAmount;
		}
		else
		{
			otherVal6 = 0;
		}

		text4 = new StringBuilder().Append(str6).Append(this.AssembleFieldWithDiff(header6, indent6, otherSep6, nextTurnAbsorbAmount, showOther6, otherVal6)).ToString();
		string str7 = text4;
		string header7 = "TechPoint Change on Apply = ";
		string indent7 = text;
		string otherSep7 = text3;
		int techPointChangeOnStart = this.m_techPointChangeOnStart;
		bool showOther7 = flag2;
		int otherVal7;
		if (flag2)
		{
			otherVal7 = other.m_techPointChangeOnStart;
		}
		else
		{
			otherVal7 = 0;
		}

		text4 = new StringBuilder().Append(str7).Append(this.AssembleFieldWithDiff(header7, indent7, otherSep7, techPointChangeOnStart, showOther7, otherVal7)).ToString();
		string str8 = text4;
		string header8 = "TechPoint Gain Per Turn = ";
		string indent8 = text;
		string otherSep8 = text3;
		int techPointGainPerTurn = this.m_techPointGainPerTurn;
		bool showOther8 = flag2;
		int otherVal8;
		if (flag2)
		{
			otherVal8 = other.m_techPointGainPerTurn;
		}
		else
		{
			otherVal8 = 0;
		}

		text4 = new StringBuilder().Append(str8).Append(this.AssembleFieldWithDiff(header8, indent8, otherSep8, techPointGainPerTurn, showOther8, otherVal8)).ToString();
		text4 += this.AssembleFieldWithDiff("TechPoint Loss Per Turn = ", text, text3, this.m_techPointLossPerTurn, flag2, (!flag2) ? 0 : other.m_techPointLossPerTurn);
		string str9 = text4;
		string header9 = "Remove Invisibility On End of Decision of Last Turn = ";
		string indent9 = text;
		string otherSep9 = text3;
		bool removeInvisibilityOnLastResolveStart = this.m_removeInvisibilityOnLastResolveStart;
		bool showOther9 = flag2;
		bool otherVal9;
		if (flag2)
		{
			otherVal9 = other.m_removeInvisibilityOnLastResolveStart;
		}
		else
		{
			otherVal9 = false;
		}

		text4 = new StringBuilder().Append(str9).Append(InEditorDescHelper.AssembleFieldWithDiff(header9, indent9, otherSep9, removeInvisibilityOnLastResolveStart, showOther9, otherVal9, ((bool b) => b))).ToString();
		string str10 = text4;
		string header10 = "Remove Revealed On End of Decision of Last Turn = ";
		string indent10 = text;
		string otherSep10 = text3;
		bool removeRevealedOnLastResolveStart = this.m_removeRevealedOnLastResolveStart;
		bool showOther10 = flag2;
		bool otherVal10;
		if (flag2)
		{
			otherVal10 = other.m_removeRevealedOnLastResolveStart;
		}
		else
		{
			otherVal10 = false;
		}

		text4 = new StringBuilder().Append(str10).Append(InEditorDescHelper.AssembleFieldWithDiff(header10, indent10, otherSep10, removeRevealedOnLastResolveStart, showOther10, otherVal10, ((bool b) => b))).ToString();
		string str11 = text4;
		string header11 = "Damage Per Movement Square = ";
		string indent11 = text;
		string otherSep11 = text3;
		int damagePerMoveSquare = this.m_damagePerMoveSquare;
		bool showOther11 = flag2;
		int otherVal11;
		if (flag2)
		{
			otherVal11 = other.m_damagePerMoveSquare;
		}
		else
		{
			otherVal11 = 0;
		}

		text4 = new StringBuilder().Append(str11).Append(this.AssembleFieldWithDiff(header11, indent11, otherSep11, damagePerMoveSquare, showOther11, otherVal11)).ToString();
		text4 += this.AssembleFieldWithDiff("Healing Per Movement Square = ", text, text3, this.m_healPerMoveSquare, flag2, (!flag2) ? 0 : other.m_healPerMoveSquare);
		text4 += this.AssembleFieldWithDiff("Tech Point Loss Per Movement Square = ", text, text3, this.m_techPointLossPerMoveSquare, flag2, (!flag2) ? 0 : other.m_techPointLossPerMoveSquare);
		string str12 = text4;
		string header12 = "Tech Point Gain Per Movement Square = ";
		string indent12 = text;
		string otherSep12 = text3;
		int techPointGainPerMoveSquare = this.m_techPointGainPerMoveSquare;
		bool showOther12 = flag2;
		int otherVal12;
		if (flag2)
		{
			otherVal12 = other.m_techPointGainPerMoveSquare;
		}
		else
		{
			otherVal12 = 0;
		}

		text4 = new StringBuilder().Append(str12).Append(this.AssembleFieldWithDiff(header12, indent12, otherSep12, techPointGainPerMoveSquare, showOther12, otherVal12)).ToString();
		string str13 = text4;
		string header13 = "Stat Mods:";
		string indent13 = text;
		AbilityStatMod[] statMods = this.m_statMods;
		bool showDiff = flag2;
		AbilityStatMod[] otherObjList;
		if (flag2)
		{
			otherObjList = other.m_statMods;
		}
		else
		{
			otherObjList = null;
		}

		text4 = new StringBuilder().Append(str13).Append(InEditorDescHelper.GetListDiffString<AbilityStatMod>(header13, indent13, statMods, showDiff, otherObjList, null)).ToString();
		string str14 = text4;
		string header14 = "Status Changes:";
		string indent14 = text;
		StatusType[] statusChanges = this.m_statusChanges;
		bool showDiff2 = flag2;
		StatusType[] otherObjList2;
		if (flag2)
		{
			otherObjList2 = other.m_statusChanges;
		}
		else
		{
			otherObjList2 = null;
		}

		text4 = new StringBuilder().Append(str14).Append(InEditorDescHelper.GetListDiffString<StatusType>(header14, indent14, statusChanges, showDiff2, otherObjList2, null)).ToString();
		if (flag2)
		{
			if (this.m_statusDelayMode != other.m_statusDelayMode)
			{
				goto IL_572;
			}
		}
		if (this.m_statusDelayMode == StandardActorEffectData.StatusDelayMode.DefaultBehavior)
		{
			goto IL_5B4;
		}
		IL_572:
		string str15 = text4;
		string header15 = "Status Delay: ";
		string indent15 = text;
		string otherSep13 = text3;
		Enum myVal = this.m_statusDelayMode;
		bool showOther13 = flag2;
		StandardActorEffectData.StatusDelayMode statusDelayMode;
		if (flag2)
		{
			statusDelayMode = other.m_statusDelayMode;
		}
		else
		{
			statusDelayMode = this.m_statusDelayMode;
		}

		text4 = new StringBuilder().Append(str15).Append(InEditorDescHelper.AssembleFieldWithDiff(header15, indent15, otherSep13, myVal, showOther13, statusDelayMode)).ToString();
		IL_5B4:
		string str16 = text4;
		string header16 = "End Triggers:";
		string indent16 = text;
		EffectEndTag[] endTriggers = this.m_endTriggers;
		bool showDiff3 = flag2;
		EffectEndTag[] otherObjList3;
		if (flag2)
		{
			otherObjList3 = other.m_endTriggers;
		}
		else
		{
			otherObjList3 = null;
		}

		text4 = new StringBuilder().Append(str16).Append(InEditorDescHelper.GetListDiffString<EffectEndTag>(header16, indent16, endTriggers, showDiff3, otherObjList3, null)).ToString();
		string str17 = text4;
		string header17 = "Effect Sequence Prefabs:\t";
		string indent17 = text;
		GameObject[] sequencePrefabs = this.m_sequencePrefabs;
		bool showDiff4 = flag2;
		GameObject[] otherObjList4;
		if (flag2)
		{
			otherObjList4 = other.m_sequencePrefabs;
		}
		else
		{
			otherObjList4 = null;
		}

		text4 = new StringBuilder().Append(str17).Append(InEditorDescHelper.GetListDiffString<GameObject>(header17, indent17, sequencePrefabs, showDiff4, otherObjList4, new InEditorDescHelper.GetListEntryStrDelegate<GameObject>(InEditorDescHelper.GetGameObjectEntryStr))).ToString();
		string str18 = text4;
		string header18 = "Tick Sequence Prefab:";
		string indent18 = text;
		string otherSep14 = text3;
		GameObject tickSequencePrefab = this.m_tickSequencePrefab;
		bool showOther14 = flag2;
		GameObject otherVal13;
		if (flag2)
		{
			otherVal13 = other.m_tickSequencePrefab;
		}
		else
		{
			otherVal13 = null;
		}

		text4 = new StringBuilder().Append(str18).Append(InEditorDescHelper.AssembleFieldWithDiff(header18, indent18, otherSep14, tickSequencePrefab, showOther14, otherVal13)).ToString();
		if (showDivider)
		{
			text4 = new StringBuilder().Append(text4).Append(initialIndent).Append("-  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -\n").ToString();
		}
		return text4;
	}

	private string DiffColorStr(string input)
	{
		return InEditorDescHelper.ColoredString(input, "orange", false);
	}

	private string AssembleFieldWithDiff(string header, string indent, string otherSep, int myVal, bool showOther, int otherVal)
	{
		return InEditorDescHelper.AssembleFieldWithDiff(header, indent, otherSep, (float)myVal, showOther, (float)otherVal, null);
	}

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
}
