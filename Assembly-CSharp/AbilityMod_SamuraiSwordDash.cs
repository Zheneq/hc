using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SamuraiSwordDash : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_damageRadiusMod;

	public AbilityModPropertyFloat m_damageRadiusAtStartMod;

	public AbilityModPropertyFloat m_damageRadiusAtEndMod;

	public AbilityModPropertyBool m_penetrateLineOfSightMod;

	public AbilityModPropertyBool m_canMoveAfterEvadeMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	[Header("-- How many targets can be damaged")]
	public AbilityModPropertyInt m_maxDamageTargetsMod;

	[Header("-- Enemy Hits, Dash Phase")]
	public AbilityModPropertyInt m_dashDamageMod;

	public AbilityModPropertyInt m_dashLessDamagePerTargetMod;

	public AbilityModPropertyEffectInfo m_dashEnemyHitEffectMod;

	[Header("-- Effect on Self")]
	public AbilityModPropertyEffectInfo m_dashSelfHitEffectMod;

	[Header("-- Mark data")]
	public AbilityModPropertyEffectInfo m_markEffectInfoMod;

	[Header("-- Energy Refund if target dashed away")]
	public AbilityModPropertyInt m_energyRefundIfTargetDashedAwayMod;

	[Separator("For Chain Ability (Knockback phase)", true)]
	public AbilityModPropertyInt m_knockbackDamageMod;

	public AbilityModPropertyInt m_knockbackLessDamagePerTargetMod;

	public AbilityModPropertyFloat m_knockbackExtraDamageFromDamageTakenMultMod;

	[Space(10f)]
	public AbilityModPropertyInt m_knockbackExtraDamageByDistMod;

	public AbilityModPropertyInt m_knockbackExtraDamageChangePerDistMod;

	[Header("-- Knockback")]
	public AbilityModPropertyFloat m_knockbackDistMod;

	public AbilityModPropertyKnockbackType m_knockbackTypeMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SamuraiSwordDash);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SamuraiSwordDash samuraiSwordDash = targetAbility as SamuraiSwordDash;
		if (samuraiSwordDash != null)
		{
			AbilityMod.AddToken(tokens, this.m_damageRadiusMod, "DamageRadius", string.Empty, samuraiSwordDash.m_damageRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageRadiusAtStartMod, "DamageRadiusAtStart", string.Empty, samuraiSwordDash.m_damageRadiusAtStart, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageRadiusAtEndMod, "DamageRadiusAtEnd", string.Empty, samuraiSwordDash.m_damageRadiusAtEnd, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargets", string.Empty, samuraiSwordDash.m_maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_maxDamageTargetsMod, "MaxDamageTargets", string.Empty, samuraiSwordDash.m_maxDamageTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_dashDamageMod, "DamageAmount", string.Empty, samuraiSwordDash.m_dashDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_dashLessDamagePerTargetMod, "LessDamagePerTarget", string.Empty, samuraiSwordDash.m_dashLessDamagePerTarget, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_dashEnemyHitEffectMod, "DashEnemyHitEffect", samuraiSwordDash.m_dashEnemyHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_dashSelfHitEffectMod, "DashSelfHitEffect", samuraiSwordDash.m_dashSelfHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_markEffectInfoMod, "MarkEffectInfo", samuraiSwordDash.m_markEffectInfo, true);
			AbilityMod.AddToken(tokens, this.m_energyRefundIfTargetDashedAwayMod, "EnergyRefundIfTargetDashedAway", string.Empty, samuraiSwordDash.m_energyRefundIfTargetDashedAway, true, false);
			AbilityMod.AddToken(tokens, this.m_knockbackDamageMod, "KnockbackDamage", string.Empty, samuraiSwordDash.m_knockbackDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_knockbackLessDamagePerTargetMod, "KnockbackLessDamagePerTarget", string.Empty, samuraiSwordDash.m_knockbackLessDamagePerTarget, true, false);
			AbilityMod.AddToken(tokens, this.m_knockbackExtraDamageFromDamageTakenMultMod, "KnockbackExtraDamageFromDamageTakenMult", string.Empty, samuraiSwordDash.m_knockbackExtraDamageFromDamageTakenMult, true, false, true);
			AbilityMod.AddToken(tokens, this.m_knockbackExtraDamageByDistMod, "KnockbackExtraDamageByDist", string.Empty, samuraiSwordDash.m_knockbackExtraDamageByDist, true, false);
			AbilityMod.AddToken(tokens, this.m_knockbackExtraDamageChangePerDistMod, "KnockbackExtraDamageChangePerDist", string.Empty, samuraiSwordDash.m_knockbackExtraDamageChangePerDist, true, false);
			AbilityMod.AddToken(tokens, this.m_knockbackDistMod, "KnockbackDist", string.Empty, samuraiSwordDash.m_knockbackDist, true, false, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SamuraiSwordDash samuraiSwordDash = base.GetTargetAbilityOnAbilityData(abilityData) as SamuraiSwordDash;
		bool flag = samuraiSwordDash != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat damageRadiusMod = this.m_damageRadiusMod;
		string prefix = "[DamageRadius]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = samuraiSwordDash.m_damageRadius;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(damageRadiusMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat damageRadiusAtStartMod = this.m_damageRadiusAtStartMod;
		string prefix2 = "[DamageRadiusAtStart]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = samuraiSwordDash.m_damageRadiusAtStart;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(damageRadiusAtStartMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat damageRadiusAtEndMod = this.m_damageRadiusAtEndMod;
		string prefix3 = "[DamageRadiusAtEnd]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = samuraiSwordDash.m_damageRadiusAtEnd;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(damageRadiusAtEndMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyBool penetrateLineOfSightMod = this.m_penetrateLineOfSightMod;
		string prefix4 = "[PenetrateLineOfSight]";
		bool showBaseVal4 = flag;
		bool baseVal4;
		if (flag)
		{
			baseVal4 = samuraiSwordDash.m_penetrateLineOfSight;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + base.PropDesc(penetrateLineOfSightMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool canMoveAfterEvadeMod = this.m_canMoveAfterEvadeMod;
		string prefix5 = "[CanMoveAfterEvade]";
		bool showBaseVal5 = flag;
		bool baseVal5;
		if (flag)
		{
			baseVal5 = samuraiSwordDash.m_canMoveAfterEvade;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(canMoveAfterEvadeMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt maxTargetsMod = this.m_maxTargetsMod;
		string prefix6 = "[MaxTargets]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			baseVal6 = samuraiSwordDash.m_maxTargets;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(maxTargetsMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyInt maxDamageTargetsMod = this.m_maxDamageTargetsMod;
		string prefix7 = "[MaxDamageTargets]";
		bool showBaseVal7 = flag;
		int baseVal7;
		if (flag)
		{
			baseVal7 = samuraiSwordDash.m_maxDamageTargets;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str7 + base.PropDesc(maxDamageTargetsMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyInt dashDamageMod = this.m_dashDamageMod;
		string prefix8 = "[DamageAmount]";
		bool showBaseVal8 = flag;
		int baseVal8;
		if (flag)
		{
			baseVal8 = samuraiSwordDash.m_dashDamage;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str8 + base.PropDesc(dashDamageMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyInt dashLessDamagePerTargetMod = this.m_dashLessDamagePerTargetMod;
		string prefix9 = "[LessDamagePerTarget]";
		bool showBaseVal9 = flag;
		int baseVal9;
		if (flag)
		{
			baseVal9 = samuraiSwordDash.m_dashLessDamagePerTarget;
		}
		else
		{
			baseVal9 = 0;
		}
		text = str9 + base.PropDesc(dashLessDamagePerTargetMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyEffectInfo dashEnemyHitEffectMod = this.m_dashEnemyHitEffectMod;
		string prefix10 = "[DashEnemyHitEffect]";
		bool showBaseVal10 = flag;
		StandardEffectInfo baseVal10;
		if (flag)
		{
			baseVal10 = samuraiSwordDash.m_dashEnemyHitEffect;
		}
		else
		{
			baseVal10 = null;
		}
		text = str10 + base.PropDesc(dashEnemyHitEffectMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyEffectInfo dashSelfHitEffectMod = this.m_dashSelfHitEffectMod;
		string prefix11 = "[DashSelfHitEffect]";
		bool showBaseVal11 = flag;
		StandardEffectInfo baseVal11;
		if (flag)
		{
			baseVal11 = samuraiSwordDash.m_dashSelfHitEffect;
		}
		else
		{
			baseVal11 = null;
		}
		text = str11 + base.PropDesc(dashSelfHitEffectMod, prefix11, showBaseVal11, baseVal11);
		string str12 = text;
		AbilityModPropertyEffectInfo markEffectInfoMod = this.m_markEffectInfoMod;
		string prefix12 = "[MarkEffectInfo]";
		bool showBaseVal12 = flag;
		StandardEffectInfo baseVal12;
		if (flag)
		{
			baseVal12 = samuraiSwordDash.m_markEffectInfo;
		}
		else
		{
			baseVal12 = null;
		}
		text = str12 + base.PropDesc(markEffectInfoMod, prefix12, showBaseVal12, baseVal12);
		text += base.PropDesc(this.m_energyRefundIfTargetDashedAwayMod, "[EnergyRefundIfTargetDashedAway]", flag, (!flag) ? 0 : samuraiSwordDash.m_energyRefundIfTargetDashedAway);
		text += base.PropDesc(this.m_knockbackDamageMod, "[KnockbackDamage]", flag, (!flag) ? 0 : samuraiSwordDash.m_knockbackDamage);
		string str13 = text;
		AbilityModPropertyInt knockbackLessDamagePerTargetMod = this.m_knockbackLessDamagePerTargetMod;
		string prefix13 = "[KnockbackLessDamagePerTarget]";
		bool showBaseVal13 = flag;
		int baseVal13;
		if (flag)
		{
			baseVal13 = samuraiSwordDash.m_knockbackLessDamagePerTarget;
		}
		else
		{
			baseVal13 = 0;
		}
		text = str13 + base.PropDesc(knockbackLessDamagePerTargetMod, prefix13, showBaseVal13, baseVal13);
		string str14 = text;
		AbilityModPropertyFloat knockbackExtraDamageFromDamageTakenMultMod = this.m_knockbackExtraDamageFromDamageTakenMultMod;
		string prefix14 = "[KnockbackExtraDamageFromDamageTakenMult]";
		bool showBaseVal14 = flag;
		float baseVal14;
		if (flag)
		{
			baseVal14 = samuraiSwordDash.m_knockbackExtraDamageFromDamageTakenMult;
		}
		else
		{
			baseVal14 = 0f;
		}
		text = str14 + base.PropDesc(knockbackExtraDamageFromDamageTakenMultMod, prefix14, showBaseVal14, baseVal14);
		string str15 = text;
		AbilityModPropertyInt knockbackExtraDamageByDistMod = this.m_knockbackExtraDamageByDistMod;
		string prefix15 = "[KnockbackExtraDamageByDist]";
		bool showBaseVal15 = flag;
		int baseVal15;
		if (flag)
		{
			baseVal15 = samuraiSwordDash.m_knockbackExtraDamageByDist;
		}
		else
		{
			baseVal15 = 0;
		}
		text = str15 + base.PropDesc(knockbackExtraDamageByDistMod, prefix15, showBaseVal15, baseVal15);
		text += base.PropDesc(this.m_knockbackExtraDamageChangePerDistMod, "[KnockbackExtraDamageChangePerDist]", flag, (!flag) ? 0 : samuraiSwordDash.m_knockbackExtraDamageChangePerDist);
		string str16 = text;
		AbilityModPropertyFloat knockbackDistMod = this.m_knockbackDistMod;
		string prefix16 = "[KnockbackDist]";
		bool showBaseVal16 = flag;
		float baseVal16;
		if (flag)
		{
			baseVal16 = samuraiSwordDash.m_knockbackDist;
		}
		else
		{
			baseVal16 = 0f;
		}
		text = str16 + base.PropDesc(knockbackDistMod, prefix16, showBaseVal16, baseVal16);
		return text + base.PropDesc(this.m_knockbackTypeMod, "[KnockbackType]", flag, (!flag) ? KnockbackType.AwayFromSource : samuraiSwordDash.m_knockbackType);
	}
}
