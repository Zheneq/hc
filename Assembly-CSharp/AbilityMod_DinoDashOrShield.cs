using AbilityContextNamespace;
using System;
using System.Collections.Generic;

public class AbilityMod_DinoDashOrShield : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod for initial cast", true)]
	public TargetSelectMod_Shape m_initialCastTargetSelectMod;

	[Separator("Target Select Mod for Dash", true)]
	public TargetSelectMod_LaserChargeWithReverseCones m_dashTargetSelectMod;

	[Separator("[Dash]: On Hit Mod", "yellow")]
	public OnHitDataMod m_dashOnHitDataMod;

	[Separator("[Dash]: Shielding per enemy hit", true)]
	public AbilityModPropertyInt m_shieldPerEnemyHitMod;

	public AbilityModPropertyInt m_shieldDurationMod;

	[Separator("For No Dash, applied on end of prep phase", true)]
	public AbilityModPropertyEffectInfo m_shieldEffectMod;

	public AbilityModPropertyInt m_healIfNoDashMod;

	public AbilityModPropertyInt m_cdrIfNoDashMod;

	[Separator("Cooldown, set on turn after initial cast", true)]
	public AbilityModPropertyInt m_delayedCooldownMod;

	[Separator("Powering up primary", true)]
	public AbilityModPropertyBool m_fullyChargeUpLayerConeMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(DinoDashOrShield);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		DinoDashOrShield dinoDashOrShield = targetAbility as DinoDashOrShield;
		if (!(dinoDashOrShield != null))
		{
			return;
		}
		while (true)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AddOnHitDataTokens(tokens, m_dashOnHitDataMod, dinoDashOrShield.m_dashOnHitData);
			AbilityMod.AddToken(tokens, m_shieldPerEnemyHitMod, "ShieldPerEnemyHit", string.Empty, dinoDashOrShield.m_shieldPerEnemyHit);
			AbilityMod.AddToken(tokens, m_shieldDurationMod, "ShieldDuration", string.Empty, dinoDashOrShield.m_shieldDuration);
			AbilityMod.AddToken_EffectMod(tokens, m_shieldEffectMod, "ShieldEffect", dinoDashOrShield.m_shieldEffect);
			AbilityMod.AddToken(tokens, m_healIfNoDashMod, "HealIfNoDash", string.Empty, dinoDashOrShield.m_healIfNoDash);
			AbilityMod.AddToken(tokens, m_cdrIfNoDashMod, "CdrIfNoDash", string.Empty, dinoDashOrShield.m_cdrIfNoDash);
			AbilityMod.AddToken(tokens, m_delayedCooldownMod, "DelayedCooldown", string.Empty, dinoDashOrShield.m_delayedCooldown);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		DinoDashOrShield dinoDashOrShield = GetTargetAbilityOnAbilityData(abilityData) as DinoDashOrShield;
		bool flag = dinoDashOrShield != null;
		string str = base.ModSpecificAutogenDesc(abilityData);
		str += GetTargetSelectModDesc(m_initialCastTargetSelectMod, dinoDashOrShield.m_targetSelectComp, "-- Initial cast Target Select --");
		str += GetTargetSelectModDesc(m_dashTargetSelectMod, dinoDashOrShield.m_targetSelectForDash, "-- Dash Target Select --");
		str += GetOnHitDataDesc(m_dashOnHitDataMod, dinoDashOrShield.m_dashOnHitData);
		string str2 = str;
		AbilityModPropertyInt shieldPerEnemyHitMod = m_shieldPerEnemyHitMod;
		int baseVal;
		if (flag)
		{
			baseVal = dinoDashOrShield.m_shieldPerEnemyHit;
		}
		else
		{
			baseVal = 0;
		}
		str = str2 + PropDesc(shieldPerEnemyHitMod, "[ShieldPerEnemyHit]", flag, baseVal);
		string str3 = str;
		AbilityModPropertyInt shieldDurationMod = m_shieldDurationMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = dinoDashOrShield.m_shieldDuration;
		}
		else
		{
			baseVal2 = 0;
		}
		str = str3 + PropDesc(shieldDurationMod, "[ShieldDuration]", flag, baseVal2);
		str += PropDesc(m_shieldEffectMod, "[ShieldEffect]", flag, (!flag) ? null : dinoDashOrShield.m_shieldEffect);
		string str4 = str;
		AbilityModPropertyInt healIfNoDashMod = m_healIfNoDashMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = dinoDashOrShield.m_healIfNoDash;
		}
		else
		{
			baseVal3 = 0;
		}
		str = str4 + PropDesc(healIfNoDashMod, "[HealIfNoDash]", flag, baseVal3);
		string str5 = str;
		AbilityModPropertyInt cdrIfNoDashMod = m_cdrIfNoDashMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = dinoDashOrShield.m_cdrIfNoDash;
		}
		else
		{
			baseVal4 = 0;
		}
		str = str5 + PropDesc(cdrIfNoDashMod, "[CdrIfNoDash]", flag, baseVal4);
		string str6 = str;
		AbilityModPropertyInt delayedCooldownMod = m_delayedCooldownMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = dinoDashOrShield.m_delayedCooldown;
		}
		else
		{
			baseVal5 = 0;
		}
		str = str6 + PropDesc(delayedCooldownMod, "[DelayedCooldown]", flag, baseVal5);
		string str7 = str;
		AbilityModPropertyBool fullyChargeUpLayerConeMod = m_fullyChargeUpLayerConeMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = (dinoDashOrShield.m_fullyChargeUpLayerCone ? 1 : 0);
		}
		else
		{
			baseVal6 = 0;
		}
		return str7 + PropDesc(fullyChargeUpLayerConeMod, "[FullyChargeUpLayerCone]", flag, (byte)baseVal6 != 0);
	}
}
