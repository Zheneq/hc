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
		if (!(spaceMarinePrimaryAttack != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken_LaserInfo(tokens, m_laserTargetInfoMod, "LaserTargetInfo", spaceMarinePrimaryAttack.m_laserTargetInfo);
			AbilityMod.AddToken_ConeInfo(tokens, m_coneTargetInfoMod, "ConeTargetInfo", spaceMarinePrimaryAttack.m_coneTargetInfo);
			AbilityMod.AddToken(tokens, m_baseDamageMod, "BaseDamage", string.Empty, spaceMarinePrimaryAttack.m_damageAmount);
			AbilityMod.AddToken(tokens, m_extraDamageOnClosestMod, "ExtraDamageOnClosest", string.Empty, spaceMarinePrimaryAttack.m_extraDamageToClosestTarget);
			AbilityMod.AddToken(tokens, m_coneDamageAmountMod, "ConeDamageAmount", string.Empty, spaceMarinePrimaryAttack.m_coneDamageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_coneEnemyHitEffectMod, "ConeEnemyHitEffect", spaceMarinePrimaryAttack.m_coneEnemyHitEffect);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SpaceMarinePrimaryAttack spaceMarinePrimaryAttack = GetTargetAbilityOnAbilityData(abilityData) as SpaceMarinePrimaryAttack;
		bool flag = spaceMarinePrimaryAttack != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyLaserInfo laserTargetInfoMod = m_laserTargetInfoMod;
		object baseLaserInfo;
		if (flag)
		{
			while (true)
			{
				switch (4)
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
			baseLaserInfo = spaceMarinePrimaryAttack.m_laserTargetInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		empty = str + PropDesc(laserTargetInfoMod, "[LaserTargetInfo]", flag, (LaserTargetingInfo)baseLaserInfo);
		empty += PropDesc(m_addConeOnFirstHitTargetMod, "[AddConeOnFirstHitTarget]", flag, flag && spaceMarinePrimaryAttack.m_addConeOnFirstHitTarget);
		empty += PropDesc(m_coneTargetInfoMod, "[ConeTargetInfo]", flag, (!flag) ? null : spaceMarinePrimaryAttack.m_coneTargetInfo);
		string str2 = empty;
		AbilityModPropertyInt baseDamageMod = m_baseDamageMod;
		int baseVal;
		if (flag)
		{
			while (true)
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
		empty = str2 + PropDesc(baseDamageMod, "[DamageAmount]", flag, baseVal);
		empty += PropDesc(m_extraDamageOnClosestMod, "[ExtraDamageToClosestTarget]", flag, flag ? spaceMarinePrimaryAttack.m_extraDamageToClosestTarget : 0);
		string str3 = empty;
		AbilityModPropertyInt coneDamageAmountMod = m_coneDamageAmountMod;
		int baseVal2;
		if (flag)
		{
			while (true)
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
		empty = str3 + PropDesc(coneDamageAmountMod, "[ConeDamageAmount]", flag, baseVal2);
		string str4 = empty;
		AbilityModPropertyEffectInfo coneEnemyHitEffectMod = m_coneEnemyHitEffectMod;
		object baseVal3;
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
			baseVal3 = spaceMarinePrimaryAttack.m_coneEnemyHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		return str4 + PropDesc(coneEnemyHitEffectMod, "[ConeEnemyHitEffect]", flag, (StandardEffectInfo)baseVal3);
	}
}
