using System;
using System.Collections.Generic;
using UnityEngine;

namespace AbilityContextNamespace
{
	[Serializable]
	public class OnHitDataMod
	{
		public IntFieldListModData m_enemyIntFieldMods;

		public EffectFieldListModData m_enemyEffectMods;

		[Space(20f)]
		public IntFieldListModData m_allyIntFieldMods;

		public EffectFieldListModData m_allyEffectMods;

		public OnHitAuthoredData symbol_001D(OnHitAuthoredData symbol_001D)
		{
			return new OnHitAuthoredData
			{
				m_enemyHitIntFields = this.m_enemyIntFieldMods.symbol_001D(symbol_001D.m_enemyHitIntFields),
				m_enemyHitEffectFields = this.m_enemyEffectMods.symbol_001D(symbol_001D.m_enemyHitEffectFields),
				m_allyHitIntFields = this.m_allyIntFieldMods.symbol_001D(symbol_001D.m_allyHitIntFields),
				m_allyHitEffectFields = this.m_allyEffectMods.symbol_001D(symbol_001D.m_allyHitEffectFields)
			};
		}

		public string symbol_001D(string symbol_001D, OnHitAuthoredData symbol_000E)
		{
			string text = string.Empty;
			text += OnHitDataMod.symbol_001D(this.m_enemyIntFieldMods, (symbol_000E == null) ? null : symbol_000E.m_enemyHitIntFields, "Enemy Int Field Mods");
			text += OnHitDataMod.symbol_001D(this.m_enemyEffectMods, (symbol_000E == null) ? null : symbol_000E.m_enemyHitEffectFields, "Enemy Effect Field Mods");
			string str = text;
			IntFieldListModData allyIntFieldMods = this.m_allyIntFieldMods;
			List<OnHitIntField> u000E;
			if (symbol_000E != null)
			{
				u000E = symbol_000E.m_allyHitIntFields;
			}
			else
			{
				u000E = null;
			}
			text = str + OnHitDataMod.symbol_001D(allyIntFieldMods, u000E, "Ally Int Field Mods");
			string str2 = text;
			EffectFieldListModData allyEffectMods = this.m_allyEffectMods;
			List<OnHitEffecField> u000E2;
			if (symbol_000E != null)
			{
				u000E2 = symbol_000E.m_allyHitEffectFields;
			}
			else
			{
				u000E2 = null;
			}
			text = str2 + OnHitDataMod.symbol_001D(allyEffectMods, u000E2, "Ally Effect Field Mods");
			if (text.Length > 0)
			{
				text = InEditorDescHelper.ColoredString(symbol_001D, "yellow", false) + "\n" + text + "\n";
			}
			return text;
		}

		public static string symbol_001D(IntFieldListModData symbol_001D, List<OnHitIntField> symbol_000E, string symbol_0012)
		{
			string text = string.Empty;
			if (symbol_001D.m_prependIntFields != null)
			{
				if (symbol_001D.m_prependIntFields.Count > 0)
				{
					text = text + "<color=cyan>" + symbol_0012 + ": New entries prepended:</color>\n";
					for (int i = 0; i < symbol_001D.m_prependIntFields.Count; i++)
					{
						OnHitIntField onHitIntField = symbol_001D.m_prependIntFields[i];
						text += onHitIntField.GetInEditorDesc();
					}
				}
			}
			if (symbol_001D.m_overrides != null)
			{
				if (symbol_001D.m_overrides.Count > 0)
				{
					text = text + "<color=cyan>" + symbol_0012 + ": Override to existing entry:</color>\n";
					for (int j = 0; j < symbol_001D.m_overrides.Count; j++)
					{
						IntFieldOverride intFieldOverride = symbol_001D.m_overrides[j];
						string text2 = intFieldOverride.symbol_001D();
						if (!string.IsNullOrEmpty(text2))
						{
							text = text + "Target Identifier: " + InEditorDescHelper.ColoredString(intFieldOverride.m_targetIdentifier, "white", false) + "\n";
							if (symbol_000E != null)
							{
								bool flag = false;
								foreach (OnHitIntField onHitIntField2 in symbol_000E)
								{
									if (onHitIntField2.GetIdentifier().Equals(text2, StringComparison.OrdinalIgnoreCase))
									{
										flag = true;
										text += intFieldOverride.m_fieldOverride.GetDesc(onHitIntField2);
										break;
									}
								}
								if (!flag)
								{
									text = text + "<color=red>Target Identifier " + text2 + " not found on base on hit data</color>\n";
								}
							}
						}
					}
				}
			}
			return text;
		}

