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
		if (sparkAoeBuffDebuff != null)
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SparkAoeBuffDebuff.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_radiusMod, "TargetingRadius", "targeting radius", sparkAoeBuffDebuff.m_radius, true, false, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyHitEffectMod, "EffectOnAlly", sparkAoeBuffDebuff.m_allyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_allyHealMod, "Heal_OnAlly", "heal on ally", sparkAoeBuffDebuff.m_allyHealAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_selfHitEffectMod, "EffectOnSelf", sparkAoeBuffDebuff.m_selfHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_baseSelfHealMod, "Heal_BaseOnSelf", "base heal on self", sparkAoeBuffDebuff.m_baseSelfHeal, true, false);
			AbilityMod.AddToken(tokens, this.m_selfHealPerHitMod, "Heal_PerTargetHit", "heal on self per hit", sparkAoeBuffDebuff.m_selfHealAmountPerHit, true, false);
			AbilityMod.AddToken(tokens, this.m_shieldOnSelfPerAllyHitMod, "SelfShieldPerAllyHit", "shield on self per ally hit", 0, false, false);
			tokens.Add(new TooltipTokenInt("ShieldOnSelfDuration", "duration for shield on self, from ally hits", this.m_shieldOnSelfDuration));
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectMod, "EffectOnEnemy", sparkAoeBuffDebuff.m_enemyHitEffect, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SparkAoeBuffDebuff sparkAoeBuffDebuff = base.GetTargetAbilityOnAbilityData(abilityData) as SparkAoeBuffDebuff;
		bool flag = sparkAoeBuffDebuff != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat radiusMod = this.m_radiusMod;
		string prefix = "[Targeting Radius]";
		bool showBaseVal = flag;
		float baseVal;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SparkAoeBuffDebuff.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = sparkAoeBuffDebuff.m_radius;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(radiusMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyBool ignoreLosMod = this.m_ignoreLosMod;
		string prefix2 = "[Ignore LoS?]";
		bool showBaseVal2 = flag;
		bool baseVal2;
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
			baseVal2 = sparkAoeBuffDebuff.m_penetrateLos;
		}
		else
		{
			baseVal2 = false;
		}
		text = str2 + base.PropDesc(ignoreLosMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyInt allyHealMod = this.m_allyHealMod;
		string prefix3 = "[Ally Heal]";
		bool showBaseVal3 = flag;
		int baseVal3;
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
			baseVal3 = sparkAoeBuffDebuff.m_allyHealAmount;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + base.PropDesc(allyHealMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyEffectInfo allyHitEffectMod = this.m_allyHitEffectMod;
		string prefix4 = "{ Ally Hit Effect }";
		bool showBaseVal4 = flag;
		StandardEffectInfo baseVal4;
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
			baseVal4 = sparkAoeBuffDebuff.m_allyHitEffect;
		}
		else
		{
			baseVal4 = null;
		}
		text = str4 + base.PropDesc(allyHitEffectMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt baseSelfHealMod = this.m_baseSelfHealMod;
		string prefix5 = "[Base Self Heal]";
		bool showBaseVal5 = flag;
		int baseVal5;
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
			baseVal5 = sparkAoeBuffDebuff.m_baseSelfHeal;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(baseSelfHealMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt selfHealPerHitMod = this.m_selfHealPerHitMod;
		string prefix6 = "[Self Heal per Hit]";
		bool showBaseVal6 = flag;
		int baseVal6;
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
			baseVal6 = sparkAoeBuffDebuff.m_selfHealAmountPerHit;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(selfHealPerHitMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyBool selfHealHitCountEnemy = this.m_selfHealHitCountEnemy;
		string prefix7 = "[Self Heal Count Enemy]";
		bool showBaseVal7 = flag;
		bool baseVal7;
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
			baseVal7 = sparkAoeBuffDebuff.m_selfHealCountEnemyHit;
		}
		else
		{
			baseVal7 = false;
		}
		text = str7 + base.PropDesc(selfHealHitCountEnemy, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyBool selfHealHitCountAlly = this.m_selfHealHitCountAlly;
		string prefix8 = "[Self Heal Count Ally]";
		bool showBaseVal8 = flag;
		bool baseVal8;
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
			baseVal8 = sparkAoeBuffDebuff.m_selfHealCountAllyHit;
		}
		else
		{
			baseVal8 = false;
		}
		text = str8 + base.PropDesc(selfHealHitCountAlly, prefix8, showBaseVal8, baseVal8);
		text += base.PropDesc(this.m_selfHitEffectMod, "{ Self Hit Effect }", flag, (!flag) ? null : sparkAoeBuffDebuff.m_selfHitEffect);
		text += base.PropDesc(this.m_shieldOnSelfPerAllyHitMod, "[Shield on Self per Hit]", false, 0);
		if (this.m_shieldOnSelfPerAllyHitMod != null && this.m_shieldOnSelfPerAllyHitMod.GetModifiedValue(0) > 0)
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
			string text2 = text;
			text = string.Concat(new object[]
			{
				text2,
				"[Shield Duration (for hit on allies)] ",
				this.m_shieldOnSelfDuration,
				"\n"
			});
		}
		string str9 = text;
		AbilityModPropertyEffectInfo enemyHitEffectMod = this.m_enemyHitEffectMod;
		string prefix9 = "{ Enemy Hit Effect }";
		bool showBaseVal9 = flag;
		StandardEffectInfo baseVal9;
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
			baseVal9 = sparkAoeBuffDebuff.m_enemyHitEffect;
		}
		else
		{
			baseVal9 = null;
		}
		return str9 + base.PropDesc(enemyHitEffectMod, prefix9, showBaseVal9, baseVal9);
	}
}
