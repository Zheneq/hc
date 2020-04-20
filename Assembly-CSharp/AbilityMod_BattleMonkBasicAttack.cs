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
		if (battleMonkBasicAttack != null)
		{
			AbilityMod.AddToken(tokens, this.m_coneAngleMod, "ConeWidthAngle", string.Empty, battleMonkBasicAttack.m_coneWidthAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneLengthMod, "ConeLength", string.Empty, battleMonkBasicAttack.m_coneLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneDamageMod, "DamageAmount", string.Empty, battleMonkBasicAttack.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_healPerTargetHitMod, "HealAmountPerTargetHit", string.Empty, battleMonkBasicAttack.m_healAmountPerTargetHit, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamagePerTarget, "ExtraDamagePerTarget", string.Empty, 0, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		string text = string.Empty;
		BattleMonkBasicAttack battleMonkBasicAttack = base.GetTargetAbilityOnAbilityData(abilityData) as BattleMonkBasicAttack;
		bool flag = battleMonkBasicAttack != null;
		string str = text;
		AbilityModPropertyFloat coneAngleMod = this.m_coneAngleMod;
		string prefix = "[Cone Angle]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = battleMonkBasicAttack.m_coneWidthAngle;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(coneAngleMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat coneLengthMod = this.m_coneLengthMod;
		string prefix2 = "[Cone Length]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = battleMonkBasicAttack.m_coneLength;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(coneLengthMod, prefix2, showBaseVal2, baseVal2);
		text += AbilityModHelper.GetModPropertyDesc(this.m_coneDamageMod, "[Cone Damage]", flag, (!flag) ? 0 : battleMonkBasicAttack.m_damageAmount);
		string str3 = text;
		AbilityModPropertyInt healPerTargetHitMod = this.m_healPerTargetHitMod;
		string prefix3 = "[Heal Per Target Hit]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = battleMonkBasicAttack.m_healAmountPerTargetHit;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(healPerTargetHitMod, prefix3, showBaseVal3, baseVal3);
		text += AbilityModHelper.GetModPropertyDesc(this.m_extraDamagePerTarget, "[Extra Damage Per Target Hit]", flag, 0);
		return text + AbilityModHelper.GetModEffectInfoDesc(this.m_enemyHitEffect, "{ Enemy Hit Effect }", string.Empty, flag, null);
	}
}
