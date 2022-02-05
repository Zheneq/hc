// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

// server-only, added in rogues
//#if SERVER
//[Serializable]
//public class OnHitKnockbackField
//{
//	[Header("-- used to match mod data and generate tooltip tokens. Not case sensitive, but should be unique within ability")]
//	public string m_identifier = "";
//	public TargetFilterConditions m_conditions;
//	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;
//	public float m_distance;

//	public string GetIdentifier()
//	{
//		return this.m_identifier.Trim();
//	}

//	public OnHitKnockbackField GetCopy()
//	{
//		OnHitKnockbackField onHitKnockbackField = base.MemberwiseClone() as OnHitKnockbackField;
//		onHitKnockbackField.m_conditions = this.m_conditions.GetCopy();
//		return onHitKnockbackField;
//	}

//	public void AddTooltipTokens(List<TooltipTokenEntry> tokens)
//	{
//		string identifier = this.GetIdentifier();
//		if (!string.IsNullOrEmpty(identifier))
//		{
//			TooltipTokenHelper.AddTokenFloat(tokens, identifier + "_Distance", this.m_distance, "", false);
//		}
//	}

//	public string GetInEditorDesc()
//	{
//		string str = "Field Type < " + InEditorDescHelper.ColoredString("knockback", "cyan", false) + " >\n";
//		if (!string.IsNullOrEmpty(this.m_identifier))
//		{
//			str = str + "Identifier: " + InEditorDescHelper.ColoredString(this.m_identifier, "white", false) + "\n";
//		}
//		str = str + "Conditions:\n" + this.m_conditions.GetInEditorDesc("    ");
//		return str + "Distance= " + InEditorDescHelper.ColoredString(this.m_distance, "cyan", false) + "\n";
//	}
//}
//#endif
