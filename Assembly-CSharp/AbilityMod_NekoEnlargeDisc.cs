using System;
using System.Collections.Generic;

public class AbilityMod_NekoEnlargeDisc : AbilityMod
{
	[Separator("Targeting", true)]
	public AbilityModPropertyFloat m_laserWidthOverrideMod;

	public AbilityModPropertyFloat m_aoeRadiusOverrideMod;

	public AbilityModPropertyFloat m_returnEndRadiusOverrideMod;

	[Separator("On Hit Damage/Effect", true)]
	public AbilityModPropertyInt m_additionalDamageAmountMod;

	public AbilityModPropertyEffectInfo m_effectOnEnemiesMod;

	[Separator("Ally Hits", true)]
	public AbilityModPropertyInt m_allyHealMod;

	public AbilityModPropertyEffectInfo m_allyHitEffectMod;

	[Separator("Shielding for target hit on return (applied on start of next turn)", true)]
	public AbilityModPropertyInt m_shieldPerTargetHitOnReturnMod;

	public AbilityModPropertyEffectData m_shieldEffectDataMod;

	[Separator("Cooldown Reduction", true)]
	public AbilityModPropertyInt m_cdrIfHitNoOneMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NekoEnlargeDisc);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NekoEnlargeDisc nekoEnlargeDisc = targetAbility as NekoEnlargeDisc;
		if (nekoEnlargeDisc != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NekoEnlargeDisc.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_laserWidthOverrideMod, "LaserWidthOverride", string.Empty, nekoEnlargeDisc.m_laserWidthOverride, true, false, false);
			AbilityMod.AddToken(tokens, this.m_aoeRadiusOverrideMod, "AoeRadiusOverride", string.Empty, nekoEnlargeDisc.m_aoeRadiusOverride, true, false, false);
			AbilityMod.AddToken(tokens, this.m_returnEndRadiusOverrideMod, "ReturnEndRadiusOverride", string.Empty, nekoEnlargeDisc.m_returnEndRadiusOverride, true, false, false);
			AbilityMod.AddToken(tokens, this.m_additionalDamageAmountMod, "AdditionalDamageAmount", string.Empty, nekoEnlargeDisc.m_additionalDamageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnEnemiesMod, "EffectOnEnemies", nekoEnlargeDisc.m_effectOnEnemies, true);
			AbilityMod.AddToken(tokens, this.m_allyHealMod, "AllyHeal", string.Empty, nekoEnlargeDisc.m_allyHeal, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyHitEffectMod, "AllyHitEffect", nekoEnlargeDisc.m_allyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_shieldPerTargetHitOnReturnMod, "ShieldPerTargetHitOnThrow", string.Empty, nekoEnlargeDisc.m_shieldPerTargetHitOnReturn, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_shieldEffectDataMod, "ShieldEffectData", nekoEnlargeDisc.m_shieldEffectData, true);
			AbilityMod.AddToken(tokens, this.m_cdrIfHitNoOneMod, "CdrIfHitNoOne", string.Empty, nekoEnlargeDisc.m_cdrIfHitNoOne, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NekoEnlargeDisc nekoEnlargeDisc = base.GetTargetAbilityOnAbilityData(abilityData) as NekoEnlargeDisc;
		bool flag = nekoEnlargeDisc != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_laserWidthOverrideMod, "[LaserWidthOverride]", flag, (!flag) ? 0f : nekoEnlargeDisc.m_laserWidthOverride);
		string str = text;
		AbilityModPropertyFloat aoeRadiusOverrideMod = this.m_aoeRadiusOverrideMod;
		string prefix = "[AoeRadiusOverride]";
		bool showBaseVal = flag;
		float baseVal;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NekoEnlargeDisc.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = nekoEnlargeDisc.m_aoeRadiusOverride;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(aoeRadiusOverrideMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat returnEndRadiusOverrideMod = this.m_returnEndRadiusOverrideMod;
		string prefix2 = "[ReturnEndRadiusOverride]";
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
			baseVal2 = nekoEnlargeDisc.m_returnEndRadiusOverride;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(returnEndRadiusOverrideMod, prefix2, showBaseVal2, baseVal2);
		text += base.PropDesc(this.m_additionalDamageAmountMod, "[AdditionalDamageAmount]", flag, (!flag) ? 0 : nekoEnlargeDisc.m_additionalDamageAmount);
		string str3 = text;
		AbilityModPropertyEffectInfo effectOnEnemiesMod = this.m_effectOnEnemiesMod;
		string prefix3 = "[EffectOnEnemies]";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal3;
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
			baseVal3 = nekoEnlargeDisc.m_effectOnEnemies;
		}
		else
		{
			baseVal3 = null;
		}
		text = str3 + base.PropDesc(effectOnEnemiesMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt allyHealMod = this.m_allyHealMod;
		string prefix4 = "[AllyHeal]";
		bool showBaseVal4 = flag;
		int baseVal4;
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
			baseVal4 = nekoEnlargeDisc.m_allyHeal;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(allyHealMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyEffectInfo allyHitEffectMod = this.m_allyHitEffectMod;
		string prefix5 = "[AllyHitEffect]";
		bool showBaseVal5 = flag;
		StandardEffectInfo baseVal5;
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
			baseVal5 = nekoEnlargeDisc.m_allyHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(allyHitEffectMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt shieldPerTargetHitOnReturnMod = this.m_shieldPerTargetHitOnReturnMod;
		string prefix6 = "[ShieldPerTargetHitOnThrow]";
		bool showBaseVal6 = flag;
		int baseVal6;
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
			baseVal6 = nekoEnlargeDisc.m_shieldPerTargetHitOnReturn;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(shieldPerTargetHitOnReturnMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyEffectData shieldEffectDataMod = this.m_shieldEffectDataMod;
		string prefix7 = "[ShieldEffectData]";
		bool showBaseVal7 = flag;
		StandardActorEffectData baseVal7;
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
			baseVal7 = nekoEnlargeDisc.m_shieldEffectData;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + base.PropDesc(shieldEffectDataMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyInt cdrIfHitNoOneMod = this.m_cdrIfHitNoOneMod;
		string prefix8 = "[CdrIfHitNoOne]";
		bool showBaseVal8 = flag;
		int baseVal8;
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
			baseVal8 = nekoEnlargeDisc.m_cdrIfHitNoOne;
		}
		else
		{
			baseVal8 = 0;
		}
		return str8 + base.PropDesc(cdrIfHitNoOneMod, prefix8, showBaseVal8, baseVal8);
	}
}
