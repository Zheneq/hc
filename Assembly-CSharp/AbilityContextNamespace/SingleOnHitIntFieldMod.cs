using System;
using System.Collections.Generic;
using UnityEngine;

namespace AbilityContextNamespace
{
	[Serializable]
	public class SingleOnHitIntFieldMod
	{
		[Header("-- Condition override")]
		public bool m_useConditionOverride;
		public TargetFilterConditions m_conditionOverride;

		[Header("-- Base value and its limits")]
		public AbilityModPropertyInt m_baseValueMod;
		public AbilityModPropertyInt m_minValueMod;
		public AbilityModPropertyInt m_maxValueMod;

		[Header("-- Limits on Base Add Total")]
		public AbilityModPropertyInt m_baseAddTotalMinValueMod;
		public AbilityModPropertyInt m_baseAddTotalMaxValueMod;

		[Header("-- Whether to override base add modifiers")]
		public bool m_useBaseAddModifierOverrides;

		public List<NumericContextOperand> m_baseAddModifierOverrides;

		public OnHitIntField GetModdedIntField(OnHitIntField baseIntField)
		{
			OnHitIntField copy = baseIntField.GetCopy();
			if (m_useConditionOverride)
			{
				copy.m_conditions = m_conditionOverride.GetCopy();
			}
			copy.m_baseValue = m_baseValueMod.GetModifiedValue(baseIntField.m_baseValue);
			copy.m_minValue = m_minValueMod.GetModifiedValue(baseIntField.m_minValue);
			copy.m_maxValue = m_maxValueMod.GetModifiedValue(baseIntField.m_maxValue);
			copy.m_baseAddTotalMinValue = m_baseAddTotalMinValueMod.GetModifiedValue(baseIntField.m_baseAddTotalMinValue);
			copy.m_baseAddTotalMaxValue = m_baseAddTotalMaxValueMod.GetModifiedValue(baseIntField.m_baseAddTotalMaxValue);
			if (m_useBaseAddModifierOverrides)
			{
				copy.m_baseAddModifiers = new List<NumericContextOperand>();
				for (int i = 0; i < m_baseAddModifierOverrides.Count; i++)
				{
					copy.m_baseAddModifiers.Add(m_baseAddModifierOverrides[i].GetCopy());
				}
			}
			return copy;
		}

		public string GetInEditorDesc(OnHitIntField baseIntField)
		{
			string text = "";
			if (baseIntField != null)
			{
				if (m_useConditionOverride)
				{
					text += "* Using Condition override *\n";
					text += m_conditionOverride._001D("    ");
				}
				text += AbilityModHelper.GetModPropertyDesc(m_baseValueMod, "[BaseValue]", true, baseIntField.m_baseValue);
				text += AbilityModHelper.GetModPropertyDesc(m_minValueMod, "[MinValue]", true, baseIntField.m_minValue);
				text += AbilityModHelper.GetModPropertyDesc(m_maxValueMod, "[MaxValue]", true, baseIntField.m_maxValue);
				text += AbilityModHelper.GetModPropertyDesc(m_baseAddTotalMinValueMod, "[BaseAddTotalMinValue]", true, baseIntField.m_baseAddTotalMinValue);
				text += AbilityModHelper.GetModPropertyDesc(m_baseAddTotalMaxValueMod, "[BaseAddTotalMaxValue]", true, baseIntField.m_baseAddTotalMaxValue);
				if (m_useBaseAddModifierOverrides)
				{
					text += "* Using Base Add Modifier Overrides *\n";
					if (m_baseAddModifierOverrides.Count > 0)
					{
						text += "+ Base Add Modifiers\n";
						foreach (NumericContextOperand numericContextOperand in m_baseAddModifierOverrides)
						{
							text += numericContextOperand.GetInEditorDesc("    ");
						}
						return text;
					}
				}
			}
			return text;
		}

		public void AddTooltipTokens(List<TooltipTokenEntry> tokens, OnHitIntField baseIntField, string name)
		{
			if (baseIntField == null)
			{
				return;
			}
			AbilityMod.AddToken(tokens, m_baseValueMod, name + "_Base", "", baseIntField.m_baseValue);
			AbilityMod.AddToken(tokens, m_minValueMod, name + "_Min", "", baseIntField.m_minValue);
			AbilityMod.AddToken(tokens, m_maxValueMod, name + "_Max", "", baseIntField.m_maxValue);
			AbilityMod.AddToken(tokens, m_baseAddTotalMinValueMod, name + "_BaseAddTotalMin", "", baseIntField.m_baseAddTotalMinValue);
			AbilityMod.AddToken(tokens, m_baseAddTotalMaxValueMod, name + "_BaseAddTotalMax", "", baseIntField.m_baseAddTotalMaxValue);

			if (!m_useBaseAddModifierOverrides || m_baseAddModifierOverrides == null)
			{
				return;
			}

			for (int i = 0; i < m_baseAddModifierOverrides.Count; i++)
			{
				NumericContextOperand numericContextOperand = m_baseAddModifierOverrides[i];
				if (!numericContextOperand.m_contextName.IsNullOrEmpty())
				{
					int num = Mathf.RoundToInt(numericContextOperand.m_modifier.value);
					if (num > 0)
					{
						tokens.Add(new TooltipTokenInt(name + "_Add_" + i + "_Main", "", num));
					}
					if (numericContextOperand.m_additionalModifiers != null)
					{
						for (int j = 0; j < numericContextOperand.m_additionalModifiers.Count; j++)
						{
							int num2 = Mathf.RoundToInt(numericContextOperand.m_additionalModifiers[j].value);
							if (num2 > 0)
							{
								tokens.Add(new TooltipTokenInt(name + "_Add_" + i + "_Extra_" + j, "", num2));
							}
						}
					}
				}
			}
		}
	}
}
