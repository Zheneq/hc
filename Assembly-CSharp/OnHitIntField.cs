using System;
using System.Collections.Generic;
using AbilityContextNamespace;
using UnityEngine;

[Serializable]
public class OnHitIntField
{
	[Header("-- used to match mod data and generate tooltip tokens. Not case sensitive, but should be unique within ability")]
	public string m_identifier = string.Empty;

	public TargetFilterConditions m_conditions;

	public OnHitIntField.HitType m_hitType;

	public int m_baseValue;

	public int m_minValue;

	public int m_maxValue;

	[Header("-- Values taken from context vars and added to base value")]
	public int m_baseAddTotalMinValue;

	public int m_baseAddTotalMaxValue;

	public List<NumericContextOperand> m_baseAddModifiers;

	public string GetIdentifier()
	{
		return this.m_identifier.Trim();
	}

	public OnHitIntField GetCopy()
	{
		OnHitIntField onHitIntField = base.MemberwiseClone() as OnHitIntField;
		onHitIntField.m_conditions = this.m_conditions.symbol_001D();
		onHitIntField.m_baseAddModifiers = new List<NumericContextOperand>();
		for (int i = 0; i < this.m_baseAddModifiers.Count; i++)
		{
			NumericContextOperand copy = this.m_baseAddModifiers[i].GetCopy();
			onHitIntField.m_baseAddModifiers.Add(copy);
		}
		return onHitIntField;
	}

	public int CalcValue(ActorHitContext hitContext, ContextVars abilityContext)
	{
		int num = this.m_baseValue;
		int num2 = 0;
		for (int i = 0; i < this.m_baseAddModifiers.Count; i++)
		{
			NumericContextOperand numericContextOperand = this.m_baseAddModifiers[i];
			int contextKey = numericContextOperand.GetContextKey();
			bool flag = false;
			float input = 0f;
			if (numericContextOperand.m_nonActorSpecificContext)
			{
				if (abilityContext.ContainsInt(contextKey))
				{
					input = (float)abilityContext.GetInt(contextKey);
					flag = true;
				}
				else if (abilityContext.ContaintFloat(contextKey))
				{
					input = abilityContext.GetFloat(contextKey);
					flag = true;
				}
			}
			else if (hitContext.symbol_0015.ContainsInt(contextKey))
			{
				input = (float)hitContext.symbol_0015.GetInt(contextKey);
				flag = true;
			}
			else if (hitContext.symbol_0015.ContaintFloat(contextKey))
			{
				input = hitContext.symbol_0015.GetFloat(contextKey);
				flag = true;
			}
			if (flag)
			{
				float modifiedValue = numericContextOperand.m_modifier.GetModifiedValue(input);
				if (numericContextOperand.m_additionalModifiers != null)
				{
					for (int j = 0; j < numericContextOperand.m_additionalModifiers.Count; j++)
					{
						modifiedValue = numericContextOperand.m_additionalModifiers[j].GetModifiedValue(modifiedValue);
					}
				}
				int num3 = Mathf.RoundToInt(modifiedValue);
				num2 += num3;
			}
		}
		if (num2 < this.m_baseAddTotalMinValue)
		{
			num2 = this.m_baseAddTotalMinValue;
		}
		else if (num2 > this.m_baseAddTotalMaxValue)
		{
			if (this.m_baseAddTotalMaxValue > 0)
			{
				num2 = this.m_baseAddTotalMaxValue;
			}
		}
		num += num2;
		if (num < this.m_minValue)
		{
			num = this.m_minValue;
		}
		else if (num > this.m_maxValue && this.m_maxValue > 0)
		{
			num = this.m_maxValue;
		}
		return num;
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens)
	{
		string identifier = this.GetIdentifier();
		if (!string.IsNullOrEmpty(identifier))
		{
			TooltipTokenHelper.AddTokenInt(tokens, identifier + "_Base", this.m_baseValue, string.Empty, false);
			TooltipTokenHelper.AddTokenInt(tokens, identifier + "_Min", this.m_minValue, string.Empty, false);
			TooltipTokenHelper.AddTokenInt(tokens, identifier + "_Max", this.m_maxValue, string.Empty, false);
			TooltipTokenHelper.AddTokenInt(tokens, identifier + "_BaseAddMin", this.m_baseAddTotalMinValue, string.Empty, false);
			TooltipTokenHelper.AddTokenInt(tokens, identifier + "_BaseAddMax", this.m_baseAddTotalMaxValue, string.Empty, false);
		}
	}

	public string GetInEditorDesc()
	{
		string text = "Field Type < " + InEditorDescHelper.ColoredString(this.m_hitType.ToString(), "cyan", false) + " >\n";
		if (!string.IsNullOrEmpty(this.m_identifier))
		{
			text = text + "Identifier: " + InEditorDescHelper.ColoredString(this.m_identifier, "white", false) + "\n";
		}
		text = text + "Conditions:\n" + this.m_conditions.symbol_001D("    ");
		text = text + "BaseValue= " + InEditorDescHelper.ColoredString((float)this.m_baseValue, "cyan", false) + "\n";
		if (this.m_minValue > 0)
		{
			text = text + "MinValue= " + InEditorDescHelper.ColoredString((float)this.m_minValue, "cyan", false) + "\n";
		}
		if (this.m_maxValue > 0)
		{
			text = text + "MaxValue= " + InEditorDescHelper.ColoredString((float)this.m_maxValue, "cyan", false) + "\n";
		}
		if (this.m_baseAddModifiers.Count > 0)
		{
			text += "+ Base Add Modifiers\n";
			using (List<NumericContextOperand>.Enumerator enumerator = this.m_baseAddModifiers.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					NumericContextOperand numericContextOperand = enumerator.Current;
					text += numericContextOperand.GetInEditorDesc("    ");
				}
			}
		}
		return text;
	}

	public enum HitType
	{
		Damage,
		Healing,
		EnergyChange
	}
}
