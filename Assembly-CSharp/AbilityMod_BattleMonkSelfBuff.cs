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
			AbilityMod.AddToken(tokens, this.m_absorbMod, "Absorb", string.Empty, battleMonkSelfBuff.m_standardActorEffectData.m_absorbAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_damageReturnMod, "DamagePerHit", string.Empty, battleMonkSelfBuff.m_damagePerHit, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_returnEffectOnEnemyMod, "ReturnEffectOnEnemy", battleMonkSelfBuff.m_returnEffectOnEnemy, true);
			if (this.m_hitNearbyAlliesMod != null)
			{
				if (this.m_hitNearbyAlliesMod.GetModifiedValue(false))
				{
					AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnAllyHit, "EffectOnAllyHit", null, false);
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BattleMonkSelfBuff battleMonkSelfBuff = base.GetTargetAbilityOnAbilityData(abilityData) as BattleMonkSelfBuff;
		bool flag = battleMonkSelfBuff != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt absorbMod = this.m_absorbMod;
		string prefix = "[Absorb]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			baseVal = battleMonkSelfBuff.m_standardActorEffectData.m_absorbAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(absorbMod, prefix, showBaseVal, baseVal);
		text += AbilityModHelper.GetModPropertyDesc(this.m_damageReturnMod, "[Damage Return]", flag, (!flag) ? 0 : battleMonkSelfBuff.m_damagePerHit);
		text += base.PropDesc(this.m_returnEffectOnEnemyMod, "[ReturnEffectOnEnemy]", flag, (!flag) ? null : battleMonkSelfBuff.m_returnEffectOnEnemy);
		text += AbilityModHelper.GetModPropertyDesc(this.m_hitNearbyAlliesMod, "[Hit Nearby Allies?]", flag, false);
		text += AbilityModHelper.GetModPropertyDesc(this.m_allyTargetShapeMod, "[Ally Target Shape]", flag, AbilityAreaShape.SingleSquare);
		text += AbilityModHelper.GetModEffectInfoDesc(this.m_effectOnAllyHit, "{ Effect on Ally Hit }", string.Empty, flag, null);
		text += base.PropDesc(this.m_selfEffectDurationPerHit, "[Duration of Effect On Self Per Received Hit]", flag, 0);
		return text + AbilityModHelper.GetModEffectInfoDesc(this.m_effectOnSelfNextTurn, "{ Effect on Self Next Turn }", string.Empty, flag, null);
	}
}
