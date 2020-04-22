using AbilityContextNamespace;
using System;
using System.Collections.Generic;
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
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!recalc)
			{
				goto IL_003b;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_centerPosContextKey = ContextVars.GetHash(m_centerPosContextName);
		goto IL_003b;
		IL_003b:
		return m_centerPosContextKey;
	}

	public int GetFacingDirContextKey(bool recalc = false)
	{
		if (m_facingDirContextKey != 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!recalc)
			{
				goto IL_003b;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_facingDirContextKey = ContextVars.GetHash(m_facingDirContextName);
		goto IL_003b;
		IL_003b:
		return m_facingDirContextKey;
	}

	public int GetBarrierWidthContextKey(bool recalc = false)
	{
		if (m_barrierWidthContextKey != 0)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (!recalc)
			{
				goto IL_0039;
			}
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		m_barrierWidthContextKey = ContextVars.GetHash(m_barrierWidthContextName);
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
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
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
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			result = "- Barrier To Spawn -\n";
			if (!string.IsNullOrEmpty(m_identifier))
			{
				result = result + "Identifier: " + InEditorDescHelper.ColoredString(m_identifier, "white") + "\n";
			}
			if (!string.IsNullOrEmpty(m_centerPosContextName))
			{
				while (true)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				result = result + "Center Pos Context Var = " + m_centerPosContextName + "\n";
			}
			else
			{
				result = result + InEditorDescHelper.ColoredString("Center Pos Context Var is empty, please specify one", "orange") + "\n";
			}
			result = (string.IsNullOrEmpty(m_facingDirContextName) ? (result + InEditorDescHelper.ColoredString("Facing Dir Context Var is empty, please specify one", "orange") + "\n") : (result + "Facing Dir Context Var = " + m_facingDirContextName + "\n"));
			result = result + m_barrierData.GetInEditorDescription() + "\n";
		}
		return result;
	}
}
