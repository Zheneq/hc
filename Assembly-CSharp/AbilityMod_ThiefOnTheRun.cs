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
			AbilityMod.AddToken(tokens, this.m_minDistanceBetweenStepsMod, "MinDistanceBetweenSteps", string.Empty, thiefOnTheRun.m_minDistanceBetweenSteps, true, false, false);
			AbilityMod.AddToken(tokens, this.m_minDistanceBetweenAnyStepsMod, "MinDistanceBetweenAnySteps", string.Empty, thiefOnTheRun.m_minDistanceBetweenAnySteps, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxDistanceBetweenStepsMod, "MaxDistanceBetweenSteps", string.Empty, thiefOnTheRun.m_maxDistanceBetweenSteps, true, false, false);
			AbilityMod.AddToken(tokens, this.m_dashRadiusMod, "DashRadius", string.Empty, thiefOnTheRun.m_dashRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, thiefOnTheRun.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_subsequentDamageMod, "SubsequentDamage", string.Empty, thiefOnTheRun.m_subsequentDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", thiefOnTheRun.m_enemyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnSelfThroughSmokeFieldMod, "EffectOnSelfThroughSmokeField", thiefOnTheRun.m_effectOnSelfThroughSmokeField, true);
			AbilityMod.AddToken(tokens, this.m_cooldownReductionIfNoEnemyMod, "CooldownReductionIfNoEnemy", string.Empty, thiefOnTheRun.m_cooldownReductionIfNoEnemy, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ThiefOnTheRun thiefOnTheRun = base.GetTargetAbilityOnAbilityData(abilityData) as ThiefOnTheRun;
		bool flag = thiefOnTheRun != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat minDistanceBetweenStepsMod = this.m_minDistanceBetweenStepsMod;
		string prefix = "[MinDistanceBetweenSteps]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ThiefOnTheRun.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = thiefOnTheRun.m_minDistanceBetweenSteps;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(minDistanceBetweenStepsMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat minDistanceBetweenAnyStepsMod = this.m_minDistanceBetweenAnyStepsMod;
		string prefix2 = "[MinDistanceBetweenAnySteps]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			for (;;)
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
		text = str2 + base.PropDesc(minDistanceBetweenAnyStepsMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_maxDistanceBetweenStepsMod, "[MaxDistanceBetweenSteps]", flag, (!flag) ? 0f : thiefOnTheRun.m_maxDistanceBetweenSteps);
		string str3 = text;
		AbilityModPropertyFloat dashRadiusMod = this.m_dashRadiusMod;
		string prefix3 = "[DashRadius]";
		bool showBaseVal3 = flag;
		float baseVal3;
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
			baseVal3 = thiefOnTheRun.m_dashRadius;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(dashRadiusMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyBool dashPenetrateLineOfSightMod = this.m_dashPenetrateLineOfSightMod;
		string prefix4 = "[DashPenetrateLineOfSight]";
		bool showBaseVal4 = flag;
		bool baseVal4;
		if (flag)
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
			baseVal4 = thiefOnTheRun.m_dashPenetrateLineOfSight;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + base.PropDesc(dashPenetrateLineOfSightMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt damageAmountMod = this.m_damageAmountMod;
		string prefix5 = "[DamageAmount]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			for (;;)
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
		text = str5 + base.PropDesc(damageAmountMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt subsequentDamageMod = this.m_subsequentDamageMod;
		string prefix6 = "[SubsequentDamage]";
		bool showBaseVal6 = flag;
		int baseVal6;
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
			baseVal6 = thiefOnTheRun.m_subsequentDamage;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(subsequentDamageMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyEffectInfo enemyHitEffectMod = this.m_enemyHitEffectMod;
		string prefix7 = "[EnemyHitEffect]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
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
			baseVal7 = thiefOnTheRun.m_enemyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + base.PropDesc(enemyHitEffectMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyEffectInfo effectOnSelfThroughSmokeFieldMod = this.m_effectOnSelfThroughSmokeFieldMod;
		string prefix8 = "[EffectOnSelfThroughSmokeField]";
		bool showBaseVal8 = flag;
		StandardEffectInfo baseVal8;
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
			baseVal8 = thiefOnTheRun.m_effectOnSelfThroughSmokeField;
		}
		else
		{
			baseVal8 = null;
		}
		text = str8 + base.PropDesc(effectOnSelfThroughSmokeFieldMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyInt cooldownReductionIfNoEnemyMod = this.m_cooldownReductionIfNoEnemyMod;
		string prefix9 = "[CooldownReductionIfNoEnemy]";
		bool showBaseVal9 = flag;
		int baseVal9;
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
			baseVal9 = thiefOnTheRun.m_cooldownReductionIfNoEnemy;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(cooldownReductionIfNoEnemyMod, prefix9, showBaseVal9, baseVal9);
		return text + base.PropDesc(this.m_spoilSpawnInfoMod, "[SpoilSpawnInfo]", flag, (!flag) ? null : thiefOnTheRun.m_spoilSpawnInfo);
	}
}
