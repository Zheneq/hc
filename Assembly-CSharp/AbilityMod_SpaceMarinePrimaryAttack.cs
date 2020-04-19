using System;
using System.Collections.Generic;

public class AbilityMod_SpaceMarinePrimaryAttack : AbilityMod
{
	[Separator("Targeting", true)]
	public AbilityModPropertyLaserInfo m_laserTargetInfoMod;

	public AbilityModPropertyBool m_addConeOnFirstHitTargetMod;

	public AbilityModPropertyConeInfo m_coneTargetInfoMod;

	[Separator("Enemy Hit: Laser", true)]
	public AbilityModPropertyInt m_baseDamageMod;

	public AbilityModPropertyInt m_extraDamageOnClosestMod;

	[Separator("Enemy Hit: Cone", true)]
	public AbilityModPropertyInt m_coneDamageAmountMod;

	public AbilityModPropertyEffectInfo m_coneEnemyHitEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SpaceMarinePrimaryAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SpaceMarinePrimaryAttack spaceMarinePrimaryAttack = targetAbility as SpaceMarinePrimaryAttack;
		if (spaceMarinePrimaryAttack != null)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SpaceMarinePrimaryAttack.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken_LaserInfo(tokens, this.m_laserTargetInfoMod, "LaserTargetInfo", spaceMarinePrimaryAttack.m_laserTargetInfo, true);
			AbilityMod.AddToken_ConeInfo(tokens, this.m_coneTargetInfoMod, "ConeTargetInfo", spaceMarinePrimaryAttack.m_coneTargetInfo, true);
			AbilityMod.AddToken(tokens, this.m_baseDamageMod, "BaseDamage", string.Empty, spaceMarinePrimaryAttack.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageOnClosestMod, "ExtraDamageOnClosest", string.Empty, spaceMarinePrimaryAttack.m_extraDamageToClosestTarget, true, false);
			AbilityMod.AddToken(tokens, this.m_coneDamageAmountMod, "ConeDamageAmount", string.Empty, spaceMarinePrimaryAttack.m_coneDamageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_coneEnemyHitEffectMod, "ConeEnemyHitEffect", spaceMarinePrimaryAttack.m_coneEnemyHitEffect, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SpaceMarinePrimaryAttack spaceMarinePrimaryAttack = base.GetTargetAbilityOnAbilityData(abilityData) as SpaceMarinePrimaryAttack;
		bool flag = spaceMarinePrimaryAttack != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyLaserInfo laserTargetInfoMod = this.m_laserTargetInfoMod;
		string prefix = "[LaserTargetInfo]";
		bool showBaseVal = flag;
		LaserTargetingInfo baseLaserInfo;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SpaceMarinePrimaryAttack.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseLaserInfo = spaceMarinePrimaryAttack.m_laserTargetInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		text = str + base.PropDesc(laserTargetInfoMod, prefix, showBaseVal, baseLaserInfo);
		text += base.PropDesc(this.m_addConeOnFirstHitTargetMod, "[AddConeOnFirstHitTarget]", flag, flag && spaceMarinePrimaryAttack.m_addConeOnFirstHitTarget);
		text += base.PropDesc(this.m_coneTargetInfoMod, "[ConeTargetInfo]", flag, (!flag) ? null : spaceMarinePrimaryAttack.m_coneTargetInfo);
		string str2 = text;
		AbilityModPropertyInt baseDamageMod = this.m_baseDamageMod;
		string prefix2 = "[DamageAmount]";
		bool showBaseVal2 = flag;
		int baseVal;
		if (flag)
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
			baseVal = spaceMarinePrimaryAttack.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str2 + base.PropDesc(baseDamageMod, prefix2, showBaseVal2, baseVal);
		text += base.PropDesc(this.m_extraDamageOnClosestMod, "[ExtraDamageToClosestTarget]", flag, (!flag) ? 0 : spaceMarinePrimaryAttack.m_extraDamageToClosestTarget);
		string str3 = text;
		AbilityModPropertyInt coneDamageAmountMod = this.m_coneDamageAmountMod;
		string prefix3 = "[ConeDamageAmount]";
		bool showBaseVal3 = flag;
		int baseVal2;
		if (flag)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = spaceMarinePrimaryAttack.m_coneDamageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str3 + base.PropDesc(coneDamageAmountMod, prefix3, showBaseVal3, baseVal2);
		string str4 = text;
		AbilityModPropertyEffectInfo coneEnemyHitEffectMod = this.m_coneEnemyHitEffectMod;
		string prefix4 = "[ConeEnemyHitEffect]";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal3;
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
			baseVal3 = spaceMarinePrimaryAttack.m_coneEnemyHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		return str4 + base.PropDesc(coneEnemyHitEffectMod, prefix4, showBaseVal4, baseVal3);
	}
}
