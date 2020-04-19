using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ClaymoreSilenceLaser : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_laserRangeMod;

	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyInt m_laserMaxTargetsMod;

	public AbilityModPropertyBool m_penetrateLosMod;

	[Header("-- Damage and Effect")]
	public AbilityModPropertyInt m_effectExplosionDamageMod;

	public AbilityModPropertyInt m_explosionDamageAfterFirstHitMod;

	public AbilityModPropertyEffectData m_enemyHitEffectDataMod;

	[Header("-- On Reaction Hit/Explosion")]
	public AbilityModPropertyInt m_onCastDamageAmountMod;

	public AbilityModPropertyBool m_explosionReduceCooldownOnlyIfHitByAllyMod;

	public AbilityModPropertyInt m_explosionCooldownReductionMod;

	public AbilityModPropertyBool m_canExplodeOncePerTurnMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ClaymoreSilenceLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ClaymoreSilenceLaser claymoreSilenceLaser = targetAbility as ClaymoreSilenceLaser;
		if (claymoreSilenceLaser != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ClaymoreSilenceLaser.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_laserRangeMod, "LaserRange", string.Empty, claymoreSilenceLaser.m_laserRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, claymoreSilenceLaser.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserMaxTargetsMod, "LaserMaxTargets", string.Empty, claymoreSilenceLaser.m_laserMaxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_onCastDamageAmountMod, "OnCastDamageAmount", string.Empty, claymoreSilenceLaser.m_onCastDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_effectExplosionDamageMod, "EffectExplosionDamage", string.Empty, claymoreSilenceLaser.m_effectExplosionDamage, true, false);
			AbilityMod.AddToken(tokens, this.m_explosionDamageAfterFirstHitMod, "ExplosionDamageAfterFirstHit", string.Empty, claymoreSilenceLaser.m_explosionDamageAfterFirstHit, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyHitEffectDataMod, "EnemyHitEffectData", claymoreSilenceLaser.m_enemyHitEffectData, true);
			AbilityMod.AddToken(tokens, this.m_explosionCooldownReductionMod, "ExplosionCooldownReduction", string.Empty, claymoreSilenceLaser.m_explosionCooldownReduction, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ClaymoreSilenceLaser claymoreSilenceLaser = base.GetTargetAbilityOnAbilityData(abilityData) as ClaymoreSilenceLaser;
		bool flag = claymoreSilenceLaser != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat laserRangeMod = this.m_laserRangeMod;
		string prefix = "[Laser Range]";
		bool showBaseVal = flag;
		float baseVal;
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
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_ClaymoreSilenceLaser.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = claymoreSilenceLaser.m_laserRange;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(laserRangeMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_laserWidthMod, "[Laser Width]", flag, (!flag) ? 0f : claymoreSilenceLaser.m_laserWidth);
		string str2 = text;
		AbilityModPropertyInt laserMaxTargetsMod = this.m_laserMaxTargetsMod;
		string prefix2 = "[Laser Max Targets]";
		bool showBaseVal2 = flag;
		int baseVal2;
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
			baseVal2 = claymoreSilenceLaser.m_laserMaxTargets;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(laserMaxTargetsMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyBool penetrateLosMod = this.m_penetrateLosMod;
		string prefix3 = "[Penetrate Los]";
		bool showBaseVal3 = flag;
		bool baseVal3;
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
			baseVal3 = claymoreSilenceLaser.m_penetrateLos;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + base.PropDesc(penetrateLosMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt onCastDamageAmountMod = this.m_onCastDamageAmountMod;
		string prefix4 = "[On Cast Damage Amount]";
		bool showBaseVal4 = flag;
		int baseVal4;
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
			baseVal4 = claymoreSilenceLaser.m_onCastDamageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(onCastDamageAmountMod, prefix4, showBaseVal4, baseVal4);
		text += base.PropDesc(this.m_enemyHitEffectDataMod, "[Enemy Hit Effect Data]", flag, (!flag) ? null : claymoreSilenceLaser.m_enemyHitEffectData);
		string str5 = text;
		AbilityModPropertyInt effectExplosionDamageMod = this.m_effectExplosionDamageMod;
		string prefix5 = "[Effect Explosion Damage]";
		bool showBaseVal5 = flag;
		int baseVal5;
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
			baseVal5 = claymoreSilenceLaser.m_effectExplosionDamage;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + base.PropDesc(effectExplosionDamageMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt explosionDamageAfterFirstHitMod = this.m_explosionDamageAfterFirstHitMod;
		string prefix6 = "[ExplosionDamageAfterFirstHit]";
		bool showBaseVal6 = flag;
		int baseVal6;
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
			baseVal6 = claymoreSilenceLaser.m_explosionDamageAfterFirstHit;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(explosionDamageAfterFirstHitMod, prefix6, showBaseVal6, baseVal6);
		string str7 = text;
		AbilityModPropertyBool explosionReduceCooldownOnlyIfHitByAllyMod = this.m_explosionReduceCooldownOnlyIfHitByAllyMod;
		string prefix7 = "[ExplosionReduceCooldownOnlyIfHitByAlly]";
		bool showBaseVal7 = flag;
		bool baseVal7;
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
			baseVal7 = claymoreSilenceLaser.m_explosionReduceCooldownOnlyIfHitByAlly;
		}
		else
		{
			baseVal7 = false;
		}
		text = str7 + base.PropDesc(explosionReduceCooldownOnlyIfHitByAllyMod, prefix7, showBaseVal7, baseVal7);
		string str8 = text;
		AbilityModPropertyInt explosionCooldownReductionMod = this.m_explosionCooldownReductionMod;
		string prefix8 = "[ExplosionCooldownReduction]";
		bool showBaseVal8 = flag;
		int baseVal8;
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
			baseVal8 = claymoreSilenceLaser.m_explosionCooldownReduction;
		}
		else
		{
			baseVal8 = 0;
		}
		text = str8 + base.PropDesc(explosionCooldownReductionMod, prefix8, showBaseVal8, baseVal8);
		return text + base.PropDesc(this.m_canExplodeOncePerTurnMod, "[ExplodeOncePerTurn]", flag, false);
	}
}
