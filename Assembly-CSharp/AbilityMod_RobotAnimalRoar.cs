using System;
using System.Collections.Generic;
using System.Text;
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
			AddToken(tokens, m_targetingRadiusMod, "TargetingRadius", string.Empty, robotAnimalRoar.m_targetingRadius);
			AddToken(tokens, m_innerRadiusMod, "InnerRadius", string.Empty, robotAnimalRoar.m_innerRadius);
			AddToken_EffectInfo(tokens, m_enemyHitEffectOverride, "EnemyHitEffect", robotAnimalRoar.m_enemyEffect);
			AddToken(tokens, m_damageMod, "Damage", string.Empty, robotAnimalRoar.m_damage);
			AddToken(tokens, m_innerShapeDamageMod, "InnerShapeDamage", string.Empty, robotAnimalRoar.m_innerShapeDamage < 0
				? robotAnimalRoar.m_damage
				: robotAnimalRoar.m_innerShapeDamage);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RobotAnimalRoar robotAnimalRoar = GetTargetAbilityOnAbilityData(abilityData) as RobotAnimalRoar;
		bool isAbilityPresent = robotAnimalRoar != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_shapeMod, "[Targeting Shape]", isAbilityPresent, isAbilityPresent ? robotAnimalRoar.m_aoeShape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_targetingRadiusMod, "[TargetingRadius]", isAbilityPresent, isAbilityPresent ? robotAnimalRoar.m_targetingRadius : 0f);
		desc += PropDesc(m_innerRadiusMod, "[InnerRadius]", isAbilityPresent, isAbilityPresent ? robotAnimalRoar.m_innerRadius : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_penetrateLosMod, "[Targeting Penetrate Los]", isAbilityPresent, isAbilityPresent && robotAnimalRoar.m_penetrateLineOfSight);
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", isAbilityPresent, isAbilityPresent ? robotAnimalRoar.m_damage : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_techPointDamageMod, "[TechPoint Damage]", isAbilityPresent);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_enemyHitEffectOverride, "{ Effect Override on Enemy Hit}", string.Empty, isAbilityPresent, isAbilityPresent ? robotAnimalRoar.m_enemyEffect : null);
		if (m_healAmountToTargetAllyOnHit > 0)
		{
			desc += new StringBuilder().Append("[Heals Targeted Ally on Hit] = ").Append(m_healAmountToTargetAllyOnHit).Append("\n").ToString();
		}
		if (m_techPointGainToTargetAllyOnHit > 0)
		{
			desc += new StringBuilder().Append("[Grants Tech Points To Targeted Ally on Hit] = ").Append(m_techPointGainToTargetAllyOnHit).Append("\n").ToString();
		}
		desc += PropDesc(m_useInnerShapeMod, "[UseInnerShape]", isAbilityPresent, isAbilityPresent && robotAnimalRoar.m_useInnerShape);
		desc += PropDesc(m_innerShapeMod, "[InnerShape]", isAbilityPresent, isAbilityPresent ? robotAnimalRoar.m_innerShape : AbilityAreaShape.SingleSquare);
		return new StringBuilder().Append(desc).Append(PropDesc(m_innerShapeDamageMod, "[InnerShapeDamage]", isAbilityPresent, isAbilityPresent ? robotAnimalRoar.m_innerShapeDamage : 0)).ToString();
	}
}
