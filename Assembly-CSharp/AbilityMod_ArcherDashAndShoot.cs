using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ArcherDashAndShoot : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_maxAngleForLaserMod;

	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_laserRangeMod;

	public AbilityModPropertyFloat m_aoeRadiusMod;

	public AbilityModPropertyBool m_aoePenetratesLoSMod;

	[Header("-- Enemy hits")]
	public AbilityModPropertyInt m_directDamageMod;

	public AbilityModPropertyInt m_aoeDamageMod;

	public AbilityModPropertyEffectInfo m_directTargetEffectMod;

	public AbilityModPropertyEffectInfo m_aoeTargetEffectMod;

	[Header("-- Misc ability interactions")]
	[Tooltip("if the target has the HealingDebuff effect, apply this effect to them also")]
	public AbilityModPropertyEffectInfo m_healingDebuffTargetEffect;

	public AbilityModPropertyInt m_cooldownAdjustmentEachTurnUnderHealthThreshold;

	public AbilityModPropertyFloat m_healthThresholdForCooldownOverride;

	public override Type GetTargetAbilityType()
	{
		return typeof(ArcherDashAndShoot);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ArcherDashAndShoot archerDashAndShoot = targetAbility as ArcherDashAndShoot;
		if (!(archerDashAndShoot != null))
		{
			return;
		}
		while (true)
		{
			switch (6)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_maxAngleForLaserMod, "MaxAngleForLaser", string.Empty, archerDashAndShoot.m_maxAngleForLaser);
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, archerDashAndShoot.m_laserWidth);
			AbilityMod.AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, archerDashAndShoot.m_laserRange);
			AbilityMod.AddToken(tokens, m_aoeRadiusMod, "AoeRadius", string.Empty, archerDashAndShoot.m_aoeRadius);
			AbilityMod.AddToken(tokens, m_directDamageMod, "DirectDamage", string.Empty, archerDashAndShoot.m_directDamage);
			AbilityMod.AddToken(tokens, m_aoeDamageMod, "AoeDamage", string.Empty, archerDashAndShoot.m_aoeDamage);
			AbilityMod.AddToken_EffectMod(tokens, m_directTargetEffectMod, "DirectTargetEffect", archerDashAndShoot.m_directTargetEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_aoeTargetEffectMod, "AoeTargetEffect", archerDashAndShoot.m_aoeTargetEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_healingDebuffTargetEffect, "HealingDebuffTargetEffect");
			AbilityMod.AddToken(tokens, m_cooldownAdjustmentEachTurnUnderHealthThreshold, "CooldownAdjustmentEachTurnUnderHealthThreshold", string.Empty, archerDashAndShoot.m_cooldown);
			AbilityMod.AddToken(tokens, m_healthThresholdForCooldownOverride, "HealthThresholdForCooldownOverride", string.Empty, 0f, false, false, true);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ArcherDashAndShoot archerDashAndShoot = GetTargetAbilityOnAbilityData(abilityData) as ArcherDashAndShoot;
		bool flag = archerDashAndShoot != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat maxAngleForLaserMod = m_maxAngleForLaserMod;
		float baseVal;
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
			baseVal = archerDashAndShoot.m_maxAngleForLaser;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(maxAngleForLaserMod, "[MaxAngleForLaser]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat laserWidthMod = m_laserWidthMod;
		float baseVal2;
		if (flag)
		{
			while (true)
			{
				switch (4)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = archerDashAndShoot.m_laserWidth;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(laserWidthMod, "[LaserWidth]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat laserRangeMod = m_laserRangeMod;
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
			baseVal3 = archerDashAndShoot.m_laserRange;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(laserRangeMod, "[LaserRange]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat aoeRadiusMod = m_aoeRadiusMod;
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
			baseVal4 = archerDashAndShoot.m_aoeRadius;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(aoeRadiusMod, "[AoeRadius]", flag, baseVal4);
		empty += PropDesc(m_aoePenetratesLoSMod, "[AoePenetratesLoS]", flag, flag && archerDashAndShoot.m_aoePenetratesLoS);
		string str5 = empty;
		AbilityModPropertyInt directDamageMod = m_directDamageMod;
		int baseVal5;
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
			baseVal5 = archerDashAndShoot.m_directDamage;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(directDamageMod, "[DirectDamage]", flag, baseVal5);
		empty += PropDesc(m_aoeDamageMod, "[AoeDamage]", flag, flag ? archerDashAndShoot.m_aoeDamage : 0);
		string str6 = empty;
		AbilityModPropertyEffectInfo directTargetEffectMod = m_directTargetEffectMod;
		object baseVal6;
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
			baseVal6 = archerDashAndShoot.m_directTargetEffect;
		}
		else
		{
			baseVal6 = null;
		}
		empty = str6 + PropDesc(directTargetEffectMod, "[DirectTargetEffect]", flag, (StandardEffectInfo)baseVal6);
		empty += PropDesc(m_aoeTargetEffectMod, "[AoeTargetEffect]", flag, (!flag) ? null : archerDashAndShoot.m_aoeTargetEffect);
		empty += PropDesc(m_healingDebuffTargetEffect, "[HealingDebuffTargetEffect]");
		string str7 = empty;
		AbilityModPropertyInt cooldownAdjustmentEachTurnUnderHealthThreshold = m_cooldownAdjustmentEachTurnUnderHealthThreshold;
		int baseVal7;
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
			baseVal7 = archerDashAndShoot.m_cooldown;
		}
		else
		{
			baseVal7 = 0;
		}
		empty = str7 + PropDesc(cooldownAdjustmentEachTurnUnderHealthThreshold, "[CooldownAdjustmentEachTurnUnderHealthThreshold]", flag, baseVal7);
		return empty + PropDesc(m_healthThresholdForCooldownOverride, "[HealthThresholdForCooldownOverride]", flag);
	}
}
