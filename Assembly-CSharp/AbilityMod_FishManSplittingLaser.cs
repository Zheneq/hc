using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_FishManSplittingLaser : AbilityMod
{
	[Header("-- Primary Laser")]
	public AbilityModPropertyBool m_primaryLaserCanHitEnemiesMod;
	public AbilityModPropertyBool m_primaryLaserCanHitAlliesMod;
	public AbilityModPropertyInt m_primaryTargetDamageAmountMod;
	public AbilityModPropertyInt m_primaryTargetHealingAmountMod;
	public AbilityModPropertyEffectInfo m_primaryTargetEnemyHitEffectMod;
	public AbilityModPropertyEffectInfo m_primaryTargetAllyHitEffectMod;
	public AbilityModPropertyLaserInfo m_primaryTargetingInfoMod;
	[Header("-- Secondary Lasers")]
	public AbilityModPropertyBool m_secondaryLasersCanHitEnemiesMod;
	public AbilityModPropertyBool m_secondaryLasersCanHitAlliesMod;
	public AbilityModPropertyInt m_secondaryTargetDamageAmountMod;
	public AbilityModPropertyInt m_secondaryTargetHealingAmountMod;
	public AbilityModPropertyEffectInfo m_secondaryTargetEnemyHitEffectMod;
	public AbilityModPropertyEffectInfo m_secondaryTargetAllyHitEffectMod;
	public AbilityModPropertyLaserInfo m_secondaryTargetingInfoMod;
	[Header("-- Split Data")]
	public AbilityModPropertyBool m_alwaysSplitMod;
	public AbilityModPropertyFloat m_minSplitAngleMod;
	public AbilityModPropertyFloat m_maxSplitAngleMod;
	public AbilityModPropertyFloat m_lengthForMinAngleMod;
	public AbilityModPropertyFloat m_lengthForMaxAngleMod;
	public AbilityModPropertyInt m_numSplitBeamPairsMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(FishManSplittingLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FishManSplittingLaser fishManSplittingLaser = targetAbility as FishManSplittingLaser;
		if (fishManSplittingLaser != null)
		{
			AddToken(tokens, m_primaryTargetDamageAmountMod, "PrimaryTargetDamageAmount", string.Empty, fishManSplittingLaser.m_primaryTargetDamageAmount);
			AddToken(tokens, m_primaryTargetHealingAmountMod, "PrimaryTargetHealingAmount", string.Empty, fishManSplittingLaser.m_primaryTargetHealingAmount);
			AddToken_EffectMod(tokens, m_primaryTargetEnemyHitEffectMod, "PrimaryTargetEnemyHitEffect", fishManSplittingLaser.m_primaryTargetEnemyHitEffect);
			AddToken_EffectMod(tokens, m_primaryTargetAllyHitEffectMod, "PrimaryTargetAllyHitEffect", fishManSplittingLaser.m_primaryTargetAllyHitEffect);
			AddToken_LaserInfo(tokens, m_primaryTargetingInfoMod, "PrimaryTargetingInfo", fishManSplittingLaser.m_primaryTargetingInfo);
			AddToken(tokens, m_secondaryTargetDamageAmountMod, "SecondaryTargetDamageAmount", string.Empty, fishManSplittingLaser.m_secondaryTargetDamageAmount);
			AddToken(tokens, m_secondaryTargetHealingAmountMod, "SecondaryTargetHealingAmount", string.Empty, fishManSplittingLaser.m_secondaryTargetHealingAmount);
			AddToken_EffectMod(tokens, m_secondaryTargetEnemyHitEffectMod, "SecondaryTargetEnemyHitEffect", fishManSplittingLaser.m_secondaryTargetEnemyHitEffect);
			AddToken_EffectMod(tokens, m_secondaryTargetAllyHitEffectMod, "SecondaryTargetAllyHitEffect", fishManSplittingLaser.m_secondaryTargetAllyHitEffect);
			AddToken_LaserInfo(tokens, m_secondaryTargetingInfoMod, "SecondaryTargetingInfo", fishManSplittingLaser.m_secondaryTargetingInfo);
			AddToken(tokens, m_minSplitAngleMod, "MinSplitAngle", string.Empty, fishManSplittingLaser.m_minSplitAngle);
			AddToken(tokens, m_maxSplitAngleMod, "MaxSplitAngle", string.Empty, fishManSplittingLaser.m_maxSplitAngle);
			AddToken(tokens, m_lengthForMinAngleMod, "LengthForMinAngle", string.Empty, fishManSplittingLaser.m_lengthForMinAngle);
			AddToken(tokens, m_lengthForMaxAngleMod, "LengthForMaxAngle", string.Empty, fishManSplittingLaser.m_lengthForMaxAngle);
			AddToken(tokens, m_numSplitBeamPairsMod, "NumSplitBeamPairs", string.Empty, fishManSplittingLaser.m_numSplitBeamPairs);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManSplittingLaser fishManSplittingLaser = GetTargetAbilityOnAbilityData(abilityData) as FishManSplittingLaser;
		bool isValid = fishManSplittingLaser != null;
		string desc = string.Empty;
		desc += PropDesc(m_primaryLaserCanHitEnemiesMod, "[PrimaryLaserCanHitEnemies]", isValid, isValid && fishManSplittingLaser.m_primaryLaserCanHitEnemies);
		desc += PropDesc(m_primaryLaserCanHitAlliesMod, "[PrimaryLaserCanHitAllies]", isValid, isValid && fishManSplittingLaser.m_primaryLaserCanHitAllies);
		desc += PropDesc(m_primaryTargetDamageAmountMod, "[PrimaryTargetDamageAmount]", isValid, isValid ? fishManSplittingLaser.m_primaryTargetDamageAmount : 0);
		desc += PropDesc(m_primaryTargetHealingAmountMod, "[PrimaryTargetHealingAmount]", isValid, isValid ? fishManSplittingLaser.m_primaryTargetHealingAmount : 0);
		desc += PropDesc(m_primaryTargetEnemyHitEffectMod, "[PrimaryTargetEnemyHitEffect]", isValid, isValid ? fishManSplittingLaser.m_primaryTargetEnemyHitEffect : null);
		desc += PropDesc(m_primaryTargetAllyHitEffectMod, "[PrimaryTargetAllyHitEffect]", isValid, isValid ? fishManSplittingLaser.m_primaryTargetAllyHitEffect : null);
		desc += PropDesc(m_primaryTargetingInfoMod, "[PrimaryTargetingInfo]", isValid, isValid ? fishManSplittingLaser.m_primaryTargetingInfo : null);
		desc += PropDesc(m_secondaryLasersCanHitEnemiesMod, "[SecondaryLasersCanHitEnemies]", isValid, isValid && fishManSplittingLaser.m_secondaryLasersCanHitEnemies);
		desc += PropDesc(m_secondaryLasersCanHitAlliesMod, "[SecondaryLasersCanHitAllies]", isValid, isValid && fishManSplittingLaser.m_secondaryLasersCanHitAllies);
		desc += PropDesc(m_secondaryTargetDamageAmountMod, "[SecondaryTargetDamageAmount]", isValid, isValid ? fishManSplittingLaser.m_secondaryTargetDamageAmount : 0);
		desc += PropDesc(m_secondaryTargetHealingAmountMod, "[SecondaryTargetHealingAmount]", isValid, isValid ? fishManSplittingLaser.m_secondaryTargetHealingAmount : 0);
		desc += PropDesc(m_secondaryTargetEnemyHitEffectMod, "[SecondaryTargetEnemyHitEffect]", isValid, isValid ? fishManSplittingLaser.m_secondaryTargetEnemyHitEffect : null);
		desc += PropDesc(m_secondaryTargetAllyHitEffectMod, "[SecondaryTargetAllyHitEffect]", isValid, isValid ? fishManSplittingLaser.m_secondaryTargetAllyHitEffect : null);
		desc += PropDesc(m_secondaryTargetingInfoMod, "[SecondaryTargetingInfo]", isValid, isValid ? fishManSplittingLaser.m_secondaryTargetingInfo : null);
		desc += PropDesc(m_alwaysSplitMod, "[AlwaysSplit]", isValid, isValid && fishManSplittingLaser.m_alwaysSplit);
		desc += PropDesc(m_minSplitAngleMod, "[MinSplitAngle]", isValid, isValid ? fishManSplittingLaser.m_minSplitAngle : 0f);
		desc += PropDesc(m_maxSplitAngleMod, "[MaxSplitAngle]", isValid, isValid ? fishManSplittingLaser.m_maxSplitAngle : 0f);
		desc += PropDesc(m_lengthForMinAngleMod, "[LengthForMinAngle]", isValid, isValid ? fishManSplittingLaser.m_lengthForMinAngle : 0f);
		desc += PropDesc(m_lengthForMaxAngleMod, "[LengthForMaxAngle]", isValid, isValid ? fishManSplittingLaser.m_lengthForMaxAngle : 0f);
		return desc + PropDesc(m_numSplitBeamPairsMod, "[NumSplitBeamPairs]", isValid, isValid ? fishManSplittingLaser.m_numSplitBeamPairs : 0);
	}
}
