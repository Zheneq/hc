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
		if (!(ninjaDarts != null))
		{
			return;
		}
		while (true)
		{
			switch (5)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken_LaserInfo(tokens, m_laserInfoMod, "LaserInfo", ninjaDarts.m_laserInfo);
			AbilityMod.AddToken(tokens, m_laserCountMod, "LaserCount", string.Empty, ninjaDarts.m_laserCount);
			AbilityMod.AddToken(tokens, m_angleInBetweenMod, "AngleInBetween", string.Empty, ninjaDarts.m_angleInBetween);
			AbilityMod.AddToken(tokens, m_targeterMinAngleMod, "TargeterMinAngle", string.Empty, ninjaDarts.m_targeterMinAngle);
			AbilityMod.AddToken(tokens, m_targeterMaxAngleMod, "TargeterMaxAngle", string.Empty, ninjaDarts.m_targeterMaxAngle);
			AbilityMod.AddToken(tokens, m_targeterMinInterpDistanceMod, "TargeterMinInterpDistance", string.Empty, ninjaDarts.m_targeterMinInterpDistance);
			AbilityMod.AddToken(tokens, m_targeterMaxInterpDistanceMod, "TargeterMaxInterpDistance", string.Empty, ninjaDarts.m_targeterMaxInterpDistance);
			AbilityMod.AddToken(tokens, m_damageMod, "Damage", string.Empty, ninjaDarts.m_damage);
			AbilityMod.AddToken(tokens, m_extraDamagePerSubseqHitMod, "ExtraDamagePerSubseqHit", string.Empty, ninjaDarts.m_extraDamagePerSubseqHit);
			AbilityMod.AddToken_EffectMod(tokens, m_enemySingleHitEffectMod, "EnemySingleHitEffect", ninjaDarts.m_enemySingleHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyMultiHitEffectMod, "EnemyMultiHitEffect", ninjaDarts.m_enemyMultiHitEffect);
			AbilityMod.AddToken(tokens, m_enemyExtraEffectHitCountMod, "EnemyExtraEffectHitCount", string.Empty, ninjaDarts.m_enemyExtraEffectHitCount);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyExtraHitEffectForHitCountMod, "EnemyExtraHitEffectForHitCount", ninjaDarts.m_enemyExtraHitEffectForHitCount);
			AbilityMod.AddToken_EffectMod(tokens, m_allySingleHitEffectMod, "AllySingleHitEffect", ninjaDarts.m_allySingleHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_allyMultiHitEffectMod, "AllyMultiHitEffect", ninjaDarts.m_allyMultiHitEffect);
			AbilityMod.AddToken(tokens, m_energyPerDartHitMod, "EnergyPerDartHit", string.Empty, ninjaDarts.m_energyPerDartHit);
			AbilityMod.AddToken(tokens, m_cdrOnMissMod, "CdrOnMiss", string.Empty, ninjaDarts.m_cdrOnMiss);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NinjaDarts ninjaDarts = GetTargetAbilityOnAbilityData(abilityData) as NinjaDarts;
		bool flag = ninjaDarts != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyLaserInfo laserInfoMod = m_laserInfoMod;
		object baseLaserInfo;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseLaserInfo = ninjaDarts.m_laserInfo;
		}
		else
		{
			baseLaserInfo = null;
		}
		empty = str + PropDesc(laserInfoMod, "[LaserInfo]", flag, (LaserTargetingInfo)baseLaserInfo);
		empty += PropDesc(m_laserCountMod, "[LaserCount]", flag, flag ? ninjaDarts.m_laserCount : 0);
		string str2 = empty;
		AbilityModPropertyFloat angleInBetweenMod = m_angleInBetweenMod;
		float baseVal;
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
			baseVal = ninjaDarts.m_angleInBetween;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str2 + PropDesc(angleInBetweenMod, "[AngleInBetween]", flag, baseVal);
		string str3 = empty;
		AbilityModPropertyBool changeAngleByCursorDistanceMod = m_changeAngleByCursorDistanceMod;
		int baseVal2;
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
			baseVal2 = (ninjaDarts.m_changeAngleByCursorDistance ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str3 + PropDesc(changeAngleByCursorDistanceMod, "[ChangeAngleByCursorDistance]", flag, (byte)baseVal2 != 0);
		string str4 = empty;
		AbilityModPropertyFloat targeterMinAngleMod = m_targeterMinAngleMod;
		float baseVal3;
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
			baseVal3 = ninjaDarts.m_targeterMinAngle;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str4 + PropDesc(targeterMinAngleMod, "[TargeterMinAngle]", flag, baseVal3);
		empty += PropDesc(m_targeterMaxAngleMod, "[TargeterMaxAngle]", flag, (!flag) ? 0f : ninjaDarts.m_targeterMaxAngle);
		string str5 = empty;
		AbilityModPropertyFloat targeterMinInterpDistanceMod = m_targeterMinInterpDistanceMod;
		float baseVal4;
		if (flag)
		{
			while (true)
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
		empty = str5 + PropDesc(targeterMinInterpDistanceMod, "[TargeterMinInterpDistance]", flag, baseVal4);
		string str6 = empty;
		AbilityModPropertyFloat targeterMaxInterpDistanceMod = m_targeterMaxInterpDistanceMod;
		float baseVal5;
		if (flag)
		{
			while (true)
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
		empty = str6 + PropDesc(targeterMaxInterpDistanceMod, "[TargeterMaxInterpDistance]", flag, baseVal5);
		string str7 = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal6;
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
			baseVal6 = ninjaDarts.m_damage;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str7 + PropDesc(damageMod, "[Damage]", flag, baseVal6);
		string str8 = empty;
		AbilityModPropertyInt extraDamagePerSubseqHitMod = m_extraDamagePerSubseqHitMod;
		int baseVal7;
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
			baseVal7 = ninjaDarts.m_extraDamagePerSubseqHit;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str8 + PropDesc(extraDamagePerSubseqHitMod, "[ExtraDamagePerSubseqHit]", flag, baseVal7);
		string str9 = empty;
		AbilityModPropertyEffectInfo enemySingleHitEffectMod = m_enemySingleHitEffectMod;
		object baseVal8;
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
			baseVal8 = ninjaDarts.m_enemySingleHitEffect;
		}
		else
		{
			baseVal8 = null;
		}
		empty = str9 + PropDesc(enemySingleHitEffectMod, "[EnemySingleHitEffect]", flag, (StandardEffectInfo)baseVal8);
		string str10 = empty;
		AbilityModPropertyEffectInfo enemyMultiHitEffectMod = m_enemyMultiHitEffectMod;
		object baseVal9;
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
			baseVal9 = ninjaDarts.m_enemyMultiHitEffect;
		}
		else
		{
			baseVal9 = null;
		}
		empty = str10 + PropDesc(enemyMultiHitEffectMod, "[EnemyMultiHitEffect]", flag, (StandardEffectInfo)baseVal9);
		empty += PropDesc(m_enemyExtraEffectHitCountMod, "[EnemyExtraEffectHitCount]", flag, flag ? ninjaDarts.m_enemyExtraEffectHitCount : 0);
		string str11 = empty;
		AbilityModPropertyEffectInfo enemyExtraHitEffectForHitCountMod = m_enemyExtraHitEffectForHitCountMod;
		object baseVal10;
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
			baseVal10 = ninjaDarts.m_enemyExtraHitEffectForHitCount;
		}
		else
		{
			baseVal10 = null;
		}
		empty = str11 + PropDesc(enemyExtraHitEffectForHitCountMod, "[EnemyExtraHitEffectForHitCount]", flag, (StandardEffectInfo)baseVal10);
		string str12 = empty;
		AbilityModPropertyEffectInfo allySingleHitEffectMod = m_allySingleHitEffectMod;
		object baseVal11;
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
			baseVal11 = ninjaDarts.m_allySingleHitEffect;
		}
		else
		{
			baseVal11 = null;
		}
		empty = str12 + PropDesc(allySingleHitEffectMod, "[AllySingleHitEffect]", flag, (StandardEffectInfo)baseVal11);
		string str13 = empty;
		AbilityModPropertyEffectInfo allyMultiHitEffectMod = m_allyMultiHitEffectMod;
		object baseVal12;
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
			baseVal12 = ninjaDarts.m_allyMultiHitEffect;
		}
		else
		{
			baseVal12 = null;
		}
		empty = str13 + PropDesc(allyMultiHitEffectMod, "[AllyMultiHitEffect]", flag, (StandardEffectInfo)baseVal12);
		string str14 = empty;
		AbilityModPropertyInt energyPerDartHitMod = m_energyPerDartHitMod;
		int baseVal13;
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
			baseVal13 = ninjaDarts.m_energyPerDartHit;
		}
		else
		{
			baseVal13 = 0;
		}
		empty = str14 + PropDesc(energyPerDartHitMod, "[EnergyPerDartHit]", flag, baseVal13);
		empty += PropDesc(m_cdrOnMissMod, "[CdrOnMiss]", flag, flag ? ninjaDarts.m_cdrOnMiss : 0);
		string str15 = empty;
		AbilityModPropertyBool applyDeathmarkEffectMod = m_applyDeathmarkEffectMod;
		int baseVal14;
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
			baseVal14 = (ninjaDarts.m_applyDeathmarkEffect ? 1 : 0);
		}
		else
		{
			baseVal14 = 0;
		}
		empty = str15 + PropDesc(applyDeathmarkEffectMod, "[ApplyDeathmarkEffect]", flag, (byte)baseVal14 != 0);
		string str16 = empty;
		AbilityModPropertyBool ignoreCoverOnTargetsMod = m_ignoreCoverOnTargetsMod;
		int baseVal15;
		if (flag)
		{
			while (true)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal15 = (ninjaDarts.m_ignoreCoverOnTargets ? 1 : 0);
		}
		else
		{
			baseVal15 = 0;
		}
		return str16 + PropDesc(ignoreCoverOnTargetsMod, "[IgnoreCoverOnTargets]", flag, (byte)baseVal15 != 0);
	}
}
