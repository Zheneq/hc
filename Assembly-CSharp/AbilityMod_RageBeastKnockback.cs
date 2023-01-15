using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_RageBeastKnockback : AbilityMod
{
	[Header("-- Targeting Mod")]
	public AbilityModPropertyInt m_maxTargetMod;
	public AbilityModPropertyFloat m_targeterWidthMod;
	public AbilityModPropertyFloat m_targeterLengthMod;
	[Header("-- Damage Mod")]
	public AbilityModPropertyInt m_onHitDamageMod;
	[Header("-- UNUSED: Damage Mods from Knockback Move")]
	public AbilityModPropertyInt m_collisionDamageToSelfMod;
	public AbilityModPropertyInt m_collisionDamageToOtherMod;
	public AbilityModPropertyInt m_collisionDamageFromGeoMod;
	[Header("-- UNUSED Effect on Geo Collision")]
	public StandardEffectInfo m_effectOnGeoCollision;
	[Header("-- Knockback Mod")]
	public AbilityModPropertyFloat m_knockbackDistanceMinMod;
	public AbilityModPropertyFloat m_knockbackDistanceMaxMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(RageBeastKnockback);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RageBeastKnockback rageBeastKnockback = targetAbility as RageBeastKnockback;
		if (rageBeastKnockback != null)
		{
			AddToken(tokens, m_targeterWidthMod, "LaserWidth", string.Empty, rageBeastKnockback.m_laserWidth);
			AddToken(tokens, m_targeterLengthMod, "LaserDistance", string.Empty, rageBeastKnockback.m_laserDistance);
			AddToken(tokens, m_maxTargetMod, "MaxTargets", string.Empty, rageBeastKnockback.m_maxTargets);
			AddToken(tokens, m_knockbackDistanceMinMod, "KnockbackMinDistance", string.Empty, rageBeastKnockback.m_knockbackDistanceMin);
			AddToken(tokens, m_knockbackDistanceMaxMod, "KnockbackMaxDistance", string.Empty, rageBeastKnockback.m_knockbackDistanceMax);
			AddToken(tokens, m_onHitDamageMod, "DamageAmount", string.Empty, rageBeastKnockback.m_damageAmount);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RageBeastKnockback rageBeastKnockback = GetTargetAbilityOnAbilityData(abilityData) as RageBeastKnockback;
		bool isValid = rageBeastKnockback != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_maxTargetMod, "[Max Targets]", isValid, isValid ? rageBeastKnockback.m_maxTargets : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_targeterWidthMod, "[Targeter Width]", isValid, isValid ? rageBeastKnockback.m_laserWidth : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_targeterLengthMod, "[Targeter Length]", isValid, isValid ? rageBeastKnockback.m_laserDistance : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_onHitDamageMod, "[On Hit Damage]", isValid, isValid ? rageBeastKnockback.m_damageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_collisionDamageToSelfMod, "[ !!!UNUSED: Collision Damage to Mover]", isValid, isValid ? rageBeastKnockback.m_damageToMoverOnCollision : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_collisionDamageToOtherMod, "[ !!!UNUSED: Collision Damage to Other]", isValid, isValid ? rageBeastKnockback.m_damageToOtherOnCollision : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_collisionDamageFromGeoMod, "[ !!!UNUSED: Collision Damage from Geo]", isValid, isValid ? rageBeastKnockback.m_damageCollisionWithGeo : 0);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_effectOnGeoCollision, "[Effect on Geo Collision]", string.Empty, isValid);
		desc += AbilityModHelper.GetModPropertyDesc(m_knockbackDistanceMinMod, "[Knockback Min Distance]", isValid, isValid ? rageBeastKnockback.m_knockbackDistanceMin : 0f);
		return desc + AbilityModHelper.GetModPropertyDesc(m_knockbackDistanceMaxMod, "[Knockback Max Distance]", isValid, isValid ? rageBeastKnockback.m_knockbackDistanceMax : 0f);
	}
}
