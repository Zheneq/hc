using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_SniperBasicAttack : AbilityMod
{
	[Header("-- Laser Property Mod")]
	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_laserRangeMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	[Header("-- Damage Mod")]
	public AbilityModPropertyInt m_damageMod;

	public AbilityModPropertyInt m_minDamageMod;

	public AbilityModPropertyInt m_damageChangePerHitMod;

	[Header("-- Distance Based Damage Mod")]
	public float m_farDistanceThreshold;

	public AbilityModPropertyInt m_farEnemyDamageMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SniperBasicAttack);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SniperBasicAttack sniperBasicAttack = targetAbility as SniperBasicAttack;
		if (!(sniperBasicAttack != null))
		{
			return;
		}
		while (true)
		{
			switch (4)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken(tokens, m_damageMod, "LaserDamageAmount", string.Empty, sniperBasicAttack.m_laserDamageAmount);
			AbilityMod.AddToken(tokens, m_minDamageMod, "MinDamageAmount", string.Empty, sniperBasicAttack.m_minDamageAmount);
			AbilityMod.AddToken(tokens, m_damageChangePerHitMod, "DamageChangePerHit", string.Empty, sniperBasicAttack.m_damageChangePerHit);
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, sniperBasicAttack.m_laserInfo.width);
			AbilityMod.AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, sniperBasicAttack.m_laserInfo.range);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "LaserMaxTargets", string.Empty, sniperBasicAttack.m_laserInfo.maxTargets);
			if (m_farDistanceThreshold > 0f)
			{
				AbilityMod.AddToken_IntDiff(tokens, "FarDistThreshold", string.Empty, Mathf.RoundToInt(m_farDistanceThreshold), false, 0);
				AbilityMod.AddToken(tokens, m_farEnemyDamageMod, "Damage_Far", string.Empty, sniperBasicAttack.m_laserDamageAmount);
			}
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SniperBasicAttack sniperBasicAttack = GetTargetAbilityOnAbilityData(abilityData) as SniperBasicAttack;
		bool flag = sniperBasicAttack != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat laserWidthMod = m_laserWidthMod;
		float baseVal;
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
			baseVal = sniperBasicAttack.m_laserInfo.width;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(laserWidthMod, "[Laser Width]", flag, baseVal);
		empty += AbilityModHelper.GetModPropertyDesc(m_laserRangeMod, "[Laser Range]", flag, (!flag) ? 0f : sniperBasicAttack.m_laserInfo.range);
		empty += AbilityModHelper.GetModPropertyDesc(m_maxTargetsMod, "[Max Targets]", flag, flag ? sniperBasicAttack.m_laserInfo.maxTargets : 0);
		empty += AbilityModHelper.GetModPropertyDesc(m_damageMod, "[Base Damage]", flag, flag ? sniperBasicAttack.m_laserDamageAmount : 0);
		string str2 = empty;
		AbilityModPropertyInt minDamageMod = m_minDamageMod;
		int baseVal2;
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
			baseVal2 = sniperBasicAttack.m_minDamageAmount;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(minDamageMod, "[Min Damage]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyInt damageChangePerHitMod = m_damageChangePerHitMod;
		int baseVal3;
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
			baseVal3 = sniperBasicAttack.m_damageChangePerHit;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(damageChangePerHitMod, "[Damage Change Per Hit]", flag, baseVal3);
		if (m_farDistanceThreshold > 0f)
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
			string text = empty;
			empty = text + "[Far Distance Threshold] = " + m_farDistanceThreshold + "\n";
			empty += AbilityModHelper.GetModPropertyDesc(m_farEnemyDamageMod, "[Far Enemy Damage]", flag, flag ? sniperBasicAttack.m_laserDamageAmount : 0);
		}
		return empty;
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		SniperBasicAttack sniperBasicAttack = abilityAsBase as SniperBasicAttack;
		if (!(sniperBasicAttack != null))
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
			int modifiedValue = m_damageMod.GetModifiedValue(sniperBasicAttack.m_laserDamageAmount);
			int modifiedValue2 = m_damageChangePerHitMod.GetModifiedValue(sniperBasicAttack.m_damageChangePerHit);
			if (modifiedValue2 != 0)
			{
				while (true)
				{
					switch (5)
					{
					case 0:
						continue;
					}
					numbers.Add(modifiedValue + modifiedValue2);
					return;
				}
			}
			return;
		}
	}
}
