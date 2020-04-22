using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ArcherBendingArrow : AbilityMod
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

	public AbilityModPropertyEffectInfo m_laserHitEffectMod;

	public AbilityModPropertyInt m_extraDamageToHealingDebuffTarget;

	public AbilityModPropertyInt m_extraDamageAfterBend;

	public AbilityModPropertyInt m_damageAfterPiercingMod;

	public AbilityModPropertyEffectInfo m_effectToHealingDebuffTarget;

	public AbilityModPropertyInt m_extraHealingFromHealingDebuffTarget;

	[Header("-- Misc Ability Interactions")]
	public AbilityModPropertyInt m_nextShieldGeneratorExtraAbsorbPerHit;

	public AbilityModPropertyInt m_nextShieldGeneratorExtraAbsorbMax;

	public override Type GetTargetAbilityType()
	{
		return typeof(ArcherBendingArrow);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ArcherBendingArrow archerBendingArrow = targetAbility as ArcherBendingArrow;
		if (!(archerBendingArrow != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, archerBendingArrow.m_laserWidth);
			AbilityMod.AddToken(tokens, m_minRangeBeforeBendMod, "MinRangeBeforeBend", string.Empty, archerBendingArrow.m_minRangeBeforeBend);
			AbilityMod.AddToken(tokens, m_maxRangeBeforeBendMod, "MaxRangeBeforeBend", string.Empty, archerBendingArrow.m_maxRangeBeforeBend);
			AbilityMod.AddToken(tokens, m_maxTotalRangeMod, "MaxTotalRange", string.Empty, archerBendingArrow.m_maxTotalRange);
			AbilityMod.AddToken(tokens, m_maxBendAngleMod, "MaxBendAngle", string.Empty, archerBendingArrow.m_maxBendAngle);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, archerBendingArrow.m_maxTargets);
			AbilityMod.AddToken(tokens, m_laserDamageAmountMod, "LaserDamageAmount", string.Empty, archerBendingArrow.m_laserDamageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_laserHitEffectMod, "LaserHitEffect", archerBendingArrow.m_laserHitEffect);
			AbilityMod.AddToken(tokens, m_nextShieldGeneratorExtraAbsorbPerHit, "NextShieldGeneratorExtraAbsorbPerHit", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_nextShieldGeneratorExtraAbsorbMax, "NextShieldGeneratorExtraAbsorbMax", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_extraDamageToHealingDebuffTarget, "ExtraDamageToHealingDebuffTarget", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_extraDamageAfterBend, "ExtraDamageAfterBend", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_damageAfterPiercingMod, "DamageToSubsequentTargetsAfterPiercing", string.Empty, archerBendingArrow.m_laserDamageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_effectToHealingDebuffTarget, "EffectToHealingDebuffTarget");
			AbilityMod.AddToken(tokens, m_extraHealingFromHealingDebuffTarget, "ExtraHealingFromHealingDebuffTarget", string.Empty, 0);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ArcherBendingArrow archerBendingArrow = GetTargetAbilityOnAbilityData(abilityData) as ArcherBendingArrow;
		bool flag = archerBendingArrow != null;
		string empty = string.Empty;
		empty += PropDesc(m_laserWidthMod, "[LaserWidth]", flag, (!flag) ? 0f : archerBendingArrow.m_laserWidth);
		string str = empty;
		AbilityModPropertyFloat minRangeBeforeBendMod = m_minRangeBeforeBendMod;
		float baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = archerBendingArrow.m_minRangeBeforeBend;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(minRangeBeforeBendMod, "[MinRangeBeforeBend]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat maxRangeBeforeBendMod = m_maxRangeBeforeBendMod;
		float baseVal2;
		if (flag)
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
			baseVal2 = archerBendingArrow.m_maxRangeBeforeBend;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(maxRangeBeforeBendMod, "[MaxRangeBeforeBend]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat maxTotalRangeMod = m_maxTotalRangeMod;
		float baseVal3;
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
			baseVal3 = archerBendingArrow.m_maxTotalRange;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(maxTotalRangeMod, "[MaxTotalRange]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat maxBendAngleMod = m_maxBendAngleMod;
		float baseVal4;
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
			baseVal4 = archerBendingArrow.m_maxBendAngle;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(maxBendAngleMod, "[MaxBendAngle]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyBool penetrateLoSMod = m_penetrateLoSMod;
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
			baseVal5 = (archerBendingArrow.m_penetrateLoS ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(penetrateLoSMod, "[PenetrateLoS]", flag, (byte)baseVal5 != 0);
		string str6 = empty;
		AbilityModPropertyInt maxTargetsMod = m_maxTargetsMod;
		int baseVal6;
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
			baseVal6 = archerBendingArrow.m_maxTargets;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(maxTargetsMod, "[MaxTargets]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyInt laserDamageAmountMod = m_laserDamageAmountMod;
		int baseVal7;
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
			baseVal7 = archerBendingArrow.m_laserDamageAmount;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(laserDamageAmountMod, "[LaserDamageAmount]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyEffectInfo laserHitEffectMod = m_laserHitEffectMod;
		object baseVal8;
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
			baseVal8 = archerBendingArrow.m_laserHitEffect;
		}
		else
		{
			baseVal8 = null;
		}
		empty = str8 + PropDesc(laserHitEffectMod, "[LaserHitEffect]", flag, (StandardEffectInfo)baseVal8);
		empty += PropDesc(m_nextShieldGeneratorExtraAbsorbPerHit, "[NextShieldGeneratorExtraAbsorbPerHit]", flag);
		empty += PropDesc(m_nextShieldGeneratorExtraAbsorbMax, "[NextShieldGeneratorExtraAbsorbMax]", flag);
		empty += PropDesc(m_extraDamageToHealingDebuffTarget, "[ExtraDamageToHealingDebuffTarget]", flag);
		empty += PropDesc(m_extraDamageAfterBend, "[ExtraDamageAfterBend]", flag);
		string str9 = empty;
		AbilityModPropertyInt damageAfterPiercingMod = m_damageAfterPiercingMod;
		int baseVal9;
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
			baseVal9 = archerBendingArrow.m_laserDamageAmount;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(damageAfterPiercingMod, "[DamageToSubsequentTargetsAfterPiercing]", flag, baseVal9);
		empty += PropDesc(m_effectToHealingDebuffTarget, "[EffectToHealingDebuffTarget]");
		return empty + PropDesc(m_extraHealingFromHealingDebuffTarget, "[ExtraHealingFromHealingDebuffTarget]", flag);
	}
}
