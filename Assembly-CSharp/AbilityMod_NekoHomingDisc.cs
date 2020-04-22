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
		if (!(nekoHomingDisc != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_laserLengthMod, "LaserLength", string.Empty, nekoHomingDisc.m_laserLength);
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, nekoHomingDisc.m_laserWidth);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, nekoHomingDisc.m_maxTargets);
			AbilityMod.AddToken(tokens, m_discReturnEndRadiusMod, "DiscReturnEndRadius", string.Empty, nekoHomingDisc.m_discReturnEndRadius);
			AbilityMod.AddToken_EffectMod(tokens, m_onCastEnemyHitEffectMod, "OnCastEnemyHitEffect", nekoHomingDisc.m_onCastEnemyHitEffect);
			AbilityMod.AddToken(tokens, m_targetDamageMod, "TargetDamage", string.Empty, nekoHomingDisc.m_targetDamage);
			AbilityMod.AddToken(tokens, m_returnTripDamageMod, "ReturnTripDamage", string.Empty, nekoHomingDisc.m_returnTripDamage);
			AbilityMod.AddToken(tokens, m_extraReturnDamagePerDistMod, "ExtraReturnDamagePerDist", string.Empty, nekoHomingDisc.m_extraReturnDamagePerDist);
			AbilityMod.AddToken_EffectMod(tokens, m_returnTripEnemyEffectMod, "ReturnTripEnemyEffect", nekoHomingDisc.m_returnTripEnemyEffect);
			AbilityMod.AddToken(tokens, m_cdrIfHitNoOneOnCastMod, "CdrIfHitNoOneOnCast", string.Empty, nekoHomingDisc.m_cdrIfHitNoOneOnCast);
			AbilityMod.AddToken(tokens, m_cdrIfHitNoOneOnReturnMod, "CdrIfHitNoOneOnReturn", string.Empty, nekoHomingDisc.m_cdrIfHitNoOneOnReturn);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NekoHomingDisc nekoHomingDisc = GetTargetAbilityOnAbilityData(abilityData) as NekoHomingDisc;
		bool flag = nekoHomingDisc != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat laserLengthMod = m_laserLengthMod;
		float baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = nekoHomingDisc.m_laserLength;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(laserLengthMod, "[LaserLength]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat laserWidthMod = m_laserWidthMod;
		float baseVal2;
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
			baseVal2 = nekoHomingDisc.m_laserWidth;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(laserWidthMod, "[LaserWidth]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt maxTargetsMod = m_maxTargetsMod;
		int baseVal3;
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
			baseVal3 = nekoHomingDisc.m_maxTargets;
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
			baseVal4 = nekoHomingDisc.m_discReturnEndRadius;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(discReturnEndRadiusMod, "[DiscReturnEndRadius]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyEffectInfo onCastEnemyHitEffectMod = m_onCastEnemyHitEffectMod;
		object baseVal5;
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
			baseVal5 = nekoHomingDisc.m_onCastEnemyHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + PropDesc(onCastEnemyHitEffectMod, "[OnCastEnemyHitEffect]", flag, (StandardEffectInfo)baseVal5);
		string str6 = empty;
		AbilityModPropertyInt targetDamageMod = m_targetDamageMod;
		int baseVal6;
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
			baseVal6 = nekoHomingDisc.m_targetDamage;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(targetDamageMod, "[TargetDamage]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyInt returnTripDamageMod = m_returnTripDamageMod;
		int baseVal7;
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
			baseVal7 = nekoHomingDisc.m_returnTripDamage;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(returnTripDamageMod, "[ReturnTripDamage]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyBool returnTripIgnoreCoverMod = m_returnTripIgnoreCoverMod;
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
			baseVal8 = (nekoHomingDisc.m_returnTripIgnoreCover ? 1 : 0);
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(returnTripIgnoreCoverMod, "[ReturnTripIgnoreCover]", flag, (byte)baseVal8 != 0);
		string str9 = empty;
		AbilityModPropertyFloat extraReturnDamagePerDistMod = m_extraReturnDamagePerDistMod;
		float baseVal9;
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
			baseVal9 = nekoHomingDisc.m_extraReturnDamagePerDist;
		}
		else
		{
			baseVal9 = 0f;
		}
		empty = str9 + PropDesc(extraReturnDamagePerDistMod, "[ExtraReturnDamagePerDist]", flag, baseVal9);
		empty += PropDesc(m_returnTripEnemyEffectMod, "[ReturnTripEnemyEffect]", flag, (!flag) ? null : nekoHomingDisc.m_returnTripEnemyEffect);
		string str10 = empty;
		AbilityModPropertyInt cdrIfHitNoOneOnCastMod = m_cdrIfHitNoOneOnCastMod;
		int baseVal10;
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
			baseVal10 = nekoHomingDisc.m_cdrIfHitNoOneOnCast;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(cdrIfHitNoOneOnCastMod, "[CdrIfHitNoOneOnCast]", flag, baseVal10);
		return empty + PropDesc(m_cdrIfHitNoOneOnReturnMod, "[CdrIfHitNoOneOnReturn]", flag, flag ? nekoHomingDisc.m_cdrIfHitNoOneOnReturn : 0);
	}
}
