using System;
using System.Collections.Generic;
using AbilityContextNamespace;

[Serializable]
public class NumericContextOperand
{
	public string m_contextName;

	public bool m_nonActorSpecificContext;

	public AbilityModPropertyFloat m_modifier;

	public List<AbilityModPropertyFloat> m_additionalModifiers = new List<AbilityModPropertyFloat>();

	private int m_contextKey;

	public int GetContextKey()
	{
		if (this.m_contextKey == 0)
		{
			this.m_contextKey = ContextVars.GetHash(this.m_contextName);
		}
		return this.m_contextKey;
	}

	public NumericContextOperand GetCopy()
	{
		NumericContextOperand numericContextOperand = base.MemberwiseClone() as NumericContextOperand;
		numericContextOperand.m_modifier = new AbilityModPropertyFloat();
		numericContextOperand.m_modifier.CopyValuesFrom(this.m_modifier);
		numericContextOperand.m_additionalModifiers = new List<AbilityModPropertyFloat>();
		if (this.m_additionalModifiers != null)
		{
			for (int i = 0; i < this.m_additionalModifiers.Count; i++)
			{
				AbilityModPropertyFloat abilityModPropertyFloat = new AbilityModPropertyFloat();
				abilityModPropertyFloat.CopyValuesFrom(this.m_additionalModifiers[i]);
				numericContextOperand.m_additionalModifiers.Add(abilityModPropertyFloat);
			}
		}
		return numericContextOperand;
	}

	public string GetInEditorDesc(string indent = "")
	{
		string text = string.Empty;
		if (!string.IsNullOrEmpty(this.m_contextName))
		{
			text = text + indent + InEditorDescHelper.ContextVarName(this.m_contextName, !this.m_nonActorSpecificContext) + AbilityModHelper.GetModPropertyDesc(this.m_modifier, string.Empty, false, 0f);
			for (int i = 0; i < this.m_additionalModifiers.Count; i++)
			{
				AbilityModPropertyFloat abilityModPropertyFloat = this.m_additionalModifiers[i];
				if (abilityModPropertyFloat.operation != AbilityModPropertyFloat.ModOp.Ignore)
				{
					text = text + indent + "    Then " + AbilityModHelper.GetModPropertyDesc(abilityModPropertyFloat, string.Empty, false, 0f);
				}
			}
		}
		return text;
	}
}
