using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SenseiBasicAttack : AbilityMod
{
	[Separator("Targeting Info", "cyan")]
	public AbilityModPropertyFloat m_circleDistThresholdMod;

	[Header("  Targeting: For Circle")]
	public AbilityModPropertyFloat m_circleRadiusMod;

	[Header("  Targeting: For Laser")]
	public AbilityModPropertyLaserInfo m_laserInfoMod;

	[Separator("On Hit Stuff", "cyan")]
	public AbilityModPropertyInt m_circleDamageMod;

	public AbilityModPropertyEffectInfo m_circleEnemyHitEffectMod;

	[Space(10f)]
	public AbilityModPropertyInt m_laserDamageMod;

	public AbilityModPropertyEffectInfo m_laserEnemyHitEffectMod;

	[Header("-- Extra Damage: alternate use")]
	public AbilityModPropertyInt m_extraDamageForAlternatingMod;

	[Header("-- Extra Damage: far away target hits")]
	public AbilityModPropertyInt m_extraDamageForFarTargetMod;

	public AbilityModPropertyFloat m_laserFarDistThreshMod;

	public AbilityModPropertyFloat m_circleFarDistThreshMod;

	[Separator("Heal Per Target Hit", true)]
	public AbilityModPropertyInt m_healPerEnemyHitMod;

	[Separator("Cooldown Reduction", true)]
	public AbilityModPropertyInt m_cdrOnAbilityMod;

	public AbilityModPropertyInt m_cdrMinTriggerHitCountMod;

	[Separator("Shielding on turn start per enemy hit", true)]
	public AbilityModPropertyInt m_absorbPerEnemyHitOnTurnStartMod;

	public AbilityModPropertyInt m_absorbAmountIfTriggeredHitCountMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SenseiBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SenseiBasicAttack senseiBasicAttack = targetAbility as SenseiBasicAttack;
		if (!(senseiBasicAttack != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_circleDistThresholdMod, "CircleDistThreshold", string.Empty, senseiBasicAttack.m_circleDistThreshold);
			AbilityMod.AddToken(tokens, m_circleRadiusMod, "CircleRadius", string.Empty, senseiBasicAttack.m_circleRadius);
			AbilityMod.AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", senseiBasicAttack.m_laserInfo);
			AbilityMod.AddToken(tokens, m_circleDamageMod, "CircleDamage", string.Empty, senseiBasicAttack.m_circleDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_circleEnemyHitEffectMod, "CircleEnemyHitEffect", senseiBasicAttack.m_circleEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_laserDamageMod, "LaserDamage", string.Empty, senseiBasicAttack.m_laserDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_laserEnemyHitEffectMod, "LaserEnemyHitEffect", senseiBasicAttack.m_laserEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_extraDamageForAlternatingMod, "ExtraDamageForAlternating", string.Empty, senseiBasicAttack.m_extraDamageForAlternating);
			AbilityMod.AddToken(tokens, m_extraDamageForFarTargetMod, "ExtraDamageForFarTarget", string.Empty, senseiBasicAttack.m_extraDamageForFarTarget);
			AbilityMod.AddToken(tokens, m_laserFarDistThreshMod, "LaserFarDistThresh", string.Empty, senseiBasicAttack.m_laserFarDistThresh);
			AbilityMod.AddToken(tokens, m_circleFarDistThreshMod, "CircleFarDistThresh", string.Empty, senseiBasicAttack.m_circleFarDistThresh);
			AbilityMod.AddToken(tokens, m_healPerEnemyHitMod, "HealPerEnemyHit", string.Empty, senseiBasicAttack.m_healPerEnemyHit);
			AbilityMod.AddToken(tokens, m_cdrOnAbilityMod, "CdrOnAbility", string.Empty, senseiBasicAttack.m_cdrOnAbility);
			AbilityMod.AddToken(tokens, m_cdrMinTriggerHitCountMod, "CdrMinTriggerHitCount", string.Empty, senseiBasicAttack.m_cdrMinTriggerHitCount);
			AbilityMod.AddToken(tokens, m_absorbPerEnemyHitOnTurnStartMod, "AbsorbPerEnemyHitOnTurnStart", string.Empty, senseiBasicAttack.m_absorbPerEnemyHitOnTurnStart);
			AbilityMod.AddToken(tokens, m_absorbAmountIfTriggeredHitCountMod, "AbsorbAmountIfTriggeredHitCount", string.Empty, senseiBasicAttack.m_absorbAmountIfTriggeredHitCount);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SenseiBasicAttack senseiBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as SenseiBasicAttack;
		bool flag = senseiBasicAttack != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat circleDistThresholdMod = m_circleDistThresholdMod;
		float baseVal;
		if (flag)
		{
			baseVal = senseiBasicAttack.m_circleDistThreshold;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(circleDistThresholdMod, "[CircleDistThreshold]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat circleRadiusMod = m_circleRadiusMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = senseiBasicAttack.m_circleRadius;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(circleRadiusMod, "[CircleRadius]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyLaserInfo laserInfoMod = m_laserInfoMod;
		object baseLaserInfo;
		if (flag)
		{
			baseLaserInfo = senseiBasicAttack.m_laserInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		empty = str3 + PropDesc(laserInfoMod, "[LaserInfo]", flag, (LaserTargetingInfo)baseLaserInfo);
		empty += PropDesc(m_circleDamageMod, "[CircleDamage]", flag, flag ? senseiBasicAttack.m_circleDamage : 0);
		string str4 = empty;
		AbilityModPropertyEffectInfo circleEnemyHitEffectMod = m_circleEnemyHitEffectMod;
		object baseVal3;
		if (flag)
		{
			baseVal3 = senseiBasicAttack.m_circleEnemyHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		empty = str4 + PropDesc(circleEnemyHitEffectMod, "[CircleEnemyHitEffect]", flag, (StandardEffectInfo)baseVal3);
		string str5 = empty;
		AbilityModPropertyInt laserDamageMod = m_laserDamageMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = senseiBasicAttack.m_laserDamage;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str5 + PropDesc(laserDamageMod, "[LaserDamage]", flag, baseVal4);
		string str6 = empty;
		AbilityModPropertyEffectInfo laserEnemyHitEffectMod = m_laserEnemyHitEffectMod;
		object baseVal5;
		if (flag)
		{
			baseVal5 = senseiBasicAttack.m_laserEnemyHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str6 + PropDesc(laserEnemyHitEffectMod, "[LaserEnemyHitEffect]", flag, (StandardEffectInfo)baseVal5);
		string str7 = empty;
		AbilityModPropertyInt extraDamageForAlternatingMod = m_extraDamageForAlternatingMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = senseiBasicAttack.m_extraDamageForAlternating;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str7 + PropDesc(extraDamageForAlternatingMod, "[ExtraDamageForAlternating]", flag, baseVal6);
		string str8 = empty;
		AbilityModPropertyInt extraDamageForFarTargetMod = m_extraDamageForFarTargetMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = senseiBasicAttack.m_extraDamageForFarTarget;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str8 + PropDesc(extraDamageForFarTargetMod, "[ExtraDamageForFarTarget]", flag, baseVal7);
		string str9 = empty;
		AbilityModPropertyFloat laserFarDistThreshMod = m_laserFarDistThreshMod;
		float baseVal8;
		if (flag)
		{
			baseVal8 = senseiBasicAttack.m_laserFarDistThresh;
		}
		else
		{
			baseVal8 = 0f;
		}
		empty = str9 + PropDesc(laserFarDistThreshMod, "[LaserFarDistThresh]", flag, baseVal8);
		string str10 = empty;
		AbilityModPropertyFloat circleFarDistThreshMod = m_circleFarDistThreshMod;
		float baseVal9;
		if (flag)
		{
			baseVal9 = senseiBasicAttack.m_circleFarDistThresh;
		}
		else
		{
			baseVal9 = 0f;
		}
		empty = str10 + PropDesc(circleFarDistThreshMod, "[CircleFarDistThresh]", flag, baseVal9);
		empty += PropDesc(m_healPerEnemyHitMod, "[HealPerEnemyHit]", flag, flag ? senseiBasicAttack.m_healPerEnemyHit : 0);
		string str11 = empty;
		AbilityModPropertyInt cdrOnAbilityMod = m_cdrOnAbilityMod;
		int baseVal10;
		if (flag)
		{
			baseVal10 = senseiBasicAttack.m_cdrOnAbility;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str11 + PropDesc(cdrOnAbilityMod, "[CdrOnAbility]", flag, baseVal10);
		string str12 = empty;
		AbilityModPropertyInt cdrMinTriggerHitCountMod = m_cdrMinTriggerHitCountMod;
		int baseVal11;
		if (flag)
		{
			baseVal11 = senseiBasicAttack.m_cdrMinTriggerHitCount;
		}
		else
		{
			baseVal11 = 0;
		}
		empty = str12 + PropDesc(cdrMinTriggerHitCountMod, "[CdrMinTriggerHitCount]", flag, baseVal11);
		string str13 = empty;
		AbilityModPropertyInt absorbPerEnemyHitOnTurnStartMod = m_absorbPerEnemyHitOnTurnStartMod;
		int baseVal12;
		if (flag)
		{
			baseVal12 = senseiBasicAttack.m_absorbPerEnemyHitOnTurnStart;
		}
		else
		{
			baseVal12 = 0;
		}
		empty = str13 + PropDesc(absorbPerEnemyHitOnTurnStartMod, "[AbsorbPerEnemyHitOnTurnStart]", flag, baseVal12);
		string str14 = empty;
		AbilityModPropertyInt absorbAmountIfTriggeredHitCountMod = m_absorbAmountIfTriggeredHitCountMod;
		int baseVal13;
		if (flag)
		{
			baseVal13 = senseiBasicAttack.m_absorbAmountIfTriggeredHitCount;
		}
		else
		{
			baseVal13 = 0;
		}
		return str14 + PropDesc(absorbAmountIfTriggeredHitCountMod, "[AbsorbAmountIfTriggeredHitCount]", flag, baseVal13);
	}
}
