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
		if (!(rageBeastKnockback != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_targeterWidthMod, "LaserWidth", string.Empty, rageBeastKnockback.m_laserWidth);
			AbilityMod.AddToken(tokens, m_targeterLengthMod, "LaserDistance", string.Empty, rageBeastKnockback.m_laserDistance);
			AbilityMod.AddToken(tokens, m_maxTargetMod, "MaxTargets", string.Empty, rageBeastKnockback.m_maxTargets);
			AbilityMod.AddToken(tokens, m_knockbackDistanceMinMod, "KnockbackMinDistance", string.Empty, rageBeastKnockback.m_knockbackDistanceMin);
			AbilityMod.AddToken(tokens, m_knockbackDistanceMaxMod, "KnockbackMaxDistance", string.Empty, rageBeastKnockback.m_knockbackDistanceMax);
			AbilityMod.AddToken(tokens, m_onHitDamageMod, "DamageAmount", string.Empty, rageBeastKnockback.m_damageAmount);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RageBeastKnockback rageBeastKnockback = GetTargetAbilityOnAbilityData(abilityData) as RageBeastKnockback;
		bool flag = rageBeastKnockback != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt maxTargetMod = m_maxTargetMod;
		int baseVal;
		if (flag)
		{
			baseVal = rageBeastKnockback.m_maxTargets;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(maxTargetMod, "[Max Targets]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat targeterWidthMod = m_targeterWidthMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = rageBeastKnockback.m_laserWidth;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(targeterWidthMod, "[Targeter Width]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat targeterLengthMod = m_targeterLengthMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = rageBeastKnockback.m_laserDistance;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(targeterLengthMod, "[Targeter Length]", flag, baseVal3);
		empty += AbilityModHelper.GetModPropertyDesc(m_onHitDamageMod, "[On Hit Damage]", flag, flag ? rageBeastKnockback.m_damageAmount : 0);
		empty += AbilityModHelper.GetModPropertyDesc(m_collisionDamageToSelfMod, "[ !!!UNUSED: Collision Damage to Mover]", flag, flag ? rageBeastKnockback.m_damageToMoverOnCollision : 0);
		string str4 = empty;
		AbilityModPropertyInt collisionDamageToOtherMod = m_collisionDamageToOtherMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = rageBeastKnockback.m_damageToOtherOnCollision;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(collisionDamageToOtherMod, "[ !!!UNUSED: Collision Damage to Other]", flag, baseVal4);
		empty += AbilityModHelper.GetModPropertyDesc(m_collisionDamageFromGeoMod, "[ !!!UNUSED: Collision Damage from Geo]", flag, flag ? rageBeastKnockback.m_damageCollisionWithGeo : 0);
		empty += AbilityModHelper.GetModEffectInfoDesc(m_effectOnGeoCollision, "[Effect on Geo Collision]", string.Empty, flag);
		string str5 = empty;
		AbilityModPropertyFloat knockbackDistanceMinMod = m_knockbackDistanceMinMod;
		float baseVal5;
		if (flag)
		{
			baseVal5 = rageBeastKnockback.m_knockbackDistanceMin;
		}
		else
		{
			baseVal5 = 0f;
		}
		empty = str5 + AbilityModHelper.GetModPropertyDesc(knockbackDistanceMinMod, "[Knockback Min Distance]", flag, baseVal5);
		return empty + AbilityModHelper.GetModPropertyDesc(m_knockbackDistanceMaxMod, "[Knockback Max Distance]", flag, (!flag) ? 0f : rageBeastKnockback.m_knockbackDistanceMax);
	}
}
