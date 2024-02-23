using AbilityContextNamespace;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

[Serializable]
public class OnHitIntField
{
	public enum HitType
	{
		Damage,
		Healing,
		EnergyChange
	}

	[Header("-- used to match mod data and generate tooltip tokens. Not case sensitive, but should be unique within ability")]
	public string m_identifier = string.Empty;

	public TargetFilterConditions m_conditions;

	public HitType m_hitType;

	public int m_baseValue;

	public int m_minValue;

	public int m_maxValue;

	[Header("-- Values taken from context vars and added to base value")]
	public int m_baseAddTotalMinValue;

	public int m_baseAddTotalMaxValue;

	public List<NumericContextOperand> m_baseAddModifiers;

	public string GetIdentifier()
	{
		return m_identifier.Trim();
	}

	public OnHitIntField GetCopy()
	{
		OnHitIntField onHitIntField = MemberwiseClone() as OnHitIntField;
		onHitIntField.m_conditions = m_conditions.GetCopy();
		onHitIntField.m_baseAddModifiers = new List<NumericContextOperand>();
		for (int i = 0; i < m_baseAddModifiers.Count; i++)
		{
			NumericContextOperand copy = m_baseAddModifiers[i].GetCopy();
			onHitIntField.m_baseAddModifiers.Add(copy);
		}
		while (true)
		{
			return onHitIntField;
		}
	}

	public int CalcValue(ActorHitContext hitContext, ContextVars abilityContext)
	{
		int baseValue = m_baseValue;
		int num = 0;
		for (int i = 0; i < m_baseAddModifiers.Count; i++)
		{
			NumericContextOperand numericContextOperand = m_baseAddModifiers[i];
			int contextKey = numericContextOperand.GetContextKey();
			bool flag = false;
			float input = 0f;
			if (numericContextOperand.m_nonActorSpecificContext)
			{
				if (abilityContext.HasVarInt(contextKey))
				{
					input = abilityContext.GetValueInt(contextKey);
					flag = true;
				}
				else if (abilityContext.HasVarFloat(contextKey))
				{
					input = abilityContext.GetValueFloat(contextKey);
					flag = true;
				}
			}
			else if (hitContext.m_contextVars.HasVarInt(contextKey))
			{
				input = hitContext.m_contextVars.GetValueInt(contextKey);
				flag = true;
			}
			else if (hitContext.m_contextVars.HasVarFloat(contextKey))
			{
				input = hitContext.m_contextVars.GetValueFloat(contextKey);
				flag = true;
			}
			if (!flag)
			{
				continue;
			}
			float modifiedValue = numericContextOperand.m_modifier.GetModifiedValue(input);
			if (numericContextOperand.m_additionalModifiers != null)
			{
				for (int j = 0; j < numericContextOperand.m_additionalModifiers.Count; j++)
				{
					modifiedValue = numericContextOperand.m_additionalModifiers[j].GetModifiedValue(modifiedValue);
				}
			}
			int num2 = Mathf.RoundToInt(modifiedValue);
			num += num2;
		}
		while (true)
		{
			if (num < m_baseAddTotalMinValue)
			{
				num = m_baseAddTotalMinValue;
			}
			else if (num > m_baseAddTotalMaxValue)
			{
				if (m_baseAddTotalMaxValue > 0)
				{
					num = m_baseAddTotalMaxValue;
				}
			}
			baseValue += num;
			if (baseValue < m_minValue)
			{
				baseValue = m_minValue;
			}
			else if (baseValue > m_maxValue && m_maxValue > 0)
			{
				baseValue = m_maxValue;
			}
			return baseValue;
		}
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens)
	{
		string identifier = GetIdentifier();
		if (!string.IsNullOrEmpty(identifier))
		{
			TooltipTokenHelper.AddTokenInt(tokens, new StringBuilder().Append(identifier).Append("_Base").ToString(), m_baseValue, string.Empty);
			TooltipTokenHelper.AddTokenInt(tokens, new StringBuilder().Append(identifier).Append("_Min").ToString(), m_minValue, string.Empty);
			TooltipTokenHelper.AddTokenInt(tokens, new StringBuilder().Append(identifier).Append("_Max").ToString(), m_maxValue, string.Empty);
			TooltipTokenHelper.AddTokenInt(tokens, new StringBuilder().Append(identifier).Append("_BaseAddMin").ToString(), m_baseAddTotalMinValue, string.Empty);
			TooltipTokenHelper.AddTokenInt(tokens, new StringBuilder().Append(identifier).Append("_BaseAddMax").ToString(), m_baseAddTotalMaxValue, string.Empty);
		}
	}

	public string GetInEditorDesc()
	{
		string str = new StringBuilder().Append("Field Type < ").Append(InEditorDescHelper.ColoredString(m_hitType.ToString())).Append(" >\n").ToString();
		if (!string.IsNullOrEmpty(m_identifier))
		{
			str = new StringBuilder().Append(str).Append("Identifier: ").Append(InEditorDescHelper.ColoredString(m_identifier, "white")).Append("\n").ToString();
		}

		str = new StringBuilder().Append(str).Append("Conditions:\n").Append(m_conditions.GetInEditorDesc("    ")).ToString();
		str = new StringBuilder().Append(str).Append("BaseValue= ").Append(InEditorDescHelper.ColoredString(m_baseValue)).Append("\n").ToString();
		if (m_minValue > 0)
		{
			str = new StringBuilder().Append(str).Append("MinValue= ").Append(InEditorDescHelper.ColoredString(m_minValue)).Append("\n").ToString();
		}
		if (m_maxValue > 0)
		{
			str = new StringBuilder().Append(str).Append("MaxValue= ").Append(InEditorDescHelper.ColoredString(m_maxValue)).Append("\n").ToString();
		}
		if (m_baseAddModifiers.Count > 0)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					break;
				default:
				{
					str += "+ Base Add Modifiers\n";
					using (List<NumericContextOperand>.Enumerator enumerator = m_baseAddModifiers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							NumericContextOperand current = enumerator.Current;
							str += current.GetInEditorDesc("    ");
						}
						while (true)
						{
							switch (7)
							{
							case 0:
								break;
							default:
								return str;
							}
						}
					}
				}
				}
			}
		}
		return str;
	}
}
