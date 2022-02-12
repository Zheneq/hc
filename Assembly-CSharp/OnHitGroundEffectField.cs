// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

// server-only, missing in reactor
#if SERVER
[Serializable]
public class OnHitGroundEffectField
{
	public List<NumericContextValueCompareCond> m_compareConditions = new List<NumericContextValueCompareCond>();
	[Header("-- used to match mod data and generate tooltip tokens. Not case sensitive, but should be unique within ability")]
	public string m_identifier = "";
	[Header("-- Context Names for center position and facing direction of barrier, set by target select component")]
	public string m_centerPosContextName = "CenterPos";
	public GroundEffectField m_groundEffect;

	private int m_centerPosContextKey;

	public string GetIdentifier()
	{
		return this.m_identifier.Trim();
	}

	public int GetCenterPosContextKey(bool recalc = false)
	{
		if (this.m_centerPosContextKey == 0 || recalc)
		{
			this.m_centerPosContextKey = ContextVars.ToContextKey(this.m_centerPosContextName);
		}
		return this.m_centerPosContextKey;
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens)
	{
		string identifier = this.GetIdentifier();
		if (!string.IsNullOrEmpty(identifier) && this.m_groundEffect != null)
		{
			this.m_groundEffect.AddTooltipTokens(tokens, identifier, false, null);
		}
	}

	public OnHitGroundEffectField GetCopy()
	{
		OnHitGroundEffectField onHitGroundEffectField = base.MemberwiseClone() as OnHitGroundEffectField;
		onHitGroundEffectField.m_groundEffect = this.m_groundEffect.GetShallowCopy();
		return onHitGroundEffectField;
	}

	public string GetInEditorDesc()
	{
		string desc = "";
		if (this.m_groundEffect != null)
		{
			desc = "- Ground Effect To Spawn -\n";
			if (!string.IsNullOrEmpty(this.m_identifier))
			{
				desc += "Identifier: " + InEditorDescHelper.ColoredString(this.m_identifier, "white", false) + "\n";
			}
			if (!string.IsNullOrEmpty(this.m_centerPosContextName))
			{
				desc += "Center Pos Context Var = " + this.m_centerPosContextName + "\n";
			}
			else
			{
				desc += InEditorDescHelper.ColoredString("Center Pos Context Var is empty, please specify one", "orange", false) + "\n";
			}
			desc += "Barrier Data:" + this.m_groundEffect.GetInEditorDescription("GroundEffectField", "    ", false, null) + "\n";
		}
		return desc;
	}
}
#endif
