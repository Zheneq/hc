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
		if (senseiBasicAttack != null)
		{
			AbilityMod.AddToken(tokens, this.m_circleDistThresholdMod, "CircleDistThreshold", string.Empty, senseiBasicAttack.m_circleDistThreshold, true, false, false);
			AbilityMod.AddToken(tokens, this.m_circleRadiusMod, "CircleRadius", string.Empty, senseiBasicAttack.m_circleRadius, true, false, false);
			AbilityMod.AddToken_LaserInfo(tokens, this.m_laserInfoMod, "LaserInfo", senseiBasicAttack.m_laserInfo, true);
			AbilityMod.AddToken(tokens, this.m_circleDamageMod, "CircleDamage", string.Empty, senseiBasicAttack.m_circleDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_circleEnemyHitEffectMod, "CircleEnemyHitEffect", senseiBasicAttack.m_circleEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_laserDamageMod, "LaserDamage", string.Empty, senseiBasicAttack.m_laserDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_laserEnemyHitEffectMod, "LaserEnemyHitEffect", senseiBasicAttack.m_laserEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_extraDamageForAlternatingMod, "ExtraDamageForAlternating", string.Empty, senseiBasicAttack.m_extraDamageForAlternating, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageForFarTargetMod, "ExtraDamageForFarTarget", string.Empty, senseiBasicAttack.m_extraDamageForFarTarget, true, false);
			AbilityMod.AddToken(tokens, this.m_laserFarDistThreshMod, "LaserFarDistThresh", string.Empty, senseiBasicAttack.m_laserFarDistThresh, true, false, false);
			AbilityMod.AddToken(tokens, this.m_circleFarDistThreshMod, "CircleFarDistThresh", string.Empty, senseiBasicAttack.m_circleFarDistThresh, true, false, false);
			AbilityMod.AddToken(tokens, this.m_healPerEnemyHitMod, "HealPerEnemyHit", string.Empty, senseiBasicAttack.m_healPerEnemyHit, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrOnAbilityMod, "CdrOnAbility", string.Empty, senseiBasicAttack.m_cdrOnAbility, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrMinTriggerHitCountMod, "CdrMinTriggerHitCount", string.Empty, senseiBasicAttack.m_cdrMinTriggerHitCount, true, false);
			AbilityMod.AddToken(tokens, this.m_absorbPerEnemyHitOnTurnStartMod, "AbsorbPerEnemyHitOnTurnStart", string.Empty, senseiBasicAttack.m_absorbPerEnemyHitOnTurnStart, true, false);
			AbilityMod.AddToken(tokens, this.m_absorbAmountIfTriggeredHitCountMod, "AbsorbAmountIfTriggeredHitCount", string.Empty, senseiBasicAttack.m_absorbAmountIfTriggeredHitCount, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SenseiBasicAttack senseiBasicAttack = base.GetTargetAbilityOnAbilityData(abilityData) as SenseiBasicAttack;
		bool flag = senseiBasicAttack != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat circleDistThresholdMod = this.m_circleDistThresholdMod;
		string prefix = "[CircleDistThreshold]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = senseiBasicAttack.m_circleDistThreshold;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(circleDistThresholdMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat circleRadiusMod = this.m_circleRadiusMod;
		string prefix2 = "[CircleRadius]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = senseiBasicAttack.m_circleRadius;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(circleRadiusMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyLaserInfo laserInfoMod = this.m_laserInfoMod;
		string prefix3 = "[LaserInfo]";
		bool showBaseVal3 = flag;
		LaserTargetingInfo baseLaserInfo;
		if (flag)
		{
			baseLaserInfo = senseiBasicAttack.m_laserInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		text = str3 + base.PropDesc(laserInfoMod, prefix3, showBaseVal3, baseLaserInfo);
		text += base.PropDesc(this.m_circleDamageMod, "[CircleDamage]", flag, (!flag) ? 0 : senseiBasicAttack.m_circleDamage);
		string str4 = text;
		AbilityModPropertyEffectInfo circleEnemyHitEffectMod = this.m_circleEnemyHitEffectMod;
		string prefix4 = "[CircleEnemyHitEffect]";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal3;
		if (flag)
		{
			baseVal3 = senseiBasicAttack.m_circleEnemyHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		text = str4 + base.PropDesc(circleEnemyHitEffectMod, prefix4, showBaseVal4, baseVal3);
		string str5 = text;
		AbilityModPropertyInt laserDamageMod = this.m_laserDamageMod;
		string prefix5 = "[LaserDamage]";
		bool showBaseVal5 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = senseiBasicAttack.m_laserDamage;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str5 + base.PropDesc(laserDamageMod, prefix5, showBaseVal5, baseVal4);
		string str6 = text;
		AbilityModPropertyEffectInfo laserEnemyHitEffectMod = this.m_laserEnemyHitEffectMod;
		string prefix6 = "[LaserEnemyHitEffect]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal5;
		if (flag)
		{
			baseVal5 = senseiBasicAttack.m_laserEnemyHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		text = str6 + base.PropDesc(laserEnemyHitEffectMod, prefix6, showBaseVal6, baseVal5);
		string str7 = text;
		AbilityModPropertyInt extraDamageForAlternatingMod = this.m_extraDamageForAlternatingMod;
		string prefix7 = "[ExtraDamageForAlternating]";
		bool showBaseVal7 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = senseiBasicAttack.m_extraDamageForAlternating;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str7 + base.PropDesc(extraDamageForAlternatingMod, prefix7, showBaseVal7, baseVal6);
		string str8 = text;
		AbilityModPropertyInt extraDamageForFarTargetMod = this.m_extraDamageForFarTargetMod;
		string prefix8 = "[ExtraDamageForFarTarget]";
		bool showBaseVal8 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = senseiBasicAttack.m_extraDamageForFarTarget;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str8 + base.PropDesc(extraDamageForFarTargetMod, prefix8, showBaseVal8, baseVal7);
		string str9 = text;
		AbilityModPropertyFloat laserFarDistThreshMod = this.m_laserFarDistThreshMod;
		string prefix9 = "[LaserFarDistThresh]";
		bool showBaseVal9 = flag;
		float baseVal8;
		if (flag)
		{
			baseVal8 = senseiBasicAttack.m_laserFarDistThresh;
		}
		else
		{
			baseVal8 = 0f;
		}
		text = str9 + base.PropDesc(laserFarDistThreshMod, prefix9, showBaseVal9, baseVal8);
		string str10 = text;
		AbilityModPropertyFloat circleFarDistThreshMod = this.m_circleFarDistThreshMod;
		string prefix10 = "[CircleFarDistThresh]";
		bool showBaseVal10 = flag;
		float baseVal9;
		if (flag)
		{
			baseVal9 = senseiBasicAttack.m_circleFarDistThresh;
		}
		else
		{
			baseVal9 = 0f;
		}
		text = str10 + base.PropDesc(circleFarDistThreshMod, prefix10, showBaseVal10, baseVal9);
		text += base.PropDesc(this.m_healPerEnemyHitMod, "[HealPerEnemyHit]", flag, (!flag) ? 0 : senseiBasicAttack.m_healPerEnemyHit);
		string str11 = text;
		AbilityModPropertyInt cdrOnAbilityMod = this.m_cdrOnAbilityMod;
		string prefix11 = "[CdrOnAbility]";
		bool showBaseVal11 = flag;
		int baseVal10;
		if (flag)
		{
			baseVal10 = senseiBasicAttack.m_cdrOnAbility;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str11 + base.PropDesc(cdrOnAbilityMod, prefix11, showBaseVal11, baseVal10);
		string str12 = text;
		AbilityModPropertyInt cdrMinTriggerHitCountMod = this.m_cdrMinTriggerHitCountMod;
		string prefix12 = "[CdrMinTriggerHitCount]";
		bool showBaseVal12 = flag;
		int baseVal11;
		if (flag)
		{
			baseVal11 = senseiBasicAttack.m_cdrMinTriggerHitCount;
		}
		else
		{
			baseVal11 = 0;
		}
		text = str12 + base.PropDesc(cdrMinTriggerHitCountMod, prefix12, showBaseVal12, baseVal11);
		string str13 = text;
		AbilityModPropertyInt absorbPerEnemyHitOnTurnStartMod = this.m_absorbPerEnemyHitOnTurnStartMod;
		string prefix13 = "[AbsorbPerEnemyHitOnTurnStart]";
		bool showBaseVal13 = flag;
		int baseVal12;
		if (flag)
		{
			baseVal12 = senseiBasicAttack.m_absorbPerEnemyHitOnTurnStart;
		}
		else
		{
			baseVal12 = 0;
		}
		text = str13 + base.PropDesc(absorbPerEnemyHitOnTurnStartMod, prefix13, showBaseVal13, baseVal12);
		string str14 = text;
		AbilityModPropertyInt absorbAmountIfTriggeredHitCountMod = this.m_absorbAmountIfTriggeredHitCountMod;
		string prefix14 = "[AbsorbAmountIfTriggeredHitCount]";
		bool showBaseVal14 = flag;
		int baseVal13;
		if (flag)
		{
			baseVal13 = senseiBasicAttack.m_absorbAmountIfTriggeredHitCount;
		}
		else
		{
			baseVal13 = 0;
		}
		return str14 + base.PropDesc(absorbAmountIfTriggeredHitCountMod, prefix14, showBaseVal14, baseVal13);
	}
}
