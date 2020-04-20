using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_MantaConeDirtyFighting : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_coneRangeMod;

	public AbilityModPropertyFloat m_coneWidthMod;

	public AbilityModPropertyBool m_penetrateLoSMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	public AbilityModPropertyFloat m_coneBackwardOffsetMod;

	[Header("-- Hit Damage/Effects")]
	public AbilityModPropertyInt m_onCastDamageAmountMod;

	public AbilityModPropertyEffectData m_dirtyFightingEffectDataMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectDataMod;

	public AbilityModPropertyEffectInfo m_effectOnTargetFromExplosionMod;

	public AbilityModPropertyEffectInfo m_effectOnTargetWhenExpiresWithoutExplosionMod;

	[Header("-- On Reaction Hit/Explosion Triggered")]
	public AbilityModPropertyInt m_effectExplosionDamageMod;

	public AbilityModPropertyBool m_explodeOnlyFromSelfDamageMod;

	public AbilityModPropertyInt m_techPointGainPerExplosionMod;

	public AbilityModPropertyInt m_healPerExplosionMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(MantaConeDirtyFighting);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		MantaConeDirtyFighting mantaConeDirtyFighting = targetAbility as MantaConeDirtyFighting;
		if (mantaConeDirtyFighting != null)
		{
			AbilityMod.AddToken(tokens, this.m_coneRangeMod, "ConeRange", string.Empty, mantaConeDirtyFighting.m_coneRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_coneWidthMod, "ConeWidth", string.Empty, mantaConeDirtyFighting.m_coneWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargets", string.Empty, mantaConeDirtyFighting.m_maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, mantaConeDirtyFighting.m_coneBackwardOffset, true, false, false);
			AbilityMod.AddToken(tokens, this.m_onCastDamageAmountMod, "OnCastDamageAmount", string.Empty, mantaConeDirtyFighting.m_onCastDamageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_dirtyFightingEffectDataMod, "DirtyFightingEffectData", mantaConeDirtyFighting.m_dirtyFightingEffectData, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectDataMod, "EnemyHitEffectData", mantaConeDirtyFighting.m_enemyHitEffectData, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnTargetFromExplosionMod, "EffectOnTargetFromExplosion", mantaConeDirtyFighting.m_effectOnTargetFromExplosion, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnTargetWhenExpiresWithoutExplosionMod, "EffectOnTargetWhenExpires", null, true);
			AbilityMod.AddToken(tokens, this.m_effectExplosionDamageMod, "EffectExplosionDamage", string.Empty, mantaConeDirtyFighting.m_effectExplosionDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_techPointGainPerExplosionMod, "TechPointGainPerExplosion", string.Empty, mantaConeDirtyFighting.m_techPointGainPerExplosion, true, false);
			AbilityMod.AddToken(tokens, this.m_healPerExplosionMod, "HealAmountPerExplosion", string.Empty, mantaConeDirtyFighting.m_healAmountPerExplosion, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MantaConeDirtyFighting mantaConeDirtyFighting = base.GetTargetAbilityOnAbilityData(abilityData) as MantaConeDirtyFighting;
		bool flag = mantaConeDirtyFighting != null;
		string text = string.Empty;
		text += base.PropDesc(this.m_coneRangeMod, "[ConeRange]", flag, (!flag) ? 0f : mantaConeDirtyFighting.m_coneRange);
		string str = text;
		AbilityModPropertyFloat coneWidthMod = this.m_coneWidthMod;
		string prefix = "[ConeWidth]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = mantaConeDirtyFighting.m_coneWidth;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(coneWidthMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyBool penetrateLoSMod = this.m_penetrateLoSMod;
		string prefix2 = "[PenetrateLoS]";
		bool showBaseVal2 = flag;
		bool baseVal2;
		if (flag)
		{
			baseVal2 = mantaConeDirtyFighting.m_penetrateLoS;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + base.PropDesc(penetrateLoSMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt maxTargetsMod = this.m_maxTargetsMod;
		string prefix3 = "[MaxTargets]";
		bool showBaseVal3 = flag;
		int baseVal3;
		if (flag)
		{
			baseVal3 = mantaConeDirtyFighting.m_maxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(maxTargetsMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, (!flag) ? 0f : mantaConeDirtyFighting.m_coneBackwardOffset);
		string str4 = text;
		AbilityModPropertyInt onCastDamageAmountMod = this.m_onCastDamageAmountMod;
		string prefix4 = "[OnCastDamageAmount]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = mantaConeDirtyFighting.m_onCastDamageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(onCastDamageAmountMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyEffectData dirtyFightingEffectDataMod = this.m_dirtyFightingEffectDataMod;
		string prefix5 = "[DirtyFightingEffectData]";
		bool showBaseVal5 = flag;
		StandardActorEffectData baseVal5;
		if (flag)
		{
			baseVal5 = mantaConeDirtyFighting.m_dirtyFightingEffectData;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(dirtyFightingEffectDataMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyEffectInfo enemyHitEffectDataMod = this.m_enemyHitEffectDataMod;
		string prefix6 = "[EnemyHitEffectData]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal6;
		if (flag)
		{
			baseVal6 = mantaConeDirtyFighting.m_enemyHitEffectData;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + base.PropDesc(enemyHitEffectDataMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyEffectInfo effectOnTargetFromExplosionMod = this.m_effectOnTargetFromExplosionMod;
		string prefix7 = "[EffectOnTargetFromExplosion]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
		if (flag)
		{
			baseVal7 = mantaConeDirtyFighting.m_effectOnTargetFromExplosion;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + base.PropDesc(effectOnTargetFromExplosionMod, prefix7, showBaseVal7, baseVal7);
		text += base.PropDesc(this.m_effectOnTargetWhenExpiresWithoutExplosionMod, "[EffectOnTargetWhenExpires]", flag, null);
		string str8 = text;
		AbilityModPropertyInt effectExplosionDamageMod = this.m_effectExplosionDamageMod;
		string prefix8 = "[EffectExplosionDamage]";
		bool showBaseVal8 = flag;
		int baseVal8;
		if (flag)
		{
			baseVal8 = mantaConeDirtyFighting.m_effectExplosionDamage;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str8 + base.PropDesc(effectExplosionDamageMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyBool explodeOnlyFromSelfDamageMod = this.m_explodeOnlyFromSelfDamageMod;
		string prefix9 = "[ExplodeOnlyFromSelfDamage]";
		bool showBaseVal9 = flag;
		bool baseVal9;
		if (flag)
		{
			baseVal9 = mantaConeDirtyFighting.m_explodeOnlyFromSelfDamage;
		}
		else
		{
			baseVal9 = false;
		}
		text = str9 + base.PropDesc(explodeOnlyFromSelfDamageMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyInt techPointGainPerExplosionMod = this.m_techPointGainPerExplosionMod;
		string prefix10 = "[TechPointGainPerExplosion]";
		bool showBaseVal10 = flag;
		int baseVal10;
		if (flag)
		{
			baseVal10 = mantaConeDirtyFighting.m_techPointGainPerExplosion;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + base.PropDesc(techPointGainPerExplosionMod, prefix10, showBaseVal10, baseVal10);
		return text + base.PropDesc(this.m_healPerExplosionMod, "[HealAmountPerExplosion]", flag, (!flag) ? 0 : mantaConeDirtyFighting.m_healAmountPerExplosion);
	}
}
