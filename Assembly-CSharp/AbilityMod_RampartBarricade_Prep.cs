using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_RampartBarricade_Prep : AbilityMod
{
	[Header("-- Barrier Aiming")]
	public AbilityModPropertyBool m_allowAimAtDiagonalsMod;
	[Header("-- Knockback Hit Damage and Effect")]
	public AbilityModPropertyInt m_damageAmountMod;
	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;
	[Header("-- Laser and Knockback")]
	public AbilityModPropertyFloat m_laserRangeMod;
	public AbilityModPropertyBool m_laserLengthIgnoreLosMod;
	public AbilityModPropertyBool m_penetrateLosMod;
	public AbilityModPropertyFloat m_knockbackDistanceMod;
	public AbilityModPropertyBarrierDataV2 m_shieldBarrierDataMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(RampartBarricade_Prep);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RampartBarricade_Prep rampartBarricade_Prep = targetAbility as RampartBarricade_Prep;
		if (rampartBarricade_Prep != null)
		{
			AddToken(tokens, m_damageAmountMod, "DamageAmount", "", rampartBarricade_Prep.m_damageAmount);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", rampartBarricade_Prep.m_enemyHitEffect);
			AddToken(tokens, m_laserRangeMod, "LaserRange", "", rampartBarricade_Prep.m_laserRange);
			AddToken(tokens, m_knockbackDistanceMod, "KnockbackDistance", "", rampartBarricade_Prep.m_knockbackDistance);
			Passive_Rampart component = rampartBarricade_Prep.GetComponent<Passive_Rampart>();
			if (component != null)
			{
				AddToken_BarrierMod(tokens, m_shieldBarrierDataMod, "ShieldBarrier", component.m_normalShieldBarrierData);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RampartBarricade_Prep rampartBarricade_Prep = GetTargetAbilityOnAbilityData(abilityData) as RampartBarricade_Prep;
		bool isAbilityPresent = rampartBarricade_Prep != null;
		Passive_Rampart passive_Rampart = abilityData?.GetComponent<Passive_Rampart>();
		string desc = "";
		desc += PropDesc(m_allowAimAtDiagonalsMod, "[AllowAimAtDiagonals]", isAbilityPresent, isAbilityPresent && rampartBarricade_Prep.m_allowAimAtDiagonals);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isAbilityPresent, rampartBarricade_Prep?.m_enemyHitEffect);
		desc += PropDesc(m_laserRangeMod, "[LaserRange]", isAbilityPresent, isAbilityPresent ? rampartBarricade_Prep.m_laserRange : 0f);
		desc += PropDesc(m_laserLengthIgnoreLosMod, "[LaserLengthIgnoreLos]", isAbilityPresent, isAbilityPresent && rampartBarricade_Prep.m_laserLengthIgnoreLos);
		desc += PropDesc(m_penetrateLosMod, "[PenetrateLos]", isAbilityPresent, isAbilityPresent && rampartBarricade_Prep.m_penetrateLos);
		desc += PropDesc(m_knockbackDistanceMod, "[KnockbackDistance]", isAbilityPresent, isAbilityPresent ? rampartBarricade_Prep.m_knockbackDistance : 0f);
		return desc + AbilityModHelper.GetModPropertyDesc(m_shieldBarrierDataMod, "{ Barrier Data Mod }", passive_Rampart?.m_normalShieldBarrierData);
	}
}
