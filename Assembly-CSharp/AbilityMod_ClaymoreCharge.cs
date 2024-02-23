using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_ClaymoreCharge : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyShape m_aoeShapeMod;
	public AbilityModPropertyFloat m_widthMod;
	public AbilityModPropertyFloat m_maxRangeMod;
	public AbilityModPropertyBool m_directHitIgnoreCoverMod;
	[Header("-- Hit Damage and Effects")]
	public AbilityModPropertyInt m_directHitDamageMod;
	public AbilityModPropertyEffectInfo m_directEnemyHitEffectMod;
	public AbilityModPropertyInt m_aoeDamageMod;
	public AbilityModPropertyEffectInfo m_aoeEnemyHitEffectMod;
	[Header("-- Extra Damage from Charge Path Length")]
	public AbilityModPropertyInt m_extraDirectHitDamagePerSquareMod;
	[Header("-- Heal on Self")]
	public AbilityModPropertyInt m_healOnSelfPerTargetHitMod;
	[Header("-- Cooldown and Chase on Hit")]
	public AbilityModPropertyInt m_cooldownOnHitMod;
	public AbilityModPropertyBool m_chaseHitActorMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ClaymoreCharge);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ClaymoreCharge claymoreCharge = targetAbility as ClaymoreCharge;
		if (claymoreCharge != null)
		{
			AddToken(tokens, m_widthMod, "ChargeLineWidth", "charge line width", claymoreCharge.m_width);
			AddToken(tokens, m_maxRangeMod, "ChargeLineRange", "max charge range", claymoreCharge.m_maxRange);
			AddToken(tokens, m_directHitDamageMod, "Damage_DirectHit", "direct hit damage from charge", claymoreCharge.m_directHitDamage);
			AddToken_EffectMod(tokens, m_directEnemyHitEffectMod, "Effect_DirectHit", claymoreCharge.m_directEnemyHitEffect);
			AddToken(tokens, m_aoeDamageMod, "Damage_AoeHit", "aoe hit damage", claymoreCharge.m_aoeDamage);
			AddToken_EffectMod(tokens, m_aoeEnemyHitEffectMod, "Effect_AoeHit", claymoreCharge.m_aoeEnemyHitEffect);
			AddToken(tokens, m_extraDirectHitDamagePerSquareMod, "ExtraDirectHitDamagePerSquare", string.Empty, claymoreCharge.m_extraDirectHitDamagePerSquare);
			AddToken(tokens, m_healOnSelfPerTargetHitMod, "HealOnSelfPerTargetHit", string.Empty, claymoreCharge.m_healOnSelfPerTargetHit);
			AddToken(tokens, m_cooldownOnHitMod, "CooldownOnHit", "set cooldown to this when hit enemy", claymoreCharge.m_cooldownOnHit);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClaymoreCharge claymoreCharge = GetTargetAbilityOnAbilityData(abilityData) as ClaymoreCharge;
		bool isAbilityPresent = claymoreCharge != null;
		string desc = string.Empty;
		desc += PropDesc(m_aoeShapeMod, "[Charge Hit Shape]", isAbilityPresent, isAbilityPresent ? claymoreCharge.m_aoeShape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_widthMod, "[Charge Line Width]", isAbilityPresent, isAbilityPresent ? claymoreCharge.m_width : 0f);
		desc += PropDesc(m_maxRangeMod, "[Charge Line Range]", isAbilityPresent, isAbilityPresent ? claymoreCharge.m_maxRange : 0f);
		desc += PropDesc(m_directHitIgnoreCoverMod, "[DirectHitIgnoreCover]", isAbilityPresent, isAbilityPresent && claymoreCharge.m_directHitIgnoreCover);
		desc += PropDesc(m_directHitDamageMod, "[Direct Hit Damage]", isAbilityPresent, isAbilityPresent ? claymoreCharge.m_directHitDamage : 0);
		desc += PropDesc(m_directEnemyHitEffectMod, "[Direct Hit Effect]", isAbilityPresent, isAbilityPresent ? claymoreCharge.m_directEnemyHitEffect : null);
		desc += PropDesc(m_aoeDamageMod, "[AOE Hit Damage]", isAbilityPresent, isAbilityPresent ? claymoreCharge.m_aoeDamage : 0);
		desc += PropDesc(m_aoeEnemyHitEffectMod, "[AOE Hit Effect]", isAbilityPresent, isAbilityPresent ? claymoreCharge.m_aoeEnemyHitEffect : null);
		desc += PropDesc(m_extraDirectHitDamagePerSquareMod, "[ExtraDirectHitDamagePerSquare]", isAbilityPresent, isAbilityPresent ? claymoreCharge.m_extraDirectHitDamagePerSquare : 0);
		desc += PropDesc(m_healOnSelfPerTargetHitMod, "[HealOnSelfPerTargetHit]", isAbilityPresent, isAbilityPresent ? claymoreCharge.m_healOnSelfPerTargetHit : 0);
		desc += PropDesc(m_cooldownOnHitMod, "[Cooldown Override (on charge ability) on Hit]", isAbilityPresent, isAbilityPresent ? claymoreCharge.m_cooldownOnHit : 0);
		return new StringBuilder().Append(desc).Append(PropDesc(m_chaseHitActorMod, "[Chase Hit Target?]", isAbilityPresent, isAbilityPresent && claymoreCharge.m_chaseHitActor)).ToString();
	}
}
