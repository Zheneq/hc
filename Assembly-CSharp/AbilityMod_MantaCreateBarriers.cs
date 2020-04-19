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
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_MantaCreateBarriers.AddModSpecificTooltipTokens(List<TooltipTokenEntry>, Ability)).MethodHandle;
			}
			AbilityMod.AddToken(tokens, this.m_prisonSidesMod, "PrisonSides", string.Empty, mantaCreateBarriers.m_prisonSides, true, false);
			AbilityMod.AddToken(tokens, this.m_prisonRadiusMod, "PrisonRadius", string.Empty, mantaCreateBarriers.m_prisonRadius, true, false, false);
			AbilityMod.AddToken_BarrierMod(tokens, this.m_prisonBarrierDataMod, "PrisonBarrierData", mantaCreateBarriers.m_prisonBarrierData);
			AbilityMod.AddToken(tokens, this.m_damageOnCastMod, "DamageOnCast", string.Empty, mantaCreateBarriers.m_damageOnCast, true, false);
			AbilityMod.AddToken(tokens, this.m_allyHealOnCastMod, "AllyHealOnCast", string.Empty, mantaCreateBarriers.m_allyHealOnCast, true, false);
			AbilityMod.AddToken_EffectInfo(tokens, this.m_effectOnAlliesOnCastMod.effectInfo, "EffectOnAlliesOnCast", mantaCreateBarriers.m_effectOnAlliesOnCast, true);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		MantaCreateBarriers mantaCreateBarriers = base.GetTargetAbilityOnAbilityData(abilityData) as MantaCreateBarriers;
		bool flag = mantaCreateBarriers != null;
		string text = string.Empty;
		string str = text;
		AbilityModPropertyBool requireCasterInShapeMod = this.m_requireCasterInShapeMod;
		string prefix = "[RequireCasterInShape]";
		bool showBaseVal = flag;
		bool baseVal;
		if (flag)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			if (!true)
			{
				RuntimeMethodHandle runtimeMethodHandle = methodof(AbilityMod_MantaCreateBarriers.ModSpecificAutogenDesc(AbilityData)).MethodHandle;
			}
			baseVal = mantaCreateBarriers.m_requireCasterInShape;
		}
		else
		{
			baseVal = false;
		}
		text = str + base.PropDesc(requireCasterInShapeMod, prefix, showBaseVal, baseVal);
		text += base.PropDesc(this.m_targetAreaShapeMod, "[TargetAreaShape]", flag, (!flag) ? AbilityAreaShape.SingleSquare : mantaCreateBarriers.m_targetAreaShape);
		text += base.PropDesc(this.m_delayBarriersUntilStartOfNextTurnMod, "[DelayBarriersUntilStartOfNextTurn]", flag, flag && mantaCreateBarriers.m_delayBarriersUntilStartOfNextTurn);
		string str2 = text;
		AbilityModPropertyInt prisonSidesMod = this.m_prisonSidesMod;
		string prefix2 = "[PrisonSides]";
		bool showBaseVal2 = flag;
		int baseVal2;
		if (flag)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal2 = mantaCreateBarriers.m_prisonSides;
		}
		else
		{
			baseVal2 = 0;
		}
		text = str2 + base.PropDesc(prisonSidesMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyFloat prisonRadiusMod = this.m_prisonRadiusMod;
		string prefix3 = "[PrisonRadius]";
		bool showBaseVal3 = flag;
		float baseVal3;
		if (flag)
		{
			for (;;)
			{
				switch (3)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal3 = mantaCreateBarriers.m_prisonRadius;
		}
		else
		{
			baseVal3 = 0f;
		}
		text = str3 + base.PropDesc(prisonRadiusMod, prefix3, showBaseVal3, baseVal3);
		text += base.PropDescBarrierMod(this.m_prisonBarrierDataMod, "{ PrisonBarrierData }", mantaCreateBarriers.m_prisonBarrierData);
		string str4 = text;
		AbilityModPropertyShape shapeForTargeterMod = this.m_shapeForTargeterMod;
		string prefix4 = "[ShapeForTargeter]";
		bool showBaseVal4 = flag;
		AbilityAreaShape baseVal4;
		if (flag)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal4 = mantaCreateBarriers.m_shapeForTargeter;
		}
		else
		{
			baseVal4 = AbilityAreaShape.SingleSquare;
		}
		text = str4 + base.PropDesc(shapeForTargeterMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyBool createBarriersImmediatelyMod = this.m_createBarriersImmediatelyMod;
		string prefix5 = "[CreateBarriersImmediately]";
		bool showBaseVal5 = flag;
		bool baseVal5;
		if (flag)
		{
			for (;;)
			{
				switch (2)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal5 = mantaCreateBarriers.m_createBarriersImmediately;
		}
		else
		{
			baseVal5 = false;
		}
		text = str5 + base.PropDesc(createBarriersImmediatelyMod, prefix5, showBaseVal5, baseVal5);
		string str6 = text;
		AbilityModPropertyInt damageOnCastMod = this.m_damageOnCastMod;
		string prefix6 = "[DamageOnCast]";
		bool showBaseVal6 = flag;
		int baseVal6;
		if (flag)
		{
			for (;;)
			{
				switch (6)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal6 = mantaCreateBarriers.m_damageOnCast;
		}
		else
		{
			baseVal6 = 0;
		}
		text = str6 + base.PropDesc(damageOnCastMod, prefix6, showBaseVal6, baseVal6);
		text += base.PropDesc(this.m_allyHealOnCastMod, "[AllyHealOnCast]", flag, (!flag) ? 0 : mantaCreateBarriers.m_allyHealOnCast);
		string str7 = text;
		AbilityModPropertyEffectInfo effectOnAlliesOnCastMod = this.m_effectOnAlliesOnCastMod;
		string prefix7 = "[EffectOnAlliesOnCast]";
		bool showBaseVal7 = flag;
		StandardEffectInfo baseVal7;
		if (flag)
		{
			for (;;)
			{
				switch (7)
				{
				case 0:
					continue;
				}
				break;
			}
			baseVal7 = mantaCreateBarriers.m_effectOnAlliesOnCast;
		}
		else
		{
			baseVal7 = null;
		}
		text = str7 + base.PropDesc(effectOnAlliesOnCastMod, prefix7, showBaseVal7, baseVal7);
		return text + base.PropDesc(this.m_addVisionProviderInsideBarriers, "[AddVisionProviderInsideBarriers]", flag, false);
	}
}
