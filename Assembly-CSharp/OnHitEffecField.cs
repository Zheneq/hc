using System;
using System.Collections.Generic;
using AbilityContextNamespace;
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
		return this.m_identifier.Trim();
	}

	public OnHitEffecField GetCopy()
	{
		OnHitEffecField onHitEffecField = base.MemberwiseClone() as OnHitEffecField;
		onHitEffecField.m_conditions = this.m_conditions.symbol_001D();
		onHitEffecField.m_effect = this.m_effect.GetShallowCopy();
		return onHitEffecField;
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens, bool diff, OnHitEffecField other, string identifierOverride = null)
	{
		string text = (identifierOverride == null) ? this.GetIdentifier() : identifierOverride;
		if (!string.IsNullOrEmpty(text) && this.m_effect.m_applyEffect)
		{
			bool flag;
			if (diff)
			{
				if (other != null)
				{
					flag = (other.m_effect != null);
					goto IL_5A;
				}
			}
			flag = false;
			IL_5A:
			bool flag2 = flag;
			this.m_effect.m_effectData.AddTooltipTokens(tokens, text, flag2, (!flag2) ? null : other.m_effect.m_effectData);
		}
	}

	public string GetInEditorDesc(bool diff, OnHitEffecField other)
	{
		string text = "(effect set to not apply)\n";
		if (this.m_effect != null)
		{
			if (this.m_effect.m_applyEffect)
			{
				text = "- Effect to Apply -\n";
				if (!string.IsNullOrEmpty(this.m_identifier))
				{
					text = text + "Identifier: " + InEditorDescHelper.ColoredString(this.m_identifier, "white", false) + "\n";
				}
				text = text + "Conditions:\n" + this.m_conditions.symbol_001D("    ");
				if (this.m_skipRemainingEffectEntriesIfMatch)
				{
					text = text + InEditorDescHelper.ColoredString("    * Skipping later entries if this one applies to target", "white", false) + "\n";
				}
				text += "Effect Data:\n";
				bool flag;
				if (diff)
				{
					if (other != null)
					{
						flag = (other.m_effect != null);
						goto IL_EE;
					}
				}
				flag = false;
				IL_EE:
				bool flag2 = flag;
				string str = text;
				StandardActorEffectData effectData = this.m_effect.m_effectData;
				string initialIndent = "    ";
				bool showDivider = false;
				bool diff2 = flag2;
				StandardActorEffectData other2;
				if (flag2)
				{
					other2 = other.m_effect.m_effectData;
				}
				else
				{
					other2 = null;
				}
				text = str + effectData.GetInEditorDescription(initialIndent, showDivider, diff2, other2) + "\n";
			}
		}
		return text;
	}
}
