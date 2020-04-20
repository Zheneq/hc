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
			AbilityMod.AddToken(tokens, this.m_extraTpCostPerTurnActiveMod, "ExtraTpCostPerTurnActive", string.Empty, clericAreaBuff.m_extraTpCostPerTurnActive, true, false);
			AbilityMod.AddToken(tokens, this.m_cooldownWhenBuffLapsesMod, "CooldownWhenBuffLapses", string.Empty, clericAreaBuff.m_cooldownWhenBuffLapses, true, false);
			AbilityMod.AddToken(tokens, this.m_effectDurationMod, "EffectDuration", string.Empty, clericAreaBuff.m_effectDuration, true, false);
			AbilityMod.AddToken(tokens, this.m_healAmountMod, "HealAmount", string.Empty, clericAreaBuff.m_healAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnCasterMod, "EffectOnCaster", clericAreaBuff.m_effectOnCaster, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnAlliesMod, "EffectOnAllies", clericAreaBuff.m_effectOnAllies, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_firstTurnOnlyEffectOnAlliesMod, "FirstTurnOnlyEffectOnAllies", clericAreaBuff.m_effectOnAllies, true);
			AbilityMod.AddToken(tokens, this.m_selfShieldingOverrideMod, "SelfShieldingOverride", string.Empty, clericAreaBuff.m_selfShieldingOverride, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnEnemiesMod, "EffectOnEnemies", clericAreaBuff.m_effectOnEnemies, true);
			AbilityMod.AddToken(tokens, this.m_extraSelfShieldingPerEnemyInShape, "ExtraSelfShieldingPerEnemyInShape", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_visionRadiusMod, "VisionRadius", string.Empty, clericAreaBuff.m_visionRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_visionDurationMod, "VisionDuration", string.Empty, clericAreaBuff.m_visionDuration, true, false);
			AbilityMod.AddToken(tokens, this.m_extraShieldsPerTurnActive, "ExtraShieldsPerTurnActive", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_allyTechPointGainPerTurnActive, "AllyEnergyGainPerTurnActive", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_extraHealForPurifyOnBuffedAllies, "ExtraHealForPurifyOnBuffedAllies", string.Empty, 0, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClericAreaBuff clericAreaBuff = base.GetTargetAbilityOnAbilityData(abilityData) as ClericAreaBuff;
		bool flag = clericAreaBuff != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_shapeMod, "[Shape]", flag, (!flag) ? AbilityAreaShape.SingleSquare : clericAreaBuff.m_shape);
		string str = text;
		AbilityModPropertyBool penetrateLoSMod = this.m_penetrateLoSMod;
		string prefix = "[PenetrateLoS]";
		bool showBaseVal = flag;
		bool baseVal;
		if (flag)
		{
			baseVal = clericAreaBuff.m_penetrateLoS;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(penetrateLoSMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyBool includeEnemiesMod = this.m_includeEnemiesMod;
		string prefix2 = "[IncludeEnemies]";
		bool showBaseVal2 = flag;
		bool baseVal2;
		if (flag)
		{
			baseVal2 = clericAreaBuff.m_includeEnemies;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + base.PropDesc(includeEnemiesMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyBool includeAlliesMod = this.m_includeAlliesMod;
		string prefix3 = "[IncludeAllies]";
		bool showBaseVal3 = flag;
		bool baseVal3;
		if (flag)
		{
			baseVal3 = clericAreaBuff.m_includeAllies;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + base.PropDesc(includeAlliesMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_includeCasterMod, "[IncludeCaster]", flag, flag && clericAreaBuff.m_includeCaster);
		string str4 = text;
		AbilityModPropertyInt extraTpCostPerTurnActiveMod = this.m_extraTpCostPerTurnActiveMod;
		string prefix4 = "[ExtraTpCostPerTurnActive]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = clericAreaBuff.m_extraTpCostPerTurnActive;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(extraTpCostPerTurnActiveMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt cooldownWhenBuffLapsesMod = this.m_cooldownWhenBuffLapsesMod;
		string prefix5 = "[CooldownWhenBuffLapses]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = clericAreaBuff.m_cooldownWhenBuffLapses;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(cooldownWhenBuffLapsesMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt effectDurationMod = this.m_effectDurationMod;
		string prefix6 = "[EffectDuration]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = clericAreaBuff.m_effectDuration;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(effectDurationMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt healAmountMod = this.m_healAmountMod;
		string prefix7 = "[HealAmount]";
		bool showBaseVal7 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = clericAreaBuff.m_healAmount;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(healAmountMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyEffectInfo effectOnCasterMod = this.m_effectOnCasterMod;
		string prefix8 = "[EffectOnCaster]";
		bool showBaseVal8 = flag;
		StandardEffectInfo baseVal8;
		if (flag)
		{
			baseVal8 = clericAreaBuff.m_effectOnCaster;
		}
		else
		{
			baseVal8 = null;
		}
		text = str8 + base.PropDesc(effectOnCasterMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyEffectInfo effectOnAlliesMod = this.m_effectOnAlliesMod;
		string prefix9 = "[EffectOnAllies]";
		bool showBaseVal9 = flag;
		StandardEffectInfo baseVal9;
		if (flag)
		{
			baseVal9 = clericAreaBuff.m_effectOnAllies;
		}
		else
		{
			baseVal9 = null;
		}
		text = str9 + base.PropDesc(effectOnAlliesMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyEffectInfo firstTurnOnlyEffectOnAlliesMod = this.m_firstTurnOnlyEffectOnAlliesMod;
		string prefix10 = "[FirstTurnOnlyEffectOnAllies]";
		bool showBaseVal10 = flag;
		StandardEffectInfo baseVal10;
		if (flag)
		{
			baseVal10 = clericAreaBuff.m_effectOnAllies;
		}
		else
		{
			baseVal10 = null;
		}
		text = str10 + base.PropDesc(firstTurnOnlyEffectOnAlliesMod, prefix10, showBaseVal10, baseVal10);
		text += base.PropDesc(this.m_selfShieldingOverrideMod, "[SelfShieldingOverride]", flag, (!flag) ? 0 : clericAreaBuff.m_selfShieldingOverride);
		string str11 = text;
		AbilityModPropertyEffectInfo effectOnEnemiesMod = this.m_effectOnEnemiesMod;
		string prefix11 = "[EffectOnEnemies]";
		bool showBaseVal11 = flag;
		StandardEffectInfo baseVal11;
		if (flag)
		{
			baseVal11 = clericAreaBuff.m_effectOnEnemies;
		}
		else
		{
			baseVal11 = null;
		}
		text = str11 + base.PropDesc(effectOnEnemiesMod, prefix11, showBaseVal11, baseVal11);
		text += base.PropDesc(this.m_extraSelfShieldingPerEnemyInShape, "[ExtraSelfShieldingPerEnemyInShape]", flag, 0);
		string str12 = text;
		AbilityModPropertyBool addVisionOnTargetSquareMod = this.m_addVisionOnTargetSquareMod;
		string prefix12 = "[AddVisionOnTargetSquare]";
		bool showBaseVal12 = flag;
		bool baseVal12;
		if (flag)
		{
			baseVal12 = clericAreaBuff.m_addVisionOnTargetSquare;
		}
		else
		{
			baseVal12 = false;
		}
		text = str12 + base.PropDesc(addVisionOnTargetSquareMod, prefix12, showBaseVal12, baseVal12);
		string str13 = text;
		AbilityModPropertyFloat visionRadiusMod = this.m_visionRadiusMod;
		string prefix13 = "[VisionRadius]";
		bool showBaseVal13 = flag;
		float baseVal13;
		if (flag)
		{
			baseVal13 = clericAreaBuff.m_visionRadius;
		}
		else
		{
			baseVal13 = 0f;
		}
		text = str13 + base.PropDesc(visionRadiusMod, prefix13, showBaseVal13, baseVal13);
		string str14 = text;
		AbilityModPropertyInt visionDurationMod = this.m_visionDurationMod;
		string prefix14 = "[VisionDuration]";
		bool showBaseVal14 = flag;
		int baseVal14;
		if (flag)
		{
			baseVal14 = clericAreaBuff.m_visionDuration;
		}
		else
		{
			baseVal14 = 0;
		}
		text = str14 + base.PropDesc(visionDurationMod, prefix14, showBaseVal14, baseVal14);
		text += base.PropDesc(this.m_visionAreaIgnoreLosMod, "[VisionAreaIgnoreLos]", flag, flag && clericAreaBuff.m_visionAreaIgnoreLos);
		text += base.PropDesc(this.m_extraShieldsPerTurnActive, "[ExtraShieldsPerTurnActive]", flag, 0);
		text += base.PropDesc(this.m_allyTechPointGainPerTurnActive, "[AllyEnergyGainPerTurnActive]", flag, 0);
		return text + base.PropDesc(this.m_extraHealForPurifyOnBuffedAllies, "[ExtraHealForPurifyOnBuffedAllies]", flag, 0);
	}
}
