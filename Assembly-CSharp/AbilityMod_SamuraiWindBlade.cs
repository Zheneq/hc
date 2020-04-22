using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SamuraiWindBlade : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_minRangeBeforeBendMod;

	public AbilityModPropertyFloat m_maxRangeBeforeBendMod;

	public AbilityModPropertyFloat m_maxTotalRangeMod;

	public AbilityModPropertyFloat m_maxBendAngleMod;

	public AbilityModPropertyBool m_penetrateLoSMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	[Header("-- Damage")]
	public AbilityModPropertyInt m_laserDamageAmountMod;

	public AbilityModPropertyInt m_damageChangePerTargetMod;

	public AbilityModPropertyEffectInfo m_laserHitEffectMod;

	[Header("-- Shielding per enemy hit on start of Next Turn")]
	public AbilityModPropertyInt m_shieldingPerEnemyHitNextTurnMod;

	public AbilityModPropertyInt m_shieldingDurationMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SamuraiWindBlade);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SamuraiWindBlade samuraiWindBlade = targetAbility as SamuraiWindBlade;
		if (!(samuraiWindBlade != null))
		{
			return;
		}
		while (true)
		{
			switch (2)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, samuraiWindBlade.m_laserWidth);
			AbilityMod.AddToken(tokens, m_minRangeBeforeBendMod, "MinRangeBeforeBend", string.Empty, samuraiWindBlade.m_minRangeBeforeBend);
			AbilityMod.AddToken(tokens, m_maxRangeBeforeBendMod, "MaxRangeBeforeBend", string.Empty, samuraiWindBlade.m_maxRangeBeforeBend);
			AbilityMod.AddToken(tokens, m_maxTotalRangeMod, "MaxTotalRange", string.Empty, samuraiWindBlade.m_maxTotalRange);
			AbilityMod.AddToken(tokens, m_maxBendAngleMod, "MaxBendAngle", string.Empty, samuraiWindBlade.m_maxBendAngle);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, samuraiWindBlade.m_maxTargets);
			AbilityMod.AddToken(tokens, m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, samuraiWindBlade.m_laserDamageAmount);
			AbilityMod.AddToken(tokens, m_damageChangePerTargetMod, "DamageChangePerTarget", string.Empty, samuraiWindBlade.m_damageChangePerTarget);
			AbilityMod.AddToken_EffectMod(tokens, m_laserHitEffectMod, "LaserHitEffect", samuraiWindBlade.m_laserHitEffect);
			AbilityMod.AddToken(tokens, m_shieldingPerEnemyHitNextTurnMod, "ShieldingPerEnemyHitNextTurn", string.Empty, samuraiWindBlade.m_shieldingPerEnemyHitNextTurn);
			AbilityMod.AddToken(tokens, m_shieldingDurationMod, "ShieldingDuration", string.Empty, samuraiWindBlade.m_shieldingDuration);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SamuraiWindBlade samuraiWindBlade = GetTargetAbilityOnAbilityData(abilityData) as SamuraiWindBlade;
		bool flag = samuraiWindBlade != null;
		string empty = string.Empty;
		empty += PropDesc(m_laserWidthMod, "[LaserWidth]", flag, (!flag) ? 0f : samuraiWindBlade.m_laserWidth);
		empty += PropDesc(m_minRangeBeforeBendMod, "[MinRangeBeforeBend]", flag, (!flag) ? 0f : samuraiWindBlade.m_minRangeBeforeBend);
		string str = empty;
		AbilityModPropertyFloat maxRangeBeforeBendMod = m_maxRangeBeforeBendMod;
		float baseVal;
		if (flag)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = samuraiWindBlade.m_maxRangeBeforeBend;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(maxRangeBeforeBendMod, "[MaxRangeBeforeBend]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat maxTotalRangeMod = m_maxTotalRangeMod;
		float baseVal2;
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
			baseVal2 = samuraiWindBlade.m_maxTotalRange;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(maxTotalRangeMod, "[MaxTotalRange]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat maxBendAngleMod = m_maxBendAngleMod;
		float baseVal3;
		if (flag)
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
			baseVal3 = samuraiWindBlade.m_maxBendAngle;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(maxBendAngleMod, "[MaxBendAngle]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyBool penetrateLoSMod = m_penetrateLoSMod;
		int baseVal4;
		if (flag)
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
			baseVal4 = (samuraiWindBlade.m_penetrateLoS ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(penetrateLoSMod, "[PenetrateLoS]", flag, (byte)baseVal4 != 0);
		empty += PropDesc(m_maxTargetsMod, "[MaxTargets]", flag, flag ? samuraiWindBlade.m_maxTargets : 0);
		empty += PropDesc(m_laserDamageAmountMod, "[LaserDamageAmount]", flag, flag ? samuraiWindBlade.m_laserDamageAmount : 0);
		string str5 = empty;
		AbilityModPropertyInt damageChangePerTargetMod = m_damageChangePerTargetMod;
		int baseVal5;
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
			baseVal5 = samuraiWindBlade.m_damageChangePerTarget;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(damageChangePerTargetMod, "[DamageChangePerTarget]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyEffectInfo laserHitEffectMod = m_laserHitEffectMod;
		object baseVal6;
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
			baseVal6 = samuraiWindBlade.m_laserHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str6 + PropDesc(laserHitEffectMod, "[LaserHitEffect]", flag, (StandardEffectInfo)baseVal6);
		string str7 = empty;
		AbilityModPropertyInt shieldingPerEnemyHitNextTurnMod = m_shieldingPerEnemyHitNextTurnMod;
		int baseVal7;
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
			baseVal7 = samuraiWindBlade.m_shieldingPerEnemyHitNextTurn;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(shieldingPerEnemyHitNextTurnMod, "[ShieldingPerEnemyHitNextTurn]", flag, baseVal7);
		return empty + PropDesc(m_shieldingDurationMod, "[ShieldingDuration]", flag, flag ? samuraiWindBlade.m_shieldingDuration : 0);
	}
}
