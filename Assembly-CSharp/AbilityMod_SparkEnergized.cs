using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SparkEnergized : AbilityMod
{
	public AbilityModPropertyEffectData m_allyBuffEffectMod;
	public AbilityModPropertyEffectData m_enemyDebuffEffectMod;
	public AbilityModPropertyInt m_healAmtPerBeamMod;
	[Header("-- Heal/Damage boost mod")]
	public AbilityModPropertyInt m_additionalHealMod;
	public AbilityModPropertyInt m_additionalDamageMod;
	[Header("-- Heal on Self (from tether), will apply regardless of whether Radiate is used")]
	public AbilityModPropertyInt m_healOnSelfFromTetherMod;
	public AbilityModPropertyInt m_energyOnSelfFromTetherMod;
	[Header("-- Need to Choose Target (if false, targeting all attached)")]
	public AbilityModPropertyBool m_needToChooseTargetMod;
	[Header("-- Effect on Enemy on start of Next Turn")]
	public StandardEffectInfo m_effectOnEnemyOnNextTurn;
	[Separator("Effect and Boosted Heal/Damage when both tethers are attached", true)]
	public AbilityModPropertyEffectInfo m_bothTetherExtraEffectOnSelfMod;
	public AbilityModPropertyEffectInfo m_bothTetherAllyEffectMod;
	public AbilityModPropertyEffectInfo m_bothTetherEnemyEffectMod;
	[Space(10f)]
	public AbilityModPropertyInt m_bothTetherExtraHealMod;
	public AbilityModPropertyInt m_bothTetherExtraDamageMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SparkEnergized);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SparkEnergized sparkEnergized = targetAbility as SparkEnergized;
		if (sparkEnergized != null)
		{
			SparkHealingBeam healingBeam = targetAbility.GetComponent<SparkHealingBeam>();
			SparkBasicAttack basicAttack = targetAbility.GetComponent<SparkBasicAttack>();
			AddToken_EffectMod(tokens, m_allyBuffEffectMod, "AllyBuffEffect", sparkEnergized.m_allyBuffEffect);
			AddToken_EffectMod(tokens, m_enemyDebuffEffectMod, "EnemyDebuffEffect", sparkEnergized.m_enemyDebuffEffect);
			AddToken(tokens, m_healAmtPerBeamMod, "HealAmtPerBeam", "", sparkEnergized.m_healAmtPerBeam);
			AddToken(tokens, m_additionalHealMod, "Heal_Additional", "additional heal", healingBeam ? healingBeam.m_additionalEnergizedHealing : 0, healingBeam != null);
			AddToken(tokens, m_additionalDamageMod, "Damage_Additional", "additional damage", basicAttack != null ? basicAttack.m_additionalEnergizedDamage : 0, basicAttack != null);
			if (healingBeam != null)
			{
				AddToken(tokens, m_healOnSelfFromTetherMod, "Heal_TetherOnSelf", "heal on self from tether", healingBeam != null ? healingBeam.m_healOnSelfOnTick : 0, healingBeam != null);
				AddToken(tokens, m_energyOnSelfFromTetherMod, "Energy_TetherOnSelf", "energy on self from tether", healingBeam != null ? healingBeam.m_energyOnCasterPerTurn : 0, healingBeam != null);
			}
			AddToken_EffectInfo(tokens, m_effectOnEnemyOnNextTurn, "EffectOnEnemyNextTurn");
			AddToken_EffectMod(tokens, m_bothTetherExtraEffectOnSelfMod, "BothTetherExtraEffectOnSelf", sparkEnergized.m_bothTetherExtraEffectOnSelf);
			AddToken_EffectMod(tokens, m_bothTetherAllyEffectMod, "BothTetherAllyEffect", sparkEnergized.m_bothTetherAllyEffect);
			AddToken_EffectMod(tokens, m_bothTetherEnemyEffectMod, "BothTetherEnemyEffect", sparkEnergized.m_bothTetherEnemyEffect);
			AddToken(tokens, m_bothTetherExtraHealMod, "BothTetherExtraHeal", "", sparkEnergized.m_bothTetherExtraHeal);
			AddToken(tokens, m_bothTetherExtraDamageMod, "BothTetherExtraDamage", "", sparkEnergized.m_bothTetherExtraDamage);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SparkEnergized sparkEnergized = GetTargetAbilityOnAbilityData(abilityData) as SparkEnergized;
		SparkHealingBeam sparkHealingBeam = sparkEnergized?.GetComponent<SparkHealingBeam>();
		SparkBasicAttack sparkBasicAttack = sparkEnergized?.GetComponent<SparkBasicAttack>();
		bool isEnergizedPresent = sparkEnergized != null;
		bool areBeamAndEnergizedPresent = isEnergizedPresent && sparkHealingBeam != null;
		bool areBasicAndEnergizedPresent = isEnergizedPresent && sparkBasicAttack != null;
		string desc = "";
		desc += PropDesc(m_allyBuffEffectMod, "[AllyBuffEffect]", isEnergizedPresent, isEnergizedPresent ? sparkEnergized.m_allyBuffEffect : null);
		desc += PropDesc(m_enemyDebuffEffectMod, "[EnemyDebuffEffect]", isEnergizedPresent, isEnergizedPresent ? sparkEnergized.m_enemyDebuffEffect : null);
		desc += PropDesc(m_healAmtPerBeamMod, "[HealAmtPerBeam]", isEnergizedPresent, isEnergizedPresent ? sparkEnergized.m_healAmtPerBeam : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_additionalHealMod, "[Additional Heal]", areBeamAndEnergizedPresent, areBeamAndEnergizedPresent ? sparkHealingBeam.m_additionalEnergizedHealing : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_additionalDamageMod, "[Additional Damage]", areBasicAndEnergizedPresent, areBasicAndEnergizedPresent ? sparkBasicAttack.m_additionalEnergizedDamage : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_healOnSelfFromTetherMod, "[Heal on Self from Tether]", isEnergizedPresent, areBeamAndEnergizedPresent ? sparkHealingBeam.m_healOnSelfOnTick : 0);
		desc += PropDesc(m_energyOnSelfFromTetherMod, "[EnergyOnSelfFromTether]", isEnergizedPresent, areBeamAndEnergizedPresent ? sparkHealingBeam.m_energyOnCasterPerTurn : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_needToChooseTargetMod, "[Need to Choose Target?]", isEnergizedPresent, !isEnergizedPresent || sparkEnergized.m_needToSelectTarget);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_effectOnEnemyOnNextTurn, "[Effect on Enemy on Next Turn]", "", isEnergizedPresent);
		desc += PropDesc(m_bothTetherExtraEffectOnSelfMod, "[BothTetherExtraEffectOnSelf]", isEnergizedPresent, isEnergizedPresent ? sparkEnergized.m_bothTetherExtraEffectOnSelf : null);
		desc += PropDesc(m_bothTetherAllyEffectMod, "[BothTetherAllyEffect]", isEnergizedPresent, isEnergizedPresent ? sparkEnergized.m_bothTetherAllyEffect : null);
		desc += PropDesc(m_bothTetherEnemyEffectMod, "[BothTetherEnemyEffect]", isEnergizedPresent, isEnergizedPresent ? sparkEnergized.m_bothTetherEnemyEffect : null);
		desc += PropDesc(m_bothTetherExtraHealMod, "[BothTetherExtraHeal]", isEnergizedPresent, isEnergizedPresent ? sparkEnergized.m_bothTetherExtraHeal : 0);
		return desc + PropDesc(m_bothTetherExtraDamageMod, "[BothTetherExtraDamage]", isEnergizedPresent, isEnergizedPresent ? sparkEnergized.m_bothTetherExtraDamage : 0);
	}
}
