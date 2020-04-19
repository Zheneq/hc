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
		if (blasterOvercharge != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_BlasterOvercharge.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_maxCastCountMod, "MaxCastCount", string.Empty, blasterOvercharge.m_maxCastCount, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageCountMod, "ExtraDamageCount", string.Empty, blasterOvercharge.m_extraDamageCount, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageMod, "ExtraDamage", string.Empty, blasterOvercharge.m_extraDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageForDelayedLaserMod, "ExtraDamageForLurkerMine", string.Empty, blasterOvercharge.m_extraDamageForDelayedLaser, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageForMultiCastMod, "ExtraDamageForMultiCast", string.Empty, blasterOvercharge.m_extraDamageForMultiCast, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnSelfOnCastMod, "EffectOnSelfOnCast", blasterOvercharge.m_effectOnSelfOnCast, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_extraEffectOnOtherAbilitiesMod, "ExtraEffectOnOtherAbilities", blasterOvercharge.m_extraEffectOnOtherAbilities, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BlasterOvercharge blasterOvercharge = base.GetTargetAbilityOnAbilityData(abilityData) as BlasterOvercharge;
		bool flag = blasterOvercharge != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt maxCastCountMod = this.m_maxCastCountMod;
		string prefix = "[MaxCastCount]";
		bool showBaseVal = flag;
		int baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_BlasterOvercharge.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = blasterOvercharge.m_maxCastCount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(maxCastCountMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt extraDamageCountMod = this.m_extraDamageCountMod;
		string prefix2 = "[ExtraDamageCount]";
		bool showBaseVal2 = flag;
		int baseVal2;
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
			baseVal2 = blasterOvercharge.m_extraDamageCount;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(extraDamageCountMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt extraDamageMod = this.m_extraDamageMod;
		string prefix3 = "[ExtraDamage]";
		bool showBaseVal3 = flag;
		int baseVal3;
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
			baseVal3 = blasterOvercharge.m_extraDamage;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(extraDamageMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt extraDamageForDelayedLaserMod = this.m_extraDamageForDelayedLaserMod;
		string prefix4 = "[ExtraDamageForLurkerMine]";
		bool showBaseVal4 = flag;
		int baseVal4;
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
			baseVal4 = blasterOvercharge.m_extraDamageForDelayedLaser;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(extraDamageForDelayedLaserMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt extraDamageForMultiCastMod = this.m_extraDamageForMultiCastMod;
		string prefix5 = "[ExtraDamageForMultiCast]";
		bool showBaseVal5 = flag;
		int baseVal5;
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
			baseVal5 = blasterOvercharge.m_extraDamageForMultiCast;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(extraDamageForMultiCastMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyEffectInfo effectOnSelfOnCastMod = this.m_effectOnSelfOnCastMod;
		string prefix6 = "[EffectOnSelfOnCast]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal6;
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
			baseVal6 = blasterOvercharge.m_effectOnSelfOnCast;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + base.PropDesc(effectOnSelfOnCastMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyEffectInfo extraEffectOnOtherAbilitiesMod = this.m_extraEffectOnOtherAbilitiesMod;
		string prefix7 = "[ExtraEffectOnOtherAbilities]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
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
			baseVal7 = blasterOvercharge.m_extraEffectOnOtherAbilities;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + base.PropDesc(extraEffectOnOtherAbilitiesMod, prefix7, showBaseVal7, baseVal7);
		if (this.m_useExtraEffectActionTypeOverride)
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
			if (this.m_extraEffectActionTypesOverride != null)
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
				text += "Using override for extra effect target abilities:\n";
				for (int i = 0; i < this.m_extraEffectActionTypesOverride.Count; i++)
				{
					text = text + "    " + this.m_extraEffectActionTypesOverride[i].ToString() + "\n";
				}
				for (;;)
				{
					switch (1)
					{
					case 0:
						continue;
					}
					break;
				}
				text += "\n";
			}
		}
		return text;
	}
}
