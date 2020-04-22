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
		if (!(mantaBasicAttack != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_coneWidthAngleMod, "ConeWidthAngle", string.Empty, mantaBasicAttack.m_coneWidthAngle);
			AbilityMod.AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, mantaBasicAttack.m_coneBackwardOffset);
			AbilityMod.AddToken(tokens, m_coneLengthInnerMod, "ConeLengthInner", string.Empty, mantaBasicAttack.m_coneLengthInner);
			AbilityMod.AddToken(tokens, m_coneLengthThroughWallsMod, "ConeLengthThroughWalls", string.Empty, mantaBasicAttack.m_coneLengthThroughWalls);
			AbilityMod.AddToken(tokens, m_damageAmountInnerMod, "DamageAmountInner", string.Empty, mantaBasicAttack.m_damageAmountInner);
			AbilityMod.AddToken(tokens, m_damageAmountThroughWallsMod, "DamageAmountThroughWalls", string.Empty, mantaBasicAttack.m_damageAmountThroughWalls);
			AbilityMod.AddToken(tokens, m_extraDamageNoLoSMod, "ExtraDamageNoLoS", string.Empty, 0);
			AbilityMod.AddToken_EffectMod(tokens, m_effectInnerMod, "EffectInner", mantaBasicAttack.m_effectInner);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOuterMod, "EffectOuter", mantaBasicAttack.m_effectOuter);
			AbilityMod.AddToken_EffectMod(tokens, m_additionalDirtyFightingExplosionEffect, "ExtraBugExplosionEffect");
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MantaBasicAttack mantaBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as MantaBasicAttack;
		bool flag = mantaBasicAttack != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat coneWidthAngleMod = m_coneWidthAngleMod;
		float baseVal;
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
			baseVal = mantaBasicAttack.m_coneWidthAngle;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(coneWidthAngleMod, "[ConeWidthAngle]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat coneBackwardOffsetMod = m_coneBackwardOffsetMod;
		float baseVal2;
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
			baseVal2 = mantaBasicAttack.m_coneBackwardOffset;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat coneLengthInnerMod = m_coneLengthInnerMod;
		float baseVal3;
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
			baseVal3 = mantaBasicAttack.m_coneLengthInner;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(coneLengthInnerMod, "[ConeLengthInner]", flag, baseVal3);
		empty += PropDesc(m_coneLengthThroughWallsMod, "[ConeLengthThroughWalls]", flag, (!flag) ? 0f : mantaBasicAttack.m_coneLengthThroughWalls);
		empty += PropDesc(m_damageAmountInnerMod, "[DamageAmountInner]", flag, flag ? mantaBasicAttack.m_damageAmountInner : 0);
		string str4 = empty;
		AbilityModPropertyInt damageAmountThroughWallsMod = m_damageAmountThroughWallsMod;
		int baseVal4;
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
			baseVal4 = mantaBasicAttack.m_damageAmountThroughWalls;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(damageAmountThroughWallsMod, "[DamageAmountThroughWalls]", flag, baseVal4);
		empty += PropDesc(m_extraDamageNoLoSMod, "[ExtraDamageNoLoS]", flag);
		empty += PropDesc(m_effectInnerMod, "[EffectInner]", flag, (!flag) ? null : mantaBasicAttack.m_effectInner);
		string str5 = empty;
		AbilityModPropertyEffectInfo effectOuterMod = m_effectOuterMod;
		object baseVal5;
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
			baseVal5 = mantaBasicAttack.m_effectOuter;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + PropDesc(effectOuterMod, "[EffectOuter]", flag, (StandardEffectInfo)baseVal5);
		empty += PropDesc(m_additionalDirtyFightingExplosionEffect, "[ExtraBugExplosionEffect]", flag);
		return empty + PropDesc(m_disruptBrushInConeMod, "[DisruptBrushInCone]", flag);
	}
}
