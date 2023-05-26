using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ClericAreaBuff : AbilityMod
{
	[Separator("Targeting")]
	public AbilityModPropertyShape m_shapeMod;
	public AbilityModPropertyBool m_penetrateLoSMod;
	public AbilityModPropertyBool m_includeEnemiesMod;
	public AbilityModPropertyBool m_includeAlliesMod;
	public AbilityModPropertyBool m_includeCasterMod;
	[Separator("Misc - Energy, Cooldown, Animation")]
	public AbilityModPropertyInt m_extraTpCostPerTurnActiveMod;
	public AbilityModPropertyInt m_cooldownWhenBuffLapsesMod;
	[Separator("On Hit Heal/Damage/Effect")]
	public AbilityModPropertyInt m_effectDurationMod;
	public AbilityModPropertyInt m_healAmountMod;
	public AbilityModPropertyEffectInfo m_effectOnCasterMod;
	public AbilityModPropertyEffectInfo m_effectOnAlliesMod;
	public AbilityModPropertyEffectInfo m_firstTurnOnlyEffectOnAlliesMod;
	[Header("-- Shielding on self override, if >= 0")]
	public AbilityModPropertyInt m_selfShieldingOverrideMod;
	public AbilityModPropertyEffectInfo m_effectOnEnemiesMod;
	public AbilityModPropertyInt m_extraSelfShieldingPerEnemyInShape;
	[Separator("Vision on Target Square")]
	public AbilityModPropertyBool m_addVisionOnTargetSquareMod;
	public AbilityModPropertyFloat m_visionRadiusMod;
	public AbilityModPropertyInt m_visionDurationMod;
	public AbilityModPropertyBool m_visionAreaIgnoreLosMod;
	[Separator("-- Per Turn Active")]
	public AbilityModPropertyInt m_extraShieldsPerTurnActive;
	public AbilityModPropertyInt m_allyTechPointGainPerTurnActive;
	[Header("-- Ability Interaction")]
	public AbilityModPropertyInt m_extraHealForPurifyOnBuffedAllies;

	public override Type GetTargetAbilityType()
	{
		return typeof(ClericAreaBuff);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ClericAreaBuff clericAreaBuff = targetAbility as ClericAreaBuff;
		if (clericAreaBuff != null)
		{
			AddToken(tokens, m_extraTpCostPerTurnActiveMod, "ExtraTpCostPerTurnActive", string.Empty, clericAreaBuff.m_extraTpCostPerTurnActive);
			AddToken(tokens, m_cooldownWhenBuffLapsesMod, "CooldownWhenBuffLapses", string.Empty, clericAreaBuff.m_cooldownWhenBuffLapses);
			AddToken(tokens, m_effectDurationMod, "EffectDuration", string.Empty, clericAreaBuff.m_effectDuration);
			AddToken(tokens, m_healAmountMod, "HealAmount", string.Empty, clericAreaBuff.m_healAmount);
			AddToken_EffectMod(tokens, m_effectOnCasterMod, "EffectOnCaster", clericAreaBuff.m_effectOnCaster);
			AddToken_EffectMod(tokens, m_effectOnAlliesMod, "EffectOnAllies", clericAreaBuff.m_effectOnAllies);
			AddToken_EffectMod(tokens, m_firstTurnOnlyEffectOnAlliesMod, "FirstTurnOnlyEffectOnAllies", clericAreaBuff.m_effectOnAllies);
			AddToken(tokens, m_selfShieldingOverrideMod, "SelfShieldingOverride", string.Empty, clericAreaBuff.m_selfShieldingOverride);
			AddToken_EffectMod(tokens, m_effectOnEnemiesMod, "EffectOnEnemies", clericAreaBuff.m_effectOnEnemies);
			AddToken(tokens, m_extraSelfShieldingPerEnemyInShape, "ExtraSelfShieldingPerEnemyInShape", string.Empty, 0);
			AddToken(tokens, m_visionRadiusMod, "VisionRadius", string.Empty, clericAreaBuff.m_visionRadius);
			AddToken(tokens, m_visionDurationMod, "VisionDuration", string.Empty, clericAreaBuff.m_visionDuration);
			AddToken(tokens, m_extraShieldsPerTurnActive, "ExtraShieldsPerTurnActive", string.Empty, 0);
			AddToken(tokens, m_allyTechPointGainPerTurnActive, "AllyEnergyGainPerTurnActive", string.Empty, 0);
			AddToken(tokens, m_extraHealForPurifyOnBuffedAllies, "ExtraHealForPurifyOnBuffedAllies", string.Empty, 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClericAreaBuff clericAreaBuff = GetTargetAbilityOnAbilityData(abilityData) as ClericAreaBuff;
		bool isValid = clericAreaBuff != null;
		string desc = string.Empty;
		desc += PropDesc(m_shapeMod, "[Shape]", isValid, isValid ? clericAreaBuff.m_shape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_penetrateLoSMod, "[PenetrateLoS]", isValid, isValid && clericAreaBuff.m_penetrateLoS);
		desc += PropDesc(m_includeEnemiesMod, "[IncludeEnemies]", isValid, isValid && clericAreaBuff.m_includeEnemies);
		desc += PropDesc(m_includeAlliesMod, "[IncludeAllies]", isValid, isValid && clericAreaBuff.m_includeAllies);
		desc += PropDesc(m_includeCasterMod, "[IncludeCaster]", isValid, isValid && clericAreaBuff.m_includeCaster);
		desc += PropDesc(m_extraTpCostPerTurnActiveMod, "[ExtraTpCostPerTurnActive]", isValid, isValid ? clericAreaBuff.m_extraTpCostPerTurnActive : 0);
		desc += PropDesc(m_cooldownWhenBuffLapsesMod, "[CooldownWhenBuffLapses]", isValid, isValid ? clericAreaBuff.m_cooldownWhenBuffLapses : 0);
		desc += PropDesc(m_effectDurationMod, "[EffectDuration]", isValid, isValid ? clericAreaBuff.m_effectDuration : 0);
		desc += PropDesc(m_healAmountMod, "[HealAmount]", isValid, isValid ? clericAreaBuff.m_healAmount : 0);
		desc += PropDesc(m_effectOnCasterMod, "[EffectOnCaster]", isValid, isValid ? clericAreaBuff.m_effectOnCaster : null);
		desc += PropDesc(m_effectOnAlliesMod, "[EffectOnAllies]", isValid, isValid ? clericAreaBuff.m_effectOnAllies : null);
		desc += PropDesc(m_firstTurnOnlyEffectOnAlliesMod, "[FirstTurnOnlyEffectOnAllies]", isValid, isValid ? clericAreaBuff.m_effectOnAllies : null);
		desc += PropDesc(m_selfShieldingOverrideMod, "[SelfShieldingOverride]", isValid, isValid ? clericAreaBuff.m_selfShieldingOverride : 0);
		desc += PropDesc(m_effectOnEnemiesMod, "[EffectOnEnemies]", isValid, isValid ? clericAreaBuff.m_effectOnEnemies : null);
		desc += PropDesc(m_extraSelfShieldingPerEnemyInShape, "[ExtraSelfShieldingPerEnemyInShape]", isValid);
		desc += PropDesc(m_addVisionOnTargetSquareMod, "[AddVisionOnTargetSquare]", isValid, isValid && clericAreaBuff.m_addVisionOnTargetSquare);
		desc += PropDesc(m_visionRadiusMod, "[VisionRadius]", isValid, isValid ? clericAreaBuff.m_visionRadius : 0f);
		desc += PropDesc(m_visionDurationMod, "[VisionDuration]", isValid, isValid ? clericAreaBuff.m_visionDuration : 0);
		desc += PropDesc(m_visionAreaIgnoreLosMod, "[VisionAreaIgnoreLos]", isValid, isValid && clericAreaBuff.m_visionAreaIgnoreLos);
		desc += PropDesc(m_extraShieldsPerTurnActive, "[ExtraShieldsPerTurnActive]", isValid);
		desc += PropDesc(m_allyTechPointGainPerTurnActive, "[AllyEnergyGainPerTurnActive]", isValid);
		return desc + PropDesc(m_extraHealForPurifyOnBuffedAllies, "[ExtraHealForPurifyOnBuffedAllies]", isValid);
	}
}
