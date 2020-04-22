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
		if (!(tricksterCatchMeIfYouCan != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_pathRadiusMod, "PathRadius", string.Empty, tricksterCatchMeIfYouCan.m_pathRadius);
			AbilityMod.AddToken(tokens, m_pathStartRadiusMod, "PathStartRadius", string.Empty, tricksterCatchMeIfYouCan.m_pathStartRadius);
			AbilityMod.AddToken(tokens, m_pathEndRadiusMod, "PathEndRadius", string.Empty, tricksterCatchMeIfYouCan.m_pathEndRadius);
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, tricksterCatchMeIfYouCan.m_damageAmount);
			AbilityMod.AddToken(tokens, m_subsequentDamageAmountMod, "SubsequentDamageAmount", string.Empty, tricksterCatchMeIfYouCan.m_subsequentDamageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", tricksterCatchMeIfYouCan.m_enemyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyMultipleHitEffectMod, "EnemyMultipleHitEffect", tricksterCatchMeIfYouCan.m_enemyMultipleHitEffect);
			AbilityMod.AddToken(tokens, m_allyHealingAmountMod, "AllyHealingAmount", string.Empty, tricksterCatchMeIfYouCan.m_allyHealingAmount);
			AbilityMod.AddToken(tokens, m_subsequentHealingAmountMod, "SubsequentHealingAmount", string.Empty, tricksterCatchMeIfYouCan.m_subsequentHealingAmount);
			AbilityMod.AddToken(tokens, m_allyEnergyGainMod, "AllyEnergyGain", string.Empty, tricksterCatchMeIfYouCan.m_allyEnergyGain);
			AbilityMod.AddToken_EffectMod(tokens, m_allyHitEffectMod, "AllyHitEffect", tricksterCatchMeIfYouCan.m_allyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_allyMultipleHitEffectMod, "AllyMultipleHitEffect", tricksterCatchMeIfYouCan.m_allyMultipleHitEffect);
			AbilityMod.AddToken(tokens, m_selfHealingAmountMod, "SelfHealingAmount", string.Empty, tricksterCatchMeIfYouCan.m_selfHealingAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_selfHitEffectMod, "SelfHitEffect", tricksterCatchMeIfYouCan.m_selfHitEffect);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		TricksterCatchMeIfYouCan tricksterCatchMeIfYouCan = GetTargetAbilityOnAbilityData(abilityData) as TricksterCatchMeIfYouCan;
		bool flag = tricksterCatchMeIfYouCan != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyBool hitActorsInPathMod = m_hitActorsInPathMod;
		int baseVal;
		if (flag)
		{
			baseVal = (tricksterCatchMeIfYouCan.m_hitActorsInPath ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(hitActorsInPathMod, "[HitActorsInPath]", flag, (byte)baseVal != 0);
		empty += PropDesc(m_pathRadiusMod, "[PathRadius]", flag, (!flag) ? 0f : tricksterCatchMeIfYouCan.m_pathRadius);
		string str2 = empty;
		AbilityModPropertyFloat pathStartRadiusMod = m_pathStartRadiusMod;
		float baseVal2;
		if (flag)
		{
			baseVal2 = tricksterCatchMeIfYouCan.m_pathStartRadius;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(pathStartRadiusMod, "[PathStartRadius]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat pathEndRadiusMod = m_pathEndRadiusMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = tricksterCatchMeIfYouCan.m_pathEndRadius;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(pathEndRadiusMod, "[PathEndRadius]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyBool penetrateLosMod = m_penetrateLosMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = (tricksterCatchMeIfYouCan.m_penetrateLos ? 1 : 0);
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(penetrateLosMod, "[PenetrateLos]", flag, (byte)baseVal4 != 0);
		string str5 = empty;
		AbilityModPropertyInt damageAmountMod = m_damageAmountMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = tricksterCatchMeIfYouCan.m_damageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(damageAmountMod, "[DamageAmount]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyInt subsequentDamageAmountMod = m_subsequentDamageAmountMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = tricksterCatchMeIfYouCan.m_subsequentDamageAmount;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(subsequentDamageAmountMod, "[SubsequentDamageAmount]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectMod = m_enemyHitEffectMod;
		object baseVal7;
		if (flag)
		{
			baseVal7 = tricksterCatchMeIfYouCan.m_enemyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + PropDesc(enemyHitEffectMod, "[EnemyHitEffect]", flag, (StandardEffectInfo)baseVal7);
		string str8 = empty;
		AbilityModPropertyBool useEnemyMultiHitEffectMod = m_useEnemyMultiHitEffectMod;
		int baseVal8;
		if (flag)
		{
			baseVal8 = (tricksterCatchMeIfYouCan.m_useEnemyMultiHitEffect ? 1 : 0);
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(useEnemyMultiHitEffectMod, "[UseEnemyMultiHitEffect]", flag, (byte)baseVal8 != 0);
		string str9 = empty;
		AbilityModPropertyEffectInfo enemyMultipleHitEffectMod = m_enemyMultipleHitEffectMod;
		object baseVal9;
		if (flag)
		{
			baseVal9 = tricksterCatchMeIfYouCan.m_enemyMultipleHitEffect;
		}
		else
		{
			baseVal9 = null;
		}
		empty = str9 + PropDesc(enemyMultipleHitEffectMod, "[EnemyMultipleHitEffect]", flag, (StandardEffectInfo)baseVal9);
		string str10 = empty;
		AbilityModPropertyInt allyHealingAmountMod = m_allyHealingAmountMod;
		int baseVal10;
		if (flag)
		{
			baseVal10 = tricksterCatchMeIfYouCan.m_allyHealingAmount;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(allyHealingAmountMod, "[AllyHealingAmount]", flag, baseVal10);
		empty += PropDesc(m_allyEnergyGainMod, "[AllyEnergyGain]", flag, flag ? tricksterCatchMeIfYouCan.m_allyEnergyGain : 0);
		string str11 = empty;
		AbilityModPropertyInt subsequentHealingAmountMod = m_subsequentHealingAmountMod;
		int baseVal11;
		if (flag)
		{
			baseVal11 = tricksterCatchMeIfYouCan.m_subsequentHealingAmount;
		}
		else
		{
			baseVal11 = 0;
		}
		empty = str11 + PropDesc(subsequentHealingAmountMod, "[SubsequentHealingAmount]", flag, baseVal11);
		string str12 = empty;
		AbilityModPropertyEffectInfo allyHitEffectMod = m_allyHitEffectMod;
		object baseVal12;
		if (flag)
		{
			baseVal12 = tricksterCatchMeIfYouCan.m_allyHitEffect;
		}
		else
		{
			baseVal12 = null;
		}
		empty = str12 + PropDesc(allyHitEffectMod, "[AllyHitEffect]", flag, (StandardEffectInfo)baseVal12);
		empty += PropDesc(m_useAllyMultiHitEffectMod, "[UseAllyMultiHitEffect]", flag, flag && tricksterCatchMeIfYouCan.m_useAllyMultiHitEffect);
		string str13 = empty;
		AbilityModPropertyEffectInfo allyMultipleHitEffectMod = m_allyMultipleHitEffectMod;
		object baseVal13;
		if (flag)
		{
			baseVal13 = tricksterCatchMeIfYouCan.m_allyMultipleHitEffect;
		}
		else
		{
			baseVal13 = null;
		}
		empty = str13 + PropDesc(allyMultipleHitEffectMod, "[AllyMultipleHitEffect]", flag, (StandardEffectInfo)baseVal13);
		string str14 = empty;
		AbilityModPropertyInt selfHealingAmountMod = m_selfHealingAmountMod;
		int baseVal14;
		if (flag)
		{
			baseVal14 = tricksterCatchMeIfYouCan.m_selfHealingAmount;
		}
		else
		{
			baseVal14 = 0;
		}
		empty = str14 + PropDesc(selfHealingAmountMod, "[SelfHealingAmount]", flag, baseVal14);
		string str15 = empty;
		AbilityModPropertyEffectInfo selfHitEffectMod = m_selfHitEffectMod;
		object baseVal15;
		if (flag)
		{
			baseVal15 = tricksterCatchMeIfYouCan.m_selfHitEffect;
		}
		else
		{
			baseVal15 = null;
		}
		return str15 + PropDesc(selfHitEffectMod, "[SelfHitEffect]", flag, (StandardEffectInfo)baseVal15);
	}
}
