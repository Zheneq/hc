using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BlasterKnockbackCone : AbilityMod
{
	[Header("-- Cone Limits")]
	public AbilityModPropertyFloat m_minLengthMod;

	public AbilityModPropertyFloat m_maxLengthMod;

	public AbilityModPropertyFloat m_minAngleMod;

	public AbilityModPropertyFloat m_maxAngleMod;

	public AbilityModPropertyFloat m_coneBackwardOffsetMod;

	public AbilityModPropertyBool m_penetrateLineOfSightMod;

	[Header("-- On Hit")]
	public AbilityModPropertyInt m_damageAmountNormalMod;

	public AbilityModPropertyEffectInfo m_enemyEffectNormalMod;

	public AbilityModPropertyEffectInfo m_enemyEffectOverchargedMod;

	[Header("-- Knockback on Enemy")]
	public AbilityModPropertyFloat m_knockbackDistanceMod;

	public AbilityModPropertyFloat m_extraKnockbackDistOnOverchargedMod;

	[Header("-- Knockback on Self")]
	public AbilityModPropertyFloat m_knockbackDistanceOnSelfMod;

	[Header("-- Set Overcharge as Free Action after cast?")]
	public AbilityModPropertyBool m_overchargeAsFreeActionAfterCastMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(BlasterKnockbackCone);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BlasterKnockbackCone blasterKnockbackCone = targetAbility as BlasterKnockbackCone;
		if (blasterKnockbackCone != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_BlasterKnockbackCone.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_minLengthMod, "MinLength", string.Empty, blasterKnockbackCone.m_minLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxLengthMod, "MaxLength", string.Empty, blasterKnockbackCone.m_maxLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_minAngleMod, "MinAngle", string.Empty, blasterKnockbackCone.m_minAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxAngleMod, "MaxAngle", string.Empty, blasterKnockbackCone.m_maxAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, blasterKnockbackCone.m_coneBackwardOffset, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountNormalMod, "DamageAmountNormal", string.Empty, blasterKnockbackCone.m_damageAmountNormal, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyEffectNormalMod, "EnemyEffectNormal", blasterKnockbackCone.m_enemyEffectNormal, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyEffectOverchargedMod, "EnemyEffectOvercharged", blasterKnockbackCone.m_enemyEffectOvercharged, true);
			AbilityMod.AddToken(tokens, this.m_knockbackDistanceMod, "KnockbackDistance", string.Empty, blasterKnockbackCone.m_knockbackDistance, true, false, false);
			AbilityMod.AddToken(tokens, this.m_extraKnockbackDistOnOverchargedMod, "ExtraKnockbackDistOnOvercharged", string.Empty, blasterKnockbackCone.m_extraKnockbackDistOnOvercharged, true, false, false);
			AbilityMod.AddToken(tokens, this.m_knockbackDistanceOnSelfMod, "KnockbackDistanceOnSelf", string.Empty, blasterKnockbackCone.m_knockbackDistanceOnSelf, true, false, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BlasterKnockbackCone blasterKnockbackCone = base.GetTargetAbilityOnAbilityData(abilityData) as BlasterKnockbackCone;
		bool flag = blasterKnockbackCone != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_minLengthMod, "[MinLength]", flag, (!flag) ? 0f : blasterKnockbackCone.m_minLength);
		string str = text;
		AbilityModPropertyFloat maxLengthMod = this.m_maxLengthMod;
		string prefix = "[MaxLength]";
		bool showBaseVal = flag;
		float baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_BlasterKnockbackCone.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = blasterKnockbackCone.m_maxLength;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(maxLengthMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat minAngleMod = this.m_minAngleMod;
		string prefix2 = "[MinAngle]";
		bool showBaseVal2 = flag;
		float baseVal2;
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
			baseVal2 = blasterKnockbackCone.m_minAngle;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(minAngleMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_maxAngleMod, "[MaxAngle]", flag, (!flag) ? 0f : blasterKnockbackCone.m_maxAngle);
		string str3 = text;
		AbilityModPropertyFloat coneBackwardOffsetMod = this.m_coneBackwardOffsetMod;
		string prefix3 = "[ConeBackwardOffset]";
		bool showBaseVal3 = flag;
		float baseVal3;
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
			baseVal3 = blasterKnockbackCone.m_coneBackwardOffset;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(coneBackwardOffsetMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, flag && blasterKnockbackCone.m_penetrateLineOfSight);
		string str4 = text;
		AbilityModPropertyInt damageAmountNormalMod = this.m_damageAmountNormalMod;
		string prefix4 = "[DamageAmountNormal]";
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
			baseVal4 = blasterKnockbackCone.m_damageAmountNormal;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(damageAmountNormalMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_enemyEffectNormalMod, "[EnemyEffectNormal]", flag, (!flag) ? null : blasterKnockbackCone.m_enemyEffectNormal);
		string str5 = text;
		AbilityModPropertyEffectInfo enemyEffectOverchargedMod = this.m_enemyEffectOverchargedMod;
		string prefix5 = "[EnemyEffectOvercharged]";
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
			baseVal5 = blasterKnockbackCone.m_enemyEffectOvercharged;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(enemyEffectOverchargedMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyFloat knockbackDistanceMod = this.m_knockbackDistanceMod;
		string prefix6 = "[KnockbackDistance]";
		bool showBaseVal6 = flag;
		float baseVal6;
		if (flag)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal6 = blasterKnockbackCone.m_knockbackDistance;
		}
		else
		{
			baseVal6 = 0f;
		}
		text = str6 + base.PropDesc(knockbackDistanceMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_extraKnockbackDistOnOverchargedMod, "[ExtraKnockbackDistOnOvercharged]", flag, (!flag) ? 0f : blasterKnockbackCone.m_extraKnockbackDistOnOvercharged);
		string str7 = text;
		AbilityModPropertyFloat knockbackDistanceOnSelfMod = this.m_knockbackDistanceOnSelfMod;
		string prefix7 = "[KnockbackDistanceOnSelf]";
		bool showBaseVal7 = flag;
		float baseVal7;
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
			baseVal7 = blasterKnockbackCone.m_knockbackDistanceOnSelf;
		}
		else
		{
			baseVal7 = 0f;
		}
		text = str7 + base.PropDesc(knockbackDistanceOnSelfMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyBool overchargeAsFreeActionAfterCastMod = this.m_overchargeAsFreeActionAfterCastMod;
		string prefix8 = "[OverchargeAsFreeActionAfterCast]";
		bool showBaseVal8 = flag;
		bool baseVal8;
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
			baseVal8 = blasterKnockbackCone.m_overchargeAsFreeActionAfterCast;
		}
		else
		{
			baseVal8 = false;
		}
		return str8 + base.PropDesc(overchargeAsFreeActionAfterCastMod, prefix8, showBaseVal8, baseVal8);
	}
}
