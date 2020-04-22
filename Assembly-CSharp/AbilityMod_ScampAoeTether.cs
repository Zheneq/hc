using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ScampAoeTether : AbilityMod
{
	[Separator("Targeting", true)]
	public AbilityModPropertyFloat m_aoeRadiusMod;

	public AbilityModPropertyBool m_ignoreLosMod;

	[Header("-- if > 0, will use this as distance to check whether tether should break")]
	public AbilityModPropertyFloat m_tetherBreakDistanceOverrideMod;

	[Separator("Whether to pull towards caster if target is out of range. If true, no longer do movement hits", true)]
	public AbilityModPropertyBool m_pullToCasterInKnockbackMod;

	public AbilityModPropertyFloat m_maxKnockbackDistMod;

	[Separator("Disable in Shield Down mode?", true)]
	public AbilityModPropertyBool m_disableIfShieldDownMod;

	[Separator("On Tether Apply", true)]
	public AbilityModPropertyEffectInfo m_tetherApplyEnemyEffectMod;

	[Separator("On Tether Break", true)]
	public AbilityModPropertyInt m_tetherBreakDamageMod;

	public AbilityModPropertyEffectInfo m_tetherBreakEnemyEffecfMod;

	[Separator("Cdr if not triggered", true)]
	public AbilityModPropertyInt m_cdrIfNoTetherTriggerMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ScampAoeTether);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScampAoeTether scampAoeTether = targetAbility as ScampAoeTether;
		if (!(scampAoeTether != null))
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_aoeRadiusMod, "AoeRadius", string.Empty, scampAoeTether.m_aoeRadius);
			AbilityMod.AddToken(tokens, m_tetherBreakDistanceOverrideMod, "TetherBreakDistanceOverride", string.Empty, scampAoeTether.m_tetherBreakDistanceOverride);
			AbilityMod.AddToken(tokens, m_maxKnockbackDistMod, "MaxKnockbackDist", string.Empty, scampAoeTether.m_maxKnockbackDist);
			AbilityMod.AddToken_EffectMod(tokens, m_tetherApplyEnemyEffectMod, "TetherApplyEnemyEffect", scampAoeTether.m_tetherApplyEnemyEffect);
			AbilityMod.AddToken(tokens, m_tetherBreakDamageMod, "TetherBreakDamage", string.Empty, scampAoeTether.m_tetherBreakDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_tetherBreakEnemyEffecfMod, "TetherBreakEnemyEffecf", scampAoeTether.m_tetherBreakEnemyEffecf);
			AbilityMod.AddToken(tokens, m_cdrIfNoTetherTriggerMod, "CdrIfNoTetherTrigger", string.Empty, scampAoeTether.m_cdrIfNoTetherTrigger);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScampAoeTether scampAoeTether = GetTargetAbilityOnAbilityData(abilityData) as ScampAoeTether;
		bool flag = scampAoeTether != null;
		string empty = string.Empty;
		empty += PropDesc(m_aoeRadiusMod, "[AoeRadius]", flag, (!flag) ? 0f : scampAoeTether.m_aoeRadius);
		string str = empty;
		AbilityModPropertyBool ignoreLosMod = m_ignoreLosMod;
		int baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = (scampAoeTether.m_ignoreLos ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(ignoreLosMod, "[IgnoreLos]", flag, (byte)baseVal != 0);
		string str2 = empty;
		AbilityModPropertyFloat tetherBreakDistanceOverrideMod = m_tetherBreakDistanceOverrideMod;
		float baseVal2;
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
			baseVal2 = scampAoeTether.m_tetherBreakDistanceOverride;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(tetherBreakDistanceOverrideMod, "[TetherBreakDistanceOverride]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyBool pullToCasterInKnockbackMod = m_pullToCasterInKnockbackMod;
		int baseVal3;
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
			baseVal3 = (scampAoeTether.m_pullToCasterInKnockback ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(pullToCasterInKnockbackMod, "[PullToCasterInKnockback]", flag, (byte)baseVal3 != 0);
		string str4 = empty;
		AbilityModPropertyFloat maxKnockbackDistMod = m_maxKnockbackDistMod;
		float baseVal4;
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
			baseVal4 = scampAoeTether.m_maxKnockbackDist;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(maxKnockbackDistMod, "[MaxKnockbackDist]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyBool disableIfShieldDownMod = m_disableIfShieldDownMod;
		int baseVal5;
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
			baseVal5 = (scampAoeTether.m_disableIfShieldDown ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(disableIfShieldDownMod, "[DisableIfShieldDown]", flag, (byte)baseVal5 != 0);
		empty += PropDesc(m_tetherApplyEnemyEffectMod, "[TetherApplyEnemyEffect]", flag, (!flag) ? null : scampAoeTether.m_tetherApplyEnemyEffect);
		string str6 = empty;
		AbilityModPropertyInt tetherBreakDamageMod = m_tetherBreakDamageMod;
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
			baseVal6 = scampAoeTether.m_tetherBreakDamage;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(tetherBreakDamageMod, "[TetherBreakDamage]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyEffectInfo tetherBreakEnemyEffecfMod = m_tetherBreakEnemyEffecfMod;
		object baseVal7;
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
			baseVal7 = scampAoeTether.m_tetherBreakEnemyEffecf;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + PropDesc(tetherBreakEnemyEffecfMod, "[TetherBreakEnemyEffecf]", flag, (StandardEffectInfo)baseVal7);
		string str8 = empty;
		AbilityModPropertyInt cdrIfNoTetherTriggerMod = m_cdrIfNoTetherTriggerMod;
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
			baseVal8 = scampAoeTether.m_cdrIfNoTetherTrigger;
		}
		else
		{
			baseVal8 = 0;
		}
		return str8 + PropDesc(cdrIfNoTetherTriggerMod, "[CdrIfNoTetherTrigger]", flag, baseVal8);
	}
}
