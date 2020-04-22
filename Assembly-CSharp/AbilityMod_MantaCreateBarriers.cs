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
		if (!(mantaCreateBarriers != null))
		{
			return;
		}
		while (true)
		{
			AbilityMod.AddToken(tokens, m_prisonSidesMod, "PrisonSides", string.Empty, mantaCreateBarriers.m_prisonSides);
			AbilityMod.AddToken(tokens, m_prisonRadiusMod, "PrisonRadius", string.Empty, mantaCreateBarriers.m_prisonRadius);
			AbilityMod.AddToken_BarrierMod(tokens, m_prisonBarrierDataMod, "PrisonBarrierData", mantaCreateBarriers.m_prisonBarrierData);
			AbilityMod.AddToken(tokens, m_damageOnCastMod, "DamageOnCast", string.Empty, mantaCreateBarriers.m_damageOnCast);
			AbilityMod.AddToken(tokens, m_allyHealOnCastMod, "AllyHealOnCast", string.Empty, mantaCreateBarriers.m_allyHealOnCast);
			AbilityMod.AddToken_EffectInfo(tokens, m_effectOnAlliesOnCastMod.effectInfo, "EffectOnAlliesOnCast", mantaCreateBarriers.m_effectOnAlliesOnCast);
			return;
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MantaCreateBarriers mantaCreateBarriers = GetTargetAbilityOnAbilityData(abilityData) as MantaCreateBarriers;
		bool flag = mantaCreateBarriers != null;
		string empty = string.Empty;
		string str = empty;
		AbilityModPropertyBool requireCasterInShapeMod = m_requireCasterInShapeMod;
		int baseVal;
		if (flag)
		{
			baseVal = (mantaCreateBarriers.m_requireCasterInShape ? 1 : 0);
		}
		else
		{
			baseVal = 0;
		}
		empty = str + PropDesc(requireCasterInShapeMod, "[RequireCasterInShape]", flag, (byte)baseVal != 0);
		empty += PropDesc(m_targetAreaShapeMod, "[TargetAreaShape]", flag, flag ? mantaCreateBarriers.m_targetAreaShape : AbilityAreaShape.SingleSquare);
		empty += PropDesc(m_delayBarriersUntilStartOfNextTurnMod, "[DelayBarriersUntilStartOfNextTurn]", flag, flag && mantaCreateBarriers.m_delayBarriersUntilStartOfNextTurn);
		string str2 = empty;
		AbilityModPropertyInt prisonSidesMod = m_prisonSidesMod;
		int baseVal2;
		if (flag)
		{
			baseVal2 = mantaCreateBarriers.m_prisonSides;
		}
		else
		{
			baseVal2 = 0;
		}
		empty = str2 + PropDesc(prisonSidesMod, "[PrisonSides]", flag, baseVal2);
		string str3 = empty;
		AbilityModPropertyFloat prisonRadiusMod = m_prisonRadiusMod;
		float baseVal3;
		if (flag)
		{
			baseVal3 = mantaCreateBarriers.m_prisonRadius;
		}
		else
		{
			baseVal3 = 0f;
		}
		empty = str3 + PropDesc(prisonRadiusMod, "[PrisonRadius]", flag, baseVal3);
		empty += PropDescBarrierMod(m_prisonBarrierDataMod, "{ PrisonBarrierData }", mantaCreateBarriers.m_prisonBarrierData);
		string str4 = empty;
		AbilityModPropertyShape shapeForTargeterMod = m_shapeForTargeterMod;
		int baseVal4;
		if (flag)
		{
			baseVal4 = (int)mantaCreateBarriers.m_shapeForTargeter;
		}
		else
		{
			baseVal4 = 0;
		}
		empty = str4 + PropDesc(shapeForTargeterMod, "[ShapeForTargeter]", flag, (AbilityAreaShape)baseVal4);
		string str5 = empty;
		AbilityModPropertyBool createBarriersImmediatelyMod = m_createBarriersImmediatelyMod;
		int baseVal5;
		if (flag)
		{
			baseVal5 = (mantaCreateBarriers.m_createBarriersImmediately ? 1 : 0);
		}
		else
		{
			baseVal5 = 0;
		}
		empty = str5 + PropDesc(createBarriersImmediatelyMod, "[CreateBarriersImmediately]", flag, (byte)baseVal5 != 0);
		string str6 = empty;
		AbilityModPropertyInt damageOnCastMod = m_damageOnCastMod;
		int baseVal6;
		if (flag)
		{
			baseVal6 = mantaCreateBarriers.m_damageOnCast;
		}
		else
		{
			baseVal6 = 0;
		}
		empty = str6 + PropDesc(damageOnCastMod, "[DamageOnCast]", flag, baseVal6);
		empty += PropDesc(m_allyHealOnCastMod, "[AllyHealOnCast]", flag, flag ? mantaCreateBarriers.m_allyHealOnCast : 0);
		string str7 = empty;
		AbilityModPropertyEffectInfo effectOnAlliesOnCastMod = m_effectOnAlliesOnCastMod;
		object baseVal7;
		if (flag)
		{
			baseVal7 = mantaCreateBarriers.m_effectOnAlliesOnCast;
		}
		else
		{
			baseVal7 = null;
		}
		empty = str7 + PropDesc(effectOnAlliesOnCastMod, "[EffectOnAlliesOnCast]", flag, (StandardEffectInfo)baseVal7);
		return empty + PropDesc(m_addVisionProviderInsideBarriers, "[AddVisionProviderInsideBarriers]", flag);
	}
}
