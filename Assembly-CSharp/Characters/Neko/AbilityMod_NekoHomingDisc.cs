using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_NekoHomingDisc : AbilityMod
{
	[Separator("Targeting")]
	public AbilityModPropertyFloat m_laserLengthMod;
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyInt m_maxTargetsMod;
	[Header("-- Disc return end radius")]
	public AbilityModPropertyFloat m_discReturnEndRadiusMod;
	[Separator("On Cast Hit")]
	public AbilityModPropertyEffectInfo m_onCastEnemyHitEffectMod;
	[Separator("On Enemy Hit")]
	public AbilityModPropertyInt m_targetDamageMod;
	public AbilityModPropertyInt m_returnTripDamageMod;
	public AbilityModPropertyBool m_returnTripIgnoreCoverMod;
	public AbilityModPropertyFloat m_extraReturnDamagePerDistMod;
	public AbilityModPropertyEffectInfo m_returnTripEnemyEffectMod;
	[Separator("Cooldown Reduction")]
	public AbilityModPropertyInt m_cdrIfHitNoOneOnCastMod;
	public AbilityModPropertyInt m_cdrIfHitNoOneOnReturnMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NekoHomingDisc);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NekoHomingDisc nekoHomingDisc = targetAbility as NekoHomingDisc;
		if (nekoHomingDisc != null)
		{
			AddToken(tokens, m_laserLengthMod, "LaserLength", string.Empty, nekoHomingDisc.m_laserLength);
			AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, nekoHomingDisc.m_laserWidth);
			AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, nekoHomingDisc.m_maxTargets);
			AddToken(tokens, m_discReturnEndRadiusMod, "DiscReturnEndRadius", string.Empty, nekoHomingDisc.m_discReturnEndRadius);
			AddToken_EffectMod(tokens, m_onCastEnemyHitEffectMod, "OnCastEnemyHitEffect", nekoHomingDisc.m_onCastEnemyHitEffect);
			AddToken(tokens, m_targetDamageMod, "TargetDamage", string.Empty, nekoHomingDisc.m_targetDamage);
			AddToken(tokens, m_returnTripDamageMod, "ReturnTripDamage", string.Empty, nekoHomingDisc.m_returnTripDamage);
			AddToken(tokens, m_extraReturnDamagePerDistMod, "ExtraReturnDamagePerDist", string.Empty, nekoHomingDisc.m_extraReturnDamagePerDist);
			AddToken_EffectMod(tokens, m_returnTripEnemyEffectMod, "ReturnTripEnemyEffect", nekoHomingDisc.m_returnTripEnemyEffect);
			AddToken(tokens, m_cdrIfHitNoOneOnCastMod, "CdrIfHitNoOneOnCast", string.Empty, nekoHomingDisc.m_cdrIfHitNoOneOnCast);
			AddToken(tokens, m_cdrIfHitNoOneOnReturnMod, "CdrIfHitNoOneOnReturn", string.Empty, nekoHomingDisc.m_cdrIfHitNoOneOnReturn);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NekoHomingDisc nekoHomingDisc = GetTargetAbilityOnAbilityData(abilityData) as NekoHomingDisc;
		bool isValid = nekoHomingDisc != null;
		string desc = string.Empty;
		desc += PropDesc(m_laserLengthMod, "[LaserLength]", isValid, isValid ? nekoHomingDisc.m_laserLength : 0f);
		desc += PropDesc(m_laserWidthMod, "[LaserWidth]", isValid, isValid ? nekoHomingDisc.m_laserWidth : 0f);
		desc += PropDesc(m_maxTargetsMod, "[MaxTargets]", isValid, isValid ? nekoHomingDisc.m_maxTargets : 0);
		desc += PropDesc(m_discReturnEndRadiusMod, "[DiscReturnEndRadius]", isValid, isValid ? nekoHomingDisc.m_discReturnEndRadius : 0f);
		desc += PropDesc(m_onCastEnemyHitEffectMod, "[OnCastEnemyHitEffect]", isValid, isValid ? nekoHomingDisc.m_onCastEnemyHitEffect : null);
		desc += PropDesc(m_targetDamageMod, "[TargetDamage]", isValid, isValid ? nekoHomingDisc.m_targetDamage : 0);
		desc += PropDesc(m_returnTripDamageMod, "[ReturnTripDamage]", isValid, isValid ? nekoHomingDisc.m_returnTripDamage : 0);
		desc += PropDesc(m_returnTripIgnoreCoverMod, "[ReturnTripIgnoreCover]", isValid, isValid && nekoHomingDisc.m_returnTripIgnoreCover);
		desc += PropDesc(m_extraReturnDamagePerDistMod, "[ExtraReturnDamagePerDist]", isValid, isValid ? nekoHomingDisc.m_extraReturnDamagePerDist : 0f);
		desc += PropDesc(m_returnTripEnemyEffectMod, "[ReturnTripEnemyEffect]", isValid, isValid ? nekoHomingDisc.m_returnTripEnemyEffect : null);
		desc += PropDesc(m_cdrIfHitNoOneOnCastMod, "[CdrIfHitNoOneOnCast]", isValid, isValid ? nekoHomingDisc.m_cdrIfHitNoOneOnCast : 0);
		return desc + PropDesc(m_cdrIfHitNoOneOnReturnMod, "[CdrIfHitNoOneOnReturn]", isValid, isValid ? nekoHomingDisc.m_cdrIfHitNoOneOnReturn : 0);
	}
}
