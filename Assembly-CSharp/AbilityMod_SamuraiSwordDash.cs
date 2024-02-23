using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_SamuraiSwordDash : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_damageRadiusMod;
	public AbilityModPropertyFloat m_damageRadiusAtStartMod;
	public AbilityModPropertyFloat m_damageRadiusAtEndMod;
	public AbilityModPropertyBool m_penetrateLineOfSightMod;
	public AbilityModPropertyBool m_canMoveAfterEvadeMod;
	public AbilityModPropertyInt m_maxTargetsMod;
	[Header("-- How many targets can be damaged")]
	public AbilityModPropertyInt m_maxDamageTargetsMod;
	[Header("-- Enemy Hits, Dash Phase")]
	public AbilityModPropertyInt m_dashDamageMod;
	public AbilityModPropertyInt m_dashLessDamagePerTargetMod;
	public AbilityModPropertyEffectInfo m_dashEnemyHitEffectMod;
	[Header("-- Effect on Self")]
	public AbilityModPropertyEffectInfo m_dashSelfHitEffectMod;
	[Header("-- Mark data")]
	public AbilityModPropertyEffectInfo m_markEffectInfoMod;
	[Header("-- Energy Refund if target dashed away")]
	public AbilityModPropertyInt m_energyRefundIfTargetDashedAwayMod;
	[Separator("For Chain Ability (Knockback phase)")]
	public AbilityModPropertyInt m_knockbackDamageMod;
	public AbilityModPropertyInt m_knockbackLessDamagePerTargetMod;
	public AbilityModPropertyFloat m_knockbackExtraDamageFromDamageTakenMultMod;
	[Space(10f)]
	public AbilityModPropertyInt m_knockbackExtraDamageByDistMod;
	public AbilityModPropertyInt m_knockbackExtraDamageChangePerDistMod;
	[Header("-- Knockback")]
	public AbilityModPropertyFloat m_knockbackDistMod;
	public AbilityModPropertyKnockbackType m_knockbackTypeMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SamuraiSwordDash);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SamuraiSwordDash samuraiSwordDash = targetAbility as SamuraiSwordDash;
		if (samuraiSwordDash != null)
		{
			AddToken(tokens, m_damageRadiusMod, "DamageRadius", string.Empty, samuraiSwordDash.m_damageRadius);
			AddToken(tokens, m_damageRadiusAtStartMod, "DamageRadiusAtStart", string.Empty, samuraiSwordDash.m_damageRadiusAtStart);
			AddToken(tokens, m_damageRadiusAtEndMod, "DamageRadiusAtEnd", string.Empty, samuraiSwordDash.m_damageRadiusAtEnd);
			AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, samuraiSwordDash.m_maxTargets);
			AddToken(tokens, m_maxDamageTargetsMod, "MaxDamageTargets", string.Empty, samuraiSwordDash.m_maxDamageTargets);
			AddToken(tokens, m_dashDamageMod, "DamageAmount", string.Empty, samuraiSwordDash.m_dashDamage);
			AddToken(tokens, m_dashLessDamagePerTargetMod, "LessDamagePerTarget", string.Empty, samuraiSwordDash.m_dashLessDamagePerTarget);
			AddToken_EffectMod(tokens, m_dashEnemyHitEffectMod, "DashEnemyHitEffect", samuraiSwordDash.m_dashEnemyHitEffect);
			AddToken_EffectMod(tokens, m_dashSelfHitEffectMod, "DashSelfHitEffect", samuraiSwordDash.m_dashSelfHitEffect);
			AddToken_EffectMod(tokens, m_markEffectInfoMod, "MarkEffectInfo", samuraiSwordDash.m_markEffectInfo);
			AddToken(tokens, m_energyRefundIfTargetDashedAwayMod, "EnergyRefundIfTargetDashedAway", string.Empty, samuraiSwordDash.m_energyRefundIfTargetDashedAway);
			AddToken(tokens, m_knockbackDamageMod, "KnockbackDamage", string.Empty, samuraiSwordDash.m_knockbackDamage);
			AddToken(tokens, m_knockbackLessDamagePerTargetMod, "KnockbackLessDamagePerTarget", string.Empty, samuraiSwordDash.m_knockbackLessDamagePerTarget);
			AddToken(tokens, m_knockbackExtraDamageFromDamageTakenMultMod, "KnockbackExtraDamageFromDamageTakenMult", string.Empty, samuraiSwordDash.m_knockbackExtraDamageFromDamageTakenMult, true, false, true);
			AddToken(tokens, m_knockbackExtraDamageByDistMod, "KnockbackExtraDamageByDist", string.Empty, samuraiSwordDash.m_knockbackExtraDamageByDist);
			AddToken(tokens, m_knockbackExtraDamageChangePerDistMod, "KnockbackExtraDamageChangePerDist", string.Empty, samuraiSwordDash.m_knockbackExtraDamageChangePerDist);
			AddToken(tokens, m_knockbackDistMod, "KnockbackDist", string.Empty, samuraiSwordDash.m_knockbackDist);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SamuraiSwordDash samuraiSwordDash = GetTargetAbilityOnAbilityData(abilityData) as SamuraiSwordDash;
		bool isValid = samuraiSwordDash != null;
		string desc = string.Empty;
		desc += PropDesc(m_damageRadiusMod, "[DamageRadius]", isValid, isValid ? samuraiSwordDash.m_damageRadius : 0f);
		desc += PropDesc(m_damageRadiusAtStartMod, "[DamageRadiusAtStart]", isValid, isValid ? samuraiSwordDash.m_damageRadiusAtStart : 0f);
		desc += PropDesc(m_damageRadiusAtEndMod, "[DamageRadiusAtEnd]", isValid, isValid ? samuraiSwordDash.m_damageRadiusAtEnd : 0f);
		desc += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", isValid, isValid && samuraiSwordDash.m_penetrateLineOfSight);
		desc += PropDesc(m_canMoveAfterEvadeMod, "[CanMoveAfterEvade]", isValid, isValid && samuraiSwordDash.m_canMoveAfterEvade);
		desc += PropDesc(m_maxTargetsMod, "[MaxTargets]", isValid, isValid ? samuraiSwordDash.m_maxTargets : 0);
		desc += PropDesc(m_maxDamageTargetsMod, "[MaxDamageTargets]", isValid, isValid ? samuraiSwordDash.m_maxDamageTargets : 0);
		desc += PropDesc(m_dashDamageMod, "[DamageAmount]", isValid, isValid ? samuraiSwordDash.m_dashDamage : 0);
		desc += PropDesc(m_dashLessDamagePerTargetMod, "[LessDamagePerTarget]", isValid, isValid ? samuraiSwordDash.m_dashLessDamagePerTarget : 0);
		desc += PropDesc(m_dashEnemyHitEffectMod, "[DashEnemyHitEffect]", isValid, isValid ? samuraiSwordDash.m_dashEnemyHitEffect : null);
		desc += PropDesc(m_dashSelfHitEffectMod, "[DashSelfHitEffect]", isValid, isValid ? samuraiSwordDash.m_dashSelfHitEffect : null);
		desc += PropDesc(m_markEffectInfoMod, "[MarkEffectInfo]", isValid, isValid ? samuraiSwordDash.m_markEffectInfo : null);
		desc += PropDesc(m_energyRefundIfTargetDashedAwayMod, "[EnergyRefundIfTargetDashedAway]", isValid, isValid ? samuraiSwordDash.m_energyRefundIfTargetDashedAway : 0);
		desc += PropDesc(m_knockbackDamageMod, "[KnockbackDamage]", isValid, isValid ? samuraiSwordDash.m_knockbackDamage : 0);
		desc += PropDesc(m_knockbackLessDamagePerTargetMod, "[KnockbackLessDamagePerTarget]", isValid, isValid ? samuraiSwordDash.m_knockbackLessDamagePerTarget : 0);
		desc += PropDesc(m_knockbackExtraDamageFromDamageTakenMultMod, "[KnockbackExtraDamageFromDamageTakenMult]", isValid, isValid ? samuraiSwordDash.m_knockbackExtraDamageFromDamageTakenMult : 0f);
		desc += PropDesc(m_knockbackExtraDamageByDistMod, "[KnockbackExtraDamageByDist]", isValid, isValid ? samuraiSwordDash.m_knockbackExtraDamageByDist : 0);
		desc += PropDesc(m_knockbackExtraDamageChangePerDistMod, "[KnockbackExtraDamageChangePerDist]", isValid, isValid ? samuraiSwordDash.m_knockbackExtraDamageChangePerDist : 0);
		desc += PropDesc(m_knockbackDistMod, "[KnockbackDist]", isValid, isValid ? samuraiSwordDash.m_knockbackDist : 0f);
		return new StringBuilder().Append(desc).Append(PropDesc(m_knockbackTypeMod, "[KnockbackType]", isValid, isValid ? samuraiSwordDash.m_knockbackType : KnockbackType.AwayFromSource)).ToString();
	}
}
