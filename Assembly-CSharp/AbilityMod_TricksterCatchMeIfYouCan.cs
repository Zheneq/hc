using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_TricksterCatchMeIfYouCan : AbilityMod
{
	[Header("-- Hit actors in path")]
	public AbilityModPropertyBool m_hitActorsInPathMod;

	public AbilityModPropertyFloat m_pathRadiusMod;

	public AbilityModPropertyFloat m_pathStartRadiusMod;

	public AbilityModPropertyFloat m_pathEndRadiusMod;

	public AbilityModPropertyBool m_penetrateLosMod;

	[Header("-- Enemy Hit")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyInt m_subsequentDamageAmountMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	[Space(10f)]
	public AbilityModPropertyBool m_useEnemyMultiHitEffectMod;

	public AbilityModPropertyEffectInfo m_enemyMultipleHitEffectMod;

	[Header("-- Ally Hit")]
	public AbilityModPropertyInt m_allyHealingAmountMod;

	public AbilityModPropertyInt m_subsequentHealingAmountMod;

	public AbilityModPropertyInt m_allyEnergyGainMod;

	public AbilityModPropertyEffectInfo m_allyHitEffectMod;

	[Space(10f)]
	public AbilityModPropertyBool m_useAllyMultiHitEffectMod;

	public AbilityModPropertyEffectInfo m_allyMultipleHitEffectMod;

	[Space(10f)]
	public AbilityModPropertyInt m_selfHealingAmountMod;

	public AbilityModPropertyEffectInfo m_selfHitEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(TricksterCatchMeIfYouCan);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		TricksterCatchMeIfYouCan tricksterCatchMeIfYouCan = targetAbility as TricksterCatchMeIfYouCan;
		if (tricksterCatchMeIfYouCan != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_TricksterCatchMeIfYouCan.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_pathRadiusMod, "PathRadius", string.Empty, tricksterCatchMeIfYouCan.m_pathRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_pathStartRadiusMod, "PathStartRadius", string.Empty, tricksterCatchMeIfYouCan.m_pathStartRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_pathEndRadiusMod, "PathEndRadius", string.Empty, tricksterCatchMeIfYouCan.m_pathEndRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, tricksterCatchMeIfYouCan.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_subsequentDamageAmountMod, "SubsequentDamageAmount", string.Empty, tricksterCatchMeIfYouCan.m_subsequentDamageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", tricksterCatchMeIfYouCan.m_enemyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyMultipleHitEffectMod, "EnemyMultipleHitEffect", tricksterCatchMeIfYouCan.m_enemyMultipleHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_allyHealingAmountMod, "AllyHealingAmount", string.Empty, tricksterCatchMeIfYouCan.m_allyHealingAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_subsequentHealingAmountMod, "SubsequentHealingAmount", string.Empty, tricksterCatchMeIfYouCan.m_subsequentHealingAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_allyEnergyGainMod, "AllyEnergyGain", string.Empty, tricksterCatchMeIfYouCan.m_allyEnergyGain, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyHitEffectMod, "AllyHitEffect", tricksterCatchMeIfYouCan.m_allyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyMultipleHitEffectMod, "AllyMultipleHitEffect", tricksterCatchMeIfYouCan.m_allyMultipleHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_selfHealingAmountMod, "SelfHealingAmount", string.Empty, tricksterCatchMeIfYouCan.m_selfHealingAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_selfHitEffectMod, "SelfHitEffect", tricksterCatchMeIfYouCan.m_selfHitEffect, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TricksterCatchMeIfYouCan tricksterCatchMeIfYouCan = base.GetTargetAbilityOnAbilityData(abilityData) as TricksterCatchMeIfYouCan;
		bool flag = tricksterCatchMeIfYouCan != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyBool hitActorsInPathMod = this.m_hitActorsInPathMod;
		string prefix = "[HitActorsInPath]";
		bool showBaseVal = flag;
		bool baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_TricksterCatchMeIfYouCan.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = tricksterCatchMeIfYouCan.m_hitActorsInPath;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(hitActorsInPathMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_pathRadiusMod, "[PathRadius]", flag, (!flag) ? 0f : tricksterCatchMeIfYouCan.m_pathRadius);
		string str2 = text;
		AbilityModPropertyFloat pathStartRadiusMod = this.m_pathStartRadiusMod;
		string prefix2 = "[PathStartRadius]";
		bool showBaseVal2 = flag;
		float baseVal2;
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
			baseVal2 = tricksterCatchMeIfYouCan.m_pathStartRadius;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(pathStartRadiusMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat pathEndRadiusMod = this.m_pathEndRadiusMod;
		string prefix3 = "[PathEndRadius]";
		bool showBaseVal3 = flag;
		float baseVal3;
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
			baseVal3 = tricksterCatchMeIfYouCan.m_pathEndRadius;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(pathEndRadiusMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyBool penetrateLosMod = this.m_penetrateLosMod;
		string prefix4 = "[PenetrateLos]";
		bool showBaseVal4 = flag;
		bool baseVal4;
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
			baseVal4 = tricksterCatchMeIfYouCan.m_penetrateLos;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + base.PropDesc(penetrateLosMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt damageAmountMod = this.m_damageAmountMod;
		string prefix5 = "[DamageAmount]";
		bool showBaseVal5 = flag;
		int baseVal5;
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
			baseVal5 = tricksterCatchMeIfYouCan.m_damageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(damageAmountMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt subsequentDamageAmountMod = this.m_subsequentDamageAmountMod;
		string prefix6 = "[SubsequentDamageAmount]";
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
			baseVal6 = tricksterCatchMeIfYouCan.m_subsequentDamageAmount;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(subsequentDamageAmountMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyEffectInfo enemyHitEffectMod = this.m_enemyHitEffectMod;
		string prefix7 = "[EnemyHitEffect]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
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
			baseVal7 = tricksterCatchMeIfYouCan.m_enemyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + base.PropDesc(enemyHitEffectMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyBool useEnemyMultiHitEffectMod = this.m_useEnemyMultiHitEffectMod;
		string prefix8 = "[UseEnemyMultiHitEffect]";
		bool showBaseVal8 = flag;
		bool baseVal8;
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
			baseVal8 = tricksterCatchMeIfYouCan.m_useEnemyMultiHitEffect;
		}
		else
		{
			baseVal8 = false;
		}
		text = str8 + base.PropDesc(useEnemyMultiHitEffectMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyEffectInfo enemyMultipleHitEffectMod = this.m_enemyMultipleHitEffectMod;
		string prefix9 = "[EnemyMultipleHitEffect]";
		bool showBaseVal9 = flag;
		StandardEffectInfo baseVal9;
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
			baseVal9 = tricksterCatchMeIfYouCan.m_enemyMultipleHitEffect;
		}
		else
		{
			baseVal9 = null;
		}
		text = str9 + base.PropDesc(enemyMultipleHitEffectMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyInt allyHealingAmountMod = this.m_allyHealingAmountMod;
		string prefix10 = "[AllyHealingAmount]";
		bool showBaseVal10 = flag;
		int baseVal10;
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
			baseVal10 = tricksterCatchMeIfYouCan.m_allyHealingAmount;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + base.PropDesc(allyHealingAmountMod, prefix10, showBaseVal10, baseVal10);
		text += base.PropDesc(this.m_allyEnergyGainMod, "[AllyEnergyGain]", flag, (!flag) ? 0 : tricksterCatchMeIfYouCan.m_allyEnergyGain);
		string str11 = text;
		AbilityModPropertyInt subsequentHealingAmountMod = this.m_subsequentHealingAmountMod;
		string prefix11 = "[SubsequentHealingAmount]";
		bool showBaseVal11 = flag;
		int baseVal11;
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
			baseVal11 = tricksterCatchMeIfYouCan.m_subsequentHealingAmount;
		}
		else
		{
			baseVal11 = 0;
		}
		text = str11 + base.PropDesc(subsequentHealingAmountMod, prefix11, showBaseVal11, baseVal11);
		string str12 = text;
		AbilityModPropertyEffectInfo allyHitEffectMod = this.m_allyHitEffectMod;
		string prefix12 = "[AllyHitEffect]";
		bool showBaseVal12 = flag;
		StandardEffectInfo baseVal12;
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
			baseVal12 = tricksterCatchMeIfYouCan.m_allyHitEffect;
		}
		else
		{
			baseVal12 = null;
		}
		text = str12 + base.PropDesc(allyHitEffectMod, prefix12, showBaseVal12, baseVal12);
		text += base.PropDesc(this.m_useAllyMultiHitEffectMod, "[UseAllyMultiHitEffect]", flag, flag && tricksterCatchMeIfYouCan.m_useAllyMultiHitEffect);
		string str13 = text;
		AbilityModPropertyEffectInfo allyMultipleHitEffectMod = this.m_allyMultipleHitEffectMod;
		string prefix13 = "[AllyMultipleHitEffect]";
		bool showBaseVal13 = flag;
		StandardEffectInfo baseVal13;
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
			baseVal13 = tricksterCatchMeIfYouCan.m_allyMultipleHitEffect;
		}
		else
		{
			baseVal13 = null;
		}
		text = str13 + base.PropDesc(allyMultipleHitEffectMod, prefix13, showBaseVal13, baseVal13);
		string str14 = text;
		AbilityModPropertyInt selfHealingAmountMod = this.m_selfHealingAmountMod;
		string prefix14 = "[SelfHealingAmount]";
		bool showBaseVal14 = flag;
		int baseVal14;
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
			baseVal14 = tricksterCatchMeIfYouCan.m_selfHealingAmount;
		}
		else
		{
			baseVal14 = 0;
		}
		text = str14 + base.PropDesc(selfHealingAmountMod, prefix14, showBaseVal14, baseVal14);
		string str15 = text;
		AbilityModPropertyEffectInfo selfHitEffectMod = this.m_selfHitEffectMod;
		string prefix15 = "[SelfHitEffect]";
		bool showBaseVal15 = flag;
		StandardEffectInfo baseVal15;
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
			baseVal15 = tricksterCatchMeIfYouCan.m_selfHitEffect;
		}
		else
		{
			baseVal15 = null;
		}
		return str15 + base.PropDesc(selfHitEffectMod, prefix15, showBaseVal15, baseVal15);
	}
}
