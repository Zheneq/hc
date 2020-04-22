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
		if (!(robotAnimalDrag != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_distanceMod, "Distance", string.Empty, robotAnimalDrag.m_distance);
			AbilityMod.AddToken(tokens, m_widthMod, "Width", string.Empty, robotAnimalDrag.m_width);
			AbilityMod.AddToken(tokens, m_damageMod, "Damage", string.Empty, robotAnimalDrag.m_damage);
			AbilityMod.AddToken_EffectMod(tokens, m_casterEffectMod, "CasterEffect", robotAnimalDrag.m_casterEffect);
			AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffectOverride, "EnemyEffectOnHit");
			AbilityMod.AddToken_EffectInfo(tokens, m_enemyEffectOnNextTurnStart, "EnemyEffectOnTurnStart");
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RobotAnimalDrag robotAnimalDrag = GetTargetAbilityOnAbilityData(abilityData) as RobotAnimalDrag;
		bool flag = robotAnimalDrag != null;
		string empty = string.Empty;
		empty += AbilityModHelper.GetModPropertyDesc(m_distanceMod, "[Targeting Distance]", flag, (!flag) ? 0f : robotAnimalDrag.m_distance);
		string str = empty;
		AbilityModPropertyFloat widthMod = m_widthMod;
		float baseVal;
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
			baseVal = robotAnimalDrag.m_width;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(widthMod, "[Width]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal2;
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
			baseVal2 = robotAnimalDrag.m_damage;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(damageMod, "[Damage]", flag, baseVal2);
		empty += AbilityModHelper.GetModEffectInfoDesc(m_enemyHitEffectOverride, "{ Effect on Enemy on Hit }", string.Empty, flag);
		empty += AbilityModHelper.GetModEffectInfoDesc(m_enemyEffectOnNextTurnStart, "{ Effect on Enemy on start of Next Turn }", string.Empty, flag);
		string str3 = empty;
		AbilityModPropertyEffectInfo casterEffectMod = m_casterEffectMod;
		object baseVal3;
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = robotAnimalDrag.m_casterEffect;
		}
		else
		{
			baseVal3 = null;
		}
		empty = str3 + PropDesc(casterEffectMod, "[CasterEffect]", flag, (StandardEffectInfo)baseVal3);
		if (m_powerUpsToSpawn != null)
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
			if (m_powerUpsToSpawn.Count > 0)
			{
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				empty += "[Spoils to Spawn] = ";
				using (List<PowerUp>.Enumerator enumerator = m_powerUpsToSpawn.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						PowerUp current = enumerator.Current;
						if (current != null)
						{
							while (true)
							{
								switch (1)
								{
								case 0:
									continue;
								}
								break;
							}
							empty = empty + current.name + ", ";
						}
					}
					while (true)
					{
						switch (7)
						{
						case 0:
							continue;
						}
						break;
					}
				}
				empty += "\n";
				empty = empty + "     in shape " + m_powerUpsSpawnShape.ToString() + "\n";
			}
		}
		return empty;
	}
}
