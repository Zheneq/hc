using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BattleMonkBasicAttack : AbilityMod
{
	[Header("-- Cone Size Mod")]
	public AbilityModPropertyFloat m_coneAngleMod;

	public AbilityModPropertyFloat m_coneLengthMod;

	[Header("-- Damage and Effect on Enemy")]
	public AbilityModPropertyInt m_coneDamageMod;

	public StandardEffectInfo m_enemyHitEffect;

	[Header("-- Heal on Self per Target Hit")]
	public AbilityModPropertyInt m_healPerTargetHitMod;

	public AbilityModPropertyInt m_extraDamagePerTarget;

	public override Type GetTargetAbilityType()
	{
		return typeof(BattleMonkBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BattleMonkBasicAttack battleMonkBasicAttack = targetAbility as BattleMonkBasicAttack;
		if (!(battleMonkBasicAttack != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_coneAngleMod, "ConeWidthAngle", string.Empty, battleMonkBasicAttack.m_coneWidthAngle);
			AbilityMod.AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, battleMonkBasicAttack.m_coneLength);
			AbilityMod.AddToken(tokens, m_coneDamageMod, "DamageAmount", string.Empty, battleMonkBasicAttack.m_damageAmount);
			AbilityMod.AddToken(tokens, m_healPerTargetHitMod, "HealAmountPerTargetHit", string.Empty, battleMonkBasicAttack.m_healAmountPerTargetHit);
			AbilityMod.AddToken(tokens, m_extraDamagePerTarget, "ExtraDamagePerTarget", string.Empty, 0);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		string empty = string.Empty;
		BattleMonkBasicAttack battleMonkBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as BattleMonkBasicAttack;
		bool flag = battleMonkBasicAttack != null;
		string str = empty;
		AbilityModPropertyFloat coneAngleMod = m_coneAngleMod;
		float baseVal;
		if (flag)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = battleMonkBasicAttack.m_coneWidthAngle;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(coneAngleMod, "[Cone Angle]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat coneLengthMod = m_coneLengthMod;
		float baseVal2;
		if (flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = battleMonkBasicAttack.m_coneLength;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(coneLengthMod, "[Cone Length]", flag, baseVal2);
		empty += AbilityModHelper.GetModPropertyDesc(m_coneDamageMod, "[Cone Damage]", flag, flag ? battleMonkBasicAttack.m_damageAmount : 0);
		string str3 = empty;
		AbilityModPropertyInt healPerTargetHitMod = m_healPerTargetHitMod;
		int baseVal3;
		if (flag)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = battleMonkBasicAttack.m_healAmountPerTargetHit;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(healPerTargetHitMod, "[Heal Per Target Hit]", flag, baseVal3);
		empty += AbilityModHelper.GetModPropertyDesc(m_extraDamagePerTarget, "[Extra Damage Per Target Hit]", flag);
		return empty + AbilityModHelper.GetModEffectInfoDesc(m_enemyHitEffect, "{ Enemy Hit Effect }", string.Empty, flag);
	}
}
