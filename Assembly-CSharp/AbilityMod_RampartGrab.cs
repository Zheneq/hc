using System;
using System.Collections.Generic;
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
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, rampartGrab.m_damageAmount);
			AbilityMod.AddToken(tokens, m_damageAfterFirstHitMod, "DamageAfterFirstHit", string.Empty, rampartGrab.m_damageAfterFirstHit);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", rampartGrab.m_enemyHitEffect);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, rampartGrab.m_maxTargets);
			AbilityMod.AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, rampartGrab.m_laserRange);
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, rampartGrab.m_laserWidth);
			AbilityMod.AddToken(tokens, m_destinationSelectRangeMod, "DestinationSelectRange", string.Empty, rampartGrab.m_destinationSelectRange);
			AbilityMod.AddToken(tokens, m_destinationAngleDegWithBackMod, "DestinationAngleDegWithBack", string.Empty, rampartGrab.m_destinationAngleDegWithBack);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RampartGrab rampartGrab = GetTargetAbilityOnAbilityData(abilityData) as RampartGrab;
		bool flag = rampartGrab != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt damageAmountMod = m_damageAmountMod;
		int baseVal;
		if (flag)
		{
			while (true)
			{
				switch (3)
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
			baseVal = rampartGrab.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(damageAmountMod, "[DamageAmount]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt damageAfterFirstHitMod = m_damageAfterFirstHitMod;
		int baseVal2;
		if (flag)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = rampartGrab.m_damageAfterFirstHit;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(damageAfterFirstHitMod, "[DamageAfterFirstHit]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectMod = m_enemyHitEffectMod;
		object baseVal3;
		if (flag)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = rampartGrab.m_enemyHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		empty = str3 + PropDesc(enemyHitEffectMod, "[EnemyHitEffect]", flag, (StandardEffectInfo)baseVal3);
		empty += PropDesc(m_chooseEndPositionMod, "[ChooseEndPosition]", flag, flag && rampartGrab.m_chooseEndPosition);
		empty += PropDesc(m_maxTargetsMod, "[MaxTargets]", flag, flag ? rampartGrab.m_maxTargets : 0);
		string str4 = empty;
		AbilityModPropertyFloat laserRangeMod = m_laserRangeMod;
		float baseVal4;
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
			baseVal4 = rampartGrab.m_laserRange;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(laserRangeMod, "[LaserRange]", flag, baseVal4);
		empty += PropDesc(m_laserWidthMod, "[LaserWidth]", flag, (!flag) ? 0f : rampartGrab.m_laserWidth);
		string str5 = empty;
		AbilityModPropertyBool penetrateLosMod = m_penetrateLosMod;
		int baseVal5;
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
			baseVal5 = (rampartGrab.m_penetrateLos ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(penetrateLosMod, "[PenetrateLos]", flag, (byte)baseVal5 != 0);
		string str6 = empty;
		AbilityModPropertyFloat destinationSelectRangeMod = m_destinationSelectRangeMod;
		float baseVal6;
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
			baseVal6 = rampartGrab.m_destinationSelectRange;
		}
		else
		{
			baseVal6 = 0f;
		}
		empty = str6 + PropDesc(destinationSelectRangeMod, "[DestinationSelectRange]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyInt destinationAngleDegWithBackMod = m_destinationAngleDegWithBackMod;
		int baseVal7;
		if (flag)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal7 = rampartGrab.m_destinationAngleDegWithBack;
		}
		else
		{
			baseVal7 = 0;
		}
		return str7 + PropDesc(destinationAngleDegWithBackMod, "[DestinationAngleDegWithBack]", flag, baseVal7);
	}
}
