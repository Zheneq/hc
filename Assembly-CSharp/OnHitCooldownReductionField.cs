// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

// server-only, missing in reactor
//#if SERVER
//[Serializable]
//public class OnHitCooldownReductionField
//{
//	[Header("-- used to match mod data and generate tooltip tokens. Not case sensitive, but should be unique within ability")]
//	public string m_identifier = "";
//	public TargetFilterConditions m_conditions;
//	public AbilityModCooldownReduction m_cooldownReduction;

//	public string GetIdentifier()
//	{
//		return m_identifier.Trim();
//	}

//	public OnHitCooldownReductionField GetCopy()
//	{
//		OnHitCooldownReductionField onHitCooldownReductionField = base.MemberwiseClone() as OnHitCooldownReductionField;
//		onHitCooldownReductionField.m_conditions = this.m_conditions.GetCopy();
//		onHitCooldownReductionField.m_cooldownReduction = this.m_cooldownReduction.GetCopy();
//		return onHitCooldownReductionField;
//	}

//	public void AddTooltipTokens(List<TooltipTokenEntry> tokens)
//	{
//		string identifier = this.GetIdentifier();
//		if (!string.IsNullOrEmpty(identifier))
//		{
//			this.m_cooldownReduction.AddTooltipTokens(tokens, identifier + "_CDR");
//		}
//	}

//	public string GetInEditorDesc()
//	{
//		string str = "Field Type < " + InEditorDescHelper.ColoredString("cooldown reduction", "cyan", false) + " >\n";
//		if (!string.IsNullOrEmpty(this.m_identifier))
//		{
//			str = str + "Identifier: " + InEditorDescHelper.ColoredString(this.m_identifier, "white", false) + "\n";
//		}
//		str = str + "Conditions:\n" + this.m_conditions.GetInEditorDesc("    ");
//		return str + "Cooldown Reduction= " + InEditorDescHelper.ColoredString(this.m_cooldownReduction.GetDescription(null), "cyan", false) + "\n";
//	}
//}
//#endif
