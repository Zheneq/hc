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
		if (!(ninja360Attack != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", ninja360Attack.m_laserInfo);
			AbilityMod.AddToken_ConeInfo(tokens, m_coneInfoMod, "ConeInfo", ninja360Attack.m_coneInfo);
			AbilityMod.AddToken(tokens, m_innerConeAngleMod, "InnerConeAngle", string.Empty, ninja360Attack.m_innerConeAngle);
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, ninja360Attack.m_damageAmount);
			AbilityMod.AddToken(tokens, m_innerAreaDamageMod, "InnerAreaDamage", string.Empty, ninja360Attack.m_innerAreaDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", ninja360Attack.m_enemyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_innerConeEnemyHitEffectMod, "InnerConeEnemyHitEffect", ninja360Attack.m_innerConeEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_energyGainOnMarkedHitMod, "EnergyGainOnMarkedHit", string.Empty, ninja360Attack.m_energyGainOnMarkedHit);
			AbilityMod.AddToken(tokens, m_selfHealOnMarkedHitMod, "SelfHealOnMarkedHit", string.Empty, ninja360Attack.m_selfHealOnMarkedHit);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		Ninja360Attack ninja360Attack = GetTargetAbilityOnAbilityData(abilityData) as Ninja360Attack;
		bool flag = ninja360Attack != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyBool penetrateLineOfSightMod = m_penetrateLineOfSightMod;
		int baseVal;
		if (flag)
		{
			baseVal = (ninja360Attack.m_penetrateLineOfSight ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, (byte)baseVal != 0);
		string str2 = empty;
		AbilityModPropertyLaserInfo laserInfoMod = m_laserInfoMod;
		object baseLaserInfo;
		if (flag)
		{
			baseLaserInfo = ninja360Attack.m_laserInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		empty = str2 + PropDesc(laserInfoMod, "[LaserInfo]", flag, (LaserTargetingInfo)baseLaserInfo);
		empty += PropDesc(m_coneInfoMod, "[ConeInfo]", flag, (!flag) ? null : ninja360Attack.m_coneInfo);
		string str3 = empty;
		AbilityModPropertyFloat innerConeAngleMod = m_innerConeAngleMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = ninja360Attack.m_innerConeAngle;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str3 + PropDesc(innerConeAngleMod, "[InnerConeAngle]", flag, baseVal2);
		string str4 = empty;
		AbilityModPropertyShape targeterShapeMod = m_targeterShapeMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = (int)ninja360Attack.m_targeterShape;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str4 + PropDesc(targeterShapeMod, "[TargeterShape]", flag, (AbilityAreaShape)baseVal3);
		string str5 = empty;
		AbilityModPropertyInt damageAmountMod = m_damageAmountMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = ninja360Attack.m_damageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str5 + PropDesc(damageAmountMod, "[DamageAmount]", flag, baseVal4);
		string str6 = empty;
		AbilityModPropertyInt innerAreaDamageMod = m_innerAreaDamageMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = ninja360Attack.m_innerAreaDamage;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str6 + PropDesc(innerAreaDamageMod, "[InnerAreaDamage]", flag, baseVal5);
		string str7 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectMod = m_enemyHitEffectMod;
		object baseVal6;
		if (flag)
		{
			baseVal6 = ninja360Attack.m_enemyHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str7 + PropDesc(enemyHitEffectMod, "[EnemyHitEffect]", flag, (StandardEffectInfo)baseVal6);
		string str8 = empty;
		AbilityModPropertyBool useDifferentEffectForInnerConeMod = m_useDifferentEffectForInnerConeMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = (ninja360Attack.m_useDifferentEffectForInnerCone ? 1 : 0);
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str8 + PropDesc(useDifferentEffectForInnerConeMod, "[UseDifferentEffectForInnerCone]", flag, (byte)baseVal7 != 0);
		empty += PropDesc(m_innerConeEnemyHitEffectMod, "[InnerConeEnemyHitEffect]", flag, (!flag) ? null : ninja360Attack.m_innerConeEnemyHitEffect);
		string str9 = empty;
		AbilityModPropertyInt energyGainOnMarkedHitMod = m_energyGainOnMarkedHitMod;
		int baseVal8;
		if (flag)
		{
			baseVal8 = ninja360Attack.m_energyGainOnMarkedHit;
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str9 + PropDesc(energyGainOnMarkedHitMod, "[EnergyGainOnMarkedHit]", flag, baseVal8);
		string str10 = empty;
		AbilityModPropertyInt selfHealOnMarkedHitMod = m_selfHealOnMarkedHitMod;
		int baseVal9;
		if (flag)
		{
			baseVal9 = ninja360Attack.m_selfHealOnMarkedHit;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str10 + PropDesc(selfHealOnMarkedHitMod, "[SelfHealOnMarkedHit]", flag, baseVal9);
		string str11 = empty;
		AbilityModPropertyBool applyDeathmarkEffectMod = m_applyDeathmarkEffectMod;
		int baseVal10;
		if (flag)
		{
			baseVal10 = (ninja360Attack.m_applyDeathmarkEffect ? 1 : 0);
		}
		else
		{
			baseVal10 = 0;
		}
		return str11 + PropDesc(applyDeathmarkEffectMod, "[ApplyDeathmarkEffect]", flag, (byte)baseVal10 != 0);
	}
}
