using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SniperPenetratingRound : AbilityMod
{
	[Header("-- Laser Mods")]
	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_laserRangeMod;

	[Header("-- Enemy Hit Effect Override")]
	public bool m_useEnemyHitEffectOverride;

	public StandardEffectInfo m_enemyHitEffectOverride;

	[Header("-- Knockback")]
	public bool m_knockbackHitEnemy;

	public float m_knockbackThresholdDistance = -1f;

	public KnockbackType m_knockbackType = KnockbackType.AwayFromSource;

	public float m_knockbackDistance;

	[Header("-- Damage")]
	public AbilityModPropertyInt m_laserDamage;

	[Header("-- Bonus Damage from Target Health Threshold --")]
	public AbilityModPropertyInt m_additionalDamageOnLowHealthTargetMod;

	public AbilityModPropertyFloat m_lowHealthThresholdMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SniperPenetratingRound);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SniperPenetratingRound sniperPenetratingRound = targetAbility as SniperPenetratingRound;
		if (!(sniperPenetratingRound != null))
		{
			return;
		}
		AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, sniperPenetratingRound.m_laserInfo.width);
		AbilityMod.AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, sniperPenetratingRound.m_laserInfo.range);
		if (m_useEnemyHitEffectOverride)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken_EffectInfo(tokens, m_enemyHitEffectOverride, "EnemyHitEffect", sniperPenetratingRound.m_laserHitEffect);
		}
		AbilityMod.AddToken(tokens, m_laserDamage, "Damage", "base damage", sniperPenetratingRound.m_laserDamageAmount);
		AbilityMod.AddToken(tokens, m_additionalDamageOnLowHealthTargetMod, "AdditionalDamageOnLowHealthTarget", string.Empty, sniperPenetratingRound.m_additionalDamageOnLowHealthTarget);
		AbilityMod.AddToken(tokens, m_lowHealthThresholdMod, "LowHealthThreshold", string.Empty, sniperPenetratingRound.m_lowHealthThreshold);
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SniperPenetratingRound sniperPenetratingRound = GetTargetAbilityOnAbilityData(abilityData) as SniperPenetratingRound;
		bool flag = sniperPenetratingRound != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat laserWidthMod = m_laserWidthMod;
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
			baseVal = sniperPenetratingRound.m_laserInfo.width;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(laserWidthMod, "[Laser Width]", flag, baseVal);
		empty += AbilityModHelper.GetModPropertyDesc(m_laserRangeMod, "[Laser Range]", flag, (!flag) ? 0f : sniperPenetratingRound.m_laserInfo.range);
		if (m_useEnemyHitEffectOverride)
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
			string str2 = empty;
			StandardEffectInfo enemyHitEffectOverride = m_enemyHitEffectOverride;
			string empty2 = string.Empty;
			object baseVal2;
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
				baseVal2 = sniperPenetratingRound.m_laserHitEffect;
			}
			else
			{
				baseVal2 = null;
			}
			empty = str2 + AbilityModHelper.GetModEffectInfoDesc(enemyHitEffectOverride, "{ Enemy Hit Effect Override }", empty2, flag, (StandardEffectInfo)baseVal2);
		}
		if (m_knockbackHitEnemy)
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
			string text = empty;
			empty = text + "Can knock back hit enemy within " + m_knockbackThresholdDistance + " squares\n";
		}
		empty += AbilityModHelper.GetModPropertyDesc(m_laserDamage, "[Laser Damage]", flag, flag ? sniperPenetratingRound.m_laserDamageAmount : 0);
		empty += PropDesc(m_additionalDamageOnLowHealthTargetMod, "[AdditionalDamageOnLowHealthTarget]", flag, flag ? sniperPenetratingRound.m_additionalDamageOnLowHealthTarget : 0);
		string str3 = empty;
		AbilityModPropertyFloat lowHealthThresholdMod = m_lowHealthThresholdMod;
		float baseVal3;
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
			baseVal3 = sniperPenetratingRound.m_lowHealthThreshold;
		}
		else
		{
			baseVal3 = 0f;
		}
		return str3 + PropDesc(lowHealthThresholdMod, "[LowHealthThreshold]", flag, baseVal3);
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		if (!m_useEnemyHitEffectOverride || !m_enemyHitEffectOverride.m_applyEffect)
		{
			return;
		}
		while (true)
		{
			switch (7)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			numbers.Add(m_enemyHitEffectOverride.m_effectData.m_duration - 1);
			return;
		}
	}
}
