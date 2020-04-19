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
		if (rampartMeleeBasicAttack != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RampartMeleeBasicAttack.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_laserRangeMod, "LaserRange", string.Empty, rampartMeleeBasicAttack.m_laserRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, rampartMeleeBasicAttack.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserMaxTargetsMod, "LaserMaxTargets", string.Empty, rampartMeleeBasicAttack.m_laserMaxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, rampartMeleeBasicAttack.m_coneWidthAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneRangeMod, "ConeRange", string.Empty, rampartMeleeBasicAttack.m_coneRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserDamageMod, "LaserDamage", string.Empty, rampartMeleeBasicAttack.m_laserDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_laserEnemyHitEffectMod, "LaserEnemyHitEffect", rampartMeleeBasicAttack.m_laserEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_coneDamageMod, "ConeDamage", string.Empty, rampartMeleeBasicAttack.m_coneDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_coneEnemyHitEffectMod, "ConeEnemyHitEffect", rampartMeleeBasicAttack.m_coneEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_bonusDamageForOverlapMod, "BonusDamageForOverlap", string.Empty, rampartMeleeBasicAttack.m_bonusDamageForOverlap, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RampartMeleeBasicAttack rampartMeleeBasicAttack = base.GetTargetAbilityOnAbilityData(abilityData) as RampartMeleeBasicAttack;
		bool flag = rampartMeleeBasicAttack != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat laserRangeMod = this.m_laserRangeMod;
		string prefix = "[LaserRange]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RampartMeleeBasicAttack.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = rampartMeleeBasicAttack.m_laserRange;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(laserRangeMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_laserWidthMod, "[LaserWidth]", flag, (!flag) ? 0f : rampartMeleeBasicAttack.m_laserWidth);
		string str2 = text;
		AbilityModPropertyInt laserMaxTargetsMod = this.m_laserMaxTargetsMod;
		string prefix2 = "[LaserMaxTargets]";
		bool showBaseVal2 = flag;
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
			baseVal2 = rampartMeleeBasicAttack.m_laserMaxTargets;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(laserMaxTargetsMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyBool penetrateLosMod = this.m_penetrateLosMod;
		string prefix3 = "[PenetrateLos]";
		bool showBaseVal3 = flag;
		bool baseVal3;
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
			baseVal3 = rampartMeleeBasicAttack.m_penetrateLos;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + base.PropDesc(penetrateLosMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_coneWidthAngleMod, "[ConeWidthAngle]", flag, (!flag) ? 0f : rampartMeleeBasicAttack.m_coneWidthAngle);
		string str4 = text;
		AbilityModPropertyFloat coneRangeMod = this.m_coneRangeMod;
		string prefix4 = "[ConeRange]";
		bool showBaseVal4 = flag;
		float baseVal4;
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
			baseVal4 = rampartMeleeBasicAttack.m_coneRange;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(coneRangeMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_laserDamageMod, "[LaserDamage]", flag, (!flag) ? 0 : rampartMeleeBasicAttack.m_laserDamage);
		text += base.PropDesc(this.m_laserEnemyHitEffectMod, "[LaserEnemyHitEffect]", flag, (!flag) ? null : rampartMeleeBasicAttack.m_laserEnemyHitEffect);
		string str5 = text;
		AbilityModPropertyInt coneDamageMod = this.m_coneDamageMod;
		string prefix5 = "[ConeDamage]";
		bool showBaseVal5 = flag;
		int baseVal5;
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
			baseVal5 = rampartMeleeBasicAttack.m_coneDamage;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(coneDamageMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyEffectInfo coneEnemyHitEffectMod = this.m_coneEnemyHitEffectMod;
		string prefix6 = "[ConeEnemyHitEffect]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal6;
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
			baseVal6 = rampartMeleeBasicAttack.m_coneEnemyHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + base.PropDesc(coneEnemyHitEffectMod, prefix6, showBaseVal6, baseVal6);
		return text + base.PropDesc(this.m_bonusDamageForOverlapMod, "[BonusDamageForOverlap]", flag, (!flag) ? 0 : rampartMeleeBasicAttack.m_bonusDamageForOverlap);
	}
}
