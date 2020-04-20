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
		if (samuraiDashAndAimedSlash != null)
		{
			AbilityMod.AddToken(tokens, this.m_maxAngleForLaserMod, "MaxAngleForLaser", string.Empty, samuraiDashAndAimedSlash.m_maxAngleForLaser, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserWidthMod, "LaserWidth", string.Empty, samuraiDashAndAimedSlash.m_laserWidth, true, false, false);
			AbilityMod.AddToken(tokens, this.m_laserRangeMod, "LaserRange", string.Empty, samuraiDashAndAimedSlash.m_laserRange, true, false, false);
			AbilityMod.AddToken(tokens, this.m_maxTargetsMod, "MaxTargets", string.Empty, samuraiDashAndAimedSlash.m_maxTargets, true, false);
			AbilityMod.AddToken(tokens, this.m_damageAmountMod, "DamageAmount", string.Empty, samuraiDashAndAimedSlash.m_damageAmount, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageIfSingleTargetMod, "ExtraDamageIfSingleTarget", string.Empty, samuraiDashAndAimedSlash.m_extraDamageIfSingleTarget, true, false);
			AbilityMod.AddToken_EffectMod(tokens, this.m_targetEffectMod, "TargetEffect", samuraiDashAndAimedSlash.m_targetEffect, true);
			AbilityMod.AddToken_EffectMod(tokens, this.m_effectOnSelfMod, "EffectOnSelf", samuraiDashAndAimedSlash.m_effectOnSelf, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SamuraiDashAndAimedSlash samuraiDashAndAimedSlash = base.GetTargetAbilityOnAbilityData(abilityData) as SamuraiDashAndAimedSlash;
		bool flag = samuraiDashAndAimedSlash != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyFloat maxAngleForLaserMod = this.m_maxAngleForLaserMod;
		string prefix = "[MaxAngleForLaser]";
		bool showBaseVal = flag;
		float baseVal;
		if (flag)
		{
			baseVal = samuraiDashAndAimedSlash.m_maxAngleForLaser;
		}
		else
		{
			baseVal = 0f;
		}
		text = str + base.PropDesc(maxAngleForLaserMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyFloat laserWidthMod = this.m_laserWidthMod;
		string prefix2 = "[LaserWidth]";
		bool showBaseVal2 = flag;
		float baseVal2;
		if (flag)
		{
			baseVal2 = samuraiDashAndAimedSlash.m_laserWidth;
		}
		else
		{
			baseVal2 = 0f;
		}
		text = str2 + base.PropDesc(laserWidthMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat laserRangeMod = this.m_laserRangeMod;
		string prefix3 = "[LaserRange]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			baseVal3 = samuraiDashAndAimedSlash.m_laserRange;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(laserRangeMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDesc(this.m_maxTargetsMod, "[MaxTargets]", flag, (!flag) ? 0 : samuraiDashAndAimedSlash.m_maxTargets);
		text += base.PropDesc(this.m_damageAmountMod, "[DamageAmount]", flag, (!flag) ? 0 : samuraiDashAndAimedSlash.m_damageAmount);
		string str4 = text;
		AbilityModPropertyInt extraDamageIfSingleTargetMod = this.m_extraDamageIfSingleTargetMod;
		string prefix4 = "[ExtraDamageIfSingleTarget]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = samuraiDashAndAimedSlash.m_extraDamageIfSingleTarget;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(extraDamageIfSingleTargetMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyEffectInfo targetEffectMod = this.m_targetEffectMod;
		string prefix5 = "[TargetEffect]";
		bool showBaseVal5 = flag;
		StandardEffectInfo baseVal5;
		if (flag)
		{
			baseVal5 = samuraiDashAndAimedSlash.m_targetEffect;
		}
		else
		{
			baseVal5 = null;
		}
		text = str5 + base.PropDesc(targetEffectMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyEffectInfo effectOnSelfMod = this.m_effectOnSelfMod;
		string prefix6 = "[EffectOnSelf]";
		bool showBaseVal6 = flag;
		StandardEffectInfo baseVal6;
		if (flag)
		{
			baseVal6 = samuraiDashAndAimedSlash.m_effectOnSelf;
		}
		else
		{
			baseVal6 = null;
		}
		return str6 + base.PropDesc(effectOnSelfMod, prefix6, showBaseVal6, baseVal6);
	}
}
