using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_TricksterCreateDamageFields : AbilityMod
{
	[Header("-- Targeting --")]
	public AbilityModPropertyBool m_addFieldAroundSelfMod;
	public AbilityModPropertyBool m_useInitialShapeOverrideMod;
	public AbilityModPropertyShape m_initialShapeOverrideMod;
	[Header("-- Ground Field Info --")]
	public AbilityModPropertyGroundEffectField m_groundFieldInfoMod;
	[Header("-- Self Effect for Multi Hit")]
	public AbilityModPropertyEffectInfo m_selfEffectForMultiHitMod;
	[Header("-- Extra Enemy Hit Effect On Cast")]
	public AbilityModPropertyEffectInfo m_extraEnemyEffectOnCastMod;
	[Header("-- Spoil spawn info")]
	public AbilityModPropertyBool m_spawnSpoilForEnemyHitMod;
	public AbilityModPropertyBool m_spawnSpoilForAllyHitMod;
	public AbilityModPropertyBool m_onlySpawnSpoilOnMultiHitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(TricksterCreateDamageFields);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		TricksterCreateDamageFields tricksterCreateDamageFields = targetAbility as TricksterCreateDamageFields;
		if (tricksterCreateDamageFields != null)
		{
			AddToken_GroundFieldMod(tokens, m_groundFieldInfoMod, "GroundFieldInfo", tricksterCreateDamageFields.m_groundFieldInfo);
			AddToken_EffectMod(tokens, m_selfEffectForMultiHitMod, "SelfEffectForMultiHit", tricksterCreateDamageFields.m_selfEffectForMultiHit);
			AddToken_EffectMod(tokens, m_extraEnemyEffectOnCastMod, "ExtraEnemyEffectOnCast", tricksterCreateDamageFields.m_extraEnemyEffectOnCast);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TricksterCreateDamageFields tricksterCreateDamageFields = GetTargetAbilityOnAbilityData(abilityData) as TricksterCreateDamageFields;
		bool isValid = tricksterCreateDamageFields != null;
		string desc = string.Empty;
		desc += PropDesc(m_addFieldAroundSelfMod, "[AddFieldAroundSelf]", isValid, isValid && tricksterCreateDamageFields.m_addFieldAroundSelf);
		bool useInitialShapeOverride = m_useInitialShapeOverrideMod != null
		             && m_useInitialShapeOverrideMod.GetModifiedValue(isValid && tricksterCreateDamageFields.m_useInitialShapeOverride);
		desc += PropDesc(m_useInitialShapeOverrideMod, "[UseInitialShapeOverride]", isValid, isValid && tricksterCreateDamageFields.m_useInitialShapeOverride);
		if (useInitialShapeOverride)
		{
			desc += PropDesc(m_initialShapeOverrideMod, "[InitialShapeOverride]", isValid, isValid ? tricksterCreateDamageFields.m_initialShapeOverride : AbilityAreaShape.SingleSquare);
		}
		desc += PropDescGroundFieldMod(m_groundFieldInfoMod, "{ GroundFieldInfo }", tricksterCreateDamageFields.m_groundFieldInfo);
		desc += PropDesc(m_selfEffectForMultiHitMod, "[SelfEffectForMultiHit]", isValid, isValid ? tricksterCreateDamageFields.m_selfEffectForMultiHit : null);
		desc += PropDesc(m_extraEnemyEffectOnCastMod, "[ExtraEnemyEffectOnCast]", isValid, isValid ? tricksterCreateDamageFields.m_extraEnemyEffectOnCast : null);
		desc += PropDesc(m_spawnSpoilForEnemyHitMod, "[SpawnSpoilForEnemyHit]", isValid, isValid && tricksterCreateDamageFields.m_spawnSpoilForEnemyHit);
		desc += PropDesc(m_spawnSpoilForAllyHitMod, "[SpawnSpoilForAllyHit]", isValid, isValid && tricksterCreateDamageFields.m_spawnSpoilForAllyHit);
		return desc + PropDesc(m_onlySpawnSpoilOnMultiHitMod, "[OnlySpawnSpoilOnMultiHit]", isValid, isValid && tricksterCreateDamageFields.m_onlySpawnSpoilOnMultiHit);
	}
}
