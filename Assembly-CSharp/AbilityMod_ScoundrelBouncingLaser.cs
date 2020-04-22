using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ScoundrelBouncingLaser : AbilityMod
{
	[Header("-- Laser Properties")]
	public AbilityModPropertyInt m_maxTargetsMod;

	public AbilityModPropertyInt m_maxBounceMod;

	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_distancePerBounceMod;

	public AbilityModPropertyFloat m_maxTotalDistanceMod;

	[Header("-- Damage")]
	public AbilityModPropertyInt m_baseDamageMod;

	public AbilityModPropertyInt m_minDamageMod;

	public AbilityModPropertyInt m_damageChangePerHitMod;

	public AbilityModPropertyInt m_bonusDamagePerBounceMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(ScoundrelBouncingLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ScoundrelBouncingLaser scoundrelBouncingLaser = targetAbility as ScoundrelBouncingLaser;
		if (!(scoundrelBouncingLaser != null))
		{
			return;
		}
		AbilityMod.AddToken(tokens, m_baseDamageMod, "DamageAmount", string.Empty, scoundrelBouncingLaser.m_damageAmount);
		AbilityMod.AddToken(tokens, m_minDamageMod, "MinDamageAmount", string.Empty, scoundrelBouncingLaser.m_minDamageAmount);
		AbilityMod.AddToken(tokens, m_damageChangePerHitMod, "DamageChangePerHit", string.Empty, scoundrelBouncingLaser.m_damageChangePerHit);
		AbilityMod.AddToken(tokens, m_bonusDamagePerBounceMod, "BonusDamagePerBounce", string.Empty, scoundrelBouncingLaser.m_bonusDamagePerBounce);
		AbilityMod.AddToken(tokens, m_laserWidthMod, "Width", string.Empty, scoundrelBouncingLaser.m_width);
		AbilityMod.AddToken(tokens, m_distancePerBounceMod, "MaxDistancePerBounce", string.Empty, scoundrelBouncingLaser.m_maxDistancePerBounce);
		AbilityMod.AddToken(tokens, m_maxTotalDistanceMod, "MaxTotalDistance", string.Empty, scoundrelBouncingLaser.m_maxTotalDistance);
		AbilityMod.AddToken(tokens, m_maxBounceMod, "MaxBounces", string.Empty, scoundrelBouncingLaser.m_maxBounces);
		AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargetsHit", string.Empty, scoundrelBouncingLaser.m_maxTargetsHit);
		if (m_baseDamageMod == null)
		{
			return;
		}
		while (true)
		{
			switch (1)
			{
			case 0:
				continue;
			}
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			if (m_damageChangePerHitMod == null)
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
				int modifiedValue = m_baseDamageMod.GetModifiedValue(scoundrelBouncingLaser.m_damageAmount);
				int modifiedValue2 = m_damageChangePerHitMod.GetModifiedValue(scoundrelBouncingLaser.m_damageChangePerHit);
				if (modifiedValue2 != 0)
				{
					while (true)
					{
						switch (3)
						{
						case 0:
							continue;
						}
						AbilityMod.AddToken_IntDiff(tokens, "FirstDamageAfterChange", string.Empty, modifiedValue + modifiedValue2, false, 0);
						return;
					}
				}
				return;
			}
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ScoundrelBouncingLaser scoundrelBouncingLaser = GetTargetAbilityOnAbilityData(abilityData) as ScoundrelBouncingLaser;
		bool flag = scoundrelBouncingLaser != null;
		string empty = string.Empty;
		empty += AbilityModHelper.GetModPropertyDesc(m_maxTargetsMod, "[Max Target Hits]", flag, flag ? scoundrelBouncingLaser.m_maxTargetsHit : 0);
		string str = empty;
		AbilityModPropertyInt maxBounceMod = m_maxBounceMod;
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
			baseVal = scoundrelBouncingLaser.m_maxBounces;
		}
		else
		{
			baseVal = 0;
		}
		empty = str + AbilityModHelper.GetModPropertyDesc(maxBounceMod, "[Max Bounces]", flag, baseVal);
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
			baseVal2 = scoundrelBouncingLaser.m_width;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + AbilityModHelper.GetModPropertyDesc(laserWidthMod, "[Laser Width]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat distancePerBounceMod = m_distancePerBounceMod;
		float baseVal3;
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
			baseVal3 = scoundrelBouncingLaser.m_maxDistancePerBounce;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + AbilityModHelper.GetModPropertyDesc(distancePerBounceMod, "[Distance Per Bounce]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat maxTotalDistanceMod = m_maxTotalDistanceMod;
		float baseVal4;
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
			baseVal4 = scoundrelBouncingLaser.m_maxTotalDistance;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + AbilityModHelper.GetModPropertyDesc(maxTotalDistanceMod, "[Max Total Distance]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyInt baseDamageMod = m_baseDamageMod;
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
			baseVal5 = scoundrelBouncingLaser.m_damageAmount;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + AbilityModHelper.GetModPropertyDesc(baseDamageMod, "[Base Damage]", flag, baseVal5);
		empty += AbilityModHelper.GetModPropertyDesc(m_minDamageMod, "[Min Base Damage Mod]", flag, flag ? scoundrelBouncingLaser.m_minDamageAmount : 0);
		string str6 = empty;
		AbilityModPropertyInt damageChangePerHitMod = m_damageChangePerHitMod;
		int baseVal6;
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
			baseVal6 = scoundrelBouncingLaser.m_damageChangePerHit;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + AbilityModHelper.GetModPropertyDesc(damageChangePerHitMod, "[Damage Change Per Hit]", flag, baseVal6);
		return empty + AbilityModHelper.GetModPropertyDesc(m_bonusDamagePerBounceMod, "[Bonus Damage Per Bounce]", flag, flag ? scoundrelBouncingLaser.m_bonusDamagePerBounce : 0);
	}

	protected override void AppendModSpecificTooltipCheckNumbers(Ability abilityAsBase, List<int> numbers)
	{
		ScoundrelBouncingLaser scoundrelBouncingLaser = abilityAsBase as ScoundrelBouncingLaser;
		if (scoundrelBouncingLaser != null)
		{
			int modifiedValue = m_baseDamageMod.GetModifiedValue(scoundrelBouncingLaser.m_damageAmount);
			int modifiedValue2 = m_damageChangePerHitMod.GetModifiedValue(scoundrelBouncingLaser.m_damageChangePerHit);
			numbers.Add(modifiedValue + modifiedValue2);
		}
	}
}
