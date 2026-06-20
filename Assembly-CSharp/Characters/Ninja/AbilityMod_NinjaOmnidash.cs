// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NinjaOmnidash : AbilityMod
{
	[Separator("Damage/Heal mod on Deathmark")]
	public AbilityModPropertyInt m_deathmarkDamageMod;
	public AbilityModPropertyInt m_deathmarkCasterHealMod;
	[Separator("On Hit Stuff")]
	public AbilityModPropertyInt m_baseDamageMod;
	public AbilityModPropertyInt m_damageChangePerEnemyAfterFirstMod;
	public AbilityModPropertyInt m_minDamageMod;
	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;
	[Header("-- Effect for single hit --")]
	public AbilityModPropertyEffectInfo m_singleHitEnemyEffectMod;
	public AbilityModPropertyEffectInfo m_extraSingleHitEnemyEffectMod;
	[Separator("Energy gain on Marked hit")]
	public AbilityModPropertyInt m_energyGainPerMarkedHitMod;
	[Separator("For Dash")]
	public AbilityModPropertyBool m_skipEvadeMod;
	[Space(10f)]
	public AbilityModPropertyBool m_isTeleportMod;
	public AbilityModPropertyFloat m_dashRadiusAtStartMod;
	public AbilityModPropertyFloat m_dashRadiusMiddleMod;
	public AbilityModPropertyFloat m_dashRadiusAtEndMod;
	public AbilityModPropertyBool m_dashPenetrateLineOfSightMod;
	[Header("-- Whether can queue movement evade")]
	public AbilityModPropertyBool m_canQueueMoveAfterEvadeMod;
	[Separator("[Deathmark] Effect", "magenta")]
	public AbilityModPropertyBool m_applyDeathmarkEffectMod;
	[Separator("Cooldown Reset on other ability")]
	public AbilityModPropertyInt m_cdrOnAbilityMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NinjaOmnidash);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NinjaOmnidash ninjaOmnidash = targetAbility as NinjaOmnidash;
		if (ninjaOmnidash != null)
		{
			Ninja_SyncComponent component = ninjaOmnidash.GetComponent<Ninja_SyncComponent>();
			if (component != null)
			{
				AddToken(tokens, m_deathmarkDamageMod, "Deathmark_TriggerDamage", string.Empty, component.m_deathmarkOnTriggerDamage);
				AddToken(tokens, m_deathmarkCasterHealMod, "Deathmark_CasterHeal", string.Empty, component.m_deathmarkOnTriggerCasterHeal);
			}
			AddToken(tokens, m_baseDamageMod, "BaseDamage", string.Empty, ninjaOmnidash.m_baseDamage);
			AddToken(tokens, m_damageChangePerEnemyAfterFirstMod, "DamageChangePerEnemyAfterFirst", string.Empty, ninjaOmnidash.m_damageChangePerEnemyAfterFirst);
			AddToken(tokens, m_minDamageMod, "MinDamage", string.Empty, ninjaOmnidash.m_minDamage);
			AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", ninjaOmnidash.m_enemyHitEffect);
			AddToken_EffectMod(tokens, m_singleHitEnemyEffectMod, "SingleHitEnemyEffect", ninjaOmnidash.m_singleHitEnemyEffect);
			AddToken_EffectMod(tokens, m_extraSingleHitEnemyEffectMod, "ExtraSingleHitEnemyEffect", ninjaOmnidash.m_extraSingleHitEnemyEffect);
			AddToken(tokens, m_energyGainPerMarkedHitMod, "EnergyGainPerMarkedHit", string.Empty, ninjaOmnidash.m_energyGainPerMarkedHit);
			AddToken(tokens, m_dashRadiusAtStartMod, "DashRadiusAtStart", string.Empty, ninjaOmnidash.m_dashRadiusAtStart);
			AddToken(tokens, m_dashRadiusMiddleMod, "DashRadiusMiddle", string.Empty, ninjaOmnidash.m_dashRadiusMiddle);
			AddToken(tokens, m_dashRadiusAtEndMod, "DashRadiusAtEnd", string.Empty, ninjaOmnidash.m_dashRadiusAtEnd);
			AddToken(tokens, m_cdrOnAbilityMod, "CdrOnAbility", string.Empty, ninjaOmnidash.m_cdrOnAbility);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)  // , Ability targetAbility in rogues
	{
		// reactor
		NinjaOmnidash ninjaOmnidash = GetTargetAbilityOnAbilityData(abilityData) as NinjaOmnidash;
		// rogues
		// NinjaOmnidash ninjaOmnidash = targetAbility as NinjaOmnidash;
		bool isValid = ninjaOmnidash != null;
		Ninja_SyncComponent ninja_SyncComponent = null;
		if (ninjaOmnidash != null)
		{
			ninja_SyncComponent = ninjaOmnidash.GetComponent<Ninja_SyncComponent>();
		}
		string desc = string.Empty;
		desc += PropDesc(m_deathmarkDamageMod, "[Deathmark_Damage]", ninja_SyncComponent != null, ninja_SyncComponent.m_deathmarkOnTriggerDamage);
		desc += PropDesc(m_deathmarkCasterHealMod, "[Deathmark_CasterHeal]", ninja_SyncComponent != null, ninja_SyncComponent.m_deathmarkOnTriggerCasterHeal);
		desc += PropDesc(m_baseDamageMod, "[BaseDamage]", isValid, isValid ? ninjaOmnidash.m_baseDamage : 0);
		desc += PropDesc(m_damageChangePerEnemyAfterFirstMod, "[DamageChangePerEnemyAfterFirst]", isValid, isValid ? ninjaOmnidash.m_damageChangePerEnemyAfterFirst : 0);
		desc += PropDesc(m_minDamageMod, "[MinDamage]", isValid, isValid ? ninjaOmnidash.m_minDamage : 0);
		desc += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", isValid, isValid ? ninjaOmnidash.m_enemyHitEffect : null);
		desc += PropDesc(m_singleHitEnemyEffectMod, "[SingleHitEnemyEffect]", isValid, isValid ? ninjaOmnidash.m_singleHitEnemyEffect : null);
		desc += PropDesc(m_extraSingleHitEnemyEffectMod, "[ExtraSingleHitEnemyEffect]", isValid, isValid ? ninjaOmnidash.m_extraSingleHitEnemyEffect : null);
		desc += PropDesc(m_energyGainPerMarkedHitMod, "[EnergyGainPerMarkedHit]", isValid, isValid ? ninjaOmnidash.m_energyGainPerMarkedHit : 0);
		desc += PropDesc(m_skipEvadeMod, "[SkipEvade]", isValid, isValid && ninjaOmnidash.m_skipEvade);
		desc += PropDesc(m_isTeleportMod, "[IsTeleport]", isValid, isValid && ninjaOmnidash.m_isTeleport);
		desc += PropDesc(m_dashRadiusAtStartMod, "[DashRadiusAtStart]", isValid, isValid ? ninjaOmnidash.m_dashRadiusAtStart : 0f);
		desc += PropDesc(m_dashRadiusMiddleMod, "[DashRadiusMiddle]", isValid, isValid ? ninjaOmnidash.m_dashRadiusMiddle : 0f);
		desc += PropDesc(m_dashRadiusAtEndMod, "[DashRadiusAtEnd]", isValid, isValid ? ninjaOmnidash.m_dashRadiusAtEnd : 0f);
		desc += PropDesc(m_dashPenetrateLineOfSightMod, "[DashPenetrateLineOfSight]", isValid, isValid && ninjaOmnidash.m_dashPenetrateLineOfSight);
		desc += PropDesc(m_canQueueMoveAfterEvadeMod, "[CanQueueMoveAfterEvade]", isValid, isValid && ninjaOmnidash.m_canQueueMoveAfterEvade);
		desc += PropDesc(m_applyDeathmarkEffectMod, "[ApplyDeathmarkEffect]", isValid, isValid && ninjaOmnidash.m_applyDeathmarkEffect);
		return desc + PropDesc(m_cdrOnAbilityMod, "[CdrOnAbility]", isValid, isValid ? ninjaOmnidash.m_cdrOnAbility : 0);
	}
}
