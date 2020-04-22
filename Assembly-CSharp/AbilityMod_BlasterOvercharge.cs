using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BlasterOvercharge : AbilityMod
{
	[Header("-- How many stacks are allowed")]
	public AbilityModPropertyInt m_maxCastCountMod;

	[Header("-- How many times extra damage is applied")]
	public AbilityModPropertyInt m_extraDamageCountMod;

	[Header("-- Extra Damage for all attacks except Lurker Mine")]
	public AbilityModPropertyInt m_extraDamageMod;

	[Header("-- Extra Damage for Lurker Mine")]
	public AbilityModPropertyInt m_extraDamageForDelayedLaserMod;

	[Header("-- Extra Damage for multiple stacks")]
	public AbilityModPropertyInt m_extraDamageForMultiCastMod;

	[Header("-- On Cast")]
	public AbilityModPropertyEffectInfo m_effectOnSelfOnCastMod;

	[Header("-- Extra Effects for other abilities")]
	public AbilityModPropertyEffectInfo m_extraEffectOnOtherAbilitiesMod;

	public bool m_useExtraEffectActionTypeOverride;

	public List<AbilityData.ActionType> m_extraEffectActionTypesOverride;

	public override Type GetTargetAbilityType()
	{
		return typeof(BlasterOvercharge);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BlasterOvercharge blasterOvercharge = targetAbility as BlasterOvercharge;
		if (!(blasterOvercharge != null))
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_maxCastCountMod, "MaxCastCount", string.Empty, blasterOvercharge.m_maxCastCount);
			AbilityMod.AddToken(tokens, m_extraDamageCountMod, "ExtraDamageCount", string.Empty, blasterOvercharge.m_extraDamageCount);
			AbilityMod.AddToken(tokens, m_extraDamageMod, "ExtraDamage", string.Empty, blasterOvercharge.m_extraDamage);
			AbilityMod.AddToken(tokens, m_extraDamageForDelayedLaserMod, "ExtraDamageForLurkerMine", string.Empty, blasterOvercharge.m_extraDamageForDelayedLaser);
			AbilityMod.AddToken(tokens, m_extraDamageForMultiCastMod, "ExtraDamageForMultiCast", string.Empty, blasterOvercharge.m_extraDamageForMultiCast);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnSelfOnCastMod, "EffectOnSelfOnCast", blasterOvercharge.m_effectOnSelfOnCast);
			AbilityMod.AddToken_EffectMod(tokens, m_extraEffectOnOtherAbilitiesMod, "ExtraEffectOnOtherAbilities", blasterOvercharge.m_extraEffectOnOtherAbilities);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BlasterOvercharge blasterOvercharge = GetTargetAbilityOnAbilityData(abilityData) as BlasterOvercharge;
		bool flag = blasterOvercharge != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt maxCastCountMod = m_maxCastCountMod;
		int baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = blasterOvercharge.m_maxCastCount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(maxCastCountMod, "[MaxCastCount]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt extraDamageCountMod = m_extraDamageCountMod;
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
			baseVal2 = blasterOvercharge.m_extraDamageCount;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(extraDamageCountMod, "[ExtraDamageCount]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt extraDamageMod = m_extraDamageMod;
		int baseVal3;
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
			baseVal3 = blasterOvercharge.m_extraDamage;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(extraDamageMod, "[ExtraDamage]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyInt extraDamageForDelayedLaserMod = m_extraDamageForDelayedLaserMod;
		int baseVal4;
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
			baseVal4 = blasterOvercharge.m_extraDamageForDelayedLaser;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(extraDamageForDelayedLaserMod, "[ExtraDamageForLurkerMine]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyInt extraDamageForMultiCastMod = m_extraDamageForMultiCastMod;
		int baseVal5;
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
			baseVal5 = blasterOvercharge.m_extraDamageForMultiCast;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(extraDamageForMultiCastMod, "[ExtraDamageForMultiCast]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyEffectInfo effectOnSelfOnCastMod = m_effectOnSelfOnCastMod;
		object baseVal6;
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
			baseVal6 = blasterOvercharge.m_effectOnSelfOnCast;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str6 + PropDesc(effectOnSelfOnCastMod, "[EffectOnSelfOnCast]", flag, (StandardEffectInfo)baseVal6);
		string str7 = empty;
		AbilityModPropertyEffectInfo extraEffectOnOtherAbilitiesMod = m_extraEffectOnOtherAbilitiesMod;
		object baseVal7;
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
			baseVal7 = blasterOvercharge.m_extraEffectOnOtherAbilities;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + PropDesc(extraEffectOnOtherAbilitiesMod, "[ExtraEffectOnOtherAbilities]", flag, (StandardEffectInfo)baseVal7);
		if (m_useExtraEffectActionTypeOverride)
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
			if (m_extraEffectActionTypesOverride != null)
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
				empty += "Using override for extra effect target abilities:\n";
				for (int i = 0; i < m_extraEffectActionTypesOverride.Count; i++)
				{
					empty = empty + "    " + m_extraEffectActionTypesOverride[i].ToString() + "\n";
				}
				while (true)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				empty += "\n";
			}
		}
		return empty;
	}
}
