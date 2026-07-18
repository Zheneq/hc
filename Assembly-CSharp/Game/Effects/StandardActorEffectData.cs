using System;
using System.Collections.Generic;
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
	public bool m_dontEndEarlyOnShieldDeplete; // TODO EFFECT unused
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

#if SERVER
	public static StandardActorEffectData MakeDefault()
	{
		StandardActorEffectData data = new StandardActorEffectData();
		data.InitWithDefaultValues();
		return data;
	}
#endif
	
	public void InitWithDefaultValues()
	{
		SetValues(
			string.Empty,
			1,
			0,
			0,
			0,
			ServerCombatManager.HealingType.Effect,
			0,
			0,
			new AbilityStatMod[0],
			new StatusType[0],
			StatusDelayMode.DefaultBehavior);
	}

	public void SetValues(
		string effectName,
		int duration,
		int maxStackSize,
		int damagePerTurn,
		int healingPerTurn,
		ServerCombatManager.HealingType healingType,
		int perTurnHitDelayTurns,
		int absorbAmount,
		AbilityStatMod[] statMods,
		StatusType[] statusChanges,
		StatusDelayMode statusDelayMode)
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
		if (m_absorbAmount != 0)
		{
			numbers.Add(new AbilityTooltipNumber(AbilityTooltipSymbol.Absorb, subject, m_absorbAmount));
		}
	}

	public void AddTooltipTokens(
		List<TooltipTokenEntry> entries,
		string name,
		bool addCompare = false,
		StandardActorEffectData other = null)
	{
		bool addDiff = addCompare && other != null;
		AbilityMod.AddToken_IntDiff(entries, name + "_Duration", "duration of " + name, m_duration, addDiff, addDiff ? other.m_duration : 0);
		if (m_duration > 1)
		{
			AbilityMod.AddToken_IntDiff(entries, name + "_Duration_MinusOne", "duration - 1", m_duration - 1, false, 0);
		}
		AbilityMod.AddToken_IntDiff(entries, name + "_Shield", "shield amount", m_absorbAmount, addDiff, addDiff ? other.m_absorbAmount : 0);
		AbilityMod.AddToken_IntDiff(entries, name + "_Delayed_Shield", "delayed shield amount", m_nextTurnAbsorbAmount, addDiff, addDiff ? other.m_nextTurnAbsorbAmount : 0);
		AbilityMod.AddToken_IntDiff(entries, name + "_HealPerTurn", "healing per turn", m_healingPerTurn, addDiff, addDiff ? other.m_healingPerTurn : 0);
		AbilityMod.AddToken_IntDiff(entries, name + "_DamagePerTurn", "damage per turn", m_damagePerTurn, addDiff, addDiff ? other.m_damagePerTurn : 0);
	}

	public StandardActorEffectData GetShallowCopy()
	{
		return (StandardActorEffectData)MemberwiseClone();
	}

	public string GetEffectName()
	{
		return m_effectName != null && m_effectName.Length > 0
			? m_effectName
			: "No_Name";
	}

	public string GetInEditorDescription(string initialIndent = "", bool showDivider = true, bool diff = false, StandardActorEffectData other = null)
	{
		bool showDiff = diff && other != null;
		string indent = initialIndent + "    ";
		string otherSep = "\t        \t | in base  =";
		string effectName = GetEffectName();
		string desc = initialIndent + "Effect [ " + effectName + " ]";
		if (showDiff && effectName != other.GetEffectName())
		{
			desc += DiffColorStr(otherSep + "Effect [ " + other.GetEffectName() + " ]");
		}
		desc += "\n";
		string durationDesc = m_duration > 0
			? "Duration: " + InEditorDescHelper.ColoredString(m_duration.ToString()) + " turn(s)."
			: "<color=white>WARNING: IS PERMANENT on Target (duration <= 0). Woof Woof Woof Woof</color>";
		if (showDiff && m_duration != other.m_duration)
		{
			durationDesc += DiffColorStr(otherSep + other.m_duration);
		}
		desc = desc + indent + durationDesc + "\n";
		desc += AssembleFieldWithDiff("Max Sack Size = ", indent, otherSep, m_maxStackSize, showDiff, showDiff ? other.m_maxStackSize : 0);
		desc += AssembleFieldWithDiff("Damage Per Turn (start of Combat) = ", indent, otherSep, m_damagePerTurn, showDiff, showDiff ? other.m_damagePerTurn : 0);
		desc += AssembleFieldWithDiff("Healing Per Turn (start of Combat) = ", indent, otherSep, m_healingPerTurn, showDiff, showDiff ? other.m_healingPerTurn : 0);
		desc += AssembleFieldWithDiff("Damage/Healing Per Turn Delay = ", indent, otherSep, m_perTurnHitDelayTurns, showDiff, showDiff ? other.m_perTurnHitDelayTurns : 0);
		desc += AssembleFieldWithDiff("Absorb(shield) = ", indent, otherSep, m_absorbAmount, showDiff, showDiff ? other.m_absorbAmount : 0);
		desc += AssembleFieldWithDiff("Absorb(shield) applied next turn = ", indent, otherSep, m_nextTurnAbsorbAmount, showDiff, showDiff ? other.m_nextTurnAbsorbAmount : 0);
		desc += AssembleFieldWithDiff("TechPoint Change on Apply = ", indent, otherSep, m_techPointChangeOnStart, showDiff, showDiff ? other.m_techPointChangeOnStart : 0);
		desc += AssembleFieldWithDiff("TechPoint Gain Per Turn = ", indent, otherSep, m_techPointGainPerTurn, showDiff, showDiff ? other.m_techPointGainPerTurn : 0);
		desc += AssembleFieldWithDiff("TechPoint Loss Per Turn = ", indent, otherSep, m_techPointLossPerTurn, showDiff, showDiff ? other.m_techPointLossPerTurn : 0);
		desc += InEditorDescHelper.AssembleFieldWithDiff("Remove Invisibility On End of Decision of Last Turn = ", indent, otherSep, m_removeInvisibilityOnLastResolveStart, showDiff, showDiff && other.m_removeInvisibilityOnLastResolveStart, b => b);
		desc += InEditorDescHelper.AssembleFieldWithDiff("Remove Revealed On End of Decision of Last Turn = ", indent, otherSep, m_removeRevealedOnLastResolveStart, showDiff, showDiff && other.m_removeRevealedOnLastResolveStart, b => b);
		desc += AssembleFieldWithDiff("Damage Per Movement Square = ", indent, otherSep, m_damagePerMoveSquare, showDiff, showDiff ? other.m_damagePerMoveSquare : 0);
		desc += AssembleFieldWithDiff("Healing Per Movement Square = ", indent, otherSep, m_healPerMoveSquare, showDiff, showDiff ? other.m_healPerMoveSquare : 0);
		desc += AssembleFieldWithDiff("Tech Point Loss Per Movement Square = ", indent, otherSep, m_techPointLossPerMoveSquare, showDiff, showDiff ? other.m_techPointLossPerMoveSquare : 0);
		desc += AssembleFieldWithDiff("Tech Point Gain Per Movement Square = ", indent, otherSep, m_techPointGainPerMoveSquare, showDiff, showDiff ? other.m_techPointGainPerMoveSquare : 0);
		desc += InEditorDescHelper.GetListDiffString("Stat Mods:", indent, m_statMods, showDiff, showDiff ? other.m_statMods : null);
		desc += InEditorDescHelper.GetListDiffString("Status Changes:", indent, m_statusChanges, showDiff, showDiff ? other.m_statusChanges : null);
		if ((showDiff && m_statusDelayMode != other.m_statusDelayMode)
		    || m_statusDelayMode != StatusDelayMode.DefaultBehavior)
		{
			desc += InEditorDescHelper.AssembleFieldWithDiff("Status Delay: ", indent, otherSep, m_statusDelayMode, showDiff, showDiff ? other.m_statusDelayMode : m_statusDelayMode);
		}

		desc += InEditorDescHelper.GetListDiffString("End Triggers:", indent, m_endTriggers, showDiff, showDiff ? other.m_endTriggers : null);

		desc += InEditorDescHelper.GetListDiffString("Effect Sequence Prefabs:\t", indent, m_sequencePrefabs, showDiff, showDiff ? other.m_sequencePrefabs : null, InEditorDescHelper.GetGameObjectEntryStr);
		desc += InEditorDescHelper.AssembleFieldWithDiff("Tick Sequence Prefab:", indent, otherSep, m_tickSequencePrefab, showDiff, showDiff ? other.m_tickSequencePrefab : null);
		if (showDivider)
		{
			desc += initialIndent + "-  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -  -\n";
		}
		return desc;
	}

	private string DiffColorStr(string input)
	{
		return InEditorDescHelper.ColoredString(input, "orange");
	}

	private string AssembleFieldWithDiff(
		string header,
		string indent,
		string otherSep,
		int myVal,
		bool showOther,
		int otherVal)
	{
		return InEditorDescHelper.AssembleFieldWithDiff(header, indent, otherSep, myVal, showOther, otherVal);
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
