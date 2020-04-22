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
			AbilityMod.AddToken(tokens, m_normalDamageMod, "DamageAmount_Normal", string.Empty, sorceressHealingCrossBeam.m_damageAmount);
			AbilityMod.AddToken(tokens, m_singleTargetDamageMod, "DamageAmount_SingleTarget", string.Empty, sorceressHealingCrossBeam.m_damageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyEffectOverride, "EnemyHitEffect", sorceressHealingCrossBeam.m_enemyHitEffect);
			AbilityMod.AddToken(tokens, m_normalHealingMod, "HealAmount_Normal", string.Empty, sorceressHealingCrossBeam.m_healAmount);
			AbilityMod.AddToken(tokens, m_singleTargetHealingMod, "HealAmount_SingleTarget", string.Empty, sorceressHealingCrossBeam.m_healAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_allyEffectOverride, "AllyHitEffect", sorceressHealingCrossBeam.m_allyHitEffect);
			AbilityMod.AddToken(tokens, m_laserWidthMod, "Width", string.Empty, sorceressHealingCrossBeam.m_width);
			AbilityMod.AddToken(tokens, m_laserRangeMod, "Distance", string.Empty, sorceressHealingCrossBeam.m_distance);
			AbilityMod.AddToken(tokens, m_laserNumberMod, "NumLasers", string.Empty, sorceressHealingCrossBeam.m_numLasers);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SorceressHealingCrossBeam sorceressHealingCrossBeam = GetTargetAbilityOnAbilityData(abilityData) as SorceressHealingCrossBeam;
		bool flag = sorceressHealingCrossBeam != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt laserNumberMod = m_laserNumberMod;
		int baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = sorceressHealingCrossBeam.m_numLasers;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(laserNumberMod, "[Number of Lasers]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat laserWidthMod = m_laserWidthMod;
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
			baseVal2 = sorceressHealingCrossBeam.m_width;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(laserWidthMod, "[Laser Width]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat laserRangeMod = m_laserRangeMod;
		float baseVal3;
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
			baseVal3 = sorceressHealingCrossBeam.m_distance;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(laserRangeMod, "[Laser Range]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyInt normalDamageMod = m_normalDamageMod;
		int baseVal4;
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
			baseVal4 = sorceressHealingCrossBeam.m_damageAmount;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(normalDamageMod, "[Damage]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyInt normalHealingMod = m_normalHealingMod;
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
			baseVal5 = sorceressHealingCrossBeam.m_healAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + AbilityModHelper.GetModPropertyDesc(normalHealingMod, "[Healing]", flag, baseVal5);
		if (m_useSingleTargetHitMods)
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
			string str6 = empty;
			AbilityModPropertyInt singleTargetDamageMod = m_singleTargetDamageMod;
			int baseVal6;
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
				baseVal6 = sorceressHealingCrossBeam.m_damageAmount;
			}
			else
			{
				baseVal6 = 0;
			}
			empty = str6 + AbilityModHelper.GetModPropertyDesc(singleTargetDamageMod, "[Damage if Target is only target in laser]", flag, baseVal6);
			string str7 = empty;
			AbilityModPropertyInt singleTargetHealingMod = m_singleTargetHealingMod;
			int baseVal7;
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
				baseVal7 = sorceressHealingCrossBeam.m_healAmount;
			}
			else
			{
				baseVal7 = 0;
			}
			empty = str7 + AbilityModHelper.GetModPropertyDesc(singleTargetHealingMod, "[Healing if Target is the only target in laser]", flag, baseVal7);
		}
		string str8 = empty;
		AbilityModPropertyEffectInfo enemyEffectOverride = m_enemyEffectOverride;
		object baseVal8;
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
			baseVal8 = sorceressHealingCrossBeam.m_enemyHitEffect;
		}
		else
		{
			baseVal8 = null;
		}
		empty = str8 + AbilityModHelper.GetModPropertyDesc(enemyEffectOverride, "{ Enemy Hit Effect Override }", flag, (StandardEffectInfo)baseVal8);
		string str9 = empty;
		AbilityModPropertyEffectInfo allyEffectOverride = m_allyEffectOverride;
		object baseVal9;
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
			baseVal9 = sorceressHealingCrossBeam.m_allyHitEffect;
		}
		else
		{
			baseVal9 = null;
		}
		empty = str9 + AbilityModHelper.GetModPropertyDesc(allyEffectOverride, "{ Ally Hit Effect Override }", flag, (StandardEffectInfo)baseVal9);
		if (m_knockbackDistance > 0f)
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
			string text;
			if (m_knockbackThresholdDistance <= 0f)
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
				text = string.Empty;
			}
			else
			{
				text = "to Targets within " + m_knockbackThresholdDistance + " squares, ";
			}
			string text2 = text;
			string text3 = empty;
			empty = string.Concat(text3, "\nKnockback ", m_knockbackDistance, " squares, ", text2, "with type ", m_knockbackType, "\n");
		}
		return empty + AbilityModHelper.GetModGroundEffectInfoDesc(m_groundEffectOnEnemyHit, "{ Ground Effect on Enemy Hit }", flag);
	}
}
