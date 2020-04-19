using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ScoundrelBlindFire : AbilityMod
{
	[Header("-- Cone Damage and Angle ------------------------")]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyFloat m_coneWidthAngleMod;

	public AbilityModPropertyBool m_penetrateLineOfSight;

	[Header("-- Effect to apply on Target hit ----------------")]
	public StandardEffectInfo m_effectOnTargetsHit;

	public override Type GetTargetAbilityType()
	{
		return typeof(ScoundrelBlindFire);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScoundrelBlindFire scoundrelBlindFire = targetAbility as ScoundrelBlindFire;
		if (scoundrelBlindFire != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ScoundrelBlindFire.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, scoundrelBlindFire.m_coneWidthAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageMod, "DamageAmount", string.Empty, scoundrelBlindFire.m_damageAmount, true, false);
			AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnTargetsHit, "EffectOnTargetHit", null, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScoundrelBlindFire scoundrelBlindFire = base.GetTargetAbilityOnAbilityData(abilityData) as ScoundrelBlindFire;
		bool flag = scoundrelBlindFire != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix = "[Cone Hit Damage]";
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ScoundrelBlindFire.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = scoundrelBlindFire.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(damageMod, prefix, showBaseVal, baseVal);
		text += AbilityModHelper.GetModPropertyDesc(this.m_coneWidthAngleMod, "[Cone Width Angle]", flag, (!flag) ? 0f : scoundrelBlindFire.m_coneWidthAngle);
		string str2 = text;
		AbilityModPropertyBool penetrateLineOfSight = this.m_penetrateLineOfSight;
		string prefix2 = "[Penetrate Line Of Sight]";
		bool showBaseVal2 = flag;
		bool baseVal2;
		if (flag)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = scoundrelBlindFire.m_penetrateLineOfSight;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(penetrateLineOfSight, prefix2, showBaseVal2, baseVal2);
		return text + AbilityModHelper.GetModEffectInfoDesc(this.m_effectOnTargetsHit, "{ Effect on Target Hit }", string.Empty, flag, null);
	}
}
