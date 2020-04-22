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
		if (!(nekoEnlargeDisc != null))
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
			AbilityMod.AddToken(tokens, m_laserWidthOverrideMod, "LaserWidthOverride", string.Empty, nekoEnlargeDisc.m_laserWidthOverride);
			AbilityMod.AddToken(tokens, m_aoeRadiusOverrideMod, "AoeRadiusOverride", string.Empty, nekoEnlargeDisc.m_aoeRadiusOverride);
			AbilityMod.AddToken(tokens, m_returnEndRadiusOverrideMod, "ReturnEndRadiusOverride", string.Empty, nekoEnlargeDisc.m_returnEndRadiusOverride);
			AbilityMod.AddToken(tokens, m_additionalDamageAmountMod, "AdditionalDamageAmount", string.Empty, nekoEnlargeDisc.m_additionalDamageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnEnemiesMod, "EffectOnEnemies", nekoEnlargeDisc.m_effectOnEnemies);
			AbilityMod.AddToken(tokens, m_allyHealMod, "AllyHeal", string.Empty, nekoEnlargeDisc.m_allyHeal);
			AbilityMod.AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", nekoEnlargeDisc.m_allyHitEffect);
			AbilityMod.AddToken(tokens, m_shieldPerTargetHitOnReturnMod, "ShieldPerTargetHitOnThrow", string.Empty, nekoEnlargeDisc.m_shieldPerTargetHitOnReturn);
			AbilityMod.AddToken_EffectMod(tokens, m_shieldEffectDataMod, "ShieldEffectData", nekoEnlargeDisc.m_shieldEffectData);
			AbilityMod.AddToken(tokens, m_cdrIfHitNoOneMod, "CdrIfHitNoOne", string.Empty, nekoEnlargeDisc.m_cdrIfHitNoOne);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NekoEnlargeDisc nekoEnlargeDisc = GetTargetAbilityOnAbilityData(abilityData) as NekoEnlargeDisc;
		bool flag = nekoEnlargeDisc != null;
		string empty = string.Empty;
		empty += PropDesc(m_laserWidthOverrideMod, "[LaserWidthOverride]", flag, (!flag) ? 0f : nekoEnlargeDisc.m_laserWidthOverride);
		string str = empty;
		AbilityModPropertyFloat aoeRadiusOverrideMod = m_aoeRadiusOverrideMod;
		float baseVal;
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
			baseVal = nekoEnlargeDisc.m_aoeRadiusOverride;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(aoeRadiusOverrideMod, "[AoeRadiusOverride]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat returnEndRadiusOverrideMod = m_returnEndRadiusOverrideMod;
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
			baseVal2 = nekoEnlargeDisc.m_returnEndRadiusOverride;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(returnEndRadiusOverrideMod, "[ReturnEndRadiusOverride]", flag, baseVal2);
		empty += PropDesc(m_additionalDamageAmountMod, "[AdditionalDamageAmount]", flag, flag ? nekoEnlargeDisc.m_additionalDamageAmount : 0);
		string str3 = empty;
		AbilityModPropertyEffectInfo effectOnEnemiesMod = m_effectOnEnemiesMod;
		object baseVal3;
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
			baseVal3 = nekoEnlargeDisc.m_effectOnEnemies;
		}
		else
		{
			baseVal3 = null;
		}
		empty = str3 + PropDesc(effectOnEnemiesMod, "[EffectOnEnemies]", flag, (StandardEffectInfo)baseVal3);
		string str4 = empty;
		AbilityModPropertyInt allyHealMod = m_allyHealMod;
		int baseVal4;
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
			baseVal4 = nekoEnlargeDisc.m_allyHeal;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(allyHealMod, "[AllyHeal]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyEffectInfo allyHitEffectMod = m_allyHitEffectMod;
		object baseVal5;
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
			baseVal5 = nekoEnlargeDisc.m_allyHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + PropDesc(allyHitEffectMod, "[AllyHitEffect]", flag, (StandardEffectInfo)baseVal5);
		string str6 = empty;
		AbilityModPropertyInt shieldPerTargetHitOnReturnMod = m_shieldPerTargetHitOnReturnMod;
		int baseVal6;
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
			baseVal6 = nekoEnlargeDisc.m_shieldPerTargetHitOnReturn;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(shieldPerTargetHitOnReturnMod, "[ShieldPerTargetHitOnThrow]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyEffectData shieldEffectDataMod = m_shieldEffectDataMod;
		object baseVal7;
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
			baseVal7 = nekoEnlargeDisc.m_shieldEffectData;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + PropDesc(shieldEffectDataMod, "[ShieldEffectData]", flag, (StandardActorEffectData)baseVal7);
		string str8 = empty;
		AbilityModPropertyInt cdrIfHitNoOneMod = m_cdrIfHitNoOneMod;
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
			baseVal8 = nekoEnlargeDisc.m_cdrIfHitNoOne;
		}
		else
		{
			baseVal8 = 0;
		}
		return str8 + PropDesc(cdrIfHitNoOneMod, "[CdrIfHitNoOne]", flag, baseVal8);
	}
}
