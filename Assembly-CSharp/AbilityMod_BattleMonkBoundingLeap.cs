using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BattleMonkBoundingLeap : AbilityMod
{
	[Header("-- Bounce Mod")]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyInt m_damageAfterFirstHitMod;

	[Header("-- Bounce")]
	public AbilityModPropertyInt m_maxHitTargetsMod;

	public AbilityModPropertyBool m_bounceOffEnemyActorMod;

	public AbilityModPropertyFloat m_maxDistancePerBounceMod;

	public AbilityModPropertyFloat m_maxTotalDistanceMod;

	public AbilityModPropertyInt m_maxBouncesMod;

	[Header("-- Whether to include allies in between")]
	public AbilityModPropertyBool m_hitAlliesInBetween;

	public StandardEffectInfo m_allyHitEffect;

	[Header("-- Heal Amount If Not Damaged This Turn")]
	public AbilityModPropertyInt m_healAmountIfNotDamagedThisTurn;

	public override Type GetTargetAbilityType()
	{
		return typeof(BattleMonkBoundingLeap);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BattleMonkBoundingLeap battleMonkBoundingLeap = targetAbility as BattleMonkBoundingLeap;
		if (battleMonkBoundingLeap != null)
		{
			AbilityMod.AddToken(tokens, this.m_maxDistancePerBounceMod, "MaxDistancePerBounce", string.Empty, battleMonkBoundingLeap.m_maxDistancePerBounce, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTotalDistanceMod, "MaxTotalDistance", string.Empty, battleMonkBoundingLeap.m_maxTotalDistance, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxBouncesMod, "MaxBounces", string.Empty, battleMonkBoundingLeap.m_maxBounces, true, false);
			AbilityMod.AddToken(tokens, this.m_damageMod, "DamageAmount", string.Empty, battleMonkBoundingLeap.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_damageAfterFirstHitMod, "DamageAfterFirstHit", string.Empty, battleMonkBoundingLeap.m_damageAfterFirstHit, true, false);
			AbilityMod.AddToken(tokens, this.m_maxBouncesMod, "MaxBounces", string.Empty, battleMonkBoundingLeap.m_maxBounces, true, false);
			AbilityMod.AddToken(tokens, this.m_maxHitTargetsMod, "MaxTargetsHit", string.Empty, battleMonkBoundingLeap.m_maxTargetsHit, true, false);
			AbilityMod.AddToken_EffectInfo(tokens, this.m_allyHitEffect, "AllyHitEffectMod", null, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BattleMonkBoundingLeap battleMonkBoundingLeap = base.GetTargetAbilityOnAbilityData(abilityData) as BattleMonkBoundingLeap;
		bool flag = battleMonkBoundingLeap != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix = "[Damage]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			baseVal = battleMonkBoundingLeap.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(damageMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyInt damageAfterFirstHitMod = this.m_damageAfterFirstHitMod;
		string prefix2 = "[DamageAfterFirstHit]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			baseVal2 = battleMonkBoundingLeap.m_damageAfterFirstHit;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(damageAfterFirstHitMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt maxHitTargetsMod = this.m_maxHitTargetsMod;
		string prefix3 = "[Max Hits]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = battleMonkBoundingLeap.m_maxTargetsHit;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(maxHitTargetsMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyBool bounceOffEnemyActorMod = this.m_bounceOffEnemyActorMod;
		string prefix4 = "[Bounce Off Enemies?]";
		bool showBaseVal4 = flag;
		bool baseVal4;
		if (flag)
		{
			baseVal4 = battleMonkBoundingLeap.m_bounceOffEnemyActor;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(bounceOffEnemyActorMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyFloat maxDistancePerBounceMod = this.m_maxDistancePerBounceMod;
		string prefix5 = "[MaxDistancePerBounce]";
		bool showBaseVal5 = flag;
		float baseVal5;
		if (flag)
		{
			baseVal5 = battleMonkBoundingLeap.m_maxDistancePerBounce;
		}
		else
		{
			baseVal5 = 0f;
		}
		text = str5 + base.PropDesc(maxDistancePerBounceMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyFloat maxTotalDistanceMod = this.m_maxTotalDistanceMod;
		string prefix6 = "[MaxTotalDistance]";
		bool showBaseVal6 = flag;
		float baseVal6;
		if (flag)
		{
			baseVal6 = battleMonkBoundingLeap.m_maxTotalDistance;
		}
		else
		{
			baseVal6 = 0f;
		}
		text = str6 + base.PropDesc(maxTotalDistanceMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt maxBouncesMod = this.m_maxBouncesMod;
		string prefix7 = "[MaxBounces]";
		bool showBaseVal7 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = battleMonkBoundingLeap.m_maxBounces;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(maxBouncesMod, prefix7, showBaseVal7, baseVal7);
		text += AbilityModHelper.GetModPropertyDesc(this.m_hitAlliesInBetween, "[Hit Allies In-Between]", flag, false);
		text += AbilityModHelper.GetModEffectInfoDesc(this.m_allyHitEffect, "{ Ally Hit Effect }", string.Empty, flag, null);
		return text + base.PropDesc(this.m_healAmountIfNotDamagedThisTurn, "{ Heal Amount If Not Damaged This Turn }", flag, 0);
	}
}
