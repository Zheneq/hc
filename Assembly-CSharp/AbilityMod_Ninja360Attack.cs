using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_Ninja360Attack : AbilityMod
{
	[Separator("Targeting", true)]
	public AbilityModPropertyBool m_penetrateLineOfSightMod;

	[Header("  (( if using Laser ))")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;

	[Header("  (( if using Cone ))")]
	public AbilityModPropertyConeInfo m_coneInfoMod;

	public AbilityModPropertyFloat m_innerConeAngleMod;

	[Header("  (( if using Shape ))")]
	public AbilityModPropertyShape m_targeterShapeMod;

	[Separator("On Hit", true)]
	public AbilityModPropertyInt m_damageAmountMod;

	[Header("-- For Inner Area Damage --")]
	public AbilityModPropertyInt m_innerAreaDamageMod;

	[Space(10f)]
	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	public AbilityModPropertyBool m_useDifferentEffectForInnerConeMod;

	public AbilityModPropertyEffectInfo m_innerConeEnemyHitEffectMod;

	[Header("-- Energy Gain on Marked Target --")]
	public AbilityModPropertyInt m_energyGainOnMarkedHitMod;

	public AbilityModPropertyInt m_selfHealOnMarkedHitMod;

	[Separator("[Deathmark] Effect", "magenta")]
	public AbilityModPropertyBool m_applyDeathmarkEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(Ninja360Attack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		Ninja360Attack ninja360Attack = targetAbility as Ninja360Attack;
		if (ninja360Attack != null)
		{
			AbilityMod.AddToken_LaserInfo(tokens, this.m_laserInfoMod, "LaserInfo", ninja360Attack.m_laserInfo, true);
			AbilityMod.AddToken_ConeInfo(tokens, this.m_coneInfoMod, "ConeInfo", ninja360Attack.m_coneInfo, true);
			AbilityMod.AddToken(tokens, this.m_innerConeAngleMod, "InnerConeAngle", string.Empty, ninja360Attack.m_innerConeAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, ninja360Attack.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_innerAreaDamageMod, "InnerAreaDamage", string.Empty, ninja360Attack.m_innerAreaDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", ninja360Attack.m_enemyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_innerConeEnemyHitEffectMod, "InnerConeEnemyHitEffect", ninja360Attack.m_innerConeEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_energyGainOnMarkedHitMod, "EnergyGainOnMarkedHit", string.Empty, ninja360Attack.m_energyGainOnMarkedHit, true, false);
			AbilityMod.AddToken(tokens, this.m_selfHealOnMarkedHitMod, "SelfHealOnMarkedHit", string.Empty, ninja360Attack.m_selfHealOnMarkedHit, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		Ninja360Attack ninja360Attack = base.GetTargetAbilityOnAbilityData(abilityData) as Ninja360Attack;
		bool flag = ninja360Attack != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyBool penetrateLineOfSightMod = this.m_penetrateLineOfSightMod;
		string prefix = "[PenetrateLineOfSight]";
		bool showBaseVal = flag;
		bool baseVal;
		if (flag)
		{
			baseVal = ninja360Attack.m_penetrateLineOfSight;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(penetrateLineOfSightMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyLaserInfo laserInfoMod = this.m_laserInfoMod;
		string prefix2 = "[LaserInfo]";
		bool showBaseVal2 = flag;
		LaserTargetingInfo baseLaserInfo;
		if (flag)
		{
			baseLaserInfo = ninja360Attack.m_laserInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		text = str2 + base.PropDesc(laserInfoMod, prefix2, showBaseVal2, baseLaserInfo);
		text += base.PropDesc(this.m_coneInfoMod, "[ConeInfo]", flag, (!flag) ? null : ninja360Attack.m_coneInfo);
		string str3 = text;
		AbilityModPropertyFloat innerConeAngleMod = this.m_innerConeAngleMod;
		string prefix3 = "[InnerConeAngle]";
		bool showBaseVal3 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = ninja360Attack.m_innerConeAngle;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str3 + base.PropDesc(innerConeAngleMod, prefix3, showBaseVal3, baseVal2);
		string str4 = text;
		AbilityModPropertyShape targeterShapeMod = this.m_targeterShapeMod;
		string prefix4 = "[TargeterShape]";
		bool showBaseVal4 = flag;
		AbilityAreaShape baseVal3;
		if (flag)
		{
			baseVal3 = ninja360Attack.m_targeterShape;
		}
		else
		{
			baseVal3 = AbilityAreaShape.SingleSquare;
		}
		text = str4 + base.PropDesc(targeterShapeMod, prefix4, showBaseVal4, baseVal3);
		string str5 = text;
		AbilityModPropertyInt damageAmountMod = this.m_damageAmountMod;
		string prefix5 = "[DamageAmount]";
		bool showBaseVal5 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = ninja360Attack.m_damageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str5 + base.PropDesc(damageAmountMod, prefix5, showBaseVal5, baseVal4);
		string str6 = text;
		AbilityModPropertyInt innerAreaDamageMod = this.m_innerAreaDamageMod;
		string prefix6 = "[InnerAreaDamage]";
		bool showBaseVal6 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = ninja360Attack.m_innerAreaDamage;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str6 + base.PropDesc(innerAreaDamageMod, prefix6, showBaseVal6, baseVal5);
		string str7 = text;
		AbilityModPropertyEffectInfo enemyHitEffectMod = this.m_enemyHitEffectMod;
		string prefix7 = "[EnemyHitEffect]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal6;
		if (flag)
		{
			baseVal6 = ninja360Attack.m_enemyHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		text = str7 + base.PropDesc(enemyHitEffectMod, prefix7, showBaseVal7, baseVal6);
		string str8 = text;
		AbilityModPropertyBool useDifferentEffectForInnerConeMod = this.m_useDifferentEffectForInnerConeMod;
		string prefix8 = "[UseDifferentEffectForInnerCone]";
		bool showBaseVal8 = flag;
		bool baseVal7;
		if (flag)
		{
			baseVal7 = ninja360Attack.m_useDifferentEffectForInnerCone;
		}
		else
		{
			baseVal7 = false;
		}
		text = str8 + base.PropDesc(useDifferentEffectForInnerConeMod, prefix8, showBaseVal8, baseVal7);
		text += base.PropDesc(this.m_innerConeEnemyHitEffectMod, "[InnerConeEnemyHitEffect]", flag, (!flag) ? null : ninja360Attack.m_innerConeEnemyHitEffect);
		string str9 = text;
		AbilityModPropertyInt energyGainOnMarkedHitMod = this.m_energyGainOnMarkedHitMod;
		string prefix9 = "[EnergyGainOnMarkedHit]";
		bool showBaseVal9 = flag;
		int baseVal8;
		if (flag)
		{
			baseVal8 = ninja360Attack.m_energyGainOnMarkedHit;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str9 + base.PropDesc(energyGainOnMarkedHitMod, prefix9, showBaseVal9, baseVal8);
		string str10 = text;
		AbilityModPropertyInt selfHealOnMarkedHitMod = this.m_selfHealOnMarkedHitMod;
		string prefix10 = "[SelfHealOnMarkedHit]";
		bool showBaseVal10 = flag;
		int baseVal9;
		if (flag)
		{
			baseVal9 = ninja360Attack.m_selfHealOnMarkedHit;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str10 + base.PropDesc(selfHealOnMarkedHitMod, prefix10, showBaseVal10, baseVal9);
		string str11 = text;
		AbilityModPropertyBool applyDeathmarkEffectMod = this.m_applyDeathmarkEffectMod;
		string prefix11 = "[ApplyDeathmarkEffect]";
		bool showBaseVal11 = flag;
		bool baseVal10;
		if (flag)
		{
			baseVal10 = ninja360Attack.m_applyDeathmarkEffect;
		}
		else
		{
			baseVal10 = false;
		}
		return str11 + base.PropDesc(applyDeathmarkEffectMod, prefix11, showBaseVal11, baseVal10);
	}
}
