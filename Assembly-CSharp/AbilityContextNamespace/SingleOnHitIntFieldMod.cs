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

		public OnHitIntField _001D(OnHitIntField _001D)
		{
			OnHitIntField copy = _001D.GetCopy();
			if (m_useConditionOverride)
			{
				copy.m_conditions = m_conditionOverride._001D();
			}
			copy.m_baseValue = m_baseValueMod.GetModifiedValue(_001D.m_baseValue);
			copy.m_minValue = m_minValueMod.GetModifiedValue(_001D.m_minValue);
			copy.m_maxValue = m_maxValueMod.GetModifiedValue(_001D.m_maxValue);
			copy.m_baseAddTotalMinValue = m_baseAddTotalMinValueMod.GetModifiedValue(_001D.m_baseAddTotalMinValue);
			copy.m_baseAddTotalMaxValue = m_baseAddTotalMaxValueMod.GetModifiedValue(_001D.m_baseAddTotalMaxValue);
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

		public string GetDesc(OnHitIntField _001D)
		{
			string text = string.Empty;
			if (_001D != null)
			{
				if (m_useConditionOverride)
				{
					text += "* Using Condition override *\n";
					text += m_conditionOverride._001D("    ");
				}
				text += AbilityModHelper.GetModPropertyDesc(m_baseValueMod, "[BaseValue]", true, _001D.m_baseValue);
				text += AbilityModHelper.GetModPropertyDesc(m_minValueMod, "[MinValue]", true, _001D.m_minValue);
				text += AbilityModHelper.GetModPropertyDesc(m_maxValueMod, "[MaxValue]", true, _001D.m_maxValue);
				text += AbilityModHelper.GetModPropertyDesc(m_baseAddTotalMinValueMod, "[BaseAddTotalMinValue]", true, _001D.m_baseAddTotalMinValue);
				text += AbilityModHelper.GetModPropertyDesc(m_baseAddTotalMaxValueMod, "[BaseAddTotalMaxValue]", true, _001D.m_baseAddTotalMaxValue);
				if (m_useBaseAddModifierOverrides)
				{
					text += "* Using Base Add Modifier Overrides *\n";
					if (m_baseAddModifierOverrides.Count > 0)
					{
						text += "+ Base Add Modifiers\n";
						using (List<NumericContextOperand>.Enumerator enumerator = m_baseAddModifierOverrides.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								NumericContextOperand current = enumerator.Current;
								text += current.GetInEditorDesc("    ");
							}
							while (true)
							{
								switch (1)
								{
								case 0:
									break;
								default:
									return text;
								}
							}
						}
					}
				}
			}
			return text;
		}

		public void AddTokens_zq(List<TooltipTokenEntry> _001D, OnHitIntField _000E, string _0012)
		{
			if (_000E == null)
			{
				return;
			}
			while (true)
			{
				AbilityMod.AddToken(_001D, m_baseValueMod, _0012 + "_Base", string.Empty, _000E.m_baseValue);
				AbilityMod.AddToken(_001D, m_minValueMod, _0012 + "_Min", string.Empty, _000E.m_minValue);
				AbilityMod.AddToken(_001D, m_maxValueMod, _0012 + "_Max", string.Empty, _000E.m_maxValue);
				AbilityMod.AddToken(_001D, m_baseAddTotalMinValueMod, _0012 + "_BaseAddTotalMin", string.Empty, _000E.m_baseAddTotalMinValue);
				AbilityMod.AddToken(_001D, m_baseAddTotalMaxValueMod, _0012 + "_BaseAddTotalMax", string.Empty, _000E.m_baseAddTotalMaxValue);
				if (!m_useBaseAddModifierOverrides)
				{
					return;
				}
				while (true)
				{
					if (m_baseAddModifierOverrides == null)
					{
						return;
					}
					while (true)
					{
						for (int i = 0; i < m_baseAddModifierOverrides.Count; i++)
						{
							NumericContextOperand numericContextOperand = m_baseAddModifierOverrides[i];
							if (numericContextOperand.m_contextName.IsNullOrEmpty())
							{
								continue;
							}
							int num = Mathf.RoundToInt(numericContextOperand.m_modifier.value);
							if (num > 0)
							{
								_001D.Add(new TooltipTokenInt(_0012 + "_Add_" + i + "_Main", string.Empty, num));
							}
							if (numericContextOperand.m_additionalModifiers == null)
							{
								continue;
							}
							for (int j = 0; j < numericContextOperand.m_additionalModifiers.Count; j++)
							{
								int num2 = Mathf.RoundToInt(numericContextOperand.m_additionalModifiers[j].value);
								if (num2 > 0)
								{
									_001D.Add(new TooltipTokenInt(_0012 + "_Add_" + i + "_Extra_" + j, string.Empty, num2));
								}
							}
						}
						while (true)
						{
							switch (6)
							{
							default:
								return;
							case 0:
								break;
							}
						}
					}
				}
			}
		}
	}
}
