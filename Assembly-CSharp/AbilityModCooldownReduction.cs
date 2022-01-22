using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityModCooldownReduction
{
	public enum ModAmountType
	{
		FlatOnCast,
		FlatOnAnyNonSelfHit,
		FlatOnAnyAllyHit,
		FlatOnAnyEnemyHit,
		MultPerAllyHit,
		MultPerEnemyHit,
		FlatOnNoEnemyHit,
		FlatOnNoAllyHit,
		FlatOnNoNonCasterHit
	}

	public AbilityData.ActionType m_onAbility = AbilityData.ActionType.INVALID_ACTION;
	public List<AbilityData.ActionType> m_additionalAbilities = new List<AbilityData.ActionType>();
	public ModAmountType m_modAmountType;

	[Header("[Flat => baseVal + finalAdd], [Mult => Round(baseVal*numTargets) + finalAdd]")]
	public float m_baseValue;
	public float m_finalAdd;

	[Space(10f)]
	public int m_minReduction;
	public int m_maxReduction = -1;

	[Space(10f)]
	[Header("[Stock/Charge Refresh]")]
	public List<AbilityData.ActionType> m_stockAbilities = new List<AbilityData.ActionType>();
	public int m_stockBaseValue;
	public int m_stockFinalAdd;
	public int m_refreshProgressBaseValue;
	public int m_refreshProgressFinalAdd;
	public bool m_resetRefreshProgress;

	public bool HasCooldownReduction()
	{
		if (IsValidActionType(m_onAbility))
		{
			return true;
		}
		foreach (AbilityData.ActionType additionalAbility in m_additionalAbilities)
		{
			if (IsValidActionType(additionalAbility))
			{
				return true;
			}
		}
		return false;
	}

	private bool IsValidActionType(AbilityData.ActionType actionType)
	{
		return actionType != AbilityData.ActionType.INVALID_ACTION
			&& (m_baseValue != 0f
				|| m_finalAdd != 0f
				|| m_stockBaseValue != 0
				|| m_stockFinalAdd != 0
				|| m_refreshProgressBaseValue != 0
				|| m_refreshProgressFinalAdd != 0);
	}

	public int CalcFinalCooldownReduction(bool selfHit, int numAlliesHit, int numEnemiesHit)
	{
		int num = 0;
		if (m_modAmountType == ModAmountType.FlatOnCast
			|| m_modAmountType == ModAmountType.FlatOnAnyNonSelfHit && (numAlliesHit > 0 || numEnemiesHit > 0)
			|| m_modAmountType == ModAmountType.FlatOnAnyAllyHit && numAlliesHit > 0
			|| m_modAmountType == ModAmountType.FlatOnAnyEnemyHit && numEnemiesHit > 0
			|| m_modAmountType == ModAmountType.FlatOnNoEnemyHit && numEnemiesHit == 0
			|| m_modAmountType == ModAmountType.FlatOnNoAllyHit && numAlliesHit == 0
			|| m_modAmountType == ModAmountType.FlatOnNoNonCasterHit && numEnemiesHit == 0 && numAlliesHit == 0)
		{
			num = Mathf.RoundToInt(m_baseValue + m_finalAdd);
		}
		else if (m_modAmountType == ModAmountType.MultPerAllyHit)
		{
			num = Mathf.RoundToInt(m_baseValue * numAlliesHit + m_finalAdd);
		}
		else if (m_modAmountType == ModAmountType.MultPerEnemyHit)
		{
			num = Mathf.RoundToInt(m_baseValue * numEnemiesHit + m_finalAdd);
		}
		int num2 = Mathf.Max(m_minReduction, num);
		if (m_maxReduction > 0)
		{
			num2 = Mathf.Min(m_maxReduction, num2);
		}
		return num2;
	}

	public int CalcFinalStockRefresh(bool selfHit, int numAlliesHit, int numEnemiesHit, out int refreshProgress)
	{
		int result = 0;
		refreshProgress = 0;
		if (m_modAmountType == ModAmountType.FlatOnCast
			|| m_modAmountType == ModAmountType.FlatOnAnyNonSelfHit && (numAlliesHit > 0 || numEnemiesHit > 0)
			|| m_modAmountType == ModAmountType.FlatOnAnyAllyHit && numAlliesHit > 0
			|| m_modAmountType == ModAmountType.FlatOnAnyEnemyHit && numEnemiesHit > 0)
		{
			result = Mathf.RoundToInt(m_stockBaseValue + m_stockFinalAdd);
			refreshProgress = Mathf.RoundToInt(m_refreshProgressBaseValue + m_refreshProgressFinalAdd);
		}
		else if (m_modAmountType == ModAmountType.MultPerAllyHit)
		{
			result = Mathf.RoundToInt(m_stockBaseValue * numAlliesHit + m_stockFinalAdd);
			refreshProgress = Mathf.RoundToInt(m_refreshProgressBaseValue * numAlliesHit + m_refreshProgressFinalAdd);
		}
		else if (m_modAmountType == ModAmountType.MultPerEnemyHit)
		{
			result = Mathf.RoundToInt(m_stockBaseValue * numEnemiesHit + m_stockFinalAdd);
			refreshProgress = Mathf.RoundToInt(m_refreshProgressBaseValue * numEnemiesHit + m_refreshProgressFinalAdd);
		}
		return result;
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> entries, string name)
	{
		if (HasCooldownReduction())
		{
			int val = CalcFinalCooldownReduction(true, 1, 1);
			if (m_onAbility != AbilityData.ActionType.INVALID_ACTION)
			{
				entries.Add(new TooltipTokenInt(name + "_CDR_" + m_onAbility, "cooldown reduction", val));
			}
			foreach (AbilityData.ActionType actionType in m_additionalAbilities)
			{
				if (actionType != AbilityData.ActionType.INVALID_ACTION)
				{
					entries.Add(new TooltipTokenInt(name + "CRD_" + actionType, "cooldown reduction", val));
				}
			}
		}
	}

	public string GetDescription(AbilityData abilityData)
	{
		string text = string.Concat(new string[]
		{
			InEditorDescHelper.ColoredString(m_baseValue.ToString()),
			" ",
			InEditorDescHelper.ColoredString(m_modAmountType.ToString(), "lime"),
			" ",
			InEditorDescHelper.ColoredString((m_finalAdd >= 0f ? " +" : "") + m_finalAdd)
		});
		string text2 = "\t<color=white>" + AbilityModHelper.GetAbilityNameFromActionType(m_onAbility, abilityData) + "</color>\n";
		foreach (AbilityData.ActionType actionType in m_additionalAbilities)
		{
			text2 += "\t<color=white>" + AbilityModHelper.GetAbilityNameFromActionType(actionType, abilityData) + "</color>\n";
		}
		text += "\nCooldown Reduction On Abilities:\n" + text2;
		text2 = "";
		foreach (AbilityData.ActionType actionType2 in m_stockAbilities)
		{
			text2 += "\t<color=white>" + AbilityModHelper.GetAbilityNameFromActionType(actionType2, abilityData) + "</color>\n";
		}
		if (m_stockBaseValue != 0 || m_stockFinalAdd != 0)
		{
			text = string.Concat(new string[]
			{
				text,
				"\n",
				InEditorDescHelper.ColoredString(m_stockBaseValue.ToString()),
				" ",
				InEditorDescHelper.ColoredString(m_modAmountType.ToString(), "lime"),
				" ",
				InEditorDescHelper.ColoredString((m_stockFinalAdd >= 0 ? " +" : "") + m_stockFinalAdd)
			});
			text += "\nStock Add On Abilities:\n" + text2;
		}
		if (m_resetRefreshProgress)
		{
			text += "\nReset Stock Refresh Progress On Abilities:\n" + text2;
		}
		else if (m_refreshProgressBaseValue != 0 || m_refreshProgressFinalAdd != 0)
		{
			text = string.Concat(new string[]
			{
				text,
				"\n",
				InEditorDescHelper.ColoredString(m_refreshProgressBaseValue.ToString()),
				" ",
				InEditorDescHelper.ColoredString(m_modAmountType.ToString(), "lime"),
				" ",
				InEditorDescHelper.ColoredString((m_refreshProgressFinalAdd >= 0 ? " +" : "") + m_refreshProgressFinalAdd)
			});
			text += "\nStock Refresh Progress On Abilities:\n" + text2;
		}
		return text;
	}
}
