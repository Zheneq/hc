using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ClericBasicAttack : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyBool m_penetrateLineOfSightMod;

	public AbilityModPropertyFloat m_coneAngleMod;

	public AbilityModPropertyFloat m_coneLengthInnerMod;

	public AbilityModPropertyFloat m_coneLengthMod;

	public AbilityModPropertyFloat m_coneBackwardOffsetMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	[Header("-- On Hit Damage/Effect")]
	public AbilityModPropertyInt m_damageAmountInnerMod;

	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyEffectInfo m_targetHitEffectInnerMod;

	public AbilityModPropertyEffectInfo m_targetHitEffectMod;

	public AbilityModPropertyInt m_extraDamageToTargetsWhoEvaded;

	[Header("-- Cooldown Reduction")]
	public bool m_useCooldownReductionOverride;

	public AbilityModCooldownReduction m_cooldownReductionOverrideMod;

	public AbilityModPropertyInt m_hitsToIgnoreForCooldownReductionMultiplier;

	[Header("-- Ability interactions")]
	public AbilityModPropertyInt m_extraTechPointGainInAreaBuff;

	public override Type GetTargetAbilityType()
	{
		return typeof(ClericBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ClericBasicAttack clericBasicAttack = targetAbility as ClericBasicAttack;
		if (clericBasicAttack != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ClericBasicAttack.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_coneAngleMod, "ConeAngle", string.Empty, clericBasicAttack.m_coneAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneLengthInnerMod, "ConeLengthInner", string.Empty, clericBasicAttack.m_coneLengthInner, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneLengthMod, "ConeLength", string.Empty, clericBasicAttack.m_coneLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, clericBasicAttack.m_coneBackwardOffset, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargets", string.Empty, clericBasicAttack.m_maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountInnerMod, "DamageAmountInner", string.Empty, clericBasicAttack.m_damageAmountInner, true, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, clericBasicAttack.m_damageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_targetHitEffectInnerMod, "TargetHitEffectInner", clericBasicAttack.m_targetHitEffectInner, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_targetHitEffectMod, "TargetHitEffect", clericBasicAttack.m_targetHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_extraDamageToTargetsWhoEvaded, "ExtraDamageToTargetsWhoEvaded", string.Empty, 0, true, false);
			if (this.m_useCooldownReductionOverride)
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
				this.m_cooldownReductionOverrideMod.AddTooltipTokens(tokens, "CooldownReductionOverride");
				AbilityMod.AddToken(tokens, this.m_hitsToIgnoreForCooldownReductionMultiplier, "HitsToIgnoreForCooldownReductionMultiplier", string.Empty, 0, true, false);
			}
			AbilityMod.AddToken(tokens, this.m_extraTechPointGainInAreaBuff, "ExtraEnergyGainInAreaBuff", string.Empty, 0, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClericBasicAttack clericBasicAttack = base.GetTargetAbilityOnAbilityData(abilityData) as ClericBasicAttack;
		bool flag = clericBasicAttack != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyBool penetrateLineOfSightMod = this.m_penetrateLineOfSightMod;
		string prefix = "[PenetrateLineOfSight]";
		bool showBaseVal = flag;
		bool baseVal;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ClericBasicAttack.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = clericBasicAttack.m_penetrateLineOfSight;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(penetrateLineOfSightMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_coneAngleMod, "[ConeAngle]", flag, (!flag) ? 0f : clericBasicAttack.m_coneAngle);
		text += base.PropDesc(this.m_coneLengthInnerMod, "[ConeLengthInner]", flag, (!flag) ? 0f : clericBasicAttack.m_coneLengthInner);
		string str2 = text;
		AbilityModPropertyFloat coneLengthMod = this.m_coneLengthMod;
		string prefix2 = "[ConeLength]";
		bool showBaseVal2 = flag;
		float baseVal2;
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
			baseVal2 = clericBasicAttack.m_coneLength;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(coneLengthMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, (!flag) ? 0f : clericBasicAttack.m_coneBackwardOffset);
		string str3 = text;
		AbilityModPropertyInt maxTargetsMod = this.m_maxTargetsMod;
		string prefix3 = "[MaxTargets]";
		bool showBaseVal3 = flag;
		int baseVal3;
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
			baseVal3 = clericBasicAttack.m_maxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(maxTargetsMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_damageAmountInnerMod, "[DamageAmountInner]", flag, (!flag) ? 0 : clericBasicAttack.m_damageAmountInner);
		string str4 = text;
		AbilityModPropertyInt damageAmountMod = this.m_damageAmountMod;
		string prefix4 = "[DamageAmount]";
		bool showBaseVal4 = flag;
		int baseVal4;
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
			baseVal4 = clericBasicAttack.m_damageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(damageAmountMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyEffectInfo targetHitEffectInnerMod = this.m_targetHitEffectInnerMod;
		string prefix5 = "[TargetHitEffectInner]";
		bool showBaseVal5 = flag;
		StandardEffectInfo baseVal5;
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
			baseVal5 = clericBasicAttack.m_targetHitEffectInner;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(targetHitEffectInnerMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyEffectInfo targetHitEffectMod = this.m_targetHitEffectMod;
		string prefix6 = "[TargetHitEffect]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal6;
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
			baseVal6 = clericBasicAttack.m_targetHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + base.PropDesc(targetHitEffectMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_extraDamageToTargetsWhoEvaded, "[ExtraDamageToTargetsWhoEvaded]", flag, 0);
		if (this.m_useCooldownReductionOverride)
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
			text += this.m_cooldownReductionOverrideMod.GetDescription(abilityData);
			text += base.PropDesc(this.m_hitsToIgnoreForCooldownReductionMultiplier, "[HitsToIgnoreForCooldownReductionMultiplier]", flag, 0);
		}
		return text + base.PropDesc(this.m_extraTechPointGainInAreaBuff, "[ExtraEnergyGainInAreaBuff]", flag, 0);
	}
}
