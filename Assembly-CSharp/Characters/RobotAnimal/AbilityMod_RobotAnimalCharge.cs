// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_RobotAnimalCharge : AbilityMod
{
	[Header("-- Heal On Next Turn Start If Killed Target")]
	public int m_healOnNextTurnStartIfKilledTarget;
	[Header("-- Damage and Life Gain Mod")]
	public AbilityModPropertyInt m_damageMod;
	public AbilityModPropertyFloat m_lifeOnFirstHitMod;
	public AbilityModPropertyFloat m_lifePerHitMod;
	[Header("-- Effect on Self")]
	public StandardEffectInfo m_effectOnSelf;
	[Header("-- Effect on Self Per Adjacent Ally At Destination")]
	public StandardEffectInfo m_effectToSelfPerAdjacentAlly;
	[Header("-- Tech Points for Caster Per Adjacent Ally At Destination")]
	public int m_techPointsPerAdjacentAlly;
	[Header("-- Targeting")]
	public AbilityModPropertyBool m_requireTargetActorMod;
	public AbilityModPropertyBool m_canIncludeEnemyMod;
	public AbilityModPropertyBool m_canIncludeAllyMod;
	[Header("-- Cooldown reduction on hitting target")]
	public AbilityModPropertyInt m_cdrOnHittingAllyMod;
	public AbilityModPropertyInt m_cdrOnHittingEnemyMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(RobotAnimalCharge);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RobotAnimalCharge robotAnimalCharge = targetAbility as RobotAnimalCharge;
		if (robotAnimalCharge != null)
		{
			AddToken(tokens, m_damageMod, "DamageAmount", string.Empty, robotAnimalCharge.m_damageAmount);
			AddToken(tokens, m_lifeOnFirstHitMod, "LifeOnFirstHit", string.Empty, robotAnimalCharge.m_lifeOnFirstHit);
			AddToken(tokens, m_lifePerHitMod, "LifePerHit", string.Empty, robotAnimalCharge.m_lifePerHit);
			AddToken_IntDiff(tokens, "HealOnNextTurnIfKilledTarget", string.Empty, m_healOnNextTurnStartIfKilledTarget, false, 0);
			AddToken(tokens, m_cdrOnHittingAllyMod, "CdrOnHittingAlly", string.Empty, robotAnimalCharge.m_cdrOnHittingAlly);
			AddToken(tokens, m_cdrOnHittingEnemyMod, "CdrOnHittingEnemy", string.Empty, robotAnimalCharge.m_cdrOnHittingEnemy);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)  // , Ability targetAbility in rogues
	{
		// reactor
		RobotAnimalCharge robotAnimalCharge = GetTargetAbilityOnAbilityData(abilityData) as RobotAnimalCharge;
		// rogues
		//RobotAnimalCharge robotAnimalCharge = targetAbility as RobotAnimalCharge;
		bool isAbilityPresent = robotAnimalCharge != null;
		string desc = string.Empty;
		desc += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", isAbilityPresent, isAbilityPresent ? robotAnimalCharge.m_damageAmount : 0);
		desc += AbilityModHelper.GetModPropertyDesc(m_lifeOnFirstHitMod, "[Life On First Hit]", isAbilityPresent, isAbilityPresent ? robotAnimalCharge.m_lifeOnFirstHit : 0f);
		desc += AbilityModHelper.GetModPropertyDesc(m_lifePerHitMod, "[Life Per Hit Mod]", isAbilityPresent, isAbilityPresent ? robotAnimalCharge.m_lifePerHit : 0f);
		if (m_healOnNextTurnStartIfKilledTarget > 0)
		{
			desc += "[Heal on Next Turn Start If Killed Target] = " + m_healOnNextTurnStartIfKilledTarget + "\n";
		}
		desc += AbilityModHelper.GetModEffectInfoDesc(m_effectOnSelf, "{ Effect on Self }", string.Empty, isAbilityPresent);
		desc += AbilityModHelper.GetModEffectInfoDesc(m_effectToSelfPerAdjacentAlly, "{ Effect on Self Per Adjacent Ally }", string.Empty, isAbilityPresent);
		if (m_techPointsPerAdjacentAlly > 0)
		{
			desc += "[Tech Points Per Adjacent Ally] = " + m_techPointsPerAdjacentAlly + "\n";
		}
		desc += PropDesc(m_requireTargetActorMod, "[RequireTargetActor]", isAbilityPresent, isAbilityPresent && robotAnimalCharge.m_requireTargetActor);
		desc += PropDesc(m_canIncludeEnemyMod, "[CanIncludeEnemy]", isAbilityPresent, isAbilityPresent && robotAnimalCharge.m_canIncludeEnemy);
		desc += PropDesc(m_canIncludeAllyMod, "[CanIncludeAlly]", isAbilityPresent, isAbilityPresent && robotAnimalCharge.m_canIncludeAlly);
		desc += PropDesc(m_cdrOnHittingAllyMod, "[CdrOnHittingAlly]", isAbilityPresent, isAbilityPresent ? robotAnimalCharge.m_cdrOnHittingAlly : 0);
		return desc + PropDesc(m_cdrOnHittingEnemyMod, "[CdrOnHittingEnemy]", isAbilityPresent, isAbilityPresent ? robotAnimalCharge.m_cdrOnHittingEnemy : 0);
	}
}
