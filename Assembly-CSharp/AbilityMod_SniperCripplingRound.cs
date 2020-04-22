using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SniperCripplingRound : AbilityMod
{
	[Header("-- Damage Mod")]
	public AbilityModPropertyInt m_laserDamageMod;

	public AbilityModPropertyInt m_explosionDamageMod;

	[Header("-- Effect Duration Mod and Additional Effect")]
	public AbilityModPropertyInt m_enemyHitEffectDurationMod;

	public StandardEffectInfo m_additionalEnemyHitEffect;

	[Header("-- Max targets hit by laser. Explosion only on first.")]
	public AbilityModPropertyInt m_maxTargetsMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SniperCripplingRound);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SniperCripplingRound sniperCripplingRound = targetAbility as SniperCripplingRound;
		if (sniperCripplingRound != null)
		{
			AbilityMod.AddToken(tokens, m_laserDamageMod, "Damage_Laser", string.Empty, sniperCripplingRound.m_laserDamageAmount);
			AbilityMod.AddToken(tokens, m_explosionDamageMod, "Damage_Explosion", string.Empty, sniperCripplingRound.m_explosionDamageAmount);
			AbilityMod.AddToken(tokens, m_enemyHitEffectDurationMod, "EffectDuration", string.Empty, sniperCripplingRound.m_effectOnLaserHitTargets.m_effectData.m_duration);
			AbilityMod.AddToken_EffectInfo(tokens, m_additionalEnemyHitEffect, "AdditionalEffect", null, false);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, 1);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SniperCripplingRound sniperCripplingRound = GetTargetAbilityOnAbilityData(abilityData) as SniperCripplingRound;
		bool flag = sniperCripplingRound != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt laserDamageMod = m_laserDamageMod;
		int baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = sniperCripplingRound.m_laserDamageAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(laserDamageMod, "[Laser Damage]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt explosionDamageMod = m_explosionDamageMod;
		int baseVal2;
		if (flag)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = sniperCripplingRound.m_explosionDamageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(explosionDamageMod, "[Explosion Damage]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt enemyHitEffectDurationMod = m_enemyHitEffectDurationMod;
		int baseVal3;
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = sniperCripplingRound.m_effectOnLaserHitTargets.m_effectData.m_duration;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(enemyHitEffectDurationMod, "[Effect Duration]", flag, baseVal3);
		empty += AbilityModHelper.GetModEffectInfoDesc(m_additionalEnemyHitEffect, "{ Additional Enemy Hit Effect }", string.Empty, flag);
		return empty + AbilityModHelper.GetModPropertyDesc(m_maxTargetsMod, "[Max Laser Targets]", flag, 1);
	}
}
