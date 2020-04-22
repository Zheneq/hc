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
		if (!(senseiYingYangDash != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_secondCastTurnsMod, "SecondCastTurns", string.Empty, senseiYingYangDash.m_secondCastTurns);
			AbilityMod.AddToken(tokens, m_damageMod, "Damage", string.Empty, senseiYingYangDash.m_damage);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", senseiYingYangDash.m_enemyHitEffect);
			AbilityMod.AddToken(tokens, m_extraDamageForDiffTeamSecondDashMod, "ExtraDamageForDiffTeamSecondDash", string.Empty, senseiYingYangDash.m_extraDamageForDiffTeamSecondDash);
			AbilityMod.AddToken(tokens, m_extraDamageForLowHealthMod, "ExtraDamageForLowHealth", string.Empty, senseiYingYangDash.m_extraDamageForLowHealth);
			AbilityMod.AddToken(tokens, m_enemyLowHealthThreshMod, "EnemyLowHealthThresh", string.Empty, senseiYingYangDash.m_enemyLowHealthThresh, true, false, true);
			AbilityMod.AddToken(tokens, m_healOnAllyMod, "HealOnAlly", string.Empty, senseiYingYangDash.m_healOnAlly);
			AbilityMod.AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", senseiYingYangDash.m_allyHitEffect);
			AbilityMod.AddToken(tokens, m_extraHealOnAllyForDiffTeamSecondDashMod, "ExtraHealOnAllyForDiffTeamSecondDash", string.Empty, senseiYingYangDash.m_extraHealOnAllyForDiffTeamSecondDash);
			AbilityMod.AddToken(tokens, m_extraHealOnAllyForLowHealthMod, "ExtraHealOnAllyForLowHealth", string.Empty, senseiYingYangDash.m_extraHealOnAllyForLowHealth);
			AbilityMod.AddToken(tokens, m_allyLowHealthThreshMod, "AllyLowHealthThresh", string.Empty, senseiYingYangDash.m_allyLowHealthThresh, true, false, true);
			AbilityMod.AddToken(tokens, m_cdrIfNoSecondDashMod, "CdrIfNoSecondDash", string.Empty, senseiYingYangDash.m_cdrIfNoSecondDash);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SenseiYingYangDash senseiYingYangDash = GetTargetAbilityOnAbilityData(abilityData) as SenseiYingYangDash;
		bool flag = senseiYingYangDash != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyShape chooseDestShapeMod = m_chooseDestShapeMod;
		int baseVal;
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
			baseVal = (int)senseiYingYangDash.m_chooseDestShape;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(chooseDestShapeMod, "[ChooseDestShape]", flag, (AbilityAreaShape)baseVal);
		string str2 = empty;
		AbilityModPropertyInt secondCastTurnsMod = m_secondCastTurnsMod;
		int baseVal2;
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
			baseVal2 = senseiYingYangDash.m_secondCastTurns;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(secondCastTurnsMod, "[SecondCastTurns]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyBool secondDashAllowBothTeamsMod = m_secondDashAllowBothTeamsMod;
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
			baseVal3 = (senseiYingYangDash.m_secondDashAllowBothTeams ? 1 : 0);
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(secondDashAllowBothTeamsMod, "[SecondDashAllowBothTeams]", flag, (byte)baseVal3 != 0);
		string str4 = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal4;
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
			baseVal4 = senseiYingYangDash.m_damage;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(damageMod, "[Damage]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectMod = m_enemyHitEffectMod;
		object baseVal5;
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
			baseVal5 = senseiYingYangDash.m_enemyHitEffect;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + PropDesc(enemyHitEffectMod, "[EnemyHitEffect]", flag, (StandardEffectInfo)baseVal5);
		string str6 = empty;
		AbilityModPropertyInt extraDamageForDiffTeamSecondDashMod = m_extraDamageForDiffTeamSecondDashMod;
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
			baseVal6 = senseiYingYangDash.m_extraDamageForDiffTeamSecondDash;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(extraDamageForDiffTeamSecondDashMod, "[ExtraDamageForDiffTeamSecondDash]", flag, baseVal6);
		empty += PropDesc(m_extraDamageForLowHealthMod, "[ExtraDamageForLowHealth]", flag, flag ? senseiYingYangDash.m_extraDamageForLowHealth : 0);
		string str7 = empty;
		AbilityModPropertyFloat enemyLowHealthThreshMod = m_enemyLowHealthThreshMod;
		float baseVal7;
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
			baseVal7 = senseiYingYangDash.m_enemyLowHealthThresh;
		}
		else
		{
			baseVal7 = 0f;
		}
		empty = str7 + PropDesc(enemyLowHealthThreshMod, "[EnemyLowHealthThresh]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyBool reverseHealthThreshForEnemyMod = m_reverseHealthThreshForEnemyMod;
		int baseVal8;
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
			baseVal8 = (senseiYingYangDash.m_reverseHealthThreshForEnemy ? 1 : 0);
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(reverseHealthThreshForEnemyMod, "[ReverseHealthThreshForEnemy]", flag, (byte)baseVal8 != 0);
		string str9 = empty;
		AbilityModPropertyInt healOnAllyMod = m_healOnAllyMod;
		int baseVal9;
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
			baseVal9 = senseiYingYangDash.m_healOnAlly;
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(healOnAllyMod, "[HealOnAlly]", flag, baseVal9);
		string str10 = empty;
		AbilityModPropertyEffectInfo allyHitEffectMod = m_allyHitEffectMod;
		object baseVal10;
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
			baseVal10 = senseiYingYangDash.m_allyHitEffect;
		}
		else
		{
			baseVal10 = null;
		}
		empty = str10 + PropDesc(allyHitEffectMod, "[AllyHitEffect]", flag, (StandardEffectInfo)baseVal10);
		string str11 = empty;
		AbilityModPropertyInt extraHealOnAllyForDiffTeamSecondDashMod = m_extraHealOnAllyForDiffTeamSecondDashMod;
		int baseVal11;
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
			baseVal11 = senseiYingYangDash.m_extraHealOnAllyForDiffTeamSecondDash;
		}
		else
		{
			baseVal11 = 0;
		}
		empty = str11 + PropDesc(extraHealOnAllyForDiffTeamSecondDashMod, "[ExtraHealOnAllyForDiffTeamSecondDash]", flag, baseVal11);
		string str12 = empty;
		AbilityModPropertyInt extraHealOnAllyForLowHealthMod = m_extraHealOnAllyForLowHealthMod;
		int baseVal12;
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
			baseVal12 = senseiYingYangDash.m_extraHealOnAllyForLowHealth;
		}
		else
		{
			baseVal12 = 0;
		}
		empty = str12 + PropDesc(extraHealOnAllyForLowHealthMod, "[ExtraHealOnAllyForLowHealth]", flag, baseVal12);
		string str13 = empty;
		AbilityModPropertyFloat allyLowHealthThreshMod = m_allyLowHealthThreshMod;
		float baseVal13;
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
			baseVal13 = senseiYingYangDash.m_allyLowHealthThresh;
		}
		else
		{
			baseVal13 = 0f;
		}
		empty = str13 + PropDesc(allyLowHealthThreshMod, "[AllyLowHealthThresh]", flag, baseVal13);
		string str14 = empty;
		AbilityModPropertyBool reverseHealthThreshForAllyMod = m_reverseHealthThreshForAllyMod;
		int baseVal14;
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
			baseVal14 = (senseiYingYangDash.m_reverseHealthThreshForAlly ? 1 : 0);
		}
		else
		{
			baseVal14 = 0;
		}
		empty = str14 + PropDesc(reverseHealthThreshForAllyMod, "[ReverseHealthThreshForAlly]", flag, (byte)baseVal14 != 0);
		string str15 = empty;
		AbilityModPropertyInt cdrIfNoSecondDashMod = m_cdrIfNoSecondDashMod;
		int baseVal15;
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
			baseVal15 = senseiYingYangDash.m_cdrIfNoSecondDash;
		}
		else
		{
			baseVal15 = 0;
		}
		return str15 + PropDesc(cdrIfNoSecondDashMod, "[CdrIfNoSecondDash]", flag, baseVal15);
	}
}
