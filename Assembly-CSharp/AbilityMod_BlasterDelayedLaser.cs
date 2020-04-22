using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BlasterDelayedLaser : AbilityMod
{
	[Header("-- Laser Data")]
	public AbilityModPropertyBool m_penetrateLineOfSightMod;

	public AbilityModPropertyFloat m_lengthMod;

	public AbilityModPropertyFloat m_widthMod;

	[Space(10f)]
	public AbilityModPropertyBool m_triggerAimAtBlasterMod;

	[Header("-- On Hit")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyEffectInfo m_effectOnHitMod;

	public AbilityModPropertyInt m_extraDamageToNearEnemyMod;

	public AbilityModPropertyFloat m_nearDistanceMod;

	[Header("-- On Cast Hit Effect")]
	public AbilityModPropertyEffectInfo m_onCastEnemyHitEffectMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(BlasterDelayedLaser);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BlasterDelayedLaser blasterDelayedLaser = targetAbility as BlasterDelayedLaser;
		if (!(blasterDelayedLaser != null))
		{
			return;
		}
		AbilityMod.AddToken(tokens, m_lengthMod, "Length", string.Empty, blasterDelayedLaser.m_length);
		AbilityMod.AddToken(tokens, m_widthMod, "Width", string.Empty, blasterDelayedLaser.m_width);
		AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, blasterDelayedLaser.m_damageAmount);
		AbilityMod.AddToken_EffectMod(tokens, m_effectOnHitMod, "EffectOnHit", blasterDelayedLaser.m_effectOnHit);
		AbilityMod.AddToken(tokens, m_extraDamageToNearEnemyMod, "ExtraDamageToNearEnemy", string.Empty, blasterDelayedLaser.m_extraDamageToNearEnemy);
		AbilityMod.AddToken(tokens, m_nearDistanceMod, "NearDistance", string.Empty, blasterDelayedLaser.m_nearDistance);
		if (m_nearDistanceMod != null)
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
			if (1 == 0)
			{
				/*OpCode not supported: LdMemberToken*/;
			}
			AbilityMod.AddToken_IntDiff(tokens, "NearDist_MinusOne", string.Empty, Mathf.RoundToInt(m_nearDistanceMod.GetModifiedValue(blasterDelayedLaser.m_nearDistance)) - 1, false, 0);
		}
		AbilityMod.AddToken_EffectMod(tokens, m_onCastEnemyHitEffectMod, "OnCastEnemyHitEffect", blasterDelayedLaser.m_onCastEnemyHitEffect);
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		BlasterDelayedLaser blasterDelayedLaser = GetTargetAbilityOnAbilityData(abilityData) as BlasterDelayedLaser;
		bool flag = blasterDelayedLaser != null;
		string empty = string.Empty;
		empty += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, flag && blasterDelayedLaser.m_penetrateLineOfSight);
		empty += PropDesc(m_lengthMod, "[Length]", flag, (!flag) ? 0f : blasterDelayedLaser.m_length);
		string str = empty;
		AbilityModPropertyFloat widthMod = m_widthMod;
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
			baseVal = blasterDelayedLaser.m_width;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(widthMod, "[Width]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyBool triggerAimAtBlasterMod = m_triggerAimAtBlasterMod;
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
			baseVal2 = (blasterDelayedLaser.m_triggerAimAtBlaster ? 1 : 0);
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(triggerAimAtBlasterMod, "[TriggerAimAtBlaster]", flag, (byte)baseVal2 != 0);
		string str3 = empty;
		AbilityModPropertyInt damageAmountMod = m_damageAmountMod;
		int baseVal3;
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
			baseVal3 = blasterDelayedLaser.m_damageAmount;
		}
		else
		{
			baseVal3 = 0;
		}
		empty = str3 + PropDesc(damageAmountMod, "[DamageAmount]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyEffectInfo effectOnHitMod = m_effectOnHitMod;
		object baseVal4;
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
			baseVal4 = blasterDelayedLaser.m_effectOnHit;
		}
		else
		{
			baseVal4 = null;
		}
		empty = str4 + PropDesc(effectOnHitMod, "[EffectOnHit]", flag, (StandardEffectInfo)baseVal4);
		string str5 = empty;
		AbilityModPropertyInt extraDamageToNearEnemyMod = m_extraDamageToNearEnemyMod;
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
			baseVal5 = blasterDelayedLaser.m_extraDamageToNearEnemy;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(extraDamageToNearEnemyMod, "[ExtraDamageToNearEnemy]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyFloat nearDistanceMod = m_nearDistanceMod;
		float baseVal6;
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
			baseVal6 = blasterDelayedLaser.m_nearDistance;
		}
		else
		{
			baseVal6 = 0f;
		}
		empty = str6 + PropDesc(nearDistanceMod, "[NearDistance]", flag, baseVal6);
		string str7 = empty;
		AbilityModPropertyEffectInfo onCastEnemyHitEffectMod = m_onCastEnemyHitEffectMod;
		object baseVal7;
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
			baseVal7 = blasterDelayedLaser.m_onCastEnemyHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		return str7 + PropDesc(onCastEnemyHitEffectMod, "[OnCastEnemyHitEffect]", flag, (StandardEffectInfo)baseVal7);
	}
}
