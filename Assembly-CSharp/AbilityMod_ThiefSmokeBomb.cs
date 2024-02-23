using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_ThiefSmokeBomb : AbilityMod
{
	[Header("-- Bomb Damage")]
	public AbilityModPropertyInt m_extraDamageOnCastMod;
	[Header("-- Bomb Targeting (shape is in Smoke Field Info)")]
	public AbilityModPropertyBool m_penetrateLosMod;
	public AbilityModPropertyInt m_maxAngleWithFirstSegmentMod;
	public AbilityModPropertyFloat m_maxDistanceWithFirstMod;
	public AbilityModPropertyFloat m_minDistanceBetweenBombsMod;
	[Header("-- On Cast Hit Effect")]
	public AbilityModPropertyEffectInfo m_bombHitEffectInfoMod;
	[Header("-- Smoke Field")]
	public AbilityModPropertyGroundEffectField m_smokeFieldInfoMod;
	[Header("-- Barrier (will make square out of 4 barriers around ground field)")]
	public AbilityModPropertyBool m_addBarriersMod;
	public AbilityModPropertyFloat m_barrierSquareWidthMod;
	public AbilityModPropertyBarrierDataV2 m_barrierDataMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ThiefSmokeBomb);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ThiefSmokeBomb thiefSmokeBomb = targetAbility as ThiefSmokeBomb;
		if (thiefSmokeBomb != null)
		{
			AddToken(tokens, m_extraDamageOnCastMod, "ExtraDamageOnCast", string.Empty, thiefSmokeBomb.m_extraDamageOnCast);
			AddToken(tokens, m_maxAngleWithFirstSegmentMod, "MaxAngleWithFirstSegment", string.Empty, thiefSmokeBomb.m_maxAngleWithFirstSegment);
			AddToken(tokens, m_maxDistanceWithFirstMod, "MaxDistanceWithFirst", string.Empty, thiefSmokeBomb.m_maxDistanceWithFirst);
			AddToken(tokens, m_minDistanceBetweenBombsMod, "MinDistanceBetweenBombs", string.Empty, thiefSmokeBomb.m_minDistanceBetweenBombs);
			AddToken_EffectMod(tokens, m_bombHitEffectInfoMod, "BombHitEffectInfo", thiefSmokeBomb.m_bombHitEffectInfo);
			AddToken_GroundFieldMod(tokens, m_smokeFieldInfoMod, "SmokeFieldInfo", thiefSmokeBomb.m_smokeFieldInfo);
			AddToken(tokens, m_barrierSquareWidthMod, "BarrierSquareWidth", string.Empty, thiefSmokeBomb.m_barrierSquareWidth);
			AddToken_BarrierMod(tokens, m_barrierDataMod, "BarrierData", thiefSmokeBomb.m_barrierData);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ThiefSmokeBomb thiefSmokeBomb = GetTargetAbilityOnAbilityData(abilityData) as ThiefSmokeBomb;
		bool isValid = thiefSmokeBomb != null;
		string desc = string.Empty;
		desc += PropDesc(m_extraDamageOnCastMod, "[ExtraDamageonCast]", isValid, isValid ? thiefSmokeBomb.m_extraDamageOnCast : 0);
		desc += PropDesc(m_penetrateLosMod, "[PenetrateLos]", isValid, isValid && thiefSmokeBomb.m_penetrateLos);
		desc += PropDesc(m_maxAngleWithFirstSegmentMod, "[MaxAngleWithFirstSegment]", isValid, isValid ? thiefSmokeBomb.m_maxAngleWithFirstSegment : 0);
		desc += PropDesc(m_maxDistanceWithFirstMod, "[MaxDistanceWithFirst]", isValid, isValid ? thiefSmokeBomb.m_maxDistanceWithFirst : 0f);
		desc += PropDesc(m_minDistanceBetweenBombsMod, "[MinDistanceBetweenBombs]", isValid, isValid ? thiefSmokeBomb.m_minDistanceBetweenBombs : 0f);
		desc += PropDesc(m_bombHitEffectInfoMod, "[BombHitEffectInfo]", isValid, isValid ? thiefSmokeBomb.m_bombHitEffectInfo : null);
		desc += PropDescGroundFieldMod(m_smokeFieldInfoMod, "{ SmokeFieldInfo }", thiefSmokeBomb.m_smokeFieldInfo);
		desc += PropDesc(m_addBarriersMod, "[AddBarriers]", isValid, isValid && thiefSmokeBomb.m_addBarriers);
		desc += PropDesc(m_barrierSquareWidthMod, "[BarrierSquareWidth]", isValid, isValid ? thiefSmokeBomb.m_barrierSquareWidth : 0f);
		return new StringBuilder().Append(desc).Append(PropDescBarrierMod(m_barrierDataMod, "{ BarrierData }", thiefSmokeBomb.m_barrierData)).ToString();
	}
}
