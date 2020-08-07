using AbilityContextNamespace;
using System;
using System.Collections.Generic;
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
		onHitIntField.m_conditions = m_conditions._001D();
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
				if (abilityContext.ContainsInt(contextKey))
				{
					input = abilityContext.GetInt(contextKey);
					flag = true;
				}
				else if (abilityContext.ContaintFloat(contextKey))
				{
					input = abilityContext.GetFloat(contextKey);
					flag = true;
				}
			}
			else if (hitContext.context.ContainsInt(contextKey))
			{
				input = hitContext.context.GetInt(contextKey);
				flag = true;
			}
			else if (hitContext.context.ContaintFloat(contextKey))
			{
				input = hitContext.context.GetFloat(contextKey);
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
			TooltipTokenHelper.AddTokenInt(tokens, identifier + "_Base", m_baseValue, string.Empty);
			TooltipTokenHelper.AddTokenInt(tokens, identifier + "_Min", m_minValue, string.Empty);
			TooltipTokenHelper.AddTokenInt(tokens, identifier + "_Max", m_maxValue, string.Empty);
			TooltipTokenHelper.AddTokenInt(tokens, identifier + "_BaseAddMin", m_baseAddTotalMinValue, string.Empty);
			TooltipTokenHelper.AddTokenInt(tokens, identifier + "_BaseAddMax", m_baseAddTotalMaxValue, string.Empty);
		}
	}

	public string GetInEditorDesc()
	{
		string str = "Field Type < " + InEditorDescHelper.ColoredString(m_hitType.ToString()) + " >\n";
		if (!string.IsNullOrEmpty(m_identifier))
		{
			str = str + "Identifier: " + InEditorDescHelper.ColoredString(m_identifier, "white") + "\n";
		}
		str = str + "Conditions:\n" + m_conditions._001D("    ");
		str = str + "BaseValue= " + InEditorDescHelper.ColoredString(m_baseValue) + "\n";
		if (m_minValue > 0)
		{
			str = str + "MinValue= " + InEditorDescHelper.ColoredString(m_minValue) + "\n";
		}
		if (m_maxValue > 0)
		{
			str = str + "MaxValue= " + InEditorDescHelper.ColoredString(m_maxValue) + "\n";
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
