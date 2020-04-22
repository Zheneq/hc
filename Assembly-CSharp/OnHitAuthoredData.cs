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
		for (int i = 0; i < intFields.Count; i++)
		{
			intFields[i].AddTooltipTokens(tokens);
		}
		while (true)
		{
			return;
		}
	}

	public static void AddTooltipTokens_EffectFields(List<TooltipTokenEntry> tokens, List<OnHitEffecField> effectFields)
	{
		for (int i = 0; i < effectFields.Count; i++)
		{
			effectFields[i].AddTooltipTokens(tokens, false, null);
		}
		while (true)
		{
			return;
		}
	}

	public static void AddTooltipTokens_BarrierFields(List<TooltipTokenEntry> tokens, List<OnHitBarrierField> barrierFields)
	{
		for (int i = 0; i < barrierFields.Count; i++)
		{
			barrierFields[i].AddTooltipTokens(tokens);
		}
		while (true)
		{
			return;
		}
	}

	public string GetInEditorDesc()
	{
		string text = string.Empty;
		if (m_enemyHitIntFields.Count > 0)
		{
			text += InEditorDescHelper.ColoredString("==== Enemy Hit Numeric Fields ====\n", "yellow");
			using (List<OnHitIntField>.Enumerator enumerator = m_enemyHitIntFields.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					OnHitIntField current = enumerator.Current;
					text += current.GetInEditorDesc();
					text += "\n";
				}
				while (true)
				{
					switch (4)
					{
					case 0:
						break;
					default:
						goto end_IL_0042;
					}
				}
				end_IL_0042:;
			}
			text += "\n";
		}
		if (m_enemyHitEffectFields.Count > 0)
		{
			text += InEditorDescHelper.ColoredString("==== Enemy Effects ====\n", "yellow");
			foreach (OnHitEffecField enemyHitEffectField in m_enemyHitEffectFields)
			{
				text += enemyHitEffectField.GetInEditorDesc(false, null);
				text += "\n";
			}
			text += "\n";
		}
		if (m_allyHitIntFields.Count > 0)
		{
			text += InEditorDescHelper.ColoredString("==== Ally Hit Numberic Fields ====\n", "yellow");
			using (List<OnHitIntField>.Enumerator enumerator3 = m_allyHitIntFields.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					OnHitIntField current3 = enumerator3.Current;
					text += current3.GetInEditorDesc();
					text += "\n";
				}
			}
			text += "\n";
		}
		if (m_allyHitEffectFields.Count > 0)
		{
			text += InEditorDescHelper.ColoredString("==== Ally Effects ====\n", "yellow");
			using (List<OnHitEffecField>.Enumerator enumerator4 = m_allyHitEffectFields.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					OnHitEffecField current4 = enumerator4.Current;
					text += current4.GetInEditorDesc(false, null);
					text += "\n";
				}
			}
			text += "\n";
		}
		if (m_barrierSpawnFields.Count > 0)
		{
			text += InEditorDescHelper.ColoredString("==== Barrier Fields ====\n", "yellow");
			using (List<OnHitBarrierField>.Enumerator enumerator5 = m_barrierSpawnFields.GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					OnHitBarrierField current5 = enumerator5.Current;
					text += current5.GetInEditorDesc();
					text += "\n";
				}
			}
			text += "\n";
		}
		return text;
	}
}
