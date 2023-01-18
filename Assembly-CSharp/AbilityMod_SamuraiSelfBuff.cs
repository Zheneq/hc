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
		if (samuraiSelfBuff != null)
		{
			AddToken_EffectMod(tokens, m_selfBuffEffectMod, "SelfBuffEffect", samuraiSelfBuff.m_selfBuffEffect);
			AddToken(tokens, m_extraDamageIfQueuedMod, "ExtraDamageIfQueued", string.Empty, samuraiSelfBuff.m_extraDamageIfQueued);
			AddToken(tokens, m_baseShieldingMod, "BaseShielding", string.Empty, samuraiSelfBuff.m_baseShielding);
			AddToken(tokens, m_extraShieldingIfOnlyAbilityMod, "ExtraShieldingIfOnlyAbility", string.Empty, samuraiSelfBuff.m_extraShieldingIfOnlyAbility);
			AddToken_EffectMod(tokens, m_generalEffectOnSelfMod, "GeneralEffectOnSelf", samuraiSelfBuff.m_generalEffectOnSelf);
			AddToken(tokens, m_aoeRadiusMod, "AoeRadius", string.Empty, samuraiSelfBuff.m_aoeRadius);
			AddToken(tokens, m_knockbackDistMod, "KnockbackDist", string.Empty, samuraiSelfBuff.m_knockbackDist);
			AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, samuraiSelfBuff.m_damageAmount);
			AddToken(tokens, m_damageIncreaseFirstHitMod, "DamageIncreaseFirstHit", string.Empty, samuraiSelfBuff.m_damageIncreaseFirstHit);
			AddToken(tokens, m_damageIncreaseSubseqHitsMod, "DamageIncreaseSubseqHits", string.Empty, samuraiSelfBuff.m_damageIncreaseSubseqHits);
			AddToken(tokens, m_techPointGainPerIncomingHitMod, "TechPointGainPerIncomingHit", string.Empty, samuraiSelfBuff.m_techPointGainPerIncomingHit);
			AddToken(tokens, m_cdrIfNotHitMod, "CdrIfNotHit", string.Empty, samuraiSelfBuff.m_cdrIfNotHit);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SamuraiSelfBuff samuraiSelfBuff = GetTargetAbilityOnAbilityData(abilityData) as SamuraiSelfBuff;
		bool isValid = samuraiSelfBuff != null;
		string desc = string.Empty;
		desc += PropDesc(m_selfBuffLastsUntilYouDealDamageMod, "[SelfBuffLastsUntilYouDealDamage]", isValid, isValid && samuraiSelfBuff.m_selfBuffLastsUntilYouDealDamage);
		desc += PropDesc(m_selfBuffEffectMod, "[SelfBuffEffect]", isValid, isValid ? samuraiSelfBuff.m_selfBuffEffect : null);
		desc += PropDesc(m_extraDamageIfQueuedMod, "[ExtraDamageIfQueued]", isValid, isValid ? samuraiSelfBuff.m_extraDamageIfQueued : 0);
		desc += PropDesc(m_baseShieldingMod, "[BaseShielding]", isValid, isValid ? samuraiSelfBuff.m_baseShielding : 0);
		desc += PropDesc(m_extraShieldingIfOnlyAbilityMod, "[ExtraShieldingIfOnlyAbility]", isValid, isValid ? samuraiSelfBuff.m_extraShieldingIfOnlyAbility : 0);
		desc += PropDesc(m_generalEffectOnSelfMod, "[GeneralEffectOnSelf]", isValid, isValid ? samuraiSelfBuff.m_generalEffectOnSelf : null);
		desc += PropDesc(m_aoeRadiusMod, "[AoeRadius]", isValid, isValid ? samuraiSelfBuff.m_aoeRadius : 0f);
		desc += PropDesc(m_knockbackDistMod, "[KnockbackDist]", isValid, isValid ? samuraiSelfBuff.m_knockbackDist : 0f);
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isValid, isValid ? samuraiSelfBuff.m_damageAmount : 0);
		desc += PropDesc(m_penetrateLoSMod, "[PenetrateLoS]", isValid, isValid && samuraiSelfBuff.m_penetrateLoS);
		desc += PropDesc(m_damageIncreaseFirstHitMod, "[DamageIncreaseFirstHit]", isValid, isValid ? samuraiSelfBuff.m_damageIncreaseFirstHit : 0);
		desc += PropDesc(m_damageIncreaseSubseqHitsMod, "[DamageIncreaseSubseqHits]", isValid, isValid ? samuraiSelfBuff.m_damageIncreaseSubseqHits : 0);
		desc += PropDesc(m_techPointGainPerIncomingHitMod, "[TechPointGainPerIncomingHit]", isValid, isValid ? samuraiSelfBuff.m_techPointGainPerIncomingHit : 0);
		desc += PropDesc(m_buffInResponseToIndirectDamageMod, "[BuffInResponseToIndirectDamage]", isValid, isValid && samuraiSelfBuff.m_buffInResponseToIndirectDamage);
		return desc + PropDesc(m_cdrIfNotHitMod, "[CdrIfNotHit]", isValid, isValid ? samuraiSelfBuff.m_cdrIfNotHit : 0);
	}
}
