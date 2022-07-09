// SERVER
// ROGUES
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_BlasterOvercharge : AbilityMod
{
	[Header("-- How many stacks are allowed")]
	public AbilityModPropertyInt m_maxCastCountMod;
	
	// removed in rogues
	[Header("-- How many times extra damage is applied")]
	public AbilityModPropertyInt m_extraDamageCountMod;
	[Header("-- Extra Damage for all attacks except Lurker Mine")]
	public AbilityModPropertyInt m_extraDamageMod;
	// end removed in rogues
	
	[Header("-- Extra Damage for Lurker Mine")]
	public AbilityModPropertyInt m_extraDamageForDelayedLaserMod;
	[Header("-- Extra Damage for multiple stacks")]
	public AbilityModPropertyInt m_extraDamageForMultiCastMod;
	[Header("-- On Cast")]
	public AbilityModPropertyEffectInfo m_effectOnSelfOnCastMod;
	[Header("-- Extra Effects for other abilities")]
	
	// reactor
	public AbilityModPropertyEffectInfo m_extraEffectOnOtherAbilitiesMod;
	public bool m_useExtraEffectActionTypeOverride;
	public List<AbilityData.ActionType> m_extraEffectActionTypesOverride;
	// rogues
	// public AbilityModPropertyEffectInfo m_extraEffectForStretchingConeMod;
	// public AbilityModPropertyEffectInfo m_extraEffectForDashAndBlastMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(BlasterOvercharge);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		BlasterOvercharge blasterOvercharge = targetAbility as BlasterOvercharge;
		if (blasterOvercharge == null)
		{
			return;
		}
		AddToken(tokens, m_maxCastCountMod, "MaxCastCount", string.Empty, blasterOvercharge.m_maxCastCount);
		
		// removed in rogues
		AddToken(tokens, m_extraDamageCountMod, "ExtraDamageCount", string.Empty, blasterOvercharge.m_extraDamageCount);
		AddToken(tokens, m_extraDamageMod, "ExtraDamage", string.Empty, blasterOvercharge.m_extraDamage);
		// end removed in rogues
		
		AddToken(tokens, m_extraDamageForDelayedLaserMod, "ExtraDamageForLurkerMine", string.Empty, blasterOvercharge.m_extraDamageForDelayedLaser);
		AddToken(tokens, m_extraDamageForMultiCastMod, "ExtraDamageForMultiCast", string.Empty, blasterOvercharge.m_extraDamageForMultiCast);
		AddToken_EffectMod(tokens, m_effectOnSelfOnCastMod, "EffectOnSelfOnCast", blasterOvercharge.m_effectOnSelfOnCast);
		
		// reactor
		AddToken_EffectMod(tokens, m_extraEffectOnOtherAbilitiesMod, "ExtraEffectOnOtherAbilities", blasterOvercharge.m_extraEffectOnOtherAbilities);
		// rogues
		// AddToken_EffectMod(tokens, m_extraEffectForStretchingConeMod, "ExtraEffectForStretchingCone", blasterOvercharge.m_extraEffectForStretchingCone);
		// AddToken_EffectMod(tokens, m_extraEffectForDashAndBlastMod, "ExtraEffectForDashAndShoot", blasterOvercharge.m_extraEffectForDashAndBlast);
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)  // , Ability targetAbility in rogues
	{
		// reactor
		BlasterOvercharge blasterOvercharge = GetTargetAbilityOnAbilityData(abilityData) as BlasterOvercharge;
		// rogues
		// BlasterOvercharge blasterOvercharge = targetAbility as BlasterOvercharge;
		
		bool isAbilityPresent = blasterOvercharge != null;
		string desc = string.Empty;
		desc += PropDesc(m_maxCastCountMod, "[MaxCastCount]", isAbilityPresent, isAbilityPresent ? blasterOvercharge.m_maxCastCount : 0);
		
		// removed in rogues
		desc += PropDesc(m_extraDamageCountMod, "[ExtraDamageCount]", isAbilityPresent, isAbilityPresent ? blasterOvercharge.m_extraDamageCount : 0);
		desc += PropDesc(m_extraDamageMod, "[ExtraDamage]", isAbilityPresent, isAbilityPresent ? blasterOvercharge.m_extraDamage : 0);
		// end removed in rogues
		
		desc += PropDesc(m_extraDamageForDelayedLaserMod, "[ExtraDamageForLurkerMine]", isAbilityPresent, isAbilityPresent ? blasterOvercharge.m_extraDamageForDelayedLaser : 0);
		desc += PropDesc(m_extraDamageForMultiCastMod, "[ExtraDamageForMultiCast]", isAbilityPresent, isAbilityPresent ? blasterOvercharge.m_extraDamageForMultiCast : 0);
		desc += PropDesc(m_effectOnSelfOnCastMod, "[EffectOnSelfOnCast]", isAbilityPresent, isAbilityPresent ? blasterOvercharge.m_effectOnSelfOnCast : null);
		
		// reactor
		desc += PropDesc(m_extraEffectOnOtherAbilitiesMod, "[ExtraEffectOnOtherAbilities]", isAbilityPresent, isAbilityPresent ? blasterOvercharge.m_extraEffectOnOtherAbilities : null);
		if (m_useExtraEffectActionTypeOverride && m_extraEffectActionTypesOverride != null)
		{
			desc += "Using override for extra effect target abilities:\n";
			foreach (AbilityData.ActionType actionType in m_extraEffectActionTypesOverride)
			{
				desc = desc + "    " + actionType + "\n";
			}
			desc += "\n";
		}
		// rogues
		// desc += PropDesc(m_extraEffectForStretchingConeMod, "[ExtraEffectForStretchingCone]", isAbilityPresent, isAbilityPresent ? blasterOvercharge.m_extraEffectForStretchingCone : null);
		// desc += PropDesc(m_extraEffectForDashAndBlastMod, "[ExtraEffectForDashAndShoot]", isAbilityPresent, isAbilityPresent ? blasterOvercharge.m_extraEffectForDashAndBlast : null);
		
		return desc;
	}
}
