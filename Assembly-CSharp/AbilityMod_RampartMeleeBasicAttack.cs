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
			AddToken(tokens, m_laserRangeMod, "LaserRange", "", rampartMeleeBasicAttack.m_laserRange);
			AddToken(tokens, m_laserWidthMod, "LaserWidth", "", rampartMeleeBasicAttack.m_laserWidth);
			AddToken(tokens, m_laserMaxTargetsMod, "LaserMaxTargets", "", rampartMeleeBasicAttack.m_laserMaxTargets);
			AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", "", rampartMeleeBasicAttack.m_coneWidthAngle);
			AddToken(tokens, m_coneRangeMod, "ConeRange", "", rampartMeleeBasicAttack.m_coneRange);
			AddToken(tokens, m_laserDamageMod, "LaserDamage", "", rampartMeleeBasicAttack.m_laserDamage);
			AddToken_EffectMod(tokens, m_laserEnemyHitEffectMod, "LaserEnemyHitEffect", rampartMeleeBasicAttack.m_laserEnemyHitEffect);
			AddToken(tokens, m_coneDamageMod, "ConeDamage", "", rampartMeleeBasicAttack.m_coneDamage);
			AddToken_EffectMod(tokens, m_coneEnemyHitEffectMod, "ConeEnemyHitEffect", rampartMeleeBasicAttack.m_coneEnemyHitEffect);
			AddToken(tokens, m_bonusDamageForOverlapMod, "BonusDamageForOverlap", "", rampartMeleeBasicAttack.m_bonusDamageForOverlap);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RampartMeleeBasicAttack rampartMeleeBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as RampartMeleeBasicAttack;
		bool isAbilityPresent = rampartMeleeBasicAttack != null;
		string desc = "";
		desc += PropDesc(m_laserRangeMod, "[LaserRange]", isAbilityPresent, isAbilityPresent ? rampartMeleeBasicAttack.m_laserRange : 0f);
		desc += PropDesc(m_laserWidthMod, "[LaserWidth]", isAbilityPresent, isAbilityPresent ? rampartMeleeBasicAttack.m_laserWidth : 0f);
		desc += PropDesc(m_laserMaxTargetsMod, "[LaserMaxTargets]", isAbilityPresent, isAbilityPresent ? rampartMeleeBasicAttack.m_laserMaxTargets : 0);
		desc += PropDesc(m_penetrateLosMod, "[PenetrateLos]", isAbilityPresent, isAbilityPresent && rampartMeleeBasicAttack.m_penetrateLos);
		desc += PropDesc(m_coneWidthAngleMod, "[ConeWidthAngle]", isAbilityPresent, isAbilityPresent ? rampartMeleeBasicAttack.m_coneWidthAngle : 0f);
		desc += PropDesc(m_coneRangeMod, "[ConeRange]", isAbilityPresent, isAbilityPresent ? rampartMeleeBasicAttack.m_coneRange : 0f);
		desc += PropDesc(m_laserDamageMod, "[LaserDamage]", isAbilityPresent, isAbilityPresent ? rampartMeleeBasicAttack.m_laserDamage : 0);
		desc += PropDesc(m_laserEnemyHitEffectMod, "[LaserEnemyHitEffect]", isAbilityPresent, isAbilityPresent ? rampartMeleeBasicAttack.m_laserEnemyHitEffect : null);
		desc += PropDesc(m_coneDamageMod, "[ConeDamage]", isAbilityPresent, isAbilityPresent ? rampartMeleeBasicAttack.m_coneDamage : 0);
		desc += PropDesc(m_coneEnemyHitEffectMod, "[ConeEnemyHitEffect]", isAbilityPresent, isAbilityPresent ? rampartMeleeBasicAttack.m_coneEnemyHitEffect : null);
		return desc + PropDesc(m_bonusDamageForOverlapMod, "[BonusDamageForOverlap]", isAbilityPresent, isAbilityPresent ? rampartMeleeBasicAttack.m_bonusDamageForOverlap : 0);
	}
}
