using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_ScampSuitToggle : AbilityMod
{
	[Separator("Whether shield down mode is free action")]
	public AbilityModPropertyBool m_shieldDownModeFreeActionMod;
	[Separator("Cooldowns")]
	public AbilityModPropertyInt m_cooldownCreateSuitMod;
	public AbilityModPropertyInt m_cooldownRefillShieldMod;
	[Header("-- Cooldown override for when suit is destroyed")]
	public AbilityModPropertyInt m_cooldownOverrideOnSuitDestroyMod;
	[Separator("Energy to Shield (shield = energy x multiplier)")]
	public AbilityModPropertyFloat m_energyToShieldMultMod;
	[Separator("Clear Energy Orbs on cast")]
	public AbilityModPropertyBool m_clearEnergyOrbsOnCastMod;
	[Separator("Extra Orbs to spawn on suit lost")]
	public AbilityModPropertyInt m_extraOrbsToSpawnOnSuitLostMod;
	[Separator("Passive Energy Regen")]
	public AbilityModPropertyInt m_passiveEnergyRegenMod;
	[Separator("Effect to apply when suit is gained or lost (applied on start of turn)")]
	public AbilityModPropertyBool m_considerRespawnForSuitGainEffectMod;
	public AbilityModPropertyEffectInfo m_effectForSuitGainedMod;
	public AbilityModPropertyEffectInfo m_effectForSuitLostMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ScampSuitToggle);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScampSuitToggle scampSuitToggle = targetAbility as ScampSuitToggle;
		if (scampSuitToggle != null)
		{
			AddToken(tokens, m_cooldownCreateSuitMod, "CooldownCreateSuit", string.Empty, scampSuitToggle.m_cooldownCreateSuit);
			AddToken(tokens, m_cooldownRefillShieldMod, "CooldownRefillShield", string.Empty, scampSuitToggle.m_cooldownRefillShield);
			AddToken(tokens, m_cooldownOverrideOnSuitDestroyMod, "CooldownOverrideOnSuitDestroy", string.Empty, scampSuitToggle.m_cooldownOverrideOnSuitDestroy);
			AddToken(tokens, m_energyToShieldMultMod, "EnergyToShieldMult", string.Empty, scampSuitToggle.m_energyToShieldMult);
			AddToken(tokens, m_extraOrbsToSpawnOnSuitLostMod, "ExtraOrbsToSpawnOnSuitLost", string.Empty, scampSuitToggle.m_extraOrbsToSpawnOnSuitLost);
			AddToken(tokens, m_passiveEnergyRegenMod, "PassiveEnergyRegen", string.Empty, scampSuitToggle.m_passiveEnergyRegen);
			AddToken_EffectMod(tokens, m_effectForSuitGainedMod, "EffectForSuitGained", scampSuitToggle.m_effectForSuitGained);
			AddToken_EffectMod(tokens, m_effectForSuitLostMod, "EffectForSuitLost", scampSuitToggle.m_effectForSuitLost);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScampSuitToggle scampSuitToggle = GetTargetAbilityOnAbilityData(abilityData) as ScampSuitToggle;
		bool isValid = scampSuitToggle != null;
		string desc = string.Empty;
		desc += PropDesc(m_shieldDownModeFreeActionMod, "[ShieldDownModeFreeAction]", isValid, isValid && scampSuitToggle.m_shieldDownModeFreeAction);
		desc += PropDesc(m_cooldownCreateSuitMod, "[CooldownCreateSuit]", isValid, isValid ? scampSuitToggle.m_cooldownCreateSuit : 0);
		desc += PropDesc(m_cooldownRefillShieldMod, "[CooldownRefillShield]", isValid, isValid ? scampSuitToggle.m_cooldownRefillShield : 0);
		desc += PropDesc(m_cooldownOverrideOnSuitDestroyMod, "[CooldownOverrideOnSuitDestroy]", isValid, isValid ? scampSuitToggle.m_cooldownOverrideOnSuitDestroy : 0);
		desc += PropDesc(m_energyToShieldMultMod, "[EnergyToShieldMult]", isValid, isValid ? scampSuitToggle.m_energyToShieldMult : 0f);
		desc += PropDesc(m_clearEnergyOrbsOnCastMod, "[ClearEnergyOrbsOnCast]", isValid, isValid && scampSuitToggle.m_clearEnergyOrbsOnCast);
		desc += PropDesc(m_extraOrbsToSpawnOnSuitLostMod, "[ExtraOrbsToSpawnOnSuitLost]", isValid, isValid ? scampSuitToggle.m_extraOrbsToSpawnOnSuitLost : 0);
		desc += PropDesc(m_passiveEnergyRegenMod, "[PassiveEnergyRegen]", isValid, isValid ? scampSuitToggle.m_passiveEnergyRegen : 0);
		desc += PropDesc(m_considerRespawnForSuitGainEffectMod, "[ConsiderRespawnForSuitGainEffect]", isValid, isValid && scampSuitToggle.m_considerRespawnForSuitGainEffect);
		desc += PropDesc(m_effectForSuitGainedMod, "[EffectForSuitGained]", isValid, isValid ? scampSuitToggle.m_effectForSuitGained : null);
		return new StringBuilder().Append(desc).Append(PropDesc(m_effectForSuitLostMod, "[EffectForSuitLost]", isValid, isValid ? scampSuitToggle.m_effectForSuitLost : null)).ToString();
	}
}
