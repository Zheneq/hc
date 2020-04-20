using System;
using System.Collections.Generic;
using UnityEngine;

public class AbilityMod_DinoMarkedAreaAttack : GenericAbility_AbilityMod
{
	[Separator("Target Select Mod", true)]
	public TargetSelectMod_AoeRadius m_targetSelectMod;

	[Separator("For Delayed Hit", true)]
	public AbilityModPropertyInt m_delayTurnsMod;

	public AbilityModPropertyShape m_shapeMod;

	public AbilityModPropertyBool m_delayedHitIgnoreLosMod;

	[Space(10f)]
	public AbilityModPropertyInt m_extraDamageForSingleMarkMod;

	public AbilityModPropertyInt m_energyToAllyOnDamageHitMod;

	public override Type GetTargetAbilityType()
	{
		return typeof(DinoMarkedAreaAttack);
	}

	public override void GenModImpl_SetTargetSelectMod(GenericAbility_TargetSelectBase targetSelect)
	{
		targetSelect.SetTargetSelectMod(this.m_targetSelectMod);
	}

	protected override void AddModSpecificTooltipTokens(List<TooltipTokenEntry> tokens, Ability targetAbility)
	{
		DinoMarkedAreaAttack dinoMarkedAreaAttack = targetAbility as DinoMarkedAreaAttack;
		if (dinoMarkedAreaAttack != null)
		{
			base.AddModSpecificTooltipTokens(tokens, targetAbility);
			AbilityMod.AddToken(tokens, this.m_delayTurnsMod, "DelayTurns", string.Empty, dinoMarkedAreaAttack.m_delayTurns, true, false);
			AbilityMod.AddToken(tokens, this.m_extraDamageForSingleMarkMod, "ExtraDamageForSingleMark", string.Empty, dinoMarkedAreaAttack.m_extraDamageForSingleMark, true, false);
			AbilityMod.AddToken(tokens, this.m_energyToAllyOnDamageHitMod, "EnergyToAllyOnDamageHit", string.Empty, dinoMarkedAreaAttack.m_energyToAllyOnDamageHit, true, false);
		}
	}

	protected override string ModSpecificAutogenDesc(AbilityData abilityData)
	{
		DinoMarkedAreaAttack dinoMarkedAreaAttack = base.GetTargetAbilityOnAbilityData(abilityData) as DinoMarkedAreaAttack;
		bool flag = dinoMarkedAreaAttack != null;
		string text = base.ModSpecificAutogenDesc(abilityData);
		if (dinoMarkedAreaAttack != null)
		{
			text += base.GetTargetSelectModDesc(this.m_targetSelectMod, dinoMarkedAreaAttack.m_targetSelectComp, "-- Target Select Mod --");
		}
		string str = text;
		AbilityModPropertyInt delayTurnsMod = this.m_delayTurnsMod;
		string prefix = "[DelayTurns]";
		bool showBaseVal = flag;
		int baseVal;
		if (flag)
		{
			baseVal = dinoMarkedAreaAttack.m_delayTurns;
		}
		else
		{
			baseVal = 0;
		}
		text = str + base.PropDesc(delayTurnsMod, prefix, showBaseVal, baseVal);
		string str2 = text;
		AbilityModPropertyShape shapeMod = this.m_shapeMod;
		string prefix2 = "[Shape]";
		bool showBaseVal2 = flag;
		AbilityAreaShape baseVal2;
		if (flag)
		{
			baseVal2 = dinoMarkedAreaAttack.m_shape;
		}
		else
		{
			baseVal2 = AbilityAreaShape.SingleSquare;
		}
		text = str2 + base.PropDesc(shapeMod, prefix2, showBaseVal2, baseVal2);
		string str3 = text;
		AbilityModPropertyBool delayedHitIgnoreLosMod = this.m_delayedHitIgnoreLosMod;
		string prefix3 = "[DelayedHitIgnoreLos]";
		bool showBaseVal3 = flag;
		bool baseVal3;
		if (flag)
		{
			baseVal3 = dinoMarkedAreaAttack.m_delayedHitIgnoreLos;
		}
		else
		{
			baseVal3 = false;
		}
		text = str3 + base.PropDesc(delayedHitIgnoreLosMod, prefix3, showBaseVal3, baseVal3);
		string str4 = text;
		AbilityModPropertyInt extraDamageForSingleMarkMod = this.m_extraDamageForSingleMarkMod;
		string prefix4 = "[ExtraDamageForSingleMark]";
		bool showBaseVal4 = flag;
		int baseVal4;
		if (flag)
		{
			baseVal4 = dinoMarkedAreaAttack.m_extraDamageForSingleMark;
		}
		else
		{
			baseVal4 = 0;
		}
		text = str4 + base.PropDesc(extraDamageForSingleMarkMod, prefix4, showBaseVal4, baseVal4);
		string str5 = text;
		AbilityModPropertyInt energyToAllyOnDamageHitMod = this.m_energyToAllyOnDamageHitMod;
		string prefix5 = "[EnergyToAllyOnDamageHit]";
		bool showBaseVal5 = flag;
		int baseVal5;
		if (flag)
		{
			baseVal5 = dinoMarkedAreaAttack.m_energyToAllyOnDamageHit;
		}
		else
		{
			baseVal5 = 0;
		}
		return str5 + base.PropDesc(energyToAllyOnDamageHitMod, prefix5, showBaseVal5, baseVal5);
	}
}
