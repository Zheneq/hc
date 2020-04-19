using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SenseiYingYangDash : AbilityMod
{
	[Separator("Targeting Info", "cyan")]
	public AbilityModPropertyShape m_chooseDestShapeMod;

	[Separator("For Second Dash", "cyan")]
	public AbilityModPropertyInt m_secondCastTurnsMod;

	public AbilityModPropertyBool m_secondDashAllowBothTeamsMod;

	[Separator("On Enemy Hit", true)]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	public AbilityModPropertyInt m_extraDamageForDiffTeamSecondDashMod;

	public AbilityModPropertyInt m_extraDamageForLowHealthMod;

	public AbilityModPropertyFloat m_enemyLowHealthThreshMod;

	public AbilityModPropertyBool m_reverseHealthThreshForEnemyMod;

	[Separator("On Ally Hit", true)]
	public AbilityModPropertyInt m_healOnAllyMod;

	public AbilityModPropertyEffectInfo m_allyHitEffectMod;

	public AbilityModPropertyInt m_extraHealOnAllyForDiffTeamSecondDashMod;

	public AbilityModPropertyInt m_extraHealOnAllyForLowHealthMod;

	public AbilityModPropertyFloat m_allyLowHealthThreshMod;

	public AbilityModPropertyBool m_reverseHealthThreshForAllyMod;

	[Header("-- Cooldown reduction")]
	public AbilityModPropertyInt m_cdrIfNoSecondDashMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SenseiYingYangDash);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SenseiYingYangDash senseiYingYangDash = targetAbility as SenseiYingYangDash;
		if (senseiYingYangDash != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SenseiYingYangDash.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_secondCastTurnsMod, "SecondCastTurns", string.Empty, senseiYingYangDash.m_secondCastTurns, true, false);
			AbilityMod.AddToken(tokens, this.m_damageMod, "Damage", string.Empty, senseiYingYangDash.m_damage, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", senseiYingYangDash.m_enemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_extraDamageForDiffTeamSecondDashMod, "ExtraDamageForDiffTeamSecondDash", string.Empty, senseiYingYangDash.m_extraDamageForDiffTeamSecondDash, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageForLowHealthMod, "ExtraDamageForLowHealth", string.Empty, senseiYingYangDash.m_extraDamageForLowHealth, true, false);
			AbilityMod.AddToken(tokens, this.m_enemyLowHealthThreshMod, "EnemyLowHealthThresh", string.Empty, senseiYingYangDash.m_enemyLowHealthThresh, true, false, true);
			AbilityMod.AddToken(tokens, this.m_healOnAllyMod, "HealOnAlly", string.Empty, senseiYingYangDash.m_healOnAlly, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyHitEffectMod, "AllyHitEffect", senseiYingYangDash.m_allyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_extraHealOnAllyForDiffTeamSecondDashMod, "ExtraHealOnAllyForDiffTeamSecondDash", string.Empty, senseiYingYangDash.m_extraHealOnAllyForDiffTeamSecondDash, true, false);
			AbilityMod.AddToken(tokens, this.m_extraHealOnAllyForLowHealthMod, "ExtraHealOnAllyForLowHealth", string.Empty, senseiYingYangDash.m_extraHealOnAllyForLowHealth, true, false);
			AbilityMod.AddToken(tokens, this.m_allyLowHealthThreshMod, "AllyLowHealthThresh", string.Empty, senseiYingYangDash.m_allyLowHealthThresh, true, false, true);
			AbilityMod.AddToken(tokens, this.m_cdrIfNoSecondDashMod, "CdrIfNoSecondDash", string.Empty, senseiYingYangDash.m_cdrIfNoSecondDash, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SenseiYingYangDash senseiYingYangDash = base.GetTargetAbilityOnAbilityData(abilityData) as SenseiYingYangDash;
		bool flag = senseiYingYangDash != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyShape chooseDestShapeMod = this.m_chooseDestShapeMod;
		string prefix = "[ChooseDestShape]";
		bool showBaseVal = flag;
		AbilityAreaShape baseVal;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SenseiYingYangDash.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = senseiYingYangDash.m_chooseDestShape;
		}
		else
		{
			baseVal = AbilityAreaShape.SingleSquare;
		}
		text = str + base.PropDesc(chooseDestShapeMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt secondCastTurnsMod = this.m_secondCastTurnsMod;
		string prefix2 = "[SecondCastTurns]";
		bool showBaseVal2 = flag;
		int baseVal2;
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
			baseVal2 = senseiYingYangDash.m_secondCastTurns;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(secondCastTurnsMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyBool secondDashAllowBothTeamsMod = this.m_secondDashAllowBothTeamsMod;
		string prefix3 = "[SecondDashAllowBothTeams]";
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
			baseVal3 = senseiYingYangDash.m_secondDashAllowBothTeams;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + base.PropDesc(secondDashAllowBothTeamsMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix4 = "[Damage]";
		bool showBaseVal4 = flag;
		int baseVal4;
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
			baseVal4 = senseiYingYangDash.m_damage;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(damageMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyEffectInfo enemyHitEffectMod = this.m_enemyHitEffectMod;
		string prefix5 = "[EnemyHitEffect]";
		bool showBaseVal5 = flag;
		StandardEffectInfo baseVal5;
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
			baseVal5 = senseiYingYangDash.m_enemyHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(enemyHitEffectMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt extraDamageForDiffTeamSecondDashMod = this.m_extraDamageForDiffTeamSecondDashMod;
		string prefix6 = "[ExtraDamageForDiffTeamSecondDash]";
		bool showBaseVal6 = flag;
		int baseVal6;
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
			baseVal6 = senseiYingYangDash.m_extraDamageForDiffTeamSecondDash;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(extraDamageForDiffTeamSecondDashMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_extraDamageForLowHealthMod, "[ExtraDamageForLowHealth]", flag, (!flag) ? 0 : senseiYingYangDash.m_extraDamageForLowHealth);
		string str7 = text;
		AbilityModPropertyFloat enemyLowHealthThreshMod = this.m_enemyLowHealthThreshMod;
		string prefix7 = "[EnemyLowHealthThresh]";
		bool showBaseVal7 = flag;
		float baseVal7;
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
			baseVal7 = senseiYingYangDash.m_enemyLowHealthThresh;
		}
		else
		{
			baseVal7 = 0f;
		}
		text = str7 + base.PropDesc(enemyLowHealthThreshMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyBool reverseHealthThreshForEnemyMod = this.m_reverseHealthThreshForEnemyMod;
		string prefix8 = "[ReverseHealthThreshForEnemy]";
		bool showBaseVal8 = flag;
		bool baseVal8;
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
			baseVal8 = senseiYingYangDash.m_reverseHealthThreshForEnemy;
		}
		else
		{
			baseVal8 = false;
		}
		text = str8 + base.PropDesc(reverseHealthThreshForEnemyMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyInt healOnAllyMod = this.m_healOnAllyMod;
		string prefix9 = "[HealOnAlly]";
		bool showBaseVal9 = flag;
		int baseVal9;
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
			baseVal9 = senseiYingYangDash.m_healOnAlly;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(healOnAllyMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyEffectInfo allyHitEffectMod = this.m_allyHitEffectMod;
		string prefix10 = "[AllyHitEffect]";
		bool showBaseVal10 = flag;
		StandardEffectInfo baseVal10;
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
			baseVal10 = senseiYingYangDash.m_allyHitEffect;
		}
		else
		{
			baseVal10 = null;
		}
		text = str10 + base.PropDesc(allyHitEffectMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyInt extraHealOnAllyForDiffTeamSecondDashMod = this.m_extraHealOnAllyForDiffTeamSecondDashMod;
		string prefix11 = "[ExtraHealOnAllyForDiffTeamSecondDash]";
		bool showBaseVal11 = flag;
		int baseVal11;
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
			baseVal11 = senseiYingYangDash.m_extraHealOnAllyForDiffTeamSecondDash;
		}
		else
		{
			baseVal11 = 0;
		}
		text = str11 + base.PropDesc(extraHealOnAllyForDiffTeamSecondDashMod, prefix11, showBaseVal11, baseVal11);
		string str12 = text;
		AbilityModPropertyInt extraHealOnAllyForLowHealthMod = this.m_extraHealOnAllyForLowHealthMod;
		string prefix12 = "[ExtraHealOnAllyForLowHealth]";
		bool showBaseVal12 = flag;
		int baseVal12;
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
			baseVal12 = senseiYingYangDash.m_extraHealOnAllyForLowHealth;
		}
		else
		{
			baseVal12 = 0;
		}
		text = str12 + base.PropDesc(extraHealOnAllyForLowHealthMod, prefix12, showBaseVal12, baseVal12);
		string str13 = text;
		AbilityModPropertyFloat allyLowHealthThreshMod = this.m_allyLowHealthThreshMod;
		string prefix13 = "[AllyLowHealthThresh]";
		bool showBaseVal13 = flag;
		float baseVal13;
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
			baseVal13 = senseiYingYangDash.m_allyLowHealthThresh;
		}
		else
		{
			baseVal13 = 0f;
		}
		text = str13 + base.PropDesc(allyLowHealthThreshMod, prefix13, showBaseVal13, baseVal13);
		string str14 = text;
		AbilityModPropertyBool reverseHealthThreshForAllyMod = this.m_reverseHealthThreshForAllyMod;
		string prefix14 = "[ReverseHealthThreshForAlly]";
		bool showBaseVal14 = flag;
		bool baseVal14;
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
			baseVal14 = senseiYingYangDash.m_reverseHealthThreshForAlly;
		}
		else
		{
			baseVal14 = false;
		}
		text = str14 + base.PropDesc(reverseHealthThreshForAllyMod, prefix14, showBaseVal14, baseVal14);
		string str15 = text;
		AbilityModPropertyInt cdrIfNoSecondDashMod = this.m_cdrIfNoSecondDashMod;
		string prefix15 = "[CdrIfNoSecondDash]";
		bool showBaseVal15 = flag;
		int baseVal15;
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
			baseVal15 = senseiYingYangDash.m_cdrIfNoSecondDash;
		}
		else
		{
			baseVal15 = 0;
		}
		return str15 + base.PropDesc(cdrIfNoSecondDashMod, prefix15, showBaseVal15, baseVal15);
	}
}
