using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SenseiBide : AbilityMod
{
	[Header("-- Targeting --")]
	public AbilityModPropertyBool m_targetingIgnoreLosMod;

	[Separator("Effect on Cast Target", "cyan")]
	public AbilityModPropertyEffectData m_onCastTargetEffectDataMod;

	[Header("-- Additional Effect on targeted actor, for shielding, etc")]
	public AbilityModPropertyEffectInfo m_additionalTargetHitEffectMod;

	[Separator("For Explosion Hits", "cyan")]
	public AbilityModPropertyFloat m_explosionRadiusMod;

	public AbilityModPropertyBool m_ignoreLosMod;

	[Header("-- Explosion Hit --")]
	public AbilityModPropertyInt m_maxDamageMod;

	public AbilityModPropertyInt m_baseDamageMod;

	public AbilityModPropertyFloat m_damageMultMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	[Header("-- Heal portion of absorb remaining")]
	public AbilityModPropertyFloat m_absorbMultForHealMod;

	[Header("-- Damage portion of initial damage, on turns after")]
	public AbilityModPropertyFloat m_multOnInitialDamageForSubseqHitsMod;

	[Separator("Extra Heal on Heal AoE Ability", true)]
	public AbilityModPropertyInt m_extraHealOnHealAoeIfQueuedMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SenseiBide);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SenseiBide senseiBide = targetAbility as SenseiBide;
		if (!(senseiBide != null))
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken_EffectMod(tokens, m_onCastTargetEffectDataMod, "OnCastTargetEffectData", senseiBide.m_onCastTargetEffectData);
			AbilityMod.AddToken_EffectMod(tokens, m_additionalTargetHitEffectMod, "AdditionalTargetHitEffect", senseiBide.m_additionalTargetHitEffect);
			AbilityMod.AddToken(tokens, m_explosionRadiusMod, "ExplosionRadius", string.Empty, senseiBide.m_explosionRadius);
			AbilityMod.AddToken(tokens, m_maxDamageMod, "MaxDamage", string.Empty, senseiBide.m_maxDamage);
			AbilityMod.AddToken(tokens, m_baseDamageMod, "BaseDamage", string.Empty, senseiBide.m_baseDamage);
			AbilityMod.AddToken(tokens, m_damageMultMod, "DamageMult", string.Empty, senseiBide.m_damageMult);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", senseiBide.m_enemyHitEffect);
			AbilityMod.AddToken(tokens, m_absorbMultForHealMod, "AbsorbMultForHeal", string.Empty, senseiBide.m_absorbMultForHeal);
			AbilityMod.AddToken(tokens, m_multOnInitialDamageForSubseqHitsMod, "MultOnInitialDamageForSubseqHits", string.Empty, senseiBide.m_multOnInitialDamageForSubseqHits);
			AbilityMod.AddToken(tokens, m_extraHealOnHealAoeIfQueuedMod, "ExtraHealOnHealAoeIfQueued", string.Empty, senseiBide.m_extraHealOnHealAoeIfQueued);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SenseiBide senseiBide = GetTargetAbilityOnAbilityData(abilityData) as SenseiBide;
		bool flag = senseiBide != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyBool targetingIgnoreLosMod = m_targetingIgnoreLosMod;
		int baseVal;
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = (senseiBide.m_targetingIgnoreLos ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(targetingIgnoreLosMod, "[TargetingIgnoreLos]", flag, (byte)baseVal != 0);
		string str2 = empty;
		AbilityModPropertyEffectData onCastTargetEffectDataMod = m_onCastTargetEffectDataMod;
		object baseVal2;
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = senseiBide.m_onCastTargetEffectData;
		}
		else
		{
			baseVal2 = null;
		}
		empty = str2 + PropDesc(onCastTargetEffectDataMod, "[OnCastTargetEffectData]", flag, (StandardActorEffectData)baseVal2);
		string str3 = empty;
		AbilityModPropertyEffectInfo additionalTargetHitEffectMod = m_additionalTargetHitEffectMod;
		object baseVal3;
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = senseiBide.m_additionalTargetHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		empty = str3 + PropDesc(additionalTargetHitEffectMod, "[AdditionalTargetHitEffect]", flag, (StandardEffectInfo)baseVal3);
		empty += PropDesc(m_explosionRadiusMod, "[ExplosionRadius]", flag, (!flag) ? 0f : senseiBide.m_explosionRadius);
		empty += PropDesc(m_ignoreLosMod, "[IgnoreLos]", flag, flag && senseiBide.m_ignoreLos);
		string str4 = empty;
		AbilityModPropertyInt maxDamageMod = m_maxDamageMod;
		int baseVal4;
		if (flag)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal4 = senseiBide.m_maxDamage;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(maxDamageMod, "[MaxDamage]", flag, baseVal4);
		empty += PropDesc(m_baseDamageMod, "[BaseDamage]", flag, flag ? senseiBide.m_baseDamage : 0);
		string str5 = empty;
		AbilityModPropertyFloat damageMultMod = m_damageMultMod;
		float baseVal5;
		if (flag)
		{
			while (true)
			{
				switch (1)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal5 = senseiBide.m_damageMult;
		}
		else
		{
			baseVal5 = 0f;
		}
		empty = str5 + PropDesc(damageMultMod, "[DamageMult]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectMod = m_enemyHitEffectMod;
		object baseVal6;
		if (flag)
		{
			while (true)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal6 = senseiBide.m_enemyHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str6 + PropDesc(enemyHitEffectMod, "[EnemyHitEffect]", flag, (StandardEffectInfo)baseVal6);
		string str7 = empty;
		AbilityModPropertyFloat absorbMultForHealMod = m_absorbMultForHealMod;
		float baseVal7;
		if (flag)
		{
			while (true)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal7 = senseiBide.m_absorbMultForHeal;
		}
		else
		{
			baseVal7 = 0f;
		}
		empty = str7 + PropDesc(absorbMultForHealMod, "[AbsorbMultForHeal]", flag, baseVal7);
		string str8 = empty;
		AbilityModPropertyFloat multOnInitialDamageForSubseqHitsMod = m_multOnInitialDamageForSubseqHitsMod;
		float baseVal8;
		if (flag)
		{
			while (true)
			{
				switch (5)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal8 = senseiBide.m_multOnInitialDamageForSubseqHits;
		}
		else
		{
			baseVal8 = 0f;
		}
		empty = str8 + PropDesc(multOnInitialDamageForSubseqHitsMod, "[MultOnInitialDamageForSubseqHits]", flag, baseVal8);
		string str9 = empty;
		AbilityModPropertyInt extraHealOnHealAoeIfQueuedMod = m_extraHealOnHealAoeIfQueuedMod;
		int baseVal9;
		if (flag)
		{
			while (true)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal9 = senseiBide.m_extraHealOnHealAoeIfQueued;
		}
		else
		{
			baseVal9 = 0;
		}
		return str9 + PropDesc(extraHealOnHealAoeIfQueuedMod, "[ExtraHealOnHealAoeIfQueued]", flag, baseVal9);
	}
}
