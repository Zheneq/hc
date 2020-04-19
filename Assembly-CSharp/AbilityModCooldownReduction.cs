using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AbilityModCooldownReduction
{
	public AbilityData.ActionType m_onAbility = AbilityData.ActionType.INVALID_ACTION;

	public List<AbilityData.ActionType> m_additionalAbilities = new List<AbilityData.ActionType>();

	public AbilityModCooldownReduction.ModAmountType m_modAmountType;

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
		if (this.IsValidActionType(this.m_onAbility))
		{
			return true;
		}
		foreach (AbilityData.ActionType actionType in this.m_additionalAbilities)
		{
			if (this.IsValidActionType(actionType))
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
				if (!true)
				{
					RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModCooldownReduction.HasCooldownReduction()).MethodHandle;
				}
				return true;
			}
		}
		return false;
	}

	private bool IsValidActionType(AbilityData.ActionType actionType)
	{
		bool result;
		if (actionType != AbilityData.ActionType.INVALID_ACTION)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModCooldownReduction.IsValidActionType(AbilityData.ActionType)).MethodHandle;
			}
			if (this.m_baseValue == 0f)
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
				if (this.m_finalAdd == 0f)
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
					if (this.m_stockBaseValue == 0)
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
						if (this.m_stockFinalAdd == 0 && this.m_refreshProgressBaseValue == 0)
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
							result = (this.m_refreshProgressFinalAdd != 0);
							goto IL_83;
						}
					}
				}
			}
			result = true;
			IL_83:;
		}
		else
		{
			result = false;
		}
		return result;
	}

	public int CalcFinalCooldownReduction(bool selfHit, int numAlliesHit, int numEnemiesHit)
	{
		int b = 0;
		if (this.m_modAmountType != AbilityModCooldownReduction.ModAmountType.FlatOnCast)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModCooldownReduction.CalcFinalCooldownReduction(bool, int, int)).MethodHandle;
			}
			if (this.m_modAmountType == AbilityModCooldownReduction.ModAmountType.FlatOnAnyNonSelfHit)
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
				if (numAlliesHit > 0)
				{
					goto IL_F6;
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
				if (numEnemiesHit > 0)
				{
					goto IL_F6;
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
			if (this.m_modAmountType == AbilityModCooldownReduction.ModAmountType.FlatOnAnyAllyHit)
			{
				if (numAlliesHit > 0)
				{
					goto IL_F6;
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
			if (this.m_modAmountType == AbilityModCooldownReduction.ModAmountType.FlatOnAnyEnemyHit)
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
				if (numEnemiesHit > 0)
				{
					goto IL_F6;
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
			if (this.m_modAmountType == AbilityModCooldownReduction.ModAmountType.FlatOnNoEnemyHit)
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
				if (numEnemiesHit == 0)
				{
					goto IL_F6;
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
			if (this.m_modAmountType == AbilityModCooldownReduction.ModAmountType.FlatOnNoAllyHit)
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
				if (numAlliesHit == 0)
				{
					goto IL_F6;
				}
			}
			if (this.m_modAmountType == AbilityModCooldownReduction.ModAmountType.FlatOnNoNonCasterHit)
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
				if (numEnemiesHit == 0)
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
					if (numAlliesHit == 0)
					{
						for (;;)
						{
							switch (1)
							{
							case 0:
								continue;
							}
							goto IL_F6;
						}
					}
				}
			}
			if (this.m_modAmountType == AbilityModCooldownReduction.ModAmountType.MultPerAllyHit)
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
				b = Mathf.RoundToInt(this.m_baseValue * (float)numAlliesHit + this.m_finalAdd);
				goto IL_165;
			}
			if (this.m_modAmountType == AbilityModCooldownReduction.ModAmountType.MultPerEnemyHit)
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
				b = Mathf.RoundToInt(this.m_baseValue * (float)numEnemiesHit + this.m_finalAdd);
				goto IL_165;
			}
			goto IL_165;
		}
		IL_F6:
		b = Mathf.RoundToInt(this.m_baseValue + this.m_finalAdd);
		IL_165:
		int num = Mathf.Max(this.m_minReduction, b);
		if (this.m_maxReduction > 0)
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
			num = Mathf.Min(this.m_maxReduction, num);
		}
		return num;
	}

	public unsafe int CalcFinalStockRefresh(bool selfHit, int numAlliesHit, int numEnemiesHit, out int refreshProgress)
	{
		int result = 0;
		refreshProgress = 0;
		if (this.m_modAmountType != AbilityModCooldownReduction.ModAmountType.FlatOnCast)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModCooldownReduction.CalcFinalStockRefresh(bool, int, int, int*)).MethodHandle;
			}
			if (this.m_modAmountType == AbilityModCooldownReduction.ModAmountType.FlatOnAnyNonSelfHit)
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
				if (numAlliesHit > 0 || numEnemiesHit > 0)
				{
					goto IL_8B;
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
			if (this.m_modAmountType == AbilityModCooldownReduction.ModAmountType.FlatOnAnyAllyHit)
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
				if (numAlliesHit > 0)
				{
					goto IL_8B;
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
			if (this.m_modAmountType == AbilityModCooldownReduction.ModAmountType.FlatOnAnyEnemyHit)
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
				if (numEnemiesHit > 0)
				{
					for (;;)
					{
						switch (6)
						{
						case 0:
							continue;
						}
						goto IL_8B;
					}
				}
			}
			if (this.m_modAmountType == AbilityModCooldownReduction.ModAmountType.MultPerAllyHit)
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
				result = Mathf.RoundToInt((float)(this.m_stockBaseValue * numAlliesHit + this.m_stockFinalAdd));
				refreshProgress = Mathf.RoundToInt((float)(this.m_refreshProgressBaseValue * numAlliesHit + this.m_refreshProgressFinalAdd));
				return result;
			}
			if (this.m_modAmountType == AbilityModCooldownReduction.ModAmountType.MultPerEnemyHit)
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
				result = Mathf.RoundToInt((float)(this.m_stockBaseValue * numEnemiesHit + this.m_stockFinalAdd));
				refreshProgress = Mathf.RoundToInt((float)(this.m_refreshProgressBaseValue * numEnemiesHit + this.m_refreshProgressFinalAdd));
				return result;
			}
			return result;
		}
		IL_8B:
		result = Mathf.RoundToInt((float)(this.m_stockBaseValue + this.m_stockFinalAdd));
		refreshProgress = Mathf.RoundToInt((float)(this.m_refreshProgressBaseValue + this.m_refreshProgressFinalAdd));
		return result;
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> entries, string name)
	{
		if (this.HasCooldownReduction())
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModCooldownReduction.AddTooltipTokens(List<TooltipTokenEntry>, string)).MethodHandle;
			}
			int val = this.CalcFinalCooldownReduction(true, 1, 1);
			if (this.m_onAbility != AbilityData.ActionType.INVALID_ACTION)
			{
				entries.Add(new TooltipTokenInt(name + "_CDR_" + this.m_onAbility.ToString(), "cooldown reduction", val));
			}
			using (List<AbilityData.ActionType>.Enumerator enumerator = this.m_additionalAbilities.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					AbilityData.ActionType actionType = enumerator.Current;
					if (actionType != AbilityData.ActionType.INVALID_ACTION)
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
						entries.Add(new TooltipTokenInt(name + "CRD_" + actionType.ToString(), "cooldown reduction", val));
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
		}
	}

	public string GetDescription(AbilityData abilityData)
	{
		string[] array = new string[5];
		array[0] = InEditorDescHelper.ColoredString(this.m_baseValue.ToString(), "cyan", false);
		array[1] = " ";
		array[2] = InEditorDescHelper.ColoredString(this.m_modAmountType.ToString(), "lime", false);
		array[3] = " ";
		int num = 4;
		string str;
		if (this.m_finalAdd >= 0f)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityModCooldownReduction.GetDescription(AbilityData)).MethodHandle;
			}
			str = " +";
		}
		else
		{
			str = string.Empty;
		}
		array[num] = InEditorDescHelper.ColoredString(str + this.m_finalAdd.ToString(), "cyan", false);
		string text = string.Concat(array);
		string text2 = "\t<color=white>" + AbilityModHelper.GetAbilityNameFromActionType(this.m_onAbility, abilityData) + "</color>\n";
		using (List<AbilityData.ActionType>.Enumerator enumerator = this.m_additionalAbilities.GetEnumerator())
		{
			while (enumerator.MoveNext())
			{
				AbilityData.ActionType actionType = enumerator.Current;
				text2 = text2 + "\t<color=white>" + AbilityModHelper.GetAbilityNameFromActionType(actionType, abilityData) + "</color>\n";
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
		text = text + "\nCooldown Reduction On Abilities:\n" + text2;
		text2 = string.Empty;
		using (List<AbilityData.ActionType>.Enumerator enumerator2 = this.m_stockAbilities.GetEnumerator())
		{
			while (enumerator2.MoveNext())
			{
				AbilityData.ActionType actionType2 = enumerator2.Current;
				text2 = text2 + "\t<color=white>" + AbilityModHelper.GetAbilityNameFromActionType(actionType2, abilityData) + "</color>\n";
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
		if (this.m_stockBaseValue == 0)
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
			if (this.m_stockFinalAdd == 0)
			{
				goto IL_28B;
			}
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
		}
		string text3 = text;
		string[] array2 = new string[7];
		array2[0] = text3;
		array2[1] = "\n";
		array2[2] = InEditorDescHelper.ColoredString(this.m_stockBaseValue.ToString(), "cyan", false);
		array2[3] = " ";
		array2[4] = InEditorDescHelper.ColoredString(this.m_modAmountType.ToString(), "lime", false);
		array2[5] = " ";
		int num2 = 6;
		string str2;
		if (this.m_stockFinalAdd >= 0)
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
			str2 = " +";
		}
		else
		{
			str2 = string.Empty;
		}
		array2[num2] = InEditorDescHelper.ColoredString(str2 + this.m_stockFinalAdd.ToString(), "cyan", false);
		text = string.Concat(array2);
		text = text + "\nStock Add On Abilities:\n" + text2;
		IL_28B:
		if (this.m_resetRefreshProgress)
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
			text = text + "\nReset Stock Refresh Progress On Abilities:\n" + text2;
		}
		else
		{
			if (this.m_refreshProgressBaseValue == 0)
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
				if (this.m_refreshProgressFinalAdd == 0)
				{
					return text;
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
			text3 = text;
			string[] array3 = new string[7];
			array3[0] = text3;
			array3[1] = "\n";
			array3[2] = InEditorDescHelper.ColoredString(this.m_refreshProgressBaseValue.ToString(), "cyan", false);
			array3[3] = " ";
			array3[4] = InEditorDescHelper.ColoredString(this.m_modAmountType.ToString(), "lime", false);
			array3[5] = " ";
			int num3 = 6;
			string str3;
			if (this.m_refreshProgressFinalAdd >= 0)
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
				str3 = " +";
			}
			else
			{
				str3 = string.Empty;
			}
			array3[num3] = InEditorDescHelper.ColoredString(str3 + this.m_refreshProgressFinalAdd.ToString(), "cyan", false);
			text = string.Concat(array3);
			text = text + "\nStock Refresh Progress On Abilities:\n" + text2;
		}
		return text;
	}

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
}
