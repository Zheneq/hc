using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ArcherHealingDebuffArrow : AbilityMod
{
	[Header("-- Hit")]
	public AbilityModPropertyEffectInfo m_laserHitEffectMod;

	public AbilityModPropertyEffectInfo m_extraHitEffectMod;

	[Header("-- Reaction For Allies Hitting Target")]
	public AbilityModPropertyInt m_reactionHealingMod;

	public AbilityModPropertyInt m_reactionHealingOnSelfMod;

	public AbilityModPropertyInt m_lessHealingOnSubsequentReactions;

	public AbilityModPropertyInt m_healsPerAllyMod;

	public AbilityModPropertyInt m_techPointsPerHealMod;

	public AbilityModPropertyEffectInfo m_reactionEffectMod;

	public AbilityModPropertyInt m_extraHealForShieldGeneratorTargets;

	public AbilityModCooldownReduction m_cooldownReductionIfNoHeals;

	public AbilityModPropertyInt m_extraHealBelowHealthThresholdMod;

	public AbilityModPropertyFloat m_healthThresholdMod;

	public AbilityModPropertyInt m_extraDamageToThisTargetFromCasterMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ArcherHealingDebuffArrow);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ArcherHealingDebuffArrow archerHealingDebuffArrow = targetAbility as ArcherHealingDebuffArrow;
		if (!(archerHealingDebuffArrow != null))
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
			AbilityMod.AddToken_EffectMod(tokens, m_laserHitEffectMod, "LaserHitEffect", archerHealingDebuffArrow.m_laserHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_extraHitEffectMod, "LaserHitEffect");
			AbilityMod.AddToken(tokens, m_reactionHealingMod, "ReactionHealing", string.Empty, archerHealingDebuffArrow.m_reactionHealing);
			AbilityMod.AddToken(tokens, m_reactionHealingOnSelfMod, "ReactionHealingOnSelf", string.Empty, archerHealingDebuffArrow.m_reactionHealingOnSelf);
			AbilityMod.AddToken(tokens, m_lessHealingOnSubsequentReactions, "LessHealingOnSubsequentReactions", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_techPointsPerHealMod, "TechPointsPerHeal", string.Empty, archerHealingDebuffArrow.m_techPointsPerHeal);
			AbilityMod.AddToken_EffectMod(tokens, m_reactionEffectMod, "ReactionEffect", archerHealingDebuffArrow.m_reactionEffect);
			AbilityMod.AddToken(tokens, m_extraHealForShieldGeneratorTargets, "ExtraHealForShieldGeneratorTargets", string.Empty, 0);
			m_cooldownReductionIfNoHeals.AddTooltipTokens(tokens, "CooldownReductionIfNoHeals");
			AbilityMod.AddToken(tokens, m_extraHealBelowHealthThresholdMod, "ExtraHealForAlliesBelowHealthThreshold", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_healthThresholdMod, "HealthThresholdForExtraHealing", string.Empty, 0f, true, false, true);
			AbilityMod.AddToken(tokens, m_extraDamageToThisTargetFromCasterMod, "ExtraDamageToThisTargetFromCaster", string.Empty, 0);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ArcherHealingDebuffArrow archerHealingDebuffArrow = GetTargetAbilityOnAbilityData(abilityData) as ArcherHealingDebuffArrow;
		bool flag = archerHealingDebuffArrow != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyEffectInfo laserHitEffectMod = m_laserHitEffectMod;
		object baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = archerHealingDebuffArrow.m_laserHitEffect;
		}
		else
		{
			baseVal = null;
		}
		empty = str + PropDesc(laserHitEffectMod, "[LaserHitEffect]", flag, (StandardEffectInfo)baseVal);
		empty += PropDesc(m_extraHitEffectMod, "[ExtraHitEffect]");
		empty += PropDesc(m_reactionHealingMod, "[ReactionHealing]", flag, flag ? archerHealingDebuffArrow.m_reactionHealing : 0);
		string str2 = empty;
		AbilityModPropertyInt reactionHealingOnSelfMod = m_reactionHealingOnSelfMod;
		int baseVal2;
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
			baseVal2 = archerHealingDebuffArrow.m_reactionHealingOnSelf;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(reactionHealingOnSelfMod, "[ReactionHealingOnSelf]", flag, baseVal2);
		empty += PropDesc(m_lessHealingOnSubsequentReactions, "[LessHealingOnSubsequentReactions]", flag);
		string str3 = empty;
		AbilityModPropertyInt healsPerAllyMod = m_healsPerAllyMod;
		int baseVal3;
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
			baseVal3 = archerHealingDebuffArrow.m_healsPerAlly;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(healsPerAllyMod, "[NumberOfHealingReactionsPerAlly]", flag, baseVal3);
		empty += PropDesc(m_techPointsPerHealMod, "[TechPointsPerHeal]", flag, flag ? archerHealingDebuffArrow.m_techPointsPerHeal : 0);
		string str4 = empty;
		AbilityModPropertyEffectInfo reactionEffectMod = m_reactionEffectMod;
		object baseVal4;
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
			baseVal4 = archerHealingDebuffArrow.m_reactionEffect;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str4 + PropDesc(reactionEffectMod, "[ReactionEffect]", flag, (StandardEffectInfo)baseVal4);
		empty += PropDesc(m_extraHealForShieldGeneratorTargets, "[ExtraHealForShieldGeneratorTargets]", flag);
		if (m_cooldownReductionIfNoHeals != null && m_cooldownReductionIfNoHeals.HasCooldownReduction())
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
			empty += m_cooldownReductionIfNoHeals.GetDescription(abilityData);
		}
		empty += PropDesc(m_extraHealBelowHealthThresholdMod, "[ExtraHealForAlliesBelowHealthThreshold]", flag);
		empty += PropDesc(m_healthThresholdMod, "[HealthThresholdForExtraHealing]", flag);
		return empty + PropDesc(m_extraDamageToThisTargetFromCasterMod, "[ExtraDamageToThisTargetFromCaster]", flag);
	}
}
