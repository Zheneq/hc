using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ClericAreaBuff : AbilityMod
{
	[Separator("Targeting", true)]
	public AbilityModPropertyShape m_shapeMod;

	public AbilityModPropertyBool m_penetrateLoSMod;

	public AbilityModPropertyBool m_includeEnemiesMod;

	public AbilityModPropertyBool m_includeAlliesMod;

	public AbilityModPropertyBool m_includeCasterMod;

	[Separator("Misc - Energy, Cooldown, Animation", true)]
	public AbilityModPropertyInt m_extraTpCostPerTurnActiveMod;

	public AbilityModPropertyInt m_cooldownWhenBuffLapsesMod;

	[Separator("On Hit Heal/Damage/Effect", true)]
	public AbilityModPropertyInt m_effectDurationMod;

	public AbilityModPropertyInt m_healAmountMod;

	public AbilityModPropertyEffectInfo m_effectOnCasterMod;

	public AbilityModPropertyEffectInfo m_effectOnAlliesMod;

	public AbilityModPropertyEffectInfo m_firstTurnOnlyEffectOnAlliesMod;

	[Header("-- Shielding on self override, if >= 0")]
	public AbilityModPropertyInt m_selfShieldingOverrideMod;

	public AbilityModPropertyEffectInfo m_effectOnEnemiesMod;

	public AbilityModPropertyInt m_extraSelfShieldingPerEnemyInShape;

	[Separator("Vision on Target Square", true)]
	public AbilityModPropertyBool m_addVisionOnTargetSquareMod;

	public AbilityModPropertyFloat m_visionRadiusMod;

	public AbilityModPropertyInt m_visionDurationMod;

	public AbilityModPropertyBool m_visionAreaIgnoreLosMod;

	[Separator("-- Per Turn Active", true)]
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
			AbilityMod.AddToken(tokens, m_extraTpCostPerTurnActiveMod, "ExtraTpCostPerTurnActive", string.Empty, clericAreaBuff.m_extraTpCostPerTurnActive);
			AbilityMod.AddToken(tokens, m_cooldownWhenBuffLapsesMod, "CooldownWhenBuffLapses", string.Empty, clericAreaBuff.m_cooldownWhenBuffLapses);
			AbilityMod.AddToken(tokens, m_effectDurationMod, "EffectDuration", string.Empty, clericAreaBuff.m_effectDuration);
			AbilityMod.AddToken(tokens, m_healAmountMod, "HealAmount", string.Empty, clericAreaBuff.m_healAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnCasterMod, "EffectOnCaster", clericAreaBuff.m_effectOnCaster);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnAlliesMod, "EffectOnAllies", clericAreaBuff.m_effectOnAllies);
			AbilityMod.AddToken_EffectMod(tokens, m_firstTurnOnlyEffectOnAlliesMod, "FirstTurnOnlyEffectOnAllies", clericAreaBuff.m_effectOnAllies);
			AbilityMod.AddToken(tokens, m_selfShieldingOverrideMod, "SelfShieldingOverride", string.Empty, clericAreaBuff.m_selfShieldingOverride);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnEnemiesMod, "EffectOnEnemies", clericAreaBuff.m_effectOnEnemies);
			AbilityMod.AddToken(tokens, m_extraSelfShieldingPerEnemyInShape, "ExtraSelfShieldingPerEnemyInShape", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_visionRadiusMod, "VisionRadius", string.Empty, clericAreaBuff.m_visionRadius);
			AbilityMod.AddToken(tokens, m_visionDurationMod, "VisionDuration", string.Empty, clericAreaBuff.m_visionDuration);
			AbilityMod.AddToken(tokens, m_extraShieldsPerTurnActive, "ExtraShieldsPerTurnActive", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_allyTechPointGainPerTurnActive, "AllyEnergyGainPerTurnActive", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_extraHealForPurifyOnBuffedAllies, "ExtraHealForPurifyOnBuffedAllies", string.Empty, 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClericAreaBuff clericAreaBuff = GetTargetAbilityOnAbilityData(abilityData) as ClericAreaBuff;
		bool flag = clericAreaBuff != null;
		string empty = string.Empty;
		empty += PropDesc(m_shapeMod, "[Shape]", flag, flag ? clericAreaBuff.m_shape : AbilityAreaShape.SingleSquare);
		string str = empty;
		AbilityModPropertyBool penetrateLoSMod = m_penetrateLoSMod;
		int baseVal;
		if (flag)
		{
			baseVal = (clericAreaBuff.m_penetrateLoS ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(penetrateLoSMod, "[PenetrateLoS]", flag, (byte)baseVal != 0);
		string str2 = empty;
		AbilityModPropertyBool includeEnemiesMod = m_includeEnemiesMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = (clericAreaBuff.m_includeEnemies ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(includeEnemiesMod, "[IncludeEnemies]", flag, (byte)baseVal2 != 0);
		string str3 = empty;
		AbilityModPropertyBool includeAlliesMod = m_includeAlliesMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = (clericAreaBuff.m_includeAllies ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(includeAlliesMod, "[IncludeAllies]", flag, (byte)baseVal3 != 0);
		empty += PropDesc(m_includeCasterMod, "[IncludeCaster]", flag, flag && clericAreaBuff.m_includeCaster);
		string str4 = empty;
		AbilityModPropertyInt extraTpCostPerTurnActiveMod = m_extraTpCostPerTurnActiveMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = clericAreaBuff.m_extraTpCostPerTurnActive;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(extraTpCostPerTurnActiveMod, "[ExtraTpCostPerTurnActive]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyInt cooldownWhenBuffLapsesMod = m_cooldownWhenBuffLapsesMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = clericAreaBuff.m_cooldownWhenBuffLapses;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(cooldownWhenBuffLapsesMod, "[CooldownWhenBuffLapses]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyInt effectDurationMod = m_effectDurationMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = clericAreaBuff.m_effectDuration;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(effectDurationMod, "[EffectDuration]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyInt healAmountMod = m_healAmountMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = clericAreaBuff.m_healAmount;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(healAmountMod, "[HealAmount]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyEffectInfo effectOnCasterMod = m_effectOnCasterMod;
		object baseVal8;
		if (flag)
		{
			baseVal8 = clericAreaBuff.m_effectOnCaster;
		}
		else
		{
			baseVal8 = null;
		}
		empty = str8 + PropDesc(effectOnCasterMod, "[EffectOnCaster]", flag, (StandardEffectInfo)baseVal8);
		string str9 = empty;
		AbilityModPropertyEffectInfo effectOnAlliesMod = m_effectOnAlliesMod;
		object baseVal9;
		if (flag)
		{
			baseVal9 = clericAreaBuff.m_effectOnAllies;
		}
		else
		{
			baseVal9 = null;
		}
		empty = str9 + PropDesc(effectOnAlliesMod, "[EffectOnAllies]", flag, (StandardEffectInfo)baseVal9);
		string str10 = empty;
		AbilityModPropertyEffectInfo firstTurnOnlyEffectOnAlliesMod = m_firstTurnOnlyEffectOnAlliesMod;
		object baseVal10;
		if (flag)
		{
			baseVal10 = clericAreaBuff.m_effectOnAllies;
		}
		else
		{
			baseVal10 = null;
		}
		empty = str10 + PropDesc(firstTurnOnlyEffectOnAlliesMod, "[FirstTurnOnlyEffectOnAllies]", flag, (StandardEffectInfo)baseVal10);
		empty += PropDesc(m_selfShieldingOverrideMod, "[SelfShieldingOverride]", flag, flag ? clericAreaBuff.m_selfShieldingOverride : 0);
		string str11 = empty;
		AbilityModPropertyEffectInfo effectOnEnemiesMod = m_effectOnEnemiesMod;
		object baseVal11;
		if (flag)
		{
			baseVal11 = clericAreaBuff.m_effectOnEnemies;
		}
		else
		{
			baseVal11 = null;
		}
		empty = str11 + PropDesc(effectOnEnemiesMod, "[EffectOnEnemies]", flag, (StandardEffectInfo)baseVal11);
		empty += PropDesc(m_extraSelfShieldingPerEnemyInShape, "[ExtraSelfShieldingPerEnemyInShape]", flag);
		string str12 = empty;
		AbilityModPropertyBool addVisionOnTargetSquareMod = m_addVisionOnTargetSquareMod;
		int baseVal12;
		if (flag)
		{
			baseVal12 = (clericAreaBuff.m_addVisionOnTargetSquare ? 1 : 0);
		}
		else
		{
			baseVal12 = 0;
		}
		empty = str12 + PropDesc(addVisionOnTargetSquareMod, "[AddVisionOnTargetSquare]", flag, (byte)baseVal12 != 0);
		string str13 = empty;
		AbilityModPropertyFloat visionRadiusMod = m_visionRadiusMod;
		float baseVal13;
		if (flag)
		{
			baseVal13 = clericAreaBuff.m_visionRadius;
		}
		else
		{
			baseVal13 = 0f;
		}
		empty = str13 + PropDesc(visionRadiusMod, "[VisionRadius]", flag, baseVal13);
		string str14 = empty;
		AbilityModPropertyInt visionDurationMod = m_visionDurationMod;
		int baseVal14;
		if (flag)
		{
			baseVal14 = clericAreaBuff.m_visionDuration;
		}
		else
		{
			baseVal14 = 0;
		}
		empty = str14 + PropDesc(visionDurationMod, "[VisionDuration]", flag, baseVal14);
		empty += PropDesc(m_visionAreaIgnoreLosMod, "[VisionAreaIgnoreLos]", flag, flag && clericAreaBuff.m_visionAreaIgnoreLos);
		empty += PropDesc(m_extraShieldsPerTurnActive, "[ExtraShieldsPerTurnActive]", flag);
		empty += PropDesc(m_allyTechPointGainPerTurnActive, "[AllyEnergyGainPerTurnActive]", flag);
		return empty + PropDesc(m_extraHealForPurifyOnBuffedAllies, "[ExtraHealForPurifyOnBuffedAllies]", flag);
	}
}
