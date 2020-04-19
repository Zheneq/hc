using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ValkyrieGuard : AbilityMod
{
	[Header("-- Shield effect")]
	public AbilityModPropertyEffectInfo m_shieldEffectInfoMod;

	[Header("-- Hit reactions")]
	public AbilityModPropertyInt m_techPointGainPerCoveredHitMod;

	public AbilityModPropertyInt m_techPointGainPerTooCloseForCoverHitMod;

	public AbilityModPropertyEffectInfo m_coveredHitReactionEffectMod;

	public AbilityModPropertyEffectInfo m_tooCloseForCoverHitReactionEffectMod;

	public AbilityModPropertyInt m_extraDamageNextShieldThrowPerCoveredHitMod;

	public AbilityModPropertyInt m_maxExtraDamageNextShieldThrow;

	[Header("-- Duration --")]
	public AbilityModPropertyInt m_coverDurationMod;

	public AbilityModPropertyBool m_coverLastsForeverMod;

	[Header("-- Cooldown reduction")]
	public AbilityModCooldownReduction m_cooldownReductionNoBlocks;

	[Header("-- Cover Ignore Min Dist?")]
	public AbilityModPropertyBool m_coverIgnoreMinDistMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ValkyrieGuard);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ValkyrieGuard valkyrieGuard = targetAbility as ValkyrieGuard;
		if (valkyrieGuard != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ValkyrieGuard.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken_EffectMod(tokens, this.m_shieldEffectInfoMod, "ShieldEffectInfo", valkyrieGuard.m_shieldEffectInfo, true);
			AbilityMod.AddToken(tokens, this.m_techPointGainPerCoveredHitMod, "TechPointGainPerCoveredHit", string.Empty, valkyrieGuard.m_techPointGainPerCoveredHit, true, false);
			AbilityMod.AddToken(tokens, this.m_techPointGainPerTooCloseForCoverHitMod, "TechPointGainPerTooCloseForCoverHit", string.Empty, valkyrieGuard.m_techPointGainPerTooCloseForCoverHit, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_coveredHitReactionEffectMod, "CoveredHitReactionEffect", valkyrieGuard.m_coveredHitReactionEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_tooCloseForCoverHitReactionEffectMod, "TooCloseForCoverHitReactionEffect", valkyrieGuard.m_tooCloseForCoverHitReactionEffect, true);
			AbilityMod.AddToken(tokens, this.m_extraDamageNextShieldThrowPerCoveredHitMod, "ExtraDamageNextShieldThrowPerCoveredHit", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_maxExtraDamageNextShieldThrow, "MaxExtraDamageNextShieldThrow", string.Empty, 0, true, false);
			AbilityMod.AddToken(tokens, this.m_coverDurationMod, "CoverDuration", string.Empty, valkyrieGuard.m_coverDuration, true, false);
			this.m_cooldownReductionNoBlocks.AddTooltipTokens(tokens, "CooldownReductionNoBlocks");
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ValkyrieGuard valkyrieGuard = base.GetTargetAbilityOnAbilityData(abilityData) as ValkyrieGuard;
		bool flag = valkyrieGuard != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_shieldEffectInfoMod, "[ShieldEffectInfo]", flag, (!flag) ? null : valkyrieGuard.m_shieldEffectInfo);
		string str = text;
		AbilityModPropertyInt techPointGainPerCoveredHitMod = this.m_techPointGainPerCoveredHitMod;
		string prefix = "[TechPointGainPerCoveredHit]";
		bool showBaseVal = flag;
		int baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ValkyrieGuard.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = valkyrieGuard.m_techPointGainPerCoveredHit;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(techPointGainPerCoveredHitMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_techPointGainPerTooCloseForCoverHitMod, "[TechPointGainPerTooCloseForCoverHit]", flag, (!flag) ? 0 : valkyrieGuard.m_techPointGainPerTooCloseForCoverHit);
		string str2 = text;
		AbilityModPropertyEffectInfo coveredHitReactionEffectMod = this.m_coveredHitReactionEffectMod;
		string prefix2 = "[CoveredHitReactionEffect]";
		bool showBaseVal2 = flag;
		StandardEffectInfo baseVal2;
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
			baseVal2 = valkyrieGuard.m_coveredHitReactionEffect;
		}
		else
		{
			baseVal2 = null;
		}
		text = str2 + base.PropDesc(coveredHitReactionEffectMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyEffectInfo tooCloseForCoverHitReactionEffectMod = this.m_tooCloseForCoverHitReactionEffectMod;
		string prefix3 = "[TooCloseForCoverHitReactionEffect]";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal3;
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
			baseVal3 = valkyrieGuard.m_tooCloseForCoverHitReactionEffect;
		}
		else
		{
			baseVal3 = null;
		}
		text = str3 + base.PropDesc(tooCloseForCoverHitReactionEffectMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_extraDamageNextShieldThrowPerCoveredHitMod, "[ExtraDamageNextShieldThrowPerCoveredHit]", flag, 0);
		text += base.PropDesc(this.m_maxExtraDamageNextShieldThrow, "[MaxExtraDamageNextShieldThrow]", flag, 0);
		string str4 = text;
		AbilityModPropertyInt coverDurationMod = this.m_coverDurationMod;
		string prefix4 = "[CoverDuration]";
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
			baseVal4 = valkyrieGuard.m_coverDuration;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(coverDurationMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool coverLastsForeverMod = this.m_coverLastsForeverMod;
		string prefix5 = "[CoverLastsForever]";
		bool showBaseVal5 = flag;
		bool baseVal5;
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
			baseVal5 = valkyrieGuard.m_coverLastsForever;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(coverLastsForeverMod, prefix5, showBaseVal5, baseVal5);
		text += this.m_cooldownReductionNoBlocks.GetDescription(abilityData);
		string str6 = text;
		AbilityModPropertyBool coverIgnoreMinDistMod = this.m_coverIgnoreMinDistMod;
		string prefix6 = "[CoverIgnoreMinDist]";
		bool showBaseVal6 = flag;
		bool baseVal6;
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
			baseVal6 = valkyrieGuard.m_coverIgnoreMinDist;
		}
		else
		{
			baseVal6 = false;
		}
		return str6 + base.PropDesc(coverIgnoreMinDistMod, prefix6, showBaseVal6, baseVal6);
	}
}
