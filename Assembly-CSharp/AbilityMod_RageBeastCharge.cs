using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_RageBeastCharge : AbilityMod
{
	[Header("-- Damage Mod")]
	public AbilityModPropertyInt m_damageMod;
	public AbilityModPropertyInt m_damageNearChargeEndMod;
	[Header("-- On Hit Effect")]
	public StandardEffectInfo m_effectOnTargetsHit;
	public AbilityModPropertyEffectInfo m_enemyHitEffectNearChargeEndMod;
	[Header("-- Targeting Mod")]
	public AbilityModPropertyFloat m_chargeLineRadiusMod;
	public AbilityModPropertyFloat m_chargeEndRadius;

	public override Type GetTargetAbilityType()
	{
		return typeof(RageBeastCharge);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RageBeastCharge rageBeastCharge = targetAbility as RageBeastCharge;
		if (rageBeastCharge != null)
		{
			AddToken(tokens, m_damageMod, "DamageAmount", string.Empty, rageBeastCharge.m_damageAmount);
			AddToken(tokens, m_damageNearChargeEndMod, "DamageNearChargeEnd", string.Empty, rageBeastCharge.m_damageNearChargeEnd);
			AddToken(tokens, m_chargeLineRadiusMod, "DamageRadius", string.Empty, rageBeastCharge.m_damageRadius);
			AddToken(tokens, m_chargeEndRadius, "RadiusBeyondEnd", string.Empty, rageBeastCharge.m_radiusBeyondEnd);
			AddToken_EffectMod(tokens, m_enemyHitEffectNearChargeEndMod, "EnemyHitEffectNearChargeEnd", rageBeastCharge.m_enemyHitEffectNearChargeEnd);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RageBeastCharge rageBeastCharge = GetTargetAbilityOnAbilityData(abilityData) as RageBeastCharge;
		bool isValid = rageBeastCharge != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage Mod]", isValid, isValid ? rageBeastCharge.m_damageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_damageNearChargeEndMod, "[Damage Near Charge End Mod]", isValid, isValid ? rageBeastCharge.m_damageNearChargeEnd : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_chargeLineRadiusMod, "[Charge Line Radius/Half-Width Mod]", isValid, isValid ? rageBeastCharge.m_damageRadius : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_chargeEndRadius, "[Charge End Radius Mod]", isValid, isValid ? rageBeastCharge.m_radiusBeyondEnd : 0f);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_effectOnTargetsHit, "{ Effect on Target Hit }", string.Empty, isValid);
		return desc + PropDesc(m_enemyHitEffectNearChargeEndMod, "[EnemyHitEffectNearChargeEnd]", isValid, isValid ? rageBeastCharge.m_enemyHitEffectNearChargeEnd : null);
	}
}
