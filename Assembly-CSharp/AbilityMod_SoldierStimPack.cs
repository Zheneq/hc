using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SoldierStimPack : AbilityMod
{
	[Header("-- On Hit Stuff --")]
	public AbilityModPropertyInt m_selfHealAmountMod;
	public AbilityModPropertyEffectInfo m_selfHitEffectMod;
	[Header("-- For other abilities when active --")]
	public AbilityModPropertyBool m_basicAttackIgnoreCoverMod;
	public AbilityModPropertyBool m_basicAttackReduceCoverEffectivenessMod;
	public AbilityModPropertyFloat m_grenadeExtraRangeMod;
	public AbilityModPropertyEffectInfo m_dashShootExtraEffectMod;
	[Header("-- Health threshold to trigger cooldown reset, value:(0-1)")]
	public AbilityModPropertyFloat m_cooldownResetHealthThresholdMod;
	[Header("-- CDR - if dash and shoot used on same turn")]
	public AbilityModPropertyInt m_cdrIfDashAndShootUsedMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SoldierStimPack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SoldierStimPack soldierStimPack = targetAbility as SoldierStimPack;
		if (soldierStimPack != null)
		{
			AddToken(tokens, m_selfHealAmountMod, "SelfHealAmount", string.Empty, soldierStimPack.m_selfHealAmount);
			AddToken_EffectMod(tokens, m_selfHitEffectMod, "SelfHitEffect", soldierStimPack.m_selfHitEffect);
			AddToken(tokens, m_grenadeExtraRangeMod, "GrenadeExtraRange", string.Empty, soldierStimPack.m_grenadeExtraRange);
			AddToken_EffectMod(tokens, m_dashShootExtraEffectMod, "DashShootExtraEffect", soldierStimPack.m_dashShootExtraEffect);
			AddToken(tokens, m_cooldownResetHealthThresholdMod, "CooldownResetHealthThreshold", string.Empty, soldierStimPack.m_cooldownResetHealthThreshold, true, false, true);
			AddToken(tokens, m_cdrIfDashAndShootUsedMod, "CdrIfDashAndShootUsed", string.Empty, soldierStimPack.m_cdrIfDashAndShootUsed);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SoldierStimPack soldierStimPack = GetTargetAbilityOnAbilityData(abilityData) as SoldierStimPack;
		bool isValid = soldierStimPack != null;
		string desc = string.Empty;
		desc += PropDesc(m_selfHealAmountMod, "[SelfHealAmount]", isValid, isValid ? soldierStimPack.m_selfHealAmount : 0);
		desc += PropDesc(m_selfHitEffectMod, "[SelfHitEffect]", isValid, isValid ? soldierStimPack.m_selfHitEffect : null);
		desc += PropDesc(m_basicAttackIgnoreCoverMod, "[BasicAttackIgnoreCover]", isValid, isValid && soldierStimPack.m_basicAttackIgnoreCover);
		desc += PropDesc(m_basicAttackReduceCoverEffectivenessMod, "[BasicAttackReduceCoverEffectiveness]", isValid, isValid && soldierStimPack.m_basicAttackReduceCoverEffectiveness);
		desc += PropDesc(m_grenadeExtraRangeMod, "[GrenadeExtraRange]", isValid, isValid ? soldierStimPack.m_grenadeExtraRange : 0f);
		desc += PropDesc(m_dashShootExtraEffectMod, "[DashShootExtraEffect]", isValid, isValid ? soldierStimPack.m_dashShootExtraEffect : null);
		desc += PropDesc(m_cooldownResetHealthThresholdMod, "[CooldownResetHealthThreshold]", isValid, isValid ? soldierStimPack.m_cooldownResetHealthThreshold : 0f);
		return desc + PropDesc(m_cdrIfDashAndShootUsedMod, "[CdrIfDashAndShootUsed]", isValid, isValid ? soldierStimPack.m_cdrIfDashAndShootUsed : 0);
	}
}
