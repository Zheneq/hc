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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RampartBarricade_Prep.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, rampartBarricade_Prep.m_damageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", rampartBarricade_Prep.m_enemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_laserRangeMod, "LaserRange", string.Empty, rampartBarricade_Prep.m_laserRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_knockbackDistanceMod, "KnockbackDistance", string.Empty, rampartBarricade_Prep.m_knockbackDistance, true, false, false);
			Passive_Rampart component = rampartBarricade_Prep.GetComponent<Passive_Rampart>();
			if (component != null)
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
				AbilityMod.AddToken_BarrierMod(tokens, this.m_shieldBarrierDataMod, "ShieldBarrier", component.m_normalShieldBarrierData);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RampartBarricade_Prep rampartBarricade_Prep = base.GetTargetAbilityOnAbilityData(abilityData) as RampartBarricade_Prep;
		bool flag = rampartBarricade_Prep != null;
		Passive_Rampart passive_Rampart;
		if (abilityData)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RampartBarricade_Prep.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			passive_Rampart = abilityData.GetComponent<Passive_Rampart>();
		}
		else
		{
			passive_Rampart = null;
		}
		Passive_Rampart passive_Rampart2 = passive_Rampart;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyBool allowAimAtDiagonalsMod = this.m_allowAimAtDiagonalsMod;
		string prefix = "[AllowAimAtDiagonals]";
		bool showBaseVal = flag;
		bool baseVal;
		if (flag)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal = rampartBarricade_Prep.m_allowAimAtDiagonals;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(allowAimAtDiagonalsMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyEffectInfo enemyHitEffectMod = this.m_enemyHitEffectMod;
		string prefix2 = "[EnemyHitEffect]";
		bool showBaseVal2 = flag;
		StandardEffectInfo baseVal2;
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
			baseVal2 = rampartBarricade_Prep.m_enemyHitEffect;
		}
		else
		{
			baseVal2 = null;
		}
		text = str2 + base.PropDesc(enemyHitEffectMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat laserRangeMod = this.m_laserRangeMod;
		string prefix3 = "[LaserRange]";
		bool showBaseVal3 = flag;
		float baseVal3;
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
			baseVal3 = rampartBarricade_Prep.m_laserRange;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(laserRangeMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_laserLengthIgnoreLosMod, "[LaserLengthIgnoreLos]", flag, flag && rampartBarricade_Prep.m_laserLengthIgnoreLos);
		text += base.PropDesc(this.m_penetrateLosMod, "[PenetrateLos]", flag, flag && rampartBarricade_Prep.m_penetrateLos);
		text += base.PropDesc(this.m_knockbackDistanceMod, "[KnockbackDistance]", flag, (!flag) ? 0f : rampartBarricade_Prep.m_knockbackDistance);
		return text + AbilityModHelper.GetModPropertyDesc(this.m_shieldBarrierDataMod, "{ Barrier Data Mod }", (!(passive_Rampart2 != null)) ? null : passive_Rampart2.m_normalShieldBarrierData);
	}
}
