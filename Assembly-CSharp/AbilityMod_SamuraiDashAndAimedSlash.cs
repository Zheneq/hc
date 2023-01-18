using System;
using System.Collections.Generic;

public class AbilityMod_SamuraiDashAndAimedSlash : AbilityMod
{
	[Separator("Targeting")]
	public AbilityModPropertyFloat m_maxAngleForLaserMod;
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyFloat m_laserRangeMod;
	public AbilityModPropertyInt m_maxTargetsMod;
	[Separator("Enemy hits")]
	public AbilityModPropertyInt m_damageAmountMod;
	public AbilityModPropertyInt m_extraDamageIfSingleTargetMod;
	public AbilityModPropertyEffectInfo m_targetEffectMod;
	[Separator("Self Hit")]
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
			AddToken(tokens, m_maxAngleForLaserMod, "MaxAngleForLaser", string.Empty, samuraiDashAndAimedSlash.m_maxAngleForLaser);
			AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, samuraiDashAndAimedSlash.m_laserWidth);
			AddToken(tokens, m_laserRangeMod, "LaserRange", string.Empty, samuraiDashAndAimedSlash.m_laserRange);
			AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, samuraiDashAndAimedSlash.m_maxTargets);
			AddToken(tokens, m_damageAmountMod, "DamageAmount", string.Empty, samuraiDashAndAimedSlash.m_damageAmount);
			AddToken(tokens, m_extraDamageIfSingleTargetMod, "ExtraDamageIfSingleTarget", string.Empty, samuraiDashAndAimedSlash.m_extraDamageIfSingleTarget);
			AddToken_EffectMod(tokens, m_targetEffectMod, "TargetEffect", samuraiDashAndAimedSlash.m_targetEffect);
			AddToken_EffectMod(tokens, m_effectOnSelfMod, "EffectOnSelf", samuraiDashAndAimedSlash.m_effectOnSelf);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		SamuraiDashAndAimedSlash samuraiDashAndAimedSlash = GetTargetAbilityOnAbilityData(abilityData) as SamuraiDashAndAimedSlash;
		bool isValid = samuraiDashAndAimedSlash != null;
		string desc = string.Empty;
		desc += PropDesc(m_maxAngleForLaserMod, "[MaxAngleForLaser]", isValid, isValid ? samuraiDashAndAimedSlash.m_maxAngleForLaser : 0f);
		desc += PropDesc(m_laserWidthMod, "[LaserWidth]", isValid, isValid ? samuraiDashAndAimedSlash.m_laserWidth : 0f);
		desc += PropDesc(m_laserRangeMod, "[LaserRange]", isValid, isValid ? samuraiDashAndAimedSlash.m_laserRange : 0f);
		desc += PropDesc(m_maxTargetsMod, "[MaxTargets]", isValid, isValid ? samuraiDashAndAimedSlash.m_maxTargets : 0);
		desc += PropDesc(m_damageAmountMod, "[DamageAmount]", isValid, isValid ? samuraiDashAndAimedSlash.m_damageAmount : 0);
		desc += PropDesc(m_extraDamageIfSingleTargetMod, "[ExtraDamageIfSingleTarget]", isValid, isValid ? samuraiDashAndAimedSlash.m_extraDamageIfSingleTarget : 0);
		desc += PropDesc(m_targetEffectMod, "[TargetEffect]", isValid, isValid ? samuraiDashAndAimedSlash.m_targetEffect : null);
		return desc + PropDesc(m_effectOnSelfMod, "[EffectOnSelf]", isValid, isValid ? samuraiDashAndAimedSlash.m_effectOnSelf : null);
	}
}
