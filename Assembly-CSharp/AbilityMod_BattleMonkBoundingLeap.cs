using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BattleMonkBoundingLeap : AbilityMod
{
	[Header("-- Bounce Mod")]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyInt m_damageAfterFirstHitMod;

	[Header("-- Bounce")]
	public AbilityModPropertyInt m_maxHitTargetsMod;

	public AbilityModPropertyBool m_bounceOffEnemyActorMod;

	public AbilityModPropertyFloat m_maxDistancePerBounceMod;

	public AbilityModPropertyFloat m_maxTotalDistanceMod;

	public AbilityModPropertyInt m_maxBouncesMod;

	[Header("-- Whether to include allies in between")]
	public AbilityModPropertyBool m_hitAlliesInBetween;

	public StandardEffectInfo m_allyHitEffect;

	[Header("-- Heal Amount If Not Damaged This Turn")]
	public AbilityModPropertyInt m_healAmountIfNotDamagedThisTurn;

	public override Type GetTargetAbilityType()
	{
		return typeof(BattleMonkBoundingLeap);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BattleMonkBoundingLeap battleMonkBoundingLeap = targetAbility as BattleMonkBoundingLeap;
		if (!(battleMonkBoundingLeap != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_maxDistancePerBounceMod, "MaxDistancePerBounce", string.Empty, battleMonkBoundingLeap.m_maxDistancePerBounce);
			AbilityMod.AddToken(tokens, m_maxTotalDistanceMod, "MaxTotalDistance", string.Empty, battleMonkBoundingLeap.m_maxTotalDistance);
			AbilityMod.AddToken(tokens, m_maxBouncesMod, "MaxBounces", string.Empty, battleMonkBoundingLeap.m_maxBounces);
			AbilityMod.AddToken(tokens, m_damageMod, "DamageAmount", string.Empty, battleMonkBoundingLeap.m_damageAmount);
			AbilityMod.AddToken(tokens, m_damageAfterFirstHitMod, "DamageAfterFirstHit", string.Empty, battleMonkBoundingLeap.m_damageAfterFirstHit);
			AbilityMod.AddToken(tokens, m_maxBouncesMod, "MaxBounces", string.Empty, battleMonkBoundingLeap.m_maxBounces);
			AbilityMod.AddToken(tokens, m_maxHitTargetsMod, "MaxTargetsHit", string.Empty, battleMonkBoundingLeap.m_maxTargetsHit);
			AbilityMod.AddToken_EffectInfo(tokens, m_allyHitEffect, "AllyHitEffectMod", null, false);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BattleMonkBoundingLeap battleMonkBoundingLeap = GetTargetAbilityOnAbilityData(abilityData) as BattleMonkBoundingLeap;
		bool flag = battleMonkBoundingLeap != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal;
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
			baseVal = battleMonkBoundingLeap.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(damageMod, "[Damage]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt damageAfterFirstHitMod = m_damageAfterFirstHitMod;
		int baseVal2;
		if (flag)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = battleMonkBoundingLeap.m_damageAfterFirstHit;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(damageAfterFirstHitMod, "[DamageAfterFirstHit]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt maxHitTargetsMod = m_maxHitTargetsMod;
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
			baseVal3 = battleMonkBoundingLeap.m_maxTargetsHit;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(maxHitTargetsMod, "[Max Hits]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyBool bounceOffEnemyActorMod = m_bounceOffEnemyActorMod;
		int baseVal4;
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
			baseVal4 = (battleMonkBoundingLeap.m_bounceOffEnemyActor ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(bounceOffEnemyActorMod, "[Bounce Off Enemies?]", flag, (byte)baseVal4 != 0);
		string str5 = empty;
		AbilityModPropertyFloat maxDistancePerBounceMod = m_maxDistancePerBounceMod;
		float baseVal5;
		if (flag)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal5 = battleMonkBoundingLeap.m_maxDistancePerBounce;
		}
		else
		{
			baseVal5 = 0f;
		}
		empty = str5 + PropDesc(maxDistancePerBounceMod, "[MaxDistancePerBounce]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyFloat maxTotalDistanceMod = m_maxTotalDistanceMod;
		float baseVal6;
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
			baseVal6 = battleMonkBoundingLeap.m_maxTotalDistance;
		}
		else
		{
			baseVal6 = 0f;
		}
		empty = str6 + PropDesc(maxTotalDistanceMod, "[MaxTotalDistance]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyInt maxBouncesMod = m_maxBouncesMod;
		int baseVal7;
		if (flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal7 = battleMonkBoundingLeap.m_maxBounces;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(maxBouncesMod, "[MaxBounces]", flag, baseVal7);
		empty += AbilityModHelper.GetModPropertyDesc(m_hitAlliesInBetween, "[Hit Allies In-Between]", flag);
		empty += AbilityModHelper.GetModEffectInfoDesc(m_allyHitEffect, "{ Ally Hit Effect }", string.Empty, flag);
		return empty + PropDesc(m_healAmountIfNotDamagedThisTurn, "{ Heal Amount If Not Damaged This Turn }", flag);
	}
}
