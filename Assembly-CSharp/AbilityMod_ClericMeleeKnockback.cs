using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ClericMeleeKnockback : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyBool m_penetrateLineOfSightMod;

	public AbilityModPropertyFloat m_minSeparationBetweenAoeAndCasterMod;

	public AbilityModPropertyFloat m_maxSeparationBetweenAoeAndCasterMod;

	public AbilityModPropertyFloat m_aoeRadiusMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	[Header("-- On Hit Damage/Effect")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyFloat m_knockbackDistanceMod;

	public AbilityModPropertyKnockbackType m_knockbackTypeMod;

	public AbilityModPropertyEffectInfo m_targetHitEffectMod;

	public AbilityModPropertyEffectInfo m_singleTargetHitEffectMod;

	public AbilityModPropertyInt m_extraTechPointsPerHitWithAreaBuff;

	[Separator("Connecting Laser between caster and aoe center", true)]
	public AbilityModPropertyFloat m_connectLaserWidthMod;

	public AbilityModPropertyInt m_connectLaserDamageMod;

	public AbilityModPropertyEffectInfo m_connectLaserEnemyHitEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ClericMeleeKnockback);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ClericMeleeKnockback clericMeleeKnockback = targetAbility as ClericMeleeKnockback;
		if (!(clericMeleeKnockback != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_minSeparationBetweenAoeAndCasterMod, "MinSeparationBetweenAoeAndCaster", string.Empty, clericMeleeKnockback.m_minSeparationBetweenAoeAndCaster);
			AbilityMod.AddToken(tokens, m_maxSeparationBetweenAoeAndCasterMod, "MaxSeparationBetweenAoeAndCaster", string.Empty, clericMeleeKnockback.m_maxSeparationBetweenAoeAndCaster);
			AbilityMod.AddToken(tokens, m_aoeRadiusMod, "AoeRadius", string.Empty, clericMeleeKnockback.m_aoeRadius);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, clericMeleeKnockback.m_maxTargets);
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, clericMeleeKnockback.m_damageAmount);
			AbilityMod.AddToken(tokens, m_knockbackDistanceMod, "KnockbackDistance", string.Empty, clericMeleeKnockback.m_knockbackDistance);
			AbilityMod.AddToken_EffectMod(tokens, m_targetHitEffectMod, "TargetHitEffect", clericMeleeKnockback.m_targetHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_singleTargetHitEffectMod, "SingleTargetHitEffect", clericMeleeKnockback.m_targetHitEffect);
			AbilityMod.AddToken(tokens, m_extraTechPointsPerHitWithAreaBuff, "ExtraEnergyPerHitWithAreaBuff", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_connectLaserWidthMod, "ConnectLaserWidth", string.Empty, clericMeleeKnockback.m_connectLaserWidth);
			AbilityMod.AddToken(tokens, m_connectLaserDamageMod, "ConnectLaserDamage", string.Empty, clericMeleeKnockback.m_connectLaserDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_connectLaserEnemyHitEffectMod, "ConnectLaserEnemyHitEffect", clericMeleeKnockback.m_connectLaserEnemyHitEffect);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClericMeleeKnockback clericMeleeKnockback = GetTargetAbilityOnAbilityData(abilityData) as ClericMeleeKnockback;
		bool flag = clericMeleeKnockback != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyBool penetrateLineOfSightMod = m_penetrateLineOfSightMod;
		int baseVal;
		if (flag)
		{
			baseVal = (clericMeleeKnockback.m_penetrateLineOfSight ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, (byte)baseVal != 0);
		string str2 = empty;
		AbilityModPropertyFloat minSeparationBetweenAoeAndCasterMod = m_minSeparationBetweenAoeAndCasterMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = clericMeleeKnockback.m_minSeparationBetweenAoeAndCaster;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(minSeparationBetweenAoeAndCasterMod, "[MinSeparationBetweenAoeAndCaster]", flag, baseVal2);
		empty += PropDesc(m_maxSeparationBetweenAoeAndCasterMod, "[MaxSeparationBetweenAoeAndCaster]", flag, (!flag) ? 0f : clericMeleeKnockback.m_maxSeparationBetweenAoeAndCaster);
		string str3 = empty;
		AbilityModPropertyFloat aoeRadiusMod = m_aoeRadiusMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = clericMeleeKnockback.m_aoeRadius;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(aoeRadiusMod, "[AoeRadius]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyInt maxTargetsMod = m_maxTargetsMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = clericMeleeKnockback.m_maxTargets;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(maxTargetsMod, "[MaxTargets]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyInt damageAmountMod = m_damageAmountMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = clericMeleeKnockback.m_damageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(damageAmountMod, "[DamageAmount]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyFloat knockbackDistanceMod = m_knockbackDistanceMod;
		float baseVal6;
		if (flag)
		{
			baseVal6 = clericMeleeKnockback.m_knockbackDistance;
		}
		else
		{
			baseVal6 = 0f;
		}
		empty = str6 + PropDesc(knockbackDistanceMod, "[KnockbackDistance]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyKnockbackType knockbackTypeMod = m_knockbackTypeMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = (int)clericMeleeKnockback.m_knockbackType;
		}
		else
		{
			baseVal7 = 4;
		}
		empty = str7 + PropDesc(knockbackTypeMod, "[KnockbackType]", flag, (KnockbackType)baseVal7);
		string str8 = empty;
		AbilityModPropertyEffectInfo targetHitEffectMod = m_targetHitEffectMod;
		object baseVal8;
		if (flag)
		{
			baseVal8 = clericMeleeKnockback.m_targetHitEffect;
		}
		else
		{
			baseVal8 = null;
		}
		empty = str8 + PropDesc(targetHitEffectMod, "[TargetHitEffect]", flag, (StandardEffectInfo)baseVal8);
		empty += PropDesc(m_extraTechPointsPerHitWithAreaBuff, "[ExtraEnergyPerHitWithAreaBuff]", flag);
		string str9 = empty;
		AbilityModPropertyFloat connectLaserWidthMod = m_connectLaserWidthMod;
		float baseVal9;
		if (flag)
		{
			baseVal9 = clericMeleeKnockback.m_connectLaserWidth;
		}
		else
		{
			baseVal9 = 0f;
		}
		empty = str9 + PropDesc(connectLaserWidthMod, "[ConnectLaserWidth]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyInt connectLaserDamageMod = m_connectLaserDamageMod;
		int baseVal10;
		if (flag)
		{
			baseVal10 = clericMeleeKnockback.m_connectLaserDamage;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(connectLaserDamageMod, "[ConnectLaserDamage]", flag, baseVal10);
		string str11 = empty;
		AbilityModPropertyEffectInfo connectLaserEnemyHitEffectMod = m_connectLaserEnemyHitEffectMod;
		object baseVal11;
		if (flag)
		{
			baseVal11 = clericMeleeKnockback.m_connectLaserEnemyHitEffect;
		}
		else
		{
			baseVal11 = null;
		}
		return str11 + PropDesc(connectLaserEnemyHitEffectMod, "[ConnectLaserEnemyHitEffect]", flag, (StandardEffectInfo)baseVal11);
	}
}
