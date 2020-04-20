using System;
using System.Collections.Generic;
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
			AbilityMod.AddToken(tokens, this.m_distanceMod, "Distance", string.Empty, robotAnimalDrag.m_distance, true, false, false);
			AbilityMod.AddToken(tokens, this.m_widthMod, "Width", string.Empty, robotAnimalDrag.m_width, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageMod, "Damage", string.Empty, robotAnimalDrag.m_damage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_casterEffectMod, "CasterEffect", robotAnimalDrag.m_casterEffect, true);
			AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyHitEffectOverride, "EnemyEffectOnHit", null, true);
			AbilityMod.AddToken_EffectInfo(tokens, this.m_enemyEffectOnNextTurnStart, "EnemyEffectOnTurnStart", null, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RobotAnimalDrag robotAnimalDrag = base.GetTargetAbilityOnAbilityData(abilityData) as RobotAnimalDrag;
		bool flag = robotAnimalDrag != null;
		string text = string.Empty;
		text += AbilityModHelper.GetModPropertyDesc(this.m_distanceMod, "[Targeting Distance]", flag, (!flag) ? 0f : robotAnimalDrag.m_distance);
		string str = text;
		AbilityModPropertyFloat widthMod = this.m_widthMod;
		string prefix = "[Width]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = robotAnimalDrag.m_width;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(widthMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix2 = "[Damage]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = robotAnimalDrag.m_damage;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(damageMod, prefix2, showBaseVal2, baseVal2);
		text += AbilityModHelper.GetModEffectInfoDesc(this.m_enemyHitEffectOverride, "{ Effect on Enemy on Hit }", string.Empty, flag, null);
		text += AbilityModHelper.GetModEffectInfoDesc(this.m_enemyEffectOnNextTurnStart, "{ Effect on Enemy on start of Next Turn }", string.Empty, flag, null);
		string str3 = text;
		AbilityModPropertyEffectInfo casterEffectMod = this.m_casterEffectMod;
		string prefix3 = "[CasterEffect]";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal3;
		if (flag)
		{
			baseVal3 = robotAnimalDrag.m_casterEffect;
		}
		else
		{
			baseVal3 = null;
		}
		text = str3 + base.PropDesc(casterEffectMod, prefix3, showBaseVal3, baseVal3);
		if (this.m_powerUpsToSpawn != null)
		{
			if (this.m_powerUpsToSpawn.Count > 0)
			{
				text += "[Spoils to Spawn] = ";
				using (List<PowerUp>.Enumerator enumerator = this.m_powerUpsToSpawn.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PowerUp powerUp = enumerator.Current;
						if (powerUp != null)
						{
							text = text + powerUp.name + ", ";
						}
					}
				}
				text += "\n";
				text = text + "     in shape " + this.m_powerUpsSpawnShape.ToString() + "\n";
			}
		}
		return text;
	}
}
