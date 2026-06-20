// ROGUES
// SERVER
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
	[Separator("Aim Shield and Cone")]
	public AbilityModPropertyFloat m_coneWidthAngleMod;
	public AbilityModPropertyFloat m_coneRadiusMod;
	public AbilityModPropertyInt m_coverDurationMod;
	[Header("-- Cover Ignore Min Dist?")]
	public AbilityModPropertyBool m_coverIgnoreMinDistMod;
	[Header("-- Whether to put guard ability on cooldown")]
	public AbilityModPropertyBool m_triggerCooldownOnGuardAbiityMod;
	[Separator("Enemy hits")]
	public AbilityModPropertyInt m_damageMod;
	public AbilityModPropertyEffectInfo m_enemyDebuffMod;
	[Separator("Ally & self hits")]
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
			AddToken_EffectMod(tokens, m_shieldEffectInfoMod, "ShieldEffectInfo", valkyrieDashAoE.m_shieldEffectInfo);
			AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, valkyrieDashAoE.m_coneWidthAngle);
			AddToken(tokens, m_coneRadiusMod, "ConeRadius", string.Empty, valkyrieDashAoE.m_coneRadius);
			AddToken(tokens, m_damageMod, "Damage", string.Empty, valkyrieDashAoE.m_damage);
			AddToken_EffectMod(tokens, m_enemyDebuffMod, "EnemyDebuff", valkyrieDashAoE.m_enemyDebuff);
			AddToken(tokens, m_absorbMod, "Absorb", string.Empty, valkyrieDashAoE.m_absorb);
			AddToken_EffectMod(tokens, m_allyBuffMod, "AllyBuff", valkyrieDashAoE.m_allyBuff);
			AddToken_EffectMod(tokens, m_selfBuffMod, "SelfBuff", valkyrieDashAoE.m_selfBuff);
			AddToken(tokens, m_techPointGainPerCoveredHitMod, "TechPointGainPerCoveredHit", string.Empty, valkyrieDashAoE.m_techPointGainPerCoveredHit);
			AddToken(tokens, m_techPointGainPerTooCloseForCoverHitMod, "TechPointGainPerTooCloseForCoverHit", string.Empty, valkyrieDashAoE.m_techPointGainPerTooCloseForCoverHit);
			AddToken(tokens, m_cooldownReductionIfDamagedThisTurnMod, "CooldownReductionIfDamagedThisTurn", string.Empty, valkyrieDashAoE.m_cooldownReductionIfDamagedThisTurn.cooldownAddAmount);
			AddToken_IntDiff(tokens, "CoverDuration_Final", string.Empty, m_coverDurationMod.GetModifiedValue(valkyrieDashAoE.m_coverDuration) - 1, false, 0);
			AddToken(tokens, m_coverDurationMod, "CoverDuration_Alt", string.Empty, valkyrieDashAoE.m_coverDuration);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData) // , Ability targetAbility in rogues
	{
		// reactor
		ValkyrieDashAoE valkyrieDashAoE = GetTargetAbilityOnAbilityData(abilityData) as ValkyrieDashAoE;
		// rogues
		// ValkyrieDashAoE valkyrieDashAoE = targetAbility as ValkyrieDashAoE;
		bool isValid = valkyrieDashAoE != null;
		string desc = string.Empty;
		desc += PropDesc(m_shieldEffectInfoMod, "[ShieldEffectInfo]", isValid, isValid ? valkyrieDashAoE.m_shieldEffectInfo : null);
		desc += PropDesc(m_aoeShapeMod, "[AoeShape]", isValid, isValid ? valkyrieDashAoE.m_aoeShape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_aoePenetratesLoSMod, "[AoePenetratesLoS]", isValid, isValid && valkyrieDashAoE.m_aoePenetratesLoS);
		desc += PropDesc(m_coneWidthAngleMod, "[ConeWidthAngle]", isValid, isValid ? valkyrieDashAoE.m_coneWidthAngle : 0f);
		desc += PropDesc(m_coneRadiusMod, "[ConeRadius]", isValid, isValid ? valkyrieDashAoE.m_coneRadius : 0f);
		desc += PropDesc(m_triggerCooldownOnGuardAbiityMod, "[TriggerCooldownOnGuardAbiity]", isValid, isValid && valkyrieDashAoE.m_triggerCooldownOnGuardAbiity);
		desc += PropDesc(m_damageMod, "[Damage]", isValid, isValid ? valkyrieDashAoE.m_damage : 0);
		desc += PropDesc(m_enemyDebuffMod, "[EnemyDebuff]", isValid, isValid ? valkyrieDashAoE.m_enemyDebuff : null);
		desc += PropDesc(m_absorbMod, "[Absorb]", isValid, isValid ? valkyrieDashAoE.m_absorb : 0);
		desc += PropDesc(m_allyBuffMod, "[AllyBuff]", isValid, isValid ? valkyrieDashAoE.m_allyBuff : null);
		desc += PropDesc(m_selfBuffMod, "[SelfBuff]", isValid, isValid ? valkyrieDashAoE.m_selfBuff : null);
		desc += PropDesc(m_techPointGainPerCoveredHitMod, "[TechPointGainPerCoveredHit]", isValid, isValid ? valkyrieDashAoE.m_techPointGainPerCoveredHit : 0);
		desc += PropDesc(m_techPointGainPerTooCloseForCoverHitMod, "[TechPointGainPerTooCloseForCoverHit]", isValid, isValid ? valkyrieDashAoE.m_techPointGainPerTooCloseForCoverHit : 0);
		desc += PropDesc(m_cooldownReductionIfDamagedThisTurnMod, "[CooldownReductionIfDamagedThisTurn]", isValid, isValid ? valkyrieDashAoE.m_cooldownReductionIfDamagedThisTurn.cooldownAddAmount : 0);
		desc += PropDesc(m_coverDurationMod, "[CoverDuration]", isValid, isValid ? valkyrieDashAoE.m_coverDuration : 0);
		return desc + PropDesc(m_coverIgnoreMinDistMod, "[CoverIgnoreMinDist]", isValid, isValid && valkyrieDashAoE.m_coverIgnoreMinDist);
	}
}
