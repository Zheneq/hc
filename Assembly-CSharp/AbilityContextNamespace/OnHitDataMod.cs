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

		public OnHitAuthoredData GetModdedOnHitData(OnHitAuthoredData input)
		{
			return new OnHitAuthoredData
			{
				m_enemyHitIntFields = m_enemyIntFieldMods.GetModdedIntFieldList(input.m_enemyHitIntFields),
				m_enemyHitEffectFields = m_enemyEffectMods.GetModdedEffectFieldList(input.m_enemyHitEffectFields),
				m_allyHitIntFields = m_allyIntFieldMods.GetModdedIntFieldList(input.m_allyHitIntFields),
				m_allyHitEffectFields = m_allyEffectMods.GetModdedEffectFieldList(input.m_allyHitEffectFields)
			};
		}

		public string GetInEditorDesc(string header, OnHitAuthoredData baseOnHitData)
		{
			string text = "";
			text += OnHitDataMod.GetIntFieldModDesc(m_enemyIntFieldMods, baseOnHitData?.m_enemyHitIntFields, "Enemy Int Field Mods");
			text += OnHitDataMod.GetEffectFieldModDesc(m_enemyEffectMods, baseOnHitData?.m_enemyHitEffectFields, "Enemy Effect Field Mods");
			text += OnHitDataMod.GetIntFieldModDesc(m_allyIntFieldMods, baseOnHitData?.m_allyHitIntFields, "Ally Int Field Mods");
			text += OnHitDataMod.GetEffectFieldModDesc(m_allyEffectMods, baseOnHitData?.m_allyHitEffectFields, "Ally Effect Field Mods");
			if (text.Length > 0)
			{
				text = InEditorDescHelper.ColoredString(header, "yellow") + "\n" + text + "\n";
			}
			return text;
		}

		public static string GetIntFieldModDesc(IntFieldListModData intMods, List<OnHitIntField> baseIntFields, string header)
		{
			string text = "";
			if (intMods.m_prependIntFields != null && intMods.m_prependIntFields.Count > 0)
			{
				text += "<color=cyan>" + header + ": New entries prepended:</color>\n";
				for (int i = 0; i < intMods.m_prependIntFields.Count; i++)
				{
					OnHitIntField onHitIntField = intMods.m_prependIntFields[i];
					text += onHitIntField.GetInEditorDesc();
				}
			}
			if (intMods.m_overrides != null && intMods.m_overrides.Count > 0)
			{
				text += "<color=cyan>" + header + ": Override to existing entry:</color>\n";
				for (int j = 0; j < intMods.m_overrides.Count; j++)
				{
					IntFieldOverride intFieldOverride = intMods.m_overrides[j];
					string identifier = intFieldOverride.GetIdentifier();
					if (!string.IsNullOrEmpty(identifier))
					{
						text += "Target Identifier: " + InEditorDescHelper.ColoredString(intFieldOverride.m_targetIdentifier, "white") + "\n";
						if (baseIntFields != null)
						{
							bool isFound = false;
							foreach (OnHitIntField item in baseIntFields)
							{
								if (item.GetIdentifier().Equals(identifier, StringComparison.OrdinalIgnoreCase))
								{
									isFound = true;
									text += intFieldOverride.m_fieldOverride.GetInEditorDesc(item);
									break;
								}
							}
							if (!isFound)
							{
								text += "<color=red>Target Identifier " + identifier + " not found on base on hit data</color>\n";
							}
						}
					}
				}
			}
			return text;
		}

		public static string GetEffectFieldModDesc(EffectFieldListModData effectMods, List<OnHitEffecField> baseEffectFields, string header)
		{
			string text = "";
			if (effectMods.m_prependEffectFields != null && effectMods.m_prependEffectFields.Count > 0)
			{
				text += "<color=cyan>" + header + ": New entries prepended:</color>\n";
				for (int i = 0; i < effectMods.m_prependEffectFields.Count; i++)
				{
					OnHitEffecField onHitEffecField = effectMods.m_prependEffectFields[i];
					text += onHitEffecField.GetInEditorDesc(false, null);
				}
			}
			if (effectMods.m_overrides != null && effectMods.m_overrides.Count > 0)
			{
				text += "<color=cyan>" + header + ": Override to existing entry:</color>\n";
				for (int j = 0; j < effectMods.m_overrides.Count; j++)
				{
					EffectFieldOverride effectFieldOverride = effectMods.m_overrides[j];
					string text2 = effectFieldOverride.GetIdentifier();
					if (!string.IsNullOrEmpty(text2))
					{
						OnHitEffecField onHitEffecField2 = null;
						if (baseEffectFields != null)
						{
							foreach (OnHitEffecField field in baseEffectFields)
							{
								if (field.GetIdentifier().Equals(text2, StringComparison.OrdinalIgnoreCase))
								{
									onHitEffecField2 = field;
									break;
								}
							}
							if (onHitEffecField2 == null)
							{
								text += "<color=red>Target Identifier " + text2 + " not found on base on hit data</color>\n";
							}
						}
						text += "Target Identifier: " + InEditorDescHelper.ColoredString(effectFieldOverride.m_targetIdentifier, "white") + "\n";
						text += effectFieldOverride.m_effectOverride.GetInEditorDesc(onHitEffecField2 != null, onHitEffecField2);
					}
				}
			}
			return text;
		}

		public void AddTooltipTokens(List<TooltipTokenEntry> tokens, OnHitAuthoredData baseOnHitData)
		{
			if (baseOnHitData != null && m_enemyIntFieldMods != null)
			{
				OnHitDataMod.AddTooltipTokens_IntFields(tokens, m_enemyIntFieldMods, baseOnHitData.m_enemyHitIntFields);
				OnHitDataMod.AddTooltipTokens_EffectFields(tokens, m_enemyEffectMods, baseOnHitData.m_enemyHitEffectFields);
				OnHitDataMod.AddTooltipTokens_IntFields(tokens, m_allyIntFieldMods, baseOnHitData.m_allyHitIntFields);
				OnHitDataMod.AddTooltipTokens_EffectFields(tokens, m_allyEffectMods, baseOnHitData.m_allyHitEffectFields);
			}
		}

		public static void AddTooltipTokens_IntFields(List<TooltipTokenEntry> tokens, IntFieldListModData intMods, List<OnHitIntField> baseIntFields)
		{
			if (intMods.m_prependIntFields != null)
			{
				for (int i = 0; i < intMods.m_prependIntFields.Count; i++)
				{
					OnHitIntField onHitIntField = intMods.m_prependIntFields[i];
					if (!string.IsNullOrEmpty(onHitIntField.GetIdentifier()))
					{
						onHitIntField.AddTooltipTokens(tokens);
					}
				}
			}
			if (intMods.m_overrides != null)
			{
				for (int j = 0; j < intMods.m_overrides.Count; j++)
				{
					IntFieldOverride intFieldOverride = intMods.m_overrides[j];
					string identifier = intFieldOverride.GetIdentifier();
					if (!string.IsNullOrEmpty(identifier))
					{
						OnHitIntField onHitIntField2 = null;
						foreach (OnHitIntField current in baseIntFields)
						{
							string identifier2 = current.GetIdentifier();
							if (identifier.Equals(identifier2, StringComparison.OrdinalIgnoreCase))
							{
								onHitIntField2 = current;
								break;
							}
						}
						if (onHitIntField2 != null)
						{
							intFieldOverride.m_fieldOverride.AddTooltipTokens(tokens, onHitIntField2, identifier);
						}
					}
				}
			}
		}

		public static void AddTooltipTokens_EffectFields(List<TooltipTokenEntry> tokens, EffectFieldListModData effectMods, List<OnHitEffecField> baseEffectFields)
		{
			if (effectMods.m_prependEffectFields != null)
			{
				for (int i = 0; i < effectMods.m_prependEffectFields.Count; i++)
				{
					OnHitEffecField onHitEffecField = effectMods.m_prependEffectFields[i];
					if (!string.IsNullOrEmpty(onHitEffecField.GetIdentifier()))
					{
						onHitEffecField.AddTooltipTokens(tokens, false, null);
					}
				}
			}
			if (effectMods.m_overrides != null)
			{
				for (int j = 0; j < effectMods.m_overrides.Count; j++)
				{
					EffectFieldOverride effectFieldOverride = effectMods.m_overrides[j];
					string identifier = effectFieldOverride.GetIdentifier();
					if (!string.IsNullOrEmpty(identifier))
					{
						OnHitEffecField onHitEffecField2 = null;
						foreach (OnHitEffecField current in baseEffectFields)
						{
							string identifier2 = current.GetIdentifier();
							if (identifier.Equals(identifier2, StringComparison.OrdinalIgnoreCase))
							{
								onHitEffecField2 = current;
								break;
							}
						}
						if (onHitEffecField2 != null)
						{
							effectFieldOverride.m_effectOverride.AddTooltipTokens(tokens, true, onHitEffecField2, identifier);
						}
					}
				}
			}
		}
	}
}
