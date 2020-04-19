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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SpaceMarineMissileBarrage.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_damageMod, "Damage", string.Empty, spaceMarineMissileBarrage.m_damage, true, false);
			AbilityMod.AddToken(tokens, this.m_activeDurationMod, "Duration", string.Empty, 1, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamagePerTarget, "ExtraDamagePerTarget", string.Empty, 0, true, false);
			if (this.m_missileHitEffectOverride != null)
			{
				for (;;)
				{
					switch (6)
					{
					case 0:
						continue;
					}
					break;
				}
				AbilityMod.AddToken_EffectInfo(tokens, this.m_missileHitEffectOverride, "EnemyHitEffect", spaceMarineMissileBarrage.m_effectOnTargets, true);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SpaceMarineMissileBarrage spaceMarineMissileBarrage = base.GetTargetAbilityOnAbilityData(abilityData) as SpaceMarineMissileBarrage;
		bool flag = spaceMarineMissileBarrage != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix = "[Damage]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SpaceMarineMissileBarrage.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = spaceMarineMissileBarrage.m_damage;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(damageMod, prefix, showBaseVal, baseVal);
		text += AbilityModHelper.GetModPropertyDesc(this.m_activeDurationMod, "[Active Duration]", flag, 1);
		text += AbilityModHelper.GetModEffectInfoDesc(this.m_missileHitEffectOverride, "{ Missile Hit Effect Override }", string.Empty, flag, (!flag) ? null : spaceMarineMissileBarrage.m_effectOnTargets);
		return text + base.PropDesc(this.m_extraDamagePerTarget, "[Extra Damage Per Target]", flag, 0);
	}
}
