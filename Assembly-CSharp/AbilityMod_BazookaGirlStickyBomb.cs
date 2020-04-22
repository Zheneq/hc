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
		if (!(bazookaGirlStickyBomb != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_energyGainOnCastPerEnemyHitMod, "EnergyGainOnCastPerEnemyHit", string.Empty, bazookaGirlStickyBomb.m_energyGainOnCastPerEnemyHit);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyOnCastHitEffectOverride, "EnemyOnCastHitEffect", bazookaGirlStickyBomb.m_enemyOnCastHitEffect);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BazookaGirlStickyBomb bazookaGirlStickyBomb = GetTargetAbilityOnAbilityData(abilityData) as BazookaGirlStickyBomb;
		bool flag = bazookaGirlStickyBomb != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt energyGainOnCastPerEnemyHitMod = m_energyGainOnCastPerEnemyHitMod;
		int baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = bazookaGirlStickyBomb.m_energyGainOnCastPerEnemyHit;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(energyGainOnCastPerEnemyHitMod, "[EnergyGainOnCastPerEnemyHit]", flag, baseVal);
		empty += AbilityModHelper.GetModPropertyDesc(m_enemyOnCastHitEffectOverride, "{ Enemy On Cast Hit Effect }", flag, (!flag) ? null : bazookaGirlStickyBomb.m_enemyOnCastHitEffect);
		string str2 = empty;
		AbilityModPropertyEffectInfo enemyOnExplosionEffectOverride = m_enemyOnExplosionEffectOverride;
		object baseVal2;
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
			baseVal2 = bazookaGirlStickyBomb.m_bombInfo.onExplodeEffect;
		}
		else
		{
			baseVal2 = null;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(enemyOnExplosionEffectOverride, "{ Enemy on Explode Effect }", flag, (StandardEffectInfo)baseVal2);
		if (m_cooldownModOnAction != AbilityData.ActionType.INVALID_ACTION)
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
			if (m_cooldownAddAmount != 0)
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
				string text = empty;
				object[] obj = new object[7]
				{
					text,
					null,
					null,
					null,
					null,
					null,
					null
				};
				object obj2;
				if (m_cooldownAddAmount < 0)
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
					obj2 = "Reduces";
				}
				else
				{
					obj2 = "Increases";
				}
				obj[1] = obj2;
				obj[2] = " cooldown on ";
				obj[3] = AbilityModHelper.GetAbilityNameFromActionType(m_cooldownModOnAction, abilityData);
				obj[4] = " by ";
				obj[5] = Mathf.Abs(m_cooldownAddAmount);
				obj[6] = " per explosion";
				empty = string.Concat(obj);
			}
		}
		return empty;
	}
}
