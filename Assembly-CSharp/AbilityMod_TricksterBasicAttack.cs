using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_TricksterBasicAttack : AbilityMod
{
	[Header("-- Laser Targeting")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;

	[Header("-- Damage and Effect")]
	public AbilityModPropertyInt m_laserDamageAmountMod;

	public AbilityModPropertyInt m_laserSubsequentDamageAmountMod;

	public AbilityModPropertyInt m_extraDamageForSingleHitMod;

	public AbilityModPropertyEffectInfo m_enemySingleHitHitEffectMod;

	public AbilityModPropertyEffectInfo m_enemyMultiHitEffectMod;

	[Header("-- Effect on Self for Multi Hit")]
	public AbilityModPropertyEffectInfo m_selfEffectForMultiHitMod;

	[Header("-- Energy Gain --")]
	public AbilityModPropertyInt m_energyGainPerLaserHitMod;

	[Header("-- For spawning spoils")]
	public AbilityModPropertySpoilsSpawnData m_spoilSpawnInfoMod;

	public AbilityModPropertyBool m_onlySpawnSpoilOnMultiHitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(TricksterBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		TricksterBasicAttack tricksterBasicAttack = targetAbility as TricksterBasicAttack;
		if (tricksterBasicAttack != null)
		{
			AbilityMod.AddToken_LaserInfo(tokens, this.m_laserInfoMod, "LaserInfo", tricksterBasicAttack.m_laserInfo, true);
			AbilityMod.AddToken(tokens, this.m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, tricksterBasicAttack.m_laserDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_laserSubsequentDamageAmountMod, "LaserSubsequentDamageAmount", string.Empty, tricksterBasicAttack.m_laserSubsequentDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageForSingleHitMod, "ExtraDamageForSingleHit", string.Empty, tricksterBasicAttack.m_extraDamageForSingleHit, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemySingleHitHitEffectMod, "EnemySingleHitHitEffect", tricksterBasicAttack.m_enemySingleHitHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyMultiHitEffectMod, "EnemyMultiHitEffect", tricksterBasicAttack.m_enemyMultiHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_selfEffectForMultiHitMod, "SelfEffectForMultiHit", tricksterBasicAttack.m_selfEffectForMultiHit, true);
			AbilityMod.AddToken(tokens, this.m_energyGainPerLaserHitMod, "EnergyGainPerLaserHit", string.Empty, tricksterBasicAttack.m_energyGainPerLaserHit, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TricksterBasicAttack tricksterBasicAttack = base.GetTargetAbilityOnAbilityData(abilityData) as TricksterBasicAttack;
		bool flag = tricksterBasicAttack != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyLaserInfo laserInfoMod = this.m_laserInfoMod;
		string prefix = "[LaserInfo]";
		bool showBaseVal = flag;
		LaserTargetingInfo baseLaserInfo;
		if (flag)
		{
			baseLaserInfo = tricksterBasicAttack.m_laserInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		text = str + base.PropDesc(laserInfoMod, prefix, showBaseVal, baseLaserInfo);
		string str2 = text;
		AbilityModPropertyInt laserDamageAmountMod = this.m_laserDamageAmountMod;
		string prefix2 = "[LaserDamageAmount]";
		bool showBaseVal2 = flag;
		int baseVal;
		if (flag)
		{
			baseVal = tricksterBasicAttack.m_laserDamageAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str2 + base.PropDesc(laserDamageAmountMod, prefix2, showBaseVal2, baseVal);
		string str3 = text;
		AbilityModPropertyInt laserSubsequentDamageAmountMod = this.m_laserSubsequentDamageAmountMod;
		string prefix3 = "[LaserSubsequentDamageAmount]";
		bool showBaseVal3 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = tricksterBasicAttack.m_laserSubsequentDamageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str3 + base.PropDesc(laserSubsequentDamageAmountMod, prefix3, showBaseVal3, baseVal2);
		string str4 = text;
		AbilityModPropertyInt extraDamageForSingleHitMod = this.m_extraDamageForSingleHitMod;
		string prefix4 = "[ExtraDamageForSingleHit]";
		bool showBaseVal4 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = tricksterBasicAttack.m_extraDamageForSingleHit;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str4 + base.PropDesc(extraDamageForSingleHitMod, prefix4, showBaseVal4, baseVal3);
		string str5 = text;
		AbilityModPropertyEffectInfo enemySingleHitHitEffectMod = this.m_enemySingleHitHitEffectMod;
		string prefix5 = "[EnemySingleHitHitEffect]";
		bool showBaseVal5 = flag;
		StandardEffectInfo baseVal4;
		if (flag)
		{
			baseVal4 = tricksterBasicAttack.m_enemySingleHitHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		text = str5 + base.PropDesc(enemySingleHitHitEffectMod, prefix5, showBaseVal5, baseVal4);
		string str6 = text;
		AbilityModPropertyEffectInfo enemyMultiHitEffectMod = this.m_enemyMultiHitEffectMod;
		string prefix6 = "[EnemyMultiHitEffect]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal5;
		if (flag)
		{
			baseVal5 = tricksterBasicAttack.m_enemyMultiHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		text = str6 + base.PropDesc(enemyMultiHitEffectMod, prefix6, showBaseVal6, baseVal5);
		string str7 = text;
		AbilityModPropertyEffectInfo selfEffectForMultiHitMod = this.m_selfEffectForMultiHitMod;
		string prefix7 = "[SelfEffectForMultiHit]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal6;
		if (flag)
		{
			baseVal6 = tricksterBasicAttack.m_selfEffectForMultiHit;
		}
		else
		{
			baseVal6 = null;
		}
		text = str7 + base.PropDesc(selfEffectForMultiHitMod, prefix7, showBaseVal7, baseVal6);
		string str8 = text;
		AbilityModPropertyInt energyGainPerLaserHitMod = this.m_energyGainPerLaserHitMod;
		string prefix8 = "[EnergyGainPerLaserHit]";
		bool showBaseVal8 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = tricksterBasicAttack.m_energyGainPerLaserHit;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str8 + base.PropDesc(energyGainPerLaserHitMod, prefix8, showBaseVal8, baseVal7);
		string str9 = text;
		AbilityModPropertySpoilsSpawnData spoilSpawnInfoMod = this.m_spoilSpawnInfoMod;
		string prefix9 = "[SpoilSpawnInfo]";
		bool showBaseVal9 = flag;
		SpoilsSpawnData baseVal8;
		if (flag)
		{
			baseVal8 = tricksterBasicAttack.m_spoilSpawnInfo;
		}
		else
		{
			baseVal8 = null;
		}
		text = str9 + base.PropDesc(spoilSpawnInfoMod, prefix9, showBaseVal9, baseVal8);
		return text + base.PropDesc(this.m_onlySpawnSpoilOnMultiHitMod, "[OnlySpawnSpoilOnMultiHit]", flag, flag && tricksterBasicAttack.m_onlySpawnSpoilOnMultiHit);
	}
}
