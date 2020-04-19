using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NekoHomingDisc : AbilityMod
{
	[Separator("Targeting", true)]
	public AbilityModPropertyFloat m_laserLengthMod;

	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	[Header("-- Disc return end radius")]
	public AbilityModPropertyFloat m_discReturnEndRadiusMod;

	[Separator("On Cast Hit", true)]
	public AbilityModPropertyEffectInfo m_onCastEnemyHitEffectMod;

	[Separator("On Enemy Hit", true)]
	public AbilityModPropertyInt m_targetDamageMod;

	public AbilityModPropertyInt m_returnTripDamageMod;

	public AbilityModPropertyBool m_returnTripIgnoreCoverMod;

	public AbilityModPropertyFloat m_extraReturnDamagePerDistMod;

	public AbilityModPropertyEffectInfo m_returnTripEnemyEffectMod;

	[Separator("Cooldown Reduction", true)]
	public AbilityModPropertyInt m_cdrIfHitNoOneOnCastMod;

	public AbilityModPropertyInt m_cdrIfHitNoOneOnReturnMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NekoHomingDisc);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NekoHomingDisc nekoHomingDisc = targetAbility as NekoHomingDisc;
		if (nekoHomingDisc != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NekoHomingDisc.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_laserLengthMod, "LaserLength", string.Empty, nekoHomingDisc.m_laserLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, nekoHomingDisc.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargets", string.Empty, nekoHomingDisc.m_maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_discReturnEndRadiusMod, "DiscReturnEndRadius", string.Empty, nekoHomingDisc.m_discReturnEndRadius, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_onCastEnemyHitEffectMod, "OnCastEnemyHitEffect", nekoHomingDisc.m_onCastEnemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_targetDamageMod, "TargetDamage", string.Empty, nekoHomingDisc.m_targetDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_returnTripDamageMod, "ReturnTripDamage", string.Empty, nekoHomingDisc.m_returnTripDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_extraReturnDamagePerDistMod, "ExtraReturnDamagePerDist", string.Empty, nekoHomingDisc.m_extraReturnDamagePerDist, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_returnTripEnemyEffectMod, "ReturnTripEnemyEffect", nekoHomingDisc.m_returnTripEnemyEffect, true);
			AbilityMod.AddToken(tokens, this.m_cdrIfHitNoOneOnCastMod, "CdrIfHitNoOneOnCast", string.Empty, nekoHomingDisc.m_cdrIfHitNoOneOnCast, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrIfHitNoOneOnReturnMod, "CdrIfHitNoOneOnReturn", string.Empty, nekoHomingDisc.m_cdrIfHitNoOneOnReturn, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NekoHomingDisc nekoHomingDisc = base.GetTargetAbilityOnAbilityData(abilityData) as NekoHomingDisc;
		bool flag = nekoHomingDisc != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat laserLengthMod = this.m_laserLengthMod;
		string prefix = "[LaserLength]";
		bool showBaseVal = flag;
		float baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NekoHomingDisc.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = nekoHomingDisc.m_laserLength;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(laserLengthMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat laserWidthMod = this.m_laserWidthMod;
		string prefix2 = "[LaserWidth]";
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
			baseVal2 = nekoHomingDisc.m_laserWidth;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(laserWidthMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt maxTargetsMod = this.m_maxTargetsMod;
		string prefix3 = "[MaxTargets]";
		bool showBaseVal3 = flag;
		int baseVal3;
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
			baseVal3 = nekoHomingDisc.m_maxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(maxTargetsMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat discReturnEndRadiusMod = this.m_discReturnEndRadiusMod;
		string prefix4 = "[DiscReturnEndRadius]";
		bool showBaseVal4 = flag;
		float baseVal4;
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
			baseVal4 = nekoHomingDisc.m_discReturnEndRadius;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(discReturnEndRadiusMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyEffectInfo onCastEnemyHitEffectMod = this.m_onCastEnemyHitEffectMod;
		string prefix5 = "[OnCastEnemyHitEffect]";
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
			baseVal5 = nekoHomingDisc.m_onCastEnemyHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(onCastEnemyHitEffectMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt targetDamageMod = this.m_targetDamageMod;
		string prefix6 = "[TargetDamage]";
		bool showBaseVal6 = flag;
		int baseVal6;
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
			baseVal6 = nekoHomingDisc.m_targetDamage;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(targetDamageMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt returnTripDamageMod = this.m_returnTripDamageMod;
		string prefix7 = "[ReturnTripDamage]";
		bool showBaseVal7 = flag;
		int baseVal7;
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
			baseVal7 = nekoHomingDisc.m_returnTripDamage;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(returnTripDamageMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyBool returnTripIgnoreCoverMod = this.m_returnTripIgnoreCoverMod;
		string prefix8 = "[ReturnTripIgnoreCover]";
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
			baseVal8 = nekoHomingDisc.m_returnTripIgnoreCover;
		}
		else
		{
			baseVal8 = false;
		}
		text = str8 + base.PropDesc(returnTripIgnoreCoverMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyFloat extraReturnDamagePerDistMod = this.m_extraReturnDamagePerDistMod;
		string prefix9 = "[ExtraReturnDamagePerDist]";
		bool showBaseVal9 = flag;
		float baseVal9;
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
			baseVal9 = nekoHomingDisc.m_extraReturnDamagePerDist;
		}
		else
		{
			baseVal9 = 0f;
		}
		text = str9 + base.PropDesc(extraReturnDamagePerDistMod, prefix9, showBaseVal9, baseVal9);
		text += base.PropDesc(this.m_returnTripEnemyEffectMod, "[ReturnTripEnemyEffect]", flag, (!flag) ? null : nekoHomingDisc.m_returnTripEnemyEffect);
		string str10 = text;
		AbilityModPropertyInt cdrIfHitNoOneOnCastMod = this.m_cdrIfHitNoOneOnCastMod;
		string prefix10 = "[CdrIfHitNoOneOnCast]";
		bool showBaseVal10 = flag;
		int baseVal10;
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
			baseVal10 = nekoHomingDisc.m_cdrIfHitNoOneOnCast;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + base.PropDesc(cdrIfHitNoOneOnCastMod, prefix10, showBaseVal10, baseVal10);
		return text + base.PropDesc(this.m_cdrIfHitNoOneOnReturnMod, "[CdrIfHitNoOneOnReturn]", flag, (!flag) ? 0 : nekoHomingDisc.m_cdrIfHitNoOneOnReturn);
	}
}
