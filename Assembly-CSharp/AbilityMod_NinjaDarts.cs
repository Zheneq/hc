using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NinjaDarts : AbilityMod
{
	[Separator("Targeting Properties", true)]
	public AbilityModPropertyLaserInfo m_laserInfoMod;

	[Space(10f)]
	public AbilityModPropertyInt m_laserCountMod;

	public AbilityModPropertyFloat m_angleInBetweenMod;

	public AbilityModPropertyBool m_changeAngleByCursorDistanceMod;

	public AbilityModPropertyFloat m_targeterMinAngleMod;

	public AbilityModPropertyFloat m_targeterMaxAngleMod;

	public AbilityModPropertyFloat m_targeterMinInterpDistanceMod;

	public AbilityModPropertyFloat m_targeterMaxInterpDistanceMod;

	[Separator("On Hit Stuff", true)]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyInt m_extraDamagePerSubseqHitMod;

	public AbilityModPropertyFloat m_damageMultPerSubseqHitMod;

	public AbilityModPropertyBool m_changeEachSubseqDamageMod;

	[Space(10f)]
	public AbilityModPropertyEffectInfo m_enemySingleHitEffectMod;

	public AbilityModPropertyEffectInfo m_enemyMultiHitEffectMod;

	[Header("-- For effect when hitting over certain number of lasers --")]
	public AbilityModPropertyInt m_enemyExtraEffectHitCountMod;

	public AbilityModPropertyEffectInfo m_enemyExtraHitEffectForHitCountMod;

	[Header("-- For Ally Hit --")]
	public AbilityModPropertyEffectInfo m_allySingleHitEffectMod;

	public AbilityModPropertyEffectInfo m_allyMultiHitEffectMod;

	[Separator("Energy per dart hit", true)]
	public AbilityModPropertyInt m_energyPerDartHitMod;

	[Separator("Cooldown Reduction", true)]
	public AbilityModPropertyInt m_cdrOnMissMod;

	[Separator("[Deathmark] Effect", "magenta")]
	public AbilityModPropertyBool m_applyDeathmarkEffectMod;

	public AbilityModPropertyBool m_ignoreCoverOnTargetsMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NinjaDarts);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NinjaDarts ninjaDarts = targetAbility as NinjaDarts;
		if (ninjaDarts != null)
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NinjaDarts.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken_LaserInfo(tokens, this.m_laserInfoMod, "LaserInfo", ninjaDarts.m_laserInfo, true);
			AbilityMod.AddToken(tokens, this.m_laserCountMod, "LaserCount", string.Empty, ninjaDarts.m_laserCount, true, false);
			AbilityMod.AddToken(tokens, this.m_angleInBetweenMod, "AngleInBetween", string.Empty, ninjaDarts.m_angleInBetween, true, false, false);
			AbilityMod.AddToken(tokens, this.m_targeterMinAngleMod, "TargeterMinAngle", string.Empty, ninjaDarts.m_targeterMinAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_targeterMaxAngleMod, "TargeterMaxAngle", string.Empty, ninjaDarts.m_targeterMaxAngle, true, false, false);
			AbilityMod.AddToken(tokens, this.m_targeterMinInterpDistanceMod, "TargeterMinInterpDistance", string.Empty, ninjaDarts.m_targeterMinInterpDistance, true, false, false);
			AbilityMod.AddToken(tokens, this.m_targeterMaxInterpDistanceMod, "TargeterMaxInterpDistance", string.Empty, ninjaDarts.m_targeterMaxInterpDistance, true, false, false);
			AbilityMod.AddToken(tokens, this.m_damageMod, "Damage", string.Empty, ninjaDarts.m_damage, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamagePerSubseqHitMod, "ExtraDamagePerSubseqHit", string.Empty, ninjaDarts.m_extraDamagePerSubseqHit, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemySingleHitEffectMod, "EnemySingleHitEffect", ninjaDarts.m_enemySingleHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyMultiHitEffectMod, "EnemyMultiHitEffect", ninjaDarts.m_enemyMultiHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_enemyExtraEffectHitCountMod, "EnemyExtraEffectHitCount", string.Empty, ninjaDarts.m_enemyExtraEffectHitCount, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_enemyExtraHitEffectForHitCountMod, "EnemyExtraHitEffectForHitCount", ninjaDarts.m_enemyExtraHitEffectForHitCount, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allySingleHitEffectMod, "AllySingleHitEffect", ninjaDarts.m_allySingleHitEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_allyMultiHitEffectMod, "AllyMultiHitEffect", ninjaDarts.m_allyMultiHitEffect, true);
			AbilityMod.AddToken(tokens, this.m_energyPerDartHitMod, "EnergyPerDartHit", string.Empty, ninjaDarts.m_energyPerDartHit, true, false);
			AbilityMod.AddToken(tokens, this.m_cdrOnMissMod, "CdrOnMiss", string.Empty, ninjaDarts.m_cdrOnMiss, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NinjaDarts ninjaDarts = base.GetTargetAbilityOnAbilityData(abilityData) as NinjaDarts;
		bool flag = ninjaDarts != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyLaserInfo laserInfoMod = this.m_laserInfoMod;
		string prefix = "[LaserInfo]";
		bool showBaseVal = flag;
		LaserTargetingInfo baseLaserInfo;
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
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_NinjaDarts.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseLaserInfo = ninjaDarts.m_laserInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		text = str + base.PropDesc(laserInfoMod, prefix, showBaseVal, baseLaserInfo);
		text += base.PropDesc(this.m_laserCountMod, "[LaserCount]", flag, (!flag) ? 0 : ninjaDarts.m_laserCount);
		string str2 = text;
		AbilityModPropertyFloat angleInBetweenMod = this.m_angleInBetweenMod;
		string prefix2 = "[AngleInBetween]";
		bool showBaseVal2 = flag;
		float baseVal;
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
			baseVal = ninjaDarts.m_angleInBetween;
		}
		else
		{
			baseVal = 0f;
		}
		text = str2 + base.PropDesc(angleInBetweenMod, prefix2, showBaseVal2, baseVal);
		string str3 = text;
		AbilityModPropertyBool changeAngleByCursorDistanceMod = this.m_changeAngleByCursorDistanceMod;
		string prefix3 = "[ChangeAngleByCursorDistance]";
		bool showBaseVal3 = flag;
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
			baseVal2 = ninjaDarts.m_changeAngleByCursorDistance;
		}
		else
		{
			baseVal2 = false;
		}
		text = str3 + base.PropDesc(changeAngleByCursorDistanceMod, prefix3, showBaseVal3, baseVal2);
		string str4 = text;
		AbilityModPropertyFloat targeterMinAngleMod = this.m_targeterMinAngleMod;
		string prefix4 = "[TargeterMinAngle]";
		bool showBaseVal4 = flag;
		float baseVal3;
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
			baseVal3 = ninjaDarts.m_targeterMinAngle;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str4 + base.PropDesc(targeterMinAngleMod, prefix4, showBaseVal4, baseVal3);
		text += base.PropDesc(this.m_targeterMaxAngleMod, "[TargeterMaxAngle]", flag, (!flag) ? 0f : ninjaDarts.m_targeterMaxAngle);
		string str5 = text;
		AbilityModPropertyFloat targeterMinInterpDistanceMod = this.m_targeterMinInterpDistanceMod;
		string prefix5 = "[TargeterMinInterpDistance]";
		bool showBaseVal5 = flag;
		float baseVal4;
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
			baseVal4 = ninjaDarts.m_targeterMinInterpDistance;
		}
		else
		{
			baseVal4 = 0f;
		}
		text = str5 + base.PropDesc(targeterMinInterpDistanceMod, prefix5, showBaseVal5, baseVal4);
		string str6 = text;
		AbilityModPropertyFloat targeterMaxInterpDistanceMod = this.m_targeterMaxInterpDistanceMod;
		string prefix6 = "[TargeterMaxInterpDistance]";
		bool showBaseVal6 = flag;
		float baseVal5;
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
			baseVal5 = ninjaDarts.m_targeterMaxInterpDistance;
		}
		else
		{
			baseVal5 = 0f;
		}
		text = str6 + base.PropDesc(targeterMaxInterpDistanceMod, prefix6, showBaseVal6, baseVal5);
		string str7 = text;
		AbilityModPropertyInt damageMod = this.m_damageMod;
		string prefix7 = "[Damage]";
		bool showBaseVal7 = flag;
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
			baseVal6 = ninjaDarts.m_damage;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str7 + base.PropDesc(damageMod, prefix7, showBaseVal7, baseVal6);
		string str8 = text;
		AbilityModPropertyInt extraDamagePerSubseqHitMod = this.m_extraDamagePerSubseqHitMod;
		string prefix8 = "[ExtraDamagePerSubseqHit]";
		bool showBaseVal8 = flag;
		int baseVal7;
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
			baseVal7 = ninjaDarts.m_extraDamagePerSubseqHit;
		}
		else
		{
			baseVal7 = 0;
		}
		text = str8 + base.PropDesc(extraDamagePerSubseqHitMod, prefix8, showBaseVal8, baseVal7);
		string str9 = text;
		AbilityModPropertyEffectInfo enemySingleHitEffectMod = this.m_enemySingleHitEffectMod;
		string prefix9 = "[EnemySingleHitEffect]";
		bool showBaseVal9 = flag;
		StandardEffectInfo baseVal8;
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
			baseVal8 = ninjaDarts.m_enemySingleHitEffect;
		}
		else
		{
			baseVal8 = null;
		}
		text = str9 + base.PropDesc(enemySingleHitEffectMod, prefix9, showBaseVal9, baseVal8);
		string str10 = text;
		AbilityModPropertyEffectInfo enemyMultiHitEffectMod = this.m_enemyMultiHitEffectMod;
		string prefix10 = "[EnemyMultiHitEffect]";
		bool showBaseVal10 = flag;
		StandardEffectInfo baseVal9;
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
			baseVal9 = ninjaDarts.m_enemyMultiHitEffect;
		}
		else
		{
			baseVal9 = null;
		}
		text = str10 + base.PropDesc(enemyMultiHitEffectMod, prefix10, showBaseVal10, baseVal9);
		text += base.PropDesc(this.m_enemyExtraEffectHitCountMod, "[EnemyExtraEffectHitCount]", flag, (!flag) ? 0 : ninjaDarts.m_enemyExtraEffectHitCount);
		string str11 = text;
		AbilityModPropertyEffectInfo enemyExtraHitEffectForHitCountMod = this.m_enemyExtraHitEffectForHitCountMod;
		string prefix11 = "[EnemyExtraHitEffectForHitCount]";
		bool showBaseVal11 = flag;
		StandardEffectInfo baseVal10;
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
			baseVal10 = ninjaDarts.m_enemyExtraHitEffectForHitCount;
		}
		else
		{
			baseVal10 = null;
		}
		text = str11 + base.PropDesc(enemyExtraHitEffectForHitCountMod, prefix11, showBaseVal11, baseVal10);
		string str12 = text;
		AbilityModPropertyEffectInfo allySingleHitEffectMod = this.m_allySingleHitEffectMod;
		string prefix12 = "[AllySingleHitEffect]";
		bool showBaseVal12 = flag;
		StandardEffectInfo baseVal11;
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
			baseVal11 = ninjaDarts.m_allySingleHitEffect;
		}
		else
		{
			baseVal11 = null;
		}
		text = str12 + base.PropDesc(allySingleHitEffectMod, prefix12, showBaseVal12, baseVal11);
		string str13 = text;
		AbilityModPropertyEffectInfo allyMultiHitEffectMod = this.m_allyMultiHitEffectMod;
		string prefix13 = "[AllyMultiHitEffect]";
		bool showBaseVal13 = flag;
		StandardEffectInfo baseVal12;
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
			baseVal12 = ninjaDarts.m_allyMultiHitEffect;
		}
		else
		{
			baseVal12 = null;
		}
		text = str13 + base.PropDesc(allyMultiHitEffectMod, prefix13, showBaseVal13, baseVal12);
		string str14 = text;
		AbilityModPropertyInt energyPerDartHitMod = this.m_energyPerDartHitMod;
		string prefix14 = "[EnergyPerDartHit]";
		bool showBaseVal14 = flag;
		int baseVal13;
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
			baseVal13 = ninjaDarts.m_energyPerDartHit;
		}
		else
		{
			baseVal13 = 0;
		}
		text = str14 + base.PropDesc(energyPerDartHitMod, prefix14, showBaseVal14, baseVal13);
		text += base.PropDesc(this.m_cdrOnMissMod, "[CdrOnMiss]", flag, (!flag) ? 0 : ninjaDarts.m_cdrOnMiss);
		string str15 = text;
		AbilityModPropertyBool applyDeathmarkEffectMod = this.m_applyDeathmarkEffectMod;
		string prefix15 = "[ApplyDeathmarkEffect]";
		bool showBaseVal15 = flag;
		bool baseVal14;
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
			baseVal14 = ninjaDarts.m_applyDeathmarkEffect;
		}
		else
		{
			baseVal14 = false;
		}
		text = str15 + base.PropDesc(applyDeathmarkEffectMod, prefix15, showBaseVal15, baseVal14);
		string str16 = text;
		AbilityModPropertyBool ignoreCoverOnTargetsMod = this.m_ignoreCoverOnTargetsMod;
		string prefix16 = "[IgnoreCoverOnTargets]";
		bool showBaseVal16 = flag;
		bool baseVal15;
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
			baseVal15 = ninjaDarts.m_ignoreCoverOnTargets;
		}
		else
		{
			baseVal15 = false;
		}
		return str16 + base.PropDesc(ignoreCoverOnTargetsMod, prefix16, showBaseVal16, baseVal15);
	}
}
