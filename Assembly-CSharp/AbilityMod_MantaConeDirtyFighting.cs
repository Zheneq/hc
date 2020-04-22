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
		if (!(mantaConeDirtyFighting != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_coneRangeMod, "ConeRange", string.Empty, mantaConeDirtyFighting.m_coneRange);
			AbilityMod.AddToken(tokens, m_coneWidthMod, "ConeWidth", string.Empty, mantaConeDirtyFighting.m_coneWidth);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, mantaConeDirtyFighting.m_maxTargets);
			AbilityMod.AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, mantaConeDirtyFighting.m_coneBackwardOffset);
			AbilityMod.AddToken(tokens, m_onCastDamageAmountMod, "OnCastDamageAmount", string.Empty, mantaConeDirtyFighting.m_onCastDamageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_dirtyFightingEffectDataMod, "DirtyFightingEffectData", mantaConeDirtyFighting.m_dirtyFightingEffectData);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectDataMod, "EnemyHitEffectData", mantaConeDirtyFighting.m_enemyHitEffectData);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnTargetFromExplosionMod, "EffectOnTargetFromExplosion", mantaConeDirtyFighting.m_effectOnTargetFromExplosion);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnTargetWhenExpiresWithoutExplosionMod, "EffectOnTargetWhenExpires");
			AbilityMod.AddToken(tokens, m_effectExplosionDamageMod, "EffectExplosionDamage", string.Empty, mantaConeDirtyFighting.m_effectExplosionDamage);
			AbilityMod.AddToken(tokens, m_techPointGainPerExplosionMod, "TechPointGainPerExplosion", string.Empty, mantaConeDirtyFighting.m_techPointGainPerExplosion);
			AbilityMod.AddToken(tokens, m_healPerExplosionMod, "HealAmountPerExplosion", string.Empty, mantaConeDirtyFighting.m_healAmountPerExplosion);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MantaConeDirtyFighting mantaConeDirtyFighting = GetTargetAbilityOnAbilityData(abilityData) as MantaConeDirtyFighting;
		bool flag = mantaConeDirtyFighting != null;
		string empty = string.Empty;
		empty += PropDesc(m_coneRangeMod, "[ConeRange]", flag, (!flag) ? 0f : mantaConeDirtyFighting.m_coneRange);
		string str = empty;
		AbilityModPropertyFloat coneWidthMod = m_coneWidthMod;
		float baseVal;
		if (flag)
		{
			baseVal = mantaConeDirtyFighting.m_coneWidth;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(coneWidthMod, "[ConeWidth]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyBool penetrateLoSMod = m_penetrateLoSMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = (mantaConeDirtyFighting.m_penetrateLoS ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(penetrateLoSMod, "[PenetrateLoS]", flag, (byte)baseVal2 != 0);
		string str3 = empty;
		AbilityModPropertyInt maxTargetsMod = m_maxTargetsMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = mantaConeDirtyFighting.m_maxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(maxTargetsMod, "[MaxTargets]", flag, baseVal3);
		empty += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, (!flag) ? 0f : mantaConeDirtyFighting.m_coneBackwardOffset);
		string str4 = empty;
		AbilityModPropertyInt onCastDamageAmountMod = m_onCastDamageAmountMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = mantaConeDirtyFighting.m_onCastDamageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(onCastDamageAmountMod, "[OnCastDamageAmount]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyEffectData dirtyFightingEffectDataMod = m_dirtyFightingEffectDataMod;
		object baseVal5;
		if (flag)
		{
			baseVal5 = mantaConeDirtyFighting.m_dirtyFightingEffectData;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + PropDesc(dirtyFightingEffectDataMod, "[DirtyFightingEffectData]", flag, (StandardActorEffectData)baseVal5);
		string str6 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectDataMod = m_enemyHitEffectDataMod;
		object baseVal6;
		if (flag)
		{
			baseVal6 = mantaConeDirtyFighting.m_enemyHitEffectData;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str6 + PropDesc(enemyHitEffectDataMod, "[EnemyHitEffectData]", flag, (StandardEffectInfo)baseVal6);
		string str7 = empty;
		AbilityModPropertyEffectInfo effectOnTargetFromExplosionMod = m_effectOnTargetFromExplosionMod;
		object baseVal7;
		if (flag)
		{
			baseVal7 = mantaConeDirtyFighting.m_effectOnTargetFromExplosion;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + PropDesc(effectOnTargetFromExplosionMod, "[EffectOnTargetFromExplosion]", flag, (StandardEffectInfo)baseVal7);
		empty += PropDesc(m_effectOnTargetWhenExpiresWithoutExplosionMod, "[EffectOnTargetWhenExpires]", flag);
		string str8 = empty;
		AbilityModPropertyInt effectExplosionDamageMod = m_effectExplosionDamageMod;
		int baseVal8;
		if (flag)
		{
			baseVal8 = mantaConeDirtyFighting.m_effectExplosionDamage;
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(effectExplosionDamageMod, "[EffectExplosionDamage]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyBool explodeOnlyFromSelfDamageMod = m_explodeOnlyFromSelfDamageMod;
		int baseVal9;
		if (flag)
		{
			baseVal9 = (mantaConeDirtyFighting.m_explodeOnlyFromSelfDamage ? 1 : 0);
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(explodeOnlyFromSelfDamageMod, "[ExplodeOnlyFromSelfDamage]", flag, (byte)baseVal9 != 0);
		string str10 = empty;
		AbilityModPropertyInt techPointGainPerExplosionMod = m_techPointGainPerExplosionMod;
		int baseVal10;
		if (flag)
		{
			baseVal10 = mantaConeDirtyFighting.m_techPointGainPerExplosion;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(techPointGainPerExplosionMod, "[TechPointGainPerExplosion]", flag, baseVal10);
		return empty + PropDesc(m_healPerExplosionMod, "[HealAmountPerExplosion]", flag, flag ? mantaConeDirtyFighting.m_healAmountPerExplosion : 0);
	}
}
