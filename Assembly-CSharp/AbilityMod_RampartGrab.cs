using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_RampartGrab : AbilityMod
{
	[Header("-- On Hit Damage and Effect")]
	public AbilityModPropertyInt m_damageAmountMod;
	public AbilityModPropertyInt m_damageAfterFirstHitMod;
	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;
	[Header("-- Knockback Targeting")]
	public AbilityModPropertyBool m_chooseEndPositionMod;
	public AbilityModPropertyInt m_maxTargetsMod;
	public AbilityModPropertyFloat m_laserRangeMod;
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyBool m_penetrateLosMod;
	[Header("-- Targeting Ranges")]
	public AbilityModPropertyFloat m_destinationSelectRangeMod;
	public AbilityModPropertyInt m_destinationAngleDegWithBackMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(RampartGrab);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RampartGrab rampartGrab = targetAbility as RampartGrab;
		if (rampartGrab != null)
		{
			AddToken(tokens, m_damageAmountMod, "DamageAmount", "", rampartGrab.m_damageAmount);
			AddToken(tokens, m_damageAfterFirstHitMod, "DamageAfterFirstHit", "", rampartGrab.m_damageAfterFirstHit);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", rampartGrab.m_enemyHitEffect);
			AddToken(tokens, m_maxTargetsMod, "MaxTargets", "", rampartGrab.m_maxTargets);
			AddToken(tokens, m_laserRangeMod, "LaserRange", "", rampartGrab.m_laserRange);
			AddToken(tokens, m_laserWidthMod, "LaserWidth", "", rampartGrab.m_laserWidth);
			AddToken(tokens, m_destinationSelectRangeMod, "DestinationSelectRange", "", rampartGrab.m_destinationSelectRange);
			AddToken(tokens, m_destinationAngleDegWithBackMod, "DestinationAngleDegWithBack", "", rampartGrab.m_destinationAngleDegWithBack);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RampartGrab rampartGrab = GetTargetAbilityOnAbilityData(abilityData) as RampartGrab;
		bool isAbilityPresent = rampartGrab != null;
		string desc = "";
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isAbilityPresent, isAbilityPresent ? rampartGrab.m_damageAmount : 0);
		desc += PropDesc(m_damageAfterFirstHitMod, "[DamageAfterFirstHit]", isAbilityPresent, isAbilityPresent ? rampartGrab.m_damageAfterFirstHit : 0);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isAbilityPresent, isAbilityPresent ? rampartGrab.m_enemyHitEffect : null);
		desc += PropDesc(m_chooseEndPositionMod, "[ChooseEndPosition]", isAbilityPresent, isAbilityPresent && rampartGrab.m_chooseEndPosition);
		desc += PropDesc(m_maxTargetsMod, "[MaxTargets]", isAbilityPresent, isAbilityPresent ? rampartGrab.m_maxTargets : 0);
		desc += PropDesc(m_laserRangeMod, "[LaserRange]", isAbilityPresent, isAbilityPresent ? rampartGrab.m_laserRange : 0f);
		desc += PropDesc(m_laserWidthMod, "[LaserWidth]", isAbilityPresent, isAbilityPresent ? rampartGrab.m_laserWidth : 0f);
		desc += PropDesc(m_penetrateLosMod, "[PenetrateLos]", isAbilityPresent, isAbilityPresent && rampartGrab.m_penetrateLos);
		desc += PropDesc(m_destinationSelectRangeMod, "[DestinationSelectRange]", isAbilityPresent, isAbilityPresent ? rampartGrab.m_destinationSelectRange : 0f);
		return new StringBuilder().Append(desc).Append(PropDesc(m_destinationAngleDegWithBackMod, "[DestinationAngleDegWithBack]", isAbilityPresent, isAbilityPresent ? rampartGrab.m_destinationAngleDegWithBack : 0)).ToString();
	}
}
