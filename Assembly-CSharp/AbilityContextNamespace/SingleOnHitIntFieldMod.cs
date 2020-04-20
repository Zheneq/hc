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

		public OnHitIntField symbol_001D(OnHitIntField symbol_001D)
		{
			OnHitIntField copy = symbol_001D.GetCopy();
			if (this.m_useConditionOverride)
			{
				copy.m_conditions = this.m_conditionOverride.symbol_001D();
			}
			copy.m_baseValue = this.m_baseValueMod.GetModifiedValue(symbol_001D.m_baseValue);
			copy.m_minValue = this.m_minValueMod.GetModifiedValue(symbol_001D.m_minValue);
			copy.m_maxValue = this.m_maxValueMod.GetModifiedValue(symbol_001D.m_maxValue);
			copy.m_baseAddTotalMinValue = this.m_baseAddTotalMinValueMod.GetModifiedValue(symbol_001D.m_baseAddTotalMinValue);
			copy.m_baseAddTotalMaxValue = this.m_baseAddTotalMaxValueMod.GetModifiedValue(symbol_001D.m_baseAddTotalMaxValue);
			if (this.m_useBaseAddModifierOverrides)
			{
				copy.m_baseAddModifiers = new List<NumericContextOperand>();
				for (int i = 0; i < this.m_baseAddModifierOverrides.Count; i++)
				{
					copy.m_baseAddModifiers.Add(this.m_baseAddModifierOverrides[i].GetCopy());
				}
			}
			return copy;
		}

		public string GetDesc(OnHitIntField symbol_001D)
		{
			string text = string.Empty;
			if (symbol_001D != null)
			{
				if (this.m_useConditionOverride)
				{
					text += "* Using Condition override *\n";
					text += this.m_conditionOverride.symbol_001D("    ");
				}
				text += AbilityModHelper.GetModPropertyDesc(this.m_baseValueMod, "[BaseValue]", true, symbol_001D.m_baseValue);
				text += AbilityModHelper.GetModPropertyDesc(this.m_minValueMod, "[MinValue]", true, symbol_001D.m_minValue);
				text += AbilityModHelper.GetModPropertyDesc(this.m_maxValueMod, "[MaxValue]", true, symbol_001D.m_maxValue);
				text += AbilityModHelper.GetModPropertyDesc(this.m_baseAddTotalMinValueMod, "[BaseAddTotalMinValue]", true, symbol_001D.m_baseAddTotalMinValue);
				text += AbilityModHelper.GetModPropertyDesc(this.m_baseAddTotalMaxValueMod, "[BaseAddTotalMaxValue]", true, symbol_001D.m_baseAddTotalMaxValue);
				if (this.m_useBaseAddModifierOverrides)
				{
					text += "* Using Base Add Modifier Overrides *\n";
					if (this.m_baseAddModifierOverrides.Count > 0)
					{
						text += "+ Base Add Modifiers\n";
						using (List<NumericContextOperand>.Enumerator enumerator = this.m_baseAddModifierOverrides.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								NumericContextOperand numericContextOperand = enumerator.Current;
								text += numericContextOperand.GetInEditorDesc("    ");
							}
						}
					}
				}
			}
			return text;
		}

		public void AddTokens_zq(List<TooltipTokenEntry> symbol_001D, OnHitIntField symbol_000E, string symbol_0012)
		{
			if (symbol_000E != null)
			{
				AbilityMod.AddToken(symbol_001D, this.m_baseValueMod, symbol_0012 + "_Base", string.Empty, symbol_000E.m_baseValue, true, false);
				AbilityMod.AddToken(symbol_001D, this.m_minValueMod, symbol_0012 + "_Min", string.Empty, symbol_000E.m_minValue, true, false);
				AbilityMod.AddToken(symbol_001D, this.m_maxValueMod, symbol_0012 + "_Max", string.Empty, symbol_000E.m_maxValue, true, false);
				AbilityMod.AddToken(symbol_001D, this.m_baseAddTotalMinValueMod, symbol_0012 + "_BaseAddTotalMin", string.Empty, symbol_000E.m_baseAddTotalMinValue, true, false);
				AbilityMod.AddToken(symbol_001D, this.m_baseAddTotalMaxValueMod, symbol_0012 + "_BaseAddTotalMax", string.Empty, symbol_000E.m_baseAddTotalMaxValue, true, false);
				if (this.m_useBaseAddModifierOverrides)
				{
					if (this.m_baseAddModifierOverrides != null)
					{
						for (int i = 0; i < this.m_baseAddModifierOverrides.Count; i++)
						{
							NumericContextOperand numericContextOperand = this.m_baseAddModifierOverrides[i];
							if (!numericContextOperand.m_contextName.IsNullOrEmpty())
							{
								int num = Mathf.RoundToInt(numericContextOperand.m_modifier.value);
								if (num > 0)
								{
									symbol_001D.Add(new TooltipTokenInt(string.Concat(new object[]
									{
										symbol_0012,
										"_Add_",
										i,
										"_Main"
									}), string.Empty, num));
								}
								if (numericContextOperand.m_additionalModifiers != null)
								{
									for (int j = 0; j < numericContextOperand.m_additionalModifiers.Count; j++)
									{
										int num2 = Mathf.RoundToInt(numericContextOperand.m_additionalModifiers[j].value);
										if (num2 > 0)
										{
											symbol_001D.Add(new TooltipTokenInt(string.Concat(new object[]
											{
												symbol_0012,
												"_Add_",
												i,
												"_Extra_",
												j
											}), string.Empty, num2));
										}
									}
								}
							}
						}
					}
				}
			}
		}
	}
}
