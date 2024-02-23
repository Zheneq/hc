using AbilityContextNamespace;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class OnHitBarrierField
{
	[Header("-- used to match mod data and generate tooltip tokens. Not case sensitive, but should be unique within ability")]
	public string m_identifier = string.Empty;

	[Header("-- Context Names for center position and facing direction of barrier, set by target select component")]
	public string m_centerPosContextName = "CenterPos";

	public string m_facingDirContextName = "FacingDir";

	public string m_barrierWidthContextName = "BarrierWidth";

	public StandardBarrierData m_barrierData;

	private int m_centerPosContextKey;

	private int m_facingDirContextKey;

	private int m_barrierWidthContextKey;

	public string GetIdentifier()
	{
		return m_identifier.Trim();
	}

	public int GetCenterPosContextKey(bool recalc = false)
	{
		if (m_centerPosContextKey != 0)
		{
			if (!recalc)
			{
				goto IL_003b;
			}
		}
		m_centerPosContextKey = ContextVars.ToContextKey(m_centerPosContextName);
		goto IL_003b;
		IL_003b:
		return m_centerPosContextKey;
	}

	public int GetFacingDirContextKey(bool recalc = false)
	{
		if (m_facingDirContextKey != 0)
		{
			if (!recalc)
			{
				goto IL_003b;
			}
		}
		m_facingDirContextKey = ContextVars.ToContextKey(m_facingDirContextName);
		goto IL_003b;
		IL_003b:
		return m_facingDirContextKey;
	}

	public int GetBarrierWidthContextKey(bool recalc = false)
	{
		if (m_barrierWidthContextKey != 0)
		{
			if (!recalc)
			{
				goto IL_0039;
			}
		}
		m_barrierWidthContextKey = ContextVars.ToContextKey(m_barrierWidthContextName);
		goto IL_0039;
		IL_0039:
		return m_barrierWidthContextKey;
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens)
	{
		string identifier = GetIdentifier();
		if (string.IsNullOrEmpty(identifier))
		{
			return;
		}
		while (true)
		{
			if (m_barrierData != null)
			{
				m_barrierData.AddTooltipTokens(tokens, identifier);
			}
			return;
		}
	}

	public string GetInEditorDesc()
	{
		string result = string.Empty;
		if (m_barrierData != null)
		{
			result = "- Barrier To Spawn -\n";
			if (!string.IsNullOrEmpty(m_identifier))
			{
				result = new StringBuilder().Append(result).Append("Identifier: ").Append(InEditorDescHelper.ColoredString(m_identifier, "white")).Append("\n").ToString();
			}
			if (!string.IsNullOrEmpty(m_centerPosContextName))
			{
				result = new StringBuilder().Append(result).Append("Center Pos Context Var = ").Append(m_centerPosContextName).Append("\n").ToString();
			}
			else
			{
				result = new StringBuilder().Append(result).Append(InEditorDescHelper.ColoredString("Center Pos Context Var is empty, please specify one", "orange")).Append("\n").ToString();
			}

			result = (string.IsNullOrEmpty(m_facingDirContextName) ? new StringBuilder().Append(result).Append(InEditorDescHelper.ColoredString("Facing Dir Context Var is empty, please specify one", "orange")).Append("\n").ToString() : new StringBuilder().Append(result).Append("Facing Dir Context Var = ").Append(m_facingDirContextName).Append("\n").ToString());
			result = new StringBuilder().Append(result).Append(m_barrierData.GetInEditorDescription()).Append("\n").ToString();
		}
		return result;
	}
}
