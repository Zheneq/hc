// ROGUES
// SERVER
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
			AddToken(tokens, m_coneAngleMod, "ConeWidthAngle", string.Empty, battleMonkBasicAttack.m_coneWidthAngle);
			AddToken(tokens, m_coneLengthMod, "ConeLength", string.Empty, battleMonkBasicAttack.m_coneLength);
			AddToken(tokens, m_coneDamageMod, "DamageAmount", string.Empty, battleMonkBasicAttack.m_damageAmount);
			AddToken(tokens, m_healPerTargetHitMod, "HealAmountPerTargetHit", string.Empty, battleMonkBasicAttack.m_healAmountPerTargetHit);
			AddToken(tokens, m_extraDamagePerTarget, "ExtraDamagePerTarget", string.Empty, 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)  // , Ability targetAbility in rogues
	{
		// reactor
		BattleMonkBasicAttack battleMonkBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as BattleMonkBasicAttack;
		// rogues
		//BattleMonkBasicAttack battleMonkBasicAttack = targetAbility as BattleMonkBasicAttack;
		
		bool isAbilityPresent = battleMonkBasicAttack != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_coneAngleMod, "[Cone Angle]", isAbilityPresent, isAbilityPresent ? battleMonkBasicAttack.m_coneWidthAngle : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_coneLengthMod, "[Cone Length]", isAbilityPresent, isAbilityPresent ? battleMonkBasicAttack.m_coneLength : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_coneDamageMod, "[Cone Damage]", isAbilityPresent, isAbilityPresent ? battleMonkBasicAttack.m_damageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_healPerTargetHitMod, "[Heal Per Target Hit]", isAbilityPresent, isAbilityPresent ? battleMonkBasicAttack.m_healAmountPerTargetHit : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_extraDamagePerTarget, "[Extra Damage Per Target Hit]", isAbilityPresent);
		return desc + AbilityModHelper.GetModEffectInfoDesc(m_enemyHitEffect, "{ Enemy Hit Effect }", string.Empty, isAbilityPresent);
	}
}
