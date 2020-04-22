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
		if (!(robotAnimalRoar != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_targetingRadiusMod, "TargetingRadius", string.Empty, robotAnimalRoar.m_targetingRadius);
			AbilityMod.AddToken(tokens, m_innerRadiusMod, "InnerRadius", string.Empty, robotAnimalRoar.m_innerRadius);
			AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffectOverride, "EnemyHitEffect", robotAnimalRoar.m_enemyEffect);
			AbilityMod.AddToken(tokens, m_damageMod, "Damage", string.Empty, robotAnimalRoar.m_damage);
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
			AbilityMod.AddToken(tokens, m_innerShapeDamageMod, "InnerShapeDamage", string.Empty, baseVal);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RobotAnimalRoar robotAnimalRoar = GetTargetAbilityOnAbilityData(abilityData) as RobotAnimalRoar;
		bool flag = robotAnimalRoar != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyShape shapeMod = m_shapeMod;
		int baseVal;
		if (flag)
		{
			baseVal = (int)robotAnimalRoar.m_aoeShape;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(shapeMod, "[Targeting Shape]", flag, (AbilityAreaShape)baseVal);
		empty += PropDesc(m_targetingRadiusMod, "[TargetingRadius]", flag, (!flag) ? 0f : robotAnimalRoar.m_targetingRadius);
		string str2 = empty;
		AbilityModPropertyFloat innerRadiusMod = m_innerRadiusMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = robotAnimalRoar.m_innerRadius;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(innerRadiusMod, "[InnerRadius]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyBool penetrateLosMod = m_penetrateLosMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = (robotAnimalRoar.m_penetrateLineOfSight ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(penetrateLosMod, "[Targeting Penetrate Los]", flag, (byte)baseVal3 != 0);
		string str4 = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = robotAnimalRoar.m_damage;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(damageMod, "[Damage]", flag, baseVal4);
		empty += AbilityModHelper.GetModPropertyDesc(m_techPointDamageMod, "[TechPoint Damage]", flag);
		string str5 = empty;
		StandardEffectInfo enemyHitEffectOverride = m_enemyHitEffectOverride;
		string empty2 = string.Empty;
		object baseVal5;
		if (flag)
		{
			baseVal5 = robotAnimalRoar.m_enemyEffect;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + AbilityModHelper.GetModEffectInfoDesc(enemyHitEffectOverride, "{ Effect Override on Enemy Hit}", empty2, flag, (StandardEffectInfo)baseVal5);
		if (m_healAmountToTargetAllyOnHit > 0)
		{
			string text = empty;
			empty = text + "[Heals Targeted Ally on Hit] = " + m_healAmountToTargetAllyOnHit + "\n";
		}
		if (m_techPointGainToTargetAllyOnHit > 0)
		{
			string text = empty;
			empty = text + "[Grants Tech Points To Targeted Ally on Hit] = " + m_techPointGainToTargetAllyOnHit + "\n";
		}
		empty += PropDesc(m_useInnerShapeMod, "[UseInnerShape]", flag, flag && robotAnimalRoar.m_useInnerShape);
		string str6 = empty;
		AbilityModPropertyShape innerShapeMod = m_innerShapeMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = (int)robotAnimalRoar.m_innerShape;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(innerShapeMod, "[InnerShape]", flag, (AbilityAreaShape)baseVal6);
		string str7 = empty;
		AbilityModPropertyInt innerShapeDamageMod = m_innerShapeDamageMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = robotAnimalRoar.m_innerShapeDamage;
		}
		else
		{
			baseVal7 = 0;
		}
		return str7 + PropDesc(innerShapeDamageMod, "[InnerShapeDamage]", flag, baseVal7);
	}
}
