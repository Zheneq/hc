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

		public OnHitAuthoredData \u001D(OnHitAuthoredData \u001D)
		{
			return new OnHitAuthoredData
			{
				m_enemyHitIntFields = this.m_enemyIntFieldMods.\u001D(\u001D.m_enemyHitIntFields),
				m_enemyHitEffectFields = this.m_enemyEffectMods.\u001D(\u001D.m_enemyHitEffectFields),
				m_allyHitIntFields = this.m_allyIntFieldMods.\u001D(\u001D.m_allyHitIntFields),
				m_allyHitEffectFields = this.m_allyEffectMods.\u001D(\u001D.m_allyHitEffectFields)
			};
		}

		public string \u001D(string \u001D, OnHitAuthoredData \u000E)
		{
			string text = string.Empty;
			text += OnHitDataMod.\u001D(this.m_enemyIntFieldMods, (\u000E == null) ? null : \u000E.m_enemyHitIntFields, "Enemy Int Field Mods");
			text += OnHitDataMod.\u001D(this.m_enemyEffectMods, (\u000E == null) ? null : \u000E.m_enemyHitEffectFields, "Enemy Effect Field Mods");
			string str = text;
			IntFieldListModData allyIntFieldMods = this.m_allyIntFieldMods;
			List<OnHitIntField> u000E;
			if (\u000E != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(OnHitDataMod.\u001D(string, OnHitAuthoredData)).MethodHandle;
				}
				u000E = \u000E.m_allyHitIntFields;
			}
			else
			{
				u000E = null;
			}
			text = str + OnHitDataMod.\u001D(allyIntFieldMods, u000E, "Ally Int Field Mods");
			string str2 = text;
			EffectFieldListModData allyEffectMods = this.m_allyEffectMods;
			List<OnHitEffecField> u000E2;
			if (\u000E != null)
			{
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
				u000E2 = \u000E.m_allyHitEffectFields;
			}
			else
			{
				u000E2 = null;
			}
			text = str2 + OnHitDataMod.\u001D(allyEffectMods, u000E2, "Ally Effect Field Mods");
			if (text.Length > 0)
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
				text = InEditorDescHelper.ColoredString(\u001D, "yellow", false) + "\n" + text + "\n";
			}
			return text;
		}

		public static string \u001D(IntFieldListModData \u001D, List<OnHitIntField> \u000E, string \u0012)
		{
			string text = string.Empty;
			if (\u001D.m_prependIntFields != null)
			{
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(OnHitDataMod.\u001D(IntFieldListModData, List<OnHitIntField>, string)).MethodHandle;
				}
				if (\u001D.m_prependIntFields.Count > 0)
				{
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
					text = text + "<color=cyan>" + \u0012 + ": New entries prepended:</color>\n";
					for (int i = 0; i < \u001D.m_prependIntFields.Count; i++)
					{
						OnHitIntField onHitIntField = \u001D.m_prependIntFields[i];
						text += onHitIntField.GetInEditorDesc();
					}
				}
			}
			if (\u001D.m_overrides != null)
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
				if (\u001D.m_overrides.Count > 0)
				{
					text = text + "<color=cyan>" + \u0012 + ": Override to existing entry:</color>\n";
					for (int j = 0; j < \u001D.m_overrides.Count; j++)
					{
						IntFieldOverride intFieldOverride = \u001D.m_overrides[j];
						string text2 = intFieldOverride.\u001D();
						if (!string.IsNullOrEmpty(text2))
						{
							text = text + "Target Identifier: " + InEditorDescHelper.ColoredString(intFieldOverride.m_targetIdentifier, "white", false) + "\n";
							if (\u000E != null)
							{
								for (;;)
								{
									switch (1)
									{
									case 0:
										continue;
									}
									break;
								}
								bool flag = false;
								foreach (OnHitIntField onHitIntField2 in \u000E)
								{
									if (onHitIntField2.GetIdentifier().Equals(text2, StringComparison.OrdinalIgnoreCase))
									{
										flag = true;
										text += intFieldOverride.m_fieldOverride.\u001D(onHitIntField2);
										break;
									}
								}
								if (!flag)
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
									text = text + "<color=red>Target Identifier " + text2 + " not found on base on hit data</color>\n";
								}
							}
						}
					}
				}
			}
			return text;
		}

		public static string \u001D(EffectFieldListModData \u001D, List<OnHitEffecField> \u000E, string \u0012)
		{
			string text = string.Empty;
			if (\u001D.m_prependEffectFields != null)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(OnHitDataMod.\u001D(EffectFieldListModData, List<OnHitEffecField>, string)).MethodHandle;
				}
				if (\u001D.m_prependEffectFields.Count > 0)
				{
					text = text + "<color=cyan>" + \u0012 + ": New entries prepended:</color>\n";
					for (int i = 0; i < \u001D.m_prependEffectFields.Count; i++)
					{
						OnHitEffecField onHitEffecField = \u001D.m_prependEffectFields[i];
						text += onHitEffecField.GetInEditorDesc(false, null);
					}
					for (;;)
					{
						switch (4)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			if (\u001D.m_overrides != null)
			{
				for (;;)
				{
					switch (2)
					{
					case 0:
						continue;
					}
					break;
				}
				if (\u001D.m_overrides.Count > 0)
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
					text = text + "<color=cyan>" + \u0012 + ": Override to existing entry:</color>\n";
					for (int j = 0; j < \u001D.m_overrides.Count; j++)
					{
						EffectFieldOverride effectFieldOverride = \u001D.m_overrides[j];
						string text2 = effectFieldOverride.\u001D();
						if (!string.IsNullOrEmpty(text2))
						{
							OnHitEffecField onHitEffecField2 = null;
							if (\u000E != null)
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
								using (List<OnHitEffecField>.Enumerator enumerator = \u000E.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										OnHitEffecField onHitEffecField3 = enumerator.Current;
										if (onHitEffecField3.GetIdentifier().Equals(text2, StringComparison.OrdinalIgnoreCase))
										{
											for (;;)
											{
												switch (4)
												{
												case 0:
													continue;
												}
												break;
											}
											onHitEffecField2 = onHitEffecField3;
											goto IL_16E;
										}
									}
									for (;;)
									{
										switch (2)
										{
										case 0:
											continue;
										}
										break;
									}
								}
								IL_16E:
								if (onHitEffecField2 == null)
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
									text = text + "<color=red>Target Identifier " + text2 + " not found on base on hit data</color>\n";
								}
							}
							text = text + "Target Identifier: " + InEditorDescHelper.ColoredString(effectFieldOverride.m_targetIdentifier, "white", false) + "\n";
							text += effectFieldOverride.m_effectOverride.GetInEditorDesc(onHitEffecField2 != null, onHitEffecField2);
						}
					}
					for (;;)
					{
						switch (5)
						{
						case 0:
							continue;
						}
						break;
					}
				}
			}
			return text;
		}

		public void \u001D(List<TooltipTokenEntry> \u001D, OnHitAuthoredData \u000E)
		{
			if (\u000E != null && this.m_enemyIntFieldMods != null)
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(OnHitDataMod.\u001D(List<TooltipTokenEntry>, OnHitAuthoredData)).MethodHandle;
				}
				OnHitDataMod.\u001D(\u001D, this.m_enemyIntFieldMods, \u000E.m_enemyHitIntFields);
				OnHitDataMod.\u001D(\u001D, this.m_enemyEffectMods, \u000E.m_enemyHitEffectFields);
				OnHitDataMod.\u001D(\u001D, this.m_allyIntFieldMods, \u000E.m_allyHitIntFields);
				OnHitDataMod.\u001D(\u001D, this.m_allyEffectMods, \u000E.m_allyHitEffectFields);
			}
		}

		public static void \u001D(List<TooltipTokenEntry> \u001D, IntFieldListModData \u000E, List<OnHitIntField> \u0012)
		{
			if (\u000E.m_prependIntFields != null)
			{
				for (int i = 0; i < \u000E.m_prependIntFields.Count; i++)
				{
					OnHitIntField onHitIntField = \u000E.m_prependIntFields[i];
					string identifier = onHitIntField.GetIdentifier();
					if (!string.IsNullOrEmpty(identifier))
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
						if (!true)
						{
							RuntimeMethodHandle runtimeMethodHandle = methodof(OnHitDataMod.\u001D(List<TooltipTokenEntry>, IntFieldListModData, List<OnHitIntField>)).MethodHandle;
						}
						onHitIntField.AddTooltipTokens(\u001D);
					}
				}
				for (;;)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (\u000E.m_overrides != null)
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
				for (int j = 0; j < \u000E.m_overrides.Count; j++)
				{
					IntFieldOverride intFieldOverride = \u000E.m_overrides[j];
					string text = intFieldOverride.\u001D();
					if (!string.IsNullOrEmpty(text))
					{
						OnHitIntField onHitIntField2 = null;
						using (List<OnHitIntField>.Enumerator enumerator = \u0012.GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								OnHitIntField onHitIntField3 = enumerator.Current;
								string identifier2 = onHitIntField3.GetIdentifier();
								if (text.Equals(identifier2, StringComparison.OrdinalIgnoreCase))
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
									onHitIntField2 = onHitIntField3;
									goto IL_10B;
								}
							}
							for (;;)
							{
								switch (7)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						IL_10B:
						if (onHitIntField2 != null)
						{
							for (;;)
							{
								switch (2)
								{
								case 0:
									continue;
								}
								break;
							}
							intFieldOverride.m_fieldOverride.\u001D(\u001D, onHitIntField2, text);
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

		public static void \u001D(List<TooltipTokenEntry> \u001D, EffectFieldListModData \u000E, List<OnHitEffecField> \u0012)
		{
			if (\u000E.m_prependEffectFields != null)
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
					RuntimeMethodHandle runtimeMethodHandle = methodof(OnHitDataMod.\u001D(List<TooltipTokenEntry>, EffectFieldListModData, List<OnHitEffecField>)).MethodHandle;
				}
				for (int i = 0; i < \u000E.m_prependEffectFields.Count; i++)
				{
					OnHitEffecField onHitEffecField = \u000E.m_prependEffectFields[i];
					string identifier = onHitEffecField.GetIdentifier();
					if (!string.IsNullOrEmpty(identifier))
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
						onHitEffecField.AddTooltipTokens(\u001D, false, null, null);
					}
				}
				for (;;)
				{
					switch (4)
					{
					case 0:
						continue;
					}
					break;
				}
			}
			if (\u000E.m_overrides != null)
			{
				for (int j = 0; j < \u000E.m_overrides.Count; j++)
				{
					EffectFieldOverride effectFieldOverride = \u000E.m_overrides[j];
					string text = effectFieldOverride.\u001D();
					if (!string.IsNullOrEmpty(text))
					{
						for (;;)
						{
							switch (2)
							{
							case 0:
								continue;
							}
							break;
						}
						OnHitEffecField onHitEffecField2 = null;
						using (List<OnHitEffecField>.Enumerator enumerator = \u0012.GetEnumerator())
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
							for (;;)
							{
								switch (3)
								{
								case 0:
									continue;
								}
								break;
							}
						}
						IL_112:
						if (onHitEffecField2 != null)
						{
							effectFieldOverride.m_effectOverride.AddTooltipTokens(\u001D, true, onHitEffecField2, text);
						}
					}
				}
			}
		}
	}
}
