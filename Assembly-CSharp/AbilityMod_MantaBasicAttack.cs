using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_MantaBasicAttack : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_coneWidthAngleMod;

	public AbilityModPropertyFloat m_coneBackwardOffsetMod;

	public AbilityModPropertyFloat m_coneLengthInnerMod;

	public AbilityModPropertyFloat m_coneLengthThroughWallsMod;

	[Header("-- Damage")]
	public AbilityModPropertyInt m_damageAmountInnerMod;

	public AbilityModPropertyInt m_damageAmountThroughWallsMod;

	public AbilityModPropertyInt m_extraDamageNoLoSMod;

	public AbilityModPropertyEffectInfo m_effectInnerMod;

	public AbilityModPropertyEffectInfo m_effectOuterMod;

	[Header("-- Other")]
	public AbilityModPropertyEffectInfo m_additionalDirtyFightingExplosionEffect;

	public AbilityModPropertyBool m_disruptBrushInConeMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(MantaBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		MantaBasicAttack mantaBasicAttack = targetAbility as MantaBasicAttack;
		if (mantaBasicAttack != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_MantaBasicAttack.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, mantaBasicAttack.m_coneWidthAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, mantaBasicAttack.m_coneBackwardOffset, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneLengthInnerMod, "ConeLengthInner", string.Empty, mantaBasicAttack.m_coneLengthInner, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneLengthThroughWallsMod, "ConeLengthThroughWalls", string.Empty, mantaBasicAttack.m_coneLengthThroughWalls, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountInnerMod, "DamageAmountInner", string.Empty, mantaBasicAttack.m_damageAmountInner, true, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountThroughWallsMod, "DamageAmountThroughWalls", string.Empty, mantaBasicAttack.m_damageAmountThroughWalls, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageNoLoSMod, "ExtraDamageNoLoS", string.Empty, 0, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectInnerMod, "EffectInner", mantaBasicAttack.m_effectInner, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOuterMod, "EffectOuter", mantaBasicAttack.m_effectOuter, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_additionalDirtyFightingExplosionEffect, "ExtraBugExplosionEffect", null, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MantaBasicAttack mantaBasicAttack = base.GetTargetAbilityOnAbilityData(abilityData) as MantaBasicAttack;
		bool flag = mantaBasicAttack != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat coneWidthAngleMod = this.m_coneWidthAngleMod;
		string prefix = "[ConeWidthAngle]";
		bool showBaseVal = flag;
		float baseVal;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_MantaBasicAttack.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = mantaBasicAttack.m_coneWidthAngle;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(coneWidthAngleMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat coneBackwardOffsetMod = this.m_coneBackwardOffsetMod;
		string prefix2 = "[ConeBackwardOffset]";
		bool showBaseVal2 = flag;
		float baseVal2;
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
			baseVal2 = mantaBasicAttack.m_coneBackwardOffset;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(coneBackwardOffsetMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat coneLengthInnerMod = this.m_coneLengthInnerMod;
		string prefix3 = "[ConeLengthInner]";
		bool showBaseVal3 = flag;
		float baseVal3;
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
			baseVal3 = mantaBasicAttack.m_coneLengthInner;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(coneLengthInnerMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_coneLengthThroughWallsMod, "[ConeLengthThroughWalls]", flag, (!flag) ? 0f : mantaBasicAttack.m_coneLengthThroughWalls);
		text += base.PropDesc(this.m_damageAmountInnerMod, "[DamageAmountInner]", flag, (!flag) ? 0 : mantaBasicAttack.m_damageAmountInner);
		string str4 = text;
		AbilityModPropertyInt damageAmountThroughWallsMod = this.m_damageAmountThroughWallsMod;
		string prefix4 = "[DamageAmountThroughWalls]";
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
			baseVal4 = mantaBasicAttack.m_damageAmountThroughWalls;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(damageAmountThroughWallsMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_extraDamageNoLoSMod, "[ExtraDamageNoLoS]", flag, 0);
		text += base.PropDesc(this.m_effectInnerMod, "[EffectInner]", flag, (!flag) ? null : mantaBasicAttack.m_effectInner);
		string str5 = text;
		AbilityModPropertyEffectInfo effectOuterMod = this.m_effectOuterMod;
		string prefix5 = "[EffectOuter]";
		bool showBaseVal5 = flag;
		StandardEffectInfo baseVal5;
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
			baseVal5 = mantaBasicAttack.m_effectOuter;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(effectOuterMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_additionalDirtyFightingExplosionEffect, "[ExtraBugExplosionEffect]", flag, null);
		return text + base.PropDesc(this.m_disruptBrushInConeMod, "[DisruptBrushInCone]", flag, false);
	}
}
