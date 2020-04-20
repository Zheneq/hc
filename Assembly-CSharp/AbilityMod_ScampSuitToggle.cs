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
		if (scampSuitToggle != null)
		{
			AbilityMod.AddToken(tokens, this.m_cooldownCreateSuitMod, "CooldownCreateSuit", string.Empty, scampSuitToggle.m_cooldownCreateSuit, true, false);
			AbilityMod.AddToken(tokens, this.m_cooldownRefillShieldMod, "CooldownRefillShield", string.Empty, scampSuitToggle.m_cooldownRefillShield, true, false);
			AbilityMod.AddToken(tokens, this.m_cooldownOverrideOnSuitDestroyMod, "CooldownOverrideOnSuitDestroy", string.Empty, scampSuitToggle.m_cooldownOverrideOnSuitDestroy, true, false);
			AbilityMod.AddToken(tokens, this.m_energyToShieldMultMod, "EnergyToShieldMult", string.Empty, scampSuitToggle.m_energyToShieldMult, true, false, false);
			AbilityMod.AddToken(tokens, this.m_extraOrbsToSpawnOnSuitLostMod, "ExtraOrbsToSpawnOnSuitLost", string.Empty, scampSuitToggle.m_extraOrbsToSpawnOnSuitLost, true, false);
			AbilityMod.AddToken(tokens, this.m_passiveEnergyRegenMod, "PassiveEnergyRegen", string.Empty, scampSuitToggle.m_passiveEnergyRegen, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectForSuitGainedMod, "EffectForSuitGained", scampSuitToggle.m_effectForSuitGained, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectForSuitLostMod, "EffectForSuitLost", scampSuitToggle.m_effectForSuitLost, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScampSuitToggle scampSuitToggle = base.GetTargetAbilityOnAbilityData(abilityData) as ScampSuitToggle;
		bool flag = scampSuitToggle != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyBool shieldDownModeFreeActionMod = this.m_shieldDownModeFreeActionMod;
		string prefix = "[ShieldDownModeFreeAction]";
		bool showBaseVal = flag;
		bool baseVal;
		if (flag)
		{
			baseVal = scampSuitToggle.m_shieldDownModeFreeAction;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(shieldDownModeFreeActionMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt cooldownCreateSuitMod = this.m_cooldownCreateSuitMod;
		string prefix2 = "[CooldownCreateSuit]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = scampSuitToggle.m_cooldownCreateSuit;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(cooldownCreateSuitMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_cooldownRefillShieldMod, "[CooldownRefillShield]", flag, (!flag) ? 0 : scampSuitToggle.m_cooldownRefillShield);
		string str3 = text;
		AbilityModPropertyInt cooldownOverrideOnSuitDestroyMod = this.m_cooldownOverrideOnSuitDestroyMod;
		string prefix3 = "[CooldownOverrideOnSuitDestroy]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = scampSuitToggle.m_cooldownOverrideOnSuitDestroy;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(cooldownOverrideOnSuitDestroyMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat energyToShieldMultMod = this.m_energyToShieldMultMod;
		string prefix4 = "[EnergyToShieldMult]";
		bool showBaseVal4 = flag;
		float baseVal4;
		if (flag)
		{
			baseVal4 = scampSuitToggle.m_energyToShieldMult;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(energyToShieldMultMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool clearEnergyOrbsOnCastMod = this.m_clearEnergyOrbsOnCastMod;
		string prefix5 = "[ClearEnergyOrbsOnCast]";
		bool showBaseVal5 = flag;
		bool baseVal5;
		if (flag)
		{
			baseVal5 = scampSuitToggle.m_clearEnergyOrbsOnCast;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(clearEnergyOrbsOnCastMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_extraOrbsToSpawnOnSuitLostMod, "[ExtraOrbsToSpawnOnSuitLost]", flag, (!flag) ? 0 : scampSuitToggle.m_extraOrbsToSpawnOnSuitLost);
		string str6 = text;
		AbilityModPropertyInt passiveEnergyRegenMod = this.m_passiveEnergyRegenMod;
		string prefix6 = "[PassiveEnergyRegen]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = scampSuitToggle.m_passiveEnergyRegen;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(passiveEnergyRegenMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_considerRespawnForSuitGainEffectMod, "[ConsiderRespawnForSuitGainEffect]", flag, flag && scampSuitToggle.m_considerRespawnForSuitGainEffect);
		string str7 = text;
		AbilityModPropertyEffectInfo effectForSuitGainedMod = this.m_effectForSuitGainedMod;
		string prefix7 = "[EffectForSuitGained]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
		if (flag)
		{
			baseVal7 = scampSuitToggle.m_effectForSuitGained;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + base.PropDesc(effectForSuitGainedMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyEffectInfo effectForSuitLostMod = this.m_effectForSuitLostMod;
		string prefix8 = "[EffectForSuitLost]";
		bool showBaseVal8 = flag;
		StandardEffectInfo baseVal8;
		if (flag)
		{
			baseVal8 = scampSuitToggle.m_effectForSuitLost;
		}
		else
		{
			baseVal8 = null;
		}
		return str8 + base.PropDesc(effectForSuitLostMod, prefix8, showBaseVal8, baseVal8);
	}
}
