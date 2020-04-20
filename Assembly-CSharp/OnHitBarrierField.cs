using System;
using System.Collections.Generic;
using AbilityContextNamespace;
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
		return this.m_identifier.Trim();
	}

	public int GetCenterPosContextKey(bool recalc = false)
	{
		if (this.m_centerPosContextKey != 0)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(OnHitBarrierField.GetCenterPosContextKey(bool)).MethodHandle;
			}
			if (!recalc)
			{
				goto IL_3B;
			}
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_centerPosContextKey = ContextVars.GetHash(this.m_centerPosContextName);
		IL_3B:
		return this.m_centerPosContextKey;
	}

	public int GetFacingDirContextKey(bool recalc = false)
	{
		if (this.m_facingDirContextKey != 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(OnHitBarrierField.GetFacingDirContextKey(bool)).MethodHandle;
			}
			if (!recalc)
			{
				goto IL_3B;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_facingDirContextKey = ContextVars.GetHash(this.m_facingDirContextName);
		IL_3B:
		return this.m_facingDirContextKey;
	}

	public int GetBarrierWidthContextKey(bool recalc = false)
	{
		if (this.m_barrierWidthContextKey != 0)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(OnHitBarrierField.GetBarrierWidthContextKey(bool)).MethodHandle;
			}
			if (!recalc)
			{
				goto IL_39;
			}
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		this.m_barrierWidthContextKey = ContextVars.GetHash(this.m_barrierWidthContextName);
		IL_39:
		return this.m_barrierWidthContextKey;
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens)
	{
		string identifier = this.GetIdentifier();
		if (!string.IsNullOrEmpty(identifier))
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(OnHitBarrierField.AddTooltipTokens(List<TooltipTokenEntry>)).MethodHandle;
			}
			if (this.m_barrierData != null)
			{
				this.m_barrierData.AddTooltipTokens(tokens, identifier, false, null);
			}
		}
	}

	public string GetInEditorDesc()
	{
		string text = string.Empty;
		if (this.m_barrierData != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(OnHitBarrierField.GetInEditorDesc()).MethodHandle;
			}
			text = "- Barrier To Spawn -\n";
			if (!string.IsNullOrEmpty(this.m_identifier))
			{
				text = text + "Identifier: " + InEditorDescHelper.ColoredString(this.m_identifier, "white", false) + "\n";
			}
			if (!string.IsNullOrEmpty(this.m_centerPosContextName))
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
				text = text + "Center Pos Context Var = " + this.m_centerPosContextName + "\n";
			}
			else
			{
				text = text + InEditorDescHelper.ColoredString("Center Pos Context Var is empty, please specify one", "orange", false) + "\n";
			}
			if (!string.IsNullOrEmpty(this.m_facingDirContextName))
			{
				text = text + "Facing Dir Context Var = " + this.m_facingDirContextName + "\n";
			}
			else
			{
				text = text + InEditorDescHelper.ColoredString("Facing Dir Context Var is empty, please specify one", "orange", false) + "\n";
			}
			text = text + this.m_barrierData.GetInEditorDescription("Barrier Data", "    ", false, null) + "\n";
		}
		return text;
	}
}
