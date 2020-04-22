using AbilityContextNamespace;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OnHitEffecField
{
	[Header("-- used to match mod data and generate tooltip tokens. Not case sensitive, but should be unique within ability")]
	public string m_identifier = string.Empty;

	public TargetFilterConditions m_conditions;

	[Header("-- Whether to ignore remaining effects after this entry, if this entry's condition matched")]
	public bool m_skipRemainingEffectEntriesIfMatch;

	public StandardEffectInfo m_effect;

	public string GetIdentifier()
	{
		return m_identifier.Trim();
	}

	public OnHitEffecField GetCopy()
	{
		OnHitEffecField onHitEffecField = MemberwiseClone() as OnHitEffecField;
		onHitEffecField.m_conditions = m_conditions._001D();
		onHitEffecField.m_effect = m_effect.GetShallowCopy();
		return onHitEffecField;
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens, bool diff, OnHitEffecField other, string identifierOverride = null)
	{
		string text = (identifierOverride == null) ? GetIdentifier() : identifierOverride;
		if (string.IsNullOrEmpty(text) || !m_effect.m_applyEffect)
		{
			return;
		}
		int num;
		if (diff)
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
			if (other != null)
			{
				while (true)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				num = ((other.m_effect != null) ? 1 : 0);
				goto IL_005a;
			}
		}
		num = 0;
		goto IL_005a;
		IL_005a:
		bool flag = (byte)num != 0;
		m_effect.m_effectData.AddTooltipTokens(tokens, text, flag, (!flag) ? null : other.m_effect.m_effectData);
	}

	public string GetInEditorDesc(bool diff, OnHitEffecField other)
	{
		string result = "(effect set to not apply)\n";
		int num;
		if (m_effect != null)
		{
			while (true)
			{
				switch (2)
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
			if (m_effect.m_applyEffect)
			{
				result = "- Effect to Apply -\n";
				if (!string.IsNullOrEmpty(m_identifier))
				{
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					result = result + "Identifier: " + InEditorDescHelper.ColoredString(m_identifier, "white") + "\n";
				}
				result = result + "Conditions:\n" + m_conditions._001D("    ");
				if (m_skipRemainingEffectEntriesIfMatch)
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
					result = result + InEditorDescHelper.ColoredString("    * Skipping later entries if this one applies to target", "white") + "\n";
				}
				result += "Effect Data:\n";
				if (diff)
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
					if (other != null)
					{
						num = ((other.m_effect != null) ? 1 : 0);
						goto IL_00ee;
					}
				}
				num = 0;
				goto IL_00ee;
			}
		}
		goto IL_012f;
		IL_012f:
		return result;
		IL_00ee:
		bool flag = (byte)num != 0;
		string str = result;
		StandardActorEffectData effectData = m_effect.m_effectData;
		object other2;
		if (flag)
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
			other2 = other.m_effect.m_effectData;
		}
		else
		{
			other2 = null;
		}
		result = str + effectData.GetInEditorDescription("    ", false, flag, (StandardActorEffectData)other2) + "\n";
		goto IL_012f;
	}
}
