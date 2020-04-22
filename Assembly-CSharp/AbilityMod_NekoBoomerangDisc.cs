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
			AbilityMod.AddToken(tokens, m_laserLengthMod, "LaserLength", string.Empty, nekoBoomerangDisc.m_laserLength);
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, nekoBoomerangDisc.m_laserWidth);
			AbilityMod.AddToken(tokens, m_aoeRadiusAtEndMod, "AoeRadiusAtEnd", string.Empty, nekoBoomerangDisc.m_aoeRadiusAtEnd);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, nekoBoomerangDisc.m_maxTargets);
			AbilityMod.AddToken(tokens, m_discReturnEndRadiusMod, "DiscReturnEndRadius", string.Empty, nekoBoomerangDisc.m_discReturnEndRadius);
			AbilityMod.AddToken(tokens, m_directDamageMod, "DirectDamage", string.Empty, nekoBoomerangDisc.m_directDamage);
			AbilityMod.AddToken(tokens, m_returnTripDamageMod, "ReturnTripDamage", string.Empty, nekoBoomerangDisc.m_returnTripDamage);
			AbilityMod.AddToken(tokens, m_extraDamageIfHitByReturnDiscMod, "ExtraDamageIfHitByReturnDisc", string.Empty, nekoBoomerangDisc.m_extraDamageIfHitByReturnDisc);
			AbilityMod.AddToken(tokens, m_extraReturnDamageIfHitNoOneMod, "ExtraReturnDamageIfHitNoOne", string.Empty, nekoBoomerangDisc.m_extraReturnDamageIfHitNoOne);
			AbilityMod.AddToken(tokens, m_shieldPerTargetHitOnThrowMod, "ShieldPerTargetHitOnThrow", string.Empty, nekoBoomerangDisc.m_shieldPerTargetHitOnThrow);
			AbilityMod.AddToken_EffectMod(tokens, m_shieldEffectDataMod, "ShieldEffectData", nekoBoomerangDisc.m_shieldEffectData);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NekoBoomerangDisc nekoBoomerangDisc = GetTargetAbilityOnAbilityData(abilityData) as NekoBoomerangDisc;
		bool flag = nekoBoomerangDisc != null;
		string empty = string.Empty;
		empty += PropDesc(m_laserLengthMod, "[LaserLength]", flag, (!flag) ? 0f : nekoBoomerangDisc.m_laserLength);
		string str = empty;
		AbilityModPropertyFloat laserWidthMod = m_laserWidthMod;
		float baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = nekoBoomerangDisc.m_laserWidth;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(laserWidthMod, "[LaserWidth]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat aoeRadiusAtEndMod = m_aoeRadiusAtEndMod;
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
			baseVal2 = nekoBoomerangDisc.m_aoeRadiusAtEnd;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(aoeRadiusAtEndMod, "[AoeRadiusAtEnd]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt maxTargetsMod = m_maxTargetsMod;
		int baseVal3;
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
			baseVal3 = nekoBoomerangDisc.m_maxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(maxTargetsMod, "[MaxTargets]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat discReturnEndRadiusMod = m_discReturnEndRadiusMod;
		float baseVal4;
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
			baseVal4 = nekoBoomerangDisc.m_discReturnEndRadius;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(discReturnEndRadiusMod, "[DiscReturnEndRadius]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyInt directDamageMod = m_directDamageMod;
		int baseVal5;
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
			baseVal5 = nekoBoomerangDisc.m_directDamage;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(directDamageMod, "[DirectDamage]", flag, baseVal5);
		empty += PropDesc(m_returnTripDamageMod, "[ReturnTripDamage]", flag, flag ? nekoBoomerangDisc.m_returnTripDamage : 0);
		string str6 = empty;
		AbilityModPropertyBool returnTripIgnoreCoverMod = m_returnTripIgnoreCoverMod;
		int baseVal6;
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
			baseVal6 = (nekoBoomerangDisc.m_returnTripIgnoreCover ? 1 : 0);
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(returnTripIgnoreCoverMod, "[ReturnTripIgnoreCover]", flag, (byte)baseVal6 != 0);
		string str7 = empty;
		AbilityModPropertyInt extraDamageIfHitByReturnDiscMod = m_extraDamageIfHitByReturnDiscMod;
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
			baseVal7 = nekoBoomerangDisc.m_extraDamageIfHitByReturnDisc;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(extraDamageIfHitByReturnDiscMod, "[ExtraDamageIfHitByReturnDisc]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyInt extraReturnDamageIfHitNoOneMod = m_extraReturnDamageIfHitNoOneMod;
		int baseVal8;
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
			baseVal8 = nekoBoomerangDisc.m_extraReturnDamageIfHitNoOne;
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(extraReturnDamageIfHitNoOneMod, "[ExtraReturnDamageIfHitNoOne]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyInt shieldPerTargetHitOnThrowMod = m_shieldPerTargetHitOnThrowMod;
		int baseVal9;
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
			baseVal9 = nekoBoomerangDisc.m_shieldPerTargetHitOnThrow;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(shieldPerTargetHitOnThrowMod, "[ShieldPerTargetHitOnThrow]", flag, baseVal9);
		return empty + PropDesc(m_shieldEffectDataMod, "[ShieldEffectData]", flag, (!flag) ? null : nekoBoomerangDisc.m_shieldEffectData);
	}
}
