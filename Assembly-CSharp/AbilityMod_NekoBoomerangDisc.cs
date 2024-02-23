using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class AbilityMod_NekoBoomerangDisc : AbilityMod
{
	[Separator("Targeting")]
	public AbilityModPropertyFloat m_laserLengthMod;
	public AbilityModPropertyFloat m_laserWidthMod;
	public AbilityModPropertyFloat m_aoeRadiusAtEndMod;
	public AbilityModPropertyInt m_maxTargetsMod;
	[Header("-- Disc return end radius")]
	public AbilityModPropertyFloat m_discReturnEndRadiusMod;
	[Separator("Damage stuff")]
	public AbilityModPropertyInt m_directDamageMod;
	public AbilityModPropertyInt m_returnTripDamageMod;
	public AbilityModPropertyBool m_returnTripIgnoreCoverMod;
	[Header("-- Extra Damage")]
	public AbilityModPropertyInt m_extraDamageIfHitByReturnDiscMod;
	public AbilityModPropertyInt m_extraReturnDamageIfHitNoOneMod;
	[Separator("-- Shielding for target hit on throw (applied on start of next turn)")]
	public AbilityModPropertyInt m_shieldPerTargetHitOnThrowMod;
	public AbilityModPropertyEffectData m_shieldEffectDataMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(NekoBoomerangDisc);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		NekoBoomerangDisc nekoBoomerangDisc = targetAbility as NekoBoomerangDisc;
		if (nekoBoomerangDisc != null)
		{
			AddToken(tokens, m_laserLengthMod, "LaserLength", string.Empty, nekoBoomerangDisc.m_laserLength);
			AddToken(tokens, m_laserWidthMod, "LaserWidth", string.Empty, nekoBoomerangDisc.m_laserWidth);
			AddToken(tokens, m_aoeRadiusAtEndMod, "AoeRadiusAtEnd", string.Empty, nekoBoomerangDisc.m_aoeRadiusAtEnd);
			AddToken(tokens, m_maxTargetsMod, "MaxTargets", string.Empty, nekoBoomerangDisc.m_maxTargets);
			AddToken(tokens, m_discReturnEndRadiusMod, "DiscReturnEndRadius", string.Empty, nekoBoomerangDisc.m_discReturnEndRadius);
			AddToken(tokens, m_directDamageMod, "DirectDamage", string.Empty, nekoBoomerangDisc.m_directDamage);
			AddToken(tokens, m_returnTripDamageMod, "ReturnTripDamage", string.Empty, nekoBoomerangDisc.m_returnTripDamage);
			AddToken(tokens, m_extraDamageIfHitByReturnDiscMod, "ExtraDamageIfHitByReturnDisc", string.Empty, nekoBoomerangDisc.m_extraDamageIfHitByReturnDisc);
			AddToken(tokens, m_extraReturnDamageIfHitNoOneMod, "ExtraReturnDamageIfHitNoOne", string.Empty, nekoBoomerangDisc.m_extraReturnDamageIfHitNoOne);
			AddToken(tokens, m_shieldPerTargetHitOnThrowMod, "ShieldPerTargetHitOnThrow", string.Empty, nekoBoomerangDisc.m_shieldPerTargetHitOnThrow);
			AddToken_EffectMod(tokens, m_shieldEffectDataMod, "ShieldEffectData", nekoBoomerangDisc.m_shieldEffectData);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		NekoBoomerangDisc nekoBoomerangDisc = GetTargetAbilityOnAbilityData(abilityData) as NekoBoomerangDisc;
		bool isValid = nekoBoomerangDisc != null;
		string desc = string.Empty;
		desc += PropDesc(m_laserLengthMod, "[LaserLength]", isValid, isValid ? nekoBoomerangDisc.m_laserLength : 0f);
		desc += PropDesc(m_laserWidthMod, "[LaserWidth]", isValid, isValid ? nekoBoomerangDisc.m_laserWidth : 0f);
		desc += PropDesc(m_aoeRadiusAtEndMod, "[AoeRadiusAtEnd]", isValid, isValid ? nekoBoomerangDisc.m_aoeRadiusAtEnd : 0f);
		desc += PropDesc(m_maxTargetsMod, "[MaxTargets]", isValid, isValid ? nekoBoomerangDisc.m_maxTargets : 0);
		desc += PropDesc(m_discReturnEndRadiusMod, "[DiscReturnEndRadius]", isValid, isValid ? nekoBoomerangDisc.m_discReturnEndRadius : 0f);
		desc += PropDesc(m_directDamageMod, "[DirectDamage]", isValid, isValid ? nekoBoomerangDisc.m_directDamage : 0);
		desc += PropDesc(m_returnTripDamageMod, "[ReturnTripDamage]", isValid, isValid ? nekoBoomerangDisc.m_returnTripDamage : 0);
		desc += PropDesc(m_returnTripIgnoreCoverMod, "[ReturnTripIgnoreCover]", isValid, isValid && nekoBoomerangDisc.m_returnTripIgnoreCover);
		desc += PropDesc(m_extraDamageIfHitByReturnDiscMod, "[ExtraDamageIfHitByReturnDisc]", isValid, isValid ? nekoBoomerangDisc.m_extraDamageIfHitByReturnDisc : 0);
		desc += PropDesc(m_extraReturnDamageIfHitNoOneMod, "[ExtraReturnDamageIfHitNoOne]", isValid, isValid ? nekoBoomerangDisc.m_extraReturnDamageIfHitNoOne : 0);
		desc += PropDesc(m_shieldPerTargetHitOnThrowMod, "[ShieldPerTargetHitOnThrow]", isValid, isValid ? nekoBoomerangDisc.m_shieldPerTargetHitOnThrow : 0);
		return new StringBuilder().Append(desc).Append(PropDesc(m_shieldEffectDataMod, "[ShieldEffectData]", isValid, isValid ? nekoBoomerangDisc.m_shieldEffectData : null)).ToString();
	}
}