		public static string symbol_001D(EffectFieldListModData symbol_001D, List<OnHitEffecField> symbol_000E, string symbol_0012)
		{
			string text = string.Empty;
			if (symbol_001D.m_prependEffectFields != null)
			{
				if (symbol_001D.m_prependEffectFields.Count > 0)
				{
					text = text + "<color=cyan>" + symbol_0012 + ": New entries prepended:</color>\n";
					for (int i = 0; i < symbol_001D.m_prependEffectFields.Count; i++)
					{
						OnHitEffecField onHitEffecField = symbol_001D.m_prependEffectFields[i];
						text += onHitEffecField.GetInEditorDesc(false, null);
					}
				}
			}
			if (symbol_001D.m_overrides != null)
			{
				if (symbol_001D.m_overrides.Count > 0)
				{
					text = text + "<color=cyan>" + symbol_0012 + ": Override to existing entry:</color>\n";
					for (int j = 0; j < symbol_001D.m_overrides.Count; j++)
					{
						EffectFieldOverride effectFieldOverride = symbol_001D.m_overrides[j];
						string text2 = effectFieldOverride.symbol_001D();
						if (!string.IsNullOrEmpty(text2))
						{
							OnHitEffecField onHitEffecField2 = null;
							if (symbol_000E != null)
							{
								using (List<OnHitEffecField>.Enumerator enumerator = symbol_000E.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										OnHitEffecField onHitEffecField3 = enumerator.Current;
										if (onHitEffecField3.GetIdentifier().Equals(text2, StringComparison.OrdinalIgnoreCase))
										{
											onHitEffecField2 = onHitEffecField3;
											goto IL_16E;
										}
									}
								}
								IL_16E:
								if (onHitEffecField2 == null)
								{
									text = text + "<color=red>Target Identifier " + text2 + " not found on base on hit data</color>\n";
								}
							}
							text = text + "Target Identifier: " + InEditorDescHelper.ColoredString(effectFieldOverride.m_targetIdentifier, "white", false) + "\n";
							text += effectFieldOverride.m_effectOverride.GetInEditorDesc(onHitEffecField2 != null, onHitEffecField2);
						}
					}
				}
			}
			return text;
		}

		public void symbol_001D(List<TooltipTokenEntry> symbol_001D, OnHitAuthoredData symbol_000E)
		{
			if (symbol_000E != null && this.m_enemyIntFieldMods != null)
			{
				OnHitDataMod.symbol_001D(symbol_001D, this.m_enemyIntFieldMods, symbol_000E.m_enemyHitIntFields);
				OnHitDataMod.symbol_001D(symbol_001D, this.m_enemyEffectMods, symbol_000E.m_enemyHitEffectFields);
				OnHitDataMod.symbol_001D(symbol_001D, this.m_allyIntFieldMods, symbol_000E.m_allyHitIntFields);
				OnHitDataMod.symbol_001D(symbol_001D, this.m_allyEffectMods, symbol_000E.m_allyHitEffectFields);
			}
		}

		public static void symbol_001D(List<TooltipTokenEntry> symbol_001D, IntFieldListModData symbol_000E, List<OnHitIntField> symbol_0012)
		{
			if (symbol_000E.m_prependIntFields != null)
			{
				for (int i = 0; i < symbol_000E.m_prependIntFields.Count; i++)
				{
					OnHitIntField onHitIntField = symbol_000E.m_prependIntFields[i];
					string identifier = onHitIntField.GetIdentifier();
					if (!string.IsNullOrEmpty(identifier))
					{
						onHitIntField.AddTooltipTokens(symbol_001D);
					}
				}
			}
			if (symbol_000E.m_overrides != null)
			{
				for (int j = 0; j < symbol_000E.m_overrides.Count; j++)
				{
					IntFieldOverride intFieldOverride = symbol_000E.m_overrides[j];
					string text = intFieldOverride.symbol_001D();
					if (!string.IsNullOrEmpty(text))
					{
						OnHitIntField onHitIntField2 = null;
						using (List<OnHitIntField>.Enumerator enumerator = symbol_0012.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								OnHitIntField onHitIntField3 = enumerator.Current;
								string identifier2 = onHitIntField3.GetIdentifier();
								if (text.Equals(identifier2, StringComparison.OrdinalIgnoreCase))
								{
									onHitIntField2 = onHitIntField3;
									goto IL_10B;
								}
							}
						}
						IL_10B:
						if (onHitIntField2 != null)
						{
							intFieldOverride.m_fieldOverride.AddTokens_zq(symbol_001D, onHitIntField2, text);
						}
					}
				}
			}
		}

		public static void symbol_001D(List<TooltipTokenEntry> symbol_001D, EffectFieldListModData symbol_000E, List<OnHitEffecField> symbol_0012)
		{
			if (symbol_000E.m_prependEffectFields != null)
			{
				for (int i = 0; i < symbol_000E.m_prependEffectFields.Count; i++)
				{
					OnHitEffecField onHitEffecField = symbol_000E.m_prependEffectFields[i];
					string identifier = onHitEffecField.GetIdentifier();
					if (!string.IsNullOrEmpty(identifier))
					{
						onHitEffecField.AddTooltipTokens(symbol_001D, false, null, null);
					}
				}
			}
			if (symbol_000E.m_overrides != null)
			{
				for (int j = 0; j < symbol_000E.m_overrides.Count; j++)
				{
					EffectFieldOverride effectFieldOverride = symbol_000E.m_overrides[j];
					string text = effectFieldOverride.symbol_001D();
					if (!string.IsNullOrEmpty(text))
					{
						OnHitEffecField onHitEffecField2 = null;
						using (List<OnHitEffecField>.Enumerator enumerator = symbol_0012.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								OnHitEffecField onHitEffecField3 = enumerator.Current;
								string identifier2 = onHitEffecField3.GetIdentifier();
								if (text.Equals(identifier2, StringComparison.OrdinalIgnoreCase))
								{
									onHitEffecField2 = onHitEffecField3;
									goto IL_112;
								}
							}
						}
						IL_112:
						if (onHitEffecField2 != null)
						{
							effectFieldOverride.m_effectOverride.AddTooltipTokens(symbol_001D, true, onHitEffecField2, text);
						}
					}
				}
			}
		}
	}
}
