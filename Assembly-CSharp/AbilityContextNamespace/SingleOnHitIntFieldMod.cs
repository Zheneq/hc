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

		public OnHitIntField \u001D(OnHitIntField \u001D)
		{
			OnHitIntField copy = \u001D.GetCopy();
			if (this.m_useConditionOverride)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SingleOnHitIntFieldMod.\u001D(OnHitIntField)).MethodHandle;
				}
				copy.m_conditions = this.m_conditionOverride.\u001D();
			}
			copy.m_baseValue = this.m_baseValueMod.GetModifiedValue(\u001D.m_baseValue);
			copy.m_minValue = this.m_minValueMod.GetModifiedValue(\u001D.m_minValue);
			copy.m_maxValue = this.m_maxValueMod.GetModifiedValue(\u001D.m_maxValue);
			copy.m_baseAddTotalMinValue = this.m_baseAddTotalMinValueMod.GetModifiedValue(\u001D.m_baseAddTotalMinValue);
			copy.m_baseAddTotalMaxValue = this.m_baseAddTotalMaxValueMod.GetModifiedValue(\u001D.m_baseAddTotalMaxValue);
			if (this.m_useBaseAddModifierOverrides)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				copy.m_baseAddModifiers = new List<NumericContextOperand>();
				for (int i = 0; i < this.m_baseAddModifierOverrides.Count; i++)
				{
					copy.m_baseAddModifiers.Add(this.m_baseAddModifierOverrides[i].GetCopy());
				}
			}
			return copy;
		}

		public string GetDesc(OnHitIntField \u001D)
		{
			string text = string.Empty;
			if (\u001D != null)
			{
				for (;;)
				{
					switch (7)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SingleOnHitIntFieldMod.GetDesc(OnHitIntField)).MethodHandle;
				}
				if (this.m_useConditionOverride)
				{
					for (;;)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
					text += "* Using Condition override *\n";
					text += this.m_conditionOverride.\u001D("    ");
				}
				text += AbilityModHelper.GetModPropertyDesc(this.m_baseValueMod, "[BaseValue]", true, \u001D.m_baseValue);
				text += AbilityModHelper.GetModPropertyDesc(this.m_minValueMod, "[MinValue]", true, \u001D.m_minValue);
				text += AbilityModHelper.GetModPropertyDesc(this.m_maxValueMod, "[MaxValue]", true, \u001D.m_maxValue);
				text += AbilityModHelper.GetModPropertyDesc(this.m_baseAddTotalMinValueMod, "[BaseAddTotalMinValue]", true, \u001D.m_baseAddTotalMinValue);
				text += AbilityModHelper.GetModPropertyDesc(this.m_baseAddTotalMaxValueMod, "[BaseAddTotalMaxValue]", true, \u001D.m_baseAddTotalMaxValue);
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
							for (;;)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
						}
					}
				}
			}
			return text;
		}

		public void AddTokens_zq(List<TooltipTokenEntry> \u001D, OnHitIntField \u000E, string \u0012)
		{
			if (\u000E != null)
			{
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(SingleOnHitIntFieldMod.AddTokens_zq(List<TooltipTokenEntry>, OnHitIntField, string)).MethodHandle;
				}
				AbilityMod.AddToken(\u001D, this.m_baseValueMod, \u0012 + "_Base", string.Empty, \u000E.m_baseValue, true, false);
				AbilityMod.AddToken(\u001D, this.m_minValueMod, \u0012 + "_Min", string.Empty, \u000E.m_minValue, true, false);
				AbilityMod.AddToken(\u001D, this.m_maxValueMod, \u0012 + "_Max", string.Empty, \u000E.m_maxValue, true, false);
				AbilityMod.AddToken(\u001D, this.m_baseAddTotalMinValueMod, \u0012 + "_BaseAddTotalMin", string.Empty, \u000E.m_baseAddTotalMinValue, true, false);
				AbilityMod.AddToken(\u001D, this.m_baseAddTotalMaxValueMod, \u0012 + "_BaseAddTotalMax", string.Empty, \u000E.m_baseAddTotalMaxValue, true, false);
				if (this.m_useBaseAddModifierOverrides)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						break;
					}
					if (this.m_baseAddModifierOverrides != null)
					{
						for (;;)
						{
							switch (3)
							{
							case 0:
								continue;
							}
							break;
						}
						for (int i = 0; i < this.m_baseAddModifierOverrides.Count; i++)
						{
							NumericContextOperand numericContextOperand = this.m_baseAddModifierOverrides[i];
							if (!numericContextOperand.m_contextName.IsNullOrEmpty())
							{
								int num = Mathf.RoundToInt(numericContextOperand.m_modifier.value);
								if (num > 0)
								{
									for (;;)
									{
										switch (7)
										{
										case 0:
											continue;
										}
										break;
									}
									\u001D.Add(new TooltipTokenInt(string.Concat(new object[]
									{
										\u0012,
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
											\u001D.Add(new TooltipTokenInt(string.Concat(new object[]
											{
												\u0012,
												"_Add_",
												i,
												"_Extra_",
												j
											}), string.Empty, num2));
										}
									}
									for (;;)
									{
										switch (1)
										{
										case 0:
											continue;
										}
										break;
									}
								}
							}
						}
						for (;;)
						{
							switch (6)
							{
							case 0:
								continue;
							}
							break;
						}
					}
				}
			}
		}
	}
}
