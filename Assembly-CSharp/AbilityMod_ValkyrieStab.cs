using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_ValkyrieStab : AbilityMod
{
	[Header("-- Targeting")]
	public AbilityModPropertyFloat m_coneWidthMinAngleMod;

	public AbilityModPropertyFloat m_coneWidthMaxAngleMod;

	public AbilityModPropertyFloat m_coneBackwardOffsetMod;

	public AbilityModPropertyFloat m_coneMinLengthMod;

	public AbilityModPropertyFloat m_coneMaxLengthMod;

	public AbilityModPropertyBool m_penetrateLineOfSightMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	[Header("-- On Hit Damage/Effect")]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyInt m_lessDamagePerTargetMod;

	public AbilityModPropertyInt m_extraDamageOnSpearTip;

	public AbilityModPropertyInt m_extraDamageFirstTarget;

	public AbilityModPropertyEffectInfo m_targetHitEffectMod;

	[Header("-- Misc ability interactions --")]
	public AbilityModPropertyInt m_perHitExtraAbsorbNextShieldBlock;

	public AbilityModPropertyInt m_maxExtraAbsorbNextShieldBlock;

	public override Type GetTargetAbilityType()
	{
		return typeof(ValkyrieStab);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		ValkyrieStab valkyrieStab = targetAbility as ValkyrieStab;
		if (!(valkyrieStab != null))
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
			AbilityMod.AddToken(tokens, m_coneWidthMinAngleMod, "ConeWidthMinAngle", string.Empty, valkyrieStab.m_coneWidthMinAngle);
			AbilityMod.AddToken(tokens, m_coneWidthMaxAngleMod, "ConeWidthMaxAngle", string.Empty, valkyrieStab.m_coneWidthMaxAngle);
			AbilityMod.AddToken(tokens, m_coneBackwardOffsetMod, "ConeBackwardOffset", string.Empty, valkyrieStab.m_coneBackwardOffset);
			AbilityMod.AddToken(tokens, m_coneMinLengthMod, "ConeMinLength", string.Empty, valkyrieStab.m_coneMinLength);
			AbilityMod.AddToken(tokens, m_coneMaxLengthMod, "ConeMaxLength", string.Empty, valkyrieStab.m_coneMaxLength);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, valkyrieStab.m_maxTargets);
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, valkyrieStab.m_damageAmount);
			AbilityMod.AddToken(tokens, m_lessDamagePerTargetMod, "LessDamagePerTarget", string.Empty, valkyrieStab.m_lessDamagePerTarget);
			AbilityMod.AddToken(tokens, m_extraDamageOnSpearTip, "ExtraDamageOnSpearTip", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_extraDamageFirstTarget, "ExtraDamageFirstTarget", string.Empty, 0);
			AbilityMod.AddToken_EffectMod(tokens, m_targetHitEffectMod, "TargetHitEffect", valkyrieStab.m_targetHitEffect);
			AbilityMod.AddToken(tokens, m_perHitExtraAbsorbNextShieldBlock, "PerHitExtraAbsorbNextShieldBlock", string.Empty, 0);
			AbilityMod.AddToken(tokens, m_maxExtraAbsorbNextShieldBlock, "MaxExtraAbsorbNextShieldBlock", string.Empty, 0);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		ValkyrieStab valkyrieStab = GetTargetAbilityOnAbilityData(abilityData) as ValkyrieStab;
		bool flag = valkyrieStab != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat coneWidthMinAngleMod = m_coneWidthMinAngleMod;
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
			baseVal = valkyrieStab.m_coneWidthMinAngle;
		}
		else
		{
			baseVal = 0f;
		}
		empty = str + PropDesc(coneWidthMinAngleMod, "[ConeWidthMinAngle]", flag, baseVal);
		string str2 = empty;
		AbilityModPropertyFloat coneWidthMaxAngleMod = m_coneWidthMaxAngleMod;
		float baseVal2;
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
			baseVal2 = valkyrieStab.m_coneWidthMaxAngle;
		}
		else
		{
			baseVal2 = 0f;
		}
		empty = str2 + PropDesc(coneWidthMaxAngleMod, "[ConeWidthMaxAngle]", flag, baseVal2);
		empty += PropDesc(m_coneBackwardOffsetMod, "[ConeBackwardOffset]", flag, (!flag) ? 0f : valkyrieStab.m_coneBackwardOffset);
		string str3 = empty;
		AbilityModPropertyFloat coneMinLengthMod = m_coneMinLengthMod;
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
			baseVal3 = valkyrieStab.m_coneMinLength;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(coneMinLengthMod, "[ConeMinLength]", flag, baseVal3);
		string str4 = empty;
		AbilityModPropertyFloat coneMaxLengthMod = m_coneMaxLengthMod;
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
			baseVal4 = valkyrieStab.m_coneMaxLength;
		}
		else
		{
			baseVal4 = 0f;
		}
		empty = str4 + PropDesc(coneMaxLengthMod, "[ConeMaxLength]", flag, baseVal4);
		empty += PropDesc(m_penetrateLineOfSightMod, "[PenetrateLineOfSight]", flag, flag && valkyrieStab.m_penetrateLineOfSight);
		string str5 = empty;
		AbilityModPropertyInt maxTargetsMod = m_maxTargetsMod;
		int baseVal5;
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
			baseVal5 = valkyrieStab.m_maxTargets;
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(maxTargetsMod, "[MaxTargets]", flag, baseVal5);
		string str6 = empty;
		AbilityModPropertyInt damageAmountMod = m_damageAmountMod;
		int baseVal6;
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
			baseVal6 = valkyrieStab.m_damageAmount;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(damageAmountMod, "[DamageAmount]", flag, baseVal6);
		empty += PropDesc(m_lessDamagePerTargetMod, "[LessDamagePerTarget]", flag, flag ? valkyrieStab.m_lessDamagePerTarget : 0);
		empty += PropDesc(m_extraDamageOnSpearTip, "[ExtraDamageOnSpearTip]", flag);
		empty += PropDesc(m_extraDamageFirstTarget, "[ExtraDamageFirstTarget]", flag);
		string str7 = empty;
		AbilityModPropertyEffectInfo targetHitEffectMod = m_targetHitEffectMod;
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
			baseVal7 = valkyrieStab.m_targetHitEffect;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + PropDesc(targetHitEffectMod, "[TargetHitEffect]", flag, (StandardEffectInfo)baseVal7);
		empty += PropDesc(m_perHitExtraAbsorbNextShieldBlock, "[PerHitExtraAbsorbNextShieldBlock]", flag);
		return empty + PropDesc(m_maxExtraAbsorbNextShieldBlock, "[MaxExtraAbsorbNextShieldBlock]", flag);
	}
}
