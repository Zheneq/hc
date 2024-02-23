using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_SparkAoeBuffDebuff : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_radiusMod;
	public AbilityModPropertyBool m_ignoreLosMod;
	[Header("-- For Ally Hit")]
	public AbilityModPropertyInt m_allyHealMod;
	public AbilityModPropertyEffectInfo m_allyHitEffectMod;
	[Header("-- For Self Hit")]
	public AbilityModPropertyInt m_baseSelfHealMod;
	public AbilityModPropertyInt m_selfHealPerHitMod;
	public AbilityModPropertyBool m_selfHealHitCountEnemy;
	public AbilityModPropertyBool m_selfHealHitCountAlly;
	public AbilityModPropertyEffectInfo m_selfHitEffectMod;
	[Header("-- Shield on Self")]
	public AbilityModPropertyInt m_shieldOnSelfPerAllyHitMod;
	public int m_shieldOnSelfDuration = 2;
	[Header("-- For Enemy Hit")]
	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SparkAoeBuffDebuff);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SparkAoeBuffDebuff sparkAoeBuffDebuff = targetAbility as SparkAoeBuffDebuff;
		if (sparkAoeBuffDebuff != null)
		{
			AddToken(tokens, m_radiusMod, "TargetingRadius", "targeting radius", sparkAoeBuffDebuff.m_radius);
			AddToken_EffectMod(tokens, m_allyHitEffectMod, "EffectOnAlly", sparkAoeBuffDebuff.m_allyHitEffect);
			AddToken(tokens, m_allyHealMod, "Heal_OnAlly", "heal on ally", sparkAoeBuffDebuff.m_allyHealAmount);
			AddToken_EffectMod(tokens, m_selfHitEffectMod, "EffectOnSelf", sparkAoeBuffDebuff.m_selfHitEffect);
			AddToken(tokens, m_baseSelfHealMod, "Heal_BaseOnSelf", "base heal on self", sparkAoeBuffDebuff.m_baseSelfHeal);
			AddToken(tokens, m_selfHealPerHitMod, "Heal_PerTargetHit", "heal on self per hit", sparkAoeBuffDebuff.m_selfHealAmountPerHit);
			AddToken(tokens, m_shieldOnSelfPerAllyHitMod, "SelfShieldPerAllyHit", "shield on self per ally hit", 0, false);
			tokens.Add(new TooltipTokenInt("ShieldOnSelfDuration", "duration for shield on self, from ally hits", m_shieldOnSelfDuration));
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EffectOnEnemy", sparkAoeBuffDebuff.m_enemyHitEffect);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SparkAoeBuffDebuff ability = GetTargetAbilityOnAbilityData(abilityData) as SparkAoeBuffDebuff;
		bool isAbilityPresent = ability != null;
		string desc = "";
		desc += PropDesc(m_radiusMod, "[Targeting Radius]", isAbilityPresent, isAbilityPresent ? ability.m_radius : 0f);
		desc += PropDesc(m_ignoreLosMod, "[Ignore LoS?]", isAbilityPresent, isAbilityPresent && ability.m_penetrateLos);
		desc += PropDesc(m_allyHealMod, "[Ally Heal]", isAbilityPresent, isAbilityPresent ? ability.m_allyHealAmount : 0);
		desc += PropDesc(m_allyHitEffectMod, "{ Ally Hit Effect }", isAbilityPresent, isAbilityPresent ? ability.m_allyHitEffect : null);
		desc += PropDesc(m_baseSelfHealMod, "[Base Self Heal]", isAbilityPresent, isAbilityPresent ? ability.m_baseSelfHeal : 0);
		desc += PropDesc(m_selfHealPerHitMod, "[Self Heal per Hit]", isAbilityPresent, isAbilityPresent ? ability.m_selfHealAmountPerHit : 0);
		desc += PropDesc(m_selfHealHitCountEnemy, "[Self Heal Count Enemy]", isAbilityPresent, isAbilityPresent && ability.m_selfHealCountEnemyHit);
		desc += PropDesc(m_selfHealHitCountAlly, "[Self Heal Count Ally]", isAbilityPresent, isAbilityPresent && ability.m_selfHealCountAllyHit);
		desc += PropDesc(m_selfHitEffectMod, "{ Self Hit Effect }", isAbilityPresent, isAbilityPresent ? ability.m_selfHitEffect : null);
		desc += PropDesc(m_shieldOnSelfPerAllyHitMod, "[Shield on Self per Hit]");
		if (m_shieldOnSelfPerAllyHitMod != null && m_shieldOnSelfPerAllyHitMod.GetModifiedValue(0) > 0)
		{
			desc += new StringBuilder().Append("[Shield Duration (for hit on allies)] ").Append(m_shieldOnSelfDuration).Append("\n").ToString();
		}
		desc += PropDesc(m_enemyHitEffectMod, "{ Enemy Hit Effect }", isAbilityPresent, isAbilityPresent ? ability.m_enemyHitEffect : null);
		return desc;
	}
}
