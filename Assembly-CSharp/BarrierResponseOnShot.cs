using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BarrierResponseOnShot
{
	[Header("-- Sequences for On Shot Visual --")]
	public GameObject m_onShotSequencePrefab;

	public bool m_useShooterPosAsReactionSequenceTargetPos;

	[Header("-- On Owner: For Enemy Shot --")]
	public int m_healOnOwnerFromEnemyShot;

	public int m_energyGainOnOwnerFromEnemyShot;

	public StandardEffectInfo m_effectOnOwnerFromEnemyShot;

	[Header("-- On Shooter: For Enemy Shot --")]
	public int m_damageOnEnemyOnShot;

	public int m_energyLossOnEnemyOnShot;

	public StandardEffectInfo m_effectOnEnemyOnShot;

	public bool HasResponses()
	{
		int result;
		if (m_healOnOwnerFromEnemyShot <= 0 && m_energyGainOnOwnerFromEnemyShot <= 0)
		{
			if (!m_effectOnOwnerFromEnemyShot.m_applyEffect)
			{
				if (m_damageOnEnemyOnShot <= 0 && m_energyLossOnEnemyOnShot <= 0)
				{
					result = (m_effectOnEnemyOnShot.m_applyEffect ? 1 : 0);
					goto IL_0066;
				}
			}
		}
		result = 1;
		goto IL_0066;
		IL_0066:
		return (byte)result != 0;
	}

	public BarrierResponseOnShot GetShallowCopy()
	{
		return (BarrierResponseOnShot)MemberwiseClone();
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens, string name, bool addCompare, BarrierResponseOnShot other)
	{
		bool flag = addCompare && other != null;
		string name2 = name + "_OnShot_HealOnOwner";
		string empty = string.Empty;
		int healOnOwnerFromEnemyShot = m_healOnOwnerFromEnemyShot;
		int otherVal;
		if (flag)
		{
			otherVal = other.m_healOnOwnerFromEnemyShot;
		}
		else
		{
			otherVal = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name2, empty, healOnOwnerFromEnemyShot, flag, otherVal);
		string name3 = name + "_OnShot_EnergyOnOwner";
		string empty2 = string.Empty;
		int energyGainOnOwnerFromEnemyShot = m_energyGainOnOwnerFromEnemyShot;
		int otherVal2;
		if (flag)
		{
			otherVal2 = other.m_energyGainOnOwnerFromEnemyShot;
		}
		else
		{
			otherVal2 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name3, empty2, energyGainOnOwnerFromEnemyShot, flag, otherVal2);
		StandardEffectInfo effectOnOwnerFromEnemyShot = m_effectOnOwnerFromEnemyShot;
		string tokenName = name + "_OnShot_EffectOnOwner";
		object baseVal;
		if (flag)
		{
			baseVal = other.m_effectOnOwnerFromEnemyShot;
		}
		else
		{
			baseVal = null;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectOnOwnerFromEnemyShot, tokenName, (StandardEffectInfo)baseVal, flag);
		string name4 = name + "_OnShot_DamageOnEnemy";
		string empty3 = string.Empty;
		int damageOnEnemyOnShot = m_damageOnEnemyOnShot;
		int otherVal3;
		if (flag)
		{
			otherVal3 = other.m_damageOnEnemyOnShot;
		}
		else
		{
			otherVal3 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name4, empty3, damageOnEnemyOnShot, flag, otherVal3);
		string name5 = name + "_OnShot_EnergyLossOnEnemy";
		string empty4 = string.Empty;
		int energyLossOnEnemyOnShot = m_energyLossOnEnemyOnShot;
		int otherVal4;
		if (flag)
		{
			otherVal4 = other.m_energyLossOnEnemyOnShot;
		}
		else
		{
			otherVal4 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name5, empty4, energyLossOnEnemyOnShot, flag, otherVal4);
		StandardEffectInfo effectOnEnemyOnShot = m_effectOnEnemyOnShot;
		string tokenName2 = name + "_OnShot_EffectOnEnemy";
		object baseVal2;
		if (flag)
		{
			baseVal2 = other.m_effectOnEnemyOnShot;
		}
		else
		{
			baseVal2 = null;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectOnEnemyOnShot, tokenName2, (StandardEffectInfo)baseVal2, flag);
	}

	public string GetInEditorDescription(string header = "- Response -", string indent = "", bool showDiff = false, BarrierResponseOnShot other = null)
	{
		int num;
		if (showDiff)
		{
			num = ((other != null) ? 1 : 0);
		}
		else
		{
			num = 0;
		}
		bool flag = (byte)num != 0;
		string otherSep = "\t        \t | in base  =";
		string text = "\n";
		string str = text + InEditorDescHelper.BoldedStirng(header) + text;
		str += InEditorDescHelper.AssembleFieldWithDiff("[ Heal on Owner on Enemy shot ] = ", indent, otherSep, m_healOnOwnerFromEnemyShot, flag, flag ? other.m_healOnOwnerFromEnemyShot : 0);
		string str2 = str;
		float myVal = m_energyGainOnOwnerFromEnemyShot;
		int num2;
		if (flag)
		{
			num2 = other.m_energyGainOnOwnerFromEnemyShot;
		}
		else
		{
			num2 = 0;
		}
		str = str2 + InEditorDescHelper.AssembleFieldWithDiff("[ EnergyGain on Owner on Enemy shot ] = ", indent, otherSep, myVal, flag, num2);
		string str3 = str;
		StandardEffectInfo effectOnOwnerFromEnemyShot = m_effectOnOwnerFromEnemyShot;
		object baseVal;
		if (flag)
		{
			baseVal = other.m_effectOnOwnerFromEnemyShot;
		}
		else
		{
			baseVal = null;
		}
		str = str3 + AbilityModHelper.GetModEffectInfoDesc(effectOnOwnerFromEnemyShot, "{ Effect on Owner on Enemy shot }", indent, flag, (StandardEffectInfo)baseVal);
		str += InEditorDescHelper.AssembleFieldWithDiff("[ Damage on Enemy on shot ] = ", indent, otherSep, m_damageOnEnemyOnShot, flag, flag ? other.m_damageOnEnemyOnShot : 0);
		str += InEditorDescHelper.AssembleFieldWithDiff("[ EnergyLoss on Enemy on shot ] = ", indent, otherSep, m_energyLossOnEnemyOnShot, flag, flag ? other.m_energyLossOnEnemyOnShot : 0);
		str += AbilityModHelper.GetModEffectInfoDesc(m_effectOnEnemyOnShot, "{ Effect on Enemy on shot }", indent, flag, (!flag) ? null : other.m_effectOnEnemyOnShot);
		string str4 = str;
		GameObject onShotSequencePrefab = m_onShotSequencePrefab;
		object otherVal;
		if (flag)
		{
			otherVal = other.m_onShotSequencePrefab;
		}
		else
		{
			otherVal = null;
		}
		str = str4 + InEditorDescHelper.AssembleFieldWithDiff("[ On Shot Sequence Prefab ]", indent, otherSep, onShotSequencePrefab, flag, (GameObject)otherVal);
		string str5 = str;
		bool useShooterPosAsReactionSequenceTargetPos = m_useShooterPosAsReactionSequenceTargetPos;
		int otherVal2;
		if (flag)
		{
			otherVal2 = (other.m_useShooterPosAsReactionSequenceTargetPos ? 1 : 0);
		}
		else
		{
			otherVal2 = 0;
		}
		return str5 + InEditorDescHelper.AssembleFieldWithDiff("[ Use Shooter Pos As Target Pos For Sequence ] = ", indent, otherSep, useShooterPosAsReactionSequenceTargetPos, flag, (byte)otherVal2 != 0);
	}
}
