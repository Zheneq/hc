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
		if (scampAoeTether != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ScampAoeTether.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_aoeRadiusMod, "AoeRadius", string.Empty, scampAoeTether.m_aoeRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_tetherBreakDistanceOverrideMod, "TetherBreakDistanceOverride", string.Empty, scampAoeTether.m_tetherBreakDistanceOverride, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxKnockbackDistMod, "MaxKnockbackDist", string.Empty, scampAoeTether.m_maxKnockbackDist, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_tetherApplyEnemyEffectMod, "TetherApplyEnemyEffect", scampAoeTether.m_tetherApplyEnemyEffect, true);
			AbilityMod.AddToken(tokens, this.m_tetherBreakDamageMod, "TetherBreakDamage", string.Empty, scampAoeTether.m_tetherBreakDamage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_tetherBreakEnemyEffecfMod, "TetherBreakEnemyEffecf", scampAoeTether.m_tetherBreakEnemyEffecf, true);
			AbilityMod.AddToken(tokens, this.m_cdrIfNoTetherTriggerMod, "CdrIfNoTetherTrigger", string.Empty, scampAoeTether.m_cdrIfNoTetherTrigger, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScampAoeTether scampAoeTether = base.GetTargetAbilityOnAbilityData(abilityData) as ScampAoeTether;
		bool flag = scampAoeTether != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_aoeRadiusMod, "[AoeRadius]", flag, (!flag) ? 0f : scampAoeTether.m_aoeRadius);
		string str = text;
		AbilityModPropertyBool ignoreLosMod = this.m_ignoreLosMod;
		string prefix = "[IgnoreLos]";
		bool showBaseVal = flag;
		bool baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ScampAoeTether.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = scampAoeTether.m_ignoreLos;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(ignoreLosMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat tetherBreakDistanceOverrideMod = this.m_tetherBreakDistanceOverrideMod;
		string prefix2 = "[TetherBreakDistanceOverride]";
		bool showBaseVal2 = flag;
		float baseVal2;
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
			baseVal2 = scampAoeTether.m_tetherBreakDistanceOverride;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(tetherBreakDistanceOverrideMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyBool pullToCasterInKnockbackMod = this.m_pullToCasterInKnockbackMod;
		string prefix3 = "[PullToCasterInKnockback]";
		bool showBaseVal3 = flag;
		bool baseVal3;
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
			baseVal3 = scampAoeTether.m_pullToCasterInKnockback;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + base.PropDesc(pullToCasterInKnockbackMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyFloat maxKnockbackDistMod = this.m_maxKnockbackDistMod;
		string prefix4 = "[MaxKnockbackDist]";
		bool showBaseVal4 = flag;
		float baseVal4;
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
			baseVal4 = scampAoeTether.m_maxKnockbackDist;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str4 + base.PropDesc(maxKnockbackDistMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool disableIfShieldDownMod = this.m_disableIfShieldDownMod;
		string prefix5 = "[DisableIfShieldDown]";
		bool showBaseVal5 = flag;
		bool baseVal5;
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
			baseVal5 = scampAoeTether.m_disableIfShieldDown;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(disableIfShieldDownMod, prefix5, showBaseVal5, baseVal5);
		text += base.PropDesc(this.m_tetherApplyEnemyEffectMod, "[TetherApplyEnemyEffect]", flag, (!flag) ? null : scampAoeTether.m_tetherApplyEnemyEffect);
		string str6 = text;
		AbilityModPropertyInt tetherBreakDamageMod = this.m_tetherBreakDamageMod;
		string prefix6 = "[TetherBreakDamage]";
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
			baseVal6 = scampAoeTether.m_tetherBreakDamage;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(tetherBreakDamageMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyEffectInfo tetherBreakEnemyEffecfMod = this.m_tetherBreakEnemyEffecfMod;
		string prefix7 = "[TetherBreakEnemyEffecf]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
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
			baseVal7 = scampAoeTether.m_tetherBreakEnemyEffecf;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + base.PropDesc(tetherBreakEnemyEffecfMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyInt cdrIfNoTetherTriggerMod = this.m_cdrIfNoTetherTriggerMod;
		string prefix8 = "[CdrIfNoTetherTrigger]";
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
			baseVal8 = scampAoeTether.m_cdrIfNoTetherTrigger;
		}
		else
		{
			baseVal8 = 0;
		}
		return str8 + base.PropDesc(cdrIfNoTetherTriggerMod, prefix8, showBaseVal8, baseVal8);
	}
}
