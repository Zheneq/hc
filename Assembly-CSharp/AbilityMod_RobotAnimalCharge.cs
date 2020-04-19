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
			AbilityMod.AddToken(tokens, this.m_damageMod, "DamageAmount", string.Empty, robotAnimalCharge.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_lifeOnFirstHitMod, "LifeOnFirstHit", string.Empty, robotAnimalCharge.m_lifeOnFirstHit, true, false, false);
			AbilityMod.AddToken(tokens, this.m_lifePerHitMod, "LifePerHit", string.Empty, robotAnimalCharge.m_lifePerHit, true, false, false);
			AbilityMod.AddToken_IntDiff(tokens, "HealOnNextTurnIfKilledTarget", string.Empty, this.m_healOnNextTurnStartIfKilledTarget, false, 0);
			AbilityMod.AddToken(tokens, this.m_cdrOnHittingAllyMod, "CdrOnHittingAlly", string.Empty, robotAnimalCharge.m_cdrOnHittingAlly, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrOnHittingEnemyMod, "CdrOnHittingEnemy", string.Empty, robotAnimalCharge.m_cdrOnHittingEnemy, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RobotAnimalCharge robotAnimalCharge = base.GetTargetAbilityOnAbilityData(abilityData) as RobotAnimalCharge;
		bool flag = robotAnimalCharge != null;
		string text = string.Empty;
		text += AbilityModHelper.GetModPropertyDesc(this.m_damageMod, "[Damage]", flag, (!flag) ? 0 : robotAnimalCharge.m_damageAmount);
		string str = text;
		AbilityModPropertyFloat lifeOnFirstHitMod = this.m_lifeOnFirstHitMod;
		string prefix = "[Life On First Hit]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			for (;;)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_RobotAnimalCharge.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = robotAnimalCharge.m_lifeOnFirstHit;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(lifeOnFirstHitMod, prefix, showBaseVal, baseVal);
		text += AbilityModHelper.GetModPropertyDesc(this.m_lifePerHitMod, "[Life Per Hit Mod]", flag, (!flag) ? 0f : robotAnimalCharge.m_lifePerHit);
		if (this.m_healOnNextTurnStartIfKilledTarget > 0)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Heal on Next Turn Start If Killed Target] = ",
				this.m_healOnNextTurnStartIfKilledTarget,
				"\n"
			});
		}
		text += AbilityModHelper.GetModEffectInfoDesc(this.m_effectOnSelf, "{ Effect on Self }", string.Empty, flag, null);
		text += AbilityModHelper.GetModEffectInfoDesc(this.m_effectToSelfPerAdjacentAlly, "{ Effect on Self Per Adjacent Ally }", string.Empty, flag, null);
		if (this.m_techPointsPerAdjacentAlly > 0)
		{
			for (;;)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Tech Points Per Adjacent Ally] = ",
				this.m_techPointsPerAdjacentAlly,
				"\n"
			});
		}
		string str2 = text;
		AbilityModPropertyBool requireTargetActorMod = this.m_requireTargetActorMod;
		string prefix2 = "[RequireTargetActor]";
		bool showBaseVal2 = flag;
		bool baseVal2;
		if (flag)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = robotAnimalCharge.m_requireTargetActor;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + base.PropDesc(requireTargetActorMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyBool canIncludeEnemyMod = this.m_canIncludeEnemyMod;
		string prefix3 = "[CanIncludeEnemy]";
		bool showBaseVal3 = flag;
		bool baseVal3;
		if (flag)
		{
			for (;;)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = robotAnimalCharge.m_canIncludeEnemy;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + base.PropDesc(canIncludeEnemyMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_canIncludeAllyMod, "[CanIncludeAlly]", flag, flag && robotAnimalCharge.m_canIncludeAlly);
		string str4 = text;
		AbilityModPropertyInt cdrOnHittingAllyMod = this.m_cdrOnHittingAllyMod;
		string prefix4 = "[CdrOnHittingAlly]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			for (;;)
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
		text = str4 + base.PropDesc(cdrOnHittingAllyMod, prefix4, showBaseVal4, baseVal4);
		return text + base.PropDesc(this.m_cdrOnHittingEnemyMod, "[CdrOnHittingEnemy]", flag, (!flag) ? 0 : robotAnimalCharge.m_cdrOnHittingEnemy);
	}
}
