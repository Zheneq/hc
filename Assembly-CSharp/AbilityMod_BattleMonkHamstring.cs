using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BattleMonkHamstring : AbilityMod
{
	[Header("-- Laser Damage and Targeting Mod")]
	public AbilityModPropertyInt m_laserDamageMod;

	public AbilityModPropertyInt m_damageAfterFirstHitMod;

	[Space(10f)]
	public AbilityModPropertyFloat m_widthMod;

	public AbilityModPropertyFloat m_rangeMod;

	public AbilityModPropertyInt m_maxTargetMod;

	public bool m_useLaserHitEffectOverride;

	public StandardEffectInfo m_laserHitEffectOverride;

	[Header("-- Laser Explosion, Damage and Effects")]
	public AbilityModPropertyInt m_explosionDamageMod;

	public AbilityModPropertyBool m_explodeOnActorHitMod;

	public AbilityModPropertyShape m_explodeShapeMod;

	public bool m_useExplosionHitEffectOverride;

	public StandardEffectInfo m_explosionHitEffectOverride;

	[Header("-- Laser Bounce")]
	public AbilityModPropertyInt m_maxBounces;

	public AbilityModPropertyFloat m_distancePerBounce;

	public AbilityModPropertySequenceOverride m_projectileSequencePrefab;

	public override Type GetTargetAbilityType()
	{
		return typeof(BattleMonkHamstring);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BattleMonkHamstring battleMonkHamstring = targetAbility as BattleMonkHamstring;
		if (battleMonkHamstring != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_BattleMonkHamstring.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_laserDamageMod, "LaserDamageAmount", string.Empty, battleMonkHamstring.m_laserDamageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_damageAfterFirstHitMod, "DamageAfterFirstHit", string.Empty, battleMonkHamstring.m_damageAfterFirstHit, true, false);
			AbilityMod.AddToken(tokens, this.m_widthMod, "LaserInfo_Width", string.Empty, battleMonkHamstring.m_laserInfo.width, true, false, false);
			AbilityMod.AddToken(tokens, this.m_rangeMod, "LaserInfo_Range", string.Empty, battleMonkHamstring.m_laserInfo.range, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetMod, "LaserInfo_MaxTargets", string.Empty, battleMonkHamstring.m_laserInfo.maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_explosionDamageMod, "ExplosionDamageAmount", string.Empty, battleMonkHamstring.m_explosionDamageAmount, true, false);
			if (this.m_useLaserHitEffectOverride)
			{
				AbilityMod.AddToken_EffectInfo(tokens, this.m_laserHitEffectOverride, "LaserHitEffect", battleMonkHamstring.m_laserHitEffect, true);
			}
			if (this.m_useExplosionHitEffectOverride)
			{
				AbilityMod.AddToken_EffectInfo(tokens, this.m_explosionHitEffectOverride, "ExplosionHitEffect", battleMonkHamstring.m_explosionHitEffect, true);
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BattleMonkHamstring battleMonkHamstring = base.GetTargetAbilityOnAbilityData(abilityData) as BattleMonkHamstring;
		bool flag = battleMonkHamstring != null;
		string text = string.Empty;
		text += AbilityModHelper.GetModPropertyDesc(this.m_laserDamageMod, "[Laser Damage]", flag, (!flag) ? 0 : battleMonkHamstring.m_laserDamageAmount);
		string str = text;
		AbilityModPropertyInt damageAfterFirstHitMod = this.m_damageAfterFirstHitMod;
		string prefix = "[DamageAfterFirstHit]";
		bool showBaseVal = flag;
		int baseVal;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_BattleMonkHamstring.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = battleMonkHamstring.m_damageAfterFirstHit;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(damageAfterFirstHitMod, prefix, showBaseVal, baseVal);
		if (this.m_useLaserHitEffectOverride)
		{
			text += "Overrideing Laser Hit Effect\n";
			text += AbilityModHelper.GetModEffectInfoDesc(this.m_laserHitEffectOverride, "{ Effect Override On Laser Hit }", string.Empty, flag, (!flag) ? null : battleMonkHamstring.m_laserHitEffect);
		}
		string str2 = text;
		AbilityModPropertyFloat widthMod = this.m_widthMod;
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
			baseVal2 = battleMonkHamstring.m_laserInfo.width;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + AbilityModHelper.GetModPropertyDesc(widthMod, prefix2, showBaseVal2, baseVal2);
		text += AbilityModHelper.GetModPropertyDesc(this.m_rangeMod, "[Laser Range]", flag, (!flag) ? 0f : battleMonkHamstring.m_laserInfo.range);
		string str3 = text;
		AbilityModPropertyInt maxTargetMod = this.m_maxTargetMod;
		string prefix3 = "[Max Targets]";
		bool showBaseVal3 = flag;
		int baseVal3;
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
			baseVal3 = battleMonkHamstring.m_laserInfo.maxTargets;
		}
		else
		{
			baseVal3 = 0;
		}
		text = str3 + AbilityModHelper.GetModPropertyDesc(maxTargetMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyBool explodeOnActorHitMod = this.m_explodeOnActorHitMod;
		string prefix4 = "[Should Explode On Actor Hit?]";
		bool showBaseVal4 = flag;
		bool baseVal4;
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
			baseVal4 = battleMonkHamstring.m_explodeOnActorHit;
		}
		else
		{
			baseVal4 = false;
		}
		text = str4 + AbilityModHelper.GetModPropertyDesc(explodeOnActorHitMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt explosionDamageMod = this.m_explosionDamageMod;
		string prefix5 = "[Explosion Damage]";
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
			baseVal5 = battleMonkHamstring.m_explosionDamageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		text = str5 + AbilityModHelper.GetModPropertyDesc(explosionDamageMod, prefix5, showBaseVal5, baseVal5);
		if (this.m_useExplosionHitEffectOverride)
		{
			text += "Overriding Explosion Hit Effect\n";
			string str6 = text;
			StandardEffectInfo explosionHitEffectOverride = this.m_explosionHitEffectOverride;
			string prefix6 = "{ Effect Override on Explosion Hit }";
			string empty = string.Empty;
			bool useBaseVal = flag;
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
				baseVal6 = battleMonkHamstring.m_explosionHitEffect;
			}
			else
			{
				baseVal6 = null;
			}
			text = str6 + AbilityModHelper.GetModEffectInfoDesc(explosionHitEffectOverride, prefix6, empty, useBaseVal, baseVal6);
		}
		string str7 = text;
		AbilityModPropertyShape explodeShapeMod = this.m_explodeShapeMod;
		string prefix7 = "[Explode Shape]";
		bool showBaseVal6 = flag;
		AbilityAreaShape baseVal7;
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
			baseVal7 = battleMonkHamstring.m_explodeShape;
		}
		else
		{
			baseVal7 = AbilityAreaShape.SingleSquare;
		}
		text = str7 + AbilityModHelper.GetModPropertyDesc(explodeShapeMod, prefix7, showBaseVal6, baseVal7);
		text += base.PropDesc(this.m_maxBounces, "[Max Laser Bounces]", flag, 0);
		text += base.PropDesc(this.m_distancePerBounce, "[Distance Per Bounce]", flag, 0f);
		if (this.m_projectileSequencePrefab != null)
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
			if (this.m_projectileSequencePrefab.operation == AbilityModPropertySequenceOverride.ModOp.Override)
			{
				string text2 = string.Empty;
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
					if (battleMonkHamstring.m_projectileSequencePrefab != null)
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
						text2 = " (base was " + battleMonkHamstring.m_projectileSequencePrefab.name + ")";
					}
				}
				string text3 = text;
				text = string.Concat(new string[]
				{
					text3,
					"[Projectile Sequence Override] = ",
					this.m_projectileSequencePrefab.value.name,
					text2,
					"\n"
				});
			}
		}
		return text;
	}
}
