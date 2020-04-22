using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SparkAoeBuffDebuff : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_radiusMod;

	public AbilityModPropertyBool m_ignoreLosMod;

	[Header("-- For Ally Hit")]
	public AbilityModPropertyInt m_allyHealMod;

	public AbilityModPropertyEffectInfo m_allyHitEffectMod;

	[Header("-- For Self Hit")]
	public AbilityModPropertyInt m_baseSelfHealMod;

	public AbilityModPropertyInt m_selfHealPerHitMod;

	public AbilityModPropertyBool m_selfHealHitCountEnemy;

	public AbilityModPropertyBool m_selfHealHitCountAlly;

	public AbilityModPropertyEffectInfo m_selfHitEffectMod;

	[Header("-- Shield on Self")]
	public AbilityModPropertyInt m_shieldOnSelfPerAllyHitMod;

	public int m_shieldOnSelfDuration = 2;

	[Header("-- For Enemy Hit")]
	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SparkAoeBuffDebuff);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SparkAoeBuffDebuff sparkAoeBuffDebuff = targetAbility as SparkAoeBuffDebuff;
		if (!(sparkAoeBuffDebuff != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_radiusMod, "TargetingRadius", "targeting radius", sparkAoeBuffDebuff.m_radius);
			AbilityMod.AddToken_EffectMod(tokens, m_allyHitEffectMod, "EffectOnAlly", sparkAoeBuffDebuff.m_allyHitEffect);
			AbilityMod.AddToken(tokens, m_allyHealMod, "Heal_OnAlly", "heal on ally", sparkAoeBuffDebuff.m_allyHealAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_selfHitEffectMod, "EffectOnSelf", sparkAoeBuffDebuff.m_selfHitEffect);
			AbilityMod.AddToken(tokens, m_baseSelfHealMod, "Heal_BaseOnSelf", "base heal on self", sparkAoeBuffDebuff.m_baseSelfHeal);
			AbilityMod.AddToken(tokens, m_selfHealPerHitMod, "Heal_PerTargetHit", "heal on self per hit", sparkAoeBuffDebuff.m_selfHealAmountPerHit);
			AbilityMod.AddToken(tokens, m_shieldOnSelfPerAllyHitMod, "SelfShieldPerAllyHit", "shield on self per ally hit", 0, false);
			tokens.Add(new TooltipTokenInt("ShieldOnSelfDuration", "duration for shield on self, from ally hits", m_shieldOnSelfDuration));
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EffectOnEnemy", sparkAoeBuffDebuff.m_enemyHitEffect);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SparkAoeBuffDebuff sparkAoeBuffDebuff = GetTargetAbilityOnAbilityData(abilityData) as SparkAoeBuffDebuff;
		bool flag = sparkAoeBuffDebuff != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat radiusMod = m_radiusMod;
		float baseVal;
		if (flag)
		{
			baseVal = sparkAoeBuffDebuff.m_radius;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(radiusMod, "[Targeting Radius]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyBool ignoreLosMod = m_ignoreLosMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = (sparkAoeBuffDebuff.m_penetrateLos ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(ignoreLosMod, "[Ignore LoS?]", flag, (byte)baseVal2 != 0);
		string str3 = empty;
		AbilityModPropertyInt allyHealMod = m_allyHealMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = sparkAoeBuffDebuff.m_allyHealAmount;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(allyHealMod, "[Ally Heal]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyEffectInfo allyHitEffectMod = m_allyHitEffectMod;
		object baseVal4;
		if (flag)
		{
			baseVal4 = sparkAoeBuffDebuff.m_allyHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str4 + PropDesc(allyHitEffectMod, "{ Ally Hit Effect }", flag, (StandardEffectInfo)baseVal4);
		string str5 = empty;
		AbilityModPropertyInt baseSelfHealMod = m_baseSelfHealMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = sparkAoeBuffDebuff.m_baseSelfHeal;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(baseSelfHealMod, "[Base Self Heal]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyInt selfHealPerHitMod = m_selfHealPerHitMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = sparkAoeBuffDebuff.m_selfHealAmountPerHit;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(selfHealPerHitMod, "[Self Heal per Hit]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyBool selfHealHitCountEnemy = m_selfHealHitCountEnemy;
		int baseVal7;
		if (flag)
		{
			baseVal7 = (sparkAoeBuffDebuff.m_selfHealCountEnemyHit ? 1 : 0);
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(selfHealHitCountEnemy, "[Self Heal Count Enemy]", flag, (byte)baseVal7 != 0);
		string str8 = empty;
		AbilityModPropertyBool selfHealHitCountAlly = m_selfHealHitCountAlly;
		int baseVal8;
		if (flag)
		{
			baseVal8 = (sparkAoeBuffDebuff.m_selfHealCountAllyHit ? 1 : 0);
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(selfHealHitCountAlly, "[Self Heal Count Ally]", flag, (byte)baseVal8 != 0);
		empty += PropDesc(m_selfHitEffectMod, "{ Self Hit Effect }", flag, (!flag) ? null : sparkAoeBuffDebuff.m_selfHitEffect);
		empty += PropDesc(m_shieldOnSelfPerAllyHitMod, "[Shield on Self per Hit]");
		if (m_shieldOnSelfPerAllyHitMod != null && m_shieldOnSelfPerAllyHitMod.GetModifiedValue(0) > 0)
		{
			string text = empty;
			empty = text + "[Shield Duration (for hit on allies)] " + m_shieldOnSelfDuration + "\n";
		}
		string str9 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectMod = m_enemyHitEffectMod;
		object baseVal9;
		if (flag)
		{
			baseVal9 = sparkAoeBuffDebuff.m_enemyHitEffect;
		}
		else
		{
			baseVal9 = null;
		}
		return str9 + PropDesc(enemyHitEffectMod, "{ Enemy Hit Effect }", flag, (StandardEffectInfo)baseVal9);
	}
}
