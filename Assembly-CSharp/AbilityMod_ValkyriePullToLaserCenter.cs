using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_ValkyriePullToLaserCenter : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyFloat m_laserRangeInSquaresMod;
	public AbilityModPropertyInt m_maxTargetsMod;
	public AbilityModPropertyBool m_lengthIgnoreLosMod;
	[Header("-- Damage & effects")]
	public AbilityModPropertyInt m_damageMod;
	public AbilityModPropertyInt m_extraDamageIfKnockedInPlaceMod;
	public AbilityModPropertyEffectInfo m_effectToEnemiesMod;
	[Header("-- Extra Damage for Center")]
	public AbilityModPropertyInt m_extraDamageForCenterHitsMod;
	public AbilityModPropertyFloat m_centerHitWidthMod;
	[Header("-- Knockback on Cast")]
	public AbilityModPropertyFloat m_maxKnockbackDistMod;
	public AbilityModPropertyKnockbackType m_knockbackTypeMod;
	[Header("-- Misc ability interactions")]
	public AbilityModPropertyBool m_nextTurnStabSkipsDamageReduction;

	public override Type GetTargetAbilityType()
	{
		return typeof(ValkyriePullToLaserCenter);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ValkyriePullToLaserCenter valkyriePullToLaserCenter = targetAbility as ValkyriePullToLaserCenter;
		if (valkyriePullToLaserCenter != null)
		{
			AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, valkyriePullToLaserCenter.m_laserWidth);
			AddToken(tokens, m_laserRangeInSquaresMod, "LaserRangeInSquares", string.Empty, valkyriePullToLaserCenter.m_laserRangeInSquares);
			AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, valkyriePullToLaserCenter.m_maxTargets);
			AddToken(tokens, m_damageMod, "Damage", string.Empty, valkyriePullToLaserCenter.m_damage);
			AddToken(tokens, m_extraDamageIfKnockedInPlaceMod, "ExtraDamageIfKnockedInPlace", string.Empty, 0);
			AddToken_EffectMod(tokens, m_effectToEnemiesMod, "EffectToEnemies", valkyriePullToLaserCenter.m_effectToEnemies);
			AddToken(tokens, m_extraDamageForCenterHitsMod, "ExtraDamageForCenterHits", string.Empty, valkyriePullToLaserCenter.m_extraDamageForCenterHits);
			AddToken(tokens, m_centerHitWidthMod, "CenterHitWidth", string.Empty, valkyriePullToLaserCenter.m_centerHitWidth);
			AddToken(tokens, m_maxKnockbackDistMod, "MaxKnockbackDist", string.Empty, valkyriePullToLaserCenter.m_maxKnockbackDist);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ValkyriePullToLaserCenter valkyriePullToLaserCenter = GetTargetAbilityOnAbilityData(abilityData) as ValkyriePullToLaserCenter;
		bool isValid = valkyriePullToLaserCenter != null;
		string desc = string.Empty;
		desc += PropDesc(m_laserWidthMod, "[LaserWidth]", isValid, isValid ? valkyriePullToLaserCenter.m_laserWidth : 0f);
		desc += PropDesc(m_laserRangeInSquaresMod, "[LaserRangeInSquares]", isValid, isValid ? valkyriePullToLaserCenter.m_laserRangeInSquares : 0f);
		desc += PropDesc(m_maxTargetsMod, "[MaxTargets]", isValid, isValid ? valkyriePullToLaserCenter.m_maxTargets : 0);
		desc += PropDesc(m_lengthIgnoreLosMod, "[LengthIgnoreLos]", isValid, isValid && valkyriePullToLaserCenter.m_lengthIgnoreLos);
		desc += PropDesc(m_damageMod, "[Damage]", isValid, isValid ? valkyriePullToLaserCenter.m_damage : 0);
		desc += PropDesc(m_extraDamageIfKnockedInPlaceMod, "[ExtraDamageIfKnockedInPlace]", isValid);
		desc += PropDesc(m_effectToEnemiesMod, "[EffectToEnemies]", isValid, isValid ? valkyriePullToLaserCenter.m_effectToEnemies : null);
		desc += PropDesc(m_extraDamageForCenterHitsMod, "[ExtraDamageForCenterHits]", isValid, isValid ? valkyriePullToLaserCenter.m_extraDamageForCenterHits : 0);
		desc += PropDesc(m_centerHitWidthMod, "[CenterHitWidth]", isValid, isValid ? valkyriePullToLaserCenter.m_centerHitWidth : 0f);
		desc += PropDesc(m_maxKnockbackDistMod, "[MaxKnockbackDist]", isValid, isValid ? valkyriePullToLaserCenter.m_maxKnockbackDist : 0f);
		desc += PropDesc(m_knockbackTypeMod, "[KnockbackType]", isValid, isValid ? valkyriePullToLaserCenter.m_knockbackType : KnockbackType.AwayFromSource);
		return new StringBuilder().Append(desc).Append(PropDesc(m_nextTurnStabSkipsDamageReduction, "[NextTurnStabSkipsDamageReduction]", isValid)).ToString();
	}
}
