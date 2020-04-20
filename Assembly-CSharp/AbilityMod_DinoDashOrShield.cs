using System;
using System.Collections.Generic;
using AbilityContextNamespace;

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
		if (dinoDashOrShield != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			base.AddOnHitDataTokens(tokens, this.m_dashOnHitDataMod, dinoDashOrShield.m_dashOnHitData);
			AbilityMod.AddToken(tokens, this.m_shieldPerEnemyHitMod, "ShieldPerEnemyHit", string.Empty, dinoDashOrShield.m_shieldPerEnemyHit, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldDurationMod, "ShieldDuration", string.Empty, dinoDashOrShield.m_shieldDuration, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_shieldEffectMod, "ShieldEffect", dinoDashOrShield.m_shieldEffect, true);
			AbilityMod.AddToken(tokens, this.m_healIfNoDashMod, "HealIfNoDash", string.Empty, dinoDashOrShield.m_healIfNoDash, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrIfNoDashMod, "CdrIfNoDash", string.Empty, dinoDashOrShield.m_cdrIfNoDash, true, false);
			AbilityMod.AddToken(tokens, this.m_delayedCooldownMod, "DelayedCooldown", string.Empty, dinoDashOrShield.m_delayedCooldown, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		DinoDashOrShield dinoDashOrShield = base.GetTargetAbilityOnAbilityData(abilityData) as DinoDashOrShield;
		bool flag = dinoDashOrShield != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		text += base.GetTargetSelectModDesc(this.m_initialCastTargetSelectMod, dinoDashOrShield.m_targetSelectComp, "-- Initial cast Target Select --");
		text += base.GetTargetSelectModDesc(this.m_dashTargetSelectMod, dinoDashOrShield.m_targetSelectForDash, "-- Dash Target Select --");
		text += base.GetOnHitDataDesc(this.m_dashOnHitDataMod, dinoDashOrShield.m_dashOnHitData, "-- On Hit Data Mod --");
		string str = text;
		AbilityModPropertyInt shieldPerEnemyHitMod = this.m_shieldPerEnemyHitMod;
		string prefix = "[ShieldPerEnemyHit]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			baseVal = dinoDashOrShield.m_shieldPerEnemyHit;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(shieldPerEnemyHitMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt shieldDurationMod = this.m_shieldDurationMod;
		string prefix2 = "[ShieldDuration]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = dinoDashOrShield.m_shieldDuration;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(shieldDurationMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_shieldEffectMod, "[ShieldEffect]", flag, (!flag) ? null : dinoDashOrShield.m_shieldEffect);
		string str3 = text;
		AbilityModPropertyInt healIfNoDashMod = this.m_healIfNoDashMod;
		string prefix3 = "[HealIfNoDash]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = dinoDashOrShield.m_healIfNoDash;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(healIfNoDashMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt cdrIfNoDashMod = this.m_cdrIfNoDashMod;
		string prefix4 = "[CdrIfNoDash]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = dinoDashOrShield.m_cdrIfNoDash;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(cdrIfNoDashMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt delayedCooldownMod = this.m_delayedCooldownMod;
		string prefix5 = "[DelayedCooldown]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = dinoDashOrShield.m_delayedCooldown;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(delayedCooldownMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyBool fullyChargeUpLayerConeMod = this.m_fullyChargeUpLayerConeMod;
		string prefix6 = "[FullyChargeUpLayerCone]";
		bool showBaseVal6 = flag;
		bool baseVal6;
		if (flag)
		{
			baseVal6 = dinoDashOrShield.m_fullyChargeUpLayerCone;
		}
		else
		{
			baseVal6 = false;
		}
		return str6 + base.PropDesc(fullyChargeUpLayerConeMod, prefix6, showBaseVal6, baseVal6);
	}
}
