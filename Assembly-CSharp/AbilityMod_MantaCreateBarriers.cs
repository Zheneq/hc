// ROGUES
// SERVER
using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_MantaCreateBarriers : AbilityMod
{
	[Header("-- Whether require Manta to be inside target area --")]
	public AbilityModPropertyBool m_requireCasterInShapeMod;
	public AbilityModPropertyShape m_targetAreaShapeMod;
	[Header("-- Barriers")]
	public AbilityModPropertyBool m_delayBarriersUntilStartOfNextTurnMod;
	public AbilityModPropertyInt m_prisonSidesMod;
	public AbilityModPropertyFloat m_prisonRadiusMod;
	public AbilityModPropertyBarrierDataV2 m_prisonBarrierDataMod;
	public AbilityModPropertyShape m_shapeForTargeterMod;
	public AbilityModPropertyBool m_createBarriersImmediatelyMod;
	[Header("-- Ground effect")]
	public StandardGroundEffectInfo m_groundEffectInfoMod;
	public AbilityModPropertyInt m_damageOnCastMod;
	[Header("-- On Cast Ally Hit (includes caster as well)")]
	public AbilityModPropertyInt m_allyHealOnCastMod;
	public AbilityModPropertyEffectInfo m_effectOnAlliesOnCastMod;
	[Space(10f)]
	public AbilityModPropertyBool m_addVisionProviderInsideBarriers;

	public override Type GetTargetAbilityType()
	{
		return typeof(MantaCreateBarriers);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		MantaCreateBarriers mantaCreateBarriers = targetAbility as MantaCreateBarriers;
		if (mantaCreateBarriers != null)
		{
			AddToken(tokens, m_prisonSidesMod, "PrisonSides", string.Empty, mantaCreateBarriers.m_prisonSides);
			AddToken(tokens, m_prisonRadiusMod, "PrisonRadius", string.Empty, mantaCreateBarriers.m_prisonRadius);
			AddToken_BarrierMod(tokens, m_prisonBarrierDataMod, "PrisonBarrierData", mantaCreateBarriers.m_prisonBarrierData);
			AddToken(tokens, m_damageOnCastMod, "DamageOnCast", string.Empty, mantaCreateBarriers.m_damageOnCast);
			AddToken(tokens, m_allyHealOnCastMod, "AllyHealOnCast", string.Empty, mantaCreateBarriers.m_allyHealOnCast);
			AddToken_EffectInfo(tokens, m_effectOnAlliesOnCastMod.effectInfo, "EffectOnAlliesOnCast", mantaCreateBarriers.m_effectOnAlliesOnCast);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData) // , Ability targetAbility in rogues
	{
		// reactor
		MantaCreateBarriers mantaCreateBarriers = GetTargetAbilityOnAbilityData(abilityData) as MantaCreateBarriers;
		// rogues
		// MantaCreateBarriers mantaCreateBarriers = targetAbility as MantaCreateBarriers;
		
		bool isValid = mantaCreateBarriers != null;
		string desc = string.Empty;
		desc += PropDesc(m_requireCasterInShapeMod, "[RequireCasterInShape]", isValid, isValid && mantaCreateBarriers.m_requireCasterInShape);
		desc += PropDesc(m_targetAreaShapeMod, "[TargetAreaShape]", isValid, isValid ? mantaCreateBarriers.m_targetAreaShape : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_delayBarriersUntilStartOfNextTurnMod, "[DelayBarriersUntilStartOfNextTurn]", isValid, isValid && mantaCreateBarriers.m_delayBarriersUntilStartOfNextTurn);
		desc += PropDesc(m_prisonSidesMod, "[PrisonSides]", isValid, isValid ? mantaCreateBarriers.m_prisonSides : 0);
		desc += PropDesc(m_prisonRadiusMod, "[PrisonRadius]", isValid, isValid ? mantaCreateBarriers.m_prisonRadius : 0f);
		desc += PropDescBarrierMod(m_prisonBarrierDataMod, "{ PrisonBarrierData }", mantaCreateBarriers.m_prisonBarrierData);
		desc += PropDesc(m_shapeForTargeterMod, "[ShapeForTargeter]", isValid, isValid ? mantaCreateBarriers.m_shapeForTargeter : AbilityAreaShape.SingleSquare);
		desc += PropDesc(m_createBarriersImmediatelyMod, "[CreateBarriersImmediately]", isValid, isValid && mantaCreateBarriers.m_createBarriersImmediately);
		desc += PropDesc(m_damageOnCastMod, "[DamageOnCast]", isValid, isValid ? mantaCreateBarriers.m_damageOnCast : 0);
		desc += PropDesc(m_allyHealOnCastMod, "[AllyHealOnCast]", isValid, isValid ? mantaCreateBarriers.m_allyHealOnCast : 0);
		desc += PropDesc(m_effectOnAlliesOnCastMod, "[EffectOnAlliesOnCast]", isValid, isValid ? mantaCreateBarriers.m_effectOnAlliesOnCast : null);
		return desc + PropDesc(m_addVisionProviderInsideBarriers, "[AddVisionProviderInsideBarriers]", isValid);
	}
}
