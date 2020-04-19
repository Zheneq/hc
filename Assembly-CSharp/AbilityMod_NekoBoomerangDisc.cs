using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NekoBoomerangDisc : AbilityMod
{
	[Separator("Targeting", true)]
	public AbilityModPropertyFloat m_laserLengthMod;

	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_aoeRadiusAtEndMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	[Header("-- Disc return end radius")]
	public AbilityModPropertyFloat m_discReturnEndRadiusMod;

	[Separator("Damage stuff", true)]
	public AbilityModPropertyInt m_directDamageMod;

	public AbilityModPropertyInt m_returnTripDamageMod;

	public AbilityModPropertyBool m_returnTripIgnoreCoverMod;

	[Header("-- Extra Damage")]
	public AbilityModPropertyInt m_extraDamageIfHitByReturnDiscMod;

	public AbilityModPropertyInt m_extraReturnDamageIfHitNoOneMod;

	[Separator("-- Shielding for target hit on throw (applied on start of next turn)", true)]
	public AbilityModPropertyInt m_shieldPerTargetHitOnThrowMod;

	public AbilityModPropertyEffectData m_shieldEffectDataMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NekoBoomerangDisc);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NekoBoomerangDisc nekoBoomerangDisc = targetAbility as NekoBoomerangDisc;
		if (nekoBoomerangDisc != null)
		{
			AbilityMod.AddToken(tokens, this.m_laserLengthMod, "LaserLength", string.Empty, nekoBoomerangDisc.m_laserLength, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, nekoBoomerangDisc.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_aoeRadiusAtEndMod, "AoeRadiusAtEnd", string.Empty, nekoBoomerangDisc.m_aoeRadiusAtEnd, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargets", string.Empty, nekoBoomerangDisc.m_maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_discReturnEndRadiusMod, "DiscReturnEndRadius", string.Empty, nekoBoomerangDisc.m_discReturnEndRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_directDamageMod, "DirectDamage", string.Empty, nekoBoomerangDisc.m_directDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_returnTripDamageMod, "ReturnTripDamage", string.Empty, nekoBoomerangDisc.m_returnTripDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageIfHitByReturnDiscMod, "ExtraDamageIfHitByReturnDisc", string.Empty, nekoBoomerangDisc.m_extraDamageIfHitByReturnDisc, true, false);
			AbilityMod.AddToken(tokens, this.m_extraReturnDamageIfHitNoOneMod, "ExtraReturnDamageIfHitNoOne", string.Empty, nekoBoomerangDisc.m_extraReturnDamageIfHitNoOne, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldPerTargetHitOnThrowMod, "ShieldPerTargetHitOnThrow", string.Empty, nekoBoomerangDisc.m_shieldPerTargetHitOnThrow, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_shieldEffectDataMod, "ShieldEffectData", nekoBoomerangDisc.m_shieldEffectData, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NekoBoomerangDisc nekoBoomerangDisc = base.GetTargetAbilityOnAbilityData(abilityData) as NekoBoomerangDisc;
		bool flag = nekoBoomerangDisc != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_laserLengthMod, "[LaserLength]", flag, (!flag) ? 0f : nekoBoomerangDisc.m_laserLength);
		string str = text;
		AbilityModPropertyFloat laserWidthMod = this.m_laserWidthMod;
		string prefix = "[LaserWidth]";
		bool showBaseVal = flag;
		float baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NekoBoomerangDisc.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = nekoBoomerangDisc.m_laserWidth;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(laserWidthMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat aoeRadiusAtEndMod = this.m_aoeRadiusAtEndMod;
		string prefix2 = "[AoeRadiusAtEnd]";
		bool showBaseVal2 = flag;
		float baseVal2;
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
			baseVal2 = nekoBoomerangDisc.m_aoeRadiusAtEnd;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(aoeRadiusAtEndMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt maxTargetsMod = this.m_maxTargetsMod;
		string prefix3 = "[MaxTargets]";
		bool showBaseVal3 = flag;
		int baseVal3;
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
			baseVal3 = nekoBoomerangDisc.m_maxTargets;
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
			baseVal4 = nekoBoomerangDisc.m_discReturnEndRadius;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(discReturnEndRadiusMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt directDamageMod = this.m_directDamageMod;
		string prefix5 = "[DirectDamage]";
		bool showBaseVal5 = flag;
		int baseVal5;
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
			baseVal5 = nekoBoomerangDisc.m_directDamage;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(directDamageMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_returnTripDamageMod, "[ReturnTripDamage]", flag, (!flag) ? 0 : nekoBoomerangDisc.m_returnTripDamage);
		string str6 = text;
		AbilityModPropertyBool returnTripIgnoreCoverMod = this.m_returnTripIgnoreCoverMod;
		string prefix6 = "[ReturnTripIgnoreCover]";
		bool showBaseVal6 = flag;
		bool baseVal6;
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
			baseVal6 = nekoBoomerangDisc.m_returnTripIgnoreCover;
		}
		else
		{
			baseVal6 = false;
		}
		text = str6 + base.PropDesc(returnTripIgnoreCoverMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt extraDamageIfHitByReturnDiscMod = this.m_extraDamageIfHitByReturnDiscMod;
		string prefix7 = "[ExtraDamageIfHitByReturnDisc]";
		bool showBaseVal7 = flag;
		int baseVal7;
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
			baseVal7 = nekoBoomerangDisc.m_extraDamageIfHitByReturnDisc;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(extraDamageIfHitByReturnDiscMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyInt extraReturnDamageIfHitNoOneMod = this.m_extraReturnDamageIfHitNoOneMod;
		string prefix8 = "[ExtraReturnDamageIfHitNoOne]";
		bool showBaseVal8 = flag;
		int baseVal8;
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
			baseVal8 = nekoBoomerangDisc.m_extraReturnDamageIfHitNoOne;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str8 + base.PropDesc(extraReturnDamageIfHitNoOneMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyInt shieldPerTargetHitOnThrowMod = this.m_shieldPerTargetHitOnThrowMod;
		string prefix9 = "[ShieldPerTargetHitOnThrow]";
		bool showBaseVal9 = flag;
		int baseVal9;
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
			baseVal9 = nekoBoomerangDisc.m_shieldPerTargetHitOnThrow;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(shieldPerTargetHitOnThrowMod, prefix9, showBaseVal9, baseVal9);
		return text + base.PropDesc(this.m_shieldEffectDataMod, "[ShieldEffectData]", flag, (!flag) ? null : nekoBoomerangDisc.m_shieldEffectData);
	}
}
