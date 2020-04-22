using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_RageBeastCharge : AbilityMod
{
	[Header("-- Damage Mod")]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyInt m_damageNearChargeEndMod;

	[Header("-- On Hit Effect")]
	public StandardEffectInfo m_effectOnTargetsHit;

	public AbilityModPropertyEffectInfo m_enemyHitEffectNearChargeEndMod;

	[Header("-- Targeting Mod")]
	public AbilityModPropertyFloat m_chargeLineRadiusMod;

	public AbilityModPropertyFloat m_chargeEndRadius;

	public override Type GetTargetAbilityType()
	{
		return typeof(RageBeastCharge);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		RageBeastCharge rageBeastCharge = targetAbility as RageBeastCharge;
		if (!(rageBeastCharge != null))
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
			AbilityMod.AddToken(tokens, m_damageMod, "DamageAmount", string.Empty, rageBeastCharge.m_damageAmount);
			AbilityMod.AddToken(tokens, m_damageNearChargeEndMod, "DamageNearChargeEnd", string.Empty, rageBeastCharge.m_damageNearChargeEnd);
			AbilityMod.AddToken(tokens, m_chargeLineRadiusMod, "DamageRadius", string.Empty, rageBeastCharge.m_damageRadius);
			AbilityMod.AddToken(tokens, m_chargeEndRadius, "RadiusBeyondEnd", string.Empty, rageBeastCharge.m_radiusBeyondEnd);
			AbilityMod.AddToken_EffectMod(tokens, m_enemyHitEffectNearChargeEndMod, "EnemyHitEffectNearChargeEnd", rageBeastCharge.m_enemyHitEffectNearChargeEnd);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		RageBeastCharge rageBeastCharge = GetTargetAbilityOnAbilityData(abilityData) as RageBeastCharge;
		bool flag = rageBeastCharge != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyInt damageMod = m_damageMod;
		int baseVal;
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			baseVal = rageBeastCharge.m_damageAmount;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(damageMod, "[Damage Mod]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyInt damageNearChargeEndMod = m_damageNearChargeEndMod;
		int baseVal2;
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
			baseVal2 = rageBeastCharge.m_damageNearChargeEnd;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(damageNearChargeEndMod, "[Damage Near Charge End Mod]", flag, baseVal2);
		empty += AbilityModHelper.GetModPropertyDesc(m_chargeLineRadiusMod, "[Charge Line Radius/Half-Width Mod]", flag, (!flag) ? 0f : rageBeastCharge.m_damageRadius);
		empty += AbilityModHelper.GetModPropertyDesc(m_chargeEndRadius, "[Charge End Radius Mod]", flag, (!flag) ? 0f : rageBeastCharge.m_radiusBeyondEnd);
		empty += AbilityModHelper.GetModEffectInfoDesc(m_effectOnTargetsHit, "{ Effect on Target Hit }", string.Empty, flag);
		string str3 = empty;
		AbilityModPropertyEffectInfo enemyHitEffectNearChargeEndMod = m_enemyHitEffectNearChargeEndMod;
		object baseVal3;
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
			baseVal3 = rageBeastCharge.m_enemyHitEffectNearChargeEnd;
		}
		else
		{
			baseVal3 = null;
		}
		return str3 + PropDesc(enemyHitEffectNearChargeEndMod, "[EnemyHitEffectNearChargeEnd]", flag, (StandardEffectInfo)baseVal3);
	}
}
