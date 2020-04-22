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
		if (!(spaceMarineMissileBarrage != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_damageMod, "Damage", string.Empty, spaceMarineMissileBarrage.m_damage);
			AbilityMod.AddToken(tokens, m_activeDurationMod, "Duration", string.Empty, 1);
			AbilityMod.AddToken(tokens, m_extraDamagePerTarget, "ExtraDamagePerTarget", string.Empty, 0);
			if (m_missileHitEffectOverride != null)
			{
				while (true)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					AbilityMod.AddToken_EffectInfo(tokens, m_missileHitEffectOverride, "EnemyHitEffect", spaceMarineMissileBarrage.m_effectOnTargets);
					return;
				}
			}
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SpaceMarineMissileBarrage spaceMarineMissileBarrage = GetTargetAbilityOnAbilityData(abilityData) as SpaceMarineMissileBarrage;
		bool flag = spaceMarineMissileBarrage != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal;
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = spaceMarineMissileBarrage.m_damage;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(damageMod, "[Damage]", flag, baseVal);
		empty += AbilityModHelper.GetModPropertyDesc(m_activeDurationMod, "[Active Duration]", flag, 1);
		empty += AbilityModHelper.GetModEffectInfoDesc(m_missileHitEffectOverride, "{ Missile Hit Effect Override }", string.Empty, flag, (!flag) ? null : spaceMarineMissileBarrage.m_effectOnTargets);
		return empty + PropDesc(m_extraDamagePerTarget, "[Extra Damage Per Target]", flag);
	}
}
