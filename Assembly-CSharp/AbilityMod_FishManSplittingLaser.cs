using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_FishManSplittingLaser : AbilityMod
{
	[Header("-- Primary Laser")]
	public AbilityModPropertyBool m_primaryLaserCanHitEnemiesMod;

	public AbilityModPropertyBool m_primaryLaserCanHitAlliesMod;

	public AbilityModPropertyInt m_primaryTargetDamageAmountMod;

	public AbilityModPropertyInt m_primaryTargetHealingAmountMod;

	public AbilityModPropertyEffectInfo m_primaryTargetEnemyHitEffectMod;

	public AbilityModPropertyEffectInfo m_primaryTargetAllyHitEffectMod;

	public AbilityModPropertyLaserInfo m_primaryTargetingInfoMod;

	[Header("-- Secondary Lasers")]
	public AbilityModPropertyBool m_secondaryLasersCanHitEnemiesMod;

	public AbilityModPropertyBool m_secondaryLasersCanHitAlliesMod;

	public AbilityModPropertyInt m_secondaryTargetDamageAmountMod;

	public AbilityModPropertyInt m_secondaryTargetHealingAmountMod;

	public AbilityModPropertyEffectInfo m_secondaryTargetEnemyHitEffectMod;

	public AbilityModPropertyEffectInfo m_secondaryTargetAllyHitEffectMod;

	public AbilityModPropertyLaserInfo m_secondaryTargetingInfoMod;

	[Header("-- Split Data")]
	public AbilityModPropertyBool m_alwaysSplitMod;

	public AbilityModPropertyFloat m_minSplitAngleMod;

	public AbilityModPropertyFloat m_maxSplitAngleMod;

	public AbilityModPropertyFloat m_lengthForMinAngleMod;

	public AbilityModPropertyFloat m_lengthForMaxAngleMod;

	public AbilityModPropertyInt m_numSplitBeamPairsMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(FishManSplittingLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		FishManSplittingLaser fishManSplittingLaser = targetAbility as FishManSplittingLaser;
		if (!(fishManSplittingLaser != null))
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
			AbilityMod.AddToken(tokens, m_primaryTargetDamageAmountMod, "PrimaryTargetDamageAmount", string.Empty, fishManSplittingLaser.m_primaryTargetDamageAmount);
			AbilityMod.AddToken(tokens, m_primaryTargetHealingAmountMod, "PrimaryTargetHealingAmount", string.Empty, fishManSplittingLaser.m_primaryTargetHealingAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_primaryTargetEnemyHitEffectMod, "PrimaryTargetEnemyHitEffect", fishManSplittingLaser.m_primaryTargetEnemyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_primaryTargetAllyHitEffectMod, "PrimaryTargetAllyHitEffect", fishManSplittingLaser.m_primaryTargetAllyHitEffect);
			AbilityMod.AddToken_LaserInfo(tokens, m_primaryTargetingInfoMod, "PrimaryTargetingInfo", fishManSplittingLaser.m_primaryTargetingInfo);
			AbilityMod.AddToken(tokens, m_secondaryTargetDamageAmountMod, "SecondaryTargetDamageAmount", string.Empty, fishManSplittingLaser.m_secondaryTargetDamageAmount);
			AbilityMod.AddToken(tokens, m_secondaryTargetHealingAmountMod, "SecondaryTargetHealingAmount", string.Empty, fishManSplittingLaser.m_secondaryTargetHealingAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_secondaryTargetEnemyHitEffectMod, "SecondaryTargetEnemyHitEffect", fishManSplittingLaser.m_secondaryTargetEnemyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_secondaryTargetAllyHitEffectMod, "SecondaryTargetAllyHitEffect", fishManSplittingLaser.m_secondaryTargetAllyHitEffect);
			AbilityMod.AddToken_LaserInfo(tokens, m_secondaryTargetingInfoMod, "SecondaryTargetingInfo", fishManSplittingLaser.m_secondaryTargetingInfo);
			AbilityMod.AddToken(tokens, m_minSplitAngleMod, "MinSplitAngle", string.Empty, fishManSplittingLaser.m_minSplitAngle);
			AbilityMod.AddToken(tokens, m_maxSplitAngleMod, "MaxSplitAngle", string.Empty, fishManSplittingLaser.m_maxSplitAngle);
			AbilityMod.AddToken(tokens, m_lengthForMinAngleMod, "LengthForMinAngle", string.Empty, fishManSplittingLaser.m_lengthForMinAngle);
			AbilityMod.AddToken(tokens, m_lengthForMaxAngleMod, "LengthForMaxAngle", string.Empty, fishManSplittingLaser.m_lengthForMaxAngle);
			AbilityMod.AddToken(tokens, m_numSplitBeamPairsMod, "NumSplitBeamPairs", string.Empty, fishManSplittingLaser.m_numSplitBeamPairs);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManSplittingLaser fishManSplittingLaser = GetTargetAbilityOnAbilityData(abilityData) as FishManSplittingLaser;
		bool flag = fishManSplittingLaser != null;
		string empty = string.Empty;
		empty += PropDesc(m_primaryLaserCanHitEnemiesMod, "[PrimaryLaserCanHitEnemies]", flag, flag && fishManSplittingLaser.m_primaryLaserCanHitEnemies);
		empty += PropDesc(m_primaryLaserCanHitAlliesMod, "[PrimaryLaserCanHitAllies]", flag, flag && fishManSplittingLaser.m_primaryLaserCanHitAllies);
		string str = empty;
		AbilityModPropertyInt primaryTargetDamageAmountMod = m_primaryTargetDamageAmountMod;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = fishManSplittingLaser.m_primaryTargetDamageAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(primaryTargetDamageAmountMod, "[PrimaryTargetDamageAmount]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt primaryTargetHealingAmountMod = m_primaryTargetHealingAmountMod;
		int baseVal2;
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
			baseVal2 = fishManSplittingLaser.m_primaryTargetHealingAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(primaryTargetHealingAmountMod, "[PrimaryTargetHealingAmount]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyEffectInfo primaryTargetEnemyHitEffectMod = m_primaryTargetEnemyHitEffectMod;
		object baseVal3;
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
			baseVal3 = fishManSplittingLaser.m_primaryTargetEnemyHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		empty = str3 + PropDesc(primaryTargetEnemyHitEffectMod, "[PrimaryTargetEnemyHitEffect]", flag, (StandardEffectInfo)baseVal3);
		string str4 = empty;
		AbilityModPropertyEffectInfo primaryTargetAllyHitEffectMod = m_primaryTargetAllyHitEffectMod;
		object baseVal4;
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
			baseVal4 = fishManSplittingLaser.m_primaryTargetAllyHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str4 + PropDesc(primaryTargetAllyHitEffectMod, "[PrimaryTargetAllyHitEffect]", flag, (StandardEffectInfo)baseVal4);
		string str5 = empty;
		AbilityModPropertyLaserInfo primaryTargetingInfoMod = m_primaryTargetingInfoMod;
		object baseLaserInfo;
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
			baseLaserInfo = fishManSplittingLaser.m_primaryTargetingInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		empty = str5 + PropDesc(primaryTargetingInfoMod, "[PrimaryTargetingInfo]", flag, (LaserTargetingInfo)baseLaserInfo);
		string str6 = empty;
		AbilityModPropertyBool secondaryLasersCanHitEnemiesMod = m_secondaryLasersCanHitEnemiesMod;
		int baseVal5;
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
			baseVal5 = (fishManSplittingLaser.m_secondaryLasersCanHitEnemies ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str6 + PropDesc(secondaryLasersCanHitEnemiesMod, "[SecondaryLasersCanHitEnemies]", flag, (byte)baseVal5 != 0);
		string str7 = empty;
		AbilityModPropertyBool secondaryLasersCanHitAlliesMod = m_secondaryLasersCanHitAlliesMod;
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
			baseVal6 = (fishManSplittingLaser.m_secondaryLasersCanHitAllies ? 1 : 0);
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str7 + PropDesc(secondaryLasersCanHitAlliesMod, "[SecondaryLasersCanHitAllies]", flag, (byte)baseVal6 != 0);
		string str8 = empty;
		AbilityModPropertyInt secondaryTargetDamageAmountMod = m_secondaryTargetDamageAmountMod;
		int baseVal7;
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
			baseVal7 = fishManSplittingLaser.m_secondaryTargetDamageAmount;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str8 + PropDesc(secondaryTargetDamageAmountMod, "[SecondaryTargetDamageAmount]", flag, baseVal7);
		string str9 = empty;
		AbilityModPropertyInt secondaryTargetHealingAmountMod = m_secondaryTargetHealingAmountMod;
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
			baseVal8 = fishManSplittingLaser.m_secondaryTargetHealingAmount;
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str9 + PropDesc(secondaryTargetHealingAmountMod, "[SecondaryTargetHealingAmount]", flag, baseVal8);
		string str10 = empty;
		AbilityModPropertyEffectInfo secondaryTargetEnemyHitEffectMod = m_secondaryTargetEnemyHitEffectMod;
		object baseVal9;
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
			baseVal9 = fishManSplittingLaser.m_secondaryTargetEnemyHitEffect;
		}
		else
		{
			baseVal9 = null;
		}
		empty = str10 + PropDesc(secondaryTargetEnemyHitEffectMod, "[SecondaryTargetEnemyHitEffect]", flag, (StandardEffectInfo)baseVal9);
		string str11 = empty;
		AbilityModPropertyEffectInfo secondaryTargetAllyHitEffectMod = m_secondaryTargetAllyHitEffectMod;
		object baseVal10;
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
			baseVal10 = fishManSplittingLaser.m_secondaryTargetAllyHitEffect;
		}
		else
		{
			baseVal10 = null;
		}
		empty = str11 + PropDesc(secondaryTargetAllyHitEffectMod, "[SecondaryTargetAllyHitEffect]", flag, (StandardEffectInfo)baseVal10);
		string str12 = empty;
		AbilityModPropertyLaserInfo secondaryTargetingInfoMod = m_secondaryTargetingInfoMod;
		object baseLaserInfo2;
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
			baseLaserInfo2 = fishManSplittingLaser.m_secondaryTargetingInfo;
		}
		else
		{
			baseLaserInfo2 = null;
		}
		empty = str12 + PropDesc(secondaryTargetingInfoMod, "[SecondaryTargetingInfo]", flag, (LaserTargetingInfo)baseLaserInfo2);
		string str13 = empty;
		AbilityModPropertyBool alwaysSplitMod = m_alwaysSplitMod;
		int baseVal11;
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
			baseVal11 = (fishManSplittingLaser.m_alwaysSplit ? 1 : 0);
		}
		else
		{
			baseVal11 = 0;
		}
		empty = str13 + PropDesc(alwaysSplitMod, "[AlwaysSplit]", flag, (byte)baseVal11 != 0);
		string str14 = empty;
		AbilityModPropertyFloat minSplitAngleMod = m_minSplitAngleMod;
		float baseVal12;
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
			baseVal12 = fishManSplittingLaser.m_minSplitAngle;
		}
		else
		{
			baseVal12 = 0f;
		}
		empty = str14 + PropDesc(minSplitAngleMod, "[MinSplitAngle]", flag, baseVal12);
		string str15 = empty;
		AbilityModPropertyFloat maxSplitAngleMod = m_maxSplitAngleMod;
		float baseVal13;
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
			baseVal13 = fishManSplittingLaser.m_maxSplitAngle;
		}
		else
		{
			baseVal13 = 0f;
		}
		empty = str15 + PropDesc(maxSplitAngleMod, "[MaxSplitAngle]", flag, baseVal13);
		string str16 = empty;
		AbilityModPropertyFloat lengthForMinAngleMod = m_lengthForMinAngleMod;
		float baseVal14;
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
			baseVal14 = fishManSplittingLaser.m_lengthForMinAngle;
		}
		else
		{
			baseVal14 = 0f;
		}
		empty = str16 + PropDesc(lengthForMinAngleMod, "[LengthForMinAngle]", flag, baseVal14);
		string str17 = empty;
		AbilityModPropertyFloat lengthForMaxAngleMod = m_lengthForMaxAngleMod;
		float baseVal15;
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
			baseVal15 = fishManSplittingLaser.m_lengthForMaxAngle;
		}
		else
		{
			baseVal15 = 0f;
		}
		empty = str17 + PropDesc(lengthForMaxAngleMod, "[LengthForMaxAngle]", flag, baseVal15);
		string str18 = empty;
		AbilityModPropertyInt numSplitBeamPairsMod = m_numSplitBeamPairsMod;
		int baseVal16;
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
			baseVal16 = fishManSplittingLaser.m_numSplitBeamPairs;
		}
		else
		{
			baseVal16 = 0;
		}
		return str18 + PropDesc(numSplitBeamPairsMod, "[NumSplitBeamPairs]", flag, baseVal16);
	}
}
