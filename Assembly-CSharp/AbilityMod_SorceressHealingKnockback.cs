using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SorceressHealingKnockback : AbilityMod
{
	[Header("-- Healing")]
	public float m_lowHealthThreshold;

	public AbilityModPropertyInt m_normalHealingMod;

	public AbilityModPropertyInt m_lowHealthHealingMod;

	public AbilityModPropertyInt m_onCastAllyEnergyGainMod;

	[Header("-- Damage and Effect Mod")]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyEffectInfo m_enemyHitEffectOverride;

	[Header("-- Knockback")]
	public AbilityModPropertyFloat m_knockbackDistanceMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SorceressHealingKnockback);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SorceressHealingKnockback sorceressHealingKnockback = targetAbility as SorceressHealingKnockback;
		if (!(sorceressHealingKnockback != null))
		{
			return;
		}
		while (true)
		{
			switch (3)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_normalHealingMod, "OnCastHealAmount_Normal", string.Empty, sorceressHealingKnockback.m_onCastHealAmount);
			AbilityMod.AddToken(tokens, m_lowHealthHealingMod, "OnCastHealAmount_LowHealth", string.Empty, sorceressHealingKnockback.m_onCastHealAmount);
			AbilityMod.AddToken(tokens, m_onCastAllyEnergyGainMod, "OnCastAllyEnergyGain", string.Empty, sorceressHealingKnockback.m_onCastAllyEnergyGain);
			AbilityMod.AddToken(tokens, m_damageMod, "OnDetonateDamageAmount", string.Empty, sorceressHealingKnockback.m_onDetonateDamageAmount);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectOverride, "OnDetonateEnemyEffect", sorceressHealingKnockback.m_onDetonateEnemyEffect);
			AbilityMod.AddToken(tokens, m_knockbackDistanceMod, "KnockbackDistance", string.Empty, sorceressHealingKnockback.m_knockbackDistance);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SorceressHealingKnockback sorceressHealingKnockback = GetTargetAbilityOnAbilityData(abilityData) as SorceressHealingKnockback;
		bool flag = sorceressHealingKnockback != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt normalHealingMod = m_normalHealingMod;
		int baseVal;
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
			baseVal = sorceressHealingKnockback.m_onCastHealAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(normalHealingMod, "[Normal Healing]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt lowHealthHealingMod = m_lowHealthHealingMod;
		int baseVal2;
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
			baseVal2 = sorceressHealingKnockback.m_onCastHealAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(lowHealthHealingMod, "[Healing when Low Health]", flag, baseVal2);
		if (m_lowHealthThreshold > 0f)
		{
			string text = empty;
			empty = text + "Health considered Low if portion is below " + m_lowHealthThreshold + "\n";
		}
		else if (m_lowHealthHealingMod != null)
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
			if (m_lowHealthHealingMod.operation != 0)
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
				empty += "Low Health Threshold not used, ignore [Healing when Low Health]\n";
			}
		}
		string str3 = empty;
		AbilityModPropertyInt onCastAllyEnergyGainMod = m_onCastAllyEnergyGainMod;
		int baseVal3;
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
			baseVal3 = sorceressHealingKnockback.m_onCastAllyEnergyGain;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(onCastAllyEnergyGainMod, "[OnCastAllyEnergyGain]", flag, baseVal3);
		empty += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Damage]", flag, flag ? sorceressHealingKnockback.m_onDetonateDamageAmount : 0);
		string str4 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectOverride = m_enemyHitEffectOverride;
		object baseVal4;
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
			baseVal4 = sorceressHealingKnockback.m_onDetonateEnemyEffect;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(enemyHitEffectOverride, "{ Enemy Hit Effect Override }", flag, (StandardEffectInfo)baseVal4);
		string str5 = empty;
		AbilityModPropertyFloat knockbackDistanceMod = m_knockbackDistanceMod;
		float baseVal5;
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
			baseVal5 = sorceressHealingKnockback.m_knockbackDistance;
		}
		else
		{
			baseVal5 = 0f;
		}
		return str5 + AbilityModHelper.GetModPropertyDesc(knockbackDistanceMod, "[Knockback Distance]", flag, baseVal5);
	}
}
