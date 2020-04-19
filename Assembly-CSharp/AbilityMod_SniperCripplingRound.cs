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
			AbilityMod.AddToken(tokens, this.m_laserDamageMod, "Damage_Laser", string.Empty, sniperCripplingRound.m_laserDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_explosionDamageMod, "Damage_Explosion", string.Empty, sniperCripplingRound.m_explosionDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_enemyHitEffectDurationMod, "EffectDuration", string.Empty, sniperCripplingRound.m_effectOnLaserHitTargets.m_effectData.m_duration, true, false);
			AbilityMod.AddToken_EffectInfo(tokens, this.m_additionalEnemyHitEffect, "AdditionalEffect", null, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargets", string.Empty, 1, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SniperCripplingRound sniperCripplingRound = base.GetTargetAbilityOnAbilityData(abilityData) as SniperCripplingRound;
		bool flag = sniperCripplingRound != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt laserDamageMod = this.m_laserDamageMod;
		string prefix = "[Laser Damage]";
		bool showBaseVal = flag;
		int baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SniperCripplingRound.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = sniperCripplingRound.m_laserDamageAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(laserDamageMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt explosionDamageMod = this.m_explosionDamageMod;
		string prefix2 = "[Explosion Damage]";
		bool showBaseVal2 = flag;
		int baseVal2;
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
			baseVal2 = sniperCripplingRound.m_explosionDamageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(explosionDamageMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt enemyHitEffectDurationMod = this.m_enemyHitEffectDurationMod;
		string prefix3 = "[Effect Duration]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
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
			baseVal3 = sniperCripplingRound.m_effectOnLaserHitTargets.m_effectData.m_duration;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(enemyHitEffectDurationMod, prefix3, showBaseVal3, baseVal3);
		text += AbilityModHelper.GetModEffectInfoDesc(this.m_additionalEnemyHitEffect, "{ Additional Enemy Hit Effect }", string.Empty, flag, null);
		return text + AbilityModHelper.GetModPropertyDesc(this.m_maxTargetsMod, "[Max Laser Targets]", flag, 1);
	}
}
