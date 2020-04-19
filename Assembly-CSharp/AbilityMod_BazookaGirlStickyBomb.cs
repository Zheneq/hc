using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BazookaGirlStickyBomb : AbilityMod
{
	[Header("-- On Cast Hit")]
	public AbilityModPropertyInt m_energyGainOnCastPerEnemyHitMod;

	public AbilityModPropertyEffectInfo m_enemyOnCastHitEffectOverride;

	[Header("-- On Explosion Hit Effect Override")]
	public AbilityModPropertyEffectInfo m_enemyOnExplosionEffectOverride;

	[Header("-- Cooldown modification on Explosion")]
	public AbilityData.ActionType m_cooldownModOnAction = AbilityData.ActionType.INVALID_ACTION;

	public int m_cooldownAddAmount;

	public override Type GetTargetAbilityType()
	{
		return typeof(BazookaGirlStickyBomb);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BazookaGirlStickyBomb bazookaGirlStickyBomb = targetAbility as BazookaGirlStickyBomb;
		if (bazookaGirlStickyBomb != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_BazookaGirlStickyBomb.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_energyGainOnCastPerEnemyHitMod, "EnergyGainOnCastPerEnemyHit", string.Empty, bazookaGirlStickyBomb.m_energyGainOnCastPerEnemyHit, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyOnCastHitEffectOverride, "EnemyOnCastHitEffect", bazookaGirlStickyBomb.m_enemyOnCastHitEffect, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BazookaGirlStickyBomb bazookaGirlStickyBomb = base.GetTargetAbilityOnAbilityData(abilityData) as BazookaGirlStickyBomb;
		bool flag = bazookaGirlStickyBomb != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt energyGainOnCastPerEnemyHitMod = this.m_energyGainOnCastPerEnemyHitMod;
		string prefix = "[EnergyGainOnCastPerEnemyHit]";
		bool showBaseVal = flag;
		int baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_BazookaGirlStickyBomb.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = bazookaGirlStickyBomb.m_energyGainOnCastPerEnemyHit;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(energyGainOnCastPerEnemyHitMod, prefix, showBaseVal, baseVal);
		text += AbilityModHelper.GetModPropertyDesc(this.m_enemyOnCastHitEffectOverride, "{ Enemy On Cast Hit Effect }", flag, (!flag) ? null : bazookaGirlStickyBomb.m_enemyOnCastHitEffect);
		string str2 = text;
		AbilityModPropertyEffectInfo enemyOnExplosionEffectOverride = this.m_enemyOnExplosionEffectOverride;
		string prefix2 = "{ Enemy on Explode Effect }";
		bool showBaseVal2 = flag;
		StandardEffectInfo baseVal2;
		if (flag)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = bazookaGirlStickyBomb.m_bombInfo.onExplodeEffect;
		}
		else
		{
			baseVal2 = null;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(enemyOnExplosionEffectOverride, prefix2, showBaseVal2, baseVal2);
		if (this.m_cooldownModOnAction != AbilityData.ActionType.INVALID_ACTION)
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
			if (this.m_cooldownAddAmount != 0)
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
				string text2 = text;
				object[] array = new object[7];
				array[0] = text2;
				int num = 1;
				object obj;
				if (this.m_cooldownAddAmount < 0)
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
					obj = "Reduces";
				}
				else
				{
					obj = "Increases";
				}
				array[num] = obj;
				array[2] = " cooldown on ";
				array[3] = AbilityModHelper.GetAbilityNameFromActionType(this.m_cooldownModOnAction, abilityData);
				array[4] = " by ";
				array[5] = Mathf.Abs(this.m_cooldownAddAmount);
				array[6] = " per explosion";
				text = string.Concat(array);
			}
		}
		return text;
	}
}
