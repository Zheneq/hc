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
		if (fishManSplittingLaser != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_FishManSplittingLaser.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_primaryTargetDamageAmountMod, "PrimaryTargetDamageAmount", string.Empty, fishManSplittingLaser.m_primaryTargetDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_primaryTargetHealingAmountMod, "PrimaryTargetHealingAmount", string.Empty, fishManSplittingLaser.m_primaryTargetHealingAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_primaryTargetEnemyHitEffectMod, "PrimaryTargetEnemyHitEffect", fishManSplittingLaser.m_primaryTargetEnemyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_primaryTargetAllyHitEffectMod, "PrimaryTargetAllyHitEffect", fishManSplittingLaser.m_primaryTargetAllyHitEffect, true);
			AbilityMod.AddToken_LaserInfo(tokens, this.m_primaryTargetingInfoMod, "PrimaryTargetingInfo", fishManSplittingLaser.m_primaryTargetingInfo, true);
			AbilityMod.AddToken(tokens, this.m_secondaryTargetDamageAmountMod, "SecondaryTargetDamageAmount", string.Empty, fishManSplittingLaser.m_secondaryTargetDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_secondaryTargetHealingAmountMod, "SecondaryTargetHealingAmount", string.Empty, fishManSplittingLaser.m_secondaryTargetHealingAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_secondaryTargetEnemyHitEffectMod, "SecondaryTargetEnemyHitEffect", fishManSplittingLaser.m_secondaryTargetEnemyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_secondaryTargetAllyHitEffectMod, "SecondaryTargetAllyHitEffect", fishManSplittingLaser.m_secondaryTargetAllyHitEffect, true);
			AbilityMod.AddToken_LaserInfo(tokens, this.m_secondaryTargetingInfoMod, "SecondaryTargetingInfo", fishManSplittingLaser.m_secondaryTargetingInfo, true);
			AbilityMod.AddToken(tokens, this.m_minSplitAngleMod, "MinSplitAngle", string.Empty, fishManSplittingLaser.m_minSplitAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxSplitAngleMod, "MaxSplitAngle", string.Empty, fishManSplittingLaser.m_maxSplitAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_lengthForMinAngleMod, "LengthForMinAngle", string.Empty, fishManSplittingLaser.m_lengthForMinAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_lengthForMaxAngleMod, "LengthForMaxAngle", string.Empty, fishManSplittingLaser.m_lengthForMaxAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_numSplitBeamPairsMod, "NumSplitBeamPairs", string.Empty, fishManSplittingLaser.m_numSplitBeamPairs, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		FishManSplittingLaser fishManSplittingLaser = base.GetTargetAbilityOnAbilityData(abilityData) as FishManSplittingLaser;
		bool flag = fishManSplittingLaser != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_primaryLaserCanHitEnemiesMod, "[PrimaryLaserCanHitEnemies]", flag, flag && fishManSplittingLaser.m_primaryLaserCanHitEnemies);
		text += base.PropDesc(this.m_primaryLaserCanHitAlliesMod, "[PrimaryLaserCanHitAllies]", flag, flag && fishManSplittingLaser.m_primaryLaserCanHitAllies);
		string str = text;
		AbilityModPropertyInt primaryTargetDamageAmountMod = this.m_primaryTargetDamageAmountMod;
		string prefix = "[PrimaryTargetDamageAmount]";
		bool showBaseVal = flag;
		int baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_FishManSplittingLaser.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = fishManSplittingLaser.m_primaryTargetDamageAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(primaryTargetDamageAmountMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt primaryTargetHealingAmountMod = this.m_primaryTargetHealingAmountMod;
		string prefix2 = "[PrimaryTargetHealingAmount]";
		bool showBaseVal2 = flag;
		int baseVal2;
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
			baseVal2 = fishManSplittingLaser.m_primaryTargetHealingAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(primaryTargetHealingAmountMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyEffectInfo primaryTargetEnemyHitEffectMod = this.m_primaryTargetEnemyHitEffectMod;
		string prefix3 = "[PrimaryTargetEnemyHitEffect]";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal3;
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
			baseVal3 = fishManSplittingLaser.m_primaryTargetEnemyHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		text = str3 + base.PropDesc(primaryTargetEnemyHitEffectMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyEffectInfo primaryTargetAllyHitEffectMod = this.m_primaryTargetAllyHitEffectMod;
		string prefix4 = "[PrimaryTargetAllyHitEffect]";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal4;
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
			baseVal4 = fishManSplittingLaser.m_primaryTargetAllyHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		text = str4 + base.PropDesc(primaryTargetAllyHitEffectMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyLaserInfo primaryTargetingInfoMod = this.m_primaryTargetingInfoMod;
		string prefix5 = "[PrimaryTargetingInfo]";
		bool showBaseVal5 = flag;
		LaserTargetingInfo baseLaserInfo;
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
			baseLaserInfo = fishManSplittingLaser.m_primaryTargetingInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		text = str5 + base.PropDesc(primaryTargetingInfoMod, prefix5, showBaseVal5, baseLaserInfo);
		string str6 = text;
		AbilityModPropertyBool secondaryLasersCanHitEnemiesMod = this.m_secondaryLasersCanHitEnemiesMod;
		string prefix6 = "[SecondaryLasersCanHitEnemies]";
		bool showBaseVal6 = flag;
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
			baseVal5 = fishManSplittingLaser.m_secondaryLasersCanHitEnemies;
		}
		else
		{
			baseVal5 = false;
		}
		text = str6 + base.PropDesc(secondaryLasersCanHitEnemiesMod, prefix6, showBaseVal6, baseVal5);
		string str7 = text;
		AbilityModPropertyBool secondaryLasersCanHitAlliesMod = this.m_secondaryLasersCanHitAlliesMod;
		string prefix7 = "[SecondaryLasersCanHitAllies]";
		bool showBaseVal7 = flag;
		bool baseVal6;
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
			baseVal6 = fishManSplittingLaser.m_secondaryLasersCanHitAllies;
		}
		else
		{
			baseVal6 = false;
		}
		text = str7 + base.PropDesc(secondaryLasersCanHitAlliesMod, prefix7, showBaseVal7, baseVal6);
		string str8 = text;
		AbilityModPropertyInt secondaryTargetDamageAmountMod = this.m_secondaryTargetDamageAmountMod;
		string prefix8 = "[SecondaryTargetDamageAmount]";
		bool showBaseVal8 = flag;
		int baseVal7;
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
			baseVal7 = fishManSplittingLaser.m_secondaryTargetDamageAmount;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str8 + base.PropDesc(secondaryTargetDamageAmountMod, prefix8, showBaseVal8, baseVal7);
		string str9 = text;
		AbilityModPropertyInt secondaryTargetHealingAmountMod = this.m_secondaryTargetHealingAmountMod;
		string prefix9 = "[SecondaryTargetHealingAmount]";
		bool showBaseVal9 = flag;
		int baseVal8;
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
			baseVal8 = fishManSplittingLaser.m_secondaryTargetHealingAmount;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str9 + base.PropDesc(secondaryTargetHealingAmountMod, prefix9, showBaseVal9, baseVal8);
		string str10 = text;
		AbilityModPropertyEffectInfo secondaryTargetEnemyHitEffectMod = this.m_secondaryTargetEnemyHitEffectMod;
		string prefix10 = "[SecondaryTargetEnemyHitEffect]";
		bool showBaseVal10 = flag;
		StandardEffectInfo baseVal9;
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
			baseVal9 = fishManSplittingLaser.m_secondaryTargetEnemyHitEffect;
		}
		else
		{
			baseVal9 = null;
		}
		text = str10 + base.PropDesc(secondaryTargetEnemyHitEffectMod, prefix10, showBaseVal10, baseVal9);
		string str11 = text;
		AbilityModPropertyEffectInfo secondaryTargetAllyHitEffectMod = this.m_secondaryTargetAllyHitEffectMod;
		string prefix11 = "[SecondaryTargetAllyHitEffect]";
		bool showBaseVal11 = flag;
		StandardEffectInfo baseVal10;
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
			baseVal10 = fishManSplittingLaser.m_secondaryTargetAllyHitEffect;
		}
		else
		{
			baseVal10 = null;
		}
		text = str11 + base.PropDesc(secondaryTargetAllyHitEffectMod, prefix11, showBaseVal11, baseVal10);
		string str12 = text;
		AbilityModPropertyLaserInfo secondaryTargetingInfoMod = this.m_secondaryTargetingInfoMod;
		string prefix12 = "[SecondaryTargetingInfo]";
		bool showBaseVal12 = flag;
		LaserTargetingInfo baseLaserInfo2;
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
			baseLaserInfo2 = fishManSplittingLaser.m_secondaryTargetingInfo;
		}
		else
		{
			baseLaserInfo2 = null;
		}
		text = str12 + base.PropDesc(secondaryTargetingInfoMod, prefix12, showBaseVal12, baseLaserInfo2);
		string str13 = text;
		AbilityModPropertyBool alwaysSplitMod = this.m_alwaysSplitMod;
		string prefix13 = "[AlwaysSplit]";
		bool showBaseVal13 = flag;
		bool baseVal11;
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
			baseVal11 = fishManSplittingLaser.m_alwaysSplit;
		}
		else
		{
			baseVal11 = false;
		}
		text = str13 + base.PropDesc(alwaysSplitMod, prefix13, showBaseVal13, baseVal11);
		string str14 = text;
		AbilityModPropertyFloat minSplitAngleMod = this.m_minSplitAngleMod;
		string prefix14 = "[MinSplitAngle]";
		bool showBaseVal14 = flag;
		float baseVal12;
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
			baseVal12 = fishManSplittingLaser.m_minSplitAngle;
		}
		else
		{
			baseVal12 = 0f;
		}
		text = str14 + base.PropDesc(minSplitAngleMod, prefix14, showBaseVal14, baseVal12);
		string str15 = text;
		AbilityModPropertyFloat maxSplitAngleMod = this.m_maxSplitAngleMod;
		string prefix15 = "[MaxSplitAngle]";
		bool showBaseVal15 = flag;
		float baseVal13;
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
			baseVal13 = fishManSplittingLaser.m_maxSplitAngle;
		}
		else
		{
			baseVal13 = 0f;
		}
		text = str15 + base.PropDesc(maxSplitAngleMod, prefix15, showBaseVal15, baseVal13);
		string str16 = text;
		AbilityModPropertyFloat lengthForMinAngleMod = this.m_lengthForMinAngleMod;
		string prefix16 = "[LengthForMinAngle]";
		bool showBaseVal16 = flag;
		float baseVal14;
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
			baseVal14 = fishManSplittingLaser.m_lengthForMinAngle;
		}
		else
		{
			baseVal14 = 0f;
		}
		text = str16 + base.PropDesc(lengthForMinAngleMod, prefix16, showBaseVal16, baseVal14);
		string str17 = text;
		AbilityModPropertyFloat lengthForMaxAngleMod = this.m_lengthForMaxAngleMod;
		string prefix17 = "[LengthForMaxAngle]";
		bool showBaseVal17 = flag;
		float baseVal15;
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
			baseVal15 = fishManSplittingLaser.m_lengthForMaxAngle;
		}
		else
		{
			baseVal15 = 0f;
		}
		text = str17 + base.PropDesc(lengthForMaxAngleMod, prefix17, showBaseVal17, baseVal15);
		string str18 = text;
		AbilityModPropertyInt numSplitBeamPairsMod = this.m_numSplitBeamPairsMod;
		string prefix18 = "[NumSplitBeamPairs]";
		bool showBaseVal18 = flag;
		int baseVal16;
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
			baseVal16 = fishManSplittingLaser.m_numSplitBeamPairs;
		}
		else
		{
			baseVal16 = 0;
		}
		return str18 + base.PropDesc(numSplitBeamPairsMod, prefix18, showBaseVal18, baseVal16);
	}
}
