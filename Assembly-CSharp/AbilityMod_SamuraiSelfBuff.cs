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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SamuraiSelfBuff.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken_EffectMod(tokens, this.m_selfBuffEffectMod, "SelfBuffEffect", samuraiSelfBuff.m_selfBuffEffect, true);
			AbilityMod.AddToken(tokens, this.m_extraDamageIfQueuedMod, "ExtraDamageIfQueued", string.Empty, samuraiSelfBuff.m_extraDamageIfQueued, true, false);
			AbilityMod.AddToken(tokens, this.m_baseShieldingMod, "BaseShielding", string.Empty, samuraiSelfBuff.m_baseShielding, true, false);
			AbilityMod.AddToken(tokens, this.m_extraShieldingIfOnlyAbilityMod, "ExtraShieldingIfOnlyAbility", string.Empty, samuraiSelfBuff.m_extraShieldingIfOnlyAbility, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_generalEffectOnSelfMod, "GeneralEffectOnSelf", samuraiSelfBuff.m_generalEffectOnSelf, true);
			AbilityMod.AddToken(tokens, this.m_aoeRadiusMod, "AoeRadius", string.Empty, samuraiSelfBuff.m_aoeRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_knockbackDistMod, "KnockbackDist", string.Empty, samuraiSelfBuff.m_knockbackDist, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, samuraiSelfBuff.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_damageIncreaseFirstHitMod, "DamageIncreaseFirstHit", string.Empty, samuraiSelfBuff.m_damageIncreaseFirstHit, true, false);
			AbilityMod.AddToken(tokens, this.m_damageIncreaseSubseqHitsMod, "DamageIncreaseSubseqHits", string.Empty, samuraiSelfBuff.m_damageIncreaseSubseqHits, true, false);
			AbilityMod.AddToken(tokens, this.m_techPointGainPerIncomingHitMod, "TechPointGainPerIncomingHit", string.Empty, samuraiSelfBuff.m_techPointGainPerIncomingHit, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrIfNotHitMod, "CdrIfNotHit", string.Empty, samuraiSelfBuff.m_cdrIfNotHit, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SamuraiSelfBuff samuraiSelfBuff = base.GetTargetAbilityOnAbilityData(abilityData) as SamuraiSelfBuff;
		bool flag = samuraiSelfBuff != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyBool selfBuffLastsUntilYouDealDamageMod = this.m_selfBuffLastsUntilYouDealDamageMod;
		string prefix = "[SelfBuffLastsUntilYouDealDamage]";
		bool showBaseVal = flag;
		bool baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SamuraiSelfBuff.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = samuraiSelfBuff.m_selfBuffLastsUntilYouDealDamage;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(selfBuffLastsUntilYouDealDamageMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyEffectInfo selfBuffEffectMod = this.m_selfBuffEffectMod;
		string prefix2 = "[SelfBuffEffect]";
		bool showBaseVal2 = flag;
		StandardEffectInfo baseVal2;
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
			baseVal2 = samuraiSelfBuff.m_selfBuffEffect;
		}
		else
		{
			baseVal2 = null;
		}
		text = str2 + base.PropDesc(selfBuffEffectMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt extraDamageIfQueuedMod = this.m_extraDamageIfQueuedMod;
		string prefix3 = "[ExtraDamageIfQueued]";
		bool showBaseVal3 = flag;
		int baseVal3;
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
			baseVal3 = samuraiSelfBuff.m_extraDamageIfQueued;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(extraDamageIfQueuedMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt baseShieldingMod = this.m_baseShieldingMod;
		string prefix4 = "[BaseShielding]";
		bool showBaseVal4 = flag;
		int baseVal4;
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
			baseVal4 = samuraiSelfBuff.m_baseShielding;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(baseShieldingMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_extraShieldingIfOnlyAbilityMod, "[ExtraShieldingIfOnlyAbility]", flag, (!flag) ? 0 : samuraiSelfBuff.m_extraShieldingIfOnlyAbility);
		string str5 = text;
		AbilityModPropertyEffectInfo generalEffectOnSelfMod = this.m_generalEffectOnSelfMod;
		string prefix5 = "[GeneralEffectOnSelf]";
		bool showBaseVal5 = flag;
		StandardEffectInfo baseVal5;
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
			baseVal5 = samuraiSelfBuff.m_generalEffectOnSelf;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(generalEffectOnSelfMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyFloat aoeRadiusMod = this.m_aoeRadiusMod;
		string prefix6 = "[AoeRadius]";
		bool showBaseVal6 = flag;
		float baseVal6;
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
			baseVal6 = samuraiSelfBuff.m_aoeRadius;
		}
		else
		{
			baseVal6 = 0f;
		}
		text = str6 + base.PropDesc(aoeRadiusMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyFloat knockbackDistMod = this.m_knockbackDistMod;
		string prefix7 = "[KnockbackDist]";
		bool showBaseVal7 = flag;
		float baseVal7;
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
			baseVal7 = samuraiSelfBuff.m_knockbackDist;
		}
		else
		{
			baseVal7 = 0f;
		}
		text = str7 + base.PropDesc(knockbackDistMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyInt damageAmountMod = this.m_damageAmountMod;
		string prefix8 = "[DamageAmount]";
		bool showBaseVal8 = flag;
		int baseVal8;
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
			baseVal8 = samuraiSelfBuff.m_damageAmount;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str8 + base.PropDesc(damageAmountMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyBool penetrateLoSMod = this.m_penetrateLoSMod;
		string prefix9 = "[PenetrateLoS]";
		bool showBaseVal9 = flag;
		bool baseVal9;
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
			baseVal9 = samuraiSelfBuff.m_penetrateLoS;
		}
		else
		{
			baseVal9 = false;
		}
		text = str9 + base.PropDesc(penetrateLoSMod, prefix9, showBaseVal9, baseVal9);
		string str10 = text;
		AbilityModPropertyInt damageIncreaseFirstHitMod = this.m_damageIncreaseFirstHitMod;
		string prefix10 = "[DamageIncreaseFirstHit]";
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
			baseVal10 = samuraiSelfBuff.m_damageIncreaseFirstHit;
		}
		else
		{
			baseVal10 = 0;
		}
		text = str10 + base.PropDesc(damageIncreaseFirstHitMod, prefix10, showBaseVal10, baseVal10);
		string str11 = text;
		AbilityModPropertyInt damageIncreaseSubseqHitsMod = this.m_damageIncreaseSubseqHitsMod;
		string prefix11 = "[DamageIncreaseSubseqHits]";
		bool showBaseVal11 = flag;
		int baseVal11;
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
			baseVal11 = samuraiSelfBuff.m_damageIncreaseSubseqHits;
		}
		else
		{
			baseVal11 = 0;
		}
		text = str11 + base.PropDesc(damageIncreaseSubseqHitsMod, prefix11, showBaseVal11, baseVal11);
		string str12 = text;
		AbilityModPropertyInt techPointGainPerIncomingHitMod = this.m_techPointGainPerIncomingHitMod;
		string prefix12 = "[TechPointGainPerIncomingHit]";
		bool showBaseVal12 = flag;
		int baseVal12;
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
			baseVal12 = samuraiSelfBuff.m_techPointGainPerIncomingHit;
		}
		else
		{
			baseVal12 = 0;
		}
		text = str12 + base.PropDesc(techPointGainPerIncomingHitMod, prefix12, showBaseVal12, baseVal12);
		string str13 = text;
		AbilityModPropertyBool buffInResponseToIndirectDamageMod = this.m_buffInResponseToIndirectDamageMod;
		string prefix13 = "[BuffInResponseToIndirectDamage]";
		bool showBaseVal13 = flag;
		bool baseVal13;
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
			baseVal13 = samuraiSelfBuff.m_buffInResponseToIndirectDamage;
		}
		else
		{
			baseVal13 = false;
		}
		text = str13 + base.PropDesc(buffInResponseToIndirectDamageMod, prefix13, showBaseVal13, baseVal13);
		string str14 = text;
		AbilityModPropertyInt cdrIfNotHitMod = this.m_cdrIfNotHitMod;
		string prefix14 = "[CdrIfNotHit]";
		bool showBaseVal14 = flag;
		int baseVal14;
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
			baseVal14 = samuraiSelfBuff.m_cdrIfNotHit;
		}
		else
		{
			baseVal14 = 0;
		}
		return str14 + base.PropDesc(cdrIfNotHitMod, prefix14, showBaseVal14, baseVal14);
	}
}
