// ROGUES
// SERVER
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
		if (battleMonkSelfBuff != null)
		{
			AddToken(tokens, m_absorbMod, "Absorb", string.Empty, battleMonkSelfBuff.m_standardActorEffectData.m_absorbAmount);
			AddToken(tokens, m_damageReturnMod, "DamagePerHit", string.Empty, battleMonkSelfBuff.m_damagePerHit);
			AddToken_EffectMod(tokens, m_returnEffectOnEnemyMod, "ReturnEffectOnEnemy", battleMonkSelfBuff.m_returnEffectOnEnemy);
			if (m_hitNearbyAlliesMod != null && m_hitNearbyAlliesMod.GetModifiedValue(false))
			{
				AddToken_EffectInfo(tokens, m_effectOnAllyHit, "EffectOnAllyHit", null, false);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)  // , Ability targetAbility in rogues
	{
		// reactor
		BattleMonkSelfBuff battleMonkSelfBuff = GetTargetAbilityOnAbilityData(abilityData) as BattleMonkSelfBuff;
		// rogues
		//BattleMonkSelfBuff battleMonkSelfBuff = targetAbility as BattleMonkSelfBuff;
		
		bool isAbilityPresent = battleMonkSelfBuff != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_absorbMod, "[Absorb]", isAbilityPresent, isAbilityPresent ? battleMonkSelfBuff.m_standardActorEffectData.m_absorbAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_damageReturnMod, "[Damage Return]", isAbilityPresent, isAbilityPresent ? battleMonkSelfBuff.m_damagePerHit : 0);
		desc += PropDesc(m_returnEffectOnEnemyMod, "[ReturnEffectOnEnemy]", isAbilityPresent, isAbilityPresent ? battleMonkSelfBuff.m_returnEffectOnEnemy : null);
		desc += AbilityModHelper.GetModPropertyDesc(m_hitNearbyAlliesMod, "[Hit Nearby Allies?]", isAbilityPresent);
		desc += AbilityModHelper.GetModPropertyDesc(m_allyTargetShapeMod, "[Ally Target Shape]", isAbilityPresent);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_effectOnAllyHit, "{ Effect on Ally Hit }", string.Empty, isAbilityPresent);
		desc += PropDesc(m_selfEffectDurationPerHit, "[Duration of Effect On Self Per Received Hit]", isAbilityPresent);
		return desc + AbilityModHelper.GetModEffectInfoDesc(m_effectOnSelfNextTurn, "{ Effect on Self Next Turn }", string.Empty, isAbilityPresent);
	}
}
