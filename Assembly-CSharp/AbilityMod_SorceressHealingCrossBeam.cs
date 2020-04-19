using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SorceressHealingCrossBeam : AbilityMod
{
	[Header("-- Laser Size and Number Mod")]
	public AbilityModPropertyInt m_laserNumberMod;

	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_laserRangeMod;

	[Header("-- Normal Damage and Healing Mod")]
	public AbilityModPropertyInt m_normalDamageMod;

	public AbilityModPropertyInt m_normalHealingMod;

	[Header("-- Separate Damage and Healing Mod if only 1 target hit in a laser")]
	public bool m_useSingleTargetHitMods;

	public AbilityModPropertyInt m_singleTargetDamageMod;

	public AbilityModPropertyInt m_singleTargetHealingMod;

	[Header("-- Hit Effect Override")]
	public AbilityModPropertyEffectInfo m_enemyEffectOverride;

	public AbilityModPropertyEffectInfo m_allyEffectOverride;

	[Header("-- Knockback")]
	public float m_knockbackDistance;

	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	public float m_knockbackThresholdDistance = -1f;

	[Header("-- Spawn Ground Effect on Enemy Hit")]
	public StandardGroundEffectInfo m_groundEffectOnEnemyHit;

	public override Type GetTargetAbilityType()
	{
		return typeof(SorceressHealingCrossBeam);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SorceressHealingCrossBeam sorceressHealingCrossBeam = targetAbility as SorceressHealingCrossBeam;
		if (sorceressHealingCrossBeam != null)
		{
			AbilityMod.AddToken(tokens, this.m_normalDamageMod, "DamageAmount_Normal", string.Empty, sorceressHealingCrossBeam.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_singleTargetDamageMod, "DamageAmount_SingleTarget", string.Empty, sorceressHealingCrossBeam.m_damageAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyEffectOverride, "EnemyHitEffect", sorceressHealingCrossBeam.m_enemyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_normalHealingMod, "HealAmount_Normal", string.Empty, sorceressHealingCrossBeam.m_healAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_singleTargetHealingMod, "HealAmount_SingleTarget", string.Empty, sorceressHealingCrossBeam.m_healAmount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyEffectOverride, "AllyHitEffect", sorceressHealingCrossBeam.m_allyHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "Width", string.Empty, sorceressHealingCrossBeam.m_width, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserRangeMod, "Distance", string.Empty, sorceressHealingCrossBeam.m_distance, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserNumberMod, "NumLasers", string.Empty, sorceressHealingCrossBeam.m_numLasers, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SorceressHealingCrossBeam sorceressHealingCrossBeam = base.GetTargetAbilityOnAbilityData(abilityData) as SorceressHealingCrossBeam;
		bool flag = sorceressHealingCrossBeam != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyInt laserNumberMod = this.m_laserNumberMod;
		string prefix = "[Number of Lasers]";
		bool showBaseVal = flag;
		int baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_SorceressHealingCrossBeam.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = sorceressHealingCrossBeam.m_numLasers;
		}
		else
		{
			baseVal = 0;
		}
		text = str + AbilityModHelper.GetModPropertyDesc(laserNumberMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat laserWidthMod = this.m_laserWidthMod;
		string prefix2 = "[Laser Width]";
		bool showBaseVal2 = flag;
		float baseVal2;
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
			baseVal2 = sorceressHealingCrossBeam.m_width;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(laserWidthMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat laserRangeMod = this.m_laserRangeMod;
		string prefix3 = "[Laser Range]";
		bool showBaseVal3 = flag;
		float baseVal3;
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
			baseVal3 = sorceressHealingCrossBeam.m_distance;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(laserRangeMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt normalDamageMod = this.m_normalDamageMod;
		string prefix4 = "[Damage]";
		bool showBaseVal4 = flag;
		int baseVal4;
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
			baseVal4 = sorceressHealingCrossBeam.m_damageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(normalDamageMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt normalHealingMod = this.m_normalHealingMod;
		string prefix5 = "[Healing]";
		bool showBaseVal5 = flag;
		int baseVal5;
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
			baseVal5 = sorceressHealingCrossBeam.m_healAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + AbilityModHelper.GetModPropertyDesc(normalHealingMod, prefix5, showBaseVal5, baseVal5);
		if (this.m_useSingleTargetHitMods)
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
			string str6 = text;
			AbilityModPropertyInt singleTargetDamageMod = this.m_singleTargetDamageMod;
			string prefix6 = "[Damage if Target is only target in laser]";
			bool showBaseVal6 = flag;
			int baseVal6;
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
				baseVal6 = sorceressHealingCrossBeam.m_damageAmount;
			}
			else
			{
				baseVal6 = 0;
			}
			text = str6 + AbilityModHelper.GetModPropertyDesc(singleTargetDamageMod, prefix6, showBaseVal6, baseVal6);
			string str7 = text;
			AbilityModPropertyInt singleTargetHealingMod = this.m_singleTargetHealingMod;
			string prefix7 = "[Healing if Target is the only target in laser]";
			bool showBaseVal7 = flag;
			int baseVal7;
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
				baseVal7 = sorceressHealingCrossBeam.m_healAmount;
			}
			else
			{
				baseVal7 = 0;
			}
			text = str7 + AbilityModHelper.GetModPropertyDesc(singleTargetHealingMod, prefix7, showBaseVal7, baseVal7);
		}
		string str8 = text;
		AbilityModPropertyEffectInfo enemyEffectOverride = this.m_enemyEffectOverride;
		string prefix8 = "{ Enemy Hit Effect Override }";
		bool showBaseVal8 = flag;
		StandardEffectInfo baseVal8;
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
			baseVal8 = sorceressHealingCrossBeam.m_enemyHitEffect;
		}
		else
		{
			baseVal8 = null;
		}
		text = str8 + AbilityModHelper.GetModPropertyDesc(enemyEffectOverride, prefix8, showBaseVal8, baseVal8);
		string str9 = text;
		AbilityModPropertyEffectInfo allyEffectOverride = this.m_allyEffectOverride;
		string prefix9 = "{ Ally Hit Effect Override }";
		bool showBaseVal9 = flag;
		StandardEffectInfo baseVal9;
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
			baseVal9 = sorceressHealingCrossBeam.m_allyHitEffect;
		}
		else
		{
			baseVal9 = null;
		}
		text = str9 + AbilityModHelper.GetModPropertyDesc(allyEffectOverride, prefix9, showBaseVal9, baseVal9);
		if (this.m_knockbackDistance > 0f)
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
			string text2;
			if (this.m_knockbackThresholdDistance <= 0f)
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
				text2 = string.Empty;
			}
			else
			{
				text2 = "to Targets within " + this.m_knockbackThresholdDistance + " squares, ";
			}
			string text3 = text2;
			string text4 = text;
			text = string.Concat(new object[]
			{
				text4,
				"\nKnockback ",
				this.m_knockbackDistance,
				" squares, ",
				text3,
				"with type ",
				this.m_knockbackType,
				"\n"
			});
		}
		return text + AbilityModHelper.GetModGroundEffectInfoDesc(this.m_groundEffectOnEnemyHit, "{ Ground Effect on Enemy Hit }", flag, null);
	}
}
