using System;
using System.Collections.Generic;

public class AbilityMod_SamuraiDashAndAimedSlash : AbilityMod
{
	[Separator("Targeting", true)]
	public AbilityModPropertyFloat m_maxAngleForLaserMod;

	public AbilityModPropertyFloat m_laserWidthMod;

	public AbilityModPropertyFloat m_laserRangeMod;

	public AbilityModPropertyInt m_maxTargetsMod;

	[Separator("Enemy hits", true)]
	public AbilityModPropertyInt m_damageAmountMod;

	public AbilityModPropertyInt m_extraDamageIfSingleTargetMod;

	public AbilityModPropertyEffectInfo m_targetEffectMod;

	[Separator("Self Hit", true)]
	public AbilityModPropertyEffectInfo m_effectOnSelfMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(SamuraiDashAndAimedSlash);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		SamuraiDashAndAimedSlash samuraiDashAndAimedSlash = targetAbility as SamuraiDashAndAimedSlash;
		if (!(samuraiDashAndAimedSlash != null))
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
			AbilityMod.AddToken(tokens, m_maxAngleForLaserMod, "MaxAngleForLaser", string.Empty, samuraiDashAndAimedSlash.m_maxAngleForLaser);
			AbilityMod.AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, samuraiDashAndAimedSlash.m_laserWidth);
			AbilityMod.AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, samuraiDashAndAimedSlash.m_laserRange);
			AbilityMod.AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, samuraiDashAndAimedSlash.m_maxTargets);
			AbilityMod.AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, samuraiDashAndAimedSlash.m_damageAmount);
			AbilityMod.AddToken(tokens, m_extraDamageIfSingleTargetMod, "ExtraDamageIfSingleTarget", string.Empty, samuraiDashAndAimedSlash.m_extraDamageIfSingleTarget);
			AbilityMod.AddToken_EffectMod(tokens, m_targetEffectMod, "TargetEffect", samuraiDashAndAimedSlash.m_targetEffect);
			AbilityMod.AddToken_EffectMod(tokens, m_effectOnSelfMod, "EffectOnSelf", samuraiDashAndAimedSlash.m_effectOnSelf);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SamuraiDashAndAimedSlash samuraiDashAndAimedSlash = GetTargetAbilityOnAbilityData(abilityData) as SamuraiDashAndAimedSlash;
		bool flag = samuraiDashAndAimedSlash != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyFloat maxAngleForLaserMod = m_maxAngleForLaserMod;
		float baseVal;
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
			baseVal = samuraiDashAndAimedSlash.m_maxAngleForLaser;
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
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = samuraiDashAndAimedSlash.m_laserWidth;
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
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = samuraiDashAndAimedSlash.m_laserRange;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(laserRangeMod, "[LaserRange]", flag, baseVal3);
		empty += PropDesc(m_maxTargetsMod, "[MaxTargets]", flag, flag ? samuraiDashAndAimedSlash.m_maxTargets : 0);
		empty += PropDesc(m_damageAmountMod, "[DamageAmount]", flag, flag ? samuraiDashAndAimedSlash.m_damageAmount : 0);
		string str4 = empty;
		AbilityModPropertyInt extraDamageIfSingleTargetMod = m_extraDamageIfSingleTargetMod;
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
			baseVal4 = samuraiDashAndAimedSlash.m_extraDamageIfSingleTarget;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(extraDamageIfSingleTargetMod, "[ExtraDamageIfSingleTarget]", flag, baseVal4);
		string str5 = empty;
		AbilityModPropertyEffectInfo targetEffectMod = m_targetEffectMod;
		object baseVal5;
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
			baseVal5 = samuraiDashAndAimedSlash.m_targetEffect;
		}
		else
		{
			baseVal5 = null;
		}
		empty = str5 + PropDesc(targetEffectMod, "[TargetEffect]", flag, (StandardEffectInfo)baseVal5);
		string str6 = empty;
		AbilityModPropertyEffectInfo effectOnSelfMod = m_effectOnSelfMod;
		object baseVal6;
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
			baseVal6 = samuraiDashAndAimedSlash.m_effectOnSelf;
		}
		else
		{
			baseVal6 = null;
		}
		return str6 + PropDesc(effectOnSelfMod, "[EffectOnSelf]", flag, (StandardEffectInfo)baseVal6);
	}
}
