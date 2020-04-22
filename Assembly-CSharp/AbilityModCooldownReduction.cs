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
				while (true)
				{
					switch (2)
					{
					case 0:
						break;
					default:
						return true;
					}
				}
			}
		}
		return false;
	}

	private bool IsValidActionType(AbilityData.ActionType actionType)
	{
		int result;
		if (actionType != AbilityData.ActionType.INVALID_ACTION)
		{
			if (m_baseValue == 0f)
			{
				if (m_finalAdd == 0f)
				{
					if (m_stockBaseValue == 0)
					{
						if (m_stockFinalAdd == 0 && m_refreshProgressBaseValue == 0)
						{
							result = ((m_refreshProgressFinalAdd != 0) ? 1 : 0);
							goto IL_0086;
						}
					}
				}
			}
			result = 1;
		}
		else
		{
			result = 0;
		}
		goto IL_0086;
		IL_0086:
		return (byte)result != 0;
	}

	public int CalcFinalCooldownReduction(bool selfHit, int numAlliesHit, int numEnemiesHit)
	{
		int b = 0;
		if (m_modAmountType != 0)
		{
			if (m_modAmountType != ModAmountType.FlatOnAnyNonSelfHit)
			{
				goto IL_0055;
			}
			if (numAlliesHit <= 0)
			{
				if (numEnemiesHit <= 0)
				{
					goto IL_0055;
				}
			}
		}
		goto IL_00f6;
		IL_0165:
		int num = Mathf.Max(m_minReduction, b);
		if (m_maxReduction > 0)
		{
			num = Mathf.Min(m_maxReduction, num);
		}
		return num;
		IL_00f6:
		b = Mathf.RoundToInt(m_baseValue + m_finalAdd);
		goto IL_0165;
		IL_0055:
		if (m_modAmountType == ModAmountType.FlatOnAnyAllyHit)
		{
			if (numAlliesHit > 0)
			{
				goto IL_00f6;
			}
		}
		if (m_modAmountType == ModAmountType.FlatOnAnyEnemyHit)
		{
			if (numEnemiesHit > 0)
			{
				goto IL_00f6;
			}
		}
		if (m_modAmountType == ModAmountType.FlatOnNoEnemyHit)
		{
			if (numEnemiesHit == 0)
			{
				goto IL_00f6;
			}
		}
		if (m_modAmountType == ModAmountType.FlatOnNoAllyHit)
		{
			if (numAlliesHit == 0)
			{
				goto IL_00f6;
			}
		}
		if (m_modAmountType == ModAmountType.FlatOnNoNonCasterHit)
		{
			if (numEnemiesHit == 0)
			{
				if (numAlliesHit == 0)
				{
					goto IL_00f6;
				}
			}
		}
		if (m_modAmountType == ModAmountType.MultPerAllyHit)
		{
			b = Mathf.RoundToInt(m_baseValue * (float)numAlliesHit + m_finalAdd);
		}
		else if (m_modAmountType == ModAmountType.MultPerEnemyHit)
		{
			b = Mathf.RoundToInt(m_baseValue * (float)numEnemiesHit + m_finalAdd);
		}
		goto IL_0165;
	}

	public int CalcFinalStockRefresh(bool selfHit, int numAlliesHit, int numEnemiesHit, out int refreshProgress)
	{
		int result = 0;
		refreshProgress = 0;
		if (m_modAmountType == ModAmountType.FlatOnCast)
		{
			goto IL_008b;
		}
		if (m_modAmountType == ModAmountType.FlatOnAnyNonSelfHit)
		{
			if (numAlliesHit > 0 || numEnemiesHit > 0)
			{
				goto IL_008b;
			}
		}
		if (m_modAmountType == ModAmountType.FlatOnAnyAllyHit)
		{
			if (numAlliesHit > 0)
			{
				goto IL_008b;
			}
		}
		if (m_modAmountType == ModAmountType.FlatOnAnyEnemyHit)
		{
			if (numEnemiesHit > 0)
			{
				goto IL_008b;
			}
		}
		if (m_modAmountType == ModAmountType.MultPerAllyHit)
		{
			result = Mathf.RoundToInt(m_stockBaseValue * numAlliesHit + m_stockFinalAdd);
			refreshProgress = Mathf.RoundToInt(m_refreshProgressBaseValue * numAlliesHit + m_refreshProgressFinalAdd);
		}
		else if (m_modAmountType == ModAmountType.MultPerEnemyHit)
		{
			result = Mathf.RoundToInt(m_stockBaseValue * numEnemiesHit + m_stockFinalAdd);
			refreshProgress = Mathf.RoundToInt(m_refreshProgressBaseValue * numEnemiesHit + m_refreshProgressFinalAdd);
		}
		goto IL_0148;
		IL_008b:
		result = Mathf.RoundToInt(m_stockBaseValue + m_stockFinalAdd);
		refreshProgress = Mathf.RoundToInt(m_refreshProgressBaseValue + m_refreshProgressFinalAdd);
		goto IL_0148;
		IL_0148:
		return result;
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> entries, string name)
	{
		if (!HasCooldownReduction())
		{
			return;
		}
		while (true)
		{
			int val = CalcFinalCooldownReduction(true, 1, 1);
			if (m_onAbility != AbilityData.ActionType.INVALID_ACTION)
			{
				entries.Add(new TooltipTokenInt(name + "_CDR_" + m_onAbility, "cooldown reduction", val));
			}
			using (List<AbilityData.ActionType>.Enumerator enumerator = m_additionalAbilities.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityData.ActionType current = enumerator.Current;
					if (current != AbilityData.ActionType.INVALID_ACTION)
					{
						entries.Add(new TooltipTokenInt(name + "CRD_" + current, "cooldown reduction", val));
					}
				}
				while (true)
				{
					switch (7)
					{
					default:
						return;
					case 0:
						break;
					}
				}
			}
		}
	}

	public string GetDescription(AbilityData abilityData)
	{
		string[] obj = new string[5]
		{
			InEditorDescHelper.ColoredString(m_baseValue.ToString()),
			" ",
			InEditorDescHelper.ColoredString(m_modAmountType.ToString(), "lime"),
			" ",
			null
		};
		object str;
		if (m_finalAdd >= 0f)
		{
			str = " +";
		}
		else
		{
			str = string.Empty;
		}
		obj[4] = InEditorDescHelper.ColoredString((string)str + m_finalAdd);
		string str2 = string.Concat(obj);
		string text = "\t<color=white>" + AbilityModHelper.GetAbilityNameFromActionType(m_onAbility, abilityData) + "</color>\n";
		using (List<AbilityData.ActionType>.Enumerator enumerator = m_additionalAbilities.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityData.ActionType current = enumerator.Current;
				text = text + "\t<color=white>" + AbilityModHelper.GetAbilityNameFromActionType(current, abilityData) + "</color>\n";
			}
		}
		str2 = str2 + "\nCooldown Reduction On Abilities:\n" + text;
		text = string.Empty;
		using (List<AbilityData.ActionType>.Enumerator enumerator2 = m_stockAbilities.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				AbilityData.ActionType current2 = enumerator2.Current;
				text = text + "\t<color=white>" + AbilityModHelper.GetAbilityNameFromActionType(current2, abilityData) + "</color>\n";
			}
		}
		if (m_stockBaseValue == 0)
		{
			if (m_stockFinalAdd == 0)
			{
				goto IL_028b;
			}
		}
		string text2 = str2;
		string[] obj2 = new string[7]
		{
			text2,
			"\n",
			InEditorDescHelper.ColoredString(m_stockBaseValue.ToString()),
			" ",
			InEditorDescHelper.ColoredString(m_modAmountType.ToString(), "lime"),
			" ",
			null
		};
		object str3;
		if (m_stockFinalAdd >= 0)
		{
			str3 = " +";
		}
		else
		{
			str3 = string.Empty;
		}
		obj2[6] = InEditorDescHelper.ColoredString((string)str3 + m_stockFinalAdd);
		str2 = string.Concat(obj2);
		str2 = str2 + "\nStock Add On Abilities:\n" + text;
		goto IL_028b;
		IL_039a:
		return str2;
		IL_028b:
		if (m_resetRefreshProgress)
		{
			str2 = str2 + "\nReset Stock Refresh Progress On Abilities:\n" + text;
		}
		else
		{
			if (m_refreshProgressBaseValue == 0)
			{
				if (m_refreshProgressFinalAdd == 0)
				{
					goto IL_039a;
				}
			}
			text2 = str2;
			string[] obj3 = new string[7]
			{
				text2,
				"\n",
				InEditorDescHelper.ColoredString(m_refreshProgressBaseValue.ToString()),
				" ",
				InEditorDescHelper.ColoredString(m_modAmountType.ToString(), "lime"),
				" ",
				null
			};
			object str4;
			if (m_refreshProgressFinalAdd >= 0)
			{
				str4 = " +";
			}
			else
			{
				str4 = string.Empty;
			}
			obj3[6] = InEditorDescHelper.ColoredString((string)str4 + m_refreshProgressFinalAdd);
			str2 = string.Concat(obj3);
			str2 = str2 + "\nStock Refresh Progress On Abilities:\n" + text;
		}
		goto IL_039a;
	}
}
