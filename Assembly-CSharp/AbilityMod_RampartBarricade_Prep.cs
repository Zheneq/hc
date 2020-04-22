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
		if (!(rampartBarricade_Prep != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, rampartBarricade_Prep.m_damageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", rampartBarricade_Prep.m_enemyHitEffect);
			AbilityMod.AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, rampartBarricade_Prep.m_laserRange);
			AbilityMod.AddToken(tokens, m_knockbackDistanceMod, "KnockbackDistance", string.Empty, rampartBarricade_Prep.m_knockbackDistance);
			Passive_Rampart component = rampartBarricade_Prep.GetComponent<Passive_Rampart>();
			if (component != null)
			{
				while (true)
				{
					AbilityMod.AddToken_BarrierMod(tokens, m_shieldBarrierDataMod, "ShieldBarrier", component.m_normalShieldBarrierData);
					return;
				}
			}
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RampartBarricade_Prep rampartBarricade_Prep = GetTargetAbilityOnAbilityData(abilityData) as RampartBarricade_Prep;
		bool flag = rampartBarricade_Prep != null;
		object obj;
		if ((bool)abilityData)
		{
			obj = abilityData.GetComponent<Passive_Rampart>();
		}
		else
		{
			obj = null;
		}
		Passive_Rampart passive_Rampart = (Passive_Rampart)obj;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyBool allowAimAtDiagonalsMod = m_allowAimAtDiagonalsMod;
		int baseVal;
		if (flag)
		{
			baseVal = (rampartBarricade_Prep.m_allowAimAtDiagonals ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(allowAimAtDiagonalsMod, "[AllowAimAtDiagonals]", flag, (byte)baseVal != 0);
		string str2 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectMod = m_enemyHitEffectMod;
		object baseVal2;
		if (flag)
		{
			baseVal2 = rampartBarricade_Prep.m_enemyHitEffect;
		}
		else
		{
			baseVal2 = null;
		}
		empty = str2 + PropDesc(enemyHitEffectMod, "[EnemyHitEffect]", flag, (StandardEffectInfo)baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat laserRangeMod = m_laserRangeMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = rampartBarricade_Prep.m_laserRange;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(laserRangeMod, "[LaserRange]", flag, baseVal3);
		empty += PropDesc(m_laserLengthIgnoreLosMod, "[LaserLengthIgnoreLos]", flag, flag && rampartBarricade_Prep.m_laserLengthIgnoreLos);
		empty += PropDesc(m_penetrateLosMod, "[PenetrateLos]", flag, flag && rampartBarricade_Prep.m_penetrateLos);
		empty += PropDesc(m_knockbackDistanceMod, "[KnockbackDistance]", flag, (!flag) ? 0f : rampartBarricade_Prep.m_knockbackDistance);
		return empty + AbilityModHelper.GetModPropertyDesc(m_shieldBarrierDataMod, "{ Barrier Data Mod }", (!(passive_Rampart != null)) ? null : passive_Rampart.m_normalShieldBarrierData);
	}
}
