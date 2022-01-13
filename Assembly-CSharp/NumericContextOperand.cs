using AbilityContextNamespace;
using System;
using System.Collections.Generic;

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
		if (m_contextKey == 0)
		{
			m_contextKey = ContextVars.ToContextKey(m_contextName);
		}
		return m_contextKey;
	}

	public NumericContextOperand GetCopy()
	{
		NumericContextOperand numericContextOperand = MemberwiseClone() as NumericContextOperand;
		numericContextOperand.m_modifier = new AbilityModPropertyFloat();
		numericContextOperand.m_modifier.CopyValuesFrom(m_modifier);
		numericContextOperand.m_additionalModifiers = new List<AbilityModPropertyFloat>();
		if (m_additionalModifiers != null)
		{
			for (int i = 0; i < m_additionalModifiers.Count; i++)
			{
				AbilityModPropertyFloat abilityModPropertyFloat = new AbilityModPropertyFloat();
				abilityModPropertyFloat.CopyValuesFrom(m_additionalModifiers[i]);
				numericContextOperand.m_additionalModifiers.Add(abilityModPropertyFloat);
			}
		}
		return numericContextOperand;
	}

	public string GetInEditorDesc(string indent = "")
	{
		string text = string.Empty;
		if (!string.IsNullOrEmpty(m_contextName))
		{
			text = text + indent + InEditorDescHelper.ContextVarName(m_contextName, !m_nonActorSpecificContext) + AbilityModHelper.GetModPropertyDesc(m_modifier, string.Empty);
			for (int i = 0; i < m_additionalModifiers.Count; i++)
			{
				AbilityModPropertyFloat abilityModPropertyFloat = m_additionalModifiers[i];
				if (abilityModPropertyFloat.operation != 0)
				{
					text = text + indent + "    Then " + AbilityModHelper.GetModPropertyDesc(abilityModPropertyFloat, string.Empty);
				}
			}
		}
		return text;
	}
}
