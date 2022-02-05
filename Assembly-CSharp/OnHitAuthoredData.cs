// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

// TODO ROGUES serializable fields added, probably they are for rogues but who knows
[Serializable]
public class OnHitAuthoredData
{
	// added in rogues
//#if SERVER
//	[Header("-- For All Hits --")]
//	public List<OnHitEffectTemplateField> m_effectTemplateFields = new List<OnHitEffectTemplateField>();
//#endif

	[Header("-- For Enemy Hits --")]
	public List<OnHitIntField> m_enemyHitIntFields = new List<OnHitIntField>();

	// added in rogues
//#if SERVER
//	public List<OnHitKnockbackField> m_enemyHitKnockbackFields = new List<OnHitKnockbackField>();
//#endif

	// added in rogues
//#if SERVER
//	public List<OnHitCooldownReductionField> m_enemyHitCooldownReductionFields = new List<OnHitCooldownReductionField>();
//#endif

	public List<OnHitEffecField> m_enemyHitEffectFields = new List<OnHitEffecField>();

	// added in rogues
//#if SERVER
//	public List<OnHitEffectTemplateField> m_enemyHitEffectTemplateFields = new List<OnHitEffectTemplateField>();
//#endif

	[Header("-- For Ally Hits --")]
	public List<OnHitIntField> m_allyHitIntFields = new List<OnHitIntField>();

	// added in rogues
//#if SERVER
//	public List<OnHitKnockbackField> m_allyHitKnockbackFields = new List<OnHitKnockbackField>();
//#endif

	// added in rogues
//#if SERVER
//	public List<OnHitCooldownReductionField> m_allyHitCooldownReductionFields = new List<OnHitCooldownReductionField>();
//#endif

	public List<OnHitEffecField> m_allyHitEffectFields = new List<OnHitEffecField>();

	// added in rogues
//#if SERVER
//	public List<OnHitEffectTemplateField> m_allyHitEffectTemplateFields = new List<OnHitEffectTemplateField>();
//#endif

	[Header("-- For Barriers --")]
	public List<OnHitBarrierField> m_barrierSpawnFields = new List<OnHitBarrierField>();

	// added in rogues
	//#if SERVER
	//	[Header(" -- For Ground Effects --")]
	//	public List<OnHitGroundEffectField> m_groundEffectFields = new List<OnHitGroundEffectField>();
	//#endif

	// added in rogues
#if SERVER
	public void AppendDataFromOther(OnHitAuthoredData other)
	{
		m_enemyHitIntFields.AddRange(other.m_enemyHitIntFields);
		m_enemyHitEffectFields.AddRange(other.m_enemyHitEffectFields);
		m_allyHitIntFields.AddRange(other.m_allyHitIntFields);
		m_allyHitEffectFields.AddRange(other.m_allyHitEffectFields);
	}
#endif

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens)
	{
		// rogues?
		//AddTooltipTokens_EffectTemplateFields(tokens, m_effectTemplateFields);

		AddTooltipTokens_IntList(tokens, m_enemyHitIntFields);

		// added in rogues
		//AddTooltipTokens_KnockbackList(tokens, m_enemyHitKnockbackFields);

		AddTooltipTokens_EffectFields(tokens, m_enemyHitEffectFields);

		// rogues?
		//AddTooltipTokens_EffectTemplateFields(tokens, m_enemyHitEffectTemplateFields);

		// added in rogues
		//AddTooltipTokens_CooldownReductionList(tokens, m_enemyHitCooldownReductionFields);

		AddTooltipTokens_IntList(tokens, m_allyHitIntFields);
		AddTooltipTokens_EffectFields(tokens, m_allyHitEffectFields);

		// rogues?
		//AddTooltipTokens_EffectTemplateFields(tokens, m_allyHitEffectTemplateFields);

		// added in rogues
		//AddTooltipTokens_CooldownReductionList(tokens, m_allyHitCooldownReductionFields);

		AddTooltipTokens_BarrierFields(tokens, m_barrierSpawnFields);

		// added in rogues
		//AddTooltipTokens_GroundEffectFields(tokens, m_groundEffectFields);
	}

	// removed in rogues
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

	// added in rogues
//#if SERVER
//	public static void AddTooltipTokens_KnockbackList(List<TooltipTokenEntry> tokens, List<OnHitKnockbackField> intFields)
//	{
//		foreach (OnHitKnockbackField field in intFields)
//		{
//			field.AddTooltipTokens(tokens);
//		}
//	}
//#endif

	// added in rogues
