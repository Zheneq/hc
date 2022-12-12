using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SpaceMarineMissileBarrage : AbilityMod
{
	[Header("----------------------------")]
	public AbilityModPropertyInt m_damageMod;
	public AbilityModPropertyInt m_activeDurationMod;
	public StandardEffectInfo m_missileHitEffectOverride;
	public AbilityModPropertyInt m_extraDamagePerTarget;

	public override Type GetTargetAbilityType()
	{
		return typeof(SpaceMarineMissileBarrage);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SpaceMarineMissileBarrage spaceMarineMissileBarrage = targetAbility as SpaceMarineMissileBarrage;
		if (spaceMarineMissileBarrage != null)
		{
			AddToken(tokens, m_damageMod, "Damage", string.Empty, spaceMarineMissileBarrage.m_damage);
			AddToken(tokens, m_activeDurationMod, "Duration", string.Empty, 1);
			AddToken(tokens, m_extraDamagePerTarget, "ExtraDamagePerTarget", string.Empty, 0);
			if (m_missileHitEffectOverride != null)
			{
				AddToken_EffectInfo(tokens, m_missileHitEffectOverride, "EnemyHitEffect", spaceMarineMissileBarrage.m_effectOnTargets);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SpaceMarineMissileBarrage spaceMarineMissileBarrage = GetTargetAbilityOnAbilityData(abilityData) as SpaceMarineMissileBarrage;
		bool isValid = spaceMarineMissileBarrage != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", isValid, isValid ? spaceMarineMissileBarrage.m_damage : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_activeDurationMod, "[Active Duration]", isValid, 1);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_missileHitEffectOverride, "{ Missile Hit Effect Override }", string.Empty, isValid, isValid ? spaceMarineMissileBarrage.m_effectOnTargets : null);
		return desc + PropDesc(m_extraDamagePerTarget, "[Extra Damage Per Target]", isValid);
	}
}
