// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SenseiBide : AbilityMod
{
	[Header("-- Targeting --")]
	public AbilityModPropertyBool m_targetingIgnoreLosMod;
	[Separator("Effect on Cast Target", "cyan")]
	public AbilityModPropertyEffectData m_onCastTargetEffectDataMod;
	[Header("-- Additional Effect on targeted actor, for shielding, etc")]
	public AbilityModPropertyEffectInfo m_additionalTargetHitEffectMod;
	[Separator("For Explosion Hits", "cyan")]
	public AbilityModPropertyFloat m_explosionRadiusMod;
	public AbilityModPropertyBool m_ignoreLosMod;
	[Header("-- Explosion Hit --")]
	public AbilityModPropertyInt m_maxDamageMod;
	public AbilityModPropertyInt m_baseDamageMod;
	public AbilityModPropertyFloat m_damageMultMod;
	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;
	[Header("-- Heal portion of absorb remaining")]
	public AbilityModPropertyFloat m_absorbMultForHealMod;
	[Header("-- Damage portion of initial damage, on turns after")]
	public AbilityModPropertyFloat m_multOnInitialDamageForSubseqHitsMod;
	[Separator("Extra Heal on Heal AoE Ability")]
	public AbilityModPropertyInt m_extraHealOnHealAoeIfQueuedMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SenseiBide);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SenseiBide senseiBide = targetAbility as SenseiBide;
		if (senseiBide != null)
		{
			AddToken_EffectMod(tokens, m_onCastTargetEffectDataMod, "OnCastTargetEffectData", senseiBide.m_onCastTargetEffectData);
			AddToken_EffectMod(tokens, m_additionalTargetHitEffectMod, "AdditionalTargetHitEffect", senseiBide.m_additionalTargetHitEffect);
			AddToken(tokens, m_explosionRadiusMod, "ExplosionRadius", string.Empty, senseiBide.m_explosionRadius);
			AddToken(tokens, m_maxDamageMod, "MaxDamage", string.Empty, senseiBide.m_maxDamage);
			AddToken(tokens, m_baseDamageMod, "BaseDamage", string.Empty, senseiBide.m_baseDamage);
			AddToken(tokens, m_damageMultMod, "DamageMult", string.Empty, senseiBide.m_damageMult);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", senseiBide.m_enemyHitEffect);
			AddToken(tokens, m_absorbMultForHealMod, "AbsorbMultForHeal", string.Empty, senseiBide.m_absorbMultForHeal);
			AddToken(tokens, m_multOnInitialDamageForSubseqHitsMod, "MultOnInitialDamageForSubseqHits", string.Empty, senseiBide.m_multOnInitialDamageForSubseqHits);
			AddToken(tokens, m_extraHealOnHealAoeIfQueuedMod, "ExtraHealOnHealAoeIfQueued", string.Empty, senseiBide.m_extraHealOnHealAoeIfQueued);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)  // , Ability targetAbility in rogues
	{
		// reactor
		SenseiBide senseiBide = GetTargetAbilityOnAbilityData(abilityData) as SenseiBide;
		// rogues
		// SenseiBide senseiBide = targetAbility as SenseiBide;
		
		bool isValid = senseiBide != null;
		string desc = string.Empty;
		desc += PropDesc(m_targetingIgnoreLosMod, "[TargetingIgnoreLos]", isValid, isValid && senseiBide.m_targetingIgnoreLos);
		desc += PropDesc(m_onCastTargetEffectDataMod, "[OnCastTargetEffectData]", isValid, isValid ? senseiBide.m_onCastTargetEffectData : null);
		desc += PropDesc(m_additionalTargetHitEffectMod, "[AdditionalTargetHitEffect]", isValid, isValid ? senseiBide.m_additionalTargetHitEffect : null);
		desc += PropDesc(m_explosionRadiusMod, "[ExplosionRadius]", isValid, isValid ? senseiBide.m_explosionRadius : 0f);
		desc += PropDesc(m_ignoreLosMod, "[IgnoreLos]", isValid, isValid && senseiBide.m_ignoreLos);
		desc += PropDesc(m_maxDamageMod, "[MaxDamage]", isValid, isValid ? senseiBide.m_maxDamage : 0);
		desc += PropDesc(m_baseDamageMod, "[BaseDamage]", isValid, isValid ? senseiBide.m_baseDamage : 0);
		desc += PropDesc(m_damageMultMod, "[DamageMult]", isValid, isValid ? senseiBide.m_damageMult : 0f);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isValid, isValid ? senseiBide.m_enemyHitEffect : null);
		desc += PropDesc(m_absorbMultForHealMod, "[AbsorbMultForHeal]", isValid, isValid ? senseiBide.m_absorbMultForHeal : 0f);
		desc += PropDesc(m_multOnInitialDamageForSubseqHitsMod, "[MultOnInitialDamageForSubseqHits]", isValid, isValid ? senseiBide.m_multOnInitialDamageForSubseqHits : 0f);
		return desc + PropDesc(m_extraHealOnHealAoeIfQueuedMod, "[ExtraHealOnHealAoeIfQueued]", isValid, isValid ? senseiBide.m_extraHealOnHealAoeIfQueued : 0);
	}
}
