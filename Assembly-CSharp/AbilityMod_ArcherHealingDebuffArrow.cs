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
		if (archerHealingDebuffArrow != null)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ArcherHealingDebuffArrow.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken_EffectMod(tokens, this.m_laserHitEffectMod, "LaserHitEffect", archerHealingDebuffArrow.m_laserHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_extraHitEffectMod, "LaserHitEffect", null, true);
			AbilityMod.AddToken(tokens, this.m_reactionHealingMod, "ReactionHealing", string.Empty, archerHealingDebuffArrow.m_reactionHealing, true, false);
			AbilityMod.AddToken(tokens, this.m_reactionHealingOnSelfMod, "ReactionHealingOnSelf", string.Empty, archerHealingDebuffArrow.m_reactionHealingOnSelf, true, false);
			AbilityMod.AddToken(tokens, this.m_lessHealingOnSubsequentReactions, "LessHealingOnSubsequentReactions", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_techPointsPerHealMod, "TechPointsPerHeal", string.Empty, archerHealingDebuffArrow.m_techPointsPerHeal, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_reactionEffectMod, "ReactionEffect", archerHealingDebuffArrow.m_reactionEffect, true);
			AbilityMod.AddToken(tokens, this.m_extraHealForShieldGeneratorTargets, "ExtraHealForShieldGeneratorTargets", string.Empty, 0, true, false);
			this.m_cooldownReductionIfNoHeals.AddTooltipTokens(tokens, "CooldownReductionIfNoHeals");
			AbilityMod.AddToken(tokens, this.m_extraHealBelowHealthThresholdMod, "ExtraHealForAlliesBelowHealthThreshold", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_healthThresholdMod, "HealthThresholdForExtraHealing", string.Empty, 0f, true, false, true);
			AbilityMod.AddToken(tokens, this.m_extraDamageToThisTargetFromCasterMod, "ExtraDamageToThisTargetFromCaster", string.Empty, 0, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ArcherHealingDebuffArrow archerHealingDebuffArrow = base.GetTargetAbilityOnAbilityData(abilityData) as ArcherHealingDebuffArrow;
		bool flag = archerHealingDebuffArrow != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyEffectInfo laserHitEffectMod = this.m_laserHitEffectMod;
		string prefix = "[LaserHitEffect]";
		bool showBaseVal = flag;
		StandardEffectInfo baseVal;
		if (flag)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ArcherHealingDebuffArrow.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = archerHealingDebuffArrow.m_laserHitEffect;
		}
		else
		{
			baseVal = null;
		}
		text = str + base.PropDesc(laserHitEffectMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_extraHitEffectMod, "[ExtraHitEffect]", false, null);
		text += base.PropDesc(this.m_reactionHealingMod, "[ReactionHealing]", flag, (!flag) ? 0 : archerHealingDebuffArrow.m_reactionHealing);
		string str2 = text;
		AbilityModPropertyInt reactionHealingOnSelfMod = this.m_reactionHealingOnSelfMod;
		string prefix2 = "[ReactionHealingOnSelf]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
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
			baseVal2 = archerHealingDebuffArrow.m_reactionHealingOnSelf;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(reactionHealingOnSelfMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_lessHealingOnSubsequentReactions, "[LessHealingOnSubsequentReactions]", flag, 0);
		string str3 = text;
		AbilityModPropertyInt healsPerAllyMod = this.m_healsPerAllyMod;
		string prefix3 = "[NumberOfHealingReactionsPerAlly]";
		bool showBaseVal3 = flag;
		int baseVal3;
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
			baseVal3 = archerHealingDebuffArrow.m_healsPerAlly;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(healsPerAllyMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_techPointsPerHealMod, "[TechPointsPerHeal]", flag, (!flag) ? 0 : archerHealingDebuffArrow.m_techPointsPerHeal);
		string str4 = text;
		AbilityModPropertyEffectInfo reactionEffectMod = this.m_reactionEffectMod;
		string prefix4 = "[ReactionEffect]";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal4;
		if (flag)
		{
			for (;;)
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
		text = str4 + base.PropDesc(reactionEffectMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_extraHealForShieldGeneratorTargets, "[ExtraHealForShieldGeneratorTargets]", flag, 0);
		if (this.m_cooldownReductionIfNoHeals != null && this.m_cooldownReductionIfNoHeals.HasCooldownReduction())
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			text += this.m_cooldownReductionIfNoHeals.GetDescription(abilityData);
		}
		text += base.PropDesc(this.m_extraHealBelowHealthThresholdMod, "[ExtraHealForAlliesBelowHealthThreshold]", flag, 0);
		text += base.PropDesc(this.m_healthThresholdMod, "[HealthThresholdForExtraHealing]", flag, 0f);
		return text + base.PropDesc(this.m_extraDamageToThisTargetFromCasterMod, "[ExtraDamageToThisTargetFromCaster]", flag, 0);
	}
}
