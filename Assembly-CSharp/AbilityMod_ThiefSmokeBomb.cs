using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ThiefSmokeBomb : AbilityMod
{
	[Header("-- Bomb Damage")]
	public AbilityModPropertyInt m_extraDamageOnCastMod;

	[Header("-- Bomb Targeting (shape is in Smoke Field Info)")]
	public AbilityModPropertyBool m_penetrateLosMod;

	public AbilityModPropertyInt m_maxAngleWithFirstSegmentMod;

	public AbilityModPropertyFloat m_maxDistanceWithFirstMod;

	public AbilityModPropertyFloat m_minDistanceBetweenBombsMod;

	[Header("-- On Cast Hit Effect")]
	public AbilityModPropertyEffectInfo m_bombHitEffectInfoMod;

	[Header("-- Smoke Field")]
	public AbilityModPropertyGroundEffectField m_smokeFieldInfoMod;

	[Header("-- Barrier (will make square out of 4 barriers around ground field)")]
	public AbilityModPropertyBool m_addBarriersMod;

	public AbilityModPropertyFloat m_barrierSquareWidthMod;

	public AbilityModPropertyBarrierDataV2 m_barrierDataMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ThiefSmokeBomb);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ThiefSmokeBomb thiefSmokeBomb = targetAbility as ThiefSmokeBomb;
		if (thiefSmokeBomb != null)
		{
			AbilityMod.AddToken(tokens, this.m_extraDamageOnCastMod, "ExtraDamageOnCast", string.Empty, thiefSmokeBomb.m_extraDamageOnCast, true, false);
			AbilityMod.AddToken(tokens, this.m_maxAngleWithFirstSegmentMod, "MaxAngleWithFirstSegment", string.Empty, thiefSmokeBomb.m_maxAngleWithFirstSegment, true, false);
			AbilityMod.AddToken(tokens, this.m_maxDistanceWithFirstMod, "MaxDistanceWithFirst", string.Empty, thiefSmokeBomb.m_maxDistanceWithFirst, true, false, false);
			AbilityMod.AddToken(tokens, this.m_minDistanceBetweenBombsMod, "MinDistanceBetweenBombs", string.Empty, thiefSmokeBomb.m_minDistanceBetweenBombs, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_bombHitEffectInfoMod, "BombHitEffectInfo", thiefSmokeBomb.m_bombHitEffectInfo, true);
			AbilityMod.AddToken_GroundFieldMod(tokens, this.m_smokeFieldInfoMod, "SmokeFieldInfo", thiefSmokeBomb.m_smokeFieldInfo);
			AbilityMod.AddToken(tokens, this.m_barrierSquareWidthMod, "BarrierSquareWidth", string.Empty, thiefSmokeBomb.m_barrierSquareWidth, true, false, false);
			AbilityMod.AddToken_BarrierMod(tokens, this.m_barrierDataMod, "BarrierData", thiefSmokeBomb.m_barrierData);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ThiefSmokeBomb thiefSmokeBomb = base.GetTargetAbilityOnAbilityData(abilityData) as ThiefSmokeBomb;
		bool flag = thiefSmokeBomb != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt extraDamageOnCastMod = this.m_extraDamageOnCastMod;
		string prefix = "[ExtraDamageonCast]";
		bool showBaseVal = flag;
		int baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ThiefSmokeBomb.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = thiefSmokeBomb.m_extraDamageOnCast;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(extraDamageOnCastMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyBool penetrateLosMod = this.m_penetrateLosMod;
		string prefix2 = "[PenetrateLos]";
		bool showBaseVal2 = flag;
		bool baseVal2;
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
			baseVal2 = thiefSmokeBomb.m_penetrateLos;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + base.PropDesc(penetrateLosMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_maxAngleWithFirstSegmentMod, "[MaxAngleWithFirstSegment]", flag, (!flag) ? 0 : thiefSmokeBomb.m_maxAngleWithFirstSegment);
		text += base.PropDesc(this.m_maxDistanceWithFirstMod, "[MaxDistanceWithFirst]", flag, (!flag) ? 0f : thiefSmokeBomb.m_maxDistanceWithFirst);
		string str3 = text;
		AbilityModPropertyFloat minDistanceBetweenBombsMod = this.m_minDistanceBetweenBombsMod;
		string prefix3 = "[MinDistanceBetweenBombs]";
		bool showBaseVal3 = flag;
		float baseVal3;
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
			baseVal3 = thiefSmokeBomb.m_minDistanceBetweenBombs;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(minDistanceBetweenBombsMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyEffectInfo bombHitEffectInfoMod = this.m_bombHitEffectInfoMod;
		string prefix4 = "[BombHitEffectInfo]";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal4;
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
			baseVal4 = thiefSmokeBomb.m_bombHitEffectInfo;
		}
		else
		{
			baseVal4 = null;
		}
		text = str4 + base.PropDesc(bombHitEffectInfoMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDescGroundFieldMod(this.m_smokeFieldInfoMod, "{ SmokeFieldInfo }", thiefSmokeBomb.m_smokeFieldInfo);
		string str5 = text;
		AbilityModPropertyBool addBarriersMod = this.m_addBarriersMod;
		string prefix5 = "[AddBarriers]";
		bool showBaseVal5 = flag;
		bool baseVal5;
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
			baseVal5 = thiefSmokeBomb.m_addBarriers;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(addBarriersMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyFloat barrierSquareWidthMod = this.m_barrierSquareWidthMod;
		string prefix6 = "[BarrierSquareWidth]";
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
			baseVal6 = thiefSmokeBomb.m_barrierSquareWidth;
		}
		else
		{
			baseVal6 = 0f;
		}
		text = str6 + base.PropDesc(barrierSquareWidthMod, prefix6, showBaseVal6, baseVal6);
		return text + base.PropDescBarrierMod(this.m_barrierDataMod, "{ BarrierData }", thiefSmokeBomb.m_barrierData);
	}
}
