// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SenseiBasicAttack : AbilityMod
{
	[Separator("Targeting Info", "cyan")]
	public AbilityModPropertyFloat m_circleDistThresholdMod;  // unused
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
	[Separator("Heal Per Target Hit")]
	public AbilityModPropertyInt m_healPerEnemyHitMod;
	[Separator("Cooldown Reduction")]
	public AbilityModPropertyInt m_cdrOnAbilityMod;
	public AbilityModPropertyInt m_cdrMinTriggerHitCountMod;
	[Separator("Shielding on turn start per enemy hit")]
	public AbilityModPropertyInt m_absorbPerEnemyHitOnTurnStartMod;
	
	// removed in rogues
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
			AddToken(tokens, m_circleDistThresholdMod, "CircleDistThreshold", string.Empty, senseiBasicAttack.m_circleDistThreshold);
			AddToken(tokens, m_circleRadiusMod, "CircleRadius", string.Empty, senseiBasicAttack.m_circleRadius);
			AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", senseiBasicAttack.m_laserInfo);
			AddToken(tokens, m_circleDamageMod, "CircleDamage", string.Empty, senseiBasicAttack.m_circleDamage);
			AddToken_EffectMod(tokens, m_circleEnemyHitEffectMod, "CircleEnemyHitEffect", senseiBasicAttack.m_circleEnemyHitEffect);
			AddToken(tokens, m_laserDamageMod, "LaserDamage", string.Empty, senseiBasicAttack.m_laserDamage);
			AddToken_EffectMod(tokens, m_laserEnemyHitEffectMod, "LaserEnemyHitEffect", senseiBasicAttack.m_laserEnemyHitEffect);
			AddToken(tokens, m_extraDamageForAlternatingMod, "ExtraDamageForAlternating", string.Empty, senseiBasicAttack.m_extraDamageForAlternating);
			AddToken(tokens, m_extraDamageForFarTargetMod, "ExtraDamageForFarTarget", string.Empty, senseiBasicAttack.m_extraDamageForFarTarget);
			AddToken(tokens, m_laserFarDistThreshMod, "LaserFarDistThresh", string.Empty, senseiBasicAttack.m_laserFarDistThresh);
			AddToken(tokens, m_circleFarDistThreshMod, "CircleFarDistThresh", string.Empty, senseiBasicAttack.m_circleFarDistThresh);
			AddToken(tokens, m_healPerEnemyHitMod, "HealPerEnemyHit", string.Empty, senseiBasicAttack.m_healPerEnemyHit);
			AddToken(tokens, m_cdrOnAbilityMod, "CdrOnAbility", string.Empty, senseiBasicAttack.m_cdrOnAbility);
			AddToken(tokens, m_cdrMinTriggerHitCountMod, "CdrMinTriggerHitCount", string.Empty, senseiBasicAttack.m_cdrMinTriggerHitCount);
			AddToken(tokens, m_absorbPerEnemyHitOnTurnStartMod, "AbsorbPerEnemyHitOnTurnStart", string.Empty, senseiBasicAttack.m_absorbPerEnemyHitOnTurnStart);
			
			// removed in rogues
			AddToken(tokens, m_absorbAmountIfTriggeredHitCountMod, "AbsorbAmountIfTriggeredHitCount", string.Empty, senseiBasicAttack.m_absorbAmountIfTriggeredHitCount);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)  // , Ability targetAbility in rogues
	{
		// reactor
		SenseiBasicAttack senseiBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as SenseiBasicAttack;
		// rogues
		// SenseiBasicAttack senseiBasicAttack = targetAbility as SenseiBasicAttack;
		
		bool isValid = senseiBasicAttack != null;
		string desc = string.Empty;
		desc += PropDesc(m_circleDistThresholdMod, "[CircleDistThreshold]", isValid, isValid ? senseiBasicAttack.m_circleDistThreshold : 0f);
		desc += PropDesc(m_circleRadiusMod, "[CircleRadius]", isValid, isValid ? senseiBasicAttack.m_circleRadius : 0f);
		desc += PropDesc(m_laserInfoMod, "[LaserInfo]", isValid, isValid ? senseiBasicAttack.m_laserInfo : null);
		desc += PropDesc(m_circleDamageMod, "[CircleDamage]", isValid, isValid ? senseiBasicAttack.m_circleDamage : 0);
		desc += PropDesc(m_circleEnemyHitEffectMod, "[CircleEnemyHitEffect]", isValid, isValid ? senseiBasicAttack.m_circleEnemyHitEffect : null);
		desc += PropDesc(m_laserDamageMod, "[LaserDamage]", isValid, isValid ? senseiBasicAttack.m_laserDamage : 0);
		desc += PropDesc(m_laserEnemyHitEffectMod, "[LaserEnemyHitEffect]", isValid, isValid ? senseiBasicAttack.m_laserEnemyHitEffect : null);
		desc += PropDesc(m_extraDamageForAlternatingMod, "[ExtraDamageForAlternating]", isValid, isValid ? senseiBasicAttack.m_extraDamageForAlternating : 0);
		desc += PropDesc(m_extraDamageForFarTargetMod, "[ExtraDamageForFarTarget]", isValid, isValid ? senseiBasicAttack.m_extraDamageForFarTarget : 0);
		desc += PropDesc(m_laserFarDistThreshMod, "[LaserFarDistThresh]", isValid, isValid ? senseiBasicAttack.m_laserFarDistThresh : 0f);
		desc += PropDesc(m_circleFarDistThreshMod, "[CircleFarDistThresh]", isValid, isValid ? senseiBasicAttack.m_circleFarDistThresh : 0f);
		desc += PropDesc(m_healPerEnemyHitMod, "[HealPerEnemyHit]", isValid, isValid ? senseiBasicAttack.m_healPerEnemyHit : 0);
		desc += PropDesc(m_cdrOnAbilityMod, "[CdrOnAbility]", isValid, isValid ? senseiBasicAttack.m_cdrOnAbility : 0);
		desc += PropDesc(m_cdrMinTriggerHitCountMod, "[CdrMinTriggerHitCount]", isValid, isValid ? senseiBasicAttack.m_cdrMinTriggerHitCount : 0);
		desc += PropDesc(m_absorbPerEnemyHitOnTurnStartMod, "[AbsorbPerEnemyHitOnTurnStart]", isValid, isValid ? senseiBasicAttack.m_absorbPerEnemyHitOnTurnStart : 0);
		return desc + PropDesc(m_absorbAmountIfTriggeredHitCountMod, "[AbsorbAmountIfTriggeredHitCount]", isValid, isValid ? senseiBasicAttack.m_absorbAmountIfTriggeredHitCount : 0);  // removed in rogues
	}
}
