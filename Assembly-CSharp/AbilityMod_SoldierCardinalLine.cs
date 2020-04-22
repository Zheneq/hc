using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SoldierCardinalLine : AbilityMod
{
	[Header("-- Targeting (shape for position targeter, line width for strafe hit area --")]
	public AbilityModPropertyBool m_useBothCardinalDirMod;

	public AbilityModPropertyShape m_positionShapeMod;

	public AbilityModPropertyFloat m_lineWidthMod;

	public AbilityModPropertyBool m_penetrateLosMod;

	[Header("-- On Hit Stuff --")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	[Header("-- Extra Damage for near center")]
	public AbilityModPropertyFloat m_nearCenterDistThresholdMod;

	public AbilityModPropertyInt m_extraDamageForNearCenterTargetsMod;

	[Header("-- AoE around targets --")]
	public AbilityModPropertyShape m_aoeShapeMod;

	public AbilityModPropertyInt m_aoeDamageMod;

	[Header("-- Subsequent Turn Hits --")]
	public AbilityModPropertyInt m_numSubsequentTurnsMod;

	public AbilityModPropertyInt m_damageOnSubsequentTurnsMod;

	public AbilityModPropertyEffectInfo m_enemyEffectOnSubsequentTurnsMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SoldierCardinalLine);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SoldierCardinalLine soldierCardinalLine = targetAbility as SoldierCardinalLine;
		if (!(soldierCardinalLine != null))
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
			AbilityMod.AddToken(tokens, m_lineWidthMod, "LineWidth", string.Empty, soldierCardinalLine.m_lineWidth);
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, soldierCardinalLine.m_damageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", soldierCardinalLine.m_enemyHitEffect);
			AbilityMod.AddToken(tokens, m_aoeDamageMod, "AoeDamage", string.Empty, soldierCardinalLine.m_aoeDamage);
			AbilityMod.AddToken(tokens, m_nearCenterDistThresholdMod, "NearCenterDistThreshold", string.Empty, soldierCardinalLine.m_nearCenterDistThreshold);
			AbilityMod.AddToken(tokens, m_extraDamageForNearCenterTargetsMod, "ExtraDamageForNearCenterTargets", string.Empty, soldierCardinalLine.m_extraDamageForNearCenterTargets);
			AbilityMod.AddToken(tokens, m_numSubsequentTurnsMod, "NumSubsequentTurns", string.Empty, soldierCardinalLine.m_numSubsequentTurns);
			AbilityMod.AddToken(tokens, m_damageOnSubsequentTurnsMod, "DamageOnSubsequentTurns", string.Empty, soldierCardinalLine.m_damageOnSubsequentTurns);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyEffectOnSubsequentTurnsMod, "EnemyEffectOnSubsequentTurns", soldierCardinalLine.m_enemyEffectOnSubsequentTurns);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SoldierCardinalLine soldierCardinalLine = GetTargetAbilityOnAbilityData(abilityData) as SoldierCardinalLine;
		bool flag = soldierCardinalLine != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyBool useBothCardinalDirMod = m_useBothCardinalDirMod;
		int baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = (soldierCardinalLine.m_useBothCardinalDir ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(useBothCardinalDirMod, "[UseBothCardinalDir]", flag, (byte)baseVal != 0);
		string str2 = empty;
		AbilityModPropertyShape positionShapeMod = m_positionShapeMod;
		int baseVal2;
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
			baseVal2 = (int)soldierCardinalLine.m_positionShape;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(positionShapeMod, "[PositionShape]", flag, (AbilityAreaShape)baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat lineWidthMod = m_lineWidthMod;
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
			baseVal3 = soldierCardinalLine.m_lineWidth;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(lineWidthMod, "[LineWidth]", flag, baseVal3);
		empty += PropDesc(m_penetrateLosMod, "[PenetrateLos]", flag, flag && soldierCardinalLine.m_penetrateLos);
		string str4 = empty;
		AbilityModPropertyInt damageAmountMod = m_damageAmountMod;
		int baseVal4;
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
			baseVal4 = soldierCardinalLine.m_damageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(damageAmountMod, "[DamageAmount]", flag, baseVal4);
		empty += PropDesc(m_enemyHitEffectMod, "[EnemyHitEffect]", flag, (!flag) ? null : soldierCardinalLine.m_enemyHitEffect);
		empty += PropDesc(m_nearCenterDistThresholdMod, "[NearCenterDistThreshold]", flag, (!flag) ? 0f : soldierCardinalLine.m_nearCenterDistThreshold);
		string str5 = empty;
		AbilityModPropertyInt extraDamageForNearCenterTargetsMod = m_extraDamageForNearCenterTargetsMod;
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
			baseVal5 = soldierCardinalLine.m_extraDamageForNearCenterTargets;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(extraDamageForNearCenterTargetsMod, "[ExtraDamageForNearCenterTargets]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyShape aoeShapeMod = m_aoeShapeMod;
		int baseVal6;
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
			baseVal6 = (int)soldierCardinalLine.m_aoeShape;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(aoeShapeMod, "[AoeShape]", flag, (AbilityAreaShape)baseVal6);
		string str7 = empty;
		AbilityModPropertyInt aoeDamageMod = m_aoeDamageMod;
		int baseVal7;
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
			baseVal7 = soldierCardinalLine.m_aoeDamage;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(aoeDamageMod, "[AoeDamage]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyInt numSubsequentTurnsMod = m_numSubsequentTurnsMod;
		int baseVal8;
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
			baseVal8 = soldierCardinalLine.m_numSubsequentTurns;
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(numSubsequentTurnsMod, "[NumSubsequentTurns]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyInt damageOnSubsequentTurnsMod = m_damageOnSubsequentTurnsMod;
		int baseVal9;
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
			baseVal9 = soldierCardinalLine.m_damageOnSubsequentTurns;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(damageOnSubsequentTurnsMod, "[DamageOnSubsequentTurns]", flag, baseVal9);
		return empty + PropDesc(m_enemyEffectOnSubsequentTurnsMod, "[EnemyEffectOnSubsequentTurns]", flag, (!flag) ? null : soldierCardinalLine.m_enemyEffectOnSubsequentTurns);
	}
}
