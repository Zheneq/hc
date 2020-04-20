using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ValkyrieDashAoE : AbilityMod
{
	[Header("-- Shield effect")]
	public AbilityModPropertyEffectInfo m_shieldEffectInfoMod;

	public AbilityModPropertyInt m_techPointGainPerCoveredHitMod;

	public AbilityModPropertyInt m_techPointGainPerTooCloseForCoverHitMod;

	[Header("-- Targeting")]
	public AbilityModPropertyShape m_aoeShapeMod;

	public AbilityModPropertyBool m_aoePenetratesLoSMod;

	[Separator("Aim Shield and Cone", true)]
	public AbilityModPropertyFloat m_coneWidthAngleMod;

	public AbilityModPropertyFloat m_coneRadiusMod;

	public AbilityModPropertyInt m_coverDurationMod;

	[Header("-- Cover Ignore Min Dist?")]
	public AbilityModPropertyBool m_coverIgnoreMinDistMod;

	[Header("-- Whether to put guard ability on cooldown")]
	public AbilityModPropertyBool m_triggerCooldownOnGuardAbiityMod;

	[Separator("Enemy hits", true)]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyEffectInfo m_enemyDebuffMod;

	[Separator("Ally & self hits", true)]
	public AbilityModPropertyInt m_absorbMod;

	public AbilityModPropertyEffectInfo m_allyBuffMod;

	public AbilityModPropertyEffectInfo m_selfBuffMod;

	[Header("-- Cooldown reductions")]
	public AbilityModPropertyInt m_cooldownReductionIfDamagedThisTurnMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ValkyrieDashAoE);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ValkyrieDashAoE valkyrieDashAoE = targetAbility as ValkyrieDashAoE;
		if (valkyrieDashAoE != null)
		{
			AbilityMod.AddToken_EffectMod(tokens, this.m_shieldEffectInfoMod, "ShieldEffectInfo", valkyrieDashAoE.m_shieldEffectInfo, true);
			AbilityMod.AddToken(tokens, this.m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, valkyrieDashAoE.m_coneWidthAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneRadiusMod, "ConeRadius", string.Empty, valkyrieDashAoE.m_coneRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageMod, "Damage", string.Empty, valkyrieDashAoE.m_damage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyDebuffMod, "EnemyDebuff", valkyrieDashAoE.m_enemyDebuff, true);
			AbilityMod.AddToken(tokens, this.m_absorbMod, "Absorb", string.Empty, valkyrieDashAoE.m_absorb, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyBuffMod, "AllyBuff", valkyrieDashAoE.m_allyBuff, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_selfBuffMod, "SelfBuff", valkyrieDashAoE.m_selfBuff, true);
			AbilityMod.AddToken(tokens, this.m_techPointGainPerCoveredHitMod, "TechPointGainPerCoveredHit", string.Empty, valkyrieDashAoE.m_techPointGainPerCoveredHit, true, false);
			AbilityMod.AddToken(tokens, this.m_techPointGainPerTooCloseForCoverHitMod, "TechPointGainPerTooCloseForCoverHit", string.Empty, valkyrieDashAoE.m_techPointGainPerTooCloseForCoverHit, true, false);
			AbilityMod.AddToken(tokens, this.m_cooldownReductionIfDamagedThisTurnMod, "CooldownReductionIfDamagedThisTurn", string.Empty, valkyrieDashAoE.m_cooldownReductionIfDamagedThisTurn.cooldownAddAmount, true, false);
			AbilityMod.AddToken_IntDiff(tokens, "CoverDuration_Final", string.Empty, this.m_coverDurationMod.GetModifiedValue(valkyrieDashAoE.m_coverDuration) - 1, false, 0);
			AbilityMod.AddToken(tokens, this.m_coverDurationMod, "CoverDuration_Alt", string.Empty, valkyrieDashAoE.m_coverDuration, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ValkyrieDashAoE valkyrieDashAoE = base.GetTargetAbilityOnAbilityData(abilityData) as ValkyrieDashAoE;
		bool flag = valkyrieDashAoE != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyEffectInfo shieldEffectInfoMod = this.m_shieldEffectInfoMod;
		string prefix = "[ShieldEffectInfo]";
		bool showBaseVal = flag;
		StandardEffectInfo baseVal;
		if (flag)
		{
			baseVal = valkyrieDashAoE.m_shieldEffectInfo;
		}
		else
		{
			baseVal = null;
		}
		text = str + base.PropDesc(shieldEffectInfoMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyShape aoeShapeMod = this.m_aoeShapeMod;
		string prefix2 = "[AoeShape]";
		bool showBaseVal2 = flag;
		AbilityAreaShape baseVal2;
		if (flag)
		{
			baseVal2 = valkyrieDashAoE.m_aoeShape;
		}
		else
		{
			baseVal2 = AbilityAreaShape.SingleSquare;
		}
		text = str2 + base.PropDesc(aoeShapeMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_aoePenetratesLoSMod, "[AoePenetratesLoS]", flag, flag && valkyrieDashAoE.m_aoePenetratesLoS);
		string str3 = text;
		AbilityModPropertyFloat coneWidthAngleMod = this.m_coneWidthAngleMod;
		string prefix3 = "[ConeWidthAngle]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = valkyrieDashAoE.m_coneWidthAngle;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(coneWidthAngleMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat coneRadiusMod = this.m_coneRadiusMod;
		string prefix4 = "[ConeRadius]";
		bool showBaseVal4 = flag;
		float baseVal4;
		if (flag)
		{
			baseVal4 = valkyrieDashAoE.m_coneRadius;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(coneRadiusMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool triggerCooldownOnGuardAbiityMod = this.m_triggerCooldownOnGuardAbiityMod;
		string prefix5 = "[TriggerCooldownOnGuardAbiity]";
		bool showBaseVal5 = flag;
		bool baseVal5;
		if (flag)
		{
			baseVal5 = valkyrieDashAoE.m_triggerCooldownOnGuardAbiity;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(triggerCooldownOnGuardAbiityMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix6 = "[Damage]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = valkyrieDashAoE.m_damage;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(damageMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_enemyDebuffMod, "[EnemyDebuff]", flag, (!flag) ? null : valkyrieDashAoE.m_enemyDebuff);
		text += base.PropDesc(this.m_absorbMod, "[Absorb]", flag, (!flag) ? 0 : valkyrieDashAoE.m_absorb);
		string str7 = text;
		AbilityModPropertyEffectInfo allyBuffMod = this.m_allyBuffMod;
		string prefix7 = "[AllyBuff]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
		if (flag)
		{
			baseVal7 = valkyrieDashAoE.m_allyBuff;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + base.PropDesc(allyBuffMod, prefix7, showBaseVal7, baseVal7);
		text += base.PropDesc(this.m_selfBuffMod, "[SelfBuff]", flag, (!flag) ? null : valkyrieDashAoE.m_selfBuff);
		string str8 = text;
		AbilityModPropertyInt techPointGainPerCoveredHitMod = this.m_techPointGainPerCoveredHitMod;
		string prefix8 = "[TechPointGainPerCoveredHit]";
		bool showBaseVal8 = flag;
		int baseVal8;
		if (flag)
		{
			baseVal8 = valkyrieDashAoE.m_techPointGainPerCoveredHit;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str8 + base.PropDesc(techPointGainPerCoveredHitMod, prefix8, showBaseVal8, baseVal8);
		text += base.PropDesc(this.m_techPointGainPerTooCloseForCoverHitMod, "[TechPointGainPerTooCloseForCoverHit]", flag, (!flag) ? 0 : valkyrieDashAoE.m_techPointGainPerTooCloseForCoverHit);
		text += base.PropDesc(this.m_cooldownReductionIfDamagedThisTurnMod, "[CooldownReductionIfDamagedThisTurn]", flag, (!flag) ? 0 : valkyrieDashAoE.m_cooldownReductionIfDamagedThisTurn.cooldownAddAmount);
		string str9 = text;
		AbilityModPropertyInt coverDurationMod = this.m_coverDurationMod;
		string prefix9 = "[CoverDuration]";
		bool showBaseVal9 = flag;
		int baseVal9;
		if (flag)
		{
			baseVal9 = valkyrieDashAoE.m_coverDuration;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(coverDurationMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyBool coverIgnoreMinDistMod = this.m_coverIgnoreMinDistMod;
		string prefix10 = "[CoverIgnoreMinDist]";
		bool showBaseVal10 = flag;
		bool baseVal10;
		if (flag)
		{
			baseVal10 = valkyrieDashAoE.m_coverIgnoreMinDist;
		}
		else
		{
			baseVal10 = false;
		}
		return str10 + base.PropDesc(coverIgnoreMinDistMod, prefix10, showBaseVal10, baseVal10);
	}
}
