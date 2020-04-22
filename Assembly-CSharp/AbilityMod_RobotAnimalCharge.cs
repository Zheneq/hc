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
			AbilityMod.AddToken(tokens, m_damageMod, "DamageAmount", string.Empty, robotAnimalCharge.m_damageAmount);
			AbilityMod.AddToken(tokens, m_lifeOnFirstHitMod, "LifeOnFirstHit", string.Empty, robotAnimalCharge.m_lifeOnFirstHit);
			AbilityMod.AddToken(tokens, m_lifePerHitMod, "LifePerHit", string.Empty, robotAnimalCharge.m_lifePerHit);
			AbilityMod.AddToken_IntDiff(tokens, "HealOnNextTurnIfKilledTarget", string.Empty, m_healOnNextTurnStartIfKilledTarget, false, 0);
			AbilityMod.AddToken(tokens, m_cdrOnHittingAllyMod, "CdrOnHittingAlly", string.Empty, robotAnimalCharge.m_cdrOnHittingAlly);
			AbilityMod.AddToken(tokens, m_cdrOnHittingEnemyMod, "CdrOnHittingEnemy", string.Empty, robotAnimalCharge.m_cdrOnHittingEnemy);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RobotAnimalCharge robotAnimalCharge = GetTargetAbilityOnAbilityData(abilityData) as RobotAnimalCharge;
		bool flag = robotAnimalCharge != null;
		string empty = string.Empty;
		empty += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", flag, flag ? robotAnimalCharge.m_damageAmount : 0);
		string str = empty;
		AbilityModPropertyFloat lifeOnFirstHitMod = m_lifeOnFirstHitMod;
		float baseVal;
		if (flag)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = robotAnimalCharge.m_lifeOnFirstHit;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(lifeOnFirstHitMod, "[Life On First Hit]", flag, baseVal);
		empty += AbilityModHelper.GetModPropertyDesc(m_lifePerHitMod, "[Life Per Hit Mod]", flag, (!flag) ? 0f : robotAnimalCharge.m_lifePerHit);
		if (m_healOnNextTurnStartIfKilledTarget > 0)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			string text = empty;
			empty = text + "[Heal on Next Turn Start If Killed Target] = " + m_healOnNextTurnStartIfKilledTarget + "\n";
		}
		empty += AbilityModHelper.GetModEffectInfoDesc(m_effectOnSelf, "{ Effect on Self }", string.Empty, flag);
		empty += AbilityModHelper.GetModEffectInfoDesc(m_effectToSelfPerAdjacentAlly, "{ Effect on Self Per Adjacent Ally }", string.Empty, flag);
		if (m_techPointsPerAdjacentAlly > 0)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			string text = empty;
			empty = text + "[Tech Points Per Adjacent Ally] = " + m_techPointsPerAdjacentAlly + "\n";
		}
		string str2 = empty;
		AbilityModPropertyBool requireTargetActorMod = m_requireTargetActorMod;
		int baseVal2;
		if (flag)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = (robotAnimalCharge.m_requireTargetActor ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(requireTargetActorMod, "[RequireTargetActor]", flag, (byte)baseVal2 != 0);
		string str3 = empty;
		AbilityModPropertyBool canIncludeEnemyMod = m_canIncludeEnemyMod;
		int baseVal3;
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = (robotAnimalCharge.m_canIncludeEnemy ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(canIncludeEnemyMod, "[CanIncludeEnemy]", flag, (byte)baseVal3 != 0);
		empty += PropDesc(m_canIncludeAllyMod, "[CanIncludeAlly]", flag, flag && robotAnimalCharge.m_canIncludeAlly);
		string str4 = empty;
		AbilityModPropertyInt cdrOnHittingAllyMod = m_cdrOnHittingAllyMod;
		int baseVal4;
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal4 = robotAnimalCharge.m_cdrOnHittingAlly;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(cdrOnHittingAllyMod, "[CdrOnHittingAlly]", flag, baseVal4);
		return empty + PropDesc(m_cdrOnHittingEnemyMod, "[CdrOnHittingEnemy]", flag, flag ? robotAnimalCharge.m_cdrOnHittingEnemy : 0);
	}
}
