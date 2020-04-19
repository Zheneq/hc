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
		if (senseiBide != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SenseiBide.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken_EffectMod(tokens, this.m_onCastTargetEffectDataMod, "OnCastTargetEffectData", senseiBide.m_onCastTargetEffectData, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_additionalTargetHitEffectMod, "AdditionalTargetHitEffect", senseiBide.m_additionalTargetHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_explosionRadiusMod, "ExplosionRadius", string.Empty, senseiBide.m_explosionRadius, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxDamageMod, "MaxDamage", string.Empty, senseiBide.m_maxDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_baseDamageMod, "BaseDamage", string.Empty, senseiBide.m_baseDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_damageMultMod, "DamageMult", string.Empty, senseiBide.m_damageMult, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EnemyHitEffect", senseiBide.m_enemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_absorbMultForHealMod, "AbsorbMultForHeal", string.Empty, senseiBide.m_absorbMultForHeal, true, false, false);
			AbilityMod.AddToken(tokens, this.m_multOnInitialDamageForSubseqHitsMod, "MultOnInitialDamageForSubseqHits", string.Empty, senseiBide.m_multOnInitialDamageForSubseqHits, true, false, false);
			AbilityMod.AddToken(tokens, this.m_extraHealOnHealAoeIfQueuedMod, "ExtraHealOnHealAoeIfQueued", string.Empty, senseiBide.m_extraHealOnHealAoeIfQueued, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SenseiBide senseiBide = base.GetTargetAbilityOnAbilityData(abilityData) as SenseiBide;
		bool flag = senseiBide != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyBool targetingIgnoreLosMod = this.m_targetingIgnoreLosMod;
		string prefix = "[TargetingIgnoreLos]";
		bool showBaseVal = flag;
		bool baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SenseiBide.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = senseiBide.m_targetingIgnoreLos;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(targetingIgnoreLosMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyEffectData onCastTargetEffectDataMod = this.m_onCastTargetEffectDataMod;
		string prefix2 = "[OnCastTargetEffectData]";
		bool showBaseVal2 = flag;
		StandardActorEffectData baseVal2;
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
			baseVal2 = senseiBide.m_onCastTargetEffectData;
		}
		else
		{
			baseVal2 = null;
		}
		text = str2 + base.PropDesc(onCastTargetEffectDataMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyEffectInfo additionalTargetHitEffectMod = this.m_additionalTargetHitEffectMod;
		string prefix3 = "[AdditionalTargetHitEffect]";
		bool showBaseVal3 = flag;
		StandardEffectInfo baseVal3;
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
			baseVal3 = senseiBide.m_additionalTargetHitEffect;
		}
		else
		{
			baseVal3 = null;
		}
		text = str3 + base.PropDesc(additionalTargetHitEffectMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_explosionRadiusMod, "[ExplosionRadius]", flag, (!flag) ? 0f : senseiBide.m_explosionRadius);
		text += base.PropDesc(this.m_ignoreLosMod, "[IgnoreLos]", flag, flag && senseiBide.m_ignoreLos);
		string str4 = text;
		AbilityModPropertyInt maxDamageMod = this.m_maxDamageMod;
		string prefix4 = "[MaxDamage]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			for (;;)
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
		text = str4 + base.PropDesc(maxDamageMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_baseDamageMod, "[BaseDamage]", flag, (!flag) ? 0 : senseiBide.m_baseDamage);
		string str5 = text;
		AbilityModPropertyFloat damageMultMod = this.m_damageMultMod;
		string prefix5 = "[DamageMult]";
		bool showBaseVal5 = flag;
		float baseVal5;
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
			baseVal5 = senseiBide.m_damageMult;
		}
		else
		{
			baseVal5 = 0f;
		}
		text = str5 + base.PropDesc(damageMultMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyEffectInfo enemyHitEffectMod = this.m_enemyHitEffectMod;
		string prefix6 = "[EnemyHitEffect]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal6;
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
			baseVal6 = senseiBide.m_enemyHitEffect;
		}
		else
		{
			baseVal6 = null;
		}
		text = str6 + base.PropDesc(enemyHitEffectMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyFloat absorbMultForHealMod = this.m_absorbMultForHealMod;
		string prefix7 = "[AbsorbMultForHeal]";
		bool showBaseVal7 = flag;
		float baseVal7;
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
			baseVal7 = senseiBide.m_absorbMultForHeal;
		}
		else
		{
			baseVal7 = 0f;
		}
		text = str7 + base.PropDesc(absorbMultForHealMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyFloat multOnInitialDamageForSubseqHitsMod = this.m_multOnInitialDamageForSubseqHitsMod;
		string prefix8 = "[MultOnInitialDamageForSubseqHits]";
		bool showBaseVal8 = flag;
		float baseVal8;
		if (flag)
		{
			for (;;)
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
		text = str8 + base.PropDesc(multOnInitialDamageForSubseqHitsMod, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyInt extraHealOnHealAoeIfQueuedMod = this.m_extraHealOnHealAoeIfQueuedMod;
		string prefix9 = "[ExtraHealOnHealAoeIfQueued]";
		bool showBaseVal9 = flag;
		int baseVal9;
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
			baseVal9 = senseiBide.m_extraHealOnHealAoeIfQueued;
		}
		else
		{
			baseVal9 = 0;
		}
		return str9 + base.PropDesc(extraHealOnHealAoeIfQueuedMod, prefix9, showBaseVal9, baseVal9);
	}
}
