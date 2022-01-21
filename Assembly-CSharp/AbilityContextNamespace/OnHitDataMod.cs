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

		public OnHitAuthoredData _001D(OnHitAuthoredData _001D)
		{
			OnHitAuthoredData onHitAuthoredData = new OnHitAuthoredData();
			onHitAuthoredData.m_enemyHitIntFields = m_enemyIntFieldMods.GetModdedIntFieldList(_001D.m_enemyHitIntFields);
			onHitAuthoredData.m_enemyHitEffectFields = m_enemyEffectMods.GetModdedEffectFieldList(_001D.m_enemyHitEffectFields);
			onHitAuthoredData.m_allyHitIntFields = m_allyIntFieldMods.GetModdedIntFieldList(_001D.m_allyHitIntFields);
			onHitAuthoredData.m_allyHitEffectFields = m_allyEffectMods.GetModdedEffectFieldList(_001D.m_allyHitEffectFields);
			return onHitAuthoredData;
		}

		public string _001D(string _001D, OnHitAuthoredData _000E)
		{
			string empty = string.Empty;
			empty += OnHitDataMod._001D(m_enemyIntFieldMods, _000E?.m_enemyHitIntFields, "Enemy Int Field Mods");
			empty += OnHitDataMod._001D(m_enemyEffectMods, _000E?.m_enemyHitEffectFields, "Enemy Effect Field Mods");
			string str = empty;
			IntFieldListModData allyIntFieldMods = m_allyIntFieldMods;
			object obj;
			if (_000E != null)
			{
				obj = _000E.m_allyHitIntFields;
			}
			else
			{
				obj = null;
			}
			empty = str + OnHitDataMod._001D(allyIntFieldMods, (List<OnHitIntField>)obj, "Ally Int Field Mods");
			string str2 = empty;
			EffectFieldListModData allyEffectMods = m_allyEffectMods;
			object obj2;
			if (_000E != null)
			{
				obj2 = _000E.m_allyHitEffectFields;
			}
			else
			{
				obj2 = null;
			}
			empty = str2 + OnHitDataMod._001D(allyEffectMods, (List<OnHitEffecField>)obj2, "Ally Effect Field Mods");
			if (empty.Length > 0)
			{
				empty = InEditorDescHelper.ColoredString(_001D, "yellow") + "\n" + empty + "\n";
			}
			return empty;
		}

		public static string _001D(IntFieldListModData _001D, List<OnHitIntField> _000E, string _0012)
		{
			string text = string.Empty;
			if (_001D.m_prependIntFields != null)
			{
				if (_001D.m_prependIntFields.Count > 0)
				{
					text = text + "<color=cyan>" + _0012 + ": New entries prepended:</color>\n";
					for (int i = 0; i < _001D.m_prependIntFields.Count; i++)
					{
						OnHitIntField onHitIntField = _001D.m_prependIntFields[i];
						text += onHitIntField.GetInEditorDesc();
					}
				}
			}
			if (_001D.m_overrides != null)
			{
				if (_001D.m_overrides.Count > 0)
				{
					text = text + "<color=cyan>" + _0012 + ": Override to existing entry:</color>\n";
					for (int j = 0; j < _001D.m_overrides.Count; j++)
					{
						IntFieldOverride intFieldOverride = _001D.m_overrides[j];
						string text2 = intFieldOverride.GetIdentifier();
						if (string.IsNullOrEmpty(text2))
						{
							continue;
						}
						text = text + "Target Identifier: " + InEditorDescHelper.ColoredString(intFieldOverride.m_targetIdentifier, "white") + "\n";
						if (_000E == null)
						{
							continue;
						}
						bool flag = false;
						foreach (OnHitIntField item in _000E)
						{
							if (item.GetIdentifier().Equals(text2, StringComparison.OrdinalIgnoreCase))
							{
								flag = true;
								text += intFieldOverride.m_fieldOverride.GetDesc(item);
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
			return text;
		}

		public static string _001D(EffectFieldListModData _001D, List<OnHitEffecField> _000E, string _0012)
		{
			string text = string.Empty;
			if (_001D.m_prependEffectFields != null)
			{
				if (_001D.m_prependEffectFields.Count > 0)
				{
					text = text + "<color=cyan>" + _0012 + ": New entries prepended:</color>\n";
					for (int i = 0; i < _001D.m_prependEffectFields.Count; i++)
					{
						OnHitEffecField onHitEffecField = _001D.m_prependEffectFields[i];
						text += onHitEffecField.GetInEditorDesc(false, null);
					}
				}
			}
			if (_001D.m_overrides != null)
			{
				if (_001D.m_overrides.Count > 0)
				{
					text = text + "<color=cyan>" + _0012 + ": Override to existing entry:</color>\n";
					for (int j = 0; j < _001D.m_overrides.Count; j++)
					{
						EffectFieldOverride effectFieldOverride = _001D.m_overrides[j];
						string text2 = effectFieldOverride.GetIdentifier();
						if (string.IsNullOrEmpty(text2))
						{
							continue;
						}
						OnHitEffecField onHitEffecField2 = null;
						if (_000E != null)
						{
							using (List<OnHitEffecField>.Enumerator enumerator = _000E.GetEnumerator())
							{
								while (true)
								{
									if (!enumerator.MoveNext())
									{
										break;
									}
									OnHitEffecField current = enumerator.Current;
									if (current.GetIdentifier().Equals(text2, StringComparison.OrdinalIgnoreCase))
									{
										while (true)
										{
											switch (4)
											{
											case 0:
												break;
											default:
												onHitEffecField2 = current;
												goto end_IL_011b;
											}
										}
									}
								}
								end_IL_011b:;
							}
							if (onHitEffecField2 == null)
							{
								text = text + "<color=red>Target Identifier " + text2 + " not found on base on hit data</color>\n";
							}
						}
						text = text + "Target Identifier: " + InEditorDescHelper.ColoredString(effectFieldOverride.m_targetIdentifier, "white") + "\n";
						text += effectFieldOverride.m_effectOverride.GetInEditorDesc(onHitEffecField2 != null, onHitEffecField2);
					}
				}
			}
			return text;
		}

		public void _001D(List<TooltipTokenEntry> _001D, OnHitAuthoredData _000E)
		{
			if (_000E == null || m_enemyIntFieldMods == null)
			{
				return;
			}
			while (true)
			{
				OnHitDataMod._001D(_001D, m_enemyIntFieldMods, _000E.m_enemyHitIntFields);
				OnHitDataMod._001D(_001D, m_enemyEffectMods, _000E.m_enemyHitEffectFields);
				OnHitDataMod._001D(_001D, m_allyIntFieldMods, _000E.m_allyHitIntFields);
				OnHitDataMod._001D(_001D, m_allyEffectMods, _000E.m_allyHitEffectFields);
				return;
			}
		}

		public static void _001D(List<TooltipTokenEntry> _001D, IntFieldListModData _000E, List<OnHitIntField> _0012)
		{
			if (_000E.m_prependIntFields != null)
			{
				for (int i = 0; i < _000E.m_prependIntFields.Count; i++)
				{
					OnHitIntField onHitIntField = _000E.m_prependIntFields[i];
					string identifier = onHitIntField.GetIdentifier();
					if (!string.IsNullOrEmpty(identifier))
					{
						onHitIntField.AddTooltipTokens(_001D);
					}
				}
			}
			if (_000E.m_overrides == null)
			{
				return;
			}
			while (true)
			{
				for (int j = 0; j < _000E.m_overrides.Count; j++)
				{
					IntFieldOverride intFieldOverride = _000E.m_overrides[j];
					string text = intFieldOverride.GetIdentifier();
					if (string.IsNullOrEmpty(text))
					{
						continue;
					}
					OnHitIntField onHitIntField2 = null;
					using (List<OnHitIntField>.Enumerator enumerator = _0012.GetEnumerator())
					{
						while (true)
						{
							if (!enumerator.MoveNext())
							{
								break;
							}
							OnHitIntField current = enumerator.Current;
							string identifier2 = current.GetIdentifier();
							if (text.Equals(identifier2, StringComparison.OrdinalIgnoreCase))
							{
								while (true)
								{
									switch (7)
									{
									case 0:
										break;
									default:
										onHitIntField2 = current;
										goto end_IL_00b2;
									}
								}
							}
						}
						end_IL_00b2:;
					}
					if (onHitIntField2 != null)
					{
						intFieldOverride.m_fieldOverride.AddTokens_zq(_001D, onHitIntField2, text);
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

		public static void _001D(List<TooltipTokenEntry> _001D, EffectFieldListModData _000E, List<OnHitEffecField> _0012)
		{
			if (_000E.m_prependEffectFields != null)
			{
				for (int i = 0; i < _000E.m_prependEffectFields.Count; i++)
				{
					OnHitEffecField onHitEffecField = _000E.m_prependEffectFields[i];
					string identifier = onHitEffecField.GetIdentifier();
					if (!string.IsNullOrEmpty(identifier))
					{
						onHitEffecField.AddTooltipTokens(_001D, false, null);
					}
				}
			}
			if (_000E.m_overrides == null)
			{
				return;
			}
			for (int j = 0; j < _000E.m_overrides.Count; j++)
			{
				EffectFieldOverride effectFieldOverride = _000E.m_overrides[j];
				string text = effectFieldOverride.GetIdentifier();
				if (!string.IsNullOrEmpty(text))
				{
					OnHitEffecField onHitEffecField2 = null;
					using (List<OnHitEffecField>.Enumerator enumerator = _0012.GetEnumerator())
					{
						while (true)
						{
							if (!enumerator.MoveNext())
							{
								break;
							}
							OnHitEffecField current = enumerator.Current;
							string identifier2 = current.GetIdentifier();
							if (text.Equals(identifier2, StringComparison.OrdinalIgnoreCase))
							{
								onHitEffecField2 = current;
								break;
							}
						}
					}
					if (onHitEffecField2 != null)
					{
						effectFieldOverride.m_effectOverride.AddTooltipTokens(_001D, true, onHitEffecField2, text);
					}
				}
			}
		}
	}
}
