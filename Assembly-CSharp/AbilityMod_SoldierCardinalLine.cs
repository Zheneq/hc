using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SoldierCardinalLine : AbilityMod
{
	[Header("-- Targeting (shape for position targeter, line width for strafe hit area --")]
	public AbilityModPropertyBool m_useBothCardinalDirMod;

	public AbilityModPropertyShape m_positionShapeMod;

	public AbilityModPropertyFloat m_lineWidthMod;

	public AbilityModPropertyBool m_penetrateLosMod;

	[Header("-- On Hit Stuff --")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	[Header("-- Extra Damage for near center")]
	public AbilityModPropertyFloat m_nearCenterDistThresholdMod;

	public AbilityModPropertyInt m_extraDamageForNearCenterTargetsMod;

	[Header("-- AoE around targets --")]
	public AbilityModPropertyShape m_aoeShapeMod;

	public AbilityModPropertyInt m_aoeDamageMod;

	[Header("-- Subsequent Turn Hits --")]
	public AbilityModPropertyInt m_numSubsequentTurnsMod;

	public AbilityModPropertyInt m_damageOnSubsequentTurnsMod;

	public AbilityModPropertyEffectInfo m_enemyEffectOnSubsequentTurnsMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SoldierCardinalLine);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SoldierCardinalLine soldierCardinalLine = targetAbility as SoldierCardinalLine;
		if (soldierCardinalLine != null)
		{
			AbilityMod.AddToken(tokens, this.m_lineWidthMod, "LineWidth", string.Empty, soldierCardinalLine.m_lineWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, soldierCardinalLine.m_damageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", soldierCardinalLine.m_enemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_aoeDamageMod, "AoeDamage", string.Empty, soldierCardinalLine.m_aoeDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_nearCenterDistThresholdMod, "NearCenterDistThreshold", string.Empty, soldierCardinalLine.m_nearCenterDistThreshold, true, false, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageForNearCenterTargetsMod, "ExtraDamageForNearCenterTargets", string.Empty, soldierCardinalLine.m_extraDamageForNearCenterTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_numSubsequentTurnsMod, "NumSubsequentTurns", string.Empty, soldierCardinalLine.m_numSubsequentTurns, true, false);
			AbilityMod.AddToken(tokens, this.m_damageOnSubsequentTurnsMod, "DamageOnSubsequentTurns", string.Empty, soldierCardinalLine.m_damageOnSubsequentTurns, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyEffectOnSubsequentTurnsMod, "EnemyEffectOnSubsequentTurns", soldierCardinalLine.m_enemyEffectOnSubsequentTurns, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SoldierCardinalLine soldierCardinalLine = base.GetTargetAbilityOnAbilityData(abilityData) as SoldierCardinalLine;
		bool flag = soldierCardinalLine != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyBool useBothCardinalDirMod = this.m_useBothCardinalDirMod;
		string prefix = "[UseBothCardinalDir]";
		bool showBaseVal = flag;
		bool baseVal;
		if (flag)
		{
			baseVal = soldierCardinalLine.m_useBothCardinalDir;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(useBothCardinalDirMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyShape positionShapeMod = this.m_positionShapeMod;
		string prefix2 = "[PositionShape]";
		bool showBaseVal2 = flag;
		AbilityAreaShape baseVal2;
		if (flag)
		{
			baseVal2 = soldierCardinalLine.m_positionShape;
		}
		else
		{
			baseVal2 = AbilityAreaShape.SingleSquare;
		}
		text = str2 + base.PropDesc(positionShapeMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat lineWidthMod = this.m_lineWidthMod;
		string prefix3 = "[LineWidth]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = soldierCardinalLine.m_lineWidth;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(lineWidthMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_penetrateLosMod, "[PenetrateLos]", flag, flag && soldierCardinalLine.m_penetrateLos);
		string str4 = text;
		AbilityModPropertyInt damageAmountMod = this.m_damageAmountMod;
		string prefix4 = "[DamageAmount]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = soldierCardinalLine.m_damageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(damageAmountMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_enemyHitEffectMod, "[EnemyHitEffect]", flag, (!flag) ? null : soldierCardinalLine.m_enemyHitEffect);
		text += base.PropDesc(this.m_nearCenterDistThresholdMod, "[NearCenterDistThreshold]", flag, (!flag) ? 0f : soldierCardinalLine.m_nearCenterDistThreshold);
		string str5 = text;
		AbilityModPropertyInt extraDamageForNearCenterTargetsMod = this.m_extraDamageForNearCenterTargetsMod;
		string prefix5 = "[ExtraDamageForNearCenterTargets]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = soldierCardinalLine.m_extraDamageForNearCenterTargets;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(extraDamageForNearCenterTargetsMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyShape aoeShapeMod = this.m_aoeShapeMod;
		string prefix6 = "[AoeShape]";
		bool showBaseVal6 = flag;
		AbilityAreaShape baseVal6;
		if (flag)
		{
			baseVal6 = soldierCardinalLine.m_aoeShape;
		}
		else
		{
			baseVal6 = AbilityAreaShape.SingleSquare;
		}
		text = str6 + base.PropDesc(aoeShapeMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt aoeDamageMod = this.m_aoeDamageMod;
		string prefix7 = "[AoeDamage]";
		bool showBaseVal7 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = soldierCardinalLine.m_aoeDamage;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(aoeDamageMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyInt numSubsequentTurnsMod = this.m_numSubsequentTurnsMod;
		string prefix8 = "[NumSubsequentTurns]";
		bool showBaseVal8 = flag;
		int baseVal8;
		if (flag)
		{
			baseVal8 = soldierCardinalLine.m_numSubsequentTurns;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str8 + base.PropDesc(numSubsequentTurnsMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyInt damageOnSubsequentTurnsMod = this.m_damageOnSubsequentTurnsMod;
		string prefix9 = "[DamageOnSubsequentTurns]";
		bool showBaseVal9 = flag;
		int baseVal9;
		if (flag)
		{
			baseVal9 = soldierCardinalLine.m_damageOnSubsequentTurns;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(damageOnSubsequentTurnsMod, prefix9, showBaseVal9, baseVal9);
		return text + base.PropDesc(this.m_enemyEffectOnSubsequentTurnsMod, "[EnemyEffectOnSubsequentTurns]", flag, (!flag) ? null : soldierCardinalLine.m_enemyEffectOnSubsequentTurns);
	}
}
