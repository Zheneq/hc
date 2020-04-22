using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_RampartMeleeBasicAttack : AbilityMod
{
	[Header("-- Laser Targeting")]
	public AbilityModPropertyFloat m_laserRangeMod;

	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyInt m_laserMaxTargetsMod;

	public AbilityModPropertyBool m_penetrateLosMod;

	[Header("-- Cone Targeting")]
	public AbilityModPropertyFloat m_coneWidthAngleMod;

	public AbilityModPropertyFloat m_coneRangeMod;

	[Header("-- Hit Damage/Effects")]
	public AbilityModPropertyInt m_laserDamageMod;

	public AbilityModPropertyEffectInfo m_laserEnemyHitEffectMod;

	public AbilityModPropertyInt m_coneDamageMod;

	public AbilityModPropertyEffectInfo m_coneEnemyHitEffectMod;

	public AbilityModPropertyInt m_bonusDamageForOverlapMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(RampartMeleeBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RampartMeleeBasicAttack rampartMeleeBasicAttack = targetAbility as RampartMeleeBasicAttack;
		if (!(rampartMeleeBasicAttack != null))
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
			AbilityMod.AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, rampartMeleeBasicAttack.m_laserRange);
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, rampartMeleeBasicAttack.m_laserWidth);
			AbilityMod.AddToken(tokens, m_laserMaxTargetsMod, "LaserMaxTargets", string.Empty, rampartMeleeBasicAttack.m_laserMaxTargets);
			AbilityMod.AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, rampartMeleeBasicAttack.m_coneWidthAngle);
			AbilityMod.AddToken(tokens, m_coneRangeMod, "ConeRange", string.Empty, rampartMeleeBasicAttack.m_coneRange);
			AbilityMod.AddToken(tokens, m_laserDamageMod, "LaserDamage", string.Empty, rampartMeleeBasicAttack.m_laserDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_laserEnemyHitEffectMod, "LaserEnemyHitEffect", rampartMeleeBasicAttack.m_laserEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_coneDamageMod, "ConeDamage", string.Empty, rampartMeleeBasicAttack.m_coneDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_coneEnemyHitEffectMod, "ConeEnemyHitEffect", rampartMeleeBasicAttack.m_coneEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_bonusDamageForOverlapMod, "BonusDamageForOverlap", string.Empty, rampartMeleeBasicAttack.m_bonusDamageForOverlap);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RampartMeleeBasicAttack rampartMeleeBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as RampartMeleeBasicAttack;
		bool flag = rampartMeleeBasicAttack != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat laserRangeMod = m_laserRangeMod;
		float baseVal;
		if (flag)
		{
			while (true)
			{
				switch (7)
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
			baseVal = rampartMeleeBasicAttack.m_laserRange;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(laserRangeMod, "[LaserRange]", flag, baseVal);
		empty += PropDesc(m_laserWidthMod, "[LaserWidth]", flag, (!flag) ? 0f : rampartMeleeBasicAttack.m_laserWidth);
		string str2 = empty;
		AbilityModPropertyInt laserMaxTargetsMod = m_laserMaxTargetsMod;
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
			baseVal2 = rampartMeleeBasicAttack.m_laserMaxTargets;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(laserMaxTargetsMod, "[LaserMaxTargets]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyBool penetrateLosMod = m_penetrateLosMod;
		int baseVal3;
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
			baseVal3 = (rampartMeleeBasicAttack.m_penetrateLos ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(penetrateLosMod, "[PenetrateLos]", flag, (byte)baseVal3 != 0);
		empty += PropDesc(m_coneWidthAngleMod, "[ConeWidthAngle]", flag, (!flag) ? 0f : rampartMeleeBasicAttack.m_coneWidthAngle);
		string str4 = empty;
		AbilityModPropertyFloat coneRangeMod = m_coneRangeMod;
		float baseVal4;
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
			baseVal4 = rampartMeleeBasicAttack.m_coneRange;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(coneRangeMod, "[ConeRange]", flag, baseVal4);
		empty += PropDesc(m_laserDamageMod, "[LaserDamage]", flag, flag ? rampartMeleeBasicAttack.m_laserDamage : 0);
		empty += PropDesc(m_laserEnemyHitEffectMod, "[LaserEnemyHitEffect]", flag, (!flag) ? null : rampartMeleeBasicAttack.m_laserEnemyHitEffect);
		string str5 = empty;
		AbilityModPropertyInt coneDamageMod = m_coneDamageMod;
		int baseVal5;
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
			baseVal5 = rampartMeleeBasicAttack.m_coneDamage;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(coneDamageMod, "[ConeDamage]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyEffectInfo coneEnemyHitEffectMod = m_coneEnemyHitEffectMod;
		object baseVal6;
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
			baseVal6 = rampartMeleeBasicAttack.m_coneEnemyHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str6 + PropDesc(coneEnemyHitEffectMod, "[ConeEnemyHitEffect]", flag, (StandardEffectInfo)baseVal6);
		return empty + PropDesc(m_bonusDamageForOverlapMod, "[BonusDamageForOverlap]", flag, flag ? rampartMeleeBasicAttack.m_bonusDamageForOverlap : 0);
	}
}
