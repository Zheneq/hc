using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_GremlinsMultiTargeterApocolypse : AbilityMod
{
	[Header("-- Damage Mods")]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyInt m_subsequentDamageMod;

	[Header("-- Leave Landmine on Empty Squares?")]
	public AbilityModPropertyBool m_leaveLandmineOnEmptySquaresMod;

	[Header("-- Energy Gain per Miss (no enemy hit)--")]
	public AbilityModPropertyInt m_energyGainPerMissMod;

	[Header("-- Targeting Mods")]
	public AbilityModPropertyShape m_shapeMod;

	public AbilityModPropertyBool m_penetrateLosMod;

	public AbilityModPropertyFloat m_minDistanceBetweenBombsMod;

	public AbilityModPropertyFloat m_maxAngleWithFirstMod;

	[Space(10f)]
	[Header("-- Global Mine Data Mods")]
	public AbilityModPropertyInt m_mineDamageMod;

	public AbilityModPropertyInt m_mineDurationMod;

	public AbilityModPropertyEffectInfo m_effectOnEnemyOverride;

	public AbilityModPropertyInt m_energyOnMineExplosionMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(GremlinsMultiTargeterApocolypse);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		GremlinsMultiTargeterApocolypse gremlinsMultiTargeterApocolypse = targetAbility as GremlinsMultiTargeterApocolypse;
		if (gremlinsMultiTargeterApocolypse != null)
		{
			AbilityMod.AddToken(tokens, this.m_energyGainPerMissMod, "EnergyGainPerMiss", string.Empty, gremlinsMultiTargeterApocolypse.m_energyGainPerMiss, true, false);
			AbilityMod.AddToken(tokens, this.m_minDistanceBetweenBombsMod, "MinDistanceBetweenBombs", string.Empty, gremlinsMultiTargeterApocolypse.m_minDistanceBetweenBombs, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxAngleWithFirstMod, "MaxAngleWithFirst", string.Empty, gremlinsMultiTargeterApocolypse.m_maxAngleWithFirst, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageMod, "BombDamageAmount", string.Empty, gremlinsMultiTargeterApocolypse.m_bombDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_subsequentDamageMod, "BombSubsequentDamageAmount", string.Empty, gremlinsMultiTargeterApocolypse.m_bombSubsequentDamageAmount, true, false);
			if (this.m_useTargetDataOverrides)
			{
				if (this.m_targetDataOverrides != null)
				{
					int val = this.m_targetDataOverrides.Length;
					int otherVal = gremlinsMultiTargeterApocolypse.m_targetData.Length;
					AbilityMod.AddToken_IntDiff(tokens, "NumBombs", string.Empty, val, true, otherVal);
					if (this.m_targetDataOverrides.Length > 0)
					{
						if (gremlinsMultiTargeterApocolypse.m_targetData.Length > 0)
						{
							AbilityMod.AddToken_IntDiff(tokens, "TargeterRange_Diff", string.Empty, Mathf.RoundToInt(this.m_targetDataOverrides[0].m_range - gremlinsMultiTargeterApocolypse.m_targetData[0].m_range), false, 0);
						}
					}
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		GremlinsMultiTargeterApocolypse gremlinsMultiTargeterApocolypse = base.GetTargetAbilityOnAbilityData(abilityData) as GremlinsMultiTargeterApocolypse;
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent;
		if (gremlinsMultiTargeterApocolypse != null)
		{
			gremlinsLandMineInfoComponent = gremlinsMultiTargeterApocolypse.GetComponent<GremlinsLandMineInfoComponent>();
		}
		else
		{
			gremlinsLandMineInfoComponent = null;
		}
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent2 = gremlinsLandMineInfoComponent;
		bool flag = gremlinsLandMineInfoComponent2 != null;
		string text = string.Empty;
		text += AbilityModHelper.GetModPropertyDesc(this.m_damageMod, "[Base Damage]", flag, (!flag) ? 0 : gremlinsMultiTargeterApocolypse.m_bombDamageAmount);
		text += AbilityModHelper.GetModPropertyDesc(this.m_subsequentDamageMod, "[Subsequent Damage]", flag, (!flag) ? 0 : gremlinsMultiTargeterApocolypse.m_bombSubsequentDamageAmount);
		string str = text;
		AbilityModPropertyBool leaveLandmineOnEmptySquaresMod = this.m_leaveLandmineOnEmptySquaresMod;
		string prefix = "[Leave Mine on Empty Squares?]";
		bool showBaseVal = flag;
		bool baseVal;
		if (flag)
		{
			baseVal = gremlinsMultiTargeterApocolypse.m_leaveLandmineOnEmptySquare;
		}
		else
		{
			baseVal = false;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(leaveLandmineOnEmptySquaresMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_energyGainPerMissMod, "[EnergyGainPerMiss]", flag, (!flag) ? 0 : gremlinsMultiTargeterApocolypse.m_energyGainPerMiss);
		text += AbilityModHelper.GetModPropertyDesc(this.m_shapeMod, "[Bomb Shape]", flag, (!flag) ? AbilityAreaShape.SingleSquare : gremlinsMultiTargeterApocolypse.m_bombShape);
		string str2 = text;
		AbilityModPropertyBool penetrateLosMod = this.m_penetrateLosMod;
		string prefix2 = "[Penetrate Los?]";
		bool showBaseVal2 = flag;
		bool baseVal2;
		if (flag)
		{
			baseVal2 = gremlinsMultiTargeterApocolypse.m_penetrateLos;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(penetrateLosMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat minDistanceBetweenBombsMod = this.m_minDistanceBetweenBombsMod;
		string prefix3 = "[Min Dist Between Bombs]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = gremlinsMultiTargeterApocolypse.m_minDistanceBetweenBombs;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(minDistanceBetweenBombsMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat maxAngleWithFirstMod = this.m_maxAngleWithFirstMod;
		string prefix4 = "[Max Angle With First Segment]";
		bool showBaseVal4 = flag;
		float baseVal4;
		if (flag)
		{
			baseVal4 = gremlinsMultiTargeterApocolypse.m_maxAngleWithFirst;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(maxAngleWithFirstMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt mineDamageMod = this.m_mineDamageMod;
		string prefix5 = "[Mine Damage]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = gremlinsLandMineInfoComponent2.m_damageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + AbilityModHelper.GetModPropertyDesc(mineDamageMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt mineDurationMod = this.m_mineDurationMod;
		string prefix6 = "[Mine Duration]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = gremlinsLandMineInfoComponent2.m_mineDuration;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + AbilityModHelper.GetModPropertyDesc(mineDurationMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyEffectInfo effectOnEnemyOverride = this.m_effectOnEnemyOverride;
		string prefix7 = "{ Effect on Enemy Hit Override }";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
		if (flag)
		{
			baseVal7 = gremlinsLandMineInfoComponent2.m_enemyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + AbilityModHelper.GetModPropertyDesc(effectOnEnemyOverride, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyInt energyOnMineExplosionMod = this.m_energyOnMineExplosionMod;
		string prefix8 = "[Energy Gain on Mine Explosion (on splort and mines left behind from primary/ult)]";
		bool showBaseVal8 = flag;
		int baseVal8;
		if (flag)
		{
			baseVal8 = gremlinsLandMineInfoComponent2.m_energyGainOnExplosion;
		}
		else
		{
			baseVal8 = 0;
		}
		return str8 + AbilityModHelper.GetModPropertyDesc(energyOnMineExplosionMod, prefix8, showBaseVal8, baseVal8);
	}
}