//#if SERVER
//	public static void AddTooltipTokens_CooldownReductionList(List<TooltipTokenEntry> tokens, List<OnHitCooldownReductionField> intFields)
//	{
//		foreach (OnHitCooldownReductionField field in intFields)
//		{
//			field.AddTooltipTokens(tokens);
//		}
//	}
//#endif

	public static void AddTooltipTokens_EffectFields(List<TooltipTokenEntry> tokens, List<OnHitEffecField> effectFields)
	{
		foreach (OnHitEffecField field in effectFields)
		{
			field.AddTooltipTokens(tokens, false, null);
		}
	}

	// rogues?
	//public static void AddTooltipTokens_EffectTemplateFields(List<TooltipTokenEntry> tokens, List<OnHitEffectTemplateField> effectTemplateFields)
	//{
	//	foreach (OnHitEffectTemplateField onHitEffectTemplateField in effectTemplateFields)
	//	{
	//		onHitEffectTemplateField.AddTooltipTokens(tokens, false, null, null);
	//	}
	//}

	public static void AddTooltipTokens_BarrierFields(List<TooltipTokenEntry> tokens, List<OnHitBarrierField> barrierFields)
	{
		foreach (OnHitBarrierField field in barrierFields)
		{
			field.AddTooltipTokens(tokens);
		}
	}

	// added in rogues
//#if SERVER
//	public static void AddTooltipTokens_GroundEffectFields(List<TooltipTokenEntry> tokens, List<OnHitGroundEffectField> groundEffectFields)
//	{
//		foreach (OnHitGroundEffectField field in groundEffectFields)
//		{
//			field.AddTooltipTokens(tokens);
//		}
//	}
//#endif

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

		// rogues?
		//if (m_enemyHitEffectTemplateFields.Count > 0)
		//{
		//	text += InEditorDescHelper.ColoredString("==== Enemy (New) Effects ====\n", "yellow", false);
		//	foreach (OnHitEffectTemplateField onHitEffectTemplateField in m_enemyHitEffectTemplateFields)
		//	{
		//		text += onHitEffectTemplateField.GetInEditorDesc(false, null);
		//		text += "\n";
		//	}
		//	text += "\n";
		//}

		// added in rogues
		//if (m_enemyHitKnockbackFields.Count > 0)
		//{
		//	text += InEditorDescHelper.ColoredString("==== Enemy Knockbacks ====\n", "yellow", false);
		//	foreach (OnHitKnockbackField onHitKnockbackField in m_enemyHitKnockbackFields)
		//	{
		//		text += onHitKnockbackField.GetInEditorDesc();
		//		text += "\n";
		//	}
		//	text += "\n";
		//}

		// added in rogues
		//if (m_enemyHitCooldownReductionFields.Count > 0)
		//{
		//	text += InEditorDescHelper.ColoredString("==== Cooldown Reductions ====\n", "yellow", false);
		//	foreach (OnHitCooldownReductionField onHitCooldownReductionField in m_enemyHitCooldownReductionFields)
		//	{
		//		text += onHitCooldownReductionField.GetInEditorDesc();
		//		text += "\n";
		//	}
		//	text += "\n";
		//}

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

		// rogues?
		//if (m_allyHitEffectTemplateFields.Count > 0)
		//{
		//	text += InEditorDescHelper.ColoredString("==== Ally (NEW) Effects ====\n", "yellow", false);
		//	foreach (OnHitEffectTemplateField onHitEffectTemplateField2 in m_allyHitEffectTemplateFields)
		//	{
		//		text += onHitEffectTemplateField2.GetInEditorDesc(false, null);
		//		text += "\n";
		//	}
		//	text += "\n";
		//}

		// added in rogues
		//if (m_allyHitCooldownReductionFields.Count > 0)
		//{
		//	text += InEditorDescHelper.ColoredString("==== Cooldown Reductions ====\n", "yellow", false);
		//	foreach (OnHitCooldownReductionField onHitCooldownReductionField2 in m_allyHitCooldownReductionFields)
		//	{
		//		text += onHitCooldownReductionField2.GetInEditorDesc();
		//		text += "\n";
		//	}
		//	text += "\n";
		//}

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


		// rogues?
		//if (m_effectTemplateFields.Count > 0)
		//{
		//	text += InEditorDescHelper.ColoredString("==== Ability-wide Effect Templates ====\n", "yellow", false);
		//	foreach (OnHitEffectTemplateField onHitEffectTemplateField3 in m_effectTemplateFields)
		//	{
		//		text += onHitEffectTemplateField3.GetInEditorDesc(false, null);
		//		text += "\n";
		//	}
		//	text += "\n";
		//}

		return text;
	}
}
