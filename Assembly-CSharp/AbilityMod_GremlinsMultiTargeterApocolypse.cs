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
		if (!(gremlinsMultiTargeterApocolypse != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_energyGainPerMissMod, "EnergyGainPerMiss", string.Empty, gremlinsMultiTargeterApocolypse.m_energyGainPerMiss);
			AbilityMod.AddToken(tokens, m_minDistanceBetweenBombsMod, "MinDistanceBetweenBombs", string.Empty, gremlinsMultiTargeterApocolypse.m_minDistanceBetweenBombs);
			AbilityMod.AddToken(tokens, m_maxAngleWithFirstMod, "MaxAngleWithFirst", string.Empty, gremlinsMultiTargeterApocolypse.m_maxAngleWithFirst);
			AbilityMod.AddToken(tokens, m_damageMod, "BombDamageAmount", string.Empty, gremlinsMultiTargeterApocolypse.m_bombDamageAmount);
			AbilityMod.AddToken(tokens, m_subsequentDamageMod, "BombSubsequentDamageAmount", string.Empty, gremlinsMultiTargeterApocolypse.m_bombSubsequentDamageAmount);
			if (!m_useTargetDataOverrides)
			{
				return;
			}
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				if (m_targetDataOverrides == null)
				{
					return;
				}
				int val = m_targetDataOverrides.Length;
				int otherVal = gremlinsMultiTargeterApocolypse.m_targetData.Length;
				AbilityMod.AddToken_IntDiff(tokens, "NumBombs", string.Empty, val, true, otherVal);
				if (m_targetDataOverrides.Length <= 0)
				{
					return;
				}
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					if (gremlinsMultiTargeterApocolypse.m_targetData.Length > 0)
					{
						while (true)
						{
							switch (4)
							{
							case 0:
								continue;
							}
							AbilityMod.AddToken_IntDiff(tokens, "TargeterRange_Diff", string.Empty, Mathf.RoundToInt(m_targetDataOverrides[0].m_range - gremlinsMultiTargeterApocolypse.m_targetData[0].m_range), false, 0);
							return;
						}
					}
					return;
				}
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		GremlinsMultiTargeterApocolypse gremlinsMultiTargeterApocolypse = GetTargetAbilityOnAbilityData(abilityData) as GremlinsMultiTargeterApocolypse;
		object obj;
		if (gremlinsMultiTargeterApocolypse != null)
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
			obj = gremlinsMultiTargeterApocolypse.GetComponent<GremlinsLandMineInfoComponent>();
		}
		else
		{
			obj = null;
		}
		GremlinsLandMineInfoComponent gremlinsLandMineInfoComponent = (GremlinsLandMineInfoComponent)obj;
		bool flag = gremlinsLandMineInfoComponent != null;
		string empty = string.Empty;
		empty += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Base Damage]", flag, flag ? gremlinsMultiTargeterApocolypse.m_bombDamageAmount : 0);
		empty += AbilityModHelper.GetModPropertyDesc(m_subsequentDamageMod, "[Subsequent Damage]", flag, flag ? gremlinsMultiTargeterApocolypse.m_bombSubsequentDamageAmount : 0);
		string str = empty;
		AbilityModPropertyBool leaveLandmineOnEmptySquaresMod = m_leaveLandmineOnEmptySquaresMod;
		int baseVal;
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal = (gremlinsMultiTargeterApocolypse.m_leaveLandmineOnEmptySquare ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(leaveLandmineOnEmptySquaresMod, "[Leave Mine on Empty Squares?]", flag, (byte)baseVal != 0);
		empty += PropDesc(m_energyGainPerMissMod, "[EnergyGainPerMiss]", flag, flag ? gremlinsMultiTargeterApocolypse.m_energyGainPerMiss : 0);
		empty += AbilityModHelper.GetModPropertyDesc(m_shapeMod, "[Bomb Shape]", flag, flag ? gremlinsMultiTargeterApocolypse.m_bombShape : AbilityAreaShape.SingleSquare);
		string str2 = empty;
		AbilityModPropertyBool penetrateLosMod = m_penetrateLosMod;
		int baseVal2;
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
			baseVal2 = (gremlinsMultiTargeterApocolypse.m_penetrateLos ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(penetrateLosMod, "[Penetrate Los?]", flag, (byte)baseVal2 != 0);
		string str3 = empty;
		AbilityModPropertyFloat minDistanceBetweenBombsMod = m_minDistanceBetweenBombsMod;
		float baseVal3;
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
			baseVal3 = gremlinsMultiTargeterApocolypse.m_minDistanceBetweenBombs;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(minDistanceBetweenBombsMod, "[Min Dist Between Bombs]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat maxAngleWithFirstMod = m_maxAngleWithFirstMod;
		float baseVal4;
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal4 = gremlinsMultiTargeterApocolypse.m_maxAngleWithFirst;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(maxAngleWithFirstMod, "[Max Angle With First Segment]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyInt mineDamageMod = m_mineDamageMod;
		int baseVal5;
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
			baseVal5 = gremlinsLandMineInfoComponent.m_damageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + AbilityModHelper.GetModPropertyDesc(mineDamageMod, "[Mine Damage]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyInt mineDurationMod = m_mineDurationMod;
		int baseVal6;
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
			baseVal6 = gremlinsLandMineInfoComponent.m_mineDuration;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + AbilityModHelper.GetModPropertyDesc(mineDurationMod, "[Mine Duration]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyEffectInfo effectOnEnemyOverride = m_effectOnEnemyOverride;
		object baseVal7;
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
			baseVal7 = gremlinsLandMineInfoComponent.m_enemyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + AbilityModHelper.GetModPropertyDesc(effectOnEnemyOverride, "{ Effect on Enemy Hit Override }", flag, (StandardEffectInfo)baseVal7);
		string str8 = empty;
		AbilityModPropertyInt energyOnMineExplosionMod = m_energyOnMineExplosionMod;
		int baseVal8;
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
			baseVal8 = gremlinsLandMineInfoComponent.m_energyGainOnExplosion;
		}
		else
		{
			baseVal8 = 0;
		}
		return str8 + AbilityModHelper.GetModPropertyDesc(energyOnMineExplosionMod, "[Energy Gain on Mine Explosion (on splort and mines left behind from primary/ult)]", flag, baseVal8);
	}
}
