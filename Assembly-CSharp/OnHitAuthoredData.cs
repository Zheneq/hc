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
		OnHitAuthoredData.AddTooltipTokens_IntList(tokens, this.m_enemyHitIntFields);
		OnHitAuthoredData.AddTooltipTokens_EffectFields(tokens, this.m_enemyHitEffectFields);
		OnHitAuthoredData.AddTooltipTokens_IntList(tokens, this.m_allyHitIntFields);
		OnHitAuthoredData.AddTooltipTokens_EffectFields(tokens, this.m_allyHitEffectFields);
		OnHitAuthoredData.AddTooltipTokens_BarrierFields(tokens, this.m_barrierSpawnFields);
	}

	public int GetFirstDamageValue()
	{
		int result = 0;
		if (this.m_enemyHitIntFields != null && this.m_enemyHitIntFields.Count > 0)
		{
			result = this.m_enemyHitIntFields[0].m_baseValue;
		}
		return result;
	}

	public static void AddTooltipTokens_IntList(List<TooltipTokenEntry> tokens, List<OnHitIntField> intFields)
	{
		for (int i = 0; i < intFields.Count; i++)
		{
			intFields[i].AddTooltipTokens(tokens);
		}
	}

	public static void AddTooltipTokens_EffectFields(List<TooltipTokenEntry> tokens, List<OnHitEffecField> effectFields)
	{
		for (int i = 0; i < effectFields.Count; i++)
		{
			effectFields[i].AddTooltipTokens(tokens, false, null, null);
		}
	}

	public static void AddTooltipTokens_BarrierFields(List<TooltipTokenEntry> tokens, List<OnHitBarrierField> barrierFields)
	{
		for (int i = 0; i < barrierFields.Count; i++)
		{
			barrierFields[i].AddTooltipTokens(tokens);
		}
	}

	public string GetInEditorDesc()
	{
		string text = string.Empty;
		if (this.m_enemyHitIntFields.Count > 0)
		{
			text += InEditorDescHelper.ColoredString("==== Enemy Hit Numeric Fields ====\n", "yellow", false);
			using (List<OnHitIntField>.Enumerator enumerator = this.m_enemyHitIntFields.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					OnHitIntField onHitIntField = enumerator.Current;
					text += onHitIntField.GetInEditorDesc();
					text += "\n";
				}
			}
			text += "\n";
		}
		if (this.m_enemyHitEffectFields.Count > 0)
		{
			text += InEditorDescHelper.ColoredString("==== Enemy Effects ====\n", "yellow", false);
			foreach (OnHitEffecField onHitEffecField in this.m_enemyHitEffectFields)
			{
				text += onHitEffecField.GetInEditorDesc(false, null);
				text += "\n";
			}
			text += "\n";
		}
		if (this.m_allyHitIntFields.Count > 0)
		{
			text += InEditorDescHelper.ColoredString("==== Ally Hit Numberic Fields ====\n", "yellow", false);
			using (List<OnHitIntField>.Enumerator enumerator3 = this.m_allyHitIntFields.GetEnumerator())
			{
				while (enumerator3.MoveNext())
				{
					OnHitIntField onHitIntField2 = enumerator3.Current;
					text += onHitIntField2.GetInEditorDesc();
					text += "\n";
				}
			}
			text += "\n";
		}
		if (this.m_allyHitEffectFields.Count > 0)
		{
			text += InEditorDescHelper.ColoredString("==== Ally Effects ====\n", "yellow", false);
			using (List<OnHitEffecField>.Enumerator enumerator4 = this.m_allyHitEffectFields.GetEnumerator())
			{
				while (enumerator4.MoveNext())
				{
					OnHitEffecField onHitEffecField2 = enumerator4.Current;
					text += onHitEffecField2.GetInEditorDesc(false, null);
					text += "\n";
				}
			}
			text += "\n";
		}
		if (this.m_barrierSpawnFields.Count > 0)
		{
			text += InEditorDescHelper.ColoredString("==== Barrier Fields ====\n", "yellow", false);
			using (List<OnHitBarrierField>.Enumerator enumerator5 = this.m_barrierSpawnFields.GetEnumerator())
			{
				while (enumerator5.MoveNext())
				{
					OnHitBarrierField onHitBarrierField = enumerator5.Current;
					text += onHitBarrierField.GetInEditorDesc();
					text += "\n";
				}
			}
			text += "\n";
		}
		return text;
	}
}
