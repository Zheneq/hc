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
			AbilityMod.AddToken_EffectMod(tokens, m_absorbEffectMod, "AbsorbEffect", exoShield.m_absorbEffect);
			AbilityMod.AddToken(tokens, m_extraAbsorbIfSiegingMod, "ExtraAbsorbIfSieging", string.Empty, exoShield.m_extraAbsorbIfSieging);
			AbilityMod.AddToken(tokens, m_cdrIfShieldNotUsedMod, "CdrIfShieldNotUsed", string.Empty, exoShield.m_cdrIfShieldNotUsed);
			AbilityMod.AddToken(tokens, m_shieldLostPerEnergyGainMod, "ShieldLostPerEnergyGain", string.Empty, exoShield.m_shieldLostPerEnergyGain);
			AbilityMod.AddToken(tokens, m_maxShieldLostForEnergyGainMod, "MaxShieldLostForEnergyGain", string.Empty, exoShield.m_maxShieldLostForEnergyGain);
			AbilityMod.AddToken(tokens, m_maxTechPointsCostMod, "MaxTechPointsCost", string.Empty, exoShield.m_maxTechPointsCost);
			AbilityMod.AddToken(tokens, m_minTechPointsForCastMod, "MinTechPointsForCast", string.Empty, exoShield.m_minTechPointsForCast);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ExoShield exoShield = GetTargetAbilityOnAbilityData(abilityData) as ExoShield;
		bool flag = exoShield != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyEffectData absorbEffectMod = m_absorbEffectMod;
		object baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = exoShield.m_absorbEffect;
		}
		else
		{
			baseVal = null;
		}
		empty = str + PropDesc(absorbEffectMod, "[AbsorbEffect]", flag, (StandardActorEffectData)baseVal);
		string str2 = empty;
		AbilityModPropertyInt extraAbsorbIfSiegingMod = m_extraAbsorbIfSiegingMod;
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
			baseVal2 = exoShield.m_extraAbsorbIfSieging;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(extraAbsorbIfSiegingMod, "[ExtraAbsorbIfSieging]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt cdrIfShieldNotUsedMod = m_cdrIfShieldNotUsedMod;
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
			baseVal3 = exoShield.m_cdrIfShieldNotUsed;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(cdrIfShieldNotUsedMod, "[CdrIfShieldNotUsed]", flag, baseVal3);
		empty += PropDesc(m_shieldLostPerEnergyGainMod, "[ShieldLostPerEnergyGain]", flag, flag ? exoShield.m_shieldLostPerEnergyGain : 0);
		string str4 = empty;
		AbilityModPropertyInt maxShieldLostForEnergyGainMod = m_maxShieldLostForEnergyGainMod;
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
			baseVal4 = exoShield.m_maxShieldLostForEnergyGain;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(maxShieldLostForEnergyGainMod, "[MaxShieldLostForEnergyGain]", flag, baseVal4);
		empty += PropDesc(m_maxTechPointsCostMod, "[MaxTechPointsCost]", flag, flag ? exoShield.m_maxTechPointsCost : 0);
		empty += PropDesc(m_minTechPointsForCastMod, "[MinTechPointsForCast]", flag, flag ? exoShield.m_minTechPointsForCast : 0);
		string str5 = empty;
		AbilityModPropertyBool freeActionWhileAnchoredMod = m_freeActionWhileAnchoredMod;
		int baseVal5;
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
			baseVal5 = (exoShield.m_freeActionWhileAnchored ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(freeActionWhileAnchoredMod, "[FreeActionWhileAnchored]", flag, (byte)baseVal5 != 0);
		return empty + PropDesc(m_targeterShapeMod, "[TargeterShape]", flag, flag ? exoShield.m_targeterShape : AbilityAreaShape.SingleSquare);
	}
}
