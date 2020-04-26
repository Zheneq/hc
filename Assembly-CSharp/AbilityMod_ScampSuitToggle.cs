using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ScampSuitToggle : AbilityMod
{
	[Separator("Whether shield down mode is free action", true)]
	public AbilityModPropertyBool m_shieldDownModeFreeActionMod;

	[Separator("Cooldowns", true)]
	public AbilityModPropertyInt m_cooldownCreateSuitMod;

	public AbilityModPropertyInt m_cooldownRefillShieldMod;

	[Header("-- Cooldown override for when suit is destroyed")]
	public AbilityModPropertyInt m_cooldownOverrideOnSuitDestroyMod;

	[Separator("Energy to Shield (shield = energy x multiplier)", true)]
	public AbilityModPropertyFloat m_energyToShieldMultMod;

	[Separator("Clear Energy Orbs on cast", true)]
	public AbilityModPropertyBool m_clearEnergyOrbsOnCastMod;

	[Separator("Extra Orbs to spawn on suit lost", true)]
	public AbilityModPropertyInt m_extraOrbsToSpawnOnSuitLostMod;

	[Separator("Passive Energy Regen", true)]
	public AbilityModPropertyInt m_passiveEnergyRegenMod;

	[Separator("Effect to apply when suit is gained or lost (applied on start of turn)", true)]
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
		if (!(scampSuitToggle != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_cooldownCreateSuitMod, "CooldownCreateSuit", string.Empty, scampSuitToggle.m_cooldownCreateSuit);
			AbilityMod.AddToken(tokens, m_cooldownRefillShieldMod, "CooldownRefillShield", string.Empty, scampSuitToggle.m_cooldownRefillShield);
			AbilityMod.AddToken(tokens, m_cooldownOverrideOnSuitDestroyMod, "CooldownOverrideOnSuitDestroy", string.Empty, scampSuitToggle.m_cooldownOverrideOnSuitDestroy);
			AbilityMod.AddToken(tokens, m_energyToShieldMultMod, "EnergyToShieldMult", string.Empty, scampSuitToggle.m_energyToShieldMult);
			AbilityMod.AddToken(tokens, m_extraOrbsToSpawnOnSuitLostMod, "ExtraOrbsToSpawnOnSuitLost", string.Empty, scampSuitToggle.m_extraOrbsToSpawnOnSuitLost);
			AbilityMod.AddToken(tokens, m_passiveEnergyRegenMod, "PassiveEnergyRegen", string.Empty, scampSuitToggle.m_passiveEnergyRegen);
			AbilityMod.AddToken_EffectMod(tokens, m_effectForSuitGainedMod, "EffectForSuitGained", scampSuitToggle.m_effectForSuitGained);
			AbilityMod.AddToken_EffectMod(tokens, m_effectForSuitLostMod, "EffectForSuitLost", scampSuitToggle.m_effectForSuitLost);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScampSuitToggle scampSuitToggle = GetTargetAbilityOnAbilityData(abilityData) as ScampSuitToggle;
		bool flag = scampSuitToggle != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyBool shieldDownModeFreeActionMod = m_shieldDownModeFreeActionMod;
		int baseVal;
		if (flag)
		{
			baseVal = (scampSuitToggle.m_shieldDownModeFreeAction ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(shieldDownModeFreeActionMod, "[ShieldDownModeFreeAction]", flag, (byte)baseVal != 0);
		string str2 = empty;
		AbilityModPropertyInt cooldownCreateSuitMod = m_cooldownCreateSuitMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = scampSuitToggle.m_cooldownCreateSuit;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(cooldownCreateSuitMod, "[CooldownCreateSuit]", flag, baseVal2);
		empty += PropDesc(m_cooldownRefillShieldMod, "[CooldownRefillShield]", flag, flag ? scampSuitToggle.m_cooldownRefillShield : 0);
		string str3 = empty;
		AbilityModPropertyInt cooldownOverrideOnSuitDestroyMod = m_cooldownOverrideOnSuitDestroyMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = scampSuitToggle.m_cooldownOverrideOnSuitDestroy;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(cooldownOverrideOnSuitDestroyMod, "[CooldownOverrideOnSuitDestroy]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat energyToShieldMultMod = m_energyToShieldMultMod;
		float baseVal4;
		if (flag)
		{
			baseVal4 = scampSuitToggle.m_energyToShieldMult;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(energyToShieldMultMod, "[EnergyToShieldMult]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyBool clearEnergyOrbsOnCastMod = m_clearEnergyOrbsOnCastMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = (scampSuitToggle.m_clearEnergyOrbsOnCast ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(clearEnergyOrbsOnCastMod, "[ClearEnergyOrbsOnCast]", flag, (byte)baseVal5 != 0);
		empty += PropDesc(m_extraOrbsToSpawnOnSuitLostMod, "[ExtraOrbsToSpawnOnSuitLost]", flag, flag ? scampSuitToggle.m_extraOrbsToSpawnOnSuitLost : 0);
		string str6 = empty;
		AbilityModPropertyInt passiveEnergyRegenMod = m_passiveEnergyRegenMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = scampSuitToggle.m_passiveEnergyRegen;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(passiveEnergyRegenMod, "[PassiveEnergyRegen]", flag, baseVal6);
		empty += PropDesc(m_considerRespawnForSuitGainEffectMod, "[ConsiderRespawnForSuitGainEffect]", flag, flag && scampSuitToggle.m_considerRespawnForSuitGainEffect);
		string str7 = empty;
		AbilityModPropertyEffectInfo effectForSuitGainedMod = m_effectForSuitGainedMod;
		object baseVal7;
		if (flag)
		{
			baseVal7 = scampSuitToggle.m_effectForSuitGained;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + PropDesc(effectForSuitGainedMod, "[EffectForSuitGained]", flag, (StandardEffectInfo)baseVal7);
		string str8 = empty;
		AbilityModPropertyEffectInfo effectForSuitLostMod = m_effectForSuitLostMod;
		object baseVal8;
		if (flag)
		{
			baseVal8 = scampSuitToggle.m_effectForSuitLost;
		}
		else
		{
			baseVal8 = null;
		}
		return str8 + PropDesc(effectForSuitLostMod, "[EffectForSuitLost]", flag, (StandardEffectInfo)baseVal8);
	}
}
