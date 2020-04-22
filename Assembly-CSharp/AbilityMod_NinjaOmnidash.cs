using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NinjaOmnidash : AbilityMod
{
	[Separator("Damage/Heal mod on Deathmark", true)]
	public AbilityModPropertyInt m_deathmarkDamageMod;

	public AbilityModPropertyInt m_deathmarkCasterHealMod;

	[Separator("On Hit Stuff", true)]
	public AbilityModPropertyInt m_baseDamageMod;

	public AbilityModPropertyInt m_damageChangePerEnemyAfterFirstMod;

	public AbilityModPropertyInt m_minDamageMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	[Header("-- Effect for single hit --")]
	public AbilityModPropertyEffectInfo m_singleHitEnemyEffectMod;

	public AbilityModPropertyEffectInfo m_extraSingleHitEnemyEffectMod;

	[Separator("Energy gain on Marked hit", true)]
	public AbilityModPropertyInt m_energyGainPerMarkedHitMod;

	[Separator("For Dash", true)]
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

	[Separator("Cooldown Reset on other ability", true)]
	public AbilityModPropertyInt m_cdrOnAbilityMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NinjaOmnidash);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NinjaOmnidash ninjaOmnidash = targetAbility as NinjaOmnidash;
		if (!(ninjaOmnidash != null))
		{
			return;
		}
		while (true)
		{
			Ninja_SyncComponent component = ninjaOmnidash.GetComponent<Ninja_SyncComponent>();
			if (component != null)
			{
				AbilityMod.AddToken(tokens, m_deathmarkDamageMod, "Deathmark_TriggerDamage", string.Empty, component.m_deathmarkOnTriggerDamage);
				AbilityMod.AddToken(tokens, m_deathmarkCasterHealMod, "Deathmark_CasterHeal", string.Empty, component.m_deathmarkOnTriggerCasterHeal);
			}
			AbilityMod.AddToken(tokens, m_baseDamageMod, "BaseDamage", string.Empty, ninjaOmnidash.m_baseDamage);
			AbilityMod.AddToken(tokens, m_damageChangePerEnemyAfterFirstMod, "DamageChangePerEnemyAfterFirst", string.Empty, ninjaOmnidash.m_damageChangePerEnemyAfterFirst);
			AbilityMod.AddToken(tokens, m_minDamageMod, "MinDamage", string.Empty, ninjaOmnidash.m_minDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", ninjaOmnidash.m_enemyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_singleHitEnemyEffectMod, "SingleHitEnemyEffect", ninjaOmnidash.m_singleHitEnemyEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_extraSingleHitEnemyEffectMod, "ExtraSingleHitEnemyEffect", ninjaOmnidash.m_extraSingleHitEnemyEffect);
			AbilityMod.AddToken(tokens, m_energyGainPerMarkedHitMod, "EnergyGainPerMarkedHit", string.Empty, ninjaOmnidash.m_energyGainPerMarkedHit);
			AbilityMod.AddToken(tokens, m_dashRadiusAtStartMod, "DashRadiusAtStart", string.Empty, ninjaOmnidash.m_dashRadiusAtStart);
			AbilityMod.AddToken(tokens, m_dashRadiusMiddleMod, "DashRadiusMiddle", string.Empty, ninjaOmnidash.m_dashRadiusMiddle);
			AbilityMod.AddToken(tokens, m_dashRadiusAtEndMod, "DashRadiusAtEnd", string.Empty, ninjaOmnidash.m_dashRadiusAtEnd);
			AbilityMod.AddToken(tokens, m_cdrOnAbilityMod, "CdrOnAbility", string.Empty, ninjaOmnidash.m_cdrOnAbility);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NinjaOmnidash ninjaOmnidash = GetTargetAbilityOnAbilityData(abilityData) as NinjaOmnidash;
		bool flag = ninjaOmnidash != null;
		Ninja_SyncComponent ninja_SyncComponent = null;
		if (ninjaOmnidash != null)
		{
			ninja_SyncComponent = ninjaOmnidash.GetComponent<Ninja_SyncComponent>();
		}
		string empty = string.Empty;
		empty += PropDesc(m_deathmarkDamageMod, "[Deathmark_Damage]", ninja_SyncComponent != null, ninja_SyncComponent.m_deathmarkOnTriggerDamage);
		empty += PropDesc(m_deathmarkCasterHealMod, "[Deathmark_CasterHeal]", ninja_SyncComponent != null, ninja_SyncComponent.m_deathmarkOnTriggerCasterHeal);
		string str = empty;
		AbilityModPropertyInt baseDamageMod = m_baseDamageMod;
		int baseVal;
		if (flag)
		{
			baseVal = ninjaOmnidash.m_baseDamage;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(baseDamageMod, "[BaseDamage]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt damageChangePerEnemyAfterFirstMod = m_damageChangePerEnemyAfterFirstMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = ninjaOmnidash.m_damageChangePerEnemyAfterFirst;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(damageChangePerEnemyAfterFirstMod, "[DamageChangePerEnemyAfterFirst]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt minDamageMod = m_minDamageMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = ninjaOmnidash.m_minDamage;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(minDamageMod, "[MinDamage]", flag, baseVal3);
		empty += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", flag, (!flag) ? null : ninjaOmnidash.m_enemyHitEffect);
		string str4 = empty;
		AbilityModPropertyEffectInfo singleHitEnemyEffectMod = m_singleHitEnemyEffectMod;
		object baseVal4;
		if (flag)
		{
			baseVal4 = ninjaOmnidash.m_singleHitEnemyEffect;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str4 + PropDesc(singleHitEnemyEffectMod, "[SingleHitEnemyEffect]", flag, (StandardEffectInfo)baseVal4);
		string str5 = empty;
		AbilityModPropertyEffectInfo extraSingleHitEnemyEffectMod = m_extraSingleHitEnemyEffectMod;
		object baseVal5;
		if (flag)
		{
			baseVal5 = ninjaOmnidash.m_extraSingleHitEnemyEffect;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + PropDesc(extraSingleHitEnemyEffectMod, "[ExtraSingleHitEnemyEffect]", flag, (StandardEffectInfo)baseVal5);
		string str6 = empty;
		AbilityModPropertyInt energyGainPerMarkedHitMod = m_energyGainPerMarkedHitMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = ninjaOmnidash.m_energyGainPerMarkedHit;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(energyGainPerMarkedHitMod, "[EnergyGainPerMarkedHit]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyBool skipEvadeMod = m_skipEvadeMod;
		int baseVal7;
		if (flag)
		{
			baseVal7 = (ninjaOmnidash.m_skipEvade ? 1 : 0);
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(skipEvadeMod, "[SkipEvade]", flag, (byte)baseVal7 != 0);
		empty += PropDesc(m_isTeleportMod, "[IsTeleport]", flag, flag && ninjaOmnidash.m_isTeleport);
		string str8 = empty;
		AbilityModPropertyFloat dashRadiusAtStartMod = m_dashRadiusAtStartMod;
		float baseVal8;
		if (flag)
		{
			baseVal8 = ninjaOmnidash.m_dashRadiusAtStart;
		}
		else
		{
			baseVal8 = 0f;
		}
		empty = str8 + PropDesc(dashRadiusAtStartMod, "[DashRadiusAtStart]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyFloat dashRadiusMiddleMod = m_dashRadiusMiddleMod;
		float baseVal9;
		if (flag)
		{
			baseVal9 = ninjaOmnidash.m_dashRadiusMiddle;
		}
		else
		{
			baseVal9 = 0f;
		}
		empty = str9 + PropDesc(dashRadiusMiddleMod, "[DashRadiusMiddle]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyFloat dashRadiusAtEndMod = m_dashRadiusAtEndMod;
		float baseVal10;
		if (flag)
		{
			baseVal10 = ninjaOmnidash.m_dashRadiusAtEnd;
		}
		else
		{
			baseVal10 = 0f;
		}
		empty = str10 + PropDesc(dashRadiusAtEndMod, "[DashRadiusAtEnd]", flag, baseVal10);
		string str11 = empty;
		AbilityModPropertyBool dashPenetrateLineOfSightMod = m_dashPenetrateLineOfSightMod;
		int baseVal11;
		if (flag)
		{
			baseVal11 = (ninjaOmnidash.m_dashPenetrateLineOfSight ? 1 : 0);
		}
		else
		{
			baseVal11 = 0;
		}
		empty = str11 + PropDesc(dashPenetrateLineOfSightMod, "[DashPenetrateLineOfSight]", flag, (byte)baseVal11 != 0);
		string str12 = empty;
		AbilityModPropertyBool canQueueMoveAfterEvadeMod = m_canQueueMoveAfterEvadeMod;
		int baseVal12;
		if (flag)
		{
			baseVal12 = (ninjaOmnidash.m_canQueueMoveAfterEvade ? 1 : 0);
		}
		else
		{
			baseVal12 = 0;
		}
		empty = str12 + PropDesc(canQueueMoveAfterEvadeMod, "[CanQueueMoveAfterEvade]", flag, (byte)baseVal12 != 0);
		string str13 = empty;
		AbilityModPropertyBool applyDeathmarkEffectMod = m_applyDeathmarkEffectMod;
		int baseVal13;
		if (flag)
		{
			baseVal13 = (ninjaOmnidash.m_applyDeathmarkEffect ? 1 : 0);
		}
		else
		{
			baseVal13 = 0;
		}
		empty = str13 + PropDesc(applyDeathmarkEffectMod, "[ApplyDeathmarkEffect]", flag, (byte)baseVal13 != 0);
		string str14 = empty;
		AbilityModPropertyInt cdrOnAbilityMod = m_cdrOnAbilityMod;
		int baseVal14;
		if (flag)
		{
			baseVal14 = ninjaOmnidash.m_cdrOnAbility;
		}
		else
		{
			baseVal14 = 0;
		}
		return str14 + PropDesc(cdrOnAbilityMod, "[CdrOnAbility]", flag, baseVal14);
	}
}
