using System;
using System.Collections.Generic;

public class AbilityMod_ArcherArrowRain : AbilityMod
{
	[Separator("Targeting Info", true)]
	public AbilityModPropertyFloat m_startRadiusMod;

	public AbilityModPropertyFloat m_endRadiusMod;

	public AbilityModPropertyFloat m_lineRadiusMod;

	public AbilityModPropertyFloat m_minRangeBetweenMod;

	public AbilityModPropertyFloat m_maxRangeBetweenMod;

	public AbilityModPropertyBool m_linePenetrateLoSMod;

	public AbilityModPropertyBool m_aoePenetrateLoSMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	[Separator("Enemy Hit", true)]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyInt m_damageBelowHealthThresholdMod;

	public AbilityModPropertyFloat m_healthThresholdForDamageMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectMod;

	public AbilityModPropertyEffectInfo m_additionalEnemyHitEffect;

	public AbilityModPropertyEffectInfo m_singleEnemyHitEffectMod;

	public AbilityModPropertyInt m_techPointRefundNoHits;

	public override Type GetTargetAbilityType()
	{
		return typeof(ArcherArrowRain);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ArcherArrowRain archerArrowRain = targetAbility as ArcherArrowRain;
		if (archerArrowRain != null)
		{
			AbilityMod.AddToken(tokens, m_startRadiusMod, "StartRadius", string.Empty, archerArrowRain.m_startRadius);
			AbilityMod.AddToken(tokens, m_endRadiusMod, "EndRadius", string.Empty, archerArrowRain.m_endRadius);
			AbilityMod.AddToken(tokens, m_lineRadiusMod, "LineRadius", string.Empty, archerArrowRain.m_lineRadius);
			AbilityMod.AddToken(tokens, m_minRangeBetweenMod, "MinRangeBetween", string.Empty, archerArrowRain.m_minRangeBetween);
			AbilityMod.AddToken(tokens, m_maxRangeBetweenMod, "MaxRangeBetween", string.Empty, archerArrowRain.m_maxRangeBetween);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, archerArrowRain.m_maxTargets);
			AbilityMod.AddToken(tokens, m_damageMod, "Damage", string.Empty, archerArrowRain.m_damage);
			AbilityMod.AddToken(tokens, m_damageBelowHealthThresholdMod, "DamageBelowHealthThreshold", string.Empty, archerArrowRain.m_damage);
			AbilityMod.AddToken(tokens, m_healthThresholdForDamageMod, "HealthThresholdForBonusDamage", string.Empty, 0f, true, false, true);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectMod, "EnemyHitEffect", archerArrowRain.m_enemyHitEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_additionalEnemyHitEffect, "AdditionalEnemyHitEffect");
			AbilityMod.AddToken_EffectMod(tokens, m_singleEnemyHitEffectMod, "SingleEnemyHitEffect");
			AbilityMod.AddToken(tokens, m_techPointRefundNoHits, "EnergyRefundIfNoTargetsHit", string.Empty, 0);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ArcherArrowRain archerArrowRain = GetTargetAbilityOnAbilityData(abilityData) as ArcherArrowRain;
		bool flag = archerArrowRain != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat startRadiusMod = m_startRadiusMod;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = archerArrowRain.m_startRadius;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(startRadiusMod, "[StartRadius]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat endRadiusMod = m_endRadiusMod;
		float baseVal2;
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
			baseVal2 = archerArrowRain.m_endRadius;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(endRadiusMod, "[EndRadius]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat lineRadiusMod = m_lineRadiusMod;
		float baseVal3;
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
			baseVal3 = archerArrowRain.m_lineRadius;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(lineRadiusMod, "[LineRadius]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat minRangeBetweenMod = m_minRangeBetweenMod;
		float baseVal4;
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
			baseVal4 = archerArrowRain.m_minRangeBetween;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(minRangeBetweenMod, "[MinRangeBetween]", flag, baseVal4);
		empty += PropDesc(m_maxRangeBetweenMod, "[MaxRangeBetween]", flag, (!flag) ? 0f : archerArrowRain.m_maxRangeBetween);
		empty += PropDesc(m_linePenetrateLoSMod, "[LinePenetrateLoS]", flag, flag && archerArrowRain.m_linePenetrateLoS);
		string str5 = empty;
		AbilityModPropertyBool aoePenetrateLoSMod = m_aoePenetrateLoSMod;
		int baseVal5;
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
			baseVal5 = (archerArrowRain.m_aoePenetrateLoS ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(aoePenetrateLoSMod, "[AoePenetrateLoS]", flag, (byte)baseVal5 != 0);
		empty += PropDesc(m_maxTargetsMod, "[MaxTargets]", flag, flag ? archerArrowRain.m_maxTargets : 0);
		empty += PropDesc(m_damageMod, "[Damage]", flag, flag ? archerArrowRain.m_damage : 0);
		string str6 = empty;
		AbilityModPropertyInt damageBelowHealthThresholdMod = m_damageBelowHealthThresholdMod;
		int baseVal6;
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
			baseVal6 = archerArrowRain.m_damage;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(damageBelowHealthThresholdMod, "[DamageBelowHealthThreshold]", flag, baseVal6);
		empty += PropDesc(m_healthThresholdForDamageMod, "[HealthThresholdForBonusDamage]", flag);
		string str7 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectMod = m_enemyHitEffectMod;
		object baseVal7;
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
			baseVal7 = archerArrowRain.m_enemyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + PropDesc(enemyHitEffectMod, "[EnemyHitEffect]", flag, (StandardEffectInfo)baseVal7);
		empty += PropDesc(m_additionalEnemyHitEffect, "[AdditionalEnemyHitEffect]");
		empty += PropDesc(m_singleEnemyHitEffectMod, "[SingleEnemyHitEffect]");
		return empty + PropDesc(m_techPointRefundNoHits, "[EnergyRefundIfNoTargetsHit]", flag);
	}
}
