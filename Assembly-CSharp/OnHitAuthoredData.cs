using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class OnHitAuthoredData
{
	[Header("-- For Enemy Hits --")]
	public List<OnHitIntField> m_enemyHitIntFields = new List<OnHitIntField>();
	public List<OnHitEffecField> m_enemyHitEffectFields = new List<OnHitEffecField>();
	[Header("-- For Ally Hits --")]
	public List<OnHitIntField> m_allyHitIntFields = new List<OnHitIntField>();
	public List<OnHitEffecField> m_allyHitEffectFields = new List<OnHitEffecField>();
	[Header("-- For Barriers --")]
	public List<OnHitBarrierField> m_barrierSpawnFields = new List<OnHitBarrierField>();

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens)
	{
		AddTooltipTokens_IntList(tokens, m_enemyHitIntFields);
		AddTooltipTokens_EffectFields(tokens, m_enemyHitEffectFields);
		AddTooltipTokens_IntList(tokens, m_allyHitIntFields);
		AddTooltipTokens_EffectFields(tokens, m_allyHitEffectFields);
		AddTooltipTokens_BarrierFields(tokens, m_barrierSpawnFields);
	}

	public int GetFirstDamageValue()
	{
		int result = 0;
		if (m_enemyHitIntFields != null && m_enemyHitIntFields.Count > 0)
		{
			result = m_enemyHitIntFields[0].m_baseValue;
		}
		return result;
	}

	public static void AddTooltipTokens_IntList(List<TooltipTokenEntry> tokens, List<OnHitIntField> intFields)
	{
		foreach (OnHitIntField field in intFields)
		{
			field.AddTooltipTokens(tokens);
		}
	}

	public static void AddTooltipTokens_EffectFields(List<TooltipTokenEntry> tokens, List<OnHitEffecField> effectFields)
	{
		foreach (OnHitEffecField field in effectFields)
		{
			field.AddTooltipTokens(tokens, false, null);
		}
	}

	public static void AddTooltipTokens_BarrierFields(List<TooltipTokenEntry> tokens, List<OnHitBarrierField> barrierFields)
	{
		foreach (OnHitBarrierField field in barrierFields)
		{
			field.AddTooltipTokens(tokens);
		}
	}

	public string GetInEditorDesc()
	{
		string text = string.Empty;
		if (m_enemyHitIntFields.Count > 0)
		{
			text += InEditorDescHelper.ColoredString("==== Enemy Hit Numeric Fields ====\n", "yellow");
			foreach (OnHitIntField field in m_enemyHitIntFields)
			{
				text += field.GetInEditorDesc();
				text += "\n";
			}
			text += "\n";
		}
		if (m_enemyHitEffectFields.Count > 0)
		{
			text += InEditorDescHelper.ColoredString("==== Enemy Effects ====\n", "yellow");
			foreach (OnHitEffecField field in m_enemyHitEffectFields)
			{
				text += field.GetInEditorDesc(false, null);
				text += "\n";
			}
			text += "\n";
		}
		if (m_allyHitIntFields.Count > 0)
		{
			text += InEditorDescHelper.ColoredString("==== Ally Hit Numberic Fields ====\n", "yellow");
			foreach (OnHitIntField field in m_allyHitIntFields)
			{
				text += field.GetInEditorDesc();
				text += "\n";
			}
			text += "\n";
		}
		if (m_allyHitEffectFields.Count > 0)
		{
			text += InEditorDescHelper.ColoredString("==== Ally Effects ====\n", "yellow");
			foreach (OnHitEffecField field in m_allyHitEffectFields)
			{
				text += field.GetInEditorDesc(false, null);
				text += "\n";
			}
			text += "\n";
		}
		if (m_barrierSpawnFields.Count > 0)
		{
			text += InEditorDescHelper.ColoredString("==== Barrier Fields ====\n", "yellow");
			foreach (OnHitBarrierField field in m_barrierSpawnFields)
			{
				text += field.GetInEditorDesc();
				text += "\n";
			}
			text += "\n";
		}
		return text;
	}
}
