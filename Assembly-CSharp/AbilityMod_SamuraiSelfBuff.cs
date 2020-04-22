using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SamuraiSelfBuff : AbilityMod
{
	[Header("-- Buffs")]
	public AbilityModPropertyBool m_selfBuffLastsUntilYouDealDamageMod;

	public AbilityModPropertyEffectInfo m_selfBuffEffectMod;

	[Header("-- Extra damage to other abilities if this ability is queued")]
	public AbilityModPropertyInt m_extraDamageIfQueuedMod;

	[Header("-- Shielding")]
	public AbilityModPropertyInt m_baseShieldingMod;

	public AbilityModPropertyInt m_extraShieldingIfOnlyAbilityMod;

	public AbilityModPropertyEffectInfo m_generalEffectOnSelfMod;

	[Header("-- AoE")]
	public AbilityModPropertyFloat m_aoeRadiusMod;

	public AbilityModPropertyFloat m_knockbackDistMod;

	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyBool m_penetrateLoSMod;

	[Header("-- Damage reactions")]
	public AbilityModPropertyInt m_damageIncreaseFirstHitMod;

	public AbilityModPropertyInt m_damageIncreaseSubseqHitsMod;

	public AbilityModPropertyInt m_techPointGainPerIncomingHitMod;

	public AbilityModPropertyBool m_buffInResponseToIndirectDamageMod;

	public AbilityModPropertyInt m_cdrIfNotHitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SamuraiSelfBuff);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SamuraiSelfBuff samuraiSelfBuff = targetAbility as SamuraiSelfBuff;
		if (!(samuraiSelfBuff != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken_EffectMod(tokens, m_selfBuffEffectMod, "SelfBuffEffect", samuraiSelfBuff.m_selfBuffEffect);
			AbilityMod.AddToken(tokens, m_extraDamageIfQueuedMod, "ExtraDamageIfQueued", string.Empty, samuraiSelfBuff.m_extraDamageIfQueued);
			AbilityMod.AddToken(tokens, m_baseShieldingMod, "BaseShielding", string.Empty, samuraiSelfBuff.m_baseShielding);
			AbilityMod.AddToken(tokens, m_extraShieldingIfOnlyAbilityMod, "ExtraShieldingIfOnlyAbility", string.Empty, samuraiSelfBuff.m_extraShieldingIfOnlyAbility);
			AbilityMod.AddToken_EffectMod(tokens, m_generalEffectOnSelfMod, "GeneralEffectOnSelf", samuraiSelfBuff.m_generalEffectOnSelf);
			AbilityMod.AddToken(tokens, m_aoeRadiusMod, "AoeRadius", string.Empty, samuraiSelfBuff.m_aoeRadius);
			AbilityMod.AddToken(tokens, m_knockbackDistMod, "KnockbackDist", string.Empty, samuraiSelfBuff.m_knockbackDist);
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, samuraiSelfBuff.m_damageAmount);
			AbilityMod.AddToken(tokens, m_damageIncreaseFirstHitMod, "DamageIncreaseFirstHit", string.Empty, samuraiSelfBuff.m_damageIncreaseFirstHit);
			AbilityMod.AddToken(tokens, m_damageIncreaseSubseqHitsMod, "DamageIncreaseSubseqHits", string.Empty, samuraiSelfBuff.m_damageIncreaseSubseqHits);
			AbilityMod.AddToken(tokens, m_techPointGainPerIncomingHitMod, "TechPointGainPerIncomingHit", string.Empty, samuraiSelfBuff.m_techPointGainPerIncomingHit);
			AbilityMod.AddToken(tokens, m_cdrIfNotHitMod, "CdrIfNotHit", string.Empty, samuraiSelfBuff.m_cdrIfNotHit);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SamuraiSelfBuff samuraiSelfBuff = GetTargetAbilityOnAbilityData(abilityData) as SamuraiSelfBuff;
		bool flag = samuraiSelfBuff != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyBool selfBuffLastsUntilYouDealDamageMod = m_selfBuffLastsUntilYouDealDamageMod;
		int baseVal;
		if (flag)
		{
			baseVal = (samuraiSelfBuff.m_selfBuffLastsUntilYouDealDamage ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(selfBuffLastsUntilYouDealDamageMod, "[SelfBuffLastsUntilYouDealDamage]", flag, (byte)baseVal != 0);
		string str2 = empty;
		AbilityModPropertyEffectInfo selfBuffEffectMod = m_selfBuffEffectMod;
		object baseVal2;
		if (flag)
		{
			baseVal2 = samuraiSelfBuff.m_selfBuffEffect;
		}
		else
		{
			baseVal2 = null;
		}
		empty = str2 + PropDesc(selfBuffEffectMod, "[SelfBuffEffect]", flag, (StandardEffectInfo)baseVal2);
		string str3 = empty;
		AbilityModPropertyInt extraDamageIfQueuedMod = m_extraDamageIfQueuedMod;
		int baseVal3;
		if (flag)
		{
			baseVal3 = samuraiSelfBuff.m_extraDamageIfQueued;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(extraDamageIfQueuedMod, "[ExtraDamageIfQueued]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyInt baseShieldingMod = m_baseShieldingMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = samuraiSelfBuff.m_baseShielding;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(baseShieldingMod, "[BaseShielding]", flag, baseVal4);
		empty += PropDesc(m_extraShieldingIfOnlyAbilityMod, "[ExtraShieldingIfOnlyAbility]", flag, flag ? samuraiSelfBuff.m_extraShieldingIfOnlyAbility : 0);
		string str5 = empty;
		AbilityModPropertyEffectInfo generalEffectOnSelfMod = m_generalEffectOnSelfMod;
		object baseVal5;
		if (flag)
		{
			baseVal5 = samuraiSelfBuff.m_generalEffectOnSelf;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + PropDesc(generalEffectOnSelfMod, "[GeneralEffectOnSelf]", flag, (StandardEffectInfo)baseVal5);
		string str6 = empty;
		AbilityModPropertyFloat aoeRadiusMod = m_aoeRadiusMod;
		float baseVal6;
		if (flag)
		{
			baseVal6 = samuraiSelfBuff.m_aoeRadius;
		}
		else
		{
			baseVal6 = 0f;
		}
		empty = str6 + PropDesc(aoeRadiusMod, "[AoeRadius]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyFloat knockbackDistMod = m_knockbackDistMod;
		float baseVal7;
		if (flag)
		{
			baseVal7 = samuraiSelfBuff.m_knockbackDist;
		}
		else
		{
			baseVal7 = 0f;
		}
		empty = str7 + PropDesc(knockbackDistMod, "[KnockbackDist]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyInt damageAmountMod = m_damageAmountMod;
		int baseVal8;
		if (flag)
		{
			baseVal8 = samuraiSelfBuff.m_damageAmount;
		}
		else
		{
			baseVal8 = 0;
		}
		empty = str8 + PropDesc(damageAmountMod, "[DamageAmount]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyBool penetrateLoSMod = m_penetrateLoSMod;
		int baseVal9;
		if (flag)
		{
			baseVal9 = (samuraiSelfBuff.m_penetrateLoS ? 1 : 0);
		}
		else
		{
			baseVal9 = 0;
		}
		empty = str9 + PropDesc(penetrateLoSMod, "[PenetrateLoS]", flag, (byte)baseVal9 != 0);
		string str10 = empty;
		AbilityModPropertyInt damageIncreaseFirstHitMod = m_damageIncreaseFirstHitMod;
		int baseVal10;
		if (flag)
		{
			baseVal10 = samuraiSelfBuff.m_damageIncreaseFirstHit;
		}
		else
		{
			baseVal10 = 0;
		}
		empty = str10 + PropDesc(damageIncreaseFirstHitMod, "[DamageIncreaseFirstHit]", flag, baseVal10);
		string str11 = empty;
		AbilityModPropertyInt damageIncreaseSubseqHitsMod = m_damageIncreaseSubseqHitsMod;
		int baseVal11;
		if (flag)
		{
			baseVal11 = samuraiSelfBuff.m_damageIncreaseSubseqHits;
		}
		else
		{
			baseVal11 = 0;
		}
		empty = str11 + PropDesc(damageIncreaseSubseqHitsMod, "[DamageIncreaseSubseqHits]", flag, baseVal11);
		string str12 = empty;
		AbilityModPropertyInt techPointGainPerIncomingHitMod = m_techPointGainPerIncomingHitMod;
		int baseVal12;
		if (flag)
		{
			baseVal12 = samuraiSelfBuff.m_techPointGainPerIncomingHit;
		}
		else
		{
			baseVal12 = 0;
		}
		empty = str12 + PropDesc(techPointGainPerIncomingHitMod, "[TechPointGainPerIncomingHit]", flag, baseVal12);
		string str13 = empty;
		AbilityModPropertyBool buffInResponseToIndirectDamageMod = m_buffInResponseToIndirectDamageMod;
		int baseVal13;
		if (flag)
		{
			baseVal13 = (samuraiSelfBuff.m_buffInResponseToIndirectDamage ? 1 : 0);
		}
		else
		{
			baseVal13 = 0;
		}
		empty = str13 + PropDesc(buffInResponseToIndirectDamageMod, "[BuffInResponseToIndirectDamage]", flag, (byte)baseVal13 != 0);
		string str14 = empty;
		AbilityModPropertyInt cdrIfNotHitMod = m_cdrIfNotHitMod;
		int baseVal14;
		if (flag)
		{
			baseVal14 = samuraiSelfBuff.m_cdrIfNotHit;
		}
		else
		{
			baseVal14 = 0;
		}
		return str14 + PropDesc(cdrIfNotHitMod, "[CdrIfNotHit]", flag, baseVal14);
	}
}
