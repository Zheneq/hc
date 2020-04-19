using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ExoShield : AbilityMod
{
	[Header("-- Shield/Absorb Effect")]
	public AbilityModPropertyEffectData m_absorbEffectMod;

	[Header("-- Extra shielding when using ult")]
	public AbilityModPropertyInt m_extraAbsorbIfSiegingMod;

	[Header("-- Cooldowwn Reduction if no shield used")]
	public AbilityModPropertyInt m_cdrIfShieldNotUsedMod;

	[Header("-- Shielding lost to energy conversion (on effect end")]
	public AbilityModPropertyInt m_shieldLostPerEnergyGainMod;

	public AbilityModPropertyInt m_maxShieldLostForEnergyGainMod;

	[Header("-- (If using energy to shield conversion) Energy to use for conversion, use 0 if there is no max")]
	public AbilityModPropertyInt m_maxTechPointsCostMod;

	public AbilityModPropertyInt m_minTechPointsForCastMod;

	public AbilityModPropertyBool m_freeActionWhileAnchoredMod;

	[Header("-- Targeter shape - use for doing stuff to nearby actors")]
	public AbilityModPropertyShape m_targeterShapeMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ExoShield);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ExoShield exoShield = targetAbility as ExoShield;
		if (exoShield != null)
		{
			AbilityMod.AddToken_EffectMod(tokens, this.m_absorbEffectMod, "AbsorbEffect", exoShield.m_absorbEffect, true);
			AbilityMod.AddToken(tokens, this.m_extraAbsorbIfSiegingMod, "ExtraAbsorbIfSieging", string.Empty, exoShield.m_extraAbsorbIfSieging, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrIfShieldNotUsedMod, "CdrIfShieldNotUsed", string.Empty, exoShield.m_cdrIfShieldNotUsed, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldLostPerEnergyGainMod, "ShieldLostPerEnergyGain", string.Empty, exoShield.m_shieldLostPerEnergyGain, true, false);
			AbilityMod.AddToken(tokens, this.m_maxShieldLostForEnergyGainMod, "MaxShieldLostForEnergyGain", string.Empty, exoShield.m_maxShieldLostForEnergyGain, true, false);
			AbilityMod.AddToken(tokens, this.m_maxTechPointsCostMod, "MaxTechPointsCost", string.Empty, exoShield.m_maxTechPointsCost, true, false);
			AbilityMod.AddToken(tokens, this.m_minTechPointsForCastMod, "MinTechPointsForCast", string.Empty, exoShield.m_minTechPointsForCast, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ExoShield exoShield = base.GetTargetAbilityOnAbilityData(abilityData) as ExoShield;
		bool flag = exoShield != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyEffectData absorbEffectMod = this.m_absorbEffectMod;
		string prefix = "[AbsorbEffect]";
		bool showBaseVal = flag;
		StandardActorEffectData baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ExoShield.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = exoShield.m_absorbEffect;
		}
		else
		{
			baseVal = null;
		}
		text = str + base.PropDesc(absorbEffectMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt extraAbsorbIfSiegingMod = this.m_extraAbsorbIfSiegingMod;
		string prefix2 = "[ExtraAbsorbIfSieging]";
		bool showBaseVal2 = flag;
		int baseVal2;
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
			baseVal2 = exoShield.m_extraAbsorbIfSieging;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(extraAbsorbIfSiegingMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt cdrIfShieldNotUsedMod = this.m_cdrIfShieldNotUsedMod;
		string prefix3 = "[CdrIfShieldNotUsed]";
		bool showBaseVal3 = flag;
		int baseVal3;
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
			baseVal3 = exoShield.m_cdrIfShieldNotUsed;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(cdrIfShieldNotUsedMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_shieldLostPerEnergyGainMod, "[ShieldLostPerEnergyGain]", flag, (!flag) ? 0 : exoShield.m_shieldLostPerEnergyGain);
		string str4 = text;
		AbilityModPropertyInt maxShieldLostForEnergyGainMod = this.m_maxShieldLostForEnergyGainMod;
		string prefix4 = "[MaxShieldLostForEnergyGain]";
		bool showBaseVal4 = flag;
		int baseVal4;
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
			baseVal4 = exoShield.m_maxShieldLostForEnergyGain;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(maxShieldLostForEnergyGainMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_maxTechPointsCostMod, "[MaxTechPointsCost]", flag, (!flag) ? 0 : exoShield.m_maxTechPointsCost);
		text += base.PropDesc(this.m_minTechPointsForCastMod, "[MinTechPointsForCast]", flag, (!flag) ? 0 : exoShield.m_minTechPointsForCast);
		string str5 = text;
		AbilityModPropertyBool freeActionWhileAnchoredMod = this.m_freeActionWhileAnchoredMod;
		string prefix5 = "[FreeActionWhileAnchored]";
		bool showBaseVal5 = flag;
		bool baseVal5;
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
			baseVal5 = exoShield.m_freeActionWhileAnchored;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(freeActionWhileAnchoredMod, prefix5, showBaseVal5, baseVal5);
		return text + base.PropDesc(this.m_targeterShapeMod, "[TargeterShape]", flag, (!flag) ? AbilityAreaShape.SingleSquare : exoShield.m_targeterShape);
	}
}
