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
		if (this.m_healOnOwnerFromEnemyShot <= 0 && this.m_energyGainOnOwnerFromEnemyShot <= 0)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierResponseOnShot.HasResponses()).MethodHandle;
			}
			if (!this.m_effectOnOwnerFromEnemyShot.m_applyEffect)
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
				if (this.m_damageOnEnemyOnShot <= 0 && this.m_energyLossOnEnemyOnShot <= 0)
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
					return this.m_effectOnEnemyOnShot.m_applyEffect;
				}
			}
		}
		return true;
	}

	public BarrierResponseOnShot GetShallowCopy()
	{
		return (BarrierResponseOnShot)base.MemberwiseClone();
	}

	public void AddTooltipTokens(List<TooltipTokenEntry> tokens, string name, bool addCompare, BarrierResponseOnShot other)
	{
		bool flag = addCompare && other != null;
		string name2 = name + "_OnShot_HealOnOwner";
		string empty = string.Empty;
		int healOnOwnerFromEnemyShot = this.m_healOnOwnerFromEnemyShot;
		bool addDiff = flag;
		int otherVal;
		if (flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierResponseOnShot.AddTooltipTokens(List<TooltipTokenEntry>, string, bool, BarrierResponseOnShot)).MethodHandle;
			}
			otherVal = other.m_healOnOwnerFromEnemyShot;
		}
		else
		{
			otherVal = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name2, empty, healOnOwnerFromEnemyShot, addDiff, otherVal);
		string name3 = name + "_OnShot_EnergyOnOwner";
		string empty2 = string.Empty;
		int energyGainOnOwnerFromEnemyShot = this.m_energyGainOnOwnerFromEnemyShot;
		bool addDiff2 = flag;
		int otherVal2;
		if (flag)
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
			otherVal2 = other.m_energyGainOnOwnerFromEnemyShot;
		}
		else
		{
			otherVal2 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name3, empty2, energyGainOnOwnerFromEnemyShot, addDiff2, otherVal2);
		StandardEffectInfo effectOnOwnerFromEnemyShot = this.m_effectOnOwnerFromEnemyShot;
		string tokenName = name + "_OnShot_EffectOnOwner";
		StandardEffectInfo baseVal;
		if (flag)
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
			baseVal = other.m_effectOnOwnerFromEnemyShot;
		}
		else
		{
			baseVal = null;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectOnOwnerFromEnemyShot, tokenName, baseVal, flag);
		string name4 = name + "_OnShot_DamageOnEnemy";
		string empty3 = string.Empty;
		int damageOnEnemyOnShot = this.m_damageOnEnemyOnShot;
		bool addDiff3 = flag;
		int otherVal3;
		if (flag)
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
			otherVal3 = other.m_damageOnEnemyOnShot;
		}
		else
		{
			otherVal3 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name4, empty3, damageOnEnemyOnShot, addDiff3, otherVal3);
		string name5 = name + "_OnShot_EnergyLossOnEnemy";
		string empty4 = string.Empty;
		int energyLossOnEnemyOnShot = this.m_energyLossOnEnemyOnShot;
		bool addDiff4 = flag;
		int otherVal4;
		if (flag)
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
			otherVal4 = other.m_energyLossOnEnemyOnShot;
		}
		else
		{
			otherVal4 = 0;
		}
		AbilityMod.AddToken_IntDiff(tokens, name5, empty4, energyLossOnEnemyOnShot, addDiff4, otherVal4);
		StandardEffectInfo effectOnEnemyOnShot = this.m_effectOnEnemyOnShot;
		string tokenName2 = name + "_OnShot_EffectOnEnemy";
		StandardEffectInfo baseVal2;
		if (flag)
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
			baseVal2 = other.m_effectOnEnemyOnShot;
		}
		else
		{
			baseVal2 = null;
		}
		AbilityMod.AddToken_EffectInfo(tokens, effectOnEnemyOnShot, tokenName2, baseVal2, flag);
	}

	public string GetInEditorDescription(string header = "- Response -", string indent = "", bool showDiff = false, BarrierResponseOnShot other = null)
	{
		bool flag;
		if (showDiff)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(BarrierResponseOnShot.GetInEditorDescription(string, string, bool, BarrierResponseOnShot)).MethodHandle;
			}
			flag = (other != null);
		}
		else
		{
			flag = false;
		}
		bool flag2 = flag;
		string text = "\t        \t | in base  =";
		string text2 = "\n";
		string text3 = text2 + InEditorDescHelper.BoldedStirng(header) + text2;
		text3 += InEditorDescHelper.AssembleFieldWithDiff("[ Heal on Owner on Enemy shot ] = ", indent, text, (float)this.m_healOnOwnerFromEnemyShot, flag2, (float)((!flag2) ? 0 : other.m_healOnOwnerFromEnemyShot), null);
		string str = text3;
		string header2 = "[ EnergyGain on Owner on Enemy shot ] = ";
		string otherSep = text;
		float myVal = (float)this.m_energyGainOnOwnerFromEnemyShot;
		bool showOther = flag2;
		float num;
		if (flag2)
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
			num = (float)other.m_energyGainOnOwnerFromEnemyShot;
		}
		else
		{
			num = (float)0;
		}
		text3 = str + InEditorDescHelper.AssembleFieldWithDiff(header2, indent, otherSep, myVal, showOther, num, null);
		string str2 = text3;
		StandardEffectInfo effectOnOwnerFromEnemyShot = this.m_effectOnOwnerFromEnemyShot;
		string prefix = "{ Effect on Owner on Enemy shot }";
		bool useBaseVal = flag2;
		StandardEffectInfo baseVal;
		if (flag2)
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
			baseVal = other.m_effectOnOwnerFromEnemyShot;
		}
		else
		{
			baseVal = null;
		}
		text3 = str2 + AbilityModHelper.GetModEffectInfoDesc(effectOnOwnerFromEnemyShot, prefix, indent, useBaseVal, baseVal);
		text3 += InEditorDescHelper.AssembleFieldWithDiff("[ Damage on Enemy on shot ] = ", indent, text, (float)this.m_damageOnEnemyOnShot, flag2, (float)((!flag2) ? 0 : other.m_damageOnEnemyOnShot), null);
		text3 += InEditorDescHelper.AssembleFieldWithDiff("[ EnergyLoss on Enemy on shot ] = ", indent, text, (float)this.m_energyLossOnEnemyOnShot, flag2, (float)((!flag2) ? 0 : other.m_energyLossOnEnemyOnShot), null);
		text3 += AbilityModHelper.GetModEffectInfoDesc(this.m_effectOnEnemyOnShot, "{ Effect on Enemy on shot }", indent, flag2, (!flag2) ? null : other.m_effectOnEnemyOnShot);
		string str3 = text3;
		string header3 = "[ On Shot Sequence Prefab ]";
		string otherSep2 = text;
		GameObject onShotSequencePrefab = this.m_onShotSequencePrefab;
		bool showOther2 = flag2;
		GameObject otherVal;
		if (flag2)
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
			otherVal = other.m_onShotSequencePrefab;
		}
		else
		{
			otherVal = null;
		}
		text3 = str3 + InEditorDescHelper.AssembleFieldWithDiff(header3, indent, otherSep2, onShotSequencePrefab, showOther2, otherVal);
		string str4 = text3;
		string header4 = "[ Use Shooter Pos As Target Pos For Sequence ] = ";
		string otherSep3 = text;
		bool useShooterPosAsReactionSequenceTargetPos = this.m_useShooterPosAsReactionSequenceTargetPos;
		bool showOther3 = flag2;
		bool otherVal2;
		if (flag2)
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
			otherVal2 = other.m_useShooterPosAsReactionSequenceTargetPos;
		}
		else
		{
			otherVal2 = false;
		}
		return str4 + InEditorDescHelper.AssembleFieldWithDiff(header4, indent, otherSep3, useShooterPosAsReactionSequenceTargetPos, showOther3, otherVal2, null);
	}
}
