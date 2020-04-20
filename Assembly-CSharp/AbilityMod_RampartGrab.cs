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
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, rampartGrab.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_damageAfterFirstHitMod, "DamageAfterFirstHit", string.Empty, rampartGrab.m_damageAfterFirstHit, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", rampartGrab.m_enemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargets", string.Empty, rampartGrab.m_maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_laserRangeMod, "LaserRange", string.Empty, rampartGrab.m_laserRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, rampartGrab.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_destinationSelectRangeMod, "DestinationSelectRange", string.Empty, rampartGrab.m_destinationSelectRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_destinationAngleDegWithBackMod, "DestinationAngleDegWithBack", string.Empty, rampartGrab.m_destinationAngleDegWithBack, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RampartGrab rampartGrab = base.GetTargetAbilityOnAbilityData(abilityData) as RampartGrab;
		bool flag = rampartGrab != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt damageAmountMod = this.m_damageAmountMod;
		string prefix = "[DamageAmount]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			baseVal = rampartGrab.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(damageAmountMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt damageAfterFirstHitMod = this.m_damageAfterFirstHitMod;
		string prefix2 = "[DamageAfterFirstHit]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = rampartGrab.m_damageAfterFirstHit;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(damageAfterFirstHitMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyEffectInfo enemyHitEffectMod = this.m_enemyHitEffectMod;
		string prefix3 = "[EnemyHitEffect]";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal3;
		if (flag)
		{
			baseVal3 = rampartGrab.m_enemyHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		text = str3 + base.PropDesc(enemyHitEffectMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_chooseEndPositionMod, "[ChooseEndPosition]", flag, flag && rampartGrab.m_chooseEndPosition);
		text += base.PropDesc(this.m_maxTargetsMod, "[MaxTargets]", flag, (!flag) ? 0 : rampartGrab.m_maxTargets);
		string str4 = text;
		AbilityModPropertyFloat laserRangeMod = this.m_laserRangeMod;
		string prefix4 = "[LaserRange]";
		bool showBaseVal4 = flag;
		float baseVal4;
		if (flag)
		{
			baseVal4 = rampartGrab.m_laserRange;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(laserRangeMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_laserWidthMod, "[LaserWidth]", flag, (!flag) ? 0f : rampartGrab.m_laserWidth);
		string str5 = text;
		AbilityModPropertyBool penetrateLosMod = this.m_penetrateLosMod;
		string prefix5 = "[PenetrateLos]";
		bool showBaseVal5 = flag;
		bool baseVal5;
		if (flag)
		{
			baseVal5 = rampartGrab.m_penetrateLos;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(penetrateLosMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyFloat destinationSelectRangeMod = this.m_destinationSelectRangeMod;
		string prefix6 = "[DestinationSelectRange]";
		bool showBaseVal6 = flag;
		float baseVal6;
		if (flag)
		{
			baseVal6 = rampartGrab.m_destinationSelectRange;
		}
		else
		{
			baseVal6 = 0f;
		}
		text = str6 + base.PropDesc(destinationSelectRangeMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt destinationAngleDegWithBackMod = this.m_destinationAngleDegWithBackMod;
		string prefix7 = "[DestinationAngleDegWithBack]";
		bool showBaseVal7 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = rampartGrab.m_destinationAngleDegWithBack;
		}
		else
		{
			baseVal7 = 0;
		}
		return str7 + base.PropDesc(destinationAngleDegWithBackMod, prefix7, showBaseVal7, baseVal7);
	}
}
