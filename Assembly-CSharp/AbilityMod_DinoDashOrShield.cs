using System;
using System.Collections.Generic;
using AbilityContextNamespace;

public class AbilityMod_DinoDashOrShield : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod for initial cast")]
	public TargetSelectMod_Shape m_initialCastTargetSelectMod;
	[Separator("Target Select Mod for Dash")]
	public TargetSelectMod_LaserChargeWithReverseCones m_dashTargetSelectMod;
	[Separator("[Dash]: On Hit Mod", "yellow")]
	public OnHitDataMod m_dashOnHitDataMod;
	[Separator("[Dash]: Shielding per enemy hit")]
	public AbilityModPropertyInt m_shieldPerEnemyHitMod;
	public AbilityModPropertyInt m_shieldDurationMod;
	[Separator("For No Dash, applied on end of prep phase")]
	public AbilityModPropertyEffectInfo m_shieldEffectMod;
	public AbilityModPropertyInt m_healIfNoDashMod;
	public AbilityModPropertyInt m_cdrIfNoDashMod;
	[Separator("Cooldown, set on turn after initial cast")]
	public AbilityModPropertyInt m_delayedCooldownMod;
	[Separator("Powering up primary")]
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
			AddOnHitDataTokens(tokens, m_dashOnHitDataMod, dinoDashOrShield.m_dashOnHitData);
			AddToken(tokens, m_shieldPerEnemyHitMod, "ShieldPerEnemyHit", string.Empty, dinoDashOrShield.m_shieldPerEnemyHit);
			AddToken(tokens, m_shieldDurationMod, "ShieldDuration", string.Empty, dinoDashOrShield.m_shieldDuration);
			AddToken_EffectMod(tokens, m_shieldEffectMod, "ShieldEffect", dinoDashOrShield.m_shieldEffect);
			AddToken(tokens, m_healIfNoDashMod, "HealIfNoDash", string.Empty, dinoDashOrShield.m_healIfNoDash);
			AddToken(tokens, m_cdrIfNoDashMod, "CdrIfNoDash", string.Empty, dinoDashOrShield.m_cdrIfNoDash);
			AddToken(tokens, m_delayedCooldownMod, "DelayedCooldown", string.Empty, dinoDashOrShield.m_delayedCooldown);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		DinoDashOrShield dinoDashOrShield = GetTargetAbilityOnAbilityData(abilityData) as DinoDashOrShield;
		bool isValid = dinoDashOrShield != null;
		string desc = base.ModSpecificAutogenDesc(abilityData);
		desc += GetTargetSelectModDesc(m_initialCastTargetSelectMod, dinoDashOrShield.m_targetSelectComp, "-- Initial cast Target Select --");
		desc += GetTargetSelectModDesc(m_dashTargetSelectMod, dinoDashOrShield.m_targetSelectForDash, "-- Dash Target Select --");
		desc += GetOnHitDataDesc(m_dashOnHitDataMod, dinoDashOrShield.m_dashOnHitData);
		desc += PropDesc(m_shieldPerEnemyHitMod, "[ShieldPerEnemyHit]", isValid, isValid ? dinoDashOrShield.m_shieldPerEnemyHit : 0);
		desc += PropDesc(m_shieldDurationMod, "[ShieldDuration]", isValid, isValid ? dinoDashOrShield.m_shieldDuration : 0);
		desc += PropDesc(m_shieldEffectMod, "[ShieldEffect]", isValid, isValid ? dinoDashOrShield.m_shieldEffect : null);
		desc += PropDesc(m_healIfNoDashMod, "[HealIfNoDash]", isValid, isValid ? dinoDashOrShield.m_healIfNoDash : 0);
		desc += PropDesc(m_cdrIfNoDashMod, "[CdrIfNoDash]", isValid, isValid ? dinoDashOrShield.m_cdrIfNoDash : 0);
		desc += PropDesc(m_delayedCooldownMod, "[DelayedCooldown]", isValid, isValid ? dinoDashOrShield.m_delayedCooldown : 0);
		return desc + PropDesc(m_fullyChargeUpLayerConeMod, "[FullyChargeUpLayerCone]", isValid, isValid && dinoDashOrShield.m_fullyChargeUpLayerCone);
	}
}
