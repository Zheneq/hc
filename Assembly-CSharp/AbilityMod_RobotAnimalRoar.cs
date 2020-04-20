using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_RobotAnimalRoar : AbilityMod
{
	public AbilityModPropertyBool m_penetrateLosMod;

	[Header("-- Targeting: Shape")]
	public AbilityModPropertyShape m_shapeMod;

	[Header("-- Inner shape for different damage --")]
	public AbilityModPropertyBool m_useInnerShapeMod;

	public AbilityModPropertyShape m_innerShapeMod;

	public AbilityModPropertyInt m_innerShapeDamageMod;

	[Header("-- Targeting: Radius")]
	public AbilityModPropertyFloat m_targetingRadiusMod;

	public AbilityModPropertyFloat m_innerRadiusMod;

	[Header("-- Enemy Hit Effect, Damage, TechPoint Damage")]
	public StandardEffectInfo m_enemyHitEffectOverride;

	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyInt m_techPointDamageMod;

	[Header("-- Healing & Energy to Targeted Allies")]
	public int m_healAmountToTargetAllyOnHit;

	public int m_techPointGainToTargetAllyOnHit;

	public override Type GetTargetAbilityType()
	{
		return typeof(RobotAnimalRoar);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RobotAnimalRoar robotAnimalRoar = targetAbility as RobotAnimalRoar;
		if (robotAnimalRoar != null)
		{
			AbilityMod.AddToken(tokens, this.m_targetingRadiusMod, "TargetingRadius", string.Empty, robotAnimalRoar.m_targetingRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_innerRadiusMod, "InnerRadius", string.Empty, robotAnimalRoar.m_innerRadius, true, false, false);
			AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffectOverride, "EnemyHitEffect", robotAnimalRoar.m_enemyEffect, true);
			AbilityMod.AddToken(tokens, this.m_damageMod, "Damage", string.Empty, robotAnimalRoar.m_damage, true, false);
			int num;
			if (robotAnimalRoar.m_innerShapeDamage < 0)
			{
				num = robotAnimalRoar.m_damage;
			}
			else
			{
				num = robotAnimalRoar.m_innerShapeDamage;
			}
			int baseVal = num;
			AbilityMod.AddToken(tokens, this.m_innerShapeDamageMod, "InnerShapeDamage", string.Empty, baseVal, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RobotAnimalRoar robotAnimalRoar = base.GetTargetAbilityOnAbilityData(abilityData) as RobotAnimalRoar;
		bool flag = robotAnimalRoar != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyShape shapeMod = this.m_shapeMod;
		string prefix = "[Targeting Shape]";
		bool showBaseVal = flag;
		AbilityAreaShape baseVal;
		if (flag)
		{
			baseVal = robotAnimalRoar.m_aoeShape;
		}
		else
		{
			baseVal = AbilityAreaShape.SingleSquare;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(shapeMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_targetingRadiusMod, "[TargetingRadius]", flag, (!flag) ? 0f : robotAnimalRoar.m_targetingRadius);
		string str2 = text;
		AbilityModPropertyFloat innerRadiusMod = this.m_innerRadiusMod;
		string prefix2 = "[InnerRadius]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = robotAnimalRoar.m_innerRadius;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(innerRadiusMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyBool penetrateLosMod = this.m_penetrateLosMod;
		string prefix3 = "[Targeting Penetrate Los]";
		bool showBaseVal3 = flag;
		bool baseVal3;
		if (flag)
		{
			baseVal3 = robotAnimalRoar.m_penetrateLineOfSight;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(penetrateLosMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix4 = "[Damage]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = robotAnimalRoar.m_damage;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(damageMod, prefix4, showBaseVal4, baseVal4);
		text += AbilityModHelper.GetModPropertyDesc(this.m_techPointDamageMod, "[TechPoint Damage]", flag, 0);
		string str5 = text;
		StandardEffectInfo enemyHitEffectOverride = this.m_enemyHitEffectOverride;
		string prefix5 = "{ Effect Override on Enemy Hit}";
		string empty = string.Empty;
		bool useBaseVal = flag;
		StandardEffectInfo baseVal5;
		if (flag)
		{
			baseVal5 = robotAnimalRoar.m_enemyEffect;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + AbilityModHelper.GetModEffectInfoDesc(enemyHitEffectOverride, prefix5, empty, useBaseVal, baseVal5);
		if (this.m_healAmountToTargetAllyOnHit > 0)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Heals Targeted Ally on Hit] = ",
				this.m_healAmountToTargetAllyOnHit,
				"\n"
			});
		}
		if (this.m_techPointGainToTargetAllyOnHit > 0)
		{
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Grants Tech Points To Targeted Ally on Hit] = ",
				this.m_techPointGainToTargetAllyOnHit,
				"\n"
			});
		}
		text += base.PropDesc(this.m_useInnerShapeMod, "[UseInnerShape]", flag, flag && robotAnimalRoar.m_useInnerShape);
		string str6 = text;
		AbilityModPropertyShape innerShapeMod = this.m_innerShapeMod;
		string prefix6 = "[InnerShape]";
		bool showBaseVal5 = flag;
		AbilityAreaShape baseVal6;
		if (flag)
		{
			baseVal6 = robotAnimalRoar.m_innerShape;
		}
		else
		{
			baseVal6 = AbilityAreaShape.SingleSquare;
		}
		text = str6 + base.PropDesc(innerShapeMod, prefix6, showBaseVal5, baseVal6);
		string str7 = text;
		AbilityModPropertyInt innerShapeDamageMod = this.m_innerShapeDamageMod;
		string prefix7 = "[InnerShapeDamage]";
		bool showBaseVal6 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = robotAnimalRoar.m_innerShapeDamage;
		}
		else
		{
			baseVal7 = 0;
		}
		return str7 + base.PropDesc(innerShapeDamageMod, prefix7, showBaseVal6, baseVal7);
	}
}
