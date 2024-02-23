using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_RobotAnimalDrag : AbilityMod
{
	[Header("-- Leash Targeting Mod")]
	public AbilityModPropertyFloat m_distanceMod;
	public AbilityModPropertyFloat m_widthMod;
	[Header("-- Damage")]
	public AbilityModPropertyInt m_damageMod;
	[Header("-- Effect on Enemy Hit")]
	public StandardEffectInfo m_enemyHitEffectOverride;
	[Header("-- Effect on Enemy on Next Turn Start")]
	public StandardEffectInfo m_enemyEffectOnNextTurnStart;
	[Header("-- Effect on Self")]
	public AbilityModPropertyEffectInfo m_casterEffectMod;
	[Header("-- Spoils to Spawn Around the Destination")]
	public List<PowerUp> m_powerUpsToSpawn;
	public AbilityAreaShape m_powerUpsSpawnShape;

	public override Type GetTargetAbilityType()
	{
		return typeof(RobotAnimalDrag);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RobotAnimalDrag robotAnimalDrag = targetAbility as RobotAnimalDrag;
		if (robotAnimalDrag != null)
		{
			AddToken(tokens, m_distanceMod, "Distance", string.Empty, robotAnimalDrag.m_distance);
			AddToken(tokens, m_widthMod, "Width", string.Empty, robotAnimalDrag.m_width);
			AddToken(tokens, m_damageMod, "Damage", string.Empty, robotAnimalDrag.m_damage);
			AddToken_EffectMod(tokens, m_casterEffectMod, "CasterEffect", robotAnimalDrag.m_casterEffect);
			AddToken_EffectInfo(tokens, m_enemyHitEffectOverride, "EnemyEffectOnHit");
			AddToken_EffectInfo(tokens, m_enemyEffectOnNextTurnStart, "EnemyEffectOnTurnStart");
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RobotAnimalDrag robotAnimalDrag = GetTargetAbilityOnAbilityData(abilityData) as RobotAnimalDrag;
		bool isAbilityPresent = robotAnimalDrag != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_distanceMod, "[Targeting Distance]", isAbilityPresent, isAbilityPresent ? robotAnimalDrag.m_distance : 0f);
		desc += PropDesc(m_widthMod, "[Width]", isAbilityPresent, isAbilityPresent ? robotAnimalDrag.m_width : 0f);
		desc += PropDesc(m_damageMod, "[Damage]", isAbilityPresent, isAbilityPresent ? robotAnimalDrag.m_damage : 0);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_enemyHitEffectOverride, "{ Effect on Enemy on Hit }", string.Empty, isAbilityPresent);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_enemyEffectOnNextTurnStart, "{ Effect on Enemy on start of Next Turn }", string.Empty, isAbilityPresent);
		desc += PropDesc(m_casterEffectMod, "[CasterEffect]", isAbilityPresent, isAbilityPresent ? robotAnimalDrag.m_casterEffect : null);
		if (m_powerUpsToSpawn != null && m_powerUpsToSpawn.Count > 0)
		{
			desc += "[Spoils to Spawn] = ";
			foreach (PowerUp powerUp in m_powerUpsToSpawn)
			{
				if (powerUp != null)
				{
					desc += new StringBuilder().Append(powerUp.name).Append(", ").ToString();
				}
			}
			desc += "\n";
			desc += new StringBuilder().Append("     in shape ").Append(m_powerUpsSpawnShape).Append("\n").ToString();
		}
		return desc;
	}
}
