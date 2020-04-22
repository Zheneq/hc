using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ThiefOnTheRun : AbilityMod
{
	[Header("-- Targeter")]
	public AbilityModPropertyFloat m_minDistanceBetweenStepsMod;

	public AbilityModPropertyFloat m_minDistanceBetweenAnyStepsMod;

	public AbilityModPropertyFloat m_maxDistanceBetweenStepsMod;

	[Header("-- Dash Hit Size")]
	public AbilityModPropertyFloat m_dashRadiusMod;

	public AbilityModPropertyBool m_dashPenetrateLineOfSightMod;

	[Header("-- Hit Damage and Effect")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyInt m_subsequentDamageMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	[Header("-- Hid On Self")]
	public AbilityModPropertyEffectInfo m_effectOnSelfThroughSmokeFieldMod;

	public AbilityModPropertyInt m_cooldownReductionIfNoEnemyMod;

	[Header("-- Spoil Powerup Spawn")]
	public AbilityModPropertySpoilsSpawnData m_spoilSpawnInfoMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ThiefOnTheRun);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ThiefOnTheRun thiefOnTheRun = targetAbility as ThiefOnTheRun;
		if (thiefOnTheRun != null)
		{
			AbilityMod.AddToken(tokens, m_minDistanceBetweenStepsMod, "MinDistanceBetweenSteps", string.Empty, thiefOnTheRun.m_minDistanceBetweenSteps);
			AbilityMod.AddToken(tokens, m_minDistanceBetweenAnyStepsMod, "MinDistanceBetweenAnySteps", string.Empty, thiefOnTheRun.m_minDistanceBetweenAnySteps);
			AbilityMod.AddToken(tokens, m_maxDistanceBetweenStepsMod, "MaxDistanceBetweenSteps", string.Empty, thiefOnTheRun.m_maxDistanceBetweenSteps);
			AbilityMod.AddToken(tokens, m_dashRadiusMod, "DashRadius", string.Empty, thiefOnTheRun.m_dashRadius);
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, thiefOnTheRun.m_damageAmount);
			AbilityMod.AddToken(tokens, m_subsequentDamageMod, "SubsequentDamage", string.Empty, thiefOnTheRun.m_subsequentDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", thiefOnTheRun.m_enemyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnSelfThroughSmokeFieldMod, "EffectOnSelfThroughSmokeField", thiefOnTheRun.m_effectOnSelfThroughSmokeField);
			AbilityMod.AddToken(tokens, m_cooldownReductionIfNoEnemyMod, "CooldownReductionIfNoEnemy", string.Empty, thiefOnTheRun.m_cooldownReductionIfNoEnemy);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ThiefOnTheRun thiefOnTheRun = GetTargetAbilityOnAbilityData(abilityData) as ThiefOnTheRun;
		bool flag = thiefOnTheRun != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat minDistanceBetweenStepsMod = m_minDistanceBetweenStepsMod;
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
			baseVal = thiefOnTheRun.m_minDistanceBetweenSteps;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(minDistanceBetweenStepsMod, "[MinDistanceBetweenSteps]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat minDistanceBetweenAnyStepsMod = m_minDistanceBetweenAnyStepsMod;
		float baseVal2;
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
			baseVal2 = thiefOnTheRun.m_minDistanceBetweenAnySteps;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(minDistanceBetweenAnyStepsMod, "[MinDistanceBetweenAnySteps]", flag, baseVal2);
		empty += PropDesc(m_maxDistanceBetweenStepsMod, "[MaxDistanceBetweenSteps]", flag, (!flag) ? 0f : thiefOnTheRun.m_maxDistanceBetweenSteps);
		string str3 = empty;
		AbilityModPropertyFloat dashRadiusMod = m_dashRadiusMod;
		float baseVal3;
		if (flag)
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
			baseVal3 = thiefOnTheRun.m_dashRadius;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(dashRadiusMod, "[DashRadius]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyBool dashPenetrateLineOfSightMod = m_dashPenetrateLineOfSightMod;
		int baseVal4;
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
			baseVal4 = (thiefOnTheRun.m_dashPenetrateLineOfSight ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(dashPenetrateLineOfSightMod, "[DashPenetrateLineOfSight]", flag, (byte)baseVal4 != 0);
		string str5 = empty;
		AbilityModPropertyInt damageAmountMod = m_damageAmountMod;
		int baseVal5;
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
			baseVal5 = thiefOnTheRun.m_damageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(damageAmountMod, "[DamageAmount]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyInt subsequentDamageMod = m_subsequentDamageMod;
		int baseVal6;
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
			baseVal6 = thiefOnTheRun.m_subsequentDamage;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(subsequentDamageMod, "[SubsequentDamage]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectMod = m_enemyHitEffectMod;
		object baseVal7;
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
			baseVal7 = thiefOnTheRun.m_enemyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + PropDesc(enemyHitEffectMod, "[EnemyHitEffect]", flag, (StandardEffectInfo)baseVal7);
		string str8 = empty;
		AbilityModPropertyEffectInfo effectOnSelfThroughSmokeFieldMod = m_effectOnSelfThroughSmokeFieldMod;
		object baseVal8;
		if (flag)
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
			baseVal8 = thiefOnTheRun.m_effectOnSelfThroughSmokeField;
		}
		else
		{
			baseVal8 = null;
		}
		empty = str8 + PropDesc(effectOnSelfThroughSmokeFieldMod, "[EffectOnSelfThroughSmokeField]", flag, (StandardEffectInfo)baseVal8);
		string str9 = empty;
		AbilityModPropertyInt cooldownReductionIfNoEnemyMod = m_cooldownReductionIfNoEnemyMod;
		int baseVal9;
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
			baseVal9 = thiefOnTheRun.m_cooldownReductionIfNoEnemy;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(cooldownReductionIfNoEnemyMod, "[CooldownReductionIfNoEnemy]", flag, baseVal9);
		return empty + PropDesc(m_spoilSpawnInfoMod, "[SpoilSpawnInfo]", flag, (!flag) ? null : thiefOnTheRun.m_spoilSpawnInfo);
	}
}
