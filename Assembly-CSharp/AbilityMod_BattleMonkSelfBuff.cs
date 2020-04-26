using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BattleMonkSelfBuff : AbilityMod
{
	[Header("-- Thorn Shield Mod")]
	public AbilityModPropertyInt m_absorbMod;

	public AbilityModPropertyInt m_damageReturnMod;

	public AbilityModPropertyEffectInfo m_returnEffectOnEnemyMod;

	[Header("-- Ally Hit Mod")]
	public AbilityModPropertyBool m_hitNearbyAlliesMod;

	public AbilityModPropertyShape m_allyTargetShapeMod;

	public StandardEffectInfo m_effectOnAllyHit;

	[Header("-- Duration of Effect On Self Per Hit Received This Turn")]
	public AbilityModPropertyInt m_selfEffectDurationPerHit;

	public StandardEffectInfo m_effectOnSelfNextTurn;

	public override Type GetTargetAbilityType()
	{
		return typeof(BattleMonkSelfBuff);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BattleMonkSelfBuff battleMonkSelfBuff = targetAbility as BattleMonkSelfBuff;
		if (!(battleMonkSelfBuff != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_absorbMod, "Absorb", string.Empty, battleMonkSelfBuff.m_standardActorEffectData.m_absorbAmount);
			AbilityMod.AddToken(tokens, m_damageReturnMod, "DamagePerHit", string.Empty, battleMonkSelfBuff.m_damagePerHit);
			AbilityMod.AddToken_EffectMod(tokens, m_returnEffectOnEnemyMod, "ReturnEffectOnEnemy", battleMonkSelfBuff.m_returnEffectOnEnemy);
			if (m_hitNearbyAlliesMod == null)
			{
				return;
			}
			while (true)
			{
				if (m_hitNearbyAlliesMod.GetModifiedValue(false))
				{
					while (true)
					{
						AbilityMod.AddToken_EffectInfo(tokens, m_effectOnAllyHit, "EffectOnAllyHit", null, false);
						return;
					}
				}
				return;
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BattleMonkSelfBuff battleMonkSelfBuff = GetTargetAbilityOnAbilityData(abilityData) as BattleMonkSelfBuff;
		bool flag = battleMonkSelfBuff != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt absorbMod = m_absorbMod;
		int baseVal;
		if (flag)
		{
			baseVal = battleMonkSelfBuff.m_standardActorEffectData.m_absorbAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(absorbMod, "[Absorb]", flag, baseVal);
		empty += AbilityModHelper.GetModPropertyDesc(m_damageReturnMod, "[Damage Return]", flag, flag ? battleMonkSelfBuff.m_damagePerHit : 0);
		empty += PropDesc(m_returnEffectOnEnemyMod, "[ReturnEffectOnEnemy]", flag, (!flag) ? null : battleMonkSelfBuff.m_returnEffectOnEnemy);
		empty += AbilityModHelper.GetModPropertyDesc(m_hitNearbyAlliesMod, "[Hit Nearby Allies?]", flag);
		empty += AbilityModHelper.GetModPropertyDesc(m_allyTargetShapeMod, "[Ally Target Shape]", flag);
		empty += AbilityModHelper.GetModEffectInfoDesc(m_effectOnAllyHit, "{ Effect on Ally Hit }", string.Empty, flag);
		empty += PropDesc(m_selfEffectDurationPerHit, "[Duration of Effect On Self Per Received Hit]", flag);
		return empty + AbilityModHelper.GetModEffectInfoDesc(m_effectOnSelfNextTurn, "{ Effect on Self Next Turn }", string.Empty, flag);
	}
}
