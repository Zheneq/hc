using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ValkyriePullToLaserCenter : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_laserRangeInSquaresMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	public AbilityModPropertyBool m_lengthIgnoreLosMod;

	[Header("-- Damage & effects")]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyInt m_extraDamageIfKnockedInPlaceMod;

	public AbilityModPropertyEffectInfo m_effectToEnemiesMod;

	[Header("-- Extra Damage for Center")]
	public AbilityModPropertyInt m_extraDamageForCenterHitsMod;

	public AbilityModPropertyFloat m_centerHitWidthMod;

	[Header("-- Knockback on Cast")]
	public AbilityModPropertyFloat m_maxKnockbackDistMod;

	public AbilityModPropertyKnockbackType m_knockbackTypeMod;

	[Header("-- Misc ability interactions")]
	public AbilityModPropertyBool m_nextTurnStabSkipsDamageReduction;

	public override Type GetTargetAbilityType()
	{
		return typeof(ValkyriePullToLaserCenter);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ValkyriePullToLaserCenter valkyriePullToLaserCenter = targetAbility as ValkyriePullToLaserCenter;
		if (!(valkyriePullToLaserCenter != null))
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
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, valkyriePullToLaserCenter.m_laserWidth);
			AbilityMod.AddToken(tokens, m_laserRangeInSquaresMod, "LaserRangeInSquares", string.Empty, valkyriePullToLaserCenter.m_laserRangeInSquares);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, valkyriePullToLaserCenter.m_maxTargets);
			AbilityMod.AddToken(tokens, m_damageMod, "Damage", string.Empty, valkyriePullToLaserCenter.m_damage);
			AbilityMod.AddToken(tokens, m_extraDamageIfKnockedInPlaceMod, "ExtraDamageIfKnockedInPlace", string.Empty, 0);
			AbilityMod.AddToken_EffectMod(tokens, m_effectToEnemiesMod, "EffectToEnemies", valkyriePullToLaserCenter.m_effectToEnemies);
			AbilityMod.AddToken(tokens, m_extraDamageForCenterHitsMod, "ExtraDamageForCenterHits", string.Empty, valkyriePullToLaserCenter.m_extraDamageForCenterHits);
			AbilityMod.AddToken(tokens, m_centerHitWidthMod, "CenterHitWidth", string.Empty, valkyriePullToLaserCenter.m_centerHitWidth);
			AbilityMod.AddToken(tokens, m_maxKnockbackDistMod, "MaxKnockbackDist", string.Empty, valkyriePullToLaserCenter.m_maxKnockbackDist);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ValkyriePullToLaserCenter valkyriePullToLaserCenter = GetTargetAbilityOnAbilityData(abilityData) as ValkyriePullToLaserCenter;
		bool flag = valkyriePullToLaserCenter != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat laserWidthMod = m_laserWidthMod;
		float baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = valkyriePullToLaserCenter.m_laserWidth;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(laserWidthMod, "[LaserWidth]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat laserRangeInSquaresMod = m_laserRangeInSquaresMod;
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
			baseVal2 = valkyriePullToLaserCenter.m_laserRangeInSquares;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(laserRangeInSquaresMod, "[LaserRangeInSquares]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt maxTargetsMod = m_maxTargetsMod;
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
			baseVal3 = valkyriePullToLaserCenter.m_maxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(maxTargetsMod, "[MaxTargets]", flag, baseVal3);
		empty += PropDesc(m_lengthIgnoreLosMod, "[LengthIgnoreLos]", flag, flag && valkyriePullToLaserCenter.m_lengthIgnoreLos);
		string str4 = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal4;
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
			baseVal4 = valkyriePullToLaserCenter.m_damage;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(damageMod, "[Damage]", flag, baseVal4);
		empty += PropDesc(m_extraDamageIfKnockedInPlaceMod, "[ExtraDamageIfKnockedInPlace]", flag);
		string str5 = empty;
		AbilityModPropertyEffectInfo effectToEnemiesMod = m_effectToEnemiesMod;
		object baseVal5;
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
			baseVal5 = valkyriePullToLaserCenter.m_effectToEnemies;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + PropDesc(effectToEnemiesMod, "[EffectToEnemies]", flag, (StandardEffectInfo)baseVal5);
		empty += PropDesc(m_extraDamageForCenterHitsMod, "[ExtraDamageForCenterHits]", flag, flag ? valkyriePullToLaserCenter.m_extraDamageForCenterHits : 0);
		string str6 = empty;
		AbilityModPropertyFloat centerHitWidthMod = m_centerHitWidthMod;
		float baseVal6;
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
			baseVal6 = valkyriePullToLaserCenter.m_centerHitWidth;
		}
		else
		{
			baseVal6 = 0f;
		}
		empty = str6 + PropDesc(centerHitWidthMod, "[CenterHitWidth]", flag, baseVal6);
		empty += PropDesc(m_maxKnockbackDistMod, "[MaxKnockbackDist]", flag, (!flag) ? 0f : valkyriePullToLaserCenter.m_maxKnockbackDist);
		string str7 = empty;
		AbilityModPropertyKnockbackType knockbackTypeMod = m_knockbackTypeMod;
		int baseVal7;
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
			baseVal7 = (int)valkyriePullToLaserCenter.m_knockbackType;
		}
		else
		{
			baseVal7 = 4;
		}
		empty = str7 + PropDesc(knockbackTypeMod, "[KnockbackType]", flag, (KnockbackType)baseVal7);
		return empty + PropDesc(m_nextTurnStabSkipsDamageReduction, "[NextTurnStabSkipsDamageReduction]", flag);
	}
}
